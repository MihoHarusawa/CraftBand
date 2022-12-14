
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables

Public Class clsDataTables

    '縁のf_i番号値(1点のみ)
    Public Const cHemNumber As Integer = 999


    Dim _dstDataTables As dstDataTables
    Dim _ExeName As String


    'tbl目標寸法は1レコードのみ
    Public Property p_row目標寸法 As clsDataRow 'tbl目標寸法Row

    'tbl底_縦横は1レコードのみ
    Public Property p_row底_縦横 As clsDataRow 'tbl底_縦横Row


    'tbl側面
    Public ReadOnly Property p_tbl側面 As tbl側面DataTable
        Get
            Return _dstDataTables.Tables("tbl側面")
        End Get
    End Property

    'tbl底_楕円
    Public ReadOnly Property p_tbl底_楕円 As tbl底_楕円DataTable
        Get
            Return _dstDataTables.Tables("tbl底_楕円")
        End Get
    End Property

    'tbl追加品
    Public ReadOnly Property p_tbl追加品 As tbl追加品DataTable
        Get
            Return _dstDataTables.Tables("tbl追加品")
        End Get
    End Property



    Public Enum enum最上と最下の短いひも
        i_なし = 0
        i_同じ幅 = 1
        i_異なる幅 = 2
    End Enum



    Sub New(ByVal exeName As String)
        _ExeName = exeName
        _dstDataTables = New dstDataTables
        Clear()
    End Sub

    Sub New(ByVal ref As clsDataTables)
        _ExeName = ref._ExeName
        _dstDataTables = ref._dstDataTables.Copy
        '各、1レコード
        p_row目標寸法 = New clsDataRow(_dstDataTables.Tables("tbl目標寸法").Rows(0))
        p_row底_縦横 = New clsDataRow(_dstDataTables.Tables("tbl底_縦横").Rows(0))
    End Sub

    '空
    Public Sub Clear()
        _dstDataTables.Clear()

        p_row目標寸法 = Nothing
        p_row底_縦横 = Nothing

        p_row目標寸法 = New clsDataRow(_dstDataTables.Tables("tbl目標寸法").NewRow)
        _dstDataTables.Tables("tbl目標寸法").Rows.Add(p_row目標寸法.DataRow)

        p_row底_縦横 = New clsDataRow(_dstDataTables.Tables("tbl底_縦横").NewRow)
        _dstDataTables.Tables("tbl底_縦横").Rows.Add(p_row底_縦横.DataRow)

    End Sub

    '初期値
    Public Sub SetInitialValue()

        p_row目標寸法.SetDefaultAll()
        p_row目標寸法.Value("f_sバンドの種類名") = g_clsSelectBasics.p_s対象バンドの種類名
        p_row目標寸法.Value("f_i基本のひも幅") = g_clsSelectBasics.p_i本幅
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "SetDefaultValue {0}", p_row目標寸法.ToString)

        p_row底_縦横.SetDefaultAll()
        p_row底_縦横.Value("f_i長い横ひも") = g_clsSelectBasics.p_i本幅
        p_row底_縦横.Value("f_i短い横ひも") = g_clsSelectBasics.p_i本幅
        p_row底_縦横.Value("f_i縦ひも") = g_clsSelectBasics.p_i本幅
        p_row底_縦横.Value("f_d垂直ひも長加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d垂直ひも加算初期値")

        p_row底_縦横.Value("f_b楕円底個別設定") = False
        p_row底_縦横.Value("f_d楕円底円弧の半径加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底円弧の半径加算")
        p_row底_縦横.Value("f_d楕円底周の加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底周の加算")
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "SetDefaultValue {0}", p_row底_縦横.ToString)

    End Sub

    '現DataSetのバージョン
    Public ReadOnly Property FormatVersion As String
        Get
            Dim r As tbl目標寸法Row = _dstDataTables.Tables("tbl目標寸法").NewRow
            Return r.f_sバージョン
        End Get
    End Property

    Public Function Load(ByVal fpath As String) As Boolean
        If Not IO.File.Exists(fpath) Then
            Return False
        End If

        _dstDataTables.Clear()
        Try
            Dim readmode As System.Data.XmlReadMode = _dstDataTables.ReadXml(fpath, System.Data.XmlReadMode.IgnoreSchema)
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.Load", fpath)
            Return False
        End Try

        '各、1レコード
        If _dstDataTables.Tables("tbl目標寸法").Rows.Count = 0 Then
            _dstDataTables.Tables("tbl目標寸法").Rows.Add(_dstDataTables.Tables("tbl目標寸法").NewRow)
        End If
        If _dstDataTables.Tables("tbl底_縦横").Rows.Count = 0 Then
            _dstDataTables.Tables("tbl底_縦横").Rows.Add(_dstDataTables.Tables("tbl底_縦横").NewRow)
        End If

        p_row目標寸法 = New clsDataRow(_dstDataTables.Tables("tbl目標寸法").Rows(0))
        p_row底_縦横 = New clsDataRow(_dstDataTables.Tables("tbl底_縦横").Rows(0))

        If 0 <> String.Compare(p_row目標寸法.Value("f_sEXE名"), _ExeName, True) Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsDataTables.Load({0}) Exe in File'{1}' <> '{2}", fpath, p_row目標寸法.Value("f_sEXE名"), _ExeName)
        End If
        If 0 <> String.Compare(p_row目標寸法.Value("f_s単位"), g_clsSelectBasics.p_unit設定時の寸法単位.Str, True) Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsDataTables.Load({0}) Unit in File'{1}' <> '{2}", fpath, p_row目標寸法.Value("f_s単位"), g_clsSelectBasics.p_unit設定時の寸法単位.Str)
        End If

        Return True
    End Function

    Public Function Save(ByVal fpath As String) As Boolean
        Try
            'データ環境
            p_row目標寸法.Value("f_s単位") = g_clsSelectBasics.p_unit設定時の寸法単位.Str
            p_row目標寸法.Value("f_sEXE名") = _ExeName
            p_row目標寸法.Value("f_sバージョン") = FormatVersion
            '
            _dstDataTables.WriteXml(fpath, System.Data.XmlWriteMode.WriteSchema)
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.Save", fpath)
            Return False
        End Try
    End Function



    Public Sub ModifySelected()

        For Each row As tbl底_楕円Row In p_tbl底_楕円
            '何本幅
            If row.f_i何本幅 < 0 Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl底_楕円Row({0}).f_i何本幅={1} -> 1", row.f_i番号, row.f_i何本幅)
                row.f_i何本幅 = 1
            ElseIf g_clsSelectBasics.p_i本幅 < row.f_i何本幅 Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl底_楕円Row({0}).f_i何本幅={1} -> {2}", row.f_i番号, row.f_i何本幅, g_clsSelectBasics.p_i本幅)
                row.f_i何本幅 = g_clsSelectBasics.p_i本幅
            End If
            '色
            If Not g_clsSelectBasics.IsExistColor(row.f_s色) Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl底_楕円Row({0}).f_s色={1} -> ''", row.f_i番号, row.f_s色)
                row.f_s色 = ""
            End If
        Next

        For Each row As tbl側面Row In p_tbl側面
            '何本幅
            If row.f_i何本幅 < 0 Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl側面Row({0}).f_i何本幅={1} -> 1", row.f_i番号, row.f_i何本幅)
                row.f_i何本幅 = 1
            ElseIf g_clsSelectBasics.p_i本幅 < row.f_i何本幅 Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl側面Row({0}).f_i何本幅={1} -> {2}", row.f_i番号, row.f_i何本幅, g_clsSelectBasics.p_i本幅)
                row.f_i何本幅 = g_clsSelectBasics.p_i本幅
            End If
            '色
            If Not g_clsSelectBasics.IsExistColor(row.f_s色) Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl側面Row({0}).f_s色={1} -> ''", row.f_i番号, row.f_s色)
                row.f_s色 = ""
            End If
        Next

        For Each row As tbl追加品Row In p_tbl追加品
            '何本幅
            If row.f_i何本幅 < 0 Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl追加品Row({0}).f_i何本幅={1} -> 1", row.f_i番号, row.f_i何本幅)
                row.f_i何本幅 = 1
            ElseIf g_clsSelectBasics.p_i本幅 < row.f_i何本幅 Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl追加品Row({0}).f_i何本幅={1} -> {2}", row.f_i番号, row.f_i何本幅, g_clsSelectBasics.p_i本幅)
                row.f_i何本幅 = g_clsSelectBasics.p_i本幅
            End If
            '色
            If Not g_clsSelectBasics.IsExistColor(row.f_s色) Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "tbl追加品Row({0}).f_s色={1} -> ''", row.f_i番号, row.f_s色)
                row.f_s色 = ""
            End If
        Next
    End Sub


#Region "編集による更新状態"

    '編集開始時の参照値
    Dim _row目標寸法Start As clsDataRow 'tbl目標寸法Row
    Dim _row底_縦横Start As clsDataRow 'tbl底_縦横Row

    'テーブル更新状態
    Dim _b底_楕円Changed As Boolean = False
    Dim _b側面Changed As Boolean = False
    Dim _b追加品Changed As Boolean = False


    Public Sub ResetStartPoint()

        _dstDataTables.Tables("tbl目標寸法").AcceptChanges()
        _dstDataTables.Tables("tbl底_縦横").AcceptChanges()

        _row目標寸法Start = Nothing
        _row目標寸法Start = p_row目標寸法.Clone
        _row底_縦横Start = Nothing
        _row底_縦横Start = p_row底_縦横.Clone

        p_tbl底_楕円.AcceptChanges()
        p_tbl側面.AcceptChanges()
        p_tbl追加品.AcceptChanges()

        _b底_楕円Changed = False
        _b側面Changed = False
        _b追加品Changed = False
    End Sub

    Public Function IsChangedFromStartPoint()
        If _b側面Changed OrElse _b底_楕円Changed OrElse _b追加品Changed Then
            Return True
        End If
        If Not p_row目標寸法.Equals(_row目標寸法Start) Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Start {0}", _row目標寸法Start.ToString)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Change{0}", p_row目標寸法.ToString)
            Return True
        End If
        If Not p_row底_縦横.Equals(_row底_縦横Start) Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Start {0}", _row底_縦横Start.ToString)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "Change{0}", p_row底_縦横.ToString)
            Return True
        End If
        Return False
    End Function

    Public Function CheckPoint(ByVal table As DataTable) As Boolean
        If table Is Nothing OrElse table.GetChanges Is Nothing Then
            Return False
        End If
        If table.TableName = "tbl底_楕円" Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "tbl底_楕円 Changes={0}", table.GetChanges.Rows.Count)
            _b底_楕円Changed = True
        ElseIf table.TableName = "tbl側面" Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "tbl側面 Changes={0}", table.GetChanges.Rows.Count)
            _b側面Changed = True
        ElseIf table.TableName = "tbl追加品" Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "tbl追加品 Changes={0}", table.GetChanges.Rows.Count)
            _b追加品Changed = True
        Else
            Return False
        End If
        table.AcceptChanges()
        Return True
    End Function
#End Region

#Region "編集用Shared"
    Shared Function SwapNumber(ByVal table As DataTable, ByVal num1 As Integer, ByVal num2 As Integer)
        Dim tmp As Integer = 9999
        Try

            table.AsEnumerable.Where(Function(r) r("f_i番号") = num1).Select(Function(r)
                                                                               r("f_i番号") = tmp
                                                                               Return r
                                                                           End Function).ToList()
            table.AsEnumerable.Where(Function(r) r("f_i番号") = num2).Select(Function(r)
                                                                               r("f_i番号") = num1
                                                                               Return r
                                                                           End Function).ToList()
            table.AsEnumerable.Where(Function(r) r("f_i番号") = tmp).Select(Function(r)
                                                                              r("f_i番号") = num2
                                                                              Return r
                                                                          End Function).ToList()

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.SwapNumber")
            Return False
        End Try
        Return True
    End Function

    Shared Function RemoveNumberFromTable(ByVal table As DataTable, ByVal number As Integer) As Boolean
        Try
            Dim dels() As DataRow = table.Select(String.Format("f_i番号={0}", number))
            For Each del In dels
                table.Rows.Remove(del)
            Next

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.RemoveNumberFromTable")
            Return False
        End Try
        Return True
    End Function

    '縁でない最大の番号、なければ-1
    Shared Function LastNumber(ByVal table As DataTable) As Integer
        Try
            Dim obj As Object = table.Compute("MAX(f_i番号)",
                                              String.Format("f_i番号 < {0}", cHemNumber))
            If IsDBNull(obj) OrElse obj < 1 Then
                Return -1
            End If
            Return obj

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.LastNumber")
            Return -1
        End Try
    End Function

    '縁でない次の番号
    Shared Function AddNumber(ByVal table As DataTable) As Integer
        Dim lastnum As Integer = LastNumber(table)
        If lastnum < 1 Then
            Return 1
        End If
        Return lastnum + 1
    End Function

    'numberの前の番号、なければ-1
    Shared Function SmallerNumber(ByVal table As DataTable, ByVal number As Integer) As Integer
        Try
            Dim obj As Object = table.Compute("MAX(f_i番号)",
                                              String.Format("f_i番号 < {0}", number))
            If IsDBNull(obj) OrElse obj < 1 Then
                Return -1
            End If
            Return obj

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.SmallerNumber")
            Return False
        End Try
    End Function

    'numberの次の縁でない番号、なければ-1
    Shared Function LargerNumber(ByVal table As DataTable, ByVal number As Integer) As Integer
        Try
            Dim obj As Object = table.Compute("MIN(f_i番号)",
                                              String.Format("{0} < f_i番号 AND f_i番号 < {1}", number, cHemNumber))
            If IsDBNull(obj) OrElse obj < 1 Then
                Return -1
            End If
            Return obj

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.LargerNumber")
            Return False
        End Try
    End Function

    'numberのレコード数
    Shared Function NumberCount(ByVal table As DataTable, ByVal number As Integer) As Integer
        Try
            Dim obj As Object = table.Compute("COUNT(f_i番号)",
                                               String.Format("f_i番号 = {0}", number))
            If IsDBNull(obj) OrElse obj < 1 Then
                Return 0
            End If
            Return obj

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.NumberCount")
            Return 0
        End Try
    End Function

    Shared Function NumberFirstRecord(ByVal table As DataTable, ByVal number As Integer) As DataRow
        Try
            Dim cond As String = String.Format("f_i番号 = {0}", number)
            Dim rows() As DataRow = table.Select(cond, "f_iひも番号 ASC")
            If rows Is Nothing OrElse rows.Count = 0 Then
                Return Nothing
            End If
            Return rows(0)

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.NumberFirstRecord")
            Return Nothing
        End Try
    End Function

#End Region

End Class
