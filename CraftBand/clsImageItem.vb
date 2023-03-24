Imports System.Runtime.CompilerServices
Imports System.Text
Imports CraftBand.clsDataTables
Imports CraftBand.Tables.dstDataTables

Public Class clsImageItem
    Implements IComparable(Of clsImageItem)

    '実サイズ(設定の単位)

    '     左上       ↑+Y      右上                         右上(A) ＼   
    '                │                                  ／         右下(D)
    ' -X <-----------●------------> +X                ／           ／
    '                │                              左上(B)      ／
    '     左下     -Y↓        右下                      ＼ 左下(C)
    '

#Region "配置構造体"
    'point:点
    Structure S実座標
        Public X As Double
        Public Y As Double
        Sub New(ByVal xx As Double, ByVal yy As Double)
            X = xx
            Y = yy
        End Sub

        '左右反転
        Function VertLeft() As S実座標
            Return New S実座標(-X, Y)
        End Function

        '単項-演算子(マイナス符号)
        Shared Operator -(ByVal c As S実座標) As S実座標
            Return New S実座標(-c.X, -c.Y)
        End Operator

        '二項+演算子 
        Shared Operator +(ByVal c1 As S実座標, ByVal delta As S差分) As S実座標
            Return New S実座標(c1.X + delta.dX, c1.Y + delta.dY)
        End Operator

        '二項*演算子 
        Shared Operator *(ByVal c1 As S実座標, ByVal c2const As Double) As S実座標
            Return New S実座標(c1.X * c2const, c1.Y * c2const)
        End Operator

        Public Overrides Function ToString() As String
            Return String.Format("({0:f1},{1:f1})", X, Y)
        End Function
    End Structure

    'delta:差分
    Structure S差分
        Public dX As Double
        Public dY As Double
        Sub New(ByVal xx As Double, ByVal yy As Double)
            dX = xx
            dY = yy
        End Sub

        Sub New(ByVal base As S実座標, ByVal way As S実座標)
            dX = way.X - base.X
            dY = way.Y - base.Y
        End Sub

        '単項-演算子(マイナス符号)
        Shared Operator -(ByVal c As S差分) As S差分
            Return New S差分(-c.dX, -c.dY)
        End Operator

        '二項*演算子 
        Shared Operator *(ByVal c1 As S差分, ByVal c2const As Double) As S差分
            Return New S差分(c1.dX * c2const, c1.dY * c2const)
        End Operator

        Public Overrides Function ToString() As String
            Return String.Format("diff({0:f1},{1:f1})", dX, dY)
        End Function
    End Structure

    'line:線分
    Structure S線分
        Public p開始 As S実座標
        Public p終了 As S実座標
        Sub New(ByVal f As S実座標, ByVal t As S実座標)
            p開始 = f
            p終了 = t
        End Sub
        Sub New(ByVal line As S線分, delta As S差分)
            p開始 = line.p開始 + delta
            p終了 = line.p終了 + delta
        End Sub

        '二項+演算子 
        Shared Operator +(ByVal c1 As S線分, ByVal delta As S差分) As S線分
            Return New S線分(c1.p開始 + delta, c1.p終了 + delta)
        End Operator

        '二項*演算子 
        Shared Operator *(ByVal c1 As S線分, ByVal c2const As Double) As S線分
            Return New S線分(c1.p開始 * c2const, c1.p終了 * c2const)
        End Operator

        '左右反転
        Function VertLeft() As S線分
            Return New S線分(p開始.VertLeft(), p終了.VertLeft)
        End Function

        ReadOnly Property r外接領域 As S領域
            Get
                Dim r As S領域
                r.x最左 = mdlUnit.Min(p開始.X, p終了.X)
                r.x最右 = mdlUnit.Max(p開始.X, p終了.X)
                r.y最下 = mdlUnit.Min(p開始.Y, p終了.Y)
                r.y最上 = mdlUnit.Max(p開始.Y, p終了.Y)
                Return r
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("line{0}-{1} range:{2}", p開始, p終了, r外接領域)
        End Function
    End Structure

    Class C線分リスト
        Inherits List(Of S線分)

        Function Get描画領域() As S領域
            If Me.Count = 0 Then
                Return Nothing 'ゼロ
            End If
            Dim r描画領域 As S領域 = Me(0).r外接領域
            For i As Integer = 1 To Count - 1
                r描画領域 = r描画領域.get拡大領域(Me(i).r外接領域)
            Next
            Return r描画領域
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder
            For Each line As S線分 In Me
                sb.AppendLine(line.ToString)
            Next
            sb.AppendFormat("All 描画領域:{0}", Get描画領域())
            Return sb.ToString
        End Function
    End Class

    'area:四隅(左<=右,下<=上の4点)
    Structure S四隅
        Public p左上 As S実座標 'B
        Public p左下 As S実座標 'C
        Public p右上 As S実座標 'A
        Public p右下 As S実座標 'D

        Sub New(a As S実座標, b As S実座標, c As S実座標, d As S実座標)
            pA = a
            pB = b
            pC = c
            pD = d
        End Sub

        '二項*演算子 
        Shared Operator *(ByVal c1 As S四隅, ByVal c2const As Double) As S四隅
            Return New S四隅(c1.pA * c2const, c1.pB * c2const, c1.pC * c2const, c1.pD * c2const)
        End Operator

        '二項+演算子 
        Shared Operator +(ByVal c1 As S四隅, ByVal delta As S差分) As S四隅
            Return New S四隅(c1.pA + delta, c1.pB + delta, c1.pC + delta, c1.pD + delta)
        End Operator

        '左右反転
        Function VertLeft() As S四隅
            Return New S四隅(pA.VertLeft, pB.VertLeft, pC.VertLeft, pD.VertLeft)
        End Function
        Property pA As S実座標
            Get
                Return p右上
            End Get
            Set(value As S実座標)
                p右上 = value
            End Set
        End Property
        Property pB As S実座標
            Get
                Return p左上
            End Get
            Set(value As S実座標)
                p左上 = value
            End Set
        End Property
        Property pC As S実座標
            Get
                Return p左下
            End Get
            Set(value As S実座標)
                p左下 = value
            End Set
        End Property
        Property pD As S実座標
            Get
                Return p右下
            End Get
            Set(value As S実座標)
                p右下 = value
            End Set
        End Property

        ReadOnly Property x最左 As Double
            Get
                Return mdlUnit.Min(p左上.X, p左下.X)
            End Get
        End Property
        ReadOnly Property x最右 As Double
            Get
                Return mdlUnit.Max(p右上.X, p右下.X)
            End Get
        End Property
        ReadOnly Property y最上 As Double
            Get
                Return mdlUnit.Max(p左上.Y, p右上.Y)
            End Get
        End Property
        ReadOnly Property y最下 As Double
            Get
                Return mdlUnit.Min(p左下.Y, p右下.Y)
            End Get
        End Property
        ReadOnly Property p中央 As S実座標
            Get
                Return New S実座標((x最左 + x最右) / 2, (y最上 + y最下) / 2)
            End Get
        End Property

        ReadOnly Property r外接領域 As S領域
            Get
                Dim r As S領域
                r.x最左 = x最左
                r.x最右 = x最右
                r.y最下 = y最下
                r.y最上 = y最上
                Return r
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("最左X({0:f1})最上Y({1:f1}) 最右X({2:f1})最下Y({3:f1})", x最左, y最上, x最右, y最下).AppendLine()
            sb.AppendFormat("  左上({0:f1},{1:f1}) 右上({2:f1},{3:f1})", p左上.X, p左上.Y, p右上.X, p右上.Y).AppendLine()
            sb.AppendFormat("  左下({0:f1},{1:f1}) 右下({2:f1},{3:f1})", p左下.X, p左下.Y, p右下.X, p右下.Y).AppendLine()
            Return sb.ToString

        End Function
    End Structure

    'rect:領域(X軸・Y軸に並行な4点)
    Structure S領域
        Public p右上 As S実座標
        Public p左下 As S実座標

        Sub New(ByVal p1 As S実座標, ByVal p2 As S実座標)
            x最左 = mdlUnit.Min(p1.X, p2.X)
            x最右 = mdlUnit.Max(p1.X, p2.X)
            y最上 = mdlUnit.Max(p1.Y, p2.Y)
            y最下 = mdlUnit.Min(p1.Y, p2.Y)
        End Sub

        '原点の四方に描画の想定
        Sub Clear()
            x最左 = 0
            x最右 = 0
            y最上 = 0
            y最下 = 0
        End Sub

        '領域を拡大
        Sub enLarge(ByVal delta As Double)
            p左下.X -= delta
            p右上.X += delta
            p右上.Y += delta
            p左下.Y -= delta
        End Sub

        Property x最左 As Double
            Get
                Return p左下.X
            End Get
            Set(value As Double)
                p左下.X = value
            End Set
        End Property
        Property x最右 As Double
            Get
                Return p右上.X
            End Get
            Set(value As Double)
                p右上.X = value
            End Set
        End Property
        Property y最上 As Double
            Get
                Return p右上.Y
            End Get
            Set(value As Double)
                p右上.Y = value
            End Set
        End Property
        Property y最下 As Double
            Get
                Return p左下.Y
            End Get
            Set(value As Double)
                p左下.Y = value
            End Set
        End Property

        Property p左上 As S実座標
            Get
                Return New S実座標(p左下.X, p右上.Y)
            End Get
            Set(value As S実座標)
                p左下.X = value.X
                p右上.Y = value.Y
            End Set
        End Property
        Property p右下 As S実座標
            Get
                Return New S実座標(p右上.X, p左下.Y)
            End Get
            Set(value As S実座標)
                p右上.X = value.X
                p左下.Y = value.Y
            End Set
        End Property

        Property x幅 As Double
            Get
                Return x最右 - x最左
            End Get
            Set(value As Double) '※先に左下セットの想定
                If p右上.X = 0 Then
                    p右上.X = p左下.X + value
                Else
                    p左下.X = p右上.X - value
                End If
            End Set
        End Property

        Property y高さ As Double
            Get
                Return y最上 - y最下
            End Get
            Set(value As Double) '※先に左下セットの想定
                If p右上.Y = 0 Then
                    p右上.Y = p左下.Y + value
                Else
                    p左下.Y = p右上.Y - value
                End If
            End Set
        End Property

        ReadOnly Property p中央 As S実座標
            Get
                Return New S実座標((x最左 + x最右) / 2, (y最上 + y最下) / 2)
            End Get
        End Property

        '両方を含む領域
        Function get拡大領域(ByVal cur As S領域) As S領域
            Dim large As S領域
            large.x最左 = mdlUnit.Min(x最左, cur.x最左)
            large.x最右 = mdlUnit.Max(x最右, cur.x最右)
            large.y最下 = mdlUnit.Min(y最下, cur.y最下)
            large.y最上 = mdlUnit.Max(y最上, cur.y最上)
            Return large
        End Function

        Function get拡大領域(ByVal cur As S四隅) As S領域
            Dim large As S領域
            large.x最左 = mdlUnit.Min(x最左, cur.x最左)
            large.x最右 = mdlUnit.Max(x最右, cur.x最右)
            large.y最下 = mdlUnit.Min(y最下, cur.y最下)
            large.y最上 = mdlUnit.Max(y最上, cur.y最上)
            Return large
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("最左X({0:f1})最上Y({1:f1}) 最右X({2:f1})最下Y({3:f1})", x最左, y最上, x最右, y最下).AppendLine()
            sb.AppendFormat("  左下({0:f1},{1:f1}) 右上({2:f1},{3:f1})", p左下.X, p左下.Y, p右上.X, p右上.Y).AppendLine()
            Return sb.ToString

        End Function
    End Structure
#End Region

    '四つ畳み編み
    Class CKnot

        '4点多角形
        Private Shared c_a右上四角 As New S四隅(New S実座標(0.448, 1.341), New S実座標(-0.447, 0.895), New S実座標(0, 0), New S実座標(1.087, 0.543))
        Private Shared c_a左上四角 As New S四隅(New S実座標(-0.543, 1.087), New S実座標(-1.341, 0.448), New S実座標(-0.895, -0.447), New S実座標(0, 0.0))

        Private Shared c_l右上線 As New S線分(New S実座標(0.768, 0.942), New S実座標(-0.223, 0.447))
        Private Shared c_l左上線 As New S線分(New S実座標(-0.942, 0.768), New S実座標(-0.447, -0.223))

        Private Const c_angle As Double = 26.34 'コマの傾き leftは左に・rightは右に


        Public p中心 As S実座標
        Public IsKnotLeft As Boolean
        Public IsDrawArea As Boolean    '領域を描く

        '4つの四角
        Public a右上四角 As S四隅
        Public a左上四角 As S四隅
        Public a右下四角 As S四隅
        Public a左下四角 As S四隅

        Public l右上線 As S線分
        Public l左上線 As S線分
        Public l右下線 As S線分
        Public l左下線 As S線分


        Sub New(ByVal p As S実座標, ByVal dひも幅 As Double, ByVal isArea As Boolean, ByVal isleft As Boolean, ByVal isgauge As Boolean)
            IsDrawArea = isArea
            isleft = isleft
            p中心 = p

            If Not isleft Then '描画時に上下反転するため形状は逆
                If Not isgauge Then
                    a右上四角 = c_a右上四角 * dひも幅
                    a左上四角 = c_a左上四角 * dひも幅
                    l右上線 = c_l右上線 * dひも幅
                    l左上線 = c_l左上線 * dひも幅
                Else
                    Dim rad As Double = -c_angle * System.Math.PI / 180
                    a右上四角 = rotateArea(c_a右上四角, rad) * dひも幅
                    a左上四角 = rotateArea(c_a左上四角, rad) * dひも幅
                    l右上線 = rotateLine(c_l右上線, rad) * dひも幅
                    l左上線 = rotateLine(c_l左上線, rad) * dひも幅
                End If
            Else
                If Not isgauge Then
                    a右上四角 = c_a右上四角.VertLeft * dひも幅
                    a左上四角 = c_a左上四角.VertLeft * dひも幅
                    l右上線 = c_l右上線.VertLeft * dひも幅
                    l左上線 = c_l左上線.VertLeft * dひも幅
                Else
                    Dim rad As Double = c_angle * System.Math.PI / 180
                    a右上四角 = rotateArea(c_a右上四角.VertLeft, rad) * dひも幅
                    a左上四角 = rotateArea(c_a左上四角.VertLeft, rad) * dひも幅
                    l右上線 = rotateLine(c_l右上線.VertLeft, rad) * dひも幅
                    l左上線 = rotateLine(c_l左上線.VertLeft, rad) * dひも幅
                End If
            End If

            a左下四角 = a右上四角 * -1 '180度
            a右下四角 = a左上四角 * -1 '180度

            l左下線 = l右上線 * -1 '180度
            l右下線 = l左上線 * -1 '180度

            Dim delta As New S差分(pOrigin, p中心)

            a右上四角 = a右上四角 + delta
            a左下四角 = a左下四角 + delta
            a左上四角 = a左上四角 + delta
            a右下四角 = a右下四角 + delta

            l右上線 = l右上線 + delta
            l左下線 = l左下線 + delta
            l左上線 = l左上線 + delta
            l右下線 = l右下線 + delta
        End Sub

        Private Function rotatePoint(ByVal p As S実座標, ByVal rad As Double) As S実座標
            Dim x As Double = p.X * System.Math.Cos(rad) - p.Y * System.Math.Sin(rad)
            Dim y As Double = p.X * System.Math.Sin(rad) + p.Y * System.Math.Cos(rad)
            Return New S実座標(x, y)
        End Function

        Private Function rotateLine(ByVal l As S線分, ByVal rad As Double) As S線分
            Dim p開始 As S実座標 = rotatePoint(l.p開始, rad)
            Dim p終了 As S実座標 = rotatePoint(l.p終了, rad)
            Return New S線分(p開始, p終了)
        End Function

        Private Function rotateArea(ByVal p As S四隅, ByVal rad As Double) As S四隅
            Dim pA As S実座標 = rotatePoint(p.pA, rad)
            Dim pB As S実座標 = rotatePoint(p.pB, rad)
            Dim pC As S実座標 = rotatePoint(p.pC, rad)
            Dim pD As S実座標 = rotatePoint(p.pD, rad)
            Return New S四隅(pA, pB, pC, pD)
        End Function

    End Class


    Public Shared pOrigin As New S実座標(0, 0)

    Public Shared Unit45 As New S差分(1, 1) '／
    Public Shared Unit135 As New S差分(-1, 1) '＼
    Public Shared Unit225 As New S差分(-1, -1) '／
    Public Shared Unit315 As New S差分(1, -1) '＼

    Public Shared Unit0 As New S差分(1, 0) '→
    Public Shared Unit90 As New S差分(0, 1) '↑
    Public Shared Unit180 As New S差分(-1, 0) '←
    Public Shared Unit270 As New S差分(0, -1) '↓

    'レコード情報
    Public m_row縦横展開 As tbl縦横展開Row = Nothing 'バンド指定の時・コマの横
    Public m_row縦横展開2 As tbl縦横展開Row = Nothing 'コマの縦

    '領域の四隅(左<=右, 下<=上)
    Public m_a四隅 As S四隅

    'ひもの配置
    Public m_dひも幅 As Double
    Public m_rひも位置 As S領域
    Public m_bNoMark As Boolean = False

    '線分リスト
    Public m_lineList As New C線分リスト

    '文字列配列
    Public m_aryString() As String
    Public m_p位置 As S実座標
    Public m_sizeFont As Double

    '四つ畳み編みのコマ
    Public m_knot As CKnot = Nothing


    '描画タイプ(描画順)
    Enum ImageTypeEnum
        _描画なし

        _縦バンド   'p_row縦横展開,p_a四隅,p_rひも位置
        _横バンド   'p_row縦横展開,p_a四隅,p_rひも位置

        _コマ     'p_row縦横展開,p_row縦横展開2,m_p中心,m_rひも位置,m_knot

        _底枠     'p_a四隅,p_listLine
        _横の側面   'p_a四隅,p_listLine
        _縦の側面   'p_a四隅,p_listLine
        _全体枠    'p_a四隅

        _底の中央線  'p_listLine

        _折り返し線 'm_rひも位置

        _文字列 'p_listLine

        _横軸線 'p_listLine
        _縦軸線 'p_listLine
    End Enum
    '
    Public m_ImageType As ImageTypeEnum = ImageTypeEnum._描画なし
    Public m_Index As Integer = 0
    Public m_Index2 As Integer = 0


    Sub New(ByVal imageType As ImageTypeEnum, ByVal idx As Integer, Optional ByVal idx2 As Integer = 0)
        m_ImageType = imageType
        m_Index = idx
        m_Index2 = idx2
    End Sub

    '記号つきバンド
    Sub New(ByVal row As tbl縦横展開Row, Optional nomark As Boolean = False)
        m_bNoMark = nomark
        FromRow(row)
    End Sub
    Private Function FromRow(ByVal row As tbl縦横展開Row) As Boolean
        m_row縦横展開 = row
        If m_row縦横展開 IsNot Nothing Then
            m_Index = m_row縦横展開.f_iひも番号
            If m_row縦横展開.f_iひも種 = enumひも種.i_横 Then
                m_ImageType = ImageTypeEnum._横バンド
            ElseIf m_row縦横展開.f_iひも種 = enumひも種.i_縦 Then
                m_ImageType = ImageTypeEnum._縦バンド
            ElseIf m_row縦横展開.f_iひも種 = enumひも種.i_側面 Then
                m_ImageType = ImageTypeEnum._横バンド '横向きに描く
            Else
                '斜めは描画対象外
            End If
            Return True
        Else
            Return False
        End If
    End Function

    '四つ畳み編みのコマ
    Sub New(ByVal p中心 As S実座標, ByVal row As tbl縦横展開Row, ByVal row2 As tbl縦横展開Row,
            ByVal dひも幅 As Double, ByVal dコマ寸法 As Double, ByVal dすき間 As Double, ByVal isleft As Boolean,
            Optional ByVal isgauge As Boolean = False)

        m_row縦横展開 = row
        m_row縦横展開2 = row2
        m_ImageType = ImageTypeEnum._コマ

        If m_row縦横展開 IsNot Nothing Then
            m_Index = CType(m_row縦横展開.f_iひも種, Integer) + m_row縦横展開.f_iひも番号
        End If
        If m_row縦横展開2 IsNot Nothing Then
            m_Index = CType(m_row縦横展開2.f_iひも種, Integer) + m_row縦横展開2.f_iひも番号
        End If

        m_dひも幅 = dひも幅
        Dim add As Double = 0
        If isgauge Then
            add = dすき間
        End If
        m_rひも位置.p右上 = p中心 + Unit45 * ((dコマ寸法 + add) / 2)
        m_rひも位置.p左下 = p中心 + Unit225 * ((dコマ寸法 + add) / 2)

        m_knot = New CKnot(p中心, dひも幅, 0 < dすき間, isleft, isgauge)

    End Sub

    '文字列
    Sub New(ByVal p As S実座標, ByVal arystr() As String, ByVal size As Double, ByVal idx As Integer)
        m_ImageType = ImageTypeEnum._文字列
        m_Index = idx
        m_p位置 = p
        m_sizeFont = size
        m_aryString = arystr
        If arystr Is Nothing Then
            Return
        End If

        'm_rひも位置におおよその領域をセット
        Dim chars As Integer = 0
        Dim line As Integer = 0
        For Each Str As String In arystr
            chars = mdlUnit.Max(chars, Len(Str))
            line += 1
        Next
        Dim delta As New S差分(chars * size, line * size * 2)
        m_rひも位置 = New S領域(m_p位置, m_p位置 + delta)
    End Sub


    Public Function Get描画領域() As S領域
        Dim r描画領域 As S領域
        Select Case Me.m_ImageType
            Case ImageTypeEnum._横バンド
                '四隅から余裕分左右へ
                r描画領域.y最下 = m_a四隅.y最下
                r描画領域.y最上 = m_a四隅.y最上
                r描画領域.x最左 = m_rひも位置.x最左
                r描画領域.x最右 = m_rひも位置.x最右

            Case ImageTypeEnum._縦バンド
                '四隅から余裕分上下へ
                r描画領域.x最左 = m_a四隅.x最左
                r描画領域.x最右 = m_a四隅.x最右
                r描画領域.y最下 = m_rひも位置.y最下
                r描画領域.y最上 = m_rひも位置.y最上

            Case ImageTypeEnum._コマ
                r描画領域 = m_rひも位置
                '他は領域内

            Case ImageTypeEnum._底枠, ImageTypeEnum._全体枠
                r描画領域 = m_a四隅.r外接領域
                r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._横の側面, ImageTypeEnum._縦の側面
                r描画領域 = m_a四隅.r外接領域
                r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._底の中央線
                r描画領域 = m_lineList.Get描画領域()

            Case ImageTypeEnum._折り返し線
                r描画領域 = m_lineList.Get描画領域()

            Case ImageTypeEnum._文字列
                r描画領域 = m_rひも位置

            Case ImageTypeEnum._横軸線, ImageTypeEnum._横軸線
                '描画領域の後に作成されるが一応
                r描画領域 = m_lineList.Get描画領域()

        End Select
        Return r描画領域
    End Function


    '描画順: 描画タイプ > p_Index > p_Index2
    Public Function CompareTo(other As clsImageItem) As Integer Implements IComparable(Of clsImageItem).CompareTo
        If m_ImageType.CompareTo(other.m_ImageType) <> 0 Then
            Return m_ImageType.CompareTo(other.m_ImageType)
        End If
        If m_Index.CompareTo(other.m_Index) <> 0 Then
            Return m_Index.CompareTo(other.m_Index)
        End If
        Return m_Index2.CompareTo(other.m_Index2)
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("ImageType({0}) Index({1})", m_ImageType, m_Index).AppendLine()
        If m_row縦横展開 IsNot Nothing Then
            sb.AppendFormat("位置({0}) d長さ({1}) dひも長({2}) d出力ひも長({3})", m_row縦横展開.f_i位置番号, m_row縦横展開.f_d長さ, m_row縦横展開.f_dひも長, m_row縦横展開.f_d出力ひも長).AppendLine()
        End If
        If m_row縦横展開2 IsNot Nothing Then
            sb.AppendFormat("位置({0}) d長さ({1}) dひも長({2}) d出力ひも長({3})", m_row縦横展開2.f_i位置番号, m_row縦横展開2.f_d長さ, m_row縦横展開2.f_dひも長, m_row縦横展開2.f_d出力ひも長).AppendLine()
        End If
        'sb.AppendFormat("四隅:{0}  中心:{1}", m_a四隅, m_p中心).AppendLine()
        sb.AppendFormat("dひも幅={0} ひも位置:{1}", m_dひも幅, m_rひも位置).AppendLine()
        sb.AppendLine(m_lineList.ToString)
        Return sb.ToString
    End Function
End Class

Public Class clsImageItemList
    Inherits List(Of clsImageItem)

    Sub New()
        MyBase.New
    End Sub

    Sub New(ByVal tmptable As tbl縦横展開DataTable)
        MyBase.New
        If tmptable Is Nothing Then
            Exit Sub
        End If
        For Each row As tbl縦横展開Row In tmptable
            Dim band As New clsImageItem(row)
            Me.AddItem(band)
        Next
    End Sub

    'ワークテーブルを生成する
    Function NewTmpTable() As tbl縦横展開DataTable
        'ワークテーブル作成
        Dim tmptable As New tbl縦横展開DataTable
        'レコード追加
        For Each band As clsImageItem In Me
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If
            tmptable.ImportRow(band.m_row縦横展開)
        Next
        Return tmptable
    End Function

    '要素追加
    Sub AddItem(ByVal item As clsImageItem)
        MyBase.Add(item)
    End Sub

    'リストの要素を移動
    Sub MoveList(ByVal lst As clsImageItemList)
        For Each item As clsImageItem In lst
            AddItem(item)
        Next
        lst.Clear()
    End Sub

    'レコード対応のアイテム検索
    Function GetRowItem(ByVal iひも種 As Integer, ByVal iひも番号 As Integer, Optional ByVal create As Boolean = True) As clsImageItem
        For Each band As clsImageItem In Me
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If
            If band.m_row縦横展開.f_iひも種 = iひも種 AndAlso band.m_row縦横展開.f_iひも番号 = iひも番号 Then
                Return band
            End If
        Next
        If create Then
            'あるはずなので
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsBandList.GetBand 追加({0},{1})", iひも種, iひも番号)
            Dim add As New clsImageItem((New tbl縦横展開DataTable).Newtbl縦横展開Row)
            add.m_row縦横展開.f_iひも種 = iひも種
            add.m_row縦横展開.f_iひも番号 = iひも番号
            Me.AddItem(add)
            Return add
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        For Each item As clsImageItem In Me
            sb.AppendLine(item.ToString)
        Next
        Return sb.ToString
    End Function

End Class
