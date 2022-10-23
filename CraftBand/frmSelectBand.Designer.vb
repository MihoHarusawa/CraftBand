<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectBand
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
        Me.lblバンド幅の寸法単位 = New System.Windows.Forms.Label()
        Me.txt本幅 = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.nud小数点以下桁数 = New System.Windows.Forms.NumericUpDown()
        Me.lbl小数点以下桁数 = New System.Windows.Forms.Label()
        Me.rad出力時の寸法単位_in = New System.Windows.Forms.RadioButton()
        Me.rad出力時の寸法単位_cm = New System.Windows.Forms.RadioButton()
        Me.rad出力時の寸法単位_mm = New System.Windows.Forms.RadioButton()
        Me.cmb対象バンドの種類名 = New System.Windows.Forms.ComboBox()
        Me.txtバンド幅 = New System.Windows.Forms.TextBox()
        Me.lblバンド幅 = New System.Windows.Forms.Label()
        Me.lbl本幅 = New System.Windows.Forms.Label()
        Me.lbl対象バンドの種類名 = New System.Windows.Forms.Label()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lblリスト出力記号 = New System.Windows.Forms.Label()
        Me.txtリスト出力記号 = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nud小数点以下桁数, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblバンド幅の寸法単位
        '
        Me.lblバンド幅の寸法単位.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblバンド幅の寸法単位.AutoSize = True
        Me.lblバンド幅の寸法単位.Location = New System.Drawing.Point(593, 20)
        Me.lblバンド幅の寸法単位.Name = "lblバンド幅の寸法単位"
        Me.lblバンド幅の寸法単位.Size = New System.Drawing.Size(35, 20)
        Me.lblバンド幅の寸法単位.TabIndex = 6
        Me.lblバンド幅の寸法単位.Text = "mm"
        '
        'txt本幅
        '
        Me.txt本幅.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt本幅.Location = New System.Drawing.Point(354, 13)
        Me.txt本幅.Name = "txt本幅"
        Me.txt本幅.ReadOnly = True
        Me.txt本幅.Size = New System.Drawing.Size(50, 27)
        Me.txt本幅.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.nud小数点以下桁数)
        Me.GroupBox1.Controls.Add(Me.lbl小数点以下桁数)
        Me.GroupBox1.Controls.Add(Me.rad出力時の寸法単位_in)
        Me.GroupBox1.Controls.Add(Me.rad出力時の寸法単位_cm)
        Me.GroupBox1.Controls.Add(Me.rad出力時の寸法単位_mm)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 62)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(415, 102)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "出力時の寸法単位"
        '
        'nud小数点以下桁数
        '
        Me.nud小数点以下桁数.Location = New System.Drawing.Point(136, 61)
        Me.nud小数点以下桁数.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nud小数点以下桁数.MinimumSize = New System.Drawing.Size(61, 0)
        Me.nud小数点以下桁数.Name = "nud小数点以下桁数"
        Me.nud小数点以下桁数.Size = New System.Drawing.Size(61, 27)
        Me.nud小数点以下桁数.TabIndex = 4
        '
        'lbl小数点以下桁数
        '
        Me.lbl小数点以下桁数.AutoSize = True
        Me.lbl小数点以下桁数.Location = New System.Drawing.Point(11, 64)
        Me.lbl小数点以下桁数.Name = "lbl小数点以下桁数"
        Me.lbl小数点以下桁数.Size = New System.Drawing.Size(114, 20)
        Me.lbl小数点以下桁数.TabIndex = 3
        Me.lbl小数点以下桁数.Text = "小数点以下桁数"
        '
        'rad出力時の寸法単位_in
        '
        Me.rad出力時の寸法単位_in.AutoSize = True
        Me.rad出力時の寸法単位_in.Location = New System.Drawing.Point(291, 19)
        Me.rad出力時の寸法単位_in.Name = "rad出力時の寸法単位_in"
        Me.rad出力時の寸法単位_in.Size = New System.Drawing.Size(42, 24)
        Me.rad出力時の寸法単位_in.TabIndex = 2
        Me.rad出力時の寸法単位_in.Text = "in"
        Me.rad出力時の寸法単位_in.UseVisualStyleBackColor = True
        '
        'rad出力時の寸法単位_cm
        '
        Me.rad出力時の寸法単位_cm.AutoSize = True
        Me.rad出力時の寸法単位_cm.Location = New System.Drawing.Point(215, 20)
        Me.rad出力時の寸法単位_cm.Name = "rad出力時の寸法単位_cm"
        Me.rad出力時の寸法単位_cm.Size = New System.Drawing.Size(50, 24)
        Me.rad出力時の寸法単位_cm.TabIndex = 1
        Me.rad出力時の寸法単位_cm.Text = "cm"
        Me.rad出力時の寸法単位_cm.UseVisualStyleBackColor = True
        '
        'rad出力時の寸法単位_mm
        '
        Me.rad出力時の寸法単位_mm.AutoSize = True
        Me.rad出力時の寸法単位_mm.Checked = True
        Me.rad出力時の寸法単位_mm.Location = New System.Drawing.Point(133, 20)
        Me.rad出力時の寸法単位_mm.Name = "rad出力時の寸法単位_mm"
        Me.rad出力時の寸法単位_mm.Size = New System.Drawing.Size(56, 24)
        Me.rad出力時の寸法単位_mm.TabIndex = 0
        Me.rad出力時の寸法単位_mm.TabStop = True
        Me.rad出力時の寸法単位_mm.Text = "mm"
        Me.rad出力時の寸法単位_mm.UseVisualStyleBackColor = True
        '
        'cmb対象バンドの種類名
        '
        Me.cmb対象バンドの種類名.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmb対象バンドの種類名.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb対象バンドの種類名.FormattingEnabled = True
        Me.cmb対象バンドの種類名.Location = New System.Drawing.Point(152, 12)
        Me.cmb対象バンドの種類名.Name = "cmb対象バンドの種類名"
        Me.cmb対象バンドの種類名.Size = New System.Drawing.Size(193, 28)
        Me.cmb対象バンドの種類名.TabIndex = 1
        '
        'txtバンド幅
        '
        Me.txtバンド幅.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtバンド幅.Location = New System.Drawing.Point(527, 13)
        Me.txtバンド幅.Name = "txtバンド幅"
        Me.txtバンド幅.ReadOnly = True
        Me.txtバンド幅.Size = New System.Drawing.Size(60, 27)
        Me.txtバンド幅.TabIndex = 5
        '
        'lblバンド幅
        '
        Me.lblバンド幅.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblバンド幅.AutoSize = True
        Me.lblバンド幅.Location = New System.Drawing.Point(463, 16)
        Me.lblバンド幅.Name = "lblバンド幅"
        Me.lblバンド幅.Size = New System.Drawing.Size(58, 20)
        Me.lblバンド幅.TabIndex = 4
        Me.lblバンド幅.Text = "バンド幅"
        '
        'lbl本幅
        '
        Me.lbl本幅.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl本幅.AutoSize = True
        Me.lbl本幅.Location = New System.Drawing.Point(410, 16)
        Me.lbl本幅.Name = "lbl本幅"
        Me.lbl本幅.Size = New System.Drawing.Size(45, 20)
        Me.lbl本幅.TabIndex = 3
        Me.lbl本幅.Text = "本幅"
        '
        'lbl対象バンドの種類名
        '
        Me.lbl対象バンドの種類名.AutoSize = True
        Me.lbl対象バンドの種類名.Location = New System.Drawing.Point(16, 15)
        Me.lbl対象バンドの種類名.Name = "lbl対象バンドの種類名"
        Me.lbl対象バンドの種類名.Size = New System.Drawing.Size(130, 20)
        Me.lbl対象バンドの種類名.TabIndex = 0
        Me.lbl対象バンドの種類名.Text = "対象バンドの種類名"
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(517, 180)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 11
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(398, 180)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 10
        Me.btnOK.Text = "OK(&O)"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'lblリスト出力記号
        '
        Me.lblリスト出力記号.AutoSize = True
        Me.lblリスト出力記号.Location = New System.Drawing.Point(27, 183)
        Me.lblリスト出力記号.Name = "lblリスト出力記号"
        Me.lblリスト出力記号.Size = New System.Drawing.Size(101, 20)
        Me.lblリスト出力記号.TabIndex = 8
        Me.lblリスト出力記号.Text = "リスト出力記号"
        '
        'txtリスト出力記号
        '
        Me.txtリスト出力記号.Location = New System.Drawing.Point(152, 180)
        Me.txtリスト出力記号.Name = "txtリスト出力記号"
        Me.txtリスト出力記号.Size = New System.Drawing.Size(83, 27)
        Me.txtリスト出力記号.TabIndex = 9
        '
        'frmSelectBand
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(644, 233)
        Me.Controls.Add(Me.txtリスト出力記号)
        Me.Controls.Add(Me.lblリスト出力記号)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblバンド幅の寸法単位)
        Me.Controls.Add(Me.txt本幅)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmb対象バンドの種類名)
        Me.Controls.Add(Me.txtバンド幅)
        Me.Controls.Add(Me.lblバンド幅)
        Me.Controls.Add(Me.lbl本幅)
        Me.Controls.Add(Me.lbl対象バンドの種類名)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(662, 280)
        Me.Name = "frmSelectBand"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "バンドの種類選択"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nud小数点以下桁数, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblバンド幅の寸法単位 As Windows.Forms.Label
    Friend WithEvents txt本幅 As Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents rad出力時の寸法単位_in As Windows.Forms.RadioButton
    Friend WithEvents rad出力時の寸法単位_cm As Windows.Forms.RadioButton
    Friend WithEvents rad出力時の寸法単位_mm As Windows.Forms.RadioButton
    Friend WithEvents cmb対象バンドの種類名 As Windows.Forms.ComboBox
    Friend WithEvents txtバンド幅 As Windows.Forms.TextBox
    Friend WithEvents lblバンド幅 As Windows.Forms.Label
    Friend WithEvents lbl本幅 As Windows.Forms.Label
    Friend WithEvents lbl対象バンドの種類名 As Windows.Forms.Label
    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents lblリスト出力記号 As Windows.Forms.Label
    Friend WithEvents txtリスト出力記号 As Windows.Forms.TextBox
    Friend WithEvents lbl小数点以下桁数 As Windows.Forms.Label
    Friend WithEvents nud小数点以下桁数 As Windows.Forms.NumericUpDown
End Class
