
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.ctrAddParts
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBandMesh.clsCalcMesh

Public Class frmMain

    '画面編集用のワーク
    Dim _clsDataTables As New clsDataTables
    '計算用のワーク
    Dim _clsCalcMesh As New clsCalcMesh(_clsDataTables, Me)

    '編集中のファイルパス
    Friend _sFilePath As String = Nothing '起動時引数があればセット(issue#8)


    Dim _isLoadingData As Boolean = True 'Designer.vb描画


#Region "基本的な画面処理"

    Dim _Profile_dgv底楕円 As New CDataGridViewProfile(
            (New tbl底_楕円DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow Or enumAction._RowHeight_iひも番号
            )
    Dim _Profile_dgv側面 As New CDataGridViewProfile(
            (New tbl側面DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow Or enumAction._RowHeight_iひも番号
            )


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _Profile_dgv底楕円.FormCaption = Me.Text
        dgv底楕円.SetProfile(_Profile_dgv底楕円)

        _Profile_dgv側面.FormCaption = Me.Text
        dgv側面.SetProfile(_Profile_dgv側面)

        editAddParts.SetNames(Me.Text, tpage追加品.Text)
        setAddPartsRefNames()

        expand横ひも.SetNames(Me.Text, tpage横ひも.Text, True, ctrExpanding.enumVisible.i_幅 Or ctrExpanding.enumVisible.i_出力ひも長, My.Resources.CaptionExpand8To2, My.Resources.CaptionExpand4To6)
        expand縦ひも.SetNames(Me.Text, tpage縦ひも.Text, True, ctrExpanding.enumVisible.i_幅 Or ctrExpanding.enumVisible.i_出力ひも長, My.Resources.CaptionExpand4To6, My.Resources.CaptionExpand8To2)


#If DEBUG Then
        btnDEBUG.Visible = (clsLog.LogLevel.Trouble <= g_clsLog.Level)
#Else
        btnDEBUG.Visible = (clsLog.LogLevel.Debug <= g_clsLog.Level)
#End If

        frmSaveTemporarily.ClearSaved()

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
        f_i何本幅1.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅1.DisplayMember = "Display"
        f_i何本幅1.ValueMember = "Value"
        f_i何本幅2.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅2.DisplayMember = "Display"
        f_i何本幅2.ValueMember = "Value"

        f_s色1.DataSource = g_clsSelectBasics.p_tblColor
        f_s色1.DisplayMember = "Display"
        f_s色1.ValueMember = "Value"
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
            colwid = My.Settings.frmMainGridOval
            Me.dgv底楕円.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridSide
            Me.dgv側面.SetColumnWidthFromString(colwid)
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

        My.Settings.frmMainGridOval = Me.dgv底楕円.GetColumnWidthString()
        My.Settings.frmMainGridSide = Me.dgv側面.GetColumnWidthString()
        My.Settings.frmMainGridOptions = Me.editAddParts.GetColumnWidthString()
        My.Settings.frmMainGridYoko = Me.expand横ひも.GetColumnWidthString()
        My.Settings.frmMainGridTate = Me.expand縦ひも.GetColumnWidthString()
        My.Settings.frmMainSize = Me.Size
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv底楕円={0}", My.Settings.frmMainGridOval)
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
            lblひとつのすき間の寸法_単位.Text = unitstr
            lbl計算寸法_単位.Text = unitstr
            lbl横ひも間のすき間_単位.Text = unitstr
            lbl垂直ひも長加算_単位.Text = unitstr
            lbl楕円底円弧の半径加算_単位.Text = unitstr
            lbl楕円底周の加算_単位.Text = unitstr


            nud横寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud縦寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud高さ寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud垂直ひも長加算.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces

            nudひとつのすき間の寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
            nud横ひも間のすき間.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
            nud楕円底円弧の半径加算.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
            nud楕円底周の加算.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1

            Dim maxLane As Integer = .p_i本幅
            nud基本のひも幅.Maximum = maxLane '表示中のValueは追随
            nud長い横ひも.Maximum = maxLane
            nud短い横ひも.Maximum = maxLane
            nud最上と最下の短いひもの幅.Maximum = maxLane
            nud縦ひも.Maximum = maxLane

            '色リスト
            '#29
            cmb基本色.Items.Clear()
            For Each r As dstWork.tblColorRow In g_clsSelectBasics.p_tblColor
                cmb基本色.Items.Add(r.Display)
            Next

            lbl基本のひも幅length.Text = New Length(.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit
            lbl短い横ひもlength.Text = New Length(.p_d指定本幅(nud短い横ひも.Value)).TextWithUnit

            'Grid
            Dim format As String = String.Format("N{0}", .p_unit設定時の寸法単位.DecimalPlaces)
            Me.f_d径1.DefaultCellStyle.Format = format
            Me.f_d径の累計1.DefaultCellStyle.Format = format
            Me.f_d円弧部分長1.DefaultCellStyle.Format = format
            Me.f_d差しひも間のすき間1.DefaultCellStyle.Format = format
            Me.f_d連続ひも長1.DefaultCellStyle.Format = format
            Me.f_d周長1.DefaultCellStyle.Format = format
            Me.f_dひも長1.DefaultCellStyle.Format = format

            Me.f_d高さ2.DefaultCellStyle.Format = format
            Me.f_d垂直ひも長2.DefaultCellStyle.Format = format
            Me.f_d周長2.DefaultCellStyle.Format = format
            Me.f_dひも長2.DefaultCellStyle.Format = format
            Me.f_d連続ひも長2.DefaultCellStyle.Format = format

            If Not chk楕円底個別設定.Checked Then
                nud楕円底円弧の半径加算.Value = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底円弧の半径加算")
                nud楕円底周の加算.Value = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底周の加算")
            End If

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
        cmb編みかた名_底楕円.Items.Clear()
        cmb編みかた名_底楕円.Items.AddRange(g_clsMasterTables.GetPatternNames(False, True))

        If chk縁の始末.Checked Then
            cmb編みかた名_側面.Items.Clear()
            cmb編みかた名_側面.Items.AddRange(g_clsMasterTables.GetPatternNames(True, False))
        Else
            cmb編みかた名_側面.Items.Clear()
            cmb編みかた名_側面.Items.AddRange(g_clsMasterTables.GetPatternNames(False, False))
        End If
    End Sub

    '再計算
    Private Function recalc(ByVal category As CalcCategory, Optional ByVal ctr As Object = Nothing, Optional ByVal key As Object = Nothing) As Boolean
        If _isLoadingData Then
            Return True
        End If

        If {CalcCategory.NewData, CalcCategory.Target}.Contains(category) Then
            Save目標寸法(_clsDataTables.p_row目標寸法)
        End If
        If {CalcCategory.NewData, CalcCategory.Expand, CalcCategory.Horizontal, CalcCategory.Vertical, CalcCategory.Oval}.Contains(category) Then
            Save底_縦横(_clsDataTables.p_row底_縦横)
        End If
        'tableについては更新中をそのまま使用

        Dim ret = _clsCalcMesh.CalcSize(category, ctr, key)
        Disp計算結果(_clsCalcMesh) 'NGはToolStripに表示

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
                Disp底_縦横(works.p_row底_縦横)
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
            Case tpage底縦横.Name
                '
            Case tpage底楕円.Name
                Show底_楕円(works)
            Case tpage側面.Name
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
        _Pattern = &H4
        _Option = &H8
        _Always = &H100
    End Enum
    Private Sub ShowDefaultTabControlPage(ByVal reason As enumReason)
        If _isLoadingData Then
            Exit Sub
        End If
        Dim needreset As Boolean = reason.HasFlag(enumReason._Always)
        If Not needreset AndAlso reason.HasFlag(enumReason._GridDropdown) Then
            If {tpage底楕円.Name, tpage側面.Name, tpage追加品.Name, tpage横ひも.Name, tpage縦ひも.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If Not needreset AndAlso reason.HasFlag(enumReason._Preview) Then
            If {tpageプレビュー.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If Not needreset AndAlso reason.HasFlag(enumReason._Pattern) Then
            If {tpageプレビュー.Name, tpage底楕円.Name, tpage側面.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If Not needreset AndAlso reason.HasFlag(enumReason._Option) Then
            If {tpageプレビュー.Name, tpage追加品.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If needreset Then
            TabControl.SelectTab(tpage底縦横.Name)
        End If
    End Sub

    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If TabControl.SelectedTab IsNot Nothing AndAlso
            _CurrentTabControlName = TabControl.SelectedTab.Name Then
            Exit Sub
        End If

        '先のページ名
        Select Case _CurrentTabControlName
            Case tpage底縦横.Name
                '
            Case tpage底楕円.Name
                Hide底_楕円(_clsDataTables)
            Case tpage側面.Name
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


    Sub Disp底_縦横(ByVal row底_縦横 As clsDataRow)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp底_縦横 {0}", row底_縦横.ToString)
        With row底_縦横
            chk縦横を展開する.Checked = .Value("f_b展開区分")
            chk縦ひもを放射状に置く.Checked = (0 < .Value("f_i織りタイプ"))
            set底の縦横展開(.Value("f_b展開区分"))

            nud長い横ひもの本数.Value = .Value("f_i長い横ひもの本数")
            dispAdjustLane(nud長い横ひも, .Value("f_i長い横ひも"))
            dispAdjustLane(nud短い横ひも, .Value("f_i短い横ひも"))
            dispAdjustLane(nud最上と最下の短いひもの幅, .Value("f_i最上と最下の短いひもの幅"))
            Dim rad As enum最上と最下の短いひも = .Value("f_i最上と最下の短いひも")
            If rad = enum最上と最下の短いひも.i_なし Then
                rad最上と最下の短いひも_なし.Checked = True
            ElseIf rad = enum最上と最下の短いひも.i_同じ幅 Then
                rad最上と最下の短いひも_同じ幅.Checked = True
            Else
                rad最上と最下の短いひも_異なる幅.Checked = True
            End If
            nud横ひも間のすき間.Value = .Value("f_d横ひも間のすき間") 'マイナス可
            chk補強ひも.Checked = .Value("f_b補強ひも区分")
            txt横ひものメモ.Text = .Value("f_s横ひものメモ")

            nud垂直ひも長加算.Value = .Value("f_d垂直ひも長加算") 'マイナス可
            chk斜めの補強ひも.Checked = .Value("f_b斜めの補強ひも区分")

            dispAdjustLane(nud縦ひも, .Value("f_i縦ひも"))
            nud縦ひもの本数.Value = .Value("f_i縦ひもの本数")
            dispValidValueNud(nudひとつのすき間の寸法, .Value("f_dひとつのすき間の寸法"))
            chk始末ひも.Checked = .Value("f_b始末ひも区分")
            txt縦ひものメモ.Text = .Value("f_s縦ひものメモ")

            chk楕円底個別設定.Checked = .Value("f_b楕円底個別設定")
            nud楕円底円弧の半径加算.Value = .Value("f_d楕円底円弧の半径加算")
            nud楕円底周の加算.Value = .Value("f_d楕円底周の加算")
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


    Private Sub Disp計算結果(ByVal calc As clsCalcMesh)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Disp計算結果 {0}", calc.ToString)
        With calc
            '
            txt内側_横.Text = .p_s内側_横
            txt外側_横.Text = .p_s外側_横
            txt内側_縦.Text = .p_s内側_縦
            txt外側_縦.Text = .p_s外側_縦
            txt内側_高さ.Text = .p_s内側_高さ
            txt外側_高さ.Text = .p_s外側_高さ
            txt内側_底の周.Text = .p_s内側_底の周
            txt外側_底の周.Text = .p_s外側_底の周

            txt内側_最大周.Text = .p_s内側_最大周
            txt外側_最大周.Text = .p_s外側_最大周
            txt内側_最小横.Text = .p_s内側_最小横
            txt外側_最大横.Text = .p_s外側_最大横
            txt内側_最小縦.Text = .p_s内側_最小縦
            txt外側_最大縦.Text = .p_s外側_最大縦

            txt内側_最大周の径.Text = .p_s内側_最大周の径
            txt外側_最大周の径.Text = .p_s外側_最大周の径


            txt垂直ひも数.Text = .p_i垂直ひも数
            txt底楕円の径.Text = .p_s径の合計
            txt厚さ.Text = .p_s厚さ

            lbl横寸法の差.Text = .p_s横寸法の差
            lbl縦寸法の差.Text = .p_s縦寸法の差
            lbl高さ寸法の差.Text = .p_s高さ寸法の差

            lbl縦置きの計.Text = .p_s縦横の横
            lbl横置きの計.Text = .p_s縦横の縦

            If .p_b有効 Then
                ToolStripStatusLabel1.Text = "OK"
                ToolStripStatusLabel2.Text = ""
            Else
                ToolStripStatusLabel1.Text = "NG"
                ToolStripStatusLabel2.Text = .p_sメッセージ
            End If
        End With

    End Sub


    Sub Show底_楕円(ByVal works As clsDataTables)
        BindingSource底_楕円.Sort = Nothing
        BindingSource底_楕円.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        BindingSource底_楕円.DataSource = works.p_tbl底_楕円
        BindingSource底_楕円.Sort = "f_i番号 , f_iひも番号"
    End Sub

    Sub Show側面(ByVal works As clsDataTables)
        BindingSource側面.Sort = Nothing
        BindingSource側面.DataSource = Nothing
        If works Is Nothing Then
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

    Function Hide底_楕円(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource底_楕円.DataSource)

        BindingSource底_楕円.Sort = Nothing
        BindingSource底_楕円.DataSource = Nothing

        Return ret
    End Function

    Function Hide側面(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource側面.DataSource)

        BindingSource側面.Sort = Nothing
        BindingSource側面.DataSource = Nothing

        Return ret
    End Function


    Function SaveTables(ByVal works As clsDataTables) As Boolean
        Save目標寸法(works.p_row目標寸法)
        Save底_縦横(works.p_row底_縦横)

        works.CheckPoint(works.p_tbl底_楕円)
        works.CheckPoint(works.p_tbl側面)
        works.CheckPoint(works.p_tbl追加品)

        expand横ひも.Save(enumひも種.i_横, works)
        expand縦ひも.Save(enumひも種.i_縦 Or enumひも種.i_斜め, works)

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

    Function Save底_縦横(ByVal row底_縦横 As clsDataRow) As Boolean
        With row底_縦横
            .Value("f_b展開区分") = chk縦横を展開する.Checked

            .Value("f_i長い横ひもの本数") = nud長い横ひもの本数.Value
            .Value("f_i長い横ひも") = nud長い横ひも.Value
            .Value("f_i短い横ひも") = nud短い横ひも.Value
            Dim rad As enum最上と最下の短いひも = enum最上と最下の短いひも.i_なし
            If rad最上と最下の短いひも_同じ幅.Checked Then
                rad = enum最上と最下の短いひも.i_同じ幅
            ElseIf rad最上と最下の短いひも_異なる幅.Checked Then
                rad = enum最上と最下の短いひも.i_異なる幅
            End If
            .Value("f_i最上と最下の短いひも") = CType(rad, Int16)
            .Value("f_i最上と最下の短いひもの幅") = nud最上と最下の短いひもの幅.Value
            .Value("f_d横ひも間のすき間") = nud横ひも間のすき間.Value   'マイナス可
            .Value("f_b補強ひも区分") = chk補強ひも.Checked
            .Value("f_s横ひものメモ") = txt横ひものメモ.Text

            .Value("f_d垂直ひも長加算") = nud垂直ひも長加算.Value  'マイナス可
            .Value("f_b斜めの補強ひも区分") = chk斜めの補強ひも.Checked

            .Value("f_i縦ひも") = nud縦ひも.Value
            .Value("f_i縦ひもの本数") = nud縦ひもの本数.Value
            .Value("f_dひとつのすき間の寸法") = nudひとつのすき間の寸法.Value
            .Value("f_b始末ひも区分") = chk始末ひも.Checked
            .Value("f_s縦ひものメモ") = txt縦ひものメモ.Text

            If chk縦ひもを放射状に置く.Checked Then
                .Value("f_i織りタイプ") = enum縦ひも配置.i_放射状 '1
            Else
                .Value("f_i織りタイプ") = enum縦ひも配置.i_縦横 '0
            End If

            .Value("f_b楕円底個別設定") = chk楕円底個別設定.Checked
            .Value("f_d楕円底円弧の半径加算") = nud楕円底円弧の半径加算.Value
            .Value("f_d楕円底周の加算") = nud楕円底周の加算.Value
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
        Const fmt3 As String = "(f_iひも種={0}) or (f_iひも種={1}) or (f_iひも種={2})"
        Const fmt4 As String = "(f_iひも種={0}) or (f_iひも種={1}) or (f_iひも種={2}) or (f_iひも種={3})"
        Dim cond_yoko As String = String.Format(fmt4, CType(enumひも種.i_横 Or enumひも種.i_長い, Integer), CType(enumひも種.i_横 Or enumひも種.i_短い, Integer),
                    CType(enumひも種.i_横 Or enumひも種.i_最上と最下, Integer), CType(enumひも種.i_横 Or enumひも種.i_補強, Integer))
        Dim cond_tate As String = String.Format(fmt3, CType(enumひも種.i_縦, Integer), CType(enumひも種.i_縦 Or enumひも種.i_補強, Integer),
                    CType(enumひも種.i_斜め Or enumひも種.i_補強, Integer))

        Dim _ColorChangeSettings() As CColorChangeSetting = {
           New CColorChangeSetting(tpage底楕円.Text, enumDataID._tbl底_楕円, Nothing, False),
           New CColorChangeSetting(tpage側面.Text, enumDataID._tbl側面, Nothing, False),
           New CColorChangeSetting(tpage追加品.Text, enumDataID._tbl追加品, Nothing, False),
           New CColorChangeSetting(tpage横ひも.Text, enumDataID._tbl縦横展開, cond_yoko, True),
           New CColorChangeSetting(tpage縦ひも.Text, enumDataID._tbl縦横展開, cond_tate, True)}

        Return CreateColorChangeForm(_ColorChangeSettings)
    End Function
    '色変更
    Private Sub ToolStripMenuItemEditColorChange_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditColorChange.Click
        SaveTables(_clsDataTables)
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色変更

        '#40:縦横のレコード数
        If chk縦横を展開する.Checked Then
            _clsCalcMesh.prepare縦横展開DataTable()
        End If

        ShowColorChangeForm(_clsDataTables)
    End Sub

    '色と幅の繰り返しの初期化(#51)
    Private Function initColorRepeat() As Boolean
        Dim cond_short As String = String.Format("(f_iひも種={0}) or (f_iひも種={1})",
                                                 CType(enumひも種.i_横 Or enumひも種.i_短い, Integer), CType(enumひも種.i_横 Or enumひも種.i_最上と最下, Integer))

        Dim _ColorRepeatSettings() As CColorRepeatSetting = {
        New CColorRepeatSetting(lbl長い横ひも.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", CType(enumひも種.i_横 Or enumひも種.i_長い, Integer)), "f_iひも番号 ASC", True, True),
        New CColorRepeatSetting(lbl短い横ひも.Text, enumDataID._tbl縦横展開, cond_short, "f_i位置番号 ASC", True, True),
        New CColorRepeatSetting(lbl縦ひも.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", CType(enumひも種.i_縦, Integer)), "f_iひも番号 ASC", True, True)}

        Return CreateColorRepeatForm(_ColorRepeatSettings)
    End Function
    '色と幅の繰り返し
    Private Sub ToolStripMenuItemEditColorRepeat_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditColorRepeat.Click
        SaveTables(_clsDataTables)
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色変更

        If chk縦横を展開する.Checked Then
            _clsCalcMesh.prepare縦横展開DataTable()
        End If

        If 0 < ShowColorRepeatForm(_clsDataTables) Then
            recalc(CalcCategory.BandColor)
        End If
    End Sub

    'リセット
    Private Sub ToolStripMenuItemEditReset_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditReset.Click
        If _clsCalcMesh.IsValidInput() Then
            '目標寸法以外をリセットします。よろしいですか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetInput, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
        End If
        ShowDefaultTabControlPage(enumReason._Always)

        _clsDataTables.Clear()
        _clsDataTables.SetInitialValue()
        Save目標寸法(_clsDataTables.p_row目標寸法) '画面値
        _clsDataTables.p_row底_縦横.Value("f_i長い横ひも") = nud基本のひも幅.Value
        _clsDataTables.p_row底_縦横.Value("f_i短い横ひも") = nud基本のひも幅.Value
        _clsDataTables.p_row底_縦横.Value("f_i縦ひも") = nud基本のひも幅.Value

        DispTables(_clsDataTables)
    End Sub

    '規定値
    Private Sub ToolStripMenuItemEditDefault_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditDefault.Click
        SaveTables(_clsDataTables) 'for Cancel

        '#88
        If frmLoadDefault.LoadDefault(My.Settings.DefaultFilePath, _clsDataTables) Then
            ShowDefaultTabControlPage(enumReason._Always)
            DispTables(_clsDataTables)
        End If
    End Sub

    '概算
    Private Sub ToolStripMenuItemEditCalc_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditCalc.Click
        SaveTables(_clsDataTables)
        '実行前のチェック
        Dim message As String = Nothing
        If _clsCalcMesh.CheckTarget(message) Then
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
            MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        '実行する
        ShowDefaultTabControlPage(enumReason._Always)
        chk縦横を展開する.Checked = False

        If _clsCalcMesh.CalcTarget() Then
            _isLoadingData = True
            '計算結果の縦横値
            Disp底_縦横(_clsDataTables.p_row底_縦横)
            '(側面のテーブルも変更されている)
            _isLoadingData = False
        Else
            'エラーあり
            MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        recalc(CalcCategory.NewData)
    End Sub

    'ひもリスト
    Private Sub ToolStripMenuItemEditList_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditList.Click
        SaveTables(_clsDataTables)
        _clsCalcMesh.CalcSize(CalcCategory.NewData, Nothing, Nothing)

        If Not _clsCalcMesh.p_b有効 Then
            Dim msg As String = _clsCalcMesh.p_sメッセージ & vbCrLf
            'このままリスト出力を行いますか？
            msg += My.Resources.AskOutput
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        Dim output As New clsOutput(_sFilePath)
        If Not _clsCalcMesh.CalcOutput(output) Then
            MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim dlg As New frmOutput(output)
        dlg.Icon = Me.Icon
        dlg.ShowDialog()
    End Sub

    '規定値保存
    Private Sub ToolStripMenuItemEditDefaultFile_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditDefaultFile.Click
        '#88
        Dim strPath As String = My.Settings.DefaultFilePath
        If frmLoadDefault.DefaultFolder(strPath) Then
            My.Settings.DefaultFilePath = strPath
        End If
    End Sub

    '一時保存
    Private Sub ToolStripMenuItemEditSaveTemporarily_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditSaveTemporarily.Click
        SaveTables(_clsDataTables)
        Dim dlg As New frmSaveTemporarily(_clsDataTables)
        If dlg.ShowDialog() = DialogResult.OK Then
            DispTables(_clsDataTables)
            setStartEditing(True)
        End If
    End Sub

    Private Sub btn一時保存_Click(sender As Object, e As EventArgs) Handles btn一時保存.Click
        ToolStripMenuItemEditSaveTemporarily.PerformClick()
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
        Save底_縦横(_clsDataTables.p_row底_縦横)

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
            frmSaveTemporarily.ClearSaved()

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
                0 < _clsCalcMesh.p_d内側_横 AndAlso 0 < _clsCalcMesh.p_d内側_縦 Then
                nud横寸法.Value = _clsCalcMesh.p_d内側_横
                nud縦寸法.Value = _clsCalcMesh.p_d内側_縦
                nud高さ寸法.Value = 1 '空の解除
                nud高さ寸法.Value = _clsCalcMesh.p_d内側_高さ
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
            If _clsDataTables.Load(fname) Then
                frmSaveTemporarily.ClearSaved()

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

#Region "底(縦横)のコントロール"
    '縦横の展開チェックボックス　※チェックは最初のタブにある
    Private Sub chk縦横を展開する_CheckedChanged(sender As Object, e As EventArgs) Handles chk縦横を展開する.CheckedChanged
        set底の縦横展開(chk縦横を展開する.Checked)
        btn展開本幅の同期.Visible = chk縦横を展開する.Checked
        recalc(CalcCategory.Expand, sender)
    End Sub

    Private Sub nud基本のひも幅_ValueChanged(sender As Object, e As EventArgs) Handles nud基本のひも幅.ValueChanged
        If nud基本のひも幅.Value = 0 Then
            nud基本のひも幅.Value = g_clsSelectBasics.p_i本幅
        End If
        lbl基本のひも幅length.Text = New Length(g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit

        nud長い横ひも.Value = nud基本のひも幅.Value
        nud縦ひも.Value = nud基本のひも幅.Value
        'イベントは長い横ひも・縦ひも側
        ShowDefaultTabControlPage(enumReason._Preview)
    End Sub

    Private Sub nud長い横ひも_ValueChanged(sender As Object, e As EventArgs) Handles nud長い横ひも.ValueChanged
        recalc(CalcCategory.Horizontal)
    End Sub

    Private Sub nud縦ひも_ValueChanged(sender As Object, e As EventArgs) Handles nud縦ひも.ValueChanged
        recalc(CalcCategory.Vertical, sender)
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

    Private Sub rad最上と最下の短いひも_なし_CheckedChanged(sender As Object, e As EventArgs) Handles rad最上と最下の短いひも_なし.CheckedChanged, rad最上と最下の短いひも_同じ幅.CheckedChanged, rad最上と最下の短いひも_異なる幅.CheckedChanged
        If rad最上と最下の短いひも_異なる幅.Checked Then
            nud最上と最下の短いひもの幅.Enabled = True
            If nud最上と最下の短いひもの幅.Value < 1 Then
                nud最上と最下の短いひもの幅.Value = 1
            End If
        Else
            nud最上と最下の短いひもの幅.Enabled = False
            If rad最上と最下の短いひも_同じ幅.Checked Then
                nud最上と最下の短いひもの幅.Value = nud短い横ひも.Value
            Else
                nud最上と最下の短いひもの幅.Value = 0
            End If
        End If
        recalc(CalcCategory.Horizontal, sender)
    End Sub


    Private Sub nud長い横ひもの本数_ValueChanged(sender As Object, e As EventArgs) Handles nud長い横ひもの本数.ValueChanged
        '奇数推奨
        If nud長い横ひもの本数.Value Mod 2 = 1 Then
            nud長い横ひもの本数.Increment = 2
        Else
            nud長い横ひもの本数.Increment = 1
        End If
        If nud長い横ひもの本数.Value = 0 Then
            rad最上と最下の短いひも_なし.Checked = True
            '※後から変更したとしてもゼロ扱いで計算
        End If
        recalc(CalcCategory.Horizontal, sender)
    End Sub

    Private Sub nud短い横ひも_ValueChanged(sender As Object, e As EventArgs) Handles nud短い横ひも.ValueChanged
        If nud短い横ひも.Value = 0 Then
            rad最上と最下の短いひも_なし.Checked = True
        End If
        lbl短い横ひもlength.Text = New Length(g_clsSelectBasics.p_d指定本幅(nud短い横ひも.Value)).TextWithUnit

        recalc(CalcCategory.Horizontal, sender)
    End Sub

    Private Sub nud最上と最下の短いひもの幅_ValueChanged(sender As Object, e As EventArgs) Handles nud最上と最下の短いひもの幅.ValueChanged
        If nud最上と最下の短いひもの幅.Value = 0 Then
            rad最上と最下の短いひも_なし.Checked = True
        End If
        recalc(CalcCategory.Horizontal, sender)
    End Sub

    Private Sub nud横ひも間のすき間_ValueChanged(sender As Object, e As EventArgs) Handles nud横ひも間のすき間.ValueChanged
        recalc(CalcCategory.Horizontal, sender)
    End Sub

    Private Sub nud縦ひもの本数_ValueChanged(sender As Object, e As EventArgs) Handles nud縦ひもの本数.ValueChanged
        If chk縦ひもを放射状に置く.Checked Then
            nud縦ひもの本数.Increment = 1
            If 1 < nud縦ひもの本数.Value Then
                txtすき間の点数.Text = nud縦ひもの本数.Value
            Else
                txtすき間の点数.Text = ""
            End If
        Else
            '奇数推奨
            If nud縦ひもの本数.Value Mod 2 = 1 Then
                nud縦ひもの本数.Increment = 2
            Else
                nud縦ひもの本数.Increment = 1
            End If
            If 1 < nud縦ひもの本数.Value Then
                txtすき間の点数.Text = nud縦ひもの本数.Value - 1
            Else
                txtすき間の点数.Text = ""
            End If
        End If

        recalc(CalcCategory.Vertical, sender)
    End Sub

    Private Sub nudひとつのすき間の寸法_ValueChanged(sender As Object, e As EventArgs) Handles nudひとつのすき間の寸法.ValueChanged
        If 0 < nudひとつのすき間の寸法.Value Then
            txt_ひとつのすき間の寸法_本幅分.Text = New Length(nudひとつのすき間の寸法.Value).ByLaneText
        Else
            txt_ひとつのすき間の寸法_本幅分.Text = ""
        End If

        recalc(CalcCategory.Vertical, sender)
    End Sub

    Private Sub btn横寸法に合わせる_Click(sender As Object, e As EventArgs) Handles btn横寸法に合わせる.Click
        If Not recalc(CalcCategory.GapFit, sender) Then
            'すき間を横寸法に合わせることができません。
            Dim msg As String = _clsCalcMesh.p_sメッセージ
            msg = msg + vbCrLf + My.Resources.WarningWidthPrime
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub btn展開本幅の同期_Click(sender As Object, e As EventArgs) Handles btn展開本幅の同期.Click
        recalc(CalcCategory.LaneSync, sender)
    End Sub

    Private Sub cmb基本色_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb基本色.SelectedIndexChanged
        ShowDefaultTabControlPage(enumReason._Preview)
    End Sub

    Private Sub chk補強ひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk補強ひも.CheckedChanged
        recalc(CalcCategory.Horizontal, sender)
    End Sub

    Private Sub chk斜めの補強ひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk斜めの補強ひも.CheckedChanged
        recalc(CalcCategory.Vertical, sender)
    End Sub

    Private Sub chk始末ひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk始末ひも.CheckedChanged
        recalc(CalcCategory.Vertical, sender)
    End Sub

    Private Sub chk縦ひもを放射状に置く_CheckedChanged(sender As Object, e As EventArgs) Handles chk縦ひもを放射状に置く.CheckedChanged
        If chk縦ひもを放射状に置く.Checked Then
            grp横置き.Enabled = False
            lbl放射状配置.Visible = True
            lbl径.Visible = True
            chk始末ひも.Checked = False
            chk始末ひも.Enabled = False
            chk斜めの補強ひも.Checked = False
            chk斜めの補強ひも.Enabled = False
            If 0 < nud縦ひもの本数.Value Then
                txtすき間の点数.Text = nud縦ひもの本数.Value
            End If
        Else
            grp横置き.Enabled = True
            lbl放射状配置.Visible = False
            lbl径.Visible = False
            chk始末ひも.Enabled = True
            chk斜めの補強ひも.Enabled = True
            If 1 < nud縦ひもの本数.Value Then
                txtすき間の点数.Text = nud縦ひもの本数.Value - 1
            Else
                txtすき間の点数.Text = ""
            End If
        End If

        set底の縦横展開(chk縦横を展開する.Checked)
        recalc(CalcCategory.Expand, sender)

    End Sub

#End Region

#Region "底楕円"

    Private Sub btn追加_底楕円_Click(sender As Object, e As EventArgs) Handles btn追加_底楕円.Click
        Dim table As tbl底_楕円DataTable = Nothing
        Dim number As Integer = -1
        'If Not getTableAndNumber(BindingSource底_楕円, table, number) Then
        If Not dgv底楕円.GetTableAndNumber(table, number) Then
            Exit Sub
        End If

        Dim row As tbl底_楕円Row = Nothing
        If _clsCalcMesh.add_底楕円(cmb編みかた名_底楕円.Text,
                     chk差しひも.Checked,
                     nud基本のひも幅.Value, nud周数_底楕円.Value,
                      row) Then

            'numberPositionsSelect(BindingSource底_楕円, row.f_i番号, dgv底楕円)
            dgv底楕円.NumberPositionsSelect(row.f_i番号)
            recalc(CalcCategory.Oval, row, "f_i周数")
        Else
            MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub btn上へ_底楕円_Click(sender As Object, e As EventArgs) Handles btn上へ_底楕円.Click
        Dim table As tbl底_楕円DataTable = Nothing
        Dim number As Integer = -1
        'If Not GetTableAndNumber(BindingSource底_楕円, table, number) Then
        If Not dgv底楕円.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        Dim nextup As Integer = clsDataTables.SmallerNumber(table, number)
        If nextup < 0 Then
            Exit Sub
        End If
        clsDataTables.SwapNumber(table, number, nextup)
        recalc(CalcCategory.Oval, Nothing, Nothing)

        'numberPositionsSelect(BindingSource底_楕円, nextup, dgv底楕円)
        dgv底楕円.NumberPositionsSelect(nextup)
    End Sub

    Private Sub btn下へ_底楕円_Click(sender As Object, e As EventArgs) Handles btn下へ_底楕円.Click
        Dim table As tbl底_楕円DataTable = Nothing
        Dim number As Integer = -1
        'If Not GetTableAndNumber(BindingSource底_楕円, table, number) Then
        If Not dgv底楕円.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        Dim nextdown As Integer
        nextdown = clsDataTables.LargerNumber(table, number)
        If nextdown < 0 Then
            Exit Sub
        End If
        clsDataTables.SwapNumber(table, number, nextdown)
        recalc(CalcCategory.Oval, Nothing, Nothing)

        'numberPositionsSelect(BindingSource底_楕円, nextdown, dgv底楕円)
        dgv底楕円.NumberPositionsSelect(nextdown)
    End Sub

    Private Sub btn削除_底楕円_Click(sender As Object, e As EventArgs) Handles btn削除_底楕円.Click
        Dim table As tbl底_楕円DataTable = Nothing
        Dim number As Integer = -1
        'If Not GetTableAndNumber(BindingSource底_楕円, table, number) Then
        If Not dgv底楕円.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        clsDataTables.RemoveNumberFromTable(table, number)
        clsDataTables.FillNumber(table) '#16
        recalc(CalcCategory.Oval, Nothing, Nothing)
    End Sub

    Private Sub chk差しひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk差しひも.CheckedChanged
        If chk差しひも.Checked Then
            cmb編みかた名_底楕円.Text = ""
            nud周数_底楕円.Value = 8
        End If
    End Sub

    Private Sub cmb編みかた名_底楕円_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb編みかた名_底楕円.SelectedIndexChanged
        If Not String.IsNullOrWhiteSpace(cmb編みかた名_底楕円.Text) Then
            If chk差しひも.Checked Then
                chk差しひも.Checked = False
                nud周数_底楕円.Value = 1
            End If
        End If
    End Sub

    Private Sub dgv底楕円_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv底楕円.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource底_楕円.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv底楕円_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName底楕円(DataPropertyName) Then
            recalc(CalcCategory.Oval, current.Row, DataPropertyName)
        End If
    End Sub

    Private Sub chk楕円底個別設定_CheckedChanged(sender As Object, e As EventArgs) Handles chk楕円底個別設定.CheckedChanged
        nud楕円底円弧の半径加算.Enabled = chk楕円底個別設定.Checked
        nud楕円底周の加算.Enabled = chk楕円底個別設定.Checked
        recalc(CalcCategory.Oval, Nothing, Nothing)
    End Sub

    Private Sub nud楕円底円弧の半径加算_ValueChanged(sender As Object, e As EventArgs) Handles nud楕円底円弧の半径加算.ValueChanged
        recalc(CalcCategory.Oval, Nothing, Nothing)
    End Sub

    Private Sub nud楕円底周の加算_ValueChanged(sender As Object, e As EventArgs) Handles nud楕円底周の加算.ValueChanged
        recalc(CalcCategory.Oval, Nothing, Nothing)
    End Sub
#End Region

#Region "側面"
    Private Sub btn追加_側面_Click(sender As Object, e As EventArgs) Handles btn追加_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        'If Not getTableAndNumber(BindingSource側面, table, number) Then
        If Not dgv側面.GetTableAndNumber(table, number) Then
            Exit Sub
        End If

        Dim row As tbl側面Row = Nothing
        If _clsCalcMesh.add_側面(cmb編みかた名_側面.Text, chk縁の始末.Checked,
                     nud基本のひも幅.Value, nud周数_側面.Value,
                      row) Then

            'numberPositionsSelect(BindingSource側面, row.f_i番号, dgv側面)
            dgv側面.NumberPositionsSelect(row.f_i番号)
            recalc(CalcCategory.Side, row, "f_i周数")
        Else
            MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub btn削除_側面_Click(sender As Object, e As EventArgs) Handles btn削除_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        'If Not GetTableAndNumber(BindingSource側面, table, number) Then
        If Not dgv側面.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        clsDataTables.RemoveNumberFromTable(table, number)
        clsDataTables.FillNumber(table) '#16
        recalc(CalcCategory.Side, Nothing, Nothing)
    End Sub

    Private Sub btn上へ_側面_Click(sender As Object, e As EventArgs) Handles btn上へ_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        'If Not GetTableAndNumber(BindingSource側面, table, number) Then
        If Not dgv側面.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 OrElse number = clsDataTables.cHemNumber Then
            Exit Sub
        End If

        Dim nextup As Integer
        If rad下から上へ.Checked Then
            nextup = clsDataTables.LargerNumber(table, number)
        Else
            nextup = clsDataTables.SmallerNumber(table, number)
        End If
        If nextup < 0 Then
            Exit Sub
        End If
        clsDataTables.SwapNumber(table, number, nextup)

        'numberPositionsSelect(BindingSource側面, nextup, dgv側面)
        dgv側面.NumberPositionsSelect(nextup)
    End Sub

    Private Sub btn下へ_側面_Click(sender As Object, e As EventArgs) Handles btn下へ_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        'If Not GetTableAndNumber(BindingSource側面, table, number) Then
        If Not dgv側面.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 OrElse number = clsDataTables.cHemNumber Then
            Exit Sub
        End If

        Dim nextdown As Integer
        If rad下から上へ.Checked Then
            nextdown = clsDataTables.SmallerNumber(table, number)
        Else
            nextdown = clsDataTables.LargerNumber(table, number)
        End If
        If nextdown < 0 Then
            Exit Sub
        End If
        clsDataTables.SwapNumber(table, number, nextdown)

        'numberPositionsSelect(BindingSource側面, nextdown, dgv側面)
        dgv側面.NumberPositionsSelect(nextdown)
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

    Private Sub chk縁の始末_CheckedChanged(sender As Object, e As EventArgs) Handles chk縁の始末.CheckedChanged
        setPattern()
        cmb編みかた名_側面.Text = ""
        If chk縁の始末.Checked Then
            nud周数_側面.Value = 1
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
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv側面_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName側面(DataPropertyName) Then
            recalc(CalcCategory.Side, current.Row, DataPropertyName)
        End If
    End Sub

#End Region

#Region "追加品"

    '追加品の参照名 #63
    Sub setAddPartsRefNames()
        Dim names(12) As String

        '横・縦・高さ・周
        names(1) = lbl計算寸法最大横.Text '& lbl最大.Text
        names(2) = lbl計算寸法最大縦.Text '& lbl最大.Text
        names(3) = lbl計算寸法高さ.Text
        names(4) = lbl計算寸法最大周.Text

        '内側・外側
        For i = 1 To 4
            names(i + 4) = names(i) & "/" & lbl外側.Text
            names(i) = names(i) & "/" & lbl内側.Text
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
        editAddParts.ShowGrid(works, _clsCalcMesh.getAddPartsRefValues)
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
        Else
            If TabControl.TabPages.Contains(tpage縦ひも) Then
                TabControl.TabPages.Remove(tpage縦ひも)
            End If
        End If
        If isExband AndAlso Not chk縦ひもを放射状に置く.Checked Then
            If Not TabControl.TabPages.Contains(tpage横ひも) Then
                TabControl.TabPages.Add(tpage横ひも)
            End If
        Else
            If TabControl.TabPages.Contains(tpage横ひも) Then
                TabControl.TabPages.Remove(tpage横ひも)
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
        Save底_縦横(works.p_row底_縦横)
        expand横ひも.PanelSize = tpage横ひも.Size
        expand横ひも.ShowGrid(_clsCalcMesh.get横展開DataTable())
    End Sub

    Function Hide横ひも(ByVal works As clsDataTables) As Boolean
        Return expand横ひも.HideGrid(enumひも種.i_横, works)
    End Function

    Sub Show縦ひも(ByVal works As clsDataTables)
        'タブ切り替えタイミングのため、表示は更新済
        Save底_縦横(works.p_row底_縦横)
        expand縦ひも.PanelSize = tpage縦ひも.Size
        expand縦ひも.ShowGrid(_clsCalcMesh.get縦展開DataTable())
    End Sub

    Function Hide縦ひも(ByVal works As clsDataTables) As Boolean
        Return expand縦ひも.HideGrid(enumひも種.i_縦 Or enumひも種.i_斜め, works)
    End Function


    Private Sub expand横ひも_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.AddButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow Is Nothing Then
            Exit Sub
        End If
        Dim iひも番号 As Integer = currow.f_iひも番号
        Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
        '最上と最下は増やせない
        If iひも種.HasFlag(enumひも種.i_最上と最下) Then
            Exit Sub
        End If
        '補強ひもは後ろに追加
        If iひも種.HasFlag(enumひも種.i_補強) Then
            iひも番号 = nud長い横ひもの本数.Value + 1
            iひも種 = enumひも種.i_横 Or enumひも種.i_短い
        End If
        'その位置に追加
        currow = clsDataTables.Insert縦横展開Row(expand横ひも.DataSource, iひも種, iひも番号)
        _clsDataTables.FromTmpTable(enumひも種.i_横, expand横ひも.DataSource)

        nud長い横ひもの本数.Value = nud長い横ひもの本数.Value + 1 'with recalc

        'expand横ひも.DataSource = _clsCalcMesh.set横展開DataTable(True)
        expand横ひも.PositionSelect(currow)
    End Sub

    Private Sub expand横ひも_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.DeleteButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow Is Nothing Then
            Exit Sub
        End If
        Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
        '補強ひも・最上と最下の短いひも
        If iひも種.HasFlag(enumひも種.i_補強) OrElse iひも種.HasFlag(enumひも種.i_最上と最下) Then
            _clsDataTables.FromTmpTable(enumひも種.i_横, expand横ひも.DataSource)
            If iひも種.HasFlag(enumひも種.i_補強) Then
                chk補強ひも.Checked = False 'with recalc
            End If
            If iひも種.HasFlag(enumひも種.i_最上と最下) Then
                rad最上と最下の短いひも_なし.Checked = True 'with recalc
            End If
            'Save底_縦横(_clsDataTables.p_row底_縦横)
            'expand横ひも.DataSource = _clsCalcMesh.set横展開DataTable(True)

            Exit Sub
        End If

        '横ひも(長い,短い)を削除
        currow = clsDataTables.Remove縦横展開Row(expand横ひも.DataSource, iひも種, currow.f_iひも番号)
        _clsDataTables.FromTmpTable(enumひも種.i_横, expand横ひも.DataSource)
        nud長い横ひもの本数.Value = nud長い横ひもの本数.Value - 1 'with recalc
        '
        'expand横ひも.DataSource = _clsCalcMesh.set横展開DataTable(True)
        expand横ひも.PositionSelect(currow)
    End Sub

    Private Sub expand横ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.ResetButton
        expand横ひも.DataSource = _clsCalcMesh.get横展開DataTable(True)
    End Sub

    Private Sub expand縦ひも_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.AddButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow Is Nothing Then
            Exit Sub
        End If
        Dim iひも番号 As Integer = currow.f_iひも番号
        Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
        '縦ひも以外は後ろに追加
        If iひも種.HasFlag(enumひも種.i_補強) Then
            iひも番号 = nud縦ひもの本数.Value + 1
            iひも種 = enumひも種.i_縦
        End If
        'その位置に追加
        currow = clsDataTables.Insert縦横展開Row(expand縦ひも.DataSource, iひも種, iひも番号)
        _clsDataTables.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, expand縦ひも.DataSource)

        nud縦ひもの本数.Value = nud縦ひもの本数.Value + 1 'with recalc

        'expand縦ひも.DataSource = _clsCalcMesh.set縦展開DataTable(True)
        expand縦ひも.PositionSelect(currow)
    End Sub

    Private Sub expand縦ひも_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.DeleteButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow Is Nothing Then
            Exit Sub
        End If
        Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
        '始末ひも・斜めの補強ひも
        If iひも種 = (enumひも種.i_縦 Or enumひも種.i_補強) OrElse iひも種 = (enumひも種.i_斜め Or enumひも種.i_補強) Then
            _clsDataTables.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, expand縦ひも.DataSource)
            If iひも種 = (enumひも種.i_縦 Or enumひも種.i_補強) Then
                chk始末ひも.Checked = False 'with recalc
            End If
            If iひも種 = (enumひも種.i_斜め Or enumひも種.i_補強) Then
                chk斜めの補強ひも.Checked = False 'with recalc
            End If
            'Save底_縦横(_clsDataTables.p_row底_縦横)
            'expand縦ひも.DataSource = _clsCalcMesh.set縦展開DataTable(True)
            Exit Sub
        End If

        '縦ひもを削除
        currow = clsDataTables.Remove縦横展開Row(expand縦ひも.DataSource, iひも種, currow.f_iひも番号)
        _clsDataTables.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, expand縦ひも.DataSource)
        nud縦ひもの本数.Value = nud縦ひもの本数.Value - 1 'with recalc
        '
        'expand縦ひも.DataSource = _clsCalcMesh.set縦展開DataTable(True)
        expand縦ひも.PositionSelect(currow)
    End Sub

    Private Sub expand縦ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.ResetButton
        expand縦ひも.DataSource = _clsCalcMesh.get縦展開DataTable(True)
    End Sub

    Private Sub expand横ひも_CellValueChanged(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.CellValueChanged
        '"f_i何本幅", "f_dひも長加算", "f_dひも長加算2", "f_s色"
        If e.Row Is Nothing OrElse String.IsNullOrEmpty(e.DataPropertyName) Then
            Exit Sub
        End If
        recalc(CalcCategory.Expand_Yoko, e.Row, e.DataPropertyName)
    End Sub

    Private Sub expand縦ひも_CellValueChanged(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.CellValueChanged
        '"f_i何本幅", "f_dひも長加算", "f_dひも長加算2", "f_s色"
        If e.Row Is Nothing OrElse String.IsNullOrEmpty(e.DataPropertyName) Then
            Exit Sub
        End If
        recalc(CalcCategory.Expand_Tate, e.Row, e.DataPropertyName)
    End Sub

#End Region

#Region "プレビュー"
    Dim _clsImageData As clsImageData
    Private Sub Showプレビュー(works As clsDataTables)
        picプレビュー.Image = Nothing
        _clsImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcMesh.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcMesh) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If

        CalcImageData()
    End Sub

    Private Sub CalcImageData()
        If ToolStripStatusLabel1.Text = "OK" Then

            Dim isUpRightOnly As Boolean = rad右上.Checked
            Dim isShowSide As Boolean = chk側面.Checked

            Cursor.Current = Cursors.WaitCursor
            _clsImageData = New clsImageData(_sFilePath)
            Dim ret As Boolean = _clsCalcMesh.CalcImage(_clsImageData, isUpRightOnly, isShowSide)
            Cursor.Current = Cursors.Default

            If Not ret AndAlso Not String.IsNullOrWhiteSpace(_clsCalcMesh.p_sメッセージ) Then
                MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Exit Sub
            End If
            picプレビュー.Image = System.Drawing.Image.FromFile(_clsImageData.GifFilePath)
        End If
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
        If Not _clsImageData.ImgBrowserOpen(clsImageData.cBrowserBasicInfo) Then
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

    Private Sub rad右上全体_CheckedChanged(sender As Object, e As EventArgs) Handles rad右上.CheckedChanged, rad全体.CheckedChanged
        If _clsImageData IsNot Nothing Then
            CalcImageData()
        End If
    End Sub

    Private Sub chk側面_CheckedChanged(sender As Object, e As EventArgs) Handles chk側面.CheckedChanged
        If _clsImageData IsNot Nothing Then
            CalcImageData()
        End If
    End Sub

#End Region


#Region "DEBUG"
    Dim bVisible As Boolean = False
    Private Sub btnDEBUG_Click(sender As Object, e As EventArgs) Handles btnDEBUG.Click
        If Not bVisible Then
            setDgvColumnsVisible(dgv底楕円)
            setDgvColumnsVisible(dgv側面)
            editAddParts.SetDgvColumnsVisible()
            expand横ひも.SetDgvColumnsVisible()
            expand縦ひも.SetDgvColumnsVisible()
            bVisible = True
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", g_clsSelectBasics.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row目標寸法.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row底_縦横.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsCalcMesh.dump())
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(_clsDataTables.p_tbl底_楕円).ToString())
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
