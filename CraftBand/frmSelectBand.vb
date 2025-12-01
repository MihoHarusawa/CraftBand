Imports System.Drawing
Imports System.Windows.Forms

Public Class frmSelectBand

    Dim _loaded As Boolean = False

    '四つ畳み編み専用値
    Public Property p_bIsKnotLeft As Boolean
    Public Property p_dMySafetyFactor As Double

    Private Sub frmTargetBand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '四つ畳み編み専用
        If g_enumExeName <> enumExeName.CraftBandKnot Then
            grp四つ畳み編みの上の縦ひも位置.Visible = False
        Else
            grp四つ畳み編みの上の縦ひも位置.Visible = True
            If p_bIsKnotLeft Then
                rad左側.Checked = True
            Else
                rad右側.Checked = True
            End If
            If 0 < p_dMySafetyFactor Then
                nudマイひも長係数.Value = p_dMySafetyFactor
            Else
                nudマイひも長係数.Value = 1
            End If
        End If

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
        chk実寸目盛.Checked = g_clsSelectBasics.p_is実寸目盛

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
        g_clsSelectBasics.p_is実寸目盛 = chk実寸目盛.Checked

        Dim sel As String = cmb対象バンドの種類名.Text
        If sel <> g_clsSelectBasics.p_s対象バンドの種類名 Then
            g_clsSelectBasics.SetTargetBandTypeName(sel, True)
            Me.DialogResult = DialogResult.OK
        Else
            'g_clsSelectBasics.UpdateTargetBandType()
            '変更なし
        End If

        '四つ畳み編みのみ
        If g_enumExeName = enumExeName.CraftBandKnot Then
            If p_bIsKnotLeft <> rad左側.Checked Then
                p_bIsKnotLeft = rad左側.Checked
                Me.DialogResult = DialogResult.OK
            End If
            If p_dMySafetyFactor <> nudマイひも長係数.Value Then
                p_dMySafetyFactor = nudマイひも長係数.Value
                Me.DialogResult = DialogResult.OK
            End If
        End If

        Me.Close()
    End Sub

    Private Sub frmSelectBand_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmSelectBand", Me.Size)
    End Sub

    Private Sub txtリスト出力記号_TextChanged(sender As Object, e As EventArgs) Handles txtリスト出力記号.TextChanged
        If 1 < txtリスト出力記号.Text.Length Then
            txtリスト出力記号.Text = txtリスト出力記号.Text.Substring(0, 1)
        End If
    End Sub
End Class