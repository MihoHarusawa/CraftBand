﻿Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstMasterTables
''' <summary>
''' tbl描画色
''' 各色の数値は255まで
''' </summary>
Public Class frmColor
    Dim cColDispColor As Integer = 5 '色を表示するカラム

    Shared _ColorFiledNames() As String = {"f_i赤", "f_i緑", "f_i青"}
    Dim _ColorFiledColumnIndex() As Integer

    Dim _table As tbl描画色DataTable
    Dim _NameColumnIndex As Integer = -1
    Dim _BandTypeNameIndex As Integer = -1

    Dim _MyProfile As New CDataGridViewProfile(
            (New tbl描画色DataTable),
            AddressOf CheckCell,
            enumAction._None
            )

    Dim _BandTypeNames() As String 'バンドの種類名の配列保持

    Private Sub frmColor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        ReDim _ColorFiledColumnIndex(_ColorFiledNames.Count - 1)
        For Each col As DataGridViewColumn In dgvData.Columns
            If col.DataPropertyName = "f_s色" Then
                _NameColumnIndex = col.Index
            ElseIf col.DataPropertyName = "f_sバンドの種類名" Then
                _BandTypeNameIndex = col.Index
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
        setBandTypeSelection(_table)
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

    Private Sub setBandTypeSelection(ByVal table As tbl描画色DataTable)
        'バンドの種類名の配列保持
        _BandTypeNames = g_clsMasterTables.GetBandTypeNames()

        Dim btypes As New List(Of String)
        btypes.Add(clsMasterTables.CommonColorBandType)
        btypes.AddRange(_BandTypeNames)

        'table参照外
        For Each r As tbl描画色Row In table
            Try
                Dim btype As String = r.f_sバンドの種類名
                If Not String.IsNullOrWhiteSpace(btype) Then
                    If Not btypes.Contains(btype) Then
                        btypes.Add(btype)
                    End If
                End If

            Catch ex As Exception
                g_clsLog.LogException(ex, "frmColor.setBandTypeSelection")
            End Try
        Next

        fsバンドの種類名ComboBoxColumn.Items.AddRange(btypes.ToArray)
        '
        cmbバンドの種類名.Items.Clear()
        cmbバンドの種類名.Items.AddRange(btypes.ToArray)
        cmbバンドの種類名.Text = clsMasterTables.CommonColorBandType
    End Sub

    Private Sub frmColor_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmColorGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmColorSize", Me.Size)
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If g_clsMasterTables.UpdateColorTable(_table) Then
            g_clsSelectBasics.UpdateTargetBandType(False) 'Update ColorTable
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
        If e.ColumnIndex = cColDispColor Then
            e.CellStyle.BackColor = gridColor(e.RowIndex)
            Exit Sub
        End If
        'バンドの種類名のセル
        If e.ColumnIndex = _BandTypeNameIndex Then
            Dim obj As Object = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            If IsDBNull(obj) Then 'Null不許可だが念のため
                e.Value = clsMasterTables.CommonColorBandType
                e.CellStyle.BackColor = Color.White
            Else
                Dim btype As String = obj.ToString
                If String.IsNullOrWhiteSpace(btype) Then
                    e.Value = clsMasterTables.CommonColorBandType
                    e.CellStyle.BackColor = Color.White
                ElseIf btype = clsMasterTables.CommonColorBandType OrElse _BandTypeNames.Contains(btype) Then
                    e.CellStyle.BackColor = Color.White
                Else
                    e.CellStyle.BackColor = Color.Gray
                End If
            End If
            Exit Sub
        End If
    End Sub

    Private Sub dgvData_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)

        '新しい行のセルは除外
        If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        '色のセル
        If _ColorFiledNames.Contains(dgv.Columns(e.ColumnIndex).DataPropertyName) Then
            dgv.Rows(e.RowIndex).Cells(cColDispColor).Style.BackColor = gridColor(e.RowIndex)
            Exit Sub
        End If
        'バンドの種類名のセル
        If e.ColumnIndex = _BandTypeNameIndex Then
            Dim obj As Object = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            If IsDBNull(obj) Then 'Null不許可だが念のため
                dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = clsMasterTables.CommonColorBandType
            End If
            Exit Sub
        End If
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

        Dim btype As String = Nothing
        If Not String.IsNullOrWhiteSpace(cmbバンドの種類名.Text) Then
            btype = cmbバンドの種類名.Text
        End If

        Dim rows() As tbl描画色Row = _table.Select(clsMasterTables.CondColorBandType(txt色.Text, btype))
        Dim row As tbl描画色Row
        If rows Is Nothing OrElse rows.Count = 0 Then
            '追加
            row = _table.NewRow
            row.f_s色 = txt色.Text.Trim
            row.f_sバンドの種類名 = btype
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
            If CType(BindingSource描画色.Item(pos).row, tbl描画色Row).f_s色 = row.f_s色 AndAlso
                CType(BindingSource描画色.Item(pos).row, tbl描画色Row).f_sバンドの種類名 = row.f_sバンドの種類名 Then
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
            cmbバンドの種類名.Text = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_BandTypeNameIndex).Value
        End If
    End Sub
End Class