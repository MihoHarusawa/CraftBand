<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStepImages
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
        components = New ComponentModel.Container()
        btnOK = New Windows.Forms.Button()
        txtMessage = New Windows.Forms.TextBox()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        nud表示番号 = New Windows.Forms.NumericUpDown()
        btn生成先フォルダ = New Windows.Forms.Button()
        lbl表示番号 = New Windows.Forms.Label()
        txtファイル名 = New Windows.Forms.TextBox()
        lblファイル名 = New Windows.Forms.Label()
        btn列を表示 = New Windows.Forms.Button()
        btn列を非表示 = New Windows.Forms.Button()
        btn画像生成 = New Windows.Forms.Button()
        lbl表示順最大値 = New Windows.Forms.Label()
        txt表示順最大値 = New Windows.Forms.TextBox()
        btn一括生成 = New Windows.Forms.Button()
        lbl非表示色 = New Windows.Forms.Label()
        cmb非表示色 = New Windows.Forms.ComboBox()
        txt生成先フォルダ = New Windows.Forms.TextBox()
        lbl生成先フォルダ = New Windows.Forms.Label()
        btnフォルダを開く = New Windows.Forms.Button()
        FolderBrowserDialog1 = New Windows.Forms.FolderBrowserDialog()
        Label2 = New Windows.Forms.Label()
        grp画像 = New Windows.Forms.GroupBox()
        radプレビュー2 = New Windows.Forms.RadioButton()
        radプレビュー = New Windows.Forms.RadioButton()
        btnフォルダクリア = New Windows.Forms.Button()
        lbl範囲 = New Windows.Forms.Label()
        nudFrom = New Windows.Forms.NumericUpDown()
        nudTo = New Windows.Forms.NumericUpDown()
        lblから = New Windows.Forms.Label()
        CType(nud表示番号, ComponentModel.ISupportInitialize).BeginInit()
        grp画像.SuspendLayout()
        CType(nudFrom, ComponentModel.ISupportInitialize).BeginInit()
        CType(nudTo, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.DialogResult = Windows.Forms.DialogResult.OK
        btnOK.Location = New System.Drawing.Point(480, 234)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 44)
        btnOK.TabIndex = 24
        btnOK.Text = "OK(&O)"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' txtMessage
        ' 
        txtMessage.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txtMessage.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        txtMessage.Location = New System.Drawing.Point(12, 190)
        txtMessage.Multiline = True
        txtMessage.Name = "txtMessage"
        txtMessage.ReadOnly = True
        txtMessage.ScrollBars = Windows.Forms.ScrollBars.Both
        txtMessage.Size = New System.Drawing.Size(579, 35)
        txtMessage.TabIndex = 19
        ' 
        ' nud表示番号
        ' 
        nud表示番号.Location = New System.Drawing.Point(85, 142)
        nud表示番号.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        nud表示番号.MinimumSize = New System.Drawing.Size(61, 0)
        nud表示番号.Name = "nud表示番号"
        nud表示番号.Size = New System.Drawing.Size(61, 26)
        nud表示番号.TabIndex = 12
        nud表示番号.TextAlign = Windows.Forms.HorizontalAlignment.Center
        ToolTip1.SetToolTip(nud表示番号, "生成する画像の表示番号")
        ' 
        ' btn生成先フォルダ
        ' 
        btn生成先フォルダ.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        btn生成先フォルダ.Location = New System.Drawing.Point(372, 90)
        btn生成先フォルダ.Name = "btn生成先フォルダ"
        btn生成先フォルダ.Size = New System.Drawing.Size(30, 28)
        btn生成先フォルダ.TabIndex = 7
        btn生成先フォルダ.Text = "..."
        ToolTip1.SetToolTip(btn生成先フォルダ, "ファイルダイアログを開きます")
        btn生成先フォルダ.UseVisualStyleBackColor = True
        ' 
        ' lbl表示番号
        ' 
        lbl表示番号.AutoSize = True
        lbl表示番号.Location = New System.Drawing.Point(14, 144)
        lbl表示番号.Name = "lbl表示番号"
        lbl表示番号.Size = New System.Drawing.Size(65, 19)
        lbl表示番号.TabIndex = 11
        lbl表示番号.Text = "表示番号"
        ' 
        ' txtファイル名
        ' 
        txtファイル名.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        txtファイル名.Location = New System.Drawing.Point(419, 92)
        txtファイル名.Name = "txtファイル名"
        txtファイル名.Size = New System.Drawing.Size(108, 26)
        txtファイル名.TabIndex = 9
        ' 
        ' lblファイル名
        ' 
        lblファイル名.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        lblファイル名.AutoSize = True
        lblファイル名.Location = New System.Drawing.Point(419, 68)
        lblファイル名.Name = "lblファイル名"
        lblファイル名.Size = New System.Drawing.Size(64, 19)
        lblファイル名.TabIndex = 8
        lblファイル名.Text = "ファイル名"
        ' 
        ' btn列を表示
        ' 
        btn列を表示.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn列を表示.Location = New System.Drawing.Point(246, 234)
        btn列を表示.Name = "btn列を表示"
        btn列を表示.Size = New System.Drawing.Size(111, 44)
        btn列を表示.TabIndex = 22
        btn列を表示.Text = "列を表示(&Y)"
        btn列を表示.UseVisualStyleBackColor = True
        ' 
        ' btn列を非表示
        ' 
        btn列を非表示.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btn列を非表示.Location = New System.Drawing.Point(363, 234)
        btn列を非表示.Name = "btn列を非表示"
        btn列を非表示.Size = New System.Drawing.Size(111, 44)
        btn列を非表示.TabIndex = 23
        btn列を非表示.Text = "列を非表示(&N)"
        btn列を非表示.UseVisualStyleBackColor = True
        ' 
        ' btn画像生成
        ' 
        btn画像生成.Enabled = False
        btn画像生成.Location = New System.Drawing.Point(156, 131)
        btn画像生成.Name = "btn画像生成"
        btn画像生成.Size = New System.Drawing.Size(111, 44)
        btn画像生成.TabIndex = 13
        btn画像生成.Text = "画像生成(&I)"
        btn画像生成.UseVisualStyleBackColor = True
        ' 
        ' lbl表示順最大値
        ' 
        lbl表示順最大値.AutoSize = True
        lbl表示順最大値.Location = New System.Drawing.Point(250, 39)
        lbl表示順最大値.Name = "lbl表示順最大値"
        lbl表示順最大値.Size = New System.Drawing.Size(93, 19)
        lbl表示順最大値.TabIndex = 1
        lbl表示順最大値.Text = "表示順最大値"
        ' 
        ' txt表示順最大値
        ' 
        txt表示順最大値.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        txt表示順最大値.Location = New System.Drawing.Point(349, 37)
        txt表示順最大値.Name = "txt表示順最大値"
        txt表示順最大値.ReadOnly = True
        txt表示順最大値.Size = New System.Drawing.Size(45, 26)
        txt表示順最大値.TabIndex = 2
        txt表示順最大値.TextAlign = Windows.Forms.HorizontalAlignment.Center
        ' 
        ' btn一括生成
        ' 
        btn一括生成.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        btn一括生成.Enabled = False
        btn一括生成.Location = New System.Drawing.Point(480, 131)
        btn一括生成.Name = "btn一括生成"
        btn一括生成.Size = New System.Drawing.Size(111, 44)
        btn一括生成.TabIndex = 18
        btn一括生成.Text = "一括生成(&A)"
        btn一括生成.UseVisualStyleBackColor = True
        ' 
        ' lbl非表示色
        ' 
        lbl非表示色.AutoSize = True
        lbl非表示色.Location = New System.Drawing.Point(424, 39)
        lbl非表示色.Name = "lbl非表示色"
        lbl非表示色.Size = New System.Drawing.Size(65, 19)
        lbl非表示色.TabIndex = 3
        lbl非表示色.Text = "非表示色"
        ' 
        ' cmb非表示色
        ' 
        cmb非表示色.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        cmb非表示色.FormattingEnabled = True
        cmb非表示色.Location = New System.Drawing.Point(495, 36)
        cmb非表示色.Name = "cmb非表示色"
        cmb非表示色.Size = New System.Drawing.Size(96, 27)
        cmb非表示色.TabIndex = 4
        ' 
        ' txt生成先フォルダ
        ' 
        txt生成先フォルダ.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txt生成先フォルダ.Location = New System.Drawing.Point(115, 92)
        txt生成先フォルダ.Name = "txt生成先フォルダ"
        txt生成先フォルダ.Size = New System.Drawing.Size(251, 26)
        txt生成先フォルダ.TabIndex = 6
        ' 
        ' lbl生成先フォルダ
        ' 
        lbl生成先フォルダ.AutoSize = True
        lbl生成先フォルダ.Location = New System.Drawing.Point(12, 95)
        lbl生成先フォルダ.Name = "lbl生成先フォルダ"
        lbl生成先フォルダ.Size = New System.Drawing.Size(94, 19)
        lbl生成先フォルダ.TabIndex = 5
        lbl生成先フォルダ.Text = "生成先フォルダ"
        ' 
        ' btnフォルダを開く
        ' 
        btnフォルダを開く.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btnフォルダを開く.Location = New System.Drawing.Point(12, 234)
        btnフォルダを開く.Name = "btnフォルダを開く"
        btnフォルダを開く.Size = New System.Drawing.Size(111, 44)
        btnフォルダを開く.TabIndex = 20
        btnフォルダを開く.Text = "フォルダを開く(&F)"
        btnフォルダを開く.UseVisualStyleBackColor = True
        ' 
        ' FolderBrowserDialog1
        ' 
        FolderBrowserDialog1.Description = "生成した画像ファイルを置くフォルダを指定してください"
        ' 
        ' Label2
        ' 
        Label2.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(526, 99)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(65, 19)
        Label2.TabIndex = 10
        Label2.Text = "_0000.gif"
        ' 
        ' grp画像
        ' 
        grp画像.Controls.Add(radプレビュー2)
        grp画像.Controls.Add(radプレビュー)
        grp画像.Location = New System.Drawing.Point(12, 12)
        grp画像.Name = "grp画像"
        grp画像.Size = New System.Drawing.Size(210, 63)
        grp画像.TabIndex = 0
        grp画像.TabStop = False
        grp画像.Text = "画像"
        ' 
        ' radプレビュー2
        ' 
        radプレビュー2.AutoSize = True
        radプレビュー2.Location = New System.Drawing.Point(103, 25)
        radプレビュー2.Name = "radプレビュー2"
        radプレビュー2.Size = New System.Drawing.Size(82, 23)
        radプレビュー2.TabIndex = 1
        radプレビュー2.Text = "プレビュー2"
        radプレビュー2.UseVisualStyleBackColor = True
        ' 
        ' radプレビュー
        ' 
        radプレビュー.AutoSize = True
        radプレビュー.Checked = True
        radプレビュー.Location = New System.Drawing.Point(19, 25)
        radプレビュー.Name = "radプレビュー"
        radプレビュー.Size = New System.Drawing.Size(74, 23)
        radプレビュー.TabIndex = 0
        radプレビュー.TabStop = True
        radプレビュー.Text = "プレビュー"
        radプレビュー.UseVisualStyleBackColor = True
        ' 
        ' btnフォルダクリア
        ' 
        btnフォルダクリア.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btnフォルダクリア.Location = New System.Drawing.Point(129, 234)
        btnフォルダクリア.Name = "btnフォルダクリア"
        btnフォルダクリア.Size = New System.Drawing.Size(111, 44)
        btnフォルダクリア.TabIndex = 21
        btnフォルダクリア.Text = "フォルダクリア(&D)"
        btnフォルダクリア.UseVisualStyleBackColor = True
        ' 
        ' lbl範囲
        ' 
        lbl範囲.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        lbl範囲.AutoSize = True
        lbl範囲.Location = New System.Drawing.Point(283, 144)
        lbl範囲.Name = "lbl範囲"
        lbl範囲.Size = New System.Drawing.Size(37, 19)
        lbl範囲.TabIndex = 14
        lbl範囲.Text = "範囲"
        ' 
        ' nudFrom
        ' 
        nudFrom.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        nudFrom.Location = New System.Drawing.Point(326, 142)
        nudFrom.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        nudFrom.MinimumSize = New System.Drawing.Size(61, 0)
        nudFrom.Name = "nudFrom"
        nudFrom.Size = New System.Drawing.Size(61, 26)
        nudFrom.TabIndex = 15
        nudFrom.TextAlign = Windows.Forms.HorizontalAlignment.Center
        ' 
        ' nudTo
        ' 
        nudTo.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        nudTo.Location = New System.Drawing.Point(413, 142)
        nudTo.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        nudTo.MinimumSize = New System.Drawing.Size(61, 0)
        nudTo.Name = "nudTo"
        nudTo.Size = New System.Drawing.Size(61, 26)
        nudTo.TabIndex = 17
        nudTo.TextAlign = Windows.Forms.HorizontalAlignment.Center
        ' 
        ' lblから
        ' 
        lblから.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Right
        lblから.AutoSize = True
        lblから.Location = New System.Drawing.Point(390, 144)
        lblから.Name = "lblから"
        lblから.Size = New System.Drawing.Size(23, 19)
        lblから.TabIndex = 16
        lblから.Text = "～"
        ' 
        ' frmStepImages
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(603, 286)
        Controls.Add(nudTo)
        Controls.Add(grp画像)
        Controls.Add(btn生成先フォルダ)
        Controls.Add(lbl非表示色)
        Controls.Add(cmb非表示色)
        Controls.Add(btnフォルダクリア)
        Controls.Add(btn一括生成)
        Controls.Add(txt表示順最大値)
        Controls.Add(lbl表示順最大値)
        Controls.Add(btnフォルダを開く)
        Controls.Add(btn画像生成)
        Controls.Add(lbl生成先フォルダ)
        Controls.Add(Label2)
        Controls.Add(lblファイル名)
        Controls.Add(txt生成先フォルダ)
        Controls.Add(txtファイル名)
        Controls.Add(nudFrom)
        Controls.Add(lblから)
        Controls.Add(lbl範囲)
        Controls.Add(nud表示番号)
        Controls.Add(lbl表示番号)
        Controls.Add(txtMessage)
        Controls.Add(btn列を非表示)
        Controls.Add(btn列を表示)
        Controls.Add(btnOK)
        MaximizeBox = False
        MinimizeBox = False
        MinimumSize = New System.Drawing.Size(619, 325)
        Name = "frmStepImages"
        StartPosition = Windows.Forms.FormStartPosition.CenterParent
        Text = "表示順画像生成"
        CType(nud表示番号, ComponentModel.ISupportInitialize).EndInit()
        grp画像.ResumeLayout(False)
        grp画像.PerformLayout()
        CType(nudFrom, ComponentModel.ISupportInitialize).EndInit()
        CType(nudTo, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents txtMessage As Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents nud表示番号 As Windows.Forms.NumericUpDown
    Friend WithEvents lbl表示番号 As Windows.Forms.Label
    Friend WithEvents txtファイル名 As Windows.Forms.TextBox
    Friend WithEvents lblファイル名 As Windows.Forms.Label
    Friend WithEvents btn列を表示 As Windows.Forms.Button
    Friend WithEvents btn列を非表示 As Windows.Forms.Button
    Friend WithEvents btn画像生成 As Windows.Forms.Button
    Friend WithEvents lbl表示順最大値 As Windows.Forms.Label
    Friend WithEvents txt表示順最大値 As Windows.Forms.TextBox
    Friend WithEvents btn一括生成 As Windows.Forms.Button
    Friend WithEvents lbl非表示色 As Windows.Forms.Label
    Friend WithEvents cmb非表示色 As Windows.Forms.ComboBox
    Friend WithEvents txt生成先フォルダ As Windows.Forms.TextBox
    Friend WithEvents lbl生成先フォルダ As Windows.Forms.Label
    Friend WithEvents btnフォルダを開く As Windows.Forms.Button
    Friend WithEvents btn生成先フォルダ As Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As Windows.Forms.FolderBrowserDialog
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents grp画像 As Windows.Forms.GroupBox
    Friend WithEvents radプレビュー2 As Windows.Forms.RadioButton
    Friend WithEvents radプレビュー As Windows.Forms.RadioButton
    Friend WithEvents btnフォルダクリア As Windows.Forms.Button
    Friend WithEvents lbl範囲 As Windows.Forms.Label
    Friend WithEvents nudFrom As Windows.Forms.NumericUpDown
    Friend WithEvents nudTo As Windows.Forms.NumericUpDown
    Friend WithEvents lblから As Windows.Forms.Label
End Class
