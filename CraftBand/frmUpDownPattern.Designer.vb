<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpDownPattern
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btn削除 = New System.Windows.Forms.Button()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.dgvData = New CraftBand.ctrDataGridView()
        Me.Fs上下模様名DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fi水平本数DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fi垂直本数DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fs上下DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Fs備考DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSource上下模様 = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource上下模様, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn削除
        '
        Me.btn削除.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn削除.Location = New System.Drawing.Point(12, 202)
        Me.btn削除.Name = "btn削除"
        Me.btn削除.Size = New System.Drawing.Size(111, 46)
        Me.btn削除.TabIndex = 9
        Me.btn削除.Text = "削除(&D)"
        Me.ToolTip1.SetToolTip(Me.btn削除, "選択されている上下模様を削除します")
        Me.btn削除.UseVisualStyleBackColor = True
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(326, 202)
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
        Me.btnOK.Location = New System.Drawing.Point(207, 202)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 13
        Me.btnOK.Text = "OK(&O)"
        Me.ToolTip1.SetToolTip(Me.btnOK, "変更を保存して終了します")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'dgvData
        '
        Me.dgvData.AllowUserToAddRows = False
        Me.dgvData.AllowUserToDeleteRows = False
        Me.dgvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvData.AutoGenerateColumns = False
        Me.dgvData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Fs上下模様名DataGridViewTextBoxColumn, Me.Fi水平本数DataGridViewTextBoxColumn, Me.Fi垂直本数DataGridViewTextBoxColumn, Me.Fs上下DataGridViewTextBoxColumn, Me.Fs備考DataGridViewTextBoxColumn})
        Me.dgvData.DataSource = Me.BindingSource上下模様
        Me.dgvData.Location = New System.Drawing.Point(12, 12)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.RowHeadersWidth = 51
        Me.dgvData.RowTemplate.Height = 29
        Me.dgvData.Size = New System.Drawing.Size(425, 176)
        Me.dgvData.TabIndex = 15
        '
        'Fs上下模様名DataGridViewTextBoxColumn
        '
        Me.Fs上下模様名DataGridViewTextBoxColumn.DataPropertyName = "f_s上下模様名"
        Me.Fs上下模様名DataGridViewTextBoxColumn.HeaderText = "上下模様名"
        Me.Fs上下模様名DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fs上下模様名DataGridViewTextBoxColumn.Name = "Fs上下模様名DataGridViewTextBoxColumn"
        Me.Fs上下模様名DataGridViewTextBoxColumn.ToolTipText = "重複しない名前"
        Me.Fs上下模様名DataGridViewTextBoxColumn.Width = 150
        '
        'Fi水平本数DataGridViewTextBoxColumn
        '
        Me.Fi水平本数DataGridViewTextBoxColumn.DataPropertyName = "f_i水平本数"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Fi水平本数DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle5
        Me.Fi水平本数DataGridViewTextBoxColumn.HeaderText = "水平本数"
        Me.Fi水平本数DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fi水平本数DataGridViewTextBoxColumn.Name = "Fi水平本数DataGridViewTextBoxColumn"
        Me.Fi水平本数DataGridViewTextBoxColumn.Width = 80
        '
        'Fi垂直本数DataGridViewTextBoxColumn
        '
        Me.Fi垂直本数DataGridViewTextBoxColumn.DataPropertyName = "f_i垂直本数"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Fi垂直本数DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle6
        Me.Fi垂直本数DataGridViewTextBoxColumn.HeaderText = "垂直本数"
        Me.Fi垂直本数DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fi垂直本数DataGridViewTextBoxColumn.Name = "Fi垂直本数DataGridViewTextBoxColumn"
        Me.Fi垂直本数DataGridViewTextBoxColumn.Width = 80
        '
        'Fs上下DataGridViewTextBoxColumn
        '
        Me.Fs上下DataGridViewTextBoxColumn.DataPropertyName = "f_s上下"
        Me.Fs上下DataGridViewTextBoxColumn.HeaderText = "上下"
        Me.Fs上下DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fs上下DataGridViewTextBoxColumn.Name = "Fs上下DataGridViewTextBoxColumn"
        Me.Fs上下DataGridViewTextBoxColumn.ToolTipText = "1はON, 0はOFF, 行は';'で区切ります"
        Me.Fs上下DataGridViewTextBoxColumn.Width = 300
        '
        'Fs備考DataGridViewTextBoxColumn
        '
        Me.Fs備考DataGridViewTextBoxColumn.DataPropertyName = "f_s備考"
        Me.Fs備考DataGridViewTextBoxColumn.HeaderText = "備考"
        Me.Fs備考DataGridViewTextBoxColumn.MinimumWidth = 6
        Me.Fs備考DataGridViewTextBoxColumn.Name = "Fs備考DataGridViewTextBoxColumn"
        Me.Fs備考DataGridViewTextBoxColumn.Width = 125
        '
        'BindingSource上下模様
        '
        Me.BindingSource上下模様.DataMember = "tbl上下模様"
        Me.BindingSource上下模様.DataSource = GetType(CraftBand.Tables.dstMasterTables)
        '
        'frmUpDownPattern
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(449, 260)
        Me.Controls.Add(Me.dgvData)
        Me.Controls.Add(Me.btn削除)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUpDownPattern"
        Me.Text = "上下模様"
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource上下模様, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btn削除 As Windows.Forms.Button
    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents BindingSource上下模様 As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents Fs上下模様名DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi水平本数DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi垂直本数DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fs上下DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fs備考DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
End Class
