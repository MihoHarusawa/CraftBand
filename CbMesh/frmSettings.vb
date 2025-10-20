Imports System.Runtime.InteropServices
Imports IWshRuntimeLibrary
Imports CraftBand

Public Class frmSettings
    Public Property IsSettingsChanged As Boolean = False

    Dim _isFormLoaded As Boolean = False
    Dim _isControlChanged As Boolean = False

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

#If DEBUG Then
        lbl実行ファイルフォルダ.Visible = True
        txt実行ファイルフォルダ.Visible = True
        btn実行ファイルフォルダ.Visible = True
#End If
        '設定値の読み込み
        chkアプリを起動したら閉じる.Checked = My.Settings.IsCloseAtStart
        chk編集対象なしで起動する.Checked = My.Settings.IsStartEmpty
        chk画面の表示を残す.Checked = My.Settings.IsMessageNoClear
        txt実行ファイルフォルダ.Text = My.Settings.ExeDirectory

        _isFormLoaded = True
    End Sub


    Private Sub btn実行ファイルフォルダ_Click(sender As Object, e As EventArgs) Handles btn実行ファイルフォルダ.Click
        FolderBrowserDialog1.SelectedPath = txt実行ファイルフォルダ.Text
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            txt実行ファイルフォルダ.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub btn終了_Click(sender As Object, e As EventArgs) Handles btn終了.Click
        '設定値の保存
        My.Settings.IsCloseAtStart = chkアプリを起動したら閉じる.Checked
        My.Settings.IsMessageNoClear = chk画面の表示を残す.Checked
        My.Settings.IsStartEmpty = chk編集対象なしで起動する.Checked
        If Not String.IsNullOrWhiteSpace(txt実行ファイルフォルダ.Text) AndAlso IO.Directory.Exists(txt実行ファイルフォルダ.Text) Then
            My.Settings.ExeDirectory = txt実行ファイルフォルダ.Text
        Else
            My.Settings.ExeDirectory = ""
        End If
        IsSettingsChanged = _isControlChanged
    End Sub

    Private Sub picGear_Click(sender As Object, e As EventArgs) Handles picGear.Click
        Dim sb As New System.Text.StringBuilder

        For Each enumExe As enumExeName In GetType(enumExeName).GetEnumValues
            If Not IsCraftBandExe(enumExe) Then
                Continue For
            End If
            Dim masterpath As String = GetCommonMasterPath(enumExe)
            If Not [String].IsNullOrWhiteSpace(masterpath) Then
                sb.AppendFormat("{0} :{1}", enumExe.ToString, masterpath).AppendLine()
            End If
        Next
        If 0 < sb.Length Then
            MessageBox.Show(sb.ToString, g_clsLog.GetResourceString("LOG_MasterConfigFile"), MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub btnShortCut_Click(sender As Object, e As EventArgs) Handles btnShortCut.Click

        For Each enumExe As enumExeName In GetType(enumExeName).GetEnumValues
            If Not IsCraftBandExe(enumExe) Then
                Continue For
            End If
            Dim targetExe As String
            If String.IsNullOrWhiteSpace(txt実行ファイルフォルダ.Text) Then
                targetExe = IO.Path.Combine(IO.Path.GetDirectoryName(g_clsLog.ExePath), GetCraftBandExeName(enumExe))
            Else
                targetExe = IO.Path.Combine(txt実行ファイルフォルダ.Text, GetCraftBandExeName(enumExe))
            End If
            CreateShortcut(targetExe, GetShortExeName(enumExe), GetCraftBandExeName(enumExe))
        Next


    End Sub

    Public Function CreateShortcut(ByVal targetPath As String, ByVal shortcutName As String, ByVal shortcutDescription As String) As Boolean
        If Not IO.File.Exists(targetPath) Then
            Return False
        End If

        Try
            Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            Dim shortcutPath As String = IO.Path.Combine(desktopPath, shortcutName & ".lnk")

            Dim WshShell As New WshShell()
            Dim shortcut As IWshShortcut = CType(WshShell.CreateShortcut(shortcutPath), IWshShortcut)
            shortcut.TargetPath = targetPath
            shortcut.WorkingDirectory = IO.Path.GetDirectoryName(targetPath)
            shortcut.Description = shortcutDescription
            shortcut.Save()
            Return True

        Catch ex As COMException
            g_clsLog.LogException(ex, "CreateShortcut.COMException")
        Catch ex As Exception
            g_clsLog.LogException(ex, "CreateShortcut")
        End Try
        Return False
    End Function

    Private Sub txt実行ファイルフォルダ_TextChanged(sender As Object, e As EventArgs) Handles txt実行ファイルフォルダ.TextChanged
        If _isFormLoaded Then
            _isControlChanged = True
        End If
    End Sub

    Private Sub chk_CheckedChanged(sender As Object, e As EventArgs) Handles chkアプリを起動したら閉じる.CheckedChanged, chk編集対象なしで起動する.CheckedChanged, chk画面の表示を残す.CheckedChanged
        If _isFormLoaded Then
            _isControlChanged = True
        End If
    End Sub
End Class