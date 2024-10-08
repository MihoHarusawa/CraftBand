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

    Public ReadOnly Property Is底位置表示 As Boolean
        Get
            Return _Is底位置表示
        End Get
    End Property

    Public Property I開始高さ四角数 As Integer
        Get
            Return _I開始高さ四角数
        End Get
        Set(value As Integer)
            _I開始高さ四角数 = value
            dgvひも上下.Refresh()
        End Set
    End Property

    'Square
    Public Property I水平領域四角数 As Integer '剰余数計算用
    Public Property I垂直領域四角数 As Integer '〃
    Public Property I上右側面本数 As Integer = 0 'Reset時の下左初期値
    'Square45
    Public Property IsSquare45 As Boolean = False
    Public Property I横の四角数 As Integer '配置表示用 45度側(表示中は変わらない)
    Public Property I縦の四角数 As Integer '〃　　　　135度側(〃)
    Dim _I開始高さ四角数 As Integer '周りにプラスされる高さ

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
        '唯一のインスタンス
        _DataGridSelection = New CDataGridSelection(dgvひも上下)
    End Sub

#If DEBUG Then
    Private Sub ctrEditUpDown_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Debug.Print("Resize: Me({0},{1})", Me.Width, Me.Height)
        Debug.Print("     Panel({0},{1})", Panel.Width, Panel.Height)
    End Sub
#End If

    'DataGridViewのカラム表示
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

    '表示を空にする(DataGridViewのBindingSourceひも上下をNothing にする)
    Private Function setDataSourceUpDownNothing() As Boolean
        BindingSourceひも上下.Sort = Nothing
        BindingSourceひも上下.DataSource = Nothing

        txt開始位置.Text = ""

        Return True
    End Function

    '編集内容をセットする(サイズ・BindingSource・カラム・残り数数)
    Private Function setDataSourceUpDown(ByVal updown As clsUpDown) As Boolean
        Try
            If updown Is Nothing Then
                setDataSourceUpDownNothing()

            Else
                nud水平に.Value = updown.HorizontalCount
                nud垂直に.Value = updown.VerticalCount

                'Square45の底表示かどうか
                _Is底位置表示 = IsSquare45 AndAlso (updown.HorizontalCount = I水平領域四角数 AndAlso updown.VerticalCount = I垂直領域四角数)
                'DataGridViewのカラム表示
                setUpdownColumns(updown.HorizontalCount)
                '上下図の残り数
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
        If I開始高さ四角数 < 0 Then
            Return If(value, Color.FromArgb(10, 10, 10), Color.FromArgb(50, 50, 50))
        End If
        If _Is底位置表示 Then
            Dim cat As bottom_category = checkIsInBottom(horzIdx, vertIdx)
            Select Case cat
                Case bottom_category._bottom '底の中
                    Return If(value, Color.FromArgb(160, 160, 160), Color.White)
                Case bottom_category._side '側面
                    Return If(value, Color.FromArgb(140, 140, 140), Color.FromArgb(240, 240, 240))
                Case bottom_category._edge_line '境界線
                    Return If(value, Color.FromArgb(140, 140, 110), Color.FromArgb(240, 240, 210))
                Case bottom_category._center_line '中央線
                    Return If(value, Color.FromArgb(140, 140, 120), Color.FromArgb(240, 240, 220))
                Case Else
                    Return If(value, Color.FromArgb(160, 160, 160), Color.White)
            End Select

        Else
            Return If(value, Color.FromArgb(160, 160, 160), Color.White)
        End If
    End Function

    Enum bottom_category
        _bottom '底
        _edge_line '境界線
        _center_line '中央線
        _side '側面
        _none   '存在しない
        _side_line_left  '側面の辺・左側
        _side_line_right  '側面の辺・右側
    End Enum
    Private Function checkIsInBottom(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As bottom_category
        Dim n左上ライン As Integer = horzIdx + vertIdx
        Dim n右下ライン As Integer = (I水平領域四角数 - horzIdx + 1) + (I垂直領域四角数 - vertIdx + 1)
        Dim n右上ライン As Integer = (I水平領域四角数 - horzIdx + 1) + vertIdx
        Dim n左下ライン As Integer = horzIdx + (I垂直領域四角数 - vertIdx + 1)

        If n左上ライン = I横の四角数 + 1 Then
            Return bottom_category._edge_line
        ElseIf n左上ライン < I横の四角数 + 1 Then
            Return bottom_category._side

        ElseIf n右下ライン = I横の四角数 + 1 Then
            Return bottom_category._edge_line
        ElseIf n右下ライン < I横の四角数 + 1 Then
            Return bottom_category._side

        ElseIf n右上ライン = I縦の四角数 + 1 Then
            Return bottom_category._edge_line
        ElseIf n右上ライン < I縦の四角数 + 1 Then
            Return bottom_category._side

        ElseIf n左下ライン = I縦の四角数 + 1 Then
            Return bottom_category._edge_line
        ElseIf n左下ライン < I縦の四角数 + 1 Then
            Return bottom_category._side

        Else
            If getIndexPosition(horzIdx) = 0 AndAlso getIndexPosition(vertIdx) = 0 Then
                If (I横の四角数 < I縦の四角数) AndAlso (horzIdx = vertIdx) Then
                    Return bottom_category._center_line
                ElseIf (I横の四角数 > I縦の四角数) AndAlso (horzIdx = (I垂直領域四角数 - vertIdx + 1)) Then
                    Return bottom_category._center_line
                End If
            End If
            Return bottom_category._bottom '底
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

        'setDataSourceUpDownNothing()#53

        Dim updown As clsUpDown = _Calc.updownサイズ変更(nud水平に.Value, nud垂直に.Value, True)
        '処理結果のUpDownを表示
        setDataSourceUpDownResult(sender, updown)
    End Sub

    Private Sub btnクリア_Click(sender As Object, e As EventArgs) Handles btnクリア.Click
        Dim isReset As Boolean = True 'サイズも初期化
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

        If Not isReset Then
            '現在の全範囲のチェックをオフにする
            _DataGridSelection.GetDataGridSelected() 'ショートカットキー対策
            _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_all) '全体
            'DataGridの値を変更する処理
            gridValueChangeOperation(sender, CDataGridValues.change_operation.chg_OFF)
            Exit Sub
        End If


        'サイズも初期化
        Dim updown As clsUpDown = _Calc.updownリセット(I上右側面本数)
        '処理結果のUpDownを表示
        setDataSourceUpDownResult(sender, updown)
    End Sub

    Private Sub btn上下交換_Click(sender As Object, e As EventArgs) Handles btn上下交換.Click
        Dim target As select_target = getTargetSelect(sender)
        If target = select_target.target_none Then
            Exit Sub
        End If

        '矩形/全体
        If target = select_target.target_all Then
            _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_all) '全体化
        End If
        'DataGridの値を変更する処理
        gridValueChangeOperation(sender, CDataGridValues.change_operation.chg_Negate)
    End Sub

    Private Sub btn左右反転_Click(sender As Object, e As EventArgs) Handles btn左右反転.Click
        Dim target As select_target = getTargetSelect(sender)
        If target = select_target.target_none Then
            Exit Sub
        End If

        '矩形/全体
        If target = select_target.target_all Then
            _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_all) '全体化
        End If
        'DataGridの値を変更する処理
        gridValueChangeOperation(sender, CDataGridValues.change_operation.chg_LeftSideRight)
    End Sub

    Private Sub btn天地反転_Click(sender As Object, e As EventArgs) Handles btn天地反転.Click
        Dim target As select_target = getTargetSelect(sender)
        If target = select_target.target_none Then
            Exit Sub
        End If
        '矩形/全体
        If target = select_target.target_all Then
            _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_all) '全体化
        End If
        'DataGridの値を変更する処理
        gridValueChangeOperation(sender, CDataGridValues.change_operation.chg_UpSideDown)
    End Sub

    Private Sub btn右回転_Click(sender As Object, e As EventArgs) Handles btn右回転.Click
        Dim target As select_target = getTargetSelect(sender, True)
        If target = select_target.target_none Then
            Exit Sub
        End If

        '矩形/全体
        If target = select_target.target_all Then
            _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_all) '全体化
        End If
        If _DataGridSelection.IsSquareSelect Then
            'DataGridの値を変更する処理
            gridValueChangeOperation(sender, CDataGridValues.change_operation.chg_RotateRight)
            Exit Sub
        End If

        '全体が対象かつサイズが変わる
        Dim desc As Boolean = _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace)
        Dim updown As clsUpDown = _Calc.updown右回転(desc)
        '処理結果のUpDownを表示
        setDataSourceUpDownResult(sender, updown)
    End Sub

    Private Sub btn左回転_Click(sender As Object, e As EventArgs) Handles btn左回転.Click
        Dim target As select_target = getTargetSelect(sender, True)
        If target = select_target.target_none Then
            Exit Sub
        End If

        '矩形/全体
        If target = select_target.target_all Then
            _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_all) '全体化
        End If
        If _DataGridSelection.IsSquareSelect Then
            'DataGridの値を変更する処理
            gridValueChangeOperation(sender, CDataGridValues.change_operation.chg_RotateLeft)
            Exit Sub
        End If

        '全体が対象かつサイズが変わる
        Dim desc As Boolean = _Is側面_下から上へ AndAlso IsSide(_CurrentTargetFace)
        Dim updown As clsUpDown = _Calc.updown右回転(Not desc)
        '処理結果のUpDownを表示
        setDataSourceUpDownResult(sender, updown)
    End Sub

    Private Sub btnシフト_Click(sender As Object, e As EventArgs) Handles btnシフト.Click
        Dim target As select_target = getTargetSelect2(sender)
        If target = select_target.target_none Then
            Exit Sub
        End If

        '矩形/全体
        Dim no_loop As Boolean = False
        If target = select_target.target_all Then
            _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_all) '全体化
        ElseIf target = select_target.target_no_loop Then
            no_loop = True
        End If

        Dim op As CDataGridValues.change_operation
        Dim delta As Integer
        Dim add_selection As Boolean = False '#38
        Dim dataGridSelectionSave As CDataGridSelection = _DataGridSelection.Clone
        If rad上.Checked Then
            op = CDataGridValues.change_operation.chg_ShiftDown
            delta = -1
            If no_loop Then
                op = CDataGridValues.change_operation.chg_ShiftDown_nl
                add_selection = _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_add_upper)
            End If
        ElseIf rad下.Checked Then
            op = CDataGridValues.change_operation.chg_ShiftDown
            delta = 1
            If no_loop Then
                op = CDataGridValues.change_operation.chg_ShiftDown_nl
                add_selection = _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_add_lower)
            End If
        ElseIf rad右.Checked Then
            op = CDataGridValues.change_operation.chg_ShiftRight
            delta = 1
            If no_loop Then
                op = CDataGridValues.change_operation.chg_ShiftRight_nl
                add_selection = _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_add_right)
            End If
        ElseIf rad左.Checked Then
            op = CDataGridValues.change_operation.chg_ShiftRight
            delta = -1
            If no_loop Then
                op = CDataGridValues.change_operation.chg_ShiftRight_nl
                add_selection = _DataGridSelection.ChangeRangeSelected(CDataGridSelection.range_type.range_add_left)
            End If
        Else
            Exit Sub
        End If

        'DataGridの値を変更する処理
        gridValueChangeOperation(sender, op, delta)

        '矩形領域の選択変更
        If no_loop Then 'AndAlso add_selection
            If rad上.Checked Then
                dataGridSelectionSave.MoveSelection(0, -1)
            ElseIf rad下.Checked Then
                dataGridSelectionSave.MoveSelection(0, 1)
            ElseIf rad右.Checked Then
                dataGridSelectionSave.MoveSelection(1, 0)
            ElseIf rad左.Checked Then
                dataGridSelectionSave.MoveSelection(-1, 0)
            End If
        End If
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

        setDataSourceUpDownNothing()
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

        '処理結果のUpDownを表示
        setDataSourceUpDownResult(sender, updown)
    End Sub

    Private Sub btnランダム_Click(sender As Object, e As EventArgs) Handles btnランダム.Click
        setDataSourceUpDownNothing()

        Dim updown As clsUpDown = _Calc.updownランダム()
        '処理結果のUpDownを表示
        setDataSourceUpDownResult(sender, updown)
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


    '選択状態を操作対象としてどう認識したか
    Enum select_target
        target_none         '非対象
        target_all          '全体
        target_rectangle    '矩形
        target_square       '正方形
        '
        target_no_loop      '一方向シフト
    End Enum

    '矩形選択されていれば無条件で矩形(正方形)が対象、以外は全体が対象であることを問い合わせる
    '戻り値: target_none=キャンセル/不可  target_all=全体対象  target_rectangle=矩形対象 target_square=正方形対象
    Private Function getTargetSelect(sender As Object, Optional ByVal isSquare As Boolean = False) As select_target
        If _DataGridSelection Is Nothing OrElse Not _DataGridSelection.IsValid Then
            Return select_target.target_none
        End If
        '選択範囲取得。選択なしもあり得る
        _DataGridSelection.GetDataGridSelected()

        '全体確定: 1セルもしくは選択なし
        If _DataGridSelection.CellSelectedCount <= 1 Then
            Return select_target.target_all
        End If

        '正方形/矩形で確定
        If isSquare AndAlso _DataGridSelection.IsSquareSelect Then
            Return select_target.target_square
        End If
        If Not isSquare AndAlso _DataGridSelection.IsRectangleSelect Then
            Return select_target.target_rectangle
        End If

        '以外の選択は全体が対象になることを問い合わせる

        '現在の選択では{0}できません。全範囲を{0}してよろしいですか？
        If MessageBox.Show(String.Format(My.Resources.AskTargetAllRange, ControlText(sender)),
                            FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
            Return select_target.target_all
        Else
            Return select_target.target_none
        End If
    End Function

    '2サイズ以上の矩形選択されていれば、一方向シフトか循環シフトかをを問い合わせる。以外は全体
    '戻り値: target_none=キャンセル/不可  target_all=全体対象  target_rectangle=矩形循環 target_no_loop=一方向シフト
    Private Function getTargetSelect2(sender As Object) As select_target
        If _DataGridSelection Is Nothing OrElse Not _DataGridSelection.IsValid Then
            Return select_target.target_none
        End If
        '選択範囲取得。選択なしもあり得る
        _DataGridSelection.GetDataGridSelected()

        '全体確定: 1セルもしくは選択なしもしくは非矩形
        If _DataGridSelection.CellSelectedCount <= 1 OrElse Not _DataGridSelection.IsRectangleSelect Then
            Return select_target.target_all
        End If
        If _DataGridSelection.RectangleWidth < 2 OrElse _DataGridSelection.RectangleHight < 2 Then
            Return select_target.target_all
        End If

        '選択された領域を循環{0}しますか？　(はい=循環,いいえ=一方向)
        Dim ret As DialogResult = MessageBox.Show(String.Format(My.Resources.AskTargetRectangle, ControlText(sender)),
                    FormCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
        If ret = DialogResult.Yes Then
            Return select_target.target_rectangle
        ElseIf ret = DialogResult.no Then
            Return select_target.target_no_loop '(一方向指定)
        End If
        Return select_target.target_none
    End Function


    'DataGridの値を変更する処理
    Private Function gridValueChangeOperation(sender As Object, ByVal op As CDataGridValues.value_op, Optional ByVal delta As Integer = 0) As Boolean
        Dim vals As New CDataGridValues(_DataGridSelection)
        If vals.ChangeChecked(op, delta) Then
            vals.SetDataGridValuesOfSelected(CDataGridValues.value_op.value)
            _UpdownChanged = True
            Return True
        Else
            '{0}できませんでした。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownNop, ControlText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False '処理は失敗したが表示は変わらない
        End If
    End Function

    '処理結果のUpDownを表示する処理
    Private Function setDataSourceUpDownResult(sender As Object, ByVal updown As clsUpDown) As Boolean
        Dim ret As Boolean = False
        If updown Is Nothing Then
            setDataSourceUpDownNothing()

            '{0}できませんでした。クリア(初期化)してやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, ControlText(sender)),
                            FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            ret = setDataSourceUpDown(updown)
        End If
        '
        dgvひも上下.Refresh()
        _UpdownChanged = True
        Return ret
    End Function


#End Region

    'テーブルの作り直しを伴う処理/clsUpDown側の処理呼び出し
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

        Function updownリセット(ByVal sidecount As Integer) As clsUpDown
            If Not _CUpDown.Reset(sidecount) Then
                Return Nothing
            End If
            Return _CUpDown
        End Function

        Function updown右回転(ByVal desc As Boolean) As clsUpDown
            If Not desc Then
                '右回転
                If Not _CUpDown.RotateRight(True) Then
                    Return Nothing
                End If
            Else
                '左回転
                If Not _CUpDown.RotateLeft(True) Then
                    Return Nothing
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
        If dlg.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

        setDataSourceUpDownNothing()
        txt上下のメモ.Text = updown_tmp.Memo

        'CheckBoxTableに結果が入っている
        Dim updown As clsUpDown = _Calc.updownサイズ変更(updown_tmp.HorizontalCount, updown_tmp.VerticalCount, True)
        '処理結果のUpDownを表示
        setDataSourceUpDownResult(sender, updown)
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
        Friend _DGV As DataGridView = Nothing

        '選択の最小と最大
        Friend _ColumnIndexFrom As Integer = Integer.MaxValue
        Friend _ColumnIndexTo As Integer = Integer.MinValue
        Friend _RowIndexFrom As Integer = Integer.MaxValue
        Friend _RowIndexTo As Integer = Integer.MinValue

        '表示されているColumnとRowの数
        Friend _ColumnCount As Integer = 0 '表示列のみ 0は非表示なので1～_ColumnCount
        Friend _RowCount As Integer = 0 '行はこの数 0～_RowCount-1

        'セルの選択状態
        Friend _CellSelected(cMaxUpdownColumns, cMaxUpdownColumns) As Boolean
        Friend _CellSelectedCount As Integer = 0


        Sub New(ByVal dgv As DataGridView)
            _DGV = dgv
        End Sub

        '使用可能
        ReadOnly Property IsValid As Boolean
            Get
                Return _DGV IsNot Nothing
            End Get
        End Property

        '状態保存用の複製
        Function Clone() As CDataGridSelection
            If Not IsValid Then
                Return Nothing
            End If
            Dim dup As New CDataGridSelection(_DGV)
            dup._ColumnIndexFrom = _ColumnIndexFrom
            dup._ColumnIndexTo = _ColumnIndexTo
            dup._RowIndexFrom = _RowIndexFrom
            dup._RowIndexTo = _RowIndexTo

            dup._ColumnCount = _ColumnCount
            dup._RowCount = _RowCount

            Array.Copy(_CellSelected, dup._CellSelected, _CellSelected.Length)
            dup._CellSelectedCount = _CellSelectedCount

            Return dup
        End Function


        '範囲が指定され使用可能な状態であること(GetSelected()が呼ばれている)
        ReadOnly Property IsValidArea As Boolean
            Get
                Return IsValid AndAlso cDataStartColumnIndex < _ColumnCount AndAlso 0 < _RowCount
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
                Return IsValidArea AndAlso 0 < _CellSelectedCount AndAlso
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

        '矩形の幅
        ReadOnly Property RectangleWidth As Integer
            Get
                Return If(IsCellSelected, _ColumnIndexTo - _ColumnIndexFrom + 1, 0)
            End Get
        End Property

        '矩形の高さ
        ReadOnly Property RectangleHight As Integer
            Get
                Return If(IsCellSelected, _RowIndexTo - _RowIndexFrom + 1, 0)
            End Get
        End Property

        '正方形に選択されている
        ReadOnly Property IsSquareSelect As Boolean
            Get
                Return IsRectangleSelect AndAlso RectangleWidth = RectangleHight
            End Get
        End Property

        '全体が選択されている
        ReadOnly Property IsAllSelect As Boolean
            Get
                Return IsCellSelected AndAlso
                    _CellSelectedCount = (_ColumnCount - cDataStartColumnIndex + 1) * (_RowCount - 0 + 1)
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

        'セルの選択をTrueにセット(表示かつ対象範囲内のみ)
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


        'DataGridViewで選択されているセルを配列に読み取る
        '戻り値: True=1点以上の選択がある　False=状態が不正もしくは選択がない
        Function GetDataGridSelected() As Boolean
            If Not IsValid Then
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

        '選択を移動する ※配列の値を変更しDataGridViewに反映させる
        Function MoveSelection(ByVal col_ref As Integer, ByVal row_ref As Integer) As Boolean
            If Not IsValidArea OrElse (col_ref = 0 AndAlso row_ref = 0) Then
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

            Return SetDataGridRangeSelected(range_type.range_selected)
        End Function

        '選択を拡張する ※配列の値を変更しDataGridViewに反映させる
        Function ExtentSelection(ByVal col_ref As Integer, ByVal row_ref As Integer) As Boolean
            If Not IsValidArea OrElse (col_ref = 0 AndAlso row_ref = 0) Then
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

            Return SetDataGridRangeSelected(range_type.range_selected)
        End Function


        Enum range_type
            range_clear '選択クリア
            range_all '全体
            range_center '全体の中心

            range_selected '選択セルのみ
            range_rectangle '選択セルを含む矩形領域
            range_square '選択セルを含む矩形領域に含まれる正方形

            range_check_on 'チェックONのセル
            range_check_off 'チェックOFFのセル

            '矩形領域が選択されている前提
            range_add_upper '上の行へ
            range_add_lower '下の行へ
            range_add_left '左の列へ
            range_add_right '右の列へ
        End Enum

        'DataGridView指定範囲のセルを選択する ※メンバー変数値は変わりません
        Function SetDataGridRangeSelected(ByVal range As range_type) As Boolean

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
                    '_DGV.ClearSelection()
                    For row As Integer = 0 To _RowCount - 1
                        For col As Integer = cDataStartColumnIndex To _ColumnCount
                            _DGV.Rows(row).Cells(col).Selected = _CellSelected(col, row)
                        Next
                    Next
                    Return True

                Case range_type.range_rectangle
                    '_DGV.ClearSelection()

                Case range_type.range_square
                    '_DGV.ClearSelection()
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
                    '_DGV.ClearSelection()
                    rfrom = ((_RowCount + 1) \ 2) - 1
                    rto = ((_RowCount \ 2) + 1) - 1
                    cfrom = (_ColumnCount + 1) \ 2
                    cto = (_ColumnCount \ 2) + 1

                Case range_type.range_check_on, range_type.range_check_off
                    Dim read As New CDataGridValues(Me)
                    For row As Integer = 0 To _RowCount - 1
                        For col As Integer = cDataStartColumnIndex To _ColumnCount
                            If range = range_type.range_check_on Then
                                _DGV.Rows(row).Cells(col).Selected = read._CellChecked(col, row)
                            ElseIf range = range_type.range_check_off Then
                                _DGV.Rows(row).Cells(col).Selected = Not read._CellChecked(col, row)
                            End If
                        Next
                    Next
                    Return True

                Case Else
                    Return False
            End Select

            '矩形領域
            For row As Integer = 0 To _RowCount - 1
                For col As Integer = cDataStartColumnIndex To _ColumnCount
                    If rfrom <= row AndAlso row <= rto AndAlso cfrom <= col AndAlso col <= cfrom Then
                        _DGV.Rows(row).Cells(col).Selected = True
                    Else
                        _DGV.Rows(row).Cells(col).Selected = False
                    End If
                Next
            Next
            Return True
        End Function


        '配列を含む選択状態のメンバー変数値を変更する ※DataGridViewは変わりません
        '変更した場合Trueを返す(厳密ではない)
        Function ChangeRangeSelected(ByVal range As range_type) As Boolean
            Select Case range
                Case range_type.range_clear
                    ClearCellSelected()
                    Return True

                Case range_type.range_all
                    '追記
                    For row As Integer = 0 To _RowCount - 1
                        For col As Integer = cDataStartColumnIndex To _ColumnCount
                            setCellSelected(col, row)
                        Next
                    Next

                Case range_type.range_selected
                    '変わりません
                    Return False

                Case range_type.range_rectangle
                    '追記
                    For row As Integer = _RowIndexFrom To _RowIndexTo
                        For col As Integer = _ColumnIndexFrom To _ColumnIndexTo
                            setCellSelected(col, row)
                        Next
                    Next

                Case range_type.range_add_upper '矩形領域が選択されている前提で、画面上の行に広げる
                    '追記
                    Dim rowadd As Integer = _RowIndexFrom - 1
                    If rowadd < 0 Then
                        Return False
                    End If
                    For col As Integer = _ColumnIndexFrom To _ColumnIndexTo
                        setCellSelected(col, rowadd)
                    Next

                Case range_type.range_add_lower '矩形領域が選択されている前提で、画面下の行に広げる
                    '追記
                    Dim rowadd As Integer = _RowIndexTo + 1
                    If _RowCount < rowadd Then
                        Return False
                    End If
                    For col As Integer = _ColumnIndexFrom To _ColumnIndexTo
                        setCellSelected(col, rowadd)
                    Next

                Case range_type.range_add_left '矩形領域が選択されている前提で、画面左の列に広げる
                    '追記
                    Dim coladd As Integer = _ColumnIndexFrom - 1
                    If coladd < cDataStartColumnIndex Then
                        Return False
                    End If
                    For row As Integer = _RowIndexFrom To _RowIndexTo
                        setCellSelected(coladd, row)
                    Next


                Case range_type.range_add_right '矩形領域が選択されている前提で、画面右の列に広げる
                    '追記
                    Dim coladd As Integer = _ColumnIndexTo + 1
                    For row As Integer = _RowIndexFrom To _RowIndexTo
                        If _ColumnCount < coladd Then
                            Return False
                        End If
                        setCellSelected(coladd, row)
                    Next

                Case range_type.range_square
                    Throw New Exception("CDataGridSelection.ChangeRangeSelected:range_type.range_square No Supported")
                Case range_type.range_center
                    Throw New Exception("CDataGridSelection.ChangeRangeSelected:range_type.range_center No Supported")

                Case Else
                    Return False
            End Select
            Return True
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0}-{1}:{2}-{3}:{4}/({5},{6})", _ColumnIndexFrom, _ColumnIndexTo, _RowIndexFrom, _RowIndexTo, _CellSelectedCount, _ColumnCount, _RowCount)
        End Function
    End Class

    'DataGridViewの値の操作
    Private Class CDataGridValues

        Dim _Selection As CDataGridSelection

        'セルの値
        Friend _CellChecked(cMaxUpdownColumns, cMaxUpdownColumns) As Boolean
        Friend _CellCheckedCount As Integer = 0


        Sub New(ByVal sel As CDataGridSelection)
            _Selection = sel
            'チェック値を読み取る
            getDataGridValues()
        End Sub

        '使用可能
        ReadOnly Property IsValid As Boolean
            Get
                Return _Selection IsNot Nothing AndAlso _Selection.IsValidArea
            End Get
        End Property

        'カレント行が編集中ならそれを確定する
        Private Sub accept_row_changes()
            If IsValid Then
                If _Selection._DGV.DataSource IsNot Nothing AndAlso _Selection._DGV.DataSource.Current IsNot Nothing AndAlso _Selection._DGV.DataSource.Current.row IsNot Nothing Then
                    _Selection._DGV.DataSource.Current.row.acceptchanges()
                End If
            End If
        End Sub

        'セルの選択をセット(表示かつ対象範囲内のみ)
        Private Sub setCellChecked(ByVal col As Integer, row As Integer)
            If col < cDataStartColumnIndex OrElse _Selection._ColumnCount < col OrElse row < 0 OrElse _Selection._RowCount <= row Then
                Exit Sub
            End If
            If Not _CellChecked(col, row) Then
                _CellChecked(col, row) = True
                _CellCheckedCount += 1
            End If
        End Sub

        'DataGridViewで表示されているセル値を読み取る
        '戻り値: True=読み取れた　False=状態が不正もしくは選択がない
        Private Function getDataGridValues() As Boolean
            If Not IsValid Then
                Return False
            End If

            Array.Clear(_CellChecked, False, _CellChecked.Length)
            _CellCheckedCount = 0

            accept_row_changes()
            For row As Integer = 0 To _Selection._RowCount - 1
                For col As Integer = cDataStartColumnIndex To _Selection._ColumnCount
                    If _Selection._DGV.Rows(row).Cells(col).Value Then
                        setCellChecked(col, row)
                    End If
                Next
            Next
            Return True
        End Function


        '操作内容　※Asc/Descによらず表示通りの上下左右で操作する
        Enum change_operation
            chg_None         'なし            －            delta値
            chg_ON            '全てON         形状自由
            chg_OFF           '全てOFF        形状自由
            chg_Negate        'Not値          形状自由       
            chg_LeftSideRight '左右反転       矩形   
            chg_UpSideDown    '上下反転       矩形  
            chg_RotateRight   '右回転　       正方形 
            chg_RotateLeft    '左回転         正方形
            chg_ShiftRight    '右シフト       矩形          プラスは右、マイナスは左
            chg_ShiftDown     '下シフト       矩形          プラスは下、マイナスは上
            chg_ShiftRight_nl '右シフト(一方向) 矩形        〃
            chg_ShiftDown_nl  '下シフト(一方向) 矩形        〃
        End Enum


        '_CellCheckedの値を変更する 対象は_Selection(CDataGridSelection)
        Function ChangeChecked(ByVal op As change_operation, Optional delta As Integer = 0) As Boolean
            Select Case op
                Case change_operation.chg_ON        '全てON  
                    Return changeCheckedValue(value_op._on)

                Case change_operation.chg_OFF        '全てOFF  
                    Return changeCheckedValue(value_op._off)

                Case change_operation.chg_Negate        'Not値  
                    Return changeCheckedValue(value_op.value_negate)

                Case change_operation.chg_LeftSideRight '左右反転
                    Return changeCheckedLeftSideRight()

                Case change_operation.chg_UpSideDown    '上下反転
                    Return changeCheckedUpSideDown()

                Case change_operation.chg_RotateRight   '右回転　
                    Return changeCheckedRotateRight()

                Case change_operation.chg_RotateLeft    '左回転 
                    Return changeCheckedRotateLeft()

                Case change_operation.chg_ShiftRight    '右シフト 
                    Return changeCheckedShiftRight(delta, False)

                Case change_operation.chg_ShiftDown    '下シフト 
                    Return changeCheckedShiftDown(delta, False)

                Case change_operation.chg_ShiftRight_nl    '右シフト(一方向) 
                    Return changeCheckedShiftRight(delta, True)

                Case change_operation.chg_ShiftDown_nl    '下シフト(一方向)  
                    Return changeCheckedShiftDown(delta, True)

                Case Else 'op_None 何もしない  
                    Return False
            End Select
        End Function


        '選択対象に指定の値をセット
        Private Function changeCheckedValue(ByVal vop As value_op) As Boolean
            If _Selection.CellSelectedCount = 0 Then
                Return False
            End If

            With _Selection
                For row As Integer = 0 To ._RowCount - 1
                    For col As Integer = cDataStartColumnIndex To ._ColumnCount
                        If ._CellSelected(col, row) Then

                            If vop = value_op._on Then
                                _CellChecked(col, row) = True
                            ElseIf vop = value_op._off Then
                                _CellChecked(col, row) = False
                            ElseIf vop = value_op.value Then
                                'そのまま
                            ElseIf vop = value_op.value_negate Then
                                _CellChecked(col, row) = Not _CellChecked(col, row)
                            End If

                        End If
                    Next
                Next
            End With
            Return True
        End Function

        '左右反転・矩形対象
        Private Function changeCheckedLeftSideRight() As Boolean
            If Not _Selection.IsRectangleSelect Then
                Return False
            End If
            Dim save(,) As Boolean = DirectCast(_CellChecked.Clone(), Boolean(,))
            With _Selection
                For row As Integer = ._RowIndexFrom To ._RowIndexTo
                    For col As Integer = ._ColumnIndexFrom To ._ColumnIndexTo
                        _CellChecked(col, row) = save(._ColumnIndexTo - (col - ._ColumnIndexFrom), row)
                    Next
                Next

            End With
            Return True
        End Function

        '上下反転・矩形対象
        Private Function changeCheckedUpSideDown() As Boolean
            If Not _Selection.IsRectangleSelect Then
                Return False
            End If
            Dim save(,) As Boolean = DirectCast(_CellChecked.Clone(), Boolean(,))
            With _Selection
                For row As Integer = ._RowIndexFrom To ._RowIndexTo
                    For col As Integer = ._ColumnIndexFrom To ._ColumnIndexTo
                        _CellChecked(col, row) = save(col, ._RowIndexTo - (row - ._RowIndexFrom))
                    Next
                Next
            End With
            Return True
        End Function

        '右回転・正方形対象
        Private Function changeCheckedRotateRight() As Boolean
            If Not _Selection.IsSquareSelect Then
                Return False
            End If
            Dim save(,) As Boolean = DirectCast(_CellChecked.Clone(), Boolean(,))
            With _Selection
                For row As Integer = ._RowIndexFrom To ._RowIndexTo
                    For col As Integer = ._ColumnIndexFrom To ._ColumnIndexTo
                        _CellChecked(col, row) = save(._ColumnIndexFrom + row - ._RowIndexFrom, ._RowIndexTo - (col - ._ColumnIndexFrom))
                    Next
                Next

            End With
            Return True
        End Function

        '右回転・正方形対象
        Private Function changeCheckedRotateLeft() As Boolean
            If Not _Selection.IsSquareSelect Then
                Return False
            End If
            Dim save(,) As Boolean = DirectCast(_CellChecked.Clone(), Boolean(,))
            With _Selection
                For row As Integer = ._RowIndexFrom To ._RowIndexTo
                    For col As Integer = ._ColumnIndexFrom To ._ColumnIndexTo
                        _CellChecked(col, row) = save(._ColumnIndexTo - (row - ._RowIndexFrom), ._RowIndexFrom + (col - ._ColumnIndexFrom))
                    Next
                Next
            End With
            Return True
        End Function


        '右シフト・矩形対象 
        Private Function changeCheckedShiftRight(ByVal delta As Integer, ByVal no_loop As Boolean) As Boolean
            If Not _Selection.IsRectangleSelect Then
                Return False
            End If
            Dim save(,) As Boolean = DirectCast(_CellChecked.Clone(), Boolean(,))
            With _Selection
                If Modulo(delta, .RectangleWidth) = 0 Then
                    Return True '同じになる
                End If
                For row As Integer = ._RowIndexFrom To ._RowIndexTo
                    If no_loop Then
                        '一方向
                        For x As Integer = 0 To .RectangleWidth - 1
                            _CellChecked(._ColumnIndexFrom + x, row) = False
                        Next
                        For x As Integer = 0 To .RectangleWidth - 1
                            Dim xdelta As Integer = (x + delta)
                            If 0 <= xdelta AndAlso xdelta < .RectangleWidth Then
                                _CellChecked(._ColumnIndexFrom + xdelta, row) = save(._ColumnIndexFrom + x, row)
                            End If
                        Next
                    Else
                        '循環
                        For x As Integer = 0 To .RectangleWidth - 1
                            Dim xdelta As Integer = Modulo((x + delta), .RectangleWidth)
                            'Debug.Print("({0},[{1}]) ← ({2},[{3}])", ._ColumnIndexFrom + xdelta, row, ._ColumnIndexFrom + x, row)
                            _CellChecked(._ColumnIndexFrom + xdelta, row) = save(._ColumnIndexFrom + x, row)
                        Next
                    End If
                Next
                Return True
            End With
        End Function

        '下シフト・矩形対象
        Private Function changeCheckedShiftDown(ByVal delta As Integer, ByVal no_loop As Boolean) As Boolean
            If Not _Selection.IsRectangleSelect Then
                Return False
            End If
            Dim save(,) As Boolean = DirectCast(_CellChecked.Clone(), Boolean(,))
            With _Selection
                If Modulo(delta, .RectangleHight) = 0 Then
                    Return True '同じになる
                End If
                For col As Integer = ._ColumnIndexFrom To ._ColumnIndexTo
                    If no_loop Then
                        '一方向
                        For y As Integer = 0 To .RectangleHight - 1
                            _CellChecked(col, ._RowIndexFrom + y) = False
                        Next
                        For y As Integer = 0 To .RectangleHight - 1
                            Dim ydelta As Integer = (y + delta)
                            If 0 <= ydelta AndAlso ydelta < .RectangleHight Then
                                _CellChecked(col, ._RowIndexFrom + ydelta) = save(col, ._RowIndexFrom + y)
                            End If
                        Next
                    Else
                        For y As Integer = 0 To .RectangleHight - 1
                            Dim ydelta As Integer = Modulo((y + delta), .RectangleHight)
                            'Debug.Print("([{0}],{1}) ← ([{2}],{3})", col, ._RowIndexFrom + ydelta, col, ._RowIndexFrom + y)
                            _CellChecked(col, ._RowIndexFrom + ydelta) = save(col, ._RowIndexFrom + y)
                        Next
                    End If
                Next
                Return True
            End With
        End Function


        'セットする値の指定
        Enum value_op
            _on             'True固定
            _off            'False固定
            value           'その値(_CellChecked)
            value_negate    '否定値(Not _CellChecked)
        End Enum

        'DataGridViewの値の変更　
        '対象セルは_Selection(CDataGridSelection) 値は_CellCheckedを元に指定の操作
        Function SetDataGridValuesOfSelected(Optional ByVal vop As value_op = value_op.value) As Boolean
            If Not _Selection.IsCellSelected Then
                Return False
            End If
            For row As Integer = 0 To _Selection._RowCount - 1
                For col As Integer = cDataStartColumnIndex To _Selection._ColumnCount
                    If _Selection._CellSelected(col, row) Then
                        If vop = value_op._on Then
                            _Selection._DGV.Rows(row).Cells(col).Value = True
                        ElseIf vop = value_op._off Then
                            _Selection._DGV.Rows(row).Cells(col).Value = False
                        ElseIf vop = value_op.value Then
                            _Selection._DGV.Rows(row).Cells(col).Value = _CellChecked(col, row)
                        ElseIf vop = value_op.value_negate Then
                            _Selection._DGV.Rows(row).Cells(col).Value = Not _CellChecked(col, row)
                        End If
                    End If
                Next
            Next
            accept_row_changes()

            Return True
        End Function


    End Class



#Region "コンテキストメニュー"

    Private Sub GridContextMenuStrip_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles GridContextMenuStrip.Opening
        _DataGridSelection.GetDataGridSelected()

        '共通
        Me.MenuItemCopy.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemCut.Enabled = ctrDataGridView.RowOrCellSelected(dgvひも上下)
        Me.MenuItemPaste.Enabled = ctrDataGridView.IsPastable(dgvひも上下)
        Me.MenuItemDelete.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemCancel.Enabled = True

        '選択
        Me.MenuItemRect.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemSquare.Enabled = _DataGridSelection.IsCellSelected
        Me.MenuItemCenter.Enabled = _DataGridSelection.IsValidArea
        Me.MenuItemSelectON.Enabled = _DataGridSelection.IsValidArea
        Me.MenuItemSelectOFF.Enabled = _DataGridSelection.IsValidArea

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
        'Fix
        If dgvひも上下.DataSource IsNot Nothing AndAlso dgvひも上下.DataSource.Current IsNot Nothing AndAlso dgvひも上下.DataSource.Current.row IsNot Nothing Then
            dgvひも上下.DataSource.Current.row.acceptchanges()
        End If
    End Sub
    Private Sub MenuItemDelete_Click(sender As Object, e As EventArgs) Handles MenuItemDelete.Click
        '行は削除されないのでチェックOFFのみ
        'チェックOFF
        MenuItemOFF_Click(sender, e)
    End Sub
    Private Sub MenuItemCancel_Click(sender As Object, e As EventArgs) Handles MenuItemCancel.Click
        SendKeys.Send("{Esc}")
    End Sub


    '選択をON
    Private Sub MenuItemON_Click(sender As Object, e As EventArgs) Handles MenuItemON.Click
        If _DataGridSelection.GetDataGridSelected() Then
            Dim vals As New CDataGridValues(_DataGridSelection)
            vals.SetDataGridValuesOfSelected(CDataGridValues.value_op._on)
        End If
    End Sub

    '選択をOFF
    Private Sub MenuItemOFF_Click(sender As Object, e As EventArgs) Handles MenuItemOFF.Click
        If _DataGridSelection.GetDataGridSelected() Then
            Dim vals As New CDataGridValues(_DataGridSelection)
            vals.SetDataGridValuesOfSelected(CDataGridValues.value_op._off)
        End If
    End Sub

    '選択を入替
    Private Sub MenuItemExchange_Click(sender As Object, e As EventArgs) Handles MenuItemExchange.Click
        If _DataGridSelection.GetDataGridSelected() Then
            Dim vals As New CDataGridValues(_DataGridSelection)
            vals.SetDataGridValuesOfSelected(CDataGridValues.value_op.value_negate)
        End If
    End Sub


    '選択矩形化
    Private Sub MenuItemRect_Click(sender As Object, e As EventArgs) Handles MenuItemRect.Click
        'Debug.Print("選択矩形化")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.SetDataGridRangeSelected(CDataGridSelection.range_type.range_rectangle)
        End If
    End Sub

    '正方形化
    Private Sub MenuItemSquare_Click(sender As Object, e As EventArgs) Handles MenuItemSquare.Click
        'Debug.Print("正方形化")
        If _DataGridSelection.GetDataGridSelected Then
            _DataGridSelection.SetDataGridRangeSelected(CDataGridSelection.range_type.range_square)
        End If
    End Sub

    '中心を選択
    Private Sub MenuItemCenter_Click(sender As Object, e As EventArgs) Handles MenuItemCenter.Click
        'Debug.Print("中心を選択")
        If _DataGridSelection.IsValidArea() Then
            _DataGridSelection.SetDataGridRangeSelected(CDataGridSelection.range_type.range_center)
        End If
    End Sub

    'ONを選択
    Private Sub MenuItemSelectON_Click(sender As Object, e As EventArgs) Handles MenuItemSelectON.Click
        Debug.Print("ONを選択")
        If _DataGridSelection.IsValidArea() Then
            _DataGridSelection.SetDataGridRangeSelected(CDataGridSelection.range_type.range_check_on)
        End If
    End Sub

    'OFFを選択
    Private Sub MenuItemSelectOFF_Click(sender As Object, e As EventArgs) Handles MenuItemSelectOFF.Click
        Debug.Print("OFFを選択")
        If _DataGridSelection.IsValidArea() Then
            _DataGridSelection.SetDataGridRangeSelected(CDataGridSelection.range_type.range_check_off)
        End If
    End Sub

    '左下移動
    Private Sub MenuItemLeftDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftDownMove.Click
        'Debug.Print("左下移動")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.MoveSelection(-1, 1)
        End If
    End Sub

    '下移動
    Private Sub MenuItemDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemDownMove.Click
        'Debug.Print("下移動")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.MoveSelection(0, 1)
        End If
    End Sub

    '右下移動
    Private Sub MenuItemRightDownMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightDownMove.Click
        'Debug.Print("右下移動")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.MoveSelection(1, 1)
        End If
    End Sub

    '左移動
    Private Sub MenuItemLeftMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftMove.Click
        'Debug.Print("左移動")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.MoveSelection(-1, 0)
        End If
    End Sub

    '右移動
    Private Sub MenuItemRightMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightMove.Click
        'Debug.Print("右移動")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.MoveSelection(1, 0)
        End If
    End Sub

    '左上移動
    Private Sub MenuItemLeftUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemLeftUpMove.Click
        'Debug.Print("左上移動")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.MoveSelection(-1, -1)
        End If
    End Sub

    '上移動
    Private Sub MenuItemUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemUpMove.Click
        'Debug.Print("上移動")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.MoveSelection(0, -1)
        End If
    End Sub

    '右上移動
    Private Sub MenuItemRightUpMove_Click(sender As Object, e As EventArgs) Handles MenuItemRightUpMove.Click
        'Debug.Print("右上移動")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.MoveSelection(1, -1)
        End If
    End Sub

    '左下拡張
    Private Sub MenuItemLeftDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftDownExt.Click
        'Debug.Print("左下拡張")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.ExtentSelection(-1, 1)
        End If
    End Sub

    '下拡張
    Private Sub MenuItemDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemDownExt.Click
        'Debug.Print("下拡張")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.ExtentSelection(0, 1)
        End If
    End Sub

    '右下拡張
    Private Sub MenuItemRightDownExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightDownExt.Click
        'Debug.Print("右下拡張")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.ExtentSelection(1, 1)
        End If
    End Sub

    '左拡張
    Private Sub MenuItemLeftExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftExt.Click
        'Debug.Print("左拡張")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.ExtentSelection(-1, 0)
        End If
    End Sub

    '右拡張
    Private Sub MenuItemRightExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightExt.Click
        'Debug.Print("右拡張")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.ExtentSelection(1, 0)
        End If
    End Sub

    '左上拡張
    Private Sub MenuItemLeftUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemLeftUpExt.Click
        'Debug.Print("左上拡張")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.ExtentSelection(-1, -1)
        End If
    End Sub

    '上拡張
    Private Sub MenuItemUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemUpExt.Click
        'Debug.Print("上拡張")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.ExtentSelection(0, -1)
        End If
    End Sub

    '右上拡張
    Private Sub MenuItemRightUpExt_Click(sender As Object, e As EventArgs) Handles MenuItemRightUpExt.Click
        'Debug.Print("右上拡張")
        If _DataGridSelection.GetDataGridSelected() Then
            _DataGridSelection.ExtentSelection(1, -1)
        End If
    End Sub


#End Region



End Class
