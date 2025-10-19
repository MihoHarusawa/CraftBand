<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrMemo
    Inherits System.Windows.Forms.UserControl

    'UserControl はコンポーネント一覧をクリーンアップするために dispose をオーバーライドします。
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
        Panel = New Windows.Forms.Panel()
        txt作成者 = New Windows.Forms.TextBox()
        txtタイトル = New Windows.Forms.TextBox()
        lbl作成者 = New Windows.Forms.Label()
        lblタイトル = New Windows.Forms.Label()
        lblメモ = New Windows.Forms.Label()
        txtメモ = New Windows.Forms.TextBox()
        ToolTip1 = New System.Windows.Forms.ToolTip(components)
        Panel.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel
        ' 
        Panel.Controls.Add(txt作成者)
        Panel.Controls.Add(txtタイトル)
        Panel.Controls.Add(lbl作成者)
        Panel.Controls.Add(lblタイトル)
        Panel.Controls.Add(lblメモ)
        Panel.Controls.Add(txtメモ)
        Panel.Location = New System.Drawing.Point(3, 3)
        Panel.Name = "Panel"
        Panel.Size = New System.Drawing.Size(713, 339)
        Panel.TabIndex = 0
        ' 
        ' txt作成者
        ' 
        txt作成者.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txt作成者.Location = New System.Drawing.Point(127, 77)
        txt作成者.Multiline = True
        txt作成者.Name = "txt作成者"
        txt作成者.Size = New System.Drawing.Size(563, 47)
        txt作成者.TabIndex = 9
        ToolTip1.SetToolTip(txt作成者, "作成者情報")
        ' 
        ' txtタイトル
        ' 
        txtタイトル.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txtタイトル.Location = New System.Drawing.Point(127, 16)
        txtタイトル.Multiline = True
        txtタイトル.Name = "txtタイトル"
        txtタイトル.Size = New System.Drawing.Size(563, 47)
        txtタイトル.TabIndex = 7
        ToolTip1.SetToolTip(txtタイトル, "タイトル情報")
        ' 
        ' lbl作成者
        ' 
        lbl作成者.AutoSize = True
        lbl作成者.Location = New System.Drawing.Point(25, 77)
        lbl作成者.Name = "lbl作成者"
        lbl作成者.Size = New System.Drawing.Size(51, 19)
        lbl作成者.TabIndex = 8
        lbl作成者.Text = "作成者"
        ' 
        ' lblタイトル
        ' 
        lblタイトル.AutoSize = True
        lblタイトル.Location = New System.Drawing.Point(25, 16)
        lblタイトル.Name = "lblタイトル"
        lblタイトル.Size = New System.Drawing.Size(52, 19)
        lblタイトル.TabIndex = 6
        lblタイトル.Text = "タイトル"
        ' 
        ' lblメモ
        ' 
        lblメモ.AutoSize = True
        lblメモ.Location = New System.Drawing.Point(25, 113)
        lblメモ.Name = "lblメモ"
        lblメモ.Size = New System.Drawing.Size(29, 19)
        lblメモ.TabIndex = 10
        lblメモ.Text = "メモ"
        ' 
        ' txtメモ
        ' 
        txtメモ.Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
        txtメモ.Location = New System.Drawing.Point(23, 139)
        txtメモ.Multiline = True
        txtメモ.Name = "txtメモ"
        txtメモ.Size = New System.Drawing.Size(667, 183)
        txtメモ.TabIndex = 11
        ToolTip1.SetToolTip(txtメモ, "自由に記述できます")
        ' 
        ' ctrMemo
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = Windows.Forms.AutoScaleMode.Font
        Controls.Add(Panel)
        Name = "ctrMemo"
        Size = New System.Drawing.Size(892, 438)
        Panel.ResumeLayout(False)
        Panel.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel As Windows.Forms.Panel
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents txt作成者 As Windows.Forms.TextBox
    Friend WithEvents txtタイトル As Windows.Forms.TextBox
    Friend WithEvents lbl作成者 As Windows.Forms.Label
    Friend WithEvents lblタイトル As Windows.Forms.Label
    Friend WithEvents lblメモ As Windows.Forms.Label
    Friend WithEvents txtメモ As Windows.Forms.TextBox

End Class
