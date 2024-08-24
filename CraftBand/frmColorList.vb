Imports System.Drawing
Imports System.Windows.Forms

Public Class frmColorList

    Public Property BandTypeName As String
    Public Property ColorString As String


    Private Sub frmColorList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = BandTypeName

        '現在の描画色
        '+共通色
        Dim collist As New List(Of String)
        collist.AddRange(g_clsMasterTables.GetColorNames(Nothing))

        '+バンドの種類の色
        For Each col As String In g_clsMasterTables.GetColorNames(BandTypeName)
            If Not collist.Contains(col) Then
                collist.Add(col)
            End If
        Next
        collist.Sort()
        clb使用色.Items.AddRange(collist.ToArray)

        '選択された色
        Dim ary() As String = ColorString.Split(",")
        For Each s As String In ary
            Dim index As Integer = clb使用色.FindStringExact(s)
            If 0 <= index Then
                clb使用色.SetItemChecked(index, True)
            End If
        Next

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmColorList", siz) Then
            Me.Size = siz
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Dim collist As New List(Of String)
        For Each checkedItem As Object In clb使用色.CheckedItems
            collist.Add(checkedItem.ToString())
        Next

        ColorString = String.Join(",", collist.ToArray)
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub

    Private Sub frmColorList_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmColorList", Me.Size)
    End Sub

    Private Sub btnチェック反転_Click(sender As Object, e As EventArgs) Handles btnチェック反転.Click
        For i As Integer = 0 To clb使用色.Items.Count - 1
            Dim isChecked As Boolean = clb使用色.GetItemChecked(i)
            clb使用色.SetItemChecked(i, Not isChecked)
        Next
    End Sub
End Class