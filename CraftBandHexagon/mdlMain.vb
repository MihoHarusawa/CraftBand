

Imports CraftBand
Imports CraftBand.clsDataTables

Module mdlMain

    Dim _MainForm As frmMain

    Function Main(ByVal CmdArgs() As String) As Integer
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
        paras.ListOutMark = My.Settings.ListOutMark
        paras.MasterTablesFilePath = My.Settings.MasterTablesFilePath

        '*DLL初期化
        If Not mdlDllMain.Initialize(paras) Then
            'DLLエラー
            MessageBox.Show(paras.Message, My.Resources.TitleDllError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            '次にやり直せるよう名前を消去
            My.Settings.MasterTablesFilePath = String.Empty
            My.Settings.Save()
            Return 99 '実行不可
        End If

        'フォーム表示
        _MainForm = New frmMain
        If 0 < CmdArgs.Count Then '#102
            _MainForm._sFilePath = CmdArgs(0)
        End If
        Dim result As DialogResult = _MainForm.ShowDialog
        'ダイアログ表示終了
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Steps, "LOG_FormEnd", result)

        Dim errcode As Integer = 0
        If result = DialogResult.OK Then
            errcode = 0
            If mdlDllMain.Finalize(True) Then
                My.Settings.ListOutMark = paras.ListOutMark
                My.Settings.MasterTablesFilePath = paras.MasterTablesFilePath
                My.Settings.LastDataDll = paras.LastDataString()
            Else
                errcode = 9 'DLLエラー
            End If
            My.Settings.Save()

        Else
            errcode = 1
            If Not mdlDllMain.Finalize(False) Then
                errcode = 9 'DLLエラー
            End If
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

End Module
