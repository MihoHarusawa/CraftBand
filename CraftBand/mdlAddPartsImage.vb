Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstDataTables

Public Module mdlAddPartsImage

    '追加品のレコードをイメージ情報化
    Function AddPartsImage(ByVal imgData As clsImageData, ByVal tbl追加品 As tbl追加品DataTable, ByVal aspectratio As Double) As Integer
        If imgData Is Nothing OrElse tbl追加品 Is Nothing Then
            Return -1
        End If
        If tbl追加品.Rows.Count = 0 Then
            Return 0
        End If

        '現在の描画位置の最下
        Dim rect As S領域 = imgData.CurrentItemDrawingRect
        Dim dY As Double = rect.y最下
        Dim dX As Double = rect.x最左



        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim count As Integer = 0

        '追加品のレコードをイメージ情報化

        '番号ごと
        Dim res = (From row As tbl追加品Row In tbl追加品
                   Select Num = row.f_i番号
                   Order By Num).Distinct
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(tbl追加品.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

            Dim i点数 As Integer = groupRow.GetNameValue("f_i点数") '一致項目
            Dim d長さ As Double = groupRow.GetIndexNameValue(1, "f_d長さ")
            Dim i本幅 As Integer = groupRow.GetIndexNameValue(1, "f_i何本幅")
            Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)


            Dim isDraw As Boolean = False
            For Each drow As clsDataRow In groupRow
                If drow.Value("f_i描画位置") = enum描画位置.i_下部 Then
                    isDraw = True
                End If
            Next
            If Not isDraw Then
                Continue For
            End If

            Do While 0 < i点数
                item = New clsImageItem(ImageTypeEnum._付属品, groupRow, i点数)

                item.m_a四隅.p左上 = New S実座標(dX, dY)
                item.m_a四隅.p右上 = New S実座標(dX + d長さ, dY)
                item.m_a四隅.p左下 = New S実座標(dX, dY - d幅)
                item.m_a四隅.p右下 = New S実座標(dX + d長さ, dY - d幅)

                '文字位置
                item.p_p文字位置 = item.m_a四隅.p右上
                itemlist.AddItem(item)
                count += 1

                dY -= d幅 * 2
                i点数 -= 1
            Loop
        Next

        If 0 < count Then
            imgData.MoveList(itemlist)
        End If
        itemlist = Nothing

        Return count
    End Function

End Module
