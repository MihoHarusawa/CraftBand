Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsUpDown
Imports CraftBand.ctrDataGridView
Imports CraftBand.ctrExpanding
Imports CraftBand.ctrInsertBand
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBandHexagon.clsCalcHexagon

Public Class frmMain

    '画面編集用のワーク
    Dim _clsDataTables As New clsDataTables
    '計算用のワーク
    Dim _clsCalcHexagon As New clsCalcHexagon(_clsDataTables, Me)

    '編集中のファイルパス
    Friend _sFilePath As String = Nothing '起動時引数があればセット(issue#8)


    Dim _isLoadingData As Boolean = True 'Designer.vb描画


#Region "基本的な画面処理"

    Dim _Profile_dgv側面 As New CDataGridViewProfile(
            (New tbl側面DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )

    '展開対応
    Dim _tabPages(cAngleCount - 1) As TabPage
    Dim _ctrExpandings(cAngleCount - 1) As ctrExpanding


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _Profile_dgv側面.FormCaption = Me.Text
        dgv側面.SetProfile(_Profile_dgv側面)

        editInsertBand.SetNames(Me.Text, tpage差しひも.Text, My.Resources.EnumStringPlate, My.Resources.EnumStringAngle, Nothing, My.Resources.EnumStringPosition)

        expand横ひも.SetNames(Me.Text, tpage横ひも.Text, True, enumVisible.i_幅 Or enumVisible.i_出力ひも長, My.Resources.CaptionExpand8To2, My.Resources.CaptionExpand4To6)
        expand斜め60度.SetNames(Me.Text, tpage斜め60度.Text, True, enumVisible.i_幅 Or enumVisible.i_出力ひも長, My.Resources.CaptionExpand7to3, My.Resources.CaptionExpand1to9)
        expand斜め120度.SetNames(Me.Text, tpage斜め120度.Text, True, enumVisible.i_幅 Or enumVisible.i_出力ひも長, My.Resources.CaptionExpand1to9, My.Resources.CaptionExpand7to3)

        editAddParts.SetNames(Me.Text, tpage追加品.Text)

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


        '未実装
        'TabControl.TabPages.Remove(tpage差しひも)
        '一旦削除
        TabControl.TabPages.Remove(tpage斜め60度)
        TabControl.TabPages.Remove(tpage横ひも)
        TabControl.TabPages.Remove(tpage斜め120度)

        '固定のテーブルを設定(対象バンドの変更時にはテーブルの中身を変える)
        f_i何本幅2.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅2.DisplayMember = "Display"
        f_i何本幅2.ValueMember = "Value"
        '
        f_s色2.DataSource = g_clsSelectBasics.p_tblColor
        f_s色2.DisplayMember = "Display"
        f_s色2.ValueMember = "Value"
        '
        '展開対応
        _tabPages(cIdxAngle0) = tpage横ひも
        _tabPages(cIdxAngle60) = tpage斜め60度
        _tabPages(cIdxAngle120) = tpage斜め120度
        _ctrExpandings(cIdxAngle0) = expand横ひも
        _ctrExpandings(cIdxAngle60) = expand斜め60度
        _ctrExpandings(cIdxAngle120) = expand斜め120度

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
            colwid = My.Settings.frmMainGridNaname120
            Me.expand斜め120度.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridYoko
            Me.expand横ひも.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridNaname60
            Me.expand斜め60度.SetColumnWidthFromString(colwid)
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
        My.Settings.frmMainGridNaname120 = Me.expand斜め120度.GetColumnWidthString()
        My.Settings.frmMainGridNaname60 = Me.expand斜め60度.GetColumnWidthString()
        My.Settings.frmMainSize = Me.Size
        '
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv側面={0}", My.Settings.frmMainGridSide)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv差しひも={0}", My.Settings.frmMainGridInsertBand)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "dgv追加品={0}", My.Settings.frmMainGridOptions)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "横ひも={0}", My.Settings.frmMainGridYoko)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "斜め120={0}", My.Settings.frmMainGridNaname120)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "斜め60={0}", My.Settings.frmMainGridNaname60)
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

            nud六つ目の高さ.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
            nud三角の中.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces + 1
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
        If {CalcCategory.NewData, CalcCategory.BsMaster, CalcCategory.Hex_Add, CalcCategory.Hex_0_60_120_Gap, CalcCategory.Hex_Vert, CalcCategory.Hex_Expand}.Contains(category) Then
            Save配置数(_clsDataTables.p_row底_縦横)
        End If
        'tableについては更新中をそのまま使用

        Dim ret = _clsCalcHexagon.CalcSize(category, ctr, key)
        Disp計算結果(_clsCalcHexagon) 'NGはToolStripに表示

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
                Disp配置数(works.p_row底_縦横)
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
            Case tpage配置数.Name
                '
            Case tpage側面と縁.Name
                Show側面(works)
            Case tpage差しひも.Name
                Show差しひも(works)
            Case tpageひも上下.Name
                Showひも上下()
            Case tpage追加品.Name
                Show追加品(works)
            Case tpageメモ他.Name
                '
            Case tpage横ひも.Name
                Show展開ひも(AngleIndex._0deg, works)
            Case tpage斜め120度.Name
                Show展開ひも(AngleIndex._120deg, works)
            Case tpage斜め60度.Name
                Show展開ひも(AngleIndex._60deg, works)
            Case tpageプレビュー.Name

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
        _Always = &H100 '#41
    End Enum
    Private Sub ShowDefaultTabControlPage(ByVal reason As enumReason)
        If _isLoadingData Then
            Exit Sub
        End If
        Dim needreset As Boolean = reason.HasFlag(enumReason._Always)
        If reason.HasFlag(enumReason._GridDropdown) Then
            If {tpage側面と縁.Name, tpage追加品.Name, tpage横ひも.Name, tpage斜め120度.Name, tpage斜め60度.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If reason.HasFlag(enumReason._Preview) Then
            If {tpageプレビュー.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If needreset Then
            TabControl.SelectTab(tpage配置数.Name)
        End If
    End Sub

    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If TabControl.SelectedTab IsNot Nothing AndAlso
            _CurrentTabControlName = TabControl.SelectedTab.Name Then
            Exit Sub
        End If

        '先のページ名
        Select Case _CurrentTabControlName
            Case tpage配置数.Name
                '
            Case tpage側面と縁.Name
                Hide側面(_clsDataTables)
            Case tpage差しひも.Name
                Hide差しひも(_clsDataTables)
            Case tpageひも上下.Name
                '
            Case tpage追加品.Name
                Hide追加品(_clsDataTables)
            Case tpageメモ他.Name
                '
            Case tpage横ひも.Name
                Hide展開ひも(AngleIndex._0deg, _clsDataTables)
            Case tpage斜め120度.Name
                Hide展開ひも(AngleIndex._120deg, _clsDataTables)
            Case tpage斜め60度.Name
                Hide展開ひも(AngleIndex._60deg, _clsDataTables)
            Case tpageプレビュー.Name
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


    Sub Disp配置数(ByVal row底_縦横 As clsDataRow)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp配置数 {0}", row底_縦横.dump)
        With row底_縦横
            chk縦横側面を展開する.Checked = .Value("f_b展開区分")
            set底の縦横展開(.Value("f_b展開区分"))

            dispValidValueNud(nud横ひもの本数, .Value("f_i長い横ひもの本数"))
            dispValidValueNud(nud斜めひも本数60度, .Value("f_i斜め60度ひも本数"))
            dispValidValueNud(nud斜めひも本数120度, .Value("f_i斜め120度ひも本数"))
            dispValidValueNud(nud編みひもの本数, Int(.Value("f_d高さの四角数")))  'double

            dispValidValueNud(nud上から何個目, .Value("f_i上から何番目"))
            dispValidValueNud(nud左から何個目, .Value("f_i左から何番目"))
            dispValidValueNud(nud左から何個目120, .Value("f_i左から何番目2"))

            dispValidValueNud(nud六つ目の高さ, .Value("f_dひも間のすき間"))
            nudひも長係数.Value = .Value("f_dひも長係数")
            nudひも長加算_縦横端.Value = .Value("f_d垂直ひも長加算")   'マイナス可
            nudひも長加算_側面.Value = .Value("f_dひも長加算_側面")   'マイナス可

            chk横の補強ひも.Checked = .Value("f_b補強ひも区分")
            chk斜めの補強ひも.Checked = .Value("f_b斜めの補強ひも区分")
            chk斜めの補強ひも_120度.Checked = .Value("f_b斜めの補強ひも2区分")

            txt横ひものメモ.Text = .Value("f_s横ひものメモ")
            txt縦ひものメモ.Text = .Value("f_s縦ひものメモ")

            nud斜め左端右端の目.Value = .Value("f_d左端右端の目")
            nud斜め左端右端の目120.Value = .Value("f_d左端右端の目2")
            nud上端下端の目.Value = .Value("f_d上端下端の目")
            nud最下段の目.Value = .Value("f_d最下段の目")

            chk斜め同数.Checked = .Value("f_b斜め同数区分")

            chkクロスひも.Checked = .Value("f_bクロスひも区分")
            chk高さの六つ目に反映.Checked = .Value("f_b高さ調整区分")
            chkひも中心合わせ.Checked = .Value("f_bひも中心区分")

            Select Case .Value("f_iコマ上側の縦ひも")
                Case enumコマ上側の縦ひも.i_左側
                    rad左綾.Checked = True
                Case enumコマ上側の縦ひも.i_右側
                    rad右綾.Checked = True
                Case Else
                    radなし.Checked = True
            End Select

            Select Case .Value("f_i織りタイプ")
                Case enum織りタイプ.i_3軸織
                    rad鉄線_3軸織.Checked = True
                Case enum織りタイプ.i_単麻の葉
                    rad麻の葉_単方向.Checked = True
                Case enum織りタイプ.i_なし
                    rad織りなし.Checked = True
                Case Else
                    rad巴_3すくみ.Checked = True 'デフォルト
            End Select

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

    Private Sub Disp計算結果(ByVal calc As clsCalcHexagon)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Disp計算結果 {0}", calc.ToString)
        With calc
            '
            txt六つ目ベース_横.Text = .p_s六つ目ベース_横
            txt六つ目ベース_縦.Text = .p_s六つ目ベース_縦
            txt六つ目ベース_高さ.Text = .p_s六つ目ベース_高さ
            txt六つ目ベース_周.Text = .p_s六つ目ベース_周
            txt六つ目ベース_周円の径.Text = .p_s六つ目ベース_周円の径
            txt縁厚さプラス_横.Text = .p_s縁厚さプラス_横
            txt縁厚さプラス_縦.Text = .p_s縁厚さプラス_縦
            txt縁厚さプラス_高さ.Text = .p_s縁厚さプラス_高さ
            txt縁厚さプラス_周.Text = .p_s縁厚さプラス_周
            txt縁厚さプラス_周円の径.Text = .p_s縁厚さプラス_周円の径

            txtひもに垂直_目.Text = .p_sひもに垂直_目
            txt対角線_目.Text = .p_s対角線_目
            txtひもに垂直_ひも幅プラス.Text = .p_sひもに垂直_ひも幅プラス
            txt対角線_ひも幅プラス.Text = .p_s対角線_ひも幅プラス

            txt垂直ひも数.Text = .p_i垂直ひも数
            txt厚さ.Text = .p_s厚さ

            lbl横寸法の差.Text = .p_s横寸法の差
            lbl縦寸法の差.Text = .p_s縦寸法の差
            lbl高さ寸法の差.Text = .p_s高さ寸法の差

            lbl横ひも本幅変更.Visible = .p_bひも本幅変更(AngleIndex._0deg)
            lbl60度本幅変更.Visible = .p_bひも本幅変更(AngleIndex._60deg)
            lbl120度本幅変更.Visible = .p_bひも本幅変更(AngleIndex._120deg)
            lbl側面ひも本幅変更.Visible = .p_b側面ひも本幅変更
            lblひも本幅変更.Visible = .p_bひも本幅変更(AngleIndex._0deg) OrElse .p_bひも本幅変更(AngleIndex._60deg) OrElse .p_bひも本幅変更(AngleIndex._120deg) OrElse .p_b側面ひも本幅変更

            txt側面周比率対底.Text = .p_s側面周比率対底

            If .p_b有効 Then
                ToolStripStatusLabel1.Text = "OK"
                ToolStripStatusLabel2.Text = .p_s警告
            Else
                ToolStripStatusLabel1.Text = "NG"
                ToolStripStatusLabel2.Text = .p_sメッセージ
            End If
        End With

    End Sub

    Function SaveTables(ByVal works As clsDataTables) As Boolean
        Save目標寸法(works.p_row目標寸法)
        Save配置数(works.p_row底_縦横)

        works.CheckPoint(works.p_tbl側面)
        works.CheckPoint(works.p_tbl追加品)

        editInsertBand.Save(works)
        expand横ひも.Save(idxひも種(AngleIndex._0deg), works)
        expand斜め60度.Save(idxひも種(AngleIndex._60deg), works)
        expand斜め120度.Save(idxひも種(AngleIndex._120deg), works)

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

    Function Save配置数(ByVal row底_縦横 As clsDataRow) As Boolean
        With row底_縦横
            .Value("f_b展開区分") = chk縦横側面を展開する.Checked

            .Value("f_d高さの四角数") = nud高さの六つ目数.Value 'double

            .Value("f_i長い横ひもの本数") = nud横ひもの本数.Value
            .Value("f_i斜め60度ひも本数") = nud斜めひも本数60度.Value
            .Value("f_b斜めの補強ひも区分") = chk斜めの補強ひも.Checked
            If chk斜め同数.Checked Then
                .Value("f_b斜め同数区分") = True
                .Value("f_i斜め120度ひも本数") = nud斜めひも本数60度.Value
                .Value("f_b斜めの補強ひも2区分") = chk斜めの補強ひも.Checked
                .Value("f_i左から何番目2") = nud左から何個目.Value
                .Value("f_d左端右端の目2") = nud斜め左端右端の目.Value
            Else
                .Value("f_b斜め同数区分") = False
                .Value("f_i斜め120度ひも本数") = nud斜めひも本数120度.Value
                .Value("f_b斜めの補強ひも2区分") = chk斜めの補強ひも_120度.Checked
                .Value("f_i左から何番目2") = nud左から何個目120.Value
                .Value("f_d左端右端の目2") = nud斜め左端右端の目120.Value
            End If
            .Value("f_b補強ひも区分") = chk横の補強ひも.Checked

            .Value("f_i上から何番目") = nud上から何個目.Value
            .Value("f_i左から何番目") = nud左から何個目.Value

            .Value("f_dひも間のすき間") = nud六つ目の高さ.Value
            .Value("f_dひも長係数") = nudひも長係数.Value
            .Value("f_d垂直ひも長加算") = nudひも長加算_縦横端.Value   'マイナス可
            .Value("f_dひも長加算_側面") = nudひも長加算_側面.Value 'マイナス可

            .Value("f_d左端右端の目") = nud斜め左端右端の目.Value
            .Value("f_d上端下端の目") = nud上端下端の目.Value
            .Value("f_d最下段の目") = nud最下段の目.Value

            .Value("f_s横ひものメモ") = txt横ひものメモ.Text
            .Value("f_s縦ひものメモ") = txt縦ひものメモ.Text

            .Value("f_bクロスひも区分") = chkクロスひも.Checked
            .Value("f_b高さ調整区分") = chk高さの六つ目に反映.Checked
            .Value("f_bひも中心区分") = chkひも中心合わせ.Checked

            If rad左綾.Checked Then
                .Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_左側
            ElseIf rad右綾.Checked Then
                .Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_右側
            Else
                .Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_どちらでも
            End If

            If rad鉄線_3軸織.Checked Then
                .Value("f_i織りタイプ") = enum織りタイプ.i_3軸織
            ElseIf rad麻の葉_単方向.Checked Then
                .Value("f_i織りタイプ") = enum織りタイプ.i_単麻の葉
            ElseIf rad巴_3すくみ.Checked Then
                .Value("f_i織りタイプ") = enum織りタイプ.i_3すくみ
            Else
                .Value("f_i織りタイプ") = enum織りタイプ.i_なし
            End If

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
        Dim cond_yoko As String = String.Format(fmt, idxひも種(AngleIndex._0deg), idxひも種補強(AngleIndex._0deg))
        Dim cond_60 As String = String.Format(fmt, idxひも種(AngleIndex._60deg), idxひも種補強(AngleIndex._60deg))
        Dim cond_120 As String = String.Format(fmt, idxひも種(AngleIndex._120deg), idxひも種補強(AngleIndex._120deg))

        Dim _ColorChangeSettings() As CColorChangeSetting = {
           New CColorChangeSetting(tpage側面と縁.Text, enumDataID._tbl側面, "0<f_i番号", False),
           New CColorChangeSetting(tpage差しひも.Text, enumDataID._tbl差しひも, Nothing, False),
           New CColorChangeSetting(tpage追加品.Text, enumDataID._tbl追加品, Nothing, False),
           New CColorChangeSetting(tpage横ひも.Text, enumDataID._tbl縦横展開, cond_yoko, True),
           New CColorChangeSetting(tpage斜め60度.Text, enumDataID._tbl縦横展開, cond_60, True),
           New CColorChangeSetting(tpage斜め120度.Text, enumDataID._tbl縦横展開, cond_120, True)}

        Return CreateColorChangeForm(_ColorChangeSettings)
    End Function
    '色変更
    Private Sub ToolStripMenuItemEditColorChange_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditColorChange.Click
        SaveTables(_clsDataTables)
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色変更

        '#40:縦横のレコード数
        If chk縦横側面を展開する.Checked Then
            _clsCalcHexagon.prepare展開DataTable()
        End If
        _clsCalcHexagon.adjust_側面()

        If 0 < ShowColorChangeForm(_clsDataTables) Then
            recalc(CalcCategory.BandColor)
        End If
    End Sub

    '色と幅の繰り返しの初期化(#51)
    Private Function initColorRepeat() As Boolean

        Dim _ColorRepeatSettings() As CColorRepeatSetting = {
        New CColorRepeatSetting(lbl側面の編みひも.Text, enumDataID._tbl側面, "f_i番号=1", "f_iひも番号 ASC", True, False),
        New CColorRepeatSetting(tpage横ひも.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", idxひも種(AngleIndex._0deg)), "f_iひも番号 ASC", True, True),
        New CColorRepeatSetting(tpage斜め60度.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", idxひも種(AngleIndex._60deg)), "f_iひも番号 ASC", True, True),
        New CColorRepeatSetting(tpage斜め120度.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", idxひも種(AngleIndex._120deg)), "f_iひも番号 ASC", True, True)}

        Return CreateColorRepeatForm(_ColorRepeatSettings)
    End Function
    '色と幅の繰り返し
    Private Sub ToolStripMenuItemEditColorRepeat_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditColorRepeat.Click
        SaveTables(_clsDataTables)
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色変更

        If chk縦横側面を展開する.Checked Then
            _clsCalcHexagon.prepare展開DataTable()
        End If
        _clsCalcHexagon.adjust_側面()

        If 0 < ShowColorRepeatForm(_clsDataTables) Then
            recalc(CalcCategory.BandColor)
        End If
    End Sub

    'リセット
    Private Sub ToolStripMenuItemEditReset_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditReset.Click
        Dim r As DialogResult = DialogResult.Yes
        If _clsCalcHexagon.IsValidInput() Then '#22
            '目標寸法以外をリセットします。六つ目の高さもリセットしてよろしいですか？
            '(はいで全てリセット、いいえで六つ目の高さを保持)
            r = MessageBox.Show(My.Resources.AskResetInput, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
            If r <> DialogResult.Yes AndAlso r <> DialogResult.No Then
                Exit Sub
            End If
        End If
        Dim d目 As Double = nud六つ目の高さ.Value
        ShowDefaultTabControlPage(enumReason._Always)

        _clsDataTables.Clear()
        _clsDataTables.SetInitialValue()
        Save目標寸法(_clsDataTables.p_row目標寸法) '画面値で上書き
        If r = DialogResult.No Then
            _clsDataTables.p_row底_縦横.Value("f_dひも間のすき間") = nud六つ目の高さ.Value
        Else
            _clsDataTables.p_row底_縦横.Value("f_dひも間のすき間") =
                get幅from三角(_clsDataTables.p_row底_縦横.Value("f_dひも間のすき間"), nud基本のひも幅.Value)
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
        If _clsCalcHexagon.IsValidInput() Then
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
        If _clsCalcHexagon.CheckTarget(message) Then
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
            MessageBox.Show(_clsCalcHexagon.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        '実行する
        ShowDefaultTabControlPage(enumReason._Always)
        If _clsCalcHexagon.CalcTarget() Then
            _isLoadingData = True
            '計算結果の縦横値
            Disp配置数(_clsDataTables.p_row底_縦横)
            '(側面のテーブルも変更されている)
            _isLoadingData = False
        Else
            'エラーあり
            MessageBox.Show(_clsCalcHexagon.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        recalc(CalcCategory.NewData)
    End Sub

    'ひもリスト
    Private Sub ToolStripMenuItemEditList_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditList.Click
        SaveTables(_clsDataTables)
        '_clsCalcSquare.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        _clsCalcHexagon.CalcSize(CalcCategory.FixLength, Nothing, Nothing)

        Dim msg As String = Nothing
        If Not _clsCalcHexagon.p_b有効 Then
            msg = _clsCalcHexagon.p_sメッセージ
        ElseIf Not String.IsNullOrEmpty(_clsCalcHexagon.p_s警告) Then
            msg = _clsCalcHexagon.p_s警告
        End If
        If Not String.IsNullOrEmpty(msg) Then
            msg += vbCrLf
            'このままリスト出力を行いますか？
            msg += My.Resources.AskOutput
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        Dim output As New clsOutput(_sFilePath)
        If Not _clsCalcHexagon.CalcOutput(output) Then
            MessageBox.Show(_clsCalcHexagon.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
        Save配置数(_clsDataTables.p_row底_縦横)

        _clsDataTables.ResetStartPoint()

        Me.Text = String.Format(My.Resources.FormCaption, IO.Path.GetFileNameWithoutExtension(_sFilePath))

        'タブ表示(#41)
        If showTabBase Then
            ShowDefaultTabControlPage(enumReason._Always)
        End If
        'プレビュー初期化
        reset_preview()
    End Sub

    '新規作成
    Private Sub ToolStripMenuItemFileNew_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileNew.Click
        If IsContinueEditing() Then
            Exit Sub
        End If

        _clsDataTables.Clear()
        _clsDataTables.SetInitialValue()
        '調整値
        _clsDataTables.p_row底_縦横.Value("f_dひも間のすき間") = get幅from三角(_clsDataTables.p_row底_縦横.Value("f_dひも間のすき間"), g_clsSelectBasics.p_i本幅)
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
                0 < _clsCalcHexagon.p_d六つ目ベース_横 AndAlso 0 < _clsCalcHexagon.p_d六つ目ベース_縦 Then
                nud横寸法.Value = _clsCalcHexagon.p_d六つ目ベース_横
                nud縦寸法.Value = _clsCalcHexagon.p_d六つ目ベース_縦
                nud高さ寸法.Value = 1 '空の解除
                nud高さ寸法.Value = _clsCalcHexagon.p_d六つ目ベース_高さ
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

#Region "目標・配置数のコントロール"
    '縦横の展開チェックボックス　※チェックは最初のタブにある
    Private Sub chk縦横を展開する_CheckedChanged(sender As Object, e As EventArgs) Handles chk縦横側面を展開する.CheckedChanged
        set底の縦横展開(chk縦横側面を展開する.Checked)
        recalc(CalcCategory.Hex_Expand, sender)
    End Sub

    Private Sub nud基本のひも幅_ValueChanged(sender As Object, e As EventArgs) Handles nud基本のひも幅.ValueChanged
        If nud基本のひも幅.Value = 0 Then
            nud基本のひも幅.Value = g_clsSelectBasics.p_i本幅
        End If
        lbl基本のひも幅length.Text = New Length(g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit

        _TriangleChangeByCode = True
        If 0 < g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value) Then
            txtひも幅比.Text = (nud六つ目の高さ.Value / g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).ToString("F2")
            nud三角の中.Value = get三角from幅(nud六つ目の高さ.Value, nud基本のひも幅.Value)
        Else
            txtひも幅比.Text = ""
            nud三角の中.Value = 0
            nud三角の中.ResetText()
        End If
        _TriangleChangeByCode = False

        ShowDefaultTabControlPage(enumReason._Preview)
        recalc(CalcCategory.Target_Band, sender)
    End Sub

    Private Sub chkひも中心合わせ_CheckedChanged(sender As Object, e As EventArgs) Handles chkひも中心合わせ.CheckedChanged
        nud三角の中.Visible = Not chkひも中心合わせ.Checked
        lbl三角の中.Visible = Not chkひも中心合わせ.Checked
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
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

    '横置き
    Private Sub nud横ひもの本数_ValueChanged(sender As Object, e As EventArgs) Handles nud横ひもの本数.ValueChanged
        '偶数推奨
        If nud横ひもの本数.Value Mod 2 = 0 AndAlso Not chkひも中心合わせ.Checked Then
            nud横ひもの本数.Increment = 2
        Else
            nud横ひもの本数.Increment = 1
        End If
        '合わせ位置
        If 1 < nud横ひもの本数.Value Then
            If chkひも中心合わせ.Checked Then
                nud上から何個目.Value = (nud横ひもの本数.Value + 1) \ 2
            Else
                nud上から何個目.Value = nud横ひもの本数.Value \ 2
            End If
        Else
            nud上から何個目.Value = 0
        End If
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub

    '合わせる位置
    Private Sub nud上から何個目_ValueChanged(sender As Object, e As EventArgs) Handles nud上から何個目.ValueChanged
        '横ひもの本数以下
        If nud横ひもの本数.Value < nud上から何個目.Value Then
            If 1 < nud横ひもの本数.Value Then
                nud上から何個目.Value = nud横ひもの本数.Value - 1 'デフォルトは目
            Else
                nud上から何個目.Value = 0
            End If
        End If
        '目で合わせる場合は、横ひもの本数-1をチェックします
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub
    Private Sub nud上端下端の目_ValueChanged(sender As Object, e As EventArgs) Handles nud上端下端の目.ValueChanged
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub
    Private Sub chk横の補強ひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk横の補強ひも.CheckedChanged
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub


    '斜め置き
    Private Sub nud斜めひも本数60度_ValueChanged(sender As Object, e As EventArgs) Handles nud斜めひも本数60度.ValueChanged
        '偶数推奨
        If nud斜めひも本数60度.Value Mod 2 = 0 AndAlso Not chkひも中心合わせ.Checked Then
            nud斜めひも本数60度.Increment = 2
        Else
            nud斜めひも本数60度.Increment = 1
        End If
        If 1 < nud斜めひも本数60度.Value Then
            If chkひも中心合わせ.Checked Then
                nud左から何個目.Value = (nud斜めひも本数60度.Value + 1) \ 2
            Else
                nud左から何個目.Value = nud斜めひも本数60度.Value \ 2
            End If
        Else
            nud左から何個目.Value = 0
        End If
        If chk斜め同数.Checked Then
            nud斜めひも本数120度.Value = nud斜めひも本数60度.Value
            nud左から何個目120.Value = nud左から何個目.Value
        End If
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub
    '合わせ目
    Private Sub nud左から何個目_ValueChanged(sender As Object, e As EventArgs) Handles nud左から何個目.ValueChanged
        '斜めひも本数-1以下
        If nud斜めひも本数60度.Value < nud左から何個目.Value Then
            If 1 < nud斜めひも本数60度.Value Then
                nud左から何個目.Value = nud斜めひも本数60度.Value - 1 'デフォルトは目
            Else
                nud左から何個目.Value = 0
            End If
        End If
        If chk斜め同数.Checked Then
            nud左から何個目120.Value = nud左から何個目.Value
        End If
        '目で合わせる場合は、斜めひも本数-1をチェックします
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub
    Private Sub chk斜めの補強ひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk斜めの補強ひも.CheckedChanged
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub
    Private Sub nud斜め左端右端の目_ValueChanged(sender As Object, e As EventArgs) Handles nud斜め左端右端の目.ValueChanged
        If chk斜め同数.Checked Then
            nud斜め左端右端の目120.Value = nud斜め左端右端の目.Value
        End If
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub

    '斜め同数
    Private Sub chk斜め同数_CheckedChanged(sender As Object, e As EventArgs) Handles chk斜め同数.CheckedChanged
        Dim visible As Boolean = Not chk斜め同数.Checked
        lbl60度.Visible = visible
        lbl120度.Visible = visible
        nud斜めひも本数120度.Visible = visible
        lbl120度ひも本数_単位.Visible = visible
        lblchk60度.Visible = visible
        chk斜めの補強ひも_120度.Visible = visible
        expand斜め120度.AddDeleteButtonVisible(visible)
        lbl六つ目60度.Visible = visible
        nud左から何個目120.Visible = visible
        lbl左合わせ目120_単位.Visible = visible
        nud斜め左端右端の目120.Visible = visible
        lbl六つ目120度.Visible = visible
        lbl斜め左端右端120_単位.Visible = visible

#If 1 Then
        '表裏のプレビューは同数時のみ
        radうら.Visible = Not visible
        radおもて.Visible = Not visible
        reset_preview()
#End If

        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub

#Region "同数ではない時のみ"
    Private Sub nud斜めひも本数120度_ValueChanged(sender As Object, e As EventArgs) Handles nud斜めひも本数120度.ValueChanged
        '偶数推奨
        If nud斜めひも本数120度.Value Mod 2 = 0 AndAlso Not chkひも中心合わせ.Checked Then
            nud斜めひも本数120度.Increment = 2
        Else
            nud斜めひも本数120度.Increment = 1
        End If
        If 1 < nud斜めひも本数120度.Value Then
            If chkひも中心合わせ.Checked Then
                nud左から何個目120.Value = (nud斜めひも本数120度.Value + 1) \ 2
            Else
                nud左から何個目120.Value = nud斜めひも本数120度.Value \ 2
            End If
        Else
            nud左から何個目120.Value = 0
        End If
        If nud斜めひも本数120度.Visible Then
            recalc(CalcCategory.Hex_0_60_120_Gap, sender)
        End If
    End Sub

    Private Sub chk斜めの補強ひも_120度_CheckedChanged(sender As Object, e As EventArgs) Handles chk斜めの補強ひも_120度.CheckedChanged
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub

    Private Sub nud左から何個目120_ValueChanged(sender As Object, e As EventArgs) Handles nud左から何個目120.ValueChanged
        If nud斜めひも本数120度.Value < nud左から何個目120.Value Then
            If 1 < nud斜めひも本数120度.Value Then
                nud左から何個目120.Value = nud斜めひも本数120度.Value - 1 'デフォルトは目
            Else
                nud左から何個目120.Value = 0
            End If
        End If
        If nud左から何個目120.Visible Then
            recalc(CalcCategory.Hex_0_60_120_Gap, sender)
        End If
    End Sub

    Private Sub nud斜め左端右端の目120_ValueChanged(sender As Object, e As EventArgs) Handles nud斜め左端右端の目120.ValueChanged
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub

#End Region

    '側面
    Private Sub nud編みひもの本数_ValueChanged(sender As Object, e As EventArgs) Handles nud編みひもの本数.ValueChanged
        nud高さの六つ目数.Value = nud編みひもの本数.Value
        recalc(CalcCategory.Hex_Vert, sender)
    End Sub
    'read only
    Private Sub nud高さの六つ目数_ValueChanged(sender As Object, e As EventArgs) Handles nud高さの六つ目数.ValueChanged
        nud編みひもの本数.Value = nud高さの六つ目数.Value
    End Sub
    Private Sub nud最下段の目_ValueChanged(sender As Object, e As EventArgs) Handles nud最下段の目.ValueChanged
        recalc(CalcCategory.Hex_Vert, sender)
    End Sub

    Private _TriangleChangeByCode As Boolean = False
    Sub nud六つ目の高さ_ValueChanged(sender As Object, e As EventArgs) Handles nud六つ目の高さ.ValueChanged
        _TriangleChangeByCode = True
        txt目_本幅分.Text = New Length(nud六つ目の高さ.Value).ByLaneText
        txt目対角線_本幅分.Text = New Length(nud六つ目の高さ.Value / SIN60).ByLaneText
        If 0 < g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value) Then
            txtひも幅比.Text = (nud六つ目の高さ.Value / g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).ToString("F2")
            nud三角の中.Value = get三角from幅(nud六つ目の高さ.Value, nud基本のひも幅.Value)
        Else
            txtひも幅比.Text = ""
            nud三角の中.Value = 0
            nud三角の中.ResetText()
        End If
        _TriangleChangeByCode = False

        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub

    Private Sub nud三角の中_ValueChanged(sender As Object, e As EventArgs) Handles nud三角の中.ValueChanged
        If Not _TriangleChangeByCode AndAlso Not _isLoadingData Then
            nud六つ目の高さ.Value = get幅from三角(nud三角の中.Value, nud基本のひも幅.Value)
        End If
    End Sub

    Private Sub nudひも長係数_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長係数.ValueChanged
        recalc(CalcCategory.Hex_Add, sender)
    End Sub

    Private Sub nudひも長加算_縦横端_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長加算_縦横端.ValueChanged
        recalc(CalcCategory.Hex_Add, sender)
    End Sub

    Private Sub nudひも長加算_側面_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長加算_側面.ValueChanged
        recalc(CalcCategory.Hex_Vert, sender)
    End Sub

    Private Sub chkクロスひも_CheckedChanged(sender As Object, e As EventArgs) Handles chkクロスひも.CheckedChanged
        recalc(CalcCategory.Hex_0_60_120_Gap, sender)
    End Sub

    Private Sub chk高さの六つ目に反映_CheckedChanged(sender As Object, e As EventArgs) Handles chk高さの六つ目に反映.CheckedChanged
        recalc(CalcCategory.Hex_Vert, sender)
    End Sub

#End Region

#Region "側面"

    Sub Show側面(ByVal works As clsDataTables)
        BindingSource側面.Sort = Nothing
        BindingSource側面.DataSource = Nothing
        If works Is Nothing Then
            Exit Sub
        End If

        Save配置数(works.p_row底_縦横) '縦横側面展開値
        If Not _clsCalcHexagon.CalcSize(CalcCategory.SideEdge, Nothing, Nothing) Then '側面と縁
            MessageBox.Show(_clsCalcHexagon.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
            nud高さの六つ目数.Value = nud高さの六つ目数.Value + 1
            Exit Sub
        End If

        Dim table As tbl側面DataTable = Nothing
        Dim currow As tbl側面Row = Nothing
        If Not dgv側面.GetTableAndRow(table, currow) Then
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(cmb編みかた名_側面.Text) Then
            '編みかた名指定なし
            If _clsCalcHexagon.add_高さ(currow) Then
                If currow IsNot Nothing Then
                    dgv側面.PositionSelect(currow, {"f_i番号", "f_iひも番号"})
                End If
                nud高さの六つ目数.Value = nud高さの六つ目数.Value + 1 '→recalc
            End If
        Else
            '編みかた名指定あり
            Dim row As tbl側面Row = Nothing
            If _clsCalcHexagon.add_縁(cmb編みかた名_側面.Text, nud基本のひも幅.Value, row) Then
                dgv側面.NumberPositionsSelect(row.f_i番号)
                recalc(CalcCategory.SideEdge, row, "f_i周数")
            Else
                MessageBox.Show(_clsCalcHexagon.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
        Else
            '高さを削除
            If _clsCalcHexagon.del_高さ(currow) Then
                If currow IsNot Nothing Then
                    dgv側面.PositionSelect(currow, {"f_i番号", "f_iひも番号"})
                End If
                nud高さの六つ目数.Value = nud高さの六つ目数.Value - 1 '→recalc
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
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv側面_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If IsDataPropertyName側面と縁(DataPropertyName) Then
            recalc(CalcCategory.SideEdge, current.Row, DataPropertyName)
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

#Region "斜めと横展開"
    '_tabPages,_ctrExpandings参照

    'タブの表示・非表示(タブのインスタンスは保持)
    Private Sub set底の縦横展開(ByVal isExband As Boolean)
        If isExband Then
            For Each tpage As TabPage In _tabPages
                If Not TabControl.TabPages.Contains(tpage) Then
                    TabControl.TabPages.Add(tpage)
                End If
            Next

        Else
            For Each tpage As TabPage In _tabPages
                If TabControl.TabPages.Contains(tpage) Then
                    TabControl.TabPages.Remove(tpage)
                End If
            Next

        End If
    End Sub

    Private Sub tpage横ひも_Resize(sender As Object, e As EventArgs) Handles tpage横ひも.Resize
        expand横ひも.PanelSize = tpage横ひも.Size
    End Sub
    Private Sub tpage斜め120度_Resize(sender As Object, e As EventArgs) Handles tpage斜め120度.Resize
        expand斜め120度.PanelSize = tpage斜め120度.Size
    End Sub
    Private Sub tpage斜め60度_Resize(sender As Object, e As EventArgs) Handles tpage斜め60度.Resize
        expand斜め60度.PanelSize = tpage斜め60度.Size
    End Sub


    Private Sub Show展開ひも(ByVal aidx As AngleIndex, ByVal works As clsDataTables)
        Save配置数(works.p_row底_縦横) '補強ひも・キャッシュなし
        _ctrExpandings(idx(aidx)).PanelSize = _tabPages(idx(aidx)).Size
        _ctrExpandings(idx(aidx)).ShowGrid(_clsCalcHexagon.get展開DataTable(aidx))
    End Sub

    Private Function Hide展開ひも(ByVal aidx As AngleIndex, ByVal works As clsDataTables) As Boolean
        Return _ctrExpandings(idx(aidx)).HideGrid(idxひも種(aidx), works)
    End Function



    Private Sub expand横ひも_CellValueChanged(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.CellValueChanged
        recalc(CalcCategory.Expand_0, e.Row, e.DataPropertyName)
    End Sub
    Private Sub expand横ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.ResetButton
        _clsDataTables.Removeひも種Rows(idxひも種(AngleIndex._0deg))
        expand横ひも.DataSource = _clsCalcHexagon.get展開DataTable(AngleIndex._0deg, True)
    End Sub
    Private Sub expand横ひも_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.AddButton
        Dim currow As tbl縦横展開Row = e.Row
        If _clsCalcHexagon.add_ひも(AngleIndex._0deg, currow) Then
            nud横ひもの本数.Value = nud横ひもの本数.Value + 1 'with recalc
            If currow IsNot Nothing Then
                expand横ひも.PositionSelect(currow)
            End If
        End If
    End Sub
    Private Sub expand横ひも_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.DeleteButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow IsNot Nothing Then
            If is_idx補強(AngleIndex._0deg, currow.f_iひも種) Then
                If currow.f_iひも番号 = ciひも番号_クロス Then
                    chkクロスひも.Checked = False 'with recalc
                Else
                    chk横の補強ひも.Checked = False 'with recalc
                End If
                Exit Sub
            ElseIf is_idxすき間(AngleIndex._0deg, currow.f_iひも種) Then
                nud上端下端の目.Value = 0 'with recalc
                Exit Sub
            End If
        End If
        If _clsCalcHexagon.del_ひも(AngleIndex._0deg, currow) Then
            nud横ひもの本数.Value = nud横ひもの本数.Value - 1 'with recalc
            If currow IsNot Nothing Then
                expand横ひも.PositionSelect(currow)
            End If
        End If
    End Sub


    Private Sub expand斜め60度_CellValueChanged(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand斜め60度.CellValueChanged
        recalc(CalcCategory.Expand_60, e.Row, e.DataPropertyName)
    End Sub
    Private Sub expand斜め60度_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand斜め60度.ResetButton
        _clsDataTables.Removeひも種Rows(idxひも種(AngleIndex._60deg))
        expand斜め60度.DataSource = _clsCalcHexagon.get展開DataTable(AngleIndex._60deg, True)
    End Sub
    Private Sub expand斜め60度_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand斜め60度.AddButton
        Dim currow As tbl縦横展開Row = e.Row
        If _clsCalcHexagon.add_ひも(AngleIndex._60deg, currow) Then
            nud斜めひも本数60度.Value = nud斜めひも本数60度.Value + 1 'with recalc
            If currow IsNot Nothing Then
                expand斜め60度.PositionSelect(currow)
            End If
        End If
    End Sub
    Private Sub expand斜め60度_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand斜め60度.DeleteButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow IsNot Nothing Then
            If is_idx補強(AngleIndex._60deg, currow.f_iひも種) Then
                If currow.f_iひも番号 = ciひも番号_クロス Then
                    chkクロスひも.Checked = False 'with recalc
                Else
                    chk斜めの補強ひも.Checked = False 'with recalc
                End If
                Exit Sub
            ElseIf is_idxすき間(AngleIndex._60deg, currow.f_iひも種) Then
                nud斜め左端右端の目.Value = 0 'with recalc
                Exit Sub
            End If
        End If
        If _clsCalcHexagon.del_ひも(AngleIndex._60deg, currow) Then
            nud斜めひも本数60度.Value = nud斜めひも本数60度.Value - 1 'with recalc
            If currow IsNot Nothing Then
                expand斜め60度.PositionSelect(currow)
            End If
        End If
    End Sub

    Private Sub expand斜め120度_CellValueChanged(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand斜め120度.CellValueChanged
        recalc(CalcCategory.Expand_120, e.Row, e.DataPropertyName)
    End Sub
    Private Sub expand斜め120度_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand斜め120度.ResetButton
        _clsDataTables.Removeひも種Rows(idxひも種(AngleIndex._120deg))
        expand斜め120度.DataSource = _clsCalcHexagon.get展開DataTable(AngleIndex._120deg, True)
    End Sub
    '追加・削除ボタンは、同数でない場合のみ表示される
    Private Sub expand斜め120度_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand斜め120度.AddButton
        Dim currow As tbl縦横展開Row = e.Row
        If _clsCalcHexagon.add_ひも(AngleIndex._120deg, currow) Then
            nud斜めひも本数120度.Value = nud斜めひも本数120度.Value + 1 'with recalc
            If currow IsNot Nothing Then
                expand斜め120度.PositionSelect(currow)
            End If
        End If
    End Sub
    Private Sub expand斜め120度_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand斜め120度.DeleteButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow IsNot Nothing Then
            If is_idx補強(AngleIndex._120deg, currow.f_iひも種) Then
                If currow.f_iひも番号 = ciひも番号_クロス Then
                    chkクロスひも.Checked = False 'with recalc
                Else
                    chk斜めの補強ひも_120度.Checked = False 'with recalc
                End If
                Exit Sub
            ElseIf is_idxすき間(AngleIndex._120deg, currow.f_iひも種) Then
                nud斜め左端右端の目120.Value = 0 'with recalc
                Exit Sub
            End If
        End If
        If _clsCalcHexagon.del_ひも(AngleIndex._120deg, currow) Then
            nud斜めひも本数120度.Value = nud斜めひも本数120度.Value - 1 'with recalc
            If currow IsNot Nothing Then
                expand斜め120度.PositionSelect(currow)
            End If
        End If
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
#End Region

#Region "ひも上下"
    '三角の中の値←幅
    Public Function get三角from幅(d六つ目の高さ As Double, iひも幅 As Integer) As Double
        Return (d六つ目の高さ / 2) - g_clsSelectBasics.p_d指定本幅(iひも幅)
    End Function

    '幅←三角の中の値
    Public Function get幅from三角(d三角の中 As Double, iひも幅 As Integer) As Double
        Return (d三角の中 + g_clsSelectBasics.p_d指定本幅(iひも幅)) * 2
    End Function

    Sub Showひも上下()
        show織りCheck()
    End Sub

    Private Sub rad綾の方向_CheckedChanged(sender As Object, e As EventArgs) Handles rad左綾.CheckedChanged, rad右綾.CheckedChanged, radなし.CheckedChanged
        If Not _isLoadingData Then
            If rad左綾.Checked Then
                _clsDataTables.p_row底_縦横.Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_左側
            ElseIf rad右綾.Checked Then
                _clsDataTables.p_row底_縦横.Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_右側
            Else
                _clsDataTables.p_row底_縦横.Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_どちらでも
            End If
            show織りCheck()
        End If
    End Sub

    Private Sub rad織りタイプ_CheckedChanged(sender As Object, e As EventArgs) Handles rad巴_3すくみ.CheckedChanged, rad麻の葉_単方向.CheckedChanged, rad鉄線_3軸織.CheckedChanged
        If Not _isLoadingData Then
            If rad鉄線_3軸織.Checked Then
                _clsDataTables.p_row底_縦横.Value("f_i織りタイプ") = enum織りタイプ.i_3軸織
            ElseIf rad麻の葉_単方向.Checked Then
                _clsDataTables.p_row底_縦横.Value("f_i織りタイプ") = enum織りタイプ.i_単麻の葉
            ElseIf rad巴_3すくみ.Checked Then
                _clsDataTables.p_row底_縦横.Value("f_i織りタイプ") = enum織りタイプ.i_3すくみ
            Else
                _clsDataTables.p_row底_縦横.Value("f_i織りタイプ") = enum織りタイプ.i_なし
            End If
            show織りCheck()
        End If
    End Sub

    Private Sub show織りCheck()
        Dim msg As String = Nothing
        If rad鉄線_3軸織.Checked Then
            msg = _clsCalcHexagon.Check3軸織()
        ElseIf rad麻の葉_単方向.Checked Then
            msg = _clsCalcHexagon.Check単麻の葉()
        ElseIf rad巴_3すくみ.Checked Then
            msg = _clsCalcHexagon.Check3すくみ()
        Else
            '織りタイプは指定されていません。
            msg = My.Resources.CalcNoPattern
        End If
        lblMessagePattern.Text = msg
    End Sub

#End Region

#Region "プレビュー"
    Dim _clsImageData As clsImageData
    Private Sub Showプレビュー(works As clsDataTables)
        picプレビュー.Image = Nothing
        _clsImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcHexagon.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcHexagon) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If

        CalcImageData()
    End Sub

    Private Sub CalcImageData()
        If ToolStripStatusLabel1.Text = "OK" Then

            Dim isBackFace As Boolean = radうら.Checked

            Dim checked(cAngleCount) As Boolean
            checked(cIdxAngle0) = chk横ひも.Checked
            checked(cAngleCount) = chk側面.Checked

            Dim data As clsDataTables
            Dim calc As clsCalcHexagon
            If radうら.Checked Then
                data = _clsDataTables.LeftSideRightData()
                calc = New clsCalcHexagon(data, Me)
                If Not calc.CalcSize(CalcCategory.NewData, Nothing, Nothing) Then
                    Return  '先にOKならOKのはずだが
                End If
                '入れ替え
                checked(cIdxAngle60) = chk斜め120度.Checked
                checked(cIdxAngle120) = chk斜め60度.Checked
            Else
                'おもて
                data = _clsDataTables
                calc = _clsCalcHexagon
                'そのまま
                checked(cIdxAngle60) = chk斜め60度.Checked
                checked(cIdxAngle120) = chk斜め120度.Checked
            End If

            Cursor.Current = Cursors.WaitCursor
            _clsImageData = New clsImageData(_sFilePath)
            Dim ret As Boolean = calc.CalcImage(_clsImageData, checked, isBackFace)
            Cursor.Current = Cursors.Default

            If Not ret Then
                If Not String.IsNullOrWhiteSpace(calc.p_sメッセージ) Then
                    MessageBox.Show(calc.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
                Exit Sub
            End If
            picプレビュー.Image = System.Drawing.Image.FromFile(_clsImageData.GifFilePath)
        End If
    End Sub

    Private Sub chkひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk横ひも.CheckedChanged, chk斜め60度.CheckedChanged, chk斜め120度.CheckedChanged, chk側面.CheckedChanged
        If _clsImageData Is Nothing Then
            Return
        End If
        CalcImageData()
    End Sub

    '※斜めひも「同数」時のみ表示される
    Private Sub radおもてうら_CheckChanged(sender As Object, e As EventArgs) Handles radおもて.CheckedChanged ' radうら.CheckedChanged
        If _clsImageData Is Nothing Then
            Return
        End If
        CalcImageData()
    End Sub

    Private Sub reset_preview()
        radおもて.Checked = True
        chk横ひも.Checked = True
        chk斜め60度.Checked = True
        chk斜め120度.Checked = True
        chk側面.Checked = True
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
        If Not _clsImageData.ImgBrowserOpen(radうら.Checked) Then
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
            editAddParts.SetDgvColumnsVisible()
            expand斜め120度.SetDgvColumnsVisible()
            expand横ひも.SetDgvColumnsVisible()
            expand斜め60度.SetDgvColumnsVisible()
            editInsertBand.SetDgvColumnsVisible()
            bVisible = True
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", g_clsSelectBasics.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row目標寸法.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsDataTables.p_row底_縦横.dump())
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "DEBUG:{0}", _clsCalcHexagon.dump())
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
