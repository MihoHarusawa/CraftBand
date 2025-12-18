Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmOutput
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
        components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOutput))
        dgvOutput = New ctrDataGridView()
        BindingSourceOutput = New BindingSource(components)
        btn閉じる = New Button()
        btnCSV出力 = New Button()
        btnTXT出力 = New Button()
        ToolTip1 = New ToolTip(components)
        f_iNo = New DataGridViewTextBoxColumn()
        f_b空行区分 = New DataGridViewCheckBoxColumn()
        f_sカテゴリー = New DataGridViewTextBoxColumn()
        f_s番号 = New DataGridViewTextBoxColumn()
        f_s記号 = New DataGridViewTextBoxColumn()
        f_s本幅 = New DataGridViewTextBoxColumn()
        f_sひも本数 = New DataGridViewTextBoxColumn()
        f_sひも長 = New DataGridViewTextBoxColumn()
        f_s色 = New DataGridViewTextBoxColumn()
        f_sタイプ = New DataGridViewTextBoxColumn()
        f_s編みかた名 = New DataGridViewTextBoxColumn()
        f_s編みひも名 = New DataGridViewTextBoxColumn()
        f_i周数 = New DataGridViewTextBoxColumn()
        f_i段数 = New DataGridViewTextBoxColumn()
        f_s高さ = New DataGridViewTextBoxColumn()
        f_s長さ = New DataGridViewTextBoxColumn()
        f_sメモ = New DataGridViewTextBoxColumn()
        CType(dgvOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSourceOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvOutput
        ' 
        dgvOutput.AllowUserToAddRows = False
        dgvOutput.AllowUserToDeleteRows = False
        dgvOutput.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvOutput.AutoGenerateColumns = False
        dgvOutput.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvOutput.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvOutput.Columns.AddRange(New DataGridViewColumn() {f_iNo, f_b空行区分, f_sカテゴリー, f_s番号, f_s記号, f_s本幅, f_sひも本数, f_sひも長, f_s色, f_sタイプ, f_s編みかた名, f_s編みひも名, f_i周数, f_i段数, f_s高さ, f_s長さ, f_sメモ})
        dgvOutput.DataSource = BindingSourceOutput
        dgvOutput.Location = New System.Drawing.Point(12, 11)
        dgvOutput.Name = "dgvOutput"
        dgvOutput.ReadOnly = True
        dgvOutput.RowHeadersWidth = 51
        dgvOutput.RowTemplate.Height = 29
        dgvOutput.Size = New System.Drawing.Size(552, 166)
        dgvOutput.TabIndex = 0
        ' 
        ' BindingSourceOutput
        ' 
        BindingSourceOutput.DataMember = "tblOutput"
        BindingSourceOutput.DataSource = GetType(Tables.dstOutput)
        ' 
        ' btn閉じる
        ' 
        btn閉じる.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btn閉じる.DialogResult = DialogResult.Cancel
        btn閉じる.Location = New System.Drawing.Point(453, 183)
        btn閉じる.Name = "btn閉じる"
        btn閉じる.Size = New System.Drawing.Size(111, 44)
        btn閉じる.TabIndex = 28
        btn閉じる.Text = "閉じる(&C)"
        ToolTip1.SetToolTip(btn閉じる, "この画面を閉じます")
        btn閉じる.UseVisualStyleBackColor = True
        ' 
        ' btnCSV出力
        ' 
        btnCSV出力.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnCSV出力.Location = New System.Drawing.Point(335, 183)
        btnCSV出力.Name = "btnCSV出力"
        btnCSV出力.Size = New System.Drawing.Size(111, 44)
        btnCSV出力.TabIndex = 27
        btnCSV出力.Text = "CSV出力(&O)"
        ToolTip1.SetToolTip(btnCSV出力, "CSVファイルとして開きます")
        btnCSV出力.UseVisualStyleBackColor = True
        ' 
        ' btnTXT出力
        ' 
        btnTXT出力.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnTXT出力.Location = New System.Drawing.Point(217, 183)
        btnTXT出力.Name = "btnTXT出力"
        btnTXT出力.Size = New System.Drawing.Size(111, 44)
        btnTXT出力.TabIndex = 29
        btnTXT出力.Text = "TXT出力(&T)"
        ToolTip1.SetToolTip(btnTXT出力, "テキストファイルとして開きます")
        btnTXT出力.UseVisualStyleBackColor = True
        ' 
        ' f_iNo
        ' 
        f_iNo.DataPropertyName = "f_iNo"
        f_iNo.HeaderText = "No"
        f_iNo.MinimumWidth = 6
        f_iNo.Name = "f_iNo"
        f_iNo.ReadOnly = True
        f_iNo.Visible = False
        f_iNo.Width = 125
        ' 
        ' f_b空行区分
        ' 
        f_b空行区分.DataPropertyName = "f_b空行区分"
        f_b空行区分.HeaderText = "空行区分"
        f_b空行区分.MinimumWidth = 6
        f_b空行区分.Name = "f_b空行区分"
        f_b空行区分.ReadOnly = True
        f_b空行区分.Visible = False
        f_b空行区分.Width = 125
        ' 
        ' f_sカテゴリー
        ' 
        f_sカテゴリー.DataPropertyName = "f_sカテゴリー"
        f_sカテゴリー.HeaderText = "カテゴリー"
        f_sカテゴリー.MinimumWidth = 6
        f_sカテゴリー.Name = "f_sカテゴリー"
        f_sカテゴリー.ReadOnly = True
        f_sカテゴリー.SortMode = DataGridViewColumnSortMode.NotSortable
        f_sカテゴリー.Width = 85
        ' 
        ' f_s番号
        ' 
        f_s番号.DataPropertyName = "f_s番号"
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight
        f_s番号.DefaultCellStyle = DataGridViewCellStyle1
        f_s番号.HeaderText = "番号"
        f_s番号.MinimumWidth = 6
        f_s番号.Name = "f_s番号"
        f_s番号.ReadOnly = True
        f_s番号.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s番号.Width = 26
        ' 
        ' f_s記号
        ' 
        f_s記号.DataPropertyName = "f_s記号"
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.BottomCenter
        f_s記号.DefaultCellStyle = DataGridViewCellStyle2
        f_s記号.HeaderText = "記号"
        f_s記号.MinimumWidth = 6
        f_s記号.Name = "f_s記号"
        f_s記号.ReadOnly = True
        f_s記号.Width = 57
        ' 
        ' f_s本幅
        ' 
        f_s本幅.DataPropertyName = "f_s本幅"
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight
        f_s本幅.DefaultCellStyle = DataGridViewCellStyle3
        f_s本幅.HeaderText = "本幅"
        f_s本幅.MinimumWidth = 6
        f_s本幅.Name = "f_s本幅"
        f_s本幅.ReadOnly = True
        f_s本幅.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s本幅.Width = 57
        ' 
        ' f_sひも本数
        ' 
        f_sひも本数.DataPropertyName = "f_sひも本数"
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight
        f_sひも本数.DefaultCellStyle = DataGridViewCellStyle4
        f_sひも本数.HeaderText = "ひも本数"
        f_sひも本数.MinimumWidth = 6
        f_sひも本数.Name = "f_sひも本数"
        f_sひも本数.ReadOnly = True
        f_sひも本数.SortMode = DataGridViewColumnSortMode.NotSortable
        f_sひも本数.Width = 95
        ' 
        ' f_sひも長
        ' 
        f_sひも長.DataPropertyName = "f_sひも長"
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight
        f_sひも長.DefaultCellStyle = DataGridViewCellStyle5
        f_sひも長.HeaderText = "ひも長"
        f_sひも長.MinimumWidth = 6
        f_sひも長.Name = "f_sひも長"
        f_sひも長.ReadOnly = True
        f_sひも長.SortMode = DataGridViewColumnSortMode.NotSortable
        f_sひも長.Width = 75
        ' 
        ' f_s色
        ' 
        f_s色.DataPropertyName = "f_s色"
        f_s色.HeaderText = "色"
        f_s色.MinimumWidth = 6
        f_s色.Name = "f_s色"
        f_s色.ReadOnly = True
        f_s色.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s色.Width = 125
        ' 
        ' f_sタイプ
        ' 
        f_sタイプ.DataPropertyName = "f_sタイプ"
        f_sタイプ.HeaderText = "タイプ"
        f_sタイプ.MinimumWidth = 6
        f_sタイプ.Name = "f_sタイプ"
        f_sタイプ.ReadOnly = True
        f_sタイプ.SortMode = DataGridViewColumnSortMode.NotSortable
        f_sタイプ.Width = 60
        ' 
        ' f_s編みかた名
        ' 
        f_s編みかた名.DataPropertyName = "f_s編みかた名"
        f_s編みかた名.HeaderText = "編みかた名"
        f_s編みかた名.MinimumWidth = 6
        f_s編みかた名.Name = "f_s編みかた名"
        f_s編みかた名.ReadOnly = True
        f_s編みかた名.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s編みかた名.Width = 125
        ' 
        ' f_s編みひも名
        ' 
        f_s編みひも名.DataPropertyName = "f_s編みひも名"
        f_s編みひも名.HeaderText = "編みひも名"
        f_s編みひも名.MinimumWidth = 6
        f_s編みひも名.Name = "f_s編みひも名"
        f_s編みひも名.ReadOnly = True
        f_s編みひも名.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s編みひも名.Width = 98
        ' 
        ' f_i周数
        ' 
        f_i周数.DataPropertyName = "f_i周数"
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight
        f_i周数.DefaultCellStyle = DataGridViewCellStyle6
        f_i周数.HeaderText = "周数"
        f_i周数.MinimumWidth = 6
        f_i周数.Name = "f_i周数"
        f_i周数.ReadOnly = True
        f_i周数.SortMode = DataGridViewColumnSortMode.NotSortable
        f_i周数.Width = 46
        ' 
        ' f_i段数
        ' 
        f_i段数.DataPropertyName = "f_i段数"
        DataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight
        f_i段数.DefaultCellStyle = DataGridViewCellStyle7
        f_i段数.HeaderText = "段数"
        f_i段数.MinimumWidth = 6
        f_i段数.Name = "f_i段数"
        f_i段数.ReadOnly = True
        f_i段数.SortMode = DataGridViewColumnSortMode.NotSortable
        f_i段数.Width = 37
        ' 
        ' f_s高さ
        ' 
        f_s高さ.DataPropertyName = "f_s高さ"
        DataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight
        f_s高さ.DefaultCellStyle = DataGridViewCellStyle8
        f_s高さ.HeaderText = "高さ"
        f_s高さ.MinimumWidth = 6
        f_s高さ.Name = "f_s高さ"
        f_s高さ.ReadOnly = True
        f_s高さ.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s高さ.Width = 65
        ' 
        ' f_s長さ
        ' 
        f_s長さ.DataPropertyName = "f_s長さ"
        DataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight
        f_s長さ.DefaultCellStyle = DataGridViewCellStyle9
        f_s長さ.HeaderText = "長さ"
        f_s長さ.MinimumWidth = 6
        f_s長さ.Name = "f_s長さ"
        f_s長さ.ReadOnly = True
        f_s長さ.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s長さ.Width = 55
        ' 
        ' f_sメモ
        ' 
        f_sメモ.DataPropertyName = "f_sメモ"
        f_sメモ.HeaderText = "メモ"
        f_sメモ.MinimumWidth = 6
        f_sメモ.Name = "f_sメモ"
        f_sメモ.ReadOnly = True
        f_sメモ.SortMode = DataGridViewColumnSortMode.NotSortable
        f_sメモ.Width = 125
        ' 
        ' frmOutput
        ' 
        AcceptButton = btn閉じる
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(576, 232)
        Controls.Add(btnTXT出力)
        Controls.Add(btn閉じる)
        Controls.Add(btnCSV出力)
        Controls.Add(dgvOutput)
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Name = "frmOutput"
        StartPosition = FormStartPosition.CenterParent
        Text = "ひもリスト"
        CType(dgvOutput, System.ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSourceOutput, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents dgvOutput As ctrDataGridView
    Friend WithEvents btn閉じる As Button
    Friend WithEvents btnCSV出力 As Button
    Friend WithEvents BindingSourceOutput As BindingSource
    Friend WithEvents btnTXT出力 As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents f_iNo As DataGridViewTextBoxColumn
    Friend WithEvents f_b空行区分 As DataGridViewCheckBoxColumn
    Friend WithEvents f_sカテゴリー As DataGridViewTextBoxColumn
    Friend WithEvents f_s番号 As DataGridViewTextBoxColumn
    Friend WithEvents f_s記号 As DataGridViewTextBoxColumn
    Friend WithEvents f_s本幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_sひも本数 As DataGridViewTextBoxColumn
    Friend WithEvents f_sひも長 As DataGridViewTextBoxColumn
    Friend WithEvents f_s色 As DataGridViewTextBoxColumn
    Friend WithEvents f_sタイプ As DataGridViewTextBoxColumn
    Friend WithEvents f_s編みかた名 As DataGridViewTextBoxColumn
    Friend WithEvents f_s編みひも名 As DataGridViewTextBoxColumn
    Friend WithEvents f_i周数 As DataGridViewTextBoxColumn
    Friend WithEvents f_i段数 As DataGridViewTextBoxColumn
    Friend WithEvents f_s高さ As DataGridViewTextBoxColumn
    Friend WithEvents f_s長さ As DataGridViewTextBoxColumn
    Friend WithEvents f_sメモ As DataGridViewTextBoxColumn
End Class
