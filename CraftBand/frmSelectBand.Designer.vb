﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.components = New System.ComponentModel.Container()
        Me.lblバンド幅の寸法単位 = New System.Windows.Forms.Label()
        Me.txt本幅 = New System.Windows.Forms.TextBox()
        Me.grp出力時の寸法単位 = New System.Windows.Forms.GroupBox()
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
        Me.grp四つ畳み編みの上の縦ひも位置 = New System.Windows.Forms.GroupBox()
        Me.nudマイひも長係数 = New System.Windows.Forms.NumericUpDown()
        Me.lblマイひも長係数 = New System.Windows.Forms.Label()
        Me.rad右側 = New System.Windows.Forms.RadioButton()
        Me.rad左側 = New System.Windows.Forms.RadioButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grp出力時の寸法単位.SuspendLayout()
        CType(Me.nud小数点以下桁数, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp四つ畳み編みの上の縦ひも位置.SuspendLayout()
        CType(Me.nudマイひも長係数, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.ToolTip1.SetToolTip(Me.txt本幅, "選んだバンドが何本幅か")
        '
        'grp出力時の寸法単位
        '
        Me.grp出力時の寸法単位.Controls.Add(Me.nud小数点以下桁数)
        Me.grp出力時の寸法単位.Controls.Add(Me.lbl小数点以下桁数)
        Me.grp出力時の寸法単位.Controls.Add(Me.rad出力時の寸法単位_in)
        Me.grp出力時の寸法単位.Controls.Add(Me.rad出力時の寸法単位_cm)
        Me.grp出力時の寸法単位.Controls.Add(Me.rad出力時の寸法単位_mm)
        Me.grp出力時の寸法単位.Location = New System.Drawing.Point(16, 62)
        Me.grp出力時の寸法単位.Name = "grp出力時の寸法単位"
        Me.grp出力時の寸法単位.Size = New System.Drawing.Size(368, 102)
        Me.grp出力時の寸法単位.TabIndex = 7
        Me.grp出力時の寸法単位.TabStop = False
        Me.grp出力時の寸法単位.Text = "出力時の寸法単位"
        Me.ToolTip1.SetToolTip(Me.grp出力時の寸法単位, "ひもリスト出力時の長さの単位")
        '
        'nud小数点以下桁数
        '
        Me.nud小数点以下桁数.Location = New System.Drawing.Point(136, 61)
        Me.nud小数点以下桁数.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nud小数点以下桁数.MinimumSize = New System.Drawing.Size(61, 0)
        Me.nud小数点以下桁数.Name = "nud小数点以下桁数"
        Me.nud小数点以下桁数.Size = New System.Drawing.Size(61, 27)
        Me.nud小数点以下桁数.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.nud小数点以下桁数, "何桁で丸めて表示するか")
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
        Me.rad出力時の寸法単位_in.Location = New System.Drawing.Point(291, 23)
        Me.rad出力時の寸法単位_in.Name = "rad出力時の寸法単位_in"
        Me.rad出力時の寸法単位_in.Size = New System.Drawing.Size(42, 24)
        Me.rad出力時の寸法単位_in.TabIndex = 2
        Me.rad出力時の寸法単位_in.Text = "in"
        Me.ToolTip1.SetToolTip(Me.rad出力時の寸法単位_in, "インチ")
        Me.rad出力時の寸法単位_in.UseVisualStyleBackColor = True
        '
        'rad出力時の寸法単位_cm
        '
        Me.rad出力時の寸法単位_cm.AutoSize = True
        Me.rad出力時の寸法単位_cm.Location = New System.Drawing.Point(215, 23)
        Me.rad出力時の寸法単位_cm.Name = "rad出力時の寸法単位_cm"
        Me.rad出力時の寸法単位_cm.Size = New System.Drawing.Size(50, 24)
        Me.rad出力時の寸法単位_cm.TabIndex = 1
        Me.rad出力時の寸法単位_cm.Text = "cm"
        Me.ToolTip1.SetToolTip(Me.rad出力時の寸法単位_cm, "センチメートル")
        Me.rad出力時の寸法単位_cm.UseVisualStyleBackColor = True
        '
        'rad出力時の寸法単位_mm
        '
        Me.rad出力時の寸法単位_mm.AutoSize = True
        Me.rad出力時の寸法単位_mm.Checked = True
        Me.rad出力時の寸法単位_mm.Location = New System.Drawing.Point(133, 23)
        Me.rad出力時の寸法単位_mm.Name = "rad出力時の寸法単位_mm"
        Me.rad出力時の寸法単位_mm.Size = New System.Drawing.Size(56, 24)
        Me.rad出力時の寸法単位_mm.TabIndex = 0
        Me.rad出力時の寸法単位_mm.TabStop = True
        Me.rad出力時の寸法単位_mm.Text = "mm"
        Me.ToolTip1.SetToolTip(Me.rad出力時の寸法単位_mm, "ミリメートル")
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
        Me.ToolTip1.SetToolTip(Me.cmb対象バンドの種類名, "使いたいバンドの種類名を選びます")
        '
        'txtバンド幅
        '
        Me.txtバンド幅.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtバンド幅.Location = New System.Drawing.Point(527, 13)
        Me.txtバンド幅.Name = "txtバンド幅"
        Me.txtバンド幅.ReadOnly = True
        Me.txtバンド幅.Size = New System.Drawing.Size(60, 27)
        Me.txtバンド幅.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.txtバンド幅, "選んだバンドの幅")
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
        Me.lbl本幅.Size = New System.Drawing.Size(39, 20)
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
        Me.btnキャンセル.TabIndex = 12
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.ToolTip1.SetToolTip(Me.btnキャンセル, "変更を保存せずに終了します")
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(398, 180)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 11
        Me.btnOK.Text = "OK(&O)"
        Me.ToolTip1.SetToolTip(Me.btnOK, "変更を保存して終了します")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'lblリスト出力記号
        '
        Me.lblリスト出力記号.AutoSize = True
        Me.lblリスト出力記号.Location = New System.Drawing.Point(27, 183)
        Me.lblリスト出力記号.Name = "lblリスト出力記号"
        Me.lblリスト出力記号.Size = New System.Drawing.Size(101, 20)
        Me.lblリスト出力記号.TabIndex = 9
        Me.lblリスト出力記号.Text = "リスト出力記号"
        '
        'txtリスト出力記号
        '
        Me.txtリスト出力記号.Location = New System.Drawing.Point(152, 180)
        Me.txtリスト出力記号.Name = "txtリスト出力記号"
        Me.txtリスト出力記号.Size = New System.Drawing.Size(83, 27)
        Me.txtリスト出力記号.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.txtリスト出力記号, "リストに付加する記号の最初の文字")
        '
        'grp四つ畳み編みの上の縦ひも位置
        '
        Me.grp四つ畳み編みの上の縦ひも位置.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grp四つ畳み編みの上の縦ひも位置.Controls.Add(Me.nudマイひも長係数)
        Me.grp四つ畳み編みの上の縦ひも位置.Controls.Add(Me.lblマイひも長係数)
        Me.grp四つ畳み編みの上の縦ひも位置.Controls.Add(Me.rad右側)
        Me.grp四つ畳み編みの上の縦ひも位置.Controls.Add(Me.rad左側)
        Me.grp四つ畳み編みの上の縦ひも位置.Location = New System.Drawing.Point(398, 62)
        Me.grp四つ畳み編みの上の縦ひも位置.Name = "grp四つ畳み編みの上の縦ひも位置"
        Me.grp四つ畳み編みの上の縦ひも位置.Size = New System.Drawing.Size(232, 102)
        Me.grp四つ畳み編みの上の縦ひも位置.TabIndex = 8
        Me.grp四つ畳み編みの上の縦ひも位置.TabStop = False
        Me.grp四つ畳み編みの上の縦ひも位置.Text = "四つ畳み編みの上の縦ひも位置"
        Me.grp四つ畳み編みの上の縦ひも位置.Visible = False
        '
        'nudマイひも長係数
        '
        Me.nudマイひも長係数.DecimalPlaces = 2
        Me.nudマイひも長係数.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudマイひも長係数.Location = New System.Drawing.Point(129, 61)
        Me.nudマイひも長係数.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudマイひも長係数.Name = "nudマイひも長係数"
        Me.nudマイひも長係数.Size = New System.Drawing.Size(68, 27)
        Me.nudマイひも長係数.TabIndex = 9
        Me.nudマイひも長係数.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.nudマイひも長係数, "出力する長さを一律で変えたい時")
        Me.nudマイひも長係数.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblマイひも長係数
        '
        Me.lblマイひも長係数.AutoSize = True
        Me.lblマイひも長係数.Location = New System.Drawing.Point(24, 64)
        Me.lblマイひも長係数.Name = "lblマイひも長係数"
        Me.lblマイひも長係数.Size = New System.Drawing.Size(99, 20)
        Me.lblマイひも長係数.TabIndex = 2
        Me.lblマイひも長係数.Text = "マイひも長係数"
        '
        'rad右側
        '
        Me.rad右側.AutoSize = True
        Me.rad右側.Checked = True
        Me.rad右側.Location = New System.Drawing.Point(127, 30)
        Me.rad右側.Name = "rad右側"
        Me.rad右側.Size = New System.Drawing.Size(60, 24)
        Me.rad右側.TabIndex = 1
        Me.rad右側.TabStop = True
        Me.rad右側.Text = "右側"
        Me.ToolTip1.SetToolTip(Me.rad右側, "縦ひもが右上に伸びるタイプ")
        Me.rad右側.UseVisualStyleBackColor = True
        '
        'rad左側
        '
        Me.rad左側.AutoSize = True
        Me.rad左側.Location = New System.Drawing.Point(52, 30)
        Me.rad左側.Name = "rad左側"
        Me.rad左側.Size = New System.Drawing.Size(60, 24)
        Me.rad左側.TabIndex = 0
        Me.rad左側.Text = "左側"
        Me.ToolTip1.SetToolTip(Me.rad左側, "縦ひもが左上に伸びるタイプ")
        Me.rad左側.UseVisualStyleBackColor = True
        '
        'frmSelectBand
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(644, 233)
        Me.Controls.Add(Me.grp四つ畳み編みの上の縦ひも位置)
        Me.Controls.Add(Me.txtリスト出力記号)
        Me.Controls.Add(Me.lblリスト出力記号)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblバンド幅の寸法単位)
        Me.Controls.Add(Me.txt本幅)
        Me.Controls.Add(Me.grp出力時の寸法単位)
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
        Me.grp出力時の寸法単位.ResumeLayout(False)
        Me.grp出力時の寸法単位.PerformLayout()
        CType(Me.nud小数点以下桁数, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp四つ畳み編みの上の縦ひも位置.ResumeLayout(False)
        Me.grp四つ畳み編みの上の縦ひも位置.PerformLayout()
        CType(Me.nudマイひも長係数, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblバンド幅の寸法単位 As Windows.Forms.Label
    Friend WithEvents txt本幅 As Windows.Forms.TextBox
    Friend WithEvents grp出力時の寸法単位 As Windows.Forms.GroupBox
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
    Friend WithEvents grp四つ畳み編みの上の縦ひも位置 As Windows.Forms.GroupBox
    Friend WithEvents rad右側 As Windows.Forms.RadioButton
    Friend WithEvents rad左側 As Windows.Forms.RadioButton
    Friend WithEvents nudマイひも長係数 As Windows.Forms.NumericUpDown
    Public WithEvents lblマイひも長係数 As Windows.Forms.Label
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
End Class
