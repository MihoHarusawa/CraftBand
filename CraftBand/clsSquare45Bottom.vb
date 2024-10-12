Imports System.Drawing
Imports CraftBand.clsImageData

Public Class clsSquare45Bottom

    Dim _i横の四角数 As Integer '配置表示用 45度側
    Dim _i縦の四角数 As Integer '〃　　　　135度側
    Dim _i高さ切上四角数 As Integer '設定の切上値(未使用領域の表示のみ)


    Dim _i領域四角数 As Integer = 0
    Dim _is底位置表示 As Boolean = False 'IsValidかつサイズ一致時

    'メイン画面コントロール指定
    Dim _i高さ編集四角数 As Integer '開始高さ,領域サイズ要因,マイナス時は底編集対象外
    Dim _EditHorizontal As Integer = 0  '
    Dim _EditVertical As Integer = 0
    Dim _IsOnce As Boolean = 0


    '全数値がセットされている
    Public ReadOnly Property IsValid As Boolean
        Get
            Return _IsValid
        End Get
    End Property
    Dim _IsValid As Boolean = False

    '底位置表示の条件を満たす
    Public ReadOnly Property Is底位置表示() As Boolean
        Get
            Return _is底位置表示
        End Get
    End Property

    Sub New(ByVal yoko As Integer, ByVal tate As Integer, ByVal takasa As Integer)
        _i横の四角数 = yoko
        _i縦の四角数 = tate
        _i高さ切上四角数 = takasa

        calcBasicCount()
    End Sub

    '領域四角数とIs底位置更新(_I高さ編集四角数変更に伴う)
    Function SetEditHeight(ByVal edit_takasa As Integer, ByVal once As Boolean) As Boolean
        _i高さ編集四角数 = edit_takasa
        _IsOnce = once

        Return calcBasicCount()
    End Function

    '編集サイズの変更
    Function SetEditCount(ByVal horz As Integer, ByVal vert As Integer) As Boolean
        If horz <> _EditHorizontal OrElse vert <> _EditVertical Then
            _EditHorizontal = horz
            _EditVertical = vert

            Return calcBasicCount()
        Else
            Return True
        End If
    End Function

    '設定値の状態
    Private Function calcBasicCount() As Boolean
        _i領域四角数 = _i横の四角数 + _i縦の四角数 + 2 * _i高さ編集四角数
        If _i横の四角数 < 0 OrElse _i縦の四角数 < 0 OrElse _i高さ切上四角数 < 0 Then
            _IsValid = False
            Return False
        End If
        _IsValid = True

        If 0 < _i横の四角数 AndAlso 0 < _i縦の四角数 AndAlso
                _IsOnce AndAlso
                0 <= _i高さ編集四角数 AndAlso
                0 < _EditHorizontal AndAlso 0 < _EditVertical AndAlso
                (_EditHorizontal = _EditVertical) AndAlso
                (_EditHorizontal = _i領域四角数) AndAlso
                _i領域四角数 < clsUpDown.cMaxUpdownColumns Then
            _is底位置表示 = True
        Else
            _is底位置表示 = False
        End If

        Return _IsValid
    End Function

    '範囲内の値である　※底位置表示に使う前提
    Private Function inRange(ByVal idx As Integer) As Boolean
        Return 1 <= idx AndAlso idx <= _i領域四角数
    End Function

    '-2="-B",-1="-A",0="0",1="A",2="B",3="C"...
    Private Function alfaString(ByVal idx As Integer) As String
        Dim sign As String
        Dim number As Integer
        If idx < 0 Then
            sign = "-"
            number = -idx
        ElseIf 0 < idx Then
            sign = ""
            number = idx
        Else
            Return "0"
        End If

        Dim str As String = ""
        While 0 < number
            Dim modulo As Integer = (number - 1) Mod 26
            str = Chr(Asc("A") + modulo) & str
            number = (number - 1) \ 26
        End While

        Return sign + str
    End Function


    '1～高さ(縦+横)高さ →-C,-B,-A .. -3, -2, -1, 0, 0, 0, 1, 2, 3 ..A,B,C.
    Function GetIndexPosition(ByVal idx As Integer) As String
        If _is底位置表示 Then
            If Not inRange(idx) Then
                Return String.Empty
            End If

            '両側高さ編集四角数
            If idx <= _i高さ編集四角数 Then
                Return alfaString(idx - _i高さ編集四角数 - 1)
            ElseIf (_i領域四角数 - _i高さ編集四角数) < idx Then
                Return alfaString(_i領域四角数 - _i高さ編集四角数 - idx + 1)
            End If

            '底の中
            Dim ib As Integer = idx - _i高さ編集四角数
            Dim smalls As Integer
            Dim coms As Integer
            If _i縦の四角数 < _i横の四角数 Then
                smalls = _i縦の四角数
                coms = _i横の四角数 - _i縦の四角数
            Else
                smalls = _i横の四角数
                coms = _i縦の四角数 - _i横の四角数
            End If

            If ib <= smalls Then
                Return ib - smalls - 1
            ElseIf ib <= smalls + coms Then
                Return 0
            ElseIf ib <= _i縦の四角数 + _i横の四角数 Then
                Return ib - (smalls + coms)
            Else
                Return ib
            End If
        Else
            '底位置以外
            Return idx.ToString
        End If
    End Function

    Enum bottom_category
        _nodef
        _bottom '底の中
        _bottom_line '境界線
        _center_line '中央線
        _side135 '側面135
        _side45R '側面45
        _noexist   '存在しない
        _side_line_front  '側面の辺・正面と背面側
        _side_line_sideR  '側面の辺・側面側
        _height_over45R '高さの外45
        _height_over '高さの外
    End Enum

    Shared ColorsON() As Color = {
    Color.Black,
    Color.FromArgb(160, 160, 160),'底の中,
    Color.FromArgb(88, 136, 36), '境界線
    Color.FromArgb(255, 0, 0), '中央線
    Color.FromArgb(255, 217, 102), '側面135
    Color.FromArgb(252, 228, 214), '側面45 ※
    Color.FromArgb(51, 63, 79), '存在しない
    Color.FromArgb(46, 118, 184), '側面の辺・正面と背面側
    Color.FromArgb(0, 51, 154), '側面の辺・側面側 ※
    Color.FromArgb(240, 240, 240), '高さの外45 ※
    Color.FromArgb(160, 160, 160) '高さの外
}

    Shared ColorsOFF() As Color = {
    Color.Black,
    Color.White,'底の中,
    Color.FromArgb(146, 208, 80), '境界線
    Color.FromArgb(255, 121, 121), '中央線
    Color.FromArgb(255, 242, 204), '側面135
    Color.FromArgb(244, 176, 132), '側面45 ※
    Color.FromArgb(117, 113, 113), '存在しない
    Color.FromArgb(155, 194, 230), '側面の辺・前面と背面側
    Color.FromArgb(0, 32, 96), '側面の辺・側面側 ※
    Color.FromArgb(170, 170, 170), '高さの外45 ※
    Color.White '高さの外
}

    '底表示時は正方形(1～_i領域四角数)
    Function GetBackColor(ByVal horzIdx As Integer, ByVal vertIdx As Integer, ByVal value As Boolean) As Drawing.Color
        If _i高さ編集四角数 < 0 Then
            Return If(value, ColorsON(bottom_category._height_over), ColorsOFF(bottom_category._height_over))
        End If
        If _is底位置表示 Then
            Dim cat As bottom_category = checkIsInBottom(horzIdx, vertIdx)
            Return If(value, ColorsON(cat), ColorsOFF(cat))
        Else
            Return If(value, ColorsON(bottom_category._bottom), ColorsOFF(bottom_category._bottom))
        End If
    End Function

#Region "領域の判定"
    '  1234...    '  
    '  →→horIdx                           
    '1↓                          左上ライン  verIdx=-horIdx+領域四角数-縦の四角数+1
    '2↓        ／＼          ／＼      45度(／中心線はverIdx=-horIdx+領域四角数+1)
    'verIdx   ／    ＼     ／      ＼       
    '       ／        ＼ ／          ＼右下ライン verIdx=-horIdx+領域四角数+縦の四角数+1
    '     ／   前面    ／＼         ／
    '      ＼        ／    ＼     ／  
    '        ＼    ／ ----   ＼ ／
    '          ＼／|中央領域| ／＼    
    '         ／ ＼  ----- ／     ＼            
    '       ／     ＼    ／         ＼      
    '     ／         ＼／   背面    ／　　右上ライン verIdx=horIdx-横の四角数
    '     ＼         ／＼         ／    
    '       ＼    ／     ＼     ／     135度 (＼中心線はverIdx=horIdx)
    '         ＼／          ＼／ 左下ライン  verIdx=horIdx+横の四角数     
    '                   
    '　　←-------------------------→ 領域四角数=縦の四角数+横の四角数+2*高さ編集四角数


    '45度(左上ラインと右下ライン) に対応する定数定義（1桁の値）
    Const area45_上枠外 As Integer = 1
    Const area45_上外 As Integer = 2
    Const area45_上線 As Integer = 3
    Const area45_間 As Integer = 4
    Const area45_下線 As Integer = 5
    Const area45_下外 As Integer = 6
    Const area45_下枠外 As Integer = 7

    '45度ライン位置
    Private Function area45(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As Integer
        Dim i45 As Integer
        '左上枠ライン
        Dim y0 As Integer = -horzIdx + _i領域四角数 - _i縦の四角数 + 1 - 2 * _i高さ切上四角数
        If vertIdx < y0 Then
            i45 = area45_上枠外
        Else
            '左上ライン
            Dim y1 As Integer = -horzIdx + _i領域四角数 - _i縦の四角数 + 1
            If vertIdx < y1 Then
                i45 = area45_上外
            ElseIf y1 = vertIdx Then
                i45 = area45_上線
            Else
                '右下ライン
                Dim y2 As Integer = -horzIdx + _i領域四角数 + _i縦の四角数 + 1
                If vertIdx < y2 Then
                    i45 = area45_間
                ElseIf y2 = vertIdx Then
                    i45 = area45_下線
                Else
                    '右下枠ライン
                    Dim y3 As Integer = -horzIdx + _i領域四角数 + _i縦の四角数 + 1 + 2 * _i高さ切上四角数
                    If vertIdx <= y3 Then
                        i45 = area45_下外
                    Else
                        i45 = area45_下枠外
                    End If
                End If
            End If
        End If

        Return i45
    End Function

    '135度(右上ラインと左下ライン) に対応する定数定義（100の位）
    Const area135_上枠外 As Integer = 100
    Const area135_上外 As Integer = 200
    Const area135_上線 As Integer = 300
    Const area135_間 As Integer = 400
    Const area135_下線 As Integer = 500
    Const area135_下外 As Integer = 600
    Const area135_下枠外 As Integer = 700

    '135度ライン位置
    Private Function area135(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As Integer
        Dim i135 As Integer
        '右上枠ライン
        Dim y0 As Integer = horzIdx - _i横の四角数 - 2 * _i高さ切上四角数
        If vertIdx < y0 Then
            i135 = area135_上枠外
        Else
            '右上ライン
            Dim y1 As Integer = horzIdx - _i横の四角数
            If vertIdx < y1 Then
                i135 = area135_上外
            ElseIf y1 = vertIdx Then
                i135 = area135_上線
            Else
                '左下ライン
                Dim y2 As Integer = horzIdx + _i横の四角数
                If vertIdx < y2 Then
                    i135 = area135_間
                ElseIf y2 = vertIdx Then
                    i135 = area135_下線
                Else
                    '左下枠ライン
                    Dim y3 As Integer = horzIdx + _i横の四角数 + 2 * _i高さ切上四角数
                    If vertIdx <= y3 Then
                        i135 = area135_下外
                    Else
                        i135 = area135_下枠外
                    End If
                End If
            End If
        End If

        Return i135
    End Function


    '中央部の並列部分(見出しが"0")          縦<横  　　縦=横 　　横<縦
    Enum center_area
        center_none     '中央領域にない   ┼──┼     　　　　┼──┼　
        center_upper    '上側             │上／│     領域    │＼上│
        center_lower    '下側             │／下│     なし    │下＼│
        center_line     '中央線上         ┼──┼             ┼──┼
    End Enum

    '中央領域の内の位置を返す
    Private Function isCenterArea(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As center_area
        Dim iExcept As Integer = IIf(_i縦の四角数 < _i横の四角数, _i縦の四角数, _i横の四角数)
        iExcept += _i高さ編集四角数

        If horzIdx <= iExcept OrElse (_i領域四角数 - iExcept) < horzIdx Then
            Return center_area.center_none
        End If
        If vertIdx <= iExcept OrElse (_i領域四角数 - iExcept) < vertIdx Then
            Return center_area.center_none
        End If

        '中央領域内
        If (_i縦の四角数 <= _i横の四角数) Then
            '／ 縦<横 45度ライン
            If horzIdx = (_i領域四角数 - vertIdx + 1) Then
                Return center_area.center_line
            ElseIf horzIdx < (_i領域四角数 - vertIdx + 1) Then
                Return center_area.center_upper
            Else
                Return center_area.center_lower
            End If
        Else
            '＼ 横<縦 135度ライン
            If horzIdx = vertIdx Then
                Return center_area.center_line
            ElseIf horzIdx < vertIdx Then
                Return center_area.center_lower
            Else
                Return center_area.center_upper
            End If
        End If
        Return True
    End Function



    '45度ラインと135度ラインで区切られる領域
    Private Function checkIsInBottom(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As bottom_category
        '45度ライン位置
        Dim i45 As Integer = area45(horzIdx, vertIdx)
        '135度ライン位置
        Dim i135 As Integer = area135(horzIdx, vertIdx)

        '存在しない領域
        If {area45_上枠外, area45_上外, area45_下外, area45_下枠外}.Contains(i45) AndAlso
            {area135_上枠外, area135_上外, area135_下外, area135_下枠外}.Contains(i135) Then
            Return bottom_category._noexist
        End If

        ' area_45 と area_135 の組み合わせで処理を分岐
        Select Case i45 + i135
            ' area45_上外 の場合
            Case area45_上外 + area135_上線
                Return bottom_category._side_line_front
            Case area45_上外 + area135_間
                Return bottom_category._side135
            Case area45_上外 + area135_下線
                Return bottom_category._side_line_front

            ' area45_上の線 の場合
            Case area45_上線 + area135_上外
                Return bottom_category._side_line_sideR
            Case area45_上線 + area135_上線
                Return bottom_category._nodef   'ないはず
            Case area45_上線 + area135_間
                Return bottom_category._bottom_line
            Case area45_上線 + area135_下線
                Return bottom_category._nodef   'ないはず
            Case area45_上線 + area135_下外
                Return bottom_category._side_line_sideR

            ' area45_間 の場合
            Case area45_間 + area135_上枠外
                Return bottom_category._height_over45R
            Case area45_間 + area135_上外
                Return bottom_category._side45R
            Case area45_間 + area135_上線
                Return bottom_category._bottom_line
            Case area45_間 + area135_間
                '4本の線の間・底
                If isCenterArea(horzIdx, vertIdx) = center_area.center_line Then
                    Return bottom_category._center_line
                End If
                Return bottom_category._bottom '底
            Case area45_間 + area135_下線
                Return bottom_category._bottom_line
            Case area45_間 + area135_下外
                Return bottom_category._side45R
            Case area45_間 + area135_下枠外
                Return bottom_category._height_over45R

            ' area45_下の線 の場合
            Case area45_下線 + area135_上外
                Return bottom_category._side_line_sideR
            Case area45_下線 + area135_上線
                Return bottom_category._nodef   'ないはず
            Case area45_下線 + area135_間
                Return bottom_category._bottom_line
            Case area45_下線 + area135_下線
                Return bottom_category._nodef   'ないはず
            Case area45_下線 + area135_下外
                Return bottom_category._side_line_sideR

            ' area45_下外 の場合
            Case area45_下外 + area135_上線
                Return bottom_category._side_line_front
            Case area45_下外 + area135_間
                Return bottom_category._side135
            Case area45_下外 + area135_下線
                Return bottom_category._side_line_front

            Case Else
                '枠外
                Return bottom_category._height_over
        End Select
    End Function
#End Region


    '「合わせる」上下生成
    Function FitSizeUpDown(ByVal is横の辺 As Boolean, ByVal i垂直に As Integer, ByVal i底に As Integer) As clsUpDown

        Dim updown As New clsUpDown(clsUpDown.enumTargetFace.Bottom)
        updown.HorizontalCount = _i領域四角数
        updown.VerticalCount = _i領域四角数
        If Not updown.IsValid(False) Then
            Return Nothing
        End If

        If i垂直に < 0 OrElse i底に < 0 OrElse (i垂直に = 0 And i底に = 0) Then
            If Not fitSizeZero(updown, is横の辺) Then
                Return Nothing
            End If
        Else
            If Not fitSize(updown, is横の辺, i垂直に, i底に) Then
                Return Nothing
            End If
        End If

        Return updown
    End Function

    '「合わせる」対象領域 ※側面の辺のラインは重複しますが整合するはず
    Private Function isFitArea(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As Boolean
        '45度ライン位置
        Dim i45 As Integer = area45(horzIdx, vertIdx)
        '135度ライン位置
        Dim i135 As Integer = area135(horzIdx, vertIdx)

        Return (i45 = area45_上線 OrElse i45 = area45_間 OrElse i45 = area45_下線) OrElse
            (i135 = area135_上線 OrElse i135 = area135_間 OrElse i135 = area135_下線)
    End Function

    'ゼロの場合: 底の線だけ
    Private Function fitSizeZero(ByVal updown As clsUpDown, ByVal is横の辺 As Boolean) As Boolean
        If updown Is Nothing OrElse Not updown.IsValid(False) Then
            Return False
        End If

        If is横の辺 Then
            '横のライン
            For horzIdx As Integer = 1 To updown.HorizontalCount
                For vertIdx As Integer = 1 To updown.VerticalCount
                    If {area45_上線, area45_下線}.Contains(area45(horzIdx, vertIdx)) AndAlso
                     area135_間 = area135(horzIdx, vertIdx) Then
                        updown.SetIsUp(horzIdx, vertIdx)
                    End If
                Next
            Next
        Else
            '縦のライン
            For horzIdx As Integer = 1 To updown.HorizontalCount
                For vertIdx As Integer = 1 To updown.VerticalCount
                    If {area135_上線, area135_下線}.Contains(area135(horzIdx, vertIdx)) AndAlso
                         area45_間 = area45(horzIdx, vertIdx) Then
                        updown.SetIsUp(horzIdx, vertIdx)
                    End If
                Next
            Next
        End If

        Return True
    End Function

    'ON数値の指定がある場合
    Private Function fitSize(ByVal updown As clsUpDown, ByVal is横の辺 As Boolean, ByVal i垂直に As Integer, ByVal i底に As Integer) As Boolean
        If updown Is Nothing OrElse Not updown.IsValid(False) Then
            Return False
        End If

        '縦のパターン　※辺を開始点として上へ
        Dim unit_vert As New clsUpDown(clsUpDown.enumTargetFace.Bottom)
        unit_vert.HorizontalCount = 1
        unit_vert.VerticalCount = (i垂直に + i底に) * 2
        For y = 1 To i垂直に
            unit_vert.SetIsUp(1, y, is横の辺)
        Next
        For y = i垂直に + 1 To i垂直に + (i垂直に + i底に)
            unit_vert.SetIsUp(1, y, Not is横の辺)
        Next
        For y = i垂直に + (i垂直に + i底に) + 1 To i垂直に + (i垂直に + i底に) + i底に
            unit_vert.SetIsUp(1, y, is横の辺)
        Next

        '横のパターン　※辺を開始点として上へ
        Dim unit_horz As New clsUpDown(unit_vert)
        unit_horz.RotateLeft()
        unit_horz.Reverse()


        '・→→→hidx_leup
        '↓(左上)　　　　　 y↑ 縦のパターン
        '↓                  ●→x  底の最左頂点をx=0,y=0
        '↓vidx_leup                (i高さ編集四角数,i横の四角数+i高さ編集四角数)

        '左上領域,右下領域
        For x As Integer = -_i高さ編集四角数 To _i横の四角数
            For y As Integer = 1 To _i横の四角数 + _i高さ編集四角数
                If unit_vert.GetIsUp(1, y - x + 1) Then
                    Dim hidx_leup As Integer = x + _i高さ編集四角数
                    Dim vidx_leup As Integer = _i横の四角数 + _i高さ編集四角数 - y + 1

                    If isFitArea(hidx_leup, vidx_leup) Then
                        Dim ca As center_area = isCenterArea(hidx_leup, vidx_leup)
                        If ca = center_area.center_lower Then
                            Continue For
                        End If
                        updown.SetIsUp(hidx_leup, vidx_leup) '左上領域

                        If ca = center_area.center_line Then
                            Continue For
                        End If
                        '点対称位置
                        updown.SetIsUp(_i領域四角数 - hidx_leup + 1, _i領域四角数 - vidx_leup + 1) '右下領域
                    End If
                End If
            Next
        Next


        '・→→→hidx_riup
        '↓(左上)　　　　　       (i横の四角数+i高さ編集四角数,i高さ編集四角数)
        '↓                  ●→x  底の最上頂点をx=0,y=0
        '↓vidx_riup        y↓  横のパターン   

        '右上領域,左下領域
        For y As Integer = -_i高さ編集四角数 To _i縦の四角数
            For x As Integer = 1 To _i縦の四角数 + _i高さ編集四角数
                If unit_horz.GetIsUp(x - y + 1, 1) Then
                    Dim hidx_riup As Integer = _i横の四角数 + _i高さ編集四角数 + x
                    Dim vidx_riup As Integer = y + _i高さ編集四角数

                    If isFitArea(hidx_riup, vidx_riup) Then
                        Dim ca As center_area = isCenterArea(hidx_riup, vidx_riup)
                        If ca = center_area.center_lower Then
                            Continue For
                        End If
                        updown.SetIsUp(hidx_riup, vidx_riup) '右上領域

                        If ca = center_area.center_line Then
                            Continue For
                        End If
                        '点対称位置
                        updown.SetIsUp(_i領域四角数 - hidx_riup + 1, _i領域四角数 - vidx_riup + 1) '左下領域
                    End If
                End If
            Next
        Next

        Return True
    End Function

    '立ち上げた時に同じ位置になる2箇所
    Structure SSamePointPair
        '正: 前面/背面側
        Public horzIdx As Integer
        Public vertIdx As Integer
        Public val As Boolean
        Public plateIdx As enumBasketPlateIdx

        '従: 両側面側
        Public horzIdxRef As Integer
        Public vertIdxRef As Integer
        Public valRef As Boolean
        Public plateIdxRef As enumBasketPlateIdx

        '下からの段数
        Public nStep As Integer

        Dim _clsSquare45Bottom As clsSquare45Bottom

        Sub New(ByVal parent As clsSquare45Bottom, ByVal hidx As Integer, ByVal vidx As Integer)
            _clsSquare45Bottom = parent
            horzIdx = hidx
            vertIdx = vidx
        End Sub

        Public ReadOnly Property isOK As Boolean
            Get
                Return (val = Not valRef)
            End Get
        End Property

        Private Shared Function OnOff(ByVal b As Boolean) As String
            Return IIf(b, "on", "off")
        End Function

        Private Shared Function OkNg(ByVal b As Boolean) As String
            Return IIf(b, "OK", "NG")
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.Append(OkNg(isOK))
            If _clsSquare45Bottom IsNot Nothing Then
                sb.AppendFormat(" {0} {1} ({2},{3})", BasketPlateString(plateIdx), nStep, _clsSquare45Bottom.GetIndexPosition(horzIdx), _clsSquare45Bottom.GetIndexPosition(vertIdx))
                sb.AppendFormat(":{0} ({1},{2}) ({3})", BasketPlateString(plateIdxRef), _clsSquare45Bottom.GetIndexPosition(horzIdxRef), _clsSquare45Bottom.GetIndexPosition(vertIdxRef), OnOff(val))
            Else
                sb.AppendFormat(" {0} {1} ({2},{3})", BasketPlateString(plateIdx), nStep, horzIdx, vertIdx)
                sb.AppendFormat(":{0} ({1},{2}) ({3})", BasketPlateString(plateIdxRef), horzIdxRef, vertIdxRef, OnOff(val))
            End If
            Return sb.ToString
        End Function

    End Structure

    Class CSamePointPairList
        Inherits List(Of SSamePointPair)

        Public Property CountOK As Integer = 0
        Public Property CountNG As Integer = 0

        Overloads Sub Add(ByVal item As SSamePointPair)
            If item.isOK Then
                CountOK += 1
            Else
                CountNG += 1
            End If
            MyBase.Add(item)
        End Sub

        '全内容
        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("CSamePointPairList Count={0} OK={1} NG={2}", Count, CountOK, CountNG).AppendLine()
            For Each sp As SSamePointPair In Me
                sb.AppendLine(sp.ToString)
            Next
            Return sb.ToString
        End Function
    End Class


    '側面の整合性をチェックする
    'NGの場合は問題点をメッセージ化し、updownに整合した値をセットする
    Function CheckSideLine(ByRef msg As String, ByRef updown As clsUpDown) As Boolean
        If updown Is Nothing OrElse updown.HorizontalCount <> _i領域四角数 OrElse updown.VerticalCount <> _i領域四角数 OrElse
            Not IsValid Then
            '"チェック対象が正しく設定されていません。"
            msg = My.Resources.ErrSquare45BottomNoTarget
            updown = Nothing
            Return False
        End If
        If _i高さ編集四角数 = 0 Then
            '開始高さがゼロのため編集対象の側面辺はありません。
            msg = My.Resources.ErrSquare45BottomZeroHeight
            updown = Nothing
            Return True
        End If

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "updown Before{0}{1}", vbCrLf, updown.ToString)


        '左上ライン・右上ラインに沿ってチェックする
        Dim checklist As New CSamePointPairList
        Dim ret As Boolean = checkSideLineLeft(updown, checklist)
        ret = ret And checkSideLineRight(updown, checklist)

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CheckSideLine({0}) {1}{2}", ret, vbCrLf, checklist.ToString())
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "updown After{0}{1}", vbCrLf, updown.ToString)

        If checklist.Count = 0 Then
            '"チェック対象の側面辺が見つかりませんでした。"
            msg = My.Resources.ErrSquare45BottomNotFound
            updown = Nothing
            ret = False 'あるはずなのでこれもエラー

        ElseIf 0 < checklist.CountNG Then
            Dim sb As New System.Text.StringBuilder
            '"全{0}点中 {1}点に不整合があります。"
            sb.AppendFormat(My.Resources.ErrrSquare45BottomBad, checklist.Count, checklist.CountNG).AppendLine()
            For Each sp As SSamePointPair In checklist
                If Not sp.isOK Then
                    sb.AppendLine(sp.ToString)
                End If
            Next
            msg = sb.ToString

        Else
            '"側面辺 {0}点の対応を確認しました。"
            msg = String.Format(My.Resources.MsgSquare45BottomChecked, checklist.Count)
            updown = Nothing

        End If
        Return ret
    End Function

    '前面左側、左下ラインに沿ったチェック
    Private Function checkSideLineLeft(ByRef updown As clsUpDown, ByVal checklist As CSamePointPairList)
        Dim ret As Boolean = True

        For horzIdx As Integer = 1 To _i領域四角数
            '左下ライン＼
            Dim vertIdx As Integer = horzIdx + _i横の四角数
            If Not inRange(vertIdx) Then
                Continue For
            End If
            Dim dd As SSamePointPair

            Dim i45 As Integer = area45(horzIdx, vertIdx)
            If i45 = area45_上外 Then
                '前面側のbottom_category._side_line_front 

                '対応する同位置は、on 左上ライン／
                dd = New SSamePointPair(Me, horzIdx, vertIdx)
                dd.horzIdxRef = horzIdx
                dd.vertIdxRef = -horzIdx + _i領域四角数 - _i縦の四角数 + 1

                dd.plateIdx = enumBasketPlateIdx._front
                dd.plateIdxRef = enumBasketPlateIdx._leftside

                dd.nStep = _i高さ編集四角数 - horzIdx + 1

            ElseIf i45 = area45_下外 Then
                '背面側のbottom_category._side_line_front 

                '対応する同位置は、on 右下ライン／
                dd = New SSamePointPair(Me, horzIdx, vertIdx)
                dd.vertIdxRef = vertIdx
                dd.horzIdxRef = -vertIdx + _i領域四角数 + _i縦の四角数 + 1

                dd.plateIdx = enumBasketPlateIdx._back
                dd.plateIdxRef = enumBasketPlateIdx._leftside

                dd.nStep = vertIdx - (_i領域四角数 - _i高さ編集四角数)

            Else
                Continue For
            End If

            If Not inRange(dd.horzIdxRef) OrElse Not inRange(dd.vertIdxRef) Then
                dd = Nothing
                Continue For
            End If

            dd.val = updown.GetIsUp(dd.horzIdx, dd.vertIdx)
            dd.valRef = updown.GetIsUp(dd.horzIdxRef, dd.vertIdxRef)
            checklist.Add(dd)

            If Not dd.isOK Then
                updown.SetReverse(dd.horzIdxRef, dd.vertIdxRef)
                ret = False
            End If
        Next

        Return ret
    End Function

    '前面右側、右上ラインに沿ったチェック
    Private Function checkSideLineRight(ByRef updown As clsUpDown, ByVal checklist As CSamePointPairList)
        Dim ret As Boolean = True

        For horzIdx As Integer = 1 To _i領域四角数
            '右上ライン＼
            Dim vertIdx As Integer = horzIdx - _i横の四角数
            If Not inRange(vertIdx) Then
                Continue For
            End If
            Dim dd As SSamePointPair

            Dim i45 As Integer = area45(horzIdx, vertIdx)
            If i45 = area45_上外 Then
                '前面側のbottom_category._side_line_front 

                '対応する同位置は、on 左上ライン／
                dd = New SSamePointPair(Me, horzIdx, vertIdx)
                dd.vertIdxRef = vertIdx
                dd.horzIdxRef = -vertIdx + _i領域四角数 - _i縦の四角数 + 1

                dd.plateIdx = enumBasketPlateIdx._front
                dd.plateIdxRef = enumBasketPlateIdx._rightside

                dd.nStep = _i高さ編集四角数 - vertIdx + 1

            ElseIf i45 = area45_下外 Then
                '背面側のbottom_category._side_line_front 

                '対応する同位置は、on 右下ライン／
                dd = New SSamePointPair(Me, horzIdx, vertIdx)
                dd.horzIdxRef = horzIdx
                dd.vertIdxRef = -horzIdx + _i領域四角数 + _i縦の四角数 + 1

                dd.plateIdx = enumBasketPlateIdx._back
                dd.plateIdxRef = enumBasketPlateIdx._rightside

                dd.nStep = horzIdx - (_i領域四角数 - _i高さ編集四角数)

            Else
                Continue For
            End If

            If Not inRange(dd.horzIdxRef) OrElse Not inRange(dd.vertIdxRef) Then
                dd = Nothing
                Continue For
            End If

            dd.val = updown.GetIsUp(dd.horzIdx, dd.vertIdx)
            dd.valRef = updown.GetIsUp(dd.horzIdxRef, dd.vertIdxRef)
            checklist.Add(dd)

            If Not dd.isOK Then
                updown.SetReverse(dd.horzIdxRef, dd.vertIdxRef)
                ret = False
            End If
        Next

        Return ret
    End Function




End Class
