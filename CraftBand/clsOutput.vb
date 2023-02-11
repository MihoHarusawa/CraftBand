Imports CraftBand.clsBandSum
Imports CraftBand.Tables
Imports CraftBand.Tables.dstOutput

Public Class clsOutput

    Dim _dstOutput As dstOutput 'DataSet
    Dim _ListOutMark As String  '出力記号

    Dim _FilePath As String 'ファイル名
    Public ReadOnly Property FilePath As String
        Get
            Return _FilePath
        End Get
    End Property

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
        _dstOutput = New dstOutput
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

    '現在行にひも属性表示
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

    'カットリスト記号を得る※既存がなければ呼び出し順
    Function GetBandMark(ByVal lane As Integer, ByVal length As Double, ByVal color As String, Optional ByVal group As String = Nothing) As String
        'カットリスト記号生成
        Return AddMark(0, lane, length, color, group)
    End Function


    '出力長(出力単位・桁数)
    Function outLengthText(ByVal d As Double) As String
        Return g_clsSelectBasics.p_sリスト出力長(d)
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
        If String.IsNullOrWhiteSpace(_ListOutMark) OrElse i < 1 Then
            Return ""
        End If
        Return ChrW(AscW(_ListOutMark) + i - 1)
    End Function

    'ひもの属性からマークを得る(なければ新規マーク・あれば既存マーク)
    'countが正の時、合計に加える
    Private Function AddMark(ByVal count As Integer, ByVal lane As Integer, ByVal length As Double, ByVal color As String, ByVal group As String) As String
        Dim lengthText As String = g_clsSelectBasics.p_sリスト出力長(length)

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

    '集計値
    Function OutSumList() As Integer
        Dim lines As Integer = 0
        NextNewRow()
        lines += 1
        _CurrentRow.f_sカテゴリー = My.Resources.CalcOutSum '集計値
        _CurrentRow.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        '
        _CurrentRow.f_s高さ = My.Resources.CalcOutShortest '面積長
        _CurrentRow.f_s長さ = My.Resources.CalcOutLonguest '最長
        _CurrentRow.f_sメモ = g_clsSelectBasics.p_unit出力時の寸法単位.Str

        For Each color As String In _clsBandSum.Colors
            Dim sumlength As Double = 0
            Dim maxmaxlen As Double = 0
            Dim sumcount As Integer = 0
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

                '面積長=割かなかったとしたらの長さ
                Dim band As Double = length / g_clsSelectBasics.p_i本幅 * lane
                _CurrentRow.f_s高さ = outLengthText(band)
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
                _CurrentRow.f_sひも長 = "====="
                _CurrentRow.f_s高さ = outLengthText(sumlength)
                _CurrentRow.f_s長さ = outLengthText(maxmaxlen)
                _CurrentRow.f_s色 = color
                _CurrentRow.f_sひも本数 = outCountText(sumcount)
            End If
        Next

        Return lines
    End Function

    'カットリスト
    Function OutCutList() As Integer
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
        Next

        Return lines
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
            sb.AppendLine("</TR>")
        Next
        sb.AppendLine("</TABLE>")
        Return sb.ToString
    End Function

End Class
