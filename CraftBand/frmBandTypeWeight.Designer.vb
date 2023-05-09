<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBandTypeWeight
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
        Me.components = New System.ComponentModel.Container()
        Me.txt長さと重さ = New System.Windows.Forms.TextBox()
        Me.lbl長さと重さ = New System.Windows.Forms.Label()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btn重さ長さ = New System.Windows.Forms.Button()
        Me.btn長さ重さ = New System.Windows.Forms.Button()
        Me.lbl重さ単位 = New System.Windows.Forms.Label()
        Me.lbl長さ単位 = New System.Windows.Forms.Label()
        Me.txt重さ = New System.Windows.Forms.TextBox()
        Me.txt長さ = New System.Windows.Forms.TextBox()
        Me.lbl重さ = New System.Windows.Forms.Label()
        Me.lbl長さ = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnクリア = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txt長さと重さ
        '
        Me.txt長さと重さ.Location = New System.Drawing.Point(153, 8)
        Me.txt長さと重さ.Name = "txt長さと重さ"
        Me.txt長さと重さ.Size = New System.Drawing.Size(244, 27)
        Me.txt長さと重さ.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.txt長さと重さ, "<数値> <長さの単位> = <数値> <重さの単位> のように入力")
        '
        'lbl長さと重さ
        '
        Me.lbl長さと重さ.AutoSize = True
        Me.lbl長さと重さ.Location = New System.Drawing.Point(49, 11)
        Me.lbl長さと重さ.Name = "lbl長さと重さ"
        Me.lbl長さと重さ.Size = New System.Drawing.Size(70, 20)
        Me.lbl長さと重さ.TabIndex = 0
        Me.lbl長さと重さ.Text = "長さと重さ"
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(420, 173)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 4
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.ToolTip1.SetToolTip(Me.btnキャンセル, "変更を保存せずに終了します")
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(301, 173)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK(&O)"
        Me.ToolTip1.SetToolTip(Me.btnOK, "「長さと重さ」値の変更を保存して終了します")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btn重さ長さ)
        Me.GroupBox1.Controls.Add(Me.btn長さ重さ)
        Me.GroupBox1.Controls.Add(Me.lbl重さ単位)
        Me.GroupBox1.Controls.Add(Me.lbl長さ単位)
        Me.GroupBox1.Controls.Add(Me.txt重さ)
        Me.GroupBox1.Controls.Add(Me.txt長さ)
        Me.GroupBox1.Controls.Add(Me.lbl重さ)
        Me.GroupBox1.Controls.Add(Me.lbl長さ)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 41)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(519, 128)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'btn重さ長さ
        '
        Me.btn重さ長さ.Font = New System.Drawing.Font("Yu Gothic UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btn重さ長さ.Location = New System.Drawing.Point(224, 70)
        Me.btn重さ長さ.Name = "btn重さ長さ"
        Me.btn重さ長さ.Size = New System.Drawing.Size(68, 40)
        Me.btn重さ長さ.TabIndex = 7
        Me.btn重さ長さ.Text = "←"
        Me.ToolTip1.SetToolTip(Me.btn重さ長さ, "重さから長さを計算します")
        Me.btn重さ長さ.UseVisualStyleBackColor = True
        '
        'btn長さ重さ
        '
        Me.btn長さ重さ.Font = New System.Drawing.Font("Yu Gothic UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btn長さ重さ.Location = New System.Drawing.Point(224, 24)
        Me.btn長さ重さ.Name = "btn長さ重さ"
        Me.btn長さ重さ.Size = New System.Drawing.Size(68, 40)
        Me.btn長さ重さ.TabIndex = 6
        Me.btn長さ重さ.Text = "→"
        Me.ToolTip1.SetToolTip(Me.btn長さ重さ, "長さから重さを計算します")
        Me.btn長さ重さ.UseVisualStyleBackColor = True
        '
        'lbl重さ単位
        '
        Me.lbl重さ単位.AutoSize = True
        Me.lbl重さ単位.Location = New System.Drawing.Point(413, 95)
        Me.lbl重さ単位.Name = "lbl重さ単位"
        Me.lbl重さ単位.Size = New System.Drawing.Size(64, 20)
        Me.lbl重さ単位.TabIndex = 5
        Me.lbl重さ単位.Text = "重さ単位"
        '
        'lbl長さ単位
        '
        Me.lbl長さ単位.AutoSize = True
        Me.lbl長さ単位.Location = New System.Drawing.Point(107, 95)
        Me.lbl長さ単位.Name = "lbl長さ単位"
        Me.lbl長さ単位.Size = New System.Drawing.Size(64, 20)
        Me.lbl長さ単位.TabIndex = 2
        Me.lbl長さ単位.Text = "長さ単位"
        '
        'txt重さ
        '
        Me.txt重さ.Location = New System.Drawing.Point(343, 58)
        Me.txt重さ.Name = "txt重さ"
        Me.txt重さ.Size = New System.Drawing.Size(134, 27)
        Me.txt重さ.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.txt重さ, "重さの数値を入力してください")
        '
        'txt長さ
        '
        Me.txt長さ.Location = New System.Drawing.Point(37, 58)
        Me.txt長さ.Name = "txt長さ"
        Me.txt長さ.Size = New System.Drawing.Size(134, 27)
        Me.txt長さ.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.txt長さ, "長さの数値を入力してください")
        '
        'lbl重さ
        '
        Me.lbl重さ.AutoSize = True
        Me.lbl重さ.Location = New System.Drawing.Point(343, 24)
        Me.lbl重さ.Name = "lbl重さ"
        Me.lbl重さ.Size = New System.Drawing.Size(34, 20)
        Me.lbl重さ.TabIndex = 3
        Me.lbl重さ.Text = "重さ"
        '
        'lbl長さ
        '
        Me.lbl長さ.AutoSize = True
        Me.lbl長さ.Location = New System.Drawing.Point(34, 24)
        Me.lbl長さ.Name = "lbl長さ"
        Me.lbl長さ.Size = New System.Drawing.Size(34, 20)
        Me.lbl長さ.TabIndex = 0
        Me.lbl長さ.Text = "長さ"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 227)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(543, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 16)
        '
        'btnクリア
        '
        Me.btnクリア.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnクリア.Location = New System.Drawing.Point(12, 173)
        Me.btnクリア.Name = "btnクリア"
        Me.btnクリア.Size = New System.Drawing.Size(111, 46)
        Me.btnクリア.TabIndex = 6
        Me.btnクリア.Text = "クリア(&C)"
        Me.ToolTip1.SetToolTip(Me.btnクリア, "長さ、重さをクリアします")
        Me.btnクリア.UseVisualStyleBackColor = True
        '
        'frmBandTypeWeight
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(543, 249)
        Me.Controls.Add(Me.btnクリア)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lbl長さと重さ)
        Me.Controls.Add(Me.txt長さと重さ)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBandTypeWeight"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "長さと重さの換算"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txt長さと重さ As Windows.Forms.TextBox
    Friend WithEvents lbl長さと重さ As Windows.Forms.Label
    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents btn重さ長さ As Windows.Forms.Button
    Friend WithEvents btn長さ重さ As Windows.Forms.Button
    Friend WithEvents lbl重さ単位 As Windows.Forms.Label
    Friend WithEvents lbl長さ単位 As Windows.Forms.Label
    Friend WithEvents txt重さ As Windows.Forms.TextBox
    Friend WithEvents txt長さ As Windows.Forms.TextBox
    Friend WithEvents lbl重さ As Windows.Forms.Label
    Friend WithEvents lbl長さ As Windows.Forms.Label
    Friend WithEvents StatusStrip1 As Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnクリア As Windows.Forms.Button
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
End Class
