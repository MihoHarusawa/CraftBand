Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmOptions
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
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
        Dim DataGridViewCellStyle10 As DataGridViewCellStyle = New DataGridViewCellStyle()
        dgvData = New ctrDataGridView()
        BindingSource付属品 = New BindingSource(components)
        lbl設定時の寸法単位 = New Label()
        lbl単位 = New Label()
        btnキャンセル = New Button()
        btnOK = New Button()
        btnひも追加 = New Button()
        lbl付属品名 = New Label()
        txt付属品名 = New TextBox()
        btn削除 = New Button()
        ToolTip1 = New ToolTip(components)
        f_s付属品名 = New DataGridViewTextBoxColumn()
        f_iひも番号 = New DataGridViewTextBoxColumn()
        f_s付属品ひも名 = New DataGridViewTextBoxColumn()
        f_b巻きひも区分 = New DataGridViewCheckBoxColumn()
        f_i本幅初期値 = New DataGridViewTextBoxColumn()
        f_iひも数 = New DataGridViewTextBoxColumn()
        f_d長さ比率対ひも1 = New DataGridViewTextBoxColumn()
        f_d長さ加減対ひも1 = New DataGridViewTextBoxColumn()
        f_dひも長比率対長さ = New DataGridViewTextBoxColumn()
        f_dひも長加算 = New DataGridViewTextBoxColumn()
        f_d巻きの厚み = New DataGridViewTextBoxColumn()
        f_d巻き回数比率 = New DataGridViewTextBoxColumn()
        f_dひも長加算初期値 = New DataGridViewTextBoxColumn()
        f_bCraftBandMesh = New DataGridViewCheckBoxColumn()
        f_bCraftBandSquare45 = New DataGridViewCheckBoxColumn()
        f_bCraftBandKnot = New DataGridViewCheckBoxColumn()
        f_bCraftBandSquare = New DataGridViewCheckBoxColumn()
        f_bCraftBandHexagon = New DataGridViewCheckBoxColumn()
        f_s備考 = New DataGridViewTextBoxColumn()
        CType(dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSource付属品, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvData
        ' 
        dgvData.AllowUserToAddRows = False
        dgvData.AllowUserToDeleteRows = False
        dgvData.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvData.AutoGenerateColumns = False
        dgvData.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvData.Columns.AddRange(New DataGridViewColumn() {f_s付属品名, f_iひも番号, f_s付属品ひも名, f_b巻きひも区分, f_i本幅初期値, f_iひも数, f_d長さ比率対ひも1, f_d長さ加減対ひも1, f_dひも長比率対長さ, f_dひも長加算, f_d巻きの厚み, f_d巻き回数比率, f_dひも長加算初期値, f_bCraftBandMesh, f_bCraftBandSquare45, f_bCraftBandKnot, f_bCraftBandSquare, f_bCraftBandHexagon, f_s備考})
        dgvData.DataSource = BindingSource付属品
        dgvData.Location = New System.Drawing.Point(12, 32)
        dgvData.Name = "dgvData"
        dgvData.RowHeadersWidth = 51
        dgvData.RowTemplate.Height = 29
        dgvData.Size = New System.Drawing.Size(731, 166)
        dgvData.TabIndex = 2
        ' 
        ' BindingSource付属品
        ' 
        BindingSource付属品.DataMember = "tbl付属品"
        BindingSource付属品.DataSource = GetType(Tables.dstMasterTables)
        ' 
        ' lbl設定時の寸法単位
        ' 
        lbl設定時の寸法単位.AutoSize = True
        lbl設定時の寸法単位.Location = New System.Drawing.Point(62, -2)
        lbl設定時の寸法単位.Name = "lbl設定時の寸法単位"
        lbl設定時の寸法単位.Size = New System.Drawing.Size(0, 20)
        lbl設定時の寸法単位.TabIndex = 1
        ' 
        ' lbl単位
        ' 
        lbl単位.AutoSize = True
        lbl単位.Location = New System.Drawing.Point(14, -2)
        lbl単位.Name = "lbl単位"
        lbl単位.Size = New System.Drawing.Size(42, 20)
        lbl単位.TabIndex = 0
        lbl単位.Text = "単位:"
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnキャンセル.DialogResult = DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(632, 211)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 46)
        btnキャンセル.TabIndex = 8
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "変更を保存せずに終了します")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(513, 211)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 46)
        btnOK.TabIndex = 7
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "変更を保存して終了します")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnひも追加
        ' 
        btnひも追加.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnひも追加.Location = New System.Drawing.Point(333, 211)
        btnひも追加.Name = "btnひも追加"
        btnひも追加.Size = New System.Drawing.Size(111, 46)
        btnひも追加.TabIndex = 6
        btnひも追加.Text = "ひも追加(&A)"
        ToolTip1.SetToolTip(btnひも追加, "新たな付属品名を追加、" & vbCrLf & "もしくは既存の付属品名に付属品ひもを追加")
        btnひも追加.UseVisualStyleBackColor = True
        ' 
        ' lbl付属品名
        ' 
        lbl付属品名.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl付属品名.AutoSize = True
        lbl付属品名.Location = New System.Drawing.Point(134, 225)
        lbl付属品名.Name = "lbl付属品名"
        lbl付属品名.Size = New System.Drawing.Size(69, 20)
        lbl付属品名.TabIndex = 4
        lbl付属品名.Text = "付属品名"
        ' 
        ' txt付属品名
        ' 
        txt付属品名.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txt付属品名.Location = New System.Drawing.Point(216, 222)
        txt付属品名.Name = "txt付属品名"
        txt付属品名.Size = New System.Drawing.Size(101, 27)
        txt付属品名.TabIndex = 5
        ToolTip1.SetToolTip(txt付属品名, "付属品の名前")
        ' 
        ' btn削除
        ' 
        btn削除.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btn削除.Location = New System.Drawing.Point(14, 211)
        btn削除.Name = "btn削除"
        btn削除.Size = New System.Drawing.Size(111, 46)
        btn削除.TabIndex = 3
        btn削除.Text = "削除(&D)"
        ToolTip1.SetToolTip(btn削除, "選択している付属品を削除します")
        btn削除.UseVisualStyleBackColor = True
        ' 
        ' f_s付属品名
        ' 
        f_s付属品名.DataPropertyName = "f_s付属品名"
        f_s付属品名.HeaderText = "付属品名"
        f_s付属品名.MinimumWidth = 6
        f_s付属品名.Name = "f_s付属品名"
        f_s付属品名.ReadOnly = True
        f_s付属品名.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s付属品名.Width = 155
        ' 
        ' f_iひも番号
        ' 
        f_iひも番号.DataPropertyName = "f_iひも番号"
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight
        f_iひも番号.DefaultCellStyle = DataGridViewCellStyle1
        f_iひも番号.HeaderText = "ひも番号"
        f_iひも番号.MinimumWidth = 6
        f_iひも番号.Name = "f_iひも番号"
        f_iひも番号.ReadOnly = True
        f_iひも番号.SortMode = DataGridViewColumnSortMode.NotSortable
        f_iひも番号.Width = 59
        ' 
        ' f_s付属品ひも名
        ' 
        f_s付属品ひも名.DataPropertyName = "f_s付属品ひも名"
        f_s付属品ひも名.HeaderText = "付属品ひも名"
        f_s付属品ひも名.MinimumWidth = 6
        f_s付属品ひも名.Name = "f_s付属品ひも名"
        f_s付属品ひも名.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s付属品ひも名.Width = 119
        ' 
        ' f_b巻きひも区分
        ' 
        f_b巻きひも区分.DataPropertyName = "f_b巻きひも区分"
        f_b巻きひも区分.HeaderText = "巻きひも"
        f_b巻きひも区分.MinimumWidth = 6
        f_b巻きひも区分.Name = "f_b巻きひも区分"
        f_b巻きひも区分.Width = 68
        ' 
        ' f_i本幅初期値
        ' 
        f_i本幅初期値.DataPropertyName = "f_i本幅初期値"
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        f_i本幅初期値.DefaultCellStyle = DataGridViewCellStyle2
        f_i本幅初期値.HeaderText = "本幅初期値"
        f_i本幅初期値.MinimumWidth = 6
        f_i本幅初期値.Name = "f_i本幅初期値"
        f_i本幅初期値.SortMode = DataGridViewColumnSortMode.NotSortable
        f_i本幅初期値.ToolTipText = "指定しない場合はゼロ"
        f_i本幅初期値.Width = 69
        ' 
        ' f_iひも数
        ' 
        f_iひも数.DataPropertyName = "f_iひも数"
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight
        f_iひも数.DefaultCellStyle = DataGridViewCellStyle3
        f_iひも数.HeaderText = "ひも数"
        f_iひも数.MinimumWidth = 6
        f_iひも数.Name = "f_iひも数"
        f_iひも数.SortMode = DataGridViewColumnSortMode.NotSortable
        f_iひも数.Width = 63
        ' 
        ' f_d長さ比率対ひも1
        ' 
        f_d長さ比率対ひも1.DataPropertyName = "f_d長さ比率対ひも1"
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d長さ比率対ひも1.DefaultCellStyle = DataGridViewCellStyle4
        f_d長さ比率対ひも1.HeaderText = "長さ比率対ひも1"
        f_d長さ比率対ひも1.MinimumWidth = 6
        f_d長さ比率対ひも1.Name = "f_d長さ比率対ひも1"
        f_d長さ比率対ひも1.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d長さ比率対ひも1.ToolTipText = "ひも番号2以降であれば、ひも番号1の長さにこの値を乗算して長さに加える"
        f_d長さ比率対ひも1.Width = 66
        ' 
        ' f_d長さ加減対ひも1
        ' 
        f_d長さ加減対ひも1.DataPropertyName = "f_d長さ加減対ひも1"
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d長さ加減対ひも1.DefaultCellStyle = DataGridViewCellStyle5
        f_d長さ加減対ひも1.HeaderText = "長さ加減対ひも1"
        f_d長さ加減対ひも1.MinimumWidth = 6
        f_d長さ加減対ひも1.Name = "f_d長さ加減対ひも1"
        f_d長さ加減対ひも1.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d長さ加減対ひも1.ToolTipText = "ひも番号2以降であれば、ひも番号1の長さにこの値を加える"
        f_d長さ加減対ひも1.Width = 66
        ' 
        ' f_dひも長比率対長さ
        ' 
        f_dひも長比率対長さ.DataPropertyName = "f_dひも長比率対長さ"
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長比率対長さ.DefaultCellStyle = DataGridViewCellStyle6
        f_dひも長比率対長さ.HeaderText = "ひも長比率対長さ"
        f_dひも長比率対長さ.MinimumWidth = 6
        f_dひも長比率対長さ.Name = "f_dひも長比率対長さ"
        f_dひも長比率対長さ.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも長比率対長さ.ToolTipText = "巻きひもでない時、ひも長にこの値を乗算してひも長に加える"
        f_dひも長比率対長さ.Width = 83
        ' 
        ' f_dひも長加算
        ' 
        f_dひも長加算.DataPropertyName = "f_dひも長加算"
        DataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長加算.DefaultCellStyle = DataGridViewCellStyle7
        f_dひも長加算.HeaderText = "ひも長加算"
        f_dひも長加算.MinimumWidth = 6
        f_dひも長加算.Name = "f_dひも長加算"
        f_dひも長加算.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも長加算.ToolTipText = "巻きひもでない時、ひも長にこの値を加える"
        f_dひも長加算.Width = 69
        ' 
        ' f_d巻きの厚み
        ' 
        f_d巻きの厚み.DataPropertyName = "f_d巻きの厚み"
        DataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d巻きの厚み.DefaultCellStyle = DataGridViewCellStyle8
        f_d巻きの厚み.HeaderText = "巻きの厚み"
        f_d巻きの厚み.MinimumWidth = 6
        f_d巻きの厚み.Name = "f_d巻きの厚み"
        f_d巻きの厚み.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d巻きの厚み.ToolTipText = "巻きひもの時、1回巻くのにひもの厚みとして計算する値"
        f_d巻きの厚み.Width = 65
        ' 
        ' f_d巻き回数比率
        ' 
        f_d巻き回数比率.DataPropertyName = "f_d巻き回数比率"
        DataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d巻き回数比率.DefaultCellStyle = DataGridViewCellStyle9
        f_d巻き回数比率.HeaderText = "巻き回数比率"
        f_d巻き回数比率.MinimumWidth = 6
        f_d巻き回数比率.Name = "f_d巻き回数比率"
        f_d巻き回数比率.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d巻き回数比率.ToolTipText = "巻きひもの時、長さをひも幅で除算した回数にこの値を乗算する"
        f_d巻き回数比率.Width = 70
        ' 
        ' f_dひも長加算初期値
        ' 
        f_dひも長加算初期値.DataPropertyName = "f_dひも長加算初期値"
        DataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長加算初期値.DefaultCellStyle = DataGridViewCellStyle10
        f_dひも長加算初期値.HeaderText = "ひも長加算初期値"
        f_dひも長加算初期値.MinimumWidth = 6
        f_dひも長加算初期値.Name = "f_dひも長加算初期値"
        f_dひも長加算初期値.ToolTipText = "ひも長に手動で加える値の初期値"
        f_dひも長加算初期値.Width = 125
        ' 
        ' f_bCraftBandMesh
        ' 
        f_bCraftBandMesh.DataPropertyName = "f_bCraftBandMesh"
        f_bCraftBandMesh.HeaderText = "Mesh"
        f_bCraftBandMesh.MinimumWidth = 6
        f_bCraftBandMesh.Name = "f_bCraftBandMesh"
        f_bCraftBandMesh.ToolTipText = "CraftBandMeshで使用する"
        f_bCraftBandMesh.Width = 125
        ' 
        ' f_bCraftBandSquare45
        ' 
        f_bCraftBandSquare45.DataPropertyName = "f_bCraftBandSquare45"
        f_bCraftBandSquare45.HeaderText = "Square45"
        f_bCraftBandSquare45.MinimumWidth = 6
        f_bCraftBandSquare45.Name = "f_bCraftBandSquare45"
        f_bCraftBandSquare45.ToolTipText = "CraftBandSquare45で使用する"
        f_bCraftBandSquare45.Width = 125
        ' 
        ' f_bCraftBandKnot
        ' 
        f_bCraftBandKnot.DataPropertyName = "f_bCraftBandKnot"
        f_bCraftBandKnot.HeaderText = "Knot"
        f_bCraftBandKnot.MinimumWidth = 6
        f_bCraftBandKnot.Name = "f_bCraftBandKnot"
        f_bCraftBandKnot.ToolTipText = "CraftBandKnotで使用する"
        f_bCraftBandKnot.Width = 125
        ' 
        ' f_bCraftBandSquare
        ' 
        f_bCraftBandSquare.DataPropertyName = "f_bCraftBandSquare"
        f_bCraftBandSquare.HeaderText = "Square"
        f_bCraftBandSquare.MinimumWidth = 6
        f_bCraftBandSquare.Name = "f_bCraftBandSquare"
        f_bCraftBandSquare.ToolTipText = "CraftBandSquareで使用する"
        f_bCraftBandSquare.Width = 125
        ' 
        ' f_bCraftBandHexagon
        ' 
        f_bCraftBandHexagon.DataPropertyName = "f_bCraftBandHexagon"
        f_bCraftBandHexagon.HeaderText = "Hexagon"
        f_bCraftBandHexagon.MinimumWidth = 6
        f_bCraftBandHexagon.Name = "f_bCraftBandHexagon"
        f_bCraftBandHexagon.ToolTipText = "CraftBandHexagonで使用する"
        f_bCraftBandHexagon.Width = 125
        ' 
        ' f_s備考
        ' 
        f_s備考.DataPropertyName = "f_s備考"
        f_s備考.HeaderText = "備考"
        f_s備考.MinimumWidth = 6
        f_s備考.Name = "f_s備考"
        f_s備考.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s備考.Width = 200
        ' 
        ' frmOptions
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(755, 266)
        Controls.Add(btn削除)
        Controls.Add(lbl付属品名)
        Controls.Add(txt付属品名)
        Controls.Add(btnひも追加)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        Controls.Add(lbl設定時の寸法単位)
        Controls.Add(lbl単位)
        Controls.Add(dgvData)
        MinimumSize = New System.Drawing.Size(773, 313)
        Name = "frmOptions"
        StartPosition = FormStartPosition.CenterParent
        Text = "付属品"
        CType(dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSource付属品, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents Fs縁かがり名DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents lbl設定時の寸法単位 As Label
    Friend WithEvents lbl単位 As Label
    Friend WithEvents btnキャンセル As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents Fs縁の始末名DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Fs縁の始末ひも名DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents 縁ひも幅比率DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents 編みひも幅比率DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents 全長比率DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents 編み目数比率DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents BindingSource付属品 As BindingSource
    Friend WithEvents btnひも追加 As Button
    Friend WithEvents lbl付属品名 As Label
    Friend WithEvents txt付属品名 As TextBox
    Friend WithEvents btn削除 As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents f_s付属品名 As DataGridViewTextBoxColumn
    Friend WithEvents f_iひも番号 As DataGridViewTextBoxColumn
    Friend WithEvents f_s付属品ひも名 As DataGridViewTextBoxColumn
    Friend WithEvents f_b巻きひも区分 As DataGridViewCheckBoxColumn
    Friend WithEvents f_i本幅初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_iひも数 As DataGridViewTextBoxColumn
    Friend WithEvents f_d長さ比率対ひも1 As DataGridViewTextBoxColumn
    Friend WithEvents f_d長さ加減対ひも1 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長比率対長さ As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算 As DataGridViewTextBoxColumn
    Friend WithEvents f_d巻きの厚み As DataGridViewTextBoxColumn
    Friend WithEvents f_d巻き回数比率 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_bCraftBandMesh As DataGridViewCheckBoxColumn
    Friend WithEvents f_bCraftBandSquare45 As DataGridViewCheckBoxColumn
    Friend WithEvents f_bCraftBandKnot As DataGridViewCheckBoxColumn
    Friend WithEvents f_bCraftBandSquare As DataGridViewCheckBoxColumn
    Friend WithEvents f_bCraftBandHexagon As DataGridViewCheckBoxColumn
    Friend WithEvents f_s備考 As DataGridViewTextBoxColumn
End Class
