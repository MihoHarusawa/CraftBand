
Imports CraftBand
Imports CraftBand.clsDataTables
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
    Dim _isChangingByCode As Boolean = False


#Region "基本的な画面処理"

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgv底楕円.RowTemplate.Height = cRowHeightIdxOne
        dgv側面.RowTemplate.Height = cRowHeightIdxOne
        dgv追加品.RowTemplate.Height = cRowHeightIdxOne

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
        TabControl.TabPages.Remove(tpage底の横)
        TabControl.TabPages.Remove(tpage底の縦)

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
        _isLoadingData = False 'Designer.vb描画完了

        DispTables(_clsDataTables)

        'サイズ復元
        Dim siz As Size = My.Settings.frmMainSize
        If 0 < siz.Height AndAlso 0 < siz.Width Then
            Me.Size = siz
            Dim colwid As String
            colwid = My.Settings.frmMainGridOval
            SetColumnWidthFromString(Me.dgv底楕円, colwid)
            colwid = My.Settings.frmMainGridSide
            SetColumnWidthFromString(Me.dgv側面, colwid)
            colwid = My.Settings.frmMainGridOptions
            SetColumnWidthFromString(Me.dgv追加品, colwid)
            colwid = My.Settings.frmMainGridYoko
            SetColumnWidthFromString(Me.dgv底の横, colwid)
            colwid = My.Settings.frmMainGridTate
            SetColumnWidthFromString(Me.dgv底の縦, colwid)
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

        My.Settings.frmMainGridOval = GetColumnWidthString(Me.dgv底楕円)
        My.Settings.frmMainGridSide = GetColumnWidthString(Me.dgv側面)
        My.Settings.frmMainGridOptions = GetColumnWidthString(Me.dgv追加品)
        My.Settings.frmMainGridYoko = GetColumnWidthString(Me.dgv底の横)
        My.Settings.frmMainGridTate = GetColumnWidthString(Me.dgv底の縦)
        My.Settings.frmMainSize = Me.Size
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv底楕円={0}", My.Settings.frmMainGridOval)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv側面={0}", My.Settings.frmMainGridSide)
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
            lblひとつのすき間の寸法_単位.Text = unitstr
            lbl長さ_単位.Text = unitstr
            lbl計算寸法_単位.Text = unitstr
            lbl横ひも間のすき間_単位.Text = unitstr
            lbl垂直ひも長加算_単位.Text = unitstr
            lbl楕円底円弧の半径加算_単位.Text = unitstr
            lbl楕円底周の加算_単位.Text = unitstr


            nud横寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud縦寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud高さ寸法.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            nud長さ.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
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
            cmb基本色.Refresh()

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

            Me.f_dひも長3.DefaultCellStyle.Format = format

            Me.f_d長さ4.DefaultCellStyle.Format = format
            Me.f_dひも長4.DefaultCellStyle.Format = format

            Me.f_d長さ5.DefaultCellStyle.Format = format
            Me.f_dひも長5.DefaultCellStyle.Format = format

            If Not chk楕円底個別設定.Checked Then
                nud楕円底円弧の半径加算.Value = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底円弧の半径加算")
                nud楕円底周の加算.Value = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底周の加算")
            End If

        End With

        '現データ
        _clsDataTables.ModifySelected()

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

        If {CalcCategory.NewData, CalcCategory.Target}.Contains(category) Then
            Save目標寸法(_clsDataTables.p_row目標寸法)
        End If
        If {CalcCategory.NewData, CalcCategory.Horizontal, CalcCategory.Vertical, CalcCategory.Oval}.Contains(category) Then
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
            Case "tpage底縦横"
                '
            Case "tpage底楕円"
                Show底_楕円(works)
            Case "tpage側面"
                Show側面(works)
            Case "tpage追加品"
                Show追加品(works)
            Case "tpageメモ他"
                '
            Case "tpage底の横"
                Show底の横(works)
            Case "tpage底の縦"
                Show底の縦(works)
            Case Else ' "tpageメモ他"

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
            If {"tpage底楕円", "tpage側面", "tpage追加品", "tpage底の横", "tpage底の縦"}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        'If reason.HasFlag(enumReason._Preview) Then
        '    If {"tpageプレビュー"}.Contains(_CurrentTabControlName) Then
        '        needreset = True
        '    End If
        'End If
        If needreset Then
            TabControl.SelectTab("tpage底縦横")
        End If
    End Sub

    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If TabControl.SelectedTab IsNot Nothing AndAlso
            _CurrentTabControlName = TabControl.SelectedTab.Name Then
            Exit Sub
        End If

        '先のページ名
        Select Case _CurrentTabControlName
            Case "tpage底縦横"
                '
            Case "tpage底楕円"
                Hide底_楕円(_clsDataTables)
            Case "tpage側面"
                Hide側面(_clsDataTables)
            Case "tpage追加品"
                Hide追加品(_clsDataTables)
            Case "tpageメモ他"
                '
            Case "tpage底の横"
                Hide底の横(_clsDataTables)
            Case "tpage底の縦"
                Hide底の縦(_clsDataTables)
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


    Sub Disp底_縦横(ByVal row底_縦横 As clsDataRow)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp底_縦横 {0}", row底_縦横.ToString)
        With row底_縦横
            chk縦横を展開する.Checked = .Value("f_b展開区分")
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

    'clsCalcMeshからセット
    Public Function setひとつのすき間の寸法(ByVal value As Double) As Boolean
        _isChangingByCode = True
        dispValidValueNud(nudひとつのすき間の寸法, value)
        _isChangingByCode = False

        Return True
    End Function


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

            txt垂直ひも数.Text = .p_i垂直ひも数
            txt底楕円の径.Text = .p_s径の合計
            txt厚さ.Text = .p_s厚さ

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

    Function Hide追加品(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource追加品.DataSource)

        BindingSource追加品.Sort = Nothing
        BindingSource追加品.DataSource = Nothing

        Return ret
    End Function


    Function SaveTables(ByVal works As clsDataTables) As Boolean
        Save目標寸法(works.p_row目標寸法)
        Save底_縦横(works.p_row底_縦横)

        works.CheckPoint(works.p_tbl底_楕円)
        works.CheckPoint(works.p_tbl側面)
        works.CheckPoint(works.p_tbl追加品)

        If _CurrentTabControlName = "tpage底の横" Then
            works.FromTmpTable(enumひも種.i_横, BindingSource底の横.DataSource)
        End If
        If _CurrentTabControlName = "tpage底の縦" Then
            works.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, BindingSource底の縦.DataSource)
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
            setBasics()
            recalc(CalcCategory.BsMaster)
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
        If _clsCalcMesh.IsValidInput() Then
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
        Save底_縦横(_clsDataTables.p_row底_縦横)

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
            '四角底{0}-{1}-{2}
            SaveFileDialog1.FileName = String.Format(My.Resources.SaveFileName,
              _clsDataTables.p_row目標寸法.Value("f_i基本のひも幅"), _clsDataTables.p_row底_縦横.Value("f_i長い横ひもの本数"), _clsDataTables.p_row底_縦横.Value("f_i縦ひもの本数"))
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
                setStartEditing()
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
    End Sub

    Private Sub nud基本のひも幅_ValueChanged(sender As Object, e As EventArgs) Handles nud基本のひも幅.ValueChanged
        If nud基本のひも幅.Value = 0 Then
            nud基本のひも幅.Value = g_clsSelectBasics.p_i本幅
        End If
        lbl基本のひも幅length.Text = New Length(g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit

        nud長い横ひも.Value = nud基本のひも幅.Value
        nud縦ひも.Value = nud基本のひも幅.Value
        'イベントは長い横ひも・縦ひも側
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

        recalc(CalcCategory.Vertical, sender)
    End Sub

    Private Sub nudひとつのすき間の寸法_ValueChanged(sender As Object, e As EventArgs) Handles nudひとつのすき間の寸法.ValueChanged
        If 0 < nudひとつのすき間の寸法.Value Then
            txt_ひとつのすき間の寸法_本幅分.Text = New Length(nudひとつのすき間の寸法.Value).ByLaneText
        Else
            txt_ひとつのすき間の寸法_本幅分.Text = ""
        End If

        If Not _isChangingByCode Then
            recalc(CalcCategory.Vertical, sender)
        End If
    End Sub

    Private Sub btn横寸法に合わせる_Click(sender As Object, e As EventArgs) Handles btn横寸法に合わせる.Click
        If Not recalc(CalcCategory.GapFit, sender) Then
            'すき間を横寸法に合わせることができません。
            Dim msg As String = _clsCalcMesh.p_sメッセージ
            msg = msg + vbCrLf + My.Resources.WarningWidthPrime
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub
#End Region

#Region "グリッド編集共通"

    Private Function numberPositionsSelect(ByVal bs As BindingSource, ByVal number As Integer, ByVal dgv As DataGridView) As Boolean
        dgv.ClearSelection()

        Dim positions As New List(Of Integer)
        For pos As Integer = 0 To bs.Count - 1
            Dim r As DataRow = bs.Item(pos).row
            If r("f_i番号") = number Then
                positions.Add(pos)
            End If
        Next
        If positions.Count = 0 Then
            Return False
        End If

        bs.Position = positions(0)

        For Each pos As Integer In positions
            dgv.Rows(pos).Selected = True
        Next
        Return True
    End Function

    Private Function getTableAndNumber(ByVal bs As BindingSource, ByRef table As DataTable, ByRef number As Integer) As Boolean
        Try
            table = bs.DataSource
            If table Is Nothing Then
                Return False
            End If

            Dim current As System.Data.DataRowView = bs.Current
            If current Is Nothing OrElse current.Row Is Nothing Then
                number = -1
                Return True
            End If

            number = current.Row("f_i番号")
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "frmMain.getTableAndNumber")
            Return False

        End Try
    End Function

    Private Sub dgv_DataErrorModify(sender As Object, e As DataGridViewDataErrorEventArgs)
        If e.Exception Is Nothing Then
            Exit Sub
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)

        If dgv.Columns(e.ColumnIndex).DataPropertyName = "f_i何本幅" Then
            '現バンドの本幅数以上の値がセットされているレコードを修正(対象バンドの変更？)

            Dim lane As Integer = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            If lane < 1 Then
                dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 1
                '本幅数の修正
                g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_LaneModified", lane, 1)
            ElseIf g_clsSelectBasics.p_i本幅 < lane Then
                dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = g_clsSelectBasics.p_i本幅
                '本幅数の修正
                g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_LaneModified", lane, g_clsSelectBasics.p_i本幅)
            End If
            e.ThrowException = False

        ElseIf dgv.Columns(e.ColumnIndex).DataPropertyName = "f_s色" Then
            '現バンドにない色がセットされているレコードを修正(対象バンドの変更？)

            '色の修正
            g_clsLog.LogResourceMessage(clsLog.LogLevel.Trouble, "LOG_ColorModified", dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
            dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = ""
            e.ThrowException = False

        Else
            dgv_DataErrorCancel(sender, e, Me.Text)

        End If
    End Sub

    Private Sub dgv_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgv底楕円.CellFormatting, dgv側面.CellFormatting, dgv追加品.CellFormatting, dgv底の横.CellFormatting, dgv底の縦.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If
        If dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly Then
            e.CellStyle.BackColor = Color.LightYellow
        End If
        For Each col As DataGridViewColumn In dgv.Columns
            If col.DataPropertyName = "f_iひも番号" Then
                If dgv.Rows(e.RowIndex).Cells(col.Index).Value = 1 Or
                    dgv.Rows(e.RowIndex).Cells(col.Index).Value = 0 Then
                    If e.ColumnIndex = col.Index Then
                        dgv.Rows(e.RowIndex).HeaderCell.Value = "+"
                    End If
                Else
                    dgv.Rows(e.RowIndex).Height = cRowHeightIdxSub
                End If
                Exit For
            End If
        Next
    End Sub

#End Region

#Region "底楕円"

    Private Sub btn追加_底楕円_Click(sender As Object, e As EventArgs) Handles btn追加_底楕円.Click
        Dim table As tbl底_楕円DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource底_楕円, table, number) Then
            Exit Sub
        End If

        Dim row As tbl底_楕円Row = Nothing
        If _clsCalcMesh.add_底楕円(cmb編みかた名_底楕円.Text,
                     chk差しひも.Checked,
                     nud基本のひも幅.Value, nud周数_底楕円.Value,
                      row) Then

            numberPositionsSelect(BindingSource底_楕円, row.f_i番号, dgv底楕円)
            recalc(CalcCategory.Oval, row, "f_i周数")
        Else
            MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub btn上へ_底楕円_Click(sender As Object, e As EventArgs) Handles btn上へ_底楕円.Click
        Dim table As tbl底_楕円DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource底_楕円, table, number) Then
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

        numberPositionsSelect(BindingSource底_楕円, nextup, dgv底楕円)
    End Sub

    Private Sub btn下へ_底楕円_Click(sender As Object, e As EventArgs) Handles btn下へ_底楕円.Click
        Dim table As tbl底_楕円DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource底_楕円, table, number) Then
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

        numberPositionsSelect(BindingSource底_楕円, nextdown, dgv底楕円)
    End Sub

    Private Sub btn削除_底楕円_Click(sender As Object, e As EventArgs) Handles btn削除_底楕円.Click
        Dim table As tbl底_楕円DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource底_楕円, table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        clsDataTables.RemoveNumberFromTable(table, number)
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

    Private Sub dgv底楕円_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgv底楕円.DataError
        dgv_DataErrorModify(sender, e)
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
        If Not getTableAndNumber(BindingSource側面, table, number) Then
            Exit Sub
        End If

        Dim row As tbl側面Row = Nothing
        If _clsCalcMesh.add_側面(cmb編みかた名_側面.Text, chk縁の始末.Checked,
                     nud基本のひも幅.Value, nud周数_側面.Value,
                      row) Then

            numberPositionsSelect(BindingSource側面, row.f_i番号, dgv側面)
            recalc(CalcCategory.Side, row, "f_i周数")
        Else
            MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub dgv側面_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgv側面.DataError
        dgv_DataErrorModify(sender, e)
    End Sub

    Private Sub btn削除_側面_Click(sender As Object, e As EventArgs) Handles btn削除_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource側面, table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        clsDataTables.RemoveNumberFromTable(table, number)
        recalc(CalcCategory.Side, Nothing, Nothing)
    End Sub

    Private Sub btn上へ_側面_Click(sender As Object, e As EventArgs) Handles btn上へ_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource側面, table, number) Then
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

        numberPositionsSelect(BindingSource側面, nextup, dgv側面)
    End Sub

    Private Sub btn下へ_側面_Click(sender As Object, e As EventArgs) Handles btn下へ_側面.Click
        Dim table As tbl側面DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource側面, table, number) Then
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

        numberPositionsSelect(BindingSource側面, nextdown, dgv側面)
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
    Private Sub btn追加_追加品_Click(sender As Object, e As EventArgs) Handles btn追加_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource追加品, table, number) Then
            Exit Sub
        End If

        Dim row As tbl追加品Row = Nothing
        If _clsCalcMesh.add_追加品(
            cmb付属品名.Text, nud基本のひも幅.Value, nud長さ.Value, nud点数.Value,
            row) Then

            numberPositionsSelect(BindingSource追加品, row.f_i番号, dgv追加品)
            recalc(CalcCategory.Options, row, "f_i点数")

        Else
            MessageBox.Show(_clsCalcMesh.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btn上へ_追加品_Click(sender As Object, e As EventArgs) Handles btn上へ_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource追加品, table, number) Then
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

        numberPositionsSelect(BindingSource追加品, nextup, dgv追加品)
    End Sub

    Private Sub btn下へ_追加品_Click(sender As Object, e As EventArgs) Handles btn下へ_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource追加品, table, number) Then
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

        numberPositionsSelect(BindingSource追加品, nextdown, dgv追加品)
    End Sub

    Private Sub btn削除_追加品_Click(sender As Object, e As EventArgs) Handles btn削除_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        If Not getTableAndNumber(BindingSource追加品, table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If

        clsDataTables.RemoveNumberFromTable(table, number)
        recalc(CalcCategory.Options, Nothing, Nothing)
    End Sub

    Private Sub dgv追加品_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgv追加品.DataError
        dgv_DataErrorModify(sender, e)
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
            If Not TabControl.TabPages.Contains(tpage底の横) Then
                TabControl.TabPages.Add(tpage底の横)
            End If
            If Not TabControl.TabPages.Contains(tpage底の縦) Then
                TabControl.TabPages.Add(tpage底の縦)
            End If
        Else
            If TabControl.TabPages.Contains(tpage底の横) Then
                TabControl.TabPages.Remove(tpage底の横)
            End If
            If TabControl.TabPages.Contains(tpage底の縦) Then
                TabControl.TabPages.Remove(tpage底の縦)
            End If
        End If
    End Sub


    Sub Show底の横(ByVal works As clsDataTables)
        BindingSource底の横.Sort = Nothing
        BindingSource底の横.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        'タブ切り替えタイミングのため、表示は更新済
        Save底_縦横(works.p_row底_縦横)
        Dim tmptable As tbl縦横展開DataTable = _clsCalcMesh.set横展開DataTable()
        works.ToTmpTable(enumひも種.i_横, tmptable)

        BindingSource底の横.DataSource = tmptable
        BindingSource底の横.Sort = "f_iひも種,f_iひも番号"

        dgv底の横.Refresh()
    End Sub

    Function Hide底の横(ByVal works As clsDataTables) As Boolean
        Dim change As Integer = works.FromTmpTable(enumひも種.i_横, BindingSource底の横.DataSource)
        BindingSource底の横.Sort = Nothing
        BindingSource底の横.DataSource = Nothing

        dgv底の横.Refresh()
        Return 0 < change
    End Function

    Sub Show底の縦(ByVal works As clsDataTables)
        BindingSource底の縦.Sort = Nothing
        BindingSource底の縦.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        'タブ切り替えタイミングのため、表示は更新済
        Save底_縦横(works.p_row底_縦横)
        Dim tmptable As tbl縦横展開DataTable = _clsCalcMesh.set縦展開DataTable()
        works.ToTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, tmptable)

        BindingSource底の縦.DataSource = tmptable
        BindingSource底の縦.Sort = "f_iひも種,f_iひも番号"

        dgv底の縦.Refresh()
    End Sub

    Function Hide底の縦(ByVal works As clsDataTables) As Boolean
        Dim change As Integer = works.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, BindingSource底の縦.DataSource)
        BindingSource底の縦.Sort = Nothing
        BindingSource底の縦.DataSource = Nothing

        dgv底の縦.Refresh()
        Return 0 < change
    End Function

    Private Sub dgv底の横_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgv底の横.DataError
        dgv_DataErrorModify(sender, e)
    End Sub

    Private Sub dgv底の縦_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgv底の縦.DataError
        dgv_DataErrorModify(sender, e)
    End Sub

    Private Sub btnリセット_横_Click(sender As Object, e As EventArgs) Handles btnリセット_横.Click
        'ひも長加算と色をすべてクリアします。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetAddLengthColor, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        BindingSource底の横.DataSource = _clsCalcMesh.set横展開DataTable()
        BindingSource底の横.Sort = "f_iひも種,f_iひも番号"
        dgv底の横.Refresh()
    End Sub

    Private Sub btnリセット_縦_Click(sender As Object, e As EventArgs) Handles btnリセット_縦.Click
        'ひも長加算と色をすべてクリアします。よろしいですか？
        Dim r As DialogResult = MessageBox.Show(My.Resources.AskResetAddLengthColor, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        BindingSource底の縦.DataSource = _clsCalcMesh.set縦展開DataTable()
        BindingSource底の縦.Sort = "f_iひも種,f_iひも番号"
        dgv底の縦.Refresh()
    End Sub
#End Region

#Region "DEBUG"
    Dim bVisible As Boolean = False
    Private Sub btnDEBUG_Click(sender As Object, e As EventArgs) Handles btnDEBUG.Click
        If Not bVisible Then
            setDgvColumnsVisible(dgv底楕円)
            setDgvColumnsVisible(dgv側面)
            setDgvColumnsVisible(dgv追加品)
            setDgvColumnsVisible(dgv底の横)
            setDgvColumnsVisible(dgv底の縦)
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
