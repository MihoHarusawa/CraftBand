Imports System.Drawing
Imports System.Threading.Channels
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstMasterTables

''' <summary>
''' 設定データ
''' </summary>
Public Class clsMasterTables

    Public Const DefaultFileName As String = "CraftBandMesh" 'インストーラ同梱
    Public Const MyExtention As String = ".XML"
    Public Const MyBakExtention As String = ".BAK"
    Public Const MaxRgbValue As Integer = 255

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
        SetAvairableOptions() Or
        SetAvairableColorType() Or
        SetAvairableGauge() Or
        SetAvairableUpDown()
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
            _copyDataSet.Dispose()
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
            _dstMasterTables.Dispose()
            _dstMasterTables = _copyDataSet
            _copyDataSet = Nothing
            Return False

        End Try
        _copyDataSet.Dispose()
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
            _copyDataSet.Dispose()
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
        If after.GetChanges Is Nothing AndAlso Not _copyDataSet.HasChanges _
            AndAlso before.Rows.Count = after.Rows.Count Then '変更なく削除だけの時対策
            Return False 'no change
        End If
        IsDirty = True

        _dstMasterTables.Clear()
        _dstMasterTables.Dispose()
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
            '※スキーマ上Null許容＆値の存在前提で使用
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
    '(issue#3)縁でも底でもなく、is概算用=Trueの時は、概算で使える編みかた
    Public Function GetPatternNames(ByVal is縁専用 As Boolean, ByVal is底使用 As Boolean,
                                    Optional ByVal is概算用 As Boolean = False) As String()

        Dim fieldNameExe As String = "f_b" & g_enumExeName.ToString
        Dim table As tbl編みかたDataTable = _dstMasterTables.Tables("tbl編みかた")
        Dim res
        If is縁専用 Then
            res = (From row As tbl編みかたRow In table
                   Where row.f_b縁専用区分 And row(fieldNameExe)
                   Select PatternName = row.f_s編みかた名
                   Order By PatternName).Distinct.ToList
        ElseIf is底使用 Then
            res = (From row As tbl編みかたRow In table
                   Where row.f_b底使用区分 And row(fieldNameExe)
                   Select PatternName = row.f_s編みかた名
                   Order By PatternName).Distinct.ToList
        ElseIf is概算用 Then
            '側面用、概算で使える(高さ比率対ひも幅が正、ひも長比率対周長が正)
            res = (From row As tbl編みかたRow In table
                   Where Not row.f_b縁専用区分 And row(fieldNameExe) And 0 < row.f_d高さ比率対ひも幅 And 0 < row.f_dひも長比率対周長
                   Select PatternName = row.f_s編みかた名
                   Order By PatternName).Distinct.ToList
        Else
            '側面用の全て
            res = (From row As tbl編みかたRow In table
                   Where Not row.f_b縁専用区分 And row(fieldNameExe)
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

        'f_dひも長 にセットする1本の値(issue#5:ひも1の幅を追加)
        Public Function GetBandLength(ByVal lane1 As Integer, ByVal length As Double, ByVal vertcount As Integer) As Double
            Return length * Me.Value("f_dひも長比率対周長") _
                + Me.Value("f_dひも長加算1目あたり") * vertcount _
                + Me.Value("f_dひも1幅係数1目あたり") * g_clsSelectBasics.p_d指定本幅(lane1) * vertcount _
                + Me.Value("f_dひも長加算1周あたり") _
                + Me.Value("f_dひも長加算ひもあたり")
        End Function

        'f_dひも長 にセットする周連続の値(issue#5:ひも1の幅を追加)
        Public Function GetContinuoutBandLength(ByVal lane1 As Integer, ByVal length As Double, ByVal vertcount As Integer, ByVal rounds As Integer) As Double
            Return _
                rounds * (length * Me.Value("f_dひも長比率対周長") _
                + Me.Value("f_dひも長加算1目あたり") * vertcount _
                + Me.Value("f_dひも1幅係数1目あたり") * g_clsSelectBasics.p_d指定本幅(lane1) * vertcount _
                + Me.Value("f_dひも長加算1周あたり")) _
                + Me.Value("f_dひも長加算ひもあたり")
        End Function

    End Class

#End Region

#Region "付属品/Options"
    Private Function SetAvairableOptions() As Boolean
        Dim table As tbl付属品DataTable = _dstMasterTables.Tables("tbl付属品")
        Dim modified As Boolean = False

        'スキーマ変更対応 : ※スキーマ上Null許容＆値の存在前提で使用
        For Each r As tbl付属品Row In table.Rows
            For Each col As DataColumn In table.Columns
                If Not {"f_d長さ比率対ひも1", "f_d長さ加減対ひも1", "f_d巻きの厚み", "f_d巻き回数比率"}.Contains(col.ColumnName) Then
                    'Null値を許すフィールド以外
                    If IsDBNull(r(col.ColumnName)) Then
                        If Not IsDBNull(col.DefaultValue) Then
                            'デフォルト値あり
                            r(col.ColumnName) = col.DefaultValue
                            modified = True
                            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableOptions Modify {0} ({1}:{2})", col.ColumnName, r("f_s付属品名"), r("f_s付属品ひも名"))
                        Else
                            'デフォルト値がNull
                            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableOptions Null {0} ({1}:{2})", col.ColumnName, r("f_s付属品名"), r("f_s付属品ひも名"))
                        End If
                    Else
                        'Debug.Print("check {0}={1}", col.ColumnName, (r(col.ColumnName)))
                    End If
                End If
            Next
        Next

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

        Dim fieldNameExe As String = "f_b" & g_enumExeName.ToString
        Dim table As tbl付属品DataTable = _dstMasterTables.Tables("tbl付属品")
        Dim res = (From row As tbl付属品Row In table
                   Where row(fieldNameExe)
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

#Region "描画色/Color"

    Class clsColorRecordSet
        Public PenColor As Drawing.Color = Color.Empty  '描画色・Solid塗りつぶし色
        Public LanePenColor As Drawing.Color = Color.Empty '本幅描画色
        Public BrushAlfaColor As Drawing.Color = Color.Empty 'Alfa塗りつぶし色

        Public PenWidth As Single = 0  'ペンの幅
        Public LanePenWidth As Single = 0  '本幅ペンの幅

        Sub New(ByVal row As tbl描画色Row)
            PenColor = RgbColor(row.f_i赤, row.f_i緑, row.f_i青)
            If Not row.Isf_i透明度Null AndAlso 0 < row.f_i透明度 Then
                BrushAlfaColor = RgbColor(row.f_i赤, row.f_i緑, row.f_i青, row.f_i透明度)
            End If
            If row.Isf_d線幅Null Then
                PenWidth = 1 'デフォルト1
            ElseIf row.f_d線幅 <= 0 Then
                PenWidth = 0
            Else
                PenWidth = row.f_d線幅
            End If
            If Not row.Isf_d中線幅Null AndAlso 0 < row.f_d中線幅 Then
                LanePenWidth = row.f_d中線幅
            End If

            '本幅描画色については、とりあえず、少し薄い色にしておく
            LanePenColor = Color.FromArgb(100, PenColor)
        End Sub
    End Class


    Private Function SetAvairableColorType() As Boolean
        Dim table As tbl描画色DataTable = _dstMasterTables.Tables("tbl描画色")

        Dim modified As Boolean = False
        For Each r As tbl描画色Row In table.Rows
            Dim crow As New clsDataRow(r)
            If crow.SetDefaultForNull() Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableColorType Modify {0}", crow.ToString)
                modified = True
            End If
        Next
        Return modified
    End Function

    Shared Function RgbColor(ByVal ored As Object, ByVal ogreen As Object, ByVal oblue As Object, Optional oalfa As Object = Nothing) As Drawing.Color
        Dim ired As Integer = 0
        If ored IsNot Nothing AndAlso Not IsDBNull(ored) Then
            ired = Val(ored)
            If ired < 0 Then
                ired = 0
            ElseIf MaxRgbValue < ired Then
                ired = MaxRgbValue
            End If
        End If

        Dim igreen As Integer = 0
        If ogreen IsNot Nothing AndAlso Not IsDBNull(ogreen) Then
            igreen = Val(ogreen)
            If igreen < 0 Then
                igreen = 0
            ElseIf MaxRgbValue < igreen Then
                igreen = MaxRgbValue
            End If
        End If

        Dim iblue As Integer = 0
        If oblue IsNot Nothing AndAlso Not IsDBNull(oblue) Then
            iblue = Val(oblue)
            If iblue < 0 Then
                iblue = 0
            ElseIf MaxRgbValue < iblue Then
                iblue = MaxRgbValue
            End If
        End If

        Dim ialfa As Integer = 255
        If oalfa IsNot Nothing AndAlso Not IsDBNull(oalfa) Then
            ialfa = Val(oalfa)
            If ialfa < 0 Then
                ialfa = 0
            ElseIf MaxRgbValue < ialfa Then
                ialfa = MaxRgbValue
            End If
        End If

        Return Color.FromArgb(ialfa, ired, igreen, iblue)
    End Function

    '指定名の描画色 なければNothing
    Public Function GetColorRecordSet(ByVal color As String) As clsColorRecordSet
        Dim table As tbl描画色DataTable = _dstMasterTables.Tables("tbl描画色")

        If table.Rows.Count = 0 OrElse String.IsNullOrWhiteSpace(color) Then
            Return Nothing
        End If

        Dim cond As String = String.Format("f_s色 = '{0}'", color)
        Dim rows() As tbl描画色Row = table.Select(cond)
        If 0 < rows.Count Then
            Return New clsColorRecordSet(rows(0)) 'First only
        End If
        Return Nothing
    End Function

    Public Function GetColorTableCopy() As tbl描画色DataTable
        Return CType(getCopyDataSetTable("tbl描画色"), tbl描画色DataTable)
    End Function

    Public Function UpdateColorTable(ByVal table As tbl描画色DataTable) As Boolean
        If table Is Nothing Then
            Return False 'No Update
        End If

        Dim original As tbl描画色DataTable = _dstMasterTables.Tables("tbl描画色")

        If table.GetChanges IsNot Nothing Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "UpdateColorTable:{0}", New clsGroupDataRow(table).ToString)
        End If

        Return updateCopyTableIfModified(original, table)
    End Function
#End Region

#Region "ゲージ/Gauge"

    Private Function SetAvairableGauge() As Boolean
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")

        Dim modified As Boolean = False
        For Each r As tblゲージRow In table.Rows
            Dim crow As New clsDataRow(r)
            If crow.SetDefaultForNull() Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableGauge Modify {0}", crow.ToString)
                modified = True
            End If
        Next
        Return modified
    End Function

    Public Function GetBandTypeGauges(ByVal bandtypename As String) As tblゲージRow()
        If String.IsNullOrWhiteSpace(bandtypename) Then
            Return Nothing
        End If
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")
        Dim cond As String = String.Format("f_sバンドの種類名 = '{0}'", bandtypename)
        Return table.Select(cond)
    End Function

    Public Function UpdateBandTypeGauges(ByVal bandtypename As String, ByVal subtable As tblゲージDataTable) As Boolean
        Dim changed As Boolean = False

        '該当バンドの種類を削除
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")
        table.AcceptChanges()

        Dim query = From r In table.AsEnumerable
                    Where r.f_sバンドの種類名 = bandtypename
                    Select r
        For Each r As tblゲージRow In query
            r.Delete()
            changed = True
        Next
        table.AcceptChanges()

        '該当バンドの種類を追加
        If subtable IsNot Nothing AndAlso 0 < subtable.Rows.Count Then
            For Each gage As tblゲージRow In subtable
                If gage.f_sバンドの種類名 = bandtypename Then
                    table.ImportRow(gage)
                    changed = True
                End If
            Next
            table.AcceptChanges()
        End If

        If changed Then
            _IsDirty = True
        End If
        Return changed
    End Function

#End Region

#Region "上下模様/UpDown"

    Private Function SetAvairableUpDown() As Boolean
        Dim table As tbl上下模様DataTable = _dstMasterTables.Tables("tbl上下模様")
        Dim modified As Boolean = False
        For Each r As tbl上下模様Row In table.Rows
            Dim crow As New clsDataRow(r)
            '※スキーマ上Null許容＆値の存在前提で使用
            If crow.SetDefaultForNull() Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableUpDown Modify {0}", crow.ToString)
                modified = True
            End If
        Next
        Return modified
    End Function

    Public Function GetUpDownTableCopy() As tbl上下模様DataTable
        Return CType(getCopyDataSetTable("tbl上下模様"), tbl上下模様DataTable)
    End Function

    Public Function UpdateUpDownTable(ByVal table As tbl上下模様DataTable) As Boolean
        If table Is Nothing Then
            Return False 'No Update
        End If

        Dim original As tbl上下模様DataTable = _dstMasterTables.Tables("tbl上下模様")

        If table.GetChanges IsNot Nothing Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "UpdateUpDownTable:{0}", New clsGroupDataRow(table).ToString)
        End If

        Return updateCopyTableIfModified(original, table)
    End Function


    '上下模様名の配列を返す　f_s上下模様名順
    Public Function GetUpDownNames() As String()
        Dim table As tbl上下模様DataTable = _dstMasterTables.Tables("tbl上下模様")
        Dim res = (From row As tbl上下模様Row In table
                   Select UpDownName = row.f_s上下模様名
                   Order By UpDownName).Distinct.ToList
        Return res.ToArray
    End Function

    '指定名のtbl上下模様Row, なければNothing
    Public Function GetUpDownRecord(ByVal updownname As String) As tbl上下模様Row
        If String.IsNullOrWhiteSpace(updownname) Then
            Return Nothing
        End If
        Dim table As tbl上下模様DataTable = _dstMasterTables.Tables("tbl上下模様")

        Dim cond As String = String.Format("f_s上下模様名 = '{0}'", updownname)
        Dim rows() As tbl上下模様Row = table.Select(cond)
        If rows IsNot Nothing AndAlso 0 < rows.Count Then
            Return rows(0) 'キーなので1点のはず
        Else
            Return Nothing
        End If
    End Function

    '指定名のtbl上下模様Rowを作る。既にあればNothing
    Public Function GetNewUpDownRecord(ByVal updownname As String) As tbl上下模様Row
        If String.IsNullOrWhiteSpace(updownname) Then
            Return Nothing
        End If
        If GetUpDownRecord(updownname) IsNot Nothing Then
            Return Nothing
        End If

        Dim table As tbl上下模様DataTable = _dstMasterTables.Tables("tbl上下模様")
        Dim row As tbl上下模様Row = table.Newtbl上下模様Row
        row.f_s上下模様名 = updownname
        table.Rows.Add(row)
        Return row
    End Function

#End Region


End Class
