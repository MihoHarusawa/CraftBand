Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.mdlUnit
Imports CraftBand.Tables.dstDataTables

Partial Public Class clsCalcMesh

    'リスト出力時に生成
    Dim _ImageList横ひも As clsImageItemList   '横ひもの展開レコードを含む
    Dim _ImageList縦ひも As clsImageItemList   '縦ひもの展開レコードを含む

    'プレビュー時に生成
    Dim _imageList側面編みかた As clsImageItemList    '側面のレコードを含む
    Dim _ImageList描画要素 As clsImageItemList '底と側面

    Dim _dPortionOver As Double = New Length(1, "cm").Value '省略部分の長さ


    '横ひもリストの描画情報
    Private Function imageList横ひも() As Boolean
        If _ImageList横ひも Is Nothing Then
            Return False
        End If

        '縦横領域の左上(短いひもの左位置)
        Dim p縦横の左上 As New S実座標(-p_d縦横の横 / 2, p_d縦横の縦 / 2)
        '底から部分描画(長いひもの左位置)
        Dim d左半分の長さ As Double = p_d内側_横 / 2 + _dPortionOver
        Dim p長いひも左上 As New S実座標(-d左半分の長さ, p_d縦横の縦 / 2)
        Dim d横ひも間のすき間 As Double = _Data.p_row底_縦横.Value("f_d横ひも間のすき間")

        '上から下へ(位置順)
        _ImageList横ひも.SortByPosition()
        For Each band As clsImageItem In _ImageList横ひも
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            Dim bandwidth As S差分 = Unit270 * g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)

            Select Case band.m_row縦横展開.f_iひも種
                Case enumひも種.i_横 Or enumひも種.i_長い
                    band.m_rひも位置.p左上 = p長いひも左上
                    band.m_rひも位置.p右下 = p長いひも左上 + bandwidth + Unit0 * (band.m_row縦横展開.f_d出力ひも長 / 2 + d左半分の長さ)
                    band.m_borderひも = band.m_borderひも And Not DirectionEnum._左

                Case (enumひも種.i_横 Or enumひも種.i_短い), (enumひも種.i_横 Or enumひも種.i_最上と最下)
                    band.m_rひも位置.p左上 = New S実座標(-band.m_row縦横展開.f_d出力ひも長 / 2, p縦横の左上.Y)
                    band.m_rひも位置.p右下 = band.m_rひも位置.p左上 + bandwidth + Unit0 * band.m_row縦横展開.f_d出力ひも長

                Case Else
                    '補強ひもは描画しない
                    band.m_bNoMark = True
                    Continue For
            End Select
            '
            p縦横の左上 = p縦横の左上 + bandwidth + Unit270 * d横ひも間のすき間
            p長いひも左上 = p長いひも左上 + bandwidth + Unit270 * d横ひも間のすき間
        Next

        Return True
    End Function

    '縦ひもリストの描画情報
    Private Function imageList縦ひも() As Boolean
        If _ImageList縦ひも Is Nothing Then
            Return False
        End If

        '縦横領域の左下(縦ひもの左下位置)
        Dim d下半分の長さ As Double = p_d内側_縦 / 2 + _dPortionOver
        Dim p左下部分 As New S実座標(-p_d縦横の横 / 2, -d下半分の長さ)
        Dim dひとつのすき間の寸法 As Double = _Data.p_row底_縦横.Value("f_dひとつのすき間の寸法")

        '左から右へ(位置順)
        _ImageList縦ひも.SortByPosition()
        For Each band As clsImageItem In _ImageList縦ひも
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If
            If band.m_row縦横展開.f_iひも種 <> enumひも種.i_縦 Then
                Continue For '始末ひも除外
            End If

            Dim bandwidth As S差分 = Unit0 * g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)
            band.m_rひも位置.p左下 = p左下部分
            band.m_rひも位置.p右上 = p左下部分 + bandwidth + Unit90 * (band.m_row縦横展開.f_d出力ひも長 / 2 + d下半分の長さ)
            band.m_borderひも = band.m_borderひも And Not DirectionEnum._下
            '
            p左下部分 = p左下部分 + bandwidth + Unit0 * dひとつのすき間の寸法
        Next

        Return True
    End Function

    '_imageList側面ひも生成、側面のレコードを含む
    Function imageList側面編みかた(ByVal dひも幅 As Double) As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        '側面のレコードをイメージ情報化
        Dim dY As Double = p_d外側_縦 / 2
        Dim dX As Double = p_d外側_横 / 2
        Dim res = (From row As tbl側面Row In _Data.p_tbl側面
                   Select Num = row.f_i番号
                   Order By Num).Distinct

        '番号ごと
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

            Dim d高さ As Double = groupRow.GetNameValueSum("f_d高さ")
            Dim nひも本数 As Integer = groupRow.GetNameValueSum("f_iひも本数")
            Dim d周長比率対底の周 As Double = groupRow.GetNameValueMax("f_d周長比率対底の周")
            Dim i周数 As Integer = groupRow.GetNameValue("f_i周数") '一致項目

            If 0 < nひも本数 Then

                'ImageTypeEnum._編みかた・横
                item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 1)
                item.m_a四隅.p左下 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, dY)
                item.m_a四隅.p右下 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, dY)
                item.m_a四隅.p左上 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, dY + d高さ)
                item.m_a四隅.p右上 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, dY + d高さ)
                '周の区切り
                If 1 < i周数 Then
                    For i As Integer = 1 To i周数 - 1
                        Dim p1 As New S実座標(item.m_a四隅.x最左, d高さ * (i / i周数) + dY)
                        Dim p2 As New S実座標(item.m_a四隅.x最右, d高さ * (i / i周数) + dY)
                        Dim line As New S線分(p1, p2)
                        item.m_lineList.Add(line)
                    Next
                End If
                '文字位置
                item.p_p文字位置 = New S実座標(dひも幅 + p_d外側_横 * d周長比率対底の周 / 2, dY + d高さ / 2)
                itemlist.AddItem(item)

                'ImageTypeEnum._編みかた・縦
                item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 2)
                item.m_a四隅.p左上 = New S実座標(dX, +p_d外側_縦 * d周長比率対底の周 / 2)
                item.m_a四隅.p左下 = New S実座標(dX, -p_d外側_縦 * d周長比率対底の周 / 2)
                item.m_a四隅.p右上 = New S実座標(dX + d高さ, +p_d外側_縦 * d周長比率対底の周 / 2)
                item.m_a四隅.p右下 = New S実座標(dX + d高さ, -p_d外側_縦 * d周長比率対底の周 / 2)
                '周の区切り
                If 1 < i周数 Then
                    For i As Integer = 1 To i周数 - 1
                        Dim p1 As New S実座標(dX + d高さ * (i / i周数), item.m_a四隅.y最上)
                        Dim p2 As New S実座標(dX + d高さ * (i / i周数), item.m_a四隅.y最下)
                        Dim line As New S線分(p1, p2)
                        item.m_lineList.Add(line)
                    Next
                End If
                '文字は指定しない

                itemlist.AddItem(item)
            End If
            dY += d高さ
            dX += d高さ
        Next

        Return itemlist
    End Function

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

    '底の上下をm_regionListにセット
    Private Function regionUpDown底(ByVal _ImageList横ひも As clsImageItemList, ByVal _ImageList縦ひも As clsImageItemList) As Boolean
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            Return False
        End If

        Dim _CUpDown As New clsUpDown
        _CUpDown.Reset(0)
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If
        Dim iTate As Integer
        Dim iYoko As Integer


        iTate = 0
        For Each itemTate As clsImageItem In _ImageList縦ひも
            iTate += 1
            If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            iYoko = 0
            For Each itemYoko As clsImageItem In _ImageList横ひも
                iYoko += 1
                If isDrawingItem(itemYoko) AndAlso _CUpDown.GetIsDown(iTate, iYoko) Then
                    itemTate.m_regionList.Add領域(itemYoko.m_rひも位置)
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        iYoko = 0
        For Each itemYoko As clsImageItem In _ImageList横ひも
            iYoko += 1
            If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            iTate = 0
            For Each itemTate As clsImageItem In _ImageList縦ひも
                iTate += 1
                If isDrawingItem(itemTate) AndAlso _CUpDown.GetIsUp(iTate, iYoko) Then
                    itemYoko.m_regionList.Add領域(itemTate.m_rひも位置)
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        Return True
    End Function

    '底と側面枠
    Function imageList底と側面枠(ByVal dひも幅 As Double) As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        Dim a底の縦横 As S四隅
        a底の縦横.p左上 = New S実座標(-p_d縦横の横 / 2, p_d縦横の縦 / 2)
        a底の縦横.p右上 = New S実座標(p_d縦横の横 / 2, p_d縦横の縦 / 2)
        a底の縦横.p左下 = -a底の縦横.p右上
        a底の縦横.p右下 = -a底の縦横.p左上

        Dim a底 As S四隅
        a底.p左上 = New S実座標(-p_d内側_横 / 2, p_d内側_縦 / 2)
        a底.p右上 = New S実座標(p_d内側_横 / 2, p_d内側_縦 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        '底
        If _d径の合計 = 0 Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
            item.m_a四隅 = a底
            itemlist.AddItem(item)
        Else
            '楕円底は縦横部分
            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, 1)
            item.m_a四隅 = a底の縦横
            itemlist.AddItem(item)
        End If

        '差しひも
        Dim n角の差しひも数 As Integer = _i垂直ひも数_楕円 \ 4
        Dim aryAngle(n角の差しひも数) As Double
        Dim aryIndex As Integer
        If 0 < n角の差しひも数 Then
            Dim angle As Double = 360 / (4 + _i垂直ひも数_楕円)
            aryIndex = 1
            For i As Integer = 1 To n角の差しひも数 Step 2
                aryAngle(i) = angle * aryIndex
                aryIndex += 1
            Next
            aryIndex = n角の差しひも数
            For i As Integer = 2 To n角の差しひも数 Step 2
                aryAngle(i) = angle * aryIndex
                aryIndex -= 1
            Next
        End If

        Dim a楕円の中心 As S四隅
        a楕円の中心.p左上 = a底の縦横.p左上 + Unit270 * _d最上と最下の短いひもの幅
        a楕円の中心.p右上 = a底の縦横.p右上 + Unit270 * _d最上と最下の短いひもの幅
        a楕円の中心.p左下 = a底の縦横.p左下 + Unit90 * _d最上と最下の短いひもの幅
        a楕円の中心.p右下 = a底の縦横.p右下 + Unit90 * _d最上と最下の短いひもの幅

        '番号ごと
        Dim res = (From row As tbl底_楕円Row In _Data.p_tbl底_楕円
                   Select Num = row.f_i番号
                   Order By Num).Distinct
        aryIndex = 1
        Dim r差しひも As Double = 0
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl底_楕円.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

            Dim b差しひも区分 As Boolean = groupRow.GetNameValue("f_b差しひも区分")

            If b差しひも区分 Then
                '差しひも
                Dim iひも数 As Integer = groupRow.GetNameValue("f_i差しひも本数") \ 4
                Dim i本幅 As Integer = groupRow.GetNameValue("f_i何本幅")
                Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
                Dim dひも長 As Double = groupRow.GetNameValue("f_d連続ひも長")

                Dim a右上ひも As S四隅
                a右上ひも.p左上 = a楕円の中心.p右上 + Unit90 * (d幅 / 2) + Unit0 * r差しひも
                a右上ひも.p左下 = a楕円の中心.p右上 + Unit270 * (d幅 / 2) + Unit0 * r差しひも
                a右上ひも.p右上 = a右上ひも.p左上 + Unit0 * dひも長
                a右上ひも.p右下 = a右上ひも.p左下 + Unit0 * dひも長

                For i As Integer = 1 To iひも数
                    '右上
                    item = New clsImageItem(ImageTypeEnum._差しひも, groupRow, i)
                    item.m_a四隅 = a右上ひも.Rotate(a楕円の中心.p右上, aryAngle(aryIndex))
                    item.p_p文字位置 = item.m_a四隅.p右上 + Unit0.Rotate(aryAngle(aryIndex)) * (dひも幅 / 2)
                    itemlist.AddItem(item)

                    aryIndex += 1
                Next
                r差しひも = groupRow.GetNameValue("f_d径の累計")

            Else
                '底楕円
                item = New clsImageItem(ImageTypeEnum._底楕円, groupRow, 1)

                Dim d径の累計 As Double = groupRow.GetNameValue("f_d径の累計") '1レコード想定

                item.m_a四隅 = a楕円の中心
                Dim line As S線分
                '右上→左上
                line = New S線分(New S実座標(a底の縦横.p右上.X, a底の縦横.p右上.Y + d径の累計), New S実座標(a底の縦横.p左上.X, a底の縦横.p左上.Y + d径の累計))
                item.m_lineList.Add(line)
                '左上→左下
                line = New S線分(New S実座標(a底の縦横.p左上.X - d径の累計, a底の縦横.p左上.Y - _d最上と最下の短いひもの幅), New S実座標(a底の縦横.p左下.X - d径の累計, a底の縦横.p左下.Y + _d最上と最下の短いひもの幅))
                item.m_lineList.Add(line)
                '左下→右下
                line = New S線分(New S実座標(a底の縦横.p左下.X, a底の縦横.p左下.Y - d径の累計), New S実座標(a底の縦横.p右下.X, a底の縦横.p右下.Y - d径の累計))
                item.p_p文字位置 = line.p終了 '文字位置
                item.m_lineList.Add(line)
                '右下→右上
                line = New S線分(New S実座標(a底の縦横.p右下.X + d径の累計, a底の縦横.p右下.Y + _d最上と最下の短いひもの幅), New S実座標(a底の縦横.p右上.X + d径の累計, a底の縦横.p右上.Y - _d最上と最下の短いひもの幅))
                item.m_lineList.Add(line)

                itemlist.AddItem(item)
            End If

        Next


        '縁のf_d周長比率対底の周
        Dim d周長比率対底の周 As Double = 1
        Dim rows() As DataRow = _Data.p_tbl側面.Select(Nothing, "f_iひも番号 DESC")
        For Each row In rows
            If Not IsDBNull(row("f_d周長比率対底の周")) AndAlso 0 < row("f_d周長比率対底の周") Then
                d周長比率対底の周 = row("f_d周長比率対底の周")
                Exit For
            End If
        Next

        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.p左下 = New S実座標(-p_d外側_横 / 2, p_d外側_縦 / 2)
        item.m_a四隅.p右下 = New S実座標(+p_d外側_横 / 2, p_d外側_縦 / 2)
        item.m_a四隅.p左上 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, _d高さの合計 + p_d外側_縦 / 2)
        item.m_a四隅.p右上 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, _d高さの合計 + p_d外側_縦 / 2)
        itemlist.AddItem(item)

        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p左上 = New S実座標(p_d外側_横 / 2, p_d外側_縦 / 2)
        item.m_a四隅.p左下 = New S実座標(p_d外側_横 / 2, -p_d外側_縦 / 2)
        item.m_a四隅.p右上 = New S実座標(_d高さの合計 + p_d外側_横 / 2, p_d外側_縦 * d周長比率対底の周 / 2)
        item.m_a四隅.p右下 = New S実座標(_d高さの合計 + p_d外側_横 / 2, -p_d外側_縦 * d周長比率対底の周 / 2)
        itemlist.AddItem(item)

        Return itemlist
    End Function

    ''付属品
    'Function imageList付属品() As clsImageItemList
    'Dim item As clsImageItem
    'Dim itemlist As New clsImageItemList

    ''追加品のレコードをイメージ情報化
    'Dim dY As Double = -(3 * _dPortionOver + p_d外側_縦 / 2)
    'Dim dX As Double = -p_d外側_横 / 2

    ''番号ごと
    'Dim res = (From row As tbl追加品Row In _Data.p_tbl追加品
    '           Select Num = row.f_i番号
    '           Order By Num).Distinct
    'For Each num As Integer In res
    '    Dim cond As String = String.Format("f_i番号 = {0}", num)
    '    Dim groupRow = New clsGroupDataRow(_Data.p_tbl追加品.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

    '    Dim i点数 As Integer = groupRow.GetNameValue("f_i点数") '一致項目
    '    Dim d長さ As Double = groupRow.GetIndexNameValue(1, "f_d長さ")
    '    Dim i本幅 As Integer = groupRow.GetIndexNameValue(1, "f_i何本幅")
    '    Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)

    '    Do While 0 < i点数
    '        item = New clsImageItem(ImageTypeEnum._付属品, groupRow, i点数)

    '        dY -= d幅 * 2
    '        i点数 -= 1
    '    Loop
    'Next

    'Return itemlist
    'End Function

    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '念のため
        _imageList側面編みかた = Nothing
        _ImageList描画要素 = Nothing

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        'リスト処理で残された情報
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '文字サイズ
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)
        '基本のひも幅と基本色
        imgData.setBasics(dひも幅, _Data.p_row目標寸法.Value("f_s基本色"))

        '描画用のデータ追加
        imageList横ひも()
        imageList縦ひも()
        If _Data.p_row底_縦横.Value("f_b展開区分") Then
            '描画用のデータ追加
            regionUpDown底(_ImageList横ひも, _ImageList縦ひも)
        End If

        _imageList側面編みかた = imageList側面編みかた(dひも幅)
        _ImageList描画要素 = imageList底と側面枠(dひも幅)


        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing
        imgData.MoveList(_imageList側面編みかた)
        _imageList側面編みかた = Nothing
        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing

        '付属品
        AddPartsImage(imgData, _frmMain.editAddParts)

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function

End Class
