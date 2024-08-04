Imports CraftBand
Imports CraftBand.clsImageItem
Imports CraftBand.clsDataTables
Imports CraftBand.Tables.dstDataTables

Partial Public Class clsCalcKnot


    'CAD測定値
    Const c_foldingRatio As Double = 2 / 6.43   '要尺の折り位置までの比
    Const c_tiltRatio As Double = 0.216   '折りの傾き(幅に対する差の比)

    'リスト出力時に生成
    Dim _ImageList横ひも As clsImageItemList   '横ひもの展開レコードを含む
    Dim _ImageList縦ひも As clsImageItemList   '縦ひもの展開レコードを含む

    'プレビュー時に生成
    Dim _imageList側面ひも As clsImageItemList    '側面の展開レコードを含む
    Dim _ImageListコマ As clsImageItemList
    Dim _ImageList描画要素 As clsImageItemList '底や側面の展開図
    Dim _ImageList開始位置 As clsImageItemList

    Private Function toPoint(ByVal x As Double, ByVal y As Double) As S実座標
        Return New S実座標(_dコマベース寸法 * x, _dコマベース寸法 * y)
    End Function

    Private Function getBand(ByVal pos As enumひも種, ByVal idx As Integer) As tbl縦横展開Row
        Try
            Dim band As clsImageItem = Nothing
            Select Case pos
                Case enumひも種.i_横
                    '上からidx番目(1～),マイナスは下から(-1～-横のコマ数)
                    If idx < 0 Then
                        idx = _i横のコマ数 + idx + 1
                    End If
                    band = _ImageList横ひも.GetRowItem(enumひも種.i_横, idx, False)
                Case enumひも種.i_縦
                    '左からidx番目(1～),マイナスは右から(-1～-縦のコマ数)
                    If idx < 0 Then
                        idx = _i縦のコマ数 + idx + 1
                    End If
                    band = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, idx, False)
                Case enumひも種.i_側面
                    '下からidx番目の色(1～),マイナスは上から(-1～-側面のコマ数)
                    If idx < 0 Then
                        idx = _i高さのコマ数 + _i折り返しコマ数 + idx + 1
                    End If
                    band = _imageList側面ひも.GetRowItem(enumひも種.i_側面, idx, False)
            End Select
            If band Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "getBand Err:{0} {1}", pos, idx)
                Return Nothing
            End If
            Return band.m_row縦横展開

        Catch ex As Exception
            g_clsLog.LogException(ex, "getBand", pos, idx)
            Return Nothing
        End Try
    End Function

    'コマベース左上→コマの中心
    Private Function addコマ(ByVal p左上 As S実座標, ByVal y_band As tbl縦横展開Row, ByVal t_band As tbl縦横展開Row,
                           ByVal dひも幅 As Double, ByVal isleft As Boolean) As Boolean

        Dim knotitem As New clsImageItem(p左上 + (Unit315 * (_dコマベース寸法 / 2)), y_band, t_band,
                                            dひも幅, _dコマの寸法, _dコマ間のすき間, isleft)
        If _ImageListコマ Is Nothing Then
            _ImageListコマ = New clsImageItemList
        End If
        _ImageListコマ.Add(knotitem)

        Return y_band IsNot Nothing AndAlso t_band IsNot Nothing
    End Function

    '横ひもリストの記号出力情報
    Private Function imageList横記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal disp_band As Boolean) As Boolean
        If _ImageList横ひも Is Nothing Then
            Return False
        End If

        '側面いっぱい
        Dim length0 As S差分
        If disp_band Then
            length0 = Unit0 * _dコマベース寸法 * (_i高さのコマ数 + _i折り返しコマ数)
        End If
        Dim band270 As S差分 = Unit270 * dひも幅

        '左側面の左 コマは右側面も
        Dim _左上 As S実座標 = toPoint(-(_i高さのコマ数 + _i折り返しコマ数 + (_i横のコマ数 / 2)), (_i縦のコマ数 / 2))
        Dim _左上_右側面 As S実座標 = toPoint((_i横のコマ数 / 2), (_i縦のコマ数 / 2))
        Dim band_pos As S差分
        If Not isKnotLeft Then
            band_pos = Unit270 * ((_dコマベース寸法 / 2) - dひも幅)
        Else
            band_pos = Unit270 * (_dコマベース寸法 / 2)
        End If

        '上から下へ
        For idx As Integer = 1 To p_i横ひもの本数 '_i縦のコマ数
            Dim band As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, idx)
            band.m_rひも位置.p右上 = _左上 + band_pos + length0
            band.m_rひも位置.p左下 = _左上 + band_pos + band270
            '
            For j = 1 To _i高さのコマ数 + _i折り返しコマ数
                addコマ(_左上 + Unit0 * (j - 1) * _dコマベース寸法, band.m_row縦横展開, getBand(enumひも種.i_側面, -j), dひも幅, isKnotLeft)
                addコマ(_左上_右側面 + Unit0 * (j - 1) * _dコマベース寸法, band.m_row縦横展開, getBand(enumひも種.i_側面, j), dひも幅, isKnotLeft)
            Next
            _左上 = _左上 + (Unit270 * _dコマベース寸法)
            _左上_右側面 = _左上_右側面 + (Unit270 * _dコマベース寸法)
        Next

        Return True
    End Function

    '縦ひもリストの記号出力情報
    Private Function imageList縦記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal disp_band As Boolean) As Boolean
        If _ImageList縦ひも Is Nothing Then
            Return False
        End If

        '側面いっぱい
        Dim length270 As S差分
        If disp_band Then
            length270 = Unit270 * _dコマベース寸法 * (_i高さのコマ数 + _i折り返しコマ数)
        End If
        Dim band0 As S差分 = Unit0 * dひも幅

        '上側面の上
        Dim _左上 As S実座標 = toPoint(-(_i横のコマ数 / 2), _i高さのコマ数 + _i折り返しコマ数 + (_i縦のコマ数 / 2))
        Dim _左上_底 As S実座標 = toPoint(-(_i横のコマ数 / 2), (_i縦のコマ数 / 2))
        Dim band_pos As S差分
        If Not isKnotLeft Then
            band_pos = Unit0 * (_dコマベース寸法 / 2)
        Else
            band_pos = Unit0 * ((_dコマベース寸法 / 2) - dひも幅)
        End If

        '上側面;左から右へ
        For idx As Integer = 1 To p_i縦ひもの本数 '_i横のコマ数
            Dim band As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, idx)
            band.m_rひも位置.p右上 = _左上 + band_pos + band0
            band.m_rひも位置.p左下 = _左上 + band_pos + length270

            '底のコマ
            For j As Integer = 1 To p_i横ひもの本数 '_i縦のコマ数
                addコマ(_左上_底 + Unit270 * (j - 1) * _dコマベース寸法, getBand(enumひも種.i_横, j), band.m_row縦横展開, dひも幅, isKnotLeft)
            Next

            _左上 = _左上 + (Unit0 * _dコマベース寸法)
            _左上_底 = _左上_底 + (Unit0 * _dコマベース寸法)
        Next

        Return True
    End Function

    '_imageList側面ひも生成、縦横展開DataTable化したレコードを含む
    Function imageList側面記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal disp_band As Boolean) As Boolean

        '側面のレコードを縦横レコード化
        Dim tmptable As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        Dim idx As Integer = 1
        For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, "f_i番号 ASC , f_iひも番号 ASC")
            If r.f_i番号 = cHemNumber Then
                Continue For
            End If
            For i As Integer = 1 To r.f_iひも本数
                row = tmptable.Newtbl縦横展開Row
                row.f_iひも種 = enumひも種.i_側面
                row.f_iひも番号 = idx
                row.f_i何本幅 = r.f_i何本幅
                row.f_s記号 = r.f_s記号
                row.f_s色 = r.f_s色
                row.f_dひも長 = r.f_dひも長
                row.f_dひも長加算 = r.f_dひも長加算
                row.f_dひも長加算2 = 0
                row.f_d出力ひも長 = r.f_d連続ひも長
                tmptable.Rows.Add(row)

                idx += 1
            Next
        Next

        '以降参照するのでここでセットする
        _imageList側面ひも = New clsImageItemList(tmptable, String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号))


        '側面いっぱい
        Dim length0 As S差分
        If disp_band Then
            length0 = Unit0 * _dコマベース寸法 * _i横のコマ数
        End If
        Dim band270 As S差分 = Unit270 * dひも幅

        '下側面の左
        Dim _左上 As S実座標 = toPoint(-(_i横のコマ数 / 2), -(_i縦のコマ数 / 2))
        Dim _左下_上側面 As S実座標 = toPoint(-(_i横のコマ数 / 2), 1 + (_i縦のコマ数 / 2))
        Dim band_pos As S差分
        If Not isKnotLeft Then
            band_pos = Unit270 * ((_dコマベース寸法 / 2) - dひも幅)
        Else
            band_pos = Unit270 * (_dコマベース寸法 / 2)
        End If

        '下側面:上から下へ
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            Dim band As clsImageItem = _imageList側面ひも.GetRowItem(enumひも種.i_側面, i)
            band.m_rひも位置.p右上 = _左上 + band_pos + length0
            band.m_rひも位置.p左下 = _左上 + band_pos + band270
            '
            For j = 1 To p_i縦ひもの本数 '_i横のコマ数
                addコマ(_左上 + Unit0 * (j - 1) * _dコマベース寸法, band.m_row縦横展開, getBand(enumひも種.i_縦, j), dひも幅, isKnotLeft)
                addコマ(_左下_上側面 + Unit0 * (j - 1) * _dコマベース寸法, band.m_row縦横展開, getBand(enumひも種.i_縦, j), dひも幅, isKnotLeft)
            Next
            '
            _左上 = _左上 + (Unit270 * _dコマベース寸法)
            _左下_上側面 = _左下_上側面 + (Unit90 * _dコマベース寸法)
        Next
        Return True
    End Function

    Function imageList描画要素(ByVal basecross As Boolean, ByVal sideline As Boolean) As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList


        Dim d側面の高さ As Double = _dコマベース寸法 * (_i高さのコマ数 + _i折り返しコマ数)
        Dim d折り返し高さ As Double = _dコマベース寸法 * _i折り返しコマ数
        Dim deltaコマ右 As S差分 = Unit0 * _dコマベース寸法
        Dim deltaコマ下 As S差分 = Unit270 * _dコマベース寸法

        Dim a底 As S四隅
        a底.p左上 = toPoint(-_i横のコマ数 / 2, _i縦のコマ数 / 2)
        a底.p右上 = toPoint(_i横のコマ数 / 2, _i縦のコマ数 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        Dim delta As S差分
        Dim line As S線分

        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底
        For i As Integer = 1 To _i横のコマ数
            If i < _i横のコマ数 AndAlso basecross Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
            For j As Integer = 1 To _i縦のコマ数
                If i = 1 AndAlso basecross Then
                    line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                    line += deltaコマ下 * j
                    item.m_lineList.Add(line)
                End If
            Next
        Next
        itemlist.AddItem(item)

        '折り返し線
        Dim itemFolding As clsImageItem = Nothing
        If 0 < d折り返し高さ Then
            itemFolding = New clsImageItem(clsImageItem.ImageTypeEnum._折り返し線, 1)
        End If

        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.p左下 = a底.p左上
        item.m_a四隅.p右下 = a底.p右上
        delta = Unit90 * d側面の高さ
        item.m_a四隅.p左上 = item.m_a四隅.p左下 + delta
        item.m_a四隅.p右上 = item.m_a四隅.p右下 + delta
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
            line += Unit270 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数 - 1
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                line += deltaコマ下 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
            line += Unit90 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '下の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅.p左上 = a底.p左下
        item.m_a四隅.p右上 = a底.p右下
        delta = Unit270 * d側面の高さ
        item.m_a四隅.p左下 = item.m_a四隅.p左上 + delta
        item.m_a四隅.p右下 = item.m_a四隅.p右上 + delta
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下)
            line += Unit90 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                line += deltaコマ下 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下)
            line += Unit270 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '左の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅.p右上 = a底.p左上
        item.m_a四隅.p右下 = a底.p左下
        delta = Unit180 * d側面の高さ
        item.m_a四隅.p左上 = item.m_a四隅.p右上 + delta
        item.m_a四隅.p左下 = item.m_a四隅.p右下 + delta
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
            line += Unit0 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
            line += Unit180 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p左上 = a底.p右上
        item.m_a四隅.p左下 = a底.p右下
        delta = Unit0 * d側面の高さ
        item.m_a四隅.p右上 = item.m_a四隅.p左上 + delta
        item.m_a四隅.p右下 = item.m_a四隅.p左下 + delta
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下)
            line += Unit180 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下)
            line += Unit0 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)


        If itemFolding IsNot Nothing Then
            itemlist.AddItem(itemFolding)
        End If

        Return itemlist
    End Function

    Function imageList開始位置(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal outp As clsOutput) As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim line As clsImageItem.S線分

        '要尺の折り位置
        Dim foldingLen As Double = c_foldingRatio * _dコマの要尺

        '要尺
        If True Then
            Dim p要尺位置 As S実座標 = toPoint(-(_i横のコマ数 / 2) - (_i高さのコマ数 + _i折り返しコマ数), (_i縦のコマ数 / 2) + (_i高さのコマ数 + _i折り返しコマ数))
            If _i高さのコマ数 + _i折り返しコマ数 = 0 Then
                p要尺位置 = p要尺位置 + Unit90 * (_dコマベース寸法)
            End If
            If (_i高さのコマ数 + _i折り返しコマ数 - 1) * _dコマベース寸法 < _dコマベース要尺 Then
                p要尺位置.X = -(1 + _i横のコマ数 / 2) * _dコマベース寸法 - (_dコマベース要尺 / 2)
            Else
                p要尺位置.X = p要尺位置.X + (_dコマベース要尺 / 2)
            End If
            item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 9)
            Dim delta As S差分 = Unit270 * dひも幅
            item.m_a四隅.p左上 = p要尺位置 + Unit180 * (_dコマベース要尺 / 2) + Unit90 * (dひも幅 / 2)
            item.m_a四隅.p右上 = p要尺位置 + Unit0 * (_dコマベース要尺 / 2) + Unit90 * (dひも幅 / 2)
            item.m_a四隅.p左下 = item.m_a四隅.p左上 + delta
            item.m_a四隅.p右下 = item.m_a四隅.p右上 + delta
            Dim p As S実座標 = p要尺位置
            If 0 < _dコマ間のすき間 Then
                p = item.m_a四隅.p左上 + Unit0 * (_dコマ間のすき間 / 2)
                line = New S線分(p, p + delta)
                item.m_lineList.Add(line)
                p = item.m_a四隅.p右上 + Unit180 * (_dコマ間のすき間 / 2)
                line = New S線分(p, p + delta)
                item.m_lineList.Add(line)
            End If
            '中心から各折り位置まで
            Dim len_1 As Double = (_dコマの要尺 - foldingLen * 2) / 2 - (c_tiltRatio * dひも幅 / 2)
            Dim len_2 As Double = (_dコマの要尺 - foldingLen * 2) / 2 + (c_tiltRatio * dひも幅 / 2)
            If isKnotLeft Then
                Dim tmp As Double = len_1
                len_1 = len_2
                len_2 = tmp
            End If
            line = New S線分(New S実座標(p要尺位置.X + len_1, p要尺位置.Y + dひも幅 / 2), New S実座標(p要尺位置.X + len_2, p要尺位置.Y - dひも幅 / 2))
            item.m_lineList.Add(line)
            line = New S線分(New S実座標(p要尺位置.X - len_1, p要尺位置.Y - dひも幅 / 2), New S実座標(p要尺位置.X - len_2, p要尺位置.Y + dひも幅 / 2))
            item.m_lineList.Add(line)
            itemlist.AddItem(item)
        End If

        '開始位置の情報をセット
        Dim startInfo As CStartInfo = setStartInfo()
        If startInfo Is Nothing Then
            '設定されていなければ
            Return itemlist 'ここまでの結果
        End If

        '底の開始位置のマーク
        If True Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)
            Dim pコマ左上 As S実座標 = toPoint(-(_i横のコマ数 / 2) + (startInfo.i左から何番目 - 1), (_i縦のコマ数 / 2) - (startInfo.i上から何番目 - 1))
            Dim pコマ右下 As S実座標 = pコマ左上 + Unit315 * _dコマベース寸法

            line = New clsImageItem.S線分(pコマ左上, pコマ左上 + Unit0 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ左上, pコマ左上 + Unit270 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ右下, pコマ右下 + Unit90 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ右下, pコマ右下 + Unit180 * _dコマベース寸法)
            item.m_lineList.Add(line)

            itemlist.AddItem(item)
        End If


        '開始位置のコマの転記
        Dim dバンド長 As Double = _dコマベース要尺
        Dim pコマ位置 As S実座標 = toPoint(0, -_i縦のコマ数 / 2 - (_i高さのコマ数 + _i折り返しコマ数) - 1) + Unit270 * dバンド長
        item = New clsImageItem(pコマ位置, startInfo.row横展開, startInfo.row縦展開,
            dひも幅, _dコマの寸法, _dコマ間のすき間, isKnotLeft, True)
        itemlist.AddItem(item)

        'コマに続くバンド
        If True Then
            Dim item1 As New clsImageItem(startInfo.row横展開, True) '右
            Dim item2 As New clsImageItem(startInfo.row横展開, True) '左
            item1.m_dひも幅 = dひも幅
            item2.m_dひも幅 = dひも幅
            If isKnotLeft Then
                item1.m_rひも位置.p左下 = pコマ位置 + Unit0 * dひも幅
                item2.m_rひも位置.p右上 = pコマ位置 + Unit180 * dひも幅
            Else
                item1.m_rひも位置.p左下 = pコマ位置 + Unit315 * dひも幅
                item2.m_rひも位置.p右上 = pコマ位置 + Unit135 * dひも幅
            End If
            item1.m_rひも位置.p右上 = New S実座標(item1.m_rひも位置.p左下.X + dバンド長, item1.m_rひも位置.p左下.Y + dひも幅)
            item2.m_rひも位置.p左下 = New S実座標(item2.m_rひも位置.p右上.X - dバンド長, item2.m_rひも位置.p右上.Y - dひも幅)
            itemlist.AddItem(item1)
            itemlist.AddItem(item2)

            Dim item3 As New clsImageItem(startInfo.row縦展開, True) '上
            Dim item4 As New clsImageItem(startInfo.row縦展開, True) '下
            item3.m_dひも幅 = dひも幅
            item4.m_dひも幅 = dひも幅
            If isKnotLeft Then
                item3.m_rひも位置.p左下 = pコマ位置 + Unit135 * dひも幅
                item4.m_rひも位置.p右上 = pコマ位置 + Unit315 * dひも幅
            Else
                item3.m_rひも位置.p左下 = pコマ位置 + Unit90 * dひも幅
                item4.m_rひも位置.p右上 = pコマ位置 + Unit270 * dひも幅
            End If
            item3.m_rひも位置.p右上 = New S実座標(item3.m_rひも位置.p左下.X + dひも幅, item3.m_rひも位置.p左下.Y + dバンド長)
            item4.m_rひも位置.p左下 = New S実座標(item4.m_rひも位置.p右上.X - dひも幅, item4.m_rひも位置.p右上.Y - dバンド長)
            itemlist.AddItem(item3)
            itemlist.AddItem(item4)
        End If


        '「開始位置」
        Dim p開始位置 As S実座標 = pコマ位置 + Unit135 * (dバンド長) + Unit180 * (dバンド長)
        item = New clsImageItem(p開始位置, {text開始位置()}, dひも幅, 1)
        itemlist.AddItem(item)

        '左から×番目,上から×番目
        Dim pos1 As String = text左から() & startInfo.i左から何番目.ToString & text番目のコマ()
        Dim pos2 As String = text上から() & startInfo.i上から何番目.ToString & text番目のコマ()
        item = New clsImageItem(p開始位置 + Unit315 * (dひも幅 * 2), {pos1, pos2}, dひも幅 * 2 / 3, 2)
        itemlist.AddItem(item)


        '各方向の説明文字列 
        If True Then
            Dim p文字(3) As S実座標
            '上のひも
            p文字(0).X = pコマ位置.X + dひも幅 * 2
            p文字(0).Y = pコマ位置.Y + dバンド長
            '下のひも
            p文字(1).X = pコマ位置.X + dひも幅 * 2
            p文字(1).Y = pコマ位置.Y - 2 * dバンド長 / 3
            '左のひも
            p文字(2).X = pコマ位置.X - dバンド長 - dひも幅 * 15 '想定文字数分
            p文字(2).Y = pコマ位置.Y - dひも幅 * 1.5
            '右のひも
            p文字(3).X = pコマ位置.X + dバンド長 + _dコマの寸法
            p文字(3).Y = pコマ位置.Y + dひも幅

            startInfo.setMyValue(True) 'マイひも長係数を乗算する
            For i As Integer = 0 To 3
                With startInfo
                    '<コマの{0}>
                    Dim str1 As String = String.Format(My.Resources.CalcOutKnotOf, .getSideString(i))
                    str1 += outp.outLengthTextWithUnit(.getBandLength(i))
                    str1 += "  "
                    str1 += .getDiffString(i, outp)

                    '加算計 {0} (縁の始末:{1} ひも長加算:{2} {3}加算:{4})
                    Dim str2 As String = String.Format(My.Resources.CalcOutAddLen,
                        outp.outLengthTextWithUnit(.getAddSum(i)), outp.outLengthText(.getAddHem()), outp.outLengthText(.getAddVert()), .getSideString(i), outp.outLengthText(.getAddEach(i)))

                    '折り位置から
                    Dim str3 As String = String.Format(My.Resources.CalcOutFromFolding)
                    str3 += outp.outLengthTextWithUnit(.getFoldingLength(i))
                    str3 += "  "
                    str3 += .getDiffFoldingString(i, outp)
                    '
                    item = New clsImageItem(p文字(i), {str1, str2, str3}, dひも幅 * 2 / 3, i + 3)
                    itemlist.AddItem(item)
                End With
            Next
        End If

        'コマ位置の中央線
        startInfo.setMyValue(False) 'マイひも長係数を乗算しない
        If True Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 2)
            Dim d1本幅 As Double = dひも幅 / _I基本のひも幅
            If Abs(startInfo.getDiff(0)) < dバンド長 * 2 + _dコマベース要尺 + d1本幅 Then
                '上下中央の横線
                Dim pp As New S実座標(pコマ位置.X - dひも幅 * 1.5, pコマ位置.Y)
                line = New S線分(pp, pp + Unit0 * dひも幅 * 3)
                If Abs(startInfo.getDiff(0)) < d1本幅 Then
                    '1本幅以下
                    item.m_lineList.Add(line)
                ElseIf (_dコマの要尺 / 2 - d1本幅) <= startInfo.getDiff(0) Then
                    '要尺以上、上が長いので中央点は上にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(0) - _dコマベース要尺) / 2
                    line = line + Unit90 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                ElseIf startInfo.getDiff(0) <= -(_dコマの要尺 / 2 - d1本幅) Then
                    '要尺以上、下が長いので中央点は下にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(1) - _dコマベース要尺) / 2
                    line = line + Unit270 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                Else
                    '描けません
                End If
            End If
            If Abs(startInfo.getDiff(2)) < dバンド長 * 2 + _dコマベース要尺 + d1本幅 Then
                '左右中央の縦線
                Dim pp As New S実座標(pコマ位置.X, pコマ位置.Y - dひも幅 * 1.5)
                line = New S線分(pp, pp + Unit90 * dひも幅 * 3)
                If Abs(startInfo.getDiff(2)) < d1本幅 Then
                    '1本幅以下
                    item.m_lineList.Add(line)
                ElseIf (_dコマの要尺 / 2 - d1本幅) <= startInfo.getDiff(2) Then
                    '要尺以上、左が長いので中央点は左にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(2) - _dコマベース要尺) / 2
                    line = line + Unit180 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                ElseIf startInfo.getDiff(2) <= -(_dコマの要尺 / 2 - d1本幅) Then
                    '要尺以上、右が長いので中央点は右にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(3) - _dコマベース要尺) / 2
                    line = line + Unit0 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                Else
                    '描けません
                End If
            End If
            If 0 < item.m_lineList.Count Then
                itemlist.AddItem(item)
            End If
        End If


        Return itemlist
    End Function

    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '念のため
        _imageList側面ひも = Nothing
        _ImageListコマ = Nothing
        _ImageList描画要素 = Nothing
        _ImageList開始位置 = Nothing

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

        '記号表示の位置をセット
        Dim isKnotLeft As Boolean = False
        Select Case _Data.p_row底_縦横.Value("f_iコマ上側の縦ひも")
            Case enumコマ上側の縦ひも.i_どちらでも
                isKnotLeft = My.Settings.IsKnotLeft
            Case enumコマ上側の縦ひも.i_左側
                isKnotLeft = True
            Case enumコマ上側の縦ひも.i_右側
                isKnotLeft = False
        End Select
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)

        '基本のひも幅と基本色
        imgData.setBasics(_dコマの寸法, _Data.p_row目標寸法.Value("f_s基本色"))

#If 1 Then
        '通常描画
        imageList側面記号(dひも幅, isKnotLeft, False)
        imageList横記号(dひも幅, isKnotLeft, False)
        imageList縦記号(dひも幅, isKnotLeft, False)
        _ImageList開始位置 = imageList開始位置(dひも幅, isKnotLeft, outp)
        _ImageList描画要素 = imageList描画要素(False, False)
#Else
        'バンド描画
        imageList側面記号(dひも幅, isKnotLeft, True)
        imageList横記号(dひも幅, isKnotLeft, True)
        imageList縦記号(dひも幅, isKnotLeft, True)
        _ImageList開始位置 = imageList開始位置(dひも幅, isKnotLeft, outp)
        _ImageList描画要素 = imageList描画要素(True, True)
#End If

        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing
        imgData.MoveList(_imageList側面ひも)
        _imageList側面ひも = Nothing
#If 1 Then
        imgData.MoveList(_ImageListコマ)
#End If
        _ImageListコマ = Nothing
        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing
        imgData.MoveList(_ImageList開始位置)
        _ImageList開始位置 = Nothing

        '付属品
        AddPartsImage(imgData, _Data.p_tbl追加品, getAspectRatio())

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function

End Class
