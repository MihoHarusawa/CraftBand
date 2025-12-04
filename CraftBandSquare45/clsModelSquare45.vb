Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBandSquare45.clsCalcSquare45

Public Class clsModelSquare45
    Inherits clsImageData

    Shared _PlateNames() As String = {"bottom", "leftside", "front", "rightside", "back"}

    '現対象
    Dim _calc As clsCalcSquare45


    '底と4側面
    Dim _region各面(cBasketPlateCount - 1) As S領域
    Dim _path各面画像(cBasketPlateCount - 1) As String
    Dim _data各面(cBasketPlateCount - 1) As clsDataTables
    Dim _delta画像サイズ(cBasketPlateCount - 1) As S差分


    '対象指定
    Sub New(ByVal calc As clsCalcSquare45, ByVal fpath As String)
        MyBase.New(fpath)
        _calc = calc
    End Sub

    Overloads Sub Clear()
        MyBase.Clear()
        For pidx As Integer = 0 To cBasketPlateCount - 1
            If _data各面(pidx) IsNot Nothing Then
                _data各面(pidx).Clear()
                _data各面(pidx) = Nothing
            End If
            If _SideBandStack(pidx) IsNot Nothing Then
                _SideBandStack(pidx).Clear()
                _SideBandStack(pidx) = Nothing
            End If
            '
            _region各面(pidx).Empty()
            _path各面画像(pidx) = Nothing

            _dxdyStart(pidx).clear()
            _IsDxDySet = False
        Next
    End Sub

    '#84 高さの算出(長方形の場合のみ・前面値)　※Calcの長さ計算用,プレビュー2には使わない
    Public Function CalcHeight() As Double
        If Not _calc.p_b長方形である Then
            Return -1
        End If

        '斜め各方向に下からバンドを積む
        setSideBandStack(_calc.p_i高さの切上四角数, False)

        '高さ=<前面>の<縦>
        Dim table縦 As tbl縦横展開DataTable = _SideBandStack(enumBasketPlateIdx._front).getBandTable(enumひも種.i_縦, CSideBandStack.emStack._45, True)
        Dim d高さの四角数 As Double = _calc.p_d高さの四角数
        Dim dひも間のすき間 As Double = _calc.p_dひも間のすき間
        Dim dHeight As Double = 0

        Dim rows() As tbl縦横展開Row = table縦.Select(Nothing, "f_iひも番号 ASC")
        For Each row As tbl縦横展開Row In rows
            If 1 < d高さの四角数 Then
                dHeight += g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
                dHeight += dひも間のすき間
                d高さの四角数 -= 1
            ElseIf 0 < d高さの四角数 Then
                Dim d = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + dひも間のすき間
                dHeight += d * d高さの四角数
                Exit For
            Else
                Exit For
            End If
        Next

        Return dHeight * ROOT2
    End Function


    Private Enum enumOriProc
        _none
        _before
        _after
    End Enum


    'プレビュー処理
    Public Function CalcModel(ByVal form As frmMain) As Boolean
        Dim isOriAfter As Boolean = form.radAfter.Checked
        Clear()
        If Not _calc.p_b長方形である Then
            '{0}が長方形でないため描画できません。
            _LastError = String.Format(My.Resources.ModelNoRectangle, BasketPlateString(enumBasketPlateIdx._bottom))
            Return False
        End If

        Dim oriProc As enumOriProc = enumOriProc._none
        If _calc.p_is折りカラー処理 Then
            If isOriAfter Then
                oriProc = enumOriProc._after
            Else
                oriProc = enumOriProc._before
            End If
        End If

        '各面の領域をセットする
        If Not setRegions(oriProc) Then
            Return False
        End If

        '各面に対応した画像用dataの初期化と四角数
        If Not setDataEachPlate(oriProc) Then
            Return False
        End If

        '画像用dataから画像生成
        If Not getImages(oriProc) Then
            Return False
        End If


        '出力ひもリスト情報
        Dim outp As New clsOutput(MyBase.FilePath)
        If Not _calc.CalcOutput(outp) Then
            Return False '既にチェック済のはず
        End If

        '基本のひも幅と基本色
        setBasics(_calc.p_dひも幅の一辺, _calc._Data.p_row目標寸法.Value("f_s基本色"))

        '絵の貼付と面枠描画
        MoveList(imageList側面展開図(oriProc))

        '原点
        CenterCoordinates = _region各面(enumBasketPlateIdx._bottom).p中央
        '付属品
        'AddPartsImage(Me, form.editAddParts, True) '描画2
        AddPartsImage(Me, _calc._Data, True) '描画2

        'ファイル作成
        If Not MakeImage(outp) Then
            Return False
        End If

        Return True
    End Function

    '3Dモデルを開く
    Function ModelFileOpen() As Boolean

        Dim height As Double = _delta画像サイズ(1).dY '左側面
        If Not NearlyEqual(height, _delta画像サイズ(2).dY) OrElse
           Not NearlyEqual(height, _delta画像サイズ(3).dY) OrElse
           Not NearlyEqual(height, _delta画像サイズ(4).dY) Then
            '側面の高さが異なるため直方体になりません。
            _LastError = My.Resources.ModelDiffHeight
            Return False
        End If

        Dim outpath As String = Nothing
        If _calc._frmMain.radビューア.Checked Then
            outpath = IO.Path.Combine(IO.Path.GetTempPath, "Square45_model")
        End If

        ' OBJとMTLファイルの出力
        Return CreateOBJWithTextures(_calc.p_d底の横長, height, _calc.p_d底の縦長,
        _path各面画像, outpath)
    End Function


#Region "各面の画像生成用のデータ"

    '各面の領域
    '
    '     左側面  前面    右側面    背面            
    '  _leftside _front _rightside _back
    '      (1)      (2)     (3)     (4)
    '　　 ────────────────  .... fuchi   
    '　　┌──┬────┬──┬────┐    
    '　　│　　│　　　　│　　│　　　　│    ↑　　　(#96,折りカラー)
    '　　│　　│　　　　│　　│　　　　│    │　　　  なし:   そのまま高さ(0.5あり)
    '　　│　　│　　　　│　　│　　　　│　takasa　　  before: 倍の高さ    (0.5なし)
    '　　│　　│　　　　│　　│　　　　│    ↓　　　　after:  そのまま高さ(0.5あり)
    '　　●──┼────┼──┴────┘    
    '　　　　　│　 (0)　│                    ↑  
    '　　　　　│_bottom │                   tate 
    '　　　　　└────┘         　         ↓　
    ' 　　<tate><- yoko -><tate><- yoko ->


    '各面の領域をセットする
    Private Function setRegions(ByVal oriProc As enumOriProc) As Boolean
        '底の横
        Dim yoko As Double = _calc.p_d底の横長
        '底の縦
        Dim tate As Double = _calc.p_d底の縦長
        '高さ
        Dim takasa As Double = _calc.p_d四角ベース_高さ
        If oriProc = enumOriProc._before Then
            takasa *= 2
        End If

        Dim p下 As S実座標 = pOrigin
        Dim p上 As S実座標 = pOrigin + Unit90 * takasa

        '左側面
        p上 += Unit0 * tate
        _region各面(enumBasketPlateIdx._leftside) = New S領域(p下, p上)

        p下 += Unit0 * tate
        p上 += Unit0 * yoko
        _region各面(enumBasketPlateIdx._front) = New S領域(p下, p上)

        p下 += Unit0 * yoko
        p上 += Unit0 * tate
        _region各面(enumBasketPlateIdx._rightside) = New S領域(p下, p上)

        p下 += Unit0 * tate
        p上 += Unit0 * yoko
        _region各面(enumBasketPlateIdx._back) = New S領域(p下, p上)

        '底
        p下 = _region各面(enumBasketPlateIdx._front).p左下 + Unit270 * tate
        p上 = _region各面(enumBasketPlateIdx._rightside).p左下
        _region各面(enumBasketPlateIdx._bottom) = New S領域(p下, p上)

        Return True
    End Function

#Region "側面に回り込んで積まれるバンド"
    '    
    '                　　　　　　／⋱　
    '                      高さ／　　⋱縦　
    '     　　　⋰＼　　     ／ -45°←⋱
    '     　横⋰　　＼高さ ／　　　　   ／　
    '       ⋰ →45°　＼   ＼ 右側面 ／　　
    '     ⋰　　　　 　／     ＼　  ／　　　
    '     ＼ 前面   ／  ／＼　 ＼／　　　　
    '     　＼　　／横／　　＼縦　　　　
    '     　　＼／　／→45度　＼ 
    '      　　　／　　　　　／　　　
    '    　　　　＼  底    ／ ／＼　　　　　　
    '    　　　／＼＼　　／ ／横　＼高さ　　　
    '    高さ／　　＼＼／ ／-135↑←＼
    '      ／↑→135 ＼ ／　　　　 　⋰　
    '    ／　　　　　／　＼ 背面   ⋰　　
    '    ⋱左側面  ／　   　＼　　⋰　　　
    '    縦⋱　　／　　　　 　＼⋰　　　　
    '    　　⋱／　　　　    
    '    　
    '    　　　
    'バンドの属性    
    Friend Class CBandAttribute
        Dim _i何本幅 As Integer
        Dim _s色 As String
        '#96
        Dim _b折り重ね As Boolean = False
        Dim _s重ね色 As String

        Sub New(ByVal row As tbl縦横展開Row, ByVal isTerminal As Boolean)
            If row IsNot Nothing Then
                _i何本幅 = row.f_i何本幅
                _s色 = row.f_s色
                '#96
                If isTerminal Then
                    If row.f_iVal2 = 1 Then
                        _b折り重ね = True
                        _s重ね色 = row.f_sVal2
                    End If
                Else
                    If row.f_iVal1 = 1 Then
                        _b折り重ね = True
                        _s重ね色 = row.f_sVal1
                    End If
                End If
            End If
        End Sub

        Sub SetAttribute(ByVal row As tbl縦横展開Row)
            If row IsNot Nothing Then
                row.f_i何本幅 = _i何本幅
                row.f_s色 = _s色
                '#96
                If _b折り重ね Then
                    row.f_i描画種 = enum描画種.i_重ねる
                    row.f_s色2 = _s重ね色
                End If
            End If
        End Sub

        Overrides Function ToString() As String
            If _b折り重ね Then
                Return String.Format("[{0}]{1}<{2}", _i何本幅, _s色, _s重ね色)
            Else
                Return String.Format("[{0}]{1}", _i何本幅, _s色)
            End If
        End Function
    End Class

    'バンドの属性リスト
    Friend Class CBandAttributeList
        Inherits List(Of CBandAttribute)

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("CBandAttributeList({0})", Me.Count).AppendLine()
            Dim idx As Integer = 1
            For Each band As CBandAttribute In Me
                sb.AppendFormat("{0} {1}", idx, band.ToString).AppendLine()
                idx += 1
            Next
            Return sb.ToString
        End Function
    End Class

    'バンドの属性リスト、45度と135度の両方向
    Private Class CSideBandStack
        '側面を垂直に見た時のひもの角度
        Enum emStack
            _45 = 0
            _135 = 1
        End Enum

        Dim m_i横の四角数 As Integer
        Dim m_i縦の四角数 As Integer


        '斜め各方向に下から積むバンド
        Dim _BandAttributeList(1) As CBandAttributeList

        Sub New(ByVal yoko As Integer, ByVal tate As Integer)
            m_i横の四角数 = yoko
            m_i縦の四角数 = tate
            _BandAttributeList(emStack._45) = New CBandAttributeList
            _BandAttributeList(emStack._135) = New CBandAttributeList
        End Sub

        Sub Clear()
            _BandAttributeList(emStack._45).Clear()
            _BandAttributeList(emStack._135).Clear()
        End Sub

        Function getBandCount() As Integer
            Return m_i横の四角数 + m_i縦の四角数
        End Function


        Function AddBandAttributeList(ByVal vert As emStack, ByVal adlist As CBandAttributeList) As Boolean
            _BandAttributeList(vert).AddRange(adlist)
            If getBandCount() < _BandAttributeList(vert).Count Then
                '必要数
                _BandAttributeList(vert).RemoveRange(getBandCount(), _BandAttributeList(vert).Count - getBandCount())
                Return False '完了
            ElseIf _BandAttributeList(vert).Count = getBandCount() Then
                Return False '完了
            Else
                Return True '継続
            End If
        End Function

        Function getBandTable(ByVal iひも種 As enumひも種, ByVal vert As CSideBandStack.emStack, ByVal isReverse As Boolean) As tbl縦横展開DataTable
            Dim table As New tbl縦横展開DataTable

            Dim iひも番号 As Integer = 1
            Dim iNext As Integer = 1
            If isReverse Then
                iひも番号 = getBandCount()
                iNext = -1
            End If

            For Each bandattr As CBandAttribute In _BandAttributeList(vert)
                Dim row As tbl縦横展開Row = table.Newtbl縦横展開Row
                row.f_iひも種 = iひも種
                row.f_iひも番号 = iひも番号

                bandattr.SetAttribute(row)

                table.Rows.Add(row)
                iひも番号 += iNext
            Next

            Return table
        End Function
    End Class
    Dim _SideBandStack(cBasketPlateCount - 1) As CSideBandStack '0は使わない

    '45度方向に積むバンド
    Private Function set_45()
        Dim base(3) As CBandAttributeList

        '横ひも,順方向,縦の四角数,後半,左側(非端)
        base(0) = _calc.getBandAttributeList(emExp._Yoko, False, _calc.p_i縦の四角数, True, False)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_45 base({0}) {1}", 0, base(0))

        '縦ひも,順方向,横の四角数,後半,下側(端)
        base(1) = _calc.getBandAttributeList(emExp._Tate, False, _calc.p_i横の四角数, True, True)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_45 base({0}) {1}", 1, base(1))

        '横ひも,逆方向,縦の四角数,前半,右側(端)
        base(2) = _calc.getBandAttributeList(emExp._Yoko, True, _calc.p_i縦の四角数, False, True)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_45 base({0}) {1}", 2, base(2))

        '縦ひも,逆方向,横の四角数,前半,上側(非端)
        base(3) = _calc.getBandAttributeList(emExp._Tate, True, _calc.p_i横の四角数, False, False)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_45 base({0}) {1}", 3, base(3))

        For iData As Integer = 1 To cBasketPlateCount - 1
            Dim base_idx As Integer = {-1, 0, 3, 2, 1}(iData)

            Do While _SideBandStack(iData).AddBandAttributeList(CSideBandStack.emStack._45, base(base_idx))
                base_idx = Modulo(base_idx + 1, 4)
            Loop
        Next

        Return True
    End Function

    '135度方向に積むバンド
    Private Function set_135()
        Dim base(3) As CBandAttributeList

        '縦ひも,逆方向,縦の四角数,前半,下側(端)
        base(0) = _calc.getBandAttributeList(emExp._Tate, True, _calc.p_i縦の四角数, False, True)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_135 base({0}) {1}", 0, base(0))

        '横ひも,逆方向,横の四角数,前半,左側(非端)
        base(1) = _calc.getBandAttributeList(emExp._Yoko, True, _calc.p_i横の四角数, False, False)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_135 base({0}) {1}", 1, base(1))

        '縦ひも,順方向,縦の四角数,後半,上側(非端)
        base(2) = _calc.getBandAttributeList(emExp._Tate, False, _calc.p_i縦の四角数, True, False)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_135 base({0}) {1}", 2, base(2))

        '横ひも,順方向,横の四角数,後半,右側(端)
        base(3) = _calc.getBandAttributeList(emExp._Yoko, False, _calc.p_i横の四角数, True, True)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_135 base({0}) {1}", 3, base(3))

        For iData As Integer = 1 To cBasketPlateCount - 1
            Dim base_idx As Integer = {-1, 0, 1, 2, 3}(iData)

            Do While _SideBandStack(iData).AddBandAttributeList(CSideBandStack.emStack._135, base(base_idx))
                base_idx = Modulo(base_idx + 1, 4)
            Loop
        Next

        Return True
    End Function

    '斜めに積まれたデータを各面の縦ひも・横ひもにセット
    Function setDataFromStack() As Boolean
        Dim table横 As New tbl縦横展開DataTable
        Dim table縦 As New tbl縦横展開DataTable

        '左側面: 底の左下、縦×高さ
        table横 = _SideBandStack(enumBasketPlateIdx._leftside).getBandTable(enumひも種.i_横, CSideBandStack.emStack._45, False)
        table縦 = _SideBandStack(enumBasketPlateIdx._leftside).getBandTable(enumひも種.i_縦, CSideBandStack.emStack._135, True)

        _data各面(enumBasketPlateIdx._leftside).p_tbl縦横展開.Clear()
        _data各面(enumBasketPlateIdx._leftside).p_tbl縦横展開.Merge(table横)
        _data各面(enumBasketPlateIdx._leftside).p_tbl縦横展開.Merge(table縦)
        table横.Clear()
        table縦.Clear()

        '前面: 底の左上、横×高さ
        table横 = _SideBandStack(enumBasketPlateIdx._front).getBandTable(enumひも種.i_横, CSideBandStack.emStack._135, True)
        table縦 = _SideBandStack(enumBasketPlateIdx._front).getBandTable(enumひも種.i_縦, CSideBandStack.emStack._45, True)

        _data各面(enumBasketPlateIdx._front).p_tbl縦横展開.Clear()
        _data各面(enumBasketPlateIdx._front).p_tbl縦横展開.Merge(table横)
        _data各面(enumBasketPlateIdx._front).p_tbl縦横展開.Merge(table縦)
        table横.Clear()
        table縦.Clear()

        '右側面: 底の右上、縦×高さ
        table横 = _SideBandStack(enumBasketPlateIdx._rightside).getBandTable(enumひも種.i_横, CSideBandStack.emStack._45, True)
        table縦 = _SideBandStack(enumBasketPlateIdx._rightside).getBandTable(enumひも種.i_縦, CSideBandStack.emStack._135, False)

        _data各面(enumBasketPlateIdx._rightside).p_tbl縦横展開.Clear()
        _data各面(enumBasketPlateIdx._rightside).p_tbl縦横展開.Merge(table横)
        _data各面(enumBasketPlateIdx._rightside).p_tbl縦横展開.Merge(table縦)
        table横.Clear()
        table縦.Clear()

        '背面: 底の右下、横×高さ
        table横 = _SideBandStack(enumBasketPlateIdx._back).getBandTable(enumひも種.i_横, CSideBandStack.emStack._135, False)
        table縦 = _SideBandStack(enumBasketPlateIdx._back).getBandTable(enumひも種.i_縦, CSideBandStack.emStack._45, False)

        _data各面(enumBasketPlateIdx._back).p_tbl縦横展開.Clear()
        _data各面(enumBasketPlateIdx._back).p_tbl縦横展開.Merge(table横)
        _data各面(enumBasketPlateIdx._back).p_tbl縦横展開.Merge(table縦)
        table横.Clear()
        table縦.Clear()

        Return True
    End Function

#End Region

    'ひも上下の適用開始位置(1回のとき)
    Dim _IsDxDySet As Boolean = False
    Private Structure SDXDY
        Public dx As Integer
        Public dy As Integer
        Sub clear()
            dx = 0
            dy = 0
        End Sub
    End Structure
    Dim _dxdyStart(cBasketPlateCount - 1) As SDXDY

    '各面に合わせたひも上下
    Function setPlateUpDown(ByVal n高さの四角数 As Integer) As Boolean
        Dim updown As New clsUpDown   'CheckBoxTableは使わない
        If Not _calc._Data.ToClsUpDown(updown) OrElse Not updown.IsValid(False) Then 'チェックはMatrix
            updown.Reset(0)
        End If

        'ひも上下の高さ・1回区分
        Dim updown_takasa As Integer = _calc._Data.p_row底_縦横.Value("f_iひも上下の高さ数")
        Dim pidx As enumBasketPlateIdx
        Dim dAddEnd As Double = _calc.p_dひも幅の一辺 '#80 底をフルに描画
        If _calc._Data.p_row底_縦横.Value("f_bひも上下1回区分") Then
            '* 1回のみの適用

            '底位置に合わせる
            pidx = enumBasketPlateIdx._bottom
            Dim bottom As New clsUpDown(updown)
            _data各面(pidx).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80
            If bottom.TrimTopLeft(updown_takasa, updown_takasa) Then
                _data各面(pidx).FromClsUpDown(bottom)
                _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
                '左上から開始
                _dxdyStart(pidx).dx = 0
                _dxdyStart(pidx).dy = 0
            Else
                _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = -1
            End If

            '左側面: 底の左下、縦×高さ
            pidx = enumBasketPlateIdx._leftside
            Dim leftside As New clsUpDown(updown)
            _data各面(pidx).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80
            If leftside.TrimTopLeft(0, _calc.p_i横の四角数 + updown_takasa) Then
                _data各面(pidx).FromClsUpDown(leftside)
                _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
                _dxdyStart(pidx).dx = n高さの四角数 - updown_takasa
                _dxdyStart(pidx).dy = 0
            Else
                _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = -1
            End If

            '前面: 底の左上、横×高さ
            pidx = enumBasketPlateIdx._front
            Dim front As New clsUpDown(updown)
            _data各面(pidx).FromClsUpDown(front)
            _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
            _data各面(pidx).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80
            '(高さの四角-updown高さ)点から開始
            _dxdyStart(pidx).dx = n高さの四角数 - updown_takasa
            _dxdyStart(pidx).dy = n高さの四角数 - updown_takasa

            '右側面: 底の右上、縦×高さ
            pidx = enumBasketPlateIdx._rightside
            Dim rightside As New clsUpDown(updown)
            _data各面(pidx).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80
            If rightside.TrimTopLeft(_calc.p_i横の四角数 + updown_takasa, 0) Then
                _data各面(pidx).FromClsUpDown(rightside)
                _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
                _dxdyStart(pidx).dx = 0
                _dxdyStart(pidx).dy = n高さの四角数 - updown_takasa
            Else
                _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = -1
            End If

            '背面: 底の右下、横×高さ
            pidx = enumBasketPlateIdx._back
            Dim back As New clsUpDown(updown)
            _data各面(pidx).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80
            If back.TrimTopLeft(_calc.p_i縦の四角数 + updown_takasa, _calc.p_i縦の四角数 + updown_takasa) Then
                _data各面(pidx).FromClsUpDown(back)
                _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
                _dxdyStart(pidx).dx = 0
                _dxdyStart(pidx).dy = 0
            Else
                _data各面(pidx).p_row底_縦横.Value("f_iひも上下の高さ数") = -1
            End If

            _IsDxDySet = True

        Else
            '* 繰り返し適用

            '底位置に合わせる
            updown.Shift(updown_takasa, updown_takasa)
            _data各面(enumBasketPlateIdx._bottom).FromClsUpDown(updown)
            _data各面(enumBasketPlateIdx._bottom).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
            _data各面(enumBasketPlateIdx._bottom).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80

            '左側面: 底の左下、縦×高さ
            Dim leftside As New clsUpDown(updown)
            leftside.Shift(-n高さの四角数, _calc.p_i横の四角数)
            _data各面(enumBasketPlateIdx._leftside).FromClsUpDown(leftside)
            _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
            _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80

            '前面: 底の左上、横×高さ
            Dim front As New clsUpDown(updown)
            front.Shift(-n高さの四角数, -n高さの四角数)
            _data各面(enumBasketPlateIdx._front).FromClsUpDown(front)
            _data各面(enumBasketPlateIdx._front).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
            _data各面(enumBasketPlateIdx._front).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80

            '右側面: 底の右上、縦×高さ
            Dim rightside As New clsUpDown(updown)
            rightside.Shift(_calc.p_i横の四角数, -n高さの四角数)
            _data各面(enumBasketPlateIdx._rightside).FromClsUpDown(rightside)
            _data各面(enumBasketPlateIdx._rightside).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
            _data各面(enumBasketPlateIdx._rightside).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80

            '背面: 底の右下、横×高さ
            Dim back As New clsUpDown(updown)
            back.Shift(_calc.p_i縦の四角数, _calc.p_i縦の四角数)
            _data各面(enumBasketPlateIdx._back).FromClsUpDown(back)
            _data各面(enumBasketPlateIdx._back).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
            _data各面(enumBasketPlateIdx._back).p_row底_縦横.Value("f_dひも長加算") = dAddEnd '#80

        End If
        Return True
    End Function


    '各面に対応した画像用dataの初期化と四角数
    Private Function setDataEachPlate(ByVal oriProc As enumOriProc) As Boolean
        '画像用データ
        '_data各面(enumBasketPlateIdx._bottom) = New clsDataTables(_calc._Data)
        '_data各面(enumBasketPlateIdx._bottom).p_tbl追加品.Clear()
        '_data各面(enumBasketPlateIdx._bottom).p_row底_縦横.Value("f_b折りカラー区分") = False
        _data各面(enumBasketPlateIdx._bottom) = setDataAsImageBase()

        For pidx As Integer = 1 To cBasketPlateCount - 1
            _data各面(pidx) = New clsDataTables(_data各面(enumBasketPlateIdx._bottom))
        Next

        '底
        _data各面(enumBasketPlateIdx._bottom).p_row底_縦横.Value("f_d高さの四角数") = 0
        '_SideBandStackは使わない

        '底の0.5は作れないので整数値
        Dim n高さの四角数 As Integer
        If oriProc = enumOriProc._before Then
            n高さの四角数 = _calc.p_d高さの四角数 * 2
        Else
            n高さの四角数 = _calc.p_i高さの切上四角数
        End If

        '左側面: 底の左下、縦×高さ
        _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_i横の四角数") = n高さの四角数
        _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_i縦の四角数") = _calc.p_i縦の四角数
        _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_d高さの四角数") = 0
        '_SideBandStack(enumBasketPlateIdx._leftside) = New CSideBandStack(_calc.p_i高さの切上四角数, _calc.p_i縦の四角数)

        '前面: 底の左上、横×高さ
        _data各面(enumBasketPlateIdx._front).p_row底_縦横.Value("f_i横の四角数") = _calc.p_i横の四角数
        _data各面(enumBasketPlateIdx._front).p_row底_縦横.Value("f_i縦の四角数") = n高さの四角数
        _data各面(enumBasketPlateIdx._front).p_row底_縦横.Value("f_d高さの四角数") = 0
        '_SideBandStack(enumBasketPlateIdx._front) = New CSideBandStack(_calc.p_i横の四角数, _calc.p_i高さの切上四角数)

        '右側面: 底の右上、縦×高さ
        _data各面(enumBasketPlateIdx._rightside).p_row底_縦横.Value("f_i横の四角数") = n高さの四角数
        _data各面(enumBasketPlateIdx._rightside).p_row底_縦横.Value("f_i縦の四角数") = _calc.p_i縦の四角数
        _data各面(enumBasketPlateIdx._rightside).p_row底_縦横.Value("f_d高さの四角数") = 0
        '_SideBandStack(enumBasketPlateIdx._rightside) = New CSideBandStack(_calc.p_i高さの切上四角数, _calc.p_i縦の四角数)

        '背面: 底の右下、横×高さ
        _data各面(enumBasketPlateIdx._back).p_row底_縦横.Value("f_i横の四角数") = _calc.p_i横の四角数
        _data各面(enumBasketPlateIdx._back).p_row底_縦横.Value("f_i縦の四角数") = n高さの四角数
        _data各面(enumBasketPlateIdx._back).p_row底_縦横.Value("f_d高さの四角数") = 0
        '_SideBandStack(enumBasketPlateIdx._back) = New CSideBandStack(_calc.p_i横の四角数, _calc.p_i高さの切上四角数)


        '斜め各方向に下からバンドを積む
        setSideBandStack(n高さの四角数, oriProc = enumOriProc._after)

        '斜めに積まれたデータを各面の縦ひも・横ひもにセット
        setDataFromStack()

        '各面に合わせたひも上下
        setPlateUpDown(n高さの四角数)

        Return True
    End Function

    '各面の画像作成の元データ
    Private Function setDataAsImageBase() As clsDataTables
        Dim data As clsDataTables = New clsDataTables(_calc._Data)

        '画像に不要な部分を除外
        data.p_tbl追加品.Clear()

        '折り返しなし
        data.p_row底_縦横.Value("f_b折りカラー区分") = False

        '#103 加算ゼロ(展開によらず)
        For Each r As tbl縦横展開Row In data.p_tbl縦横展開.Rows
            r.f_dひも長加算 = 0
            r.f_dひも長加算2 = 0
        Next

        Return data
    End Function

    '斜め各方向に下からバンドを積む
    Private Function setSideBandStack(ByVal n高さの四角数 As Integer, ByVal isOriColor As Boolean) As Boolean
        '左側面: 底の左下、縦×高さ
        _SideBandStack(enumBasketPlateIdx._leftside) = New CSideBandStack(n高さの四角数, _calc.p_i縦の四角数)
        '前面: 底の左上、横×高さ
        _SideBandStack(enumBasketPlateIdx._front) = New CSideBandStack(_calc.p_i横の四角数, n高さの四角数)
        '右側面: 底の右上、縦×高さ
        _SideBandStack(enumBasketPlateIdx._rightside) = New CSideBandStack(n高さの四角数, _calc.p_i縦の四角数)
        '背面: 底の右下、横×高さ
        _SideBandStack(enumBasketPlateIdx._back) = New CSideBandStack(_calc.p_i横の四角数, n高さの四角数)

        '#96 バンドの折り返し色初期化
        _calc.set縦横展開Color(isOriColor, True) '外側の色

        '2方向にバンドを積む
        set_45()
        set_135()

        Return True
    End Function

    '画像用dataから画像生成
    Private Function getImages(ByVal oriProc As enumOriProc) As Boolean

        Dim ret As Boolean = True
        For pidx As Integer = 0 To cBasketPlateCount - 1
            _data各面(pidx).ResetStartPoint()
            _path各面画像(pidx) = IO.Path.Combine(IO.Path.GetTempPath, IO.Path.ChangeExtension(_PlateNames(pidx), CImageDraw.cImageClipFileExtention))

#If 0 Then 'DEBUG Then
            'Debug用・生成したデータファイルを保存
            '※Ver1.6より古いデータは、保存処理で幅が不一致になる可能性があります
            Dim fdir As String = IO.Path.Combine(IO.Path.GetTempPath, GetShortExeName(g_enumExeName))
            Dim fpath As String = IO.Path.GetFileNameWithoutExtension(MyBase.FilePath) & "-" & _PlateNames(pidx)
            fpath = IO.Path.Combine(fdir, fpath)
            fpath = IO.Path.ChangeExtension(fpath, IO.Path.GetExtension(MyBase.FilePath))
            Try
                If Not IO.Directory.Exists(fdir) Then
                    IO.Directory.CreateDirectory(fdir)
                End If
                If _data各面(pidx).Save(fpath) Then
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "SaveFile={0}", fpath)
                Else
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "SaveFileError={0}", _data各面(pidx).LastError)
                End If
            Catch ex As Exception
                g_clsLog.LogException(ex, fpath)
            End Try
#End If
            Dim isTrimHalf As Boolean = (0 < pidx) AndAlso (oriProc = enumOriProc._after) AndAlso _calc.p_b高さ半四角
            Dim calcTmp As New clsCalcSquare45(_data各面(pidx), _calc._frmMain)
            '画像ファイル名
            calcTmp.p_sBottomPngFilePath(True) = _path各面画像(pidx) 'あれば削除
            '面と0.5四角の有無を指定
            calcTmp.setBottomPngPlateIndex(pidx, isTrimHalf)

            If calcTmp.CalcSize(CalcCategory.NewData, Nothing, Nothing) Then
                If Not calcTmp.p_b長方形である Then
                    '{0}が長方形でないため描画できません。
                    _LastError = String.Format(My.Resources.ModelNoRectangle, BasketPlateString(pidx))
                    ret = False
                    '
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "getImages:{0} {1}", pidx, LastError)
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "calcTmp={0}", calcTmp.ToString)
                    'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "calcTmp={0}", calcTmp.dump())
                    Exit For
                End If

                If _IsDxDySet Then
                    'UpDown適用開始位置
                    calcTmp.setUpDownStartPosition(_dxdyStart(pidx).dx, _dxdyStart(pidx).dy)
                End If

                Dim imgdata As New clsImageData(_PlateNames(pidx)) '仮の名前で
                If Not calcTmp.CalcImage(imgdata, False) Then
                    _LastError = calcTmp.p_sメッセージ
                    ret = False
                ElseIf String.IsNullOrEmpty(calcTmp.p_sBottomPngFilePath(True)) Then
                    '{0}が描画できませんでした。
                    _LastError = String.Format(My.Resources.ModelNoImage, BasketPlateString(pidx))
                    ret = False
                End If
                If {1, 3}.Contains(pidx) Then
                    '右側面と左側面
                    _delta画像サイズ(pidx) = New S差分(calcTmp.p_d底の縦長, calcTmp.p_d底の横長)
                Else
                    '底と前面と背面
                    _delta画像サイズ(pidx) = New S差分(calcTmp.p_d底の横長, calcTmp.p_d底の縦長)
                End If
                If isTrimHalf Then
                    _delta画像サイズ(pidx).dY += -calcTmp.p_d四角の対角線 / 2
                End If

                imgdata.Clear()
                If Not ret Then
                    Exit For
                End If
            End If
            calcTmp.Clear()
        Next

        Return ret
    End Function
#End Region

    '絵の貼付と面枠描画
    Private Function imageList側面展開図(ByVal oriProc As enumOriProc) As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim line As S線分

        Dim fuchi As Double = _calc.p_d縁の高さ

        '側面
        For pidx As Integer = 1 To cBasketPlateCount - 1 '底以外
            '面枠
            item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, pidx)
            item.m_a四隅 = New S四隅(_region各面(pidx))

            line = New S線分(_region各面(pidx).p左上, _region各面(pidx).p右上)
            line += Unit90 * fuchi
            item.m_lineList.Add(line)
            itemlist.AddItem(item)

            '絵
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像貼付, pidx)
            Dim s As New S領域(_region各面(pidx).p左下, _region各面(pidx).p左下 + _delta画像サイズ(pidx))
            item.m_a四隅 = New S四隅(s)
            item.m_fpath = _path各面画像(pidx)
            itemlist.AddItem(item)
        Next

        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = New S四隅(_region各面(enumBasketPlateIdx._bottom))
        itemlist.AddItem(item)

        '底の絵
        item = New clsImageItem(clsImageItem.ImageTypeEnum._画像貼付, enumBasketPlateIdx._bottom)
        item.m_a四隅 = New S四隅(_region各面(enumBasketPlateIdx._bottom))
        item.m_fpath = _path各面画像(enumBasketPlateIdx._bottom)
        itemlist.AddItem(item)

        '折りカラーの折り返し線
        If oriProc <> enumOriProc._none Then

            '2倍高さ表示時のライン
            If oriProc = enumOriProc._before Then
                item = New clsImageItem(clsImageItem.ImageTypeEnum._折り返し線, 0)
                item.m_lineList = New C線分リスト

                Dim p1 As New S実座標(0, _calc.p_d四角ベース_高さ)
                Dim p2 As New S実座標((_calc.p_d底の横長 + _calc.p_d底の縦長) * 2, _calc.p_d四角ベース_高さ)
                line = New S線分(p1, p2)
                item.m_lineList.Add(line)

                item.m_ltype = LineTypeEnum._black_thin
                itemlist.AddItem(item)
            End If

            '個別バンドのライン
            addOriLine(Nothing, 0, 0, 0) '計算の初期値
            Dim oriTable As dstWork.tblOriColorDataTable = _calc.GetOriColorTable()
            If oriTable IsNot Nothing Then
                Dim lineList外側 As New C線分リスト
                Dim lineList内側 As New C線分リスト

                For Each oriRow As dstWork.tblOriColorRow In oriTable
                    If oriRow.f_b外側_135 OrElse oriRow.f_b外側_45 Then
                        '縁の折り返しライン
                        addOriLine(lineList外側, oriRow.f_iPlate, oriRow.f_iPosition, False)

                        '折り返し表示時は下のラインも表示
                        If oriProc = enumOriProc._after Then
                            If oriRow.f_b外側_45 Then
                                addOriLine(lineList外側, oriRow.f_iゼロ面_135, oriRow.f_iゼロ位置_135, True)
                            End If
                            If oriRow.f_b外側_135 Then
                                addOriLine(lineList外側, oriRow.f_iゼロ面_45, oriRow.f_iゼロ位置_45, True)
                            End If
                        End If

                    End If

                    If oriRow.f_b内側_135 OrElse oriRow.f_b内側_45 Then
                        '縁の折り返しラインだけ
                        addOriLine(lineList内側, oriRow.f_iPlate, oriRow.f_iPosition, False)
                    End If
                Next

                If 0 < lineList外側.Count Then
                    item = New clsImageItem(clsImageItem.ImageTypeEnum._折り返し線, 2)
                    item.m_lineList = lineList外側
                    item.m_ltype = LineTypeEnum._red
                    itemlist.AddItem(item)
                End If

                If 0 < lineList内側.Count Then
                    item = New clsImageItem(clsImageItem.ImageTypeEnum._折り返し線, 1)
                    item.m_lineList = lineList内側
                    item.m_ltype = LineTypeEnum._blue
                    itemlist.AddItem(item)
                End If
            End If
        End If

        Return itemlist
    End Function

    Private Sub addOriLine(ByVal llist As C線分リスト, ByVal plateIdx As Integer, ByVal position As Double, ByVal is底 As Boolean)
        '初期化で保持する変数
        Static deltaBand As S差分
        Static dHalf As Single
        Static dSpace As Double
        Static dYY As Double = _calc.p_d四角ベース_高さ
        Static aryLen() As Double

        'Nothing指定は初期化
        If llist Is Nothing Then
            deltaBand = New S差分(_calc.p_sひも幅の対角線, 0)
            dHalf = IIf(_calc.p_b高さ半四角, -0.5, 0)
            dSpace = (_calc.p_d四角の対角線 - _calc.p_sひも幅の対角線) / 2
            aryLen = {0, 0, _calc.p_d底の縦長, _calc.p_d底の縦長 + _calc.p_d底の横長, _calc.p_d底の横長 + _calc.p_d底の縦長 * 2}
            Return
        End If

        '線分を追加
        Dim dy As Double = IIf(is底, 0, dYY)
        Dim dx As Double = aryLen(plateIdx) + dSpace
        Dim x As Double = IIf(is底, position - 1, position - 1 + dHalf)
        dx += x * _calc.p_d四角の対角線
        Dim p1 As New S実座標(dx, dy)
        Dim p2 As S実座標 = p1 + deltaBand
        If 0 <= p1.X Then
            Dim line As S線分 = New S線分(p1, p2)
            llist.Add(line)
        Else
            Dim line As S線分 = New S線分(New S実座標(0, p1.Y), p2)
            llist.Add(line)
            line = New S線分(p1, New S実座標(0, p1.Y))
            llist.Add(line + New S差分(_calc.p_d底の横長 * 2 + _calc.p_d底の縦長 * 2, 0))
        End If
    End Sub

End Class

