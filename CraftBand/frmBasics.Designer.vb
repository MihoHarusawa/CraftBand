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
        components = New System.ComponentModel.Container()
        btnOK = New Button()
        btnキャンセル = New Button()
        grp設定時の寸法単位 = New GroupBox()
        rad設定時の寸法単位_in = New RadioButton()
        rad設定時の寸法単位_cm = New RadioButton()
        rad設定時の寸法単位_mm = New RadioButton()
        lbl設定データの保存先 = New Label()
        txt設定データの保存先 = New TextBox()
        btn設定データの保存先 = New Button()
        SaveFileDialogMasterTable = New SaveFileDialog()
        ToolTip1 = New ToolTip(components)
        btnImport = New Button()
        btnExport = New Button()
        OpenFileDialogImport = New OpenFileDialog()
        SaveFileDialog1 = New SaveFileDialog()
        SaveFileDialogExport = New SaveFileDialog()
        grp設定時の寸法単位.SuspendLayout()
        SuspendLayout()
        ' 
        ' btnOK
        ' 
        btnOK.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnOK.Location = New System.Drawing.Point(263, 149)
        btnOK.Name = "btnOK"
        btnOK.Size = New System.Drawing.Size(111, 44)
        btnOK.TabIndex = 6
        btnOK.Text = "OK(&O)"
        ToolTip1.SetToolTip(btnOK, "変更を保存して終了します")
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnキャンセル
        ' 
        btnキャンセル.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnキャンセル.DialogResult = DialogResult.Cancel
        btnキャンセル.Location = New System.Drawing.Point(382, 149)
        btnキャンセル.Name = "btnキャンセル"
        btnキャンセル.Size = New System.Drawing.Size(111, 44)
        btnキャンセル.TabIndex = 7
        btnキャンセル.Text = "キャンセル(&C)"
        ToolTip1.SetToolTip(btnキャンセル, "変更を保存せずに終了します")
        btnキャンセル.UseVisualStyleBackColor = True
        ' 
        ' grp設定時の寸法単位
        ' 
        grp設定時の寸法単位.Controls.Add(rad設定時の寸法単位_in)
        grp設定時の寸法単位.Controls.Add(rad設定時の寸法単位_cm)
        grp設定時の寸法単位.Controls.Add(rad設定時の寸法単位_mm)
        grp設定時の寸法単位.Location = New System.Drawing.Point(21, 21)
        grp設定時の寸法単位.Name = "grp設定時の寸法単位"
        grp設定時の寸法単位.Size = New System.Drawing.Size(415, 51)
        grp設定時の寸法単位.TabIndex = 0
        grp設定時の寸法単位.TabStop = False
        grp設定時の寸法単位.Text = "設定時の寸法単位"
        ToolTip1.SetToolTip(grp設定時の寸法単位, "長さを設定するときの単位")
        ' 
        ' rad設定時の寸法単位_in
        ' 
        rad設定時の寸法単位_in.AutoSize = True
        rad設定時の寸法単位_in.Location = New System.Drawing.Point(291, 19)
        rad設定時の寸法単位_in.Name = "rad設定時の寸法単位_in"
        rad設定時の寸法単位_in.Size = New System.Drawing.Size(38, 23)
        rad設定時の寸法単位_in.TabIndex = 2
        rad設定時の寸法単位_in.Text = "in"
        ToolTip1.SetToolTip(rad設定時の寸法単位_in, "インチ単位")
        rad設定時の寸法単位_in.UseVisualStyleBackColor = True
        ' 
        ' rad設定時の寸法単位_cm
        ' 
        rad設定時の寸法単位_cm.AutoSize = True
        rad設定時の寸法単位_cm.Location = New System.Drawing.Point(215, 20)
        rad設定時の寸法単位_cm.Name = "rad設定時の寸法単位_cm"
        rad設定時の寸法単位_cm.Size = New System.Drawing.Size(45, 23)
        rad設定時の寸法単位_cm.TabIndex = 1
        rad設定時の寸法単位_cm.Text = "cm"
        ToolTip1.SetToolTip(rad設定時の寸法単位_cm, "センチメートル単位")
        rad設定時の寸法単位_cm.UseVisualStyleBackColor = True
        ' 
        ' rad設定時の寸法単位_mm
        ' 
        rad設定時の寸法単位_mm.AutoSize = True
        rad設定時の寸法単位_mm.Checked = True
        rad設定時の寸法単位_mm.Location = New System.Drawing.Point(133, 20)
        rad設定時の寸法単位_mm.Name = "rad設定時の寸法単位_mm"
        rad設定時の寸法単位_mm.Size = New System.Drawing.Size(51, 23)
        rad設定時の寸法単位_mm.TabIndex = 0
        rad設定時の寸法単位_mm.TabStop = True
        rad設定時の寸法単位_mm.Text = "mm"
        ToolTip1.SetToolTip(rad設定時の寸法単位_mm, "ミリメートル単位")
        rad設定時の寸法単位_mm.UseVisualStyleBackColor = True
        ' 
        ' lbl設定データの保存先
        ' 
        lbl設定データの保存先.AutoSize = True
        lbl設定データの保存先.Location = New System.Drawing.Point(21, 100)
        lbl設定データの保存先.Name = "lbl設定データの保存先"
        lbl設定データの保存先.Size = New System.Drawing.Size(121, 19)
        lbl設定データの保存先.TabIndex = 1
        lbl設定データの保存先.Text = "設定データの保存先"
        ' 
        ' txt設定データの保存先
        ' 
        txt設定データの保存先.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txt設定データの保存先.Location = New System.Drawing.Point(157, 97)
        txt設定データの保存先.Name = "txt設定データの保存先"
        txt設定データの保存先.Size = New System.Drawing.Size(298, 26)
        txt設定データの保存先.TabIndex = 2
        ToolTip1.SetToolTip(txt設定データの保存先, "設定データの保存先ファイル名")
        ' 
        ' btn設定データの保存先
        ' 
        btn設定データの保存先.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btn設定データの保存先.Location = New System.Drawing.Point(459, 96)
        btn設定データの保存先.Name = "btn設定データの保存先"
        btn設定データの保存先.Size = New System.Drawing.Size(30, 28)
        btn設定データの保存先.TabIndex = 3
        btn設定データの保存先.Text = "..."
        ToolTip1.SetToolTip(btn設定データの保存先, "ファイルダイアログを開きます")
        btn設定データの保存先.UseVisualStyleBackColor = True
        ' 
        ' SaveFileDialogMasterTable
        ' 
        SaveFileDialogMasterTable.Filter = "設定ファイル (*.CBMESH;*.XML)|*.CBMESH;*.XML|全て (*.*)|*.*"
        SaveFileDialogMasterTable.OverwritePrompt = False
        SaveFileDialogMasterTable.Title = "設定データを保存するファイルを指定してください"
        ' 
        ' btnImport
        ' 
        btnImport.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnImport.Location = New System.Drawing.Point(15, 149)
        btnImport.Name = "btnImport"
        btnImport.Size = New System.Drawing.Size(111, 44)
        btnImport.TabIndex = 4
        btnImport.Text = "インポート(&I)"
        ToolTip1.SetToolTip(btnImport, "別の設定データファイルを読み取ります")
        btnImport.UseVisualStyleBackColor = True
        ' 
        ' btnExport
        ' 
        btnExport.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnExport.Location = New System.Drawing.Point(134, 149)
        btnExport.Name = "btnExport"
        btnExport.Size = New System.Drawing.Size(111, 44)
        btnExport.TabIndex = 5
        btnExport.Text = "エクスポート(&E)"
        ToolTip1.SetToolTip(btnExport, "現データで参照している設定を別のファイルに書き出します")
        btnExport.UseVisualStyleBackColor = True
        ' 
        ' OpenFileDialogImport
        ' 
        OpenFileDialogImport.DefaultExt = "CBMESH"
        OpenFileDialogImport.Filter = "設定ファイル (*.CBMESH;*.XML)|*.CBMESH;*.XML|全て (*.*)|*.*"
        OpenFileDialogImport.Title = "インポートしたい設定データファイルを指定してください"
        ' 
        ' SaveFileDialog1
        ' 
        SaveFileDialog1.DefaultExt = "CBMESH"
        SaveFileDialog1.Filter = "設定ファイル (*.CBMESH)|*.CBMESH|旧ファイル (*.XML)|*.XML|全て (*.*)|*.*"
        SaveFileDialog1.OverwritePrompt = False
        SaveFileDialog1.Title = "設定データを保存するファイルを指定してください"
        ' 
        ' SaveFileDialogExport
        ' 
        SaveFileDialogExport.DefaultExt = "CBMESH"
        SaveFileDialogExport.Filter = "設定ファイル (*.CBMESH)|*.CBMESH|旧ファイル (*.XML)|*.XML|全て (*.*)|*.*"
        SaveFileDialogExport.Title = "参照している設定データを書き出すファイルを指定してください"
        ' 
        ' frmBasics
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(8F, 19F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(508, 204)
        Controls.Add(btnExport)
        Controls.Add(btnImport)
        Controls.Add(btn設定データの保存先)
        Controls.Add(txt設定データの保存先)
        Controls.Add(lbl設定データの保存先)
        Controls.Add(grp設定時の寸法単位)
        Controls.Add(btnキャンセル)
        Controls.Add(btnOK)
        MaximizeBox = False
        MinimizeBox = False
        MinimumSize = New System.Drawing.Size(519, 243)
        Name = "frmBasics"
        StartPosition = FormStartPosition.CenterParent
        Text = "基本設定"
        grp設定時の寸法単位.ResumeLayout(False)
        grp設定時の寸法単位.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

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
