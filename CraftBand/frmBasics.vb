Imports System.Drawing
Imports System.Windows.Forms

Public Class frmBasics

    Dim _loaded As Boolean = False

    Private Sub frmBasics_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '寸法単位
        If g_clsSelectBasics.p_unit設定時の寸法単位.Is_inch Then
            rad設定時の寸法単位_in.Checked = True
        ElseIf g_clsSelectBasics.p_unit設定時の寸法単位.Is_cm Then
            rad設定時の寸法単位_cm.Checked = True
        Else
            rad設定時の寸法単位_mm.Checked = True
        End If

        txt設定データの保存先.Text = g_clsMasterTables.MasterTablesFilePath


        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmBasicsSize", siz) Then
            Me.Size = siz
        End If

        _loaded = True
    End Sub



    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        If String.Compare(txt設定データの保存先.Text, g_clsMasterTables.MasterTablesFilePath, True) <> 0 Then
            If IO.File.Exists(txt設定データの保存先.Text) Then
                '{0}が変更されました。選択されたファイルを読み直しますか？
                Dim msg As String = String.Format(My.Resources.AskReloadMasterDataFile, lbl設定データの保存先.Text)
                Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                If r = DialogResult.Cancel Then
                    Exit Sub
                ElseIf r = DialogResult.Yes Then
                    If Not g_clsMasterTables.LoadFile(txt設定データの保存先.Text, True) Then
                        '指定ファイル'{0}'から設定データを読み取ることができませんでした。
                        msg = String.Format(My.Resources.ErrReadMasterTableFile, txt設定データの保存先.Text)
                        MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                    '読み取れた
                    g_clsSelectBasics.p_unit設定時の寸法単位 = New LUnit(g_clsMasterTables.GetBasicUnit())
                    '画面に反映
                    If g_clsSelectBasics.p_unit設定時の寸法単位.Is_inch Then
                        rad設定時の寸法単位_in.Checked = True
                    ElseIf g_clsSelectBasics.p_unit設定時の寸法単位.Is_cm Then
                        rad設定時の寸法単位_cm.Checked = True
                    Else
                        rad設定時の寸法単位_mm.Checked = True
                    End If

                Else
                    g_clsMasterTables.MasterTablesFilePath = txt設定データの保存先.Text
                End If
            Else
                g_clsMasterTables.MasterTablesFilePath = txt設定データの保存先.Text
            End If
        End If

        Dim unit As LUnit
        If rad設定時の寸法単位_in.Checked Then
            unit.Set_inch()
        ElseIf rad設定時の寸法単位_cm.Checked Then
            unit.Set_cm()
        End If
        If Not g_clsSelectBasics.p_unit設定時の寸法単位.Equals(unit) Then
            '『設定時の寸法単位』を変更しても、既に設定済みの値は変わりません。よろしいですか？
            Dim r As DialogResult = MessageBox.Show(My.Resources.AskConfirmChangeUnit, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.Yes Then
                Exit Sub
            End If

        End If

        g_clsMasterTables.SetBasicUnit(unit.Str)
        g_clsSelectBasics.p_unit設定時の寸法単位 = unit

        g_clsSelectBasics.UpdateTargetBandType()

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btn設定データの保存先_Click(sender As Object, e As EventArgs) Handles btn設定データの保存先.Click
        SaveFileDialogMasterTable.FileName = txt設定データの保存先.Text
        If SaveFileDialogMasterTable.ShowDialog = DialogResult.OK Then
            txt設定データの保存先.Text = SaveFileDialogMasterTable.FileName
        End If
    End Sub

    Private Sub frmBasics_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmBasicsSize", Me.Size)
    End Sub
End Class