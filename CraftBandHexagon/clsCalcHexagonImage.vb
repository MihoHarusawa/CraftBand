Imports System.Drawing.Imaging
Imports System.Reflection.Metadata
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsImageItem.CBand
Imports CraftBand.Tables.dstDataTables
Imports CraftBandHexagon.clsCalcHexagon
Imports CraftBandHexagon.clsCalcHexagon.CHex
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
    'バンドの軸方向(+90)
    Shared cDeltaAxisDirection() As S差分 = {New S差分(90), New S差分(150), New S差分(210)}
    '設定画面での方向(ひも長加算→ひも長加算2)とバンドの方向角が同じ時True
    Shared cSameDirectionWithDialog() As Boolean = {True, True, False} '左→右,左下→右上,左上→右下


    '(hexidx)                                    i_1st         i_2nd
    '            (3)　　　　　　　　 線の角度　 最初の線　　　最後の線       軸方向
    '         p34 -- p23                          側面外方向   　側面外方向
    '   (4)  ／        ↙＼  (2)         0度     (0) 270度      (3)  90度       90度   (ひも逆順)      
    '      p45          p12            60度     (1) 330度      (4) 150度      150度   (ひも逆順)
    '   (5)  ＼   ↑ 　↖／  (1)       120度     (2)  60度      (5) 210度      210度   (ひも逆順)
    '         p50 -- p01           cBandAngleDegree                         cDeltaAxisDirection
    '            (0)
    '

    Enum CalcStatus
        '           _CalcStatus     以外でセット
        _none       '未                  ※ 
        _basic      '基本数
        _hex        '六角形
        _height     '側面の高さ
        _length     'ひも長                                       
        _reflect    '各レコードに反映

        _position                       '※

        _error                          '※
    End Enum

    '* 基本: 3方向のバンドと六角形
    Dim _CalcStatus As CalcStatus = CalcStatus._none

    '3方向のバンド位置
    Dim _BandPositions(cAngleCount - 1) As CBandPositionList
    '六角形領域
    Dim _hex最外六角形 As CHex   '最も外側にある底ひもの中心線で作られる六角形
    Dim _hex外目幅の中心 As CHex   '最も外側のひもの外側+(六つ目幅/2)で作られる六角形
    Dim _hex底の辺 As CHex         '端の目分を加えた底の六角形,底の長さ計算、高さゼロ
    Dim _hex底の辺に厚さ As CHex   '底の辺に厚さをプラス, 側面枠のベース
    Dim _hex側面上辺 As CHex       '側面の上辺、縁は含まない
    '底
    Dim _底の領域 As S領域
    Dim _底の周 As Double
    Dim _側面周比率対底 As Double

    '* 追加情報: 基本情報に依存
    '編みひもの情報
    Dim _CalcStatusSideBand As CalcStatus = CalcStatus._none
    Dim _CSideBandList As CSideBandList

    '底ひも直角方向の情報(軸方向:90,150,210)
    Dim _CalcStatusBottomRightAngle As CalcStatus = CalcStatus._none
    Dim _BottomRightAngle(cAngleCount - 1) As CBottomRightAngle

    '側面の情報
    Dim _CalcStatusSidePlate As CalcStatus = CalcStatus._none
    Dim _SidePlate(CHex.cHexCount - 1) As CSidePlate



    Private Sub NewImageData()
        '* 基本
        _BandPositions(cIdxAngle0) = New CBandPositionList(AngleIndex._0deg)
        _BandPositions(cIdxAngle60) = New CBandPositionList(AngleIndex._60deg)
        _BandPositions(cIdxAngle120) = New CBandPositionList(AngleIndex._120deg)

        '* 編みひもの情報
        _CSideBandList = New CSideBandList

        '* 底ひも直角方向の情報
        _BottomRightAngle(cIdxAngle0) = New CBottomRightAngle
        _BottomRightAngle(cIdxAngle60) = New CBottomRightAngle
        _BottomRightAngle(cIdxAngle120) = New CBottomRightAngle

        '* 側面の情報
        For hxidx As Integer = 0 To CHex.cHexCount - 1
            _SidePlate(hxidx) = New CSidePlate(hxidx)
        Next

        '* 差しひも保存値
        _InsertExpand = New clsInsertExpand
    End Sub

    Private Sub ClearImageData()
        _CalcStatus = CalcStatus._none
        _CalcStatusSideBand = CalcStatus._none
        _CalcStatusBottomRightAngle = CalcStatus._none
        _CalcStatusSidePlate = CalcStatus._none

        _hex最外六角形 = Nothing
        _hex外目幅の中心 = Nothing
        _hex底の辺 = Nothing
        _hex底の辺に厚さ = Nothing
        _hex側面上辺 = Nothing

        _底の領域.Clear()
        _底の周 = -1
        _側面周比率対底 = 1

        For idx As Integer = 0 To cAngleCount - 1
            _BandPositions(idx).Clear()
            _BottomRightAngle(idx).Clear()
        Next

        For hxidx As Integer = 0 To CHex.cHexCount - 1
            _SidePlate(hxidx).Clear()
        Next

        _CSideBandList.Clear()
        _InsertExpand.Clear()
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ClearImageData")
    End Sub

    Private Function ToStringImageData() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("_CalcStatus({0} 底の領域:{1} _底の周({2}) _側面周比率対底({3})", _CalcStatus, _底の領域, _底の周, _側面周比率対底).AppendLine()
        For idx As Integer = 0 To cAngleCount - 1
            sb.AppendLine(_BandPositions(idx).ToString)
        Next
        If _hex最外六角形 IsNot Nothing Then
            sb.Append("_hex最外六角形").AppendLine(_hex最外六角形.ToString)
        End If
        If _hex外目幅の中心 IsNot Nothing Then
            sb.Append("_hex外目幅の中心").AppendLine(_hex外目幅の中心.ToString)
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
        sb.AppendFormat("編みひもの情報 {0} {1}", _CalcStatusSideBand, _CSideBandList).AppendLine()
        sb.AppendFormat("底ひも直角方向の情報 {0}", _CalcStatusBottomRightAngle).AppendLine()
        For idx As Integer = 0 To cAngleCount - 1
            sb.AppendFormat("{0}({1}) {2}", cBandAngleDegree(idx) + 90, cBandAngleDegree(idx), _BottomRightAngle(idx)).AppendLine()
        Next
        '位置と長さ計算時にはクリア状態
        'sb.AppendFormat("側面の情報 {0}", _CalcStatusSidePlate).AppendLine()
        'For idx As Integer = 0 To cHexCount - 1
        '    sb.AppendFormat("{0} {1}", idx, _SidePlate(idx)).AppendLine()
        'Next

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
    '_BandPositions,_hex系,底位置
    Private Function calc_位置と長さ計算(ByVal is位置計算 As Boolean) As Boolean
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "calc_位置と長さ計算({0})", is位置計算)
        Dim ret As Boolean = True

        If is位置計算 Then
            '_CalcStatus = CalcStatus._none
            ClearImageData()

            '設定情報
            For Each aidx As AngleIndex In enumExeName.GetValues(GetType(AngleIndex))
                ret = ret And _BandPositions(idx(aidx)).SetTable(p_tbl縦横展開(aidx), _iひもの本数(idx(aidx)), _I基本のひも幅)
                ret = ret And _BandPositions(idx(aidx)).CalcBasicPositions(_d六つ目の高さ, _d端の目(idx(aidx)), _i何個目位置(idx(aidx)), _bひも中心合わせ)
            Next
            If Not ret Then
                '基本的な情報を取得できません。
                p_sメッセージ = My.Resources.CalcErrorBasic
                Return False
            End If
            _CalcStatus = CalcStatus._basic

            '底位置を計算
            _hex最外六角形 = New CHex(
            _BandPositions(cIdxAngle0)._hln最外ひもの2辺,
            _BandPositions(cIdxAngle60)._hln最外ひもの2辺,
            _BandPositions(cIdxAngle120)._hln最外ひもの2辺)

            _hex外目幅の中心 = New CHex(
            _BandPositions(cIdxAngle0)._hln外目幅の中心の2辺,
            _BandPositions(cIdxAngle60)._hln外目幅の中心の2辺,
            _BandPositions(cIdxAngle120)._hln外目幅の中心の2辺)

            _hex底の辺 = New CHex(
            _BandPositions(cIdxAngle0)._hln底の2辺,
            _BandPositions(cIdxAngle60)._hln底の2辺,
            _BandPositions(cIdxAngle120)._hln底の2辺)

            _hex底の辺に厚さ = New CHex(
            _BandPositions(cIdxAngle0)._hln底の2辺に厚さ,
            _BandPositions(cIdxAngle60)._hln底の2辺に厚さ,
            _BandPositions(cIdxAngle120)._hln底の2辺に厚さ)

            '#62
            If Not _hex最外六角形.IsValidHexagon(True) OrElse
               Not _hex外目幅の中心.IsValidHexagon(False) OrElse
               Not _hex底の辺.IsValidHexagon(True) OrElse
               Not _hex底の辺に厚さ.IsValidHexagon(True) Then
                '立ち上げ可能な底を作れません。
                p_s警告 = My.Resources.CalcBadBottom
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "NOT ValidHexagon :{0}", p_s警告)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "_hex最外六角形 :{0}", _hex最外六角形.dump())
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "_hex外目幅の中心 :{0}", _hex外目幅の中心.dump())
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "_hex底の辺 :{0}", _hex底の辺.dump())
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "_hex底の辺に厚さ :{0}", _hex底の辺に厚さ.dump())
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


            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_hex底の辺 :{0}", _hex底の辺.dump())
            '底とクロスする位置・その長さ
            Dim err_band As New List(Of String)
            For idx As Integer = 0 To cAngleCount - 1 '0度・60度・120度
                For ax As Integer = 1 To _iひもの本数(idx)
                    Dim band As CBandPosition = _BandPositions(idx).ByAxis(ax)
                    band.ReSet底の交点()

                    '六角形との交点を得る
                    Dim cp As CHex.CCrossPoint = _hex底の辺.get辺との交点(band.m_pひもの中心, band.BandAngle)
                    Dim cp0 As CHex.CCrossPoint = _hex最外六角形.get辺との交点(band.m_pひもの中心, band.BandAngle)
                    If Not band.Set底の交点(cp, cp0) Then
                        err_band.Add(band.Ident)
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Error:IsSet底の交点Set {0}", band.ToString)
                    End If
                Next
                '補強ひもに長さを反映
                _BandPositions(idx).Set補強ひも長(ciひも番号_補強1, _hex底の辺.line辺(CHex.hexidx(idx, CHexLine.lineIdx.i_2nd)).Length)
                _BandPositions(idx).Set補強ひも長(ciひも番号_補強2, _hex底の辺.line辺(CHex.hexidx(idx, CHexLine.lineIdx.i_1st)).Length)
                _BandPositions(idx).Set補強ひも長(ciひも番号_クロス, _hex底の辺.CrossLine(idx).Length)
                '#72
                _BandPositions(idx).Set角位置(Not p_b横ひもゼロ And Not _bひも中心合わせ, _hex外目幅の中心.CrossLine(idx))
            Next
            If 0 < err_band.Count Then
                '長さを計算できないひもが{0}本あります。
                p_s警告 = String.Format(My.Resources.CalcErrorBandLength, err_band.Count)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "{0} {1}", p_s警告, String.Join(",", err_band))

            Else
                _CalcStatus = CalcStatus._length
            End If

            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(0)={0}", _BandPositions(cIdxAngle0).ToString)
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(60)={0}", _BandPositions(cIdxAngle60).ToString)
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList(120)={0}", _BandPositions(cIdxAngle120).ToString)
        End If

        If CalcStatus._length = _CalcStatus OrElse CalcStatus._reflect = _CalcStatus Then
            '長さからひもの長さに反映
            ret = ret And adjust_展開ひも(AngleIndex._0deg)
            ret = ret And adjust_展開ひも(AngleIndex._60deg)
            ret = ret And adjust_展開ひも(AngleIndex._120deg)
        Else
            ret = False
        End If
        '
        If ret Then
            _CalcStatus = CalcStatus._reflect
        Else
            'ひもの長さを結果に反映することができません。
            p_sメッセージ = My.Resources.CalcErrorReflection
            Return False
        End If

        Return ret
    End Function

    'calc_位置と長さ計算正常終了後,リスト・画像出力のための追加計算
    '必要情報のステイタス指定(編みひも,底ひも直角方向の情報,側面の情報)
    Private Function calc_追加計算(ByVal stSideBand As CalcStatus, ByVal stBottom As CalcStatus, ByVal stSidePlate As CalcStatus) As Boolean
        '基本計算は終わっていること
        If _CalcStatus <> CalcStatus._reflect Then
            Return False
        End If

        '編みひも
        If CalcStatus._none < stSideBand Then
            If _CalcStatusSideBand = CalcStatus._error Then
                Return False
            ElseIf _CalcStatusSideBand < stSideBand Then

                Dim r As Boolean = setSideBandList()
                If r Then
                    _CalcStatusSideBand = CalcStatus._position
                Else
                    _CalcStatusSideBand = CalcStatus._error
                    Return False
                End If
            Else
                '計算済
            End If
        End If

        '底ひも直角方向の情報
        If CalcStatus._none < stBottom Then
            If _CalcStatusBottomRightAngle = CalcStatus._error Then
                Return False
            ElseIf _CalcStatusBottomRightAngle < stBottom Then

                Dim r As Boolean = setBottomRightAngle()
                If r Then
                    _CalcStatusBottomRightAngle = CalcStatus._position
                Else
                    _CalcStatusBottomRightAngle = CalcStatus._error
                    Return False
                End If
            Else
                '計算済
            End If
        End If

        '側面の情報
        If CalcStatus._none < stSidePlate Then
            If _CalcStatusSidePlate = CalcStatus._error Then
                Return False
            ElseIf _CalcStatusSidePlate < stSidePlate Then

                Dim r As Boolean = setSidePlate()
                If r Then
                    _CalcStatusSidePlate = CalcStatus._position
                Else
                    _CalcStatusSidePlate = CalcStatus._error
                    Return False
                End If
            Else
                '計算済
            End If
        End If

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "calc_追加計算:_CalcStatusSideBand={0} _CalcStatusBottomRightAngle={1} _CalcStatusSidePlate={2}", _CalcStatusSideBand, _CalcStatusBottomRightAngle, _CalcStatusSidePlate)
        Return True
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

        '   <---  ・  ---   i_2nd (バンドと逆方向)
        '         | ゼロ=IsFlattened()
        '  ----  ・  ---->  i_1st (バンドと同方向)

        <Flags>
        Enum enumShapeHexLine
            none = 0    '
            flattened = &H1   '高さがない
            no_line_1st = &H2   '線がない(点)
            no_line_2nd = &H4
            bad_direction_1st = &H10  '方向が違っている
            bad_direction_2nd = &H20
        End Enum

        Dim _p辺の中心点(HexLineCount - 1) As S実座標   'Input:中心点　角度:parent値
        Dim _line辺(HexLineCount - 1) As S線分          '計算結果:中心点を通る辺

        Sub New(ByVal parent As CBandPositionList)
            _parent = parent
        End Sub

        '2辺の中心点で初期化
        Sub SetCenters(ByVal p1st As S実座標, ByVal p2nd As S実座標)
            _p辺の中心点(lineIdx.i_1st) = p1st
            _p辺の中心点(lineIdx.i_2nd) = p2nd
        End Sub
        Sub SetCentersDelta(ByVal base As CHexLine, ByVal delta As S差分)
            _p辺の中心点(lineIdx.i_1st) = base._p辺の中心点(lineIdx.i_1st) + (delta * -1)
            _p辺の中心点(lineIdx.i_2nd) = base._p辺の中心点(lineIdx.i_2nd) + delta
        End Sub

        '他方向との交点をセット
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
                '180度方向も式は同じ
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

        '2辺が1本につぶれている(六角形にならない)
        Private ReadOnly Property IsFlattened() As Boolean
            Get
                Return _p辺の中心点(lineIdx.i_1st).Near(_p辺の中心点(lineIdx.i_2nd))
            End Get
        End Property

        Private ReadOnly Property Is辺Exist(ByVal i As lineIdx) As Boolean
            Get
                Return Not _line辺(i).IsDot
            End Get
        End Property

        ReadOnly Property ShapeHexLine() As enumShapeHexLine
            Get
                Dim shape As enumShapeHexLine = enumShapeHexLine.none

                If IsFlattened Then
                    shape = shape Or enumShapeHexLine.flattened
                End If

                If Is辺Exist(CHexLine.lineIdx.i_1st) Then
                    If Not Is辺方向Valid(CHexLine.lineIdx.i_1st) Then
                        shape = shape Or enumShapeHexLine.bad_direction_1st
                    End If
                Else
                    shape = shape Or enumShapeHexLine.no_line_1st
                End If

                If Is辺Exist(CHexLine.lineIdx.i_2nd) Then
                    If Not Is辺方向Valid(CHexLine.lineIdx.i_2nd) Then
                        shape = shape Or enumShapeHexLine.bad_direction_2nd
                    End If
                Else
                    shape = shape Or enumShapeHexLine.no_line_2nd
                End If

                Return shape
            End Get
        End Property

        ReadOnly Property BandAngle(ByVal i As lineIdx) As Integer
            Get
                If i = lineIdx.i_1st Then
                    Return _parent.BandAngleDegree
                ElseIf i = lineIdx.i_2nd Then
                    Return _parent.BandAngleDegree + 180
                Else
                    Return False
                End If
            End Get
        End Property

        Private ReadOnly Property Is辺方向Valid(ByVal i As lineIdx) As Boolean
            Get
                Dim line As S線分
                If i = lineIdx.i_1st Then
                    line = _line辺(lineIdx.i_1st)
                ElseIf i = lineIdx.i_2nd Then
                    line = _line辺(lineIdx.i_2nd)
                Else
                    Return False
                End If
                If line.IsDot Then
                    Return False    '辺が必要
                End If
                Dim angle As Double = line.s差分.Angle
                If SameAngle(BandAngle(i), angle) Then
                    Return True
                End If
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "NOT Is辺方向Valid({0}){1}<>{2}", CType(i, lineIdx), BandAngle(i), angle)
                Return False
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0} {1} {2}", Me.GetType().Name, _parent.BandAngleDegree, FlagEnumString(GetType(enumShapeHexLine), ShapeHexLine()))
            sb.AppendFormat("1st({0})[中心{1} 辺({2})] ", Is辺方向Valid(lineIdx.i_1st), _p辺の中心点(lineIdx.i_1st), _line辺(lineIdx.i_1st))
            sb.AppendFormat("2nd({0})[中心{1} 辺({2})] ", Is辺方向Valid(lineIdx.i_2nd), _p辺の中心点(lineIdx.i_2nd), _line辺(lineIdx.i_2nd))
            Return sb.ToString
        End Function
        Public Function dump() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0} {1} {2}", Me.GetType().Name, _parent.BandAngleDegree, FlagEnumString(GetType(enumShapeHexLine), ShapeHexLine())).AppendLine()
            sb.AppendFormat("1st: Valid({0})点({1}) 中心{2} 辺({3})", Is辺方向Valid(lineIdx.i_1st), _line辺(lineIdx.i_1st).IsDot, _p辺の中心点(lineIdx.i_1st), _line辺(lineIdx.i_1st).dump()).AppendLine()
            sb.AppendFormat("2nd: Valid({0})点({1}) 中心{2} 辺({3})", Is辺方向Valid(lineIdx.i_2nd), _line辺(lineIdx.i_2nd).IsDot, _p辺の中心点(lineIdx.i_2nd), _line辺(lineIdx.i_2nd).dump())
            Return sb.ToString
        End Function

    End Class

    '2辺×3の六角形
    Friend Class CHex
        Friend Const cHexCount As Integer = 6

        'Hex値から、CHexLine の i_1st/i_2nd
        Shared ReadOnly Property hex_line(ByVal hexidx As Integer) As CHexLine.lineIdx
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
        Shared ReadOnly Property hexidx(ByVal aidx As Integer, ByVal line As CHexLine.lineIdx) As Integer
            Get
                Return line * 3 + aidx
            End Get
        End Property

        '2辺×3:AngleIndex順
        Dim _HexLine(cAngleCount - 1) As CHexLine
        '同方向の対角線:AngleIndex順
        Dim _CrossLine(cAngleCount - 1) As S線分
        '形状
        Dim _ShapeHexLine(cAngleCount - 1) As CHexLine.enumShapeHexLine

        '形状として有効
        ReadOnly Property IsValidHexagon(ByVal isAllowFlat0 As Boolean) As Boolean
            Get
                If isAllowFlat0 Then
                    '2辺が1本に重なって良いのは横方向のみ(#62)
                    If _ShapeHexLine(cIdxAngle60).HasFlag(CHexLine.enumShapeHexLine.flattened) OrElse
                    _ShapeHexLine(cIdxAngle120).HasFlag(CHexLine.enumShapeHexLine.flattened) Then
                        Return False
                    End If
                    If _ShapeHexLine(cIdxAngle0).HasFlag(CHexLine.enumShapeHexLine.flattened) Then
                        '横方向に重なるのを許す
                        Return True
                    End If
                End If
                '各方向に幅があること
                For ix As Integer = 0 To cAngleCount - 1
                    If _ShapeHexLine(ix) <> CHexLine.enumShapeHexLine.none Then
                        Return False
                    End If
                Next
                Return True

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

        '-90,-30,30,90,150,-150
        Shared ReadOnly Property delta辺の外向き法線(ByVal hexidx As Integer) As S差分
            Get
                'Return _HexLine(hex_aidx(hexidx)).delta辺の外向き法線(hex_line(hexidx))
                Dim delta As S差分 = cDeltaAxisDirection(hex_aidx(hexidx))
                If hex_line(hexidx) = CHexLine.lineIdx.i_1st Then
                    delta *= -1
                End If
                Return delta
            End Get
        End Property

        '0,60,120,180,240,300
        Shared ReadOnly Property Angle辺(ByVal hexidx As Integer) As Integer
            Get
                '_HexLine(hex_aidx(hexidx)).BandAngle(hex_line(hxidx))
                Dim angle As Integer = cBandAngleDegree(hex_aidx(hexidx))
                If hex_line(hexidx) = CHexLine.lineIdx.i_2nd Then
                    angle += 180
                End If
                Return angle
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
            '                p34 <- p23
            'fn4:60度の2nd ／         ＼ fn2:120度の1st
            '            p45           p12
            'fn5:120度の2nd＼         ／ fn1:60度の1st
            '                p50 -> p01
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

            '形状を得る
            _ShapeHexLine(cIdxAngle0) = _HexLine(cIdxAngle0).ShapeHexLine()
            _ShapeHexLine(cIdxAngle60) = _HexLine(cIdxAngle60).ShapeHexLine()
            _ShapeHexLine(cIdxAngle120) = _HexLine(cIdxAngle120).ShapeHexLine()

            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p50{0} p01{1} p12{2} p23{3} p34{4} p45{5} p50{6}", p50, p01, p12, p23, p34, p45, p50)
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang0 {0}", _HexLine(cIdxAngle0).dump())
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang60 {0}", _HexLine(cIdxAngle60).dump())
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "ang120 {0}", _HexLine(cIdxAngle120).dump())

            Return True
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("IsValidHexagon({0})({1}) ", IsValidHexagon(False), IsValidHexagon(True)).AppendLine()
            For i As Integer = 0 To cAngleCount - 1
                sb.AppendFormat("({0})HexLine {1}", i, _HexLine(i)).AppendLine()
                'sb.AppendFormat("({0})CrossLine {1}", i, _CrossLine(i)).AppendLine()
            Next
            Return sb.ToString
        End Function
        Public Function dump() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendLine()
            sb.AppendFormat("IsValidHexagon({0})({1}) ", IsValidHexagon(False), IsValidHexagon(True)).AppendLine()
            For i As Integer = 0 To cAngleCount - 1
                sb.AppendFormat("({0})HexLine {1}", cBandAngleDegree(i), _HexLine(i).dump()).AppendLine()
                sb.AppendFormat("({0})CrossLine {1}", cBandAngleDegree(i), _CrossLine(i).dump()).AppendLine()
            Next
            Return sb.ToString
        End Function


        '六角形の辺と線の交点
        Friend Class CCrossPoint
            Friend CrossPoint(CHex.cHexCount - 1) As S実座標
            Friend PointCount As Integer = 0

            Friend CrossLine As S線分
            Friend CrossLength As Double = 0

            Enum enumCrossStatus
                _none   '交点なし
                _flat   '点(つぶれた六角形)
                _point  '1点(2辺)
                _line   '長さのある線分
                _vertex '〃 (3辺)
                _vertices '〃 (4辺)
                _etc    '？
            End Enum
            Friend Status As enumCrossStatus = enumCrossStatus._none

            ReadOnly Property IsExist As Boolean
                Get
                    Return Status <> enumCrossStatus._none AndAlso Status <> enumCrossStatus._etc
                End Get
            End Property

            Sub New()
            End Sub

            Public Overrides Function ToString() As String
                Dim sb As New System.Text.StringBuilder
                sb.AppendFormat("Length={0:f2} ({1}) {2}", CrossLength, Status, CrossLine)
                Return sb.ToString
            End Function
            Public Function dump() As String
                Dim sb As New System.Text.StringBuilder
                sb.AppendFormat("{0} Count={1} Angle={2:f1}", ToString(), PointCount, CrossLine.s差分.Angle).AppendLine()
                For hidx As Integer = 0 To cHexCount - 1
                    sb.AppendFormat(" - {0} {1} ", hidx, IIf(CrossPoint(hidx).IsZero, "", CrossPoint(hidx))).AppendLine()
                Next
                Return sb.ToString
            End Function
        End Class

        '六角形の辺と線の交点を計算, ない時にはNothingを返す　※pointは内側とは限らない
        Function get辺との交点(ByVal point As S実座標, ByVal angle As Integer) As CCrossPoint
            Dim cp As New CCrossPoint

            Dim fn As New S直線式(angle, point)
            Dim diffs As New List(Of S実座標)

            With cp
                '6辺に対して
                For hxidx = 0 To CHex.cHexCount - 1
                    Dim bandAngle As Integer = CHex.Angle辺(hxidx)
                    Dim anglediff As Integer = bandAngle - angle
                    '平行方向は除外
                    If SameAngle(0, anglediff) OrElse SameAngle(180, anglediff) Then
                        Continue For
                    End If

                    .CrossPoint(hxidx) = line辺(hxidx).p交点(fn)
                    If Not .CrossPoint(hxidx).IsZero Then
                        .PointCount += 1

                        Dim exist As Boolean = False
                        For Each pd As S実座標 In diffs
                            If pd.Near(.CrossPoint(hxidx)) Then
                                exist = True
                                Exit For
                            End If
                        Next
                        If Not exist Then
                            diffs.Add(.CrossPoint(hxidx))
                        End If
                    End If
                Next

                '6辺との交点数
                If .PointCount = 0 OrElse diffs.Count = 0 Then
                    '交点がない
                    Return Nothing
                End If

                '1点
                If diffs.Count = 1 Then
                    .CrossLine = New S線分(diffs(0), diffs(0))
                    .CrossLength = 0

                    'ノーチェックで決めつけます
                    If _ShapeHexLine(cIdxAngle0).HasFlag(CHexLine.enumShapeHexLine.flattened) Then
                        .Status = CCrossPoint.enumCrossStatus._flat
                    Else
                        .Status = CCrossPoint.enumCrossStatus._point
                    End If

                ElseIf diffs.Count = 2 Then
                    .CrossLine = New S線分(diffs(0), diffs(1))
                    If Not SameAngle(angle, .CrossLine.s差分.Angle) Then
                        .CrossLine.Revert()
                    End If
                    .CrossLength = .CrossLine.Length

                    'ノーチェックで決めつけます
                    If .PointCount = 2 Then
                        .Status = CCrossPoint.enumCrossStatus._line
                    ElseIf .PointCount = 3 Then
                        .Status = CCrossPoint.enumCrossStatus._vertex
                    ElseIf .PointCount = 4 Then
                        .Status = CCrossPoint.enumCrossStatus._vertices
                    End If

                Else
                    .Status = CCrossPoint.enumCrossStatus._etc
                    'CrossLine特定不可
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "get辺との交点・CrossLine特定不可({0}) {1} {2}", angle, point, cp.dump())

                End If
            End With
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "get辺との交点({0}) {1} {2}", angle, point, cp.dump())

            Return cp
        End Function

        '指定方向の最も外側の点
        Function get最外の点(ByVal angle As Integer) As S実座標
            '辺と垂直方向
            For hxidx As Integer = 0 To CHex.cHexCount - 1
                If SameAngle(angle, Angle辺(hxidx) - 90) Then
                    Return line辺(hxidx).p中点
                End If
            Next

            Dim pPoint As S実座標 = pOrigin
            Dim cp As CCrossPoint = get辺との交点(pPoint, angle)
            Dim retry As Integer = 0

            Do While cp IsNot Nothing AndAlso cp.IsExist AndAlso retry < 20
                'g_clsLog.LogResourceMessage(clsLog.LogLevel.Debug, "get最外の点 {0}:{1}", retry, pPoint)
                pPoint = cp.CrossLine.p終了
                cp = get辺との交点(pPoint, angle + 90)
                If cp Is Nothing OrElse Not cp.IsExist Then
                    Exit Do
                End If
                If cp.Status = CCrossPoint.enumCrossStatus._point Then
                    Return cp.CrossLine.p開始
                End If
                pPoint = cp.CrossLine.p中点
                cp = get辺との交点(pPoint, angle)
                retry += 1
            Loop
            'g_clsLog.LogResourceMessage(clsLog.LogLevel.Debug, "get最外の点(Exit Loop) {0}:{1}", retry, pPoint)
            Return pPoint

        End Function

        '六角形の内側にある時Trueを返す
        Function is内側(ByVal point As S実座標) As Boolean
            '3方向でチェック
            For Each angle As Integer In cBandAngleDegree
                Dim cp As CCrossPoint = get辺との交点(point, angle + 90)
                If cp Is Nothing OrElse Not cp.IsExist Then
                    Return False
                End If
                If Not cp.CrossLine.IsOn(point) Then
                    Return False
                End If
            Next
            Return True
        End Function

    End Class



    'enum値のdump
    Friend Shared Function FlagEnumString(ByVal enumtype As Type, ByVal val As Object) As String
        If enumtype.IsEnum Then
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0}({1:X})", enumtype.Name, CType(val, Integer))
            If CType(val, Integer) = 0 Then
                sb.Append("-")
            Else
                For Each flag In [Enum].GetValues(enumtype)
                    If CType(flag, Integer) <> 0 Then
                        If val.HasFlag(flag) Then
                            sb.Append(flag.ToString()).Append(" ")
                        End If
                    End If
                Next
            End If
            Return sb.ToString
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "ひものセット、3方向"

    '各バンド
    Friend Class CBandPosition
        Dim _parent As CBandPositionList    '方向情報
        Dim m_iひも番号 As Integer

        Friend m_row縦横展開 As tbl縦横展開Row
        Friend m_dひも幅 As Double
        Friend m_pひもの中心 As S実座標
        Friend m_d合わせ位置からの幅 As Double '合わせライン上の中心位置との差

        Friend m_cp底の辺 As CHex.CCrossPoint = Nothing   '底との交点
        Friend m_cp最外六角形 As CHex.CCrossPoint = Nothing   '最外六角形との交点

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

        Friend ReadOnly Property BandAngle As Integer
            Get
                Return _parent.BandAngleDegree
            End Get
        End Property

        Sub ReSet底の交点()
            m_cp底の辺 = Nothing
            m_cp最外六角形 = Nothing
        End Sub

        Function IsSet底の交点Set() As Boolean
            Return m_cp底の辺 IsNot Nothing AndAlso
                m_cp底の辺.Status <> CHex.CCrossPoint.enumCrossStatus._none
        End Function

        '六角形との交点情報
        Function Set底の交点(ByVal cp As CHex.CCrossPoint, ByVal cp0 As CHex.CCrossPoint) As Boolean
            m_cp底の辺 = cp
            m_cp最外六角形 = cp0
            If cp Is Nothing OrElse cp0 Is Nothing Then
                Return False
            End If
            'save
            If m_row縦横展開 IsNot Nothing Then
                m_row縦横展開.f_d長さ = m_cp底の辺.CrossLength
            End If
            Return True
        End Function

        '底ひも(加算して描画)
        Function ToBand() As CBand
            If m_row縦横展開 Is Nothing OrElse Not IsSet底の交点Set() Then
                Return Nothing
            End If

            Dim band = New CBand(m_row縦横展開)

            'f_dVal1に加算の側、f_dVal2に加算2の側(by adjust_展開ひも)
            Dim pA As S実座標 = m_cp底の辺.CrossLine.p開始
            Dim pB As S実座標 = m_cp底の辺.CrossLine.p終了
            If _parent.SameDirectionAddLength Then
                pA = pA + _parent.DeltaBandDirection * -m_row縦横展開.f_dVal1
                pB = pB + _parent.DeltaBandDirection * m_row縦横展開.f_dVal2
            Else
                pA = pA + _parent.DeltaBandDirection * -m_row縦横展開.f_dVal2
                pB = pB + _parent.DeltaBandDirection * m_row縦横展開.f_dVal1
            End If

            'バンド描画位置
            band.SetBand(New S線分(pA, pB), m_dひも幅, _parent.DeltaAxisDirection)

            '記号描画位置(現物合わせ) #60
            If IsDrawMarkCurrent Then
                Dim mark As enumMarkPosition = enumMarkPosition._なし
                Dim delta As S差分
                If _parent.BandAngleDegree = 120 Then
                    mark = enumMarkPosition._終点の後
                    delta = New S差分(-m_dひも幅 / 4, -m_dひも幅 * 2 / 3)
                ElseIf _parent.BandAngleDegree = 60 Then
                    mark = enumMarkPosition._始点の前
                    delta = New S差分(-m_dひも幅 / 3, 0)
                ElseIf _parent.BandAngleDegree = 0 Then
                    mark = enumMarkPosition._始点Fの前
                End If
                band.SetMarkPosition(mark, m_dひも幅, delta)
            End If

            Return band
        End Function


        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0} ひも幅({1:f1})", Ident, m_dひも幅).Append(vbTab)
            sb.AppendFormat("合わせ位置からの幅:({0:f1}) ひもの中心{1}", m_d合わせ位置からの幅, m_pひもの中心).Append(vbTab)
            If m_row縦横展開 IsNot Nothing Then
                sb.AppendFormat("row縦横展開:({0},{1},{2}){3}本幅 ", FlagEnumString(GetType(enumひも種), CType(m_row縦横展開.f_iひも種, enumひも種)), m_row縦横展開.f_iひも番号, m_row縦横展開.f_i位置番号, m_row縦横展開.f_i何本幅)
            Else
                sb.Append("No row縦横展開 ")
            End If
            If m_cp底の辺 IsNot Nothing Then
                sb.AppendFormat("cp底の辺{0}", m_cp底の辺.ToString)
            End If
            If m_cp最外六角形 IsNot Nothing Then
                'sb.AppendFormat("cp最外六角形{0}", m_cp最外六角形.dump())
                sb.AppendFormat("cp最外六角形{0}", m_cp最外六角形.ToString)
            End If
            Return sb.ToString
        End Function

    End Class

    '各方向のセット
    Friend Class CBandPositionList
        Implements IEnumerable(Of CBandPosition)

        'セットに対する定数値
        Dim _AngleIndex As AngleIndex
        Friend BandAngleDegree As Integer 'バンドの方向角
        Friend DeltaBandDirection As S差分 'バンドの方向
        Friend DeltaAxisDirection As S差分 'バンドの軸方向
        Friend SameDirectionAddLength As Boolean 'ひも長加算とバンドの方向角


        'ひも番号順のリスト
        Dim _BandList As New List(Of CBandPosition)     'Idx=1(要素0),Idx=2(要素1)
        '補強ひも
        Dim _row補強ひも(ciひも番号_クロス) As tbl縦横展開Row '0は使わない

        '基本値の保持
        Dim _iひもの本数 As Integer
        Dim _i何個目位置 As Integer  '設定した合わせ目の位置、1～ひもの本数-1
        Dim _d六つ目の高さc As Double
        Dim _bひも中心合わせc As Boolean
        Dim _b端の目あり As Boolean

        '集計結果
        Friend _b本幅変更あり As Boolean
        Friend _d底領域幅 As Double  '目の数,端の目,ひも幅計,六つ目
        Friend _d合わせ位置までの幅 As Double  '軸方向の1から

        'バンドに平行な2辺
        Friend _hln最外ひもの2辺 As CHexLine
        Friend _hln外目幅の中心の2辺 As CHexLine
        Friend _hln底の2辺 As CHexLine
        Friend _hln底の2辺に厚さ As CHexLine
        Friend _hln側面上2辺 As CHexLine

        '角位置
        Friend _i角位置(HexLineCount - 1) As Integer '角のひも番号位置(idx～idx+1間)
        Friend _fn角の目(HexLineCount - 1) As S直線式 '五角形の中心ライン(バンドに直交)


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
            SameDirectionAddLength = cSameDirectionWithDialog(idx(aidx))

            _hln最外ひもの2辺 = New CHexLine(Me)
            _hln外目幅の中心の2辺 = New CHexLine(Me)
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
            If table Is Nothing Then
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

            _i何個目位置 = i何個目位置
            _d六つ目の高さc = d六つ目の高さ
            _b端の目あり = (g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d目と数える端の目") <= d端の目)
            _bひも中心合わせc = bひも中心合わせ

            '条件確認
            If 0 < _iひもの本数 Then
                If _iひもの本数 < 2 OrElse d六つ目の高さ < 0 OrElse
                    i何個目位置 < 1 OrElse _iひもの本数 < i何個目位置 Then
                    Return False
                End If
                If Not bひも中心合わせ AndAlso _iひもの本数 = i何個目位置 Then
                    Return False
                End If
            Else
                '#62 チェック済
                '横ひも, _iひもの本数=0, i何個目位置=0, d端の目=0
            End If

            Dim d端の目幅 As Double = d六つ目の高さ * d端の目
            Dim i合わせAxisIdx As Integer = AxisIdx(i何個目位置)

            '目の中心で合わせる場合(bひも中心合わせ=False)
            '       ひも番号      本数  本数-1    ▽合わせ位置         2     1  
            '       軸方向          1     2       ▽合わせ位置       本数-1 本数
            '                <-----|+|---|+|---      --|+|--       ---|+|---|+|------>
            '                端の目 紐 目 紐 目      目 紐 目       目 紐 目 紐 端の目
            ' d端からの長さ  |→　  +     +             +              +     +     →|d幅の計
            '                |→                  △d端から合わせ位置まで
            '     合わせ位置からの幅(マイナス) ← | → 合わせ位置からの幅(プラス)
            '

            'ひも中心で合わせる場合(bひも中心合わせ=True)
            '       ひも番号      本数  本数-1     ▽             2     1
            '                                   合わせ位置
            '       軸方向          1     2        ▽           本数-1 本数
            '                <-----|+|---|+|--   -|+|--       ---|+|---|+|------>
            '                端の目 紐 目 紐 目    紐 目       目 紐 目 紐 端の目
            ' d端からの長さ  |→　  +     +        +              +     +     →|d幅の計
            '                |→                   △d端から合わせ位置まで
            '     合わせ位置からの幅(マイナス)  ← | → 合わせ位置からの幅(プラス)

            Dim d端からの長さ(_iひもの本数) As Double
            Dim d端から合わせ位置まで As Double = 0
            '軸方向に累計
            Dim d幅の計 As Double = d端の目幅
            For axis As Integer = 1 To _iひもの本数
                Dim band As CBandPosition = ByAxis(axis)
                Dim row As tbl縦横展開Row = band.m_row縦横展開

                band.m_dひも幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
                d端からの長さ(axis) = d幅の計 + band.m_dひも幅 / 2

                '合わせ位置
                If axis = i合わせAxisIdx Then
                    If bひも中心合わせ Then
                        'ひもの真ん中
                        d端から合わせ位置まで = d端からの長さ(axis)
                    Else
                        '目の真ん中: ひも番号に対して逆方向なのでマイナスする
                        d端から合わせ位置まで = d幅の計 - (d六つ目の高さ / 2)
                    End If

                End If

                d幅の計 += band.m_dひも幅
                If axis < _iひもの本数 Then
                    d幅の計 += d六つ目の高さ
                End If
            Next
            d幅の計 += d端の目幅

            '合わせ位置との差分
            _d底領域幅 = d幅の計
            _d合わせ位置までの幅 = d端から合わせ位置まで
            Dim p底の辺の中心1 As S実座標 = pOrigin + DeltaAxisDirection * -d端から合わせ位置まで
            For axis As Integer = 1 To _iひもの本数
                Dim band As CBandPosition = ByAxis(axis)

                band.m_d合わせ位置からの幅 = d端からの長さ(axis) - d端から合わせ位置まで
                band.m_pひもの中心 = pOrigin + DeltaAxisDirection * band.m_d合わせ位置からの幅
            Next
            Dim p底の辺の中心2 As S実座標 = pOrigin + DeltaAxisDirection * (d幅の計 - d端から合わせ位置まで)
            _hln底の2辺.SetCenters(p底の辺の中心1, p底の辺の中心2)

            '最も外側のひもの中心, 最も外側のひもの外側+(六つ目幅/2)
            If _iひもの本数 = 0 Then '#62
                _hln最外ひもの2辺.SetCenters(pOrigin, pOrigin)
                _hln外目幅の中心の2辺.SetCenters(pOrigin - DeltaAxisDirection * (d六つ目の高さ / 2), pOrigin + DeltaAxisDirection * (d六つ目の高さ / 2))
            Else
                _hln最外ひもの2辺.SetCenters(ByAxis(1).m_pひもの中心, ByAxis(_iひもの本数).m_pひもの中心)
                _hln外目幅の中心の2辺.SetCenters(ByAxis(1).m_pひもの中心 - DeltaAxisDirection * (ByAxis(1).m_dひも幅 / 2 + (d六つ目の高さ / 2)),
                                      ByAxis(_iひもの本数).m_pひもの中心 + DeltaAxisDirection * (ByAxis(_iひもの本数).m_dひも幅 / 2 + (d六つ目の高さ / 2)))
            End If
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

        '角位置をセットする
        Function Set角位置(ByVal isUse As Boolean, ByVal crossline As S線分) As Boolean
            _i角位置(lineIdx.i_1st) = -1
            _i角位置(lineIdx.i_2nd) = -1
            If Not isUse Then
                Return True
            End If

            Dim iCornerPre As Integer = Modulo(idx(_AngleIndex) + 4, 6)   '4
            Dim iCornerPst As Integer = Modulo(iCornerPre + 1, 6)   '5

            Dim iLast1st As Integer = -1
            For idx As Integer = 1 To _iひもの本数
                Dim bandposition As CBandPosition = ByIdx(idx)
                If bandposition.m_cp最外六角形 IsNot Nothing Then
                    If Not bandposition.m_cp最外六角形.CrossPoint(iCornerPre).IsZero Then
                        iLast1st = idx
                    ElseIf Not bandposition.m_cp最外六角形.CrossPoint(iCornerPst).IsZero Then
                        Exit For
                    End If
                End If
            Next
            _i角位置(lineIdx.i_1st) = iLast1st

            iCornerPre = Modulo(idx(_AngleIndex) + 2, 6)   '2
            iCornerPst = Modulo(iCornerPre - 1, 6)  '1

            Dim iLast2nd As Integer = -1
            For idx As Integer = 1 To _iひもの本数
                Dim bandposition As CBandPosition = ByIdx(idx)
                If bandposition.m_cp最外六角形 IsNot Nothing Then
                    If Not bandposition.m_cp最外六角形.CrossPoint(iCornerPre).IsZero Then
                        iLast2nd = idx
                    ElseIf Not bandposition.m_cp最外六角形.CrossPoint(iCornerPst).IsZero Then
                        Exit For
                    End If
                End If
            Next
            _i角位置(lineIdx.i_2nd) = iLast2nd

            '五角形の中心ライン(バンドに直交)
            _fn角の目(lineIdx.i_1st) = New S直線式(BandAngleDegree + 90, crossline.p開始)
            _fn角の目(lineIdx.i_2nd) = New S直線式(BandAngleDegree + 90, crossline.p終了)

            Return 0 <= _i角位置(lineIdx.i_1st) AndAlso 0 <= _i角位置(lineIdx.i_2nd)
        End Function

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

        '* 差しひも
        '
        '   CBandPosition
        '       ひも番号  :n(1～本数)
        '
        '       m_dひも幅
        '        <--1-->           <--2-->               <--n-->                          <--本数-->
        '<--端-->|紐(1)|<--六つ目->|紐(2)|<--六つ目->....|紐(n)|<--六つ目->....<--六つ目->|紐(本数)|<--端-->
        '          ↑                ↑                     ↑                                ↑
        '     m_pひもの中心(1) m_pひもの中心(2)       m_pひもの中心(n)                 m_pひもの中心(本数)
        '
        '端の目無し         *(1)              *(2)                  *(n)            *(本数-1)              
        '端の目あり
        ' *(1)              *(2)              *(3)                  *(n+1)          *(本数)            *(本数+1)            
        '  ※設定によっては六角形の外　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　※


        '端の目を加味した差しひもの数(バンドに平行方向)
        Friend ReadOnly Property InsertCount(ByVal is全面 As Boolean) As Integer
            Get
                If _b端の目あり Then
                    Return _iひもの本数 + 1 '両サイドにプラス
                Else
                    Return _iひもの本数 - 1 'ひも間の数
                End If
            End Get
        End Property

        '1～InsertCount
        Function getInsertBandPoint(ByVal idx As Integer, ByVal is全面 As Boolean, ByVal dInnerPosition As Double, ByRef pPoint As S実座標) As Boolean
            If idx < 1 OrElse InsertCount(is全面) < idx Then
                Return False
            End If

            Dim iBand As Integer = idx
            Dim isLast As Boolean = False
            If idx = (_iひもの本数 + 1) Then '_b端の目あり の時の最後
                isLast = True
                iBand = idx - 1
            End If

            Dim band As CBandPosition = ByIdx(iBand)
            If band Is Nothing Then
                Return False
            End If

            Dim d As Double
            If _b端の目あり Then
                If isLast Then
                    'band位置の後
                    d = band.m_dひも幅 / 2 + _d六つ目の高さc * dInnerPosition
                Else
                    'band位置の前
                    d = _d六つ目の高さc * (-1 + dInnerPosition) - band.m_dひも幅 / 2
                End If
            Else
                'band位置の後
                d = band.m_dひも幅 / 2 + _d六つ目の高さc * dInnerPosition
            End If
            'Dim p As S実座標 = band.m_pひもの中心 + DeltaAxisDirection * -d
            'pPoint.Copy(p)
            pPoint = band.m_pひもの中心 + DeltaAxisDirection * -d

            Return True
        End Function

        '全面、角ならば五角形の中心ラインを返す 1～InsertCount
        Function getIsPentagon(ByVal idx As Integer, ByVal lidx As lineIdx, ByRef fn As S直線式) As Boolean
            If _bひも中心合わせc OrElse idx < 1 OrElse InsertCount(True) < idx Then 'is全面=True
                Return False
            End If

            Dim iBand As Integer
            If _b端の目あり Then
                iBand = idx - 1
            Else
                iBand = idx
            End If
            Dim i角位置 As Integer = _i角位置(lidx)
            If i角位置 < 0 OrElse i角位置 <> iBand Then
                Return False
            End If

            '*対応する角位置にある
            fn = _fn角の目(lidx)
            Return True
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("Direction={0} BandAngleDegree={1} ひもの本数={2} 何個目位置={3} ", _AngleIndex, BandAngleDegree, _iひもの本数, _i何個目位置)
            sb.AppendFormat("本幅変更{0} 端の目{1} ", IIf(_b本幅変更あり, "あり", "なし"), IIf(_b端の目あり, "あり", "なし")).AppendLine()
            sb.AppendFormat("_底領域幅={0:f2} _中心までの幅={1:f2}", _d底領域幅, _d合わせ位置までの幅).AppendLine()
            sb.Append("最外ひもの2辺:").Append(_hln最外ひもの2辺).AppendLine()
            sb.Append("底の2辺:").Append(_hln底の2辺).AppendLine()
            sb.Append("底の2辺に厚さ:").Append(_hln底の2辺に厚さ).AppendLine()
            sb.Append("側面上2辺:").Append(_hln側面上2辺).AppendLine()
            sb.Append("角位置:").Append(_i角位置(lineIdx.i_1st)).Append(":").Append(_i角位置(lineIdx.i_2nd)).AppendLine()
            sb.Append("fn角の目:").Append(_fn角の目(lineIdx.i_1st)).Append(":").Append(_fn角の目(lineIdx.i_2nd)).AppendLine()

            For Each band As CBandPosition In _BandList
                sb.AppendLine(band.ToString)
            Next
            Return sb.ToString
        End Function

        Public Function GetEnumerator() As IEnumerator(Of CBandPosition) Implements IEnumerable(Of CBandPosition).GetEnumerator
            Return _BandList.GetEnumerator()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function
    End Class
#End Region

#Region "編みひもの情報"
    '差しひも・プレビューで使用します
    '別途計算済の値と重複しますが、比較チェックはしません

    Friend Class CSideBand
        Friend m_row側面 As tbl側面Row
        Friend m_d周長比率対底の周 As Double
        Friend m_dひも幅 As Double
        Friend m_dひも下の高さ As Double

        Sub New()
            m_row側面 = Nothing
            m_dひも下の高さ = 0
        End Sub

        Sub New(ByVal r As tbl側面Row, ByVal d As Double)
            m_row側面 = r
            m_dひも幅 = g_clsSelectBasics.p_d指定本幅(r.f_i何本幅)
            m_d周長比率対底の周 = r.f_d周長比率対底の周
            m_dひも下の高さ = d
        End Sub

        'line:ゼロ位置の線分 delta:辺の外向き法線
        Function ToBand(ByVal line As S線分, ByVal delta As S差分, ByVal isMark As Boolean) As CBand
            Dim band = New CBand(m_row側面)

            'ゼロ位置からの高さを加える
            line += delta * (m_dひも下の高さ)
            band.SetBandF(line, m_dひも幅, delta)
            band.SetLengthRatio(m_d周長比率対底の周)
            If isMark Then
                '(コード固定・上側面が指定されているため)
                band.p文字位置 = band.p終点F
            End If
            band.is始点FT線 = False
            band.is終点FT線 = False

            Return band
        End Function

        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("ひも下の高さ({0:f1}) ひも幅({1:f1}) 周長比率対底の周({2}) ", m_dひも下の高さ, m_dひも幅, m_d周長比率対底の周)
            If m_row側面 IsNot Nothing Then
                sb.AppendFormat("row側面:({0},{1}){2}本幅", m_row側面.f_i番号, m_row側面.f_iひも番号, m_row側面.f_i何本幅)
            Else
                sb.Append("No row側面")
            End If
            Return sb.ToString
        End Function
    End Class

    Friend Class CSideBandList
        Dim _Ary() As CSideBand
        Dim _iひも本数 As Integer '_i側面の編みひも数
        Dim _is最下段に目あり As Boolean
        Dim _d側面の六つ目の高さ As Double

        '番号    ひも番号(展開なし):(展開あり)  n=_i側面の編みひも数
        'cHemNumber                                                     
        '            縁 ==================================　← m_dひも下の高さ(n+1)
        '                                 ↑get側面の六つ目の高さ()            →端の目がない時のn,ある時のn+1
        '                      ------------------------
        'cIdxHeight   1:n           側面のひも(n) _Ary(n)                               
        '                      ------------------------　← m_dひも下の高さ(n)
        '                        ....
        '                        ....
        '                      ------------------------
        'cIdxHeight   1:2           側面のひも(2) _Ary(2)                                          
        '                      ------------------------　← m_dひも下の高さ(2) 
        '                                 ↑get側面の六つ目の高さ()             →端の目がない時の1,ある時の2
        '                      ------------------------
        'cIdxHeight   1:1           側面のひも(1) _Ary(1)                                         
        '                      ------------------------　← m_dひも下の高さ(1)
        'cIdxSpace                        ↑_d最下段の目 * _d六つ目の高さ       →端の目がある時の1
        '            底 ==================================　← m_dひも下の高さ(0)=0 ※底の厚さは加えない

        Sub Clear()
            ReDim _Ary(0)   '使用時に最Redim前提
            _iひも本数 = 0
            _is最下段に目あり = False
            _d側面の六つ目の高さ = 0
        End Sub

        Sub SetCount(ByVal iひも本数 As Integer, ByVal is最下段に目あり As Boolean, ByVal d側面の六つ目の高さ As Double)
            _iひも本数 = iひも本数
            _is最下段に目あり = is最下段に目あり
            _d側面の六つ目の高さ = d側面の六つ目の高さ
            ReDim _Ary(iひも本数 + 1) 'Clear
        End Sub

        '指定位置の要素 下からの数:1～本数+1, 0は最下段のスペース
        Friend Property At(ByVal idx As Integer) As CSideBand
            Get
                If _Ary Is Nothing OrElse idx < 0 OrElse _Ary.Count <= idx Then
                    Return Nothing
                End If
                Return _Ary(idx)
            End Get
            Set(value As CSideBand)
                If idx < 0 Then
                    Exit Property
                End If
                If _Ary Is Nothing Then
                    ReDim _Ary(idx + 1)
                ElseIf _Ary.Count <= idx Then
                    ReDim Preserve _Ary(idx + 1)
                End If
                _Ary(idx) = value
            End Set
        End Property

        '最下段の目を加味したさしひも位置の数
        Friend ReadOnly Property InsertCount() As Integer
            Get
                If _is最下段に目あり Then
                    Return _iひも本数 + 1
                Else
                    Return _iひも本数
                End If
            End Get
        End Property

        '全て同じ周長比率対底の周
        Friend ReadOnly Property AllSameRatio() As Double
            Get
                If InsertCount() < 1 Then
                    Return -1
                ElseIf _iひも本数 = 0 Then
                    Return 1
                End If
                Dim ratio As Double = _Ary(1).m_d周長比率対底の周
                For i As Integer = 2 To _iひも本数
                    If ratio <> _Ary(i).m_d周長比率対底の周 Then
                        Return -1
                    End If
                Next
                Return ratio
            End Get
        End Property

        '最下段の目を加味した指定位置の高さ dInnerPosition:同幅内位置(0～1値・中央が0.5)
        Friend Function GetHeight(ByVal idx As Integer, ByVal dInnerPosition As Double, ByRef dHeight As Double) As Boolean
            If idx < 1 OrElse InsertCount < idx Then
                Return False
            End If
            Dim sand As CSideBand = _Ary(idx)
            If _is最下段に目あり Then
                'ひもより下の位置
                dHeight = sand.m_dひも下の高さ - _d側面の六つ目の高さ * (1 - dInnerPosition)
            Else
                'ひもより上の位置
                dHeight = sand.m_dひも下の高さ + sand.m_dひも幅 + _d側面の六つ目の高さ * dInnerPosition
            End If
            Return True
        End Function

        '最下段の目を加味した指定位置の周長比率対底の周
        Friend Function GetRatio(ByVal idx As Integer, ByVal dInnerPosition As Double) As Double
            If idx < 1 OrElse InsertCount < idx Then
                Return 1 '既定値
            End If
            Dim sand As CSideBand = _Ary(idx)
            If _is最下段に目あり Then
                'ひもより下の位置
                Dim sand0 As CSideBand = _Ary(idx - 1)
                Return sand0.m_d周長比率対底の周 +
                    (sand.m_d周長比率対底の周 - sand0.m_d周長比率対底の周) * dInnerPosition
            Else
                'ひもより上の位置
                Dim sand1 As CSideBand = _Ary(idx + 1)
                Return sand.m_d周長比率対底の周 +
                    (sand1.m_d周長比率対底の周 - sand.m_d周長比率対底の周) * dInnerPosition
            End If
        End Function


        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.Append("CSideBandList")
            If _Ary IsNot Nothing Then
                sb.AppendFormat(" Count={0}", _Ary.Count)
                For i As Integer = 0 To _Ary.Count - 1
                    Dim band As CSideBand = _Ary(i)
                    If band IsNot Nothing Then
                        sb.AppendLine().AppendFormat("{0} {1}", i, band.ToString)
                    End If
                Next
            End If
            Return sb.ToString
        End Function
    End Class


    '側面のひもの情報をセットする
    Private Function setSideBandList() As Boolean
        '基本計算は終わっていること
        If _CalcStatus <> CalcStatus._reflect Then
            Return False
        End If

        _CSideBandList.SetCount(_i側面の編みひも数, (g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d目と数える端の目") <= _d最下段の目), get側面の六つ目の高さ())

        Dim d高さ As Double = 0
        '側面のレコード→1本ごとのバンド　
        '1～本数を追加
        Dim idx = 1
        For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, "f_i番号 ASC , f_iひも番号 ASC")
            If r.f_i番号 = cIdxSpace Then
                '最下段の目の高さがセットされている
                _CSideBandList.At(0) = New CSideBand(r, d高さ)
                d高さ += r.f_d高さ

            ElseIf r.f_i番号 = cIdxHeight Then
                For i As Integer = 1 To r.f_iひも本数
                    Dim band As New CSideBand(r, d高さ)
                    _CSideBandList.At(idx) = band

                    d高さ += (band.m_dひも幅 + get側面の六つ目の高さ())
                    idx += 1
                Next

            Else 'f_i番号 = cHemNumber
                '縁は別途(複数レコード構成のため)
                Exit For
            End If
        Next

        '縁位置
        If 1 < idx Then
            Dim edge As New CSideBand
            edge.m_dひも下の高さ = d高さ
            edge.m_d周長比率対底の周 = _d縁の周長比率対底の周
            _CSideBandList.At(idx) = edge
        End If

        'ゼロ位置
        Dim space As CSideBand = _CSideBandList.At(0)
        If space Is Nothing Then
            space = New CSideBand
            _CSideBandList.At(0) = space
        End If
        space.m_d周長比率対底の周 = 1
        Dim ret As Boolean = (idx = (_i側面の編みひも数 + 1))

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_CSideBandList({0})={1}", ret, _CSideBandList.ToString)
        Return ret
    End Function
#End Region

#Region "底ひも直角方向の情報"
    '直角方向の差しひもで使用します。

    '底ひも直角方向
    Friend Class CBottomRightAngle
        '角度
        Friend _angle差しひも方向 As Integer
        Friend _angle基準点ライン As Integer

        '底の差しひもの基準点
        Friend _List底の基準点 As New List(Of S実座標)
        '全体の差しひもの基準点
        Friend _List全面の基準点 As New List(Of S実座標)
        '基準ライン方向の六つ目1個分
        Friend _delta六つ目 As S差分

        'バンドの中央点,カウント方向
        Friend _line最外六角形の最小最大 As S線分  '形状によっては六角形の外に出る


        'バンド=基本のひも幅を前提として計算
        '
        '  (*) *   *    *   *   *  ...  差し位置　　　　底の場合:  底の辺の内側分
        '             +─+                              全体の場合:角の五角形位置まで
        '          |／　　＼|
        '          |＼　　／|
        '      |  +─+ ─ +─+  |
        ' 角の |／　　＼／　　＼|       ↑
        '五角形|＼　　／＼　　／|       ↓六つ目の高さ
        '      |  +─+    +─+  |
        '   │ |     
        '   │ | ←---→ ←---→ 対角線幅 
        '   │ |     ←---→                
        '   │↑最外六角形
        '   ↑底の辺
        '   
        '                                   差しひも方向
        '                                      ┃
        '                    ＼＼／／      ＼＼┃／／        angle差しひも方向
        '                       ◇             ◇               ↑
        '                    ／／＼＼      ／／┃＼＼           ↑
        '            ＝＝＝＝＝＝＝＝＝＝＝＝＝┃＝＝＝＝＝     ・→→ angle基準点ライン
        '            ＼＼／／       ＼＼／／   ┃   ＼＼／／          (Idx方向)
        '               ◇             ◇      ┃     ◇             基準点ライン
        '            ／／＼＼       ／／＼＼   ┃  ／／＼＼
        '                                      ┃
        '               ※     ※      ※      ※     ※      基準点
        '               <-<---------->->
        '                ⇑            ⇑(基本のひも幅/2)/SIN60
        '                 0--  0.5 --1 
        '                  dInnerPosition は対角線幅を基準にする
        '


        '角度の選択  │   90度Hex │  150度Hex │   30度Hex  │ 
        'クラス指定  │ cIdxAngle0│cIdxAngle60│cIdxAngle120│ 
        '基準点ライン│    0度 　 │    60度   │   -60度    │=SameDirectionWithDialogで BandAngleの+/-
        '差しひも方向│    90度   │   150度   │    30度    │=基準点ライン+90
        '                                                      

        Sub New()
        End Sub

        Sub Clear()
            _List底の基準点.Clear()
            _List全面の基準点.Clear()
        End Sub

        Function InsertCount(ByVal is全面 As Boolean) As Integer
            If is全面 Then
                Return _List全面の基準点.Count
            Else
                Return _List底の基準点.Count
            End If
        End Function

        '最下段の目を加味した指定位置の高さ dInnerPosition:同幅内位置(0～1値・中央が0.5)
        Friend Function GetPoint(ByVal idx As Integer, ByVal is全面 As Boolean, ByVal dInnerPosition As Double, ByRef pPoint As S実座標) As Boolean
            If idx < 1 OrElse InsertCount(is全面) < idx Then
                Return False
            End If
            Dim p As S実座標
            If is全面 Then
                p = _List全面の基準点(idx - 1)
            Else
                p = _List底の基準点(idx - 1)
            End If
            '基準点を中央として
            pPoint = p + _delta六つ目 * (dInnerPosition - 0.5)
            Return True
        End Function


        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0} 差しひも方向({1}) 基準点ライン{2} ", Me.GetType().Name, _angle差しひも方向, _angle基準点ライン)
            sb.AppendFormat("_delta六つ目{0}  最外六角形の最小最大{1} ", _delta六つ目, _line最外六角形の最小最大)
            sb.AppendFormat("_List全面の基準点={0} ", _List全面の基準点.Count)
            For i As Integer = 0 To _List底の基準点.Count - 1
                sb.AppendFormat("{0}{1} ", i, _List底の基準点(i))
            Next
            sb.AppendLine()
            sb.AppendFormat("_List全面の基準点={0} ", _List全面の基準点.Count)
            For i As Integer = 0 To _List全面の基準点.Count - 1
                sb.AppendFormat("{0}{1} ", i, _List全面の基準点(i))
            Next
            Return sb.ToString
        End Function
    End Class

    Private Function setBottomRightAngle() As Boolean
        '基本計算は終わっていること
        If _CalcStatus <> CalcStatus._reflect Then
            Return False
        End If

        '基準点ライン方向の最小と最大を取得し、単位を元に基準点のリストを得る
        For aidx As Integer = 0 To cAngleCount - 1
            '再セットするのでクリア
            _BottomRightAngle(aidx).Clear()

            '既知のため固定値計算
            Dim d対角線幅 As Double = _d六つ目の高さ / SIN60
            Dim dひも幅 As Double = _d基本のひも幅 / SIN60
            '単位幅
            Dim unit As Double = (d対角線幅 + dひも幅) '(ひも幅/2)+目+(ひも幅/2)

            '基準点ライン方向
            Dim angle基準点ライン As Integer = cBandAngleDegree(aidx)
            If Not cSameDirectionWithDialog(aidx) Then
                angle基準点ライン += 180
            End If
            Dim angle差しひも方向 As Integer = angle基準点ライン + 90
            Dim fn基準点ライン As New S直線式(angle基準点ライン, pOrigin)

            '最小点
            Dim pmin0 As S実座標 = _hex最外六角形.get最外の点(angle基準点ライン - 180)
            'pmin0から基準点ラインへの垂線の交点
            Dim pmin As S実座標 = fn基準点ライン.p直交点(pmin0)

            '最大点
            Dim pmax0 As S実座標 = _hex最外六角形.get最外の点(angle基準点ライン)
            'pmin0から基準点ラインへの垂線の交点
            Dim pmax As S実座標 = fn基準点ライン.p直交点(pmax0)

            '最小～最大間に何個の差し位置があるか
            Dim min2max As New S線分(pmin, pmax)
            Dim div As Double = min2max.Length / unit * 2   '単位幅あたり2点
            Dim iDiv As Integer = div

            Dim cp As CCrossPoint
            Dim delta As New S差分(angle基準点ライン)
            delta *= (unit / 2) '単位幅あたり2点
            Dim p As S実座標

            '最小点より前、底の六角形に差せる差しひも
            Dim point_before As New List(Of S実座標)
            p = pmin - delta
            cp = _hex底の辺.get辺との交点(p, angle差しひも方向)
            Do While cp IsNot Nothing AndAlso cp.IsExist
                point_before.Add(p)
                p -= delta
                cp = _hex底の辺.get辺との交点(p, angle差しひも方向)
            Loop

            '最大点より後、底の六角計とクロスする線
            Dim point_after As New List(Of S実座標)
            p = pmax + delta
            cp = _hex底の辺.get辺との交点(p, angle差しひも方向)
            Do While cp IsNot Nothing AndAlso cp.IsExist
                point_after.Add(p)
                p += delta
                cp = _hex底の辺.get辺との交点(p, angle差しひも方向)
            Loop

            '底については辺の六角形とのクロスすべて
            For i As Integer = point_before.Count - 1 To 0 Step -1
                _BottomRightAngle(aidx)._List底の基準点.Add(point_before(i))
            Next

            '全面についてはひとつ前まで(クロスによらず)
            _BottomRightAngle(aidx)._List全面の基準点.Add(pmin - delta)

            '線上の点は両方
            Dim sdiv As S差分 = min2max.s差分 * (1 / iDiv)
            For i As Integer = 0 To iDiv
                Dim p1 As S実座標 = pmin + sdiv * i
                _BottomRightAngle(aidx)._List底の基準点.Add(p1)
                _BottomRightAngle(aidx)._List全面の基準点.Add(p1)
            Next

            '底については辺の六角形とのクロスすべて
            _BottomRightAngle(aidx)._List底の基準点.AddRange(point_after)

            '全面についてはひとつ前まで(クロスによらず)
            _BottomRightAngle(aidx)._List全面の基準点.Add(pmax + delta)

            '保存値
            _BottomRightAngle(aidx)._angle基準点ライン = angle基準点ライン
            _BottomRightAngle(aidx)._angle差しひも方向 = angle差しひも方向
            _BottomRightAngle(aidx)._line最外六角形の最小最大 = min2max
            _BottomRightAngle(aidx)._delta六つ目 = New S差分(angle基準点ライン) * d対角線幅

            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, " BottomRightAngle({0})={1}", aidx, _BottomRightAngle(aidx))
        Next

        Return True
    End Function
#End Region

#Region "側面の情報"
    '差しひも・プレビューで使用します

    '各側面
    Friend Class CSidePlate
        Dim _MyHexIndex As Integer

        '  _BandList120    (角は両方のリストに入る前提)    _BandList60
        '             ＼／     ＼／     ＼／             ＼／
        '             ／＼     ／＼     ／＼             ／＼
        '           ×----×-×----×-×----×-- - - --×----×  →開始方向

        '各方向、ひも番号順に保持
        Friend _BandList60 As New List(Of CBandPosition)    'こちらを使用
        Friend _BandList120 As New List(Of CBandPosition)   '予備用


        '上側面から順に回る
        Friend Shared PlateOrderHexIndex() As Integer = {3, 2, 1, 0, 5, 4}
        '
        '差しひもの指定角度
        'enum角度Hex  0,i_30度H,i_60度H,i_90度H,i_120度H,i_150度H
        Friend Shared SideAngleSelected() As Integer = {0, 30, 60, 90, 120, 150}

        '画面で指定する開始位置の方向(Angle辺の逆向き)
        '(hexidx)      
        '              ／  ／_BandList60
        '          1 ／・／ ... 六角形の辺上にある目の中心を、60度方向に延長
        '           →・→→→ 1         側面最初の目の水平ライン上の点で回転
        '        ／    (3)     ＼
        '     1／(4)          (2)＼
        '      ＼(5)          (1)／1
        '        ＼    (0)     ／  
        '         1 ←←←←←
        '                    1
        '
        Friend _Delta開始方向 As S差分 '単位
        Friend _Delta六つ目 As S差分 '開始方向の六つ目1個分


        Sub New(ByVal hxidx As Integer)
            _MyHexIndex = hxidx
        End Sub

        Sub Clear()
            _BandList60.Clear()
            _BandList120.Clear()
        End Sub

        Function InsertCount() As Integer
            '同数前提
            Return _BandList60.Count
        End Function

        '最外六角形上の六つ目内の中心点を返す
        'idx:1～InsertCount,開始方向(画面指定)順
        Function getCenter(ByVal idx As Integer, ByVal dInnerPosition As Double, ByRef pPoint As S実座標) As Boolean
            If idx < 1 OrElse InsertCount() < idx Then
                Return False
            End If

            Dim i As Integer
            If {2, 3, 4}.Contains(_MyHexIndex) Then 'i_2nd XOR (cSameDirectionWithDialog)
                i = idx - 1
            Else
                i = _BandList60.Count - idx '逆から
            End If

            Dim band As CBandPosition = _BandList60(i)
            pPoint = band.m_cp最外六角形.CrossPoint(_MyHexIndex)
            pPoint += _Delta開始方向 * (band.m_dひも幅 / 2)
            pPoint += _Delta六つ目 * dInnerPosition

            Return True
        End Function

        '最外六角形上の六つ目内の中心点を通る目のライン
        'idx:1～InsertCount,開始方向(画面指定)順
        Function getFnCenter(ByVal idx As Integer, ByVal dInnerPosition As Double, ByRef fn As S直線式) As Boolean
            Dim pPoint As S実座標
            If Not getCenter(idx, dInnerPosition, pPoint) Then
                Return False
            End If

            '開始位置の方向(Angle辺の逆向き)に60度(_BandList60)
            Dim angle As Integer = (CHex.Angle辺(_MyHexIndex) - 180) + 60

            '目を通るラインの式
            fn = New S直線式(angle, pPoint)

            Return True
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0}_MyHexIndex={1} ", Me.GetType().Name, _MyHexIndex)
            sb.AppendFormat("BandList60 Count={0}", _BandList60.Count).AppendLine()
            For Each band As CBandPosition In _BandList60
                sb.AppendFormat("BandList60 {0} ({1}) {2}", band.Ident, band.m_cp最外六角形.Status, band.m_cp最外六角形.CrossPoint(_MyHexIndex)).AppendLine()
            Next
            sb.AppendFormat("BandList120 Count={0}", _BandList120.Count).AppendLine()
            For Each band As CBandPosition In _BandList120
                sb.AppendFormat("BandList120 {0} ({1}) {2}", band.Ident, band.m_cp最外六角形.Status, band.m_cp最外六角形.CrossPoint(_MyHexIndex)).AppendLine()
            Next
            sb.AppendLine()
            Return sb.ToString
        End Function


    End Class


    '側面のひもの情報
    Private Function setSidePlate() As Boolean
        '基本計算は終わっていること
        If _CalcStatus <> CalcStatus._reflect Then
            Return False
        End If

        '6側面クリア
        For hxidx As Integer = 0 To CHex.cHexCount - 1
            _SidePlate(hxidx).Clear()
        Next

        '3方向のバンドが、6側面のどれに入っているかを振り分ける
        For aidx As Integer = 0 To cAngleCount - 1
            For Each band As CBandPosition In _BandPositions(aidx)
                If band Is Nothing OrElse band.m_cp最外六角形 Is Nothing Then
                    Continue For
                End If

                Dim cp As CHex.CCrossPoint = band.m_cp最外六角形
                For hxidx As Integer = 0 To CHex.cHexCount - 1
                    If Not cp.CrossPoint(hxidx).IsZero Then
                        If SameAngle(CHex.Angle辺(hxidx) + 60, band.BandAngle) OrElse
                            SameAngle(CHex.Angle辺(hxidx) + 60 + 180, band.BandAngle) Then
                            'バンドに+60度方向
                            _SidePlate(hxidx)._BandList60.Add(band)

                        ElseIf SameAngle(CHex.Angle辺(hxidx) + 120, band.BandAngle) OrElse
                            SameAngle(CHex.Angle辺(hxidx) + 120 + 180, band.BandAngle) Then
                            'バンドに+120度方向
                            _SidePlate(hxidx)._BandList120.Add(band)

                        Else
                            'バンドに平行はあり得ない
                            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "setSidePlate No Angle({0}) {1}", hxidx, band.ToString)
                        End If
                    End If
                Next
            Next
        Next


        For hxidx As Integer = 0 To CHex.cHexCount - 1
            With _SidePlate(hxidx)
                '各辺同数のバンドが入っているはず
                If ._BandList60.Count <> ._BandList120.Count Then
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "setSidePlate({0}) Count {1}<>{2}", hxidx, ._BandList60.Count, ._BandList120.Count)
                End If

                '単位方向値をセット(Angle辺の逆方向)
                ._Delta開始方向 = New S差分(CHex.Angle辺(hxidx) - 180)
                ._Delta六つ目 = ._Delta開始方向 * p_d六つ目の対角線

                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_SidePlate({0})", hxidx)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}", .ToString)
            End With
        Next

        Return True
    End Function
#End Region



#Region "バンドと模様の描画"

    '描画ソート順
    Const IdxBandDrawSide As Integer = 20 '側面
    Const IdxBandDrawBottom As Integer = 30 '底のひも
    Const IdxBandDrawInsert As Integer = 40 '差しひも



    '3本組の位置識別
    Private Function OneOfThree(ByVal idx As Integer) As Integer
        Return Modulo(idx, 3)
    End Function

    '2本組の位置識別
    Private Function OneOfTow(ByVal idx As Integer) As Integer
        Return Modulo(idx, 2)
    End Function

    '綾を考慮した次の方向
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


    '6側面のバンドセット配列, 縁は含まない
    Private Function bandList側面(ByVal isDraw As Boolean, ByVal bandListForClip差しひも As CBandList) As CBandList()
        Dim bandlists As New List(Of CBandList)

        '6側面
        For hexidx As Integer = 0 To CHex.cHexCount - 1
            Dim bandlist As New CBandList
            bandlists.Add(bandlist)

            '側面のレコード→1本ごとのバンド　※厚さを加えた幅・高さには加えない
            For idx As Integer = 1 To _i側面の編みひも数
                Dim sband As CSideBand = _CSideBandList.At(idx)

                Dim band As CBand = Nothing
                If isDraw Then

                    Dim line As S線分 = _hex底の辺に厚さ.line辺(hexidx)
                    line.Revert() '反転。辺の方向を、外向き法線-90にするため
                    line += CHex.delta辺の外向き法線(hexidx) * (-p_d底の厚さ) '底の辺に厚さ→底の辺

                    '文字表示位置は上側面(コード固定) #60
                    band = sband.ToBand(line, CHex.delta辺の外向き法線(hexidx), IsDrawMarkCurrent AndAlso (hexidx = 3))

                End If
                bandlist.AddAt(band, idx)
            Next

            bandListForClip差しひも.AddRange(bandlist)
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "imglst({0}):{1}", hexidx, imglst.ToString)
        Next

        Return bandlists.ToArray
    End Function

    '側面のバンドを加えた3方向の描画リストを返す
    Private Function imageItemListBandSet(ByVal checked As CDispImageChecked, ByVal bandListForClip差しひも As CBandList) As clsImageItemList()

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
            bandListForClip差しひも.AddRange(bandListBottom(idx))
        Next

        '6側面のバンドセット(縁は含まない)
        Dim bandListSide() As CBandList = bandList側面(checked(cAngleCount), bandListForClip差しひも)
        If bandListSide.Count <> CHex.cHexCount Then
            Return Nothing
        End If


        '*2 描画アイテムにしてリストに追加する
        '側面のバンド(1～高さ) → 底のバンド(1～本数) → 側面のバンド(1～高さ)

        '3方向の描画リスト
        Dim imageItemBandLists(cAngleCount - 1) As clsImageItemList

        For idx As Integer = 0 To cAngleCount - 1
            imageItemBandLists(idx) = New clsImageItemList

            Dim hexidx_1st As Integer = CHex.hexidx(idx, CHexLine.lineIdx.i_1st)
            For i As Integer = _i側面の編みひも数 To 1 Step -1
                Dim band As CBand = bandListSide(hexidx_1st)(i)
                Dim item As New clsImageItem(band, IdxBandDrawSide, idx)
                imageItemBandLists(idx).AddItem(item)
            Next

            For ax As Integer = 1 To _iひもの本数(idx)
                Dim band As CBand = bandListBottom(idx)(ax)
                Dim item As New clsImageItem(band, IdxBandDrawBottom, idx)
                imageItemBandLists(idx).AddItem(item)
            Next

            Dim hexidx_2nd As Integer = CHex.hexidx(idx, CHexLine.lineIdx.i_2nd)
            For i As Integer = 1 To _i側面の編みひも数
                Dim band As CBand = bandListSide(hexidx_2nd)(i)
                Dim item As New clsImageItem(band, IdxBandDrawSide, idx)
                imageItemBandLists(idx).AddItem(item)
            Next
        Next

        Return imageItemBandLists
    End Function

    '織り指定がない・描けない場合
    Function imageListバンドセット(ByVal checked As CDispImageChecked, ByVal bandListForClip差しひも As CBandList) As clsImageItemList
        Dim imageListバンド As New clsImageItemList

        '3方向の描画リスト
        Dim imageItemBandLists() As clsImageItemList = imageItemListBandSet(checked, bandListForClip差しひも)
        If imageItemBandLists Is Nothing OrElse imageItemBandLists.Count < cAngleCount Then
            Return Nothing
        End If

        '3方向のバンドセット追加
        For idx As Integer = 0 To cAngleCount - 1
            imageListバンド.MoveList(imageItemBandLists(idx))
            imageItemBandLists(idx) = Nothing
        Next

        Return imageListバンド
    End Function

    '描ければNothing,NGなら理由を返す
    Function Check3すくみ() As String
        If Not (_frmMain.rad右綾.Checked Or _frmMain.rad左綾.Checked) Then
            '右綾/左綾を指定してください。
            Return My.Resources.CalcNoPatternLeftRight
        End If
        If _bひも中心合わせ Then
            '「ひも中心合わせ」をオフにしてください。
            Return My.Resources.CalcNoPatternBandCenter
        Else
            If _frmMain.nud三角の中.Value < 0 Then
                '「三角の中」の値がゼロ以上になるよう設定してください。
                Return My.Resources.CalcNoPatternTriangle
            End If
        End If
        Return Nothing

    End Function

    '巴(3すくみ)の描画 ※底と側面個別
    Function imageListバンドセット3すくみ(ByVal checked As CDispImageChecked, ByVal bandListForClip差しひも As CBandList) As clsImageItemList
        If Check3すくみ() IsNot Nothing Then
            Return Nothing
        End If
        Dim imageListバンドと縁 As New clsImageItemList

        '底の3方向のバンドセット
        Dim bandListBottom(cAngleCount - 1) As CBandList
        '底の3方向のバンドセット描画
        Dim imageItemBandBottom(cAngleCount - 1) As clsImageItem

        For idx As Integer = 0 To cAngleCount - 1
            bandListBottom(idx) = Nothing
            If checked(idx) Then
                bandListBottom(idx) = _BandPositions(idx).ConvertToBandList()
                bandListForClip差しひも.AddRange(bandListBottom(idx))
            End If
            imageItemBandBottom(idx) = New clsImageItem(bandListBottom(idx), IdxBandDrawBottom, idx)
        Next


        '6側面のバンドセット(縁は含まない)
        Dim bandListSide() As CBandList = bandList側面(checked(cAngleCount), bandListForClip差しひも)
        If bandListSide.Count <> CHex.cHexCount Then
            Return Nothing
        End If

        '6側面のバンドセット描画
        Dim imageItemBandSide(CHex.cHexCount - 1) As clsImageItem
        For hexidx As Integer = 0 To CHex.cHexCount - 1
            imageItemBandSide(hexidx) = New clsImageItem(bandListSide(hexidx), IdxBandDrawSide, hexidx)
        Next

        '綾指定があれば領域をクリップ
        Dim d三角の中値 As Double = _frmMain.nud三角の中.Value
        If 0 <= NextDirection() Then
            If 0 <= d三角の中値 Then
                Dim is側面の三角形(cAngleCount - 1) As Boolean
                For idx As Integer = 0 To cAngleCount - 1
                    Dim d As Double '三角位置からのずれ
                    If p_b横ひもゼロ Then
                        d = (2 * (_d端の目(idx) + _d最下段の目) - 1) * _d六つ目の高さ +
                            (get側面の六つ目の高さ() - _d六つ目の高さ) * _i側面の編みひも数
                    Else
                        d = (_d端の目(idx) + _d最下段の目 - 1) * _d六つ目の高さ +
                            (get側面の六つ目の高さ() - _d六つ目の高さ) * _i側面の編みひも数
                    End If
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
            imageListバンドと縁.AddItem(imageItemBandBottom(idx))
        Next
        '6側面のバンドセット追加
        For hexidx As Integer = 0 To CHex.cHexCount - 1
            imageListバンドと縁.AddItem(imageItemBandSide(hexidx))
        Next

        Return imageListバンドと縁
    End Function

    '描ければNothing,NGなら理由を返す
    Function Check3軸織() As String
        If Not (_frmMain.rad右綾.Checked Or _frmMain.rad左綾.Checked) Then
            '右綾/左綾を指定してください。
            Return My.Resources.CalcNoPatternLeftRight
        End If
        If _bひも中心合わせ Then
            If _d六つ目の高さ < (_d基本のひも幅 / 2) Then
                '「六つ目の高さ」の値を {0} 以上に設定してください。
                Return String.Format(My.Resources.CalcNoPatternSpace, _d基本のひも幅 / 2)
            End If
        Else
            '目合わせならゼロでもよい
        End If
        Return Nothing
    End Function

    '3軸織り(鉄線)の描画 ※底と側面一括
    Function imageListバンドセット3軸織(ByVal checked As CDispImageChecked, ByVal isBackFace As Boolean, ByVal bandListForClip差しひも As CBandList) As clsImageItemList
        If Check3軸織() IsNot Nothing Then
            Return Nothing
        End If
        Dim imageListバンド As New clsImageItemList

        '3方向の描画リスト
        Dim imageItemBandLists() As clsImageItemList = imageItemListBandSet(checked, bandListForClip差しひも)
        If imageItemBandLists Is Nothing OrElse imageItemBandLists.Count < cAngleCount Then
            Return Nothing
        End If

        '3軸描画
        ThreeAxisBasic(imageItemBandLists, isBackFace)

        '3方向のバンドセット追加
        For idx As Integer = 0 To cAngleCount - 1
            imageListバンド.MoveList(imageItemBandLists(idx))
            imageItemBandLists(idx) = Nothing
        Next

        Return imageListバンド
    End Function

    '描ければNothing,NGなら理由を返す
    Function Check単麻の葉() As String
        If Not (_frmMain.rad右綾.Checked Or _frmMain.rad左綾.Checked) Then
            '右綾/左綾を指定してください。
            Return My.Resources.CalcNoPatternLeftRight
        End If
        If _bひも中心合わせ Then
            If _d六つ目の高さ < (_d基本のひも幅 / 2) Then
                '「六つ目の高さ」の値を {0} 以上に設定してください。
                Return String.Format(My.Resources.CalcNoPatternSpace, _d基本のひも幅 / 2)
            End If
        Else
            If _frmMain.nud三角の中.Value < 0 Then
                '「三角の中」の値がゼロ以上になるよう設定してください。
                Return My.Resources.CalcNoPatternTriangle
            End If
        End If
        Return Nothing
    End Function

    '単麻の葉 ※底と側面一括
    Function imageListバンドセット単麻の葉(ByVal checked As CDispImageChecked, ByVal bandListForClip差しひも As CBandList) As clsImageItemList
        If Check単麻の葉() IsNot Nothing Then
            Return Nothing
        End If
        Dim imageListバンド As New clsImageItemList

        '3方向の描画リスト
        Dim imageItemBandLists() As clsImageItemList = imageItemListBandSet(checked, bandListForClip差しひも)
        If imageItemBandLists Is Nothing OrElse imageItemBandLists.Count < cAngleCount Then
            Return Nothing
        End If

        '麻の葉描画
        TwoRepeatBasic(imageItemBandLists)

        '3方向のバンドセット追加
        For idx As Integer = 0 To cAngleCount - 1
            imageListバンド.MoveList(imageItemBandLists(idx))
            imageItemBandLists(idx) = Nothing
        Next

        Return imageListバンド
    End Function


    '3軸描画(綾指定あり)
    Private Function ThreeAxisBasic(ByVal imageItemBandList() As clsImageItemList, ByVal isBackFace As Boolean) As Boolean
        'スターの中心は合わせ位置(ひも中心の場合は軸方向後) -1位置をセット
        Dim ax合わせ前(cAngleCount - 1) As Integer
        For idx As Integer = 0 To cAngleCount - 1
            If idx = cIdxAngle0 AndAlso p_b横ひもゼロ Then
                '側面分をプラス、1を0に詰めた分をマイナス
                ax合わせ前(idx) = 0 + _i側面の編みひも数 - 1
            Else
                '合わせ目の前後のひも、前側
                ax合わせ前(idx) = _BandPositions(idx).i合わせ位置AxisIdx - 1
                '側面分をプラス、1を0に詰めた分をマイナス
                ax合わせ前(idx) = ax合わせ前(idx) + _i側面の編みひも数 - 1
                'ひも中心で裏面は、60度・120度で軸方向位置を後へ
                If _bひも中心合わせ AndAlso idx <> cIdxAngle0 Then
                    ax合わせ前(idx) -= 1
                End If
            End If
        Next

        For idx As Integer = 0 To cAngleCount - 1
            '±60度方向
            Dim ax_angle As Integer = cBandAngleDegree(idx) + 90 '軸角度
            Dim iNext As Integer = NextDirection(idx)

            For idx2 As Integer = 0 To cAngleCount - 1
                If idx = idx2 Then
                    Continue For
                End If
                If idx2 = iNext Then
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
        Return True
    End Function

    '単麻の葉(綾指定あり)
    Private Function TwoRepeatBasic(ByVal imageItemBandList() As clsImageItemList) As Boolean
        Dim ax合わせ位置(cAngleCount - 1) As Integer
        For idx As Integer = 0 To cAngleCount - 1
            '合わせ位置のひも
            ax合わせ位置(idx) = _BandPositions(idx).i合わせ位置AxisIdx
            '側面分をプラス、1を0に詰めた分をマイナス
            ax合わせ位置(idx) = ax合わせ位置(idx) + _i側面の編みひも数 - 1
        Next

        '特異的な方向: 0度 ひもの間を通る
        '斜め60,120度: 各、全上・全下の繰り返し
        '右綾・左綾とも同じ編み方(中心とみなす位置が変わるだけ)
        Dim shift As Integer = 0 '右綾
        If NextDirection(1) < 1 Then
            '左綾
            shift = 1
        End If

        '0度
        Dim iAngle As Integer = cIdxAngle0
        For idx As Integer = iAngle To iAngle
            '+60度方向
            Dim iNext As Integer = IdxNext(idx)
            For idx2 As Integer = 0 To cAngleCount - 1
                If idx = idx2 Then
                    Continue For
                End If
                If idx2 = iNext Then
                    '+60度方向 : 交互に、全てのひもに対して上になる/もしくは下になる
                    Dim kClip As Integer = OneOfTow(ax合わせ位置(idx) + 1 + shift)
                    For k As Integer = 0 To imageItemBandList(idx).Count - 1
                        If kClip = OneOfTow(k) Then
                            For j As Integer = 0 To imageItemBandList(idx2).Count - 1
                                imageItemBandList(idx)(k).AddClip(imageItemBandList(idx2)(j))
                            Next
                        End If
                    Next

                Else
                    '+120度方向 : 交互に、全てのひもに対して上になる/もしくは下になる
                    Dim kClip As Integer = OneOfTow(ax合わせ位置(idx) + shift)
                    For k As Integer = 0 To imageItemBandList(idx).Count - 1
                        If kClip = OneOfTow(k) Then
                            For j As Integer = 0 To imageItemBandList(idx2).Count - 1
                                imageItemBandList(idx)(k).AddClip(imageItemBandList(idx2)(j))
                            Next
                        End If
                    Next

                End If
            Next
        Next

        '60度
        iAngle = IdxNext(iAngle)
        For idx As Integer = iAngle To iAngle
            Dim iNext As Integer = IdxNext(idx)
            For idx2 As Integer = 0 To cAngleCount - 1
                If idx = idx2 Then
                    Continue For
                End If
                If idx2 = iNext Then
                    '+60度方向 : 各ひもに対して上/下交互, ひも交互
                    For k As Integer = 0 To imageItemBandList(idx).Count - 1
                        Dim jClip As Integer = OneOfTow(ax合わせ位置(idx2) + (k - ax合わせ位置(idx)) + shift)
                        For j As Integer = 0 To imageItemBandList(idx2).Count - 1
                            If jClip = OneOfTow(j) Then
                                imageItemBandList(idx)(k).AddClip(imageItemBandList(idx2)(j))
                            End If
                        Next
                    Next

                Else
                    '+120度方向 : 各ひもに対して上/下交互, 全ひも同じ
                    Dim jClip As Integer = OneOfTow(ax合わせ位置(idx2) + shift)
                    For k As Integer = 0 To imageItemBandList(idx).Count - 1
                        For j As Integer = 0 To imageItemBandList(idx2).Count - 1
                            If jClip = OneOfTow(j) Then
                                imageItemBandList(idx)(k).AddClip(imageItemBandList(idx2)(j))
                            End If
                        Next
                    Next
                End If
            Next
        Next

        '120度
        iAngle = IdxNext(iAngle)
        For idx As Integer = iAngle To iAngle
            Dim iNext As Integer = IdxNext(idx)
            For idx2 As Integer = 0 To cAngleCount - 1
                If idx = idx2 Then
                    Continue For
                End If
                If idx2 = iNext Then
                    '+60度方向 : 各ひもに対して上/下交互, 全ひも同じ
                    Dim jClip As Integer = OneOfTow(ax合わせ位置(idx2) + 1 + shift)
                    For k As Integer = 0 To imageItemBandList(idx).Count - 1
                        For j As Integer = 0 To imageItemBandList(idx2).Count - 1
                            If jClip = OneOfTow(j) Then
                                imageItemBandList(idx)(k).AddClip(imageItemBandList(idx2)(j))
                            End If
                        Next
                    Next

                Else
                    '+120度方向 : 各ひもに対して上/下交互, ひも交互
                    For k As Integer = 0 To imageItemBandList(idx).Count - 1
                        Dim jClip As Integer = OneOfTow(ax合わせ位置(idx2) + 1 + k - ax合わせ位置(idx) + shift)
                        For j As Integer = 0 To imageItemBandList(idx2).Count - 1
                            If jClip = OneOfTow(j) Then
                                imageItemBandList(idx)(k).AddClip(imageItemBandList(idx2)(j))
                            End If
                        Next
                    Next
                End If
            Next

        Next

        Return True
    End Function
#End Region

    '底と側面枠と縁
    Function imageList底と側面枠(ByVal b側面描画 As Boolean) As clsImageItemList
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
        If (1 < _iひもの本数(cIdxAngle0) AndAlso 1 < _iひもの本数(cIdxAngle60) AndAlso 1 < _iひもの本数(cIdxAngle120)) OrElse
            p_b横ひもゼロ() Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)

            Dim dRadius As Double
            If _bひも中心合わせ Then
                '基本のひも幅の対角線
                dRadius = _d基本のひも幅 / SIN60 / 2
            Else
                '目の対角線
                dRadius = _d六つ目の高さ / SIN60 / 2
            End If

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
        '_hex底の辺
        For hxidx As Integer = 0 To CHex.cHexCount - 1
            line = New S線分(_hex底の辺.line辺(hxidx))
            item.m_lineList.Add(line)
        Next
        itemlist.AddItem(item)
#If 0 Then
        '_hex外目幅の中心
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠2, 2)
        For hxidx As Integer = 0 To CHex.cHexCount - 1
            line = New S線分(_hex外目幅の中心.line辺(hxidx))
            item.m_lineList.Add(line)
        Next
        itemlist.AddItem(item)
#End If

        '側面枠・底の厚さ枠ベース　※底の厚さを加えて描くが、高さは_hex底の辺がゼロ
        Dim d底から縁までを描く高さ As Double = get側面高さ() + _d縁の高さ - p_d底の厚さ
        For hexidx As Integer = 0 To CHex.cHexCount - 1
            '側面枠,縁も加えた長方形
            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, hexidx)
            item.m_a四隅.pD = _hex底の辺に厚さ.line辺(hexidx).p開始
            item.m_a四隅.pC = _hex底の辺に厚さ.line辺(hexidx).p終了
            item.m_a四隅.pB = item.m_a四隅.pC + CHex.delta辺の外向き法線(hexidx) * d底から縁までを描く高さ
            item.m_a四隅.pA = item.m_a四隅.pD + CHex.delta辺の外向き法線(hexidx) * d底から縁までを描く高さ

            '側面上辺ライン
            line = New S線分(_hex側面上辺.line辺(hexidx))
            item.m_lineList.Add(line)

            itemlist.AddItem(item)


            If b側面描画 Then
                '縁のレコードをイメージ情報化
                Dim cond As String = String.Format("f_i番号 = {0}", cHemNumber)
                Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

                Dim d高さ As Double = groupRow.GetNameValueSum("f_d高さ")
                Dim nひも本数 As Integer = groupRow.GetNameValueSum("f_iひも本数")
                Dim d周長比率対底の周 As Double = groupRow.GetNameValueMax("f_d周長比率対底の周")
                If 0 < nひも本数 Then
                    '側面枠上辺
                    line = New S線分(_hex底の辺に厚さ.line辺(hexidx))
                    line += CHex.delta辺の外向き法線(hexidx) * (get側面高さ() - p_d底の厚さ)

                    Dim band As New CBand '周長比率対底の周計算用
                    band.p始点F = line.p開始
                    band.p終点F = line.p終了
                    band.p始点T = band.p始点F + CHex.delta辺の外向き法線(hexidx) * d高さ
                    band.p終点T = band.p終点F + CHex.delta辺の外向き法線(hexidx) * d高さ
                    band.SetLengthRatio(d周長比率対底の周)

                    item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 1)
                    item.m_a四隅 = band.aバンド位置
                    If IsDrawMarkCurrent Then
                        '文字は上側面(コード固定) #60 仮位置
                        If hexidx = 3 Then
                            item.p_p文字位置 = band.p始点F '領域処理を伴う
                        End If
                    End If

                    itemlist.AddItem(item)
                End If

            End If
        Next

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "imageList底と側面枠:{0}", itemlist.ToString)
        Return itemlist
    End Function


#Region "プレビュー画像生成"

    'プレビュータブのCheckBox値
    Public Class CDispImageChecked
        Private _checked(cAngleCount) As Boolean

        Public Sub New()
        End Sub

        ' インデクサ
        Default Public Property Item(index As Integer) As Boolean
            Get
                Return _checked(index)
            End Get
            Set(value As Boolean)
                _checked(index) = value
            End Set
        End Property

        Public Property Side As Boolean
            Get
                Return _checked(cAngleCount)
            End Get
            Set(value As Boolean)
                _checked(cAngleCount) = value
            End Set
        End Property

        Public ReadOnly Property IsSetAll As Boolean
            Get
                For i As Integer = 0 To _checked.Length - 1
                    If Not _checked(i) Then
                        Return False
                    End If
                Next
                Return True
            End Get
        End Property
    End Class


    '現画像生成時に記号を表示する(グローバル参照)
    Shared IsDrawMarkCurrent As Boolean = True

    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData, ByVal checked As CDispImageChecked, ByVal isBackFace As Boolean) As Boolean
        '記号順が変わるので裏面には表示しない
        IsDrawMarkCurrent = Not isBackFace AndAlso
                Not String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号)

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
        Dim imageListバンドセット As clsImageItemList = Nothing
        Dim bandListForClip差しひも As New CBandList

        If 0 <= NextDirection() Then
            '綾指定あり
            If _frmMain.rad巴_3すくみ.Checked Then
                '3すくみ指定
                imageListバンドセット = imageListバンドセット3すくみ(checked, bandListForClip差しひも)
            ElseIf _frmMain.rad鉄線_3軸織.Checked Then
                '鉄線_3軸織り
                imageListバンドセット = imageListバンドセット3軸織(checked, isBackFace, bandListForClip差しひも)
            ElseIf _frmMain.rad麻の葉_単方向.Checked Then
                '単麻の葉編み
                imageListバンドセット = imageListバンドセット単麻の葉(checked, bandListForClip差しひも)
            End If
        End If

        '綾指定がない・描けない
        If imageListバンドセット Is Nothing Then
            imageListバンドセット = Me.imageListバンドセット(checked, bandListForClip差しひも)
        End If
        If imageListバンドセット Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '底と側面
        Dim imageList描画要素 As clsImageItemList = imageList底と側面枠(checked.Side)


        '差しひもはおもて全描画時(うらはレコード無だが念のため
        If Not isBackFace AndAlso checked.IsSetAll Then
            Dim imgList差しひも As clsImageItemList = imageList差しひも(bandListForClip差しひも)
            imgData.MoveList(imgList差しひも)
        End If


        imgData.MoveList(imageListバンドセット)
        imageListバンドセット = Nothing
        '底と側面
        imgData.MoveList(imageList描画要素)
        imageList描画要素 = Nothing

        '付属品
        If Not isBackFace AndAlso checked.IsSetAll Then
            AddPartsImage(imgData, _frmMain.editAddParts)
        End If

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function


#End Region


End Class
