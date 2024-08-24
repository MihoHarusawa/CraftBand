<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBandTypeWidth
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        dgvData = New ctrDataGridView()
        BindingSource本幅の幅リスト = New System.Windows.Forms.BindingSource(components)
        btnリセット = New Windows.Forms.Button()
        btnキャンセル = New Windows.Forms.Button()
        btnOK = New Windows.Forms.Button()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        lbl本幅数 = New Windows.Forms.Label()
        lblバンドの幅 = New Windows.Forms.Label()
        lbl本幅数値 = New Windows.Forms.Label()
        lblバンドの幅値 = New Windows.Forms.Label()
        lbl設定時の寸法単位 = New Windows.Forms.Label()
        f_i本幅 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d等分幅 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d変更幅 = New Windows.Forms.DataGridViewTextBoxColumn()
        CType(dgvData, ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSource本幅の幅リスト, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvData
        ' 
        dgvData.AllowUserToAddRows = False
        dgvData.AllowUserToDeleteRows = False
        dgvData.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgvData.AutoGenerateColumns = False
        dgvData.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvData.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvData.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {f_i本幅, f_d等分幅, f_d変更幅})
        dgvData.DataSource = BindingSource本幅の幅リスト
        dgvData.Location = New System.Drawing.Point(9, 35)
        dgvData.Name = "dgvData"
        dgvData.RowHeadersWidth = 51
        dgvData.RowTemplate.Height = 29
        dgvData.Size = New System.Drawing.Size(385, 190)
        dgvData.TabIndex = 5
        ' 
        ' BindingSource本幅の幅リスト
        ' 
        BindingSource本幅の幅リスト.DataMember = "tblLaneWidth"
        BindingSource本幅の幅リスト.DataSource = GetType(Tables.dstWork)
        ' 
        ' btnリセット
        ' 
        btnリセット.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btnリセット.Location = New System.Drawing.Point(11, 240)
        btnリセット.Name = "btnリセット"
        btnリセット.Size = New System.Drawing.Size(111, 46)
        btnリセット.TabIndex = 6
        btnリセット.Text = "リセット(&R)"
        ToolTip1.SetToolTip(btnリセット, "変更幅をすべてクリアします")
        btnリセット.UseVisualStyleBackColor = True
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnキャンセル.Location = New System.Drawing.Point(287, 240)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 46)
        btnキャンセル.TabIndex = 8
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "変更を保存せずに終了します")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(168, 240)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 46)
        btnOK.TabIndex = 7
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "変更を保存して終了します")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' lbl本幅数
        ' 
        lbl本幅数.AutoSize = True
        lbl本幅数.Location = New System.Drawing.Point(13, 7)
        lbl本幅数.Name = "lbl本幅数"
        lbl本幅数.Size = New System.Drawing.Size(54, 20)
        lbl本幅数.TabIndex = 0
        lbl本幅数.Text = "本幅数"
        ' 
        ' lblバンドの幅
        ' 
        lblバンドの幅.AutoSize = True
        lblバンドの幅.Location = New System.Drawing.Point(156, 7)
        lblバンドの幅.Name = "lblバンドの幅"
        lblバンドの幅.Size = New System.Drawing.Size(70, 20)
        lblバンドの幅.TabIndex = 2
        lblバンドの幅.Text = "バンドの幅"
        ' 
        ' lbl本幅数値
        ' 
        lbl本幅数値.AutoSize = True
        lbl本幅数値.Location = New System.Drawing.Point(72, 7)
        lbl本幅数値.Name = "lbl本幅数値"
        lbl本幅数値.Size = New System.Drawing.Size(49, 20)
        lbl本幅数値.TabIndex = 1
        lbl本幅数値.Text = "(本幅)"
        ' 
        ' lblバンドの幅値
        ' 
        lblバンドの幅値.AutoSize = True
        lblバンドの幅値.Location = New System.Drawing.Point(242, 7)
        lblバンドの幅値.Name = "lblバンドの幅値"
        lblバンドの幅値.Size = New System.Drawing.Size(68, 20)
        lblバンドの幅値.TabIndex = 3
        lblバンドの幅値.Text = "(バンド幅)"
        lblバンドの幅値.TextAlign = Drawing.ContentAlignment.TopCenter
        ' 
        ' lbl設定時の寸法単位
        ' 
        lbl設定時の寸法単位.AutoSize = True
        lbl設定時の寸法単位.Location = New System.Drawing.Point(319, 7)
        lbl設定時の寸法単位.Name = "lbl設定時の寸法単位"
        lbl設定時の寸法単位.Size = New System.Drawing.Size(35, 20)
        lbl設定時の寸法単位.TabIndex = 4
        lbl設定時の寸法単位.Text = "mm"
        ' 
        ' f_i本幅
        ' 
        f_i本幅.DataPropertyName = "f_i本幅"
        DataGridViewCellStyle1.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        f_i本幅.DefaultCellStyle = DataGridViewCellStyle1
        f_i本幅.HeaderText = "本幅"
        f_i本幅.MinimumWidth = 6
        f_i本幅.Name = "f_i本幅"
        f_i本幅.ReadOnly = True
        f_i本幅.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i本幅.ToolTipText = "本幅値"
        f_i本幅.Width = 80
        ' 
        ' f_d等分幅
        ' 
        f_d等分幅.DataPropertyName = "f_d等分幅"
        DataGridViewCellStyle2.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        f_d等分幅.DefaultCellStyle = DataGridViewCellStyle2
        f_d等分幅.HeaderText = "等分幅"
        f_d等分幅.MinimumWidth = 6
        f_d等分幅.Name = "f_d等分幅"
        f_d等分幅.ReadOnly = True
        f_d等分幅.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d等分幅.ToolTipText = "バンド幅の等分値"
        f_d等分幅.Width = 125
        ' 
        ' f_d変更幅
        ' 
        f_d変更幅.DataPropertyName = "f_d変更幅"
        DataGridViewCellStyle3.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.NullValue = Nothing
        f_d変更幅.DefaultCellStyle = DataGridViewCellStyle3
        f_d変更幅.HeaderText = "変更幅"
        f_d変更幅.MinimumWidth = 6
        f_d変更幅.Name = "f_d変更幅"
        f_d変更幅.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d変更幅.ToolTipText = "等分値を変更する場合のみ、その値"
        f_d変更幅.Width = 125
        ' 
        ' frmBandTypeWidth
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(406, 293)
        Controls.Add(lbl設定時の寸法単位)
        Controls.Add(lblバンドの幅値)
        Controls.Add(lblバンドの幅)
        Controls.Add(lbl本幅数値)
        Controls.Add(lbl本幅数)
        Controls.Add(btnリセット)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        Controls.Add(dgvData)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmBandTypeWidth"
        Text = "何本幅ごとの幅 <{0}>"
        CType(dgvData, ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSource本幅の幅リスト, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents btnリセット As Windows.Forms.Button
    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents BindingSource本幅の幅リスト As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents lbl本幅数 As Windows.Forms.Label
    Friend WithEvents lblバンドの幅 As Windows.Forms.Label
    Friend WithEvents lbl本幅数値 As Windows.Forms.Label
    Friend WithEvents lblバンドの幅値 As Windows.Forms.Label
    Friend WithEvents lbl設定時の寸法単位 As Windows.Forms.Label
    Friend WithEvents f_i本幅 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d等分幅 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d変更幅 As Windows.Forms.DataGridViewTextBoxColumn
End Class
