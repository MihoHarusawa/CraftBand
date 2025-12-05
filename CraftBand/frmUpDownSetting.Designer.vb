<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpDownSetting
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
        btnキャンセル = New Windows.Forms.Button()
        btnOK = New Windows.Forms.Button()
        cmb上下図名 = New Windows.Forms.ComboBox()
        lbl上下図名 = New Windows.Forms.Label()
        lbl垂直本数 = New Windows.Forms.Label()
        lbl水平本数 = New Windows.Forms.Label()
        txtl水平本数 = New Windows.Forms.TextBox()
        txtl垂直本数 = New Windows.Forms.TextBox()
        lblしてください = New Windows.Forms.Label()
        grp反映方法 = New Windows.Forms.GroupBox()
        chk繰り返す = New Windows.Forms.CheckBox()
        chk入れ換え = New Windows.Forms.CheckBox()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        grp反映方法.SuspendLayout()
        SuspendLayout()
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnキャンセル.DialogResult = Windows.Forms.DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(433, 140)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 44)
        btnキャンセル.TabIndex = 9
        btnキャンセル.Text = "キャンセル(&C)"
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(316, 140)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 44)
        btnOK.TabIndex = 8
        btnOK.Text = "OK(&O)"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' cmb上下図名
        ' 
        cmb上下図名.FormattingEnabled = True
        cmb上下図名.Location = New System.Drawing.Point(117, 48)
        cmb上下図名.Name = "cmb上下図名"
        cmb上下図名.Size = New System.Drawing.Size(427, 27)
        cmb上下図名.TabIndex = 2
        ' 
        ' lbl上下図名
        ' 
        lbl上下図名.AutoSize = True
        lbl上下図名.Location = New System.Drawing.Point(12, 51)
        lbl上下図名.Name = "lbl上下図名"
        lbl上下図名.Size = New System.Drawing.Size(65, 19)
        lbl上下図名.TabIndex = 1
        lbl上下図名.Text = "上下図名"
        ' 
        ' lbl垂直本数
        ' 
        lbl垂直本数.AutoSize = True
        lbl垂直本数.Location = New System.Drawing.Point(281, 93)
        lbl垂直本数.Name = "lbl垂直本数"
        lbl垂直本数.Size = New System.Drawing.Size(65, 19)
        lbl垂直本数.TabIndex = 5
        lbl垂直本数.Text = "垂直本数"
        ' 
        ' lbl水平本数
        ' 
        lbl水平本数.AutoSize = True
        lbl水平本数.Location = New System.Drawing.Point(116, 93)
        lbl水平本数.Name = "lbl水平本数"
        lbl水平本数.Size = New System.Drawing.Size(65, 19)
        lbl水平本数.TabIndex = 3
        lbl水平本数.Text = "水平本数"
        ' 
        ' txtl水平本数
        ' 
        txtl水平本数.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        txtl水平本数.Location = New System.Drawing.Point(191, 91)
        txtl水平本数.Name = "txtl水平本数"
        txtl水平本数.ReadOnly = True
        txtl水平本数.Size = New System.Drawing.Size(71, 26)
        txtl水平本数.TabIndex = 4
        ' 
        ' txtl垂直本数
        ' 
        txtl垂直本数.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        txtl垂直本数.Location = New System.Drawing.Point(356, 91)
        txtl垂直本数.Name = "txtl垂直本数"
        txtl垂直本数.ReadOnly = True
        txtl垂直本数.Size = New System.Drawing.Size(71, 26)
        txtl垂直本数.TabIndex = 6
        ' 
        ' lblしてください
        ' 
        lblしてください.AutoSize = True
        lblしてください.Location = New System.Drawing.Point(12, 15)
        lblしてください.Name = "lblしてください"
        lblしてください.Size = New System.Drawing.Size(68, 19)
        lblしてください.TabIndex = 0
        lblしてください.Text = "してください"
        ' 
        ' grp反映方法
        ' 
        grp反映方法.Controls.Add(chk繰り返す)
        grp反映方法.Controls.Add(chk入れ換え)
        grp反映方法.Location = New System.Drawing.Point(14, 123)
        grp反映方法.Name = "grp反映方法"
        grp反映方法.Size = New System.Drawing.Size(248, 61)
        grp反映方法.TabIndex = 7
        grp反映方法.TabStop = False
        grp反映方法.Text = "反映方法"
        ToolTip1.SetToolTip(grp反映方法, "現在編集中のひも上下図への反映方法")
        ' 
        ' chk繰り返す
        ' 
        chk繰り返す.AutoSize = True
        chk繰り返す.Enabled = False
        chk繰り返す.Location = New System.Drawing.Point(127, 26)
        chk繰り返す.Name = "chk繰り返す"
        chk繰り返す.Size = New System.Drawing.Size(77, 23)
        chk繰り返す.TabIndex = 1
        chk繰り返す.Text = "繰り返す"
        ToolTip1.SetToolTip(chk繰り返す, "ONは繰り返す、OFFは1回のみ")
        chk繰り返す.UseVisualStyleBackColor = True
        ' 
        ' chk入れ換え
        ' 
        chk入れ換え.AutoSize = True
        chk入れ換え.Checked = True
        chk入れ換え.CheckState = Windows.Forms.CheckState.Checked
        chk入れ換え.Location = New System.Drawing.Point(16, 26)
        chk入れ換え.Name = "chk入れ換え"
        chk入れ換え.Size = New System.Drawing.Size(79, 23)
        chk入れ換え.TabIndex = 0
        chk入れ換え.Text = "入れ換え"
        ToolTip1.SetToolTip(chk入れ換え, "ONは選択した上下図に入れ替え、OFFは現状に追記")
        chk入れ換え.UseVisualStyleBackColor = True
        ' 
        ' frmUpDownSetting
        ' 
        AcceptButton = btnキャンセル
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(556, 195)
        Controls.Add(grp反映方法)
        Controls.Add(lblしてください)
        Controls.Add(txtl垂直本数)
        Controls.Add(txtl水平本数)
        Controls.Add(lbl垂直本数)
        Controls.Add(lbl水平本数)
        Controls.Add(lbl上下図名)
        Controls.Add(cmb上下図名)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmUpDownSetting"
        StartPosition = Windows.Forms.FormStartPosition.CenterParent
        Text = "上下図の呼び出しと登録"
        grp反映方法.ResumeLayout(False)
        grp反映方法.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents cmb上下図名 As Windows.Forms.ComboBox
    Friend WithEvents lbl上下図名 As Windows.Forms.Label
    Friend WithEvents lbl垂直本数 As Windows.Forms.Label
    Friend WithEvents lbl水平本数 As Windows.Forms.Label
    Friend WithEvents txtl水平本数 As Windows.Forms.TextBox
    Friend WithEvents txtl垂直本数 As Windows.Forms.TextBox
    Friend WithEvents lblしてください As Windows.Forms.Label
    Friend WithEvents grp反映方法 As Windows.Forms.GroupBox
    Friend WithEvents chk繰り返す As Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents chk入れ換え As Windows.Forms.CheckBox
End Class
