Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstMasterTables

''' <summary>
'''tbl付属品 
'''ひも番号が2以上の時に使う項目: f_d長さ比率対ひも1,f_d長さ加減対ひも1
'''巻きひも区分がTrueの時に使う項目:f_d巻きの厚み,f_d巻き回数比率
'''Null値をセットする項目:f_d長さ比率対ひも1,f_d長さ加減対ひも1,f_d巻きの厚み,f_d巻き回数比率
''' </summary>
Public Class frmOptions

    Dim _table As tbl付属品DataTable
    Dim _NameColumnIndex As Integer = -1
    Dim _SubNameHeaderText As String = Nothing

    Dim _MyProfile As New CDataGridViewProfile(
            (New tbl付属品DataTable),
            Nothing,
            enumAction._RowHeight_iひも番号_BackColor Or enumAction._CheckBoxGray_iひも番号
            )


    Private Sub frmOptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        '選択肢
        f_i描画位置.DataSource = clsMasterTables.get描画位置table
        f_i描画位置.DisplayMember = "Display"
        f_i描画位置.ValueMember = "Value"

        f_i描画形状.DataSource = clsMasterTables.get描画形状table
        f_i描画形状.DisplayMember = "Display"
        f_i描画形状.ValueMember = "Value"

        For Each col As DataGridViewColumn In dgvData.Columns
            If col.DataPropertyName = "f_s付属品名" Then
                _NameColumnIndex = col.Index
            ElseIf col.DataPropertyName = "f_s付属品ひも名" Then
                _SubNameHeaderText = col.HeaderText
            End If
            If 0 <= _NameColumnIndex AndAlso Not String.IsNullOrEmpty(_SubNameHeaderText) Then
                Exit For
            End If
        Next
        lbl設定時の寸法単位.Text = g_clsSelectBasics.p_unit設定時の寸法単位.ToString

        BindingSource付属品.Sort = Nothing
        BindingSource付属品.DataSource = Nothing

        _table = g_clsMasterTables.GetOptionsTableCopy()
        BindingSource付属品.DataSource = _table
        BindingSource付属品.Sort = "f_s付属品名 , f_iひも番号"

        dgvData.Refresh()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmOptionsSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmOptionsGrid", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If g_clsMasterTables.UpdateOptionsTable(_table) Then
            g_clsSelectBasics.UpdateTargetBandType()
            Me.DialogResult = DialogResult.OK 'Changed, Need Update
        Else
            Me.DialogResult = DialogResult.Cancel 'No Change
        End If
        Me.Close()
    End Sub

    Private Sub frmOptions_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmOptionsGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmOptionsSize", Me.Size)
    End Sub

    Private Sub dgvData_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvData.CurrentCellChanged
        If dgvData.CurrentRow IsNot Nothing AndAlso 0 <= dgvData.CurrentRow.Index Then
            txt付属品名.Text = dgvData.Rows(dgvData.CurrentRow.Index).Cells(_NameColumnIndex).Value
        End If
    End Sub

    Private Sub btnひも追加_Click(sender As Object, e As EventArgs) Handles btnひも追加.Click
        If String.IsNullOrWhiteSpace(txt付属品名.Text) Then
            '新しい/既存の{0}を指定してください。
            Dim msg As String = String.Format(My.Resources.ErrNoName, lbl付属品名.Text)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim add As clsDataRow = Nothing
        Dim exist As clsDataRow = findRecord(txt付属品名.Text)
        If exist Is Nothing Then
            '新たに'{0}'を追加します。よろしいですか？
            Dim msg As String = String.Format(My.Resources.AskConfirmAdd, txt付属品名.Text)
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
            add = setNewRecord(txt付属品名.Text)
        Else
            '既存の'{0}'に{1}を追加します。よろしいですか？
            Dim msg As String = String.Format(My.Resources.AskConfirmAppend, txt付属品名.Text, _SubNameHeaderText)
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

    Private Function findRecord(ByVal s付属品名 As String) As clsDataRow
        Dim cond As String = String.Format("f_s付属品名='{0}'", s付属品名)
        Dim rows() As tbl付属品Row = _table.Select(cond, "f_iひも番号 DESC")
        If rows Is Nothing OrElse rows.Count = 0 Then
            Return Nothing
        End If
        Return New clsDataRow(rows(0)) '最大ひも番号
    End Function


    Private Function setNewRecord(ByVal s付属品 As String) As clsDataRow
        Dim add As New clsDataRow(_table.Newtbl付属品Row)
        add.Value("f_s付属品名") = s付属品
        add.Value("f_s付属品ひも名") = _SubNameHeaderText
        add.Value("f_iひも番号") = 1
        add.Value("f_d長さ比率対ひも1") = DBNull.Value
        add.Value("f_d長さ加減対ひも1") = DBNull.Value
        add.Value("f_b巻きひも区分") = False
        add.Value("f_d巻きの厚み") = DBNull.Value
        add.Value("f_d巻き回数比率") = DBNull.Value

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "setNewRecord {0}", add.dump())
        Return add
    End Function

    Private Function setNextRecord(ByVal exist As clsDataRow) As clsDataRow
        If Not exist.IsValid OrElse Not exist.HasValue("f_s付属品名") Then
            Return Nothing
        End If
        Dim cond As String = String.Format("f_s付属品名='{0}'", exist.Value("f_s付属品名"))
        Dim nextidx As Integer = CType(_table.Compute("Max(f_iひも番号)", cond), Integer) + 1

        Dim add As New clsDataRow(exist)
        add.Value("f_s付属品ひも名") = exist.Value("f_s付属品ひも名") & nextidx.ToString
        add.Value("f_iひも番号") = nextidx
        add.Value("f_d長さ比率対ひも1") = 1
        add.Value("f_d長さ加減対ひも1") = 0

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "setNextRecord {0}", add.dump())
        Return add
    End Function

    Private Sub setBindingSourcePosition(ByVal drow As clsDataRow)
        Dim name As String = drow.Value("f_s付属品名")
        Dim idx As Integer = drow.Value("f_iひも番号")
        For pos As Integer = 0 To _table.Rows.Count - 1
            Dim c As clsDataRow = New clsDataRow(CType(BindingSource付属品.Item(pos).row, tbl付属品Row))
            If c.Value("f_s付属品名") = name AndAlso c.Value("f_iひも番号") = idx Then
                BindingSource付属品.Position = pos
                Exit For
            End If
        Next
    End Sub

    Private Sub dgvData_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        If dgv.Columns(e.ColumnIndex).DataPropertyName = "f_b巻きひも区分" Then
            Dim isLoop As Boolean = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            For Each col As DataGridViewColumn In dgv.Columns
                If col.DataPropertyName = "f_d巻きの厚み" Then
                    If isLoop Then
                        dgv.Rows(e.RowIndex).Cells(col.Index).Value = New Length(0.55, "cm").Value '規定値
                    Else
                        dgv.Rows(e.RowIndex).Cells(col.Index).Value = DBNull.Value
                    End If
                ElseIf col.DataPropertyName = "f_d巻き回数比率" Then
                    If isLoop Then
                        dgv.Rows(e.RowIndex).Cells(col.Index).Value = 1
                    Else
                        dgv.Rows(e.RowIndex).Cells(col.Index).Value = DBNull.Value
                    End If
                End If
            Next

        ElseIf {"f_bCraftBandMesh", "f_bCraftBandSquare45", "f_bCraftBandKnot", "f_bCraftBandSquare", "f_bCraftBandHexagon"}.Contains(dgv.Columns(e.ColumnIndex).DataPropertyName) Then
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
        Dim current As System.Data.DataRowView = BindingSource付属品.Current
        If current Is Nothing OrElse current.Row Is Nothing Then
            Exit Sub
        End If

        Dim row As tbl付属品Row = CType(current.Row, tbl付属品Row)
        Dim idx As Integer = row.f_iひも番号
        Dim name As String = row.f_s付属品名
        Dim dels() As DataRow
        Dim msg As String
        If idx = 1 Then
            dels = _table.Select(String.Format("f_s付属品名='{0}'", name))
            If dels Is Nothing OrElse dels.Count = 0 Then
                Exit Sub
            End If
            ''{0}'をすべて削除します。よろしいですか？
            msg = String.Format(My.Resources.AskConfirmDeleteGroup, name)
        ElseIf 1 < idx Then
            dels = _table.Select(String.Format("f_s付属品名='{0}' AND f_iひも番号={1}", name, idx))
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