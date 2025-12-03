Imports System.Text.RegularExpressions
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstDataTables

Public Module mdlAddPartsImage

    '追加品のレコードをイメージ情報化
    Function AddPartsImage(ByVal imgData As clsImageData, ByVal editAddParts As ctrAddParts, ByVal is2nd As Boolean) As Integer
        If imgData Is Nothing OrElse editAddParts Is Nothing Then
            Return -1
        End If
        _基本のひも幅 = imgData.BasicBandWidth

        'i番号,iひも番号順
        Dim rows() As tbl追加品Row = editAddParts.GetAddPartsRecords()
        If rows.Count = 0 Then
            Return 0
        End If

        '現在の描画位置
        __p左下 = imgData.CurrentItemDrawingRect.p左下 '最下
        __p中心 = imgData.CenterCoordinates


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
            If 0 < i点数 AndAlso (first描画位置 <> enum描画位置.i_なし) AndAlso
                (Not is2nd AndAlso (rows(iFrom).f_b描画区分) OrElse (is2nd AndAlso rows(iFrom).f_b描画区分2)) Then
                Dim isCenter As Boolean = (first描画位置 <> enum描画位置.i_左下)
                If isCenter Then
                    i点数 = 1 '同位置なので描画は1回だけ
                End If

                '点数分を繰り返す(左下の時)
                For iset As Integer = 1 To i点数
                    _list点 = New List(Of S実座標)

                    '各、iひも番号
                    For isub As Integer = iFrom To iTo
                        Dim row As tbl追加品Row = rows(isub)
                        '同位置のみ対象→異なってもよい
                        'If row.f_i描画位置 <> first描画位置 Then
                        If row.f_i描画位置 = enum描画位置.i_なし Then
                            Continue For
                        End If
                        isCenter = (row.f_i描画位置 <> enum描画位置.i_左下)

                        '文字表示
                        Dim isMark As Boolean = (iset = 1 AndAlso Not String.IsNullOrWhiteSpace(row.f_s記号))
                        '描画点(固定の場合)
                        Dim p中心 As S実座標 = __p中心
                        Dim d角度 As Double = 0
                        If row.f_i描画位置 = enum描画位置.i_中心 Then
                            p中心 = __p中心
                        ElseIf row.f_i描画位置 = enum描画位置.i_座標 Then
                            '#106 座標と角度を指定
                            If Parse座標と角度(row.f_s座標, p中心, d角度) Then
                                Dim dif As New S差分(pOrigin, p中心)
                                p中心 = __p中心 + dif
                            Else
                                '設定文字列が不正
                                Continue For
                            End If
                        End If 'enum描画位置.i_左下 は参照なし

                        Select Case row.f_i描画形状
                            Case enum描画形状.i_横バンド
                                count += add_横バンド(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_横四角
                                count += add_横四角(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_正方形_辺
                                count += add_正方形_辺(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_長方形_横
                                count += add_長方形_横(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_正方形_周
                                count += add_正方形_周(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_長方形_周
                                count += add_長方形_周(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_円_径
                                count += add_円_径(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_楕円_横径
                                count += add_楕円_横径(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_円_周
                                count += add_円_周(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_楕円_周
                                count += add_楕円_周(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_上半円_径
                                count += add_上半円_径(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_上半円_周
                                count += add_上半円_周(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_横線
                                count += add_横線(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_縦線
                                count += add_縦線(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_線分
                                count += add_線分(itemlist, isCenter, p中心, isMark, row, d角度)

                            Case enum描画形状.i_点
                                count += add_点(itemlist, isCenter, p中心, isMark, row)

                            Case enum描画形状.i_バンド
                                count += add_バンド(itemlist, isCenter, p中心, isMark, row, d角度)

                            Case Else
                                Continue For
                        End Select
                    Next

                    _list点.Clear()
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

    Private Function Parse座標と角度(ByVal obj As Object, ByRef p As S実座標, ByRef angle As Double) As Boolean
        '初期値(原点、0度)
        p.Zero()
        angle = 0

        '空やNullは初期値とします
        If obj Is Nothing OrElse IsDBNull(obj) Then
            Return True
        End If
        Dim str As String = CStr(obj)
        If String.IsNullOrWhiteSpace(str) Then
            Return True
        End If

        ' ----------------- 正規表現パターン -----------------
        ' 形式 1: (x,y)α または (x,y)  -> グループ 1, 2, 5
        ' 形式 2: x,y,α または x,y      -> グループ 3, 4, 6
        ' 形式 3: (x,y),α               -> グループ 3, 4, 6 (カンマ区切りとして処理)
        Dim pattern As String = "^\s*(?:" &
                            "\(\s*([\d\.-]+)\s*,\s*([\d\.-]+)\s*\)\s*([\d\.-]+)?" &
                            "|" &
                            "([\d\.-]+)\s*,\s*([\d\.-]+)\s*(?:,\s*([\d\.-]+)\s*)?" &
                            ")\s*$"
        Dim match As Match = Regex.Match(str, pattern)

        If Not match.Success Then
            Return False ' パターンに一致しない (NG)
        End If

        Dim xStr As String
        Dim yStr As String
        Dim alphaStr As String = ""

        'x, y の値を取得
        If match.Groups(1).Success Then
            ' --- 形式 (x,y)α にマッチした場合 (グループ 1, 2, 3) 
            xStr = match.Groups(1).Value
            yStr = match.Groups(2).Value
            alphaStr = match.Groups(3).Value ' αはグループ3

        ElseIf match.Groups(4).Success Then
            ' --- 形式 x,y,α または (x,y),α にマッチした場合 (グループ 4, 5, 6) 
            xStr = match.Groups(4).Value
            yStr = match.Groups(5).Value
            alphaStr = match.Groups(6).Value ' αはグループ6

        Else
            Return False ' マッチングロジックの失敗
        End If

        '数値変換
        ' x, y が正しくなければ NG
        If Not Double.TryParse(xStr, p.X) Then Return False
        If Not Double.TryParse(yStr, p.Y) Then Return False

        'α の値を取得 (省略時は初期値ゼロ)
        If 0 < alphaStr.Length Then
            If Not Double.TryParse(alphaStr, angle) Then Return False
        End If

        Return True '変換に成功
    End Function



    Dim __p中心 As S実座標
    Dim __p左下 As S実座標
    Dim _list点 As List(Of S実座標)
    Dim _基本のひも幅 As Double

    Private Function add_横バンド(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As New clsImageItem(ImageTypeEnum._付属品, row)
        item.m_bandList = New CBandList

        '左下:マスターの1セット分を描く 
        '中心:マスターが複数本であっても1本分だけ描く
        Dim i本数 As Integer = IIf(isCenter, 1, row.f_iひも本数 / row.f_i点数) 'マスターのひも数
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)

        For i As Integer = 1 To i本数

            If isCenter Then
                '指定位置をバンドの中心として描画
                item.m_rひも位置.p左下 = New S実座標(p中心.X - row.f_d長さ / 2, p中心.Y - dひも幅 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + row.f_d長さ / 2, p中心.Y + dひも幅 / 2)
            Else
                If i = 1 AndAlso row.f_iひも番号 = 1 Then
                    __p左下.Y -= dひも幅
                End If
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - dひも幅)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + row.f_d長さ, __p左下.Y)
                __p左下.Y -= dひも幅 * 2
            End If

            Dim band As New CBand(row)
            band.SetBandF(New S線分(item.m_rひも位置.p左下, item.m_rひも位置.p右下), dひも幅, Unit90)
            item.m_bandList.Add(band)

            If isMark AndAlso i = 1 Then
                item.p_p文字位置 = item.m_rひも位置.p右上
            End If
            count += 1
        Next
        If 0 < count Then
            itemlist.AddItem(item)
        End If

        Return count
    End Function


    Private Function add_横四角(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)
        Dim d幅 As Double = row.f_d描画厚
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d長さ / 2, p中心.Y - d幅 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d長さ / 2, p中心.Y + d幅 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d幅)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d長さ, __p左下.Y)

                __p左下.Y -= d幅 * 2
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


    Private Function add_正方形_辺(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d辺 As Double = d長さ
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d辺 / 2, p中心.Y - d辺 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d辺 / 2, p中心.Y + d辺 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d辺)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d辺, __p左下.Y)

                __p左下.Y -= (d辺 + row.f_d描画厚)
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


    Private Function add_長方形_横(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim dAspectRatio As Double = row.f_d縦対横比率
        If dAspectRatio = 0 Then
            Return 0
        End If

        Dim count As Integer = 0
        Dim item As clsImageItem

        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d横 As Double = d長さ
        Dim d縦 As Double = d長さ * dAspectRatio
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d横 / 2, p中心.Y - d縦 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d横 / 2, p中心.Y + d縦 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d縦)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d横, __p左下.Y)

                __p左下.Y -= (d縦 + row.f_d描画厚)
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


    Private Function add_正方形_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d辺 As Double = d長さ / 4
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d辺 / 2, p中心.Y - d辺 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d辺 / 2, p中心.Y + d辺 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d辺)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d辺, __p左下.Y)

                __p左下.Y -= (d辺 + row.f_d描画厚)
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


    Private Function add_長方形_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim dAspectRatio As Double = row.f_d縦対横比率
        If dAspectRatio = 0 Then
            Return 0
        End If

        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d横 As Double = d長さ / (2 + 2 * dAspectRatio)
        Dim d縦 As Double = d横 * dAspectRatio
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d横 / 2, p中心.Y - d縦 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d横 / 2, p中心.Y + d縦 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d縦)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d横, __p左下.Y)

                __p左下.Y -= (d縦 + row.f_d描画厚)
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


    Private Function add_円_径(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d径 As Double = d長さ
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d径 / 2, p中心.Y - d径 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d径 / 2, p中心.Y + d径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d径)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d径, __p左下.Y)

                __p左下.Y -= (d径 + row.f_d描画厚)
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


    Private Function add_楕円_横径(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim dAspectRatio As Double = row.f_d縦対横比率
        If dAspectRatio = 0 Then
            Return 0
        End If

        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d横径 As Double = d長さ
        Dim d縦径 As Double = d横径 * dAspectRatio
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d横径 / 2, p中心.Y - d縦径 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d横径 / 2, p中心.Y + d縦径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d縦径)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d横径, __p左下.Y)

                __p左下.Y -= (d縦径 + row.f_d描画厚)
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


    Private Function add_円_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d径 As Double = d長さ / Math.PI
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d径 / 2, p中心.Y - d径 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d径 / 2, p中心.Y + d径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d径)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d径, __p左下.Y)

                __p左下.Y -= (d径 + row.f_d描画厚)
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


    Private Function add_楕円_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim dAspectRatio As Double = row.f_d縦対横比率
        If dAspectRatio = 0 Then
            Return 0
        End If

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim count As Integer = 0
        Dim item As clsImageItem

        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d As Double = 1 / dAspectRatio
        Dim ph As Double = Math.PI * (3 * (d + 1) - Math.Sqrt((3 * d + 1) * (d + 3)))
        Dim d縦径 As Double = 2 * d長さ / ph
        Dim d横径 As Double = d縦径 * d
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d横径 / 2, p中心.Y - d縦径 / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d横径 / 2, p中心.Y + d縦径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d縦径)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d横径, __p左下.Y)

                __p左下.Y -= (d縦径 + row.f_d描画厚)
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


    Private Function add_上半円_径(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d径 As Double = d長さ
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d径 / 2, p中心.Y)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d径 / 2, p中心.Y + d径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d径 / 2)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d径, __p左下.Y)

                __p左下.Y -= (d径 / 2 + row.f_d描画厚)
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


    Private Function add_上半円_周(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        '左下・中心: 1点,長さ/出力ひも長(集計対象外時)
        Dim i本数 As Integer = 1
        Dim d長さ As Double = IIf(row.f_b集計対象外区分, row.f_d出力ひも長, row.f_d長さ)

        Dim d径 As Double = d長さ * 2 / Math.PI
        For i As Integer = 1 To i本数
            item = New clsImageItem(ImageTypeEnum._付属品, row)

            If isCenter Then
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d径 / 2, p中心.Y)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d径 / 2, p中心.Y + d径 / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - d径 / 2)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d径, __p左下.Y)

                __p左下.Y -= (d径 / 2 + row.f_d描画厚)
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


    Private Function add_横線(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
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
                item.m_rひも位置.p左下 = New S実座標(p中心.X - d長さ / 2, p中心.Y + i * row.f_d描画厚)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + d長さ / 2, p中心.Y + i * row.f_d描画厚)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X, __p左下.Y - i * row.f_d描画厚)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + d長さ, __p左下.Y - i * row.f_d描画厚)
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
            __p左下.Y -= i本数 * row.f_d描画厚
        End If
        Return count
    End Function

    Private Function add_縦線(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
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
                item.m_rひも位置.p左下 = New S実座標(p中心.X + i * row.f_d描画厚, p中心.Y - d長さ / 2)
                item.m_rひも位置.p右上 = New S実座標(p中心.X + i * row.f_d描画厚, p中心.Y + d長さ / 2)
            Else
                item.m_rひも位置.p左下 = New S実座標(__p左下.X + i * row.f_d描画厚, __p左下.Y - d長さ)
                item.m_rひも位置.p右上 = New S実座標(__p左下.X + i * row.f_d描画厚, __p左下.Y)
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
            __p左下.Y -= d長さ
        End If

        Return count
    End Function

    '角度は座標とセットで指定した時のみ,以外はゼロ
    Function add_線分(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row, ByVal d角度 As Double) As Integer
        Dim count As Integer = 0
        Dim item As clsImageItem

        Dim d長さ As Double = row.f_d長さ
        'Dim d角度 As Double = row.f_dひも長加算


        Dim delta As New S差分(New S実座標(0, 0), New S実座標(d長さ, 0))
        Dim line As S線分
        item = New clsImageItem(ImageTypeEnum._付属品, row)

        If isCenter Then
            line = New S線分(p中心, p中心 + delta.Rotate(d角度))
        Else
            line = New S線分(__p左下, __p左下 + delta.Rotate(d角度))
        End If

        '2点をセットする(領域として扱わない)
        item.m_rひも位置.p左下 = line.p開始
        item.m_rひも位置.p右上 = line.p終了
        item.m_dひも幅 = row.f_d描画厚

        If isMark Then
            item.p_p文字位置 = item.m_rひも位置.p右上
        End If
        itemlist.AddItem(item)
        count += 1

        If Not isCenter Then
            __p左下.Y -= item.m_rひも位置.y高さ + row.f_d描画厚
        End If
        Return count
    End Function

    Function add_点(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row) As Integer
        Dim count As Integer = 0
        If _list点.Count = 0 Then
            If isCenter Then
                _list点.Add(p中心)
            Else
                _list点.Add(__p左下)
            End If
            Return count
        End If

        '最後の点
        Dim pLast As S実座標 = _list点(_list点.Count - 1)
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)

        '2点をセットする(領域として扱わない)
        If isCenter Then
            item.m_rひも位置.p左下 = pLast
            item.m_rひも位置.p右上 = p中心
            _list点.Add(p中心)
        Else
            item.m_rひも位置.p左下 = pLast
            item.m_rひも位置.p右上 = __p左下
            _list点.Add(__p左下)
        End If

        If isMark Then
            item.p_p文字位置 = item.m_rひも位置.p右上
        ElseIf item.m_rひも位置.IsDot Then
            Return 0
        End If

        '描画あり
        item.m_dひも幅 = row.f_d描画厚
        itemlist.AddItem(item)
        count += 1

        'シフトなし
        'If Not isCenter Then
        '    __p左下.Y -= item.m_rひも位置.y高さ + row.f_d描画厚
        'End If
        Return count
    End Function

    '角度は座標とセットで指定した時のみ,以外はゼロ
    Function add_バンド(ByVal itemlist As clsImageItemList, ByVal isCenter As Boolean, ByVal p中心 As S実座標, ByVal isMark As Boolean, ByVal row As tbl追加品Row, ByVal d角度 As Double) As Integer
        '●→───────┐T 　┌─┐┌─┐
        '│０度　　　　　　│^ 　│　││　│     反・deltaAx(F→T)方向に並べる
        '└────────┘F 　│90││　│     間隔は描画厚
        '┌────────┐　　│度││　│
        '│　　　　　　　　│　　↑　││　│
        '└────────┘　　●─┘└─┘
        '　　　　　　　　　　　　T <- F
        Dim item As clsImageItem = New clsImageItem(ImageTypeEnum._付属品, row)
        item.m_bandList = New CBandList
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)

        '左下:マスターの1セット分を描く (点数分呼び出されるので)
        '中心:全点数分を描く(呼び出しは1回だけなので)
        Dim i本数 As Integer = IIf(isCenter, row.f_iひも本数, row.f_iひも本数 / row.f_i点数) 'マスターのひも数
        If i本数 < 1 Then
            Return 0
        End If

        Dim deltaBand As New S差分(d角度) '始点→終点
        Dim deltaAx As New S差分(d角度 + 90) 'F→T(軸方向)
        Dim deltaOrder As New S差分(d角度 - 90) 'T→F方向

        Dim p As S実座標
        If isCenter Then
            p = p中心 + deltaOrder * (dひも幅 / 2)
        Else
            p = __p左下 + deltaOrder * dひも幅
            __p左下.Y -= (dひも幅 + row.f_d描画厚) * i本数
        End If
        Dim line As New S線分(p, p + deltaBand * row.f_d長さ)

        For i As Integer = 1 To i本数
            Dim band As New CBand(row)
            band.SetBand(line, dひも幅, deltaAx)
            If isMark AndAlso i = 1 Then
                band.SetMarkPosition(CBand.enumMarkPosition._始点の前, _基本のひも幅)
            End If

            item.m_bandList.Add(band)
            line += deltaOrder * (dひも幅 + row.f_d描画厚)
        Next
        itemlist.AddItem(item)
        Return i本数
    End Function

End Module
