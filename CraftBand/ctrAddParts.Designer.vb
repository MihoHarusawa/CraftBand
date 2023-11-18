<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrAddParts
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel = New System.Windows.Forms.Panel()
        Me.lbl長さ_単位 = New System.Windows.Forms.Label()
        Me.nud長さ = New System.Windows.Forms.NumericUpDown()
        Me.lbl長さ = New System.Windows.Forms.Label()
        Me.lbl点数 = New System.Windows.Forms.Label()
        Me.lbl付属品名 = New System.Windows.Forms.Label()
        Me.nud点数 = New System.Windows.Forms.NumericUpDown()
        Me.btn削除_追加品 = New System.Windows.Forms.Button()
        Me.btn追加_追加品 = New System.Windows.Forms.Button()
        Me.btn下へ_追加品 = New System.Windows.Forms.Button()
        Me.btn上へ_追加品 = New System.Windows.Forms.Button()
        Me.cmb付属品名 = New System.Windows.Forms.ComboBox()
        Me.dgv追加品 = New CraftBand.ctrDataGridView()
        Me.f_i番号3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s付属品名3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s付属品ひも名3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_iひも番号3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_b巻きひも区分3 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_i何本幅3 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.f_d長さ3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_i点数3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s色3 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.f_dひも長3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長加算3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_iひも本数3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s記号3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_sメモ3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_bError3 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.BindingSource追加品 = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel.SuspendLayout()
        CType(Me.nud長さ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nud点数, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv追加品, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource追加品, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel
        '
        Me.Panel.Controls.Add(Me.lbl長さ_単位)
        Me.Panel.Controls.Add(Me.nud長さ)
        Me.Panel.Controls.Add(Me.lbl長さ)
        Me.Panel.Controls.Add(Me.lbl点数)
        Me.Panel.Controls.Add(Me.lbl付属品名)
        Me.Panel.Controls.Add(Me.nud点数)
        Me.Panel.Controls.Add(Me.btn削除_追加品)
        Me.Panel.Controls.Add(Me.btn追加_追加品)
        Me.Panel.Controls.Add(Me.btn下へ_追加品)
        Me.Panel.Controls.Add(Me.btn上へ_追加品)
        Me.Panel.Controls.Add(Me.cmb付属品名)
        Me.Panel.Controls.Add(Me.dgv追加品)
        Me.Panel.Enabled = False
        Me.Panel.Location = New System.Drawing.Point(3, 3)
        Me.Panel.Name = "Panel"
        Me.Panel.Size = New System.Drawing.Size(840, 413)
        Me.Panel.TabIndex = 0
        '
        'lbl長さ_単位
        '
        Me.lbl長さ_単位.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl長さ_単位.AutoSize = True
        Me.lbl長さ_単位.Location = New System.Drawing.Point(599, 346)
        Me.lbl長さ_単位.Name = "lbl長さ_単位"
        Me.lbl長さ_単位.Size = New System.Drawing.Size(39, 20)
        Me.lbl長さ_単位.TabIndex = 20
        Me.lbl長さ_単位.Text = "単位"
        '
        'nud長さ
        '
        Me.nud長さ.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nud長さ.DecimalPlaces = 2
        Me.nud長さ.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nud長さ.Location = New System.Drawing.Point(559, 377)
        Me.nud長さ.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        Me.nud長さ.Name = "nud長さ"
        Me.nud長さ.Size = New System.Drawing.Size(86, 27)
        Me.nud長さ.TabIndex = 19
        Me.ToolTip1.SetToolTip(Me.nud長さ, "付属品の寸法")
        '
        'lbl長さ
        '
        Me.lbl長さ.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl長さ.AutoSize = True
        Me.lbl長さ.Location = New System.Drawing.Point(559, 346)
        Me.lbl長さ.Name = "lbl長さ"
        Me.lbl長さ.Size = New System.Drawing.Size(34, 20)
        Me.lbl長さ.TabIndex = 18
        Me.lbl長さ.Text = "長さ"
        '
        'lbl点数
        '
        Me.lbl点数.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl点数.AutoSize = True
        Me.lbl点数.Location = New System.Drawing.Point(651, 346)
        Me.lbl点数.Name = "lbl点数"
        Me.lbl点数.Size = New System.Drawing.Size(39, 20)
        Me.lbl点数.TabIndex = 21
        Me.lbl点数.Text = "点数"
        '
        'lbl付属品名
        '
        Me.lbl付属品名.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl付属品名.AutoSize = True
        Me.lbl付属品名.Location = New System.Drawing.Point(369, 346)
        Me.lbl付属品名.Name = "lbl付属品名"
        Me.lbl付属品名.Size = New System.Drawing.Size(69, 20)
        Me.lbl付属品名.TabIndex = 16
        Me.lbl付属品名.Text = "付属品名"
        '
        'nud点数
        '
        Me.nud点数.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nud点数.Location = New System.Drawing.Point(651, 377)
        Me.nud点数.Name = "nud点数"
        Me.nud点数.Size = New System.Drawing.Size(60, 27)
        Me.nud点数.TabIndex = 22
        Me.ToolTip1.SetToolTip(Me.nud点数, "何点追加するか")
        Me.nud点数.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'btn削除_追加品
        '
        Me.btn削除_追加品.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn削除_追加品.Location = New System.Drawing.Point(243, 358)
        Me.btn削除_追加品.Name = "btn削除_追加品"
        Me.btn削除_追加品.Size = New System.Drawing.Size(111, 46)
        Me.btn削除_追加品.TabIndex = 15
        Me.btn削除_追加品.Text = "削除(&R)"
        Me.btn削除_追加品.UseVisualStyleBackColor = True
        '
        'btn追加_追加品
        '
        Me.btn追加_追加品.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn追加_追加品.Location = New System.Drawing.Point(722, 358)
        Me.btn追加_追加品.Name = "btn追加_追加品"
        Me.btn追加_追加品.Size = New System.Drawing.Size(111, 46)
        Me.btn追加_追加品.TabIndex = 23
        Me.btn追加_追加品.Text = "追加(&A)"
        Me.btn追加_追加品.UseVisualStyleBackColor = True
        '
        'btn下へ_追加品
        '
        Me.btn下へ_追加品.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn下へ_追加品.Location = New System.Drawing.Point(126, 358)
        Me.btn下へ_追加品.Name = "btn下へ_追加品"
        Me.btn下へ_追加品.Size = New System.Drawing.Size(111, 46)
        Me.btn下へ_追加品.TabIndex = 14
        Me.btn下へ_追加品.Text = "下へ(&D)"
        Me.btn下へ_追加品.UseVisualStyleBackColor = True
        '
        'btn上へ_追加品
        '
        Me.btn上へ_追加品.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn上へ_追加品.Location = New System.Drawing.Point(9, 358)
        Me.btn上へ_追加品.Name = "btn上へ_追加品"
        Me.btn上へ_追加品.Size = New System.Drawing.Size(111, 46)
        Me.btn上へ_追加品.TabIndex = 13
        Me.btn上へ_追加品.Text = "上へ(&U)"
        Me.btn上へ_追加品.UseVisualStyleBackColor = True
        '
        'cmb付属品名
        '
        Me.cmb付属品名.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmb付属品名.FormattingEnabled = True
        Me.cmb付属品名.Location = New System.Drawing.Point(369, 376)
        Me.cmb付属品名.Name = "cmb付属品名"
        Me.cmb付属品名.Size = New System.Drawing.Size(184, 28)
        Me.cmb付属品名.TabIndex = 17
        '
        'dgv追加品
        '
        Me.dgv追加品.AllowUserToAddRows = False
        Me.dgv追加品.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgv追加品.AutoGenerateColumns = False
        Me.dgv追加品.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgv追加品.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv追加品.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.f_i番号3, Me.f_s付属品名3, Me.f_s付属品ひも名3, Me.f_iひも番号3, Me.f_b巻きひも区分3, Me.f_i何本幅3, Me.f_d長さ3, Me.f_i点数3, Me.f_s色3, Me.f_dひも長3, Me.f_dひも長加算3, Me.f_iひも本数3, Me.f_s記号3, Me.f_sメモ3, Me.f_bError3})
        Me.dgv追加品.DataSource = Me.BindingSource追加品
        Me.dgv追加品.Location = New System.Drawing.Point(6, 8)
        Me.dgv追加品.Name = "dgv追加品"
        Me.dgv追加品.RowHeadersWidth = 51
        Me.dgv追加品.RowTemplate.Height = 29
        Me.dgv追加品.Size = New System.Drawing.Size(828, 327)
        Me.dgv追加品.TabIndex = 12
        '
        'f_i番号3
        '
        Me.f_i番号3.DataPropertyName = "f_i番号"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_i番号3.DefaultCellStyle = DataGridViewCellStyle9
        Me.f_i番号3.HeaderText = "番号"
        Me.f_i番号3.MinimumWidth = 6
        Me.f_i番号3.Name = "f_i番号3"
        Me.f_i番号3.ReadOnly = True
        Me.f_i番号3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_i番号3.Width = 67
        '
        'f_s付属品名3
        '
        Me.f_s付属品名3.DataPropertyName = "f_s付属品名"
        Me.f_s付属品名3.HeaderText = "付属品名"
        Me.f_s付属品名3.MinimumWidth = 6
        Me.f_s付属品名3.Name = "f_s付属品名3"
        Me.f_s付属品名3.ReadOnly = True
        Me.f_s付属品名3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s付属品名3.Width = 125
        '
        'f_s付属品ひも名3
        '
        Me.f_s付属品ひも名3.DataPropertyName = "f_s付属品ひも名"
        Me.f_s付属品ひも名3.HeaderText = "付属品ひも名"
        Me.f_s付属品ひも名3.MinimumWidth = 6
        Me.f_s付属品ひも名3.Name = "f_s付属品ひも名3"
        Me.f_s付属品ひも名3.ReadOnly = True
        Me.f_s付属品ひも名3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s付属品ひも名3.Width = 125
        '
        'f_iひも番号3
        '
        Me.f_iひも番号3.DataPropertyName = "f_iひも番号"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_iひも番号3.DefaultCellStyle = DataGridViewCellStyle10
        Me.f_iひも番号3.HeaderText = "ひも番号"
        Me.f_iひも番号3.MinimumWidth = 6
        Me.f_iひも番号3.Name = "f_iひも番号3"
        Me.f_iひも番号3.ReadOnly = True
        Me.f_iひも番号3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_iひも番号3.Width = 78
        '
        'f_b巻きひも区分3
        '
        Me.f_b巻きひも区分3.DataPropertyName = "f_b巻きひも区分"
        Me.f_b巻きひも区分3.HeaderText = "巻きひも"
        Me.f_b巻きひも区分3.MinimumWidth = 6
        Me.f_b巻きひも区分3.Name = "f_b巻きひも区分3"
        Me.f_b巻きひも区分3.ReadOnly = True
        Me.f_b巻きひも区分3.Width = 76
        '
        'f_i何本幅3
        '
        Me.f_i何本幅3.DataPropertyName = "f_i何本幅"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.f_i何本幅3.DefaultCellStyle = DataGridViewCellStyle11
        Me.f_i何本幅3.HeaderText = "何本幅"
        Me.f_i何本幅3.MinimumWidth = 6
        Me.f_i何本幅3.Name = "f_i何本幅3"
        Me.f_i何本幅3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.f_i何本幅3.Width = 81
        '
        'f_d長さ3
        '
        Me.f_d長さ3.DataPropertyName = "f_d長さ"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle12.NullValue = Nothing
        Me.f_d長さ3.DefaultCellStyle = DataGridViewCellStyle12
        Me.f_d長さ3.HeaderText = "長さ"
        Me.f_d長さ3.MinimumWidth = 6
        Me.f_d長さ3.Name = "f_d長さ3"
        Me.f_d長さ3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_d長さ3.Width = 111
        '
        'f_i点数3
        '
        Me.f_i点数3.DataPropertyName = "f_i点数"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_i点数3.DefaultCellStyle = DataGridViewCellStyle13
        Me.f_i点数3.HeaderText = "点数"
        Me.f_i点数3.MinimumWidth = 6
        Me.f_i点数3.Name = "f_i点数3"
        Me.f_i点数3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_i点数3.Width = 58
        '
        'f_s色3
        '
        Me.f_s色3.DataPropertyName = "f_s色"
        Me.f_s色3.HeaderText = "色"
        Me.f_s色3.MinimumWidth = 6
        Me.f_s色3.Name = "f_s色3"
        Me.f_s色3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.f_s色3.Width = 74
        '
        'f_dひも長3
        '
        Me.f_dひも長3.DataPropertyName = "f_dひも長"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle14.Format = "N2"
        DataGridViewCellStyle14.NullValue = Nothing
        Me.f_dひも長3.DefaultCellStyle = DataGridViewCellStyle14
        Me.f_dひも長3.HeaderText = "ひも長"
        Me.f_dひも長3.MinimumWidth = 6
        Me.f_dひも長3.Name = "f_dひも長3"
        Me.f_dひも長3.ReadOnly = True
        Me.f_dひも長3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_dひも長3.Width = 125
        '
        'f_dひも長加算3
        '
        Me.f_dひも長加算3.DataPropertyName = "f_dひも長加算"
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長加算3.DefaultCellStyle = DataGridViewCellStyle15
        Me.f_dひも長加算3.HeaderText = "ひも長加算"
        Me.f_dひも長加算3.MinimumWidth = 6
        Me.f_dひも長加算3.Name = "f_dひも長加算3"
        Me.f_dひも長加算3.ToolTipText = "出力時に加える余裕長"
        Me.f_dひも長加算3.Width = 125
        '
        'f_iひも本数3
        '
        Me.f_iひも本数3.DataPropertyName = "f_iひも本数"
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_iひも本数3.DefaultCellStyle = DataGridViewCellStyle16
        Me.f_iひも本数3.HeaderText = "ひも本数"
        Me.f_iひも本数3.MinimumWidth = 6
        Me.f_iひも本数3.Name = "f_iひも本数3"
        Me.f_iひも本数3.ReadOnly = True
        Me.f_iひも本数3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_iひも本数3.Width = 125
        '
        'f_s記号3
        '
        Me.f_s記号3.DataPropertyName = "f_s記号"
        Me.f_s記号3.HeaderText = "f_s記号"
        Me.f_s記号3.MinimumWidth = 6
        Me.f_s記号3.Name = "f_s記号3"
        Me.f_s記号3.Visible = False
        Me.f_s記号3.Width = 125
        '
        'f_sメモ3
        '
        Me.f_sメモ3.DataPropertyName = "f_sメモ"
        Me.f_sメモ3.HeaderText = "メモ"
        Me.f_sメモ3.MinimumWidth = 6
        Me.f_sメモ3.Name = "f_sメモ3"
        Me.f_sメモ3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_sメモ3.Width = 125
        '
        'f_bError3
        '
        Me.f_bError3.DataPropertyName = "f_bError"
        Me.f_bError3.HeaderText = "Error"
        Me.f_bError3.MinimumWidth = 6
        Me.f_bError3.Name = "f_bError3"
        Me.f_bError3.Visible = False
        Me.f_bError3.Width = 125
        '
        'BindingSource追加品
        '
        Me.BindingSource追加品.DataMember = "tbl追加品"
        Me.BindingSource追加品.DataSource = GetType(CraftBand.Tables.dstDataTables)
        '
        'ctrAddParts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.Panel)
        Me.Name = "ctrAddParts"
        Me.Size = New System.Drawing.Size(872, 454)
        Me.Panel.ResumeLayout(False)
        Me.Panel.PerformLayout()
        CType(Me.nud長さ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nud点数, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv追加品, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource追加品, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel As Windows.Forms.Panel
    Friend WithEvents BindingSource追加品 As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents lbl長さ_単位 As Windows.Forms.Label
    Friend WithEvents nud長さ As Windows.Forms.NumericUpDown
    Friend WithEvents lbl長さ As Windows.Forms.Label
    Friend WithEvents lbl点数 As Windows.Forms.Label
    Friend WithEvents lbl付属品名 As Windows.Forms.Label
    Friend WithEvents nud点数 As Windows.Forms.NumericUpDown
    Friend WithEvents btn削除_追加品 As Windows.Forms.Button
    Friend WithEvents btn追加_追加品 As Windows.Forms.Button
    Friend WithEvents btn下へ_追加品 As Windows.Forms.Button
    Friend WithEvents btn上へ_追加品 As Windows.Forms.Button
    Friend WithEvents cmb付属品名 As Windows.Forms.ComboBox
    Friend WithEvents dgv追加品 As ctrDataGridView
    Friend WithEvents f_i番号3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s付属品名3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s付属品ひも名3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_iひも番号3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_b巻きひも区分3 As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents f_i何本幅3 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_d長さ3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i点数3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s色3 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_dひも長3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_iひも本数3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s記号3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_sメモ3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_bError3 As Windows.Forms.DataGridViewCheckBoxColumn
End Class
