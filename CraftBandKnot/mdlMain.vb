Imports System.Runtime.InteropServices
Imports CraftBand

Module mdlMain
    Private Const ATTACH_PARENT_PROCESS As Integer = -1

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function AttachConsole(dwProcessId As Integer) As Boolean
    End Function

    Dim _MainForm As frmMain

    Function Main(ByVal CmdArgs() As String) As Integer
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

        endCode = MainMain(CmdArg)
#Else
        Try
            endCode = MainMain(cmdArg)

        Catch ex As Exception
            '例外発生(まだlogが生きていたら)
            If (g_clsLog IsNot Nothing) Then
                g_clsLog.LogException(ex, "Main")
                g_clsLog.Close()
                g_clsLog = Nothing
            End if
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
        paras.ListOutMark = My.Settings.ListOutMark

        'コマンドライン引数のマスターテーブルのパスを優先する
        If Not String.IsNullOrEmpty(cmdArg.MasterPath) Then
            paras.MasterTablesFilePath = cmdArg.MasterPath
        Else
            paras.MasterTablesFilePath = My.Settings.MasterTablesFilePath
        End If

        '*DLL初期化
        If Not mdlDllMain.Initialize(paras, cmdArg) Then
            'DLLエラー(マスターテーブルの読み込みに失敗など)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "DllInitialize Error: {0}", paras.Message)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "cmdArg: {0}", cmdArg.dump)

            If cmdArg.IsHeadlessMode Then
                AttachConsole(ATTACH_PARENT_PROCESS)
                Console.WriteLine(paras.Message)
            Else
                MessageBox.Show(paras.Message, My.Resources.TitleDllError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                '次にやり直せるよう名前を消去
                If String.IsNullOrEmpty(cmdArg.MasterPath) Then
                    My.Settings.MasterTablesFilePath = String.Empty
                    My.Settings.Save()
                End If
            End If
            Return DllParameters.ProcessCode.DllInitializeError ' 99 '実行不可
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "cmdArg: {0}", cmdArg.dump)

        '*フォームのメイン処理とDLL後処理
        _MainForm = New frmMain
        Dim ret As Boolean = mdlDllMain.MainProcess(_MainForm)

        '前回値の保存
        If paras.IsSettingSave Then
            My.Settings.ListOutMark = paras.ListOutMark
            My.Settings.MasterTablesFilePath = paras.MasterTablesFilePath
            My.Settings.LastDataDll = paras.LastDataString()
            My.Settings.Save()
        End If

        'エラー・警告メッセージがあれば表示
        If Not String.IsNullOrEmpty(paras.Message) Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "MainProcess message({0}): {1}", ret, paras.Message)
            If cmdArg.IsHeadlessMode Then
                AttachConsole(ATTACH_PARENT_PROCESS)
                Console.WriteLine(paras.Message)
            ElseIf Not ret Then
                MessageBox.Show(paras.Message, My.Resources.TitleDllError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If

        '*後処理完了
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Steps, "LOG_Finalized", paras.EndCode)
        g_clsLog.Close()
        g_clsLog = Nothing

        Return paras.EndCode
    End Function



End Module
