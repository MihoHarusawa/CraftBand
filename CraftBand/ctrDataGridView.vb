Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

'セルチェック時のコールバック
Public Delegate Function CallBackCheckCell(ByVal DataPropertyName As String, ByVal value As Object) As String

Public Class ctrDataGridView
    Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

    End Sub

#Region "動作指定"

    Const cRowHeightIdxOne As Integer = 34
    Const cRowHeightIdxSub As Integer = 20

    <Flags()>
    Enum enumAction
        _None = 0
        _Modify_i何本幅 = &H1
        _Modify_s色 = &H2
        _BackColorReadOnlyYellow = &H4
        _RowHeight_iひも番号 = &H8
        _RowHeight_iひも番号_BackColor = &H10
        _CheckBoxGray_iひも番号 = &H20
    End Enum

    Class CDataGridViewProfile
        '動作指定
        Friend Actions As enumAction = enumAction._None
        'データチェック用
        Friend GridDataRow As clsDataRow = Nothing
        Friend adr_CheckCell As CallBackCheckCell = Nothing
        'メッセージ表示用
        Public FormCaption As String

        Sub New(ByVal table As DataTable, ByVal adr As CallBackCheckCell, ByVal action As enumAction)
            If table IsNot Nothing Then
                GridDataRow = New clsDataRow(table.NewRow)
            End If
            adr_CheckCell = adr
            Actions = action
        End Sub
    End Class
    Dim _Profile As CDataGridViewProfile = Nothing
    Dim _ColIndexひも番号 As Integer = -1

    'グリッドの動作を指定する
    Public Sub SetProfile(ByVal prof As CDataGridViewProfile)
        _Profile = prof

        If _Profile.Actions.HasFlag(enumAction._RowHeight_iひも番号) OrElse
            _Profile.Actions.HasFlag(enumAction._RowHeight_iひも番号_BackColor) Then
            Me.RowTemplate.Height = cRowHeightIdxOne
        End If
        For Each col As DataGridViewColumn In Me.Columns
            If col.DataPropertyName = "f_iひも番号" Then
                _ColIndexひも番号 = col.Index
                Exit For
            End If
        Next
    End Sub

    Private Function messageTitle() As String
        If _Profile IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(_Profile.FormCaption) Then
            Return _Profile.FormCaption
        End If
        'グリッドの警告
        Return My.Resources.ErrGridTitle
    End Function

    Private ReadOnly Property MyGridDataRow As clsDataRow
        Get
            If _Profile Is Nothing OrElse _Profile.GridDataRow Is Nothing OrElse Not _Profile.GridDataRow.IsValid Then
                Return Nothing
            End If
            Return _Profile.GridDataRow
        End Get
    End Property
#End Region

#Region "イベント処理"
    'セルのチェック
    Private Sub DataGridView_CellValidating(sender As System.Object, e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles MyBase.CellValidating
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        '新しい行と入力前セルは除外
        If e.RowIndex = Me.NewRowIndex OrElse Not Me.IsCurrentCellDirty Then
            Exit Sub
        End If

        'Debug.Print("CellValidating row={0} col={1}", e.RowIndex, e.ColumnIndex)
        If _Profile Is Nothing OrElse _Profile.adr_CheckCell Is Nothing Then
            Exit Sub
        End If

        Dim column As DataGridViewColumn = Me.Columns(e.ColumnIndex)
        If String.IsNullOrEmpty(column.DataPropertyName) Then
            Exit Sub
        End If

        Dim err As String = _Profile.adr_CheckCell.Invoke(column.DataPropertyName, e.FormattedValue)
        If err IsNot Nothing Then
            '{0}行目<{1}> {2}
            Dim msg As String = String.Format(My.Resources.ErrGridLine, e.RowIndex + 1, column.HeaderText, err)
            MessageBox.Show(msg, messageTitle(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.CancelEdit()
            e.Cancel = True
        End If

    End Sub

    '行のチェック(複数セルのペースト・DeleteではCellValidatingされないため)
    Private Sub DataGridView_RowValidating(sender As System.Object, e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles MyBase.RowValidating
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If
        '新しい行と入力前行は除外
        If e.RowIndex = Me.NewRowIndex OrElse Not Me.IsCurrentRowDirty Then
            Exit Sub
        End If

        'Debug.Print("RowValidating row={0} col={1}", e.RowIndex, e.ColumnIndex)
        If _Profile Is Nothing OrElse _Profile.adr_CheckCell Is Nothing Then
            Exit Sub
        End If

        For i As Integer = 0 To Me.Columns.Count - 1
            Dim cell As DataGridViewCell = Me.Rows(e.RowIndex).Cells(i)
            If cell.ReadOnly OrElse Not cell.Visible Then
                Continue For
            End If
            Dim column As DataGridViewColumn = Me.Columns(i)
            Dim err As String = _Profile.adr_CheckCell.Invoke(column.DataPropertyName, cell.FormattedValue)
            If err IsNot Nothing Then
                '{0}行目<{1}> {2}
                Dim msg As String = String.Format(My.Resources.ErrGridLine, e.RowIndex + 1, column.HeaderText, err)
                MessageBox.Show(msg, messageTitle(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                e.Cancel = True
            End If
        Next
    End Sub

    'データエラー
    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles MyBase.DataError
        If e.Exception Is Nothing Then
            Exit Sub
        End If

        If _Profile IsNot Nothing Then
            If _Profile.Actions.HasFlag(enumAction._Modify_i何本幅) Then
                If Me.Columns(e.ColumnIndex).DataPropertyName = "f_i何本幅" Then
                    '現バンドの本幅数以上の値がセットされているレコードを修正(対象バンドの変更？)
                    Dim lane As Integer = Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    If lane < 1 Then
                        Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 1
                        '本幅数の修正
                        g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_LaneModified", lane, 1)
                    ElseIf g_clsSelectBasics.p_i本幅 < lane Then
                        Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = g_clsSelectBasics.p_i本幅
                        '本幅数の修正
                        g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_LaneModified", lane, g_clsSelectBasics.p_i本幅)
                    End If
                    e.ThrowException = False
                    Exit Sub
                End If
            End If

            If _Profile.Actions.HasFlag(enumAction._Modify_s色) Then
                If Me.Columns(e.ColumnIndex).DataPropertyName = "f_s色" Then
                    '現バンドにない色がセットされているレコードを修正(対象バンドの変更？)
                    g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_ColorModified", Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
                    Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = ""
                    e.ThrowException = False
                    Exit Sub
                End If
            End If
        End If


        '受け付けない(要・再入力)
        '{0}行目<{1}> データエラー{2}{3}
        Dim msg As String = String.Format(My.Resources.ErrGridData, e.RowIndex + 1, Me.Columns(e.ColumnIndex).HeaderText,
                                 vbCrLf, e.Exception.Message)
        MessageBox.Show(msg, messageTitle(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        e.Cancel = True
    End Sub

    '書式設定
    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles MyBase.CellFormatting
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If
        If _Profile IsNot Nothing Then
            'R/O背景色
            If _Profile.Actions.HasFlag(enumAction._BackColorReadOnlyYellow) Then
                If Me.Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly Then
                    e.CellStyle.BackColor = Color.LightYellow
                End If
            End If
            'ひも番号の行の高さと背景色
            If _Profile.Actions.HasFlag(enumAction._RowHeight_iひも番号_BackColor) AndAlso 0 <= _ColIndexひも番号 Then
                If Me.Rows(e.RowIndex).Cells(_ColIndexひも番号).Value = 1 Then
                    If e.ColumnIndex = _ColIndexひも番号 Then
                        Me.Rows(e.RowIndex).HeaderCell.Value = "*"
                    End If
                Else
                    e.CellStyle.BackColor = Color.Gainsboro '全col
                    Me.Rows(e.RowIndex).Height = cRowHeightIdxSub '1回でよいが全部やる
                End If
            End If
            'ひも番号の行の高さ
            If _Profile.Actions.HasFlag(enumAction._RowHeight_iひも番号) AndAlso 0 <= _ColIndexひも番号 Then
                If Me.Rows(e.RowIndex).Cells(_ColIndexひも番号).Value = 1 Or
                Me.Rows(e.RowIndex).Cells(_ColIndexひも番号).Value = 0 Then
                    If e.ColumnIndex = _ColIndexひも番号 Then
                        Me.Rows(e.RowIndex).HeaderCell.Value = "+"
                    End If
                Else
                    Me.Rows(e.RowIndex).Height = cRowHeightIdxSub
                End If
            End If
        End If
    End Sub

    Private Sub DataGridView_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles MyBase.CellPainting
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        If _Profile.Actions.HasFlag(enumAction._CheckBoxGray_iひも番号) AndAlso 0 <= _ColIndexひも番号 Then
            '処理対象チェックボックス列
            If (TypeOf Me.Columns(e.ColumnIndex) IsNot DataGridViewCheckBoxColumn) OrElse
              Not {"f_b縁専用区分", "f_b底使用区分", "f_bCraftBandMesh", "f_bCraftBandSquare45", "f_bCraftBandKnot", "f_bCraftBandSquare", "f_bCraftBandHexagon"}.Contains(Me.Columns(e.ColumnIndex).DataPropertyName) Then
                Exit Sub
            End If

            'その行のひも番号
            Dim idx As Integer = Me.Rows(e.RowIndex).Cells(_ColIndexひも番号).Value
            If 1 < idx Then
                e.PaintBackground(e.CellBounds, False)
                Dim rect As Rectangle = e.CellBounds
                rect.Width = 15
                rect.Height = 15
                rect.Offset((e.CellBounds.Width - 16) \ 2, rect.Height \ 2 - 5)
                ControlPaint.DrawFocusRectangle(e.Graphics, rect)

                e.Handled = True
            End If
        End If
    End Sub


    'ワンクリック編集　※CellEnterは何もしない(コピペしにくくなるので)
    Private Sub DataGridView_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles MyBase.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Return
        End If
        Dim row As DataGridViewRow = Me.Rows(e.RowIndex)
        If Me.Rows(e.RowIndex).ReadOnly Then
            Return
        End If
        Dim cell As DataGridViewCell = row.Cells(e.ColumnIndex)
        If cell.ReadOnly Then
            Return
        End If

        If TypeOf cell Is DataGridViewTextBoxCell Then
            Me.BeginEdit(True)
            'SendKeys.Send("{F2}")
        Else
            Try
                Dim comboBox = TryCast(Me.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewComboBoxCell)
                If comboBox Is Nothing Then
                    Return
                End If
                Dim orgValue = comboBox.Value
                If String.IsNullOrWhiteSpace(orgValue) AndAlso comboBox.Items.Count > 0 Then
                    SendKeys.Send("{F4}")
                End If

            Catch ex As Exception
                'comboBox = Nothing?
            End Try
        End If
    End Sub
#End Region

#Region "コピー・ペースト"
    '基本、行単位の個別レコードなので、貼り付け対象は選択された行・セルのみとする

    Private Sub ContextMenuStrip_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStripDgv.Opening
        Me.MenuItemCopy.Enabled = RowOrCellSelected(Me)
        Me.MenuItemCut.Enabled = RowOrCellSelected(Me)
        Me.MenuItemPaste.Enabled = IsPastable(Me)
        Me.MenuItemDelete.Enabled = HasWritableCell(Me)
        Me.MenuItemCancel.Enabled = True
    End Sub

    Private Sub MenuItemCopy_Click(sender As Object, e As EventArgs) Handles MenuItemCopy.Click
        SetToClipBoard(Me)
        'MyBase.ClearSelection()
    End Sub

    Private Sub MenuItemCut_Click(sender As Object, e As EventArgs) Handles MenuItemCut.Click
        SetToClipBoard(Me)
        SetDefaultWritableCell(Me, MyGridDataRow)
        '
        If Me.DataSource IsNot Nothing AndAlso Me.DataSource.Current IsNot Nothing AndAlso Me.DataSource.Current.row IsNot Nothing Then
            Me.DataSource.Current.row.acceptchanges()
        End If
    End Sub

    Private Sub MenuItemPaste_Click(sender As Object, e As EventArgs) Handles MenuItemPaste.Click
        If Me.DataSource.Current Is Nothing OrElse Me.DataSource.Current.IsNew Then
            Exit Sub
        End If
        DoPaste(Me, MyGridDataRow)
        '
        If Me.DataSource IsNot Nothing AndAlso Me.DataSource.Current IsNot Nothing AndAlso Me.DataSource.Current.row IsNot Nothing Then
            Me.DataSource.Current.row.acceptchanges()
        End If
    End Sub

    Private Sub MenuItemDelete_Click(sender As Object, e As EventArgs) Handles MenuItemDelete.Click
        If Not DeleteLines(Me) Then
            SetDefaultWritableCell(Me, MyGridDataRow)
            '
            If Me.DataSource IsNot Nothing AndAlso Me.DataSource.Current IsNot Nothing AndAlso Me.DataSource.Current.row IsNot Nothing Then
                Me.DataSource.Current.row.acceptchanges()
            End If
        End If
    End Sub

    Private Sub MenuItemCancel_Click(sender As Object, e As EventArgs) Handles MenuItemCancel.Click
        SendKeys.Send("{Esc}")
    End Sub

#Region "DataGridViewとレコードスキーマに対応したShared関数"

    '行もしくはセルが選択されている
    Public Shared Function RowOrCellSelected(ByVal dgv As DataGridView) As Boolean
        Return 0 < dgv.SelectedRows.Count Or 0 < dgv.SelectedCells.Count
    End Function

    'クリップボードにコピーする
    Public Shared Function SetToClipBoard(ByVal dgv As DataGridView) As Boolean
        'SendKeys.Send("^C")

        '元のDataObjectを取得する
        Dim oldData As DataObject = dgv.GetClipboardContent()
        If oldData Is Nothing Then
            Return False
        End If

        '新しいDataObjectを作成する
        Dim newData As New DataObject()

        'テキスト形式,UnicodeText
        newData.SetData(DataFormats.Text, oldData.GetData(DataFormats.Text))

        'HTML形式のデータ
        Dim htmlObj As Object = oldData.GetData(DataFormats.Html)
        newData.SetData(DataFormats.Html, htmlObj)

        'CSV形式
        Dim csvObj As Object = oldData.GetData(DataFormats.CommaSeparatedValue)
        Dim csvStrm As MemoryStream = Nothing
        If TypeOf csvObj Is String Then
            'ANSI
            csvStrm = New MemoryStream(System.Text.Encoding.Default.GetBytes(DirectCast(csvObj, String)))
        Else
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "ClipBoard CSV Type={0}", csvObj.GetType().Name)
        End If
        If csvStrm IsNot Nothing Then
            newData.SetData(DataFormats.CommaSeparatedValue, csvStrm)
        End If

        Clipboard.SetDataObject(newData)
        Return Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)
    End Function

    '貼り付け可能か？
    Public Shared Function IsPastable(ByVal dgv As DataGridView) As Boolean
        If Not RowOrCellSelected(dgv) Then
            Return False
        End If
        'クリップボードにテキストがある
        Dim clipdata As IDataObject = Clipboard.GetDataObject()
        If Not clipdata.GetDataPresent(DataFormats.Text) Then
            Return False
        End If

        '行選択がある時の貼り付け条件
        If dgv.SelectedRows.Count > 0 Then
            '行のセル以外が選択されるのはNG
            For Each c As DataGridViewCell In dgv.SelectedCells
                If Not dgv.SelectedRows.Contains(dgv.Rows(c.RowIndex)) Then
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Not Pastable 選択行以外のセル({0},{1})", c.ColumnIndex, c.RowIndex)
                    Return False
                End If
            Next
            '表示行数
            Dim ColumnCountVisible As Integer = 0
            For Each column As DataGridViewColumn In dgv.Columns
                If column.Visible Then
                    ColumnCountVisible += 1
                End If
            Next
            '全行表示分がある
            For Each line As String In CType(clipdata.GetData(DataFormats.Text), String).Split(vbCrLf)
                If line.Split(vbTab).Count < ColumnCountVisible Then
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Not Pastable({0}) <{1}>", ColumnCountVisible, line.Replace(vbTab, ","))
                    Return False
                End If
            Next
            Return True
        End If

        'セルのみの選択
        Return HasWritableCell(dgv)
    End Function

    '選択に入力可能なセルを含む
    Public Shared Function HasWritableCell(ByVal dgv As DataGridView) As Boolean
        For Each cell As DataGridViewCell In dgv.SelectedCells
            If Not cell.ReadOnly AndAlso cell.Visible Then
                Return True '入力可能
            End If
        Next
        Return False
    End Function

    '行の削除
    Public Shared Function DeleteLines(ByVal dgv As DataGridView) As Boolean
        If Not dgv.AllowUserToDeleteRows OrElse dgv.SelectedRows.Count = 0 Then
            Return False
        End If
        '行のみ選択
        For Each c As DataGridViewCell In dgv.SelectedCells
            If Not dgv.SelectedRows.Contains(dgv.Rows(c.RowIndex)) Then
                Return False
            End If
        Next

        Dim del As Boolean = False
        For Each r In dgv.SelectedRows
            If Not r.IsNewRow Then
                dgv.Rows.Remove(r)
                del = True
            End If
        Next r
        Return del
    End Function

    '入力可能なセルをデフォルト値にする
    Public Shared Sub SetDefaultWritableCell(ByVal dgv As DataGridView, ByVal gridDataRow As clsDataRow)
        If gridDataRow Is Nothing OrElse Not gridDataRow.IsValid Then
            Exit Sub
        End If
        '選択セル
        For Each cell As DataGridViewCell In dgv.SelectedCells
            If Not cell.ReadOnly AndAlso cell.Visible Then
                Dim col As DataGridViewColumn = dgv.Columns(cell.ColumnIndex)
                cell.Value = gridDataRow.DefaultValue(col.DataPropertyName)
            End If
        Next
        '行選択がある時
        If dgv.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In dgv.SelectedRows
                For Each cell As DataGridViewCell In row.Cells
                    If Not cell.ReadOnly AndAlso cell.Visible Then
                        Dim col As DataGridViewColumn = dgv.Columns(cell.ColumnIndex)
                        cell.Value = gridDataRow.DefaultValue(col.DataPropertyName)
                    End If
                Next
            Next
        End If
    End Sub

    '貼り付け
    Public Shared Sub DoPaste(ByVal dgv As DataGridView, ByVal gridDataRow As clsDataRow)
        If Not IsPastable(dgv) Then
            Return
        End If
        Dim cliptext As String = Clipboard.GetDataObject().GetData(DataFormats.Text)
        If String.IsNullOrWhiteSpace(cliptext) Then
            Exit Sub
        End If
        cliptext = cliptext.TrimEnd()

        Dim clipTSV As New List(Of String())
        For Each line As String In cliptext.Split(vbLf)
            clipTSV.Add(line.Split(vbTab))
        Next
        If dgv.SelectedRows.Count > 0 Then
            '行選択がある場合は、行のみの貼り付け(項目数確認済)
            Dim dstLine As New SortedDictionary(Of Integer, Boolean)
            For Each row As DataGridViewRow In dgv.SelectedRows
                dstLine(row.Index) = True
            Next
            DoPasteLines(dstLine, clipTSV, dgv, gridDataRow)

        Else
            '選択されたセルへの貼り付け
            Dim dstcels As New List(Of Integer())
            For Each c As DataGridViewCell In dgv.SelectedCells
                dstcels.Add(New Integer() {c.RowIndex, c.ColumnIndex})
            Next
            dstcels.Sort(AddressOf cmp)

            Dim prevLine As Integer = -1
            Dim prevColm As Integer = -1
            Dim src_c As Integer = 0
            Dim src_r As Integer = -1
            For Each dst() As Integer In dstcels
                If prevLine <> dst(0) Then
                    src_c = 0
                    src_r += 1
                Else
                    src_c += 1
                End If
                prevLine = dst(0)
                prevColm = dst(1)
                If src_c >= clipTSV(src_r Mod clipTSV.Count).Count Then
                    Continue For
                End If
                DoPasteOneCell(dst(0), dst(1), clipTSV(src_r Mod clipTSV.Count)(src_c), dgv, gridDataRow)
            Next
        End If
    End Sub

    Private Shared Function DoPasteOneCell(ByVal cell As DataGridViewCell, ByVal pasteValue As String,
                                           ByVal dgv As DataGridView, ByVal gridDataRow As clsDataRow) As Integer
        If Not cell.Visible Then
            Return 0    'スキップ
        ElseIf cell.ReadOnly Then
            Return 1 'スルー
        End If

        Dim value As String = ""
        If pasteValue IsNot Nothing Then
            value = pasteValue.Replace(vbCr, "").Replace(vbLf, "").Replace(vbTab, "")
        End If

        If gridDataRow IsNot Nothing AndAlso gridDataRow.IsValid Then
            Dim col As DataGridViewColumn = dgv.Columns(cell.ColumnIndex)
            cell.Value = gridDataRow.ValidValue(col.DataPropertyName, value)
        Else
            cell.Value = value
        End If

        'ComboBoxの場合、含まれなければ空になる
        Return 1
    End Function

    Private Shared Sub DoPasteOneLine(ByVal row As DataGridViewRow, ByVal tsv() As String,
                                      ByVal dgv As DataGridView, ByVal gridDataRow As clsDataRow)
        Dim i As Integer = 0
        For Each cell As DataGridViewCell In row.Cells
            If i >= tsv.Count Then
                Return
            End If
            i += DoPasteOneCell(cell, tsv(i), dgv, gridDataRow)
        Next
    End Sub

    Private Shared Sub DoPasteLines(ByVal dstLine As SortedDictionary(Of Integer, Boolean), ByVal clipTSV As List(Of String()),
                             ByVal dgv As DataGridView, ByVal gridDataRow As clsDataRow)
        '貼り付け先の方が多ければ繰り返し
        Dim src_r As Integer = 0
        For Each p As KeyValuePair(Of Integer, Boolean) In dstLine
            DoPasteOneLine(dgv.Rows(p.Key), clipTSV(src_r Mod clipTSV.Count), dgv, gridDataRow)
            src_r += 1
        Next
        'クリップボードに残っていても貼り付けない
    End Sub

    Private Shared Function cmp(ByVal x As Integer(), ByVal y As Integer()) As Integer
        If x(0) <> y(0) Then
            Return x(0) - y(0) 'row
        Else
            Return x(1) - y(1) 'col
        End If
    End Function

    Private Shared Function DoPasteOneCell(ByVal r As Integer, ByVal c As Integer, ByVal pasteValue As String,
                                    ByVal dgv As DataGridView, ByVal gridDataRow As clsDataRow) As Integer
        'Debug.Print("{0}行 {1}列 値<{2}>", r, c, pasteValue)
        Return DoPasteOneCell(dgv.Rows(r).Cells(c), pasteValue, dgv, gridDataRow)
    End Function
#End Region

#End Region

#Region "操作"
    '"f_i番号"が指定値の行を選択する
    Function NumberPositionsSelect(ByVal number As Integer) As Boolean
        Me.ClearSelection()

        Dim bs As BindingSource = Me.DataSource
        If bs Is Nothing Then
            Return False
        End If

        Dim positions As New List(Of Integer)
        For pos As Integer = 0 To bs.Count - 1
            Dim r As DataRow = bs.Item(pos).row
            If r("f_i番号") = number Then
                positions.Add(pos)
            End If
        Next
        If positions.Count = 0 Then
            Return False
        End If

        bs.Position = positions(0)

        For Each pos As Integer In positions
            Me.Rows(pos).Selected = True
        Next
        Return True
    End Function

    'テーブルと現"f_i番号"値を返す
    Function GetTableAndNumber(ByRef table As DataTable, ByRef number As Integer) As Boolean
        Try
            Dim bs As BindingSource = Me.DataSource
            If bs Is Nothing Then
                Return False
            End If
            table = bs.DataSource
            If table Is Nothing Then
                Return False
            End If

            Dim current As System.Data.DataRowView = bs.Current
            If current Is Nothing OrElse current.Row Is Nothing Then
                number = -1
                Return True
            End If

            number = current.Row("f_i番号")
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "ctrDataGridView.GetTableAndNumber")
            Return False

        End Try
    End Function

    'テーブルと現DataRowを返す
    Function GetTableAndRow(ByRef table As DataTable, ByRef row As DataRow) As Boolean
        Try
            Dim bs As BindingSource = Me.DataSource
            If bs Is Nothing Then
                Return False
            End If
            table = bs.DataSource

            Dim current As System.Data.DataRowView = bs.Current
            If current Is Nothing OrElse current.Row Is Nothing Then
                Return False
            End If
            row = current.Row
            Return (table IsNot Nothing)
        Catch ex As Exception
            g_clsLog.LogException(ex, "ctrDataGridView.GetTableAndRow")
            Return False

        End Try
    End Function

    '指定名のフィールド値が一致する最初の1行を選択する
    Function PositionSelect(ByVal row As DataRow, ByVal fldnames() As String) As Boolean
        Me.ClearSelection()

        Dim bs As BindingSource = Me.DataSource
        If bs Is Nothing Then
            Return False
        End If

        For pos As Integer = 0 To bs.Count - 1
            Dim r As DataRow = bs.Item(pos).row
            Dim match As Boolean = True
            For Each fldname As String In fldnames
                If r(fldname) <> row(fldname) Then
                    match = False
                    Continue For
                End If
            Next
            If match Then
                bs.Position = pos
                Me.Rows(pos).Selected = True
                Return True
            End If
        Next
        Return False
    End Function

#End Region

#Region "カラム幅"

    'カラム幅を文字列に保存
    Public ReadOnly Property GetColumnWidthString() As String
        Get
            Dim sb As New System.Text.StringBuilder
            For Each d1 As DataGridViewColumn In Me.Columns
                If d1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill OrElse
                Not d1.Visible OrElse
                d1.Resizable = DataGridViewTriState.False Then
                    sb.Append("-1")
                Else
                    sb.Append(d1.Width.ToString)
                End If
                sb.Append(",")
            Next
            Return sb.ToString
        End Get
    End Property

    '文字列からカラム幅を復元
    Public Function SetColumnWidthFromString(ByVal csvStr As String) As Integer
        If [String].IsNullOrEmpty(csvStr) Then
            Return -1
        End If
        Dim widthStr As String() = csvStr.Split(",")
        For i As Integer = 0 To widthStr.Length - 1
            If i >= Me.Columns.Count Then
                Return i
            End If
            Dim d1 As DataGridViewColumn = Me.Columns(i)
            If d1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill OrElse
                Not d1.Visible OrElse
                d1.Resizable = DataGridViewTriState.False Then
                Continue For
            End If

            Dim width As Integer = -1
            If Not Integer.TryParse(widthStr(i), width) OrElse width <= 0 Then
                Return i
            End If
            Me.Columns(i).Width = width
        Next
        Return widthStr.Length
    End Function
#End Region

#Region "出力"
    Public Function GridCsvOut(ByVal filepath As String, ByRef errmsg As String) As Boolean
        Dim fpath As String = IO.Path.GetFileNameWithoutExtension(filepath) & "_"
        fpath = IO.Path.ChangeExtension(fpath, ".csv")
        fpath = IO.Path.Combine(IO.Path.GetTempPath, fpath)

        Dim sb As New System.Text.StringBuilder

        For Each col As DataGridViewColumn In Me.Columns
            If col.Visible Then
                sb.Append(""""c).Append(col.HeaderText).Append(""",")
            End If
        Next
        sb.AppendLine()

        For Each row As DataGridViewRow In Me.Rows

            For Each col As DataGridViewColumn In Me.Columns
                If col.Visible Then
                    sb.Append(""""c).Append(row.Cells(col.Index).Value).Append(""",")
                End If
            Next
            sb.AppendLine()
        Next

        Try
            'UTF8:エクセルはUNICODEはダメ, shift_jis は丸数字などが文字化けのため
            Using sw As New System.IO.StreamWriter(fpath, False, System.Text.Encoding.UTF8) '上書き
                sw.Write(sb.ToString)
                sw.Close()
            End Using
        Catch ex As Exception
            '指定されたファイル'{0}'への保存ができませんでした。
            errmsg = String.Format(My.Resources.WarningFileSaveError, fpath)
            g_clsLog.LogException(ex, "mdlGrid.GridCsvOut", errmsg)
            Return False
        End Try

        Try
            Dim p As New Process
            p.StartInfo.FileName = fpath
            p.StartInfo.UseShellExecute = True
            p.Start()
        Catch ex As Exception
            'ファイル'{0}'を起動できませんでした。
            errmsg = String.Format(My.Resources.WarningFileStartError, fpath)
            g_clsLog.LogException(ex, "mdlGrid.GridCsvOut", errmsg)
            Return False
        End Try

        Return True
    End Function

    Private Function paddingSpace(ByVal str As String, ByVal keta As Integer, ByVal isRight As Boolean) As String
        If str Is Nothing Then
            str = ""
        End If
        Const space As String = " "

        '（文字数　=　桁　-　(対象文字列のバイト数 - 対象文字列の文字列数)）
        Dim padLength As Integer = keta - (__encShiftJis.GetByteCount(str) - str.Length)
        If padLength <= 0 Then
            Return str
        End If

        If isRight Then
            Return str.PadLeft(padLength, space.ToCharArray()(0)) '右寄せ
        Else
            Return str.PadRight(padLength, space.ToCharArray()(0)) '左寄せ
        End If
    End Function

    Public Function GridTxtOut(ByVal filepath As String, ByRef errmsg As String) As Boolean
        Dim fpath As String = IO.Path.GetFileNameWithoutExtension(filepath) & "_"
        fpath = IO.Path.ChangeExtension(fpath, ".txt")
        fpath = IO.Path.Combine(IO.Path.GetTempPath, fpath)

        Dim sb As New System.Text.StringBuilder

        For Each col As DataGridViewColumn In Me.Columns
            If col.Visible Then
                Dim str As String = col.HeaderText
                Dim len As Integer = col.Width / 5
                Dim isRight As Boolean = col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                sb.Append(paddingSpace(str, len, isRight)).Append(" ")
            End If
        Next
        sb.AppendLine()

        For Each row As DataGridViewRow In Me.Rows
            For Each col As DataGridViewColumn In Me.Columns
                If col.Visible Then
                    Dim str As String = row.Cells(col.Index).Value.ToString
                    Dim len As Integer = col.Width / 5
                    If len < 2 Then
                        len = 2
                    End If
                    Dim isRight As Boolean = col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    sb.Append(paddingSpace(str, len, isRight)).Append(" "c)
                End If
            Next
            sb.AppendLine()
        Next

        Try
            'csvと同じ文字コード
            Using sw As New System.IO.StreamWriter(fpath, False, System.Text.Encoding.UTF8) '上書き
                sw.Write(sb.ToString)
                sw.Close()
            End Using
        Catch ex As Exception
            '指定されたファイル'{0}'への保存ができませんでした。
            errmsg = String.Format(My.Resources.WarningFileSaveError, fpath)
            g_clsLog.LogException(ex, "mdlGrid.GridTxtOut", errmsg)
            Return False
        End Try

        Try
            Dim p As New Process
            p.StartInfo.FileName = fpath
            p.StartInfo.UseShellExecute = True
            p.Start()
        Catch ex As Exception
            'ファイル'{0}'を起動できませんでした。
            errmsg = String.Format(My.Resources.WarningFileStartError, fpath)
            g_clsLog.LogException(ex, "mdlGrid.GridTxtOut", errmsg)
            Return False
        End Try

        Return True
    End Function


#End Region


End Class
