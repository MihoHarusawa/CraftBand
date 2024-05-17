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
        components = New ComponentModel.Container()
        lblバンド幅の寸法単位 = New Windows.Forms.Label()
        txt本幅 = New Windows.Forms.TextBox()
        grp出力時の寸法単位 = New Windows.Forms.GroupBox()
        nud小数点以下桁数 = New Windows.Forms.NumericUpDown()
        lbl小数点以下桁数 = New Windows.Forms.Label()
        rad出力時の寸法単位_in = New Windows.Forms.RadioButton()
        rad出力時の寸法単位_cm = New Windows.Forms.RadioButton()
        rad出力時の寸法単位_mm = New Windows.Forms.RadioButton()
        cmb対象バンドの種類名 = New Windows.Forms.ComboBox()
        txtバンド幅 = New Windows.Forms.TextBox()
        lblバンド幅 = New Windows.Forms.Label()
        lbl本幅 = New Windows.Forms.Label()
        lbl対象バンドの種類名 = New Windows.Forms.Label()
        btnキャンセル = New Windows.Forms.Button()
        btnOK = New Windows.Forms.Button()
        lblリスト出力記号 = New Windows.Forms.Label()
        txtリスト出力記号 = New Windows.Forms.TextBox()
        grp四つ畳み編みの上の縦ひも位置 = New Windows.Forms.GroupBox()
        nudマイひも長係数 = New Windows.Forms.NumericUpDown()
        lblマイひも長係数 = New Windows.Forms.Label()
        rad右側 = New Windows.Forms.RadioButton()
        rad左側 = New Windows.Forms.RadioButton()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        grp出力時の寸法単位.SuspendLayout()
        CType(nud小数点以下桁数, ComponentModel.ISupportInitialize).BeginInit()
        grp四つ畳み編みの上の縦ひも位置.SuspendLayout()
        CType(nudマイひも長係数, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblバンド幅の寸法単位
        ' 
        lblバンド幅の寸法単位.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        lblバンド幅の寸法単位.AutoSize = True
        lblバンド幅の寸法単位.Location = New System.Drawing.Point(593, 20)
        lblバンド幅の寸法単位.Name = "lblバンド幅の寸法単位"
        lblバンド幅の寸法単位.Size = New System.Drawing.Size(35, 20)
        lblバンド幅の寸法単位.TabIndex = 6
        lblバンド幅の寸法単位.Text = "mm"
        ' 
        ' txt本幅
        ' 
        txt本幅.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        txt本幅.Location = New System.Drawing.Point(354, 13)
        txt本幅.Name = "txt本幅"
        txt本幅.ReadOnly = True
        txt本幅.Size = New System.Drawing.Size(50, 27)
        txt本幅.TabIndex = 2
        ToolTip1.SetToolTip(txt本幅, "選んだバンドが何本幅か")
        ' 
        ' grp出力時の寸法単位
        ' 
        grp出力時の寸法単位.Controls.Add(nud小数点以下桁数)
        grp出力時の寸法単位.Controls.Add(lbl小数点以下桁数)
        grp出力時の寸法単位.Controls.Add(rad出力時の寸法単位_in)
        grp出力時の寸法単位.Controls.Add(rad出力時の寸法単位_cm)
        grp出力時の寸法単位.Controls.Add(rad出力時の寸法単位_mm)
        grp出力時の寸法単位.Location = New System.Drawing.Point(16, 62)
        grp出力時の寸法単位.Name = "grp出力時の寸法単位"
        grp出力時の寸法単位.Size = New System.Drawing.Size(368, 102)
        grp出力時の寸法単位.TabIndex = 7
        grp出力時の寸法単位.TabStop = False
        grp出力時の寸法単位.Text = "出力時の寸法単位"
        ToolTip1.SetToolTip(grp出力時の寸法単位, "ひもリスト出力時の長さの単位")
        ' 
        ' nud小数点以下桁数
        ' 
        nud小数点以下桁数.Location = New System.Drawing.Point(136, 61)
        nud小数点以下桁数.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        nud小数点以下桁数.MinimumSize = New System.Drawing.Size(61, 0)
        nud小数点以下桁数.Name = "nud小数点以下桁数"
        nud小数点以下桁数.Size = New System.Drawing.Size(61, 27)
        nud小数点以下桁数.TabIndex = 4
        ToolTip1.SetToolTip(nud小数点以下桁数, "何桁で丸めて表示するか")
        ' 
        ' lbl小数点以下桁数
        ' 
        lbl小数点以下桁数.AutoSize = True
        lbl小数点以下桁数.Location = New System.Drawing.Point(11, 64)
        lbl小数点以下桁数.Name = "lbl小数点以下桁数"
        lbl小数点以下桁数.Size = New System.Drawing.Size(114, 20)
        lbl小数点以下桁数.TabIndex = 3
        lbl小数点以下桁数.Text = "小数点以下桁数"
        ' 
        ' rad出力時の寸法単位_in
        ' 
        rad出力時の寸法単位_in.AutoSize = True
        rad出力時の寸法単位_in.Location = New System.Drawing.Point(291, 23)
        rad出力時の寸法単位_in.Name = "rad出力時の寸法単位_in"
        rad出力時の寸法単位_in.Size = New System.Drawing.Size(42, 24)
        rad出力時の寸法単位_in.TabIndex = 2
        rad出力時の寸法単位_in.Text = "in"
        ToolTip1.SetToolTip(rad出力時の寸法単位_in, "インチ")
        rad出力時の寸法単位_in.UseVisualStyleBackColor = True
        ' 
        ' rad出力時の寸法単位_cm
        ' 
        rad出力時の寸法単位_cm.AutoSize = True
        rad出力時の寸法単位_cm.Location = New System.Drawing.Point(215, 23)
        rad出力時の寸法単位_cm.Name = "rad出力時の寸法単位_cm"
        rad出力時の寸法単位_cm.Size = New System.Drawing.Size(50, 24)
        rad出力時の寸法単位_cm.TabIndex = 1
        rad出力時の寸法単位_cm.Text = "cm"
        ToolTip1.SetToolTip(rad出力時の寸法単位_cm, "センチメートル")
        rad出力時の寸法単位_cm.UseVisualStyleBackColor = True
        ' 
        ' rad出力時の寸法単位_mm
        ' 
        rad出力時の寸法単位_mm.AutoSize = True
        rad出力時の寸法単位_mm.Checked = True
        rad出力時の寸法単位_mm.Location = New System.Drawing.Point(133, 23)
        rad出力時の寸法単位_mm.Name = "rad出力時の寸法単位_mm"
        rad出力時の寸法単位_mm.Size = New System.Drawing.Size(56, 24)
        rad出力時の寸法単位_mm.TabIndex = 0
        rad出力時の寸法単位_mm.TabStop = True
        rad出力時の寸法単位_mm.Text = "mm"
        ToolTip1.SetToolTip(rad出力時の寸法単位_mm, "ミリメートル")
        rad出力時の寸法単位_mm.UseVisualStyleBackColor = True
        ' 
        ' cmb対象バンドの種類名
        ' 
        cmb対象バンドの種類名.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        cmb対象バンドの種類名.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
        cmb対象バンドの種類名.FormattingEnabled = True
        cmb対象バンドの種類名.Location = New System.Drawing.Point(152, 12)
        cmb対象バンドの種類名.Name = "cmb対象バンドの種類名"
        cmb対象バンドの種類名.Size = New System.Drawing.Size(193, 28)
        cmb対象バンドの種類名.TabIndex = 1
        ToolTip1.SetToolTip(cmb対象バンドの種類名, "使いたいバンドの種類名を選びます")
        ' 
        ' txtバンド幅
        ' 
        txtバンド幅.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        txtバンド幅.Location = New System.Drawing.Point(527, 13)
        txtバンド幅.Name = "txtバンド幅"
        txtバンド幅.ReadOnly = True
        txtバンド幅.Size = New System.Drawing.Size(60, 27)
        txtバンド幅.TabIndex = 5
        ToolTip1.SetToolTip(txtバンド幅, "選んだバンドの幅")
        ' 
        ' lblバンド幅
        ' 
        lblバンド幅.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        lblバンド幅.AutoSize = True
        lblバンド幅.Location = New System.Drawing.Point(463, 16)
        lblバンド幅.Name = "lblバンド幅"
        lblバンド幅.Size = New System.Drawing.Size(58, 20)
        lblバンド幅.TabIndex = 4
        lblバンド幅.Text = "バンド幅"
        ' 
        ' lbl本幅
        ' 
        lbl本幅.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        lbl本幅.AutoSize = True
        lbl本幅.Location = New System.Drawing.Point(410, 16)
        lbl本幅.Name = "lbl本幅"
        lbl本幅.Size = New System.Drawing.Size(39, 20)
        lbl本幅.TabIndex = 3
        lbl本幅.Text = "本幅"
        ' 
        ' lbl対象バンドの種類名
        ' 
        lbl対象バンドの種類名.AutoSize = True
        lbl対象バンドの種類名.Location = New System.Drawing.Point(16, 15)
        lbl対象バンドの種類名.Name = "lbl対象バンドの種類名"
        lbl対象バンドの種類名.Size = New System.Drawing.Size(130, 20)
        lbl対象バンドの種類名.TabIndex = 0
        lbl対象バンドの種類名.Text = "対象バンドの種類名"
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnキャンセル.DialogResult = Windows.Forms.DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(517, 180)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 46)
        btnキャンセル.TabIndex = 12
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "変更を保存せずに終了します")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(398, 180)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 46)
        btnOK.TabIndex = 11
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "変更を保存して終了します")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' lblリスト出力記号
        ' 
        lblリスト出力記号.AutoSize = True
        lblリスト出力記号.Location = New System.Drawing.Point(27, 183)
        lblリスト出力記号.Name = "lblリスト出力記号"
        lblリスト出力記号.Size = New System.Drawing.Size(101, 20)
        lblリスト出力記号.TabIndex = 9
        lblリスト出力記号.Text = "リスト出力記号"
        ' 
        ' txtリスト出力記号
        ' 
        txtリスト出力記号.Location = New System.Drawing.Point(152, 180)
        txtリスト出力記号.Name = "txtリスト出力記号"
        txtリスト出力記号.Size = New System.Drawing.Size(83, 27)
        txtリスト出力記号.TabIndex = 10
        ToolTip1.SetToolTip(txtリスト出力記号, "リストに付加する記号の最初の文字")
        ' 
        ' grp四つ畳み編みの上の縦ひも位置
        ' 
        grp四つ畳み編みの上の縦ひも位置.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        grp四つ畳み編みの上の縦ひも位置.Controls.Add(nudマイひも長係数)
        grp四つ畳み編みの上の縦ひも位置.Controls.Add(lblマイひも長係数)
        grp四つ畳み編みの上の縦ひも位置.Controls.Add(rad右側)
        grp四つ畳み編みの上の縦ひも位置.Controls.Add(rad左側)
        grp四つ畳み編みの上の縦ひも位置.Location = New System.Drawing.Point(398, 62)
        grp四つ畳み編みの上の縦ひも位置.Name = "grp四つ畳み編みの上の縦ひも位置"
        grp四つ畳み編みの上の縦ひも位置.Size = New System.Drawing.Size(232, 102)
        grp四つ畳み編みの上の縦ひも位置.TabIndex = 8
        grp四つ畳み編みの上の縦ひも位置.TabStop = False
        grp四つ畳み編みの上の縦ひも位置.Text = "四つ畳み編みの上の縦ひも位置"
        grp四つ畳み編みの上の縦ひも位置.Visible = False
        ' 
        ' nudマイひも長係数
        ' 
        nudマイひも長係数.DecimalPlaces = 2
        nudマイひも長係数.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        nudマイひも長係数.Location = New System.Drawing.Point(129, 61)
        nudマイひも長係数.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        nudマイひも長係数.Name = "nudマイひも長係数"
        nudマイひも長係数.Size = New System.Drawing.Size(68, 27)
        nudマイひも長係数.TabIndex = 9
        nudマイひも長係数.TextAlign = Windows.Forms.HorizontalAlignment.Right
        ToolTip1.SetToolTip(nudマイひも長係数, "出力する長さを一律で変えたい時")
        nudマイひも長係数.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' lblマイひも長係数
        ' 
        lblマイひも長係数.AutoSize = True
        lblマイひも長係数.Location = New System.Drawing.Point(24, 64)
        lblマイひも長係数.Name = "lblマイひも長係数"
        lblマイひも長係数.Size = New System.Drawing.Size(99, 20)
        lblマイひも長係数.TabIndex = 2
        lblマイひも長係数.Text = "マイひも長係数"
        ' 
        ' rad右側
        ' 
        rad右側.AutoSize = True
        rad右側.Checked = True
        rad右側.Location = New System.Drawing.Point(127, 30)
        rad右側.Name = "rad右側"
        rad右側.Size = New System.Drawing.Size(60, 24)
        rad右側.TabIndex = 1
        rad右側.TabStop = True
        rad右側.Text = "右側"
        ToolTip1.SetToolTip(rad右側, "縦ひもが右上に伸びるタイプ")
        rad右側.UseVisualStyleBackColor = True
        ' 
        ' rad左側
        ' 
        rad左側.AutoSize = True
        rad左側.Location = New System.Drawing.Point(52, 30)
        rad左側.Name = "rad左側"
        rad左側.Size = New System.Drawing.Size(60, 24)
        rad左側.TabIndex = 0
        rad左側.Text = "左側"
        ToolTip1.SetToolTip(rad左側, "縦ひもが左上に伸びるタイプ")
        rad左側.UseVisualStyleBackColor = True
        ' 
        ' frmSelectBand
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(644, 233)
        Controls.Add(grp四つ畳み編みの上の縦ひも位置)
        Controls.Add(txtリスト出力記号)
        Controls.Add(lblリスト出力記号)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        Controls.Add(lblバンド幅の寸法単位)
        Controls.Add(txt本幅)
        Controls.Add(grp出力時の寸法単位)
        Controls.Add(cmb対象バンドの種類名)
        Controls.Add(txtバンド幅)
        Controls.Add(lblバンド幅)
        Controls.Add(lbl本幅)
        Controls.Add(lbl対象バンドの種類名)
        MaximizeBox = False
        MinimizeBox = False
        MinimumSize = New System.Drawing.Size(662, 280)
        Name = "frmSelectBand"
        StartPosition = Windows.Forms.FormStartPosition.CenterParent
        Text = "バンドの種類選択"
        grp出力時の寸法単位.ResumeLayout(False)
        grp出力時の寸法単位.PerformLayout()
        CType(nud小数点以下桁数, ComponentModel.ISupportInitialize).EndInit()
        grp四つ畳み編みの上の縦ひも位置.ResumeLayout(False)
        grp四つ畳み編みの上の縦ひも位置.PerformLayout()
        CType(nudマイひも長係数, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

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
