﻿

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstOutput

Class clsCalcHexagon
    Public Const ROOT2 As Double = 1.41421356237

#Region "底の展開データ識別値"
    Enum AngleIndex
        idx_0 = 0    '横
        idx_60 = 60   '斜め60度
        idx_120 = 120  '斜め120度
    End Enum
    Friend Const cAngleCount As Integer = 3

    Shared Function idx(ByVal aidx As AngleIndex) As Integer
        Return CType(aidx, Integer) / 60
    End Function

    'ベースのひも種
    Shared _enumひも種() As enumひも種 = {enumひも種.i_横, enumひも種.i_60度, enumひも種.i_120度}

    Shared Function idxひも種(ByVal aidx As AngleIndex) As Integer
        Return CType(_enumひも種(idx(aidx)), Integer)
    End Function

    Shared Function idxひも種補強(ByVal aidx As AngleIndex) As Integer
        Return CType(_enumひも種(idx(aidx)) Or enumひも種.i_補強, Integer)
    End Function

    Shared Function idxひも種すき間(ByVal aidx As AngleIndex) As Integer
        Return CType(_enumひも種(idx(aidx)) Or enumひも種.i_すき間, Integer)
    End Function

    Shared Function is_idxひも種(ByVal aidx As AngleIndex, ByVal val As Integer) As Boolean
        Return val = idxひも種(aidx)
    End Function

    Shared Function is_idx補強(ByVal aidx As AngleIndex, ByVal val As Integer) As Boolean
        Return val = idxひも種補強(aidx)
    End Function

    Shared Function is_idxすき間(ByVal aidx As AngleIndex, ByVal val As Integer) As Boolean
        Return val = idxひも種すき間(aidx)
    End Function
#End Region





    Dim _ExpandCategory() As CalcCategory = {CalcCategory.Expand_0, CalcCategory.Expand_60, CalcCategory.Expand_120}

    '処理のカテゴリー
    Public Enum CalcCategory
        None

        NewData 'データ変更
        BsMaster  '基本値/マスター/バンド選択

        '目標寸法
        Target    '目標寸法
        Target_Band '基本のひも幅

        '配置数
        Hex_Gap  '目(ひも間のすき間)
        Hex_Yoko '横置き
        Hex_60120 '斜め置き
        Hex_Vert '高さ
        Hex_Expand '縦横展開

        'レコード変更/再計算
        SideEdge  '側面と縁
        Inserted '差しひも 
        UpDown  '編み目
        Options  '追加品
        Expand_0 '横
        Expand_60 '斜め60度
        Expand_120 '斜め120度

        '相互参照値のFix
        FixLength   '長さ確定

    End Enum

    Public Property p_b有効 As Boolean
    Public Property p_sメッセージ As String 'p_b有効でない場合のみ参照

    '目標値キャッシュ
    Private Property _d横_目標 As Double
    Private Property _d縦_目標 As Double
    Private Property _d高さ_目標 As Double
    Private Property _I基本のひも幅 As Integer
    Private Property _d基本のひも幅 As Double

    '配置数キャッシュ
    Dim _iひもの本数(cAngleCount - 1) As Integer '
    Dim _i何個目位置(cAngleCount - 1) As Integer
    Dim _d端の目(cAngleCount - 1) As Double '0～1
    Dim _b補強ひも(cAngleCount - 1) As Boolean '
    Dim _bひも本幅変更(cAngleCount - 1) As Boolean

    Private Property _i側面の編みひも数 As Integer '=高さの六つ目数
    Private Property _d最下段の目 As Double '0～1

    Private Property _d六つ目の高さ As Double
    Private Property _d六つ目の対角線 As Double
    Private Property _dひも長係数 As Double
    Private Property _dひも長加算_上端 As Double
    Private Property _dひも長加算_側面 As Double

    Private Property _b縦横側面を展開する As Boolean


    '計算結果の保持
    Dim _d六角ベース計(cAngleCount - 1) As Double '目の数,端の目,ひも幅計,すき間
    Dim _d六角ベース中(cAngleCount - 1) As Double '真ん中の目の中央まで

    Private Property _d六角ベース_高さ計 As Double '高さの目数,最下段の目,編みひも幅計,すき間　(縁は含まない)


    '※タブ表示時に編集、Hide時に保存、非表示時にも目の数を合わせる
    '側面と縁: _Data.p_tbl側面
    Private Property _b側面ひも本幅変更 As Boolean
    Private Property _d縁の高さ As Double '縁の合計値,ゼロ以上
    Private Property _d縁の垂直ひも長 As Double '縁の合計値,ゼロ以上
    Private Property _d縁の厚さ As Double '縁の最大値,ゼロ以上

    '※ここまでの集計値については、CalcSizeで正しく得られること。
    '　レコード内のひも長については、1Pass処理値とし、不正確な可能性あり
    '　リスト出力時に、再計算して合わせる


    '※タブ表示時に同期、非表示時はそのまま保持
    '側面と縁: _Data.p_tbl側面
    '差しひも: _Data.p_tbl差しひも
    '追加品:   _Data.p_tbl追加品

    '※計算用テーブルを保持、タブ表示時に同期、非表示時はそのまま保持
    '横ひも:   _tbl縦横展開_横ひも
    '縦ひも:   _tbl縦横展開_縦ひも

    '※タブ表示時に一時テーブル展開して編集、非表示時は無し
    'ひも上下


    '初期化
    Public Sub Clear()
        p_b有効 = False
        p_sメッセージ = Nothing

        _d横_目標 = -1
        _d縦_目標 = -1
        _d高さ_目標 = -1
        _I基本のひも幅 = -1
        _d基本のひも幅 = -1

        For idx As Integer = 0 To cAngleCount - 1
            _iひもの本数(idx) = -1
            _i何個目位置(idx) = -1
            _b補強ひも(idx) = False
            _d端の目(idx) = -1
            _d六角ベース計(idx) = -1
            _d六角ベース中(idx) = -1
            _bひも本幅変更(idx) = False
            _tbl縦横展開(idx).Clear()
        Next
        _i側面の編みひも数 = -1
        _d最下段の目 = -1

        _d六つ目の高さ = -1
        _d六つ目の対角線 = -1

        _dひも長係数 = -1
        _dひも長加算_上端 = -1
        _dひも長加算_側面 = -1

        _b縦横側面を展開する = False
        _b側面ひも本幅変更 = False

        _d六角ベース_高さ計 = -1

        _d縁の高さ = 0
        _d縁の垂直ひも長 = 0
        _d縁の厚さ = 0

    End Sub

#Region "プロパティ値"
    Public ReadOnly Property p_iひもの本数(ByVal idx As AngleIndex) As Integer
        Get
            Return _iひもの本数(idx)
        End Get
    End Property

    Public ReadOnly Property p_i側面ひもの本数 As Integer
        Get
            Return _i側面の編みひも数
        End Get
    End Property

    'ひも幅+すき間
    Public ReadOnly Property p_d縦横_四角 As Double
        Get
            If 0 <= _d六つ目の高さ AndAlso 0 < _d基本のひも幅 Then
                Return _d六つ目の高さ + _d基本のひも幅
            Else
                Return 0
            End If
        End Get
    End Property

    ReadOnly Property p_d厚さ As Double
        Get
            If _d縁の厚さ < g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ") Then
                Return g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
            Else
                Return _d縁の厚さ
            End If
        End Get
    End Property

    Public ReadOnly Property p_d四角ベース_横 As Double '底ひもが配置される領域
        Get
            Return _d六角ベース計(0)
        End Get
    End Property
    Public ReadOnly Property p_d四角ベース_縦 As Double '底ひもが配置される領域
        Get
            Return _d六角ベース計(1)
        End Get
    End Property
    Public ReadOnly Property p_d四角ベース_高さ As Double '側面ひもが配置される高さ
        Get
            Return _d六角ベース_高さ計
        End Get
    End Property

    Public ReadOnly Property p_bひも本幅変更(ByVal aidx As AngleIndex) As Boolean
        Get
            Return _bひも本幅変更(idx(aidx))
        End Get
    End Property

    Public ReadOnly Property p_b側面ひも本幅変更() As Boolean
        Get
            Return _b側面ひも本幅変更
        End Get
    End Property

#Region "表示用(計算には使わない)"
    ReadOnly Property p_d四角ベース_周 As Double '底の配置領域に立ち上げ増分をプラス
        Get
            If 0 <= p_d四角ベース_横 AndAlso 0 <= p_d四角ベース_縦 Then
                Return 2 * (p_d四角ベース_横 + p_d四角ベース_縦) _
                + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分")
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property p_d縁厚さプラス_周 As Double
        Get
            If 0 <= p_d四角ベース_周 Then
                Return p_d四角ベース_周 + 8 * p_d厚さ
            End If
            Return 0
        End Get
    End Property

    '表示文字列
    Public ReadOnly Property p_s縦横_目 As String
        Get
            If 0 < _d六つ目の高さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d六つ目の高さ)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s縦横_四角 As String
        Get
            If 0 < p_d縦横_四角 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d縦横_四角)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s対角線_目 As String
        Get
            If 0 < _d六つ目の対角線 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d六つ目の対角線)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s対角線_四角 As String
        Get
            If 0 < p_d縦横_四角 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text((p_d縦横_四角) * ROOT2)
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property p_s四角ベース_横 As String
        Get
            If 0 <= p_d四角ベース_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d四角ベース_横)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s四角ベース_縦 As String
        Get
            If 0 <= p_d四角ベース_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d四角ベース_縦)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s四角ベース_高さ As String
        Get
            If 0 <= p_d四角ベース_高さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d四角ベース_高さ)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s四角ベース_周 As String
        Get
            If 0 < p_d四角ベース_周 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d四角ベース_周)
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property p_d縁厚さプラス_横 As Double
        Get
            If 0 <= p_d四角ベース_横 Then
                Return p_d四角ベース_横 + p_d厚さ * 2
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property p_s縁厚さプラス_横 As String
        Get
            If 0 <= p_d縁厚さプラス_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d縁厚さプラス_横)
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property p_d縁厚さプラス_縦 As Double
        Get
            If 0 <= p_d四角ベース_縦 Then
                Return p_d四角ベース_縦 + p_d厚さ * 2
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property p_s縁厚さプラス_縦 As String
        Get
            If 0 <= p_d縁厚さプラス_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d縁厚さプラス_縦)
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property p_d縁厚さプラス_高さ As Double
        Get
            Return _d六角ベース_高さ計 + _d縁の高さ +
                g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
        End Get
    End Property
    Public ReadOnly Property p_s縁厚さプラス_高さ As String
        Get
            Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d縁厚さプラス_高さ)
        End Get
    End Property

    Public ReadOnly Property p_s縁厚さプラス_周 As String
        Get
            If 0 < p_d縁厚さプラス_周 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d縁厚さプラス_周)
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property p_s厚さ As String
        Get
            If 0 <= p_d厚さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d厚さ)
            End If
            Return ""
        End Get
    End Property

    '計算寸法と目標寸法の差(四角ベースの差)
    Public ReadOnly Property p_s横寸法の差 As String
        Get
            If (0 < _d横_目標) AndAlso (0 < _d六角ベース計(0)) Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d六角ベース計(0) - _d横_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s縦寸法の差 As String
        Get
            If (0 < _d縦_目標) AndAlso (0 < _d六角ベース計(1)) Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d六角ベース計(1) - _d縦_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s高さ寸法の差 As String
        Get
            If (0 <= _d高さ_目標) AndAlso 0 <= _d六角ベース_高さ計 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d六角ベース_高さ計 - _d高さ_目標, True)
            End If
            Return ""
        End Get
    End Property
#End Region

    Public ReadOnly Property p_i垂直ひも数 As Integer
        Get
            'If 0 < _i横の目の数 AndAlso 0 < _i縦の目の数 Then
            Return 2 * (p_iひもの本数(0) + p_iひもの本数(1) + p_iひもの本数(2))
            'Else
            '    Return 0
            'End If
        End Get
    End Property

    Public ReadOnly Property p_i垂直ひも半数 As Integer
        Get
            Return p_iひもの本数(0) + p_iひもの本数(1) + p_iひもの本数(2)
        End Get
    End Property




    'データ内容
    Public Function dump() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("Properties Of {0} ({1})", Me.GetType.Name, IIf(p_b有効, "Valid", "InValid")).AppendLine()

        'プロパティ値
        Const flgs As BindingFlags = BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static Or BindingFlags.GetField Or BindingFlags.GetProperty Or BindingFlags.DeclaredOnly
        Dim members As MemberInfo() = Me.GetType.GetMembers(flgs)
        For Each mem In members
            If mem.MemberType = MemberTypes.Property Then
                Try
                    sb.AppendFormat(" *{0} = '{1}'", mem.Name, (Me.GetType.InvokeMember(mem.Name, flgs, Nothing, Me, Nothing))).AppendLine()
                Catch ex As Exception
                    sb.AppendFormat(" *Exception!! {0} - {1}", mem.Name, ex.Message).AppendLine()
                End Try
            Else
                'Debug.Print(" {0} - {1}", mem.MemberType, mem.Name)
            End If
        Next

        Return sb.ToString
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("{0}({1}){2}", Me.GetType.Name, IIf(p_b有効, "Valid", "InValid"), p_sメッセージ).AppendLine()
        sb.AppendFormat("Target:({0},{1},{2}) Lane({3}){4}", _d横_目標, _d縦_目標, _d高さ_目標, _I基本のひも幅, _d基本のひも幅).AppendLine()
        sb.AppendFormat("  0:({0},mark({1}),end({2}),add({3}),change({4}))", _iひもの本数(0), _i何個目位置(0), _d端の目(0), _b補強ひも(0), _bひも本幅変更(0)).AppendLine()
        sb.AppendFormat(" 60:({0},mark({1}),end({2}),add({3}),change({4}))", _iひもの本数(1), _i何個目位置(1), _d端の目(1), _b補強ひも(1), _bひも本幅変更(1)).AppendLine()
        sb.AppendFormat("120:({0},mark({1}),end({2}),add({3}),change({4}))", _iひもの本数(2), _i何個目位置(2), _d端の目(2), _b補強ひも(2), _bひも本幅変更(2)).AppendLine()
        'sb.AppendFormat("Depth:({0},{1}) Periphery:{2}", _i縦の目の数, _d四角ベース_縦計, p_d四角ベース_周).AppendLine()
        sb.AppendFormat("Height:({0},{1})", _i側面の編みひも数, _d六角ベース_高さ計).AppendLine()
        sb.AppendFormat("Edge({0}) VerticalLength({1}) Thickness({2})", _d縁の高さ, _d縁の垂直ひも長, _d縁の厚さ).AppendLine()
        Return sb.ToString
    End Function

#End Region



    Dim _Data As clsDataTables
    Dim _frmMain As frmMain

    Sub New(ByVal data As clsDataTables, ByVal frm As frmMain)
        _Data = data
        _frmMain = frm

        '最初に1点だけ作っておく。以降は存在が前提
        _tbl縦横展開(idx(AngleIndex.idx_0)) = New tbl縦横展開DataTable
        _tbl縦横展開(idx(AngleIndex.idx_60)) = New tbl縦横展開DataTable
        _tbl縦横展開(idx(AngleIndex.idx_120)) = New tbl縦横展開DataTable

        Clear()
    End Sub

    Function CalcSize(ByVal category As CalcCategory, ByVal ctr As Object, ByVal key As Object) As Boolean
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CalcSize {0} {1} {2}", category, ctr, key)

        Dim ret As Boolean = True
        Select Case category
            Case CalcCategory.NewData, CalcCategory.BsMaster
                'データ変更/基本値/マスター/バンド選択
                Clear()
                ret = ret And set_目標寸法(False)
                ret = ret And set_配置数()
                '
                ret = ret And calc_側面と縁(category, Nothing, Nothing)     '高さ系
                ret = ret And calc_ひも展開(AngleIndex.idx_0, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_60, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_120, category, Nothing, Nothing)

                ret = ret And calc_差しひも(category, Nothing, Nothing)
                If ret Then
                    p_sメッセージ = _frmMain.editAddParts.CheckError(_Data)
                    If Not String.IsNullOrEmpty(p_sメッセージ) Then
                        ret = False
                    End If
                End If

            Case CalcCategory.Target    '目標寸法(※基本のひも幅以外)
                ret = ret And set_目標寸法(False)
                '(差の表示が変わるだけ)

            Case CalcCategory.Target_Band     '基本のひも幅
                ret = ret And set_目標寸法(False)
                '
                ret = ret And calc_側面と縁(category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_0, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_60, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_120, category, Nothing, Nothing)

        '四角数のタブ内
            Case CalcCategory.Hex_Gap     '目(ひも間のすき間),ひも長係数,ひも長加算
                ret = ret And set_配置数()
                '
                ret = ret And calc_側面と縁(category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_0, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_60, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_120, category, Nothing, Nothing)

            Case CalcCategory.Hex_Yoko '四角数タブ(横の目の数)縦ひもの本数
                ret = ret And set_配置数()
                '
                ret = ret And calc_ひも展開(AngleIndex.idx_0, category, Nothing, Nothing)

            Case CalcCategory.Hex_60120 '四角数タブ(縦の目の数)横ひもの本数
                ret = ret And set_配置数()
                '
                ret = ret And calc_ひも展開(AngleIndex.idx_60, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_120, category, Nothing, Nothing)

            Case CalcCategory.Hex_Vert '四角数タブ高さ
                ret = ret And set_配置数()
                '
                ret = ret And calc_側面と縁(category, Nothing, Nothing)

            Case CalcCategory.Hex_Expand '四角数タブ縦横展開
                ret = ret And set_配置数()
                '何本幅かを変更していた場合の影響
                ret = ret And calc_側面と縁(category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_0, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_60, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_120, category, Nothing, Nothing)

        '以下、row指定がある場合は、そのタブが表示された状態
            Case CalcCategory.SideEdge  '側面と縁
                Dim row As tbl側面Row = CType(ctr, tbl側面Row)
                ret = ret And calc_側面と縁(category, row, key)

            Case CalcCategory.Inserted '差しひも
                Dim row As tbl差しひもRow = CType(ctr, tbl差しひもRow)
                ret = ret And calc_差しひも(category, row, key)
                  '(差しひもは計算寸法変更なし)

            Case CalcCategory.UpDown 'ひも上下
                '(ひも上下は計算寸法変更なし)

            Case CalcCategory.Options  '追加品
                'editAddParts内の処理, エラー文字列表示のみ
                p_sメッセージ = _frmMain.editAddParts.CheckError(_Data)
                If Not String.IsNullOrEmpty(p_sメッセージ) Then
                    ret = False
                End If
                '(追加品は計算寸法変更なし)

            Case CalcCategory.Expand_0  '横ひも展開
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                ret = ret And calc_ひも展開(AngleIndex.idx_0, category, row, key)

            Case CalcCategory.Expand_60  '斜め60度
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                ret = ret And calc_ひも展開(AngleIndex.idx_60, category, row, key)

            Case CalcCategory.Expand_120  '斜め120度
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                ret = ret And calc_ひも展開(AngleIndex.idx_120, category, row, key)

            '相互参照値のFix(1Pass値は得られている前提)
            Case CalcCategory.FixLength
                ret = ret And calc_側面と縁(category, Nothing, Nothing)     '高さ系
                ret = ret And calc_ひも展開(AngleIndex.idx_0, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_60, category, Nothing, Nothing)
                ret = ret And calc_ひも展開(AngleIndex.idx_120, category, Nothing, Nothing)
                ret = ret And calc_差しひも(category, Nothing, Nothing)

            Case Else
                '未定義のカテゴリー'{0}'が参照されました。
                p_sメッセージ = String.Format(My.Resources.CalcNoDefCategory, category)
                ret = False

        End Select

        p_b有効 = ret
        Return ret
    End Function

    '目標寸法をセットする
    'needTarget=Trueの時、値がなければFalse
    Private Function set_目標寸法(ByVal needTarget As Boolean) As Boolean

        With _Data.p_row目標寸法
            _d横_目標 = .Value("f_d横寸法")
            _d縦_目標 = .Value("f_d縦寸法")
            _d高さ_目標 = .Value("f_d高さ寸法")
            _I基本のひも幅 = .Value("f_i基本のひも幅")
            _d基本のひも幅 = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)
        End With

        If Not isValidTarget(needTarget) Then
            Return False
        End If

        Return True
    End Function


    '配置数タブの取得
    'IN: _I基本のひも幅(チェック時)
    Function set_配置数() As Boolean

        With _Data.p_row底_縦横
            _iひもの本数(idx(AngleIndex.idx_0)) = .Value("f_i長い横ひもの本数")
            _iひもの本数(idx(AngleIndex.idx_60)) = .Value("f_i斜め60度ひも本数")
            _iひもの本数(idx(AngleIndex.idx_120)) = .Value("f_i斜め120度ひも本数")

            _i何個目位置(idx(AngleIndex.idx_0)) = .Value("f_i上から何番目")
            _i何個目位置(idx(AngleIndex.idx_60)) = .Value("f_i左から何番目")
            _i何個目位置(idx(AngleIndex.idx_120)) = .Value("f_i左から何番目") '兼用

            _i側面の編みひも数 = .Value("f_d高さの四角数")

            _d六つ目の高さ = .Value("f_dひも間のすき間")
            _dひも長係数 = .Value("f_dひも長係数")
            _dひも長加算_上端 = .Value("f_d垂直ひも長加算")
            _dひも長加算_側面 = .Value("f_dひも長加算_側面")

            _b縦横側面を展開する = .Value("f_b展開区分")
            _d最下段の目 = .Value("f_d最下段の目")

            _b補強ひも(idx(AngleIndex.idx_0)) = .Value("f_b補強ひも区分")
            _b補強ひも(idx(AngleIndex.idx_60)) = .Value("f_b斜めの補強ひも区分")
            _b補強ひも(idx(AngleIndex.idx_120)) = .Value("f_b斜めの補強ひも2区分")

            _d端の目(idx(AngleIndex.idx_0)) = .Value("f_d上端下端の目")
            _d端の目(idx(AngleIndex.idx_60)) = .Value("f_d左端右端の目")
            _d端の目(idx(AngleIndex.idx_120)) = .Value("f_d左端右端の目") '兼用

            _d六つ目の対角線 = _d六つ目の高さ * 2 / Math.Sqrt(3)

        End With

        Return IsValidInput()
    End Function

#Region "概算"

    '有効な目標寸法がある
    Public Function isValidTarget(ByVal needTarget As Boolean) As Boolean
        If needTarget Then
            If _d横_目標 <= 0 AndAlso _d縦_目標 <= 0 AndAlso _d高さ_目標 <= 0 Then
                '目標とする縦寸法・横寸法・高さ寸法を設定してください。
                p_sメッセージ = My.Resources.CalcNoTargetSet
                Return False
            End If
        End If
        '常に必要
        If _I基本のひも幅 <= 0 Then
            '基本のひも幅を設定してください。
            p_sメッセージ = My.Resources.CalcNoBaseBandSet
            Return False
        End If

        Return True
    End Function

    '有効な入力がある
    Public Function IsValidInput() As Boolean
        For idx As Integer = 0 To cAngleCount - 1
            If _iひもの本数(idx) < 0 Then
                'ひもの本数をセットしてください。
                p_sメッセージ = My.Resources.CalcNoCountSet
                Return False
            ElseIf _iひもの本数(idx) < 2 Then
                '六つ目を作るために、各ひも2本以上にしてください。
                p_sメッセージ = My.Resources.CalcBandCountAtLeast
                Return False
            ElseIf _i何個目位置(idx) < 1 Then    '六つ目のマーク位置、1以上
                '六つ目を作るために、各ひも2本以上にしてください。
                p_sメッセージ = My.Resources.CalcBandCountAtLeast
                Return False
            ElseIf _iひもの本数(idx) <= _i何個目位置(idx) Then '六つ目数=本数-1
                'ひもの本数をセットしてください。
                p_sメッセージ = My.Resources.CalcNoCountSet
                Return False    '回避されているはずなので仮メッセージ
            End If
        Next
        If _i側面の編みひも数 < 0 Then
            'ひもの本数をセットしてください。
            p_sメッセージ = My.Resources.CalcNoCountSet
            Return False
        End If
        If _I基本のひも幅 <= 0 Then
            '基本のひも幅を設定してください。
            p_sメッセージ = My.Resources.CalcNoBaseBandSet
            Return False
        End If
        If _dひも長係数 < 0.5 Then   '半分以下はNGとする
            'ひも長係数が小さすぎます。通常1以上の値です。
            p_sメッセージ = My.Resources.CalcSmallLengthRatio
            Return False
        End If

        Return True
    End Function


    '目標寸法→底_縦横(チェックと確認)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="message">確認メッセージ</param>
    ''' <returns></returns>
    Public Function CheckTarget(ByRef message As String) As Boolean
        If Not isCalcable() Then
            Return False
        End If

        If Not IsValidInput() Then
            message = Nothing
            Return True
        End If
        '有効な入力がある場合

        '概算不要か？
        If isNear(p_d四角ベース_横, _d横_目標) _
            AndAlso isNear(p_d四角ベース_縦, _d縦_目標) _
            AndAlso isNear(p_d四角ベース_高さ, _d高さ_目標) Then
            'ほぼ目標のサイズになっています。やり直す場合はリセットしてください。
            p_sメッセージ = My.Resources.CalcNoMoreChange
            Return False
        End If

        '目標に基づき横・縦・高さのひも(目)数を再計算します。よろしいですか？
        message = My.Resources.CalcConfirmRecalc

        Return True
    End Function

    Public Function CalcTarget() As Boolean
        If Not isCalcable() Then
            Return False
        End If
        Return calc_Target()
    End Function

    Private Function isCalcable() As Boolean
        If Not set_目標寸法(True) Then
            Return False
        End If
        If Not (0 < _d横_目標) OrElse Not (0 < _d縦_目標) OrElse Not (0 <= _d高さ_目標) OrElse Not (0 < _I基本のひも幅) Then
            '目標寸法もしくは基本のひも幅が正しくありません。
            p_sメッセージ = My.Resources.CalcNoTargetSet
            Return False
        End If

        Return True
    End Function

    Private Function isNear(ByVal d1 As Double, ByVal d2 As Double) As Boolean
        If d1 < 0 OrElse d2 < 0 Then
            Return False
        End If
        Dim diff As Double = IIf(d1 < d2, d2 - d1, d1 - d2)
        Return diff < g_clsSelectBasics.p_lenバンド幅.Value
    End Function

    '目標寸法→横・縦・高さの四角数
    Private Function calc_Target() As Boolean
        If p_d縦横_四角 <= 0 Then
            Return False
        End If

        Dim ret As Boolean = True
        ret = ret And calc_Target_横()
        ret = ret And calc_Target_縦()
        ret = ret And calc_Target_高さ()
        Return ret
    End Function

    '横寸法から横に並ぶ四角数・偶数
    Private Function calc_Target_横() As Boolean
        Dim i横の四角数 As Integer = Int((_d横_目標 - _d基本のひも幅) / p_d縦横_四角) '#22
        If _Data.p_row目標寸法.Value("f_b内側区分") Then
            '内側
            If i横の四角数 Mod 2 <> 0 Then
                i横の四角数 -= 1
            End If
        Else
            '外側
            Do While i横の四角数 * p_d縦横_四角 < (_d横_目標 - _d基本のひも幅)
                i横の四角数 += 1
            Loop
            If i横の四角数 Mod 2 <> 0 Then
                i横の四角数 += 1
            End If
        End If
        If i横の四角数 <= 0 Then
            i横の四角数 = 1
        End If

        _Data.p_row底_縦横.Value("f_i横の四角数") = i横の四角数
        _Data.p_row底_縦横.Value("f_i縦ひもの本数") = i横の四角数 + 1
        _Data.p_row底_縦横.Value("f_d左端右端の目") = 0

        Return True
    End Function

    '縦寸法から縦に並ぶ四角数・偶数
    Private Function calc_Target_縦()
        Dim i縦の四角数 As Integer = Int((_d縦_目標 - _d基本のひも幅) / p_d縦横_四角) '#22
        If _Data.p_row目標寸法.Value("f_b内側区分") Then
            '内側
            If i縦の四角数 Mod 2 <> 0 Then
                i縦の四角数 -= 1
            End If
        Else
            Do While i縦の四角数 * p_d縦横_四角 < (_d縦_目標 - _d基本のひも幅)
                i縦の四角数 += 1
            Loop
            If i縦の四角数 Mod 2 <> 0 Then
                i縦の四角数 += 1
            End If
        End If
        If i縦の四角数 <= 0 Then
            i縦の四角数 = 1
        End If

        _Data.p_row底_縦横.Value("f_i縦の四角数") = i縦の四角数
        _Data.p_row底_縦横.Value("f_i長い横ひもの本数") = i縦の四角数 + 1
        _Data.p_row底_縦横.Value("f_d上端下端の目") = 0

        Return True
    End Function

    '高さ寸法から高さの四角数
    Private Function calc_Target_高さ()
        Dim i高さの四角数 As Integer = Int((_d高さ_目標 - _d六つ目の高さ) / p_d縦横_四角)
        If Not _Data.p_row目標寸法.Value("f_b内側区分") Then
            Do While i高さの四角数 * p_d縦横_四角 < (_d高さ_目標 - _d六つ目の高さ)
                i高さの四角数 += 1
            Loop
        End If
        If i高さの四角数 = 0 AndAlso 0 < _d高さ_目標 Then
            i高さの四角数 = 1
        End If

        _Data.p_row底_縦横.Value("f_d高さの四角数") = i高さの四角数 'double
        _Data.p_row底_縦横.Value("f_i高さのコマ数") = i高さの四角数 '念のためのinteger
        _Data.p_row底_縦横.Value("f_d最下段の目") = 1

        Return True
    End Function

#End Region

#Region "側面と縁"
    Const cIdxSpace As Integer = 0 '最下段スペースの番号
    Const cIdxHeight As Integer = 1 '側面の編みひもの番号

    'f_i番号=0    _d最下段の目があれば、レコードを作る
    'f_i番号=1    各編みひものレコード、展開しない場合は1レコード
    'f_i番号=1    各編みひものレコード、展開する場合はひも番号1～_i高さの目の数
    '             　高さ・垂直ひも長は、全てひも本幅数分の幅+_d目_ひも間のすき間

    'リスト出力値=f_d連続ひも長(出力ひも長) f_dひも長は係数乗算、ひも長加算は2PassでFix


    'p_tbl側面の縁を除くレコードを現在の各設定値に合わせる　※中でadjust_側面(row)呼び出し
    'IN:    _b縦横側面を展開する,_i高さの目の数,_d最下段の目,_d目_ひも間のすき間
    'OUT:    
    Public Function adjust_側面() As Boolean

        Dim table As tbl側面DataTable = _Data.p_tbl側面
        Dim row As tbl側面Row
        Dim ret As Boolean = True

        If 0 < _d最下段の目 * _d六つ目の高さ Then
            '最下段のスペースのみ
            row = clsDataTables.NumberSubRecord(table, cIdxSpace, 1)
            If row Is Nothing Then
                row = table.Newtbl側面Row
                row.f_i番号 = cIdxSpace
                row.f_iひも番号 = 1
                row.f_i何本幅 = _I基本のひも幅
                table.Rows.Add(row)
            End If
            row.f_s編みかた名 = text最下段()
            row.f_s編みひも名 = text高さの目の数()
            row.f_iひも本数 = 0
            row.f_d高さ = _d最下段の目 * _d六つ目の高さ
            row.f_d垂直ひも長 = _d最下段の目 * _d六つ目の高さ
            row.f_d周長 = 0
            row.f_dひも長 = 0
            row.f_d厚さ = 0
            row.f_i周数 = 0
            row.f_s色 = ""
        Else
            RemoveNumberFromTable(table, cIdxSpace)
        End If

        If 0 < p_i側面ひもの本数 Then
            '「編みひも+目」のセット
            Dim colprv As String = ""
            For i As Integer = 1 To p_i側面ひもの本数
                row = clsDataTables.NumberSubRecord(table, cIdxHeight, i)
                If i = 1 OrElse _b縦横側面を展開する Then
                    If row Is Nothing Then
                        row = table.Newtbl側面Row
                        row.f_i番号 = cIdxHeight
                        row.f_iひも番号 = i
                        row.f_i何本幅 = _I基本のひも幅
                        table.Rows.Add(row)
                    ElseIf i = 1 Then
                        colprv = row.f_s色
                    End If
                    row.f_s編みかた名 = String.Format(My.Resources.FormCaption, "")
                    row.f_s編みひも名 = text側面の編みひも()
                    If _b縦横側面を展開する Then
                        row.f_iひも本数 = 1
                        row.f_i周数 = 1
                        If String.IsNullOrWhiteSpace(row.f_s色) Then
                            row.f_s色 = colprv
                        Else
                            colprv = row.f_s色
                        End If
                    Else
                        row.f_i何本幅 = _I基本のひも幅
                        row.f_iひも本数 = p_i側面ひもの本数
                        row.f_i周数 = p_i側面ひもの本数
                    End If
                    ret = ret And adjust_側面(row)
                Else
                    If row IsNot Nothing Then
                        table.Rows.Remove(row)
                    End If
                End If
            Next

            '以降はあれば削除
            Dim submax As Integer = clsDataTables.NumberSubMax(table, cIdxHeight)
            For i As Integer = p_i側面ひもの本数 + 1 To submax
                row = clsDataTables.NumberSubRecord(table, cIdxHeight, i)
                If row IsNot Nothing Then
                    table.Rows.Remove(row)
                End If
            Next

        Else
            RemoveNumberFromTable(table, cIdxHeight)
        End If

        Return ret
    End Function

    'cIdxHeightレコード対象、高さと垂直ひも長の計算
    'IN:    p_d縁厚さプラス_周,_dひも長係数,_dひも長加算_側面,f_d底の厚さ,f_d連続ひも長
    'OUT:   各、f_d高さ,f_d垂直ひも長
    Private Function adjust_側面(ByVal row As tbl側面Row) As Boolean
        If 0 < row.f_iひも本数 Then
            row.f_d高さ = row.f_iひも本数 * (g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _d六つ目の高さ)
            row.f_d垂直ひも長 = row.f_d高さ
        Else
            row.Setf_d高さNull()
            row.Setf_d垂直ひも長Null()
        End If
        row.f_d周長 = get側面の周長()
        row.f_dひも長 = row.f_d周長 * _dひも長係数
        row.f_d連続ひも長 = row.f_dひも長 + _dひも長加算_側面 + row.f_dひも長加算
        row.f_d厚さ = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")

        Return True
    End Function

    '縁を含まない高さ(四角ベース_高さ)を計算
    'IN:    
    'OUT:   _d四角ベース_高さ計,_b側面ひも本幅変更
    Private Function calc_側面計() As Boolean
        '高さの合計
        Dim cond As String = String.Format("(f_i番号 = {0}) OR (f_i番号 = {1})", cIdxSpace, cIdxHeight)
        Dim obj As Object = _Data.p_tbl側面.Compute("SUM(f_d高さ)", cond)
        If IsDBNull(obj) OrElse obj < 0 Then
            _d六角ベース_高さ計 = 0
        Else
            _d六角ベース_高さ計 = obj
        End If

        '幅が変更されているか
        cond = String.Format("f_i番号 = {0}", cIdxHeight)
        obj = _Data.p_tbl側面.Compute("SUM(f_i何本幅)", cond)
        If IsDBNull(obj) OrElse obj <= 0 Then
            '高さゼロ
            _b側面ひも本幅変更 = False
            Return True
        End If

        Dim iMax何本幅 As Integer = 0
        obj = _Data.p_tbl側面.Compute("MAX(f_i何本幅)", cond)
        If Not IsDBNull(obj) AndAlso 0 < obj Then
            iMax何本幅 = obj
        End If

        Dim iMin何本幅 As Integer = 0
        obj = _Data.p_tbl側面.Compute("MIN(f_i何本幅)", cond)
        If Not IsDBNull(obj) AndAlso 0 < obj Then
            iMin何本幅 = obj
        End If

        _b側面ひも本幅変更 = _I基本のひも幅 <> iMax何本幅 OrElse _I基本のひも幅 <> iMin何本幅

        Return True
    End Function


    '追加ボタンによる、縁の編みかた追加
    Function add_縁(ByVal nameselect As String,
                     ByVal i何本幅 As Integer, ByRef row As tbl側面Row) As Boolean

        If String.IsNullOrWhiteSpace(nameselect) Then
            '{0}を指定してください。
            p_sメッセージ = String.Format(My.Resources.CalcNoSelect, text編みかた名())
            Return False
        End If

        'tbl編みかたRow
        Dim grpMst As New clsGroupDataRow(g_clsMasterTables.GetPatternRecordGroup(nameselect))
        If Not grpMst.IsValid Then
            '{0}'{1}'は登録されていません。
            p_sメッセージ = String.Format(My.Resources.CalcNoMaster, text編みかた名(), nameselect)
            Return False
        End If

        Dim table As tbl側面DataTable = _Data.p_tbl側面
        Dim addNumber As Integer = clsDataTables.cHemNumber
        clsDataTables.RemoveNumberFromTable(table, addNumber)

        'tbl編みかたぶんのレコード
        Dim groupRow As New clsGroupDataRow("f_iひも番号")
        For Each idx As Int16 In grpMst.Keys
            row = table.Newtbl側面Row
            row.f_i番号 = addNumber
            row.f_iひも番号 = idx
            row.f_s編みかた名 = nameselect
            row.f_i周数 = 1

            groupRow.Add(row)
            table.Rows.Add(row)
        Next
        groupRow.SetNameIndexValue("f_s編みひも名", grpMst)
        groupRow.SetNameIndexValue("f_dひも長加算", grpMst, "f_dひも長加算初期値")
        groupRow.SetNameIndexValue("f_sメモ", grpMst, "f_s備考")

        For Each drow As clsDataRow In groupRow
            Dim mst As New clsOptionDataRow(grpMst.IndexDataRow(drow)) '必ずある
            drow.Value("f_i何本幅") = mst.GetFirstLane(i何本幅)
        Next
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Side Add: {0}", groupRow.ToString)
        Return True
    End Function

    '追加ボタンによる側面の高さ追加
    'Trueを返すと、四角数のタブの高さを+1
    Function add_高さ(ByRef currow As tbl側面Row) As Boolean
        If Not _b縦横側面を展開する OrElse p_i側面ひもの本数 = 0 Then
            Return True '高さをUPするだけ
        End If

        '追加位置
        Dim bandnum As Integer = -1
        If currow.f_i番号 = cIdxHeight Then
            bandnum = currow.f_iひも番号
        ElseIf currow.f_i番号 = cIdxSpace Then
            bandnum = 1 '最初
        ElseIf currow.f_i番号 = cHemNumber Then
            bandnum = p_i側面ひもの本数 + 1  '最後の後
        End If
        If bandnum < 0 Then
            Return False
        End If

        'レコードを後ろに追加
        Dim table As tbl側面DataTable = _Data.p_tbl側面
        Dim add As tbl側面Row = table.Newtbl側面Row
        add.f_i番号 = cIdxHeight
        add.f_iひも番号 = p_i側面ひもの本数 + 1
        add.f_i何本幅 = _I基本のひも幅
        table.Rows.Add(add)

        currow = Nothing
        If p_i側面ひもの本数 < bandnum Then
            currow = add
            Return True
        End If

        '追加位置以降を後ろにシフト(高さUPによりrecalcされるので、レコードに保持される分のみ
        Dim cond As String = String.Format("f_i番号 = {0}", cIdxHeight)
        Dim rows() As DataRow = table.Select(cond, "f_iひも番号 DESC")
        If rows Is Nothing OrElse rows.Count = 0 Then
            Return False
        End If

        Dim lastrow As tbl側面Row = Nothing
        For Each row As tbl側面Row In rows
            If lastrow Is Nothing Then
                lastrow = row
                Continue For
            End If
            If row.f_iひも番号 < bandnum Then
                Exit For '↓で処理済のはずだが
            End If

            lastrow.f_i何本幅 = row.f_i何本幅
            lastrow.f_s色 = row.f_s色
            lastrow.f_dひも長加算 = row.f_dひも長加算
            lastrow.f_sメモ = row.f_sメモ
            If row.f_iひも番号 = bandnum Then
                currow = row
                row.f_i何本幅 = _I基本のひも幅
                row.f_s色 = ""
                row.f_dひも長加算 = 0
                row.f_sメモ = ""
                Exit For
            End If
            lastrow = row
        Next

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "add_高さ currow={0}", New clsDataRow(currow).ToString)
        Return True
    End Function

    '削除ボタンによる側面の高さ削除
    'Trueを返すと、四角数のタブの高さを-1
    Function del_高さ(ByRef currow As tbl側面Row) As Boolean
        If _i側面の編みひも数 = 0 OrElse currow.f_i番号 <> cIdxHeight Then
            Return False
        End If
        '削除位置
        Dim bandnum As Integer = currow.f_iひも番号
        If bandnum < 1 OrElse _i側面の編みひも数 < bandnum Then
            Return False
        End If
        If Not _b縦横側面を展開する Then
            Return True '高さをDOWNするだけ
        End If
        currow = Nothing

        Dim table As tbl側面DataTable = _Data.p_tbl側面
        '現在位置以降を前にシフトする(高さDOWNによりrecalcされるので、レコードに保持される分のみ)
        Dim cond As String = String.Format("f_i番号 = {0}", cIdxHeight)
        Dim rows() As DataRow = table.Select(cond, "f_iひも番号 ASC")
        If rows Is Nothing OrElse rows.Count = 0 Then
            Return False
        End If

        Dim lastrow As tbl側面Row = Nothing
        For Each row As tbl側面Row In rows
            If row.f_iひも番号 < bandnum Then
                Continue For
            ElseIf row.f_iひも番号 = bandnum Then
                currow = row
                lastrow = row
                Continue For
            End If
            If lastrow IsNot Nothing Then
                lastrow.f_i何本幅 = row.f_i何本幅
                lastrow.f_s色 = row.f_s色
                lastrow.f_dひも長加算 = row.f_dひも長加算
                lastrow.f_sメモ = row.f_sメモ
            End If
            lastrow = row
        Next
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "del_高さ currow={0}", New clsDataRow(currow).ToString)
        Return True
    End Function


    '更新処理が必要なフィールド名
    Shared _fields側面と縁() As String = {"f_i何本幅", "f_i周数", "f_dひも長加算"}
    Shared Function IsDataPropertyName側面と縁(ByVal name As String) As Boolean
        Return _fields側面と縁.Contains(name)
    End Function

    'CalcSizeからの呼び出し 対応した更新の後、再集計(縁・側面)
    'SideEdgeカテゴリの場合は、対応レコード(縁・側面)の派生更新    
    '以外は、adjust_側面(), recalc_縁()  
    Private Function calc_側面と縁(ByVal category As CalcCategory, ByVal row As tbl側面Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category <> CalcCategory.SideEdge OrElse row Is Nothing Then
            'マスター変更もしくは派生更新または削除
            ret = ret And adjust_側面()
            ret = ret And recalc_縁()
        Else
            '追加もしくは更新
            If row IsNot Nothing Then
                '追加もしくは更新
                If row.f_i番号 = cHemNumber Then
                    '縁
                    Dim cond As String = String.Format("f_i番号 = {0}", row.f_i番号)
                    Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond), "f_iひも番号")
                    If dataPropertyName = "f_i周数" Then
                        Dim i周数 As Integer = row.f_i周数
                        groupRow.SetNameValue("f_i周数", i周数)
                    End If
                    ret = ret And set_groupRow編みかた(groupRow)
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "縁の変更: {0}", groupRow.ToString)

                ElseIf row.f_i番号 = cIdxHeight Then
                    '側面の編みひも
                    ret = ret And adjust_側面(row)

                End If '最下段はスキップ
            End If
        End If

        ret = ret And calc_縁計()
        ret = ret And calc_側面計()
        Return ret
    End Function

    '縁の集計値計算
    'IN:    
    'OUT:   _d縁の高さ  _d縁の垂直ひも長   _d縁の厚さ
    Private Function calc_縁計() As Boolean
        Dim cond As String = String.Format("f_i番号 = {0}", cHemNumber)

        '高さの合計
        Dim obj As Object = _Data.p_tbl側面.Compute("SUM(f_d高さ)", cond)
        If IsDBNull(obj) OrElse obj < 0 Then
            _d縁の高さ = 0
        Else
            _d縁の高さ = obj
        End If

        '垂直ひもの合計
        Dim obj2 As Object = _Data.p_tbl側面.Compute("SUM(f_d垂直ひも長)", cond)
        If IsDBNull(obj2) OrElse obj2 < 0 Then
            _d縁の垂直ひも長 = 0
        Else
            _d縁の垂直ひも長 = obj2
        End If

        '厚さの最大値
        Dim obj3 As Object = _Data.p_tbl側面.Compute("MAX(f_d厚さ)", cond)
        If IsDBNull(obj3) OrElse obj3 < 0 Then
            _d縁の厚さ = 0
        Else
            _d縁の厚さ = obj3
        End If

        Return True
    End Function

    '縁のレコードのマスタ参照   ※set_groupRow編みかた 呼び出し
    Private Function recalc_縁() As Boolean
        Dim cond As String = String.Format("f_i番号 = {0}", cHemNumber)
        Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")
        If groupRow.IsValid Then
            Return set_groupRow編みかた(groupRow)
        Else
            Return True '縁なし
        End If
    End Function

    '縁のレコードにマスタ参照値をセットする
    'IN:    p_i垂直ひも数,p_d縁厚さプラス_周
    'OUT:   f_d高さ,f_d垂直ひも長,f_dひも長,f_iひも本数,f_d厚さ
    Private Function set_groupRow編みかた(ByVal groupRow As clsGroupDataRow) As Boolean
        '周数は一致項目
        Dim i周数 As Integer = groupRow.GetNameValue("f_i周数")
        '周長は固定
        groupRow.SetNameValue("f_d周長", get側面の周長())

        'tbl編みかたRow
        Dim grpMst As clsGroupDataRow = g_clsMasterTables.GetPatternRecordGroup(groupRow.GetNameValue("f_s編みかた名"))
        If Not grpMst.IsValid Then
            'なし
            groupRow.SetNameValue("f_d高さ", DBNull.Value)
            groupRow.SetNameValue("f_d垂直ひも長", DBNull.Value)
            groupRow.SetNameValue("f_dひも長", DBNull.Value)
            groupRow.SetNameValue("f_iひも本数", DBNull.Value)
            groupRow.SetNameValue("f_d厚さ", DBNull.Value)
            groupRow.SetNameValue("f_d連続ひも長", DBNull.Value)
            groupRow.SetNameValue("f_bError", True)

            '{0}の番号{1}で設定にない編みかた名'{2}'(ひも番号{3})が参照されています。
            p_sメッセージ = String.Format(My.Resources.CalcNoMasterPattern, text側面と縁(), groupRow.GetNameValue("f_i番号"), groupRow.GetNameValue("f_s編みかた名"), groupRow.GetNameValue("f_iひも番号"))
            Return False

        Else
            Dim ret As Boolean = True
            Dim nひも1何本幅 As Integer = groupRow.GetIndexNameValue(1, "f_i何本幅")
            For Each drow As clsDataRow In groupRow
                Dim mst As clsPatternDataRow = Nothing
                If grpMst.IsExistIndexDataRow(drow) Then
                    mst = New clsPatternDataRow(grpMst.IndexDataRow(drow))
                End If
                If mst IsNot Nothing AndAlso mst.IsValid Then

                    drow.Value("f_d高さ") = i周数 * mst.GetHeight(drow.Value("f_i何本幅"))
                    drow.Value("f_d垂直ひも長") = i周数 * mst.GetBandLength(drow.Value("f_i何本幅"))
                    drow.Value("f_d厚さ") = mst.Value("f_d厚さ")
                    drow.Value("f_dひも長") = mst.GetBandLength(nひも1何本幅, drow.Value("f_d周長"), p_i垂直ひも数)
                    drow.Value("f_iひも本数") = i周数 * mst.Value("f_iひも数")
                    '
                    drow.Value("f_d連続ひも長") = drow.Value("f_dひも長") + drow.Value("f_dひも長加算")

                Else
                    drow.Value("f_d高さ") = DBNull.Value
                    drow.Value("f_d垂直ひも長") = DBNull.Value
                    drow.Value("f_d厚さ") = DBNull.Value
                    drow.Value("f_dひも長") = DBNull.Value
                    drow.Value("f_iひも本数") = DBNull.Value
                    drow.Value("f_d連続ひも長") = DBNull.Value
                    drow.Value("f_bError") = True
                    '{0}の番号{1}で設定にない編みかた名'{2}'(ひも番号{3})が参照されています。
                    p_sメッセージ = String.Format(My.Resources.CalcNoMasterPattern, text側面と縁(), drow.Value("f_i番号"), drow.Value("f_s編みかた名"), drow.Value("f_iひも番号"))
                    ret = False

                End If
            Next
            Return ret
        End If

    End Function


#End Region

#Region "底のひも展開"
    Dim _tbl縦横展開(cAngleCount - 1) As tbl縦横展開DataTable 'New時に作成、以降は存在が前提



    '展開処理により、縦ひも・横ひもそれぞれ、1本につき1レコードを生成する
    'ひも番号=0...上端下端/左端右端　の目がある時、f_d幅のみ持つ。enumひも種.i_すき間
    'ひも番号=1～本数-1   ... f_d幅は、ひも本幅数分の幅+_d目_ひも間のすき間
    'ひも番号=本数        ... f_d幅は、ひも本幅数分の幅+上端下端/左端右端の幅
    '始末ひも・補強ひもはenumひも種.i_補強

    'リスト出力値=f_d出力ひも長 係数・ひも長加算は2PassでFix

    Friend Function get展開DataTable(ByVal aidx As AngleIndex, Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        If isReset Then
            'calc_縦ひも展開のコード抜粋
            renew展開DataTable(aidx, False)
            recalc_ひも展開(aidx)
        Else
            calc_ひも展開(aidx, _ExpandCategory(idx(aidx)), Nothing, Nothing)
        End If
        Return _tbl縦横展開(idx(aidx))
    End Function

    '底の横ひもの編集完了,_tbl縦横展開_横ひもを_Dataに反映
    Friend Function save展開DataTable(ByVal aidx As AngleIndex, Optional ByVal always As Boolean = False) As Boolean
        Try
            If always OrElse _tbl縦横展開(idx(aidx)).GetChanges IsNot Nothing Then
                Dim change As Integer = _Data.FromTmpTable(idxひも種(aidx), _tbl縦横展開(idx(aidx)))
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "save展開DataTable")
            Return False
        End Try
    End Function


    Function prepare展開DataTable() As Boolean
        Try
            For Each aidx As AngleIndex In enumExeName.GetValues(GetType(AngleIndex))
                Dim table As tbl縦横展開DataTable = get展開DataTable(aidx)
                _Data.FromTmpTable(idxひも種(aidx), table)
                save展開DataTable(aidx, True)
            Next
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "prepare展開DataTable")
            Return False
        End Try
    End Function


    '更新処理が必要なフィールド名
    Shared _fields縦横展開() As String = {"f_i何本幅", "f_dひも長加算"}
    Shared Function IsDataPropertyName縦横展開(ByVal name As String) As Boolean
        Return _fields縦横展開.Contains(name)
    End Function


    Private Function condひも種(ByVal aidx As AngleIndex) As String
        Return String.Format("f_iひも種 = {0}", idxひも種(aidx))
    End Function

    Private Function condひも種とすき間(ByVal aidx As AngleIndex) As String
        Return String.Format("(f_iひも種 = {0}) OR (f_iひも種 = {1})", idxひも種(aidx), idxひも種すき間(aidx))
    End Function

    '本幅変更と幅の合計
    '※各レコードの位置番号・幅はセットされている
    Private Function recalc_ひも展開(ByVal aidx As AngleIndex) As Boolean
        Dim table As tbl縦横展開DataTable = _tbl縦横展開(idx(aidx))
        _d六角ベース計(idx(aidx)) = 0
        _d六角ベース中(idx(aidx)) = 0
        _bひも本幅変更(idx(aidx)) = False
        If table Is Nothing OrElse table.Rows.Count = 0 Then
            Return True
        End If

        Dim rows() As tbl縦横展開Row = table.Select(condひも種とすき間(aidx), "f_i位置番号 ASC")
        If rows Is Nothing OrElse rows.Count = 0 Then
            Return True
        End If

        '位置番号順
        Dim accum As Double = 0
        For Each row As tbl縦横展開Row In rows
            accum += row.f_d幅
            If is_idxひも種(aidx, row.f_iひも種) AndAlso row.f_i何本幅 <> _I基本のひも幅 Then
                _bひも本幅変更(idx(aidx)) = True
            End If
            '累積位置
            row.f_dVal1 = accum
            '半分の位置
            If row.f_iひも番号 = _i何個目位置(idx(aidx)) Then
                _d六角ベース中(idx(aidx)) = accum - (_d六つ目の高さ / 2)
            End If
        Next

        _d六角ベース計(idx(aidx)) = accum
        Return True
    End Function


    'CalcSizeからの呼び出し 対応した更新の後、再集計(_d四角ベース_縦計,_b横ひも本幅変更)
    'Expand_Yokoカテゴリの場合は、対応レコードの派生更新(画面で編集中のみ)   
    '以外は、set横展開DataTable()  により _tbl縦横展開_横ひも 更新
    'IN:    _b縦横側面を展開する
    'OUT:   _d四角ベース_縦計 _b横ひも本幅変更
    Function calc_ひも展開(ByVal aidx As AngleIndex, ByVal category As CalcCategory, ByVal row As tbl縦横展開Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category = _ExpandCategory(idx(aidx)) AndAlso row IsNot Nothing Then
            'テーブル編集中
            ret = adjust_展開ひも(aidx, row, p_iひもの本数(idx(aidx)))

        Else
            '_tbl縦横展開_横ひもを再セット
            ret = renew展開DataTable(aidx, _b縦横側面を展開する)

        End If

        'f_i何本幅の設定状態
        recalc_ひも展開(aidx)

        Return ret
    End Function

    '横ひもの指定レコードの幅と長さを計算
    'IN:    _d四角ベース_横計,_d四角ベース_高さ計,_d縁の垂直ひも長,_dひも長加算_縦横端,_dひも長係数
    'OUT:   各レコードのf_d長さ,f_dひも長,f_d出力ひも長
    Friend Function adjust_展開ひも(ByVal aidx As AngleIndex, ByVal row As tbl縦横展開Row, ByVal lastひも番号 As Integer) As Boolean
        If is_idxひも種(aidx, row.f_iひも種) Then
            If row.f_iひも番号 = lastひも番号 Then
                row.f_d幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _d端の目(idx(aidx)) * _d六つ目の高さ
            Else
                row.f_d幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _d六つ目の高さ
            End If
            row.f_d長さ = get周の横() + get側面高(2)
            row.f_dひも長 = (get周の横() + get側面高(2)) * _dひも長係数
            row.f_d出力ひも長 = row.f_dひも長 +
                2 * (_dひも長加算_上端 + _d縁の垂直ひも長) +
                row.f_dひも長加算

        ElseIf is_idxすき間(aidx, row.f_iひも種) Then
            row.Setf_i何本幅Null()
            row.Setf_dひも長加算Null()
            row.Setf_s色Null()

        ElseIf is_idx補強(aidx, row.f_iひも種) Then
            row.f_d出力ひも長 = row.f_dひも長 + row.f_dひも長加算

        End If

        Return True
    End Function

    '追加ボタン(縦横側面を展開時のみ)
    Friend Function add_ひも(ByVal aidx As AngleIndex, ByRef currow As tbl縦横展開Row) As Boolean
        currow = Insert縦横展開Row(_tbl縦横展開(idx(aidx)), currow.f_iひも種, currow.f_iひも番号)
        If currow Is Nothing Then
            Return False
        End If

        save展開DataTable(aidx, True)
        Return True
    End Function

    '削除ボタン(縦横側面を展開時のみ)
    Friend Function del_ひも(ByVal aidx As AngleIndex, ByRef currow As tbl縦横展開Row) As Boolean
        currow = Remove縦横展開Row(_tbl縦横展開(idx(aidx)), currow.f_iひも種, currow.f_iひも番号)
        If currow Is Nothing Then
            Return False
        End If

        save展開DataTable(aidx, True)
        Return True
    End Function

    '底の横ひもの再展開       ※adjust_横ひも()呼び出し
    'IN:    p_i横ひもの本数,_d上端下端の目,_d目_ひも間のすき間,_d四角ベース_横計
    'OUT:   _tbl縦横展開_横ひも
    Private Function renew展開DataTable(ByVal aidx As AngleIndex, ByVal isRefSaved As Boolean) As Boolean
        If Not isRefSaved Then
            _tbl縦横展開(idx(aidx)).Clear()
        End If
        _tbl縦横展開(idx(aidx)).AcceptChanges()

        '上から下へ
        Dim pos As Integer = -(p_iひもの本数(idx(aidx)) \ 2)
        Dim zero As Integer = p_iひもの本数(idx(aidx)) Mod 2
        Dim row As tbl縦横展開Row

        If 0 < _d端の目(idx(aidx)) * _d六つ目の高さ Then
            row = Find縦横展開Row(_tbl縦横展開(idx(aidx)), idxひも種すき間(aidx), 0, True)
            row.f_i位置番号 = pos - 1
            row.f_sひも名 = text端の目(aidx)
            row.Setf_i何本幅Null()
            row.f_d幅 = _d端の目(idx(aidx)) * _d六つ目の高さ
            row.Setf_dひも長Null()
            row.Setf_d長さNull()
            row.f_d出力ひも長 = 0
            row.Setf_s色Null()
        Else
            row = Find縦横展開Row(_tbl縦横展開(idx(aidx)), idxひも種すき間(aidx), 0, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
        End If

        SetMaxひも番号(_tbl縦横展開(idx(aidx)), p_iひもの本数(idx(aidx)))
        If 0 < p_iひもの本数(idx(aidx)) Then
            For i As Integer = 1 To p_iひもの本数(idx(aidx))
                row = Find縦横展開Row(_tbl縦横展開(idx(aidx)), idxひも種(aidx), i, True)

                row.f_i位置番号 = pos
                row.f_sひも名 = text配置ひも(aidx)
                row.f_i何本幅 = _I基本のひも幅
                adjust_展開ひも(aidx, row, p_iひもの本数(idx(aidx)))

                pos += 1
                If pos = 0 AndAlso zero = 0 Then
                    pos = 1
                End If
            Next
        End If

        '以降は裏側
        Dim posyoko As Integer = cBackPosition
        If _b補強ひも(idx(aidx)) Then
            For i As Integer = 1 To 2
                row = Find縦横展開Row(_tbl縦横展開(idx(aidx)), idxひも種補強(aidx), i, True)
                row.f_i位置番号 = posyoko
                row.f_sひも名 = text補強ひも(aidx)
                row.f_i何本幅 = _I基本のひも幅
                row.Setf_d幅Null()

                row.f_d長さ = p_d四角ベース_横
                row.f_dひも長 = p_d四角ベース_横
                row.f_d出力ひも長 = row.f_dひも長

                posyoko += 1
            Next
        Else
            row = Find縦横展開Row(_tbl縦横展開(idx(aidx)), idxひも種補強(aidx), 1, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
            row = Find縦横展開Row(_tbl縦横展開(idx(aidx)), idxひも種補強(aidx), 2, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
        End If
        _tbl縦横展開(idx(aidx)).AcceptChanges()

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(idxひも種(aidx), _tbl縦横展開(idx(aidx)))
            For Each row In _tbl縦横展開(idx(aidx))
                adjust_展開ひも(aidx, row, p_iひもの本数(idx(aidx)))
                '本幅・色なし
                If row.f_iひも種 And enumひも種.i_すき間 Then
                    row.Setf_s色Null()
                    row.Setf_i何本幅Null()
                End If
            Next
        End If

        _tbl縦横展開(idx(aidx)).AcceptChanges()
        Return True
    End Function



#End Region

#Region "差しひも"
    Private Function adjust_差しひも() As Boolean
        Dim table As tbl差しひもDataTable = _Data.p_tbl差しひも
        Dim ret As Boolean = True
        For Each row In table
            ret = ret And adjust_差しひも(row)
        Next
        Return ret
    End Function

    Private Function adjust_差しひも(ByRef row As tbl差しひもRow) As Boolean
        '    If row Is Nothing Then
        '        Return False '念のため
        '    End If
        '    row.f_b有効区分 = False
        '    row.Setf_iひも本数Null()
        '    row.Setf_dひも長Null()
        '    row.Setf_d出力ひも長Null()

        '    '開始位置は1以上、何本ごとはゼロ以上
        '    If row.f_i開始位置 < 1 Then
        '        row.f_s無効理由 = text開始位置()
        '        Return True '無効を返す
        '    End If
        '    If row.f_i何本ごと < 0 Then
        '        row.f_s無効理由 = text何本ごと()
        '        Return True '無効を返す
        '    End If


        '    '差しひもが有効か？
        '    If Not is差しひもavairable(row) Then
        '        Return True '無効を返す
        '    End If

        '    '本数を得る
        '    Dim count As Integer = get差しひもCount(row)
        '    If count <= 0 OrElse count < row.f_i開始位置 Then
        '        row.f_s無効理由 = text開始位置()
        '        Return True 'unnable
        '    End If
        '    If count < row.f_i開始位置 + row.f_i何本ごと Then
        '        row.f_s無効理由 = text何本ごと()
        '        Return True 'unnable
        '    End If
        '    '本数OK
        '    row.f_b有効区分 = True
        '    row.Setf_s無効理由Null()

        '    '本数計算
        '    If row.f_i何本ごと = 0 Then
        '        row.f_iひも本数 = 1
        '    Else
        '        Dim left As Integer = count - row.f_i開始位置
        '        row.f_iひも本数 = (left \ row.f_i何本ごと) + 1
        '    End If
        '    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} {1},{2},{3} 全{4}本のうち{5}本ごと{6}から{7}本",
        '             row.f_i番号, CType(row.f_i配置面, enum配置面), CType(row.f_i角度, enum角度), CType(row.f_i中心点, enum中心点), count, row.f_i何本ごと, row.f_i開始位置, row.f_iひも本数)

        '    If Not row.Isf_dひも長Null AndAlso 0 < row.f_dひも長 Then
        '        row.f_d出力ひも長 = row.f_dひも長 + 2 * row.f_dひも長加算
        '    End If
        Return True
    End Function

    '追加ボタン
    Function add_差しひも(ByRef row As tbl差しひもRow) As Boolean
        Dim table As tbl差しひもDataTable = _Data.p_tbl差しひも

        Dim addNumber As Integer = clsDataTables.AddNumber(table)
        If addNumber < 0 Then
            '{0}追加用の番号がとれません。
            p_sメッセージ = String.Format(My.Resources.CalcNoAddNumber, text差しひも())
            Return False
        End If

        'tbl差しひものレコード
        row = table.Newtbl差しひもRow
        row.f_i番号 = addNumber
        If 2 < _I基本のひも幅 Then
            row.f_i何本幅 = _I基本のひも幅 \ 2
        Else
            row.f_i何本幅 = 1
        End If
        row.f_dひも長加算 = _dひも長加算_上端

        table.Rows.Add(row)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "add_差しひも: {0}", addNumber)
        Return True
    End Function

    '更新処理が必要なフィールド名
    Shared _fields差しひも() As String = {"f_i配置面", "f_i角度", "f_i中心点", "f_i何本幅", "f_i開始位置", "f_i何本ごと", "f_dひも長加算"}
    Shared Function IsDataPropertyName差しひも(ByVal name As String) As Boolean
        Return _fields差しひも.Contains(name)
    End Function

    Private Function calc_差しひも(ByVal category As CalcCategory, ByVal row As tbl差しひもRow, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category <> CalcCategory.Options OrElse row Is Nothing Then
            'マスター変更もしくは派生更新または削除
            Return adjust_差しひも()

        Else
            If row IsNot Nothing Then
                '追加もしくは更新
                Return adjust_差しひも(row)
            End If
        End If
        Return ret
    End Function

#End Region

#Region "リスト出力"
    Dim _sasihimo As New Dictionary(Of Integer, tbl縦横展開DataTable)

    'リスト生成
    Public Function CalcOutput(ByVal output As clsOutput) As Boolean
        _sasihimo.Clear()

        If output Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '長さ計算をFix
        If Not CalcSize(CalcCategory.FixLength, Nothing, Nothing) Then
            Return False
        End If

        '出力レコード生成
        output.Clear()
        Dim row As tblOutputRow
        Dim order As String

        output.OutBasics(_Data.p_row目標寸法) '空行で終わる

        row = output.NextNewRow
        row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        row.f_s高さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str

        row = output.NextNewRow
        row.f_sカテゴリー = text四角数()
        row.f_s編みかた名 = text目_ひも間のすき間()
        row.f_s高さ = output.outLengthText(_d六つ目の高さ)

        '***四角数
        'このカテゴリーは先に行をつくる
        row = output.NextNewRow
        '横置き,縦置き
        For Each aidx As AngleIndex In [Enum].GetValues(GetType(AngleIndex))
            Dim tmpTable As tbl縦横展開DataTable = get展開DataTable(aidx)
            Dim sbMemo As New Text.StringBuilder

            If aidx = AngleIndex.idx_0 Then
                row.f_sタイプ = text横置き()
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s横ひものメモ"))
            ElseIf aidx = AngleIndex.idx_60 Then
                row.f_sタイプ = text斜め置き()
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s縦ひものメモ"))
            Else

            End If
            If tmpTable Is Nothing OrElse tmpTable.Rows.Count = 0 Then
                Continue For
            End If
            'レコードあり

            '長い順に記号を振る
            Dim tmps() As tbl縦横展開Row = tmpTable.Select(Nothing, "f_iひも種 ASC, f_d出力ひも長 DESC, f_s色")
            For Each tt As tbl縦横展開Row In tmps
                If 0 < tt.f_iひも番号 AndAlso 0 < tt.f_d出力ひも長 Then
                    tt.f_s記号 = output.SetBandRow(0, tt.f_i何本幅, tt.f_d出力ひも長, tt.f_s色)
                End If
            Next
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "DEBUG:{0}", New clsGroupDataRow(tmpTable).ToString())

            'リスト出力
            tmps = tmpTable.Select(Nothing, "f_iひも種 ASC, f_iひも番号 ASC")
            '0
            Dim lasttmp As tbl縦横展開Row = tmps(0)
            Dim contcount As Integer = 1
            If Not String.IsNullOrWhiteSpace(lasttmp.f_sメモ) Then
                If 0 < sbMemo.Length Then
                    sbMemo.Append(" "c)
                End If
                sbMemo.Append(lasttmp.f_sメモ)
            End If
            '1～tmps.Length - 1, tmps.Lengthは出力のみ
            For i As Integer = 1 To tmps.Length
                Dim tmp As tbl縦横展開Row = Nothing
                If i < tmps.Length Then
                    tmp = tmps(i)
                End If
                If tmp IsNot Nothing AndAlso
                lasttmp.f_iひも種 = tmp.f_iひも種 AndAlso lasttmp.f_s記号 = tmp.f_s記号 Then
                    '同じひも種・記号の継続
                    contcount += 1
                    If Not String.IsNullOrWhiteSpace(tmp.f_sメモ) Then
                        If 0 < sbMemo.Length Then
                            sbMemo.Append(" "c)
                        End If
                        sbMemo.Append(tmp.f_sメモ)
                    End If

                Else
                    '異なるので、まず先のレコードをまとめ出力
                    row.f_s編みかた名 = lasttmp.f_sひも名
                    If lasttmp.f_iひも番号 = 0 Then
                        row.f_s高さ = output.outLengthText(lasttmp.f_d幅)
                    Else
                        output.SetBandRow(contcount, lasttmp.f_i何本幅, lasttmp.f_d出力ひも長, lasttmp.f_s色)
                        row.f_s長さ = output.outLengthText(lasttmp.f_d長さ)
                    End If

                    row.f_sメモ = sbMemo.ToString
                    If _Data.p_row底_縦横.Value("f_b展開区分") Then
                        If contcount = 1 Then
                            If 0 < lasttmp.f_iひも番号 Then
                                row.f_s編みひも名 = String.Format("{0}", lasttmp.f_iひも番号)
                            End If
                        Else
                            row.f_s編みひも名 = String.Format("{0} - {1}", lasttmp.f_iひも番号, lasttmp.f_iひも番号 + contcount - 1)
                        End If
                        row.f_s編みひも名 = String.Format("[{0}] {1}", p_iひもの本数(idx(aidx)), row.f_s編みひも名)
                    End If
                    row = output.NextNewRow

                    '現レコードから開始
                    If tmp IsNot Nothing Then
                        lasttmp = tmp
                        contcount = 1
                        sbMemo.Clear()
                        sbMemo.Append(tmp.f_sメモ)
                    End If
                End If
            Next

        Next
        output.SetBlankLine() '先に行をつくっているので


        '***側面と縁
        If 0 < _Data.p_tbl側面.Rows.Count Then
            row = output.NextNewRow
            row.f_sカテゴリー = text側面と縁()
            row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_s高さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str

            order = "f_i番号 ASC , f_iひも番号 ASC"
            If is側面下から上へ() Then
                '記号をとる
                For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, order)
                    If r.f_i番号 = cIdxSpace Then
                        Continue For
                    End If
                    If 0 < r.f_iひも本数 Then
                        If 0 < r.f_d連続ひも長 Then
                            r.f_s記号 = output.SetBandRow(0, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                        End If
                    End If
                Next
                order = "f_i番号 DESC , f_iひも番号 DESC"
            End If
            'リスト出力
            Dim r_prv As tbl側面Row = Nothing
            Dim contcount As Integer = 0
            For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, order)
                If r_prv IsNot Nothing AndAlso r_prv.f_s記号 = r.f_s記号 _
                    AndAlso r_prv.f_s編みひも名 = r.f_s編みひも名 AndAlso r_prv.f_sメモ = r.f_sメモ Then
                    contcount += r.f_iひも本数
                Else
                    If r_prv IsNot Nothing Then
                        row = output.NextNewRow
                        If r_prv.f_i番号 = cHemNumber Then
                            row.f_sタイプ = text縁の始末()
                            row.f_s編みかた名 = r_prv.f_s編みかた名
                            row.f_s編みひも名 = r_prv.f_s編みひも名
                        ElseIf r_prv.f_i番号 = cIdxHeight Then
                            row.f_s番号 = r_prv.f_iひも番号.ToString
                            row.f_s編みかた名 = r_prv.f_s編みひも名
                        Else '最下段スペース
                            row.f_s編みかた名 = r_prv.f_s編みかた名
                            row.f_s編みひも名 = r_prv.f_s編みひも名
                        End If
                        row.f_i周数 = contcount
                        row.f_s高さ = output.outLengthText(r_prv.f_d高さ)
                        row.f_s長さ = output.outLengthText(r_prv.f_dひも長)
                        If 0 < r_prv.f_d連続ひも長 AndAlso 0 < contcount Then
                            output.SetBandRow(contcount, r_prv.f_i何本幅, r_prv.f_d連続ひも長, r_prv.f_s色)
                        End If
                        row.f_sメモ = r_prv.f_sメモ
                    End If
                    contcount = r.f_iひも本数
                End If
                r_prv = r
            Next
            If r_prv IsNot Nothing Then
                row = output.NextNewRow
                If r_prv.f_i番号 = cHemNumber Then
                    row.f_sタイプ = text縁の始末()
                    row.f_s編みかた名 = r_prv.f_s編みかた名
                    row.f_s編みひも名 = r_prv.f_s編みひも名
                ElseIf r_prv.f_i番号 = cIdxHeight Then
                    row.f_s番号 = r_prv.f_iひも番号.ToString
                    row.f_s編みかた名 = r_prv.f_s編みひも名
                Else '最下段スペース
                    row.f_s編みかた名 = r_prv.f_s編みかた名
                    row.f_s編みひも名 = r_prv.f_s編みひも名
                End If
                row.f_i周数 = contcount
                row.f_s高さ = output.outLengthText(r_prv.f_d高さ)
                row.f_s長さ = output.outLengthText(r_prv.f_dひも長)
                If 0 < r_prv.f_d連続ひも長 AndAlso 0 < contcount Then
                    output.SetBandRow(contcount, r_prv.f_i何本幅, r_prv.f_d連続ひも長, r_prv.f_s色)
                End If
                row.f_sメモ = r_prv.f_sメモ
            End If

            output.AddBlankLine()
        End If

        '***差しひも
        If 0 < _Data.p_tbl差しひも.Rows.Count Then
            row = output.NextNewRow
            row.f_sカテゴリー = text差しひも()
            row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str

            order = "f_i番号"
            For Each r As tbl差しひもRow In _Data.p_tbl差しひも.Select(Nothing, order)
                row = output.NextNewRow

                If r.f_b有効区分 Then
                    row.f_s番号 = r.f_i番号.ToString
                Else
                    row.f_s番号 = String.Format("({0})", r.f_i番号)
                    row.f_s色 = r.f_s色
                End If
                row.f_sタイプ = PlateString(r.f_i配置面)
                row.f_s編みかた名 = AngleString(r.f_i角度)
                row.f_s編みひも名 = CenterString(r.f_i中心点)
                row.f_i周数 = r.f_i開始位置
                row.f_i段数 = r.f_i何本ごと
                row.f_sメモ = r.f_sメモ
                If Not r.f_b有効区分 Then
                    Continue For
                End If

                If 0 < r.f_dひも長 AndAlso 0 < r.f_iひも本数 Then
                    '固定長
                    row.f_s長さ = output.outLengthText(r.f_dひも長)
                    r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d出力ひも長, r.f_s色)

                ElseIf 0 < r.f_iひも本数 Then
                    '差しひもの各長をセットしたテーブル
                    'Dim tmptable As tbl縦横展開DataTable = get差しひもLength(r)
                    'If tmptable IsNot Nothing AndAlso 0 < tmptable.Rows.Count Then
                    '    For Each tmp As tbl縦横展開Row In tmptable
                    '        row = output.NextNewRow
                    '        row.f_s長さ = output.outLengthText(tmp.f_dひも長)
                    '        row.f_s編みひも名 = tmp.f_iひも番号
                    '        'ひも数=f_iVal1
                    '        tmp.f_s記号 = output.SetBandRow(tmp.f_iVal1, tmp.f_i何本幅, tmp.f_d出力ひも長, tmp.f_s色)
                    '    Next
                    '    _sasihimo.Add(r.f_i番号, tmptable)
                    'End If

                End If
            Next

            output.AddBlankLine()
        End If

        '***追加品 
        If 0 < _Data.p_tbl追加品.Rows.Count Then
            row = output.NextNewRow
            row.f_sカテゴリー = text追加品()
            row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str

            order = "f_i番号 , f_iひも番号"
            For Each r As tbl追加品Row In _Data.p_tbl追加品.Select(Nothing, order)
                row = output.NextNewRow
                row.f_s番号 = r.f_i番号.ToString
                row.f_s編みかた名 = r.f_s付属品名
                row.f_s編みひも名 = r.f_s付属品ひも名
                row.f_i周数 = r.f_i点数
                row.f_s長さ = output.outLengthText(r.f_d長さ)
                If 0 < r.f_iひも本数 Then
                    r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_dひも長 + r.f_dひも長加算, r.f_s色)
                End If
                row.f_sメモ = r.f_sメモ
            Next

            output.AddBlankLine()
        End If

        '***計算寸法
        row = output.NextNewRow
        row.f_sカテゴリー = text計算寸法()
        row.f_sひも本数 = text四角ベース()
        row.f_sひも長 = text縁厚さプラス()
        row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str

        row = output.NextNewRow
        row.f_s色 = text横寸法()
        row.f_sひも本数 = output.outLengthText(p_d四角ベース_横)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_横)
        row.f_sメモ = text厚さ()
        row.f_s長さ = output.outLengthText(p_d厚さ)

        row = output.NextNewRow
        row.f_s色 = text縦寸法()
        row.f_sひも本数 = output.outLengthText(p_d四角ベース_縦)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_縦)
        row.f_sメモ = text垂直ひも数()
        row.f_s長さ = p_i垂直ひも数

        row = output.NextNewRow
        row.f_s色 = text高さ寸法()
        row.f_sひも本数 = output.outLengthText(p_d四角ベース_高さ)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_高さ)
        row.f_sメモ = text垂直ひも長()
        row.f_s長さ = output.outLengthText(_d六角ベース_高さ計 + _d縁の垂直ひも長 + _dひも長加算_上端)

        row = output.NextNewRow
        row.f_s色 = text周()
        row.f_sひも本数 = output.outLengthText(p_d四角ベース_周)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_周)
        row.f_sメモ = My.Resources.CalcOutTurn '折り返し
        row.f_s長さ = output.outLengthText(_d縁の垂直ひも長 + _dひも長加算_上端 - _d縁の高さ)

        output.AddBlankLine()

        '集計値
        output.OutSummery() '間に空行

        'メモがあれば追記
        Dim memo As String = _Data.p_row目標寸法.Value("f_sメモ")
        If Not String.IsNullOrEmpty(memo) Then
            output.AddBlankLine()
            row = output.NextNewRow
            row.f_sカテゴリー = memo
        End If
        Return True
    End Function
#End Region

#Region "画面から文字列取得"

    Private Function text側面の編みひも() As String
        Return _frmMain.lbl側面の編みひも.Text
    End Function

    Private Function is側面下から上へ() As Boolean
        Return _frmMain.rad下から上へ.Checked
    End Function

    Private Function text四角数() As String
        Return _frmMain.tpage四角数.Text
    End Function

    Private Function text側面と縁() As String
        Return _frmMain.tpage側面と縁.Text
    End Function

    Private Function text追加品() As String
        Return _frmMain.tpage追加品.Text
    End Function

    Private Function text計算寸法() As String
        Return _frmMain.lbl計算寸法.Text
    End Function

    Private Function text横置き() As String
        Return _frmMain.grp横置き.Text
    End Function

    Private Function text斜め置き() As String
        Return _frmMain.grp斜め置き.Text
    End Function

    Private Function text差しひも() As String
        Return _frmMain.tpage差しひも.Text
    End Function

    Private Function textひも上下() As String
        Return _frmMain.tpageひも上下.Text
    End Function

    Private Function text配置ひも(ByVal aidx As AngleIndex) As String
        If aidx = AngleIndex.idx_0 Then
            Return _frmMain.tpage横ひも.Text
        ElseIf aidx = AngleIndex.idx_60 Then
            Return _frmMain.tpage斜め60度.Text
        ElseIf aidx = AngleIndex.idx_120 Then
            Return _frmMain.tpage斜め120度.Text
        Else
            Return Nothing
        End If
    End Function


    Private Function text端の目(ByVal idx As AngleIndex) As String
        If idx = AngleIndex.idx_0 Then
            Return _frmMain.lbl上端下端.Text
        ElseIf idx = AngleIndex.idx_60 Then
            Return _frmMain.lbl斜め左端右端.Text & _frmMain.lbl60度.Text
        ElseIf idx = AngleIndex.idx_120 Then
            Return _frmMain.lbl斜め左端右端.Text & _frmMain.lbl120度.Text
        Else
            Return Nothing
        End If
    End Function

    Private Function text補強ひも(ByVal idx As AngleIndex) As String
        If idx = AngleIndex.idx_0 Then
            Return _frmMain.chk横の補強ひも.Text
        ElseIf idx = AngleIndex.idx_60 Then
            Return _frmMain.chk斜めの補強ひも.Text & _frmMain.lblchk60度.Text
        ElseIf idx = AngleIndex.idx_120 Then
            Return _frmMain.chk斜めの補強ひも_120度.Text
        Else
            Return Nothing
        End If
    End Function

    Private Function text高さの目の数() As String
        Return _frmMain.lbl高さの六つ目数.Text
    End Function

    Private Function text最下段() As String
        Return _frmMain.lbl最下段.Text
    End Function

    Private Function text編みかた名() As String
        Return _frmMain.lbl編みかた名_側面.Text
    End Function

    Private Function text縁の始末() As String
        Return _frmMain.lbl縁の始末.Text
    End Function

    Private Function text内側() As String
        Return _frmMain.rad目標寸法_内側.Text
    End Function

    Private Function text外側() As String
        Return _frmMain.rad目標寸法_外側.Text
    End Function

    Private Function text横寸法() As String
        Return _frmMain.lbl横寸法.Text
    End Function
    Private Function text縦寸法() As String
        Return _frmMain.lbl縦寸法.Text
    End Function

    Private Function text高さ寸法() As String
        Return _frmMain.lbl高さ寸法.Text
    End Function

    Private Function text本幅() As String
        Return _frmMain.lbl基本のひも幅_単位.Text
    End Function

    Private Function text本() As String
        Return _frmMain.lbl横ひもの本数_単位.Text
    End Function

    Private Function text垂直ひも数() As String
        Return _frmMain.lbl垂直ひも数.Text
    End Function

    Private Function text目_ひも間のすき間() As String
        Return _frmMain.lbl六つ目の高さ.Text
    End Function

    Private Function text厚さ() As String
        Return _frmMain.lbl厚さ.Text
    End Function

    Private Function text垂直ひも長() As String
        'dgv側面
        Return _frmMain.f_d垂直ひも長2.HeaderText
    End Function

    Private Function text四角ベース() As String
        Return _frmMain.lbl六つ目ベース.Text
    End Function

    Private Function text縁厚さプラス() As String
        Return _frmMain.lbl縁厚さプラス.Text
    End Function

    Private Function text周() As String
        Return _frmMain.lbl計算寸法の周.Text
    End Function

    '差しひも
    Private Function text配置面() As String
        Return _frmMain.editInsertBand.text配置面()
    End Function

    Private Function text角度() As String
        Return _frmMain.editInsertBand.text角度()
    End Function

    Private Function text開始位置() As String
        Return _frmMain.editInsertBand.text開始位置()
    End Function

    Private Function text何本ごと() As String
        Return _frmMain.editInsertBand.text何本ごと()
    End Function

    Private Function text何本幅() As String
        Return _frmMain.editInsertBand.text何本幅()
    End Function

    Private Function PlateString(ByVal i As Integer) As String
        Return _frmMain.editInsertBand.PlateString(i)
    End Function

    Private Function AngleString(ByVal i As Integer) As String
        Return _frmMain.editInsertBand.AngleString(i)
    End Function

    Private Function CenterString(ByVal i As Integer) As String
        Return _frmMain.editInsertBand.CenterString(i)
    End Function
#End Region

End Class
