Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Window
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstMasterTables

Public Class frmBandType

    Dim _MyProfile As New CDataGridViewProfile(
            (New tblバンドの種類DataTable),
            AddressOf CheckCell,
            enumAction._None
            )

    Private Sub frmBandType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        lbl設定時の寸法単位.Text = g_clsSelectBasics.p_unit設定時の寸法単位.ToString


        'テーブル表示
        BindingSourceバンドの種類.DataSource = g_clsMasterTables.GetBandTypeTableCopy()
        dgvData.Refresh()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmBandTypeSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmBandTypeGrid", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If g_clsMasterTables.UpdateBandTypeTable(BindingSourceバンドの種類.DataSource) Then
            '対象バンドの種類名に対するマスターの再読み込み
            g_clsSelectBasics.UpdateTargetBandType()
            Me.DialogResult = DialogResult.OK 'Changed, Need Update
        Else
            Me.DialogResult = DialogResult.Cancel 'No Change
        End If
        Me.Close()
    End Sub

    Private Sub frmBandType_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmBandTypeGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmBandTypeSize", Me.Size)
    End Sub

    'Private Sub dgvData_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvData.DataError
    '    dgv_DataErrorCancel(sender, e, Me.Text)
    'End Sub

    'セル値のチェック
    Private Function CheckCell(ByVal DataPropertyName As String, ByVal value As Object) As String
        If DataPropertyName = "f_i本幅" Then
            Dim i As Integer
            If Integer.TryParse(value, i) AndAlso 1 <= i Then
                Return Nothing 'OK
            End If
            '{0}以上の数値を設定してください。
            Dim msg As String = String.Format(My.Resources.ErrMsgTooSmall, 1)
            Return msg

        ElseIf DataPropertyName = "f_dバンド幅" Then
            Dim d As Double
            If Double.TryParse(value, d) AndAlso 0 < d Then
                Return Nothing 'OK
            End If
            '{0}以上の数値を設定してください。
            Dim msg As String = String.Format(My.Resources.ErrMsgTooSmall, 0)
            Return msg

        End If
        Return Nothing
    End Function



    'Private Sub dgvData_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvData.CellValidating
    '    Dim dgv As DataGridView = CType(sender, DataGridView)

    '    '新しい行のセルは除外
    '    If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty Then
    '        Exit Sub
    '    End If

    '    If dgv.Columns(e.ColumnIndex).DataPropertyName = "f_i本幅" Then
    '        Dim i As Integer
    '        If Integer.TryParse(e.FormattedValue.ToString(), i) AndAlso 1 <= i Then
    '            Exit Sub 'OK
    '        End If
    '        '{0}行目{1}値には{2}以上の数値を設定してください。
    '        Dim msg As String = String.Format(My.Resources.ErrGridMinimumValue, e.RowIndex + 1, dgv.Columns(e.ColumnIndex).HeaderText, 1)
    '        MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '        dgv.CancelEdit()
    '        e.Cancel = True

    '    ElseIf dgv.Columns(e.ColumnIndex).DataPropertyName = "f_dバンド幅" Then
    '        Dim d As Double
    '        If Double.TryParse(e.FormattedValue.ToString(), d) AndAlso 0 < d Then
    '            Exit Sub 'OK
    '        End If
    '        '{0}行目{1}値には{2}以上の数値を設定してください。
    '        Dim msg As String = String.Format(My.Resources.ErrGridMinimumValue, e.RowIndex + 1, dgv.Columns(e.ColumnIndex).HeaderText, 0)
    '        MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '        dgv.CancelEdit()
    '        e.Cancel = True

    '    End If

    'End Sub
End Class