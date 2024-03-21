Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Public Module mdlDllMain


    Public g_clsMasterTables As clsMasterTables
    Public g_clsSelectBasics As clsSelectBasics
    Public g_clsLog As clsLog

    '実行中のプログラム
    Enum enumExeName
        Nodef = 0
        CraftBandMesh
        CraftBandSquare45
        CraftBandKnot
        CraftBandSquare
        CraftBandHexagon
    End Enum
    Public g_enumExeName As enumExeName = enumExeName.Nodef

    '省略名
    Dim _ShortNames() As String = {"", "Mesh", "Square45", "Knot", "Square", "Hexagon"}
    Public Function GetShortExeName(ByVal exename As enumExeName) As String
        Return _ShortNames(exename)
    End Function

    'DLL共通パラメータ
    Public Class DllParameters
        Public Log As clsLog = Nothing

        Public Property MasterTablesFilePath As String
        Public Property ListOutMark As String

        Public Property Message As String

#Region "前回値の保持"

        Dim _KeyValue As New Dictionary(Of String, String)
        Sub New(ByVal lastdataString As String)
            If String.IsNullOrWhiteSpace(lastdataString) Then
                Exit Sub
            End If
            lastdataString.Replace(vbCrLf, vbLf)
            Dim ary() As String = lastdataString.Split(vbLf)
            For Each str As String In ary
                If String.IsNullOrWhiteSpace(str) Then
                    Continue For
                End If
                Dim eqpos As Integer = str.IndexOf("=")
                If 0 < eqpos Then
                    If _KeyValue.ContainsKey(str.Substring(0, eqpos)) Then
                        '重複は1点目のみ
                        Continue For
                    End If
                    _KeyValue.Add(str.Substring(0, eqpos), str.Substring(eqpos + 1).Trim)
                Else
                    'キーなしはNG
                End If
            Next
        End Sub

        Friend Function GetLastData(ByVal key As String, ByRef str As String) As Boolean
            If Not _KeyValue.ContainsKey(key) Then
                Return False
            End If
            str = _KeyValue(key)
            Return True
        End Function

        Friend Function GetLastData(ByVal key As String, ByRef pos As Size) As Boolean
            Dim str As String = Nothing
            If Not GetLastData(key, str) OrElse String.IsNullOrWhiteSpace(str) Then
                Return False
            End If
            pos.Width = 0
            pos.Height = 0
            Dim ary() As String = str.Split(",")
            If 0 < ary.Length Then
                pos.Width = Val(ary(0))
            End If
            If 1 < ary.Length Then
                pos.Height = Val(ary(1))
            End If
            Return 0 < pos.Width AndAlso 0 < pos.Height
        End Function

        Friend Function GetLastData(ByVal key As String, ByRef int As Integer) As Boolean
            Dim str As String = Nothing
            If Not GetLastData(key, str) OrElse String.IsNullOrWhiteSpace(str) Then
                Return False
            End If
            If Integer.TryParse(str, int) Then
                Return True
            Else
                Return False
            End If
        End Function

        Friend Function SetLastData(ByVal key As String, ByVal str As String) As Boolean
            If _KeyValue.ContainsKey(key) Then
                _KeyValue(key) = str
                Return False 'update
            Else
                _KeyValue.Add(key, str)
                Return True 'add
            End If
        End Function

        Friend Function SetLastData(ByVal key As String, ByVal pos As Size) As Boolean
            Dim str As String = String.Format("{0},{1}", pos.Width, pos.Height)
            Return SetLastData(key, str)
        End Function

        Friend Function SetLastData(ByVal key As String, ByVal int As Integer) As Boolean
            Return SetLastData(key, int.ToString)
        End Function


        Function LastDataString() As String
            Dim sb As New System.Text.StringBuilder
            For Each key As String In _KeyValue.Keys
                sb.AppendFormat("{0}={1}", key, _KeyValue(key)).AppendLine()
            Next
            Return sb.ToString
        End Function
#End Region

    End Class

    Friend __paras As DllParameters
    Friend __encShiftJis As System.Text.Encoding

    Public Function Initialize(ByVal paras As DllParameters) As Boolean
        If paras.Log Is Nothing Then
            '起動に必要なパラメータがありません。
            paras.Message = My.Resources.ErrNoInitializeParameters
            Return False
        End If
        __paras = paras

        'shift_jisを扱えるようにする
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        __encShiftJis = System.Text.Encoding.GetEncoding("shift_jis")

        '*ログとEXE名
        g_clsLog = paras.Log
        Dim exename As String = IO.Path.GetFileNameWithoutExtension(g_clsLog.ExePath)
        For Each enumExe As enumExeName In GetType(enumExeName).GetEnumValues
            If [String].Compare(exename, enumExe.ToString, True) = 0 Then
                g_enumExeName = enumExe
                Exit For
            End If
        Next

        '*設定データ(マスターテーブル)
        g_clsMasterTables = New clsMasterTables 'まだ使えません

        '保存した名前を得る
        Dim masterTablesFilePath As String = __paras.MasterTablesFilePath
        If String.IsNullOrWhiteSpace(masterTablesFilePath) Then
            '名前がない時(初回)
            Dim dlg As New frmBasics
            Dim fname As String = IO.Path.ChangeExtension(clsMasterTables.DefaultFileName, clsMasterTables.MyExtention)
            dlg.SaveFileDialogMasterTable.FileName = IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fname)
            If dlg.SaveFileDialogMasterTable.ShowDialog() <> DialogResult.OK Then
                '設定データを保存するファイルが指定されませんでした。
                paras.Message = My.Resources.ErrMasterTableFileCancel
                Return False
            End If
            masterTablesFilePath = dlg.SaveFileDialogMasterTable.FileName

            If IO.File.Exists(masterTablesFilePath) Then
                '既存ファイルがあれば読み取る
                If Not g_clsMasterTables.LoadFile(masterTablesFilePath, True) Then
                    '指定ファイル'{0}'から設定データを読み取ることができませんでした。
                    paras.Message = My.Resources.ErrReadMasterTableFile
                    Return False
                End If

            Else
                '既存ファイルがない時の初期値
                Dim default_exe As String = IO.Path.ChangeExtension(g_clsLog.ExePath, clsMasterTables.MyExtention)
                Dim default_master As String = IO.Path.ChangeExtension(IO.Path.Combine(IO.Path.GetDirectoryName(g_clsLog.ExePath), clsMasterTables.DefaultFileName), clsMasterTables.MyExtention)
                If g_clsMasterTables.LoadFile(default_exe, False) Then
                    '読めた
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "MasterTables.LoadFile={0}", default_exe)
                ElseIf g_clsMasterTables.LoadFile(default_master, False) Then
                    '読めた
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "MasterTables.LoadFile={0}", default_master)
                Else
                    '読めないので初期値から
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "MasterTables.New={0}", masterTablesFilePath)
                    g_clsMasterTables.SetAvairable() '使える状態にする
                End If
                '名前のみセット
                g_clsMasterTables.MasterTablesFilePath = masterTablesFilePath
            End If

        Else
            '名前がある(再処理)
            '既存ファイルがあれば読み取る
            If Not g_clsMasterTables.LoadFile(masterTablesFilePath, True) Then
                '指定ファイル'{0}'から設定データを読み取ることができませんでした。
                paras.Message = String.Format(My.Resources.ErrReadMasterTableFile, masterTablesFilePath)
                Return False
            End If
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "MasterTables.LoadFile={0}", masterTablesFilePath)
        End If


        '*基本設定 ※g_clsMasterTablesが有効になってから
        g_clsSelectBasics = New clsSelectBasics(__paras.ListOutMark)

        Return True
    End Function

    Public Function Finalize(ByVal isOK As Boolean) As Boolean
        If __paras Is Nothing Then
            Return False
        End If

        Dim ret As Boolean = True
        If isOK Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "LastDataString={0}", __paras.LastDataString)
            If Not g_clsMasterTables.Save() Then
                '設定データのファイル'{0}への保存に失敗しました。
                __paras.Message = String.Format(My.Resources.ErrSaveMasterTableFile, g_clsMasterTables.MasterTablesFilePath)
                ret = False
            End If
            __paras.MasterTablesFilePath = g_clsMasterTables.MasterTablesFilePath

            __paras.ListOutMark = g_clsSelectBasics.p_sリスト出力記号
            g_clsSelectBasics.save()
        Else
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "Skip Saving Master(IsDirty={0}) And SelectBasics", g_clsMasterTables.IsDirty)
        End If
        g_clsSelectBasics = Nothing
        g_clsMasterTables = Nothing

        _frmColorRepeat = Nothing
        _frmColorChange = Nothing

        'g_clsLogはexe側
        Return ret
    End Function






End Module
