Imports CraftBand.clsDataTables

Public Module mdlColorForm

    Public Class CColorChangeSetting

        '「f_s色」を持つレコードが対象

        Public ChkBoxText As String = Nothing       'チェックボックス表示
        Public TableID As enumDataID = Nothing   '対象テーブル
        Public Condition As String = Nothing        'テーブルの選択条件
        Public AtExpanded As Boolean = False    '展開時のみ対象か

        Sub New(ByVal chkname As String, ByVal id As enumDataID, ByVal cond As String, ByVal iexpand As Boolean)
            ChkBoxText = chkname
            TableID = id
            Condition = cond
            AtExpanded = iexpand
        End Sub

        '色の変更
        'Mesh   5   底(楕円) 側面 追加品    横ひも　縦ひも
        'Knot   4   側面と縁　追加品　   横ひも　縦ひも
        'Square 5   側面と縁　差しひも　追加品　  縦ひも　横ひも　
        'Sq45   4   縁の始末　追加品　   横ひも　縦ひも
        'Hex    6   側面と縁　差しひも　追加品　横ひも　斜め60度　斜め120度
    End Class

    Friend _frmColorChange As frmColorChange = Nothing

    Public Function CreateColorChangeForm(ByVal settings As CColorChangeSetting()) As Boolean
        _frmColorChange = New frmColorChange
        Return _frmColorChange.Initialize(settings)
    End Function

    '戻り値:マイナス=エラー 0以上:変更レコード数
    Public Function ShowColorChangeForm(ByVal data As clsDataTables, Optional ByVal isAlwaysExpand As Boolean = False) As Boolean
        If _frmColorChange Is Nothing OrElse Not _frmColorChange.IsInitialized Then
            Return -1
        End If

        If Not _frmColorChange.SetDataAndExpand(data, isAlwaysExpand) Then
            Return -1
        End If
        _frmColorChange.ShowDialog()
        Return _frmColorChange.ChangeCount
    End Function

    Public Function ShowColorChangeFormForUndef(ByVal data As clsDataTables, Optional ByVal isAlwaysExpand As Boolean = False) As Boolean
        If _frmColorChange Is Nothing OrElse Not _frmColorChange.IsInitialized Then
            Return -1
        End If

        If Not _frmColorChange.SetDataAndExpand(data, isAlwaysExpand) Then
            Return -1
        End If
        _frmColorChange.ShowDialogForUndef()
        Return _frmColorChange.ChangeCount
    End Function



    Public Class CColorRepeatSetting

        '「f_s色」「f_i何本幅」

        Public ChkBoxText As String = Nothing       'チェックボックス表示
        Public TableID As enumDataID = Nothing   '対象テーブル
        Public Condition As String = Nothing        'テーブルの選択条件
        Public Order As String = Nothing            '並び指定
        Public AdjustableLane As Boolean = False  '何本幅は変更可能か
        Public AtExpanded As Boolean = False    '展開時のみ対象か

        Sub New(ByVal chkname As String, ByVal id As enumDataID, ByVal cond As String, ByVal odr As String, ByVal ialane As Boolean, ByVal iexpand As Boolean)
            ChkBoxText = chkname
            TableID = id
            Condition = cond
            Order = odr
            AdjustableLane = ialane
            AtExpanded = iexpand
        End Sub

        '色の繰り返し
        'Mesh   3   長い横ひも　短い横ひも　縦ひも
        'Knot   3   編みひも　横ひも　縦ひも
        'Square 3   側面の編みひも　縦ひも　横ひも　
        'Sq45   2   横ひも　縦ひも
        'Hex    4   側面の編みひも　横ひも　斜め60度　斜め120度
    End Class


    Friend _frmColorRepeat As frmColorRepeat = Nothing

    Public Function CreateColorRepeatForm(ByVal settings As CColorRepeatSetting()) As Boolean
        _frmColorRepeat = New frmColorRepeat
        Return _frmColorRepeat.Initialize(settings)
    End Function

    '戻り値:マイナス=エラー 0以上:変更レコード数
    Public Function ShowColorRepeatForm(ByVal data As clsDataTables, Optional ByVal isAlwaysExpand As Boolean = False) As Boolean
        If _frmColorRepeat Is Nothing OrElse Not _frmColorRepeat.IsInitialized Then
            Return -1
        End If

        If Not _frmColorRepeat.SetDataAndExpand(data, isAlwaysExpand) Then
            Return -1
        End If
        _frmColorRepeat.ShowDialog()
        Return _frmColorRepeat.ChangeCount
    End Function

End Module
