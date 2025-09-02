<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmColor
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
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        BindingSource描画色 = New System.Windows.Forms.BindingSource(components)
        dgvData = New ctrDataGridView()
        Fs色DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        fsバンドの種類名ComboBoxColumn = New Windows.Forms.DataGridViewComboBoxColumn()
        Fi赤DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        Fi緑DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        Fi青DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        disp = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i透明度 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d線幅 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s線色 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d中線幅 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s中線色 = New Windows.Forms.DataGridViewTextBoxColumn()
        Texture = New Windows.Forms.DataGridViewImageColumn()
        f_s画像情報 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s画像文字列 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s製品情報 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s備考 = New Windows.Forms.DataGridViewTextBoxColumn()
        btnキャンセル = New Windows.Forms.Button()
        btnOK = New Windows.Forms.Button()
        txt色 = New Windows.Forms.TextBox()
        nud赤 = New Windows.Forms.NumericUpDown()
        nud緑 = New Windows.Forms.NumericUpDown()
        nud青 = New Windows.Forms.NumericUpDown()
        lbl色 = New Windows.Forms.Label()
        lbl赤 = New Windows.Forms.Label()
        lbl緑 = New Windows.Forms.Label()
        lbl青 = New Windows.Forms.Label()
        btn追加 = New Windows.Forms.Button()
        lblColor = New Windows.Forms.Label()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        cmbバンドの種類名 = New Windows.Forms.ComboBox()
        btn画像 = New Windows.Forms.Button()
        lblバンドの種類名 = New Windows.Forms.Label()
        OpenFileDialogPng = New Windows.Forms.OpenFileDialog()
        CType(BindingSource描画色, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvData, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud赤, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud緑, ComponentModel.ISupportInitialize).BeginInit()
        CType(nud青, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' BindingSource描画色
        ' 
        BindingSource描画色.DataMember = "tbl描画色"
        BindingSource描画色.DataSource = GetType(Tables.dstMasterTables)
        ' 
        ' dgvData
        ' 
        dgvData.AllowUserToAddRows = False
        dgvData.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgvData.AutoGenerateColumns = False
        dgvData.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvData.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvData.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {Fs色DataGridViewTextBoxColumn, fsバンドの種類名ComboBoxColumn, Fi赤DataGridViewTextBoxColumn, Fi緑DataGridViewTextBoxColumn, Fi青DataGridViewTextBoxColumn, disp, f_i透明度, f_d線幅, f_s線色, f_d中線幅, f_s中線色, Texture, f_s画像情報, f_s画像文字列, f_s製品情報, f_s備考})
        dgvData.DataSource = BindingSource描画色
        dgvData.Location = New System.Drawing.Point(12, 11)
        dgvData.Name = "dgvData"
        dgvData.RowHeadersWidth = 51
        dgvData.RowTemplate.Height = 29
        dgvData.Size = New System.Drawing.Size(1010, 122)
        dgvData.TabIndex = 0
        ' 
        ' Fs色DataGridViewTextBoxColumn
        ' 
        Fs色DataGridViewTextBoxColumn.DataPropertyName = "f_s色"
        Fs色DataGridViewTextBoxColumn.HeaderText = "色"
        Fs色DataGridViewTextBoxColumn.MinimumWidth = 6
        Fs色DataGridViewTextBoxColumn.Name = "Fs色DataGridViewTextBoxColumn"
        Fs色DataGridViewTextBoxColumn.ToolTipText = "色の名前"
        Fs色DataGridViewTextBoxColumn.Width = 125
        ' 
        ' fsバンドの種類名ComboBoxColumn
        ' 
        fsバンドの種類名ComboBoxColumn.DataPropertyName = "f_sバンドの種類名"
        fsバンドの種類名ComboBoxColumn.HeaderText = "バンドの種類名"
        fsバンドの種類名ComboBoxColumn.MinimumWidth = 6
        fsバンドの種類名ComboBoxColumn.Name = "fsバンドの種類名ComboBoxColumn"
        fsバンドの種類名ComboBoxColumn.Resizable = Windows.Forms.DataGridViewTriState.True
        fsバンドの種類名ComboBoxColumn.SortMode = Windows.Forms.DataGridViewColumnSortMode.Automatic
        fsバンドの種類名ComboBoxColumn.ToolTipText = "バンドの種類を特定した色を指定したい場合のみ指定"
        fsバンドの種類名ComboBoxColumn.Width = 125
        ' 
        ' Fi赤DataGridViewTextBoxColumn
        ' 
        Fi赤DataGridViewTextBoxColumn.DataPropertyName = "f_i赤"
        DataGridViewCellStyle1.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.Format = "N0"
        DataGridViewCellStyle1.NullValue = "0"
        Fi赤DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle1
        Fi赤DataGridViewTextBoxColumn.HeaderText = "赤"
        Fi赤DataGridViewTextBoxColumn.MinimumWidth = 6
        Fi赤DataGridViewTextBoxColumn.Name = "Fi赤DataGridViewTextBoxColumn"
        Fi赤DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Fi赤DataGridViewTextBoxColumn.Width = 80
        ' 
        ' Fi緑DataGridViewTextBoxColumn
        ' 
        Fi緑DataGridViewTextBoxColumn.DataPropertyName = "f_i緑"
        DataGridViewCellStyle2.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N0"
        DataGridViewCellStyle2.NullValue = "0"
        Fi緑DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        Fi緑DataGridViewTextBoxColumn.HeaderText = "緑"
        Fi緑DataGridViewTextBoxColumn.MinimumWidth = 6
        Fi緑DataGridViewTextBoxColumn.Name = "Fi緑DataGridViewTextBoxColumn"
        Fi緑DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Fi緑DataGridViewTextBoxColumn.Width = 80
        ' 
        ' Fi青DataGridViewTextBoxColumn
        ' 
        Fi青DataGridViewTextBoxColumn.DataPropertyName = "f_i青"
        DataGridViewCellStyle3.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "N0"
        DataGridViewCellStyle3.NullValue = "0"
        Fi青DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle3
        Fi青DataGridViewTextBoxColumn.HeaderText = "青"
        Fi青DataGridViewTextBoxColumn.MinimumWidth = 6
        Fi青DataGridViewTextBoxColumn.Name = "Fi青DataGridViewTextBoxColumn"
        Fi青DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Fi青DataGridViewTextBoxColumn.Width = 80
        ' 
        ' disp
        ' 
        disp.HeaderText = ""
        disp.MinimumWidth = 6
        disp.Name = "disp"
        disp.ReadOnly = True
        disp.Width = 70
        ' 
        ' f_i透明度
        ' 
        f_i透明度.DataPropertyName = "f_i透明度"
        f_i透明度.HeaderText = "透明度"
        f_i透明度.MinimumWidth = 6
        f_i透明度.Name = "f_i透明度"
        f_i透明度.ToolTipText = "塗りつぶし0が透明 255が不透明"
        f_i透明度.Width = 125
        ' 
        ' f_d線幅
        ' 
        f_d線幅.DataPropertyName = "f_d線幅"
        f_d線幅.HeaderText = "線幅"
        f_d線幅.MinimumWidth = 6
        f_d線幅.Name = "f_d線幅"
        f_d線幅.ToolTipText = "バンド描画のペン幅"
        f_d線幅.Width = 125
        ' 
        ' f_s線色
        ' 
        f_s線色.DataPropertyName = "f_s線色"
        f_s線色.HeaderText = "線色"
        f_s線色.MinimumWidth = 6
        f_s線色.Name = "f_s線色"
        f_s線色.ToolTipText = "バンドの線色を指定したい場合、透明度(,赤,緑,青)値"
        f_s線色.Width = 125
        ' 
        ' f_d中線幅
        ' 
        f_d中線幅.DataPropertyName = "f_d中線幅"
        f_d中線幅.HeaderText = "中線幅"
        f_d中線幅.MinimumWidth = 6
        f_d中線幅.Name = "f_d中線幅"
        f_d中線幅.ToolTipText = "バンドの中線幅描画幅"
        f_d中線幅.Width = 125
        ' 
        ' f_s中線色
        ' 
        f_s中線色.DataPropertyName = "f_s中線色"
        f_s中線色.HeaderText = "中線色"
        f_s中線色.MinimumWidth = 6
        f_s中線色.Name = "f_s中線色"
        f_s中線色.ToolTipText = "バンドの中線色を指定したい場合、透明度(,赤,緑,青)値"
        f_s中線色.Width = 125
        ' 
        ' Texture
        ' 
        Texture.HeaderText = "画像"
        Texture.Name = "Texture"
        Texture.Resizable = Windows.Forms.DataGridViewTriState.True
        Texture.SortMode = Windows.Forms.DataGridViewColumnSortMode.Automatic
        ' 
        ' f_s画像情報
        ' 
        f_s画像情報.DataPropertyName = "f_s画像情報"
        f_s画像情報.HeaderText = "画像情報"
        f_s画像情報.Name = "f_s画像情報"
        f_s画像情報.ReadOnly = True
        ' 
        ' f_s画像文字列
        ' 
        f_s画像文字列.DataPropertyName = "f_s画像文字列"
        f_s画像文字列.HeaderText = "f_s画像文字列"
        f_s画像文字列.MaxInputLength = 1000000
        f_s画像文字列.Name = "f_s画像文字列"
        f_s画像文字列.ReadOnly = True
        f_s画像文字列.Visible = False
        ' 
        ' f_s製品情報
        ' 
        f_s製品情報.DataPropertyName = "f_s製品情報"
        f_s製品情報.HeaderText = "製品情報"
        f_s製品情報.MinimumWidth = 6
        f_s製品情報.Name = "f_s製品情報"
        f_s製品情報.ToolTipText = "メーカーや型番など購入のためのメモ"
        f_s製品情報.Width = 125
        ' 
        ' f_s備考
        ' 
        f_s備考.DataPropertyName = "f_s備考"
        f_s備考.HeaderText = "備考"
        f_s備考.MinimumWidth = 6
        f_s備考.Name = "f_s備考"
        f_s備考.Width = 125
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnキャンセル.DialogResult = Windows.Forms.DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(910, 155)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 44)
        btnキャンセル.TabIndex = 15
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "変更を保存せずに終了します")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(793, 155)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 44)
        btnOK.TabIndex = 14
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "変更を保存して終了します")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' txt色
        ' 
        txt色.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txt色.Location = New System.Drawing.Point(68, 169)
        txt色.Name = "txt色"
        txt色.Size = New System.Drawing.Size(31, 26)
        txt色.TabIndex = 3
        ToolTip1.SetToolTip(txt色, "色の名前")
        ' 
        ' nud赤
        ' 
        nud赤.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        nud赤.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        nud赤.Location = New System.Drawing.Point(321, 170)
        nud赤.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        nud赤.Name = "nud赤"
        nud赤.Size = New System.Drawing.Size(61, 26)
        nud赤.TabIndex = 7
        ToolTip1.SetToolTip(nud赤, "赤の値、0～255")
        ' 
        ' nud緑
        ' 
        nud緑.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        nud緑.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        nud緑.Location = New System.Drawing.Point(388, 170)
        nud緑.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        nud緑.Name = "nud緑"
        nud緑.Size = New System.Drawing.Size(61, 26)
        nud緑.TabIndex = 9
        ToolTip1.SetToolTip(nud緑, "緑の値、0～255")
        ' 
        ' nud青
        ' 
        nud青.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        nud青.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        nud青.Location = New System.Drawing.Point(455, 170)
        nud青.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        nud青.Name = "nud青"
        nud青.Size = New System.Drawing.Size(61, 26)
        nud青.TabIndex = 11
        ToolTip1.SetToolTip(nud青, "青の値、0～255")
        ' 
        ' lbl色
        ' 
        lbl色.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        lbl色.AutoSize = True
        lbl色.Location = New System.Drawing.Point(86, 142)
        lbl色.Name = "lbl色"
        lbl色.Size = New System.Drawing.Size(23, 19)
        lbl色.TabIndex = 2
        lbl色.Text = "色"
        ' 
        ' lbl赤
        ' 
        lbl赤.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        lbl赤.AutoSize = True
        lbl赤.Location = New System.Drawing.Point(321, 142)
        lbl赤.Name = "lbl赤"
        lbl赤.Size = New System.Drawing.Size(23, 19)
        lbl赤.TabIndex = 6
        lbl赤.Text = "赤"
        ' 
        ' lbl緑
        ' 
        lbl緑.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        lbl緑.AutoSize = True
        lbl緑.Location = New System.Drawing.Point(388, 142)
        lbl緑.Name = "lbl緑"
        lbl緑.Size = New System.Drawing.Size(23, 19)
        lbl緑.TabIndex = 8
        lbl緑.Text = "緑"
        ' 
        ' lbl青
        ' 
        lbl青.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        lbl青.AutoSize = True
        lbl青.Location = New System.Drawing.Point(455, 142)
        lbl青.Name = "lbl青"
        lbl青.Size = New System.Drawing.Size(23, 19)
        lbl青.TabIndex = 10
        lbl青.Text = "青"
        ' 
        ' btn追加
        ' 
        btn追加.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn追加.Location = New System.Drawing.Point(522, 155)
        btn追加.Name = "btn追加"
        btn追加.Size = New System.Drawing.Size(111, 44)
        btn追加.TabIndex = 12
        btn追加.Text = "更新/追加(&A)"
        ToolTip1.SetToolTip(btn追加, "同名は更新、新たな名前は追加します")
        btn追加.UseVisualStyleBackColor = True
        ' 
        ' lblColor
        ' 
        lblColor.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        lblColor.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        lblColor.FlatStyle = Windows.Forms.FlatStyle.Popup
        lblColor.Location = New System.Drawing.Point(12, 155)
        lblColor.Name = "lblColor"
        lblColor.Size = New System.Drawing.Size(50, 44)
        lblColor.TabIndex = 1
        ToolTip1.SetToolTip(lblColor, "現在の設定色、クリックで変更可")
        ' 
        ' cmbバンドの種類名
        ' 
        cmbバンドの種類名.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        cmbバンドの種類名.FormattingEnabled = True
        cmbバンドの種類名.Location = New System.Drawing.Point(103, 169)
        cmbバンドの種類名.Name = "cmbバンドの種類名"
        cmbバンドの種類名.Size = New System.Drawing.Size(212, 27)
        cmbバンドの種類名.TabIndex = 5
        ToolTip1.SetToolTip(cmbバンドの種類名, "通常は'-' バンドの種類を特定した色を指定したい場合のみ指定")
        ' 
        ' btn画像
        ' 
        btn画像.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn画像.Location = New System.Drawing.Point(639, 155)
        btn画像.Name = "btn画像"
        btn画像.Size = New System.Drawing.Size(111, 44)
        btn画像.TabIndex = 13
        btn画像.Text = "画像(&T)"
        ToolTip1.SetToolTip(btn画像, "テクスチャ画像を指定します")
        btn画像.UseVisualStyleBackColor = True
        btn画像.Visible = False
        ' 
        ' lblバンドの種類名
        ' 
        lblバンドの種類名.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        lblバンドの種類名.AutoSize = True
        lblバンドの種類名.Location = New System.Drawing.Point(122, 142)
        lblバンドの種類名.Name = "lblバンドの種類名"
        lblバンドの種類名.Size = New System.Drawing.Size(93, 19)
        lblバンドの種類名.TabIndex = 4
        lblバンドの種類名.Text = "バンドの種類名"
        ' 
        ' OpenFileDialogPng
        ' 
        OpenFileDialogPng.Filter = "PNGファイル (*.png)|*.png"
        OpenFileDialogPng.Title = "テクスチャ画像ファイルを指定してください"
        ' 
        ' frmColor
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(1033, 208)
        Controls.Add(btn画像)
        Controls.Add(lblバンドの種類名)
        Controls.Add(cmbバンドの種類名)
        Controls.Add(lblColor)
        Controls.Add(btn追加)
        Controls.Add(lbl青)
        Controls.Add(lbl緑)
        Controls.Add(lbl赤)
        Controls.Add(lbl色)
        Controls.Add(nud青)
        Controls.Add(nud緑)
        Controls.Add(nud赤)
        Controls.Add(txt色)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        Controls.Add(dgvData)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmColor"
        Text = "描画色"
        CType(BindingSource描画色, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvData, ComponentModel.ISupportInitialize).EndInit()
        CType(nud赤, ComponentModel.ISupportInitialize).EndInit()
        CType(nud緑, ComponentModel.ISupportInitialize).EndInit()
        CType(nud青, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents BindingSource描画色 As Windows.Forms.BindingSource
    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents 表示 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt色 As Windows.Forms.TextBox
    Friend WithEvents nud赤 As Windows.Forms.NumericUpDown
    Friend WithEvents nud緑 As Windows.Forms.NumericUpDown
    Friend WithEvents nud青 As Windows.Forms.NumericUpDown
    Friend WithEvents lbl色 As Windows.Forms.Label
    Friend WithEvents lbl赤 As Windows.Forms.Label
    Friend WithEvents lbl緑 As Windows.Forms.Label
    Friend WithEvents lbl青 As Windows.Forms.Label
    Friend WithEvents btn追加 As Windows.Forms.Button
    Friend WithEvents lblColor As Windows.Forms.Label
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents cmbバンドの種類名 As Windows.Forms.ComboBox
    Friend WithEvents lblバンドの種類名 As Windows.Forms.Label
    Friend WithEvents btn画像 As Windows.Forms.Button
    Friend WithEvents OpenFileDialogPng As Windows.Forms.OpenFileDialog
    Friend WithEvents Fs色DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fsバンドの種類名ComboBoxColumn As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fi赤DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi緑DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi青DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents disp As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i透明度 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d線幅 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s線色 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d中線幅 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s中線色 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Texture As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents f_s画像情報 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s画像文字列 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s製品情報 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s備考 As Windows.Forms.DataGridViewTextBoxColumn
End Class
