Imports System.Drawing.Drawing2D
Imports System.Net.Security
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsUpDown
Imports CraftBand.Tables.dstDataTables
Imports CraftBandHexagon.clsCalcHexagon.CHexLine

Partial Public Class clsCalcHexagon
    '                       
    '                  バンド方向                                                                    
    '    位置順     ↗  120度                                          バンド方向  ＼　位置順     ↖軸方向
    '   (30度方向)／  　 ＼      ＼      ＼   　 ／　　　／　　　／       60度 　　 ↘(-30度方向)  ↖150度
    '   軸方向は210度      ＼  ──+──+─+──+─+──+──　／
    '                       ＼　　　＼／　　＼／　　＼／　　　／斜め60度　
    '                       　＼　　／＼　　／＼　　／＼　　／　　　       
    '                        ─+──+─+──+─+──+─+──横                                  
    '                       　　＼／　　＼／　　＼／　　＼／   ↑六つ目(ひも間)   バンド方向 ↓　位置順　　↑軸方向
    '                       　　／＼　　／＼　　／＼　　／＼   ↓                    0度     ↓(-90度方向) ↑90度
    '                        ─+──+─+──+─+──+─+──横                                  
    '                       　／　　＼／　　＼／　　＼／　　＼　　　　
    '                       ／　　　／＼　　／＼　　／＼　　　＼斜め120度　　　
    '                    ／　  ──+──+─+──+─+──+──　　＼
    '                                                                       


    'バンドの方向角=AngleIndex値
    Shared cBandAngleDegree() As Integer = {0, 60, 120}
    'バンドの方向
    Shared cDeltaBandDirection() As S差分 = {New S差分(0), New S差分(60), New S差分(120)}
    'バンドの軸方向
    Shared cDeltaAxisDirection() As S差分 = {New S差分(90), New S差分(150), New S差分(210)}


    '                                           i_1st         i_2nd
    '            (3)　　　　　　　　 線の角度　 最初の線　　　最後の線       軸方向
    '         p34 -- p23                          側面外方向   　側面外方向
    '   (4)  ／        ↙＼  (2)         0度     (0) 270度      (3)  90度       90度   (ひも逆順)      
    '      p45          p12            60度     (1) 330度      (4) 150度      150度   (ひも逆順)
    '   (5)  ＼   ↑ 　↖／  (1)       120度     (2)  60度      (5) 210度      210度   (ひも逆順)
    '         p50 -- p01           cBandAngleDegree                         cDeltaAxisDirection
    '            (0)
    '

    Enum CalcStatus
        _none
        _basic
        _hex
        _height
        _length
        _expanded
    End Enum
    Dim _CalcStatus As CalcStatus = CalcStatus._none


    '3方向のバンド位置
    Dim _BandPositions(cAngleCount - 1) As CBandPositionList

    '六角形領域
    Dim _hex最外六角形 As CHex   '最も外側にある底ひもの中心線で作られる六角形
    Dim _hex底の辺 As CHex         '端の目分を加えた底の六角形
    Dim _hex底の辺に厚さ As CHex   '底の辺に厚さをプラス
    Dim _hex側面上辺 As CHex       '側面の上辺、縁は含まない

    '底
    Dim _底の領域 As S領域
    Dim _底の周 As Double
    Dim _側面周比率対底 As Double


    Private Sub NewImageData()
        _BandPositions(cIdxAngle0) = New CBandPositionList(AngleIndex._0deg)
        _BandPositions(cIdxAngle60) = New CBandPositionList(AngleIndex._60deg)
        _BandPositions(cIdxAngle120) = New CBandPositionList(AngleIndex._120deg)

    End Sub

    Private Sub ClearImageData()
        _CalcStatus = CalcStatus._none

        For idx As Integer = 0 To cAngleCount - 1
            _BandPositions(idx).Clear()
        Next
        _hex最外六角形 = Nothing
        _hex底の辺 = Nothing
        _hex底の辺に厚さ = Nothing
        _hex側面上辺 = Nothing
    End Sub

    Private Function ToStringImageData() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("底の領域{0} _底の周({1}) _側面周比率対底({2})", _底の領域, _底の周, _側面周比率対底).AppendLine()
        For idx As Integer = 0 To cAngleCount - 1
            sb.AppendLine(_BandPositions(idx).ToString)
        Next
        If _hex最外六角形 IsNot Nothing Then
            sb.Append("_hex最外六角形").AppendLine(_hex最外六角形.ToString)
        End If
        If _hex底の辺 IsNot Nothing Then
            sb.Append("_hex底の辺").AppendLine(_hex底の辺.ToString)
        End If
        If _hex底の辺に厚さ IsNot Nothing Then
            sb.Append("_hex底の辺に厚さ").AppendLine(_hex底の辺に厚さ.ToString)
        End If
        If _hex側面上辺 IsNot Nothing Then
            sb.Append("_hex側面上辺").AppendLine(_hex側面上辺.ToString)
        End If

        Return sb.ToString
    End Function


#Region "位置と長さ計算結果"

    Private Function get六角ベース_横() As Double
        Return _底の領域.x幅
    End Function

    Private Function get六角ベース_縦() As Double
        Return _底の領域.y高さ
    End Function

    Private Function get底の六角計の周() As Double
        Return _底の周
    End Function

    '底の配置領域に立ち上げ増分をプラス, = p_d六つ目ベース_周　※底の厚さは加えない
    Private Function get側面の周長() As Double
        Return get底の六角計の周() _
            + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分")
    End Function

    '側面の高さ、最下段+編みひも分、六つ目分は_側面周比率対底を考慮 
    Private Function get側面高さ() As Double
        Return (_d最下段の目 * _d六つ目の高さ) + _d側面ひも幅計 +
            (_i側面の編みひも数 * get側面の六つ目の高さ())
    End Function

    '側面の高さに対応したひも長、計算・描画に使用   '縁は含まない
    Private Function get側面ひも長() As Double
        Return get側面高さ() / SIN60
    End Function

    Private Function get本幅変更あり(ByVal aidx As AngleIndex) As Boolean
        Return _BandPositions(idx(aidx))._b本幅変更あり
    End Function

    Private Function get側面周比率対底() As Double
        If _hex最外六角形 IsNot Nothing AndAlso _hex底の辺 IsNot Nothing Then
            Return _側面周比率対底
        Else
            Return 1
        End If
    End Function

    Private Function get側面の六つ目の高さ() As Double
        If _b高さの六つ目に反映 Then
            Return _d六つ目の高さ * get側面周比率対底()
        Else
            Return _d六つ目の高さ
        End If
    End Function

#End Region


    '配置数,展開各入力値(ひも長加算,ひも幅)がFixした状態で、長さを計算する
    Private Function calc_位置と長さ計算(ByVal is位置計算 As Boolean) As Boolean
        Dim ret As Boolean = True

        If is位置計算 Then
            '設定情報
            _CalcStatus = CalcStatus._none
            For Each aidx As AngleIndex In enumExeName.GetValues(GetType(AngleIndex))
                ret = ret And _BandPositions(idx(aidx)).SetTable(p_tbl縦横展開(aidx), _iひもの本数(idx(aidx)), _I基本のひも幅)
                ret = ret And _BandPositions(idx(aidx)).CalcBasicPositions(_d六つ目の高さ, _d端の目(idx(aidx)), _i何個目位置(idx(aidx)), _bひも中心合わせ)
            Next
            _CalcStatus = CalcStatus._basic

            '底位置を計算
            _hex最外六角形 = Nothing
            _hex底の辺 = Nothing
            _hex底の辺に厚さ = Nothing
            _hex側面上辺 = Nothing

            _hex最外六角形 = New CHex(
            _BandPositions(cIdxAngle0)._hln最外ひもの2辺,
            _BandPositions(cIdxAngle60)._hln最外ひもの2辺,
            _BandPositions(cIdxAngle120)._hln最外ひもの2辺)

            _hex底の辺 = New CHex(
            _BandPositions(cIdxAngle0)._hln底の2辺,
            _BandPositions(cIdxAngle60)._hln底の2辺,
            _BandPositions(cIdxAngle120)._hln底の2辺)

            _hex底の辺に厚さ = New CHex(
            _BandPositions(cIdxAngle0)._hln底の2辺に厚さ,
            _BandPositions(cIdxAngle60)._hln底の2辺に厚さ,
            _BandPositions(cIdxAngle120)._hln底の2辺に厚さ)

            If Not _hex最外六角形.IsValidHexagon OrElse
               Not _hex底の辺.IsValidHexagon OrElse
               Not _hex底の辺に厚さ.IsValidHexagon Then
                '立ち上げ可能な底を作れません。
                p_s警告 = My.Resources.CalcBadBottom
            End If
            _CalcStatus = CalcStatus._hex

            _底の領域 = _hex底の辺.r外接領域
            _底の周 = _hex底の辺.d周長
            _側面周比率対底 = _底の周 / _hex最外六角形.d周長


            '_側面周比率対底を反映した高さ
            For Each aidx As AngleIndex In enumExeName.GetValues(GetType(AngleIndex))
                ret = ret And _BandPositions(idx(aidx)).CalcInterPositions(get側面高さ())
            Next
            _CalcStatus = CalcStatus._height

            _hex側面上辺 = New CHex(
            _BandPositions(cIdxAngle0)._hln側面上2辺,
            _BandPositions(cIdxAngle60)._hln側面上2辺,
            _BandPositions(cIdxAngle120)._hln側面上2辺)


            '底とクロスする位置・その長さ
            Dim err_band As New List(Of String)
            For idx As Integer = 0 To cAngleCount - 1 '0度・60度・120度
                For ax As Integer = 1 To _iひもの本数(idx)
                    Dim band As CBandPosition = _BandPositions(idx).ByAxis(ax)
                    band.ReSet底の交点()

                    '六角形の各辺に対して
                    For hexidx = 0 To CHex.cHexCount - 1
                        If CHex.hex_aidx(hexidx) = idx Then
                            Continue For '同方向は除外
                        End If
                        Dim p As S実座標 = _hex底の辺.line辺(hexidx).p交点(band.fnひも中心線)
                        band.Set底の交点(hexidx, p)
                    Next

                    If Not band.IsSet底の交点Set() Then
                        err_band.Add(band.Ident)
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Error:IsSet底の交点Set {0}", band.ToString)
                    End If
                Next
                '補強ひもに長さを反映
                _BandPositions(idx).Set補強ひも長(ciひも番号_補強1, _hex底の辺.line辺(CHex.hexidx(idx, CHexLine.lineIdx.i_2nd)).Length)
                _BandPositions(idx).Set補強ひも長(ciひも番号_補強2, _hex底の辺.line辺(CHex.hexidx(idx, CHexLine.lineIdx.i_1st)).Length)
                _BandPositions(idx).Set補強ひも長(ciひも番号_クロス, _hex底の辺.CrossLine(idx).Length)
            Next
            If 0 < err_band.Count Then
                '長さを計算できないひもがあります-{0}
                p_s警告 = String.Format(My.Resources.CalcErrorBandLength, String.Join(",", err_band))

            Else
                _CalcStatus = CalcStatus._length
            End If


            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(0)={0}", _BandPositions(cIdxAngle0).ToString)
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(60)={0}", _BandPositions(cIdxAngle60).ToString)
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(120)={0}", _BandPositions(cIdxAngle120).ToString)
        End If

        '長さからひもの長さに反映
        ret = ret And adjust_展開ひも(AngleIndex._0deg)
        ret = ret And adjust_展開ひも(AngleIndex._60deg)
        ret = ret And adjust_展開ひも(AngleIndex._120deg)
        _CalcStatus = CalcStatus._expanded

        Return ret
    End Function

#Region "六角形"

    '平行な2辺
    Friend Class CHexLine
        Enum lineIdx
            i_1st = 0 '軸方向・先
            i_2nd = 1 '軸方向・後
        End Enum
        Friend Const HexLineCount As Integer = 2
        Dim _parent As CBandPositionList    '方向情報

        Dim _p辺の中心点(HexLineCount - 1) As S実座標   'Input:中心点と角度
        Dim _line辺(HexLineCount - 1) As S線分          '計算結果:中心点を通る辺

        Sub New(ByVal parent As CBandPositionList)
            _parent = parent
        End Sub

        Sub SetCenters(ByVal p1st As S実座標, ByVal p2nd As S実座標)
            _p辺の中心点(lineIdx.i_1st) = p1st
            _p辺の中心点(lineIdx.i_2nd) = p2nd
        End Sub

        Sub SetCentersDelta(ByVal base As CHexLine, ByVal delta As S差分)
            _p辺の中心点(lineIdx.i_1st) = base._p辺の中心点(lineIdx.i_1st) + (delta * -1)
            _p辺の中心点(lineIdx.i_2nd) = base._p辺の中心点(lineIdx.i_2nd) + delta
        End Sub

        Sub SetLinePoints(ByVal i As lineIdx, ByVal p1 As S実座標, ByVal p2 As S実座標)
            _line辺(i) = New S線分(p1, p2)
        End Sub

        ReadOnly Property line辺(ByVal i As lineIdx) As S線分
            Get
                Return _line辺(i)
            End Get
        End Property

        ReadOnly Property fn辺の式(ByVal i As lineIdx) As S直線式
            Get
                Return New S直線式(_parent.BandAngleDegree, _p辺の中心点(i))
            End Get
        End Property

        ReadOnly Property delta辺の外向き法線(ByVal i As lineIdx) As S差分
            Get
                If i = lineIdx.i_1st Then
                    Return _parent.DeltaAxisDirection * -1
                ElseIf i = lineIdx.i_2nd Then
                    Return _parent.DeltaAxisDirection
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ReadOnly Property IsValid辺の方向() As Boolean
            Get
                Return SameAngle(_parent.BandAngleDegree, _line辺(lineIdx.i_1st).s差分.Angle) AndAlso
                    SameAngle(_parent.BandAngleDegree + 180, _line辺(lineIdx.i_2nd).s差分.Angle)
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("1st[中心{0} 辺({1})] ", _p辺の中心点(lineIdx.i_1st), _line辺(lineIdx.i_1st))
            sb.AppendFormat("2nd[中心{0} 辺({1})] ", _p辺の中心点(lineIdx.i_2nd), _line辺(lineIdx.i_2nd))
            Return sb.ToString
        End Function
        Public Function dump() As String
            Dim sb As New System.Text.StringBuilder
            sb.Append(_parent.BandAngleDegree).AppendLine()
            sb.AppendFormat("1st: 中心{0} 辺({1})", _p辺の中心点(lineIdx.i_1st), _line辺(lineIdx.i_1st).dump()).AppendLine()
            sb.AppendFormat("2nd: 中心{0} 辺({1})", _p辺の中心点(lineIdx.i_2nd), _line辺(lineIdx.i_2nd).dump())
            Return sb.ToString
        End Function

    End Class

    '2辺×3の六角形
    Friend Class CHex
        Friend Const cHexCount As Integer = 6

        'Hex値から、CHexLine の i_1st/i_2nd
        Shared ReadOnly Property hex_line(ByVal hexidx As Integer) As lineIdx
            Get
                Return hexidx \ 3
            End Get
        End Property
        'Hex値から、AngleIndexのidx値
        Shared ReadOnly Property hex_aidx(ByVal hexidx As Integer) As Integer
            Get
                Return hexidx Mod 3
            End Get
        End Property
        'AngleIndexとi_1st/i_2ndからHex値
        Shared ReadOnly Property hexidx(ByVal aidx As Integer, ByVal line As lineIdx) As Integer
            Get
                Return line * 3 + aidx
            End Get
        End Property

        '2辺×3:AngleIndex順
        Dim _HexLine(cAngleCount - 1) As CHexLine
        '同方向の対角線:AngleIndex順
        Dim _CrossLine(cAngleCount - 1) As S線分

        ReadOnly Property IsValidHexagon() As Boolean
            Get
                Return _HexLine(cIdxAngle0).IsValid辺の方向 AndAlso
                    _HexLine(cIdxAngle60).IsValid辺の方向 AndAlso
                    _HexLine(cIdxAngle120).IsValid辺の方向
            End Get
        End Property

        ReadOnly Property r外接領域() As S領域
            Get
                Dim region As S領域
                For hexidx As Integer = 0 To cHexCount - 1
                    region = region.get拡大領域(line辺(hexidx).r外接領域)
                Next
                Return region
            End Get
        End Property

        ReadOnly Property d周長() As Double
            Get
                Dim len As Double
                For hexidx As Integer = 0 To cHexCount - 1
                    len += line辺(hexidx).Length
                Next
                Return len
            End Get
        End Property

        ReadOnly Property CrossLine(ByVal aidx As Integer) As S線分
            Get
                Return _CrossLine(aidx)
            End Get
        End Property

        ReadOnly Property line辺(ByVal hexidx As Integer) As S線分
            Get
                Return _HexLine(hex_aidx(hexidx)).line辺(hex_line(hexidx))
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
            Dim fn0 As S直線式 = _HexLine(cIdxAngle0).fn辺の式(CHexLine.lineIdx.i_1st) '0
            Dim fn3 As S直線式 = _HexLine(cIdxAngle0).fn辺の式(CHexLine.lineIdx.i_2nd) '180
            'AngleIndex.idx_60
            Dim fn1 As S直線式 = _HexLine(cIdxAngle60).fn辺の式(CHexLine.lineIdx.i_1st) '60
            Dim fn4 As S直線式 = _HexLine(cIdxAngle60).fn辺の式(CHexLine.lineIdx.i_2nd) '240
            'AngleIndex.idx_120
            Dim fn2 As S直線式 = _HexLine(cIdxAngle120).fn辺の式(CHexLine.lineIdx.i_1st) '120
            Dim fn5 As S直線式 = _HexLine(cIdxAngle120).fn辺の式(CHexLine.lineIdx.i_2nd) '300

            Dim p12 As S実座標 = fn1.p交点(fn2)
            Dim p23 As S実座標 = fn2.p交点(fn3)
            Dim p34 As S実座標 = fn3.p交点(fn4)
            Dim p45 As S実座標 = fn4.p交点(fn5)
            Dim p50 As S実座標 = fn5.p交点(fn0)
            Dim p01 As S実座標 = fn0.p交点(fn1)

            _HexLine(cIdxAngle0).SetLinePoints(CHexLine.lineIdx.i_1st, p50, p01)
            _HexLine(cIdxAngle60).SetLinePoints(CHexLine.lineIdx.i_1st, p01, p12)
            _HexLine(cIdxAngle120).SetLinePoints(CHexLine.lineIdx.i_1st, p12, p23)
            _HexLine(cIdxAngle0).SetLinePoints(CHexLine.lineIdx.i_2nd, p23, p34)
            _HexLine(cIdxAngle60).SetLinePoints(CHexLine.lineIdx.i_2nd, p34, p45)
            _HexLine(cIdxAngle120).SetLinePoints(CHexLine.lineIdx.i_2nd, p45, p50)

            '対角線:i_1stと同方向
            _CrossLine(cIdxAngle0) = New S線分(p45, p12)
            _CrossLine(cIdxAngle60) = New S線分(p50, p23)
            _CrossLine(cIdxAngle120) = New S線分(p01, p34)

            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p50{0} p01{1} p12{2} p23{3} p34{4} p45{5} p50{6}", p50, p01, p12, p23, p34, p45, p50)
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang0 {0}", _HexLine(cIdxAngle0).dump())
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang60 {0}", _HexLine(cIdxAngle60).dump())
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang120 {0}", _HexLine(cIdxAngle120).dump())

            Return IsValidHexagon()
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            For i As Integer = 0 To cAngleCount - 1
                sb.AppendFormat("_HexLine({0}) {1}", i, _HexLine(i)).AppendLine()
                sb.AppendFormat("_CrossLine({0}) {1}", i, _CrossLine(i))
            Next
            Return sb.ToString
        End Function
    End Class


#End Region

#Region "ひものセット、3方向"

    '各バンド
    Friend Class CBandPosition
        Dim _parent As CBandPositionList    '方向情報
        Dim m_iひも番号 As Integer

        Friend m_row縦横展開 As tbl縦横展開Row
        Friend m_dひも幅 As Double
        Friend m_p合わせ位置 As S実座標 '合わせライン上の点
        Friend m_d合わせ位置からの幅 As Double

        '底の六角形との交点・ST→ENが方向と一致
        Dim m_底の交点_ST As S実座標
        Dim m_交点HexIndex_ST As Integer
        Dim m_底の交点_EN As S実座標
        Dim m_交点HexIndex_EN As Integer
        Dim m_底の交点間長 As Double

        '識別情報
        Friend ReadOnly Property Ident As String
            Get
                Return String.Format("Angle({0}) {1}({2})", _parent.BandAngleDegree, m_iひも番号, _parent.AxisIdx(m_iひも番号))
            End Get
        End Property

        Sub New(ByVal parent As CBandPositionList, ByVal i As Integer)
            _parent = parent
            m_iひも番号 = i
        End Sub

        Friend ReadOnly Property fnひも中心線 As S直線式
            Get
                Return New S直線式(_parent.BandAngleDegree, m_p合わせ位置)
            End Get
        End Property

        Sub ReSet底の交点()
            m_交点HexIndex_ST = -1
            m_交点HexIndex_EN = -1
            m_底の交点_ST.Zero()
            m_底の交点_EN.Zero()
            m_底の交点間長 = -1
        End Sub

        Function IsSet底の交点Set() As Boolean
            Return 0 <= m_交点HexIndex_ST AndAlso 0 <= m_交点HexIndex_EN
        End Function

        Function Set底の交点(ByVal hexidx As Integer, ByVal p As S実座標) As Boolean
            If hexidx < 0 OrElse CHex.cHexCount <= hexidx OrElse p.IsZero Then
                Return False
            End If

            If m_交点HexIndex_ST < 0 Then
                '1点目
                m_交点HexIndex_ST = hexidx
                m_底の交点_ST = p
                Return True

            ElseIf m_交点HexIndex_EN < 0 Then
                '2点目
                Dim delta As New S差分(m_底の交点_ST, p)
                If SameAngle(delta.Angle, _parent.BandAngleDegree) Then
                    m_底の交点_EN = p
                    m_交点HexIndex_EN = hexidx
                Else
                    m_底の交点_EN = m_底の交点_ST
                    m_交点HexIndex_EN = m_交点HexIndex_ST
                    m_底の交点_ST = p
                    m_交点HexIndex_ST = hexidx
                End If
                m_底の交点間長 = delta.Length
                '
                If m_row縦横展開 IsNot Nothing Then
                    m_row縦横展開.f_d長さ = m_底の交点間長
                    m_row縦横展開.f_iVal1 = m_交点HexIndex_ST
                    m_row縦横展開.f_iVal2 = m_交点HexIndex_EN
                End If
                Return True

            Else
                Return False '2点以上はNG
            End If
        End Function

        Function ToBand() As CBand
            If m_row縦横展開 Is Nothing OrElse Not IsSet底の交点Set() Then
                Return Nothing
            End If

            Dim band = New CBand(m_row縦横展開)

            'f_dVal1に加算の側、f_dVal2に加算2の側(_SameDirection=Falseの前提・adjust_展開ひも)
            Dim pA As S実座標 = m_底の交点_ST + _parent.DeltaBandDirection * -m_row縦横展開.f_dVal1
            Dim pB As S実座標 = m_底の交点_EN + _parent.DeltaBandDirection * m_row縦横展開.f_dVal2

            'バンド描画位置
            band.p始点F = pA + _parent.DeltaAxisDirection * (-m_dひも幅 / 2)
            band.p終点F = pB + _parent.DeltaAxisDirection * (-m_dひも幅 / 2)
            band.p始点T = pA + _parent.DeltaAxisDirection * (m_dひも幅 / 2)
            band.p終点T = pB + _parent.DeltaAxisDirection * (m_dひも幅 / 2)

            '記号描画位置(現物合わせ)
            If _parent.BandAngleDegree = 120 Then
                band.p文字位置 = pB +
                  _parent.DeltaBandDirection * (m_dひも幅 / 2) +
                  _parent.DeltaAxisDirection * (m_dひも幅 / 2)
            ElseIf _parent.BandAngleDegree = 60 Then
                band.p文字位置 = pA +
                    _parent.DeltaBandDirection * -m_dひも幅 +
                    _parent.DeltaAxisDirection * (m_dひも幅 / 3)
            ElseIf _parent.BandAngleDegree = 0 Then
                band.p文字位置 = pA +
                    _parent.DeltaBandDirection * -m_dひも幅 +
                    _parent.DeltaAxisDirection * -(m_dひも幅 / 2)
            End If

            Return band
        End Function



        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0} ひも幅({1:f1})", Ident, m_dひも幅).Append(vbTab)
            sb.AppendFormat("中心からの幅:({0:f1}) ひもの中心{1}", m_d合わせ位置からの幅, m_p合わせ位置).Append(vbTab)
            If m_row縦横展開 IsNot Nothing Then
                sb.AppendFormat("row縦横展開:({0},{1},{2}){3}本幅", m_row縦横展開.f_iひも種, m_row縦横展開.f_iひも番号, m_row縦横展開.f_i位置番号, m_row縦横展開.f_i何本幅)
            Else
                sb.Append("No row縦横展開")
            End If
            sb.AppendFormat("底の交点 長さ{0:f2}", m_底の交点間長).AppendLine()
            sb.AppendFormat(" START({0}):{1}", m_交点HexIndex_ST, m_底の交点_ST)
            sb.AppendFormat(" END ({0}):{1}", m_交点HexIndex_EN, m_底の交点_EN).AppendLine()
            Return sb.ToString
        End Function

    End Class

    '各方向のセット
    Friend Class CBandPositionList
        'セットに対する定数値
        Dim _AngleIndex As AngleIndex
        Friend BandAngleDegree As AngleIndex 'バンドの方向角
        Friend DeltaBandDirection As S差分 'バンドの方向
        Friend DeltaAxisDirection As S差分 'バンドの軸方向



        'ひも番号順のリスト
        Dim _BandList As New List(Of CBandPosition)
        '補強ひも
        Dim _row補強ひも(ciひも番号_クロス) As tbl縦横展開Row '0は使わない

        Dim _iひもの本数 As Integer
        Dim _i何個目位置 As Integer  '設定した合わせ目の位置、1～ひもの本数-1

        '集計結果
        Friend _b本幅変更あり As Boolean
        Friend _d底領域幅 As Double  '目の数,端の目,ひも幅計,六つ目
        Friend _d合わせ位置までの幅 As Double  '軸方向の1から

        'バンドに平行な2辺
        Friend _hln最外ひもの2辺 As CHexLine
        Friend _hln底の2辺 As CHexLine
        Friend _hln底の2辺に厚さ As CHexLine
        Friend _hln側面上2辺 As CHexLine


        '軸方向順←→ひも番号値　:1～_iひもの本数
        ReadOnly Property AxisIdx(ByVal ax As Integer) As Integer
            Get
                If ax < 1 OrElse _iひもの本数 < ax Then
                    Return -1
                End If
                'いずれの角度に対しても、軸方向はひも番号に対して逆
                Return _iひもの本数 - ax + 1
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

        '合わせ位置 軸方向のax,合わせ目は(ax-1)～(ax)間にある
        Friend ReadOnly Property i合わせ位置AxisIdx() As Integer
            Get
                Return AxisIdx(_i何個目位置)
            End Get
        End Property


        Sub New(ByVal aidx As AngleIndex)
            _AngleIndex = aidx
            BandAngleDegree = CType(aidx, Integer) 'cBandAngleDegree(idx(aidx))
            DeltaBandDirection = cDeltaBandDirection(idx(aidx))
            DeltaAxisDirection = cDeltaAxisDirection(idx(aidx))

            _hln最外ひもの2辺 = New CHexLine(Me)
            _hln底の2辺 = New CHexLine(Me)
            _hln底の2辺に厚さ = New CHexLine(Me)
            _hln側面上2辺 = New CHexLine(Me)
        End Sub

        Sub Clear()
            SetBasicCount(0)
        End Sub

        '指定サイズにする
        Private Function SetBasicCount(ByVal iひもの本数 As Integer) As Boolean
            _iひもの本数 = iひもの本数

            For i As Integer = 0 To ciひも番号_クロス
                _row補強ひも(i) = Nothing
            Next

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
            _b本幅変更あり = False

            For Each row As tbl縦横展開Row In table.Rows
                Dim iひも番号 As Integer = row.f_iひも番号
                If is_idx補強(_AngleIndex, row.f_iひも種) Then
                    'iひも番号設定値を信じて
                    _row補強ひも(iひも番号) = row
                ElseIf is_idxひも種(_AngleIndex, row.f_iひも種) Then
                    '処理のひも
                    If 1 <= iひも番号 AndAlso iひも番号 <= _iひもの本数 Then
                        ByIdx(iひも番号).m_row縦横展開 = row
                        setcount += 1

                        If row.f_i何本幅 <> i基本のひも幅 Then
                            _b本幅変更あり = True
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
                                    ByVal i何個目位置 As Integer, ByVal bひも中心合わせ As Boolean) As Boolean
            '条件確認
            If _iひもの本数 < 2 OrElse d六つ目の高さ < 0 OrElse
                    i何個目位置 < 1 OrElse _iひもの本数 < i何個目位置 Then
                Return False
            End If
            If Not bひも中心合わせ AndAlso _iひもの本数 = i何個目位置 Then
                Return False
            End If
            _i何個目位置 = i何個目位置

            Dim d端の目幅 As Double = d六つ目の高さ * d端の目
            Dim i合わせAxisIdx As Integer = AxisIdx(i何個目位置)

            '目の中心で合わせる場合
            '       ひも番号      本数  本数-1    ▽合わせ位置         2     1  
            '       軸方向          1     2       ▽合わせ位置       本数-1 本数
            '                <-----|+|---|+|---      --|+|--       ---|+|---|+|------>
            '                端の目 紐 目 紐 目      目 紐 目       目 紐 目 紐 端の目
            ' d端からの長さ  |→　  +     +             +              +     +     →|d幅の計
            '                |→                  △d端から合わせ位置まで
            '     合わせ位置からの幅(マイナス) ← | → 合わせ位置からの幅(プラス)
            '

            'ひも中心で合わせる場合
            '       ひも番号      本数  本数-1     ▽合わせ位置         2     1  
            '       軸方向          1     2        ▽合わせ位置       本数-1 本数
            '                <-----|+|---|+|--   -|+|--       ---|+|---|+|------>
            '                端の目 紐 目 紐 目    紐 目       目 紐 目 紐 端の目
            ' d端からの長さ  |→　  +     +        +              +     +     →|d幅の計
            '                |→                   △d端から合わせ位置まで
            '     合わせ位置からの幅(マイナス)  ← | → 合わせ位置からの幅(プラス)

            Dim d端からの長さ(_iひもの本数) As Double
            Dim d端から合わせ位置まで As Double
            '軸方向に累計
            Dim d幅の計 As Double = d端の目幅
            For axis As Integer = 1 To _iひもの本数
                Dim band As CBandPosition = ByAxis(axis)
                Dim row As tbl縦横展開Row = band.m_row縦横展開

                band.m_dひも幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
                d端からの長さ(axis) = d幅の計 + band.m_dひも幅 / 2

                '合わせ位置
                If axis = i合わせAxisIdx Then
                    'ひも番号に対して逆方向なのでマイナスする
                    d端から合わせ位置まで = d幅の計 - (d六つ目の高さ / 2)
                End If

                d幅の計 += band.m_dひも幅
                If axis < _iひもの本数 Then
                    d幅の計 += d六つ目の高さ
                End If
            Next
            d幅の計 += d端の目幅


            _d底領域幅 = d幅の計
            _d合わせ位置までの幅 = d端から合わせ位置まで

            Dim p底の辺の中心1 As S実座標 = pOrigin + DeltaAxisDirection * -d端から合わせ位置まで
            For axis As Integer = 1 To _iひもの本数
                Dim band As CBandPosition = ByAxis(axis)

                band.m_d合わせ位置からの幅 = d端からの長さ(axis) - d端から合わせ位置まで
                band.m_p合わせ位置 = pOrigin + DeltaAxisDirection * band.m_d合わせ位置からの幅
            Next
            Dim p底の辺の中心2 As S実座標 = pOrigin + DeltaAxisDirection * (d幅の計 - d端から合わせ位置まで)
            _hln底の2辺.SetCenters(p底の辺の中心1, p底の辺の中心2)

            '最も外側のひもの中心
            _hln最外ひもの2辺.SetCenters(ByAxis(1).m_p合わせ位置, ByAxis(_iひもの本数).m_p合わせ位置)
            '底の厚さをプラス
            _hln底の2辺に厚さ.SetCentersDelta(_hln底の2辺, DeltaAxisDirection * p_d底の厚さ)

            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, Me.ToString)
            Return True
        End Function

        '相互計算後の値をセットする
        Function CalcInterPositions(ByVal d側面高さ As Double) As Boolean

            '_底の辺の中心から
            _hln側面上2辺.SetCentersDelta(_hln底の2辺, DeltaAxisDirection * d側面高さ)

            Return True
        End Function

        Public Sub Set補強ひも長(ByVal idx As Integer, ByVal len As Double)
            If idx = ciひも番号_補強1 OrElse idx = ciひも番号_補強2 OrElse idx = ciひも番号_クロス Then
                If _row補強ひも(idx) IsNot Nothing Then
                    _row補強ひも(idx).f_d長さ = len
                End If
            Else
                Return
            End If
        End Sub


        '位置リストをもとに描画用のバンドリストを作成する(軸Idx順)
        Function ConvertToBandList(Optional ByVal isEmpty As Boolean = False) As CBandList
            Dim bandlist As New CBandList
            For idx As Integer = 1 To _iひもの本数
                Dim band As CBand = Nothing
                If Not isEmpty Then
                    Dim bandposition As CBandPosition = ByAxis(idx) 'ByIdx
                    band = bandposition.ToBand
                End If
                bandlist.AddAt(band, idx)
            Next
            Return bandlist
        End Function


        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("Direction={0} BandAngleDegree={1} ひもの本数={2}", _AngleIndex, BandAngleDegree, _iひもの本数)
            If _b本幅変更あり Then
                sb.Append("本幅変更あり").AppendLine()
            Else
                sb.Append("本幅変更なし").AppendLine()
            End If
            sb.AppendFormat("_底領域幅={0:f2} _中心までの幅={1:f2}", _d底領域幅, _d合わせ位置までの幅).AppendLine()
            sb.Append("最外ひもの2辺:").Append(_hln最外ひもの2辺).AppendLine()
            sb.Append("底の2辺:").Append(_hln底の2辺).AppendLine()
            sb.Append("底の2辺に厚さ:").Append(_hln底の2辺に厚さ).AppendLine()
            sb.Append("側面上2辺:").Append(_hln側面上2辺).AppendLine()

            For Each band As CBandPosition In _BandList
                sb.AppendLine(band.ToString)
            Next
            Return sb.ToString
        End Function

    End Class
#End Region

    '3本組の位置識別
    Private Function OneOfThree(ByVal idx As Integer) As Integer
        Return Modulo(idx, cAngleCount)
    End Function

    '次の回転方向
    Private Function NextDirection(Optional ByVal idx As Integer = 0) As Integer
        If _Data.p_row底_縦横.Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_左側 Then
            'マイナス60度方向 cIdxAngle0-cIdxAngle120
            Return Modulo(idx - 1, cAngleCount)
        ElseIf _Data.p_row底_縦横.Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_右側 Then
            'プラス60度方向 cIdxAngle0-cIdxAngle60
            Return Modulo(idx + 1, cAngleCount)
        Else
            Return -1   'なし
        End If
    End Function


    '6側面のバンドセット, 縁はimglistに追加
    Function bandList側面(ByVal imglist As clsImageItemList, ByVal isDraw As Boolean) As CBandList()
        Dim bandlists As New List(Of CBandList)

        '6側面
        For hexidx As Integer = 0 To CHex.cHexCount - 1
            Dim bandlist As New CBandList
            bandlists.Add(bandlist)

            '厚さを加えた長さ、底位置からスタート
            Dim line As S線分 = _hex底の辺に厚さ.line辺(hexidx) +
                 _hex底の辺.delta辺の外向き法線(hexidx) * -p_d底の厚さ

            '側面のレコード→1本ごとのバンド　※高さに、底の厚さは加えない
            Dim idx As Integer = 1 '0=空き, 1～本数
            For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, "f_i番号 ASC , f_iひも番号 ASC")
                If r.f_i番号 = cHemNumber Then
                    '縁は後で処理
                    Continue For
                End If
                If r.f_i番号 = cIdxSpace Then
                    '最下段のスペースは高さを加えるだけ
                    line += _hex底の辺.delta辺の外向き法線(hexidx) * r.f_d高さ
                    Continue For
                End If
                For i As Integer = 1 To r.f_iひも本数
                    Dim band As CBand = Nothing
                    If isDraw Then
                        '展開されている場合は、f_d高さにセットされているが再計算する
                        Dim dバンド幅 As Double = g_clsSelectBasics.p_d指定本幅(r.f_i何本幅)

                        band = New CBand(r)
                        band.p始点F = line.p開始
                        band.p終点F = line.p終了
                        band.p始点T = line.p開始 + _hex底の辺.delta辺の外向き法線(hexidx) * dバンド幅
                        band.p終点T = line.p終了 + _hex底の辺.delta辺の外向き法線(hexidx) * dバンド幅
                        band.SetLengthRatio(r.f_d周長比率対底の周)
                        '文字は上側面(コード固定)
                        If hexidx = 3 Then
                            band.p文字位置 = band.p始点F
                        End If
                        band.is始点FT線 = False
                        band.is終点FT線 = False

                        line += _hex底の辺.delta辺の外向き法線(hexidx) * (dバンド幅 + get側面の六つ目の高さ())
                    End If
                    bandlist.AddAt(band, idx)
                    idx += 1
                Next
            Next

            '縁のレコードをイメージ情報化
            If Not isDraw Then
                Continue For
            End If
            Dim cond As String = String.Format("f_i番号 = {0}", cHemNumber)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

            Dim d高さ As Double = groupRow.GetNameValueSum("f_d高さ")
            Dim nひも本数 As Integer = groupRow.GetNameValueSum("f_iひも本数")
            Dim d周長比率対底の周 As Double = groupRow.GetNameValueMax("f_d周長比率対底の周")
            If 0 < nひも本数 Then

                Dim band As New CBand '一時値
                band.p始点F = line.p開始
                band.p終点F = line.p終了
                band.p始点T = line.p開始 + _hex底の辺.delta辺の外向き法線(hexidx) * d高さ
                band.p終点T = line.p終了 + _hex底の辺.delta辺の外向き法線(hexidx) * d高さ
                band.SetLengthRatio(d周長比率対底の周)

                Dim item As New clsImageItem(ImageTypeEnum._編みかた, groupRow, 1)
                item.m_a四隅 = band.aバンド位置
                '文字は上側面(コード固定)
                If hexidx = 3 Then
                    item.p_p文字位置 = band.p始点F '領域処理を伴う
                End If

                imglist.AddItem(item)
            End If
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "imglst({0}):{1}", hexidx, imglst.ToString)
        Next

        Return bandlists.ToArray
    End Function

    '巴(3すくみ)(指定なしを含む)の描画
    Function imageListバンドセット巴(ByVal d三角の中値 As Double, ByVal checked() As Boolean) As clsImageItemList
        Dim _ImageListバンドと縁 As New clsImageItemList

        '底の3方向のバンドセット
        Dim bandListBottom(cAngleCount - 1) As CBandList
        '底の3方向のバンドセット描画
        Dim imageItemBandBottom(cAngleCount - 1) As clsImageItem

        For idx As Integer = 0 To cAngleCount - 1
            bandListBottom(idx) = Nothing
            If checked(idx) Then
                bandListBottom(idx) = _BandPositions(idx).ConvertToBandList()
            End If
            imageItemBandBottom(idx) = New clsImageItem(bandListBottom(idx), 1, idx)
        Next


        '6側面のバンドセット
        Dim bandListSide() As CBandList = bandList側面(_ImageListバンドと縁, checked(cAngleCount))
        If bandListSide.Count <> CHex.cHexCount Then
            Return Nothing
        End If

        '6側面のバンドセット描画
        Dim imageItemBandSide(CHex.cHexCount - 1) As clsImageItem
        For hexidx As Integer = 0 To CHex.cHexCount - 1
            imageItemBandSide(hexidx) = New clsImageItem(bandListSide(hexidx), 2, hexidx)
        Next

        '綾指定があれば領域をクリップ
        If 0 <= NextDirection() Then
            If 0 <= d三角の中値 Then
                Dim is側面の三角形(cAngleCount - 1) As Boolean
                For idx As Integer = 0 To cAngleCount - 1
                    Dim d As Double = (_d端の目(idx) + _d最下段の目 - 1) * _d六つ目の高さ +
                        (get側面の六つ目の高さ() - _d六つ目の高さ) * _i側面の編みひも数
                    is側面の三角形(idx) = (Abs(d) <= d三角の中値)
                Next

                For idx As Integer = 0 To cAngleCount - 1
                    Dim iNext As Integer = NextDirection(idx)
                    imageItemBandBottom(idx).AddClip(bandListBottom(iNext))

                    If is側面の三角形(idx) Then
                        imageItemBandSide(CHex.hexidx(idx, CHexLine.lineIdx.i_1st)).AddClip(bandListBottom(iNext))
                        imageItemBandSide(CHex.hexidx(idx, CHexLine.lineIdx.i_2nd)).AddClip(bandListBottom(iNext))
                    End If
                    If is側面の三角形(iNext) Then
                        imageItemBandBottom(idx).AddClip(bandListSide(CHex.hexidx(iNext, CHexLine.lineIdx.i_1st)))
                        imageItemBandBottom(idx).AddClip(bandListSide(CHex.hexidx(iNext, CHexLine.lineIdx.i_2nd)))
                    End If
                Next
            End If
        End If

        '底の3方向のバンドセット追加
        For idx As Integer = 0 To cAngleCount - 1
            _ImageListバンドと縁.AddItem(imageItemBandBottom(idx))
        Next
        '6側面のバンドセット追加
        For hexidx As Integer = 0 To CHex.cHexCount - 1
            _ImageListバンドと縁.AddItem(imageItemBandSide(hexidx))
        Next

        Return _ImageListバンドと縁
    End Function

    '鉄線の描画
    Function imageListバンドセット鉄線(ByVal checked() As Boolean) As clsImageItemList
        Dim _ImageListバンドと縁 As New clsImageItemList

        '*1 checkedを反映した個別バンドを作る
        '　いずれも、(0は空,1～ひも本数,Countはひも本数+1)

        '底の3方向のバンドセット
        Dim bandListBottom(cAngleCount - 1) As CBandList
        For idx As Integer = 0 To cAngleCount - 1
            '軸Idx順のリスト
            bandListBottom(idx) = _BandPositions(idx).ConvertToBandList(Not checked(idx))
            If bandListBottom(idx) Is Nothing Then
                Return Nothing
            End If
        Next

        '6側面のバンドセット, 縁のみ描画リストに追加
        Dim bandListSide() As CBandList = bandList側面(_ImageListバンドと縁, checked(cAngleCount))
        If bandListSide.Count <> CHex.cHexCount Then
            Return Nothing
        End If


        '*2 描画アイテムにしてリストに追加する
        '側面のバンド(1～高さ) → 底のバンド(1～本数) → 側面のバンド(1～高さ)

        '3方向の描画リスト
        Dim imageItemBandList(cAngleCount - 1) As clsImageItemList

        For idx As Integer = 0 To cAngleCount - 1
            imageItemBandList(idx) = New clsImageItemList
            Dim num As Integer = 0

            Dim hexidx_1st As Integer = CHex.hexidx(idx, lineIdx.i_1st)
            For i As Integer = _i側面の編みひも数 To 1 Step -1
                Dim band As CBand = bandListSide(hexidx_1st)(i)
                Dim item As New clsImageItem(band, num, idx)
                imageItemBandList(idx).AddItem(item)
                num += 1
            Next

            For ax As Integer = 1 To _iひもの本数(idx)
                Dim band As CBand = bandListBottom(idx)(ax)
                Dim item As New clsImageItem(band, num, idx)
                imageItemBandList(idx).AddItem(item)
                num += 1
            Next

            Dim hexidx_2nd As Integer = CHex.hexidx(idx, lineIdx.i_2nd)
            For i As Integer = 1 To _i側面の編みひも数
                Dim band As CBand = bandListSide(hexidx_2nd)(i)
                Dim item As New clsImageItem(band, num, idx)
                imageItemBandList(idx).AddItem(item)
                num += 1
            Next
        Next


        '*3 3軸描画

        Dim ax合わせ前(cAngleCount - 1) As Integer
        For idx As Integer = 0 To cAngleCount - 1
            '合わせ目の前後のひも、前側
            ax合わせ前(idx) = _BandPositions(idx).i合わせ位置AxisIdx - 1
            '側面分をプラス、1を0に詰めた分をマイナス
            ax合わせ前(idx) = ax合わせ前(idx) + _i側面の編みひも数 - 1
        Next

        For idx As Integer = 0 To cAngleCount - 1
            '±60度方向
            Dim ax_angle As Integer = cBandAngleDegree(idx) + 90 '軸角度
            Dim iNest As Integer = NextDirection(idx)

            For idx2 As Integer = 0 To cAngleCount - 1
                If idx = idx2 Then
                    Continue For
                End If
                If idx2 = iNest Then
                    '±60度方向 が最上・下になるのでclip(1本分)

                    Dim shift_type As Boolean '2択
                    Dim ax_angle2 As Integer = cBandAngleDegree(idx2) + 90 '軸角度
                    If 1 < NextDirection(1) Then
                        '右綾
                        shift_type = (ax_angle + 60 = ax_angle2)
                    Else
                        '左綾
                        shift_type = (ax_angle + 120 <> ax_angle2)
                    End If

                    For k As Integer = 0 To imageItemBandList(idx).Count - 1
                        Dim jClip As Integer
                        If shift_type Then
                            jClip = OneOfThree(ax合わせ前(idx2) + 1 - (k - ax合わせ前(idx)))
                        Else
                            jClip = OneOfThree(ax合わせ前(idx2) + (k - ax合わせ前(idx)))
                        End If
                        For j As Integer = 0 To imageItemBandList(idx2).Count - 1
                            If OneOfThree(j) = jClip Then
                                imageItemBandList(idx)(k).AddClip(imageItemBandList(idx2)(j))
                            End If
                        Next
                    Next

                Else
                    '±120度方向 がその下・更に下になるのでclip(2本分)

                    Dim shift_type As Boolean '2択
                    Dim ax_angle2 As Integer = cBandAngleDegree(idx2) + 90 '軸角度
                    If 1 < NextDirection(1) Then
                        '右綾
                        shift_type = (ax_angle + 120 <> ax_angle2)
                    Else
                        '左綾
                        shift_type = (ax_angle + 60 = ax_angle2)
                    End If

                    For k As Integer = 0 To imageItemBandList(idx).Count - 1
                        Dim jLeft As Integer = OneOfThree(ax合わせ前(idx2) + (k - ax合わせ前(idx)))
                        If shift_type Then
                            jLeft = OneOfThree(ax合わせ前(idx2) + 1 - (k - ax合わせ前(idx)))
                        Else
                            jLeft = OneOfThree(ax合わせ前(idx2) + (k - ax合わせ前(idx)))
                        End If
                        For j As Integer = 0 To imageItemBandList(idx2).Count - 1
                            If OneOfThree(j) <> jLeft Then
                                imageItemBandList(idx)(k).AddClip(imageItemBandList(idx2)(j))
                            End If
                        Next
                    Next

                End If
            Next
        Next

        '3方向のバンドセット追加
        For idx As Integer = 0 To cAngleCount - 1
            _ImageListバンドと縁.MoveList(imageItemBandList(idx))
            imageItemBandList(idx) = Nothing
        Next

        Return _ImageListバンドと縁
    End Function

    '本麻の葉
    Function imageListバンドセット本麻の葉(ByVal checked() As Boolean) As clsImageItemList

        Return Nothing
    End Function

    '底と側面枠
    Function imageList底と側面枠() As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim line As S線分

        '                3:0度の2nd
        '                p34 -- p23
        ' 4:60度の2nd ／         ＼ 2:120度の1st
        '            p45           p12
        ' 5:120度の2nd＼         ／ 1:60度の1st
        '                p50 -- p01
        '                 0:0度の1st

        '合わせ位置の六つ目
        If 1 < _iひもの本数(0) AndAlso 1 < _iひもの本数(1) AndAlso 1 < _iひもの本数(2) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)

            Dim dRadius As Double = _d六つ目の高さ / SIN60 / 2
            Dim p12 As S実座標 = pOrigin + New S差分(0) * dRadius
            Dim p23 As S実座標 = pOrigin + New S差分(60) * dRadius
            Dim p34 As S実座標 = pOrigin + New S差分(120) * dRadius
            Dim p45 As S実座標 = pOrigin + New S差分(180) * dRadius
            Dim p50 As S実座標 = pOrigin + New S差分(240) * dRadius
            Dim p01 As S実座標 = pOrigin + New S差分(300) * dRadius

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

        line = New S線分(_BandPositions(cIdxAngle0)._hln底の2辺.line辺(CHexLine.lineIdx.i_1st)) '0
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(cIdxAngle60)._hln底の2辺.line辺(CHexLine.lineIdx.i_1st)) '1
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(cIdxAngle120)._hln底の2辺.line辺(CHexLine.lineIdx.i_1st)) '2
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(cIdxAngle0)._hln底の2辺.line辺(CHexLine.lineIdx.i_2nd)) '3
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(cIdxAngle60)._hln底の2辺.line辺(CHexLine.lineIdx.i_2nd)) '4
        item.m_lineList.Add(line)
        line = New S線分(_BandPositions(cIdxAngle120)._hln底の2辺.line辺(CHexLine.lineIdx.i_2nd)) '5
        item.m_lineList.Add(line)

        itemlist.AddItem(item)

        '側面枠・底の厚さ枠ベース　※側面枠のみ、底の厚さを加えて描く
        Dim d底から縁までを描く高さ As Double = get側面高さ() + _d縁の高さ - p_d底の厚さ
        For hexidx As Integer = 0 To CHex.cHexCount - 1
            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, hexidx)
            item.m_a四隅.pD = _hex底の辺に厚さ.line辺(hexidx).p開始
            item.m_a四隅.pC = _hex底の辺に厚さ.line辺(hexidx).p終了
            item.m_a四隅.pB = item.m_a四隅.pC + _hex底の辺に厚さ.delta辺の外向き法線(hexidx) * d底から縁までを描く高さ
            item.m_a四隅.pA = item.m_a四隅.pD + _hex底の辺に厚さ.delta辺の外向き法線(hexidx) * d底から縁までを描く高さ

            line = New S線分(_hex側面上辺.line辺(hexidx))
            item.m_lineList.Add(line)

            itemlist.AddItem(item)
        Next

        Return itemlist
    End Function





#Region "プレビュー画像生成"

    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData, ByVal checked() As Boolean) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        '基本のひも幅(文字サイズ)と基本色
        imgData.setBasics(_d基本のひも幅, _Data.p_row目標寸法.Value("f_s基本色"))

        'バンドセット
        Dim _ImageListバンドセット As clsImageItemList = Nothing

        If NextDirection() < 0 OrElse _frmMain.rad巴_3すくみ.Checked Then
            '綾指定がないか、3すくみ指定
            _ImageListバンドセット = imageListバンドセット巴(_frmMain.nud三角の中.Value, checked)
        ElseIf _frmMain.rad鉄線_3軸織り.Checked Then
            '綾指定があり、鉄線
            _ImageListバンドセット = imageListバンドセット鉄線(checked)
        ElseIf _frmMain.rad本麻の葉編み.Checked Then
            '綾指定があり、鉄線
            _ImageListバンドセット = imageListバンドセット本麻の葉(checked)
        End If

        If _ImageListバンドセット Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '底と側面
        Dim _ImageList描画要素 As clsImageItemList = imageList底と側面枠()



        imgData.MoveList(_ImageListバンドセット)
        _ImageListバンドセット = Nothing
        '底と側面
        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function
#End Region


End Class
