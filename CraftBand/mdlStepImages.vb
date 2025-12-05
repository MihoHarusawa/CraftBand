
Imports CraftBand.Tables.dstDataTables

'メインフォームの共通機能を定義するインターフェイス
Public Interface ICommonActions

    '列の表示
    Sub SetDrawOrder(ByVal show As Boolean)

    'プレビュー画像
    Function MakeImageFile(ByVal n As Integer, ByVal col As String, ByVal fpath As String, ByRef msg As String) As Boolean

    'プレビュー2画像
    Function MakeImageFile2(ByVal n As Integer, ByVal col As String, ByVal fpath As String, ByRef msg As String) As Boolean

End Interface


Public Module mdlStepImages

    '表示番号がnの時の色を変えるか
    Private Function no_disp(ByVal n As Integer, ByVal o表示順 As Object, ByVal o非表示順 As Object) As Boolean
        If n < 0 Then
            Return False 'nはゼロ以上の前提
        End If
        Dim i表示順 As Integer = 0
        If Not IsDBNull(o表示順) Then
            i表示順 = CInt(o表示順)
        End If
        Dim i非表示順 As Integer = 0
        If Not IsDBNull(o非表示順) Then
            i非表示順 = CInt(o非表示順)
        End If

        If i表示順 < 0 Then
            Return True '非表示
        ElseIf i表示順 = 0 Then
            Return False 'そのまま
        ElseIf n < i表示順 Then
            Return True '非表示
        ElseIf i表示順 = n Then
            Return False 'そのまま
        Else ' i表示順 < n
            If i非表示順 < n Then
                If i表示順 <= i非表示順 Then
                    Return True '非表示
                End If
            End If
            Return False 'そのまま
        End If
    End Function


    '表示がnの時、現Dataの色を変える
    Public Sub SetStepDispData(ByVal data As clsDataTables, ByVal n As Integer, ByVal no_color As String)
        With data
            '側面と縁
            For Each row As tbl側面Row In .p_tbl側面.Rows
                If no_disp(n, row.f_i表示順, row.f_i非表示順) Then
                    row.f_s色 = no_color
                End If
            Next

            '底_楕円
            For Each row As tbl底_楕円Row In .p_tbl底_楕円.Rows
                If no_disp(n, row.f_i表示順, row.f_i非表示順) Then
                    row.f_s色 = no_color
                End If
            Next

            '差しひも
            For Each row As tbl差しひもRow In .p_tbl差しひも.Rows
                If no_disp(n, row.f_i表示順, row.f_i非表示順) Then
                    row.f_s色 = no_color
                End If
            Next

            '追加品
            For Each row As tbl追加品Row In .p_tbl追加品.Rows
                If no_disp(n, row.f_i表示順, row.f_i非表示順) Then
                    row.f_s色 = no_color
                End If
            Next

            '縦ひもと横ひも
            For Each row As tbl縦横展開Row In .p_tbl縦横展開.Rows
                If no_disp(n, row.f_i表示順, row.f_i非表示順) Then
                    row.f_s色 = no_color
                End If
            Next
        End With
    End Sub

    '表示順の最大値を取得する
    Public Function GetMaxStepDisp(ByVal data As clsDataTables) As Integer
        Dim maxn As Integer = 0
        With data
            '側面と縁
            Dim max側面 As Integer? = .p_tbl側面.AsEnumerable() _
            .Select(Function(row) row.Field(Of Integer?)("f_i表示順")) _
            .Max()
            If max側面 IsNot Nothing AndAlso maxn < max側面.Value Then
                maxn = max側面.Value
            End If

            '底_楕円
            Dim max底_楕円 As Integer? = .p_tbl底_楕円.AsEnumerable() _
            .Select(Function(row) row.Field(Of Integer?)("f_i表示順")) _
            .Max()
            If max底_楕円 IsNot Nothing AndAlso maxn < max底_楕円.Value Then
                maxn = max底_楕円.Value
            End If

            '差しひも
            Dim max差しひも As Integer? = .p_tbl差しひも.AsEnumerable() _
            .Select(Function(row) row.Field(Of Integer?)("f_i表示順")) _
            .Max()
            If max差しひも IsNot Nothing AndAlso maxn < max差しひも.Value Then
                maxn = max差しひも.Value
            End If

            '追加品
            Dim max追加品 As Integer? = .p_tbl追加品.AsEnumerable() _
            .Select(Function(row) row.Field(Of Integer?)("f_i表示順")) _
            .Max()
            If max追加品 IsNot Nothing AndAlso maxn < max追加品.Value Then
                maxn = max追加品.Value
            End If

            '縦ひもと横ひも
            Dim max縦横展開 As Integer? = .p_tbl縦横展開.AsEnumerable() _
            .Select(Function(row) row.Field(Of Integer?)("f_i表示順")) _
            .Max()
            If max縦横展開 IsNot Nothing AndAlso maxn < max縦横展開.Value Then
                maxn = max縦横展開.Value
            End If
        End With

        Return maxn
    End Function

    '指定の表示順のレコード数を返す
    Public Function CountDispStepRecord(ByVal data As clsDataTables, ByVal n As Integer, ByRef memo As String) As Integer
        Dim count As Integer = 0
        Dim sb As New System.Text.StringBuilder
        With data
            '側面と縁
            For Each row As tbl側面Row In .p_tbl側面.Rows
                count += disp_count(n, row, sb)
            Next

            '底_楕円
            For Each row As tbl底_楕円Row In .p_tbl底_楕円.Rows
                count += disp_count(n, row, sb)
            Next

            '差しひも
            For Each row As tbl差しひもRow In .p_tbl差しひも.Rows
                count += disp_count(n, row, sb)
            Next

            '追加品
            For Each row As tbl追加品Row In .p_tbl追加品.Rows
                count += disp_count(n, row, sb)
            Next

            '縦ひもと横ひも
            For Each row As tbl縦横展開Row In .p_tbl縦横展開.Rows
                count += disp_count(n, row, sb)
            Next
        End With

        memo = sb.ToString()
        Return count
    End Function

    Private Function disp_count(ByVal n As Integer, ByVal row As DataRow, ByVal sb As Text.StringBuilder) As Integer
        If IsDBNull(row("f_i表示順")) Then
            If n = 0 Then
                Return 1
            End If
        ElseIf n = row("f_i表示順") Then
            If Not String.IsNullOrWhiteSpace(row("f_sメモ")) Then
                sb.AppendLine(row("f_sメモ"))
            End If
            Return 1
        End If
        Return 0
    End Function


End Module
