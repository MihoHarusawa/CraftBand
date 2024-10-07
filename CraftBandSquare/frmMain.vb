Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsUpDown
Imports CraftBand.ctrInsertBand
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBandSquare.clsCalcSquare
Imports CraftBand.mdlColorForm
Imports CraftBand.ctrAddParts

Public Class frmMain

    '画面編集用のワーク
    Dim _clsDataTables As New clsDataTables
    '計算用のワーク
    Dim _clsCalcSquare As New clsCalcSquare(_clsDataTables, Me)

    '編集中のファイルパス
    Friend _sFilePath As String = Nothing '起動時引数があればセット(issue#8)


    Dim _isLoadingData As Boolean = True 'Designer.vb描画


#Region "基本的な画面処理"

    Dim _Profile_dgv側面 As New CDataGridViewProfile(
            (New tbl側面DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _Profile_dgv側面.FormCaption = Me.Text
        dgv側面.SetProfile(_Profile_dgv側面)

        editInsertBand.SetNames(Me.Text, tpage差しひも.Text, My.Resources.EnumStringPlate, My.Resources.EnumStringAngle, My.Resources.EnumStringCenter, My.Resources.EnumStringPosition)

        editAddParts.SetNames(Me.Text, tpage追加品.Text)
        setAddPartsRefNames()
        editUpDown.FormCaption = Me.Text

        expand横ひも.SetNames(Me.Text, tpage横ひも.Text, True, ctrExpanding.enumVisible.i_幅 Or ctrExpanding.enumVisible.i_出力ひも長, My.Resources.CaptionExpand8To2, Nothing)
        expand縦ひも.SetNames(Me.Text, tpage縦ひも.Text, True, ctrExpanding.enumVisible.i_幅 Or ctrExpanding.enumVisible.i_出力ひも長, My.Resources.CaptionExpand4To6, Nothing)


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
        '
        f_s色2.DataSource = g_clsSelectBasics.p_tblColor
        f_s色2.DisplayMember = "Display"
        f_s色2.ValueMember = "Value"
        '
        setBasics(g_clsSelectBasics.p_s対象バンドの種類名 = _clsDataTables.p_row目標寸法.Value("f_sバンドの種類名")) '異なる場合は DispTables内
        setPattern()

        initColorChange() '色変更の初期化
        initColorRepeat() '色と幅の繰り返しの初期化(#51)

        _isLoadingData = False 'Designer.vb描画完了

        DispTables(_clsDataTables) 'バンドの種類変更対応含む

        'サイズ復元
        Dim siz As Size = My.Settings.frmMainSize
        If 0 < siz.Height AndAlso 0 < siz.Width Then
            Me.Size = siz
            Dim colwid As String
            colwid = My.Settings.frmMainGridSide
            Me.dgv側面.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridInsertBand
            Me.editInsertBand.SetColumnWidthFromString(colwid)
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

        My.Settings.frmMainGridSide = Me.dgv側面.GetColumnWidthString()
        My.Settings.frmMainGridInsertBand = Me.editInsertBand.GetColumnWidthString()
        My.Settings.frmMainGridOptions = Me.editAddParts.GetColumnWidthString()
        My.Settings.frmMainGridYoko = Me.expand横ひも.GetColumnWidthString()
        My.Settings.frmMainGridTate = Me.expand縦ひも.GetColumnWidthString()
        My.Settings.frmMainSize = Me.Size
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv側面={0}", My.Settings.frmMainGridSide)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv差しひも={0}", My.Settings.frmMainGridInsertBand)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv追加品={0}", My.Settings.frmMainGridOptions)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv底の横={0}", My.Settings.frmMainGridYoko)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv底の縦={0}", My.Settings.frmMainGridTate)
    End Sub


    '対象バンド・基本値の更新
    Sub setBasics(ByVal isCheckUndef As Boolean)

        With g_clsSelectBasics
            txtバンドの種類名.Text = .p_s対象バンドの種類名

            Dim unitstr As String = .p_unit設定時の寸法単位.Str
            lbl目標寸法_単位.Text = unitstr
            lbl目_ひも間のすき間_単位.Text = unitstr
            lbl計算寸法_単位.Text = unitstr

            lbl目_ひも間のすき間_単位.Text = unitstr
            lblひも長加算_縦横端_単位.Text = unitstr
            lblひも長加算_側面_単位.Text = unitstr


            nud横寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud縦寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud高さ寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces

            nud目_ひも間のすき間.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
            nudひも長加算_縦横端.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nudひも長加算_側面.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces

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

        End With

        '#42
        If isCheckUndef Then
            '未定義色の変更確認
            ShowColorChangeFormForUndef(_clsDataTables, True) '縦横展開を含める

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

        If {CalcCategory.NewData, CalcCategory.BsMaster, CalcCategory.Target, CalcCategory.Target_Band}.Contains(category) Then
            Save目標寸法(_clsDataTables.p_row目標寸法)
        End If
        If {CalcCategory.NewData, CalcCategory.BsMaster, CalcCategory.Square_Gap, CalcCategory.Square_Yoko, CalcCategory.Square_Tate, CalcCategory.Square_Vert, CalcCategory.Square_Expand}.Contains(category) Then
            Save四角数(_clsDataTables.p_row底_縦横)
        End If
        'tableについては更新中をそのまま使用

        Dim ret = _clsCalcSquare.CalcSize(category, ctr, key)
        Disp計算結果(_clsCalcSquare) 'NGはToolStripに表示

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
            Case tpage四角数.Name
                '
            Case tpage側面と縁.Name
                Show側面(works)
            Case tpage差しひも.Name
                Show差しひも(works)
            Case tpageひも上下.Name
                Showひも上下(works)
            Case tpage追加品.Name
                Show追加品(works)
            Case tpageメモ他.Name
                '
            Case tpage横ひも.Name
                Show横ひも(works)
            Case tpage縦ひも.Name
                Show縦ひも(works)
            Case tpageプレビュー.Name
                Showプレビュー()
            Case tpageプレビュー2.Name
                Showプレビュー2()
            Case Else ' 
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "ShowGridSelected TabName={0}", TabControl.SelectedTab.Name)
        End Select
        _CurrentTabControlName = TabControl.SelectedTab.Name
    End Sub

    'デフォルトタブ表示
    <Flags()>
    Enum enumReason
        _GridDropdown = &H1
        _Preview = &H2
        _Pattern = &H4
        _Option = &H8
        _Always = &H100 '#41
    End Enum
    Private Sub ShowDefaultTabControlPage(ByVal reason As enumReason)
        If _isLoadingData Then
            Exit Sub
        End If
        Dim needreset As Boolean = reason.HasFlag(enumReason._Always)
        If Not needreset AndAlso reason.HasFlag(enumReason._GridDropdown) Then
            If {tpage側面と縁.Name, tpage差しひも.Name, tpage追加品.Name, tpage横ひも.Name, tpage縦ひも.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If Not needreset AndAlso reason.HasFlag(enumReason._Preview) Then
            If {tpageプレビュー.Name, tpageプレビュー2.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If Not needreset AndAlso reason.HasFlag(enumReason._Pattern) Then
            If {tpageプレビュー.Name, tpageプレビュー2.Name, tpage側面と縁.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If Not needreset AndAlso reason.HasFlag(enumReason._Option) Then
            If {tpageプレビュー.Name, tpageプレビュー2.Name, tpage追加品.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If needreset Then
            TabControl.SelectTab(tpage四角数.Name)
        End If
    End Sub

    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If TabControl.SelectedTab IsNot Nothing AndAlso
            _CurrentTabControlName = TabControl.SelectedTab.Name Then
            Exit Sub
        End If

        '先のページ名
        Select Case _CurrentTabControlName
            Case tpage四角数.Name
                '
            Case tpage側面と縁.Name
                Hide側面(_clsDataTables)
            Case tpage差しひも.Name
                Hide差しひも(_clsDataTables)
            Case tpageひも上下.Name
                Hideひも上下(_clsDataTables)
            Case tpage追加品.Name
                Hide追加品(_clsDataTables)
            Case tpageメモ他.Name
                '
            Case tpage横ひも.Name
                Hide横ひも(_clsDataTables)
            Case tpage縦ひも.Name
                Hide縦ひも(_clsDataTables)
            Case tpageプレビュー.Name
                Hideプレビュー()
            Case tpageプレビュー2.Name
                Hideプレビュー2()
            Case Else ' 
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "TabControl_SelectedIndexChanged TabName={0}", TabControl.SelectedTab.Name)
        End Select
        _CurrentTabControlName = ""

        ShowGridSelected(_clsDataTables)
    End Sub


    Sub Disp目標寸法(ByVal row目標寸法 As clsDataRow)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp目標寸法() {0}", row目標寸法.ToString)
        With row目標寸法
            If .Value("f_sバンドの種類名") <> g_clsSelectBasics.p_s対象バンドの種類名 Then
                g_clsSelectBasics.SetTargetBandTypeName(.Value("f_sバンドの種類名"), True)
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
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp四角数 {0}", row底_縦横.ToString)
        With row底_縦横
            chk縦横側面を展開する.Checked = .Value("f_b展開区分")
            set底の縦横展開(.Value("f_b展開区分"))

            nud横の目の数.Value = .Value("f_i横の四角数")
            nud縦の目の数.Value = .Value("f_i縦の四角数")
            nud高さの目の数.Value = Int(.Value("f_d高さの四角数"))  'double

            nud横ひもの本数.Value = .Value("f_i長い横ひもの本数")
            nud縦ひもの本数.Value = .Value("f_i縦ひもの本数")
            'nud編みひもの本数.Value = nud高さの目の数.Value

            dispValidValueNud(nud目_ひも間のすき間, .Value("f_dひも間のすき間"))
            nudひも長係数.Value = .Value("f_dひも長係数")
            nudひも長加算_縦横端.Value = .Value("f_d垂直ひも長加算")   'マイナス可
            nudひも長加算_側面.Value = .Value("f_dひも長加算_側面")   'マイナス可

            chk横の補強ひも.Checked = .Value("f_b補強ひも区分")
            chk縦の補強ひも.Checked = .Value("f_b始末ひも区分")

            txt横ひものメモ.Text = .Value("f_s横ひものメモ")
            txt縦ひものメモ.Text = .Value("f_s縦ひものメモ")

            nud左端右端の目.Value = .Value("f_d左端右端の目")
            nud上端下端の目.Value = .Value("f_d上端下端の目")
            nud最下段の目.Value = .Value("f_d最下段の目")

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

    Private Sub Disp計算結果(ByVal calc As clsCalcSquare)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Disp計算結果 {0}", calc.ToString)
        With calc
            '
            txt四角ベース_横.Text = .p_s四角ベース_横
            txt縁厚さプラス_横.Text = .p_s縁厚さプラス_横
            txt四角ベース_縦.Text = .p_s四角ベース_縦
            txt縁厚さプラス_縦.Text = .p_s縁厚さプラス_縦
            txt四角ベース_高さ.Text = .p_s四角ベース_高さ
            txt縁厚さプラス_高さ.Text = .p_s縁厚さプラス_高さ
            txt四角ベース_周.Text = .p_s四角ベース_周
            txt縁厚さプラス_周.Text = .p_s縁厚さプラス_周

            txt縦横_目.Text = .p_s縦横_目
            txt対角線_目.Text = .p_s対角線_目
            txt縦横_四角.Text = .p_s縦横_四角
            txt対角線_四角.Text = .p_s対角線_四角

            txt垂直ひも数.Text = .p_i垂直ひも数
            txt厚さ.Text = .p_s厚さ

            lbl横寸法の差.Text = .p_s横寸法の差
            lbl縦寸法の差.Text = .p_s縦寸法の差
            lbl高さ寸法の差.Text = .p_s高さ寸法の差

            lbl横ひも本幅変更.Visible = .p_b横ひも本幅変更
            lbl縦ひも本幅変更.Visible = .p_b縦ひも本幅変更
            lbl側面ひも本幅変更.Visible = .p_b側面ひも本幅変更
            lblひも本幅変更.Visible = .p_b横ひも本幅変更 OrElse .p_b縦ひも本幅変更 OrElse .p_b側面ひも本幅変更

            If .p_b有効 Then
                ToolStripStatusLabel1.Text = "OK"
                ToolStripStatusLabel2.Text = ""
            Else
                ToolStripStatusLabel1.Text = "NG"
                ToolStripStatusLabel2.Text = .p_sメッセージ
            End If
        End With

    End Sub

    Function SaveTables(ByVal works As clsDataTables) As Boolean
        Save目標寸法(works.p_row目標寸法)
        Save四角数(works.p_row底_縦横)

        works.CheckPoint(works.p_tbl側面)
        works.CheckPoint(works.p_tbl追加品)

        editInsertBand.Save(works)
        expand横ひも.Save(enumひも種.i_横, works)
        expand縦ひも.Save(enumひも種.i_縦, works)

        If _CurrentTabControlName = tpageひも上下.Name Then
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
            .Value("f_b展開区分") = chk縦横側面を展開する.Checked

            .Value("f_i横の四角数") = nud横の目の数.Value
            .Value("f_i縦の四角数") = nud縦の目の数.Value
            .Value("f_d高さの四角数") = nud高さの目の数.Value 'double

            '読み取りますが四角数と合わなければ調整されます
            .Value("f_i長い横ひもの本数") = nud横ひもの本数.Value
            .Value("f_i縦ひもの本数") = nud縦ひもの本数.Value
            '読み取りなし　nud高さの目の数.Value 

            .Value("f_dひも間のすき間") = nud目_ひも間のすき間.Value
            .Value("f_dひも長係数") = nudひも長係数.Value
            .Value("f_d垂直ひも長加算") = nudひも長加算_縦横端.Value   'マイナス可
            .Value("f_dひも長加算_側面") = nudひも長加算_側面.Value 'マイナス可

            .Value("f_b補強ひも区分") = chk横の補強ひも.Checked
            .Value("f_b始末ひも区分") = chk縦の補強ひも.Checked

            .Value("f_s横ひものメモ") = txt横ひものメモ.Text
            .Value("f_s縦ひものメモ") = txt縦ひものメモ.Text

            .Value("f_d左端右端の目") = nud左端右端の目.Value
            .Value("f_d上端下端の目") = nud上端下端の目.Value
            .Value("f_d最下段の目") = nud最下段の目.Value

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

    '色変更の初期化
    Private Function initColorChange() As Boolean
        Const fmt As String = "(f_iひも種={0}) or (f_iひも種={1})"
        Dim cond_tate As String = String.Format(fmt, CType(enumひも種.i_縦, Integer), CType(enumひも種.i_縦 Or enumひも種.i_補強, Integer))
        Dim cond_yoko As String = String.Format(fmt, CType(enumひも種.i_横, Integer), CType(enumひも種.i_横 Or enumひも種.i_補強, Integer))

        Dim _ColorChangeSettings() As CColorChangeSetting = {
           New CColorChangeSetting(tpage側面と縁.Text, enumDataID._tbl側面, "0<f_i番号", False),
           New CColorChangeSetting(tpage差しひも.Text, enumDataID._tbl差しひも, Nothing, False),
           New CColorChangeSetting(tpage追加品.Text, enumDataID._tbl追加品, Nothing, False),
           New CColorChangeSetting(tpage縦ひも.Text, enumDataID._tbl縦横展開, cond_tate, True),
           New CColorChangeSetting(tpage横ひも.Text, enumDataID._tbl縦横展開, cond_yoko, True)}

        Return CreateColorChangeForm(_ColorChangeSettings)
    End Function
    '色変更
    Private Sub ToolStripMenuItemEditColorChange_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditColorChange.Click
        SaveTables(_clsDataTables)
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色変更

        '#40:縦横のレコード数
        If chk縦横側面を展開する.Checked Then
            _clsCalcSquare.prepare縦横展開DataTable()
        End If
        _clsCalcSquare.adjust_側面()

        ShowColorChangeForm(_clsDataTables)
    End Sub

    '色と幅の繰り返しの初期化(#51)
    Private Function initColorRepeat() As Boolean

        Dim _ColorRepeatSettings() As CColorRepeatSetting = {
        New CColorRepeatSetting(lbl側面の編みひも.Text, enumDataID._tbl側面, "f_i番号=1", "f_iひも番号 ASC", True, False),
        New CColorRepeatSetting(tpage縦ひも.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", CType(enumひも種.i_縦, Integer)), "f_iひも番号 ASC", True, True),
        New CColorRepeatSetting(tpage横ひも.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", CType(enumひも種.i_横, Integer)), "f_iひも番号 ASC", True, True)}

        Return CreateColorRepeatForm(_ColorRepeatSettings)
    End Function
    '色と幅の繰り返し
    Private Sub ToolStripMenuItemEditColorRepeat_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditColorRepeat.Click
        SaveTables(_clsDataTables)
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色変更

        If chk縦横側面を展開する.Checked Then
            _clsCalcSquare.prepare縦横展開DataTable()
        End If
        _clsCalcSquare.adjust_側面()

        If 0 < ShowColorRepeatForm(_clsDataTables) Then
            recalc(CalcCategory.BandColor)
        End If
    End Sub

    'リセット
    Private Sub ToolStripMenuItemEditReset_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditReset.Click
        Dim r As DialogResult = DialogResult.Yes
        If _clsCalcSquare.IsValidInput() Then '#22
            '目標寸法以外をリセットします。目(ひも間のすき間)もリセットしてよろしいですか？
            '(はいで全てリセット、いいえで目(ひも間のすき間)を保持)
            r = MessageBox.Show(My.Resources.AskResetInput, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
            If r <> DialogResult.Yes AndAlso r <> DialogResult.No Then
                Exit Sub
            End If
        End If
        Dim d目 As Double = nud目_ひも間のすき間.Value
        ShowDefaultTabControlPage(enumReason._Always)

        _clsDataTables.Clear()
        _clsDataTables.SetInitialValue()
        Save目標寸法(_clsDataTables.p_row目標寸法) '画面値で上書き
        If r = DialogResult.No Then
            _clsDataTables.p_row底_縦横.Value("f_dひも間のすき間") = nud目_ひも間のすき間.Value
        End If

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
        If _clsCalcSquare.IsValidInput() Then
            '規定値をロードします。よろしいですか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskLoadDefault, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
        End If
        If _clsDataTables.Load(My.Settings.DefaultFilePath) Then
            ShowDefaultTabControlPage(enumReason._Always)
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
        If _clsCalcSquare.CheckTarget(message) Then
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
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        '実行する
        ShowDefaultTabControlPage(enumReason._Always)
        If _clsCalcSquare.CalcTarget() Then
            _isLoadingData = True
            '計算結果の縦横値
            Disp四角数(_clsDataTables.p_row底_縦横)
            '(側面のテーブルも変更されている)
            _isLoadingData = False
        Else
            'エラーあり
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        recalc(CalcCategory.NewData)
    End Sub

    'ひもリスト
    Private Sub ToolStripMenuItemEditList_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditList.Click
        SaveTables(_clsDataTables)
        '_clsCalcSquare.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        _clsCalcSquare.CalcSize(CalcCategory.FixLength, Nothing, Nothing)

        If Not _clsCalcSquare.p_b有効 Then
            Dim msg As String = _clsCalcSquare.p_sメッセージ & vbCrLf
            'このままリスト出力を行いますか？
            msg += My.Resources.AskOutput
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        Dim output As New clsOutput(_sFilePath)
        If Not _clsCalcSquare.CalcOutput(output) Then
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
            ShowDefaultTabControlPage(enumReason._Pattern)
            SaveTables(_clsDataTables)
            setPattern()
            recalc(CalcCategory.BsMaster)
        End If
    End Sub

    '付属品
    Private Sub ToolStripMenuItemSettingOptions_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingOptions.Click
        Dim dlg As New frmOptions
        If dlg.ShowDialog() = DialogResult.OK Then
            ShowDefaultTabControlPage(enumReason._Option)
            SaveTables(_clsDataTables)
            'setOptions()
            recalc(CalcCategory.BsMaster)
        End If
    End Sub

    '描画色
    Private Sub ToolStripMenuItemSettingColor_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSettingColor.Click
        Dim dlg As New frmColor
        If dlg.ShowDialog() = DialogResult.OK Then
            ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色にかかわるため
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
        '表面プレビュー
        radおもて.Checked = True
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
                0 < _clsCalcSquare.p_d四角ベース_横 AndAlso 0 < _clsCalcSquare.p_d四角ベース_縦 Then
                nud横寸法.Value = _clsCalcSquare.p_d四角ベース_横
                nud縦寸法.Value = _clsCalcSquare.p_d四角ベース_縦
                nud高さ寸法.Value = 1 '空の解除
                nud高さ寸法.Value = _clsCalcSquare.p_d四角ベース_高さ
            End If
            'Mesh(本幅)横-縦-高さ
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
            ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '#27
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

#Region "目標・四角数のコントロール"
    '縦横の展開チェックボックス　※チェックは最初のタブにある
    Private Sub chk縦横を展開する_CheckedChanged(sender As Object, e As EventArgs) Handles chk縦横側面を展開する.CheckedChanged
        set底の縦横展開(chk縦横側面を展開する.Checked)
        recalc(CalcCategory.Square_Expand, sender)
    End Sub

    Private Sub nud基本のひも幅_ValueChanged(sender As Object, e As EventArgs) Handles nud基本のひも幅.ValueChanged
        If nud基本のひも幅.Value = 0 Then
            nud基本のひも幅.Value = g_clsSelectBasics.p_i本幅
        End If
        lbl基本のひも幅length.Text = New Length(g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit
        If 0 < g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value) Then
            txtひも幅比.Text = (nud目_ひも間のすき間.Value / g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).ToString("F2")
        Else
            txtひも幅比.Text = ""
        End If

        ShowDefaultTabControlPage(enumReason._Preview)
        recalc(CalcCategory.Target_Band, sender)
    End Sub

    Private Sub cmb基本色_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb基本色.SelectedIndexChanged
        ShowDefaultTabControlPage(enumReason._Preview)
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

    Private Sub nud高さ寸法_ValueChanged(sender As Object, e As EventArgs) Handles nud高さ寸法.ValueChanged
        recalc(CalcCategory.Target, sender)
    End Sub

    Private Sub nud編みひもの本数_ValueChanged(sender As Object, e As EventArgs) Handles nud編みひもの本数.ValueChanged
        nud高さの目の数.Value = nud編みひもの本数.Value
        'recalc(CalcCategory.Square_Vert, sender)
    End Sub

    Private Sub nud高さの目の数_ValueChanged(sender As Object, e As EventArgs) Handles nud高さの目の数.ValueChanged
        nud編みひもの本数.Value = nud高さの目の数.Value
        recalc(CalcCategory.Square_Vert, sender)
    End Sub

    Private Sub nud左端右端の目_ValueChanged(sender As Object, e As EventArgs) Handles nud左端右端の目.ValueChanged
        recalc(CalcCategory.Square_Yoko, sender)
    End Sub

    Private Sub nud上端下端の目_ValueChanged(sender As Object, e As EventArgs) Handles nud上端下端の目.ValueChanged
        recalc(CalcCategory.Square_Tate, sender)
    End Sub

    Private Sub nud最下段の目_ValueChanged(sender As Object, e As EventArgs) Handles nud最下段の目.ValueChanged
        recalc(CalcCategory.Square_Vert, sender)
    End Sub
    Private Sub nud横ひもの本数_ValueChanged(sender As Object, e As EventArgs) Handles nud横ひもの本数.ValueChanged
        '奇数推奨
        If nud横ひもの本数.Value Mod 2 = 1 Then
            nud横ひもの本数.Increment = 2
        Else
            nud横ひもの本数.Increment = 1
        End If
        If 1 < nud横ひもの本数.Value Then
            nud縦の目の数.Value = nud横ひもの本数.Value - 1
        Else
            nud縦の目の数.Value = 0
        End If
        recalc(CalcCategory.Square_Tate, sender)
    End Sub

    Private Sub nud縦の目の数_ValueChanged(sender As Object, e As EventArgs) Handles nud縦の目の数.ValueChanged
        '偶数推奨
        If nud縦の目の数.Value Mod 2 = 1 Then
            nud縦の目の数.Increment = 1
        Else
            nud縦の目の数.Increment = 2
        End If
        nud横ひもの本数.Value = nud縦の目の数.Value + 1 'recalc
        'recalc(CalcCategory.Square_Tate, sender)
    End Sub

    Private Sub nud縦ひもの本数_ValueChanged(sender As Object, e As EventArgs) Handles nud縦ひもの本数.ValueChanged
        '奇数推奨
        If nud縦ひもの本数.Value Mod 2 = 1 Then
            nud縦ひもの本数.Increment = 2
        Else
            nud縦ひもの本数.Increment = 1
        End If
        If 1 < nud縦ひもの本数.Value Then
            nud横の目の数.Value = nud縦ひもの本数.Value - 1
        Else
            nud横の目の数.Value = 0
        End If
        recalc(CalcCategory.Square_Yoko, sender)
    End Sub

    Private Sub nud横の目の数_ValueChanged(sender As Object, e As EventArgs) Handles nud横の目の数.ValueChanged
        '偶数推奨
        If nud横の目の数.Value Mod 2 = 1 Then
            nud横の目の数.Increment = 1
        Else
            nud横の目の数.Increment = 2
        End If
        nud縦ひもの本数.Value = nud横の目の数.Value + 1 'recalc
        'recalc(CalcCategory.Square_Yoko, sender)
    End Sub

    Private Sub 目_ひも間のすき間_ValueChanged(sender As Object, e As EventArgs) Handles nud目_ひも間のすき間.ValueChanged
        If 0 < nud目_ひも間のすき間.Value Then
            txt目_本幅分.Text = New Length(nud目_ひも間のすき間.Value).ByLaneText
            txt目対角線_本幅分.Text = New Length(nud目_ひも間のすき間.Value * ROOT2).ByLaneText
            If 0 < g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value) Then
                txtひも幅比.Text = (nud目_ひも間のすき間.Value / g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).ToString("F2")
            Else
                txtひも幅比.Text = ""
            End If
        Else
            txt目_本幅分.Text = ""
        End If

        recalc(CalcCategory.Square_Gap, sender)
    End Sub

    Private Sub nudひも長係数_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長係数.ValueChanged
        recalc(CalcCategory.Square_Gap, sender)
    End Sub

    Private Sub nudひも長加算_縦横端_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長加算_縦横端.ValueChanged
        recalc(CalcCategory.Square_Gap, sender)
    End Sub

    Private Sub nudひも長加算_側面_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長加算_側面.ValueChanged
        recalc(CalcCategory.Square_Vert, sender)
    End Sub
#End Region

#Region "側面"

    Sub Show側面(ByVal works As clsDataTables)
        BindingSource側面.Sort = Nothing
        BindingSource側面.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        Save四角数(works.p_row底_縦横) '縦横側面展開値
        If Not _clsCalcSquare.CalcSize(CalcCategory.SideEdge, Nothing, Nothing) Then '側面と縁
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        BindingSource側面.DataSource = works.p_tbl側面
        If rad下から上へ.Checked Then
            BindingSource側面.Sort = "f_i番号 desc , f_iひも番号 desc"
        Else
            BindingSource側面.Sort = "f_i番号 , f_iひも番号"
        End If

        dgv側面.Refresh()
    End Sub
    Function Hide側面(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource側面.DataSource)

        BindingSource側面.Sort = Nothing
        BindingSource側面.DataSource = Nothing

        Return ret
    End Function


    Private Sub btn追加_側面_Click(sender As Object, e As EventArgs) Handles btn追加_側面.Click
        If dgv側面.Rows.Count = 0 Then
            nud高さの目の数.Value = nud高さの目の数.Value + 1
            Exit Sub
        End If

        Dim table As tbl側面DataTable = Nothing
        Dim currow As tbl側面Row = Nothing
        If Not dgv側面.GetTableAndRow(table, currow) Then
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(cmb編みかた名_側面.Text) Then
            '編みかた名指定なし
            If _clsCalcSquare.add_高さ(currow) Then
                If currow IsNot Nothing Then
                    dgv側面.PositionSelect(currow, {"f_i番号", "f_iひも番号"})
                End If
                nud高さの目の数.Value = nud高さの目の数.Value + 1 '→recalc
            End If
        Else
            '編みかた名指定あり
            Dim row As tbl側面Row = Nothing
            If _clsCalcSquare.add_縁(cmb編みかた名_側面.Text, nud基本のひも幅.Value, row) Then
                dgv側面.NumberPositionsSelect(row.f_i番号)
                recalc(CalcCategory.SideEdge, row, "f_i周数")
            Else
                MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    Private Sub btn削除_側面_Click(sender As Object, e As EventArgs) Handles btn削除_側面.Click
        If dgv側面.Rows.Count = 0 Then
            Exit Sub
        End If

        Dim table As tbl側面DataTable = Nothing
        Dim currow As tbl側面Row = Nothing
        If Not dgv側面.GetTableAndRow(table, currow) Then
            Exit Sub
        End If

        If currow.f_i番号 = clsDataTables.cHemNumber Then
            '縁を削除
            clsDataTables.RemoveNumberFromTable(table, clsDataTables.cHemNumber)
            recalc(CalcCategory.SideEdge, Nothing, Nothing)

        ElseIf currow.f_i番号 = clsCalcSquare.cIdxSpace Then
            '最下段を削除
            nud最下段の目.Value = 0
            recalc(CalcCategory.SideEdge, Nothing, Nothing)

        Else
            '高さを削除
            If _clsCalcSquare.del_高さ(currow) Then
                If currow IsNot Nothing Then
                    dgv側面.PositionSelect(currow, {"f_i番号", "f_iひも番号"})
                End If
                nud高さの目の数.Value = nud高さの目の数.Value - 1 '→recalc
            End If
        End If
    End Sub

    Private Sub rad下から上へ_CheckedChanged(sender As Object, e As EventArgs) Handles rad下から上へ.CheckedChanged, rad上から下へ.CheckedChanged
        If BindingSource側面.DataSource IsNot Nothing Then
            If rad下から上へ.Checked Then
                BindingSource側面.Sort = "f_i番号 desc , f_iひも番号 desc"
            Else
                BindingSource側面.Sort = "f_i番号  , f_iひも番号"
            End If
        End If
    End Sub

    Private Sub dgv側面_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv側面.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource側面.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv側面_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName側面と縁(DataPropertyName) Then
            recalc(CalcCategory.SideEdge, current.Row, DataPropertyName)
        End If
    End Sub

    Private Sub dgv_CellFormatting側面(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgv側面.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource側面.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If
        If dgv.Columns(e.ColumnIndex).DataPropertyName = "f_b集計対象外区分" Then
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
    End Sub


#End Region

#Region "追加品"
    '追加品の参照名 #63
    Sub setAddPartsRefNames()
        Dim names(12) As String

        '横・縦・高さ・周
        names(1) = lbl計算寸法横.Text
        names(2) = lbl計算寸法縦.Text
        names(3) = lbl計算寸法高さ.Text
        names(4) = lbl計算寸法の周.Text

        '四角ベース・縁厚さプラス
        For i = 1 To 4
            names(i + 4) = names(i) & "/" & lbl縁厚さプラス.Text
            names(i) = names(i) & "/" & lbl四角ベース.Text
        Next

        '目標寸法/横・縦・高さ・基本のひも幅
        names(9) = lbl横寸法.Text & "/" & grp目標寸法.Text
        names(10) = lbl縦寸法.Text & "/" & grp目標寸法.Text
        names(11) = lbl高さ寸法.Text & "/" & grp目標寸法.Text
        names(12) = lbl基本のひも幅.Text

        editAddParts.SetRefLenNames(names)
    End Sub

    Sub Show追加品(ByVal works As clsDataTables)
        editAddParts.PanelSize = tpage追加品.Size

        '追加品の参照値 #63
        editAddParts.ShowGrid(works, _clsCalcSquare.getAddPartsRefValues)
        'recalc(CalcCategory.Options)'エラーはイベントで通知
    End Sub

    Function Hide追加品(ByVal works As clsDataTables) As Boolean
        Return editAddParts.HideGrid(works)
        '※エラーメッセージは、他のタブがOKなら上書きされて消えます
    End Function

    Private Sub editAddParts_AddPartsError(sender As Object, e As AddPartsEventArgs) Handles editAddParts.AddPartsError
        recalc(CalcCategory.Options, e.Message)
    End Sub

    Private Sub tpage追加品_Resize(sender As Object, e As EventArgs) Handles tpage追加品.Resize
        editAddParts.PanelSize = tpage追加品.Size
    End Sub

#End Region

#Region "縦横展開"
    'タブの表示・非表示(タブのインスタンスは保持)
    Private Sub set底の縦横展開(ByVal isExband As Boolean)
        If isExband Then
            If Not TabControl.TabPages.Contains(tpage縦ひも) Then
                TabControl.TabPages.Add(tpage縦ひも)
            End If
            If Not TabControl.TabPages.Contains(tpage横ひも) Then
                TabControl.TabPages.Add(tpage横ひも)
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
        Save四角数(works.p_row底_縦横) '補強ひも・キャッシュなし
        expand横ひも.PanelSize = tpage横ひも.Size
        expand横ひも.ShowGrid(_clsCalcSquare.get横展開DataTable())
    End Sub

    Function Hide横ひも(ByVal works As clsDataTables) As Boolean
        Return expand横ひも.HideGrid(enumひも種.i_横, works)
    End Function

    Sub Show縦ひも(ByVal works As clsDataTables)
        Save四角数(works.p_row底_縦横) '補強ひも・キャッシュなし
        expand縦ひも.PanelSize = tpage縦ひも.Size
        expand縦ひも.ShowGrid(_clsCalcSquare.get縦展開DataTable())
    End Sub

    Function Hide縦ひも(ByVal works As clsDataTables) As Boolean
        Return expand縦ひも.HideGrid(enumひも種.i_縦, works)
    End Function


    Private Sub expand横ひも_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.AddButton
        Dim currow As tbl縦横展開Row = e.Row
        If _clsCalcSquare.add_横ひも(currow) Then
            nud縦の目の数.Value = nud縦の目の数.Value + 1 'with recalc
            expand横ひも.PositionSelect(currow)
        End If
    End Sub

    Private Sub expand横ひも_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.DeleteButton
        Dim currow As tbl縦横展開Row = e.Row

        '補強ひも/上端・下端
        If currow IsNot Nothing Then
            Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
            If (iひも種.HasFlag(enumひも種.i_補強) OrElse iひも種.HasFlag(enumひも種.i_すき間)) Then
                _clsCalcSquare.save横展開DataTable()
                If iひも種.HasFlag(enumひも種.i_補強) Then
                    chk横の補強ひも.Checked = False
                ElseIf iひも種.HasFlag(enumひも種.i_すき間) Then
                    nud上端下端の目.Value = 0
                End If
                Save四角数(_clsDataTables.p_row底_縦横)
                expand横ひも.DataSource = _clsCalcSquare.get横展開DataTable()
                Exit Sub
            End If
        End If

        '横ひも
        If _clsCalcSquare.del_横ひも(currow) Then
            nud縦の目の数.Value = nud縦の目の数.Value - 1 'with recalc
            expand横ひも.PositionSelect(currow)
        End If
    End Sub

    Private Sub expand横ひも_CellValueChanged(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.CellValueChanged
        '"f_i何本幅", "f_dひも長加算", "f_dひも長加算2", "f_s色"
        If e.Row Is Nothing OrElse String.IsNullOrEmpty(e.DataPropertyName) Then
            Exit Sub
        End If
        recalc(CalcCategory.Expand_Yoko, e.Row, e.DataPropertyName)
    End Sub

    Private Sub expand横ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.ResetButton
        expand横ひも.DataSource = _clsCalcSquare.get横展開DataTable(True)
    End Sub

    Private Sub expand縦ひも_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.AddButton
        Dim currow As tbl縦横展開Row = e.Row
        If _clsCalcSquare.add_縦ひも(currow) Then
            nud横の目の数.Value = nud横の目の数.Value + 1 'with recalc
            expand縦ひも.PositionSelect(currow)
        End If
    End Sub

    Private Sub expand縦ひも_CellValueChanged(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.CellValueChanged
        '"f_i何本幅", "f_dひも長加算", "f_dひも長加算2", "f_s色"
        If e.Row Is Nothing OrElse String.IsNullOrEmpty(e.DataPropertyName) Then
            Exit Sub
        End If
        recalc(CalcCategory.Expand_Tate, e.Row, e.DataPropertyName)
    End Sub

    Private Sub expand縦ひも_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.DeleteButton
        Dim currow As tbl縦横展開Row = e.Row

        '補強ひも/左端・右端
        If currow IsNot Nothing Then
            Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
            If (iひも種.HasFlag(enumひも種.i_補強) OrElse iひも種.HasFlag(enumひも種.i_すき間)) Then
                _clsCalcSquare.save縦展開DataTable()
                If iひも種.HasFlag(enumひも種.i_補強) Then
                    chk縦の補強ひも.Checked = False
                ElseIf iひも種.HasFlag(enumひも種.i_すき間) Then
                    nud左端右端の目.Value = 0
                End If
                Save四角数(_clsDataTables.p_row底_縦横)
                expand縦ひも.DataSource = _clsCalcSquare.get縦展開DataTable()
                Exit Sub
            End If
        End If

        '縦ひも
        If _clsCalcSquare.del_縦ひも(currow) Then
            nud横の目の数.Value = nud横の目の数.Value - 1 'with recalc
            expand縦ひも.PositionSelect(currow)
        End If
    End Sub

    Private Sub expand縦ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.ResetButton
        expand縦ひも.DataSource = _clsCalcSquare.get縦展開DataTable(True)
    End Sub


#End Region

#Region "差しひも"
    Private Sub tpage差しひも_Resize(sender As Object, e As EventArgs) Handles tpage差しひも.Resize
        editInsertBand.PanelSize = tpage差しひも.Size
    End Sub

    Sub Show差しひも(ByVal works As clsDataTables)
        editInsertBand.PanelSize = tpage差しひも.Size
        editInsertBand.ShowGrid(works)
    End Sub

    Function Hide差しひも(ByVal works As clsDataTables) As Boolean
        Return editInsertBand.HideGrid(works)
    End Function

    Private Sub editInsertBand_CellValueChanged(sender As Object, e As InsertBandEventArgs) Handles editInsertBand.CellValueChanged
        Dim DataPropertyName As String = e.DataPropertyName
        Dim row As tbl差しひもRow = e.Row
        If row Is Nothing Then
            Exit Sub
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} editInsertBand_CellValueChanged({1}){2}", Now, DataPropertyName, New clsDataRow(row).ToString)
        recalc(CalcCategory.Inserted, row, DataPropertyName)
    End Sub

    Private Sub editInsertBand_InnerPositionsSet(sender As Object, e As InsertBandEventArgs) Handles editInsertBand.InnerPositionsSet
        recalc(CalcCategory.Inserted, Nothing, Nothing)
    End Sub
#End Region

#Region "ひも上下"
    Dim _CurrentTargetFace As enumTargetFace


    Sub Showひも上下(ByVal works As clsDataTables)
        If works Is Nothing Then
            Exit Sub
        End If

        editUpDown.Is側面_下から上へ = rad下から上へ.Checked '側面のタブ
        editUpDown.I上右側面本数 = _clsCalcSquare.p_i垂直ひも半数 '下左側面の開始パターン取得用

        _CurrentTargetFace = enumTargetFace.NoDef
        If rad側面_上右.Checked Then
            _CurrentTargetFace = enumTargetFace.Side12
            editUpDown.I水平領域四角数 = _clsCalcSquare.p_i垂直ひも半数
            editUpDown.I垂直領域四角数 = _clsCalcSquare.p_i側面ひもの本数

        ElseIf rad側面_下左.Checked Then
            _CurrentTargetFace = enumTargetFace.Side34
            editUpDown.I水平領域四角数 = _clsCalcSquare.p_i垂直ひも半数
            editUpDown.I垂直領域四角数 = _clsCalcSquare.p_i側面ひもの本数

        ElseIf rad底.Checked Then
            _CurrentTargetFace = enumTargetFace.Bottom
            editUpDown.I水平領域四角数 = _clsCalcSquare.p_i縦ひもの本数
            editUpDown.I垂直領域四角数 = _clsCalcSquare.p_i横ひもの本数
        Else
            Exit Sub
        End If

        editUpDown.PanelSize = tpageひも上下.Size
        editUpDown.ShowGrid(works, _CurrentTargetFace)
    End Sub

    Function saveひも上下(ByVal works As clsDataTables, ByVal isMsg As Boolean) As Boolean
        Return editUpDown.Save(works, isMsg)
    End Function

    Function Hideひも上下(ByVal works As clsDataTables) As Boolean
        Return editUpDown.HideGrid(works)
    End Function

    Private Sub rad底側面_CheckedChanged(sender As Object, e As EventArgs) Handles rad底.CheckedChanged, rad側面_上右.CheckedChanged, rad側面_下左.CheckedChanged
        Dim target As enumTargetFace = enumTargetFace.Bottom
        If rad側面_上右.Checked Then
            target = enumTargetFace.Side12
        ElseIf rad側面_下左.Checked Then
            target = enumTargetFace.Side34
        End If

        If _CurrentTargetFace <> target Then
            editUpDown.Save(_clsDataTables, True)
            Showひも上下(_clsDataTables)
        End If
    End Sub

    Private Sub btn先と同じ_Click(sender As Object, e As EventArgs) Handles btn先と同じ.Click
        Dim prvS As String
        Dim prv As enumTargetFace
        Select Case _CurrentTargetFace
            Case enumTargetFace.Side12
                prv = enumTargetFace.Bottom
                prvS = rad底.Text
            Case enumTargetFace.Side34
                prv = enumTargetFace.Side12
                prvS = rad側面_上右.Text
            Case Else 'bottom
                prv = enumTargetFace.Side34
                prvS = rad側面_下左.Text
        End Select
        '現編集内容を破棄し{0}と同じにします。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(String.Format(My.Resources.AskLoadSameAs, prvS),
                                                Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If

        Dim updown As New clsUpDown(prv)
        If _clsDataTables.ToClsUpDown(updown) Then
            editUpDown.Replace(updown)
        Else
            '{0}のデータを表示できませんでした。
            MessageBox.Show(String.Format(My.Resources.MessageReplaceError, prvS),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub tpageひも上下_Resize(sender As Object, e As EventArgs) Handles tpageひも上下.Resize
        editUpDown.PanelSize = tpageひも上下.Size
    End Sub

#End Region

#Region "プレビュー"
    Dim _clsImageData As clsImageData
    Private Sub Showプレビュー()
        picプレビュー.Image = Nothing
        _clsImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcSquare.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcSquare) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If

        Dim data As clsDataTables = _clsDataTables
        Dim calc As clsCalcSquare = _clsCalcSquare
        Dim isBackFace As Boolean = False
        If radうら.Checked Then
            data = _clsDataTables.LeftSideRightData()
            calc = New clsCalcSquare(data, Me)
            isBackFace = True
            If Not calc.CalcSize(CalcCategory.NewData, Nothing, Nothing) Then
                Return  '先にOKならOKのはずだが
            End If
        End If
        Cursor.Current = Cursors.WaitCursor
        _clsImageData = New clsImageData(_sFilePath)
        ret = calc.CalcImage(_clsImageData, isBackFace)
        Cursor.Current = Cursors.Default

        If Not ret AndAlso Not String.IsNullOrWhiteSpace(calc.p_sメッセージ) Then
            MessageBox.Show(calc.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        picプレビュー.Image = System.Drawing.Image.FromFile(_clsImageData.GifFilePath)
    End Sub

    Private Sub Hideプレビュー()
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
        If Not _clsImageData.ImgBrowserOpen(IIf(radうら.Checked, clsImageData.cBrowserBackFace, clsImageData.cBrowserBasicInfo)) Then
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

    Private Sub radおもてうら_CheckChanged(sender As Object, e As EventArgs) Handles radおもて.CheckedChanged ', radうら.CheckedChanged
        If _clsImageData Is Nothing Then
            Return
        End If
        Showプレビュー()
    End Sub

#End Region

#Region "プレビュー2"
    Dim _clsModelImageData As clsModelSquare
    Private Sub Showプレビュー2()
        picプレビュー2.Image = Nothing
        _clsModelImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcSquare.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcSquare) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If

        Cursor.Current = Cursors.WaitCursor
        _clsModelImageData = New clsModelSquare(_clsCalcSquare, _sFilePath)
        ret = _clsModelImageData.CalcModel()
        Cursor.Current = Cursors.Default

        If Not ret AndAlso Not String.IsNullOrWhiteSpace(_clsModelImageData.LastError) Then
            MessageBox.Show(_clsModelImageData.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        picプレビュー2.Image = System.Drawing.Image.FromFile(_clsModelImageData.GifFilePath)
    End Sub

    Private Sub Hideプレビュー2()
        picプレビュー2.Image = Nothing
        If _clsModelImageData IsNot Nothing Then
            _clsModelImageData.Clear()
            _clsModelImageData = Nothing
        End If
    End Sub

    Private Sub btn3Dモデル_Click(sender As Object, e As EventArgs) Handles btn3Dモデル.Click
        If _clsModelImageData Is Nothing Then
            Return
        End If
        If Not _clsModelImageData.ModelFileOpen Then
            MessageBox.Show(_clsModelImageData.LastError, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btn画像ファイル2_Click(sender As Object, e As EventArgs) Handles btn画像ファイル2.Click
        If _clsModelImageData Is Nothing Then
            Return
        End If
        If Not _clsModelImageData.ImgFileOpen Then
            MessageBox.Show(_clsModelImageData.LastError, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btnブラウザ2_Click(sender As Object, e As EventArgs) Handles btnブラウザ2.Click
        If _clsModelImageData Is Nothing Then
            Return
        End If
        If Not _clsModelImageData.ImgBrowserOpen(clsImageData.cBrowserSize) Then
            MessageBox.Show(_clsModelImageData.LastError, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
#End Region


#Region "DEBUG"
    Dim bVisible As Boolean = False
    Private Sub btnDEBUG_Click(sender As Object, e As EventArgs) Handles btnDEBUG.Click
        If Not bVisible Then
            setDgvColumnsVisible(dgv側面)
            editInsertBand.SetDgvColumnsVisible()
            editAddParts.SetDgvColumnsVisible()
            expand横ひも.SetDgvColumnsVisible()
            expand縦ひも.SetDgvColumnsVisible()
            expand縦ひも.SetDgvColumnsVisible()
            bVisible = True
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", g_clsSelectBasics.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row目標寸法.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row底_縦横.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsCalcSquare.dump())
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tbl側面).ToString())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tbl追加品).ToString())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tbl縦横展開).ToString())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tbl差しひも).ToString())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tblひも上下).ToString())

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
