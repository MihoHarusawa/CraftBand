Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables

Public Class frmBandTypeWidth

    Public Property p_sバンドの種類名 As String 'Write Only
    Public Property p_i本幅 As Integer 'Write Only
    Public Property p_dバンド幅 As Double 'Write Only
    Public Property p_s本幅の幅リスト As String    'Write/Read

    '本幅の幅リスト文字列の仕様
    '　カンマ区切り、比例配分と異なる箇所のみ数値をセット
    '　0はNG, 昇順であること(同値連続は可), 最大本数は幅と一致(セット不可)


    Dim _loaded As Boolean = False
    Dim _MyProfile As New CDataGridViewProfile(
            (New dstWork.tblLaneWidthDataTable),
            Nothing,
            enumAction._BackColorReadOnlyYellow
            )

    Dim _tblLaneWidthDataTable As New dstWork.tblLaneWidthDataTable


    Private Sub frmBandTypeWidth_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = String.Format(Me.Text, p_sバンドの種類名)
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        lbl設定時の寸法単位.Text = g_clsSelectBasics.p_unit設定時の寸法単位.ToString
        lbl本幅数値.Text = p_i本幅
        lblバンドの幅値.Text = p_dバンド幅

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmBandTypeWidth", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmBandTypeWidthWidth", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If

        '等分幅表示:小数点桁数をひとつUP
        Dim format As String = String.Format("N{0}", 1 + g_clsSelectBasics.p_unit設定時の寸法単位.DecimalPlaces)
        Me.f_d等分幅.DefaultCellStyle.Format = format

        _loaded = True

        setLaneBandWidth()
    End Sub

    Private Sub frmBandTypeWidth_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmBandTypeWidthWidth", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmBandTypeWidth", Me.Size)
    End Sub

    Private Function setLaneBandWidth() As Boolean
        _tblLaneWidthDataTable.Clear()
        If p_i本幅 < 1 OrElse p_dバンド幅 <= 0 Then
            Return False
        End If

        Dim ary() As String = Nothing
        If Not String.IsNullOrWhiteSpace(p_s本幅の幅リスト) Then
            ary = p_s本幅の幅リスト.Split(",")
        End If

        For i As Integer = 1 To p_i本幅
            Dim r As dstWork.tblLaneWidthRow = _tblLaneWidthDataTable.NewtblLaneWidthRow
            r.f_i本幅 = i
            r.f_d等分幅 = get等分値(p_i本幅, p_dバンド幅, i)
            r.Setf_d変更幅Null()

            If ary IsNot Nothing AndAlso i <= ary.Length Then
                Dim d As Double
                If Double.TryParse(ary(i - 1), d) Then
                    r.f_d変更幅 = d
                End If
            End If
            _tblLaneWidthDataTable.Rows.Add(r)
        Next

        '表示
        BindingSource本幅の幅リスト.DataSource = _tblLaneWidthDataTable
        BindingSource本幅の幅リスト.Sort = "f_i本幅"
        dgvData.Refresh()

        Return True
    End Function

    Private Sub btnリセット_Click(sender As Object, e As EventArgs) Handles btnリセット.Click
        For Each r As dstWork.tblLaneWidthRow In _tblLaneWidthDataTable
            r.Setf_d変更幅Null()
        Next
        dgvData.Refresh()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If p_i本幅 < 1 OrElse p_dバンド幅 <= 0 OrElse _tblLaneWidthDataTable.Rows.Count <> p_i本幅 Then
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "frmBandTypeWidth.btnOK NG-Close i本幅 {0} {1}", p_i本幅, _tblLaneWidthDataTable.Rows.Count)
            DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
            Exit Sub
        End If

        '幅リスト文字列化
        Dim sb As New System.Text.StringBuilder
        Dim set_count As Integer = 0
        Dim rows() As dstWork.tblLaneWidthRow = _tblLaneWidthDataTable.Select("0 < f_i本幅", "f_i本幅 ASC")
        For Each row As dstWork.tblLaneWidthRow In rows
            If Not row.Isf_d変更幅Null() Then
                sb.Append(row.f_d変更幅.ToString)
                set_count += 1
            End If
            If row.f_i本幅 < p_i本幅 Then
                sb.Append(",")
            Else
                Exit For
            End If
        Next
        If set_count = 0 Then
            If p_s本幅の幅リスト = "" Then 'default値
                DialogResult = Windows.Forms.DialogResult.Cancel '変更なし
            Else
                p_s本幅の幅リスト = ""
                DialogResult = Windows.Forms.DialogResult.OK
            End If

        Else
            '文字列がある
            Dim s幅リスト As String = sb.ToString
            Dim err As String = Nothing
            If Not Check幅リスト文字列(p_i本幅, p_dバンド幅, s幅リスト, err) Then
                MessageBox.Show(err, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            '結果
            If p_s本幅の幅リスト = s幅リスト Then
                DialogResult = Windows.Forms.DialogResult.Cancel '変更なし
            Else
                p_s本幅の幅リスト = s幅リスト
                DialogResult = Windows.Forms.DialogResult.OK
            End If

        End If

        Me.Close()
    End Sub

    Private Sub btnキャンセル_Click(sender As Object, e As EventArgs) Handles btnキャンセル.Click
        If p_s本幅の幅リスト = "" Then 'default値
            DialogResult = Windows.Forms.DialogResult.Cancel '変更なし
        Else
            '既存の文字列にエラーがあるか
            Dim Err As String = Nothing
            If Check幅リスト文字列(p_i本幅, p_dバンド幅, p_s本幅の幅リスト, Err) Then
                DialogResult = Windows.Forms.DialogResult.Cancel 'エラーなし
            Else
                '現在の設定値にエラーがあります。リセットしてよいですか。
                Dim ret As DialogResult = MessageBox.Show(My.Resources.MsgBandTypeWidthReset, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                If ret = DialogResult.Yes Then
                    p_s本幅の幅リスト = ""
                    DialogResult = Windows.Forms.DialogResult.OK
                ElseIf ret = DialogResult.No Then
                    DialogResult = Windows.Forms.DialogResult.Cancel
                Else
                    Exit Sub '再入力
                End If
            End If
        End If
        Me.Close()
    End Sub


#Region "Shared関数"
    '等分値
    Private Shared Function get等分値(ByVal i本幅 As Integer, ByVal dバンド幅 As Double, ByVal i本 As Integer) As Double
        If i本幅 = 0 Then
            Return 0
        Else
            Return (i本) * dバンド幅 / i本幅
        End If
    End Function

    '等分値の配列
    Private Shared Function get等分値Ary(ByVal i本幅 As Integer, ByVal dバンド幅 As Double) As Double()
        Dim aryバンド幅(i本幅 - 1) As Double
        For i As Integer = 0 To i本幅 - 1
            aryバンド幅(i) = get等分値(i本幅, dバンド幅, i + 1)
        Next
        Return aryバンド幅
    End Function

    '文字列を分割して読み取る err文字列があればエラー
    Private Shared Function get幅リスト(ByVal i本幅 As Integer, ByVal dバンド幅 As Double, ByVal s本幅の幅リスト As String,
                            ByRef err As String) As Double()

        Dim ary幅リスト() As Double
        If i本幅 < 1 OrElse dバンド幅 <= 0 Then
            '"本幅,バンド幅が正しくありません。"
            err = My.Resources.MsgBandTypeWidthInvalid
            Return Nothing
        End If

        '空の文字列
        If String.IsNullOrWhiteSpace(s本幅の幅リスト) Then
            Return Nothing '変更数ゼロ(OK)
        End If

        '幅のリスト
        Dim ary() As String = s本幅の幅リスト.Split(",")
        Dim width_list As New List(Of Double)
        Dim width_set_count As Integer = 0

        '各値のチェック 0～バンド幅
        Dim sbErr As New System.Text.StringBuilder
        For Each str As String In ary
            If String.IsNullOrWhiteSpace(str) Then
                '設定がない値(OK)
                width_list.Add(0)
            Else
                Dim d As Double
                If Double.TryParse(str, d) Then
                    If d <= 0 Then
                        'NG値
                        width_list.Add(-1)
                        '"{0}本幅の変更幅 '{1}'正の値にしてください。"
                        sbErr.AppendFormat(My.Resources.MsgBandTypeWidthPlus, width_list.Count, d).AppendLine()
                    ElseIf dバンド幅 < d Then
                        'NG値
                        width_list.Add(-1)
                        '"{0}本幅の変更幅 '{1}'バンド幅'{2}'以下にしてください。"
                        sbErr.AppendFormat(My.Resources.MsgBandTypeWidthUnder, width_list.Count, d, dバンド幅).AppendLine()
                    ElseIf width_list.Count + 1 = i本幅 Then
                        width_list.Add(0) '設定がない値
                        '"{0}本幅の変更幅 '{1}'最大幅には設定しないでください"
                        sbErr.AppendFormat(My.Resources.MsgBandTypeWidthNoMax, width_list.Count, d).AppendLine()
                    Else
                        'OK値
                        width_list.Add(d)
                        width_set_count += 1
                    End If
                Else
                    width_list.Add(-1) 'NG値
                    '{0}本幅の文字列'{1}'が読み取れません。
                    sbErr.AppendFormat(My.Resources.MsgBandTypeWidthBadString, width_list.Count, str).AppendLine()
                End If
            End If
        Next

        If width_list.Count <> i本幅 Then
            '本幅数{0}とリストの登録数{1}が異なります。
            sbErr.AppendFormat(My.Resources.MsgBandTypeWidthDiffCount, i本幅, width_list.Count).AppendLine()
        End If

        ary幅リスト = width_list.ToArray
        err = sbErr.ToString
        Return ary幅リスト
    End Function

    '大小関係のチェックとバンド幅Ary化 err文字列があればエラー
    Private Shared Function check幅リストtoバンド幅Ary(ByVal i本幅 As Integer, ByVal dバンド幅 As Double, ByVal ary幅リスト() As Double,
                                 ByRef err As String) As Double()

        Dim aryバンド幅(i本幅 - 1) As Double
        For i As Integer = 0 To i本幅 - 1
            If ary幅リスト IsNot Nothing AndAlso i < ary幅リスト.Count AndAlso 0 < ary幅リスト(i) Then
                aryバンド幅(i) = ary幅リスト(i)
            Else
                aryバンド幅(i) = get等分値(i本幅, dバンド幅, i + 1)
            End If
        Next

        Dim sbErr As New System.Text.StringBuilder
        For i As Integer = 1 To i本幅 - 1
            If aryバンド幅(i - 1) > aryバンド幅(i) Then
                '"{0}本幅の値{1:F2}と{2}本幅の値{3:F2}が逆です。"
                sbErr.AppendFormat(My.Resources.MsgBandTypeWidthReverse, i, aryバンド幅(i - 1), i + 1, aryバンド幅(i)).AppendLine()
            End If
        Next

        If 0 < sbErr.Length Then
            err = sbErr.ToString
            Return Nothing
        Else
            Return aryバンド幅
        End If
    End Function

    '変更幅文字列のチェック　戻り値:OK=true, false時はerr文字列セット
    Shared Function Check幅リスト文字列(ByVal i本幅 As Integer, ByVal dバンド幅 As Double, ByVal s幅リスト As String, ByRef err As String) As Boolean
        Dim ary幅リスト() As Double = get幅リスト(i本幅, dバンド幅, s幅リスト, err)
        If Not String.IsNullOrEmpty(err) Then
            Return False
        End If

        Dim aryバンド幅() As Double = check幅リストtoバンド幅Ary(i本幅, dバンド幅, ary幅リスト, err)
        Return String.IsNullOrEmpty(err)
    End Function

    '本幅の幅リスト文字列が正しい場合のみ読み取り値、以外は等分値を返す。エラー文字列があればエラー
    Shared Function Getバンド幅Ary(ByVal i本幅 As Integer, ByVal dバンド幅 As Double, ByVal s本幅の幅リスト As String, ByRef err As String) As Double()

        Dim ary幅リスト() As Double = get幅リスト(i本幅, dバンド幅, s本幅の幅リスト, err)

        'エラーがある
        If Not String.IsNullOrEmpty(err) Then
            'エラーはログに出力する
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "get幅リスト err:{0}{1}{2}", err, Environment.NewLine, s本幅の幅リスト)
            Return get等分値Ary(i本幅, dバンド幅)
        End If

        '幅リストが設定されていない
        If ary幅リスト Is Nothing Then
            Return get等分値Ary(i本幅, dバンド幅)
        End If

        '大小関係のチェックとバンド幅Ary化
        Dim aryバンド幅() As Double = check幅リストtoバンド幅Ary(i本幅, dバンド幅, ary幅リスト, err)
        If String.IsNullOrEmpty(err) Then
            'この幅でOK
            Return aryバンド幅
        End If

        'エラーはログに出力する
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "check幅リストtoバンド幅Ary err:{0}{1}{2}", err, Environment.NewLine, s本幅の幅リスト)

        '等分値を返す
        Return get等分値Ary(i本幅, dバンド幅)
    End Function
#End Region

End Class