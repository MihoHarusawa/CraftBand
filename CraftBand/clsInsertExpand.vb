

Imports CraftBand.clsImageItem
Imports CraftBand.Tables.dstDataTables

Public Class clsInsertExpand
    Class CInsertItem
        Dim _parent As CInsertItemList

        ReadOnly Property p_i番号 As Integer
            Get
                Return _parent._row.f_i番号
            End Get
        End Property

        ReadOnly Property p_i何本幅 As Integer
            Get
                Return _parent._row.f_i何本幅
            End Get
        End Property

        ReadOnly Property p_s色 As String
            Get
                Return _parent._row.f_s色
            End Get
        End Property


        Public m_dひも長 As Double '描画するひも長(加算値は描かない)
        Public m_s記号 As String  'セットされる記号

        ReadOnly Property p_d出力ひも長 As Double
            Get
                Return m_dひも長 + _parent._row.f_dひも長加算 * 2
            End Get
        End Property


        '長さ計算保持・描画用情報
        Dim m_angle As Integer
        Dim m_line As S線分 'm_angle方向

        Public m_iひも種 As Integer
        Public m_iひも番号 As Integer
        Public m_iひも数 As Integer
        Public m_i開始位置 As Integer
        Public m_d長さ As Double

        Public m_idx As Integer
        Public m_iFlag As Integer
        Public m_pCenter As S実座標
        Public m_delta As S差分


        '識別情報
        Friend ReadOnly Property Ident As String
            Get
                Return String.Format("({0})", p_i番号)
            End Get
        End Property

        Sub New(ByVal parent As CInsertItemList)
            _parent = parent
        End Sub

        Enum mark_position
            _なし
            _始点の前
            _終点の後
        End Enum


        'Function ToBand(ByVal draw_mark As mark_position) As CBand
        '    Dim band = New CBand(_parent._row)

        '    Dim delta As New S差分(m_angle)
        '    Dim deltaAx As New S差分(m_angle + 90)

        '    'バンド描画位置
        '    band.p始点F = m_line.p開始 + deltaAx * (-m_dひも幅 / 2)
        '    band.p終点F = m_line.p終了 + deltaAx * (-m_dひも幅 / 2)
        '    band.p始点T = m_line.p開始 + deltaAx * (m_dひも幅 / 2)
        '    band.p終点T = m_line.p終了 + deltaAx * (m_dひも幅 / 2)

        '    '記号描画位置
        '    If draw_mark = mark_position._始点の前 Then
        '        band.p文字位置 = m_line.p開始 + delta * -m_dひも幅
        '    ElseIf draw_mark = mark_position._終点の後 Then
        '        band.p文字位置 = m_line.p終了 + delta * m_dひも幅
        '    End If

        '    Return band
        'End Function

        'Overrides Function ToString() As String
        '    Dim sb As New System.Text.StringBuilder
        '    sb.AppendFormat("{0} ひも幅({1:f1}) ", Ident, m_dひも幅)
        '    sb.AppendFormat("長さ({0:f1}) ひも数{1} 記号{2} ", m_d長さ, m_iひも数, m_s記号)
        '    sb.AppendFormat("角度({0}) 線分{1}", m_angle, m_line)
        '    Return sb.ToString
        'End Function

    End Class

    '
    Class CInsertItemList
        Inherits List(Of CInsertItem)

        Friend _row As tbl差しひもRow

        Sub New(ByVal row As tbl差しひもRow)
            _row = row
        End Sub

        'Function ConvertToBandList(ByVal draw_mark As mark_position) As CBandList
        '    Dim bandlist As New CBandList
        '    For Each item As CInsertItem In Me
        '        Dim band As CBand = item.ToBand(draw_mark)
        '        bandlist.Add(band)
        '    Next
        '    Return bandlist
        'End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            For Each item As CInsertItem In Me
                sb.AppendLine(item.ToString)
            Next
            Return sb.ToString
        End Function

    End Class

    Class CInsertItemListMap
        Inherits Dictionary(Of Integer, CInsertItemList)


        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            For Each key As Integer In Me.Keys
                sb.AppendFormat("{0}:{1}", key, Me(key).ToString)
            Next
            Return sb.ToString
        End Function
    End Class

    Dim _InsertItemListMap As New CInsertItemListMap


    Sub New()

    End Sub

    Sub Clear()
        _InsertItemListMap.Clear()
    End Sub

    Sub Add(ByVal i As Integer, ByVal ilist As CInsertItemList)
        _InsertItemListMap.Add(i, ilist)
    End Sub

    Function ContainsKey(ByVal i As Integer) As Boolean
        Return _InsertItemListMap.ContainsKey(i)
    End Function

    Function GetList(ByVal i As Integer) As CInsertItemList
        If Not _InsertItemListMap.ContainsKey(i) Then
            Return Nothing
        End If
        Return _InsertItemListMap(i)
    End Function

    Function GetOneItem(ByVal row As tbl差しひもRow) As CInsertItem
        'tbl差しひもRowが1点=長さが確定している
        If row.Isf_dひも長Null Then Throw New Exception("ひも長Null")

        If _InsertItemListMap.ContainsKey(row.f_i番号) Then
            Dim lst As CInsertItemList = _InsertItemListMap(row.f_i番号)
            If lst.Count <> 1 Then Throw New Exception("lst.Count <> 1")
            If Not String.IsNullOrEmpty(row.f_s記号) Then
                lst(0).m_s記号 = row.f_s記号
            End If
            Return lst(0)
        Else
            Dim lst As New CInsertItemList(row)
            Dim item As New CInsertItem(lst)
            item.m_dひも長 = row.f_dひも長
            item.m_s記号 = row.f_s記号 '有無とも
            lst.Add(item)
            _InsertItemListMap.Add(row.f_i番号, lst)
            Return item
        End If
    End Function



End Class
