

Imports System.Reflection
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports System.Xml
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsMasterTables
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstOutput

Class clsCalcMesh

    '処理のカテゴリー
    Public Enum CalcCategory
        None

        NewData 'データ変更
        BsMaster  '基本値/マスター/バンド選択

        Target    '目標寸法
        Target_Band '基本のひも幅

        '底(縦横)
        Horizontal '横置き項目(底の縦横)
        Vertical '縦置き項目(底の縦横)
        Expand '縦横展開

        GapFit  'すき間を横寸法に合わせる。画面値更新と再計算
        LaneSync  '展開本幅の同期。底の縦横の本幅に合わせる

        Oval '底の楕円 
        Side  '側面 
        Options  '追加品
        Expand_Yoko '横ひも展開
        Expand_Tate '縦ひも展開

        '相互参照値のFix
        FixLength   '長さ確定
        BandColor   '色と幅の変更画面

    End Enum

    Public Property p_b有効 As Boolean
    Public Property p_sメッセージ As String 'p_b有効でない場合のみ参照

    '目標
    Private Property _d横_目標 As Double
    Private Property _d縦_目標 As Double
    Private Property _d高さ_目標 As Double
    Private Property _I基本のひも幅 As Integer
    Private Property _d基本のひも幅 As Double
    '底(縦横)のキャッシュ
    Private Property _i長い横ひもの本数 As Integer
    Private Property _i短い横ひもの本数 As Integer '_i長い横ひもの本数-1
    Private Property _i最上と最下の横ひも何本幅 As Integer  '三択から, ゼロはなし
    Private Property _d最上と最下の短いひもの幅 As Double   '〃　※展開タブでは変更不可
    Private Property _d横ひも間のすき間 As Double
    Private Property _i縦ひもの本数 As Integer
    Private Property _dひとつのすき間の寸法 As Double '縦ひも間
    Private Property _d垂直ひも長加算 As Double
    Private Property _b縦横を展開する As Boolean
    '縦横計算値
    Private Property _d縦横の横 As Double '縦横分のみ
    Private Property _d縦横の縦 As Double '縦横分のみ
    Private Property _i垂直ひも数_縦横 As Integer 'ゼロ以上
    Private Property _d縦横の垂直ひも間の周 As Double '2*(縦+最上と最下の短いひもを除く横)'ゼロ以上
    '底楕円
    Private Property _d径の合計 As Double '底_楕円の合計値
    Private Property _d底の周 As Double '_d縦横の周+2π径+定数,ゼロ以上
    Private Property _i垂直ひも数_楕円 As Integer 'ゼロ以上
    '側面
    Private Property _d高さの合計 As Double '側面の合計値,ゼロ以上
    Private Property _d周の最大値 As Double '周の最大値,ゼロ以上
    Private Property _d周の最小値 As Double '周の最小値,ゼロ以上
    Private Property _d垂直ひも長合計 As Double '側面の合計値,ゼロ以上
    Private Property _d厚さの最大値 As Double '厚さの最大値,ゼロ以上


    '※ここまでの各、個別集計値については、CalcSizeで正しく得られること。
    '　レコード内のひも長については、1Pass処理値とし、不正確な可能性あり
    '　2Passのadjust_横ひも()adjust_縦ひも()で、底の縦横・楕円・側面の結果反映


    '初期化
    Public Sub Clear()
        p_b有効 = False
        p_sメッセージ = Nothing

        _d横_目標 = -1
        _d縦_目標 = -1
        _d高さ_目標 = -1
        _I基本のひも幅 = -1
        _d基本のひも幅 = -1

        _i長い横ひもの本数 = -1
        _i短い横ひもの本数 = -1
        _i最上と最下の横ひも何本幅 = -1
        _d横ひも間のすき間 = -1
        _i縦ひもの本数 = -1
        _dひとつのすき間の寸法 = -1
        _d垂直ひも長加算 = -1
        _b縦横を展開する = False

        _d縦横の横 = -1
        _d縦横の縦 = -1
        _i垂直ひも数_縦横 = 0
        _d縦横の垂直ひも間の周 = 0
        _d最上と最下の短いひもの幅 = 0

        _d径の合計 = -1
        _d底の周 = -1
        _i垂直ひも数_楕円 = 0

        _d高さの合計 = 0
        _d周の最大値 = 0
        _d周の最小値 = 0
        _d垂直ひも長合計 = 0
        _d厚さの最大値 = 0

        __tbl横展開.Clear()
        __tbl縦展開.Clear()
    End Sub

#Region "プロパティ値"
    ReadOnly Property p_d内側_高さ As Double
        Get
            Return _d高さの合計
        End Get
    End Property
    ReadOnly Property p_d厚さ As Double
        Get
            'issues#4
            'If _d底の周 < _d周の最大値 Then
            '    Dim thick As Double = 8
            '    If 0 < _d径の合計 Then
            '        thick = 2 * PAI
            '    End If
            '    Return _d厚さの最大値 + (_d周の最大値 - _d底の周) / thick
            'Else
            '    Return _d厚さの最大値
            'End If
            Return _d厚さの最大値
        End Get
    End Property
    ReadOnly Property p_d外側_高さ As Double
        Get
            Return _d高さの合計 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
        End Get
    End Property

    '縦横部分
    ReadOnly Property p_d縦横の横 As Double
        Get
            Return _d縦横の横
        End Get
    End Property
    ReadOnly Property p_d縦横の縦 As Double
        Get
            Return _d縦横の縦
        End Get
    End Property

    '底のサイズ
    ReadOnly Property p_d内側_横 As Double
        Get
            Return _d縦横の横 + 2 * _d径の合計
        End Get
    End Property
    ReadOnly Property p_d内側_縦 As Double
        Get
            Return _d縦横の縦 + 2 * _d径の合計
        End Get
    End Property
    ReadOnly Property p_d外側_横 As Double
        Get
            Return p_d内側_横 + (g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ") * 2)
        End Get
    End Property
    ReadOnly Property p_d外側_縦 As Double
        Get
            Return p_d内側_縦 + (g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ") * 2)
        End Get
    End Property

    '厚さ(1/2づつ内側・外側)と最大・最小を考慮する issue#4
    ReadOnly Property p_d内側_最小横 As Double
        Get
            Return p_d内側_横 * (_d周の最小値 / _d底の周) - p_d厚さ
        End Get
    End Property
    ReadOnly Property p_d内側_最小縦 As Double
        Get
            Return p_d内側_縦 * (_d周の最小値 / _d底の周) - p_d厚さ
        End Get
    End Property
    ReadOnly Property p_d外側_最大横 As Double
        Get
            Return p_d外側_横 * (_d周の最大値 / _d底の周) + p_d厚さ
        End Get
    End Property
    ReadOnly Property p_d外側_最大縦 As Double
        Get
            Return p_d外側_縦 * (_d周の最大値 / _d底の周) + p_d厚さ
        End Get
    End Property

    Public ReadOnly Property p_d外側_最大周 As Double
        Get
            Dim thick As Double = 8
            If 0 < _d径の合計 Then
                thick = 2 * System.Math.PI
            End If
            Return _d周の最大値 + _d厚さの最大値 * thick
        End Get
    End Property

    '表示文字列
    Public ReadOnly Property p_s内側_横 As String
        Get
            If isValid計算_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d内側_横)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s内側_縦 As String
        Get
            If isValid計算_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d内側_縦)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s内側_高さ As String
        Get
            If isValid計算_高さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d内側_高さ)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s内側_底の周 As String
        Get
            If isValid計算_底の周 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d底の周)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s内側_最大周 As String
        Get
            If isValid計算_最大周 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d周の最大値)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s内側_最大周の径 As String
        Get
            If isValid計算_最大周 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d周の最大値 / Math.PI)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s内側_最小横 As String
        Get
            If isValid計算_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d内側_最小横)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s内側_最小縦 As String
        Get
            If isValid計算_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d内側_最小縦)
            End If
            Return ""
        End Get
    End Property


    Public ReadOnly Property p_s外側_横 As String
        Get
            If isValid計算_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d外側_横)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s外側_縦 As String
        Get
            If isValid計算_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d外側_縦)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s外側_高さ As String
        Get
            If isValid計算_高さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d外側_高さ)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s外側_底の周 As String
        Get
            If isValid計算_底の周 Then
                Dim thick As Double = 8 '長方形
                If 0 < _d径の合計 Then
                    thick = 2 * System.Math.PI '円
                End If
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d底の周 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ") * thick)
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property p_s外側_最大周 As String
        Get
            If isValid計算_最大周 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d外側_最大周)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s外側_最大周の径 As String
        Get
            If isValid計算_最大周 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d外側_最大周 / Math.PI)
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property p_s外側_最大横 As String
        Get
            If isValid計算_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d外側_最大横)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s外側_最大縦 As String
        Get
            If isValid計算_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(p_d外側_最大縦)
            End If
            Return ""
        End Get
    End Property

    '計算寸法と目標寸法の差
    Public ReadOnly Property p_s横寸法の差 As String
        Get
            If isValid横_目標 AndAlso isValid計算_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_d内側_横 - _d横_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s縦寸法の差 As String
        Get
            If isValid縦_目標 AndAlso isValid計算_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_d内側_縦 - _d縦_目標, True)
            End If
            Return ""
        End Get
    End Property
    Public ReadOnly Property p_s高さ寸法の差 As String
        Get
            If isValid高さ_目標 AndAlso isValid計算_高さ Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(p_d内側_高さ - _d高さ_目標, True)
            End If
            Return ""
        End Get
    End Property


    Public ReadOnly Property p_i垂直ひも数 As Integer
        Get
            Return _i垂直ひも数_縦横 + _i垂直ひも数_楕円
        End Get
    End Property

    Public ReadOnly Property p_s径の合計 As String
        Get
            If 0 <= _d径の合計 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d径の合計)
            End If
            Return ""
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

    Public ReadOnly Property p_s縦横の横 As String
        Get
            If isValid計算_横 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d縦横の横, False)
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property p_s縦横の縦 As String
        Get
            If isValid計算_縦 Then
                Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d縦横の縦, False)
            Else
                Return ""
            End If
        End Get
    End Property




    '値の有無
    Private ReadOnly Property isValid横_目標 As Boolean
        Get
            Return 0 < _d横_目標
        End Get
    End Property
    Private ReadOnly Property isValid縦_目標 As Boolean
        Get
            Return 0 < _d縦_目標
        End Get
    End Property
    Private ReadOnly Property isValid高さ_目標 As Boolean
        Get
            Return 0 <= _d高さ_目標
        End Get
    End Property
    Private ReadOnly Property isValid基本のひも幅 As Boolean
        Get
            Return 0 < _I基本のひも幅
        End Get
    End Property

    Private ReadOnly Property isValid計算_横 As Boolean
        Get
            Return 0 < _d縦横の横
        End Get
    End Property
    Private ReadOnly Property isValid計算_縦 As Boolean
        Get
            Return 0 <= _d縦横の縦
        End Get
    End Property
    Private ReadOnly Property isValid計算_高さ As Boolean
        Get
            Return 0 <= _d高さの合計
        End Get
    End Property
    Private ReadOnly Property isValid計算_底の周 As Boolean
        Get
            Return 0 < _d底の周
        End Get
    End Property
    Private ReadOnly Property isValid計算_最大周 As Boolean
        Get
            Return 0 < _d周の最大値
        End Get
    End Property

    '追加品の参照値 #63
    Function getAddPartsRefValues() As Double()
        Dim values(12) As Double
        values(0) = 1 'すべて有効

        '(内側)横・縦・高さ・周
        values(1) = p_d内側_最小横 'p_s内側_最小横
        values(2) = p_d内側_最小縦 'p_s内側_最小縦
        values(3) = p_d内側_高さ 'p_s内側_高さ
        values(4) = _d周の最大値 'p_s内側_最大周
        '(外側)横・縦・高さ・周
        values(5) = p_d外側_最大横 'p_s外側_最大横
        values(6) = p_d外側_最大縦 'p_s外側_最大縦
        values(7) = p_d外側_高さ 'p_s外側_高さ
        values(8) = p_d外側_最大周 'p_s外側_最大周
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

        Return sb.ToString
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("{0}({1}){2}", Me.GetType.Name, IIf(p_b有効, "Valid", "InValid"), p_sメッセージ).AppendLine()
        sb.AppendFormat("Target:({0},{1},{2}) Lane({3})", _d横_目標, _d縦_目標, _d高さ_目標, _I基本のひも幅).AppendLine()
        sb.AppendFormat("Mesh:({0},{1}) RoundVert({2}) Short Lane({3}) BandCount({4}) ", _d縦横の横, _d縦横の縦, _d縦横の垂直ひも間の周, _d最上と最下の短いひもの幅, _i垂直ひも数_縦横).AppendLine()
        sb.AppendFormat("Oval:r({0}) Circle({1}) BandCount({2})", _d径の合計, _d底の周, _i垂直ひも数_楕円).AppendLine()
        sb.AppendFormat("Side:H({0}) Circle({1},{2}) VerticalLength({3}) Thickness({4})", _d高さの合計, _d周の最小値, _d周の最大値, _d垂直ひも長合計, _d厚さの最大値).AppendLine()
        Return sb.ToString
    End Function

#End Region



    Dim _Data As clsDataTables
    Dim _frmMain As frmMain

    Sub New(ByVal data As clsDataTables, ByVal frm As frmMain)
        _Data = data
        _frmMain = frm

        __tbl横展開 = New tbl縦横展開DataTable
        __tbl縦展開 = New tbl縦横展開DataTable

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
                ret = ret And set_底の縦横()
                ret = ret And calc_横ひも展開(category, Nothing, Nothing) '縦の集計値
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing) '横の集計値
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_横ひも() '縦の集計値反映
                ret = ret And adjust_縦ひも() '横の集計値反映
                'calc_縦寸法()
                'calc_横寸法()

                If ret Then
                    p_sメッセージ = _frmMain.editAddParts.SetRefValueAndCheckError(_Data, getAddPartsRefValues)
                    If Not String.IsNullOrEmpty(p_sメッセージ) Then
                        ret = False
                    End If
                End If

            Case CalcCategory.Target    '目標寸法
                ret = ret And set_目標寸法(False)
               '(差の表示が変わるだけ)

            Case CalcCategory.Horizontal '横置き
                ret = ret And set_底の縦横()
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_縦ひも()
                'calc_縦寸法()

            Case CalcCategory.Vertical '縦置き
                ret = ret And set_底の縦横()
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_横ひも()
                'calc_横寸法()

            Case CalcCategory.GapFit     '横寸法に合わせる
                set_底の縦横()  '念のため
                Return calc_すき間の寸法()

            Case CalcCategory.LaneSync  '展開本幅の同期
                set_底の縦横()  '念のため
                If Not _b縦横を展開する Then
                    Return True '対象外
                End If
                ret = sync_展開本幅() '本幅と集計値
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_横ひも()
                ret = ret And adjust_縦ひも()


            Case CalcCategory.Expand '縦横展開
                ret = ret And set_底の縦横() 'CheckBox
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_横ひも()
                ret = ret And adjust_縦ひも()

            Case CalcCategory.Expand_Yoko  '横ひも展開
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                ret = ret And calc_横ひも展開(category, row, key)
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_縦ひも()

            Case CalcCategory.Expand_Tate  '縦ひも展開
                Dim row As tbl縦横展開Row = CType(ctr, tbl縦横展開Row)
                ret = ret And calc_縦ひも展開(category, row, key)
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_横ひも()

            Case CalcCategory.Oval '底の楕円
                Dim row As tbl底_楕円Row = CType(ctr, tbl底_楕円Row)
                ret = ret And calc_底楕円(category, row, key)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_横ひも()
                ret = ret And adjust_縦ひも()

            Case CalcCategory.Side  '側面
                Dim row As tbl側面Row = CType(ctr, tbl側面Row)
                ret = ret And calc_側面(category, row, key)
                ret = ret And adjust_横ひも()
                ret = ret And adjust_縦ひも()

            Case CalcCategory.Options  '追加品
                'エラーメッセージ通知
                p_sメッセージ = ctr
                If Not String.IsNullOrEmpty(p_sメッセージ) Then
                    ret = False
                End If
                '(追加品は計算寸法変更なし)

            Case CalcCategory.BandColor '色と幅の変更画面
                ret = ret And calc_横ひも展開(category, Nothing, Nothing)
                ret = ret And calc_縦ひも展開(category, Nothing, Nothing)
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)
                ret = ret And adjust_横ひも()
                ret = ret And adjust_縦ひも()

            Case CalcCategory.FixLength '相互参照値のFix(1Pass値は得られている前提)
                ret = ret And adjust_横ひも()
                ret = ret And adjust_縦ひも()
                p_sメッセージ = _frmMain.editAddParts.SetRefValueAndCheckError(_Data, getAddPartsRefValues)
                If Not String.IsNullOrEmpty(p_sメッセージ) Then
                    ret = False
                End If

            Case Else
                '未定義のカテゴリー'{0}'が参照されました。
                p_sメッセージ = String.Format(My.Resources.CalcNoDefCategory, category)
                ret = False

        End Select

        p_b有効 = ret
        Return ret
    End Function

    '目標寸法(内側値)をセットする
    Private Function set_目標寸法(ByVal needTarget As Boolean) As Boolean

        With _Data.p_row目標寸法
            _d横_目標 = .Value("f_d横寸法")
            _d縦_目標 = .Value("f_d縦寸法")
            _d高さ_目標 = .Value("f_d高さ寸法")
            _I基本のひも幅 = .Value("f_i基本のひも幅")
            _d基本のひも幅 = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)

            '内側の寸法を目標とする
            If Not .Value("f_b内側区分") Then
                _d横_目標 -= (g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ") * 2)
                _d縦_目標 -= (g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ") * 2)
                If 0 < _d高さ_目標 Then
                    _d高さ_目標 -= g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
                End If
            End If
        End With

        If Not isValidTarget(needTarget) Then
            Return False
        End If

        Return True
    End Function


    '
    '                  ■■■■■■                ←_i最上と最下の横ひもの本数(0/2)　同幅でも別扱い
    '        □□□□□□□□□□□□□□□□      ←_i長い横ひもの本数
    '                  □□□□□□                ←_i短い横ひもの本数
    '        □□□□□□□□□□□□□□□□
    '                  □□□□□□
    '        □□□□□□□□□□□□□□□□
    '                  ■■■■■■        ※最上と最下の横ひもの幅は、展開レコードでは変更不可 
    '                                        底の楕円の計算に使うため

    '底(縦横)の基本的な設定値のキャッシュ
    Private Function set_底の縦横() As Boolean
        With _Data.p_row底_縦横
            _b縦横を展開する = .Value("f_b展開区分")
            _d垂直ひも長加算 = .Value("f_d垂直ひも長加算")

            '横
            _i長い横ひもの本数 = .Value("f_i長い横ひもの本数")
            _d横ひも間のすき間 = .Value("f_d横ひも間のすき間")

            _i短い横ひもの本数 = 0
            If 1 < _i長い横ひもの本数 AndAlso 0 < .Value("f_i短い横ひも") Then
                _i短い横ひもの本数 = _i長い横ひもの本数 - 1
            End If

            '縦
            _i縦ひもの本数 = .Value("f_i縦ひもの本数")
            _dひとつのすき間の寸法 = .Value("f_dひとつのすき間の寸法")

            '派生値
            _i垂直ひも数_縦横 = 2 * (_i長い横ひもの本数 + _i縦ひもの本数)

            '三択値
            _i最上と最下の横ひも何本幅 = 0
            _d最上と最下の短いひもの幅 = 0
            If 0 < _i長い横ひもの本数 Then
                If .Value("f_i最上と最下の短いひも") = enum最上と最下の短いひも.i_同じ幅 _
                AndAlso 0 < .Value("f_i短い横ひも") Then
                    _i最上と最下の横ひも何本幅 = .Value("f_i短い横ひも")
                    _d最上と最下の短いひもの幅 = g_clsSelectBasics.p_d指定本幅(_i最上と最下の横ひも何本幅)
                End If
                If .Value("f_i最上と最下の短いひも") = enum最上と最下の短いひも.i_異なる幅 _
                AndAlso 0 < .Value("f_i最上と最下の短いひもの幅") Then
                    _i最上と最下の横ひも何本幅 = .Value("f_i最上と最下の短いひもの幅")
                    _d最上と最下の短いひもの幅 = g_clsSelectBasics.p_d指定本幅(_i最上と最下の横ひも何本幅)
                End If
            Else
                If .Value("f_i最上と最下の短いひも") = enum最上と最下の短いひも.i_同じ幅 _
                AndAlso 0 < .Value("f_i短い横ひも") Then
                    '{0}を指定するのであれば{1}をセットしてください。
                    p_sメッセージ = String.Format(My.Resources.CalcSetHorizontal, text最上と最下の短いひも(), text長い横ひも())
                    Return False
                End If
                If .Value("f_i最上と最下の短いひも") = enum最上と最下の短いひも.i_異なる幅 _
                AndAlso 0 < .Value("f_i最上と最下の短いひもの幅") Then
                    '{0}を指定するのであれば{1}をセットしてください。
                    p_sメッセージ = String.Format(My.Resources.CalcSetHorizontal, text最上と最下の短いひも(), text長い横ひも())
                    Return False
                End If
            End If

            '値のチェック
            If _i縦ひもの本数 < 1 Then
                '縦ひもの本数の指定が正しくありません。
                p_sメッセージ = My.Resources.CalcNoHeightCount
                Return False
            End If

            Dim d縦ひも間の最小間隔 As Double = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d縦ひも間の最小間隔")
            If 0 < d縦ひも間の最小間隔 AndAlso
                _dひとつのすき間の寸法 < d縦ひも間の最小間隔 Then
                '縦ひも間のすき間が最小間隔より小さくなっています。
                p_sメッセージ = My.Resources.CalcNoSpaceHeight
                Return False
            End If

        End With
        Return True
    End Function


#Region "縦横"

    '横寸法=(目標寸法-2径)として、間のすき間を算出する
    '　不可時はエラーメッセージで中断。OK時は、画面値を更新し関連再計算
    'IN:    _d横_目標,_d径の合計,_i縦ひもの本数,_d縦横の横,_dひとつのすき間の寸法
    'OUT:   nudひとつのすき間の寸法 
    Function calc_すき間の寸法() As Boolean
        Dim d縦ひも間の最小間隔 As Double = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d縦ひも間の最小間隔")

        Dim target As Double = _d横_目標 - 2 * _d径の合計

        Dim ret As Boolean = False
        Dim dひとつのすき間の寸法 As Double = -1

        If Not isValid横_目標 Then
            '横寸法が指定されていないため、すき間の寸法を計算できません。
            p_sメッセージ = My.Resources.CalcNoTargetWidth
            Return False

        Else
            If _i縦ひもの本数 < 2 Then
                '縦ひもの本数の指定が正しくありません。
                p_sメッセージ = My.Resources.CalcNoHeightCount
                Return False
            End If

            '縦ひも分の横寸法
            'Dim band As Double = g_clsSelectBasics.p_d指定本幅(.Value("f_i縦ひも")) * .Value("f_i縦ひもの本数")
            Dim band As Double = _d縦横の横 - _dひとつのすき間の寸法 * (_i縦ひもの本数 - 1)
            If target <= band Then
                '横寸法が小さすぎるため縦ひもを置けません。
                p_sメッセージ = My.Resources.CalcNoShortWidth
                Return False
            End If

            dひとつのすき間の寸法 = (target - band) / (_i縦ひもの本数 - 1)
        End If
        'すき間が計算できた

        '警告
        If dひとつのすき間の寸法 < d縦ひも間の最小間隔 Then
            '縦ひも間のすき間が最小間隔より小さくなっています。
            p_sメッセージ = My.Resources.CalcNoSpaceHeight
            Return False
        End If

        '結果のセット
        _frmMain.nudひとつのすき間の寸法.Value = dひとつのすき間の寸法 'recalc

        Return True
    End Function

#End Region

#Region "概算"
    '目標寸法のキャッシュを参照しながら、_Data.p_row底_縦横にセット

    '有効な目標寸法がある
    Public Function isValidTarget(ByVal needTarget As Boolean) As Boolean
        If needTarget Then
            If Not isValid横_目標 OrElse Not isValid縦_目標 OrElse Not isValid高さ_目標 OrElse Not isValid基本のひも幅 Then
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
        Return isValid計算_横 AndAlso isValid計算_縦 AndAlso isValid計算_高さ
        '高さはゼロでもよい
    End Function


    '目標寸法→底_縦横(チェックと確認)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="message">確認メッセージ</param>
    ''' <returns></returns>
    Public Function CheckTarget(ByRef message As String) As Boolean
        If Not isCalcable() Then
            Return False '概算できない
        End If

        If Not IsValidInput() Then
            message = Nothing
            Return True '入力がないので、すぐに概算していい
        End If
        '有効な入力がある場合

        '概算不要か？
        If isNear(p_d内側_横, _d横_目標) _
            AndAlso isNear(p_d内側_縦, _d縦_目標) _
            AndAlso isNear(p_d内側_高さ, _d高さ_目標) Then
            'ほぼ目標のサイズになっています。やり直す場合はリセットしてください。
            p_sメッセージ = My.Resources.CalcNoMoreChange
            Return False
        End If

        If isNear(p_d内側_横, _d横_目標) AndAlso isNear(p_d内側_縦, _d縦_目標) Then
            '側面のみ
            If 0 < _d高さの合計 Then
                '入力されている編みかたの周数を調整します。よろしいですか？
                message = My.Resources.CalcConfirmHight
            Else
                '最初に見つかった編みかた(指定する場合は1周セット)で周数を調整します。よろしいですか？
                message = My.Resources.CalcConfirmPattern
            End If
        Else
            '底の縦横から
            If 0 < _d径の合計 Then
                '"底の縦横を目標および底(楕円)の径に基づき再計算します。よろしいですか？"
                message = My.Resources.CalcConfirmRecalcDiameter
            Else
                '"底の縦横を目標に基づき再計算します。よろしいですか？"
                message = My.Resources.CalcConfirmRecalc
            End If
        End If

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

        If 0 < _d径の合計 Then
            If _d縦_目標 <= _d径の合計 * 2 Then
                '底(楕円)の径({0})が縦寸法以上になっているため横ひもを置けません。
                p_sメッセージ = String.Format(My.Resources.CalcHeightOver, _d径の合計 * 2)
                Return False
            End If
            If _d横_目標 <= _d径の合計 * 2 Then
                '底(楕円)の径({0})が横寸法以上になっているため縦ひもを置けません。
                p_sメッセージ = String.Format(My.Resources.CalcWidthOver, _d径の合計 * 2)
                Return False
            End If
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

    '目標寸法→底_縦横
    Private Function calc_Target() As Boolean
        '長い横ひもは基本のひも幅

        Dim ret As Boolean = True
        '_Data.p_row底_縦横.Value("f_b展開区分") = False  概算ボタンでOFF


        If Not isValid計算_横 OrElse Not isNear(p_d内側_横, _d横_目標) Then
            ret = ret And calc_Target_縦()
        End If
        If Not isValid計算_縦 OrElse Not isNear(p_d内側_縦, _d縦_目標) Then
            ret = ret And calc_Target_横()
        End If
        If Not isValid計算_高さ OrElse Not isNear(p_d内側_高さ, _d高さ_目標) Then
            ret = ret And calc_Target_高さ()
        End If
        Return ret
    End Function

    '縦寸法から横ひも(底楕円が設定されていればその分マイナス)
    Private Function calc_Target_横() As Boolean
        Dim d横ひも間のすき間 As Double = 0

        Dim d縦 As Double = _d縦_目標 - _d径の合計 * 2
        Dim yoko As Integer = Math.Truncate(d縦 / (_d基本のひも幅 + d横ひも間のすき間)) 'ドット以下切り捨て
        'issue#2
        Dim n長い横ひもの本数 As Integer = getOddHalf(yoko, ((g_clsSelectBasics.p_i本幅 / 2) < _I基本のひも幅))
        If n長い横ひもの本数 < 1 Then
            '縦寸法が小さすぎるため横ひもを置けません。
            p_sメッセージ = My.Resources.CalcTooShortHeight
            Return False
        End If

        Dim n短い横ひも数 As Integer = n長い横ひもの本数 - 1 'fix
        Dim n短い横ひもn本幅 As Integer = _I基本のひも幅 '開始値

        '短い横ひもで埋める分
        Dim dLeft As Double = d縦 - (n長い横ひもの本数 * (_d基本のひも幅 + d横ひも間のすき間))

        Dim dLeft2 As Double = dLeft - (n短い横ひも数 * (g_clsSelectBasics.p_d指定本幅(n短い横ひもn本幅) + d横ひも間のすき間))
        If dLeft2 < 0 Then
            '広すぎるので減らしていく
            Do While True
                If 1 < n短い横ひもn本幅 Then
                    n短い横ひもn本幅 -= 1
                Else
                    '減らせない
                    Exit Do
                End If
                dLeft2 = dLeft - (n短い横ひも数 * (g_clsSelectBasics.p_d指定本幅(n短い横ひもn本幅) + d横ひも間のすき間))
                If 0 < dLeft2 Then
                    Exit Do
                End If
            Loop

        Else
            '狭すぎるので増やしていく
            Do While True
                dLeft2 = dLeft - (n短い横ひも数 * (g_clsSelectBasics.p_d指定本幅(n短い横ひもn本幅) + d横ひも間のすき間))
                If dLeft2 <= g_clsSelectBasics.p_d指定本幅(g_clsSelectBasics.p_i本幅) * 2 Then
                    Exit Do
                ElseIf n短い横ひもn本幅 < g_clsSelectBasics.p_i本幅 Then
                    n短い横ひもn本幅 += 1
                    Continue Do
                Else
                    '増やせない
                    Exit Do
                End If
            Loop

        End If
        '最上と最下の短いひもで調整する分
        Dim dLeftAdjust As Double = 0
        If 0 < dLeft2 Then
            dLeftAdjust = dLeft2 / 2
        End If

        '***結果をセット
        With _Data.p_row底_縦横
            .Value("f_i長い横ひも") = _I基本のひも幅
            .Value("f_i長い横ひもの本数") = n長い横ひもの本数
            .Value("f_i短い横ひも") = n短い横ひもn本幅

            Dim rad As enum最上と最下の短いひも
            If dLeftAdjust = 0 Then
                rad = enum最上と最下の短いひも.i_なし
            Else
                Dim lane As Integer = Math.Round(dLeftAdjust / g_clsSelectBasics.p_d指定本幅(1), MidpointRounding.AwayFromZero * 2) '四捨五入 
                If g_clsSelectBasics.p_i本幅 < lane Then
                    lane = g_clsSelectBasics.p_i本幅
                End If
                If lane = n短い横ひもn本幅 Then
                    rad = enum最上と最下の短いひも.i_同じ幅
                Else
                    .Value("f_i最上と最下の短いひもの幅") = lane
                    rad = enum最上と最下の短いひも.i_異なる幅
                End If
            End If
            .Value("f_i最上と最下の短いひも") = CType(rad, Int16)
            .Value("f_d横ひも間のすき間") = d横ひも間のすき間
            .Value("f_b補強ひも区分") = False
            .Value("f_b斜めの補強ひも区分") = False
        End With

        Return True
    End Function

    '横寸法から縦ひも(横寸法優先)(底楕円が設定されていればその分マイナス)
    Private Function calc_Target_縦()
        Dim d横 As Double = _d横_目標 - _d径の合計 * 2
        Dim n縦ひもn本幅 As Integer = _I基本のひも幅
        Dim tate As Integer = Math.Truncate(d横 / _d基本のひも幅) 'ドット以下切り捨て
        'issue#2
        Dim n縦ひもの本数 = getOddHalf(tate, ((g_clsSelectBasics.p_i本幅 / 2) < _I基本のひも幅))
        If n縦ひもの本数 <= 1 Then
            '横寸法が小さすぎるため縦ひもを置けません。
            p_sメッセージ = My.Resources.CalcNoShortWidth
            Return False
        End If
        '縦ひも分の横寸法
        Dim band As Double = g_clsSelectBasics.p_d指定本幅(n縦ひもn本幅) * n縦ひもの本数
        If d横 <= band Then
            '横寸法が小さすぎるため縦ひもを置けません。
            p_sメッセージ = My.Resources.CalcNoShortWidth
            Return False
        End If
        Dim dひとつのすき間の寸法 As Double = (d横 - band) / (n縦ひもの本数 - 1)

        '***結果をセット
        With _Data.p_row底_縦横
            '縦ひも
            .Value("f_i縦ひも") = n縦ひもn本幅
            .Value("f_i縦ひもの本数") = n縦ひもの本数
            .Value("f_dひとつのすき間の寸法") = dひとつのすき間の寸法
            .Value("f_b始末ひも区分") = True
        End With

        Return True
    End Function

    '高さ
    Private Function calc_Target_高さ()
        If isNear(_d高さ_目標, _d高さの合計) Then
            Return True 'OK
        End If
        If _d高さ_目標 < _d高さの合計 Then
            Return calc_Target_高さ_down()
        End If

        Dim table As tbl側面DataTable = _Data.p_tbl側面
        If table.Rows.Count = 0 Then
            '空の場合のみ縁を追加
            If Not add_any_hem(table) Then
                Return False
            End If
        End If
        '編みかたがなければ追加
        If Not add_any_pattern(table) Then
            Return False
        End If

        '最後の編みかたを得る
        Dim lasttnum As Integer = clsDataTables.LastNumber(table)
        If lasttnum < 1 Then
            '編みかたが設定されていないため高さを算出できません。
            p_sメッセージ = My.Resources.CalcNoPatternRecord
            Return False
        End If
        Dim row As tbl側面Row = clsDataTables.NumberFirstRecord(table, lasttnum)

        Dim ret As Boolean = True
        Dim dLastHeight As Double = _d高さの合計
        Do While ret AndAlso (_d高さの合計 < _d高さ_目標)
            row.f_i周数 = row.f_i周数 + 1
            ret = ret And calc_側面(CalcCategory.Side, row, "f_i周数")
            If dLastHeight >= _d高さの合計 Then
                '編みかた'{0}'の設定を確認してください。
                p_sメッセージ = String.Format(My.Resources.CalcZeroPatternHeight, row.f_s編みかた名)
                Return False
            End If
        Loop
        Return ret
    End Function

    Private Function calc_Target_高さ_down()

        Dim ret As Boolean = True
        Dim table As tbl側面DataTable = _Data.p_tbl側面
        Dim row As tbl側面Row = Nothing

        '最後の編みかた
        Dim lasttnum As Integer = clsDataTables.LastNumber(table)
        If lasttnum < 1 Then
            '編みかたが設定されていないため高さを算出できません。
            p_sメッセージ = My.Resources.CalcNoPatternRecord
            Return False
        End If
        row = clsDataTables.NumberFirstRecord(table, lasttnum)

        Dim dLastHeight As Double = _d高さの合計
        Do While _d高さ_目標 < _d高さの合計 AndAlso 1 < row.f_i周数
            row.f_i周数 = row.f_i周数 - 1
            ret = ret And calc_側面(CalcCategory.Side, row, "f_i周数")
            If dLastHeight <= _d高さの合計 Then
                '編みかた'{0}'の設定を確認してください。
                p_sメッセージ = String.Format(My.Resources.CalcZeroPatternHeight, row.f_s編みかた名)
                Return False
            End If
        Loop

        Return ret
    End Function

    '編みかたがなければ追加する。編みかたの登録がなければTrueを返す
    Private Function add_any_pattern(ByVal table As tbl側面DataTable) As Boolean
        Dim ret As Boolean = True
        Dim firstnum As Integer = clsDataTables.LargerNumber(table, 0)
        If firstnum < 1 Then
            'issues#3
            Dim patts() As String = g_clsMasterTables.GetPatternNames(False, False, True)
            If 0 < patts.Count Then
                Dim any As Integer = New System.Random().Next(patts.Count)
                Dim row As tbl側面Row = Nothing
                ret = ret And add_側面(patts(any), False, _I基本のひも幅, 1, row)
                ret = ret And calc_側面(CalcCategory.Side, row, "f_i周数")
            End If
        End If
        Return ret
    End Function

    '縁がなければ追加する。縁の登録がなければTrueを返す
    Private Function add_any_hem(ByVal table As tbl側面DataTable) As Boolean
        Dim ret As Boolean = True
        If clsDataTables.NumberCount(table, clsDataTables.cHemNumber) = 0 Then
            Dim hems() As String = g_clsMasterTables.GetPatternNames(True, False)
            If 0 < hems.Count Then
                Dim any As Integer = New System.Random().Next(hems.Count)
                Dim row As tbl側面Row = Nothing
                ret = ret And add_側面(hems(any), True, _I基本のひも幅, 1, row)
                ret = ret And calc_側面(CalcCategory.Side, row, "f_i周数")
            End If
        End If
        Return ret
    End Function

    '1/2値の奇数、偶数ならプラス/マイナス1(issue#2)
    Private Shared Function getOddHalf(ByVal num As Integer, ByVal isUp As Boolean) As Integer
        If num <= 0 Then
            Return 0
        ElseIf num <= 2 Then
            Return 1
        End If

        Dim half1 As Integer = num \ 2
        Dim half2 As Integer = num - half1

        If half1 Mod 2 = 1 Then
            Return half1
        ElseIf half2 Mod 2 = 1 Then
            Return half2
        Else
            If isUp Then
                Return half2 + 1
            Else
                Return half2 - 1
            End If
        End If
    End Function

#End Region

#Region "底楕円"
    Function add_底楕円(ByVal nameselect As String,
                 ByVal is差しひも As Boolean,
                 ByVal i何本幅 As Integer, ByVal i周数 As Integer,
                 ByRef row As tbl底_楕円Row) As Boolean

        Dim table As tbl底_楕円DataTable = _Data.p_tbl底_楕円

        Dim addNumber As Integer = clsDataTables.AddNumber(table)
        If addNumber < 0 Then
            '{0}追加用の番号がとれません。
            p_sメッセージ = String.Format(My.Resources.CalcNoAddNumber, text編みかた名())
            Return False
        End If

        If is差しひも Then
            '差しひも
            row = table.Newtbl底_楕円Row
            With row
                .f_i番号 = addNumber
                .f_b差しひも区分 = True
                .f_i差しひも本数 = i周数
                .f_s編みかた名 = "(" & text差しひも() & ")"
                .f_s編みひも名 = text差しひも()
                .f_iひも番号 = 0
                .f_i何本幅 = i何本幅
                .f_dひも長加算 = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d差しひも長加算初期値")
                .Setf_i周数Null()
            End With
            table.Rows.Add(row)
            Return True

        Else
            '編みひも
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

            'tbl編みかたぶんのレコード
            Dim groupRow As New clsGroupDataRow("f_iひも番号")
            For Each idx As Int16 In grpMst.Keys
                row = table.Newtbl底_楕円Row
                row.f_i番号 = addNumber
                row.f_iひも番号 = idx
                row.f_b差しひも区分 = False
                row.Setf_i差しひも本数Null()
                row.f_s編みかた名 = nameselect
                row.f_i周数 = i周数

                groupRow.Add(row)
                table.Rows.Add(row)
            Next
            groupRow.SetNameIndexValue("f_s編みひも名", grpMst)
            groupRow.SetNameIndexValue("f_b周連続区分", grpMst)
            groupRow.SetNameIndexValue("f_dひも長加算", grpMst, "f_dひも長加算初期値")
            groupRow.SetNameIndexValue("f_sメモ", grpMst, "f_s備考")
            groupRow.SetNameIndexValue("f_b集計対象外区分", grpMst, "f_b集計対象外区分初期値")

            '2本幅が普通(#18)
            If groupRow.Count = 1 Then
                groupRow.SetNameValue("f_i何本幅", 2)
                groupRow.SetNameValue("f_b次周連続区分", True)
            Else
                For Each drow As clsDataRow In groupRow
                    Dim mst As New clsOptionDataRow(grpMst.IndexDataRow(drow)) '必ずある
                    drow.Value("f_i何本幅") = mst.GetFirstLane(i何本幅)
                Next
            End If

            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "Oval Add: {0}", groupRow.ToString)
            Return True
        End If
    End Function

    '更新処理が必要なフィールド名
    Shared _fields底楕円() As String = {"f_i何本幅", "f_i周数", "f_i差しひも本数", "f_b周連続区分", "f_b集計対象外区分", "f_b次周連続区分"}
    Shared Function IsDataPropertyName底楕円(ByVal name As String) As Boolean
        Return _fields底楕円.Contains(name)
    End Function

    'IN:   _d縦横の垂直ひも間の周 _d最上と最下の短いひもの幅
    'OUT:  _d径の合計    _i垂直ひも数_楕円  _d底の周
    Private Function calc_底楕円(ByVal category As CalcCategory, ByVal row As tbl底_楕円Row, ByVal dataPropertyName As String) As Boolean
        If _Data.p_tbl底_楕円.Rows.Count = 0 Then
            'レコードがなければ四角
            _d径の合計 = 0
            _i垂直ひも数_楕円 = 0
            _d底の周 = _d縦横の垂直ひも間の周 _
                + 4 * _d最上と最下の短いひもの幅 _
                + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分")
            Return True
        End If

        'レコードあり
        Dim ret As Boolean = True
        '1回目:レコード個々
        If category <> CalcCategory.Oval Then
            'マスター変更もしくは派生更新
            For Each r As tbl底_楕円Row In _Data.p_tbl底_楕円
                ret = ret And set_row底楕円(r)
            Next

        Else
            If row IsNot Nothing Then
                '追加もしくは更新
                If dataPropertyName = "f_i周数" Then
                    '同じ番号＆編みかた名のレコード　※差しひも数はレコード1点のみ
                    Dim i周数 As Integer = row.f_i周数
                    Dim cond As String = String.Format("f_i番号 = {0} AND f_s編みかた名 = '{1}'", row.f_i番号, row.f_s編みかた名)
                    For Each r As tbl底_楕円Row In _Data.p_tbl底_楕円.Select(cond)
                        r.f_i周数 = i周数
                        ret = ret And set_row底楕円(r)
                    Next
                ElseIf dataPropertyName = "f_b集計対象外区分" Then
                    If row.f_b集計対象外区分 Then
                        row.f_b次周連続区分 = False
                    End If

                ElseIf dataPropertyName = "f_b次周連続区分" Then
                    If row.f_b次周連続区分 Then
                        row.f_b集計対象外区分 = False
                    End If

                Else
                    'そのレコード
                    ret = ret And set_row底楕円(row)
                End If
            Else
                '削除・移動
            End If
        End If
        '径は確定したので合計が得られる
        _d径の合計 = compute指定以降の径の合計(0)

        '2回目:番号順
        Dim d楕円底円弧の半径加算 As Double
        Dim d楕円底周の加算 As Double
        If _Data.p_row底_縦横.Value("f_b楕円底個別設定") Then
            d楕円底円弧の半径加算 = _Data.p_row底_縦横.Value("f_d楕円底円弧の半径加算")
            d楕円底周の加算 = _Data.p_row底_縦横.Value("f_d楕円底周の加算")
        Else
            d楕円底円弧の半径加算 = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底円弧の半径加算")
            d楕円底周の加算 = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d楕円底周の加算")
        End If
        Dim lastNum差しひも As Integer = 0
        Dim prv As tbl底_楕円Row = Nothing
        For Each r As tbl底_楕円Row In _Data.p_tbl底_楕円.Select(Nothing, "f_i番号 ASC ,  f_iひも番号 ASC")
            ret = ret And set_row底楕円_2回目(d楕円底円弧の半径加算, d楕円底周の加算, r, prv, lastNum差しひも)
            prv = r
        Next

        '最後の周長
        _d底の周 = prv.f_d周長 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の楕円底周の増分")

        '差しひも数の合計
        _i垂直ひも数_楕円 = prv.f_i差しひも累計

        Return ret
    End Function

    '※径をセット後に有効
    Private Function compute指定以降の径の合計(ByVal i番号 As Integer) As Double
        Dim cond As String = String.Format("({0} <= f_i番号) AND (f_d径 IS NOT NULL)", i番号)
        Dim obj As Object = _Data.p_tbl底_楕円.Compute("SUM(f_d径)", cond)
        If IsDBNull(obj) OrElse obj < 1 Then
            Return 0
        Else
            Return obj
        End If
    End Function


    'IN:    
    'OUT:   
    Private Function set_row底楕円(ByVal row As tbl底_楕円Row) As Boolean
        If row.f_b差しひも区分 Then
            '差しひも
            row.Setf_i周数Null()
            row.Setf_i段数Null()
            row.f_iひも本数 = row.f_i差しひも本数
            row.f_d径 = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d差しひもの径")
            row.f_b次周連続区分 = False
            row.f_b周連続区分 = False
        Else
            '編みひも
            row.Setf_i差しひも本数Null()

            ' tbl編みかたRow
            Dim mst As clsPatternDataRow = g_clsMasterTables.GetPatternRecord(row.f_s編みかた名, row.f_iひも番号)
            If Not mst.IsValid Then
                'なし
                row.Setf_i段数Null()
                row.Setf_d径Null()
                row.Setf_dひも長Null()
                row.Setf_iひも本数Null()
                '{0}の番号{1}で設定にない編みかた名'{2}'(ひも番号{3})が参照されています。
                p_sメッセージ = String.Format(My.Resources.CalcNoMasterPattern, text底楕円(), row.f_i番号, row.f_s編みかた名, row.f_iひも番号)
                row.f_bError = True
                Return False

            Else
                '指定本幅
                row.f_i段数 = row.f_i周数 * mst.Value("f_i周あたり段数")
                row.f_d径 = row.f_i周数 * mst.GetDiameter(row.f_i何本幅)

                If row.f_b周連続区分 Then
                    row.f_iひも本数 = mst.Value("f_iひも数")
                Else
                    row.f_iひも本数 = row.f_i周数 * mst.Value("f_iひも数")
                End If

            End If
        End If
        Return True
    End Function

    'IN:    _d縦横の垂直ひも間の周  _d最上と最下の短いひもの幅  _d径の合計  _i垂直ひも数_縦横
    'OUT:   
    Private Function set_row底楕円_2回目(ByVal d楕円底円弧の半径加算 As Double, ByVal d楕円底周の加算 As Double,
                                    ByVal row As tbl底_楕円Row, ByVal prv As tbl底_楕円Row, ByRef lastNum差しひも As Integer) As Boolean
        Dim d円弧長 As Double = 0
        Dim i角の差しひも数 As Integer
        '最初のレコード
        If prv Is Nothing Then
            If row.Isf_d径Null Then
                row.f_d径の累計 = 0
                row.f_d周長 = 4 * _d最上と最下の短いひもの幅 + _d縦横の垂直ひも間の周 + d楕円底周の加算
            Else
                row.f_d径の累計 = row.f_d径
                d円弧長 = 2 * System.Math.PI * (row.f_d径の累計 + d楕円底円弧の半径加算) _
                    + System.Math.PI * _d最上と最下の短いひもの幅
                row.f_d周長 = d円弧長 + _d縦横の垂直ひも間の周 + d楕円底周の加算
            End If
            row.f_d円弧部分長 = d円弧長 / 4

            If row.f_b差しひも区分 Then
                '差しひも
                row.f_i差しひも累計 = row.f_i差しひも本数
                i角の差しひも数 = row.f_i差しひも累計 / 4
                If i角の差しひも数 = 0 Then
                    row.Setf_d差しひも間のすき間Null()
                Else
                    row.f_d差しひも間のすき間 = (row.f_d円弧部分長 - (i角の差しひも数 * g_clsSelectBasics.p_d指定本幅(row.f_i何本幅))) / (i角の差しひも数 + 1)
                End If
                'ひも長は底部分のみをセット
                row.f_dひも長 = _d径の合計 _
                    + _d最上と最下の短いひもの幅 _
                    + g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
                lastNum差しひも = row.f_i番号
            Else
                '編みひも
                row.f_i差しひも累計 = 0
                row.Setf_d差しひも間のすき間Null()

                ' tbl編みかたRow
                Dim mst As clsPatternDataRow = g_clsMasterTables.GetPatternRecord(row.f_s編みかた名, row.f_iひも番号)
                If Not mst.IsValid Then
                    Return True '1回目でNullセット済

                Else
                    '(issue#5)楕円底の場合は、ひも1幅は自身の本幅
                    If row.f_b周連続区分 Then
                        row.f_dひも長 = mst.GetContinuoutBandLength(row.f_i何本幅, row.f_d周長, _i垂直ひも数_縦横, row.f_i周数)
                    Else
                        row.f_dひも長 = mst.GetBandLength(row.f_i何本幅, row.f_d周長, _i垂直ひも数_縦横)
                    End If
                End If

            End If
            Return True
        End If '最初のレコード

        '2番目以降のレコード
        If row.Isf_d径Null Then
            row.f_d径の累計 = prv.f_d径の累計
        Else
            row.f_d径の累計 = row.f_d径 + prv.f_d径の累計
        End If
        d円弧長 = 2 * System.Math.PI * (row.f_d径の累計 + d楕円底円弧の半径加算) _
             + System.Math.PI * _d最上と最下の短いひもの幅
        row.f_d周長 = d円弧長 + _d縦横の垂直ひも間の周 + d楕円底周の加算
        row.f_d円弧部分長 = d円弧長 / 4

        If row.f_b差しひも区分 Then
            '差しひも
            row.f_i差しひも累計 = prv.f_i差しひも累計 + row.f_i差しひも本数
            'ひも長は底部分のみをセット
            row.f_dひも長 = compute指定以降の径の合計(lastNum差しひも) _
                + _d最上と最下の短いひもの幅 _
                + g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
            lastNum差しひも = row.f_i番号

        Else
            '編みひも
            row.f_i差しひも累計 = prv.f_i差しひも累計

            ' tbl編みかたRow
            Dim mst As clsPatternDataRow = g_clsMasterTables.GetPatternRecord(row.f_s編みかた名, row.f_iひも番号)
            If mst.IsValid Then
                '(issue#5)楕円底の場合は、ひも1幅は自身の本幅
                If row.f_b周連続区分 Then
                    row.f_dひも長 = mst.GetContinuoutBandLength(row.f_i何本幅, row.f_d周長, _i垂直ひも数_縦横 + row.f_i差しひも累計, row.f_i周数)
                Else
                    row.f_dひも長 = mst.GetBandLength(row.f_i何本幅, row.f_d周長, _i垂直ひも数_縦横)
                End If
            Else
                '1回目で処理済
            End If
        End If '2番目以降のレコード

        i角の差しひも数 = row.f_i差しひも累計 / 4
        If i角の差しひも数 = 0 Then
            row.Setf_d差しひも間のすき間Null()
        Else
            row.f_d差しひも間のすき間 = (row.f_d円弧部分長 - (i角の差しひも数 * g_clsSelectBasics.p_d指定本幅(row.f_i何本幅))) / (i角の差しひも数 + 1)
        End If
        Return True
    End Function


#End Region

#Region "側面"

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
    Shared _fields側面() As String = {"f_i何本幅", "f_i周数", "f_b周連続区分", "f_d周長比率対底の周", "f_d周長", "f_b集計対象外区分", "f_b次周連続区分"}
    Shared Function IsDataPropertyName側面(ByVal name As String) As Boolean
        Return _fields側面.Contains(name)
    End Function

    'IN:    _d底の周
    'OUT:   _d高さの合計 垂直ひもの合計 _d周の最大値 _d周の最小値 _d厚さの最大値
    Private Function calc_側面(ByVal category As CalcCategory, ByVal row As tbl側面Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category <> CalcCategory.Side Then
            'マスター変更もしくは派生更新
            ret = ret And recalc_側面()

        Else
            If row IsNot Nothing Then
                '#65
                If dataPropertyName = "f_d周長" Then
                    row.f_d周長比率対底の周 = row.f_d周長 / _d底の周

                ElseIf dataPropertyName = "f_b集計対象外区分" Then
                    If row.f_b集計対象外区分 Then
                        row.f_b次周連続区分 = False
                    End If

                ElseIf dataPropertyName = "f_b次周連続区分" Then
                    If row.f_b次周連続区分 Then
                        row.f_b集計対象外区分 = False
                    End If
                End If

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
            _d高さの合計 = 0
        Else
            _d高さの合計 = obj
        End If

        '垂直ひもの合計
        Dim obj2 As Object = _Data.p_tbl側面.Compute("SUM(f_d垂直ひも長)", "f_d垂直ひも長 IS NOT NULL")
        If IsDBNull(obj2) OrElse obj2 < 0 Then
            _d垂直ひも長合計 = 0
        Else
            _d垂直ひも長合計 = obj2
        End If

        '周の最大値
        Dim obj3 As Object = _Data.p_tbl側面.Compute("MAX(f_d周長)", "f_d周長 IS NOT NULL")
        If IsDBNull(obj3) OrElse obj3 < 0 Then
            _d周の最大値 = -1
        Else
            _d周の最大値 = obj3
        End If

        '厚さの最大値
        Dim obj4 As Object = _Data.p_tbl側面.Compute("MAX(f_d厚さ)", "f_d厚さ IS NOT NULL")
        If IsDBNull(obj4) OrElse obj4 < 0 Then
            _d厚さの最大値 = 0
        Else
            _d厚さの最大値 = obj4
        End If

        '周の最小値
        Dim obj5 As Object = _Data.p_tbl側面.Compute("MIN(f_d周長)", "f_d周長 IS NOT NULL")
        If IsDBNull(obj5) OrElse obj5 < 0 Then
            _d周の最小値 = -1
        Else
            _d周の最小値 = obj5
        End If

        ret = ret And recalc_側面_高さ比率()
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

    'IN:    _d底の周    p_i垂直ひも数
    'OUT:
    Private Function set_groupRow側面(ByVal groupRow As clsGroupDataRow) As Boolean
        '周数は一致項目
        Dim i周数 As Integer = groupRow.GetNameValue("f_i周数")

        'マスタ参照なし
        For Each drow As clsDataRow In groupRow
            drow.Value("f_d周長") = _d底の周 * drow.Value("f_d周長比率対底の周")
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
            p_sメッセージ = String.Format(My.Resources.CalcNoMasterPattern, text側面(), groupRow.GetNameValue("f_i番号"), groupRow.GetNameValue("f_s編みかた名"), groupRow.GetNameValue("f_iひも番号"))
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
                    p_sメッセージ = String.Format(My.Resources.CalcNoMasterPattern, text側面(), drow.Value("f_i番号"), drow.Value("f_s編みかた名"), drow.Value("f_iひも番号"))
                    ret = False

                End If
                drow.Value("f_d高さ比率") = DBNull.Value
            Next
            Return ret
        End If

    End Function

    'issue#9 高さ比率をセット 
    'IN:    _d高さの合計    
    Private Function recalc_側面_高さ比率() As Boolean
        If _d高さの合計 < 0 Then
            Return False
        End If

        Dim res = (From row As tbl側面Row In _Data.p_tbl側面
                   Select Num = row.f_i番号
                   Order By Num).Distinct

        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")
            'Dim dGroup高さ As Double = group高さ_側面(groupRow)
            Dim dGroup高さ As Double = groupRow.GetNameValueSum("f_d高さ")
            Dim drow As clsDataRow = groupRow.IndexDataRow(1)
            If drow IsNot Nothing Then
                drow.Value("f_d高さ比率") = dGroup高さ / _d高さの合計
            End If
        Next

        Return True
    End Function

#End Region

#Region "縦横展開"

    '縦横の展開DataTable
    '　非展開時 :タブ非表示。加算・色なし、固定幅。底(縦横)全てを反映した状態で保持
    '　展開時   :タブに表示。底(縦横)の変更時、既存レコードは保持
    Dim __tbl横展開 As tbl縦横展開DataTable 'New時に作成、以降は存在が前提
    Dim __tbl縦展開 As tbl縦横展開DataTable 'New時に作成、以降は存在が前提

    '横ひも: enumひも種: i_横 + i_長い,i_短い,i_最上と最下,i_補強,
    '縦ひも: enumひも種: i_縦 + i_補強 / i_斜め+i_補強

    '編集中の値を_Data.p_tbl縦横展開 に反映させる(色編集時)
    Function prepare縦横展開DataTable() As Boolean
        Try
            Dim yokotable As tbl縦横展開DataTable = get横展開DataTable()
            _Data.FromTmpTable(enumひも種.i_横, yokotable)

            Dim tatetable As tbl縦横展開DataTable = get縦展開DataTable()
            _Data.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, tatetable)

            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "prepare縦横展開DataTable")
            Return False
        End Try
    End Function

    '底(縦横)の本幅値に合わせる
    Function sync_展開本幅() As Boolean
        Dim rows() As tbl縦横展開Row

        '長い横ひも
        rows = __tbl横展開.Select(String.Format("f_iひも種={0}", CType(enumひも種.i_横 Or enumひも種.i_長い, Integer)))
        For Each row As tbl縦横展開Row In rows
            row.f_i何本幅 = _Data.p_row底_縦横.Value("f_i長い横ひも")
        Next
        '短い横ひも
        rows = __tbl横展開.Select(String.Format("f_iひも種={0}", CType(enumひも種.i_横 Or enumひも種.i_短い, Integer)))
        For Each row As tbl縦横展開Row In rows
            row.f_i何本幅 = _Data.p_row底_縦横.Value("f_i短い横ひも")
        Next
        '最上と最下
        rows = __tbl横展開.Select(String.Format("f_iひも種={0}", CType(enumひも種.i_横 Or enumひも種.i_最上と最下, Integer)))
        For Each row As tbl縦横展開Row In rows
            row.f_i何本幅 = _i最上と最下の横ひも何本幅
        Next
        '補強ひも
        rows = __tbl横展開.Select(String.Format("f_iひも種={0}", CType(enumひも種.i_横 Or enumひも種.i_最上と最下, Integer)))
        For Each row As tbl縦横展開Row In rows
            If 0 < _i最上と最下の横ひも何本幅 Then
                row.f_i何本幅 = _i最上と最下の横ひも何本幅
            Else
                row.f_i何本幅 = _Data.p_row底_縦横.Value("f_i長い横ひも")
            End If
        Next
        adjust_横ひも()
        _Data.FromTmpTable(enumひも種.i_横, __tbl横展開)


        '縦縦ひも
        rows = __tbl縦展開.Select(String.Format("f_iひも種={0}", CType(enumひも種.i_縦, Integer)))
        For Each row As tbl縦横展開Row In rows
            row.f_i何本幅 = _Data.p_row底_縦横.Value("f_i縦ひも")
        Next
        '始末ひも
        rows = __tbl縦展開.Select(String.Format("f_iひも種={0}", CType(enumひも種.i_縦 Or enumひも種.i_補強, Integer)))
        For Each row As tbl縦横展開Row In rows
            row.f_i何本幅 = _Data.p_row底_縦横.Value("f_i縦ひも")
        Next
        '斜めの補強ひも
        rows = __tbl縦展開.Select(String.Format("f_iひも種={0}", CType(enumひも種.i_斜め Or enumひも種.i_補強, Integer)))
        For Each row As tbl縦横展開Row In rows
            row.f_i何本幅 = _I基本のひも幅
        Next
        adjust_縦ひも()
        _Data.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, __tbl縦展開)

        Return calc_集計値(True, True)
    End Function

    '集計値更新
    Function calc_集計値(ByVal is横展開 As Boolean, ByVal is縦展開 As Boolean) As Boolean

        If is横展開 Then
            _d縦横の縦 = 0
            Dim obj As Object = __tbl横展開.Compute("SUM(f_d幅)", Nothing)
            If Not IsDBNull(obj) AndAlso 0 < obj Then
                _d縦横の縦 = obj
            End If
        End If

        If is縦展開 Then
            _d縦横の横 = 0
            Dim obj As Object = __tbl縦展開.Compute("SUM(f_d幅)", Nothing)
            If Not IsDBNull(obj) AndAlso 0 < obj Then
                _d縦横の横 = obj
            End If
        End If

        _d縦横の垂直ひも間の周 = _d縦横の横 * 2 +
                                (_d縦横の縦 - (2 * _d最上と最下の短いひもの幅)) * 2
        Return True
    End Function

#Region "横"
    '底(縦横)設定に基づき縦展開テーブルを作り直す(レコード数増減＆Fix)
    'isRefSaved: True=_Data.p_tbl縦横展開を反映する False=反映しない(非展開値)
    Function renew横展開DataTable(ByVal isRefSaved As Boolean) As Boolean
        If Not isRefSaved Then
            __tbl横展開.Clear()
        Else
            For Each r As tbl縦横展開Row In __tbl横展開.Rows
                r.f_iVal1 = 1 '存在のフラグ値
            Next
        End If
        __tbl横展開.AcceptChanges()


        'f_iVal1を一時的なフラグとして使用
        Dim row As tbl縦横展開Row

        'f_dひも長加算,f_dひも長加算2,f_s色は既定値
        With _Data.p_row底_縦横
            Dim posyoko As Integer = 1

            If 0 < _i長い横ひもの本数 Then

                Dim n異なる幅の位置 As Integer = -1 '最上と最下
                Dim n短い横ひもの開始位置 As Integer
                If 0 < .Value("f_i短い横ひも") Then
                    n短い横ひもの開始位置 = posyoko + 1 '長い横ひもの次
                Else
                    n短い横ひもの開始位置 = -1 '短い横ひもなし
                End If

                '最上・最下は変更不可のため常に別扱い
                If 0 < _i最上と最下の横ひも何本幅 Then
                    n異なる幅の位置 = posyoko
                    posyoko += 1 '空ける
                    n短い横ひもの開始位置 += 1 'ひとつずれる(-1→0)
                End If

                For idx As Integer = 1 To _i長い横ひもの本数 'n長い横ひもの数
                    row = Find縦横展開Row(__tbl横展開, enumひも種.i_横 Or enumひも種.i_長い, idx, True)

                    row.f_i位置番号 = posyoko
                    row.f_sひも名 = text長い横ひも()
                    row.f_i何本幅 = .Value("f_i長い横ひも")
                    adjust_横ひも(row, Nothing)

                    row.f_iVal1 = 0 'used
                    If 0 < n短い横ひもの開始位置 Then
                        posyoko += 2
                    Else
                        posyoko += 1
                    End If
                Next

                If 0 < n短い横ひもの開始位置 Then
                    posyoko = n短い横ひもの開始位置
                    For idx As Integer = 1 To _i短い横ひもの本数 'n短い横ひもの数
                        row = Find縦横展開Row(__tbl横展開, enumひも種.i_横 Or enumひも種.i_短い, idx, True)

                        row.f_i位置番号 = posyoko
                        row.f_sひも名 = text短い横ひも()
                        row.f_i何本幅 = .Value("f_i短い横ひも")
                        adjust_横ひも(row, Nothing)

                        row.f_iVal1 = 0 'used
                        posyoko += 2
                    Next
                End If

                If 0 < n異なる幅の位置 Then
                    For idx As Integer = 1 To 2
                        row = Find縦横展開Row(__tbl横展開, enumひも種.i_横 Or enumひも種.i_最上と最下, idx, True)

                        If idx = 1 Then
                            row.f_i位置番号 = n異なる幅の位置 '空けておいた番号
                        Else
                            row.f_i位置番号 = posyoko
                            posyoko += 1
                        End If
                        row.f_iひも番号 = idx
                        row.f_sひも名 = text最上と最下の短いひも()
                        row.f_i何本幅 = _i最上と最下の横ひも何本幅 ' .Value("f_i最上と最下の短いひもの幅")
                        adjust_横ひも(row, Nothing)

                        row.f_iVal1 = 0 'used
                    Next
                End If
            End If

            '#48 長短セットで奇数
            Dim shift As Integer = posyoko \ 2
            For Each row In __tbl横展開.Rows
                row.f_i位置番号 -= shift
            Next

            '以降は裏側
            posyoko = cBackPosition
            If .Value("f_b補強ひも区分") Then
                For idx As Integer = 1 To 2
                    row = Find縦横展開Row(__tbl横展開, enumひも種.i_横 Or enumひも種.i_補強, idx, True)

                    row.f_i位置番号 = posyoko
                    row.f_sひも名 = text補強ひも()
                    adjust_横ひも(row, Nothing)

                    '重なるひもの幅に合わせる
                    If 0 < _i最上と最下の横ひも何本幅 Then
                        row.f_i何本幅 = _i最上と最下の横ひも何本幅
                    Else
                        row.f_i何本幅 = .Value("f_i長い横ひも")
                    End If

                    row.f_iVal1 = 0 'used
                    posyoko += 1
                Next
            End If
        End With

        'not used
        For Each row In __tbl横展開.Rows
            If row.f_iVal1 <> 0 Then
                row.Delete()
            End If
        Next
        __tbl横展開.AcceptChanges()

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_横, __tbl横展開)
            adjust_横ひも()
            __tbl横展開.AcceptChanges()
        End If

        Return True
    End Function

    '現編集内容をそのまま渡す。isReset指定時は初期化
    Friend Function get横展開DataTable(Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        If isReset Then
            _Data.Removeひも種Rows(enumひも種.i_横)
            renew横展開DataTable(False) '既存反映なし・サイズは同じ
        End If
        Return __tbl横展開
    End Function

    'CalcSizeからの呼び出し 
    '   Expand_Yoko: 対応レコードの派生更新(画面で編集中のみ)   
    '   以外:   __tbl横展開 テーブルごと更新
    'IN:_b縦横側面を展開する,_d最上と最下の短いひもの幅
    'OUT:_d縦横の縦 _d縦横の垂直ひも間の周
    Function calc_横ひも展開(ByVal category As CalcCategory, ByVal row As tbl縦横展開Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category = CalcCategory.Expand_Yoko AndAlso row IsNot Nothing Then
            'テーブル編集中
            ret = adjust_横ひも(row, dataPropertyName)
        Else
            '横ひもを再セット
            ret = renew横展開DataTable(_b縦横を展開する)
        End If

        '集計値更新
        Return calc_集計値(True, False) 'is横展開
    End Function

    '横ひもの全レコードの幅と長さをセット
    Function adjust_横ひも() As Boolean
        Dim ret As Boolean = True
        For Each row As tbl縦横展開Row In __tbl横展開
            If Not adjust_横ひも(row, Nothing) Then
                ret = False
            End If
        Next
        Return ret
    End Function

    '横ひもの指定レコードの幅と長さを計算
    'Ref:   _d横ひも間のすき間,_d垂直ひも長加算,_i長い横ひもの本数,_d最上と最下の短いひもの幅
    'IN:    _d縦横の横,_d径の合計,_d垂直ひも長合計
    'OUT:   各レコードのf_d幅,f_d長さ,f_dひも長,f_d出力ひも長
    Function adjust_横ひも(ByVal row As tbl縦横展開Row, ByVal dataPropertyName As String) As Boolean
        If Not String.IsNullOrEmpty(dataPropertyName) Then
            'セル編集操作時
            If dataPropertyName = "f_s色" Then
                Return True
            End If
        End If

        '●セットする値 位置順(下方向)
        'f_d幅   :領域の幅(f_i何本幅分+ひもの下に加えるすき間)　→　この合計が_d縦横の縦
        'f_d長さ :_d縦横の横
        'f_dひも長:(短い横ひも)_d縦横の横 - f_d短い横ひも長のばらつき
        '          (長い横ひも)_d縦横の横 + 2*(底と高さ分の長さ)
        'f_d出力ひも長:f_dひも長 + f_dひも長加算 + f_dひも長加算2
        '
        'f_dVal1 = ひも幅(f_i何本幅)
        'f_dVal2 = 左半分の出力ひも長
        'f_dVal3 = 右半分の出力ひも長

        row.f_dVal1 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
        If row.f_iひも種 = (enumひも種.i_横 Or enumひも種.i_長い) Then
            If _i最上と最下の横ひも何本幅 = 0 AndAlso row.f_iひも番号 = _i長い横ひもの本数 Then
                row.f_d幅 = row.f_dVal1
            Else
                row.f_d幅 = row.f_dVal1 + _d横ひも間のすき間
            End If
            row.f_d長さ = _d縦横の横
            row.f_dひも長 = _d縦横の横 + 2 * (_d径の合計 + _d垂直ひも長合計 + _d垂直ひも長加算)
            row.f_dVal2 = (row.f_dひも長 / 2) + row.f_dひも長加算
            row.f_dVal3 = (row.f_dひも長 / 2) + row.f_dひも長加算2
            row.f_d出力ひも長 = row.f_dVal2 + row.f_dVal3

        ElseIf row.f_iひも種 = (enumひも種.i_横 Or enumひも種.i_短い) Then
            '最下にはならない
            row.f_d幅 = row.f_dVal1 + _d横ひも間のすき間

            row.f_d長さ = _d縦横の横
            row.f_dひも長 = _d縦横の横 - g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d短い横ひも長のばらつき")
            row.f_dVal2 = (row.f_dひも長 / 2) + row.f_dひも長加算
            row.f_dVal3 = (row.f_dひも長 / 2) + row.f_dひも長加算2
            row.f_d出力ひも長 = row.f_dVal2 + row.f_dVal3

        ElseIf row.f_iひも種 = (enumひも種.i_横 Or enumひも種.i_最上と最下) Then
            '何本幅変更不可
            row.f_i何本幅 = _i最上と最下の横ひも何本幅
            row.f_dVal1 = g_clsSelectBasics.p_d指定本幅(_i最上と最下の横ひも何本幅)

            If Not String.IsNullOrEmpty(dataPropertyName) AndAlso dataPropertyName = "f_i何本幅" Then
                '{0}は [{1}] で変更してください。
                Dim msg As String = String.Format(My.Resources.CalcMsgTopBottomBand, text最上と最下の短いひも(), text底縦横)
                MessageBox.Show(msg, _frmMain.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return True
            End If
            If row.f_iひも番号 = 2 Then
                row.f_d幅 = row.f_dVal1
            Else
                row.f_d幅 = row.f_dVal1 + _d横ひも間のすき間
            End If
            row.f_d長さ = _d縦横の横
            row.f_dひも長 = _d縦横の横 - g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d短い横ひも長のばらつき")
            row.f_dVal2 = (row.f_dひも長 / 2) + row.f_dひも長加算
            row.f_dVal3 = (row.f_dひも長 / 2) + row.f_dひも長加算2
            row.f_d出力ひも長 = row.f_dVal2 + row.f_dVal3

        ElseIf row.f_iひも種 = (enumひも種.i_横 Or enumひも種.i_補強) Then
            row.f_d幅 = 0
            row.f_d長さ = _d縦横の横
            row.f_dひも長 = _d縦横の横
            row.f_d出力ひも長 = row.f_dひも長 + row.f_dひも長加算 + row.f_dひも長加算2
            'f_dVal2,f_dVal3,f_dVal4は使いません

        End If

        Return True
    End Function


#End Region

#Region "縦"
    '底(縦横)設定に基づき縦展開テーブルを作り直す(レコード数増減＆Fix)
    'isRefSaved: True=_Data.p_tbl縦横展開を反映する False=反映しない(非展開値)
    Function renew縦展開DataTable(ByVal isRefSaved As Boolean) As Boolean
        If Not isRefSaved Then
            __tbl縦展開.Clear()
        Else
            For Each r As tbl縦横展開Row In __tbl縦展開.Rows
                r.f_iVal1 = 1 '存在のフラグ値
            Next
        End If
        __tbl縦展開.AcceptChanges()


        'f_iVal1を一時的なフラグとして使用
        Dim row As tbl縦横展開Row

        With _Data.p_row底_縦横
            '#48 位置番号
            Dim postate As Integer = -_i縦ひもの本数 \ 2
            Dim skipzero As Boolean = (_i縦ひもの本数 Mod 2) = 0
            '.Value("f_i縦ひも") は1以上
            If 0 < _i縦ひもの本数 Then
                For idx As Integer = 1 To _i縦ひもの本数
                    row = Find縦横展開Row(__tbl縦展開, enumひも種.i_縦, idx, True)

                    row.f_i位置番号 = postate
                    row.f_sひも名 = text縦ひも()
                    row.f_i何本幅 = .Value("f_i縦ひも")
                    adjust_縦ひも(row, Nothing)

                    row.f_iVal1 = 0 'used
                    postate += 1
                    If skipzero AndAlso (postate = 0) Then
                        postate += 1
                    End If
                Next
            End If

            '以降は裏側
            postate = cBackPosition
            If .Value("f_b始末ひも区分") Then
                For idx As Integer = 1 To 2
                    row = Find縦横展開Row(__tbl縦展開, enumひも種.i_縦 Or enumひも種.i_補強, idx, True)
                    row.f_i位置番号 = postate
                    row.f_sひも名 = text始末ひも()
                    row.f_i何本幅 = .Value("f_i縦ひも")
                    adjust_縦ひも(row, Nothing)

                    row.f_iVal1 = 0 'used
                    postate += 1
                Next
            End If

            If .Value("f_b斜めの補強ひも区分") Then
                For idx As Integer = 1 To 2
                    row = Find縦横展開Row(__tbl縦展開, enumひも種.i_斜め Or enumひも種.i_補強, idx, True)

                    row.f_i位置番号 = postate
                    row.f_sひも名 = text斜めの補強ひも()
                    row.f_i何本幅 = _I基本のひも幅
                    adjust_縦ひも(row, Nothing)

                    row.f_iVal1 = 0 'used
                    postate += 1
                Next
            End If

        End With

        'not used
        For Each row In __tbl縦展開.Rows
            If row.f_iVal1 <> 0 Then
                row.Delete()
            End If
        Next
        __tbl縦展開.AcceptChanges()

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, __tbl縦展開)
            adjust_縦ひも()
            __tbl縦展開.AcceptChanges()
        End If

        Return True
    End Function

    '現編集内容をそのまま渡す。isReset指定時は初期化
    Friend Function get縦展開DataTable(Optional ByVal isReset As Boolean = False) As tbl縦横展開DataTable
        If isReset Then
            _Data.Removeひも種Rows(enumひも種.i_縦 Or enumひも種.i_斜め)
            renew縦展開DataTable(False) '既存反映なし・サイズは同じ
        End If
        Return __tbl縦展開
    End Function

    'CalcSizeからの呼び出し 
    '   Expand_Tate: 対応レコードの派生更新(画面で編集中のみ)   
    '   以外:   _tbl縦展開 テーブルごと更新
    'IN:_b縦横側面を展開する,_d最上と最下の短いひもの幅
    'OUT:_d縦横の横 _d縦横の垂直ひも間の周
    Function calc_縦ひも展開(ByVal category As CalcCategory, ByVal row As tbl縦横展開Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category = CalcCategory.Expand_Tate AndAlso row IsNot Nothing Then
            'テーブル編集中
            ret = adjust_縦ひも(row, dataPropertyName)
        Else
            '縦ひもを再セット
            ret = renew縦展開DataTable(_b縦横を展開する)
        End If

        '集計値更新
        Return calc_集計値(False, True) 'is縦展開
    End Function

    '縦ひもの全レコードの幅と長さをセット
    Function adjust_縦ひも() As Boolean
        Dim ret As Boolean = True
        For Each row As tbl縦横展開Row In __tbl縦展開
            If Not adjust_縦ひも(row, Nothing) Then
                ret = False
            End If
        Next
        Return ret
    End Function

    '縦ひもの指定レコードの幅と長さをセット
    'Ref:   _dひとつのすき間の寸法,_d垂直ひも長加算,_i縦ひもの本数
    'IN:    _d縦横の縦,_d径の合計,_d垂直ひも長合計,_d縦横の横(クロス値)
    'OUT:   各レコードのf_d幅,f_d長さ,f_dひも長,f_d出力ひも長
    Function adjust_縦ひも(ByVal row As tbl縦横展開Row, ByVal dataPropertyName As String) As Boolean
        If Not String.IsNullOrEmpty(dataPropertyName) Then
            'セル編集操作時
            If dataPropertyName = "f_s色" Then
                Return True
            End If
        End If

        '●セットする値　位置順(右方向)
        'f_d幅   :領域の幅(f_i何本幅分+ひもの右に加えるすき間)　→　この合計が_d縦横の横
        'f_d長さ :_d縦横の縦
        'f_dひも長:(縦ひも)_d縦横の縦 + 2*(底と高さ分の長さ)
        'f_d出力ひも長:f_dひも長 + f_dひも長加算 + f_dひも長加算2
        '
        'f_dVal1 = ひも幅(f_i何本幅)
        'f_dVal2 = 下半分の出力ひも長
        'f_dVal3 = 上半分の出力ひも長

        row.f_dVal1 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
        If row.f_iひも種 = enumひも種.i_縦 Then
            If row.f_iひも番号 = _i縦ひもの本数 Then
                row.f_d幅 = row.f_dVal1
            Else
                row.f_d幅 = row.f_dVal1 + _dひとつのすき間の寸法
            End If
            row.f_d長さ = _d縦横の縦
            row.f_dひも長 = _d縦横の縦 + 2 * (_d径の合計 + _d垂直ひも長合計 + _d垂直ひも長加算)
            row.f_dVal2 = (row.f_dひも長 / 2) + row.f_dひも長加算2
            row.f_dVal3 = (row.f_dひも長 / 2) + row.f_dひも長加算
            row.f_d出力ひも長 = row.f_dVal2 + row.f_dVal3

        ElseIf row.f_iひも種 = (enumひも種.i_縦 Or enumひも種.i_補強) Then
            row.f_d幅 = 0
            row.f_d長さ = _d縦横の縦
            row.f_dひも長 = _d縦横の縦
            row.f_d出力ひも長 = row.f_dひも長 + row.f_dひも長加算 + row.f_dひも長加算2
            'f_dVal2,f_dVal3,f_dVal4は使いません

        ElseIf row.f_iひも種 = (enumひも種.i_斜め Or enumひも種.i_補強) Then
            row.f_d幅 = 0
            row.f_d長さ = _d縦横の縦 + _d縦横の横
            row.f_dひも長 = Math.Sqrt(_d縦横の横 ^ 2 + _d縦横の縦 ^ 2) 'クロス値
            row.f_d出力ひも長 = row.f_dひも長 + row.f_dひも長加算 + row.f_dひも長加算2
            'f_dVal2,f_dVal3,f_dVal4は使いません

        End If

        Return True
    End Function
#End Region

#End Region

#Region "リスト出力"
    '底楕円の出力するひも長を「連続ひも長」フィールドにセットする
    Private Function set底楕円_連続ひも長(ByVal d垂直ひも長 As Double) As Boolean
        Dim res = (From row As tbl底_楕円Row In _Data.p_tbl底_楕円
                   Select Idx = row.f_iひも番号
                   Order By Idx).Distinct

        'ひも番号ごと
        Dim ret As Boolean = True
        For Each idx As Integer In res
            Dim cond As String = String.Format("f_iひも番号 = {0} AND NOT f_b差しひも区分", idx)
            '同じひも番号のレコード、番号順に
            Dim sum As Double = 0
            Dim lastRow As tbl底_楕円Row = Nothing
            For Each row As tbl底_楕円Row In _Data.p_tbl底_楕円.Select(cond, "f_i番号 ASC")
                If lastRow Is Nothing Then
                    If row.f_b次周連続区分 Then
                        '累積開始
                        row.f_d連続ひも長 = -1
                        sum = (row.f_dひも長 + row.f_dひも長加算)
                        lastRow = row
                    Else
                        '累積なし
                        row.f_d連続ひも長 = (row.f_dひも長 + row.f_dひも長加算)
                        sum = 0
                        'lastRow = Nothing
                    End If
                Else
                    If row.f_b次周連続区分 _
                        AndAlso row.f_iひも本数 = lastRow.f_iひも本数 _
                        AndAlso row.f_i何本幅 = lastRow.f_i何本幅 _
                        AndAlso row.f_s色 = lastRow.f_s色 Then
                        '累積継続
                        row.f_d連続ひも長 = -1
                        sum = sum + (row.f_dひも長 + row.f_dひも長加算)
                        lastRow = row
                    Else
                        '累積終了
                        lastRow.f_d連続ひも長 = sum
                        sum = 0
                        lastRow = Nothing
                        If row.f_b次周連続区分 Then
                            '累積開始
                            row.f_d連続ひも長 = -1
                            sum = (row.f_dひも長 + row.f_dひも長加算)
                            lastRow = row
                        Else
                            '累積なし
                            row.f_d連続ひも長 = (row.f_dひも長 + row.f_dひも長加算)
                        End If
                    End If
                End If
            Next row
            If lastRow IsNot Nothing Then
                lastRow.f_d連続ひも長 = sum
            End If
        Next idx

        '差しひも
        Dim conda As String = "f_b差しひも区分"
        For Each row As tbl底_楕円Row In _Data.p_tbl底_楕円.Select(conda, "f_i番号 ASC")
            row.f_d連続ひも長 = (row.f_dひも長 + row.f_dひも長加算) + d垂直ひも長
        Next

        _Data.p_tbl底_楕円.AcceptChanges()
        Return ret
    End Function

    '側面の出力するひも長を「連続ひも長」フィールドにセットする
    Private Function set側面_連続ひも長() As Boolean
        Dim res = (From row As tbl側面Row In _Data.p_tbl側面
                   Select Idx = row.f_iひも番号
                   Order By Idx).Distinct

        'ひも番号ごと
        Dim ret As Boolean = True
        For Each idx As Integer In res
            Dim cond As String = String.Format("f_iひも番号 = {0}", idx)
            '同じひも番号のレコード、番号順に
            Dim sum As Double = 0
            Dim lastRow As tbl側面Row = Nothing
            For Each row As tbl側面Row In _Data.p_tbl側面.Select(cond, "f_i番号 ASC")
                If lastRow Is Nothing Then
                    If row.f_b次周連続区分 Then
                        '累積開始
                        row.f_d連続ひも長 = -1
                        sum = (row.f_dひも長 + row.f_dひも長加算)
                        lastRow = row
                    Else
                        '累積なし
                        row.f_d連続ひも長 = (row.f_dひも長 + row.f_dひも長加算)
                        sum = 0
                        'lastRow = Nothing
                    End If
                Else
                    If row.f_b次周連続区分 _
                        AndAlso row.f_iひも本数 = lastRow.f_iひも本数 _
                        AndAlso row.f_i何本幅 = lastRow.f_i何本幅 _
                        AndAlso row.f_s色 = lastRow.f_s色 Then
                        '累積継続
                        row.f_d連続ひも長 = -1
                        sum = sum + (row.f_dひも長 + row.f_dひも長加算)
                        lastRow = row
                    Else
                        '累積終了
                        lastRow.f_d連続ひも長 = sum
                        sum = 0
                        lastRow = Nothing
                        If row.f_b次周連続区分 Then
                            '累積開始
                            row.f_d連続ひも長 = -1
                            sum = (row.f_dひも長 + row.f_dひも長加算)
                            lastRow = row
                        Else
                            '累積なし
                            row.f_d連続ひも長 = (row.f_dひも長 + row.f_dひも長加算)
                        End If
                    End If
                End If
            Next row
            If lastRow IsNot Nothing Then
                lastRow.f_d連続ひも長 = sum
            End If
        Next idx

        _Data.p_tbl側面.AcceptChanges()
        Return ret
    End Function

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

        Dim d垂直ひも長 As Double = _d垂直ひも長合計 + _Data.p_row底_縦横.Value("f_d垂直ひも長加算")

        set底楕円_連続ひも長(d垂直ひも長)
        set側面_連続ひも長()

        output.Clear()
        Dim row As tblOutputRow
        Dim cond As String
        Dim order As String

        output.OutBasics(_Data.p_row目標寸法) '空行で終わる

        row = output.NextNewRow
        row.f_sカテゴリー = text底縦横()
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
                tmpTable = get横展開DataTable()
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s横ひものメモ"))
            Else
                row.f_sタイプ = text縦置き()
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
                tt.f_s記号 = output.SetBandRow(0, tt.f_i何本幅, tt.f_d出力ひも長, tt.f_s色)
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
                lasttmp.f_iひも種 = tmp.f_iひも種 AndAlso lasttmp.f_s記号 = tmp.f_s記号 _
                AndAlso lasttmp.f_dひも長加算 = tmp.f_dひも長加算 Then
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
                    If (lasttmp.f_dひも長加算 + lasttmp.f_dひも長加算2) <> 0 Then
                        row.f_s高さ = output.outLengthText(lasttmp.f_dひも長加算 + lasttmp.f_dひも長加算2)
                    End If
                    row.f_sメモ = sbMemo.ToString
                    If _b縦横を展開する Then
                        If contcount = 1 Then
                            row.f_s編みひも名 = String.Format("{0}", lasttmp.f_iひも番号)
                        Else
                            row.f_s編みひも名 = String.Format("{0} - {1}", lasttmp.f_iひも番号, lasttmp.f_iひも番号 + contcount - 1)
                        End If
                        Select Case lasttmp.f_sひも名
                            Case text長い横ひも()
                                row.f_s編みひも名 = String.Format("[{0}] {1}", _Data.p_row底_縦横.Value("f_i長い横ひもの本数"), row.f_s編みひも名)
                            Case text縦ひも()
                                row.f_s編みひも名 = String.Format("[{0}] {1}", _Data.p_row底_縦横.Value("f_i縦ひもの本数"), row.f_s編みひも名)
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

        '***底楕円
        If 0 < _Data.p_tbl底_楕円.Rows.Count Then
            row = output.NextNewRow
            row.f_sカテゴリー = text底楕円()
            row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_s高さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            '編みひも
            cond = "NOT f_b差しひも区分"
            order = "f_i番号 , f_iひも番号"
            For Each r As tbl底_楕円Row In _Data.p_tbl底_楕円.Select(cond, order)
                row = output.NextNewRow
                row.f_s番号 = r.f_i番号.ToString
                row.f_s編みかた名 = r.f_s編みかた名
                row.f_s編みひも名 = r.f_s編みひも名
                row.f_i周数 = r.f_i周数
                row.f_i段数 = r.f_i段数
                row.f_s高さ = output.outLengthText(r.f_d径)
                row.f_s長さ = output.outLengthText(r.f_d周長)
                If 0 < r.f_iひも本数 Then
                    If 0 < r.f_d連続ひも長 Then
                        If Not r.f_b集計対象外区分 Then
                            r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                        Else
                            r.f_s記号 = ""
                            output.SetBandRowNoMark(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                        End If
                    Else
                        r.f_s記号 = ""
                        row.f_s本幅 = r.f_i何本幅
                        row.f_sひも本数 = output.outCountText(r.f_iひも本数)
                        row.f_sひも長 = text次周連続()
                        row.f_s色 = r.f_s色
                    End If
                End If
                row.f_sメモ = r.f_sメモ
            Next
            '差しひも
            cond = "f_b差しひも区分"
            Dim first As Boolean = True
            For Each r As tbl底_楕円Row In _Data.p_tbl底_楕円.Select(cond, order)
                row = output.NextNewRow
                If first Then
                    row.f_sタイプ = text差しひも()
                    first = False
                End If
                row.f_s番号 = r.f_i番号.ToString
                row.f_s編みひも名 = r.f_s編みひも名
                If Not r.f_b集計対象外区分 Then
                    r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                Else
                    r.f_s記号 = ""
                    output.SetBandRowNoMark(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                End If
                row.f_sひも長 = output.outLengthText(r.f_d連続ひも長)
                row.f_sメモ = r.f_sメモ
            Next

            output.AddBlankLine()
        End If

        '***側面
        If 0 < _Data.p_tbl側面.Rows.Count Then
            row = output.NextNewRow
            row.f_sカテゴリー = text側面()
            row.f_s長さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_sひも長 = g_clsSelectBasics.p_unit出力時の寸法単位.Str
            row.f_s高さ = g_clsSelectBasics.p_unit出力時の寸法単位.Str

            order = "f_i番号 ASC , f_iひも番号 ASC"
            If is側面下から上へ() Then
                '記号をとる
                For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, order)
                    If 0 < r.f_iひも本数 Then
                        If 0 <= r.f_d連続ひも長 Then
                            If Not r.f_b集計対象外区分 Then
                                output.SetBandRow(0, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                            End If
                        End If
                    End If
                Next
                order = "f_i番号 DESC , f_iひも番号 DESC"
            End If
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
                        If Not r.f_b集計対象外区分 Then
                            r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                        Else
                            r.f_s記号 = ""
                            output.SetBandRowNoMark(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
                        End If
                    Else
                        r.f_s記号 = ""
                        row.f_s本幅 = output.outLaneText(r.f_i何本幅)
                        row.f_sひも本数 = output.outCountText(r.f_iひも本数)
                        row.f_sひも長 = text次周連続()
                        row.f_s色 = r.f_s色
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
        row.f_s高さ = text内側()
        row.f_s長さ = text外側()

        row = output.NextNewRow
        row.f_s色 = text横寸法() & text底()
        row.f_sメモ = text横寸法() & text最大()
        row.f_sひも本数 = output.outLengthText(p_d内側_横)
        row.f_sひも長 = output.outLengthText(p_d外側_横)
        row.f_s高さ = output.outLengthText(p_d内側_最小横)
        row.f_s長さ = output.outLengthText(p_d外側_最大横)
        row.f_s編みかた名 = String.Format("{0}={1}{2}", text底楕円の径(), output.outLengthText(_d径の合計), g_clsSelectBasics.p_unit出力時の寸法単位.Str)

        row = output.NextNewRow
        row.f_s色 = text縦寸法() & text底()
        row.f_sメモ = text縦寸法() & text最大()
        row.f_sひも本数 = output.outLengthText(p_d内側_縦)
        row.f_sひも長 = output.outLengthText(p_d外側_縦)
        row.f_s高さ = output.outLengthText(p_d内側_最小縦)
        row.f_s長さ = output.outLengthText(p_d外側_最大縦)
        row.f_s編みかた名 = String.Format("{0}={1}{2}", text厚さ(), output.outLengthText(p_d厚さ), g_clsSelectBasics.p_unit出力時の寸法単位.Str)

        row = output.NextNewRow
        row.f_s色 = text高さ寸法()
        row.f_sメモ = text高さ寸法()
        row.f_sひも本数 = output.outLengthText(p_d内側_高さ)
        row.f_sひも長 = output.outLengthText(p_d外側_高さ)
        row.f_s高さ = output.outLengthText(p_d内側_高さ)
        row.f_s長さ = output.outLengthText(p_d外側_高さ)
        row.f_s編みかた名 = String.Format("{0}={1}", text垂直ひも数(), p_i垂直ひも数)

        row = output.NextNewRow
        row.f_s色 = text垂直ひも長()
        row.f_sひも本数 = output.outLengthText(d垂直ひも長)
        row.f_sメモ = My.Resources.CalcOutTurn
        row.f_s高さ = output.outLengthText(d垂直ひも長 - p_d内側_高さ)
        row.f_s編みかた名 = String.Format("{0}={1}{2}", My.Resources.CalcOutAverageMesh, output.outLengthText(_d周の最大値 / p_i垂直ひも数), g_clsSelectBasics.p_unit出力時の寸法単位.Str)

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
    Private Function is側面下から上へ() As Boolean
        Return _frmMain.rad下から上へ.Checked
    End Function

    Private Function text底縦横() As String
        Return _frmMain.tpage底縦横.Text
    End Function

    Private Function text底楕円() As String
        Return _frmMain.tpage底楕円.Text
    End Function

    Private Function text側面() As String
        Return _frmMain.tpage側面.Text
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

    Private Function text長い横ひも() As String
        Return _frmMain.lbl長い横ひも.Text
    End Function

    Private Function text短い横ひも() As String
        Return _frmMain.lbl短い横ひも.Text
    End Function

    Private Function text最上と最下の短いひも() As String
        Return _frmMain.lbl最上と最下の短いひも.Text
    End Function

    Private Function text補強ひも() As String
        Return _frmMain.chk補強ひも.Text
    End Function

    Private Function text縦ひも() As String
        Return _frmMain.lbl縦ひも.Text
    End Function

    Private Function text始末ひも() As String
        Return _frmMain.chk始末ひも.Text
    End Function

    Private Function text斜めの補強ひも() As String
        Return _frmMain.chk斜めの補強ひも.Text
    End Function

    Private Function text縁の始末() As String
        Return _frmMain.chk縁の始末.Text
    End Function

    Private Function text次周連続() As String
        'dgv側面
        Return _frmMain.f_b次周連続区分2.HeaderText
    End Function

    Private Function text差しひも() As String
        Return _frmMain.chk差しひも.Text
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
        Return _frmMain.lbl長い横ひもの本数_単位.Text
    End Function

    Private Function text底() As String
        Return _frmMain.lbl底.Text
    End Function

    Private Function text最大() As String
        Return _frmMain.lbl最大.Text
    End Function

    Private Function text底楕円の径() As String
        Return _frmMain.lbl底楕円の径.Text
    End Function

    Private Function text垂直ひも数() As String
        Return _frmMain.lbl垂直ひも数.Text
    End Function

    Private Function text厚さ() As String
        Return _frmMain.lbl厚さ.Text
    End Function

    Private Function text垂直ひも長() As String
        'dgv側面
        Return _frmMain.f_d垂直ひも長2.HeaderText
    End Function
#End Region

End Class
