Imports System.Drawing

Public Class frmMasterDescription

    Private Sub frmMasterDescription_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = g_clsMasterTables.MasterTablesFilePath
        txt識別情報.Text = g_clsMasterTables.MasterDescription
        txt備考.Text = g_clsMasterTables.MasterAppendix

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmMasterDescription", siz) Then
            Me.Size = siz
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        g_clsMasterTables.MasterDescription = txt識別情報.Text
        g_clsMasterTables.MasterAppendix = txt備考.Text
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frmMasterDescription_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmMasterDescription", Me.Size)
    End Sub
End Class