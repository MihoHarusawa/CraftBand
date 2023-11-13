Imports CraftBand.Tables

''' <summary>
''' バンドの種類選択＆基本設定
''' ※マスター確定後にNewのこと
''' </summary>
Public Class clsSelectBasics

    Public Property p_s対象バンドの種類名 As String
    Public Property p_unit設定時の寸法単位 As LUnit 'MasterTables

    'リスト出力用
    Public Property p_sリスト出力記号 As String    'My.Settings
    Public Property p_unit出力時の寸法単位 As LUnit 'LastData
    Public Property p_i小数点以下桁数 As Integer = 0 'LastData

    'リスト出力単位・桁数による文字列
    Public ReadOnly Property p_sリスト出力長(ByVal d As Double) As String
        Get
            Return p_unit出力時の寸法単位.TextDecimalPlaces(New Length(d), p_i小数点以下桁数)
        End Get
    End Property

    '指定されたtblバンドの種類
    Public ReadOnly Property p_i本幅 As Integer
        Get
            Return _i本幅
        End Get
    End Property
    Public ReadOnly Property p_lenバンド幅 As Length
        Get
            Return _lenバンド幅
        End Get
    End Property
    Public ReadOnly Property p_d指定本幅(ByVal n As Integer) As Double
        Get
            Return _d一本幅 * n
        End Get
    End Property

    ''色の選択肢
    Public ReadOnly Property p_tblColor As dstWork.tblColorDataTable
        Get
            Return _dstWork.Tables("tblColor")
        End Get
    End Property
    '空もしくは登録色
    Function IsExistColor(ByVal color As String) As Boolean
        If String.IsNullOrEmpty(color) Then
            Return True
        End If
        Return _dstWork.Tables("tblColor").Rows.Contains(color)
    End Function
    '色フィールド値を文字列化(空は"")
    Shared Function ColorString(ByVal col_obj As Object) As String
        If col_obj Is Nothing OrElse IsDBNull(col_obj) Then
            Return ""
        End If
        Dim color As String = col_obj
        Return color.Trim
    End Function


    'n本幅の選択肢
    Public ReadOnly Property p_tblLane As dstWork.tblLaneDataTable
        Get
            Return _dstWork.Tables("tblLane")
        End Get
    End Property

    '選択したバンドの種類
    Public ReadOnly Property p_row選択中バンドの種類 As clsDataRow 'tblバンドの種類Row
        Get
            Return _row選択中バンドの種類
        End Get
    End Property



    Dim _i本幅 As Integer
    Dim _lenバンド幅 As Length
    Dim _d一本幅 As Double
    Dim _row選択中バンドの種類 As clsDataRow 'tblバンドの種類Row

    Dim _dstWork As dstWork



    Sub New(ByVal sListOutMark As String)
        _dstWork = New dstWork

        p_sリスト出力記号 = sListOutMark
        p_unit設定時の寸法単位 = New LUnit(g_clsMasterTables.GetBasicUnit())


        Dim lengthUnitOutput As String = Nothing
        If __paras.GetLastData("LengthUnitOutput", lengthUnitOutput) Then
            p_unit出力時の寸法単位 = New LUnit(lengthUnitOutput)
        Else
            p_unit出力時の寸法単位 = New LUnit("")
        End If

        Dim decimalPlaceOutput As Integer = 0
        If __paras.GetLastData("DecimalPlaceOutput", decimalPlaceOutput) Then
            p_i小数点以下桁数 = decimalPlaceOutput
        Else
            p_i小数点以下桁数 = 1
        End If

        '対象バンドの種類 
        Dim bandtypename As String = Nothing
        If __paras.GetLastData("TargetBandTypeName", bandtypename) Then
            SetTargetBandTypeName(bandtypename)
        Else
            SetTargetBandTypeName(Nothing)
        End If
    End Sub

    Friend Sub save()
        __paras.SetLastData("LengthUnitOutput", Me.p_unit出力時の寸法単位.Str)
        __paras.SetLastData("TargetBandTypeName", Me.p_s対象バンドの種類名)
        __paras.SetLastData("DecimalPlaceOutput", Me.p_i小数点以下桁数)
    End Sub

    'g_clsMasterTablesがセットされた後、マスターを参照して対象バンドをセットする
    'p_unit設定時の寸法単位の変更の影響あり(lenバンド幅)
    Public Sub SetTargetBandTypeName(ByVal bandtypename As String)

        _row選択中バンドの種類 = g_clsMasterTables.GetBandTypeRecord(bandtypename, True).Clone
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Steps, "SetTargetBandTypeName({0}) {1}", bandtypename, _row選択中バンドの種類.dump)

        Dim str As String
        With _row選択中バンドの種類
            p_s対象バンドの種類名 = .Value("f_sバンドの種類名") '存在するレコード

            _i本幅 = .Value("f_i本幅")
            If _i本幅 < 1 Then
                _i本幅 = 1
            End If

            Dim dバンド幅 As Double = .Value("f_dバンド幅")
            If dバンド幅 < 0 Then
                dバンド幅 = 0
            End If

            _lenバンド幅 = New Length(dバンド幅, p_unit設定時の寸法単位)
            _d一本幅 = dバンド幅 / _i本幅

            str = .Value("f_s色リスト")

            '以外の設定値についてはレコード参照
        End With

        '色リストの選択肢
        ToColorTable(_dstWork.Tables("tblColor"), str, True)


        'n本幅の選択肢
        Dim _LaneTable As dstWork.tblLaneDataTable = _dstWork.Tables("tblLane")
        Dim maxval As Integer = 0
        If 0 < _LaneTable.Rows.Count Then
            maxval = CType(_LaneTable.Compute("Max(Value)", Nothing), Integer)
        End If
        If maxval < _i本幅 Then
            For i As Integer = maxval + 1 To _i本幅
                Dim rl As DataRow = _LaneTable.NewRow
                rl("Display") = i.ToString
                rl("Value") = i
                _LaneTable.Rows.InsertAt(rl, 0)
            Next
            _LaneTable.AcceptChanges()
        ElseIf _i本幅 < maxval Then
            For i As Integer = _LaneTable.Rows.Count - 1 To 0 Step -1
                Dim rl As DataRow = _LaneTable.Rows(i)
                If _i本幅 < rl("Value") Then
                    _LaneTable.Rows.RemoveAt(i)
                End If
            Next
            _LaneTable.AcceptChanges()
        End If

    End Sub

    '色文字列をテーブルレコード化,レコード数を返す
    Public Shared Function ToColorTable(ByVal colorTable As dstWork.tblColorDataTable, ByVal str As String, ByVal isAddBlank As Boolean) As Integer
        colorTable.Rows.Clear()
        Dim rc As DataRow
        If isAddBlank Then
            rc = colorTable.NewRow
            rc("Display") = ""
            rc("Value") = ""
            colorTable.Rows.Add(rc)
        End If
        If Not String.IsNullOrWhiteSpace(str) Then
            Dim ary As String() = str.Split(",")
            For Each s As String In ary
                If Not String.IsNullOrWhiteSpace(s) Then
                    s = s.Trim
                    If Not colorTable.Rows.Contains(s) Then
                        rc = colorTable.NewRow
                        rc("Display") = s
                        rc("Value") = s
                        colorTable.Rows.Add(rc)
                    End If
                End If
            Next
        End If
        Return colorTable.Rows.Count
    End Function

    '色文字列の追加
    Public Shared Function AddColorString(ByRef strBase As String, ByRef strAdd As String) As Boolean
        If String.IsNullOrWhiteSpace(strBase) OrElse String.IsNullOrWhiteSpace(strAdd) Then
            Return False
        End If
        Dim tableBase As New dstWork.tblColorDataTable
        If ToColorTable(tableBase, strBase, False) = 0 Then
            Return False '変更なし
        End If
        Dim tableAdd As New dstWork.tblColorDataTable
        If ToColorTable(tableAdd, strAdd, False) = 0 Then
            Return False '変更なし
        End If
        Dim isAdd As Boolean = False
        For Each r As dstWork.tblColorRow In tableAdd
            If tableBase.FindByValue(r.Value) Is Nothing Then 'TODO
                Dim rc As DataRow = tableBase.NewRow
                rc.ItemArray = r.ItemArray 'TODO
                tableBase.Rows.Add(rc)
                isAdd = True
            End If
        Next
        If isAdd Then
            strBase = colorString(tableBase)
            Return True
        End If
        Return False
    End Function

    Private Shared Function colorString(ByVal coltable As dstWork.tblColorDataTable) As String
        If coltable Is Nothing OrElse coltable.Rows.Count = 0 Then
            Return String.Empty
        End If
        Dim res = (From row In coltable
                   Select val = row.Field(Of String)("Value") Order By val).Distinct.ToList()

        Return String.Join(",", res.ToArray)
    End Function

    '対象バンドの種類名に対するマスターの再読み込み
    Public Sub UpdateTargetBandType()
        SetTargetBandTypeName(p_s対象バンドの種類名)
    End Sub

    Public Overloads Function ToString() As String
        Return String.Format("{0} {1} /Output {2} {3} .{4}", p_s対象バンドの種類名, p_unit設定時の寸法単位, p_sリスト出力記号, p_unit出力時の寸法単位, p_i小数点以下桁数)
    End Function

    Public Function dump() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("Current lane={0} width={1}", p_i本幅, p_lenバンド幅).AppendLine()
        sb.AppendFormat("Selected Band {0}", p_row選択中バンドの種類.dump())
        sb.AppendFormat("Unit Setting={0} Output={1} {2} .{3}", p_unit設定時の寸法単位, p_unit出力時の寸法単位, p_sリスト出力記号, p_i小数点以下桁数)
        Return sb.ToString
    End Function

End Class
