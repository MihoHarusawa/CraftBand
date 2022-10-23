Imports System.Text
Imports System.Text.RegularExpressions
Imports CraftBand

Public Class frmOutput

    Dim _EncShiftJis As System.Text.Encoding
    Dim _Output As clsOutput
    Dim _BlankColumnIndex As Integer = -1

    Sub New(ByVal output As clsOutput)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        _Output = output

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        _EncShiftJis = System.Text.Encoding.GetEncoding("shift_jis")

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
        Dim siz As Size = My.Settings.frmOutputSize
        If 0 < siz.Height AndAlso 0 < siz.Width Then
            Me.Size = siz
        End If
        Dim colwid As String = My.Settings.frmOutputGrid
        If Not String.IsNullOrWhiteSpace(colwid) Then
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
        My.Settings.frmOutputGrid = GetColumnWidthString(Me.dgvOutput)
        My.Settings.frmOutputSize = Me.Size
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "frmOutput Grid={0}", My.Settings.frmOutputGrid)
    End Sub


    Private Function PaddingSpace(ByVal str As String, ByVal keta As Integer, ByVal isRight As Boolean) As String
        If str Is Nothing Then
            str = ""
        End If
        Const space As String = " "

        '（文字数　=　桁　-　(対象文字列のバイト数 - 対象文字列の文字列数)）
        Dim padLength As Integer = keta - (_EncShiftJis.GetByteCount(str) - str.Length)
        If padLength <= 0 Then
            Return str
        End If

        If isRight Then
            Return str.PadLeft(padLength, space.ToCharArray()(0)) '右寄せ
        Else
            Return str.PadRight(padLength, space.ToCharArray()(0)) '左寄せ
        End If
    End Function

    Private Sub btnCSV出力_Click(sender As Object, e As EventArgs) Handles btnCSV出力.Click
        Dim fpath As String = IO.Path.GetFileNameWithoutExtension(_Output.FilePath) & "_"
        fpath = IO.Path.ChangeExtension(fpath, ".csv")
        fpath = IO.Path.Combine(IO.Path.GetTempPath, fpath)

        Dim sb As New System.Text.StringBuilder

        For Each col As DataGridViewColumn In dgvOutput.Columns
            If col.Visible Then
                sb.Append("""").Append(col.HeaderText).Append(""",")
            End If
        Next
        sb.AppendLine()

        For Each row As DataGridViewRow In dgvOutput.Rows

            For Each col As DataGridViewColumn In dgvOutput.Columns
                If col.Visible Then
                    sb.Append("""").Append(row.Cells(col.Index).Value).Append(""",")
                End If
            Next
            sb.AppendLine()
        Next

        Try
            My.Computer.FileSystem.WriteAllText(fpath, sb.ToString, False)

            Dim p As New Process
            p.StartInfo.FileName = fpath
            p.StartInfo.UseShellExecute = True
            p.Start()

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmOutput.btnCSV出力_Click")
            '指定されたファイル'{0}'への保存ができませんでした。
            Dim msg As String = String.Format(My.Resources.WarningFileSaveError, fpath)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub btnTXT出力_Click(sender As Object, e As EventArgs) Handles btnTXT出力.Click
        Dim fpath As String = IO.Path.GetFileNameWithoutExtension(_Output.FilePath) & "_"
        fpath = IO.Path.ChangeExtension(fpath, ".txt")
        fpath = IO.Path.Combine(IO.Path.GetTempPath, fpath)

        Dim sb As New System.Text.StringBuilder

        For Each col As DataGridViewColumn In dgvOutput.Columns
            If col.Visible Then
                Dim str As String = col.HeaderText
                Dim len As Integer = col.Width / 5
                Dim isRight As Boolean = col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                sb.Append(PaddingSpace(str, len, isRight)).Append(" ")
            End If
        Next
        sb.AppendLine()

        For Each row As DataGridViewRow In dgvOutput.Rows
            For Each col As DataGridViewColumn In dgvOutput.Columns
                If col.Visible Then
                    Dim str As String = row.Cells(col.Index).Value.ToString
                    Dim len As Integer = col.Width / 5
                    If len < 2 Then
                        len = 2
                    End If
                    Dim isRight As Boolean = col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    sb.Append(PaddingSpace(str, len, isRight)).Append(" ")
                End If
            Next
            sb.AppendLine()
        Next

        Try
            My.Computer.FileSystem.WriteAllText(fpath, sb.ToString, False)

            Dim p As New Process
            p.StartInfo.FileName = fpath
            p.StartInfo.UseShellExecute = True
            p.Start()

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmOutput.btnTXT出力_Click")
            '指定されたファイル'{0}'への保存ができませんでした。
            Dim msg As String = String.Format(My.Resources.WarningFileSaveError, fpath)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub
End Class