
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Page
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Spin
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsUpDown
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBandSquare.clsCalcSquare

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
    Dim _Profile_追加品 As New CDataGridViewProfile(
            (New tbl追加品DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow Or enumAction._RowHeight_iひも番号
            )
    Dim _Profile_dgv縦横ひも As New CDataGridViewProfile(
            (New tbl縦横展開DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )

    Dim _Profile_dgv差しひも As New CDataGridViewProfile(
            (New tbl差しひもDataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )

    Dim _Profile_dgvひも上下 As New CDataGridViewProfile(
            (New dstWork.tblCheckBoxDataTable),
            Nothing,
            enumAction._None
            )

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _Profile_dgv側面.FormCaption = Me.Text
        dgv側面.SetProfile(_Profile_dgv側面)

        _Profile_追加品.FormCaption = Me.Text
        dgv追加品.SetProfile(_Profile_追加品)

        _Profile_dgv縦横ひも.FormCaption = Me.Text
        dgv横ひも.SetProfile(_Profile_dgv縦横ひも)
        dgv縦ひも.SetProfile(_Profile_dgv縦横ひも)

        _Profile_dgv差しひも.FormCaption = Me.Text
        dgv差しひも.SetProfile(_Profile_dgv差しひも)

        _Profile_dgvひも上下.FormCaption = Me.Text
        dgvひも上下.SetProfile(_Profile_dgvひも上下)

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
        f_i何本幅1.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅1.DisplayMember = "Display"
        f_i何本幅1.ValueMember = "Value"
        f_i何本幅2.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅2.DisplayMember = "Display"
        f_i何本幅2.ValueMember = "Value"
        f_i何本幅3.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅3.DisplayMember = "Display"
        f_i何本幅3.ValueMember = "Value"
        '
        f_i何本幅4.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅4.DisplayMember = "Display"
        f_i何本幅4.ValueMember = "Value"
        f_i何本幅5.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅5.DisplayMember = "Display"
        f_i何本幅5.ValueMember = "Value"

        f_s色1.DataSource = g_clsSelectBasics.p_tblColor
        f_s色1.DisplayMember = "Display"
        f_s色1.ValueMember = "Value"
        f_s色2.DataSource = g_clsSelectBasics.p_tblColor
        f_s色2.DisplayMember = "Display"
        f_s色2.ValueMember = "Value"
        f_s色3.DataSource = g_clsSelectBasics.p_tblColor
        f_s色3.DisplayMember = "Display"
        f_s色3.ValueMember = "Value"
        '
        f_s色4.DataSource = g_clsSelectBasics.p_tblColor
        f_s色4.DisplayMember = "Display"
        f_s色4.ValueMember = "Value"
        f_s色5.DataSource = g_clsSelectBasics.p_tblColor
        f_s色5.DisplayMember = "Display"
        f_s色5.ValueMember = "Value"

        cmb基本色.DataSource = g_clsSelectBasics.p_tblColor
        cmb基本色.DisplayMember = "Display"
        cmb基本色.ValueMember = "Value"

        setBasics()
        setPattern()
        setOptions()
        init差しひも()
        _isLoadingData = False 'Designer.vb描画完了

        DispTables(_clsDataTables)

        'サイズ復元
        Dim siz As Size = My.Settings.frmMainSize
        If 0 < siz.Height AndAlso 0 < siz.Width Then
            Me.Size = siz
            Dim colwid As String
            colwid = My.Settings.frmMainGridSide
            Me.dgv側面.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridInsertBand
            Me.dgv差しひも.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridOptions
            Me.dgv追加品.SetColumnWidthFromString(colwid)
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

        My.Settings.frmMainGridSide = Me.dgv側面.GetColumnWidthString()
        My.Settings.frmMainGridInsertBand = Me.dgv差しひも.GetColumnWidthString()
        My.Settings.frmMainGridOptions = Me.dgv追加品.GetColumnWidthString()
        My.Settings.frmMainGridYoko = Me.dgv横ひも.GetColumnWidthString()
        My.Settings.frmMainGridTate = Me.dgv縦ひも.GetColumnWidthString()
        My.Settings.frmMainSize = Me.Size
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv側面={0}", My.Settings.frmMainGridSide)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv差しひも={0}", My.Settings.frmMainGridInsertBand)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv追加品={0}", My.Settings.frmMainGridOptions)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv底の横={0}", My.Settings.frmMainGridYoko)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv底の縦={0}", My.Settings.frmMainGridTate)
    End Sub


    '対象バンド・基本値の更新
    Sub setBasics()

        With g_clsSelectBasics
            txtバンドの種類名.Text = .p_s対象バンドの種類名

            Dim unitstr As String = .p_unit設定時の寸法単位.Str
            lbl目標寸法_単位.Text = unitstr
            lbl目_ひも間のすき間_単位.Text = unitstr
            lbl長さ_単位.Text = unitstr
            lbl計算寸法_単位.Text = unitstr

            lbl目_ひも間のすき間_単位.Text = unitstr
            lblひも長加算_縦横端_単位.Text = unitstr
            lblひも長加算_側面_単位.Text = unitstr


            nud横寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud縦寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud高さ寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud長さ.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces

            nud目_ひも間のすき間.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
            nudひも長加算_縦横端.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nudひも長加算_側面.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces

            Dim maxLane As Integer = .p_i本幅
            nud基本のひも幅.Maximum = maxLane '表示中のValueは追随

            '色リスト
            cmb基本色.Refresh()

            lbl基本のひも幅length.Text = New Length(.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit

            'Grid
            Dim format As String = String.Format("N{0}", .p_unit設定時の寸法単位.DecimalPlaces)

            Me.f_dひも長1.DefaultCellStyle.Format = format
            Me.f_d出力ひも長1.DefaultCellStyle.Format = format

            Me.f_d高さ2.DefaultCellStyle.Format = format
            Me.f_d垂直ひも長2.DefaultCellStyle.Format = format
            Me.f_d周長2.DefaultCellStyle.Format = format
            Me.f_dひも長2.DefaultCellStyle.Format = format
            Me.f_d連続ひも長2.DefaultCellStyle.Format = format

            Me.f_dひも長3.DefaultCellStyle.Format = format

            Me.f_d長さ4.DefaultCellStyle.Format = format
            Me.f_dひも長4.DefaultCellStyle.Format = format
            Me.f_d出力ひも長4.DefaultCellStyle.Format = format
            Me.f_d幅4.DefaultCellStyle.Format = format

            Me.f_d長さ5.DefaultCellStyle.Format = format
            Me.f_dひも長5.DefaultCellStyle.Format = format
            Me.f_d出力ひも長5.DefaultCellStyle.Format = format
            Me.f_d幅5.DefaultCellStyle.Format = format

        End With

        '現データ
        _clsDataTables.ModifySelected()

    End Sub

    '編みかたの変更
    Sub setPattern()
        cmb編みかた名_側面.Items.Clear()
        cmb編みかた名_側面.Items.AddRange(g_clsMasterTables.GetPatternNames(True, False))
    End Sub

    '付属品の変更
    Sub setOptions()
        cmb付属品名.Items.Clear()
        cmb付属品名.Items.AddRange(g_clsMasterTables.GetOptionNames())
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
            Case "tpage四角数"
                '
            Case "tpage側面と縁"
                Show側面(works)
            Case "tpage差しひも"
                Show差しひも(works)
            Case "tpageひも上下"
                Showひも上下(works)
            Case "tpage追加品"
                Show追加品(works)
            Case "tpageメモ他"
                '
            Case "tpage横ひも"
                Show横ひも(works)
            Case "tpage縦ひも"
                Show縦ひも(works)
            Case "tpageプレビュー"
                Showプレビュー(works)
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
    End Enum
    Private Sub ShowDefaultTabControlPage(ByVal reason As enumReason)
        Dim needreset As Boolean = False
        If reason.HasFlag(enumReason._GridDropdown) Then
            If {"tpage側面と縁", "tpage追加品", "tpage横ひも", "tpage縦ひも"}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If reason.HasFlag(enumReason._Preview) Then
            If {"tpageプレビュー"}.Contains(_CurrentTabControlName) Then
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
            Case "tpage側面と縁"
                Hide側面(_clsDataTables)
            Case "tpage差しひも"
                Hide差しひも(_clsDataTables)
            Case "tpageひも上下"
                Hideひも上下(_clsDataTables)
            Case "tpage追加品"
                Hide追加品(_clsDataTables)
            Case "tpageメモ他"
                '
            Case "tpage横ひも"
                Hide横ひも(_clsDataTables)
            Case "tpage縦ひも"
                Hide縦ひも(_clsDataTables)
            Case "tpageプレビュー"
                Hideプレビュー(_clsDataTables)
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
                g_clsSelectBasics.SetTargetBandTypeName(.Value("f_sバンドの種類名"))
                setBasics()
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

            'nud横ひもの本数.Value = .Value("f_i長い横ひもの本数")
            'nud縦ひもの本数.Value = .Value("f_i縦ひもの本数")
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
        works.CheckPoint(works.p_tbl差しひも)

        If _CurrentTabControlName = "tpage底の横" Then
            works.FromTmpTable(enumひも種.i_横, BindingSource横ひも.DataSource)
        End If
        If _CurrentTabControlName = "tpage底の縦" Then
            works.FromTmpTable(enumひも種.i_縦, BindingSource縦ひも.DataSource)
        End If
        If _CurrentTabControlName = "tpageひも上下" Then
            saveひも上下(False)
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
        End With
        Return True
    End Function

    Function Save四角数(ByVal row底_縦横 As clsDataRow) As Boolean
        With row底_縦横
            .Value("f_b展開区分") = chk縦横側面を展開する.Checked

            .Value("f_i横の四角数") = nud横の目の数.Value
            .Value("f_i縦の四角数") = nud縦の目の数.Value
            .Value("f_d高さの四角数") = nud高さの目の数.Value 'double

            '読み取りますが使いません
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
            setBasics()
            recalc(CalcCategory.BsMaster)
        End If
    End Sub

    'リセット
    Private Sub ToolStripMenuItemEditReset_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditReset.Click
        Dim r As DialogResult
        If _clsCalcSquare.IsValidInput() Then '#22
            '目標寸法以外をリセットします。目(ひも間のすき間)もリセットしてよろしいですか？
            '(はいで全てリセット、いいえで目(ひも間のすき間)を保持)
            r = MessageBox.Show(My.Resources.AskResetInput, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
            If r <> DialogResult.Yes AndAlso r <> DialogResult.No Then
                Exit Sub
            End If
        End If
        Dim d目 As Double = nud目_ひも間のすき間.Value

        _clsDataTables.Clear()
        _clsDataTables.SetInitialValue()
        Save目標寸法(_clsDataTables.p_row目標寸法) '画面値
        _clsDataTables.p_row底_縦横.Value("f_i長い横ひも") = nud基本のひも幅.Value
        _clsDataTables.p_row底_縦横.Value("f_i短い横ひも") = nud基本のひも幅.Value
        _clsDataTables.p_row底_縦横.Value("f_i縦ひも") = nud基本のひも幅.Value
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
            setBasics()
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
            setOptions()
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
        If dlg.ShowDialog() = DialogResult.OK Then
            SaveTables(_clsDataTables)
            setBasics()
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

    Private Sub setStartEditing()
        '表示後の編集開始
        Save目標寸法(_clsDataTables.p_row目標寸法)
        Save四角数(_clsDataTables.p_row底_縦横)

        _clsDataTables.ResetStartPoint()

        Me.Text = String.Format(My.Resources.FormCaption, IO.Path.GetFileNameWithoutExtension(_sFilePath))
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
            setStartEditing()
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
                setStartEditing()
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
        'recalc(CalcCategory.Square_Tate, sender)縦の目の数
    End Sub

    Private Sub nud縦の目の数_ValueChanged(sender As Object, e As EventArgs) Handles nud縦の目の数.ValueChanged
        '偶数推奨
        If nud縦の目の数.Value Mod 2 = 1 Then
            nud縦の目の数.Increment = 1
        Else
            nud縦の目の数.Increment = 2
        End If
        nud横ひもの本数.Value = nud縦の目の数.Value + 1
        recalc(CalcCategory.Square_Tate, sender)
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
        'recalc(CalcCategory.Square_Yoko, sender)横の目の数
    End Sub

    Private Sub nud横の目の数_ValueChanged(sender As Object, e As EventArgs) Handles nud横の目の数.ValueChanged
        '偶数推奨
        If nud横の目の数.Value Mod 2 = 1 Then
            nud横の目の数.Increment = 1
        Else
            nud横の目の数.Increment = 2
        End If
        nud縦ひもの本数.Value = nud横の目の数.Value + 1
        recalc(CalcCategory.Square_Yoko, sender)
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
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        If Not dgv側面.GetTableAndNumber(table, number) Then
            Exit Sub
        End If

        Dim row As tbl側面Row = Nothing
        If _clsCalcSquare.add_縁(cmb編みかた名_側面.Text,
                     nud基本のひも幅.Value, row) Then

            dgv側面.NumberPositionsSelect(row.f_i番号)
            recalc(CalcCategory.SideEdge, row, "f_i周数")
        Else
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub btn削除_側面_Click(sender As Object, e As EventArgs) Handles btn削除_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        If Not dgv側面.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If NumberCount(table, clsDataTables.cHemNumber) = 0 Then
            Exit Sub
        End If
        If number <> clsDataTables.cHemNumber Then
            '縁を削除してよろしいですか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskDeleteEdge, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        '縁を削除
        clsDataTables.RemoveNumberFromTable(table, clsDataTables.cHemNumber)
        recalc(CalcCategory.SideEdge, Nothing, Nothing)
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
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv側面_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName側面と縁(DataPropertyName) Then
            recalc(CalcCategory.SideEdge, current.Row, DataPropertyName)
        End If
    End Sub

#End Region

#Region "追加品"

    Sub Show追加品(ByVal works As clsDataTables)
        BindingSource追加品.Sort = Nothing
        BindingSource追加品.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        BindingSource追加品.DataSource = works.p_tbl追加品
        BindingSource追加品.Sort = "f_i番号 , f_iひも番号"

        dgv追加品.Refresh()
    End Sub

    Function Hide追加品(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource追加品.DataSource)

        BindingSource追加品.Sort = Nothing
        BindingSource追加品.DataSource = Nothing

        Return ret
    End Function

    Private Sub btn追加_追加品_Click(sender As Object, e As EventArgs) Handles btn追加_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        If Not dgv追加品.GetTableAndNumber(table, number) Then
            Exit Sub
        End If

        Dim row As tbl追加品Row = Nothing
        If _clsCalcSquare.add_追加品(
            cmb付属品名.Text, nud基本のひも幅.Value, nud長さ.Value, nud点数.Value,
            row) Then

            dgv追加品.NumberPositionsSelect(row.f_i番号)
            recalc(CalcCategory.Options, row, "f_i点数")

        Else
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btn上へ_追加品_Click(sender As Object, e As EventArgs) Handles btn上へ_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        'If Not getTableAndNumber(BindingSource追加品, table, number) Then
        If Not dgv追加品.GetTableAndNumber(table, number) Then
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

        'numberPositionsSelect(BindingSource追加品, nextup, dgv追加品)
        dgv追加品.NumberPositionsSelect(nextup)
    End Sub

    Private Sub btn下へ_追加品_Click(sender As Object, e As EventArgs) Handles btn下へ_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        'If Not GetTableAndNumber(BindingSource追加品, table, number) Then
        If Not dgv追加品.GetTableAndNumber(table, number) Then
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

        'numberPositionsSelect(BindingSource追加品, nextdown, dgv追加品)
        dgv追加品.NumberPositionsSelect(nextdown)
    End Sub

    Private Sub btn削除_追加品_Click(sender As Object, e As EventArgs) Handles btn削除_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        'If Not GetTableAndNumber(BindingSource追加品, table, number) Then
        If Not dgv追加品.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        clsDataTables.RemoveNumberFromTable(table, number)
        clsDataTables.FillNumber(table) '#16
        recalc(CalcCategory.Options, Nothing, Nothing)
    End Sub

    Private Sub dgv追加品_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv追加品.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource追加品.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv追加品_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName追加品(DataPropertyName) Then
            recalc(CalcCategory.Options, current.Row, DataPropertyName)
        End If
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


    Sub Show横ひも(ByVal works As clsDataTables)
        Save四角数(works.p_row底_縦横) '補強ひも・キャッシュなし
        BindingSource横ひも.DataSource = _clsCalcSquare.get横展開DataTable()
        BindingSource横ひも.Sort = "f_i位置番号"

        dgv横ひも.Refresh()
    End Sub

    Function Hide横ひも(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = _clsCalcSquare.save横展開DataTable(True)
        BindingSource横ひも.Sort = Nothing
        BindingSource横ひも.DataSource = Nothing

        dgv横ひも.Refresh()
        Return ret
    End Function

    Sub Show縦ひも(ByVal works As clsDataTables)
        Save四角数(works.p_row底_縦横) '補強ひも・キャッシュなし
        BindingSource縦ひも.DataSource = _clsCalcSquare.get縦展開DataTable()
        BindingSource縦ひも.Sort = "f_i位置番号"

        dgv縦ひも.Refresh()
    End Sub

    Function Hide縦ひも(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = _clsCalcSquare.save縦展開DataTable(True)
        BindingSource縦ひも.Sort = Nothing
        BindingSource縦ひも.DataSource = Nothing

        dgv縦ひも.Refresh()
        Return ret
    End Function

    Private Sub btnリセット_横_Click(sender As Object, e As EventArgs) Handles btnリセット_横.Click
        'ひも長加算と色をすべてクリアします。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetAddLengthColor, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        BindingSource横ひも.DataSource = _clsCalcSquare.get横展開DataTable(True)
        dgv横ひも.Refresh()
    End Sub

    Private Sub btnリセット_縦_Click(sender As Object, e As EventArgs) Handles btnリセット_縦.Click
        'ひも長加算と色をすべてクリアします。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetAddLengthColor, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        BindingSource縦ひも.DataSource = _clsCalcSquare.get縦展開DataTable(True)
        dgv縦ひも.Refresh()
    End Sub

    Private Sub dgv横ひも_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv横ひも.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource横ひも.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv横ひも_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName縦横展開(DataPropertyName) Then
            recalc(CalcCategory.Expand_Yoko, current.Row, DataPropertyName)
        End If
    End Sub

    Private Sub dgv縦ひも_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv縦ひも.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource縦ひも.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv縦ひも_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName縦横展開(DataPropertyName) Then
            recalc(CalcCategory.Expand_Tate, current.Row, DataPropertyName)
        End If
    End Sub

#End Region

#Region "差しひものEnum文字列"
    Dim _PlateTable As dstWork.tblEnumDataTable
    Dim _AngleTable As dstWork.tblEnumDataTable
    Dim _CenterTable As dstWork.tblEnumDataTable

    Private Sub init差しひも()
        Dim str As String

        '配置面の選択肢
        _PlateTable = New dstWork.tblEnumDataTable
        str = My.Resources.EnumStringPlate
        If Not String.IsNullOrWhiteSpace(str) Then
            Dim ary As String() = str.Split(",")
            Dim idx As Integer = 0
            For Each s As String In ary
                s = s.Trim
                Dim rc As dstWork.tblEnumRow = _PlateTable.NewRow
                rc.Display = s
                rc.Value = idx
                _PlateTable.Rows.Add(rc)
                idx += 1
            Next
        End If
        f_i配置面1.DataSource = _PlateTable
        f_i配置面1.DisplayMember = "Display"
        f_i配置面1.ValueMember = "Value"

        '角度の選択肢
        _AngleTable = New dstWork.tblEnumDataTable
        str = My.Resources.EnumStringAngle
        If Not String.IsNullOrWhiteSpace(str) Then
            Dim ary As String() = str.Split(",")
            Dim idx As Integer = 0
            For Each s As String In ary
                s = s.Trim
                Dim rc As dstWork.tblEnumRow = _AngleTable.NewRow
                rc.Display = s
                rc.Value = idx
                _AngleTable.Rows.Add(rc)
                idx += 1
            Next
        End If
        f_i角度1.DataSource = _AngleTable
        f_i角度1.DisplayMember = "Display"
        f_i角度1.ValueMember = "Value"

        '中心点
        _CenterTable = New dstWork.tblEnumDataTable
        str = My.Resources.EnumStringCenter
        If Not String.IsNullOrWhiteSpace(str) Then
            Dim ary As String() = str.Split(",")
            Dim idx As Integer = 0
            For Each s As String In ary
                s = s.Trim
                Dim rc As dstWork.tblEnumRow = _CenterTable.NewRow
                rc.Display = s
                rc.Value = idx
                _CenterTable.Rows.Add(rc)
                idx += 1
            Next
        End If
        f_i中心点1.DataSource = _CenterTable
        f_i中心点1.DisplayMember = "Display"
        f_i中心点1.ValueMember = "Value"

    End Sub

    Public Function PlateString(ByVal plate As enum配置面) As String
        Try
            Return CType(_PlateTable.Rows(CType(plate, Integer)), dstWork.tblEnumRow).Display
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function AngleString(ByVal angle As enum角度) As String
        Try
            Return CType(_AngleTable.Rows(CType(angle, Integer)), dstWork.tblEnumRow).Display
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function CenterString(ByVal center As enum中心点) As String
        Try
            Return CType(_CenterTable.Rows(CType(center, Integer)), dstWork.tblEnumRow).Display
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#End Region

#Region "差しひも"
    Sub Show差しひも(ByVal works As clsDataTables)
        BindingSource差しひも.Sort = Nothing
        BindingSource差しひも.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If
        If Not _clsCalcSquare.CalcSize(CalcCategory.Inserted, Nothing, Nothing) Then
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        BindingSource差しひも.DataSource = works.p_tbl差しひも
        BindingSource差しひも.Sort = "f_i番号"

        dgv差しひも.Refresh()
    End Sub

    Function Hide差しひも(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource差しひも.DataSource)

        BindingSource差しひも.Sort = Nothing
        BindingSource差しひも.DataSource = Nothing

        Return ret
    End Function

    Private Sub btn追加_差しひも_Click(sender As Object, e As EventArgs) Handles btn追加_差しひも.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
            Exit Sub
        End If

        Dim row As tbl差しひもRow = Nothing
        If _clsCalcSquare.add_差しひも(row) Then
            dgv差しひも.NumberPositionsSelect(row.f_i番号)
            'recalc(CalcCategory.Inserted, row, "f_i点数")
        Else
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btn上へ_差しひも_Click(sender As Object, e As EventArgs) Handles btn上へ_差しひも.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
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

        dgv差しひも.NumberPositionsSelect(nextup)
    End Sub

    Private Sub btn下へ_差しひも_Click(sender As Object, e As EventArgs) Handles btn下へ_差しひも.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        Dim nextdown As Integer = clsDataTables.LargerNumber(table, number)
        If nextdown < 0 Then
            Exit Sub
        End If
        clsDataTables.SwapNumber(table, number, nextdown)

        dgv差しひも.NumberPositionsSelect(nextdown)
    End Sub

    Private Sub btn削除_差しひも_Click(sender As Object, e As EventArgs) Handles btn削除_差しひも.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        clsDataTables.RemoveNumberFromTable(table, number)
        clsDataTables.FillNumber(table) '#16
        'recalc(CalcCategory.Inserted, Nothing, Nothing)
    End Sub

    Private Sub dgv差しひも_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv差しひも.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource差しひも.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv差しひも_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName差しひも(DataPropertyName) Then
            recalc(CalcCategory.Inserted, current.Row, DataPropertyName)
        End If
    End Sub

#End Region

#Region "ひも上下"
    Dim _CurrentTargetFace As enumTargetFace
    Dim _UpdownChanged As Boolean

    Private Function setUpdownColumns(ByVal col As Integer) As Boolean
        If col < 0 OrElse clsUpDown.cMaxUpdownColumns < col Then
            Return False
        End If
        '0はIndex, 1～
        For i As Integer = 1 To col
            dgvひも上下.Columns(i).Visible = True
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
                setUpdownColumns(updown.HorizontalCount)
                setUpDownCountLeft()
                BindingSourceひも上下.DataSource = updown.CheckBoxTable
                If rad下から上へ.Checked AndAlso IsSide(_CurrentTargetFace) Then
                    BindingSourceひも上下.Sort = "Index desc"
                    txt開始位置.Text = My.Resources.LeftLower
                Else
                    BindingSourceひも上下.Sort = "Index"
                    txt開始位置.Text = My.Resources.LeftUpper
                End If
            End If
            btnサイズ変更_ひも上下.Enabled = False
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmMain.setDataSourceUpDown")
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

        Dim left_h As Integer
        Dim left_v As Integer

        If IsSide(_CurrentTargetFace) Then
            left_h = _clsCalcSquare.p_i垂直ひも半数 Mod horizontalCount
            left_v = _clsCalcSquare.p_i側面ひもの本数 Mod verticalCount
        Else
            left_h = _clsCalcSquare.p_i横ひもの本数 Mod horizontalCount
            left_v = _clsCalcSquare.p_i縦ひもの本数 Mod verticalCount
        End If

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


    Sub Showひも上下(ByVal works As clsDataTables)
        BindingSourceひも上下.Sort = Nothing
        BindingSourceひも上下.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        _CurrentTargetFace = enumTargetFace.Bottom
        If rad側面_上右.Checked Then
            _CurrentTargetFace = enumTargetFace.Side12
        ElseIf rad側面_下左.Checked Then
            _CurrentTargetFace = enumTargetFace.Side34
        End If

        Dim updown As clsUpDown = _clsCalcSquare.loadひも上下(_CurrentTargetFace)
        If updown Is Nothing Then
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        txt上下のメモ.Text = updown.Memo
        setDataSourceUpDown(updown)

        dgvひも上下.Refresh()
        _UpdownChanged = False
    End Sub

    Function saveひも上下(ByVal isMsg As Boolean) As Boolean
        If _UpdownChanged Then
            If BindingSourceひも上下.DataSource IsNot Nothing Then
                Dim updown As New clsUpDown(_CurrentTargetFace, nud水平に.Value, nud垂直に.Value)
                updown.CheckBoxTable = BindingSourceひも上下.DataSource
                updown.Memo = txt上下のメモ.Text

                If Not _clsCalcSquare.saveひも上下(updown) Then
                    If isMsg Then
                        MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                    Return False
                End If
            Else
                Return False
            End If
        End If
        Return True
    End Function

    Function Hideひも上下(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = saveひも上下(True)
        BindingSourceひも上下.Sort = Nothing
        BindingSourceひも上下.DataSource = Nothing
        Return ret
    End Function

    Private Sub btnサイズ変更_ひも上下_Click(sender As Object, e As EventArgs) Handles btnサイズ変更_ひも上下.Click
        If nud水平に.Value <= 0 OrElse nud垂直に.Value <= 0 Then
            '水平・垂直の本数を指定してください。
            MessageBox.Show(My.Resources.MessageNoUpDownSize, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _clsCalcSquare.updownサイズ変更(nud水平に.Value, nud垂直に.Value, False)
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btnサイズ変更_ひも上下.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btnリセット_ひも上下_Click(sender As Object, e As EventArgs) Handles btnリセット_ひも上下.Click
        If BindingSourceひも上下.DataSource IsNot Nothing Then
            'ひも上下の編集内容をすべてクリアします。よろしいですか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetUpDown, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        setDataSourceUpDown(Nothing)
        txt上下のメモ.Text = ""

        Dim updown As clsUpDown = _clsCalcSquare.updownリセット()
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btnリセット_ひも上下.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btn上下交換_Click(sender As Object, e As EventArgs) Handles btn上下交換.Click
        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _clsCalcSquare.updown上下交換()
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btn上下交換.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btn左右反転_Click(sender As Object, e As EventArgs) Handles btn左右反転.Click
        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _clsCalcSquare.updown左右反転()
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btn左右反転.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btn天地反転_Click(sender As Object, e As EventArgs) Handles btn天地反転.Click
        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _clsCalcSquare.updown天地反転()
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btn天地反転.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btn右回転_Click(sender As Object, e As EventArgs) Handles btn右回転.Click
        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _clsCalcSquare.updown右回転()
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btn右回転.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btnシフト_Click(sender As Object, e As EventArgs) Handles btnシフト.Click
        Dim updown As clsUpDown
        If rad上.Checked Then
            setDataSourceUpDown(Nothing)
            If rad下から上へ.Checked AndAlso IsSide(_CurrentTargetFace) Then
                updown = _clsCalcSquare.updown垂直シフト(False) '逆順なので後
            Else
                updown = _clsCalcSquare.updown垂直シフト(True) '正順なので先頭
            End If

        ElseIf rad下.Checked Then
            setDataSourceUpDown(Nothing)
            If rad下から上へ.Checked AndAlso IsSide(_CurrentTargetFace) Then
                updown = _clsCalcSquare.updown垂直シフト(True) '逆順なので先頭
            Else
                updown = _clsCalcSquare.updown垂直シフト(False) '正順なので後
            End If

        ElseIf rad右.Checked Then
            setDataSourceUpDown(Nothing)
            updown = _clsCalcSquare.updown水平シフト(False)

        ElseIf rad左.Checked Then
            setDataSourceUpDown(Nothing)
            updown = _clsCalcSquare.updown水平シフト(True)

        Else
            Exit Sub
        End If

        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btnシフト.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
                MessageBox.Show(My.Resources.MessageNoMore, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

        ElseIf rad右.Checked OrElse rad左.Checked Then
            If dgvひも上下.Columns(clsUpDown.cMaxUpdownColumns).Visible Then
                'これ以上増やせません。
                MessageBox.Show(My.Resources.MessageNoMore, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
        Else
            Exit Sub
        End If

        setDataSourceUpDown(Nothing)
        Dim updown As clsUpDown = Nothing

        If rad上.Checked Then
            If rad下から上へ.Checked AndAlso IsSide(_CurrentTargetFace) Then
                updown = _clsCalcSquare.updown垂直追加(False) '逆順なので後
            Else
                updown = _clsCalcSquare.updown垂直追加(True) '正順なので先頭
            End If

        ElseIf rad下.Checked Then
            If rad下から上へ.Checked AndAlso IsSide(_CurrentTargetFace) Then
                updown = _clsCalcSquare.updown垂直追加(True) '逆順なので先頭
            Else
                updown = _clsCalcSquare.updown垂直追加(False) '正順なので後
            End If

        ElseIf rad右.Checked Then
            updown = _clsCalcSquare.updown水平追加(False)

        ElseIf rad左.Checked Then
            updown = _clsCalcSquare.updown水平追加(True)
        End If

        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btn追加.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btnランダム_Click(sender As Object, e As EventArgs) Handles btnランダム.Click
        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _clsCalcSquare.updownランダム()
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btnランダム.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub btnチェック_Click(sender As Object, e As EventArgs) Handles btnチェック.Click
        If Not _clsCalcSquare.updownチェック() Then
            MessageBox.Show(_clsCalcSquare.p_sメッセージ,
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            'チェックOKです。
            MessageBox.Show(My.Resources.MessageCheckOK,
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub dgvひも上下_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvひも上下.CellValueChanged
        _UpdownChanged = True
    End Sub

    Private Sub txt上下のメモ_TextChanged(sender As Object, e As EventArgs) Handles txt上下のメモ.TextChanged
        _UpdownChanged = True
    End Sub

    Private Sub rad底側面_CheckedChanged(sender As Object, e As EventArgs) Handles rad底.CheckedChanged, rad側面_上右.CheckedChanged, rad側面_下左.CheckedChanged
        Dim target As enumTargetFace = enumTargetFace.Bottom
        If rad側面_上右.Checked Then
            target = enumTargetFace.Side12
        ElseIf rad側面_下左.Checked Then
            target = enumTargetFace.Side34
        End If

        If _CurrentTargetFace <> target Then
            saveひも上下(True)
            Showひも上下(_clsDataTables)
        End If
    End Sub

    Private Sub btn先と同じ_Click(sender As Object, e As EventArgs) Handles btn先と同じ.Click
        Dim prvS As String
        Dim prv As enumTargetFace
        Select Case _CurrentTargetFace
            Case enumTargetFace.Side12
                prv = enumTargetFace.Bottom
                prvS = String.Format(My.Resources.AskLoadSameAs, rad底.Text)
            Case enumTargetFace.Side34
                prv = enumTargetFace.Side12
                prvS = String.Format(My.Resources.AskLoadSameAs, rad側面_上右.Text)
            Case Else 'bottom
                prv = enumTargetFace.Side34
                prvS = String.Format(My.Resources.AskLoadSameAs, rad側面_下左.Text)
        End Select
        '現編集内容を破棄し{0}と同じにします。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(prvS, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If

        setDataSourceUpDown(Nothing)

        Dim updown As clsUpDown = _clsCalcSquare.loadひも上下(prv)
        updown.TargetFace = _CurrentTargetFace
        If updown Is Nothing Then
            '{0}できませんでした。リセットしてやり直してください。
            MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btn先と同じ.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            setDataSourceUpDown(updown)
        End If
        dgvひも上下.Refresh()
        _UpdownChanged = True
    End Sub

    Private Sub nud水平に_ValueChanged(sender As Object, e As EventArgs) Handles nud水平に.ValueChanged
        btnサイズ変更_ひも上下.Enabled = True
        lbl水平残.Text = "-"
    End Sub

    Private Sub nud垂直に_ValueChanged(sender As Object, e As EventArgs) Handles nud垂直に.ValueChanged
        btnサイズ変更_ひも上下.Enabled = True
        lbl垂直残.Text = "-"
    End Sub

    Private Sub dgvひも上下_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvひも上下.CellPainting
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
                                  Index.ToString(),
                e.CellStyle.Font, indexRect, e.CellStyle.ForeColor, TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
            '描画が完了したことを知らせる
            e.Handled = True
        End If
    End Sub

    Private Sub dgvひも上下_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvひも上下.CellFormatting
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If
        If dgvひも上下.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Then
            e.CellStyle.BackColor = Color.DarkGray
        Else
            e.CellStyle.BackColor = Color.White
        End If
    End Sub

    Private Sub btn設定呼出_Click(sender As Object, e As EventArgs) Handles btn設定呼出.Click
        Dim updown_tmp As New clsUpDown(_CurrentTargetFace, nud水平に.Value, nud垂直に.Value)
        updown_tmp.CheckBoxTable = BindingSourceひも上下.DataSource

        Dim dlg As New frmUpDownSetting
        dlg.CurrentUpdown = updown_tmp
        dlg.IsLoadToCurrent = True
        If dlg.ShowDialog() = DialogResult.OK Then
            setDataSourceUpDown(Nothing)
            txt上下のメモ.Text = updown_tmp.Memo
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "updown_tmp:{0}", updown_tmp.ToString)

            'CheckBoxTableに結果が入っている
            Dim updown As clsUpDown = _clsCalcSquare.updownサイズ変更(updown_tmp.HorizontalCount, updown_tmp.VerticalCount, True)
            If updown Is Nothing Then
                '{0}できませんでした。リセットしてやり直してください。
                MessageBox.Show(String.Format(My.Resources.MessageUpDownError, btn設定呼出.Text),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

#Region "プレビュー"
    Dim _clsImageData As clsImageData
    Private Sub Showプレビュー(works As clsDataTables)
        picプレビュー.Image = Nothing
        _clsImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcSquare.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcSquare) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If

        _clsImageData = New clsImageData(_sFilePath)
        ret = _clsCalcSquare.CalcImage(_clsImageData)
        If Not ret AndAlso Not String.IsNullOrWhiteSpace(_clsCalcSquare.p_sメッセージ) Then
            MessageBox.Show(_clsCalcSquare.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
            setDgvColumnsVisible(dgv側面)
            setDgvColumnsVisible(dgv追加品)
            setDgvColumnsVisible(dgv横ひも)
            setDgvColumnsVisible(dgv縦ひも)
            setDgvColumnsVisible(dgv差しひも)
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
