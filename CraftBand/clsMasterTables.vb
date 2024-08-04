Imports System.Drawing
Imports System.Threading.Channels
Imports System.Windows.Forms
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
    Public Const CommonColorBandType As String = "-" '描画色のバンドの種類名のDefaultValue


    Dim _dstMasterTables As dstMasterTables

    Dim _MasterTablesFilePath As String = Nothing
    Public Property MasterTablesFilePath As String
        Get
            Return _MasterTablesFilePath
        End Get
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If String.IsNullOrEmpty(_MasterTablesFilePath) OrElse String.Compare(value, _MasterTablesFilePath, True) <> 0 Then
                    _MasterTablesFilePath = value
                    _IsDirty = True
                End If
            Else
                '空はスキップ
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

    '生成。まだ設定ファイルとしては使えない状態
    Sub New()
        _dstMasterTables = New dstMasterTables
        IsDirty = False
    End Sub

    '現内容をクリアし設定ファイルとして使える状態にする
    Sub ReNew()
        _dstMasterTables.Dispose()
        _dstMasterTables = Nothing
        _dstMasterTables = New dstMasterTables

        If _copyDataSet IsNot Nothing Then
            _copyDataSet.Dispose()
            _copyDataSet = Nothing
        End If
        If _copyTable IsNot Nothing Then
            _copyTable.Dispose()
            _copyTable = Nothing
        End If

        _MasterTablesFilePath = Nothing

        SetAvairable()
        IsDirty = False
    End Sub


    '設定ファイルとして使える状態にする
    '更新した場合はTrueを返す,_IsDirtyは変えない。
    Public Function SetAvairable() As Boolean

        Dim changed As Boolean = SetAvairableBasics() Or
        SetAvairableBandType() Or
        SetAvairablePattern() Or
        SetAvairableOptions() Or
        SetAvairableColorType() Or
        SetAvairableGauge() Or
        SetAvairableUpDown()

        If changed Then
            acceptChangesAll()
        End If

        Return changed
    End Function

    'DataSet.HasChanges←False
    Private Sub acceptChangesAll()
        For Each table As DataTable In _dstMasterTables.Tables
            table.AcceptChanges()
        Next
    End Sub

    'ファイル読み取り。読み取れた場合は設定ファイルとして使える状態にする。読み取れなければ元の状態
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

            '正しいXML形式であることをチェック
            If _dstMasterTables.Tables("tbl基本値").Rows.Count = 0 Then
                Throw New Exception("Bad XML (No Basic Record)")
            End If

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

#Region "インポート・エクスポート"
    Private Enum ImportResult
        NoOtherRecord       'レコードなし
        KeepThisSkip        '両: 変更なし・上書きしないため。比較はしない
        KeepThisModify      '両: 上書しない指定だが、色のみ追記
        SameNoAction        '両: 上書きだが同じなので変更なし
        UpdateThis          '両: 上書き更新
        AddToThis           '追加
    End Enum

    '別の設定ファイルからの更新インポート
    Public Function Import(ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean, ByRef msg As String) As Boolean
        '設定ファイルのインポート (上書き)
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Basic, "LogImportMasterFile", othermaster.MasterTablesFilePath, isOverWrite)

        Dim changecount As Integer
        Dim changesum As Integer = 0
        Dim success As Boolean = True
        Dim sb As New System.Text.StringBuilder
        Dim changedBandTypeList As New List(Of String)

        '基本値は更新なし

        'バンドの種類名 
        changecount = importBandTypeTable(othermaster, isOverWrite, changedBandTypeList) '※参照色リスト追記
        sb.AppendLine(change_message((New frmBandType).Text, changecount))
        If changecount < 0 Then
            success = False
        Else
            changesum += changecount
        End If

        '描画色
        changecount = importColorTable(othermaster, isOverWrite) '※有効なバンドの種類名の色
        sb.AppendLine(change_message((New frmColor).Text, changecount))
        If changecount < 0 Then
            success = False
        Else
            changesum += changecount
        End If

        '編みかた
        changecount = importPatternTable(othermaster, isOverWrite)
        sb.AppendLine(change_message((New frmPattern).Text, changecount))
        If changecount < 0 Then
            success = False
        Else
            changesum += changecount
        End If

        '付属品
        changecount = importOptionsTable(othermaster, isOverWrite)
        sb.AppendLine(change_message((New frmOptions).Text, changecount))
        If changecount < 0 Then
            success = False
        Else
            changesum += changecount
        End If

        'ゲージ
        changecount = importBandTypeGauges(othermaster, isOverWrite, changedBandTypeList)
        sb.AppendLine(change_message((New frmGauge).Text, changecount))
        If changecount < 0 Then
            success = False
        Else
            changesum += changecount
        End If

        '上下図
        changecount = importUpDownTable(othermaster, isOverWrite)
        sb.AppendLine(change_message((New frmUpDownPattern).Text, changecount))
        If changecount < 0 Then
            success = False
        Else
            changesum += changecount
        End If

        If 0 < changesum Then
            '全更新数は {0}点でした。
            sb.AppendLine(String.Format(My.Resources.MsgUpdateAll, changesum))
        End If

        msg = sb.ToString
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "success={0} {1}", success, msg)
        Return success
    End Function

    Private Function change_message(ByVal tablename As String, ByVal changecount As Integer) As String
        If 0 < changecount Then
            '[{0}] の {1}点を更新しました。
            Return String.Format(My.Resources.MsgUpdate, tablename, changecount)
        ElseIf changecount < 0 Then
            '[{0}] の更新エラーです。設定メニューで確認してください。
            Return String.Format(My.Resources.MsgUpdateError, tablename)
        Else
            '[{0}] の更新はありません。
            Return String.Format(My.Resources.MsgUpdateNone, tablename)
        End If
    End Function

    'データによる参照を別の設定ファイルに書き出し
    '※othermasterはNewされただけ(SetAvairable()が呼ばれていない)可能性あり
    Public Function Export(ByVal data As clsDataTables, ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean, ByRef msg As String) As Boolean
        '設定ファイルのエクスポート (上書き)
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Basic, "LogExportMasterFile", othermaster.MasterTablesFilePath, isOverWrite)

        Dim changecount As Integer
        Dim changesum As Integer = 0
        Dim success As Boolean = True
        Dim sb As New System.Text.StringBuilder

        Dim level As clsLog.LogLevel = clsLog.LogLevel.Detail
        Dim result As ImportResult

        '基本値
        othermaster.SetBasicUnit(GetBasicUnit())

        'バンドの種類
        Dim bandTypeName As String = data.p_row目標寸法.Value("f_sバンドの種類名")
        result = othermaster.importBandType(bandTypeName, othermaster._dstMasterTables.Tables("tblバンドの種類"), level, Me, isOverWrite)
        If {ImportResult.AddToThis, ImportResult.UpdateThis, ImportResult.KeepThisModify}.Contains(result) Then
            sb.AppendLine(change_message((New frmBandType).Text, 1))
            changesum += 1
        Else
            sb.AppendLine(change_message((New frmBandType).Text, 0))
        End If

        'バンドの種類の色
        changecount = 0
        Dim bandTypeRecord As clsDataRow = GetBandTypeRecord(bandTypeName, False)
        If bandTypeRecord IsNot Nothing AndAlso bandTypeRecord.IsValid Then
            Dim colstr As String = bandTypeRecord.Value("f_s色リスト")
            Dim tableColor As New dstWork.tblColorDataTable
            If 0 < clsSelectBasics.ToColorTable(tableColor, colstr, False) Then
                For Each colrow As dstWork.tblColorRow In tableColor
                    '色を追加
                    result = exportColorBandType(colrow.Value, bandTypeName, othermaster, level, isOverWrite)
                    If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                        changecount += 1
                    End If
                Next
            End If
        End If
        sb.AppendLine(change_message((New frmColor).Text, changecount))
        changesum += changecount

        'バンドの種類のゲージ
        result = othermaster.importBandTypeGauge(bandTypeName, othermaster._dstMasterTables.Tables("tblゲージ"), level, Me, isOverWrite)
        If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
            sb.AppendLine(change_message((New frmGauge).Text, 1))
            changesum += 1
        Else
            sb.AppendLine(change_message((New frmGauge).Text, 0))
        End If

        'EXEの種類
        Dim enumExeName As enumExeName = enumExeName.Nodef
#If 1 Then
        enumExeName = g_enumExeName
#Else
        Dim exename As String = data.p_row目標寸法.Value("f_sEXE名")
        For Each enumExe As enumExeName In GetType(enumExeName).GetEnumValues
            If [String].Compare(exename, enumExe.ToString, True) = 0 Then
                enumExeName = enumExe
                Exit For
            End If
        Next
#End If
        '編みかた
        Dim patternlist As New List(Of String)
        'p_tbl底_楕円 Meshのみ使用
        For Each row As tbl底_楕円Row In data.p_tbl底_楕円
            If Not row.f_b差しひも区分 Then
                If Not patternlist.Contains(row.f_s編みかた名) Then
                    patternlist.Add(row.f_s編みかた名)
                End If
            End If
        Next
        'p_tbl側面
        For Each row As tbl側面Row In data.p_tbl側面
            If row.f_i番号 = clsDataTables.cHemNumber Then
                If Not patternlist.Contains(row.f_s編みかた名) Then
                    patternlist.Add(row.f_s編みかた名)
                End If
            Else
                If enumExeName = enumExeName.CraftBandMesh Then
                    If Not patternlist.Contains(row.f_s編みかた名) Then
                        patternlist.Add(row.f_s編みかた名)
                    End If
                End If
            End If
        Next
        changecount = 0
        For Each patternname As String In patternlist
            result = othermaster.importPattern(patternname, othermaster._dstMasterTables.Tables("tbl編みかた"), level, Me, isOverWrite)
            If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                changecount += 1
            End If
        Next
        sb.AppendLine(change_message((New frmPattern).Text, changecount))
        changesum += changecount

        '付属品
        'p_tbl追加品
        Dim optionlist As New List(Of String)
        For Each row As tbl追加品Row In data.p_tbl追加品
            If Not optionlist.Contains(row.f_s付属品名) Then
                optionlist.Add(row.f_s付属品名)
            End If
        Next
        changecount = 0
        For Each optionname As String In optionlist
            result = othermaster.importOption(optionname, othermaster._dstMasterTables.Tables("tbl付属品"), level, Me, isOverWrite)
            If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                changecount += 1
            End If
        Next
        sb.AppendLine(change_message((New frmOptions).Text, changecount))
        changesum += changecount

        'ファイル'{0}'に {1}点の書き出しを行いました。
        sb.AppendLine(String.Format(My.Resources.MsgExported, othermaster._MasterTablesFilePath, changesum))

        If Not othermaster.Save() Then
            '指定されたファイル'{0}'への保存ができませんでした。
            msg = String.Format(My.Resources.WarningFileSaveError, othermaster._MasterTablesFilePath)
            Return False
        Else
            msg = sb.ToString
            Return True
        End If
    End Function
#End Region

#Region "基本値/Basics"
    '設定ファイルとして使える状態にする。更新した場合はTrueを返すだけ。_IsDirtyは変えない。
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
    '設定ファイルとして使える状態にする。更新した場合はTrueを返すだけ。_IsDirtyは変えない。
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
            If Not isSetAnyIfNone Then
                Return Nothing
            End If
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

    Private Function importBandTypeTable(ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean, ByVal changedBandTypeList As List(Of String)) As Integer
        Dim table As tblバンドの種類DataTable = _dstMasterTables.Tables("tblバンドの種類")
        Dim changecount As Integer = 0

        'バンドの種類名
        Dim otherBandTypeNames() As String = othermaster.GetBandTypeNames()
        'テーブルの現点数とファイル点数
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Basic, "LogImportTable", table.TableName, GetBandTypeNames().Count, otherBandTypeNames.Count)

        For Each otherBandTypeName As String In otherBandTypeNames
            Dim result As ImportResult = importBandType(otherBandTypeName, table, clsLog.LogLevel.Basic,
                                                        othermaster, isOverWrite)

            If {ImportResult.AddToThis, ImportResult.UpdateThis, ImportResult.KeepThisModify}.Contains(result) Then
                changecount += 1
            End If
            If {ImportResult.AddToThis, ImportResult.UpdateThis, ImportResult.SameNoAction}.Contains(result) Then
                changedBandTypeList.Add(otherBandTypeName)
            End If
        Next

        If 0 < changecount Then
            table.AcceptChanges()
            IsDirty = True
        End If

        Return changecount
    End Function

    Private Function importBandType(ByVal bandTypeName As String, ByVal table As tblバンドの種類DataTable, ByVal level As clsLog.LogLevel,
                                    ByVal otherMaster As clsMasterTables, ByVal isOverWrite As Boolean) As ImportResult

        Dim otherBandTypeRedord As clsDataRow = otherMaster.GetBandTypeRecord(bandTypeName, False)
        If otherBandTypeRedord Is Nothing Then
            Return ImportResult.NoOtherRecord
        End If

        Dim thisBandTypeRedord As clsDataRow = GetBandTypeRecord(bandTypeName, False)
        If thisBandTypeRedord Is Nothing Then
            'thisにないので追加
            Dim newDataRow As New clsDataRow(table.NewRow)
            newDataRow.SetValuesFrom(otherBandTypeRedord)
            table.Rows.Add(newDataRow.DataRow)
            '- 追加
            g_clsLog.LogResourceMessage(level, "LogImportAdd", otherBandTypeRedord)
            Return ImportResult.AddToThis

        ElseIf isOverWrite Then
            '両方にある。入れ替え
            If Not thisBandTypeRedord.Equals(otherBandTypeRedord) Then
                '- 変更前
                g_clsLog.LogResourceMessage(level, "LogImportBefore", thisBandTypeRedord)
                thisBandTypeRedord.SetValuesFrom(otherBandTypeRedord)
                thisBandTypeRedord.DataRow.AcceptChanges()
                '- 変更後
                g_clsLog.LogResourceMessage(level, "LogImportAfter", thisBandTypeRedord)
                Return ImportResult.UpdateThis
            Else
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", thisBandTypeRedord)
                Return ImportResult.SameNoAction
            End If

        Else
            '両方にある。上書きしないが色のみ追加

            ' - 同名あり 既存を保持
            g_clsLog.LogResourceMessage(level, "LogImportExistSkip", thisBandTypeRedord)

            Dim strThis As String = thisBandTypeRedord.Value("f_s色リスト")
            Dim strOther As String = otherBandTypeRedord.Value("f_s色リスト")
            If clsSelectBasics.AddColorString(strThis, strOther) Then
                thisBandTypeRedord.Value("f_s色リスト") = strThis
                '- 色の追加
                g_clsLog.LogResourceMessage(level, "LogImportAddColor", strThis)
                Return ImportResult.KeepThisModify
            Else
                Return ImportResult.KeepThisSkip
            End If
        End If

    End Function


#End Region

#Region "編みかた/Pattern"
    '設定ファイルとして使える状態にする。更新した場合はTrueを返すだけ。_IsDirtyは変えない。
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
                                    Optional ByVal is概算用 As Boolean = False,
                                    Optional ByVal isAll As Boolean = False) As String()

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
        ElseIf isAll Then
            'テーブル全て
            res = (From row As tbl編みかたRow In table
                   Select PatternName = row.f_s編みかた名
                   Order By PatternName).Distinct.ToList
        Else
            'そのEXEの側面用の全て
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

    Private Function importPatternTable(ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As Integer
        Dim table As tbl編みかたDataTable = _dstMasterTables.Tables("tbl編みかた")
        Dim changecount As Integer = 0

        Dim otherPatternNames() As String = othermaster.GetPatternNames(False, False, False, True)

        'テーブルの現点数とファイル点数
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Basic, "LogImportTable", table.TableName, GetPatternNames(False, False, False, True).Count, otherPatternNames.Count)

        For Each otherPatternName As String In otherPatternNames
            Dim result As ImportResult = importPattern(otherPatternName, table, clsLog.LogLevel.Basic,
                                                        othermaster, isOverWrite)
            If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                changecount += 1
            End If
        Next

        If 0 < changecount Then
            table.AcceptChanges()
            IsDirty = True
        End If
        Return changecount
    End Function

    Private Function importPattern(ByVal patternName As String, ByVal table As tbl編みかたDataTable, ByVal level As clsLog.LogLevel,
                                   ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As ImportResult

        Dim otherPatternGroup As clsGroupDataRow = othermaster.GetPatternRecordGroup(patternName)
        If otherPatternGroup Is Nothing OrElse Not otherPatternGroup.IsValid OrElse otherPatternGroup.Count = 0 Then
            Return ImportResult.NoOtherRecord
        End If

        Dim thisPatternGroup As clsGroupDataRow = GetPatternRecordGroup(patternName)
        If thisPatternGroup Is Nothing OrElse Not thisPatternGroup.IsValid OrElse thisPatternGroup.Count = 0 Then
            'ないので追加する
            For Each drow As clsDataRow In otherPatternGroup
                Dim newDataRow As New clsDataRow(table.NewRow)
                newDataRow.SetValuesFrom(drow)
                table.Rows.Add(newDataRow.DataRow)
            Next
            '- 追加
            g_clsLog.LogResourceMessage(level, "LogImportAdd", otherPatternGroup)
            Return ImportResult.AddToThis

        ElseIf isOverWrite Then
            '上書きあり
            If Not thisPatternGroup.Equals(otherPatternGroup) Then
                '両方にある。入れ替え

                '- 変更前
                g_clsLog.LogResourceMessage(level, "LogImportBefore", thisPatternGroup)

                '一旦削除
                For Each drow As clsDataRow In thisPatternGroup
                    table.Rows.Remove(drow.DataRow)
                Next
                table.AcceptChanges()
                '改めて追加
                For Each drow As clsDataRow In otherPatternGroup
                    Dim newDataRow As New clsDataRow(table.NewRow)
                    newDataRow.SetValuesFrom(drow)
                    table.Rows.Add(newDataRow.DataRow)
                Next
                '- 変更後
                g_clsLog.LogResourceMessage(level, "LogImportAfter", otherPatternGroup)
                Return ImportResult.UpdateThis

            Else
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", patternName)
                Return ImportResult.SameNoAction
            End If
        Else
            '上書きなし
            If thisPatternGroup.Equals(otherPatternGroup) Then
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", patternName)
                Return ImportResult.SameNoAction
            Else
                ' - 同名あり 既存を保持
                g_clsLog.LogResourceMessage(level, "LogImportExistSkip", patternName)
                Return ImportResult.KeepThisSkip
            End If
        End If
    End Function


#End Region

#Region "付属品/Options"

#Region "描画位置と形状"

    Public Enum enum描画位置
        i_なし = 0
        i_左下
        i_中心
    End Enum

    Public Enum enum描画形状
        i_横バンド = 0
        i_横線
        i_正方形_辺
        i_長方形_横
        i_円_径
        i_楕円_横径
        i_上半円_径
        i_正方形_周
        i_長方形_周
        i_円_周
        i_楕円_周
        i_上半円_周
    End Enum

    Shared _描画位置table As dstWork.tblEnumDataTable = Nothing
    Shared _描画形状table As dstWork.tblEnumDataTable = Nothing

    Shared Function get描画位置table() As dstWork.tblEnumDataTable
        If _描画位置table Is Nothing Then
            _描画位置table = New dstWork.tblEnumDataTable

            'enum描画位置の文字列
            Dim ary() As String = My.Resources.StringDrawPosition.Split(",")
            If ary IsNot Nothing AndAlso 0 < ary.Count Then
                For i As Integer = 0 To ary.Count - 1
                    Dim rc As dstWork.tblEnumRow = _描画位置table.NewRow
                    rc.Display = ary(i)
                    rc.Value = i
                    _描画位置table.Rows.Add(rc)
                Next
            End If
        End If
        Return _描画位置table
    End Function

    Shared Function get描画位置string(ByVal ival As Integer) As String
        Dim table As dstWork.tblEnumDataTable = get描画位置table()
        Dim rc() As dstWork.tblEnumRow = table.Select(String.Format("Value={0}", ival))
        If rc IsNot Nothing AndAlso 0 < rc.Count Then
            Return rc(0).Display
        End If
        Return Nothing
    End Function

    Shared Function get描画形状table() As dstWork.tblEnumDataTable
        If _描画形状table Is Nothing Then
            _描画形状table = New dstWork.tblEnumDataTable

            'enum描画形状の文字列
            Dim ary() As String = My.Resources.StringDrawType.Split(",")
            If ary IsNot Nothing AndAlso 0 < ary.Count Then
                For i As Integer = 0 To ary.Count - 1
                    Dim rc As dstWork.tblEnumRow = _描画形状table.NewRow
                    rc.Display = ary(i)
                    rc.Value = i
                    _描画形状table.Rows.Add(rc)
                Next
            End If

        End If
        Return _描画形状table
    End Function

    Shared Function get描画形状string(ByVal ival As Integer) As String
        Dim table As dstWork.tblEnumDataTable = get描画形状table()
        Dim rc() As dstWork.tblEnumRow = table.Select(String.Format("Value={0}", ival))
        If rc IsNot Nothing AndAlso 0 < rc.Count Then
            Return rc(0).Display
        End If
        Return Nothing
    End Function
#End Region

    '設定ファイルとして使える状態にする。更新した場合はTrueを返すだけ。_IsDirtyは変えない。
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
    Public Function GetOptionNames(Optional ByVal isAll As Boolean = False) As String()

        Dim fieldNameExe As String = "f_b" & g_enumExeName.ToString
        Dim table As tbl付属品DataTable = _dstMasterTables.Tables("tbl付属品")
        Dim res
        If isAll Then
            res = (From row As tbl付属品Row In table
                   Select OptionName = row.f_s付属品名
                   Order By OptionName).Distinct.ToList()
        Else
            res = (From row As tbl付属品Row In table
                   Where row(fieldNameExe)
                   Select OptionName = row.f_s付属品名
                   Order By OptionName).Distinct.ToList()
        End If

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
                Dim len As Double = length * Me.Value("f_d長さ比率対ひも1") + Me.Value("f_d長さ加減対ひも1")
                Return If(len < 0, 0, len) 'f_d長さ加減対ひも1 のマイナス設定対策
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

    Private Function importOptionsTable(ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As Integer
        Dim table As tbl付属品DataTable = _dstMasterTables.Tables("tbl付属品")
        Dim changecount As Integer = 0

        Dim otherOptionNames() As String = othermaster.GetOptionNames(True)

        'テーブルの現点数とファイル点数
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Basic, "LogImportTable", table.TableName, GetOptionNames(True).Count, otherOptionNames.Count)

        For Each otherOptionName As String In otherOptionNames
            Dim result As ImportResult = importOption(otherOptionName, table, clsLog.LogLevel.Basic,
                                                        othermaster, isOverWrite)
            If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                changecount += 1
            End If
        Next

        If 0 < changecount Then
            table.AcceptChanges()
            IsDirty = True
        End If
        Return changecount
    End Function

    Private Function importOption(ByVal otherOptionName As String, ByVal table As tbl付属品DataTable, ByVal level As clsLog.LogLevel,
                                  ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As ImportResult

        Dim otherOptionGroup As clsGroupDataRow = othermaster.GetOptionRecordGroup(otherOptionName)
        If otherOptionGroup Is Nothing OrElse Not otherOptionGroup.IsValid OrElse otherOptionGroup.Count = 0 Then
            Return ImportResult.NoOtherRecord
        End If

        Dim thisOptionGroup As clsGroupDataRow = GetOptionRecordGroup(otherOptionName)
        If thisOptionGroup Is Nothing OrElse Not thisOptionGroup.IsValid OrElse thisOptionGroup.Count = 0 Then
            'ないので追加する
            For Each drow As clsDataRow In otherOptionGroup
                Dim newDataRow As New clsDataRow(table.NewRow)
                newDataRow.SetValuesFrom(drow)
                table.Rows.Add(newDataRow.DataRow)
            Next
            '- 追加
            g_clsLog.LogResourceMessage(level, "LogImportAdd", otherOptionGroup)
            Return ImportResult.AddToThis

        ElseIf isOverWrite Then
            If Not thisOptionGroup.Equals(otherOptionGroup) Then
                '両方にある。入れ替え

                '- 変更前
                g_clsLog.LogResourceMessage(level, "LogImportBefore", thisOptionGroup)

                '一旦削除
                For Each drow As clsDataRow In thisOptionGroup
                    table.Rows.Remove(drow.DataRow)
                Next
                table.AcceptChanges()
                '改めて追加
                For Each drow As clsDataRow In otherOptionGroup
                    Dim newDataRow As New clsDataRow(table.NewRow)
                    newDataRow.SetValuesFrom(drow)
                    table.Rows.Add(newDataRow.DataRow)
                Next
                '- 変更後
                g_clsLog.LogResourceMessage(level, "LogImportAfter", otherOptionGroup)
                Return ImportResult.UpdateThis

            Else
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", otherOptionName)
                Return ImportResult.SameNoAction
            End If
        Else
            If thisOptionGroup.Equals(otherOptionGroup) Then
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", otherOptionName)
                Return ImportResult.SameNoAction
            End If
            ' - 同名あり 既存を保持
            g_clsLog.LogResourceMessage(level, "LogImportExistSkip", otherOptionName)
            Return ImportResult.KeepThisSkip
        End If

    End Function
#End Region

#Region "描画色/Color"

    Class clsColorRecordSet

        Private Const AlfaFrameDefault As Integer = 255 'Solid
        Private Const AlfaLaneDefault As Integer = 100 '少し薄い色

        Public BaseColor As Drawing.Color = Color.Empty  '描画色　(省略値と記号)
        Public FramePenColor As Drawing.Color = Color.Empty  '線色(外枠)
        Public LanePenColor As Drawing.Color = Color.Empty '中線色
        Public BrushAlfaColor As Drawing.Color = Color.Empty 'Alfa塗りつぶし色

        Public FramePenWidth As Single = 0  '外枠ペンの幅
        Public LanePenWidth As Single = 0  '中線ペンの幅

        'レコード値の保持
        Public Name As String '色名
        Public BandTypeName As String 'バンドの種類名
        Public Product As String '製品情報
        Public Appendix As String '備考

        Public ReadOnly Property IsEmptyBandType As Boolean
            Get
                Return BandTypeName = CommonColorBandType
            End Get
        End Property


        Sub New(ByVal row As tbl描画色Row)
            Name = row.f_s色
            Appendix = row.f_s備考
            BandTypeName = row.f_sバンドの種類名
            Product = row.f_s製品情報

            '基本色と塗りつぶし色
            BaseColor = RgbColor(row.f_i赤, row.f_i緑, row.f_i青)
            If Not row.Isf_i透明度Null AndAlso 0 < row.f_i透明度 Then
                BrushAlfaColor = RgbColor(row.f_i赤, row.f_i緑, row.f_i青, row.f_i透明度)
            End If

            '線幅・中線幅
            If row.Isf_d線幅Null Then
                FramePenWidth = 1 'デフォルト1
            ElseIf row.f_d線幅 <= 0 Then
                FramePenWidth = 0 '明示的にゼロ指定
            Else
                FramePenWidth = row.f_d線幅
            End If
            If Not row.Isf_d中線幅Null AndAlso 0 < row.f_d中線幅 Then
                LanePenWidth = row.f_d中線幅
            End If

            '線色
            If IsDBNull(row.f_s線色) Then
                FramePenColorString = Nothing
            Else
                FramePenColorString = row.f_s線色.ToString
            End If

            '中線色
            If IsDBNull(row.f_s中線色) Then
                LanePenColorString = Nothing
            Else
                LanePenColorString = row.f_s中線色.ToString
            End If
        End Sub

        Public Function ToRow(ByVal row As tbl描画色Row) As Boolean
            If row Is Nothing Then
                Return False
            End If
            row.f_i赤 = BaseColor.R
            row.f_i緑 = BaseColor.G
            row.f_i青 = BaseColor.B
            row.f_i透明度 = BrushAlfaColor.A
            row.f_d線幅 = FramePenWidth
            row.f_s線色 = FramePenColorString
            row.f_d中線幅 = LanePenWidth
            row.f_s中線色 = LanePenColorString

            row.f_s製品情報 = Product
            row.f_sバンドの種類名 = BandTypeName
            row.f_s色 = Name
            row.f_s備考 = Appendix
            Return True
        End Function

        '外枠描画色文字列 ※描画色セット後使用可
        Private Property FramePenColorString As String
            Get
                If BaseColor.R = FramePenColor.R AndAlso BaseColor.G = FramePenColor.G AndAlso BaseColor.B = FramePenColor.B Then
                    If FramePenColor.A = AlfaFrameDefault Then
                        Return String.Empty
                    Else
                        Return FramePenColor.A.ToString
                    End If
                End If
                Return String.Format("{0},{1},{2},{3}", FramePenColor.A, FramePenColor.R, FramePenColor.G, FramePenColor.B)
            End Get
            Set(value As String)
                FramePenColor = Color.FromArgb(AlfaFrameDefault, BaseColor)
                If String.IsNullOrWhiteSpace(value) Then Exit Property

                Dim ary() As String = value.Split(",")
                If ary.Length < 1 Then Exit Property
                Dim alfa As Integer
                If Not Integer.TryParse(ary(0), alfa) OrElse alfa < 0 OrElse MaxRgbValue < alfa Then Exit Property
                If ary.Length = 1 Then
                    FramePenColor = Color.FromArgb(alfa, BaseColor)
                    Exit Property
                ElseIf ary.Length <> 4 Then
                    Exit Property
                End If
                Dim r As Integer
                If Not Integer.TryParse(ary(1), r) OrElse r < 0 OrElse MaxRgbValue < r Then Exit Property
                Dim g As Integer
                If Not Integer.TryParse(ary(2), g) OrElse g < 0 OrElse MaxRgbValue < g Then Exit Property
                Dim b As Integer
                If Not Integer.TryParse(ary(3), b) OrElse b < 0 OrElse MaxRgbValue < b Then Exit Property
                FramePenColor = Color.FromArgb(alfa, r, g, b)
            End Set
        End Property

        '中線描画色文字列 ※描画色セット後使用可
        Private Property LanePenColorString As String
            Get
                If BaseColor.R = LanePenColor.R AndAlso BaseColor.G = LanePenColor.G AndAlso BaseColor.B = LanePenColor.B Then
                    If LanePenColor.A = AlfaLaneDefault Then
                        Return String.Empty
                    Else
                        Return LanePenColor.A.ToString
                    End If
                End If
                Return String.Format("{0},{1},{2},{3}", LanePenColor.A, LanePenColor.R, LanePenColor.G, LanePenColor.B)
            End Get
            Set(value As String)
                LanePenColor = Color.FromArgb(AlfaLaneDefault, BaseColor)
                If String.IsNullOrWhiteSpace(value) Then Exit Property

                Dim ary() As String = value.Split(",")
                If ary.Length < 1 Then Exit Property
                Dim alfa As Integer
                If Not Integer.TryParse(ary(0), alfa) OrElse alfa < 0 OrElse MaxRgbValue < alfa Then Exit Property
                If ary.Length = 1 Then
                    LanePenColor = Color.FromArgb(alfa, BaseColor)
                    Exit Property
                ElseIf ary.Length <> 4 Then
                    Exit Property
                End If
                Dim r As Integer
                If Not Integer.TryParse(ary(1), r) OrElse r < 0 OrElse MaxRgbValue < r Then Exit Property
                Dim g As Integer
                If Not Integer.TryParse(ary(2), g) OrElse g < 0 OrElse MaxRgbValue < g Then Exit Property
                Dim b As Integer
                If Not Integer.TryParse(ary(3), b) OrElse b < 0 OrElse MaxRgbValue < b Then Exit Property
                LanePenColor = Color.FromArgb(alfa, r, g, b)
            End Set
        End Property

        '描画されない値(#52)
        Public ReadOnly Property IsNoDrawing
            Get
                Return BrushAlfaColor = Color.Empty AndAlso
                    FramePenWidth = 0 AndAlso LanePenWidth = 0 AndAlso
                    BaseColor.R = MaxRgbValue AndAlso BaseColor.G = MaxRgbValue AndAlso BaseColor.B = MaxRgbValue
            End Get
        End Property

        '色の値のみを比較
        Public Function IsSameColor(other As clsColorRecordSet) As Boolean
            Return BrushAlfaColor = other.BrushAlfaColor AndAlso LanePenColor = other.LanePenColor AndAlso
                FramePenColor = other.FramePenColor AndAlso BaseColor = other.BaseColor AndAlso
            FramePenWidth = other.FramePenWidth AndAlso LanePenWidth = other.LanePenWidth
        End Function

        '色名とバンドの種類以外を比較
        Public Function IsSameValue(other As clsColorRecordSet) As Boolean
            Return IsSameColor(other) AndAlso Product = other.Product AndAlso Appendix = other.Appendix
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0}{1}({2}):R({3}) G({4}) B({5}) Alfa({6}) Frame({7}:{8}) Lane({9}:{10}) [{11}]{12}",
                                 IIf(IsNoDrawing, "-", "+"),
                                 Name, BandTypeName, BaseColor.R, BaseColor.G, BaseColor.B, BrushAlfaColor.A,
                                 FramePenWidth, FramePenColorString, LanePenWidth, LanePenColorString, Product, Appendix)
        End Function
    End Class

    '設定ファイルとして使える状態にする。更新した場合はTrueを返すだけ。_IsDirtyは変えない。
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

    '色とバンド種の選択条件
    Friend Shared Function CondColorBandType(ByVal color As String, ByVal bandtype As String) As String
        If bandtype Is Nothing Then
            Return String.Format("f_s色='{0}' AND f_sバンドの種類名='{1}'", color, CommonColorBandType)
        Else
            Return String.Format("f_s色='{0}' AND f_sバンドの種類名='{1}'", color, bandtype)
        End If
    End Function

    '色・バンド種・バンド種一致を指定して描画色取得(#42) なければNothing
    Public Function GetColorRecordSet(ByVal color As String, ByVal bandType As String, ByVal isSpecifyBand As Boolean) As clsColorRecordSet
        Dim table As tbl描画色DataTable = _dstMasterTables.Tables("tbl描画色")
        If table.Rows.Count = 0 OrElse String.IsNullOrWhiteSpace(color) Then
            Return Nothing
        End If

        '色とバンドの種類で検索
        Dim rows() As tbl描画色Row = table.Select(CondColorBandType(color, bandType))
        If 0 < rows.Count Then
            Return New clsColorRecordSet(rows(0)) '1点のはず
        End If

        'バンドの種類を特定するか
        If isSpecifyBand Then
            '他のバンドの種類の色は参照しない
            Return Nothing

        Else
            '共通色
            rows = table.Select(CondColorBandType(color, Nothing))
            If 0 < rows.Count Then
                Return New clsColorRecordSet(rows(0)) '1点のはず
            End If

            '他のバンドの種類の色は参照しない
            Return Nothing
        End If
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

    Public Function GetColorNames(Optional ByVal bandTypeName As String = "*") As String()
        Dim table As tbl描画色DataTable = _dstMasterTables.Tables("tbl描画色")
        Dim res
        If bandTypeName Is Nothing Then
            res = (From row As tbl描画色Row In table
                   Where row.f_sバンドの種類名 = CommonColorBandType
                   Select ColorName = row.f_s色
                   Order By ColorName).Distinct.ToList
        ElseIf bandTypeName = "*" Then
            res = (From row As tbl描画色Row In table
                   Select ColorName = row.f_s色
                   Order By ColorName).Distinct.ToList
        Else
            res = (From row As tbl描画色Row In table
                   Where row.f_sバンドの種類名 = bandTypeName
                   Select ColorName = row.f_s色
                   Order By ColorName).Distinct.ToList

        End If
        Return res.ToArray
    End Function

    Private Function importColorTable(ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As Integer
        Dim table As tbl描画色DataTable = _dstMasterTables.Tables("tbl描画色")
        Dim changecount As Integer = 0

        'tbl描画色の色名
        Dim otherColorNames() As String = othermaster.GetColorNames()

        '登録されているバンドの種類名
        Dim res = (From row As tblバンドの種類Row In _dstMasterTables.Tables("tblバンドの種類")
                   Select BandTypeName = row.f_sバンドの種類名
                   Order By BandTypeName).Distinct.ToList
        Dim bandTypeNames() As String = res.ToArray


        'テーブルの現点数とファイル点数
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Basic, "LogImportTable", table.TableName, GetColorNames().Count, otherColorNames.Count)

        For Each otherColorName As String In otherColorNames
            Dim count As Integer = importColor(otherColorName, bandTypeNames, table, clsLog.LogLevel.Basic,
                                                        othermaster, isOverWrite)
            If 0 < count Then
                changecount += count
            End If
        Next

        If 0 < changecount Then
            table.AcceptChanges()
            IsDirty = True
        End If
        Return changecount
    End Function

    '指定色のインポート。現有効なバンド種、共通と一致はスキップ
    Private Function importColor(ByVal colorName As String, ByVal bandTypeNames() As String, ByVal table As tbl描画色DataTable, ByVal level As clsLog.LogLevel,
                                 ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As Integer

        Dim changecount As Integer = 0

        '相手の共通色
        Dim otherColorCommon As clsColorRecordSet = othermaster.GetColorRecordSet(colorName, Nothing, True)
        If otherColorCommon IsNot Nothing Then
            Dim result As ImportResult = importColorRecordSet(otherColorCommon, level, isOverWrite)
            If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                changecount += 1
            End If
        End If
        '自分の共通色(既存もしくはインポート結果)
        Dim thisColorCommon As clsColorRecordSet = GetColorRecordSet(colorName, Nothing, True)

        '相手のバンド種
        Dim res = (From row As tbl描画色Row In othermaster._dstMasterTables.Tables("tbl描画色")
                   Where row.f_s色 = colorName And row.f_sバンドの種類名 <> CommonColorBandType
                   Select BandTypeName = row.f_sバンドの種類名
                   Order By BandTypeName).Distinct.ToList
        Dim otherColorBandTypeNames() As String = res.ToArray

        If otherColorBandTypeNames.Length = 0 Then
            Return changecount
        End If

        For Each otherBandtype As String In otherColorBandTypeNames
            '自分が持たないバンド種は除外
            If Not bandTypeNames.Contains(otherBandtype) Then
                '- 対象外
                g_clsLog.LogResourceMessage(level, "LogImportNoTarget", colorName, otherBandtype)
                Continue For
            End If

            Dim otherColor As clsColorRecordSet = othermaster.GetColorRecordSet(colorName, otherBandtype, True)
            If otherColor Is Nothing Then
                Continue For 'あるはずだが念のため
            End If

            '自分の共通色と色が一致すればスキップ
            If thisColorCommon IsNot Nothing AndAlso otherColor.IsSameColor(thisColorCommon) Then
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", colorName, otherBandtype, thisColorCommon.BandTypeName)
                Continue For
            End If

            '自分のバンド色
            Dim thisColor As clsColorRecordSet = GetColorRecordSet(colorName, otherBandtype, True)
            If thisColor IsNot Nothing Then
                'thisにある色
                If otherColor.IsSameValue(thisColor) Then
                    '- 同名あり 既存と一致
                    g_clsLog.LogResourceMessage(level, "LogImportSameSkip", colorName, otherBandtype, thisColor.BandTypeName)
                    Continue For
                End If
            End If

            'なし、同名で不一致、もしくは共通と不一致
            Dim result As ImportResult = importColorRecordSet(otherColor, level, isOverWrite)
            If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                changecount += 1
            End If
        Next

        Return changecount
    End Function

    '指定色・バンドの種類('-'もあり)をそのままインポート
    Private Function importColorRecordSet(ByVal colorRecSet As clsColorRecordSet, ByVal level As clsLog.LogLevel, ByVal isOverWrite As Boolean) As ImportResult
        Dim table As tbl描画色DataTable = _dstMasterTables.Tables("tbl描画色")

        Dim thisColor As clsColorRecordSet = GetColorRecordSet(colorRecSet.Name, colorRecSet.BandTypeName, True)
        If thisColor IsNot Nothing Then
            'thisにある色・バンド種
            If colorRecSet.IsSameValue(thisColor) Then
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", colorRecSet.Name, colorRecSet.BandTypeName)
                Return ImportResult.SameNoAction
            End If
            If Not isOverWrite Then
                ' - 同名あり 既存を保持
                g_clsLog.LogResourceMessage(level, "LogImportExistSkip", colorRecSet.Name, colorRecSet.BandTypeName)
                Return ImportResult.KeepThisSkip
            End If

            '入れ替え
            Dim rows() As tbl描画色Row = table.Select(CondColorBandType(colorRecSet.Name, colorRecSet.BandTypeName))
            If rows.Count < 1 Then
                Return ImportResult.NoOtherRecord '念のため
            End If
            colorRecSet.ToRow(rows(0))
            rows(0).AcceptChanges()
            '- 変更前
            g_clsLog.LogResourceMessage(level, "LogImportBefore", thisColor)
            '- 変更後
            g_clsLog.LogResourceMessage(level, "LogImportAfter", colorRecSet)
            Return ImportResult.UpdateThis
        Else
            'thisにない色・バンド種
            Dim row As tbl描画色Row = table.Newtbl描画色Row
            colorRecSet.ToRow(row)
            table.Rows.Add(row)
            '- 追加
            g_clsLog.LogResourceMessage(level, "LogImportAdd", colorRecSet)
            Return ImportResult.AddToThis
        End If
    End Function

    'エクスポート(現取得色にバンド種を付加する)
    Private Function exportColorBandType(ByVal colorName As String, ByVal bandTypeName As String, ByVal othermaster As clsMasterTables, ByVal level As clsLog.LogLevel, ByVal isOverWrite As Boolean) As ImportResult
        Dim thisColor As clsColorRecordSet = GetColorRecordSet(colorName, bandTypeName, False)
        If thisColor Is Nothing Then
            Return ImportResult.NoOtherRecord 'このバンド種・共通には無し
        End If
        If thisColor.IsEmptyBandType Then
            thisColor.BandTypeName = bandTypeName 'このバンド種にする
        End If
        Return othermaster.importColorRecordSet(thisColor, level, isOverWrite)
    End Function

#End Region

#Region "ゲージ/Gauge"
    '既定値はレコード無し。入力があるレコードのみ生成。

    '設定ファイルとして使える状態にする。更新した場合はTrueを返すだけ。_IsDirtyは変えない。
    Private Function SetAvairableGauge() As Boolean
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")

        '登録されているバンドの種類名
        Dim bandTypeNames() As String = GetBandTypeNames()

        Dim modified As Boolean = False
        For Each r As tblゲージRow In table.Rows
            Dim crow As New clsDataRow(r)
            If Not bandTypeNames.Contains(crow.Value("f_sバンドの種類名")) Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableGauge Delete {0}", crow)
                r.Delete()
                modified = True
                Continue For
            End If

            If crow.SetDefaultForNull() Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableGauge Modify {0}", crow)
                modified = True
            End If
        Next
        Return modified
    End Function

    '指定名のレコードを取得
    Public Function GetBandTypeGauges(ByVal bandtypename As String) As tblゲージRow()
        If String.IsNullOrWhiteSpace(bandtypename) Then
            Return Nothing
        End If
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")
        Dim cond As String = String.Format("f_sバンドの種類名 = '{0}'", bandtypename)
        Return table.Select(cond)
    End Function

    '該当バンドの種類を削除
    Private Function DeleteBandTypeGauges(ByVal bandtypename As String) As Integer
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")
        Dim deleted As Integer = 0

        table.AcceptChanges()
        Dim query = From r In table.AsEnumerable
                    Where r.f_sバンドの種類名 = bandtypename
                    Select r
        For Each r As tblゲージRow In query
            r.Delete()
            deleted += 1
        Next
        table.AcceptChanges()

        Return deleted
    End Function

    '指定名のレコードを入れ替え
    Public Function UpdateBandTypeGauges(ByVal bandtypename As String, ByVal subtable As tblゲージDataTable) As Boolean
        Dim changed As Boolean = False

        '該当バンドの種類を削除
        If 0 < DeleteBandTypeGauges(bandtypename) Then
            changed = True
        End If

        '該当バンドの種類を追加
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")
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

    '(名前で選択された)レコードを入れ替え
    Private Function UpdateBandTypeGauges(ByVal gaugeGroup As clsGroupDataRow) As Boolean
        Dim changed As Boolean = False

        '該当バンドの種類を削除
        If 0 < DeleteBandTypeGauges(gaugeGroup.GetNameValue("f_sバンドの種類名")) Then
            changed = True
        End If

        'レコードを追加
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")
        For Each drow As clsDataRow In gaugeGroup
            Dim newDataRow As New clsDataRow(table.NewRow)
            newDataRow.SetValuesFrom(drow)
            table.Rows.Add(newDataRow.DataRow)
            changed = True
        Next

        If changed Then
            table.AcceptChanges()
            _IsDirty = True
        End If
        Return changed
    End Function

    Private Function importBandTypeGauges(ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean, ByVal changedBandTypeList As List(Of String)) As Integer
        Dim table As tblゲージDataTable = _dstMasterTables.Tables("tblゲージ")
        Dim changecount As Integer = 0

        'テーブルの現点数とファイル点数
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Basic, "LogImportTable", table.TableName, table.Rows.Count, String.Join(",", changedBandTypeList.ToArray))
        If changedBandTypeList.Count = 0 Then
            Return 0
        End If

        For Each bandTypeName As String In changedBandTypeList
            Dim result As ImportResult = importBandTypeGauge(bandTypeName, table, clsLog.LogLevel.Basic,
                                                        othermaster, isOverWrite)
            If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                changecount += 1
            End If
        Next

        If 0 < changecount Then
            table.AcceptChanges()
            IsDirty = True
        End If
        Return changecount
    End Function

    Private Function importBandTypeGauge(ByVal bandTypeName As String, ByVal table As tblゲージDataTable, ByVal level As clsLog.LogLevel,
                                         ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As ImportResult

        Dim otherBandTypeGauges As New clsGroupDataRow(othermaster.GetBandTypeGauges(bandTypeName), "f_i本幅")
        Dim thisBandTypeGauges As New clsGroupDataRow(GetBandTypeGauges(bandTypeName), "f_i本幅")

        'otherレコードなし
        If Not otherBandTypeGauges.IsValid OrElse otherBandTypeGauges.Count = 0 Then
            If Not thisBandTypeGauges.IsValid OrElse thisBandTypeGauges.Count = 0 Then
                'thisレコードもない
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", bandTypeName, "No Gauge")
                Return ImportResult.SameNoAction
            Else
                '- 変更前
                g_clsLog.LogResourceMessage(level, "LogImportBefore", thisBandTypeGauges)
                'thisレコードがあるので削除
                DeleteBandTypeGauges(bandTypeName)
                '- 変更後
                g_clsLog.LogResourceMessage(level, "LogImportAfter", bandTypeName, "No Gauge")
                Return ImportResult.UpdateThis
            End If
        End If

        'otherレコードがある
        If Not thisBandTypeGauges.IsValid OrElse thisBandTypeGauges.Count = 0 Then
            'thisレコードはないので追加するだけ
            UpdateBandTypeGauges(otherBandTypeGauges)
            '- 追加
            g_clsLog.LogResourceMessage(level, "LogImportAdd", otherBandTypeGauges)
            Return ImportResult.AddToThis

        ElseIf isOverWrite Then
            'ともにレコードがある
            If thisBandTypeGauges.Equals(otherBandTypeGauges) Then
                '- 同名あり 既存と一致
                g_clsLog.LogResourceMessage(level, "LogImportSameSkip", bandTypeName)
                Return ImportResult.SameNoAction
            Else
                '- 変更前
                g_clsLog.LogResourceMessage(level, "LogImportBefore", thisBandTypeGauges)

                '入れ替え
                UpdateBandTypeGauges(otherBandTypeGauges)
                '- 変更後
                g_clsLog.LogResourceMessage(level, "LogImportAfter", otherBandTypeGauges)
                Return ImportResult.UpdateThis
            End If
        Else
            ' - 同名あり 既存を保持
            g_clsLog.LogResourceMessage(level, "LogImportExistSkip", bandTypeName)
            Return ImportResult.KeepThisSkip
        End If

    End Function


#End Region

#Region "上下図/UpDown"
    '設定ファイルとして使える状態にする。更新した場合はTrueを返すだけ。_IsDirtyは変えない。
    Private Function SetAvairableUpDown() As Boolean
        Dim table As tbl上下図DataTable = _dstMasterTables.Tables("tbl上下図")
        Dim modified As Boolean = False
        For Each r As tbl上下図Row In table.Rows
            Dim crow As New clsDataRow(r)
            '※スキーマ上Null許容＆値の存在前提で使用
            If crow.SetDefaultForNull() Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "SetAvairableUpDown Modify {0}", crow.ToString)
                modified = True
            End If
        Next
        Return modified
    End Function

    Public Function GetUpDownTableCopy() As tbl上下図DataTable
        Return CType(getCopyDataSetTable("tbl上下図"), tbl上下図DataTable)
    End Function

    Public Function UpdateUpDownTable(ByVal table As tbl上下図DataTable) As Boolean
        If table Is Nothing Then
            Return False 'No Update
        End If

        Dim original As tbl上下図DataTable = _dstMasterTables.Tables("tbl上下図")

        If table.GetChanges IsNot Nothing Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "UpdateUpDownTable:{0}", New clsGroupDataRow(table).ToString)
        End If

        Return updateCopyTableIfModified(original, table)
    End Function


    '上下図名の配列を返す　f_s上下図名順
    Public Function GetUpDownNames() As String()
        Dim table As tbl上下図DataTable = _dstMasterTables.Tables("tbl上下図")
        Dim res = (From row As tbl上下図Row In table
                   Select UpDownName = row.f_s上下図名
                   Order By UpDownName).Distinct.ToList
        Return res.ToArray
    End Function

    '指定名のtbl上下図Row, なければNothing
    Public Function GetUpDownRecord(ByVal updownname As String) As tbl上下図Row
        If String.IsNullOrWhiteSpace(updownname) Then
            Return Nothing
        End If
        Dim table As tbl上下図DataTable = _dstMasterTables.Tables("tbl上下図")

        Dim cond As String = String.Format("f_s上下図名 = '{0}'", updownname)
        Dim rows() As tbl上下図Row = table.Select(cond)
        If rows IsNot Nothing AndAlso 0 < rows.Count Then
            Return rows(0) 'キーなので1点のはず
        Else
            Return Nothing
        End If
    End Function

    '指定名のtbl上下図Rowを作る。既にあればNothing
    Public Function GetNewUpDownRecord(ByVal updownname As String) As tbl上下図Row
        If String.IsNullOrWhiteSpace(updownname) Then
            Return Nothing
        End If
        If GetUpDownRecord(updownname) IsNot Nothing Then
            Return Nothing
        End If

        Dim table As tbl上下図DataTable = _dstMasterTables.Tables("tbl上下図")
        Dim row As tbl上下図Row = table.Newtbl上下図Row
        row.f_s上下図名 = updownname
        table.Rows.Add(row)
        Return row
    End Function

    Public Function SaveUpDownTable(ByVal isAlways As Boolean) As Boolean
        If _dstMasterTables.Tables("tbl上下図").GetChanges IsNot Nothing OrElse isAlways Then
            _IsDirty = True
            _dstMasterTables.Tables("tbl上下図").AcceptChanges()
        End If
        Return False
    End Function

    Private Function importUpDownTable(ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As Integer
        Dim table As tbl上下図DataTable = _dstMasterTables.Tables("tbl上下図")
        Dim changecount As Integer = 0

        Dim otherUpDownNames() As String = othermaster.GetUpDownNames

        'テーブルの現点数とファイル点数
        g_clsLog.LogResourceMessage(clsLog.LogLevel.Basic, "LogImportTable", table.TableName, GetUpDownNames().Count, otherUpDownNames.Count)

        For Each otherUpDownName As String In otherUpDownNames
            Dim result As ImportResult = importUpDown(otherUpDownName, table, clsLog.LogLevel.Basic,
                                                        othermaster, isOverWrite)
            If {ImportResult.AddToThis, ImportResult.UpdateThis}.Contains(result) Then
                changecount += 1
            End If
        Next

        If 0 < changecount Then
            table.AcceptChanges()
            IsDirty = True
        End If
        Return changecount
    End Function

    Private Function importUpDown(ByVal upDownName As String, ByVal table As tbl上下図DataTable, ByVal level As clsLog.LogLevel,
                                  ByVal othermaster As clsMasterTables, ByVal isOverWrite As Boolean) As ImportResult

        Dim otherUpDownRecord As clsDataRow = New clsDataRow(othermaster.GetUpDownRecord(upDownName))
        If Not otherUpDownRecord.IsValid Then
            Return ImportResult.NoOtherRecord
        End If

        Dim thisUpDownRecord As clsDataRow = New clsDataRow(GetUpDownRecord(upDownName))
        If thisUpDownRecord.IsValid Then
            'thisにある上下図

            If isOverWrite Then
                'ともにある。上書き指定

                If otherUpDownRecord.Equals(thisUpDownRecord) Then
                    '- 同名あり 既存と一致
                    g_clsLog.LogResourceMessage(level, "LogImportSameSkip", upDownName)
                    Return ImportResult.SameNoAction

                Else
                    '両方にある。入れ替え
                    '- 変更前
                    g_clsLog.LogResourceMessage(level, "LogImportBefore", thisUpDownRecord)
                    thisUpDownRecord.SetValuesFrom(otherUpDownRecord)
                    thisUpDownRecord.DataRow.AcceptChanges()
                    '- 変更後
                    g_clsLog.LogResourceMessage(level, "LogImportAfter", thisUpDownRecord)
                    Return ImportResult.UpdateThis
                End If

            Else
                '上書きなし

                If otherUpDownRecord.Equals(thisUpDownRecord) Then
                    '- 同名あり 既存と一致
                    g_clsLog.LogResourceMessage(level, "LogImportSameSkip", upDownName)
                    Return ImportResult.SameNoAction
                Else
                    ' - 同名あり 既存を保持
                    g_clsLog.LogResourceMessage(level, "LogImportExistSkip", upDownName)
                    Return ImportResult.KeepThisSkip
                End If

            End If
        Else
            'thisにない上下図
            Dim newDataRow As New clsDataRow(table.NewRow)
            newDataRow.SetValuesFrom(otherUpDownRecord)

            table.Rows.Add(newDataRow.DataRow)
            '- 追加
            g_clsLog.LogResourceMessage(level, "LogImportAdd", otherUpDownRecord)
            Return ImportResult.AddToThis
        End If
    End Function


#End Region


End Class
