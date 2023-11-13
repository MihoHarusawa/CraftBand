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
        Me.components = New System.ComponentModel.Container()
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnImport = New System.Windows.Forms.Button()
        Me.OpenFileDialogImport = New System.Windows.Forms.OpenFileDialog()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.SaveFileDialogExport = New System.Windows.Forms.SaveFileDialog()
        Me.grp設定時の寸法単位.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(259, 146)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 46)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "OK(&O)"
        Me.ToolTip1.SetToolTip(Me.btnOK, "変更を保存して終了します")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnキャンセル
        '
        Me.btnキャンセル.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnキャンセル.Location = New System.Drawing.Point(378, 146)
        Me.btnキャンセル.Name = "btnキャンセル"
        Me.btnキャンセル.Size = New System.Drawing.Size(111, 46)
        Me.btnキャンセル.TabIndex = 7
        Me.btnキャンセル.Text = "キャンセル(&C)"
        Me.ToolTip1.SetToolTip(Me.btnキャンセル, "変更を保存せずに終了します")
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
        Me.ToolTip1.SetToolTip(Me.grp設定時の寸法単位, "長さを設定するときの単位")
        '
        'rad設定時の寸法単位_in
        '
        Me.rad設定時の寸法単位_in.AutoSize = True
        Me.rad設定時の寸法単位_in.Location = New System.Drawing.Point(291, 20)
        Me.rad設定時の寸法単位_in.Name = "rad設定時の寸法単位_in"
        Me.rad設定時の寸法単位_in.Size = New System.Drawing.Size(42, 24)
        Me.rad設定時の寸法単位_in.TabIndex = 2
        Me.rad設定時の寸法単位_in.Text = "in"
        Me.ToolTip1.SetToolTip(Me.rad設定時の寸法単位_in, "インチ単位")
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
        Me.ToolTip1.SetToolTip(Me.rad設定時の寸法単位_cm, "センチメートル単位")
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
        Me.ToolTip1.SetToolTip(Me.rad設定時の寸法単位_mm, "ミリメートル単位")
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
        Me.txt設定データの保存先.Size = New System.Drawing.Size(298, 27)
        Me.txt設定データの保存先.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.txt設定データの保存先, "設定データの保存先ファイル名")
        '
        'btn設定データの保存先
        '
        Me.btn設定データの保存先.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn設定データの保存先.Location = New System.Drawing.Point(459, 101)
        Me.btn設定データの保存先.Name = "btn設定データの保存先"
        Me.btn設定データの保存先.Size = New System.Drawing.Size(30, 30)
        Me.btn設定データの保存先.TabIndex = 3
        Me.btn設定データの保存先.Text = "..."
        Me.ToolTip1.SetToolTip(Me.btn設定データの保存先, "ファイルダイアログを開きます")
        Me.btn設定データの保存先.UseVisualStyleBackColor = True
        '
        'SaveFileDialogMasterTable
        '
        Me.SaveFileDialogMasterTable.Filter = "設定ファイル (*.XML)|*.XML|全て (*.*)|*.*"
        Me.SaveFileDialogMasterTable.OverwritePrompt = False
        Me.SaveFileDialogMasterTable.Title = "設定データを保存するファイルを指定してください"
        '
        'btnImport
        '
        Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImport.Location = New System.Drawing.Point(21, 146)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(111, 46)
        Me.btnImport.TabIndex = 4
        Me.btnImport.Text = "インポート(&I)"
        Me.ToolTip1.SetToolTip(Me.btnImport, "別の設定データファイルを読み取ります")
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'OpenFileDialogImport
        '
        Me.OpenFileDialogImport.DefaultExt = "XML"
        Me.OpenFileDialogImport.Filter = "設定ファイル (*.XML)|*.XML|全て (*.*)|*.*"
        Me.OpenFileDialogImport.Title = "インポートしたい設定データファイルを指定してください"
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Location = New System.Drawing.Point(140, 146)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(111, 46)
        Me.btnExport.TabIndex = 5
        Me.btnExport.Text = "エクスポート(&E)"
        Me.ToolTip1.SetToolTip(Me.btnExport, "現データで参照している設定を別のファイルに書き出します")
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "設定ファイル (*.XML)|*.XML|全て (*.*)|*.*"
        Me.SaveFileDialog1.OverwritePrompt = False
        Me.SaveFileDialog1.Title = "設定データを保存するファイルを指定してください"
        '
        'SaveFileDialogExport
        '
        Me.SaveFileDialogExport.Filter = "設定ファイル (*.XML)|*.XML|全て (*.*)|*.*"
        Me.SaveFileDialogExport.Title = "参照している設定データを書き出すファイルを指定してください"
        '
        'frmBasics
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(508, 207)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnImport)
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
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnImport As Button
    Friend WithEvents OpenFileDialogImport As OpenFileDialog
    Friend WithEvents btnExport As Button
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents SaveFileDialogExport As SaveFileDialog
End Class
