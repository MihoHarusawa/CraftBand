Imports CraftBand.clsDataTables
Imports CraftBand.clsMasterTables
Imports CraftBand.clsUpDown
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables.dstDataTables
Imports System.Drawing
Imports System.Windows.Forms

Public Class ctrExpanding

    'Panelを置き、各ControlはPanelにAnchorし、Panelをコードでリサイズする


    Public Property PanelSize As Drawing.Size
        Set(value As Drawing.Size)
            If Not _isLoadingData Then
                Panel.Size = New Size(value.Width, value.Height)
            End If
        End Set
        Get
            Return Panel.Size
        End Get
    End Property

    'イベント
    Public Class ExpandingEventArgs
        Inherits EventArgs

        Public Property Row As tbl縦横展開Row = Nothing
        Public Property DataPropertyName As String

        Public Sub New(ByVal r As tbl縦横展開Row, Optional pname As String = Nothing)
            Me.Row = r
            DataPropertyName = pname
        End Sub
    End Class

    Public Event ResetButton As EventHandler(Of ExpandingEventArgs)
    Public Event AddButton As EventHandler(Of ExpandingEventArgs)
    Public Event DeleteButton As EventHandler(Of ExpandingEventArgs)
    Public Event CellValueChanged As EventHandler(Of ExpandingEventArgs)



    '対象バンド・基本値の更新
    Sub setBasics()
        With g_clsSelectBasics
            Dim format As String = String.Format("N{0}", .p_unit設定時の寸法単位.DecimalPlaces)
            Me.f_d長さ4.DefaultCellStyle.Format = format
            Me.f_dひも長4.DefaultCellStyle.Format = format
            Me.f_d出力ひも長4.DefaultCellStyle.Format = format
            Me.f_d幅4.DefaultCellStyle.Format = format
        End With
    End Sub


#Region "公開関数"

    <Flags()>
    Public Enum enumVisible
        i_None = 0
        i_幅 = &H1
        i_出力ひも長 = &H2
    End Enum

    'Load後に一度だけセットしてください
    Sub SetNames(ByVal formcaption As String, ByVal tabname As String,
                 ByVal isWidthChangeable As Boolean, ByVal bit_visible As enumVisible,
                 ByVal directions As String, ByVal addLenNames As String)

        _FormCaption = formcaption
        _Profile_dgv展開.FormCaption = formcaption
        _TabPageName = tabname
        '
        f_i何本幅4.DataSource = g_clsSelectBasics.p_tblLane
        f_i何本幅4.DisplayMember = "Display"
        f_i何本幅4.ValueMember = "Value"

        f_s色4.DataSource = g_clsSelectBasics.p_tblColor
        f_s色4.DisplayMember = "Display"
        f_s色4.ValueMember = "Value"

        '幅変更可能なら(デフォルトは不可)
        If isWidthChangeable Then
            f_i何本幅4.ReadOnly = False
        End If

        '表示選択カラム
        f_d幅4.Visible = bit_visible.HasFlag(enumVisible.i_幅)
        f_d出力ひも長4.Visible = bit_visible.HasFlag(enumVisible.i_出力ひも長)

        '方向の表示
        Dim format As String = lblDirection.Text
        Dim direction() As String = Nothing
        If Not String.IsNullOrWhiteSpace(directions) Then
            direction = directions.Split(",")
        End If
        If direction IsNot Nothing AndAlso 2 <= direction.Length Then
            lblDirection.Text = String.Format(format, direction(0), direction(1))
        Else
            lblDirection.Text = "" '指定する前提
        End If

        'ひも長加算
        If Not String.IsNullOrWhiteSpace(addLenNames) Then
            Dim names() As String = addLenNames.Split(",")
            If 0 < names.Length Then
                f_dひも長加算4.HeaderText += Parentheses(names(0))
                If 1 < names.Length Then
                    f_dひも長加算24.HeaderText += Parentheses(names(1))
                    f_dひも長加算24.Visible = True
                End If
            End If
        End If
    End Sub

    Property DataSource As tbl縦横展開DataTable
        Get
            Return BindingSource展開.DataSource
        End Get
        Set(value As tbl縦横展開DataTable)
            If Panel.Enabled Then
                BindingSource展開.DataSource = value
                BindingSource展開.Sort = "f_iひも種,f_iひも番号"
                dgv展開.Refresh()
            End If
        End Set
    End Property

    '編集表示する
    Function ShowGrid(ByVal table As tbl縦横展開DataTable) As Boolean
        Try
            setBasics()
            Panel.Enabled = True

            DataSource = table

            _EditChanged = False
            Return True

        Catch ex As Exception
            BindingSource展開.DataSource = Nothing
            g_clsLog.LogException(ex, "ctrExpanding.ShowGrid")
            Return False
        End Try
    End Function

    '指定位置の選択(f_iひも種", "f_iひも番号)
    Function PositionSelect(ByVal row As tbl縦横展開Row) As Boolean
        Try
            Return dgv展開.PositionSelect(row, {"f_iひも種", "f_iひも番号"})
        Catch ex As Exception
            g_clsLog.LogException(ex, "ctrExpanding.PositionSelect")
        End Try
        Return False
    End Function

    '表示中で編集されていればデータ保存する
    Function Save(ByVal btype As enumひも種, ByVal works As clsDataTables) As Boolean
        If BindingSource展開.DataSource Is Nothing OrElse Not Panel.Enabled OrElse works Is Nothing Then
            Return False
        End If
        If Not _EditChanged Then
            Return False
        End If

        works.FromTmpTable(btype, BindingSource展開.DataSource)
        _EditChanged = False
        Return True
    End Function

    '削除ボタンと追加ボタンの表示
    Sub AddDeleteButtonVisible(ByVal visible As Boolean)
        btn削除.Visible = visible
        btn追加.Visible = visible
    End Sub

    '編集完了、非表示にする
    Function HideGrid(ByVal btype As enumひも種, ByVal works As clsDataTables) As Boolean
        Dim ret As Boolean = Save(btype, works)
        BindingSource展開.Sort = Nothing
        BindingSource展開.DataSource = Nothing

        Panel.Enabled = False
        Return ret
    End Function

    Sub SetDrawOrder(ByVal isVisible As Boolean)
        f_i表示順.Visible = isVisible
        f_i非表示順.Visible = isVisible
    End Sub

    'カラム幅を文字列に保存
    Public ReadOnly Property GetColumnWidthString() As String
        Get
            Return dgv展開.GetColumnWidthString()
        End Get
    End Property

    '文字列からカラム幅を復元
    Public Function SetColumnWidthFromString(ByVal csvStr As String) As Integer
        Return dgv展開.SetColumnWidthFromString(csvStr)
    End Function

    'Debug用に非表示カラムを表示
    Public Sub SetDgvColumnsVisible()
        For Each col As DataGridViewColumn In dgv展開.Columns
            If Not col.Visible Then
                col.Visible = True
            End If
        Next
    End Sub

    ''幅'列の用途　※Formatは変えません
    Public Function SetWidthText(ByVal header_tooltip As String) As String
        'previous
        Dim header_prv = f_d幅4.HeaderText
        Dim tooltip_prv = f_d幅4.ToolTipText

        If Not String.IsNullOrWhiteSpace(header_tooltip) Then
            Dim ary() As String = header_tooltip.Split(",")
            If 1 <= ary.Length Then
                f_d幅4.HeaderText = ary(0)
                If 2 <= ary.Length Then
                    f_d幅4.ToolTipText = ary(1)
                End If
            End If
        End If

        Return header_prv & "," & tooltip_prv
    End Function

#End Region


    Dim _isLoadingData As Boolean = True 'Designer.vb描画
    Dim _FormCaption As String
    Dim _TabPageName As String
    Dim _EditChanged As Boolean = False

    Dim _Profile_dgv展開 As New CDataGridViewProfile(
            (New tbl縦横展開DataTable),
            Nothing,
            enumAction._Modify_i何本幅 Or enumAction._Modify_s色 Or enumAction._BackColorReadOnlyYellow
            )


    Private Sub ctrExpanding_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgv展開.SetProfile(_Profile_dgv展開)

        '※フォームのデザイン時にもLoadされますので、グローバル参照値は参照できない

        _isLoadingData = False 'Designer.vb描画完了
    End Sub

    Private Sub btnリセット_Click(sender As Object, e As EventArgs) Handles btnリセット.Click
        '{0}をすべて初期状態に戻します。よろしいですか？
        Dim msg As String = String.Format(My.Resources.AskResetExpanding, _TabPageName)
        Dim r As DialogResult = MessageBox.Show(msg, _FormCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.OK Then
            Exit Sub
        End If
        _EditChanged = True

        RaiseEvent ResetButton(Me, New ExpandingEventArgs(Nothing))
    End Sub

    Private Sub btn削除_Click(sender As Object, e As EventArgs) Handles btn削除.Click
        Dim current As System.Data.DataRowView = BindingSource展開.Current
        If current Is Nothing OrElse current.Row Is Nothing Then
            Exit Sub
        End If
        _EditChanged = True

        RaiseEvent DeleteButton(Me, New ExpandingEventArgs(current.Row))
    End Sub

    Private Sub btn追加_Click(sender As Object, e As EventArgs) Handles btn追加.Click
        Dim current As System.Data.DataRowView = BindingSource展開.Current
        If current Is Nothing OrElse current.Row Is Nothing Then
            Exit Sub
        End If
        _EditChanged = True

        RaiseEvent AddButton(Me, New ExpandingEventArgs(current.Row))
    End Sub

    Private Sub dgv展開_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv展開.CellValueChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim current As System.Data.DataRowView = BindingSource展開.Current
        If dgv Is Nothing OrElse current Is Nothing OrElse current.Row Is Nothing _
            OrElse e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then
            Exit Sub
        End If
        _EditChanged = True

        '編集対象のカラム
        Dim dataPropertyName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} dgv展開_CellValueChanged({1},{2}){3}", Now, dataPropertyName, e.RowIndex, dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        If {"f_i何本幅", "f_dひも長加算", "f_dひも長加算2", "f_s色"}.Contains(dataPropertyName) Then
            RaiseEvent CellValueChanged(Me, New ExpandingEventArgs(current.Row, dataPropertyName))
        End If
    End Sub
End Class
