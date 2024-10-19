Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstMasterTables

Public Class frmBandType

    Dim _NameColumnIndex As Integer = -1
    Dim _WeightColumnIndex As Integer = -1
    Dim _ColorListColumnIndex As Integer = -1

    Dim _LaneCountColumnIndex As Integer = -1
    Dim _WidthColumnIndex As Integer = -1
    Dim _LaneWidthColumnIndex As Integer = -1
    Dim _table As tblバンドの種類DataTable

    Dim _MyProfile As New CDataGridViewProfile(
            (New tblバンドの種類DataTable),
            AddressOf CheckCell,
            enumAction._None
            )

    Private Sub frmBandType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        lbl設定時の寸法単位.Text = g_clsSelectBasics.p_unit設定時の寸法単位.ToString

        For Each col As DataGridViewColumn In dgvData.Columns
            If col.DataPropertyName = "f_sバンドの種類名" Then
                _NameColumnIndex = col.Index
            ElseIf col.DataPropertyName = "f_s長さと重さ" Then
                _WeightColumnIndex = col.Index
            ElseIf col.DataPropertyName = "f_s色リスト" Then
                _ColorListColumnIndex = col.Index
            ElseIf col.DataPropertyName = "f_i本幅" Then
                _LaneCountColumnIndex = col.Index
            ElseIf col.DataPropertyName = "f_dバンド幅" Then
                _WidthColumnIndex = col.Index
            ElseIf col.DataPropertyName = "f_s本幅の幅リスト" Then
                _LaneWidthColumnIndex = col.Index
            End If
        Next

        'テーブル表示
        BindingSourceバンドの種類.DataSource = Nothing
        _table = g_clsMasterTables.GetBandTypeTableCopy()
        BindingSourceバンドの種類.DataSource = _table
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

#If DEBUG Then
        Me.f_s本幅の幅リスト.ReadOnly = False
#End If

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        '#77
        For Each rec As tblバンドの種類Row In _table
            If rec.RowState <> DataRowState.Deleted Then '#81
                Dim err As String = Nothing
                If Not frmBandTypeWidth.Check幅リスト文字列(rec.f_i本幅, rec.f_dバンド幅, rec.f_s本幅の幅リスト, err) Then
                    'バンドの種類'{0}'の本幅の幅リストはエラーのため適用されません
                    Dim msg As String = String.Format(My.Resources.MsgBandTypeWidthErrSkip, rec.f_sバンドの種類名, err)
                    MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    selectBandRow(rec.f_sバンドの種類名)
                    Exit Sub
                End If
            End If
        Next

        If g_clsMasterTables.UpdateBandTypeTable(BindingSourceバンドの種類.DataSource) Then
            '対象バンドの種類名に対するマスターの再読み込み
            g_clsSelectBasics.UpdateTargetBandType(True)
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


    Private Sub btn複製_Click(sender As Object, e As EventArgs) Handles btn複製.Click
        Dim basename As String = Nothing
        If dgvData.CurrentRow IsNot Nothing AndAlso 0 <= dgvData.CurrentRow.Index Then
            basename = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_NameColumnIndex).Value
        Else
            Exit Sub 'スルー
        End If
        If String.IsNullOrWhiteSpace(txtバンドの種類名.Text) Then
            '{0}を指定してください。
            Dim msg As String = String.Format(My.Resources.ErrNewName, lblバンドの種類名.Text)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        '既存にないこと
        Dim cond As String = String.Format("f_sバンドの種類名='{0}'", txtバンドの種類名.Text)
        Dim rows() As tblバンドの種類Row = _table.Select(cond)
        If rows IsNot Nothing AndAlso 0 < rows.Count Then
            '既存の{0}<{1}>と同じにはできません。
            Dim msg As String = String.Format(My.Resources.ErrSameName, lblバンドの種類名.Text, txtバンドの種類名.Text)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        '複製元
        cond = String.Format("f_sバンドの種類名='{0}'", basename)
        rows = _table.Select(cond)
        If rows Is Nothing OrElse 0 = rows.Count Then
            Exit Sub '選択名がなければスルー
        End If

        '複製を追加
        Dim base As New clsDataRow(rows(0))
        Dim add As New clsDataRow(base)
        add.Value("f_sバンドの種類名") = txtバンドの種類名.Text.Trim
        _table.Rows.Add(add.DataRow)

        selectBandRow(add.Value("f_sバンドの種類名"))
    End Sub

    Private Sub btn追加_Click(sender As Object, e As EventArgs) Handles btn追加.Click
        If String.IsNullOrWhiteSpace(txtバンドの種類名.Text) Then
            '{0}を指定してください。
            Dim msg As String = String.Format(My.Resources.ErrNewName, lblバンドの種類名.Text)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        '既存にないこと
        Dim cond As String = String.Format("f_sバンドの種類名='{0}'", txtバンドの種類名.Text)
        Dim rows() As tblバンドの種類Row = _table.Select(cond)
        If rows IsNot Nothing AndAlso 0 < rows.Count Then
            '既存の{0}<{1}>と同じにはできません。
            Dim msg As String = String.Format(My.Resources.ErrSameName, lblバンドの種類名.Text, txtバンドの種類名.Text)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        '追加
        Dim add As tblバンドの種類Row = _table.Newtblバンドの種類Row
        add.f_sバンドの種類名 = txtバンドの種類名.Text.Trim
        _table.Rows.Add(add)

        selectBandRow(add.f_sバンドの種類名)
    End Sub

    Sub selectBandRow(ByVal bandname As String)
        For pos As Integer = 0 To _table.Rows.Count - 1
            If CType(BindingSourceバンドの種類.Item(pos).row, tblバンドの種類Row).f_sバンドの種類名 = bandname Then
                BindingSourceバンドの種類.Position = pos
                Exit For
            End If
        Next
    End Sub

    Private Sub btn長さと重さ_Click(sender As Object, e As EventArgs) Handles btn長さと重さ.Click
        If dgvData.CurrentRow Is Nothing OrElse dgvData.CurrentRow.Index < 0 Then
            Exit Sub
        End If
        Dim bandname As String = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_NameColumnIndex).Value
        If String.IsNullOrWhiteSpace(bandname) Then
            Exit Sub
        End If

        Dim dlg As New frmBandTypeWeight
        Try
            dlg.p_sバンドの種類名 = bandname
            dlg.p_s長さと重さ = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_WeightColumnIndex).Value
            If dlg.ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If

            Dim cond As String = String.Format("f_sバンドの種類名='{0}'", bandname)
            Dim rows() As tblバンドの種類Row = _table.Select(cond)
            If rows Is Nothing OrElse rows.Count <= 0 Then
                Exit Sub
            End If
            rows(0).f_s長さと重さ = dlg.p_s長さと重さ

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmBandTypeWeight")
        Finally
            dlg = Nothing
        End Try
    End Sub

    Private Sub dgvData_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 OrElse _NameColumnIndex < 0 OrElse _ColorListColumnIndex < 0 Then
            Exit Sub
        End If

        If e.ColumnIndex = dgvData.Columns("ColumnBotton").Index Then
            '色選択ボタンクリック
            Dim dlg As New frmColorList
            dlg.BandTypeName = dgvData.Rows(e.RowIndex).Cells(_NameColumnIndex).Value
            dlg.ColorString = dgvData.Rows(e.RowIndex).Cells(_ColorListColumnIndex).Value

            If dlg.ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            dgvData.Rows(e.RowIndex).Cells(_ColorListColumnIndex).Value = dlg.ColorString

        ElseIf e.ColumnIndex = dgvData.Columns("ColumnBotton本幅の幅").Index Then
            '本幅の幅ボタンクリック

            Dim dlg As New frmBandTypeWidth
            dlg.p_sバンドの種類名 = dgvData.Rows(e.RowIndex).Cells(_NameColumnIndex).Value
            dlg.p_i本幅 = dgvData.Rows(e.RowIndex).Cells(_LaneCountColumnIndex).Value
            dlg.p_dバンド幅 = dgvData.Rows(e.RowIndex).Cells(_WidthColumnIndex).Value
            dlg.p_s本幅の幅リスト = dgvData.Rows(e.RowIndex).Cells(_LaneWidthColumnIndex).Value

            If dlg.ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            dgvData.Rows(e.RowIndex).Cells(_LaneWidthColumnIndex).Value = dlg.p_s本幅の幅リスト

        End If
    End Sub
End Class