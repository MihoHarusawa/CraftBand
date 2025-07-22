Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageData
Imports CraftBand.clsImageItem
Imports CraftBand.clsImageItem.CBand
Imports CraftBand.clsUpDown
Imports CraftBand.Tables.dstDataTables

Partial Public Class clsCalcSquare

    '
    'ひも長で描画(出力ひも長は、これに係数をかけて、ひも長加算する)
    '　　　　　　　　┌──────┐
    '　　　　　　　　├──────┤           側面枠の高さ = get側面高() +  _d縁の高さ
    '　　　　　　　　│　　　　　　│           
    '　　　　　　　　├‥‥‥‥‥‥┤           ─ get周の横() × get周の縦()  
    '　　　　┌┬─┬┼──────┼┬─┬┐           ↑  (この差は)
    '　　　　││　：│┏━━━━┓│：　││           ↓f_d立ち上げ時の四角底周の増分
    '　　　　││　：│┃　　　　┃│：　││   ━　p_d四角ベース_横 × p_d四角ベース_縦
    '　　　　││　：│┃　　　　┃│：　││   　　　　　(縦ひも・横ひもはこの中)
    '　　　　││　：│┗━━━━┛│：　││   　
    '　　　　└┴─┴┼──────┼┴─┴┘     
    '　点線がZero位置├‥‥‥‥‥‥┤f_d底の厚さ 　　　　　┐
    '　　　　　　　　│　　　　　　│p_d四角ベース_高さ　　┘get側面高()    p_d縁厚さプラス_高さ
    '　　　　　　　　├──────┤　　　　　　　　　　　　　　　　　　
    '　　　　　　　　└──────┘_d縁の高さ/_d縁の垂直ひも長
    '
    '描画するひもの長さ = get周の横()/get周の縦()   
    '　　　　　　　　　       + get側面高()
    '　　　　　　　　　　　　 + [_d縁の高さ と _d縁の垂直ひも長 の小さい方]
    '
    '


    'プレビュー時に生成,描画後はNothing
    Dim _ImageList横ひも As clsImageItemList   '横ひもの展開レコードを含む
    Dim _ImageList縦ひも As clsImageItemList   '縦ひもの展開レコードを含む
    Dim _imageList側面上 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面左 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面下 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面右 As clsImageItemList    '側面の展開レコードを含む
    'Dim _ImageList差しひも As clsImageItemList
    Dim _ImageList描画要素 As clsImageItemList '底と側面


    '現画像生成時に記号を表示する #60
    Shared IsDrawMarkCurrent As Boolean = True

#Region "テクスチャファイル作成用"
    '絵のファイル
    Dim _PlatePngFilePath(clsImageData.cBasketPlateCount - 1) As String
    Property p_sPlatePngFilePath(ByVal pidx As clsImageData.enumBasketPlateIdx, ByVal check As Boolean) As String
        Get
            If check AndAlso Not String.IsNullOrWhiteSpace(_PlatePngFilePath(pidx)) Then
                If IO.File.Exists(_PlatePngFilePath(pidx)) Then
                    Return _PlatePngFilePath(pidx)
                Else
                    Return Nothing
                End If
            Else
                Return _PlatePngFilePath(pidx)
            End If
        End Get
        Set(value As String)
            _PlatePngFilePath(pidx) = value
            If check AndAlso Not String.IsNullOrWhiteSpace(value) AndAlso IO.File.Exists(value) Then
                IO.File.Delete(value)
            End If
        End Set
    End Property

    '絵の回転角度
    Shared _PlateAngle() As Integer = {0, 90, 0, -90, 180}

#End Region

    'プレビュー画像生成
    'isBackFace=trueの時、UpDownを裏面として適用
    Public Function CalcImage(ByVal imgData As clsImageData, ByVal isBackFace As Boolean) As Boolean
        '記号順が変わるので裏面には表示しない
        IsDrawMarkCurrent = Not isBackFace AndAlso
            Not String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号)

        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '念のため
        _ImageList横ひも = Nothing
        _ImageList縦ひも = Nothing
        _imageList側面上 = Nothing
        _imageList側面左 = Nothing
        _imageList側面下 = Nothing
        _imageList側面右 = Nothing
        _ImageList描画要素 = Nothing
        '_ImageList差しひも = Nothing

        _BandListForClip.Clear()

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        '_tbl縦横展開_横ひも,_tbl縦横展開_縦ひもにレコードが残されているはず
        '_ImageList横ひも, _ImageList縦ひもを作る
        If Not imageList横ひも() OrElse Not imageList縦ひも() Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '基本のひも幅(文字サイズ)と基本色
        imgData.setBasics(_d基本のひも幅, _Data.p_row目標寸法.Value("f_s基本色"))


        If Not imageList四側面() Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If
        _ImageList描画要素 = imageList底と側面枠()


        '描画用のデータ追加
        regionUpDown底(isBackFace)
        regionUpDown側面1(isBackFace)
        regionUpDown側面2(isBackFace)
        regionUpDown側面3(isBackFace)
        regionUpDown側面4(isBackFace)

        Dim imgList差しひも As clsImageItemList = Nothing
        If Not isBackFace Then
            '差しひも
            imgList差しひも = imageList差しひも()
        End If

        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing

        imgData.MoveList(_imageList側面上)
        imgData.MoveList(_imageList側面左)
        imgData.MoveList(_imageList側面下)
        imgData.MoveList(_imageList側面右)
        _imageList側面上 = Nothing
        _imageList側面左 = Nothing
        _imageList側面下 = Nothing
        _imageList側面右 = Nothing

        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing

        If imgList差しひも IsNot Nothing Then
            imgData.MoveList(imgList差しひも)
        End If
        imgList差しひも = Nothing

        If Not isBackFace Then
            '付属品
            AddPartsImage(imgData, _frmMain.editAddParts)
        End If

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function


#Region "描画のベースとなる値"

    '底の配置領域に立ち上げ増分をプラス, = p_d四角ベース_周
    Private Function get側面の周長() As Double
        Return 2 * (_d四角ベース_横計 + _d四角ベース_縦計) _
            + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分")
    End Function

    '横ひも長計算・描画に使用
    Friend Function get周の横(Optional ByVal multi As Double = 1) As Double
        Return multi *
            (_d四角ベース_横計 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4)
    End Function

    '縦ひも長計算・描画に使用
    Friend Function get周の縦(Optional ByVal multi As Double = 1) As Double
        Return multi *
            (_d四角ベース_縦計 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4)
    End Function

    '縦ひも長計算・描画に使用   '縁は含まない
    Friend Function get側面高(ByVal count As Integer) As Double
        Return count * (_d四角ベース_高さ計 + getZeroSide())
    End Function

    '側面の高さゼロ位置(1=1端)
    Private Function getZeroSide(Optional ByVal multi As Double = 1) As Double
        Return multi * g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
    End Function

    '描画の横高ゼロ描画位置
    Private Function getZeroX(Optional ByVal multi As Double = 1) As Double
        Return multi * (get周の横() + getZeroSide(2))
    End Function

    '描画の縦高ゼロ描画位置
    Private Function getZeroY(Optional ByVal multi As Double = 1) As Double
        Return multi * (get周の縦() + getZeroSide(2))
    End Function

    '側面描画長、底の周から縁を加えた高さ
    Friend Function get縁厚さプラス_高さ() As Double
        Return get側面高(1) + _d縁の高さ
    End Function

#End Region

#Region "横ひも・縦ひも・4側面"

    '横ひもリストの描画情報 : _tbl縦横展開_横ひも → _ImageList横ひも
    Private Function imageList横ひも() As Boolean
        If _tbl縦横展開_横ひも Is Nothing Then
            Return False
        End If

        _ImageList横ひも = New clsImageItemList(_tbl縦横展開_横ひも, Not IsDrawMarkCurrent)
        If _tbl縦横展開_横ひも.Rows.Count = 0 Then
            Return True
        End If

        Dim Y横ひも上 As Double = p_d四角ベース_縦 / 2

        '上から下へ(位置順)
        _ImageList横ひも.SortByPosition()
        For Each item As clsImageItem In _ImageList横ひも
            If item.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            If item.m_row縦横展開.f_iひも種 = enumひも種.i_横 Then
                '始点T　　　　　　　終点T
                '　　　[□□→□□]
                '始点F　　　　　　　終点F
                Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅)
                Dim bandwidth As S差分 = Unit270 * dひも幅
                Dim d横ひも表示長 As Double = item.m_row縦横展開.f_d出力ひも長

                '#92 band.m_rひも位置.p左上 = New S実座標(-d横ひも表示長 / 2, Y横ひも上)
                item.m_rひも位置.p左上 = New S実座標(-item.m_row縦横展開.f_dVal1, Y横ひも上)
                item.m_rひも位置.p右下 = item.m_rひも位置.p左上 + bandwidth + Unit0 * (d横ひも表示長)

                Dim band As New CBand(item.m_row縦横展開)
                band.p始点F = item.m_rひも位置.p左下
                band.p終点F = item.m_rひも位置.p右下
                band.p始点T = item.m_rひも位置.p左上
                band.p終点T = item.m_rひも位置.p右上
                If IsDrawMarkCurrent Then
                    '横バンドの左
                    band.SetMarkPosition(enumMarkPosition._始点Fの前, dひも幅 * 1.4)
                End If

                item.AddBand(band)
                AddClipItem(band)
            Else
                '補強ひもは描画しない
                item.m_ImageType = ImageTypeEnum._描画なし
            End If
            '
            Y横ひも上 -= item.m_row縦横展開.f_d幅
        Next

        Return True
    End Function

    '縦ひもリストの描画情報 : _tbl縦横展開_縦ひも → _ImageList縦ひも
    Private Function imageList縦ひも() As Boolean
        If _tbl縦横展開_縦ひも Is Nothing Then
            Return False
        End If

        _ImageList縦ひも = New clsImageItemList(_tbl縦横展開_縦ひも, Not IsDrawMarkCurrent)
        If _tbl縦横展開_縦ひも.Rows.Count = 0 Then
            Return True
        End If

        Dim X縦ひも左 As Double = -p_d四角ベース_横 / 2

        '左から右へ(位置順)
        _ImageList縦ひも.SortByPosition()
        For Each item As clsImageItem In _ImageList縦ひも
            If item.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            If item.m_row縦横展開.f_iひも種 = enumひも種.i_縦 Then
                '始点F　□　始点T　
                '　　 　↓
                '終点F　□　終点T
                Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅)
                Dim bandwidth As S差分 = Unit0 * dひも幅
                Dim d縦ひも表示長 As Double = item.m_row縦横展開.f_d出力ひも長

                '#92 band.m_rひも位置.p左上 = New S実座標(X縦ひも左, d縦ひも表示長 / 2)
                item.m_rひも位置.p左上 = New S実座標(X縦ひも左, item.m_row縦横展開.f_dVal1)
                item.m_rひも位置.p右下 = item.m_rひも位置.p左上 + bandwidth + Unit270 * (d縦ひも表示長)

                Dim band As New CBand(item.m_row縦横展開)
                band.p始点F = item.m_rひも位置.p左上
                band.p終点F = item.m_rひも位置.p左下
                band.p始点T = item.m_rひも位置.p右上
                band.p終点T = item.m_rひも位置.p右下
                If IsDrawMarkCurrent Then
                    '縦バンドの上
                    band.SetMarkPosition(enumMarkPosition._始点Fの前)
                End If

                item.AddBand(band)
                AddClipItem(band)
            Else
                '補強ひもは描画しない
                item.m_ImageType = ImageTypeEnum._描画なし
            End If

            X縦ひも左 += item.m_row縦横展開.f_d幅
        Next

        Return True
    End Function

    '_imageList側面上・_imageList側面左・_imageList側面下・_imageList側面右生成
    '※側面のレコードはリスト出力時にadjust_側面() 済み
    Function imageList四側面() As Boolean

        '側面のレコードを縦横レコード化
        Dim tmptable As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        Dim d最下の高さ As Double = 0

        Dim idx As Integer = 1
        For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, "f_i番号 ASC , f_iひも番号 ASC")
            If r.f_i番号 = cHemNumber Then
                '縁は編みかたとして処理
                Continue For
            End If
            If r.f_i番号 = cIdxSpace Then
                '最下段のスペースはレコードにしない
                d最下の高さ = r.f_d高さ
                Continue For
            End If
            For i As Integer = 1 To r.f_iひも本数
                row = tmptable.Newtbl縦横展開Row
                row.f_iひも種 = enumひも種.i_側面
                row.f_iひも番号 = idx
                row.f_i位置番号 = i '参考値
                row.f_i何本幅 = r.f_i何本幅
                row.f_s記号 = r.f_s記号
                row.f_s色 = r.f_s色
                row.f_dひも長 = r.f_dひも長
                row.f_dひも長加算 = r.f_dひも長加算
                row.f_dひも長加算2 = 0
                row.f_d出力ひも長 = r.f_d連続ひも長
                row.f_dVal1 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) 'ひも幅

                If _b縦横側面を展開する Then
                    row.f_d幅 = r.f_d高さ '個別
                Else
                    row.f_d幅 = row.f_dVal1 + _d目_ひも間のすき間_高さ 'ひも幅+目、合計なので再計算
                End If

                tmptable.Rows.Add(row)

                idx += 1
            Next
        Next
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "tmptable:{0}", New clsGroupDataRow(tmptable).ToString)


        '以降参照するのでここでセットする
        _imageList側面上 = New clsImageItemList(tmptable, Not IsDrawMarkCurrent)
        _imageList側面左 = New clsImageItemList(tmptable, Not IsDrawMarkCurrent)
        _imageList側面下 = New clsImageItemList(tmptable, Not IsDrawMarkCurrent)
        _imageList側面右 = New clsImageItemList(tmptable, Not IsDrawMarkCurrent)

        Dim item As clsImageItem

        Dim p上ひも始点F As New S実座標(-get周の横(1 / 2), d最下の高さ + getZeroY(1 / 2))
        Dim p下ひも始点F As New S実座標(get周の横(1 / 2), -d最下の高さ - getZeroY(1 / 2))
        Dim p左ひも始点F As New S実座標(-d最下の高さ - getZeroX(1 / 2), -get周の縦(1 / 2))
        Dim p右ひも始点F As New S実座標(d最下の高さ + getZeroX(1 / 2), get周の縦(1 / 2))

        '1～_i高さの目の数
        For i As Integer = 1 To p_i側面ひもの本数
            '*上(→)
            item = _imageList側面上.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面上:{0}", i)
            Else
                '始点T　　　　　　　終点T
                '　　[□□→(0)□□]　　　↑deltaAx(90)
                '始点F(*) 　　　　　終点F

                Dim band As New CBand(item.m_row縦横展開)
                band.SetBandF(New S線分(p上ひも始点F, p上ひも始点F + Unit0 * get周の横()),
                    item.m_row縦横展開.f_dVal1, Unit90)
                band.is始点FT線 = False
                band.is終点FT線 = False
                If IsDrawMarkCurrent Then
                    '横バンドの左
                    band.SetMarkPosition(enumMarkPosition._始点Fの前, item.m_row縦横展開.f_dVal1) 'ひも幅
                End If

                item.AddBand(band)
                item.m_Index2 = 1
                AddClipItem(band)

                p上ひも始点F = p上ひも始点F + Unit90 * item.m_row縦横展開.f_d幅
            End If

            '*下(←)
            item = _imageList側面下.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面下:{0}", i)
            Else
                '終点F　　　　　始点F(*)　　
                '　[□□←(180)□□]    ↓deltaAx(270)
                '終点T　　　　　始点T　　

                Dim band As New CBand(item.m_row縦横展開)
                band.SetBandF(New S線分(p下ひも始点F, p下ひも始点F + Unit180 * get周の横()),
                    item.m_row縦横展開.f_dVal1, Unit270)
                band.is始点FT線 = False
                band.is終点FT線 = False

                item.AddBand(band)
                item.m_Index2 = 2
                AddClipItem(band)

                p下ひも始点F = p下ひも始点F + Unit270 * item.m_row縦横展開.f_d幅
            End If

            '*左(↑)
            item = _imageList側面左.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面左:{0}", i)
            Else
                '終点T　□　終点F　←deltaAx(180)
                '　　 　↑(90)
                '始点T　□　始点F(*)

                Dim band As New CBand(item.m_row縦横展開)
                band.SetBandF(New S線分(p左ひも始点F, p左ひも始点F + Unit90 * get周の縦()),
                    item.m_row縦横展開.f_dVal1, Unit180)
                band.is始点FT線 = False
                band.is終点FT線 = False

                item.AddBand(band)
                item.m_Index2 = 3
                AddClipItem(band)

                p左ひも始点F = p左ひも始点F + Unit180 * item.m_row縦横展開.f_d幅
            End If

            '*右(↓)
            item = _imageList側面右.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面右:{0}", i)
            Else
                '始点F(*)□　始点T　→deltaAx(0)
                '　　  　↓(270)
                '終点F 　□　終点T

                Dim band As New CBand(item.m_row縦横展開)
                band.SetBandF(New S線分(p右ひも始点F, p右ひも始点F + Unit270 * get周の縦()),
                    item.m_row縦横展開.f_dVal1, Unit0)
                band.is始点FT線 = False
                band.is終点FT線 = False

                item.AddBand(band)
                item.m_Index2 = 4
                AddClipItem(band)

                p右ひも始点F = p右ひも始点F + Unit0 * item.m_row縦横展開.f_d幅
            End If
        Next

        '縁のレコードをイメージ情報化
        Dim cond As String = String.Format("f_i番号 = {0}", cHemNumber)
        Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

        Dim d高さ As Double = groupRow.GetNameValueSum("f_d高さ")
        Dim nひも本数 As Integer = groupRow.GetNameValueSum("f_iひも本数")
        If 0 < nひも本数 Then

            '*上
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 1)
            item.m_a四隅.p左下 = p上ひも始点F
            item.m_a四隅.p右下 = p上ひも始点F + Unit0 * get周の横()
            item.m_a四隅.p左上 = p上ひも始点F + Unit90 * d高さ
            item.m_a四隅.p右上 = p上ひも始点F + Unit90 * d高さ + Unit0 * get周の横()

            '文字位置
            '#60
            If IsDrawMarkCurrent Then
                item.p_p文字位置 = p上ひも始点F + Unit0 * get周の横() + Unit90 * d高さ
            End If
            _imageList側面上.AddItem(item)

            '*下
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 2)
            item.m_a四隅.p右上 = p下ひも始点F
            item.m_a四隅.p左上 = p下ひも始点F + Unit180 * get周の横()
            item.m_a四隅.p右下 = p下ひも始点F + Unit270 * d高さ
            item.m_a四隅.p左下 = p下ひも始点F + Unit270 * d高さ + Unit180 * get周の横()
            '文字なし
            _imageList側面下.AddItem(item)

            '*左
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 3)
            item.m_a四隅.p右下 = p左ひも始点F
            item.m_a四隅.p左下 = p左ひも始点F + Unit180 * d高さ
            item.m_a四隅.p右上 = p左ひも始点F + Unit90 * get周の縦()
            item.m_a四隅.p左上 = p左ひも始点F + Unit180 * d高さ + Unit90 * get周の縦()
            '文字なし
            _imageList側面左.AddItem(item)

            '*右
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 4)
            item.m_a四隅.p左上 = p右ひも始点F
            item.m_a四隅.p右上 = p右ひも始点F + Unit0 * d高さ
            item.m_a四隅.p左下 = p右ひも始点F + Unit270 * get周の縦()
            item.m_a四隅.p右下 = p右ひも始点F + Unit0 * d高さ + Unit270 * get周の縦()
            '文字なし
            _imageList側面右.AddItem(item)

        End If

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面上:{0}", _imageList側面上.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面左:{0}", _imageList側面左.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面下:{0}", _imageList側面下.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面右:{0}", _imageList側面右.ToString)
        Return True
    End Function

    '底と側面枠
    Function imageList底と側面枠() As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        Dim a底 As S四隅
        a底.p左上 = New S実座標(-p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2)
        a底.p右上 = New S実座標(p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        Dim a底の周 As S四隅
        a底の周.p左上 = New S実座標(-get周の横(1 / 2), get周の縦(1 / 2))
        a底の周.p右上 = New S実座標(get周の横(1 / 2), get周の縦(1 / 2))
        a底の周.p左下 = -a底の周.p右上
        a底の周.p右下 = -a底の周.p左上


        '底
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底

        Dim line As S線分
        '右上→左上
        line = New S線分(a底の周.p右上, a底の周.p左上)
        item.m_lineList.Add(line)
        '左上→左下
        line = New S線分(a底の周.p左上, a底の周.p左下)
        item.m_lineList.Add(line)
        '左下→右下
        line = New S線分(a底の周.p左下, a底の周.p右下)
        item.m_lineList.Add(line)
        '右下→右上
        line = New S線分(a底の周.p右下, a底の周.p右上)
        item.m_lineList.Add(line)

        itemlist.AddItem(item)
        '底の画像
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._bottom, False)) Then
            Dim item2 As New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 1)
            item2.m_a四隅 = a底の周
            item2.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._bottom, False)
            item2.m_angle = _PlateAngle(enumBasketPlateIdx._bottom)
            itemlist.AddItem(item2)
        End If


        Dim _d縁厚さプラス_高さ As Double = get縁厚さプラス_高さ()
        Dim d底の厚さ As Double = getZeroSide()

        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.p左下 = a底の周.p左上
        item.m_a四隅.p右下 = a底の周.p右上
        item.m_a四隅.p左上 = item.m_a四隅.p左下 + Unit90 * _d縁厚さプラス_高さ
        item.m_a四隅.p右上 = item.m_a四隅.p右下 + Unit90 * _d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下) + Unit90 * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)
        '上の側面(前面の画像)
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._front, False)) Then
            Dim item2 As New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 2)
            item2.m_a四隅 = item.m_a四隅
            item2.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._front, False)
            item2.m_angle = _PlateAngle(enumBasketPlateIdx._front)
            itemlist.AddItem(item2)
        End If


        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅.p左上 = a底の周.p右上
        item.m_a四隅.p左下 = a底の周.p右下
        item.m_a四隅.p右上 = item.m_a四隅.p左上 + Unit0 * _d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p左下 + Unit0 * _d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下) + Unit0 * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)
        '右の側面(右側面の画像)
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._rightside, False)) Then
            Dim item2 As New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 3)
            item2.m_a四隅 = item.m_a四隅
            item2.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._rightside, False)
            item2.m_angle = _PlateAngle(enumBasketPlateIdx._rightside)
            itemlist.AddItem(item2)
        End If


        '下の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅.p左上 = a底の周.p左下
        item.m_a四隅.p右上 = a底の周.p右下
        item.m_a四隅.p左下 = item.m_a四隅.p左上 + Unit270 * _d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p右上 + Unit270 * _d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上) + Unit270 * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)
        '下の側面(背面の画像)
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._back, False)) Then
            Dim item2 As New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 4)
            item2.m_a四隅 = item.m_a四隅
            item2.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._back, False)
            item2.m_angle = _PlateAngle(enumBasketPlateIdx._back)
            itemlist.AddItem(item2)
        End If

        '左の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p右上 = a底の周.p左上
        item.m_a四隅.p右下 = a底の周.p左下
        item.m_a四隅.p左上 = item.m_a四隅.p右上 + Unit180 * _d縁厚さプラス_高さ
        item.m_a四隅.p左下 = item.m_a四隅.p右下 + Unit180 * _d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下) + Unit180 * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)
        '左の側面(左側面の画像)
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._leftside, False)) Then
            Dim item2 As New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 5)
            item2.m_a四隅 = item.m_a四隅
            item2.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._leftside, False)
            item2.m_angle = _PlateAngle(enumBasketPlateIdx._leftside)
            itemlist.AddItem(item2)
        End If

        Return itemlist
    End Function

#End Region

#Region "ひも上下"
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

    '底の上下をAddClip
    Private Function regionUpDown底(ByVal isBackFace As Boolean) As Boolean
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            Return False
        End If

        _CUpDown.TargetFace = enumTargetFace.Bottom '底
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If
        Dim revRange As Integer = -1
        If isBackFace Then
            revRange = p_i縦ひもの本数
        End If

        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            'If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                If _CUpDown.GetIsDown(iTate, iYoko, revRange) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If isDrawingItem(itemYoko) Then
                        'itemTate.m_regionList.Add領域(itemYoko.m_rひも位置)
                        itemTate.AddClip(itemYoko)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            'If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                If _CUpDown.GetIsUp(iTate, iYoko, revRange) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If isDrawingItem(itemTate) Then
                        'itemYoko.m_regionList.Add領域(itemTate.m_rひも位置)
                        itemYoko.AddClip(itemTate)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        Return True
    End Function


    '側面(上)の上下をAddClip
    Private Function regionUpDown側面1(ByVal isBackFace As Boolean) As Boolean
        If _ImageList縦ひも Is Nothing OrElse _imageList側面上 Is Nothing Then
            Return False
        End If

        _CUpDown.TargetFace = enumTargetFace.Side12 '側面(上右)
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If
        Dim revRange As Integer = -1
        If isBackFace Then
            revRange = p_i縦ひもの本数
        End If


        Dim horzDif As Integer = 0 '横に上→右
        '*上の側面
        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            'If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            For iTakasa As Integer = 1 To p_i側面ひもの本数
                If _CUpDown.GetIsDown(iTate + horzDif, iTakasa, revRange) Then
                    Dim itemUSide As clsImageItem = _imageList側面上.GetRowItem(enumひも種.i_側面, iTakasa)
                    If isDrawingItem(itemUSide) Then
                        'itemTate.m_regionList.Add領域(itemUSide.m_rひも位置)
                        itemTate.AddClip(itemUSide)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To p_i側面ひもの本数
            Dim itemUSide As clsImageItem = _imageList側面上.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemUSide Is Nothing Then
                Continue For
            End If
            'If itemUSide.m_regionList Is Nothing Then itemUSide.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                If _CUpDown.GetIsUp(iTate + horzDif, iTakasa, revRange) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If isDrawingItem(itemTate) Then
                        'itemUSide.m_regionList.Add領域(itemTate.m_rひも位置)
                        itemUSide.AddClip(itemTate)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemUSide({0}):{1}", iTakasa, itemUSide.m_regionList.ToString)
        Next


        Return True
    End Function

    '側面(右)の上下をAddClip
    Private Function regionUpDown側面2(ByVal isBackFace As Boolean) As Boolean
        If _ImageList横ひも Is Nothing OrElse _imageList側面右 Is Nothing Then
            Return False
        End If

        Dim revRange As Integer
        Dim revRange2 As Integer
        If isBackFace Then
            'うら
            _CUpDown.TargetFace = enumTargetFace.Side34 '側面(下左) の左を適用
            revRange = p_i縦ひもの本数
            revRange2 = p_i横ひもの本数
        Else
            'おもて
            _CUpDown.TargetFace = enumTargetFace.Side12 '側面(上右) の右を適用
            revRange = -1
            revRange2 = -1
        End If
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If


        Dim horzDif As Integer = p_i縦ひもの本数 '横に上→右

        '*右の側面
        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            'If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            For iTakasa As Integer = 1 To p_i側面ひもの本数
                If _CUpDown.GetIsDown(iYoko + horzDif, iTakasa, revRange, revRange2) Then
                    Dim itemRSide As clsImageItem = _imageList側面右.GetRowItem(enumひも種.i_側面, iTakasa)
                    If isDrawingItem(itemRSide) Then
                        'itemYoko.m_regionList.Add領域(itemRSide.m_rひも位置)
                        itemYoko.AddClip(itemRSide)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To p_i側面ひもの本数
            Dim itemRSide As clsImageItem = _imageList側面右.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemRSide Is Nothing Then
                Continue For
            End If
            'If itemRSide.m_regionList Is Nothing Then itemRSide.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                If _CUpDown.GetIsUp(iYoko + horzDif, iTakasa, revRange, revRange2) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If isDrawingItem(itemYoko) Then
                        'itemRSide.m_regionList.Add領域(itemYoko.m_rひも位置)
                        itemRSide.AddClip(itemYoko)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemRSide({0}):{1}", iTakasa, itemRSide.m_regionList.ToString)
        Next

        Return True
    End Function

    '側面(下)の上下をAddClip
    Private Function regionUpDown側面3(ByVal isBackFace As Boolean) As Boolean
        If _ImageList縦ひも Is Nothing OrElse _imageList側面下 Is Nothing Then
            Return False
        End If

        _CUpDown.TargetFace = enumTargetFace.Side34 '側面
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If
        Dim revRange As Integer = -1
        If isBackFace Then
            revRange = p_i縦ひもの本数
        End If

        Dim horzDif As Integer = 0 '横に下→左

        '*下の側面(UpDownは左→右)
        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            'If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト
            Dim horzIdx As Integer = p_i縦ひもの本数 - iTate + 1 + horzDif

            For iTakasa As Integer = 1 To p_i側面ひもの本数
                If _CUpDown.GetIsDown(horzIdx, iTakasa, revRange) Then
                    Dim itemDSide As clsImageItem = _imageList側面下.GetRowItem(enumひも種.i_側面, iTakasa)
                    If isDrawingItem(itemDSide) Then
                        'itemTate.m_regionList.Add領域(itemDSide.m_rひも位置)
                        itemTate.AddClip(itemDSide)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To p_i側面ひもの本数
            Dim itemDSide As clsImageItem = _imageList側面下.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemDSide Is Nothing Then
                Continue For
            End If
            'If itemDSide.m_regionList Is Nothing Then itemDSide.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                Dim horzIdx As Integer = p_i縦ひもの本数 - iTate + 1 + horzDif
                If _CUpDown.GetIsUp(horzIdx, iTakasa, revRange) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If isDrawingItem(itemTate) Then
                        'itemDSide.m_regionList.Add領域(itemTate.m_rひも位置)
                        itemDSide.AddClip(itemTate)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemUSide({0}):{1}", iTakasa, itemDSide.m_regionList.ToString)
        Next

        Return True
    End Function

    '側面(左)の上下をAddClip
    Private Function regionUpDown側面4(ByVal isBackFace As Boolean) As Boolean
        If _ImageList横ひも Is Nothing OrElse _imageList側面左 Is Nothing Then
            Return False
        End If

        Dim revRange As Integer
        Dim revRange2 As Integer
        If isBackFace Then
            'うら
            _CUpDown.TargetFace = enumTargetFace.Side12 '側面(上右) の右を適用
            revRange = p_i縦ひもの本数
            revRange2 = p_i横ひもの本数
        Else
            'おもて
            _CUpDown.TargetFace = enumTargetFace.Side34 '側面(下左) の左を適用
            revRange = -1
            revRange2 = -1
        End If
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If

        Dim horzDif As Integer = p_i縦ひもの本数 '横に下→左

        '*左の側面(UpDownは下→上)
        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            'If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            Dim horzIdx As Integer = p_i横ひもの本数 - iYoko + 1 + horzDif
            For iTakasa As Integer = 1 To p_i側面ひもの本数
                If _CUpDown.GetIsDown(horzIdx, iTakasa, revRange, revRange2) Then
                    Dim itemLSide As clsImageItem = _imageList側面左.GetRowItem(enumひも種.i_側面, iTakasa)
                    If isDrawingItem(itemLSide) Then
                        'itemYoko.m_regionList.Add領域(itemLSide.m_rひも位置)
                        itemYoko.AddClip(itemLSide)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To p_i側面ひもの本数
            Dim itemLSide As clsImageItem = _imageList側面左.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemLSide Is Nothing Then
                Continue For
            End If
            'If itemLSide.m_regionList Is Nothing Then itemLSide.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                Dim horzIdx As Integer = p_i横ひもの本数 - iYoko + 1 + horzDif
                If _CUpDown.GetIsUp(horzIdx, iTakasa, revRange, revRange2) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If isDrawingItem(itemYoko) Then
                        'itemLSide.m_regionList.Add領域(itemYoko.m_rひも位置)
                        itemLSide.AddClip(itemYoko)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemRSide({0}):{1}", iTakasa, itemLSide.m_regionList.ToString)
        Next

        Return True
    End Function

#End Region

End Class
