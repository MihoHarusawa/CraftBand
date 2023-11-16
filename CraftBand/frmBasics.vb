Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Public Class frmBasics

    'for Export
    Public DataPath As String = Nothing
    Public DataEditing As clsDataTables = Nothing


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
        '※新規ファイル指定時はまだ作成(保存)されていない可能性があります

        'サイズ復元
        Dim siz As Size
        If __paras.GetLastData("frmBasicsSize", siz) Then
            Me.Size = siz
        End If

        btnExport.Enabled = Not String.IsNullOrWhiteSpace(DataPath) AndAlso DataEditing IsNot Nothing

        _loaded = True
    End Sub



    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        '設定データの保存先が変更された場合
        If String.Compare(txt設定データの保存先.Text, g_clsMasterTables.MasterTablesFilePath, True) <> 0 Then
            If IO.File.Exists(txt設定データの保存先.Text) Then
                'ファイルあり
                '{0}が変更されました。選択されたファイル'{1}'を読み直しますか？(はい=読み直す,いいえ=現設定を上書きする)
                Dim msg As String = String.Format(My.Resources.AskReloadMasterDataFile, lbl設定データの保存先.Text, txt設定データの保存先.Text)
                Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                If r = DialogResult.Cancel Then
                    Exit Sub
                ElseIf r = DialogResult.Yes Then
                    '読み直す
                    If Not g_clsMasterTables.LoadFile(txt設定データの保存先.Text, True) Then
                        '指定ファイル'{0}'から設定データを読み取ることができませんでした。
                        msg = String.Format(My.Resources.ErrReadMasterTableFile, txt設定データの保存先.Text)
                        MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                    '読み取れたので入れ替え
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
                    '読み直さない。保存先の内容が上書きされる
                    g_clsMasterTables.MasterTablesFilePath = txt設定データの保存先.Text
                End If
            Else
                'ファイルなし
                '新しい{0}として'{1}'が指定されました。現在の設定をクリアしますか？(はい=ゼロから作り直す,いいえ=現設定維持)
                Dim msg As String = String.Format(My.Resources.AskResetMasterDataFile, lbl設定データの保存先.Text, txt設定データの保存先.Text)
                Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                If r = DialogResult.Cancel Then
                    Exit Sub
                ElseIf r = DialogResult.Yes Then
                    '作り直す
                    g_clsMasterTables.ReNew()
                Else
                    '現設定内容維持。保存先が変わるだけ
                End If
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
            btnImport.Enabled = False 'まだ読み取っていないため
            btnExport.Enabled = False
        End If
    End Sub

    'インポートボタン
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If OpenFileDialogImport.ShowDialog <> DialogResult.OK Then
            Exit Sub
        End If
        Dim msg As String
        If String.Compare(g_clsMasterTables.MasterTablesFilePath, OpenFileDialogImport.FileName, True) = 0 Then
            ''{0}'とは別の設定データファイルを指定してください。
            msg = String.Format(My.Resources.ErrMsgSameMasterTableFile, g_clsMasterTables.MasterTablesFilePath)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim importMaster As New clsMasterTables
        If Not importMaster.LoadFile(OpenFileDialogImport.FileName, True) Then
            '指定ファイル'{0}'から設定データを読み取ることができませんでした。
            msg = String.Format(My.Resources.ErrReadMasterTableFile, OpenFileDialogImport.FileName)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        '同名データに対しては、読み取り値で上書きしてよろしいですか？(はい=上書き,いいえ=現在値保持)
        msg = String.Format(My.Resources.AskOverwriteForSame, OpenFileDialogImport.FileName)
        Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r <> DialogResult.Yes AndAlso r <> DialogResult.No Then
            Exit Sub
        End If

        If g_clsMasterTables.Import(importMaster, (r = DialogResult.Yes), msg) Then
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
        If r = DialogResult.Yes Then
            g_clsSelectBasics.UpdateTargetBandType()
        End If
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    'エクスポートボタン
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If String.IsNullOrWhiteSpace(DataPath) OrElse DataEditing Is Nothing Then
            Exit Sub
        End If
        '推奨名
        Dim path As String = IO.Path.Combine(IO.Path.GetDirectoryName(DataPath), clsMasterTables.DefaultFileName & "-" & IO.Path.GetFileNameWithoutExtension(DataPath))
        path = IO.Path.ChangeExtension(path, clsMasterTables.MyExtention)
        SaveFileDialogExport.FileName = path
        If SaveFileDialogExport.ShowDialog <> DialogResult.OK Then
            Exit Sub
        End If
        Dim msg As String
        If String.Compare(g_clsMasterTables.MasterTablesFilePath, SaveFileDialogExport.FileName, True) = 0 Then
            ''{0}'とは別の設定データファイルを指定してください。
            msg = String.Format(My.Resources.ErrMsgSameMasterTableFile, g_clsMasterTables.MasterTablesFilePath)
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim exportMaster As New clsMasterTables
        Dim isOverWrite As Boolean = True
        If IO.File.Exists(SaveFileDialogExport.FileName) Then
            '既存あり
            If Not exportMaster.LoadFile(SaveFileDialogExport.FileName, True) Then
                '指定ファイル'{0}'から設定データを読み取ることができませんでした。
                msg = String.Format(My.Resources.ErrReadMasterTableFile, SaveFileDialogExport.FileName)
                MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            '同名データに対しては、現設定値で上書きしてよろしいですか？(はい=上書き,いいえ=既存値保持)
            msg = String.Format(My.Resources.AskOverwriteForExist, SaveFileDialogExport.FileName)
            Dim r As DialogResult = MessageBox.Show(msg, Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r <> DialogResult.Yes AndAlso r <> DialogResult.No Then
                Exit Sub
            End If
            isOverWrite = (r = DialogResult.Yes)
        Else
            '既存なし
            exportMaster.MasterTablesFilePath = SaveFileDialogExport.FileName
        End If

        msg = Nothing
        If g_clsMasterTables.Export(DataEditing, exportMaster, isOverWrite, msg) Then
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        '画面維持
    End Sub

    Private Sub frmBasics_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        __paras.SetLastData("frmBasicsSize", Me.Size)
    End Sub


End Class