<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        btnMesh = New Button()
        btnKnot = New Button()
        btnSquare45 = New Button()
        btnSquare = New Button()
        btnHexagon = New Button()
        ToolTip1 = New ToolTip(components)
        lblMesh = New Label()
        lblKnot = New Label()
        lblSquare45 = New Label()
        lblSquare = New Label()
        lblHexagon = New Label()
        radすぐに開く = New RadioButton()
        rad情報を表示する = New RadioButton()
        rad旧拡張子変更 = New RadioButton()
        FolderBrowserDialog1 = New FolderBrowserDialog()
        MenuStrip1 = New MenuStrip()
        ToolStripMenuItemFile = New ToolStripMenuItem()
        ToolStripMenuItemFileOpen = New ToolStripMenuItem()
        ToolStripMenuItemクリア = New ToolStripMenuItem()
        ToolStripMenuItemFileExit = New ToolStripMenuItem()
        ToolStripMenuItemSetting = New ToolStripMenuItem()
        ToolStripMenuItemHelp = New ToolStripMenuItem()
        rtxtMessage = New RichTextBox()
        MenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' btnMesh
        ' 
        btnMesh.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnMesh.Image = CType(resources.GetObject("btnMesh.Image"), Image)
        btnMesh.Location = New Point(21, 98)
        btnMesh.Name = "btnMesh"
        btnMesh.Size = New Size(73, 64)
        btnMesh.TabIndex = 5
        ToolTip1.SetToolTip(btnMesh, "縦横と楕円の底・丸底や輪弧底、プラス側面を編む方式")
        btnMesh.UseVisualStyleBackColor = True
        ' 
        ' btnKnot
        ' 
        btnKnot.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnKnot.Image = CType(resources.GetObject("btnKnot.Image"), Image)
        btnKnot.Location = New Point(201, 98)
        btnKnot.Name = "btnKnot"
        btnKnot.Size = New Size(73, 64)
        btnKnot.TabIndex = 7
        ToolTip1.SetToolTip(btnKnot, "四つ畳み編み(石畳編み/ノット編み/2本結び)")
        btnKnot.UseVisualStyleBackColor = True
        ' 
        ' btnSquare45
        ' 
        btnSquare45.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnSquare45.Image = CType(resources.GetObject("btnSquare45.Image"), Image)
        btnSquare45.Location = New Point(111, 98)
        btnSquare45.Name = "btnSquare45"
        btnSquare45.Size = New Size(73, 64)
        btnSquare45.TabIndex = 6
        ToolTip1.SetToolTip(btnSquare45, "北欧編みや斜め網代など、縦横に組んだ底を、斜め45度で立ち上げる方式")
        btnSquare45.UseVisualStyleBackColor = True
        ' 
        ' btnSquare
        ' 
        btnSquare.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnSquare.Image = CType(resources.GetObject("btnSquare.Image"), Image)
        btnSquare.Location = New Point(291, 98)
        btnSquare.Name = "btnSquare"
        btnSquare.Size = New Size(73, 64)
        btnSquare.TabIndex = 8
        ToolTip1.SetToolTip(btnSquare, "縦横に組んだ長方形の底をそのまま立ち上げる方式")
        btnSquare.UseVisualStyleBackColor = True
        ' 
        ' btnHexagon
        ' 
        btnHexagon.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnHexagon.Image = CType(resources.GetObject("btnHexagon.Image"), Image)
        btnHexagon.Location = New Point(381, 98)
        btnHexagon.Name = "btnHexagon"
        btnHexagon.Size = New Size(73, 64)
        btnHexagon.TabIndex = 9
        ToolTip1.SetToolTip(btnHexagon, "六つ目、つまり60度ごと3方向にひもを組み、ひもに沿って立ち上げるタイプ")
        btnHexagon.UseVisualStyleBackColor = True
        ' 
        ' lblMesh
        ' 
        lblMesh.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblMesh.AutoSize = True
        lblMesh.Location = New Point(36, 166)
        lblMesh.Name = "lblMesh"
        lblMesh.Size = New Size(43, 19)
        lblMesh.TabIndex = 10
        lblMesh.Text = "Mesh"
        ToolTip1.SetToolTip(lblMesh, "縦横と楕円の底・丸底や輪弧底、プラス側面を編む方式")
        ' 
        ' lblKnot
        ' 
        lblKnot.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblKnot.AutoSize = True
        lblKnot.Location = New Point(220, 166)
        lblKnot.Name = "lblKnot"
        lblKnot.Size = New Size(38, 19)
        lblKnot.TabIndex = 12
        lblKnot.Text = "Knot"
        ToolTip1.SetToolTip(lblKnot, "四つ畳み編み(石畳編み/ノット編み/2本結び)")
        ' 
        ' lblSquare45
        ' 
        lblSquare45.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblSquare45.AutoSize = True
        lblSquare45.Location = New Point(116, 166)
        lblSquare45.Name = "lblSquare45"
        lblSquare45.Size = New Size(67, 19)
        lblSquare45.TabIndex = 11
        lblSquare45.Text = "Square45"
        ToolTip1.SetToolTip(lblSquare45, "北欧編みや斜め網代など、縦横に組んだ底を、斜め45度で立ち上げる方式")
        ' 
        ' lblSquare
        ' 
        lblSquare.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblSquare.AutoSize = True
        lblSquare.Location = New Point(304, 166)
        lblSquare.Name = "lblSquare"
        lblSquare.Size = New Size(51, 19)
        lblSquare.TabIndex = 13
        lblSquare.Text = "Square"
        ToolTip1.SetToolTip(lblSquare, "縦横に組んだ長方形の底をそのまま立ち上げる方式")
        ' 
        ' lblHexagon
        ' 
        lblHexagon.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblHexagon.AutoSize = True
        lblHexagon.Location = New Point(384, 166)
        lblHexagon.Name = "lblHexagon"
        lblHexagon.Size = New Size(63, 19)
        lblHexagon.TabIndex = 14
        lblHexagon.Text = "Hexagon"
        ToolTip1.SetToolTip(lblHexagon, "六つ目、つまり60度ごと3方向にひもを組み、ひもに沿って立ち上げるタイプ")
        ' 
        ' radすぐに開く
        ' 
        radすぐに開く.AutoSize = True
        radすぐに開く.Checked = True
        radすぐに開く.Location = New Point(23, 30)
        radすぐに開く.Name = "radすぐに開く"
        radすぐに開く.Size = New Size(95, 23)
        radすぐに開く.TabIndex = 1
        radすぐに開く.TabStop = True
        radすぐに開く.Text = "すぐに開く(&S)"
        ToolTip1.SetToolTip(radすぐに開く, "ファイルを指定したら、すぐに対応するアプリを起動して開きます")
        radすぐに開く.UseVisualStyleBackColor = True
        ' 
        ' rad情報を表示する
        ' 
        rad情報を表示する.AutoSize = True
        rad情報を表示する.Location = New Point(133, 30)
        rad情報を表示する.Name = "rad情報を表示する"
        rad情報を表示する.Size = New Size(128, 23)
        rad情報を表示する.TabIndex = 2
        rad情報を表示する.Text = "情報を表示する(&I)"
        ToolTip1.SetToolTip(rad情報を表示する, "シリーズのファイルについて、それが何かを表示します")
        rad情報を表示する.UseVisualStyleBackColor = True
        ' 
        ' rad旧拡張子変更
        ' 
        rad旧拡張子変更.AutoSize = True
        rad旧拡張子変更.Location = New Point(277, 30)
        rad旧拡張子変更.Name = "rad旧拡張子変更"
        rad旧拡張子変更.Size = New Size(126, 23)
        rad旧拡張子変更.TabIndex = 3
        rad旧拡張子変更.Text = "旧拡張子変更(&E)"
        ToolTip1.SetToolTip(rad旧拡張子変更, "旧拡張子(.xml)を新拡張子(.cbmesh)に変更します")
        rad旧拡張子変更.UseVisualStyleBackColor = True
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {ToolStripMenuItemFile, ToolStripMenuItemSetting, ToolStripMenuItemHelp})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(474, 27)
        MenuStrip1.TabIndex = 0
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' ToolStripMenuItemFile
        ' 
        ToolStripMenuItemFile.DropDownItems.AddRange(New ToolStripItem() {ToolStripMenuItemFileOpen, ToolStripMenuItemクリア, ToolStripMenuItemFileExit})
        ToolStripMenuItemFile.Name = "ToolStripMenuItemFile"
        ToolStripMenuItemFile.Size = New Size(77, 23)
        ToolStripMenuItemFile.Text = "ファイル(&F)"
        ' 
        ' ToolStripMenuItemFileOpen
        ' 
        ToolStripMenuItemFileOpen.Name = "ToolStripMenuItemFileOpen"
        ToolStripMenuItemFileOpen.Size = New Size(127, 24)
        ToolStripMenuItemFileOpen.Text = "開く(&O)"
        ' 
        ' ToolStripMenuItemクリア
        ' 
        ToolStripMenuItemクリア.Name = "ToolStripMenuItemクリア"
        ToolStripMenuItemクリア.Size = New Size(127, 24)
        ToolStripMenuItemクリア.Text = "クリア(&C)"
        ' 
        ' ToolStripMenuItemFileExit
        ' 
        ToolStripMenuItemFileExit.Name = "ToolStripMenuItemFileExit"
        ToolStripMenuItemFileExit.Size = New Size(127, 24)
        ToolStripMenuItemFileExit.Text = "終了(&X)"
        ' 
        ' ToolStripMenuItemSetting
        ' 
        ToolStripMenuItemSetting.Name = "ToolStripMenuItemSetting"
        ToolStripMenuItemSetting.Size = New Size(64, 23)
        ToolStripMenuItemSetting.Text = "設定(&S)"
        ' 
        ' ToolStripMenuItemHelp
        ' 
        ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp"
        ToolStripMenuItemHelp.Size = New Size(73, 23)
        ToolStripMenuItemHelp.Text = "ヘルプ(&H)"
        ' 
        ' rtxtMessage
        ' 
        rtxtMessage.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        rtxtMessage.BackColor = Color.White
        rtxtMessage.BorderStyle = BorderStyle.FixedSingle
        rtxtMessage.Location = New Point(23, 64)
        rtxtMessage.Name = "rtxtMessage"
        rtxtMessage.ReadOnly = True
        rtxtMessage.Size = New Size(431, 28)
        rtxtMessage.TabIndex = 4
        rtxtMessage.Text = ""
        ' 
        ' frmMain
        ' 
        AllowDrop = True
        AutoScaleDimensions = New SizeF(8F, 19F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(474, 187)
        Controls.Add(rtxtMessage)
        Controls.Add(lblHexagon)
        Controls.Add(lblSquare)
        Controls.Add(lblSquare45)
        Controls.Add(lblKnot)
        Controls.Add(lblMesh)
        Controls.Add(rad旧拡張子変更)
        Controls.Add(rad情報を表示する)
        Controls.Add(radすぐに開く)
        Controls.Add(btnHexagon)
        Controls.Add(btnSquare)
        Controls.Add(btnSquare45)
        Controls.Add(btnKnot)
        Controls.Add(btnMesh)
        Controls.Add(MenuStrip1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = MenuStrip1
        MinimumSize = New Size(490, 226)
        Name = "frmMain"
        Text = "CraftBandMesh シリーズ"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnMesh As Button
    Friend WithEvents btnKnot As Button
    Friend WithEvents btnSquare45 As Button
    Friend WithEvents btnSquare As Button
    Friend WithEvents btnHexagon As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents radすぐに開く As RadioButton
    Friend WithEvents rad情報を表示する As RadioButton
    Friend WithEvents rad旧拡張子変更 As RadioButton
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ToolStripMenuItemFile As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFileOpen As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFileExit As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSetting As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemHelp As ToolStripMenuItem
    Friend WithEvents lblMesh As Label
    Friend WithEvents lblKnot As Label
    Friend WithEvents lblSquare45 As Label
    Friend WithEvents lblSquare As Label
    Friend WithEvents lblHexagon As Label
    Friend WithEvents ToolStripMenuItemクリア As ToolStripMenuItem
    Friend WithEvents rtxtMessage As RichTextBox

End Class
