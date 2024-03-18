<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrAdditionalBand
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        BindingSource差しひも = New System.Windows.Forms.BindingSource(components)
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        btn削除_差しひも = New Windows.Forms.Button()
        btn追加_差しひも = New Windows.Forms.Button()
        btn下へ_差しひも = New Windows.Forms.Button()
        btn上へ_差しひも = New Windows.Forms.Button()
        Panel = New Windows.Forms.Panel()
        dgv差しひも = New ctrDataGridView()
        f_i番号1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_b有効区分1 = New Windows.Forms.DataGridViewCheckBoxColumn()
        f_i配置面1 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_i角度1 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_i中心点1 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_i何本幅1 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_s色1 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_i開始位置1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i何本ごと1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_dひも長1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_iひも本数1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_dひも長加算1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d出力ひも長1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s無効理由1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_sメモ1 = New Windows.Forms.DataGridViewTextBoxColumn()
        Fi同位置順DataGridViewTextBoxColumn = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i同位置数 = New Windows.Forms.DataGridViewTextBoxColumn()
        CType(BindingSource差しひも, ComponentModel.ISupportInitialize).BeginInit()
        Panel.SuspendLayout()
        CType(dgv差しひも, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' BindingSource差しひも
        ' 
        BindingSource差しひも.DataMember = "tbl差しひも"
        BindingSource差しひも.DataSource = GetType(Tables.dstDataTables)
        ' 
        ' btn削除_差しひも
        ' 
        btn削除_差しひも.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn削除_差しひも.Location = New System.Drawing.Point(240, 361)
        btn削除_差しひも.Name = "btn削除_差しひも"
        btn削除_差しひも.Size = New System.Drawing.Size(111, 46)
        btn削除_差しひも.TabIndex = 8
        btn削除_差しひも.Text = "削除(&R)"
        ToolTip1.SetToolTip(btn削除_差しひも, "選択した行を削除します")
        btn削除_差しひも.UseVisualStyleBackColor = True
        ' 
        ' btn追加_差しひも
        ' 
        btn追加_差しひも.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn追加_差しひも.Location = New System.Drawing.Point(723, 361)
        btn追加_差しひも.Name = "btn追加_差しひも"
        btn追加_差しひも.Size = New System.Drawing.Size(111, 46)
        btn追加_差しひも.TabIndex = 9
        btn追加_差しひも.Text = "追加(&A)"
        ToolTip1.SetToolTip(btn追加_差しひも, "行を追加します")
        btn追加_差しひも.UseVisualStyleBackColor = True
        ' 
        ' btn下へ_差しひも
        ' 
        btn下へ_差しひも.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn下へ_差しひも.Location = New System.Drawing.Point(123, 361)
        btn下へ_差しひも.Name = "btn下へ_差しひも"
        btn下へ_差しひも.Size = New System.Drawing.Size(111, 46)
        btn下へ_差しひも.TabIndex = 7
        btn下へ_差しひも.Text = "下へ(&D)"
        ToolTip1.SetToolTip(btn下へ_差しひも, "選択した行を下に移動します")
        btn下へ_差しひも.UseVisualStyleBackColor = True
        ' 
        ' btn上へ_差しひも
        ' 
        btn上へ_差しひも.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn上へ_差しひも.Location = New System.Drawing.Point(6, 361)
        btn上へ_差しひも.Name = "btn上へ_差しひも"
        btn上へ_差しひも.Size = New System.Drawing.Size(111, 46)
        btn上へ_差しひも.TabIndex = 6
        btn上へ_差しひも.Text = "上へ(&U)"
        ToolTip1.SetToolTip(btn上へ_差しひも, "選択した行を上に移動します")
        btn上へ_差しひも.UseVisualStyleBackColor = True
        ' 
        ' Panel
        ' 
        Panel.Controls.Add(btn削除_差しひも)
        Panel.Controls.Add(btn追加_差しひも)
        Panel.Controls.Add(btn下へ_差しひも)
        Panel.Controls.Add(btn上へ_差しひも)
        Panel.Controls.Add(dgv差しひも)
        Panel.Location = New System.Drawing.Point(3, 3)
        Panel.Name = "Panel"
        Panel.Size = New System.Drawing.Size(840, 413)
        Panel.TabIndex = 0
        ' 
        ' dgv差しひも
        ' 
        dgv差しひも.AccessibleRole = Windows.Forms.AccessibleRole.MenuBar
        dgv差しひも.AllowUserToAddRows = False
        dgv差しひも.AllowUserToDeleteRows = False
        dgv差しひも.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        dgv差しひも.AutoGenerateColumns = False
        dgv差しひも.ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgv差しひも.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgv差しひも.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {f_i番号1, f_b有効区分1, f_i配置面1, f_i角度1, f_i中心点1, f_i何本幅1, f_s色1, f_i開始位置1, f_i何本ごと1, f_dひも長1, f_iひも本数1, f_dひも長加算1, f_d出力ひも長1, f_s無効理由1, f_sメモ1, Fi同位置順DataGridViewTextBoxColumn, f_i同位置数})
        dgv差しひも.DataSource = BindingSource差しひも
        dgv差しひも.Location = New System.Drawing.Point(6, 6)
        dgv差しひも.Name = "dgv差しひも"
        dgv差しひも.RowHeadersWidth = 51
        dgv差しひも.RowTemplate.Height = 29
        dgv差しひも.Size = New System.Drawing.Size(828, 349)
        dgv差しひも.TabIndex = 5
        ' 
        ' f_i番号1
        ' 
        f_i番号1.DataPropertyName = "f_i番号"
        DataGridViewCellStyle1.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i番号1.DefaultCellStyle = DataGridViewCellStyle1
        f_i番号1.HeaderText = "番号"
        f_i番号1.MinimumWidth = 6
        f_i番号1.Name = "f_i番号1"
        f_i番号1.ReadOnly = True
        f_i番号1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i番号1.Width = 70
        ' 
        ' f_b有効区分1
        ' 
        f_b有効区分1.DataPropertyName = "f_b有効区分"
        f_b有効区分1.HeaderText = "有効"
        f_b有効区分1.MinimumWidth = 6
        f_b有効区分1.Name = "f_b有効区分1"
        f_b有効区分1.ReadOnly = True
        f_b有効区分1.ToolTipText = "有効な設定にはチェックが入ります"
        f_b有効区分1.Width = 60
        ' 
        ' f_i配置面1
        ' 
        f_i配置面1.DataPropertyName = "f_i配置面"
        f_i配置面1.HeaderText = "配置面"
        f_i配置面1.MinimumWidth = 6
        f_i配置面1.Name = "f_i配置面1"
        f_i配置面1.ToolTipText = "なし、を選ぶと無効になります"
        f_i配置面1.Width = 125
        ' 
        ' f_i角度1
        ' 
        f_i角度1.DataPropertyName = "f_i角度"
        f_i角度1.HeaderText = "角度"
        f_i角度1.MinimumWidth = 6
        f_i角度1.Name = "f_i角度1"
        f_i角度1.ToolTipText = "全面の場合は、最初の面に対して"
        f_i角度1.Width = 125
        ' 
        ' f_i中心点1
        ' 
        f_i中心点1.DataPropertyName = "f_i中心点"
        f_i中心点1.HeaderText = "中心点"
        f_i中心点1.MinimumWidth = 6
        f_i中心点1.Name = "f_i中心点1"
        f_i中心点1.ToolTipText = "ひもの上に乗せるか、目を通すか"
        f_i中心点1.Width = 125
        ' 
        ' f_i何本幅1
        ' 
        f_i何本幅1.DataPropertyName = "f_i何本幅"
        f_i何本幅1.HeaderText = "何本幅"
        f_i何本幅1.MinimumWidth = 6
        f_i何本幅1.Name = "f_i何本幅1"
        f_i何本幅1.ToolTipText = "ひも幅/目の対角線より大きくすると無効になります"
        f_i何本幅1.Width = 80
        ' 
        ' f_s色1
        ' 
        f_s色1.DataPropertyName = "f_s色"
        f_s色1.HeaderText = "色"
        f_s色1.MinimumWidth = 6
        f_s色1.Name = "f_s色1"
        f_s色1.Width = 125
        ' 
        ' f_i開始位置1
        ' 
        f_i開始位置1.DataPropertyName = "f_i開始位置"
        DataGridViewCellStyle2.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i開始位置1.DefaultCellStyle = DataGridViewCellStyle2
        f_i開始位置1.HeaderText = "開始位置"
        f_i開始位置1.MinimumWidth = 6
        f_i開始位置1.Name = "f_i開始位置1"
        f_i開始位置1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i開始位置1.ToolTipText = "底の左上・側面の左下が1、135度全面は右下から逆回り"
        f_i開始位置1.Width = 80
        ' 
        ' f_i何本ごと1
        ' 
        f_i何本ごと1.DataPropertyName = "f_i何本ごと"
        DataGridViewCellStyle3.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i何本ごと1.DefaultCellStyle = DataGridViewCellStyle3
        f_i何本ごと1.HeaderText = "何本ごと"
        f_i何本ごと1.MinimumWidth = 6
        f_i何本ごと1.Name = "f_i何本ごと1"
        f_i何本ごと1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i何本ごと1.ToolTipText = "1は全て、0は1本だけ"
        f_i何本ごと1.Width = 80
        ' 
        ' f_dひも長1
        ' 
        f_dひも長1.DataPropertyName = "f_dひも長"
        DataGridViewCellStyle4.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N2"
        DataGridViewCellStyle4.NullValue = Nothing
        f_dひも長1.DefaultCellStyle = DataGridViewCellStyle4
        f_dひも長1.HeaderText = "ひも長"
        f_dひも長1.MinimumWidth = 6
        f_dひも長1.Name = "f_dひも長1"
        f_dひも長1.ReadOnly = True
        f_dひも長1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_dひも長1.Width = 125
        ' 
        ' f_iひも本数1
        ' 
        f_iひも本数1.DataPropertyName = "f_iひも本数"
        DataGridViewCellStyle5.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_iひも本数1.DefaultCellStyle = DataGridViewCellStyle5
        f_iひも本数1.HeaderText = "ひも本数"
        f_iひも本数1.MinimumWidth = 6
        f_iひも本数1.Name = "f_iひも本数1"
        f_iひも本数1.ReadOnly = True
        f_iひも本数1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_iひも本数1.Width = 125
        ' 
        ' f_dひも長加算1
        ' 
        f_dひも長加算1.DataPropertyName = "f_dひも長加算"
        DataGridViewCellStyle6.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_dひも長加算1.DefaultCellStyle = DataGridViewCellStyle6
        f_dひも長加算1.HeaderText = "ひも長加算"
        f_dひも長加算1.MinimumWidth = 6
        f_dひも長加算1.Name = "f_dひも長加算1"
        f_dひも長加算1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_dひも長加算1.ToolTipText = "各端に加算、全体では2倍分"
        f_dひも長加算1.Width = 125
        ' 
        ' f_d出力ひも長1
        ' 
        f_d出力ひも長1.DataPropertyName = "f_d出力ひも長"
        DataGridViewCellStyle7.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "N2"
        DataGridViewCellStyle7.NullValue = Nothing
        f_d出力ひも長1.DefaultCellStyle = DataGridViewCellStyle7
        f_d出力ひも長1.HeaderText = "出力ひも長"
        f_d出力ひも長1.MinimumWidth = 6
        f_d出力ひも長1.Name = "f_d出力ひも長1"
        f_d出力ひも長1.ReadOnly = True
        f_d出力ひも長1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_d出力ひも長1.Width = 125
        ' 
        ' f_s無効理由1
        ' 
        f_s無効理由1.DataPropertyName = "f_s無効理由"
        f_s無効理由1.HeaderText = "無効理由"
        f_s無効理由1.MinimumWidth = 6
        f_s無効理由1.Name = "f_s無効理由1"
        f_s無効理由1.ReadOnly = True
        f_s無効理由1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_s無効理由1.ToolTipText = "有効にならない理由"
        f_s無効理由1.Width = 125
        ' 
        ' f_sメモ1
        ' 
        f_sメモ1.DataPropertyName = "f_sメモ"
        f_sメモ1.HeaderText = "メモ"
        f_sメモ1.MinimumWidth = 6
        f_sメモ1.Name = "f_sメモ1"
        f_sメモ1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_sメモ1.Width = 125
        ' 
        ' Fi同位置順DataGridViewTextBoxColumn
        ' 
        Fi同位置順DataGridViewTextBoxColumn.DataPropertyName = "f_i同位置順"
        Fi同位置順DataGridViewTextBoxColumn.HeaderText = "f_i同位置順"
        Fi同位置順DataGridViewTextBoxColumn.MinimumWidth = 6
        Fi同位置順DataGridViewTextBoxColumn.Name = "Fi同位置順DataGridViewTextBoxColumn"
        Fi同位置順DataGridViewTextBoxColumn.Width = 125
        ' 
        ' f_i同位置数
        ' 
        f_i同位置数.DataPropertyName = "f_i同位置数"
        f_i同位置数.HeaderText = "f_i同位置数"
        f_i同位置数.MinimumWidth = 6
        f_i同位置数.Name = "f_i同位置数"
        f_i同位置数.Width = 125
        ' 
        ' ctrAdditionalBand
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
        Controls.Add(Panel)
        Name = "ctrAdditionalBand"
        Size = New System.Drawing.Size(867, 448)
        CType(BindingSource差しひも, ComponentModel.ISupportInitialize).EndInit()
        Panel.ResumeLayout(False)
        CType(dgv差しひも, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents BindingSource差しひも As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents Panel As Windows.Forms.Panel
    Friend WithEvents btn削除_差しひも As Windows.Forms.Button
    Friend WithEvents btn追加_差しひも As Windows.Forms.Button
    Friend WithEvents btn下へ_差しひも As Windows.Forms.Button
    Friend WithEvents btn上へ_差しひも As Windows.Forms.Button
    Friend WithEvents dgv差しひも As ctrDataGridView
    Friend WithEvents f_i番号1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_b有効区分1 As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents f_i配置面1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_i角度1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_i中心点1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_i何本幅1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_s色1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_i開始位置1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i何本ごと1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_iひも本数1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d出力ひも長1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s無効理由1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_sメモ1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fi同位置順DataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i同位置数 As Windows.Forms.DataGridViewTextBoxColumn

End Class
