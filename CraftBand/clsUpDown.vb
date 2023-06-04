Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstMasterTables

''' <summary>
''' tblひも上下のレコードを展開したクラス
''' </summary>
Public Class clsUpDown
    Public Const cMaxUpdownColumns As Integer = 50 'CheckBoxフィールド数
    Public Const cBottomNumber As Integer = 0 '底のレコード番号
    Public Const cSideNumber As Integer = 1 '側面のレコード番号


    Public IsSide As Boolean 'False=底, True=側面

    Public HorizontalCount As Integer
    Public VerticalCount As Integer
    Public CheckBoxTable As dstWork.tblCheckBoxDataTable = Nothing

    Public Memo As String

    Sub New(ByVal side As Boolean)
        IsSide = side
    End Sub
    Sub New(ByVal side As Boolean, ByVal horizon As Integer, ByVal vert As Integer)
        IsSide = side
        HorizontalCount = horizon
        VerticalCount = vert
    End Sub

    ReadOnly Property IsValid As Boolean
        Get
            '照合は垂直本数のみ
            Return 0 < HorizontalCount AndAlso 0 < VerticalCount AndAlso
                CheckBoxTable IsNot Nothing AndAlso
                CheckBoxTable.Rows.Count = VerticalCount
        End Get
    End Property

    Sub Clear(ByVal isCreateTable As Boolean)
        HorizontalCount = 0
        VerticalCount = 0
        Memo = String.Empty
        If isCreateTable Then
            CheckBoxTable = New dstWork.tblCheckBoxDataTable
        ElseIf CheckBoxTable IsNot Nothing Then
            CheckBoxTable.Rows.Clear()
        End If
    End Sub

    '初期値
    Function Reset() As Boolean
        Try
            Dim isFirstUp As Boolean = IsSide

            HorizontalCount = 2
            VerticalCount = 2

            CheckBoxTable.Rows.Clear()
            Dim r As dstWork.tblCheckBoxRow

            r = CheckBoxTable.NewtblCheckBoxRow
            r.Index = 1
            r.CheckBox01 = isFirstUp
            r.CheckBox02 = Not isFirstUp
            CheckBoxTable.Rows.Add(r)

            r = CheckBoxTable.NewtblCheckBoxRow
            r.Index = 2
            r.CheckBox01 = Not isFirstUp
            r.CheckBox02 = isFirstUp
            CheckBoxTable.Rows.Add(r)

            CheckBoxTable.AcceptChanges()
            Memo = ""
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.Reset")
            Return False
        End Try
    End Function

    '文字列をCheckBoxTableにする
    Private Function updownstrToTable(ByVal str As String) As Boolean
        '現テーブルはクリア
        CheckBoxTable.Rows.Clear()
        If 0 < HorizontalCount AndAlso 0 < VerticalCount AndAlso Not String.IsNullOrWhiteSpace(str) Then
            Dim index As Integer = 1
            Dim lines() As String = str.Split(";")
            For Each line As String In lines
                If String.IsNullOrWhiteSpace(line) Then
                    Exit For
                End If
                Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.NewtblCheckBoxRow
                r.Index = index
                index += 1
                For idx As Integer = 1 To clsUpDown.cMaxUpdownColumns
                    If line.Count < idx Then
                        Exit For
                    End If
                    Dim s As String = line.Substring(idx - 1, 1)
                    r(String.Format("CheckBox{0:00}", idx)) = (s = "1")
                Next
                CheckBoxTable.Rows.Add(r)
            Next
            CheckBoxTable.AcceptChanges()
            Return True
        Else
            Return False
        End If
    End Function

    'CheckBoxTableから文字列を作る
    Private Function updownstrFromTable() As String
        Dim sb As New System.Text.StringBuilder
        For Each r As dstWork.tblCheckBoxRow In CheckBoxTable
            For idx As Integer = 1 To HorizontalCount
                If r(String.Format("CheckBox{0:00}", idx)) Then
                    sb.Append("1")
                Else
                    sb.Append("0")
                End If
            Next
            sb.Append(";")
        Next
        Return sb.ToString
    End Function

    'データレコードからセットする
    Function FromRecord(ByVal row As tblひも上下Row) As Boolean
        Try
            HorizontalCount = row.f_i水平本数
            VerticalCount = row.f_i垂直本数
            Memo = row.f_sメモ
            Dim str As String = row.f_s上下

            If updownstrToTable(str) Then
                If IsValid Then
                    Return True
                Else
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsUpDown.FromRecord ({0},{1}) {2}", HorizontalCount, VerticalCount, str)
                    Return False
                End If
            Else
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "clsUpDown.FromRecord Empty")
                Return False
            End If
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.FromRecord")
            Return False
        End Try
    End Function

    'データレコードにセットする
    Function ToRecord(ByVal row As tblひも上下Row) As Boolean
        Try
            row.f_i水平本数 = HorizontalCount
            row.f_i垂直本数 = VerticalCount
            row.f_s上下 = updownstrFromTable()
            row.f_sメモ = Memo
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.ToRecord")
            Return False
        End Try
    End Function

    '設定レコードからセットする
    Function FromMasterRecord(ByVal row As tbl上下模様Row) As Boolean
        Try
            HorizontalCount = row.f_i水平本数
            VerticalCount = row.f_i垂直本数
            Memo = row.f_s備考
            Dim str As String = row.f_s上下

            If updownstrToTable(str) Then
                If IsValid Then
                    Return True
                Else
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsUpDown.FromRecord ({0},{1}) {2}", HorizontalCount, VerticalCount, str)
                    Return False
                End If
            Else
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Detail, "clsUpDown.FromRecord Empty")
                Return False
            End If
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.FromRecord")
            Return False
        End Try
    End Function

    '設定レコードにセットする
    Function ToMasterRecord(ByVal row As tbl上下模様Row) As Boolean
        Try
            row.f_i水平本数 = HorizontalCount
            row.f_i垂直本数 = VerticalCount
            row.f_s上下 = updownstrFromTable()
            row.f_s備考 = Memo
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.ToRecord")
            Return False
        End Try
    End Function

    'サイズ変更(VerticalCountに合わせる)
    Function ReSize() As Boolean
        Try
            If CheckBoxTable.Rows.Count < VerticalCount Then
                Do While CheckBoxTable.Rows.Count < VerticalCount
                    Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.NewtblCheckBoxRow
                    r.Index = CheckBoxTable.Rows.Count + 1
                    CheckBoxTable.Rows.Add(r)
                Loop

            ElseIf VerticalCount < CheckBoxTable.Rows.Count Then
                Do While VerticalCount < CheckBoxTable.Rows.Count
                    Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.Rows(CheckBoxTable.Rows.Count - 1)
                    CheckBoxTable.Rows.Remove(r)
                Loop
            End If
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.ReSize")
            Return False
        End Try

    End Function

    '上下交換
    Function Revert() As Boolean
        Try
            For Each r As dstWork.tblCheckBoxRow In CheckBoxTable
                For idx As Integer = 1 To HorizontalCount
                    r(String.Format("CheckBox{0:00}", idx)) = Not r(String.Format("CheckBox{0:00}", idx))
                Next
            Next
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.Revert")
            Return False
        End Try

    End Function

    '左右反転
    Function LeftSideRight() As Boolean
        Try
            For Each r As dstWork.tblCheckBoxRow In CheckBoxTable
                Dim lst As New List(Of Boolean)
                For idx As Integer = 1 To HorizontalCount
                    lst.Add(r(String.Format("CheckBox{0:00}", idx)))
                Next
                For idx As Integer = 1 To HorizontalCount
                    r(String.Format("CheckBox{0:00}", idx)) = lst(HorizontalCount - idx)
                Next
            Next
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.LeftSideRight")
            Return False
        End Try

    End Function

    '天地反転
    Function UpSideDown() As Boolean
        Try
            Dim tmptable As dstWork.tblCheckBoxDataTable = CheckBoxTable.Copy
            For i As Integer = 0 To CheckBoxTable.Rows.Count - 1
                Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.Rows(i)
                Dim tmp As dstWork.tblCheckBoxRow = tmptable.Rows(CheckBoxTable.Rows.Count - i - 1)
                For idx As Integer = 1 To HorizontalCount
                    r(String.Format("CheckBox{0:00}", idx)) = tmp(String.Format("CheckBox{0:00}", idx))
                Next
            Next
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.UpSideDown")
            Return False
        End Try
    End Function

    '右回転
    Function RotateRight() As Boolean
        Try
            Dim iOldHorizontalCount = HorizontalCount
            Dim iOldVerticalCount = VerticalCount
            HorizontalCount = iOldVerticalCount
            VerticalCount = iOldHorizontalCount

            Dim tmptable As dstWork.tblCheckBoxDataTable = CheckBoxTable.Copy
            CheckBoxTable.Rows.Clear()
            '
            For i As Integer = 0 To VerticalCount - 1
                Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.NewtblCheckBoxRow
                r.Index = i + 1
                For idx As Integer = 1 To HorizontalCount
                    Dim tmp As dstWork.tblCheckBoxRow = tmptable.Rows(iOldVerticalCount - idx)
                    r(String.Format("CheckBox{0:00}", idx)) = tmp(String.Format("CheckBox{0:00}", i + 1))
                Next
                CheckBoxTable.Rows.Add(r)
            Next
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.RotateRight")
            Return False
        End Try
    End Function


    '上値を取得 Idx は各、1～Countの値
    Function GetIsUp(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As Boolean
        If Not IsValid Then
            Return False
        End If
        Try
            Dim hIdx As Integer = ((horzIdx - 1) Mod HorizontalCount) + 1
            Dim vIdx As Integer = ((vertIdx - 1) Mod VerticalCount)
            Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.Rows(vIdx)
            Return r(String.Format("CheckBox{0:00}", hIdx))

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.GetIsUp")
            Return False
        End Try
    End Function

    '下値を取得 Idx は各、1～Countの値
    Function GetIsDown(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As Boolean
        If Not IsValid Then
            Return False
        End If
        Try
            Dim hIdx As Integer = ((horzIdx - 1) Mod HorizontalCount) + 1
            Dim vIdx As Integer = ((vertIdx - 1) Mod VerticalCount)
            Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.Rows(vIdx)
            Return Not r(String.Format("CheckBox{0:00}", hIdx))

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.GetIsDown")
            Return False
        End Try
    End Function


End Class
