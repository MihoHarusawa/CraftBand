<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrDataGridView
    Inherits System.Windows.Forms.DataGridView

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
        Me.components = New System.ComponentModel.Container()
        'Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ContextMenuStripDgv = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuItemCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemCancel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripDgv.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStripDgv
        '
        Me.ContextMenuStripDgv.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripDgv.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItemCopy, Me.MenuItemCut, Me.MenuItemPaste, Me.MenuItemDelete, Me.MenuItemCancel})
        Me.ContextMenuStripDgv.Name = "ContextMenuStrip"
        Me.ContextMenuStripDgv.Size = New System.Drawing.Size(211, 152)
        '
        'MenuItemCut
        '
        Me.MenuItemCut.Name = "MenuItemCut"
        Me.MenuItemCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.MenuItemCut.Size = New System.Drawing.Size(210, 24)
        Me.MenuItemCut.Text = "切り取り(&T)"
        '
        'MenuItemCopy
        '
        Me.MenuItemCopy.Name = "MenuItemCopy"
        Me.MenuItemCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.MenuItemCopy.Size = New System.Drawing.Size(210, 24)
        Me.MenuItemCopy.Text = "コピー(&C)"
        '
        'MenuItemPaste
        '
        Me.MenuItemPaste.Name = "MenuItemPaste"
        Me.MenuItemPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.MenuItemPaste.Size = New System.Drawing.Size(210, 24)
        Me.MenuItemPaste.Text = "貼り付け(&P)"
        '
        'MenuItemDelete
        '
        Me.MenuItemDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar
        Me.MenuItemDelete.Name = "MenuItemDelete"
        Me.MenuItemDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.MenuItemDelete.Size = New System.Drawing.Size(210, 24)
        Me.MenuItemDelete.Text = "削除(&D)"
        '
        'MenuItemCancel
        '
        Me.MenuItemCancel.Name = "MenuItemCancel"
        Me.MenuItemCancel.Size = New System.Drawing.Size(210, 24)
        Me.MenuItemCancel.Text = "キャンセル(Esc)"

        Me.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.ContextMenuStrip = Me.ContextMenuStripDgv
        Me.ContextMenuStripDgv.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ContextMenuStripDgv As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuItemCut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemCancel As System.Windows.Forms.ToolStripMenuItem

End Class
