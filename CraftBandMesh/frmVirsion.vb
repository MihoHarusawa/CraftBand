
Imports CraftBand

Public Class frmVirsion
    Private Sub frmVirsion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim viExe As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(g_clsLog.ExePath)
        Dim viDll As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(g_clsLog.DllPath)

        lblVersion.Text = String.Format("FileVersion {0} / {1}", viExe.FileVersion, viDll.FileVersion)

    End Sub
End Class