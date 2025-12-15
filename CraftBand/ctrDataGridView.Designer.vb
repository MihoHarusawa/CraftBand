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
        components = New ComponentModel.Container()
        ContextMenuStripDgv = New System.Windows.Forms.ContextMenuStrip(components)
        MenuItemCopy = New Windows.Forms.ToolStripMenuItem()
        MenuItemCut = New Windows.Forms.ToolStripMenuItem()
        MenuItemPaste = New Windows.Forms.ToolStripMenuItem()
        MenuItemDelete = New Windows.Forms.ToolStripMenuItem()
        MenuItemCancel = New Windows.Forms.ToolStripMenuItem()
        ToolStripFillSeparator = New Windows.Forms.ToolStripSeparator()
        MenuItemFill = New Windows.Forms.ToolStripMenuItem()
        ContextMenuStripDgv.SuspendLayout()
        CType(Me, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' ContextMenuStripDgv
        ' 
        ContextMenuStripDgv.ImageScalingSize = New System.Drawing.Size(20, 20)
        ContextMenuStripDgv.Items.AddRange(New Windows.Forms.ToolStripItem() {MenuItemCopy, MenuItemCut, MenuItemPaste, MenuItemDelete, MenuItemCancel, ToolStripFillSeparator, MenuItemFill})
        ContextMenuStripDgv.Name = "ContextMenuStrip"
        ContextMenuStripDgv.Size = New System.Drawing.Size(194, 154)
        ' 
        ' MenuItemCopy
        ' 
        MenuItemCopy.Name = "MenuItemCopy"
        MenuItemCopy.ShortcutKeys = Windows.Forms.Keys.Control Or Windows.Forms.Keys.C
        MenuItemCopy.Size = New System.Drawing.Size(193, 24)
        MenuItemCopy.Text = "コピー(&C)"
        ' 
        ' MenuItemCut
        ' 
        MenuItemCut.Name = "MenuItemCut"
        MenuItemCut.ShortcutKeys = Windows.Forms.Keys.Control Or Windows.Forms.Keys.X
        MenuItemCut.Size = New System.Drawing.Size(193, 24)
        MenuItemCut.Text = "切り取り(&T)"
        ' 
        ' MenuItemPaste
        ' 
        MenuItemPaste.Name = "MenuItemPaste"
        MenuItemPaste.ShortcutKeys = Windows.Forms.Keys.Control Or Windows.Forms.Keys.V
        MenuItemPaste.Size = New System.Drawing.Size(193, 24)
        MenuItemPaste.Text = "貼り付け(&P)"
        ' 
        ' MenuItemDelete
        ' 
        MenuItemDelete.Name = "MenuItemDelete"
        MenuItemDelete.ShortcutKeys = Windows.Forms.Keys.Delete
        MenuItemDelete.Size = New System.Drawing.Size(193, 24)
        MenuItemDelete.Text = "削除(&D)"
        ' 
        ' MenuItemCancel
        ' 
        MenuItemCancel.Name = "MenuItemCancel"
        MenuItemCancel.Size = New System.Drawing.Size(193, 24)
        MenuItemCancel.Text = "キャンセル(Esc)"
        ' 
        ' ToolStripFillSeparator
        ' 
        ToolStripFillSeparator.Name = "ToolStripFillSeparator"
        ToolStripFillSeparator.Size = New System.Drawing.Size(190, 6)
        ' 
        ' MenuItemFill
        ' 
        MenuItemFill.Name = "MenuItemFill"
        MenuItemFill.ShowShortcutKeys = False
        MenuItemFill.Size = New System.Drawing.Size(193, 24)
        MenuItemFill.Text = "等差補完"
        MenuItemFill.ToolTipText = "最初の2点から等差数列をセット"
        MenuItemFill.Visible = False
        ' 
        ' ctrDataGridView
        ' 
        ClipboardCopyMode = Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        ContextMenuStrip = ContextMenuStripDgv
        RowTemplate.Height = 28
        ContextMenuStripDgv.ResumeLayout(False)
        CType(Me, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents ContextMenuStripDgv As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuItemCut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemCancel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemFill As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripFillSeparator As Windows.Forms.ToolStripSeparator

End Class
