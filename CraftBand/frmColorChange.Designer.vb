<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmColorChange
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.chk側面と縁 = New System.Windows.Forms.CheckBox()
        Me.chk横ひも = New System.Windows.Forms.CheckBox()
        Me.chk縦ひも = New System.Windows.Forms.CheckBox()
        Me.chk追加品 = New System.Windows.Forms.CheckBox()
        Me.chk個別 = New System.Windows.Forms.CheckBox()
        Me.btn使用色 = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btn変更実行 = New System.Windows.Forms.Button()
        Me.dgvColorChange = New CraftBand.ctrDataGridView()
        Me.BeforeDataGridViewColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Count = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IsTargetDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.AfterDataGridViewColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.BindingSourceColorChange = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.dgvColorChange, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceColorChange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chk側面と縁
        '
        Me.chk側面と縁.AutoSize = True
        Me.chk側面と縁.Checked = True
        Me.chk側面と縁.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk側面と縁.Location = New System.Drawing.Point(38, 17)
        Me.chk側面と縁.Name = "chk側面と縁"
        Me.chk側面と縁.Size = New System.Drawing.Size(87, 24)
        Me.chk側面と縁.TabIndex = 0
        Me.chk側面と縁.Text = "側面と縁"
        Me.chk側面と縁.UseVisualStyleBackColor = True
        '
        'chk横ひも
        '
        Me.chk横ひも.AutoSize = True
        Me.chk横ひも.Checked = True
        Me.chk横ひも.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk横ひも.Location = New System.Drawing.Point(149, 17)
        Me.chk横ひも.Name = "chk横ひも"
        Me.chk横ひも.Size = New System.Drawing.Size(121, 24)
        Me.chk横ひも.TabIndex = 1
        Me.chk横ひも.Text = "横ひも(底の横)"
        Me.chk横ひも.UseVisualStyleBackColor = True
        '
        'chk縦ひも
        '
        Me.chk縦ひも.AutoSize = True
        Me.chk縦ひも.Checked = True
        Me.chk縦ひも.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk縦ひも.Location = New System.Drawing.Point(294, 17)
        Me.chk縦ひも.Name = "chk縦ひも"
        Me.chk縦ひも.Size = New System.Drawing.Size(121, 24)
        Me.chk縦ひも.TabIndex = 2
        Me.chk縦ひも.Text = "縦ひも(底の縦)"
        Me.chk縦ひも.UseVisualStyleBackColor = True
        '
        'chk追加品
        '
        Me.chk追加品.AutoSize = True
        Me.chk追加品.Checked = True
        Me.chk追加品.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk追加品.Location = New System.Drawing.Point(439, 17)
        Me.chk追加品.Name = "chk追加品"
        Me.chk追加品.Size = New System.Drawing.Size(76, 24)
        Me.chk追加品.TabIndex = 3
        Me.chk追加品.Text = "追加品"
        Me.chk追加品.UseVisualStyleBackColor = True
        '
        'chk個別
        '
        Me.chk個別.AutoSize = True
        Me.chk個別.Checked = True
        Me.chk個別.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk個別.Location = New System.Drawing.Point(539, 17)
        Me.chk個別.Name = "chk個別"
        Me.chk個別.Size = New System.Drawing.Size(71, 24)
        Me.chk個別.TabIndex = 4
        Me.chk個別.Text = "(個別)"
        Me.chk個別.UseVisualStyleBackColor = True
        '
        'btn使用色
        '
        Me.btn使用色.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn使用色.Location = New System.Drawing.Point(555, 63)
        Me.btn使用色.Name = "btn使用色"
        Me.btn使用色.Size = New System.Drawing.Size(111, 46)
        Me.btn使用色.TabIndex = 6
        Me.btn使用色.Text = "使用色(&P)"
        Me.ToolTip1.SetToolTip(Me.btn使用色, "使われている色をピックアップします")
        Me.btn使用色.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(555, 197)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 8
        Me.btnOK.Text = "OK(&O)"
        Me.ToolTip1.SetToolTip(Me.btnOK, "画面を閉じます")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btn変更実行
        '
        Me.btn変更実行.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn変更実行.Location = New System.Drawing.Point(555, 134)
        Me.btn変更実行.Name = "btn変更実行"
        Me.btn変更実行.Size = New System.Drawing.Size(111, 46)
        Me.btn変更実行.TabIndex = 7
        Me.btn変更実行.Text = "変更実行(&E)"
        Me.ToolTip1.SetToolTip(Me.btn変更実行, "色の変更を実行します")
        Me.btn変更実行.UseVisualStyleBackColor = True
        '
        'dgvColorChange
        '
        Me.dgvColorChange.AllowUserToAddRows = False
        Me.dgvColorChange.AllowUserToDeleteRows = False
        Me.dgvColorChange.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvColorChange.AutoGenerateColumns = False
        Me.dgvColorChange.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvColorChange.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvColorChange.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.BeforeDataGridViewColumn, Me.Count, Me.IsTargetDataGridViewCheckBoxColumn, Me.AfterDataGridViewColumn})
        Me.dgvColorChange.DataSource = Me.BindingSourceColorChange
        Me.dgvColorChange.Location = New System.Drawing.Point(38, 63)
        Me.dgvColorChange.Name = "dgvColorChange"
        Me.dgvColorChange.RowHeadersWidth = 30
        Me.dgvColorChange.RowTemplate.Height = 29
        Me.dgvColorChange.Size = New System.Drawing.Size(494, 180)
        Me.dgvColorChange.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.dgvColorChange, "何色を何色に変更するか")
        '
        'BeforeDataGridViewColumn
        '
        Me.BeforeDataGridViewColumn.DataPropertyName = "Before"
        Me.BeforeDataGridViewColumn.HeaderText = "現在の色"
        Me.BeforeDataGridViewColumn.MinimumWidth = 6
        Me.BeforeDataGridViewColumn.Name = "BeforeDataGridViewColumn"
        Me.BeforeDataGridViewColumn.ReadOnly = True
        Me.BeforeDataGridViewColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.BeforeDataGridViewColumn.Width = 150
        '
        'Count
        '
        Me.Count.DataPropertyName = "Count"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Count.DefaultCellStyle = DataGridViewCellStyle1
        Me.Count.HeaderText = "行数"
        Me.Count.MinimumWidth = 6
        Me.Count.Name = "Count"
        Me.Count.ReadOnly = True
        Me.Count.ToolTipText = "その色が設定された行数"
        Me.Count.Width = 125
        '
        'IsTargetDataGridViewCheckBoxColumn
        '
        Me.IsTargetDataGridViewCheckBoxColumn.DataPropertyName = "IsTarget"
        Me.IsTargetDataGridViewCheckBoxColumn.HeaderText = "対象"
        Me.IsTargetDataGridViewCheckBoxColumn.MinimumWidth = 6
        Me.IsTargetDataGridViewCheckBoxColumn.Name = "IsTargetDataGridViewCheckBoxColumn"
        Me.IsTargetDataGridViewCheckBoxColumn.ToolTipText = "色変更対象の場合チェックON"
        Me.IsTargetDataGridViewCheckBoxColumn.Width = 80
        '
        'AfterDataGridViewColumn
        '
        Me.AfterDataGridViewColumn.DataPropertyName = "After"
        Me.AfterDataGridViewColumn.HeaderText = "変更する色"
        Me.AfterDataGridViewColumn.MinimumWidth = 6
        Me.AfterDataGridViewColumn.Name = "AfterDataGridViewColumn"
        Me.AfterDataGridViewColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AfterDataGridViewColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.AfterDataGridViewColumn.Width = 180
        '
        'BindingSourceColorChange
        '
        Me.BindingSourceColorChange.DataMember = "tblColorChange"
        Me.BindingSourceColorChange.DataSource = GetType(CraftBand.Tables.dstWork)
        '
        'frmColorChange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(682, 265)
        Me.Controls.Add(Me.dgvColorChange)
        Me.Controls.Add(Me.btn変更実行)
        Me.Controls.Add(Me.btn使用色)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.chk個別)
        Me.Controls.Add(Me.chk追加品)
        Me.Controls.Add(Me.chk縦ひも)
        Me.Controls.Add(Me.chk横ひも)
        Me.Controls.Add(Me.chk側面と縁)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(700, 312)
        Me.Name = "frmColorChange"
        Me.Text = "色の変更"
        CType(Me.dgvColorChange, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceColorChange, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chk側面と縁 As Windows.Forms.CheckBox
    Friend WithEvents chk横ひも As Windows.Forms.CheckBox
    Friend WithEvents chk縦ひも As Windows.Forms.CheckBox
    Friend WithEvents chk追加品 As Windows.Forms.CheckBox
    Friend WithEvents chk個別 As Windows.Forms.CheckBox
    Friend WithEvents btn使用色 As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents btn変更実行 As Windows.Forms.Button
    Friend WithEvents dgvColorChange As ctrDataGridView
    Friend WithEvents BindingSourceColorChange As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents BeforeDataGridViewColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Count As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IsTargetDataGridViewCheckBoxColumn As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents AfterDataGridViewColumn As Windows.Forms.DataGridViewComboBoxColumn
End Class
