Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBandType
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
        Dim DataGridViewCellStyle11 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle18 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle19 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle20 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle21 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle22 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle23 As DataGridViewCellStyle = New DataGridViewCellStyle()
        dgvData = New ctrDataGridView()
        BindingSourceバンドの種類 = New BindingSource(components)
        btnキャンセル = New Button()
        btnOK = New Button()
        lbl設定時の寸法単位 = New Label()
        lbl単位 = New Label()
        txtバンドの種類名 = New TextBox()
        btn複製 = New Button()
        btn追加 = New Button()
        btn長さと重さ = New Button()
        lblバンドの種類名 = New Label()
        ToolTip1 = New ToolTip(components)
        f_sバンドの種類名 = New DataGridViewTextBoxColumn()
        f_i本幅 = New DataGridViewTextBoxColumn()
        f_dバンド幅 = New DataGridViewTextBoxColumn()
        f_d底の厚さ = New DataGridViewTextBoxColumn()
        f_s長さと重さ = New DataGridViewTextBoxColumn()
        ColumnBotton = New DataGridViewButtonColumn()
        f_d短い横ひも長のばらつき = New DataGridViewTextBoxColumn()
        f_d縦ひも間の最小間隔 = New DataGridViewTextBoxColumn()
        f_d垂直ひも加算初期値 = New DataGridViewTextBoxColumn()
        f_d立ち上げ時の四角底周の増分 = New DataGridViewTextBoxColumn()
        f_d差しひもの径 = New DataGridViewTextBoxColumn()
        f_d差しひも長加算初期値 = New DataGridViewTextBoxColumn()
        f_d楕円底円弧の半径加算 = New DataGridViewTextBoxColumn()
        f_d楕円底周の加算 = New DataGridViewTextBoxColumn()
        f_d立ち上げ時の楕円底周の増分 = New DataGridViewTextBoxColumn()
        f_dひも間のすき間初期値 = New DataGridViewTextBoxColumn()
        f_dひも長係数初期値 = New DataGridViewTextBoxColumn()
        f_dひも長加算初期値 = New DataGridViewTextBoxColumn()
        f_dコマ寸法係数a = New DataGridViewTextBoxColumn()
        f_dコマ寸法係数b = New DataGridViewTextBoxColumn()
        f_dコマ要尺係数a = New DataGridViewTextBoxColumn()
        f_dコマ要尺係数b = New DataGridViewTextBoxColumn()
        f_d四つ畳みひも長加算初期値 = New DataGridViewTextBoxColumn()
        f_d目と数える端の目 = New DataGridViewTextBoxColumn()
        f_s色リスト = New DataGridViewTextBoxColumn()
        f_s製品情報 = New DataGridViewTextBoxColumn()
        f_s備考 = New DataGridViewTextBoxColumn()
        ColumnBotton本幅の幅 = New DataGridViewButtonColumn()
        f_s本幅の幅リスト = New DataGridViewTextBoxColumn()
        CType(dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(BindingSourceバンドの種類, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvData
        ' 
        dgvData.AllowUserToAddRows = False
        DataGridViewCellStyle1.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        dgvData.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        dgvData.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvData.AutoGenerateColumns = False
        dgvData.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvData.Columns.AddRange(New DataGridViewColumn() {f_sバンドの種類名, f_i本幅, f_dバンド幅, f_d底の厚さ, f_s長さと重さ, ColumnBotton, f_d短い横ひも長のばらつき, f_d縦ひも間の最小間隔, f_d垂直ひも加算初期値, f_d立ち上げ時の四角底周の増分, f_d差しひもの径, f_d差しひも長加算初期値, f_d楕円底円弧の半径加算, f_d楕円底周の加算, f_d立ち上げ時の楕円底周の増分, f_dひも間のすき間初期値, f_dひも長係数初期値, f_dひも長加算初期値, f_dコマ寸法係数a, f_dコマ寸法係数b, f_dコマ要尺係数a, f_dコマ要尺係数b, f_d四つ畳みひも長加算初期値, f_d目と数える端の目, f_s色リスト, f_s製品情報, f_s備考, ColumnBotton本幅の幅, f_s本幅の幅リスト})
        dgvData.DataSource = BindingSourceバンドの種類
        dgvData.Location = New System.Drawing.Point(12, 32)
        dgvData.Name = "dgvData"
        dgvData.RowHeadersWidth = 51
        dgvData.RowTemplate.Height = 29
        dgvData.Size = New System.Drawing.Size(850, 169)
        dgvData.TabIndex = 2
        ' 
        ' BindingSourceバンドの種類
        ' 
        BindingSourceバンドの種類.DataMember = "tblバンドの種類"
        BindingSourceバンドの種類.DataSource = GetType(Tables.dstMasterTables)
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnキャンセル.DialogResult = DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(751, 224)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 46)
        btnキャンセル.TabIndex = 9
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "変更を保存せずに終了します")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(632, 224)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 46)
        btnOK.TabIndex = 8
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "変更を保存して終了します")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' lbl設定時の寸法単位
        ' 
        lbl設定時の寸法単位.AutoSize = True
        lbl設定時の寸法単位.Location = New System.Drawing.Point(60, -2)
        lbl設定時の寸法単位.Name = "lbl設定時の寸法単位"
        lbl設定時の寸法単位.Size = New System.Drawing.Size(0, 20)
        lbl設定時の寸法単位.TabIndex = 1
        ' 
        ' lbl単位
        ' 
        lbl単位.AutoSize = True
        lbl単位.Location = New System.Drawing.Point(12, -2)
        lbl単位.Name = "lbl単位"
        lbl単位.Size = New System.Drawing.Size(42, 20)
        lbl単位.TabIndex = 0
        lbl単位.Text = "単位:"
        ' 
        ' txtバンドの種類名
        ' 
        txtバンドの種類名.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtバンドの種類名.Location = New System.Drawing.Point(143, 243)
        txtバンドの種類名.Name = "txtバンドの種類名"
        txtバンドの種類名.Size = New System.Drawing.Size(209, 27)
        txtバンドの種類名.TabIndex = 5
        ToolTip1.SetToolTip(txtバンドの種類名, "新しいバンドの種類名を入力してください")
        ' 
        ' btn複製
        ' 
        btn複製.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btn複製.Location = New System.Drawing.Point(360, 224)
        btn複製.Name = "btn複製"
        btn複製.Size = New System.Drawing.Size(111, 46)
        btn複製.TabIndex = 6
        btn複製.Text = "複製(&D)"
        ToolTip1.SetToolTip(btn複製, "選択中のバンドを複製して追加します")
        btn複製.UseVisualStyleBackColor = True
        ' 
        ' btn追加
        ' 
        btn追加.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btn追加.Location = New System.Drawing.Point(477, 224)
        btn追加.Name = "btn追加"
        btn追加.Size = New System.Drawing.Size(111, 46)
        btn追加.TabIndex = 7
        btn追加.Text = "追加(&A)"
        ToolTip1.SetToolTip(btn追加, "新たなバンドを初期値で追加します")
        btn追加.UseVisualStyleBackColor = True
        ' 
        ' btn長さと重さ
        ' 
        btn長さと重さ.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btn長さと重さ.Location = New System.Drawing.Point(12, 223)
        btn長さと重さ.Name = "btn長さと重さ"
        btn長さと重さ.Size = New System.Drawing.Size(111, 46)
        btn長さと重さ.TabIndex = 3
        btn長さと重さ.Text = "長さと重さ(&W)"
        ToolTip1.SetToolTip(btn長さと重さ, "選択中のバンドの長さと重さを計算します")
        btn長さと重さ.UseVisualStyleBackColor = True
        ' 
        ' lblバンドの種類名
        ' 
        lblバンドの種類名.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblバンドの種類名.AutoSize = True
        lblバンドの種類名.Location = New System.Drawing.Point(145, 215)
        lblバンドの種類名.Name = "lblバンドの種類名"
        lblバンドの種類名.Size = New System.Drawing.Size(100, 20)
        lblバンドの種類名.TabIndex = 4
        lblバンドの種類名.Text = "バンドの種類名"
        ' 
        ' f_sバンドの種類名
        ' 
        f_sバンドの種類名.DataPropertyName = "f_sバンドの種類名"
        f_sバンドの種類名.HeaderText = "バンドの種類名"
        f_sバンドの種類名.MinimumWidth = 6
        f_sバンドの種類名.Name = "f_sバンドの種類名"
        f_sバンドの種類名.ToolTipText = "識別可能な名前"
        f_sバンドの種類名.Width = 160
        ' 
        ' f_i本幅
        ' 
        f_i本幅.DataPropertyName = "f_i本幅"
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        f_i本幅.DefaultCellStyle = DataGridViewCellStyle2
        f_i本幅.HeaderText = "本幅"
        f_i本幅.MinimumWidth = 6
        f_i本幅.Name = "f_i本幅"
        f_i本幅.ToolTipText = "何本幅のバンドか"
        f_i本幅.Width = 80
        ' 
        ' f_dバンド幅
        ' 
        f_dバンド幅.DataPropertyName = "f_dバンド幅"
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dバンド幅.DefaultCellStyle = DataGridViewCellStyle3
        f_dバンド幅.HeaderText = "バンド幅"
        f_dバンド幅.MinimumWidth = 6
        f_dバンド幅.Name = "f_dバンド幅"
        f_dバンド幅.ToolTipText = "設定した単位でバンドの幅をセット"
        f_dバンド幅.Width = 83
        ' 
        ' f_d底の厚さ
        ' 
        f_d底の厚さ.DataPropertyName = "f_d底の厚さ"
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d底の厚さ.DefaultCellStyle = DataGridViewCellStyle4
        f_d底の厚さ.HeaderText = "底の厚さ"
        f_d底の厚さ.MinimumWidth = 6
        f_d底の厚さ.Name = "f_d底の厚さ"
        f_d底の厚さ.ToolTipText = "内側・外側の基本的なサイズ差"
        f_d底の厚さ.Width = 113
        ' 
        ' f_s長さと重さ
        ' 
        f_s長さと重さ.DataPropertyName = "f_s長さと重さ"
        f_s長さと重さ.HeaderText = "長さと重さ"
        f_s長さと重さ.MinimumWidth = 6
        f_s長さと重さ.Name = "f_s長さと重さ"
        f_s長さと重さ.ToolTipText = "<数値> <長さの単位> = <数値> <重さの単位> "
        f_s長さと重さ.Width = 125
        ' 
        ' ColumnBotton
        ' 
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.BackColor = Drawing.Color.FromArgb(CByte(224), CByte(224), CByte(224))
        ColumnBotton.DefaultCellStyle = DataGridViewCellStyle5
        ColumnBotton.FlatStyle = FlatStyle.Popup
        ColumnBotton.HeaderText = "色選択"
        ColumnBotton.MinimumWidth = 6
        ColumnBotton.Name = "ColumnBotton"
        ColumnBotton.Text = "色選択"
        ColumnBotton.ToolTipText = "描画色から選択する場合クリック"
        ColumnBotton.UseColumnTextForButtonValue = True
        ColumnBotton.Width = 60
        ' 
        ' f_d短い横ひも長のばらつき
        ' 
        f_d短い横ひも長のばらつき.DataPropertyName = "f_d短い横ひも長のばらつき"
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d短い横ひも長のばらつき.DefaultCellStyle = DataGridViewCellStyle6
        f_d短い横ひも長のばらつき.HeaderText = "短い横ひも長のばらつき"
        f_d短い横ひも長のばらつき.MinimumWidth = 6
        f_d短い横ひも長のばらつき.Name = "f_d短い横ひも長のばらつき"
        f_d短い横ひも長のばらつき.ToolTipText = "計算値よりこの設定値分、ひも長を短く出力します"
        f_d短い横ひも長のばらつき.Width = 125
        ' 
        ' f_d縦ひも間の最小間隔
        ' 
        f_d縦ひも間の最小間隔.DataPropertyName = "f_d縦ひも間の最小間隔"
        DataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d縦ひも間の最小間隔.DefaultCellStyle = DataGridViewCellStyle7
        f_d縦ひも間の最小間隔.HeaderText = "縦ひも間の最小間隔"
        f_d縦ひも間の最小間隔.MinimumWidth = 6
        f_d縦ひも間の最小間隔.Name = "f_d縦ひも間の最小間隔"
        f_d縦ひも間の最小間隔.ToolTipText = "縦ひも間の隙間がこの値より小さければ警告します"
        f_d縦ひも間の最小間隔.Width = 125
        ' 
        ' f_d垂直ひも加算初期値
        ' 
        f_d垂直ひも加算初期値.DataPropertyName = "f_d垂直ひも加算初期値"
        DataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d垂直ひも加算初期値.DefaultCellStyle = DataGridViewCellStyle8
        f_d垂直ひも加算初期値.HeaderText = "垂直ひも加算初期値"
        f_d垂直ひも加算初期値.MinimumWidth = 6
        f_d垂直ひも加算初期値.Name = "f_d垂直ひも加算初期値"
        f_d垂直ひも加算初期値.ToolTipText = "垂直ひも加算値の初期値"
        f_d垂直ひも加算初期値.Width = 125
        ' 
        ' f_d立ち上げ時の四角底周の増分
        ' 
        f_d立ち上げ時の四角底周の増分.DataPropertyName = "f_d立ち上げ時の四角底周の増分"
        DataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d立ち上げ時の四角底周の増分.DefaultCellStyle = DataGridViewCellStyle9
        f_d立ち上げ時の四角底周の増分.HeaderText = "立ち上げ時の四角底周の増分"
        f_d立ち上げ時の四角底周の増分.MinimumWidth = 6
        f_d立ち上げ時の四角底周の増分.Name = "f_d立ち上げ時の四角底周の増分"
        f_d立ち上げ時の四角底周の増分.ToolTipText = "四角底の時、底の周長にこの値を加えて側面の周長を得る"
        f_d立ち上げ時の四角底周の増分.Width = 153
        ' 
        ' f_d差しひもの径
        ' 
        f_d差しひもの径.DataPropertyName = "f_d差しひもの径"
        DataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d差しひもの径.DefaultCellStyle = DataGridViewCellStyle10
        f_d差しひもの径.HeaderText = "差しひもの径"
        f_d差しひもの径.MinimumWidth = 6
        f_d差しひもの径.Name = "f_d差しひもの径"
        f_d差しひもの径.ToolTipText = "楕円底設定時に、差しひも分の径として計算する値"
        f_d差しひもの径.Width = 125
        ' 
        ' f_d差しひも長加算初期値
        ' 
        f_d差しひも長加算初期値.DataPropertyName = "f_d差しひも長加算初期値"
        DataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d差しひも長加算初期値.DefaultCellStyle = DataGridViewCellStyle11
        f_d差しひも長加算初期値.HeaderText = "差しひも長加算初期値"
        f_d差しひも長加算初期値.MinimumWidth = 6
        f_d差しひも長加算初期値.Name = "f_d差しひも長加算初期値"
        f_d差しひも長加算初期値.ToolTipText = "差しひも長加算値の初期値"
        f_d差しひも長加算初期値.Width = 125
        ' 
        ' f_d楕円底円弧の半径加算
        ' 
        f_d楕円底円弧の半径加算.DataPropertyName = "f_d楕円底円弧の半径加算"
        DataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d楕円底円弧の半径加算.DefaultCellStyle = DataGridViewCellStyle12
        f_d楕円底円弧の半径加算.HeaderText = "楕円底円弧の半径加算"
        f_d楕円底円弧の半径加算.MinimumWidth = 6
        f_d楕円底円弧の半径加算.Name = "f_d楕円底円弧の半径加算"
        f_d楕円底円弧の半径加算.ToolTipText = "円周計算時の半径の加減値。小さくするにはマイナスを設定"
        f_d楕円底円弧の半径加算.Width = 125
        ' 
        ' f_d楕円底周の加算
        ' 
        f_d楕円底周の加算.DataPropertyName = "f_d楕円底周の加算"
        DataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d楕円底周の加算.DefaultCellStyle = DataGridViewCellStyle13
        f_d楕円底周の加算.HeaderText = "楕円底周の加算"
        f_d楕円底周の加算.MinimumWidth = 6
        f_d楕円底周の加算.Name = "f_d楕円底周の加算"
        f_d楕円底周の加算.ToolTipText = "楕円部の周の加減値。斜めカット分のマイナスを設定"
        f_d楕円底周の加算.Width = 125
        ' 
        ' f_d立ち上げ時の楕円底周の増分
        ' 
        f_d立ち上げ時の楕円底周の増分.DataPropertyName = "f_d立ち上げ時の楕円底周の増分"
        DataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d立ち上げ時の楕円底周の増分.DefaultCellStyle = DataGridViewCellStyle14
        f_d立ち上げ時の楕円底周の増分.HeaderText = "立ち上げ時の楕円底周の増分"
        f_d立ち上げ時の楕円底周の増分.MinimumWidth = 6
        f_d立ち上げ時の楕円底周の増分.Name = "f_d立ち上げ時の楕円底周の増分"
        f_d立ち上げ時の楕円底周の増分.ToolTipText = "楕円底の時、底の周長にこの値を加えて側面の周長を得る"
        f_d立ち上げ時の楕円底周の増分.Width = 125
        ' 
        ' f_dひも間のすき間初期値
        ' 
        f_dひも間のすき間初期値.DataPropertyName = "f_dひも間のすき間初期値"
        DataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも間のすき間初期値.DefaultCellStyle = DataGridViewCellStyle15
        f_dひも間のすき間初期値.HeaderText = "ひも間のすき間初期値"
        f_dひも間のすき間初期値.MinimumWidth = 6
        f_dひも間のすき間初期値.Name = "f_dひも間のすき間初期値"
        f_dひも間のすき間初期値.ToolTipText = "組み編みのすき間の初期値"
        f_dひも間のすき間初期値.Width = 125
        ' 
        ' f_dひも長係数初期値
        ' 
        f_dひも長係数初期値.DataPropertyName = "f_dひも長係数初期値"
        DataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長係数初期値.DefaultCellStyle = DataGridViewCellStyle16
        f_dひも長係数初期値.HeaderText = "ひも長係数初期値"
        f_dひも長係数初期値.MinimumWidth = 6
        f_dひも長係数初期値.Name = "f_dひも長係数初期値"
        f_dひも長係数初期値.ToolTipText = "組み編みのひも長に乗算する値"
        f_dひも長係数初期値.Width = 125
        ' 
        ' f_dひも長加算初期値
        ' 
        f_dひも長加算初期値.DataPropertyName = "f_dひも長加算初期値"
        DataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleRight
        f_dひも長加算初期値.DefaultCellStyle = DataGridViewCellStyle17
        f_dひも長加算初期値.HeaderText = "ひも長加算初期値"
        f_dひも長加算初期値.MinimumWidth = 6
        f_dひも長加算初期値.Name = "f_dひも長加算初期値"
        f_dひも長加算初期値.ToolTipText = "組み編みのひも長加算初期値"
        f_dひも長加算初期値.Width = 125
        ' 
        ' f_dコマ寸法係数a
        ' 
        f_dコマ寸法係数a.DataPropertyName = "f_dコマ寸法係数a"
        DataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle18.Format = "N2"
        DataGridViewCellStyle18.NullValue = Nothing
        f_dコマ寸法係数a.DefaultCellStyle = DataGridViewCellStyle18
        f_dコマ寸法係数a.HeaderText = "マ寸法係数a"
        f_dコマ寸法係数a.MinimumWidth = 6
        f_dコマ寸法係数a.Name = "f_dコマ寸法係数a"
        f_dコマ寸法係数a.ToolTipText = "四つ畳み編みの1コマの寸法、ひも幅に対する係数、1が2D値"
        f_dコマ寸法係数a.Width = 125
        ' 
        ' f_dコマ寸法係数b
        ' 
        f_dコマ寸法係数b.DataPropertyName = "f_dコマ寸法係数b"
        DataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle19.Format = "N2"
        DataGridViewCellStyle19.NullValue = Nothing
        f_dコマ寸法係数b.DefaultCellStyle = DataGridViewCellStyle19
        f_dコマ寸法係数b.HeaderText = "コマ寸法係数b"
        f_dコマ寸法係数b.MinimumWidth = 6
        f_dコマ寸法係数b.Name = "f_dコマ寸法係数b"
        f_dコマ寸法係数b.ToolTipText = "四つ畳み編みの1コマの寸法、プラスする係数、0が2D値・1がひも幅"
        f_dコマ寸法係数b.Width = 125
        ' 
        ' f_dコマ要尺係数a
        ' 
        f_dコマ要尺係数a.DataPropertyName = "f_dコマ要尺係数a"
        DataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle20.Format = "N2"
        DataGridViewCellStyle20.NullValue = Nothing
        f_dコマ要尺係数a.DefaultCellStyle = DataGridViewCellStyle20
        f_dコマ要尺係数a.HeaderText = "コマ要尺係数a"
        f_dコマ要尺係数a.MinimumWidth = 6
        f_dコマ要尺係数a.Name = "f_dコマ要尺係数a"
        f_dコマ要尺係数a.ToolTipText = "四つ畳み編みの1コマに必要な長さ、ひも幅に対する係数、1が2D値"
        f_dコマ要尺係数a.Width = 125
        ' 
        ' f_dコマ要尺係数b
        ' 
        f_dコマ要尺係数b.DataPropertyName = "f_dコマ要尺係数b"
        DataGridViewCellStyle21.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle21.Format = "N2"
        DataGridViewCellStyle21.NullValue = Nothing
        f_dコマ要尺係数b.DefaultCellStyle = DataGridViewCellStyle21
        f_dコマ要尺係数b.HeaderText = "コマ要尺係数b"
        f_dコマ要尺係数b.MinimumWidth = 6
        f_dコマ要尺係数b.Name = "f_dコマ要尺係数b"
        f_dコマ要尺係数b.ToolTipText = "四つ畳み編みの1コマに必要な長さ、プラスする係数、0が2D値・1がひも幅"
        f_dコマ要尺係数b.Width = 125
        ' 
        ' f_d四つ畳みひも長加算初期値
        ' 
        f_d四つ畳みひも長加算初期値.DataPropertyName = "f_d四つ畳みひも長加算初期値"
        DataGridViewCellStyle22.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d四つ畳みひも長加算初期値.DefaultCellStyle = DataGridViewCellStyle22
        f_d四つ畳みひも長加算初期値.HeaderText = "四つ畳みひも長加算初期値"
        f_d四つ畳みひも長加算初期値.MinimumWidth = 6
        f_d四つ畳みひも長加算初期値.Name = "f_d四つ畳みひも長加算初期値"
        f_d四つ畳みひも長加算初期値.ToolTipText = "四つ畳み編みの縦横と側面のひも長加算初期値"
        f_d四つ畳みひも長加算初期値.Width = 125
        ' 
        ' f_d目と数える端の目
        ' 
        f_d目と数える端の目.DataPropertyName = "f_d目と数える端の目"
        DataGridViewCellStyle23.Alignment = DataGridViewContentAlignment.MiddleRight
        f_d目と数える端の目.DefaultCellStyle = DataGridViewCellStyle23
        f_d目と数える端の目.HeaderText = "目と数える端の目"
        f_d目と数える端の目.MinimumWidth = 6
        f_d目と数える端の目.Name = "f_d目と数える端の目"
        f_d目と数える端の目.ToolTipText = "上端下端・左端右端・最下段の目がこの値以上のとき、1目とカウントする"
        f_d目と数える端の目.Width = 125
        ' 
        ' f_s色リスト
        ' 
        f_s色リスト.DataPropertyName = "f_s色リスト"
        f_s色リスト.HeaderText = "色リスト"
        f_s色リスト.MinimumWidth = 6
        f_s色リスト.Name = "f_s色リスト"
        f_s色リスト.ToolTipText = "カンマ区切りで複数を入力できます"
        f_s色リスト.Width = 136
        ' 
        ' f_s製品情報
        ' 
        f_s製品情報.DataPropertyName = "f_s製品情報"
        f_s製品情報.HeaderText = "製品情報"
        f_s製品情報.MinimumWidth = 6
        f_s製品情報.Name = "f_s製品情報"
        f_s製品情報.ToolTipText = "メーカーや型番など購入のためのメモ"
        f_s製品情報.Width = 125
        ' 
        ' f_s備考
        ' 
        f_s備考.DataPropertyName = "f_s備考"
        f_s備考.HeaderText = "備考"
        f_s備考.MinimumWidth = 6
        f_s備考.Name = "f_s備考"
        f_s備考.Width = 125
        ' 
        ' ColumnBotton本幅の幅
        ' 
        ColumnBotton本幅の幅.HeaderText = "本幅の幅"
        ColumnBotton本幅の幅.MinimumWidth = 6
        ColumnBotton本幅の幅.Name = "ColumnBotton本幅の幅"
        ColumnBotton本幅の幅.Resizable = DataGridViewTriState.True
        ColumnBotton本幅の幅.SortMode = DataGridViewColumnSortMode.Automatic
        ColumnBotton本幅の幅.Text = "本幅の幅"
        ColumnBotton本幅の幅.ToolTipText = "本幅ごとの幅を個別に変更します"
        ColumnBotton本幅の幅.UseColumnTextForButtonValue = True
        ColumnBotton本幅の幅.Width = 60
        ' 
        ' f_s本幅の幅リスト
        ' 
        f_s本幅の幅リスト.DataPropertyName = "f_s本幅の幅リスト"
        f_s本幅の幅リスト.HeaderText = "本幅の幅リスト"
        f_s本幅の幅リスト.MinimumWidth = 6
        f_s本幅の幅リスト.Name = "f_s本幅の幅リスト"
        f_s本幅の幅リスト.ReadOnly = True
        f_s本幅の幅リスト.ToolTipText = "本幅ごとの幅が個別に指定されている場合"
        f_s本幅の幅リスト.Width = 125
        ' 
        ' frmBandType
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(881, 281)
        Controls.Add(lblバンドの種類名)
        Controls.Add(btn長さと重さ)
        Controls.Add(btn追加)
        Controls.Add(btn複製)
        Controls.Add(txtバンドの種類名)
        Controls.Add(lbl設定時の寸法単位)
        Controls.Add(lbl単位)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        Controls.Add(dgvData)
        MinimumSize = New System.Drawing.Size(764, 297)
        Name = "frmBandType"
        Text = "バンドの種類"
        CType(dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(BindingSourceバンドの種類, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents dgvData As ctrDataGridView
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents btnキャンセル As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents BindingSourceバンドの種類 As BindingSource
    Friend WithEvents lbl設定時の寸法単位 As Label
    Friend WithEvents lbl単位 As Label
    Friend WithEvents txtバンドの種類名 As TextBox
    Friend WithEvents btn複製 As Button
    Friend WithEvents btn追加 As Button
    Friend WithEvents btn長さと重さ As Button
    Friend WithEvents lblバンドの種類名 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents f_sバンドの種類名 As DataGridViewTextBoxColumn
    Friend WithEvents f_i本幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_dバンド幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_d底の厚さ As DataGridViewTextBoxColumn
    Friend WithEvents f_s長さと重さ As DataGridViewTextBoxColumn
    Friend WithEvents ColumnBotton As DataGridViewButtonColumn
    Friend WithEvents f_d短い横ひも長のばらつき As DataGridViewTextBoxColumn
    Friend WithEvents f_d縦ひも間の最小間隔 As DataGridViewTextBoxColumn
    Friend WithEvents f_d垂直ひも加算初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_d立ち上げ時の四角底周の増分 As DataGridViewTextBoxColumn
    Friend WithEvents f_d差しひもの径 As DataGridViewTextBoxColumn
    Friend WithEvents f_d差しひも長加算初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_d楕円底円弧の半径加算 As DataGridViewTextBoxColumn
    Friend WithEvents f_d楕円底周の加算 As DataGridViewTextBoxColumn
    Friend WithEvents f_d立ち上げ時の楕円底周の増分 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも間のすき間初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長係数初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_dひも長加算初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_dコマ寸法係数a As DataGridViewTextBoxColumn
    Friend WithEvents f_dコマ寸法係数b As DataGridViewTextBoxColumn
    Friend WithEvents f_dコマ要尺係数a As DataGridViewTextBoxColumn
    Friend WithEvents f_dコマ要尺係数b As DataGridViewTextBoxColumn
    Friend WithEvents f_d四つ畳みひも長加算初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_d目と数える端の目 As DataGridViewTextBoxColumn
    Friend WithEvents f_s色リスト As DataGridViewTextBoxColumn
    Friend WithEvents f_s製品情報 As DataGridViewTextBoxColumn
    Friend WithEvents f_s備考 As DataGridViewTextBoxColumn
    Friend WithEvents ColumnBotton本幅の幅 As DataGridViewButtonColumn
    Friend WithEvents f_s本幅の幅リスト As DataGridViewTextBoxColumn
End Class
