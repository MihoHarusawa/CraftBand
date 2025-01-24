Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables


Public Class frmSaveTemporarily


    Dim _DataTables As clsDataTables = Nothing
    Dim _Table As dstWork.tblSaveTemporarilyDataTable
    Dim _EditChanged As Boolean = False

    Sub New(ByRef data As clsDataTables)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        _DataTables = data
    End Sub

#Region "Shared関数"

    Shared _TempSaveFolder As String
    Shared _TempSaveInfoPath As String

    '%tmp%<EXE名>
    Private Shared Sub setPath()
        If String.IsNullOrWhiteSpace(_TempSaveFolder) OrElse String.IsNullOrWhiteSpace(_TempSaveInfoPath) Then
            _TempSaveFolder = IO.Path.Combine(IO.Path.GetTempPath, g_enumExeName.ToString)
            _TempSaveInfoPath = IO.Path.Combine(_TempSaveFolder, "SaveInfo.txt")
        End If
        If Not IO.Directory.Exists(_TempSaveFolder) Then
            Try
                IO.Directory.CreateDirectory(_TempSaveFolder)
            Catch ex As Exception
                g_clsLog.LogException(ex, "frmSaveTemporarily.setPath")
            End Try
        End If
    End Sub

    'クリアする
    Public Shared Function ClearSaved() As Boolean
        setPath()

        If Not IO.Directory.Exists(_TempSaveFolder) OrElse
          IO.Directory.GetFiles(_TempSaveFolder).Count = 0 Then
            Return True
        End If

        Try
            For Each file As String In IO.Directory.GetFiles(_TempSaveFolder)
                IO.File.Delete(file)
            Next
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmSaveTemporarily.ClearSaved")
            Return False
        End Try
    End Function

    Private Shared Function getFilePath(ByVal timestr As String) As String
        setPath()
        Return IO.Path.ChangeExtension(IO.Path.Combine(_TempSaveFolder, timestr.Trim), clsDataTables.DataExtention)
    End Function

    Private Shared Function getNextIndex(ByVal table As dstWork.tblSaveTemporarilyDataTable) As Integer
        If table Is Nothing Then
            Return 0
        End If

        Dim maxindex As Object = table.Compute("MAX(index)", Nothing)
        If IsDBNull(maxindex) Then
            Return 1 '開始値
        End If
        Return Convert.ToInt32(maxindex) + 1
    End Function
#End Region


    Dim _MyProfile As New CDataGridViewProfile(
            (New dstWork.tblSaveTemporarilyDataTable),
            Nothing,
            enumAction._BackColorReadOnlyYellow
            )

    Private Sub frmSaveTemporarily_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)
        setPath()

        txt時刻.Text = DateTime.Now.ToString("HH-mm-ss-fff")
        _Table = New dstWork.tblSaveTemporarilyDataTable

        'ファイルを読み込み、データをテーブルに追加
        If IO.File.Exists(_TempSaveInfoPath) Then
            Using reader As New StreamReader(_TempSaveInfoPath)
                While Not reader.EndOfStream
                    Dim line As String = reader.ReadLine()
                    Dim fields As String() = line.Split(vbTab)

                    If 2 <= fields.Length AndAlso
                        Not String.IsNullOrWhiteSpace(fields(0)) AndAlso
                        Not String.IsNullOrWhiteSpace(fields(1)) Then

                        Dim index As Integer
                        If Integer.TryParse(fields(0), index) Then

                            Dim row As dstWork.tblSaveTemporarilyRow = _Table.NewtblSaveTemporarilyRow
                            row.Index = index
                            row.TimeString = fields(1).Trim
                            Dim i As Integer = 2
                            While i < fields.Length
                                If Not String.IsNullOrEmpty(fields(i)) Then
                                    If String.IsNullOrEmpty(row.Description) Then
                                        row.Description = fields(i)
                                    Else
                                        row.Description = row.Description & " / " & fields(i)
                                    End If
                                End If
                                i += 1
                            End While
                            row.IsFileExist = IO.File.Exists(getFilePath(row.TimeString))

                            _Table.Rows.Add(row)
                        End If
                    End If
                End While
            End Using

            ' 読み込んだデータを表示
            BindingSourceSaveTemporarily.DataSource = _Table
            BindingSourceSaveTemporarily.Sort = "Index Desc"
            dgvData.Refresh()
        End If

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmSaveTemporarilySize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmSaveTemporarilyGrid", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If

    End Sub

    Private Sub frmSaveTemporarily_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmSaveTemporarilyGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmSaveTemporarilySize", Me.Size)

        '情報ファイルの書き込み
        If _EditChanged Then
            Dim rows As dstWork.tblSaveTemporarilyRow() = _Table.Select("IsFileExist = True", "Index ASC")
            Dim sb As New StringBuilder()
            For Each r As dstWork.tblSaveTemporarilyRow In rows
                sb.Append(r.Index).Append(vbTab)
                sb.Append(r.TimeString).Append(vbTab)
                sb.AppendLine(r.Description.Replace(vbCrLf, vbTab))
            Next
            File.WriteAllText(_TempSaveInfoPath, sb.ToString())
        End If
    End Sub


    Private Sub btn一時保存_Click(sender As Object, e As EventArgs) Handles btn一時保存.Click
        setPath()

        If _Table Is Nothing Then
            Return
        End If

        If Not _DataTables.Save(getFilePath(txt時刻.Text)) Then
            MessageBox.Show(_DataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        'レコード追加
        Dim row As dstWork.tblSaveTemporarilyRow = _Table.NewtblSaveTemporarilyRow
        row.Index = getNextIndex(_Table)
        row.TimeString = txt時刻.Text
        row.Description = txtメモ.Text
        row.IsFileExist = True
        _Table.Rows.Add(row)
        _Table.AcceptChanges()

        _EditChanged = True

        '閉じて終了
        Me.Close()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


    Private Sub dgvData_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 OrElse
            BindingSourceSaveTemporarily.Current Is Nothing Then
            Exit Sub
        End If

        Dim timestring As String = CType(BindingSourceSaveTemporarily.Current, DataRowView)("TimeString")
        Dim fpath As String = getFilePath(timestring)
        If e.ColumnIndex = dgvData.Columns("col復元").Index Then
            '復元ボタンクリック
            If _DataTables IsNot Nothing AndAlso IO.File.Exists(fpath) Then
                _DataTables.Load(fpath)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If

        ElseIf e.ColumnIndex = dgvData.Columns("col削除").Index Then
            '削除ボタンクリック
            If IO.File.Exists(fpath) Then
                IO.File.Delete(fpath)
            End If
            BindingSourceSaveTemporarily.RemoveCurrent()
            _EditChanged = True
        End If
    End Sub

    Private Sub dgvData_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellEndEdit
        _EditChanged = True
    End Sub


End Class