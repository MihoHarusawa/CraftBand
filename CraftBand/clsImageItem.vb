Imports System.Runtime.ConstrainedExecution
Imports System.Text
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
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

        '単項-演算子(マイナス符号)
        Shared Operator -(ByVal c As S実座標) As S実座標
            Return New S実座標(-c.X, -c.Y)
        End Operator

        '二項+演算子 
        Shared Operator +(ByVal c1 As S実座標, ByVal c2 As S実座標) As S実座標
            Return New S実座標(c1.X + c2.X, c1.Y + c2.Y)
        End Operator
        Shared Operator +(ByVal c1 As S実座標, ByVal dif As S差分) As S実座標
            Return New S実座標(c1.X + dif.dX, c1.Y + dif.dY)
        End Operator

        '二項-演算子 
        Shared Operator -(ByVal c1 As S実座標, ByVal c2 As S実座標) As S実座標
            Return New S実座標(c1.X - c2.X, c1.Y - c2.Y)
        End Operator

        Public Overrides Function ToString() As String
            Return String.Format("({0},{1})", X, Y)
        End Function
    End Structure

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
        Shared Operator *(ByVal c1 As S差分, ByVal c2const As Double) As S差分
            Return New S差分(c1.dX * c2const, c1.dY * c2const)
        End Operator

        Public Overrides Function ToString() As String
            Return String.Format("diff({0},{1})", dX, dY)
        End Function
    End Structure

    Structure S線分
        Public p開始 As S実座標
        Public p終了 As S実座標
        Sub New(ByVal f As S実座標, ByVal t As S実座標)
            p開始 = f
            p終了 = t
        End Sub

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
        Public p左上 As S実座標
        Public p左下 As S実座標
        Public p右上 As S実座標
        Public p右下 As S実座標
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
            sb.AppendFormat("最左X({0})最上Y({1}) 最右X({2})最下Y({3})", x最左, y最上, x最右, y最下).AppendLine()
            sb.AppendFormat("  左上({0},{1}) 右上({2},{3})", p左上.X, p左上.Y, p右上.X, p右上.Y).AppendLine()
            sb.AppendFormat("  左下({0},{1}) 右下({2},{3})", p左下.X, p左下.Y, p右下.X, p右下.Y).AppendLine()
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

        ReadOnly Property x幅 As Double
            Get
                Return x最右 - x最左
            End Get
        End Property

        ReadOnly Property y高さ As Double
            Get
                Return y最上 - y最下
            End Get
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
            sb.AppendFormat("最左X({0})最上Y({1}) 最右X({2})最下Y({3})", x最左, y最上, x最右, y最下).AppendLine()
            sb.AppendFormat("  左下({0},{1}) 右上({2},{3})", p左下.X, p左下.Y, p右上.X, p右上.Y).AppendLine()
            Return sb.ToString

        End Function
    End Structure
#End Region



    Public Shared Unit45 As New S差分(1, 1) '／
    Public Shared Unit315 As New S差分(1, -1) '＼
    Public Shared Unit225 As New S差分(-1, -1) '／


    'レコード情報
    Public m_row縦横展開 As tbl縦横展開Row = Nothing

    '領域の四隅(左<=右, 下<=上)
    Public m_a四隅 As S四隅

    'ひもの配置
    Public m_dひも幅 As Double
    Public m_rひも位置 As S領域

    '線分リスト
    Public m_lineList As New C線分リスト

    '描画タイプ
    Enum ImageTypeEnum
        _描画なし

        _横バンド   'p_row縦横展開,p_a四隅,p_rひも位置
        _縦バンド   'p_row縦横展開,p_a四隅,p_rひも位置

        _底枠     'p_a四隅
        _全体枠    'p_a四隅
        _横の側面   'p_a四隅
        _縦の側面   'p_a四隅

        _底の中央線  'p_listLine

        _横軸線 'p_listLine
        _縦軸線 'p_listLine
    End Enum
    '
    Public m_ImageType As ImageTypeEnum = ImageTypeEnum._描画なし
    Public m_Index As Integer = 0


    Sub New(ByVal imageType As ImageTypeEnum, ByVal idx As Integer)
        m_ImageType = imageType
        m_Index = idx
    End Sub

    Sub New(ByVal row As tbl縦横展開Row)
        FromRow(row)
    End Sub

    Public Function FromRow(ByVal row As tbl縦横展開Row) As Boolean
        m_row縦横展開 = row
        If m_row縦横展開 IsNot Nothing Then
            m_Index = m_row縦横展開.f_iひも番号
            If m_row縦横展開.f_iひも種 = enumひも種.i_横 Then
                m_ImageType = ImageTypeEnum._横バンド
            ElseIf m_row縦横展開.f_iひも種 = enumひも種.i_縦 Then
                m_ImageType = ImageTypeEnum._縦バンド
            Else
                '斜めは描画対象外
            End If
            Return True
        Else
            Return False
        End If
    End Function

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

            Case ImageTypeEnum._底枠, ImageTypeEnum._全体枠, ImageTypeEnum._横の側面, ImageTypeEnum._縦の側面
                r描画領域 = m_a四隅.r外接領域

            Case ImageTypeEnum._底の中央線
                r描画領域 = m_lineList.Get描画領域()

            Case ImageTypeEnum._横軸線, ImageTypeEnum._横軸線
                '描画領域の後に作成されるが一応
                r描画領域 = m_lineList.Get描画領域()

        End Select
        Return r描画領域
    End Function


    '描画順: 描画タイプ > p_Index
    Public Function CompareTo(other As clsImageItem) As Integer Implements IComparable(Of clsImageItem).CompareTo
        If m_ImageType.CompareTo(other.m_ImageType) <> 0 Then
            Return m_ImageType.CompareTo(other.m_ImageType)
        End If
        Return m_Index.CompareTo(other.m_Index)
    End Function

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("ImageType({0}) Index({1})", m_ImageType, m_Index).AppendLine()
        If m_row縦横展開 IsNot Nothing Then
            sb.AppendFormat("位置({0}) d長さ({1}) dひも長({2}) d出力ひも長({3})", m_row縦横展開.f_i位置番号, m_row縦横展開.f_d長さ, m_row縦横展開.f_dひも長, m_row縦横展開.f_d出力ひも長).AppendLine()
        End If
        sb.AppendFormat("四隅:", m_a四隅).AppendLine()
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
