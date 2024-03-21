Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.clsDataTables
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables

Friend Class frmColorChange
    Const cCheckBoxCount As Integer = 6 '0～5

    Dim _Table As New dstWork.tblColorChangeDataTable
    Dim _Settings() As CColorChangeSetting = Nothing

    Dim _Initialized As Boolean = False
    Friend ReadOnly Property IsInitialized As Boolean
        Get
            Return _Initialized
        End Get
    End Property

    Dim _ChangeCount As Integer = 0
    Friend ReadOnly Property ChangeCount As Integer
        Get
            Return _ChangeCount
        End Get
    End Property

    Dim _Data As clsDataTables = Nothing
    Dim _Expanding As Boolean = False

    Dim _CurrentColors As New Dictionary(Of String, Integer)


    Dim _MyProfile As New CDataGridViewProfile(
            (New dstWork.tblColorChangeDataTable),
            Nothing,
            enumAction._BackColorReadOnlyYellow
            )
    Private Sub frmColorChange_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvColorChange.SetProfile(_MyProfile)

        'グリッドの色
        AfterDataGridViewColumn.DataSource = g_clsSelectBasics.p_tblColor
        AfterDataGridViewColumn.DisplayMember = "Display"
        AfterDataGridViewColumn.ValueMember = "Value"

        '最初の表示
        collectCurrentColor()
        setGridCurrentColor()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmColorChangeSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmColorChangeGrid", colwid) Then
            Me.dgvColorChange.SetColumnWidthFromString(colwid)
        End If
    End Sub

    Private Sub frmColorChange_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmColorChangeGrid", Me.dgvColorChange.GetColumnWidthString())
        __paras.SetLastData("frmColorChangeSize", Me.Size)
    End Sub

    '表示画面の初期化
    Function Initialize(ByVal settings As CColorChangeSetting()) As Boolean
        If settings Is Nothing OrElse settings.Count = 0 Then
            Return False
        End If
        _Settings = settings

        If _Settings IsNot Nothing Then
            For idx As Integer = 0 To _Settings.Length - 1
                If Not setCheckBox(idx, _Settings(idx)) Then
                    Return False
                End If
            Next
        End If

        _Initialized = True
        Return True
    End Function

    Private Function setCheckBox(ByVal idx As Integer, ByVal setting As CColorChangeSetting) As Boolean
        If setting Is Nothing Then
            Return False
        End If
        Dim chdIdx As CheckBox = findCheckBox(idx)
        If chdIdx Is Nothing Then
            Return False
        End If

        'チェックボックス表示
        chdIdx.Visible = True
        chdIdx.Text = setting.ChkBoxText

        Return True
    End Function

    Private Function findCheckBox(ByVal idx As Integer) As CheckBox
        If idx < 0 OrElse cCheckBoxCount <= idx Then
            Return Nothing
        End If
        Try
            Dim chdIdx As CheckBox = Nothing
            For Each ctrl As Control In Me.Controls
                If TypeOf ctrl Is CheckBox AndAlso ctrl.Name = String.Format("CheckBox{0}", idx) Then
                    Return DirectCast(ctrl, CheckBox)
                End If
            Next
            Return Nothing
        Catch ex As Exception
            g_clsLog.LogException(ex, "frmColorChange.findCheckBox")
            Return Nothing
        End Try
    End Function

    '対象データ指定・処理開始
    Function SetDataAndExpand(ByVal data As clsDataTables, ByVal expandAlways As Boolean) As Boolean
        If Not IsInitialized Then
            Return False
        End If
        _Data = data
        _ChangeCount = 0
        If expandAlways Then
            _Expanding = True
        Else
            _Expanding = _Data.p_row底_縦横.Value("f_b展開区分")
        End If

        For idx As Integer = 0 To _Settings.Length - 1
            Dim chdIdx As CheckBox = findCheckBox(idx)
            If chdIdx Is Nothing Then
                Continue For 'チェック済みだが念のため
            End If
            chdIdx.Enabled = (_Expanding OrElse Not _Settings(idx).AtExpanded)
        Next
        Return True
    End Function

    '未定義色の変更確認(#42)
    Sub ShowDialogForUndef()
        Dim undefs As String = _Data.GetUndefColors(_Expanding)
        If Not String.IsNullOrWhiteSpace(undefs) Then
            'バンドの種類にない色({0})は使えません。変更しますか？
            Dim msg As String = String.Format(My.Resources.AskNoBandTypeColor, undefs)
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r = DialogResult.Yes Then
                ShowDialog()
            End If
        End If
    End Sub


    Private Function collectCurrentColorRow(ByVal row As DataRow) As Boolean
        Try
            Dim color As String = clsSelectBasics.ColorString(row("f_s色"))
            If _CurrentColors.Keys.Contains(color) Then
                _CurrentColors(color) += 1
            Else
                _CurrentColors.Add(color, 1)
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmColorChange.countColor")
            Return False
        End Try
    End Function

    Private Function collectCurrentColor() As Boolean
        _CurrentColors.Clear()
        If _Data Is Nothing Then
            Return False
        End If

        '空を最初に
        _CurrentColors.Add("", 0)

        '現在の色を集める
        For idx As Integer = 0 To _Settings.Length - 1
            Dim chdIdx As CheckBox = findCheckBox(idx)
            If chdIdx Is Nothing OrElse Not chdIdx.Enabled OrElse Not chdIdx.Checked Then
                Continue For
            End If

            Dim rows() As DataRow = _Data.GetTableRows(_Settings(idx).TableID, _Settings(idx).Condition)
            If rows Is Nothing OrElse rows.Count = 0 Then
                Continue For
            End If

            For Each row In rows
                collectCurrentColorRow(row)
            Next
        Next

        Return True
    End Function

    Private Function setGridCurrentColor() As Boolean

        Try
            BindingSourceColorChange.DataSource = Nothing
            _Table.Clear()

            Dim newrow As dstWork.tblColorChangeRow = _Table.NewtblColorChangeRow
            For Each color As String In _CurrentColors.Keys
                newrow = _Table.NewtblColorChangeRow
                newrow.IsTarget = False '非対象
                newrow.Before = color
                newrow.Count = _CurrentColors(color)
                _Table.Rows.Add(newrow)
            Next

            BindingSourceColorChange.DataSource = _Table
            dgvColorChange.Refresh()
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmColorChange.setGridCurrentColor")
            Return False
        End Try

    End Function

    '使用色ボタン
    Private Sub btn使用色_Click(sender As Object, e As EventArgs) Handles btn使用色.Click
        collectCurrentColor()
        setGridCurrentColor()
    End Sub

    '該当なし・変更なしはNothingを返す。
    Private Function before_after(ByVal before As String) As String
        Dim after As String = Nothing
        For Each rcolchg As dstWork.tblColorChangeRow In _Table
            If Not rcolchg.IsTarget Then
                Continue For
            End If

            Dim rcolbefore As String = clsSelectBasics.ColorString(rcolchg.Before)
            If before = rcolbefore Then
                after = clsSelectBasics.ColorString(rcolchg.After)
                Exit For
            End If
        Next

        If after Is Nothing Then
            Return Nothing '該当なし

        ElseIf after = before Then
            Return Nothing '同じ

        Else
            Return after

        End If
    End Function

    Private Function colorChangeRow(ByVal row As DataRow) As Boolean
        Try
            Dim color As String = clsSelectBasics.ColorString(row("f_s色"))
            Dim after As String = before_after(color)
            If after IsNot Nothing Then
                row("f_s色") = after
                Return True
            End If
            Return False

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmColorChange.countColor")
            Return False
        End Try
    End Function

    '変更実行ボタン
    Private Sub btn変更実行_Click(sender As Object, e As EventArgs) Handles btn変更実行.Click
        If Not IsInitialized Then
            Exit Sub
        End If

        '変更する色が空/対象なし
        Dim targetExist As Boolean = False
        Dim emptylist As New List(Of String)

        For Each rcolchg As dstWork.tblColorChangeRow In _Table
            If Not rcolchg.IsTarget Then
                Continue For
            End If
            targetExist = True

            If clsSelectBasics.ColorString(rcolchg.After) = "" Then
                Dim rcolbefore As String = clsSelectBasics.ColorString(rcolchg.Before)
                If rcolbefore <> "" Then
                    emptylist.Add(rcolbefore)
                End If
            End If
        Next

        '少なくとも1点のチェックがあること
        If Not targetExist Then
            '変更したい色の'対象'にチェックを入れてください。
            MessageBox.Show(My.Resources.ErrNoChangeCheck, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        'afterに空が指定されている場合は確認をとる
        If 0 < emptylist.Count Then
            '色<{0}>はクリアされます。よろしいですか？(残す場合は'対象'のチェックを外してください)
            Dim msg As String = String.Format(My.Resources.AskColorClear, String.Join(",", emptylist.ToArray))
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        '変更実行
        Dim changed As Integer = 0
        For idx As Integer = 0 To _Settings.Length - 1
            Dim chdIdx As CheckBox = findCheckBox(idx)
            If chdIdx Is Nothing OrElse Not chdIdx.Enabled OrElse Not chdIdx.Checked Then
                Continue For
            End If

            Dim rows() As DataRow = _Data.GetTableRows(_Settings(idx).TableID, _Settings(idx).Condition)
            If rows Is Nothing OrElse rows.Count = 0 Then
                Continue For
            End If

            For Each row In rows
                If colorChangeRow(row) Then
                    changed += 1
                End If
            Next
        Next

        If changed = 0 Then
            '変更はありませんでした。
            MessageBox.Show(My.Resources.MessageColorNoChange, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            '{0}点を変更しました。
            MessageBox.Show(String.Format(My.Resources.MessageColorChanged, changed), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            _ChangeCount += changed
        End If

        '再表示
        collectCurrentColor()
        setGridCurrentColor()
    End Sub

    Private Sub dgvColorChange_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvColorChange.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSourceColorChange.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        If {"After"}.Contains(DataPropertyName) Then
            current.Row("IsTarget") = True
        End If
    End Sub

    Private Sub btn閉じる_Click(sender As Object, e As EventArgs) Handles btn閉じる.Click
        Me.Hide()
    End Sub
End Class