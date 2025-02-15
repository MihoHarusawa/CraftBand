Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsImageItem.CBand
Imports CraftBand.clsUpDown
Imports CraftBand.mdlUnit
Imports CraftBand.Tables.dstDataTables


Partial Public Class clsCalcMesh
    '               
    '             ┌────┐配置ラインは外側(底の周+底の厚さ)
    '             │        │↙             【側面図】
    '       ┌──┼────┼──┐           [右上]底辺=外側_横,外側_縦           
    '       │    │╭───╮│    │            '周長比率対底の周'倍の台形を描画
    '       │全体│| 縦横 |径    │            ※楕円底や丸底は、実サイズではない
    '       │    │╰───╯│    │           [全体]底辺=底の内側周を縦横比で分割
    '       └──┼────┼──┘            '周長比率対底の周'倍の台形を描画
    '             │ 全体時 │                  ※厚さはプラスされていない
    '             └────┘
    '
    '右上時の省略部分の長さ(輪弧以外)
    Dim _dPortionOver As Double = New Length(1, "cm").Value

    Dim _bandPositionListYoko As New CBandPositionList(DirectionIndex._yoko)
    Dim _bandPositionListTate As New CBandPositionList(DirectionIndex._tate)


#Region "底ひものセット"
    '                              終点
    '     ┌──────┐        ┌┬┐               放射状          輪弧
    '始点 ├── → ──┤終点    │││
    '     └──────┘        │↑│並び方向→      1 2             1 2
    '      並び方向↓             │││                ↖↑↗            ↖↑↗       
    '                             └┴┘                 ・→ n      n ←・→ n/2
    '                              始点                                 ↙↓↘
    Enum DirectionIndex
        _yoko = 0   '横
        _tate       '縦
        _radial     '放射状　(縦の代替・横なし)
        _circle     '輪弧
    End Enum

    'バンドの方向                         横ひも→ 　縦ひも↑
    Shared cDeltaBandDirection() As S差分 = {Unit0, Unit90}
    'バンドの並び方向(-90)　              横ひも↓　縦ひも→　放射状(Zero)  輪弧(Zero)
    Shared cDeltaAxisDirection() As S差分 = {Unit270, Unit0, New S差分(0, 0), New S差分(0, 0)}


    '展開した各バンド
    Friend Class CBandPosition
        Friend _parent As CBandPositionList = Nothing   '方向情報
        Friend m_Index As Integer = -1 '1～要素数

        Friend m_row縦横展開 As tbl縦横展開Row

        Friend m_p中心点 As S実座標 '加算ゼロ時, X軸上/Y軸上
        Friend m_p始点 As S実座標
        Friend m_p終点 As S実座標

        Friend m_band As CBand = Nothing

        Sub New(ByVal row As tbl縦横展開Row)
            m_row縦横展開 = row
        End Sub

        Sub Clear()
            m_row縦横展開 = Nothing
            m_band = Nothing
        End Sub

        '識別情報
        Friend ReadOnly Property Ident As String
            Get
                Return String.Format("Direction({0}){1}", _parent._DirectionIndex, m_Index)
            End Get
        End Property

        'バンド描画
        Function ToBand(ByVal d基本のひも幅 As Double) As CBand
            m_band = Nothing
            If m_row縦横展開 Is Nothing Then
                Return Nothing
            End If

            m_band = New CBand(m_row縦横展開)

            'バンド描画位置
            Dim dひも幅 As Double = m_row縦横展開.f_dVal1
            Dim p始点 As S実座標 = m_p始点
            If _parent._IsUpRightOnly Then
                If _parent._DirectionIndex = DirectionIndex._yoko Then
                    If p始点.X < _parent._xLimit Then
                        p始点.X = _parent._xLimit
                        m_band.is始点FT線 = False
                    End If
                ElseIf _parent._DirectionIndex = DirectionIndex._tate Then
                    If p始点.Y < _parent._yLimit Then
                        p始点.Y = _parent._yLimit
                        m_band.is始点FT線 = False
                    End If
                End If
            End If
            m_band.SetBand(New S線分(p始点, m_p終点), dひも幅, _parent.DeltaAxisDirection)
            If _parent._DirectionIndex = DirectionIndex._radial AndAlso _parent._IsUpRightOnly Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} {1}", m_row縦横展開.f_iひも番号, m_band.ToString)
                Dim ret1 As Boolean = m_band.TrimBandY(_parent._yLimit)
                Dim ret2 As Boolean = m_band.TrimBandX(_parent._xLimit, m_row縦横展開.f_d幅 < 90)
                '※角度の差0.3で平行扱いされます
            End If

            '記号描画位置
            If IsDrawMarkCurrent Then
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
                    delta = New S差分(-dひも幅 / 4, 0)
                Else
                    '終点側
                    mark = enumMarkPosition._終点の後
                    delta = New S差分(-dひも幅 / 2, -dひも幅 / 2)
                    distance = d基本のひも幅
                End If
                m_band.SetMarkPosition(mark, distance, delta)
            End If

            Return m_band
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

        '並びの方向　横ひも↓　縦ひも→  放射状(Zero)  輪弧(Zero)
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
            For Each bandpos As CBandPosition In _BandList
                bandpos.Clear()
            Next
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
        '指定剰余位置の要素 1～_iひもの本数
        Friend ReadOnly Property ByIdx(ByVal idx As Integer) As CBandPosition
            Get
                Dim i As Integer = Modulo(idx - 1, _BandList.Count)
                Return _BandList(i)
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
        ElseIf bandPositionList._DirectionIndex = DirectionIndex._radial Then
            'pバンド辺中心は使わない
        ElseIf bandPositionList._DirectionIndex = DirectionIndex._circle Then
            'pバンド辺中心は使わない
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

            If bandPositionList._DirectionIndex = DirectionIndex._radial Then
                '放射状に配置
                bandpos.m_p中心点 = pOrigin
                Dim delta As New S差分(row.f_d幅)
                'f_dVal2 = 先半分の出力ひも長
                bandpos.m_p始点 = pOrigin - delta * row.f_dVal2
                'f_dVal3 = 後半分の出力ひも長
                bandpos.m_p終点 = pOrigin + delta * row.f_dVal3

            ElseIf bandPositionList._DirectionIndex = DirectionIndex._circle Then
                '輪弧に配置
                Dim d半径 As Double = _d内円の半径 + g_clsSelectBasics.p_d指定本幅(_i縦ひも何本幅) / 2
                bandpos.m_p中心点 = pOrigin + New S差分(row.f_d幅) * d半径
                Dim delta As New S差分(row.f_d幅 - 90)
                'f_dVal2 = 先半分の出力ひも長
                bandpos.m_p始点 = bandpos.m_p中心点 - delta * row.f_dVal2
                'f_dVal3 = 後半分の出力ひも長
                bandpos.m_p終点 = bandpos.m_p中心点 + delta * row.f_dVal3

            Else
                '縦横に配置

                'f_dVal1 = ひも幅(f_i何本幅) の半分
                bandpos.m_p中心点 = pバンド辺中心 + bandPositionList.DeltaAxisDirection * (row.f_dVal1 / 2)
                'f_dVal2 = 先半分の出力ひも長
                bandpos.m_p始点 = bandpos.m_p中心点 - bandPositionList.DeltaBandDirection * row.f_dVal2
                'f_dVal3 = 後半分の出力ひも長
                bandpos.m_p終点 = bandpos.m_p中心点 + bandPositionList.DeltaBandDirection * row.f_dVal3
                '
                pバンド辺中心 = pバンド辺中心 + bandPositionList.DeltaAxisDirection * row.f_d幅
            End If

            bandPositionList.Add(bandpos)
        Next

        Return True
    End Function

    '横ひもリストの描画情報
    Private Function imageList横ひも(ByVal imgList横ひも As clsImageItemList, ByVal table As tbl縦横展開DataTable, ByVal isUpRightOnly As Boolean) As Boolean
        If imgList横ひも Is Nothing Then
            Return False
        End If
        If _enum配置タイプ <> enum配置タイプ.i_縦横 Then
            Return True
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

    '縦ひも(縦・放射状・輪弧)リストの描画情報
    Private Function imageList縦ひも(ByVal imgList縦ひも As clsImageItemList, ByVal table As tbl縦横展開DataTable, ByVal isUpRightOnly As Boolean) As Boolean
        If imgList縦ひも Is Nothing Then
            Return False
        End If

        Dim skip_odd As Boolean = False
        If _enum配置タイプ = enum配置タイプ.i_放射状 Then
            _bandPositionListTate._DirectionIndex = DirectionIndex._radial
        ElseIf _enum配置タイプ = enum配置タイプ.i_輪弧 Then
            _bandPositionListTate._DirectionIndex = DirectionIndex._circle
            skip_odd = _b二重
        Else
            _bandPositionListTate._DirectionIndex = DirectionIndex._tate
        End If

        If Not setBandPositionList(_bandPositionListTate, table, isUpRightOnly) Then
            Return False
        End If

        For i As Integer = 0 To _bandPositionListTate.Count - 1
            Dim bandpos As CBandPosition = _bandPositionListTate(i)
            If skip_odd AndAlso (i Mod 2) = 1 Then
                Continue For
            End If
            '
            Dim band As CBand = bandpos.ToBand(_d基本のひも幅)
            If band IsNot Nothing Then
                Dim item As New clsImageItem(band, 20, bandpos.m_Index)
                imgList縦ひも.AddItem(item)
            End If
        Next

        Return True
    End Function


    '_imageList側面ひも生成、側面のレコードを含む　
    Function imageList側面編みかた(ByVal d横辺ベース As Double, ByVal d縦辺ベース As Double, ByVal isUpRightOnly As Boolean, ByVal isShowSide As Boolean) As clsImageItemList
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
                    item.m_a四隅.p左下 = New S実座標(-d横辺ベース * d周長比率対底の周 / 2, dY)
                    item.m_a四隅.p右下 = New S実座標(+d横辺ベース * d周長比率対底の周 / 2, dY)
                    item.m_a四隅.p左上 = New S実座標(-d横辺ベース * d周長比率対底の周 / 2, dY + d高さ)
                    item.m_a四隅.p右上 = New S実座標(+d横辺ベース * d周長比率対底の周 / 2, dY + d高さ)
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
                    If IsDrawMarkCurrent Then
                        item.p_p文字位置 = New S実座標(_d基本のひも幅 + d横辺ベース * d周長比率対底の周 / 2, dY + d高さ / 2)
                    End If
                    itemlist.AddItem(item)

                    '--- 右 ---
                    'ImageTypeEnum._編みかた・縦
                    item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 2)
                    item.m_a四隅.p左上 = New S実座標(dX, +d縦辺ベース * d周長比率対底の周 / 2)
                    item.m_a四隅.p左下 = New S実座標(dX, -d縦辺ベース * d周長比率対底の周 / 2)
                    item.m_a四隅.p右上 = New S実座標(dX + d高さ, +d縦辺ベース * d周長比率対底の周 / 2)
                    item.m_a四隅.p右下 = New S実座標(dX + d高さ, -d縦辺ベース * d周長比率対底の周 / 2)
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
                    item.m_a四隅.p左上 = New S実座標(-d横辺ベース * d周長比率対底の周 / 2, -dY)
                    item.m_a四隅.p右上 = New S実座標(+d横辺ベース * d周長比率対底の周 / 2, -dY)
                    item.m_a四隅.p左下 = New S実座標(-d横辺ベース * d周長比率対底の周 / 2, -dY - d高さ)
                    item.m_a四隅.p右下 = New S実座標(+d横辺ベース * d周長比率対底の周 / 2, -dY - d高さ)
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
                    item.m_a四隅.p右上 = New S実座標(-dX, +d縦辺ベース * d周長比率対底の周 / 2)
                    item.m_a四隅.p右下 = New S実座標(-dX, -d縦辺ベース * d周長比率対底の周 / 2)
                    item.m_a四隅.p左上 = New S実座標(-dX - d高さ, +d縦辺ベース * d周長比率対底の周 / 2)
                    item.m_a四隅.p左下 = New S実座標(-dX - d高さ, -d縦辺ベース * d周長比率対底の周 / 2)
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

        'ひとつの角
        Class clsCorner
            Dim m_lst段のひも数 As New List(Of Integer)
            Dim m_n合計ひも数 As Integer = 0
            Dim m_aryAngle() As Double

            Dim m_countBand As Integer = 0

            'ひも数を増やす
            Sub incBand()
                m_countBand += 1
            End Sub

            'ひも数を、段のひも数として追加し、次の段へ
            Sub nextDan()
                m_lst段のひも数.Add(m_countBand)
                m_n合計ひも数 += m_countBand
                m_countBand = 0
            End Sub

            '段数確定後の角度計算, 引数は分割対象の角度
            Function calcAngle(ByVal targetAngle As Double) As Boolean
                If m_n合計ひも数 <= 0 OrElse targetAngle = 0 Then
                    Return False
                End If
                ReDim m_aryAngle(m_n合計ひも数)    '0=0,unitAngle未満の角度: 1～_n合計ひも数に値  

                Dim aryIndex As Integer
                If 0 < m_n合計ひも数 Then
                    Dim angle_step As Double = targetAngle / (1 + m_n合計ひも数)
                    aryIndex = 1
                    For i As Integer = 1 To m_n合計ひも数 Step 2
                        m_aryAngle(i) = angle_step * aryIndex
                        aryIndex += 1
                    Next
                    aryIndex = m_n合計ひも数
                    For i As Integer = 2 To m_n合計ひも数 Step 2
                        m_aryAngle(i) = angle_step * aryIndex
                        aryIndex -= 1
                    Next
                End If

                Return True
            End Function

            '指定の段のひも数(iDan=0～ )
            Function getDanCount(ByVal iDan As Integer) As Integer
                If iDan < 0 OrElse m_lst段のひも数.Count <= iDan Then
                    Return 0
                End If
                Return m_lst段のひも数(iDan)
            End Function

            '指定段・位置の角度取得 iPos=1～段のひも数
            Function getAngle(ByVal iDan As Integer, ByVal iPos As Integer) As Integer
                If m_n合計ひも数 <= 0 OrElse m_aryAngle Is Nothing Then
                    Return 0
                End If
                Dim iTotal As Integer = 0
                For i As Integer = 0 To iDan - 1
                    iTotal += m_lst段のひも数(i)
                Next
                iTotal += iPos

                If m_n合計ひも数 < iTotal OrElse m_aryAngle.Length <= iTotal Then
                    Return 0
                End If
                Return m_aryAngle(iTotal)
            End Function

            Overrides Function ToString() As String
                Dim sb As New System.Text.StringBuilder
                sb.AppendFormat("合計ひも数={0} 段数={1}({2})", m_n合計ひも数, m_lst段のひも数.Count, String.Join(",", m_lst段のひも数.ToArray))
                If m_aryAngle IsNot Nothing Then
                    sb.AppendFormat(" Angle={0}", String.Join(", ", m_aryAngle.Select(Function(v) v.ToString("F1"))))
                End If
                Return sb.ToString
            End Function

        End Class

        Dim _isUpRightOnly As Boolean '右上描画フラグ
        Dim _dひも基本幅 As Double
        Dim _n角の数 As Integer
        Dim _ary配置点() As S実座標
        Dim _d開始径 As Double

        'それぞれの角
        Dim _clsCorners() As clsCorner

        '段のレコード
        Dim _listGroupDataRow As New List(Of clsGroupDataRow)
        Dim _d前段の径累計 As Double = 0

        Dim _idxCorner As Integer = 0 '次に加える角のインデックス


        Private ReadOnly Property p配置点(ByVal idx As Integer) As S実座標
            Get
                If _ary配置点 Is Nothing OrElse _ary配置点.Length = 0 Then
                    Return pOrigin
                End If
                If idx < 0 OrElse _ary配置点.Length <= idx Then
                    Return pOrigin
                End If
                Return _ary配置点(idx)
            End Get
        End Property


        Sub New(ByVal n角の数 As Integer, ByVal dひも幅 As Double, ByVal ary配置点() As S実座標, ByVal d開始径 As Double, ByVal isUpRightOnly As Boolean)
            _n角の数 = n角の数
            _dひも基本幅 = dひも幅
            _ary配置点 = ary配置点
            _d開始径 = d開始径
            _isUpRightOnly = isUpRightOnly

            ReDim _clsCorners(_n角の数 - 1)
            For i As Integer = 0 To _n角の数 - 1
                _clsCorners(i) = New clsCorner
            Next
        End Sub

        Sub Clear()
            _listGroupDataRow.Clear()
            If _clsCorners IsNot Nothing Then
                Array.Clear(_clsCorners, 0, _clsCorners.Length)
            End If
        End Sub

        Sub AddGroupRow(ByVal groupRow As clsGroupDataRow)
            _listGroupDataRow.Add(groupRow)
            Dim i差しひも本数 As Integer = groupRow.GetNameValue("f_i差しひも本数")

            '各角に振り分ける
            For i As Integer = 1 To i差しひも本数
                If _n角の数 <= _idxCorner Then
                    _idxCorner = 0
                End If
                _clsCorners(_idxCorner).incBand()

                _idxCorner += 1
            Next
            '次の段へ
            For i As Integer = 0 To _n角の数 - 1
                _clsCorners(i).nextDan()
            Next

        End Sub

        '描画リストに差しひもを追加
        Function GetImageItem(ByVal itemlist As clsImageItemList) As Boolean
            '各角を、その角の差しひも数で分割
            For i As Integer = 0 To _n角の数 - 1
                _clsCorners(i).calcAngle(360 / _n角の数)
                '
                'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}:{1}", i, _clsCorners(i).ToString)
            Next

            '差しひもの描画アイテム
            _d前段の径累計 = _d開始径
            For iDan As Integer = 0 To _listGroupDataRow.Count - 1
                Dim groupRow As clsGroupDataRow = _listGroupDataRow(iDan)
                getDanItem(itemlist, groupRow, iDan)

                _d前段の径累計 = groupRow.GetNameValue("f_d径の累計")
            Next

            Return True
        End Function

        Private Function getDanItem(ByVal itemlist As clsImageItemList, ByVal groupRow As clsGroupDataRow, ByVal iDan As Integer) As Boolean

            Dim iひも数 As Integer '= groupRow.GetNameValue("f_i差しひも本数") \ 4
            Dim i本幅 As Integer = groupRow.GetNameValue("f_i何本幅")
            Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
            Dim dひも長 As Double = groupRow.GetNameValue("f_d連続ひも長")

            Dim bands(_n角の数 - 1) As CBand
            Dim p始点(_n角の数 - 1) As S実座標

            Dim angle開始方向(_n角の数 - 1) As Double
            For i As Integer = 0 To _n角の数 - 1
                angle開始方向(i) = (360 / _n角の数) * i
            Next

            For iCorner As Integer = 0 To _n角の数 - 1
                '右上の場合は(ゼロから開始して)90度まで
                If _isUpRightOnly AndAlso 90 <= angle開始方向(iCorner) Then
                    Exit For
                End If

                Dim delta As New S差分(angle開始方向(iCorner))
                bands(iCorner) = New CBand(groupRow)
                p始点(iCorner) = p配置点(iCorner) + delta * _d前段の径累計
                bands(iCorner).SetBand(New S線分(p始点(iCorner), p始点(iCorner) + delta * dひも長), d幅)
                If IsDrawMarkCurrent Then
                    bands(iCorner).SetMarkPosition(enumMarkPosition._終点の後, _dひも基本幅 / 4)
                End If


                iひも数 = _clsCorners(iCorner).getDanCount(iDan)
                For iPos As Integer = 1 To iひも数
                    Dim angle As Double = _clsCorners(iCorner).getAngle(iDan, iPos)
                    Dim band As New CBand(bands(iCorner))
                    band.RotateBand(p配置点(iCorner), angle)
                    Dim item As New clsImageItem(band, iCorner + iDan, iCorner)
                    itemlist.AddItem(item)
                Next
            Next

            Return True
        End Function
    End Class

    '底の編み目
    Private Function regionUpDown底(ByVal _ImageList横ひも As clsImageItemList, ByVal _ImageList縦ひも As clsImageItemList, ByVal updownnone As enumUpDownNone) As Boolean
        If updownnone = enumUpDownNone._None Then
            Return True
        End If
        If _enum配置タイプ = enum配置タイプ.i_輪弧 Then
            Return upDown輪弧(_ImageList縦ひも, updownnone)
        ElseIf _enum配置タイプ = enum配置タイプ.i_放射状 Then
            Return True '放射状は対象外
        End If

        'enum配置タイプ.i_縦横(平編み)
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            Return False
        End If

        Dim _CUpDown As New clsUpDown
        _CUpDown.Reset(0)
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If
        If updownnone = enumUpDownNone._UpDown Then
            _CUpDown.Reverse()
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

    '底の編み目
    Private Function upDown輪弧(ByVal _ImageList縦ひも As clsImageItemList, ByVal updownnone As enumUpDownNone) As Boolean
        If updownnone = enumUpDownNone._None Then
            Return True
        End If
        If _ImageList縦ひも Is Nothing Then
            Return False
        End If

        Dim i連続数 As Integer = _i連続数1 + _i連続数2 + _i連続数3 + _i連続数4 + _i連続数5 + _i連続数6
        Dim _CUpDown As New clsUpDown(enumTargetFace.Bottom, i連続数, 1)
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return True '描画対象外
        End If
        For idx As Integer = 1 To _i連続数1
            _CUpDown.SetIsUp(idx, 1)
        Next
        For idx As Integer = 1 To _i連続数3
            _CUpDown.SetIsUp(_i連続数1 + _i連続数2 + idx, 1)
        Next
        For idx As Integer = 1 To _i連続数5
            _CUpDown.SetIsUp(_i連続数1 + _i連続数2 + _i連続数3 + _i連続数4 + idx, 1)
        Next
        If updownnone = enumUpDownNone._DownUp Then
            _CUpDown.Reverse()
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "({0},{1}) UpDown={2}", _i連続数1, _i連続数2, _CUpDown.ToString)

        Dim drawcount As Integer = _ImageList縦ひも.Count / 2
        If _CUpDown.HorizontalCount < drawcount Then
            drawcount = _CUpDown.HorizontalCount
        End If
        For iBase As Integer = 0 To _ImageList縦ひも.Count - 1
            Dim itemTate As clsImageItem = _ImageList縦ひも(iBase)
            For idx As Integer = 1 To drawcount
                Dim i As Integer = Modulo(iBase + idx, _ImageList縦ひも.Count)
                Dim band As clsImageItem = _ImageList縦ひも(i)
                If iBase <> i Then
                    If _CUpDown.GetIsDown(idx, 1) Then
                        itemTate.AddClip(band)
                    Else
                        band.AddClip(itemTate)
                    End If
                Else
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "Same iBase({0},{1}) _ImageList縦ひもSame ", iBase, idx)
                End If
            Next
        Next

        Return True
    End Function

    '底(楕円と差しひも)　※輪弧では呼ばれない
    Function imageList底(ByVal isUpRightOnly As Boolean, ByVal isShowSide As Boolean) As clsImageItemList
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

        '底の縦横部分
        If _d径の合計 = 0 Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
            item.m_a四隅 = a底
        Else
            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, 1)
            item.m_a四隅 = a底の縦横
        End If
        item.m_is円 = (_enum配置タイプ = enum配置タイプ.i_放射状)
        itemlist.AddItem(item)


        Dim a楕円の中心 As S四隅
        a楕円の中心.p左上 = a底の縦横.p左上 + Unit270 * _d最上と最下の短いひもの幅
        a楕円の中心.p右上 = a底の縦横.p右上 + Unit270 * _d最上と最下の短いひもの幅
        a楕円の中心.p左下 = a底の縦横.p左下 + Unit90 * _d最上と最下の短いひもの幅
        a楕円の中心.p右下 = a底の縦横.p右下 + Unit90 * _d最上と最下の短いひもの幅

        Dim sasihimo As cls差しひも
        If _enum配置タイプ = enum配置タイプ.i_縦横 Then
            Dim ary配置点(3) As S実座標
            ary配置点(0) = a楕円の中心.Point(0) 'p右上 'A
            ary配置点(1) = a楕円の中心.Point(1) 'p左上 'B
            ary配置点(2) = a楕円の中心.Point(2) 'p左下 'C
            ary配置点(3) = a楕円の中心.Point(3) 'p右下 'D
            sasihimo = New cls差しひも(4, _d基本のひも幅, ary配置点, 0, isUpRightOnly)
        Else
            sasihimo = New cls差しひも(_i縦ひもの本数 * 2, _d基本のひも幅, Nothing, _d縦横の縦 / 2, isUpRightOnly)
        End If

        '番号ごと
        item = Nothing
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
                item = New clsImageItem(ImageTypeEnum._底楕円, groupRow, groupRow.GetNameValue("f_i番号"))

                item.m_dひも幅 = groupRow.GetNameValue("f_d径") '1レコード想定
                Dim d径の累計 As Double = groupRow.GetNameValue("f_d径の累計") '1レコード想定
                If (_enum配置タイプ = enum配置タイプ.i_放射状) Then
                    item.m_a四隅 = New S四隅(New S領域(New S実座標(-d径の累計, -d径の累計), New S実座標(d径の累計, d径の累計)))
                    item.m_is円 = True
                    If IsDrawMarkCurrent Then
                        item.p_p文字位置 = pOrigin + New S差分(310) * (d径の累計 - _d基本のひも幅)
                    End If

                Else
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
                    If IsDrawMarkCurrent Then
                        item.p_p文字位置 = line.p終了 '文字位置
                    End If
                    item.m_lineList.Add(line)
                    '右下→右上
                    line = New S線分(New S実座標(a底の縦横.p右下.X + d径の累計, a底の縦横.p右下.Y + _d最上と最下の短いひもの幅), New S実座標(a底の縦横.p右上.X + d径の累計, a底の縦横.p右上.Y - _d最上と最下の短いひもの幅))
                    item.m_lineList.Add(line)

                End If

                itemlist.AddItem(item)
            End If
        Next
        '最後の底楕円を底枠描画
        If item IsNot Nothing Then
            Dim itemBottomLine As New clsImageItem(ImageTypeEnum._底楕円, CType(Nothing, clsGroupDataRow), 999)
            itemBottomLine.m_a四隅 = item.m_a四隅
            itemBottomLine.m_is円 = item.m_is円
            itemBottomLine.m_lineList.AddRange(item.m_lineList)
            itemlist.AddItem(itemBottomLine)
        End If

        '差しひも
        sasihimo.GetImageItem(itemlist)
        sasihimo.Clear()

        Return itemlist
    End Function

    '輪弧の底
    Function imageList底_輪弧() As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        Dim c底の円 As New S円(_d内円の半径 + _d底部分の径)
        Dim c高さの円 As New S円(_d内円の半径 + _d底部分の径 + _d高さの合計)

        '底
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = New S四隅(c底の円.r外接領域)
        item.m_is円 = True
#If 0 Then
        Dim dバンド径 As Double = _d内円の半径 + g_clsSelectBasics.p_d指定本幅(_i縦ひも何本幅)
        For Each bandpos As CBandPosition In _bandPositionListTate
            Dim p As S実座標 = pOrigin + New S差分(bandpos.m_row縦横展開.f_d幅) * dバンド径
            Dim fn As New S直線式(bandpos.m_row縦横展開.f_d幅 - 90, p)
            Dim ary() As S実座標 = c高さの円.ary直線との交点(fn)
            If ary IsNot Nothing AndAlso 2 <= ary.Count Then
                item.m_lineList.Add(New S線分(ary(0), ary(1)))
            End If
        Next
#End If
        itemlist.AddItem(item)

        '合わせ位置
        If _b二重 AndAlso 0 < _d合わせ位置の半径 Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._全体枠, 1)
            item.m_a四隅 = New S四隅(New S円(_d合わせ位置の半径).r外接領域)
            item.m_is円 = True
            itemlist.AddItem(item)
        End If

        '全体枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._全体枠, 1)
        item.m_a四隅 = New S四隅(c高さの円.r外接領域)
        item.m_is円 = True
        itemlist.AddItem(item)

        Return itemlist
    End Function

    '側面の枠　
    Function imageList側面枠(ByVal d横辺ベース As Double, ByVal d縦辺ベース As Double, ByVal isUpRightOnly As Boolean, ByVal isShowSide As Boolean) As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        '側面の枠

        '縁/最上のf_d周長比率対底の周(同じf_i番号内の最大)
        Dim d周長比率対底の周 As Double = 0
        Dim rows() As tbl側面Row = _Data.p_tbl側面.Select(Nothing, "f_i番号 DESC")
        Dim i番号 As Integer = -1
        For Each row As tbl側面Row In rows
            If i番号 < 0 Then
                i番号 = row.f_i番号
            ElseIf i番号 > row.f_i番号 Then
                Exit For
            End If
            If Not IsDBNull(row("f_d周長比率対底の周")) AndAlso 0 < row("f_d周長比率対底の周") Then
                If d周長比率対底の周 < row("f_d周長比率対底の周") Then
                    d周長比率対底の周 = row("f_d周長比率対底の周")
                End If
            End If
        Next
        If d周長比率対底の周 = 0 Then
            d周長比率対底の周 = 1
        End If
        If isShowSide Then
            '上の側面
            item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
            item.m_a四隅.p左下 = New S実座標(-d横辺ベース / 2, p_d外側_縦 / 2)
            item.m_a四隅.p右下 = New S実座標(+d横辺ベース / 2, p_d外側_縦 / 2)
            item.m_a四隅.p左上 = New S実座標(-d横辺ベース * d周長比率対底の周 / 2, _d高さの合計 + p_d外側_縦 / 2)
            item.m_a四隅.p右上 = New S実座標(+d横辺ベース * d周長比率対底の周 / 2, _d高さの合計 + p_d外側_縦 / 2)
            itemlist.AddItem(item)

            '右の側面
            item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
            item.m_a四隅.p左上 = New S実座標(p_d外側_横 / 2, d縦辺ベース / 2)
            item.m_a四隅.p左下 = New S実座標(p_d外側_横 / 2, -d縦辺ベース / 2)
            item.m_a四隅.p右上 = New S実座標(_d高さの合計 + p_d外側_横 / 2, d縦辺ベース * d周長比率対底の周 / 2)
            item.m_a四隅.p右下 = New S実座標(_d高さの合計 + p_d外側_横 / 2, -d縦辺ベース * d周長比率対底の周 / 2)
            itemlist.AddItem(item)
        End If
        If Not isUpRightOnly AndAlso isShowSide Then
            '下の側面
            item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 3)
            item.m_a四隅.p左上 = New S実座標(-d横辺ベース / 2, -p_d外側_縦 / 2)
            item.m_a四隅.p右上 = New S実座標(+d横辺ベース / 2, -p_d外側_縦 / 2)
            item.m_a四隅.p左下 = New S実座標(-d横辺ベース * d周長比率対底の周 / 2, -_d高さの合計 - p_d外側_縦 / 2)
            item.m_a四隅.p右下 = New S実座標(+d横辺ベース * d周長比率対底の周 / 2, -_d高さの合計 - p_d外側_縦 / 2)
            itemlist.AddItem(item)

            '左の側面
            item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 4)
            item.m_a四隅.p右上 = New S実座標(-p_d外側_横 / 2, d縦辺ベース / 2)
            item.m_a四隅.p右下 = New S実座標(-p_d外側_横 / 2, -d縦辺ベース / 2)
            item.m_a四隅.p左上 = New S実座標(-_d高さの合計 - p_d外側_横 / 2, d縦辺ベース * d周長比率対底の周 / 2)
            item.m_a四隅.p左下 = New S実座標(-_d高さの合計 - p_d外側_横 / 2, -d縦辺ベース * d周長比率対底の周 / 2)
            itemlist.AddItem(item)
        End If

        Return itemlist
    End Function


    '現画像生成時に記号を表示する #60
    Shared IsDrawMarkCurrent As Boolean = True

    Enum enumUpDownNone
        _None
        _DownUp
        _UpDown
    End Enum
    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData, ByVal isUpRightOnly As Boolean, ByVal isShowSide As Boolean, ByVal updownnone As enumUpDownNone) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        IsDrawMarkCurrent = Not String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号)
        '輪弧は常に全体
        If _enum配置タイプ = enum配置タイプ.i_輪弧 Then
            isUpRightOnly = False
        End If

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If
        Dim imgList横ひも As New clsImageItemList()
        Dim imgList縦ひも As New clsImageItemList()


        '文字サイズ(基本のひも幅)と基本色
        imgData.setBasics(_d基本のひも幅, _Data.p_row目標寸法.Value("f_s基本色"))

        '描画用のデータ追加
        Me.imageList横ひも(imgList横ひも, get横展開DataTable(), isUpRightOnly)
        Me.imageList縦ひも(imgList縦ひも, get縦展開DataTable(), isUpRightOnly)

        '底の編み目
        regionUpDown底(imgList横ひも, imgList縦ひも, updownnone)

        '中身を移動
        imgData.MoveList(imgList横ひも)
        imgList横ひも = Nothing
        imgData.MoveList(imgList縦ひも)
        imgList縦ひも = Nothing

        Dim d横辺ベース As Double = p_d外側_横
        Dim d縦辺ベース As Double = p_d外側_縦
        If _enum配置タイプ <> enum配置タイプ.i_輪弧 Then
            If Not isUpRightOnly Then
                '全体の場合は周長ベース
                d横辺ベース = (1 / 2) * _d底の周 * (p_d内側_横 / (p_d内側_横 + p_d内側_縦))
                d縦辺ベース = (1 / 2) * _d底の周 * (p_d内側_縦 / (p_d内側_横 + p_d内側_縦))
            End If

            '底(楕円と差しひも)
            Dim imageList底 As clsImageItemList = Me.imageList底(isUpRightOnly, isShowSide)
            imgData.MoveList(imageList底)
            imageList底 = Nothing
        Else
            '輪弧は常に全体
            d横辺ベース = (1 / 4) * _d底の周
            d縦辺ベース = (1 / 4) * _d底の周

            imageList底_輪弧()
            Dim imageList底 As clsImageItemList = Me.imageList底_輪弧()
            imgData.MoveList(imageList底)
            imageList底 = Nothing
        End If

        If 0 < _d高さの合計 Then
            '側面の枠
            Dim imageList側面枠 As clsImageItemList = Me.imageList側面枠(d横辺ベース, d縦辺ベース, isUpRightOnly, isShowSide)
            imgData.MoveList(imageList側面枠)
            imageList側面枠 = Nothing

            '側面のレコードを含む
            Dim imageList側面編みかた As clsImageItemList = Me.imageList側面編みかた(d横辺ベース, d縦辺ベース, isUpRightOnly, isShowSide)
            imgData.MoveList(imageList側面編みかた)
            imageList側面編みかた = Nothing
        End If

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
