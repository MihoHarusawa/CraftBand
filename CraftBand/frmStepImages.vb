Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports CraftBand.Tables

Public Class frmStepImages

    Dim _frmMain As Windows.Forms.Form
    Dim _commonActions As ICommonActions = Nothing
    Dim _dataCurrent As clsDataTables
    Dim _dataFilePath As String

    'クラス内保持
    Shared _ImgSel As Integer = 1 '1=プレビュー、2=プレビュー2
    Shared _FolderSel As String
    Shared _filename As String

    Const TARGET_EXT As String = ".gif"



    Public Function SetMainForm(ByVal frm As Windows.Forms.Form, ByVal data As clsDataTables, ByVal fpath As String)
        _frmMain = frm
        _dataCurrent = data
        _dataFilePath = fpath
        _commonActions = TryCast(frm, ICommonActions)
        Return _commonActions IsNot Nothing
    End Function

    Private Sub frmStepImages_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btn画像生成.Enabled = False
        btn一括生成.Enabled = False
        If _commonActions Is Nothing Then
            Exit Sub
        End If

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmStepImages", siz) Then
            Me.Size = siz
        End If

        '色リスト
        cmb非表示色.Items.Clear()
        For Each r As dstWork.tblColorRow In g_clsSelectBasics.p_tblColor
            cmb非表示色.Items.Add(r.Display)
        Next
        Dim text As String = Nothing
        If __paras.GetLastData("frmStepImagesNoDispColor", text) Then
            cmb非表示色.Text = text
        End If

        If _ImgSel = 2 Then
            radプレビュー2.Checked = True
        End If

        'データが展開されていること
        If Not _dataCurrent.p_row底_縦横.Value("f_b展開区分") Then
            addMessage("縦横が展開されていません。列を表示し、展開して表示順をセットしてください。")
            Exit Sub
        End If

        '表示順最大値
        Dim maxStep As Integer = GetMaxStepDisp(_dataCurrent)
        txt表示順最大値.Text = maxStep.ToString
        nud表示番号.Maximum = maxStep + 1
        nudFrom.Maximum = maxStep
        nudTo.Maximum = maxStep + 1
        nudTo.Value = maxStep

        'ファイル名
        txtファイル名.Text = _filename

        'フォルダ
        If Not String.IsNullOrWhiteSpace(_FolderSel) Then
            txt生成先フォルダ.Text = _FolderSel
        ElseIf String.IsNullOrWhiteSpace(_dataFilePath) Then
            txt生成先フォルダ.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Else
            txt生成先フォルダ.Text = IO.Path.Combine(IO.Path.GetDirectoryName(_dataFilePath), IO.Path.GetFileNameWithoutExtension(_dataFilePath))
        End If

        btn画像生成.Enabled = True
        btn一括生成.Enabled = True
    End Sub

    Private Sub frmStepImages_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If radプレビュー2.Checked Then
            _ImgSel = 2
        Else
            _ImgSel = 1
        End If
        _filename = txtファイル名.Text

        __paras.SetLastData("frmStepImagesNoDispColor", cmb非表示色.Text)
        __paras.SetLastData("frmStepImages", Me.Size)
    End Sub

    Private Sub btn列を表示_Click(sender As Object, e As EventArgs) Handles btn列を表示.Click
        If _commonActions IsNot Nothing Then
            _commonActions.SetDrawOrder(True)
            Me.Close()
            Exit Sub
        End If
    End Sub

    Private Sub btn列を非表示_Click(sender As Object, e As EventArgs) Handles btn列を非表示.Click
        If _commonActions IsNot Nothing Then
            _commonActions.SetDrawOrder(False)
            Me.Close()
            Exit Sub
        End If
    End Sub

    Private Sub btnフォルダクリア_Click(sender As Object, e As EventArgs) Handles btnフォルダクリア.Click
        If Not isExistDestFolder(False) Then
            Exit Sub
        End If

        Dim folderPath As String = txt生成先フォルダ.Text.Trim()

        'SearchOption.TopDirectoryOnly は現在のフォルダ直下のみを検索
        Dim gifFiles As String() = Directory.GetFiles(folderPath, "*" & TARGET_EXT, SearchOption.TopDirectoryOnly)
        If gifFiles.Length = 0 Then
            addFormatMessage("生成先フォルダ'{0}'内に {1} ファイルはありません。", folderPath, TARGET_EXT)
            Exit Sub
        End If

        '削除の可否を質問
        Dim message As String = String.Format("{0}ファイル{1}点を削除します。よろしいですか？", TARGET_EXT, gifFiles.Length)
        Dim result As DialogResult = MessageBox.Show(message, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            Dim deletedCount As Integer = 0
            Dim failedFiles As New List(Of String)()

            For Each filePath In gifFiles
                Try
                    File.Delete(filePath)
                    deletedCount += 1
                Catch ex As Exception
                    ' 削除に失敗した場合（ファイルが使用中など）
                    failedFiles.Add(Path.GetFileName(filePath))
                End Try
            Next
            addFormatMessage("{0}ファイル{1}点を削除しました。", TARGET_EXT, deletedCount)
            If 0 < failedFiles.Count Then
                addFormatMessage("{0}点のファイル削除に失敗しました。", failedFiles.Count)
                For i As Integer = 0 To failedFiles.Count - 1
                    addFormatMessage("  {0}. {1}", i + 1, failedFiles(i))
                Next
            End If
        End If
    End Sub

    Private Sub btn生成先フォルダ_Click(sender As Object, e As EventArgs) Handles btn生成先フォルダ.Click
        FolderBrowserDialog1.SelectedPath = txt生成先フォルダ.Text
        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            txt生成先フォルダ.Text = FolderBrowserDialog1.SelectedPath
            _FolderSel = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub btnフォルダを開く_Click(sender As Object, e As EventArgs) Handles btnフォルダを開く.Click
        If Not isExistDestFolder(True) Then
            Exit Sub
        End If

        Dim folderPath As String = txt生成先フォルダ.Text.Trim()
        Try
            Process.Start("explorer.exe", folderPath)

        Catch ex As Exception
            addFormatMessage("フォルダ'{0}'のオープンに失敗しました。", folderPath)
            addMessage(ex.Message)
            Return
        End Try
    End Sub

    '作成先がセットされており、フォルダが存在する状態だとTrueを返す(isCreate=Trueなら作成)
    Private Function isExistDestFolder(ByVal isCreate As Boolean) As Boolean
        clearMessage()
        Dim folderPath As String = txt生成先フォルダ.Text.Trim()

        If String.IsNullOrWhiteSpace(folderPath) Then
            addMessage("生成先フォルダが指定されていません。")
            Return False
        End If

        If Not Directory.Exists(folderPath) Then
            If isCreate Then
                Try
                    Directory.CreateDirectory(folderPath)
                Catch ex As Exception
                    addFormatMessage("フォルダ'{0}'の作成に失敗しました。{1}{2}", folderPath, vbCrLf, ex.Message)
                    Return False
                End Try
            Else
                addFormatMessage("生成先フォルダ'{0}'が存在しません。", folderPath)
                Return False
            End If
        End If
        Return True
    End Function

    'ファイル名
    Private Function getFilePath(ByVal n As Integer) As String
        Dim filename As String = String.Format("{0}_{1:D4}", txtファイル名.Text.Trim(), n)
        Return IO.Path.Combine(txt生成先フォルダ.Text.Trim(), filename & TARGET_EXT)
    End Function

    Private Sub btn画像生成_Click(sender As Object, e As EventArgs) Handles btn画像生成.Click
        If _commonActions Is Nothing Then
            Exit Sub
        End If
        If Not isExistDestFolder(True) Then
            Exit Sub
        End If

        Dim color As String = cmb非表示色.Text.Trim()
        If String.IsNullOrEmpty(color) Then
            addMessage("非表示色を選択してください。")
            Exit Sub
        End If

        Dim n As Integer = CInt(nud表示番号.Value)
        Dim memo As String = Nothing
        Dim count As Integer = CountDispStepRecord(_dataCurrent, n, memo)
        addFormatMessage("表示番号<{0}>のレコード数は {1}", n, count)
        addMessage(memo)

        Dim filePath As String = getFilePath(n)
        addMessage(filePath)
        Application.DoEvents()


        Cursor = Cursors.WaitCursor
        Me.Enabled = False

        Dim errMsg As String = Nothing
        Dim ret As Boolean
        If radプレビュー2.Checked Then
            ret = _commonActions.MakeImageFile2(n, color, filePath, errMsg)
        Else
            ret = _commonActions.MakeImageFile(n, color, filePath, errMsg)
        End If
        Cursor = Cursors.Default
        Me.Enabled = True

        If ret Then
            addFormatMessage("表示番号<{0}> ファイル'{1}'の生成に成功しました。", n, filePath)

            Dim psi As New ProcessStartInfo()
            psi.FileName = filePath
            psi.UseShellExecute = True
            Process.Start(psi)

        Else
            addFormatMessage("表示番号<{0}> ファイル'{1}'の生成に失敗しました。", n, filePath)
            addMessage(errMsg)
        End If
    End Sub

    Private Sub btn一括生成_Click(sender As Object, e As EventArgs) Handles btn一括生成.Click
        If _commonActions Is Nothing Then
            Exit Sub
        End If
        If Not isExistDestFolder(True) Then
            Exit Sub
        End If

        Dim color As String = cmb非表示色.Text.Trim()
        If String.IsNullOrEmpty(color) Then
            addMessage("非表示色を選択してください。")
            Exit Sub
        End If

        Dim nFrom As Integer = CInt(nudFrom.Value)
        Dim nTo As Integer = CInt(nudTo.Value)
        If nFrom > nTo Then
            addMessage("開始番号は終了番号以下にしてください。")
            Exit Sub
        End If


        Cursor = Cursors.WaitCursor
        Enabled = False

        Dim generate As Integer = 0
        For n As Integer = nFrom To nTo
            Dim memo As String = Nothing
            Dim skip As String = Nothing
            Dim count As Integer = CountDispStepRecord(_dataCurrent, n, memo)
            If count = 0 AndAlso Not (n = nFrom OrElse n = nTo) Then
                skip = "skip"
            End If
            addFormatMessage("ステップ画像生成中 {0}/({1},{2}) {3}", n, nFrom, nTo, skip)
            addMessage(memo)
            Application.DoEvents()
            If Not String.IsNullOrEmpty(skip) Then
                Continue For
            End If

            Dim filePath As String = getFilePath(n)
            Dim ret As Boolean
            Dim errMsg As String = Nothing
            If radプレビュー2.Checked Then
                ret = _commonActions.MakeImageFile2(n, color, filePath, errMsg)
            Else
                ret = _commonActions.MakeImageFile(n, color, filePath, errMsg)
            End If

            If Not ret Then
                addFormatMessage("表示番号<{0}> ファイル'{1}'の生成に失敗しました。", n, filePath)
                addMessage(errMsg)
                Exit For
            End If
            generate += 1
        Next
        addFormatMessage("画像一括生成完了 {0}枚", generate)

        Cursor = Cursors.Default
        Enabled = True

    End Sub

    Private Function clearMessage() As Boolean
        txtMessage.Text = ""
        Return True
    End Function

    Private Function addMessage(ByVal msg As String) As Boolean
        If String.IsNullOrEmpty(msg) Then
            Return False
        End If

        'カーソルを末尾に移動し、選択を解除
        txtMessage.SelectionStart = txtMessage.TextLength
        txtMessage.SelectionLength = 0

        '追記する文字列
        Dim textToAppend As String = msg
        If Not String.IsNullOrEmpty(txtMessage.Text) Then
            textToAppend = Environment.NewLine & msg
        End If

        'テキストを追記
        txtMessage.AppendText(textToAppend)

        '末尾を表示
        txtMessage.ScrollToCaret()

        Return True
    End Function

    Public Sub addFormatMessage(ByVal format As String, ByVal ParamArray args() As Object)
        Dim msg As String = String.Format(format, args)
        addMessage(msg)
    End Sub

End Class