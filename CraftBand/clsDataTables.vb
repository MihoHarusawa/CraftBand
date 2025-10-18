Imports System.Windows.Forms
Imports CraftBand.clsUpDown
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables

Public Class clsDataTables

    'ファイルの拡張子(#102)
    Public Const DataExtention As String = ".cbmesh"
    Const DataExtention0 As String = ".xml" '旧バージョン互換

    Public Shared Function IsDataExtention(ByVal fpath As String) As Boolean
        Dim ext As String = IO.Path.GetExtension(fpath)
        If ext.Equals(DataExtention, StringComparison.OrdinalIgnoreCase) OrElse
         ext.Equals(DataExtention0, StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If
        Return False
    End Function


    '縁のf_i番号値(1点のみ)
    Public Const cHemNumber As Integer = 999
    '裏側の開始位置(tbl縦横展開)
    Public Const cBackPosition As Integer = 1001


    Dim _dstDataTables As dstDataTables

    'ファイルエラー文字列
    Public ReadOnly Property LastError As String

    '*画面コントロールに展開
    'tbl目標寸法は1レコードのみ
    Public Property p_row目標寸法 As clsDataRow 'tbl目標寸法Row

    'tbl底_縦横は1レコードのみ
    Public Property p_row底_縦横 As clsDataRow 'tbl底_縦横Row

    '*テーブルそのままを表示・編集
    'tbl側面
    Public ReadOnly Property p_tbl側面 As tbl側面DataTable
        Get
            Return _dstDataTables.Tables("tbl側面")
        End Get
    End Property
    '"f_d周長比率対底の周" 定義ミスでstringになっています。使用注意。

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

    'tbl差しひも
    Public ReadOnly Property p_tbl差しひも As tbl差しひもDataTable
        Get
            Return _dstDataTables.Tables("tbl差しひも")
        End Get
    End Property

    '*必要に応じてワークテーブルと転送
    'tbl縦横展開
    Public ReadOnly Property p_tbl縦横展開 As tbl縦横展開DataTable
        Get
            Return _dstDataTables.Tables("tbl縦横展開")
        End Get
    End Property

    'tblひも上下
    Public ReadOnly Property p_tblひも上下 As tblひも上下DataTable
        Get
            Return _dstDataTables.Tables("tblひも上下")
        End Get
    End Property

    'テーブル識別値(任意レコード数タイプ)
    Public Enum enumDataID
        _tbl底_楕円 = 0
        _tbl側面 = 1
        _tbl追加品 = 2
        _tbl縦横展開 = 3
        _tbl差しひも = 4
        _tblひも上下 = 5
    End Enum
    Const enumDataIdCount As Integer = 6

    Public ReadOnly Property p_tbl(ByVal id As enumDataID) As DataTable
        Get
            Select Case id
                Case enumDataID._tbl底_楕円
                    Return p_tbl底_楕円
                Case enumDataID._tbl側面
                    Return p_tbl側面
                Case enumDataID._tbl追加品
                    Return p_tbl追加品
                Case enumDataID._tbl縦横展開
                    Return p_tbl縦横展開
                Case enumDataID._tbl差しひも
                    Return p_tbl差しひも
                Case enumDataID._tblひも上下
                    Return p_tblひも上下
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    '*フィールド値
    'tbl底_縦横
    Public Enum enum最上と最下の短いひも
        i_なし = 0
        i_同じ幅 = 1
        i_異なる幅 = 2
    End Enum

    Public Enum enumコマ上側の縦ひも
        i_どちらでも = 0
        i_左側 = 1
        i_右側 = 2
    End Enum

    'Hexagon
    Public Enum enum織りタイプ
        i_なし = -1
        i_3すくみ = 0
        i_3軸織 = 1
        i_単麻の葉 = 2
    End Enum
    'Mesh
    Public Enum enum配置タイプ
        i_縦横 = 0 '直角に交差(横ひも・縦ひも)
        i_放射状 = 1 '放射状に置く(縦ひも)
        i_輪弧 = 2  '輪弧に置く(縦ひも)
    End Enum

    'tbl縦横展開
    'カテゴリー出力順
    <Flags()>
    Public Enum enumひも種
        i_横 = &H100
        i_縦 = &H200
        i_斜め = &H400
        i_45度 = &H800
        i_315度 = &H1000
        i_側面 = &H2000

        i_60度 = &H800   'ビット兼用
        i_120度 = &H1000 'ビット兼用

        i_長い = &H10
        i_短い = &H20
        i_最上と最下 = &H40
        i_補強 = &H80
        i_すき間 = &H1
    End Enum
    '描画種
    Public Enum enum描画種
        i_指定なし = 0
        i_重ねる
    End Enum

    'tbl差しひも
    Public Enum enum配置面
        i_なし = 0
        i_底面 = 1
        i_側面 = 2
        i_全面 = 3
    End Enum

    'Square
    Public Enum enum角度
        i_0度 = 0

        i_45度 = 1   '1/1系
        i_90度 = 2
        i_135度 = 3

        i_18度 = 4   '1/3系
        i_72度 = 5
        i_108度 = 6
        i_162度 = 7
    End Enum

    'Hexagon
    Public Enum enum角度Hex
        i_0度H = 0
        i_30度H = 1
        i_60度H = 2
        i_90度H = 3
        i_120度H = 4
        i_150度H = 5
    End Enum


    Public Enum enum中心点
        i_目の中央 = 0
        i_ひも中央 = 1
    End Enum

    Public Enum enum差し位置
        i_おもて = 0
        i_うら = 1
        i_とも編み = 2 '#94
    End Enum

    Sub New()
        _dstDataTables = New dstDataTables
        Clear()
    End Sub

    Sub New(ByVal ref As clsDataTables)
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

        '#87
        Dim i基本のひも幅 As String = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_i基本のひも幅")
        If i基本のひも幅 = 0 OrElse g_clsSelectBasics.p_i本幅 < i基本のひも幅 Then
            i基本のひも幅 = g_clsSelectBasics.p_i本幅
        End If
        p_row目標寸法.Value("f_i基本のひも幅") = i基本のひも幅

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "SetDefaultValue {0}", p_row目標寸法.ToString)

        p_row底_縦横.SetDefaultAll()

        Select Case g_enumExeName
            Case enumExeName.CraftBandMesh
                p_row底_縦横.Value("f_i長い横ひも") = g_clsSelectBasics.p_i本幅
                p_row底_縦横.Value("f_i短い横ひも") = g_clsSelectBasics.p_i本幅
                p_row底_縦横.Value("f_i縦ひも") = g_clsSelectBasics.p_i本幅
                p_row底_縦横.Value("f_b始末ひも区分") = True
                p_row底_縦横.Value("f_d垂直ひも長加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d垂直ひも加算初期値")
                p_row底_縦横.Value("f_b楕円底個別設定") = False
                p_row底_縦横.Value("f_d楕円底円弧の半径加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底円弧の半径加算")
                p_row底_縦横.Value("f_d楕円底周の加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底周の加算")

            Case enumExeName.CraftBandSquare45
                p_row底_縦横.Value("f_b始末ひも区分") = False '縦の補強ひも
                p_row底_縦横.Value("f_dひも間のすき間") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも間のすき間初期値")
                p_row底_縦横.Value("f_dひも長係数") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも長係数初期値")
                p_row底_縦横.Value("f_dひも長加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも長加算初期値")
                p_row底_縦横.Value("f_iひも上下の高さ数") = -1'展開なしの値

            Case enumExeName.CraftBandKnot
                p_row底_縦横.Value("f_b始末ひも区分") = False '使わない
                p_row底_縦横.Value("f_dひも長加算_側面") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d四つ畳みひも長加算初期値")
                p_row底_縦横.Value("f_dひも長加算") = p_row底_縦横.Value("f_dひも長加算_側面") / 2

            Case enumExeName.CraftBandSquare
                p_row底_縦横.Value("f_b始末ひも区分") = False '縦の補強ひも
                p_row底_縦横.Value("f_dひも間のすき間") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも間のすき間初期値")
                p_row底_縦横.Value("f_d垂直ひも長加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d垂直ひも加算初期値")
                p_row底_縦横.Value("f_dひも長加算_側面") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも長加算初期値")
                p_row底_縦横.Value("f_dひも長係数") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも長係数初期値")

            Case enumExeName.CraftBandHexagon
                p_row底_縦横.Value("f_b斜め同数区分") = True
                p_row底_縦横.Value("f_d垂直ひも長加算") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d垂直ひも加算初期値")
                p_row底_縦横.Value("f_dひも長加算_側面") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも長加算初期値")
                p_row底_縦横.Value("f_dひも長係数") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも長係数初期値")
                p_row底_縦横.Value("f_iコマ上側の縦ひも") = enumコマ上側の縦ひも.i_右側 '右綾
                '仮の値
                p_row底_縦横.Value("f_dひも間のすき間") = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_dひも間のすき間初期値")

        End Select

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "SetDefaultValue({0}) {1}", g_enumExeName, p_row底_縦横.ToString)
    End Sub

    '現DataSetのバージョン
    Public ReadOnly Property FormatVersion As String
        Get
            Dim r As tbl目標寸法Row = _dstDataTables.Tables("tbl目標寸法").NewRow
            Return r.f_sバージョン
        End Get
    End Property


    'ファイル読み取り・元データは不変、エラーはLastError(#102)
    Public Shared Function PreLoad(ByVal fpath As String, ByRef ename As enumExeName, ByRef errmsg As String) As dstDataTables
        ename = enumExeName.Nodef
        If Not IO.File.Exists(fpath) Then
            '指定されたファイル'{0}'が存在しません。
            errmsg = String.Format(My.Resources.WarningFileNoExist, fpath)
            Return Nothing
        End If

        'dstDataTablesに読み取る
        Using tmpDataTable As New dstDataTables
            Try
                Dim readmode As System.Data.XmlReadMode = tmpDataTable.ReadXml(fpath, System.Data.XmlReadMode.IgnoreSchema)
            Catch ex As Exception
                '指定されたファイル'{0}'は読み取れませんでした。
                errmsg = String.Format(My.Resources.WarningBadWorkData, fpath)
                g_clsLog.LogException(ex, "clsDataTables.PreLoad", fpath)
                Return Nothing
            End Try

            '各、少なくとも1レコードがあること
            If tmpDataTable.Tables("tbl目標寸法").Rows.Count = 0 OrElse
                tmpDataTable.Tables("tbl底_縦横").Rows.Count = 0 Then
                '指定された'{0}'はデータファイルではありません。
                errmsg = String.Format(My.Resources.WarningNoDataFile, fpath)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsDataTables.PreLoad({0}) No Default Record", fpath)
                Return Nothing
            End If

            'レコードの存在チェック済み
            Dim rowTmp As clsDataRow = New clsDataRow(tmpDataTable.Tables("tbl目標寸法").Rows(0))
            Dim exename As String = rowTmp.Value("f_sEXE名")
            For Each enumExe As enumExeName In GetType(enumExeName).GetEnumValues
                If [String].Compare(exename, enumExe.ToString, True) = 0 Then
                    ename = enumExe
                    Return tmpDataTable.Copy
                End If
            Next

            '指定された'{0}'はデータファイルではありません。
            errmsg = String.Format(My.Resources.WarningNoDataFile, fpath)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsDataTables.PreLoad({0}) Bad ExeName({1})", fpath, exename)
            Return Nothing
        End Using

    End Function

    '現EXE用ファイル読み取り、エラーはLastError・エラー時は元データ不変
    Public Function Load(ByVal fpath As String) As Boolean

        'dstDataTablesに読み取る
        Dim errmsg As String = ""
        Dim ename As enumExeName = enumExeName.Nodef
        Dim tmpDataTable As dstDataTables = PreLoad(fpath, ename, errmsg)
        If tmpDataTable Is Nothing Then
            _LastError = errmsg
            Return False
        End If

        '現EXEと一致
        If ename <> g_enumExeName Then
            '指定されたファイル'{0}'は{1}用です。{2}では使えません。
            _LastError = String.Format(My.Resources.ErrOnotherFormat, fpath, ename.ToString, g_enumExeName.ToString)
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsDataTables.Load({0}) Exe in File'{1}' <> '{2}", fpath, ename.ToString, g_enumExeName.ToString)
            tmpDataTable.Dispose()
            Return False
        End If

        '単位不一致チェック
        Dim rowTmp As clsDataRow = New clsDataRow(tmpDataTable.Tables("tbl目標寸法").Rows(0))
        If 0 <> String.Compare(rowTmp.Value("f_s単位"), g_clsSelectBasics.p_unit設定時の寸法単位.Str, True) Then
            'ログのみ
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsDataTables.Load({0}) Unit in File'{1}' <> '{2}", fpath, rowTmp.Value("f_s単位"), g_clsSelectBasics.p_unit設定時の寸法単位.Str)
        End If

        '入れ替える
        _dstDataTables.Clear()
        _dstDataTables.Dispose()
        _dstDataTables = tmpDataTable.Copy

        p_row目標寸法 = New clsDataRow(_dstDataTables.Tables("tbl目標寸法").Rows(0))
        p_row底_縦横 = New clsDataRow(_dstDataTables.Tables("tbl底_縦横").Rows(0))

        Return True
    End Function

    Public Function Save(ByVal fpath As String) As Boolean
        Try
            'データ環境
            p_row目標寸法.Value("f_s単位") = g_clsSelectBasics.p_unit設定時の寸法単位.Str
            p_row目標寸法.Value("f_sEXE名") = g_enumExeName.ToString
            p_row目標寸法.Value("f_sバージョン") = FormatVersion
            '
            _dstDataTables.WriteXml(fpath, System.Data.XmlWriteMode.WriteSchema)
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.Save", fpath)
            '指定されたファイル'{0}'への保存ができませんでした。
            _LastError = String.Format(My.Resources.WarningFileSaveError, fpath)
            Return False
        End Try
    End Function

    '各テーブルの参照状況
    Private Class tblFeature
        Friend isLane As Boolean 'f_i何本幅 がある
        Friend isColor As Boolean    'f_s色 がある
        Friend isExpandOnly As Boolean '展開時のみ
        Sub New(ByVal lane As Boolean, ByVal color As Boolean, ByVal expand As Boolean)
            isLane = lane
            isColor = color
            isExpandOnly = expand
        End Sub
        ReadOnly Property isLaneColor As Boolean
            Get
                Return isLane OrElse isColor
            End Get
        End Property
    End Class

    Private Shared _Features() As tblFeature = {
        New tblFeature(True, True, False),  '_tbl底_楕円 
        New tblFeature(True, True, False),  '_tbl側面
        New tblFeature(True, True, False),  '_tbl追加品
        New tblFeature(True, True, True),  '_tbl縦横展開
        New tblFeature(True, True, False),  '_tbl差しひも
        New tblFeature(False, False, False)}  '_tblひも上下


    '未定義色をリストアップ(#42)
    Public Function GetUndefColors(ByVal is縦横展開 As Boolean) As String
        Dim colors As New List(Of String)

        '色フィールドを持つテーブル
        For Each id As enumDataID In [Enum].GetValues(GetType(enumDataID))
            If _Features(CType(id, Integer)).isColor AndAlso
                (is縦横展開 OrElse Not _Features(CType(id, Integer)).isExpandOnly) Then
                '未定義色
                For Each row As DataRow In p_tbl(id).Rows
                    Dim coler As String = New clsDataRow(row).Value("f_s色")
                    If Not g_clsSelectBasics.IsExistColor(coler) Then
                        If Not colors.Contains(coler) Then
                            colors.Add(coler)
                        End If
                    End If
                Next
            End If
        Next

        Return String.Join(",", colors.ToArray)
    End Function

    '参照値のチェックと更新(マスタやバンドの種類が変わった時用)
    Public Sub ModifySelected()
        Dim currentMaxLane As Integer = g_clsSelectBasics.p_i本幅

        For Each id As enumDataID In [Enum].GetValues(GetType(enumDataID))
            Dim ttype As tblFeature = _Features(CType(id, Integer))

            '色/何本幅フィールドを持ち、展開によらないテーブル
            If ttype.isLaneColor AndAlso Not ttype.isExpandOnly Then
                '
                For Each row As DataRow In p_tbl(id).Rows
                    Dim drow As New clsDataRow(row)
                    '何本幅
                    If ttype.isLane AndAlso Not drow.IsNull("f_i何本幅") Then
                        Dim lane As Integer = drow.Value("f_i何本幅")
                        If lane < 0 Then
                            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "{0}Row f_i番号({1}) f_i何本幅={2} -> 1", p_tbl(id).TableName, drow.Value("f_i番号"), lane)
                            drow.Value("f_i何本幅") = 1
                        ElseIf currentMaxLane < lane Then
                            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "{0}Row f_i番号({1}) f_i何本幅={2} -> {3}", p_tbl(id).TableName, drow.Value("f_i番号"), lane, currentMaxLane)
                            drow.Value("f_i何本幅") = currentMaxLane
                        ElseIf lane = 0 Then
                            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "{0}Row f_i番号({1}) f_i何本幅=0", p_tbl(id).TableName, drow.Value("f_i番号"))
                        End If
                    End If
                    '色
                    If ttype.isColor AndAlso Not drow.IsNull("f_s色") Then
                        If Not g_clsSelectBasics.IsExistColor(drow.Value("f_s色")) Then
                            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "{0}Row f_i番号({1}) f_s色={2} -> ''", p_tbl(id).TableName, drow.Value("f_i番号"), drow.Value("f_s色"))
                            drow.Value("f_s色") = ""
                        End If
                    End If
                Next
            End If
        Next

        'tbl縦横展開は転送時に処理
    End Sub

    '左右を反転したデータ
    Public Function LeftSideRightData() As clsDataTables
        Dim turn As New clsDataTables(Me)

        '横に並ぶ縦ひもの数
        Dim WCount As Integer = 0
        Dim bExpand As Boolean

        With p_row底_縦横
            bExpand = .Value("f_b展開区分")

            Select Case g_enumExeName
                Case enumExeName.CraftBandMesh
                    'うらおもて未対応
                    'WCount = .Value("f_i縦ひもの本数")
                    Return Nothing

                Case enumExeName.CraftBandSquare45
                    WCount = .Value("f_i横の四角数") + .Value("f_i縦の四角数")
                    turn.p_row底_縦横.Value("f_i横の四角数") = .Value("f_i縦の四角数")
                    turn.p_row底_縦横.Value("f_i縦の四角数") = .Value("f_i横の四角数")
                    If WCount = 0 OrElse Not bExpand Then
                        Return turn
                    End If

                Case enumExeName.CraftBandKnot
                    'うらおもて未対応
                    'WCount = .Value("f_i縦の四角数")
                    Return Nothing

                Case enumExeName.CraftBandSquare
                    WCount = p_row底_縦横.Value("f_i縦ひもの本数")
                    If WCount = 0 OrElse Not bExpand Then
                        Return turn
                    End If

                Case enumExeName.CraftBandHexagon
                    WCount = .Value("f_i斜め60度ひも本数")
                    If .Value("f_bひも中心区分") Then
                        turn.p_row底_縦横.Value("f_i左から何番目") = .Value("f_i斜め60度ひも本数") - .Value("f_i左から何番目") + 1
                        turn.p_row底_縦横.Value("f_i左から何番目2") = .Value("f_i斜め120度ひも本数") - .Value("f_i左から何番目2") + 1
                    Else
                        turn.p_row底_縦横.Value("f_i左から何番目") = .Value("f_i斜め60度ひも本数") - .Value("f_i左から何番目")
                        turn.p_row底_縦横.Value("f_i左から何番目2") = .Value("f_i斜め120度ひも本数") - .Value("f_i左から何番目2")
                    End If
                    turn.p_row底_縦横.Value("f_d左端右端の目") = .Value("f_d左端右端の目2")
                    turn.p_row底_縦横.Value("f_d左端右端の目2") = .Value("f_d左端右端の目")
                    If Not bExpand AndAlso (WCount = .Value("f_i斜め120度ひも本数")) Then
                        Return turn
                    End If

                Case Else
                    Return Nothing

            End Select
        End With

        '入れ替え処理
        If {enumExeName.CraftBandSquare, enumExeName.CraftBandSquare45}.Contains(g_enumExeName) Then

            turn.p_tbl縦横展開.Clear()
            For Each row As tbl縦横展開Row In p_tbl縦横展開
                '縦ひもの左右を入れ替えます(補強ひもは描かないので除外)
                If row.f_iひも種 = enumひも種.i_縦 AndAlso
                    0 <= row.f_iひも番号 AndAlso row.f_iひも番号 <= WCount Then
                    '番号を逆順にする
                    Dim tmp As tbl縦横展開Row = turn.p_tbl縦横展開.Newtbl縦横展開Row
                    tmp.f_iひも種 = enumひも種.i_縦
                    tmp.f_iひも番号 = WCount - row.f_iひも番号 + 1
                    '入力対象: f_i何本幅,f_dひも長加算,f_dひも長加算2, f_s色, f_sメモ
                    getOtherSaved(tmp, row, True) '本幅も転記
                    turn.p_tbl縦横展開.Rows.Add(tmp)

                ElseIf row.f_iひも種 = enumひも種.i_横 Then
                    '#92 横ひも加算の左右入れ替え(補強ひもは描かないので除外)
                    Dim tmp As tbl縦横展開Row = turn.p_tbl縦横展開.Newtbl縦横展開Row
                    tmp.f_iひも種 = enumひも種.i_横
                    tmp.f_iひも番号 = row.f_iひも番号
                    '入力対象: f_i何本幅,f_dひも長加算,f_dひも長加算2, f_s色, f_sメモ
                    getOtherSaved(tmp, row, True) '本幅も転記
                    tmp.f_dひも長加算 = row.f_dひも長加算2
                    tmp.f_dひも長加算2 = row.f_dひも長加算
                    '#98 左右入れ替え
                    tmp.f_b内側区分 = row.f_b内側区分2
                    tmp.f_b内側区分2 = row.f_b内側区分
                    tmp.f_b外側区分 = row.f_b外側区分2
                    tmp.f_b外側区分2 = row.f_b外側区分
                    turn.p_tbl縦横展開.Rows.Add(tmp)

                Else
                    'とりあえずそのまま
                    turn.p_tbl縦横展開.ImportRow(row)
                End If
            Next

        ElseIf {enumExeName.CraftBandHexagon}.Contains(g_enumExeName) Then

            Dim WCount2 As Integer = p_row底_縦横.Value("f_i斜め120度ひも本数")
            turn.p_tbl縦横展開.Clear()
            For Each row As tbl縦横展開Row In p_tbl縦横展開
                Dim tmp As tbl縦横展開Row = turn.p_tbl縦横展開.Newtbl縦横展開Row
                getOtherSaved(tmp, row, True) '本幅も転記

                If row.f_iひも種 = CType(enumひも種.i_横, Integer) Then
                    tmp.f_iひも種 = enumひも種.i_横
                    tmp.f_iひも番号 = row.f_iひも番号
                ElseIf row.f_iひも種 = CType(enumひも種.i_60度, Integer) Then
                    '斜め60度→120度、逆順
                    tmp.f_iひも種 = enumひも種.i_120度
                    tmp.f_iひも番号 = WCount - row.f_iひも番号 + 1
                ElseIf row.f_iひも種 = CType(enumひも種.i_120度, Integer) Then
                    '斜め120度→60度、逆順
                    tmp.f_iひも種 = enumひも種.i_60度
                    tmp.f_iひも番号 = WCount2 - row.f_iひも番号 + 1
                Else
                    '描画対象外
                    tmp = Nothing
                    Continue For
                End If

                '加算の左右入れ替え
                tmp.f_dひも長加算 = row.f_dひも長加算2
                tmp.f_dひも長加算2 = row.f_dひも長加算

                turn.p_tbl縦横展開.Rows.Add(tmp)
            Next
        End If
        turn.p_tbl縦横展開.AcceptChanges()

        '差しひもはクリア
        turn.p_tbl差しひも.Clear()

        Return turn
    End Function

    'テーブル参照
    Function GetTableRows(ByVal id As enumDataID, Optional ByVal cond As String = Nothing, Optional ByVal order As String = Nothing) As DataRow()
        Try
            Return p_tbl(id).Select(cond, order)
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.GetTableRows", id, cond, order)
            Return Nothing
        End Try
    End Function

#Region "編集による更新状態"

    '編集開始時の参照値
    Dim _row目標寸法Start As clsDataRow 'tbl目標寸法Row
    Dim _row底_縦横Start As clsDataRow 'tbl底_縦横Row

    'テーブル更新状態
    Dim _bDataTableChanged(enumDataIdCount - 1) As Boolean

    Public Sub ResetStartPoint()

        _dstDataTables.Tables("tbl目標寸法").AcceptChanges()
        _dstDataTables.Tables("tbl底_縦横").AcceptChanges()

        _row目標寸法Start = Nothing
        _row目標寸法Start = p_row目標寸法.Clone
        _row底_縦横Start = Nothing
        _row底_縦横Start = p_row底_縦横.Clone

        For Each id As enumDataID In [Enum].GetValues(GetType(enumDataID))
            p_tbl(id).AcceptChanges()
            _bDataTableChanged(CType(id, Integer)) = False
        Next
    End Sub

    Public Function IsChangedFromStartPoint()
        For Each id As enumDataID In [Enum].GetValues(GetType(enumDataID))
            If _bDataTableChanged(CType(id, Integer)) Then
                Return True
            End If
        Next
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

    'そのまま編集するタイプのテーブルの更新チェック
    Public Function CheckPoint(ByVal table As DataTable) As Boolean
        If table Is Nothing OrElse table.GetChanges Is Nothing Then
            Return False
        End If

        '対応するテーブルのフラグに変換
        For Each id As enumDataID In [Enum].GetValues(GetType(enumDataID))
            If p_tbl(id).TableName = table.TableName Then
                _bDataTableChanged(CType(id, Integer)) = True
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "{0} Changes={1}", table.TableName, table.GetChanges.Rows.Count)
                table.AcceptChanges()
                Return True
            End If
        Next
        Return False
    End Function
#End Region


#Region "ワークテーブルとtbl縦横展開の転送"
    '※キーはf_iひも種,f_iひも番号 [対象は、f_iひも種のbitを持つレコード]
    '保持するのは入力値:  f_dひも長加算,f_dひも長加算2, f_s色, f_sメモ   f_i何本幅は対応Exeのみ

    'f_i何本幅の保持対象の時True
    Private Function isLoad何本幅() As Boolean
        Return _
        (g_enumExeName = enumExeName.CraftBandSquare) OrElse
                (g_enumExeName = enumExeName.CraftBandSquare45 AndAlso "1.6.0" <= p_row目標寸法.Value("f_sバージョン")) OrElse
                (g_enumExeName = enumExeName.CraftBandMesh AndAlso "1.8.1" <= p_row目標寸法.Value("f_sバージョン")) OrElse
                (g_enumExeName = enumExeName.CraftBandHexagon)
    End Function

    '保持フィールド値を別レコードから取得
    Private Shared Function getOtherSaved(ByVal this As tbl縦横展開Row, ByVal other As tbl縦横展開Row, ByVal isget何本幅 As Boolean) As Boolean
        Try
            '入力対象: f_dひも長加算,f_dひも長加算2, f_s色, f_sメモ, f_i何本幅(引数指定)
            If other Is Nothing Then
                this.f_dひも長加算 = 0
                this.f_dひも長加算2 = 0
                this.f_s色 = ""
                this.f_sメモ = ""
                this.f_b外側区分 = False
                this.f_b外側区分2 = False
                this.f_b内側区分 = False
                this.f_b内側区分2 = False
                this.f_s色2 = ""
                this.f_i描画種 = enum描画種.i_指定なし
                If isget何本幅 Then
                    this.f_i何本幅 = 1
                End If

            Else
                this.f_dひも長加算 = other.f_dひも長加算
                this.f_dひも長加算2 = other.f_dひも長加算2
                '仮の値としてセットしておく
                If this.f_dひも長加算 <> 0 OrElse this.f_dひも長加算2 <> 0 Then
                    this.f_d出力ひも長 = this.f_dひも長 + this.f_dひも長加算 + this.f_dひも長加算2
                End If
                '使用可能色
                If g_clsSelectBasics.IsExistColor(other.f_s色) Then
                    this.f_s色 = other.f_s色
                Else
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "skip tbl縦横展開Row({0}:{1}).f_s色={2}", other.f_sひも名, other.f_iひも番号, other.f_s色)
                End If
                this.f_sメモ = other.f_sメモ
                this.f_b外側区分 = other.f_b外側区分
                this.f_b外側区分2 = other.f_b外側区分2
                this.f_b内側区分 = other.f_b内側区分
                this.f_b内側区分2 = other.f_b内側区分2
                this.f_s色2 = other.f_s色2
                this.f_i描画種 = other.f_i描画種

                'f_i何本幅
                If isget何本幅 Then
                    If other.f_i何本幅 < 0 Then
                        this.f_i何本幅 = 1
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "skip tbl縦横展開Row({0}:{1}).f_i何本幅={2}->1", other.f_sひも名, other.f_iひも番号, other.f_i何本幅)
                    ElseIf g_clsSelectBasics.p_i本幅 < other.f_i何本幅 Then
                        this.f_i何本幅 = g_clsSelectBasics.p_i本幅
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "skip tbl縦横展開Row({0}:{1}).f_i何本幅={2}->{3}", other.f_sひも名, other.f_iひも番号, other.f_i何本幅, g_clsSelectBasics.p_i本幅)
                    Else
                        this.f_i何本幅 = other.f_i何本幅
                    End If
                End If

            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "getOtherSaved")
            Return False
        End Try
    End Function


    'ワークテーブルの編集フィールドにデータベース値をセットする
    '※iひも種はレコード参照には使わない
    Public Function ToTmpTable(ByVal iひも種 As Integer, ByVal tmptable As tbl縦横展開DataTable) As Integer
        If tmptable Is Nothing OrElse tmptable.Rows.Count = 0 OrElse iひも種 = 0 Then
            Return 0
        End If

        Dim count As Integer = 0
        For Each tmp As tbl縦横展開Row In tmptable
            '同じキーレコードを検索
            Dim row As tbl縦横展開Row = Find縦横展開Row(p_tbl縦横展開, tmp.f_iひも種, tmp.f_iひも番号, False) '追加しない
            If row Is Nothing Then
                Continue For
            End If

            getOtherSaved(tmp, row, isLoad何本幅())

            count += 1
        Next
        tmptable.AcceptChanges()
        Return count
    End Function

    'データベース値をワークテーブル値に置換 
    '※iひも種のいずれかビットを持つレコードを削除してから追加する
    Public Function FromTmpTable(ByVal iひも種 As Integer, ByVal tmptable As tbl縦横展開DataTable) As Integer
        If tmptable Is Nothing OrElse tmptable.Rows.Count = 0 OrElse iひも種 = 0 Then
            Return 0
        End If

        'ひも種で指定されたカテゴリーのレコードを削除
        Dim count As Integer = Removeひも種Rows(iひも種)

        'レコードを追加
        For Each tmp As tbl縦横展開Row In tmptable
            p_tbl縦横展開.ImportRow(tmp)
            count += 1
            _bDataTableChanged(CType(enumDataID._tbl縦横展開, Integer)) = True
        Next
        p_tbl縦横展開.AcceptChanges()
        tmptable.AcceptChanges()

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "tbl縦横展開 Add({0}) {1}点", iひも種, tmptable.Rows.Count)
        Return count
    End Function

    'p_tbl縦横展開からひも種で指定されたカテゴリーを削除
    Public Function Removeひも種Rows(ByVal iひも種 As Integer) As Integer
        Dim count As Integer = 0

        p_tbl縦横展開.AcceptChanges()
        Dim query = From r In p_tbl縦横展開.AsEnumerable
                    Where (r.f_iひも種 And iひも種) <> 0
                    Select r

        For Each r As tbl縦横展開Row In query
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "tbl縦横展開 Remove({0},{1})", r.f_sひも名, r.f_iひも番号)
            r.Delete()
            count -= 1
        Next
        If count <> 0 Then
            _bDataTableChanged(CType(enumDataID._tbl縦横展開, Integer)) = True
            p_tbl縦横展開.AcceptChanges()
        End If

        Return count
    End Function

    '指定キーのレコード取得・追加も可能
    Public Shared Function Find縦横展開Row(ByVal table As tbl縦横展開DataTable, ByVal iひも種 As Integer, ByVal iひも番号 As Integer, ByVal addnew As Boolean) As tbl縦横展開Row
        Dim cond As String = String.Format("f_iひも種={0} AND f_iひも番号={1}", iひも種, iひも番号)
        Dim sels() As tbl縦横展開Row = table.Select(cond)
        If sels IsNot Nothing AndAlso 0 < sels.Length Then
            Return sels(0)
        End If
        If addnew Then
            Dim row As tbl縦横展開Row = table.Newtbl縦横展開Row
            row.f_iひも種 = iひも種
            row.f_iひも番号 = iひも番号
            table.Rows.Add(row)
            Return row
        End If
        Return Nothing
    End Function

    'iひも番号より大きいレコードがあれば削除する
    Public Shared Function SetMaxひも番号(ByVal table As tbl縦横展開DataTable, ByVal iひも番号 As Integer) As Integer
        Dim cond As String = String.Format("f_iひも番号 > {0}", iひも番号)
        Dim sels() As tbl縦横展開Row = table.Select(cond)
        If sels Is Nothing OrElse sels.Length = 0 Then
            Return 0 '対象なし
        End If
        For Each sel As tbl縦横展開Row In sels
            sel.Delete()
        Next
        Return sels.Length
    End Function

    'iひも番号位置にレコードを追加し、以降を後ろにシフトする
    '追加位置のレコードを返す
    Public Shared Function Insert縦横展開Row(ByVal table As tbl縦横展開DataTable, ByVal iひも種 As enumひも種, ByVal iひも番号 As Integer) As tbl縦横展開Row

        Dim iMaxひも番号 As Integer = 0
        Dim objMax As Object = table.Compute("MAX(f_iひも番号)", String.Format("f_iひも種 = {0}", CType(iひも種, Integer)))
        If Not IsDBNull(objMax) AndAlso 0 < objMax Then
            iMaxひも番号 = objMax
        End If
        If iMaxひも番号 = 0 Then
            Return Nothing '既存がないと追加できません
        End If


        '追加位置
        Dim bandnum As Integer = iひも番号
        If bandnum < 1 Then
            bandnum = 1 '最初の前
        ElseIf iMaxひも番号 < bandnum Then
            bandnum = iMaxひも番号 + 1  '最後の後
        End If

        'レコードを後ろに追加
        Dim add As tbl縦横展開Row = Find縦横展開Row(table, iひも種, iMaxひも番号 + 1, True)
        Dim rowmax As tbl縦横展開Row = Find縦横展開Row(table, iひも種, iMaxひも番号, False)
        If rowmax Is Nothing Then
            add.f_i何本幅 = 1 'あるはずだが
        Else
            add.f_i何本幅 = rowmax.f_i何本幅
        End If
        If iMaxひも番号 < bandnum Then
            Return add
        End If

        Dim currow As tbl縦横展開Row = Nothing

        '追加位置以降を後ろにシフト ※入力対象fieldのみ
        Dim cond As String = String.Format("f_iひも種 = {0}", CType(iひも種, Integer))
        Dim rows() As DataRow = table.Select(cond, "f_iひも番号 DESC")
        If rows Is Nothing OrElse rows.Count = 0 Then
            Return add '追加分はあるはずだが
        End If

        Dim lastrow As tbl縦横展開Row = Nothing
        For Each row As tbl縦横展開Row In rows
            If lastrow Is Nothing Then
                lastrow = row
                Continue For
            End If
            If row.f_iひも番号 < bandnum Then
                currow = row
                Exit For '↓で処理済のはずだが
            End If

            getOtherSaved(lastrow, row, True) 'f_i何本幅 も転記
            If row.f_iひも番号 = bandnum Then
                currow = row
                getOtherSaved(row, Nothing, False) 'f_i何本幅 は上書きしない
                Exit For
            End If
            lastrow = row
        Next

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Insert縦横展開Row currow={0}", New clsDataRow(currow).ToString)
        Return currow
    End Function

    'iひも番号位置のレコードを削除し、以降を前に詰める
    '削除位置のレコードを返す
    Public Shared Function Remove縦横展開Row(ByVal table As tbl縦横展開DataTable, ByVal iひも種 As enumひも種, ByVal iひも番号 As Integer) As tbl縦横展開Row

        Dim iMaxひも番号 As Integer = 0
        Dim objMax As Object = table.Compute("MAX(f_iひも番号)", String.Format("f_iひも種 = {0}", CType(iひも種, Integer)))
        If Not IsDBNull(objMax) AndAlso 0 < objMax Then
            iMaxひも番号 = objMax
        End If
        If iMaxひも番号 = 0 Then
            Return Nothing '既存なし
        End If

        If iひも番号 < 1 OrElse iMaxひも番号 < iひも番号 Then
            Return Nothing '対象外
        End If

        '最終レコードはシフト不要
        If iMaxひも番号 = iひも番号 Then
            Return Find縦横展開Row(table, iひも種, iMaxひも番号, False)
        End If

        Dim currow As tbl縦横展開Row = Nothing

        '削除位置以降を前にシフトする ※入力対象fieldのみ
        Dim cond As String = String.Format("f_iひも種 = {0}", CType(iひも種, Integer))
        Dim rows() As DataRow = table.Select(cond, "f_iひも番号 ASC")
        If rows Is Nothing OrElse rows.Count = 0 Then
            Return Nothing 'あるはずだが
        End If

        Dim lastrow As tbl縦横展開Row = Nothing
        For Each row As tbl縦横展開Row In rows
            If row.f_iひも番号 <= iひも番号 Then
                lastrow = row
                Continue For
            End If
            If lastrow IsNot Nothing Then
                getOtherSaved(lastrow, row, True) '何本幅も転記
                If currow Is Nothing Then
                    currow = lastrow
                End If
            End If
            lastrow = row
        Next

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Remove縦横展開Row currow={0}", New clsDataRow(currow).ToString)
        Return currow
    End Function


#End Region

#Region "p_tblひも上下とclsUpDownの転送"

    '指定面のレコード内容をupdownにセット
    Public Function ToClsUpDown(ByVal updown As clsUpDown) As Boolean
        If updown Is Nothing Then
            Return False
        End If
        Select Case updown.TargetFace
            Case enumTargetFace.Bottom
                Return toClsUpDown(clsUpDown.cBottomNumber, updown)
            Case enumTargetFace.Side12
                Return toClsUpDown(clsUpDown.cSide12Number, updown)
            Case enumTargetFace.Side34
                Return toClsUpDown(clsUpDown.cSide34Number, updown)
            Case Else
                Return False
        End Select
    End Function

    '指定番号のレコード内容をupdownにセット
    Private Function toClsUpDown(ByVal num As Integer, ByVal updown As clsUpDown) As Boolean
        If num < 0 Then
            Return False
        End If
        If num < p_tblひも上下.Rows.Count Then
            updown.FromRecord(p_tblひも上下.Rows(num))
            Return updown.IsValid(False) 'チェックはMatrix
        Else
            Return False
        End If
    End Function

    'updown内容を指定面のレコードにセット
    Public Function FromClsUpDown(ByVal updown As clsUpDown) As Boolean
        If updown Is Nothing Then
            Return False
        End If
        Select Case updown.TargetFace
            Case enumTargetFace.Bottom
                Return fromClsUpDown(clsUpDown.cBottomNumber, updown)
            Case enumTargetFace.Side12
                Return fromClsUpDown(clsUpDown.cSide12Number, updown)
            Case enumTargetFace.Side34
                Return fromClsUpDown(clsUpDown.cSide34Number, updown)
            Case Else
                Return False
        End Select
    End Function

    'updown内容を指定番号のレコードにセット、空き番は空レコード
    Private Function fromClsUpDown(ByVal num As Integer, ByVal updown As clsUpDown) As Boolean
        Dim row As tblひも上下Row
        Do While p_tblひも上下.Rows.Count <= num
            row = p_tblひも上下.Newtblひも上下Row
            p_tblひも上下.Rows.Add(row)
        Loop
        row = p_tblひも上下.Rows(num)

        'レコードにセットする
        Dim ret As Boolean = updown.ToRecord(row)

        p_tblひも上下.AcceptChanges()
        _bDataTableChanged(CType(enumDataID._tblひも上下, Integer)) = True

        Return ret
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
            Return 1 '開始番号
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

    'numberの最初のレコード
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

    'numberのsubnumber最大値　なければ-1
    Shared Function NumberSubMax(ByVal table As DataTable, ByVal number As Integer) As Integer
        Try
            Dim obj As Object = table.Compute("MAX(f_iひも番号)",
                                              String.Format("f_i番号 = {0}", number))
            If IsDBNull(obj) OrElse obj < 1 Then
                Return -1
            End If
            Return obj

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.NumberSubMax")
            Return -1
        End Try
    End Function

    'number,subnumberのレコード
    Shared Function NumberSubRecord(ByVal table As DataTable, ByVal number As Integer, ByVal subnumber As Integer) As DataRow
        Try
            Dim cond As String = String.Format("f_i番号 = {0} AND f_iひも番号 = {1}", number, subnumber)
            Dim rows() As DataRow = table.Select(cond, "f_iひも番号 ASC")
            If rows Is Nothing OrElse rows.Count = 0 Then
                Return Nothing
            End If
            Return rows(0)

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.NumberSubRecord")
            Return Nothing
        End Try
    End Function

    '空き番があれば詰める(#16)
    Shared Function FillNumber(ByVal table As DataTable) As Boolean
        Dim idxseq As Integer = 1 '開始番号
        Try
            Dim res = (From row As DataRow In table
                       Select Idx = row("f_i番号")
                       Order By Idx).Distinct

            'レコード番号順
            For Each idx As Integer In res
                If idx = cHemNumber Then
                    Return True
                End If
                If idx < idxseq Then
                    'ないはず。詰められません
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsDataTables.FillNumber BadNumber={0}", idx)
                    Return False
                ElseIf idxseq < idx Then
                    '全サブレコード
                    For Each row As DataRow In table.Select(String.Format("f_i番号 = {0}", idx))
                        row("f_i番号") = idxseq
                    Next row
                Else
                    '一致はOK
                End If
                '
                idxseq += 1
            Next idx
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsDataTables.FillNumber")
            Return False
        End Try

    End Function

#End Region


End Class
