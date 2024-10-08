Imports CraftBand
Imports CraftBand.clsImageData
Imports CraftBand.clsImageItem
Imports CraftBand.clsDataTables
Imports CraftBandSquare45.clsCalcSquare45
Imports CraftBand.Tables.dstDataTables

Public Class clsModelSquare45
    Inherits clsImageData

    Shared _PlateNames() As String = {"bottom", "leftside", "front", "rightside", "back"}
    Shared _PlateAngle() As Integer = {45, 135, 45, -45, -135}

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
        For i As Integer = 0 To cBasketPlateCount - 1
            If _data各面(i) IsNot Nothing Then
                _data各面(i).Clear()
                _data各面(i) = Nothing
            End If
            If _SideBandStack(i) IsNot Nothing Then
                _SideBandStack(i).Clear()
                _SideBandStack(i) = Nothing
            End If
            '
            _region各面(i).Empty()
            _path各面画像(i) = Nothing
        Next
    End Sub

    'プレビュー処理
    Public Function CalcModel() As Boolean
        If Not _calc.p_b長方形である Then
            '{0}が長方形でないため描画できません。
            _LastError = String.Format(My.Resources.ModelNoRectangle, dispPlateName(enumBasketPlateIdx._bottom))
            Return False
        End If

        '各面の領域をセットする
        If Not setRegions() Then
            Return False
        End If

        '各面に対応した画像用dataの初期化と四角数
        If Not setSideDataBandCount() Then
            Return False
        End If

        '画像用dataから画像生成
        If Not getImages() Then
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
        MoveList(imageList側面展開図())

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


    '各面の領域
    '
    '     左側面  前面    右側面    背面            
    '  _leftside _front _rightside _back
    '      (1)      (2)     (3)     (4)
    '　　 ────────────────  .... fuchi   
    '　　┌──┬────┬──┬────┐    
    '　　│　　│　　　　│　　│　　　　│    ↑　　　
    '　　│　　│　　　　│　　│　　　　│    │　　　
    '　　│　　│　　　　│　　│　　　　│　takasa　　
    '　　│　　│　　　　│　　│　　　　│    ↓　　　　
    '　　●──┼────┼──┴────┘    
    '　　　　　│　 (0)　│                    ↑  
    '　　　　　│_bottom │                   tate 
    '　　　　　└────┘         　         ↓　
    ' 　　<tate><- yoko -><tate><- yoko ->

    '面名
    Private Function dispPlateName(ByVal i As Integer) As String
        If i < 0 OrElse cBasketPlateCount <= i Then
            Return Nothing
        End If
        '底,左側面,前面,右側面,背面
        Dim ary() As String = My.Resources.ModelPlateNames.Split(",")
        Return ary(i)
    End Function

    '各面の領域をセットする
    Private Function setRegions() As Boolean
        '底の横
        Dim yoko As Double = _calc.p_d底の横長
        '底の縦
        Dim tate As Double = _calc.p_d底の縦長
        '高さ
        Dim takasa As Double = _calc.p_d四角ベース_高さ

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
    '            　　　　　　／＼　
    '                  高さ／　　＼縦　
    ' 　　　／＼　　     ／ -45°←＼
    ' 　横／　　＼高さ ／　　　　  ／　
    '   ／→45°　＼   ＼ 右側面 ／　　
    ' ／　　　　　／     ＼　  ／　　　
    ' ＼ 前面   ／  ／＼　 ＼／　　　　
    ' 　＼　　／横／　　＼縦　　　　
    ' 　　＼／　／→45度　＼ 
    '  　　　／　　　　　／　　　
    '　　　　＼  底    ／ ／＼　　　　　　
    '　　　／＼＼　　／ ／横　＼高さ　　　
    '高さ／　　＼＼／ ／-135↑←＼
    '  ／↑→135 ＼ ／　　　　 　／　
    '／　　　　　／　＼ 背面   ／　　
    '＼左側面  ／　  　＼　　／　　　
    '縦＼　　／　　　　　＼／　　　　
    '　　＼／　　　　    
    '　　　
    '　　　
    '
    '
    Friend Class CBandAttribute
        Dim _i何本幅 As Integer
        Dim _s色 As String

        Sub New(ByVal row As tbl縦横展開Row)
            If row IsNot Nothing Then
                _i何本幅 = row.f_i何本幅
                _s色 = row.f_s色
            End If
        End Sub

        Sub SetAttribute(ByVal row As tbl縦横展開Row)
            If row IsNot Nothing Then
                row.f_i何本幅 = _i何本幅
                row.f_s色 = _s色
            End If
        End Sub

        Overrides Function ToString() As String
            Return String.Format("[{0}]{1}", _i何本幅, _s色)
        End Function
    End Class

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

        '横ひも,順方向,縦の四角数,後半
        base(0) = _calc.getBandAttributeList(emExp._Yoko, False, _calc.p_i縦の四角数, True)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_45 base({0}) {1}", 0, base(0))

        '縦ひも,順方向,横の四角数,後半
        base(1) = _calc.getBandAttributeList(emExp._Tate, False, _calc.p_i横の四角数, True)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_45 base({0}) {1}", 1, base(1))

        '横ひも,逆方向,縦の四角数,前半
        base(2) = _calc.getBandAttributeList(emExp._Yoko, True, _calc.p_i縦の四角数, False)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_45 base({0}) {1}", 2, base(2))

        '縦ひも,逆方向,横の四角数,前半
        base(3) = _calc.getBandAttributeList(emExp._Tate, True, _calc.p_i横の四角数, False)
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

        '縦ひも,逆方向,縦の四角数,前半
        base(0) = _calc.getBandAttributeList(emExp._Tate, True, _calc.p_i縦の四角数, False)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_135 base({0}) {1}", 0, base(0))

        '横ひも,逆方向,横の四角数,前半
        base(1) = _calc.getBandAttributeList(emExp._Yoko, True, _calc.p_i横の四角数, False)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_135 base({0}) {1}", 1, base(1))

        '縦ひも,順方向,縦の四角数,後半
        base(2) = _calc.getBandAttributeList(emExp._Tate, False, _calc.p_i縦の四角数, True)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "set_135 base({0}) {1}", 2, base(2))

        '横ひも,順方向,横の四角数,後半
        base(3) = _calc.getBandAttributeList(emExp._Yoko, False, _calc.p_i横の四角数, True)
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



    Function setUpDown() As Boolean
        Dim updown As New clsUpDown   'CheckBoxTableは使わない
        If Not _calc._Data.ToClsUpDown(updown) OrElse Not updown.IsValid(False) Then 'チェックはMatrix
            updown.Reset(0)
        End If

        '左側面: 底の左下、縦×高さ
        Dim leftside As New clsUpDown(updown)
        leftside.Shift(-_calc.p_i高さの切上四角数, _calc.p_i横の四角数)
        _data各面(enumBasketPlateIdx._leftside).FromClsUpDown(leftside)

        '前面: 底の左上、横×高さ
        Dim front As New clsUpDown(updown)
        front.Shift(-_calc.p_i高さの切上四角数, -_calc.p_i高さの切上四角数)
        _data各面(enumBasketPlateIdx._front).FromClsUpDown(front)

        '右側面: 底の右上、縦×高さ
        Dim rightside As New clsUpDown(updown)
        rightside.Shift(_calc.p_i横の四角数, -_calc.p_i高さの切上四角数)
        _data各面(enumBasketPlateIdx._rightside).FromClsUpDown(rightside)

        '背面: 底の右下、横×高さ
        Dim back As New clsUpDown(updown)
        back.Shift(_calc.p_i縦の四角数, _calc.p_i縦の四角数)
        _data各面(enumBasketPlateIdx._back).FromClsUpDown(back)



        _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_iひも上下の高さ数") = 0
        _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_bひも上下1回区分") = 0


        Return True
    End Function


    '各面に対応した画像用dataの初期化と四角数
    Function setSideDataBandCount() As Boolean
        '画像用データ
        _data各面(0) = New clsDataTables(_calc._Data)
        _data各面(0).p_tbl追加品.Clear()

        For i As Integer = 1 To cBasketPlateCount - 1
            _data各面(i) = New clsDataTables(_data各面(0))
        Next


        '左側面: 底の左下、縦×高さ
        _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_i横の四角数") = _calc.p_i高さの切上四角数
        _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_i縦の四角数") = _calc.p_i縦の四角数
        _data各面(enumBasketPlateIdx._leftside).p_row底_縦横.Value("f_d高さの四角数") = 0
        _SideBandStack(enumBasketPlateIdx._leftside) = New CSideBandStack(_calc.p_i高さの切上四角数, _calc.p_i縦の四角数)

        '前面: 底の左上、横×高さ
        _data各面(enumBasketPlateIdx._front).p_row底_縦横.Value("f_i横の四角数") = _calc.p_i横の四角数
        _data各面(enumBasketPlateIdx._front).p_row底_縦横.Value("f_i縦の四角数") = _calc.p_i高さの切上四角数
        _data各面(enumBasketPlateIdx._front).p_row底_縦横.Value("f_d高さの四角数") = 0
        _SideBandStack(enumBasketPlateIdx._front) = New CSideBandStack(_calc.p_i横の四角数, _calc.p_i高さの切上四角数)

        '右側面: 底の右上、縦×高さ
        _data各面(enumBasketPlateIdx._rightside).p_row底_縦横.Value("f_i横の四角数") = _calc.p_i高さの切上四角数
        _data各面(enumBasketPlateIdx._rightside).p_row底_縦横.Value("f_i縦の四角数") = _calc.p_i縦の四角数
        _data各面(enumBasketPlateIdx._rightside).p_row底_縦横.Value("f_d高さの四角数") = 0
        _SideBandStack(enumBasketPlateIdx._rightside) = New CSideBandStack(_calc.p_i高さの切上四角数, _calc.p_i縦の四角数)

        '背面: 底の右下、横×高さ
        _data各面(enumBasketPlateIdx._back).p_row底_縦横.Value("f_i横の四角数") = _calc.p_i横の四角数
        _data各面(enumBasketPlateIdx._back).p_row底_縦横.Value("f_i縦の四角数") = _calc.p_i高さの切上四角数
        _data各面(enumBasketPlateIdx._back).p_row底_縦横.Value("f_d高さの四角数") = 0
        _SideBandStack(enumBasketPlateIdx._back) = New CSideBandStack(_calc.p_i横の四角数, _calc.p_i高さの切上四角数)


        '斜め各方向に下からバンドを積む
        set_45()
        set_135()
        '斜めに積まれたデータを各面の縦ひも・横ひもにセット
        setDataFromStack()
        'ひも上下
        setUpDown()

        Return True
    End Function

    '画像用dataから画像生成
    Function getImages() As Boolean

        Dim ret As Boolean = True
        For i As Integer = 0 To cBasketPlateCount - 1
            _data各面(i).ResetStartPoint()
            _path各面画像(i) = IO.Path.Combine(IO.Path.GetTempPath, IO.Path.ChangeExtension(_PlateNames(i), CImageDraw.cImageClipFileExtention))

            Dim calc As New clsCalcSquare45(_data各面(i), _calc._frmMain)
            calc.p_sBottomPngFilePath(True) = _path各面画像(i) 'あれば削除
            calc.p_dBottomPngRotateAngle = _PlateAngle(i)

            If calc.CalcSize(CalcCategory.NewData, Nothing, Nothing) Then
                If Not calc.p_b長方形である Then
                    '{0}が長方形でないため描画できません。
                    _LastError = String.Format(My.Resources.ModelNoRectangle, dispPlateName(i))
                    ret = False
                    Exit For
                End If

                Dim imgdata As New clsImageData(_PlateNames(i)) '仮の名前で
                If Not calc.CalcImage(imgdata, False) Then
                    _LastError = calc.p_sメッセージ
                    ret = False
                ElseIf String.IsNullOrEmpty(calc.p_sBottomPngFilePath(True)) Then
                    '{0}が描画できませんでした。
                    _LastError = String.Format(My.Resources.ModelNoImage, dispPlateName(i))
                    ret = False
                End If
                If {1, 3}.Contains(i) Then
                    '右側面と左側面
                    _delta画像サイズ(i) = New S差分(calc.p_d底の縦長, calc.p_d底の横長)
                Else
                    '底と前面と背面
                    _delta画像サイズ(i) = New S差分(calc.p_d底の横長, calc.p_d底の縦長)
                End If

                imgdata.Clear()
                If Not ret Then
                    Exit For
                End If
            End If
            calc.Clear()
        Next

        Return ret
    End Function


    '絵の貼付と面枠描画
    Function imageList側面展開図() As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim line As S線分

        Dim fuchi As Double = _calc.p_d縁の高さ

        '側面
        For i As Integer = 1 To cBasketPlateCount - 1
            '面枠
            item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, i)
            item.m_a四隅 = New S四隅(_region各面(i))

            line = New S線分(_region各面(i).p左上, _region各面(i).p右上)
            line += Unit90 * fuchi
            item.m_lineList.Add(line)
            itemlist.AddItem(item)

            '絵
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像貼付, i)
            Dim s As New S領域(_region各面(i).p左下, _region各面(i).p左下 + _delta画像サイズ(i))
            item.m_a四隅 = New S四隅(s)
            item.m_fpath = _path各面画像(i)
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

        Return itemlist
    End Function

End Class

