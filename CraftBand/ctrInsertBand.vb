Imports CraftBand.clsDataTables
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports System.Drawing
Imports System.Windows.Forms

Public Class ctrInsertBand

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
    Public Class InsertBandEventArgs
        Inherits EventArgs

        Public Property Row As tbl差しひもRow = Nothing
        Public Property DataPropertyName As String

        Public Sub New(ByVal r As tbl差しひもRow, Optional pname As String = Nothing)
            Me.Row = r
            DataPropertyName = pname
        End Sub
    End Class

    'セルの変更通知
    Public Event CellValueChanged As EventHandler(Of InsertBandEventArgs)
    Public Event InnerPositionsSet As EventHandler(Of InsertBandEventArgs)
    '追加・削除・移動はコントロール内で完結


    '対象バンド・基本値の更新
    Private Sub setBasics()
        With g_clsSelectBasics
            Dim format As String = String.Format("N{0}", .p_unit設定時の寸法単位.DecimalPlaces)
            Me.f_dひも長1.DefaultCellStyle.Format = format
            Me.f_d出力ひも長1.DefaultCellStyle.Format = format
        End With
    End Sub

#Region "公開関数"

    'Load後に一度だけセットしてください
    Sub SetNames(ByVal formcaption As String, ByVal tabname As String,
                 ByVal plates As String, ByVal angles As String, ByVal centers As String, ByVal positions As String)

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
        If Not String.IsNullOrWhiteSpace(centers) Then
            _CenterTable = New dstWork.tblEnumDataTable
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
            f_i中心点1.DataSource = _CenterTable
            f_i中心点1.DisplayMember = "Display"
            f_i中心点1.ValueMember = "Value"
        Else
            f_i中心点1.DataSource = Nothing
            f_i中心点1.Visible = False
        End If

        '差し位置
        If Not String.IsNullOrWhiteSpace(positions) Then
            _PositionTable = New dstWork.tblEnumDataTable
            Dim ary As String() = positions.Split(",")
            Dim idx As Integer = 0
            For Each s As String In ary
                s = s.Trim
                Dim rc As dstWork.tblEnumRow = _PositionTable.NewRow
                rc.Display = s
                rc.Value = idx
                _PositionTable.Rows.Add(rc)
                idx += 1
            Next
            f_i差し位置1.DataSource = _PositionTable
            f_i差し位置1.DisplayMember = "Display"
            f_i差し位置1.ValueMember = "Value"
        Else
            f_i差し位置1.DataSource = Nothing
            f_i差し位置1.Visible = False
        End If

    End Sub

    '編集表示する
    Function ShowGrid(ByVal works As clsDataTables) As Boolean
        BindingSource差しひも.Sort = Nothing
        BindingSource差しひも.DataSource = Nothing
        If works Is Nothing Then
            Return False
        End If

        '非表示の間の変更を反映
        setBasics()

        Dim _i基本のひも幅 As Integer = works.p_row目標寸法.Value("f_i基本のひも幅")
        If 2 < _i基本のひも幅 Then
            _i何本幅 = _i基本のひも幅 \ 2
        Else
            _i何本幅 = 1
        End If
        If g_clsSelectBasics.p_d指定本幅(_i何本幅) < works.p_row底_縦横.Value("f_dひも間のすき間") Then
            _i中心点 = enum中心点.i_目の中央
        Else
            _i中心点 = enum中心点.i_ひも中央
        End If

        BindingSource差しひも.DataSource = works.p_tbl差しひも
        BindingSource差しひも.Sort = "f_i番号"

        dgv差しひも.Refresh()

        Panel.Enabled = True
        Return True
    End Function

    '表示中で編集されていればデータ保存する
    Function Save(ByVal works As clsDataTables) As Boolean
        If BindingSource差しひも.DataSource Is Nothing OrElse Not Panel.Enabled OrElse works Is Nothing Then
            Return False
        End If
        If Not _EditChanged Then
            Return False
        End If

        Dim ret As Boolean = works.CheckPoint(BindingSource差しひも.DataSource)
        _EditChanged = False
        Return ret
    End Function

    '編集完了、非表示にする
    Function HideGrid(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = Save(works)

        BindingSource差しひも.Sort = Nothing
        BindingSource差しひも.DataSource = Nothing

        Panel.Enabled = False
        Return ret
    End Function

    'enum文字列
    Public ReadOnly Property PlateString(ByVal plate As enum配置面) As String
        Get
            Try
                Return CType(_PlateTable.Rows(CType(plate, Integer)), dstWork.tblEnumRow).Display
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property AngleString(ByVal enumAngle As Integer) As String
        Get
            Try
                Return CType(_AngleTable.Rows(enumAngle), dstWork.tblEnumRow).Display
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property CenterString(ByVal center As enum中心点) As String
        Get
            Try
                If _CenterTable Is Nothing Then
                    Return Nothing
                End If
                Return CType(_CenterTable.Rows(CType(center, Integer)), dstWork.tblEnumRow).Display
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property PositionString(ByVal position As enum差し位置) As String
        Get
            Try
                If _PositionTable Is Nothing Then
                    Return Nothing
                End If
                Return CType(_PositionTable.Rows(CType(position, Integer)), dstWork.tblEnumRow).Display
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    'ヘッダー文字列
    Public ReadOnly Property text配置面() As String
        Get
            Return f_i配置面1.HeaderText
        End Get
    End Property

    Public ReadOnly Property text角度() As String
        Get
            Return f_i角度1.HeaderText
        End Get
    End Property

    Public ReadOnly Property text開始位置() As String
        Get
            Return f_i開始位置1.HeaderText
        End Get
    End Property

    Public ReadOnly Property text何本ごと() As String
        Get
            Return f_i何本ごと1.HeaderText
        End Get
    End Property

    Public ReadOnly Property text何本幅() As String
        Get
            Return f_i何本幅1.HeaderText
        End Get
    End Property

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

    'ドロップダウン選択肢
    Dim _PlateTable As dstWork.tblEnumDataTable = Nothing 'あり
    Dim _AngleTable As dstWork.tblEnumDataTable = Nothing 'あり
    Dim _CenterTable As dstWork.tblEnumDataTable = Nothing '任意
    Dim _PositionTable As dstWork.tblEnumDataTable = Nothing '任意

    '追加の初期値
    Dim _i何本幅 As Integer
    Dim _i中心点 As Integer

    Dim _EditChanged As Boolean = False


    Dim _Profile_dgv差しひも As New CDataGridViewProfile(
            (New tbl差しひもDataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )

    Private Sub ctrInsertBand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgv差しひも.SetProfile(_Profile_dgv差しひも)

        '※フォームのデザイン時にもLoadされますので、グローバル参照値は参照できない

        _isLoadingData = False 'Designer.vb描画完了
    End Sub

    Private Sub btn追加_Click(sender As Object, e As EventArgs) Handles btn追加.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        _EditChanged = True

        Dim addNumber As Integer = clsDataTables.AddNumber(table)
        If addNumber < 0 Then
            '{0}追加用の番号がとれません。
            Dim msg As String = String.Format(My.Resources.CalcNoAddNumber, _TabPageName)
            MessageBox.Show(msg, _FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        'tbl差しひものレコード
        Dim row As tbl差しひもRow = table.Newtbl差しひもRow '配置面なし
        row.f_i番号 = addNumber
        row.f_i何本幅 = _i何本幅
        row.f_i中心点 = _i中心点
        row.Setf_i同位置数Null()
        row.Setf_i同位置順Null()
        row.f_dひも長加算 = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d差しひも長加算初期値")

        table.Rows.Add(row)
        dgv差しひも.NumberPositionsSelect(row.f_i番号)
    End Sub

    Private Sub btn上へ_Click(sender As Object, e As EventArgs) Handles btn上へ.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If
        _EditChanged = True

        Dim nextup As Integer = clsDataTables.SmallerNumber(table, number)
        If nextup < 0 Then
            Exit Sub
        End If
        clsDataTables.SwapNumber(table, number, nextup)

        dgv差しひも.NumberPositionsSelect(nextup)
    End Sub

    Private Sub btn下へ_Click(sender As Object, e As EventArgs) Handles btn下へ.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If
        _EditChanged = True

        Dim nextdown As Integer = clsDataTables.LargerNumber(table, number)
        If nextdown < 0 Then
            Exit Sub
        End If
        clsDataTables.SwapNumber(table, number, nextdown)

        dgv差しひも.NumberPositionsSelect(nextdown)
    End Sub

    Private Sub btn削除_Click(sender As Object, e As EventArgs) Handles btn削除.Click
        Dim table As tbl差しひもDataTable = Nothing
        Dim number As Integer = -1
        If Not dgv差しひも.GetTableAndNumber(table, number) Then
            Exit Sub
        End If
        If number < 0 Then
            Exit Sub
        End If
        _EditChanged = True

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
        _EditChanged = True

        Dim row As tbl差しひもRow = current.Row
        If IsInvalidRow(row) Then
            '無効
            row.f_b有効区分 = False
            row.Setf_iひも本数Null()
            row.Setf_dひも長Null()
            row.Setf_d出力ひも長Null()
            Exit Sub
        End If

        Dim DataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv差しひも_CellValueChanged({1},{2}){3}", Now, DataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        '編集対象のカラム
        If {"f_i配置面", "f_i角度", "f_i中心点", "f_i何本幅", "f_i開始位置", "f_i何本ごと", "f_dひも長加算", "f_i同位置数", "f_i同位置順", "f_i差し位置"}.Contains(DataPropertyName) Then
            RaiseEvent CellValueChanged(Me, New InsertBandEventArgs(row, DataPropertyName))
        End If
    End Sub

    Private Function IsInvalidRow(ByVal row As tbl差しひもRow) As Boolean
        If row Is Nothing Then
            Return True 'InValid
        End If

        '開始位置は1以上、何本ごとはゼロ以上
        If row.f_i開始位置 < 1 Then
            row.f_s無効理由 = text開始位置()
            Return True '無効
        End If
        If row.f_i何本ごと < 0 Then
            row.f_s無効理由 = text何本ごと()
            Return True '無効
        End If

        '選択が有効
        If row.f_i配置面 = enum配置面.i_なし Then
            row.f_s無効理由 = text配置面()
            Return True '無効
        End If

        '同位置
        Dim i同位置数 As Integer = IIf(row.Isf_i同位置数Null(), 0, row.f_i同位置数)
        Dim i同位置順 As Integer = IIf(row.Isf_i同位置順Null(), 0, row.f_i同位置順)
        If i同位置数 < 0 OrElse (i同位置数 = 0 AndAlso 0 < i同位置順) Then
            row.f_s無効理由 = f_i同位置数1.HeaderText
            Return True '無効
        End If
        If i同位置順 < 0 OrElse (i同位置順 = 0 AndAlso 0 < i同位置数) Then
            row.f_s無効理由 = f_i同位置順1.HeaderText
            Return True '無効
        End If
        If 0 < i同位置数 AndAlso (i同位置順 = 0 OrElse i同位置数 < i同位置順) Then
            row.f_s無効理由 = f_i同位置数1.HeaderText
            Return True '無効
        End If

        Return False 'メイン側で追加チェック
    End Function

    '配置面・角度・[中心点]・開始位置・何本ごと・[差し位置]　が同じ数
    Private Sub btn同位置_Click(sender As Object, e As EventArgs) Handles btn同位置.Click
        Dim table As tbl差しひもDataTable = BindingSource差しひも.DataSource
        If table Is Nothing OrElse table.Rows.Count = 0 Then
            Exit Sub
        End If

        Dim match_key_count As New Dictionary(Of String, Integer)
        For Each row As tbl差しひもRow In table.Rows
            row.Setf_i同位置数Null()
            row.Setf_i同位置順Null()
            Dim mkey As String = match_key(row)
            If match_key_count.ContainsKey(mkey) Then
                match_key_count(mkey) += 1
            Else
                match_key_count(mkey) = 1
            End If
        Next
        For Each key As String In match_key_count.Keys
            If 1 < match_key_count(key) Then
                Dim odr As Integer = 1
                For Each row As tbl差しひもRow In table.Rows
                    If row.f_i配置面 = enum配置面.i_なし Then
                        Continue For
                    End If
                    If match_key(row) = key Then
                        row.f_i同位置数 = match_key_count(key)
                        row.f_i同位置順 = odr
                        odr += 1
                    End If
                Next
            End If
        Next
        RaiseEvent InnerPositionsSet(Me, New InsertBandEventArgs(Nothing, Nothing))
    End Sub

    Private Function match_key(ByVal row As tbl差しひもRow) As String
        If row Is Nothing Then
            Return ""
        End If

        Dim i中心点 As Integer = IIf(_CenterTable Is Nothing, 0, row.f_i中心点)
        Dim i差し位置 As Integer = IIf(_PositionTable Is Nothing, 0, row.f_i差し位置)
        Return String.Format("{0}:{1}:{2}:{3}:{4}:{5}",
                             row.f_i配置面, row.f_i角度, i中心点, row.f_i開始位置, row.f_i何本ごと, i差し位置)
    End Function

End Class
