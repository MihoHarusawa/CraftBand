

Imports System.Reflection
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstOutput

Class clsCalcSquare45
    Const PAI As Double = 3.1416
    Const ROOT2 As Double = 1.41421356237

    '横と縦の展開
    Enum emExp
        _Yoko = 0
        _Tate = 1
    End Enum
    Const cExpandCount As Integer = 2


    '処理のカテゴリー
    Public Enum CalcCategory
        None

        NewData 'データ変更
        BsMaster  '基本値/マスター/バンド選択

        Target    '目標寸法(基本のひも幅以外)
        Target_Band   '基本のひも幅

        Square_TateYokoGap 'ひも間のすき間・横の四角数・縦の四角数・補強ひも
        Square_Add  'ひも長係数・ひも長加算
        Square_Vert '高さの四角数
        Square_Expand '縦横展開

        Edge  '縁の始末 
        Options  '追加品
        UpDown  '編み目

        Expand_Yoko '横ひも展開のセル編集
        Expand_Tate '縦ひも展開のセル編集

        FixLength '相互参照値のFix
        BandColor   '色の変更
    End Enum

    Public Property p_b有効 As Boolean
    Public Property p_sメッセージ As String 'p_b有効でない場合のみ参照
    Public Property p_s警告 As String '位置と長さ計算

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
    Private Property _b縦横を展開する As Boolean


    '縁の始末
    Private Property _d縁の高さ As Double '合計値,ゼロ以上
    Private Property _d縁の垂直ひも長 As Double '合計値,ゼロ以上
    Private Property _d縁の厚さ As Double '厚さの最大値,ゼロ以上

    '初期化
    Public Sub Clear()
        p_b有効 = False
        p_sメッセージ = Nothing
        p_s警告 = Nothing

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
        _b縦横を展開する = False

        _d縁の高さ = 0
        _d縁の垂直ひも長 = 0
        _d縁の厚さ = 0

        _tbl縦横展開(emExp._Yoko).Clear()
        _tbl縦横展開(emExp._Tate).Clear()

        'clsCalcSquare45Image.vb
        _BandPositions(emExp._Yoko).Clear()
        _BandPositions(emExp._Tate).Clear()
        _a底領域.Empty()

    End Sub

#Region "プロパティ値"

    ReadOnly Property p_dひも幅の一辺 As Double
        Get
            Return _dひも幅の一辺
        End Get
    End Property

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

    ReadOnly Property p_i高さの切上四角数 As Integer
        Get
            Return CInt(Math.Ceiling(_d高さの四角数))
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
            'Return _d四角の対角線 * _i横の四角数
            If p_b長方形である Then
                Return p_d底の横長
            End If
            Return -1
        End Get
    End Property

    ReadOnly Property p_d四角ベース_縦 As Double
        Get
            'Return _d四角の対角線 * _i縦の四角数
            If p_b長方形である Then
                Return p_d底の縦長
            End If
            Return -1
        End Get
    End Property

    ReadOnly Property p_d四角ベース_高さ As Double
        Get
            Return _d四角の対角線 * _d高さの四角数
        End Get
    End Property

    ReadOnly Property p_d四角ベース_周 As Double
        Get
            If p_d四角ベース_横 < 0 OrElse p_d四角ベース_縦 < 0 Then
                Return -1
            End If
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

    ReadOnly Property p_d縁の高さ As Double
        Get
            Return _d縁の高さ
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


    '配置の形状
    ReadOnly Property p_b横の四角数が縦以上 As Boolean   'Trueの時右上がり, 縦横同じもtrue
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

    ReadOnly Property p_i縦横四角数の小さい方 As Integer '縦横同じならその四角数
        Get
            If _i縦の四角数 < _i横の四角数 Then
                Return _i縦の四角数
            Else
                Return _i横の四角数
            End If
        End Get
    End Property

    'ひも数として縦横同じに扱う場合
    ReadOnly Property p_iひもの本数 As Integer
        Get
            Return _i横の四角数 + _i縦の四角数
        End Get
    End Property

    ReadOnly Property p_i位置番号(ByVal iひも番号 As Integer) As Integer
        Get
            If iひも番号 < 1 Then
                Return -9999    'ないはず
            ElseIf p_iひもの本数 < iひも番号 Then
                Return 9999 'ないはず
            ElseIf iひも番号 <= p_i縦横四角数の小さい方 Then
                Return iひも番号 - p_i縦横四角数の小さい方 - 1
            ElseIf iひも番号 <= (p_i縦横四角数の小さい方 + p_i縦横四角数の差) Then
                Return 0
            Else
                Return iひも番号 - (p_i縦横四角数の小さい方 + p_i縦横四角数の差)
            End If
        End Get
    End Property

    '追加品の参照値 #63
    Function getAddPartsRefValues() As Double()
        Dim values(12) As Double
        values(0) = 1 'すべて有効

        '(四角ベース)横・縦・高さ・周
        values(1) = p_d四角ベース_横 'p_s四角ベース_横
        values(2) = p_d四角ベース_縦 'p_s四角ベース_縦
        values(3) = p_d四角ベース_高さ 'p_s四角ベース_高さ
        values(4) = p_d四角ベース_周 'p_s四角ベース_周
        '(縁厚さプラス)横・縦・高さ・周
        values(5) = p_d縁厚さプラス_横 'p_s縁厚さプラス_横
        values(6) = p_d縁厚さプラス_縦 'p_s縁厚さプラス_縦
        values(7) = p_d縁厚さプラス_高さ 'p_s縁厚さプラス_高さ
        values(8) = p_d縁厚さプラス_周 'p_s縁厚さプラス_周
        '目標寸法/横・縦・高さ・基本のひも幅
        values(9) = _d横_目標 '目標寸法/横
        values(10) = _d縦_目標 '目標寸法/縦
        values(11) = _d高さ_目標 '目標寸法/高さ
        values(12) = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅) '基本のひも幅

        Return values
    End Function


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

        sb.AppendLine("_BandPositions:_Yoko")
        sb.AppendLine(_BandPositions(emExp._Yoko).ToString)
        sb.AppendLine("_BandPositions:_Tate")
        sb.AppendLine(_BandPositions(emExp._Tate).ToString)

        Return sb.ToString
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("{0}({1}){2}", Me.GetType.Name, IIf(p_b有効, "Valid", "InValid"), p_sメッセージ).AppendLine()
        sb.AppendFormat("Target:({0},{1},{2}) Lane({3})", _d横_目標, _d縦_目標, _d高さ_目標, _I基本のひも幅).AppendLine()
        sb.AppendFormat("Unit: band({0:f3}/{1:f3}) square({2:f3}/{3:f3})", _dひも幅の一辺, _dひも幅の対角線, _d四角の一辺, _d四角の対角線).AppendLine()
        sb.AppendFormat("Square: W({0:f3}, {1:f3}) D({2:f3}, {3:f3}) H({4:f3}, {5:f3}) ", _i横の四角数, p_d四角ベース_横, _i縦の四角数, p_d四角ベース_縦, _d高さの四角数, p_d四角ベース_高さ).AppendLine()
        sb.AppendFormat("Edge:H({0}) VerticalLength({1}) Thickness({2})", _d縁の高さ, _d縁の垂直ひも長, _d縁の厚さ).AppendLine()

        sb.AppendFormat("WidthChange({0}) IsRectangle({1}) {2}", p_b本幅変更あり, p_b長方形である, _a底領域).AppendLine()
        sb.AppendFormat("Up{0:f3}° Left{1:f3}° Bottom{2:f3}° Right{3:f3}°",
                        p_d底の角度(DirectionEnum._上), p_d底の角度(DirectionEnum._左),
                        p_d底の角度(DirectionEnum._下), p_d底の角度(DirectionEnum._右)).AppendLine()
        sb.AppendFormat("Bottom H:{0:f3} {1:f3}  D:{2:f3} {3:f3}", p_d底の横長, p_d底の横長(True), p_d底の縦長, p_d底の縦長(True))

        Return sb.ToString
    End Function

#End Region



    Friend _Data As clsDataTables
    Friend _frmMain As frmMain

    Sub New(ByVal data As clsDataTables, ByVal frm As frmMain)
        _Data = data
        _frmMain = frm

        '最初に1点だけ作っておく。以降は存在が前提
        _tbl縦横展開(emExp._Yoko) = New tbl縦横展開DataTable
        _tbl縦横展開(emExp._Tate) = New tbl縦横展開DataTable

        'clsCalcSquare45Image
        _BandPositions(emExp._Yoko) = New CBandPositionList(emExp._Yoko)
        _BandPositions(emExp._Tate) = New CBandPositionList(emExp._Tate)

        Clear()
    End Sub

    Function CalcSize(ByVal category As CalcCategory, ByVal ctr As Object, ByVal key As Object) As Boolean
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CalcSize {0} {1} {2}", category, ctr, key)

        p_s警告 = Nothing
        Dim ret As Boolean = True
        Select Case category
            Case CalcCategory.NewData, CalcCategory.BsMaster
                'データ変更/基本値/マスター/バンド選択
                Clear()
                ret = ret And set_目標寸法(False)
                ret = ret And set_四角数()
                ret = ret And calc_側面(category, Nothing, Nothing) '縁の始末
                ret = ret And renew_横ひも展開(category)
                ret = ret And renew_縦ひも展開(category)
                ret = ret AndAlso calc_位置と長さ計算(True)
                ret = ret And calc_側面(category, Nothing, Nothing) '周長
                If ret Then
                    p_sメッセージ = _frmMain.editAddParts.SetRefValueAndCheckError(_Data, getAddPartsRefValues)
                    If Not String.IsNullOrEmpty(p_sメッセージ) Then
                        ret = False
                    End If
                End If

            Case CalcCategory.Target    '目標寸法(※基本のひも幅以外)
                ret = set_目標寸法(False)
                '(差の表示が変わるだけ)

            Case CalcCategory.Target_Band    '基本のひも幅
                ret = set_目標寸法(False)
                ret = ret And renew_横ひも展開(category)
                ret = ret And renew_縦ひも展開(category)
                ret = ret AndAlso calc_位置と長さ計算(True)
                ret = ret And calc_側面(category, Nothing, Nothing) '周長

        '四角数のタブ内
            Case CalcCategory.Square_Add     'ひも長係数,ひも長加算
                ret = ret And set_四角数()
                ret = ret AndAlso calc_位置と長さ計算(False)
                ret = ret And calc_側面(category, Nothing, Nothing) '周長

            Case CalcCategory.Square_TateYokoGap 'ひも間のすき間・横の四角数・縦の四角数
                ret = ret And set_四角数()
                ret = ret And renew_横ひも展開(category)
                ret = ret And renew_縦ひも展開(category)
                ret = ret AndAlso calc_位置と長さ計算(True)
                ret = ret And calc_側面(category, Nothing, Nothing) '周長

            Case CalcCategory.Square_Vert '四角数タブ高さ
                ret = ret And set_四角数()
                ret = ret AndAlso calc_位置と長さ計算(False)
                ret = ret And calc_側面(category, Nothing, Nothing) '周長

            Case CalcCategory.Square_Expand '四角数タブ縦横展開
                ret = ret And set_四角数()
                ret = ret And renew_横ひも展開(category)
                ret = ret And renew_縦ひも展開(category)
                ret = ret AndAlso calc_位置と長さ計算(True)
                ret = ret And calc_側面(category, Nothing, Nothing) '周長

        '以下、row指定がある場合は、そのタブが表示された状態
            Case CalcCategory.Edge  '縁の始末
                Dim row As tbl側面Row = CType(ctr, tbl側面Row)
                ret = ret And calc_側面(category, row, key)
                ret = ret AndAlso calc_位置と長さ計算(False) '縁

            Case CalcCategory.UpDown 'ひも上下
                '(ひも上下は計算寸法変更なし)

            Case CalcCategory.Options  '追加品
                'エラーメッセージ通知
                p_sメッセージ = ctr
                If Not String.IsNullOrEmpty(p_sメッセージ) Then
                    ret = False
                End If

            Case CalcCategory.Expand_Yoko  '横ひも展開のセル編集
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                If adjust_ひも(row, key) Then
                    ret = ret And calc_位置と長さ計算(True)
                    ret = ret And calc_側面(category, Nothing, Nothing) '周長
                End If

            Case CalcCategory.Expand_Tate  '縦ひも展開のセル編集
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                If adjust_ひも(row, key) Then
                    ret = ret And calc_位置と長さ計算(True)
                    ret = ret And calc_側面(category, Nothing, Nothing) '周長
                End If

            Case CalcCategory.BandColor '色の変更
                ret = ret And renew_横ひも展開(category)
                ret = ret And renew_縦ひも展開(category)
                ret = ret AndAlso calc_位置と長さ計算(True)
                ret = ret And calc_側面(category, Nothing, Nothing) '周長

            Case CalcCategory.FixLength '相互参照値のFix(1Pass値は得られている前提)
                If ret Then
                    p_sメッセージ = _frmMain.editAddParts.SetRefValueAndCheckError(_Data, getAddPartsRefValues)
                    If Not String.IsNullOrEmpty(p_sメッセージ) Then
                        ret = False
                    End If
                End If

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
            _b縦横を展開する = .Value("f_b展開区分")
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
        _Data.p_row底_縦横.Value("f_b展開区分") = False
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
        groupRow.SetNameIndexValue("f_b集計対象外区分", grpMst, "f_b集計対象外区分初期値")

        For Each drow As clsDataRow In groupRow
            Dim mst As New clsOptionDataRow(grpMst.IndexDataRow(drow)) '必ずある
            drow.Value("f_i何本幅") = mst.GetFirstLane(i何本幅)
        Next
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Side Add: {0}", groupRow.ToString)
        Return True
    End Function

    '更新処理が必要なフィールド名
    'ひも長加算はリスト出力時に処理する
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
            If p_d四角ベース_周 < 0 Then
                drow.Value("f_d周長") = 0
            Else
                drow.Value("f_d周長") = p_d四角ベース_周 * drow.Value("f_d周長比率対底の周")
            End If
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
                    '点対称位置
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
                    '点対称位置
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

#Region "横ひも・縦ひも展開"

    'New時に作成、以降は存在が前提
    '入力変更イベントで内容を更新し、常に最新の状態を保持する
    Dim _tbl縦横展開(cExpandCount - 1) As tbl縦横展開DataTable

    '縦・横の区別
    Dim _処理のひも種() As enumひも種 = {enumひも種.i_横, enumひも種.i_縦}
    Dim _補強のひも種() As enumひも種 = {
        enumひも種.i_45度 Or enumひも種.i_補強,
        enumひも種.i_315度 Or enumひも種.i_補強}
    Dim _保存のひも種() As enumひも種 = {
        enumひも種.i_横 Or enumひも種.i_45度,
        enumひも種.i_縦 Or enumひも種.i_315度}

    Dim _補強フィールド名() As String = {"f_b補強ひも区分", "f_b始末ひも区分"}


    '各展開テーブルに含まれるのは2択(処理のひも種/補強のひも種)
    Private Shared Function is補強ひも(ByVal iひも種 As enumひも種) As Boolean
        Return iひも種.HasFlag(enumひも種.i_補強)
    End Function
    Private Shared Function is補強ひも(ByVal row As tbl縦横展開Row) As Boolean
        Return is補強ひも(row.f_iひも種)
    End Function


    '横ひもの_tbl縦横展開 を取得。isReset=Trueでリセット
    Function get横展開DataTable(Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        Return get展開DataTable(emExp._Yoko, isReset)
    End Function

    '縦ひもの_tbl縦横展開 を取得。isReset=Trueでリセット
    Function get縦展開DataTable(Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        Return get展開DataTable(emExp._Tate, isReset)
    End Function

    Private Function get展開DataTable(ByVal dir As emExp, ByVal isReset As Boolean) As tbl縦横展開DataTable
        If isReset Then
            _Data.Removeひも種Rows(_保存のひも種(dir))
            renew展開DataTable(dir, False) '既存反映なし・サイズは同じ
            calc_位置と長さ計算(True)
        End If
        Return _tbl縦横展開(dir)
    End Function


    '横ひもの編集完了,_tbl縦横展開_横ひもを_Dataに反映
    Function save横展開DataTable(Optional ByVal always As Boolean = False) As Boolean
        Return save展開DataTable(emExp._Yoko, always)
    End Function

    '縦ひもの編集完了,_tbl縦横展開_縦ひもを_Dataに反映
    Function save縦展開DataTable(Optional ByVal always As Boolean = False) As Boolean
        Return save展開DataTable(emExp._Tate, always)
    End Function

    Private Function save展開DataTable(ByVal dir As emExp, ByVal always As Boolean) As Boolean
        Try
            If always OrElse _tbl縦横展開(dir).GetChanges IsNot Nothing Then
                Dim change As Integer = _Data.FromTmpTable(_保存のひも種(dir), _tbl縦横展開(dir))
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "save展開DataTable", dir)
            Return False
        End Try
    End Function


    '縦横を展開したtbl縦横展開DataTableのレコードの初期化(なければ作成)
    Function prepare縦横展開DataTable() As Boolean
        Try
            For Each dir As emExp In [Enum].GetValues(GetType(emExp))
                Dim table As tbl縦横展開DataTable = _tbl縦横展開(dir)
                _Data.FromTmpTable(_保存のひも種(dir), table)
            Next
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "prepare縦横展開DataTable")
            Return False
        End Try
    End Function


    'CalcSizeからの呼び出し 
    'IN:_b縦横側面を展開する
    'OUT:_tbl縦横展開(横ひも)の縦横四角数に合わせたサイズ確定・長さは未セット
    Function renew_横ひも展開(ByVal category As CalcCategory) As Boolean
        Return renew展開DataTable(emExp._Yoko, _b縦横を展開する)
    End Function


    'CalcSizeからの呼び出し 
    'IN:_b縦横側面を展開する
    'OUT:_tbl縦横展開(縦ひも)の縦横四角数に合わせたサイズ確定・長さは未セット
    Function renew_縦ひも展開(ByVal category As CalcCategory) As Boolean
        Return renew展開DataTable(emExp._Tate, _b縦横を展開する)
    End Function


    '指定レコードの幅と出力ひも長を計算
    'IN:.f_d長さ  _dひも間のすき間,_d高さの四角数,_dひも長係数,_dひも長加算,_d縁の垂直ひも長
    'OUT:.f_dひも長,.f_d出力ひも長
    'フィールド名指定がある場合は、再計算が必要な時Trueを返す
    Function adjust_ひも(ByVal row As tbl縦横展開Row, ByVal dataPropertyName As String) As Boolean
        Dim isNeedRecalc As Boolean = False
        'セル編集操作時
        If Not String.IsNullOrEmpty(dataPropertyName) Then
            Select Case dataPropertyName
                Case "f_s色"
                    Return False
                Case "f_dひも長加算2"
                    row.f_dひも長加算2 = 0 '使わない
                    Return False
                Case "f_dひも長加算"
                    '再計算は不要
                Case "f_i何本幅"
                    '長さの再計算
                    isNeedRecalc = True
            End Select
        End If

        '本幅の変更、もしくは再計算
        'f_dひも長 :四角の一辺(角から出る分)と高さ*2 をプラス
        'f_d出力ひも長 :ひも長に係数をかけて、+2*(ひも長加算(一端)+縁の垂直ひも長)+ひも長加算
        With row
            .f_d幅 = g_clsSelectBasics.p_d指定本幅(.f_i何本幅) + _dひも間のすき間
            If is補強ひも(row) Then
                '補強ひも
                .f_dひも長 = .f_d長さ
                .f_d出力ひも長 = .f_dひも長 + .f_dひも長加算
            Else
                '通常のひも ※高さ分は固定:高さの四角数 * _d四角の一辺
                .f_dひも長 = .f_d長さ +
                    g_clsSelectBasics.p_d指定本幅(.f_i何本幅) + _dひも間のすき間 +
                    4 * _d高さの四角数 * _d四角の一辺
                .f_d出力ひも長 = .f_dひも長 * _dひも長係数 + .f_dひも長加算 +
                    (_dひも長加算 + _d縁の垂直ひも長) * 2
            End If
        End With
        Return isNeedRecalc
    End Function

    '長さ確定後のひも長・出力ひも長計算
    Function adjust_ひも(ByVal table As tbl縦横展開DataTable) As Boolean
        For Each row As tbl縦横展開Row In table
            adjust_ひも(row, Nothing)
        Next
        Return True
    End Function


    '展開テーブルを作り直す レコード数増減＆Fix
    '固定: f_iひも種,f_iひも番号,f_i位置番号,f_sひも名,[f_i何本幅,f_d幅]
    '要計算:f_d長さ
    '入力値:f_s色,f_dひも長加算,f_i何本幅,f_dひも長加算2(使わない)
    '入力・計算後に再セット:f_d幅,f_dひも長,f_d出力ひも長
    Function renew展開DataTable(ByVal dir As emExp, ByVal isRefSaved As Boolean) As Boolean
        Dim table As tbl縦横展開DataTable = _tbl縦横展開(dir)

        If Not isRefSaved Then
            table.Clear()
        End If
        table.AcceptChanges()

        Dim row As tbl縦横展開Row

        SetMaxひも番号(table, p_iひもの本数)

        'ひも分のレコード作成 (セットする以外は初期値)
        For idx As Integer = 1 To p_iひもの本数
            row = Find縦横展開Row(table, _処理のひも種(dir), idx, True)

            row.f_i位置番号 = p_i位置番号(idx)
            If dir = emExp._Yoko Then
                row.f_sひも名 = text横ひも()
            ElseIf dir = emExp._Tate Then
                row.f_sひも名 = text縦ひも()
            End If
            row.f_i何本幅 = _I基本のひも幅   '保存対象
            row.f_d幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _dひも間のすき間

        Next
        '以降は裏側
        Dim pos As Integer = cBackPosition
        If _Data.p_row底_縦横.Value(_補強フィールド名(dir)) Then
            For idx As Integer = 1 To 2
                row = Find縦横展開Row(table, _補強のひも種(dir), idx, True)

                row.f_i位置番号 = pos
                row.f_sひも名 = text縦の補強ひも()
                If dir = emExp._Yoko Then
                    row.f_sひも名 = text横の補強ひも()
                ElseIf dir = emExp._Tate Then
                    row.f_sひも名 = text縦の補強ひも()
                End If
                row.f_i何本幅 = _I基本のひも幅

                pos += 1
            Next
        Else
            row = Find縦横展開Row(table, _補強のひも種(dir), 1, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
            row = Find縦横展開Row(table, _補強のひも種(dir), 2, False)
            If row IsNot Nothing Then
                row.Delete()
            End If
        End If
        table.AcceptChanges()

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(_保存のひも種(dir), table)
        End If

        Return _BandPositions(dir).SetTable(table, p_iひもの本数)
    End Function


#End Region

#Region "リスト出力"

    'リスト生成
    Public Function CalcOutput(ByVal output As clsOutput) As Boolean
        If output Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If
        If Not CalcSize(CalcCategory.FixLength, Nothing, Nothing) Then
            Return False
        End If

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
                tmpTable = get横展開DataTable() '直前のCalcSizeの結果
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s横ひものメモ"))
            Else
                row.f_sタイプ = text縦置き()
                tmpTable = get縦展開DataTable() '直前のCalcSizeの結果
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
                    If 0 <= r.f_dひも長 OrElse 0 <= r.f_dひも長加算 Then
                        If Not r.f_b集計対象外区分 Then
                            r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_dひも長 + r.f_dひも長加算, r.f_s色)
                        Else
                            r.f_s記号 = ""
                            output.SetBandRowNoMark(r.f_iひも本数, r.f_i何本幅, r.f_dひも長 + r.f_dひも長加算, r.f_s色)
                        End If
                    End If

                End If
                row.f_sメモ = r.f_sメモ
            Next

            output.AddBlankLine()
        End If

        '***追加品
        output.OutAddParts(_frmMain.editAddParts)

        '***計算寸法
        row = output.NextNewRow
        row.f_sカテゴリー = text計算寸法()
        row.f_sひも本数 = text内側()
        row.f_sひも長 = text外側()
        row.f_s本幅 = text四角数()

        row = output.NextNewRow
        row.f_s色 = text横寸法()
        row.f_s本幅 = _i横の四角数
        If 0 <= p_d四角ベース_横 Then
            row.f_sひも本数 = output.outLengthText(p_d四角ベース_横)
            row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_横)
        Else
            row.f_s編みかた名 = output.outLengthText(p_d底の横長)
            row.f_s編みひも名 = output.outLengthText(p_d底の横長(True))
            row.f_i周数 = p_d底の角度(DirectionEnum._上)
            row.f_i段数 = p_d底の角度(DirectionEnum._左)
        End If

        row = output.NextNewRow
        row.f_s色 = text縦寸法()
        row.f_s本幅 = _i縦の四角数
        If 0 <= p_d四角ベース_縦 Then
            row.f_sひも本数 = output.outLengthText(p_d四角ベース_縦)
            row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_縦)
        Else
            row.f_s編みかた名 = output.outLengthText(p_d底の縦長)
            row.f_s編みひも名 = output.outLengthText(p_d底の縦長(True))
            row.f_i周数 = p_d底の角度(DirectionEnum._下)
            row.f_i段数 = p_d底の角度(DirectionEnum._右)
        End If

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
