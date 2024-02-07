Imports CraftBand.clsMasterTables
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstDataTables
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox

Public Class ctrAddParts

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

    'エラー発生(何等かの操作による)
    Public Event AddPartsError As EventHandler
    '※追加品の編集は他のサイズには影響しないので、エラーのみ通知する


    Dim _isLoadingData As Boolean = True 'Designer.vb描画
    Dim _i基本のひも幅 As Integer '目標寸法
    Dim _FormCaption As String
    Dim _TabPageName As String




    '対象バンド・基本値の更新
    Private Sub setBasics()
        With g_clsSelectBasics
            lbl長さ_単位.Text = .p_unit設定時の寸法単位.Str
            nud長さ.DecimalPlaces = .p_unit設定時の寸法単位.DecimalPlaces
            Me.f_dひも長3.DefaultCellStyle.Format = String.Format("N{0}", .p_unit設定時の寸法単位.DecimalPlaces)
        End With
    End Sub

    '付属品の変更
    Private Sub setOptions()
        cmb付属品名.Items.Clear()
        cmb付属品名.Items.AddRange(g_clsMasterTables.GetOptionNames())
    End Sub


#Region "公開関数"

    'Load後に一度だけセットしてください
    Sub SetNames(ByVal formcaption As String, ByVal tabname As String)
        _FormCaption = formcaption
        _Profile_追加品.FormCaption = formcaption
        _TabPageName = tabname
        '
        f_i何本幅3.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅3.DisplayMember = "Display"
        f_i何本幅3.ValueMember = "Value"

        f_s色3.DataSource = g_clsSelectBasics.p_tblColor
        f_s色3.DisplayMember = "Display"
        f_s色3.ValueMember = "Value"
    End Sub

    '編集表示する
    Function ShowGrid(ByVal works As clsDataTables) As Boolean
        BindingSource追加品.Sort = Nothing
        BindingSource追加品.DataSource = Nothing
        If works Is Nothing Then
            Return False
        End If


        '非表示の間の変更を反映
        _i基本のひも幅 = works.p_row目標寸法.Value("f_i基本のひも幅")
        setOptions()
        setBasics()
        _Calc.setData(works)

        BindingSource追加品.DataSource = works.p_tbl追加品
        BindingSource追加品.Sort = "f_i番号 , f_iひも番号"

        dgv追加品.Refresh()

        Panel.Enabled = True
        Return True
    End Function

    '編集完了、非表示にする
    Function HideGrid(ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = works.CheckPoint(BindingSource追加品.DataSource)

        BindingSource追加品.Sort = Nothing
        BindingSource追加品.DataSource = Nothing

        Panel.Enabled = False
        Return ret
    End Function

    '追加品テーブルをチェックする
    '戻り値: OK:Nothing  NG:エラー文字列
    Function CheckError(ByVal works As clsDataTables) As String
        _Calc.setData(works)
        If Not _Calc.recalc_追加品() Then
            Return _Calc.p_sメッセージ
        End If
        Return Nothing
    End Function


    'カラム幅を文字列に保存
    Public ReadOnly Property GetColumnWidthString() As String
        Get
            Return dgv追加品.GetColumnWidthString()
        End Get
    End Property

    '文字列からカラム幅を復元
    Public Function SetColumnWidthFromString(ByVal csvStr As String) As Integer
        Return dgv追加品.SetColumnWidthFromString(csvStr)
    End Function

    'Debug用に非表示カラムを表示
    Public Sub SetDgvColumnsVisible()
        For Each col As DataGridViewColumn In dgv追加品.Columns
            If Not col.Visible Then
                col.Visible = True
            End If
        Next
    End Sub

#End Region



    Dim _Calc As New Calc(Me)
    Dim _Profile_追加品 As New CDataGridViewProfile(
            (New tbl追加品DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow Or enumAction._RowHeight_iひも番号
            )

    Private Sub ctrAddParts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgv追加品.SetProfile(_Profile_追加品)

        '※フォームのデザイン時にもLoadされますので、グローバル参照値は参照できない

        _isLoadingData = False 'Designer.vb描画完了
    End Sub


    Private Sub btn追加_追加品_Click(sender As Object, e As EventArgs) Handles btn追加_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
        If Not dgv追加品.GetTableAndNumber(table, number) Then
            Exit Sub
        End If

        Dim row As tbl追加品Row = Nothing
        If _Calc.add_追加品(
            cmb付属品名.Text, _i基本のひも幅, nud長さ.Value, nud点数.Value,
            row) Then

            dgv追加品.NumberPositionsSelect(row.f_i番号)
            If Not _Calc.calc_追加品(row, "f_i点数") Then
                RaiseEvent AddPartsError(Me, EventArgs.Empty)
            End If

        Else
            MessageBox.Show(_Calc.p_sメッセージ, _FormCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btn上へ_追加品_Click(sender As Object, e As EventArgs) Handles btn上へ_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
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

        dgv追加品.NumberPositionsSelect(nextup)
    End Sub

    Private Sub btn下へ_追加品_Click(sender As Object, e As EventArgs) Handles btn下へ_追加品.Click
        Dim table As tbl追加品DataTable = Nothing
        Dim number As Integer = -1
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
        If Not _Calc.calc_追加品(Nothing, Nothing) Then
            RaiseEvent AddPartsError(Me, EventArgs.Empty)
        End If
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
        If _Calc.IsDataPropertyName追加品(DataPropertyName) Then
            If Not _Calc.calc_追加品(current.Row, DataPropertyName) Then
                RaiseEvent AddPartsError(Me, EventArgs.Empty)
            End If
        End If
    End Sub



    'データ保持とクラス関数呼び出し
    Private Class Calc

        Dim _Parent As ctrAddParts
        Dim _Data As clsDataTables

        Property p_sメッセージ As String

        Sub New(ByVal parent As ctrAddParts)
            _Parent = parent
        End Sub

        Sub setData(data As clsDataTables)
            _Data = data
        End Sub

        Function add_追加品(ByVal nameselect As String,
                         ByVal i何本幅 As Integer, ByVal d長さ As Double, ByVal i点数 As Integer,
                         ByRef row As tbl追加品Row) As Boolean
            Dim table As tbl追加品DataTable = _Data.p_tbl追加品

            If String.IsNullOrWhiteSpace(nameselect) Then
                '{0}を指定してください。
                p_sメッセージ = String.Format(My.Resources.CalcNoSelect, _Parent.lbl付属品名.Text)
                Return False
            End If
            'tbl付属品Row
            Dim grpMst As New clsGroupDataRow(g_clsMasterTables.GetOptionRecordGroup(nameselect))
            If Not grpMst.IsValid Then
                '{0}'{1}'は登録されていません。
                p_sメッセージ = String.Format(My.Resources.CalcNoMaster, _Parent.lbl付属品名.Text, nameselect)
                Return False
            End If

            Dim addNumber As Integer = clsDataTables.AddNumber(table)
            If addNumber < 0 Then
                '{0}追加用の番号がとれません。
                p_sメッセージ = String.Format(My.Resources.CalcNoAddNumber, _Parent.lbl付属品名.Text)
                Return False
            End If

            'tbl付属品ぶんのレコード
            Dim groupRow As New clsGroupDataRow("f_iひも番号")
            For Each idx As Int16 In grpMst.Keys
                row = table.Newtbl追加品Row
                row.f_i番号 = addNumber
                row.f_iひも番号 = idx
                row.f_s付属品名 = nameselect
                row.f_i点数 = i点数

                groupRow.Add(row)
                table.Rows.Add(row)
            Next
            groupRow.SetNameIndexValue("f_s付属品ひも名", grpMst)
            groupRow.SetNameIndexValue("f_b巻きひも区分", grpMst)
            groupRow.SetNameIndexValue("f_dひも長加算", grpMst, "f_dひも長加算初期値")
            groupRow.SetNameIndexValue("f_sメモ", grpMst, "f_s備考")

            Dim first As Boolean = True
            For Each drow As clsDataRow In groupRow
                Dim mst As New clsOptionDataRow(grpMst.IndexDataRow(drow)) '存在チェック済
                drow.Value("f_i何本幅") = mst.GetFirstLane(i何本幅)
                drow.Value("f_d長さ") = mst.GetLength(first, d長さ)
                If first Then
                    first = False
                End If
            Next
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Option Add: {0}", groupRow.ToString)
            Return True
        End Function

        '更新処理が必要なフィールド名
        Dim _fields追加品() As String = {"f_i何本幅", "f_i点数", "f_d長さ"}
        Function IsDataPropertyName追加品(ByVal name As String) As Boolean
            Return _fields追加品.Contains(name)
        End Function

        Function calc_追加品(ByVal row As tbl追加品Row, ByVal dataPropertyName As String) As Boolean
            Dim ret As Boolean = True
            If row IsNot Nothing Then
                '追加もしくは更新
                Dim cond As String = String.Format("f_i番号 = {0}", row.f_i番号)
                Dim groupRow = New clsGroupDataRow(_Data.p_tbl追加品.Select(cond), "f_iひも番号")
                If dataPropertyName = "f_i点数" Then
                    Dim i点数 As Integer = row.f_i点数
                    groupRow.SetNameValue("f_i点数", i点数)
                End If
                ret = ret And set_groupRow追加品(groupRow)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Option Change: {0}", groupRow.ToString)
            Else
                '削除, 念のため他一式チェック
                ret = recalc_追加品()
            End If
            Return ret
        End Function

        Function recalc_追加品() As Boolean
            Dim res = (From row As tbl追加品Row In _Data.p_tbl追加品
                       Select Num = row.f_i番号
                       Order By Num).Distinct

            Dim ret As Boolean = True
            For Each num As Integer In res
                Dim cond As String = String.Format("f_i番号 = {0}", num)
                Dim groupRow = New clsGroupDataRow(_Data.p_tbl追加品.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")
                ret = ret And set_groupRow追加品(groupRow)
            Next

            Return ret
        End Function

        Private Function set_groupRow追加品(ByVal groupRow As clsGroupDataRow) As Boolean
            '点数は一致項目
            Dim i点数 As Integer = groupRow.GetNameValue("f_i点数")

            'tbl付属品Row
            Dim grpMst As clsGroupDataRow = g_clsMasterTables.GetOptionRecordGroup(groupRow.GetNameValue("f_s付属品名"))
            If Not grpMst.IsValid Then
                'なし
                groupRow.SetNameValue("f_dひも長", DBNull.Value)
                groupRow.SetNameValue("f_iひも本数", DBNull.Value)
                groupRow.SetNameValue("f_bError", True)
                '{0}の番号{1}で設定にない付属品名'{2}'(ひも番号{3})が参照されています。
                p_sメッセージ = String.Format(My.Resources.CalcNoMasterOption, _Parent._TabPageName, groupRow.GetNameValue("f_i番号"), groupRow.GetNameValue("f_s付属品名"), groupRow.GetNameValue("f_iひも番号"))
                Return False
            Else
                Dim ret As Boolean = True

                Dim i直前の何本幅 As Integer = 0
                For Each drow As clsDataRow In groupRow
                    Dim ok As Boolean = False
                    Dim idxrow As clsDataRow = grpMst.IndexDataRow(drow)
                    If idxrow IsNot Nothing Then
                        Dim mst As New clsOptionDataRow(idxrow)
                        If mst.IsValid Then
                            drow.Value("f_iひも本数") = i点数 * mst.Value("f_iひも数")

                            'ひも長
                            If drow.Value("f_b巻きひも区分") Then
                                drow.Value("f_dひも長") = mst.GetWindBandLength(drow.Value("f_d長さ"), drow.Value("f_i何本幅"), i直前の何本幅)
                            Else
                                drow.Value("f_dひも長") = mst.GetBandLength(drow.Value("f_d長さ"))
                            End If
                            ok = True
                        End If
                    End If
                    If Not ok Then
                        drow.Value("f_iひも本数") = DBNull.Value
                        drow.Value("f_dひも長") = DBNull.Value
                        drow.Value("f_bError") = True
                        '{0}の番号{1}で設定にない付属品名'{2}'(ひも番号{3})が参照されています。
                        p_sメッセージ = String.Format(My.Resources.CalcNoMasterOption, _Parent._TabPageName, drow.Value("f_i番号"), drow.Value("f_s付属品名"), drow.Value("f_iひも番号"))
                        ret = False
                    End If
                    i直前の何本幅 = drow.Value("f_i何本幅")
                Next

                Return ret
            End If

        End Function

    End Class

End Class
