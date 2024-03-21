<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmColorRepeat
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
        dgvColorRepeat = New ctrDataGridView()
        DataGridViewColumnIndex = New Windows.Forms.DataGridViewTextBoxColumn()
        DataGridViewColumnLane = New Windows.Forms.DataGridViewComboBoxColumn()
        DataGridViewColumnColor = New Windows.Forms.DataGridViewComboBoxColumn()
        BindingSourceColorRepeat = New System.Windows.Forms.BindingSource(components)
        CheckBox0 = New Windows.Forms.CheckBox()
        CheckBox3 = New Windows.Forms.CheckBox()
        CheckBox2 = New Windows.Forms.CheckBox()
        CheckBox1 = New Windows.Forms.CheckBox()
        btn変更実行 = New Windows.Forms.Button()
        btn削除 = New Windows.Forms.Button()
        btn閉じる = New Windows.Forms.Button()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        btn追加 = New Windows.Forms.Button()
        CType(dgvColorRepeat, ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSourceColorRepeat, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvColorRepeat
        ' 
        dgvColorRepeat.AllowUserToAddRows = False
        dgvColorRepeat.AllowUserToDeleteRows = False
        dgvColorRepeat.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgvColorRepeat.AutoGenerateColumns = False
        dgvColorRepeat.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvColorRepeat.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvColorRepeat.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {DataGridViewColumnIndex, DataGridViewColumnLane, DataGridViewColumnColor})
        dgvColorRepeat.DataSource = BindingSourceColorRepeat
        dgvColorRepeat.Location = New System.Drawing.Point(12, 56)
        dgvColorRepeat.Name = "dgvColorRepeat"
        dgvColorRepeat.RowHeadersWidth = 30
        dgvColorRepeat.RowTemplate.Height = 29
        dgvColorRepeat.Size = New System.Drawing.Size(374, 156)
        dgvColorRepeat.TabIndex = 4
        ' 
        ' DataGridViewColumnIndex
        ' 
        DataGridViewColumnIndex.DataPropertyName = "Index"
        DataGridViewCellStyle1.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewColumnIndex.DefaultCellStyle = DataGridViewCellStyle1
        DataGridViewColumnIndex.HeaderText = "順番"
        DataGridViewColumnIndex.MinimumWidth = 6
        DataGridViewColumnIndex.Name = "DataGridViewColumnIndex"
        DataGridViewColumnIndex.ReadOnly = True
        DataGridViewColumnIndex.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        DataGridViewColumnIndex.Width = 80
        ' 
        ' DataGridViewColumnLane
        ' 
        DataGridViewColumnLane.DataPropertyName = "Lane"
        DataGridViewColumnLane.HeaderText = "何本幅"
        DataGridViewColumnLane.MinimumWidth = 6
        DataGridViewColumnLane.Name = "DataGridViewColumnLane"
        DataGridViewColumnLane.Resizable = Windows.Forms.DataGridViewTriState.True
        DataGridViewColumnLane.Width = 125
        ' 
        ' DataGridViewColumnColor
        ' 
        DataGridViewColumnColor.DataPropertyName = "Color"
        DataGridViewColumnColor.HeaderText = "色"
        DataGridViewColumnColor.MinimumWidth = 6
        DataGridViewColumnColor.Name = "DataGridViewColumnColor"
        DataGridViewColumnColor.Resizable = Windows.Forms.DataGridViewTriState.True
        DataGridViewColumnColor.Width = 150
        ' 
        ' BindingSourceColorRepeat
        ' 
        BindingSourceColorRepeat.DataMember = "tblColorRepeat"
        BindingSourceColorRepeat.DataSource = GetType(Tables.dstWork)
        ' 
        ' CheckBox0
        ' 
        CheckBox0.AutoSize = True
        CheckBox0.Checked = True
        CheckBox0.CheckState = Windows.Forms.CheckState.Checked
        CheckBox0.Location = New System.Drawing.Point(12, 12)
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
        CheckBox3.Location = New System.Drawing.Point(390, 12)
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
        CheckBox2.Location = New System.Drawing.Point(264, 12)
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
        CheckBox1.Location = New System.Drawing.Point(138, 12)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New System.Drawing.Size(103, 24)
        CheckBox1.TabIndex = 1
        CheckBox1.Text = "CheckBox1"
        CheckBox1.UseVisualStyleBackColor = True
        CheckBox1.Visible = False
        ' 
        ' btn変更実行
        ' 
        btn変更実行.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn変更実行.Location = New System.Drawing.Point(405, 166)
        btn変更実行.Name = "btn変更実行"
        btn変更実行.Size = New System.Drawing.Size(111, 46)
        btn変更実行.TabIndex = 7
        btn変更実行.Text = "変更実行(&E)"
        ToolTip1.SetToolTip(btn変更実行, "対象のデータに繰り返しをセットします")
        btn変更実行.UseVisualStyleBackColor = True
        ' 
        ' btn削除
        ' 
        btn削除.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn削除.Location = New System.Drawing.Point(12, 225)
        btn削除.Name = "btn削除"
        btn削除.Size = New System.Drawing.Size(111, 46)
        btn削除.TabIndex = 5
        btn削除.Text = "削除(&D)"
        ToolTip1.SetToolTip(btn削除, "選択行を削除します")
        btn削除.UseVisualStyleBackColor = True
        ' 
        ' btn閉じる
        ' 
        btn閉じる.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn閉じる.Location = New System.Drawing.Point(405, 225)
        btn閉じる.Name = "btn閉じる"
        btn閉じる.Size = New System.Drawing.Size(111, 46)
        btn閉じる.TabIndex = 8
        btn閉じる.Text = "閉じる(&C)"
        ToolTip1.SetToolTip(btn閉じる, "この画面を閉じます")
        btn閉じる.UseVisualStyleBackColor = True
        ' 
        ' btn追加
        ' 
        btn追加.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn追加.Location = New System.Drawing.Point(275, 225)
        btn追加.Name = "btn追加"
        btn追加.Size = New System.Drawing.Size(111, 46)
        btn追加.TabIndex = 6
        btn追加.Text = "追加(&A)"
        ToolTip1.SetToolTip(btn追加, "行を追加します")
        btn追加.UseVisualStyleBackColor = True
        ' 
        ' frmColorRepeat
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(528, 276)
        Controls.Add(btn変更実行)
        Controls.Add(btn追加)
        Controls.Add(btn削除)
        Controls.Add(btn閉じる)
        Controls.Add(CheckBox0)
        Controls.Add(CheckBox3)
        Controls.Add(CheckBox2)
        Controls.Add(CheckBox1)
        Controls.Add(dgvColorRepeat)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmColorRepeat"
        Text = "色の繰り返し"
        CType(dgvColorRepeat, ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSourceColorRepeat, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dgvColorRepeat As ctrDataGridView
    Friend WithEvents CheckBox0 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As Windows.Forms.CheckBox
    Friend WithEvents btn変更実行 As Windows.Forms.Button
    Friend WithEvents btn削除 As Windows.Forms.Button
    Friend WithEvents btn閉じる As Windows.Forms.Button
    Friend WithEvents BindingSourceColorRepeat As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents btn追加 As Windows.Forms.Button
    Friend WithEvents DataGridViewColumnIndex As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewColumnLane As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridViewColumnColor As Windows.Forms.DataGridViewComboBoxColumn
End Class
