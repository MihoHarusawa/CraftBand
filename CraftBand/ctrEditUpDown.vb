Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports CraftBand.clsUpDown
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables

Public Class ctrEditUpDown

    'Panelを置き、各ControlはPanelにAnchorし、Panelをコードでリサイズする
    '※ユーザーコントロールとしてのサイズでは制御できない・表示がずれる

    Public Property FormCaption As String

    Public Property PanelSize As Drawing.Size
        Set(value As Drawing.Size)
            If Not _isLoadingData Then
                Panel.Size = New Size(value.Width, value.Height)
            End If
        End Set
        Get
            Return Panel.Size
        End Get
    End Property

    Public WriteOnly Property Is側面_下から上へ As Boolean
        Set(value As Boolean)
            _Is側面_下から上へ = value
        End Set
    End Property

    'Square
    Public Property I水平領域四角数 As Integer '剰余数計算用
    Public Property I垂直領域四角数 As Integer '〃
    Public Property I上右側面本数 As Integer = 0 'Reset時の下左初期値
    'Square45
    Public Property IsSquare45 As Boolean = False
    Public Property I横の四角数 As Integer '配置表示用 45度側
    Public Property I縦の四角数 As Integer '〃　　　　135度側

    Dim _Is底位置表示 As Boolean = False 'Square45かつサイズ一致時


    Dim _isLoadingData As Boolean = True 'Designer.vb描画
    Dim _Is側面_下から上へ As Boolean = True

    Dim _CurrentTargetFace As enumTargetFace = enumTargetFace.NoDef
    Dim _UpdownChanged As Boolean = False

    Dim _Calc As New Calc

#Region "公開関数"

    '編集表示する
    Function ShowGrid(ByVal works As clsDataTables, ByVal face As enumTargetFace) As Boolean
        BindingSourceひも上下.Sort = Nothing
        BindingSourceひも上下.DataSource = Nothing
        If works Is Nothing Then
            Return False
        End If

        _CurrentTargetFace = face
        If IsSquare45 Then
            I水平領域四角数 = I横の四角数 + I縦の四角数
            I垂直領域四角数 = I横の四角数 + I縦の四角数
        End If

        Dim updown As clsUpDown = _Calc.loadData(face, works, I上右側面本数)
        If updown Is Nothing Then
            'ひも上下レコードの読み取りエラーです。
            MessageBox.Show(My.Resources.CalcUpDownLoadErr, FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        txt上下のメモ.Text = updown.Memo
        _UpdownChanged = False
        setDataSourceUpDown(updown)

        dgvひも上下.Refresh()
        Panel.Enabled = True
        Return True
    End Function

    '値の入れ替え
    Function Replace(ByVal other_updown As clsUpDown) As Boolean
        If other_updown Is Nothing OrElse Not other_updown.IsValid(False) Then
            Return False
        End If

        Dim updown As clsUpDown = _Calc.replace(other_updown)
        If updown Is Nothing Then
            'ひも上下データが表示できません。
            MessageBox.Show(My.Resources.CalcUpDownLoadErr, FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        _UpdownChanged = True '入れ替え

        setDataSourceUpDown(updown)

        dgvひも上下.Refresh()
        Panel.Enabled = True
        Return True
    End Function

    '編集中の値を保存する(表示はそのまま)
    Function Save(ByVal works As clsDataTables, ByVal isMsg As Boolean) As Boolean
        Dim ret As Boolean = True
        If _UpdownChanged Then
            If BindingSourceひも上下.DataSource IsNot Nothing Then
                Dim updown As New clsUpDown(_CurrentTargetFace, nud水平に.Value, nud垂直に.Value)
                updown.CheckBoxTable = BindingSourceひも上下.DataSource
                updown.Memo = txt上下のメモ.Text

                If _Calc.saveData(updown, works) Then
                    _UpdownChanged = False
                Else
                    If isMsg Then
                        'ひも上下レコードの保存エラーです。
                        MessageBox.Show(My.Resources.CalcUpDownSaveErr, FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                    ret = False
                End If
            Else
                ret = False
            End If
        End If
        Return ret
    End Function

    '編集完了、非表示にする
    Function HideGrid(ByVal works As clsDataTables, Optional isSave As Boolean = True) As Boolean
        Dim ret As Boolean = True
        If isSave Then
            ret = Save(works, True)
        End If
        BindingSourceひも上下.Sort = Nothing
        BindingSourceひも上下.DataSource = Nothing

        Panel.Enabled = False
        _CurrentTargetFace = enumTargetFace.NoDef
        Return ret
    End Function
#End Region

#Region "表示関連処理"

    Private Sub ctrEditUpDown_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _isLoadingData = False

        '全カラムに波及・定数
        dgvひも上下.Columns(1).Width = 45 'dgvひも上下.RowTemplate.Height
    End Sub

#If DEBUG Then
    Private Sub ctrEditUpDown_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Debug.Print("Resize: Me({0},{1})", Me.Width, Me.Height)
        Debug.Print("     Panel({0},{1})", Panel.Width, Panel.Height)
    End Sub
#End If

    Private Function setUpdownColumns(ByVal col As Integer) As Boolean
        If col < 0 OrElse clsUpDown.cMaxUpdownColumns < col Then
            Return False
        End If
        '0はIndex, 1～
        For i As Integer = 1 To col
            dgvひも上下.Columns(i).Visible = True
            dgvひも上下.Columns(i).HeaderText = getIndexPosition(i)
        Next
        For i As Integer = col + 1 To clsUpDown.cMaxUpdownColumns
            dgvひも上下.Columns(i).Visible = False
        Next
        Return True
    End Function

    Private Function setDataSourceUpDown(ByVal updown As clsUpDown) As Boolean
        Try
            If updown Is Nothing Then
                BindingSourceひも上下.Sort = Nothing
                BindingSourceひも上下.DataSource = Nothing
                txt開始位置.Text = ""
            Else
                nud水平に.Value = updown.HorizontalCount
                nud垂直に.Value = updown.VerticalCount

                _Is底位置表示 = IsSquare45 AndAlso (updown.HorizontalCount = I水平領域四角数 AndAlso updown.VerticalCount = I垂直領域四角数)
                setUpdownColumns(updown.HorizontalCount)
                setUpDownCountLeft()
                BindingSourceひも上下.DataSource = updown.CheckBoxTable
                If _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace) Then
                    BindingSourceひも上下.Sort = "Index desc"
                    txt開始位置.Text = My.Resources.LeftLower
                Else
                    BindingSourceひも上下.Sort = "Index"
                    txt開始位置.Text = My.Resources.LeftUpper
                End If
            End If
            btnサイズ変更.Enabled = False

            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "ctrEditUpDown.setDataSourceUpDown")
            Return False
        End Try
    End Function

    '上下図の残り数
    Private Function setUpDownCountLeft() As Boolean
        lbl水平残.Text = "-"
        lbl垂直残.Text = "-"

        Dim horizontalCount As Integer = nud水平に.Value
        Dim verticalCount As Integer = nud垂直に.Value
        If horizontalCount = 0 OrElse verticalCount = 0 Then
            Return False
        End If


        Dim left_h As Integer = I水平領域四角数 Mod horizontalCount
        Dim left_v As Integer = I垂直領域四角数 Mod verticalCount

        If left_h = 0 Then
            lbl水平残.Text = "OK"
        Else
            lbl水平残.Text = String.Format("[ {0} ]", left_h)
        End If
        If left_v = 0 Then
            lbl垂直残.Text = "OK"
        Else
            lbl垂直残.Text = String.Format("[ {0} ]", left_v)
        End If

        Return True
    End Function

    '1～(縦+横) → .. -3, -2, -1, 0, 0, 0, 1, 2, 3 ..
    Private Function getIndexPosition(ByVal idx As Integer) As Integer
        If _Is底位置表示 Then

            Dim smalls As Integer
            Dim coms As Integer
            If I縦の四角数 < I横の四角数 Then
                smalls = I縦の四角数
                coms = I横の四角数 - I縦の四角数
            Else
                smalls = I横の四角数
                coms = I縦の四角数 - I横の四角数
            End If

            If idx <= smalls Then
                Return idx - smalls - 1
            ElseIf idx <= smalls + coms Then
                Return 0
            ElseIf idx <= I縦の四角数 + I横の四角数 Then
                Return idx - (smalls + coms)
            Else
                Return idx
            End If
        Else
            Return idx
        End If
    End Function

    '1～I水平領域四角数, 1～I垂直領域四角数
    Private Function getBackColor(ByVal horzIdx As Integer, ByVal vertIdx As Integer, ByVal value As Boolean) As Drawing.Color
        If _Is底位置表示 Then
            Dim cat As Integer = checkIsInBottom(horzIdx, vertIdx)
            If 0 < cat Then
                '底の中
                Return If(value, Color.FromArgb(160, 160, 160), Color.White)
            ElseIf cat < 0 Then
                '側面
                Return If(value, Color.FromArgb(140, 140, 140), Color.FromArgb(240, 240, 240))
            Else 'cat=0
                '境界線
                Return If(value, Color.FromArgb(140, 140, 110), Color.FromArgb(240, 240, 210))
            End If
        Else
            Return If(value, Color.FromArgb(160, 160, 160), Color.White)
        End If
    End Function

    '戻り値 1=底 0=境界線 -1=側面
    Private Function checkIsInBottom(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As Integer
        Dim n左上ライン As Integer = horzIdx + vertIdx
        Dim n右下ライン As Integer = (I水平領域四角数 - horzIdx + 1) + (I垂直領域四角数 - vertIdx + 1)
        Dim n右上ライン As Integer = (I水平領域四角数 - horzIdx + 1) + vertIdx
        Dim n左下ライン As Integer = horzIdx + (I垂直領域四角数 - vertIdx + 1)

        If n左上ライン = I横の四角数 + 1 Then
            Return 0
        ElseIf n左上ライン < I横の四角数 + 1 Then
            Return -1

        ElseIf n右下ライン = I横の四角数 + 1 Then
            Return 0
        ElseIf n右下ライン < I横の四角数 + 1 Then
            Return -1

        ElseIf n右上ライン = I縦の四角数 + 1 Then
            Return 0
        ElseIf n右上ライン < I縦の四角数 + 1 Then
            Return -1

        ElseIf n左下ライン = I縦の四角数 + 1 Then
            Return 0
        ElseIf n左下ライン < I縦の四角数 + 1 Then
            Return -1

        Else
            Return 1 '底
        End If
    End Function

    '全リサイズ対象か？ 0～表示列数-1
    Private Function getIsResizeColumn(ByVal columnIndex As Integer) As Boolean
        Return (columnIndex = 1) OrElse (columnIndex = nud水平に.Value) '0=非表示のIndexカラム
    End Function

    '全リサイズ対象か？ 0～表示行数-1
    Private Function getIsResizeRowIndex(ByVal rowIndex As Integer) As Boolean
        Return (rowIndex = 0) OrElse (rowIndex = nud垂直に.Value - 1)
    End Function
#End Region

#Region "ボタン処理"

    Private Sub btnサイズ変更_Click(sender As Object, e As EventArgs) Handles btnサイズ変更.Click
        If nud水平に.Value <= 0 OrElse nud垂直に.Value <= 0 Then
            '水平・垂直の本数を指定してください。
            MessageBox.Show(My.Resources.MessageNoUpDownSize, FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _Calc.updownサイズ変更(nud水平に.Value, nud垂直に.Value, False)
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btnクリア_Click(sender As Object, e As EventArgs) Handles btnクリア.Click
        Dim isReset As Boolean = True
        If BindingSourceひも上下.DataSource IsNot Nothing Then
            'ひも上下の編集内容をクリアします。サイズも初期化してよろしいですか？(はいで全て初期化、いいえはサイズ保持)
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetUpDown, FormCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r = DialogResult.Yes Then
                isReset = True
            ElseIf r = DialogResult.No Then
                isReset = False
            Else
                Exit Sub
            End If
        End If

        txt上下のメモ.Text = ""

        Dim updown As clsUpDown = _Calc.updownリセット(isReset, I上右側面本数)
        If updown Is Nothing Then
            setDataSourceUpDown(Nothing)
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btn上下交換_Click(sender As Object, e As EventArgs) Handles btn上下交換.Click
        Dim gridsel As New CDataGridSelection
        If Not gridsel.GridSelectedCells(dgvひも上下) Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim updown As clsUpDown = _Calc.updown上下交換(gridsel.SubRange(dgvひも上下))
        If updown Is Nothing Then
            setDataSourceUpDown(Nothing)
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
        gridsel.GridCellsSelect(dgvひも上下)
    End Sub

    Private Sub btn左右反転_Click(sender As Object, e As EventArgs) Handles btn左右反転.Click
        Dim gridsel As New CDataGridSelection
        If Not gridsel.GridSelectedCells(dgvひも上下) Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim updown As clsUpDown = _Calc.updown左右反転(gridsel.SubRange(dgvひも上下))
        If updown Is Nothing Then
            setDataSourceUpDown(Nothing)
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
        gridsel.GridCellsSelect(dgvひも上下)
    End Sub

    Private Sub btn天地反転_Click(sender As Object, e As EventArgs) Handles btn天地反転.Click
        Dim gridsel As New CDataGridSelection
        If Not gridsel.GridSelectedCells(dgvひも上下) Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim updown As clsUpDown = _Calc.updown天地反転(gridsel.SubRange(dgvひも上下))
        If updown Is Nothing Then
            setDataSourceUpDown(Nothing)
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
        gridsel.GridCellsSelect(dgvひも上下)
    End Sub

    Private Sub btn右回転_Click(sender As Object, e As EventArgs) Handles btn右回転.Click
        Dim gridsel As New CDataGridSelection
        If Not gridsel.GridSelectedCells(dgvひも上下) OrElse Not gridsel.IsSquareSelect Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim desc As Boolean = _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace)
        Dim updown As clsUpDown = _Calc.updown右回転(desc, gridsel.SubRange(dgvひも上下))
        If updown Is Nothing Then
            setDataSourceUpDown(Nothing)
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
        gridsel.GridCellsSelect(dgvひも上下)
    End Sub

    Private Sub btn左回転_Click(sender As Object, e As EventArgs) Handles btn左回転.Click
        Dim gridsel As New CDataGridSelection
        If Not gridsel.GridSelectedCells(dgvひも上下) OrElse Not gridsel.IsSquareSelect Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim desc As Boolean = _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace)
        Dim updown As clsUpDown = _Calc.updown右回転(Not desc, gridsel.SubRange(dgvひも上下))
        If updown Is Nothing Then
            setDataSourceUpDown(Nothing)
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
        gridsel.GridCellsSelect(dgvひも上下)
    End Sub

    Private Sub btnシフト_Click(sender As Object, e As EventArgs) Handles btnシフト.Click
        Dim updown As clsUpDown
        If rad上.Checked Then
            setDataSourceUpDown(Nothing)
            If _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace) Then
                updown = _Calc.updown垂直シフト(False) '逆順なので後
            Else
                updown = _Calc.updown垂直シフト(True) '正順なので先頭
            End If

        ElseIf rad下.Checked Then
            setDataSourceUpDown(Nothing)
            If _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace) Then
                updown = _Calc.updown垂直シフト(True) '逆順なので先頭
            Else
                updown = _Calc.updown垂直シフト(False) '正順なので後
            End If

        ElseIf rad右.Checked Then
            setDataSourceUpDown(Nothing)
            updown = _Calc.updown水平シフト(False)

        ElseIf rad左.Checked Then
            setDataSourceUpDown(Nothing)
            updown = _Calc.updown水平シフト(True)

        Else
            Exit Sub
        End If

        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btn追加_Click(sender As Object, e As EventArgs) Handles btn追加.Click
        If rad上.Checked OrElse rad下.Checked Then
            If clsUpDown.cMaxUpdownColumns <= dgvひも上下.Rows.Count Then
                'これ以上増やせません。
                MessageBox.Show(My.Resources.MessageNoMore, FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

        ElseIf rad右.Checked OrElse rad左.Checked Then
            If dgvひも上下.Columns(clsUpDown.cMaxUpdownColumns).Visible Then
                'これ以上増やせません。
                MessageBox.Show(My.Resources.MessageNoMore, FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
        Else
            Exit Sub
        End If

        setDataSourceUpDown(Nothing)
        Dim updown As clsUpDown = Nothing

        If rad上.Checked Then
            If _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace) Then
                updown = _Calc.updown垂直追加(False) '逆順なので後
            Else
                updown = _Calc.updown垂直追加(True) '正順なので先頭
            End If

        ElseIf rad下.Checked Then
            If _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace) Then
                updown = _Calc.updown垂直追加(True) '逆順なので先頭
            Else
                updown = _Calc.updown垂直追加(False) '正順なので後
            End If

        ElseIf rad右.Checked Then
            updown = _Calc.updown水平追加(False)

        ElseIf rad左.Checked Then
            updown = _Calc.updown水平追加(True)
        End If

        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btnランダム_Click(sender As Object, e As EventArgs) Handles btnランダム.Click
        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _Calc.updownランダム()
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btnチェック_Click(sender As Object, e As EventArgs) Handles btnチェック.Click
        Dim msg As String = Nothing
        If Not _Calc.updownチェック(msg) Then
            MessageBox.Show(msg, FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            If String.IsNullOrWhiteSpace(msg) Then
                'チェックOKです。
                MessageBox.Show(My.Resources.MessageCheckOK,
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(msg, FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Function buttenText(sender As Object) As String
        Dim btn As Button = CType(sender, Button)
        Dim text = btn.Text
        If String.IsNullOrEmpty(text) OrElse text.Length < 2 Then
            Return text
        End If
        '(&x)
        Dim nPh As Integer = text.IndexOf("(")
        If 1 < nPh Then
            Return text.Substring(0, nPh)
        End If
        '&A
        Return text.Replace("&", "")
    End Function
#End Region

#Region "DataGridViewイベント処理"

    Private Sub dgvひも上下_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvひも上下.CellValueChanged
        _UpdownChanged = True
    End Sub

    Private Sub txt上下のメモ_TextChanged(sender As Object, e As EventArgs) Handles txt上下のメモ.TextChanged
        _UpdownChanged = True
    End Sub

    Private Sub nud水平に_ValueChanged(sender As Object, e As EventArgs) Handles nud水平に.ValueChanged
        btnサイズ変更.Enabled = True
        lbl水平残.Text = "-"
    End Sub

    Private Sub nud垂直に_ValueChanged(sender As Object, e As EventArgs) Handles nud垂直に.ValueChanged
        btnサイズ変更.Enabled = True
        lbl垂直残.Text = "-"
    End Sub

    Private Sub dgvひも上下_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvひも上下.CellPainting
        If _isLoadingData Then
            Exit Sub
        End If
        '列ヘッダーかどうか調べる
        If e.ColumnIndex < 0 And e.RowIndex >= 0 Then
            Dim Index As Integer = dgvひも上下.Rows(e.RowIndex).Cells(0).Value '垂直の番号

            'セルを描画する
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All)

            '行番号を描画する範囲を決定する
            'e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
            Dim indexRect As Rectangle = e.CellBounds
            indexRect.Inflate(-2, -2)
            '行番号を描画する
            TextRenderer.DrawText(e.Graphics,
                                  getIndexPosition(Index).ToString(),
                e.CellStyle.Font, indexRect, e.CellStyle.ForeColor, TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
            '描画が完了したことを知らせる
            e.Handled = True
        End If
    End Sub

    Private Sub dgvひも上下_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvひも上下.CellFormatting
        If _isLoadingData Then
            Exit Sub
        End If
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If
        If Not dgvひも上下.Rows(e.RowIndex).Cells(e.ColumnIndex).Visible Then
            Exit Sub
        End If

        'Index = e.RowIndex + 1
        e.CellStyle.BackColor = getBackColor(e.ColumnIndex, e.RowIndex + 1, dgvひも上下.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
    End Sub

    '幅変更を波及　※lastdata対象外
    Private Sub dgvひも上下_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvひも上下.ColumnWidthChanged
        If _isLoadingData OrElse e.Column Is Nothing OrElse e.Column.Index < 1 Then
            Exit Sub
        End If
        'Debug.Print("Index={0} width={1}", e.Column.Index, e.Column.Width) 1～
        If getIsResizeColumn(e.Column.Index) Then
            For i As Integer = 1 To cMaxUpdownColumns '0(Index)を除く全カラム
                If i <> e.Column.Index Then
                    dgvひも上下.Columns(i).Width = e.Column.Width
                End If
            Next
        End If
    End Sub

    '高さ変更を波及　※lastdata対象外
    Private Sub dgvひも上下_RowHeightChanged(sender As Object, e As DataGridViewRowEventArgs) Handles dgvひも上下.RowHeightChanged
        If _isLoadingData OrElse e.Row Is Nothing OrElse e.Row.Index < 0 Then
            Exit Sub
        End If
        'Debug.Print("Index={0} width={1}", e.Row.Index, e.Row.Height) 0～
        If getIsResizeRowIndex(e.Row.Index) Then
            For i As Integer = 0 To nud垂直に.Value - 1 '表示分
                If i <> e.Row.Index Then
                    dgvひも上下.Rows(i).Height = e.Row.Height
                End If
            Next
            dgvひも上下.RowTemplate.Height = e.Row.Height
        End If
    End Sub

    'True/False以外の貼り付けは無視
    Private Sub dgvひも上下_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvひも上下.DataError
        If e.Exception Is Nothing Then
            Exit Sub
        End If
        Debug.Print("({0},{1}){2}", e.ColumnIndex, e.RowIndex, e.Exception.Message)
        'dgvひも上下.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = False
        e.ThrowException = False
    End Sub

#End Region

#Region "設定"
    Private Sub btn設定呼出_Click(sender As Object, e As EventArgs) Handles btn設定呼出.Click
        Dim updown_tmp As New clsUpDown(_CurrentTargetFace, nud水平に.Value, nud垂直に.Value)
        updown_tmp.CheckBoxTable = BindingSourceひも上下.DataSource
        updown_tmp.Memo = txt上下のメモ.Text

        Dim dlg As New frmUpDownSetting
        dlg.CurrentUpdown = updown_tmp
        dlg.IsLoadToCurrent = True
        If dlg.ShowDialog() = DialogResult.OK Then
            setDataSourceUpDown(Nothing)
            txt上下のメモ.Text = updown_tmp.Memo
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "updown_tmp:{0}", updown_tmp.ToString)

            'CheckBoxTableに結果が入っている
            Dim updown As clsUpDown = _Calc.updownサイズ変更(updown_tmp.HorizontalCount, updown_tmp.VerticalCount, True)
            If updown Is Nothing Then
                '{0}できませんでした。リセットしてやり直してください。
                MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btn設定呼出.Text),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "updown:{0}", updown.ToString)
                setDataSourceUpDown(updown)
            End If
            dgvひも上下.Refresh()
            _UpdownChanged = True
        End If
    End Sub

    Private Sub btn設定登録_Click(sender As Object, e As EventArgs) Handles btn設定登録.Click
        Dim updown_tmp As New clsUpDown(_CurrentTargetFace, nud水平に.Value, nud垂直に.Value)
        updown_tmp.CheckBoxTable = BindingSourceひも上下.DataSource
        Dim dlg As New frmUpDownSetting
        dlg.CurrentUpdown = updown_tmp
        dlg.IsSaveFromCurrent = True
        If dlg.ShowDialog() = DialogResult.OK Then
            '変わりません
        End If
    End Sub
#End Region

    'DataGridViewの選択
    Private Class CDataGridSelection
        '選択の最小と最大
        Dim ColumnIndexFrom As Integer = Integer.MaxValue
        Dim ColumnIndexTo As Integer = Integer.MinValue
        Dim RowIndexFrom As Integer = Integer.MaxValue
        Dim RowIndexTo As Integer = Integer.MinValue
        Dim SelectedCellsCount As Integer = 0


        ReadOnly Property IsValid As Boolean
            Get
                Return (ColumnIndexFrom <= ColumnIndexTo) AndAlso (RowIndexFrom <= RowIndexTo)
            End Get
        End Property

        ReadOnly Property FromToRangeCells As Integer
            Get
                If Not IsValid Then
                    Return 0
                Else
                    Return (ColumnIndexTo - ColumnIndexFrom + 1) * (RowIndexTo - RowIndexFrom + 1)
                End If
            End Get
        End Property

        '矩形選択
        ReadOnly Property IsRangeSelect As Boolean
            Get
                Return 0 < SelectedCellsCount AndAlso SelectedCellsCount = FromToRangeCells
            End Get
        End Property

        '正方形選択
        ReadOnly Property IsSquareSelect As Boolean
            Get
                Return IsRangeSelect AndAlso (ColumnIndexTo - ColumnIndexFrom) = (RowIndexTo - RowIndexFrom)
            End Get
        End Property


        Private WriteOnly Property ColumnnIndex As Integer
            Set(value As Integer)
                If value < ColumnIndexFrom Then
                    ColumnIndexFrom = value
                End If
                If ColumnIndexTo < value Then
                    ColumnIndexTo = value
                End If
            End Set
        End Property

        Private WriteOnly Property RowIndex As Integer
            Set(value As Integer)
                If value < RowIndexFrom Then
                    RowIndexFrom = value
                End If
                If RowIndexTo < value Then
                    RowIndexTo = value
                End If
            End Set
        End Property

        'DataGridViewで選択されているセルを取得
        Function GridSelectedCells(ByVal dgv As DataGridView) As Boolean
            '行選択
            If 0 < dgv.SelectedRows.Count Then
                Return False
            End If

            'セル選択
            For Each c As DataGridViewCell In dgv.SelectedCells
                ColumnnIndex = c.ColumnIndex
                RowIndex = c.RowIndex
            Next

            '数が一致
            SelectedCellsCount = dgv.SelectedCells.Count
            Return IsRangeSelect
        End Function

        Function GridCellsSelect(ByVal dgv As DataGridView) As Boolean
            '範囲選択であること
            'If Not IsRangeSelect Then
            '    Return False
            'End If

            '範囲を選択
            dgv.ClearSelection()
            For r As Integer = RowIndexFrom To RowIndexTo
                For c As Integer = ColumnIndexFrom To ColumnIndexTo
                    dgv.Rows(r).Cells(c).Selected = True
                Next
            Next

            SelectedCellsCount = dgv.SelectedCells.Count
            Return True
        End Function

        Function SubRange(ByVal dgv As DataGridView) As CSubRange
            '範囲選択かつ2点以上であること
            If Not IsRangeSelect OrElse SelectedCellsCount = 1 Then
                Return Nothing
            End If

            '垂直の番号
            Dim IndexFrom As Integer = dgv.Rows(RowIndexFrom).Cells(0).Value
            Dim IndexTo As Integer = dgv.Rows(RowIndexTo).Cells(0).Value
            If IndexFrom <= IndexTo Then
                Return New CSubRange(ColumnIndexFrom, ColumnIndexTo, IndexFrom, IndexTo)
            Else
                Return New CSubRange(ColumnIndexFrom, ColumnIndexTo, IndexTo, IndexFrom)
            End If
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0}-{1}:{2}-{3}:{4}/{5}", ColumnIndexFrom, ColumnIndexTo, RowIndexFrom, RowIndexTo, SelectedCellsCount, FromToRangeCells)
        End Function
    End Class


    'テーブルを含むインスタンス
    Private Class Calc
        Dim _CUpDown As clsUpDown 'New時に作成、以降は存在が前提

        Sub New()
            _CUpDown = New clsUpDown(False)
            _CUpDown.Clear(True) '唯一のテーブル作成
        End Sub

        'ひも上下の展開
        Function loadData(ByVal face As enumTargetFace, ByVal data As clsDataTables, ByVal sidecount As Integer) As clsUpDown

            '_CUpDownにセットする
            _CUpDown.Clear()
            _CUpDown.TargetFace = face

            data.ToClsUpDown(_CUpDown)

            If Not _CUpDown.IsValid(True) Then 'Tableあり
                _CUpDown.Reset(sidecount)
            End If

            Return _CUpDown
        End Function


        'ひも上下の編集完了,_Dataに反映
        Function saveData(ByVal updown As clsUpDown, data As clsDataTables) As Boolean
            'レコードにセットする
            Dim ret As Boolean = data.FromClsUpDown(updown)

            '_CUpDownは先のまま(CheckBoxTableは共通)

            Return ret
        End Function

        '中身を読み取ったテーブル
        Function replace(ByVal other_updown As clsUpDown) As clsUpDown
            Dim ret As Boolean = _CUpDown.Import(other_updown)
            If ret Then
                Return _CUpDown
            Else
                Return Nothing
            End If
        End Function

        Function updownサイズ変更(ByVal horizontal As Integer, ByVal vertical As Integer, ByVal read_tbl As Boolean) As clsUpDown
            _CUpDown.HorizontalCount = horizontal
            _CUpDown.VerticalCount = vertical
            If Not _CUpDown.ReSize(read_tbl) Then
                Return Nothing
            End If
            Return _CUpDown
        End Function

        Function updownリセット(ByVal isReset As Boolean, ByVal sidecount As Integer) As clsUpDown
            If isReset Then
                If Not _CUpDown.Reset(sidecount) Then
                    Return Nothing
                End If
            Else
                If Not _CUpDown.ResetValue() Then
                    Return Nothing
                End If
            End If
            Return _CUpDown
        End Function

        Function updown上下交換(ByVal subrange As CSubRange) As clsUpDown
            If subrange IsNot Nothing Then
                If Not _CUpDown.RevertRange(subrange, True) Then
                    Return Nothing
                End If
            Else
                '#32 DataGrid操作が認識されないケースもあるようなので
                If Not _CUpDown.Revert(True) Then
                    Return Nothing
                End If
            End If
            Return _CUpDown
        End Function

        Function updown左右反転(ByVal subrange As CSubRange) As clsUpDown
            If subrange IsNot Nothing Then
                If Not _CUpDown.LeftSideRightRange(subrange, True) Then
                    Return Nothing
                End If
            Else
                If Not _CUpDown.LeftSideRight(True) Then
                    Return Nothing
                End If
            End If
            Return _CUpDown
        End Function

        Function updown天地反転(ByVal subrange As CSubRange) As clsUpDown
            If subrange IsNot Nothing Then
                If Not _CUpDown.UpSideDownRange(subrange, True) Then
                    Return Nothing
                End If
            Else
                If Not _CUpDown.UpSideDown(True) Then
                    Return Nothing
                End If
            End If
            Return _CUpDown
        End Function

        Function updown右回転(ByVal desc As Boolean, ByVal subrange As CSubRange) As clsUpDown
            If Not desc Then
                '右回転
                If subrange IsNot Nothing Then
                    If Not _CUpDown.RotateRightRange(subrange, True) Then
                        Return Nothing
                    End If
                Else
                    If Not _CUpDown.RotateRight(True) Then
                        Return Nothing
                    End If
                End If
            Else
                '左回転
                If subrange IsNot Nothing Then
                    If Not _CUpDown.RotateLeftRange(subrange, True) Then
                        Return Nothing
                    End If
                Else
                    If Not _CUpDown.RotateLeft(True) Then
                        Return Nothing
                    End If
                End If

            End If
            Return _CUpDown
        End Function

        Function updown水平追加(ByVal atTop As Boolean) As clsUpDown
            If Not _CUpDown.AddHorizontal(atTop) Then
                Return Nothing
            End If
            Return _CUpDown
        End Function

        Function updown垂直追加(ByVal atTop As Boolean) As clsUpDown
            If Not _CUpDown.AddVertical(atTop, True) Then
                Return Nothing
            End If
            Return _CUpDown
        End Function

        Function updown水平シフト(ByVal desc As Boolean) As clsUpDown
            If Not _CUpDown.ShiftHorizontal(desc, True) Then
                Return Nothing
            End If
            Return _CUpDown
        End Function

        Function updown垂直シフト(ByVal desc As Boolean) As clsUpDown
            If Not _CUpDown.ShiftVertical(desc, True) Then
                Return Nothing
            End If
            Return _CUpDown
        End Function

        Function updownランダム() As clsUpDown
            If Not _CUpDown.Randomize() Then
                Return Nothing
            End If
            Return _CUpDown
        End Function

        'チェック #28
        Function updownチェック(ByRef msg As String) As Boolean
            Return _CUpDown.Check(msg)
        End Function

    End Class




    Private Sub GridContextMenuStrip_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles GridContextMenuStrip.Opening
        '共通
        Me.MenuItemCopy.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemCut.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemPaste.Enabled = ctrDataGridView.IsPastable(dgvひも上下)
        Me.MenuItemDelete.Enabled = ctrDataGridView.HasWritableCell(dgvひも上下)

        Me.MenuItemCancel.Enabled = True
        '選択
        Me.MenuItemRect.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemSquare.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemCenter.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)

        '選択の移動
        Me.MenuItemLeftDownMove.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemDownMove.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemRightDownMove.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemLeftMove.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemRightMove.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemLeftUpMove.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemUpMove.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemRightUpMove.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)

        '選択の拡張
        Me.MenuItemLeftDownExt.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemDownExt.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemRightDownExt.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemLeftExt.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemRightExt.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemLeftUpExt.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemUpExt.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemRightUpExt.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)

        '操作
        Me.MenuItemON.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemOFF.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemExchange.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
    End Sub

    Private Sub MenuItemCut_Click(sender As Object, e As EventArgs) Handles MenuItemCut.Click
        ctrDataGridView.SetToClipBoard(dgvひも上下)
        SetSelectedCellValue(dgvひも上下, False)
    End Sub

    Private Sub MenuItemCopy_Click(sender As Object, e As EventArgs) Handles MenuItemCopy.Click
        ctrDataGridView.SetToClipBoard(dgvひも上下)
        dgvひも上下.ClearSelection()
    End Sub

    Private Sub MenuItemPaste_Click(sender As Object, e As EventArgs) Handles MenuItemPaste.Click
        'IsNewの行はありません
        ctrDataGridView.DoPaste(dgvひも上下, Nothing)
    End Sub

    Private Sub MenuItemDelete_Click(sender As Object, e As EventArgs) Handles MenuItemDelete.Click
        '行は削除されません
        SetSelectedCellValue(dgvひも上下, check_change.check_off)
    End Sub

    Private Sub MenuItemCancel_Click(sender As Object, e As EventArgs) Handles MenuItemCancel.Click
        SendKeys.Send("{Esc}")
    End Sub

    Private Enum check_change
        check_on
        check_off
        check_rev
    End Enum

    '全選択セルに指定値をセット
    Private Shared Sub SetSelectedCellValue(ByVal dgv As DataGridView, ByVal val As check_change)
        '選択セル
        For Each cell As DataGridViewCell In dgv.SelectedCells
            If Not cell.ReadOnly AndAlso cell.Visible Then
                If val = check_change.check_on Then
                    cell.Value = 1
                ElseIf val = check_change.check_off Then
                    cell.Value = 0
                ElseIf val = check_change.check_rev Then
                    cell.Value = Not cell.Value
                End If
            End If
        Next
        '行選択がある時 TODO:revert時の重複操作
        If dgv.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In dgv.SelectedRows
                For Each cell As DataGridViewCell In row.Cells
                    If Not cell.ReadOnly AndAlso cell.Visible Then
                        If val = check_change.check_on Then
                            cell.Value = 1
                        ElseIf val = check_change.check_off Then
                            cell.Value = 0
                        ElseIf val = check_change.check_rev Then
                            cell.Value = Not cell.Value
                        End If
                    End If
                Next
            Next
        End If
        '
    End Sub

    'チェックON
    Private Sub MenuItemON_Click(sender As Object, e As EventArgs) Handles MenuItemON.Click
        SetSelectedCellValue(dgvひも上下, check_change.check_on)
        BindingSourceひも上下.Current.row.acceptchanges()
    End Sub

    'チェックOFF
    Private Sub MenuItemOFF_Click(sender As Object, e As EventArgs) Handles MenuItemOFF.Click
        SetSelectedCellValue(dgvひも上下, check_change.check_off)
        BindingSourceひも上下.Current.row.acceptchanges()
    End Sub

    '入れ替え
    Private Sub MenuItemExchange_Click(sender As Object, e As EventArgs) Handles MenuItemExchange.Click
        SetSelectedCellValue(dgvひも上下, check_change.check_rev)
        BindingSourceひも上下.Current.row.acceptchanges()
    End Sub


    '矩形選択
    Private Sub MenuItemRect_Click(sender As Object, e As EventArgs) Handles MenuItemRect.Click
        Dim gridsel As New CDataGridSelection
        If Not gridsel.GridSelectedCells(dgvひも上下) Then
            gridsel.GridCellsSelect(dgvひも上下)
        End If
        Debug.Print("矩形選択")
    End Sub

    '正方形選択
    Private Sub MenuItemSquare_Click(sender As Object, e As EventArgs) Handles MenuItemSquare.Click
        Debug.Print("正方形選択")
    End Sub

    '中心点
    Private Sub MenuItemCenter_Click(sender As Object, e As EventArgs) Handles MenuItemCenter.Click
        Debug.Print("中心点")
    End Sub


    '左下移動
    Private Sub MenuItemLeftDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftDownMove.Click
        Debug.Print("左下移動")
    End Sub

    '下移動
    Private Sub MenuItemDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemDownMove.Click
        Debug.Print("下移動")
    End Sub

    '右下移動
    Private Sub MenuItemRightDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightDownMove.Click
        Debug.Print("右下移動")
    End Sub

    '左移動
    Private Sub MenuItemLeftMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftMove.Click
        Debug.Print("左移動")
    End Sub

    '右移動
    Private Sub MenuItemRightMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightMove.Click
        Debug.Print("右移動")
    End Sub

    '左上移動
    Private Sub MenuItemLeftUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftUpMove.Click
        Debug.Print("左上移動")
    End Sub

    '上移動
    Private Sub MenuItemUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemUpMove.Click
        Debug.Print("上移動")
    End Sub

    '右上移動
    Private Sub MenuItemRightUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightUpMove.Click
        Debug.Print("右上移動")
    End Sub

    '左下拡張
    Private Sub MenuItemLeftDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftDownExt.Click
        Debug.Print("左下拡張")
    End Sub

    '下拡張
    Private Sub MenuItemDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemDownExt.Click
        Debug.Print("下拡張")
    End Sub

    '右下拡張
    Private Sub MenuItemRightDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightDownExt.Click
        Debug.Print("右下拡張")
    End Sub

    '左拡張
    Private Sub MenuItemLeftExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftExt.Click
        Debug.Print("左拡張")
    End Sub

    '右拡張
    Private Sub MenuItemRightExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightExt.Click
        Debug.Print("右拡張")
    End Sub

    '左上拡張
    Private Sub MenuItemLeftUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftUpExt.Click
        Debug.Print("左上拡張")
    End Sub

    '上拡張
    Private Sub MenuItemUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemUpExt.Click
        Debug.Print("上拡張")
    End Sub

    '右上拡張
    Private Sub MenuItemRightUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightUpExt.Click
        Debug.Print("右上拡張")
    End Sub


End Class
