Imports System.Drawing.Drawing2D
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstMasterTables

''' <summary>
''' tblひも上下のレコードを展開したクラス
''' ※今のところそう大きくはないので、固定サイズ・固定フィールドで処理
''' </summary>
Public Class clsUpDown
    Public Const cMaxUpdownColumns As Integer = 50 'CheckBoxフィールド数
    Public Const cBottomNumber As Integer = 0 '底のレコード番号
    Public Const cSide12Number As Integer = 1 '側面(上右)のレコード番号
    Public Const cSide34Number As Integer = 2 '側面(下左)のレコード番号

    Enum enumTargetFace
        Bottom = 0  '底
        Side12 = 1  '側面(上右)
        Side34 = 2  '側面(下左)
    End Enum
    '側面(下から適用)
    Public Shared Function IsSide(ByVal face As enumTargetFace) As Boolean
        Return face = enumTargetFace.Side12 OrElse face = enumTargetFace.Side34
    End Function


    Public TargetFace As enumTargetFace = enumTargetFace.Bottom 　'※初期化・リセット対象外
    Public HorizontalCount As Integer
    Public VerticalCount As Integer
    Public CheckBoxTable As dstWork.tblCheckBoxDataTable = Nothing  '編集用

    Public Memo As String

    Dim _Matrix(cMaxUpdownColumns, cMaxUpdownColumns) As Boolean '1～HorizontalCount, 1～VerticalCountで使用


    Sub New(ByVal face As enumTargetFace)
        TargetFace = face
    End Sub
    Sub New(ByVal face As enumTargetFace, ByVal horizon As Integer, ByVal vert As Integer)
        TargetFace = face
        HorizontalCount = horizon
        VerticalCount = vert
    End Sub

    ReadOnly Property IsValid As Boolean
        Get
            '数が有効で、テーブルがある
            Return 2 <= HorizontalCount AndAlso 2 <= VerticalCount AndAlso
                 HorizontalCount <= cMaxUpdownColumns AndAlso VerticalCount <= cMaxUpdownColumns AndAlso
                CheckBoxTable IsNot Nothing AndAlso
                CheckBoxTable.Rows.Count = VerticalCount
        End Get
    End Property

    Sub Clear(Optional ByVal isCreateTable As Boolean = False)
        HorizontalCount = 0
        VerticalCount = 0
        Memo = String.Empty
        If isCreateTable Then
            CheckBoxTable = New dstWork.tblCheckBoxDataTable
        ElseIf CheckBoxTable IsNot Nothing Then
            CheckBoxTable.Rows.Clear()
        End If
        Array.Clear(_Matrix, False, _Matrix.Length)
    End Sub

    'テーブルに存在するレコードをそのまま配列へ
    Private Function table_to_matrix(Optional ByVal set_mtx As Boolean = False) As Boolean
        Try
            If set_mtx OrElse CheckBoxTable.GetChanges IsNot Nothing Then
                For y As Integer = 1 To CheckBoxTable.Rows.Count
                    Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.Rows(y - 1)
                    If r.Index <> y Then
                        '警告のみ、
                        g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "table_to_matrix no match {0}<>{1}", r.Index, y)
                    End If
                    'レコード順優先
                    For x As Integer = 1 To cMaxUpdownColumns
                        If r(String.Format("CheckBox{0:00}", x)) Then
                            _Matrix(x, y) = True
                        Else
                            _Matrix(x, y) = False
                        End If
                    Next
                    If y = cMaxUpdownColumns Then
                        Exit For '残りは読まない
                    End If
                Next
                CheckBoxTable.AcceptChanges()
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.table_to_matrix")
            Return False
        End Try
    End Function

    '配列をVerticalCount,HorizontalCountサイズのテーブルへ(x残りは保持)
    Private Function matrix_to_table() As Boolean
        Try
            If cMaxUpdownColumns < VerticalCount Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsUpDown.matrix_to_table VerticalCount{0}->{1}", VerticalCount, cMaxUpdownColumns)
                VerticalCount = cMaxUpdownColumns
            End If
            If cMaxUpdownColumns < HorizontalCount Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsUpDown.matrix_to_table HorizontalCount{0}->{1}", HorizontalCount, cMaxUpdownColumns)
                HorizontalCount = cMaxUpdownColumns
            End If

            'レコード数=VerticalCountにする
            If CheckBoxTable.Rows.Count < VerticalCount Then
                Do While CheckBoxTable.Rows.Count < VerticalCount
                    Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.NewtblCheckBoxRow
                    r.Index = CheckBoxTable.Rows.Count + 1 'NoCheck
                    CheckBoxTable.Rows.Add(r)
                Loop

            ElseIf VerticalCount < CheckBoxTable.Rows.Count Then
                Do While VerticalCount < CheckBoxTable.Rows.Count AndAlso 0 < CheckBoxTable.Rows.Count
                    Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.Rows(CheckBoxTable.Rows.Count - 1)
                    CheckBoxTable.Rows.Remove(r)
                Loop
            End If

            'VerticalCount,HorizontalCount分をテーブルレコードにする
            For y As Integer = 1 To VerticalCount
                Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.Rows(y - 1)
                r.Index = y '上書き
                For x As Integer = 1 To HorizontalCount
                    r(String.Format("CheckBox{0:00}", x)) = _Matrix(x, y)
                Next
            Next
            CheckBoxTable.AcceptChanges()
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.matrix_to_table")
            Return False
        End Try
    End Function


    '初期値
    Function Reset(ByVal sidecount As Integer) As Boolean
        Try
            Dim isFirstUp As Boolean
            Select Case TargetFace
                Case enumTargetFace.Side12
                    isFirstUp = True
                Case enumTargetFace.Side34
                    isFirstUp = (sidecount Mod 2) = 0
                Case Else 'bottom
                    isFirstUp = False
            End Select
            Clear()

            HorizontalCount = 2
            VerticalCount = 2

            _Matrix(1, 1) = isFirstUp
            _Matrix(1, 2) = Not isFirstUp
            _Matrix(2, 1) = Not isFirstUp
            _Matrix(2, 2) = isFirstUp

            Return matrix_to_table()
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.Reset")
            Return False
        End Try
    End Function

#Region "レコードR/W"
    '文字列をそのまま全て読みとって_Matrixへ 行数を返す
    Private Function updownstrToMatrix(ByVal str As String) As Integer
        '_Matrixクリア
        Array.Clear(_Matrix, False, _Matrix.Length)

        If Not String.IsNullOrWhiteSpace(str) Then
            Dim lines() As String = str.Split(";")
            Dim y As Integer = 0
            For Each line As String In lines
                y += 1
                If cMaxUpdownColumns < y Then
                    Return cMaxUpdownColumns '読める分は読んだ
                End If
                If String.IsNullOrWhiteSpace(line) Then
                    Continue For
                End If
                For x As Integer = 1 To clsUpDown.cMaxUpdownColumns
                    If line.Count < x Then
                        Exit For
                    End If
                    Dim s As String = line.Substring(x - 1, 1)
                    _Matrix(x, y) = (s = "1")
                Next
            Next
            Return y
        Else
            Return 0 '空
        End If
    End Function

    '_MatrixからHorizontalCount,VerticalCount分の文字列を作る
    Private Function updownstrFromMatrix() As String
        Dim sb As New System.Text.StringBuilder
        For y As Integer = 1 To VerticalCount
            For x As Integer = 1 To HorizontalCount
                If _Matrix(x, y) Then
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
            Clear()
            HorizontalCount = row.f_i水平本数
            VerticalCount = row.f_i垂直本数
            Memo = row.f_sメモ
            Dim str As String = row.f_s上下

            Dim y As Integer = updownstrToMatrix(str)
            If y <> VerticalCount Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsUpDown.FromRecord ({0},{1}) {2} {3}", HorizontalCount, VerticalCount, y, str)
            End If
            Return matrix_to_table()

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.FromRecord")
            Return False
        End Try
    End Function

    'データレコードにセットする
    Function ToRecord(ByVal row As tblひも上下Row) As Boolean
        Try
            table_to_matrix(True) '常に反映
            row.f_i水平本数 = HorizontalCount
            row.f_i垂直本数 = VerticalCount
            row.f_s上下 = updownstrFromMatrix()
            row.f_sメモ = Memo
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.ToRecord")
            Return False
        End Try
    End Function

    '設定レコードからセットする
    Function FromMasterRecord(ByVal row As tbl上下図Row) As Boolean
        Try
            Clear()
            HorizontalCount = row.f_i水平本数
            VerticalCount = row.f_i垂直本数
            Memo = row.f_s備考
            Dim str As String = row.f_s上下

            Dim y As Integer = updownstrToMatrix(str)
            If y <> VerticalCount Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsUpDown.FromMasterRecord ({0},{1}) {2} {3}", HorizontalCount, VerticalCount, y, str)
            End If
            Return matrix_to_table()

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.FromRecord")
            Return False
        End Try
    End Function

    '設定レコードにセットする
    Function ToMasterRecord(ByVal row As tbl上下図Row) As Boolean
        Try
            table_to_matrix(True) '常に反映
            row.f_i水平本数 = HorizontalCount
            row.f_i垂直本数 = VerticalCount
            row.f_s上下 = updownstrFromMatrix()
            row.f_s備考 = Memo
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "clsUpDown.ToRecord")
            Return False
        End Try
    End Function
#End Region

#Region "パターン操作"
    'サイズ変更(VerticalCountに合わせる)
    Function ReSize(Optional ByVal set_mtx As Boolean = False) As Boolean
        Return table_to_matrix(set_mtx) AndAlso matrix_to_table()
    End Function

    '上下交換
    Function Revert(Optional ByVal set_mtx As Boolean = False) As Boolean
        If table_to_matrix(set_mtx) Then
            For y As Integer = 1 To VerticalCount
                For x As Integer = 1 To HorizontalCount
                    _Matrix(x, y) = Not _Matrix(x, y)
                Next
            Next
            Return matrix_to_table()
        Else
            Return False
        End If
    End Function

    '左右反転
    Function LeftSideRight(Optional ByVal set_mtx As Boolean = False) As Boolean
        If table_to_matrix(set_mtx) Then
            Dim save(,) As Boolean = DirectCast(_Matrix.Clone(), Boolean(,))
            For y As Integer = 1 To VerticalCount
                For x As Integer = 1 To HorizontalCount
                    _Matrix(x, y) = save(HorizontalCount - x + 1, y)
                Next
            Next
            Return matrix_to_table()
        Else
            Return False
        End If
    End Function

    '天地反転
    Function UpSideDown(Optional ByVal set_mtx As Boolean = False) As Boolean
        If table_to_matrix(set_mtx) Then
            Dim save(,) As Boolean = DirectCast(_Matrix.Clone(), Boolean(,))
            For y As Integer = 1 To VerticalCount
                For x As Integer = 1 To HorizontalCount
                    _Matrix(x, y) = save(x, VerticalCount - y + 1)
                Next
            Next
            Return matrix_to_table()
        Else
            Return False
        End If
    End Function

    '右回転
    Function RotateRight(Optional ByVal set_mtx As Boolean = False) As Boolean
        If table_to_matrix(set_mtx) Then
            Dim save(,) As Boolean = DirectCast(_Matrix.Clone(), Boolean(,))
            Dim iOldHorizontalCount = HorizontalCount
            Dim iOldVerticalCount = VerticalCount
            HorizontalCount = iOldVerticalCount
            VerticalCount = iOldHorizontalCount

            For y As Integer = 1 To VerticalCount
                For x As Integer = 1 To HorizontalCount
                    _Matrix(x, y) = save(y, iOldVerticalCount - x + 1)
                Next
            Next
            Return matrix_to_table()
        Else
            Return False
        End If
    End Function

    '水平追加
    Function AddHorizontal(ByVal atTop As Boolean, Optional ByVal set_mtx As Boolean = False) As Boolean
        If table_to_matrix(set_mtx) Then
            HorizontalCount += 1
            If atTop Then
                For y As Integer = 1 To VerticalCount
                    For x As Integer = HorizontalCount - 1 To 2 Step -1
                        _Matrix(x, y) = _Matrix(x - 1, y)
                    Next
                    _Matrix(1, y) = False
                Next
            End If
            Return matrix_to_table()
        Else
            Return False
        End If
    End Function

    '垂直追加
    Function AddVertical(ByVal atTop As Boolean, Optional ByVal set_mtx As Boolean = False) As Boolean
        If table_to_matrix(set_mtx) Then
            VerticalCount += 1
            If atTop Then
                For y As Integer = VerticalCount - 1 To 2 Step -1
                    For x As Integer = 1 To HorizontalCount
                        _Matrix(x, y) = _Matrix(x, y - 1)
                    Next
                Next
                For x As Integer = 1 To HorizontalCount
                    _Matrix(x, 1) = False
                Next
            End If
            Return matrix_to_table()
        Else
            Return False
        End If
    End Function

    '水平シフト
    Function ShiftHorizontal(ByVal desc As Boolean, Optional ByVal set_mtx As Boolean = False) As Boolean
        If table_to_matrix(set_mtx) Then
            Dim save(,) As Boolean = DirectCast(_Matrix.Clone(), Boolean(,))
            For y As Integer = 1 To VerticalCount
                If desc Then
                    For x As Integer = 2 To HorizontalCount
                        _Matrix(x - 1, y) = save(x, y)
                    Next
                    _Matrix(HorizontalCount, y) = save(1, y)
                Else
                    For x As Integer = 1 To HorizontalCount - 1
                        _Matrix(x + 1, y) = save(x, y)
                    Next
                    _Matrix(1, y) = save(HorizontalCount, y)
            End If
            Next
            Return matrix_to_table()
        Else
            Return False
        End If
    End Function

    '垂直シフト
    Function ShiftVertical(ByVal desc As Boolean, Optional ByVal set_mtx As Boolean = False) As Boolean
        If table_to_matrix(set_mtx) Then
            Dim save(,) As Boolean = DirectCast(_Matrix.Clone(), Boolean(,))
            For x As Integer = 1 To HorizontalCount
                If desc Then
                    For y As Integer = 2 To VerticalCount
                        _Matrix(x, y - 1) = save(x, y)
                    Next
                    _Matrix(x, VerticalCount) = save(x, 1)
                Else
                    For y As Integer = 1 To VerticalCount - 1
                        _Matrix(x, y + 1) = save(x, y)
                    Next
                    _Matrix(x, 1) = save(x, VerticalCount)
                End If
            Next
            Return matrix_to_table()
        Else
            Return False
        End If
    End Function
#End Region

    'ランダム化
    Function Randomize() As Boolean
        Dim retry As Integer = 20
        Dim random As New Random()
        For y As Integer = 1 To VerticalCount
            Do While 0 < retry
                For x As Integer = 1 To HorizontalCount
                    Dim randomNumber As Integer = random.Next(2) ' 0から1までの範囲の乱数
                    _Matrix(x, y) = (randomNumber = 1)
                Next
                If check_y(y) Then
                    Exit Do
                End If
                retry -= 1
            Loop
        Next

        For x As Integer = 1 To HorizontalCount
            Do While 0 < retry
                If check_x(x) Then
                    Exit Do
                End If
                For y As Integer = 1 To VerticalCount
                    Dim randomNumber As Integer = random.Next(2) ' 0から1までの範囲の乱数
                    _Matrix(x, y) = (randomNumber = 1)
                Next
                retry -= 1
            Loop
        Next

        Return matrix_to_table()
    End Function

    'xの列はOKか？(全ON/全OFFはNG)
    Private Function check_x(ByVal x As Integer) As Boolean
        If VerticalCount < 2 Then
            Return False
        End If
        For y As Integer = 2 To VerticalCount
            If _Matrix(x, 1) <> _Matrix(x, y) Then
                Return True
            End If
        Next
        Return False
    End Function

    'yの行はOKか？(全ON/全OFFはNG)
    Private Function check_y(ByVal y As Integer) As Boolean
        If HorizontalCount < 2 Then
            Return False
        End If
        For x As Integer = 2 To HorizontalCount
            If _Matrix(1, y) <> _Matrix(x, y) Then
                Return True
            End If
        Next
        Return False
    End Function

    Function Check(ByRef errmsg As String) As Boolean
        If Not IsValid Then
            'サイズ({0},{1})のエラーです。
            errmsg = String.Format(My.Resources.ErrCheckValid, HorizontalCount, VerticalCount)
            Return False
        End If

        table_to_matrix() '読めた結果をチェック
        Dim sb As New System.Text.StringBuilder
        Dim bad As New List(Of Integer)
        For y As Integer = 1 To VerticalCount
            If Not check_y(y) Then
                bad.Add(y)
            End If
        Next
        If 0 < bad.Count Then
            '{0}行を確認してください。
            sb.AppendFormat(My.Resources.ErrCheckHorizontal, String.Join(",", bad))
            bad.Clear()
        End If

        For x As Integer = 1 To HorizontalCount
            If Not check_x(x) Then
                bad.Add(x)
            End If
        Next
        If 0 < bad.Count Then
            '{0}列を確認してください。
            sb.AppendFormat(My.Resources.ErrCheckVertical, String.Join(",", bad))
        End If

        errmsg = sb.ToString
        Return sb.Length = 0
    End Function


    '上値を取得 Idx は各、1～Countの値
    Function GetIsUp(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As Boolean
        If Not IsValid Then
            Return False
        End If
        Dim hIdx As Integer = ((horzIdx - 1) Mod HorizontalCount) + 1
        Dim vIdx As Integer = ((vertIdx - 1) Mod VerticalCount) + 1
        Return _Matrix(hIdx, vIdx)
    End Function

    '下値を取得 Idx は各、1～Countの値
    Function GetIsDown(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As Boolean
        If Not IsValid Then
            Return False
        End If
        Dim hIdx As Integer = ((horzIdx - 1) Mod HorizontalCount) + 1
        Dim vIdx As Integer = ((vertIdx - 1) Mod VerticalCount) + 1
        Return Not _Matrix(hIdx, vIdx)
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("{0} TargetFace={1} Size({2},{3})", Me.GetType.Name, TargetFace, HorizontalCount, VerticalCount).AppendLine()
        sb.Append("Table ")
        For y As Integer = 1 To CheckBoxTable.Rows.Count
            Dim r As dstWork.tblCheckBoxRow = CheckBoxTable.Rows(y - 1)
            sb.AppendFormat("[{0}]", r.Index)
            For x As Integer = 1 To HorizontalCount
                If r(String.Format("CheckBox{0:00}", x)) Then
                    sb.Append("1 ")
                Else
                    sb.Append("0 ")
                End If
            Next
        Next
        sb.AppendLine()
        sb.Append("Matrix")
        For y As Integer = 1 To VerticalCount
            sb.AppendFormat("[{0}]", y)
            For x As Integer = 1 To HorizontalCount
                If _Matrix(x, y) Then
                    sb.Append("T ")
                Else
                    sb.Append("0 ")
                End If
            Next
        Next
        sb.AppendLine()
        sb.AppendFormat("Memo:{0}", Memo)
        Return sb.ToString
    End Function

End Class
