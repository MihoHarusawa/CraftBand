Imports System.Globalization
Imports CraftBand
Imports CraftBand.mdlDllMain
Imports CraftBand.Tables

Public Class frmMain
    '編集中のファイルパス
    Friend _cmdArgs() As String = Nothing '起動時引数があればセット

    '表示色
    Dim _colorDefault As Color = Color.Black
    Dim _colorMasterInfo As Color = Color.DarkOrange
    Dim _colorDataInfo As Color = Color.Brown
    Dim _colorWarning As Color = Color.Red


#Region "メニューとボタン(ファイル以外)"
    Private Sub ToolStripMenuItemクリア_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemクリア.Click
        clearText()
    End Sub

    Private Sub ToolStripMenuItemFileExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileExit.Click
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub ToolStripMenuItemSetting_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSetting.Click
        Dim dlg As New frmSettings
        dlg.ShowDialog(Me)
    End Sub

    Private Sub ToolStripMenuItemHelp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemHelp.Click
        Dim dlg As New frmVersion
        dlg.ShowDialog(Me)
    End Sub
    Private Sub btnMesh_Click(sender As Object, e As EventArgs) Handles btnMesh.Click
        startButton(enumExeName.CraftBandMesh)
    End Sub

    Private Sub btnKnot_Click(sender As Object, e As EventArgs) Handles btnKnot.Click
        startButton(enumExeName.CraftBandKnot)
    End Sub

    Private Sub btnSquare45_Click(sender As Object, e As EventArgs) Handles btnSquare45.Click
        startButton(enumExeName.CraftBandSquare45)
    End Sub

    Private Sub btnSquare_Click(sender As Object, e As EventArgs) Handles btnSquare.Click
        startButton(enumExeName.CraftBandSquare)
    End Sub

    Private Sub btnHexagon_Click(sender As Object, e As EventArgs) Handles btnHexagon.Click
        startButton(enumExeName.CraftBandHexagon)
    End Sub
#End Region

#Region "ファイル指定"

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'サイズ復元
        Dim siz As Size = My.Settings.frmMainSize
        If 0 < siz.Height AndAlso 0 < siz.Width Then
            Me.Size = siz
        End If

        '処理方法選択復元
        Select Case My.Settings.TypeProcess
            Case 2
                rad旧拡張子変更.Checked = True
            Case 1
                rad情報を表示する.Checked = True
            Case Else
                radすぐに開く.Checked = True
        End Select

        '起動引数があれば処理
        If _cmdArgs IsNot Nothing Then
            fileStart()
            For Each fname In _cmdArgs
                If IO.File.Exists(fname) Then
                    fileAdd(fname)
                End If
            Next
            fileEnd()
        Else
            '[ファイル]メニューから[開く]もしくはドラッグ＆ドロップでファイルを指定してください。
            addText(My.Resources.MsgNeedFile)
        End If
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me.DialogResult = DialogResult.OK Then
            '正常終了
            If rad旧拡張子変更.Checked Then
                My.Settings.TypeProcess = 2
            ElseIf rad情報を表示する.Checked Then
                My.Settings.TypeProcess = 1
            Else
                My.Settings.TypeProcess = 0
            End If
            My.Settings.frmMainSize = Me.Size

        Else
            '×ボタンやAlt+F4で閉じた場合
            '変更は保存されませんがよろしいですか？(設定を保存するには[メニュー]から[終了]してください)
            Dim result As DialogResult = MessageBox.Show(My.Resources.MsgAskExit, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.No Then
                ' 閉じる処理をキャンセル
                e.Cancel = True
                'DialogResult を初期状態に戻す
                Me.DialogResult = DialogResult.None
            End If
        End If
    End Sub

    Private Sub ToolStripMenuItemFileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileOpen.Click
        Dim filename As String = mdlProcess.OpenDataFileDialog()
        If String.IsNullOrWhiteSpace(filename) Then
            Exit Sub
        End If
        fileStart()
        fileAdd(filename)
        fileEnd()
    End Sub

    Private Sub frmMain_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        'コントロール内にドラッグされたとき実行される
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーアイコン
            e.Effect = DragDropEffects.Copy
        Else
            'ファイル以外は受け付けない
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub frmMain_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim fileNames As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
        fileStart()
        For Each fname In fileNames
            fileAdd(fname)
        Next
        fileEnd()
    End Sub

#End Region

    '起動ボタン
    Private Sub startButton(ByVal enumExe As enumExeName)
        '起動対象
        Dim fpath As String = Nothing
        If _mapExeFpath.ContainsKey(enumExe) Then
            fpath = _mapExeFpath(enumExe)
            _mapExeFpath.Remove(enumExe)
        End If

        'プロクラム起動
        If StartCraftbandExe(enumExe, fpath) Then
            If My.Settings.IsCloseAtStart Then
                DialogResult = DialogResult.OK
                Close()
            End If
        End If
    End Sub

    '起動対象ファイルリスト
    Dim _mapExeFpath As New Dictionary(Of enumExeName, String)

    'ファイル処理開始
    Private Sub fileStart()
        _mapExeFpath.Clear()
        If Not My.Settings.IsMessageNoClear Then
            clearText()
        End If
    End Sub

    'ファイル処理終了
    Private Sub fileEnd()
        If _mapExeFpath.Count = 0 Then
            Return
        End If

        '起動指定以外は対象表示
        If Not radすぐに開く.Checked Then
            '起動可能なデータファイル:
            addFormatText(My.Resources.MsgStartable)
            For Each enumExe As enumExeName In _mapExeFpath.Keys
                Dim fpath As String = _mapExeFpath(enumExe)
                If String.IsNullOrEmpty(fpath) Then
                    Continue For '念のため
                End If
                '起動可能なデータファイル:
                addFormatText(" - {0} : {1}", GetShortExeName(enumExe), fpath)
            Next
            Return
        End If

        '起動する
        Dim errExist As Boolean = False
        For Each enumExe As enumExeName In _mapExeFpath.Keys
            Dim fpath As String = _mapExeFpath(enumExe)
            If String.IsNullOrEmpty(fpath) Then
                Continue For '念のため
            End If
            'プロクラム起動
            If StartCraftbandExe(enumExe, fpath) Then
                '{0}({1})を起動しました。
                addFormatText(My.Resources.MsgStartExe, GetShortExeName(enumExe), fpath)
            Else
                '{0}({1})を起動できませんでした。
                addFormatText(_colorWarning, My.Resources.MsgCouldNotStartExe, GetShortExeName(enumExe), fpath)
                errExist = True
            End If
        Next
        _mapExeFpath.Clear()

        'エラー表示は残す
        If Not errExist AndAlso My.Settings.IsCloseAtStart Then
            DialogResult = DialogResult.OK
            Close()
        End If
    End Sub

    'ファイル追加処理
    Private Sub fileAdd(ByVal fpath As String)
        addText(vbCrLf & "----- " & fpath)

        Dim enumExe As enumExeName = enumExeName.Nodef
        Dim errmsg As String = String.Empty

        '形式とEXE名フィールドをチェック
        Dim dst As dstDataTables = clsDataTables.PreLoad(fpath, enumExe, errmsg)
        If dst Is Nothing Then
            'データではない
            Dim description As String = String.Empty
            If clsMasterTables.IsValidMasterFile(fpath, description) Then
                'シリーズの設定ファイルです。
                addText(My.Resources.MsgSettingFile)
                addText(_colorMasterInfo, description)
                If rad旧拡張子変更.Checked Then
                    renameExtention(fpath, clsMasterTables.MyExtention)
                End If
            Else
                '設定ファイルでもない
                addText(_colorWarning, errmsg)
            End If
            Exit Sub

        Else
            '{0}のデータファイルです。
            addFormatText(My.Resources.MsgDataFile, GetShortExeName(enumExe))
            Dim row目標寸法 As clsDataRow = New clsDataRow(dst.Tables("tbl目標寸法").Rows(0))
            With row目標寸法
                ''{0}' {1}本幅 ({2} , {3} , {4})
                addFormatText(_colorDataInfo, My.Resources.MsgInfo1, .Value("f_sバンドの種類名"), .Value("f_i基本のひも幅"), .Value("f_d横寸法"), .Value("f_d縦寸法"), .Value("f_d高さ寸法"))
                'タイトル【{0}】{1}
                addFormatText(_colorDataInfo, My.Resources.MsgInfo2, .Value("f_sタイトル"), .Value("f_s作成者"))
                addText(_colorDataInfo, .Value("f_sメモ"))
            End With

            dst.Dispose()
        End If

        Dim procname As String = fpath
        If rad旧拡張子変更.Checked Then
            procname = renameExtention(fpath, clsDataTables.DataExtention)
        End If

        'プロクラム起動対象にセット
        If Not String.IsNullOrWhiteSpace(procname) AndAlso IO.File.Exists(procname) AndAlso
            IsCraftBandExe(enumExe) AndAlso Not _mapExeFpath.ContainsKey(enumExe) Then
            _mapExeFpath.Add(enumExe, procname)
        End If

    End Sub


    '拡張子のリネーム、fpath存在確認済み、ファイルパスを返す
    Private Function renameExtention(ByVal fpath As String, ByVal extention As String) As String
        '新拡張子
        Dim newpathExt As String = IO.Path.ChangeExtension(fpath, extention)
        If newpathExt.Equals(fpath, StringComparison.OrdinalIgnoreCase) Then
            '拡張子の変更は不要です。
            addText(My.Resources.MsgNoNeedChange)
            Return fpath
        End If
        '既存は不可
        If IO.File.Exists(newpathExt) Then
            'ファイル'{0}'があるため、拡張子を変更できません。
            addFormatText(_colorWarning, My.Resources.MsgCantChange, newpathExt)
            Return Nothing '除外
        End If

        'リネーム（移動）する
        Try
            IO.File.Move(fpath, newpathExt)
            ''{0}'に変更しました。
            addFormatText(My.Resources.MsgChanged, newpathExt)
            Return newpathExt

        Catch ex As Exception
            ''{0}'に変更できません -{1}
            addFormatText(_colorWarning, My.Resources.MsgChangeError, newpathExt, ex.Message)
            Return Nothing '除外
        End Try
    End Function

#Region "情報表示用"

    Private Function clearText() As Boolean
        rtxtMessage.Text = ""
        Return True
    End Function

    Private Function addText(ByVal msg As String) As Boolean
        Return addText(_colorDefault, msg)
    End Function

    Private Function addText(ByVal color As Color, ByVal msg As String) As Boolean
        If String.IsNullOrEmpty(msg) Then
            Return False
        End If

        'カーソルを末尾に移動し、選択を解除
        rtxtMessage.SelectionStart = rtxtMessage.TextLength
        rtxtMessage.SelectionLength = 0

        '色を設定
        rtxtMessage.SelectionColor = color

        '追記する文字列
        Dim textToAppend As String = msg
        If Not String.IsNullOrEmpty(rtxtMessage.Text) Then
            textToAppend = Environment.NewLine & msg
        End If

        'テキストを追記
        rtxtMessage.AppendText(textToAppend)
        'rtxtMessage.SelectionColor = _colorDefault

        '末尾を表示
        rtxtMessage.ScrollToCaret()

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, msg)
        Return True
    End Function

    Public Sub addFormatText(ByVal color As Color, ByVal format As String, ByVal ParamArray args() As Object)
        Dim msg As String = String.Format(format, args)
        addText(color, msg)
    End Sub

    Public Sub addFormatText(ByVal format As String, ByVal ParamArray args() As Object)
        Dim msg As String = String.Format(format, args)
        addText(msg)
    End Sub
#End Region

End Class
