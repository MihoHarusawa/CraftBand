Imports CraftBand.clsDataTables
Imports CraftBand.clsMasterTables
Imports CraftBand.ctrDataGridView
Imports CraftBand.ctrExpanding
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports System.Drawing
Imports System.Windows.Forms

Public Class ctrAdditionalBand

    'Panelを置き、各ControlはPanelにAnchorし、Panelをコードでリサイズする
    '※ユーザーコントロールとしてのサイズでは制御できない・表示がずれる


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

    'イベント
    Public Class AdditionalBandEventArgs
        Inherits EventArgs

        Public Property Row As tbl縦横展開Row = Nothing
        Public Property DataPropertyName As String

        Public Sub New(ByVal r As tbl縦横展開Row, Optional pname As String = Nothing)
            Me.Row = r
            DataPropertyName = pname
        End Sub
    End Class

    Public Event CellValueChanged As EventHandler(Of AdditionalBandEventArgs)

    '対象バンド・基本値の更新
    Private Sub setBasics()
        With g_clsSelectBasics
            Dim format As String = String.Format("N{0}", .p_unit設定時の寸法単位.DecimalPlaces)
            Me.f_dひも長1.DefaultCellStyle.Format = format
            Me.f_d出力ひも長1.DefaultCellStyle.Format = format
        End With
    End Sub

#Region "公開関数"

    <Flags()>
    Public Enum enumVisible
        i_None = 0
        i_幅 = &H1
        i_出力ひも長 = &H2
    End Enum

    'Load後に一度だけセットしてください
    Sub SetNames(ByVal formcaption As String, ByVal tabname As String,
                 ByVal plates As String, ByVal angles As String, ByVal centers As String)

        _FormCaption = formcaption
        _Profile_dgv差しひも.FormCaption = formcaption
        _TabPageName = tabname
        '
        f_i何本幅1.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅1.DisplayMember = "Display"
        f_i何本幅1.ValueMember = "Value"

        f_s色1.DataSource = g_clsSelectBasics.p_tblColor
        f_s色1.DisplayMember = "Display"
        f_s色1.ValueMember = "Value"

        '配置面の選択肢
        _PlateTable = New dstWork.tblEnumDataTable
        If Not String.IsNullOrWhiteSpace(plates) Then
            Dim ary As String() = plates.Split(",")
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
        If Not String.IsNullOrWhiteSpace(angles) Then
            Dim ary As String() = angles.Split(",")
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
        If Not String.IsNullOrWhiteSpace(centers) Then
            Dim ary As String() = centers.Split(",")
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

    '編集表示する
    Function ShowGrid(ByVal works As clsDataTables) As Boolean
        BindingSource差しひも.Sort = Nothing
        BindingSource差しひも.DataSource = Nothing
        If works Is Nothing Then
            Return False
        End If

        '非表示の間の変更を反映
        _i基本のひも幅 = works.p_row目標寸法.Value("f_i基本のひも幅")
        setBasics()

        BindingSource差しひも.DataSource = works.p_tbl差しひも
        BindingSource差しひも.Sort = "f_i番号"

        dgv差しひも.Refresh()

        Panel.Enabled = True
        Return True
    End Function

    '表示中で編集されていればデータ保存する
    Function Save(ByVal btype As enumひも種, ByVal works As clsDataTables) As Boolean
        If BindingSource差しひも.DataSource Is Nothing OrElse Not Panel.Enabled OrElse works Is Nothing Then
            Return False
        End If
        If Not _EditChanged Then
            Return False
        End If

        works.FromTmpTable(btype, BindingSource差しひも.DataSource)
        _EditChanged = False
        Return True
    End Function

    '編集完了、非表示にする
    Function HideGrid(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource差しひも.DataSource)

        BindingSource差しひも.Sort = Nothing
        BindingSource差しひも.DataSource = Nothing

        Panel.Enabled = False
        Return ret
    End Function

    'カラム幅を文字列に保存
    Public ReadOnly Property GetColumnWidthString() As String
        Get
            Return dgv差しひも.GetColumnWidthString()
        End Get
    End Property

    '文字列からカラム幅を復元
    Public Function SetColumnWidthFromString(ByVal csvStr As String) As Integer
        Return dgv差しひも.SetColumnWidthFromString(csvStr)
    End Function

    'Debug用に非表示カラムを表示
    Public Sub SetDgvColumnsVisible()
        For Each col As DataGridViewColumn In dgv差しひも.Columns
            If Not col.Visible Then
                col.Visible = True
            End If
        Next
    End Sub
#End Region


    Dim _isLoadingData As Boolean = True 'Designer.vb描画
    Dim _FormCaption As String
    Dim _TabPageName As String
    Dim _EditChanged As Boolean = False
    Dim _i基本のひも幅 As Integer

    'ドロップダウン選択肢
    Dim _PlateTable As dstWork.tblEnumDataTable
    Dim _AngleTable As dstWork.tblEnumDataTable
    Dim _CenterTable As dstWork.tblEnumDataTable




    Dim _Profile_dgv差しひも As New CDataGridViewProfile(
            (New tbl差しひもDataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )

    Private Sub ctrAdditionalBand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgv差しひも.SetProfile(_Profile_dgv差しひも)

        '※フォームのデザイン時にもLoadされますので、グローバル参照値は参照できない

        _isLoadingData = False 'Designer.vb描画完了
    End Sub

    Private Sub btn追加_差しひも_Click(sender As Object, e As EventArgs) Handles btn追加_差しひも.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
            Exit Sub
        End If

        Dim addNumber As Integer = clsDataTables.AddNumber(table)
        If addNumber < 0 Then
            '{0}追加用の番号がとれません。
            Dim msg As String = String.Format(My.Resources.CalcNoAddNumber, _TabPageName)
            MessageBox.Show(msg, _FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        'tbl差しひものレコード
        Dim row As tbl差しひもRow = table.Newtbl差しひもRow
        row.f_i番号 = addNumber
        If 2 < _i基本のひも幅 Then
            row.f_i何本幅 = _i基本のひも幅 \ 2
        Else
            row.f_i何本幅 = 1
        End If
        row.f_dひも長加算 = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d差しひも長加算初期値")
        dgv差しひも.NumberPositionsSelect(row.f_i番号)
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
        '編集対象のカラム
        If {"f_i配置面", "f_i角度", "f_i中心点", "f_i何本幅", "f_i開始位置", "f_i何本ごと", "f_dひも長加算"}.Contains(DataPropertyName) Then
            RaiseEvent CellValueChanged(Me, New AdditionalBandEventArgs(current.Row, DataPropertyName))
        End If
    End Sub

End Class
