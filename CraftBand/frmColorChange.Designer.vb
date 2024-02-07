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
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        chk側面と縁 = New Windows.Forms.CheckBox()
        chk横ひも = New Windows.Forms.CheckBox()
        chk縦ひも = New Windows.Forms.CheckBox()
        chk追加品 = New Windows.Forms.CheckBox()
        chk個別 = New Windows.Forms.CheckBox()
        btn使用色 = New Windows.Forms.Button()
        btnOK = New Windows.Forms.Button()
        btn変更実行 = New Windows.Forms.Button()
        dgvColorChange = New ctrDataGridView()
        BeforeDataGridViewColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        Count = New Windows.Forms.DataGridViewTextBoxColumn()
        IsTargetDataGridViewCheckBoxColumn = New Windows.Forms.DataGridViewCheckBoxColumn()
        AfterDataGridViewColumn = New Windows.Forms.DataGridViewComboBoxColumn()
        BindingSourceColorChange = New System.Windows.Forms.BindingSource(components)
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        chk60度 = New Windows.Forms.CheckBox()
        CType(dgvColorChange, ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSourceColorChange, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' chk側面と縁
        ' 
        chk側面と縁.AutoSize = True
        chk側面と縁.Checked = True
        chk側面と縁.CheckState = Windows.Forms.CheckState.Checked
        chk側面と縁.Location = New System.Drawing.Point(37, 29)
        chk側面と縁.Name = "chk側面と縁"
        chk側面と縁.Size = New System.Drawing.Size(87, 24)
        chk側面と縁.TabIndex = 0
        chk側面と縁.Text = "側面と縁"
        chk側面と縁.UseVisualStyleBackColor = True
        ' 
        ' chk横ひも
        ' 
        chk横ひも.AutoSize = True
        chk横ひも.Checked = True
        chk横ひも.CheckState = Windows.Forms.CheckState.Checked
        chk横ひも.Location = New System.Drawing.Point(151, 29)
        chk横ひも.Name = "chk横ひも"
        chk横ひも.Size = New System.Drawing.Size(69, 24)
        chk横ひも.TabIndex = 1
        chk横ひも.Text = "横ひも"
        chk横ひも.UseVisualStyleBackColor = True
        ' 
        ' chk縦ひも
        ' 
        chk縦ひも.AutoSize = True
        chk縦ひも.Checked = True
        chk縦ひも.CheckState = Windows.Forms.CheckState.Checked
        chk縦ひも.Location = New System.Drawing.Point(247, 29)
        chk縦ひも.Name = "chk縦ひも"
        chk縦ひも.Size = New System.Drawing.Size(69, 24)
        chk縦ひも.TabIndex = 2
        chk縦ひも.Text = "縦ひも"
        chk縦ひも.UseVisualStyleBackColor = True
        ' 
        ' chk追加品
        ' 
        chk追加品.AutoSize = True
        chk追加品.Checked = True
        chk追加品.CheckState = Windows.Forms.CheckState.Checked
        chk追加品.Location = New System.Drawing.Point(432, 29)
        chk追加品.Name = "chk追加品"
        chk追加品.Size = New System.Drawing.Size(76, 24)
        chk追加品.TabIndex = 3
        chk追加品.Text = "追加品"
        chk追加品.UseVisualStyleBackColor = True
        ' 
        ' chk個別
        ' 
        chk個別.AutoSize = True
        chk個別.Checked = True
        chk個別.CheckState = Windows.Forms.CheckState.Checked
        chk個別.Location = New System.Drawing.Point(535, 29)
        chk個別.Name = "chk個別"
        chk個別.Size = New System.Drawing.Size(71, 24)
        chk個別.TabIndex = 4
        chk個別.Text = "(個別)"
        chk個別.UseVisualStyleBackColor = True
        ' 
        ' btn使用色
        ' 
        btn使用色.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        btn使用色.Location = New System.Drawing.Point(555, 63)
        btn使用色.Name = "btn使用色"
        btn使用色.Size = New System.Drawing.Size(111, 46)
        btn使用色.TabIndex = 6
        btn使用色.Text = "使用色(&P)"
        ToolTip1.SetToolTip(btn使用色, "使われている色をピックアップします")
        btn使用色.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.DialogResult = Windows.Forms.DialogResult.OK
        btnOK.Location = New System.Drawing.Point(555, 197)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 46)
        btnOK.TabIndex = 8
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "画面を閉じます")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btn変更実行
        ' 
        btn変更実行.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn変更実行.Location = New System.Drawing.Point(555, 134)
        btn変更実行.Name = "btn変更実行"
        btn変更実行.Size = New System.Drawing.Size(111, 46)
        btn変更実行.TabIndex = 7
        btn変更実行.Text = "変更実行(&E)"
        ToolTip1.SetToolTip(btn変更実行, "色の変更を実行します")
        btn変更実行.UseVisualStyleBackColor = True
        ' 
        ' dgvColorChange
        ' 
        dgvColorChange.AllowUserToAddRows = False
        dgvColorChange.AllowUserToDeleteRows = False
        dgvColorChange.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgvColorChange.AutoGenerateColumns = False
        dgvColorChange.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvColorChange.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvColorChange.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {BeforeDataGridViewColumn, Count, IsTargetDataGridViewCheckBoxColumn, AfterDataGridViewColumn})
        dgvColorChange.DataSource = BindingSourceColorChange
        dgvColorChange.Location = New System.Drawing.Point(38, 63)
        dgvColorChange.Name = "dgvColorChange"
        dgvColorChange.RowHeadersWidth = 30
        dgvColorChange.RowTemplate.Height = 29
        dgvColorChange.Size = New System.Drawing.Size(494, 180)
        dgvColorChange.TabIndex = 5
        ToolTip1.SetToolTip(dgvColorChange, "何色を何色に変更するか")
        ' 
        ' BeforeDataGridViewColumn
        ' 
        BeforeDataGridViewColumn.DataPropertyName = "Before"
        BeforeDataGridViewColumn.HeaderText = "現在の色"
        BeforeDataGridViewColumn.MinimumWidth = 6
        BeforeDataGridViewColumn.Name = "BeforeDataGridViewColumn"
        BeforeDataGridViewColumn.ReadOnly = True
        BeforeDataGridViewColumn.Resizable = Windows.Forms.DataGridViewTriState.True
        BeforeDataGridViewColumn.Width = 150
        ' 
        ' Count
        ' 
        Count.DataPropertyName = "Count"
        DataGridViewCellStyle1.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Count.DefaultCellStyle = DataGridViewCellStyle1
        Count.HeaderText = "行数"
        Count.MinimumWidth = 6
        Count.Name = "Count"
        Count.ReadOnly = True
        Count.ToolTipText = "その色が設定された行数"
        Count.Width = 125
        ' 
        ' IsTargetDataGridViewCheckBoxColumn
        ' 
        IsTargetDataGridViewCheckBoxColumn.DataPropertyName = "IsTarget"
        IsTargetDataGridViewCheckBoxColumn.HeaderText = "対象"
        IsTargetDataGridViewCheckBoxColumn.MinimumWidth = 6
        IsTargetDataGridViewCheckBoxColumn.Name = "IsTargetDataGridViewCheckBoxColumn"
        IsTargetDataGridViewCheckBoxColumn.ToolTipText = "色変更対象の場合チェックON"
        IsTargetDataGridViewCheckBoxColumn.Width = 80
        ' 
        ' AfterDataGridViewColumn
        ' 
        AfterDataGridViewColumn.DataPropertyName = "After"
        AfterDataGridViewColumn.HeaderText = "変更する色"
        AfterDataGridViewColumn.MinimumWidth = 6
        AfterDataGridViewColumn.Name = "AfterDataGridViewColumn"
        AfterDataGridViewColumn.Resizable = Windows.Forms.DataGridViewTriState.True
        AfterDataGridViewColumn.SortMode = Windows.Forms.DataGridViewColumnSortMode.Automatic
        AfterDataGridViewColumn.Width = 180
        ' 
        ' BindingSourceColorChange
        ' 
        BindingSourceColorChange.DataMember = "tblColorChange"
        BindingSourceColorChange.DataSource = GetType(Tables.dstWork)
        ' 
        ' chk60度
        ' 
        chk60度.AutoSize = True
        chk60度.Checked = True
        chk60度.CheckState = Windows.Forms.CheckState.Checked
        chk60度.Location = New System.Drawing.Point(343, 29)
        chk60度.Name = "chk60度"
        chk60度.Size = New System.Drawing.Size(62, 24)
        chk60度.TabIndex = 2
        chk60度.Text = "60度"
        chk60度.UseVisualStyleBackColor = True
        chk60度.Visible = False
        ' 
        ' frmColorChange
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(682, 265)
        Controls.Add(dgvColorChange)
        Controls.Add(btn変更実行)
        Controls.Add(btn使用色)
        Controls.Add(btnOK)
        Controls.Add(chk個別)
        Controls.Add(chk追加品)
        Controls.Add(chk60度)
        Controls.Add(chk縦ひも)
        Controls.Add(chk横ひも)
        Controls.Add(chk側面と縁)
        MaximizeBox = False
        MinimizeBox = False
        MinimumSize = New System.Drawing.Size(700, 312)
        Name = "frmColorChange"
        Text = "色の変更"
        CType(dgvColorChange, ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSourceColorChange, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

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
    Friend WithEvents CheckBox1 As Windows.Forms.CheckBox
    Friend WithEvents chk60度 As Windows.Forms.CheckBox
End Class
