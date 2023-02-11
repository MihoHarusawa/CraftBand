<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmColor
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BindingSource描画色 = New System.Windows.Forms.BindingSource(Me.components)
        Me.dgvData = New System.Windows.Forms.DataGridView()
        Me.Fs色DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fi赤DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fi緑DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fi青DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.色表示 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        CType(Me.BindingSource描画色, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BindingSource描画色
        '
        Me.BindingSource描画色.DataMember = "tbl描画色"
        Me.BindingSource描画色.DataSource = GetType(CraftBand.Tables.dstMasterTables)
        '
        'dgvData
        '
        Me.dgvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvData.AutoGenerateColumns = False
        Me.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Fs色DataGridViewTextBoxColumn, Me.Fi赤DataGridViewTextBoxColumn, Me.Fi緑DataGridViewTextBoxColumn, Me.Fi青DataGridViewTextBoxColumn, Me.色表示})
        Me.dgvData.DataSource = Me.BindingSource描画色
        Me.dgvData.Location = New System.Drawing.Point(12, 12)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.RowHeadersWidth = 51
        Me.dgvData.RowTemplate.Height = 29
        Me.dgvData.Size = New System.Drawing.Size(552, 135)
        Me.dgvData.TabIndex = 0
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
        'Fi赤DataGridViewTextBoxColumn
        '
        Me.Fi赤DataGridViewTextBoxColumn.DataPropertyName = "f_i赤"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N0"
        DataGridViewCellStyle4.NullValue = "0"
        Me.Fi赤DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle4
        Me.Fi赤DataGridViewTextBoxColumn.HeaderText = "赤"
        Me.Fi赤DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fi赤DataGridViewTextBoxColumn.Name = "Fi赤DataGridViewTextBoxColumn"
        Me.Fi赤DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Me.Fi赤DataGridViewTextBoxColumn.Width = 80
        '
        'Fi緑DataGridViewTextBoxColumn
        '
        Me.Fi緑DataGridViewTextBoxColumn.DataPropertyName = "f_i緑"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N0"
        DataGridViewCellStyle5.NullValue = "0"
        Me.Fi緑DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle5
        Me.Fi緑DataGridViewTextBoxColumn.HeaderText = "緑"
        Me.Fi緑DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fi緑DataGridViewTextBoxColumn.Name = "Fi緑DataGridViewTextBoxColumn"
        Me.Fi緑DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Me.Fi緑DataGridViewTextBoxColumn.Width = 80
        '
        'Fi青DataGridViewTextBoxColumn
        '
        Me.Fi青DataGridViewTextBoxColumn.DataPropertyName = "f_i青"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N0"
        DataGridViewCellStyle6.NullValue = "0"
        Me.Fi青DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle6
        Me.Fi青DataGridViewTextBoxColumn.HeaderText = "青"
        Me.Fi青DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fi青DataGridViewTextBoxColumn.Name = "Fi青DataGridViewTextBoxColumn"
        Me.Fi青DataGridViewTextBoxColumn.ToolTipText = "0～255の値で指定"
        Me.Fi青DataGridViewTextBoxColumn.Width = 80
        '
        '色表示
        '
        Me.色表示.HeaderText = "色表示"
        Me.色表示.MinimumWidth = 6
        Me.色表示.Name = "色表示"
        Me.色表示.ReadOnly = True
        Me.色表示.Width = 125
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(452, 163)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 6
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(333, 163)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "OK(&O)"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'frmColor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(575, 219)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.dgvData)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmColor"
        Me.Text = "描画色"
        CType(Me.BindingSource描画色, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BindingSource描画色 As Windows.Forms.BindingSource
    Friend WithEvents dgvData As Windows.Forms.DataGridView
    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents Fs色DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi赤DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi緑DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi青DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 色表示 As Windows.Forms.DataGridViewTextBoxColumn
End Class
