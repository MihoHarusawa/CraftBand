
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsUpDown
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBandSquare45.clsCalcSquare45

Public Class frmMain

    '画面編集用のワーク
    Dim _clsDataTables As New clsDataTables
    '計算用のワーク
    Dim _clsCalcSquare45 As New clsCalcSquare45(_clsDataTables, Me)

    '編集中のファイルパス
    Friend _sFilePath As String = Nothing '起動時引数があればセット(issue#8)


    Dim _isLoadingData As Boolean = True 'Designer.vb描画
    Dim _isChangingByCode As Boolean = False


#Region "基本的な画面処理"

    Dim _Profile_dgv縁の始末 As New CDataGridViewProfile(
            (New tbl側面DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow Or enumAction._RowHeight_iひも番号
            )
    Dim _Profile_dgv縦横ひも As New CDataGridViewProfile(
            (New tbl縦横展開DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dgv縁の始末.RowTemplate.Height = cRowHeightIdxOne
        'dgv追加品.RowTemplate.Height = cRowHeightIdxOne

        _Profile_dgv縁の始末.FormCaption = Me.Text
        dgv縁の始末.SetProfile(_Profile_dgv縁の始末)

        _Profile_dgv縦横ひも.FormCaption = Me.Text
        dgv横ひも.SetProfile(_Profile_dgv縦横ひも)
        dgv縦ひも.SetProfile(_Profile_dgv縦横ひも)

        editAddParts.SetNames(Me.Text, tpage追加品.Text)
        editUpDown.FormCaption = Me.Text
        editUpDown.IsSquare45 = True

#If DEBUG Then
        btnDEBUG.Visible = (clsLog.LogLevel.Trouble <= g_clsLog.Level)
#Else
        btnDEBUG.Visible = (clsLog.LogLevel.Debug <= g_clsLog.Level)
#End If

        Dim lastFilePath As String
        If Not String.IsNullOrWhiteSpace(_sFilePath) Then
            lastFilePath = _sFilePath
            _sFilePath = Nothing
        Else
            lastFilePath = My.Settings.LastFilePath
        End If
        If Not String.IsNullOrWhiteSpace(lastFilePath) AndAlso IO.File.Exists(lastFilePath) Then
            If _clsDataTables.Load(lastFilePath) Then
                _sFilePath = lastFilePath
            Else
                _clsDataTables.SetInitialValue()
            End If
        Else
            _clsDataTables.SetInitialValue()

        End If

        'プレビューを先にするため一旦削除
        TabControl.TabPages.Remove(tpage横ひも)
        TabControl.TabPages.Remove(tpage縦ひも)

        '固定のテーブルを設定(対象バンドの変更時にはテーブルの中身を変える)
        f_i何本幅2.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅2.DisplayMember = "Display"
        f_i何本幅2.ValueMember = "Value"

        f_s色2.DataSource = g_clsSelectBasics.p_tblColor
        f_s色2.DisplayMember = "Display"
        f_s色2.ValueMember = "Value"
        '
        f_s色4.DataSource = g_clsSelectBasics.p_tblColor
        f_s色4.DisplayMember = "Display"
        f_s色4.ValueMember = "Value"
        f_s色5.DataSource = g_clsSelectBasics.p_tblColor
        f_s色5.DisplayMember = "Display"
        f_s色5.ValueMember = "Value"

        setBasics(g_clsSelectBasics.p_s対象バンドの種類名 = _clsDataTables.p_row目標寸法.Value("f_sバンドの種類名")) '異なる場合は DispTables内
        setPattern()
        _isLoadingData = False 'Designer.vb描画完了

        DispTables(_clsDataTables) 'バンドの種類変更対応含む

        'ひも上下の初期値
        chk横の辺.Checked = True
        nud垂直に.Value = 2
        nud底に.Value = 0

        'サイズ復元
        Dim siz As Size = My.Settings.frmMainSize
        If 0 < siz.Height AndAlso 0 < siz.Width Then
            Me.Size = siz
            Dim colwid As String
            colwid = My.Settings.frmMainGridSide
            Me.dgv縁の始末.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridOptions
            Me.editAddParts.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridYoko
            Me.dgv横ひも.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridTate
            Me.dgv縦ひも.SetColumnWidthFromString(colwid)
        End If

        setStartEditing()
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            If Me.DialogResult <> DialogResult.OK AndAlso Me.DialogResult <> DialogResult.Abort Then
                If IsContinueEditing() Then
                    e.Cancel = True
                End If
            End If
        End If

        My.Settings.frmMainGridSide = Me.dgv縁の始末.GetColumnWidthString()
        My.Settings.frmMainGridOptions = Me.editAddParts.GetColumnWidthString()
        My.Settings.frmMainGridYoko = Me.dgv横ひも.GetColumnWidthString()
        My.Settings.frmMainGridTate = Me.dgv縦ひも.GetColumnWidthString()
        My.Settings.frmMainSize = Me.Size
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv側面={0}", My.Settings.frmMainGridSide)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv追加品={0}", My.Settings.frmMainGridOptions)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv横ひも={0}", My.Settings.frmMainGridYoko)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv縦ひも={0}", My.Settings.frmMainGridTate)
    End Sub


    '対象バンド・基本値の更新
    Sub setBasics(ByVal isCheckUndef As Boolean)

        With g_clsSelectBasics
            txtバンドの種類名.Text = .p_s対象バンドの種類名

            Dim unitstr As String = .p_unit設定時の寸法単位.Str
            lbl目標寸法_単位.Text = unitstr
            lbl計算寸法_単位.Text = unitstr
            lblひも間のすき間_単位.Text = unitstr
            lblひも長加算_単位.Text = unitstr


            nud横寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud縦寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud高さ寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nudひも長加算.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces

            nudひも間のすき間.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
            Dim maxLane As Integer = .p_i本幅
            nud基本のひも幅.Maximum = maxLane '表示中のValueは追随

            '色リスト
            '#29
            cmb基本色.Items.Clear()
            For Each r As dstWork.tblColorRow In g_clsSelectBasics.p_tblColor
                cmb基本色.Items.Add(r.Display)
            Next

            lbl基本のひも幅length.Text = New Length(.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit

            'Grid
            Dim format As String = String.Format("N{0}", .p_unit設定時の寸法単位.DecimalPlaces)
            Me.f_d高さ2.DefaultCellStyle.Format = format
            Me.f_d垂直ひも長2.DefaultCellStyle.Format = format
            Me.f_d周長2.DefaultCellStyle.Format = format
            Me.f_dひも長2.DefaultCellStyle.Format = format
            Me.f_d連続ひも長2.DefaultCellStyle.Format = format

            Me.f_d長さ4.DefaultCellStyle.Format = format
            Me.f_dひも長4.DefaultCellStyle.Format = format

            Me.f_d長さ5.DefaultCellStyle.Format = format
            Me.f_dひも長5.DefaultCellStyle.Format = format
        End With

        '#42
        If isCheckUndef Then
            '未定義色の変更確認
            Dim dlg As New frmColorChange
            dlg.SetDataAndExpand(_clsDataTables, True) '縦横展開を含める
            dlg.ShowDialogForUndef()

            '未参照を排除
            _clsDataTables.ModifySelected()
        End If

    End Sub

    '編みかたの変更
    Sub setPattern()
        cmb編みかた名_側面.Items.Clear()
        cmb編みかた名_側面.Items.AddRange(g_clsMasterTables.GetPatternNames(True, False))
    End Sub

    '再計算
    Private Function recalc(ByVal category As CalcCategory, Optional ByVal ctr As Object = Nothing, Optional ByVal key As Object = Nothing) As Boolean
        If _isLoadingData Then
            Return True
        End If

        If {CalcCategory.NewData, CalcCategory.Target, CalcCategory.BandWidth}.Contains(category) Then
            Save目標寸法(_clsDataTables.p_row目標寸法)
        End If
        If {CalcCategory.NewData, CalcCategory.Square, CalcCategory.BandWidth}.Contains(category) Then
            Save四角数(_clsDataTables.p_row底_縦横)
        End If
        'tableについては更新中をそのまま使用

        Dim ret = _clsCalcSquare45.CalcSize(category, ctr, key)
        Disp計算結果(_clsCalcSquare45) 'NGはToolStripに表示

        Return ret
    End Function
#End Region

#Region "クラス値と表示・タブ"

    Dim _CurrentTabControlName As String = ""

    Sub DispTables(ByVal works As clsDataTables)
        If works IsNot Nothing Then
            _isLoadingData = True

            If works IsNot Nothing Then
                Disp目標寸法(works.p_row目標寸法)
                Disp四角数(works.p_row底_縦横)
            End If

            ShowGridSelected(works)

            _isLoadingData = False

            If works IsNot Nothing Then
                recalc(CalcCategory.NewData)
            End If
        End If
    End Sub

    Private Sub ShowGridSelected(ByVal works As clsDataTables)
        If TabControl.SelectedIndex < 0 OrElse TabControl.SelectedTab Is Nothing Then
            Exit Sub
        End If

        'タブページ名
        Select Case TabControl.SelectedTab.Name
            Case "tpage四角数"
                '
            Case "tpage縁の始末"
                Show側面(works)
            Case "tpage追加品"
                Show追加品(works)
            Case "tpageメモ他"
                '
            Case "tpage横ひも"
                Show底の横(works)
            Case "tpage縦ひも"
                Show底の縦(works)
            Case "tpageプレビュー"
                Showプレビュー(works)
            Case "tpageひも上下"
                Showひも上下(works)
            Case Else ' "tpageメモ他"

        End Select
        _CurrentTabControlName = TabControl.SelectedTab.Name
    End Sub

    'デフォルトタブ表示
    <Flags()>
    Enum enumReason
        _GridDropdown = &H1
        _Preview = &H2
        _Always = &H100 '#41
    End Enum
    Private Sub ShowDefaultTabControlPage(ByVal reason As enumReason)
        Dim needreset As Boolean = reason.HasFlag(enumReason._Always)
        If reason.HasFlag(enumReason._GridDropdown) Then
            If {"tpage縁の始末", "tpage追加品", "tpage横ひも", "tpage縦ひも"}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If reason.HasFlag(enumReason._Preview) Then
            If {"tpageプレビュー", "tpageひも上下"}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If needreset Then
            TabControl.SelectTab("tpage四角数")
        End If
    End Sub

    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If TabControl.SelectedTab IsNot Nothing AndAlso
            _CurrentTabControlName = TabControl.SelectedTab.Name Then
            Exit Sub
        End If

        '先のページ名
        Select Case _CurrentTabControlName
            Case "tpage四角数"
                '
            Case "tpage縁の始末"
                Hide側面(_clsDataTables)
            Case "tpage追加品"
                Hide追加品(_clsDataTables)
            Case "tpageメモ他"
                '
            Case "tpage横ひも"
                Hide底の横(_clsDataTables)
            Case "tpage縦ひも"
                Hide底の縦(_clsDataTables)
            Case "tpageプレビュー"
                Hideプレビュー(_clsDataTables)
            Case "tpageひも上下"
                Hideひも上下(_clsDataTables)
            Case Else ' 
                '
        End Select
        _CurrentTabControlName = ""

        ShowGridSelected(_clsDataTables)
    End Sub

    Sub Disp目標寸法(ByVal row目標寸法 As clsDataRow)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp目標寸法() {0}", row目標寸法.ToString)
        With row目標寸法
            If .Value("f_sバンドの種類名") <> g_clsSelectBasics.p_s対象バンドの種類名 Then
                g_clsSelectBasics.SetTargetBandTypeName(.Value("f_sバンドの種類名"))
                setBasics(True)
            End If
            '
            If .Value("f_b内側区分") Then
                rad目標寸法_内側.Checked = True
            Else
                rad目標寸法_外側.Checked = True
            End If

            dispValidValueNud(nud横寸法, .Value("f_d横寸法"))
            dispValidValueNud(nud縦寸法, .Value("f_d縦寸法"))
            dispValidValueNud(nud高さ寸法, .Value("f_d高さ寸法"))

            dispAdjustLane(nud基本のひも幅, .Value("f_i基本のひも幅"))
            cmb基本色.Text = .Value("f_s基本色")

            txtメモ.Text = .Value("f_sメモ")
            txtタイトル.Text = .Value("f_sタイトル")
            txt作成者.Text = .Value("f_s作成者")
        End With
    End Sub


    Sub Disp四角数(ByVal row底_縦横 As clsDataRow)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp四角 {0}", row底_縦横.ToString)
        With row底_縦横
            chk縦横を展開する.Checked = .Value("f_b展開区分")
            set底の縦横展開(.Value("f_b展開区分"))

            dispValidValueNud(nudひも間のすき間, .Value("f_dひも間のすき間")) 'マイナス不可
            dispValidValueNud(nudひも長係数, .Value("f_dひも長係数")) 'マイナス不可
            nudひも長加算.Value = .Value("f_dひも長加算") 'マイナス可

            nud横の四角数.Value = .Value("f_i横の四角数")
            chk横の補強ひも.Checked = .Value("f_b補強ひも区分")
            txt横ひものメモ.Text = .Value("f_s横ひものメモ")

            nud縦の四角数.Value = .Value("f_i縦の四角数")
            chk縦の補強ひも.Checked = .Value("f_b始末ひも区分")
            txt縦ひものメモ.Text = .Value("f_s縦ひものメモ")

            nud高さの四角数.Value = .Value("f_d高さの四角数")
        End With
    End Sub

    Private Sub dispAdjustLane(ByVal nud As NumericUpDown, ByVal lane As Integer)
        If nud.Maximum < lane Then
            nud.Value = nud.Maximum
        ElseIf lane < nud.Minimum Then
            nud.Value = nud.Minimum
        Else
            nud.Value = lane
        End If
    End Sub

    Private Sub dispValidValueNud(ByVal nud As NumericUpDown, ByVal value As Double)
        If 0 <= value Then
            nud.Value = value
        Else
            nud.Value = 0
            nud.ResetText()
        End If
    End Sub



    Private Sub Disp計算結果(ByVal calc As clsCalcSquare45)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Disp計算結果 {0}", calc.ToString)
        With calc
            '
            txt横ひもの本数.Text = .p_i横ひもの本数
            txt縦ひもの本数.Text = .p_i縦ひもの本数
            txt垂直ひも数.Text = .p_i垂直ひも数

            txt1つの辺_ひも幅.Text = .p_sひも幅の辺
            txt対角線_ひも幅.Text = .p_sひも幅の対角線
            txt1つの辺_四角.Text = .p_s四角の辺
            txt対角線_四角.Text = .p_s四角の対角線

            txt四角ベース_横.Text = .p_s四角ベース_横
            txt縁厚さプラス_横.Text = .p_s縁厚さプラス_横
            txt四角ベース_縦.Text = .p_s四角ベース_縦
            txt縁厚さプラス_縦.Text = .p_s縁厚さプラス_縦
            txt四角ベース_高さ.Text = .p_s四角ベース_高さ
            txt縁厚さプラス_高さ.Text = .p_s縁厚さプラス_高さ
            txt四角ベース_周.Text = .p_s四角ベース_周
            txt縁厚さプラス_周.Text = .p_s縁厚さプラス_周

            txt垂直ひも数.Text = .p_i垂直ひも数
            txt厚さ.Text = .p_d厚さ

            lbl横寸法の差.Text = .p_s横寸法の差
            lbl縦寸法の差.Text = .p_s縦寸法の差
            lbl高さ寸法の差.Text = .p_s高さ寸法の差

            If .p_b有効 Then
                ToolStripStatusLabel1.Text = "OK"
                ToolStripStatusLabel2.Text = ""
            Else
                ToolStripStatusLabel1.Text = "NG"
                ToolStripStatusLabel2.Text = .p_sメッセージ
            End If
        End With

    End Sub


    Sub Show側面(ByVal works As clsDataTables)
        BindingSource縁の始末.Sort = Nothing
        BindingSource縁の始末.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        BindingSource縁の始末.DataSource = works.p_tbl側面
        BindingSource縁の始末.Sort = "f_i番号 , f_iひも番号"

        dgv縁の始末.Refresh()
    End Sub

    Function Hide側面(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource縁の始末.DataSource)

        BindingSource縁の始末.Sort = Nothing
        BindingSource縁の始末.DataSource = Nothing

        Return ret
    End Function

    Function SaveTables(ByVal works As clsDataTables) As Boolean
        Save目標寸法(works.p_row目標寸法)
        Save四角数(works.p_row底_縦横)

        works.CheckPoint(works.p_tbl側面)
        works.CheckPoint(works.p_tbl追加品)

        If _CurrentTabControlName = "tpage横ひも" Then
            works.FromTmpTable(enumひも種.i_横 Or enumひも種.i_45度, BindingSource横ひも.DataSource)
        End If
        If _CurrentTabControlName = "tpage縦ひも" Then
            works.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_315度, BindingSource縦ひも.DataSource)
        End If
        If _CurrentTabControlName = "tpageひも上下" Then
            saveひも上下(works, False)
        End If

        Return True
    End Function

    Function Save目標寸法(ByVal row目標寸法 As clsDataRow) As Boolean
        With row目標寸法
            .Value("f_sバンドの種類名") = g_clsSelectBasics.p_s対象バンドの種類名
            .Value("f_b内側区分") = rad目標寸法_内側.Checked
            .Value("f_d横寸法") = nud横寸法.Value
            .Value("f_d縦寸法") = nud縦寸法.Value
            .Value("f_d高さ寸法") = nud高さ寸法.Value
            .Value("f_i基本のひも幅") = nud基本のひも幅.Value
            .Value("f_s基本色") = cmb基本色.Text
            .Value("f_sメモ") = txtメモ.Text
            .Value("f_sタイトル") = txtタイトル.Text
            .Value("f_s作成者") = txt作成者.Text
        End With
        Return True
    End Function

    Function Save四角数(ByVal row底_縦横 As clsDataRow) As Boolean
        With row底_縦横
            .Value("f_b展開区分") = chk縦横を展開する.Checked

            .Value("f_dひも間のすき間") = nudひも間のすき間.Value   'マイナス不可
            .Value("f_dひも長係数") = nudひも長係数.Value  'マイナス不可
            .Value("f_dひも長加算") = nudひも長加算.Value  'マイナス可

            .Value("f_i横の四角数") = nud横の四角数.Value
            .Value("f_b補強ひも区分") = chk横の補強ひも.Checked
            .Value("f_s横ひものメモ") = txt横ひものメモ.Text

            .Value("f_i縦の四角数") = nud縦の四角数.Value
            .Value("f_b始末ひも区分") = chk縦の補強ひも.Checked
            .Value("f_s縦ひものメモ") = txt縦ひものメモ.Text

            .Value("f_d高さの四角数") = nud高さの四角数.Value
        End With
        Return True
    End Function


#End Region

#Region "編集メニューとボタン"
    'バンドの種類選択
    Private Sub ToolStripMenuItemEditSelectBand_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditSelectBand.Click
        Dim dlg As New frmSelectBand
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色と本幅数変更の可能性
        If dlg.ShowDialog() = DialogResult.OK Then
            SaveTables(_clsDataTables)
            setBasics(True)
            recalc(CalcCategory.BsMaster)
        End If
    End Sub

    '色変更
    Private Sub ToolStripMenuItemEditColorChange_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditColorChange.Click
        SaveTables(_clsDataTables)
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色変更

        '#40:縦横のレコード数
        If chk縦横を展開する.Checked Then
            _clsCalcSquare45.prepare縦横展開DataTable()
        End If

        Dim dlg As New frmColorChange
        dlg.SetDataAndExpand(_clsDataTables, chk縦横を展開する.Checked)
        dlg.ShowDialog()
    End Sub

    'リセット
    Private Sub ToolStripMenuItemEditReset_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditReset.Click
        If _clsCalcSquare45.IsValidInput() Then
            '目標寸法以外をリセットします。よろしいですか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetInput, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        _clsDataTables.Clear()
        _clsDataTables.SetInitialValue()
        Save目標寸法(_clsDataTables.p_row目標寸法) '画面値

        DispTables(_clsDataTables)
    End Sub

    '規定値
    Private Sub ToolStripMenuItemEditDefault_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditDefault.Click
        SaveTables(_clsDataTables)
        If String.IsNullOrWhiteSpace(My.Settings.DefaultFilePath) Then
            '規定値が保存されていません。先に規定値保存を行ってください。
            MessageBox.Show(My.Resources.MessageNoDefaultFile, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If Not IO.File.Exists(My.Settings.DefaultFilePath) Then
            '規定値保存ファイル'{0}'がありません。再度規定値保存を行ってください。
            Dim msg As String = String.Format(My.Resources.MessageNotExistDefaultFile, My.Settings.DefaultFilePath)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If _clsCalcSquare45.IsValidInput() Then
            '規定値をロードします。よろしいですか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskLoadDefault, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
        End If
        If _clsDataTables.Load(My.Settings.DefaultFilePath) Then
            DispTables(_clsDataTables)
        Else
            MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    '概算
    Private Sub ToolStripMenuItemEditCalc_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditCalc.Click
        SaveTables(_clsDataTables)
        '実行前のチェック
        Dim message As String = Nothing
        If _clsCalcSquare45.CheckTarget(message) Then
            If String.IsNullOrWhiteSpace(message) Then
                'そのまま進めてOK
            Else
                '要確認
                Dim r As DialogResult = MessageBox.Show(message, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                If DialogResult.OK <> r Then
                    Exit Sub
                End If
            End If
        Else
            '実行不可
            MessageBox.Show(_clsCalcSquare45.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        '実行する
        If _clsCalcSquare45.CalcTarget() Then
            _isLoadingData = True
            '計算結果の縦横値
            Disp四角数(_clsDataTables.p_row底_縦横)
            '(側面のテーブルも変更されている)
            _isLoadingData = False
        Else
            'エラーあり
            MessageBox.Show(_clsCalcSquare45.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        recalc(CalcCategory.NewData)
    End Sub

    'ひもリスト
    Private Sub ToolStripMenuItemEditList_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditList.Click
        SaveTables(_clsDataTables)
        _clsCalcSquare45.CalcSize(CalcCategory.NewData, Nothing, Nothing)

        If Not _clsCalcSquare45.p_b有効 Then
            Dim msg As String = _clsCalcSquare45.p_sメッセージ & vbCrLf
            'このままリスト出力を行いますか？
            msg += My.Resources.AskOutput
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        Dim output As New clsOutput(_sFilePath)
        If Not _clsCalcSquare45.CalcOutput(output) Then
            MessageBox.Show(_clsCalcSquare45.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim dlg As New frmOutput(output)
        dlg.Icon = Me.Icon
        dlg.ShowDialog()
    End Sub

    '規定値保存
    Private Sub ToolStripMenuItemEditDefaultFile_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditDefaultFile.Click
        '現データの状態を規定値として保存します。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(My.Resources.AskSaveDefault, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        Dim fpath As String = My.Settings.DefaultFilePath
        If String.IsNullOrWhiteSpace(fpath) Then
            fpath = IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    My.Resources.DefaultFileName)
        End If

        SaveFileDialog1.FileName = fpath
        If SaveFileDialog1.ShowDialog <> DialogResult.OK Then
            Exit Sub
        End If

        SaveTables(_clsDataTables)
        If _clsDataTables.Save(SaveFileDialog1.FileName) Then
            My.Settings.DefaultFilePath = SaveFileDialog1.FileName
        Else
            MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub


    Private Sub btnリセット_Click(sender As Object, e As EventArgs) Handles btnリセット.Click
        ToolStripMenuItemEditReset.PerformClick()
    End Sub

    Private Sub btn規定値_Click(sender As Object, e As EventArgs) Handles btn規定値.Click
        ToolStripMenuItemEditDefault.PerformClick()
    End Sub

    Private Sub btn概算_Click(sender As Object, e As EventArgs) Handles btn概算.Click
        ToolStripMenuItemEditCalc.PerformClick()
    End Sub

    Private Sub btnひもリスト_Click(sender As Object, e As EventArgs) Handles btnひもリスト.Click
        ToolStripMenuItemEditList.PerformClick()
    End Sub

    Private Sub btn終了_Click(sender As Object, e As EventArgs) Handles btn終了.Click
        ToolStripMenuItemFileExit.PerformClick()
    End Sub
#End Region

#Region "設定メニュー・ヘルプ"
    'バンドの種類
    Private Sub ToolStripMenuItemSettingBandType_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingBandType.Click
        Dim dlg As New frmBandType
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色と本幅数変更の可能性
        If dlg.ShowDialog() = DialogResult.OK Then
            SaveTables(_clsDataTables)
            setBasics(True)
            recalc(CalcCategory.BsMaster)
        End If
    End Sub

    '編みかた
    Private Sub ToolStripMenuItemSettingPattern_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingPattern.Click
        Dim dlg As New frmPattern
        If dlg.ShowDialog() = DialogResult.OK Then
            SaveTables(_clsDataTables)
            setPattern()
            recalc(CalcCategory.BsMaster)
        End If
    End Sub

    '付属品
    Private Sub ToolStripMenuItemSettingOptions_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingOptions.Click
        Dim dlg As New frmOptions
        If dlg.ShowDialog() = DialogResult.OK Then
            SaveTables(_clsDataTables)
            'setOptions()
            recalc(CalcCategory.BsMaster)
        End If
    End Sub

    '描画色
    Private Sub ToolStripMenuItemSettingColor_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingColor.Click
        Dim dlg As New frmColor
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色にかかわるため
        If dlg.ShowDialog() = DialogResult.OK Then
            'プレビュー以外は関係なし
        End If
    End Sub

    '上下図
    Private Sub ToolStripMenuItemSettingUpDown_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingUpDown.Click
        Dim dlg As New frmUpDownPattern
        If dlg.ShowDialog() = DialogResult.OK Then
            '基本、影響なし
        End If
    End Sub

    '基本設定
    Private Sub ToolStripMenuItemSettingBasics_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingBasics.Click
        Dim dlg As New frmBasics
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色と本幅数変更の可能性
        SaveTables(_clsDataTables)
        dlg.DataEditing = _clsDataTables
        dlg.DataPath = _sFilePath

        If dlg.ShowDialog() = DialogResult.OK Then
            setBasics(True)
            recalc(CalcCategory.BsMaster)
        End If
    End Sub

    'ヘルプ
    Private Sub ToolStripMenuItemHelp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemHelp.Click
        Dim dlg = New frmVirsion
        dlg.ShowDialog()
    End Sub

#End Region

#Region "ファイルメニュー"
    Private Function IsContinueEditing(Optional ByVal ask As Boolean = True) As Boolean
        If _clsDataTables Is Nothing Then
            Return False
        End If
        SaveTables(_clsDataTables)
        If Not _clsDataTables.IsChangedFromStartPoint() Then
            Return False
        End If
        If ask Then
            '編集中のデータを破棄してよろしいですか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskAbandanCurrentWork, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r = DialogResult.Yes Then
                Return False '破棄
            End If
        End If
        Return True '編集継続
    End Function

    Private Sub setStartEditing(Optional ByVal showTabBase As Boolean = False)
        '表示後の編集開始
        Save目標寸法(_clsDataTables.p_row目標寸法)
        Save四角数(_clsDataTables.p_row底_縦横)

        _clsDataTables.ResetStartPoint()

        Me.Text = String.Format(My.Resources.FormCaption, IO.Path.GetFileNameWithoutExtension(_sFilePath))

        'タブ表示(#41)
        If showTabBase Then
            ShowDefaultTabControlPage(enumReason._Always)
        End If
    End Sub

    '新規作成
    Private Sub ToolStripMenuItemFileNew_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileNew.Click
        If IsContinueEditing() Then
            Exit Sub
        End If

        _clsDataTables.Clear()
        _clsDataTables.SetInitialValue()
        DispTables(_clsDataTables)

        _sFilePath = Nothing
        setStartEditing()
    End Sub

    '開く
    Private Sub ToolStripMenuItemFileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileOpen.Click
        If IsContinueEditing() Then
            Exit Sub
        End If

        If OpenFileDialog1.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If
        If _clsDataTables.Load(OpenFileDialog1.FileName) Then
            DispTables(_clsDataTables)
            _sFilePath = OpenFileDialog1.FileName
            setStartEditing(True)
        Else
            MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    '名前をつけて保存
    Private Sub ToolStripMenuItemFileSaveAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileSaveAs.Click
        SaveFileDialog1.FileName = _sFilePath
        If String.IsNullOrWhiteSpace(SaveFileDialog1.FileName) Then
            '目標寸法が空ならセット(#19)
            If nud横寸法.Value = 0 AndAlso nud縦寸法.Value = 0 AndAlso nud高さ寸法.Value = 0 AndAlso
                0 < _clsCalcSquare45.p_d四角ベース_横 AndAlso 0 < _clsCalcSquare45.p_d四角ベース_縦 Then
                nud横寸法.Value = _clsCalcSquare45.p_d四角ベース_横
                nud縦寸法.Value = _clsCalcSquare45.p_d四角ベース_縦
                nud高さ寸法.Value = 1 '空の解除
                nud高さ寸法.Value = _clsCalcSquare45.p_d四角ベース_高さ
            End If
            'Square45(本幅)横-縦-高さ
            Dim defname As String
            Save目標寸法(_clsDataTables.p_row目標寸法)
            With _clsDataTables.p_row目標寸法
                defname = String.Format("{0}({1}){2}-{3}-{4}",
                GetShortExeName(g_enumExeName), .Value("f_i基本のひも幅"),
                Int(.Value("f_d横寸法")), Int(.Value("f_d縦寸法")), Int(.Value("f_d高さ寸法")))
            End With
            SaveFileDialog1.FileName = defname
        End If
        If SaveFileDialog1.ShowDialog <> DialogResult.OK Then
            Exit Sub
        End If

        SaveTables(_clsDataTables)
        If _clsDataTables.Save(SaveFileDialog1.FileName) Then
            _sFilePath = SaveFileDialog1.FileName
            setStartEditing()

        Else
            MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    '保存
    Private Sub ToolStripMenuItemFileSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileSave.Click
        If String.IsNullOrEmpty(_sFilePath) Then
            SaveFileDialog1.FileName = Nothing
            If SaveFileDialog1.ShowDialog <> DialogResult.OK Then
                Exit Sub
            End If
            _sFilePath = SaveFileDialog1.FileName
        End If

        SaveTables(_clsDataTables)
        If _clsDataTables.Save(_sFilePath) Then
            setStartEditing()
        Else
            MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    '中止
    Private Sub ToolStripMenuItemFileAbort_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileAbort.Click
        If IsContinueEditing() Then
            Exit Sub
        End If
        Me.DialogResult = DialogResult.Abort
        Me.Close()
    End Sub

    '終了
    Private Sub ToolStripMenuItemExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileExit.Click
        If IsContinueEditing(False) Then
            '編集中のデータを保存しますか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskSaveCurrentWork, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
            If r = DialogResult.Cancel Then
                Exit Sub
            ElseIf r = DialogResult.Yes Then

                If String.IsNullOrEmpty(_sFilePath) Then
                    SaveFileDialog1.FileName = Nothing
                    If SaveFileDialog1.ShowDialog <> DialogResult.OK Then
                        Exit Sub
                    End If
                    _sFilePath = SaveFileDialog1.FileName
                End If

                SaveTables(_clsDataTables)
                If Not _clsDataTables.Save(_sFilePath) Then
                    MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            End If
        End If
        Me.DialogResult = DialogResult.OK
        My.Settings.LastFilePath = _sFilePath
        Me.Close()
    End Sub

    'issue#8 DragDrop
    Private Sub frmMain_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        'コントロール内にドラッグされたとき実行される
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーアイコン
            e.Effect = DragDropEffects.Copy
        Else
            'ファイル以外は受け付けない
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub frmMain_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim fileNames As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
        For Each fname In fileNames
            If IsContinueEditing() Then
                Exit Sub
            End If
            If _clsDataTables.Load(fname) Then
                DispTables(_clsDataTables)
                _sFilePath = fname
                setStartEditing(True)
                Exit Sub
            Else
                MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Next
    End Sub
#End Region

#Region "コントロール変更イベント"
    '縦横の展開チェックボックス　※チェックは最初のタブにある
    Private Sub chk縦横を展開する_CheckedChanged(sender As Object, e As EventArgs) Handles chk縦横を展開する.CheckedChanged
        set底の縦横展開(chk縦横を展開する.Checked)
    End Sub

    Private Sub nud基本のひも幅_ValueChanged(sender As Object, e As EventArgs) Handles nud基本のひも幅.ValueChanged
        If nud基本のひも幅.Value = 0 Then
            nud基本のひも幅.Value = g_clsSelectBasics.p_i本幅
        End If
        lbl基本のひも幅length.Text = New Length(g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit
        ShowDefaultTabControlPage(enumReason._Preview) '色と本幅数変更の可能性

        recalc(CalcCategory.BandWidth, sender)
    End Sub

    Private Sub rad目標寸法_CheckedChanged(sender As Object, e As EventArgs) Handles rad目標寸法_外側.CheckedChanged, rad目標寸法_内側.CheckedChanged
        recalc(CalcCategory.Target, sender)
    End Sub

    Private Sub nud横寸法_ValueChanged(sender As Object, e As EventArgs) Handles nud横寸法.ValueChanged
        recalc(CalcCategory.Target, sender)
    End Sub

    Private Sub nud縦寸法_ValueChanged(sender As Object, e As EventArgs) Handles nud縦寸法.ValueChanged
        recalc(CalcCategory.Target, sender)
    End Sub

    Private Sub nud横の四角数_ValueChanged(sender As Object, e As EventArgs) Handles nud横の四角数.ValueChanged
        recalc(CalcCategory.Square, sender)
    End Sub

    Private Sub nudひも間のすき間_ValueChanged(sender As Object, e As EventArgs) Handles nudひも間のすき間.ValueChanged
        recalc(CalcCategory.BandWidth, sender)
    End Sub

    Private Sub nud縦の四角数_ValueChanged(sender As Object, e As EventArgs) Handles nud縦の四角数.ValueChanged
        recalc(CalcCategory.Square, sender)
    End Sub

    Private Sub nud高さの四角数_ValueChanged(sender As Object, e As EventArgs) Handles nud高さの四角数.ValueChanged
        recalc(CalcCategory.Square, sender)
    End Sub

    Private Sub cmb基本色_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb基本色.SelectedIndexChanged
        ShowDefaultTabControlPage(enumReason._Preview) '色と本幅数変更の可能性
    End Sub
#End Region

#Region "縁の始末"
    Private Sub btn追加_側面_Click(sender As Object, e As EventArgs) Handles btn追加_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        'If Not getTableAndNumber(BindingSource縁の始末, table, number) Then
        If Not dgv縁の始末.GetTableAndNumber(table, number) Then
            Exit Sub
        End If

        Dim row As tbl側面Row = Nothing
        If _clsCalcSquare45.add_側面(cmb編みかた名_側面.Text, True,
                     nud基本のひも幅.Value, 1,
                      row) Then

            'numberPositionsSelect(BindingSource縁の始末, row.f_i番号, dgv縁の始末)
            dgv縁の始末.NumberPositionsSelect(row.f_i番号)
            recalc(CalcCategory.Edge, row, "f_i周数")
        Else
            MessageBox.Show(_clsCalcSquare45.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub btn削除_側面_Click(sender As Object, e As EventArgs) Handles btn削除_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        'If Not getTableAndNumber(BindingSource縁の始末, table, number) Then
        If Not dgv縁の始末.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        clsDataTables.RemoveNumberFromTable(table, number)
        recalc(CalcCategory.Edge, Nothing, Nothing)
    End Sub


    Private Sub dgv側面_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv縁の始末.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource縁の始末.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv側面_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName側面(DataPropertyName) Then
            recalc(CalcCategory.Edge, current.Row, DataPropertyName)
        End If
    End Sub

#End Region

#Region "追加品"
    Sub Show追加品(ByVal works As clsDataTables)
        editAddParts.PanelSize = tpage追加品.Size
        editAddParts.ShowGrid(works)
        recalc(CalcCategory.Options)
    End Sub

    Function Hide追加品(ByVal works As clsDataTables) As Boolean
        Return editAddParts.HideGrid(works)
    End Function

    Private Sub editAddParts_AddPartsError(sender As Object, e As EventArgs) Handles editAddParts.AddPartsError
        recalc(CalcCategory.Options)
    End Sub

    Private Sub tpage追加品_Resize(sender As Object, e As EventArgs) Handles tpage追加品.Resize
        editAddParts.PanelSize = tpage追加品.Size
    End Sub

#End Region

#Region "縦横展開"

    'タブの表示・非表示(タブのインスタンスは保持)
    Private Sub set底の縦横展開(ByVal isExband As Boolean)
        If isExband Then
            If Not TabControl.TabPages.Contains(tpage横ひも) Then
                TabControl.TabPages.Add(tpage横ひも)
            End If
            If Not TabControl.TabPages.Contains(tpage縦ひも) Then
                TabControl.TabPages.Add(tpage縦ひも)
            End If
            If Not TabControl.TabPages.Contains(tpageひも上下) Then
                TabControl.TabPages.Add(tpageひも上下)
            End If
        Else
            If TabControl.TabPages.Contains(tpage横ひも) Then
                TabControl.TabPages.Remove(tpage横ひも)
            End If
            If TabControl.TabPages.Contains(tpage縦ひも) Then
                TabControl.TabPages.Remove(tpage縦ひも)
            End If
            If TabControl.TabPages.Contains(tpageひも上下) Then
                TabControl.TabPages.Remove(tpageひも上下)
            End If
        End If
    End Sub


    Sub Show底の横(ByVal works As clsDataTables)
        BindingSource横ひも.Sort = Nothing
        BindingSource横ひも.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        'タブ切り替えタイミングのため、表示は更新済
        Save四角数(works.p_row底_縦横)
        Dim tmptable As tbl縦横展開DataTable = _clsCalcSquare45.set横展開DataTable(True)

        BindingSource横ひも.DataSource = tmptable
        BindingSource横ひも.Sort = "f_iひも種,f_iひも番号"

        dgv横ひも.Refresh()
    End Sub

    Function Hide底の横(ByVal works As clsDataTables) As Boolean
        Dim change As Integer = works.FromTmpTable(enumひも種.i_横 Or enumひも種.i_45度, BindingSource横ひも.DataSource)
        BindingSource横ひも.Sort = Nothing
        BindingSource横ひも.DataSource = Nothing

        dgv横ひも.Refresh()
        Return 0 < change
    End Function

    Sub Show底の縦(ByVal works As clsDataTables)
        BindingSource縦ひも.Sort = Nothing
        BindingSource縦ひも.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        'タブ切り替えタイミングのため、表示は更新済
        Save四角数(works.p_row底_縦横)
        Dim tmptable As tbl縦横展開DataTable = _clsCalcSquare45.set縦展開DataTable(True)

        BindingSource縦ひも.DataSource = tmptable
        BindingSource縦ひも.Sort = "f_iひも種,f_iひも番号"

        dgv縦ひも.Refresh()
    End Sub

    Function Hide底の縦(ByVal works As clsDataTables) As Boolean
        Dim change As Integer = works.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_315度, BindingSource縦ひも.DataSource)
        BindingSource縦ひも.Sort = Nothing
        BindingSource縦ひも.DataSource = Nothing

        dgv縦ひも.Refresh()
        Return 0 < change
    End Function

    Private Sub btnリセット_横_Click(sender As Object, e As EventArgs) Handles btnリセット_横.Click
        'ひも長加算と色をすべてクリアします。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetAddLengthColor, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        BindingSource横ひも.DataSource = _clsCalcSquare45.set横展開DataTable(False)
        BindingSource横ひも.Sort = "f_iひも種,f_iひも番号"
        dgv横ひも.Refresh()
    End Sub

    Private Sub btnリセット_縦_Click(sender As Object, e As EventArgs) Handles btnリセット_縦.Click
        'ひも長加算と色をすべてクリアします。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetAddLengthColor, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        BindingSource縦ひも.DataSource = _clsCalcSquare45.set縦展開DataTable(False)
        BindingSource縦ひも.Sort = "f_iひも種,f_iひも番号"
        dgv縦ひも.Refresh()
    End Sub
#End Region

#Region "ひも上下"

    Sub Showひも上下(ByVal works As clsDataTables)
        editUpDown.I横の四角数 = _clsCalcSquare45.p_i横の四角数
        editUpDown.I縦の四角数 = _clsCalcSquare45.p_i縦の四角数
        editUpDown.PanelSize = tpageひも上下.Size

        editUpDown.ShowGrid(works, enumTargetFace.Bottom)
    End Sub

    Function Hideひも上下(ByVal works As clsDataTables) As Boolean
        Return editUpDown.HideGrid(works)
    End Function

    Function saveひも上下(ByVal works As clsDataTables, ByVal isMsg As Boolean) As Boolean
        Return editUpDown.Save(works, isMsg)
    End Function

    Private Sub tpageひも上下_Resize(sender As Object, e As EventArgs) Handles tpageひも上下.Resize
        editUpDown.PanelSize = tpageひも上下.Size
    End Sub


    Private Sub btn合わせる_Click(sender As Object, e As EventArgs) Handles btn合わせる.Click
        If editUpDown.Is底位置表示 Then
            '{0}に基づき再度初期化してよろしいですか？
            If MessageBox.Show(String.Format(My.Resources.AskInitializeAgain, grp縦横の四角.Text),
                               Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim updown As clsUpDown = _clsCalcSquare45.fitsizeひも上下(chk横の辺.Checked, nud垂直に.Value, nud底に.Value)
        If updown Is Nothing OrElse Not updown.IsValid(False) Then
            '現在の値では合わせることはできません。
            MessageBox.Show(My.Resources.MessageCannotSuit, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        editUpDown.Replace(updown)
    End Sub

#End Region

#Region "プレビュー"
    Dim _clsImageData As clsImageData
    Private Sub Showプレビュー(works As clsDataTables)
        picプレビュー.Image = Nothing
        _clsImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcSquare45.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcSquare45) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If

        Cursor.Current = Cursors.WaitCursor
        _clsImageData = New clsImageData(_sFilePath)
        ret = _clsCalcSquare45.CalcImage(_clsImageData)
        Cursor.Current = Cursors.Default

        If Not ret AndAlso Not String.IsNullOrWhiteSpace(_clsCalcSquare45.p_sメッセージ) Then
            MessageBox.Show(_clsCalcSquare45.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        picプレビュー.Image = System.Drawing.Image.FromFile(_clsImageData.GifFilePath)
    End Sub

    Private Sub Hideプレビュー(clsDataTables As clsDataTables)
        picプレビュー.Image = Nothing
        If _clsImageData IsNot Nothing Then
            _clsImageData.Clear()
            _clsImageData = Nothing
        End If
    End Sub

    Private Sub btnブラウザ_Click(sender As Object, e As EventArgs) Handles btnブラウザ.Click
        If _clsImageData Is Nothing Then
            Return
        End If
        If Not _clsImageData.ImgBrowserOpen() Then
            MessageBox.Show(_clsImageData.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btn画像ファイル_Click(sender As Object, e As EventArgs) Handles btn画像ファイル.Click
        If _clsImageData Is Nothing Then
            Return
        End If
        If Not _clsImageData.ImgFileOpen() Then
            MessageBox.Show(_clsImageData.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
#End Region


#Region "DEBUG"
    Dim bVisible As Boolean = False
    Private Sub btnDEBUG_Click(sender As Object, e As EventArgs) Handles btnDEBUG.Click

        If Not bVisible Then
            setDgvColumnsVisible(dgv縁の始末)
            editAddParts.SetDgvColumnsVisible()
            setDgvColumnsVisible(dgv横ひも)
            setDgvColumnsVisible(dgv縦ひも)
            bVisible = True
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", g_clsSelectBasics.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row目標寸法.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row底_縦横.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsCalcSquare45.dump())
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tbl側面).ToString())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tbl追加品).ToString())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tbl縦横展開).ToString())

    End Sub
    Private Sub setDgvColumnsVisible(ByVal dgv As DataGridView)
        For Each col As DataGridViewColumn In dgv.Columns
            If Not col.Visible Then
                col.Visible = True
            End If
        Next
    End Sub



#End Region

End Class
