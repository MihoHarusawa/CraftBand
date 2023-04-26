Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPattern
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
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvData = New ctrDataGridView()
        Me.f_s編みかた名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_iひも番号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s編みひも名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_b縁専用区分 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_b底使用区分 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_i周あたり段数 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_iひも数 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_i本幅初期値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_b周連続区分 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_d高さ比率対ひも幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d垂直ひも長比率対ひも幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長比率対周長 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長加算1目あたり = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長加算1周あたり = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも1幅係数1目あたり = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長加算ひもあたり = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dひも長加算初期値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d厚さ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_bCraftBandMesh = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_bCraftBandSquare45 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_bCraftBandKnot = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_bCraftBandSquare = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.f_s備考 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSource編みかた = New System.Windows.Forms.BindingSource(Me.components)
        Me.lbl設定時の寸法単位 = New System.Windows.Forms.Label()
        Me.lbl単位 = New System.Windows.Forms.Label()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnひも追加 = New System.Windows.Forms.Button()
        Me.txt編みかた名 = New System.Windows.Forms.TextBox()
        Me.lbl編みかた名 = New System.Windows.Forms.Label()
        Me.btn削除 = New System.Windows.Forms.Button()
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource編みかた, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.f_s編みかた名, Me.f_iひも番号, Me.f_s編みひも名, Me.f_b縁専用区分, Me.f_b底使用区分, Me.f_i周あたり段数, Me.f_iひも数, Me.f_i本幅初期値, Me.f_b周連続区分, Me.f_d高さ比率対ひも幅, Me.f_d垂直ひも長比率対ひも幅, Me.f_dひも長比率対周長, Me.f_dひも長加算1目あたり, Me.f_dひも長加算1周あたり, Me.f_dひも1幅係数1目あたり, Me.f_dひも長加算ひもあたり, Me.f_dひも長加算初期値, Me.f_d厚さ, Me.f_bCraftBandMesh, Me.f_bCraftBandSquare45, Me.f_bCraftBandKnot, Me.f_bCraftBandSquare, Me.f_s備考})
        Me.dgvData.DataSource = Me.BindingSource編みかた
        Me.dgvData.Location = New System.Drawing.Point(12, 32)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.RowHeadersWidth = 51
        Me.dgvData.RowTemplate.Height = 29
        Me.dgvData.Size = New System.Drawing.Size(746, 156)
        Me.dgvData.TabIndex = 2
        '
        'f_s編みかた名
        '
        Me.f_s編みかた名.DataPropertyName = "f_s編みかた名"
        Me.f_s編みかた名.HeaderText = "編みかた名"
        Me.f_s編みかた名.MinimumWidth = 6
        Me.f_s編みかた名.Name = "f_s編みかた名"
        Me.f_s編みかた名.ReadOnly = True
        Me.f_s編みかた名.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s編みかた名.Width = 181
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
        Me.f_iひも番号.Width = 77
        '
        'f_s編みひも名
        '
        Me.f_s編みひも名.DataPropertyName = "f_s編みひも名"
        Me.f_s編みひも名.HeaderText = "編みひも名"
        Me.f_s編みひも名.MinimumWidth = 6
        Me.f_s編みひも名.Name = "f_s編みひも名"
        Me.f_s編みひも名.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s編みひも名.Width = 135
        '
        'f_b縁専用区分
        '
        Me.f_b縁専用区分.DataPropertyName = "f_b縁専用区分"
        Me.f_b縁専用区分.HeaderText = "縁専用"
        Me.f_b縁専用区分.MinimumWidth = 6
        Me.f_b縁専用区分.Name = "f_b縁専用区分"
        Me.f_b縁専用区分.ToolTipText = "縁専用のあみ方の場合、チェックを入れる"
        Me.f_b縁専用区分.Width = 74
        '
        'f_b底使用区分
        '
        Me.f_b底使用区分.DataPropertyName = "f_b底使用区分"
        Me.f_b底使用区分.HeaderText = "底使用"
        Me.f_b底使用区分.MinimumWidth = 6
        Me.f_b底使用区分.Name = "f_b底使用区分"
        Me.f_b底使用区分.ToolTipText = "楕円底で使用可能な編みかたであればチェックを入れる"
        Me.f_b底使用区分.Width = 74
        '
        'f_i周あたり段数
        '
        Me.f_i周あたり段数.DataPropertyName = "f_i周あたり段数"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_i周あたり段数.DefaultCellStyle = DataGridViewCellStyle2
        Me.f_i周あたり段数.HeaderText = "周あたり段数"
        Me.f_i周あたり段数.MinimumWidth = 6
        Me.f_i周あたり段数.Name = "f_i周あたり段数"
        Me.f_i周あたり段数.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_i周あたり段数.Width = 60
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
        Me.f_iひも数.Width = 55
        '
        'f_i本幅初期値
        '
        Me.f_i本幅初期値.DataPropertyName = "f_i本幅初期値"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.f_i本幅初期値.DefaultCellStyle = DataGridViewCellStyle4
        Me.f_i本幅初期値.HeaderText = "本幅初期値"
        Me.f_i本幅初期値.MinimumWidth = 6
        Me.f_i本幅初期値.Name = "f_i本幅初期値"
        Me.f_i本幅初期値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_i本幅初期値.ToolTipText = "0であれば基本の幅、以外はその値"
        Me.f_i本幅初期値.Width = 62
        '
        'f_b周連続区分
        '
        Me.f_b周連続区分.DataPropertyName = "f_b周連続区分"
        Me.f_b周連続区分.HeaderText = "周連続"
        Me.f_b周連続区分.MinimumWidth = 6
        Me.f_b周連続区分.Name = "f_b周連続区分"
        Me.f_b周連続区分.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.f_b周連続区分.ToolTipText = "同じひもで続けて編む場合にチェックを入れる"
        Me.f_b周連続区分.Width = 57
        '
        'f_d高さ比率対ひも幅
        '
        Me.f_d高さ比率対ひも幅.DataPropertyName = "f_d高さ比率対ひも幅"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d高さ比率対ひも幅.DefaultCellStyle = DataGridViewCellStyle5
        Me.f_d高さ比率対ひも幅.HeaderText = "高さ比率対ひも幅"
        Me.f_d高さ比率対ひも幅.MinimumWidth = 6
        Me.f_d高さ比率対ひも幅.Name = "f_d高さ比率対ひも幅"
        Me.f_d高さ比率対ひも幅.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_d高さ比率対ひも幅.ToolTipText = "何本取りで指定したひも幅に対する高さの比率"
        Me.f_d高さ比率対ひも幅.Width = 75
        '
        'f_d垂直ひも長比率対ひも幅
        '
        Me.f_d垂直ひも長比率対ひも幅.DataPropertyName = "f_d垂直ひも長比率対ひも幅"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d垂直ひも長比率対ひも幅.DefaultCellStyle = DataGridViewCellStyle6
        Me.f_d垂直ひも長比率対ひも幅.HeaderText = "垂直ひも長比率対ひも幅"
        Me.f_d垂直ひも長比率対ひも幅.MinimumWidth = 6
        Me.f_d垂直ひも長比率対ひも幅.Name = "f_d垂直ひも長比率対ひも幅"
        Me.f_d垂直ひも長比率対ひも幅.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_d垂直ひも長比率対ひも幅.ToolTipText = "何本取りで指定したひも幅に対する必要な垂直ひもの比率"
        Me.f_d垂直ひも長比率対ひも幅.Width = 75
        '
        'f_dひも長比率対周長
        '
        Me.f_dひも長比率対周長.DataPropertyName = "f_dひも長比率対周長"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長比率対周長.DefaultCellStyle = DataGridViewCellStyle7
        Me.f_dひも長比率対周長.HeaderText = "ひも長比率対周長"
        Me.f_dひも長比率対周長.MinimumWidth = 6
        Me.f_dひも長比率対周長.Name = "f_dひも長比率対周長"
        Me.f_dひも長比率対周長.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_dひも長比率対周長.ToolTipText = "周長にこの値を乗算してひも長に加える"
        Me.f_dひも長比率対周長.Width = 75
        '
        'f_dひも長加算1目あたり
        '
        Me.f_dひも長加算1目あたり.DataPropertyName = "f_dひも長加算1目あたり"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長加算1目あたり.DefaultCellStyle = DataGridViewCellStyle8
        Me.f_dひも長加算1目あたり.HeaderText = "ひも長加算1目あたり"
        Me.f_dひも長加算1目あたり.MinimumWidth = 6
        Me.f_dひも長加算1目あたり.Name = "f_dひも長加算1目あたり"
        Me.f_dひも長加算1目あたり.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_dひも長加算1目あたり.ToolTipText = "垂直ひも数にこの値を乗算してひも長に加える"
        Me.f_dひも長加算1目あたり.Width = 67
        '
        'f_dひも長加算1周あたり
        '
        Me.f_dひも長加算1周あたり.DataPropertyName = "f_dひも長加算1周あたり"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長加算1周あたり.DefaultCellStyle = DataGridViewCellStyle9
        Me.f_dひも長加算1周あたり.HeaderText = "ひも長加算1周あたり"
        Me.f_dひも長加算1周あたり.MinimumWidth = 6
        Me.f_dひも長加算1周あたり.Name = "f_dひも長加算1周あたり"
        Me.f_dひも長加算1周あたり.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_dひも長加算1周あたり.ToolTipText = "1周につき1回、この値をひも長に加える"
        Me.f_dひも長加算1周あたり.Width = 67
        '
        'f_dひも1幅係数1目あたり
        '
        Me.f_dひも1幅係数1目あたり.DataPropertyName = "f_dひも1幅係数1目あたり"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも1幅係数1目あたり.DefaultCellStyle = DataGridViewCellStyle10
        Me.f_dひも1幅係数1目あたり.HeaderText = "ひも1幅係数1目あたり"
        Me.f_dひも1幅係数1目あたり.MinimumWidth = 6
        Me.f_dひも1幅係数1目あたり.Name = "f_dひも1幅係数1目あたり"
        Me.f_dひも1幅係数1目あたり.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_dひも1幅係数1目あたり.ToolTipText = "ひも1の幅にこの値と垂直ひも数を乗算してひも長に加える"
        Me.f_dひも1幅係数1目あたり.Width = 125
        '
        'f_dひも長加算ひもあたり
        '
        Me.f_dひも長加算ひもあたり.DataPropertyName = "f_dひも長加算ひもあたり"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長加算ひもあたり.DefaultCellStyle = DataGridViewCellStyle11
        Me.f_dひも長加算ひもあたり.HeaderText = "ひも長加算ひもあたり"
        Me.f_dひも長加算ひもあたり.MinimumWidth = 6
        Me.f_dひも長加算ひもあたり.Name = "f_dひも長加算ひもあたり"
        Me.f_dひも長加算ひもあたり.ToolTipText = "ひもにつき1回、この値をひも長に加える"
        Me.f_dひも長加算ひもあたり.Width = 125
        '
        'f_dひも長加算初期値
        '
        Me.f_dひも長加算初期値.DataPropertyName = "f_dひも長加算初期値"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dひも長加算初期値.DefaultCellStyle = DataGridViewCellStyle12
        Me.f_dひも長加算初期値.HeaderText = "ひも長加算初期値"
        Me.f_dひも長加算初期値.MinimumWidth = 6
        Me.f_dひも長加算初期値.Name = "f_dひも長加算初期値"
        Me.f_dひも長加算初期値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_dひも長加算初期値.ToolTipText = "ひも長に手動で加える値の初期値"
        Me.f_dひも長加算初期値.Width = 67
        '
        'f_d厚さ
        '
        Me.f_d厚さ.DataPropertyName = "f_d厚さ"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d厚さ.DefaultCellStyle = DataGridViewCellStyle13
        Me.f_d厚さ.HeaderText = "厚さ"
        Me.f_d厚さ.MinimumWidth = 6
        Me.f_d厚さ.Name = "f_d厚さ"
        Me.f_d厚さ.Width = 125
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
        Me.f_bCraftBandSquare.Visible = False
        Me.f_bCraftBandSquare.Width = 125
        '
        'f_s備考
        '
        Me.f_s備考.DataPropertyName = "f_s備考"
        Me.f_s備考.HeaderText = "備考"
        Me.f_s備考.MinimumWidth = 6
        Me.f_s備考.Name = "f_s備考"
        Me.f_s備考.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.f_s備考.Width = 125
        '
        'BindingSource編みかた
        '
        Me.BindingSource編みかた.DataMember = "tbl編みかた"
        Me.BindingSource編みかた.DataSource = GetType(CraftBand.Tables.dstMasterTables)
        '
        'lbl設定時の寸法単位
        '
        Me.lbl設定時の寸法単位.AutoSize = True
        Me.lbl設定時の寸法単位.Location = New System.Drawing.Point(60, -1)
        Me.lbl設定時の寸法単位.Name = "lbl設定時の寸法単位"
        Me.lbl設定時の寸法単位.Size = New System.Drawing.Size(0, 20)
        Me.lbl設定時の寸法単位.TabIndex = 1
        '
        'lbl単位
        '
        Me.lbl単位.AutoSize = True
        Me.lbl単位.Location = New System.Drawing.Point(12, -1)
        Me.lbl単位.Name = "lbl単位"
        Me.lbl単位.Size = New System.Drawing.Size(42, 20)
        Me.lbl単位.TabIndex = 0
        Me.lbl単位.Text = "単位:"
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(647, 202)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 8
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(528, 202)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "OK(&O)"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnひも追加
        '
        Me.btnひも追加.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnひも追加.Location = New System.Drawing.Point(329, 202)
        Me.btnひも追加.Name = "btnひも追加"
        Me.btnひも追加.Size = New System.Drawing.Size(111, 46)
        Me.btnひも追加.TabIndex = 6
        Me.btnひも追加.Text = "ひも追加(&B)"
        Me.btnひも追加.UseVisualStyleBackColor = True
        '
        'txt編みかた名
        '
        Me.txt編みかた名.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt編みかた名.Location = New System.Drawing.Point(212, 212)
        Me.txt編みかた名.Name = "txt編みかた名"
        Me.txt編みかた名.Size = New System.Drawing.Size(101, 27)
        Me.txt編みかた名.TabIndex = 5
        '
        'lbl編みかた名
        '
        Me.lbl編みかた名.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl編みかた名.AutoSize = True
        Me.lbl編みかた名.Location = New System.Drawing.Point(130, 215)
        Me.lbl編みかた名.Name = "lbl編みかた名"
        Me.lbl編みかた名.Size = New System.Drawing.Size(76, 20)
        Me.lbl編みかた名.TabIndex = 4
        Me.lbl編みかた名.Text = "編みかた名"
        '
        'btn削除
        '
        Me.btn削除.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn削除.Location = New System.Drawing.Point(12, 202)
        Me.btn削除.Name = "btn削除"
        Me.btn削除.Size = New System.Drawing.Size(111, 46)
        Me.btn削除.TabIndex = 3
        Me.btn削除.Text = "削除(&D)"
        Me.btn削除.UseVisualStyleBackColor = True
        '
        'frmPattern
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(770, 256)
        Me.Controls.Add(Me.btn削除)
        Me.Controls.Add(Me.lbl編みかた名)
        Me.Controls.Add(Me.txt編みかた名)
        Me.Controls.Add(Me.btnひも追加)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lbl設定時の寸法単位)
        Me.Controls.Add(Me.lbl単位)
        Me.Controls.Add(Me.dgvData)
        Me.MinimumSize = New System.Drawing.Size(788, 303)
        Me.Name = "frmPattern"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "編みかた"
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource編みかた, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents lbl設定時の寸法単位 As Label
    Friend WithEvents lbl単位 As Label
    Friend WithEvents btnキャンセル As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents BindingSource編みかた As BindingSource
    Friend WithEvents btnひも追加 As Button
    Friend WithEvents txt編みかた名 As TextBox
    Friend WithEvents lbl編みかた名 As Label
    Friend WithEvents btn削除 As Button
    Friend WithEvents f_s編みかた名 As DataGridViewTextBoxColumn
    Friend WithEvents f_iひも番号 As DataGridViewTextBoxColumn
    Friend WithEvents f_s編みひも名 As DataGridViewTextBoxColumn
    Friend WithEvents f_b縁専用区分 As DataGridViewCheckBoxColumn
    Friend WithEvents f_b底使用区分 As DataGridViewCheckBoxColumn
    Friend WithEvents f_i周あたり段数 As DataGridViewTextBoxColumn
    Friend WithEvents f_iひも数 As DataGridViewTextBoxColumn
    Friend WithEvents f_i本幅初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_b周連続区分 As DataGridViewCheckBoxColumn
    Friend WithEvents f_d高さ比率対ひも幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_d垂直ひも長比率対ひも幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長比率対周長 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算1目あたり As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算1周あたり As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも1幅係数1目あたり As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算ひもあたり As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_d厚さ As DataGridViewTextBoxColumn
    Friend WithEvents f_bCraftBandMesh As DataGridViewCheckBoxColumn
    Friend WithEvents f_bCraftBandSquare45 As DataGridViewCheckBoxColumn
    Friend WithEvents f_bCraftBandKnot As DataGridViewCheckBoxColumn
    Friend WithEvents f_bCraftBandSquare As DataGridViewCheckBoxColumn
    Friend WithEvents f_s備考 As DataGridViewTextBoxColumn
End Class
