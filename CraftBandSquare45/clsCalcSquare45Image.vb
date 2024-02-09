Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsUpDown
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstMasterTables

Partial Public Class clsCalcSquare45

    '位置計算用
    Class CBandPosition
        Public idx As Integer
        Public m_a四隅 As S四隅
        Public m_rひも位置 As S領域
        Public m_dひも幅 As Double
        Public m_row縦横展開 As tbl縦横展開Row

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("idx={0} ", idx)
            sb.AppendFormat("四隅:({0}) ", m_a四隅)
            sb.AppendFormat("ひも位置:({0}) ", m_rひも位置)
            sb.AppendFormat("ひも幅e:({0}) ", m_dひも幅)
            If m_row縦横展開 IsNot Nothing Then
                sb.AppendFormat("row縦横展開:({0},{1},{2})", m_row縦横展開.f_iひも種, m_row縦横展開.f_iひも番号, m_row縦横展開.f_i位置番号)
            Else
                sb.Append("No row縦横展開")
            End If
            Return sb.ToString
        End Function
    End Class

    '1～p_i縦ひもの本数=p_i横ひもの本数
    Dim _BandPositions横ひも As New List(Of CBandPosition)
    Dim _BandPositions縦ひも As New List(Of CBandPosition)

    Private Function setBandPositions(ByVal _ひも種 As enumひも種, ByVal table As tbl縦横展開DataTable, ByVal bandPositionList As List(Of CBandPosition)) As Integer
        For Each row As tbl縦横展開Row In table
            If row.f_iひも種 <> _ひも種 Then
                Continue For
            End If
            Dim idx As Integer = row.f_iひも番号
            If idx < 1 Then
                Continue For
            End If
            Do While bandPositionList.Count < idx + 1
                bandPositionList.Add(New CBandPosition)
            Loop
            bandPositionList(idx).m_row縦横展開 = row
            bandPositionList(idx).idx = idx
        Next

        'p_i縦ひもの本数=p_i横ひもの本数
        While bandPositionList.Count > p_i横ひもの本数 + 1
            bandPositionList.RemoveAt(bandPositionList.Count - 1)
        End While

        Return bandPositionList.Count
    End Function


    'Dim _ImageList横ひも As clsImageItemList
    'Dim _ImageList縦ひも As clsImageItemList

    Private Function toPoint(ByVal x As Double, ByVal y As Double) As S実座標
        Return New S実座標(_d四角の一辺 * x, _d四角の一辺 * y)
    End Function


    Public Function CalcImage(ByVal imgData As clsImageData) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        ''処理に伴い残されたイメージ情報
        'If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
        '    '処理に必要な情報がありません。
        '    p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
        '    Return False
        'End If

        Dim _ImageList横ひも As New clsImageItemList
        Dim _ImageList縦ひも As New clsImageItemList
        Try
            For idx As Integer = 1 To p_i横ひもの本数
                Dim bandposition As CBandPosition = _BandPositions横ひも(idx)
                Dim band As New clsImageItem(bandposition.m_row縦横展開)
                band.m_a四隅 = bandposition.m_a四隅
                band.m_rひも位置 = bandposition.m_rひも位置
                band.m_dひも幅 = bandposition.m_dひも幅
                _ImageList横ひも.AddItem(band)
            Next

            For idx As Integer = 1 To p_i縦ひもの本数
                Dim bandposition As CBandPosition = _BandPositions縦ひも(idx)
                Dim band As New clsImageItem(bandposition.m_row縦横展開)
                band.m_a四隅 = bandposition.m_a四隅
                band.m_rひも位置 = bandposition.m_rひも位置
                band.m_dひも幅 = bandposition.m_dひも幅
                _ImageList縦ひも.AddItem(band)
            Next
        Catch ex As Exception
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End Try

        '
        '基本のひも幅と基本色
        imgData.setBasics(_dひも幅の一辺, _Data.p_row目標寸法.Value("f_s基本色"))

        If _Data.p_row底_縦横.Value("f_b展開区分") Then
            '描画用のデータ追加
            regionUpDown底(_ImageList横ひも, _ImageList縦ひも)
        End If

        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing

        'その他の描画パーツ
        imgData.MoveList(imageList描画要素())

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function

    Private Function bandPositions長さ計算(ByVal _ひもリスト As List(Of CBandPosition), ByVal _ひも種 As enumひも種) As Boolean
        If _ひもリスト Is Nothing Then
            Return False
        End If

        For Each band As CBandPosition In _ひもリスト
            If band.m_row縦横展開 Is Nothing OrElse band.m_row縦横展開.f_iひも種 <> _ひも種 Then
                Continue For
            End If

            Dim p中央 As S実座標 = band.m_a四隅.p中央
            Dim d加算分 As Double = (_dひも長加算 + _d縁の垂直ひも長) * 2

            With band.m_row縦横展開
                band.m_dひも幅 = g_clsSelectBasics.p_d指定本幅(.f_i何本幅)
                If _ひも種 = enumひも種.i_横 Then
                    .f_dひも長 = band.m_a四隅.x最右 - band.m_a四隅.x最左
                    .f_d出力ひも長 = .f_dひも長 * _dひも長係数 + .f_dひも長加算 + d加算分

                    band.m_rひも位置.y最上 = p中央.Y + band.m_dひも幅 / 2
                    band.m_rひも位置.y最下 = p中央.Y - band.m_dひも幅 / 2
                    band.m_rひも位置.x最右 = p中央.X + .f_d出力ひも長 / 2
                    band.m_rひも位置.x最左 = p中央.X - .f_d出力ひも長 / 2

                ElseIf _ひも種 = enumひも種.i_縦 Then
                    .f_dひも長 = band.m_a四隅.y最上 - band.m_a四隅.y最下
                    .f_d出力ひも長 = .f_dひも長 * _dひも長係数 + .f_dひも長加算 + d加算分

                    band.m_rひも位置.x最右 = p中央.X + band.m_dひも幅 / 2
                    band.m_rひも位置.x最左 = p中央.X - band.m_dひも幅 / 2
                    band.m_rひも位置.y最上 = p中央.Y + .f_d出力ひも長 / 2
                    band.m_rひも位置.y最下 = p中央.Y - .f_d出力ひも長 / 2

                End If
            End With
        Next
        Return True
    End Function


    Private Function setBandPositions横ひも() As Boolean

        Dim delta225 As S差分 = Unit225 * _d四角の一辺 '／
        Dim delta315 As S差分 = Unit315 * _d四角の一辺 '＼
        Dim delta As S差分

        Dim updowncount As Integer
        Dim samecount As Integer
        If _i縦の四角数 <= _i横の四角数 Then
            updowncount = _i縦の四角数
            samecount = _i横の四角数 - _i縦の四角数
            delta = delta225 '／
        Else
            updowncount = _i横の四角数
            samecount = _i縦の四角数 - _i横の四角数
            delta = delta315 '＼
        End If


        Dim _左上 As S実座標 = toPoint(-2 * _d高さの四角数 + (_i横の四角数 - _i縦の四角数) / 2, (_i横の四角数 + _i縦の四角数) / 2)
        Dim _右上 As S実座標 = toPoint(2 * _d高さの四角数 + (_i横の四角数 - _i縦の四角数) / 2, (_i横の四角数 + _i縦の四角数) / 2)

        '上から下へ
        Dim idx As Integer = 1
        For i As Integer = 0 To updowncount - 1
            Dim band As CBandPosition = _BandPositions横ひも(idx)
            band.m_row縦横展開.f_i位置番号 = -updowncount + i
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * (i * 2 + 1)

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p右上 = _右上
            '
            _左上 = _左上 + delta225 '／
            _右上 = _右上 + delta315 '＼
            band.m_a四隅.p左下 = _左上
            band.m_a四隅.p右下 = _右上

            idx += 1
        Next
        For i As Integer = 0 To samecount - 1
            Dim band As CBandPosition = _BandPositions横ひも(idx)
            band.m_row縦横展開.f_i位置番号 = 0
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * (updowncount * 2)

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p右上 = _右上
            '
            _左上 = _左上 + delta
            _右上 = _右上 + delta
            band.m_a四隅.p左下 = _左上
            band.m_a四隅.p右下 = _右上
            idx += 1
        Next
        For i As Integer = 0 To updowncount - 1
            Dim band As CBandPosition = _BandPositions横ひも(idx)
            band.m_row縦横展開.f_i位置番号 = i + 1
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * ((updowncount * 2) - (i * 2 + 1))

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p右上 = _右上
            '
            _左上 = _左上 + delta315 '＼
            _右上 = _右上 + delta225 '／
            band.m_a四隅.p左下 = _左上
            band.m_a四隅.p右下 = _右上

            idx += 1
        Next

        Return bandPositions長さ計算(_BandPositions横ひも, enumひも種.i_横)
    End Function


    Private Function setBandPositions縦ひも() As Boolean

        Dim delta45 As S差分 = Unit45 * _d四角の一辺 '／
        Dim delta315 As S差分 = Unit315 * _d四角の一辺 '＼
        Dim delta As S差分

        Dim updowncount As Integer
        Dim samecount As Integer
        If _i縦の四角数 <= _i横の四角数 Then
            updowncount = _i縦の四角数
            samecount = _i横の四角数 - _i縦の四角数
            delta = delta45 '／
        Else
            updowncount = _i横の四角数
            samecount = _i縦の四角数 - _i横の四角数
            delta = delta315 '＼

        End If


        Dim _左上 As S実座標 = toPoint(-(_i横の四角数 + _i縦の四角数) / 2, 2 * _d高さの四角数 - (_i横の四角数 - _i縦の四角数) / 2)
        Dim _左下 As S実座標 = toPoint(-(_i横の四角数 + _i縦の四角数) / 2, -2 * _d高さの四角数 - (_i横の四角数 - _i縦の四角数) / 2)

        '左から右へ
        Dim idx As Integer = 1
        For i As Integer = 0 To updowncount - 1
            Dim band As CBandPosition = _BandPositions縦ひも(idx)
            band.m_row縦横展開.f_i位置番号 = -updowncount + i
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * (i * 2 + 1)

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p左下 = _左下
            '
            _左上 = _左上 + delta45 '／
            _左下 = _左下 + delta315 '＼
            band.m_a四隅.p右上 = _左上
            band.m_a四隅.p右下 = _左下

            idx += 1
        Next
        For i As Integer = 0 To samecount - 1
            Dim band As CBandPosition = _BandPositions縦ひも(idx)
            band.m_row縦横展開.f_i位置番号 = 0
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * (updowncount * 2)

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p左下 = _左下
            '
            _左上 = _左上 + delta
            _左下 = _左下 + delta
            band.m_a四隅.p右上 = _左上
            band.m_a四隅.p右下 = _左下

            idx += 1
        Next
        For i As Integer = 0 To updowncount - 1
            Dim band As CBandPosition = _BandPositions縦ひも(idx)
            band.m_row縦横展開.f_i位置番号 = i + 1
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * ((updowncount * 2) - (i * 2 + 1))

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p左下 = _左下
            '
            _左上 = _左上 + delta315 '＼
            _左下 = _左下 + delta45 '／
            band.m_a四隅.p右上 = _左上
            band.m_a四隅.p右下 = _左下

            idx += 1
        Next

        Return bandPositions長さ計算(_BandPositions縦ひも, enumひも種.i_縦)
    End Function





    Function imageList描画要素() As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        Dim d差の半分 As Double = (_i横の四角数 - _i縦の四角数) / 2
        Dim d和の半分 As Double = (_i横の四角数 + _i縦の四角数) / 2

        Dim a底 As S四隅
        a底.pA = (toPoint(d差の半分, d和の半分)) '右上
        a底.pC = -a底.pA '左下
        a底.pD = toPoint(d和の半分, d差の半分) '右下
        a底.pB = -a底.pD '左上

        Dim d縁XY As Double = _d縁の高さ / ROOT2
        Dim line As S線分

        '全体枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._全体枠, 1)
        item.m_a四隅.pA = toPoint(d差の半分, (2 * _d高さの四角数 + d和の半分))
        item.m_a四隅.pC = -item.m_a四隅.pA
        item.m_a四隅.pD = toPoint(2 * _d高さの四角数 + d和の半分, d差の半分)
        item.m_a四隅.pB = -item.m_a四隅.pD
        itemlist.AddItem(item)

        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底
        itemlist.AddItem(item)

        '横の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.pA = toPoint(-_d高さの四角数 + d差の半分, _d高さの四角数 + d和の半分)
        item.m_a四隅.pB = toPoint(-_d高さの四角数 - d和の半分, _d高さの四角数 - d差の半分)
        item.m_a四隅.pC = a底.pB
        item.m_a四隅.pD = a底.pA
        'ABを135度シフト
        line = New S線分(item.m_a四隅.pA, item.m_a四隅.pB)
        line += Unit135 * d縁XY
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅.pA = a底.pD
        item.m_a四隅.pB = a底.pC
        item.m_a四隅.pC = toPoint(_d高さの四角数 - d差の半分, -_d高さの四角数 - d和の半分)
        item.m_a四隅.pD = toPoint(_d高さの四角数 + d和の半分, -_d高さの四角数 + d差の半分)
        'CDを315度シフト
        line = New S線分(item.m_a四隅.pC, item.m_a四隅.pD)
        line += Unit315 * d縁XY
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        '縦の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅.pA = toPoint(_d高さの四角数 + d差の半分, _d高さの四角数 + d和の半分)
        item.m_a四隅.pB = a底.pA
        item.m_a四隅.pC = a底.pD
        item.m_a四隅.pD = toPoint(_d高さの四角数 + d和の半分, _d高さの四角数 + d差の半分)
        'DAを45度シフト
        line = New S線分(item.m_a四隅.pD, item.m_a四隅.pA)
        line += Unit45 * d縁XY
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.pA = a底.pB
        item.m_a四隅.pB = toPoint(-_d高さの四角数 - d和の半分, -_d高さの四角数 - d差の半分)
        item.m_a四隅.pC = toPoint(-_d高さの四角数 - d差の半分, -_d高さの四角数 - d和の半分)
        item.m_a四隅.pD = a底.pC
        'BCを225度シフト
        line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
        line += Unit225 * d縁XY
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        '底の中央線
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)
        If _i横の四角数 = _i縦の四角数 Then
            line = New clsImageItem.S線分(a底.pC, a底.pA) '底のC,底のA
            item.m_lineList.Add(line)

            line = New clsImageItem.S線分(a底.pB, a底.pD) '底のB,底のD
            item.m_lineList.Add(line)
        Else
            Dim p上クロス点 As S実座標
            Dim p下クロス点 As S実座標
            If 0 <= d差の半分 Then
                p上クロス点 = toPoint(d差の半分, d差の半分)
                p下クロス点 = toPoint(-d差の半分, -d差の半分)

                line = New clsImageItem.S線分(p上クロス点, a底.pD)
                item.m_lineList.Add(line)

                line = New clsImageItem.S線分(p下クロス点, a底.pB)
                item.m_lineList.Add(line)
            Else
                p上クロス点 = toPoint(d差の半分, -d差の半分)
                p下クロス点 = toPoint(-d差の半分, d差の半分)

                line = New clsImageItem.S線分(p上クロス点, a底.pB)
                item.m_lineList.Add(line)

                line = New clsImageItem.S線分(p下クロス点, a底.pD)
                item.m_lineList.Add(line)
            End If
            line = New clsImageItem.S線分(a底.pA, p上クロス点)
            item.m_lineList.Add(line)

            line = New clsImageItem.S線分(p下クロス点, a底.pC)
            item.m_lineList.Add(line)

            line = New clsImageItem.S線分(p下クロス点, p上クロス点)
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        Return itemlist
    End Function



    Dim _CUpDown As New clsUpDown   'CheckBoxTableは使わない

    '底の上下をm_regionListにセット
    Private Function regionUpDown底(ByVal _ImageList横ひも As clsImageItemList, ByVal _ImageList縦ひも As clsImageItemList) As Boolean
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            Return False
        End If

        _CUpDown.TargetFace = enumTargetFace.Bottom '底
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(0)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If

        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                If _CUpDown.GetIsDown(iTate, iYoko) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If itemYoko IsNot Nothing Then
                        itemTate.m_regionList.Add領域(itemYoko.m_rひも位置)
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
            If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                If _CUpDown.GetIsUp(iTate, iYoko) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If itemTate IsNot Nothing Then
                        itemYoko.m_regionList.Add領域(itemTate.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        Return True
    End Function


End Class
