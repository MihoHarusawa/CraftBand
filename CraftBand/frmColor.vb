Imports System.Drawing
Imports System.Security.Permissions
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstMasterTables
''' <summary>
''' tbl描画色
''' 各色の数値は255まで
''' </summary>
Public Class frmColor
    Shared _ColorFiledNames() As String = {"f_i赤", "f_i緑", "f_i青"}

    Dim _MyProfile As New CDataGridViewProfile(
            (New tbl描画色DataTable),
            AddressOf CheckCell,
            enumAction._None
            )

    Dim _table As tbl描画色DataTable
    Dim cColDispColor As Integer = 4 '色を表示するカラム

    Private Sub frmColor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        BindingSource描画色.DataSource = Nothing
        _table = g_clsMasterTables.GetColorTableCopy()
        BindingSource描画色.DataSource = _table

        dgvData.Refresh()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmColorSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmColorGrid", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If

    End Sub

    Private Sub frmColor_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmColorGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmColorSize", Me.Size)
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If g_clsMasterTables.UpdateColorTable(_table) Then
            Me.DialogResult = DialogResult.OK 'Changed, Need Update
        Else
            Me.DialogResult = DialogResult.Cancel 'No Change
        End If
        Me.Close()
    End Sub


    'セル値のチェック
    Function CheckCell(ByVal DataPropertyName As String, ByVal value As Object) As String

        '色と透明度
        If _ColorFiledNames.Contains(DataPropertyName) OrElse
            DataPropertyName = "f_i透明度" Then
            Dim v As Integer
            If Integer.TryParse(value, v) AndAlso 0 <= v AndAlso v <= clsMasterTables.MaxRgbValue Then
                Return Nothing 'OK
            End If
            '{0}以下の数値を設定してください。
            Dim msg As String = String.Format(My.Resources.ErrMsgTooLarge, clsMasterTables.MaxRgbValue)
            Return msg
        End If

        Return Nothing
    End Function

    'Private Sub dgvData_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvData.CellValidating
    '    Dim dgv As DataGridView = CType(sender, DataGridView)

    '    '新しい行のセルは除外
    '    If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty OrElse e.ColumnIndex < 0 Then
    '        Exit Sub
    '    End If

    '    '色と透明度
    '    If _ColorFiledNames.Contains(dgv.Columns(e.ColumnIndex).DataPropertyName) OrElse
    '        dgv.Columns(e.ColumnIndex).DataPropertyName = "f_i透明度" Then
    '        Dim v As Integer
    '        If Integer.TryParse(e.FormattedValue.ToString(), v) AndAlso 0 <= v AndAlso v <= clsMasterTables.MaxRgbValue Then
    '            Exit Sub 'OK
    '        End If
    '        '{0}行目{1}値には{2}以下の数値を設定してください。
    '        Dim msg As String = String.Format(My.Resources.ErrGridMaximumValue, e.RowIndex + 1, dgv.Columns(e.ColumnIndex).HeaderText, clsMasterTables.MaxRgbValue)
    '        MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '        dgv.CancelEdit()
    '        e.Cancel = True
    '    End If

    'End Sub

    'Private Sub dgvData_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvData.DataError
    '    dgv_DataErrorCancel(sender, e, Me.Text)
    'End Sub

    Private Sub dgvData_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvData.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        '新しい行のセルは除外
        If e.RowIndex = dgv.NewRowIndex Then
            Exit Sub
        End If
        '色表示のセル
        If e.ColumnIndex <> cColDispColor Then
            Exit Sub
        End If

        'その行の各色値
        Dim r As Object = 0
        Dim g As Object = 0
        Dim b As Object = 0
        For Each col As DataGridViewColumn In dgv.Columns
            If col.DataPropertyName = "f_i赤" Then
                r = dgv.Rows(e.RowIndex).Cells(col.Index).Value
            ElseIf col.DataPropertyName = "f_i緑" Then
                g = dgv.Rows(e.RowIndex).Cells(col.Index).Value
            ElseIf col.DataPropertyName = "f_i青" Then
                b = dgv.Rows(e.RowIndex).Cells(col.Index).Value
            End If
        Next
        e.CellStyle.BackColor = clsMasterTables.RgbColor(r, g, b)
    End Sub

    Private Sub dgvData_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)

        '新しい行のセルは除外
        If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        '色のセル
        If Not _ColorFiledNames.Contains(dgv.Columns(e.ColumnIndex).DataPropertyName) Then
            Exit Sub
        End If

        'その行の各色値
        Dim r As Object = 0
        Dim g As Object = 0
        Dim b As Object = 0
        For Each col As DataGridViewColumn In dgv.Columns
            If col.DataPropertyName = "f_i赤" Then
                r = dgv.Rows(e.RowIndex).Cells(col.Index).Value
            ElseIf col.DataPropertyName = "f_i緑" Then
                g = dgv.Rows(e.RowIndex).Cells(col.Index).Value
            ElseIf col.DataPropertyName = "f_i青" Then
                b = dgv.Rows(e.RowIndex).Cells(col.Index).Value
            End If
        Next
        dgv.Rows(e.RowIndex).Cells(cColDispColor).Style.BackColor = clsMasterTables.RgbColor(r, g, b)
    End Sub
End Class