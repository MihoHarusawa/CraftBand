<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVirsion
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVirsion))
        btn終了 = New Button()
        lblExeName = New Label()
        lblDescription = New Label()
        txtDescription = New TextBox()
        lblVersion = New Label()
        PictureBox1 = New PictureBox()
        lnkLabo = New LinkLabel()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btn終了
        ' 
        btn終了.DialogResult = DialogResult.OK
        btn終了.Location = New Point(435, 187)
        btn終了.Name = "btn終了"
        btn終了.Size = New Size(111, 46)
        btn終了.TabIndex = 26
        btn終了.Text = "OK(&O)"
        btn終了.UseVisualStyleBackColor = True
        ' 
        ' lblExeName
        ' 
        lblExeName.AutoSize = True
        lblExeName.Location = New Point(116, 20)
        lblExeName.Name = "lblExeName"
        lblExeName.Size = New Size(110, 20)
        lblExeName.TabIndex = 27
        lblExeName.Text = "CraftBandMesh"
        ' 
        ' lblDescription
        ' 
        lblDescription.AutoSize = True
        lblDescription.Location = New Point(116, 49)
        lblDescription.Name = "lblDescription"
        lblDescription.Size = New Size(173, 20)
        lblDescription.TabIndex = 28
        lblDescription.Text = "四角・楕円底かごのデザイン"
        ' 
        ' txtDescription
        ' 
        txtDescription.Location = New Point(116, 81)
        txtDescription.Multiline = True
        txtDescription.Name = "txtDescription"
        txtDescription.ReadOnly = True
        txtDescription.Size = New Size(260, 78)
        txtDescription.TabIndex = 30
        txtDescription.Text = "四角底・楕円底から垂直に立ち上げて、" & vbCrLf & "側面を下から上に編んでいき、" & vbCrLf & "縁の始末をします。"
        ' 
        ' lblVersion
        ' 
        lblVersion.AutoSize = True
        lblVersion.Location = New Point(116, 174)
        lblVersion.Name = "lblVersion"
        lblVersion.Size = New Size(56, 20)
        lblVersion.TabIndex = 31
        lblVersion.Text = "version"
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), Image)
        PictureBox1.Location = New Point(12, 12)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(98, 98)
        PictureBox1.TabIndex = 32
        PictureBox1.TabStop = False
        ' 
        ' lnkLabo
        ' 
        lnkLabo.AutoSize = True
        lnkLabo.Location = New Point(116, 204)
        lnkLabo.Name = "lnkLabo"
        lnkLabo.Size = New Size(302, 20)
        lnkLabo.TabIndex = 33
        lnkLabo.TabStop = True
        lnkLabo.Text = "https://labo.com/CraftBand/craftbandmesh/"
        ' 
        ' frmVirsion
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(558, 242)
        Controls.Add(lnkLabo)
        Controls.Add(PictureBox1)
        Controls.Add(lblVersion)
        Controls.Add(txtDescription)
        Controls.Add(lblDescription)
        Controls.Add(lblExeName)
        Controls.Add(btn終了)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmVirsion"
        StartPosition = FormStartPosition.CenterParent
        Text = "バージョン情報"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents btn終了 As Button
    Friend WithEvents lblExeName As Label
    Friend WithEvents lblDescription As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents lblVersion As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lnkLabo As LinkLabel
End Class
