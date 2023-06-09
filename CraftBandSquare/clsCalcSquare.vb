

Imports System.Reflection
Imports System.Windows.Forms.AxHost
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.mdlUnit
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstOutput

Class clsCalcSquare
    Public Const ROOT2 As Double = 1.41421356237

    '処理のカテゴリー
    Public Enum CalcCategory
        None

        NewData 'データ変更
        BsMaster  '基本値/マスター/バンド選択

        '目標寸法
        Target    '目標寸法
        Target_Band '基本のひも幅

        '四角数
        Square_Gap  '目(ひも間のすき間)
        Square_Yoko '横の長さ
        Square_Tate '縦の長さ
        Square_Vert '高さ
        Square_Expand '縦横展開

        'レコード変更/再計算
        SideEdge  '側面と縁
        Inserted '差しひも 
        UpDown  '編み目
        Options  '追加品
        Expand_Yoko '横ひも展開
        Expand_Tate '縦ひも展開

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

    '四角数キャッシュ
    Private Property _i横の目の数 As Integer '縦ひも数=この数+1
    Private Property _i縦の目の数 As Integer '横ひも数=この数+1
    Private Property _i高さの目の数 As Integer '=側面の編みひも数
    Private Property _d左端右端の目 As Double '0～1
    Private Property _d上端下端の目 As Double '0～1
    Private Property _d最下段の目 As Double '0～1

    Private Property _d目_ひも間のすき間 As Double
    Private Property _dひも長係数 As Double
    Private Property _dひも長加算_縦横端 As Double
    Private Property _dひも長加算_側面 As Double

    Private Property _b縦横側面を展開する As Boolean


    '計算結果の保持
    Private Property _d四角ベース_横計 As Double '横の目数,左右の目,縦ひも幅計,すき間
    Private Property _d四角ベース_縦計 As Double '縦の目数,上下の目,横ひも幅計,すき間
    Private Property _d四角ベース_高さ計 As Double '高さの目数,最下段の目,編みひも幅計,すき間　(縁は含まない)
    '横ひも: _tbl縦横展開_横ひも
    Private Property _b横ひも本幅変更 As Boolean
    '縦ひも: _tbl縦横展開_縦ひも
    Private Property _b縦ひも本幅変更 As Boolean


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

        _i横の目の数 = -1
        _i縦の目の数 = -1
        _i高さの目の数 = -1
        _d左端右端の目 = -1
        _d上端下端の目 = -1
        _d最下段の目 = -1

        _d目_ひも間のすき間 = -1
        _dひも長係数 = -1
        _dひも長加算_縦横端 = -1
        _dひも長加算_側面 = -1

        _b縦横側面を展開する = False
        _b横ひも本幅変更 = False
        _b縦ひも本幅変更 = False
        _b側面ひも本幅変更 = False

        _d四角ベース_横計 = -1
        _d四角ベース_縦計 = -1
        _d四角ベース_高さ計 = -1

        _d縁の高さ = 0
        _d縁の垂直ひも長 = 0
        _d縁の厚さ = 0
    End Sub

#Region "プロパティ値"
    Public ReadOnly Property p_i縦ひもの本数 As Integer
        Get
            Return _i横の目の数 + 1
        End Get
    End Property

    Public ReadOnly Property p_i横ひもの本数 As Integer
        Get
            Return _i縦の目の数 + 1
        End Get
    End Property

    Public ReadOnly Property p_i側面ひもの本数 As Integer
        Get
            Return _i高さの目の数
        End Get
    End Property

    'ひも幅+すき間
    Public ReadOnly Property p_d縦横_四角 As Double
        Get
            If 0 < _d目_ひも間のすき間 AndAlso 0 < _d基本のひも幅 Then
                Return _d目_ひも間のすき間 + _d基本のひも幅
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

    Public ReadOnly Property p_d四角ベース_横 As Double '底の長さとして使用
        Get
            Return _d四角ベース_横計
        End Get
    End Property
    Public ReadOnly Property p_d四角ベース_縦 As Double '底の長さとして使用
        Get
            Return _d四角ベース_縦計
        End Get
    End Property
    Public ReadOnly Property p_d四角ベース_高さ As Double
        Get
            Return _d四角ベース_高さ計
        End Get
    End Property

    ReadOnly Property p_d四角ベース_周 As Double '編みひも長計算に使用
        Get
            If 0 < p_d四角ベース_横 AndAlso 0 < p_d四角ベース_縦 Then
                Return 2 * (p_d四角ベース_横 + p_d四角ベース_縦) _
                + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分")
            Else
                Return 0
            End If
        End Get
    End Property

    ReadOnly Property p_d四角ベース_周の横 As Double '横ひも長計算に使用
        Get
            Return _d四角ベース_横計 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4
        End Get
    End Property

    ReadOnly Property p_d四角ベース_周の縦 As Double '縦ひも長計算に使用
        Get
            Return _d四角ベース_縦計 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4
        End Get
    End Property

#Region "表示用・計算には使わない値"

    Public ReadOnly Property p_d縁厚さプラス_周 As Double
        Get
            If 0 < p_d四角ベース_周 Then
                Return p_d四角ベース_周 + 8 * p_d厚さ
            End If
            Return 0
        End Get
    End Property


    '表示文字列
    Public ReadOnly Property p_s縦横_目 As String
        Get
            If 0 < _d目_ひも間のすき間 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d目_ひも間のすき間)
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
            If 0 < _d目_ひも間のすき間 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d目_ひも間のすき間 * ROOT2)
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

    Public ReadOnly Property p_d縁厚さプラス_横 As Double
        Get
            If 0 < p_d四角ベース_横 Then
                Return p_d四角ベース_横 + p_d厚さ * 2
            End If
            Return 0
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

    Public ReadOnly Property p_d縁厚さプラス_縦 As Double
        Get
            If 0 < p_d四角ベース_縦 Then
                Return p_d四角ベース_縦 + p_d厚さ * 2
            End If
            Return 0
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

    Public ReadOnly Property p_d縁厚さプラス_高さ As Double
        Get
            Return _d四角ベース_高さ計 + _d縁の高さ +
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
            If (0 < _d横_目標) AndAlso (0 < _d四角ベース_横計) Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d四角ベース_横計 - _d横_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s縦寸法の差 As String
        Get
            If (0 < _d縦_目標) AndAlso (0 < _d四角ベース_縦計) Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d四角ベース_縦計 - _d縦_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s高さ寸法の差 As String
        Get
            If (0 <= _d高さ_目標) AndAlso 0 <= _d四角ベース_高さ計 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d四角ベース_高さ計 - _d高さ_目標, True)
            End If
            Return ""
        End Get
    End Property
#End Region

    Public ReadOnly Property p_i垂直ひも数 As Integer
        Get
            If 0 < _i横の目の数 AndAlso 0 < _i縦の目の数 Then
                Return 2 * ((_i横の目の数 + 1) + (_i縦の目の数 + 1))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property p_b横ひも本幅変更 As Boolean
        Get
            Return _b横ひも本幅変更
        End Get
    End Property
    Public ReadOnly Property p_b縦ひも本幅変更 As Boolean
        Get
            Return _b縦ひも本幅変更
        End Get
    End Property
    Public ReadOnly Property p_b側面ひも本幅変更 As Boolean
        Get
            Return _b側面ひも本幅変更
        End Get
    End Property

    '底の角(縦ひもと横ひものクロス位置)の外に有効な目がある
    Public ReadOnly Property p_i底の斜めの目の数 As Integer
        Get
            If 1 <= _d左端右端の目 + _d上端下端の目 Then
                Return _i縦の目の数 + _i横の目の数 + 1
            Else
                Return _i縦の目の数 + _i横の目の数 - 1
            End If
        End Get
    End Property

    '縦の目数として処理する数
    Public ReadOnly Property p_i縦の目の実質数 As Integer
        Get
            If 1 <= _d上端下端の目 Then
                Return _i縦の目の数 + 2
            Else
                Return _i縦の目の数
            End If
        End Get
    End Property

    '横の目数として処理する数
    Public ReadOnly Property p_i横の目の実質数 As Integer
        Get
            If 1 <= _d左端右端の目 Then
                Return _i横の目の数 + 2
            Else
                Return _i横の目の数
            End If
        End Get
    End Property

    '高さの目数として処理する数
    Public ReadOnly Property p_i高さの目の実質数 As Integer
        Get
            If 1 <= _d最下段の目 Then
                Return _i高さの目の数 + 1
            Else
                Return _i高さの目の数
            End If
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
        sb.AppendFormat("Width:({0},{1})", _i横の目の数, _d四角ベース_横計).AppendLine()
        sb.AppendFormat("Depth:({0},{1}) Periphery:{2}", _i縦の目の数, _d四角ベース_縦計, p_d四角ベース_周).AppendLine()
        sb.AppendFormat("Height:({0},{1})", _i高さの目の数, _d四角ベース_高さ計).AppendLine()
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
        _tbl縦横展開_横ひも = New tbl縦横展開DataTable
        _tbl縦横展開_縦ひも = New tbl縦横展開DataTable

        _CUpDown = New clsUpDown(False)
        _CUpDown.Clear(True) 'テーブル作成

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
                '
                ret = ret And calc_側面と縁(category, Nothing, Nothing)     '高さ系
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)     '縦計
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)    '横計

                ret = ret And calc_差しひも(category, Nothing, Nothing)
                ret = ret And calc_追加品(category, Nothing, Nothing)

            Case CalcCategory.Target    '目標寸法(※基本のひも幅以外)
                ret = ret And set_目標寸法(False)
                '(差の表示が変わるだけ)

            Case CalcCategory.Target_Band     '基本のひも幅
                ret = ret And set_目標寸法(False)
                '
                ret = ret And calc_側面と縁(category, Nothing, Nothing)
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)

        '四角数のタブ内
            Case CalcCategory.Square_Gap     '目(ひも間のすき間),ひも長係数,ひも長加算
                ret = ret And set_四角数()
                '
                ret = ret And calc_側面と縁(category, Nothing, Nothing)
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)

            Case CalcCategory.Square_Yoko '四角数タブ横の目の数
                ret = ret And set_四角数()
                '
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)

            Case CalcCategory.Square_Tate '四角数タブ縦の目の数
                ret = ret And set_四角数()
                '
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)

            Case CalcCategory.Square_Vert '四角数タブ高さ
                ret = ret And set_四角数()
                '
                ret = ret And calc_側面と縁(category, Nothing, Nothing)

            Case CalcCategory.Square_Expand '四角数タブ縦横展開
                ret = ret And set_四角数()
                '何本幅かを変更していた場合の影響
                ret = ret And calc_側面と縁(category, Nothing, Nothing)
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)

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
                Dim row As tbl追加品Row = CType(ctr, tbl追加品Row)
                ret = ret And calc_追加品(category, row, key)
                '(追加品は計算寸法変更なし)

            Case CalcCategory.Expand_Yoko  '横ひも展開
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                ret = ret And calc_横ひも展開(category, row, key)

            Case CalcCategory.Expand_Tate  '縦ひも展開
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                ret = ret And calc_縦ひも展開(category, row, key)


            '相互参照値のFix(1Pass値は得られている前提)
            Case CalcCategory.FixLength
                ret = ret And calc_側面と縁(category, Nothing, Nothing)     '高さ系
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)     '縦計
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)    '横計
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


    '四角数タブの取得
    'IN: _I基本のひも幅(チェック時)
    Function set_四角数() As Boolean

        With _Data.p_row底_縦横
            _i横の目の数 = .Value("f_i横の四角数")
            _i縦の目の数 = .Value("f_i縦の四角数")
            _i高さの目の数 = .Value("f_d高さの四角数")

            _d目_ひも間のすき間 = .Value("f_dひも間のすき間")
            _dひも長係数 = .Value("f_dひも長係数")
            _dひも長加算_縦横端 = .Value("f_d垂直ひも長加算")
            _d左端右端の目 = .Value("f_dひも長加算")
            _dひも長加算_側面 = .Value("f_dひも長加算_側面")

            _b縦横側面を展開する = .Value("f_b展開区分")
            _d左端右端の目 = .Value("f_d左端右端の目")
            _d上端下端の目 = .Value("f_d上端下端の目")
            _d最下段の目 = .Value("f_d最下段の目")
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

        If _i横の目の数 <= 0 OrElse _i縦の目の数 <= 0 Then
            '横ひも・縦ひも・編みひもの本数(目の数)をセットしてください。
            p_sメッセージ = My.Resources.CalcNoSquareCountSet
            Return False
        End If
        If _i高さの目の数 < 0 Then
            '横ひも・縦ひも・編みひもの本数(目の数)をセットしてください。
            p_sメッセージ = My.Resources.CalcNoSquareCountSet
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
        Dim i横の四角数 As Integer = Int(_d横_目標 / p_d縦横_四角)
        If _Data.p_row目標寸法.Value("f_b内側区分") Then
            '内側
            If i横の四角数 Mod 2 <> 0 Then
                i横の四角数 -= 1
            End If
        Else
            '外側
            Do While i横の四角数 * p_d縦横_四角 < _d横_目標
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
        Dim i縦の四角数 As Integer = Int(_d縦_目標 / p_d縦横_四角)
        If _Data.p_row目標寸法.Value("f_b内側区分") Then
            '内側
            If i縦の四角数 Mod 2 <> 0 Then
                i縦の四角数 -= 1
            End If
        Else
            Do While i縦の四角数 * p_d縦横_四角 < _d縦_目標
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
        Dim i高さの四角数 As Integer = Int((_d高さ_目標 - _d目_ひも間のすき間) / p_d縦横_四角)
        If Not _Data.p_row目標寸法.Value("f_b内側区分") Then
            Do While i高さの四角数 * p_d縦横_四角 < (_d高さ_目標 - _d目_ひも間のすき間)
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
    Private Function adjust_側面() As Boolean

        Dim table As tbl側面DataTable = _Data.p_tbl側面
        Dim row As tbl側面Row
        Dim ret As Boolean = True

        If 0 < _d最下段の目 * _d目_ひも間のすき間 Then
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
            row.f_d高さ = _d最下段の目 * _d目_ひも間のすき間
            row.f_d垂直ひも長 = _d最下段の目 * _d目_ひも間のすき間
            row.f_d周長 = 0
            row.f_dひも長 = 0
            row.f_d厚さ = 0
            row.f_i周数 = 0
        Else
            RemoveNumberFromTable(table, cIdxSpace)
        End If

        If 0 < _i高さの目の数 Then
            '「編みひも+目」のセット
            For i As Integer = 1 To _i高さの目の数
                row = clsDataTables.NumberSubRecord(table, cIdxHeight, i)
                If i = 1 OrElse _b縦横側面を展開する Then
                    If row Is Nothing Then
                        row = table.Newtbl側面Row
                        row.f_i番号 = cIdxHeight
                        row.f_iひも番号 = i
                        row.f_i何本幅 = _I基本のひも幅
                        table.Rows.Add(row)
                    End If
                    row.f_s編みかた名 = String.Format(My.Resources.FormCaption, "")
                    row.f_s編みひも名 = text側面の編みひも()
                    If _b縦横側面を展開する Then
                        row.f_iひも本数 = 1
                        row.f_i周数 = 1
                    Else
                        row.f_i何本幅 = _I基本のひも幅
                        row.f_iひも本数 = _i高さの目の数
                        row.f_i周数 = _i高さの目の数
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
            For i As Integer = _i高さの目の数 + 1 To submax
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
            row.f_d高さ = row.f_iひも本数 * (g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _d目_ひも間のすき間)
            row.f_d垂直ひも長 = row.f_d高さ
        Else
            row.Setf_d高さNull()
            row.Setf_d垂直ひも長Null()
        End If
        row.f_d周長 = p_d四角ベース_周 '立ち上げ時の増分
        row.f_dひも長 = p_d四角ベース_周 * _dひも長係数
        row.f_d厚さ = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
        row.f_d連続ひも長 = row.f_dひも長 + _dひも長加算_側面 + row.f_dひも長加算

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
            _d四角ベース_高さ計 = 0
        Else
            _d四角ベース_高さ計 = obj
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
        groupRow.SetNameValue("f_d周長", p_d四角ベース_周)

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

#Region "横ひも・縦ひも展開"
    Dim _tbl縦横展開_横ひも As tbl縦横展開DataTable 'New時に作成、以降は存在が前提
    Dim _tbl縦横展開_縦ひも As tbl縦横展開DataTable 'New時に作成、以降は存在が前提

    '展開処理により、縦ひも・横ひもそれぞれ、1本につき1レコードを生成する
    'ひも番号=0...上端下端/左端右端　の目がある時、f_d幅のみ持つ。enumひも種.i_すき間
    'ひも番号=1～本数-1   ... f_d幅は、ひも本幅数分の幅+_d目_ひも間のすき間
    'ひも番号=本数        ... f_d幅は、ひも本幅数分の幅+上端下端/左端右端の幅
    '始末ひも・補強ひもはenumひも種.i_補強

    'リスト出力値=f_d出力ひも長 係数・ひも長加算は2PassでFix


    '底の_tbl縦横展開_横ひもを得る。isReset=Trueでリセット
    Function get横展開DataTable(Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        If isReset Then
            'calc_縦ひも展開のコード抜粋
            renew横展開DataTable(False)
            _d四角ベース_縦計 = recalc_ひも展開(_tbl縦横展開_横ひも, enumひも種.i_横, _b横ひも本幅変更)
        Else
            calc_横ひも展開(CalcCategory.Expand_Yoko, Nothing, Nothing)
        End If
        Return _tbl縦横展開_横ひも
    End Function

    '底の_tbl縦横展開_縦ひもを得る。isReset=Trueでリセット
    Function get縦展開DataTable(Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        If isReset Then
            'calc_縦ひも展開のコード抜粋
            renew縦展開DataTable(False)
            _d四角ベース_横計 = recalc_ひも展開(_tbl縦横展開_縦ひも, enumひも種.i_縦, _b縦ひも本幅変更)
        Else
            calc_縦ひも展開(CalcCategory.Expand_Tate, Nothing, Nothing)
        End If
        Return _tbl縦横展開_縦ひも
    End Function


    '底の横ひもの編集完了,_tbl縦横展開_横ひもを_Dataに反映
    Function save横展開DataTable(Optional ByVal always As Boolean = False) As Boolean
        Try
            If always OrElse _tbl縦横展開_横ひも.GetChanges IsNot Nothing Then
                Dim change As Integer = _Data.FromTmpTable(enumひも種.i_横, _tbl縦横展開_横ひも)
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
                Dim change As Integer = _Data.FromTmpTable(enumひも種.i_縦, _tbl縦横展開_縦ひも)
            End If
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "save縦展開DataTable")
            Return False
        End Try
    End Function

    '更新処理が必要なフィールド名
    Shared _fields縦横展開() As String = {"f_i何本幅", "f_dひも長加算"}
    Shared Function IsDataPropertyName縦横展開(ByVal name As String) As Boolean
        Return _fields縦横展開.Contains(name)
    End Function

    '本幅変更と幅の合計を返す共通関数
    Private Function recalc_ひも展開(ByVal table As tbl縦横展開DataTable, ByVal filt As enumひも種, ByRef isChange As Boolean) As Double
        Dim dSum幅 As Double = 0
        Dim obj As Object = table.Compute("SUM(f_d幅)", Nothing)
        If Not IsDBNull(obj) AndAlso 0 < obj Then
            dSum幅 = obj
        End If

        Dim itype As Integer = filt
        Dim iMax何本幅 As Integer = 0
        Dim objMax As Object = table.Compute("MAX(f_i何本幅)", String.Format("f_iひも種 = {0}", itype))
        If Not IsDBNull(objMax) AndAlso 0 < objMax Then
            iMax何本幅 = objMax
        End If

        Dim iMin何本幅 As Integer = 0
        Dim objMin As Object = table.Compute("MIN(f_i何本幅)", String.Format("f_iひも種 = {0}", itype))
        If Not IsDBNull(objMin) AndAlso 0 < objMin Then
            iMin何本幅 = objMin
        End If

        isChange = iMax何本幅 <> _I基本のひも幅 OrElse iMin何本幅 <> _I基本のひも幅

        Return dSum幅
    End Function


#Region "横ひも"
    'CalcSizeからの呼び出し 対応した更新の後、再集計(_d四角ベース_縦計,_b横ひも本幅変更)
    'Expand_Yokoカテゴリの場合は、対応レコードの派生更新(画面で編集中のみ)   
    '以外は、set横展開DataTable()  により _tbl縦横展開_横ひも 更新
    'IN:    _b縦横側面を展開する
    'OUT:   _d四角ベース_縦計 _b横ひも本幅変更
    Function calc_横ひも展開(ByVal category As CalcCategory, ByVal row As tbl縦横展開Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category = CalcCategory.Expand_Yoko AndAlso row IsNot Nothing Then
            'テーブル編集中
            ret = adjust_横ひも(row, p_i横ひもの本数)

        Else
            '_tbl縦横展開_横ひもを再セット
            ret = renew横展開DataTable(_b縦横側面を展開する)

        End If

        'f_i何本幅の設定状態
        _d四角ベース_縦計 = recalc_ひも展開(_tbl縦横展開_横ひも, enumひも種.i_横, _b横ひも本幅変更)

        Return ret
    End Function

    '横ひもの指定レコードの幅と長さを計算
    'IN:    _d四角ベース_横計,_d四角ベース_高さ計,_d縁の垂直ひも長,_dひも長加算_縦横端,_dひも長係数
    'OUT:   各レコードのf_d長さ,f_dひも長,f_d出力ひも長
    Function adjust_横ひも(ByVal row As tbl縦横展開Row, ByVal lastひも番号 As Integer) As Boolean
        If row.f_iひも種 = enumひも種.i_横 Then
            If row.f_iひも番号 = lastひも番号 Then
                row.f_d幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _d上端下端の目 * _d目_ひも間のすき間
            Else
                row.f_d幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _d目_ひも間のすき間
            End If
            row.f_d長さ = p_d四角ベース_周の横 '_d四角ベース_横計に立ち上げ分プラス
            row.f_dひも長 = (p_d四角ベース_周の横 + 2 * _d四角ベース_高さ計) * _dひも長係数
            row.f_d出力ひも長 = row.f_dひも長 +
                2 * (_dひも長加算_縦横端 + _d縁の垂直ひも長) +
                row.f_dひも長加算
        End If

        Return True
    End Function


    '底の横ひもの再展開       ※adjust_横ひも()呼び出し
    'IN:    p_i横ひもの本数,_d上端下端の目,_d目_ひも間のすき間,_d四角ベース_横計
    'OUT:   _tbl縦横展開_横ひも
    Private Function renew横展開DataTable(ByVal isRefSaved As Boolean) As Boolean
        '上から下へ
        Dim pos As Integer = -(p_i横ひもの本数 \ 2)
        Dim zero As Integer = p_i横ひもの本数 Mod 2

        _tbl縦横展開_横ひも.Clear()
        Dim row As tbl縦横展開Row

        If 0 < _d上端下端の目 * _d目_ひも間のすき間 Then
            row = _tbl縦横展開_横ひも.Newtbl縦横展開Row
            row.f_iひも種 = enumひも種.i_横 Or enumひも種.i_すき間
            row.f_i位置番号 = pos - 1
            row.f_iひも番号 = 0
            row.f_sひも名 = text上端下端()
            row.Setf_i何本幅Null()
            row.f_d幅 = _d上端下端の目 * _d目_ひも間のすき間
            row.Setf_dひも長Null()
            row.Setf_d長さNull()
            row.f_d出力ひも長 = 0

            _tbl縦横展開_横ひも.Rows.Add(row)
        End If

        If 0 < p_i横ひもの本数 Then
            For idx As Integer = 1 To p_i横ひもの本数
                row = _tbl縦横展開_横ひも.Newtbl縦横展開Row

                row.f_iひも種 = enumひも種.i_横
                row.f_i位置番号 = pos
                row.f_iひも番号 = idx
                row.f_sひも名 = text横ひも()
                row.f_i何本幅 = _I基本のひも幅
                adjust_横ひも(row, p_i横ひもの本数)

                _tbl縦横展開_横ひも.Rows.Add(row)
                pos += 1
                If pos = 0 AndAlso zero = 0 Then
                    pos = 1
                End If
            Next
        End If

        '以降は裏側
        Dim posyoko As Integer = cBackPosition
        If _Data.p_row底_縦横.Value("f_b補強ひも区分") Then
            For idx As Integer = 1 To 2
                row = _tbl縦横展開_横ひも.Newtbl縦横展開Row
                row.f_iひも種 = enumひも種.i_横 Or enumひも種.i_補強
                row.f_i位置番号 = posyoko
                row.f_iひも番号 = idx
                row.f_sひも名 = text横の補強ひも()
                row.f_i何本幅 = _I基本のひも幅
                row.Setf_d幅Null()

                row.f_d長さ = p_d四角ベース_横
                row.f_dひも長 = p_d四角ベース_横
                row.f_d出力ひも長 = row.f_dひも長

                _tbl縦横展開_横ひも.Rows.Add(row)
                posyoko += 1
            Next
        End If

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_横, _tbl縦横展開_横ひも)
            For Each row In _tbl縦横展開_横ひも
                adjust_横ひも(row, p_i横ひもの本数)
            Next
        End If

        _tbl縦横展開_横ひも.AcceptChanges()
        Return True
    End Function


#End Region

#Region "縦ひも"

    'CalcSizeからの呼び出し 対応した更新の後、再集計(_d四角ベース_横計,_b縦ひも本幅変更)
    'Expand_Tate、対応レコードの派生更新(画面で編集中のみ)   
    '以外は、set縦展開DataTable()  により _tbl縦横展開_縦ひも 更新
    'IN:_b縦横側面を展開する
    'OUT:_d四角ベース_横計 _b縦ひも本幅変更
    Function calc_縦ひも展開(ByVal category As CalcCategory, ByVal row As tbl縦横展開Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category = CalcCategory.Expand_Tate AndAlso row IsNot Nothing Then
            'テーブル編集中
            ret = adjust_縦ひも(row, p_i縦ひもの本数)

        Else
            '_tbl縦横展開_縦ひもを再セット
            ret = renew縦展開DataTable(_b縦横側面を展開する)

        End If

        'f_i何本幅の設定状態
        _d四角ベース_横計 = recalc_ひも展開(_tbl縦横展開_縦ひも, enumひも種.i_縦, _b縦ひも本幅変更)

        Return ret
    End Function

    '縦ひもの指定レコードの幅と長さを計算
    'IN:    _d四角ベース_縦計,_d四角ベース_高さ計,_d縁の垂直ひも長,_dひも長加算_縦横端,_dひも長係数
    'OUT:   各レコードのf_d長さ,f_dひも長,f_d出力ひも長
    Function adjust_縦ひも(ByVal row As tbl縦横展開Row, ByVal lastひも番号 As Integer) As Boolean
        If row.f_iひも種 = enumひも種.i_縦 Then
            If row.f_iひも番号 = lastひも番号 Then
                row.f_d幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _d左端右端の目 * _d目_ひも間のすき間
            Else
                row.f_d幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) + _d目_ひも間のすき間
            End If
            row.f_d長さ = p_d四角ベース_周の縦 '_d四角ベース_縦計に立ち上げ分プラス
            row.f_dひも長 = (p_d四角ベース_周の縦 + 2 * _d四角ベース_高さ計) * _dひも長係数
            row.f_d出力ひも長 = row.f_dひも長 +
                2 * (_dひも長加算_縦横端 + _d縁の垂直ひも長) +
                row.f_dひも長加算
        End If

        Return True
    End Function

    '底の縦ひもの再展開       ※adjust_縦ひも()呼び出し
    'IN:    p_i縦ひもの本数,_d左端右端の目,_d目_ひも間のすき間,_d四角ベース_縦計
    'OUT:   _tbl縦横展開_縦ひも
    Private Function renew縦展開DataTable(ByVal isRefSaved As Boolean) As Boolean
        '左から右へ
        Dim pos As Integer = -(p_i縦ひもの本数 \ 2)
        Dim zero As Integer = p_i縦ひもの本数 Mod 2

        _tbl縦横展開_縦ひも.Clear()
        Dim row As tbl縦横展開Row

        If 0 < _d左端右端の目 * _d目_ひも間のすき間 Then
            row = _tbl縦横展開_縦ひも.Newtbl縦横展開Row
            row.f_iひも種 = enumひも種.i_縦 Or enumひも種.i_すき間
            row.f_i位置番号 = pos - 1
            row.f_iひも番号 = 0
            row.f_sひも名 = text左端右端()
            row.Setf_i何本幅Null()
            row.f_d幅 = _d左端右端の目 * _d目_ひも間のすき間
            row.Setf_dひも長Null()
            row.Setf_d長さNull()
            row.f_d出力ひも長 = 0

            _tbl縦横展開_縦ひも.Rows.Add(row)
        End If

        If 0 < p_i縦ひもの本数 Then
            For idx As Integer = 1 To p_i縦ひもの本数
                row = _tbl縦横展開_縦ひも.Newtbl縦横展開Row

                row.f_iひも種 = enumひも種.i_縦
                row.f_i位置番号 = pos
                row.f_iひも番号 = idx
                row.f_sひも名 = text縦ひも()
                row.f_i何本幅 = _I基本のひも幅
                adjust_縦ひも(row, p_i縦ひもの本数)

                _tbl縦横展開_縦ひも.Rows.Add(row)
                pos += 1
                If pos = 0 AndAlso zero = 0 Then
                    pos = 1
                End If
            Next
        End If

        '以降は裏側
        Dim postate As Integer = cBackPosition
        If _Data.p_row底_縦横.Value("f_b始末ひも区分") Then
            For idx As Integer = 1 To 2
                row = _tbl縦横展開_縦ひも.Newtbl縦横展開Row
                row.f_iひも種 = enumひも種.i_縦 Or enumひも種.i_補強
                row.f_i位置番号 = postate
                row.f_iひも番号 = idx
                row.f_sひも名 = text縦の補強ひも()
                row.f_i何本幅 = _I基本のひも幅
                row.Setf_d幅Null()

                row.f_d長さ = p_d四角ベース_縦
                row.f_dひも長 = p_d四角ベース_縦
                row.f_d出力ひも長 = row.f_dひも長

                _tbl縦横展開_縦ひも.Rows.Add(row)
                postate += 1
            Next
        End If

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, _tbl縦横展開_縦ひも)
            For Each row In _tbl縦横展開_縦ひも
                adjust_縦ひも(row, p_i縦ひもの本数)
            Next
        End If

        _tbl縦横展開_縦ひも.AcceptChanges()
        Return True
    End Function

#End Region

#End Region

#Region "追加品"
    'リスト出力時にひも長加算する。入力時には反映されない

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
        If row Is Nothing Then
            Return False '念のため
        End If
        row.f_b有効区分 = False
        row.Setf_iひも本数Null()
        row.Setf_dひも長Null()
        row.Setf_d出力ひも長Null()

        '設定なし
        If row.f_i配置面 = enum配置面.i_なし Then
            row.f_s無効理由 = text配置面()
            Return True 'unnable
        End If
        If row.f_i開始位置 < 1 Then
            row.f_s無効理由 = text開始位置()
            Return True 'unnable
        End If
        If row.f_i何本ごと < 0 Then
            row.f_s無効理由 = text何本ごと()
            Return True 'unnable
        End If

        '斜めの場合
        If (row.f_i角度 = enum角度.i_45度 OrElse row.f_i角度 = enum角度.i_135度) Then
            '幅変更なし
            If (_b横ひも本幅変更 OrElse _b縦ひも本幅変更 OrElse _b側面ひも本幅変更) Then
                row.f_s無効理由 = textひも本幅変更()
                Return True 'unnable
            End If
            If (row.f_i中心点 = enum中心点.i_目の中央) Then
                '目の中央を通すので対角線分の空き
                If _d目_ひも間のすき間 * ROOT2 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                    row.f_s無効理由 = text何本幅()
                    Return True 'unnable
                End If
            End If
        End If
        '縦横の場合
        If (row.f_i角度 = enum角度.i_0度 OrElse row.f_i角度 = enum角度.i_90度) Then
            '重ねるなら幅変更なし
            If (_b横ひも本幅変更 OrElse _b縦ひも本幅変更 OrElse _b側面ひも本幅変更) AndAlso
                (row.f_i中心点 = enum中心点.i_ひも中央) Then
                row.f_s無効理由 = textひも本幅変更()
                Return True 'unnable
            End If
            '目を通すなら
            If (row.f_i中心点 = enum中心点.i_目の中央) AndAlso
                _d目_ひも間のすき間 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                row.f_s無効理由 = text何本幅()
                Return True 'unnable
            End If
        End If


        '本数を得る
        Dim count As Integer = 0
        If row.f_i配置面 = enum配置面.i_底面 Then
            If row.f_i角度 = enum角度.i_0度 Then '底の横
                If row.f_i中心点 = enum中心点.i_目の中央 Then
                    count = p_i縦の目の実質数
                ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                    count = p_i横ひもの本数 '_i縦の目の数+1
                End If
                row.f_dひも長 = p_d四角ベース_横

            ElseIf row.f_i角度 = enum角度.i_90度 Then '底の縦
                If row.f_i中心点 = enum中心点.i_目の中央 Then
                    count = p_i横の目の実質数
                ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                    count = p_i縦ひもの本数 '_i横の目の数+1
                End If
                row.f_dひも長 = p_d四角ベース_縦

            ElseIf row.f_i角度 = enum角度.i_45度 OrElse row.f_i角度 = enum角度.i_135度 Then '底斜め
                count = p_i底の斜めの目の数
            End If

        ElseIf row.f_i配置面 = enum配置面.i_側面 Then
            If row.f_i角度 = enum角度.i_0度 Then '側面を水平に
                If row.f_i中心点 = enum中心点.i_目の中央 Then
                    count = p_i横の目の実質数
                ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                    count = _i高さの目の数 '編みひもの数
                End If
                row.f_dひも長 = p_d四角ベース_周

            ElseIf row.f_i角度 = enum角度.i_90度 Then '側面を垂直に
                If row.f_i中心点 = enum中心点.i_目の中央 Then
                    count = 2 * (p_i横の目の実質数 + p_i縦の目の実質数)
                ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                    count = p_i垂直ひも数
                End If
                row.f_dひも長 = _d四角ベース_高さ計 + _d縁の高さ

            ElseIf row.f_i角度 = enum角度.i_45度 OrElse row.f_i角度 = enum角度.i_135度 Then '側面斜め
                '目に配置
                count = 2 * (p_i横の目の実質数 + p_i縦の目の実質数)
                row.f_dひも長 = ROOT2 * (_d四角ベース_高さ計 + _d縁の高さ)
            End If

        ElseIf row.f_i配置面 = enum配置面.i_全面 Then
            If row.f_i角度 = enum角度.i_0度 Then '水平、側面に対してと同じ
                If row.f_i中心点 = enum中心点.i_目の中央 Then
                    count = p_i横の目の実質数
                ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                    count = _i高さの目の数 '編みひもの数
                End If
                row.f_dひも長 = p_d四角ベース_周

            ElseIf row.f_i角度 = enum角度.i_90度 Then '垂直、縦ひも+横ひも
                If row.f_i中心点 = enum中心点.i_目の中央 Then
                    count = p_i横の目の実質数 + p_i縦の目の実質数
                ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                    count = p_i縦ひもの本数 + p_i横ひもの本数
                End If

            ElseIf row.f_i角度 = enum角度.i_45度 OrElse row.f_i角度 = enum角度.i_135度 Then '底斜め
                '底と同じ
                count = p_i底の斜めの目の数
            End If
        End If

        If count <= 0 OrElse count < row.f_i開始位置 Then
            row.f_s無効理由 = text開始位置()
            Return True 'unnable
        End If
        row.f_b有効区分 = True
        row.Setf_s無効理由Null()

        '本数計算
        If row.f_i何本ごと = 0 Then
            row.f_iひも本数 = 1
        Else
            Dim left As Integer = count - row.f_i開始位置
            row.f_iひも本数 = (left \ row.f_i何本ごと) + 1
        End If
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0} {1},{2},{3} 全{4}本のうち{5}本ごと{6}から{7}本",
                 row.f_i番号, CType(row.f_i配置面, enum配置面), CType(row.f_i角度, enum角度), CType(row.f_i中心点, enum中心点), count, row.f_i何本ごと, row.f_i開始位置, row.f_iひも本数)

        If Not row.Isf_dひも長Null AndAlso 0 < row.f_dひも長 Then
            row.f_d出力ひも長 = row.f_dひも長 + 2 * row.f_dひも長加算
        End If
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
        row.f_dひも長加算 = _dひも長加算_縦横端

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

#Region "ひも上下"

    Dim _CUpDown As clsUpDown 'New時に作成、以降は存在が前提

    'ひも上下の展開
    Function loadひも上下(ByVal isSide As Boolean) As clsUpDown

        '_CUpDownにセットする
        _CUpDown.Clear()
        _CUpDown.IsSide = isSide

        If isSide Then
            _Data.ToClsUpDown(clsUpDown.cSideNumber, _CUpDown)
        Else
            _Data.ToClsUpDown(clsUpDown.cBottomNumber, _CUpDown)
        End If

        If Not _CUpDown.IsValid Then
            _CUpDown.Reset()
        End If

        Return _CUpDown
    End Function

    'ひも上下の編集完了,_Dataに反映
    Function saveひも上下(ByVal updown As clsUpDown) As Boolean
        'レコードにセットする
        Dim ret As Boolean
        If updown.IsSide Then
            ret = _Data.FromClsUpDown(clsUpDown.cSideNumber, updown)
        Else
            ret = _Data.FromClsUpDown(clsUpDown.cBottomNumber, updown)
        End If

        If Not ret Then
            'ひも上下レコードの保存エラーです。
            p_sメッセージ = My.Resources.CalcUpDownSaveErr
        End If

        Return ret
    End Function

    Function updownサイズ変更(ByVal horizontal As Integer, ByVal vertical As Integer, ByVal read_tbl As Boolean) As clsUpDown
        _CUpDown.HorizontalCount = horizontal
        _CUpDown.VerticalCount = vertical
        If Not _CUpDown.ReSize(read_tbl) Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updownリセット() As clsUpDown
        If Not _CUpDown.Reset() Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updown上下交換() As clsUpDown
        If Not _CUpDown.Revert() Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updown左右反転() As clsUpDown
        If Not _CUpDown.LeftSideRight() Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updown天地反転() As clsUpDown
        If Not _CUpDown.UpSideDown() Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updown右回転() As clsUpDown
        If Not _CUpDown.RotateRight() Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updown水平追加(ByVal atTop As Boolean) As clsUpDown
        If Not _CUpDown.AddHorizontal(atTop) Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updown垂直追加(ByVal atTop As Boolean) As clsUpDown
        If Not _CUpDown.AddVertical(atTop) Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updown水平シフト(ByVal desc As Boolean) As clsUpDown
        If Not _CUpDown.ShiftHorizontal(desc) Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updown垂直シフト(ByVal desc As Boolean) As clsUpDown
        If Not _CUpDown.ShiftVertical(desc) Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updownランダム() As clsUpDown
        If Not _CUpDown.Randomize() Then
            Return Nothing
        End If
        Return _CUpDown
    End Function

    Function updownチェック() As Boolean
        Return _CUpDown.Check(p_sメッセージ)
    End Function

#End Region

#Region "プレビュー"

    'プレビュー時に生成,描画後はNothing
    Dim _ImageList横ひも As clsImageItemList   '横ひもの展開レコードを含む
    Dim _ImageList縦ひも As clsImageItemList   '縦ひもの展開レコードを含む
    Dim _imageList側面上 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面左 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面下 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面右 As clsImageItemList    '側面の展開レコードを含む
    Dim _ImageList描画要素 As clsImageItemList '底と側面



    '横ひもリストの描画情報 : _tbl縦横展開_横ひも → _ImageList横ひも
    Private Function imageList横ひも() As Boolean
        If _tbl縦横展開_横ひも Is Nothing OrElse _tbl縦横展開_横ひも.Rows.Count = 0 Then
            Return False
        End If

        _ImageList横ひも = New clsImageItemList(_tbl縦横展開_横ひも)

        Dim dひも表示長 As Double = p_d四角ベース_高さ + _d縁の高さ + p_d厚さ * 2
        Dim p横ひも左上 As New S実座標(-dひも表示長 - p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2)
        Dim p横ひも右上 As New S実座標(dひも表示長 + p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2)

        '上から下へ(位置順)
        _ImageList横ひも.SortByPosition()
        For Each band As clsImageItem In _ImageList横ひも
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            Dim width As S差分 = Unit270 * band.m_row縦横展開.f_d幅
            Dim bandwidth As S差分 = Unit270 * g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)

            If band.m_row縦横展開.f_iひも種 = enumひも種.i_横 Then
                band.m_rひも位置.p左上 = p横ひも左上
                band.m_rひも位置.p右下 = p横ひも右上 + bandwidth
                band.m_borderひも = DirectionEnum._上 Or DirectionEnum._下
            Else
                '補強ひもは描画しない
                band.m_bNoMark = True
            End If
            '
            p横ひも左上 = p横ひも左上 + width
            p横ひも右上 = p横ひも右上 + width
        Next

        Return True
    End Function

    '縦ひもリストの描画情報 : _tbl縦横展開_縦ひも → _ImageList縦ひも
    Private Function imageList縦ひも() As Boolean
        If _tbl縦横展開_縦ひも Is Nothing OrElse _tbl縦横展開_縦ひも.Rows.Count = 0 Then
            Return False
        End If

        _ImageList縦ひも = New clsImageItemList(_tbl縦横展開_縦ひも)

        Dim dひも表示長 As Double = p_d四角ベース_高さ + _d縁の高さ + p_d厚さ * 2
        Dim p縦ひも左上 As New S実座標(-p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2 + dひも表示長)
        Dim p縦ひも左下 As New S実座標(-p_d四角ベース_横 / 2, -p_d四角ベース_縦 / 2 - dひも表示長)

        '左から右へ(位置順)
        _ImageList縦ひも.SortByPosition()
        For Each band As clsImageItem In _ImageList縦ひも
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            Dim width As S差分 = Unit0 * band.m_row縦横展開.f_d幅
            Dim bandwidth As S差分 = Unit0 * g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)

            If band.m_row縦横展開.f_iひも種 = enumひも種.i_縦 Then
                band.m_rひも位置.p左上 = p縦ひも左上
                band.m_rひも位置.p右下 = p縦ひも左下 + bandwidth
                band.m_borderひも = DirectionEnum._左 Or DirectionEnum._右
            Else
                band.m_bNoMark = True
            End If

            '
            p縦ひも左上 = p縦ひも左上 + width
            p縦ひも左下 = p縦ひも左下 + width
        Next

        Return True
    End Function

    '底の上下をm_regionListにセット
    Private Function regionUpDown底() As Boolean
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            Return False
        End If

        _CUpDown.IsSide = False '底
        If Not _Data.ToClsUpDown(clsUpDown.cBottomNumber, _CUpDown) Then
            _CUpDown.Reset()
        End If
        If Not _CUpDown.IsValid Then
            Return False
        End If

        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                If _CUpDown.GetIsDown(iTate, iYoko) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If itemYoko IsNot Nothing Then
                        itemTate.m_regionList.Add領域(itemYoko.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                If _CUpDown.GetIsUp(iTate, iYoko) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If itemTate IsNot Nothing Then
                        itemYoko.m_regionList.Add領域(itemTate.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        Return True
    End Function

    '側面の上下をm_regionListにセット
    Private Function regionUpDown側面() As Boolean
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            Return False
        End If
        If _imageList側面上 Is Nothing OrElse _imageList側面左 Is Nothing OrElse _imageList側面下 Is Nothing OrElse _imageList側面右 Is Nothing Then
            Return False
        End If

        _CUpDown.IsSide = True '側面
        If Not _Data.ToClsUpDown(clsUpDown.cSideNumber, _CUpDown) Then
            _CUpDown.Reset()
        End If
        If Not _CUpDown.IsValid Then
            Return False
        End If


        Dim horzDif As Integer = 0 '横に上→右→下→左
        '*上の側面
        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            For iTakasa As Integer = 1 To _i高さの目の数
                If _CUpDown.GetIsDown(iTate + horzDif, iTakasa) Then
                    Dim itemUSide As clsImageItem = _imageList側面上.GetRowItem(enumひも種.i_側面, iTakasa)
                    If itemUSide IsNot Nothing Then
                        itemTate.m_regionList.Add領域(itemUSide.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To _i高さの目の数
            Dim itemUSide As clsImageItem = _imageList側面上.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemUSide Is Nothing Then
                Continue For
            End If
            If itemUSide.m_regionList Is Nothing Then itemUSide.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                If _CUpDown.GetIsUp(iTate + horzDif, iTakasa) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If itemTate IsNot Nothing Then
                        itemUSide.m_regionList.Add領域(itemTate.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemUSide({0}):{1}", iTakasa, itemUSide.m_regionList.ToString)
        Next
        horzDif += p_i縦ひもの本数


        '*右の側面
        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            For iTakasa As Integer = 1 To _i高さの目の数
                If _CUpDown.GetIsDown(iYoko + horzDif, iTakasa) Then
                    Dim itemRSide As clsImageItem = _imageList側面右.GetRowItem(enumひも種.i_側面, iTakasa)
                    If itemRSide IsNot Nothing Then
                        itemYoko.m_regionList.Add領域(itemRSide.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To _i高さの目の数
            Dim itemRSide As clsImageItem = _imageList側面右.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemRSide Is Nothing Then
                Continue For
            End If
            If itemRSide.m_regionList Is Nothing Then itemRSide.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                If _CUpDown.GetIsUp(iYoko + horzDif, iTakasa) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If itemYoko IsNot Nothing Then
                        itemRSide.m_regionList.Add領域(itemYoko.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemRSide({0}):{1}", iTakasa, itemRSide.m_regionList.ToString)
        Next
        horzDif += p_i横ひもの本数


        '*下の側面(UpDownは左→右)
        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト
            Dim horzIdx As Integer = p_i縦ひもの本数 - iTate + 1 + horzDif

            For iTakasa As Integer = 1 To _i高さの目の数
                If _CUpDown.GetIsDown(horzIdx, iTakasa) Then
                    Dim itemDSide As clsImageItem = _imageList側面下.GetRowItem(enumひも種.i_側面, iTakasa)
                    If itemDSide IsNot Nothing Then
                        itemTate.m_regionList.Add領域(itemDSide.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To _i高さの目の数
            Dim itemDSide As clsImageItem = _imageList側面下.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemDSide Is Nothing Then
                Continue For
            End If
            If itemDSide.m_regionList Is Nothing Then itemDSide.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                Dim horzIdx As Integer = p_i縦ひもの本数 - iTate + 1 + horzDif
                If _CUpDown.GetIsUp(horzIdx, iTakasa) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If itemTate IsNot Nothing Then
                        itemDSide.m_regionList.Add領域(itemTate.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemUSide({0}):{1}", iTakasa, itemDSide.m_regionList.ToString)
        Next
        horzDif += p_i縦ひもの本数

        '*左の側面(UpDownは下→上)
        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            Dim horzIdx As Integer = p_i横ひもの本数 - iYoko + 1 + horzDif
            For iTakasa As Integer = 1 To _i高さの目の数
                If _CUpDown.GetIsDown(horzIdx, iTakasa) Then
                    Dim itemLSide As clsImageItem = _imageList側面左.GetRowItem(enumひも種.i_側面, iTakasa)
                    If itemLSide IsNot Nothing Then
                        itemYoko.m_regionList.Add領域(itemLSide.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To _i高さの目の数
            Dim itemLSide As clsImageItem = _imageList側面左.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemLSide Is Nothing Then
                Continue For
            End If
            If itemLSide.m_regionList Is Nothing Then itemLSide.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                Dim horzIdx As Integer = p_i横ひもの本数 - iYoko + 1 + horzDif
                If _CUpDown.GetIsUp(horzIdx, iTakasa) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If itemYoko IsNot Nothing Then
                        itemLSide.m_regionList.Add領域(itemYoko.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemRSide({0}):{1}", iTakasa, itemLSide.m_regionList.ToString)
        Next

        Return True
    End Function

    '_imageList側面上・_imageList側面左・_imageList側面下・_imageList側面右生成、
    '※側面のレコードはリスト出力時にadjust_側面() 済み
    Function imageList四側面() As Boolean

        '側面のレコードを縦横レコード化
        Dim tmptable As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        Dim d最下の高さ As Double = 0

        Dim idx As Integer = 1
        For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, "f_i番号 ASC , f_iひも番号 ASC")
            If r.f_i番号 = cHemNumber Then
                '縁は編みかたとして処理
                Continue For
            End If
            If r.f_i番号 = cIdxSpace Then
                '最下段のスペースはレコードにしない
                d最下の高さ = r.f_d高さ
                Continue For
            End If
            For i As Integer = 1 To r.f_iひも本数
                row = tmptable.Newtbl縦横展開Row
                row.f_iひも種 = enumひも種.i_側面
                row.f_iひも番号 = idx
                row.f_i位置番号 = i '参考値
                row.f_i何本幅 = r.f_i何本幅
                row.f_s記号 = r.f_s記号
                row.f_s色 = r.f_s色
                row.f_dひも長 = r.f_dひも長
                row.f_dひも長加算 = r.f_dひも長加算
                row.f_dひも長加算2 = 0
                row.f_d出力ひも長 = r.f_d連続ひも長
                If _b縦横側面を展開する Then
                    row.f_d幅 = r.f_d高さ '個別
                Else
                    row.f_d幅 = _d基本のひも幅 + _d目_ひも間のすき間 '合計なので再計算
                End If
                tmptable.Rows.Add(row)

                idx += 1
            Next
        Next
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "tmptable:{0}", New clsGroupDataRow(tmptable).ToString)


        '以降参照するのでここでセットする
        _imageList側面上 = New clsImageItemList(tmptable)
        _imageList側面左 = New clsImageItemList(tmptable)
        _imageList側面下 = New clsImageItemList(tmptable)
        _imageList側面右 = New clsImageItemList(tmptable)

        Dim item As clsImageItem

        Dim p上ひも左下 As New S実座標(-p_d四角ベース_周の横 / 2, d最下の高さ + p_d四角ベース_周の縦 / 2)
        Dim p下ひも左上 As New S実座標(-p_d四角ベース_周の横 / 2, -d最下の高さ - p_d四角ベース_周の縦 / 2)
        Dim p左ひも右上 As New S実座標(-d最下の高さ - p_d四角ベース_周の横 / 2, p_d四角ベース_周の縦 / 2)
        Dim p右ひも左上 As New S実座標(d最下の高さ + p_d四角ベース_周の横 / 2, p_d四角ベース_周の縦 / 2)

        '1～_i高さの目の数
        For i As Integer = 1 To _i高さの目の数
            '*上
            item = _imageList側面上.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面上:{0}", i)
            Else
                item.m_ImageType = ImageTypeEnum._横バンド

                item.m_rひも位置.p左下 = p上ひも左下
                item.m_rひも位置.p右上 = p上ひも左下 _
                + Unit90 * g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅) _
                + Unit0 * p_d四角ベース_周の横
                item.m_borderひも = DirectionEnum._上 Or DirectionEnum._下

                p上ひも左下 = p上ひも左下 + Unit90 * item.m_row縦横展開.f_d幅
            End If

            '*下
            item = _imageList側面下.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面下:{0}", i)
            Else
                item.m_ImageType = ImageTypeEnum._横バンド

                item.m_rひも位置.p左上 = p下ひも左上
                item.m_rひも位置.p右下 = p下ひも左上 _
                + Unit270 * g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅) _
                + Unit0 * p_d四角ベース_周の横
                item.m_borderひも = DirectionEnum._上 Or DirectionEnum._下
                item.m_bNoMark = True '記号なし

                p下ひも左上 = p下ひも左上 + Unit270 * item.m_row縦横展開.f_d幅
            End If

            '*左
            item = _imageList側面左.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面左:{0}", i)
            Else
                item.m_ImageType = ImageTypeEnum._縦バンド

                item.m_rひも位置.p右上 = p左ひも右上
                item.m_rひも位置.p左下 = p左ひも右上 _
                + Unit180 * g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅) _
                + Unit270 * p_d四角ベース_周の縦
                item.m_borderひも = DirectionEnum._左 Or DirectionEnum._右
                item.m_bNoMark = True '記号なし

                p左ひも右上 = p左ひも右上 + Unit180 * item.m_row縦横展開.f_d幅
            End If

            '*右
            item = _imageList側面右.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面右:{0}", i)
            Else
                item.m_ImageType = ImageTypeEnum._縦バンド

                item.m_rひも位置.p左上 = p右ひも左上
                item.m_rひも位置.p右下 = p右ひも左上 _
                + Unit0 * g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅) _
                + Unit270 * p_d四角ベース_周の縦
                item.m_borderひも = DirectionEnum._左 Or DirectionEnum._右
                item.m_bNoMark = True '記号なし

                p右ひも左上 = p右ひも左上 + Unit0 * item.m_row縦横展開.f_d幅
            End If
        Next

        '縁のレコードをイメージ情報化
        Dim cond As String = String.Format("f_i番号 = {0}", cHemNumber)
        Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

        Dim d高さ As Double = groupRow.GetNameValueSum("f_d高さ")
        Dim nひも本数 As Integer = groupRow.GetNameValueSum("f_iひも本数")
        If 0 < nひも本数 Then

            '*上
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 1)
            item.m_a四隅.p左下 = p上ひも左下
            item.m_a四隅.p右下 = p上ひも左下 + Unit0 * p_d四角ベース_周の横
            item.m_a四隅.p左上 = p上ひも左下 + Unit90 * d高さ
            item.m_a四隅.p右上 = p上ひも左下 + Unit90 * d高さ + Unit0 * p_d四角ベース_周の横

            '文字位置
            item.p_p文字位置 = p上ひも左下 + Unit0 * p_d四角ベース_周の横 + Unit90 * d高さ
            _imageList側面上.AddItem(item)

            '*下
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 2)
            item.m_a四隅.p左上 = p下ひも左上
            item.m_a四隅.p右上 = p下ひも左上 + Unit0 * p_d四角ベース_周の横
            item.m_a四隅.p左下 = p下ひも左上 + Unit270 * d高さ
            item.m_a四隅.p右下 = p下ひも左上 + Unit270 * d高さ + Unit0 * p_d四角ベース_周の横
            '文字なし
            _imageList側面下.AddItem(item)

            '*左
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 3)
            item.m_a四隅.p右上 = p左ひも右上
            item.m_a四隅.p左上 = p左ひも右上 + Unit180 * d高さ
            item.m_a四隅.p右下 = p左ひも右上 + Unit270 * p_d四角ベース_周の縦
            item.m_a四隅.p左下 = p左ひも右上 + Unit180 * d高さ + Unit270 * p_d四角ベース_周の縦
            '文字なし
            _imageList側面左.AddItem(item)

            '*右
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 4)
            item.m_a四隅.p左上 = p右ひも左上
            item.m_a四隅.p右上 = p右ひも左上 + Unit0 * d高さ
            item.m_a四隅.p左下 = p右ひも左上 + Unit270 * p_d四角ベース_周の縦
            item.m_a四隅.p右下 = p右ひも左上 + Unit0 * d高さ + Unit270 * p_d四角ベース_周の縦
            '文字なし
            _imageList側面右.AddItem(item)

        End If

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面上:{0}", _imageList側面上.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面左:{0}", _imageList側面左.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面下:{0}", _imageList側面下.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面右:{0}", _imageList側面右.ToString)
        Return True
    End Function

    '底と側面枠
    Function imageList底と側面枠() As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList


        Dim a底 As S四隅
        a底.p左上 = New S実座標(-p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2)
        a底.p右上 = New S実座標(p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        Dim a底の周 As S四隅
        a底の周.p左上 = New S実座標(-p_d四角ベース_周の横 / 2, p_d四角ベース_周の縦 / 2)
        a底の周.p右上 = New S実座標(p_d四角ベース_周の横 / 2, p_d四角ベース_周の縦 / 2)
        a底の周.p左下 = -a底の周.p右上
        a底の周.p右下 = -a底の周.p左上


        '底
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底

        Dim line As S線分
        '右上→左上
        line = New S線分(a底の周.p右上, a底の周.p左上)
        item.m_lineList.Add(line)
        '左上→左下
        line = New S線分(a底の周.p左上, a底の周.p左下)
        item.m_lineList.Add(line)
        '左下→右下
        line = New S線分(a底の周.p左下, a底の周.p右下)
        item.m_lineList.Add(line)
        '右下→右上
        line = New S線分(a底の周.p右下, a底の周.p右上)
        item.m_lineList.Add(line)

        itemlist.AddItem(item)


        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.p左下 = a底の周.p左上
        item.m_a四隅.p右下 = a底の周.p右上
        item.m_a四隅.p左上 = item.m_a四隅.p左下 + Unit90 * p_d縁厚さプラス_高さ
        item.m_a四隅.p右上 = item.m_a四隅.p右下 + Unit90 * p_d縁厚さプラス_高さ
        itemlist.AddItem(item)

        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅.p左上 = a底の周.p右上
        item.m_a四隅.p左下 = a底の周.p右下
        item.m_a四隅.p右上 = item.m_a四隅.p左上 + Unit0 * p_d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p左下 + Unit0 * p_d縁厚さプラス_高さ
        itemlist.AddItem(item)

        '下の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅.p左上 = a底の周.p左下
        item.m_a四隅.p右上 = a底の周.p右下
        item.m_a四隅.p左下 = item.m_a四隅.p左上 + Unit270 * p_d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p右上 + Unit270 * p_d縁厚さプラス_高さ
        itemlist.AddItem(item)

        '左の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p右上 = a底の周.p左上
        item.m_a四隅.p右下 = a底の周.p左下
        item.m_a四隅.p左上 = item.m_a四隅.p右上 + Unit180 * p_d縁厚さプラス_高さ
        item.m_a四隅.p左下 = item.m_a四隅.p右下 + Unit180 * p_d縁厚さプラス_高さ
        itemlist.AddItem(item)

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
        _ImageList横ひも = Nothing
        _ImageList縦ひも = Nothing
        _imageList側面上 = Nothing
        _imageList側面左 = Nothing
        _imageList側面下 = Nothing
        _imageList側面右 = Nothing
        _ImageList描画要素 = Nothing

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        '_tbl縦横展開_横ひも,_tbl縦横展開_縦ひもにレコードが残されているはず
        '_ImageList横ひも, _ImageList縦ひもを作る
        If Not imageList横ひも() OrElse Not imageList縦ひも() Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '基本のひも幅(文字サイズ)と基本色
        imgData.setBasics(_d基本のひも幅, _Data.p_row目標寸法.Value("f_s基本色"))


        If Not imageList四側面() Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If
        _ImageList描画要素 = imageList底と側面枠()


        '描画用のデータ追加
        regionUpDown底()
        regionUpDown側面()


        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing

        imgData.MoveList(_imageList側面上)
        imgData.MoveList(_imageList側面左)
        imgData.MoveList(_imageList側面下)
        imgData.MoveList(_imageList側面右)
        _imageList側面上 = Nothing
        _imageList側面左 = Nothing
        _imageList側面下 = Nothing
        _imageList側面右 = Nothing

        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
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

        '長さ計算をFix
        If Not CalcSize(CalcCategory.FixLength, Nothing, Nothing) Then
            Return False
        End If

        '出力レコード生成
        output.Clear()
        Dim row As tblOutputRow
        Dim order As String

        row = output.NextNewRow
        row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        row.f_s高さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
        row.f_s色 = _Data.p_row目標寸法.Value("f_s基本色")
        row.f_s編みかた名 = IO.Path.GetFileNameWithoutExtension(output.FilePath) '名前

        row = output.NextNewRow
        row.f_sカテゴリー = text四角数()
        row.f_s編みかた名 = text目_ひも間のすき間()
        row.f_s高さ = output.outLengthText(_d目_ひも間のすき間)

        '***四角数
        'このカテゴリーは先に行をつくる
        row = output.NextNewRow
        '横置き,縦置き
        For yokotate As Integer = 1 To 2
            Dim tmpTable As tbl縦横展開DataTable
            Dim sbMemo As New Text.StringBuilder

            If yokotate = 1 Then
                row.f_sタイプ = text横置き()
                tmpTable = _tbl縦横展開_横ひも 'set横展開DataTable(_b縦横側面を展開する)
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s横ひものメモ"))
            Else
                row.f_sタイプ = text縦置き()
                tmpTable = _tbl縦横展開_縦ひも 'set縦展開DataTable(_b縦横側面を展開する)
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s縦ひものメモ"))
            End If
            If tmpTable Is Nothing OrElse tmpTable.Rows.Count = 0 Then
                Continue For
            End If
            'レコードあり

            '長い順に記号を振る
            Dim tmps() As tbl縦横展開Row = tmpTable.Select(Nothing, "f_iひも種 ASC, f_d出力ひも長 DESC, f_s色")
            For Each tt As tbl縦横展開Row In tmps
                If 0 < tt.f_iひも番号 Then
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
                        Select Case lasttmp.f_sひも名
                            Case text横ひも()
                                row.f_s編みひも名 = String.Format("[{0}] {1}", p_i横ひもの本数, row.f_s編みひも名)
                            Case text縦ひも()
                                row.f_s編みひも名 = String.Format("[{0}] {1}", p_i縦ひもの本数, row.f_s編みひも名)
                        End Select
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
                    If 0 < r.f_iひも本数 Then
                        If 0 <= r.f_d連続ひも長 Then
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
                        output.SetBandRow(contcount, r_prv.f_i何本幅, r_prv.f_d連続ひも長, r_prv.f_s色)
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
                output.SetBandRow(contcount, r_prv.f_i何本幅, r_prv.f_d連続ひも長, r_prv.f_s色)
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
                row.f_sタイプ = _frmMain.PlateString(r.f_i配置面)
                row.f_s編みかた名 = _frmMain.AngleString(r.f_i角度)
                row.f_s編みひも名 = _frmMain.CenterString(r.f_i中心点)
                row.f_i周数 = r.f_i開始位置
                row.f_i段数 = r.f_i何本ごと
                If 0 < r.f_dひも長 AndAlso 0 < r.f_iひも本数 Then
                    row.f_s長さ = output.outLengthText(r.f_dひも長)
                    r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d出力ひも長, r.f_s色)
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
        row.f_s長さ = output.outLengthText(_d四角ベース_高さ計 + _d縁の垂直ひも長 + _dひも長加算_縦横端)

        row = output.NextNewRow
        row.f_s色 = text周()
        row.f_sひも本数 = output.outLengthText(p_d四角ベース_周)
        row.f_sひも長 = output.outLengthText(p_d縁厚さプラス_周)
        row.f_sメモ = My.Resources.CalcOutTurn '折り返し
        row.f_s長さ = output.outLengthText(_d縁の垂直ひも長 + _dひも長加算_縦横端 - _d縁の高さ)

        output.AddBlankLine()

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

    Private Function text縦置き() As String
        Return _frmMain.grp縦置き.Text
    End Function

    Private Function text差しひも() As String
        Return _frmMain.tpage差しひも.Text
    End Function

    Private Function textひも上下() As String
        Return _frmMain.tpageひも上下.Text
    End Function

    Private Function text横ひも() As String
        Return _frmMain.tpage横ひも.Text
    End Function

    Private Function text縦ひも() As String
        Return _frmMain.tpage縦ひも.Text
    End Function

    Private Function text横の目の数() As String
        Return _frmMain.lbl横の目の数.Text
    End Function

    Private Function text左端右端() As String
        Return _frmMain.lbl左端右端.Text
    End Function

    Private Function text縦の目の数() As String
        Return _frmMain.lbl縦の目の数.Text
    End Function

    Private Function text上端下端() As String
        Return _frmMain.lbl上端下端.Text
    End Function

    Private Function text横の補強ひも() As String
        Return _frmMain.chk横の補強ひも.Text
    End Function

    Private Function text縦の補強ひも() As String
        Return _frmMain.chk縦の補強ひも.Text
    End Function

    Private Function text高さの目の数() As String
        Return _frmMain.lbl高さの目の数.Text
    End Function

    Private Function text最下段() As String
        Return _frmMain.lbl最下段.Text
    End Function

    Private Function text巻きひも() As String
        'dgv追加品
        Return _frmMain.f_b巻きひも区分3.HeaderText
    End Function

    Private Function text編みかた名() As String
        Return _frmMain.lbl編みかた名_側面.Text
    End Function

    Private Function text縁の始末() As String
        Return _frmMain.lbl縁の始末.Text
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

    Private Function text本() As String
        Return _frmMain.lbl横ひもの本数_単位.Text
    End Function

    Private Function text垂直ひも数() As String
        Return _frmMain.lbl垂直ひも数.Text
    End Function

    Private Function text目_ひも間のすき間() As String
        Return _frmMain.lbl目_ひも間のすき間.Text
    End Function

    Private Function text厚さ() As String
        Return _frmMain.lbl厚さ.Text
    End Function

    Private Function text垂直ひも長() As String
        'dgv側面
        Return _frmMain.f_d垂直ひも長2.HeaderText
    End Function

    Private Function text四角ベース() As String
        Return _frmMain.lbl四角ベース.Text
    End Function

    Private Function text縁厚さプラス() As String
        Return _frmMain.lbl縁厚さプラス.Text
    End Function

    Private Function text周() As String
        Return _frmMain.lbl計算寸法の周.Text
    End Function

    Private Function textひも本幅変更() As String
        Return _frmMain.lblひも本幅変更.Text
    End Function

    'dgv差しひも
    Private Function text配置面() As String
        Return _frmMain.f_i配置面1.HeaderText
    End Function

    Private Function text開始位置() As String
        Return _frmMain.f_i開始位置1.HeaderText
    End Function

    Private Function text何本ごと() As String
        Return _frmMain.f_i何本ごと1.HeaderText
    End Function

    Private Function text何本幅() As String
        Return _frmMain.f_i何本幅1.HeaderText
    End Function

#End Region

End Class
