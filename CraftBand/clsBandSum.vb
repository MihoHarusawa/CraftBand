Public Class clsBandSum

    '本幅ごとの長さ
    Class LaneLength
        Inherits SortedDictionary(Of Integer, Double)

        Sub New()
            MyBase.New
        End Sub

        Sub New(ByVal ref As LaneLength)
            MyBase.New
            For Each lane As Integer In ref.Keys
                Me.Add(lane, Me(lane))
            Next
        End Sub
    End Class


    '色と本幅ごと集計(設定単位)
    Dim _ColorBandLength As New Dictionary(Of String, LaneLength)
    '色と本幅ごと最大長(設定単位)
    Dim _ColorBandMaxLength As New Dictionary(Of String, LaneLength)

    Sub New()
    End Sub

    Sub New(ByVal ref As clsBandSum)
        For Each color As String In ref._ColorBandLength.Keys
            _ColorBandLength.Add(color, New LaneLength(ref._ColorBandLength(color)))
        Next
        For Each color As String In ref._ColorBandMaxLength.Keys
            _ColorBandMaxLength.Add(color, New LaneLength(ref._ColorBandMaxLength(color)))
        Next
    End Sub

    '登録色
    Function Colors() As String()
        Return _ColorBandLength.Keys().ToArray
    End Function

    '指定色の集計
    Function BandLength(ByVal color As String) As LaneLength
        If _ColorBandLength.ContainsKey(color) Then
            Return _ColorBandLength(color)
        Else
            Return Nothing
        End If
    End Function

    '指定色の最大長
    Function BandMaxLength(ByVal color As String) As LaneLength
        If _ColorBandMaxLength.ContainsKey(color) Then
            Return _ColorBandMaxLength(color)
        Else
            Return Nothing
        End If
    End Function


    'ひもの集計にプラスする
    Function Add(ByVal count As Integer, ByVal lane As Integer, ByVal length As Double, ByVal color As String) As Boolean
        If count = 0 OrElse lane = 0 OrElse length <= 0 Then
            Return False
        End If
        If String.IsNullOrWhiteSpace(color) Then
            color = ""
        End If

        'ひもの集計(同一色同時登録)
        Dim bandLength As LaneLength
        Dim bandMaxLength As LaneLength
        If _ColorBandLength.ContainsKey(color) Then
            bandLength = _ColorBandLength(color)
            bandMaxLength = _ColorBandMaxLength(color)
        Else
            bandLength = New LaneLength
            _ColorBandLength.Add(color, bandLength)
            bandMaxLength = New LaneLength
            _ColorBandMaxLength.Add(color, bandMaxLength)
        End If
        If bandLength.ContainsKey(lane) Then
            bandLength(lane) = bandLength(lane) + count * length
            If bandMaxLength(lane) < length Then
                bandMaxLength(lane) = length
            End If
        Else
            bandLength.Add(lane, count * length)
            bandMaxLength.Add(lane, length)
        End If
        Return True
    End Function



    Public Sub Clear()
        _ColorBandLength.Clear()
        _ColorBandMaxLength.Clear()
    End Sub


End Class
