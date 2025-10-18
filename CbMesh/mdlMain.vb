

Imports CraftBand
Imports CraftBand.Tables

Module mdlMain

    Dim _MainForm As frmMain


    <STAThread()>
    Public Function Main(ByVal CmdArgs() As String) As Integer
        Dim endCode As Integer


#If DEBUG Then

        endCode = MainMain(CmdArgs)
#Else
        Try
            endCode = mainmain(CmdArgs)

        Catch ex As Exception
            g_clsLog.LogException(ex, "Main")
            '例外発生
            MessageBox.Show(ex.Message, My.Resources.TitleException, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            endCode = 9    '例外

        Finally
            _MainForm = Nothing

        End Try
#End If

        Return endCode
    End Function


    Public Function MainMain(ByVal CmdArgs() As String) As Integer

        'バージョンアップ対応
        If My.Settings.UpdateRequired Then
            My.Settings.Upgrade()
            My.Settings.UpdateRequired = False
            My.Settings.Save()
        End If

        '*ログファイル
        Dim myLog As New clsMyLog(My.Settings.LogLevel)

        '*DLL共通パラメータ
        Dim paras As New DllParameters(My.Settings.LastDataDll)
        '
        paras.Log = myLog
        '
        'ListOutMark,MasterTablesFilePathは使いません

        '*DLL初期化
        If Not mdlDllMain.Initialize(paras) Then
            'DLLエラー
            MessageBox.Show(paras.Message, My.Resources.TitleDllError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return 99 '実行不可
        End If

        Dim isStarted As Boolean = False
        Dim errcode As Integer = 0

        '引数1点(アイコンのダブルクリック)
        If CmdArgs IsNot Nothing AndAlso CmdArgs.Count = 1 Then
            Dim fpath As String = CmdArgs(0)
            If IO.File.Exists(fpath) Then
                Dim ename As enumExeName = enumExeName.Nodef
                Dim errmsg As String = String.Empty
                '形式とEXE名フィールドをチェック
                Dim dst As dstDataTables = clsDataTables.PreLoad(fpath, ename, errmsg)
                If dst IsNot Nothing Then
                    dst.Dispose()
                    If StartCraftbandExe(ename, fpath) Then
                        '起動できた
                        isStarted = True
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "StartCraftbandExe({0}){1}", ename, fpath)
                    End If
                End If
            End If
        End If

        If Not isStarted Then
            'フォームによる処理
            _MainForm = New frmMain
            If CmdArgs IsNot Nothing AndAlso 0 < CmdArgs.Count Then
                _MainForm._cmdArgs = CmdArgs
            End If
            Dim result As DialogResult = _MainForm.ShowDialog
            'ダイアログ表示終了
            g_clsLog.LogResourceMessage(clsLog.LogLevel.Steps, "LOG_FormEnd", result)
            If result <> DialogResult.OK Then
                errcode = 1
            End If
        End If

        If mdlDllMain.Finalize(errcode = 0) Then
            'ListOutMark,MasterTablesFilePathは使いません
            My.Settings.LastDataDll = paras.LastDataString()
        Else
            errcode = 9 'DLLエラー
        End If
        If errcode = 0 Then
            My.Settings.Save()
        End If

        If errcode = 9 Then
            MessageBox.Show(paras.Message, My.Resources.TitleDllError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        '後処理完了
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Steps, "LOG_Finalized", errcode)
        g_clsLog.Close()
        g_clsLog = Nothing

        Return errcode
    End Function

    'シリーズEXEの起動
    Function StartCraftbandExe(ByVal enumExe As enumExeName, ByVal fpath As String) As Boolean
        If Not IsCraftBandExe(enumExe) Then
            Return False
        End If

        '同名のプロセスが起動中か？
        Dim procname As String = enumExe.ToString
        Dim procs() As Process = Process.GetProcessesByName(procname)
        If procs.Length > 0 Then
            '起動中のプロセスをアクティブにする
            For Each p As Process In procs
                If p.MainWindowHandle <> IntPtr.Zero Then
                    mdlProcess.ActivateWindow(p.MainWindowHandle)
                    'ファイルパスを送る
                    If Not String.IsNullOrWhiteSpace(fpath) AndAlso IO.File.Exists(fpath) Then
                        mdlProcess.SendFilePath(p.MainWindowHandle, fpath)
                    End If
                    Exit For
                End If
            Next
            Return True ' 既に起動している
        End If

        '起動
        Dim startinfo As New ProcessStartInfo
        startinfo.FileName = GetCraftBandExeName(enumExe)

        If Not String.IsNullOrWhiteSpace(My.Settings.ExeDirectory) AndAlso
            IO.Directory.Exists(My.Settings.ExeDirectory) Then
            Dim exepath As String = IO.Path.Combine(My.Settings.ExeDirectory, startinfo.FileName)
            If IO.File.Exists(exepath) Then
                startinfo.FileName = exepath
            End If
        End If

        If String.IsNullOrEmpty(fpath) Then
            If My.Settings.IsStartEmpty Then
                startinfo.Arguments = "/N"
            End If
        Else
            If IO.File.Exists(fpath) Then
                startinfo.Arguments = """" & fpath & """"
            End If
        End If

        Try
            Process.Start(startinfo)
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "mdlMain.StartCraftbandExe", enumExe, fpath)
        End Try
        Return False
    End Function

End Module
