

Imports CraftBand.clsImageItem
Imports CraftBand.Tables.dstDataTables

Public Class clsInsertExpand
    '差しひも1点
    Class CInsertItem
        Dim _parent As CInsertItemList

        ReadOnly Property p_i番号 As Integer '(※)出力リスト記載
            Get
                Return _parent._row.f_i番号
            End Get
        End Property

        ReadOnly Property p_i何本幅 As Integer '(※)
            Get
                Return _parent._row.f_i何本幅
            End Get
        End Property

        ReadOnly Property p_s色 As String '(※)
            Get
                Return _parent._row.f_s色
            End Get
        End Property


        Public m_s記号 As String  'セットされる記号(※)
        Public m_iひも番号 As Integer '(※)
        Public m_iひも数 As Integer = 1 '(※)
        Public m_dひも長 As Double '(※)計算したひも長(加算値は含まない)

        ReadOnly Property p_d出力ひも長 As Double '(※)
            Get
                Return m_dひも長 + _parent._row.f_dひも長加算 * 2
            End Get
        End Property


        '長さ計算保持・描画用情報
        Public m_idx As Integer 'CInsertItemListの検索値

        Public m_iひも種 As Integer
        Public m_i開始位置 As Integer
        Public m_d長さ As Double '0<m_iFlag時のみ(#72)

        Public m_iFlag As Integer = 0
        Public m_pCenter As S実座標
        Public m_delta As S差分
        Public m_line As S線分

        '角の分岐用(#72)
        Public m_lineSub(3) As S線分


        '識別情報
        Friend ReadOnly Property Ident As String
            Get
                Return String.Format("({0}){1}", p_i番号, m_idx)
            End Get
        End Property

        Sub New(ByVal parent As CInsertItemList)
            _parent = parent
        End Sub

        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0} 記号({1}) ひも番号({2}) ひも数({3}) ひも長({4:f1}) ", Ident, m_s記号, m_iひも番号, m_iひも数, m_dひも長)
            sb.AppendFormat("ひも種({0}) 開始位置({1}) 長さ({2:f1}) ", m_iひも種, m_i開始位置, m_d長さ)
            sb.AppendFormat("iFlag({0}) pCenter{1} delta({2}) {3}", m_iFlag, m_pCenter, m_delta, m_line)
            Return sb.ToString
        End Function
    End Class

    '差しひものリスト
    Class CInsertItemList
        Inherits List(Of CInsertItem)

        '差しひもの1レコードに対応
        Friend _row As tbl差しひもRow
        '固定長の時 True
        Public Property IsFixedLength As Boolean = False

        Sub New(ByVal row As tbl差しひもRow)
            _row = row
        End Sub

        Function FindItem(ByVal idx As Integer) As CInsertItem
            For Each item As CInsertItem In Me
                If item.m_idx = idx Then
                    Return item
                End If
            Next
            Return Nothing
        End Function

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

    '固定長用。tbl差しひもRowから記号を転記したCInsertItemを生成
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
            lst.IsFixedLength = True
            Dim item As New CInsertItem(lst)
            item.m_dひも長 = row.f_dひも長
            item.m_iひも数 = row.f_iひも本数
            item.m_s記号 = row.f_s記号 '有無とも
            lst.Add(item)
            _InsertItemListMap.Add(row.f_i番号, lst)
            Return item
        End If
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendLine(_InsertItemListMap.ToString)
        Return sb.ToString
    End Function

End Class
