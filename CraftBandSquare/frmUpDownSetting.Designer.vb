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
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.cmb上下模様名 = New System.Windows.Forms.ComboBox()
        Me.lbl上下模様名 = New System.Windows.Forms.Label()
        Me.lbl垂直本数 = New System.Windows.Forms.Label()
        Me.lbl水平本数 = New System.Windows.Forms.Label()
        Me.txtl水平本数 = New System.Windows.Forms.TextBox()
        Me.txtl垂直本数 = New System.Windows.Forms.TextBox()
        Me.lblしてください = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(433, 147)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 7
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(316, 147)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "OK(&O)"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'cmb上下模様名
        '
        Me.cmb上下模様名.FormattingEnabled = True
        Me.cmb上下模様名.Location = New System.Drawing.Point(117, 51)
        Me.cmb上下模様名.Name = "cmb上下模様名"
        Me.cmb上下模様名.Size = New System.Drawing.Size(427, 28)
        Me.cmb上下模様名.TabIndex = 8
        '
        'lbl上下模様名
        '
        Me.lbl上下模様名.AutoSize = True
        Me.lbl上下模様名.Location = New System.Drawing.Point(12, 54)
        Me.lbl上下模様名.Name = "lbl上下模様名"
        Me.lbl上下模様名.Size = New System.Drawing.Size(84, 20)
        Me.lbl上下模様名.TabIndex = 9
        Me.lbl上下模様名.Text = "上下模様名"
        '
        'lbl垂直本数
        '
        Me.lbl垂直本数.AutoSize = True
        Me.lbl垂直本数.Location = New System.Drawing.Point(281, 98)
        Me.lbl垂直本数.Name = "lbl垂直本数"
        Me.lbl垂直本数.Size = New System.Drawing.Size(69, 20)
        Me.lbl垂直本数.TabIndex = 12
        Me.lbl垂直本数.Text = "垂直本数"
        '
        'lbl水平本数
        '
        Me.lbl水平本数.AutoSize = True
        Me.lbl水平本数.Location = New System.Drawing.Point(116, 98)
        Me.lbl水平本数.Name = "lbl水平本数"
        Me.lbl水平本数.Size = New System.Drawing.Size(69, 20)
        Me.lbl水平本数.TabIndex = 10
        Me.lbl水平本数.Text = "水平本数"
        '
        'txtl水平本数
        '
        Me.txtl水平本数.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtl水平本数.Location = New System.Drawing.Point(191, 96)
        Me.txtl水平本数.Name = "txtl水平本数"
        Me.txtl水平本数.ReadOnly = True
        Me.txtl水平本数.Size = New System.Drawing.Size(71, 27)
        Me.txtl水平本数.TabIndex = 13
        '
        'txtl垂直本数
        '
        Me.txtl垂直本数.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtl垂直本数.Location = New System.Drawing.Point(356, 96)
        Me.txtl垂直本数.Name = "txtl垂直本数"
        Me.txtl垂直本数.ReadOnly = True
        Me.txtl垂直本数.Size = New System.Drawing.Size(71, 27)
        Me.txtl垂直本数.TabIndex = 14
        '
        'lblしてください
        '
        Me.lblしてください.AutoSize = True
        Me.lblしてください.Location = New System.Drawing.Point(12, 16)
        Me.lblしてください.Name = "lblしてください"
        Me.lblしてください.Size = New System.Drawing.Size(73, 20)
        Me.lblしてください.TabIndex = 15
        Me.lblしてください.Text = "してください"
        '
        'frmUpDownSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(556, 205)
        Me.Controls.Add(Me.lblしてください)
        Me.Controls.Add(Me.txtl垂直本数)
        Me.Controls.Add(Me.txtl水平本数)
        Me.Controls.Add(Me.lbl垂直本数)
        Me.Controls.Add(Me.lbl水平本数)
        Me.Controls.Add(Me.lbl上下模様名)
        Me.Controls.Add(Me.cmb上下模様名)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUpDownSetting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "上下模様の呼び出しと登録"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnキャンセル As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents cmb上下模様名 As ComboBox
    Friend WithEvents lbl上下模様名 As Label
    Friend WithEvents lbl垂直本数 As Label
    Friend WithEvents lbl水平本数 As Label
    Friend WithEvents txtl水平本数 As TextBox
    Friend WithEvents txtl垂直本数 As TextBox
    Friend WithEvents lblしてください As Label
End Class
