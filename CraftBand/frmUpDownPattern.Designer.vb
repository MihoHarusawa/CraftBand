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
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        btn削除 = New Windows.Forms.Button()
        btnキャンセル = New Windows.Forms.Button()
        btnOK = New Windows.Forms.Button()
        dgvData = New ctrDataGridView()
        Fs上下図名DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        Fi水平本数DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        Fi垂直本数DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        Fs上下DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        Fs備考DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        BindingSource上下図 = New System.Windows.Forms.BindingSource(components)
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        CType(dgvData, ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSource上下図, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btn削除
        ' 
        btn削除.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn削除.Location = New System.Drawing.Point(12, 192)
        btn削除.Name = "btn削除"
        btn削除.Size = New System.Drawing.Size(111, 44)
        btn削除.TabIndex = 9
        btn削除.Text = "削除(&D)"
        ToolTip1.SetToolTip(btn削除, "選択されている上下図を削除します")
        btn削除.UseVisualStyleBackColor = True
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnキャンセル.DialogResult = Windows.Forms.DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(326, 192)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 44)
        btnキャンセル.TabIndex = 14
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "変更を保存せずに終了します")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(207, 192)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 44)
        btnOK.TabIndex = 13
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "変更を保存して終了します")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' dgvData
        ' 
        dgvData.AllowUserToAddRows = False
        dgvData.AllowUserToDeleteRows = False
        dgvData.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgvData.AutoGenerateColumns = False
        dgvData.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvData.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvData.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {Fs上下図名DataGridViewTextBoxColumn, Fi水平本数DataGridViewTextBoxColumn, Fi垂直本数DataGridViewTextBoxColumn, Fs上下DataGridViewTextBoxColumn, Fs備考DataGridViewTextBoxColumn})
        dgvData.DataSource = BindingSource上下図
        dgvData.Location = New System.Drawing.Point(12, 11)
        dgvData.Name = "dgvData"
        dgvData.RowHeadersWidth = 51
        dgvData.RowTemplate.Height = 29
        dgvData.Size = New System.Drawing.Size(425, 167)
        dgvData.TabIndex = 15
        ' 
        ' Fs上下図名DataGridViewTextBoxColumn
        ' 
        Fs上下図名DataGridViewTextBoxColumn.DataPropertyName = "f_s上下図名"
        Fs上下図名DataGridViewTextBoxColumn.HeaderText = "上下図名"
        Fs上下図名DataGridViewTextBoxColumn.MinimumWidth = 6
        Fs上下図名DataGridViewTextBoxColumn.Name = "Fs上下図名DataGridViewTextBoxColumn"
        Fs上下図名DataGridViewTextBoxColumn.ToolTipText = "重複しない名前"
        Fs上下図名DataGridViewTextBoxColumn.Width = 150
        ' 
        ' Fi水平本数DataGridViewTextBoxColumn
        ' 
        Fi水平本数DataGridViewTextBoxColumn.DataPropertyName = "f_i水平本数"
        DataGridViewCellStyle1.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Fi水平本数DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle1
        Fi水平本数DataGridViewTextBoxColumn.HeaderText = "水平本数"
        Fi水平本数DataGridViewTextBoxColumn.MinimumWidth = 6
        Fi水平本数DataGridViewTextBoxColumn.Name = "Fi水平本数DataGridViewTextBoxColumn"
        Fi水平本数DataGridViewTextBoxColumn.Width = 80
        ' 
        ' Fi垂直本数DataGridViewTextBoxColumn
        ' 
        Fi垂直本数DataGridViewTextBoxColumn.DataPropertyName = "f_i垂直本数"
        DataGridViewCellStyle2.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Fi垂直本数DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        Fi垂直本数DataGridViewTextBoxColumn.HeaderText = "垂直本数"
        Fi垂直本数DataGridViewTextBoxColumn.MinimumWidth = 6
        Fi垂直本数DataGridViewTextBoxColumn.Name = "Fi垂直本数DataGridViewTextBoxColumn"
        Fi垂直本数DataGridViewTextBoxColumn.Width = 80
        ' 
        ' Fs上下DataGridViewTextBoxColumn
        ' 
        Fs上下DataGridViewTextBoxColumn.DataPropertyName = "f_s上下"
        Fs上下DataGridViewTextBoxColumn.HeaderText = "上下"
        Fs上下DataGridViewTextBoxColumn.MinimumWidth = 6
        Fs上下DataGridViewTextBoxColumn.Name = "Fs上下DataGridViewTextBoxColumn"
        Fs上下DataGridViewTextBoxColumn.ToolTipText = "1はON, 0はOFF, 行は';'で区切ります"
        Fs上下DataGridViewTextBoxColumn.Width = 300
        ' 
        ' Fs備考DataGridViewTextBoxColumn
        ' 
        Fs備考DataGridViewTextBoxColumn.DataPropertyName = "f_s備考"
        Fs備考DataGridViewTextBoxColumn.HeaderText = "備考"
        Fs備考DataGridViewTextBoxColumn.MinimumWidth = 6
        Fs備考DataGridViewTextBoxColumn.Name = "Fs備考DataGridViewTextBoxColumn"
        Fs備考DataGridViewTextBoxColumn.Width = 125
        ' 
        ' BindingSource上下図
        ' 
        BindingSource上下図.DataMember = "tbl上下図"
        BindingSource上下図.DataSource = GetType(Tables.dstMasterTables)
        ' 
        ' frmUpDownPattern
        ' 
        AcceptButton = btnキャンセル
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(449, 247)
        Controls.Add(dgvData)
        Controls.Add(btn削除)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmUpDownPattern"
        Text = "上下図"
        CType(dgvData, ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSource上下図, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents btn削除 As Windows.Forms.Button
    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents BindingSource上下図 As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents Fs上下図名DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi水平本数DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi垂直本数DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fs上下DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fs備考DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
End Class
