Imports System.Drawing
Imports System.Windows.Forms

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

        '#105
        txtロゴ文字列.Text = row目標寸法.Value("f_sロゴ文字列")
        Dim picstr As String = row目標寸法.Value("f_sロゴ画像")
        If Not String.IsNullOrWhiteSpace(picstr) Then
            picロゴ画像.Image = frmColor.CompressedBase64ToImage(picstr)
        Else
            picロゴ画像.Image = Nothing
        End If

    End Sub

    Public Sub SaveMemo(ByVal row目標寸法 As clsDataRow)
        row目標寸法.Value("f_sメモ") = txtメモ.Text
        row目標寸法.Value("f_sタイトル") = txtタイトル.Text
        row目標寸法.Value("f_s作成者") = txt作成者.Text

        '#105
        row目標寸法.Value("f_sロゴ文字列") = txtロゴ文字列.Text

        Dim img As Image = picロゴ画像.Image
        If img Is Nothing Then
            row目標寸法.Value("f_sロゴ画像") = ""
        Else
            Dim b64 As String = frmColor.ImageToCompressedBase64(img, Imaging.ImageFormat.Png)
            row目標寸法.Value("f_sロゴ画像") = b64
        End If

    End Sub

    Public Sub FileSaveAs(ByVal filename As String)
        If String.IsNullOrEmpty(txtタイトル.Text) AndAlso String.IsNullOrEmpty(txt作成者.Text) Then
            txtタイトル.Text = IO.Path.GetFileNameWithoutExtension(filename)
            txt作成者.Text = Environment.UserName
        End If
    End Sub

    '#105
    Private Sub picロゴ画像_Click(sender As Object, e As EventArgs) Handles picロゴ画像.Click
        'png画像を選択
        If OpenFileDialogPng.ShowDialog() <> DialogResult.OK Then
            'もし画像があれば、クリアするか問い合わせる
            If picロゴ画像.Image IsNot Nothing Then
                '現在の画像をクリアしますか？
                If MessageBox.Show(My.Resources.AskClearCurrentTexture, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    picロゴ画像.Image = Nothing
                End If
            End If
            Exit Sub
        End If

        '選択されたファイルパス
        Dim filepath As String = OpenFileDialogPng.FileName
        If Not System.IO.File.Exists(filepath) Then
            Exit Sub
        End If

        '画像ファイルを読み取り、BASE64変換チェック後、picロゴ画像に設定
        Const MaxImageStringLen As Integer = 500 * 1024 '500KB
        Dim img As Image = Nothing
        Try
            img = Image.FromFile(filepath)
            Dim b64 As String = frmColor.ImageToCompressedBase64(img, Imaging.ImageFormat.Png)
            If b64.Length > MaxImageStringLen Then
                '画像のサイズ({0})が大きすぎます。小さくしてください。
                Dim msg As String = String.Format(My.Resources.ErrMsgColorFileTooLarge, b64.Length)
                MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            'セット
            picロゴ画像.Image = img

        Catch ex As Exception
            g_clsLog.LogException(ex, "ctrMemo.picロゴ画像_Click")
            '画像ファイル'{0}'を読み取れませんでした。
            Dim msg As String = String.Format(My.Resources.ErrMsgColorBadTexture, filepath)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            'クリア
            picロゴ画像.Image = Nothing
            Exit Sub
        End Try
    End Sub
End Class
