Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGauge
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtコマ寸法係数a = New System.Windows.Forms.TextBox()
        Me.txtコマ寸法係数b = New System.Windows.Forms.TextBox()
        Me.txtコマ要尺係数a = New System.Windows.Forms.TextBox()
        Me.txtコマ要尺係数b = New System.Windows.Forms.TextBox()
        Me.lblコマ寸法係数a = New System.Windows.Forms.Label()
        Me.lblコマ寸法係数b = New System.Windows.Forms.Label()
        Me.lblコマ要尺係数a = New System.Windows.Forms.Label()
        Me.lblコマ要尺係数b = New System.Windows.Forms.Label()
        Me.dgvData = New System.Windows.Forms.DataGridView()
        Me.f_sバンドの種類名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_i本幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dコマ寸法計算値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dコマ要尺計算値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_b係数取得区分 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_dコマ寸法実測値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dコマ要尺実測値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_b実測値使用区分 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_s備考 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSourceゲージ = New System.Windows.Forms.BindingSource(Me.components)
        Me.f_d実測値使用区分 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.cmb対象バンドの種類名 = New System.Windows.Forms.ComboBox()
        Me.lbl対象バンドの種類名 = New System.Windows.Forms.Label()
        Me.btnリセット = New System.Windows.Forms.Button()
        Me.btn係数取得 = New System.Windows.Forms.Button()
        Me.lbl設定時の寸法単位 = New System.Windows.Forms.Label()
        Me.lbl単位 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceゲージ, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtコマ寸法係数a
        '
        Me.txtコマ寸法係数a.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtコマ寸法係数a.Location = New System.Drawing.Point(315, 34)
        Me.txtコマ寸法係数a.Name = "txtコマ寸法係数a"
        Me.txtコマ寸法係数a.ReadOnly = True
        Me.txtコマ寸法係数a.Size = New System.Drawing.Size(116, 27)
        Me.txtコマ寸法係数a.TabIndex = 3
        Me.txtコマ寸法係数a.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.txtコマ寸法係数a, "1コマの寸法、ひも幅に対する係数、1が2D値")
        '
        'txtコマ寸法係数b
        '
        Me.txtコマ寸法係数b.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtコマ寸法係数b.Location = New System.Drawing.Point(434, 34)
        Me.txtコマ寸法係数b.Name = "txtコマ寸法係数b"
        Me.txtコマ寸法係数b.ReadOnly = True
        Me.txtコマ寸法係数b.Size = New System.Drawing.Size(116, 27)
        Me.txtコマ寸法係数b.TabIndex = 5
        Me.txtコマ寸法係数b.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.txtコマ寸法係数b, "1コマの寸法、プラスする係数、0が2D値・1がひも幅")
        '
        'txtコマ要尺係数a
        '
        Me.txtコマ要尺係数a.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtコマ要尺係数a.Location = New System.Drawing.Point(553, 34)
        Me.txtコマ要尺係数a.Name = "txtコマ要尺係数a"
        Me.txtコマ要尺係数a.ReadOnly = True
        Me.txtコマ要尺係数a.Size = New System.Drawing.Size(116, 27)
        Me.txtコマ要尺係数a.TabIndex = 7
        Me.txtコマ要尺係数a.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.txtコマ要尺係数a, "1コマに必要な長さ、ひも幅に対する係数、1が2D値")
        '
        'txtコマ要尺係数b
        '
        Me.txtコマ要尺係数b.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtコマ要尺係数b.Location = New System.Drawing.Point(672, 34)
        Me.txtコマ要尺係数b.Name = "txtコマ要尺係数b"
        Me.txtコマ要尺係数b.ReadOnly = True
        Me.txtコマ要尺係数b.Size = New System.Drawing.Size(116, 27)
        Me.txtコマ要尺係数b.TabIndex = 9
        Me.txtコマ要尺係数b.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.txtコマ要尺係数b, "1コマに必要な長さ、プラスする係数、0が2D値・1がひも幅")
        '
        'lblコマ寸法係数a
        '
        Me.lblコマ寸法係数a.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblコマ寸法係数a.AutoSize = True
        Me.lblコマ寸法係数a.Location = New System.Drawing.Point(333, 6)
        Me.lblコマ寸法係数a.Name = "lblコマ寸法係数a"
        Me.lblコマ寸法係数a.Size = New System.Drawing.Size(98, 20)
        Me.lblコマ寸法係数a.TabIndex = 2
        Me.lblコマ寸法係数a.Text = "コマ寸法係数a"
        '
        'lblコマ寸法係数b
        '
        Me.lblコマ寸法係数b.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblコマ寸法係数b.AutoSize = True
        Me.lblコマ寸法係数b.Location = New System.Drawing.Point(451, 6)
        Me.lblコマ寸法係数b.Name = "lblコマ寸法係数b"
        Me.lblコマ寸法係数b.Size = New System.Drawing.Size(99, 20)
        Me.lblコマ寸法係数b.TabIndex = 4
        Me.lblコマ寸法係数b.Text = "コマ寸法係数b"
        '
        'lblコマ要尺係数a
        '
        Me.lblコマ要尺係数a.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblコマ要尺係数a.AutoSize = True
        Me.lblコマ要尺係数a.Location = New System.Drawing.Point(570, 6)
        Me.lblコマ要尺係数a.Name = "lblコマ要尺係数a"
        Me.lblコマ要尺係数a.Size = New System.Drawing.Size(98, 20)
        Me.lblコマ要尺係数a.TabIndex = 6
        Me.lblコマ要尺係数a.Text = "コマ要尺係数a"
        '
        'lblコマ要尺係数b
        '
        Me.lblコマ要尺係数b.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblコマ要尺係数b.AutoSize = True
        Me.lblコマ要尺係数b.Location = New System.Drawing.Point(688, 6)
        Me.lblコマ要尺係数b.Name = "lblコマ要尺係数b"
        Me.lblコマ要尺係数b.Size = New System.Drawing.Size(99, 20)
        Me.lblコマ要尺係数b.TabIndex = 8
        Me.lblコマ要尺係数b.Text = "コマ要尺係数b"
        '
        'dgvData
        '
        Me.dgvData.AllowUserToAddRows = False
        Me.dgvData.AllowUserToDeleteRows = False
        Me.dgvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvData.AutoGenerateColumns = False
        Me.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.f_sバンドの種類名, Me.f_i本幅, Me.f_dひも幅, Me.f_dコマ寸法計算値, Me.f_dコマ要尺計算値, Me.f_b係数取得区分, Me.f_dコマ寸法実測値, Me.f_dコマ要尺実測値, Me.f_b実測値使用区分, Me.f_s備考})
        Me.dgvData.DataSource = Me.BindingSourceゲージ
        Me.dgvData.Location = New System.Drawing.Point(12, 70)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.RowHeadersWidth = 51
        Me.dgvData.RowTemplate.Height = 29
        Me.dgvData.Size = New System.Drawing.Size(779, 315)
        Me.dgvData.TabIndex = 12
        '
        'f_sバンドの種類名
        '
        Me.f_sバンドの種類名.DataPropertyName = "f_sバンドの種類名"
        Me.f_sバンドの種類名.HeaderText = "f_sバンドの種類名"
        Me.f_sバンドの種類名.MinimumWidth = 6
        Me.f_sバンドの種類名.Name = "f_sバンドの種類名"
        Me.f_sバンドの種類名.Visible = False
        Me.f_sバンドの種類名.Width = 125
        '
        'f_i本幅
        '
        Me.f_i本幅.DataPropertyName = "f_i本幅"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.f_i本幅.DefaultCellStyle = DataGridViewCellStyle1
        Me.f_i本幅.HeaderText = "本幅"
        Me.f_i本幅.MinimumWidth = 6
        Me.f_i本幅.Name = "f_i本幅"
        Me.f_i本幅.ReadOnly = True
        Me.f_i本幅.ToolTipText = "何本幅か"
        Me.f_i本幅.Width = 125
        '
        'f_dひも幅
        '
        Me.f_dひも幅.DataPropertyName = "f_dひも幅"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.f_dひも幅.DefaultCellStyle = DataGridViewCellStyle2
        Me.f_dひも幅.HeaderText = "ひも幅"
        Me.f_dひも幅.MinimumWidth = 6
        Me.f_dひも幅.Name = "f_dひも幅"
        Me.f_dひも幅.ReadOnly = True
        Me.f_dひも幅.ToolTipText = "本幅に相当するひも幅"
        Me.f_dひも幅.Width = 125
        '
        'f_dコマ寸法計算値
        '
        Me.f_dコマ寸法計算値.DataPropertyName = "f_dコマ寸法計算値"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "N2"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.f_dコマ寸法計算値.DefaultCellStyle = DataGridViewCellStyle3
        Me.f_dコマ寸法計算値.HeaderText = "コマ寸法計算値"
        Me.f_dコマ寸法計算値.MinimumWidth = 6
        Me.f_dコマ寸法計算値.Name = "f_dコマ寸法計算値"
        Me.f_dコマ寸法計算値.ReadOnly = True
        Me.f_dコマ寸法計算値.ToolTipText = "係数をもとに計算したコマのサイズ"
        Me.f_dコマ寸法計算値.Width = 125
        '
        'f_dコマ要尺計算値
        '
        Me.f_dコマ要尺計算値.DataPropertyName = "f_dコマ要尺計算値"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N2"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.f_dコマ要尺計算値.DefaultCellStyle = DataGridViewCellStyle4
        Me.f_dコマ要尺計算値.HeaderText = "コマ要尺計算値"
        Me.f_dコマ要尺計算値.MinimumWidth = 6
        Me.f_dコマ要尺計算値.Name = "f_dコマ要尺計算値"
        Me.f_dコマ要尺計算値.ReadOnly = True
        Me.f_dコマ要尺計算値.ToolTipText = "係数をもとに計算した1コマ編むのに必要な長さ"
        Me.f_dコマ要尺計算値.Width = 125
        '
        'f_b係数取得区分
        '
        Me.f_b係数取得区分.DataPropertyName = "f_b係数取得区分"
        Me.f_b係数取得区分.HeaderText = "係数取得区分"
        Me.f_b係数取得区分.MinimumWidth = 6
        Me.f_b係数取得区分.Name = "f_b係数取得区分"
        Me.f_b係数取得区分.ToolTipText = "係数の計算に実測値を使用する場合チェックをONにする"
        Me.f_b係数取得区分.Width = 125
        '
        'f_dコマ寸法実測値
        '
        Me.f_dコマ寸法実測値.DataPropertyName = "f_dコマ寸法実測値"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dコマ寸法実測値.DefaultCellStyle = DataGridViewCellStyle5
        Me.f_dコマ寸法実測値.HeaderText = "コマ寸法実測値"
        Me.f_dコマ寸法実測値.MinimumWidth = 6
        Me.f_dコマ寸法実測値.Name = "f_dコマ寸法実測値"
        Me.f_dコマ寸法実測値.ToolTipText = "1コマの寸法(コマの中央-コマの中央)の実測値"
        Me.f_dコマ寸法実測値.Width = 125
        '
        'f_dコマ要尺実測値
        '
        Me.f_dコマ要尺実測値.DataPropertyName = "f_dコマ要尺実測値"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dコマ要尺実測値.DefaultCellStyle = DataGridViewCellStyle6
        Me.f_dコマ要尺実測値.HeaderText = "コマ要尺実測値"
        Me.f_dコマ要尺実測値.MinimumWidth = 6
        Me.f_dコマ要尺実測値.Name = "f_dコマ要尺実測値"
        Me.f_dコマ要尺実測値.ToolTipText = "1コマを編むのに必要な長さ"
        Me.f_dコマ要尺実測値.Width = 125
        '
        'f_b実測値使用区分
        '
        Me.f_b実測値使用区分.DataPropertyName = "f_b実測値使用区分"
        Me.f_b実測値使用区分.HeaderText = "実測値使用区分"
        Me.f_b実測値使用区分.MinimumWidth = 6
        Me.f_b実測値使用区分.Name = "f_b実測値使用区分"
        Me.f_b実測値使用区分.ToolTipText = "ひもの計算に使う値、ONの時実測値・OFFの時計算値"
        Me.f_b実測値使用区分.Width = 125
        '
        'f_s備考
        '
        Me.f_s備考.DataPropertyName = "f_s備考"
        Me.f_s備考.HeaderText = "備考"
        Me.f_s備考.MinimumWidth = 6
        Me.f_s備考.Name = "f_s備考"
        Me.f_s備考.Width = 125
        '
        'BindingSourceゲージ
        '
        Me.BindingSourceゲージ.DataMember = "tblGauge"
        Me.BindingSourceゲージ.DataSource = GetType(CraftBand.Tables.dstWork)
        '
        'f_d実測値使用区分
        '
        Me.f_d実測値使用区分.DataPropertyName = "f_d実測値使用区分"
        Me.f_d実測値使用区分.HeaderText = "実測値使用区分"
        Me.f_d実測値使用区分.MinimumWidth = 6
        Me.f_d実測値使用区分.Name = "f_d実測値使用区分"
        Me.f_d実測値使用区分.Width = 125
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.Location = New System.Drawing.Point(682, 399)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 16
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(563, 399)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 15
        Me.btnOK.Text = "OK(&O)"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'cmb対象バンドの種類名
        '
        Me.cmb対象バンドの種類名.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmb対象バンドの種類名.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb対象バンドの種類名.FormattingEnabled = True
        Me.cmb対象バンドの種類名.Location = New System.Drawing.Point(158, 6)
        Me.cmb対象バンドの種類名.Name = "cmb対象バンドの種類名"
        Me.cmb対象バンドの種類名.Size = New System.Drawing.Size(147, 28)
        Me.cmb対象バンドの種類名.TabIndex = 1
        '
        'lbl対象バンドの種類名
        '
        Me.lbl対象バンドの種類名.AutoSize = True
        Me.lbl対象バンドの種類名.Location = New System.Drawing.Point(22, 9)
        Me.lbl対象バンドの種類名.Name = "lbl対象バンドの種類名"
        Me.lbl対象バンドの種類名.Size = New System.Drawing.Size(130, 20)
        Me.lbl対象バンドの種類名.TabIndex = 0
        Me.lbl対象バンドの種類名.Text = "対象バンドの種類名"
        '
        'btnリセット
        '
        Me.btnリセット.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnリセット.Location = New System.Drawing.Point(12, 399)
        Me.btnリセット.Name = "btnリセット"
        Me.btnリセット.Size = New System.Drawing.Size(111, 46)
        Me.btnリセット.TabIndex = 13
        Me.btnリセット.Text = "リセット(&R)"
        Me.ToolTip1.SetToolTip(Me.btnリセット, "実測値として入力した値をすべてクリアし初期値に戻す")
        Me.btnリセット.UseVisualStyleBackColor = True
        '
        'btn係数取得
        '
        Me.btn係数取得.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn係数取得.Location = New System.Drawing.Point(129, 399)
        Me.btn係数取得.Name = "btn係数取得"
        Me.btn係数取得.Size = New System.Drawing.Size(111, 46)
        Me.btn係数取得.TabIndex = 14
        Me.btn係数取得.Text = "係数取得(&E)"
        Me.ToolTip1.SetToolTip(Me.btn係数取得, "入力した実測値をもとに係数を取得する")
        Me.btn係数取得.UseVisualStyleBackColor = True
        '
        'lbl設定時の寸法単位
        '
        Me.lbl設定時の寸法単位.AutoSize = True
        Me.lbl設定時の寸法単位.Location = New System.Drawing.Point(285, 40)
        Me.lbl設定時の寸法単位.Name = "lbl設定時の寸法単位"
        Me.lbl設定時の寸法単位.Size = New System.Drawing.Size(0, 20)
        Me.lbl設定時の寸法単位.TabIndex = 11
        '
        'lbl単位
        '
        Me.lbl単位.AutoSize = True
        Me.lbl単位.Location = New System.Drawing.Point(239, 40)
        Me.lbl単位.Name = "lbl単位"
        Me.lbl単位.Size = New System.Drawing.Size(42, 20)
        Me.lbl単位.TabIndex = 10
        Me.lbl単位.Text = "単位:"
        '
        'frmGauge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 454)
        Me.Controls.Add(Me.lbl設定時の寸法単位)
        Me.Controls.Add(Me.lbl単位)
        Me.Controls.Add(Me.btn係数取得)
        Me.Controls.Add(Me.btnリセット)
        Me.Controls.Add(Me.cmb対象バンドの種類名)
        Me.Controls.Add(Me.lbl対象バンドの種類名)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.dgvData)
        Me.Controls.Add(Me.lblコマ要尺係数b)
        Me.Controls.Add(Me.lblコマ要尺係数a)
        Me.Controls.Add(Me.lblコマ寸法係数b)
        Me.Controls.Add(Me.lblコマ寸法係数a)
        Me.Controls.Add(Me.txtコマ要尺係数b)
        Me.Controls.Add(Me.txtコマ要尺係数a)
        Me.Controls.Add(Me.txtコマ寸法係数b)
        Me.Controls.Add(Me.txtコマ寸法係数a)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGauge"
        Me.Text = "ゲージ"
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceゲージ, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtコマ寸法係数a As TextBox
    Friend WithEvents txtコマ寸法係数b As TextBox
    Friend WithEvents txtコマ要尺係数a As TextBox
    Friend WithEvents txtコマ要尺係数b As TextBox
    Friend WithEvents lblコマ寸法係数a As Label
    Friend WithEvents lblコマ寸法係数b As Label
    Friend WithEvents lblコマ要尺係数a As Label
    Friend WithEvents lblコマ要尺係数b As Label
    Friend WithEvents dgvData As DataGridView
    Friend WithEvents btnキャンセル As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents BindingSourceゲージ As BindingSource
    Friend WithEvents cmb対象バンドの種類名 As ComboBox
    Friend WithEvents lbl対象バンドの種類名 As Label
    Friend WithEvents btnリセット As Button
    Friend WithEvents btn係数取得 As Button
    Friend WithEvents f_d実測値使用区分 As DataGridViewCheckBoxColumn
    Friend WithEvents lbl設定時の寸法単位 As Label
    Friend WithEvents lbl単位 As Label
    Friend WithEvents f_sバンドの種類名 As DataGridViewTextBoxColumn
    Friend WithEvents f_i本幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_dコマ寸法計算値 As DataGridViewTextBoxColumn
    Friend WithEvents f_dコマ要尺計算値 As DataGridViewTextBoxColumn
    Friend WithEvents f_b係数取得区分 As DataGridViewCheckBoxColumn
    Friend WithEvents f_dコマ寸法実測値 As DataGridViewTextBoxColumn
    Friend WithEvents f_dコマ要尺実測値 As DataGridViewTextBoxColumn
    Friend WithEvents f_b実測値使用区分 As DataGridViewCheckBoxColumn
    Friend WithEvents f_s備考 As DataGridViewTextBoxColumn
    Friend WithEvents ToolTip1 As ToolTip
End Class
