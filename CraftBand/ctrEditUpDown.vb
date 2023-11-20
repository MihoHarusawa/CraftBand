Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports CraftBand.clsUpDown
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables

Public Class ctrEditUpDown

    'Panelを置き、各ControlはPanelにAnchorし、Panelをコードでリサイズする
    '※ユーザーコントロールとしてのサイズでは制御できない・表示がずれる

    Const cDataStartColumnIndex As Integer = 1 'CheckBoxの開始位置
    'DataGridViewの表示はColumnは1～水平数・Rowは1～垂直数-1
    'ColumnIndex=0 には<Index>フィールド値を非表示でセット
    'Descのケースも含め、表示された状態に対して、上・下・左・右で編集します

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
    Dim _DataGridSelection As CDataGridSelection = Nothing

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
        _DataGridSelection = New CDataGridSelection(dgvひも上下)
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
        _DataGridSelection = Nothing

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
        If _DataGridSelection Is Nothing Then
            Exit Sub
        End If
        If Not _DataGridSelection.GetSelected() OrElse Not _DataGridSelection.IsRectangleSelect Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim updown As clsUpDown = _Calc.updown上下交換(_DataGridSelection.SubRange())
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
        _DataGridSelection.SetRangeCellSelected(CDataGridSelection.range_type.range_rectangle)
    End Sub

    Private Sub btn左右反転_Click(sender As Object, e As EventArgs) Handles btn左右反転.Click
        If _DataGridSelection Is Nothing Then
            Exit Sub
        End If
        If Not _DataGridSelection.GetSelected() OrElse Not _DataGridSelection.IsRectangleSelect Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim updown As clsUpDown = _Calc.updown左右反転(_DataGridSelection.SubRange())
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
        _DataGridSelection.SetRangeCellSelected(CDataGridSelection.range_type.range_rectangle)
    End Sub

    Private Sub btn天地反転_Click(sender As Object, e As EventArgs) Handles btn天地反転.Click
        If _DataGridSelection Is Nothing Then
            Exit Sub
        End If
        If Not _DataGridSelection.GetSelected() OrElse Not _DataGridSelection.IsRectangleSelect Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim updown As clsUpDown = _Calc.updown天地反転(_DataGridSelection.SubRange())
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
        _DataGridSelection.SetRangeCellSelected(CDataGridSelection.range_type.range_rectangle)
    End Sub

    Private Sub btn右回転_Click(sender As Object, e As EventArgs) Handles btn右回転.Click
        If _DataGridSelection Is Nothing Then
            Exit Sub
        End If
        If Not _DataGridSelection.GetSelected() OrElse Not _DataGridSelection.IsSquareSelect Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim desc As Boolean = _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace)
        Dim updown As clsUpDown = _Calc.updown右回転(desc, _DataGridSelection.SubRange())
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
        _DataGridSelection.SetRangeCellSelected(CDataGridSelection.range_type.range_rectangle)
    End Sub

    Private Sub btn左回転_Click(sender As Object, e As EventArgs) Handles btn左回転.Click
        If _DataGridSelection Is Nothing Then
            Exit Sub
        End If
        If Not _DataGridSelection.GetSelected() OrElse Not _DataGridSelection.IsSquareSelect Then
            '全範囲を{0}します。よろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, buttenText(sender)),
                                FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim desc As Boolean = _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace)
        Dim updown As clsUpDown = _Calc.updown右回転(Not desc, _DataGridSelection.SubRange())
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
        _DataGridSelection.SetRangeCellSelected(CDataGridSelection.range_type.range_rectangle)
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
        Dim _DGV As DataGridView = Nothing

        '選択の最小と最大
        Dim _ColumnIndexFrom As Integer = Integer.MaxValue
        Dim _ColumnIndexTo As Integer = Integer.MinValue
        Dim _RowIndexFrom As Integer = Integer.MaxValue
        Dim _RowIndexTo As Integer = Integer.MinValue

        '表示されているColumnとRowの数
        Dim _ColumnCount As Integer = 0 '表示列のみ 0は非表示なので1～_ColumnCount
        Dim _RowCount As Integer = 0 '行はこの数 0～_RowCount-1

        'セルの選択状態
        Dim _CellSelected(cMaxUpdownColumns, cMaxUpdownColumns) As Boolean
        Dim _CellSelectedCount As Integer = 0


        Sub New(ByVal dgv As DataGridView)
            _DGV = dgv
        End Sub

        'データとして使用可能な状態であること
        ReadOnly Property IsValid As Boolean
            Get
                Return _DGV IsNot Nothing AndAlso cDataStartColumnIndex < _ColumnCount AndAlso 0 < _RowCount
            End Get
        End Property

        'セルの選択数
        ReadOnly Property CellSelectedCount As Integer
            Get
                Return _CellSelectedCount
            End Get
        End Property

        '少なくとも1点のセルが選択されており、内部値が正しくセットされている
        ReadOnly Property IsCellSelected As Boolean
            Get
                Return IsValid AndAlso 0 < _CellSelectedCount AndAlso
                    cDataStartColumnIndex <= _ColumnIndexFrom AndAlso _ColumnIndexFrom <= _ColumnCount AndAlso _ColumnIndexFrom <= _ColumnIndexTo AndAlso
                    0 <= _RowIndexFrom AndAlso _RowIndexFrom < _RowCount AndAlso _RowIndexFrom <= _RowIndexTo
            End Get
        End Property

        '矩形に選択されている
        ReadOnly Property IsRectangleSelect As Boolean
            Get
                Return IsCellSelected AndAlso
                    _CellSelectedCount = (_ColumnIndexTo - _ColumnIndexFrom + 1) * (_RowIndexTo - _RowIndexFrom + 1)
            End Get
        End Property

        '正方形に選択されている
        ReadOnly Property IsSquareSelect As Boolean
            Get
                Return IsRectangleSelect AndAlso (_ColumnIndexTo - _ColumnIndexFrom) = (_RowIndexTo - _RowIndexFrom)
            End Get
        End Property

        'サイズを変えて初期化
        Private Sub ClearCellSelected(ByVal colcount As Integer, rowcount As Integer)
            _ColumnCount = colcount
            _RowCount = rowcount
            ClearCellSelected()
        End Sub

        'セルの選択を初期化
        Private Sub ClearCellSelected()
            Array.Clear(_CellSelected, False, _CellSelected.Length)

            _CellSelectedCount = 0
            _ColumnIndexFrom = Integer.MaxValue
            _ColumnIndexTo = Integer.MinValue
            _RowIndexFrom = Integer.MaxValue
            _RowIndexTo = Integer.MinValue
        End Sub

        'セルの選択をセット(表示かつ対象範囲内のみ)
        Private Sub setCellSelected(ByVal col As Integer, row As Integer)
            If col < cDataStartColumnIndex OrElse _ColumnCount < col OrElse row < 0 OrElse _RowCount <= row Then
                Exit Sub
            End If
            If Not _CellSelected(col, row) Then
                _CellSelected(col, row) = True
                '
                If col < _ColumnIndexFrom Then
                    _ColumnIndexFrom = col
                End If
                If _ColumnIndexTo < col Then
                    _ColumnIndexTo = col
                End If
                '
                If row < _RowIndexFrom Then
                    _RowIndexFrom = row
                End If
                If _RowIndexTo < row Then
                    _RowIndexTo = row
                End If

                'DataGridView.SelectedCells.Countは行選択の場合非表示のセルも含みますので独自にカウントします
                _CellSelectedCount += 1
            End If
        End Sub
        Private Sub setCellSelected(cell As DataGridViewCell)
            If cell.Visible Then
                setCellSelected(cell.ColumnIndex, cell.RowIndex)
            End If
        End Sub


        'DataGridViewで選択されているセルを取得
        '戻り値: True=1点以上の選択がある　False=状態が不正もしくは選択がない
        Function GetSelected() As Boolean
            If _DGV Is Nothing Then
                Return False
            End If

            '一旦無効化
            _ColumnCount = 0
            _RowCount = 0

            'サイズの初期化とクリア
            Dim rowcount As Integer = _DGV.Rows.Count
            If rowcount <= 0 Then
                Return False
            End If
            Dim colcount As Integer = 0
            For col As Integer = _DGV.Columns.Count - 1 To cDataStartColumnIndex Step -1
                If _DGV.Columns(col).Visible Then
                    colcount = col
                    Exit For
                End If
            Next
            If colcount <= 0 Then
                Return False
            End If
            ClearCellSelected(colcount, rowcount)

            '行選択からセル選択を取得
            If 0 < _DGV.SelectedRows.Count Then
                For Each row As DataGridViewRow In _DGV.SelectedRows
                    For col As Integer = cDataStartColumnIndex To colcount
                        setCellSelected(col, row.Index)
                    Next
                Next
            End If

            'セル選択を取得
            For Each cell As DataGridViewCell In _DGV.SelectedCells
                setCellSelected(cell)
            Next

            Return IsCellSelected
        End Function

        '選択を移動する
        Function MoveSelection(ByVal col_ref As Integer, ByVal row_ref As Integer) As Boolean
            If Not IsValid OrElse (col_ref = 0 AndAlso row_ref = 0) Then
                Return False
            End If
            Dim save(,) As Boolean = DirectCast(_CellSelected.Clone(), Boolean(,))
            ClearCellSelected()
            For row As Integer = 0 To _RowCount - 1
                For col As Integer = cDataStartColumnIndex To _ColumnCount
                    If save(col, row) Then
                        setCellSelected(col + col_ref, row + row_ref)
                    End If
                Next
            Next

            Return SetRangeCellSelected(range_type.range_selected)
        End Function

        '選択を拡張する
        Function ExtentSelection(ByVal col_ref As Integer, ByVal row_ref As Integer) As Boolean
            If Not IsValid OrElse (col_ref = 0 AndAlso row_ref = 0) Then
                Return False
            End If
            Dim save(,) As Boolean = DirectCast(_CellSelected.Clone(), Boolean(,))
            For row As Integer = 0 To _RowCount - 1
                For col As Integer = cDataStartColumnIndex To _ColumnCount
                    If save(col, row) Then
                        setCellSelected(col + col_ref, row + row_ref)
                    End If
                Next
            Next

            Return SetRangeCellSelected(range_type.range_selected)
        End Function


        Enum check_value
            check_on
            check_off
            check_revert
        End Enum

        '全選択セルに指定値をセット
        Function SetSelectedCellValue(ByVal val As check_value) As Boolean
            If Not IsCellSelected Then
                Return False
            End If
            For row As Integer = 0 To _RowCount - 1
                For col As Integer = cDataStartColumnIndex To _ColumnCount
                    If _CellSelected(col, row) Then
                        If val = check_value.check_on Then
                            _DGV.Rows(row).Cells(col).Value = True
                        ElseIf val = check_value.check_off Then
                            _DGV.Rows(row).Cells(col).Value = False
                        ElseIf val = check_value.check_revert Then
                            _DGV.Rows(row).Cells(col).Value = Not _DGV.Rows(row).Cells(col).Value
                        End If
                    End If
                Next
            Next
            Return True
        End Function



        Enum range_type
            range_clear
            range_all
            range_selected
            range_rectangle
            range_square
            range_center
        End Enum

        '指定のセルを選択する
        Function SetRangeCellSelected(ByVal range As range_type) As Boolean

            Dim rfrom As Integer = _RowIndexFrom
            Dim rto As Integer = _RowIndexTo
            Dim cfrom As Integer = _ColumnIndexFrom
            Dim cto As Integer = _ColumnIndexTo

            Select Case range
                Case range_type.range_clear
                    _DGV.ClearSelection()
                    Return True

                Case range_type.range_all
                    rfrom = 0
                    rto = _RowCount - 1
                    cfrom = cDataStartColumnIndex
                    cto = _ColumnCount

                Case range_type.range_selected
                    _DGV.ClearSelection()
                    For row As Integer = 0 To _RowCount - 1
                        For col As Integer = cDataStartColumnIndex To _ColumnCount
                            If _CellSelected(col, row) Then
                                _DGV.Rows(row).Cells(col).Selected = True
                            End If
                        Next
                    Next
                    Return True

                Case range_type.range_rectangle
                    _DGV.ClearSelection()

                Case range_type.range_square
                    _DGV.ClearSelection()
                    If _RowIndexTo - _RowIndexFrom < _ColumnIndexTo - _ColumnIndexFrom Then
                        '横が大きい
                        Dim diff As Integer = (_ColumnIndexTo - _ColumnIndexFrom) - (_RowIndexTo - _RowIndexFrom)
                        Dim half_diff As Integer = diff / 2
                        cfrom += half_diff
                        cto -= diff - half_diff
                    ElseIf _RowIndexTo - _RowIndexFrom > _ColumnIndexTo - _ColumnIndexFrom Then
                        '縦が大きい
                        Dim diff As Integer = (_RowIndexTo - _RowIndexFrom) - (_ColumnIndexTo - _ColumnIndexFrom)
                        Dim half_diff As Integer = diff / 2
                        rfrom += half_diff
                        rto -= diff - half_diff
                    End If

                Case range_type.range_center
                    _DGV.ClearSelection()
                    rfrom = ((_RowCount + 1) \ 2) - 1
                    rto = ((_RowCount \ 2) + 1) - 1
                    cfrom = (_ColumnCount + 1) \ 2
                    cto = (_ColumnCount \ 2) + 1

                Case Else
                    Return False
            End Select

            For row As Integer = rfrom To rto
                For col As Integer = cfrom To cto
                    _DGV.Rows(row).Cells(col).Selected = True
                Next
            Next
            Return True
        End Function

        Function SubRange() As CSubRange
            '範囲選択かつ2点以上であること
            If Not IsRectangleSelect OrElse CellSelectedCount = 1 Then
                Return Nothing
            End If

            '垂直の番号
            Dim IndexFrom As Integer = _DGV.Rows(_RowIndexFrom).Cells(cDataStartColumnIndex - 1).Value
            Dim IndexTo As Integer = _DGV.Rows(_RowIndexTo).Cells(cDataStartColumnIndex - 1).Value
            If IndexFrom <= IndexTo Then
                Return New CSubRange(_ColumnIndexFrom, _ColumnIndexTo, IndexFrom, IndexTo)
            Else
                Return New CSubRange(_ColumnIndexFrom, _ColumnIndexTo, IndexTo, IndexFrom)
            End If
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0}-{1}:{2}-{3}:{4}/({5},{6})", _ColumnIndexFrom, _ColumnIndexTo, _RowIndexFrom, _RowIndexTo, _CellSelectedCount, _ColumnCount, _RowCount)
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


#Region "コンテキストメニュー"

    Private Sub GridContextMenuStrip_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles GridContextMenuStrip.Opening
        _DataGridSelection.GetSelected()

        '共通
        Me.MenuItemCopy.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemCut.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemPaste.Enabled = ctrDataGridView.IsPastable(dgvひも上下)
        Me.MenuItemDelete.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemCancel.Enabled = True

        '選択
        Me.MenuItemRect.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemSquare.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemCenter.Enabled = _DataGridSelection.IsCellSelected

        '選択の移動
        Me.MenuItemLeftDownMove.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemDownMove.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemRightDownMove.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemLeftMove.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemRightMove.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemLeftUpMove.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemUpMove.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemRightUpMove.Enabled = _DataGridSelection.IsCellSelected

        '選択の拡張
        Me.MenuItemLeftDownExt.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemDownExt.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemRightDownExt.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemLeftExt.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemRightExt.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemLeftUpExt.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemUpExt.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemRightUpExt.Enabled = _DataGridSelection.IsCellSelected

        '操作
        Me.MenuItemON.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemOFF.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemExchange.Enabled = _DataGridSelection.IsCellSelected
    End Sub

    '共通
    Private Sub MenuItemCut_Click(sender As Object, e As EventArgs) Handles MenuItemCut.Click
        ctrDataGridView.SetToClipBoard(dgvひも上下)
        'チェックOFF
        MenuItemOFF_Click(sender, e)
    End Sub
    Private Sub MenuItemCopy_Click(sender As Object, e As EventArgs) Handles MenuItemCopy.Click
        ctrDataGridView.SetToClipBoard(dgvひも上下)
        'クリアしません
    End Sub
    Private Sub MenuItemPaste_Click(sender As Object, e As EventArgs) Handles MenuItemPaste.Click
        'IsNewの行はありません
        ctrDataGridView.DoPaste(dgvひも上下, Nothing)
    End Sub
    Private Sub MenuItemDelete_Click(sender As Object, e As EventArgs) Handles MenuItemDelete.Click
        '行は削除されないのでチェックOFFのみ
        'チェックOFF
        MenuItemOFF_Click(sender, e)
    End Sub
    Private Sub MenuItemCancel_Click(sender As Object, e As EventArgs) Handles MenuItemCancel.Click
        SendKeys.Send("{Esc}")
    End Sub


    'チェックON
    Private Sub MenuItemON_Click(sender As Object, e As EventArgs) Handles MenuItemON.Click
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.SetSelectedCellValue(CDataGridSelection.check_value.check_on)
            BindingSourceひも上下.Current.row.acceptchanges()
        End If
    End Sub

    'チェックOFF
    Private Sub MenuItemOFF_Click(sender As Object, e As EventArgs) Handles MenuItemOFF.Click
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.SetSelectedCellValue(CDataGridSelection.check_value.check_off)
            BindingSourceひも上下.Current.row.acceptchanges()
        End If
    End Sub

    '入れ替え
    Private Sub MenuItemExchange_Click(sender As Object, e As EventArgs) Handles MenuItemExchange.Click
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.SetSelectedCellValue(CDataGridSelection.check_value.check_revert)
            BindingSourceひも上下.Current.row.acceptchanges()
        End If
    End Sub


    '矩形選択
    Private Sub MenuItemRect_Click(sender As Object, e As EventArgs) Handles MenuItemRect.Click
        'Debug.Print("矩形選択")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.SetRangeCellSelected(CDataGridSelection.range_type.range_rectangle)
        End If
    End Sub

    '正方形選択
    Private Sub MenuItemSquare_Click(sender As Object, e As EventArgs) Handles MenuItemSquare.Click
        'Debug.Print("正方形選択")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.SetRangeCellSelected(CDataGridSelection.range_type.range_square)
        End If
    End Sub

    '中心点
    Private Sub MenuItemCenter_Click(sender As Object, e As EventArgs) Handles MenuItemCenter.Click
        'Debug.Print("中心点")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.SetRangeCellSelected(CDataGridSelection.range_type.range_center)
        End If
    End Sub


    '左下移動
    Private Sub MenuItemLeftDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftDownMove.Click
        'Debug.Print("左下移動")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.MoveSelection(-1, 1)
        End If
    End Sub

    '下移動
    Private Sub MenuItemDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemDownMove.Click
        'Debug.Print("下移動")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.MoveSelection(0, 1)
        End If
    End Sub

    '右下移動
    Private Sub MenuItemRightDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightDownMove.Click
        'Debug.Print("右下移動")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.MoveSelection(1, 1)
        End If
    End Sub

    '左移動
    Private Sub MenuItemLeftMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftMove.Click
        'Debug.Print("左移動")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.MoveSelection(-1, 0)
        End If
    End Sub

    '右移動
    Private Sub MenuItemRightMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightMove.Click
        'Debug.Print("右移動")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.MoveSelection(1, 0)
        End If
    End Sub

    '左上移動
    Private Sub MenuItemLeftUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftUpMove.Click
        'Debug.Print("左上移動")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.MoveSelection(-1, -1)
        End If
    End Sub

    '上移動
    Private Sub MenuItemUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemUpMove.Click
        'Debug.Print("上移動")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.MoveSelection(0, -1)
        End If
    End Sub

    '右上移動
    Private Sub MenuItemRightUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightUpMove.Click
        'Debug.Print("右上移動")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.MoveSelection(1, -1)
        End If
    End Sub

    '左下拡張
    Private Sub MenuItemLeftDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftDownExt.Click
        'Debug.Print("左下拡張")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.ExtentSelection(-1, 1)
        End If
    End Sub

    '下拡張
    Private Sub MenuItemDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemDownExt.Click
        'Debug.Print("下拡張")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.ExtentSelection(0, 1)
        End If
    End Sub

    '右下拡張
    Private Sub MenuItemRightDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightDownExt.Click
        'Debug.Print("右下拡張")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.ExtentSelection(1, 1)
        End If
    End Sub

    '左拡張
    Private Sub MenuItemLeftExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftExt.Click
        'Debug.Print("左拡張")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.ExtentSelection(-1, 0)
        End If
    End Sub

    '右拡張
    Private Sub MenuItemRightExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightExt.Click
        'Debug.Print("右拡張")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.ExtentSelection(1, 0)
        End If
    End Sub

    '左上拡張
    Private Sub MenuItemLeftUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftUpExt.Click
        'Debug.Print("左上拡張")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.ExtentSelection(-1, -1)
        End If
    End Sub

    '上拡張
    Private Sub MenuItemUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemUpExt.Click
        'Debug.Print("上拡張")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.ExtentSelection(0, -1)
        End If
    End Sub

    '右上拡張
    Private Sub MenuItemRightUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightUpExt.Click
        'Debug.Print("右上拡張")
        If _DataGridSelection.GetSelected() Then
            _DataGridSelection.ExtentSelection(1, -1)
        End If
    End Sub
#End Region



End Class
