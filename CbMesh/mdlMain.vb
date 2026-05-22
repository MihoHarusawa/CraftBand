

Imports System.Runtime.InteropServices
Imports CraftBand
Imports CraftBand.DllParameters
Imports CraftBand.Tables

Module mdlMain
    Private Const ATTACH_PARENT_PROCESS As Integer = -1

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function AttachConsole(dwProcessId As Integer) As Boolean
    End Function

    Dim _MainForm As frmMain


    <STAThread()>
    Public Function Main(ByVal CmdArgs() As String) As Integer
        Dim endCode As Integer
        Dim cmdArg As New clsCommandLine(CmdArgs)
        Debug.WriteLine(cmdArg.dump)

        If cmdArg.IsHeadlessMode AndAlso Not cmdArg.IsValid Then
            '親コンソールがあれば接続して出力。
            AttachConsole(ATTACH_PARENT_PROCESS)
            Console.WriteLine(cmdArg.Message)
            Return DllParameters.ProcessCode.InvalidArgument '引数エラー
        End If

#If DEBUG Then

        endCode = MainMain(cmdArg)
#Else
        Try
            endCode = MainMain(cmdArg)

        Catch ex As Exception
            '例外発生(まだlogが生きていたら)
            If (g_clsLog IsNot Nothing) Then
                g_clsLog.LogException(ex, "Main")
                g_clsLog.Close()
                g_clsLog = Nothing
            End If
            If cmdArg.IsHeadlessMode Then
                AttachConsole(ATTACH_PARENT_PROCESS)
                Console.WriteLine(ex.Message)
            Else
                MessageBox.Show(ex.Message, My.Resources.TitleException, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            endCode = DllParameters.ProcessCode.Exception '9    '例外

        Finally
            _MainForm = Nothing

        End Try
#End If

        Return endCode
    End Function


    Public Function MainMain(ByVal cmdArg As clsCommandLine) As Integer

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
        If Not mdlDllMain.Initialize(paras, cmdArg) Then
            'DLLエラー
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "DllInitialize Error: {0}", paras.Message)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "cmdArg: {0}", cmdArg.dump)

            If cmdArg.IsHeadlessMode Then
                AttachConsole(ATTACH_PARENT_PROCESS)
                Console.WriteLine(paras.Message)
            Else
                MessageBox.Show(paras.Message, My.Resources.TitleDllError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            Return DllParameters.ProcessCode.DllInitializeError ' 99 '実行不可
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CommandLine: {0}", cmdArg.dump)

        '元のコマンドラインのうち、EXE名以降の引数部分
        Dim originalArgs As String = cmdArg.OriginalCommandLine()
        Dim isStarted As Boolean = False
        Dim errcode As Integer = ProcessCode.InvalidData 'ヘッドレス用

        'データ指定があるかどうか
        Dim ename As enumExeName = enumExeName.Nodef
        Dim message As String = String.Empty
        If Not String.IsNullOrWhiteSpace(cmdArg.DataPath) AndAlso IO.File.Exists(cmdArg.DataPath) Then
            'あれば、形式とEXE名フィールドをチェック→ename
            Dim dst As dstDataTables = clsDataTables.PreLoad(cmdArg.DataPath, ename, message)
            If dst IsNot Nothing Then
                dst.Dispose()

                '対象EXEが確定
                If cmdArg.GetDataPathArray.Count = 1 Then
                    '1ファイルのみ指定なら即起動
                    If StartCraftbandExeWithArguments(ename, originalArgs, message) Then
                        '起動できた
                        isStarted = True
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "Start Exe({0}) : {1}", ename, originalArgs)
                        message = "Start Exe :" & ename.ToString
                        errcode = ProcessCode.NormalEnd
                    Else
                        errcode = ProcessCode.HeadlessExecuteError
                    End If
                Else
                    '複数ファイル指定なら即起動できない
                    message = "MultipleData:" & String.Join(",", cmdArg.GetDataPathArray)
                End If
            Else
                '非対象データ
                message = "InvalidData:" & cmdArg.DataPath
            End If
        Else
            'データがないので対象が判別できない
            message = "NoData to Execute"
        End If

        '個別EXEの起動なし
        If Not isStarted Then
            'ヘッドレスでなければフォームによるユーザー操作
            If Not cmdArg.IsHeadlessMode Then
                _MainForm = New frmMain
                _MainForm._cmdArgs = cmdArg.GetDataPathArray
                Dim result As DialogResult = _MainForm.ShowDialog
                'ダイアログ表示終了
                g_clsLog.LogResourceMessage(clsLog.LogLevel.Steps, "LOG_FormEnd", result)
                If result = DialogResult.OK Then
                    errcode = ProcessCode.NormalEnd
                Else
                    errcode = DllParameters.ProcessCode.DialogResultNG
                End If
            End If
        End If

        '終了処理
        If cmdArg.IsHeadlessMode Then
            'ノーチェック
            mdlDllMain.Finalize(False)
            If Not String.IsNullOrWhiteSpace(message) Then
                AttachConsole(ATTACH_PARENT_PROCESS)
                Console.WriteLine(message)
            End If
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "Headless errcode({0}) {1}", errcode, message)

        Else
            '結果で前回値保存
            If mdlDllMain.Finalize(False) Then
                'ListOutMark,MasterTablesFilePathは使いません
                My.Settings.LastDataDll = paras.LastDataString()
                If errcode = ProcessCode.NormalEnd Then
                    My.Settings.Save()
                End If
            Else
                errcode = DllParameters.ProcessCode.DllFinalizeError
                message = paras.Message
            End If
        End If

        '後処理完了
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Steps, "LOG_Finalized", errcode)
        g_clsLog.Close()
        g_clsLog = Nothing

        Return errcode
    End Function

    '*シリーズEXEの起動

    'ファイル指定で起動。すでに起動している場合はアクティブにしてファイルパスを送る。
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

        'コマンドライン
        Dim arguments As String = String.Empty
        If String.IsNullOrEmpty(fpath) Then
            If My.Settings.IsStartEmpty Then
                arguments = "/N"
            End If
        Else
            If IO.File.Exists(fpath) Then
                arguments = """" & fpath & """"
            End If
        End If

        Dim errmsg As String = String.Empty
        Return StartCraftbandExeWithArguments(enumExe, arguments, errmsg)
    End Function

    'コマンドライン指定で起動。
    Private Function StartCraftbandExeWithArguments(ByVal enumExe As enumExeName, ByVal arguments As String, ByRef errmsg As String) As Boolean
        If Not IsCraftBandExe(enumExe) Then
            Return False
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
        startinfo.Arguments = arguments

        Try
            Dim p As Process = Process.Start(startinfo)
            If p Is Nothing Then
                errmsg = "Failed to start process."
                Return False
            End If
            Threading.Thread.Sleep(500)
            If p.HasExited Then
                errmsg = errmsg & "Process exited immediately. ExitCode=" & p.ExitCode
                Return False
            End If
            Dim procs() As Process = Process.GetProcessesByName(enumExe.ToString)
            If 0 < procs.Length Then
                Return True
            End If
            errmsg = "Process started but not found - " & enumExe.ToString
            Return False

        Catch ex As Exception
            g_clsLog.LogException(ex, "mdlMain.StartCraftbandExeWithArguments", enumExe, arguments)
            errmsg = startinfo.FileName & ":" & ex.Message
            Return False

        End Try
    End Function


End Module
