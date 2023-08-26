Public Class frmBandTypeWeight

    Public Property p_sバンドの種類名 As String
    Public Property p_s長さと重さ As String

    Shared _digit_point() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "."}

    Dim _d_長さ As Double = -1
    Dim _d_重さ As Double = -1

    Private Function isValid() As Boolean
        Return 0 < _d_長さ AndAlso 0 < _d_重さ
    End Function

    Private Sub frmBandTypeWeight_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = String.Format(My.Resources.CaptionBandTypeWeight, p_sバンドの種類名)
        txt長さと重さ.Text = p_s長さと重さ
        If Not split_values() Then
            '<数値> <長さの単位> = <数値> <重さの単位> のように入力
            ToolStripStatusLabel1.Text = My.Resources.ErrLengthWeightValue
        Else
            '長さを入力して[→] / 重さを入力して[←]
            ToolStripStatusLabel1.Text = My.Resources.GuideLengthWeigt
        End If
    End Sub

    '[OK]
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If isValid() AndAlso p_s長さと重さ <> txt長さと重さ.Text Then
            p_s長さと重さ = txt長さと重さ.Text
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    '長さと重さの再入力
    Private Sub txt長さと重さ_TextChanged(sender As Object, e As EventArgs) Handles txt長さと重さ.TextChanged
        If Not split_values() Then
            '<数値> <長さの単位> = <数値> <重さの単位> のように入力
            ToolStripStatusLabel1.Text = My.Resources.ErrLengthWeightValue
        Else
            '長さを入力して[→] / 重さを入力して[←]
            ToolStripStatusLabel1.Text = My.Resources.GuideLengthWeigt
        End If
    End Sub

    '[→]
    Private Sub btn長さ重さ_Click(sender As Object, e As EventArgs) Handles btn長さ重さ.Click
        If Not isValid() Then
            '『長さと重さ』値を正しくセットしてください
            ToolStripStatusLabel1.Text = My.Resources.ErrLengthWeight
            Exit Sub
        End If
        Dim len As Double
        If Not Double.TryParse(txt長さ.Text, len) Then
            '{0}の値をセットしてください。
            ToolStripStatusLabel1.Text = String.Format(My.Resources.ErrBadValue, lbl長さ.Text)
            Exit Sub
        End If

        Dim weight As Double = len / _d_長さ * _d_重さ
        txt重さ.Text = output_string(weight)
    End Sub

    '[←]
    Private Sub btn重さ長さ_Click(sender As Object, e As EventArgs) Handles btn重さ長さ.Click
        If Not isValid() Then
            '『長さと重さ』値を正しくセットしてください
            ToolStripStatusLabel1.Text = My.Resources.ErrLengthWeight
        End If
        Dim weight As Double
        If Not Double.TryParse(txt重さ.Text, weight) Then
            '{0}の値をセットしてください。
            ToolStripStatusLabel1.Text = String.Format(My.Resources.ErrBadValue, lbl重さ.Text)
            Exit Sub
        End If

        Dim len As Double = weight / _d_重さ * _d_長さ
        txt長さ.Text = output_string(len)
    End Sub

    '[クリア]
    Private Sub btnクリア_Click(sender As Object, e As EventArgs) Handles btnクリア.Click
        txt長さ.Text = Nothing
        txt重さ.Text = Nothing
        ToolStripStatusLabel1.Text = Nothing
    End Sub

    Private Function split_values() As Boolean
        lbl長さ単位.Text = Nothing
        lbl重さ単位.Text = Nothing
        _d_長さ = -1
        _d_重さ = -1

        Dim txt As String = txt長さと重さ.Text
        '=
        Dim ary() As String = txt.Split("=")
        If ary.Length <> 2 Then
            Return False
        End If

        Dim val0 As Double
        Dim unit0 As String = Nothing
        Dim val1 As Double
        Dim unit1 As String = Nothing
        If Not value_unit(ary(0), val0, unit0) OrElse Not value_unit(ary(1), val1, unit1) Then
            Return False
        End If

        lbl長さ単位.Text = unit0
        lbl重さ単位.Text = unit1
        _d_長さ = val0
        _d_重さ = val1

        Return isValid()
    End Function

    Private Function value_unit(ByVal str As String, ByRef val As Double, ByRef unit As String) As Boolean
        str = str.Trim
        Dim valstr As String = ""
        unit = ""
        For pos As Integer = 0 To str.Count - 1
            Dim c As String = str.Substring(pos, 1)
            If _digit_point.Contains(c) Then
                If String.IsNullOrEmpty(unit) Then
                    valstr += c
                Else
                    Return False
                End If
            Else
                If String.IsNullOrEmpty(unit) Then
                    unit = str.Substring(pos)
                End If
            End If
        Next

        If Double.TryParse(valstr, val) Then
            Return True
        End If
        Return False
    End Function

    Private Function output_string(ByVal d As Double) As String
        If d - System.Math.Floor(d) = 0 Then
            '小数以下がない
            Return d.ToString
        ElseIf 100 < d Then
            Return d.ToString("0.0")
        Else
            Return d.ToString("0.00")
        End If
    End Function

End Class