Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstDataTables

Public Module mdlAddPartsImage



    '追加品のレコードをイメージ情報化
    Function AddPartsImage(ByVal imgData As clsImageData, ByVal editAddParts As ctrAddParts) As Integer
        If imgData Is Nothing OrElse editAddParts Is Nothing Then
            Return -1
        End If
        Dim rows() As tbl追加品Row = editAddParts.GetAddPartsRecords()
        If rows.Count = 0 Then
            Return 0
        End If

        '横に対する縦の比率
        _dAspectRatio = editAddParts.getAspectRatio()

        '現在の描画位置
        _p左下 = imgData.CurrentItemDrawingRect.p左下 '最下
        _p中心 = imgData.CenterCoordinates


        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim count As Integer = 0

        '追加品のレコードをイメージ情報化

        'i番号,iひも番号順
        Dim imain As Integer = 0
        Do While imain < rows.Count
            Dim i点数 As Integer = rows(imain).f_i点数 '一致項目・最初のレコード値

            '同じi番号の範囲
            Dim iFrom As Integer = imain
            Dim iTo As Integer = imain
            For j As Integer = imain + 1 To rows.Count - 1
                If rows(j).f_i番号 = rows(imain).f_i番号 Then
                    iTo = j
                Else
                    Exit For
                End If
            Next

            '点数分を繰り返す
            For iset As Integer = 1 To i点数
                For isub As Integer = iFrom To iTo
                    Dim row As tbl追加品Row = rows(isub)

                    If row.f_d長さ <= 0 Then
                        Continue For
                    End If

                    Dim isCenter As Boolean = False
                    If row.f_i描画位置 = enum描画位置.i_中心 Then
                        isCenter = True
                    ElseIf row.f_i描画位置 = enum描画位置.i_左下 Then
                        '
                    Else
                        Continue For
                    End If


                    Select Case row.f_i描画形状
                        Case enum描画形状.i_横バンド
                            If isCenter Then
                                item = sub_横バンド_中心(row)
                            Else
                                item = sub_横バンド_左下(row)
                            End If

                        Case enum描画形状.i_横線
                            If isCenter Then
                                item = sub_横線_中心(row)
                            Else
                                item = sub_横線_左下(row)
                            End If

                        Case enum描画形状.i_正方形_辺
                            If isCenter Then
                                item = sub_正方形_辺_中心(row)
                            Else
                                item = sub_正方形_辺_左下(row)
                            End If

                        Case enum描画形状.i_長方形_横
                            If isCenter Then
                                item = sub_長方形_横_中心(row)
                            Else
                                item = sub_長方形_横_左下(row)
                            End If

                        Case enum描画形状.i_正方形_周
                            If isCenter Then
                                item = sub_正方形_周_中心(row)
                            Else
                                item = sub_正方形_周_左下(row)
                            End If

                        Case enum描画形状.i_長方形_周
                            If isCenter Then
                                item = sub_長方形_周_中心(row)
                            Else
                                item = sub_長方形_周_左下(row)
                            End If

                        Case enum描画形状.i_円_径
                            If isCenter Then
                                item = sub_円_径_中心(row)
                            Else
                                item = sub_円_径_左下(row)
                            End If

                        Case enum描画形状.i_楕円_横径
                            If isCenter Then
                                item = sub_楕円_横径_中心(row)
                            Else
                                item = sub_楕円_横径_左下(row)
                            End If

                        Case enum描画形状.i_円_周
                            If isCenter Then
                                item = sub_円_周_中心(row)
                            Else
                                item = sub_円_周_左下(row)
                            End If

                        Case enum描画形状.i_楕円_周
                            If isCenter Then
                                item = sub_楕円_周_中心(row)
                            Else
                                item = sub_楕円_周_左下(row)
                            End If

                        Case enum描画形状.i_上半円_径
                            If isCenter Then
                                item = sub_上半円_径_中心(row)
                            Else
                                item = sub_上半円_径_左下(row)
                            End If

                        Case enum描画形状.i_上半円_周
                            If isCenter Then
                                item = sub_上半円_周_中心(row)
                            Else
                                item = sub_上半円_周_左下(row)
                            End If

                        Case Else
                            Continue For
                    End Select
                    If item Is Nothing Then
                        Continue For
                    End If


                    '文字位置
                    If iset = 1 AndAlso Not String.IsNullOrWhiteSpace(row.f_s記号) Then
                        item.p_p文字位置 = item.m_rひも位置.p右上
                    End If

                    itemlist.AddItem(item)
                    count += 1
                Next
            Next

            imain = iTo + 1
        Loop

        If 0 < count Then
            imgData.MoveList(itemlist)
        End If
        itemlist = Nothing

        Return count
    End Function


    Dim _p左下 As S実座標
    Dim _p中心 As S実座標
    Dim _dAspectRatio As Double

    Private Function sub_横バンド_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - row.f_d長さ / 2, _p中心.Y - dひも幅 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + row.f_d長さ / 2, _p中心.Y + dひも幅 / 2)
        item.m_dひも幅 = dひも幅

        Return item
    End Function
    Private Function sub_横バンド_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - dひも幅)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + row.f_d長さ, _p左下.Y)
        item.m_dひも幅 = dひも幅

        _p左下.Y -= dひも幅 * 2

        Return item
    End Function


    Private Function sub_横線_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d幅 As Double = row.f_d描画厚
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - row.f_d長さ / 2, _p中心.Y - d幅 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + row.f_d長さ / 2, _p中心.Y + d幅 / 2)
        item.m_dひも幅 = d幅

        Return item
    End Function
    Private Function sub_横線_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d幅 As Double = row.f_d描画厚
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d幅)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + row.f_d長さ, _p左下.Y)
        item.m_dひも幅 = d幅

        _p左下.Y -= d幅 * 2

        Return item
    End Function


    Private Function sub_正方形_辺_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d辺 As Double = row.f_d長さ
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d辺 / 2, _p中心.Y - d辺 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d辺 / 2, _p中心.Y + d辺 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_正方形_辺_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d辺 As Double = row.f_d長さ
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d辺)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d辺, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d辺 + row.f_d描画厚)

        Return item
    End Function


    Private Function sub_長方形_横_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d横 As Double = row.f_d長さ
        Dim d縦 As Double = row.f_d長さ * _dAspectRatio
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d横 / 2, _p中心.Y - d縦 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d横 / 2, _p中心.Y + d縦 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_長方形_横_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d横 As Double = row.f_d長さ
        Dim d縦 As Double = row.f_d長さ * _dAspectRatio
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d縦)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d横, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d縦 + row.f_d描画厚)

        Return item
    End Function


    Private Function sub_正方形_周_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d辺 As Double = row.f_d長さ / 4
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d辺 / 2, _p中心.Y - d辺 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d辺 / 2, _p中心.Y + d辺 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_正方形_周_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d辺 As Double = row.f_d長さ / 4
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d辺)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d辺, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d辺 + row.f_d描画厚)

        Return item
    End Function


    Private Function sub_長方形_周_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d横 As Double = row.f_d長さ / (2 + 2 * _dAspectRatio)
        Dim d縦 As Double = d横 * _dAspectRatio
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d横 / 2, _p中心.Y - d縦 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d横 / 2, _p中心.Y + d縦 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_長方形_周_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d横 As Double = row.f_d長さ / (2 + 2 * _dAspectRatio)
        Dim d縦 As Double = d横 * _dAspectRatio
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d縦)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d横, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d縦 + row.f_d描画厚)

        Return item
    End Function


    Private Function sub_円_径_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d径 As Double = row.f_d長さ
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d径 / 2, _p中心.Y - d径 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d径 / 2, _p中心.Y + d径 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_円_径_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d径 As Double = row.f_d長さ
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d径)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d径, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d径 + row.f_d描画厚)

        Return item
    End Function

    Private Function sub_楕円_横径_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d横径 As Double = row.f_d長さ
        Dim d縦径 As Double = d横径 * _dAspectRatio
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d横径 / 2, _p中心.Y - d縦径 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d横径 / 2, _p中心.Y + d縦径 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_楕円_横径_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d横径 As Double = row.f_d長さ
        Dim d縦径 As Double = d横径 * _dAspectRatio
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d縦径)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d横径, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d縦径 + row.f_d描画厚)

        Return item
    End Function


    Private Function sub_円_周_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d径 As Double = row.f_d長さ / Math.PI
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d径 / 2, _p中心.Y - d径 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d径 / 2, _p中心.Y + d径 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_円_周_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d径 As Double = row.f_d長さ / Math.PI
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d径)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d径, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d径 + row.f_d描画厚)

        Return item
    End Function


    Private Function sub_楕円_周_中心(ByVal row As tbl追加品Row) As clsImageItem
        If _dAspectRatio = 0 Then
            Return Nothing
        End If
        Dim d As Double = 1 / _dAspectRatio
        Dim ph As Double = Math.PI * (3 * (d + 1) - Math.Sqrt((3 * d + 1) * (d + 3)))
        Dim d縦径 As Double = 2 * row.f_d長さ / ph
        Dim d横径 As Double = d縦径 * d

        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d横径 / 2, _p中心.Y - d縦径 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d横径 / 2, _p中心.Y + d縦径 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_楕円_周_左下(ByVal row As tbl追加品Row) As clsImageItem
        If _dAspectRatio = 0 Then
            Return Nothing
        End If
        Dim d As Double = 1 / _dAspectRatio
        Dim ph As Double = Math.PI * (3 * (d + 1) - Math.Sqrt((3 * d + 1) * (d + 3)))
        Dim d縦径 As Double = 2 * row.f_d長さ / ph
        Dim d横径 As Double = d縦径 * d

        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d縦径)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d横径, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d縦径 + row.f_d描画厚)

        Return item
    End Function


    Private Function sub_上半円_径_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d径 As Double = row.f_d長さ
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d径 / 2, _p中心.Y - d径 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d径 / 2, _p中心.Y + d径 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_上半円_径_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d径 As Double = row.f_d長さ
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d径)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d径, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d径 + row.f_d描画厚)

        Return item
    End Function


    Private Function sub_上半円_周_中心(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d径 As Double = row.f_d長さ * 2 / Math.PI
        item.m_rひも位置.p左下 = New S実座標(_p中心.X - d径 / 2, _p中心.Y - d径 / 2)
        item.m_rひも位置.p右上 = New S実座標(_p中心.X + d径 / 2, _p中心.Y + d径 / 2)
        item.m_dひも幅 = row.f_d描画厚

        Return item
    End Function
    Private Function sub_上半円_周_左下(ByVal row As tbl追加品Row) As clsImageItem
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        Dim d径 As Double = row.f_d長さ * 2 / Math.PI
        item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d径)
        item.m_rひも位置.p右上 = New S実座標(_p左下.X + d径, _p左下.Y)
        item.m_dひも幅 = row.f_d描画厚

        _p左下.Y -= (d径 / 2 + row.f_d描画厚)

        Return item
    End Function




End Module
