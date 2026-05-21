Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView

Public Class frmOutput

    Dim _Output As clsOutput
    Dim _BlankColumnIndex As Integer = -1

    Dim _MyProfile As New CDataGridViewProfile(
            (New Tables.dstOutput.tblOutputDataTable),
            Nothing,
            enumAction._None
            )

    Sub New(ByVal output As clsOutput)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        _Output = output

    End Sub

    Private Sub frmOutput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvOutput.SetProfile(_MyProfile)

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
            Me.dgvOutput.SetColumnWidthFromString(colwid)
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
        __paras.SetLastData("frmOutputGrid", Me.dgvOutput.GetColumnWidthString())
        __paras.SetLastData("frmOutputSize", Me.Size)
    End Sub

    Private Sub btnCSV出力_Click(sender As Object, e As EventArgs) Handles btnCSV出力.Click
        Dim errmsg As String = Nothing
        If Not dgvOutput.GridCsvOut(_Output.FilePath, errmsg) Then
            MessageBox.Show(errmsg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btnTXT出力_Click(sender As Object, e As EventArgs) Handles btnTXT出力.Click
        Dim errmsg As String = Nothing
        If Not dgvOutput.GridTxtOut(_Output.FilePath, errmsg) Then
            MessageBox.Show(errmsg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Public Function GetTableOutput(ByVal fpath As String, ByRef errmsg As String) As Boolean

        ' データソースはあるはずだが念のため
        If _Output Is Nothing OrElse _Output.Table Is Nothing Then
            errmsg = "No output table data."
            Return False
        End If

        Dim sb As New System.Text.StringBuilder

        ' ヘッダー：表示中の列ヘッダーを使う
        Dim visibleCols As New List(Of DataGridViewColumn)
        For Each col As DataGridViewColumn In dgvOutput.Columns
            If col.Visible Then
                visibleCols.Add(col)
                sb.Append(""""c).Append(col.HeaderText).Append(""",")
            End If
        Next
        sb.AppendLine()

        ' レコード部分は _Output.Table から取得（DataPropertyName をキーにする）
        For Each dr As System.Data.DataRow In _Output.Table.Rows
            For Each col As DataGridViewColumn In visibleCols
                Dim cellValue As String = String.Empty
                Dim propName As String = col.DataPropertyName
                If Not String.IsNullOrEmpty(propName) AndAlso _Output.Table.Columns.Contains(propName) Then
                    Dim o As Object = dr(propName)
                    If o IsNot Nothing AndAlso Not Convert.IsDBNull(o) Then
                        cellValue = o.ToString()
                    End If
                Else
                    ' DataPropertyName が未設定かテーブルに列が無い場合は空文字を入れる
                    cellValue = String.Empty
                End If
                ' 値が改行やダブルクォートを含む可能性があるため簡易エスケープ（ダブルクォートを2つに）
                cellValue = cellValue.Replace("""", """""")
                sb.Append(""""c).Append(cellValue).Append(""",")
            Next
            sb.AppendLine()
        Next

        Try
            'UTF8
            Using sw As New System.IO.StreamWriter(fpath, False, System.Text.Encoding.UTF8) '上書き
                sw.Write(sb.ToString)
                sw.Close()
            End Using
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmOutput.GetGridOutput (FileWrite)", errmsg)
            '指定されたファイル'{0}'への保存ができませんでした。
            errmsg = String.Format(My.Resources.WarningFileSaveError, fpath)
            Return False
        End Try

    End Function
End Class