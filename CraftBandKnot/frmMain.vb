
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBandKnot.clsCalcKnot

Public Class frmMain

    '画面編集用のワーク
    Dim _clsDataTables As New clsDataTables
    '計算用のワーク
    Dim _clsCalcKnot As New clsCalcKnot(_clsDataTables, Me)

    '編集中のファイルパス
    Friend _sFilePath As String = Nothing '起動時引数があればセット(issue#8)


    Dim _isLoadingData As Boolean = True 'Designer.vb描画
    Dim _isChangingByCode As Boolean = False


#Region "基本的な画面処理"

    Dim _Profile_dgv側面と縁 As New CDataGridViewProfile(
            (New tbl側面DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _Profile_dgv側面と縁.FormCaption = Me.Text
        dgv側面と縁.SetProfile(_Profile_dgv側面と縁)

        editAddParts.SetNames(Me.Text, tpage追加品.Text)

        expand横ひも.SetNames(Me.Text, tpage横ひも.Text, False, ctrExpanding.enumVisible.i_None, My.Resources.CaptionExpand8To2, My.Resources.CaptionExpand4To6)
        expand縦ひも.SetNames(Me.Text, tpage縦ひも.Text, False, ctrExpanding.enumVisible.i_None, My.Resources.CaptionExpand4To6, My.Resources.CaptionExpand8To2)


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
        setBasics(g_clsSelectBasics.p_s対象バンドの種類名 = _clsDataTables.p_row目標寸法.Value("f_sバンドの種類名")) '異なる場合は DispTables内
        setPattern()
        _isLoadingData = False 'Designer.vb描画完了

        DispTables(_clsDataTables) 'バンドの種類変更対応含む

        'サイズ復元
        Dim siz As Size = My.Settings.frmMainSize
        If 0 < siz.Height AndAlso 0 < siz.Width Then
            Me.Size = siz
            Dim colwid As String
            colwid = My.Settings.frmMainGridSide
            Me.dgv側面と縁.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridOptions
            Me.editAddParts.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridYoko
            Me.expand横ひも.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridTate
            Me.expand縦ひも.SetColumnWidthFromString(colwid)
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

        My.Settings.frmMainGridSide = Me.dgv側面と縁.GetColumnWidthString()
        My.Settings.frmMainGridOptions = Me.editAddParts.GetColumnWidthString()
        My.Settings.frmMainGridYoko = Me.expand横ひも.GetColumnWidthString()
        My.Settings.frmMainGridTate = Me.expand縦ひも.GetColumnWidthString()
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
            lblコマ間のすき間_単位.Text = unitstr
            lblひも長加算_縦横_単位.Text = unitstr
            lblひも長加算_側面_単位.Text = unitstr

            nud横寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud縦寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud高さ寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nudひも長加算_縦横.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nudひも長加算_側面.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces

            nudコマ間のすき間.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
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

            'コマ寸法と要尺(ここではセットするだけ)
            _clsCalcKnot.SetBandName(.p_s対象バンドの種類名)
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
        If {CalcCategory.NewData, CalcCategory.Knot, CalcCategory.BandWidth}.Contains(category) Then
            Saveコマ数(_clsDataTables.p_row底_縦横)
        End If
        'tableについては更新中をそのまま使用

        Dim ret = _clsCalcKnot.CalcSize(category, ctr, key)
        Disp計算結果(_clsCalcKnot) 'NGはToolStripに表示

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
                Dispコマ数(works.p_row底_縦横)
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
            Case tpageコマ数.Name
                '
            Case tpage側面と縁.Name
                Show側面(works)
            Case tpage追加品.Name
                Show追加品(works)
            Case tpageメモ他.Name
                '
            Case tpage横ひも.Name
                Show横ひも(works)
            Case tpage縦ひも.Name
                Show縦ひも(works)
            Case tpageプレビュー.Name
                Showプレビュー(works)
            Case Else ' 

        End Select
        _CurrentTabControlName = TabControl.SelectedTab.Name
    End Sub

    'デフォルトタブ表示
    <Flags()>
    Enum enumReason
        _GridDropdown = &H1
        _Preview = &H2
        _Gauge = &H4
        _Always = &H100
    End Enum
    Private Sub ShowDefaultTabControlPage(ByVal reason As enumReason)
        If _isLoadingData Then
            Exit Sub
        End If
        Dim needreset As Boolean = reason.HasFlag(enumReason._Always)
        If reason.HasFlag(enumReason._GridDropdown) Then
            If {tpage側面と縁.Name, tpage追加品.Name, tpage横ひも.Name, tpage縦ひも.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If reason.HasFlag(enumReason._Preview) Then
            If {tpageプレビュー.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If reason.HasFlag(enumReason._Gauge) Then
            If {tpage側面と縁.Name, tpage横ひも.Name, tpage縦ひも.Name, tpageプレビュー.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If needreset Then
            TabControl.SelectTab(tpageコマ数.Name)
        End If
    End Sub

    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If TabControl.SelectedTab IsNot Nothing AndAlso
            _CurrentTabControlName = TabControl.SelectedTab.Name Then
            Exit Sub
        End If

        '先のページ名
        Select Case _CurrentTabControlName
            Case tpageコマ数.Name
                '
            Case tpage側面と縁.Name
                Hide側面(_clsDataTables)
            Case tpage追加品.Name
                Hide追加品(_clsDataTables)
            Case tpageメモ他.Name
                '
            Case tpage横ひも.Name
                Hide横ひも(_clsDataTables)
            Case tpage縦ひも.Name
                Hide縦ひも(_clsDataTables)
            Case tpageプレビュー.Name
                Hideプレビュー(_clsDataTables)
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


    Sub Dispコマ数(ByVal row底_縦横 As clsDataRow)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Dispコマ数 {0}", row底_縦横.ToString)
        With row底_縦横
            chk縦横側面を展開する.Checked = .Value("f_b展開区分")
            set底の縦横展開(.Value("f_b展開区分"))

            Select Case .Value("f_iコマ上側の縦ひも")
                Case enumコマ上側の縦ひも.i_どちらでも
                    radどちらでも.Checked = True
                Case enumコマ上側の縦ひも.i_左側
                    rad左側.Checked = True
                Case enumコマ上側の縦ひも.i_右側
                    rad右側.Checked = True
            End Select

            dispValidValueNud(nudコマ間のすき間, .Value("f_dひも間のすき間")) 'マイナス不可
            nudひも長加算_縦横.Value = .Value("f_dひも長加算") 'マイナス可

            nud横のコマ数.Value = .Value("f_i横の四角数")
            nud左から何番目のコマ.Value = .Value("f_i左から何番目")
            txt横ひものメモ.Text = .Value("f_s横ひものメモ")

            nud縦のコマ数.Value = .Value("f_i縦の四角数")
            nud上から何番目のコマ.Value = .Value("f_i上から何番目")
            txt縦ひものメモ.Text = .Value("f_s縦ひものメモ")

            nud高さのコマ数.Value = .Value("f_i高さのコマ数")
            nud折り返しコマ数.Value = .Value("f_i折り返しコマ数")
            nudひも長加算_側面.Value = .Value("f_dひも長加算_側面")  'マイナス可
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



    Private Sub Disp計算結果(ByVal calc As clsCalcKnot)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Disp計算結果 {0}", calc.ToString)
        With calc
            '
            txt横ひもの本数.Text = .p_i横ひもの本数
            txt縦ひもの本数.Text = .p_i縦ひもの本数
            txt垂直ひも数.Text = .p_i垂直ひも数

            txt1コマ_寸法.Text = .p_sコマの寸法
            txt1コマ_要尺.Text = .p_sコマの要尺
            txtコマベース_寸法.Text = .p_sコマベース寸法
            txtコマベース_要尺.Text = .p_sコマベース要尺

            txtコマベース_横.Text = .p_sコマベース_横
            txt縁厚さプラス_横.Text = .p_s縁厚さプラス_横
            txtコマベース_縦.Text = .p_sコマベース_縦
            txt縁厚さプラス_縦.Text = .p_s縁厚さプラス_縦
            txtコマベース_高さ.Text = .p_sコマベース_高さ
            txt縁厚さプラス_高さ.Text = .p_s縁厚さプラス_高さ
            txtコマベース_周.Text = .p_sコマベース_周
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
        BindingSource側面と縁.Sort = Nothing
        BindingSource側面と縁.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        Saveコマ数(works.p_row底_縦横) '縦横側面展開値
        If Not _clsCalcKnot.adjust_側面() Then
            MessageBox.Show(_clsCalcKnot.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        BindingSource側面と縁.DataSource = works.p_tbl側面
        BindingSource側面と縁.Sort = "f_i番号 , f_iひも番号"

        dgv側面と縁.Refresh()
    End Sub

    Function Hide側面(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource側面と縁.DataSource)

        BindingSource側面と縁.Sort = Nothing
        BindingSource側面と縁.DataSource = Nothing

        Return ret
    End Function

    Function SaveTables(ByVal works As clsDataTables) As Boolean
        Save目標寸法(works.p_row目標寸法)
        Saveコマ数(works.p_row底_縦横)

        works.CheckPoint(works.p_tbl側面)
        works.CheckPoint(works.p_tbl追加品)

        expand横ひも.Save(enumひも種.i_横, works)
        expand縦ひも.Save(enumひも種.i_縦, works)

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

    Function Saveコマ数(ByVal row底_縦横 As clsDataRow) As Boolean
        With row底_縦横
            .Value("f_b展開区分") = chk縦横側面を展開する.Checked

            Dim iコマ上側の縦ひも As Integer = enumコマ上側の縦ひも.i_どちらでも
            If rad左側.Checked Then
                iコマ上側の縦ひも = enumコマ上側の縦ひも.i_左側
            ElseIf rad右側.Checked Then
                iコマ上側の縦ひも = enumコマ上側の縦ひも.i_右側
            End If
            .Value("f_iコマ上側の縦ひも") = iコマ上側の縦ひも

            .Value("f_dひも間のすき間") = nudコマ間のすき間.Value   'マイナス不可
            .Value("f_dひも長加算") = nudひも長加算_縦横.Value  'マイナス可

            .Value("f_i横の四角数") = nud横のコマ数.Value
            .Value("f_i左から何番目") = nud左から何番目のコマ.Value
            .Value("f_s横ひものメモ") = txt横ひものメモ.Text

            .Value("f_i縦の四角数") = nud縦のコマ数.Value
            .Value("f_i上から何番目") = nud上から何番目のコマ.Value
            .Value("f_s縦ひものメモ") = txt縦ひものメモ.Text

            .Value("f_i高さのコマ数") = nud高さのコマ数.Value
            .Value("f_i折り返しコマ数") = nud折り返しコマ数.Value
            .Value("f_dひも長加算_側面") = nudひも長加算_側面.Value  'マイナス可
        End With
        Return True
    End Function


#End Region

#Region "編集メニューとボタン"
    'バンドの種類選択
    Private Sub ToolStripMenuItemEditSelectBand_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditSelectBand.Click
        Dim dlg As New frmSelectBand
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色と本幅数変更の可能性

        dlg.p_bIsKnotLeft = My.Settings.IsKnotLeft
        dlg.p_dMySafetyFactor = My.Settings.MySafetyFactor
        If dlg.ShowDialog() = DialogResult.OK Then
            My.Settings.IsKnotLeft = dlg.p_bIsKnotLeft
            My.Settings.MySafetyFactor = dlg.p_dMySafetyFactor

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
        If chk縦横側面を展開する.Checked Then
            _clsCalcKnot.prepare縦横展開DataTable()
        End If
        _clsCalcKnot.adjust_側面()

        Dim dlg As New frmColorChange
        dlg.SetDataAndExpand(_clsDataTables, chk縦横側面を展開する.Checked)
        dlg.ShowDialog()
    End Sub


    'リセット
    Private Sub ToolStripMenuItemEditReset_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditReset.Click
        If _clsCalcKnot.IsValidInput() Then
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
        If _clsCalcKnot.IsValidInput() Then
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
        If _clsCalcKnot.CheckTarget(message) Then
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
            MessageBox.Show(_clsCalcKnot.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        '実行する
        If _clsCalcKnot.CalcTarget() Then
            _isLoadingData = True
            '計算結果の縦横値
            Dispコマ数(_clsDataTables.p_row底_縦横)
            '(側面のテーブルも変更されている)
            _isLoadingData = False
        Else
            'エラーあり
            MessageBox.Show(_clsCalcKnot.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        recalc(CalcCategory.NewData)
    End Sub

    'ひもリスト
    Private Sub ToolStripMenuItemEditList_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditList.Click
        SaveTables(_clsDataTables)
        _clsCalcKnot.CalcSize(CalcCategory.NewData, Nothing, Nothing)

        If Not _clsCalcKnot.p_b有効 Then
            Dim msg As String = _clsCalcKnot.p_sメッセージ & vbCrLf
            'このままリスト出力を行いますか？
            msg += My.Resources.AskOutput
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        Dim output As New clsOutput(_sFilePath)
        If Not _clsCalcKnot.CalcOutput(output) Then
            MessageBox.Show(_clsCalcKnot.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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

    'ゲージ
    Private Sub ToolStripMenuItemSettingGauge_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingGauge.Click
        Dim dlg As New frmGauge
        ShowDefaultTabControlPage(enumReason._Gauge Or enumReason._Preview)
        If dlg.ShowDialog() = DialogResult.OK Then
            'ゲージ再計算
            SaveTables(_clsDataTables)
            _clsCalcKnot.SetBandName(g_clsSelectBasics.p_s対象バンドの種類名)
            recalc(CalcCategory.BsMaster)
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
        Saveコマ数(_clsDataTables.p_row底_縦横)

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
                0 < _clsCalcKnot.p_dコマベース_横 AndAlso 0 < _clsCalcKnot.p_dコマベース_縦 Then
                nud横寸法.Value = _clsCalcKnot.p_dコマベース_横
                nud縦寸法.Value = _clsCalcKnot.p_dコマベース_縦
                nud高さ寸法.Value = 1 '空の解除
                nud高さ寸法.Value = _clsCalcKnot.p_dコマベース_高さ
            End If
            'Knot(本幅)横-縦-高さ
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

        '保存名確定
        If String.IsNullOrEmpty(txtタイトル.Text) AndAlso String.IsNullOrEmpty(txt作成者.Text) Then
            txtタイトル.Text = IO.Path.GetFileNameWithoutExtension(SaveFileDialog1.FileName)
            txt作成者.Text = Environment.UserName
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
    Private Sub chk縦横を展開する_CheckedChanged(sender As Object, e As EventArgs) Handles chk縦横側面を展開する.CheckedChanged
        set底の縦横展開(chk縦横側面を展開する.Checked)
    End Sub

    Private Sub nud基本のひも幅_ValueChanged(sender As Object, e As EventArgs) Handles nud基本のひも幅.ValueChanged
        If nud基本のひも幅.Value = 0 Then
            nud基本のひも幅.Value = g_clsSelectBasics.p_i本幅
        End If
        lbl基本のひも幅length.Text = New Length(g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit
        ShowDefaultTabControlPage(enumReason._Preview Or enumReason._Gauge) '色と本幅数変更の可能性

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

    Private Sub nud横のコマ数_ValueChanged(sender As Object, e As EventArgs) Handles nud横のコマ数.ValueChanged
        If Not _isLoadingData Then
            nud左から何番目のコマ.Value = KnotsCenter(nud横のコマ数.Value)
        End If
        recalc(CalcCategory.Knot, sender)
    End Sub

    Private Sub nudコマ間のすき間_ValueChanged(sender As Object, e As EventArgs) Handles nudコマ間のすき間.ValueChanged
        recalc(CalcCategory.Knot, sender)
    End Sub

    Private Sub nudひも長加算_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長加算_縦横.ValueChanged
        recalc(CalcCategory.Knot, sender)
    End Sub

    Private Sub nudひも長加算_側面_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長加算_側面.ValueChanged
        recalc(CalcCategory.Knot, sender)
    End Sub

    Private Sub nud縦のコマ数_ValueChanged(sender As Object, e As EventArgs) Handles nud縦のコマ数.ValueChanged
        If Not _isLoadingData Then
            nud上から何番目のコマ.Value = KnotsCenter(nud縦のコマ数.Value)
        End If
        recalc(CalcCategory.Knot, sender)
    End Sub

    Private Sub nud左から何番目のコマ_ValueChanged(sender As Object, e As EventArgs) Handles nud左から何番目のコマ.ValueChanged
        If nud横のコマ数.Value <= 0 Then
            Exit Sub
        End If
        If nud横のコマ数.Value < nud左から何番目のコマ.Value Then
            nud左から何番目のコマ.Value = nud横のコマ数.Value
            'ElseIf nud左から何番目のコマ.Value <= 0 Then
            '    nud左から何番目のコマ.Value = 1
        End If
    End Sub

    Private Sub nud上から何番目のコマ_ValueChanged(sender As Object, e As EventArgs) Handles nud上から何番目のコマ.ValueChanged
        If nud縦のコマ数.Value <= 0 Then
            Exit Sub
        End If
        If nud縦のコマ数.Value < nud上から何番目のコマ.Value Then
            nud上から何番目のコマ.Value = nud縦のコマ数.Value
            'ElseIf nud上から何番目のコマ.Value <= 0 Then
            '    nud上から何番目のコマ.Value = 1
        End If
    End Sub

    Private Sub nud高さのコマ数_ValueChanged(sender As Object, e As EventArgs) Handles nud高さのコマ数.ValueChanged
        recalc(CalcCategory.Knot, sender)
    End Sub

    Private Sub nud折り返しコマ数_ValueChanged(sender As Object, e As EventArgs) Handles nud折り返しコマ数.ValueChanged
        recalc(CalcCategory.Knot, sender)
    End Sub

    Private Sub cmb基本色_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb基本色.SelectedIndexChanged
        ShowDefaultTabControlPage(enumReason._Preview) '色と本幅数変更の可能性
    End Sub
#End Region

#Region "側面"

    Private Sub dgv_CellFormatting側面と縁(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgv側面と縁.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If
        If dgv.Columns(e.ColumnIndex).DataPropertyName = "f_i何本幅" Then
            For Each col As DataGridViewColumn In dgv.Columns
                If col.DataPropertyName = "f_i番号" Then
                    If dgv.Rows(e.RowIndex).Cells(col.Index).Value = clsDataTables.cHemNumber Then
                        dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = False
                    Else
                        dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = True
                    End If
                    Exit For
                End If
            Next
        End If
        'dgv_CellFormatting(sender, e)
    End Sub

    Private Sub btn追加_側面_Click(sender As Object, e As EventArgs) Handles btn追加_側面.Click
        '編みかた名指定あり
        If Not String.IsNullOrWhiteSpace(cmb編みかた名_側面.Text) Then
            Dim row As tbl側面Row = Nothing
            If _clsCalcKnot.add_側面_縁(cmb編みかた名_側面.Text, nud基本のひも幅.Value, row) Then
                dgv側面と縁.NumberPositionsSelect(row.f_i番号)
                recalc(CalcCategory.Edge, row, "f_i周数")
            Else
                MessageBox.Show(_clsCalcKnot.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            Exit Sub
        End If

        Dim table As tbl側面DataTable = Nothing
        Dim currow As tbl側面Row = Nothing

        'レコードなし
        If _clsDataTables.p_tbl側面.Rows.Count = 0 Then
            currow = _clsCalcKnot.Add側面HeightRow()
            If currow IsNot Nothing Then
                nud高さのコマ数.Value = nud高さのコマ数.Value + 1 '→recalc
                dgv側面と縁.PositionSelect(currow, {"f_i番号", "f_iひも番号"})
            End If
            Exit Sub
        End If

        'レコードあり
        If Not dgv側面と縁.GetTableAndRow(table, currow) Then
            Exit Sub
        End If
        '編みかた名指定なし
        If currow.f_i番号 = clsCalcKnot.cIdxFolding Then
            '折り返しのコマ
            If _clsCalcKnot.add_高さ(currow) Then
                nud折り返しコマ数.Value = nud折り返しコマ数.Value + 1 '→recalc
            Else
                Exit Sub
            End If

        Else
            '縁もしくは高さのコマ
            If _clsCalcKnot.add_高さ(currow) Then
                nud高さのコマ数.Value = nud高さのコマ数.Value + 1 '→recalc
            Else
                Exit Sub
            End If

        End If
        If currow IsNot Nothing Then
            dgv側面と縁.PositionSelect(currow, {"f_i番号", "f_iひも番号"})
        End If
    End Sub

    Private Sub btn削除_側面_Click(sender As Object, e As EventArgs) Handles btn削除_側面.Click
        If dgv側面と縁.Rows.Count = 0 Then
            Exit Sub
        End If

        Dim table As tbl側面DataTable = Nothing
        Dim currow As tbl側面Row = Nothing
        If Not dgv側面と縁.GetTableAndRow(table, currow) Then
            Exit Sub
        End If

        If currow.f_i番号 = clsDataTables.cHemNumber Then
            '縁を削除
            clsDataTables.RemoveNumberFromTable(table, clsDataTables.cHemNumber)
            Exit Sub

        ElseIf currow.f_i番号 = clsCalcknot.cIdxFolding Then
            '折り返しのコマ
            If _clsCalcKnot.del_高さ(currow) Then
                nud折り返しコマ数.Value = nud折り返しコマ数.Value - 1
            Else
                Exit Sub
            End If

        ElseIf currow.f_i番号 = clsCalcknot.cIdxHeight Then
            '高さのコマ
            If _clsCalcKnot.del_高さ(currow) Then
                nud高さのコマ数.Value = nud高さのコマ数.Value - 1
            Else
                Exit Sub
            End If

        Else
            Exit Sub
        End If

        recalc(CalcCategory.Edge, Nothing, Nothing) '計算寸法の再計算
        If currow IsNot Nothing Then
            dgv側面と縁.PositionSelect(currow, {"f_i番号", "f_iひも番号"})
        End If
    End Sub

    'セル変更の再計算は縁に対してのみ
    Private Sub dgv側面_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv側面と縁.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource側面と縁.Current
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
        Else
            If TabControl.TabPages.Contains(tpage横ひも) Then
                TabControl.TabPages.Remove(tpage横ひも)
            End If
            If TabControl.TabPages.Contains(tpage縦ひも) Then
                TabControl.TabPages.Remove(tpage縦ひも)
            End If
        End If
    End Sub

    Private Sub tpage横ひも_Resize(sender As Object, e As EventArgs) Handles tpage横ひも.Resize
        expand横ひも.PanelSize = tpage横ひも.Size
    End Sub

    Private Sub tpage縦ひも_Resize(sender As Object, e As EventArgs) Handles tpage縦ひも.Resize
        expand縦ひも.PanelSize = tpage縦ひも.Size
    End Sub

    Sub Show横ひも(ByVal works As clsDataTables)
        'タブ切り替えタイミングのため、表示は更新済
        Saveコマ数(works.p_row底_縦横)
        expand横ひも.PanelSize = tpage横ひも.Size
        expand横ひも.ShowGrid(_clsCalcKnot.set横展開DataTable(True))
    End Sub

    Function Hide横ひも(ByVal works As clsDataTables) As Boolean
        Return expand横ひも.HideGrid(enumひも種.i_横, works)
    End Function

    Sub Show縦ひも(ByVal works As clsDataTables)
        'タブ切り替えタイミングのため、表示は更新済
        Saveコマ数(works.p_row底_縦横)
        expand縦ひも.PanelSize = tpage縦ひも.Size
        expand縦ひも.ShowGrid(_clsCalcKnot.set縦展開DataTable(True))
    End Sub

    Function Hide縦ひも(ByVal works As clsDataTables) As Boolean
        Return expand縦ひも.HideGrid(enumひも種.i_縦, works)
    End Function

    Private Sub expand横ひも_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.AddButton
        Dim currow As DataRow = e.Row
        If _clsCalcKnot.add_縦横展開(expand横ひも.DataSource, currow) Then
            nud縦のコマ数.Value = nud縦のコマ数.Value + 1 'with recalc
            '
            expand横ひも.DataSource = _clsCalcKnot.set横展開DataTable(True)
            expand横ひも.PositionSelect(currow)
        End If
    End Sub

    Private Sub expand横ひも_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.DeleteButton
        Dim currow As DataRow = e.Row
        If _clsCalcKnot.del_縦横展開(expand横ひも.DataSource, currow) Then
            nud縦のコマ数.Value = nud縦のコマ数.Value - 1 'with recalc
            '
            expand横ひも.DataSource = _clsCalcKnot.set横展開DataTable(True)
            expand横ひも.PositionSelect(currow)
        End If
    End Sub

    Private Sub expand横ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.ResetButton
        expand横ひも.DataSource = _clsCalcKnot.set横展開DataTable(False)
    End Sub

    Private Sub expand縦ひも_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.AddButton
        Dim currow As DataRow = e.Row
        If _clsCalcKnot.add_縦横展開(expand縦ひも.DataSource, currow) Then
            nud横のコマ数.Value = nud横のコマ数.Value + 1 'with recalc
            '
            expand縦ひも.DataSource = _clsCalcKnot.set縦展開DataTable(True)
            expand縦ひも.PositionSelect(currow)
        End If
    End Sub

    Private Sub expand縦ひも_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.DeleteButton
        Dim currow As DataRow = e.Row
        If _clsCalcKnot.del_縦横展開(expand縦ひも.DataSource, currow) Then
            nud横のコマ数.Value = nud横のコマ数.Value - 1 'with recalc
            '
            expand縦ひも.DataSource = _clsCalcKnot.set縦展開DataTable(True)
            expand縦ひも.PositionSelect(currow)
        End If
    End Sub

    Private Sub expand縦ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.ResetButton
        expand縦ひも.DataSource = _clsCalcKnot.set縦展開DataTable(False)
    End Sub

#End Region

#Region "プレビュー"
    Dim _clsImageData As clsImageData
    Private Sub Showプレビュー(works As clsDataTables)
        picプレビュー.Image = Nothing
        _clsImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcKnot.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcKnot) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If

        Cursor.Current = Cursors.WaitCursor
        _clsImageData = New clsImageData(_sFilePath)
        ret = _clsCalcKnot.CalcImage(_clsImageData)
        Cursor.Current = Cursors.Default

        If Not ret AndAlso Not String.IsNullOrWhiteSpace(_clsCalcKnot.p_sメッセージ) Then
            MessageBox.Show(_clsCalcKnot.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
            setDgvColumnsVisible(dgv側面と縁)
            editAddParts.SetDgvColumnsVisible()
            expand横ひも.SetDgvColumnsVisible()
            expand縦ひも.SetDgvColumnsVisible()

            bVisible = True
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", g_clsSelectBasics.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row目標寸法.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row底_縦横.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsCalcKnot.dump())
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
