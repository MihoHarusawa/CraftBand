<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrMemo
    Inherits System.Windows.Forms.UserControl

    'UserControl はコンポーネント一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Panel = New Windows.Forms.Panel()
        lblロゴ文字列 = New Windows.Forms.Label()
        lblロゴ画像 = New Windows.Forms.Label()
        txtロゴ文字列 = New Windows.Forms.TextBox()
        picロゴ画像 = New Windows.Forms.PictureBox()
        txt作成者 = New Windows.Forms.TextBox()
        txtタイトル = New Windows.Forms.TextBox()
        lbl作成者 = New Windows.Forms.Label()
        lblタイトル = New Windows.Forms.Label()
        lblメモ = New Windows.Forms.Label()
        txtメモ = New Windows.Forms.TextBox()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        OpenFileDialogPng = New Windows.Forms.OpenFileDialog()
        Panel.SuspendLayout()
        CType(picロゴ画像, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Panel
        ' 
        Panel.Controls.Add(lblロゴ文字列)
        Panel.Controls.Add(lblロゴ画像)
        Panel.Controls.Add(txtロゴ文字列)
        Panel.Controls.Add(picロゴ画像)
        Panel.Controls.Add(txt作成者)
        Panel.Controls.Add(txtタイトル)
        Panel.Controls.Add(lbl作成者)
        Panel.Controls.Add(lblタイトル)
        Panel.Controls.Add(lblメモ)
        Panel.Controls.Add(txtメモ)
        Panel.Location = New System.Drawing.Point(3, 3)
        Panel.Name = "Panel"
        Panel.Size = New System.Drawing.Size(713, 339)
        Panel.TabIndex = 0
        ' 
        ' lblロゴ文字列
        ' 
        lblロゴ文字列.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        lblロゴ文字列.AutoSize = True
        lblロゴ文字列.Location = New System.Drawing.Point(166, 285)
        lblロゴ文字列.Name = "lblロゴ文字列"
        lblロゴ文字列.Size = New System.Drawing.Size(71, 19)
        lblロゴ文字列.TabIndex = 7
        lblロゴ文字列.Text = "ロゴ文字列"
        ' 
        ' lblロゴ画像
        ' 
        lblロゴ画像.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        lblロゴ画像.AutoSize = True
        lblロゴ画像.Location = New System.Drawing.Point(14, 285)
        lblロゴ画像.Name = "lblロゴ画像"
        lblロゴ画像.Size = New System.Drawing.Size(57, 19)
        lblロゴ画像.TabIndex = 6
        lblロゴ画像.Text = "ロゴ画像"
        ' 
        ' txtロゴ文字列
        ' 
        txtロゴ文字列.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txtロゴ文字列.Location = New System.Drawing.Point(243, 285)
        txtロゴ文字列.Multiline = True
        txtロゴ文字列.Name = "txtロゴ文字列"
        txtロゴ文字列.Size = New System.Drawing.Size(454, 44)
        txtロゴ文字列.TabIndex = 8
        ToolTip1.SetToolTip(txtロゴ文字列, "プレビュー画像・ひもリストの下に入れる文字列があれば入力")
        ' 
        ' picロゴ画像
        ' 
        picロゴ画像.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        picロゴ画像.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
        picロゴ画像.Location = New System.Drawing.Point(83, 282)
        picロゴ画像.Name = "picロゴ画像"
        picロゴ画像.Size = New System.Drawing.Size(77, 49)
        picロゴ画像.SizeMode = Windows.Forms.PictureBoxSizeMode.Zoom
        picロゴ画像.TabIndex = 12
        picロゴ画像.TabStop = False
        ToolTip1.SetToolTip(picロゴ画像, "プレビュー画像の下に入れる画像があればクリックして指定する" & vbCrLf & "キャンセルでクリアできます")
        ' 
        ' txt作成者
        ' 
        txt作成者.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txt作成者.Location = New System.Drawing.Point(83, 70)
        txt作成者.Multiline = True
        txt作成者.Name = "txt作成者"
        txt作成者.Size = New System.Drawing.Size(614, 47)
        txt作成者.TabIndex = 3
        ToolTip1.SetToolTip(txt作成者, "作成者情報")
        ' 
        ' txtタイトル
        ' 
        txtタイトル.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txtタイトル.Location = New System.Drawing.Point(83, 12)
        txtタイトル.Multiline = True
        txtタイトル.Name = "txtタイトル"
        txtタイトル.Size = New System.Drawing.Size(616, 47)
        txtタイトル.TabIndex = 1
        ToolTip1.SetToolTip(txtタイトル, "タイトル情報")
        ' 
        ' lbl作成者
        ' 
        lbl作成者.AutoSize = True
        lbl作成者.Location = New System.Drawing.Point(14, 70)
        lbl作成者.Name = "lbl作成者"
        lbl作成者.Size = New System.Drawing.Size(51, 19)
        lbl作成者.TabIndex = 2
        lbl作成者.Text = "作成者"
        ' 
        ' lblタイトル
        ' 
        lblタイトル.AutoSize = True
        lblタイトル.Location = New System.Drawing.Point(14, 12)
        lblタイトル.Name = "lblタイトル"
        lblタイトル.Size = New System.Drawing.Size(52, 19)
        lblタイトル.TabIndex = 0
        lblタイトル.Text = "タイトル"
        ' 
        ' lblメモ
        ' 
        lblメモ.AutoSize = True
        lblメモ.Location = New System.Drawing.Point(14, 128)
        lblメモ.Name = "lblメモ"
        lblメモ.Size = New System.Drawing.Size(29, 19)
        lblメモ.TabIndex = 4
        lblメモ.Text = "メモ"
        ' 
        ' txtメモ
        ' 
        txtメモ.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txtメモ.Location = New System.Drawing.Point(83, 128)
        txtメモ.Multiline = True
        txtメモ.Name = "txtメモ"
        txtメモ.Size = New System.Drawing.Size(614, 148)
        txtメモ.TabIndex = 5
        ToolTip1.SetToolTip(txtメモ, "自由に記述できます")
        ' 
        ' OpenFileDialogPng
        ' 
        OpenFileDialogPng.Filter = "画像ファイル (*.png;*.gif;*.jpg;*.jpeg)|*.png;*.gif;*.jpg;*.jpeg|PNGファイル (*.png)|*.png|GIFファイル (*.gif)|*.gif|JPEGファイル (*.jpg;*.jpeg)|*.jpg;*.jpeg"
        OpenFileDialogPng.Title = "テクスチャ画像ファイルを指定してください"
        ' 
        ' ctrMemo
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        Controls.Add(Panel)
        Name = "ctrMemo"
        Size = New System.Drawing.Size(892, 438)
        Panel.ResumeLayout(False)
        Panel.PerformLayout()
        CType(picロゴ画像, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel As Windows.Forms.Panel
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents txt作成者 As Windows.Forms.TextBox
    Friend WithEvents txtタイトル As Windows.Forms.TextBox
    Friend WithEvents lbl作成者 As Windows.Forms.Label
    Friend WithEvents lblタイトル As Windows.Forms.Label
    Friend WithEvents lblメモ As Windows.Forms.Label
    Friend WithEvents txtメモ As Windows.Forms.TextBox
    Friend WithEvents txtロゴ文字列 As Windows.Forms.TextBox
    Friend WithEvents picロゴ画像 As Windows.Forms.PictureBox
    Friend WithEvents lblロゴ画像 As Windows.Forms.Label
    Friend WithEvents lblロゴ文字列 As Windows.Forms.Label
    Friend WithEvents OpenFileDialogPng As Windows.Forms.OpenFileDialog

End Class
