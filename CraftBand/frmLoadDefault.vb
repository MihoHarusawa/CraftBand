Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables

Public Class frmLoadDefault

    Friend FolderPath As String
    Friend FilePath As String

#Region "Shared関数"

    Public Shared Function DefaultFolder(ByRef strPath As String) As Boolean
        Dim folderBrowserDialog As New FolderBrowserDialog()

        '規定値を保存するフォルダを指定してください。
        folderBrowserDialog.Description = My.Resources.MsgLoadDefaultFolder

        '初期表示フォルダ
        If IO.Directory.Exists(strPath) Then
            folderBrowserDialog.SelectedPath = strPath
        ElseIf IO.File.Exists(strPath) Then
            'ファイルを指定していた時の互換
            folderBrowserDialog.SelectedPath = IO.Path.GetDirectoryName(strPath)
        End If

        'ダイアログを表示し、ユーザーがOKを押した場合
        If folderBrowserDialog.ShowDialog() = DialogResult.OK Then
            strPath = folderBrowserDialog.SelectedPath
            Return True
        End If

        Return False
    End Function

    Public Shared Function LoadDefault(ByRef strPath As String, ByVal clsDataTables As clsDataTables) As Boolean
        If clsDataTables Is Nothing Then
            Return False
        End If
        Dim dlg = New frmLoadDefault

        If String.IsNullOrWhiteSpace(strPath) OrElse Not IO.Directory.Exists(strPath) Then
            '規定値を保存するフォルダを指定してください。
            MessageBox.Show(My.Resources.MsgLoadDefaultFolder, dlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        dlg.FolderPath = strPath
        If dlg.ShowDialog() = DialogResult.OK Then
            If clsDataTables.Load(dlg.FilePath) Then
                Return True
            Else
                MessageBox.Show(clsDataTables.LastError, dlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If

        Return False
    End Function

#End Region

    Dim _DataTables As clsDataTables = Nothing
    Dim _Table As dstWork.tblDefaultDataDataTable

    Dim _MyProfile As New CDataGridViewProfile(
            (New dstWork.tblDefaultDataDataTable),
            Nothing,
            enumAction._BackColorReadOnlyYellow
            )

    Private Sub frmLoadDefault_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        _Table = New dstWork.tblDefaultDataDataTable

        Dim directoryInfo As New DirectoryInfo(folderPath)

        '指定されたフォルダ内のすべてのファイルを取得
        For Each fileInfo As FileInfo In directoryInfo.GetFiles()
            '対象拡張子のファイルのみ(#102)
            If Not clsDataTables.IsDataExtention(fileInfo.Extension) Then
                Continue For
            End If
            '現EXEで読めるデータ
            Dim data As New clsDataTables
            If Not data.Load(fileInfo.FullName) Then
                Continue For
            End If

            Dim row As dstWork.tblDefaultDataRow = _Table.NewtblDefaultDataRow
            row.FilePath = fileInfo.FullName
            row.FileName = IO.Path.GetFileName(fileInfo.FullName)
            With data.p_row目標寸法
                row.BandTypeName = .Value("f_sバンドの種類名")
                row.TargetWidth = .Value("f_d横寸法")
                row.TargetDepth = .Value("f_d縦寸法")
                row.TargetHeight = .Value("f_d高さ寸法")
                row.BasicBandWidth = .Value("f_i基本のひも幅")
                row.Title = .Value("f_sタイトル")
                row.Creator = .Value("f_s作成者")
                row.Memo = .Value("f_sメモ")
            End With

            _Table.Rows.Add(row)
        Next

        ' 読み込んだデータを表示
        BindingSourceDefaultData.DataSource = _Table
        BindingSourceDefaultData.Sort = "FileName Asc"
        dgvData.Refresh()
        dgvData.ClearSelection()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmLoadDefaultSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmLoadDefaultGrid", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If

        'ない場合
        If _Table.Rows.Count = 0 Then
            '現在の規定値保存フォルダ'{0}'には {1} のファイルはありません。
            Dim msg As String = String.Format(My.Resources.MsgLoadDefaultNoFile, FolderPath, GetShortExeName(g_enumExeName))
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.Close()
        End If

    End Sub

    Private Sub frmLoadDefault_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmLoadDefaultGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmLoadDefaultSize", Me.Size)
    End Sub

    Private Sub btnロードする_Click(sender As Object, e As EventArgs) Handles btnロードする.Click
        FilePath = txt選択ファイル.Text
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub txt選択ファイル_TextChanged(sender As Object, e As EventArgs) Handles txt選択ファイル.TextChanged
        btnロードする.Enabled = IO.File.Exists(txt選択ファイル.Text)
    End Sub

    Private Sub dgvData_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 OrElse
            BindingSourceDefaultData.Current Is Nothing OrElse
            BindingSourceDefaultData.Current.row Is Nothing Then
            Exit Sub
        End If

        If e.ColumnIndex = dgvData.Columns("col選択").Index Then
            txt選択ファイル.Text = CType(BindingSourceDefaultData.Current.row, dstWork.tblDefaultDataRow).FilePath
        Else
            txt選択ファイル.Text = String.Empty
        End If
    End Sub
End Class