Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBasics
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnキャンセル = New System.Windows.Forms.Button()
        Me.grp設定時の寸法単位 = New System.Windows.Forms.GroupBox()
        Me.rad設定時の寸法単位_in = New System.Windows.Forms.RadioButton()
        Me.rad設定時の寸法単位_cm = New System.Windows.Forms.RadioButton()
        Me.rad設定時の寸法単位_mm = New System.Windows.Forms.RadioButton()
        Me.lbl設定データの保存先 = New System.Windows.Forms.Label()
        Me.txt設定データの保存先 = New System.Windows.Forms.TextBox()
        Me.btn設定データの保存先 = New System.Windows.Forms.Button()
        Me.SaveFileDialogMasterTable = New System.Windows.Forms.SaveFileDialog()
        Me.grp設定時の寸法単位.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(252, 146)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK(&O)"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(371, 146)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 5
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.btnキャンセル.UseVisualStyleBackColor = True
        '
        'grp設定時の寸法単位
        '
        Me.grp設定時の寸法単位.Controls.Add(Me.rad設定時の寸法単位_in)
        Me.grp設定時の寸法単位.Controls.Add(Me.rad設定時の寸法単位_cm)
        Me.grp設定時の寸法単位.Controls.Add(Me.rad設定時の寸法単位_mm)
        Me.grp設定時の寸法単位.Location = New System.Drawing.Point(21, 22)
        Me.grp設定時の寸法単位.Name = "grp設定時の寸法単位"
        Me.grp設定時の寸法単位.Size = New System.Drawing.Size(415, 54)
        Me.grp設定時の寸法単位.TabIndex = 0
        Me.grp設定時の寸法単位.TabStop = False
        Me.grp設定時の寸法単位.Text = "設定時の寸法単位"
        '
        'rad設定時の寸法単位_in
        '
        Me.rad設定時の寸法単位_in.AutoSize = True
        Me.rad設定時の寸法単位_in.Location = New System.Drawing.Point(291, 20)
        Me.rad設定時の寸法単位_in.Name = "rad設定時の寸法単位_in"
        Me.rad設定時の寸法単位_in.Size = New System.Drawing.Size(42, 24)
        Me.rad設定時の寸法単位_in.TabIndex = 2
        Me.rad設定時の寸法単位_in.Text = "in"
        Me.rad設定時の寸法単位_in.UseVisualStyleBackColor = True
        '
        'rad設定時の寸法単位_cm
        '
        Me.rad設定時の寸法単位_cm.AutoSize = True
        Me.rad設定時の寸法単位_cm.Location = New System.Drawing.Point(215, 21)
        Me.rad設定時の寸法単位_cm.Name = "rad設定時の寸法単位_cm"
        Me.rad設定時の寸法単位_cm.Size = New System.Drawing.Size(50, 24)
        Me.rad設定時の寸法単位_cm.TabIndex = 1
        Me.rad設定時の寸法単位_cm.Text = "cm"
        Me.rad設定時の寸法単位_cm.UseVisualStyleBackColor = True
        '
        'rad設定時の寸法単位_mm
        '
        Me.rad設定時の寸法単位_mm.AutoSize = True
        Me.rad設定時の寸法単位_mm.Checked = True
        Me.rad設定時の寸法単位_mm.Location = New System.Drawing.Point(133, 21)
        Me.rad設定時の寸法単位_mm.Name = "rad設定時の寸法単位_mm"
        Me.rad設定時の寸法単位_mm.Size = New System.Drawing.Size(56, 24)
        Me.rad設定時の寸法単位_mm.TabIndex = 0
        Me.rad設定時の寸法単位_mm.TabStop = True
        Me.rad設定時の寸法単位_mm.Text = "mm"
        Me.rad設定時の寸法単位_mm.UseVisualStyleBackColor = True
        '
        'lbl設定データの保存先
        '
        Me.lbl設定データの保存先.AutoSize = True
        Me.lbl設定データの保存先.Location = New System.Drawing.Point(21, 105)
        Me.lbl設定データの保存先.Name = "lbl設定データの保存先"
        Me.lbl設定データの保存先.Size = New System.Drawing.Size(129, 20)
        Me.lbl設定データの保存先.TabIndex = 1
        Me.lbl設定データの保存先.Text = "設定データの保存先"
        '
        'txt設定データの保存先
        '
        Me.txt設定データの保存先.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt設定データの保存先.Location = New System.Drawing.Point(157, 102)
        Me.txt設定データの保存先.Name = "txt設定データの保存先"
        Me.txt設定データの保存先.Size = New System.Drawing.Size(291, 27)
        Me.txt設定データの保存先.TabIndex = 2
        '
        'btn設定データの保存先
        '
        Me.btn設定データの保存先.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn設定データの保存先.Location = New System.Drawing.Point(452, 101)
        Me.btn設定データの保存先.Name = "btn設定データの保存先"
        Me.btn設定データの保存先.Size = New System.Drawing.Size(30, 30)
        Me.btn設定データの保存先.TabIndex = 3
        Me.btn設定データの保存先.Text = "..."
        Me.btn設定データの保存先.UseVisualStyleBackColor = True
        '
        'SaveFileDialogMasterTable
        '
        Me.SaveFileDialogMasterTable.Filter = "設定ファイル (*.XML)|*.XML|全て (*.*)|*.*"
        Me.SaveFileDialogMasterTable.OverwritePrompt = False
        Me.SaveFileDialogMasterTable.Title = "設定データを保存するファイルを指定してください"
        '
        'frmBasics
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(501, 207)
        Me.Controls.Add(Me.btn設定データの保存先)
        Me.Controls.Add(Me.txt設定データの保存先)
        Me.Controls.Add(Me.lbl設定データの保存先)
        Me.Controls.Add(Me.grp設定時の寸法単位)
        Me.Controls.Add(Me.btnキャンセル)
        Me.Controls.Add(Me.btnOK)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(519, 254)
        Me.Name = "frmBasics"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "基本設定"
        Me.grp設定時の寸法単位.ResumeLayout(False)
        Me.grp設定時の寸法単位.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As Button
    Friend WithEvents btnキャンセル As Button
    Friend WithEvents grp設定時の寸法単位 As GroupBox
    Friend WithEvents rad設定時の寸法単位_in As RadioButton
    Friend WithEvents rad設定時の寸法単位_cm As RadioButton
    Friend WithEvents rad設定時の寸法単位_mm As RadioButton
    Friend WithEvents lbl設定データの保存先 As Label
    Friend WithEvents txt設定データの保存先 As TextBox
    Friend WithEvents btn設定データの保存先 As Button
    Friend WithEvents SaveFileDialogMasterTable As SaveFileDialog
End Class
