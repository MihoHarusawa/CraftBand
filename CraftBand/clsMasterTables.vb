Imports CraftBand.Tables
Imports CraftBand.Tables.dstMasterTables

''' <summary>
''' 設定データ
''' </summary>
Public Class clsMasterTables

    Public Const MyExtention As String = ".XML"
    Public Const MyBakExtention As String = ".BAK"

    Dim _dstMasterTables As dstMasterTables

    Dim _MasterTablesFilePath As String = Nothing
    Public Property MasterTablesFilePath As String
        Get
            Return _MasterTablesFilePath
        End Get
        Set(value As String)
            If String.Compare(value, _MasterTablesFilePath, True) <> 0 Then
                _MasterTablesFilePath = value
                _IsDirty = True
            End If
        End Set
    End Property

    Dim _IsDirty As Boolean = False
    Public Property IsDirty As Boolean
        Get
            Return _IsDirty OrElse _dstMasterTables.HasChanges
        End Get
        Set(value As Boolean)
            _IsDirty = value
        End Set
    End Property


    'work
    Dim _copyDataSet As dstMasterTables = Nothing
    Dim _copyTable As DataTable = Nothing


    Sub New()
        _dstMasterTables = New dstMasterTables
        IsDirty = False
        SetAvairable() 'Dirtyにはしない
    End Sub

    Sub New(ByVal ref As clsMasterTables)
        _dstMasterTables = ref._dstMasterTables.Copy
        _MasterTablesFilePath = ref._MasterTablesFilePath
        IsDirty = ref.IsDirty
    End Sub

    '有効にするため更新した場合はTrueを返す
    Private Function SetAvairable() As Boolean
        Return SetAvairableBasics() Or
        SetAvairableBandType() Or
        SetAvairablePattern() Or
        SetAvairableOptions()
    End Function


    'DataSet.HasChanges←False
    Private Sub acceptChangesAll()
        For Each table As DataTable In _dstMasterTables.Tables
            table.AcceptChanges()
        Next
    End Sub


    Public Function LoadFile(ByVal path As String, ByVal isSetName As Boolean) As Boolean
        If String.IsNullOrWhiteSpace(path) OrElse Not IO.File.Exists(path) Then
            Return False
        End If
        If _copyDataSet IsNot Nothing Then
            _copyDataSet = Nothing
        End If
        _copyDataSet = _dstMasterTables.Copy
        '
        _dstMasterTables.Clear()
        Try
            Dim readmode As System.Data.XmlReadMode = _dstMasterTables.ReadXml(path, System.Data.XmlReadMode.IgnoreSchema)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "dstMasterTables.ReadXml={0} {1}", path, readmode)
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsMasterTables.LoadFile", path)
            _dstMasterTables = _copyDataSet
            _copyDataSet = Nothing
            Return False

        End Try
        _copyDataSet = Nothing

        '_dstMasterTables.HasChanges = True
        acceptChangesAll()

        If isSetName Then
            _MasterTablesFilePath = path
        End If

        If SetAvairable() Then
            IsDirty = True
        End If

        Return True
    End Function

    '現DataSetのバージョン
    Public ReadOnly Property FormatVersion As String
        Get
            Dim r As tbl基本値Row = _dstMasterTables.Tables("tbl基本値").NewRow
            Return r.f_sバージョン
        End Get
    End Property

    Public Function Save() As Boolean
        'バイナリによるバージョン
        SetAvairableBasics()
        Dim drow As New clsDataRow(_dstMasterTables.Tables("tbl基本値").Rows(0))
        If drow.Value("f_sバージョン") <> FormatVersion Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "MasterTablesVersion {0} -> {1}", drow.Value("f_sバージョン"), FormatVersion)
            drow.Value("f_sバージョン") = FormatVersion
            IsDirty = True
        End If

        If IsDirty Then
            Try
                If IO.File.Exists(_MasterTablesFilePath) Then
                    Dim bak As String = IO.Path.ChangeExtension(_MasterTablesFilePath, MyBakExtention)
                    If IO.File.Exists(bak) Then
                        IO.File.Delete(bak)
                    End If
                    IO.File.Move(_MasterTablesFilePath, bak)
                End If
                _dstMasterTables.WriteXml(_MasterTablesFilePath, System.Data.XmlWriteMode.WriteSchema)
            Catch ex As Exception
                g_clsLog.LogException(ex, "clsMasterTables.Save", _MasterTablesFilePath)
                Return False

            End Try
            IsDirty = False
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "dstMasterTables.WriteXml={0}", _MasterTablesFilePath)
        Else
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "dstMasterTables No Save File {0}", _MasterTablesFilePath)
        End If
        Return True
    End Function


    'DataSetを丸ごとコピーして,その一部であるDataTableを返す
    Private Function getCopyDataSetTable(ByVal tablename As String) As DataTable
        acceptChangesAll()

        If _copyDataSet IsNot Nothing Then
            _copyDataSet.Clear()
            _copyDataSet = Nothing
        End If
        _copyDataSet = _dstMasterTables.Copy

        _copyTable = _copyDataSet.Tables(tablename)
        Return _copyTable
    End Function

    '変更があった時はTrueを返す
    Private Function updateCopyTableIfModified(ByVal before As DataTable, ByVal after As DataTable) As Boolean
        '丸ごと置換するのでbeforeは参照しません

        If _copyDataSet Is Nothing OrElse _copyTable Is Nothing Then
            Return False 'no update
        End If
        If after.GetHashCode <> _copyTable.GetHashCode Then
            Return False 'make sure
        End If
        If after.GetChanges Is Nothing AndAlso Not _copyDataSet.HasChanges Then
            Return False 'no change
        End If
        IsDirty = True

        _dstMasterTables.Clear()
        _dstMasterTables = Nothing
        _dstMasterTables = _copyDataSet

        _copyDataSet = Nothing
        _copyTable = Nothing

        acceptChangesAll()
        Return True
    End Function


#Region "基本値/Basics"
    Private Function SetAvairableBasics() As Boolean
        '1レコードのみ
        Dim deleted As Boolean = False
        Dim table As tbl基本値DataTable = _dstMasterTables.Tables("tbl基本値")
        If table.Rows.Count = 0 Then
            Dim crow As New clsDataRow(table.NewRow)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableBasics Add {0}", crow.ToString)
            table.Rows.Add(crow.DataRow)
            Return True 'added

        ElseIf 1 < table.Rows.Count Then
            Do While 1 < table.Rows.Count
                Dim lastindex As Integer = table.Rows.Count - 1
                table.Rows.RemoveAt(lastindex)
            Loop
            deleted = True
        End If
        '1レコードになった
        Dim crow0 As New clsDataRow(table.Rows(0))
        If crow0.SetDefaultForNull() Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableBasics Modify {0}", crow0.ToString)
            Return True
        Else
            Return deleted
        End If
    End Function


    'tbl基本値のf_s単位値
    Public Function GetBasicUnit() As String
        SetAvairableBasics()
        Try
            Return _dstMasterTables.Tables("tbl基本値").Rows(0).Item("f_s単位").ToString
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsMasterTables.GetBasicUnit", "f_s単位")
            Return Nothing
        End Try
        Return New clsDataRow(_dstMasterTables.Tables("tbl基本値").Rows(0)).Value("f_s単位")
    End Function

    Public Function SetBasicUnit(ByVal str As String) As Boolean
        SetAvairableBasics()
        Try
            Dim val As String = _dstMasterTables.Tables("tbl基本値").Rows(0).Item("f_s単位")
            If val = str Then
                Return True
            Else
                _dstMasterTables.Tables("tbl基本値").Rows(0).Item("f_s単位") = str
                IsDirty = True
            End If
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsMasterTables.SetBasicUnit", "f_s単位", str)
            Return False
        End Try
    End Function

#End Region

#Region "バンドの種類/BandType"
    Private Function SetAvairableBandType() As Boolean
        Dim table As tblバンドの種類DataTable = _dstMasterTables.Tables("tblバンドの種類")

        'レコードは少なくとも1点
        If 0 = table.Rows.Count Then
            Dim crow As New clsDataRow(table.NewRow)
            With crow
                .Value("f_sバンドの種類名") = "DefaultBand"
                .Value("f_i本幅") = 12
                .Value("f_dバンド幅") = 14.5
            End With
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableBandType Add {0}", crow.ToString)
            table.Rows.Add(crow.DataRow)
            Return True 'Changed
        End If

        Dim modified As Boolean = False
        For Each r As tblバンドの種類Row In table.Rows
            Dim crow As New clsDataRow(r)
            If crow.SetDefaultForNull() Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableBandType Modify {0}", crow.ToString)
                modified = True
            End If
        Next
        Return modified
    End Function

    '指定名のtblバンドの種類Row
    Public Function GetBandTypeRecord(ByVal bandtypename As String, ByVal isSetAnyIfNone As Boolean) As clsDataRow
        Dim table As tblバンドの種類DataTable = _dstMasterTables.Tables("tblバンドの種類")

        If table.Rows.Count = 0 Then
            SetAvairableBandType()
            IsDirty = True
        End If
        If String.IsNullOrWhiteSpace(bandtypename) AndAlso isSetAnyIfNone Then
            Return New clsDataRow(table.Rows(0)) 'First
        End If

        Dim cond As String = String.Format("f_sバンドの種類名 = '{0}'", bandtypename)
        Dim rows() As tblバンドの種類Row = table.Select(cond)
        If 0 < rows.Count Then
            Return New clsDataRow(rows(0)) 'First only
        End If

        If Not isSetAnyIfNone Then
            Return Nothing
        Else
            Return New clsDataRow(table.Rows(0))
        End If
    End Function

    'バンドの種類名の配列
    Public Function GetBandTypeNames() As String()
        Dim table As tblバンドの種類DataTable = _dstMasterTables.Tables("tblバンドの種類")

        If table.Rows.Count = 0 Then
            SetAvairableBandType()
            IsDirty = True
        End If

        Dim res = (From row In table
                   Select row.Field(Of String)("f_sバンドの種類名")).Distinct.ToList()

        Return res.ToArray
    End Function


    Public Function GetBandTypeTableCopy() As tblバンドの種類DataTable
        Return CType(getCopyDataSetTable("tblバンドの種類"), tblバンドの種類DataTable)
    End Function

    Public Function UpdateBandTypeTable(ByVal table As tblバンドの種類DataTable) As Boolean
        If table Is Nothing Then
            Return False 'No Update
        End If

        Dim original As tblバンドの種類DataTable = _dstMasterTables.Tables("tblバンドの種類")

        If table.GetChanges IsNot Nothing Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "UpdateBandTypeTable:{0}", New clsGroupDataRow(table).ToString)
        End If

        Return updateCopyTableIfModified(original, table)
    End Function
#End Region


#Region "編みかた/Pattern"
    Private Function SetAvairablePattern() As Boolean
        Dim table As tbl編みかたDataTable = _dstMasterTables.Tables("tbl編みかた")
        Dim modified As Boolean = False
        For Each r As tbl編みかたRow In table.Rows
            Dim crow As New clsDataRow(r)
            If crow.SetDefaultForNull() Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairablePattern Modify {0}", crow.ToString)
                modified = True
            End If
        Next
        Return modified
    End Function

    Public Function GetPatternTableCopy() As tbl編みかたDataTable
        Return CType(getCopyDataSetTable("tbl編みかた"), tbl編みかたDataTable)
    End Function

    Public Function UpdatePatternTable(ByVal table As tbl編みかたDataTable) As Boolean
        If table Is Nothing Then
            Return False 'No Update
        End If

        Dim original As tbl編みかたDataTable = _dstMasterTables.Tables("tbl編みかた")

        If table.GetChanges IsNot Nothing Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "UpdatePatternTable:{0}", New clsGroupDataRow(table).ToString)
        End If

        Return updateCopyTableIfModified(original, table)
    End Function


    '編みかた名の配列を返す　f_s編みかた名順
    Public Function GetPatternNames(ByVal is縁専用 As Boolean, ByVal is底使用 As Boolean) As String()

        Dim table As tbl編みかたDataTable = _dstMasterTables.Tables("tbl編みかた")
        Dim res
        If is縁専用 Then
            res = (From row As tbl編みかたRow In table
                   Where row.f_b縁専用区分 And Not row.f_b非表示
                   Select PatternName = row.f_s編みかた名
                   Order By PatternName).Distinct.ToList
        ElseIf is底使用 Then
            res = (From row As tbl編みかたRow In table
                   Where row.f_b底使用区分 And Not row.f_b非表示
                   Select PatternName = row.f_s編みかた名
                   Order By PatternName).Distinct.ToList
        Else
            res = (From row As tbl編みかたRow In table
                   Where Not row.f_b縁専用区分 And Not row.f_b非表示
                   Select PatternName = row.f_s編みかた名
                   Order By PatternName).Distinct.ToList
        End If

        Return res.ToArray
    End Function

    '指定名のtbl編みかたRow
    Public Function GetPatternRecord(ByVal pattername As String, ByVal bandnum As Integer) As clsPatternDataRow
        Dim table As tbl編みかたDataTable = _dstMasterTables.Tables("tbl編みかた")

        Dim cond As String = String.Format("f_s編みかた名 = '{0}' AND f_iひも番号 = {1}", pattername, bandnum)
        Dim rows() As tbl編みかたRow = table.Select(cond)
        Dim row As tbl編みかたRow = Nothing
        If rows IsNot Nothing AndAlso 0 < rows.Count Then
            row = rows(0)
        End If
        Return New clsPatternDataRow(row)
    End Function

    Public Function GetPatternRecordGroup(ByVal pattername As String) As clsGroupDataRow
        Dim table As tbl編みかたDataTable = _dstMasterTables.Tables("tbl編みかた")

        Dim cond As String = String.Format("f_s編みかた名 = '{0}'", pattername)
        Return New clsGroupDataRow(table.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")
    End Function

    'tbl編みかたRow処理用のラッパー
    Class clsPatternDataRow
        Inherits clsDataRow

        Public Sub New(row As tbl編みかたRow)
            MyBase.New(row)
        End Sub

        Public Sub New(base As clsDataRow)
            MyBase.New(base.DataRow)
        End Sub

        'f_i何本幅 にセットする初期値を返す
        Public Function GetFirstLane(ByVal basic As Integer) As Integer
            Dim lane As Integer = Me.Value("f_i本幅初期値")
            If lane < 1 Then
                Return basic
            ElseIf g_clsSelectBasics.p_i本幅 < lane Then
                Return g_clsSelectBasics.p_i本幅
            Else
                Return lane
            End If
        End Function

        'f_d高さ にセットする値(1周あたり)
        Public Function GetHeight(ByVal lane As Integer) As Double
            Return g_clsSelectBasics.p_d指定本幅(lane) * Me.Value("f_d高さ比率対ひも幅")
        End Function

        'f_d垂直ひも長 にセットする値(1周あたり)
        Public Function GetBandLength(ByVal lane As Integer) As Double
            Return g_clsSelectBasics.p_d指定本幅(lane) * Me.Value("f_d垂直ひも長比率対ひも幅")
        End Function

        'f_d径 にセットする値(1周あたり)
        Public Function GetDiameter(ByVal lane As Integer) As Double
            Return g_clsSelectBasics.p_d指定本幅(lane) * Me.Value("f_d垂直ひも長比率対ひも幅")
        End Function

        'f_dひも長 にセットする1本の値
        Public Function GetBandLength(ByVal length As Double, ByVal vertcount As Integer) As Double
            Return length * Me.Value("f_dひも長比率対周長") _
                + Me.Value("f_dひも長加算1目あたり") * vertcount _
                + Me.Value("f_dひも長加算1周あたり") _
                + Me.Value("f_dひも長加算ひもあたり")
        End Function

        'f_dひも長 にセットする周連続の値
        Public Function GetContinuoutBandLength(ByVal length As Double, ByVal vertcount As Integer, ByVal rounds As Integer) As Double
            Return _
                rounds * (length * Me.Value("f_dひも長比率対周長") _
                + Me.Value("f_dひも長加算1目あたり") * vertcount _
                + Me.Value("f_dひも長加算1周あたり")) _
                + Me.Value("f_dひも長加算ひもあたり")
        End Function

    End Class

#End Region

#Region "付属品/Options"
    Private Function SetAvairableOptions() As Boolean
        Dim table As tbl付属品DataTable = _dstMasterTables.Tables("tbl付属品")
        Dim modified As Boolean = False

#If 0 Then  '付属品にはNull値も登録しているのでそのままにする
       For Each r As tbl付属品Row In table.Rows
            Dim crow As New clsDataRow(r)
            If crow.SetDefaultForNull() Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableOptions Modify {0}", crow.ToString)
                modified = True
            End If
        Next
#End If
        Return modified
    End Function

    Public Function GetOptionsTableCopy() As tbl付属品DataTable
        Return CType(getCopyDataSetTable("tbl付属品"), tbl付属品DataTable)
    End Function

    Public Function UpdateOptionsTable(ByVal table As tbl付属品DataTable) As Boolean
        If table Is Nothing Then
            Return False 'No Update
        End If

        Dim original As tbl付属品DataTable = _dstMasterTables.Tables("tbl付属品")

        If table.GetChanges IsNot Nothing Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "UpdateOptionsTable:{0}", New clsGroupDataRow(table).ToString)
        End If

        Return updateCopyTableIfModified(original, table)
    End Function

    '付属品名の配列を返す f_s付属品名順 
    Public Function GetOptionNames() As String()

        Dim table As tbl付属品DataTable = _dstMasterTables.Tables("tbl付属品")
        Dim res = (From row As tbl付属品Row In table
                   Where Not row.f_b非表示
                   Select OptionName = row.f_s付属品名
                   Order By OptionName).Distinct.ToList()

        Return res.ToArray
    End Function

    '指定名のtbl付属品Rowのグループ
    Public Function GetOptionRecordGroup(ByVal optionname As String) As clsGroupDataRow
        Dim table As tbl付属品DataTable = _dstMasterTables.Tables("tbl付属品")

        Dim cond As String = String.Format("f_s付属品名 = '{0}'", optionname)
        Return New clsGroupDataRow(table.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")
    End Function

    'tbl付属品Row処理用のラッパー
    Class clsOptionDataRow
        Inherits clsDataRow

        Public Sub New(row As tbl付属品Row)
            MyBase.New(row)
        End Sub

        Public Sub New(base As clsDataRow)
            MyBase.New(base.DataRow)
        End Sub

        'f_i何本幅 にセットする初期値を返す
        Public Function GetFirstLane(ByVal basic As Integer) As Integer
            Dim lane As Integer = Me.Value("f_i本幅初期値")
            If lane < 1 Then
                Return basic
            ElseIf g_clsSelectBasics.p_i本幅 < lane Then
                Return g_clsSelectBasics.p_i本幅
            Else
                Return lane
            End If
        End Function

        'f_d長さ にセットする値
        Public Function GetLength(ByVal isFirst As Boolean, ByVal length As Double) As Double
            If isFirst Then
                Return length
            Else
                Return length * Me.Value("f_d長さ比率対ひも1") + Me.Value("f_d長さ加減対ひも1")
            End If
        End Function

        'f_dひも長 にセットする値
        Public Function GetBandLength(ByVal length As Double) As Double
            Return length * Me.Value("f_dひも長比率対長さ") + Me.Value("f_dひも長加算")
        End Function
        'f_dひも長 にセットする巻きひもの値
        Public Function GetWindBandLength(ByVal length As Double, ByVal thisLane As Integer, ByVal lastLane As Integer) As Double
            If thisLane <= 0 Then
                Return 0
            End If
            If lastLane <= 0 Then
                lastLane = thisLane
            End If
            Dim dひと巻き As Double = 2 * (g_clsSelectBasics.p_d指定本幅(lastLane) + Me.Value("f_d巻きの厚み"))
            Dim d巻き数 As Double = length / g_clsSelectBasics.p_d指定本幅(thisLane)

            Return dひと巻き * d巻き数 * Me.Value("f_d巻き回数比率") + Me.Value("f_dひも長加算")
        End Function

    End Class



#End Region

End Class
