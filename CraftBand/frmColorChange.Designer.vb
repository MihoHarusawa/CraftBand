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
        btn使用色 = New Windows.Forms.Button()
        btn閉じる = New Windows.Forms.Button()
        btn変更実行 = New Windows.Forms.Button()
        dgvColorChange = New ctrDataGridView()
        BeforeDataGridViewColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        Count = New Windows.Forms.DataGridViewTextBoxColumn()
        IsTargetDataGridViewCheckBoxColumn = New Windows.Forms.DataGridViewCheckBoxColumn()
        AfterDataGridViewColumn = New Windows.Forms.DataGridViewComboBoxColumn()
        BindingSourceColorChange = New System.Windows.Forms.BindingSource(components)
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        CheckBox0 = New Windows.Forms.CheckBox()
        CheckBox3 = New Windows.Forms.CheckBox()
        CheckBox2 = New Windows.Forms.CheckBox()
        CheckBox1 = New Windows.Forms.CheckBox()
        CheckBox4 = New Windows.Forms.CheckBox()
        CheckBox5 = New Windows.Forms.CheckBox()
        CType(dgvColorChange, ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSourceColorChange, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btn使用色
        ' 
        btn使用色.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        btn使用色.Location = New System.Drawing.Point(559, 54)
        btn使用色.Name = "btn使用色"
        btn使用色.Size = New System.Drawing.Size(111, 46)
        btn使用色.TabIndex = 7
        btn使用色.Text = "使用色(&P)"
        ToolTip1.SetToolTip(btn使用色, "使われている色をピックアップします")
        btn使用色.UseVisualStyleBackColor = True
        ' 
        ' btn閉じる
        ' 
        btn閉じる.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn閉じる.Location = New System.Drawing.Point(559, 207)
        btn閉じる.Name = "btn閉じる"
        btn閉じる.Size = New System.Drawing.Size(111, 46)
        btn閉じる.TabIndex = 9
        btn閉じる.Text = "閉じる(&C)"
        ToolTip1.SetToolTip(btn閉じる, "この画面を閉じます")
        btn閉じる.UseVisualStyleBackColor = True
        ' 
        ' btn変更実行
        ' 
        btn変更実行.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn変更実行.Location = New System.Drawing.Point(559, 146)
        btn変更実行.Name = "btn変更実行"
        btn変更実行.Size = New System.Drawing.Size(111, 46)
        btn変更実行.TabIndex = 8
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
        dgvColorChange.Location = New System.Drawing.Point(12, 54)
        dgvColorChange.Name = "dgvColorChange"
        dgvColorChange.RowHeadersWidth = 30
        dgvColorChange.RowTemplate.Height = 29
        dgvColorChange.Size = New System.Drawing.Size(526, 199)
        dgvColorChange.TabIndex = 6
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
        ' CheckBox0
        ' 
        CheckBox0.AutoSize = True
        CheckBox0.Checked = True
        CheckBox0.CheckState = Windows.Forms.CheckState.Checked
        CheckBox0.Location = New System.Drawing.Point(11, 12)
        CheckBox0.Name = "CheckBox0"
        CheckBox0.Size = New System.Drawing.Size(103, 24)
        CheckBox0.TabIndex = 0
        CheckBox0.Text = "CheckBox0"
        CheckBox0.UseVisualStyleBackColor = True
        CheckBox0.Visible = False
        ' 
        ' CheckBox3
        ' 
        CheckBox3.AutoSize = True
        CheckBox3.Checked = True
        CheckBox3.CheckState = Windows.Forms.CheckState.Checked
        CheckBox3.Location = New System.Drawing.Point(347, 12)
        CheckBox3.Name = "CheckBox3"
        CheckBox3.Size = New System.Drawing.Size(103, 24)
        CheckBox3.TabIndex = 3
        CheckBox3.Text = "CheckBox3"
        CheckBox3.UseVisualStyleBackColor = True
        CheckBox3.Visible = False
        ' 
        ' CheckBox2
        ' 
        CheckBox2.AutoSize = True
        CheckBox2.Checked = True
        CheckBox2.CheckState = Windows.Forms.CheckState.Checked
        CheckBox2.Location = New System.Drawing.Point(235, 12)
        CheckBox2.Name = "CheckBox2"
        CheckBox2.Size = New System.Drawing.Size(103, 24)
        CheckBox2.TabIndex = 2
        CheckBox2.Text = "CheckBox2"
        CheckBox2.UseVisualStyleBackColor = True
        CheckBox2.Visible = False
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Checked = True
        CheckBox1.CheckState = Windows.Forms.CheckState.Checked
        CheckBox1.Location = New System.Drawing.Point(123, 12)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New System.Drawing.Size(103, 24)
        CheckBox1.TabIndex = 1
        CheckBox1.Text = "CheckBox1"
        CheckBox1.UseVisualStyleBackColor = True
        CheckBox1.Visible = False
        ' 
        ' CheckBox4
        ' 
        CheckBox4.AutoSize = True
        CheckBox4.Checked = True
        CheckBox4.CheckState = Windows.Forms.CheckState.Checked
        CheckBox4.Location = New System.Drawing.Point(459, 12)
        CheckBox4.Name = "CheckBox4"
        CheckBox4.Size = New System.Drawing.Size(103, 24)
        CheckBox4.TabIndex = 4
        CheckBox4.Text = "CheckBox4"
        CheckBox4.UseVisualStyleBackColor = True
        CheckBox4.Visible = False
        ' 
        ' CheckBox5
        ' 
        CheckBox5.AutoSize = True
        CheckBox5.Checked = True
        CheckBox5.CheckState = Windows.Forms.CheckState.Checked
        CheckBox5.Location = New System.Drawing.Point(571, 12)
        CheckBox5.Name = "CheckBox5"
        CheckBox5.Size = New System.Drawing.Size(103, 24)
        CheckBox5.TabIndex = 5
        CheckBox5.Text = "CheckBox5"
        CheckBox5.UseVisualStyleBackColor = True
        CheckBox5.Visible = False
        ' 
        ' frmColorChange
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(682, 265)
        Controls.Add(CheckBox5)
        Controls.Add(CheckBox4)
        Controls.Add(CheckBox0)
        Controls.Add(CheckBox3)
        Controls.Add(CheckBox2)
        Controls.Add(CheckBox1)
        Controls.Add(dgvColorChange)
        Controls.Add(btn変更実行)
        Controls.Add(btn使用色)
        Controls.Add(btn閉じる)
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
    Friend WithEvents btn使用色 As Windows.Forms.Button
    Friend WithEvents btn閉じる As Windows.Forms.Button
    Friend WithEvents btn変更実行 As Windows.Forms.Button
    Friend WithEvents dgvColorChange As ctrDataGridView
    Friend WithEvents BindingSourceColorChange As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents BeforeDataGridViewColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Count As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IsTargetDataGridViewCheckBoxColumn As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents AfterDataGridViewColumn As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents CheckBox1 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox0 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox4 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox5 As Windows.Forms.CheckBox
End Class
