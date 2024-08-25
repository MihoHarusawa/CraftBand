Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstMasterTables

''' <summary>
''' tbl編みかた
''' </summary>
Public Class frmPattern

    Dim _table As tbl編みかたDataTable
    Dim _NameColumnIndex As Integer = -1
    Dim _SubNameHeaderText As String = Nothing

    Dim _MyProfile As New CDataGridViewProfile(
            (New tbl編みかたDataTable),
            Nothing,
            enumAction._RowHeight_iひも番号_BackColor Or enumAction._CheckBoxGray_iひも番号
            )

    Private Sub frmPattern_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)


        For Each col As DataGridViewColumn In dgvData.Columns
            If col.DataPropertyName = "f_s編みかた名" Then
                _NameColumnIndex = col.Index
            ElseIf col.DataPropertyName = "f_s編みひも名" Then
                _SubNameHeaderText = col.HeaderText
            End If
            If 0 <= _NameColumnIndex AndAlso Not String.IsNullOrEmpty(_SubNameHeaderText) Then
                Exit For
            End If
        Next

        lbl設定時の寸法単位.Text = g_clsSelectBasics.p_unit設定時の寸法単位.ToString

        BindingSource編みかた.Sort = Nothing
        BindingSource編みかた.DataSource = Nothing

        _table = g_clsMasterTables.GetPatternTableCopy()
        BindingSource編みかた.DataSource = _table
        'BindingSource編みかた.Sort = "f_b縁専用区分 , f_s編みかた名 , f_iひも番号"
        BindingSource編みかた.Sort = "f_s編みかた名 , f_iひも番号"

        dgvData.Refresh()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmPatternSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmPatternGrid", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If g_clsMasterTables.UpdatePatternTable(_table) Then
            g_clsSelectBasics.UpdateTargetBandType(False)
            Me.DialogResult = DialogResult.OK 'Changed, Need Update
        Else
            Me.DialogResult = DialogResult.Cancel 'No Change
        End If
        Me.Close()
    End Sub

    Private Sub frmPattern_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmPatternGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmPatternSize", Me.Size)
    End Sub

    Private Sub dgvData_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvData.CurrentCellChanged
        If dgvData.CurrentRow IsNot Nothing AndAlso 0 <= dgvData.CurrentRow.Index Then
            txt編みかた名.Text = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_NameColumnIndex).Value
        End If
    End Sub

    Private Sub btnひも追加_Click(sender As Object, e As EventArgs) Handles btnひも追加.Click
        If String.IsNullOrWhiteSpace(txt編みかた名.Text) Then
            '新しい/既存の{0}を指定してください。
            Dim msg As String = String.Format(My.Resources.ErrNoName, lbl編みかた名.Text)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim add As clsDataRow = Nothing
        Dim exist As clsDataRow = findRecord(txt編みかた名.Text)
        If exist Is Nothing Then
            '新たに'{0}'を追加します。よろしいですか？
            Dim msg As String = String.Format(My.Resources.AskConfirmAdd, txt編みかた名.Text)
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
            add = setNewRecord(txt編みかた名.Text)
        Else
            '既存の'{0}'に{1}を追加します。よろしいですか？
            Dim msg As String = String.Format(My.Resources.AskConfirmAppend, txt編みかた名.Text, _SubNameHeaderText)
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
            add = setNextRecord(exist)
        End If
        If add Is Nothing OrElse Not add.IsValid Then
            'レコードを追加できませんでした。
            MessageBox.Show(My.Resources.ErrAddRecord, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        _table.Rows.Add(add.DataRow)
        setBindingSourcePosition(add)
    End Sub

    Private Function findRecord(ByVal s編みかた名 As String) As clsDataRow
        Dim cond As String = String.Format("f_s編みかた名='{0}'", s編みかた名)
        Dim rows() As tbl編みかたRow = _table.Select(cond, "f_iひも番号 DESC")
        If rows Is Nothing OrElse rows.Count = 0 Then
            Return Nothing
        End If
        Return New clsDataRow(rows(0)) '最大ひも番号
    End Function

    Private Function setNewRecord(ByVal s編みかた名 As String) As clsDataRow
        Dim add As New clsDataRow(_table.Newtbl編みかたRow)
        add.Value("f_s編みかた名") = s編みかた名
        add.Value("f_s編みひも名") = _SubNameHeaderText
        add.Value("f_iひも番号") = 1
        add.Value("f_b底使用区分") = False

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "setNewRecord {0}", add.dump())
        Return add
    End Function

    Private Function setNextRecord(ByVal exist As clsDataRow) As clsDataRow
        If Not exist.IsValid OrElse Not exist.HasValue("f_s編みかた名") Then
            Return Nothing
        End If
        Dim cond As String = String.Format("f_s編みかた名='{0}'", exist.Value("f_s編みかた名"))
        Dim nextidx As Integer = CType(_table.Compute("Max(f_iひも番号)", cond), Integer) + 1

        Dim add As New clsDataRow(exist)
        add.Value("f_s編みひも名") = exist.Value("f_s編みひも名") & nextidx.ToString
        add.Value("f_iひも番号") = nextidx

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "setNextRecord {0}", add.dump())
        Return add
    End Function

    Private Sub setBindingSourcePosition(ByVal drow As clsDataRow)
        Dim name As String = drow.Value("f_s編みかた名")
        Dim idx As Integer = drow.Value("f_iひも番号")
        For pos As Integer = 0 To _table.Rows.Count - 1
            Dim c As clsDataRow = New clsDataRow(CType(BindingSource編みかた.Item(pos).Row, tbl編みかたRow))
            If c.Value("f_s編みかた名") = name AndAlso c.Value("f_iひも番号") = idx Then
                BindingSource編みかた.Position = pos
                Exit For
            End If
        Next
    End Sub

    Private Sub dgvData_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        If {"f_b縁専用区分", "f_b底使用区分", "f_bCraftBandMesh", "f_bCraftBandSquare45", "f_bCraftBandKnot", "f_bCraftBandSquare", "f_bCraftBandHexagon"}.Contains(dgv.Columns(e.ColumnIndex).DataPropertyName) Then
            '名前・Idx順にソートされており、Idx=1が変更された前提!
            Dim setval As Boolean = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            Dim name As String = dgv.Rows(e.RowIndex).Cells(_NameColumnIndex).Value
            For ridx As Integer = e.RowIndex + 1 To dgv.Rows.Count - 1
                If dgv.Rows(ridx).Cells(_NameColumnIndex).Value <> name Then
                    Exit For
                End If
                dgv.Rows(ridx).Cells(e.ColumnIndex).Value = setval
            Next

        End If

    End Sub

    Private Sub btn削除_Click(sender As Object, e As EventArgs) Handles btn削除.Click
        Dim current As System.Data.DataRowView = BindingSource編みかた.Current
        If current Is Nothing OrElse current.Row Is Nothing Then
            Exit Sub
        End If

        Dim row As tbl編みかたRow = CType(current.Row, tbl編みかたRow)
        Dim idx As Integer = row.f_iひも番号
        Dim name As String = row.f_s編みかた名
        Dim dels() As DataRow
        Dim msg As String
        If idx = 1 Then
            dels = _table.Select(String.Format("f_s編みかた名='{0}'", name))
            If dels Is Nothing OrElse dels.Count = 0 Then
                Exit Sub
            End If
            ''{0}'をすべて削除します。よろしいですか？
            msg = String.Format(My.Resources.AskConfirmDeleteGroup, name)
        ElseIf 1 < idx Then
            dels = _table.Select(String.Format("f_s編みかた名='{0}' AND f_iひも番号={1}", name, idx))
            If dels Is Nothing OrElse dels.Count = 0 Then
                Exit Sub
            End If
            ''{0}'(ひも番号{1})を削除します。よろしいですか？
            msg = String.Format(My.Resources.AskConfirmDeleteSub, name, idx)
        Else
            Exit Sub
        End If
        Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        For Each del In dels
            _table.Rows.Remove(del)
        Next
    End Sub


End Class