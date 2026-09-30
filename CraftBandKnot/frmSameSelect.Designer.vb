<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSameSelect
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
        lbl長さ指定 = New Label()
        rad縦横同一 = New RadioButton()
        rad対角線同一 = New RadioButton()
        ToolTip1 = New ToolTip(components)
        btnOK = New Button()
        btnキャンセル = New Button()
        radクリア = New RadioButton()
        lblエラー = New Label()
        SuspendLayout()
        ' 
        ' lbl長さ指定
        ' 
        lbl長さ指定.AutoSize = True
        lbl長さ指定.Location = New Point(12, 9)
        lbl長さ指定.Name = "lbl長さ指定"
        lbl長さ指定.Size = New Size(459, 57)
        lbl長さ指定.TabIndex = 0
        lbl長さ指定.Text = "現在のコマ数で、縦ひも・横ひもが全て同じ長さになるよう計算します。" & vbCrLf & "結果は、各ひもの「ひも長加算」「ひも長加算2」に上書きセットします。" & vbCrLf & "正方形の底編みにおいて、どの値に合わせるか、基準となる長さを指定してください。"
        ' 
        ' rad縦横同一
        ' 
        rad縦横同一.AutoSize = True
        rad縦横同一.Location = New Point(84, 108)
        rad縦横同一.Name = "rad縦横同一"
        rad縦横同一.Size = New Size(328, 23)
        rad縦横同一.TabIndex = 1
        rad縦横同一.Text = "底編み完了時、4方向全ての残りひもが同じになる長さ" & vbCrLf
        rad縦横同一.UseVisualStyleBackColor = True
        ' 
        ' rad対角線同一
        ' 
        rad対角線同一.AutoSize = True
        rad対角線同一.Location = New Point(84, 137)
        rad対角線同一.Name = "rad対角線同一"
        rad対角線同一.Size = New Size(384, 23)
        rad対角線同一.TabIndex = 2
        rad対角線同一.Text = "対角線位置のコマ全てが、縦ひも・横ひもとも中央に編まれる長さ"
        rad対角線同一.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.DialogResult = DialogResult.OK
        btnOK.Location = New Point(265, 203)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(111, 44)
        btnOK.TabIndex = 4
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "加算値をセットします")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.DialogResult = DialogResult.Cancel
        btnキャンセル.Location = New Point(382, 203)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New Size(111, 44)
        btnキャンセル.TabIndex = 5
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "加算値のセットを取りやめます")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' radクリア
        ' 
        radクリア.AutoSize = True
        radクリア.Location = New Point(84, 166)
        radクリア.Name = "radクリア"
        radクリア.Size = New Size(390, 23)
        radクリア.TabIndex = 3
        radクリア.Text = "「ひも長加算」「ひも長加算2」を全てクリアし、各最適な長さに戻す"
        radクリア.UseVisualStyleBackColor = True
        ' 
        ' lblエラー
        ' 
        lblエラー.AutoSize = True
        lblエラー.ForeColor = Color.Red
        lblエラー.Location = New Point(12, 75)
        lblエラー.Name = "lblエラー"
        lblエラー.Size = New Size(97, 19)
        lblエラー.TabIndex = 6
        lblエラー.Text = "(エラーメッセージ)"
        lblエラー.Visible = False
        ' 
        ' frmSameSelect
        ' 
        AutoScaleDimensions = New SizeF(8F, 19F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(505, 255)
        Controls.Add(lblエラー)
        Controls.Add(radクリア)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        Controls.Add(rad対角線同一)
        Controls.Add(rad縦横同一)
        Controls.Add(lbl長さ指定)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmSameSelect"
        SizeGripStyle = SizeGripStyle.Hide
        StartPosition = FormStartPosition.CenterParent
        Text = "同一長の指定"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lbl長さ指定 As Label
    Friend WithEvents rad縦横同一 As RadioButton
    Friend WithEvents rad対角線同一 As RadioButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnOK As Button
    Friend WithEvents btnキャンセル As Button
    Friend WithEvents radクリア As RadioButton
    Friend WithEvents lblエラー As Label
End Class
