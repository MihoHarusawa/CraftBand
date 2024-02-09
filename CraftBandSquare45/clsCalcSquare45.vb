﻿

Imports System.Reflection
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.ctrEditUpDown
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstOutput

Class clsCalcSquare45
    Const PAI As Double = 3.1416
    Const ROOT2 As Double = 1.41421356237

    '処理のカテゴリー
    Public Enum CalcCategory
        None

        NewData 'データ変更
        BsMaster  '基本値/マスター/バンド選択
        Target    '目標寸法(基本のひも幅以外)
        BandWidth   '基本のひも幅
        Square '四角数・ひも間のすき間
        Edge  '縁の始末 
        Options  '追加品
        Expand '縦横展開
    End Enum

    Public Property p_b有効 As Boolean
    Public Property p_sメッセージ As String 'p_b有効でない場合のみ参照

    '目標
    Private Property _b内側区分 As Boolean
    Private Property _d横_目標 As Double
    Private Property _d縦_目標 As Double
    Private Property _d高さ_目標 As Double
    Private Property _I基本のひも幅 As Integer
    '基本単位
    Private Property _dひも幅の一辺 As Double
    Private Property _dひも幅の対角線 As Double
    Private Property _d四角の一辺 As Double
    Private Property _d四角の対角線 As Double
    '四角
    Private Property _i横の四角数 As Integer
    Private Property _i縦の四角数 As Integer
    Private Property _d高さの四角数 As Double
    Private Property _dひも間のすき間 As Double 'nudゼロ以上
    Private Property _dひも長係数 As Double 'nudゼロ以上
    Private Property _dひも長加算 As Double 'プラスマイナス可

    '縁の始末
    Private Property _d縁の高さ As Double '合計値,ゼロ以上
    Private Property _d縁の垂直ひも長 As Double '合計値,ゼロ以上
    Private Property _d縁の厚さ As Double '厚さの最大値,ゼロ以上

    '初期化
    Public Sub Clear()
        p_b有効 = False
        p_sメッセージ = Nothing

        _b内側区分 = False
        _d横_目標 = -1
        _d縦_目標 = -1
        _d高さ_目標 = -1
        _I基本のひも幅 = -1

        _dひも幅の一辺 = -1
        _dひも幅の対角線 = -1
        _d四角の一辺 = -1
        _d四角の対角線 = -1

        _i横の四角数 = -1
        _i縦の四角数 = -1
        _d高さの四角数 = -1
        _dひも間のすき間 = 0
        _dひも長係数 = 0
        _dひも長加算 = 0

        _d縁の高さ = 0
        _d縁の垂直ひも長 = 0
        _d縁の厚さ = 0

        _tbl縦横展開_横ひも.Clear()
        _tbl縦横展開_縦ひも.Clear()
    End Sub

#Region "プロパティ値"

    ReadOnly Property p_sひも幅の辺 As String
        Get
            If 0 < _dひも幅の一辺 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_dひも幅の一辺)
            End If
            Return ""
        End Get
    End Property

    ReadOnly Property p_sひも幅の対角線 As String
        Get
            If 0 < _dひも幅の一辺 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_dひも幅の対角線)
            End If
            Return ""
        End Get
    End Property

    ReadOnly Property p_s四角の辺 As String
        Get
            If 0 < _d四角の一辺 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d四角の一辺)
            End If
            Return ""
        End Get
    End Property

    ReadOnly Property p_s四角の対角線 As String
        Get
            If 0 < _d四角の対角線 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d四角の対角線)
            End If
            Return ""
        End Get
    End Property

    ReadOnly Property p_i横の四角数 As Integer
        Get
            Return _i横の四角数
        End Get
    End Property

    ReadOnly Property p_i縦の四角数 As Integer
        Get
            Return _i縦の四角数
        End Get
    End Property

    ReadOnly Property p_b横の四角数が縦以上 As Boolean   '縦横同じならtrue
        Get
            Return _i縦の四角数 <= _i横の四角数
        End Get
    End Property

    ReadOnly Property p_i縦横四角数の差 As Integer  '縦横同じならゼロ
        Get
            If _i縦の四角数 < _i横の四角数 Then
                Return _i横の四角数 - _i縦の四角数
            Else
                Return _i縦の四角数 - _i横の四角数
            End If
        End Get
    End Property

    ReadOnly Property p_i縦横四角数の小さい方 As Integer
        Get
            If _i縦の四角数 < _i横の四角数 Then
                Return _i縦の四角数
            Else
                Return _i横の四角数
            End If
        End Get
    End Property


    ReadOnly Property p_i横ひもの本数 As Integer
        Get
            Return _i横の四角数 + _i縦の四角数
        End Get
    End Property

    ReadOnly Property p_i縦ひもの本数 As Integer
        Get
            Return _i横の四角数 + _i縦の四角数
        End Get
    End Property

    ReadOnly Property p_d四角ベース_横 As Double
        Get
            Return _d四角の対角線 * _i横の四角数
        End Get
    End Property

    ReadOnly Property p_d四角ベース_縦 As Double
        Get
            Return _d四角の対角線 * _i縦の四角数
        End Get
    End Property

    ReadOnly Property p_d四角ベース_高さ As Double
        Get
            Return _d四角の対角線 * _d高さの四角数
        End Get
    End Property

    ReadOnly Property p_d四角ベース_周 As Double
        Get
            Return 2 * (p_d四角ベース_横 + p_d四角ベース_縦)
        End Get
    End Property

    ReadOnly Property p_d厚さ As Double
        Get
            Dim d As Double = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
            If d < _d縁の厚さ Then
                Return _d縁の厚さ
            Else
                Return d
            End If
        End Get
    End Property

    ReadOnly Property p_d縁厚さプラス_横 As Double
        Get
            If p_d四角ベース_横 <= 0 Then
                Return 0
            End If
            Return p_d四角ベース_横 + p_d厚さ '外側には1/2,その2倍
        End Get
    End Property

    ReadOnly Property p_d縁厚さプラス_縦 As Double
        Get
            If p_d四角ベース_縦 <= 0 Then
                Return 0
            End If
            Return p_d四角ベース_縦 + p_d厚さ * 2 '外側には1/2,その2倍
        End Get
    End Property

    ReadOnly Property p_d縁厚さプラス_高さ As Double
        Get
            If p_d四角ベース_高さ < 0 Then
                Return 0
            End If
            Return p_d四角ベース_高さ +
                _d縁の高さ + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
        End Get
    End Property

    ReadOnly Property p_d縁厚さプラス_周 As Double
        Get
            If p_d縁厚さプラス_横 <= 0 OrElse p_d縁厚さプラス_縦 <= 0 Then
                Return 0
            End If
            Return 2 * (p_d縁厚さプラス_横 + p_d縁厚さプラス_縦)
        End Get
    End Property

    '表示文字列
    Public ReadOnly Property p_s四角ベース_横 As String
        Get
            If 0 < p_d四角ベース_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d四角ベース_横)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s四角ベース_縦 As String
        Get
            If 0 < p_d四角ベース_縦 Then
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

    Public ReadOnly Property p_s縁厚さプラス_横 As String
        Get
            If 0 < p_d縁厚さプラス_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d縁厚さプラス_横)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s縁厚さプラス_縦 As String
        Get
            If 0 < p_d縁厚さプラス_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d縁厚さプラス_縦)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s縁厚さプラス_高さ As String
        Get
            If 0 <= p_d縁厚さプラス_高さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d縁厚さプラス_高さ)
            End If
            Return ""
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


    '計算寸法と目標寸法の差(四角ベースの差)
    Public ReadOnly Property p_s横寸法の差 As String
        Get
            If 0 < p_d四角ベース_横 AndAlso 0 < _d横_目標 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_d四角ベース_横 - _d横_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s縦寸法の差 As String
        Get
            If 0 < p_d四角ベース_縦 AndAlso 0 < _d縦_目標 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_d四角ベース_縦 - _d縦_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s高さ寸法の差 As String
        Get
            If 0 < p_d四角ベース_高さ AndAlso 0 < _d高さ_目標 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_d四角ベース_高さ - _d高さ_目標, True)
            End If
            Return ""
        End Get
    End Property


    Public ReadOnly Property p_i垂直ひも数 As Integer
        Get
            Return 2 * (p_i横ひもの本数 + p_i縦ひもの本数)
        End Get
    End Property

    Public ReadOnly Property p_s厚さ As String
        Get
            If 0 < p_d厚さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d厚さ)
            End If
            Return ""
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


        If _BandPositions横ひも IsNot Nothing Then
            For Each band As CBandPosition In _BandPositions横ひも
                sb.AppendLine(band.ToString)
            Next
        End If
        If _BandPositions縦ひも IsNot Nothing Then
            For Each band As CBandPosition In _BandPositions縦ひも
                sb.AppendLine(band.ToString)
            Next
        End If

        Return sb.ToString
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("{0}({1}){2}", Me.GetType.Name, IIf(p_b有効, "Valid", "InValid"), p_sメッセージ).AppendLine()
        sb.AppendFormat("Target:({0},{1},{2}) Lane({3})", _d横_目標, _d縦_目標, _d高さ_目標, _I基本のひも幅).AppendLine()
        sb.AppendFormat("Unit: band({0}/{1}) square({2}/{3})", _dひも幅の一辺, _dひも幅の対角線, _d四角の一辺, _d四角の対角線).AppendLine()
        sb.AppendFormat("Square: W({0},{1}) D({2},{3}) H({4},{5}) ", _i横の四角数, p_d四角ベース_横, _i縦の四角数, p_d四角ベース_縦, _d高さの四角数, p_d四角ベース_高さ).AppendLine()
        sb.AppendFormat("Edge:H({0}) VerticalLength({1}) Thickness({2})", _d縁の高さ, _d縁の垂直ひも長, _d縁の厚さ).AppendLine()
        Return sb.ToString
    End Function

#End Region



    Dim _Data As clsDataTables
    Dim _frmMain As frmMain

    Sub New(ByVal data As clsDataTables, ByVal frm As frmMain)
        _Data = data
        _frmMain = frm

        '最初に1点だけ作っておく。以降は存在が前提
        _tbl縦横展開_横ひも = New tbl縦横展開DataTable
        _tbl縦横展開_縦ひも = New tbl縦横展開DataTable

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
                ret = ret And set_四角数()
                ret = ret And calc_側面(category, Nothing, Nothing)
                If ret Then
                    p_sメッセージ = _frmMain.editAddParts.CheckError(_Data)
                    If Not String.IsNullOrEmpty(p_sメッセージ) Then
                        ret = False
                    End If
                End If

            Case CalcCategory.Target    '目標寸法
                ret = set_目標寸法(False)

            Case CalcCategory.BandWidth    '基本のひも幅
                ret = set_目標寸法(False)
                ret = ret And set_四角数()

            Case CalcCategory.Square '四角数・ひも間のすき間
                ret = ret And set_四角数()
                'ret = ret And calc_側面(category, Nothing, Nothing)

            Case CalcCategory.Edge  '縁の始末
                Dim row As tbl側面Row = CType(ctr, tbl側面Row)
                ret = ret And calc_側面(category, row, key)

            Case CalcCategory.Options  '追加品
                'editAddParts内の処理, エラー文字列表示のみ
                p_sメッセージ = _frmMain.editAddParts.CheckError(_Data)
                If Not String.IsNullOrEmpty(p_sメッセージ) Then
                    ret = False
                End If
                '(追加品は計算寸法変更なし)

            Case Else
                '未定義のカテゴリー'{0}'が参照されました。
                p_sメッセージ = String.Format(My.Resources.CalcNoDefCategory, category)
                ret = False

        End Select

        p_b有効 = ret
        Return ret
    End Function


#Region "四角"

    '目標寸法の取得
    Private Function set_目標寸法(ByVal needtarget As Boolean) As Boolean

        With _Data.p_row目標寸法
            _b内側区分 = .Value("f_b内側区分")

            _d横_目標 = .Value("f_d横寸法")
            _d縦_目標 = .Value("f_d縦寸法")
            _d高さ_目標 = .Value("f_d高さ寸法")

            _I基本のひも幅 = .Value("f_i基本のひも幅")
            _dひも幅の一辺 = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)
            _dひも幅の対角線 = _dひも幅の一辺 * ROOT2
        End With

        Return isValidTarget(needtarget)
    End Function

    '四角数の取得
    'IN: _I基本のひも幅(チェック時)
    Function set_四角数() As Boolean

        With _Data.p_row底_縦横
            _i横の四角数 = .Value("f_i横の四角数") '
            _i縦の四角数 = .Value("f_i縦の四角数") '
            _d高さの四角数 = .Value("f_d高さの四角数") '
            _dひも間のすき間 = .Value("f_dひも間のすき間") '
            _dひも長係数 = .Value("f_dひも長係数") '
            _dひも長加算 = .Value("f_dひも長加算") '
        End With

        _d四角の一辺 = _dひも幅の一辺 + _dひも間のすき間
        _d四角の対角線 = _d四角の一辺 * ROOT2

        Return IsValidInput()
    End Function

#End Region

#Region "概算"

    '有効な目標寸法がある
    Public Function isValidTarget(ByVal needtarget As Boolean) As Boolean
        If needtarget Then
            If _d横_目標 <= 0 AndAlso _d縦_目標 <= 0 AndAlso _d高さ_目標 <= 0 Then
                '目標とする縦寸法・横寸法・高さ寸法を設定してください。
                p_sメッセージ = My.Resources.CalcNoTargetSet
                Return False
            End If
        End If
        '必須
        If _I基本のひも幅 <= 0 Then
            '基本のひも幅を設定してください。
            p_sメッセージ = My.Resources.CalcNoBaseBandSet
            Return False
        End If
        Return True
    End Function

    '有効な入力がある
    Public Function IsValidInput() As Boolean
        If _i横の四角数 < 0 OrElse _i縦の四角数 < 0 OrElse _d高さの四角数 < 0 Then
            '横の四角数・縦の四角数・高さの四角数をセットしてください。
            p_sメッセージ = My.Resources.CalcNoSquareCountSet
            Return False
        End If
        If _I基本のひも幅 <= 0 Then
            '基本のひも幅を設定してください。
            p_sメッセージ = My.Resources.CalcNoBaseBandSet
            Return False
        End If

        Dim nonzero_count As Integer = 0
        If 0 < _i横の四角数 Then
            nonzero_count += 1
        End If
        If 0 < _i縦の四角数 Then
            nonzero_count += 1
        End If
        If 0 < _d高さの四角数 Then
            nonzero_count += 1
        End If
        If nonzero_count < 2 Then
            '横の四角数・縦の四角数・高さの四角数をセットしてください。
            p_sメッセージ = My.Resources.CalcNoSquareCountSet
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

        '目標に基づき横・縦・高さの四角数を再計算します。よろしいですか？
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
        If _d四角の対角線 <= 0 Then
            Return False
        End If

        Dim ret As Boolean = True
        ret = ret And calc_Target_横()
        ret = ret And calc_Target_縦()
        ret = ret And calc_Target_高さ()
        Return ret
    End Function

    '横寸法から横の四角数
    Private Function calc_Target_横() As Boolean
        Dim i横の四角数 As Integer = Int(_d横_目標 / _d四角の対角線)
        If Not _b内側区分 Then
            Do While i横の四角数 * _d四角の対角線 < _d横_目標
                i横の四角数 += 1
            Loop
        End If
        If i横の四角数 = 0 Then
            i横の四角数 = 1
        End If

        _Data.p_row底_縦横.Value("f_i横の四角数") = i横の四角数

        Return True
    End Function

    '縦寸法から縦の四角数
    Private Function calc_Target_縦()
        Dim i縦の四角数 As Integer = Int(_d縦_目標 / _d四角の対角線)
        If Not _b内側区分 Then
            Do While i縦の四角数 * _d四角の対角線 < _d縦_目標
                i縦の四角数 += 1
            Loop
        End If
        If i縦の四角数 = 0 Then
            i縦の四角数 = 1
        End If

        _Data.p_row底_縦横.Value("f_i縦の四角数") = i縦の四角数

        Return True
    End Function

    '高さ寸法から高さの四角数(0.5単位)
    Private Function calc_Target_高さ()
        Dim i高さの四角数の2倍 As Integer = Int((_d高さ_目標 * 2) / _d四角の対角線)
        If Not _b内側区分 Then
            Do While i高さの四角数の2倍 * _d四角の対角線 < (_d高さ_目標 * 2)
                i高さの四角数の2倍 += 1
            Loop
        End If
        If i高さの四角数の2倍 = 0 Then
            i高さの四角数の2倍 = 1
        End If

        _Data.p_row底_縦横.Value("f_d高さの四角数") = i高さの四角数の2倍 / 2

        Return True
    End Function



#End Region

#Region "縁の始末"

    Function add_側面(ByVal nameselect As String, ByVal is縁 As Boolean,
                     ByVal i何本幅 As Integer, ByVal i周数 As Integer,
                     ByRef row As tbl側面Row) As Boolean

        Dim table As tbl側面DataTable = _Data.p_tbl側面

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

        Dim addNumber As Integer
        If is縁 Then
            addNumber = clsDataTables.cHemNumber
            clsDataTables.RemoveNumberFromTable(table, addNumber)
        Else
            addNumber = clsDataTables.AddNumber(table)
        End If
        If addNumber < 0 Then
            '{0}追加用の番号がとれません。
            p_sメッセージ = String.Format(My.Resources.CalcNoAddNumber, text編みかた名())
            Return False
        End If

        Dim d周長比率対底の周 As Double = 1
        Dim lastnum As Integer = clsDataTables.LastNumber(table)
        If 0 < lastnum Then
            Dim lastrow As tbl側面Row = clsDataTables.NumberFirstRecord(table, lastnum)
            If lastrow IsNot Nothing Then
                d周長比率対底の周 = lastrow.f_d周長比率対底の周
            End If
        End If

        'tbl編みかたぶんのレコード
        Dim groupRow As New clsGroupDataRow("f_iひも番号")
        For Each idx As Int16 In grpMst.Keys
            row = table.Newtbl側面Row
            row.f_i番号 = addNumber
            row.f_iひも番号 = idx
            row.f_s編みかた名 = nameselect
            row.f_i周数 = i周数
            row.f_d周長比率対底の周 = d周長比率対底の周

            groupRow.Add(row)
            table.Rows.Add(row)
        Next
        groupRow.SetNameIndexValue("f_s編みひも名", grpMst)
        groupRow.SetNameIndexValue("f_b周連続区分", grpMst)
        groupRow.SetNameIndexValue("f_dひも長加算", grpMst, "f_dひも長加算初期値")
        groupRow.SetNameIndexValue("f_sメモ", grpMst, "f_s備考")

        For Each drow As clsDataRow In groupRow
            Dim mst As New clsOptionDataRow(grpMst.IndexDataRow(drow)) '必ずある
            drow.Value("f_i何本幅") = mst.GetFirstLane(i何本幅)
        Next
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Side Add: {0}", groupRow.ToString)
        Return True
    End Function

    '更新処理が必要なフィールド名
    Shared _fields側面() As String = {"f_i何本幅", "f_i周数", "f_b周連続区分", "f_d周長比率対底の周"}
    Shared Function IsDataPropertyName側面(ByVal name As String) As Boolean
        Return _fields側面.Contains(name)
    End Function

    'IN:    p_d四角ベース_周
    'OUT:   _d縁の高さ _d縁の垂直ひも長 _d縁の厚さ 
    Private Function calc_側面(ByVal category As CalcCategory, ByVal row As tbl側面Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category <> CalcCategory.Edge Then
            'マスター変更もしくは派生更新
            ret = ret And recalc_側面()

        Else
            If row IsNot Nothing Then
                '追加もしくは更新
                If row IsNot Nothing Then
                    '追加もしくは更新
                    Dim cond As String = String.Format("f_i番号 = {0}", row.f_i番号)
                    Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond), "f_iひも番号")
                    If dataPropertyName = "f_i周数" Then
                        Dim i周数 As Integer = row.f_i周数
                        groupRow.SetNameValue("f_i周数", i周数)
                    End If
                    ret = ret And set_groupRow側面(groupRow)
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Side Change: {0}", groupRow.ToString)
                Else
                    '削除
                End If
            End If

        End If
        '高さの合計
        Dim obj As Object = _Data.p_tbl側面.Compute("SUM(f_d高さ)", "f_d高さ IS NOT NULL")
        If IsDBNull(obj) OrElse obj < 0 Then
            _d縁の高さ = 0
        Else
            _d縁の高さ = obj
        End If

        '垂直ひもの合計
        Dim obj2 As Object = _Data.p_tbl側面.Compute("SUM(f_d垂直ひも長)", "f_d垂直ひも長 IS NOT NULL")
        If IsDBNull(obj2) OrElse obj2 < 0 Then
            _d縁の垂直ひも長 = 0
        Else
            _d縁の垂直ひも長 = obj2
        End If

        '厚さの最大値
        Dim obj4 As Object = _Data.p_tbl側面.Compute("MAX(f_d厚さ)", "f_d厚さ IS NOT NULL")
        If IsDBNull(obj4) OrElse obj4 < 0 Then
            _d縁の厚さ = 0
        Else
            _d縁の厚さ = obj4
        End If

        Return ret
    End Function

    Private Function recalc_側面() As Boolean
        Dim res = (From row As tbl側面Row In _Data.p_tbl側面
                   Select Num = row.f_i番号
                   Order By Num).Distinct

        Dim ret As Boolean = True
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")
            ret = ret And set_groupRow側面(groupRow)
        Next

        Return ret
    End Function

    'IN:    p_d四角ベース_周    p_i垂直ひも数
    'OUT:
    Private Function set_groupRow側面(ByVal groupRow As clsGroupDataRow) As Boolean
        '周数は一致項目
        Dim i周数 As Integer = groupRow.GetNameValue("f_i周数")

        'マスタ参照なし
        For Each drow As clsDataRow In groupRow
            drow.Value("f_d周長") = p_d四角ベース_周 * drow.Value("f_d周長比率対底の周")
        Next

        'tbl編みかたRow
        Dim grpMst As clsGroupDataRow = g_clsMasterTables.GetPatternRecordGroup(groupRow.GetNameValue("f_s編みかた名"))
        If Not grpMst.IsValid Then
            'なし
            groupRow.SetNameValue("f_i段数", DBNull.Value)
            groupRow.SetNameValue("f_d高さ", DBNull.Value)
            groupRow.SetNameValue("f_d高さ比率", DBNull.Value)
            groupRow.SetNameValue("f_i段数", DBNull.Value)
            groupRow.SetNameValue("f_d垂直ひも長", DBNull.Value)
            groupRow.SetNameValue("f_dひも長", DBNull.Value)
            groupRow.SetNameValue("f_iひも本数", DBNull.Value)
            groupRow.SetNameValue("f_d厚さ", DBNull.Value)
            groupRow.SetNameValue("f_bError", True)

            '{0}の番号{1}で設定にない編みかた名'{2}'(ひも番号{3})が参照されています。
            p_sメッセージ = String.Format(My.Resources.CalcNoMasterPattern, text縁の始末(), groupRow.GetNameValue("f_i番号"), groupRow.GetNameValue("f_s編みかた名"), groupRow.GetNameValue("f_iひも番号"))
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
                    drow.Value("f_i段数") = i周数 * mst.Value("f_i周あたり段数")
                    drow.Value("f_d厚さ") = mst.Value("f_d厚さ")
                    If drow.Value("f_b周連続区分") Then
                        drow.Value("f_dひも長") = mst.GetContinuoutBandLength(nひも1何本幅, drow.Value("f_d周長"), p_i垂直ひも数, i周数)
                        drow.Value("f_iひも本数") = mst.Value("f_iひも数")
                    Else
                        drow.Value("f_dひも長") = mst.GetBandLength(nひも1何本幅, drow.Value("f_d周長"), p_i垂直ひも数)
                        drow.Value("f_iひも本数") = i周数 * mst.Value("f_iひも数")
                    End If

                Else
                    drow.Value("f_i段数") = DBNull.Value
                    drow.Value("f_d高さ") = DBNull.Value
                    drow.Value("f_d垂直ひも長") = DBNull.Value
                    drow.Value("f_dひも長") = DBNull.Value
                    drow.Value("f_iひも本数") = DBNull.Value
                    drow.Value("f_bError") = True
                    '{0}の番号{1}で設定にない編みかた名'{2}'(ひも番号{3})が参照されています。
                    p_sメッセージ = String.Format(My.Resources.CalcNoMasterPattern, text縁の始末(), drow.Value("f_i番号"), drow.Value("f_s編みかた名"), drow.Value("f_iひも番号"))
                    ret = False

                End If
                drow.Value("f_d高さ比率") = DBNull.Value
            Next
            Return ret
        End If

    End Function

#End Region

#Region "ひも上下"

    Function fitsizeひも上下(ByVal yoko As Boolean, ByVal sideup As Integer, ByVal botm As Integer) As clsUpDown

        Dim updown As New clsUpDown(clsUpDown.enumTargetFace.Bottom)
        updown.HorizontalCount = p_i横ひもの本数
        updown.VerticalCount = p_i縦ひもの本数

        'ゼロの場合は底の線だけ
        If sideup < 0 OrElse botm < 0 OrElse (sideup = 0 And botm = 0) Then
            If yoko Then
                '横のライン
                For horzIdx As Integer = 1 To updown.HorizontalCount
                    For vertIdx As Integer = 1 To updown.VerticalCount
                        If horzIdx + vertIdx = _i横の四角数 + 1 Then
                            updown.SetIsUp(horzIdx, vertIdx)
                        ElseIf ((p_i縦ひもの本数 - horzIdx + 1) + (p_i横ひもの本数 - vertIdx + 1)) = _i横の四角数 + 1 Then
                            updown.SetIsUp(horzIdx, vertIdx)
                        End If
                    Next
                Next
            Else
                '縦のライン
                For horzIdx As Integer = 1 To updown.HorizontalCount
                    For vertIdx As Integer = 1 To updown.VerticalCount
                        If ((p_i縦ひもの本数 - horzIdx + 1) + vertIdx) = _i縦の四角数 + 1 Then
                            updown.SetIsUp(horzIdx, vertIdx)
                        ElseIf (horzIdx + (p_i横ひもの本数 - vertIdx + 1)) = _i縦の四角数 + 1 Then
                            updown.SetIsUp(horzIdx, vertIdx)
                        End If
                    Next
                Next
            End If
            Return updown
        End If

        '縦のパターン　※辺を開始点として上へ
        Dim unit_vert As New clsUpDown(clsUpDown.enumTargetFace.Bottom)
        unit_vert.HorizontalCount = 1
        unit_vert.VerticalCount = (sideup + botm) * 2
        For y = 1 To sideup
            unit_vert.SetIsUp(1, y, yoko)
        Next
        For y = sideup + 1 To sideup + (sideup + botm)
            unit_vert.SetIsUp(1, y, Not yoko)
        Next
        For y = sideup + (sideup + botm) + 1 To sideup + (sideup + botm) + botm
            unit_vert.SetIsUp(1, y, yoko)
        Next

        '横のパターン　※辺を開始点として上へ
        Dim unit_horz As New clsUpDown(unit_vert)
        unit_horz.RotateLeft()
        unit_horz.Reverse()


        '・→→→HorizontalIndex
        '↓(左上)
        '↓
        '↓VerticalIndex

        '左上領域,右下領域
        For x As Integer = 1 To _i横の四角数
            For y As Integer = 1 To _i横の四角数
                If unit_vert.GetIsUp(1, y - x + 1) Then
                    Dim hidx_leup As Integer = x
                    Dim vidx_leup As Integer = _i横の四角数 - y + 1

                    Dim ca As center_area = isCenterArea(hidx_leup, vidx_leup)
                    If ca = center_area.center_lower Then
                        'Debug.Print("center_lower skip左上領域:({0},{1})", hidx_leup, vidx_leup)
                        Continue For
                    End If
                    updown.SetIsUp(hidx_leup, vidx_leup) '左上領域

                    If ca = center_area.center_line Then
                        'Debug.Print("center_line skip右下領域:({0},{1})", hidx_leup, vidx_leup)
                        Continue For
                    End If
                    '点対象位置
                    updown.SetIsUp(p_i縦ひもの本数 - hidx_leup + 1, p_i横ひもの本数 - vidx_leup + 1) '右下領域
                End If
            Next

        Next

        '右上領域,左下領域
        For y As Integer = 1 To _i縦の四角数
            For x As Integer = 1 To _i縦の四角数
                If unit_horz.GetIsUp(x - y + 1, 1) Then
                    Dim hidx_riup As Integer = _i横の四角数 + x
                    Dim vidx_riup As Integer = y

                    Dim ca As center_area = isCenterArea(hidx_riup, vidx_riup)
                    If ca = center_area.center_lower Then
                        'Debug.Print("center_lower skip右上領域:({0},{1})", hidx_riup, vidx_riup)
                        Continue For
                    End If
                    updown.SetIsUp(hidx_riup, y) '右上領域

                    If ca = center_area.center_line Then
                        'Debug.Print("center_line skip左下領域:({0},{1})", hidx_riup, vidx_riup)
                        Continue For
                    End If
                    '点対象位置
                    updown.SetIsUp(p_i縦ひもの本数 - hidx_riup + 1, p_i横ひもの本数 - vidx_riup + 1) '左下領域
                End If
            Next
        Next

        Return updown
    End Function

    '                                      縦<横  　　縦=横 　　横<縦
    Enum center_area
        center_none     '中央領域にない   ┼──┼     　　　　┼──┼　
        center_upper    '上側             │上／│     領域    │＼上│
        center_lower    '下側             │／下│     なし    │下＼│
        center_line     '中央線上         ┼──┼             ┼──┼
    End Enum

    '中央領域の内の位置を返す
    Private Function isCenterArea(ByVal horzIdx As Integer, ByVal vertIdx As Integer) As center_area
        If horzIdx <= p_i縦横四角数の小さい方 OrElse (p_i縦横四角数の小さい方 + p_i縦横四角数の差) < horzIdx Then
            Return center_area.center_none
        End If
        If vertIdx <= p_i縦横四角数の小さい方 OrElse (p_i縦横四角数の小さい方 + p_i縦横四角数の差) < vertIdx Then
            Return center_area.center_none
        End If

        '中央領域内
        If p_b横の四角数が縦以上 Then
            '／ 縦<横
            If horzIdx = (p_i横ひもの本数 - vertIdx + 1) Then
                Return center_area.center_line
            ElseIf horzIdx < (p_i横ひもの本数 - vertIdx + 1) Then
                Return center_area.center_upper
            Else
                Return center_area.center_lower
            End If
        Else
            '＼ 横<縦
            If horzIdx = vertIdx Then
                Return center_area.center_line
            ElseIf horzIdx < vertIdx Then
                Return center_area.center_lower
            Else
                Return center_area.center_upper
            End If
        End If
        Return True
    End Function



#End Region

#Region "縦横展開"

    Dim _tbl縦横展開_横ひも As tbl縦横展開DataTable 'New時に作成、以降は存在が前提
    Dim _tbl縦横展開_縦ひも As tbl縦横展開DataTable 'New時に作成、以降は存在が前提


    '横ひも: enumひも種.i_横 / enumひも種.i_45度 Or enumひも種.i_補強
    '縦ひも: enumひも種.i_縦 / enumひも種.i_315度 Or enumひも種.i_補強

    '底の_tbl縦横展開_横ひもを得る。isReset=Trueでリセット
    Function get横展開DataTable(Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        If isReset Then
            _Data.Removeひも種Rows(enumひも種.i_横)
            renew横展開DataTable(False)
            '_d四角ベース_縦計 = recalc_ひも展開(_tbl縦横展開_横ひも, enumひも種.i_横, _b横ひも本幅変更)
        Else
            'calc_横ひも展開(CalcCategory.Expand_Yoko, Nothing, Nothing)
            renew横展開DataTable(True)
        End If
        Return _tbl縦横展開_横ひも
    End Function

    '底の_tbl縦横展開_縦ひもを得る。isReset=Trueでリセット
    Function get縦展開DataTable(Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        If isReset Then
            _Data.Removeひも種Rows(enumひも種.i_縦)
            renew縦展開DataTable(False)
            '_d四角ベース_横計 = recalc_ひも展開(_tbl縦横展開_縦ひも, enumひも種.i_縦, _b縦ひも本幅変更)
        Else
            'calc_縦ひも展開(CalcCategory.Expand_Tate, Nothing, Nothing)
            renew縦展開DataTable(True)
        End If
        Return _tbl縦横展開_縦ひも
    End Function


    '底の横ひもの編集完了,_tbl縦横展開_横ひもを_Dataに反映
    Function save横展開DataTable(Optional ByVal always As Boolean = False) As Boolean
        Try
            If always OrElse _tbl縦横展開_横ひも.GetChanges IsNot Nothing Then
                Dim change As Integer = _Data.FromTmpTable(enumひも種.i_横 Or enumひも種.i_45度, _tbl縦横展開_横ひも)
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "save横展開DataTable")
            Return False
        End Try
    End Function

    '底の縦ひもの編集完了,_tbl縦横展開_縦ひもを_Dataに反映
    Function save縦展開DataTable(Optional ByVal always As Boolean = False) As Boolean
        Try
            If always OrElse _tbl縦横展開_縦ひも.GetChanges IsNot Nothing Then
                Dim change As Integer = _Data.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_315度, _tbl縦横展開_縦ひも)
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "save縦展開DataTable")
            Return False
        End Try
    End Function

    '縦横を展開したtbl縦横展開DataTableのレコードの初期化(なければ作成)
    Function prepare縦横展開DataTable() As Boolean
        Try
            Dim yokotable As tbl縦横展開DataTable = get横展開DataTable()
            _Data.FromTmpTable(enumひも種.i_横 Or enumひも種.i_45度, yokotable)

            Dim tatetable As tbl縦横展開DataTable = get縦展開DataTable()
            _Data.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_315度, tatetable)

            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "prepare縦横展開DataTable")
            Return False
        End Try
    End Function



    '横ひもの展開テーブルを作り直す
    'OUT:   _BandPositions横ひも
    Function renew横展開DataTable(ByVal isRefSaved As Boolean) As Boolean
        If Not isRefSaved Then
            _tbl縦横展開_横ひも.Clear()
        End If
        _tbl縦横展開_横ひも.AcceptChanges()

        Dim row As tbl縦横展開Row

        '横置き数分のレコード作成 (色,記号,メモは初期値)
        SetMaxひも番号(_tbl縦横展開_横ひも, p_i横ひもの本数)

        For idx As Integer = 1 To p_i横ひもの本数
            'row = tmptable.Newtbl縦横展開Row
            row = Find縦横展開Row(_tbl縦横展開_横ひも, enumひも種.i_横, idx, True)

            'row.f_iひも種 = enumひも種.i_横
            row.f_i位置番号 = 0 '別途計算
            row.f_sひも名 = text横ひも()
            'row.f_iひも番号 = idx
            row.f_d長さ = 0 '別途計算
            row.f_dひも長 = 0 '別途計算
            row.f_i何本幅 = _I基本のひも幅
            row.f_dひも長加算 = 0
            row.f_dひも長加算2 = 0 '使わない
            row.f_d出力ひも長 = 0 '別途計算

            'tmptable.Rows.Add(row)
        Next
        '以降は裏側, ひも長はFix
        Dim posyoko As Integer = cBackPosition
        If _Data.p_row底_縦横.Value("f_b補強ひも区分") Then
            For idx As Integer = 1 To 2
                'row = tmptable.Newtbl縦横展開Row
                row = Find縦横展開Row(_tbl縦横展開_横ひも, enumひも種.i_45度 Or enumひも種.i_補強, idx, True)

                'row.f_iひも種 = enumひも種.i_45度 Or enumひも種.i_補強
                row.f_i位置番号 = posyoko
                row.f_sひも名 = text横の補強ひも()
                'row.f_iひも番号 = idx
                row.f_d長さ = p_d四角ベース_横
                row.f_dひも長 = p_d四角ベース_横
                row.f_dひも長 = p_d四角ベース_横
                row.f_i何本幅 = _I基本のひも幅
                row.f_dひも長加算 = 0
                row.f_dひも長加算2 = 0 '使わない
                row.f_d出力ひも長 = row.f_dひも長

                'tmptable.Rows.Add(row)
                posyoko += 1
            Next

        Else
            row = Find縦横展開Row(_tbl縦横展開_横ひも, enumひも種.i_45度 Or enumひも種.i_補強, 1, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
            row = Find縦横展開Row(_tbl縦横展開_横ひも, enumひも種.i_45度 Or enumひも種.i_補強, 2, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
        End If
        _tbl縦横展開_横ひも.AcceptChanges()

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_横 Or enumひも種.i_45度, _tbl縦横展開_横ひも)
        End If
        '_ImageList横ひも = Nothing
        '_ImageList横ひも = New clsImageItemList(_tbl縦横展開_横ひも)

        ''横置きひも長の計算
        'imageList横位置(_ImageList横ひも)

        setBandPositions(enumひも種.i_横, _tbl縦横展開_横ひも, _BandPositions横ひも)
        setBandPositions横ひも()

        'Return _ImageList横ひも.NewTmpTable
        Return True
    End Function

    '縦ひもの展開テーブルを作り直す
    'OUT:   _BandPositions縦ひも
    Function renew縦展開DataTable(ByVal isRefSaved As Boolean) As Boolean
        If Not isRefSaved Then
            _tbl縦横展開_縦ひも.Clear()
        End If
        _tbl縦横展開_縦ひも.AcceptChanges()

        Dim row As tbl縦横展開Row

        SetMaxひも番号(_tbl縦横展開_縦ひも, p_i縦ひもの本数)
        '縦置き数分のレコード作成 (色,記号,メモは初期値)
        For idx As Integer = 1 To p_i縦ひもの本数
            'row = tmptable.Newtbl縦横展開Row
            row = Find縦横展開Row(_tbl縦横展開_縦ひも, enumひも種.i_縦, idx, True)

            'row.f_iひも種 = enumひも種.i_縦
            row.f_i位置番号 = 0 '別途計算
            row.f_sひも名 = text縦ひも()
            'row.f_iひも番号 = idx
            row.f_d長さ = 0 '別途計算
            row.f_dひも長 = 0 '別途計算
            row.f_i何本幅 = _I基本のひも幅
            row.f_dひも長加算 = 0
            row.f_dひも長加算2 = 0 '使わない
            row.f_d出力ひも長 = 0 '別途計算

            'tmptable.Rows.Add(row)
        Next
        '以降は裏側, ひも長はFix
        Dim postate As Integer = cBackPosition
        If _Data.p_row底_縦横.Value("f_b始末ひも区分") Then
            For idx As Integer = 1 To 2
                'row = tmptable.Newtbl縦横展開Row
                row = Find縦横展開Row(_tbl縦横展開_縦ひも, enumひも種.i_315度 Or enumひも種.i_補強, idx, True)

                'row.f_iひも種 = enumひも種.i_315度 Or enumひも種.i_補強
                row.f_i位置番号 = postate
                row.f_sひも名 = text縦の補強ひも()
                'row.f_iひも番号 = idx
                row.f_d長さ = p_d四角ベース_縦
                row.f_dひも長 = p_d四角ベース_縦
                row.f_i何本幅 = _I基本のひも幅
                row.f_dひも長加算 = 0
                row.f_dひも長加算2 = 0 '使わない
                row.f_d出力ひも長 = row.f_dひも長

                'tmptable.Rows.Add(row)
                postate += 1
            Next
        Else
            row = Find縦横展開Row(_tbl縦横展開_縦ひも, enumひも種.i_315度 Or enumひも種.i_補強, 1, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
            row = Find縦横展開Row(_tbl縦横展開_縦ひも, enumひも種.i_315度 Or enumひも種.i_補強, 2, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
        End If
        _tbl縦横展開_縦ひも.AcceptChanges()

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_縦 Or enumひも種.i_315度, _tbl縦横展開_縦ひも)
        End If
        '_ImageList縦ひも = Nothing
        '_ImageList縦ひも = New clsImageItemList(_tbl縦横展開_縦ひも)

        ''縦置きひも長の計算
        'imageList縦配置(_ImageList縦ひも)

        setBandPositions(enumひも種.i_縦, _tbl縦横展開_縦ひも, _BandPositions縦ひも)
        setBandPositions縦ひも()

        'Return _ImageList縦ひも.NewTmpTable
        Return True
    End Function


#End Region

#Region "リスト出力"
    '「連続ひも長」フィールドにひも長加算した値をセットする
    Private Function set側面_連続ひも長() As Boolean
        For Each row As tbl側面Row In _Data.p_tbl側面.Rows
            row.f_d連続ひも長 = (row.f_dひも長 + row.f_dひも長加算)
        Next row

        _Data.p_tbl側面.AcceptChanges()
        Return True
    End Function

    'リスト生成
    Public Function CalcOutput(ByVal output As clsOutput) As Boolean
        If output Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        set側面_連続ひも長()

        output.Clear()
        Dim row As tblOutputRow
        Dim order As String

        output.OutBasics(_Data.p_row目標寸法) '空行で終わる

        row = output.NextNewRow
        row.f_sカテゴリー = text四角数()
        row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str

        '***底の縦横
        'このカテゴリーは先に行をつくる
        row = output.NextNewRow
        '横置き,縦置き
        For yokotate As Integer = 1 To 2
            Dim tmpTable As tbl縦横展開DataTable
            Dim sbMemo As New Text.StringBuilder

            If yokotate = 1 Then
                row.f_sタイプ = text横置き()
                'tmpTable = set横展開DataTable(_Data.p_row底_縦横.Value("f_b展開区分"))
                tmpTable = get横展開DataTable()
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s横ひものメモ"))
            Else
                row.f_sタイプ = text縦置き()
                'tmpTable = set縦展開DataTable(_Data.p_row底_縦横.Value("f_b展開区分"))
                tmpTable = get縦展開DataTable()
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s縦ひものメモ"))
            End If
            If tmpTable Is Nothing OrElse tmpTable.Rows.Count = 0 Then
                Continue For
            End If
            'レコードあり

            '長い順に記号を振る
            Dim tmps() As tbl縦横展開Row = tmpTable.Select(Nothing, "f_iひも種 ASC, f_d出力ひも長 DESC, f_s色")
            For Each tt As tbl縦横展開Row In tmps
                tt.f_s記号 = output.GetBandMark(tt.f_i何本幅, tt.f_d出力ひも長, tt.f_s色)
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
                    output.SetBandRow(contcount, lasttmp.f_i何本幅, lasttmp.f_d出力ひも長, lasttmp.f_s色)
                    row.f_s長さ = output.outLengthText(lasttmp.f_d長さ)
                    If lasttmp.f_dひも長加算 <> 0 Then
                        row.f_s高さ = output.outLengthText(lasttmp.f_dひも長加算)
                    End If
                    row.f_sメモ = sbMemo.ToString
                    'If _Data.p_row底_縦横.Value("f_b展開区分") Then
                    If contcount = 1 Then
                        row.f_s編みひも名 = String.Format("{0}", lasttmp.f_iひも番号)
                    Else
                        row.f_s編みひも名 = String.Format("{0} - {1}", lasttmp.f_iひも番号, lasttmp.f_iひも番号 + contcount - 1)
                    End If
                    Select Case lasttmp.f_sひも名
                        Case text横ひも()
                            row.f_s編みひも名 = String.Format("[{0}] {1}", p_i横ひもの本数, row.f_s編みひも名)
                        Case text縦ひも()
                            row.f_s編みひも名 = String.Format("[{0}] {1}", p_i縦ひもの本数, row.f_s編みひも名)
                    End Select
                    'End If
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


        '***縁の始末
        If 0 < _Data.p_tbl側面.Rows.Count Then
            row = output.NextNewRow
            row.f_sカテゴリー = text縁の始末()
            row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_s高さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str

            order = "f_i番号 ASC , f_iひも番号 ASC"
            'リスト出力
            For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, order)
                row = output.NextNewRow
                If r.f_i番号 = cHemNumber Then
                    row.f_sタイプ = text縁の始末()
                Else
                    row.f_s番号 = r.f_i番号.ToString
                End If
                row.f_s編みかた名 = r.f_s編みかた名
                row.f_s編みひも名 = r.f_s編みひも名
                row.f_i周数 = r.f_i周数
                row.f_i段数 = r.f_i段数
                row.f_s高さ = output.outLengthText(r.f_d高さ)
                row.f_s長さ = output.outLengthText(r.f_d周長)
                If 0 < r.f_iひも本数 Then
                    If 0 <= r.f_d連続ひも長 Then
                        r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                    End If

                End If
                row.f_sメモ = r.f_sメモ
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
        row.f_sひも本数 = text内側()
        row.f_sひも長 = text外側()
        row.f_s本幅 = text四角数()

        row = output.NextNewRow
        row.f_s色 = text横寸法()
        row.f_s本幅 = _i横の四角数
        row.f_sひも本数 = output.outLengthText(p_d四角ベース_横)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_横)

        row = output.NextNewRow
        row.f_s色 = text縦寸法()
        row.f_s本幅 = _i縦の四角数
        row.f_sひも本数 = output.outLengthText(p_d四角ベース_縦)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_縦)

        row = output.NextNewRow
        row.f_s色 = text高さ寸法()
        row.f_s本幅 = _d高さの四角数
        row.f_sひも本数 = output.outLengthText(p_d四角ベース_高さ)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_高さ)

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

    Private Function text四角数() As String
        Return _frmMain.tpage四角数.Text
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

    Private Function text縦置き() As String
        Return _frmMain.grp縦置き.Text
    End Function

    Private Function text横の四角数() As String
        Return _frmMain.lbl横の四角数.Text
    End Function

    Private Function text横ひも() As String
        Return _frmMain.lbl横ひも.Text
    End Function

    Private Function text横の補強ひも() As String
        Return _frmMain.chk横の補強ひも.Text
    End Function

    Private Function text縦の四角数() As String
        Return _frmMain.lbl縦の四角数.Text
    End Function

    Private Function text縦ひも() As String
        Return _frmMain.lbl縦ひも.Text
    End Function

    Private Function text縦の補強ひも() As String
        Return _frmMain.chk縦の補強ひも.Text
    End Function

    Private Function text高さの四角数() As String
        Return _frmMain.lbl高さの四角数.Text
    End Function

    Private Function text縁の始末() As String
        Return _frmMain.tpage縁の始末.Text
    End Function

    Private Function text編みかた名() As String
        Return _frmMain.lbl編みかた名_側面.Text
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
        Return _frmMain.lbl横の四角数_単位.Text
    End Function

    Private Function text垂直ひも数() As String
        Return _frmMain.lbl垂直ひも数.Text
    End Function

#End Region

End Class
