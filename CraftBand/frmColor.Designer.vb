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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BindingSource描画色 = New System.Windows.Forms.BindingSource(Me.components)
        Me.dgvData = New CraftBand.ctrDataGridView()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.txt色 = New System.Windows.Forms.TextBox()
        Me.nud赤 = New System.Windows.Forms.NumericUpDown()
        Me.nud緑 = New System.Windows.Forms.NumericUpDown()
        Me.nud青 = New System.Windows.Forms.NumericUpDown()
        Me.lbl色 = New System.Windows.Forms.Label()
        Me.lbl赤 = New System.Windows.Forms.Label()
        Me.lbl緑 = New System.Windows.Forms.Label()
        Me.lbl青 = New System.Windows.Forms.Label()
        Me.btn追加 = New System.Windows.Forms.Button()
        Me.lblColor = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmbバンドの種類名 = New System.Windows.Forms.ComboBox()
        Me.lblバンドの種類名 = New System.Windows.Forms.Label()
        Me.Fs色DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fsバンドの種類名ComboBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Fi赤DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fi緑DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fi青DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.disp = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d線幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_i透明度 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d中線幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s中線色 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s製品情報 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s備考 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.BindingSource描画色, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nud赤, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nud緑, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nud青, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BindingSource描画色
        '
        Me.BindingSource描画色.DataMember = "tbl描画色"
        Me.BindingSource描画色.DataSource = GetType(CraftBand.Tables.dstMasterTables)
        '
        'dgvData
        '
        Me.dgvData.AllowUserToAddRows = False
        Me.dgvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvData.AutoGenerateColumns = False
        Me.dgvData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Fs色DataGridViewTextBoxColumn, Me.fsバンドの種類名ComboBoxColumn, Me.Fi赤DataGridViewTextBoxColumn, Me.Fi緑DataGridViewTextBoxColumn, Me.Fi青DataGridViewTextBoxColumn, Me.disp, Me.f_d線幅, Me.f_i透明度, Me.f_d中線幅, Me.f_s中線色, Me.f_s製品情報, Me.f_s備考})
        Me.dgvData.DataSource = Me.BindingSource描画色
        Me.dgvData.Location = New System.Drawing.Point(12, 12)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.RowHeadersWidth = 51
        Me.dgvData.RowTemplate.Height = 29
        Me.dgvData.Size = New System.Drawing.Size(1010, 128)
        Me.dgvData.TabIndex = 0
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(910, 163)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 14
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.ToolTip1.SetToolTip(Me.btnキャンセル, "変更を保存せずに終了します")
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(791, 163)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 13
        Me.btnOK.Text = "OK(&O)"
        Me.ToolTip1.SetToolTip(Me.btnOK, "変更を保存して終了します")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'txt色
        '
        Me.txt色.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt色.Location = New System.Drawing.Point(68, 178)
        Me.txt色.Name = "txt色"
        Me.txt色.Size = New System.Drawing.Size(167, 27)
        Me.txt色.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.txt色, "色の名前")
        '
        'nud赤
        '
        Me.nud赤.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nud赤.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nud赤.Location = New System.Drawing.Point(459, 179)
        Me.nud赤.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nud赤.Name = "nud赤"
        Me.nud赤.Size = New System.Drawing.Size(61, 27)
        Me.nud赤.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.nud赤, "赤の値、0～255")
        '
        'nud緑
        '
        Me.nud緑.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nud緑.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nud緑.Location = New System.Drawing.Point(526, 179)
        Me.nud緑.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nud緑.Name = "nud緑"
        Me.nud緑.Size = New System.Drawing.Size(61, 27)
        Me.nud緑.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.nud緑, "緑の値、0～255")
        '
        'nud青
        '
        Me.nud青.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nud青.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nud青.Location = New System.Drawing.Point(593, 179)
        Me.nud青.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nud青.Name = "nud青"
        Me.nud青.Size = New System.Drawing.Size(61, 27)
        Me.nud青.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.nud青, "青の値、0～255")
        '
        'lbl色
        '
        Me.lbl色.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl色.AutoSize = True
        Me.lbl色.Location = New System.Drawing.Point(86, 149)
        Me.lbl色.Name = "lbl色"
        Me.lbl色.Size = New System.Drawing.Size(24, 20)
        Me.lbl色.TabIndex = 2
        Me.lbl色.Text = "色"
        '
        'lbl赤
        '
        Me.lbl赤.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl赤.AutoSize = True
        Me.lbl赤.Location = New System.Drawing.Point(459, 149)
        Me.lbl赤.Name = "lbl赤"
        Me.lbl赤.Size = New System.Drawing.Size(24, 20)
        Me.lbl赤.TabIndex = 6
        Me.lbl赤.Text = "赤"
        '
        'lbl緑
        '
        Me.lbl緑.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl緑.AutoSize = True
        Me.lbl緑.Location = New System.Drawing.Point(526, 149)
        Me.lbl緑.Name = "lbl緑"
        Me.lbl緑.Size = New System.Drawing.Size(24, 20)
        Me.lbl緑.TabIndex = 8
        Me.lbl緑.Text = "緑"
        '
        'lbl青
        '
        Me.lbl青.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl青.AutoSize = True
        Me.lbl青.Location = New System.Drawing.Point(593, 149)
        Me.lbl青.Name = "lbl青"
        Me.lbl青.Size = New System.Drawing.Size(24, 20)
        Me.lbl青.TabIndex = 10
        Me.lbl青.Text = "青"
        '
        'btn追加
        '
        Me.btn追加.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn追加.Location = New System.Drawing.Point(660, 163)
        Me.btn追加.Name = "btn追加"
        Me.btn追加.Size = New System.Drawing.Size(111, 46)
        Me.btn追加.TabIndex = 12
        Me.btn追加.Text = "更新/追加(&A)"
        Me.ToolTip1.SetToolTip(Me.btn追加, "同名は更新、新たな名前は追加します")
        Me.btn追加.UseVisualStyleBackColor = True
        '
        'lblColor
        '
        Me.lblColor.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblColor.Location = New System.Drawing.Point(12, 163)
        Me.lblColor.Name = "lblColor"
        Me.lblColor.Size = New System.Drawing.Size(50, 46)
        Me.lblColor.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.lblColor, "現在の設定色")
        '
        'cmbバンドの種類名
        '
        Me.cmbバンドの種類名.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbバンドの種類名.FormattingEnabled = True
        Me.cmbバンドの種類名.Location = New System.Drawing.Point(241, 178)
        Me.cmbバンドの種類名.Name = "cmbバンドの種類名"
        Me.cmbバンドの種類名.Size = New System.Drawing.Size(212, 28)
        Me.cmbバンドの種類名.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.cmbバンドの種類名, "通常は'-' バンドの種類を特定した色を指定したい場合のみ指定")
        '
        'lblバンドの種類名
        '
        Me.lblバンドの種類名.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblバンドの種類名.AutoSize = True
        Me.lblバンドの種類名.Location = New System.Drawing.Point(260, 149)
        Me.lblバンドの種類名.Name = "lblバンドの種類名"
        Me.lblバンドの種類名.Size = New System.Drawing.Size(100, 20)
        Me.lblバンドの種類名.TabIndex = 4
        Me.lblバンドの種類名.Text = "バンドの種類名"
        '
        'Fs色DataGridViewTextBoxColumn
        '
        Me.Fs色DataGridViewTextBoxColumn.DataPropertyName = "f_s色"
        Me.Fs色DataGridViewTextBoxColumn.HeaderText = "色"
        Me.Fs色DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fs色DataGridViewTextBoxColumn.Name = "Fs色DataGridViewTextBoxColumn"
        Me.Fs色DataGridViewTextBoxColumn.ToolTipText = "色の名前"
        Me.Fs色DataGridViewTextBoxColumn.Width = 125
        '
        'fsバンドの種類名ComboBoxColumn
        '
        Me.fsバンドの種類名ComboBoxColumn.DataPropertyName = "f_sバンドの種類名"
        Me.fsバンドの種類名ComboBoxColumn.HeaderText = "バンドの種類名"
        Me.fsバンドの種類名ComboBoxColumn.MinimumWidth = 6
        Me.fsバンドの種類名ComboBoxColumn.Name = "fsバンドの種類名ComboBoxColumn"
        Me.fsバンドの種類名ComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.fsバンドの種類名ComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.fsバンドの種類名ComboBoxColumn.ToolTipText = "バンドの種類を特定した色を指定したい場合のみ指定"
        Me.fsバンドの種類名ComboBoxColumn.Width = 125
        '
        'Fi赤DataGridViewTextBoxColumn
        '
        Me.Fi赤DataGridViewTextBoxColumn.DataPropertyName = "f_i赤"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.Format = "N0"
        DataGridViewCellStyle1.NullValue = "0"
        Me.Fi赤DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle1
        Me.Fi赤DataGridViewTextBoxColumn.HeaderText = "赤"
        Me.Fi赤DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fi赤DataGridViewTextBoxColumn.Name = "Fi赤DataGridViewTextBoxColumn"
        Me.Fi赤DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Me.Fi赤DataGridViewTextBoxColumn.Width = 80
        '
        'Fi緑DataGridViewTextBoxColumn
        '
        Me.Fi緑DataGridViewTextBoxColumn.DataPropertyName = "f_i緑"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N0"
        DataGridViewCellStyle2.NullValue = "0"
        Me.Fi緑DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.Fi緑DataGridViewTextBoxColumn.HeaderText = "緑"
        Me.Fi緑DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fi緑DataGridViewTextBoxColumn.Name = "Fi緑DataGridViewTextBoxColumn"
        Me.Fi緑DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Me.Fi緑DataGridViewTextBoxColumn.Width = 80
        '
        'Fi青DataGridViewTextBoxColumn
        '
        Me.Fi青DataGridViewTextBoxColumn.DataPropertyName = "f_i青"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "N0"
        DataGridViewCellStyle3.NullValue = "0"
        Me.Fi青DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle3
        Me.Fi青DataGridViewTextBoxColumn.HeaderText = "青"
        Me.Fi青DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fi青DataGridViewTextBoxColumn.Name = "Fi青DataGridViewTextBoxColumn"
        Me.Fi青DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Me.Fi青DataGridViewTextBoxColumn.Width = 80
        '
        'disp
        '
        Me.disp.HeaderText = ""
        Me.disp.MinimumWidth = 6
        Me.disp.Name = "disp"
        Me.disp.ReadOnly = True
        Me.disp.Width = 70
        '
        'f_d線幅
        '
        Me.f_d線幅.DataPropertyName = "f_d線幅"
        Me.f_d線幅.HeaderText = "線幅"
        Me.f_d線幅.MinimumWidth = 6
        Me.f_d線幅.Name = "f_d線幅"
        Me.f_d線幅.ToolTipText = "バンド描画のペン幅"
        Me.f_d線幅.Width = 125
        '
        'f_i透明度
        '
        Me.f_i透明度.DataPropertyName = "f_i透明度"
        Me.f_i透明度.HeaderText = "透明度"
        Me.f_i透明度.MinimumWidth = 6
        Me.f_i透明度.Name = "f_i透明度"
        Me.f_i透明度.ToolTipText = "塗りつぶし0が透明 255が不透明"
        Me.f_i透明度.Width = 125
        '
        'f_d中線幅
        '
        Me.f_d中線幅.DataPropertyName = "f_d中線幅"
        Me.f_d中線幅.HeaderText = "中線幅"
        Me.f_d中線幅.MinimumWidth = 6
        Me.f_d中線幅.Name = "f_d中線幅"
        Me.f_d中線幅.ToolTipText = "バンドの中線幅描画幅"
        Me.f_d中線幅.Width = 125
        '
        'f_s中線色
        '
        Me.f_s中線色.DataPropertyName = "f_s中線色"
        Me.f_s中線色.HeaderText = "中線色"
        Me.f_s中線色.MinimumWidth = 6
        Me.f_s中線色.Name = "f_s中線色"
        Me.f_s中線色.ToolTipText = "バンドの中線色を指定したい場合、透明度(,赤,緑,青)値"
        Me.f_s中線色.Width = 125
        '
        'f_s製品情報
        '
        Me.f_s製品情報.DataPropertyName = "f_s製品情報"
        Me.f_s製品情報.HeaderText = "製品情報"
        Me.f_s製品情報.MinimumWidth = 6
        Me.f_s製品情報.Name = "f_s製品情報"
        Me.f_s製品情報.ToolTipText = "メーカーや型番など購入のためのメモ"
        Me.f_s製品情報.Width = 125
        '
        'f_s備考
        '
        Me.f_s備考.DataPropertyName = "f_s備考"
        Me.f_s備考.HeaderText = "備考"
        Me.f_s備考.MinimumWidth = 6
        Me.f_s備考.Name = "f_s備考"
        Me.f_s備考.Width = 125
        '
        'frmColor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1033, 219)
        Me.Controls.Add(Me.lblバンドの種類名)
        Me.Controls.Add(Me.cmbバンドの種類名)
        Me.Controls.Add(Me.lblColor)
        Me.Controls.Add(Me.btn追加)
        Me.Controls.Add(Me.lbl青)
        Me.Controls.Add(Me.lbl緑)
        Me.Controls.Add(Me.lbl赤)
        Me.Controls.Add(Me.lbl色)
        Me.Controls.Add(Me.nud青)
        Me.Controls.Add(Me.nud緑)
        Me.Controls.Add(Me.nud赤)
        Me.Controls.Add(Me.txt色)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.dgvData)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmColor"
        Me.Text = "描画色"
        CType(Me.BindingSource描画色, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nud赤, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nud緑, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nud青, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
    Friend WithEvents Fs色DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fsバンドの種類名ComboBoxColumn As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fi赤DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi緑DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi青DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents disp As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d線幅 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i透明度 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d中線幅 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s中線色 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s製品情報 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s備考 As Windows.Forms.DataGridViewTextBoxColumn
End Class
