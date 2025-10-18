Imports CraftBand

Public Class frmSettings
    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
End Class