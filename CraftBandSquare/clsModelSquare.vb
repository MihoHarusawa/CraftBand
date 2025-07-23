Imports CraftBand
Imports CraftBand.clsImageItem
Imports CraftBandSquare.clsCalcSquare

Public Class clsModelSquare
    Inherits clsImageData

    Shared _PlateNames() As String = {"bottom", "leftside", "front", "rightside", "back"}
    Shared _PlateAngle() As Integer = {45, 135, 45, -45, -135}

    '現対象
    Dim _calc As clsCalcSquare


    '底と4側面
    Dim _region各面(cBasketPlateCount - 1) As S領域
    Dim _path各面画像(cBasketPlateCount - 1) As String
    Dim _delta画像サイズ(cBasketPlateCount - 1) As S差分

    Dim _data画像生成用 As clsDataTables

    '対象指定
    Sub New(ByVal calc As clsCalcSquare, ByVal fpath As String)
        MyBase.New(fpath)
        _calc = calc
    End Sub

    Overloads Sub Clear()
        MyBase.Clear()

        If _data画像生成用 IsNot Nothing Then
            _data画像生成用.Clear()
            _data画像生成用 = Nothing
        End If

        For i As Integer = 0 To cBasketPlateCount - 1
            _region各面(i).Empty()
            _path各面画像(i) = Nothing
        Next
    End Sub

    'プレビュー処理
    Public Function CalcModel() As Boolean

        '各面の領域をセットする
        If Not setRegions() Then
            Return False
        End If

        '画像生成用データを作る
        If Not setDataForTexture() Then
            Return False
        End If

        '画像生成用データから画像生成
        If Not getImages() Then
            Return False
        End If


        '出力ひもリスト情報
        Dim outp As New clsOutput(MyBase.FilePath)
        If Not _calc.CalcOutput(outp) Then
            Return False '既にチェック済のはず
        End If

        '基本のひも幅と基本色
        setBasics(_calc.p_d基本のひも幅, _calc._Data.p_row目標寸法.Value("f_s基本色"))

        '描画
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
            outpath = IO.Path.Combine(IO.Path.GetTempPath, "Square_model")
        End If

        ' OBJとMTLファイルの出力
        Return CreateOBJWithTextures(_calc.get周の横, height, _calc.get周の縦,
        _path各面画像, outpath)
    End Function


    '各面の領域
    '
    '     左側面  前面    右側面    背面            
    '  _leftside _front _rightside _back
    '      (1)      (2)     (3)     (4)
    '　　┌──┬────┬──┬────┐    
    '　　├──┼────┼──┼────┤    ↑　(縁を含む絵)　　
    '　　│　　│　　　　│　　│　　　　│    │　　　
    '　　│　　│　　　　│　　│　　　　│　takasa　　
    '　　│　　│　　　　│　　│　　　　│    ↓　　　　
    '　　●──┼────┼──┴────┘    
    '　　　　　│　 (0)　│                    ↑  
    '　　　　　│_bottom │                   tate 
    '　　　　　└────┘         　         ↓　
    ' 　　<tate><- yoko -><tate><- yoko ->

    '各面の領域をセットする
    Private Function setRegions() As Boolean
        '底の横
        Dim yoko As Double = _calc.get周の横
        '底の縦
        Dim tate As Double = _calc.get周の縦
        '高さ
        Dim takasa As Double = _calc.get縁厚さプラス_高さ()

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


    '画像生成用データを作る
    Function setDataForTexture() As Boolean
        '生成用データ
        _data画像生成用 = New clsDataTables(_calc._Data)

        '差しひも(#95)
        '_data画像生成用.p_tbl差しひも.Clear()

        '追加品なし
        _data画像生成用.p_tbl追加品.Clear()

        Return True
    End Function

    '画像生成用データから画像生成
    Function getImages() As Boolean

        Dim ret As Boolean = True
        Dim calcTmp As New clsCalcSquare(_data画像生成用, _calc._frmMain)

        For i As Integer = 0 To cBasketPlateCount - 1
            _path各面画像(i) = IO.Path.Combine(IO.Path.GetTempPath, IO.Path.ChangeExtension(_PlateNames(i), CImageDraw.cImageClipFileExtention))
            calcTmp.p_sPlatePngFilePath(i, True) = _path各面画像(i) 'あれば削除
        Next

        If calcTmp.CalcSize(CalcCategory.NewData, Nothing, Nothing) Then

            Dim imgdata As New clsImageData(_PlateNames(0)) '仮の名前で
            If Not calcTmp.CalcImage(imgdata, False, False, enum差しひも表示._回り込み) Then
                _LastError = calcTmp.p_sメッセージ
                ret = False
            Else

                For i As Integer = 0 To cBasketPlateCount - 1
                    If String.IsNullOrEmpty(calcTmp.p_sPlatePngFilePath(i, True)) Then
                        '{0}が描画できませんでした。
                        _LastError = String.Format(My.Resources.ModelNoImage, BasketPlateString(i))
                        ret = False
                    End If
                    If i = 0 Then
                        '底
                        _delta画像サイズ(i) = New S差分(calcTmp.get周の横, calcTmp.get周の縦)
                    ElseIf {1, 3}.Contains(i) Then
                        '右側面と左側面
                        _delta画像サイズ(i) = New S差分(calcTmp.get周の縦, calcTmp.get縁厚さプラス_高さ())
                    Else
                        '前面と背面
                        _delta画像サイズ(i) = New S差分(calcTmp.get周の横, calcTmp.get縁厚さプラス_高さ())
                    End If

                    If Not ret Then
                        Exit For
                    End If
                Next
            End If

            imgdata.Clear()
            calcTmp.Clear()
        End If
        Return ret
    End Function


    '絵の貼付と面枠
    Function imageList側面展開図() As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        '側面
        For i As Integer = 1 To cBasketPlateCount - 1
            '面枠
            item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, i)
            item.m_a四隅 = New S四隅(_region各面(i))

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

