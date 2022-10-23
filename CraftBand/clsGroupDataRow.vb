''' <summary>
''' インデックス値で識別される(同じメインキーを持つ)レコードのグループ
''' </summary>
Public Class clsGroupDataRow
    Implements IEnumerable

    Dim _fieldNameKey As String = Nothing
    Dim _map As New SortedDictionary(Of Int16, clsDataRow)
    Dim _isValid As Boolean = False

    ReadOnly Property IsValid As Boolean
        Get
            Return _isValid AndAlso Not String.IsNullOrWhiteSpace(_fieldNameKey)
        End Get
    End Property

    ReadOnly Property Keys As Int16()
        Get
            Return _map.Keys.ToArray
        End Get
    End Property

    'インデックス値のフィールド名のみを指定
    Sub New(ByVal fldname As String)
        _fieldNameKey = fldname
    End Sub

    'インデックス値のフィールド名と対象の複数レコード
    Sub New(ByVal rows() As DataRow, ByVal fldname As String)
        Try
            _fieldNameKey = fldname
            If rows IsNot Nothing AndAlso 0 < rows.Count Then
                For Each row As DataRow In rows
                    Dim idx As Int16 = row(_fieldNameKey)
                    _map.Add(idx, New clsDataRow(row))
                Next
                _isValid = True
            End If

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsGroupDataRow.New")
        End Try
    End Sub

    Sub New(ByVal table As DataTable)
        Dim idx As Integer = 1
        For Each row As DataRow In table.Rows
            Add(idx, row)
            idx += 1
        Next
    End Sub

    '複製
    Sub New(ByVal ref As clsGroupDataRow)
        For Each idx As Int16 In ref._map.Keys
            _map.Add(idx, ref._map(idx).Clone)
        Next
        _isValid = ref._isValid
        _fieldNameKey = ref._fieldNameKey
    End Sub

    'レコード追加(フィールド名からインデックス値取得)
    Sub Add(ByVal row As DataRow)
        Try
            Dim drow As New clsDataRow(row)
            Dim idx As Int16 = drow.Value(_fieldNameKey)
            If Not _map.ContainsKey(idx) Then
                _map.Add(idx, drow)
            End If
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsGroupDataRow.Add")
        End Try
    End Sub

    'インデックス値を指定してレコード追加
    Sub Add(ByVal idx As Integer, ByVal row As DataRow)
        If Not _map.ContainsKey(idx) Then
            _map.Add(idx, New clsDataRow(row))
        End If
    End Sub

    'レコード数
    ReadOnly Property Count As Integer
        Get
            Return _map.Count
        End Get
    End Property

    '指定インデックス値のレコード取得
    Function IndexDataRow(ByVal idx As Int16) As clsDataRow
        If _map.ContainsKey(idx) Then
            Return _map(idx)
        Else
            Return Nothing
        End If
    End Function

    '指定レコードと同じインデックス値のレコード取得
    Function IndexDataRow(ByVal row As clsDataRow) As clsDataRow
        Dim idx As Int16 = row.Value(_fieldNameKey)
        Return IndexDataRow(idx)
    End Function

    '指定名のフィールド値を返す(最初のレコードから)
    Function GetNameValue(ByVal name As String) As Object
        If Me.Count = 0 Then
            Return Nothing
        End If
        Dim kv As KeyValuePair(Of Int16, clsDataRow) = _map.First
        Return kv.Value.Value(name)
    End Function

    '指定名のフィールド値をセットする(全レコード同じ)
    Function SetNameValue(ByVal name As String, ByVal value As Object) As Boolean
        For Each drow As clsDataRow In _map.Values
            drow.Value(name) = value
        Next
        Return True
    End Function

    '指定名のフィールドに、参照グループの同じIndex・指定名の値をセットする
    Function SetNameIndexValue(ByVal name As String, ByVal groupRef As clsGroupDataRow, Optional ByVal refName As String = Nothing) As Boolean
        Dim ret As Boolean = True
        For Each idx As Int16 In _map.Keys
            Dim dref As clsDataRow = groupRef.IndexDataRow(idx)
            If dref Is Nothing Then
                ret = False
                _map(idx).Value(name) = DBNull.Value
            Else
                If Not String.IsNullOrWhiteSpace(refName) Then
                    _map(idx).Value(name) = dref.Value(refName)
                Else
                    _map(idx).Value(name) = dref.Value(name)
                End If
            End If
        Next
        Return ret
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("{0}({1}){2}", Me.GetType.Name, IIf(IsValid, "Valid", "InValid"), _fieldNameKey).AppendLine()
        Dim first As Boolean = True
        For Each idx In Keys
            If first Then
                sb.Append(IndexDataRow(idx).ToStringHeader())
                first = False
            End If
            sb.Append(IndexDataRow(idx).ToString())
        Next
        Return sb.ToString
    End Function

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return New CArrayEnumerator(Of clsDataRow)(_map.Values().ToArray)
    End Function
End Class

Class CArrayEnumerator(Of Teach)
    Implements System.Collections.IEnumerator
    Private Property values As Object()
    Private Property cursor As Integer
    Sub New(ByVal vals As Object())
        values = vals
        Reset()
    End Sub
    Function MoveNext() As Boolean Implements IEnumerator.MoveNext
        If values Is Nothing Then
            Return False
        End If
        If cursor < values.Count - 1 Then
            cursor += 1
            Return True
        Else
            Return False
        End If
    End Function
    ReadOnly Property Current As Object Implements IEnumerator.Current
        Get
            If values IsNot Nothing Then
                Return CType(values(cursor), Teach)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Sub Reset() Implements IEnumerator.Reset
        cursor = -1  'Incして0
    End Sub
End Class



