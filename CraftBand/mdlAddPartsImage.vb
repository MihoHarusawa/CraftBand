Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstDataTables

Public Module mdlAddPartsImage

    '追加品のレコードをイメージ情報化
    Function AddPartsImage(ByVal imgData As clsImageData, ByVal editAddParts As ctrAddParts) As Integer
        If imgData Is Nothing OrElse editAddParts Is Nothing Then
            Return -1
        End If
        'i番号,iひも番号順
        Dim rows() As tbl追加品Row = editAddParts.GetAddPartsRecords()
        If rows.Count = 0 Then
            Return 0
        End If

        '横に対する縦の比率
        _dAspectRatio = editAddParts.getAspectRatio()

        '現在の描画位置
        _p左下 = imgData.CurrentItemDrawingRect.p左下 '最下
        _p中心 = imgData.CenterCoordinates


        'イメージ情報
        Dim itemlist As New clsImageItemList
        Dim count As Integer = 0

        '追加品のレコード(i番号,iひも番号順)
        Dim imain As Integer = 0
        Do While imain < rows.Count
            Dim i点数 As Integer = rows(imain).f_i点数 '一致項目・最初のレコード値

            '同じi番号の範囲
            Dim first描画位置 As enum描画位置 = enum描画位置.i_なし
            Dim iFrom As Integer = imain
            Dim iTo As Integer
            For j As Integer = imain To rows.Count - 1
                If first描画位置 = enum描画位置.i_なし Then
                    first描画位置 = rows(j).f_i描画位置
                End If
                If rows(j).f_i番号 = rows(imain).f_i番号 Then
                    iTo = j
                Else
                    Exit For
                End If
            Next

            '描画指定されている場合(f_b描画区分はi番号ごと)
            If 0 < i点数 AndAlso (first描画位置 <> enum描画位置.i_なし) AndAlso rows(iFrom).f_b描画区分 Then
                Dim isCenter As Boolean = (first描画位置 = enum描画位置.i_中心)
                If isCenter Then
                    i点数 = 1 '中心は1回だけ
                End If

                '点数分を繰り返す(左下の時)
                For iset As Integer = 1 To i点数
                    '各、iひも番号
                    For isub As Integer = iFrom To iTo
                        Dim row As tbl追加品Row = rows(isub)
                        '同位置のみ対象
                        If row.f_i描画位置 <> first描画位置 Then
                            Continue For
                        End If

                        '文字位置
                        Dim isMark As Boolean = (iset = 1 AndAlso Not String.IsNullOrWhiteSpace(row.f_s記号))

                        Select Case row.f_i描画形状
                            Case enum描画形状.i_横バンド
                                count += add_横バンド(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_横四角
                                count += add_横四角(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_正方形_辺
                                count += add_正方形_辺(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_長方形_横
                                count += add_長方形_横(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_正方形_周
                                count += add_正方形_周(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_長方形_周
                                count += add_長方形_周(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_円_径
                                count += add_円_径(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_楕円_横径
                                count += add_楕円_横径(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_円_周
                                count += add_円_周(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_楕円_周
                                count += add_楕円_周(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_上半円_径
                                count += add_上半円_径(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_上半円_周
                                count += add_上半円_周(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_横線
                                count += add_横線(itemlist, isCenter, isMark, row)

                            Case enum描画形状.i_縦線
                                count += add_縦線(itemlist, isCenter, isMark, row)

                            Case Else
                                Continue For
                        End Select
                    Next
                Next
            End If

            imain = iTo + 1
        Loop

        If 0 < count Then
            imgData.MoveList(itemlist)
        End If
        itemlist = Nothing

        Return count
    End Function


    Dim _dAspectRatio As Double
    Dim _p中心 As S実座標
    Dim _p左下 As S実座標

    Private Function add_横バンド(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下:マスター本数,長さ 
        '中心:1本,長さ
        Dim i本数 As Integer = IIf(isCenter, 1, row.f_iひも本数 / row.f_i点数) 'マスターのひも数
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - row.f_d長さ / 2, _p中心.Y - dひも幅 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + row.f_d長さ / 2, _p中心.Y + dひも幅 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - dひも幅)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + row.f_d長さ, _p左下.Y)

                _p左下.Y -= dひも幅 * 2
            End If
            item.m_dひも幅 = dひも幅

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_横四角(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)
        Dim d幅 As Double = row.f_d描画厚
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d長さ / 2, _p中心.Y - d幅 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d長さ / 2, _p中心.Y + d幅 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d幅)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d長さ, _p左下.Y)

                _p左下.Y -= d幅 * 2
            End If
            item.m_dひも幅 = d幅

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_正方形_辺(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d辺 As Double = d長さ
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d辺 / 2, _p中心.Y - d辺 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d辺 / 2, _p中心.Y + d辺 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d辺)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d辺, _p左下.Y)

                _p左下.Y -= (d辺 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_長方形_横(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        If _dAspectRatio = 0 Then
            Return 0
        End If

        Dim count As Integer = 0
        Dim item As clsImageItem

        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d横 As Double = d長さ
        Dim d縦 As Double = d長さ * _dAspectRatio
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d横 / 2, _p中心.Y - d縦 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d横 / 2, _p中心.Y + d縦 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d縦)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d横, _p左下.Y)

                _p左下.Y -= (d縦 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_正方形_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d辺 As Double = d長さ / 4
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d辺 / 2, _p中心.Y - d辺 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d辺 / 2, _p中心.Y + d辺 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d辺)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d辺, _p左下.Y)

                _p左下.Y -= (d辺 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_長方形_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        If _dAspectRatio = 0 Then
            Return 0
        End If

        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d横 As Double = d長さ / (2 + 2 * _dAspectRatio)
        Dim d縦 As Double = d横 * _dAspectRatio
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d横 / 2, _p中心.Y - d縦 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d横 / 2, _p中心.Y + d縦 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d縦)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d横, _p左下.Y)

                _p左下.Y -= (d縦 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_円_径(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d径 As Double = d長さ
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d径 / 2, _p中心.Y - d径 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d径 / 2, _p中心.Y + d径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d径)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d径, _p左下.Y)

                _p左下.Y -= (d径 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_楕円_横径(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        If _dAspectRatio = 0 Then
            Return 0
        End If

        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d横径 As Double = d長さ
        Dim d縦径 As Double = d横径 * _dAspectRatio
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d横径 / 2, _p中心.Y - d縦径 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d横径 / 2, _p中心.Y + d縦径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d縦径)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d横径, _p左下.Y)

                _p左下.Y -= (d縦径 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_円_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d径 As Double = d長さ / Math.PI
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d径 / 2, _p中心.Y - d径 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d径 / 2, _p中心.Y + d径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d径)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d径, _p左下.Y)

                _p左下.Y -= (d径 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_楕円_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        If _dAspectRatio = 0 Then
            Return 0
        End If

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim count As Integer = 0
        Dim item As clsImageItem

        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d As Double = 1 / _dAspectRatio
        Dim ph As Double = Math.PI * (3 * (d + 1) - Math.Sqrt((3 * d + 1) * (d + 3)))
        Dim d縦径 As Double = 2 * d長さ / ph
        Dim d横径 As Double = d縦径 * d
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d横径 / 2, _p中心.Y - d縦径 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d横径 / 2, _p中心.Y + d縦径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d縦径)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d横径, _p左下.Y)

                _p左下.Y -= (d縦径 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_上半円_径(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d径 As Double = d長さ
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d径 / 2, _p中心.Y)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d径 / 2, _p中心.Y + d径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d径 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d径, _p左下.Y)

                _p左下.Y -= (d径 / 2 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_上半円_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d径 As Double = d長さ * 2 / Math.PI
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d径 / 2, _p中心.Y)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d径 / 2, _p中心.Y + d径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - d径 / 2)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d径, _p左下.Y)

                _p左下.Y -= (d径 / 2 + row.f_d描画厚)
            End If
            item.m_dひも幅 = row.f_d描画厚

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1
        Next
        Return count
    End Function


    Private Function add_横線(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '本数は、左下=マスター本数 ; 中心=全ひも本数(1回しか呼び出されないので)
        '　本数が1本の時: ゼロ位置からf_d描画厚をシフトした位置
        '　本数が複数の時: ゼロ位置からf_d描画厚をシフトしつつ指定点数
        Dim i本数 As Integer = row.f_iひも本数
        If Not isCenter Then
            i本数 = i本数 / row.f_i点数 'マスターのひも数
        End If
        '線の長さは、長さ/集計対象外時は出力ひも長
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X - d長さ / 2, _p中心.Y + i * row.f_d描画厚)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + d長さ / 2, _p中心.Y + i * row.f_d描画厚)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X, _p左下.Y - i * row.f_d描画厚)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + d長さ, _p左下.Y - i * row.f_d描画厚)
            End If

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1

            If row.f_d描画厚 = 0 Then
                Exit For
            End If
        Next
        If Not isCenter Then
            _p左下.Y -= i本数 * row.f_d描画厚
        End If
        Return count
    End Function

    Private Function add_縦線(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '本数は、左下=マスター本数 ; 中心=全ひも本数(1回しか呼び出されないので)
        '　本数が1本の時: ゼロ位置からf_d描画厚をシフトした位置
        '　本数が複数の時: ゼロ位置からf_d描画厚をシフトしつつ指定点数
        Dim i本数 As Integer = row.f_iひも本数
        If Not isCenter Then
            i本数 = i本数 / row.f_i点数 'マスターのひも数
        End If
        '線の長さは、長さ/集計対象外時は出力ひも長
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(_p中心.X + i * row.f_d描画厚, _p中心.Y - d長さ / 2)
                item.m_rひも位置.p右上 = New S実座標(_p中心.X + i * row.f_d描画厚, _p中心.Y + d長さ / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(_p左下.X + i * row.f_d描画厚, _p左下.Y - d長さ)
                item.m_rひも位置.p右上 = New S実座標(_p左下.X + i * row.f_d描画厚, _p左下.Y)
            End If

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            itemlist.AddItem(item)
            count += 1

            If row.f_d描画厚 = 0 Then
                Exit For
            End If
        Next
        If Not isCenter Then
            _p左下.Y -= d長さ
        End If

        Return count
    End Function


End Module
