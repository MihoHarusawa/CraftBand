<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmColorList
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
        clb使用色 = New Windows.Forms.CheckedListBox()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        btnキャンセル = New Windows.Forms.Button()
        btnOK = New Windows.Forms.Button()
        btnチェック反転 = New Windows.Forms.Button()
        SuspendLayout()
        ' 
        ' clb使用色
        ' 
        clb使用色.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        clb使用色.CheckOnClick = True
        clb使用色.FormattingEnabled = True
        clb使用色.Location = New System.Drawing.Point(8, 2)
        clb使用色.Name = "clb使用色"
        clb使用色.Size = New System.Drawing.Size(356, 92)
        clb使用色.TabIndex = 0
        ToolTip1.SetToolTip(clb使用色, "描画色にない色は指定できません")
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnキャンセル.DialogResult = Windows.Forms.DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(253, 125)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 46)
        btnキャンセル.TabIndex = 16
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "変更を保存せずに終了します")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(133, 125)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 46)
        btnOK.TabIndex = 15
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "変更を保存して終了します")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnチェック反転
        ' 
        btnチェック反転.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        btnチェック反転.Location = New System.Drawing.Point(8, 125)
        btnチェック反転.Name = "btnチェック反転"
        btnチェック反転.Size = New System.Drawing.Size(111, 46)
        btnチェック反転.TabIndex = 17
        btnチェック反転.Text = "チェック反転(&R)"
        ToolTip1.SetToolTip(btnチェック反転, "全ての色にチェックを入れます")
        btnチェック反転.UseVisualStyleBackColor = True
        ' 
        ' frmColorList
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 20F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(370, 180)
        Controls.Add(btnチェック反転)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        Controls.Add(clb使用色)
        MaximizeBox = False
        MinimizeBox = False
        MinimumSize = New System.Drawing.Size(266, 179)
        Name = "frmColorList"
        Text = "使用色の選択"
        ResumeLayout(False)

    End Sub

    Friend WithEvents clb使用色 As Windows.Forms.CheckedListBox
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents btnチェック反転 As Windows.Forms.Button
End Class
