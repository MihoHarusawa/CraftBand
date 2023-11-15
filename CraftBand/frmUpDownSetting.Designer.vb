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
        Me.components = New System.ComponentModel.Container()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.cmb上下図名 = New System.Windows.Forms.ComboBox()
        Me.lbl上下図名 = New System.Windows.Forms.Label()
        Me.lbl垂直本数 = New System.Windows.Forms.Label()
        Me.lbl水平本数 = New System.Windows.Forms.Label()
        Me.txtl水平本数 = New System.Windows.Forms.TextBox()
        Me.txtl垂直本数 = New System.Windows.Forms.TextBox()
        Me.lblしてください = New System.Windows.Forms.Label()
        Me.grp反映方法 = New System.Windows.Forms.GroupBox()
        Me.chk繰り返す = New System.Windows.Forms.CheckBox()
        Me.chk入れ換え = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grp反映方法.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(433, 147)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 9
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(316, 147)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 8
        Me.btnOK.Text = "OK(&O)"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'cmb上下図名
        '
        Me.cmb上下図名.FormattingEnabled = True
        Me.cmb上下図名.Location = New System.Drawing.Point(117, 51)
        Me.cmb上下図名.Name = "cmb上下図名"
        Me.cmb上下図名.Size = New System.Drawing.Size(427, 28)
        Me.cmb上下図名.TabIndex = 2
        '
        'lbl上下図名
        '
        Me.lbl上下図名.AutoSize = True
        Me.lbl上下図名.Location = New System.Drawing.Point(12, 54)
        Me.lbl上下図名.Name = "lbl上下図名"
        Me.lbl上下図名.Size = New System.Drawing.Size(69, 20)
        Me.lbl上下図名.TabIndex = 1
        Me.lbl上下図名.Text = "上下図名"
        '
        'lbl垂直本数
        '
        Me.lbl垂直本数.AutoSize = True
        Me.lbl垂直本数.Location = New System.Drawing.Point(281, 98)
        Me.lbl垂直本数.Name = "lbl垂直本数"
        Me.lbl垂直本数.Size = New System.Drawing.Size(69, 20)
        Me.lbl垂直本数.TabIndex = 5
        Me.lbl垂直本数.Text = "垂直本数"
        '
        'lbl水平本数
        '
        Me.lbl水平本数.AutoSize = True
        Me.lbl水平本数.Location = New System.Drawing.Point(116, 98)
        Me.lbl水平本数.Name = "lbl水平本数"
        Me.lbl水平本数.Size = New System.Drawing.Size(69, 20)
        Me.lbl水平本数.TabIndex = 3
        Me.lbl水平本数.Text = "水平本数"
        '
        'txtl水平本数
        '
        Me.txtl水平本数.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtl水平本数.Location = New System.Drawing.Point(191, 96)
        Me.txtl水平本数.Name = "txtl水平本数"
        Me.txtl水平本数.ReadOnly = True
        Me.txtl水平本数.Size = New System.Drawing.Size(71, 27)
        Me.txtl水平本数.TabIndex = 4
        '
        'txtl垂直本数
        '
        Me.txtl垂直本数.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtl垂直本数.Location = New System.Drawing.Point(356, 96)
        Me.txtl垂直本数.Name = "txtl垂直本数"
        Me.txtl垂直本数.ReadOnly = True
        Me.txtl垂直本数.Size = New System.Drawing.Size(71, 27)
        Me.txtl垂直本数.TabIndex = 6
        '
        'lblしてください
        '
        Me.lblしてください.AutoSize = True
        Me.lblしてください.Location = New System.Drawing.Point(12, 16)
        Me.lblしてください.Name = "lblしてください"
        Me.lblしてください.Size = New System.Drawing.Size(73, 20)
        Me.lblしてください.TabIndex = 0
        Me.lblしてください.Text = "してください"
        '
        'grp反映方法
        '
        Me.grp反映方法.Controls.Add(Me.chk繰り返す)
        Me.grp反映方法.Controls.Add(Me.chk入れ換え)
        Me.grp反映方法.Location = New System.Drawing.Point(14, 129)
        Me.grp反映方法.Name = "grp反映方法"
        Me.grp反映方法.Size = New System.Drawing.Size(248, 64)
        Me.grp反映方法.TabIndex = 7
        Me.grp反映方法.TabStop = False
        Me.grp反映方法.Text = "反映方法"
        Me.ToolTip1.SetToolTip(Me.grp反映方法, "現在編集中のひも上下図への反映方法")
        '
        'chk繰り返す
        '
        Me.chk繰り返す.AutoSize = True
        Me.chk繰り返す.Enabled = False
        Me.chk繰り返す.Location = New System.Drawing.Point(127, 27)
        Me.chk繰り返す.Name = "chk繰り返す"
        Me.chk繰り返す.Size = New System.Drawing.Size(83, 24)
        Me.chk繰り返す.TabIndex = 1
        Me.chk繰り返す.Text = "繰り返す"
        Me.ToolTip1.SetToolTip(Me.chk繰り返す, "ONは繰り返す、OFFは1回のみ")
        Me.chk繰り返す.UseVisualStyleBackColor = True
        '
        'chk入れ換え
        '
        Me.chk入れ換え.AutoSize = True
        Me.chk入れ換え.Checked = True
        Me.chk入れ換え.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk入れ換え.Location = New System.Drawing.Point(16, 27)
        Me.chk入れ換え.Name = "chk入れ換え"
        Me.chk入れ換え.Size = New System.Drawing.Size(86, 24)
        Me.chk入れ換え.TabIndex = 0
        Me.chk入れ換え.Text = "入れ換え"
        Me.ToolTip1.SetToolTip(Me.chk入れ換え, "ONは選択した上下図に入れ替え、OFFは現状に追記")
        Me.chk入れ換え.UseVisualStyleBackColor = True
        '
        'frmUpDownSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(556, 205)
        Me.Controls.Add(Me.grp反映方法)
        Me.Controls.Add(Me.lblしてください)
        Me.Controls.Add(Me.txtl垂直本数)
        Me.Controls.Add(Me.txtl水平本数)
        Me.Controls.Add(Me.lbl垂直本数)
        Me.Controls.Add(Me.lbl水平本数)
        Me.Controls.Add(Me.lbl上下図名)
        Me.Controls.Add(Me.cmb上下図名)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUpDownSetting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "上下図の呼び出しと登録"
        Me.grp反映方法.ResumeLayout(False)
        Me.grp反映方法.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
