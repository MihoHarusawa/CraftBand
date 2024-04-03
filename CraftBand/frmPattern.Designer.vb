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
        Dim DataGridViewCellStyle11 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As DataGridViewCellStyle = New DataGridViewCellStyle()
        dgvData = New ctrDataGridView()
        BindingSource編みかた = New BindingSource(components)
        lbl設定時の寸法単位 = New Label()
        lbl単位 = New Label()
        btnキャンセル = New Button()
        btnOK = New Button()
        btnひも追加 = New Button()
        txt編みかた名 = New TextBox()
        lbl編みかた名 = New Label()
        btn削除 = New Button()
        ToolTip1 = New ToolTip(components)
        f_s編みかた名 = New DataGridViewTextBoxColumn()
        f_iひも番号 = New DataGridViewTextBoxColumn()
        f_s編みひも名 = New DataGridViewTextBoxColumn()
        f_b縁専用区分 = New DataGridViewCheckBoxColumn()
        f_b底使用区分 = New DataGridViewCheckBoxColumn()
        f_i周あたり段数 = New DataGridViewTextBoxColumn()
        f_iひも数 = New DataGridViewTextBoxColumn()
        f_i本幅初期値 = New DataGridViewTextBoxColumn()
        f_b周連続区分 = New DataGridViewCheckBoxColumn()
        f_d高さ比率対ひも幅 = New DataGridViewTextBoxColumn()
        f_d垂直ひも長比率対ひも幅 = New DataGridViewTextBoxColumn()
        f_dひも長比率対周長 = New DataGridViewTextBoxColumn()
        f_dひも長加算1目あたり = New DataGridViewTextBoxColumn()
        f_dひも長加算1周あたり = New DataGridViewTextBoxColumn()
        f_dひも1幅係数1目あたり = New DataGridViewTextBoxColumn()
        f_dひも長加算ひもあたり = New DataGridViewTextBoxColumn()
        f_dひも長加算初期値 = New DataGridViewTextBoxColumn()
        f_d厚さ = New DataGridViewTextBoxColumn()
        f_bCraftBandMesh = New DataGridViewCheckBoxColumn()
        f_bCraftBandSquare45 = New DataGridViewCheckBoxColumn()
        f_bCraftBandKnot = New DataGridViewCheckBoxColumn()
        f_bCraftBandSquare = New DataGridViewCheckBoxColumn()
        f_bCraftBandHexagon = New DataGridViewCheckBoxColumn()
        f_s備考 = New DataGridViewTextBoxColumn()
        CType(dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSource編みかた, System.ComponentModel.ISupportInitialize).BeginInit()
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
        dgvData.Columns.AddRange(New DataGridViewColumn() {f_s編みかた名, f_iひも番号, f_s編みひも名, f_b縁専用区分, f_b底使用区分, f_i周あたり段数, f_iひも数, f_i本幅初期値, f_b周連続区分, f_d高さ比率対ひも幅, f_d垂直ひも長比率対ひも幅, f_dひも長比率対周長, f_dひも長加算1目あたり, f_dひも長加算1周あたり, f_dひも1幅係数1目あたり, f_dひも長加算ひもあたり, f_dひも長加算初期値, f_d厚さ, f_bCraftBandMesh, f_bCraftBandSquare45, f_bCraftBandKnot, f_bCraftBandSquare, f_bCraftBandHexagon, f_s備考})
        dgvData.DataSource = BindingSource編みかた
        dgvData.Location = New System.Drawing.Point(12, 32)
        dgvData.Name = "dgvData"
        dgvData.RowHeadersWidth = 51
        dgvData.RowTemplate.Height = 29
        dgvData.Size = New System.Drawing.Size(746, 156)
        dgvData.TabIndex = 2
        ' 
        ' BindingSource編みかた
        ' 
        BindingSource編みかた.DataMember = "tbl編みかた"
        BindingSource編みかた.DataSource = GetType(Tables.dstMasterTables)
        ' 
        ' lbl設定時の寸法単位
        ' 
        lbl設定時の寸法単位.AutoSize = True
        lbl設定時の寸法単位.Location = New System.Drawing.Point(60, -1)
        lbl設定時の寸法単位.Name = "lbl設定時の寸法単位"
        lbl設定時の寸法単位.Size = New System.Drawing.Size(0, 20)
        lbl設定時の寸法単位.TabIndex = 1
        ' 
        ' lbl単位
        ' 
        lbl単位.AutoSize = True
        lbl単位.Location = New System.Drawing.Point(12, -1)
        lbl単位.Name = "lbl単位"
        lbl単位.Size = New System.Drawing.Size(42, 20)
        lbl単位.TabIndex = 0
        lbl単位.Text = "単位:"
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnキャンセル.DialogResult = DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(647, 202)
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
        btnOK.Location = New System.Drawing.Point(528, 202)
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
        btnひも追加.Location = New System.Drawing.Point(329, 202)
        btnひも追加.Name = "btnひも追加"
        btnひも追加.Size = New System.Drawing.Size(111, 46)
        btnひも追加.TabIndex = 6
        btnひも追加.Text = "ひも追加(&B)"
        ToolTip1.SetToolTip(btnひも追加, "新たな編みかた名を追加、" & vbCrLf & "もしくは既存の編みかた名に編みひもを追加")
        btnひも追加.UseVisualStyleBackColor = True
        ' 
        ' txt編みかた名
        ' 
        txt編みかた名.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txt編みかた名.Location = New System.Drawing.Point(212, 212)
        txt編みかた名.Name = "txt編みかた名"
        txt編みかた名.Size = New System.Drawing.Size(101, 27)
        txt編みかた名.TabIndex = 5
        ToolTip1.SetToolTip(txt編みかた名, "編みかたの名前")
        ' 
        ' lbl編みかた名
        ' 
        lbl編みかた名.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lbl編みかた名.AutoSize = True
        lbl編みかた名.Location = New System.Drawing.Point(130, 215)
        lbl編みかた名.Name = "lbl編みかた名"
        lbl編みかた名.Size = New System.Drawing.Size(76, 20)
        lbl編みかた名.TabIndex = 4
        lbl編みかた名.Text = "編みかた名"
        ' 
        ' btn削除
        ' 
        btn削除.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btn削除.Location = New System.Drawing.Point(12, 202)
        btn削除.Name = "btn削除"
        btn削除.Size = New System.Drawing.Size(111, 46)
        btn削除.TabIndex = 3
        btn削除.Text = "削除(&D)"
        ToolTip1.SetToolTip(btn削除, "選択している編みかたを削除します")
        btn削除.UseVisualStyleBackColor = True
        ' 
        ' f_s編みかた名
        ' 
        f_s編みかた名.DataPropertyName = "f_s編みかた名"
        f_s編みかた名.HeaderText = "編みかた名"
        f_s編みかた名.MinimumWidth = 6
        f_s編みかた名.Name = "f_s編みかた名"
        f_s編みかた名.ReadOnly = True
        f_s編みかた名.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s編みかた名.Width = 181
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
        f_iひも番号.Width = 77
        ' 
        ' f_s編みひも名
        ' 
        f_s編みひも名.DataPropertyName = "f_s編みひも名"
        f_s編みひも名.HeaderText = "編みひも名"
        f_s編みひも名.MinimumWidth = 6
        f_s編みひも名.Name = "f_s編みひも名"
        f_s編みひも名.SortMode = DataGridViewColumnSortMode.NotSortable
        f_s編みひも名.Width = 135
        ' 
        ' f_b縁専用区分
        ' 
        f_b縁専用区分.DataPropertyName = "f_b縁専用区分"
        f_b縁専用区分.HeaderText = "縁専用"
        f_b縁専用区分.MinimumWidth = 6
        f_b縁専用区分.Name = "f_b縁専用区分"
        f_b縁専用区分.ToolTipText = "縁専用のあみ方の場合、チェックを入れる"
        f_b縁専用区分.Width = 74
        ' 
        ' f_b底使用区分
        ' 
        f_b底使用区分.DataPropertyName = "f_b底使用区分"
        f_b底使用区分.HeaderText = "底使用"
        f_b底使用区分.MinimumWidth = 6
        f_b底使用区分.Name = "f_b底使用区分"
        f_b底使用区分.ToolTipText = "楕円底で使用可能な編みかたであればチェックを入れる"
        f_b底使用区分.Width = 74
        ' 
        ' f_i周あたり段数
        ' 
        f_i周あたり段数.DataPropertyName = "f_i周あたり段数"
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight
        f_i周あたり段数.DefaultCellStyle = DataGridViewCellStyle2
        f_i周あたり段数.HeaderText = "周あたり段数"
        f_i周あたり段数.MinimumWidth = 6
        f_i周あたり段数.Name = "f_i周あたり段数"
        f_i周あたり段数.SortMode = DataGridViewColumnSortMode.NotSortable
        f_i周あたり段数.Width = 60
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
        f_iひも数.Width = 55
        ' 
        ' f_i本幅初期値
        ' 
        f_i本幅初期値.DataPropertyName = "f_i本幅初期値"
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter
        f_i本幅初期値.DefaultCellStyle = DataGridViewCellStyle4
        f_i本幅初期値.HeaderText = "本幅初期値"
        f_i本幅初期値.MinimumWidth = 6
        f_i本幅初期値.Name = "f_i本幅初期値"
        f_i本幅初期値.SortMode = DataGridViewColumnSortMode.NotSortable
        f_i本幅初期値.ToolTipText = "0であれば基本の幅、以外はその値"
        f_i本幅初期値.Width = 62
        ' 
        ' f_b周連続区分
        ' 
        f_b周連続区分.DataPropertyName = "f_b周連続区分"
        f_b周連続区分.HeaderText = "周連続"
        f_b周連続区分.MinimumWidth = 6
        f_b周連続区分.Name = "f_b周連続区分"
        f_b周連続区分.Resizable = DataGridViewTriState.True
        f_b周連続区分.ToolTipText = "同じひもで続けて編む場合にチェックを入れる"
        f_b周連続区分.Width = 57
        ' 
        ' f_d高さ比率対ひも幅
        ' 
        f_d高さ比率対ひも幅.DataPropertyName = "f_d高さ比率対ひも幅"
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d高さ比率対ひも幅.DefaultCellStyle = DataGridViewCellStyle5
        f_d高さ比率対ひも幅.HeaderText = "高さ比率対ひも幅"
        f_d高さ比率対ひも幅.MinimumWidth = 6
        f_d高さ比率対ひも幅.Name = "f_d高さ比率対ひも幅"
        f_d高さ比率対ひも幅.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d高さ比率対ひも幅.ToolTipText = "何本取りで指定したひも幅に対する高さの比率"
        f_d高さ比率対ひも幅.Width = 75
        ' 
        ' f_d垂直ひも長比率対ひも幅
        ' 
        f_d垂直ひも長比率対ひも幅.DataPropertyName = "f_d垂直ひも長比率対ひも幅"
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d垂直ひも長比率対ひも幅.DefaultCellStyle = DataGridViewCellStyle6
        f_d垂直ひも長比率対ひも幅.HeaderText = "垂直ひも長比率対ひも幅"
        f_d垂直ひも長比率対ひも幅.MinimumWidth = 6
        f_d垂直ひも長比率対ひも幅.Name = "f_d垂直ひも長比率対ひも幅"
        f_d垂直ひも長比率対ひも幅.SortMode = DataGridViewColumnSortMode.NotSortable
        f_d垂直ひも長比率対ひも幅.ToolTipText = "何本取りで指定したひも幅に対する必要な垂直ひもの比率"
        f_d垂直ひも長比率対ひも幅.Width = 75
        ' 
        ' f_dひも長比率対周長
        ' 
        f_dひも長比率対周長.DataPropertyName = "f_dひも長比率対周長"
        DataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長比率対周長.DefaultCellStyle = DataGridViewCellStyle7
        f_dひも長比率対周長.HeaderText = "ひも長比率対周長"
        f_dひも長比率対周長.MinimumWidth = 6
        f_dひも長比率対周長.Name = "f_dひも長比率対周長"
        f_dひも長比率対周長.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも長比率対周長.ToolTipText = "周長にこの値を乗算してひも長に加える"
        f_dひも長比率対周長.Width = 75
        ' 
        ' f_dひも長加算1目あたり
        ' 
        f_dひも長加算1目あたり.DataPropertyName = "f_dひも長加算1目あたり"
        DataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長加算1目あたり.DefaultCellStyle = DataGridViewCellStyle8
        f_dひも長加算1目あたり.HeaderText = "ひも長加算1目あたり"
        f_dひも長加算1目あたり.MinimumWidth = 6
        f_dひも長加算1目あたり.Name = "f_dひも長加算1目あたり"
        f_dひも長加算1目あたり.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも長加算1目あたり.ToolTipText = "垂直ひも数にこの値を乗算してひも長に加える"
        f_dひも長加算1目あたり.Width = 67
        ' 
        ' f_dひも長加算1周あたり
        ' 
        f_dひも長加算1周あたり.DataPropertyName = "f_dひも長加算1周あたり"
        DataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長加算1周あたり.DefaultCellStyle = DataGridViewCellStyle9
        f_dひも長加算1周あたり.HeaderText = "ひも長加算1周あたり"
        f_dひも長加算1周あたり.MinimumWidth = 6
        f_dひも長加算1周あたり.Name = "f_dひも長加算1周あたり"
        f_dひも長加算1周あたり.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも長加算1周あたり.ToolTipText = "1周につき1回、この値をひも長に加える"
        f_dひも長加算1周あたり.Width = 67
        ' 
        ' f_dひも1幅係数1目あたり
        ' 
        f_dひも1幅係数1目あたり.DataPropertyName = "f_dひも1幅係数1目あたり"
        DataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも1幅係数1目あたり.DefaultCellStyle = DataGridViewCellStyle10
        f_dひも1幅係数1目あたり.HeaderText = "ひも1幅係数1目あたり"
        f_dひも1幅係数1目あたり.MinimumWidth = 6
        f_dひも1幅係数1目あたり.Name = "f_dひも1幅係数1目あたり"
        f_dひも1幅係数1目あたり.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも1幅係数1目あたり.ToolTipText = "ひも1の幅にこの値と垂直ひも数を乗算してひも長に加える"
        f_dひも1幅係数1目あたり.Width = 125
        ' 
        ' f_dひも長加算ひもあたり
        ' 
        f_dひも長加算ひもあたり.DataPropertyName = "f_dひも長加算ひもあたり"
        DataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長加算ひもあたり.DefaultCellStyle = DataGridViewCellStyle11
        f_dひも長加算ひもあたり.HeaderText = "ひも長加算ひもあたり"
        f_dひも長加算ひもあたり.MinimumWidth = 6
        f_dひも長加算ひもあたり.Name = "f_dひも長加算ひもあたり"
        f_dひも長加算ひもあたり.ToolTipText = "ひもにつき1回、この値をひも長に加える"
        f_dひも長加算ひもあたり.Width = 125
        ' 
        ' f_dひも長加算初期値
        ' 
        f_dひも長加算初期値.DataPropertyName = "f_dひも長加算初期値"
        DataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長加算初期値.DefaultCellStyle = DataGridViewCellStyle12
        f_dひも長加算初期値.HeaderText = "ひも長加算初期値"
        f_dひも長加算初期値.MinimumWidth = 6
        f_dひも長加算初期値.Name = "f_dひも長加算初期値"
        f_dひも長加算初期値.SortMode = DataGridViewColumnSortMode.NotSortable
        f_dひも長加算初期値.ToolTipText = "ひも長に手動で加える値の初期値"
        f_dひも長加算初期値.Width = 67
        ' 
        ' f_d厚さ
        ' 
        f_d厚さ.DataPropertyName = "f_d厚さ"
        DataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d厚さ.DefaultCellStyle = DataGridViewCellStyle13
        f_d厚さ.HeaderText = "厚さ"
        f_d厚さ.MinimumWidth = 6
        f_d厚さ.Name = "f_d厚さ"
        f_d厚さ.Width = 125
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
        f_s備考.Width = 125
        ' 
        ' frmPattern
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(770, 256)
        Controls.Add(btn削除)
        Controls.Add(lbl編みかた名)
        Controls.Add(txt編みかた名)
        Controls.Add(btnひも追加)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        Controls.Add(lbl設定時の寸法単位)
        Controls.Add(lbl単位)
        Controls.Add(dgvData)
        MinimumSize = New System.Drawing.Size(788, 303)
        Name = "frmPattern"
        StartPosition = FormStartPosition.CenterParent
        Text = "編みかた"
        CType(dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSource編みかた, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

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
    Friend WithEvents ToolTip1 As ToolTip
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
    Friend WithEvents f_bCraftBandHexagon As DataGridViewCheckBoxColumn
    Friend WithEvents f_s備考 As DataGridViewTextBoxColumn
End Class
