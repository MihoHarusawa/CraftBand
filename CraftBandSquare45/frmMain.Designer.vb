﻿Imports CraftBand

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        BindingSource縁の始末 = New BindingSource(components)
        ToolTip1 = New ToolTip(components)
        nud基本のひも幅 = New NumericUpDown()
        lbl基本のひも幅 = New Label()
        lbl横寸法 = New Label()
        nud横寸法 = New NumericUpDown()
        nud縦寸法 = New NumericUpDown()
        lbl縦寸法 = New Label()
        nud高さ寸法 = New NumericUpDown()
        lbl高さ寸法 = New Label()
        chk縦の補強ひも = New CheckBox()
        chk横の補強ひも = New CheckBox()
        btn概算 = New Button()
        btnひもリスト = New Button()
        lbl垂直ひも数 = New Label()
        txt垂直ひも数 = New TextBox()
        nudひも間のすき間 = New NumericUpDown()
        lblひも間のすき間 = New Label()
        nudひも長加算 = New NumericUpDown()
        lblひも長加算 = New Label()
        btn規定値 = New Button()
        btnリセット = New Button()
        txt四角ベース_横 = New TextBox()
        txt四角ベース_縦 = New TextBox()
        txt四角ベース_高さ = New TextBox()
        txt縁厚さプラス_横 = New TextBox()
        txt縁厚さプラス_縦 = New TextBox()
        txt縁厚さプラス_高さ = New TextBox()
        txt四角ベース_周 = New TextBox()
        txt縁厚さプラス_周 = New TextBox()
        lbl横ひも = New Label()
        nudひも長係数 = New NumericUpDown()
        nud高さの四角数 = New NumericUpDown()
        lbl高さの四角数 = New Label()
        lbl縦ひも = New Label()
        lbl1つの辺 = New Label()
        lblひも幅 = New Label()
        Label10 = New Label()
        txt縦ひもの本数 = New TextBox()
        txt横ひもの本数 = New TextBox()
        lbl四角 = New Label()
        lbl厚さ = New Label()
        txt厚さ = New TextBox()
        btn画像ファイル = New Button()
        btnブラウザ = New Button()
        lbl縦の四角数 = New Label()
        nud縦の四角数 = New NumericUpDown()
        lbl横の四角数 = New Label()
        nud横の四角数 = New NumericUpDown()
        grp目標寸法 = New GroupBox()
        rad目標寸法_内側 = New RadioButton()
        rad目標寸法_外側 = New RadioButton()
        chk縦横を展開する = New CheckBox()
        txt対角線_ひも幅 = New TextBox()
        txt1つの辺_ひも幅 = New TextBox()
        lbl対角線 = New Label()
        txt対角線_四角 = New TextBox()
        txt1つの辺_四角 = New TextBox()
        nud底に = New NumericUpDown()
        chk横の辺 = New CheckBox()
        nud垂直に = New NumericUpDown()
        btn合わせる = New Button()
        txtメモ = New TextBox()
        txt作成者 = New TextBox()
        txtタイトル = New TextBox()
        lbl本幅の差 = New Label()
        txt先の三角形の本幅の差 = New TextBox()
        txt四辺形の本幅の差 = New TextBox()
        txt後の三角形の本幅の差 = New TextBox()
        btn画像ファイル2 = New Button()
        btnブラウザ2 = New Button()
        btn3Dモデル = New Button()
        radビューア = New RadioButton()
        radファイル = New RadioButton()
        MenuStrip1 = New MenuStrip()
        ToolStripMenuItemFile = New ToolStripMenuItem()
        ToolStripMenuItemFileNew = New ToolStripMenuItem()
        ToolStripMenuItemFileOpen = New ToolStripMenuItem()
        ToolStripMenuItemFileSaveAs = New ToolStripMenuItem()
        ToolStripMenuItemFileSave = New ToolStripMenuItem()
        ToolStripSeparator2 = New ToolStripSeparator()
        ToolStripMenuItemFileAbort = New ToolStripMenuItem()
        ToolStripMenuItemFileExit = New ToolStripMenuItem()
        ToolStripMenuItemEdit = New ToolStripMenuItem()
        ToolStripMenuItemEditSelectBand = New ToolStripMenuItem()
        ToolStripSeparator4 = New ToolStripSeparator()
        ToolStripMenuItemEditReset = New ToolStripMenuItem()
        ToolStripMenuItemEditDefault = New ToolStripMenuItem()
        ToolStripMenuItemEditCalc = New ToolStripMenuItem()
        ToolStripSeparator5 = New ToolStripSeparator()
        ToolStripMenuItemEditColorChange = New ToolStripMenuItem()
        ToolStripMenuItemEditColorRepeat = New ToolStripMenuItem()
        ToolStripSeparator1 = New ToolStripSeparator()
        ToolStripMenuItemEditList = New ToolStripMenuItem()
        ToolStripMenuItemEditDefaultFile = New ToolStripMenuItem()
        ToolStripMenuItemSetting = New ToolStripMenuItem()
        ToolStripMenuItemSettingBandType = New ToolStripMenuItem()
        ToolStripMenuItemSettingPattern = New ToolStripMenuItem()
        ToolStripMenuItemSettingOptions = New ToolStripMenuItem()
        ToolStripMenuItemSettingColor = New ToolStripMenuItem()
        ToolStripMenuItemSettingUpDown = New ToolStripMenuItem()
        ToolStripSeparator3 = New ToolStripSeparator()
        ToolStripMenuItemSettingBasics = New ToolStripMenuItem()
        ToolStripMenuItemHelp = New ToolStripMenuItem()
        lbl基本のひも幅_単位 = New Label()
        lbl目標寸法_単位 = New Label()
        TabControl = New TabControl()
        tpage四角数 = New TabPage()
        lbl高さの四角数_単位 = New Label()
        lblひも長加算_単位 = New Label()
        lblひも間のすき間_単位 = New Label()
        grp縦置き = New GroupBox()
        lbl縦ひもの本数_単位 = New Label()
        lbl縦ひものメモ = New Label()
        txt縦ひものメモ = New TextBox()
        lbl縦の四角数_単位 = New Label()
        grp横置き = New GroupBox()
        lbl横ひもの本数_単位 = New Label()
        lbl横ひものメモ = New Label()
        txt横ひものメモ = New TextBox()
        lbl横の四角数_単位 = New Label()
        tpage縁の始末 = New TabPage()
        dgv縁の始末 = New ctrDataGridView()
        lbl編みかた名_側面 = New Label()
        btn削除_側面 = New Button()
        btn追加_側面 = New Button()
        cmb編みかた名_側面 = New ComboBox()
        tpage追加品 = New TabPage()
        editAddParts = New ctrAddParts()
        tpageメモ他 = New TabPage()
        lbl作成者 = New Label()
        lblタイトル = New Label()
        lblメモ = New Label()
        tpage横ひも = New TabPage()
        expand横ひも = New ctrExpanding()
        tpage縦ひも = New TabPage()
        expand縦ひも = New ctrExpanding()
        tpageプレビュー = New TabPage()
        radうら = New RadioButton()
        radおもて = New RadioButton()
        picプレビュー = New PictureBox()
        tpageひも上下 = New TabPage()
        grp縦横の四角 = New GroupBox()
        lbl底に = New Label()
        lbl垂直に = New Label()
        editUpDown = New ctrEditUpDown()
        tpageプレビュー2 = New TabPage()
        grp3D = New GroupBox()
        picプレビュー2 = New PictureBox()
        f_i段数2 = New DataGridViewTextBoxColumn()
        lbl四角ベース = New Label()
        lbl計算寸法 = New Label()
        lbl計算寸法縦 = New Label()
        lbl計算寸法高さ = New Label()
        lbl計算寸法_単位 = New Label()
        lbl縁厚さプラス = New Label()
        lbl計算寸法横 = New Label()
        lbl計算寸法周 = New Label()
        btn終了 = New Button()
        OpenFileDialog1 = New OpenFileDialog()
        SaveFileDialog1 = New SaveFileDialog()
        cmb基本色 = New ComboBox()
        lbl基本色 = New Label()
        lbl横寸法の差 = New Label()
        lbl縦寸法の差 = New Label()
        lbl高さ寸法の差 = New Label()
        txtバンドの種類名 = New TextBox()
        lbl基本のひも幅length = New Label()
        StatusStrip1 = New StatusStrip()
        ToolStripStatusLabel1 = New ToolStripStatusLabel()
        ToolStripStatusLabel2 = New ToolStripStatusLabel()
        lbl単位 = New Label()
        btnDEBUG = New Button()
        f_i番号 = New DataGridViewTextBoxColumn()
        f_s編みかた名 = New DataGridViewTextBoxColumn()
        f_s編みひも名 = New DataGridViewTextBoxColumn()
        f_iひも番号 = New DataGridViewTextBoxColumn()
        f_i何本幅 = New DataGridViewComboBoxColumn()
        f_b集計対象外区分 = New DataGridViewCheckBoxColumn()
        f_s色 = New DataGridViewComboBoxColumn()
        f_d高さ = New DataGridViewTextBoxColumn()
        f_d垂直ひも長 = New DataGridViewTextBoxColumn()
        f_d周長比率対底の周 = New DataGridViewTextBoxColumn()
        f_d周長 = New DataGridViewTextBoxColumn()
        f_dひも長 = New DataGridViewTextBoxColumn()
        f_dひも長加算 = New DataGridViewTextBoxColumn()
        f_iひも本数 = New DataGridViewTextBoxColumn()
        f_d厚さ = New DataGridViewTextBoxColumn()
        f_sメモ = New DataGridViewTextBoxColumn()
        f_d連続ひも長 = New DataGridViewTextBoxColumn()
        f_s記号 = New DataGridViewTextBoxColumn()
        f_bError = New DataGridViewCheckBoxColumn()
        CType(BindingSource縁の始末, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud基本のひも幅, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud横寸法, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud縦寸法, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud高さ寸法, ComponentModel.ISupportInitialize).BeginInit()
        CType(nudひも間のすき間, ComponentModel.ISupportInitialize).BeginInit()
        CType(nudひも長加算, ComponentModel.ISupportInitialize).BeginInit()
        CType(nudひも長係数, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud高さの四角数, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud縦の四角数, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud横の四角数, ComponentModel.ISupportInitialize).BeginInit()
        grp目標寸法.SuspendLayout()
        CType(nud底に, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud垂直に, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        TabControl.SuspendLayout()
        tpage四角数.SuspendLayout()
        grp縦置き.SuspendLayout()
        grp横置き.SuspendLayout()
        tpage縁の始末.SuspendLayout()
        CType(dgv縁の始末, ComponentModel.ISupportInitialize).BeginInit()
        tpage追加品.SuspendLayout()
        tpageメモ他.SuspendLayout()
        tpage横ひも.SuspendLayout()
        tpage縦ひも.SuspendLayout()
        tpageプレビュー.SuspendLayout()
        CType(picプレビュー, ComponentModel.ISupportInitialize).BeginInit()
        tpageひも上下.SuspendLayout()
        grp縦横の四角.SuspendLayout()
        tpageプレビュー2.SuspendLayout()
        grp3D.SuspendLayout()
        CType(picプレビュー2, ComponentModel.ISupportInitialize).BeginInit()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' BindingSource縁の始末
        ' 
        BindingSource縁の始末.DataMember = "tbl側面"
        BindingSource縁の始末.DataSource = GetType(Tables.dstDataTables)
        ' 
        ' nud基本のひも幅
        ' 
        nud基本のひも幅.Location = New Point(618, 66)
        nud基本のひも幅.Name = "nud基本のひも幅"
        nud基本のひも幅.Size = New Size(66, 27)
        nud基本のひも幅.TabIndex = 14
        nud基本のひも幅.TextAlign = HorizontalAlignment.Center
        ToolTip1.SetToolTip(nud基本のひも幅, "何本幅のひもを使うか")
        nud基本のひも幅.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' lbl基本のひも幅
        ' 
        lbl基本のひも幅.AutoSize = True
        lbl基本のひも幅.Location = New Point(618, 39)
        lbl基本のひも幅.Name = "lbl基本のひも幅"
        lbl基本のひも幅.Size = New Size(89, 20)
        lbl基本のひも幅.TabIndex = 13
        lbl基本のひも幅.Text = "基本のひも幅"
        ToolTip1.SetToolTip(lbl基本のひも幅, "何本幅のひもを使うか")
        ' 
        ' lbl横寸法
        ' 
        lbl横寸法.AutoSize = True
        lbl横寸法.Location = New Point(250, 39)
        lbl横寸法.Name = "lbl横寸法"
        lbl横寸法.Size = New Size(54, 20)
        lbl横寸法.TabIndex = 3
        lbl横寸法.Text = "横寸法"
        ToolTip1.SetToolTip(lbl横寸法, "目標とする横の長さ")
        ' 
        ' nud横寸法
        ' 
        nud横寸法.Location = New Point(211, 66)
        nud横寸法.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        nud横寸法.Name = "nud横寸法"
        nud横寸法.Size = New Size(116, 27)
        nud横寸法.TabIndex = 4
        nud横寸法.TextAlign = HorizontalAlignment.Right
        nud横寸法.ThousandsSeparator = True
        ToolTip1.SetToolTip(nud横寸法, "目標とする横の長さ")
        ' 
        ' nud縦寸法
        ' 
        nud縦寸法.Location = New Point(333, 66)
        nud縦寸法.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        nud縦寸法.Name = "nud縦寸法"
        nud縦寸法.Size = New Size(116, 27)
        nud縦寸法.TabIndex = 7
        nud縦寸法.TextAlign = HorizontalAlignment.Right
        nud縦寸法.ThousandsSeparator = True
        ToolTip1.SetToolTip(nud縦寸法, "目標とする縦の長さ")
        ' 
        ' lbl縦寸法
        ' 
        lbl縦寸法.AutoSize = True
        lbl縦寸法.Location = New Point(372, 39)
        lbl縦寸法.Name = "lbl縦寸法"
        lbl縦寸法.Size = New Size(54, 20)
        lbl縦寸法.TabIndex = 6
        lbl縦寸法.Text = "縦寸法"
        ToolTip1.SetToolTip(lbl縦寸法, "目標とする縦の長さ")
        ' 
        ' nud高さ寸法
        ' 
        nud高さ寸法.Location = New Point(455, 66)
        nud高さ寸法.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        nud高さ寸法.Name = "nud高さ寸法"
        nud高さ寸法.Size = New Size(116, 27)
        nud高さ寸法.TabIndex = 10
        nud高さ寸法.TextAlign = HorizontalAlignment.Right
        nud高さ寸法.ThousandsSeparator = True
        ToolTip1.SetToolTip(nud高さ寸法, "目標とする高さ")
        ' 
        ' lbl高さ寸法
        ' 
        lbl高さ寸法.AutoSize = True
        lbl高さ寸法.Location = New Point(494, 39)
        lbl高さ寸法.Name = "lbl高さ寸法"
        lbl高さ寸法.Size = New Size(64, 20)
        lbl高さ寸法.TabIndex = 9
        lbl高さ寸法.Text = "高さ寸法"
        ToolTip1.SetToolTip(lbl高さ寸法, "目標とする高さ")
        ' 
        ' chk縦の補強ひも
        ' 
        chk縦の補強ひも.AutoSize = True
        chk縦の補強ひも.Location = New Point(32, 136)
        chk縦の補強ひも.Name = "chk縦の補強ひも"
        chk縦の補強ひも.Size = New Size(111, 24)
        chk縦の補強ひも.TabIndex = 6
        chk縦の補強ひも.Text = "縦の補強ひも"
        ToolTip1.SetToolTip(chk縦の補強ひも, "底の縦側に貼るひもを置く場合はチェックON")
        chk縦の補強ひも.UseVisualStyleBackColor = True
        ' 
        ' chk横の補強ひも
        ' 
        chk横の補強ひも.AutoSize = True
        chk横の補強ひも.Location = New Point(26, 131)
        chk横の補強ひも.Name = "chk横の補強ひも"
        chk横の補強ひも.Size = New Size(111, 24)
        chk横の補強ひも.TabIndex = 6
        chk横の補強ひも.Text = "横の補強ひも"
        ToolTip1.SetToolTip(chk横の補強ひも, "底の横側に貼るひもを置く場合はチェックON")
        chk横の補強ひも.UseVisualStyleBackColor = True
        ' 
        ' btn概算
        ' 
        btn概算.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btn概算.Location = New Point(751, 137)
        btn概算.Name = "btn概算"
        btn概算.Size = New Size(111, 46)
        btn概算.TabIndex = 21
        btn概算.Text = "概算(&C)"
        ToolTip1.SetToolTip(btn概算, "目標寸法と基本のひも幅から底の縦横と側面値を計算します")
        btn概算.UseVisualStyleBackColor = True
        ' 
        ' btnひもリスト
        ' 
        btnひもリスト.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnひもリスト.Location = New Point(756, 650)
        btnひもリスト.Name = "btnひもリスト"
        btnひもリスト.Size = New Size(111, 46)
        btnひもリスト.TabIndex = 56
        btnひもリスト.Text = "ひもリスト(&L)"
        ToolTip1.SetToolTip(btnひもリスト, "入力値に基づきひも幅と長さのリストを表示します")
        btnひもリスト.UseVisualStyleBackColor = True
        ' 
        ' lbl垂直ひも数
        ' 
        lbl垂直ひも数.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl垂直ひも数.AutoSize = True
        lbl垂直ひも数.Location = New Point(262, 625)
        lbl垂直ひも数.Name = "lbl垂直ひも数"
        lbl垂直ひも数.Size = New Size(77, 20)
        lbl垂直ひも数.TabIndex = 26
        lbl垂直ひも数.Text = "垂直ひも数"
        ToolTip1.SetToolTip(lbl垂直ひも数, "垂直にたちあげるひも(長い横ひもと縦ひもの合計)")
        ' 
        ' txt垂直ひも数
        ' 
        txt垂直ひも数.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt垂直ひも数.BorderStyle = BorderStyle.FixedSingle
        txt垂直ひも数.Location = New Point(348, 623)
        txt垂直ひも数.Name = "txt垂直ひも数"
        txt垂直ひも数.ReadOnly = True
        txt垂直ひも数.Size = New Size(67, 27)
        txt垂直ひも数.TabIndex = 27
        txt垂直ひも数.TextAlign = HorizontalAlignment.Center
        ToolTip1.SetToolTip(txt垂直ひも数, "垂直にたちあげるひも(長い横ひもと縦ひもの合計")
        ' 
        ' nudひも間のすき間
        ' 
        nudひも間のすき間.DecimalPlaces = 2
        nudひも間のすき間.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        nudひも間のすき間.Location = New Point(573, 45)
        nudひも間のすき間.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        nudひも間のすき間.Name = "nudひも間のすき間"
        nudひも間のすき間.Size = New Size(68, 27)
        nudひも間のすき間.TabIndex = 5
        nudひも間のすき間.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(nudひも間のすき間, "ひもとひものすき間の寸法")
        ' 
        ' lblひも間のすき間
        ' 
        lblひも間のすき間.AutoSize = True
        lblひも間のすき間.Location = New Point(447, 47)
        lblひも間のすき間.Name = "lblひも間のすき間"
        lblひも間のすき間.Size = New Size(97, 20)
        lblひも間のすき間.TabIndex = 4
        lblひも間のすき間.Text = "ひも間のすき間"
        ToolTip1.SetToolTip(lblひも間のすき間, "ひもと横ひものすき間の寸法")
        ' 
        ' nudひも長加算
        ' 
        nudひも長加算.DecimalPlaces = 2
        nudひも長加算.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        nudひも長加算.Location = New Point(573, 131)
        nudひも長加算.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        nudひも長加算.Minimum = New Decimal(New Integer() {99999, 0, 0, Integer.MinValue})
        nudひも長加算.Name = "nudひも長加算"
        nudひも長加算.Size = New Size(68, 27)
        nudひも長加算.TabIndex = 10
        nudひも長加算.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(nudひも長加算, "全てのひもの端に加える長さ(両端で2倍を加えます)")
        ' 
        ' lblひも長加算
        ' 
        lblひも長加算.AutoSize = True
        lblひも長加算.Location = New Point(447, 133)
        lblひも長加算.Name = "lblひも長加算"
        lblひも長加算.Size = New Size(117, 20)
        lblひも長加算.TabIndex = 9
        lblひも長加算.Text = "ひも長加算(一端)"
        ToolTip1.SetToolTip(lblひも長加算, "全てのひもの端に加える長さ")
        ' 
        ' btn規定値
        ' 
        btn規定値.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btn規定値.Location = New Point(634, 137)
        btn規定値.Name = "btn規定値"
        btn規定値.Size = New Size(111, 46)
        btn規定値.TabIndex = 20
        btn規定値.Text = "規定値(&D)"
        ToolTip1.SetToolTip(btn規定値, "登録した規定値にセットします")
        btn規定値.UseVisualStyleBackColor = True
        ' 
        ' btnリセット
        ' 
        btnリセット.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnリセット.Location = New Point(517, 137)
        btnリセット.Name = "btnリセット"
        btnリセット.Size = New Size(111, 46)
        btnリセット.TabIndex = 19
        btnリセット.Text = "リセット(&R)"
        ToolTip1.SetToolTip(btnリセット, "入力した値をクリアします")
        btnリセット.UseVisualStyleBackColor = True
        ' 
        ' txt四角ベース_横
        ' 
        txt四角ベース_横.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt四角ベース_横.Location = New Point(386, 689)
        txt四角ベース_横.Name = "txt四角ベース_横"
        txt四角ベース_横.ReadOnly = True
        txt四角ベース_横.Size = New Size(80, 27)
        txt四角ベース_横.TabIndex = 45
        txt四角ベース_横.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(txt四角ベース_横, "四角(ひも幅+すき間)部分の横の長さ")
        ' 
        ' txt四角ベース_縦
        ' 
        txt四角ベース_縦.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt四角ベース_縦.Location = New Point(472, 689)
        txt四角ベース_縦.Name = "txt四角ベース_縦"
        txt四角ベース_縦.ReadOnly = True
        txt四角ベース_縦.Size = New Size(80, 27)
        txt四角ベース_縦.TabIndex = 48
        txt四角ベース_縦.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(txt四角ベース_縦, "四角(ひも幅+すき間)部分の縦の長さ")
        ' 
        ' txt四角ベース_高さ
        ' 
        txt四角ベース_高さ.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt四角ベース_高さ.Location = New Point(558, 689)
        txt四角ベース_高さ.Name = "txt四角ベース_高さ"
        txt四角ベース_高さ.ReadOnly = True
        txt四角ベース_高さ.Size = New Size(80, 27)
        txt四角ベース_高さ.TabIndex = 51
        txt四角ベース_高さ.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(txt四角ベース_高さ, "四角(ひも幅+すき間)部分の高さ")
        ' 
        ' txt縁厚さプラス_横
        ' 
        txt縁厚さプラス_横.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt縁厚さプラス_横.Location = New Point(386, 720)
        txt縁厚さプラス_横.Name = "txt縁厚さプラス_横"
        txt縁厚さプラス_横.ReadOnly = True
        txt縁厚さプラス_横.Size = New Size(80, 27)
        txt縁厚さプラス_横.TabIndex = 46
        txt縁厚さプラス_横.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(txt縁厚さプラス_横, "四角ベースの長さに厚さを加えた横長")
        ' 
        ' txt縁厚さプラス_縦
        ' 
        txt縁厚さプラス_縦.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt縁厚さプラス_縦.Location = New Point(472, 720)
        txt縁厚さプラス_縦.Name = "txt縁厚さプラス_縦"
        txt縁厚さプラス_縦.ReadOnly = True
        txt縁厚さプラス_縦.Size = New Size(80, 27)
        txt縁厚さプラス_縦.TabIndex = 49
        txt縁厚さプラス_縦.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(txt縁厚さプラス_縦, "四角ベースの長さに厚さを加えた縦長")
        ' 
        ' txt縁厚さプラス_高さ
        ' 
        txt縁厚さプラス_高さ.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt縁厚さプラス_高さ.Location = New Point(558, 720)
        txt縁厚さプラス_高さ.Name = "txt縁厚さプラス_高さ"
        txt縁厚さプラス_高さ.ReadOnly = True
        txt縁厚さプラス_高さ.Size = New Size(80, 27)
        txt縁厚さプラス_高さ.TabIndex = 52
        txt縁厚さプラス_高さ.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(txt縁厚さプラス_高さ, "四角ベースの高さに底厚さと縁を加えた高さ")
        ' 
        ' txt四角ベース_周
        ' 
        txt四角ベース_周.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt四角ベース_周.Location = New Point(644, 689)
        txt四角ベース_周.Name = "txt四角ベース_周"
        txt四角ベース_周.ReadOnly = True
        txt四角ベース_周.Size = New Size(80, 27)
        txt四角ベース_周.TabIndex = 54
        txt四角ベース_周.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(txt四角ベース_周, "四角(ひも幅+すき間)部分の周の長さ")
        ' 
        ' txt縁厚さプラス_周
        ' 
        txt縁厚さプラス_周.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt縁厚さプラス_周.Location = New Point(644, 720)
        txt縁厚さプラス_周.Name = "txt縁厚さプラス_周"
        txt縁厚さプラス_周.ReadOnly = True
        txt縁厚さプラス_周.Size = New Size(80, 27)
        txt縁厚さプラス_周.TabIndex = 55
        txt縁厚さプラス_周.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(txt縁厚さプラス_周, "四角ベースの周に厚さを加えた外周")
        ' 
        ' lbl横ひも
        ' 
        lbl横ひも.AutoSize = True
        lbl横ひも.Location = New Point(26, 87)
        lbl横ひも.Name = "lbl横ひも"
        lbl横ひも.Size = New Size(47, 20)
        lbl横ひも.TabIndex = 3
        lbl横ひも.Text = "横ひも"
        ToolTip1.SetToolTip(lbl横ひも, "横に置かれるひもの本数")
        ' 
        ' nudひも長係数
        ' 
        nudひも長係数.DecimalPlaces = 2
        nudひも長係数.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        nudひも長係数.Location = New Point(573, 88)
        nudひも長係数.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        nudひも長係数.Name = "nudひも長係数"
        nudひも長係数.Size = New Size(68, 27)
        nudひも長係数.TabIndex = 8
        nudひも長係数.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(nudひも長係数, "すべてのひもの計算値に乗算する値")
        nudひも長係数.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' nud高さの四角数
        ' 
        nud高さの四角数.DecimalPlaces = 1
        nud高さの四角数.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        nud高さの四角数.InterceptArrowKeys = False
        nud高さの四角数.Location = New Point(188, 291)
        nud高さの四角数.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        nud高さの四角数.Name = "nud高さの四角数"
        nud高さの四角数.Size = New Size(68, 27)
        nud高さの四角数.TabIndex = 2
        nud高さの四角数.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(nud高さの四角数, "折ってたちあげ側面となる四角(対角線)数")
        ' 
        ' lbl高さの四角数
        ' 
        lbl高さの四角数.AutoSize = True
        lbl高さの四角数.Location = New Point(54, 291)
        lbl高さの四角数.Name = "lbl高さの四角数"
        lbl高さの四角数.Size = New Size(91, 20)
        lbl高さの四角数.TabIndex = 1
        lbl高さの四角数.Text = "高さの四角数"
        ToolTip1.SetToolTip(lbl高さの四角数, "折ってたちあげ側面となる四角数")
        ' 
        ' lbl縦ひも
        ' 
        lbl縦ひも.AutoSize = True
        lbl縦ひも.Location = New Point(32, 90)
        lbl縦ひも.Name = "lbl縦ひも"
        lbl縦ひも.Size = New Size(47, 20)
        lbl縦ひも.TabIndex = 3
        lbl縦ひも.Text = "縦ひも"
        ToolTip1.SetToolTip(lbl縦ひも, "縦に置かれるひもの本数")
        ' 
        ' lbl1つの辺
        ' 
        lbl1つの辺.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl1つの辺.AutoSize = True
        lbl1つの辺.Location = New Point(24, 692)
        lbl1つの辺.Name = "lbl1つの辺"
        lbl1つの辺.Size = New Size(56, 20)
        lbl1つの辺.TabIndex = 34
        lbl1つの辺.Text = "1つの辺"
        ToolTip1.SetToolTip(lbl1つの辺, "ひとつの四角の1辺")
        ' 
        ' lblひも幅
        ' 
        lblひも幅.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblひも幅.AutoSize = True
        lblひも幅.Location = New Point(101, 660)
        lblひも幅.Name = "lblひも幅"
        lblひも幅.Size = New Size(47, 20)
        lblひも幅.TabIndex = 36
        lblひも幅.Text = "ひも幅"
        ToolTip1.SetToolTip(lblひも幅, "基本のひも幅の寸法")
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(447, 90)
        Label10.Name = "Label10"
        Label10.Size = New Size(77, 20)
        Label10.TabIndex = 7
        Label10.Text = "ひも長係数"
        ToolTip1.SetToolTip(Label10, "すべてのひもの計算値に乗算する値")
        ' 
        ' txt縦ひもの本数
        ' 
        txt縦ひもの本数.Location = New Point(158, 87)
        txt縦ひもの本数.Name = "txt縦ひもの本数"
        txt縦ひもの本数.ReadOnly = True
        txt縦ひもの本数.Size = New Size(68, 27)
        txt縦ひもの本数.TabIndex = 4
        ToolTip1.SetToolTip(txt縦ひもの本数, "縦に置かれるひもの本数")
        ' 
        ' txt横ひもの本数
        ' 
        txt横ひもの本数.Location = New Point(160, 84)
        txt横ひもの本数.Name = "txt横ひもの本数"
        txt横ひもの本数.ReadOnly = True
        txt横ひもの本数.Size = New Size(68, 27)
        txt横ひもの本数.TabIndex = 4
        ToolTip1.SetToolTip(txt横ひもの本数, "横に置かれるひもの本数")
        ' 
        ' lbl四角
        ' 
        lbl四角.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl四角.AutoSize = True
        lbl四角.Location = New Point(169, 660)
        lbl四角.Name = "lbl四角"
        lbl四角.Size = New Size(135, 20)
        lbl四角.TabIndex = 39
        lbl四角.Text = "四角(ひも幅+すき間)"
        ToolTip1.SetToolTip(lbl四角, "基本のひも幅にすき間をプラスした、四角の寸法")
        ' 
        ' lbl厚さ
        ' 
        lbl厚さ.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl厚さ.AutoSize = True
        lbl厚さ.Location = New Point(427, 625)
        lbl厚さ.Name = "lbl厚さ"
        lbl厚さ.Size = New Size(34, 20)
        lbl厚さ.TabIndex = 28
        lbl厚さ.Text = "厚さ"
        ToolTip1.SetToolTip(lbl厚さ, "バンドの種類による厚さ・縁の厚さ")
        ' 
        ' txt厚さ
        ' 
        txt厚さ.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt厚さ.BorderStyle = BorderStyle.FixedSingle
        txt厚さ.Location = New Point(472, 623)
        txt厚さ.Name = "txt厚さ"
        txt厚さ.ReadOnly = True
        txt厚さ.Size = New Size(69, 27)
        txt厚さ.TabIndex = 29
        txt厚さ.TextAlign = HorizontalAlignment.Center
        ToolTip1.SetToolTip(txt厚さ, "バンドの種類による厚さ・縁の厚さ")
        ' 
        ' btn画像ファイル
        ' 
        btn画像ファイル.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btn画像ファイル.Location = New Point(606, 361)
        btn画像ファイル.Name = "btn画像ファイル"
        btn画像ファイル.Size = New Size(111, 46)
        btn画像ファイル.TabIndex = 2
        btn画像ファイル.Text = "画像ファイル(&I)"
        ToolTip1.SetToolTip(btn画像ファイル, "生成した画像ファイルを開きます")
        btn画像ファイル.UseVisualStyleBackColor = True
        ' 
        ' btnブラウザ
        ' 
        btnブラウザ.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnブラウザ.Location = New Point(723, 361)
        btnブラウザ.Name = "btnブラウザ"
        btnブラウザ.Size = New Size(111, 46)
        btnブラウザ.TabIndex = 3
        btnブラウザ.Text = "ブラウザ(&B)"
        ToolTip1.SetToolTip(btnブラウザ, "生成した画像ファイルをブラウザで開きます")
        btnブラウザ.UseVisualStyleBackColor = True
        ' 
        ' lbl縦の四角数
        ' 
        lbl縦の四角数.AutoSize = True
        lbl縦の四角数.Location = New Point(32, 44)
        lbl縦の四角数.Name = "lbl縦の四角数"
        lbl縦の四角数.Size = New Size(81, 20)
        lbl縦の四角数.TabIndex = 0
        lbl縦の四角数.Text = "縦の四角数"
        ToolTip1.SetToolTip(lbl縦の四角数, "-45度の側面の四角(対角線)数")
        ' 
        ' nud縦の四角数
        ' 
        nud縦の四角数.Location = New Point(158, 42)
        nud縦の四角数.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        nud縦の四角数.Name = "nud縦の四角数"
        nud縦の四角数.Size = New Size(68, 27)
        nud縦の四角数.TabIndex = 1
        nud縦の四角数.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(nud縦の四角数, "-45度の側面の四角(対角線)数")
        nud縦の四角数.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' lbl横の四角数
        ' 
        lbl横の四角数.AutoSize = True
        lbl横の四角数.Location = New Point(26, 43)
        lbl横の四角数.Name = "lbl横の四角数"
        lbl横の四角数.Size = New Size(81, 20)
        lbl横の四角数.TabIndex = 0
        lbl横の四角数.Text = "横の四角数"
        ToolTip1.SetToolTip(lbl横の四角数, "+45度の側面の四角(対角線)数")
        ' 
        ' nud横の四角数
        ' 
        nud横の四角数.Location = New Point(160, 41)
        nud横の四角数.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        nud横の四角数.Name = "nud横の四角数"
        nud横の四角数.Size = New Size(68, 27)
        nud横の四角数.TabIndex = 1
        nud横の四角数.TextAlign = HorizontalAlignment.Right
        ToolTip1.SetToolTip(nud横の四角数, "+45度の側面の四角(対角線)数")
        nud横の四角数.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' grp目標寸法
        ' 
        grp目標寸法.Controls.Add(rad目標寸法_内側)
        grp目標寸法.Controls.Add(rad目標寸法_外側)
        grp目標寸法.Location = New Point(19, 31)
        grp目標寸法.Name = "grp目標寸法"
        grp目標寸法.Size = New Size(181, 62)
        grp目標寸法.TabIndex = 1
        grp目標寸法.TabStop = False
        grp目標寸法.Text = "目標寸法"
        ToolTip1.SetToolTip(grp目標寸法, "目標とする横・縦・高さ寸法を指定")
        ' 
        ' rad目標寸法_内側
        ' 
        rad目標寸法_内側.AutoSize = True
        rad目標寸法_内側.Location = New Point(113, 23)
        rad目標寸法_内側.Name = "rad目標寸法_内側"
        rad目標寸法_内側.Size = New Size(60, 24)
        rad目標寸法_内側.TabIndex = 1
        rad目標寸法_内側.Text = "内側"
        ToolTip1.SetToolTip(rad目標寸法_内側, "設定寸法より小さくなる四角数を計算")
        rad目標寸法_内側.UseVisualStyleBackColor = True
        ' 
        ' rad目標寸法_外側
        ' 
        rad目標寸法_外側.AutoSize = True
        rad目標寸法_外側.Checked = True
        rad目標寸法_外側.Location = New Point(47, 23)
        rad目標寸法_外側.Name = "rad目標寸法_外側"
        rad目標寸法_外側.Size = New Size(60, 24)
        rad目標寸法_外側.TabIndex = 0
        rad目標寸法_外側.TabStop = True
        rad目標寸法_外側.Text = "外側"
        ToolTip1.SetToolTip(rad目標寸法_外側, "設定寸法より大きくなる四角数を計算")
        rad目標寸法_外側.UseVisualStyleBackColor = True
        ' 
        ' chk縦横を展開する
        ' 
        chk縦横を展開する.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        chk縦横を展開する.AutoSize = True
        chk縦横を展開する.Location = New Point(698, 21)
        chk縦横を展開する.Name = "chk縦横を展開する"
        chk縦横を展開する.Size = New Size(125, 24)
        chk縦横を展開する.TabIndex = 13
        chk縦横を展開する.Text = "縦横を展開する"
        ToolTip1.SetToolTip(chk縦横を展開する, "縦ひも・横ひもを個別に設定したい時にON")
        chk縦横を展開する.UseVisualStyleBackColor = True
        ' 
        ' txt対角線_ひも幅
        ' 
        txt対角線_ひも幅.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt対角線_ひも幅.Location = New Point(86, 720)
        txt対角線_ひも幅.Name = "txt対角線_ひも幅"
        txt対角線_ひも幅.ReadOnly = True
        txt対角線_ひも幅.Size = New Size(80, 27)
        txt対角線_ひも幅.TabIndex = 38
        ToolTip1.SetToolTip(txt対角線_ひも幅, "ひも幅の四角の対角線")
        ' 
        ' txt1つの辺_ひも幅
        ' 
        txt1つの辺_ひも幅.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt1つの辺_ひも幅.Location = New Point(86, 689)
        txt1つの辺_ひも幅.Name = "txt1つの辺_ひも幅"
        txt1つの辺_ひも幅.ReadOnly = True
        txt1つの辺_ひも幅.Size = New Size(80, 27)
        txt1つの辺_ひも幅.TabIndex = 37
        ToolTip1.SetToolTip(txt1つの辺_ひも幅, "ひも幅の四角の一辺")
        ' 
        ' lbl対角線
        ' 
        lbl対角線.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl対角線.AutoSize = True
        lbl対角線.Location = New Point(24, 723)
        lbl対角線.Name = "lbl対角線"
        lbl対角線.Size = New Size(54, 20)
        lbl対角線.TabIndex = 35
        lbl対角線.Text = "対角線"
        ToolTip1.SetToolTip(lbl対角線, "ひとつの四角の対角線")
        ' 
        ' txt対角線_四角
        ' 
        txt対角線_四角.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt対角線_四角.Location = New Point(172, 720)
        txt対角線_四角.Name = "txt対角線_四角"
        txt対角線_四角.ReadOnly = True
        txt対角線_四角.Size = New Size(80, 27)
        txt対角線_四角.TabIndex = 41
        ToolTip1.SetToolTip(txt対角線_四角, "ひも幅にすき間を加えた、単位となるの四角の対角線")
        ' 
        ' txt1つの辺_四角
        ' 
        txt1つの辺_四角.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt1つの辺_四角.Location = New Point(172, 689)
        txt1つの辺_四角.Name = "txt1つの辺_四角"
        txt1つの辺_四角.ReadOnly = True
        txt1つの辺_四角.Size = New Size(80, 27)
        txt1つの辺_四角.TabIndex = 40
        ToolTip1.SetToolTip(txt1つの辺_四角, "ひも幅にすき間を加えた、単位となるの四角の一辺")
        ' 
        ' nud底に
        ' 
        nud底に.Location = New Point(266, 37)
        nud底に.Name = "nud底に"
        nud底に.Size = New Size(46, 27)
        nud底に.TabIndex = 6
        ToolTip1.SetToolTip(nud底に, "底側(辺を含まない)にいくつ連続するか")
        nud底に.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' chk横の辺
        ' 
        chk横の辺.AutoSize = True
        chk横の辺.Location = New Point(8, 38)
        chk横の辺.Name = "chk横の辺"
        chk横の辺.Size = New Size(73, 24)
        chk横の辺.TabIndex = 4
        chk横の辺.Text = "横の辺"
        ToolTip1.SetToolTip(chk横の辺, "左上と右下(横の四角数側)の辺のチェックの状態" & vbCrLf & "右上と左下(縦の四角数側)は逆になります")
        chk横の辺.UseVisualStyleBackColor = True
        ' 
        ' nud垂直に
        ' 
        nud垂直に.Location = New Point(152, 36)
        nud垂直に.Name = "nud垂直に"
        nud垂直に.Size = New Size(46, 27)
        nud垂直に.TabIndex = 3
        ToolTip1.SetToolTip(nud垂直に, "立ち上げる側(辺を含む)にいくつ連続するか")
        nud垂直に.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' btn合わせる
        ' 
        btn合わせる.Location = New Point(341, 25)
        btn合わせる.Name = "btn合わせる"
        btn合わせる.Size = New Size(111, 46)
        btn合わせる.TabIndex = 2
        btn合わせる.Text = "合わせる(&I)"
        ToolTip1.SetToolTip(btn合わせる, "編集サイズを現在の四角数(ひも数)に合わせます")
        btn合わせる.UseVisualStyleBackColor = True
        ' 
        ' txtメモ
        ' 
        txtメモ.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtメモ.Location = New Point(22, 147)
        txtメモ.Multiline = True
        txtメモ.Name = "txtメモ"
        txtメモ.Size = New Size(794, 246)
        txtメモ.TabIndex = 5
        ToolTip1.SetToolTip(txtメモ, "自由に記述できます")
        ' 
        ' txt作成者
        ' 
        txt作成者.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txt作成者.Location = New Point(126, 81)
        txt作成者.Multiline = True
        txt作成者.Name = "txt作成者"
        txt作成者.Size = New Size(690, 49)
        txt作成者.TabIndex = 3
        ToolTip1.SetToolTip(txt作成者, "作成者情報")
        ' 
        ' txtタイトル
        ' 
        txtタイトル.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtタイトル.Location = New Point(126, 17)
        txtタイトル.Multiline = True
        txtタイトル.Name = "txtタイトル"
        txtタイトル.Size = New Size(690, 49)
        txtタイトル.TabIndex = 1
        ToolTip1.SetToolTip(txtタイトル, "タイトル情報")
        ' 
        ' lbl本幅の差
        ' 
        lbl本幅の差.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl本幅の差.AutoSize = True
        lbl本幅の差.Location = New Point(557, 627)
        lbl本幅の差.Name = "lbl本幅の差"
        lbl本幅の差.Size = New Size(66, 20)
        lbl本幅の差.TabIndex = 30
        lbl本幅の差.Text = "本幅の差"
        ToolTip1.SetToolTip(lbl本幅の差, "縦ひも・横ひもの本幅合計値の差")
        lbl本幅の差.Visible = False
        ' 
        ' txt先の三角形の本幅の差
        ' 
        txt先の三角形の本幅の差.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt先の三角形の本幅の差.BorderStyle = BorderStyle.FixedSingle
        txt先の三角形の本幅の差.Location = New Point(632, 623)
        txt先の三角形の本幅の差.Name = "txt先の三角形の本幅の差"
        txt先の三角形の本幅の差.ReadOnly = True
        txt先の三角形の本幅の差.Size = New Size(35, 27)
        txt先の三角形の本幅の差.TabIndex = 31
        txt先の三角形の本幅の差.TextAlign = HorizontalAlignment.Center
        ToolTip1.SetToolTip(txt先の三角形の本幅の差, "先の三角形の本幅の差")
        txt先の三角形の本幅の差.Visible = False
        ' 
        ' txt四辺形の本幅の差
        ' 
        txt四辺形の本幅の差.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt四辺形の本幅の差.BorderStyle = BorderStyle.FixedSingle
        txt四辺形の本幅の差.Location = New Point(673, 623)
        txt四辺形の本幅の差.Name = "txt四辺形の本幅の差"
        txt四辺形の本幅の差.ReadOnly = True
        txt四辺形の本幅の差.Size = New Size(35, 27)
        txt四辺形の本幅の差.TabIndex = 32
        txt四辺形の本幅の差.TextAlign = HorizontalAlignment.Center
        ToolTip1.SetToolTip(txt四辺形の本幅の差, "四辺形部分の本幅の差")
        txt四辺形の本幅の差.Visible = False
        ' 
        ' txt後の三角形の本幅の差
        ' 
        txt後の三角形の本幅の差.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txt後の三角形の本幅の差.BorderStyle = BorderStyle.FixedSingle
        txt後の三角形の本幅の差.Location = New Point(714, 623)
        txt後の三角形の本幅の差.Name = "txt後の三角形の本幅の差"
        txt後の三角形の本幅の差.ReadOnly = True
        txt後の三角形の本幅の差.Size = New Size(35, 27)
        txt後の三角形の本幅の差.TabIndex = 33
        txt後の三角形の本幅の差.TextAlign = HorizontalAlignment.Center
        ToolTip1.SetToolTip(txt後の三角形の本幅の差, "後の三角形の本幅の差")
        txt後の三角形の本幅の差.Visible = False
        ' 
        ' btn画像ファイル2
        ' 
        btn画像ファイル2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btn画像ファイル2.Location = New Point(606, 361)
        btn画像ファイル2.Name = "btn画像ファイル2"
        btn画像ファイル2.Size = New Size(111, 46)
        btn画像ファイル2.TabIndex = 2
        btn画像ファイル2.Text = "画像ファイル(&I)"
        ToolTip1.SetToolTip(btn画像ファイル2, "生成した画像ファイルを開きます")
        btn画像ファイル2.UseVisualStyleBackColor = True
        ' 
        ' btnブラウザ2
        ' 
        btnブラウザ2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnブラウザ2.Location = New Point(723, 361)
        btnブラウザ2.Name = "btnブラウザ2"
        btnブラウザ2.Size = New Size(111, 46)
        btnブラウザ2.TabIndex = 3
        btnブラウザ2.Text = "ブラウザ(&B)"
        ToolTip1.SetToolTip(btnブラウザ2, "生成した画像ファイルをブラウザで開きます")
        btnブラウザ2.UseVisualStyleBackColor = True
        ' 
        ' btn3Dモデル
        ' 
        btn3Dモデル.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btn3Dモデル.Location = New Point(489, 361)
        btn3Dモデル.Name = "btn3Dモデル"
        btn3Dモデル.Size = New Size(111, 46)
        btn3Dモデル.TabIndex = 1
        btn3Dモデル.Text = "3Dモデル(&M)"
        ToolTip1.SetToolTip(btn3Dモデル, "生成した画像ファイルを開きます")
        btn3Dモデル.UseVisualStyleBackColor = True
        ' 
        ' radビューア
        ' 
        radビューア.AutoSize = True
        radビューア.Checked = True
        radビューア.Location = New Point(29, 19)
        radビューア.Name = "radビューア"
        radビューア.Size = New Size(73, 24)
        radビューア.TabIndex = 0
        radビューア.TabStop = True
        radビューア.Text = "ビューア"
        ToolTip1.SetToolTip(radビューア, "3Dファイルを標準ビューアで開きます")
        radビューア.UseVisualStyleBackColor = True
        ' 
        ' radファイル
        ' 
        radファイル.AutoSize = True
        radファイル.Location = New Point(114, 19)
        radファイル.Name = "radファイル"
        radファイル.Size = New Size(72, 24)
        radファイル.TabIndex = 1
        radファイル.Text = "ファイル"
        ToolTip1.SetToolTip(radファイル, "3Dファイルを圧縮ファイルにします")
        radファイル.UseVisualStyleBackColor = True
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.ImageScalingSize = New Size(20, 20)
        MenuStrip1.Items.AddRange(New ToolStripItem() {ToolStripMenuItemFile, ToolStripMenuItemEdit, ToolStripMenuItemSetting, ToolStripMenuItemHelp})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(886, 28)
        MenuStrip1.TabIndex = 0
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' ToolStripMenuItemFile
        ' 
        ToolStripMenuItemFile.DropDownItems.AddRange(New ToolStripItem() {ToolStripMenuItemFileNew, ToolStripMenuItemFileOpen, ToolStripMenuItemFileSaveAs, ToolStripMenuItemFileSave, ToolStripSeparator2, ToolStripMenuItemFileAbort, ToolStripMenuItemFileExit})
        ToolStripMenuItemFile.Name = "ToolStripMenuItemFile"
        ToolStripMenuItemFile.Size = New Size(82, 24)
        ToolStripMenuItemFile.Text = "ファイル(&F)"
        ' 
        ' ToolStripMenuItemFileNew
        ' 
        ToolStripMenuItemFileNew.Name = "ToolStripMenuItemFileNew"
        ToolStripMenuItemFileNew.Size = New Size(218, 26)
        ToolStripMenuItemFileNew.Text = "新規作成(&N)"
        ' 
        ' ToolStripMenuItemFileOpen
        ' 
        ToolStripMenuItemFileOpen.Name = "ToolStripMenuItemFileOpen"
        ToolStripMenuItemFileOpen.Size = New Size(218, 26)
        ToolStripMenuItemFileOpen.Text = "開く(&O)"
        ' 
        ' ToolStripMenuItemFileSaveAs
        ' 
        ToolStripMenuItemFileSaveAs.Name = "ToolStripMenuItemFileSaveAs"
        ToolStripMenuItemFileSaveAs.Size = New Size(218, 26)
        ToolStripMenuItemFileSaveAs.Text = "名前をつけて保存(&A)"
        ' 
        ' ToolStripMenuItemFileSave
        ' 
        ToolStripMenuItemFileSave.Name = "ToolStripMenuItemFileSave"
        ToolStripMenuItemFileSave.Size = New Size(218, 26)
        ToolStripMenuItemFileSave.Text = "保存(&S)"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(215, 6)
        ' 
        ' ToolStripMenuItemFileAbort
        ' 
        ToolStripMenuItemFileAbort.Name = "ToolStripMenuItemFileAbort"
        ToolStripMenuItemFileAbort.Size = New Size(218, 26)
        ToolStripMenuItemFileAbort.Text = "中止(&C)"
        ' 
        ' ToolStripMenuItemFileExit
        ' 
        ToolStripMenuItemFileExit.Name = "ToolStripMenuItemFileExit"
        ToolStripMenuItemFileExit.Size = New Size(218, 26)
        ToolStripMenuItemFileExit.Text = "終了(&X)"
        ' 
        ' ToolStripMenuItemEdit
        ' 
        ToolStripMenuItemEdit.DropDownItems.AddRange(New ToolStripItem() {ToolStripMenuItemEditSelectBand, ToolStripSeparator4, ToolStripMenuItemEditReset, ToolStripMenuItemEditDefault, ToolStripMenuItemEditCalc, ToolStripSeparator5, ToolStripMenuItemEditColorChange, ToolStripMenuItemEditColorRepeat, ToolStripSeparator1, ToolStripMenuItemEditList, ToolStripMenuItemEditDefaultFile})
        ToolStripMenuItemEdit.Name = "ToolStripMenuItemEdit"
        ToolStripMenuItemEdit.Size = New Size(71, 24)
        ToolStripMenuItemEdit.Text = "編集(&E)"
        ' 
        ' ToolStripMenuItemEditSelectBand
        ' 
        ToolStripMenuItemEditSelectBand.Name = "ToolStripMenuItemEditSelectBand"
        ToolStripMenuItemEditSelectBand.Size = New Size(216, 26)
        ToolStripMenuItemEditSelectBand.Text = "バンドの種類選択(&S)"
        ' 
        ' ToolStripSeparator4
        ' 
        ToolStripSeparator4.Name = "ToolStripSeparator4"
        ToolStripSeparator4.Size = New Size(213, 6)
        ' 
        ' ToolStripMenuItemEditReset
        ' 
        ToolStripMenuItemEditReset.Name = "ToolStripMenuItemEditReset"
        ToolStripMenuItemEditReset.Size = New Size(216, 26)
        ToolStripMenuItemEditReset.Text = "リセット(&R)"
        ' 
        ' ToolStripMenuItemEditDefault
        ' 
        ToolStripMenuItemEditDefault.Name = "ToolStripMenuItemEditDefault"
        ToolStripMenuItemEditDefault.Size = New Size(216, 26)
        ToolStripMenuItemEditDefault.Text = "規定値(&D)"
        ' 
        ' ToolStripMenuItemEditCalc
        ' 
        ToolStripMenuItemEditCalc.Name = "ToolStripMenuItemEditCalc"
        ToolStripMenuItemEditCalc.Size = New Size(216, 26)
        ToolStripMenuItemEditCalc.Text = "概算(&C)"
        ' 
        ' ToolStripSeparator5
        ' 
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New Size(213, 6)
        ' 
        ' ToolStripMenuItemEditColorChange
        ' 
        ToolStripMenuItemEditColorChange.Name = "ToolStripMenuItemEditColorChange"
        ToolStripMenuItemEditColorChange.Size = New Size(216, 26)
        ToolStripMenuItemEditColorChange.Text = "色の変更(&H)"
        ' 
        ' ToolStripMenuItemEditColorRepeat
        ' 
        ToolStripMenuItemEditColorRepeat.Name = "ToolStripMenuItemEditColorRepeat"
        ToolStripMenuItemEditColorRepeat.Size = New Size(216, 26)
        ToolStripMenuItemEditColorRepeat.Text = "色の繰り返し(&E)"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(213, 6)
        ' 
        ' ToolStripMenuItemEditList
        ' 
        ToolStripMenuItemEditList.Name = "ToolStripMenuItemEditList"
        ToolStripMenuItemEditList.Size = New Size(216, 26)
        ToolStripMenuItemEditList.Text = "ひもリスト(&L)"
        ' 
        ' ToolStripMenuItemEditDefaultFile
        ' 
        ToolStripMenuItemEditDefaultFile.Name = "ToolStripMenuItemEditDefaultFile"
        ToolStripMenuItemEditDefaultFile.Size = New Size(216, 26)
        ToolStripMenuItemEditDefaultFile.Text = "規定値保存(&F)"
        ' 
        ' ToolStripMenuItemSetting
        ' 
        ToolStripMenuItemSetting.DropDownItems.AddRange(New ToolStripItem() {ToolStripMenuItemSettingBandType, ToolStripMenuItemSettingPattern, ToolStripMenuItemSettingOptions, ToolStripMenuItemSettingColor, ToolStripMenuItemSettingUpDown, ToolStripSeparator3, ToolStripMenuItemSettingBasics})
        ToolStripMenuItemSetting.Name = "ToolStripMenuItemSetting"
        ToolStripMenuItemSetting.Size = New Size(71, 24)
        ToolStripMenuItemSetting.Text = "設定(&S)"
        ' 
        ' ToolStripMenuItemSettingBandType
        ' 
        ToolStripMenuItemSettingBandType.Name = "ToolStripMenuItemSettingBandType"
        ToolStripMenuItemSettingBandType.Size = New Size(186, 26)
        ToolStripMenuItemSettingBandType.Text = "バンドの種類(&T)"
        ' 
        ' ToolStripMenuItemSettingPattern
        ' 
        ToolStripMenuItemSettingPattern.Name = "ToolStripMenuItemSettingPattern"
        ToolStripMenuItemSettingPattern.Size = New Size(186, 26)
        ToolStripMenuItemSettingPattern.Text = "編みかた(&P)"
        ' 
        ' ToolStripMenuItemSettingOptions
        ' 
        ToolStripMenuItemSettingOptions.Name = "ToolStripMenuItemSettingOptions"
        ToolStripMenuItemSettingOptions.Size = New Size(186, 26)
        ToolStripMenuItemSettingOptions.Text = "付属品(&O)"
        ' 
        ' ToolStripMenuItemSettingColor
        ' 
        ToolStripMenuItemSettingColor.Name = "ToolStripMenuItemSettingColor"
        ToolStripMenuItemSettingColor.Size = New Size(186, 26)
        ToolStripMenuItemSettingColor.Text = "描画色(&C)"
        ' 
        ' ToolStripMenuItemSettingUpDown
        ' 
        ToolStripMenuItemSettingUpDown.Name = "ToolStripMenuItemSettingUpDown"
        ToolStripMenuItemSettingUpDown.Size = New Size(186, 26)
        ToolStripMenuItemSettingUpDown.Text = "上下図(&U)"
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(183, 6)
        ' 
        ' ToolStripMenuItemSettingBasics
        ' 
        ToolStripMenuItemSettingBasics.Name = "ToolStripMenuItemSettingBasics"
        ToolStripMenuItemSettingBasics.Size = New Size(186, 26)
        ToolStripMenuItemSettingBasics.Text = "基本設定(&B)"
        ' 
        ' ToolStripMenuItemHelp
        ' 
        ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp"
        ToolStripMenuItemHelp.Size = New Size(79, 24)
        ToolStripMenuItemHelp.Text = "ヘルプ(&H)"
        ' 
        ' lbl基本のひも幅_単位
        ' 
        lbl基本のひも幅_単位.AutoSize = True
        lbl基本のひも幅_単位.Location = New Point(690, 73)
        lbl基本のひも幅_単位.Name = "lbl基本のひも幅_単位"
        lbl基本のひも幅_単位.Size = New Size(39, 20)
        lbl基本のひも幅_単位.TabIndex = 16
        lbl基本のひも幅_単位.Text = "本幅"
        ' 
        ' lbl目標寸法_単位
        ' 
        lbl目標寸法_単位.AutoSize = True
        lbl目標寸法_単位.Location = New Point(576, 73)
        lbl目標寸法_単位.Name = "lbl目標寸法_単位"
        lbl目標寸法_単位.Size = New Size(35, 20)
        lbl目標寸法_単位.TabIndex = 12
        lbl目標寸法_単位.Text = "mm"
        ' 
        ' TabControl
        ' 
        TabControl.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TabControl.Controls.Add(tpage四角数)
        TabControl.Controls.Add(tpage縁の始末)
        TabControl.Controls.Add(tpage追加品)
        TabControl.Controls.Add(tpageメモ他)
        TabControl.Controls.Add(tpage横ひも)
        TabControl.Controls.Add(tpage縦ひも)
        TabControl.Controls.Add(tpageプレビュー)
        TabControl.Controls.Add(tpageプレビュー2)
        TabControl.Controls.Add(tpageひも上下)
        TabControl.Location = New Point(19, 171)
        TabControl.Name = "TabControl"
        TabControl.SelectedIndex = 0
        TabControl.Size = New Size(848, 446)
        TabControl.TabIndex = 0
        ' 
        ' tpage四角数
        ' 
        tpage四角数.Controls.Add(lbl高さの四角数_単位)
        tpage四角数.Controls.Add(nud高さの四角数)
        tpage四角数.Controls.Add(lbl高さの四角数)
        tpage四角数.Controls.Add(nudひも長係数)
        tpage四角数.Controls.Add(Label10)
        tpage四角数.Controls.Add(chk縦横を展開する)
        tpage四角数.Controls.Add(nudひも長加算)
        tpage四角数.Controls.Add(lblひも長加算_単位)
        tpage四角数.Controls.Add(nudひも間のすき間)
        tpage四角数.Controls.Add(lblひも間のすき間_単位)
        tpage四角数.Controls.Add(lblひも長加算)
        tpage四角数.Controls.Add(lblひも間のすき間)
        tpage四角数.Controls.Add(grp縦置き)
        tpage四角数.Controls.Add(grp横置き)
        tpage四角数.Location = New Point(4, 29)
        tpage四角数.Name = "tpage四角数"
        tpage四角数.Padding = New Padding(3)
        tpage四角数.Size = New Size(840, 413)
        tpage四角数.TabIndex = 0
        tpage四角数.Text = "四角数"
        tpage四角数.UseVisualStyleBackColor = True
        ' 
        ' lbl高さの四角数_単位
        ' 
        lbl高さの四角数_単位.AutoSize = True
        lbl高さの四角数_単位.Location = New Point(268, 293)
        lbl高さの四角数_単位.Name = "lbl高さの四角数_単位"
        lbl高さの四角数_単位.Size = New Size(24, 20)
        lbl高さの四角数_単位.TabIndex = 3
        lbl高さの四角数_単位.Text = "個"
        ' 
        ' lblひも長加算_単位
        ' 
        lblひも長加算_単位.AutoSize = True
        lblひも長加算_単位.Location = New Point(657, 133)
        lblひも長加算_単位.Name = "lblひも長加算_単位"
        lblひも長加算_単位.Size = New Size(35, 20)
        lblひも長加算_単位.TabIndex = 11
        lblひも長加算_単位.Text = "mm"
        ' 
        ' lblひも間のすき間_単位
        ' 
        lblひも間のすき間_単位.AutoSize = True
        lblひも間のすき間_単位.Location = New Point(657, 47)
        lblひも間のすき間_単位.Name = "lblひも間のすき間_単位"
        lblひも間のすき間_単位.Size = New Size(35, 20)
        lblひも間のすき間_単位.TabIndex = 6
        lblひも間のすき間_単位.Text = "mm"
        ' 
        ' grp縦置き
        ' 
        grp縦置き.Controls.Add(txt縦ひもの本数)
        grp縦置き.Controls.Add(lbl縦ひもの本数_単位)
        grp縦置き.Controls.Add(lbl縦ひも)
        grp縦置き.Controls.Add(lbl縦ひものメモ)
        grp縦置き.Controls.Add(txt縦ひものメモ)
        grp縦置き.Controls.Add(chk縦の補強ひも)
        grp縦置き.Controls.Add(lbl縦の四角数_単位)
        grp縦置き.Controls.Add(lbl縦の四角数)
        grp縦置き.Controls.Add(nud縦の四角数)
        grp縦置き.Location = New Point(415, 178)
        grp縦置き.Name = "grp縦置き"
        grp縦置き.Size = New Size(381, 229)
        grp縦置き.TabIndex = 12
        grp縦置き.TabStop = False
        grp縦置き.Text = "縦置き"
        ' 
        ' lbl縦ひもの本数_単位
        ' 
        lbl縦ひもの本数_単位.AutoSize = True
        lbl縦ひもの本数_単位.Location = New Point(242, 90)
        lbl縦ひもの本数_単位.Name = "lbl縦ひもの本数_単位"
        lbl縦ひもの本数_単位.Size = New Size(24, 20)
        lbl縦ひもの本数_単位.TabIndex = 5
        lbl縦ひもの本数_単位.Text = "本"
        ' 
        ' lbl縦ひものメモ
        ' 
        lbl縦ひものメモ.AutoSize = True
        lbl縦ひものメモ.Location = New Point(32, 186)
        lbl縦ひものメモ.Name = "lbl縦ひものメモ"
        lbl縦ひものメモ.Size = New Size(81, 20)
        lbl縦ひものメモ.TabIndex = 7
        lbl縦ひものメモ.Text = "縦ひものメモ"
        ' 
        ' txt縦ひものメモ
        ' 
        txt縦ひものメモ.Location = New Point(158, 183)
        txt縦ひものメモ.Name = "txt縦ひものメモ"
        txt縦ひものメモ.Size = New Size(201, 27)
        txt縦ひものメモ.TabIndex = 8
        ' 
        ' lbl縦の四角数_単位
        ' 
        lbl縦の四角数_単位.AutoSize = True
        lbl縦の四角数_単位.Location = New Point(242, 44)
        lbl縦の四角数_単位.Name = "lbl縦の四角数_単位"
        lbl縦の四角数_単位.Size = New Size(24, 20)
        lbl縦の四角数_単位.TabIndex = 2
        lbl縦の四角数_単位.Text = "個"
        ' 
        ' grp横置き
        ' 
        grp横置き.Controls.Add(txt横ひもの本数)
        grp横置き.Controls.Add(lbl横ひもの本数_単位)
        grp横置き.Controls.Add(lbl横ひも)
        grp横置き.Controls.Add(lbl横ひものメモ)
        grp横置き.Controls.Add(txt横ひものメモ)
        grp横置き.Controls.Add(chk横の補強ひも)
        grp横置き.Controls.Add(lbl横の四角数_単位)
        grp横置き.Controls.Add(lbl横の四角数)
        grp横置き.Controls.Add(nud横の四角数)
        grp横置き.Location = New Point(28, 21)
        grp横置き.Name = "grp横置き"
        grp横置き.Size = New Size(381, 229)
        grp横置き.TabIndex = 0
        grp横置き.TabStop = False
        grp横置き.Text = "横置き"
        ' 
        ' lbl横ひもの本数_単位
        ' 
        lbl横ひもの本数_単位.AutoSize = True
        lbl横ひもの本数_単位.Location = New Point(240, 87)
        lbl横ひもの本数_単位.Name = "lbl横ひもの本数_単位"
        lbl横ひもの本数_単位.Size = New Size(24, 20)
        lbl横ひもの本数_単位.TabIndex = 5
        lbl横ひもの本数_単位.Text = "本"
        ' 
        ' lbl横ひものメモ
        ' 
        lbl横ひものメモ.AutoSize = True
        lbl横ひものメモ.Location = New Point(26, 183)
        lbl横ひものメモ.Name = "lbl横ひものメモ"
        lbl横ひものメモ.Size = New Size(81, 20)
        lbl横ひものメモ.TabIndex = 7
        lbl横ひものメモ.Text = "横ひものメモ"
        ' 
        ' txt横ひものメモ
        ' 
        txt横ひものメモ.Location = New Point(160, 180)
        txt横ひものメモ.Name = "txt横ひものメモ"
        txt横ひものメモ.Size = New Size(201, 27)
        txt横ひものメモ.TabIndex = 8
        ' 
        ' lbl横の四角数_単位
        ' 
        lbl横の四角数_単位.AutoSize = True
        lbl横の四角数_単位.Location = New Point(240, 43)
        lbl横の四角数_単位.Name = "lbl横の四角数_単位"
        lbl横の四角数_単位.Size = New Size(24, 20)
        lbl横の四角数_単位.TabIndex = 2
        lbl横の四角数_単位.Text = "個"
        ' 
        ' tpage縁の始末
        ' 
        tpage縁の始末.Controls.Add(dgv縁の始末)
        tpage縁の始末.Controls.Add(lbl編みかた名_側面)
        tpage縁の始末.Controls.Add(btn削除_側面)
        tpage縁の始末.Controls.Add(btn追加_側面)
        tpage縁の始末.Controls.Add(cmb編みかた名_側面)
        tpage縁の始末.Location = New Point(4, 29)
        tpage縁の始末.Name = "tpage縁の始末"
        tpage縁の始末.Padding = New Padding(3)
        tpage縁の始末.Size = New Size(840, 413)
        tpage縁の始末.TabIndex = 2
        tpage縁の始末.Text = "縁の始末"
        tpage縁の始末.UseVisualStyleBackColor = True
        ' 
        ' dgv縁の始末
        ' 
        dgv縁の始末.AllowUserToAddRows = False
        dgv縁の始末.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgv縁の始末.AutoGenerateColumns = False
        dgv縁の始末.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgv縁の始末.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgv縁の始末.Columns.AddRange(New DataGridViewColumn() {f_i番号, f_s編みかた名, f_s編みひも名, f_iひも番号, f_i何本幅, f_b集計対象外区分, f_s色, f_d高さ, f_d垂直ひも長, f_d周長比率対底の周, f_d周長, f_dひも長, f_dひも長加算, f_iひも本数, f_d厚さ, f_sメモ, f_d連続ひも長, f_s記号, f_bError})
        dgv縁の始末.DataSource = BindingSource縁の始末
        dgv縁の始末.Location = New Point(6, 6)
        dgv縁の始末.Name = "dgv縁の始末"
        dgv縁の始末.RowHeadersWidth = 51
        dgv縁の始末.RowTemplate.Height = 29
        dgv縁の始末.Size = New Size(826, 345)
        dgv縁の始末.TabIndex = 0
        ' 
        ' lbl編みかた名_側面
        ' 
        lbl編みかた名_側面.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl編みかた名_側面.AutoSize = True
        lbl編みかた名_側面.Location = New Point(432, 370)
        lbl編みかた名_側面.Name = "lbl編みかた名_側面"
        lbl編みかた名_側面.Size = New Size(76, 20)
        lbl編みかた名_側面.TabIndex = 2
        lbl編みかた名_側面.Text = "編みかた名"
        ' 
        ' btn削除_側面
        ' 
        btn削除_側面.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btn削除_側面.Location = New Point(6, 357)
        btn削除_側面.Name = "btn削除_側面"
        btn削除_側面.Size = New Size(111, 46)
        btn削除_側面.TabIndex = 1
        btn削除_側面.Text = "削除(&R)"
        btn削除_側面.UseVisualStyleBackColor = True
        ' 
        ' btn追加_側面
        ' 
        btn追加_側面.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btn追加_側面.Location = New Point(721, 357)
        btn追加_側面.Name = "btn追加_側面"
        btn追加_側面.Size = New Size(111, 46)
        btn追加_側面.TabIndex = 4
        btn追加_側面.Text = "追加(&A)"
        btn追加_側面.UseVisualStyleBackColor = True
        ' 
        ' cmb編みかた名_側面
        ' 
        cmb編みかた名_側面.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        cmb編みかた名_側面.FormattingEnabled = True
        cmb編みかた名_側面.Location = New Point(522, 367)
        cmb編みかた名_側面.Name = "cmb編みかた名_側面"
        cmb編みかた名_側面.Size = New Size(184, 28)
        cmb編みかた名_側面.TabIndex = 3
        ' 
        ' tpage追加品
        ' 
        tpage追加品.Controls.Add(editAddParts)
        tpage追加品.Location = New Point(4, 29)
        tpage追加品.Name = "tpage追加品"
        tpage追加品.Padding = New Padding(3)
        tpage追加品.Size = New Size(840, 413)
        tpage追加品.TabIndex = 3
        tpage追加品.Text = "追加品"
        tpage追加品.UseVisualStyleBackColor = True
        ' 
        ' editAddParts
        ' 
        editAddParts.AutoSize = True
        editAddParts.AutoSizeMode = AutoSizeMode.GrowAndShrink
        editAddParts.Location = New Point(-3, -3)
        editAddParts.Name = "editAddParts"
        editAddParts.PanelSize = New Size(840, 413)
        editAddParts.Size = New Size(846, 419)
        editAddParts.TabIndex = 1
        ' 
        ' tpageメモ他
        ' 
        tpageメモ他.Controls.Add(txt作成者)
        tpageメモ他.Controls.Add(txtタイトル)
        tpageメモ他.Controls.Add(lbl作成者)
        tpageメモ他.Controls.Add(lblタイトル)
        tpageメモ他.Controls.Add(lblメモ)
        tpageメモ他.Controls.Add(txtメモ)
        tpageメモ他.Location = New Point(4, 29)
        tpageメモ他.Name = "tpageメモ他"
        tpageメモ他.Padding = New Padding(3)
        tpageメモ他.Size = New Size(840, 413)
        tpageメモ他.TabIndex = 4
        tpageメモ他.Text = "メモ他"
        tpageメモ他.UseVisualStyleBackColor = True
        ' 
        ' lbl作成者
        ' 
        lbl作成者.AutoSize = True
        lbl作成者.Location = New Point(24, 81)
        lbl作成者.Name = "lbl作成者"
        lbl作成者.Size = New Size(54, 20)
        lbl作成者.TabIndex = 2
        lbl作成者.Text = "作成者"
        ' 
        ' lblタイトル
        ' 
        lblタイトル.AutoSize = True
        lblタイトル.Location = New Point(24, 17)
        lblタイトル.Name = "lblタイトル"
        lblタイトル.Size = New Size(53, 20)
        lblタイトル.TabIndex = 0
        lblタイトル.Text = "タイトル"
        ' 
        ' lblメモ
        ' 
        lblメモ.AutoSize = True
        lblメモ.Location = New Point(24, 119)
        lblメモ.Name = "lblメモ"
        lblメモ.Size = New Size(31, 20)
        lblメモ.TabIndex = 4
        lblメモ.Text = "メモ"
        ' 
        ' tpage横ひも
        ' 
        tpage横ひも.Controls.Add(expand横ひも)
        tpage横ひも.Location = New Point(4, 29)
        tpage横ひも.Name = "tpage横ひも"
        tpage横ひも.Padding = New Padding(3)
        tpage横ひも.Size = New Size(840, 413)
        tpage横ひも.TabIndex = 5
        tpage横ひも.Text = "横ひも"
        tpage横ひも.UseVisualStyleBackColor = True
        ' 
        ' expand横ひも
        ' 
        expand横ひも.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        expand横ひも.AutoSizeMode = AutoSizeMode.GrowAndShrink
        expand横ひも.Location = New Point(-3, -3)
        expand横ひも.Name = "expand横ひも"
        expand横ひも.PanelSize = New Size(840, 413)
        expand横ひも.Size = New Size(843, 413)
        expand横ひも.TabIndex = 0
        ' 
        ' tpage縦ひも
        ' 
        tpage縦ひも.Controls.Add(expand縦ひも)
        tpage縦ひも.Location = New Point(4, 29)
        tpage縦ひも.Name = "tpage縦ひも"
        tpage縦ひも.Padding = New Padding(3)
        tpage縦ひも.Size = New Size(840, 413)
        tpage縦ひも.TabIndex = 6
        tpage縦ひも.Text = "縦ひも"
        tpage縦ひも.UseVisualStyleBackColor = True
        ' 
        ' expand縦ひも
        ' 
        expand縦ひも.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        expand縦ひも.AutoSizeMode = AutoSizeMode.GrowAndShrink
        expand縦ひも.Location = New Point(-3, -3)
        expand縦ひも.Name = "expand縦ひも"
        expand縦ひも.PanelSize = New Size(840, 413)
        expand縦ひも.Size = New Size(843, 416)
        expand縦ひも.TabIndex = 0
        ' 
        ' tpageプレビュー
        ' 
        tpageプレビュー.Controls.Add(radうら)
        tpageプレビュー.Controls.Add(radおもて)
        tpageプレビュー.Controls.Add(btn画像ファイル)
        tpageプレビュー.Controls.Add(btnブラウザ)
        tpageプレビュー.Controls.Add(picプレビュー)
        tpageプレビュー.Location = New Point(4, 29)
        tpageプレビュー.Name = "tpageプレビュー"
        tpageプレビュー.Padding = New Padding(3)
        tpageプレビュー.Size = New Size(840, 413)
        tpageプレビュー.TabIndex = 7
        tpageプレビュー.Text = "プレビュー"
        tpageプレビュー.UseVisualStyleBackColor = True
        ' 
        ' radうら
        ' 
        radうら.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        radうら.AutoSize = True
        radうら.Location = New Point(109, 372)
        radうら.Name = "radうら"
        radうら.Size = New Size(51, 24)
        radうら.TabIndex = 1
        radうら.Text = "うら"
        radうら.UseVisualStyleBackColor = True
        ' 
        ' radおもて
        ' 
        radおもて.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        radおもて.AutoSize = True
        radおもて.Checked = True
        radおもて.Location = New Point(23, 372)
        radおもて.Name = "radおもて"
        radおもて.Size = New Size(64, 24)
        radおもて.TabIndex = 0
        radおもて.TabStop = True
        radおもて.Text = "おもて"
        radおもて.UseVisualStyleBackColor = True
        ' 
        ' picプレビュー
        ' 
        picプレビュー.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        picプレビュー.Location = New Point(6, 6)
        picプレビュー.Name = "picプレビュー"
        picプレビュー.Size = New Size(828, 349)
        picプレビュー.SizeMode = PictureBoxSizeMode.Zoom
        picプレビュー.TabIndex = 0
        picプレビュー.TabStop = False
        ' 
        ' tpageひも上下
        ' 
        tpageひも上下.Controls.Add(grp縦横の四角)
        tpageひも上下.Controls.Add(editUpDown)
        tpageひも上下.Location = New Point(4, 29)
        tpageひも上下.Name = "tpageひも上下"
        tpageひも上下.Padding = New Padding(3)
        tpageひも上下.Size = New Size(840, 413)
        tpageひも上下.TabIndex = 8
        tpageひも上下.Text = "ひも上下"
        tpageひも上下.UseVisualStyleBackColor = True
        ' 
        ' grp縦横の四角
        ' 
        grp縦横の四角.Controls.Add(lbl底に)
        grp縦横の四角.Controls.Add(nud底に)
        grp縦横の四角.Controls.Add(lbl垂直に)
        grp縦横の四角.Controls.Add(chk横の辺)
        grp縦横の四角.Controls.Add(nud垂直に)
        grp縦横の四角.Controls.Add(btn合わせる)
        grp縦横の四角.Location = New Point(22, 6)
        grp縦横の四角.Name = "grp縦横の四角"
        grp縦横の四角.Size = New Size(463, 84)
        grp縦横の四角.TabIndex = 3
        grp縦横の四角.TabStop = False
        grp縦横の四角.Text = "縦横の四角"
        ' 
        ' lbl底に
        ' 
        lbl底に.AutoSize = True
        lbl底に.Location = New Point(224, 39)
        lbl底に.Name = "lbl底に"
        lbl底に.Size = New Size(36, 20)
        lbl底に.TabIndex = 7
        lbl底に.Text = "底に"
        ' 
        ' lbl垂直に
        ' 
        lbl垂直に.AutoSize = True
        lbl垂直に.Location = New Point(98, 39)
        lbl垂直に.Name = "lbl垂直に"
        lbl垂直に.Size = New Size(51, 20)
        lbl垂直に.TabIndex = 5
        lbl垂直に.Text = "垂直に"
        ' 
        ' editUpDown
        ' 
        editUpDown.AutoSize = True
        editUpDown.AutoSizeMode = AutoSizeMode.GrowAndShrink
        editUpDown.FormCaption = Nothing
        editUpDown.IsSquare45 = False
        editUpDown.I上右側面本数 = 0
        editUpDown.I垂直領域四角数 = 0
        editUpDown.I横の四角数 = 0
        editUpDown.I水平領域四角数 = 0
        editUpDown.I縦の四角数 = 0
        editUpDown.Location = New Point(-3, -3)
        editUpDown.Name = "editUpDown"
        editUpDown.PanelSize = New Size(800, 400)
        editUpDown.Size = New Size(806, 406)
        editUpDown.TabIndex = 1
        ' 
        ' tpageプレビュー2
        ' 
        tpageプレビュー2.Controls.Add(grp3D)
        tpageプレビュー2.Controls.Add(btn3Dモデル)
        tpageプレビュー2.Controls.Add(btn画像ファイル2)
        tpageプレビュー2.Controls.Add(btnブラウザ2)
        tpageプレビュー2.Controls.Add(picプレビュー2)
        tpageプレビュー2.Location = New Point(4, 29)
        tpageプレビュー2.Name = "tpageプレビュー2"
        tpageプレビュー2.Padding = New Padding(3)
        tpageプレビュー2.Size = New Size(840, 413)
        tpageプレビュー2.TabIndex = 9
        tpageプレビュー2.Text = "プレビュー2"
        tpageプレビュー2.UseVisualStyleBackColor = True
        ' 
        ' grp3D
        ' 
        grp3D.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        grp3D.Controls.Add(radファイル)
        grp3D.Controls.Add(radビューア)
        grp3D.Location = New Point(284, 356)
        grp3D.Name = "grp3D"
        grp3D.Size = New Size(199, 51)
        grp3D.TabIndex = 0
        grp3D.TabStop = False
        grp3D.Text = "3D"
        ' 
        ' picプレビュー2
        ' 
        picプレビュー2.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        picプレビュー2.Location = New Point(6, 6)
        picプレビュー2.Name = "picプレビュー2"
        picプレビュー2.Size = New Size(828, 349)
        picプレビュー2.SizeMode = PictureBoxSizeMode.Zoom
        picプレビュー2.TabIndex = 4
        picプレビュー2.TabStop = False
        ' 
        ' f_i段数2
        ' 
        f_i段数2.DataPropertyName = "f_i段数"
        f_i段数2.HeaderText = "段数"
        f_i段数2.MinimumWidth = 6
        f_i段数2.Name = "f_i段数2"
        f_i段数2.SortMode = DataGridViewColumnSortMode.NotSortable
        f_i段数2.Width = 125
        ' 
        ' lbl四角ベース
        ' 
        lbl四角ベース.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl四角ベース.AutoSize = True
        lbl四角ベース.Location = New Point(291, 692)
        lbl四角ベース.Name = "lbl四角ベース"
        lbl四角ベース.Size = New Size(72, 20)
        lbl四角ベース.TabIndex = 42
        lbl四角ベース.Text = "四角ベース"
        ' 
        ' lbl計算寸法
        ' 
        lbl計算寸法.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl計算寸法.AutoSize = True
        lbl計算寸法.Location = New Point(23, 625)
        lbl計算寸法.Name = "lbl計算寸法"
        lbl計算寸法.Size = New Size(69, 20)
        lbl計算寸法.TabIndex = 23
        lbl計算寸法.Text = "計算寸法"
        ' 
        ' lbl計算寸法縦
        ' 
        lbl計算寸法縦.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl計算寸法縦.AutoSize = True
        lbl計算寸法縦.Location = New Point(501, 660)
        lbl計算寸法縦.Name = "lbl計算寸法縦"
        lbl計算寸法縦.Size = New Size(24, 20)
        lbl計算寸法縦.TabIndex = 47
        lbl計算寸法縦.Text = "縦"
        ' 
        ' lbl計算寸法高さ
        ' 
        lbl計算寸法高さ.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl計算寸法高さ.AutoSize = True
        lbl計算寸法高さ.Location = New Point(579, 660)
        lbl計算寸法高さ.Name = "lbl計算寸法高さ"
        lbl計算寸法高さ.Size = New Size(34, 20)
        lbl計算寸法高さ.TabIndex = 50
        lbl計算寸法高さ.Text = "高さ"
        ' 
        ' lbl計算寸法_単位
        ' 
        lbl計算寸法_単位.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl計算寸法_単位.AutoSize = True
        lbl計算寸法_単位.Location = New Point(159, 625)
        lbl計算寸法_単位.Name = "lbl計算寸法_単位"
        lbl計算寸法_単位.Size = New Size(35, 20)
        lbl計算寸法_単位.TabIndex = 25
        lbl計算寸法_単位.Text = "mm"
        ' 
        ' lbl縁厚さプラス
        ' 
        lbl縁厚さプラス.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl縁厚さプラス.AutoSize = True
        lbl縁厚さプラス.Location = New Point(291, 723)
        lbl縁厚さプラス.Name = "lbl縁厚さプラス"
        lbl縁厚さプラス.Size = New Size(89, 20)
        lbl縁厚さプラス.TabIndex = 43
        lbl縁厚さプラス.Text = "縁・厚さプラス"
        ' 
        ' lbl計算寸法横
        ' 
        lbl計算寸法横.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl計算寸法横.AutoSize = True
        lbl計算寸法横.Location = New Point(412, 660)
        lbl計算寸法横.Name = "lbl計算寸法横"
        lbl計算寸法横.Size = New Size(24, 20)
        lbl計算寸法横.TabIndex = 44
        lbl計算寸法横.Text = "横"
        ' 
        ' lbl計算寸法周
        ' 
        lbl計算寸法周.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl計算寸法周.AutoSize = True
        lbl計算寸法周.Location = New Point(674, 660)
        lbl計算寸法周.Name = "lbl計算寸法周"
        lbl計算寸法周.Size = New Size(24, 20)
        lbl計算寸法周.TabIndex = 53
        lbl計算寸法周.Text = "周"
        ' 
        ' btn終了
        ' 
        btn終了.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btn終了.Location = New Point(756, 702)
        btn終了.Name = "btn終了"
        btn終了.Size = New Size(111, 46)
        btn終了.TabIndex = 57
        btn終了.Text = "終了(&X)"
        btn終了.UseVisualStyleBackColor = True
        ' 
        ' OpenFileDialog1
        ' 
        OpenFileDialog1.FileName = "OpenFileDialog1"
        OpenFileDialog1.Filter = "データファイル (*.xml)|*.xml|全て (*.*)|*.*"
        OpenFileDialog1.Title = "データを読み取るファイルを指定してください"
        ' 
        ' SaveFileDialog1
        ' 
        SaveFileDialog1.Filter = "データファイル (*.xml)|*.xml|全て (*.*)|*.*"
        SaveFileDialog1.Title = "データを保存するファイルを指定してください"
        ' 
        ' cmb基本色
        ' 
        cmb基本色.FormattingEnabled = True
        cmb基本色.Location = New Point(733, 66)
        cmb基本色.Name = "cmb基本色"
        cmb基本色.Size = New Size(130, 28)
        cmb基本色.TabIndex = 18
        ' 
        ' lbl基本色
        ' 
        lbl基本色.AutoSize = True
        lbl基本色.Location = New Point(752, 39)
        lbl基本色.Name = "lbl基本色"
        lbl基本色.Size = New Size(54, 20)
        lbl基本色.TabIndex = 17
        lbl基本色.Text = "基本色"
        ' 
        ' lbl横寸法の差
        ' 
        lbl横寸法の差.AutoSize = True
        lbl横寸法の差.Location = New Point(234, 101)
        lbl横寸法の差.Name = "lbl横寸法の差"
        lbl横寸法の差.Size = New Size(81, 20)
        lbl横寸法の差.TabIndex = 5
        lbl横寸法の差.Text = "横寸法の差"
        ' 
        ' lbl縦寸法の差
        ' 
        lbl縦寸法の差.AutoSize = True
        lbl縦寸法の差.Location = New Point(351, 101)
        lbl縦寸法の差.Name = "lbl縦寸法の差"
        lbl縦寸法の差.Size = New Size(81, 20)
        lbl縦寸法の差.TabIndex = 8
        lbl縦寸法の差.Text = "縦寸法の差"
        ' 
        ' lbl高さ寸法の差
        ' 
        lbl高さ寸法の差.AutoSize = True
        lbl高さ寸法の差.Location = New Point(479, 101)
        lbl高さ寸法の差.Name = "lbl高さ寸法の差"
        lbl高さ寸法の差.Size = New Size(91, 20)
        lbl高さ寸法の差.TabIndex = 11
        lbl高さ寸法の差.Text = "高さ寸法の差"
        ' 
        ' txtバンドの種類名
        ' 
        txtバンドの種類名.BorderStyle = BorderStyle.FixedSingle
        txtバンドの種類名.Location = New Point(19, 99)
        txtバンドの種類名.Name = "txtバンドの種類名"
        txtバンドの種類名.ReadOnly = True
        txtバンドの種類名.Size = New Size(181, 27)
        txtバンドの種類名.TabIndex = 2
        ' 
        ' lbl基本のひも幅length
        ' 
        lbl基本のひも幅length.AutoSize = True
        lbl基本のひも幅length.Location = New Point(618, 101)
        lbl基本のひも幅length.Name = "lbl基本のひも幅length"
        lbl基本のひも幅length.Size = New Size(131, 20)
        lbl基本のひも幅length.TabIndex = 15
        lbl基本のひも幅length.Text = "基本のひも幅length"
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.ImageScalingSize = New Size(20, 20)
        StatusStrip1.Items.AddRange(New ToolStripItem() {ToolStripStatusLabel1, ToolStripStatusLabel2})
        StatusStrip1.Location = New Point(0, 757)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Size = New Size(886, 26)
        StatusStrip1.TabIndex = 58
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' ToolStripStatusLabel1
        ' 
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New Size(152, 20)
        ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        ' 
        ' ToolStripStatusLabel2
        ' 
        ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        ToolStripStatusLabel2.Size = New Size(152, 20)
        ToolStripStatusLabel2.Text = "ToolStripStatusLabel2"
        ' 
        ' lbl単位
        ' 
        lbl単位.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl単位.AutoSize = True
        lbl単位.Location = New Point(98, 625)
        lbl単位.Name = "lbl単位"
        lbl単位.Size = New Size(42, 20)
        lbl単位.TabIndex = 24
        lbl単位.Text = "単位:"
        ' 
        ' btnDEBUG
        ' 
        btnDEBUG.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnDEBUG.Location = New Point(758, 619)
        btnDEBUG.Name = "btnDEBUG"
        btnDEBUG.Size = New Size(81, 27)
        btnDEBUG.TabIndex = 59
        btnDEBUG.Text = "DEBUG"
        btnDEBUG.UseVisualStyleBackColor = True
        btnDEBUG.Visible = False
        ' 
        ' f_i番号
        ' 
        f_i番号.DataPropertyName = "f_i番号"
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight
        f_i番号.DefaultCellStyle = DataGridViewCellStyle1
        f_i番号.HeaderText = "番号"
        f_i番号.MinimumWidth = 6
        f_i番号.Name = "f_i番号"
        f_i番号.ReadOnly = True
        f_i番号.SortMode = DataGridViewColumnSortMode.NotSortable
        f_i番号.Width = 59
        ' 
        ' f_s編みかた名
        ' 
        f_s編みかた名.DataPropertyName = "f_s編みかた名"
        f_s編みかた名.HeaderText = "編みかた名"
        f_s編みかた名.MinimumWidth = 6
        f_s編みかた名.Name = "f_s編みかた名"
        f_s編みかた名.ReadOnly = True
        f_s編みかた名.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s編みかた名.Width = 125
        ' 
        ' f_s編みひも名
        ' 
        f_s編みひも名.DataPropertyName = "f_s編みひも名"
        f_s編みひも名.HeaderText = "編みひも名"
        f_s編みひも名.MinimumWidth = 6
        f_s編みひも名.Name = "f_s編みひも名"
        f_s編みひも名.ReadOnly = True
        f_s編みひも名.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s編みひも名.Width = 112
        ' 
        ' f_iひも番号
        ' 
        f_iひも番号.DataPropertyName = "f_iひも番号"
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight
        f_iひも番号.DefaultCellStyle = DataGridViewCellStyle2
        f_iひも番号.HeaderText = "ひも番号"
        f_iひも番号.MinimumWidth = 6
        f_iひも番号.Name = "f_iひも番号"
        f_iひも番号.ReadOnly = True
        f_iひも番号.SortMode = DataGridViewColumnSortMode.NotSortable
        f_iひも番号.Width = 77
        ' 
        ' f_i何本幅
        ' 
        f_i何本幅.DataPropertyName = "f_i何本幅"
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter
        f_i何本幅.DefaultCellStyle = DataGridViewCellStyle3
        f_i何本幅.HeaderText = "何本幅"
        f_i何本幅.MinimumWidth = 6
        f_i何本幅.Name = "f_i何本幅"
        f_i何本幅.Resizable = DataGridViewTriState.True
        f_i何本幅.Width = 77
        ' 
        ' f_b集計対象外区分
        ' 
        f_b集計対象外区分.DataPropertyName = "f_b集計対象外区分"
        f_b集計対象外区分.HeaderText = "集計対象外"
        f_b集計対象外区分.MinimumWidth = 6
        f_b集計対象外区分.Name = "f_b集計対象外区分"
        f_b集計対象外区分.Width = 125
        ' 
        ' f_s色
        ' 
        f_s色.DataPropertyName = "f_s色"
        f_s色.HeaderText = "色"
        f_s色.MinimumWidth = 6
        f_s色.Name = "f_s色"
        f_s色.Resizable = DataGridViewTriState.True
        f_s色.Width = 80
        ' 
        ' f_d高さ
        ' 
        f_d高さ.DataPropertyName = "f_d高さ"
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N2"
        DataGridViewCellStyle4.NullValue = Nothing
        f_d高さ.DefaultCellStyle = DataGridViewCellStyle4
        f_d高さ.HeaderText = "高さ"
        f_d高さ.MinimumWidth = 6
        f_d高さ.Name = "f_d高さ"
        f_d高さ.ReadOnly = True
        f_d高さ.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d高さ.Width = 125
        ' 
        ' f_d垂直ひも長
        ' 
        f_d垂直ひも長.DataPropertyName = "f_d垂直ひも長"
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N2"
        DataGridViewCellStyle5.NullValue = Nothing
        f_d垂直ひも長.DefaultCellStyle = DataGridViewCellStyle5
        f_d垂直ひも長.HeaderText = "垂直ひも長"
        f_d垂直ひも長.MinimumWidth = 6
        f_d垂直ひも長.Name = "f_d垂直ひも長"
        f_d垂直ひも長.ReadOnly = True
        f_d垂直ひも長.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d垂直ひも長.Width = 125
        ' 
        ' f_d周長比率対底の周
        ' 
        f_d周長比率対底の周.DataPropertyName = "f_d周長比率対底の周"
        f_d周長比率対底の周.HeaderText = "f_d周長比率対底の周"
        f_d周長比率対底の周.MinimumWidth = 6
        f_d周長比率対底の周.Name = "f_d周長比率対底の周"
        f_d周長比率対底の周.Visible = False
        f_d周長比率対底の周.Width = 125
        ' 
        ' f_d周長
        ' 
        f_d周長.DataPropertyName = "f_d周長"
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N2"
        DataGridViewCellStyle6.NullValue = Nothing
        f_d周長.DefaultCellStyle = DataGridViewCellStyle6
        f_d周長.HeaderText = "周長"
        f_d周長.MinimumWidth = 6
        f_d周長.Name = "f_d周長"
        f_d周長.ReadOnly = True
        f_d周長.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d周長.Width = 125
        ' 
        ' f_dひも長
        ' 
        f_dひも長.DataPropertyName = "f_dひも長"
        DataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "N2"
        DataGridViewCellStyle7.NullValue = Nothing
        f_dひも長.DefaultCellStyle = DataGridViewCellStyle7
        f_dひも長.HeaderText = "ひも長"
        f_dひも長.MinimumWidth = 6
        f_dひも長.Name = "f_dひも長"
        f_dひも長.ReadOnly = True
        f_dひも長.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも長.Width = 125
        ' 
        ' f_dひも長加算
        ' 
        f_dひも長加算.DataPropertyName = "f_dひも長加算"
        DataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長加算.DefaultCellStyle = DataGridViewCellStyle8
        f_dひも長加算.HeaderText = "ひも長加算"
        f_dひも長加算.MinimumWidth = 6
        f_dひも長加算.Name = "f_dひも長加算"
        f_dひも長加算.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも長加算.ToolTipText = "出力に加える余裕長"
        f_dひも長加算.Width = 125
        ' 
        ' f_iひも本数
        ' 
        f_iひも本数.DataPropertyName = "f_iひも本数"
        DataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight
        f_iひも本数.DefaultCellStyle = DataGridViewCellStyle9
        f_iひも本数.HeaderText = "ひも本数"
        f_iひも本数.MinimumWidth = 6
        f_iひも本数.Name = "f_iひも本数"
        f_iひも本数.ReadOnly = True
        f_iひも本数.SortMode = DataGridViewColumnSortMode.NotSortable
        f_iひも本数.Width = 125
        ' 
        ' f_d厚さ
        ' 
        f_d厚さ.DataPropertyName = "f_d厚さ"
        DataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle10.NullValue = Nothing
        f_d厚さ.DefaultCellStyle = DataGridViewCellStyle10
        f_d厚さ.HeaderText = "厚さ"
        f_d厚さ.MinimumWidth = 6
        f_d厚さ.Name = "f_d厚さ"
        f_d厚さ.ReadOnly = True
        f_d厚さ.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d厚さ.Width = 125
        ' 
        ' f_sメモ
        ' 
        f_sメモ.DataPropertyName = "f_sメモ"
        f_sメモ.HeaderText = "メモ"
        f_sメモ.MinimumWidth = 6
        f_sメモ.Name = "f_sメモ"
        f_sメモ.SortMode = DataGridViewColumnSortMode.NotSortable
        f_sメモ.Width = 125
        ' 
        ' f_d連続ひも長
        ' 
        f_d連続ひも長.DataPropertyName = "f_d連続ひも長"
        DataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle11.Format = "N2"
        DataGridViewCellStyle11.NullValue = Nothing
        f_d連続ひも長.DefaultCellStyle = DataGridViewCellStyle11
        f_d連続ひも長.HeaderText = "f_d連続ひも長"
        f_d連続ひも長.MinimumWidth = 6
        f_d連続ひも長.Name = "f_d連続ひも長"
        f_d連続ひも長.Visible = False
        f_d連続ひも長.Width = 125
        ' 
        ' f_s記号
        ' 
        f_s記号.DataPropertyName = "f_s記号"
        f_s記号.HeaderText = "f_s記号"
        f_s記号.MinimumWidth = 6
        f_s記号.Name = "f_s記号"
        f_s記号.Visible = False
        f_s記号.Width = 125
        ' 
        ' f_bError
        ' 
        f_bError.DataPropertyName = "f_bError"
        f_bError.HeaderText = "f_bError"
        f_bError.MinimumWidth = 6
        f_bError.Name = "f_bError"
        f_bError.Visible = False
        f_bError.Width = 125
        ' 
        ' frmMain
        ' 
        AllowDrop = True
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(886, 783)
        Controls.Add(txt後の三角形の本幅の差)
        Controls.Add(txt四辺形の本幅の差)
        Controls.Add(lbl本幅の差)
        Controls.Add(txt先の三角形の本幅の差)
        Controls.Add(lbl厚さ)
        Controls.Add(txt厚さ)
        Controls.Add(lbl四角)
        Controls.Add(txt対角線_四角)
        Controls.Add(txt1つの辺_四角)
        Controls.Add(lblひも幅)
        Controls.Add(lbl1つの辺)
        Controls.Add(txt対角線_ひも幅)
        Controls.Add(txt1つの辺_ひも幅)
        Controls.Add(lbl対角線)
        Controls.Add(btnリセット)
        Controls.Add(btn規定値)
        Controls.Add(btn概算)
        Controls.Add(btnDEBUG)
        Controls.Add(StatusStrip1)
        Controls.Add(lbl基本のひも幅length)
        Controls.Add(txtバンドの種類名)
        Controls.Add(lbl高さ寸法の差)
        Controls.Add(lbl縦寸法の差)
        Controls.Add(lbl横寸法の差)
        Controls.Add(lbl基本色)
        Controls.Add(cmb基本色)
        Controls.Add(btn終了)
        Controls.Add(btnひもリスト)
        Controls.Add(lbl計算寸法_単位)
        Controls.Add(lbl計算寸法高さ)
        Controls.Add(lbl計算寸法周)
        Controls.Add(lbl計算寸法縦)
        Controls.Add(lbl計算寸法横)
        Controls.Add(lbl垂直ひも数)
        Controls.Add(lbl単位)
        Controls.Add(lbl計算寸法)
        Controls.Add(txt縁厚さプラス_周)
        Controls.Add(txt縁厚さプラス_高さ)
        Controls.Add(txt縁厚さプラス_縦)
        Controls.Add(txt四角ベース_周)
        Controls.Add(txt四角ベース_高さ)
        Controls.Add(txt縁厚さプラス_横)
        Controls.Add(txt四角ベース_縦)
        Controls.Add(lbl縁厚さプラス)
        Controls.Add(txt垂直ひも数)
        Controls.Add(txt四角ベース_横)
        Controls.Add(lbl四角ベース)
        Controls.Add(TabControl)
        Controls.Add(lbl目標寸法_単位)
        Controls.Add(nud高さ寸法)
        Controls.Add(lbl高さ寸法)
        Controls.Add(nud縦寸法)
        Controls.Add(lbl縦寸法)
        Controls.Add(nud横寸法)
        Controls.Add(grp目標寸法)
        Controls.Add(lbl横寸法)
        Controls.Add(lbl基本のひも幅_単位)
        Controls.Add(lbl基本のひも幅)
        Controls.Add(nud基本のひも幅)
        Controls.Add(MenuStrip1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = MenuStrip1
        MinimumSize = New Size(904, 830)
        Name = "frmMain"
        Text = "斜め四角45度"
        CType(BindingSource縁の始末, ComponentModel.ISupportInitialize).EndInit()
        CType(nud基本のひも幅, ComponentModel.ISupportInitialize).EndInit()
        CType(nud横寸法, ComponentModel.ISupportInitialize).EndInit()
        CType(nud縦寸法, ComponentModel.ISupportInitialize).EndInit()
        CType(nud高さ寸法, ComponentModel.ISupportInitialize).EndInit()
        CType(nudひも間のすき間, ComponentModel.ISupportInitialize).EndInit()
        CType(nudひも長加算, ComponentModel.ISupportInitialize).EndInit()
        CType(nudひも長係数, ComponentModel.ISupportInitialize).EndInit()
        CType(nud高さの四角数, ComponentModel.ISupportInitialize).EndInit()
        CType(nud縦の四角数, ComponentModel.ISupportInitialize).EndInit()
        CType(nud横の四角数, ComponentModel.ISupportInitialize).EndInit()
        grp目標寸法.ResumeLayout(False)
        grp目標寸法.PerformLayout()
        CType(nud底に, ComponentModel.ISupportInitialize).EndInit()
        CType(nud垂直に, ComponentModel.ISupportInitialize).EndInit()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        TabControl.ResumeLayout(False)
        tpage四角数.ResumeLayout(False)
        tpage四角数.PerformLayout()
        grp縦置き.ResumeLayout(False)
        grp縦置き.PerformLayout()
        grp横置き.ResumeLayout(False)
        grp横置き.PerformLayout()
        tpage縁の始末.ResumeLayout(False)
        tpage縁の始末.PerformLayout()
        CType(dgv縁の始末, ComponentModel.ISupportInitialize).EndInit()
        tpage追加品.ResumeLayout(False)
        tpage追加品.PerformLayout()
        tpageメモ他.ResumeLayout(False)
        tpageメモ他.PerformLayout()
        tpage横ひも.ResumeLayout(False)
        tpage縦ひも.ResumeLayout(False)
        tpageプレビュー.ResumeLayout(False)
        tpageプレビュー.PerformLayout()
        CType(picプレビュー, ComponentModel.ISupportInitialize).EndInit()
        tpageひも上下.ResumeLayout(False)
        tpageひも上下.PerformLayout()
        grp縦横の四角.ResumeLayout(False)
        grp縦横の四角.PerformLayout()
        tpageプレビュー2.ResumeLayout(False)
        grp3D.ResumeLayout(False)
        grp3D.PerformLayout()
        CType(picプレビュー2, ComponentModel.ISupportInitialize).EndInit()
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ToolStripMenuItemFile As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFileOpen As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFileSaveAs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFileSave As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFileExit As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSetting As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemHelp As ToolStripMenuItem
    Friend WithEvents nud基本のひも幅 As NumericUpDown
    Friend WithEvents lbl基本のひも幅 As Label
    Friend WithEvents lbl基本のひも幅_単位 As Label
    Friend WithEvents lbl横寸法 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents ToolStripMenuItemSettingOptions As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSettingBandType As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSettingBasics As ToolStripMenuItem
    Friend WithEvents grp目標寸法 As GroupBox
    Friend WithEvents rad目標寸法_内側 As RadioButton
    Friend WithEvents rad目標寸法_外側 As RadioButton
    Friend WithEvents nud横寸法 As NumericUpDown
    Friend WithEvents nud縦寸法 As NumericUpDown
    Friend WithEvents lbl縦寸法 As Label
    Friend WithEvents nud高さ寸法 As NumericUpDown
    Friend WithEvents lbl高さ寸法 As Label
    Friend WithEvents lbl目標寸法_単位 As Label
    Friend WithEvents btn概算 As Button
    Friend WithEvents TabControl As TabControl
    Friend WithEvents tpage四角数 As TabPage
    Friend WithEvents grp縦置き As GroupBox
    Friend WithEvents lbl縦の四角数_単位 As Label
    Friend WithEvents lbl縦の四角数 As Label
    Friend WithEvents nud縦の四角数 As NumericUpDown
    Friend WithEvents grp横置き As GroupBox
    Friend WithEvents lbl横の四角数_単位 As Label
    Friend WithEvents lbl横の四角数 As Label
    Friend WithEvents nud横の四角数 As NumericUpDown
    Friend WithEvents tpage縁の始末 As TabPage
    Friend WithEvents lbl四角ベース As Label
    Friend WithEvents txt四角ベース_横 As TextBox
    Friend WithEvents lbl計算寸法 As Label
    Friend WithEvents txt四角ベース_縦 As TextBox
    Friend WithEvents lbl計算寸法縦 As Label
    Friend WithEvents txt四角ベース_高さ As TextBox
    Friend WithEvents lbl計算寸法高さ As Label
    Friend WithEvents lbl計算寸法_単位 As Label
    Friend WithEvents lbl縁厚さプラス As Label
    Friend WithEvents txt縁厚さプラス_横 As TextBox
    Friend WithEvents txt縁厚さプラス_縦 As TextBox
    Friend WithEvents txt縁厚さプラス_高さ As TextBox
    Friend WithEvents lbl計算寸法横 As Label
    Friend WithEvents lbl計算寸法周 As Label
    Friend WithEvents btnひもリスト As Button
    Friend WithEvents btn終了 As Button
    Friend WithEvents btn削除_側面 As Button
    Friend WithEvents btn追加_側面 As Button
    Friend WithEvents cmb編みかた名_側面 As ComboBox
    Friend WithEvents tpage追加品 As TabPage
    Friend WithEvents ToolStripMenuItemSettingPattern As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFileNew As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents txt四角ベース_周 As TextBox
    Friend WithEvents txt縁厚さプラス_周 As TextBox
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents cmb基本色 As ComboBox
    Friend WithEvents lbl基本色 As Label
    Friend WithEvents lbl横寸法の差 As Label
    Friend WithEvents lbl縦寸法の差 As Label
    Friend WithEvents lbl高さ寸法の差 As Label
    Friend WithEvents txtバンドの種類名 As TextBox
    Friend WithEvents lbl基本のひも幅length As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItemFileAbort As ToolStripMenuItem
    Friend WithEvents BindingSource縁の始末 As BindingSource
    Friend WithEvents Fd全周の高さDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Fd周囲の寸法DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Fd対底周囲比率DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents lbl編みかた名_側面 As Label
    Friend WithEvents f_i段数2 As DataGridViewTextBoxColumn
    Friend WithEvents lbl垂直ひも数 As Label
    Friend WithEvents txt垂直ひも数 As TextBox
    Friend WithEvents chk縦の補強ひも As CheckBox
    Friend WithEvents chk横の補強ひも As CheckBox
    Friend WithEvents lbl単位 As Label
    Friend WithEvents btnDEBUG As Button
    Friend WithEvents nudひも長加算 As NumericUpDown
    Friend WithEvents lblひも長加算_単位 As Label
    Friend WithEvents lblひも長加算 As Label
    Friend WithEvents nudひも間のすき間 As NumericUpDown
    Friend WithEvents lblひも間のすき間_単位 As Label
    Friend WithEvents lblひも間のすき間 As Label
    Friend WithEvents tpageメモ他 As TabPage
    Friend WithEvents txtメモ As TextBox
    Friend WithEvents lblメモ As Label
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents btn規定値 As Button
    Friend WithEvents btnリセット As Button
    Friend WithEvents ToolStripMenuItemEdit As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditSelectBand As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemEditReset As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditDefault As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditCalc As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemEditList As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditDefaultFile As ToolStripMenuItem
    Friend WithEvents lbl縦ひものメモ As Label
    Friend WithEvents txt縦ひものメモ As TextBox
    Friend WithEvents lbl横ひものメモ As Label
    Friend WithEvents txt横ひものメモ As TextBox
    Friend WithEvents tpage横ひも As TabPage
    Friend WithEvents tpage縦ひも As TabPage
    Friend WithEvents chk縦横を展開する As CheckBox
    Friend WithEvents txt横ひもの本数 As TextBox
    Friend WithEvents lbl横ひもの本数_単位 As Label
    Friend WithEvents lbl横ひも As Label
    Friend WithEvents nudひも長係数 As NumericUpDown
    Friend WithEvents Label10 As Label
    Friend WithEvents lbl高さの四角数_単位 As Label
    Friend WithEvents nud高さの四角数 As NumericUpDown
    Friend WithEvents lbl高さの四角数 As Label
    Friend WithEvents txt縦ひもの本数 As TextBox
    Friend WithEvents lbl縦ひもの本数_単位 As Label
    Friend WithEvents lbl縦ひも As Label
    Friend WithEvents lbl1つの辺 As Label
    Friend WithEvents txt対角線_ひも幅 As TextBox
    Friend WithEvents txt1つの辺_ひも幅 As TextBox
    Friend WithEvents lbl対角線 As Label
    Friend WithEvents lblひも幅 As Label
    Friend WithEvents tpageプレビュー As TabPage
    Friend WithEvents txt対角線_四角 As TextBox
    Friend WithEvents txt1つの辺_四角 As TextBox
    Friend WithEvents lbl四角 As Label
    Friend WithEvents lbl厚さ As Label
    Friend WithEvents txt厚さ As TextBox
    Friend WithEvents btnブラウザ As Button
    Friend WithEvents picプレビュー As PictureBox
    Friend WithEvents btn画像ファイル As Button
    Friend WithEvents ToolStripMenuItemSettingColor As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditColorChange As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditColorRepeat As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents tpageひも上下 As TabPage
    Friend WithEvents editUpDown As ctrEditUpDown
    Friend WithEvents grp縦横の四角 As GroupBox
    Friend WithEvents lbl底に As Label
    Friend WithEvents nud底に As NumericUpDown
    Friend WithEvents lbl垂直に As Label
    Friend WithEvents chk横の辺 As CheckBox
    Friend WithEvents nud垂直に As NumericUpDown
    Friend WithEvents btn合わせる As Button
    Friend WithEvents editAddParts As ctrAddParts
    Friend WithEvents ToolStripMenuItemSettingUpDown As ToolStripMenuItem
    Friend WithEvents txt作成者 As TextBox
    Friend WithEvents txtタイトル As TextBox
    Friend WithEvents lbl作成者 As Label
    Friend WithEvents lblタイトル As Label
    Friend WithEvents expand横ひも As ctrExpanding
    Friend WithEvents expand縦ひも As ctrExpanding
    Friend WithEvents lbl本幅の差 As Label
    Friend WithEvents txt先の三角形の本幅の差 As TextBox
    Friend WithEvents txt四辺形の本幅の差 As TextBox
    Friend WithEvents txt後の三角形の本幅の差 As TextBox
    Friend WithEvents radうら As RadioButton
    Friend WithEvents radおもて As RadioButton
    Friend WithEvents dgv縁の始末 As ctrDataGridView
    Friend WithEvents tpageプレビュー2 As TabPage
    Friend WithEvents btn画像ファイル2 As Button
    Friend WithEvents btnブラウザ2 As Button
    Friend WithEvents picプレビュー2 As PictureBox
    Friend WithEvents btn3Dモデル As Button
    Friend WithEvents grp3D As GroupBox
    Friend WithEvents radビューア As RadioButton
    Friend WithEvents radファイル As RadioButton
    Friend WithEvents f_i番号 As DataGridViewTextBoxColumn
    Friend WithEvents f_s編みかた名 As DataGridViewTextBoxColumn
    Friend WithEvents f_s編みひも名 As DataGridViewTextBoxColumn
    Friend WithEvents f_iひも番号 As DataGridViewTextBoxColumn
    Friend WithEvents f_i何本幅 As DataGridViewComboBoxColumn
    Friend WithEvents f_b集計対象外区分 As DataGridViewCheckBoxColumn
    Friend WithEvents f_s色 As DataGridViewComboBoxColumn
    Friend WithEvents f_d高さ As DataGridViewTextBoxColumn
    Friend WithEvents f_d垂直ひも長 As DataGridViewTextBoxColumn
    Friend WithEvents f_d周長比率対底の周 As DataGridViewTextBoxColumn
    Friend WithEvents f_d周長 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算 As DataGridViewTextBoxColumn
    Friend WithEvents f_iひも本数 As DataGridViewTextBoxColumn
    Friend WithEvents f_d厚さ As DataGridViewTextBoxColumn
    Friend WithEvents f_sメモ As DataGridViewTextBoxColumn
    Friend WithEvents f_d連続ひも長 As DataGridViewTextBoxColumn
    Friend WithEvents f_s記号 As DataGridViewTextBoxColumn
    Friend WithEvents f_bError As DataGridViewCheckBoxColumn
End Class
