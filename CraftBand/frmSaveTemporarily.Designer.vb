<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSaveTemporarily
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
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        btn一時保存 = New Windows.Forms.Button()
        btnClose = New Windows.Forms.Button()
        dgvData = New ctrDataGridView()
        BindingSourceSaveTemporarily = New System.Windows.Forms.BindingSource(components)
        txtメモ = New Windows.Forms.TextBox()
        lblメモ = New Windows.Forms.Label()
        txt時刻 = New Windows.Forms.Label()
        lbl時刻 = New Windows.Forms.Label()
        lbl一時保存リスト = New Windows.Forms.Label()
        col削除 = New Windows.Forms.DataGridViewButtonColumn()
        colIndex = New Windows.Forms.DataGridViewTextBoxColumn()
        colTimeString = New Windows.Forms.DataGridViewTextBoxColumn()
        ColIsFileExist = New Windows.Forms.DataGridViewCheckBoxColumn()
        colDescription = New Windows.Forms.DataGridViewTextBoxColumn()
        col復元 = New Windows.Forms.DataGridViewButtonColumn()
        CType(dgvData, ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSourceSaveTemporarily, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btn一時保存
        ' 
        btn一時保存.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        btn一時保存.Location = New System.Drawing.Point(357, 14)
        btn一時保存.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        btn一時保存.Name = "btn一時保存"
        btn一時保存.Size = New System.Drawing.Size(97, 34)
        btn一時保存.TabIndex = 4
        btn一時保存.Text = "一時保存(&S)"
        ToolTip1.SetToolTip(btn一時保存, "現在のデータを保存します")
        btn一時保存.UseVisualStyleBackColor = True
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        btnClose.Location = New System.Drawing.Point(357, 52)
        btnClose.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        btnClose.Name = "btnClose"
        btnClose.Size = New System.Drawing.Size(97, 34)
        btnClose.TabIndex = 5
        btnClose.Text = "閉じる(&C)"
        ToolTip1.SetToolTip(btnClose, "この画面を閉じます")
        btnClose.UseVisualStyleBackColor = True
        ' 
        ' dgvData
        ' 
        dgvData.AllowUserToAddRows = False
        dgvData.AllowUserToDeleteRows = False
        dgvData.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgvData.AutoGenerateColumns = False
        dgvData.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvData.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvData.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {col削除, colIndex, colTimeString, ColIsFileExist, colDescription, col復元})
        dgvData.DataSource = BindingSourceSaveTemporarily
        dgvData.Location = New System.Drawing.Point(5, 125)
        dgvData.Name = "dgvData"
        dgvData.RowHeadersWidth = 20
        dgvData.RowTemplate.Height = 25
        dgvData.Size = New System.Drawing.Size(450, 105)
        dgvData.TabIndex = 7
        ' 
        ' BindingSourceSaveTemporarily
        ' 
        BindingSourceSaveTemporarily.DataMember = "tblSaveTemporarily"
        BindingSourceSaveTemporarily.DataSource = GetType(Tables.dstWork)
        ' 
        ' txtメモ
        ' 
        txtメモ.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txtメモ.Location = New System.Drawing.Point(46, 30)
        txtメモ.Multiline = True
        txtメモ.Name = "txtメモ"
        txtメモ.Size = New System.Drawing.Size(305, 56)
        txtメモ.TabIndex = 3
        ' 
        ' lblメモ
        ' 
        lblメモ.AutoSize = True
        lblメモ.Location = New System.Drawing.Point(5, 30)
        lblメモ.Name = "lblメモ"
        lblメモ.Size = New System.Drawing.Size(24, 15)
        lblメモ.TabIndex = 2
        lblメモ.Text = "メモ"
        ' 
        ' txt時刻
        ' 
        txt時刻.AutoSize = True
        txt時刻.Location = New System.Drawing.Point(46, 9)
        txt時刻.Name = "txt時刻"
        txt時刻.Size = New System.Drawing.Size(31, 15)
        txt時刻.TabIndex = 1
        txt時刻.Text = "時刻"
        ' 
        ' lbl時刻
        ' 
        lbl時刻.AutoSize = True
        lbl時刻.Location = New System.Drawing.Point(5, 9)
        lbl時刻.Name = "lbl時刻"
        lbl時刻.Size = New System.Drawing.Size(31, 15)
        lbl時刻.TabIndex = 0
        lbl時刻.Text = "時刻"
        ' 
        ' lbl一時保存リスト
        ' 
        lbl一時保存リスト.AutoSize = True
        lbl一時保存リスト.Location = New System.Drawing.Point(5, 102)
        lbl一時保存リスト.Name = "lbl一時保存リスト"
        lbl一時保存リスト.Size = New System.Drawing.Size(80, 15)
        lbl一時保存リスト.TabIndex = 6
        lbl一時保存リスト.Text = "一時保存リスト"
        ' 
        ' col削除
        ' 
        col削除.HeaderText = "削除"
        col削除.Name = "col削除"
        col削除.Resizable = Windows.Forms.DataGridViewTriState.True
        col削除.Text = "削除"
        col削除.ToolTipText = "このデータを削除します"
        col削除.UseColumnTextForButtonValue = True
        ' 
        ' colIndex
        ' 
        colIndex.DataPropertyName = "Index"
        colIndex.HeaderText = "番号"
        colIndex.Name = "colIndex"
        colIndex.ReadOnly = True
        colIndex.Width = 80
        ' 
        ' colTimeString
        ' 
        colTimeString.DataPropertyName = "TimeString"
        colTimeString.HeaderText = "時刻"
        colTimeString.Name = "colTimeString"
        colTimeString.ReadOnly = True
        colTimeString.Width = 120
        ' 
        ' ColIsFileExist
        ' 
        ColIsFileExist.DataPropertyName = "IsFileExist"
        ColIsFileExist.HeaderText = "有効"
        ColIsFileExist.Name = "ColIsFileExist"
        ColIsFileExist.ReadOnly = True
        ColIsFileExist.SortMode = Windows.Forms.DataGridViewColumnSortMode.Automatic
        ColIsFileExist.Width = 80
        ' 
        ' colDescription
        ' 
        colDescription.DataPropertyName = "Description"
        colDescription.HeaderText = "メモ"
        colDescription.Name = "colDescription"
        colDescription.ToolTipText = "この保存データの説明"
        colDescription.Width = 200
        ' 
        ' col復元
        ' 
        col復元.HeaderText = "復元"
        col復元.Name = "col復元"
        col復元.Text = "復元"
        col復元.ToolTipText = "このデータの状態に戻します"
        col復元.UseColumnTextForButtonValue = True
        ' 
        ' frmSaveTemporarily
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(7F, 15F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(467, 239)
        Controls.Add(btnClose)
        Controls.Add(lbl一時保存リスト)
        Controls.Add(lbl時刻)
        Controls.Add(btn一時保存)
        Controls.Add(dgvData)
        Controls.Add(txtメモ)
        Controls.Add(txt時刻)
        Controls.Add(lblメモ)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmSaveTemporarily"
        StartPosition = Windows.Forms.FormStartPosition.CenterParent
        Text = "一時保存"
        CType(dgvData, ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSourceSaveTemporarily, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents txtメモ As Windows.Forms.TextBox
    Friend WithEvents lblメモ As Windows.Forms.Label
    Friend WithEvents txt時刻 As Windows.Forms.Label
    Friend WithEvents btn閉じる As Windows.Forms.Button
    Friend WithEvents btn一時保存 As Windows.Forms.Button
    Friend WithEvents lbl時刻 As Windows.Forms.Label
    Friend WithEvents lbl一時保存リスト As Windows.Forms.Label
    Friend WithEvents btnClose As Windows.Forms.Button
    Friend WithEvents BindingSourceSaveTemporarily As Windows.Forms.BindingSource
    Friend WithEvents col削除 As Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colIndex As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTimeString As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColIsFileExist As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colDescription As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents col復元 As Windows.Forms.DataGridViewButtonColumn
End Class
