Imports System.ComponentModel.Design.ObjectSelectorEditor
Imports System.Drawing
Imports System.Windows.Forms

Public Module mdlGrid
    Public Const cRowHeightIdxOne As Integer = 34
    Public Const cRowHeightIdxSub As Integer = 20


    'ワンクリック編集
    Friend Sub dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        If dgv Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        If dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly OrElse
            dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Visible = False Then
            Exit Sub
        End If
        SendKeys.Send("{F2}")
    End Sub

    'データエラーは受け付けない
    Public Sub dgv_DataErrorCancel(sender As Object, e As DataGridViewDataErrorEventArgs, ByVal title As String)
        If e.Exception Is Nothing Then
            Exit Sub
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)

        '{0}行目{1}値のデータエラー{2}{3}
        Dim msg As String = String.Format(My.Resources.ErrGridData, e.RowIndex + 1, dgv.Columns(e.ColumnIndex).HeaderText,
                                 vbCrLf, e.Exception.Message)
        MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        e.Cancel = True
    End Sub

    'ひも番号は１以上
    Public Sub dgv_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs, ByVal title As String)
        Dim dgv As DataGridView = CType(sender, DataGridView)

        '新しい行のセルは除外
        If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        If dgv.Columns(e.ColumnIndex).DataPropertyName = "f_iひも番号" Then
            Dim v As Integer
            If Integer.TryParse(e.FormattedValue.ToString(), v) AndAlso 1 <= v Then
                Exit Sub 'OK
            End If
            '{0}行目{1}値には{2}以上の数値を設定してください。
            Dim msg As String = String.Format(My.Resources.ErrGridMinimumValue, e.RowIndex + 1, dgv.Columns(e.ColumnIndex).HeaderText, 1)
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            dgv.CancelEdit()
            e.Cancel = True
        End If
    End Sub

    'ひも番号１以外の行の背景色をグレーにする
    Friend Sub dgv_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        'その行のひも番号
        Dim idx As Integer = -1
        For Each col As DataGridViewColumn In dgv.Columns
            If col.DataPropertyName = "f_iひも番号" Then
                idx = dgv.Rows(e.RowIndex).Cells(col.Index).Value
                Exit For
            End If
        Next
        If idx = 1 Then
            If e.ColumnIndex = 1 Then '1回だけ
                dgv.Rows(e.RowIndex).HeaderCell.Value = "*"
            End If
        ElseIf 1 < idx Then
            e.CellStyle.BackColor = Color.Gainsboro '全col
            dgv.Rows(e.RowIndex).Height = cRowHeightIdxSub '1回でよいが全部やる
        End If

    End Sub

    Friend Sub dgvData_CellPaintingCheckBox(sender As Object, e As DataGridViewCellPaintingEventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        If dgv Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        '処理対象チェックボックス列
        If (TypeOf dgv.Columns(e.ColumnIndex) IsNot DataGridViewCheckBoxColumn) OrElse
              Not {"f_b非表示", "f_b縁専用区分", "f_b底使用区分"}.Contains(dgv.Columns(e.ColumnIndex).DataPropertyName) Then
            Exit Sub
        End If

        'その行のひも番号
        Dim idx As Integer = -1
        For Each col As DataGridViewColumn In dgv.Columns
            If col.DataPropertyName = "f_iひも番号" Then
                idx = dgv.Rows(e.RowIndex).Cells(col.Index).Value
                Exit For
            End If
        Next
        If 1 < idx Then
            e.PaintBackground(e.CellBounds, False)
            Dim rect As Rectangle = e.CellBounds
            rect.Width = 15
            rect.Height = 15
            rect.Offset((e.CellBounds.Width - 16) \ 2, rect.Height \ 2 - 5)
            ControlPaint.DrawFocusRectangle(e.Graphics, rect)

            e.Handled = True
        End If
    End Sub


    'カラム幅を文字列に保存
    Public Function GetColumnWidthString(ByVal dgv As System.Windows.Forms.DataGridView) As String
        Dim sb As New System.Text.StringBuilder
        For Each d1 As DataGridViewColumn In dgv.Columns
            If d1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill OrElse Not d1.Visible Then
                sb.Append("-1,")
            Else
                sb.AppendFormat("{0},", d1.Width)
            End If
        Next
        Return sb.ToString
    End Function

    '文字列からカラム幅を復元
    Public Function SetColumnWidthFromString(ByVal dgv As System.Windows.Forms.DataGridView, ByVal csvStr As String) As Integer
        If [String].IsNullOrEmpty(csvStr) Then
            Return -1
        End If
        Dim widthStr As String() = csvStr.Split(",")
        For i As Integer = 0 To widthStr.Length - 1
            If i >= dgv.Columns.Count Then
                Return i
            End If
            Dim d1 = dgv.Columns(i)
            If d1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill OrElse
                Not d1.Visible OrElse
                d1.Resizable = DataGridViewTriState.False Then
                Continue For
            End If

            Dim width As Integer = -1
            If Not Integer.TryParse(widthStr(i), width) OrElse width <= 0 Then
                Return i
            End If
            dgv.Columns(i).Width = width
        Next
        Return widthStr.Length
    End Function



End Module
