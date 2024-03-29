﻿Imports System.Windows.Forms
Imports CraftBand
Imports CraftBand.Tables.dstMasterTables

Public Class frmUpDownSetting

    '対象と指示
    Public CurrentUpdown As clsUpDown = Nothing
    Public IsLoadToCurrent As Boolean = False
    Public IsSaveFromCurrent As Boolean = False


    Private Sub frmUpDownSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CurrentUpdown Is Nothing Then
            Me.Close()
        End If

        Dim names() As String = g_clsMasterTables.GetUpDownNames()
        cmb上下図名.Items.Clear()
        For Each name As String In names
            cmb上下図名.Items.Add(name)
        Next

        If IsLoadToCurrent Then
            LoadCurrent()

        ElseIf IsSaveFromCurrent Then
            SaveCurrent()

        Else
            Me.Close()
        End If
    End Sub

    Private Function LoadCurrent() As Boolean
        grp反映方法.Visible = True
        '上下図名を選択してください。[OK]ボタンで現パターンに反映します。
        lblしてください.Text = My.Resources.UpDownReadInstruction
        Me.Text = My.Resources.UpDownRead

        Return True
    End Function

    Private Function SaveCurrent() As Boolean
        grp反映方法.Visible = False
        '新たな上下図名を入力してください。[OK]ボタンで現パターンを登録します。
        lblしてください.Text = My.Resources.UpDownAppendInstruction
        Me.Text = My.Resources.UpDownAppend
        txtl水平本数.Text = CurrentUpdown.HorizontalCount
        txtl垂直本数.Text = CurrentUpdown.VerticalCount

        Return True
    End Function

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If IsLoadToCurrent Then
            '呼び出し
            If String.IsNullOrWhiteSpace(cmb上下図名.Text) Then
                '上下図名がセットされていません。
                MessageBox.Show(My.Resources.UpDownNoName, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            Dim row As tbl上下図Row = g_clsMasterTables.GetUpDownRecord(cmb上下図名.Text)
            If row Is Nothing Then
                '{0}'は登録されていません。登録名を選択してください。
                Dim msg As String = String.Format(My.Resources.UpDownNoExistName, cmb上下図名.Text)
                MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            Dim memo As String = CurrentUpdown.Memo
            If chk入れ換え.Checked Then
                CurrentUpdown.FromMasterRecord(row)
            Else
                CurrentUpdown.ReflectMasterRecord(row, chk繰り返す.Checked)
            End If
            If String.IsNullOrWhiteSpace(CurrentUpdown.Memo) Then
                If String.IsNullOrWhiteSpace(memo) Then
                    CurrentUpdown.Memo = cmb上下図名.Text
                Else
                    CurrentUpdown.Memo = memo 'あれば先を保持
                End If
            End If

        ElseIf IsSaveFromCurrent Then
            '保存
            If String.IsNullOrWhiteSpace(cmb上下図名.Text) Then
                '上下図名がセットされていません。
                MessageBox.Show(My.Resources.UpDownNoName, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            Dim row As tbl上下図Row = g_clsMasterTables.GetUpDownRecord(cmb上下図名.Text)
            If row IsNot Nothing Then
                '{0}'は既に登録されています。置き換えますか？(いいえで別の名前を指定)
                Dim msg As String = String.Format(My.Resources.UpDownExistName, cmb上下図名.Text)
                Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
                If r <> DialogResult.Yes Then
                    Exit Sub
                End If
                '置換(#39)
                CurrentUpdown.ToMasterRecord(row)
                row.AcceptChanges()
            Else
                '新たな名前
                row = g_clsMasterTables.GetNewUpDownRecord(cmb上下図名.Text)
                CurrentUpdown.ToMasterRecord(row)
            End If
            g_clsMasterTables.SaveUpDownTable(True)
        End If

        DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmb上下図名_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb上下図名.SelectedIndexChanged
        If IsLoadToCurrent Then
            '呼び出し
            If String.IsNullOrWhiteSpace(cmb上下図名.Text) Then
                txtl水平本数.Text = Nothing
                txtl垂直本数.Text = Nothing
                Exit Sub
            End If
            Dim row As tbl上下図Row = g_clsMasterTables.GetUpDownRecord(cmb上下図名.Text)
            If row Is Nothing Then
                txtl水平本数.Text = Nothing
                txtl垂直本数.Text = Nothing
                Exit Sub
            Else
                txtl水平本数.Text = row.f_i水平本数
                txtl垂直本数.Text = row.f_i垂直本数
            End If
        End If

    End Sub

    Private Sub chk入れ換え_CheckedChanged(sender As Object, e As EventArgs) Handles chk入れ換え.CheckedChanged
        chk繰り返す.Enabled = Not chk入れ換え.Checked
    End Sub
End Class