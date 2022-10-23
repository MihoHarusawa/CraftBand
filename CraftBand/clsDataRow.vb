Public Class clsDataRow
    Implements IEquatable(Of clsDataRow)
    Implements IEquatable(Of DataRow)

    Dim _DataRow As DataRow
    Public ReadOnly Property DataRow() As DataRow
        Get
            Return _DataRow
        End Get
    End Property

    Public ReadOnly Property IsValid() As Boolean
        Get
            Return _DataRow IsNot Nothing AndAlso _DataRow.Table IsNot Nothing
        End Get
    End Property

    Sub New(ByVal row As DataRow)
        _DataRow = row
    End Sub

    Sub New(ByVal ref As clsDataRow)
        _DataRow = ref._DataRow.Table.NewRow
        SetValuesFrom(ref)
    End Sub

    Public Function Clone() As clsDataRow
        Dim copy As New clsDataRow(Me)
        Return copy
    End Function

    Public Property Value(ByVal name As String) As Object
        Get
            If String.IsNullOrWhiteSpace(name) OrElse Not IsValid Then
                Return Nothing 'Never
            End If
            If _DataRow.Table.Columns.Contains(name) Then
                Dim obj As Object = _DataRow(name)
                Dim col As DataColumn = _DataRow.Table.Columns(name)
                If IsDBNull(obj) Then
                    'NullValue値はとれないので、'空の値'を返す
                    Select Case col.DataType.ToString
                        Case "System.String"
                            Return ""
                        Case "System.Int16", "System.Int32"
                            Return 0
                        Case "System.Double"
                            Return 0
                        Case "System.Boolean"
                            Return False
                        Case Else
                            '未定義のDataType
                            g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_DataType", _DataRow.Table.TableName, name, col.DataType)
                            Return 0
                    End Select

                End If
                Select Case col.DataType.ToString
                    Case "System.String"
                        Return CType(obj, String)
                    Case "System.Int16", "System.Int32"
                        Return CType(obj, Integer)
                    Case "System.Double"
                        Return CType(obj, Double)
                    Case "System.Boolean"
                        Return CType(obj, Boolean)
                    Case Else
                        '未定義のDataType
                        g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_DataType", _DataRow.Table.TableName, name, col.DataType)
                        Return obj 'as is
                End Select

            Else
                '未定義のフィールド名
                g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_FieldName", _DataRow.Table.TableName, name)
                Dim mark As String = name.Substring(2, 1) '3文字目
                Select Case mark
                    Case "s"
                        Return String.Empty
                    Case "i"
                        Return 0
                    Case "d"
                        Return 0.0
                    Case "b"
                        Return False
                    Case Else
                        Return Nothing 'Never
                End Select

            End If

        End Get
        Set(value As Object)
            If String.IsNullOrWhiteSpace(name) OrElse Not IsValid Then
                Return  'Never
            End If
            If _DataRow.Table.Columns.Contains(name) Then
                _DataRow(name) = value

            Else
                '未定義のフィールド名
                g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_FieldName", _DataRow.Table.TableName, name, value)

            End If
        End Set


    End Property

    Public ReadOnly Property HasValue(ByVal name As String) As Boolean
        Get
            If String.IsNullOrWhiteSpace(name) OrElse Not IsValid Then
                Return False 'Never
            End If
            If _DataRow.Table.Columns.Contains(name) Then
                Dim obj As Object = _DataRow(name)
                Dim col As DataColumn = _DataRow.Table.Columns(name)
                If IsDBNull(obj) Then
                    Return False
                End If
                Select Case col.DataType.ToString
                    Case "System.String"
                        Return 0 < obj.ToString.Length
                    Case "System.Int16", "System.Int32", "System.Double", "System.Boolean"
                        Return True
                    Case Else
                        '未定義のDataType
                        g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_DataType", _DataRow.Table.TableName, name, col.DataType)
                        Return False
                End Select

            Else
                '未定義のフィールド名
                g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_FieldName", _DataRow.Table.TableName, name)
                Return False

            End If
        End Get
    End Property

    '初期値に戻す
    Public Sub SetDefaultAll()
        If Not IsValid Then
            Return  'Never
        End If

        For Each col As DataColumn In _DataRow.Table.Columns
            _DataRow(col) = col.DefaultValue
        Next
        _DataRow.ClearErrors()
    End Sub

    'Null値にDefault値をセットする(フィールドの異なるXMLファイル読み取り対策)
    Public Function SetDefaultForNull() As Boolean
        If Not IsValid Then
            Return False 'Never
        End If

        Dim changed As Boolean = False
        For Each col As DataColumn In _DataRow.Table.Columns
            If IsDBNull(_DataRow(col)) Then
                If Not IsDBNull(col.DefaultValue) Then
                    _DataRow(col) = col.DefaultValue
                    changed = True
                End If
            End If
        Next
        _DataRow.ClearErrors()
        Return changed
    End Function

    Public Function SetValuesFrom(ByVal other As clsDataRow) As Boolean
        'otherがMeと同じ値を持つ
        If Not IsValid OrElse Not other.IsValid Then
            Throw New Exception("clsDataRow.SetValuesFrom:Copy No Valid Rows")
        End If
        For Each col As DataColumn In _DataRow.Table.Columns
            _DataRow(col.ColumnName) = other._DataRow(col.ColumnName)
        Next
        Return True
    End Function

    Public Function dump() As String
        If Not IsValid Then
            Return Nothing
        End If
        Dim sb As New System.Text.StringBuilder
        sb.Append(_DataRow.Table.TableName).AppendLine()
        For Each col As DataColumn In _DataRow.Table.Columns
            Try
                sb.Append(col.ColumnName).Append(vbTab)
                If IsDBNull(_DataRow(col)) Then
                    sb.Append("<DBNull>")
                Else
                    sb.Append(_DataRow(col))
                End If
                sb.AppendLine()
            Catch ex As Exception
                sb.AppendLine("-" & vbTab & "-") 'table内の削除レコード
            End Try
        Next
        sb.Append("RowState").Append(vbTab)
        If _DataRow.RowState And DataRowState.Added Then
            sb.Append("Added ")
        End If
        If _DataRow.RowState And DataRowState.Deleted Then
            sb.Append("Deleted ")
        End If
        If _DataRow.RowState And DataRowState.Detached Then
            sb.Append("Detached ")
        End If
        If _DataRow.RowState And DataRowState.Modified Then
            sb.Append("Modified ")
        End If
        If _DataRow.RowState And DataRowState.Unchanged Then
            sb.Append("Unchanged ")
        End If
        sb.AppendLine()

        Return sb.ToString
    End Function

    Public Overrides Function ToString() As String
        If Not IsValid Then
            Return Nothing
        End If
        Dim sb As New System.Text.StringBuilder
        For Each col As DataColumn In _DataRow.Table.Columns
            Try
                If IsDBNull(_DataRow(col)) Then
                    sb.Append("<DBNull>")
                Else
                    sb.Append(_DataRow(col))
                End If
            Catch ex As Exception
                sb.Append("-") 'table内の削除レコード
            End Try
            sb.Append(",")
        Next
        If _DataRow.RowState And DataRowState.Added Then
            sb.Append("Added ")
        End If
        If _DataRow.RowState And DataRowState.Deleted Then
            sb.Append("Deleted ")
        End If
        If _DataRow.RowState And DataRowState.Detached Then
            sb.Append("Detached ")
        End If
        If _DataRow.RowState And DataRowState.Modified Then
            sb.Append("Modified ")
        End If
        If _DataRow.RowState And DataRowState.Unchanged Then
            sb.Append("Unchanged ")
        End If
        sb.AppendLine()

        Return sb.ToString
    End Function

    Public Function ToStringHeader() As String
        If Not IsValid Then
            Return Nothing
        End If
        Dim sb As New System.Text.StringBuilder
        sb.Append(_DataRow.Table.TableName).AppendLine()
        For Each col As DataColumn In _DataRow.Table.Columns
            sb.Append(col.ColumnName).Append(",")
        Next
        sb.Append("RowState")
        sb.AppendLine()

        Return sb.ToString
    End Function

    Public Overloads Function Equals(other As clsDataRow) As Boolean Implements IEquatable(Of clsDataRow).Equals
        'otherがMeと同じ値を持つ
        If Not IsValid OrElse Not other.IsValid Then
            Throw New Exception("clsDataRow.Equals:Compare No Valid Rows")
        End If
        For Each col As DataColumn In _DataRow.Table.Columns
            Dim myval As Object = _DataRow(col.ColumnName)
            Dim otherval As Object = other._DataRow(col.ColumnName)
            If IsDBNull(otherval) Then
                If IsDBNull(myval) Then
                    Continue For
                Else
                    Return False
                End If
            Else
                If IsDBNull(myval) Then
                    Return False
                Else
                    If myval <> otherval Then
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "not Equals:{0} {1} {2}", col.ColumnName, myval, otherval)
                        Return False
                    End If
                End If
            End If
        Next
        Return True
    End Function

    Public Overloads Function Equals(other As DataRow) As Boolean Implements IEquatable(Of DataRow).Equals
        Return Equals(New clsDataRow(other))
    End Function
End Class
