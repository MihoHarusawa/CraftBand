<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrInsertBand
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
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New Windows.Forms.DataGridViewCellStyle()
        BindingSource差しひも = New System.Windows.Forms.BindingSource(components)
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        btn削除 = New Windows.Forms.Button()
        btn追加 = New Windows.Forms.Button()
        btn下へ = New Windows.Forms.Button()
        btn上へ = New Windows.Forms.Button()
        btn同位置 = New Windows.Forms.Button()
        btn複製 = New Windows.Forms.Button()
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
        f_i同位置数1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i同位置順1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i差し位置1 = New Windows.Forms.DataGridViewComboBoxColumn()
        f_dひも長1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_iひも本数1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_dひも長加算1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_d出力ひも長1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_s無効理由1 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i表示順 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_i非表示順 = New Windows.Forms.DataGridViewTextBoxColumn()
        f_sメモ1 = New Windows.Forms.DataGridViewTextBoxColumn()
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
        ' btn削除
        ' 
        btn削除.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn削除.Location = New System.Drawing.Point(357, 343)
        btn削除.Name = "btn削除"
        btn削除.Size = New System.Drawing.Size(111, 44)
        btn削除.TabIndex = 4
        btn削除.Text = "削除(&R)"
        ToolTip1.SetToolTip(btn削除, "選択した行を削除します")
        btn削除.UseVisualStyleBackColor = True
        ' 
        ' btn追加
        ' 
        btn追加.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn追加.Location = New System.Drawing.Point(723, 343)
        btn追加.Name = "btn追加"
        btn追加.Size = New System.Drawing.Size(111, 44)
        btn追加.TabIndex = 5
        btn追加.Text = "追加(&A)"
        ToolTip1.SetToolTip(btn追加, "行を追加します")
        btn追加.UseVisualStyleBackColor = True
        ' 
        ' btn下へ
        ' 
        btn下へ.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn下へ.Location = New System.Drawing.Point(123, 343)
        btn下へ.Name = "btn下へ"
        btn下へ.Size = New System.Drawing.Size(111, 44)
        btn下へ.TabIndex = 2
        btn下へ.Text = "下へ(&D)"
        ToolTip1.SetToolTip(btn下へ, "選択した行を下に移動します")
        btn下へ.UseVisualStyleBackColor = True
        ' 
        ' btn上へ
        ' 
        btn上へ.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn上へ.Location = New System.Drawing.Point(6, 343)
        btn上へ.Name = "btn上へ"
        btn上へ.Size = New System.Drawing.Size(111, 44)
        btn上へ.TabIndex = 1
        btn上へ.Text = "上へ(&U)"
        ToolTip1.SetToolTip(btn上へ, "選択した行を上に移動します")
        btn上へ.UseVisualStyleBackColor = True
        ' 
        ' btn同位置
        ' 
        btn同位置.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btn同位置.Location = New System.Drawing.Point(240, 343)
        btn同位置.Name = "btn同位置"
        btn同位置.Size = New System.Drawing.Size(111, 44)
        btn同位置.TabIndex = 3
        btn同位置.Text = "同位置(&S)"
        ToolTip1.SetToolTip(btn同位置, "同位置数・同位置順を再設定します")
        btn同位置.UseVisualStyleBackColor = True
        ' 
        ' btn複製
        ' 
        btn複製.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn複製.Location = New System.Drawing.Point(606, 344)
        btn複製.Name = "btn複製"
        btn複製.Size = New System.Drawing.Size(111, 44)
        btn複製.TabIndex = 6
        btn複製.Text = "複製(&P)"
        ToolTip1.SetToolTip(btn複製, "行を追加します")
        btn複製.UseVisualStyleBackColor = True
        ' 
        ' Panel
        ' 
        Panel.Controls.Add(btn複製)
        Panel.Controls.Add(btn同位置)
        Panel.Controls.Add(btn削除)
        Panel.Controls.Add(btn追加)
        Panel.Controls.Add(btn下へ)
        Panel.Controls.Add(btn上へ)
        Panel.Controls.Add(dgv差しひも)
        Panel.Location = New System.Drawing.Point(3, 3)
        Panel.Name = "Panel"
        Panel.Size = New System.Drawing.Size(840, 392)
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
        dgv差しひも.Columns.AddRange(New Windows.Forms.DataGridViewColumn() {f_i番号1, f_b有効区分1, f_i配置面1, f_i角度1, f_i中心点1, f_i何本幅1, f_s色1, f_i開始位置1, f_i何本ごと1, f_i同位置数1, f_i同位置順1, f_i差し位置1, f_dひも長1, f_iひも本数1, f_dひも長加算1, f_d出力ひも長1, f_s無効理由1, f_i表示順, f_i非表示順, f_sメモ1})
        dgv差しひも.DataSource = BindingSource差しひも
        dgv差しひも.Location = New System.Drawing.Point(6, 6)
        dgv差しひも.Name = "dgv差しひも"
        dgv差しひも.RowHeadersWidth = 51
        dgv差しひも.RowTemplate.Height = 29
        dgv差しひも.Size = New System.Drawing.Size(828, 332)
        dgv差しひも.TabIndex = 0
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
        f_i開始位置1.ToolTipText = "1は初めから、2はひとつ空けて、3は二つ空けて.."
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
        ' f_i同位置数1
        ' 
        f_i同位置数1.DataPropertyName = "f_i同位置数"
        DataGridViewCellStyle4.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i同位置数1.DefaultCellStyle = DataGridViewCellStyle4
        f_i同位置数1.HeaderText = "同位置数"
        f_i同位置数1.MinimumWidth = 6
        f_i同位置数1.Name = "f_i同位置数1"
        f_i同位置数1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i同位置数1.ToolTipText = "配置面・角度・中心点・開始位置・何本ごと　が同じ数"
        f_i同位置数1.Width = 125
        ' 
        ' f_i同位置順1
        ' 
        f_i同位置順1.DataPropertyName = "f_i同位置順"
        DataGridViewCellStyle5.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i同位置順1.DefaultCellStyle = DataGridViewCellStyle5
        f_i同位置順1.HeaderText = "同位置順"
        f_i同位置順1.MinimumWidth = 6
        f_i同位置順1.Name = "f_i同位置順1"
        f_i同位置順1.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i同位置順1.ToolTipText = "同位置数の中で何番目か。ゼロ/空は指定なし"
        f_i同位置順1.Width = 125
        ' 
        ' f_i差し位置1
        ' 
        f_i差し位置1.DataPropertyName = "f_i差し位置"
        f_i差し位置1.HeaderText = "差し位置"
        f_i差し位置1.MinimumWidth = 6
        f_i差し位置1.Name = "f_i差し位置1"
        f_i差し位置1.Resizable = Windows.Forms.DataGridViewTriState.True
        f_i差し位置1.SortMode = Windows.Forms.DataGridViewColumnSortMode.Automatic
        f_i差し位置1.Width = 125
        ' 
        ' f_dひも長1
        ' 
        f_dひも長1.DataPropertyName = "f_dひも長"
        DataGridViewCellStyle6.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N2"
        DataGridViewCellStyle6.NullValue = Nothing
        f_dひも長1.DefaultCellStyle = DataGridViewCellStyle6
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
        DataGridViewCellStyle7.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_iひも本数1.DefaultCellStyle = DataGridViewCellStyle7
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
        DataGridViewCellStyle8.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_dひも長加算1.DefaultCellStyle = DataGridViewCellStyle8
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
        DataGridViewCellStyle9.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "N2"
        DataGridViewCellStyle9.NullValue = Nothing
        f_d出力ひも長1.DefaultCellStyle = DataGridViewCellStyle9
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
        ' f_i表示順
        ' 
        f_i表示順.DataPropertyName = "f_i表示順"
        DataGridViewCellStyle10.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i表示順.DefaultCellStyle = DataGridViewCellStyle10
        f_i表示順.HeaderText = "表示順"
        f_i表示順.Name = "f_i表示順"
        f_i表示順.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i表示順.Visible = False
        ' 
        ' f_i非表示順
        ' 
        f_i非表示順.DataPropertyName = "f_i非表示順"
        DataGridViewCellStyle11.Alignment = Windows.Forms.DataGridViewContentAlignment.MiddleRight
        f_i非表示順.DefaultCellStyle = DataGridViewCellStyle11
        f_i非表示順.HeaderText = "非表示順"
        f_i非表示順.Name = "f_i非表示順"
        f_i非表示順.SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
        f_i非表示順.Visible = False
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
        ' ctrInsertBand
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
        Controls.Add(Panel)
        Name = "ctrInsertBand"
        Size = New System.Drawing.Size(867, 426)
        CType(BindingSource差しひも, ComponentModel.ISupportInitialize).EndInit()
        Panel.ResumeLayout(False)
        CType(dgv差しひも, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents BindingSource差しひも As Windows.Forms.BindingSource
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents Panel As Windows.Forms.Panel
    Friend WithEvents btn削除 As Windows.Forms.Button
    Friend WithEvents btn追加 As Windows.Forms.Button
    Friend WithEvents btn下へ As Windows.Forms.Button
    Friend WithEvents btn上へ As Windows.Forms.Button
    Friend WithEvents dgv差しひも As ctrDataGridView
    Friend WithEvents btn同位置 As Windows.Forms.Button
    Friend WithEvents f_i番号1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_b有効区分1 As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents f_i配置面1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_i角度1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_i中心点1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_i何本幅1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_s色1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_i開始位置1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i何本ごと1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i同位置数1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i同位置順1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i差し位置1 As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents f_dひも長1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_iひも本数1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_d出力ひも長1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_s無効理由1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i表示順 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_i非表示順 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents f_sメモ1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btn複製 As Windows.Forms.Button

End Class
