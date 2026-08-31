Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button

Public Class frmSameSelect

    Public Enum EnumSameSelect
        Same_縦横
        Same_対角線
        Same_クリア
    End Enum


    Sub New(ByVal initialSelection As EnumSameSelect, ByVal errMsg As String)
        InitializeComponent()

        ' 受け取ったEnumの値に応じてラジオボタンを選択状態にする
        Select Case initialSelection
            Case EnumSameSelect.Same_縦横
                rad縦横同一.Checked = True
            Case EnumSameSelect.Same_対角線
                rad対角線同一.Checked = True
            Case Else
                radクリア.Checked = True
        End Select

        If Not String.IsNullOrEmpty(errMsg) Then
            lbl長さ指定.Enabled = False
            lblエラー.Visible = True
            lblエラー.Text = errMsg
            radクリア.Checked = True
            rad縦横同一.Enabled = False
            rad対角線同一.Enabled = False
        End If
    End Sub

    Public ReadOnly Property SelectedResult As EnumSameSelect
        Get
            If rad縦横同一.Checked Then Return EnumSameSelect.Same_縦横
            If rad対角線同一.Checked Then Return EnumSameSelect.Same_対角線

            Return EnumSameSelect.Same_クリア ' 万が一のフォールバック
        End Get
    End Property


    Private Sub frmSameSelect_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmSameSelect_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

    End Sub
End Class