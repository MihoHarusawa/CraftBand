<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        btn終了 = New Button()
        ToolTip1 = New ToolTip(components)
        btn実行ファイルフォルダ = New Button()
        txt実行ファイルフォルダ = New TextBox()
        lbl実行ファイルフォルダ = New Label()
        chkアプリを起動したら閉じる = New CheckBox()
        chk画面の表示を残す = New CheckBox()
        FolderBrowserDialog1 = New FolderBrowserDialog()
        chk編集対象なしで起動する = New CheckBox()
        SuspendLayout()
        ' 
        ' btn終了
        ' 
        btn終了.DialogResult = DialogResult.OK
        btn終了.Location = New Point(376, 137)
        btn終了.Name = "btn終了"
        btn終了.Size = New Size(111, 44)
        btn終了.TabIndex = 6
        btn終了.Text = "OK(&O)"
        btn終了.UseVisualStyleBackColor = True
        ' 
        ' btn実行ファイルフォルダ
        ' 
        btn実行ファイルフォルダ.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btn実行ファイルフォルダ.Location = New Point(457, 101)
        btn実行ファイルフォルダ.Name = "btn実行ファイルフォルダ"
        btn実行ファイルフォルダ.Size = New Size(30, 28)
        btn実行ファイルフォルダ.TabIndex = 5
        btn実行ファイルフォルダ.Text = "..."
        ToolTip1.SetToolTip(btn実行ファイルフォルダ, "フォルダダイアログを開きます")
        btn実行ファイルフォルダ.UseVisualStyleBackColor = True
        ' 
        ' txt実行ファイルフォルダ
        ' 
        txt実行ファイルフォルダ.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txt実行ファイルフォルダ.Location = New Point(137, 103)
        txt実行ファイルフォルダ.Name = "txt実行ファイルフォルダ"
        txt実行ファイルフォルダ.Size = New Size(314, 26)
        txt実行ファイルフォルダ.TabIndex = 4
        ToolTip1.SetToolTip(txt実行ファイルフォルダ, "このプログラムとは別の場所にあるアプリを起動したい場合のみ指定してください。" & vbCrLf & "通常は空にしておいてください。" & vbCrLf)
        ' 
        ' lbl実行ファイルフォルダ
        ' 
        lbl実行ファイルフォルダ.AutoSize = True
        lbl実行ファイルフォルダ.Location = New Point(10, 106)
        lbl実行ファイルフォルダ.Name = "lbl実行ファイルフォルダ"
        lbl実行ファイルフォルダ.Size = New Size(121, 19)
        lbl実行ファイルフォルダ.TabIndex = 3
        lbl実行ファイルフォルダ.Text = "実行ファイルフォルダ"
        ToolTip1.SetToolTip(lbl実行ファイルフォルダ, "このプログラムとは別の場所にあるアプリを起動したい場合のみ指定してください。" & vbCrLf & "通常は空にしておいてください。")
        ' 
        ' chkアプリを起動したら閉じる
        ' 
        chkアプリを起動したら閉じる.AutoSize = True
        chkアプリを起動したら閉じる.Location = New Point(12, 12)
        chkアプリを起動したら閉じる.Name = "chkアプリを起動したら閉じる"
        chkアプリを起動したら閉じる.Size = New Size(164, 23)
        chkアプリを起動したら閉じる.TabIndex = 0
        chkアプリを起動したら閉じる.Text = "アプリを起動したら閉じる"
        ToolTip1.SetToolTip(chkアプリを起動したら閉じる, "指定のアプリを起動したら、このプログラムは終了する" & vbCrLf & "チェックオフの場合は、終了しない")
        chkアプリを起動したら閉じる.UseVisualStyleBackColor = True
        ' 
        ' chk画面の表示を残す
        ' 
        chk画面の表示を残す.AutoSize = True
        chk画面の表示を残す.Location = New Point(12, 66)
        chk画面の表示を残す.Name = "chk画面の表示を残す"
        chk画面の表示を残す.Size = New Size(131, 23)
        chk画面の表示を残す.TabIndex = 2
        chk画面の表示を残す.Text = "画面の表示を残す"
        ToolTip1.SetToolTip(chk画面の表示を残す, "[ファイル]メニューの[クリア]をしない限り情報が残る" & vbCrLf & "チェックオフの場合は、開始時にクリアします")
        chk画面の表示を残す.UseVisualStyleBackColor = True
        ' 
        ' FolderBrowserDialog1
        ' 
        FolderBrowserDialog1.Description = "一連の実行ファイルがあるフォルダを指定してください"
        ' 
        ' chk編集対象なしで起動する
        ' 
        chk編集対象なしで起動する.AutoSize = True
        chk編集対象なしで起動する.Location = New Point(12, 39)
        chk編集対象なしで起動する.Name = "chk編集対象なしで起動する"
        chk編集対象なしで起動する.Size = New Size(167, 23)
        chk編集対象なしで起動する.TabIndex = 1
        chk編集対象なしで起動する.Text = "編集対象なしで起動する"
        ToolTip1.SetToolTip(chk編集対象なしで起動する, "アプリを、編集対象データがない状態で起動します" & vbCrLf & "チェックオフの場合は、先に開いていたデータが開かれます")
        chk編集対象なしで起動する.UseVisualStyleBackColor = True
        ' 
        ' frmSettings
        ' 
        AutoScaleDimensions = New SizeF(8F, 19F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(498, 193)
        Controls.Add(chk編集対象なしで起動する)
        Controls.Add(chk画面の表示を残す)
        Controls.Add(chkアプリを起動したら閉じる)
        Controls.Add(btn実行ファイルフォルダ)
        Controls.Add(txt実行ファイルフォルダ)
        Controls.Add(lbl実行ファイルフォルダ)
        Controls.Add(btn終了)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmSettings"
        StartPosition = FormStartPosition.CenterParent
        Text = "設定"
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents btn終了 As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btn実行ファイルフォルダ As Button
    Friend WithEvents txt実行ファイルフォルダ As TextBox
    Friend WithEvents lbl実行ファイルフォルダ As Label
    Friend WithEvents chkアプリを起動したら閉じる As CheckBox
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents chk画面の表示を残す As CheckBox
    Friend WithEvents chk編集対象なしで起動する As CheckBox
End Class
