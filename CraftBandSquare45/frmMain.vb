
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsUpDown
Imports CraftBand.ctrAddParts
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


#Region "基本的な画面処理"

    Dim _Profile_dgv縁の始末 As New CDataGridViewProfile(
            (New tbl側面DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow Or enumAction._RowHeight_iひも番号
            )
    Dim _Profile_dgv折りカラー As New CDataGridViewProfile(
            (New dstWork.tblOriColorDataTable),
            Nothing,
             enumAction._BackColorReadOnlyYellow
            )

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _Profile_dgv縁の始末.FormCaption = Me.Text
        dgv縁の始末.SetProfile(_Profile_dgv縁の始末)
        _Profile_dgv折りカラー.FormCaption = Me.Text
        dgv折りカラー.SetProfile(_Profile_dgv折りカラー)

        editAddParts.SetNames(Me.Text, tpage追加品.Text)
        setAddPartsRefNames()
        editUpDown.FormCaption = Me.Text
        editUpDown.IsSquare45 = True

        expand横ひも.SetNames(Me.Text, tpage横ひも.Text, True, ctrExpanding.enumVisible.i_幅 Or ctrExpanding.enumVisible.i_出力ひも長, My.Resources.CaptionExpand8To2, My.Resources.CaptionExpand4To6)
        expand縦ひも.SetNames(Me.Text, tpage縦ひも.Text, True, ctrExpanding.enumVisible.i_幅 Or ctrExpanding.enumVisible.i_出力ひも長, My.Resources.CaptionExpand4To6, My.Resources.CaptionExpand8To2)

#If DEBUG Then
        btnDEBUG.Visible = (clsLog.LogLevel.Trouble <= g_clsLog.Level)
#Else
        btnDEBUG.Visible = (clsLog.LogLevel.Debug <= g_clsLog.Level)
#End If
        frmSaveTemporarily.ClearSaved()

        '引数/前回ファイルはそのまま開く(#102 /N はNot Exists)
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
        f_i何本幅.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅.DisplayMember = "Display"
        f_i何本幅.ValueMember = "Value"

        f_s色.DataSource = g_clsSelectBasics.p_tblColor
        f_s色.DisplayMember = "Display"
        f_s色.ValueMember = "Value"
        '
        setBasics(g_clsSelectBasics.p_s対象バンドの種類名 = _clsDataTables.p_row目標寸法.Value("f_sバンドの種類名")) '異なる場合は DispTables内
        setPattern()

        initColorChange() '色変更の初期化
        initColorRepeat() '色と幅の繰り返しの初期化(#51)

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
            Me.expand横ひも.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridTate
            Me.expand縦ひも.SetColumnWidthFromString(colwid)
            colwid = My.Settings.frmMainGridOriColor
            Me.dgv折りカラー.SetColumnWidthFromString(colwid)
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

        My.Settings.frmMainGridOriColor = Me.dgv折りカラー.GetColumnWidthString()
        My.Settings.frmMainGridSide = Me.dgv縁の始末.GetColumnWidthString()
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
            Me.f_d高さ.DefaultCellStyle.Format = format
            Me.f_d垂直ひも長.DefaultCellStyle.Format = format
            Me.f_d周長.DefaultCellStyle.Format = format
            Me.f_dひも長.DefaultCellStyle.Format = format
            Me.f_d連続ひも長.DefaultCellStyle.Format = format

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

        If {CalcCategory.NewData, CalcCategory.Target, CalcCategory.Target_Band}.Contains(category) Then
            Save目標寸法(_clsDataTables.p_row目標寸法)
        End If
        If {CalcCategory.NewData, CalcCategory.Square_Add, CalcCategory.Square_TateYokoGap, CalcCategory.Square_Vert, CalcCategory.Square_Expand, CalcCategory.Target_Band}.Contains(category) Then
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
            Case tpage四角数.Name
                '
            Case tpage縁の始末.Name
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
                Showプレビュー()
            Case tpageプレビュー2.Name
                Showプレビュー2()
            Case tpageひも上下.Name
                Showひも上下(works)
            Case tpage折りカラー.Name
                Show折りカラー()
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
        _Always = &H100 '#41
    End Enum
    Private Sub ShowDefaultTabControlPage(ByVal reason As enumReason)
        If _isLoadingData Then
            Exit Sub
        End If
        Dim needreset As Boolean = reason.HasFlag(enumReason._Always)
        If Not needreset AndAlso reason.HasFlag(enumReason._GridDropdown) Then
            If {tpage縁の始末.Name, tpage追加品.Name, tpage横ひも.Name, tpage縦ひも.Name, tpage折りカラー.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If Not needreset AndAlso reason.HasFlag(enumReason._Preview) Then
            If {tpageプレビュー.Name, tpageプレビュー2.Name, tpageひも上下.Name}.Contains(_CurrentTabControlName) Then
                needreset = True
            End If
        End If
        If Not needreset AndAlso reason.HasFlag(enumReason._Pattern) Then
            If {tpageプレビュー.Name, tpageプレビュー2.Name, tpage縁の始末.Name}.Contains(_CurrentTabControlName) Then
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
            Case tpage縁の始末.Name
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
                Hideプレビュー()
            Case tpageプレビュー2.Name
                Hideプレビュー2()
            Case tpageひも上下.Name
                Hideひも上下(_clsDataTables)
            Case tpage折りカラー.Name
                Hide折りカラー()
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


    Sub Disp四角数(ByVal row底_縦横 As clsDataRow)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp四角 {0}", row底_縦横.ToString)
        With row底_縦横
            chk縦横を展開する.Checked = .Value("f_b展開区分")
            chk折りカラー編み.Checked = .Value("f_b折りカラー区分")
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

            'ひも上下
            chk1回のみ.Checked = .Value("f_bひも上下1回区分") 'Null値はFalse
            Dim startheight As Integer
            If .IsNull("f_iひも上下の高さ数") Then
                'Ver1.8.2以前との互換
                If .Value("f_b展開区分") Then
                    startheight = 0
                Else
                    startheight = -1
                End If
            Else
                startheight = .Value("f_iひも上下の高さ数")
            End If
            nud開始高さ.Value = startheight
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
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Disp計算結果 {0}", calc.dump())
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
                ToolStripStatusLabel2.Text = .p_s警告
            Else
                ToolStripStatusLabel1.Text = "NG"
                ToolStripStatusLabel2.Text = .p_sメッセージ
            End If

            lbl本幅の差.Visible = .p_b本幅変更あり '#84
            txt先の三角形の本幅の差.Visible = False
            txt四辺形の本幅の差.Visible = False
            txt後の三角形の本幅の差.Visible = False
            If chk縦横を展開する.Checked Then
                If .p_i先の三角形の本幅の差 <> 0 Then
                    txt先の三角形の本幅の差.Text = .p_i先の三角形の本幅の差
                    txt先の三角形の本幅の差.Visible = True
                    lbl本幅の差.Visible = True
                End If
                If .p_i四辺形の本幅の差 <> 0 Then
                    txt四辺形の本幅の差.Text = .p_i四辺形の本幅の差
                    txt四辺形の本幅の差.Visible = True
                    lbl本幅の差.Visible = True
                End If
                If .p_i後の三角形の本幅の差 <> 0 Then
                    txt後の三角形の本幅の差.Text = .p_i後の三角形の本幅の差
                    txt後の三角形の本幅の差.Visible = True
                    lbl本幅の差.Visible = True
                End If
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

        expand横ひも.Save(enumひも種.i_横 Or enumひも種.i_45度, works)
        expand縦ひも.Save(enumひも種.i_縦 Or enumひも種.i_315度, works)

        If _CurrentTabControlName = tpageひも上下.Name Then
            saveひも上下(works, False)
        End If
        If _CurrentTabControlName = tpage折りカラー.Name Then
            save折りカラー()
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
            .Value("f_b折りカラー区分") = chk折りカラー編み.Checked


            .Value("f_bひも上下1回区分") = chk1回のみ.Checked
            .Value("f_iひも上下の高さ数") = nud開始高さ.Value
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
        Dim cond_yoko As String = String.Format(fmt, CType(enumひも種.i_横, Integer), CType(enumひも種.i_45度 Or enumひも種.i_補強, Integer))
        Dim cond_tate As String = String.Format(fmt, CType(enumひも種.i_縦, Integer), CType(enumひも種.i_315度 Or enumひも種.i_補強, Integer))

        Dim _ColorChangeSettings() As CColorChangeSetting = {
           New CColorChangeSetting(tpage縁の始末.Text, enumDataID._tbl側面, Nothing, False),
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
            _clsCalcSquare45.prepare縦横展開DataTable()
        End If

        If 0 < ShowColorChangeForm(_clsDataTables) Then
            'recalc(CalcCategory.BandColor)
        End If
    End Sub

    '色と幅の繰り返しの初期化(#51)
    Private Function initColorRepeat() As Boolean

        Dim _ColorRepeatSettings() As CColorRepeatSetting = {
        New CColorRepeatSetting(tpage横ひも.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", CType(enumひも種.i_横, Integer)), "f_iひも番号 ASC", True, True),
        New CColorRepeatSetting(tpage縦ひも.Text, enumDataID._tbl縦横展開, String.Format("f_iひも種={0}", CType(enumひも種.i_縦, Integer)), "f_iひも番号 ASC", True, True)}

        Return CreateColorRepeatForm(_ColorRepeatSettings)
    End Function
    '色と幅の繰り返し
    Private Sub ToolStripMenuItemEditColorRepeat_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditColorRepeat.Click
        SaveTables(_clsDataTables)
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色変更

        If chk縦横を展開する.Checked Then
            _clsCalcSquare45.prepare縦横展開DataTable()
        End If

        If 0 < ShowColorRepeatForm(_clsDataTables) Then
            recalc(CalcCategory.BandColor)
        End If
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
        ShowDefaultTabControlPage(enumReason._Always)

        _clsDataTables.Clear()
        _clsDataTables.SetInitialValue()
        Save目標寸法(_clsDataTables.p_row目標寸法) '画面値

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
        ShowDefaultTabControlPage(enumReason._Always)
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

        Dim msg As String = Nothing
        If Not _clsCalcSquare45.p_b有効 Then
            msg = _clsCalcSquare45.p_sメッセージ
        ElseIf Not String.IsNullOrEmpty(_clsCalcSquare45.p_s警告) Then
            msg = _clsCalcSquare45.p_s警告
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
            'プレビュー以外は関係なし
            ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview) '色にかかわるため
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
        Dim dlg = New frmVersion
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
        radBefore.Checked = True
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

        Dim filename As String = mdlProcess.OpenDataFileDialog()
        If String.IsNullOrWhiteSpace(filename) Then
            Exit Sub
        End If
        If _clsDataTables.Load(filename) Then
            frmSaveTemporarily.ClearSaved()

            DispTables(_clsDataTables)
            _sFilePath = filename
            setStartEditing(True)
        Else
            MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    '名前をつけて保存
    Private Sub ToolStripMenuItemFileSaveAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileSaveAs.Click
        Dim filename As String = _sFilePath
        If String.IsNullOrWhiteSpace(filename) Then
            '目標寸法が空ならセット(#19)
            If nud横寸法.Value = 0 AndAlso nud縦寸法.Value = 0 AndAlso nud高さ寸法.Value = 0 AndAlso
                0 < _clsCalcSquare45.p_d四角ベース_横 AndAlso 0 < _clsCalcSquare45.p_d四角ベース_縦 Then
                nud横寸法.Value = _clsCalcSquare45.p_d四角ベース_横
                nud縦寸法.Value = _clsCalcSquare45.p_d四角ベース_縦
                nud高さ寸法.Value = 1 '空の解除
                nud高さ寸法.Value = _clsCalcSquare45.p_d四角ベース_高さ
            End If
            'Square45(本幅)横-縦-高さ
            Save目標寸法(_clsDataTables.p_row目標寸法)
            With _clsDataTables.p_row目標寸法
                filename = String.Format("{0}({1}){2}-{3}-{4}",
                GetShortExeName(g_enumExeName), .Value("f_i基本のひも幅"),
                Int(.Value("f_d横寸法")), Int(.Value("f_d縦寸法")), Int(.Value("f_d高さ寸法")))
            End With
        End If
        filename = mdlProcess.SaveDataFileDialog(filename)
        If String.IsNullOrWhiteSpace(filename) Then
            Exit Sub
        End If

        '保存名確定
        If String.IsNullOrEmpty(txtタイトル.Text) AndAlso String.IsNullOrEmpty(txt作成者.Text) Then
            txtタイトル.Text = IO.Path.GetFileNameWithoutExtension(filename)
            txt作成者.Text = Environment.UserName
        End If
        SaveTables(_clsDataTables)
        If _clsDataTables.Save(filename) Then
            _sFilePath = filename
            setStartEditing()

        Else
            MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    '保存
    Private Sub ToolStripMenuItemFileSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFileSave.Click
        Dim filename As String = _sFilePath
        If String.IsNullOrEmpty(filename) Then
            filename = mdlProcess.SaveDataFileDialog()
            If String.IsNullOrWhiteSpace(filename) Then
                Exit Sub
            End If
            _sFilePath = filename
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
                Dim filename As String = _sFilePath
                If String.IsNullOrEmpty(filename) Then
                    filename = mdlProcess.SaveDataFileDialog()
                    If String.IsNullOrWhiteSpace(filename) Then
                        Exit Sub
                    End If
                    _sFilePath = filename
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
            If changeFile(fname) Then
                Exit Sub
            End If
        Next
    End Sub

    Private Function changeFile(ByVal fpath As String) As Boolean
        If IsContinueEditing() Then
            Return True '入れ替えない
        End If
        ShowDefaultTabControlPage(enumReason._GridDropdown Or enumReason._Preview)
        If _clsDataTables.Load(fpath) Then
            frmSaveTemporarily.ClearSaved()

            DispTables(_clsDataTables)
            _sFilePath = fpath
            setStartEditing(True)
            Return True
        Else
            '対象外のファイル(他EXEデータも)
            MessageBox.Show(_clsDataTables.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If
    End Function

    'ファイル通知(#102)
    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = WM_COPYDATA Then
            Dim files() As String = mdlProcess.ReceiveFilePath(m)
            If files IsNot Nothing Then
                For Each fpath As String In files
                    If changeFile(fpath) Then
                        Exit For
                    End If
                Next
            End If

            Return
        End If

        MyBase.WndProc(m)
    End Sub

#End Region

#Region "コントロール変更イベント"
    '縦横の展開チェックボックス　※チェックは最初のタブにある
    Private Sub chk縦横を展開する_CheckedChanged(sender As Object, e As EventArgs) Handles chk縦横を展開する.CheckedChanged
        If chk縦横を展開する.Checked AndAlso nud開始高さ.Value < 0 Then
            nud開始高さ.Value = 0
        End If
        set底の縦横展開(chk縦横を展開する.Checked)
        recalc(CalcCategory.Square_Expand, sender)
    End Sub
    '
    Private Sub chk折りカラー編み_CheckedChanged(sender As Object, e As EventArgs) Handles chk折りカラー編み.CheckedChanged
        set底の縦横展開(chk縦横を展開する.Checked)
        recalc(CalcCategory.Square_Expand, sender)
    End Sub

    Private Sub nud基本のひも幅_ValueChanged(sender As Object, e As EventArgs) Handles nud基本のひも幅.ValueChanged
        If nud基本のひも幅.Value = 0 Then
            nud基本のひも幅.Value = g_clsSelectBasics.p_i本幅
        End If
        lbl基本のひも幅length.Text = New Length(g_clsSelectBasics.p_d指定本幅(nud基本のひも幅.Value)).TextWithUnit
        ShowDefaultTabControlPage(enumReason._Always)

        recalc(CalcCategory.Target_Band, sender)
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
        recalc(CalcCategory.Square_TateYokoGap, sender)
    End Sub

    Private Sub nudひも間のすき間_ValueChanged(sender As Object, e As EventArgs) Handles nudひも間のすき間.ValueChanged
        recalc(CalcCategory.Square_TateYokoGap, sender)
    End Sub

    Private Sub nud縦の四角数_ValueChanged(sender As Object, e As EventArgs) Handles nud縦の四角数.ValueChanged
        recalc(CalcCategory.Square_TateYokoGap, sender)
    End Sub

    Private Sub nud高さの四角数_ValueChanged(sender As Object, e As EventArgs) Handles nud高さの四角数.ValueChanged
        txt折り返しの高さ数.Text = nud高さの四角数.Value
        recalc(CalcCategory.Square_Vert, sender)
    End Sub

    Private Sub nudひも長係数_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長係数.ValueChanged
        recalc(CalcCategory.Square_Add, sender)
    End Sub

    Private Sub nudひも長加算_ValueChanged(sender As Object, e As EventArgs) Handles nudひも長加算.ValueChanged
        recalc(CalcCategory.Square_Add, sender)
    End Sub

    Private Sub chk縦の補強ひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk縦の補強ひも.CheckedChanged
        recalc(CalcCategory.Square_TateYokoGap, sender)
    End Sub

    Private Sub chk横の補強ひも_CheckedChanged(sender As Object, e As EventArgs) Handles chk横の補強ひも.CheckedChanged
        recalc(CalcCategory.Square_TateYokoGap, sender)
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
    '追加品の参照名 #63
    Sub setAddPartsRefNames()
        Dim names(12) As String

        '横・縦・高さ・周
        names(1) = lbl計算寸法横.Text
        names(2) = lbl計算寸法縦.Text
        names(3) = lbl計算寸法高さ.Text
        names(4) = lbl計算寸法周.Text

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
        editAddParts.ShowGrid(works, _clsCalcSquare45.getAddPartsRefValues)
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
        '
        chk折りカラー編み.Visible = isExband
        If isExband AndAlso chk折りカラー編み.Checked Then
            If Not TabControl.TabPages.Contains(tpage折りカラー) Then
                TabControl.TabPages.Add(tpage折りカラー)
            End If
            grp折り返し.Visible = True
        Else
            If TabControl.TabPages.Contains(tpage折りカラー) Then
                TabControl.TabPages.Remove(tpage折りカラー)
            End If
            grp折り返し.Visible = False
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
        Save四角数(works.p_row底_縦横)
        expand横ひも.PanelSize = tpage横ひも.Size
        expand横ひも.ShowGrid(_clsCalcSquare45.get横展開DataTable())
    End Sub

    Function Hide横ひも(ByVal works As clsDataTables) As Boolean
        Return expand横ひも.HideGrid(enumひも種.i_横 Or enumひも種.i_45度, works)
    End Function

    Sub Show縦ひも(ByVal works As clsDataTables)
        'タブ切り替えタイミングのため、表示は更新済
        Save四角数(works.p_row底_縦横)
        expand縦ひも.PanelSize = tpage縦ひも.Size
        expand縦ひも.ShowGrid(_clsCalcSquare45.get縦展開DataTable())
    End Sub

    Function Hide縦ひも(ByVal works As clsDataTables) As Boolean
        Return expand縦ひも.HideGrid(enumひも種.i_縦 Or enumひも種.i_315度, works)
    End Function


    Private Sub expand横ひも_AddButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.AddButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow Is Nothing Then
            Exit Sub
        End If
        Dim iひも番号 As Integer = currow.f_iひも番号
        Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
        '補強ひもは後ろに追加
        If iひも種.HasFlag(enumひも種.i_補強) Then
            iひも番号 = Val(txt横ひもの本数.Text) + 1
            iひも種 = enumひも種.i_横
        End If
        'その位置に追加
        currow = clsDataTables.Insert縦横展開Row(expand横ひも.DataSource, iひも種, iひも番号)
        _clsCalcSquare45.save横展開DataTable()

        nud横の四角数.Value = nud横の四角数.Value + 1 'with recalc

        expand横ひも.DataSource = _clsCalcSquare45.get横展開DataTable()
        expand横ひも.PositionSelect(currow)
    End Sub

    Private Sub expand横ひも_DeleteButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.DeleteButton
        Dim currow As tbl縦横展開Row = e.Row
        If currow Is Nothing Then
            Exit Sub
        End If
        Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
        '補強ひも
        If iひも種.HasFlag(enumひも種.i_補強) Then
            _clsCalcSquare45.save横展開DataTable()
            chk横の補強ひも.Checked = False
            Save四角数(_clsDataTables.p_row底_縦横)
            expand横ひも.DataSource = _clsCalcSquare45.get横展開DataTable()
            Exit Sub
        End If

        '横の四角数をマイナス
        currow = clsDataTables.Remove縦横展開Row(expand横ひも.DataSource, iひも種, currow.f_iひも番号)
        _clsCalcSquare45.save横展開DataTable()
        If 0 < nud横の四角数.Value Then
            nud横の四角数.Value = nud横の四角数.Value - 1 'with recalc
        ElseIf 0 < nud縦の四角数.Value Then
            nud縦の四角数.Value = nud縦の四角数.Value - 1
        End If
        expand横ひも.DataSource = _clsCalcSquare45.get横展開DataTable()
        expand横ひも.PositionSelect(currow)
    End Sub

    Private Sub expand横ひも_CellValueChanged(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.CellValueChanged
        '"f_i何本幅", "f_dひも長加算", "f_dひも長加算2", "f_s色"
        If e.Row Is Nothing OrElse String.IsNullOrEmpty(e.DataPropertyName) Then
            Exit Sub
        End If
        recalc(CalcCategory.Expand_Yoko, e.Row, e.DataPropertyName)
    End Sub

    Private Sub expand横ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand横ひも.ResetButton
        expand横ひも.DataSource = _clsCalcSquare45.get横展開DataTable(True)
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
            iひも番号 = Val(txt縦ひもの本数.Text) + 1
            iひも種 = enumひも種.i_縦
        End If
        'その位置に追加
        currow = clsDataTables.Insert縦横展開Row(expand縦ひも.DataSource, iひも種, iひも番号)
        _clsCalcSquare45.save縦展開DataTable()

        nud縦の四角数.Value = nud縦の四角数.Value + 1 'with recalc

        expand縦ひも.DataSource = _clsCalcSquare45.get縦展開DataTable()
        expand縦ひも.PositionSelect(currow)
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
        If currow Is Nothing Then
            Exit Sub
        End If
        Dim iひも種 As enumひも種 = CType(currow.f_iひも種, enumひも種)
        '補強ひも
        If iひも種.HasFlag(enumひも種.i_補強) Then
            _clsCalcSquare45.save縦展開DataTable()
            chk縦の補強ひも.Checked = False
            Save四角数(_clsDataTables.p_row底_縦横)
            expand縦ひも.DataSource = _clsCalcSquare45.get縦展開DataTable()
            Exit Sub
        End If

        '縦の四角数をマイナス
        currow = clsDataTables.Remove縦横展開Row(expand縦ひも.DataSource, iひも種, currow.f_iひも番号)
        _clsCalcSquare45.save縦展開DataTable()
        If 0 < nud縦の四角数.Value Then
            nud縦の四角数.Value = nud縦の四角数.Value - 1 'with recalc
        ElseIf 0 < nud横の四角数.Value Then
            nud横の四角数.Value = nud横の四角数.Value - 1
        End If
        expand縦ひも.DataSource = _clsCalcSquare45.get縦展開DataTable()
        expand縦ひも.PositionSelect(currow)
    End Sub

    Private Sub expand縦ひも_ResetButton(sender As Object, e As ctrExpanding.ExpandingEventArgs) Handles expand縦ひも.ResetButton
        expand縦ひも.DataSource = _clsCalcSquare45.get縦展開DataTable(True)
    End Sub

#End Region

#Region "折りカラー"'#96
    Sub Show折りカラー()
        txt折り返し数_外側.Text = ""
        txt折り返し数_内側.Text = ""
        SetReadonlyColumnVisibility(0)

        'タブ切り替えタイミングのため、表示は更新済
        If Not _clsCalcSquare45.p_is折りカラー処理 Then
            '折りカラー処理できません。高さをセットし本幅を一致させてください。
            MessageBox.Show(My.Resources.MsgCannotOricolor, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            BindingSource折りカラー.Sort = Nothing
            BindingSource折りカラー.DataSource = Nothing
            dgv折りカラー.Visible = False
            timer折りカラー.Enabled = False
            Exit Sub
        End If

        Dim oriColorTable As dstWork.tblOriColorDataTable = _clsCalcSquare45.GetOriColorTable()
        If oriColorTable IsNot Nothing Then
            Dim ary() As String = _clsCalcSquare45.CountCheckedOriColorTable(oriColorTable).Split(",")
            If 2 <= ary.Length Then
                txt折り返し数_外側.Text = ary(0)
                txt折り返し数_内側.Text = ary(1)
            End If

            BindingSource折りカラー.DataSource = oriColorTable
            BindingSource折りカラー.Sort = "f_index"

            dgv折りカラー.Refresh()
            dgv折りカラー.Visible = True
            timer折りカラー.Enabled = True
        End If
    End Sub

    Function Hide折りカラー() As Boolean
        Dim ret As Boolean = save折りカラー()
        dgv折りカラー.Visible = False
        BindingSource折りカラー.Sort = Nothing
        BindingSource折りカラー.DataSource = Nothing
        timer折りカラー.Enabled = False
        Return ret
    End Function

    Function save折りカラー() As Boolean
        Dim oriColorTable As dstWork.tblOriColorDataTable = TryCast(BindingSource折りカラー.DataSource, dstWork.tblOriColorDataTable)
        If oriColorTable IsNot Nothing Then
            Return _clsCalcSquare45.SaveOriColorTable(oriColorTable)
        End If
        Return False
    End Function

#Region "グリッドイベント"

    Dim _formatHeader As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Near}
    Dim _bgBrushR As New SolidBrush(Color.LightGreen)
    Dim _bgBrushL As New SolidBrush(Color.LightBlue)
    Const ColIndexDetail As Integer = 11

    'ヘッダーを2段表示
    Private Sub dgv折りカラー_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgv折りカラー.CellPainting
        ' ヘッダーセルのみを対象とする
        If e.RowIndex = -1 AndAlso e.ColumnIndex >= 0 Then
            Dim headerText As String = dgv折りカラー.Columns(e.ColumnIndex).HeaderText
            If headerText Is Nothing Then
                Exit Sub
            End If

            If headerText.Contains(":") Then
                e.Handled = True  ' 標準描画を抑制

                ' 文字を上下に分割
                Dim parts = headerText.Split(":"c)
                If 3 <= parts.Length Then
                    '背景色の塗りつぶし
                    If parts(0) = "R" Then
                        e.Graphics.FillRectangle(_bgBrushR, e.CellBounds)
                    ElseIf parts(0) = "L" Then
                        e.Graphics.FillRectangle(_bgBrushL, e.CellBounds)
                    End If
                    ' 上段描画
                    e.Graphics.DrawString(parts(1), e.CellStyle.Font, Brushes.Black,
                                      New RectangleF(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height / 2),
                                      _formatHeader)
                    ' 下段描画
                    _formatHeader.LineAlignment = StringAlignment.Far
                    e.Graphics.DrawString(parts(2), e.CellStyle.Font, Brushes.Black,
                                      New RectangleF(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height),
                                      _formatHeader)
                    ' 枠線を描画
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Border)
                End If
            End If
            ' ":"がない場合は、通常描画（何もしない）
        End If
    End Sub

    'チェックオン/オフ、色表示変更を伴う
    Private Sub dgv折りカラー_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv折りカラー.CellValueChanged
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        ' チェックボックスの列のみ処理
        If ColIndexDetail < e.ColumnIndex OrElse (dgv折りカラー.Columns(e.ColumnIndex).ValueType <> GetType(Boolean)) Then
            Return
        End If
        ' 編集結果をレコードに反映
        dgv折りカラー.EndEdit()

        ' 対応するDataRowを取得（DataTableから直接）
        Dim rowIndex As Integer = e.RowIndex
        Dim drv As DataRowView = CType(dgv折りカラー.Rows(rowIndex).DataBoundItem, DataRowView)
        Dim dataRow As DataRow = drv.Row
        Dim fieldName As String = dgv折りカラー.Columns(e.ColumnIndex).DataPropertyName

        If _clsCalcSquare45.OriColor_RecordChanged(fieldName, dataRow) Then
            'dgv折りカラー.Refresh()
            ' 変更があったのでタイマーをリセット
            timer折りカラー.Stop()
            timer折りカラー.Start()
        End If
    End Sub

    'チェック操作後即時更新
    Private Sub dgv折りカラー_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgv折りカラー.CurrentCellDirtyStateChanged
        If TypeOf dgv折りカラー.CurrentCell Is DataGridViewCheckBoxCell Then
            dgv折りカラー.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    'セル編集、Editのマーク
    Private Sub dgv折りカラー_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgv折りカラー.CellEndEdit
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        ' チェックボックスは処理済
        If (dgv折りカラー.Columns(e.ColumnIndex).ValueType = GetType(Boolean)) Then
            Return
        End If

        ' 対応するDataRowを取得（DataTableから直接）
        Dim rowIndex As Integer = e.RowIndex
        Dim drv As DataRowView = CType(dgv折りカラー.Rows(rowIndex).DataBoundItem, DataRowView)
        Dim dataRow As DataRow = drv.Row
        Dim fieldName As String = dgv折りカラー.Columns(e.ColumnIndex).DataPropertyName

        _clsCalcSquare45.OriColor_RecordChanged(fieldName, dataRow)
    End Sub

    'チェック数を遅延カウント
    Private Sub timer折り返しカウント_Tick(sender As Object, e As EventArgs) Handles timer折りカラー.Tick
        timer折りカラー.Stop()
        dgv折りカラー.Refresh()

        txt折り返し数_外側.Text = ""
        txt折り返し数_内側.Text = ""
        Dim oriColorTable As dstWork.tblOriColorDataTable = TryCast(BindingSource折りカラー.DataSource, dstWork.tblOriColorDataTable)
        If oriColorTable IsNot Nothing Then
            Dim ary() As String = _clsCalcSquare45.CountCheckedOriColorTable(oriColorTable).Split(",")
            If 2 <= ary.Length Then
                txt折り返し数_外側.Text = ary(0)
                txt折り返し数_内側.Text = ary(1)
            End If
        End If
    End Sub

    '入れ替えチェックの背景色(#99)
    Private Sub dgv折りカラー_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgv折りカラー.CellFormatting
        If e.ColumnIndex > ColIndexDetail Then
            Exit Sub
        End If
        If TypeOf dgv折りカラー.Columns(e.ColumnIndex) IsNot DataGridViewCheckBoxColumn Then
            Exit Sub
        End If

        ' 行に対応付けられたDataRowを取得
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        Dim dr As DataRowView = DirectCast(dgv.Rows(e.RowIndex).DataBoundItem, DataRowView)
        If dr Is Nothing Then
            ' DataRowが取得できない場合は処理をスキップ
            Exit Sub
        End If

        '同色は折りカラーでは入れ替え不可
        If Not IsColorChangeable(dr.Row) Then
            e.CellStyle.BackColor = Color.LightGray
        Else
            ' 条件を満たさない場合、デフォルトの色に戻す
            e.CellStyle.BackColor = dgv.DefaultCellStyle.BackColor
            e.CellStyle.ForeColor = dgv.DefaultCellStyle.ForeColor
        End If
    End Sub

    '折りカラーによる入れ替え可(#99)
    Private Function IsColorChangeable(ByVal r As DataRow) As Boolean
        Return (r.Field(Of String)("f_s色_45") <> r.Field(Of String)("f_s色_135"))
    End Function

#End Region

    'クリアボタン
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        If dgv折りカラー.Visible Then

            txt折り返し数_外側.Text = ""
            txt折り返し数_内側.Text = ""
            Dim oriColorTable As dstWork.tblOriColorDataTable = TryCast(BindingSource折りカラー.DataSource, dstWork.tblOriColorDataTable)
            If oriColorTable IsNot Nothing Then
                If _clsCalcSquare45.ClearOriColor(oriColorTable) Then
                    dgv折りカラー.Refresh()
                End If
            End If

        Else
            _clsCalcSquare45.ClearOriColor(Nothing) '全展開レコード
        End If
    End Sub

    '詳細表示ボタン、カラムの表示をトグル
    Private Sub btn詳細表示_Click(sender As Object, e As EventArgs) Handles btn詳細表示.Click
        If Not dgv折りカラー.Visible OrElse BindingSource折りカラー.DataSource Is Nothing Then
            Exit Sub
        End If
        SetReadonlyColumnVisibility(-1)
    End Sub

    '選択をONボタン
    Private Sub btn選択をON_折り_Click(sender As Object, e As EventArgs) Handles btn選択をON_折り.Click
        If 0 < dgv折りカラー.SelectedCells.Count Then
            SetSelectedCheckCells(True)
        End If
    End Sub

    '選択をOFFボタン
    Private Sub btn選択をOFF_折り_Click(sender As Object, e As EventArgs) Handles btn選択をOFF_折り.Click
        If 0 < dgv折りカラー.SelectedCells.Count Then
            SetSelectedCheckCells(False)
        End If
    End Sub

    '外側反転ボタン
    Private Sub btn外側反転_Click(sender As Object, e As EventArgs) Handles btn外側反転.Click
        If dgv折りカラー.Visible Then

            Dim oriColorTable As dstWork.tblOriColorDataTable = TryCast(BindingSource折りカラー.DataSource, dstWork.tblOriColorDataTable)
            If oriColorTable IsNot Nothing Then
                Dim idx As Integer = -1
                If _clsCalcSquare45.RevertOriColor(oriColorTable, idx) Then
                    dgv折りカラー.Refresh()
                    Dim ary() As String = _clsCalcSquare45.CountCheckedOriColorTable(oriColorTable).Split(",")
                    If 2 <= ary.Length Then
                        txt折り返し数_外側.Text = ary(0)
                        txt折り返し数_内側.Text = ary(1)
                    End If

                ElseIf 0 <= idx Then
                    'idx行を選択する
                    dgv折りカラー.ClearSelection()
                    dgv折りカラー.Rows(idx).Selected = True
                End If
            End If

        End If
    End Sub


    '詳細表示切替
    'ColIndexDetail より右のカラムを表示/非表示/トグルする　※Debug用カラムはその前に置くこと!
    'mode: -1: トグル（現在の表示状態を反転）/0: 非表示/1: 表示
    Private Sub SetReadonlyColumnVisibility(ByVal mode As Integer)
        If Not dgv折りカラー.Visible Then
            Exit Sub
        End If

        Dim newVisible As Boolean
        Select Case mode
            Case 0
                newVisible = False
            Case 1
                newVisible = True
            Case -1
                ' トグル用に、対象列の Visible 状態を基準に反転
                newVisible = Not dgv折りカラー.Columns(ColIndexDetail).Visible
            Case Else
                Exit Sub ' 無効な mode の場合は何もしない
        End Select

        ' 対象カラムの Visible を設定
        For i As Integer = ColIndexDetail To dgv折りカラー.Columns.Count - 1
            dgv折りカラー.Columns(i).Visible = newVisible
        Next
    End Sub

    '選択されたチェックボックスを引数の状態にする
    Private Sub SetSelectedCheckCells(ByVal bVal As Boolean)
        ' 編集確定しておく
        dgv折りカラー.EndEdit()

        For Each cell As DataGridViewCell In dgv折りカラー.SelectedCells
            ' 対象セルがBoolean型で、ReadOnlyでない場合のみ処理
            If TypeOf cell.Value Is Boolean AndAlso Not cell.ReadOnly Then
                cell.Value = bVal
            End If
        Next

        ' 編集反映
        dgv折りカラー.EndEdit()
        dgv折りカラー.Refresh()
    End Sub


#End Region

#Region "ひも上下"

    Sub Showひも上下(ByVal works As clsDataTables)
        Dim ret As Boolean =
        editUpDown.SetSquare45Basics(_clsCalcSquare45.p_i横の四角数,
                                    _clsCalcSquare45.p_i縦の四角数,
                                    _clsCalcSquare45.p_i高さの切上四角数) AndAlso
        editUpDown.ChangeSquare45EditHeight(nud開始高さ.Value, chk1回のみ.Checked)

        editUpDown.PanelSize = tpageひも上下.Size
        editUpDown.ShowGrid(works, enumTargetFace.Bottom)
        If Not ret Then
            '現在の値で編集することはできません。
            MessageBox.Show(My.Resources.MsgCannotEdit, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Function Hideひも上下(ByVal works As clsDataTables) As Boolean
        _clsDataTables.p_row底_縦横.Value("f_bひも上下1回区分") = chk1回のみ.Checked
        _clsDataTables.p_row底_縦横.Value("f_iひも上下の高さ数") = nud開始高さ.Value
        Return editUpDown.HideGrid(works)
    End Function

    Function saveひも上下(ByVal works As clsDataTables, ByVal isMsg As Boolean) As Boolean
        '開始高さ・1回のみは、Save四角数で保存済
        Return editUpDown.Save(works, isMsg)
    End Function

    Private Sub tpageひも上下_Resize(sender As Object, e As EventArgs) Handles tpageひも上下.Resize
        editUpDown.PanelSize = tpageひも上下.Size
    End Sub

    Private Sub nud開始高さ_ValueChanged(sender As Object, e As EventArgs) Handles nud開始高さ.ValueChanged
        If _CurrentTabControlName = tpageひも上下.Name Then
            If Not editUpDown.ChangeSquare45EditHeight(nud開始高さ.Value, chk1回のみ.Checked) Then
                '現在の値で編集することはできません。
                MessageBox.Show(My.Resources.MsgCannotEdit, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    Private Sub chk1回のみ_Click(sender As Object, e As EventArgs) Handles chk1回のみ.CheckedChanged
        If _CurrentTabControlName = tpageひも上下.Name Then
            If Not editUpDown.ChangeSquare45EditHeight(nud開始高さ.Value, chk1回のみ.Checked) Then
                '現在の値で編集することはできません。
                MessageBox.Show(My.Resources.MsgCannotEdit, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    Private Sub btn合わせる_Click(sender As Object, e As EventArgs) Handles btn合わせる.Click
        Dim chkPrv As Boolean = chk1回のみ.Checked
        chk1回のみ.Checked = True
        Dim takasa As Integer = nud開始高さ.Value
        If takasa < 0 Then
            nud開始高さ.Value = 0
            takasa = 0
        Else
            If chkPrv AndAlso editUpDown.Is底位置表示 Then
                '{0}に基づき再度初期化してよろしいですか？
                If MessageBox.Show(String.Format(My.Resources.AskInitializeAgain, grp縦横の四角.Text),
                               Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
                    Exit Sub
                End If
            End If
        End If

        Dim updown As clsUpDown = _clsCalcSquare45.fitsizeひも上下(chk横の辺.Checked, nud垂直に.Value, nud底に.Value, takasa)
        If updown Is Nothing OrElse Not updown.IsValid(False) Then
            '現在の値では合わせることはできません。
            MessageBox.Show(My.Resources.MsgCannotSuit & _clsCalcSquare45.p_sメッセージ, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        editUpDown.Replace(updown)
    End Sub

#End Region

#Region "プレビュー"
    Dim _clsImageData As clsImageData
    Private Sub Showプレビュー()
        picプレビュー.Image = Nothing
        _clsImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcSquare45.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcSquare45) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If

        Dim data As clsDataTables = _clsDataTables
        Dim calc As clsCalcSquare45 = _clsCalcSquare45
        Dim isBackFace As Boolean = False
        If radうら.Checked Then
            data = _clsDataTables.LeftSideRightData()
            calc = New clsCalcSquare45(data, Me)
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
    Dim _clsModelImageData As clsModelSquare45
    Private Sub Showプレビュー2()
        picプレビュー2.Image = Nothing
        _clsModelImageData = Nothing

        SaveTables(_clsDataTables)
        Dim ret As Boolean = _clsCalcSquare45.CalcSize(CalcCategory.NewData, Nothing, Nothing)
        Disp計算結果(_clsCalcSquare45) 'NGはToolStripに表示
        If Not ret Then
            Return
        End If
        grp折り返し.Enabled = _clsCalcSquare45.p_is折りカラー処理

        Cursor.Current = Cursors.WaitCursor
        _clsModelImageData = New clsModelSquare45(_clsCalcSquare45, _sFilePath)
        ret = _clsModelImageData.CalcModel(radAfter.Checked)
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

    '#96 Before/After 変更
    Private Sub radBeforeAfter_CheckedChanged(sender As Object, e As EventArgs) Handles radBefore.CheckedChanged, radAfter.CheckedChanged
        'プレビュー2処理後に呼び出される
        If _clsModelImageData Is Nothing Then
            Return
        End If
        If Not _clsCalcSquare45.p_is折りカラー処理 Then
            grp折り返し.Enabled = False
            Return
        End If
        '
        Cursor.Current = Cursors.WaitCursor
        Dim ret As Boolean = _clsModelImageData.CalcModel(radAfter.Checked)
        Cursor.Current = Cursors.Default

        If Not ret AndAlso Not String.IsNullOrWhiteSpace(_clsModelImageData.LastError) Then
            MessageBox.Show(_clsModelImageData.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        picプレビュー2.Image = System.Drawing.Image.FromFile(_clsModelImageData.GifFilePath)
    End Sub

#End Region

#Region "DEBUG"
    Dim bVisible As Boolean = False
    Private Sub btnDEBUG_Click(sender As Object, e As EventArgs) Handles btnDEBUG.Click

        If Not bVisible Then
            setDgvColumnsVisible(dgv縁の始末)
            editAddParts.SetDgvColumnsVisible()
            expand横ひも.SetDgvColumnsVisible()
            expand縦ひも.SetDgvColumnsVisible()
            setDgvColumnsVisible(dgv折りカラー)
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
