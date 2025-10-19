Imports System.Drawing

Public Class ctrMemo

    'Panelを置き、各ControlはPanelにAnchorし、Panelをコードでリサイズする
    '※ユーザーコントロールとしてのサイズでは制御できない・表示がずれる

    Public Property PanelSize As Drawing.Size
        Set(value As Drawing.Size)
            If Not _isLoadingData Then
                Panel.Size = New Size(value.Width, value.Height)
            End If
        End Set
        Get
            Return Panel.Size
        End Get
    End Property

    Dim _isLoadingData As Boolean = True 'Designer.vb描画

    Private Sub ctrMemo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '※フォームのデザイン時にもLoadされますので、グローバル参照値は参照できない
        _isLoadingData = False 'Designer.vb描画完了
    End Sub

    Public Sub DispMemo(ByVal row目標寸法 As clsDataRow)
        txtメモ.Text = row目標寸法.Value("f_sメモ")
        txtタイトル.Text = row目標寸法.Value("f_sタイトル")
        txt作成者.Text = row目標寸法.Value("f_s作成者")
    End Sub

    Public Sub SaveMemo(ByVal row目標寸法 As clsDataRow)
        row目標寸法.Value("f_sメモ") = txtメモ.Text
        row目標寸法.Value("f_sタイトル") = txtタイトル.Text
        row目標寸法.Value("f_s作成者") = txt作成者.Text
    End Sub

    Public Sub FileSaveAs(ByVal filename As String)
        If String.IsNullOrEmpty(txtタイトル.Text) AndAlso String.IsNullOrEmpty(txt作成者.Text) Then
            txtタイトル.Text = IO.Path.GetFileNameWithoutExtension(filename)
            txt作成者.Text = Environment.UserName
        End If
    End Sub


End Class
