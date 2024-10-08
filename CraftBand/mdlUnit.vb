﻿Public Module mdlUnit

    Const cMilliCentiRatio As Integer = 10
    Const cMilliInchRatio As Integer = 25.4
    Const cMilliMeterRatio As Integer = 1000
    Const cMilliFeetRatio As Integer = 304.8

    '寸法単位
    Structure LUnit
        Implements IEquatable(Of LUnit)

        '単位表記
        Private Shared _UnitString() As String = {"mm", "cm", "in", "m", "ft"}
        '設定用の小数点位置
        Private Shared _DecimalPlaces() As Integer = {1, 2, 3, 2, 3}

        Private Enum LengthUnit
            mm = 0
            cm = 1
            inch = 2 'inはキーワードのため使用できない
            m = 3
            feet = 4
        End Enum

        Private _LengthUnit As LengthUnit


        Sub New(ByVal s As String)
            Str = s
        End Sub

        Sub New(ByVal i As Integer)
            Int = i
        End Sub

        Sub New(ByVal ref As LUnit)
            _LengthUnit = ref._LengthUnit
        End Sub

        Public ReadOnly Property Is_mm As Boolean
            Get
                Return _LengthUnit = LengthUnit.mm
            End Get
        End Property
        Public ReadOnly Property Is_cm As Boolean
            Get
                Return _LengthUnit = LengthUnit.cm
            End Get
        End Property
        Public ReadOnly Property Is_inch As Boolean
            Get
                Return _LengthUnit = LengthUnit.inch
            End Get
        End Property
        Public ReadOnly Property Is_m As Boolean
            Get
                Return _LengthUnit = LengthUnit.m
            End Get
        End Property
        Public ReadOnly Property Is_feet As Boolean
            Get
                Return _LengthUnit = LengthUnit.feet
            End Get
        End Property

        Public Sub Set_inch()
            _LengthUnit = LengthUnit.inch
        End Sub
        Public Sub Set_cm()
            _LengthUnit = LengthUnit.cm
        End Sub
        Public Sub Set_mm()
            _LengthUnit = LengthUnit.mm
        End Sub
        Public Sub Set_m()
            _LengthUnit = LengthUnit.m
        End Sub
        Public Sub Set_feet()
            _LengthUnit = LengthUnit.feet
        End Sub


        Public Property Int() As Integer
            Get
                Return CType(_LengthUnit, Integer)
            End Get
            Set(value As Integer)
                If 0 <= value AndAlso value < _UnitString.Length Then
                    _LengthUnit = CType(value, LengthUnit)
                Else
                    _LengthUnit = LengthUnit.mm
                End If
            End Set
        End Property

        Public Property Str() As String
            Get
                Return _UnitString(CType(_LengthUnit, Integer))
            End Get
            Set(value As String)
                If Not String.IsNullOrWhiteSpace(value) Then
                    For i As Integer = 0 To _UnitString.Length - 1
                        If String.Compare(value, _UnitString(i), True) = 0 Then
                            _LengthUnit = CType(i, LengthUnit)
                            Exit Property
                        End If
                    Next
                End If
                _LengthUnit = LengthUnit.mm
            End Set
        End Property


        Public ReadOnly Property DecimalPlaces As Integer
            Get
                Return _DecimalPlaces(CType(_LengthUnit, Integer))
            End Get
        End Property

        Public Function Text(ByVal d As Double) As String
            Return TextDecimalPlaces(d, DecimalPlaces)
        End Function

        Public Function Text(ByVal len As Length) As String
            If Is_inch Then
                Return Text(len.Inch)
            ElseIf is_cm Then
                Return Text(len.Centimeter)
            ElseIf Is_m Then
                Return Text(len.Meter)
            ElseIf Is_feet Then
                Return Text(len.Feet)
            Else
                Return Text(len.Millimeter)
            End If
        End Function

        Public Shared Function TextDecimalPlaces(ByVal d As Double, ByVal decipl As Integer) As String
            Dim format As String = String.Format("N{0}", decipl)
            Return d.ToString(format)
        End Function

        Public Function TextDecimalPlaces(ByVal len As Length, ByVal decipl As Integer) As String
            If Is_inch Then
                Return TextDecimalPlaces(len.Inch, decipl)
            ElseIf Is_cm Then
                Return TextDecimalPlaces(len.Centimeter, decipl)
            ElseIf Is_m Then
                Return TextDecimalPlaces(len.Meter, decipl)
            ElseIf Is_feet Then
                Return TextDecimalPlaces(len.Feet, decipl)
            Else
                Return TextDecimalPlaces(len.Millimeter, decipl)
            End If
        End Function


        Public Function TextWithUnit(ByVal len As Length) As String
            Return Text(len) & Str
        End Function

        Public Function TextWithUnit(ByVal d As Double, ByVal sign As Boolean) As String
            If 0 < d AndAlso sign Then
                Return "+" & Text(d) & Str
            Else
                Return Text(d) & Str
            End If
        End Function

        '集計用の出力
        Public Function SumTextWithUnit(ByVal len As Length, ByVal decipl As Integer) As String
            If Is_inch OrElse Is_feet Then
                Return TextDecimalPlaces(len.Feet, decipl) & _UnitString(CType(LengthUnit.feet, Integer))
            ElseIf Is_cm OrElse Is_m Then
                Return TextDecimalPlaces(len.Meter, decipl) & _UnitString(CType(LengthUnit.m, Integer))
            Else
                Return TextDecimalPlaces(len.Centimeter, decipl) & _UnitString(CType(LengthUnit.cm, Integer))
            End If
        End Function

        Public Overrides Function ToString() As String
            Return Str
        End Function

        Public Overloads Function Equals(other As LUnit) As Boolean Implements IEquatable(Of LUnit).Equals
            Return _LengthUnit = other._LengthUnit
        End Function
    End Structure

    '寸法値
    Structure Length
        Implements IEquatable(Of Length)
        Implements IComparable(Of Length)

        Dim _Millimeter As Double  '内部値はミリメートル

        Sub New(ByVal ref As Length)
            _Millimeter = ref._Millimeter
        End Sub

        Sub New(ByVal d As Double, ByVal uni As LUnit)
            If uni.Is_inch Then
                Inch = d
            ElseIf uni.Is_cm Then
                Centimeter = d
            ElseIf uni.Is_m Then
                Meter = d
            ElseIf uni.Is_feet Then
                Feet = d
            Else
                Millimeter = d
            End If
        End Sub

        Sub New(ByVal d As Double, ByVal unitstr As String)
            Me.New(d, New LUnit(unitstr))
        End Sub


        '単位省略はg_clsBasics参照
        Sub New(ByVal d As Double)
            Me.New(d, g_clsSelectBasics.p_unit設定時の寸法単位)
        End Sub


        '現設定の何本幅分か
        Private Property ByLane() As Double  '現設定の何本幅分か
            Get
                Return _Millimeter / (g_clsSelectBasics.p_lenバンド幅._Millimeter / g_clsSelectBasics.p_i本幅)
            End Get
            Set(value As Double)
                _Millimeter = value * (g_clsSelectBasics.p_lenバンド幅._Millimeter / g_clsSelectBasics.p_i本幅)
            End Set
        End Property
        Public ReadOnly Property ByLaneText As String
            Get
                If g_clsSelectBasics.p_isバンド個別幅 Then
                    Return "--"
                Else
                    Return ByLane.ToString("F1")
                End If
            End Get
        End Property

        'ミリメートル値
        Public Property Millimeter() As Double  '
            Get
                Return _Millimeter
            End Get
            Set(value As Double)
                _Millimeter = value
            End Set
        End Property

        'センチメートル
        Public Property Centimeter() As Double  '
            Get
                Return _Millimeter / cMilliCentiRatio
            End Get
            Set(value As Double)
                _Millimeter = value * cMilliCentiRatio
            End Set
        End Property

        'インチ
        Public Property Inch() As Double  '
            Get
                Return _Millimeter / cMilliInchRatio
            End Get
            Set(value As Double)
                _Millimeter = value * cMilliInchRatio
            End Set
        End Property

        'メートル
        Public Property Meter() As Double  '
            Get
                Return _Millimeter / cMilliMeterRatio
            End Get
            Set(value As Double)
                _Millimeter = value * cMilliMeterRatio
            End Set
        End Property

        'フィート
        Public Property Feet() As Double  '
            Get
                Return _Millimeter / cMilliFeetRatio
            End Get
            Set(value As Double)
                _Millimeter = value * cMilliFeetRatio
            End Set
        End Property


        Public ReadOnly Property IsPlus As Boolean
            Get
                Return 0 < _Millimeter
            End Get
        End Property


        'ドット以下四捨五入(-1.5→-2)
        Function DotRound() As Length
            _Millimeter = Math.Round(_Millimeter, MidpointRounding.AwayFromZero) '四捨五入
            Return Me
        End Function

        'ドット以下切り捨て(-1.5→-1)
        Function DotDown() As Length
            _Millimeter = Math.Truncate(_Millimeter)
            Return Me
        End Function



        '二項+演算子 
        Public Shared Operator +(ByVal c1 As Length, ByVal c2 As Length) As Length
            Dim plus As New Length
            plus._Millimeter = c1._Millimeter + c2._Millimeter
            Return plus
        End Operator

        '二項-演算子 
        Public Shared Operator -(ByVal c1 As Length, ByVal c2 As Length) As Length
            Dim minus As New Length
            minus._Millimeter = c1._Millimeter - c2._Millimeter
            Return minus
        End Operator

        '二項*演算子 
        Public Shared Operator *(ByVal c1 As Length, ByVal d As Double) As Length
            Dim multi As New Length
            multi._Millimeter = c1._Millimeter * d
            Return multi
        End Operator
        Public Shared Operator *(ByVal c1 As Length, ByVal i As Integer) As Length
            Dim multi As New Length
            multi._Millimeter = c1._Millimeter * i
            Return multi
        End Operator

        '二項/演算子 
        Public Shared Operator /(ByVal c1 As Length, ByVal d As Double) As Length
            Dim div As New Length
            div._Millimeter = c1._Millimeter / d
            Return div
        End Operator
        Public Shared Operator /(ByVal c1 As Length, ByVal i As Integer) As Length
            Dim div As New Length
            div._Millimeter = c1._Millimeter / i
            Return div
        End Operator

        '二項\演算子 
        Public Shared Operator \(ByVal c1 As Length, ByVal i As Integer) As Length
            Dim div As New Length
            div._Millimeter = c1._Millimeter / i
            div.DotDown()
            Return div
        End Operator

        '二項Mod演算子
        Public Shared Operator Mod(ByVal c1 As Length, ByVal i As Integer) As Length
            Dim div As Length = c1 \ i
            Return (c1 - div)
        End Operator

        '単項-演算子(マイナス符号)
        Public Shared Operator -(ByVal c1 As Length) As Length
            Dim minus As New Length
            minus._Millimeter = -c1._Millimeter
            Return minus
        End Operator

        '比較演算子
        Public Shared Operator =(ByVal c1 As Length, ByVal c2 As Length) As Boolean
            Return (c1._Millimeter = c2._Millimeter)
        End Operator

        Public Shared Operator <>(ByVal c1 As Length, ByVal c2 As Length) As Boolean
            Return Not (c1 = c2)
        End Operator

        Public Shared Operator <(ByVal c1 As Length, ByVal c2 As Length) As Boolean
            Return (c1._Millimeter < c2._Millimeter)
        End Operator
        Public Shared Operator >(ByVal c1 As Length, ByVal c2 As Length) As Boolean
            Return (c1._Millimeter > c2._Millimeter)
        End Operator

        Public Shared Operator <=(ByVal c1 As Length, ByVal c2 As Length) As Boolean
            Return (c1._Millimeter <= c2._Millimeter)
        End Operator
        Public Shared Operator >=(ByVal c1 As Length, ByVal c2 As Length) As Boolean
            Return (c1._Millimeter >= c2._Millimeter)
        End Operator


        Public Overloads Function Equals(other As Length) As Boolean Implements IEquatable(Of Length).Equals
            Return Me._Millimeter.Equals(other._Millimeter)
        End Function

        Public Function CompareTo(other As Length) As Integer Implements IComparable(Of Length).CompareTo
            Return Me._Millimeter.CompareTo(other._Millimeter)
        End Function

        Public Function Value(ByVal uni As LUnit) As Double
            If uni.Is_inch Then
                Return Inch
            ElseIf uni.Is_cm Then
                Return Centimeter
            ElseIf uni.Is_m Then
                Return Meter
            ElseIf uni.Is_feet Then
                Return Feet
            Else
                Return Millimeter
            End If
        End Function
        Public Function Value() As Double
            Return Value(g_clsSelectBasics.p_unit設定時の寸法単位)
        End Function

        Public Function Text(ByVal uni As LUnit) As String
            Return uni.Text(Me)
        End Function
        Public Function Text() As String
            Return g_clsSelectBasics.p_unit設定時の寸法単位.Text(Me)
        End Function

        Public Function TextWithUnit(ByVal uni As LUnit) As String
            Return uni.TextWithUnit(Me)
        End Function
        Public Function TextWithUnit() As String
            Return g_clsSelectBasics.p_unit設定時の寸法単位.TextWithUnit(Me)
        End Function


        Public Overrides Function ToString() As String
            Return String.Format("[{0} mm/ {1} cm/ {2} in/ {3} m/ {4} ft/{5} ByLine]", Millimeter, Centimeter, Inch, Meter, Feet, ByLane)
        End Function
    End Structure


    '*共用関数

    '最小
    Public Function Min(ByVal x1 As Double, ByVal x2 As Double) As Double
        If x1 <= x2 Then
            Return x1
        Else
            Return x2
        End If
    End Function
    Public Function Min(ByVal x1 As Double, ByVal x2 As Double, ByVal x3 As Double, ByVal x4 As Double) As Double
        Return Min(Min(x1, x2), Min(x3, x4))
    End Function

    '最大
    Public Function Max(ByVal x1 As Double, ByVal x2 As Double) As Double
        If x1 <= x2 Then
            Return x2
        Else
            Return x1
        End If
    End Function
    Public Function Max(ByVal x1 As Double, ByVal x2 As Double, ByVal x3 As Double, ByVal x4 As Double) As Double
        Return Max(Max(x1, x2), Max(x3, x4))
    End Function

    '絶対値
    Public Function Abs(ByVal x As Double) As Double
        If x < 0 Then
            Return -x
        Else
            Return x
        End If
    End Function

    'ボタンの文字列取得
    Public Function ControlText(ByVal obj As Object) As String
        Try
            Dim ctrl As System.Windows.Forms.Control = CType(obj, System.Windows.Forms.Control)
            Dim text = ctrl.Text
            If String.IsNullOrEmpty(text) OrElse text.Length < 2 Then
                Return text
            End If
            '(&x)
            Dim nPh As Integer = text.IndexOf("(")
            If 1 < nPh Then
                Return text.Substring(0, nPh)
            End If
            '&A
            Return text.Replace("&", "")

        Catch ex As Exception
            g_clsLog.LogException(ex, "ControlText", obj)
            Return Nothing
        End Try
    End Function

    'カッコ付き文字列を返す、空でなければ
    Public Function Parentheses(ByVal obj As Object) As String
        If obj Is Nothing OrElse IsDBNull(obj) Then
            Return ""
        End If
        Dim str As String = obj.ToString
        If String.IsNullOrWhiteSpace(str) Then
            Return str
        End If
        Return "(" & str & ")"
    End Function

    'マイナスを許す剰余値 0～range-1 の値で返す
    Public Function Modulo(ByVal val As Integer, ByVal range As Integer) As Integer
        If range <= 0 Then
            Return (val Mod range)  'システム通り
        End If
        Dim v As Integer = val Mod range 'VBでは、-(range-1) ～ (range-1)の値
        If 0 <= v Then
            Return v
        Else
            Return v + range
        End If
    End Function


End Module
