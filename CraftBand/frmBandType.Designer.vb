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
        Me.dgvData = New System.Windows.Forms.DataGridView()
        Me.BindingSourceバンドの種類 = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lbl設定時の寸法単位 = New System.Windows.Forms.Label()
        Me.lbl単位 = New System.Windows.Forms.Label()
        Me.f_sバンドの種類名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_i本幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_dバンド幅 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d底の厚さ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d短い横ひも長のばらつき = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d縦ひも間の最小間隔 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d垂直ひも加算初期値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d立ち上げ時の四角底周の増分 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d差しひもの径 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d差しひも長加算初期値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d楕円底円弧の半径加算 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d楕円底周の加算 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_d立ち上げ時の楕円底周の増分 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s色リスト = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.f_s備考 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceバンドの種類, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvData
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.dgvData.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvData.AutoGenerateColumns = False
        Me.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.f_sバンドの種類名, Me.f_i本幅, Me.f_dバンド幅, Me.f_d底の厚さ, Me.f_d短い横ひも長のばらつき, Me.f_d縦ひも間の最小間隔, Me.f_d垂直ひも加算初期値, Me.f_d立ち上げ時の四角底周の増分, Me.f_d差しひもの径, Me.f_d差しひも長加算初期値, Me.f_d楕円底円弧の半径加算, Me.f_d楕円底周の加算, Me.f_d立ち上げ時の楕円底周の増分, Me.f_s色リスト, Me.f_s備考})
        Me.dgvData.DataSource = Me.BindingSourceバンドの種類
        Me.dgvData.Location = New System.Drawing.Point(12, 32)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.RowHeadersWidth = 51
        Me.dgvData.RowTemplate.Height = 29
        Me.dgvData.Size = New System.Drawing.Size(715, 138)
        Me.dgvData.TabIndex = 2
        '
        'BindingSourceバンドの種類
        '
        Me.BindingSourceバンドの種類.DataMember = "tblバンドの種類"
        Me.BindingSourceバンドの種類.DataSource = GetType(CraftBand.Tables.dstMasterTables)
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(616, 193)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 4
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(497, 193)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK(&O)"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'lbl設定時の寸法単位
        '
        Me.lbl設定時の寸法単位.AutoSize = True
        Me.lbl設定時の寸法単位.Location = New System.Drawing.Point(60, -2)
        Me.lbl設定時の寸法単位.Name = "lbl設定時の寸法単位"
        Me.lbl設定時の寸法単位.Size = New System.Drawing.Size(0, 20)
        Me.lbl設定時の寸法単位.TabIndex = 1
        '
        'lbl単位
        '
        Me.lbl単位.AutoSize = True
        Me.lbl単位.Location = New System.Drawing.Point(12, -2)
        Me.lbl単位.Name = "lbl単位"
        Me.lbl単位.Size = New System.Drawing.Size(42, 20)
        Me.lbl単位.TabIndex = 0
        Me.lbl単位.Text = "単位:"
        '
        'f_sバンドの種類名
        '
        Me.f_sバンドの種類名.DataPropertyName = "f_sバンドの種類名"
        Me.f_sバンドの種類名.HeaderText = "バンドの種類名"
        Me.f_sバンドの種類名.MinimumWidth = 6
        Me.f_sバンドの種類名.Name = "f_sバンドの種類名"
        Me.f_sバンドの種類名.ToolTipText = "識別可能な名前"
        Me.f_sバンドの種類名.Width = 160
        '
        'f_i本幅
        '
        Me.f_i本幅.DataPropertyName = "f_i本幅"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.f_i本幅.DefaultCellStyle = DataGridViewCellStyle2
        Me.f_i本幅.HeaderText = "本幅"
        Me.f_i本幅.MinimumWidth = 6
        Me.f_i本幅.Name = "f_i本幅"
        Me.f_i本幅.ToolTipText = "何本幅のバンドか"
        Me.f_i本幅.Width = 80
        '
        'f_dバンド幅
        '
        Me.f_dバンド幅.DataPropertyName = "f_dバンド幅"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_dバンド幅.DefaultCellStyle = DataGridViewCellStyle3
        Me.f_dバンド幅.HeaderText = "バンド幅"
        Me.f_dバンド幅.MinimumWidth = 6
        Me.f_dバンド幅.Name = "f_dバンド幅"
        Me.f_dバンド幅.ToolTipText = "設定した単位でバンドの幅をセット"
        Me.f_dバンド幅.Width = 83
        '
        'f_d底の厚さ
        '
        Me.f_d底の厚さ.DataPropertyName = "f_d底の厚さ"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d底の厚さ.DefaultCellStyle = DataGridViewCellStyle4
        Me.f_d底の厚さ.HeaderText = "底の厚さ"
        Me.f_d底の厚さ.MinimumWidth = 6
        Me.f_d底の厚さ.Name = "f_d底の厚さ"
        Me.f_d底の厚さ.ToolTipText = "内側・外側の基本的なサイズ差"
        Me.f_d底の厚さ.Width = 113
        '
        'f_d短い横ひも長のばらつき
        '
        Me.f_d短い横ひも長のばらつき.DataPropertyName = "f_d短い横ひも長のばらつき"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d短い横ひも長のばらつき.DefaultCellStyle = DataGridViewCellStyle5
        Me.f_d短い横ひも長のばらつき.HeaderText = "短い横ひも長のばらつき"
        Me.f_d短い横ひも長のばらつき.MinimumWidth = 6
        Me.f_d短い横ひも長のばらつき.Name = "f_d短い横ひも長のばらつき"
        Me.f_d短い横ひも長のばらつき.ToolTipText = "計算値よりこの設定値分、ひも長を短く出力します"
        Me.f_d短い横ひも長のばらつき.Width = 125
        '
        'f_d縦ひも間の最小間隔
        '
        Me.f_d縦ひも間の最小間隔.DataPropertyName = "f_d縦ひも間の最小間隔"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d縦ひも間の最小間隔.DefaultCellStyle = DataGridViewCellStyle6
        Me.f_d縦ひも間の最小間隔.HeaderText = "縦ひも間の最小間隔"
        Me.f_d縦ひも間の最小間隔.MinimumWidth = 6
        Me.f_d縦ひも間の最小間隔.Name = "f_d縦ひも間の最小間隔"
        Me.f_d縦ひも間の最小間隔.ToolTipText = "縦ひも間の隙間がこの値より小さければ警告します"
        Me.f_d縦ひも間の最小間隔.Width = 125
        '
        'f_d垂直ひも加算初期値
        '
        Me.f_d垂直ひも加算初期値.DataPropertyName = "f_d垂直ひも加算初期値"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d垂直ひも加算初期値.DefaultCellStyle = DataGridViewCellStyle7
        Me.f_d垂直ひも加算初期値.HeaderText = "垂直ひも加算初期値"
        Me.f_d垂直ひも加算初期値.MinimumWidth = 6
        Me.f_d垂直ひも加算初期値.Name = "f_d垂直ひも加算初期値"
        Me.f_d垂直ひも加算初期値.ToolTipText = "垂直ひも加算値の初期値"
        Me.f_d垂直ひも加算初期値.Width = 125
        '
        'f_d立ち上げ時の四角底周の増分
        '
        Me.f_d立ち上げ時の四角底周の増分.DataPropertyName = "f_d立ち上げ時の四角底周の増分"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d立ち上げ時の四角底周の増分.DefaultCellStyle = DataGridViewCellStyle8
        Me.f_d立ち上げ時の四角底周の増分.HeaderText = "立ち上げ時の四角底周の増分"
        Me.f_d立ち上げ時の四角底周の増分.MinimumWidth = 6
        Me.f_d立ち上げ時の四角底周の増分.Name = "f_d立ち上げ時の四角底周の増分"
        Me.f_d立ち上げ時の四角底周の増分.ToolTipText = "四角底の時、底の周長にこの値を加えて側面の周長を得る"
        Me.f_d立ち上げ時の四角底周の増分.Width = 153
        '
        'f_d差しひもの径
        '
        Me.f_d差しひもの径.DataPropertyName = "f_d差しひもの径"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d差しひもの径.DefaultCellStyle = DataGridViewCellStyle9
        Me.f_d差しひもの径.HeaderText = "差しひもの径"
        Me.f_d差しひもの径.MinimumWidth = 6
        Me.f_d差しひもの径.Name = "f_d差しひもの径"
        Me.f_d差しひもの径.ToolTipText = "楕円底設定時に、差しひも分の径として計算する値"
        Me.f_d差しひもの径.Width = 125
        '
        'f_d差しひも長加算初期値
        '
        Me.f_d差しひも長加算初期値.DataPropertyName = "f_d差しひも長加算初期値"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d差しひも長加算初期値.DefaultCellStyle = DataGridViewCellStyle10
        Me.f_d差しひも長加算初期値.HeaderText = "差しひも長加算初期値"
        Me.f_d差しひも長加算初期値.MinimumWidth = 6
        Me.f_d差しひも長加算初期値.Name = "f_d差しひも長加算初期値"
        Me.f_d差しひも長加算初期値.ToolTipText = "差しひも長加算値の初期値"
        Me.f_d差しひも長加算初期値.Width = 125
        '
        'f_d楕円底円弧の半径加算
        '
        Me.f_d楕円底円弧の半径加算.DataPropertyName = "f_d楕円底円弧の半径加算"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d楕円底円弧の半径加算.DefaultCellStyle = DataGridViewCellStyle11
        Me.f_d楕円底円弧の半径加算.HeaderText = "楕円底円弧の半径加算"
        Me.f_d楕円底円弧の半径加算.MinimumWidth = 6
        Me.f_d楕円底円弧の半径加算.Name = "f_d楕円底円弧の半径加算"
        Me.f_d楕円底円弧の半径加算.ToolTipText = "円周計算時の半径の加減値。小さくするにはマイナスを設定"
        Me.f_d楕円底円弧の半径加算.Width = 125
        '
        'f_d楕円底周の加算
        '
        Me.f_d楕円底周の加算.DataPropertyName = "f_d楕円底周の加算"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d楕円底周の加算.DefaultCellStyle = DataGridViewCellStyle12
        Me.f_d楕円底周の加算.HeaderText = "楕円底周の加算"
        Me.f_d楕円底周の加算.MinimumWidth = 6
        Me.f_d楕円底周の加算.Name = "f_d楕円底周の加算"
        Me.f_d楕円底周の加算.ToolTipText = "楕円部の周の加減値。斜めカット分のマイナスを設定"
        Me.f_d楕円底周の加算.Width = 125
        '
        'f_d立ち上げ時の楕円底周の増分
        '
        Me.f_d立ち上げ時の楕円底周の増分.DataPropertyName = "f_d立ち上げ時の楕円底周の増分"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.f_d立ち上げ時の楕円底周の増分.DefaultCellStyle = DataGridViewCellStyle13
        Me.f_d立ち上げ時の楕円底周の増分.HeaderText = "立ち上げ時の楕円底周の増分"
        Me.f_d立ち上げ時の楕円底周の増分.MinimumWidth = 6
        Me.f_d立ち上げ時の楕円底周の増分.Name = "f_d立ち上げ時の楕円底周の増分"
        Me.f_d立ち上げ時の楕円底周の増分.ToolTipText = "楕円底の時、底の周長にこの値を加えて側面の周長を得る"
        Me.f_d立ち上げ時の楕円底周の増分.Width = 125
        '
        'f_s色リスト
        '
        Me.f_s色リスト.DataPropertyName = "f_s色リスト"
        Me.f_s色リスト.HeaderText = "色リスト"
        Me.f_s色リスト.MinimumWidth = 6
        Me.f_s色リスト.Name = "f_s色リスト"
        Me.f_s色リスト.ToolTipText = "カンマ区切りで複数を入力できます"
        Me.f_s色リスト.Width = 136
        '
        'f_s備考
        '
        Me.f_s備考.DataPropertyName = "f_s備考"
        Me.f_s備考.HeaderText = "備考"
        Me.f_s備考.MinimumWidth = 6
        Me.f_s備考.Name = "f_s備考"
        Me.f_s備考.Width = 125
        '
        'frmBandType
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(746, 250)
        Me.Controls.Add(Me.lbl設定時の寸法単位)
        Me.Controls.Add(Me.lbl単位)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.dgvData)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(764, 297)
        Me.Name = "frmBandType"
        Me.Text = "バンドの種類"
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceバンドの種類, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvData As DataGridView
    Friend WithEvents Fsバンドの種類名DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Fi本幅DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Fdバンド幅DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Fs色リストDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents btnキャンセル As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents BindingSourceバンドの種類 As BindingSource
    Friend WithEvents lbl設定時の寸法単位 As Label
    Friend WithEvents lbl単位 As Label
    Friend WithEvents f_sバンドの種類名 As DataGridViewTextBoxColumn
    Friend WithEvents f_i本幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_dバンド幅 As DataGridViewTextBoxColumn
    Friend WithEvents f_d底の厚さ As DataGridViewTextBoxColumn
    Friend WithEvents f_d短い横ひも長のばらつき As DataGridViewTextBoxColumn
    Friend WithEvents f_d縦ひも間の最小間隔 As DataGridViewTextBoxColumn
    Friend WithEvents f_d垂直ひも加算初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_d立ち上げ時の四角底周の増分 As DataGridViewTextBoxColumn
    Friend WithEvents f_d差しひもの径 As DataGridViewTextBoxColumn
    Friend WithEvents f_d差しひも長加算初期値 As DataGridViewTextBoxColumn
    Friend WithEvents f_d楕円底円弧の半径加算 As DataGridViewTextBoxColumn
    Friend WithEvents f_d楕円底周の加算 As DataGridViewTextBoxColumn
    Friend WithEvents f_d立ち上げ時の楕円底周の増分 As DataGridViewTextBoxColumn
    Friend WithEvents f_s色リスト As DataGridViewTextBoxColumn
    Friend WithEvents f_s備考 As DataGridViewTextBoxColumn
End Class
