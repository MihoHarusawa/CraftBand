Imports System.Drawing
Imports System.Runtime
Imports System.Windows.Forms

Public Class frmSelectBand

    Dim _loaded As Boolean = False

    Private Sub frmTargetBand_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '単位
        lblバンド幅の寸法単位.Text = g_clsSelectBasics.p_unit設定時の寸法単位.Str

        '選択肢
        cmb対象バンドの種類名.Items.AddRange(g_clsMasterTables.GetBandTypeNames())

        '現在の選択値
        cmb対象バンドの種類名.Text = g_clsSelectBasics.p_s対象バンドの種類名
        txt本幅.Text = g_clsSelectBasics.p_i本幅
        txtバンド幅.Text = g_clsSelectBasics.p_lenバンド幅.Value

        If g_clsSelectBasics.p_unit出力時の寸法単位.Is_inch Then
            rad出力時の寸法単位_in.Checked = True
        ElseIf g_clsSelectBasics.p_unit出力時の寸法単位.Is_cm Then
            rad出力時の寸法単位_cm.Checked = True
        Else
            rad出力時の寸法単位_mm.Checked = True
        End If
        txtリスト出力記号.Text = g_clsSelectBasics.p_sリスト出力記号
        nud小数点以下桁数.Value = g_clsSelectBasics.p_i小数点以下桁数

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmSelectBand", siz) Then
            Me.Size = siz
        End If

        _loaded = True
    End Sub


    Private Sub cmb対象バンドの種類名_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb対象バンドの種類名.SelectedIndexChanged
        If Not _loaded Then
            Exit Sub
        End If

        Dim sel As String = cmb対象バンドの種類名.Text
        'tblバンドの種類Row
        Dim crow As clsDataRow = g_clsMasterTables.GetBandTypeRecord(sel, True)
        With crow
            txt本幅.Text = .Value("f_i本幅")
            txtバンド幅.Text = .Value("f_dバンド幅")
        End With

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Dim unit As LUnit
        If rad出力時の寸法単位_in.Checked Then
            unit.Set_inch()
        ElseIf rad出力時の寸法単位_cm.Checked Then
            unit.Set_cm()
        Else
            unit.Set_mm()
        End If
        g_clsSelectBasics.p_unit出力時の寸法単位 = unit

        g_clsSelectBasics.p_sリスト出力記号 = txtリスト出力記号.Text
        g_clsSelectBasics.p_i小数点以下桁数 = nud小数点以下桁数.Value

        Dim sel As String = cmb対象バンドの種類名.Text
        If sel <> g_clsSelectBasics.p_s対象バンドの種類名 Then
            g_clsSelectBasics.SetTargetBandTypeName(sel)
            Me.DialogResult = DialogResult.OK
        Else
            'g_clsSelectBasics.UpdateTargetBandType()
            '変更なし
        End If

        Me.Close()
    End Sub

    Private Sub frmSelectBand_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmSelectBand", Me.Size)
    End Sub
End Class