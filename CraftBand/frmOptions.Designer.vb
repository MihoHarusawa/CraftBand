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
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvData = New CraftBand.ctrDataGridView()
        Me.BindingSource付属品 = New System.Windows.Forms.BindingSource(Me.components)
        Me.lbl設定時の寸法単位 = New System.Windows.Forms.Label()
        Me.lbl単位 = New System.Windows.Forms.Label()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnひも追加 = New System.Windows.Forms.Button()
        Me.lbl付属品名 = New System.Windows.Forms.Label()
        Me.txt付属品名 = New System.Windows.Forms.TextBox()
        Me.btn削除 = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.f_s付属品名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_iひも番号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s付属品ひも名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_b巻きひも区分 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_i本幅初期値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_iひも数 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d長さ比率対ひも1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d長さ加減対ひも1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長比率対長さ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長加算 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d巻きの厚み = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d巻き回数比率 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長加算初期値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_bCraftBandMesh = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_bCraftBandSquare45 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_bCraftBandKnot = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_bCraftBandSquare = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_bCraftBandHexagon = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_s備考 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource付属品, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvData
        '
        Me.dgvData.AllowUserToAddRows = False
        Me.dgvData.AllowUserToDeleteRows = False
        Me.dgvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvData.AutoGenerateColumns = False
        Me.dgvData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.f_s付属品名, Me.f_iひも番号, Me.f_s付属品ひも名, Me.f_b巻きひも区分, Me.f_i本幅初期値, Me.f_iひも数, Me.f_d長さ比率対ひも1, Me.f_d長さ加減対ひも1, Me.f_dひも長比率対長さ, Me.f_dひも長加算, Me.f_d巻きの厚み, Me.f_d巻き回数比率, Me.f_dひも長加算初期値, Me.f_bCraftBandMesh, Me.f_bCraftBandSquare45, Me.f_bCraftBandKnot, Me.f_bCraftBandSquare, Me.f_bCraftBandHexagon, Me.f_s備考})
        Me.dgvData.DataSource = Me.BindingSource付属品
        Me.dgvData.Location = New System.Drawing.Point(12, 32)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.RowHeadersWidth = 51
        Me.dgvData.RowTemplate.Height = 29
        Me.dgvData.Size = New System.Drawing.Size(731, 166)
        Me.dgvData.TabIndex = 2
        '
        'BindingSource付属品
        '
        Me.BindingSource付属品.DataMember = "tbl付属品"
        Me.BindingSource付属品.DataSource = GetType(CraftBand.Tables.dstMasterTables)
        '
        'lbl設定時の寸法単位
        '
        Me.lbl設定時の寸法単位.AutoSize = True
        Me.lbl設定時の寸法単位.Location = New System.Drawing.Point(62, -2)
        Me.lbl設定時の寸法単位.Name = "lbl設定時の寸法単位"
        Me.lbl設定時の寸法単位.Size = New System.Drawing.Size(0, 20)
        Me.lbl設定時の寸法単位.TabIndex = 1
        '
        'lbl単位
        '
        Me.lbl単位.AutoSize = True
        Me.lbl単位.Location = New System.Drawing.Point(14, -2)
        Me.lbl単位.Name = "lbl単位"
        Me.lbl単位.Size = New System.Drawing.Size(42, 20)
        Me.lbl単位.TabIndex = 0
        Me.lbl単位.Text = "単位:"
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(632, 211)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 8
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.ToolTip1.SetToolTip(Me.btnキャンセル, "変更を保存せずに終了します")
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(513, 211)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "OK(&O)"
        Me.ToolTip1.SetToolTip(Me.btnOK, "変更を保存して終了します")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnひも追加
        '
        Me.btnひも追加.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnひも追加.Location = New System.Drawing.Point(333, 211)
        Me.btnひも追加.Name = "btnひも追加"
        Me.btnひも追加.Size = New System.Drawing.Size(111, 46)
        Me.btnひも追加.TabIndex = 6
        Me.btnひも追加.Text = "ひも追加(&A)"
        Me.ToolTip1.SetToolTip(Me.btnひも追加, "新たな付属品名を追加、" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "もしくは既存の付属品名に付属品ひもを追加")
        Me.btnひも追加.UseVisualStyleBackColor = True
        '
        'lbl付属品名
        '
        Me.lbl付属品名.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl付属品名.AutoSize = True
        Me.lbl付属品名.Location = New System.Drawing.Point(134, 225)
        Me.lbl付属品名.Name = "lbl付属品名"
        Me.lbl付属品名.Size = New System.Drawing.Size(69, 20)
        Me.lbl付属品名.TabIndex = 4
        Me.lbl付属品名.Text = "付属品名"
        '
        'txt付属品名
        '
        Me.txt付属品名.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt付属品名.Location = New System.Drawing.Point(216, 222)
        Me.txt付属品名.Name = "txt付属品名"
        Me.txt付属品名.Size = New System.Drawing.Size(101, 27)
        Me.txt付属品名.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.txt付属品名, "付属品の名前")
        '
        'btn削除
        '
        Me.btn削除.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn削除.Location = New System.Drawing.Point(14, 211)
        Me.btn削除.Name = "btn削除"
        Me.btn削除.Size = New System.Drawing.Size(111, 46)
        Me.btn削除.TabIndex = 3
        Me.btn削除.Text = "削除(&D)"
        Me.ToolTip1.SetToolTip(Me.btn削除, "選択している付属品を削除します")
        Me.btn削除.UseVisualStyleBackColor = True
        '
        'f_s付属品名
        '
        Me.f_s付属品名.DataPropertyName = "f_s付属品名"
        Me.f_s付属品名.HeaderText = "付属品名"
        Me.f_s付属品名.MinimumWidth = 6
        Me.f_s付属品名.Name = "f_s付属品名"
        Me.f_s付属品名.ReadOnly = True
        Me.f_s付属品名.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s付属品名.Width = 155
        '
        'f_iひも番号
        '
        Me.f_iひも番号.DataPropertyName = "f_iひも番号"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_iひも番号.DefaultCellStyle = DataGridViewCellStyle1
        Me.f_iひも番号.HeaderText = "ひも番号"
        Me.f_iひも番号.MinimumWidth = 6
        Me.f_iひも番号.Name = "f_iひも番号"
        Me.f_iひも番号.ReadOnly = True
        Me.f_iひも番号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_iひも番号.Width = 59
        '
        'f_s付属品ひも名
        '
        Me.f_s付属品ひも名.DataPropertyName = "f_s付属品ひも名"
        Me.f_s付属品ひも名.HeaderText = "付属品ひも名"
        Me.f_s付属品ひも名.MinimumWidth = 6
        Me.f_s付属品ひも名.Name = "f_s付属品ひも名"
        Me.f_s付属品ひも名.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s付属品ひも名.Width = 119
        '
        'f_b巻きひも区分
        '
        Me.f_b巻きひも区分.DataPropertyName = "f_b巻きひも区分"
        Me.f_b巻きひも区分.HeaderText = "巻きひも"
        Me.f_b巻きひも区分.MinimumWidth = 6
        Me.f_b巻きひも区分.Name = "f_b巻きひも区分"
        Me.f_b巻きひも区分.Width = 68
        '
        'f_i本幅初期値
        '
        Me.f_i本幅初期値.DataPropertyName = "f_i本幅初期値"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.f_i本幅初期値.DefaultCellStyle = DataGridViewCellStyle2
        Me.f_i本幅初期値.HeaderText = "本幅初期値"
        Me.f_i本幅初期値.MinimumWidth = 6
        Me.f_i本幅初期値.Name = "f_i本幅初期値"
        Me.f_i本幅初期値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_i本幅初期値.ToolTipText = "指定しない場合はゼロ"
        Me.f_i本幅初期値.Width = 69
        '
        'f_iひも数
        '
        Me.f_iひも数.DataPropertyName = "f_iひも数"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_iひも数.DefaultCellStyle = DataGridViewCellStyle3
        Me.f_iひも数.HeaderText = "ひも数"
        Me.f_iひも数.MinimumWidth = 6
        Me.f_iひも数.Name = "f_iひも数"
        Me.f_iひも数.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_iひも数.Width = 63
        '
        'f_d長さ比率対ひも1
        '
        Me.f_d長さ比率対ひも1.DataPropertyName = "f_d長さ比率対ひも1"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d長さ比率対ひも1.DefaultCellStyle = DataGridViewCellStyle4
        Me.f_d長さ比率対ひも1.HeaderText = "長さ比率対ひも1"
        Me.f_d長さ比率対ひも1.MinimumWidth = 6
        Me.f_d長さ比率対ひも1.Name = "f_d長さ比率対ひも1"
        Me.f_d長さ比率対ひも1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_d長さ比率対ひも1.ToolTipText = "ひも番号2以降であれば、ひも番号1の長さにこの値を乗算して長さに加える"
        Me.f_d長さ比率対ひも1.Width = 66
        '
        'f_d長さ加減対ひも1
        '
        Me.f_d長さ加減対ひも1.DataPropertyName = "f_d長さ加減対ひも1"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d長さ加減対ひも1.DefaultCellStyle = DataGridViewCellStyle5
        Me.f_d長さ加減対ひも1.HeaderText = "長さ加減対ひも1"
        Me.f_d長さ加減対ひも1.MinimumWidth = 6
        Me.f_d長さ加減対ひも1.Name = "f_d長さ加減対ひも1"
        Me.f_d長さ加減対ひも1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_d長さ加減対ひも1.ToolTipText = "ひも番号2以降であれば、ひも番号1の長さにこの値を加える"
        Me.f_d長さ加減対ひも1.Width = 66
        '
        'f_dひも長比率対長さ
        '
        Me.f_dひも長比率対長さ.DataPropertyName = "f_dひも長比率対長さ"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長比率対長さ.DefaultCellStyle = DataGridViewCellStyle6
        Me.f_dひも長比率対長さ.HeaderText = "ひも長比率対長さ"
        Me.f_dひも長比率対長さ.MinimumWidth = 6
        Me.f_dひも長比率対長さ.Name = "f_dひも長比率対長さ"
        Me.f_dひも長比率対長さ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_dひも長比率対長さ.ToolTipText = "巻きひもでない時、ひも長にこの値を乗算してひも長に加える"
        Me.f_dひも長比率対長さ.Width = 83
        '
        'f_dひも長加算
        '
        Me.f_dひも長加算.DataPropertyName = "f_dひも長加算"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長加算.DefaultCellStyle = DataGridViewCellStyle7
        Me.f_dひも長加算.HeaderText = "ひも長加算"
        Me.f_dひも長加算.MinimumWidth = 6
        Me.f_dひも長加算.Name = "f_dひも長加算"
        Me.f_dひも長加算.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_dひも長加算.ToolTipText = "巻きひもでない時、ひも長にこの値を加える"
        Me.f_dひも長加算.Width = 69
        '
        'f_d巻きの厚み
        '
        Me.f_d巻きの厚み.DataPropertyName = "f_d巻きの厚み"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d巻きの厚み.DefaultCellStyle = DataGridViewCellStyle8
        Me.f_d巻きの厚み.HeaderText = "巻きの厚み"
        Me.f_d巻きの厚み.MinimumWidth = 6
        Me.f_d巻きの厚み.Name = "f_d巻きの厚み"
        Me.f_d巻きの厚み.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_d巻きの厚み.ToolTipText = "巻きひもの時、1回巻くのにひもの厚みとして計算する値"
        Me.f_d巻きの厚み.Width = 65
        '
        'f_d巻き回数比率
        '
        Me.f_d巻き回数比率.DataPropertyName = "f_d巻き回数比率"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d巻き回数比率.DefaultCellStyle = DataGridViewCellStyle9
        Me.f_d巻き回数比率.HeaderText = "巻き回数比率"
        Me.f_d巻き回数比率.MinimumWidth = 6
        Me.f_d巻き回数比率.Name = "f_d巻き回数比率"
        Me.f_d巻き回数比率.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_d巻き回数比率.ToolTipText = "巻きひもの時、長さをひも幅で除算した回数にこの値を乗算する"
        Me.f_d巻き回数比率.Width = 70
        '
        'f_dひも長加算初期値
        '
        Me.f_dひも長加算初期値.DataPropertyName = "f_dひも長加算初期値"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長加算初期値.DefaultCellStyle = DataGridViewCellStyle10
        Me.f_dひも長加算初期値.HeaderText = "ひも長加算初期値"
        Me.f_dひも長加算初期値.MinimumWidth = 6
        Me.f_dひも長加算初期値.Name = "f_dひも長加算初期値"
        Me.f_dひも長加算初期値.ToolTipText = "ひも長に手動で加える値の初期値"
        Me.f_dひも長加算初期値.Width = 125
        '
        'f_bCraftBandMesh
        '
        Me.f_bCraftBandMesh.DataPropertyName = "f_bCraftBandMesh"
        Me.f_bCraftBandMesh.HeaderText = "Mesh"
        Me.f_bCraftBandMesh.MinimumWidth = 6
        Me.f_bCraftBandMesh.Name = "f_bCraftBandMesh"
        Me.f_bCraftBandMesh.ToolTipText = "CraftBandMeshで使用する"
        Me.f_bCraftBandMesh.Width = 125
        '
        'f_bCraftBandSquare45
        '
        Me.f_bCraftBandSquare45.DataPropertyName = "f_bCraftBandSquare45"
        Me.f_bCraftBandSquare45.HeaderText = "Square45"
        Me.f_bCraftBandSquare45.MinimumWidth = 6
        Me.f_bCraftBandSquare45.Name = "f_bCraftBandSquare45"
        Me.f_bCraftBandSquare45.ToolTipText = "CraftBandSquare45で使用する"
        Me.f_bCraftBandSquare45.Width = 125
        '
        'f_bCraftBandKnot
        '
        Me.f_bCraftBandKnot.DataPropertyName = "f_bCraftBandKnot"
        Me.f_bCraftBandKnot.HeaderText = "Knot"
        Me.f_bCraftBandKnot.MinimumWidth = 6
        Me.f_bCraftBandKnot.Name = "f_bCraftBandKnot"
        Me.f_bCraftBandKnot.ToolTipText = "CraftBandKnotで使用する"
        Me.f_bCraftBandKnot.Width = 125
        '
        'f_bCraftBandSquare
        '
        Me.f_bCraftBandSquare.DataPropertyName = "f_bCraftBandSquare"
        Me.f_bCraftBandSquare.HeaderText = "Square"
        Me.f_bCraftBandSquare.MinimumWidth = 6
        Me.f_bCraftBandSquare.Name = "f_bCraftBandSquare"
        Me.f_bCraftBandSquare.ToolTipText = "CraftBandSquareで使用する"
        Me.f_bCraftBandSquare.Width = 125
        '
        'f_bCraftBandHexagon
        '
        Me.f_bCraftBandHexagon.DataPropertyName = "f_bCraftBandHexagon"
        Me.f_bCraftBandHexagon.HeaderText = "Hexagon"
        Me.f_bCraftBandHexagon.MinimumWidth = 6
        Me.f_bCraftBandHexagon.Name = "f_bCraftBandHexagon"
        Me.f_bCraftBandHexagon.ToolTipText = "CraftBandHexagonで使用する"
        Me.f_bCraftBandHexagon.Width = 125
        '
        'f_s備考
        '
        Me.f_s備考.DataPropertyName = "f_s備考"
        Me.f_s備考.HeaderText = "備考"
        Me.f_s備考.MinimumWidth = 6
        Me.f_s備考.Name = "f_s備考"
        Me.f_s備考.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s備考.Width = 200
        '
        'frmOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(755, 266)
        Me.Controls.Add(Me.btn削除)
        Me.Controls.Add(Me.lbl付属品名)
        Me.Controls.Add(Me.txt付属品名)
        Me.Controls.Add(Me.btnひも追加)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lbl設定時の寸法単位)
        Me.Controls.Add(Me.lbl単位)
        Me.Controls.Add(Me.dgvData)
        Me.MinimumSize = New System.Drawing.Size(773, 313)
        Me.Name = "frmOptions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "付属品"
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource付属品, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
