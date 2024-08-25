Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstMasterTables

Public Class frmUpDownPattern

    Dim _table As tbl上下図DataTable
    Dim _NameColumnIndex As Integer = -1
    Dim _SubNameHeaderText As String = Nothing

    Dim _MyProfile As New CDataGridViewProfile(
            (New tbl上下図DataTable),
            Nothing,
            enumAction._None
            )

    Private Sub frmUpDownPattern_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        _table = g_clsMasterTables.GetUpDownTableCopy()
        BindingSource上下図.DataSource = _table
        BindingSource上下図.Sort = "f_s上下図名"

        dgvData.Refresh()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmUpDownPatternSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmUpDownPatternGrid", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If g_clsMasterTables.UpdateUpDownTable(_table) Then
            g_clsSelectBasics.UpdateTargetBandType(False)
            Me.DialogResult = DialogResult.OK 'Changed, Need Update
        Else
            Me.DialogResult = DialogResult.Cancel 'No Change
        End If
        Me.Close()
    End Sub

    Private Sub btn削除_Click(sender As Object, e As EventArgs) Handles btn削除.Click
        Dim current As System.Data.DataRowView = BindingSource上下図.Current
        If current Is Nothing OrElse current.Row Is Nothing Then
            Exit Sub
        End If

        Dim row As tbl上下図Row = CType(current.Row, tbl上下図Row)
        Dim name As String = row.f_s上下図名
        Dim dels() As DataRow
        dels = _table.Select(String.Format("f_s上下図名='{0}'", name))
        If dels Is Nothing OrElse dels.Count = 0 Then
            Exit Sub
        End If
        ''{0}'をすべて削除します。よろしいですか？
        Dim msg As String = String.Format(My.Resources.AskConfirmDeleteGroup, name)
        Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        For Each del In dels
            _table.Rows.Remove(del)
        Next
    End Sub

    Private Sub frmUpDownPattern_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmUpDownPatternGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmUpDownPatternSize", Me.Size)
    End Sub
End Class