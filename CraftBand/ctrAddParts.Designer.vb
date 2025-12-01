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
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Panel = New Windows.Forms.Panel()
        lbl長さ_単位 = New Windows.Forms.Label()
        nud長さ = New Windows.Forms.NumericUpDown()
        lbl長さ = New Windows.Forms.Label()
        lbl点数 = New Windows.Forms.Label()
        lbl付属品名 = New Windows.Forms.Label()
        nud点数 = New Windows.Forms.NumericUpDown()
        btn削除_追加品 = New Windows.Forms.Button()
        btn追加_追加品 = New Windows.Forms.Button()
        btn下へ_追加品 = New Windows.Forms.Button()
        btn上へ_追加品 = New Windows.Forms.Button()
        cmb付属品名 = New Windows.Forms.ComboBox()
        dgv追加品 = New ctrDataGridView()
        BindingSource追加品 = New System.Windows.Forms.BindingSource(components)
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        f_i番号3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s付属品名3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s付属品ひも名3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_iひも番号3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_b巻きひも区分3 = New Windows.Forms.DataGridViewCheckBoxColumn()
        f_i何本幅3 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_d長さ3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i長さ参照3 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_b集計対象外区分3 = New Windows.Forms.DataGridViewCheckBoxColumn()
        f_i点数3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s色3 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_dひも長3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d縦対横比率 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_dひも長加算3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_iひも本数3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d出力ひも長3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s記号3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_b描画区分3 = New Windows.Forms.DataGridViewCheckBoxColumn()
        f_b描画区分2 = New Windows.Forms.DataGridViewCheckBoxColumn()
        f_i描画位置3 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_d描画厚3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s座標 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i描画形状3 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_sメモ3 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i表示順 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i非表示順 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_bError3 = New Windows.Forms.DataGridViewCheckBoxColumn()
        Panel.SuspendLayout()
        CType(nud長さ, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud点数, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgv追加品, ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSource追加品, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Panel
        ' 
        Panel.Controls.Add(lbl長さ_単位)
        Panel.Controls.Add(nud長さ)
        Panel.Controls.Add(lbl長さ)
        Panel.Controls.Add(lbl点数)
        Panel.Controls.Add(lbl付属品名)
        Panel.Controls.Add(nud点数)
        Panel.Controls.Add(btn削除_追加品)
        Panel.Controls.Add(btn追加_追加品)
        Panel.Controls.Add(btn下へ_追加品)
        Panel.Controls.Add(btn上へ_追加品)
        Panel.Controls.Add(cmb付属品名)
        Panel.Controls.Add(dgv追加品)
        Panel.Enabled = False
        Panel.Location = New System.Drawing.Point(3, 3)
        Panel.Name = "Panel"
        Panel.Size = New System.Drawing.Size(840, 392)
        Panel.TabIndex = 0
        ' 
        ' lbl長さ_単位
        ' 
        lbl長さ_単位.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        lbl長さ_単位.AutoSize = True
        lbl長さ_単位.Location = New System.Drawing.Point(599, 329)
        lbl長さ_単位.Name = "lbl長さ_単位"
        lbl長さ_単位.Size = New System.Drawing.Size(37, 19)
        lbl長さ_単位.TabIndex = 20
        lbl長さ_単位.Text = "単位"
        ' 
        ' nud長さ
        ' 
        nud長さ.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        nud長さ.DecimalPlaces = 2
        nud長さ.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        nud長さ.Location = New System.Drawing.Point(559, 358)
        nud長さ.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        nud長さ.Name = "nud長さ"
        nud長さ.Size = New System.Drawing.Size(86, 26)
        nud長さ.TabIndex = 19
        ToolTip1.SetToolTip(nud長さ, "付属品の寸法")
        ' 
        ' lbl長さ
        ' 
        lbl長さ.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        lbl長さ.AutoSize = True
        lbl長さ.Location = New System.Drawing.Point(559, 329)
        lbl長さ.Name = "lbl長さ"
        lbl長さ.Size = New System.Drawing.Size(32, 19)
        lbl長さ.TabIndex = 18
        lbl長さ.Text = "長さ"
        ' 
        ' lbl点数
        ' 
        lbl点数.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        lbl点数.AutoSize = True
        lbl点数.Location = New System.Drawing.Point(651, 329)
        lbl点数.Name = "lbl点数"
        lbl点数.Size = New System.Drawing.Size(37, 19)
        lbl点数.TabIndex = 21
        lbl点数.Text = "点数"
        ' 
        ' lbl付属品名
        ' 
        lbl付属品名.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        lbl付属品名.AutoSize = True
        lbl付属品名.Location = New System.Drawing.Point(369, 329)
        lbl付属品名.Name = "lbl付属品名"
        lbl付属品名.Size = New System.Drawing.Size(65, 19)
        lbl付属品名.TabIndex = 16
        lbl付属品名.Text = "付属品名"
        ' 
        ' nud点数
        ' 
        nud点数.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        nud点数.Location = New System.Drawing.Point(651, 358)
        nud点数.Name = "nud点数"
        nud点数.Size = New System.Drawing.Size(60, 26)
        nud点数.TabIndex = 22
        ToolTip1.SetToolTip(nud点数, "追加する点数")
        nud点数.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' btn削除_追加品
        ' 
        btn削除_追加品.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn削除_追加品.Location = New System.Drawing.Point(243, 340)
        btn削除_追加品.Name = "btn削除_追加品"
        btn削除_追加品.Size = New System.Drawing.Size(111, 44)
        btn削除_追加品.TabIndex = 15
        btn削除_追加品.Text = "削除(&R)"
        ToolTip1.SetToolTip(btn削除_追加品, "選択した行を削除します")
        btn削除_追加品.UseVisualStyleBackColor = True
        ' 
        ' btn追加_追加品
        ' 
        btn追加_追加品.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn追加_追加品.Location = New System.Drawing.Point(722, 340)
        btn追加_追加品.Name = "btn追加_追加品"
        btn追加_追加品.Size = New System.Drawing.Size(111, 44)
        btn追加_追加品.TabIndex = 23
        btn追加_追加品.Text = "追加(&A)"
        ToolTip1.SetToolTip(btn追加_追加品, "付属品を追加します")
        btn追加_追加品.UseVisualStyleBackColor = True
        ' 
        ' btn下へ_追加品
        ' 
        btn下へ_追加品.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn下へ_追加品.Location = New System.Drawing.Point(126, 340)
        btn下へ_追加品.Name = "btn下へ_追加品"
        btn下へ_追加品.Size = New System.Drawing.Size(111, 44)
        btn下へ_追加品.TabIndex = 14
        btn下へ_追加品.Text = "下へ(&D)"
        ToolTip1.SetToolTip(btn下へ_追加品, "選択した行を下に移動します")
        btn下へ_追加品.UseVisualStyleBackColor = True
        ' 
        ' btn上へ_追加品
        ' 
        btn上へ_追加品.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn上へ_追加品.Location = New System.Drawing.Point(9, 340)
        btn上へ_追加品.Name = "btn上へ_追加品"
        btn上へ_追加品.Size = New System.Drawing.Size(111, 44)
        btn上へ_追加品.TabIndex = 13
        btn上へ_追加品.Text = "上へ(&U)"
        ToolTip1.SetToolTip(btn上へ_追加品, "選択した行を上に移動します")
        btn上へ_追加品.UseVisualStyleBackColor = True
        ' 
        ' cmb付属品名
        ' 
        cmb付属品名.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        cmb付属品名.FormattingEnabled = True
        cmb付属品名.Location = New System.Drawing.Point(369, 357)
        cmb付属品名.Name = "cmb付属品名"
        cmb付属品名.Size = New System.Drawing.Size(184, 27)
        cmb付属品名.TabIndex = 17
        ToolTip1.SetToolTip(cmb付属品名, "追加したい付属品名を選択します")
        ' 
        ' dgv追加品
        ' 
        dgv追加品.AllowUserToAddRows = False
        dgv追加品.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgv追加品.AutoGenerateColumns = False
        dgv追加品.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgv追加品.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgv追加品.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {f_i番号3, f_s付属品名3, f_s付属品ひも名3, f_iひも番号3, f_b巻きひも区分3, f_i何本幅3, f_d長さ3, f_i長さ参照3, f_b集計対象外区分3, f_i点数3, f_s色3, f_dひも長3, f_d縦対横比率, f_dひも長加算3, f_iひも本数3, f_d出力ひも長3, f_s記号3, f_b描画区分3, f_b描画区分2, f_i描画位置3, f_d描画厚3, f_s座標, f_i描画形状3, f_sメモ3, f_i表示順, f_i非表示順, f_bError3})
        dgv追加品.DataSource = BindingSource追加品
        dgv追加品.Location = New System.Drawing.Point(6, 8)
        dgv追加品.Name = "dgv追加品"
        dgv追加品.RowHeadersWidth = 51
        dgv追加品.RowTemplate.Height = 29
        dgv追加品.Size = New System.Drawing.Size(828, 311)
        dgv追加品.TabIndex = 12
        ' 
        ' BindingSource追加品
        ' 
        BindingSource追加品.DataMember = "tbl追加品"
        BindingSource追加品.DataSource = GetType(Tables.dstDataTables)
        ' 
        ' f_i番号3
        ' 
        f_i番号3.DataPropertyName = "f_i番号"
        DataGridViewCellStyle1.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i番号3.DefaultCellStyle = DataGridViewCellStyle1
        f_i番号3.HeaderText = "番号"
        f_i番号3.MinimumWidth = 6
        f_i番号3.Name = "f_i番号3"
        f_i番号3.ReadOnly = True
        f_i番号3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i番号3.Width = 67
        ' 
        ' f_s付属品名3
        ' 
        f_s付属品名3.DataPropertyName = "f_s付属品名"
        f_s付属品名3.HeaderText = "付属品名"
        f_s付属品名3.MinimumWidth = 6
        f_s付属品名3.Name = "f_s付属品名3"
        f_s付属品名3.ReadOnly = True
        f_s付属品名3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_s付属品名3.Width = 125
        ' 
        ' f_s付属品ひも名3
        ' 
        f_s付属品ひも名3.DataPropertyName = "f_s付属品ひも名"
        f_s付属品ひも名3.HeaderText = "付属品ひも名"
        f_s付属品ひも名3.MinimumWidth = 6
        f_s付属品ひも名3.Name = "f_s付属品ひも名3"
        f_s付属品ひも名3.ReadOnly = True
        f_s付属品ひも名3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_s付属品ひも名3.Width = 125
        ' 
        ' f_iひも番号3
        ' 
        f_iひも番号3.DataPropertyName = "f_iひも番号"
        DataGridViewCellStyle2.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_iひも番号3.DefaultCellStyle = DataGridViewCellStyle2
        f_iひも番号3.HeaderText = "ひも番号"
        f_iひも番号3.MinimumWidth = 6
        f_iひも番号3.Name = "f_iひも番号3"
        f_iひも番号3.ReadOnly = True
        f_iひも番号3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_iひも番号3.Width = 78
        ' 
        ' f_b巻きひも区分3
        ' 
        f_b巻きひも区分3.DataPropertyName = "f_b巻きひも区分"
        f_b巻きひも区分3.HeaderText = "巻きひも"
        f_b巻きひも区分3.MinimumWidth = 6
        f_b巻きひも区分3.Name = "f_b巻きひも区分3"
        f_b巻きひも区分3.ReadOnly = True
        f_b巻きひも区分3.ToolTipText = "先のひもに巻き付ける場合にチェック"
        f_b巻きひも区分3.Width = 76
        ' 
        ' f_i何本幅3
        ' 
        f_i何本幅3.DataPropertyName = "f_i何本幅"
        DataGridViewCellStyle3.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        f_i何本幅3.DefaultCellStyle = DataGridViewCellStyle3
        f_i何本幅3.HeaderText = "何本幅"
        f_i何本幅3.MinimumWidth = 6
        f_i何本幅3.Name = "f_i何本幅3"
        f_i何本幅3.Resizable = Windows.Forms.DataGridViewTriState.True
        f_i何本幅3.Width = 81
        ' 
        ' f_d長さ3
        ' 
        f_d長さ3.DataPropertyName = "f_d長さ"
        DataGridViewCellStyle4.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.NullValue = Nothing
        f_d長さ3.DefaultCellStyle = DataGridViewCellStyle4
        f_d長さ3.HeaderText = "長さ"
        f_d長さ3.MinimumWidth = 6
        f_d長さ3.Name = "f_d長さ3"
        f_d長さ3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d長さ3.ToolTipText = "計算のベースとなる値"
        f_d長さ3.Width = 111
        ' 
        ' f_i長さ参照3
        ' 
        f_i長さ参照3.DataPropertyName = "f_i長さ参照"
        f_i長さ参照3.HeaderText = "長さ参照"
        f_i長さ参照3.MinimumWidth = 6
        f_i長さ参照3.Name = "f_i長さ参照3"
        f_i長さ参照3.ReadOnly = True
        f_i長さ参照3.Resizable = Windows.Forms.DataGridViewTriState.True
        f_i長さ参照3.SortMode = Windows.Forms.DataGridViewColumnSortMode.Automatic
        f_i長さ参照3.ToolTipText = "計算寸法値を参照する場合に指定"
        f_i長さ参照3.Width = 125
        ' 
        ' f_b集計対象外区分3
        ' 
        f_b集計対象外区分3.DataPropertyName = "f_b集計対象外区分"
        f_b集計対象外区分3.HeaderText = "集計対象外"
        f_b集計対象外区分3.MinimumWidth = 6
        f_b集計対象外区分3.Name = "f_b集計対象外区分3"
        f_b集計対象外区分3.ToolTipText = "本幅ベースの集計から除外する場合にチェック"
        f_b集計対象外区分3.Width = 125
        ' 
        ' f_i点数3
        ' 
        f_i点数3.DataPropertyName = "f_i点数"
        DataGridViewCellStyle5.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i点数3.DefaultCellStyle = DataGridViewCellStyle5
        f_i点数3.HeaderText = "点数"
        f_i点数3.MinimumWidth = 6
        f_i点数3.Name = "f_i点数3"
        f_i点数3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i点数3.Width = 58
        ' 
        ' f_s色3
        ' 
        f_s色3.DataPropertyName = "f_s色"
        f_s色3.HeaderText = "色"
        f_s色3.MinimumWidth = 6
        f_s色3.Name = "f_s色3"
        f_s色3.Resizable = Windows.Forms.DataGridViewTriState.True
        f_s色3.Width = 74
        ' 
        ' f_dひも長3
        ' 
        f_dひも長3.DataPropertyName = "f_dひも長"
        DataGridViewCellStyle6.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N2"
        DataGridViewCellStyle6.NullValue = Nothing
        f_dひも長3.DefaultCellStyle = DataGridViewCellStyle6
        f_dひも長3.HeaderText = "ひも長"
        f_dひも長3.MinimumWidth = 6
        f_dひも長3.Name = "f_dひも長3"
        f_dひも長3.ReadOnly = True
        f_dひも長3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_dひも長3.ToolTipText = "長さをもとに、付属品に設定した係数で計算した値"
        f_dひも長3.Width = 125
        ' 
        ' f_d縦対横比率
        ' 
        f_d縦対横比率.DataPropertyName = "f_d縦対横比率"
        DataGridViewCellStyle7.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "N2"
        DataGridViewCellStyle7.NullValue = Nothing
        f_d縦対横比率.DefaultCellStyle = DataGridViewCellStyle7
        f_d縦対横比率.HeaderText = "f_d縦対横比率"
        f_d縦対横比率.MinimumWidth = 6
        f_d縦対横比率.Name = "f_d縦対横比率"
        f_d縦対横比率.Visible = False
        f_d縦対横比率.Width = 125
        ' 
        ' f_dひも長加算3
        ' 
        f_dひも長加算3.DataPropertyName = "f_dひも長加算"
        DataGridViewCellStyle8.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_dひも長加算3.DefaultCellStyle = DataGridViewCellStyle8
        f_dひも長加算3.HeaderText = "ひも長加算"
        f_dひも長加算3.MinimumWidth = 6
        f_dひも長加算3.Name = "f_dひも長加算3"
        f_dひも長加算3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_dひも長加算3.ToolTipText = "出力時に加える余裕長"
        f_dひも長加算3.Width = 125
        ' 
        ' f_iひも本数3
        ' 
        f_iひも本数3.DataPropertyName = "f_iひも本数"
        DataGridViewCellStyle9.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_iひも本数3.DefaultCellStyle = DataGridViewCellStyle9
        f_iひも本数3.HeaderText = "ひも本数"
        f_iひも本数3.MinimumWidth = 6
        f_iひも本数3.Name = "f_iひも本数3"
        f_iひも本数3.ReadOnly = True
        f_iひも本数3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_iひも本数3.Width = 125
        ' 
        ' f_d出力ひも長3
        ' 
        f_d出力ひも長3.DataPropertyName = "f_d出力ひも長"
        DataGridViewCellStyle10.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_d出力ひも長3.DefaultCellStyle = DataGridViewCellStyle10
        f_d出力ひも長3.HeaderText = "出力ひも長"
        f_d出力ひも長3.MinimumWidth = 6
        f_d出力ひも長3.Name = "f_d出力ひも長3"
        f_d出力ひも長3.ReadOnly = True
        f_d出力ひも長3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d出力ひも長3.ToolTipText = "ひも長加算を加えた出力長"
        f_d出力ひも長3.Width = 125
        ' 
        ' f_s記号3
        ' 
        f_s記号3.DataPropertyName = "f_s記号"
        f_s記号3.HeaderText = "f_s記号"
        f_s記号3.MinimumWidth = 6
        f_s記号3.Name = "f_s記号3"
        f_s記号3.Visible = False
        f_s記号3.Width = 125
        ' 
        ' f_b描画区分3
        ' 
        f_b描画区分3.DataPropertyName = "f_b描画区分"
        f_b描画区分3.HeaderText = "描画"
        f_b描画区分3.MinimumWidth = 6
        f_b描画区分3.Name = "f_b描画区分3"
        f_b描画区分3.ToolTipText = "プレビュー図に描画する場合チェック"
        f_b描画区分3.Width = 125
        ' 
        ' f_b描画区分2
        ' 
        f_b描画区分2.DataPropertyName = "f_b描画区分2"
        f_b描画区分2.HeaderText = "描画2"
        f_b描画区分2.Name = "f_b描画区分2"
        f_b描画区分2.ToolTipText = "プレビュー2図に描画する場合チェック"
        f_b描画区分2.Visible = False
        ' 
        ' f_i描画位置3
        ' 
        f_i描画位置3.DataPropertyName = "f_i描画位置"
        f_i描画位置3.HeaderText = "描画位置"
        f_i描画位置3.MinimumWidth = 6
        f_i描画位置3.Name = "f_i描画位置3"
        f_i描画位置3.Resizable = Windows.Forms.DataGridViewTriState.True
        f_i描画位置3.ToolTipText = "画像に描画する場合はその位置を指定"
        f_i描画位置3.Width = 125
        ' 
        ' f_d描画厚3
        ' 
        f_d描画厚3.DataPropertyName = "f_d描画厚"
        DataGridViewCellStyle11.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_d描画厚3.DefaultCellStyle = DataGridViewCellStyle11
        f_d描画厚3.HeaderText = "描画厚"
        f_d描画厚3.MinimumWidth = 6
        f_d描画厚3.Name = "f_d描画厚3"
        f_d描画厚3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d描画厚3.ToolTipText = "画像に描画する場合の厚さ(幅)"
        f_d描画厚3.Width = 125
        ' 
        ' f_s座標
        ' 
        f_s座標.DataPropertyName = "f_s座標"
        f_s座標.HeaderText = "座標"
        f_s座標.Name = "f_s座標"
        f_s座標.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_s座標.ToolTipText = "x , y の形式で指定してください"
        f_s座標.Visible = False
        ' 
        ' f_i描画形状3
        ' 
        f_i描画形状3.DataPropertyName = "f_i描画形状"
        f_i描画形状3.HeaderText = "描画形状"
        f_i描画形状3.MinimumWidth = 6
        f_i描画形状3.Name = "f_i描画形状3"
        f_i描画形状3.ReadOnly = True
        f_i描画形状3.Resizable = Windows.Forms.DataGridViewTriState.True
        f_i描画形状3.ToolTipText = "描画する形状"
        f_i描画形状3.Visible = False
        f_i描画形状3.Width = 125
        ' 
        ' f_sメモ3
        ' 
        f_sメモ3.DataPropertyName = "f_sメモ"
        f_sメモ3.HeaderText = "メモ"
        f_sメモ3.MinimumWidth = 6
        f_sメモ3.Name = "f_sメモ3"
        f_sメモ3.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_sメモ3.Width = 125
        ' 
        ' f_i表示順
        ' 
        f_i表示順.DataPropertyName = "f_i表示順"
        DataGridViewCellStyle12.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i表示順.DefaultCellStyle = DataGridViewCellStyle12
        f_i表示順.HeaderText = "表示順"
        f_i表示順.Name = "f_i表示順"
        f_i表示順.Visible = False
        ' 
        ' f_i非表示順
        ' 
        f_i非表示順.DataPropertyName = "f_i非表示順"
        DataGridViewCellStyle13.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i非表示順.DefaultCellStyle = DataGridViewCellStyle13
        f_i非表示順.HeaderText = "非表示順"
        f_i非表示順.Name = "f_i非表示順"
        f_i非表示順.Visible = False
        ' 
        ' f_bError3
        ' 
        f_bError3.DataPropertyName = "f_bError"
        f_bError3.HeaderText = "Error"
        f_bError3.MinimumWidth = 6
        f_bError3.Name = "f_bError3"
        f_bError3.Visible = False
        f_bError3.Width = 125
        ' 
        ' ctrAddParts
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
        Controls.Add(Panel)
        Name = "ctrAddParts"
        Size = New System.Drawing.Size(872, 431)
        Panel.ResumeLayout(False)
        Panel.PerformLayout()
        CType(nud長さ, ComponentModel.ISupportInitialize).EndInit()
        CType(nud点数, ComponentModel.ISupportInitialize).EndInit()
        CType(dgv追加品, ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSource追加品, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

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
    Friend WithEvents f_i長さ参照3 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_b集計対象外区分3 As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents f_i点数3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s色3 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_dひも長3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d縦対横比率 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_iひも本数3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d出力ひも長3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s記号3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_b描画区分3 As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents f_b描画区分2 As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents f_i描画位置3 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_d描画厚3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s座標 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i描画形状3 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_sメモ3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i表示順 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i非表示順 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_bError3 As Windows.Forms.DataGridViewCheckBoxColumn
End Class
