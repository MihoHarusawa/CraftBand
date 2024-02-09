<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrExpanding
    Inherits System.Windows.Forms.UserControl

    'UserControl はコンポーネント一覧をクリーンアップするために dispose をオーバーライドします。
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Panel = New Windows.Forms.Panel()
        lblDirection = New Windows.Forms.Label()
        btn削除 = New Windows.Forms.Button()
        btn追加 = New Windows.Forms.Button()
        btnリセット = New Windows.Forms.Button()
        dgv展開 = New ctrDataGridView()
        f_iひも種4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i位置番号4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_sひも名4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_iひも番号4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i何本幅4 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_d長さ4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d幅4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_dひも長4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_dひも長加算4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_dひも長加算24 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s記号4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d出力ひも長4 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s色4 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_sメモ4 = New Windows.Forms.DataGridViewTextBoxColumn()
        BindingSource展開 = New System.Windows.Forms.BindingSource(components)
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        Panel.SuspendLayout()
        CType(dgv展開, ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSource展開, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Panel
        ' 
        Panel.Controls.Add(lblDirection)
        Panel.Controls.Add(btn削除)
        Panel.Controls.Add(btn追加)
        Panel.Controls.Add(btnリセット)
        Panel.Controls.Add(dgv展開)
        Panel.Enabled = False
        Panel.Location = New System.Drawing.Point(3, 3)
        Panel.Name = "Panel"
        Panel.Size = New System.Drawing.Size(840, 413)
        Panel.TabIndex = 0
        ' 
        ' lblDirection
        ' 
        lblDirection.AutoSize = True
        lblDirection.Location = New System.Drawing.Point(6, 8)
        lblDirection.Name = "lblDirection"
        lblDirection.Size = New System.Drawing.Size(115, 20)
        lblDirection.TabIndex = 0
        lblDirection.Text = "{0} から順に {1}へ"
        ' 
        ' btn削除
        ' 
        btn削除.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn削除.Location = New System.Drawing.Point(123, 362)
        btn削除.Name = "btn削除"
        btn削除.Size = New System.Drawing.Size(111, 46)
        btn削除.TabIndex = 3
        btn削除.Text = "削除(&R)"
        ToolTip1.SetToolTip(btn削除, "選択した行を削除します")
        btn削除.UseVisualStyleBackColor = True
        ' 
        ' btn追加
        ' 
        btn追加.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn追加.Location = New System.Drawing.Point(723, 362)
        btn追加.Name = "btn追加"
        btn追加.Size = New System.Drawing.Size(111, 46)
        btn追加.TabIndex = 4
        btn追加.Text = "追加(&A)"
        ToolTip1.SetToolTip(btn追加, "行を追加します")
        btn追加.UseVisualStyleBackColor = True
        ' 
        ' btnリセット
        ' 
        btnリセット.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btnリセット.Location = New System.Drawing.Point(6, 362)
        btnリセット.Name = "btnリセット"
        btnリセット.Size = New System.Drawing.Size(111, 46)
        btnリセット.TabIndex = 2
        btnリセット.Text = "リセット(&R)"
        ToolTip1.SetToolTip(btnリセット, "変更をリセットし初期状態に戻します")
        btnリセット.UseVisualStyleBackColor = True
        ' 
        ' dgv展開
        ' 
        dgv展開.AllowUserToAddRows = False
        dgv展開.AllowUserToDeleteRows = False
        dgv展開.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgv展開.AutoGenerateColumns = False
        dgv展開.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgv展開.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgv展開.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {f_iひも種4, f_i位置番号4, f_sひも名4, f_iひも番号4, f_i何本幅4, f_d長さ4, f_d幅4, f_dひも長4, f_dひも長加算4, f_dひも長加算24, f_s記号4, f_d出力ひも長4, f_s色4, f_sメモ4})
        dgv展開.DataSource = BindingSource展開
        dgv展開.Location = New System.Drawing.Point(6, 38)
        dgv展開.Name = "dgv展開"
        dgv展開.RowHeadersWidth = 51
        dgv展開.RowTemplate.Height = 29
        dgv展開.Size = New System.Drawing.Size(828, 315)
        dgv展開.TabIndex = 1
        ' 
        ' f_iひも種4
        ' 
        f_iひも種4.DataPropertyName = "f_iひも種"
        f_iひも種4.HeaderText = "f_iひも種"
        f_iひも種4.MinimumWidth = 6
        f_iひも種4.Name = "f_iひも種4"
        f_iひも種4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_iひも種4.Visible = False
        f_iひも種4.Width = 125
        ' 
        ' f_i位置番号4
        ' 
        f_i位置番号4.DataPropertyName = "f_i位置番号"
        DataGridViewCellStyle9.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i位置番号4.DefaultCellStyle = DataGridViewCellStyle9
        f_i位置番号4.HeaderText = "位置"
        f_i位置番号4.MinimumWidth = 6
        f_i位置番号4.Name = "f_i位置番号4"
        f_i位置番号4.ReadOnly = True
        f_i位置番号4.ToolTipText = "上から順の番号"
        f_i位置番号4.Width = 80
        ' 
        ' f_sひも名4
        ' 
        f_sひも名4.DataPropertyName = "f_sひも名"
        f_sひも名4.HeaderText = "ひも名"
        f_sひも名4.MinimumWidth = 6
        f_sひも名4.Name = "f_sひも名4"
        f_sひも名4.ReadOnly = True
        f_sひも名4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_sひも名4.Width = 125
        ' 
        ' f_iひも番号4
        ' 
        f_iひも番号4.DataPropertyName = "f_iひも番号"
        f_iひも番号4.HeaderText = "ひも番号"
        f_iひも番号4.MinimumWidth = 6
        f_iひも番号4.Name = "f_iひも番号4"
        f_iひも番号4.ReadOnly = True
        f_iひも番号4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_iひも番号4.Width = 125
        ' 
        ' f_i何本幅4
        ' 
        f_i何本幅4.DataPropertyName = "f_i何本幅"
        DataGridViewCellStyle10.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        f_i何本幅4.DefaultCellStyle = DataGridViewCellStyle10
        f_i何本幅4.HeaderText = "何本幅"
        f_i何本幅4.MinimumWidth = 6
        f_i何本幅4.Name = "f_i何本幅4"
        f_i何本幅4.ReadOnly = True
        f_i何本幅4.Resizable = Windows.Forms.DataGridViewTriState.True
        f_i何本幅4.Width = 125
        ' 
        ' f_d長さ4
        ' 
        f_d長さ4.DataPropertyName = "f_d長さ"
        DataGridViewCellStyle11.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle11.Format = "N2"
        DataGridViewCellStyle11.NullValue = Nothing
        f_d長さ4.DefaultCellStyle = DataGridViewCellStyle11
        f_d長さ4.HeaderText = "長さ"
        f_d長さ4.MinimumWidth = 6
        f_d長さ4.Name = "f_d長さ4"
        f_d長さ4.ReadOnly = True
        f_d長さ4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d長さ4.ToolTipText = "底部分"
        f_d長さ4.Width = 125
        ' 
        ' f_d幅4
        ' 
        f_d幅4.DataPropertyName = "f_d幅"
        DataGridViewCellStyle12.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle12.Format = "N2"
        DataGridViewCellStyle12.NullValue = Nothing
        f_d幅4.DefaultCellStyle = DataGridViewCellStyle12
        f_d幅4.HeaderText = "幅"
        f_d幅4.MinimumWidth = 6
        f_d幅4.Name = "f_d幅4"
        f_d幅4.ReadOnly = True
        f_d幅4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d幅4.ToolTipText = "ひも幅分プラス目"
        f_d幅4.Visible = False
        f_d幅4.Width = 125
        ' 
        ' f_dひも長4
        ' 
        f_dひも長4.DataPropertyName = "f_dひも長"
        DataGridViewCellStyle13.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle13.Format = "N2"
        DataGridViewCellStyle13.NullValue = Nothing
        f_dひも長4.DefaultCellStyle = DataGridViewCellStyle13
        f_dひも長4.HeaderText = "ひも長"
        f_dひも長4.MinimumWidth = 6
        f_dひも長4.Name = "f_dひも長4"
        f_dひも長4.ReadOnly = True
        f_dひも長4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_dひも長4.ToolTipText = "高さを加えひも長係数乗算"
        f_dひも長4.Width = 125
        ' 
        ' f_dひも長加算4
        ' 
        f_dひも長加算4.DataPropertyName = "f_dひも長加算"
        DataGridViewCellStyle14.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_dひも長加算4.DefaultCellStyle = DataGridViewCellStyle14
        f_dひも長加算4.HeaderText = "ひも長加算"
        f_dひも長加算4.MinimumWidth = 6
        f_dひも長加算4.Name = "f_dひも長加算4"
        f_dひも長加算4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_dひも長加算4.Width = 125
        ' 
        ' f_dひも長加算24
        ' 
        f_dひも長加算24.DataPropertyName = "f_dひも長加算2"
        DataGridViewCellStyle15.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_dひも長加算24.DefaultCellStyle = DataGridViewCellStyle15
        f_dひも長加算24.HeaderText = "ひも長加算2"
        f_dひも長加算24.MinimumWidth = 6
        f_dひも長加算24.Name = "f_dひも長加算24"
        f_dひも長加算24.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_dひも長加算24.Visible = False
        f_dひも長加算24.Width = 125
        ' 
        ' f_s記号4
        ' 
        f_s記号4.DataPropertyName = "f_s記号"
        f_s記号4.HeaderText = "f_s記号"
        f_s記号4.MinimumWidth = 6
        f_s記号4.Name = "f_s記号4"
        f_s記号4.ReadOnly = True
        f_s記号4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_s記号4.Visible = False
        f_s記号4.Width = 125
        ' 
        ' f_d出力ひも長4
        ' 
        f_d出力ひも長4.DataPropertyName = "f_d出力ひも長"
        DataGridViewCellStyle16.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle16.Format = "N2"
        DataGridViewCellStyle16.NullValue = Nothing
        f_d出力ひも長4.DefaultCellStyle = DataGridViewCellStyle16
        f_d出力ひも長4.HeaderText = "出力ひも長"
        f_d出力ひも長4.MinimumWidth = 6
        f_d出力ひも長4.Name = "f_d出力ひも長4"
        f_d出力ひも長4.ReadOnly = True
        f_d出力ひも長4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d出力ひも長4.Visible = False
        f_d出力ひも長4.Width = 125
        ' 
        ' f_s色4
        ' 
        f_s色4.DataPropertyName = "f_s色"
        f_s色4.HeaderText = "色"
        f_s色4.MinimumWidth = 6
        f_s色4.Name = "f_s色4"
        f_s色4.Resizable = Windows.Forms.DataGridViewTriState.True
        f_s色4.Width = 125
        ' 
        ' f_sメモ4
        ' 
        f_sメモ4.DataPropertyName = "f_sメモ"
        f_sメモ4.HeaderText = "メモ"
        f_sメモ4.MinimumWidth = 6
        f_sメモ4.Name = "f_sメモ4"
        f_sメモ4.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_sメモ4.Width = 125
        ' 
        ' BindingSource展開
        ' 
        BindingSource展開.DataMember = "tbl縦横展開"
        BindingSource展開.DataSource = GetType(Tables.dstDataTables)
        ' 
        ' ctrExpanding
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
        Controls.Add(Panel)
        Name = "ctrExpanding"
        Size = New System.Drawing.Size(872, 449)
        Panel.ResumeLayout(False)
        Panel.PerformLayout()
        CType(dgv展開, ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSource展開, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel As Windows.Forms.Panel
    Friend WithEvents BindingSource展開 As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents btn削除 As Windows.Forms.Button
    Friend WithEvents btn追加 As Windows.Forms.Button
    Friend WithEvents btnリセット As Windows.Forms.Button
    Friend WithEvents dgv展開 As ctrDataGridView
    Friend WithEvents lblDirection As Windows.Forms.Label
    Friend WithEvents f_iひも種4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i位置番号4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_sひも名4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_iひも番号4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i何本幅4 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_d長さ4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d幅4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算24 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s記号4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d出力ひも長4 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s色4 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_sメモ4 As Windows.Forms.DataGridViewTextBoxColumn

End Class
