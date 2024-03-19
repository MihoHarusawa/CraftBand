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
        Me.btn終了 = New System.Windows.Forms.Button()
        Me.lblExeName = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lnkLabo = New System.Windows.Forms.LinkLabel()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn終了
        '
        Me.btn終了.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btn終了.Location = New System.Drawing.Point(435, 187)
        Me.btn終了.Name = "btn終了"
        Me.btn終了.Size = New System.Drawing.Size(111, 46)
        Me.btn終了.TabIndex = 26
        Me.btn終了.Text = "OK(&O)"
        Me.btn終了.UseVisualStyleBackColor = True
        '
        'lblExeName
        '
        Me.lblExeName.AutoSize = True
        Me.lblExeName.Location = New System.Drawing.Point(116, 20)
        Me.lblExeName.Name = "lblExeName"
        Me.lblExeName.Size = New System.Drawing.Size(135, 20)
        Me.lblExeName.TabIndex = 27
        Me.lblExeName.Text = "CraftBandHexagon"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(116, 49)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(224, 20)
        Me.lblDescription.TabIndex = 28
        Me.lblDescription.Text = "クラフトバンド / 紙バンドのサイズ計算"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(116, 81)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(260, 78)
        Me.txtDescription.TabIndex = 30
        Me.txtDescription.Text = "六つ目" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "60度ごと3方向にバンドを組むと" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "中に六角形の「六つ目」ができます。"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(116, 174)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(56, 20)
        Me.lblVersion.TabIndex = 31
        Me.lblVersion.Text = "version"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(98, 98)
        Me.PictureBox1.TabIndex = 32
        Me.PictureBox1.TabStop = False
        '
        'lnkLabo
        '
        Me.lnkLabo.AutoSize = True
        Me.lnkLabo.Location = New System.Drawing.Point(116, 204)
        Me.lnkLabo.Name = "lnkLabo"
        Me.lnkLabo.Size = New System.Drawing.Size(324, 20)
        Me.lnkLabo.TabIndex = 33
        Me.lnkLabo.TabStop = True
        Me.lnkLabo.Text = "https://labo.com/CraftBand/craftbandhexagon/"
        '
        'frmVirsion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 242)
        Me.Controls.Add(Me.lnkLabo)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lblExeName)
        Me.Controls.Add(Me.btn終了)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVirsion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "バージョン情報"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btn終了 As Button
    Friend WithEvents lblExeName As Label
    Friend WithEvents lblDescription As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents lblVersion As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lnkLabo As LinkLabel
End Class
