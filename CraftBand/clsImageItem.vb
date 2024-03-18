Imports System.Text
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

    <Flags()>
    Enum DirectionEnum
        _上 = 1
        _左 = 2
        _下 = 4
        _右 = 8
    End Enum
    Public Const cDirectionEnumAll As DirectionEnum = DirectionEnum._上 Or DirectionEnum._左 Or DirectionEnum._右 Or DirectionEnum._下

#Region "配置構造体"
    'point:点
    Structure S実座標
        Public X As Double
        Public Y As Double
        Sub New(ByVal xx As Double, ByVal yy As Double)
            X = xx
            Y = yy
        End Sub

        Sub Zero()
            X = 0
            Y = 0
        End Sub

        ReadOnly Property IsZero() As Boolean
            Get
                Return X = 0 AndAlso Y = 0
            End Get
        End Property

        '左右反転
        Function VertLeft() As S実座標
            Return New S実座標(-X, Y)
        End Function

        '回転位置
        Function Rotate(ByVal center As S実座標, ByVal angle As Double) As S実座標
            Dim delta As New S差分(center, Me)
            Return center + delta.Rotate(angle)
        End Function
        Function Rotate(ByVal angle As Double) As S実座標
            Return Rotate(pOrigin, angle)
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

        '比較演算子=
        Shared Operator =(ByVal c1 As S実座標, ByVal c2 As S実座標) As Boolean
            Return (c1.X = c2.X) AndAlso (c1.Y = c2.Y)
        End Operator

        '比較演算子<>
        Shared Operator <>(ByVal c1 As S実座標, ByVal c2 As S実座標) As Boolean
            Return Not (c1 = c2)
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

        '角度(度)の単位(長さ1)
        Sub New(ByVal degrees As Double)
            Dim radians As Double = degrees * Math.PI / 180
            dX = Math.Cos(radians)
            dY = Math.Sin(radians)
        End Sub

        Sub Zero()
            dX = 0
            dY = 0
        End Sub

        ReadOnly Property IsZero() As Boolean
            Get
                Return dX = 0 AndAlso dY = 0
            End Get
        End Property

        '回転
        Function Rotate(ByVal angle As Double) As S差分
            Dim rad As Double = angle * System.Math.PI / 180
            Dim x As Double = dX * System.Math.Cos(rad) - dY * System.Math.Sin(rad)
            Dim y As Double = dX * System.Math.Sin(rad) + dY * System.Math.Cos(rad)
            Return New S差分(x, y)
        End Function

        '長さ
        Property Length() As Double
            Get
                If IsZero Then
                    Return 0
                End If
                Return Math.Sqrt(dX * dX + dY * dY)
            End Get
            Set(value As Double)
                If value = 0 Then
                    Zero()
                ElseIf IsZero Then
                    '方向が決まらないが、単位化しておく
                    dX = value
                ElseIf dX = 0 Then
                    dY = IIf(0 < dY, value, -value)
                ElseIf dY = 0 Then
                    dX = IIf(0 < dX, value, -value)
                Else
                    Dim ratio As Double = value / Math.Sqrt(dX * dX + dY * dY)
                    dX *= ratio
                    dY *= ratio
                End If
            End Set
        End Property

        'X軸からの角度
        Function Angle() As Double
            If IsZero Then
                Return 0
            End If
            Dim angleRad As Double = System.Math.Atan2(dY, dX)
            Return angleRad * (180 / System.Math.PI)
        End Function

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

        ReadOnly Property IsEmpty() As Boolean
            Get
                Return p開始.IsZero AndAlso p終了.IsZero
            End Get
        End Property

        Sub Empty()
            p開始.Zero()
            p終了.Zero()
        End Sub

        '回転
        Function Rotate(ByVal center As S実座標, ByVal angle As Double) As S線分
            Return New S線分(Me.p開始.Rotate(center, angle), Me.p終了.Rotate(center, angle))
        End Function
        Function Rotate(ByVal angle As Double) As S線分
            Return Rotate(pOrigin, angle)
        End Function

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

        '線分上の点か
        Function IsOn(ByVal p As S実座標) As Boolean
            If Not r外接領域.isInner(p) Then
                Return False
            End If
            Dim fn As New S直線式(Me)
            Return fn.IsOn(p)
        End Function

        '交点の座標　※平行線を指定しないように!!
        Shared Function p交点(ByVal line1 As S線分, ByVal line2 As S線分) As S実座標
            Dim fn1 As New S直線式(line1)
            Dim fn2 As New S直線式(line2)
            Return S直線式.p交点(fn1, fn2)
        End Function
        Function p交点(ByVal line As S線分) As S実座標
            Return p交点(Me, line)
        End Function

        '平行
        Shared Function is平行(ByVal line1 As S線分, ByVal line2 As S線分) As Boolean
            Dim fn1 As New S直線式(line1)
            Dim fn2 As New S直線式(line2)
            Return S直線式.is平行(fn1, fn2)
        End Function
        Function is平行(ByVal line As S線分) As Boolean
            Return is平行(Me, line)
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("S線分<{0}-{1}> range:{2}", p開始, p終了, r外接領域)
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
            If 0 < Me.Count Then
                sb.AppendFormat("C線分リスト 描画領域:{0}", Get描画領域())
            End If
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

        Sub New(r As S領域)
            p左上 = r.p左上
            p左下 = r.p左下
            p右上 = r.p右上
            p右下 = r.p右下
        End Sub

        Sub Empty()
            p左上.Zero()
            p左下.Zero()
            p右上.Zero()
            p右下.Zero()
        End Sub

        ReadOnly Property IsEmpty() As Boolean
            Get
                Return p左上.IsZero AndAlso p左下.IsZero AndAlso p右上.IsZero AndAlso p右下.IsZero
            End Get
        End Property

        '回転 ※回転角によっては、左<=右,下<=上を満たさなくなります
        Function Rotate(ByVal center As S実座標, ByVal angle As Double) As S四隅
            Return New S四隅(Me.pA.Rotate(center, angle), Me.pB.Rotate(center, angle), Me.pC.Rotate(center, angle), Me.pD.Rotate(center, angle))
        End Function
        Function Rotate(ByVal angle As Double) As S四隅
            Return Rotate(pOrigin, angle)
        End Function

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

        'A～D(位置関係が合っていれば)
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
                Return mdlUnit.Min(p左上.X, p左下.X, p右上.X, p右下.X)
            End Get
        End Property
        ReadOnly Property x最右 As Double
            Get
                Return mdlUnit.Max(p右上.X, p右下.X, p左上.X, p左下.X)
            End Get
        End Property
        ReadOnly Property y最上 As Double
            Get
                Return mdlUnit.Max(p左上.Y, p右上.Y, p左下.Y, p右下.Y)
            End Get
        End Property
        ReadOnly Property y最下 As Double
            Get
                Return mdlUnit.Min(p左下.Y, p右下.Y, p左上.Y, p右上.Y)
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
            sb.AppendFormat("S四隅<最左X({0:f1})最上Y({1:f1}) 最右X({2:f1})最下Y({3:f1})>", x最左, y最上, x最右, y最下)
            sb.AppendFormat(" 左上({0:f1},{1:f1}) 右上({2:f1},{3:f1})", p左上.X, p左上.Y, p右上.X, p右上.Y)
            sb.AppendFormat(" 左下({0:f1},{1:f1}) 右下({2:f1},{3:f1})", p左下.X, p左下.Y, p右下.X, p右下.Y)
            Return sb.ToString

        End Function
    End Structure

    'rect:領域(X軸・Y軸に並行な4点)
    Structure S領域
        Implements IComparable(Of S領域)

        Public p右上 As S実座標
        Public p左下 As S実座標

        Sub New(ByVal p1 As S実座標, ByVal p2 As S実座標)
            x最左 = mdlUnit.Min(p1.X, p2.X)
            x最右 = mdlUnit.Max(p1.X, p2.X)
            y最上 = mdlUnit.Max(p1.Y, p2.Y)
            y最下 = mdlUnit.Min(p1.Y, p2.Y)
        End Sub

        Sub New(ByVal ref As S領域)
            Me.New(ref.p右上, ref.p左下)
        End Sub

        Sub Empty()
            p右上.Zero()
            p左下.Zero()
        End Sub

        ReadOnly Property IsEmpty() As Boolean
            Get
                Return p右上.IsZero AndAlso p左下.IsZero
            End Get
        End Property

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

        '領域内の点か？
        Function isInner(ByVal p As S実座標) As Boolean
            Return x最左 <= p.X AndAlso p.X <= x最右 AndAlso
                y最下 <= p.Y AndAlso p.Y <= y最上
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("S領域<最左X({0:f1})最上Y({1:f1}) 最右X({2:f1})最下Y({3:f1})>", x最左, y最上, x最右, y最下)
            sb.AppendFormat("  左下({0:f1},{1:f1}) 右上({2:f1},{3:f1})", p左下.X, p左下.Y, p右上.X, p右上.Y)
            Return sb.ToString

        End Function

        '既定のソート: 右上座標の原点からの距離
        Public Function CompareTo(other As S領域) As Integer Implements IComparable(Of S領域).CompareTo
            Dim myDis As Double = p右上.X ^ 2 + p右上.Y ^ 2
            Dim otherDis As Double = other.p右上.X ^ 2 + other.p右上.Y ^ 2
            If myDis.CompareTo(otherDis) <> 0 Then
                Return myDis.CompareTo(otherDis)
            End If
            myDis = p左下.X ^ 2 + p左下.Y ^ 2
            otherDis = other.p左下.X ^ 2 + other.p左下.Y ^ 2
            Return myDis.CompareTo(otherDis)
        End Function

        'X座標
        Public Shared Function CompareToX(this As S領域, other As S領域) As Integer
            If this.p右上.X.CompareTo(other.p右上.X) <> 0 Then
                Return this.p右上.X.CompareTo(other.p右上.X)
            End If
            Return this.p左下.X.CompareTo(other.p左下.X)
        End Function

        'Y座標
        Public Shared Function CompareToY(this As S領域, other As S領域) As Integer
            If this.p右上.Y.CompareTo(other.p右上.Y) <> 0 Then
                Return this.p右上.Y.CompareTo(other.p右上.Y)
            End If
            Return this.p左下.Y.CompareTo(other.p左下.Y)
        End Function

    End Structure

    Class C領域リスト
        Inherits List(Of S領域)

        Function Get描画領域() As S領域
            If Me.Count = 0 Then
                Return Nothing 'ゼロ
            End If
            Dim r描画領域 As S領域 = Me(0)
            For i As Integer = 1 To Count - 1
                r描画領域 = r描画領域.get拡大領域(Me(i))
            Next
            Return r描画領域
        End Function

        Sub Add領域(ByVal r As S領域)
            Me.Add(New S領域(r))
        End Sub

        Sub SortByX()
            Me.Sort(AddressOf S領域.CompareToX)
        End Sub

        Sub SortByY()
            Me.Sort(AddressOf S領域.CompareToY)
        End Sub

        Function CrossingX(ByVal x_from As Double, ByVal x_to As Double) As C領域リスト
            Dim sublist As New C領域リスト
            For Each rct As S領域 In Me
                If rct.x最左 <= x_from AndAlso x_to <= rct.x最右 Then
                    sublist.Add(rct)
                End If
            Next
            Return sublist
        End Function

        Function CrossingY(ByVal y_from As Double, ByVal y_to As Double) As C領域リスト
            Dim sublist As New C領域リスト
            For Each rct As S領域 In Me
                If rct.y最下 <= y_from AndAlso y_to <= rct.y最上 Then
                    sublist.Add(rct)
                End If
            Next
            Return sublist
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder
            For Each rct As S領域 In Me
                sb.AppendLine(rct.ToString)
            Next
            sb.AppendFormat("C領域リスト 描画領域:{0}", Get描画領域())
            Return sb.ToString
        End Function
    End Class

    'fn:y=ax+b
    Structure S直線式
        Public a As Double
        Public b As Double
        Sub New(ByVal aa As Double, ByVal bb As Double)
            a = aa
            b = bb
        End Sub

        Sub New(ByVal p1 As S実座標, ByVal p2 As S実座標)
            by2points(p1, p2)
        End Sub

        Sub New(ByVal line As S線分)
            by2points(line.p開始, line.p終了)
        End Sub

        Private Sub by2points(ByVal p1 As S実座標, ByVal p2 As S実座標)
            a = (p1.Y - p2.Y) / (p1.X - p2.X)
            b = p1.Y - a * p1.X
        End Sub

        Sub New(ByVal degrees As Double, ByVal p As S実座標)
            Dim radians As Double = degrees * Math.PI / 180
            a = Math.Tan(radians)
            b = p.Y - a * p.X
        End Sub

        '直線上の点か
        Function IsOn(ByVal p As S実座標) As Boolean
            Dim yy As Double = Y(p.X)
            If yy = p.Y Then
                Return True '一致
            End If
            Dim mm As Integer = 1 '小数点以下1桁
            Return Math.Round(yy, mm) = Math.Round(p.Y, mm)
        End Function

        '傾きの角度
        Function Angle() As Double
            Dim angleRad As Double = System.Math.Atan2(a, 1)
            Return angleRad * (180 / System.Math.PI)
        End Function

        'xからy
        Function Y(ByVal x As Double) As Double
            Return a * x + b
        End Function

        'yからx
        Function X(ByVal y As Double) As Double
            Return (y - b) / a
        End Function

        '交点
        Shared Function p交点(ByVal fn1 As S直線式, ByVal fn2 As S直線式) As S実座標
            If fn1.a = 0 Then
                Return New S実座標(fn2.X(fn1.b), fn1.b)
            End If
            If fn2.a = 0 Then
                Return New S実座標(fn1.X(fn2.b), fn2.b)
            End If
            'y=a1*x+b1, y=a2*x+b2 の交点
            Dim xx As Double = (fn2.b - fn1.b) / (fn1.a - fn2.a)
            Dim yy As Double = fn1.Y(xx)
            Return New S実座標(xx, yy)
        End Function
        Function p交点(ByVal fn As S直線式) As S実座標
            Return p交点(Me, fn)
        End Function

        '平行
        Shared Function is平行(ByVal fn1 As S直線式, ByVal fn2 As S直線式) As Boolean
            If fn1.a = fn2.a Then
                Return True
            End If
            Dim mm As Integer = 1 '小数点以下1桁
            Return Math.Round(fn1.a, mm) = Math.Round(fn2.a, mm)
        End Function
        Function is平行(ByVal fn As S直線式) As Boolean
            Return is平行(Me, fn)
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("S直線式: y={0:f1}x+{1:f1}", a, b)
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
                    a右上四角 = c_a右上四角.Rotate(-c_angle) * dひも幅
                    a左上四角 = c_a左上四角.Rotate(-c_angle) * dひも幅
                    l右上線 = c_l右上線.Rotate(-c_angle) * dひも幅
                    l左上線 = c_l左上線.Rotate(-c_angle) * dひも幅
                End If
            Else
                If Not isgauge Then
                    a右上四角 = c_a右上四角.VertLeft * dひも幅
                    a左上四角 = c_a左上四角.VertLeft * dひも幅
                    l右上線 = c_l右上線.VertLeft * dひも幅
                    l左上線 = c_l左上線.VertLeft * dひも幅
                Else
                    Dim rad As Double = c_angle * System.Math.PI / 180
                    a右上四角 = c_a右上四角.VertLeft.Rotate(c_angle) * dひも幅
                    a左上四角 = c_a左上四角.VertLeft.Rotate(c_angle) * dひも幅
                    l右上線 = c_l右上線.VertLeft.Rotate(c_angle) * dひも幅
                    l左上線 = c_l左上線.VertLeft.Rotate(c_angle) * dひも幅
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
    Public m_groupRow As clsGroupDataRow = Nothing 'Meshの側面,付属品("f_s記号","f_s編みかた名","f_s色"))
    Public m_rowData As clsDataRow = Nothing 'Squareの差しひも

    '領域の四隅(左<=右, 下<=上)
    Public m_a四隅 As S四隅     'バンド

    'ひもの配置
    Public m_dひも幅 As Double
    Public m_rひも位置 As S領域   'バンドの記号位置
    Public m_bNoMark As Boolean = False '記号なし
    Public m_borderひも As DirectionEnum = cDirectionEnumAll '周囲の線描画
    Public m_regionList As C領域リスト 'クロス表示用


    '線分リスト
    Public m_lineList As New C線分リスト

    '文字列配列
    Public m_aryString() As String
    Public m_sizeFont As Double
    Dim _p文字位置 As S実座標

    '四つ畳み編みのコマ
    Public m_knot As CKnot = Nothing


    '描画タイプ(描画順)
    Enum ImageTypeEnum
        _描画なし

        _縦バンド   'm_row縦横展開,m_a四隅,m_rひも位置
        _横バンド   'm_row縦横展開,m_a四隅,m_rひも位置

        _バンド   'm_row縦横展開,m_a四隅,m_rひも位置

        _コマ     'm_row縦横展開,m_row縦横展開2,m_p中心,m_rひも位置,m_knot
        _編みかた   'm_groupRow,m_a四隅,m_lineList,m_p位置,m_rひも位置
        _付属品   'm_groupRow,m_a四隅,m_lineList,m_p位置,m_rひも位置

        _底枠     'm_a四隅,m_lineList
        _横の側面   'm_a四隅,m_lineList
        _縦の側面   'm_a四隅,m_lineList
        _四隅領域 'm_a四隅,m_lineList
        _全体枠    'm_a四隅

        _底枠2     'm_lineList        (Hexagonの底)


        _底楕円    'm_groupRow,m_a四隅,m_lineList,m_p位置,m_rひも位置
        _差しひも    'm_groupRow,m_a四隅,m_p位置,m_rひも位置           (Meshの底)

        _ひも領域   'm_rowData,m_a四隅,m_p位置                       (Squareの差しひも)

        _底の中央線  'm_listLine

        _折り返し線 'm_rひも位置

        _文字列 'm_p位置,m_rひも位置

        _横軸線 'm_listLine
        _縦軸線 'm_listLine
    End Enum
    '
    Public m_ImageType As ImageTypeEnum = ImageTypeEnum._描画なし
    Public m_Index As Integer = 0
    Public m_Index2 As Integer = 0

    '基本のフォントサイズ(実サイズベースの計算用)
    Friend Shared s_BasicFontSize As Double


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
            ElseIf (m_row縦横展開.f_iひも種 And enumひも種.i_横) Then
                'Meshの場合
                m_ImageType = ImageTypeEnum._横バンド
            Else
                '他は別途指定
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

    '編みかた・付属品・差しひも・底楕円
    Sub New(ByVal imageType As ImageTypeEnum, ByVal rows As clsGroupDataRow, ByVal idx2 As Integer)
        m_ImageType = imageType
        m_groupRow = rows
        m_Index2 = idx2
        If m_groupRow IsNot Nothing Then
            m_Index = m_groupRow.GetNameValue("f_i番号") '一致項目
        End If
    End Sub

    'Squareの差しひも
    Sub New(ByVal imageType As ImageTypeEnum, ByVal row As clsDataRow, ByVal idx1 As Integer, ByVal idx2 As Integer)
        m_ImageType = imageType
        m_rowData = row
        m_Index = idx1
        m_Index2 = idx2
    End Sub

    '文字列
    Sub New(ByVal p As S実座標, ByVal arystr() As String, ByVal siz As Double, ByVal idx As Integer)
        m_ImageType = ImageTypeEnum._文字列
        m_Index = idx
        m_sizeFont = siz
        m_aryString = arystr
        If arystr Is Nothing Then
            Return
        End If

        'm_rひも位置におおよその領域をセット
        p_p文字位置 = p

    End Sub

    '文字位置
    Public Property p_p文字位置 As S実座標
        Get
            Return _p文字位置
        End Get
        Set(value As S実座標)
            _p文字位置 = value

            'm_rひも位置におおよその領域をセット
            Dim chars As Integer = 0
            Dim line As Integer = 0
            Dim siz As Double = s_BasicFontSize
            If Not p_p文字位置.IsZero Then
                Select Case m_ImageType
                    Case ImageTypeEnum._編みかた
                        If m_groupRow IsNot Nothing Then
                            chars = Len(m_groupRow.GetIndexNameValue(1, "f_s編みかた名"))
                            chars += m_groupRow.Count '記号数
                            line = 1
                        End If
                    Case ImageTypeEnum._底楕円, ImageTypeEnum._差しひも
                        If m_groupRow IsNot Nothing Then
                            chars = Len(m_groupRow.GetNameValue("f_s編みかた名")) '差しひもは0
                            chars += 1 '記号1点
                            line = 1
                        End If
                    Case ImageTypeEnum._付属品
                        If m_groupRow IsNot Nothing Then
                            chars = Len(m_groupRow.GetIndexNameValue(1, "f_s付属品名"))
                            chars += m_groupRow.Count '記号数
                            line = 1
                        End If
                    Case ImageTypeEnum._ひも領域
                        If m_rowData IsNot Nothing Then
                            chars += 1 '記号1点
                            line = 1
                        End If
                    Case ImageTypeEnum._文字列
                        If m_aryString IsNot Nothing Then
                            For Each Str As String In m_aryString
                                chars = mdlUnit.Max(chars, Len(Str))
                                line += 1
                            Next
                            If 0 < m_sizeFont Then
                                siz = m_sizeFont
                            End If
                        End If
                End Select

                If 0 < line AndAlso 0 < chars Then
                    Dim delta As New S差分(chars * siz, line * siz * 2)
                    m_rひも位置 = New S領域(p_p文字位置, p_p文字位置 + delta)
                End If
            End If
        End Set
    End Property


    Public Function Get描画領域() As S領域
        Dim r描画領域 As S領域
        Select Case Me.m_ImageType
            Case ImageTypeEnum._横バンド, ImageTypeEnum._縦バンド
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(m_rひも位置)

            Case ImageTypeEnum._バンド
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(m_rひも位置)

            Case ImageTypeEnum._コマ
                r描画領域 = m_rひも位置
                '他は領域内

            Case ImageTypeEnum._編みかた, ImageTypeEnum._付属品
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(m_rひも位置) 'm_p位置を含む
                r描画領域 = r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._底枠, ImageTypeEnum._全体枠
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._横の側面, ImageTypeEnum._縦の側面, ImageTypeEnum._四隅領域
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._底楕円, ImageTypeEnum._差しひも, ImageTypeEnum._ひも領域
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(m_rひも位置) 'm_p位置を含む
                r描画領域 = r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._底の中央線, ImageTypeEnum._底枠2
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

    '位置番号順: 描画タイプ > row.f_i位置番号
    Public Shared Function CompareByPosition(left As clsImageItem, right As clsImageItem) As Integer
        If left.m_ImageType.CompareTo(right.m_ImageType) <> 0 Then
            Return left.m_ImageType.CompareTo(right.m_ImageType)
        End If
        If left.m_row縦横展開 Is Nothing Then
            If right.m_row縦横展開 Is Nothing Then
                'ともになければIndex順
                If left.m_Index.CompareTo(right.m_Index) <> 0 Then
                    Return left.m_Index.CompareTo(right.m_Index)
                End If
                Return left.m_Index2.CompareTo(right.m_Index2)
            Else
                Return 1
            End If

        Else
            If right.m_row縦横展開 Is Nothing Then
                Return 0
            Else
                'ともにある
                Return left.m_row縦横展開.f_i位置番号.CompareTo(right.m_row縦横展開.f_i位置番号)
            End If

        End If
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
        If Not m_a四隅.IsEmpty Then
            sb.AppendFormat("四隅:{0}", m_a四隅).AppendLine()
        End If
        sb.AppendFormat("dひも幅={0} ひも位置:{1}", m_dひも幅, m_rひも位置).AppendLine()
        sb.AppendFormat("bNoMark={0} border:{1}", m_bNoMark, m_borderひも).AppendLine()
        If m_regionList IsNot Nothing AndAlso 0 < m_regionList.Count Then
            sb.AppendLine(m_regionList.ToString)
        End If
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

    '要素追加
    Sub AddItem(ByVal item As clsImageItem)
        If item IsNot Nothing Then
            MyBase.Add(item)
        End If
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
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsBandList.GetRowItem 追加({0},{1})", iひも種, iひも番号)
            Dim add As New clsImageItem((New tbl縦横展開DataTable).Newtbl縦横展開Row)
            add.m_row縦横展開.f_iひも種 = iひも種
            add.m_row縦横展開.f_iひも番号 = iひも番号
            Me.AddItem(add)
            Return add
        Else
            Return Nothing
        End If
    End Function

    '位置順にソート
    Sub SortByPosition()
        Me.Sort(AddressOf clsImageItem.CompareByPosition)
    End Sub

    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        For Each item As clsImageItem In Me
            sb.AppendLine(item.ToString)
        Next
        Return sb.ToString
    End Function

End Class
