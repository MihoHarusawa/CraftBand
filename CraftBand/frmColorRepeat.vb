Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.clsDataTables
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables


Friend Class frmColorRepeat
    Const cCheckBoxCount As Integer = 4 '0～3

    Dim _Table As New dstWork.tblColorRepeatDataTable
    Dim _Settings() As CColorRepeatSetting = Nothing

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



    Dim _MyProfile As New CDataGridViewProfile(
            (New dstWork.tblColorRepeatDataTable),
            Nothing,
            enumAction._BackColorReadOnlyYellow
            )
    Private Sub frmColorRepeat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvColorRepeat.SetProfile(_MyProfile)

        'グリッドの色
        DataGridViewColumnColor.DataSource = g_clsSelectBasics.p_tblColor
        DataGridViewColumnColor.DisplayMember = "Display"
        DataGridViewColumnColor.ValueMember = "Value"
        '本幅
        DataGridViewColumnLane.DataSource = g_clsSelectBasics.p_tblLane
        DataGridViewColumnLane.DisplayMember = "Display"
        DataGridViewColumnLane.ValueMember = "Value"

        '参照チェック
        For Each r As dstWork.tblColorRepeatRow In _Table
            If Not r.IsLaneNull Then
                If g_clsSelectBasics.p_i本幅 < r.Lane Then
                    r.SetLaneNull()
                End If
            End If
            If Not g_clsSelectBasics.IsExistColor(r.Color) Then
                r.SetColorNull()
            End If
        Next
        If _Table.Rows.Count = 0 Then
            add_record()
        End If
        'テーブル表示
        BindingSourceColorRepeat.DataSource = _Table
        dgvColorRepeat.Refresh()


        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmColorRepeatSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmColorRepeatGrid", colwid) Then
            Me.dgvColorRepeat.SetColumnWidthFromString(colwid)
        End If
    End Sub

    Private Sub frmColorRepeat_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmColorRepeatGrid", Me.dgvColorRepeat.GetColumnWidthString())
        __paras.SetLastData("frmColorRepeatSize", Me.Size)
    End Sub

    '表示画面の初期化
    Function Initialize(ByVal settings As CColorRepeatSetting()) As Boolean
        If settings Is Nothing OrElse settings.Count = 0 Then
            Return False
        End If
        _Settings = settings

        Dim lanechange As Boolean = False
        If _Settings IsNot Nothing Then
            For idx As Integer = 0 To _Settings.Length - 1
                If setCheckBox(idx, _Settings(idx)) Then
                    If _Settings(idx).AdjustableLane Then
                        lanechange = True
                    End If
                Else
                    Return False
                End If
            Next
        End If
        If Not lanechange Then
            DataGridViewColumnLane.Visible = False
        End If

        _Initialized = True
        Return True
    End Function

    Private Function setCheckBox(ByVal idx As Integer, ByVal setting As CColorRepeatSetting) As Boolean
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
            g_clsLog.LogException(ex, "frmColorRepeat.findCheckBox")
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



    Private Sub btn削除_Click(sender As Object, e As EventArgs) Handles btn削除.Click
        Dim current As System.Data.DataRowView = BindingSourceColorRepeat.Current
        If current Is Nothing OrElse current.Row Is Nothing Then
            Exit Sub
        End If

        current.Row.Delete()
        _Table.AcceptChanges()

        Dim idxseq As Integer = 1 '開始番号
        Dim res = (From row As DataRow In _Table
                   Select Idx = row("Index")
                   Order By Idx).Distinct

        'レコード番号順
        For Each idx As Integer In res
            If idx < idxseq Then
                Exit Sub
            ElseIf idxseq < idx Then
                For Each row As DataRow In _Table.Select(String.Format("Index = {0}", idx))
                    row("Index") = idxseq
                Next row
            Else
                '一致はOK
            End If
            idxseq += 1
        Next idx
    End Sub

    Private Sub add_record()
        Dim row As dstWork.tblColorRepeatRow = _Table.NewtblColorRepeatRow
        row.Index = _Table.Rows.Count + 1
        row.SetColorNull()
        row.SetLaneNull()

        _Table.Rows.Add(row)
    End Sub

    Private Sub btn追加_Click(sender As Object, e As EventArgs) Handles btn追加.Click
        add_record()
        dgvColorRepeat.Refresh()
    End Sub

    '変更実行ボタン
    Private Sub btn変更実行_Click(sender As Object, e As EventArgs) Handles btn変更実行.Click
        If Not IsInitialized Then
            Exit Sub
        End If
        If _Table.Rows.Count = 0 Then
            '少なくとも1点の変更を指定してください。
            MessageBox.Show(My.Resources.MessageColorSet, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim changed As Integer = 0
        For idx As Integer = 0 To _Settings.Length - 1
            Dim chdIdx As CheckBox = findCheckBox(idx)
            If chdIdx Is Nothing OrElse Not chdIdx.Enabled OrElse Not chdIdx.Checked Then
                Continue For
            End If

            Dim rows() As DataRow = _Data.GetTableRows(_Settings(idx).TableID, _Settings(idx).Condition, _Settings(idx).Order)
            If rows Is Nothing OrElse rows.Count = 0 Then
                Continue For
            End If

            reset_record()
            For Each row In rows
                Dim record As dstWork.tblColorRepeatRow = next_record()
                If Not record.IsColorNull AndAlso Not String.IsNullOrWhiteSpace(record.Color) Then
                    row("f_s色") = record.Color
                    changed += 1
                End If
                If _Settings(idx).AdjustableLane AndAlso
                 Not record.IsLaneNull AndAlso 0 < record.Lane Then
                    row("f_i何本幅") = record.Lane
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
    End Sub

    Dim _current As Integer = -1
    Private Sub reset_record()
        _current = -1
    End Sub

    Private Function next_record() As dstWork.tblColorRepeatRow
        If _Table.Rows.Count = 0 Then
            Return Nothing
        ElseIf _Table.Rows.Count = 1 Then
            Return _Table.Rows(0)
        End If
        _current += 1
        If _Table.Rows.Count <= _current Then
            _current = 0
        End If
        Return _Table.Rows(_current)
    End Function

    Private Sub btn閉じる_Click(sender As Object, e As EventArgs) Handles btn閉じる.Click
        BindingSourceColorRepeat.DataSource = Nothing
        Me.Hide()
    End Sub
End Class