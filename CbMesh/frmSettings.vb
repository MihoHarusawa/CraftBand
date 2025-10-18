Imports CraftBand

Public Class frmSettings
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
    End Sub

    Private Sub picGear_Click(sender As Object, e As EventArgs) Handles picGear.Click
        Dim sb As New System.Text.StringBuilder

        For Each enumExe As enumExeName In GetType(enumExeName).GetEnumValues
            If Not IsCraftBandExe(enumExe) Then
                Continue For
            End If
            Dim masterpath As String = GetMasterPathFromLog(g_clsLog.FilePath, enumExe)
            If Not [String].IsNullOrWhiteSpace(masterpath) Then
                sb.AppendFormat("{0} :{1}", enumExe.ToString, masterpath).AppendLine()
            End If
        Next
        If 0 < sb.Length Then
            MessageBox.Show(sb.ToString, g_clsLog.GetResourceString("LOG_MasterConfigFile"), MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub
End Class