Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsUpDown
Imports CraftBand.Tables.dstDataTables

Partial Public Class clsCalcHexagon
    '                       
    '                  バンド方向                                                                    
    '    位置順     ↗  120度                                          バンド方向  ＼　位置順
    '   (30度方向)／  　 ＼      ＼      ＼   　 ／　　　／　　　／       60度 　　 ↘(-30度方向)
    '                      ＼  ──+──+─+──+─+──+──　／
    '                       ＼　　　＼／　　＼／　　＼／　　　／斜め60度　
    '                       　＼　　／＼　　／＼　　／＼　　／　　　       
    '                        ─+──+─+──+─+──+─+──横                                  
    '                       　　＼／　　＼／　　＼／　　＼／   ↑六つ目(ひも間)   バンド方向 ↓　位置順
    '                       　　／＼　　／＼　　／＼　　／＼   ↓                    0度     ↓(-90度方向)
    '                        ─+──+─+──+─+──+─+──横                                  
    '                       　／　　＼／　　＼／　　＼／　　＼　　　　
    '                       ／　　　／＼　　／＼　　／＼　　　＼斜め120度　　　
    '                    ／　  ──+──+─+──+─+──+──　　＼
    '                                                                       
    '
    '         



    'バンドの方向角
    Shared cBandAngleDegree() As Double = {0, 60, 120}

    'バンドの方向
    Shared cDeltaBandDirection() As S差分 = {New S差分(0), New S差分(60), New S差分(120)}
    'バンドの軸方向
    Shared cDeltaAxisDirection() As S差分 = {New S差分(90), New S差分(150), New S差分(210)}


    'バンド位置の計算結果
    Dim _BandPositions(cAngleCount - 1) As CBandPositionList
    '                                           i_1st         i_2nd
    '            (3)　　　　　　　　 線の角度　 最初の線　　　最後の線       軸方向
    '         p34 -- p23                          側面外方向   　側面外方向
    '   (4)  ／        ↙＼  (2)         0度     (0) 270度      (3)  90度       90度   (ひも逆順)      
    '      p45          p12            60度     (1) 330度      (4) 150度      150度   (ひも逆順)
    '   (5)  ＼   ↑ 　↖／  (1)       120度     (2)  60度      (5) 210度      210度   (ひも逆順)
    '         p50 -- p01           cBandAngleDegree                         cDeltaAxisDirection
    '            (0)
    '




    'プレビュー時に生成,描画後はNothing
    Dim _ImageList描画要素 As clsImageItemList '底と側面

    Dim _hex底の辺 As CHex
    Dim _hex厚さプラス底の辺 As CHex
    Dim _hex側面上辺 As CHex
    Dim _hex縁の辺 As CHex


    Private Sub NewImageData()
        _BandPositions(cIdxAngle0) = New CBandPositionList(AngleIndex._0deg)
        _BandPositions(cIdxAngle60) = New CBandPositionList(AngleIndex._60deg)
        _BandPositions(cIdxAngle120) = New CBandPositionList(AngleIndex._120deg)

    End Sub

    Private Sub ClearImageData()

    End Sub

    Private Function ToStringImageData() As String
        Dim sb As New System.Text.StringBuilder

        Return sb.ToString
    End Function


#Region "位置と長さ計算結果"

    Private Function get六角ベース_横() As Double
        Return Max(_BandPositions(cIdxAngle60)._底領域幅, _BandPositions(cIdxAngle120)._底領域幅) *
            SIN60
    End Function

    Private Function get六角ベース_縦() As Double
        Return _BandPositions(cIdxAngle0)._底領域幅
    End Function

    '底の配置領域に立ち上げ増分をプラス, = p_d四角ベース_周
    Private Function get側面の周長() As Double
        Return 2 * (get六角ベース_横() + get六角ベース_縦()) _
            + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分")
    End Function

    '横ひも長計算・描画に使用
    Private Function get周の横(Optional ByVal multi As Double = 1) As Double
        Return multi *
            (get六角ベース_横() + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4)
    End Function

    '縦ひも長計算・描画に使用
    Private Function get周の縦(Optional ByVal multi As Double = 1) As Double
        Return multi *
            (get六角ベース_縦() + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4)
    End Function

    '側面の高さに対応したひも長、計算・描画に使用   '縁は含まない
    Private Function get側面ひも長(ByVal count As Integer) As Double
        Return count * (_d六角ベース_高さ計 / SIN60)
    End Function

    Private Function get本幅変更あり(ByVal aidx As AngleIndex) As Boolean
        Return _BandPositions(idx(aidx))._本幅変更あり
    End Function


#End Region





    '配置数,展開各入力値(ひも長加算,ひも幅)がFixした状態で、長さを計算する
    Private Function calc_位置と長さ計算(ByVal is位置計算 As Boolean) As Boolean
        Dim ret As Boolean = True

        If is位置計算 Then

            For Each aidx As AngleIndex In enumExeName.GetValues(GetType(AngleIndex))
                ret = ret And _BandPositions(idx(aidx)).SetTable(p_tbl縦横展開(aidx), _iひもの本数(idx(aidx)), _I基本のひも幅)
                ret = ret And _BandPositions(idx(aidx)).CalcBasicPositions(_d六つ目の高さ, _d端の目(idx(aidx)), _i何個目位置(idx(aidx)), _d六角ベース_高さ計, _d縁の高さ)
            Next


            '底位置

            _hex底の辺 = New CHex(
            _BandPositions(cIdxAngle0)._底の辺,
            _BandPositions(cIdxAngle60)._底の辺,
            _BandPositions(cIdxAngle120)._底の辺)

            _hex厚さプラス底の辺 = New CHex(
            _BandPositions(cIdxAngle0)._厚さプラス底の辺,
            _BandPositions(cIdxAngle60)._厚さプラス底の辺,
            _BandPositions(cIdxAngle120)._厚さプラス底の辺)

            _hex側面上辺 = New CHex(
            _BandPositions(cIdxAngle0)._側面上辺,
            _BandPositions(cIdxAngle60)._側面上辺,
            _BandPositions(cIdxAngle120)._側面上辺)

            _hex縁の辺 = New CHex(
            _BandPositions(cIdxAngle0)._縁の辺,
            _BandPositions(cIdxAngle60)._縁の辺,
            _BandPositions(cIdxAngle120)._縁の辺)

            If Not _hex底の辺.IsValidHexagon OrElse Not _hex厚さプラス底の辺.IsValidHexagon Then
                '立ち上げ可能な底を作れません。
                p_s警告 = My.Resources.CalcBadBottom
            End If

            Dim p1 As S実座標
            Dim p2 As S実座標
            For idx As Integer = 0 To 2
                For ax As Integer = 1 To _BandPositions(idx)._iひもの本数
                    Dim band As CBandPosition = _BandPositions(idx).ByAxis(ax)
                    p1.Zero()
                    p2.Zero()

                    For i = 0 To 5
                        If CHex.hex_aidx(i) = idx Then
                            Continue For
                        End If
                        If p1.IsZero Then
                            p1 = _hex底の辺.辺(i).p交点(band.fnひも中心線)
                            If Not p1.IsZero Then
                                '1st
                                band.m_底の交点HexIndex(0) = i
                                band.m_底の交点(0) = New S実座標(p1)
                            End If
                        ElseIf p2.IsZero Then
                            p2 = _hex底の辺.辺(i).p交点(band.fnひも中心線)
                            If Not p2.IsZero Then
                                '2nd
                                band.m_底の交点HexIndex(1) = i
                                band.m_底の交点(1) = New S実座標(p2)
                            End If
                        Else
                            Exit For
                        End If
                    Next

                    If p1.IsZero OrElse p2.IsZero Then
                        '長さを計算できないひもがあります。{0} {1}
                        p_s警告 = String.Format(My.Resources.CalcErrorBandLength, aidx(idx), ax)
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, p_s警告)
                    Else
                        band.m_底の長さ = New S差分(p1, p2).Length
                        band.m_row縦横展開.f_d長さ = band.m_底の長さ
                    End If
                Next
            Next

            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(0)={0}", _BandPositions(cIdxAngle0.ToString))
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(60)={0}", _BandPositions(cIdxAngle60.ToString))
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(120)={0}", _BandPositions(cIdxAngle120.ToString))
        End If

        '長さを反映
        ret = ret And adjust_展開ひも(AngleIndex._0deg)
        ret = ret And adjust_展開ひも(AngleIndex._60deg)
        ret = ret And adjust_展開ひも(AngleIndex._120deg)

        Return ret
    End Function

#Region "六角形"

    '平行な2辺
    Friend Class CHexLine
        Friend Const i_1st As Integer = 0
        Friend Const i_2nd As Integer = 1
        Dim _parent As CBandPositionList    '方向

        Dim _中心(1) As S実座標      '最初にセットする中心点
        Friend _辺(1) As S線分          '中心点を通る辺

        Sub New(ByVal parent As CBandPositionList)
            _parent = parent
        End Sub

        Sub SetCenters(ByVal p1st As S実座標, ByVal p2nd As S実座標)
            _中心(i_1st) = p1st
            _中心(i_2nd) = p2nd
        End Sub

        Sub SetCentersDelta(ByVal base As CHexLine, ByVal delta As S差分)
            _中心(i_1st) = base._中心(i_1st) + (delta * -1)
            _中心(i_2nd) = base._中心(i_2nd) + delta
        End Sub

        ReadOnly Property fn辺の式(ByVal i As Integer) As S直線式
            Get
                Return New S直線式(cBandAngleDegree(idx(_parent._AngleIndex)), _中心(i))
            End Get
        End Property

        ReadOnly Property delta辺の外向き法線(ByVal i As Integer) As S差分
            Get
                If i = i_1st Then
                    Return cDeltaAxisDirection(idx(_parent._AngleIndex)) * -1
                ElseIf i = i_2nd Then
                    Return cDeltaAxisDirection(idx(_parent._AngleIndex))
                Else
                    Return Nothing
                End If
            End Get
        End Property


        ReadOnly Property IsValid辺の方向() As Boolean
            Get
                Return SameAngle(cBandAngleDegree(idx(_parent._AngleIndex)), _辺(i_1st).s差分.Angle) AndAlso
                    SameAngle(cBandAngleDegree(idx(_parent._AngleIndex)) + 180, _辺(i_2nd).s差分.Angle)
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("1st[中心{0} 辺({1})] ", _中心(i_1st), _辺(i_1st))
            sb.AppendFormat("2nd[中心{0} 辺({1})] ", _中心(i_2nd), _辺(i_2nd))
            Return sb.ToString
        End Function
        Public Function dump() As String
            Dim sb As New System.Text.StringBuilder
            sb.Append(_parent._AngleIndex).AppendLine()
            sb.AppendFormat("1st: 中心{0} 辺({1})", _中心(i_1st), _辺(i_1st).dump()).AppendLine()
            sb.AppendFormat("2nd: 中心{0} 辺({1})", _中心(i_2nd), _辺(i_2nd).dump())
            Return sb.ToString
        End Function

    End Class

    '2辺×3の六角形
    Friend Class CHex
        Dim _HexLine(cAngleCount - 1) As CHexLine


        Shared ReadOnly Property hex_line(ByVal hexidx As Integer) As Integer
            Get
                Return hexidx \ 3
            End Get
        End Property
        Shared ReadOnly Property hex_aidx(ByVal hexidx As Integer) As Integer
            Get
                Return hexidx Mod 3
            End Get
        End Property


        ReadOnly Property IsValidHexagon() As Boolean
            Get
                Return _HexLine(cIdxAngle0).IsValid辺の方向 AndAlso
                    _HexLine(cIdxAngle60).IsValid辺の方向 AndAlso
                    _HexLine(cIdxAngle120).IsValid辺の方向
            End Get
        End Property

        ReadOnly Property fn(ByVal hexidx As Integer) As S直線式
            Get
                Return _HexLine(hex_aidx(hexidx)).fn辺の式(hex_line(hexidx))
            End Get
        End Property

        ReadOnly Property 辺(ByVal hexidx As Integer) As S線分
            Get
                Return _HexLine(hex_aidx(hexidx))._辺(hex_line(hexidx))
            End Get
        End Property

        ReadOnly Property delta辺の外向き法線(ByVal hexidx As Integer) As S差分
            Get
                Return _HexLine(hex_aidx(hexidx)).delta辺の外向き法線(hex_line(hexidx))
            End Get
        End Property



        '0度,60度,120度の順に指定
        Sub New(ByVal hline1 As CHexLine, ByVal hline2 As CHexLine, ByVal hline3 As CHexLine)
            _HexLine(cIdxAngle0) = hline1
            _HexLine(cIdxAngle60) = hline2
            _HexLine(cIdxAngle120) = hline3
            calc_hexagon()
        End Sub

        Private Function calc_hexagon() As Boolean

            '                fn3:0度の2nd
            '                p34 -- p23
            'fn4:60度の2nd ／         ＼ fn2:120度の1st
            '            p45           p12
            'fn5:120度の2nd＼         ／ fn1:60度の1st
            '                p50 -- p01
            '                 fn0:0度の1st

            'AngleIndex.idx_0
            Dim fn0 As S直線式 = _HexLine(cIdxAngle0).fn辺の式(CHexLine.i_1st) '0
            Dim fn3 As S直線式 = _HexLine(cIdxAngle0).fn辺の式(CHexLine.i_2nd) '180
            'AngleIndex.idx_60
            Dim fn1 As S直線式 = _HexLine(cIdxAngle60).fn辺の式(CHexLine.i_1st) '60
            Dim fn4 As S直線式 = _HexLine(cIdxAngle60).fn辺の式(CHexLine.i_2nd) '240
            'AngleIndex.idx_120
            Dim fn2 As S直線式 = _HexLine(cIdxAngle120).fn辺の式(CHexLine.i_1st) '120
            Dim fn5 As S直線式 = _HexLine(cIdxAngle120).fn辺の式(CHexLine.i_2nd) '300

            Dim p12 As S実座標 = fn1.p交点(fn2)
            Dim p23 As S実座標 = fn2.p交点(fn3)
            Dim p34 As S実座標 = fn3.p交点(fn4)
            Dim p45 As S実座標 = fn4.p交点(fn5)
            Dim p50 As S実座標 = fn5.p交点(fn0)
            Dim p01 As S実座標 = fn0.p交点(fn1)

            _HexLine(cIdxAngle0)._辺(CHexLine.i_1st) = New S線分(p50, p01)
            _HexLine(cIdxAngle60)._辺(CHexLine.i_1st) = New S線分(p01, p12)
            _HexLine(cIdxAngle120)._辺(CHexLine.i_1st) = New S線分(p12, p23)
            _HexLine(cIdxAngle0)._辺(CHexLine.i_2nd) = New S線分(p23, p34)
            _HexLine(cIdxAngle60)._辺(CHexLine.i_2nd) = New S線分(p34, p45)
            _HexLine(cIdxAngle120)._辺(CHexLine.i_2nd) = New S線分(p45, p50)

            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p50{0} p01{1} p12{2} p23{3} p34{4} p45{5} p50{6}", p50, p01, p12, p23, p34, p45, p50)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang0 {0}", _HexLine(cIdxAngle0).dump())
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang60 {0}", _HexLine(cIdxAngle60).dump())
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang120 {0}", _HexLine(cIdxAngle120).dump())

            Return IsValidHexagon()
        End Function
    End Class


#End Region

#Region "ひものセット、3方向"



    Friend Class CBandPosition
        Dim _parent As CBandPositionList    '方向
        Dim _iひも番号 As Integer

        Friend m_row縦横展開 As tbl縦横展開Row
        Friend m_dひも幅 As Double
        Friend m_d中心からの幅 As Double
        Friend m_ひもの中心 As S実座標

        '1st/2nd
        Friend m_底の交点HexIndex(1) As Integer
        Friend m_底の交点(1) As S実座標
        Friend m_底の長さ As Double


        Friend ReadOnly Property fnひも中心線 As S直線式
            Get
                Return New S直線式(cBandAngleDegree(idx(_parent._AngleIndex)), m_ひもの中心)
            End Get
        End Property



        Friend m_a四隅 As S四隅
        Friend m_p文字位置 As S実座標


        Sub New(ByVal parent As CBandPositionList, ByVal i As Integer)
            _parent = parent
            _iひも番号 = i
        End Sub

        Function AddImageItemToList(ByVal imglist As clsImageItemList) As Boolean
            If m_row縦横展開 Is Nothing Then
                Return False
            End If
            Dim item As New clsImageItem(m_row縦横展開)

            '描画タイプ(再)指定
            item.m_ImageType = ImageTypeEnum._バンド

            item.m_band = New CBand

            'm_a四隅にバンド描画位置
            Dim bandwidth As Double = m_dひも幅
#If 1 Then
            Dim d横ひも表示長 As Double = m_底の長さ
            m_a四隅.pA = m_底の交点(0) + cDeltaAxisDirection(idx(_parent._AngleIndex)) * (-bandwidth / 2)
            m_a四隅.pB = m_底の交点(1) + cDeltaAxisDirection(idx(_parent._AngleIndex)) * (-bandwidth / 2)
            m_a四隅.pD = m_底の交点(0) + cDeltaAxisDirection(idx(_parent._AngleIndex)) * (bandwidth / 2)
            m_a四隅.pC = m_底の交点(1) + cDeltaAxisDirection(idx(_parent._AngleIndex)) * (bandwidth / 2)


            'm_rひも位置に記号描画位置
            m_p文字位置 = m_ひもの中心 + cDeltaBandDirection(idx(_parent._AngleIndex)) * ((-d横ひも表示長 / 2) - bandwidth * 2)


#Else
           Dim d横ひも表示長 As Double = item.m_row縦横展開.f_d出力ひも長
            Dim p As S実座標 = m_ひもの中心 + cDeltaBandDirection(idx(_parent._AngleIndex)) * (-d横ひも表示長 / 2) + cDeltaAxisDirection(idx(_parent._AngleIndex)) * (-bandwidth / 2)

            m_a四隅.pA = p
            m_a四隅.pB = p + cDeltaBandDirection(idx(_parent._AngleIndex)) * d横ひも表示長
            m_a四隅.pD = p + cDeltaAxisDirection(idx(_parent._AngleIndex)) * bandwidth
            m_a四隅.pC = m_a四隅.pB + cDeltaAxisDirection(idx(_parent._AngleIndex)) * bandwidth


            'm_rひも位置に記号描画位置
            m_p文字位置 = m_ひもの中心 + cDeltaBandDirection(idx(_parent._AngleIndex)) * ((-d横ひも表示長 / 2) - bandwidth * 2)

#End If


            item.m_band.aバンド位置 = m_a四隅
            item.m_band.p文字位置 = m_p文字位置
            item.m_dひも幅 = m_dひも幅
            imglist.Add(item)

            Return True
        End Function


        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("Idx={0} ひも幅({1:f1})", _iひも番号, m_dひも幅).Append(vbTab)
            sb.AppendFormat("中心からの幅:({0:f1}) ひもの中心{1}", m_d中心からの幅, m_ひもの中心).Append(vbTab)
            sb.AppendFormat("m_a四隅:({0}) m_p文字位置({1})", m_a四隅, m_p文字位置).Append(vbTab)
            If m_row縦横展開 IsNot Nothing Then
                sb.AppendFormat("row縦横展開:({0},{1},{2}){3}本幅", m_row縦横展開.f_iひも種, m_row縦横展開.f_iひも番号, m_row縦横展開.f_i位置番号, m_row縦横展開.f_i何本幅)
            Else
                sb.Append("No row縦横展開")
            End If
            sb.AppendFormat("底の交点 長さ{0:f2}", m_底の長さ).AppendLine()
            sb.AppendFormat("1st({0}):{1}", m_底の交点HexIndex(0), m_底の交点(0)).AppendLine()
            sb.AppendFormat("2nd({0}):{1}", m_底の交点HexIndex(1), m_底の交点(1)).AppendLine()
            Return sb.ToString
        End Function

    End Class

    '展開テーブルの位置計算用
    Friend Class CBandPositionList
        Friend _AngleIndex As AngleIndex
        Dim _SameDirection As Boolean = True '軸方向は、ひも番号方向と同じか


        Friend _iひもの本数 As Integer

        'ひも番号順
        Dim _BandList As New List(Of CBandPosition)

        Dim _row補強ひも(1) As tbl縦横展開Row 'idx1→0, idx2=1



        '集計結果
        Friend _本幅変更あり As Boolean
        Friend _底領域幅 As Double  '目の数,端の目,ひも幅計,六つ目
        Friend _中心までの幅 As Double  '軸方向の1から



        Friend _底の辺 As CHexLine
        Friend _厚さプラス底の辺 As CHexLine
        Friend _側面上辺 As CHexLine
        Friend _縁の辺 As CHexLine


        '軸方向順←→ひも番号値　:1～_iひもの本数
        ReadOnly Property AxisIdx(ByVal ax As Integer) As Integer
            Get
                If ax < 1 OrElse _iひもの本数 < ax Then
                    Return -1
                End If
                If _SameDirection Then
                    Return ax
                Else
                    Return _iひもの本数 - ax + 1
                End If
            End Get
        End Property

        '指定位置の要素 ひも番号:1～_iひもの本数　で使用
        Friend ReadOnly Property ByIdx(ByVal idx As Integer) As CBandPosition
            Get
                If idx < 1 OrElse _iひもの本数 < idx Then
                    Return Nothing
                End If
                Return _BandList(idx - 1)
            End Get
        End Property

        '軸方向　:1～_iひもの本数
        Friend ReadOnly Property ByAxis(ByVal ax As Integer) As CBandPosition
            Get
                Return ByIdx(AxisIdx(ax))
            End Get
        End Property



        Sub New(ByVal dir As AngleIndex)
            _AngleIndex = dir

            Select Case _AngleIndex
                Case AngleIndex._0deg, AngleIndex._60deg, AngleIndex._120deg
                    _SameDirection = False '軸方向は、ひも番号方向と逆
                Case Else
                    '
            End Select

            _底の辺 = New CHexLine(Me)
            _厚さプラス底の辺 = New CHexLine(Me)
            _側面上辺 = New CHexLine(Me)
            _縁の辺 = New CHexLine(Me)
        End Sub

        Sub Clear()
            SetBasicCount(0)
        End Sub

        '指定サイズにする
        Private Function SetBasicCount(ByVal iひもの本数 As Integer) As Boolean
            _iひもの本数 = iひもの本数

            _row補強ひも(0) = Nothing
            _row補強ひも(1) = Nothing

            If _iひもの本数 < _BandList.Count Then
                '多い
                Do While _BandList.Count > _iひもの本数
                    _BandList.RemoveAt(_BandList.Count - 1)
                Loop
            ElseIf _BandList.Count < _iひもの本数 Then
                '少ない
                Do While _BandList.Count < _iひもの本数
                    _BandList.Add(New CBandPosition(Me, _BandList.Count + 1))
                Loop
            End If
            Return True
        End Function

        'テーブルのレコードをセットする
        Function SetTable(ByVal table As tbl縦横展開DataTable, ByVal iひもの本数 As Integer, ByVal i基本のひも幅 As Integer) As Boolean
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                SetBasicCount(0)
                Return False
            End If
            SetBasicCount(iひもの本数)

            Dim ret As Boolean = True
            Dim setcount As Integer = 0
            _本幅変更あり = False

            For Each row As tbl縦横展開Row In table.Rows
                Dim iひも番号 As Integer = row.f_iひも番号
                If is_idx補強(_AngleIndex, row.f_iひも種) Then
                    If iひも番号 = 1 OrElse iひも番号 = 2 Then
                        _row補強ひも(iひも番号 - 1) = row
                    Else
                        ret = False
                    End If
                ElseIf is_idxひも種(_AngleIndex, row.f_iひも種) Then
                    '処理のひも
                    If 1 <= iひも番号 AndAlso iひも番号 <= _iひもの本数 Then
                        ByIdx(iひも番号).m_row縦横展開 = row
                        setcount += 1

                        If row.f_i何本幅 <> i基本のひも幅 Then
                            _本幅変更あり = True
                        End If
                    Else
                        ret = False
                    End If
                Else
                    'すき間はスキップ
                End If
            Next

            Return ret And (setcount = _iひもの本数)
        End Function

        '基本的な配置情報を計算する
        Function CalcBasicPositions(ByVal d六つ目の高さ As Double, ByVal d端の目 As Double,
                                    ByVal i何個目位置 As Integer, ByVal d高さ計 As Double, ByVal d縁の高さ As Double) As Boolean
            Dim d端の目幅 As Double = d六つ目の高さ * d端の目
            Dim i中心位置 As Integer = AxisIdx(i何個目位置)


            Dim d端からの長さ(_iひもの本数) As Double
            Dim d端から中心まで As Double
            '軸方向に累計
            Dim d幅の計 As Double = d端の目幅
            For axis As Integer = 1 To _iひもの本数
                Dim band As CBandPosition = ByAxis(axis)
                Dim row As tbl縦横展開Row = band.m_row縦横展開

                band.m_dひも幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
                d端からの長さ(axis) = d幅の計 + band.m_dひも幅 / 2

                'マーク位置
                If axis = i中心位置 Then
                    If _SameDirection Then
                        d端から中心まで = d幅の計 + band.m_dひも幅 + (d六つ目の高さ / 2)
                    Else
                        d端から中心まで = d幅の計 - (d六つ目の高さ / 2)
                    End If
                End If

                d幅の計 += band.m_dひも幅
                If axis < _iひもの本数 Then
                    d幅の計 += d六つ目の高さ
                End If
            Next
            d幅の計 += d端の目幅


            _底領域幅 = d幅の計
            _中心までの幅 = d端から中心まで

            Dim d底の辺の中心1 As S実座標 = pOrigin + cDeltaAxisDirection(idx(_AngleIndex)) * -d端から中心まで
            For axis As Integer = 1 To _iひもの本数
                Dim band As CBandPosition = ByAxis(axis)

                band.m_d中心からの幅 = d端からの長さ(axis) - d端から中心まで
                band.m_ひもの中心 = pOrigin + cDeltaAxisDirection(idx(_AngleIndex)) * band.m_d中心からの幅
            Next
            Dim d底の辺の中心2 As S実座標 = pOrigin + cDeltaAxisDirection(idx(_AngleIndex)) * (d幅の計 - d端から中心まで)
            _底の辺.SetCenters(d底の辺の中心1, d底の辺の中心2)

            Dim d底の厚さ As Double = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
            _厚さプラス底の辺.SetCentersDelta(_底の辺, cDeltaAxisDirection(idx(_AngleIndex)) * d底の厚さ)

            '_底の辺の中心から
            _側面上辺.SetCentersDelta(_底の辺, cDeltaAxisDirection(idx(_AngleIndex)) * d高さ計)
            _縁の辺.SetCentersDelta(_側面上辺, cDeltaAxisDirection(idx(_AngleIndex)) * d縁の高さ)

            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, Me.ToString)
            Return True
        End Function



        Public Sub Set補強ひも長(ByVal idx As Integer, ByVal len As Double)
            If idx = 1 OrElse idx = 2 Then
                If _row補強ひも(idx - 1) IsNot Nothing Then
                    _row補強ひも(idx - 1).f_d長さ = len
                End If
            Else
                Return
            End If
        End Sub


        '位置リストをもとに描画用のリストを作成する
        Function ConvertToImageList() As clsImageItemList
            Dim imglist As New clsImageItemList
            For idx As Integer = 1 To _iひもの本数
                Dim bandposition As CBandPosition = ByIdx(idx)
                bandposition.AddImageItemToList(imglist)
            Next
            Return imglist
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("Direction={0} SameDirection={1} ひもの本数={2}", _AngleIndex, _SameDirection, _iひもの本数)
            If _本幅変更あり Then
                sb.Append("本幅変更あり").AppendLine()
            Else
                sb.Append("本幅変更なし").AppendLine()
            End If
            sb.AppendFormat("_底領域幅={0:f2} _中心までの幅={1:f2}", _底領域幅, _中心までの幅).AppendLine()
            sb.Append("底の辺:").Append(_底の辺).AppendLine()
            sb.Append("厚さプラス底の辺:").Append(_厚さプラス底の辺).AppendLine()
            sb.Append("側面上辺:").Append(_側面上辺).AppendLine()
            sb.Append("縁の辺:").Append(_縁の辺).AppendLine()

            For Each band As CBandPosition In _BandList
                sb.AppendLine(band.ToString)
            Next
            Return sb.ToString
        End Function

    End Class




    '底と側面枠
    Function imageList底と側面枠() As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        Dim line As S線分
        Dim p12 As S実座標
        Dim p23 As S実座標
        Dim p34 As S実座標
        Dim p45 As S実座標
        Dim p50 As S実座標
        Dim p01 As S実座標


        '                3:0度の2nd
        '                p34 -- p23
        ' 4:60度の2nd ／         ＼ 2:120度の1st
        '            p45           p12
        ' 5:120度の2nd＼         ／ 1:60度の1st
        '                p50 -- p01
        '                 0:0度の1st

        If 1 < _iひもの本数(0) AndAlso 1 < _iひもの本数(1) AndAlso 1 < _iひもの本数(2) Then
            '真ん中の六つ目
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)

            Dim dRadius As Double = _d六つ目の高さ / SIN60 / 2
            p12 = pOrigin + New S差分(0) * dRadius
            p23 = pOrigin + New S差分(60) * dRadius
            p34 = pOrigin + New S差分(120) * dRadius
            p45 = pOrigin + New S差分(180) * dRadius
            p50 = pOrigin + New S差分(240) * dRadius
            p01 = pOrigin + New S差分(300) * dRadius

            line = New S線分(p12, p23)
            item.m_lineList.Add(line)
            line = New S線分(p23, p34)
            item.m_lineList.Add(line)
            line = New S線分(p34, p45)
            item.m_lineList.Add(line)
            line = New S線分(p45, p50)
            item.m_lineList.Add(line)
            line = New S線分(p50, p01)
            item.m_lineList.Add(line)
            line = New S線分(p01, p12)
            item.m_lineList.Add(line)

            itemlist.AddItem(item)
        End If


        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠2, 1)

        line = New S線分(_BandPositions(0)._底の辺._辺(CHexLine.i_1st)) '0
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(1)._底の辺._辺(CHexLine.i_1st)) '1
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(2)._底の辺._辺(CHexLine.i_1st)) '2
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(0)._底の辺._辺(CHexLine.i_2nd)) '3
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(1)._底の辺._辺(CHexLine.i_2nd)) '4
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(2)._底の辺._辺(CHexLine.i_2nd)) '5
        item.m_lineList.Add(line)

        itemlist.AddItem(item)

        '側面枠

        Dim d縁厚さプラス_高さ As Double = _d六角ベース_高さ計 + _d縁の高さ
        For i As Integer = 0 To 5
            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, i)
            item.m_a四隅.pD = _hex底の辺.辺(i).p開始
            item.m_a四隅.pC = _hex底の辺.辺(i).p終了
            item.m_a四隅.pB = item.m_a四隅.pC + _hex底の辺.delta辺の外向き法線(i) * d縁厚さプラス_高さ
            item.m_a四隅.pA = item.m_a四隅.pD + _hex底の辺.delta辺の外向き法線(i) * d縁厚さプラス_高さ

            line = New S線分(_hex厚さプラス底の辺.辺(i))
            item.m_lineList.Add(line)

            line = New S線分(_hex側面上辺.辺(i))
            item.m_lineList.Add(line)

            itemlist.AddItem(item)
        Next

        Return itemlist
    End Function

#End Region





#Region "プレビュー画像生成"

    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If


        _ImageList描画要素 = Nothing

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        '_ImageList展開ひもを作る
        Dim _ImageList展開ひも(cAngleCount - 1) As clsImageItemList
        For Each aidx As AngleIndex In [Enum].GetValues(GetType(AngleIndex))
            _ImageList展開ひも(idx(aidx)) = _BandPositions(idx(aidx)).ConvertToImageList()
            If _ImageList展開ひも(idx(aidx)) Is Nothing Then
                '処理に必要な情報がありません。
                p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
                Return False
            End If
        Next

        '基本のひも幅(文字サイズ)と基本色
        imgData.setBasics(_d基本のひも幅, _Data.p_row目標寸法.Value("f_s基本色"))

        _ImageList描画要素 = imageList底と側面枠()




        '中身を移動
        For idx As Integer = 0 To cAngleCount - 1
            imgData.MoveList(_ImageList展開ひも(idx))
            _ImageList展開ひも(idx) = Nothing
        Next


        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing
        '        imgData.MoveList(_ImageList差しひも)
        '        _ImageList差しひも = Nothing

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function
#End Region


End Class
