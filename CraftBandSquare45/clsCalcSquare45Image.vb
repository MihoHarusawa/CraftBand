Imports System.Diagnostics.Eventing.Reader
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsUpDown
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstMasterTables

Partial Public Class clsCalcSquare45

    '1～p_iひもの本数
    Dim _BandPositions(cExpandCount - 1) As CBandPositionList


    '
    Dim _p中央 As S実座標
    Dim _a底 As S四隅

    '計算結果の保持
    Private Property _d四角ベース_横計 As Double '横の目数,縦ひも幅計,すき間
    Private Property _d四角ベース_縦計 As Double '縦の目数,横ひも幅計,すき間
    Private Property _d四角ベース_高さ計 As Double '高さの目数,編みひも幅計,すき間　(縁は含まない)
    '横ひも: _tbl縦横展開_横ひも
    Private Property _b横ひも本幅変更 As Boolean
    '縦ひも: _tbl縦横展開_縦ひも
    Private Property _b縦ひも本幅変更 As Boolean


    ReadOnly Property p_i先の三角形の本幅の差 As Integer
        Get
            Return 1
        End Get
    End Property
    ReadOnly Property p_i四辺形の本幅の差 As Integer
        Get
            Return 0
        End Get
    End Property
    ReadOnly Property p_i後の三角形の本幅の差 As Integer
        Get
            Return -1
        End Get
    End Property


    Private Sub imageDataClear()


        _b横ひも本幅変更 = False
        _b縦ひも本幅変更 = False


        _BandPositions(enumExpandDirection._Yoko).Clear()
        _BandPositions(enumExpandDirection._Tate).Clear()

    End Sub




    '位置のカテゴリー
    Enum enumPositionType
        _Inc = 0    '角から中へ、増えていく
        _Zero = 1 '中、(平行)四辺形
        _Dec = 2 '中から角へ、減っていく
        '
        _Sum = 3
    End Enum
    Const cPositionTypeCount As Integer = 4



    '四角数,入力値(ひも長加算,ひも幅)がFixした状態で、長さを計算する
    Private Function calc_位置と長さ計算(ByVal is位置計算 As Boolean) As Boolean
        Dim ret As Boolean = True

        If is位置計算 Then
            'f_i何本幅の設定状態
            _d四角ベース_縦計 = recalc_ひも展開(_tbl縦横展開(enumExpandDirection._Yoko), enumひも種.i_横, _b横ひも本幅変更)

            'f_i何本幅の設定状態
            _d四角ベース_横計 = recalc_ひも展開(_tbl縦横展開(enumExpandDirection._Tate), enumひも種.i_縦, _b縦ひも本幅変更)

            setBandPositions縦ひも()
            setBandPositions横ひも()
        End If

        '長さを反映
        ret = ret And adjust_ひも(_tbl縦横展開(enumExpandDirection._Yoko))
        ret = ret And adjust_ひも(_tbl縦横展開(enumExpandDirection._Tate))

        Return ret
    End Function





    '位置計算用
    Private Class CBandPosition
        Dim idx As Integer

        Public m_a四隅 As S四隅
        Public m_row縦横展開 As tbl縦横展開Row

        Sub New(ByVal i As Integer)
            idx = i
        End Sub

        Function getNewImageItem(ByVal dir As enumExpandDirection) As clsImageItem
            If m_row縦横展開 Is Nothing Then
                Return Nothing
            End If
            Dim band As New clsImageItem(m_row縦横展開)
            band.m_a四隅 = m_a四隅

            Dim p中央 As S実座標 = band.m_a四隅.p中央

            With band.m_row縦横展開
                band.m_dひも幅 = g_clsSelectBasics.p_d指定本幅(.f_i何本幅)
                If dir = enumExpandDirection._Yoko Then

                    band.m_rひも位置.y最上 = p中央.Y + band.m_dひも幅 / 2
                    band.m_rひも位置.y最下 = p中央.Y - band.m_dひも幅 / 2
                    band.m_rひも位置.x最右 = p中央.X + .f_d出力ひも長 / 2
                    band.m_rひも位置.x最左 = p中央.X - .f_d出力ひも長 / 2

                ElseIf dir = enumExpandDirection._Tate Then

                    band.m_rひも位置.x最右 = p中央.X + band.m_dひも幅 / 2
                    band.m_rひも位置.x最左 = p中央.X - band.m_dひも幅 / 2
                    band.m_rひも位置.y最上 = p中央.Y + .f_d出力ひも長 / 2
                    band.m_rひも位置.y最下 = p中央.Y - .f_d出力ひも長 / 2

                End If
            End With

            Return band
        End Function


        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("idx={0}", idx).Append(vbTab)
            sb.AppendFormat("四隅:({0})", m_a四隅).Append(vbTab)
            'sb.AppendFormat("ひも位置:({0})", m_rひも位置).Append(vbTab)
            'sb.AppendFormat("ひも幅e:({0})", m_dひも幅).Append(vbTab)
            If m_row縦横展開 IsNot Nothing Then
                sb.AppendFormat("row縦横展開:({0},{1},{2})", m_row縦横展開.f_iひも種, m_row縦横展開.f_iひも番号, m_row縦横展開.f_i位置番号)
            Else
                sb.Append("No row縦横展開")
            End If
            Return sb.ToString
        End Function
    End Class

    '展開テーブルの位置計算用
    Private Class CBandPositionList

        Dim _Direction As enumExpandDirection

        'idx=0は使わない。1～サイズで使用する
        Dim _List As New List(Of CBandPosition)

        Dim _補強ひも(2) As tbl縦横展開Row '1,2を使う。0は使わない

        Dim _幅の計(cExpandCount - 1, cPositionTypeCount - 1) As Double
        Dim _本幅の計(cExpandCount - 1, cPositionTypeCount - 1) As Integer

        ReadOnly Property At(ByVal idx As Integer) As CBandPosition
            Get
                Return _List(idx)
            End Get
        End Property


        Sub New(ByVal dir As enumExpandDirection)
            _Direction = dir
        End Sub

        Sub Clear()
            SetSize(0)
        End Sub

        '指定サイズにする
        Private Function SetSize(ByVal size As Integer) As Boolean
            _補強ひも(1) = Nothing
            _補強ひも(2) = Nothing

            If (size + 1) < _List.Count Then
                '多い
                Do While _List.Count > (size + 1)
                    _List.RemoveAt(_List.Count - 1)
                Loop
            ElseIf _List.Count < (size + 1) Then
                '少ない
                Do While _List.Count < (size + 1)
                    _List.Add(New CBandPosition(_List.Count))
                Loop
            End If
            Return True
        End Function

        'テーブルのレコードをセットする
        Function SetTable(ByVal table As tbl縦横展開DataTable, ByVal bandcount As Integer) As Boolean
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                SetSize(0)
                Return False
            End If

            Dim ret As Boolean = True
            Dim setcount As Integer = 0
            SetSize(bandcount)
            For Each row As tbl縦横展開Row In table
                Dim idx As Integer = row.f_iひも番号

                If is補強ひも(row) Then
                    If idx = 1 OrElse idx = 2 Then
                        _補強ひも(idx) = row
                    Else
                        ret = False
                    End If
                Else
                    '処理のひも
                    If 1 <= idx AndAlso idx <= bandcount Then
                        _List(idx).m_row縦横展開 = row
                        setcount += 1
                    Else
                        ret = False
                    End If
                End If
            Next
            Return ret And (setcount = bandcount)
        End Function



        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            For Each band As CBandPosition In _List
                sb.AppendLine(band.ToString)
            Next
            Return sb.ToString
        End Function

    End Class



    '本幅変更と幅の合計を返す共通関数
    Private Function recalc_ひも展開(ByVal table As tbl縦横展開DataTable, ByVal filt As enumひも種, ByRef isChange As Boolean) As Double
        Dim dSum幅 As Double = 0
        Dim iMax何本幅 As Integer = 0
        Dim iMin何本幅 As Integer = 0
        isChange = False
        If table Is Nothing OrElse table.Rows.Count = 0 Then
            Return dSum幅
        End If

        Dim obj As Object = table.Compute("SUM(f_d幅)", Nothing)
        If Not IsDBNull(obj) AndAlso 0 < obj Then
            dSum幅 = obj
        End If

        Dim itype As Integer = filt
        Dim objMax As Object = table.Compute("MAX(f_i何本幅)", String.Format("f_iひも種 = {0}", itype))
        If Not IsDBNull(objMax) AndAlso 0 < objMax Then
            iMax何本幅 = objMax
        End If

        Dim objMin As Object = table.Compute("MIN(f_i何本幅)", String.Format("f_iひも種 = {0}", itype))
        If Not IsDBNull(objMin) AndAlso 0 < objMin Then
            iMin何本幅 = objMin
        End If

        isChange = iMax何本幅 <> _I基本のひも幅 OrElse iMin何本幅 <> _I基本のひも幅

        Return dSum幅
    End Function






    Private Function toPoint(ByVal x As Double, ByVal y As Double) As S実座標
        Return New S実座標(_d四角の一辺 * x, _d四角の一辺 * y)
    End Function

    'f_d長さ  :底の中、ひも長方向の中心線の長さ
    'f_dひも長 :
    'f_d出力ひも長 :ひも長に係数をかけて、+2*(ひも長加算(一端)+縁の垂直ひも長)+ひも長加算


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


        Dim _ImageList横ひも As New clsImageItemList
        Dim _ImageList縦ひも As New clsImageItemList
        Try
            For idx As Integer = 1 To p_iひもの本数
                Dim bandposition As CBandPosition = _BandPositions(enumExpandDirection._Yoko).At(idx)
                Dim item As clsImageItem = bandposition.getNewImageItem(enumExpandDirection._Yoko)
                _ImageList横ひも.AddItem(item)
#If 1 Then
                item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, idx)
                item.m_a四隅 = bandposition.m_a四隅
                _ImageList横ひも.AddItem(item)
#End If
            Next
            For idx As Integer = 1 To p_iひもの本数
                Dim bandposition As CBandPosition = _BandPositions(enumExpandDirection._Tate).At(idx)
                Dim item As clsImageItem = bandposition.getNewImageItem(enumExpandDirection._Tate)
                _ImageList縦ひも.AddItem(item)
#If 1 Then
                item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, idx)
                item.m_a四隅 = bandposition.m_a四隅
                _ImageList縦ひも.AddItem(item)
#End If
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

    'Private Function bandPositions長さ計算(ByVal _ひもリスト As CBandPositionList, ByVal _ひも種 As enumひも種) As Boolean
    '    If _ひもリスト Is Nothing Then
    '        Return False
    '    End If

    '    For Each band As CBandPosition In _ひもリスト
    '        If band.m_row縦横展開 Is Nothing OrElse band.m_row縦横展開.f_iひも種 <> _ひも種 Then
    '            Continue For
    '        End If

    '        Dim p中央 As S実座標 = band.m_a四隅.p中央
    '        Dim d加算分 As Double = (_dひも長加算 + _d縁の垂直ひも長) * 2

    '        With band.m_row縦横展開
    '            band.m_dひも幅 = g_clsSelectBasics.p_d指定本幅(.f_i何本幅)
    '            If _ひも種 = enumひも種.i_横 Then
    '                .f_dひも長 = band.m_a四隅.x最右 - band.m_a四隅.x最左

    '                '.f_d出力ひも長 = .f_dひも長 * _dひも長係数 + .f_dひも長加算 + d加算分
    '                Dim d出力ひも長 As Double = .f_dひも長 * _dひも長係数 + .f_dひも長加算 + d加算分

    '                band.m_rひも位置.y最上 = p中央.Y + band.m_dひも幅 / 2
    '                band.m_rひも位置.y最下 = p中央.Y - band.m_dひも幅 / 2
    '                band.m_rひも位置.x最右 = p中央.X + d出力ひも長 / 2
    '                band.m_rひも位置.x最左 = p中央.X - d出力ひも長 / 2

    '            ElseIf _ひも種 = enumひも種.i_縦 Then
    '                .f_dひも長 = band.m_a四隅.y最上 - band.m_a四隅.y最下
    '                '.f_d出力ひも長 = .f_dひも長 * _dひも長係数 + .f_dひも長加算 + d加算分
    '                Dim d出力ひも長 As Double = .f_dひも長 * _dひも長係数 + .f_dひも長加算 + d加算分

    '                band.m_rひも位置.x最右 = p中央.X + band.m_dひも幅 / 2
    '                band.m_rひも位置.x最左 = p中央.X - band.m_dひも幅 / 2
    '                band.m_rひも位置.y最上 = p中央.Y + d出力ひも長 / 2
    '                band.m_rひも位置.y最下 = p中央.Y - d出力ひも長 / 2

    '            End If
    '        End With
    '    Next
    '    Return True
    'End Function


    Private Function setBandPositions横ひも() As Boolean
        Dim _BandPositions横ひも As CBandPositionList = _BandPositions(enumExpandDirection._Yoko)

        'Dim delta225 As S差分 = Unit225 * _d四角の一辺 '／
        'Dim delta315 As S差分 = Unit315 * _d四角の一辺 '＼
        Dim delta As S差分

        Dim updowncount As Integer
        Dim samecount As Integer
        If _i縦の四角数 <= _i横の四角数 Then
            updowncount = _i縦の四角数
            samecount = _i横の四角数 - _i縦の四角数
            delta = Unit225 '／
        Else
            updowncount = _i横の四角数
            samecount = _i縦の四角数 - _i横の四角数
            delta = Unit315 '＼
        End If


        Dim _左上 As S実座標 = toPoint(-2 * _d高さの四角数 + (_i横の四角数 - _i縦の四角数) / 2, (_i横の四角数 + _i縦の四角数) / 2)
        Dim _右上 As S実座標 = toPoint(2 * _d高さの四角数 + (_i横の四角数 - _i縦の四角数) / 2, (_i横の四角数 + _i縦の四角数) / 2)

        '上から下へ
        Dim idx As Integer = 1
        For i As Integer = 0 To updowncount - 1
            Dim band As CBandPosition = _BandPositions横ひも.At(idx)
            'band.m_row縦横展開.f_i位置番号 = -updowncount + i
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * (i * 2 + 1)

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p右上 = _右上
            '
            _左上 = _左上 + Unit225 * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間) 'delta225 '／
            _右上 = _右上 + Unit315 * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間) ' '＼
            band.m_a四隅.p左下 = _左上
            band.m_a四隅.p右下 = _右上
            band.m_row縦横展開.f_dひも長 = band.m_a四隅.x最右 - band.m_a四隅.x最左

            idx += 1
        Next
        For i As Integer = 0 To samecount - 1
            Dim band As CBandPosition = _BandPositions横ひも.At(idx)
            ' band.m_row縦横展開.f_i位置番号 = 0
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * (updowncount * 2)

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p右上 = _右上
            '
            _左上 = _左上 + delta * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間)
            _右上 = _右上 + delta * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間)
            band.m_a四隅.p左下 = _左上
            band.m_a四隅.p右下 = _右上
            band.m_row縦横展開.f_dひも長 = band.m_a四隅.x最右 - band.m_a四隅.x最左

            idx += 1
        Next
        For i As Integer = 0 To updowncount - 1
            Dim band As CBandPosition = _BandPositions横ひも.At(idx)
            'band.m_row縦横展開.f_i位置番号 = i + 1
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * ((updowncount * 2) - (i * 2 + 1))

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p右上 = _右上
            '
            _左上 = _左上 + Unit315 * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間) ' '＼
            _右上 = _右上 + Unit225 * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間) ' '／
            band.m_a四隅.p左下 = _左上
            band.m_a四隅.p右下 = _右上
            band.m_row縦横展開.f_dひも長 = band.m_a四隅.x最右 - band.m_a四隅.x最左

            idx += 1
        Next

        Return True 'bandPositions長さ計算(_BandPositions横ひも, enumひも種.i_横)
    End Function


    Private Function setBandPositions縦ひも() As Boolean
        Dim _BandPositions縦ひも As CBandPositionList = _BandPositions(enumExpandDirection._Tate)

        'Dim delta45 As S差分 = Unit45 * _d四角の一辺 '／
        'Dim delta315 As S差分 = Unit315 * _d四角の一辺 '＼
        Dim delta As S差分

        Dim updowncount As Integer
        Dim samecount As Integer
        If _i縦の四角数 <= _i横の四角数 Then
            updowncount = _i縦の四角数
            samecount = _i横の四角数 - _i縦の四角数
            delta = Unit45 '／
        Else
            updowncount = _i横の四角数
            samecount = _i縦の四角数 - _i横の四角数
            delta = Unit315 '＼

        End If


        Dim _左上 As S実座標 = toPoint(-(_i横の四角数 + _i縦の四角数) / 2, 2 * _d高さの四角数 - (_i横の四角数 - _i縦の四角数) / 2)
        Dim _左下 As S実座標 = toPoint(-(_i横の四角数 + _i縦の四角数) / 2, -2 * _d高さの四角数 - (_i横の四角数 - _i縦の四角数) / 2)

        '左から右へ
        Dim idx As Integer = 1
        For i As Integer = 0 To updowncount - 1
            Dim band As CBandPosition = _BandPositions縦ひも.At(idx)
            'band.m_row縦横展開.f_i位置番号 = -updowncount + i
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * (i * 2 + 1)

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p左下 = _左下
            '
            _左上 = _左上 + Unit45 * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間)  '／
            _左下 = _左下 + Unit315 * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間)  '＼
            band.m_a四隅.p右上 = _左上
            band.m_a四隅.p右下 = _左下
            band.m_row縦横展開.f_dひも長 = band.m_a四隅.y最上 - band.m_a四隅.y最下

            idx += 1
        Next
        For i As Integer = 0 To samecount - 1
            Dim band As CBandPosition = _BandPositions縦ひも.At(idx)
            'band.m_row縦横展開.f_i位置番号 = 0
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * (updowncount * 2)

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p左下 = _左下
            '
            _左上 = _左上 + delta * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間)
            _左下 = _左下 + delta * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間)
            band.m_a四隅.p右上 = _左上
            band.m_a四隅.p右下 = _左下
            band.m_row縦横展開.f_dひも長 = band.m_a四隅.y最上 - band.m_a四隅.y最下

            idx += 1
        Next
        For i As Integer = 0 To updowncount - 1
            Dim band As CBandPosition = _BandPositions縦ひも.At(idx)
            'band.m_row縦横展開.f_i位置番号 = i + 1
            band.m_row縦横展開.f_d長さ = _d四角の一辺 * ((updowncount * 2) - (i * 2 + 1))

            band.m_a四隅.p左上 = _左上
            band.m_a四隅.p左下 = _左下
            '
            _左上 = _左上 + Unit315 * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間)  '＼
            _左下 = _左下 + Unit45 * (g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅) + _dひも間のすき間)  '／
            band.m_a四隅.p右上 = _左上
            band.m_a四隅.p右下 = _左下
            band.m_row縦横展開.f_dひも長 = band.m_a四隅.y最上 - band.m_a四隅.y最下

            idx += 1
        Next

        Return True 'bandPositions長さ計算(_BandPositions縦ひも, enumひも種.i_縦)
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
