

Imports System.Reflection
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Header
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstOutput

Class clsCalcKnot

    Dim _clsBandTypeGauge As clsBandTypeGauge
    Private Property p_dマイひも長係数 As Double '設定値


    '処理のカテゴリー
    Public Enum CalcCategory
        None

        NewData 'データ変更
        BsMaster  '基本値/マスター/バンド選択
        Target    '目標寸法(基本のひも幅以外)
        BandWidth   '基本のひも幅
        Knot 'コマ数・コマ間のすき間
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
    Private Property _dコマの寸法 As Double
    Private Property _dコマの要尺 As Double
    Private Property _dコマベース寸法 As Double
    Private Property _dコマベース要尺 As Double
    'コマ数
    Private Property _i横のコマ数 As Integer
    Private Property _i縦のコマ数 As Integer
    Private Property _i高さのコマ数 As Integer
    Private Property _i折り返しコマ数 As Integer
    Private Property _dコマ間のすき間 As Double 'nudゼロ以上
    '展開時参照
    Private Property _dひも長加算_縦横端 As Double 'プラスマイナス可
    Private Property _dひも長加算_側面 As Double 'プラスマイナス可


    '縁部分のみ
    Private Property _d縁の高さ As Double '縁の合計値,ゼロ以上
    Private Property _d縁の垂直ひも長 As Double '縁の合計値,ゼロ以上
    Private Property _d縁の厚さ As Double '縁の厚さの最大値,ゼロ以上

    '初期化
    Public Sub Clear()
        p_b有効 = False
        p_sメッセージ = Nothing

        _b内側区分 = False
        _d横_目標 = -1
        _d縦_目標 = -1
        _d高さ_目標 = -1
        _I基本のひも幅 = -1

        _dコマの寸法 = -1
        _dコマの要尺 = -1
        _dコマベース寸法 = -1
        _dコマベース要尺 = -1

        _i横のコマ数 = -1
        _i縦のコマ数 = -1
        _i高さのコマ数 = -1
        _i折り返しコマ数 = -1
        _dコマ間のすき間 = 0
        _dひも長加算_縦横端 = 0
        _dひも長加算_側面 = 0

        _d縁の高さ = 0
        _d縁の垂直ひも長 = 0
        _d縁の厚さ = 0

        p_dマイひも長係数 = My.Settings.MySafetyFactor
    End Sub

    Function SetBandName(ByVal bandtypename As String) As Boolean
        '同時設定値
        p_dマイひも長係数 = My.Settings.MySafetyFactor

        If _clsBandTypeGauge IsNot Nothing Then
            _clsBandTypeGauge = Nothing
        End If
        _clsBandTypeGauge = New clsBandTypeGauge(g_clsMasterTables, bandtypename)
        If Not _clsBandTypeGauge.IsValidValues(p_sメッセージ) Then
            p_b有効 = False
            Return False
        End If
        Return True
    End Function

#Region "プロパティ値"

    ReadOnly Property p_sコマの寸法 As String
        Get
            If 0 < _dコマの寸法 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_dコマの寸法)
            End If
            Return ""
        End Get
    End Property

    ReadOnly Property p_sコマの要尺 As String
        Get
            If 0 < _dコマの寸法 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_dコマの要尺)
            End If
            Return ""
        End Get
    End Property

    ReadOnly Property p_sコマベース寸法 As String
        Get
            If 0 < _dコマベース寸法 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_dコマベース寸法)
            End If
            Return ""
        End Get
    End Property

    ReadOnly Property p_sコマベース要尺 As String
        Get
            If 0 < _dコマベース要尺 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_dコマベース要尺)
            End If
            Return ""
        End Get
    End Property

    ReadOnly Property p_i横ひもの本数 As Integer
        Get
            Return _i縦のコマ数
        End Get
    End Property

    ReadOnly Property p_i縦ひもの本数 As Integer
        Get
            Return _i横のコマ数
        End Get
    End Property

    ReadOnly Property p_dコマベース_横 As Double
        Get
            Return _dコマベース寸法 * _i横のコマ数
        End Get
    End Property

    ReadOnly Property p_dコマベース_縦 As Double
        Get
            Return _dコマベース寸法 * _i縦のコマ数
        End Get
    End Property

    ReadOnly Property p_dコマベース_高さ As Double
        Get
            Return _dコマベース寸法 * _i高さのコマ数
        End Get
    End Property

    ReadOnly Property p_dコマベース_周 As Double
        Get
            Return 2 * (p_dコマベース_横 + p_dコマベース_縦)
        End Get
    End Property

    ReadOnly Property p_d四つ畳み編みの厚さ As Double
        Get
            Return g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
        End Get
    End Property

    ReadOnly Property p_d折り返しの厚さ As Double
        Get
            Return 2 * p_d四つ畳み編みの厚さ
        End Get
    End Property

    ReadOnly Property p_d厚さ As Double
        Get
            Dim d As Double = 0
            If 0 < _i折り返しコマ数 Then
                d = p_d折り返しの厚さ
            Else
                d = p_d四つ畳み編みの厚さ
            End If
            If d < _d縁の厚さ Then
                Return _d縁の厚さ
            Else
                Return d
            End If
        End Get
    End Property

    ReadOnly Property p_d縁厚さプラス_横 As Double
        Get
            If p_dコマベース_横 <= 0 Then
                Return 0
            End If
            Return p_dコマベース_横 + p_d厚さ '外側には1/2,その2倍
        End Get
    End Property

    ReadOnly Property p_d縁厚さプラス_縦 As Double
        Get
            If p_dコマベース_縦 <= 0 Then
                Return 0
            End If
            Return p_dコマベース_縦 + p_d厚さ '外側には1/2,その2倍
        End Get
    End Property

    ReadOnly Property p_d縁厚さプラス_高さ As Double
        Get
            If p_dコマベース_高さ < 0 Then
                Return 0
            End If
            Return p_dコマベース_高さ + _d縁の高さ + p_d四つ畳み編みの厚さ
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
    Public ReadOnly Property p_sコマベース_横 As String
        Get
            If 0 < p_dコマベース_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_dコマベース_横)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_sコマベース_縦 As String
        Get
            If 0 < p_dコマベース_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_dコマベース_縦)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_sコマベース_高さ As String
        Get
            If 0 <= p_dコマベース_高さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_dコマベース_高さ)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_sコマベース_周 As String
        Get
            If 0 < p_dコマベース_周 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_dコマベース_周)
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


    '計算寸法と目標寸法の差(コマベースの差)
    Public ReadOnly Property p_s横寸法の差 As String
        Get
            If 0 < p_dコマベース_横 AndAlso 0 < _d横_目標 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_dコマベース_横 - _d横_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s縦寸法の差 As String
        Get
            If 0 < p_dコマベース_縦 AndAlso 0 < _d縦_目標 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_dコマベース_縦 - _d縦_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s高さ寸法の差 As String
        Get
            If 0 < p_dコマベース_高さ AndAlso 0 < _d高さ_目標 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_dコマベース_高さ - _d高さ_目標, True)
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

        '
        If _ImageList横ひも IsNot Nothing Then
            For Each band As clsImageItem In _ImageList横ひも
                sb.AppendLine(band.ToString)
            Next
        End If
        If _ImageList縦ひも IsNot Nothing Then
            For Each band As clsImageItem In _ImageList縦ひも
                sb.AppendLine(band.ToString)
            Next
        End If

        Return sb.ToString
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("{0}({1}){2}", Me.GetType.Name, IIf(p_b有効, "Valid", "InValid"), p_sメッセージ).AppendLine()
        sb.AppendFormat("Target:({0},{1},{2}) Lane({3})", _d横_目標, _d縦_目標, _d高さ_目標, _I基本のひも幅).AppendLine()
        sb.AppendFormat("Unit: band({0}/{1}) square({2}/{3})", _dコマの寸法, _dコマの要尺, _dコマベース寸法, _dコマベース要尺).AppendLine()
        sb.AppendFormat("Piece: W({0},{1}) D({2},{3}) H({4},{5}) ", _i横のコマ数, p_dコマベース_横, _i縦のコマ数, p_dコマベース_縦, _i高さのコマ数, p_dコマベース_高さ).AppendLine()
        sb.AppendFormat("Edge:H({0}) VerticalLength({1}) Thickness({2})", _d縁の高さ, _d縁の垂直ひも長, _d縁の厚さ).AppendLine()
        Return sb.ToString
    End Function

#End Region


    '参照値
    Dim _Data As clsDataTables
    Dim _frmMain As frmMain



    Sub New(ByVal data As clsDataTables, ByVal frm As frmMain)
        _Data = data
        _frmMain = frm

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
                ret = ret And set_コマ数()
                ret = ret And set_寸法と要尺()
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And calc_追加品(category, Nothing, Nothing)

            Case CalcCategory.Target    '目標寸法
                ret = set_目標寸法(False)
                ret = ret And set_寸法と要尺()

            Case CalcCategory.BandWidth    '基本のひも幅
                ret = set_目標寸法(False)
                ret = ret And set_コマ数()
                ret = ret And set_寸法と要尺()

            Case CalcCategory.Knot 'コマ数・コマ間のすき間
                ret = ret And set_コマ数()
                ret = ret And set_寸法と要尺()

            Case CalcCategory.Edge  '縁の始末
                Dim row As tbl側面Row = CType(ctr, tbl側面Row)
                ret = ret And calc_側面(category, row, key)

            Case CalcCategory.Options  '追加品
                Dim row As tbl追加品Row = CType(ctr, tbl追加品Row)
                ret = ret And calc_追加品(category, row, key)
                '(追加品は計算寸法変更なし)

            Case Else
                '未定義のカテゴリー'{0}'が参照されました。
                p_sメッセージ = String.Format(My.Resources.CalcNoDefCategory, category)
                ret = False

        End Select

        p_b有効 = ret
        Return ret
    End Function


#Region "コマ"

    '目標寸法の取得
    Private Function set_目標寸法(ByVal needTarget As Boolean) As Boolean

        With _Data.p_row目標寸法
            _b内側区分 = .Value("f_b内側区分")

            _d横_目標 = .Value("f_d横寸法")
            _d縦_目標 = .Value("f_d縦寸法")
            _d高さ_目標 = .Value("f_d高さ寸法")

            _I基本のひも幅 = .Value("f_i基本のひも幅")
        End With

        If Not isValidTarget(needTarget) Then
            Return False
        End If

        Return True
    End Function

    'コマ数の取得
    'IN: _I基本のひも幅(チェック時)
    Function set_コマ数() As Boolean

        With _Data.p_row底_縦横
            _i横のコマ数 = .Value("f_i横の四角数")
            _i縦のコマ数 = .Value("f_i縦の四角数")
            _i高さのコマ数 = .Value("f_i高さのコマ数")
            _i折り返しコマ数 = .Value("f_i折り返しコマ数")
            _dコマ間のすき間 = _Data.p_row底_縦横.Value("f_dひも間のすき間")
            _dひも長加算_縦横端 = .Value("f_dひも長加算")
            _dひも長加算_側面 = .Value("f_dひも長加算_側面")
        End With

        Return IsValidInput()
    End Function

    Function set_寸法と要尺() As Boolean

        If _clsBandTypeGauge Is Nothing Then
            'バンドの種類が正しく設定されていません。
            p_sメッセージ = clsBandTypeGauge.p_sErrKnotBandType
            Return False
        End If
        If Not _clsBandTypeGauge.IsValidValues(p_sメッセージ) Then
            Return False
        End If

        _dコマの寸法 = _clsBandTypeGauge.Getコマ寸法値(_I基本のひも幅)
        _dコマの要尺 = _clsBandTypeGauge.Getコマ要尺値(_I基本のひも幅)
        _dコマベース寸法 = _dコマの寸法 + _dコマ間のすき間
        _dコマベース要尺 = _dコマの要尺 + _dコマ間のすき間

        Return True
    End Function

#End Region

#Region "概算"

    '真ん中の位置
    Public Shared Function KnotsCenter(ByVal pieces As Integer) As Integer
        If pieces Mod 2 = 0 Then
            '偶数
            Return (pieces \ 2)
        Else
            '奇数
            Return (pieces \ 2) + 1
        End If
    End Function

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

        If _i横のコマ数 <= 0 OrElse _i縦のコマ数 <= 0 Then
            '横のコマ数・縦のコマ数・高さのコマ数をセットしてください。
            p_sメッセージ = My.Resources.CalcNoPieceCountSet
            Return False
        End If
        If _i高さのコマ数 < 0 Then
            '横のコマ数・縦のコマ数・高さのコマ数をセットしてください。
            p_sメッセージ = My.Resources.CalcNoPieceCountSet
            Return False
        End If
        If _I基本のひも幅 <= 0 Then
            '基本のひも幅を設定してください。
            p_sメッセージ = My.Resources.CalcNoBaseBandSet
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
        If isNear(p_dコマベース_横, _d横_目標) _
            AndAlso isNear(p_dコマベース_縦, _d縦_目標) _
            AndAlso isNear(p_dコマベース_高さ, _d高さ_目標) Then
            'ほぼ目標のサイズになっています。やり直す場合はリセットしてください。
            p_sメッセージ = My.Resources.CalcNoMoreChange
            Return False
        End If

        '目標に基づき横・縦・高さのコマ数を再計算します。よろしいですか？
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

    '目標寸法→横・縦・高さのコマ数
    Private Function calc_Target() As Boolean
        If _dコマベース寸法 <= 0 Then
            Return False
        End If

        Dim ret As Boolean = True
        ret = ret And calc_Target_横()
        ret = ret And calc_Target_縦()
        ret = ret And calc_Target_高さ()
        Return ret
    End Function

    '横寸法から横のコマ数
    Private Function calc_Target_横() As Boolean
        Dim i横の四角数 As Integer = Int(_d横_目標 / _dコマベース寸法)
        If Not _b内側区分 Then
            Do While i横の四角数 * _dコマベース寸法 < _d横_目標
                i横の四角数 += 1
            Loop
        End If
        If i横の四角数 = 0 Then
            i横の四角数 = 1
        End If

        _Data.p_row底_縦横.Value("f_i横の四角数") = i横の四角数
        _Data.p_row底_縦横.Value("f_i左から何番目") = KnotsCenter(i横の四角数)

        Return True
    End Function

    '縦寸法から縦のコマ数
    Private Function calc_Target_縦()
        Dim i縦の四角数 As Integer = Int(_d縦_目標 / _dコマベース寸法)
        If Not _b内側区分 Then
            Do While i縦の四角数 * _dコマベース寸法 < _d縦_目標
                i縦の四角数 += 1
            Loop
        End If
        If i縦の四角数 = 0 Then
            i縦の四角数 = 1
        End If

        _Data.p_row底_縦横.Value("f_i縦の四角数") = i縦の四角数
        _Data.p_row底_縦横.Value("f_i上から何番目") = KnotsCenter(i縦の四角数)

        Return True
    End Function

    '高さ寸法から高さのコマ数
    Private Function calc_Target_高さ()
        Dim i高さのコマ数 As Integer = Int(_d高さ_目標 / _dコマベース寸法)
        If Not _b内側区分 Then
            Do While i高さのコマ数 * _dコマベース寸法 < _d高さ_目標
                i高さのコマ数 += 1
            Loop
        End If
        If i高さのコマ数 = 0 Then
            i高さのコマ数 = 1
        End If

        _Data.p_row底_縦横.Value("f_i高さのコマ数") = i高さのコマ数
        _Data.p_row底_縦横.Value("f_i折り返しコマ数") = 0

        Return True
    End Function



#End Region

#Region "側面と縁"
    Const cIdxHeight As Integer = 1
    Const cIdxFolding As Integer = 2

    '高さのコマ数+折り返しのコマ数をレコード化　※マイ係数未セット
    Function adjust_側面() As Boolean

        Dim table As tbl側面DataTable = _Data.p_tbl側面
        Dim row As tbl側面Row

        If 0 < _i高さのコマ数 Then
            For i As Integer = 1 To _i高さのコマ数
                row = clsDataTables.NumberSubRecord(table, cIdxHeight, i)
                If i = 1 OrElse _Data.p_row底_縦横.Value("f_b展開区分") Then
                    If row Is Nothing Then
                        row = table.Newtbl側面Row
                        row.f_i番号 = cIdxHeight
                        row.f_iひも番号 = i
                        table.Rows.Add(row)
                    End If
                    row.f_s編みかた名 = text四つ畳み編み()
                    row.f_s編みひも名 = text高さのコマ()
                    If _Data.p_row底_縦横.Value("f_b展開区分") Then
                        row.f_iひも本数 = 1
                    Else
                        row.f_iひも本数 = _i高さのコマ数
                    End If
                    row.f_i何本幅 = _I基本のひも幅
                    row.f_d高さ = _dコマベース寸法
                    row.f_d垂直ひも長 = _dコマベース要尺
                    row.f_d周長 = p_dコマベース_周
                    row.f_dひも長 = p_i垂直ひも数 * _dコマベース要尺 + _dひも長加算_側面
                    row.f_d厚さ = p_d四つ畳み編みの厚さ
                    '非表示
                    row.f_i周数 = 1
                    row.f_i段数 = p_i垂直ひも数 'コマ数
                Else
                    If row IsNot Nothing Then
                        table.Rows.Remove(row)
                    End If
                End If
            Next

            '以降はあれば削除
            Dim submax As Integer = clsDataTables.NumberSubMax(table, cIdxHeight)
            For i As Integer = _i高さのコマ数 + 1 To submax
                row = clsDataTables.NumberSubRecord(table, cIdxHeight, i)
                If row IsNot Nothing Then
                    table.Rows.Remove(row)
                End If
            Next

        Else
            RemoveNumberFromTable(table, cIdxHeight)
        End If

        If 0 < _i折り返しコマ数 Then
            For i As Integer = 1 To _i折り返しコマ数
                row = clsDataTables.NumberSubRecord(table, cIdxFolding, i)
                If i = 1 OrElse _Data.p_row底_縦横.Value("f_b展開区分") Then
                    If row Is Nothing Then
                        row = table.Newtbl側面Row
                        row.f_i番号 = cIdxFolding
                        row.f_iひも番号 = i
                        table.Rows.Add(row)
                    End If
                    row.f_s編みかた名 = text四つ畳み編み()
                    row.f_s編みひも名 = text折り返しのコマ()
                    If _Data.p_row底_縦横.Value("f_b展開区分") Then
                        row.f_iひも本数 = 1
                    Else
                        row.f_iひも本数 = _i折り返しコマ数
                    End If
                    row.f_i何本幅 = _I基本のひも幅
                    row.f_d高さ = 0
                    row.f_d垂直ひも長 = _dコマベース要尺
                    row.f_d周長 = p_dコマベース_周
                    row.f_dひも長 = p_i垂直ひも数 * _dコマベース要尺 + _dひも長加算_側面
                    row.f_d厚さ = p_d折り返しの厚さ
                    '非表示
                    row.f_i周数 = 1
                    row.f_i段数 = p_i垂直ひも数 'コマ数
                Else
                    If row IsNot Nothing Then
                        table.Rows.Remove(row)
                    End If
                End If
            Next

            '以降はあれば削除
            Dim submax As Integer = clsDataTables.NumberSubMax(table, cIdxFolding)
            For i As Integer = _i折り返しコマ数 + 1 To submax
                row = clsDataTables.NumberSubRecord(table, cIdxFolding, i)
                If row IsNot Nothing Then
                    table.Rows.Remove(row)
                End If
            Next

        Else
            RemoveNumberFromTable(table, cIdxFolding)
        End If

        Return True
    End Function

    Function add_側面_縁(ByVal nameselect As String,
                     ByVal i何本幅 As Integer,
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
    Shared _fields側面() As String = {"f_i何本幅"}
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
                '縁に対してのみ
                If row.f_i番号 = clsDataTables.cHemNumber Then
                    '追加もしくは更新
                    Dim cond As String = String.Format("f_i番号 = {0}", row.f_i番号)
                    Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond), "f_iひも番号")
                    If dataPropertyName = "f_i周数" Then
                        Dim i周数 As Integer = row.f_i周数
                        groupRow.SetNameValue("f_i周数", i周数)
                    End If
                    ret = ret And set_groupRow側面(groupRow)
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Side Change: {0}", groupRow.ToString)
                End If
            Else
                '削除
            End If

        End If
        Dim isHem As String = String.Format("f_i番号 = {0}", cHemNumber)
        '縁の高さの合計
        Dim obj As Object = _Data.p_tbl側面.Compute("SUM(f_d高さ)", isHem)
        If IsDBNull(obj) OrElse obj < 0 Then
            _d縁の高さ = 0
        Else
            _d縁の高さ = obj
        End If

        '縁の垂直ひもの合計
        Dim obj2 As Object = _Data.p_tbl側面.Compute("SUM(f_d垂直ひも長)", isHem)
        If IsDBNull(obj2) OrElse obj2 < 0 Then
            _d縁の垂直ひも長 = 0
        Else
            _d縁の垂直ひも長 = obj2
        End If

        '縁の厚さの最大値
        Dim obj4 As Object = _Data.p_tbl側面.Compute("MAX(f_d厚さ)", isHem)
        If IsDBNull(obj4) OrElse obj4 < 0 Then
            _d縁の厚さ = 0
        Else
            _d縁の厚さ = obj4
        End If

        Return ret
    End Function

    Private Function recalc_側面() As Boolean
        '縁についてのみ計算
        Dim cond As String = String.Format("f_i番号 = {0}", clsDataTables.cHemNumber)
        Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")
        Return set_groupRow側面(groupRow)
    End Function

    'IN:    p_dコマベース_周    p_i垂直ひも数
    'OUT:
    Private Function set_groupRow側面(ByVal groupRow As clsGroupDataRow) As Boolean
        If groupRow.Count = 0 Then
            Return True
        End If

        '周数は一致項目
        Dim i周数 As Integer = groupRow.GetNameValue("f_i周数")

        'マスタ参照なし
        For Each drow As clsDataRow In groupRow
            drow.Value("f_d周長") = p_dコマベース_周
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

#Region "追加品"

    Function add_追加品(ByVal nameselect As String,
                     ByVal i何本幅 As Integer, ByVal d長さ As Double, ByVal i点数 As Integer,
                     ByRef row As tbl追加品Row) As Boolean
        Dim table As tbl追加品DataTable = _Data.p_tbl追加品

        If String.IsNullOrWhiteSpace(nameselect) Then
            '{0}を指定してください。
            p_sメッセージ = String.Format(My.Resources.CalcNoSelect, text付属品名())
            Return False
        End If
        'tbl付属品Row
        Dim grpMst As New clsGroupDataRow(g_clsMasterTables.GetOptionRecordGroup(nameselect))
        If Not grpMst.IsValid Then
            '{0}'{1}'は登録されていません。
            p_sメッセージ = String.Format(My.Resources.CalcNoMaster, text付属品名(), nameselect)
            Return False
        End If

        Dim addNumber As Integer = clsDataTables.AddNumber(table)
        If addNumber < 0 Then
            '{0}追加用の番号がとれません。
            p_sメッセージ = String.Format(My.Resources.CalcNoAddNumber, text付属品名())
            Return False
        End If

        'tbl付属品ぶんのレコード
        Dim groupRow As New clsGroupDataRow("f_iひも番号")
        For Each idx As Int16 In grpMst.Keys
            row = table.Newtbl追加品Row
            row.f_i番号 = addNumber
            row.f_iひも番号 = idx
            row.f_s付属品名 = nameselect
            row.f_i点数 = i点数

            groupRow.Add(row)
            table.Rows.Add(row)
        Next
        groupRow.SetNameIndexValue("f_s付属品ひも名", grpMst)
        groupRow.SetNameIndexValue("f_b巻きひも区分", grpMst)
        groupRow.SetNameIndexValue("f_dひも長加算", grpMst, "f_dひも長加算初期値")
        groupRow.SetNameIndexValue("f_sメモ", grpMst, "f_s備考")

        Dim first As Boolean = True
        For Each drow As clsDataRow In groupRow
            Dim mst As New clsOptionDataRow(grpMst.IndexDataRow(drow)) '存在チェック済
            drow.Value("f_i何本幅") = mst.GetFirstLane(i何本幅)
            drow.Value("f_d長さ") = mst.GetLength(first, d長さ)
            If first Then
                first = False
            End If
        Next
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Option Add: {0}", groupRow.ToString)
        Return True
    End Function

    '更新処理が必要なフィールド名
    Shared _fields追加品() As String = {"f_i何本幅", "f_i点数", "f_d長さ"}
    Shared Function IsDataPropertyName追加品(ByVal name As String) As Boolean
        Return _fields追加品.Contains(name)
    End Function

    Private Function calc_追加品(ByVal category As CalcCategory, ByVal row As tbl追加品Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category <> CalcCategory.Options Then
            'マスター変更もしくは派生更新
            recalc_追加品()

        Else
            If row IsNot Nothing Then
                '追加もしくは更新
                Dim cond As String = String.Format("f_i番号 = {0}", row.f_i番号)
                Dim groupRow = New clsGroupDataRow(_Data.p_tbl追加品.Select(cond), "f_iひも番号")
                If dataPropertyName = "f_i点数" Then
                    Dim i点数 As Integer = row.f_i点数
                    groupRow.SetNameValue("f_i点数", i点数)
                End If
                ret = ret And set_groupRow追加品(groupRow)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Option Change: {0}", groupRow.ToString)
            Else
                '削除
            End If
        End If
        Return ret
    End Function

    Private Function recalc_追加品() As Boolean
        Dim res = (From row As tbl追加品Row In _Data.p_tbl追加品
                   Select Num = row.f_i番号
                   Order By Num).Distinct

        Dim ret As Boolean = True
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl追加品.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")
            ret = ret And set_groupRow追加品(groupRow)
        Next

        Return ret
    End Function

    Private Function set_groupRow追加品(ByVal groupRow As clsGroupDataRow) As Boolean
        '点数は一致項目
        Dim i点数 As Integer = groupRow.GetNameValue("f_i点数")

        'tbl付属品Row
        Dim grpMst As clsGroupDataRow = g_clsMasterTables.GetOptionRecordGroup(groupRow.GetNameValue("f_s付属品名"))
        If Not grpMst.IsValid Then
            'なし
            groupRow.SetNameValue("f_dひも長", DBNull.Value)
            groupRow.SetNameValue("f_iひも本数", DBNull.Value)
            groupRow.SetNameValue("f_bError", True)
            '{0}の番号{1}で設定にない付属品名'{2}'(ひも番号{3})が参照されています。
            p_sメッセージ = String.Format(My.Resources.CalcNoMasterOption, text追加品(), groupRow.GetNameValue("f_i番号"), groupRow.GetNameValue("f_s付属品名"), groupRow.GetNameValue("f_iひも番号"))
            Return False
        Else
            Dim ret As Boolean = True

            Dim i直前の何本幅 As Integer = 0
            For Each drow As clsDataRow In groupRow
                Dim mst As New clsOptionDataRow(grpMst.IndexDataRow(drow))
                If mst.IsValid Then
                    drow.Value("f_iひも本数") = i点数 * mst.Value("f_iひも数")

                    'ひも長
                    If drow.Value("f_b巻きひも区分") Then
                        drow.Value("f_dひも長") = mst.GetWindBandLength(drow.Value("f_d長さ"), drow.Value("f_i何本幅"), i直前の何本幅)
                    Else
                        drow.Value("f_dひも長") = mst.GetBandLength(drow.Value("f_d長さ"))
                    End If

                Else
                    drow.Value("f_iひも本数") = DBNull.Value
                    drow.Value("f_dひも長") = DBNull.Value
                    drow.Value("f_bError") = True
                    '{0}の番号{1}で設定にない付属品名'{2}'(ひも番号{3})が参照されています。
                    p_sメッセージ = String.Format(My.Resources.CalcNoMasterOption, text追加品(), drow.Value("f_i番号"), drow.Value("f_s付属品名"), drow.Value("f_iひも番号"))
                    ret = False

                End If
                i直前の何本幅 = drow.Value("f_i何本幅")
            Next

            Return ret
        End If

    End Function
#End Region

#Region "プレビュー描画"
    'CAD測定値
    Const c_foldingRatio As Double = 2 / 6.43   '要尺の折り位置までの比
    Const c_tiltRatio As Double = 0.216   '折りの傾き(幅に対する差の比)

    'リスト出力時に生成
    Dim _ImageList横ひも As clsImageItemList   '横ひもの展開レコードを含む
    Dim _ImageList縦ひも As clsImageItemList   '縦ひもの展開レコードを含む

    'プレビュー時に生成
    Dim _imageList側面ひも As clsImageItemList    '側面の展開レコードを含む
    Dim _ImageListコマ As clsImageItemList
    Dim _ImageList描画要素 As clsImageItemList '底や側面の展開図
    Dim _ImageList開始位置 As clsImageItemList

    Private Function toPoint(ByVal x As Double, ByVal y As Double) As S実座標
        Return New S実座標(_dコマベース寸法 * x, _dコマベース寸法 * y)
    End Function

    Private Function getBand(ByVal pos As enumひも種, ByVal idx As Integer) As tbl縦横展開Row
        Try
            Dim band As clsImageItem = Nothing
            Select Case pos
                Case enumひも種.i_横
                    '上からidx番目(1～),マイナスは下から(-1～-横のコマ数)
                    If idx < 0 Then
                        idx = _i横のコマ数 + idx + 1
                    End If
                    band = _ImageList横ひも.GetRowItem(enumひも種.i_横, idx, False)
                Case enumひも種.i_縦
                    '左からidx番目(1～),マイナスは右から(-1～-縦のコマ数)
                    If idx < 0 Then
                        idx = _i縦のコマ数 + idx + 1
                    End If
                    band = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, idx, False)
                Case enumひも種.i_側面
                    '下からidx番目の色(1～),マイナスは上から(-1～-側面のコマ数)
                    If idx < 0 Then
                        idx = _i高さのコマ数 + _i折り返しコマ数 + idx + 1
                    End If
                    band = _imageList側面ひも.GetRowItem(enumひも種.i_側面, idx, False)
            End Select
            If band Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "getBand Err:{0} {1}", pos, idx)
                Return Nothing
            End If
            Return band.m_row縦横展開

        Catch ex As Exception
            g_clsLog.LogException(ex, "getBand", pos, idx)
            Return Nothing
        End Try
    End Function

    'コマベース左上→コマの中心
    Private Function addコマ(ByVal p左上 As S実座標, ByVal y_band As tbl縦横展開Row, ByVal t_band As tbl縦横展開Row,
                           ByVal dひも幅 As Double, ByVal isleft As Boolean) As Boolean

        Dim knotitem As New clsImageItem(p左上 + (Unit315 * (_dコマベース寸法 / 2)), y_band, t_band,
                                            dひも幅, _dコマの寸法, _dコマ間のすき間, isleft)
        If _ImageListコマ Is Nothing Then
            _ImageListコマ = New clsImageItemList
        End If
        _ImageListコマ.Add(knotitem)

        Return y_band IsNot Nothing AndAlso t_band IsNot Nothing
    End Function

    '横ひもリストの記号出力情報
    Private Function imageList横記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal disp_band As Boolean) As Boolean
        If _ImageList横ひも Is Nothing Then
            Return False
        End If

        '側面いっぱい
        Dim length0 As S差分
        If disp_band Then
            length0 = Unit0 * _dコマベース寸法 * (_i高さのコマ数 + _i折り返しコマ数)
        End If
        Dim band270 As S差分 = Unit270 * dひも幅

        '左側面の左 コマは右側面も
        Dim _左上 As S実座標 = toPoint(-(_i高さのコマ数 + _i折り返しコマ数 + (_i横のコマ数 / 2)), (_i縦のコマ数 / 2))
        Dim _左上_右側面 As S実座標 = toPoint((_i横のコマ数 / 2), (_i縦のコマ数 / 2))
        Dim band_pos As S差分
        If Not isKnotLeft Then
            band_pos = Unit270 * ((_dコマベース寸法 / 2) - dひも幅)
        Else
            band_pos = Unit270 * (_dコマベース寸法 / 2)
        End If

        '上から下へ
        For idx As Integer = 1 To p_i横ひもの本数 '_i縦のコマ数
            Dim band As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, idx)
            band.m_rひも位置.p右上 = _左上 + band_pos + length0
            band.m_rひも位置.p左下 = _左上 + band_pos + band270
            '
            For j = 1 To _i高さのコマ数 + _i折り返しコマ数
                addコマ(_左上 + Unit0 * (j - 1) * _dコマベース寸法, band.m_row縦横展開, getBand(enumひも種.i_側面, -j), dひも幅, isKnotLeft)
                addコマ(_左上_右側面 + Unit0 * (j - 1) * _dコマベース寸法, band.m_row縦横展開, getBand(enumひも種.i_側面, j), dひも幅, isKnotLeft)
            Next
            _左上 = _左上 + (Unit270 * _dコマベース寸法)
            _左上_右側面 = _左上_右側面 + (Unit270 * _dコマベース寸法)
        Next

        Return True
    End Function

    '縦ひもリストの記号出力情報
    Private Function imageList縦記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal disp_band As Boolean) As Boolean
        If _ImageList縦ひも Is Nothing Then
            Return False
        End If

        '側面いっぱい
        Dim length270 As S差分
        If disp_band Then
            length270 = Unit270 * _dコマベース寸法 * (_i高さのコマ数 + _i折り返しコマ数)
        End If
        Dim band0 As S差分 = Unit0 * dひも幅

        '上側面の上
        Dim _左上 As S実座標 = toPoint(-(_i横のコマ数 / 2), _i高さのコマ数 + _i折り返しコマ数 + (_i縦のコマ数 / 2))
        Dim _左上_底 As S実座標 = toPoint(-(_i横のコマ数 / 2), (_i縦のコマ数 / 2))
        Dim band_pos As S差分
        If Not isKnotLeft Then
            band_pos = Unit0 * (_dコマベース寸法 / 2)
        Else
            band_pos = Unit0 * ((_dコマベース寸法 / 2) - dひも幅)
        End If

        '上側面;左から右へ
        For idx As Integer = 1 To p_i縦ひもの本数 '_i横のコマ数
            Dim band As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, idx)
            band.m_rひも位置.p右上 = _左上 + band_pos + band0
            band.m_rひも位置.p左下 = _左上 + band_pos + length270

            '底のコマ
            For j As Integer = 1 To p_i横ひもの本数 '_i縦のコマ数
                addコマ(_左上_底 + Unit270 * (j - 1) * _dコマベース寸法, getBand(enumひも種.i_横, j), band.m_row縦横展開, dひも幅, isKnotLeft)
            Next

            _左上 = _左上 + (Unit0 * _dコマベース寸法)
            _左上_底 = _左上_底 + (Unit0 * _dコマベース寸法)
        Next

        Return True
    End Function

    '_imageList側面ひも生成、縦横展開DataTable化したレコードを含む
    Function imageList側面記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal disp_band As Boolean) As Boolean

        '側面のレコードを縦横レコード化
        Dim tmptable As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        Dim idx As Integer = 1
        For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, "f_i番号 ASC , f_iひも番号 ASC")
            If r.f_i番号 = cHemNumber Then
                Continue For
            End If
            For i As Integer = 1 To r.f_iひも本数
                row = tmptable.Newtbl縦横展開Row
                row.f_iひも種 = enumひも種.i_側面
                row.f_iひも番号 = idx
                row.f_i何本幅 = r.f_i何本幅
                row.f_s記号 = r.f_s記号
                row.f_s色 = r.f_s色
                row.f_dひも長 = r.f_dひも長
                row.f_dひも長加算 = r.f_dひも長加算
                row.f_dひも長加算2 = 0
                row.f_d出力ひも長 = r.f_d連続ひも長
                tmptable.Rows.Add(row)

                idx += 1
            Next
        Next

        '以降参照するのでここでセットする
        _imageList側面ひも = New clsImageItemList(tmptable)


        '側面いっぱい
        Dim length0 As S差分
        If disp_band Then
            length0 = Unit0 * _dコマベース寸法 * _i横のコマ数
        End If
        Dim band270 As S差分 = Unit270 * dひも幅

        '下側面の左
        Dim _左上 As S実座標 = toPoint(-(_i横のコマ数 / 2), -(_i縦のコマ数 / 2))
        Dim _左下_上側面 As S実座標 = toPoint(-(_i横のコマ数 / 2), 1 + (_i縦のコマ数 / 2))
        Dim band_pos As S差分
        If Not isKnotLeft Then
            band_pos = Unit270 * ((_dコマベース寸法 / 2) - dひも幅)
        Else
            band_pos = Unit270 * (_dコマベース寸法 / 2)
        End If

        '下側面:上から下へ
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            Dim band As clsImageItem = _imageList側面ひも.GetRowItem(enumひも種.i_側面, i)
            band.m_rひも位置.p右上 = _左上 + band_pos + length0
            band.m_rひも位置.p左下 = _左上 + band_pos + band270
            '
            For j = 1 To p_i縦ひもの本数 '_i横のコマ数
                addコマ(_左上 + Unit0 * (j - 1) * _dコマベース寸法, band.m_row縦横展開, getBand(enumひも種.i_縦, j), dひも幅, isKnotLeft)
                addコマ(_左下_上側面 + Unit0 * (j - 1) * _dコマベース寸法, band.m_row縦横展開, getBand(enumひも種.i_縦, j), dひも幅, isKnotLeft)
            Next
            '
            _左上 = _左上 + (Unit270 * _dコマベース寸法)
            _左下_上側面 = _左下_上側面 + (Unit90 * _dコマベース寸法)
        Next
        Return True
    End Function

    Function imageList描画要素(ByVal basecross As Boolean, ByVal sideline As Boolean) As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList


        Dim d側面の高さ As Double = _dコマベース寸法 * (_i高さのコマ数 + _i折り返しコマ数)
        Dim d折り返し高さ As Double = _dコマベース寸法 * _i折り返しコマ数
        Dim deltaコマ右 As S差分 = Unit0 * _dコマベース寸法
        Dim deltaコマ下 As S差分 = Unit270 * _dコマベース寸法

        Dim a底 As S四隅
        a底.p左上 = toPoint(-_i横のコマ数 / 2, _i縦のコマ数 / 2)
        a底.p右上 = toPoint(_i横のコマ数 / 2, _i縦のコマ数 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        Dim delta As S差分
        Dim line As S線分

        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底
        For i As Integer = 1 To _i横のコマ数
            If i < _i横のコマ数 AndAlso basecross Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
            For j As Integer = 1 To _i縦のコマ数
                If i = 1 AndAlso basecross Then
                    line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                    line += deltaコマ下 * j
                    item.m_lineList.Add(line)
                End If
            Next
        Next
        itemlist.AddItem(item)

        '折り返し線
        Dim itemFolding As clsImageItem = Nothing
        If 0 < d折り返し高さ Then
            itemFolding = New clsImageItem(clsImageItem.ImageTypeEnum._折り返し線, 1)
        End If

        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.p左下 = a底.p左上
        item.m_a四隅.p右下 = a底.p右上
        delta = Unit90 * d側面の高さ
        item.m_a四隅.p左上 = item.m_a四隅.p左下 + delta
        item.m_a四隅.p右上 = item.m_a四隅.p右下 + delta
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
            line += Unit270 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数 - 1
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                line += deltaコマ下 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
            line += Unit90 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '下の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅.p左上 = a底.p左下
        item.m_a四隅.p右上 = a底.p右下
        delta = Unit270 * d側面の高さ
        item.m_a四隅.p左下 = item.m_a四隅.p左上 + delta
        item.m_a四隅.p右下 = item.m_a四隅.p右上 + delta
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下)
            line += Unit90 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                line += deltaコマ下 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下)
            line += Unit270 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '左の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅.p右上 = a底.p左上
        item.m_a四隅.p右下 = a底.p左下
        delta = Unit180 * d側面の高さ
        item.m_a四隅.p左上 = item.m_a四隅.p右上 + delta
        item.m_a四隅.p左下 = item.m_a四隅.p右下 + delta
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
            line += Unit0 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
            line += Unit180 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p左上 = a底.p右上
        item.m_a四隅.p左下 = a底.p右下
        delta = Unit0 * d側面の高さ
        item.m_a四隅.p右上 = item.m_a四隅.p左上 + delta
        item.m_a四隅.p右下 = item.m_a四隅.p左下 + delta
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下)
            line += Unit180 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下)
            line += Unit0 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)


        If itemFolding IsNot Nothing Then
            itemlist.AddItem(itemFolding)
        End If

        Return itemlist
    End Function

    Function imageList開始位置(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal outp As clsOutput) As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim line As clsImageItem.S線分

        '要尺の折り位置
        Dim foldingLen As Double = c_foldingRatio * _dコマの要尺

        '要尺
        If True Then
            Dim p要尺位置 As S実座標 = toPoint(-(_i横のコマ数 / 2) - (_i高さのコマ数 + _i折り返しコマ数), (_i縦のコマ数 / 2) + (_i高さのコマ数 + _i折り返しコマ数))
            If _i高さのコマ数 + _i折り返しコマ数 = 0 Then
                p要尺位置 = p要尺位置 + Unit90 * (_dコマベース寸法)
            End If
            If (_i高さのコマ数 + _i折り返しコマ数 - 1) * _dコマベース寸法 < _dコマベース要尺 Then
                p要尺位置.X = -(1 + _i横のコマ数 / 2) * _dコマベース寸法 - (_dコマベース要尺 / 2)
            Else
                p要尺位置.X = p要尺位置.X + (_dコマベース要尺 / 2)
            End If
            item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 9)
            Dim delta As S差分 = Unit270 * dひも幅
            item.m_a四隅.p左上 = p要尺位置 + Unit180 * (_dコマベース要尺 / 2) + Unit90 * (dひも幅 / 2)
            item.m_a四隅.p右上 = p要尺位置 + Unit0 * (_dコマベース要尺 / 2) + Unit90 * (dひも幅 / 2)
            item.m_a四隅.p左下 = item.m_a四隅.p左上 + delta
            item.m_a四隅.p右下 = item.m_a四隅.p右上 + delta
            Dim p As S実座標 = p要尺位置
            If 0 < _dコマ間のすき間 Then
                p = item.m_a四隅.p左上 + Unit0 * (_dコマ間のすき間 / 2)
                line = New S線分(p, p + delta)
                item.m_lineList.Add(line)
                p = item.m_a四隅.p右上 + Unit180 * (_dコマ間のすき間 / 2)
                line = New S線分(p, p + delta)
                item.m_lineList.Add(line)
            End If
            '中心から各折り位置まで
            Dim len_1 As Double = (_dコマの要尺 - foldingLen * 2) / 2 - (c_tiltRatio * dひも幅 / 2)
            Dim len_2 As Double = (_dコマの要尺 - foldingLen * 2) / 2 + (c_tiltRatio * dひも幅 / 2)
            If isKnotLeft Then
                Dim tmp As Double = len_1
                len_1 = len_2
                len_2 = tmp
            End If
            line = New S線分(New S実座標(p要尺位置.X + len_1, p要尺位置.Y + dひも幅 / 2), New S実座標(p要尺位置.X + len_2, p要尺位置.Y - dひも幅 / 2))
            item.m_lineList.Add(line)
            line = New S線分(New S実座標(p要尺位置.X - len_1, p要尺位置.Y - dひも幅 / 2), New S実座標(p要尺位置.X - len_2, p要尺位置.Y + dひも幅 / 2))
            item.m_lineList.Add(line)
            itemlist.AddItem(item)
        End If

        '開始位置の情報をセット
        Dim startInfo As CStartInfo = setStartInfo()
        If startInfo Is Nothing Then
            '設定されていなければ
            Return itemlist 'ここまでの結果
        End If

        '底の開始位置のマーク
        If True Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)
            Dim pコマ左上 As S実座標 = toPoint(-(_i横のコマ数 / 2) + (startInfo.i左から何番目 - 1), (_i縦のコマ数 / 2) - (startInfo.i上から何番目 - 1))
            Dim pコマ右下 As S実座標 = pコマ左上 + Unit315 * _dコマベース寸法

            line = New clsImageItem.S線分(pコマ左上, pコマ左上 + Unit0 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ左上, pコマ左上 + Unit270 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ右下, pコマ右下 + Unit90 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ右下, pコマ右下 + Unit180 * _dコマベース寸法)
            item.m_lineList.Add(line)

            itemlist.AddItem(item)
        End If


        '開始位置のコマの転記
        Dim dバンド長 As Double = _dコマベース要尺
        Dim pコマ位置 As S実座標 = toPoint(0, -_i縦のコマ数 / 2 - (_i高さのコマ数 + _i折り返しコマ数) - 1) + Unit270 * dバンド長
        item = New clsImageItem(pコマ位置, startInfo.row横展開, startInfo.row縦展開,
            dひも幅, _dコマの寸法, _dコマ間のすき間, isKnotLeft, True)
        itemlist.AddItem(item)

        'コマに続くバンド
        If True Then
            Dim item1 As New clsImageItem(startInfo.row横展開, True) '右
            Dim item2 As New clsImageItem(startInfo.row横展開, True) '左
            item1.m_dひも幅 = dひも幅
            item2.m_dひも幅 = dひも幅
            If isKnotLeft Then
                item1.m_rひも位置.p左下 = pコマ位置 + Unit0 * dひも幅
                item2.m_rひも位置.p右上 = pコマ位置 + Unit180 * dひも幅
            Else
                item1.m_rひも位置.p左下 = pコマ位置 + Unit315 * dひも幅
                item2.m_rひも位置.p右上 = pコマ位置 + Unit135 * dひも幅
            End If
            item1.m_rひも位置.p右上 = New S実座標(item1.m_rひも位置.p左下.X + dバンド長, item1.m_rひも位置.p左下.Y + dひも幅)
            item2.m_rひも位置.p左下 = New S実座標(item2.m_rひも位置.p右上.X - dバンド長, item2.m_rひも位置.p右上.Y - dひも幅)
            itemlist.AddItem(item1)
            itemlist.AddItem(item2)

            Dim item3 As New clsImageItem(startInfo.row縦展開, True) '上
            Dim item4 As New clsImageItem(startInfo.row縦展開, True) '下
            item3.m_dひも幅 = dひも幅
            item4.m_dひも幅 = dひも幅
            If isKnotLeft Then
                item3.m_rひも位置.p左下 = pコマ位置 + Unit135 * dひも幅
                item4.m_rひも位置.p右上 = pコマ位置 + Unit315 * dひも幅
            Else
                item3.m_rひも位置.p左下 = pコマ位置 + Unit90 * dひも幅
                item4.m_rひも位置.p右上 = pコマ位置 + Unit270 * dひも幅
            End If
            item3.m_rひも位置.p右上 = New S実座標(item3.m_rひも位置.p左下.X + dひも幅, item3.m_rひも位置.p左下.Y + dバンド長)
            item4.m_rひも位置.p左下 = New S実座標(item4.m_rひも位置.p右上.X - dひも幅, item4.m_rひも位置.p右上.Y - dバンド長)
            itemlist.AddItem(item3)
            itemlist.AddItem(item4)
        End If


        '「開始位置」
        Dim p開始位置 As S実座標 = pコマ位置 + Unit135 * (dバンド長) + Unit180 * (dバンド長)
        item = New clsImageItem(p開始位置, {text開始位置()}, dひも幅, 1)
        itemlist.AddItem(item)

        '左から×番目,上から×番目
        Dim pos1 As String = text左から() & startInfo.i左から何番目.ToString & text番目のコマ()
        Dim pos2 As String = text上から() & startInfo.i上から何番目.ToString & text番目のコマ()
        item = New clsImageItem(p開始位置 + Unit315 * (dひも幅 * 2), {pos1, pos2}, dひも幅 * 2 / 3, 2)
        itemlist.AddItem(item)


        '各方向の説明文字列 
        If True Then
            Dim p文字(3) As S実座標
            '上のひも
            p文字(0).X = pコマ位置.X + dひも幅 * 2
            p文字(0).Y = pコマ位置.Y + dバンド長
            '下のひも
            p文字(1).X = pコマ位置.X + dひも幅 * 2
            p文字(1).Y = pコマ位置.Y - 2 * dバンド長 / 3
            '左のひも
            p文字(2).X = pコマ位置.X - dバンド長 - dひも幅 * 15 '想定文字数分
            p文字(2).Y = pコマ位置.Y - dひも幅 * 1.5
            '右のひも
            p文字(3).X = pコマ位置.X + dバンド長 + _dコマの寸法
            p文字(3).Y = pコマ位置.Y + dひも幅

            startInfo.setMyValue(True) 'マイひも長係数を乗算する
            For i As Integer = 0 To 3
                With startInfo
                    '<コマの{0}>
                    Dim str1 As String = String.Format(My.Resources.CalcOutKnotOf, .getSideString(i))
                    str1 += outp.outLengthTextWithUnit(.getBandLength(i))
                    str1 += "  "
                    str1 += .getDiffString(i, outp)

                    '加算計 {0} (縁の始末:{1} ひも長加算:{2} {3}加算:{4})
                    Dim str2 As String = String.Format(My.Resources.CalcOutAddLen,
                        outp.outLengthTextWithUnit(.getAddSum(i)), outp.outLengthText(.getAddHem()), outp.outLengthText(.getAddVert()), .getSideString(i), outp.outLengthText(.getAddEach(i)))

                    '折り位置から
                    Dim str3 As String = String.Format(My.Resources.CalcOutFromFolding)
                    str3 += outp.outLengthTextWithUnit(.getFoldingLength(i))
                    str3 += "  "
                    str3 += .getDiffFoldingString(i, outp)
                    '
                    item = New clsImageItem(p文字(i), {str1, str2, str3}, dひも幅 * 2 / 3, i + 3)
                    itemlist.AddItem(item)
                End With
            Next
        End If

        'コマ位置の中央線
        startInfo.setMyValue(False) 'マイひも長係数を乗算しない
        If True Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 2)
            Dim d1本幅 As Double = dひも幅 / _I基本のひも幅
            If Abs(startInfo.getDiff(0)) < dバンド長 * 2 + _dコマベース要尺 + d1本幅 Then
                '上下中央の横線
                Dim pp As New S実座標(pコマ位置.X - dひも幅 * 1.5, pコマ位置.Y)
                line = New S線分(pp, pp + Unit0 * dひも幅 * 3)
                If Abs(startInfo.getDiff(0)) < d1本幅 Then
                    '1本幅以下
                    item.m_lineList.Add(line)
                ElseIf (_dコマの要尺 / 2 - d1本幅) <= startInfo.getDiff(0) Then
                    '要尺以上、上が長いので中央点は上にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(0) - _dコマベース要尺) / 2
                    line = line + Unit90 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                ElseIf startInfo.getDiff(0) <= -(_dコマの要尺 / 2 - d1本幅) Then
                    '要尺以上、下が長いので中央点は下にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(1) - _dコマベース要尺) / 2
                    line = line + Unit270 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                Else
                    '描けません
                End If
            End If
            If Abs(startInfo.getDiff(2)) < dバンド長 * 2 + _dコマベース要尺 + d1本幅 Then
                '左右中央の縦線
                Dim pp As New S実座標(pコマ位置.X, pコマ位置.Y - dひも幅 * 1.5)
                line = New S線分(pp, pp + Unit90 * dひも幅 * 3)
                If Abs(startInfo.getDiff(2)) < d1本幅 Then
                    '1本幅以下
                    item.m_lineList.Add(line)
                ElseIf (_dコマの要尺 / 2 - d1本幅) <= startInfo.getDiff(2) Then
                    '要尺以上、左が長いので中央点は左にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(2) - _dコマベース要尺) / 2
                    line = line + Unit180 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                ElseIf startInfo.getDiff(2) <= -(_dコマの要尺 / 2 - d1本幅) Then
                    '要尺以上、右が長いので中央点は右にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(3) - _dコマベース要尺) / 2
                    line = line + Unit0 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                Else
                    '描けません
                End If
            End If
            If 0 < item.m_lineList.Count Then
                itemlist.AddItem(item)
            End If
        End If


        Return itemlist
    End Function

    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '念のため
        _imageList側面ひも = Nothing
        _ImageListコマ = Nothing
        _ImageList描画要素 = Nothing
        _ImageList開始位置 = Nothing

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        'リスト処理で残された情報
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '記号表示の位置をセット
        Dim isKnotLeft As Boolean = False
        Select Case _Data.p_row底_縦横.Value("f_iコマ上側の縦ひも")
            Case enumコマ上側の縦ひも.i_どちらでも
                isKnotLeft = My.Settings.IsKnotLeft
            Case enumコマ上側の縦ひも.i_左側
                isKnotLeft = True
            Case enumコマ上側の縦ひも.i_右側
                isKnotLeft = False
        End Select
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)

        '基本のひも幅と基本色
        imgData.setBasics(_dコマの寸法, _Data.p_row目標寸法.Value("f_s基本色"))

#If 1 Then
        '通常描画
        imageList側面記号(dひも幅, isKnotLeft, False)
        imageList横記号(dひも幅, isKnotLeft, False)
        imageList縦記号(dひも幅, isKnotLeft, False)
        _ImageList開始位置 = imageList開始位置(dひも幅, isKnotLeft, outp)
        _ImageList描画要素 = imageList描画要素(False, False)
#Else
        'バンド描画
        imageList側面記号(dひも幅, isKnotLeft, True)
        imageList横記号(dひも幅, isKnotLeft, True)
        imageList縦記号(dひも幅, isKnotLeft, True)
        _ImageList開始位置 = imageList開始位置(dひも幅, isKnotLeft)
        _ImageList描画要素 = imageList描画要素(True, True)
#End If

        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing
        imgData.MoveList(_imageList側面ひも)
        _imageList側面ひも = Nothing
#If 1 Then
        imgData.MoveList(_ImageListコマ)
#End If
        _ImageListコマ = Nothing
        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing
        imgData.MoveList(_ImageList開始位置)
        _ImageList開始位置 = Nothing

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function
#End Region

#Region "縦横展開と開始位置"
    '横ひもの展開テーブル作成 ※入力値ベース
    'OUT:   _ImageList横ひも
    Function set横展開DataTable(ByVal isRefSaved As Boolean) As tbl縦横展開DataTable

        Dim tmptable As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        Dim pos As Integer = -(p_i横ひもの本数 \ 2)
        Dim zero As Integer = p_i横ひもの本数 Mod 2

        '横置き数分のレコード作成 (色,記号,メモは初期値)
        For idx As Integer = 1 To p_i横ひもの本数
            row = tmptable.Newtbl縦横展開Row

            row.f_iひも種 = enumひも種.i_横
            row.f_i位置番号 = pos
            row.f_sひも名 = text横ひも()
            row.f_iひも番号 = idx
            Dim iコマ数 As Integer = _i横のコマ数 + 2 * (_i高さのコマ数 + _i折り返しコマ数)
            row.f_d長さ = iコマ数 * _dコマベース寸法
            row.f_dひも長 = iコマ数 * _dコマベース要尺 + 2 * (_d縁の垂直ひも長 + _dひも長加算_縦横端)
            row.f_i何本幅 = _I基本のひも幅
            row.f_dひも長加算 = 0
            row.f_dひも長加算2 = 0
            row.f_d出力ひも長 = row.f_dひも長 '加算ゼロ

            tmptable.Rows.Add(row)
            pos += 1
            If pos = 0 AndAlso zero = 0 Then
                pos = 1
            End If
        Next

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_横, tmptable) '出力ひも長も加算
        End If
        _ImageList横ひも = Nothing
        _ImageList横ひも = New clsImageItemList(tmptable)

        'image計算結果は不要
        Return tmptable
    End Function

    '縦ひもの展開テーブル作成 ※入力値ベース
    'OUT:   _ImageList縦ひも
    Function set縦展開DataTable(ByVal isRefSaved As Boolean) As tbl縦横展開DataTable

        Dim tmptable As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        Dim pos As Integer = (p_i縦ひもの本数 \ 2)
        Dim zero As Integer = p_i縦ひもの本数 Mod 2

        '縦置き数分のレコード作成 (色,記号,メモは初期値)
        For idx As Integer = 1 To p_i縦ひもの本数
            row = tmptable.Newtbl縦横展開Row

            row.f_iひも種 = enumひも種.i_縦
            row.f_i位置番号 = pos
            row.f_sひも名 = text縦ひも()
            row.f_iひも番号 = idx
            Dim iコマ数 As Integer = _i縦のコマ数 + 2 * (_i高さのコマ数 + _i折り返しコマ数)
            row.f_d長さ = iコマ数 * _dコマベース寸法
            row.f_dひも長 = iコマ数 * _dコマベース要尺 + 2 * (_d縁の垂直ひも長 + _dひも長加算_縦横端)
            row.f_i何本幅 = _I基本のひも幅
            row.f_dひも長加算 = 0
            row.f_dひも長加算2 = 0
            row.f_d出力ひも長 = row.f_dひも長 '加算ゼロ

            tmptable.Rows.Add(row)
            pos -= 1
            If pos = 0 AndAlso zero = 0 Then
                pos = -1
            End If
        Next

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_縦, tmptable) '出力ひも長も加算
        End If
        _ImageList縦ひも = Nothing
        _ImageList縦ひも = New clsImageItemList(tmptable)

        'image計算結果は不要
        Return tmptable
    End Function


    '開始位置情報
    Private Class CStartInfo
        Dim _parent As clsCalcKnot  '親クラス

        '各方向の計算値　上,下,左,右の順
        Dim _knots(3) As Integer 'コマ数
        Dim _addeach(3) As Double '個別の加算長
        Dim _addsum(3) As Double '加算合計
        Dim _bandlen(3) As Double 'ひもの長さ ※開始位置コマのすき間を含まない
        Dim _diff(3) As Double '差,中央線で参照
        Dim _foldinglen(3) As Double '折り位置からのひも長
        Dim _foldingdiff(3) As Double '折り位置前後の差

        Dim _isMyValue As Boolean = False
        Dim _SideString() As String

        Friend i左から何番目 As Integer
        Friend i上から何番目 As Integer

        Friend row横展開 As tbl縦横展開Row '横バンド
        Friend row縦展開 As tbl縦横展開Row '縦バンド

        ReadOnly Property IsValid As Boolean = False

        Sub New(ByVal parent As clsCalcKnot, ByVal i左 As Integer, ByVal i上 As Integer)
            _parent = parent

            i左から何番目 = i左
            i上から何番目 = i上
            row横展開 = _parent.getBand(enumひも種.i_横, i上から何番目)
            row縦展開 = _parent.getBand(enumひも種.i_縦, i左から何番目)

            If row横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "getBand({0},{1})", enumひも種.i_横, i上から何番目)
                Return
            End If
            If row縦展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "getBand({0},{1})", enumひも種.i_縦, i左から何番目)
                Return
            End If
            _IsValid = True


            ReDim _SideString(3)
            _SideString(0) = My.Resources.CalcOutSideUpper '下
            _SideString(1) = My.Resources.CalcOutSideLower '上
            _SideString(2) = My.Resources.CalcOutSideLeft '右
            _SideString(3) = My.Resources.CalcOutSideRight '左

            With _parent
                '上のひも
                _knots(0) = ._i高さのコマ数 + ._i折り返しコマ数 + (i上から何番目 - 1)
                _addeach(0) = row縦展開.f_dひも長加算
                '下のひも
                _knots(1) = ._i高さのコマ数 + ._i折り返しコマ数 + (._i縦のコマ数 - i上から何番目)
                _addeach(1) = row縦展開.f_dひも長加算2
                '左のひも
                _knots(2) = ._i高さのコマ数 + ._i折り返しコマ数 + (i左から何番目 - 1)
                _addeach(2) = row横展開.f_dひも長加算
                '右のひも
                _knots(3) = ._i高さのコマ数 + ._i折り返しコマ数 + (._i横のコマ数 - i左から何番目)
                _addeach(3) = row横展開.f_dひも長加算2

                For i As Integer = 0 To 3
                    _addsum(i) = ._d縁の垂直ひも長 + ._dひも長加算_縦横端 + _addeach(i)
                    _bandlen(i) = _knots(i) * ._dコマベース要尺 + _addsum(i)
                Next

                _diff(0) = _bandlen(0) - _bandlen(1)
                _diff(1) = -_diff(0)

                _diff(2) = _bandlen(2) - _bandlen(3)
                _diff(3) = -_diff(2)

                For i As Integer = 0 To 3
                    Dim foldingLen As Double = c_foldingRatio * _parent._dコマの要尺
                    '折り位置からのひも長
                    _foldinglen(i) = _bandlen(i) + foldingLen + (_parent._dコマ間のすき間 / 2)
                    '折り位置前後の差
                    _foldingdiff(i) = _diff(i) - (_parent._dコマの要尺 - 2 * foldingLen)
                Next
            End With

        End Sub

        'マイひも長係数を掛けた値を返すときTrue
        Sub setMyValue(ByVal ismyvalue As Boolean)
            _isMyValue = ismyvalue AndAlso
                    (0 < _parent.p_dマイひも長係数) AndAlso
                    (_parent.p_dマイひも長係数 <> 1)
        End Sub

        '方向の文字列
        Function getSideString(ByVal i As Integer) As String
            Return _SideString(i)
        End Function

        '反対方向の文字列
        Function getPairSideString(ByVal i As Integer) As String
            If i = 0 Or i = 2 Then
                Return _SideString(i + 1)
            End If
            If i = 1 Or i = 3 Then
                Return _SideString(i - 1)
            End If
            Return Nothing
        End Function

        'コマの数
        Function knots(ByVal i As Integer) As Integer
            Return _knots(i)
        End Function

        'コマ数分の長さ
        Function getKnotLength(ByVal i As Integer) As Double
            If _isMyValue Then
                Return knots(i) * _parent._dコマベース要尺 * _parent.p_dマイひも長係数
            Else
                Return knots(i) * _parent._dコマベース要尺
            End If
        End Function

        '個別の加算長
        Function getAddEach(ByVal i As Integer) As Double
            If _isMyValue Then
                Return _addeach(i) * _parent.p_dマイひも長係数
            Else
                Return _addeach(i)
            End If
        End Function

        '縁の始末分
        Function getAddHem() As Double
            If _isMyValue Then
                Return _parent._d縁の垂直ひも長 * _parent.p_dマイひも長係数
            Else
                Return _parent._d縁の垂直ひも長
            End If
        End Function

        'ひも長加算(縦横端)
        Function getAddVert() As Double
            If _isMyValue Then
                Return _parent._dひも長加算_縦横端 * _parent.p_dマイひも長係数
            Else
                Return _parent._dひも長加算_縦横端
            End If
        End Function

        '加算長合計
        Function getAddSum(ByVal i As Integer) As Double
            If _isMyValue Then
                Return _addsum(i) * _parent.p_dマイひも長係数
            Else
                Return _addsum(i)
            End If
        End Function

        'コマからのひも長
        Function getBandLength(ByVal i As Integer) As Double
            If _isMyValue Then
                Return _bandlen(i) * _parent.p_dマイひも長係数
            Else
                Return _bandlen(i)
            End If
        End Function

        '反対方向との差
        Function getDiff(ByVal i As Integer) As Double
            If _isMyValue Then
                Return _diff(i) * _parent.p_dマイひも長係数
            Else
                Return _diff(i)
            End If
        End Function

        'コマからのひも長の差の文字列
        Function getDiffString(ByVal i As Integer, ByVal outp As clsOutput) As String
            'コマの{0}より
            Dim str As String = String.Format(My.Resources.CalcOutDiffFrom, getPairSideString(i))
            Dim diff As Double = getDiff(i)
            If mdlUnit.Abs(diff) < g_clsSelectBasics.p_d指定本幅(1) Then
                'コマの{0}と同じ長さ
                Return String.Format(My.Resources.CalcOutKnotSame, getPairSideString(i))

            ElseIf 0 < diff Then
                str += outp.outLengthTextWithUnit(diff)
                '長い
                str += My.Resources.CalcOutLong

            Else 'diff<0 Then
                str += outp.outLengthTextWithUnit(-diff)
                '短い
                str += My.Resources.CalcOutShort

            End If
            Return str

        End Function

        '折り位置からのひも長
        Function getFoldingLength(ByVal i As Integer) As Double
            If _isMyValue Then
                Return _foldinglen(i) * _parent.p_dマイひも長係数
            Else
                Return _foldinglen(i)
            End If
        End Function

        '折り位置前後の差
        Function getDiffFolding(ByVal i As Integer) As Double
            If _isMyValue Then
                Return _foldingdiff(i) * _parent.p_dマイひも長係数
            Else
                Return _foldingdiff(i)
            End If
        End Function

        '折り位置前後の差の文字列
        Function getDiffFoldingString(ByVal i As Integer, ByVal outp As clsOutput) As String
            '手前({0})が奥より
            Dim str As String = String.Format(My.Resources.CalcOutFront, _SideString(i))
            Dim diff As Double = getDiffFolding(i)
            If mdlUnit.Abs(diff) < g_clsSelectBasics.p_d指定本幅(1) Then
                '手前({0})と奥は同じ、真ん中で折る
                Return String.Format(My.Resources.CalcOutFoldingSame, _SideString(i))

            ElseIf 0 < diff Then
                str += outp.outLengthTextWithUnit(diff)
                '長い
                str += My.Resources.CalcOutLong

            Else 'diff<0 Then
                str += outp.outLengthTextWithUnit(-diff)
                '短い
                str += My.Resources.CalcOutShort

            End If
            Return str
        End Function

    End Class

    '開始位置の情報のセット
    Private Function setStartInfo() As CStartInfo
        Dim _i左から何番目 As Integer = _Data.p_row底_縦横.Value("f_i左から何番目")
        Dim _i上から何番目 As Integer = _Data.p_row底_縦横.Value("f_i上から何番目")

        If _i左から何番目 <= 0 OrElse _i横のコマ数 < _i左から何番目 OrElse
            _i上から何番目 <= 0 OrElse _i縦のコマ数 < _i上から何番目 Then
            Return Nothing '開始位置の対象外
        End If

        Dim startInfo As CStartInfo
        startInfo = New CStartInfo(Me, _i左から何番目, _i上から何番目)
        If startInfo.IsValid Then
            Return startInfo
        Else
            startInfo = Nothing
            Return Nothing
        End If
    End Function
#End Region


#Region "リスト出力"
    '「連続ひも長」フィールドに入力値を加算し係数倍した値をセットする(縁は除外)
    Private Function set側面_連続ひも長() As Boolean
        For Each row As tbl側面Row In _Data.p_tbl側面.Rows
            row.f_d連続ひも長 = (row.f_dひも長 + row.f_dひも長加算)
            If row.f_i番号 <> cHemNumber Then
                row.f_d連続ひも長 = row.f_d連続ひも長 * p_dマイひも長係数
            End If
        Next row

        _Data.p_tbl側面.AcceptChanges()
        Return True
    End Function

    '縦横展開DataTableの「出力ひも長」フィールドに入力値を加算し係数倍した値をセットする
    Private Function set側面_出力ひも長(ByVal tmptable As tbl縦横展開DataTable) As Boolean
        For Each row As tbl縦横展開Row In tmptable
            row.f_d出力ひも長 = (row.f_dひも長 + row.f_dひも長加算 + row.f_dひも長加算2) * p_dマイひも長係数
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
        Dim b展開区分 As Boolean = _Data.p_row底_縦横.Value("f_b展開区分")

        adjust_側面()
        set側面_連続ひも長()

        output.Clear()
        Dim row As tblOutputRow
        Dim order As String

        row = output.NextNewRow
        row.f_s本幅 = g_clsSelectBasics.p_i本幅
        row.f_s長さ = output.outLengthText(_dコマ間のすき間)
        row.f_s高さ = output.outLengthText(_dコマの寸法)
        row.f_sひも長 = output.outLengthText(_dコマの要尺)
        row.f_s編みかた名 = IO.Path.GetFileNameWithoutExtension(output.FilePath) 'あれば名前
        row.f_s編みひも名 = g_clsSelectBasics.p_s対象バンドの種類名
        row.f_s色 = _Data.p_row目標寸法.Value("f_s基本色")

        '底
        row = output.NextNewRow
        row.f_sカテゴリー = My.Resources.OutputTextBottom
        row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str

        '***底
        'このカテゴリーは先に行をつくる
        row = output.NextNewRow
        '横置き,縦置き
        For yokotate As Integer = 1 To 2
            Dim tmpTable As tbl縦横展開DataTable
            Dim sbMemo As New Text.StringBuilder

            If yokotate = 1 Then
                row.f_sタイプ = text横置き()
                tmpTable = set横展開DataTable(b展開区分)
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s横ひものメモ"))
            Else
                row.f_sタイプ = text縦置き()
                tmpTable = set縦展開DataTable(b展開区分)
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s縦ひものメモ"))
            End If
            If tmpTable Is Nothing OrElse tmpTable.Rows.Count = 0 Then
                Continue For
            End If
            'レコードあり

            '長い順に記号を振る
            set側面_出力ひも長(tmpTable)
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
                    If lasttmp.f_dひも長加算 + lasttmp.f_dひも長加算2 <> 0 Then
                        row.f_s高さ = output.outLengthText(lasttmp.f_dひも長加算 + lasttmp.f_dひも長加算2)
                    End If
                    row.f_sメモ = sbMemo.ToString
                    '
                    If contcount = 1 Then
                        row.f_s編みひも名 = String.Format("{0}", lasttmp.f_iひも番号)
                    Else
                        row.f_s編みひも名 = String.Format("{0} - {1}", lasttmp.f_iひも番号, lasttmp.f_iひも番号 + contcount - 1)
                    End If
                    Select Case lasttmp.f_sひも名
                        Case text横ひも()
                            row.f_s編みひも名 = String.Format("[{0}] {1}", p_i横ひもの本数, row.f_s編みひも名)
                            row.f_i段数 = _i横のコマ数 + 2 * (_i高さのコマ数 + _i折り返しコマ数)
                        Case text縦ひも()
                            row.f_s編みひも名 = String.Format("[{0}] {1}", p_i縦ひもの本数, row.f_s編みひも名)
                            row.f_i段数 = _i縦のコマ数 + 2 * (_i高さのコマ数 + _i折り返しコマ数)
                    End Select

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
            row.f_sカテゴリー = text縁の始末()
            row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_s高さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str

            order = "f_i番号 ASC , f_iひも番号 ASC"
            'リスト出力
            Dim lastname As String = ""
            For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, order)
                row = output.NextNewRow
                If r.f_i番号 = cHemNumber Then
                    row.f_sタイプ = text縁の始末()
                    row.f_s高さ = output.outLengthText(r.f_d高さ)
                Else
                    If b展開区分 Then
                        row.f_s番号 = r.f_iひも番号.ToString
                    End If
                    If 0 < r.f_dひも長加算 Then
                        row.f_s高さ = output.outLengthText(r.f_dひも長加算)
                    End If
                End If
                If lastname <> r.f_s編みひも名 Then
                    row.f_s編みかた名 = r.f_s編みかた名
                    row.f_s編みひも名 = r.f_s編みひも名
                    lastname = r.f_s編みひも名
                End If
                row.f_i段数 = r.f_i段数
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
        row.f_s本幅 = textコマ数()

        row = output.NextNewRow
        row.f_s色 = text横寸法()
        row.f_s本幅 = _i横のコマ数
        row.f_sひも本数 = output.outLengthText(p_dコマベース_横)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_横)
        row.f_sメモ = textひも長加算_縦横端()
        row.f_s長さ = output.outLengthText(_dひも長加算_縦横端)

        row = output.NextNewRow
        row.f_s色 = text縦寸法()
        row.f_s本幅 = _i縦のコマ数
        row.f_sひも本数 = output.outLengthText(p_dコマベース_縦)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_縦)
        row.f_sメモ = textひも長加算_側面()
        row.f_s長さ = output.outLengthText(_dひも長加算_側面)

        row = output.NextNewRow
        row.f_s色 = text高さ寸法()
        row.f_s本幅 = _i高さのコマ数
        row.f_sひも本数 = output.outLengthText(p_dコマベース_高さ)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_高さ)
        row.f_sメモ = textマイひも長係数()
        row.f_s長さ = p_dマイひも長係数

        output.AddBlankLine()


        '開始位置の情報をセット
        Dim startInfo As CStartInfo = setStartInfo()
        If startInfo IsNot Nothing Then
            '設定されていれば
            startInfo.setMyValue(True)

            '***開始位置
            row = output.NextNewRow
            row.f_sカテゴリー = text開始位置()

            '横ひも
            row = output.NextNewRow
            row.f_s記号 = startInfo.row横展開.f_s記号
            row.f_s色 = startInfo.row横展開.f_s色
            row.f_s本幅 = output.outLaneText(startInfo.row横展開.f_i何本幅)
            row.f_sひも長 = output.outLengthText(startInfo.row横展開.f_d出力ひも長)
            row.f_sタイプ = startInfo.row横展開.f_sひも名
            row.f_s編みかた名 = text左から() & startInfo.i左から何番目.ToString & text番目のコマ()
            row.f_s高さ = My.Resources.CalcOutFromFolding '折り位置から
            row.f_s長さ = My.Resources.CalcOutFromKnot 'コマから

            '左
            row = output.NextNewRow
            row.f_s編みかた名 = String.Format(My.Resources.CalcOutKnotOf, startInfo.getSideString(2)) '<コマの{0}> 
            row.f_s編みひも名 = startInfo.getDiffFoldingString(2, output)
            row.f_s高さ = output.outLengthText(startInfo.getFoldingLength(2))
            row.f_s長さ = output.outLengthText(startInfo.getBandLength(2))

            '右
            row = output.NextNewRow
            row.f_s編みかた名 = String.Format(My.Resources.CalcOutKnotOf, startInfo.getSideString(3)) '<コマの{0}> 
            row.f_s編みひも名 = startInfo.getDiffFoldingString(3, output)
            row.f_s高さ = output.outLengthText(startInfo.getFoldingLength(3))
            row.f_s長さ = output.outLengthText(startInfo.getBandLength(3))

            '縦ひも
            row = output.NextNewRow
            row.f_s記号 = startInfo.row縦展開.f_s記号
            row.f_s色 = startInfo.row縦展開.f_s色
            row.f_s本幅 = output.outLaneText(startInfo.row縦展開.f_i何本幅)
            row.f_sひも長 = output.outLengthText(startInfo.row縦展開.f_d出力ひも長)
            row.f_sタイプ = startInfo.row縦展開.f_sひも名
            row.f_s編みかた名 = text上から() & startInfo.i上から何番目.ToString & text番目のコマ()
            row.f_s高さ = My.Resources.CalcOutFromFolding '折り位置から
            row.f_s長さ = My.Resources.CalcOutFromKnot 'コマから

            '上
            row = output.NextNewRow
            row.f_s編みかた名 = String.Format(My.Resources.CalcOutKnotOf, startInfo.getSideString(0)) '<コマの{0}> 
            row.f_s編みひも名 = startInfo.getDiffFoldingString(0, output)
            row.f_s高さ = output.outLengthText(startInfo.getFoldingLength(0))
            row.f_s長さ = output.outLengthText(startInfo.getBandLength(0))

            '下
            row = output.NextNewRow
            row.f_s編みかた名 = String.Format(My.Resources.CalcOutKnotOf, startInfo.getSideString(1)) '<コマの{0}> 
            row.f_s編みひも名 = startInfo.getDiffFoldingString(1, output)
            row.f_s高さ = output.outLengthText(startInfo.getFoldingLength(1))
            row.f_s長さ = output.outLengthText(startInfo.getBandLength(1))

            output.AddBlankLine()
        End If


        '集計値
        output.OutSumList()
        output.OutCutList()

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

    Private Function textコマ数() As String
        Return _frmMain.tpageコマ数.Text
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

    Private Function text横のコマ数() As String
        Return _frmMain.lbl横のコマ数.Text
    End Function

    Private Function text横ひも() As String
        Return _frmMain.lbl横ひも.Text
    End Function

    Private Function text縦のコマ数() As String
        Return _frmMain.lbl縦のコマ数.Text
    End Function

    Private Function text縦ひも() As String
        Return _frmMain.lbl縦ひも.Text
    End Function

    Private Function text高さのコマ数() As String
        Return _frmMain.lbl高さのコマ.Text
    End Function

    Private Function text縁の始末() As String
        Return _frmMain.tpage側面と縁.Text
    End Function

    Private Function text巻きひも() As String
        'dgv追加品
        Return _frmMain.f_b巻きひも区分3.HeaderText
    End Function

    Private Function text編みかた名() As String
        Return _frmMain.lbl編みかた名_側面.Text
    End Function

    Private Function text付属品名() As String
        Return _frmMain.lbl付属品名.Text
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

    Private Function text個() As String
        Return _frmMain.lbl横のコマ数_単位.Text
    End Function

    Private Function text垂直ひも数() As String
        Return _frmMain.lbl垂直ひも数.Text
    End Function

    Private Function text開始位置() As String
        Return _frmMain.lbl開始位置.Text
    End Function

    Private Function text番目のコマ() As String
        Return _frmMain.lbl番目のコマ_l.Text
    End Function

    Private Function text左から() As String
        Return _frmMain.lbl左から.Text
    End Function

    Private Function text上から() As String
        Return _frmMain.lbl上から.Text
    End Function

    Private Function text四つ畳み編み() As String
        Return String.Format(My.Resources.FormCaption, "")
    End Function

    Private Function text高さのコマ() As String
        Return _frmMain.lbl高さのコマ.Text
    End Function

    Private Function text折り返しのコマ() As String
        Return _frmMain.lbl折り返しのコマ.Text
    End Function

    Private Function textコマ() As String
        Return _frmMain.lblコマ.Text
    End Function

    Private Function textひも長加算_縦横端() As String
        Return _frmMain.lblひも長加算_縦横端.Text
    End Function

    Private Function textひも長加算_側面() As String
        Return _frmMain.lblひも長加算_側面.Text
    End Function

    Private Function textマイひも長係数() As String
        Dim tmp As New frmSelectBand
        Return tmp.lblマイひも長係数.Text
    End Function

#End Region

End Class
