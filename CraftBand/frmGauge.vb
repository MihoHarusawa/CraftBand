Imports System.Drawing
Imports System.Windows.Forms
Imports CraftBand.ctrDataGridView
Imports CraftBand.Tables

Public Class frmGauge
    Const coeff_format As String = "0.00"

    Dim _loaded As Boolean = False
    Dim _clsBandTypeGauge As clsBandTypeGauge = Nothing
    Dim _bSaved As Boolean = False

    Dim _MyProfile As New CDataGridViewProfile(
            (New dstWork.tblGaugeDataTable),
            Nothing,
            enumAction._None
            )

    Private Sub frmGauge_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _MyProfile.FormCaption = Me.Text
        dgvData.SetProfile(_MyProfile)

        lbl設定時の寸法単位.Text = g_clsSelectBasics.p_unit設定時の寸法単位.ToString

        '選択肢
        cmb対象バンドの種類名.Items.AddRange(g_clsMasterTables.GetBandTypeNames())

        '現在の選択値
        cmb対象バンドの種類名.Text = g_clsSelectBasics.p_s対象バンドの種類名

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmGauge", siz) Then
            Me.Size = siz
        End If
        Dim colwid As String = Nothing
        If __paras.GetLastData("frmGaugeGrid", colwid) Then
            Me.dgvData.SetColumnWidthFromString(colwid)
        End If

        'Grid
        Dim format As String = String.Format("N{0}", g_clsSelectBasics.p_unit設定時の寸法単位.DecimalPlaces)
        Me.f_dコマ寸法実測値.DefaultCellStyle.Format = format
        Me.f_dコマ寸法計算値.DefaultCellStyle.Format = format
        Me.f_dコマ要尺実測値.DefaultCellStyle.Format = format
        Me.f_dコマ要尺計算値.DefaultCellStyle.Format = format

        _loaded = True

        setBandInfo(cmb対象バンドの種類名.Text)
    End Sub

    Private Sub cmb対象バンドの種類名_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb対象バンドの種類名.SelectedIndexChanged
        If Not _loaded Then
            Exit Sub
        End If

        If _clsBandTypeGauge IsNot Nothing AndAlso _clsBandTypeGauge.IsCurrentBandTypeName(cmb対象バンドの種類名.Text) Then
            Exit Sub
        End If

        If _clsBandTypeGauge IsNot Nothing AndAlso _clsBandTypeGauge.IsChanged Then

            Dim errmsg As String = Nothing
            If Not _clsBandTypeGauge.IsValidValues(errmsg) Then
                MessageBox.Show(errmsg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                '差し戻し
                cmb対象バンドの種類名.Text = _clsBandTypeGauge.p_sバンドの種類名
                Exit Sub
            End If

            '先の'{0}'の変更を保存します。よろしいですか？(保存しない場合は[いいえ])
            Dim msg As String = String.Format(My.Resources.AskSaveChange, _clsBandTypeGauge.p_sバンドの種類名)
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r = DialogResult.Yes Then
                saveBandInfo()
            ElseIf r = DialogResult.Cancel Then
                '再編集
                cmb対象バンドの種類名.Text = _clsBandTypeGauge.p_sバンドの種類名
                Exit Sub
            End If
        End If

        'いったんクリア
        BindingSourceゲージ.Sort = Nothing
        BindingSourceゲージ.DataSource = Nothing
        txtコマ寸法係数a.Text = ""
        txtコマ寸法係数b.Text = ""
        txtコマ要尺係数a.Text = ""
        txtコマ要尺係数b.Text = ""
        _clsBandTypeGauge = Nothing

        setBandInfo(cmb対象バンドの種類名.Text)
    End Sub

    Private Function saveBandInfo() As Boolean
        If _clsBandTypeGauge Is Nothing OrElse Not _clsBandTypeGauge.IsValid Then
            Return False 'スルー
        End If
        _bSaved = True
        Return _clsBandTypeGauge.UpdateMaster(g_clsMasterTables)
    End Function

    Private Function setBandInfo(ByVal bandtypename As String) As Boolean
        'ゲージ取得
        _clsBandTypeGauge = New clsBandTypeGauge(g_clsMasterTables, bandtypename)
        If Not _clsBandTypeGauge.IsValid Then
            Return False
        End If

        '表示
        BindingSourceゲージ.DataSource = _clsBandTypeGauge.GauteDataTable
        BindingSourceゲージ.Sort = clsBandTypeGauge.c_SortOrder
        refreshDisp()

        Return True
    End Function

    '表示
    Private Function refreshDisp() As Boolean

        With _clsBandTypeGauge
            txtコマ寸法係数a.Text = .p_dコマ寸法係数a.ToString(coeff_format)
            txtコマ寸法係数b.Text = .p_dコマ寸法係数b.ToString(coeff_format)
            txtコマ要尺係数a.Text = .p_dコマ要尺係数a.ToString(coeff_format)
            txtコマ要尺係数b.Text = .p_dコマ要尺係数b.ToString(coeff_format)
        End With
        dgvData.Refresh()
        Return True
    End Function

    Private Sub btnリセット_Click(sender As Object, e As EventArgs) Handles btnリセット.Click
        If _clsBandTypeGauge Is Nothing OrElse Not _clsBandTypeGauge.IsValid Then
            Exit Sub
        End If
        '実測値を全てクリアし規定値に戻します。よろしいですか？
        Dim msg As String = String.Format(My.Resources.AskResetAll)
        Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r = DialogResult.Yes Then
            _clsBandTypeGauge.Reset()
            refreshDisp()
        End If
    End Sub

    Private Sub btn係数取得_Click(sender As Object, e As EventArgs) Handles btn係数取得.Click
        If _clsBandTypeGauge Is Nothing OrElse Not _clsBandTypeGauge.IsValid Then
            Exit Sub
        End If
        '係数計算
        If Not _clsBandTypeGauge.CalcCoefficient() Then
            '実測値の不足もしくは値のエラーのため計算できなかった係数があります。
            MessageBox.Show(My.Resources.ErrGridCoeff, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        refreshDisp()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If _clsBandTypeGauge IsNot Nothing AndAlso _clsBandTypeGauge.IsChanged Then
            Dim errmsg As String = Nothing
            If Not _clsBandTypeGauge.IsValidValues(errmsg) Then
                MessageBox.Show(errmsg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                '差し戻し
                Exit Sub
            End If
            saveBandInfo()
        End If

        If _bSaved Then
            Me.DialogResult = DialogResult.OK 'Changed, Need Update
        Else
            Me.DialogResult = DialogResult.Cancel 'No Change
        End If
    End Sub

    Private Sub btnキャンセル_Click(sender As Object, e As EventArgs) Handles btnキャンセル.Click
        If _bSaved Then
            Me.DialogResult = DialogResult.OK 'Changed, Need Update
        Else
            Me.DialogResult = DialogResult.Cancel 'No Change
        End If
    End Sub

    Private Sub frmGauge_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmGaugeGrid", Me.dgvData.GetColumnWidthString())
        __paras.SetLastData("frmGauge", Me.Size)
    End Sub


End Class