Imports System.Drawing
Imports System.Text
Imports CraftBand.clsDataTables
Imports CraftBand.clsInsertExpand
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

    Shared Function NearlyEqual(ByVal d1 As Double, ByVal d2 As Double) As Boolean
        If d2 = d1 Then
            Return True '一致
        End If
#If 1 Then
        Return Abs(d1 - d2) < 0.3
#Else
        Dim mm As Integer = 1 '小数点以下1桁
        Return Math.Round(d2, mm) = Math.Round(d1, mm)
#End If
    End Function

    Shared Function SameAngle(ByVal d1 As Integer, ByVal d2 As Integer) As Boolean
        Return Modulo(d1, 360) = Modulo(d2, 360)
    End Function

    Shared Function SameAngle(ByVal deg1 As Double, ByVal deg2 As Double) As Boolean
        Dim d1 As Integer = deg1
        Dim d2 As Integer = deg2
        Return SameAngle(d1, d2) OrElse SameAngle(d1, d2 - 1) OrElse SameAngle(d1, d2 + 1)
    End Function

    'point:点
    Structure S実座標
        Public X As Double
        Public Y As Double
        Sub New(ByVal xx As Double, ByVal yy As Double)
            X = xx
            Y = yy
        End Sub

        Sub New(ByVal ref As S実座標)
            X = ref.X
            Y = ref.Y
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

        '近傍にある
        Function Near(ByVal other As S実座標, Optional ByVal distance As Double = 0) As Boolean
            If NearlyEqual(X, other.X) AndAlso NearlyEqual(Y, other.Y) Then
                Return True
            End If
            If 0 < distance Then
                Dim delta As New S差分(other, Me)
                Return (delta.Length <= distance)
            Else
                Return False
            End If
        End Function

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

        '二項-演算子 
        Shared Operator -(ByVal c1 As S実座標, ByVal delta As S差分) As S実座標
            Return New S実座標(c1.X - delta.dX, c1.Y - delta.dY)
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

        Public Function FromString(ByVal obj As Object) As Boolean
            '空やNullは原点とします
            If obj Is Nothing OrElse IsDBNull(obj) Then
                Zero()
                Return True
            End If
            Dim str As String = CStr(obj)
            If String.IsNullOrWhiteSpace(str) Then
                Zero()
                Return True
            End If

            '文字列処理
            str = str.Trim()
            If str.StartsWith("(") AndAlso str.EndsWith(")") Then
                str = str.Substring(1, str.Length - 2).Trim()
            End If

            '文字があるのに形式が合わなければNG
            Dim parts = str.Split(","c)
            If parts.Length <> 2 Then
                Return False
            End If

            '数値に変換（小数・マイナス対応）
            Dim xVal As Double, yVal As Double
            If Double.TryParse(parts(0).Trim(), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, xVal) AndAlso
               Double.TryParse(parts(1).Trim(), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, yVal) Then
                X = xVal
                Y = yVal
                Return True
            Else
                Return False
            End If
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

        '単位化
        Sub ToUnit()
            Length = 1
        End Sub

        '回転した差分
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
            If dX = 0 Then
                If 0 < dY Then
                    Return 90
                Else
                    Return 270
                End If
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

        '二項+演算子 
        Shared Operator +(ByVal delta1 As S差分, ByVal delta2 As S差分) As S差分
            Return New S差分(delta1.dX + delta2.dY, delta1.dX + delta2.dY)
        End Operator

        Public Overrides Function ToString() As String
            Return String.Format("delta({0:f1},{1:f1})", dX, dY)
        End Function
        Public Function dump() As String
            Return String.Format("{0} Length={1:f2} Angle={2:f1})", ToString(), Length, Angle)
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
        Sub New(ByVal line As S線分)
            p開始 = line.p開始
            p終了 = line.p終了
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

        Sub Revert()
            Dim tmp As New S実座標(p開始)
            p開始 = p終了
            p終了 = tmp
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

        '線分ではなく点
        Function IsDot() As Boolean
            Return p開始.Near(p終了)
        End Function

        '線分上の点か
        Function IsOn(ByVal p As S実座標) As Boolean
            If Not r外接領域.isInner(p) Then
                Return False
            End If
            If IsDot() Then
                Return p開始.Near(p)
            End If
            Dim fn As New S直線式(Me)
            Return fn.IsOn(p)
        End Function

        '中間点
        ReadOnly Property p中点() As S実座標
            Get
                Return New S実座標((p開始.X + p終了.X) / 2, (p開始.Y + p終了.Y) / 2)
            End Get
        End Property

        '長さ
        Property Length As Double
            Get
                Return Math.Sqrt((p終了.X - p開始.X) ^ 2 + (p終了.Y - p開始.Y) ^ 2)
            End Get
            Set(value As Double)
                If Not IsDot() Then
                    Dim ratio As Double = value / Length
                    If Not NearlyEqual(ratio, 1) Then
                        Dim p中点F As S実座標 = p中点()
                        Dim delta開始 As S差分 = (New S差分(p中点F, p開始)) * ratio
                        Dim delta終了 As S差分 = (New S差分(p中点F, p終了)) * ratio
                        p開始 = p中点F + delta開始
                        p終了 = p中点F + delta終了
                    End If
                End If
            End Set
        End Property

        '差分
        Shared Function s差分(ByVal line As S線分) As S差分
            Return New S差分(line.p開始, line.p終了)
        End Function
        Function s差分() As S差分
            Return s差分(Me)
        End Function

        '交点の座標　※線分上にない場合はZero値。平行線を指定すると例外になります
        Shared Function p交点(ByVal line1 As S線分, ByVal line2 As S線分) As S実座標
            Dim p As S実座標 'Zero
            If line2.IsDot Then
                'line2が点
                If line1.IsDot Then
                    'ともに点
                    If line2.p開始.Near(line1.p開始) Then
                        Return line2.p開始
                    Else
                        Return p 'Zero
                    End If
                End If
                'line1が線
                Dim fn1 As New S直線式(line1)
                p = p交点(line2, fn1)
                If Not p.IsZero AndAlso line1.r外接領域.isInner(p) Then
                    Return p
                Else
                    p.Zero()
                    Return p
                End If

            Else
                'line2が線
                Dim fn2 As New S直線式(line2)
                p = p交点(line1, fn2)
                If Not p.IsZero AndAlso line2.r外接領域.isInner(p) Then
                    Return p
                Else
                    p.Zero()
                    Return p
                End If
            End If
        End Function
        Function p交点(ByVal line As S線分) As S実座標
            Return p交点(Me, line)
        End Function
        '
        Shared Function p交点(ByVal line1 As S線分, ByVal fn2 As S直線式) As S実座標
            Dim p As S実座標 'Zero
            If line1.IsDot Then
                If fn2.IsOn(line1.p開始) Then
                    Return line1.p開始
                Else
                    Return p 'Zero
                End If
            End If
            '線あり
            Dim fn1 As New S直線式(line1)
            p = S直線式.p交点(fn1, fn2)
            If Not p.IsZero AndAlso line1.r外接領域.isInner(p) Then
                Return p
            Else
                p.Zero()
                Return p
            End If
        End Function
        Function p交点(ByVal fn As S直線式) As S実座標
            Return p交点(Me, fn)
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
        Function is平行(ByVal fn As S直線式) As Boolean
            Dim fm As New S直線式(Me)
            Return fn.is平行(fm)
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("S線分<{0}-{1}>", p開始, p終了)
        End Function
        Public Function dump() As String
            Return String.Format("{0} {1} {2}", ToString(), s差分.dump(), r外接領域)
        End Function
    End Structure

    Class C線分リスト
        Inherits List(Of S線分)

        Sub New()
            MyBase.New
        End Sub

        Sub New(ByVal ref As C線分リスト)
            MyBase.New
            For Each line As S線分 In ref
                Me.Add(New S線分(line))
            Next
        End Sub

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
            sb.AppendFormat("C線分リスト Count={0} ", Me.Count)
            For Each line As S線分 In Me
                sb.Append(line.ToString).Append("-")
            Next
            sb.Length -= 1
            Return sb.ToString
        End Function
        Public Function dump() As String
            Dim sb As New StringBuilder
            sb.Append(ToString())
            If 0 < Me.Count Then
                sb.AppendLine()
                sb.AppendFormat("描画領域{0}", Get描画領域().dump).AppendLine()
                For Each line As S線分 In Me
                    sb.AppendLine(line.dump)
                Next
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

        Sub New(a As S四隅)
            p左上 = a.p左上
            p左下 = a.p左下
            p右上 = a.p右上
            p右下 = a.p右下
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

        '描画時に配列化する時の順
        Property Point(ByVal i As Integer) As S実座標
            Get
                Select Case i Mod 4
                    Case 0
                        Return p右上 'A
                    Case 1
                        Return p左上 'B
                    Case 2
                        Return p左下 'C
                    Case 3
                        Return p右下 'D
                End Select
            End Get
            Set(value As S実座標)
                Select Case i Mod 4
                    Case 0
                        p右上 = value 'A
                    Case 1
                        p左上 = value 'B
                    Case 2
                        p左下 = value 'C
                    Case 3
                        p右下 = value 'D
                End Select
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
            Return String.Format("S四隅<A{0} B{1} C{2} D{3}>", pA, pB, pC, pD)
        End Function
        Public Function dump() As String
            Dim sb As New System.Text.StringBuilder
            sb.Append(ToString())
            sb.AppendFormat(" <最左X({0:f1})最上Y({1:f1}) 最右X({2:f1})最下Y({3:f1})>", x最左, y最上, x最右, y最下)
            sb.AppendFormat(" 左上({0:f1},{1:f1}) 右上({2:f1},{3:f1})", p左上.X, p左上.Y, p右上.X, p右上.Y)
            sb.AppendFormat(" 左下({0:f1},{1:f1}) 右下({2:f1},{3:f1})", p左下.X, p左下.Y, p右下.X, p右下.Y)
            Return sb.ToString
        End Function
    End Structure

    'rect:領域(X軸・Y軸に並行な4点)
    Structure S領域
        Implements IComparable(Of S領域)

        Public p右上 As S実座標 '※直接セットする場合は位置関係に注意のこと
        Public p左下 As S実座標

        '位置関係を使う場合はこちら
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

        Function IsDot() As Boolean
            Return p右上.Near(p左下)
        End Function


        '原点の四方に描画の想定
        Sub Clear()
            x最左 = 0
            x最右 = 0
            y最上 = 0
            y最下 = 0
        End Sub

        '領域を拡大(マイナスは縮小)
        Sub enLarge(ByVal delta As Double)
            If (delta < 0) AndAlso (x幅 < (-delta * 2)) Then
                Dim midx As Double = (p左下.X + p右上.X) / 2
                p左下.X = midx
                p右上.X = midx
            Else
                p左下.X -= delta
                p右上.X += delta
            End If
            If (delta < 0) AndAlso (y高さ < (-delta * 2)) Then
                Dim midy As Double = (p左下.Y + p右上.Y) / 2
                p右上.Y = midy
                p左下.Y = midy
            Else
                p右上.Y += delta
                p左下.Y -= delta
            End If
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

        'もとの位置関係によらず関係性保証
        ReadOnly Property 外接領域 As S領域
            Get
                Dim r As S領域
                r.x最左 = mdlUnit.Min(p右上.X, p左下.X)
                r.x最右 = mdlUnit.Max(p右上.X, p左下.X)
                r.y最下 = mdlUnit.Min(p右上.Y, p左下.Y)
                r.y最上 = mdlUnit.Max(p右上.Y, p左下.Y)
                Return r
            End Get
        End Property

        '両方を含む領域
        Function get拡大領域(ByVal cur As S領域) As S領域
            Dim large As S領域
            large.x最左 = mdlUnit.Min(p右上.X, p左下.X, cur.p右上.X, cur.p左下.X)
            large.x最右 = mdlUnit.Max(p右上.X, p左下.X, cur.p右上.X, cur.p左下.X)
            large.y最下 = mdlUnit.Min(p右上.Y, p左下.Y, cur.p右上.Y, cur.p左下.Y)
            large.y最上 = mdlUnit.Max(p右上.Y, p左下.Y, cur.p右上.Y, cur.p左下.Y)
            Return large
        End Function

        Function get拡大領域(ByVal cur As S四隅) As S領域
            Dim large As S領域 = 外接領域
            large.x最左 = mdlUnit.Min(large.x最左, cur.x最左)
            large.x最右 = mdlUnit.Max(large.x最右, cur.x最右)
            large.y最下 = mdlUnit.Min(large.y最下, cur.y最下)
            large.y最上 = mdlUnit.Max(large.y最上, cur.y最上)
            Return large
        End Function

        Function get拡大領域(ByVal p As S実座標) As S領域
            Dim large As S領域 = 外接領域
            large.x最左 = mdlUnit.Min(large.x最左, p.X)
            large.x最右 = mdlUnit.Max(large.x最右, p.X)
            large.y最下 = mdlUnit.Min(large.y最下, p.Y)
            large.y最上 = mdlUnit.Max(large.y最上, p.Y)
            Return large
        End Function

        'マイナス値もそのまま処理します
        Function get拡大領域(ByVal width As Double) As S領域
            Dim large As S領域 = 外接領域
            large.x最左 = large.x最左 - width
            large.x最右 = large.x最右 + width
            large.y最下 = large.y最下 - width
            large.y最上 = large.y最上 + width
            Return large
        End Function

        '領域内の点か？
        Function isInner(ByVal p As S実座標) As Boolean
            Dim large As S領域 = 外接領域
            If p.X < large.x最左 AndAlso Not NearlyEqual(large.x最左, p.X) Then
                Return False
            End If
            If large.x最右 < p.X AndAlso Not NearlyEqual(large.x最右, p.X) Then
                Return False
            End If
            If p.Y < large.y最下 AndAlso Not NearlyEqual(large.y最下, p.Y) Then
                Return False
            End If
            If large.y最上 < p.Y AndAlso Not NearlyEqual(large.y最上, p.Y) Then
                Return False
            End If
            Return True
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("S領域<左下{0} 右上{1}>", p左下, p右上)
        End Function
        Public Function dump() As String
            Dim sb As New System.Text.StringBuilder
            sb.Append(ToString)
            sb.AppendFormat(" <左上{0} 右下{1}>", p左上, p右下)
            sb.AppendFormat(" <最左X({0:f1})最上Y({1:f1}) 最右X({2:f1})最下Y({3:f1})>", x最左, y最上, x最右, y最下)
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
            sb.AppendFormat("C領域リスト Count={0} ", Me.Count)
            For Each rct As S領域 In Me
                sb.Append(rct.ToString).Append(" ")
            Next
            Return sb.ToString
        End Function
        Public Function dump() As String
            Dim sb As New StringBuilder
            sb.Append(ToString())
            If 0 < Me.Count Then
                sb.AppendLine()
                sb.AppendFormat("描画領域{0}", Get描画領域().dump).AppendLine()
                For Each rct As S領域 In Me
                    sb.AppendLine(rct.dump)
                Next
            End If
            Return sb.ToString
        End Function
    End Class

    'fn:y=ax+b  is90時はx=b
    Structure S直線式
        Public a As Double
        Public b As Double
        Public is90 As Boolean
        Sub New(ByVal aa As Double, ByVal bb As Double, Optional ByVal vert As Boolean = False)
            a = aa
            b = bb
            is90 = vert
        End Sub

        Sub New(ByVal p1 As S実座標, ByVal p2 As S実座標)
            by2points(p1, p2)
        End Sub

        Sub New(ByVal line As S線分)
            by2points(line.p開始, line.p終了)
        End Sub

        Private Sub by2points(ByVal p1 As S実座標, ByVal p2 As S実座標)
            If NearlyEqual(p1.X, p2.X) Then
                If NearlyEqual(p1.Y, p2.Y) Then
                    Throw New Exception("no line.")
                End If
                is90 = True
                a = Double.PositiveInfinity '∞
                b = p1.X
            Else
                is90 = False
                a = (p1.Y - p2.Y) / (p1.X - p2.X)
                b = p1.Y - a * p1.X
            End If
        End Sub

        Sub New(ByVal degrees As Integer, ByVal p As S実座標)
            If SameAngle(degrees, 90) OrElse SameAngle(degrees, -90) Then
                is90 = True
                a = Double.PositiveInfinity '∞
                b = p.X
            Else
                is90 = False
                Dim radians As Double = degrees * Math.PI / 180
                a = Math.Tan(radians)
                b = p.Y - a * p.X
            End If
        End Sub

        '直線上の点か
        Function IsOn(ByVal p As S実座標) As Boolean
            If is90 Then
                Return NearlyEqual(b, p.X)
            Else
                Return NearlyEqual(Y(p.X), p.Y)
            End If
        End Function

        '傾きの角度
        Function Angle() As Double
            If is90 Then
                Return 90
            Else
                Dim angleRad As Double = System.Math.Atan2(a, 1)
                Return angleRad * (180 / System.Math.PI)
            End If
        End Function

        'xからy
        Function Y(ByVal xx As Double) As Double
            If is90 Then
                If NearlyEqual(b, xx) Then
                    Throw New Exception("cannot determine.")
                Else
                    Throw New Exception("no value.")
                End If
            Else
                Return a * xx + b
            End If
        End Function

        'yからx
        Function X(ByVal yy As Double) As Double
            If NearlyEqual(a, 0) Then
                If NearlyEqual(b, yy) Then
                    Throw New Exception("cannot determine.")
                Else
                    Throw New Exception("no value.")
                End If
            Else
                Return (yy - b) / a
            End If
        End Function

        '交点　※平行線を指定すると例外になります
        Shared Function p交点(ByVal fn1 As S直線式, ByVal fn2 As S直線式) As S実座標
            If is平行(fn1, fn2) Then
                Throw New Exception("no intersection.")
            End If
            If fn1.is90 Then
                If fn2.is90 Then
                    Throw New Exception("parallel or identical.") '平行か一致
                Else
                    Return New S実座標(fn1.b, fn2.Y(fn1.b))
                End If
            Else
                If fn2.is90 Then
                    Return New S実座標(fn2.b, fn1.Y(fn2.b))
                Else
                    If NearlyEqual(fn1.a, fn2.a) Then
                        Throw New Exception("parallel or identical.") '平行か一致
                    Else
                        'y=a1*x+b1, y=a2*x+b2 の交点
                        Dim xx As Double = (fn2.b - fn1.b) / (fn1.a - fn2.a)
                        Dim yy As Double = fn1.Y(xx)
                        Return New S実座標(xx, yy)
                    End If
                End If
            End If
        End Function
        Function p交点(ByVal fn As S直線式) As S実座標
            Return p交点(Me, fn)
        End Function

        'pから直線への垂線の交点
        Function p直交点(ByVal p As S実座標) As S実座標
            If is90 Then
                Return New S実座標(b, p.Y)
            ElseIf a = 0 Then
                Return New S実座標(p.X, b)
            Else
                Dim x0 = (a * p.Y + p.X - a * b) / (a ^ 2 + 1)
                Dim y0 = (a ^ 2 * p.Y + a * p.X + b) / (a ^ 2 + 1)
                Return New S実座標(x0, y0)
            End If
        End Function

        'pから直交点までの長さ
        Function d距離(ByVal p As S実座標) As Double
            Return New S差分(p, p直交点(p)).Length
        End Function

        '平行である時True (一致を含む)
        Shared Function is平行(ByVal fn1 As S直線式, ByVal fn2 As S直線式) As Boolean
            If fn1.is90 Then
                If fn2.is90 Then
                    Return True
                Else
                    Return False
                End If
            Else
                If fn2.is90 Then
                    Return False
                Else
                    Return NearlyEqual(fn1.a, fn2.a)
                End If
            End If
        End Function
        Function is平行(ByVal fn As S直線式) As Boolean
            Return is平行(Me, fn)
        End Function

        Public Overrides Function ToString() As String
            If is90 Then
                Return String.Format("S直線式: x={0:f1}", b)
            Else
                Return String.Format("S直線式: y={0:f1}x+{1:f1}", a, b)
            End If
        End Function
    End Structure

    Structure S円
        Public p中心 As S実座標
        Public d半径 As Double

        Sub New(ByVal p As S実座標, ByVal d As Double)
            p中心 = p
            d半径 = Abs(d)
        End Sub

        Sub New(ByVal d As Double)
            p中心 = pOrigin
            d半径 = Abs(d)
        End Sub

        Function ary直線との交点(fn As S直線式) As S実座標()
            Dim points As New List(Of S実座標)
            Dim rx As Double = p中心.X
            Dim ry As Double = p中心.Y

            If fn.is90 Then
                ' 垂直線 x = b
                Dim c As Double = fn.b
                Dim d As Double = d半径 * d半径 - (c - rx) * (c - rx) ' 判別式

                If d < 0 Then
                    ' 交点なし
                ElseIf d = 0 Then
                    ' 接点
                    Dim y As Double = ry
                    points.Add(New S実座標(c, y))
                Else
                    ' 交点2つ
                    Dim sqrtD As Double = Math.Sqrt(d)
                    points.Add(New S実座標(c, ry + sqrtD))
                    points.Add(New S実座標(c, ry - sqrtD))
                End If
            Else
                ' 一般的な直線 y = ax + b
                Dim a1 As Double = 1 + fn.a * fn.a
                Dim b1 As Double = -2 * rx + 2 * fn.a * (fn.b - ry)
                Dim c1 As Double = rx * rx + (fn.b - ry) * (fn.b - ry) - d半径 * d半径

                Dim d1 As Double = b1 * b1 - 4 * a1 * c1 ' 判別式

                If d1 < 0 Then
                    ' 交点なし
                ElseIf d1 = 0 Then
                    ' 接点
                    Dim x As Double = -b1 / (2 * a1)
                    Dim y As Double = fn.a * x + fn.b
                    points.Add(New S実座標(x, y))
                Else
                    ' 交点2つ
                    Dim sqrtD As Double = Math.Sqrt(d1)
                    Dim x1 As Double = (-b1 + sqrtD) / (2 * a1)
                    Dim x2 As Double = (-b1 - sqrtD) / (2 * a1)
                    Dim y1 As Double = fn.a * x1 + fn.b
                    Dim y2 As Double = fn.a * x2 + fn.b
                    points.Add(New S実座標(x1, y1))
                    points.Add(New S実座標(x2, y2))
                End If
            End If

            Return points.ToArray
        End Function

        ReadOnly Property r外接領域 As S領域
            Get
                Dim r As S領域
                r.x最左 = p中心.X - d半径
                r.x最右 = p中心.X + d半径
                r.y最下 = p中心.Y - d半径
                r.y最上 = p中心.Y + d半径
                Return r
            End Get
        End Property

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

    'バンド
    Class CBand
        '始点→終点 バンドの方向角
        'F→T　軸方向(幅方向)=バンドの方向角+90(deltaAx)
        '
        '  始点T(D)┌──────┐終点T(C)
        '          ├── → ──┤
        '  始点F(A)└──────┘終点F(B)

        Friend Const i_始点F As Integer = 0 'A
        Friend Const i_終点F As Integer = 1 'B
        Friend Const i_終点T As Integer = 2 'C
        Friend Const i_始点T As Integer = 3 'D

        Public aバンド位置 As S四隅
        Public is始点FT線 As Boolean = True
        Public is終点FT線 As Boolean = True
        Public p文字位置 As S実座標 '原点(Zero)は描かない

        '描画に必要な情報
        Public _i何本幅 As Integer
        Public _s色 As String
        Public _s記号 As String
        '
        Friend _s色2 As String
        Friend _i描画種 As enum描画種 = enum描画種.i_指定なし

        Sub New(ByVal ref As CBand)
            aバンド位置 = ref.aバンド位置
            is始点FT線 = ref.is始点FT線
            is終点FT線 = ref.is終点FT線
            p文字位置 = ref.p文字位置
            _i何本幅 = ref._i何本幅
            _s色 = ref._s色
            _s記号 = ref._s記号

            _s色2 = ref._s色2
            _i描画種 = ref._i描画種
        End Sub

        Sub New()
            _i何本幅 = 1
            _s色 = ""
            _s記号 = ""
        End Sub

        Sub New(ByVal row As tbl縦横展開Row)
            _i何本幅 = row.f_i何本幅
            _s色 = row.f_s色
            _s記号 = row.f_s記号
        End Sub

        Sub New(ByVal row As tbl側面Row)
            _i何本幅 = row.f_i何本幅
            _s色 = row.f_s色
            _s記号 = row.f_s記号
        End Sub

        Sub New(ByVal row As tbl差しひもRow)
            _i何本幅 = row.f_i何本幅
            _s色 = row.f_s色
            _s記号 = row.f_s記号
        End Sub

        Sub New(ByVal row As tbl追加品Row)
            _i何本幅 = row.f_i何本幅
            _s色 = row.f_s色
            _s記号 = row.f_s記号
        End Sub

        Sub New(ByVal groupRow As clsGroupDataRow)
            _i何本幅 = groupRow.GetNameValue("f_i何本幅")
            _s色 = groupRow.GetNameValue("f_s色")
            Dim str As String = groupRow.GetNameValueSum("f_s記号")
            str += groupRow.GetNameValue("f_s編みかた名")
            _s記号 = str
        End Sub

        Sub New(ByVal item As CInsertItem)
            _i何本幅 = item.p_i何本幅
            _s色 = item.p_s色
            _s記号 = item.m_s記号
        End Sub

        Property p始点F As S実座標
            Get
                Return aバンド位置.Point(i_始点F)
            End Get
            Set(value As S実座標)
                aバンド位置.Point(i_始点F) = value
            End Set
        End Property

        Property p終点F As S実座標
            Get
                Return aバンド位置.Point(i_終点F)
            End Get
            Set(value As S実座標)
                aバンド位置.Point(i_終点F) = value
            End Set
        End Property

        Property p始点T As S実座標
            Get
                Return aバンド位置.Point(i_始点T)
            End Get
            Set(value As S実座標)
                aバンド位置.Point(i_始点T) = value
            End Set
        End Property

        Property p終点T As S実座標
            Get
                Return aバンド位置.Point(i_終点T)
            End Get
            Set(value As S実座標)
                aバンド位置.Point(i_終点T) = value
            End Set
        End Property

        '始点→終点
        ReadOnly Property delta始点終点 As S差分
            Get
                Return New S差分(p始点F, p終点F)
            End Get
        End Property
        ReadOnly Property line始点終点 As S線分
            Get
                Dim p始点FT中 As S実座標 = New S線分(p始点F, p始点T).p中点
                Dim p終点FT中 As S実座標 = New S線分(p終点F, p終点T).p中点
                Return New S線分(p始点FT中, p終点FT中)
            End Get
        End Property

        'F→T
        ReadOnly Property deltaFT As S差分
            Get
                Return New S差分(p始点F, p始点T)
            End Get
        End Property

        'バンドの中心ライン,幅,軸方向の単位差分
        Function SetBand(ByVal line As S線分, ByVal width As Double, ByVal deltaAx As S差分) As Boolean
            If deltaAx.IsZero Then
                Return SetBand(line, width)
            End If
            p始点F = line.p開始 + deltaAx * (-width / 2)
            p終点F = line.p終了 + deltaAx * (-width / 2)
            p始点T = line.p開始 + deltaAx * (width / 2)
            p終点T = line.p終了 + deltaAx * (width / 2)
            Return True
        End Function
        Function SetBand(ByVal line As S線分, ByVal width As Double) As Boolean
            If line.IsDot Then
                Return False
            End If
            Dim deltaAx As S差分 = line.s差分.Rotate(90)
            deltaAx.ToUnit()  '単位化
            Return SetBand(line, width, deltaAx)
        End Function

        Function SetBand(ByVal p開始 As S実座標, ByVal p終了 As S実座標, ByVal width As Double, ByVal deltaAx As S差分) As Boolean
            Return SetBand(New S線分(p開始, p終了), width, deltaAx)
        End Function
        Function SetBand(ByVal p開始 As S実座標, ByVal p終了 As S実座標, ByVal width As Double) As Boolean
            Return SetBand(New S線分(p開始, p終了), width)
        End Function


        'バンドのFライン,幅,軸方向の単位差分
        Function SetBandF(ByVal line As S線分, ByVal width As Double, ByVal deltaAx As S差分) As Boolean
            p始点F = line.p開始
            p終点F = line.p終了
            p始点T = line.p開始 + deltaAx * width
            p終点T = line.p終了 + deltaAx * width
            Return True
        End Function
        Function SetBandF(ByVal line As S線分, ByVal width As Double) As Boolean
            If line.IsDot Then
                Return False
            End If
            Dim deltaAx As S差分 = line.s差分.Rotate(90)
            deltaAx.ToUnit()  '単位化
            Return SetBandF(line, width, deltaAx)
        End Function


        '長さの伸縮(中点を中心に)
        Function SetLengthRatio(ByVal ratio As Double) As Boolean
            If ratio = 1 Then
                Return True
            End If
            Dim p中点F As S実座標 = New S線分(p始点F, p終点F).p中点
            Dim delta始点F As S差分 = (New S差分(p中点F, p始点F)) * ratio
            Dim delta終点F As S差分 = (New S差分(p中点F, p終点F)) * ratio
            p始点F = p中点F + delta始点F
            p終点F = p中点F + delta終点F

            Dim p中点T As S実座標 = New S線分(p始点T, p終点T).p中点
            Dim delta始点T As S差分 = (New S差分(p中点T, p始点T)) * ratio
            Dim delta終点T As S差分 = (New S差分(p中点T, p終点T)) * ratio
            p始点T = p中点T + delta始点T
            p終点T = p中点T + delta終点T

            Return True
        End Function

        '幅の伸縮(中点を中心に)
        Function SetWideRatio(ByVal ratio As Double) As Boolean
            If ratio = 1 Then
                Return True
            End If
            Dim p中点始点 As S実座標 = New S線分(p始点F, p始点T).p中点
            Dim deltaF始点 As S差分 = (New S差分(p中点始点, p始点F)) * ratio
            Dim deltaT始点 As S差分 = (New S差分(p中点始点, p始点T)) * ratio
            p始点F = p中点始点 + deltaF始点
            p始点T = p中点始点 + deltaT始点

            Dim p中点終点 As S実座標 = New S線分(p終点F, p終点T).p中点
            Dim deltaF終点 As S差分 = (New S差分(p中点終点, p終点F)) * ratio
            Dim deltaT終点 As S差分 = (New S差分(p中点終点, p終点T)) * ratio
            p終点F = p中点終点 + deltaF終点
            p終点T = p中点終点 + deltaT終点

            Return True
        End Function

        '位置を移動
        Sub MoveBand(ByVal delta As S差分)
            aバンド位置 = aバンド位置 + delta
            If Not p文字位置.IsZero Then
                p文字位置 = p文字位置 + delta
            End If
        End Sub

        'バンド方向に移動
        Sub ShiftBand(ByVal distance As Double)
            If distance <> 0 Then
                Dim bandline As S差分 = delta始点終点
                bandline.Length = distance
                MoveBand(bandline)
            End If
        End Sub

        '回転
        Sub RotateBand(ByVal center As S実座標, ByVal angle As Double)
            aバンド位置 = aバンド位置.Rotate(center, angle)
            If Not p文字位置.IsZero Then
                p文字位置 = p文字位置.Rotate(center, angle) '位置のみ
            End If
        End Sub

        '左右反転(F/Tはそのまま)
        Sub VertLeftBand()
            aバンド位置 = aバンド位置.VertLeft()
            If Not p文字位置.IsZero Then
                p文字位置 = p文字位置.VertLeft() '位置のみ
            End If
        End Sub

        'Xの始点側を除去, クロスしなければFalse
        Function TrimBandX(ByVal x As Double, Optional is始点側 As Boolean = True)
            Dim p始点FT As S実座標 = New S線分(p始点F, p始点T).p中点
            Dim p終点FT As S実座標 = New S線分(p終点F, p終点T).p中点
            Dim line As New S線分(p始点FT, p終点FT)
            Dim fn As New S直線式(Double.PositiveInfinity, x, True)
            If line.is平行(fn) Then
                Return False
            End If
            Dim p交点 As S実座標 = line.p交点(fn)
            If p交点.IsZero Then
                Return False
            End If
            Dim halfFT As S差分 = deltaFT * (1 / 2)
            If is始点側 Then
                p始点F = p交点 - halfFT
                p始点T = p交点 + halfFT
                is始点FT線 = False
            Else
                p終点F = p交点 - halfFT
                p終点T = p交点 + halfFT
                is終点FT線 = False
            End If
            Return True
        End Function

        'Yの始点側を除去, クロスしなければFalse
        Function TrimBandY(ByVal y As Double, Optional is始点側 As Boolean = True)
            Dim p始点FT As S実座標 = New S線分(p始点F, p始点T).p中点
            Dim p終点FT As S実座標 = New S線分(p終点F, p終点T).p中点
            Dim line As New S線分(p始点FT, p終点FT)
            Dim fn As New S直線式(0, y, False)
            If line.is平行(fn) Then
                Return False
            End If
            Dim p交点 As S実座標 = line.p交点(fn)
            If p交点.IsZero Then
                Return False
            End If
            Dim halfFT As S差分 = deltaFT * (1 / 2)
            If is始点側 Then
                p始点F = p交点 - halfFT
                p始点T = p交点 + halfFT
                is始点FT線 = False
            Else
                p終点F = p交点 - halfFT
                p終点T = p交点 + halfFT
                is終点FT線 = False
            End If
            Return True
        End Function


        Enum enumMarkPosition
            _なし
            _始点の前
            _始点Fの前
            _始点Tの前
            _終点の後
            _終点Fの後
            _終点Tの後
        End Enum
        '文字位置(距離と差分指定)
        Function SetMarkPosition(ByVal mark As enumMarkPosition,
                             Optional ByVal distance As Double = 0, Optional ByVal delta As S差分 = Nothing) As Boolean
            '文字サイズは基本のひも幅
            Dim p As S実座標
            Select Case mark
                Case enumMarkPosition._なし
                    p文字位置.Zero() '消去
                    Return True
                Case enumMarkPosition._始点の前
                    p = New S線分(p始点F, p始点T).p中点
                Case enumMarkPosition._始点Fの前
                    p = p始点F
                Case enumMarkPosition._始点Tの前
                    p = p始点T
                Case enumMarkPosition._終点の後
                    p = New S線分(p終点F, p終点T).p中点
                Case enumMarkPosition._終点Fの後
                    p = p終点F
                Case enumMarkPosition._終点Tの後
                    p = p終点T
                Case Else
                    Return False
            End Select

            '距離
            If 0 < distance Then
                Dim bandline As S差分 = delta始点終点
                bandline.Length = distance
                If {enumMarkPosition._始点の前, enumMarkPosition._始点Fの前, enumMarkPosition._始点Tの前}.Contains(mark) Then
                    bandline = -bandline
                End If
                p = p + bandline
            End If

            '差分
            p = p + delta

            p文字位置 = p
            Return True
        End Function

        '文字位置(差分指定)
        Function SetMarkPosition(ByVal mark As enumMarkPosition, ByVal delta As S差分) As Boolean
            '文字サイズは基本のひも幅
            Dim p As S実座標
            If mark = enumMarkPosition._なし Then
                p文字位置.Zero() '消去
                Return True
            ElseIf mark = enumMarkPosition._始点の前 Then
                p = New S線分(p始点F, p始点T).p中点
            ElseIf mark = enumMarkPosition._終点の後 Then
                p = New S線分(p終点F, p終点T).p中点
            Else
                Return False
            End If

            p文字位置 = p + delta
            Return True
        End Function

        '色2と描画方法
        Sub SetColorDraw(ByVal s色2 As String, ByVal i描画種 As enum描画種)
            _s色2 = s色2
            _i描画種 = i描画種
        End Sub


        Function Get描画領域() As S領域
            Dim r描画領域 As S領域 = aバンド位置.r外接領域
            If Not p文字位置.IsZero AndAlso Not String.IsNullOrEmpty(_s記号) Then
                Dim delta As New S差分(s_BasicFontSize + 1, s_BasicFontSize * 2)
                r描画領域.get拡大領域(New S領域(p文字位置, p文字位置 + delta))
            End If
            Return r描画領域
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder
            sb.AppendFormat("[A]始点F{0}{4}[D]始点T{1} : [B]終点F{2}{5}[C]終点T{3} ", p始点F, p始点T, p終点F, p終点T,
                            IIf(is始点FT線, "=", "."), IIf(is終点FT線, "=", "."))
            sb.AppendFormat("角度={0} ", delta始点終点.Angle)
            sb.AppendFormat("_i何本幅={0} _s色={1} _s記号={2} p文字位置{3}", _i何本幅, _s色, _s記号, p文字位置)
            sb.AppendFormat(" _s色2={0} _i描画種={1}", _s色2, _i描画種)
            Return sb.ToString
        End Function
    End Class

    'バンドのリスト(※位置合わせのためnothingを含む可能性があります)
    Class CBandList
        Inherits List(Of CBand)

        Function Get描画領域() As S領域
            If Me.Count = 0 Then
                Return Nothing 'ゼロ
            End If
            Dim r描画領域 As New S領域
            For Each band As CBand In Me
                If band IsNot Nothing Then
                    r描画領域 = r描画領域.get拡大領域(band.Get描画領域)
                End If
            Next
            Return r描画領域
        End Function

        Function AddAt(ByVal band As CBand, ByVal idx As Integer) As Boolean
            If Count < idx Then
                Do While Count < idx
                    Add(Nothing)
                Loop
            End If
            If Count = idx Then
                Add(band)
            ElseIf idx < Count Then
                Me(idx) = band
            End If
            Return True
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder
            sb.AppendFormat("CBandList Count={0} ", Me.Count).AppendLine()
            For Each band As CBand In Me
                If band IsNot Nothing Then
                    sb.Append(band.ToString).AppendLine()
                End If
            Next
            Return sb.ToString
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
    Public m_row縦横展開 As tbl縦横展開Row = Nothing '縦バンド・横バンド・コマの横
    Public m_row縦横展開2 As tbl縦横展開Row = Nothing 'コマの縦
    Public m_groupRow As clsGroupDataRow = Nothing '編みかた,底楕円("f_s記号","f_s編みかた名","f_s色"))
    Public m_row追加品 As tbl追加品Row = Nothing '付属品

    '領域の四隅(左<=右, 下<=上)
    Public m_a四隅 As S四隅
    Public m_is円 As Boolean = False
    Public m_ltype As LineTypeEnum = LineTypeEnum._nodef

    '縦バンド・横バンド・コマ・付属品
    Public m_dひも幅 As Double = 0
    Public m_rひも位置 As S領域

    '縦バンド・横バンド
    Public m_bNoMark As Boolean = False '記号なし
    Public m_borderひも As DirectionEnum = cDirectionEnumAll '周囲の線描画
    'Public m_regionList As C領域リスト 

    '線分リスト
    Public m_lineList As New C線分リスト

    '文字列配列
    Public m_aryString() As String
    Public m_sizeFont As Double
    Dim _p文字位置 As S実座標  '文字表示を伴う場合共有
    Dim _r文字領域 As S領域   '〃

    '四つ畳み編みのコマ
    Public m_knot As CKnot = Nothing

    'バンドセット
    Friend m_bandList As CBandList = Nothing    '付属品
    Friend m_clipList As CBandList = Nothing
    Friend m_aDraw As S四隅 '指定があればこの範囲にだけ

    'クリップ
    Public m_fpath As String 'jpg
    Public m_angle As Double
    Public m_alfa As Integer = 100 '保存の透明度 0=完全透明 255=不透明
    Public m_image As Image = Nothing '読み込み済み画像


    '描画タイプ(描画順)
    Enum ImageTypeEnum
        _描画なし

        _縦バンド   'm_row縦横展開,m_a四隅,m_rひも位置
        _横バンド   'm_row縦横展開,m_a四隅,m_rひも位置

        _バンドセット   'm_bandList

        _コマ     'm_row縦横展開,m_row縦横展開2,m_rひも位置,m_knot
        _編みかた   'm_groupRow,m_a四隅,m_lineList,_r文字領域

        _付属品0   'システム用。_付属品と同処理・保存画像に描画

        _画像貼付   'm_a四隅
        '_折り返し線 'm_listLine
        _画像保存   'm_a四隅

        _底枠     'm_a四隅,m_lineList,m_is円
        _横の側面   'm_a四隅,m_lineList
        _縦の側面   'm_a四隅,m_lineList
        _四隅領域 'm_a四隅,m_lineList,m_is円
        _四隅領域線    'm_a四隅,m_lineList,m_is円,m_ltype
        _底枠2     'm_lineList        (Hexagonの底)
        _底楕円    'm_groupRow,m_a四隅,m_lineList,_r文字領域,m_is円,m_dひも幅 (Meshの底)

        _折り返し線 'm_listLine

        _底の中央線  'm_listLine
        _付属品   'm_row追加品,m_rひも位置,m_dひも幅,_r文字領域,m_bandList

        _文字列 'm_p位置,_r文字領域

        _横軸線 'm_listLine
        _縦軸線 'm_listLine
    End Enum

    'システム色
    Enum LineTypeEnum
        _nodef = 0
        _black_thin
        _black_thick
        _black_dot
        _red
        _blue
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

    '編みかた・底楕円(差しひも含む)
    Sub New(ByVal imageType As ImageTypeEnum, ByVal rows As clsGroupDataRow, ByVal idx2 As Integer)
        m_ImageType = imageType
        m_groupRow = rows
        m_Index2 = idx2
        If m_groupRow IsNot Nothing Then
            m_Index = m_groupRow.GetNameValue("f_i番号") '一致項目
        End If
    End Sub

    '付属品
    Sub New(ByVal imageType As ImageTypeEnum, ByVal row As tbl追加品Row)
        m_ImageType = imageType
        m_row追加品 = row
        m_Index = row.f_i番号
        m_Index2 = row.f_iひも番号
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

        '_r文字領域セット
        p_p文字位置 = p

    End Sub

    '*バンドの描画
    'バンドセット
    Sub New(ByVal bandlist As CBandList, ByVal idx1 As Integer, ByVal idx2 As Integer)
        m_ImageType = ImageTypeEnum._バンドセット
        m_Index = idx1
        m_Index2 = idx2
        If bandlist Is Nothing Then
            m_bandList = New CBandList
        Else
            m_bandList = bandlist
        End If
    End Sub
    'バンド
    Sub New(ByVal band As CBand, ByVal idx1 As Integer, ByVal idx2 As Integer)
        m_ImageType = ImageTypeEnum._バンドセット
        m_Index = idx1
        m_Index2 = idx2
        m_bandList = New CBandList
        If band IsNot Nothing Then
            m_bandList.Add(band) '有効なバンドのみ
        End If
    End Sub
    'バンド化
    Sub AddBand(ByVal band As CBand, ByVal idx1 As Integer, ByVal idx2 As Integer)
        If band IsNot Nothing Then
            m_ImageType = ImageTypeEnum._バンドセット
            If m_bandList Is Nothing Then
                m_bandList = New CBandList
                m_Index = idx1
                m_Index2 = idx2
            End If
            m_bandList.Add(band)
        End If
    End Sub
    'バンドセットの下に描画
    Sub AddClip(ByVal bandlist As CBandList)
        'm_ImageType = ImageTypeEnum._バンドセット の想定
        If m_clipList Is Nothing Then
            m_clipList = New CBandList
        End If
        If bandlist IsNot Nothing Then
            m_clipList.AddRange(bandlist)
        End If
    End Sub
    'バンドの下に描画
    Sub AddClip(ByVal band As CBand)
        'm_ImageType = ImageTypeEnum._バンドセット の想定
        If m_clipList Is Nothing Then
            m_clipList = New CBandList
        End If
        If band IsNot Nothing Then
            m_clipList.Add(band)
        End If
    End Sub
    'バンドの下に描画
    Sub AddClip(ByVal item As clsImageItem)
        'm_ImageType = ImageTypeEnum._バンドセット の想定
        If m_clipList Is Nothing Then
            m_clipList = New CBandList
        End If
        If item IsNot Nothing AndAlso item.m_bandList IsNot Nothing Then
            m_clipList.AddRange(item.m_bandList)
        End If
    End Sub
    'とも編み
    Sub AddSameClip(ByVal item As clsImageItem)
        'm_ImageType = ImageTypeEnum._バンドセット の想定
        If m_clipList Is Nothing Then
            m_clipList = New CBandList
        End If
        If item IsNot Nothing AndAlso item.m_clipList IsNot Nothing Then
            m_clipList.AddRange(item.m_clipList)
        End If
    End Sub
    '円を抜く
    Sub AddClip(ByVal circle As S円)
        '円は今のところ1点のみとする
        m_is円 = True
        m_a四隅 = New S四隅(circle.r外接領域)
    End Sub
    '描画領域を限定
    Sub SetDrawArea(ByVal r As S領域)
        m_aDraw = New S四隅(r)
    End Sub


    '文字位置
    Public Property p_p文字位置 As S実座標
        Get
            Return _p文字位置
        End Get
        Set(value As S実座標)
            _p文字位置 = value

            '_r文字領域におおよその領域をセット
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
                    Case ImageTypeEnum._底楕円 ', ImageTypeEnum._差しひも
                        If m_groupRow IsNot Nothing Then
                            chars = Len(m_groupRow.GetNameValue("f_s編みかた名")) '差しひもは0
                            chars += 1 '記号1点
                            line = 1
                        End If
                    Case ImageTypeEnum._付属品, ImageTypeEnum._付属品0
                        If m_row追加品 IsNot Nothing Then
                            chars = Len(m_row追加品.f_s付属品名) + Len(m_row追加品.f_s付属品ひも名) + Len(m_row追加品.f_s記号)
                            chars += 1 '"/"分
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
                    _r文字領域 = New S領域(p_p文字位置, p_p文字位置 + delta)
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

            Case ImageTypeEnum._バンドセット
                r描画領域 = m_bandList.Get描画領域()

            Case ImageTypeEnum._コマ
                r描画領域 = m_rひも位置
                '他は領域内

            Case ImageTypeEnum._編みかた
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(_r文字領域) 'm_p文字位置を含む
                r描画領域 = r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._付属品, ImageTypeEnum._付属品0
                'Debug.Print("m_dひも幅({0}) m_rひも位置 {1}", m_dひも幅, m_rひも位置.ToString)
                r描画領域 = m_rひも位置.get拡大領域(m_dひも幅)
                r描画領域 = r描画領域.get拡大領域(_r文字領域) 'm_p文字位置を含む
                If m_bandList IsNot Nothing Then
                    r描画領域 = r描画領域.get拡大領域(m_bandList.Get描画領域())
                End If

            Case ImageTypeEnum._底枠, ImageTypeEnum._四隅領域, ImageTypeEnum._四隅領域線
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._横の側面, ImageTypeEnum._縦の側面
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._底楕円 ', ImageTypeEnum._差しひも
                r描画領域 = m_a四隅.r外接領域
                r描画領域 = r描画領域.get拡大領域(_r文字領域) 'm_p文字位置を含む
                r描画領域 = r描画領域.get拡大領域(m_lineList.Get描画領域())

            Case ImageTypeEnum._底の中央線, ImageTypeEnum._底枠2
                r描画領域 = m_lineList.Get描画領域()

            Case ImageTypeEnum._折り返し線
                r描画領域 = m_lineList.Get描画領域()

            Case ImageTypeEnum._文字列 'm_p文字位置を含む
                r描画領域 = _r文字領域

            Case ImageTypeEnum._横軸線, ImageTypeEnum._横軸線
                '描画領域の後に作成されるが一応
                r描画領域 = m_lineList.Get描画領域()

            Case ImageTypeEnum._画像保存, ImageTypeEnum._画像貼付
                r描画領域 = m_a四隅.r外接領域

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
        'If m_regionList IsNot Nothing AndAlso 0 < m_regionList.Count Then
        '    sb.AppendLine(m_regionList.ToString)
        'End If
        sb.AppendLine(m_lineList.ToString)
        Return sb.ToString
    End Function
End Class

Public Class clsImageItemList
    Inherits List(Of clsImageItem)

    Sub New()
        MyBase.New
    End Sub

    Sub New(ByVal tmptable As tbl縦横展開DataTable, Optional nomark As Boolean = False)
        MyBase.New
        If tmptable Is Nothing Then
            Exit Sub
        End If
        For Each row As tbl縦横展開Row In tmptable
            Dim band As New clsImageItem(row, nomark)
            Me.AddItem(band)
        Next
    End Sub

    '要素追加
    Sub AddItem(ByVal item As clsImageItem)
        If item IsNot Nothing Then
            MyBase.Add(item)
        End If
    End Sub

    '位置を指定して追加
    Sub AddItem(ByVal item As clsImageItem, ByVal ix As Integer)
        If Count < ix Then
            Do While Count < ix
                MyBase.Add(Nothing)
            Loop
        End If
        If Count = ix Then
            MyBase.Add(item) 'nothingもあり
        ElseIf ix < Count Then
            Me(ix) = item
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
    'Function GetRowItem(ByVal iひも種 As Integer, ByVal iひも番号 As Integer, Optional ByVal create As Boolean = True) As clsImageItem
    Function GetRowItem(ByVal iひも種 As Integer, ByVal iひも番号 As Integer) As clsImageItem
        For Each band As clsImageItem In Me
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If
            If band.m_row縦横展開.f_iひも種 = iひも種 AndAlso band.m_row縦横展開.f_iひも番号 = iひも番号 Then
                Return band
            End If
        Next
        'If create Then
        '    'あるはずなので
        '    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "clsBandList.GetRowItem 追加({0},{1})", iひも種, iひも番号)
        '    Dim add As New clsImageItem((New tbl縦横展開DataTable).Newtbl縦横展開Row)
        '    add.m_row縦横展開.f_iひも種 = iひも種
        '    add.m_row縦横展開.f_iひも番号 = iひも番号
        '    Me.AddItem(add)
        '    Return add
        'Else
        Return Nothing
        'End If
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
