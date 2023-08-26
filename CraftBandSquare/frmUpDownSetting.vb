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
        '上下図名を選択してください。[OK]ボタンで現パターンに反映します。
        lblしてください.Text = My.Resources.UpDownReadInstruction
        Me.Text = My.Resources.UpDownRead

        Return True
    End Function

    Private Function SaveCurrent() As Boolean
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
            CurrentUpdown.FromMasterRecord(row)
            If String.IsNullOrWhiteSpace(CurrentUpdown.Memo) Then
                CurrentUpdown.Memo = cmb上下図名.Text
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
                    '置換
                    CurrentUpdown.ToMasterRecord(row)
                End If
            Else
                '新たな名前
                row = g_clsMasterTables.GetNewUpDownRecord(cmb上下図名.Text)
                CurrentUpdown.ToMasterRecord(row)
            End If
            g_clsMasterTables.SaveUpDownTable()
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
End Class