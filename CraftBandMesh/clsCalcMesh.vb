

Imports System.Reflection
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.mdlUnit
Imports CraftBand.Tables.dstDataTables
Imports CraftBand.Tables.dstOutput

Class clsCalcMesh

    '処理のカテゴリー
    Public Enum CalcCategory
        None

        NewData 'データ変更
        BsMaster  '基本値/マスター/バンド選択
        Target    '目標寸法
        Horizontal '横置き項目(底の縦横)
        Vertical '縦置き項目(底の縦横)

        GapFit  'すき間を横寸法に合わせる。画面値更新と再計算

        Oval '底の楕円 
        Side  '側面 
        Options  '追加品
        Expand '縦横展開
    End Enum

    Public Property p_b有効 As Boolean
    Public Property p_sメッセージ As String 'p_b有効でない場合のみ参照

    '目標
    Private Property _d横_目標 As Double
    Private Property _d縦_目標 As Double
    Private Property _d高さ_目標 As Double
    Private Property _I基本のひも幅 As Integer
    '縦横
    Private Property _d縦横の横 As Double '縦横分のみ
    Private Property _d縦横の縦 As Double '縦横分のみ
    Private Property _i垂直ひも数_縦横 As Integer 'ゼロ以上
    Private Property _d縦横の垂直ひも間の周 As Double '2*(縦+最上と最下の短いひもを除く横)'ゼロ以上
    Private Property _d最上と最下の短いひもの幅 As Double 'ゼロ以上
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

    '初期化
    Public Sub Clear()
        p_b有効 = False
        p_sメッセージ = Nothing

        _d横_目標 = -1
        _d縦_目標 = -1
        _d高さ_目標 = -1
        _I基本のひも幅 = -1

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
                Dim thick As Double = 8
                If 0 < _d径の合計 Then
                    thick = 2 * System.Math.PI
                End If
                Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(_d周の最大値 + _d厚さの最大値 * thick)
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

        Clear()
    End Sub

    Function CalcSize(ByVal category As CalcCategory, ByVal ctr As Object, ByVal key As Object) As Boolean
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "CalcSize {0} {1} {2}", category, ctr, key)

        Dim ret As Boolean = True
        Select Case category
            Case CalcCategory.NewData, CalcCategory.BsMaster
                'データ変更/基本値/マスター/バンド選択
                Clear()
                set_目標寸法()
                ret = ret And calc_縦寸法()
                ret = ret And calc_横寸法()

                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)

                If ret Then
                    p_sメッセージ = _frmMain.editAddParts.CheckError(_Data)
                    If Not String.IsNullOrEmpty(p_sメッセージ) Then
                        ret = False
                    End If
                End If

            Case CalcCategory.Target    '目標寸法
                set_目標寸法()

            Case CalcCategory.Horizontal '横置き
                ret = ret And calc_縦寸法()
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)

            Case CalcCategory.Vertical '縦置き
                ret = ret And calc_横寸法()
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)

            Case CalcCategory.GapFit     '横寸法に合わせる
                set_目標寸法()
                If Not calc_すき間の寸法() Then
                    Return False
                End If
                ret = ret And calc_底楕円(category, Nothing, Nothing)
                ret = ret And calc_側面(category, Nothing, Nothing)

            Case CalcCategory.Oval '底の楕円
                Dim row As tbl底_楕円Row = CType(ctr, tbl底_楕円Row)
                ret = ret And calc_底楕円(category, row, key)
                ret = ret And calc_横寸法()
                ret = ret And calc_縦寸法()
                ret = ret And calc_底楕円(category, row, key)
                ret = ret And calc_側面(category, Nothing, Nothing)

            Case CalcCategory.Side  '側面
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

    '目標寸法(内側値)をセットする
    Private Sub set_目標寸法()

        With _Data.p_row目標寸法
            _d横_目標 = .Value("f_d横寸法")
            _d縦_目標 = .Value("f_d縦寸法")
            _d高さ_目標 = .Value("f_d高さ寸法")
            _I基本のひも幅 = .Value("f_i基本のひも幅")

            '内側の寸法を目標とする
            If Not .Value("f_b内側区分") Then
                _d横_目標 -= (g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ") * 2)
                _d縦_目標 -= (g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ") * 2)
                If 0 < _d高さ_目標 Then
                    _d高さ_目標 -= g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
                End If
            End If
        End With

    End Sub


#Region "縦横"

    '横寸法=(目標寸法-2径)として、間のすき間を算出する
    '　不可時はエラーメッセージで中断。OK時は、画面値を更新し関連再計算
    'IN:    _D横_目標  _d計算_径  縦ひもの本数  縦ひも(本幅)
    'OUT:   _D計算_横  nudひとつのすき間の寸法   setSaveValue縦横()
    Function calc_すき間の寸法() As Boolean
        Dim d縦ひも間の最小間隔 As Double = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d縦ひも間の最小間隔")

        Dim target As Double = _d横_目標 - 2 * _d径の合計

        Dim ret As Boolean = False
        Dim dひとつのすき間の寸法 As Double = -1

        If Not isValid横_目標 Then
            '横寸法が指定されていないため、すき間の寸法を計算できません。
            p_sメッセージ = My.Resources.CalcNoTargetWidth

        Else
            Do While True
                With _Data.p_row底_縦横
                    If .Value("f_i縦ひもの本数") < 2 Then
                        '縦ひもの本数の指定が正しくありません。
                        p_sメッセージ = My.Resources.CalcNoHeightCount
                        Exit Do
                    End If

                    '縦ひも分の横寸法
                    Dim band As Double = g_clsSelectBasics.p_d指定本幅(.Value("f_i縦ひも")) * .Value("f_i縦ひもの本数")
                    If target <= band Then
                        '横寸法が小さすぎるため縦ひもを置けません。
                        p_sメッセージ = My.Resources.CalcNoShortWidth
                        Exit Do
                    End If

                    dひとつのすき間の寸法 = (target - band) / (.Value("f_i縦ひもの本数") - 1)
                End With

                ret = True
                _d縦横の横 = target

                Exit Do
            Loop
        End If

        If Not ret Then
            'すき間が計算できない時
            Return False
        End If
        'すき間が計算できた

        '警告
        If dひとつのすき間の寸法 < d縦ひも間の最小間隔 Then
            '縦ひも間のすき間が最小間隔より小さくなっています。
            p_sメッセージ = My.Resources.CalcNoSpaceHeight
            ret = False
        End If

        '結果のセットと表示
        _Data.p_row底_縦横.Value("f_dひとつのすき間の寸法") = dひとつのすき間の寸法
        _frmMain.setひとつのすき間の寸法(dひとつのすき間の寸法)

        setSaveValue縦横()
        Return ret
    End Function

    '縦置き項目→横寸法を算出
    'IN:    縦ひもの本数  ひとつのすき間の寸法  縦ひも(本幅)
    'OUT:   _D計算_横  setSaveValue縦横()
    Function calc_横寸法() As Boolean
        Dim d縦ひも間の最小間隔 As Double = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d縦ひも間の最小間隔")

        Dim band As Double = 0 '縦ひも分の横寸法
        Dim space As Double = 0 'すき間の計
        Dim warning As Boolean = False
        With _Data.p_row底_縦横
            Dim i縦ひもの本数 As Integer = .Value("f_i縦ひもの本数") '最小ゼロ

            If i縦ひもの本数 < 1 Then
                '縦ひもの本数の指定が正しくありません。
                p_sメッセージ = My.Resources.CalcNoHeightCount
                warning = True

            ElseIf 2 <= i縦ひもの本数 Then
                'すき間あり
                space = .Value("f_dひとつのすき間の寸法") * (i縦ひもの本数 - 1)
            Else
                '1本の時はすき間なし
            End If

            If 0 < d縦ひも間の最小間隔 AndAlso .Value("f_dひとつのすき間の寸法") < d縦ひも間の最小間隔 Then
                '縦ひも間のすき間が最小間隔より小さくなっています。
                p_sメッセージ = My.Resources.CalcNoSpaceHeight
                warning = True
            End If

            '縦ひも分の横寸法
            band = g_clsSelectBasics.p_d指定本幅(.Value("f_i縦ひも")) * i縦ひもの本数
        End With
        _d縦横の横 = band + space

        setSaveValue縦横()
        Return Not warning
    End Function

    '横ひも項目→縦寸法を算出
    'IN:    長い横ひも   長い横ひもの本数    短い横ひも   最上と最下の短いひも  最上と最下の短いひもの幅
    'OUT:   _D計算_縦  _d最上と最下の短いひもの幅  setSaveValue縦横()
    Function calc_縦寸法() As Boolean

        '内側の縦寸法
        Dim vert_in As Double

        With _Data.p_row底_縦横
            Dim i横ひもの数 As Integer = 0

            '長い横ひもの縦寸法
            Dim vert1 As Double = g_clsSelectBasics.p_d指定本幅(.Value("f_i長い横ひも")) * .Value("f_i長い横ひもの本数")
            i横ひもの数 += .Value("f_i長い横ひもの本数")

            '間の短い横ひもの縦寸法
            Dim vert2 As Double = 0
            If 1 < .Value("f_i長い横ひもの本数") AndAlso 0 < .Value("f_i短い横ひも") Then
                vert2 = g_clsSelectBasics.p_d指定本幅(.Value("f_i短い横ひも")) * (.Value("f_i長い横ひもの本数") - 1)
                i横ひもの数 += (.Value("f_i長い横ひもの本数") - 1)
            End If

            '最上と最下の短いひもの縦寸法
            Dim vert3 As Double = 0
            If 0 < i横ひもの数 Then
                If .Value("f_i最上と最下の短いひも") = enum最上と最下の短いひも.i_同じ幅 Then
                    _d最上と最下の短いひもの幅 = g_clsSelectBasics.p_d指定本幅(.Value("f_i短い横ひも"))
                    vert3 = g_clsSelectBasics.p_d指定本幅(.Value("f_i短い横ひも")) * 2
                    i横ひもの数 += 2

                ElseIf .Value("f_i最上と最下の短いひも") = enum最上と最下の短いひも.i_異なる幅 Then
                    _d最上と最下の短いひもの幅 = g_clsSelectBasics.p_d指定本幅(.Value("f_i最上と最下の短いひもの幅"))
                    vert3 = _d最上と最下の短いひもの幅 * 2
                    i横ひもの数 += 2
                Else 'なし
                    _d最上と最下の短いひもの幅 = 0
                End If
            End If

            '横ひも間のすき間の合計
            Dim vert4 As Double = 0
            If 0 < i横ひもの数 Then
                vert4 = .Value("f_d横ひも間のすき間") * (i横ひもの数 - 1)
            End If

            '内側=縦寸法
            vert_in = vert1 + vert2 + vert3 + vert4
        End With

        _d縦横の縦 = vert_in

        setSaveValue縦横()
        Return True
    End Function

    Private Sub setSaveValue縦横()
        _i垂直ひも数_縦横 = 2 * (_Data.p_row底_縦横.Value("f_i長い横ひもの本数") + _Data.p_row底_縦横.Value("f_i縦ひもの本数"))

        _d縦横の垂直ひも間の周 = 0
        If isValid計算_横 Then
            _d縦横の垂直ひも間の周 += _d縦横の横 * 2
            _frmMain.lbl縦置きの計.Text = g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d縦横の横, False)
        Else
            _frmMain.lbl縦置きの計.Text = ""
        End If
        If isValid計算_縦 Then
            _d縦横の垂直ひも間の周 += (_d縦横の縦 - 2 * _d最上と最下の短いひもの幅) * 2
            _frmMain.lbl横置きの計.Text = g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(_d縦横の縦, False)
        Else
            _frmMain.lbl横置きの計.Text = ""
        End If
    End Sub
#End Region

#Region "概算"

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
            Return False
        End If

        If Not IsValidInput() Then
            message = Nothing
            Return True
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
        set_目標寸法()
        If Not isValid横_目標 OrElse Not isValid縦_目標 OrElse Not isValid高さ_目標 OrElse Not isValid基本のひも幅 Then
            '目標寸法もしくは基本のひも幅が正しくありません。
            p_sメッセージ = My.Resources.CalcNoTargetSet
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
        Dim d基本のひも幅 As Double = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)

        Dim ret As Boolean = True

        If Not isValid計算_横 OrElse Not isNear(p_d内側_横, _d横_目標) Then
            ret = ret And calc_Target_横(d基本のひも幅)
        End If
        If Not isValid計算_縦 OrElse Not isNear(p_d内側_縦, _d縦_目標) Then
            ret = ret And calc_Target_縦(d基本のひも幅)
        End If
        If Not isValid計算_高さ OrElse Not isNear(p_d内側_高さ, _d高さ_目標) Then
            ret = ret And calc_Target_高さ()
        End If
        Return ret
    End Function

    '縦寸法から横ひも(底楕円が設定されていればその分マイナス)
    Private Function calc_Target_横(d基本のひも幅 As Double) As Boolean
        Dim d横ひも間のすき間 As Double = 0

        Dim d縦 As Double = _d縦_目標 - _d径の合計 * 2
        Dim yoko As Integer = Math.Truncate(d縦 / (d基本のひも幅 + d横ひも間のすき間)) 'ドット以下切り捨て
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
        Dim dLeft As Double = d縦 - (n長い横ひもの本数 * (d基本のひも幅 + d横ひも間のすき間))

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
    Private Function calc_Target_縦(d基本のひも幅 As Double)
        Dim d横 As Double = _d横_目標 - _d径の合計 * 2
        Dim n縦ひもn本幅 As Integer = _I基本のひも幅
        Dim tate As Integer = Math.Truncate(d横 / d基本のひも幅) 'ドット以下切り捨て
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
    Shared _fields底楕円() As String = {"f_i何本幅", "f_i周数", "f_i差しひも本数", "f_b周連続区分"}
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

    'IN:    _d底の周
    'OUT:   _d高さの合計 垂直ひもの合計 _d周の最大値 _d周の最小値 _d厚さの最大値
    Private Function calc_側面(ByVal category As CalcCategory, ByVal row As tbl側面Row, ByVal dataPropertyName As String) As Boolean
        Dim ret As Boolean = True
        If category <> CalcCategory.Side Then
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

    'リスト出力時に生成
    Dim _ImageList横ひも As clsImageItemList   '横ひもの展開レコードを含む
    Dim _ImageList縦ひも As clsImageItemList   '縦ひもの展開レコードを含む

    'プレビュー時に生成
    Dim _imageList側面編みかた As clsImageItemList    '側面のレコードを含む
    Dim _ImageList描画要素 As clsImageItemList '底と側面
    Dim _ImageList付属品 As clsImageItemList

    Dim _dPortionOver As Double = New Length(1, "cm").Value '省略部分の長さ


    '横ひもリストの描画情報
    Private Function imageList横ひも() As Boolean
        If _ImageList横ひも Is Nothing Then
            Return False
        End If

        '縦横領域の左上(短いひもの左位置)
        Dim p縦横の左上 As New S実座標(-p_d縦横の横 / 2, p_d縦横の縦 / 2)
        '底から部分描画(長いひもの左位置)
        Dim d左半分の長さ As Double = p_d内側_横 / 2 + _dPortionOver
        Dim p長いひも左上 As New S実座標(-d左半分の長さ, p_d縦横の縦 / 2)
        Dim d横ひも間のすき間 As Double = _Data.p_row底_縦横.Value("f_d横ひも間のすき間")

        '上から下へ(位置順)
        _ImageList横ひも.SortByPosition()
        For Each band As clsImageItem In _ImageList横ひも
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            Dim bandwidth As S差分 = Unit270 * g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)

            Select Case band.m_row縦横展開.f_iひも種
                Case enumひも種.i_横 Or enumひも種.i_長い
                    band.m_rひも位置.p左上 = p長いひも左上
                    band.m_rひも位置.p右下 = p長いひも左上 + bandwidth + Unit0 * (band.m_row縦横展開.f_d出力ひも長 / 2 + d左半分の長さ)
                    band.m_borderひも = band.m_borderひも And Not DirectionEnum._左

                Case (enumひも種.i_横 Or enumひも種.i_短い), (enumひも種.i_横 Or enumひも種.i_最上と最下)
                    band.m_rひも位置.p左上 = New S実座標(-band.m_row縦横展開.f_d出力ひも長 / 2, p縦横の左上.Y)
                    band.m_rひも位置.p右下 = band.m_rひも位置.p左上 + bandwidth + Unit0 * band.m_row縦横展開.f_d出力ひも長

                Case Else
                    '補強ひもは描画しない
                    band.m_bNoMark = True
                    Continue For
            End Select
            '
            p縦横の左上 = p縦横の左上 + bandwidth + Unit270 * d横ひも間のすき間
            p長いひも左上 = p長いひも左上 + bandwidth + Unit270 * d横ひも間のすき間
        Next

        Return True
    End Function

    '縦ひもリストの描画情報
    Private Function imageList縦ひも() As Boolean
        If _ImageList縦ひも Is Nothing Then
            Return False
        End If

        '縦横領域の左下(縦ひもの左下位置)
        Dim d下半分の長さ As Double = p_d内側_縦 / 2 + _dPortionOver
        Dim p左下部分 As New S実座標(-p_d縦横の横 / 2, -d下半分の長さ)
        Dim dひとつのすき間の寸法 As Double = _Data.p_row底_縦横.Value("f_dひとつのすき間の寸法")

        '左から右へ(位置順)
        _ImageList縦ひも.SortByPosition()
        For Each band As clsImageItem In _ImageList縦ひも
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If
            If band.m_row縦横展開.f_iひも種 <> enumひも種.i_縦 Then
                Continue For '始末ひも除外
            End If

            Dim bandwidth As S差分 = Unit0 * g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)
            band.m_rひも位置.p左下 = p左下部分
            band.m_rひも位置.p右上 = p左下部分 + bandwidth + Unit90 * (band.m_row縦横展開.f_d出力ひも長 / 2 + d下半分の長さ)
            band.m_borderひも = band.m_borderひも And Not DirectionEnum._下
            '
            p左下部分 = p左下部分 + bandwidth + Unit0 * dひとつのすき間の寸法
        Next

        Return True
    End Function

    '_imageList側面ひも生成、側面のレコードを含む
    Function imageList側面編みかた(ByVal dひも幅 As Double) As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        '側面のレコードをイメージ情報化
        Dim dY As Double = p_d外側_縦 / 2
        Dim dX As Double = p_d外側_横 / 2
        Dim res = (From row As tbl側面Row In _Data.p_tbl側面
                   Select Num = row.f_i番号
                   Order By Num).Distinct

        '番号ごと
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

            Dim d高さ As Double = groupRow.GetNameValueSum("f_d高さ")
            Dim nひも本数 As Integer = groupRow.GetNameValueSum("f_iひも本数")
            Dim d周長比率対底の周 As Double = groupRow.GetNameValueMax("f_d周長比率対底の周")
            Dim i周数 As Integer = groupRow.GetNameValue("f_i周数") '一致項目

            If 0 < nひも本数 Then

                'ImageTypeEnum._編みかた・横
                item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 1)
                item.m_a四隅.p左下 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, dY)
                item.m_a四隅.p右下 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, dY)
                item.m_a四隅.p左上 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, dY + d高さ)
                item.m_a四隅.p右上 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, dY + d高さ)
                '周の区切り
                If 1 < i周数 Then
                    For i As Integer = 1 To i周数 - 1
                        Dim p1 As New S実座標(item.m_a四隅.x最左, d高さ * (i / i周数) + dY)
                        Dim p2 As New S実座標(item.m_a四隅.x最右, d高さ * (i / i周数) + dY)
                        Dim line As New S線分(p1, p2)
                        item.m_lineList.Add(line)
                    Next
                End If
                '文字位置
                item.p_p文字位置 = New S実座標(dひも幅 + p_d外側_横 * d周長比率対底の周 / 2, dY + d高さ / 2)
                itemlist.AddItem(item)

                'ImageTypeEnum._編みかた・縦
                item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 2)
                item.m_a四隅.p左上 = New S実座標(dX, +p_d外側_縦 * d周長比率対底の周 / 2)
                item.m_a四隅.p左下 = New S実座標(dX, -p_d外側_縦 * d周長比率対底の周 / 2)
                item.m_a四隅.p右上 = New S実座標(dX + d高さ, +p_d外側_縦 * d周長比率対底の周 / 2)
                item.m_a四隅.p右下 = New S実座標(dX + d高さ, -p_d外側_縦 * d周長比率対底の周 / 2)
                '周の区切り
                If 1 < i周数 Then
                    For i As Integer = 1 To i周数 - 1
                        Dim p1 As New S実座標(dX + d高さ * (i / i周数), item.m_a四隅.y最上)
                        Dim p2 As New S実座標(dX + d高さ * (i / i周数), item.m_a四隅.y最下)
                        Dim line As New S線分(p1, p2)
                        item.m_lineList.Add(line)
                    Next
                End If
                '文字は指定しない

                itemlist.AddItem(item)
            End If
            dY += d高さ
            dX += d高さ
        Next

        Return itemlist
    End Function

    '底と側面枠
    Function imageList底と側面枠(ByVal dひも幅 As Double) As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        Dim a底の縦横 As S四隅
        a底の縦横.p左上 = New S実座標(-p_d縦横の横 / 2, p_d縦横の縦 / 2)
        a底の縦横.p右上 = New S実座標(p_d縦横の横 / 2, p_d縦横の縦 / 2)
        a底の縦横.p左下 = -a底の縦横.p右上
        a底の縦横.p右下 = -a底の縦横.p左上

        Dim a底 As S四隅
        a底.p左上 = New S実座標(-p_d内側_横 / 2, p_d内側_縦 / 2)
        a底.p右上 = New S実座標(p_d内側_横 / 2, p_d内側_縦 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        '底
        If _d径の合計 = 0 Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
            item.m_a四隅 = a底
            itemlist.AddItem(item)
        Else
            '楕円底は縦横部分
            item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域, 1)
            item.m_a四隅 = a底の縦横
            itemlist.AddItem(item)
        End If

        '差しひも
        Dim n角の差しひも数 As Integer = _i垂直ひも数_楕円 \ 4
        Dim aryAngle(n角の差しひも数) As Double
        Dim aryIndex As Integer
        If 0 < n角の差しひも数 Then
            Dim angle As Double = 360 / (4 + _i垂直ひも数_楕円)
            aryIndex = 1
            For i As Integer = 1 To n角の差しひも数 Step 2
                aryAngle(i) = angle * aryIndex
                aryIndex += 1
            Next
            aryIndex = n角の差しひも数
            For i As Integer = 2 To n角の差しひも数 Step 2
                aryAngle(i) = angle * aryIndex
                aryIndex -= 1
            Next
        End If

        Dim a楕円の中心 As S四隅
        a楕円の中心.p左上 = a底の縦横.p左上 + Unit270 * _d最上と最下の短いひもの幅
        a楕円の中心.p右上 = a底の縦横.p右上 + Unit270 * _d最上と最下の短いひもの幅
        a楕円の中心.p左下 = a底の縦横.p左下 + Unit90 * _d最上と最下の短いひもの幅
        a楕円の中心.p右下 = a底の縦横.p右下 + Unit90 * _d最上と最下の短いひもの幅

        '番号ごと
        Dim res = (From row As tbl底_楕円Row In _Data.p_tbl底_楕円
                   Select Num = row.f_i番号
                   Order By Num).Distinct
        aryIndex = 1
        Dim r差しひも As Double = 0
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl底_楕円.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

            Dim b差しひも区分 As Boolean = groupRow.GetNameValue("f_b差しひも区分")

            If b差しひも区分 Then
                '差しひも
                Dim iひも数 As Integer = groupRow.GetNameValue("f_i差しひも本数") \ 4
                Dim i本幅 As Integer = groupRow.GetNameValue("f_i何本幅")
                Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
                Dim dひも長 As Double = groupRow.GetNameValue("f_d連続ひも長")

                Dim a右上ひも As S四隅
                a右上ひも.p左上 = a楕円の中心.p右上 + Unit90 * (d幅 / 2) + Unit0 * r差しひも
                a右上ひも.p左下 = a楕円の中心.p右上 + Unit270 * (d幅 / 2) + Unit0 * r差しひも
                a右上ひも.p右上 = a右上ひも.p左上 + Unit0 * dひも長
                a右上ひも.p右下 = a右上ひも.p左下 + Unit0 * dひも長

                For i As Integer = 1 To iひも数
                    '右上
                    item = New clsImageItem(ImageTypeEnum._差しひも, groupRow, i)
                    item.m_a四隅 = a右上ひも.Rotate(a楕円の中心.p右上, aryAngle(aryIndex))
                    item.p_p文字位置 = item.m_a四隅.p右上 + Unit0.Rotate(aryAngle(aryIndex)) * (dひも幅 / 2)
                    itemlist.AddItem(item)

                    aryIndex += 1
                Next
                r差しひも = groupRow.GetNameValue("f_d径の累計")

            Else
                '底楕円
                item = New clsImageItem(ImageTypeEnum._底楕円, groupRow, 1)

                Dim d径の累計 As Double = groupRow.GetNameValue("f_d径の累計") '1レコード想定

                item.m_a四隅 = a楕円の中心
                Dim line As S線分
                '右上→左上
                line = New S線分(New S実座標(a底の縦横.p右上.X, a底の縦横.p右上.Y + d径の累計), New S実座標(a底の縦横.p左上.X, a底の縦横.p左上.Y + d径の累計))
                item.m_lineList.Add(line)
                '左上→左下
                line = New S線分(New S実座標(a底の縦横.p左上.X - d径の累計, a底の縦横.p左上.Y - _d最上と最下の短いひもの幅), New S実座標(a底の縦横.p左下.X - d径の累計, a底の縦横.p左下.Y + _d最上と最下の短いひもの幅))
                item.m_lineList.Add(line)
                '左下→右下
                line = New S線分(New S実座標(a底の縦横.p左下.X, a底の縦横.p左下.Y - d径の累計), New S実座標(a底の縦横.p右下.X, a底の縦横.p右下.Y - d径の累計))
                item.p_p文字位置 = line.p終了 '文字位置
                item.m_lineList.Add(line)
                '右下→右上
                line = New S線分(New S実座標(a底の縦横.p右下.X + d径の累計, a底の縦横.p右下.Y + _d最上と最下の短いひもの幅), New S実座標(a底の縦横.p右上.X + d径の累計, a底の縦横.p右上.Y - _d最上と最下の短いひもの幅))
                item.m_lineList.Add(line)

                itemlist.AddItem(item)
            End If

        Next


        '縁のf_d周長比率対底の周
        Dim d周長比率対底の周 As Double = 1
        Dim rows() As DataRow = _Data.p_tbl側面.Select(Nothing, "f_iひも番号 DESC")
        For Each row In rows
            If Not IsDBNull(row("f_d周長比率対底の周")) AndAlso 0 < row("f_d周長比率対底の周") Then
                d周長比率対底の周 = row("f_d周長比率対底の周")
                Exit For
            End If
        Next

        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.p左下 = New S実座標(-p_d外側_横 / 2, p_d外側_縦 / 2)
        item.m_a四隅.p右下 = New S実座標(+p_d外側_横 / 2, p_d外側_縦 / 2)
        item.m_a四隅.p左上 = New S実座標(-p_d外側_横 * d周長比率対底の周 / 2, _d高さの合計 + p_d外側_縦 / 2)
        item.m_a四隅.p右上 = New S実座標(+p_d外側_横 * d周長比率対底の周 / 2, _d高さの合計 + p_d外側_縦 / 2)
        itemlist.AddItem(item)

        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p左上 = New S実座標(p_d外側_横 / 2, p_d外側_縦 / 2)
        item.m_a四隅.p左下 = New S実座標(p_d外側_横 / 2, -p_d外側_縦 / 2)
        item.m_a四隅.p右上 = New S実座標(_d高さの合計 + p_d外側_横 / 2, p_d外側_縦 * d周長比率対底の周 / 2)
        item.m_a四隅.p右下 = New S実座標(_d高さの合計 + p_d外側_横 / 2, -p_d外側_縦 * d周長比率対底の周 / 2)
        itemlist.AddItem(item)

        Return itemlist
    End Function

    '付属品
    Function imageList付属品() As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        '追加品のレコードをイメージ情報化
        Dim dY As Double = -(3 * _dPortionOver + p_d外側_縦 / 2)
        Dim dX As Double = -p_d外側_横 / 2

        '番号ごと
        Dim res = (From row As tbl追加品Row In _Data.p_tbl追加品
                   Select Num = row.f_i番号
                   Order By Num).Distinct
        For Each num As Integer In res
            Dim cond As String = String.Format("f_i番号 = {0}", num)
            Dim groupRow = New clsGroupDataRow(_Data.p_tbl追加品.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

            Dim i点数 As Integer = groupRow.GetNameValue("f_i点数") '一致項目
            Dim d長さ As Double = groupRow.GetIndexNameValue(1, "f_d長さ")
            Dim i本幅 As Integer = groupRow.GetIndexNameValue(1, "f_i何本幅")
            Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)

            Do While 0 < i点数
                item = New clsImageItem(ImageTypeEnum._付属品, groupRow, i点数)

                item.m_a四隅.p左上 = New S実座標(dX, dY)
                item.m_a四隅.p右上 = New S実座標(dX + d長さ, dY)
                item.m_a四隅.p左下 = New S実座標(dX, dY - d幅)
                item.m_a四隅.p右下 = New S実座標(dX + d長さ, dY - d幅)

                '文字位置
                item.p_p文字位置 = item.m_a四隅.p右上
                itemlist.AddItem(item)

                dY -= d幅 * 2
                i点数 -= 1
            Loop
        Next

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
        _imageList側面編みかた = Nothing
        _ImageList描画要素 = Nothing
        _ImageList付属品 = Nothing

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

        '文字サイズ
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)
        '基本のひも幅と基本色
        imgData.setBasics(dひも幅, _Data.p_row目標寸法.Value("f_s基本色"))

        '描画用のデータ追加
        imageList横ひも()
        imageList縦ひも()
        _imageList側面編みかた = imageList側面編みかた(dひも幅)
        _ImageList描画要素 = imageList底と側面枠(dひも幅)
        _ImageList付属品 = imageList付属品()


        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing
        imgData.MoveList(_imageList側面編みかた)
        _imageList側面編みかた = Nothing
        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing
        imgData.MoveList(_ImageList付属品)
        _ImageList付属品 = Nothing

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function

#Region "リスト出力"

    '底の縦横、横置きの展開
    Function set横展開DataTable(ByVal isRefSaved As Boolean, Optional ByVal dVert As Double = -1) As tbl縦横展開DataTable
        Dim d垂直ひも長 As Double = dVert
        If dVert <= 0 Then
            d垂直ひも長 = _d垂直ひも長合計 + _Data.p_row底_縦横.Value("f_d垂直ひも長加算")
        End If

        Dim d短い横ひも長のばらつき As Double = g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d短い横ひも長のばらつき")

        Dim tbl縦横展開 As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        'f_dひも長加算,f_dひも長加算2,f_s色は既定値
        With _Data.p_row底_縦横
            Dim posyoko As Integer = 1

            '.Value("f_i長い横ひも") は1以上
            If 0 < .Value("f_i長い横ひもの本数") Then

                Dim n長い横ひもの数 As Integer = .Value("f_i長い横ひもの本数")
                Dim n短い横ひもの数 As Integer = n長い横ひもの数 - 1
                Dim n異なる幅の位置 As Integer = -1 '最上と最下
                Dim n短い横ひもの開始位置 As Integer
                If 0 < .Value("f_i短い横ひも") Then
                    n短い横ひもの開始位置 = posyoko + 1 '長い横ひもの次
                Else
                    n短い横ひもの開始位置 = -1 '短い横ひもなし
                End If

                Dim rad As enum最上と最下の短いひも = .Value("f_i最上と最下の短いひも")
                If rad = enum最上と最下の短いひも.i_同じ幅 Then
                    If 0 < n短い横ひもの開始位置 Then
                        n短い横ひもの数 += 2
                        n短い横ひもの開始位置 = posyoko '長い横ひもの前
                        posyoko += 1 '空ける
                    End If
                ElseIf rad = enum最上と最下の短いひも.i_異なる幅 Then
                    n異なる幅の位置 = posyoko
                    posyoko += 1 '空ける
                    n短い横ひもの開始位置 += 1 'ひとつずれる(-1→0)
                End If

                For idx As Integer = 1 To n長い横ひもの数
                    row = tbl縦横展開.Newtbl縦横展開Row

                    row.f_iひも種 = enumひも種.i_横 Or enumひも種.i_長い
                    row.f_i位置番号 = posyoko
                    row.f_iひも番号 = idx
                    row.f_sひも名 = text長い横ひも()
                    row.f_i何本幅 = .Value("f_i長い横ひも")
                    row.f_dひも長 = _d縦横の横 + 2 * (_d径の合計 + d垂直ひも長)
                    row.f_d出力ひも長 = row.f_dひも長
                    row.f_d長さ = _d縦横の横

                    tbl縦横展開.Rows.Add(row)
                    If 0 < n短い横ひもの開始位置 Then
                        posyoko += 2
                    Else
                        posyoko += 1
                    End If
                Next

                If 0 < n短い横ひもの開始位置 Then
                    posyoko = n短い横ひもの開始位置
                    For idx As Integer = 1 To n短い横ひもの数
                        row = tbl縦横展開.Newtbl縦横展開Row

                        row.f_iひも種 = enumひも種.i_横 Or enumひも種.i_短い
                        row.f_i位置番号 = posyoko
                        row.f_iひも番号 = idx
                        row.f_sひも名 = text短い横ひも()
                        row.f_i何本幅 = .Value("f_i短い横ひも")
                        row.f_dひも長 = _d縦横の横 - d短い横ひも長のばらつき
                        row.f_d出力ひも長 = row.f_dひも長
                        row.f_d長さ = _d縦横の横

                        tbl縦横展開.Rows.Add(row)
                        posyoko += 2
                    Next
                End If

                If 0 < n異なる幅の位置 Then
                    For idx As Integer = 1 To 2
                        row = tbl縦横展開.Newtbl縦横展開Row

                        row.f_iひも種 = enumひも種.i_横 Or enumひも種.i_最上と最下
                        If idx = 1 Then
                            row.f_i位置番号 = n異なる幅の位置 '空けておいた番号
                        Else
                            row.f_i位置番号 = posyoko
                            posyoko += 1
                        End If
                        row.f_iひも番号 = idx
                        row.f_sひも名 = text最上と最下の短いひも()
                        row.f_i何本幅 = .Value("f_i最上と最下の短いひもの幅")
                        row.f_dひも長 = _d縦横の横 - d短い横ひも長のばらつき
                        row.f_d出力ひも長 = row.f_dひも長
                        row.f_d長さ = _d縦横の横

                        tbl縦横展開.Rows.Add(row)
                    Next
                End If
            End If

            '以降は裏側
            posyoko = cBackPosition
            If .Value("f_b補強ひも区分") Then
                For idx As Integer = 1 To 2
                    row = tbl縦横展開.Newtbl縦横展開Row

                    row.f_iひも種 = enumひも種.i_横 Or enumひも種.i_補強
                    row.f_i位置番号 = posyoko
                    row.f_iひも番号 = idx
                    row.f_sひも名 = text補強ひも()
                    row.f_dひも長 = _d縦横の横
                    row.f_d出力ひも長 = row.f_dひも長

                    Dim rad As enum最上と最下の短いひも = .Value("f_i最上と最下の短いひも")
                    If rad = enum最上と最下の短いひも.i_なし Then
                        row.f_i何本幅 = .Value("f_i長い横ひも")
                    ElseIf rad = enum最上と最下の短いひも.i_同じ幅 Then
                        row.f_i何本幅 = .Value("f_i長い横ひも")
                    Else
                        row.f_i何本幅 = .Value("f_i最上と最下の短いひもの幅")
                    End If
                    row.f_d長さ = _d縦横の横

                    tbl縦横展開.Rows.Add(row)
                    posyoko += 1
                Next
            End If
        End With

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_横, tbl縦横展開)
        End If

        _ImageList横ひも = Nothing
        _ImageList横ひも = New clsImageItemList(tbl縦横展開)

        'image計算結果は不要
        Return tbl縦横展開
    End Function

    '底の縦横、縦置きの展開
    Function set縦展開DataTable(ByVal isRefSaved As Boolean, Optional ByVal dVert As Double = -1) As tbl縦横展開DataTable
        Dim d垂直ひも長 As Double = dVert
        If dVert <= 0 Then
            d垂直ひも長 = _d垂直ひも長合計 + _Data.p_row底_縦横.Value("f_d垂直ひも長加算")
        End If

        Dim tbl縦横展開 As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        'f_dひも長加算,f_s色は初期値
        With _Data.p_row底_縦横
            Dim postate As Integer = 1
            '.Value("f_i縦ひも") は1以上
            If 0 < .Value("f_i縦ひもの本数") Then
                For idx As Integer = 1 To .Value("f_i縦ひもの本数")
                    row = tbl縦横展開.Newtbl縦横展開Row

                    row.f_iひも種 = enumひも種.i_縦
                    row.f_i位置番号 = postate
                    row.f_iひも番号 = idx
                    row.f_sひも名 = text縦ひも()
                    row.f_i何本幅 = .Value("f_i縦ひも")
                    row.f_d長さ = _d縦横の縦
                    row.f_dひも長 = _d縦横の縦 + 2 * (_d径の合計 + d垂直ひも長)
                    row.f_d出力ひも長 = row.f_dひも長

                    tbl縦横展開.Rows.Add(row)
                    postate += 1
                Next
            End If

            '以降は裏側
            postate = cBackPosition
            If .Value("f_b始末ひも区分") Then
                For idx As Integer = 1 To 2
                    row = tbl縦横展開.Newtbl縦横展開Row
                    row.f_iひも種 = enumひも種.i_縦 Or enumひも種.i_補強
                    row.f_i位置番号 = postate
                    row.f_iひも番号 = idx
                    row.f_sひも名 = text始末ひも()
                    row.f_i何本幅 = .Value("f_i縦ひも")
                    row.f_d長さ = _d縦横の縦
                    row.f_dひも長 = _d縦横の縦
                    row.f_d出力ひも長 = row.f_dひも長

                    tbl縦横展開.Rows.Add(row)
                    postate += 1
                Next
            End If

            If .Value("f_b斜めの補強ひも区分") Then
                For idx As Integer = 1 To 2
                    row = tbl縦横展開.Newtbl縦横展開Row

                    row.f_iひも種 = enumひも種.i_斜め Or enumひも種.i_補強
                    row.f_i位置番号 = postate
                    row.f_iひも番号 = idx
                    row.f_sひも名 = text斜めの補強ひも()
                    row.f_i何本幅 = _I基本のひも幅
                    row.f_dひも長 = Math.Sqrt(_d縦横の横 ^ 2 + _d縦横の縦 ^ 2)
                    row.f_d出力ひも長 = row.f_dひも長
                    row.f_d長さ = _d縦横の縦 + _d縦横の横

                    tbl縦横展開.Rows.Add(row)
                    postate += 1
                Next
            End If

        End With

        '指定があれば既存情報反映
        If isRefSaved Then
            _Data.ToTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, tbl縦横展開)
        End If

        _ImageList縦ひも = Nothing
        _ImageList縦ひも = New clsImageItemList(tbl縦横展開)

        'image計算結果は不要
        Return tbl縦横展開
    End Function


    Function prepare縦横展開DataTable() As Boolean
        Try
            Dim yokotable As tbl縦横展開DataTable = set横展開DataTable(True)
            _Data.FromTmpTable(enumひも種.i_横, yokotable)

        Dim tatetable As tbl縦横展開DataTable = set縦展開DataTable(True)
            _Data.FromTmpTable(enumひも種.i_縦 Or enumひも種.i_斜め, tatetable)

            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "prepare縦横展開DataTable")
            Return False
        End Try
    End Function

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
                tmpTable = set横展開DataTable(_Data.p_row底_縦横.Value("f_b展開区分"), d垂直ひも長)
                sbMemo.Append(_Data.p_row底_縦横.Value("f_s横ひものメモ"))
            Else
                row.f_sタイプ = text縦置き()
                tmpTable = set縦展開DataTable(_Data.p_row底_縦横.Value("f_b展開区分"), d垂直ひも長)
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
                    If _Data.p_row底_縦横.Value("f_b展開区分") Then
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
                        r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
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
                r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
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
                            output.SetBandRow(0, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
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
                        r.f_s記号 = output.SetBandRow(r.f_iひも本数, r.f_i何本幅, r.f_d連続ひも長, r.f_s色)
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
