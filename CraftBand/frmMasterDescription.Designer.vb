<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMasterDescription
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
        txt識別情報 = New Windows.Forms.TextBox()
        txt備考 = New Windows.Forms.TextBox()
        lbl識別情報 = New Windows.Forms.Label()
        lbl備考 = New Windows.Forms.Label()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        SuspendLayout()
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnキャンセル.DialogResult = Windows.Forms.DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(340, 125)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 44)
        btnキャンセル.TabIndex = 9
        btnキャンセル.Text = "キャンセル(&C)"
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(221, 125)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 44)
        btnOK.TabIndex = 8
        btnOK.Text = "OK(&O)"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' txt識別情報
        ' 
        txt識別情報.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txt識別情報.Location = New System.Drawing.Point(83, 12)
        txt識別情報.Multiline = True
        txt識別情報.Name = "txt識別情報"
        txt識別情報.Size = New System.Drawing.Size(368, 30)
        txt識別情報.TabIndex = 10
        ToolTip1.SetToolTip(txt識別情報, "設定データの識別情報")
        ' 
        ' txt備考
        ' 
        txt備考.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txt備考.Location = New System.Drawing.Point(83, 48)
        txt備考.Multiline = True
        txt備考.Name = "txt備考"
        txt備考.Size = New System.Drawing.Size(368, 66)
        txt備考.TabIndex = 11
        ' 
        ' lbl識別情報
        ' 
        lbl識別情報.AutoSize = True
        lbl識別情報.Location = New System.Drawing.Point(12, 12)
        lbl識別情報.Name = "lbl識別情報"
        lbl識別情報.Size = New System.Drawing.Size(65, 19)
        lbl識別情報.TabIndex = 12
        lbl識別情報.Text = "識別情報"
        ' 
        ' lbl備考
        ' 
        lbl備考.Anchor = Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left
        lbl備考.AutoSize = True
        lbl備考.Location = New System.Drawing.Point(12, 48)
        lbl備考.Name = "lbl備考"
        lbl備考.Size = New System.Drawing.Size(37, 19)
        lbl備考.TabIndex = 13
        lbl備考.Text = "備考"
        ' 
        ' frmMasterDescription
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(463, 177)
        Controls.Add(lbl備考)
        Controls.Add(lbl識別情報)
        Controls.Add(txt備考)
        Controls.Add(txt識別情報)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmMasterDescription"
        StartPosition = Windows.Forms.FormStartPosition.CenterParent
        Text = "frmMasterDescription"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnキャンセル As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents txt識別情報 As Windows.Forms.TextBox
    Friend WithEvents txt備考 As Windows.Forms.TextBox
    Friend WithEvents lbl識別情報 As Windows.Forms.Label
    Friend WithEvents lbl備考 As Windows.Forms.Label
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
End Class
