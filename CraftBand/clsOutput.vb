Imports CraftBand.clsBandSum
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstOutput
Imports CraftBand.Tables.dstDataTables

Public Class clsOutput

    Dim _dstOutput As Tables.dstOutput 'DataSet
    Dim _ListOutMark As String  '出力記号
    Dim _editAddParts As ctrAddParts    '追加品

    Dim _FilePath As String 'ファイル名
    Public ReadOnly Property FilePath As String
        Get
            Return _FilePath
        End Get
    End Property

    Friend DataTitle As String 'タイトル
    Friend DataCreater As String '作成者
    Friend DataMemo As String 'メモ欄

    'ひもの集計
    Dim _clsBandSum As clsBandSum

    '出力リストのワーク
    Dim _LastNumber As Integer = 0  '出力リスト番号
    Dim _CurrentRow As tblOutputRow

    '出力リストテーブル
    Public ReadOnly Property Table As tblOutputDataTable
        Get
            Return _dstOutput.Tables("tblOutput")
        End Get
    End Property

    'カットリストテーブル
    Public ReadOnly Property CutListTable As tblCutListDataTable
        Get
            Return _dstOutput.Tables("tblCutList")
        End Get
    End Property

    Sub New(ByVal fpath As String)
        _dstOutput = New Tables.dstOutput
        _clsBandSum = New clsBandSum
        _FilePath = fpath

        _ListOutMark = g_clsSelectBasics.p_sリスト出力記号
    End Sub

    Sub New(ByVal ref As clsOutput)
        _dstOutput = ref._dstOutput.Copy
        _ListOutMark = ref._ListOutMark
        _FilePath = ref.FilePath
        _clsBandSum = New clsBandSum(ref._clsBandSum)
        _LastNumber = ref._LastNumber
    End Sub

    Sub Clear()
        _dstOutput.Clear()
        _LastNumber = 0
        _clsBandSum.Clear()
    End Sub


#Region "出力リストテーブル"

#End Region

    '空行
    Sub AddBlankLine()
        _CurrentRow = Table.NewtblOutputRow
        _LastNumber += 1
        _CurrentRow.f_iNo = _LastNumber

        _CurrentRow.f_b空行区分 = True
        Table.Rows.Add(_CurrentRow)
    End Sub

    'レコード行
    Function NextNewRow() As tblOutputRow
        _CurrentRow = Table.NewtblOutputRow
        _LastNumber += 1
        _CurrentRow.f_iNo = _LastNumber

        Table.Rows.Add(_CurrentRow)
        Return _CurrentRow
    End Function

    '現在行を空行にする
    Function SetBlankLine()
        If _CurrentRow IsNot Nothing Then
            _CurrentRow.f_b空行区分 = True
            Return True
        End If
        Return False
    End Function

    '現在行にひも属性表示し、記号を返す
    Function SetBandRow(ByVal count As Integer, ByVal lane As Integer, ByVal length As Double, ByVal color As String, Optional ByVal group As String = Nothing) As String
        'カットリスト記号生成
        Dim mark As String = AddMark(count, lane, length, color, group)

        '表示
        If 0 < lane AndAlso 0 < count AndAlso _CurrentRow IsNot Nothing Then
            _CurrentRow.f_s本幅 = outLaneText(lane)
            _CurrentRow.f_sひも本数 = outCountText(count)
            _CurrentRow.f_sひも長 = outLengthText(length)
            _CurrentRow.f_s色 = color
            _CurrentRow.f_s記号 = mark

            '集計に追加
            _clsBandSum.Add(count, lane, length, color)
        End If

        Return mark
    End Function

    Sub SetBandRowNoMark(ByVal count As Integer, ByVal lane As Integer, ByVal length As Double, ByVal color As String, Optional ByVal group As String = Nothing)
        If _CurrentRow IsNot Nothing Then
            _CurrentRow.f_s記号 = My.Resources.CalcOutNoSum '集計対象外
            _CurrentRow.f_s本幅 = outLaneText(lane)
            _CurrentRow.f_sひも本数 = outCountText(count)
            _CurrentRow.f_sひも長 = outLengthText(length)
            _CurrentRow.f_s色 = color
        End If
    End Sub


    'カットリスト記号を得る※既存がなければ呼び出し順
    Function GetBandMark(ByVal lane As Integer, ByVal length As Double, ByVal color As String, Optional ByVal group As String = Nothing) As String
        'カットリスト記号生成
        Return AddMark(0, lane, length, color, group)
    End Function


    '出力長(出力単位・桁数)
    Function outLengthText(ByVal d As Double) As String
        Return g_clsSelectBasics.p_sリスト出力長(d)
    End Function

    '出力長(出力単位・桁数)単位付き
    Function outLengthTextWithUnit(ByVal d As Double) As String
        Return g_clsSelectBasics.p_sリスト出力長(d) & g_clsSelectBasics.p_unit出力時の寸法単位.Str
    End Function

    'ひも本数
    Function outCountText(ByVal count As Integer) As String
        '{0} 本
        Return String.Format(My.Resources.CalcOutCount, count)
    End Function

    '本幅
    Function outLaneText(ByVal lane As Integer) As String
        '{0}幅
        Return String.Format(My.Resources.CalcOutLane, lane)
    End Function


#Region "カットリストテーブル"
    Private Function markStr(ByVal i As Integer) As String
        '#60
        If i < 1 Then
            Return ""
        ElseIf String.IsNullOrEmpty(_ListOutMark) OrElse _ListOutMark = "1" Then
            Return i.ToString
        End If
        '記号が指定されている#67
        Select Case _ListOutMark(0) '1文字のはずだけれど念のため1文字目
            Case "a"c, "ａ"c, "A"c, "Ａ"c
                If 26 < i Then
                    Dim m As Integer = (i - 1) \ 26
                    Dim n As Integer = (i - 1) Mod 26 + 1
                    Return ChrW(AscW(_ListOutMark) + m - 1) & ChrW(AscW(_ListOutMark) + n - 1)
                End If
            Case "①"c
                If 20 < i Then
                    Return String.Format("({0})", i)
                End If
            Case "１"c
                If 10 <= i Then
                    Return i.ToString
                End If
            Case Else
                '
        End Select
        '続く文字コード
        Return ChrW(AscW(_ListOutMark) + i - 1)
    End Function

    'ひもの属性からマークを得る(なければ新規マーク・あれば既存マーク)
    'countが正の時、合計に加える
    Private Function AddMark(ByVal count As Integer, ByVal lane As Integer, ByVal length As Double, ByVal color As String, ByVal group As String) As String
        Dim lengthText As String = g_clsSelectBasics.p_sリスト出力長(length)
        'If lengthText = g_clsSelectBasics.p_sリスト出力長(0) Then
        '    Debug.Print("length=0")
        'End If

        Dim cond As String = String.Format("f_i本幅 = {0} AND f_s長さ = '{1}' AND f_s色 = '{2}' AND f_s記号範囲 = '{3}'", lane, lengthText, color, group)
        Dim sels() As tblCutListRow = CutListTable.Select(cond)
        If sels Is Nothing OrElse sels.Count = 0 Then
            Dim rcut As tblCutListRow = CutListTable.NewtblCutListRow
            rcut.f_i本幅 = lane
            rcut.f_d長さ = length
            rcut.f_s長さ = lengthText
            If String.IsNullOrWhiteSpace(color) Then
                rcut.f_s色 = ""
            Else
                rcut.f_s色 = color
            End If
            If String.IsNullOrWhiteSpace(group) Then
                rcut.f_s記号範囲 = ""
            Else
                rcut.f_s記号範囲 = group
            End If
            rcut.f_iNo = CutListTable.Rows.Count + 1
            rcut.f_s記号 = markStr(rcut.f_iNo)
            If 0 < count Then
                rcut.f_i合計本数 = count
            Else
                rcut.f_i合計本数 = 0
            End If
            CutListTable.Rows.Add(rcut)

            Return rcut.f_s記号

        Else
            Dim rcut As tblCutListRow = sels(0)
            If 0 < count Then
                rcut.f_i合計本数 += count
            End If

            Return rcut.f_s記号
        End If

    End Function
#End Region

    Dim _RowSemmery As tblOutputRow = Nothing

    '基本情報
    Function OutBasics(ByVal row目標寸法 As clsDataRow) As Integer
        DataTitle = row目標寸法.Value("f_sタイトル")
        DataCreater = row目標寸法.Value("f_s作成者")
        DataMemo = row目標寸法.Value("f_sメモ")

        NextNewRow()

        _CurrentRow.f_sカテゴリー = IO.Path.GetFileNameWithoutExtension(FilePath) '名前
        _CurrentRow.f_s本幅 = outLaneText(g_clsSelectBasics.p_i本幅)
        _CurrentRow.f_sひも長 = g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(g_clsSelectBasics.p_lenバンド幅)
        _CurrentRow.f_s編みかた名 = g_clsSelectBasics.p_s対象バンドの種類名
        _CurrentRow.f_sメモ = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_s製品情報")

        NextNewRow()

        _CurrentRow.f_sカテゴリー = DataTitle
        _CurrentRow.f_s本幅 = outLaneText(row目標寸法.Value("f_i基本のひも幅"))
        Dim color As String = row目標寸法.Value("f_s基本色")
        If Not String.IsNullOrWhiteSpace(color) Then
            _CurrentRow.f_s色 = color
            Dim drcol As clsColorRecordSet = g_clsMasterTables.GetColorRecordSet(color, g_clsSelectBasics.p_s対象バンドの種類名, False)
            If drcol IsNot Nothing Then
                _CurrentRow.f_sメモ = drcol.Product
            End If
        End If
        _RowSemmery = _CurrentRow

        AddBlankLine()
        _CurrentRow.f_sカテゴリー = DataCreater

        Return 3    '2行
    End Function

    '追加品
    Function OutAddParts(ByVal editAddParts As ctrAddParts) As Integer
        _editAddParts = editAddParts
        If editAddParts Is Nothing Then
            Return 0
        End If
        Dim raws() As tbl追加品Row = editAddParts.GetAddPartsRecords()
        If raws Is Nothing OrElse raws.Count = 0 Then
            Return 0
        End If

        Dim lines As Integer = 0
        NextNewRow()
        _CurrentRow.f_sカテゴリー = editAddParts.TabPageName()
        _CurrentRow.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        _CurrentRow.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str

        For Each r As tbl追加品Row In raws
            NextNewRow()
            _CurrentRow.f_s番号 = r.f_i番号.ToString
            _CurrentRow.f_s編みかた名 = r.f_s付属品名
            _CurrentRow.f_s編みひも名 = r.f_s付属品ひも名
            _CurrentRow.f_i周数 = r.f_i点数
            _CurrentRow.f_s長さ = outLengthText(r.f_d長さ)
            If 0 < r.f_iひも本数 AndAlso Not r.f_b集計対象外区分 Then
                r.f_s記号 = SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d出力ひも長, r.f_s色)
            Else
                r.f_s記号 = ""
                SetBandRowNoMark(r.f_iひも本数, r.f_i何本幅, r.f_d出力ひも長, r.f_s色)

                '_CurrentRow.f_s記号 = My.Resources.CalcOutNoSum '集計対象外
                '_CurrentRow.f_s本幅 = outLaneText(r.f_i何本幅)
                '_CurrentRow.f_sひも本数 = outCountText(r.f_iひも本数)
                '_CurrentRow.f_sひも長 = outLengthText(r.f_d出力ひも長)
                '_CurrentRow.f_s色 = r.f_s色
            End If
            _CurrentRow.f_sメモ = r.f_sメモ
            If 0 < r.f_i長さ参照 Then
                _CurrentRow.f_s高さ = editAddParts.RefLenNames(r.f_i長さ参照)
            End If
        Next

        AddBlankLine()
        Return lines + 1
    End Function

    '集計
    Function OutSummery() As Integer
        Dim lines As Integer = OutSumList()
        AddBlankLine()
        lines += OutCutList()
        Return lines + 1
    End Function

    '集計値
    Private Function OutSumList() As Integer
        Dim lines As Integer = 0
        NextNewRow()
        lines += 1
        _CurrentRow.f_sカテゴリー = My.Resources.CalcOutSum '集計値
        _CurrentRow.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        '
        _CurrentRow.f_s編みかた名 = My.Resources.CalcOutLongest '単純計
        If Not g_clsSelectBasics.p_isバンド個別幅 Then
            _CurrentRow.f_s編みひも名 = My.Resources.CalcOutShortest '面積長
            _CurrentRow.f_s高さ = My.Resources.CalcOutShortest '面積長
        End If
        _CurrentRow.f_s長さ = My.Resources.CalcOutLonguest '最長
        _CurrentRow.f_sメモ = g_clsSelectBasics.p_unit出力時の寸法単位.Str

        Dim sumAllColor As Double = 0 '全色単純計
        Dim sumlengthAllColor As Double = 0 '全色面積長の計
        Dim sumcountAllColor As Integer = 0 '全色本数の計
        For Each color As String In _clsBandSum.Colors
            Dim sum As Double = 0 '単純計
            Dim sumlength As Double = 0 '面積長の計
            Dim maxmaxlen As Double = 0 '最長の最長
            Dim sumcount As Integer = 0 '本数の計
            Dim bandLength As LaneLength = _clsBandSum.BandLength(color)
            Dim bandMaxLength As LaneLength = _clsBandSum.BandMaxLength(color)
            Dim bandCount As LaneLength = _clsBandSum.BandCount(color)
            For Each lane As Integer In bandLength.Keys
                NextNewRow()
                lines += 1
                _CurrentRow.f_s本幅 = outLaneText(lane)
                Dim length As Double = bandLength(lane)
                _CurrentRow.f_sひも長 = outLengthText(length)
                _CurrentRow.f_s色 = color
                '単純計
                sum += length
                '面積長=割かなかったとしたらの長さ
                Dim band As Double = length / g_clsSelectBasics.p_i本幅 * lane
                If Not g_clsSelectBasics.p_isバンド個別幅 Then
                    _CurrentRow.f_s高さ = outLengthText(band)
                Else
                    _CurrentRow.f_sタイプ = g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(g_clsSelectBasics.p_d指定本幅(lane), False)
                End If
                '最長
                Dim maxlen As Double = bandMaxLength(lane)
                _CurrentRow.f_s長さ = outLengthText(maxlen)
                '本数
                Dim count As Integer = bandCount(lane)
                _CurrentRow.f_sひも本数 = outCountText(count)

                sumlength += band
                If maxmaxlen < maxlen Then
                    maxmaxlen = maxlen
                End If
                sumcount += count
            Next
            If 0 < sumlength Then
                NextNewRow()
                lines += 1
                _CurrentRow.f_s本幅 = Parentheses(My.Resources.CalcOutLongest) '単純計
                _CurrentRow.f_sひも長 = outLengthText(sum)
                If Not g_clsSelectBasics.p_isバンド個別幅 Then
                    _CurrentRow.f_s高さ = outLengthText(sumlength)
                End If
                _CurrentRow.f_s長さ = outLengthText(maxmaxlen)
                _CurrentRow.f_sひも本数 = Parentheses(outCountText(sumcount))

                If Not String.IsNullOrWhiteSpace(color) Then
                    _CurrentRow.f_s色 = color
                    Dim drcol As clsColorRecordSet = g_clsMasterTables.GetColorRecordSet(color, g_clsSelectBasics.p_s対象バンドの種類名, False)
                    If drcol IsNot Nothing Then
                        _CurrentRow.f_sメモ = drcol.Product
                    End If
                End If
                _CurrentRow.f_s編みかた名 = g_clsSelectBasics.p_sリスト集計出力長(sum)
                If Not g_clsSelectBasics.p_isバンド個別幅 Then
                    _CurrentRow.f_s編みひも名 = g_clsSelectBasics.p_sリスト集計出力長(sumlength)
                End If
            End If

            '全色計
            sumAllColor += sum '単純計
            sumlengthAllColor += sumlength '面積長の計
            sumcountAllColor += sumcount '本数の計
        Next

        If _RowSemmery IsNot Nothing Then
            '単純計
            _RowSemmery.f_sひも本数 = outCountText(sumcountAllColor)
            _RowSemmery.f_s記号 = Parentheses(My.Resources.CalcOutLongest)
            _RowSemmery.f_sひも長 = g_clsSelectBasics.p_sリスト集計出力長(sumAllColor)
            '面積長
            If Not g_clsSelectBasics.p_isバンド個別幅 Then
                _RowSemmery.f_s編みかた名 = Parentheses(My.Resources.CalcOutShortest)
                _RowSemmery.f_s編みひも名 = g_clsSelectBasics.p_sリスト集計出力長(sumlengthAllColor)
            End If
        End If

        Return lines
    End Function

    'カットリスト
    Private Function OutCutList() As Integer
        Dim lines As Integer = 0
        NextNewRow()
        lines += 1
        _CurrentRow.f_sカテゴリー = My.Resources.CalcOutCutList 'カットリスト
        _CurrentRow.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        '
        For Each rcut As tblCutListRow In CutListTable.Rows
            NextNewRow()
            lines += 1
            _CurrentRow.f_s記号 = rcut.f_s記号
            _CurrentRow.f_s本幅 = outLaneText(rcut.f_i本幅)
            _CurrentRow.f_sひも長 = outLengthText(rcut.f_d長さ)
            _CurrentRow.f_sひも本数 = outCountText(rcut.f_i合計本数)
            _CurrentRow.f_s色 = rcut.f_s色
            If g_clsSelectBasics.p_isバンド個別幅 Then
                _CurrentRow.f_sタイプ = g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(g_clsSelectBasics.p_d指定本幅(rcut.f_i本幅), False)
            End If
        Next

        Return lines
    End Function

    'メモ
    Function OutMemo() As Integer
        Dim lines As Integer = 0
        If Not String.IsNullOrEmpty(DataMemo) Then
            NextNewRow()
            _CurrentRow.f_sカテゴリー = DataMemo
            lines += 1
        End If
        '
        NextNewRow()
        Dim viExe As FileVersionInfo = FileVersionInfo.GetVersionInfo(g_clsLog.ExePath)
        Dim viDll As FileVersionInfo = FileVersionInfo.GetVersionInfo(g_clsLog.DllPath)
        _CurrentRow.f_s色 = String.Format("{0} ({1})", IO.Path.GetFileName(viExe.FileName), viExe.FileVersion)
        _CurrentRow.f_s編みかた名 = String.Format("{0} ({1})", IO.Path.GetFileName(viDll.FileName), viDll.FileVersion)
        _CurrentRow.f_s編みひも名 = IO.Path.GetDirectoryName(viExe.FileName)
        _CurrentRow.f_sメモ = "Output " & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Return lines + 1
    End Function


    Function OutCutListHtml() As String
        Dim sb As New System.Text.StringBuilder

        'sb.Append("<H2>").Append(My.Resources.CalcOutCutList).AppendLine("</H2>") 'カットリスト
        sb.AppendLine("<TABLE>")
        For Each rcut As tblCutListRow In CutListTable.Rows
            sb.Append("<TR>")
            sb.Append("<TD>").Append(rcut.f_s記号).Append("</TD>").AppendLine()
            sb.Append("<TD>").Append(outLaneText(rcut.f_i本幅)).Append("</TD>").AppendLine()
            sb.Append("<TD>").Append(outLengthText(rcut.f_d長さ)).Append(g_clsSelectBasics.p_unit出力時の寸法単位.Str).Append("</TD>").AppendLine()
            sb.Append("<TD>").Append(outCountText(rcut.f_i合計本数)).Append("</TD>").AppendLine()
            sb.Append("<TD>").Append(rcut.f_s色).Append("</TD>").AppendLine()

            If g_clsSelectBasics.p_isバンド個別幅 Then
                sb.Append("<TD>").Append(g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(g_clsSelectBasics.p_d指定本幅(rcut.f_i本幅), False)).Append("</TD>").AppendLine()
            End If

            sb.AppendLine("</TR>")
        Next
        sb.AppendLine("</TABLE>")
        Return sb.ToString
    End Function

    Function OutSizeHtml() As String
        If _editAddParts Is Nothing Then
            Return Nothing
        End If

        Dim sb As New System.Text.StringBuilder

        sb.AppendLine("<TABLE>")
        Dim i As Integer = 1

        Dim name As String = _editAddParts.RefLenNames(i)
        Do While Not String.IsNullOrWhiteSpace(name)
            Dim val As Double = ctrAddParts._Refvalues(i)

            sb.Append("<TR>")
            sb.Append("<TD>").Append(name).Append("</TD>").AppendLine()
            sb.Append("<TD>").Append(outLengthTextWithUnit(val)).Append("</TD>").AppendLine()
            sb.AppendLine("</TR>")

            i += 1
            name = _editAddParts.RefLenNames(i)
        Loop
        sb.AppendLine("</TABLE>")
        Return sb.ToString
    End Function


End Class
