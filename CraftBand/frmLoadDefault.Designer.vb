<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLoadDefault
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        BindingSourceDefaultData = New System.Windows.Forms.BindingSource(components)
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        btnClose = New Windows.Forms.Button()
        btnロードする = New Windows.Forms.Button()
        dgvData = New ctrDataGridView()
        col選択 = New Windows.Forms.DataGridViewButtonColumn()
        colFileName = New Windows.Forms.DataGridViewTextBoxColumn()
        colFilePath = New Windows.Forms.DataGridViewTextBoxColumn()
        colBandTypeName = New Windows.Forms.DataGridViewTextBoxColumn()
        TargetWidthDataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        TargetDepthDataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        TargetHeightDataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        BasicBandWidthDataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        TitleDataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        CreatorDataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        MemoDataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        txt選択ファイル = New Windows.Forms.TextBox()
        lblロード対象 = New Windows.Forms.Label()
        CType(BindingSourceDefaultData, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvData, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' BindingSourceDefaultData
        ' 
        BindingSourceDefaultData.DataMember = "tblDefaultData"
        BindingSourceDefaultData.DataSource = GetType(Tables.dstWork)
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnClose.DialogResult = Windows.Forms.DialogResult.Cancel
        btnClose.Location = New System.Drawing.Point(373, 112)
        btnClose.Name = "btnClose"
        btnClose.Size = New System.Drawing.Size(111, 43)
        btnClose.TabIndex = 4
        btnClose.Text = "閉じる(&C)"
        ToolTip1.SetToolTip(btnClose, "この画面を閉じます")
        btnClose.UseVisualStyleBackColor = True
        ' 
        ' btnロードする
        ' 
        btnロードする.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnロードする.Enabled = False
        btnロードする.Location = New System.Drawing.Point(256, 112)
        btnロードする.Name = "btnロードする"
        btnロードする.Size = New System.Drawing.Size(111, 43)
        btnロードする.TabIndex = 3
        btnロードする.Text = "ロードする(&L)"
        ToolTip1.SetToolTip(btnロードする, "ロード対象データを読み取ります")
        btnロードする.UseVisualStyleBackColor = True
        ' 
        ' dgvData
        ' 
        dgvData.AllowUserToAddRows = False
        dgvData.AllowUserToDeleteRows = False
        dgvData.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgvData.AutoGenerateColumns = False
        dgvData.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvData.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvData.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {col選択, colFileName, colFilePath, colBandTypeName, TargetWidthDataGridViewTextBoxColumn, TargetDepthDataGridViewTextBoxColumn, TargetHeightDataGridViewTextBoxColumn, BasicBandWidthDataGridViewTextBoxColumn, TitleDataGridViewTextBoxColumn, CreatorDataGridViewTextBoxColumn, MemoDataGridViewTextBoxColumn})
        dgvData.DataSource = BindingSourceDefaultData
        dgvData.Location = New System.Drawing.Point(12, 13)
        dgvData.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        dgvData.Name = "dgvData"
        dgvData.RowHeadersWidth = 20
        dgvData.RowTemplate.Height = 25
        dgvData.Size = New System.Drawing.Size(472, 92)
        dgvData.TabIndex = 0
        ' 
        ' col選択
        ' 
        col選択.HeaderText = "選択"
        col選択.Name = "col選択"
        col選択.Text = "選択"
        col選択.ToolTipText = "このデータを選択します"
        col選択.UseColumnTextForButtonValue = True
        col選択.Width = 70
        ' 
        ' colFileName
        ' 
        colFileName.DataPropertyName = "FileName"
        colFileName.HeaderText = "ファイル名"
        colFileName.Name = "colFileName"
        colFileName.ReadOnly = True
        ' 
        ' colFilePath
        ' 
        colFilePath.DataPropertyName = "FilePath"
        colFilePath.HeaderText = "FilePath"
        colFilePath.Name = "colFilePath"
        colFilePath.ReadOnly = True
        colFilePath.Visible = False
        ' 
        ' colBandTypeName
        ' 
        colBandTypeName.DataPropertyName = "BandTypeName"
        colBandTypeName.HeaderText = "バンドの種類名"
        colBandTypeName.Name = "colBandTypeName"
        colBandTypeName.ReadOnly = True
        ' 
        ' TargetWidthDataGridViewTextBoxColumn
        ' 
        TargetWidthDataGridViewTextBoxColumn.DataPropertyName = "TargetWidth"
        DataGridViewCellStyle1.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        TargetWidthDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle1
        TargetWidthDataGridViewTextBoxColumn.HeaderText = "横寸法"
        TargetWidthDataGridViewTextBoxColumn.Name = "TargetWidthDataGridViewTextBoxColumn"
        TargetWidthDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' TargetDepthDataGridViewTextBoxColumn
        ' 
        TargetDepthDataGridViewTextBoxColumn.DataPropertyName = "TargetDepth"
        DataGridViewCellStyle2.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        TargetDepthDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        TargetDepthDataGridViewTextBoxColumn.HeaderText = "縦寸法"
        TargetDepthDataGridViewTextBoxColumn.Name = "TargetDepthDataGridViewTextBoxColumn"
        TargetDepthDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' TargetHeightDataGridViewTextBoxColumn
        ' 
        TargetHeightDataGridViewTextBoxColumn.DataPropertyName = "TargetHeight"
        DataGridViewCellStyle3.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        TargetHeightDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle3
        TargetHeightDataGridViewTextBoxColumn.HeaderText = "高さ寸法"
        TargetHeightDataGridViewTextBoxColumn.Name = "TargetHeightDataGridViewTextBoxColumn"
        TargetHeightDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' BasicBandWidthDataGridViewTextBoxColumn
        ' 
        BasicBandWidthDataGridViewTextBoxColumn.DataPropertyName = "BasicBandWidth"
        DataGridViewCellStyle4.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        BasicBandWidthDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle4
        BasicBandWidthDataGridViewTextBoxColumn.HeaderText = "基本のひも幅"
        BasicBandWidthDataGridViewTextBoxColumn.Name = "BasicBandWidthDataGridViewTextBoxColumn"
        BasicBandWidthDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' TitleDataGridViewTextBoxColumn
        ' 
        TitleDataGridViewTextBoxColumn.DataPropertyName = "Title"
        TitleDataGridViewTextBoxColumn.HeaderText = "タイトル"
        TitleDataGridViewTextBoxColumn.Name = "TitleDataGridViewTextBoxColumn"
        TitleDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' CreatorDataGridViewTextBoxColumn
        ' 
        CreatorDataGridViewTextBoxColumn.DataPropertyName = "Creator"
        CreatorDataGridViewTextBoxColumn.HeaderText = "作成者"
        CreatorDataGridViewTextBoxColumn.Name = "CreatorDataGridViewTextBoxColumn"
        CreatorDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' MemoDataGridViewTextBoxColumn
        ' 
        MemoDataGridViewTextBoxColumn.DataPropertyName = "Memo"
        MemoDataGridViewTextBoxColumn.HeaderText = "メモ"
        MemoDataGridViewTextBoxColumn.Name = "MemoDataGridViewTextBoxColumn"
        MemoDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' txt選択ファイル
        ' 
        txt選択ファイル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txt選択ファイル.Location = New System.Drawing.Point(84, 121)
        txt選択ファイル.Name = "txt選択ファイル"
        txt選択ファイル.ReadOnly = True
        txt選択ファイル.Size = New System.Drawing.Size(149, 26)
        txt選択ファイル.TabIndex = 2
        ' 
        ' lblロード対象
        ' 
        lblロード対象.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        lblロード対象.AutoSize = True
        lblロード対象.Location = New System.Drawing.Point(12, 124)
        lblロード対象.Name = "lblロード対象"
        lblロード対象.Size = New System.Drawing.Size(66, 19)
        lblロード対象.TabIndex = 1
        lblロード対象.Text = "ロード対象"
        ' 
        ' frmLoadDefault
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(496, 161)
        Controls.Add(lblロード対象)
        Controls.Add(txt選択ファイル)
        Controls.Add(btnClose)
        Controls.Add(btnロードする)
        Controls.Add(dgvData)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmLoadDefault"
        StartPosition = Windows.Forms.FormStartPosition.CenterParent
        Text = "規定値"
        CType(BindingSourceDefaultData, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvData, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents BindingSourceDefaultData As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents btnClose As Windows.Forms.Button
    Friend WithEvents btnロードする As Windows.Forms.Button
    Friend WithEvents txt選択ファイル As Windows.Forms.TextBox
    Friend WithEvents col選択 As Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colFileName As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFilePath As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBandTypeName As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TargetWidthDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TargetDepthDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TargetHeightDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BasicBandWidthDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TitleDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreatorDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MemoDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblロード対象 As Windows.Forms.Label
End Class
