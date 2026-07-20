<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrPreview
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
        panelPreview = New Windows.Forms.Panel()
        picPreview = New Windows.Forms.PictureBox()
        trkbarPreviewImage = New Windows.Forms.TrackBar()
        grp3D = New Windows.Forms.GroupBox()
        radファイル = New Windows.Forms.RadioButton()
        radビューア = New Windows.Forms.RadioButton()
        btn3Dモデル = New Windows.Forms.Button()
        btn画像ファイル = New Windows.Forms.Button()
        btnブラウザ = New Windows.Forms.Button()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        Panel.SuspendLayout()
        panelPreview.SuspendLayout()
        CType(picPreview, ComponentModel.ISupportInitialize).BeginInit()
        CType(trkbarPreviewImage, ComponentModel.ISupportInitialize).BeginInit()
        grp3D.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel
        ' 
        Panel.Controls.Add(panelPreview)
        Panel.Controls.Add(grp3D)
        Panel.Controls.Add(btn3Dモデル)
        Panel.Controls.Add(btn画像ファイル)
        Panel.Controls.Add(btnブラウザ)
        Panel.Controls.Add(trkbarPreviewImage)
        Panel.Location = New System.Drawing.Point(0, 0)
        Panel.Name = "Panel"
        Panel.Size = New System.Drawing.Size(713, 339)
        Panel.TabIndex = 1
        ' 
        ' panelPreview
        ' 
        panelPreview.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        panelPreview.AutoScroll = True
        panelPreview.Controls.Add(picPreview)
        panelPreview.Location = New System.Drawing.Point(5, 5)
        panelPreview.Name = "panelPreview"
        panelPreview.Size = New System.Drawing.Size(703, 280)
        panelPreview.TabIndex = 0
        ' 
        ' picPreview
        ' 
        picPreview.Location = New System.Drawing.Point(27, 16)
        picPreview.Name = "picPreview"
        picPreview.Size = New System.Drawing.Size(351, 209)
        picPreview.SizeMode = Windows.Forms.PictureBoxSizeMode.StretchImage
        picPreview.TabIndex = 0
        picPreview.TabStop = False
        ' 
        ' trkbarPreviewImage
        ' 
        trkbarPreviewImage.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        trkbarPreviewImage.Location = New System.Drawing.Point(4, 291)
        trkbarPreviewImage.Maximum = 1000
        trkbarPreviewImage.Name = "trkbarPreviewImage"
        trkbarPreviewImage.Size = New System.Drawing.Size(165, 45)
        trkbarPreviewImage.TabIndex = 1
        trkbarPreviewImage.TickStyle = Windows.Forms.TickStyle.None
        ToolTip1.SetToolTip(trkbarPreviewImage, "画像を拡大します")
        trkbarPreviewImage.Value = 100
        ' 
        ' grp3D
        ' 
        grp3D.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        grp3D.Controls.Add(radファイル)
        grp3D.Controls.Add(radビューア)
        grp3D.Location = New System.Drawing.Point(175, 283)
        grp3D.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        grp3D.Name = "grp3D"
        grp3D.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        grp3D.Size = New System.Drawing.Size(178, 48)
        grp3D.TabIndex = 2
        grp3D.TabStop = False
        grp3D.Text = "3D"
        ToolTip1.SetToolTip(grp3D, "3Dモデルをビューアで表示するかファイルにするか選びます")
        grp3D.Visible = False
        ' 
        ' radファイル
        ' 
        radファイル.AutoSize = True
        radファイル.Location = New System.Drawing.Point(102, 19)
        radファイル.Name = "radファイル"
        radファイル.Size = New System.Drawing.Size(68, 23)
        radファイル.TabIndex = 1
        radファイル.Text = "ファイル"
        ToolTip1.SetToolTip(radファイル, "3Dファイルを圧縮ファイルにします")
        radファイル.UseVisualStyleBackColor = True
        ' 
        ' radビューア
        ' 
        radビューア.AutoSize = True
        radビューア.Checked = True
        radビューア.Location = New System.Drawing.Point(29, 19)
        radビューア.Name = "radビューア"
        radビューア.Size = New System.Drawing.Size(65, 23)
        radビューア.TabIndex = 0
        radビューア.TabStop = True
        radビューア.Text = "ビューア"
        ToolTip1.SetToolTip(radビューア, "3Dファイルを標準ビューアで開きます")
        radビューア.UseVisualStyleBackColor = True
        ' 
        ' btn3Dモデル
        ' 
        btn3Dモデル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn3Dモデル.Location = New System.Drawing.Point(359, 289)
        btn3Dモデル.Name = "btn3Dモデル"
        btn3Dモデル.Size = New System.Drawing.Size(111, 43)
        btn3Dモデル.TabIndex = 3
        btn3Dモデル.Text = "3Dモデル(&M)"
        ToolTip1.SetToolTip(btn3Dモデル, "生成した画像ファイルを開きます")
        btn3Dモデル.UseVisualStyleBackColor = True
        btn3Dモデル.Visible = False
        ' 
        ' btn画像ファイル
        ' 
        btn画像ファイル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn画像ファイル.Location = New System.Drawing.Point(476, 289)
        btn画像ファイル.Name = "btn画像ファイル"
        btn画像ファイル.Size = New System.Drawing.Size(111, 43)
        btn画像ファイル.TabIndex = 4
        btn画像ファイル.Text = "画像ファイル(&I)"
        ToolTip1.SetToolTip(btn画像ファイル, "生成した画像ファイルを開きます")
        btn画像ファイル.UseVisualStyleBackColor = True
        ' 
        ' btnブラウザ
        ' 
        btnブラウザ.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnブラウザ.Location = New System.Drawing.Point(593, 289)
        btnブラウザ.Name = "btnブラウザ"
        btnブラウザ.Size = New System.Drawing.Size(111, 43)
        btnブラウザ.TabIndex = 5
        btnブラウザ.Text = "ブラウザ(&B)"
        ToolTip1.SetToolTip(btnブラウザ, "生成した画像ファイルをブラウザで開きます")
        btnブラウザ.UseVisualStyleBackColor = True
        ' 
        ' ctrPreview
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        Controls.Add(Panel)
        Name = "ctrPreview"
        Size = New System.Drawing.Size(861, 451)
        Panel.ResumeLayout(False)
        Panel.PerformLayout()
        panelPreview.ResumeLayout(False)
        CType(picPreview, ComponentModel.ISupportInitialize).EndInit()
        CType(trkbarPreviewImage, ComponentModel.ISupportInitialize).EndInit()
        grp3D.ResumeLayout(False)
        grp3D.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel As Windows.Forms.Panel
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents grp3D As Windows.Forms.GroupBox
    Friend WithEvents radファイル As Windows.Forms.RadioButton
    Friend WithEvents radビューア As Windows.Forms.RadioButton
    Friend WithEvents btn3Dモデル As Windows.Forms.Button
    Friend WithEvents btn画像ファイル As Windows.Forms.Button
    Friend WithEvents btnブラウザ As Windows.Forms.Button
    Friend WithEvents trkbarPreviewImage As Windows.Forms.TrackBar
    Friend WithEvents panelPreview As Windows.Forms.Panel
    Friend WithEvents picPreview As Windows.Forms.PictureBox

End Class
