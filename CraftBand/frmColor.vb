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
    Dim cColDispColor As Integer = 4 '色を表示するカラム

    Shared _ColorFiledNames() As String = {"f_i赤", "f_i緑", "f_i青"}
    Dim _ColorFiledColumnIndex() As Integer

    Dim _table As tbl描画色DataTable
    Dim _NameColumnIndex As Integer = -1

    Dim _MyProfile As New CDataGridViewProfile(
            (New tbl描画色DataTable),
            AddressOf CheckCell,
            enumAction._None
            )

    Private Sub frmColor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        ReDim _ColorFiledColumnIndex(_ColorFiledNames.Count - 1)
        For Each col As DataGridViewColumn In dgvData.Columns
            If col.DataPropertyName = "f_s色" Then
                _NameColumnIndex = col.Index
            ElseIf col.DataPropertyName = _ColorFiledNames(0) Then
                _ColorFiledColumnIndex(0) = col.Index
            ElseIf col.DataPropertyName = _ColorFiledNames(1) Then
                _ColorFiledColumnIndex(1) = col.Index
            ElseIf col.DataPropertyName = _ColorFiledNames(2) Then
                _ColorFiledColumnIndex(2) = col.Index
            End If
        Next

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
        If _ColorFiledNames.Contains(DataPropertyName) OrElse DataPropertyName = "f_i透明度" Then
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
        e.CellStyle.BackColor = gridColor(e.RowIndex)
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

        dgv.Rows(e.RowIndex).Cells(cColDispColor).Style.BackColor = gridColor(e.RowIndex)
    End Sub

    Private Function gridColor(ByVal rowidx As Integer) As Color
        Dim r As Object = dgvData.Rows(rowidx).Cells(_ColorFiledColumnIndex(0)).Value '"f_i赤"
        Dim g As Object = dgvData.Rows(rowidx).Cells(_ColorFiledColumnIndex(1)).Value '"f_i緑"
        Dim b As Object = dgvData.Rows(rowidx).Cells(_ColorFiledColumnIndex(2)).Value '"f_i青"
        Return clsMasterTables.RgbColor(r, g, b)
    End Function

    Private Sub nud_ValueChanged(sender As Object, e As EventArgs) Handles nud赤.ValueChanged, nud緑.ValueChanged, nud青.ValueChanged
        Dim r As Object = nud赤.Value
        Dim g As Object = nud緑.Value
        Dim b As Object = nud青.Value
        lblColor.BackColor = clsMasterTables.RgbColor(r, g, b)
    End Sub

    Private Sub btn追加_Click(sender As Object, e As EventArgs) Handles btn追加.Click
        If String.IsNullOrWhiteSpace(txt色.Text) Then
            '{0}を指定してください。
            Dim msg As String = String.Format(My.Resources.ErrNewName, lbl色.Text)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If


        Dim cond As String = String.Format("f_s色='{0}'", txt色.Text)
        Dim rows() As tbl描画色Row = _table.Select(cond)
        Dim row As tbl描画色Row
        If rows Is Nothing OrElse rows.Count = 0 Then
            '追加
            row = _table.NewRow
            row.f_s色 = txt色.Text.Trim
            row.f_i赤 = nud赤.Value
            row.f_i緑 = nud緑.Value
            row.f_i青 = nud青.Value
            _table.Rows.Add(row)
        Else
            '既存の変更
            row = rows(0)
            row.f_i赤 = nud赤.Value
            row.f_i緑 = nud緑.Value
            row.f_i青 = nud青.Value
        End If

        For pos As Integer = 0 To _table.Rows.Count - 1
            If CType(BindingSource描画色.Item(pos).row, tbl描画色Row).f_s色 = row.f_s色 Then
                BindingSource描画色.Position = pos
                dgvData.Refresh()
                Exit For
            End If
        Next

    End Sub

    Private Sub dgvData_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvData.CurrentCellChanged
        If dgvData.CurrentRow IsNot Nothing AndAlso 0 <= dgvData.CurrentRow.Index Then
            txt色.Text = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_NameColumnIndex).Value
            nud赤.Value = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_ColorFiledColumnIndex(0)).Value
            nud緑.Value = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_ColorFiledColumnIndex(1)).Value
            nud青.Value = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_ColorFiledColumnIndex(2)).Value
        End If
    End Sub
End Class