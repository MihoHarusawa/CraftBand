Imports System.Drawing
Imports System.Windows.Forms

Public Class frmOutput

    Dim _Output As clsOutput
    Dim _BlankColumnIndex As Integer = -1

    Sub New(ByVal output As clsOutput)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        _Output = output

    End Sub

    Private Sub frmOutput_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        For Each col As DataGridViewColumn In dgvOutput.Columns
            If col.DataPropertyName = "f_b空行区分" Then
                _BlankColumnIndex = col.Index
                Exit For
            End If
        Next

        BindingSourceOutput.DataSource = _Output.Table
        dgvOutput.Refresh()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmOutputSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmOutputGrid", colwid) Then
            SetColumnWidthFromString(Me.dgvOutput, colwid)
        End If
    End Sub

    Private Sub dgvOutput_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvOutput.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If _BlankColumnIndex < 0 OrElse dgv Is Nothing OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        If dgv.Rows(e.RowIndex).Cells(_BlankColumnIndex).Value Then
            e.CellStyle.BackColor = Color.DarkGray
        End If
    End Sub

    Private Sub frmOutput_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmOutputGrid", GetColumnWidthString(Me.dgvOutput))
        __paras.SetLastData("frmOutputSize", Me.Size)
    End Sub

    Private Sub btnCSV出力_Click(sender As Object, e As EventArgs) Handles btnCSV出力.Click
        Dim errmsg As String = Nothing
        If Not mdlGrid.GridCsvOut(dgvOutput, _Output.FilePath, errmsg) Then
            MessageBox.Show(errmsg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btnTXT出力_Click(sender As Object, e As EventArgs) Handles btnTXT出力.Click
        Dim errmsg As String = Nothing
        If Not mdlGrid.GridTxtOut(dgvOutput, _Output.FilePath, errmsg) Then
            MessageBox.Show(errmsg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
End Class