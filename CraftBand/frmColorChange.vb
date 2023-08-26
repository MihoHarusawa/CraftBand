Imports System.Diagnostics.Metrics
Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.clsDataTables
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstMasterTables

Public Class frmColorChange

    Dim _Data As clsDataTables = Nothing
    Dim _CurrentColors As New Dictionary(Of String, Integer)

    Enum enmUsageAppendix
        usage_None      '使用なし
        usage_BottomOval '底_楕円(Mesh)
        usage_InsertBand    '差しひも(Square)
    End Enum
    Dim _UsageAppendix As enmUsageAppendix = enmUsageAppendix.usage_None

    Dim _Table As New dstWork.tblColorChangeDataTable

    Dim _MyProfile As New CDataGridViewProfile(
            (New dstWork.tblColorChangeDataTable),
            Nothing,
            enumAction._BackColorReadOnlyYellow
            )

    Sub New(ByVal data As clsDataTables)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        _Data = data

        If g_enumExeName = enumExeName.CraftBandSquare Then
            _UsageAppendix = enmUsageAppendix.usage_InsertBand
        ElseIf g_enumExeName = enumExeName.CraftBandMesh Then
            _UsageAppendix = enmUsageAppendix.usage_BottomOval
        End If
    End Sub

    Private Sub frmColorChange_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvColorChange.SetProfile(_MyProfile)

        If _UsageAppendix = enmUsageAppendix.usage_BottomOval Then
            'chk個別.Visible = True
            'chk個別.Checked = True
            chk個別.Text = My.Resources.CaptionBottomOval

        ElseIf _UsageAppendix = enmUsageAppendix.usage_InsertBand Then
            'chk個別.Visible = True
            'chk個別.Checked = True
            chk個別.Text = My.Resources.CaptionInsertBand

        Else
            chk個別.Visible = False
            chk個別.Checked = False
        End If

        AfterDataGridViewColumn.DataSource = g_clsSelectBasics.p_tblColor
        AfterDataGridViewColumn.DisplayMember = "Display"
        AfterDataGridViewColumn.ValueMember = "Value"

        '
        collectCurrentColor()
        setGridCurrentColor()

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmColorChangeSize", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmColorChangeGrid", colwid) Then
            Me.dgvColorChange.SetColumnWidthFromString(colwid)
        End If
    End Sub

    Private Sub frmColorChange_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmColorChangeGrid", Me.dgvColorChange.GetColumnWidthString())
        __paras.SetLastData("frmColorChangeSize", Me.Size)
    End Sub

    Private Function collectCurrentColorRow(ByVal row As DataRow) As Boolean
        Try
            Dim color As String = clsSelectBasics.ColorString(row("f_s色"))
            If _CurrentColors.Keys.Contains(color) Then
                _CurrentColors(color) += 1
            Else
                _CurrentColors.Add(color, 1)
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmColorChange.countColor")
            Return False
        End Try
    End Function

    Private Function collectCurrentColor() As Boolean
        _CurrentColors.Clear()
        If _Data Is Nothing Then
            Return False
        End If

        '空を最初に
        _CurrentColors.Add("", 0)

        '現在の色を集める
        If chk側面と縁.Checked Then
            For Each row As tbl側面Row In _Data.p_tbl側面
                collectCurrentColorRow(row)
            Next
        End If
        '
        If chk横ひも.Checked Then
            For Each row As tbl縦横展開Row In _Data.p_tbl縦横展開
                If row.f_iひも種 And enumひも種.i_横 Then
                    collectCurrentColorRow(row)
                End If
            Next
        End If
        '
        If chk縦ひも.Checked Then
            For Each row As tbl縦横展開Row In _Data.p_tbl縦横展開
                If row.f_iひも種 And enumひも種.i_縦 Then
                    collectCurrentColorRow(row)
                End If
            Next
        End If
        '
        If chk追加品.Checked Then
            For Each row As tbl追加品Row In _Data.p_tbl追加品
                If Not String.IsNullOrWhiteSpace(row.f_s色) Then
                    collectCurrentColorRow(row)
                End If
            Next
        End If
        '
        If chk個別.Checked Then
            If _UsageAppendix = enmUsageAppendix.usage_BottomOval Then
                For Each row As tbl底_楕円Row In _Data.p_tbl底_楕円
                    If Not String.IsNullOrWhiteSpace(row.f_s色) Then
                        collectCurrentColorRow(row)
                    End If
                Next

            ElseIf _UsageAppendix = enmUsageAppendix.usage_InsertBand Then
                For Each row As tbl差しひもRow In _Data.p_tbl差しひも
                    If Not String.IsNullOrWhiteSpace(row.f_s色) Then
                        collectCurrentColorRow(row)
                    End If
                Next
            End If
        End If

        Return True
    End Function

    Private Function setGridCurrentColor() As Boolean

        Try
            dgvColorChange.DataSource = Nothing
            _Table.Clear()

            Dim newrow As dstWork.tblColorChangeRow = _Table.NewtblColorChangeRow
            For Each color As String In _CurrentColors.Keys
                newrow = _Table.NewtblColorChangeRow
                newrow.IsTarget = True
                newrow.Before = color
                newrow.Count = _CurrentColors(color)
                _Table.Rows.Add(newrow)
            Next

            dgvColorChange.DataSource = _Table
            dgvColorChange.Refresh()
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmColorChange.setGridCurrentColor")
            Return False
        End Try

    End Function

    '使用色ボタン
    Private Sub btn使用色_Click(sender As Object, e As EventArgs) Handles btn使用色.Click
        collectCurrentColor()
        setGridCurrentColor()
    End Sub

    '該当なし・変更なしはNothingを返す。
    Private Function before_after(ByVal before As String) As String
        Dim after As String = Nothing
        For Each rcolchg As dstWork.tblColorChangeRow In _Table
            If Not rcolchg.IsTarget Then
                Continue For
            End If

            Dim rcolbefore As String = clsSelectBasics.ColorString(rcolchg.Before)
            If before = rcolbefore Then
                after = clsSelectBasics.ColorString(rcolchg.After)
                Exit For
            End If
        Next

        If after Is Nothing Then
            Return Nothing '該当なし

        ElseIf after = before Then
            Return Nothing '同じ

        Else
            Return after

        End If
    End Function

    Private Function colorChangeRow(ByVal row As DataRow) As Boolean
        Try
            Dim color As String = clsSelectBasics.ColorString(row("f_s色"))
            Dim after As String = before_after(color)
            If after IsNot Nothing Then
                row("f_s色") = after
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "frmColorChange.countColor")
            Return False
        End Try
    End Function

    '色変更ボタン
    Private Sub btn色変更_Click(sender As Object, e As EventArgs) Handles btn色変更.Click
        '変更する色が空
        Dim emptylist As New List(Of String)
        For Each rcolchg As dstWork.tblColorChangeRow In _Table
            If Not rcolchg.IsTarget Then
                Continue For
            End If

            If clsSelectBasics.ColorString(rcolchg.After) = "" Then
                Dim rcolbefore As String = clsSelectBasics.ColorString(rcolchg.Before)
                If rcolbefore <> "" Then
                    emptylist.Add(rcolbefore)
                End If
            End If
        Next
        If 0 < emptylist.Count Then
            '色<{0}>はクリアされます。よろしいですか？(残す場合は変更対象のチェックを外してください)
            Dim msg As String = String.Format(My.Resources.AskColorClear, String.Join(",", emptylist.ToArray))
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        '色変更
        If chk側面と縁.Checked Then
            For Each row As tbl側面Row In _Data.p_tbl側面
                colorChangeRow(row)
            Next
        End If
        '
        If chk横ひも.Checked Then
            For Each row As tbl縦横展開Row In _Data.p_tbl縦横展開
                If row.f_iひも種 And enumひも種.i_横 Then
                    colorChangeRow(row)
                End If
            Next
        End If
        '
        If chk縦ひも.Checked Then
            For Each row As tbl縦横展開Row In _Data.p_tbl縦横展開
                If row.f_iひも種 And enumひも種.i_縦 Then
                    colorChangeRow(row)
                End If
            Next
        End If
        '
        If chk追加品.Checked Then
            For Each row As tbl追加品Row In _Data.p_tbl追加品
                colorChangeRow(row)
            Next
        End If
        '
        If chk個別.Checked Then
            If _UsageAppendix = enmUsageAppendix.usage_BottomOval Then
                For Each row As tbl底_楕円Row In _Data.p_tbl底_楕円
                    colorChangeRow(row)
                Next

            ElseIf _UsageAppendix = enmUsageAppendix.usage_InsertBand Then
                For Each row As tbl差しひもRow In _Data.p_tbl差しひも
                    colorChangeRow(row)
                Next
            End If
        End If

        '再表示
        collectCurrentColor()
        setGridCurrentColor()

    End Sub

End Class