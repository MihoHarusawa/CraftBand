Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsImageItem.CBand
Imports CraftBand.mdlUnit
Imports CraftBand.Tables.dstDataTables


Partial Public Class clsCalcMesh

    'プレビュー時に生成
    Dim _imageList側面編みかた As clsImageItemList    '側面のレコードを含む
    Dim _ImageList描画要素 As clsImageItemList '底と側面

    Dim _dPortionOver As Double = New Length(1, "cm").Value '省略部分の長さ



    Dim _bandPositionListYoko As New CBandPositionList(DirectionIndex._yoko)
    Dim _bandPositionListTate As New CBandPositionList(DirectionIndex._tate)


#Region "底ひものセット"
    '                              終点
    '     ┌──────┐        ┌┬┐
    '始点 ├── → ──┤終点    │││
    '     └──────┘        │↑│並び方向→
    '      並び方向↓             │││
    '                             └┴┘
    '                              始点
    Enum DirectionIndex
        _yoko = 0    '横
        _tate    '縦
    End Enum

    'バンドの方向                         横ひも→ 　縦ひも↑
    Shared cDeltaBandDirection() As S差分 = {Unit0, Unit90}
    'バンドの並び方向(-90)　              横ひも↓　縦ひも→
    Shared cDeltaAxisDirection() As S差分 = {Unit270, Unit0}


    '各バンド
    Friend Class CBandPosition
        Friend _parent As CBandPositionList = Nothing   '方向情報
        Friend m_Index As Integer = -1 '1～要素数

        Friend m_row縦横展開 As tbl縦横展開Row

        Friend m_p中心点 As S実座標 '加算ゼロ時, X軸上/Y軸上
        Friend m_p始点 As S実座標
        Friend m_p終点 As S実座標

        Sub New(ByVal row As tbl縦横展開Row)
            m_row縦横展開 = row
        End Sub

        '識別情報
        Friend ReadOnly Property Ident As String
            Get
                Return String.Format("Direction({0}){1}", _parent._DirectionIndex, m_Index)
            End Get
        End Property

        'バンド描画
        Function ToBand(ByVal d基本のひも幅 As Double) As CBand
            If m_row縦横展開 Is Nothing Then
                Return Nothing
            End If

            Dim band = New CBand(m_row縦横展開)

            'バンド描画位置
            Dim dひも幅 As Double = m_row縦横展開.f_dVal1
            Dim p始点 As S実座標 = m_p始点
            If _parent._IsUpRightOnly Then
                If _parent._DirectionIndex = DirectionIndex._yoko Then
                    If p始点.X < _parent._xLimit Then
                        p始点.X = _parent._xLimit
                        band.is始点FT線 = False
                    End If
                ElseIf _parent._DirectionIndex = DirectionIndex._tate Then
                    If p始点.Y < _parent._yLimit Then
                        p始点.Y = _parent._yLimit
                        band.is始点FT線 = False
                    End If
                End If
            End If
            band.SetBand(New S線分(p始点, m_p終点), dひも幅, _parent.DeltaAxisDirection)

            '記号描画位置
            Dim mark As enumMarkPosition = enumMarkPosition._なし
            Dim delta As S差分
            Dim distance As Double = 0
            If _parent._DirectionIndex = DirectionIndex._yoko Then
                '記号を左に
                If Not String.IsNullOrWhiteSpace(m_row縦横展開.f_s記号) Then
                    mark = enumMarkPosition._始点の前
                    delta = New S差分(0, -dひも幅 / 2)
                    distance = d基本のひも幅
                End If
            ElseIf _parent._DirectionIndex = DirectionIndex._tate Then
                '記号を上に
                mark = enumMarkPosition._終点の後
                delta = New S差分(-dひも幅 / 2, 0)
            End If
            band.SetMarkPosition(mark, distance, delta)

            Return band
        End Function


        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0} {1} {2}→{3}", Ident, m_p中心点, m_p始点, m_p終点).Append(vbTab)
            If m_row縦横展開 IsNot Nothing Then
                sb.AppendFormat("row縦横展開:({0},{1},{2}){3}本幅 ", m_row縦横展開.f_i位置番号, m_row縦横展開.f_iひも種, m_row縦横展開.f_iひも番号, m_row縦横展開.f_i何本幅)
            End If
            Return sb.ToString
        End Function

    End Class

    '各方向のセット
    Friend Class CBandPositionList
        Implements IEnumerable(Of CBandPosition)

        'セットに対する定数値
        Friend _DirectionIndex As DirectionIndex
        '位置順のリスト
        Dim _BandList As New List(Of CBandPosition)     '0～要素数-1

        '右上のみ
        Friend _IsUpRightOnly As Boolean = False
        '左限
        Friend _xLimit As Double
        '下限
        Friend _yLimit As Double

        'バンドの方向 横ひも→ 　縦ひも↑
        Friend ReadOnly Property DeltaBandDirection As S差分
            Get
                Return cDeltaBandDirection(_DirectionIndex)
            End Get
        End Property

        '並びの方向　横ひも↓　縦ひも→
        Friend ReadOnly Property DeltaAxisDirection As S差分
            Get
                Return cDeltaAxisDirection(_DirectionIndex)
            End Get
        End Property

        Friend ReadOnly Property Count As Integer
            Get
                Return _BandList.Count
            End Get
        End Property

        Friend Sub Clear(ByVal isUpRightOnly As Boolean, xLimit As Double, yLimit As Double)
            _BandList.Clear()

            _IsUpRightOnly = isUpRightOnly
            _xLimit = xLimit
            _yLimit = yLimit
        End Sub

        Friend Sub Add(ByVal bpos As CBandPosition)
            _BandList.Add(bpos)
            bpos._parent = Me
            bpos.m_Index = _BandList.Count '1～要素数
        End Sub

#If 0 Then
        '位置の逆順　:1～_iひもの本数
        ReadOnly Property RevertIdx(ByVal ax As Integer) As Integer
            Get
                If ax < 1 OrElse _BandList.Count < ax Then
                    Return -1
                End If
                'いずれの角度に対しても、軸方向はひも番号に対して逆
                Return _BandList.Count - ax + 1
            End Get
        End Property

        '指定位置の要素 1～_iひもの本数　で使用
        Friend ReadOnly Property ByIdx(ByVal idx As Integer) As CBandPosition
            Get
                If idx < 1 OrElse _BandList.Count < idx Then
                    Return Nothing
                End If
                Return _BandList(idx - 1)
            End Get
        End Property

        '軸方向　:1～_iひもの本数
        Friend ReadOnly Property ByAxis(ByVal ax As Integer) As CBandPosition
            Get
                Return ByIdx(RevertIdx(ax))
            End Get
        End Property
#End If

        Sub New(ByVal didx As DirectionIndex)
            _DirectionIndex = didx
        End Sub

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("Direction={0} DeltaBandDirection={1} DeltaAxisDirection={2} Count={3} ", _DirectionIndex, DeltaBandDirection, DeltaAxisDirection, Count)

            For Each band As CBandPosition In _BandList
                sb.AppendLine(band.ToString)
            Next
            Return sb.ToString
        End Function

        Public Function GetEnumerator() As IEnumerator(Of CBandPosition) Implements IEnumerable(Of CBandPosition).GetEnumerator
            Return _BandList.GetEnumerator()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function
    End Class

#End Region

    '描画情報
    Private Function setBandPositionList(ByVal bandPositionList As CBandPositionList, ByVal table As tbl縦横展開DataTable, ByVal isUpRightOnly As Boolean) As Boolean
        bandPositionList.Clear(isUpRightOnly, -p_d内側_横 / 2 - _dPortionOver, -p_d内側_縦 / 2 - _dPortionOver)
        If table Is Nothing OrElse table.Rows.Count = 0 Then
            Return False
        End If

        Dim pバンド辺中心 As S実座標
        If bandPositionList._DirectionIndex = DirectionIndex._yoko Then
            '上の辺の中心
            pバンド辺中心 = New S実座標(0, p_d縦横の縦 / 2)
        ElseIf bandPositionList._DirectionIndex = DirectionIndex._tate Then
            '左の辺の中心
            pバンド辺中心 = New S実座標(-p_d縦横の横 / 2, 0)
        Else
            Return False
        End If

        Dim cond As String = String.Format("f_i位置番号 < {0}", cBackPosition)
        Dim rows() As DataRow = table.Select(cond, "f_i位置番号 ASC")
        If rows Is Nothing OrElse rows.Count = 0 Then
            Return False
        End If
        '位置順(並びの方向へ)
        For Each row As tbl縦横展開Row In rows
            Dim bandpos As New CBandPosition(row)

            'f_dVal1 = ひも幅(f_i何本幅) の半分
            bandpos.m_p中心点 = pバンド辺中心 + bandPositionList.DeltaAxisDirection * (row.f_dVal1 / 2)
            'f_dVal2 = 先半分の出力ひも長
            bandpos.m_p始点 = bandpos.m_p中心点 - bandPositionList.DeltaBandDirection * row.f_dVal2
            'f_dVal3 = 後半分の出力ひも長
            bandpos.m_p終点 = bandpos.m_p中心点 + bandPositionList.DeltaBandDirection * row.f_dVal3

            bandPositionList.Add(bandpos)
            '
            pバンド辺中心 = pバンド辺中心 + bandPositionList.DeltaAxisDirection * row.f_d幅
        Next

        Return True
    End Function


    '横ひもリストの描画情報
    Private Function imageList横ひも(ByVal imgList横ひも As clsImageItemList, ByVal table As tbl縦横展開DataTable, ByVal isUpRightOnly As Boolean) As Boolean
        If imgList横ひも Is Nothing Then
            Return False
        End If

        If Not setBandPositionList(_bandPositionListYoko, table, isUpRightOnly) Then
            Return False
        End If

        For Each bandpos As CBandPosition In _bandPositionListYoko
            Dim band As CBand = bandpos.ToBand(_d基本のひも幅)
            If band IsNot Nothing Then
                Dim item As New clsImageItem(band, 10, bandpos.m_Index)
                imgList横ひも.AddItem(item)
            End If
        Next

        Return True
    End Function

    '縦ひもリストの描画情報
    Private Function imageList縦ひも(ByVal imgList縦ひも As clsImageItemList, ByVal table As tbl縦横展開DataTable, ByVal isUpRightOnly As Boolean) As Boolean
        If imgList縦ひも Is Nothing Then
            Return False
        End If

        If Not setBandPositionList(_bandPositionListTate, table, isUpRightOnly) Then
            Return False
        End If

        For Each bandpos As CBandPosition In _bandPositionListTate
            Dim band As CBand = bandpos.ToBand(_d基本のひも幅)
            If band IsNot Nothing Then
                Dim item As New clsImageItem(band, 20, bandpos.m_Index)
                imgList縦ひも.AddItem(item)
            End If
        Next

        Return True
    End Function


    '_imageList側面ひも生成、側面のレコードを含む
    Function imageList側面編みかた(ByVal dひも幅 As Double, ByVal isUpRightOnly As Boolean, ByVal isShowSide As Boolean) As clsImageItemList
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
                If isShowSide Then
                    '--- 上 ---
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

                    '--- 右 ---
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

                If Not isUpRightOnly AndAlso isShowSide Then
                    '--- 下 ---
                    'ImageTypeEnum._編みかた・横
                    item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 3)
                    item.m_a四隅.p左上 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, -dY)
                    item.m_a四隅.p右上 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, -dY)
                    item.m_a四隅.p左下 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, -dY - d高さ)
                    item.m_a四隅.p右下 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, -dY - d高さ)
                    '周の区切り
                    If 1 < i周数 Then
                        For i As Integer = 1 To i周数 - 1
                            Dim p1 As New S実座標(item.m_a四隅.x最左, -d高さ * (i / i周数) - dY)
                            Dim p2 As New S実座標(item.m_a四隅.x最右, -d高さ * (i / i周数) - dY)
                            Dim line As New S線分(p1, p2)
                            item.m_lineList.Add(line)
                        Next
                    End If
                    '文字は指定しない
                    itemlist.AddItem(item)

                    '--- 左 ---
                    'ImageTypeEnum._編みかた・縦
                    item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 4)
                    item.m_a四隅.p右上 = New S実座標(-dX, +p_d外側_縦 * d周長比率対底の周 / 2)
                    item.m_a四隅.p右下 = New S実座標(-dX, -p_d外側_縦 * d周長比率対底の周 / 2)
                    item.m_a四隅.p左上 = New S実座標(-dX - d高さ, +p_d外側_縦 * d周長比率対底の周 / 2)
                    item.m_a四隅.p左下 = New S実座標(-dX - d高さ, -p_d外側_縦 * d周長比率対底の周 / 2)
                    '周の区切り
                    If 1 < i周数 Then
                        For i As Integer = 1 To i周数 - 1
                            Dim p1 As New S実座標(-dX - d高さ * (i / i周数), item.m_a四隅.y最上)
                            Dim p2 As New S実座標(-dX - d高さ * (i / i周数), item.m_a四隅.y最下)
                            Dim line As New S線分(p1, p2)
                            item.m_lineList.Add(line)
                        Next
                    End If
                    '文字は指定しない
                    itemlist.AddItem(item)
                End If

            End If
            dY += d高さ
            dX += d高さ
        Next

        Return itemlist
    End Function

    '楕円底の差しひも
    Friend Class cls差しひも

        '4つの角
        Class clsCorner
            Dim _lst段のひも数 As New List(Of Integer)
            Dim _n合計ひも数 As Integer = 0
            Dim _aryAngle() As Double

            '段の追加、ひも数指定
            Sub addDan(ByVal count As Integer)
                _lst段のひも数.Add(count)
                _n合計ひも数 += count
            End Sub

            '段数確定後の角度計算
            Function calcAngle() As Boolean
                If _n合計ひも数 <= 0 Then
                    Return False
                End If
                ReDim _aryAngle(_n合計ひも数)    '0=0,90度までの角度: 1～_n合計ひも数に値  

                Dim aryIndex As Integer
                If 0 < _n合計ひも数 Then
                    Dim angle As Double = 90 / (1 + _n合計ひも数)
                    aryIndex = 1
                    For i As Integer = 1 To _n合計ひも数 Step 2
                        _aryAngle(i) = angle * aryIndex
                        aryIndex += 1
                    Next
                    aryIndex = _n合計ひも数
                    For i As Integer = 2 To _n合計ひも数 Step 2
                        _aryAngle(i) = angle * aryIndex
                        aryIndex -= 1
                    Next
                End If
                'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Angle={0}", String.Join(", ", _aryAngle.Select(Function(v) v.ToString("F1"))))

                Return True
            End Function

            '指定の段のひも数(iDan=0～ )
            Function getDanCount(ByVal iDan As Integer) As Integer
                If iDan < 0 OrElse _lst段のひも数.Count <= iDan Then
                    Return 0
                End If
                Return _lst段のひも数(iDan)
            End Function

            '指定段・位置の角度取得 iPos=1～段のひも数
            Function getAngle(ByVal iDan As Integer, ByVal iPos As Integer) As Integer
                If _n合計ひも数 <= 0 Then
                    Return 0
                End If
                Dim iTotal As Integer = 0
                For i As Integer = 0 To iDan - 1
                    iTotal += _lst段のひも数(i)
                Next
                iTotal += iPos

                If _n合計ひも数 < iTotal Then
                    Return 0
                End If
                Return _aryAngle(iTotal)
            End Function

        End Class


        Dim _a楕円の中心 As S四隅
        Dim _list As New List(Of clsGroupDataRow)
        Dim _dひも幅 As Double

        Dim _clsCorners(3) As clsCorner

        Dim _r差しひも As Double = 0

        Dim _isUpRightOnly As Boolean


        Sub New(ByVal dひも幅 As Double, ByVal a楕円の中心 As S四隅, ByVal isUpRightOnly As Boolean)
            _dひも幅 = dひも幅
            _a楕円の中心 = a楕円の中心
            _isUpRightOnly = isUpRightOnly

            For i As Integer = 0 To 3
                _clsCorners(i) = New clsCorner
            Next
        End Sub

        Sub AddGroupRow(ByVal groupRow As clsGroupDataRow)
            _list.Add(groupRow)
            Dim i差しひも本数 As Integer = groupRow.GetNameValue("f_i差しひも本数")

            '四隅に振り分ける
            For i As Integer = 0 To 3
                Dim cnt As Integer = i差しひも本数 \ 4
                Dim amari As Integer = i差しひも本数 Mod 4
                If 0 < i AndAlso 0 < amari Then
                    If i <= amari Then
                        cnt += 1
                    End If
                End If
                _clsCorners(i).addDan(cnt)

                '0(4の倍数のみ) 1(余り1以上) 2(余り2以上) 3(余り3以上)
                'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}:{1}/{2}", i, cnt, i差しひも本数)
            Next

        End Sub

        Function GetImageItem(ByVal itemlist As clsImageItemList) As Boolean

            For i As Integer = 0 To 3
                _clsCorners(i).calcAngle()
            Next

            _r差しひも = 0
            For iDan As Integer = 0 To _list.Count - 1
                Dim groupRow As clsGroupDataRow = _list(iDan)
                getDanItem(itemlist, groupRow, iDan)

                _r差しひも = groupRow.GetNameValue("f_d径の累計")
            Next

            Return True
        End Function

        Private Function getDanItem(ByVal itemlist As clsImageItemList, ByVal groupRow As clsGroupDataRow, ByVal iDan As Integer) As Boolean

            Dim iひも数 As Integer '= groupRow.GetNameValue("f_i差しひも本数") \ 4
            Dim i本幅 As Integer = groupRow.GetNameValue("f_i何本幅")
            Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
            Dim dひも長 As Double = groupRow.GetNameValue("f_d連続ひも長")

            Dim iCorner As Integer
            Dim bands(3) As CBand
            Dim p始点(3) As S実座標

            '*右上 : 0 (0度方向から)
            iCorner = 0
            bands(iCorner) = New CBand(groupRow)
            p始点(iCorner) = _a楕円の中心.p右上 + Unit0 * _r差しひも
            bands(iCorner).SetBand(New S線分(p始点(iCorner), p始点(iCorner) + Unit0 * dひも長), d幅)
            bands(iCorner).SetMarkPosition(enumMarkPosition._終点の後, _dひも幅 / 4)

            iひも数 = _clsCorners(iCorner).getDanCount(iDan)
            For iPos As Integer = 1 To iひも数
                Dim angle As Double = _clsCorners(iCorner).getAngle(iDan, iPos)
                Dim band As New CBand(bands(iCorner))
                band.RotateBand(_a楕円の中心.p右上, angle)
                Dim item As New clsImageItem(band, iCorner + iDan, iCorner)
                itemlist.AddItem(item)
            Next

            If Not _isUpRightOnly Then
                '*左上 : 1　(90度方向から)
                iCorner = 1
                bands(iCorner) = New CBand(groupRow)
                p始点(iCorner) = _a楕円の中心.p左上 + Unit90 * _r差しひも
                bands(iCorner).SetBand(New S線分(p始点(iCorner), p始点(iCorner) + Unit90 * dひも長), d幅)
                bands(iCorner).SetMarkPosition(enumMarkPosition._終点の後, _dひも幅 / 2, Unit180 * (_dひも幅 / 2))

                iひも数 = _clsCorners(iCorner).getDanCount(iDan)
                For iPos As Integer = 1 To iひも数
                    Dim angle As Double = _clsCorners(iCorner).getAngle(iDan, iPos)
                    Dim band As New CBand(bands(iCorner))
                    band.RotateBand(_a楕円の中心.p左上, angle)
                    Dim item As New clsImageItem(band, iCorner + iDan, iCorner)
                    itemlist.AddItem(item)
                Next

                '*左下 : 2　(180度方向から)
                iCorner = 2
                bands(iCorner) = New CBand(groupRow)
                p始点(iCorner) = _a楕円の中心.p左下 + Unit180 * _r差しひも
                bands(iCorner).SetBand(New S線分(p始点(iCorner), p始点(iCorner) + Unit180 * dひも長), d幅)
                bands(iCorner).SetMarkPosition(enumMarkPosition._終点の後, _dひも幅 * 2, Unit90 * (_dひも幅 / 2))

                iひも数 = _clsCorners(iCorner).getDanCount(iDan)
                For iPos As Integer = 1 To iひも数
                    Dim angle As Double = _clsCorners(iCorner).getAngle(iDan, iPos)
                    Dim band As New CBand(bands(iCorner))
                    band.RotateBand(_a楕円の中心.p左下, angle)
                    Dim item As New clsImageItem(band, iCorner + iDan, iCorner)
                    itemlist.AddItem(item)
                Next

                '*右下 : 3　(270度方向から)
                iCorner = 3
                bands(iCorner) = New CBand(groupRow)
                p始点(iCorner) = _a楕円の中心.p右下 + Unit270 * _r差しひも
                bands(iCorner).SetBand(New S線分(p始点(iCorner), p始点(iCorner) + Unit270 * dひも長), d幅)
                bands(iCorner).SetMarkPosition(enumMarkPosition._終点の後, _dひも幅 / 3, Unit270 * (_dひも幅 / 2))

                iひも数 = _clsCorners(iCorner).getDanCount(iDan)
                For iPos As Integer = 1 To iひも数
                    Dim angle As Double = _clsCorners(iCorner).getAngle(iDan, iPos)
                    Dim band As New CBand(bands(iCorner))
                    band.RotateBand(_a楕円の中心.p右下, angle)
                    Dim item As New clsImageItem(band, iCorner + iDan, iCorner)
                    itemlist.AddItem(item)
                Next
            End If

            Return True
        End Function
    End Class

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

            iYoko = 0
            For Each itemYoko As clsImageItem In _ImageList横ひも
                iYoko += 1
                If _CUpDown.GetIsDown(iTate, iYoko) Then
                    itemTate.AddClip(itemYoko)
                End If
            Next
        Next

        iYoko = 0
        For Each itemYoko As clsImageItem In _ImageList横ひも
            iYoko += 1

            iTate = 0
            For Each itemTate As clsImageItem In _ImageList縦ひも
                iTate += 1
                If _CUpDown.GetIsUp(iTate, iYoko) Then
                    itemYoko.AddClip(itemTate)
                End If
            Next
        Next

        Return True
    End Function

    '底と側面枠
    Function imageList底と側面枠(ByVal dひも幅 As Double, ByVal isUpRightOnly As Boolean, ByVal isShowSide As Boolean) As clsImageItemList
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

        Dim a楕円の中心 As S四隅
        a楕円の中心.p左上 = a底の縦横.p左上 + Unit270 * _d最上と最下の短いひもの幅
        a楕円の中心.p右上 = a底の縦横.p右上 + Unit270 * _d最上と最下の短いひもの幅
        a楕円の中心.p左下 = a底の縦横.p左下 + Unit90 * _d最上と最下の短いひもの幅
        a楕円の中心.p右下 = a底の縦横.p右下 + Unit90 * _d最上と最下の短いひもの幅

        Dim sasihimo As New cls差しひも(dひも幅, a楕円の中心, isUpRightOnly)

        '番号ごと
        Dim res = (From row As tbl底_楕円Row In _Data.p_tbl底_楕円
                   Select Num = row.f_i番号
                   Order By Num).Distinct
        Dim r差しひも As Double = 0
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl底_楕円.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

            Dim b差しひも区分 As Boolean = groupRow.GetNameValue("f_b差しひも区分")

            If b差しひも区分 Then
                '差しひも
                sasihimo.AddGroupRow(groupRow)

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

        If isShowSide Then
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
        End If

        If Not isUpRightOnly AndAlso isShowSide Then
            '下の側面
            item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 3)
            item.m_a四隅.p左上 = New S実座標(-p_d外側_横 / 2, -p_d外側_縦 / 2)
            item.m_a四隅.p右上 = New S実座標(+p_d外側_横 / 2, -p_d外側_縦 / 2)
            item.m_a四隅.p左下 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, -_d高さの合計 - p_d外側_縦 / 2)
            item.m_a四隅.p右下 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, -_d高さの合計 - p_d外側_縦 / 2)
            itemlist.AddItem(item)

            '左の側面
            item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 4)
            item.m_a四隅.p右上 = New S実座標(-p_d外側_横 / 2, p_d外側_縦 / 2)
            item.m_a四隅.p右下 = New S実座標(-p_d外側_横 / 2, -p_d外側_縦 / 2)
            item.m_a四隅.p左上 = New S実座標(-_d高さの合計 - p_d外側_横 / 2, p_d外側_縦 * d周長比率対底の周 / 2)
            item.m_a四隅.p左下 = New S実座標(-_d高さの合計 - p_d外側_横 / 2, -p_d外側_縦 * d周長比率対底の周 / 2)
            itemlist.AddItem(item)
        End If

        '差しひも
        sasihimo.GetImageItem(itemlist)

        Return itemlist
    End Function


    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData, ByVal isUpRightOnly As Boolean, ByVal isShowSide As Boolean) As Boolean
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

        Dim imgList横ひも As New clsImageItemList()
        Dim imgList縦ひも As New clsImageItemList()


        '文字サイズ
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)
        '基本のひも幅と基本色
        imgData.setBasics(dひも幅, _Data.p_row目標寸法.Value("f_s基本色"))

        '描画用のデータ追加
        Me.imageList横ひも(imgList横ひも, get横展開DataTable(), isUpRightOnly)
        Me.imageList縦ひも(imgList縦ひも, get縦展開DataTable(), isUpRightOnly)

        If _Data.p_row底_縦横.Value("f_b展開区分") Then
            '描画用のデータ追加
            regionUpDown底(imgList横ひも, imgList縦ひも)
        End If

        _imageList側面編みかた = imageList側面編みかた(dひも幅, isUpRightOnly, isShowSide)
        _ImageList描画要素 = imageList底と側面枠(dひも幅, isUpRightOnly, isShowSide)


        '中身を移動
        imgData.MoveList(imgList横ひも)
        imgList横ひも = Nothing
        imgData.MoveList(imgList縦ひも)
        imgList縦ひも = Nothing
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
