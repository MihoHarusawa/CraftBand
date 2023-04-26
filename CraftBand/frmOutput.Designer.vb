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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOutput))
        Me.dgvOutput = New ctrDataGridView()
        Me.BindingSourceOutput = New System.Windows.Forms.BindingSource(Me.components)
        Me.btn閉じる = New System.Windows.Forms.Button()
        Me.btnCSV出力 = New System.Windows.Forms.Button()
        Me.btnTXT出力 = New System.Windows.Forms.Button()
        Me.f_iNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_b空行区分 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_sカテゴリー = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s番号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s記号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s本幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_sひも本数 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_sひも長 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s色 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_sタイプ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s編みかた名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s編みひも名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_i周数 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_i段数 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s高さ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s長さ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_sメモ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvOutput
        '
        Me.dgvOutput.AllowUserToAddRows = False
        Me.dgvOutput.AllowUserToDeleteRows = False
        Me.dgvOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvOutput.AutoGenerateColumns = False
        Me.dgvOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOutput.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.f_iNo, Me.f_b空行区分, Me.f_sカテゴリー, Me.f_s番号, Me.f_s記号, Me.f_s本幅, Me.f_sひも本数, Me.f_sひも長, Me.f_s色, Me.f_sタイプ, Me.f_s編みかた名, Me.f_s編みひも名, Me.f_i周数, Me.f_i段数, Me.f_s高さ, Me.f_s長さ, Me.f_sメモ})
        Me.dgvOutput.DataSource = Me.BindingSourceOutput
        Me.dgvOutput.Location = New System.Drawing.Point(12, 12)
        Me.dgvOutput.Name = "dgvOutput"
        Me.dgvOutput.ReadOnly = True
        Me.dgvOutput.RowHeadersWidth = 51
        Me.dgvOutput.RowTemplate.Height = 29
        Me.dgvOutput.Size = New System.Drawing.Size(552, 175)
        Me.dgvOutput.TabIndex = 0
        '
        'BindingSourceOutput
        '
        Me.BindingSourceOutput.DataMember = "tblOutput"
        Me.BindingSourceOutput.DataSource = GetType(CraftBand.Tables.dstOutput)
        '
        'btn閉じる
        '
        Me.btn閉じる.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn閉じる.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn閉じる.Location = New System.Drawing.Point(453, 193)
        Me.btn閉じる.Name = "btn閉じる"
        Me.btn閉じる.Size = New System.Drawing.Size(111, 46)
        Me.btn閉じる.TabIndex = 28
        Me.btn閉じる.Text = "閉じる(&C)"
        Me.btn閉じる.UseVisualStyleBackColor = True
        '
        'btnCSV出力
        '
        Me.btnCSV出力.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCSV出力.Location = New System.Drawing.Point(335, 193)
        Me.btnCSV出力.Name = "btnCSV出力"
        Me.btnCSV出力.Size = New System.Drawing.Size(111, 46)
        Me.btnCSV出力.TabIndex = 27
        Me.btnCSV出力.Text = "CSV出力(&O)"
        Me.btnCSV出力.UseVisualStyleBackColor = True
        '
        'btnTXT出力
        '
        Me.btnTXT出力.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTXT出力.Location = New System.Drawing.Point(217, 193)
        Me.btnTXT出力.Name = "btnTXT出力"
        Me.btnTXT出力.Size = New System.Drawing.Size(111, 46)
        Me.btnTXT出力.TabIndex = 29
        Me.btnTXT出力.Text = "TXT出力(&T)"
        Me.btnTXT出力.UseVisualStyleBackColor = True
        '
        'f_iNo
        '
        Me.f_iNo.DataPropertyName = "f_iNo"
        Me.f_iNo.HeaderText = "No"
        Me.f_iNo.MinimumWidth = 6
        Me.f_iNo.Name = "f_iNo"
        Me.f_iNo.ReadOnly = True
        Me.f_iNo.Visible = False
        Me.f_iNo.Width = 125
        '
        'f_b空行区分
        '
        Me.f_b空行区分.DataPropertyName = "f_b空行区分"
        Me.f_b空行区分.HeaderText = "空行区分"
        Me.f_b空行区分.MinimumWidth = 6
        Me.f_b空行区分.Name = "f_b空行区分"
        Me.f_b空行区分.ReadOnly = True
        Me.f_b空行区分.Visible = False
        Me.f_b空行区分.Width = 125
        '
        'f_sカテゴリー
        '
        Me.f_sカテゴリー.DataPropertyName = "f_sカテゴリー"
        Me.f_sカテゴリー.HeaderText = "カテゴリー"
        Me.f_sカテゴリー.MinimumWidth = 6
        Me.f_sカテゴリー.Name = "f_sカテゴリー"
        Me.f_sカテゴリー.ReadOnly = True
        Me.f_sカテゴリー.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_sカテゴリー.Width = 85
        '
        'f_s番号
        '
        Me.f_s番号.DataPropertyName = "f_s番号"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_s番号.DefaultCellStyle = DataGridViewCellStyle1
        Me.f_s番号.HeaderText = "番号"
        Me.f_s番号.MinimumWidth = 6
        Me.f_s番号.Name = "f_s番号"
        Me.f_s番号.ReadOnly = True
        Me.f_s番号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s番号.Width = 26
        '
        'f_s記号
        '
        Me.f_s記号.DataPropertyName = "f_s記号"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter
        Me.f_s記号.DefaultCellStyle = DataGridViewCellStyle2
        Me.f_s記号.HeaderText = "記号"
        Me.f_s記号.MinimumWidth = 6
        Me.f_s記号.Name = "f_s記号"
        Me.f_s記号.ReadOnly = True
        Me.f_s記号.Width = 57
        '
        'f_s本幅
        '
        Me.f_s本幅.DataPropertyName = "f_s本幅"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_s本幅.DefaultCellStyle = DataGridViewCellStyle3
        Me.f_s本幅.HeaderText = "本幅"
        Me.f_s本幅.MinimumWidth = 6
        Me.f_s本幅.Name = "f_s本幅"
        Me.f_s本幅.ReadOnly = True
        Me.f_s本幅.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s本幅.Width = 57
        '
        'f_sひも本数
        '
        Me.f_sひも本数.DataPropertyName = "f_sひも本数"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_sひも本数.DefaultCellStyle = DataGridViewCellStyle4
        Me.f_sひも本数.HeaderText = "ひも本数"
        Me.f_sひも本数.MinimumWidth = 6
        Me.f_sひも本数.Name = "f_sひも本数"
        Me.f_sひも本数.ReadOnly = True
        Me.f_sひも本数.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_sひも本数.Width = 95
        '
        'f_sひも長
        '
        Me.f_sひも長.DataPropertyName = "f_sひも長"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_sひも長.DefaultCellStyle = DataGridViewCellStyle5
        Me.f_sひも長.HeaderText = "ひも長"
        Me.f_sひも長.MinimumWidth = 6
        Me.f_sひも長.Name = "f_sひも長"
        Me.f_sひも長.ReadOnly = True
        Me.f_sひも長.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_sひも長.Width = 75
        '
        'f_s色
        '
        Me.f_s色.DataPropertyName = "f_s色"
        Me.f_s色.HeaderText = "色"
        Me.f_s色.MinimumWidth = 6
        Me.f_s色.Name = "f_s色"
        Me.f_s色.ReadOnly = True
        Me.f_s色.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s色.Width = 125
        '
        'f_sタイプ
        '
        Me.f_sタイプ.DataPropertyName = "f_sタイプ"
        Me.f_sタイプ.HeaderText = "タイプ"
        Me.f_sタイプ.MinimumWidth = 6
        Me.f_sタイプ.Name = "f_sタイプ"
        Me.f_sタイプ.ReadOnly = True
        Me.f_sタイプ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_sタイプ.Width = 60
        '
        'f_s編みかた名
        '
        Me.f_s編みかた名.DataPropertyName = "f_s編みかた名"
        Me.f_s編みかた名.HeaderText = "編みかた名"
        Me.f_s編みかた名.MinimumWidth = 6
        Me.f_s編みかた名.Name = "f_s編みかた名"
        Me.f_s編みかた名.ReadOnly = True
        Me.f_s編みかた名.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s編みかた名.Width = 125
        '
        'f_s編みひも名
        '
        Me.f_s編みひも名.DataPropertyName = "f_s編みひも名"
        Me.f_s編みひも名.HeaderText = "編みひも名"
        Me.f_s編みひも名.MinimumWidth = 6
        Me.f_s編みひも名.Name = "f_s編みひも名"
        Me.f_s編みひも名.ReadOnly = True
        Me.f_s編みひも名.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s編みひも名.Width = 98
        '
        'f_i周数
        '
        Me.f_i周数.DataPropertyName = "f_i周数"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_i周数.DefaultCellStyle = DataGridViewCellStyle6
        Me.f_i周数.HeaderText = "周数"
        Me.f_i周数.MinimumWidth = 6
        Me.f_i周数.Name = "f_i周数"
        Me.f_i周数.ReadOnly = True
        Me.f_i周数.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_i周数.Width = 46
        '
        'f_i段数
        '
        Me.f_i段数.DataPropertyName = "f_i段数"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_i段数.DefaultCellStyle = DataGridViewCellStyle7
        Me.f_i段数.HeaderText = "段数"
        Me.f_i段数.MinimumWidth = 6
        Me.f_i段数.Name = "f_i段数"
        Me.f_i段数.ReadOnly = True
        Me.f_i段数.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_i段数.Width = 37
        '
        'f_s高さ
        '
        Me.f_s高さ.DataPropertyName = "f_s高さ"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_s高さ.DefaultCellStyle = DataGridViewCellStyle8
        Me.f_s高さ.HeaderText = "高さ"
        Me.f_s高さ.MinimumWidth = 6
        Me.f_s高さ.Name = "f_s高さ"
        Me.f_s高さ.ReadOnly = True
        Me.f_s高さ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s高さ.Width = 65
        '
        'f_s長さ
        '
        Me.f_s長さ.DataPropertyName = "f_s長さ"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_s長さ.DefaultCellStyle = DataGridViewCellStyle9
        Me.f_s長さ.HeaderText = "長さ"
        Me.f_s長さ.MinimumWidth = 6
        Me.f_s長さ.Name = "f_s長さ"
        Me.f_s長さ.ReadOnly = True
        Me.f_s長さ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s長さ.Width = 55
        '
        'f_sメモ
        '
        Me.f_sメモ.DataPropertyName = "f_sメモ"
        Me.f_sメモ.HeaderText = "メモ"
        Me.f_sメモ.MinimumWidth = 6
        Me.f_sメモ.Name = "f_sメモ"
        Me.f_sメモ.ReadOnly = True
        Me.f_sメモ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_sメモ.Width = 125
        '
        'frmOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(576, 244)
        Me.Controls.Add(Me.btnTXT出力)
        Me.Controls.Add(Me.btn閉じる)
        Me.Controls.Add(Me.btnCSV出力)
        Me.Controls.Add(Me.dgvOutput)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmOutput"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ひもリスト"
        CType(Me.dgvOutput, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceOutput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvOutput As ctrDataGridView
    Friend WithEvents btn閉じる As Button
    Friend WithEvents btnCSV出力 As Button
    Friend WithEvents BindingSourceOutput As BindingSource
    Friend WithEvents btnTXT出力 As Button
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
