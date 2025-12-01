Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsImageItem.CBand
Imports CraftBand.clsUpDown
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBandSquare45.clsModelSquare45

Partial Public Class clsCalcSquare45

    '1～p_iひもの本数
    Dim _BandPositions(cExpandCount - 1) As CBandPositionList
    '底の形状
    Dim _a底領域 As S四隅
    '側面高さのキャッシュ値(バンド積み上げ)#84
    Dim _dバンド側面の高さ As Double

#Region "テクスチャファイル作成用"
    '底の絵のファイル
    Dim _BottomPngFilePath As String = Nothing
    Property p_sBottomPngFilePath(ByVal check As Boolean) As String
        Get
            If check AndAlso Not String.IsNullOrWhiteSpace(_BottomPngFilePath) Then
                If IO.File.Exists(_BottomPngFilePath) Then
                    Return _BottomPngFilePath
                Else
                    Return Nothing
                End If
            Else
                Return _BottomPngFilePath
            End If
        End Get
        Set(value As String)
            _BottomPngFilePath = value
            If check AndAlso Not String.IsNullOrWhiteSpace(value) AndAlso IO.File.Exists(value) Then
                IO.File.Delete(value)
            End If
        End Set
    End Property

    ''底の絵の回転角度
    'Dim _dBottomPngRotateAngle As Double
    '底の絵からバンド半分小さくするか
    Dim _isBottomPngTrimHalfBand As Boolean
    '側面順の回転角度
    Shared _PlateAngle() As Integer = {45, 135, 45, -45, -135}
    Dim _idxBottomPngPlate As enumBasketPlateIdx

    '面と0.5四角の有無を指定
    Friend Sub setBottomPngPlateIndex(ByVal plateIdx As enumBasketPlateIdx, ByVal isTrimHalf As Boolean)
        _idxBottomPngPlate = plateIdx
        '_dBottomPngRotateAngle = _PlateAngle(plateIdx)
        _isBottomPngTrimHalfBand = isTrimHalf
    End Sub

    'UpDownの適用開始位置
    Dim _is_updown_start_set As Boolean = False
    Dim _updown_start_x As Integer = 0
    Dim _updown_start_y As Integer = 0
    Sub setUpDownStartPosition(ByVal idx_x As Integer, idx_y As Integer)
        _updown_start_x = idx_x
        _updown_start_y = idx_y
        _is_updown_start_set = Not ((idx_x = 0) AndAlso (idx_y = 0))
    End Sub

    'iExp:縦/横  isReverse:逆方向  count:点数 isFromLast:後半部分 isTerminal:端の側
    Friend Function getBandAttributeList(ByVal iExp As emExp, ByVal isReverse As Boolean, ByVal count As Integer, ByVal isFromLast As Boolean, ByVal isTerminal As Boolean) As CBandAttributeList
        Dim balist As New CBandAttributeList

        '開始位置
        Dim idx As Integer = 1
        '後半を正順に / 前半を逆順に
        If isFromLast AndAlso Not isReverse OrElse Not isFromLast AndAlso isReverse Then
            idx = _BandPositions(iExp).ListCount - count + 1 '1～の値
        End If
        Do While 0 < count

            Dim band As CBandPosition
            If isReverse Then
                band = _BandPositions(iExp).ByReverseIdx(idx)
            Else
                band = _BandPositions(iExp).ByIdx(idx)
            End If

            If band IsNot Nothing AndAlso band.m_row縦横展開 IsNot Nothing Then
                Dim ba As New CBandAttribute(band.m_row縦横展開, isTerminal)
                balist.Add(ba)
            Else
                'あるはず・スペースフォルダとして
                balist.Add(Nothing)
            End If

            idx += 1
            count -= 1
        Loop

        Return balist
    End Function

    '両端のバンド色をf_sVal1,f_sVal2,f_iVal1,f_iVal2にセットする
    Friend Function set縦横展開Color(ByVal isOriColor As Boolean, ByVal is外側 As Boolean) As Boolean

        '補強ひもも含め全てクリア
        For Each r As tbl縦横展開Row In get横展開DataTable()
            r.f_iVal1 = 0
            r.f_iVal2 = 0
            r.f_sVal1 = ""
            r.f_sVal2 = ""
        Next
        For Each r As tbl縦横展開Row In get縦展開DataTable()
            r.f_iVal1 = 0
            r.f_iVal2 = 0
            r.f_sVal1 = ""
            r.f_sVal2 = ""
        Next

        If isOriColor Then
            '側面を構成する全バンド
            Dim oriTable As dstWork.tblOriColorDataTable = GetOriColorTable()
            If oriTable Is Nothing Then
                Return False
            End If
            '
            For Each oriRow As dstWork.tblOriColorRow In oriTable
                If is外側 Then
                    If oriRow.f_b外側_135 Then
                        '135度を45度に重ねる
                        Dim r45 As tbl縦横展開Row = find縦横展開Row(oriRow.f_iひも種_45, oriRow.f_iひも番号_45)
                        If r45 IsNot Nothing Then
                            If oriRow.f_bTerminal_45 Then
                                r45.f_iVal2 = 1
                                r45.f_sVal2 = oriRow.f_s外側色_45
                            Else
                                r45.f_iVal1 = 1
                                r45.f_sVal1 = oriRow.f_s外側色_45
                            End If
                        End If
                    End If
                    If oriRow.f_b外側_45 Then
                        '45度を135度に重ねる
                        Dim r135 As tbl縦横展開Row = find縦横展開Row(oriRow.f_iひも種_135, oriRow.f_iひも番号_135)
                        If r135 IsNot Nothing Then
                            If oriRow.f_bTerminal_135 Then
                                r135.f_iVal2 = 1
                                r135.f_sVal2 = oriRow.f_s外側色_135
                            Else
                                r135.f_iVal1 = 1
                                r135.f_sVal1 = oriRow.f_s外側色_135
                            End If
                        End If
                    End If

                Else
                    If oriRow.f_b内側_135 Then
                        '135度を45度に重ねる
                        Dim r45 As tbl縦横展開Row = find縦横展開Row(oriRow.f_iひも種_45, oriRow.f_iひも番号_45)
                        If r45 IsNot Nothing Then
                            If oriRow.f_bTerminal_45 Then
                                r45.f_iVal2 = 1
                                r45.f_sVal2 = oriRow.f_s内側色_45
                            Else
                                r45.f_iVal1 = 1
                                r45.f_sVal1 = oriRow.f_s内側色_45
                            End If
                        End If
                    End If
                    If oriRow.f_b内側_45 Then
                        '45度を135度に重ねる
                        Dim r135 As tbl縦横展開Row = find縦横展開Row(oriRow.f_iひも種_135, oriRow.f_iひも番号_135)
                        If r135 IsNot Nothing Then
                            If oriRow.f_bTerminal_135 Then
                                r135.f_iVal2 = 1
                                r135.f_sVal2 = oriRow.f_s内側色_135
                            Else
                                r135.f_iVal1 = 1
                                r135.f_sVal1 = oriRow.f_s内側色_135
                            End If
                        End If
                    End If

                End If

            Next

        End If

        Return True
    End Function



#End Region

    '【外側から見た図】
    '
    '横のひも番号    右上(A)            縦ひも X→→ 右上   
    '    ①          ／＼                            ／│      端:No              左上              右上   
    '    ②  +Y  横／    ＼縦                   ／││ │ (上)ひも長加算            __________________   ひも番号
    '    ③  ↑  ／        ＼右下(D)           │ ││ │       ↑                  ＼______________／     ①
    '    ④  │／          ／              ／││ ││ │       │                    ______________       ②
    '    ..  │＼左上(B) ／                ＼││ ││ │       ↓                 ↑ ＼__________／       ③
    '        │  ＼    ／                       ＼││ │ (下)ひも長加算2          ↑      ____
    '        │    ＼／  左下(C)                     ＼│      端:Yes          横ひも Y    ＼／
    '        ●─────→+X         ひも番号①②③ 右下                  (左)ひも長加算  ←─→  (右)ひも長加算2         
    '縦のひも番号①②③④...                                                   端:No                    端:Yes

    '
    '重ねたバンドの表示については、
    '・f_i描画種,f_s色2がセットされていれば、常に重ね表示(折りカラーによらず)
    '・通常の入力・編集処理においては、現状、これらのフィールドは不可視

#Region "結果表示用"

    '基本のひも幅と不一致がある
    ReadOnly Property p_b本幅変更あり As Boolean
        Get
            If _b縦横を展開する Then
                Return _BandPositions(emExp._Yoko)._本幅変更あり OrElse
                 _BandPositions(emExp._Tate)._本幅変更あり
            Else
                Return False
            End If
        End Get
    End Property

    '#84 0より大きいとき、すべて同じ本幅の値
    ReadOnly Property p_i同じ本幅 As Integer
        Get
            If 0 < _BandPositions(emExp._Yoko)._同じ本幅 AndAlso
                0 < _BandPositions(emExp._Tate)._同じ本幅 AndAlso
                _BandPositions(emExp._Yoko)._同じ本幅 = _BandPositions(emExp._Tate)._同じ本幅 Then
                Return _BandPositions(emExp._Yoko)._同じ本幅
            Else
                Return -1
            End If
        End Get
    End Property

    '#84 本幅変更時の高さ
    ReadOnly Property p_d側面の高さ As Double
        Get
            Return _dバンド側面の高さ
        End Get
    End Property

    '本幅ベースの長さでチェック
    ReadOnly Property p_b長方形である As Boolean
        Get
            If Not p_b本幅変更あり Then
                Return True
            End If
            If 0 < p_i同じ本幅 Then
                Return True
            End If
            If _BandPositions(emExp._Yoko)._本幅の計_Z <> _BandPositions(emExp._Tate)._本幅の計_Z Then
                Return False
            End If
            If _BandPositions(emExp._Yoko)._本幅の計_M <> _BandPositions(emExp._Tate)._本幅の計_M Then
                Return False
            End If
            If _BandPositions(emExp._Yoko)._本幅の計_P <> _BandPositions(emExp._Tate)._本幅の計_P Then
                Return False
            End If
            If _BandPositions(emExp._Yoko)._本幅の計_M <> _BandPositions(emExp._Yoko)._本幅の計_P Then
                Return False
            End If
            If _BandPositions(emExp._Tate)._本幅の計_M <> _BandPositions(emExp._Tate)._本幅の計_P Then
                Return False
            End If

            Return True
        End Get
    End Property


    ReadOnly Property p_i先の三角形の本幅の差 As Integer
        Get
            If Not p_b本幅変更あり Then
                Return 0
            End If
            Dim dif As Integer = _BandPositions(emExp._Yoko)._本幅の計_M - _BandPositions(emExp._Tate)._本幅の計_M
            If dif <> 0 Then
                Return dif
            End If
            Return _BandPositions(emExp._Yoko)._本幅の計_M - _BandPositions(emExp._Yoko)._本幅の計_P
        End Get
    End Property

    ReadOnly Property p_i四辺形の本幅の差 As Integer
        Get
            If Not p_b本幅変更あり Then
                Return 0
            End If
            Return _BandPositions(emExp._Yoko)._本幅の計_Z - _BandPositions(emExp._Tate)._本幅の計_Z
        End Get
    End Property

    ReadOnly Property p_i後の三角形の本幅の差 As Integer
        Get
            If Not p_b本幅変更あり Then
                Return 0
            End If
            Dim dif As Integer = _BandPositions(emExp._Yoko)._本幅の計_P - _BandPositions(emExp._Tate)._本幅の計_P
            If dif <> 0 Then
                Return dif
            End If
            Return _BandPositions(emExp._Tate)._本幅の計_M - _BandPositions(emExp._Tate)._本幅の計_P
        End Get
    End Property

    ReadOnly Property p_d底の横長(Optional alt As Boolean = False) As Double
        Get
            If alt Then
                Return New S差分(_a底領域.pC, _a底領域.pD).Length
            Else
                Return New S差分(_a底領域.pB, _a底領域.pA).Length
            End If
        End Get
    End Property

    ReadOnly Property p_d底の縦長(Optional alt As Boolean = False) As Double
        Get
            If alt Then
                Return New S差分(_a底領域.pB, _a底領域.pC).Length
            Else
                Return New S差分(_a底領域.pA, _a底領域.pD).Length
            End If
        End Get
    End Property

    ReadOnly Property p_d底の角度(ByVal pos As DirectionEnum) As Double
        Get
            Dim angle As Double
            Select Case pos
                Case DirectionEnum._上   '(A) AD-AB
                    angle = New S差分(_a底領域.pA, _a底領域.pD).Angle - New S差分(_a底領域.pA, _a底領域.pB).Angle

                Case DirectionEnum._左   '(B) BA-BC
                    angle = New S差分(_a底領域.pB, _a底領域.pA).Angle - New S差分(_a底領域.pB, _a底領域.pC).Angle

                Case DirectionEnum._下   '(C) CB-CD
                    angle = New S差分(_a底領域.pC, _a底領域.pB).Angle - New S差分(_a底領域.pC, _a底領域.pD).Angle

                Case DirectionEnum._右   '(D) DC-DA
                    angle = New S差分(_a底領域.pD, _a底領域.pC).Angle - New S差分(_a底領域.pD, _a底領域.pA).Angle

                Case Else
                    Return 0
            End Select
            If angle < 0 Then
                angle += 360
            End If
            If 360 < angle Then
                angle -= 360
            End If
            Return angle
        End Get
    End Property


#End Region


    '四角数,入力値(ひも長加算,ひも幅)がFixした状態で、長さを計算する
    Private Function calc_位置と長さ計算(ByVal is位置計算 As Boolean) As Boolean
        Dim ret As Boolean = True

        If is位置計算 Then
            '側面高さ
            _dバンド側面の高さ = -1

            '縦ひも,横ひもの配置位置
            _BandPositions(emExp._Yoko).CalcBasicPositions(_I基本のひも幅, _dひも間のすき間)
            _BandPositions(emExp._Tate).CalcBasicPositions(_I基本のひも幅, _dひも間のすき間)


            '底の四辺形
            _a底領域.pA = New S実座標(_BandPositions(emExp._Tate).p_d幅の累計(p_i横の四角数),
             _BandPositions(emExp._Yoko).p_d幅の累計(p_iひもの本数))
            _a底領域.pB = New S実座標(0,
             _BandPositions(emExp._Yoko).p_d幅の累計(_i縦の四角数))
            _a底領域.pC = New S実座標(_BandPositions(emExp._Tate).p_d幅の累計(_i縦の四角数),
                0)
            _a底領域.pD = New S実座標(_BandPositions(emExp._Tate).p_d幅の累計(p_iひもの本数),
             _BandPositions(emExp._Yoko).p_d幅の累計(_i横の四角数))


            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_a底={0}", _a底領域.ToString)

            '縦ひも・横ひもが底とクロスする領域
            Dim xbase As Double
            Dim ybase As Double
            '
            Dim BA As New S差分(_a底領域.pB, _a底領域.pA)
            xbase = _a底領域.pB.X
            ybase = _a底領域.pB.Y
            For x As Integer = 1 To p_i横の四角数
                '縦ひもの上側
                _BandPositions(emExp._Tate).ByXY(x).m_a底の四隅.p左上 = _a底領域.pB + BA * Abs((_BandPositions(emExp._Tate).p_d幅の累計(x - 1) - xbase) / BA.dX)
                _BandPositions(emExp._Tate).ByXY(x).m_a底の四隅.p右上 = _a底領域.pB + BA * Abs((_BandPositions(emExp._Tate).p_d幅の累計(x) - xbase) / BA.dX)

                '横ひもの左側
                Dim y As Integer = x + p_i縦の四角数
                _BandPositions(emExp._Yoko).ByXY(y).m_a底の四隅.p左下 = _a底領域.pB + BA * Abs((_BandPositions(emExp._Yoko).p_d幅の累計(y - 1) - ybase) / BA.dY)
                _BandPositions(emExp._Yoko).ByXY(y).m_a底の四隅.p左上 = _a底領域.pB + BA * Abs((_BandPositions(emExp._Yoko).p_d幅の累計(y) - ybase) / BA.dY)
            Next

            Dim AD As New S差分(_a底領域.pA, _a底領域.pD)
            xbase = _a底領域.pA.X
            ybase = _a底領域.pA.Y
            For x As Integer = p_i横の四角数 + 1 To p_iひもの本数
                '縦ひもの上側
                _BandPositions(emExp._Tate).ByXY(x).m_a底の四隅.p左上 = _a底領域.pA + AD * Abs((_BandPositions(emExp._Tate).p_d幅の累計(x - 1) - xbase) / AD.dX)
                _BandPositions(emExp._Tate).ByXY(x).m_a底の四隅.p右上 = _a底領域.pA + AD * Abs((_BandPositions(emExp._Tate).p_d幅の累計(x) - xbase) / AD.dX)

                '横ひもの右側
                Dim i As Integer = x - p_i横の四角数
                _BandPositions(emExp._Yoko).ByIdx(i).m_a底の四隅.p右下 = _a底領域.pA + AD * Abs((_BandPositions(emExp._Yoko).p_d幅の累計by番号(i) - ybase) / AD.dY)
                _BandPositions(emExp._Yoko).ByIdx(i).m_a底の四隅.p右上 = _a底領域.pA + AD * Abs((_BandPositions(emExp._Yoko).p_d幅の累計by番号(i - 1) - ybase) / AD.dY)
            Next

            Dim BC As New S差分(_a底領域.pB, _a底領域.pC)
            xbase = _a底領域.pB.X
            ybase = _a底領域.pB.Y
            For x As Integer = 1 To p_i縦の四角数
                '縦ひもの下側
                _BandPositions(emExp._Tate).ByXY(x).m_a底の四隅.p左下 = _a底領域.pB + BC * Abs((_BandPositions(emExp._Tate).p_d幅の累計(x - 1) - xbase) / BC.dX)
                _BandPositions(emExp._Tate).ByXY(x).m_a底の四隅.p右下 = _a底領域.pB + BC * Abs((_BandPositions(emExp._Tate).p_d幅の累計(x) - xbase) / BC.dX)

                '横ひもの左側
                Dim i As Integer = x + p_i横の四角数
                _BandPositions(emExp._Yoko).ByIdx(i).m_a底の四隅.p左下 = _a底領域.pB + BC * Abs((_BandPositions(emExp._Yoko).p_d幅の累計by番号(i) - ybase) / BC.dY)
                _BandPositions(emExp._Yoko).ByIdx(i).m_a底の四隅.p左上 = _a底領域.pB + BC * Abs((_BandPositions(emExp._Yoko).p_d幅の累計by番号(i - 1) - ybase) / BC.dY)
            Next

            Dim CD As New S差分(_a底領域.pC, _a底領域.pD)
            xbase = _a底領域.pC.X
            ybase = _a底領域.pC.Y
            For x As Integer = p_i縦の四角数 + 1 To p_iひもの本数
                '縦ひもの下側
                _BandPositions(emExp._Tate).ByXY(x).m_a底の四隅.p左下 = _a底領域.pC + CD * Abs((_BandPositions(emExp._Tate).p_d幅の累計(x - 1) - xbase) / CD.dX)
                _BandPositions(emExp._Tate).ByXY(x).m_a底の四隅.p右下 = _a底領域.pC + CD * Abs((_BandPositions(emExp._Tate).p_d幅の累計(x) - xbase) / CD.dX)

                '横ひもの右側
                Dim y As Integer = x - p_i縦の四角数
                _BandPositions(emExp._Yoko).ByXY(y).m_a底の四隅.p右下 = _a底領域.pC + CD * Abs((_BandPositions(emExp._Yoko).p_d幅の累計(y - 1) - ybase) / CD.dY)
                _BandPositions(emExp._Yoko).ByXY(y).m_a底の四隅.p右上 = _a底領域.pC + CD * Abs((_BandPositions(emExp._Yoko).p_d幅の累計(y) - ybase) / CD.dY)
            Next

            '底部分の長さ計算
            _BandPositions(emExp._Yoko).CalcBandLength()
            _BandPositions(emExp._Tate).CalcBandLength()

            '補強ひもの長さをセットする
            '45度は横
            _BandPositions(emExp._Yoko).Set補強ひも長(1, BA.Length)
            _BandPositions(emExp._Yoko).Set補強ひも長(2, CD.Length)
            '315度は縦
            _BandPositions(emExp._Tate).Set補強ひも長(1, BC.Length)
            _BandPositions(emExp._Tate).Set補強ひも長(2, AD.Length)

            '側面高さのキャッシュ値(バンド積み上げ)
            If 0 < p_i同じ本幅 Then
                'p_b長方形である=true
                _dバンド側面の高さ = (g_clsSelectBasics.p_d指定本幅(p_i同じ本幅) + _dひも間のすき間) * ROOT2
            ElseIf Not p_b長方形である Then
                '非長方形・概算
                _dバンド側面の高さ = (p_d底の横長 + p_d底の縦長) * (_d高さの四角数 / (_i横の四角数 + _i縦の四角数))
                '底が長方形になっていません。横{0:f1} ({1:f1})  縦{2:f1} ({3:f1})
                p_s警告 = String.Format(My.Resources.CalcNoRectangle,
                        p_d底の横長, p_d底の横長(True), p_d底の縦長, p_d底の縦長(True))
            Else
                '長方形である
                Dim modelImageData = New clsModelSquare45(Me, Nothing)
                _dバンド側面の高さ = modelImageData.CalcHeight()
            End If

            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList={0}", _BandPositions(emExp._Yoko).ToString)
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CBandPositionList={0}", _BandPositions(emExp._Tate).ToString)
        End If

        '長さを反映
        ret = ret And adjust_ひも(_tbl縦横展開(emExp._Yoko))
        ret = ret And adjust_ひも(_tbl縦横展開(emExp._Tate))

        Return ret
    End Function



    'ひもの底位置
    Friend Class CBandPosition
        Dim _Idx As Integer

        Public m_row縦横展開 As tbl縦横展開Row
        Public m_a底の四隅 As S四隅
        Public m_dひも幅 As Double '何本幅分


        Sub New(ByVal i As Integer)
            _Idx = i
        End Sub

        '        Function AddImageItemToList(ByVal imglist As clsImageItemList, dir As emExp) As Boolean
        '            If m_row縦横展開 Is Nothing Then
        '                Return False
        '            End If
        '            Dim item As New clsImageItem(m_row縦横展開, Not IsDrawMarkCurrent)
        '            item.m_a四隅 = m_a底の四隅

        '            Dim p中央 As S実座標 = m_a底の四隅.p中央

        '            item.m_dひも幅 = m_dひも幅
        '            With item.m_row縦横展開
        '                If dir = emExp._Yoko Then
        '                    item.m_rひも位置.y最上 = p中央.Y + item.m_dひも幅 / 2
        '                    item.m_rひも位置.y最下 = p中央.Y - item.m_dひも幅 / 2
        '                    item.m_rひも位置.x最右 = p中央.X + .f_dVal2 '右 .f_d出力ひも長 / 2
        '                    item.m_rひも位置.x最左 = p中央.X - .f_dVal1 '左 .f_d出力ひも長 / 2

        '                ElseIf dir = emExp._Tate Then
        '                    item.m_rひも位置.x最右 = p中央.X + item.m_dひも幅 / 2
        '                    item.m_rひも位置.x最左 = p中央.X - item.m_dひも幅 / 2
        '                    item.m_rひも位置.y最上 = p中央.Y + .f_dVal1 '上.f_d出力ひも長 / 2
        '                    item.m_rひも位置.y最下 = p中央.Y - .f_dVal2 '下.f_d出力ひも長 / 2

        '                End If
        '            End With
        '            imglist.Add(item)

        '#If 0 Then
        '            '四隅領域の描画
        '            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, _Idx)
        '            item.m_a四隅 = m_a底の四隅
        '            imglist.AddItem(item)
        '#End If
        '            Return True
        '        End Function

        Function ToBandImageItem(ByVal dir As emExp, ByVal idx As Integer) As clsImageItem
            If m_row縦横展開 Is Nothing Then
                Return Nothing
            End If
            Dim band As New CBand(m_row縦横展開)

            Dim p中央 As S実座標 = m_a底の四隅.p中央
            Dim index1 As Integer
            With m_row縦横展開
                If dir = emExp._Yoko Then
                    '左→右
                    band.SetBand(
                        New S実座標(p中央.X - .f_dVal1, p中央.Y),
                        New S実座標(p中央.X + .f_dVal2, p中央.Y),
                        m_dひも幅,
                        Unit90)

                    If IsDrawMarkCurrent Then
                        '横バンドの左
                        band.SetMarkPosition(enumMarkPosition._始点Fの前, m_dひも幅 * 1.4)
                    End If
                    index1 = 2 'ImageTypeEnum._横バンド

                ElseIf dir = emExp._Tate Then
                    '下→上
                    band.SetBand(
                        New S実座標(p中央.X, p中央.Y - .f_dVal2),
                        New S実座標(p中央.X, p中央.Y + .f_dVal1),
                        m_dひも幅,
                        Unit180)

                    If IsDrawMarkCurrent Then
                        '縦バンドの上
                        band.SetMarkPosition(enumMarkPosition._終点Tの後)
                    End If
                    index1 = 1 'ImageTypeEnum._縦バンド

                Else
                    Return Nothing

                End If
            End With

            '#96
            If m_row縦横展開.f_i描画種 = enum描画種.i_重ねる Then
                band.SetColorDraw(m_row縦横展開.f_s色2, enum描画種.i_重ねる)
            End If

            Dim imgItem As clsImageItem = New clsImageItem(band, index1, idx)
            imgItem.m_row縦横展開 = m_row縦横展開 'for GetRowItem,isDrawingItem
            Return imgItem
        End Function

        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("Idx={0} ひも幅({1:f1})", _Idx, m_dひも幅).Append(vbTab)
            sb.AppendFormat("底の四隅:({0})", m_a底の四隅).Append(vbTab)
            If m_row縦横展開 IsNot Nothing Then
                sb.AppendFormat("row縦横展開:({0},{1},{2}){3}本幅 {4}", m_row縦横展開.f_iひも種, m_row縦横展開.f_iひも番号, m_row縦横展開.f_i位置番号, m_row縦横展開.f_i何本幅, m_row縦横展開.f_s色)
            Else
                sb.Append("No row縦横展開")
            End If
            Return sb.ToString
        End Function
    End Class

    '展開テーブルの位置計算用
    Friend Class CBandPositionList
        Dim _Direction As emExp


        Dim _iひもの本数 As Integer = 0

        'Idx(ひも番号)順
        Dim _BandList As New List(Of CBandPosition)

        Dim _row補強ひも(1) As tbl縦横展開Row 'idx1→0, idx2=1


        'X/Y座標順, ひもが置かれる位置 横ひも=下から上へ, 縦ひも=左から右へ
        Dim _d幅の累計() As Double

        '集計結果
        Public _本幅変更あり As Boolean
        Public _同じ本幅 As Integer '初期値0,不一致はマイナス

        Public _本幅の計_M As Integer
        Public _本幅の計_Z As Integer
        Public _本幅の計_P As Integer

        Public Function ListCount() As Integer
            Return _BandList.Count
        End Function

        '指定位置の要素 ひも番号:1～_iひもの本数　で使用
        ReadOnly Property ByIdx(ByVal idx As Integer) As CBandPosition
            Get
                If idx < 1 OrElse _iひもの本数 < idx Then
                    Return Nothing
                End If
                Return _BandList(idx - 1)
            End Get
        End Property

        '最期から指定位置の要素 ひも番号:1～_iひもの本数　で使用
        ReadOnly Property ByReverseIdx(ByVal idx As Integer) As CBandPosition
            Get
                If idx < 1 OrElse _iひもの本数 < idx Then
                    Return Nothing
                End If
                Return _BandList(_iひもの本数 - idx)
            End Get
        End Property

        'X,Yの小さい方から大きい方へ　:1～_iひもの本数
        ReadOnly Property ByXY(ByVal xy As Integer) As CBandPosition
            Get
                If 1 <= xy AndAlso xy <= _iひもの本数 Then
                    If _Direction = emExp._Tate Then
                        '縦ひもの場合はそのまま
                        Return _BandList(xy - 1)
                    ElseIf _Direction = emExp._Yoko Then
                        '横ひもは逆からの位置
                        Return _BandList(_iひもの本数 - xy)
                    End If
                End If
                Return Nothing
            End Get
        End Property

        '座標方向に 0～_iひもの本数
        ReadOnly Property p_d幅の累計(ByVal xy As Integer) As Double
            Get
                If xy < 0 OrElse _d幅の累計 Is Nothing OrElse _d幅の累計.Length <= xy Then
                    Return 0
                End If
                Return _d幅の累計(xy)
            End Get
        End Property

        ReadOnly Property p_d幅の累計by番号(ByVal idx As Integer) As Double
            Get
                If _Direction = emExp._Tate Then
                    '縦ひもの場合はそのまま
                    Return p_d幅の累計(idx)
                ElseIf _Direction = emExp._Yoko Then
                    '横ひもは逆からの位置
                    Return p_d幅の累計(_iひもの本数 - idx)
                End If
                Return 0
            End Get
        End Property



        Sub New(ByVal dir As emExp)
            _Direction = dir
        End Sub

        Sub Clear()
            SetBandCount(0)
        End Sub

        '指定サイズにする
        Private Function SetBandCount(ByVal bcount As Integer) As Boolean
            _iひもの本数 = bcount

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
                    _BandList.Add(New CBandPosition(_BandList.Count + 1))
                Loop
            End If
            Return True
        End Function

        'テーブルのレコードをセットする
        Function SetTable(ByVal table As tbl縦横展開DataTable, ByVal bcount As Integer) As Boolean
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                SetBandCount(0)
                Return False
            End If

            Dim ret As Boolean = True
            Dim setcount As Integer = 0
            SetBandCount(bcount)
            For Each row As tbl縦横展開Row In table
                Dim idx As Integer = row.f_iひも番号

                If is補強ひも(row) Then
                    If idx = 1 OrElse idx = 2 Then
                        _row補強ひも(idx - 1) = row
                    Else
                        ret = False
                    End If
                Else
                    '処理のひも
                    If 1 <= idx AndAlso idx <= bcount Then
                        ByIdx(idx).m_row縦横展開 = row
                        setcount += 1
                    Else
                        ret = False
                    End If
                End If
            Next
            Return ret And (setcount = bcount)
        End Function


        '基本的な配置情報を計算する
        Function CalcBasicPositions(ByVal I基本のひも幅 As Integer, ByVal dひも間のすき間 As Double) As Boolean

            _本幅変更あり = False
            _同じ本幅 = 0
            _本幅の計_M = 0
            _本幅の計_Z = 0
            _本幅の計_P = 0

            ReDim _d幅の累計(_iひもの本数)
            _d幅の累計(0) = 0

            '縦ひも:左から右へ
            '横ひも:下から上へ
            Dim d幅の計 As Double = 0
            For xy As Integer = 1 To _iひもの本数
                Dim band As CBandPosition = ByXY(xy)
                Dim row As tbl縦横展開Row = band.m_row縦横展開

                band.m_dひも幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)

                d幅の計 += (band.m_dひも幅 + dひも間のすき間)
                _d幅の累計(xy) = d幅の計

                If row.f_i何本幅 <> I基本のひも幅 Then
                    _本幅変更あり = True
                End If
                If _同じ本幅 = 0 Then
                    _同じ本幅 = row.f_i何本幅
                ElseIf 0 < _同じ本幅 Then
                    If _同じ本幅 <> row.f_i何本幅 Then
                        _同じ本幅 = -1
                    End If
                End If
                If row.f_i位置番号 < 0 Then
                    _本幅の計_M += row.f_i何本幅
                ElseIf 0 < row.f_i位置番号 Then
                    _本幅の計_P += row.f_i何本幅
                Else
                    _本幅の計_Z += row.f_i何本幅
                End If
            Next
            Return True
        End Function

        '底の四隅から長さを計算する
        Function CalcBandLength() As Boolean
            For i As Integer = 1 To _iひもの本数
                Dim band As CBandPosition = ByIdx(i)
                If _Direction = emExp._Tate Then
                    '縦ひもは上下
                    Dim d1 As Double = band.m_a底の四隅.p左上.Y - band.m_a底の四隅.p左下.Y
                    Dim d2 As Double = band.m_a底の四隅.p右上.Y - band.m_a底の四隅.p右下.Y
                    band.m_row縦横展開.f_d長さ = (d1 + d2) / 2

                ElseIf _Direction = emExp._Yoko Then
                    '横ひもは左右
                    Dim d1 As Double = band.m_a底の四隅.p右上.X - band.m_a底の四隅.p左上.X
                    Dim d2 As Double = band.m_a底の四隅.p右下.X - band.m_a底の四隅.p左下.X
                    band.m_row縦横展開.f_d長さ = (d1 + d2) / 2

                Else
                    Return False
                End If
            Next
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
                'bandposition.AddImageItemToList(imglist, _Direction)
                Dim item As clsImageItem = bandposition.ToBandImageItem(_Direction, idx)
                imglist.Add(item)
            Next
            Return imglist
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("本幅の計:{0},{1},{2}", _本幅の計_M, _本幅の計_Z, _本幅の計_P)
            If _本幅変更あり Then
                sb.Append(_Direction).AppendFormat("本幅変更あり(同じ本幅={0})", _同じ本幅).AppendLine()
            Else
                sb.Append(_Direction).Append("本幅変更なし").AppendLine()
            End If
            If _d幅の累計 IsNot Nothing Then
                sb.Append("幅の累計")
                For Each val As Double In _d幅の累計
                    sb.AppendFormat("{0:f2},", val)
                Next
                sb.AppendLine()
            End If

            For Each band As CBandPosition In _BandList
                sb.AppendLine(band.ToString)
            Next
            Return sb.ToString
        End Function

    End Class


    '現画像生成時に記号を表示する
    Shared IsDrawMarkCurrent As Boolean = True

    'プレビュー処理
    Public Function CalcImage(ByVal imgData As clsImageData, ByVal isBackFace As Boolean) As Boolean
        '記号順が変わるので裏面には表示しない
        IsDrawMarkCurrent = Not isBackFace AndAlso
            Not String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号)

        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '原点
        imgData.CenterCoordinates = _a底領域.p中央

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        Dim _ImageList横ひも As clsImageItemList = Nothing
        Dim _ImageList縦ひも As clsImageItemList = Nothing

        Try
            _ImageList横ひも = _BandPositions(emExp._Yoko).ConvertToImageList()
            _ImageList縦ひも = _BandPositions(emExp._Tate).ConvertToImageList()
            If _ImageList横ひも Is Nothing OrElse _ImageList横ひも Is Nothing Then
                Throw New Exception()
            End If
            If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
                Throw New Exception()
            End If

        Catch ex As Exception
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End Try

        '
        '基本のひも幅と基本色
        imgData.setBasics(_dひも幅の一辺, _Data.p_row目標寸法.Value("f_s基本色"))

        'ひも上下で指定
        If 0 <= _Data.p_row底_縦横.Value("f_iひも上下の高さ数") Then 'If _b縦横を展開する Then
            '描画用のデータ追加
            regionUpDown底(_ImageList横ひも, _ImageList縦ひも, isBackFace)
            'regionUpDown底(isBackFace)
        End If

        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing

        'その他の描画パーツ
        imgData.MoveList(imageList描画要素())

        If Not isBackFace Then
            '付属品
            AddPartsImage(imgData, _frmMain.editAddParts, False) '描画
        End If

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function



    'IN:    _d縁の高さ, p_d四角ベース_高さ
    '※上下左右の位置関係は満たしません
    Private Function getSideRectangle(ByVal item As clsImageItem, ByVal pS As S実座標, ByVal pE As S実座標) As Boolean
        Dim delta As New S差分(pS, pE)
        delta = delta.Rotate(90) '回転
        delta.Length = p_d四角ベース_高さ

        Dim pS1 As S実座標 = pS + delta
        Dim pE1 As S実座標 = pE + delta
        item.m_a四隅.pA = pS
        item.m_a四隅.pB = pE
        item.m_a四隅.pD = pS1
        item.m_a四隅.pC = pE1

        delta.Length = _d縁の高さ
        Dim pS2 As S実座標 = pS1 + delta
        Dim pE2 As S実座標 = pE1 + delta
        Dim line As New S線分(pS2, pE2)
        item.m_lineList.Add(line)

        Return True
    End Function


    Function imageList描画要素() As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList


        If Not p_b長方形である Then
            '全体枠
            Dim d差の半分 As Double = _d四角の一辺 * ((_i横の四角数 - _i縦の四角数) / 2)
            Dim d和の半分 As Double = _d四角の一辺 * (2 * _d高さの四角数 + (_i横の四角数 + _i縦の四角数) / 2) + 2 * (_d縁の高さ / ROOT2)
            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, 1)

            item.m_a四隅.pA = New S実座標(d差の半分, d和の半分)
            item.m_a四隅.pC = -item.m_a四隅.pA
            item.m_a四隅.pD = New S実座標(d和の半分, d差の半分)
            item.m_a四隅.pB = -item.m_a四隅.pD

            Dim delta As New S差分(pOrigin, _a底領域.p中央)
            item.m_a四隅.pA = item.m_a四隅.pA + delta
            item.m_a四隅.pC = item.m_a四隅.pC + delta
            item.m_a四隅.pD = item.m_a四隅.pD + delta
            item.m_a四隅.pB = item.m_a四隅.pB + delta
            itemlist.AddItem(item)

        End If

        '底の画像
        If Not String.IsNullOrWhiteSpace(p_sBottomPngFilePath(False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 1)
            item.m_fpath = p_sBottomPngFilePath(False)
            item.m_angle = _PlateAngle(_idxBottomPngPlate)
            item.m_a四隅 = _a底領域

            If _isBottomPngTrimHalfBand Then '0.5高さの分を縮める
                Dim delta As S差分 = New S差分(_d四角の対角線 / 2, 0).Rotate(_PlateAngle(_idxBottomPngPlate) - 90) '0度から回転
                If _idxBottomPngPlate = enumBasketPlateIdx._leftside Then
                    'B,C
                    item.m_a四隅.pB = item.m_a四隅.pB + delta
                    item.m_a四隅.pC = item.m_a四隅.pC + delta
                ElseIf _idxBottomPngPlate = enumBasketPlateIdx._front Then
                    'A,B
                    item.m_a四隅.pA = item.m_a四隅.pA + delta
                    item.m_a四隅.pB = item.m_a四隅.pB + delta
                ElseIf _idxBottomPngPlate = enumBasketPlateIdx._rightside Then
                    'D,A
                    item.m_a四隅.pD = item.m_a四隅.pD + delta
                    item.m_a四隅.pA = item.m_a四隅.pA + delta
                ElseIf _idxBottomPngPlate = enumBasketPlateIdx._back Then
                    'C,D
                    item.m_a四隅.pC = item.m_a四隅.pC + delta
                    item.m_a四隅.pD = item.m_a四隅.pD + delta
                End If

            End If
            itemlist.AddItem(item)
        End If

        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = _a底領域
        itemlist.AddItem(item)

        '横の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        getSideRectangle(item, _a底領域.pB, _a底領域.pA)
        itemlist.AddItem(item)

        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        getSideRectangle(item, _a底領域.pD, _a底領域.pC)
        itemlist.AddItem(item)

        '縦の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        getSideRectangle(item, _a底領域.pA, _a底領域.pD)
        itemlist.AddItem(item)

        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        getSideRectangle(item, _a底領域.pC, _a底領域.pB)
        itemlist.AddItem(item)

        '底の中央線
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)

        'クロス点
        Dim p上クロス点 As New S実座標(_a底領域.pA.X, Max(_a底領域.pB.Y, _a底領域.pD.Y))
        Dim p下クロス点 As New S実座標(_a底領域.pC.X, Min(_a底領域.pB.Y, _a底領域.pD.Y))

        Dim line As S線分
        If p下クロス点.X < p上クロス点.X Then
            line = New clsImageItem.S線分(p上クロス点, _a底領域.pD)
            item.m_lineList.Add(line)

            line = New clsImageItem.S線分(p下クロス点, _a底領域.pB)
            item.m_lineList.Add(line)
        Else
            line = New clsImageItem.S線分(p上クロス点, _a底領域.pB)
            item.m_lineList.Add(line)

            line = New clsImageItem.S線分(p下クロス点, _a底領域.pD)
            item.m_lineList.Add(line)
        End If

        line = New clsImageItem.S線分(_a底領域.pA, p上クロス点)
        item.m_lineList.Add(line)

        line = New clsImageItem.S線分(p下クロス点, _a底領域.pC)
        item.m_lineList.Add(line)

        line = New clsImageItem.S線分(p下クロス点, p上クロス点)
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        '#98 2倍高さライン
        If p_is折りカラー処理 Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域線, 1)
            Dim bt_sq As Double = p_d四角ベース_縦 / ROOT2
            Dim sd_sq As Double = p_d四角ベース_高さ * ROOT2
            Dim sq As Double = bt_sq + 2 * sd_sq
            If p下クロス点.X < p上クロス点.X Then
                item.m_a四隅.pA = p上クロス点 + Unit90 * sq
                item.m_a四隅.pB = p下クロス点 + Unit180 * sq
                item.m_a四隅.pC = p下クロス点 + Unit270 * sq
                item.m_a四隅.pD = p上クロス点 + Unit0 * sq
            Else
                item.m_a四隅.pA = p上クロス点 + Unit90 * sq
                item.m_a四隅.pB = p上クロス点 + Unit180 * sq
                item.m_a四隅.pC = p下クロス点 + Unit270 * sq
                item.m_a四隅.pD = p下クロス点 + Unit0 * sq
            End If
            item.m_ltype = LineTypeEnum._black_dot
            itemlist.AddItem(item)
        End If


        Return itemlist
    End Function

    Dim _CUpDown As New clsUpDown   'CheckBoxTableは使わない

    '#52 描画しない色
    Private Function isDrawingItem(ByVal item As clsImageItem) As Boolean
        If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
            Return False
        End If
        If g_clsSelectBasics.IsNoDrawingColor(item.m_row縦横展開.f_s色) Then
            Return False
        End If
        Return True
    End Function


    '底の上下をクリップ
    Private Function regionUpDown底(ByVal _ImageList横ひも As clsImageItemList, ByVal _ImageList縦ひも As clsImageItemList, ByVal isBackFace As Boolean) As Boolean
        _CUpDown.TargetFace = enumTargetFace.Bottom '底
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(0)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If

        'ひも上下の高さ・1回区分
        Dim updown_takasa As Integer = _Data.p_row底_縦横.Value("f_iひも上下の高さ数")
        If _Data.p_row底_縦横.Value("f_bひも上下1回区分") Then
            If Not _CUpDown.TrimTopLeft(updown_takasa, updown_takasa) Then
                Return True '不要
            End If
            _CUpDown.SetTrueInRangeOnly(True)
        Else
            _CUpDown.Shift(updown_takasa, updown_takasa)
            _CUpDown.SetTrueInRangeOnly(False)
        End If

        '裏面は左右反転
        Dim revRange As Integer = IIf(isBackFace, p_i縦ひもの本数, -1)

        '左上(0,0)からの開始位置
        Dim dx As Integer = 0
        Dim dy As Integer = 0
        If _is_updown_start_set Then
            dx = _updown_start_x
            dy = _updown_start_y
        End If

        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            'If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                If _CUpDown.GetIsDown(iTate - dx, iYoko - dy, revRange) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If isDrawingItem(itemYoko) Then
                        'itemTate.m_regionList.Add領域(itemYoko.m_rひも位置)
                        itemTate.AddClip(itemYoko)
                    End If
                End If
            Next
        Next

        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            'If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                If _CUpDown.GetIsUp(iTate - dx, iYoko - dy, revRange) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If isDrawingItem(itemTate) Then
                        'itemYoko.m_regionList.Add領域(itemTate.m_rひも位置)
                        itemYoko.AddClip(itemTate)
                    End If
                End If
            Next
        Next

        Return True
    End Function

End Class
