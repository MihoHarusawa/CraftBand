Imports CraftBand

Public Class frmVirsion
    Private Sub frmVirsion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim viExe As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(g_clsLog.ExePath)
        Dim viDll As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(g_clsLog.DllPath)

        lblVersion.Text = String.Format("FileVersion {0} / {1}", viExe.FileVersion, viDll.FileVersion)

    End Sub

    Private Sub lnkLabo_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkLabo.LinkClicked
        lnkLabo.LinkVisited = True
        Try
            System.Diagnostics.Process.Start("cmd", "/C start " & lnkLabo.Text)
        Catch ex As Exception
            g_clsLog.LogException(ex, "frmVirsion.lnkLabo_LinkClicked", lnkLabo.Text)
        End Try
    End Sub


End Class