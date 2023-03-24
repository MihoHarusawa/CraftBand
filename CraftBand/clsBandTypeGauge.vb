Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports CraftBand.Tables
Imports CraftBand.Tables.dstMasterTables

Public Class clsBandTypeGauge

    Const c_2D寸法比率 As Double = 2.236
    Const c_2D要尺比率 As Double = 6.43

    Dim _dコマ寸法係数a As Double 'コマ寸法のa(傾き)を2D値で割った値
    Dim _dコマ寸法係数b As Double 'コマ寸法のb(切片)をひも幅で割った値
    Dim _dコマ要尺係数a As Double 'コマ要尺のa(傾き)を2D値で割った値
    Dim _dコマ要尺係数b As Double 'コマ要尺のb(切片)をひも幅で割った値
    Dim _bCoefficientChanged As Boolean = False

    Dim _sバンドの種類名 As String
    Dim _i本幅 As Integer
    Dim _dバンド幅 As Double

    Public GauteDataTable As dstWork.tblGaugeDataTable = Nothing
    Public Const c_SortOrder As String = "f_i本幅"
    'バンドの種類が正しく設定されていません。
    Public Shared ReadOnly Property p_sErrKnotBandType As String = My.Resources.ErrKnotBandType

    Dim _NoCalcIndex() As Integer = {1}

#Region "プロパティ値"
    Public ReadOnly Property p_sバンドの種類名 As String
        Get
            Return _sバンドの種類名
        End Get
    End Property

    Public Property p_dコマ寸法係数a As Double
        Get
            Return _dコマ寸法係数a
        End Get
        Set(value As Double)
            If value <> _dコマ寸法係数a Then
                _dコマ寸法係数a = value
                _bCoefficientChanged = True
            End If
        End Set
    End Property
    Public Property p_dコマ寸法係数b As Double
        Get
            Return _dコマ寸法係数b
        End Get
        Set(value As Double)
            If value <> _dコマ寸法係数b Then
                _dコマ寸法係数b = value
                _bCoefficientChanged = True
            End If
        End Set
    End Property
    Public Property p_dコマ要尺係数a As Double
        Get
            Return _dコマ要尺係数a
        End Get
        Set(value As Double)
            If value <> _dコマ要尺係数a Then
                _dコマ要尺係数a = value
                _bCoefficientChanged = True
            End If
        End Set
    End Property
    Public Property p_dコマ要尺係数b As Double
        Get
            Return _dコマ要尺係数b
        End Get
        Set(value As Double)
            If value <> _dコマ要尺係数b Then
                _dコマ要尺係数b = value
                _bCoefficientChanged = True
            End If
        End Set
    End Property

    Public ReadOnly Property IsValid As Boolean = False

    Public ReadOnly Property IsChanged As Boolean
        Get
            Return IsValid AndAlso (_bCoefficientChanged OrElse GauteDataTable.GetChanges() IsNot Nothing)
        End Get
    End Property
#End Region


    Sub New(ByVal base As clsMasterTables, ByVal bandtypename As String)
        _sバンドの種類名 = bandtypename

        'tblバンドの種類Row
        Dim crow As clsDataRow = base.GetBandTypeRecord(_sバンドの種類名, False)
        If crow Is Nothing Then
            Exit Sub
        End If
        With crow
            _dコマ寸法係数a = .Value("f_dコマ寸法係数a")
            _dコマ寸法係数b = .Value("f_dコマ寸法係数b")
            _dコマ要尺係数a = .Value("f_dコマ要尺係数a")
            _dコマ要尺係数b = .Value("f_dコマ要尺係数b")
            _i本幅 = .Value("f_i本幅")
            _dバンド幅 = .Value("f_dバンド幅")
        End With

        '全本幅のレコード作成
        GauteDataTable = New dstWork.tblGaugeDataTable
        For i As Integer = 1 To _i本幅
            Dim workrow As dstWork.tblGaugeRow = GauteDataTable.NewtblGaugeRow
            workrow.f_sバンドの種類名 = _sバンドの種類名
            workrow.f_i本幅 = i
            workrow.f_dひも幅 = (_dバンド幅 / _i本幅) * i
            workrow.f_dコマ寸法計算値 = calcコマ寸法計算値(workrow.f_dひも幅)
            workrow.f_dコマ要尺計算値 = calcコマ要尺計算値(workrow.f_dひも幅)
            If Not _NoCalcIndex.Contains(i) Then
                workrow.f_b係数取得区分 = True
            End If
            GauteDataTable.Rows.Add(workrow)
        Next

        '既存レコード値取得
        Dim gauges() As tblゲージRow = base.GetBandTypeGauges(_sバンドの種類名)
        If gauges IsNot Nothing Then
            For Each gauge As tblゲージRow In gauges
                If 0 < gauge.f_i本幅 AndAlso gauge.f_i本幅 <= _i本幅 Then
                    Dim cond As String = String.Format("f_i本幅 = {0}", gauge.f_i本幅)
                    Dim rows() As dstWork.tblGaugeRow = GauteDataTable.Select(cond)
                    If 0 < rows.Count Then 'First only
                        With rows(0)
                            If 0 < gauge.f_dコマ寸法実測値 Then 'MasterにはNullなし
                                .f_dコマ寸法実測値 = gauge.f_dコマ寸法実測値
                            End If
                            If 0 < gauge.f_dコマ要尺実測値 Then
                                .f_dコマ要尺実測値 = gauge.f_dコマ要尺実測値
                            End If
                            .f_b実測値使用区分 = gauge.f_b実測値使用区分
                            .f_s備考 = gauge.f_s備考
                        End With
                    End If
                End If
            Next
        End If
        GauteDataTable.AcceptChanges()

        _IsValid = True
    End Sub


    Public Function IsCurrentBandTypeName(ByVal bandtypename As String) As Boolean
        If Not IsValid Then
            Return False
        End If
        Return 0 = String.Compare(bandtypename, _sバンドの種類名, True)
    End Function

    Public Function Getコマ寸法値(ByVal i本幅 As Integer) As Double
        If Not IsValid Then
            Return -1 'Error
        End If

        Dim cond As String = String.Format("f_i本幅 = {0}", i本幅)
        Dim rows() As dstWork.tblGaugeRow = GauteDataTable.Select(cond)
        If 0 < rows.Count Then 'First only
            With rows(0)
                If .f_b実測値使用区分 AndAlso Not .Isf_dコマ寸法実測値Null Then
                    Return .f_dコマ寸法実測値
                Else
                    Return .f_dコマ寸法計算値
                End If
            End With
        Else
            Return -2 'Not Found
        End If
    End Function

    Public Function Getコマ要尺値(ByVal i本幅 As Integer) As Double
        If Not IsValid Then
            Return -1 'Error
        End If

        Dim cond As String = String.Format("f_i本幅 = {0}", i本幅)
        Dim rows() As dstWork.tblGaugeRow = GauteDataTable.Select(cond)
        If 0 < rows.Count Then 'First only
            With rows(0)
                If .f_b実測値使用区分 AndAlso Not .Isf_dコマ要尺実測値Null Then
                    Return .f_dコマ要尺実測値
                Else
                    Return .f_dコマ要尺計算値
                End If
            End With
        Else
            Return -2 'Not Found
        End If
    End Function

    Public Function IsValidValues(ByRef errmsg As String) As Boolean
        If Not IsValid Then
            'バンドの種類が正しく設定されていません。
            errmsg = p_sErrKnotBandType
            Return False
        End If

        Dim sb As New System.Text.StringBuilder
        For i As Integer = 1 To _i本幅
            If Getコマ寸法値(i) <= 0 Then
                '{0}本幅のコマ寸法値が不正です。
                sb.AppendFormat(My.Resources.ErrKnotPieceSize, i)
            End If
            If Getコマ要尺値(i) <= 0 Then
                '{0}本幅の要尺寸法値が不正です。
                sb.AppendFormat(My.Resources.ErrKnotPieceLength, i)
            End If
        Next

        If sb.Length = 0 Then
            Return True
        Else
            errmsg = sb.ToString
            Return False
        End If
    End Function

    Public Function UpdateMaster(ByVal base As clsMasterTables) As Boolean
        If Not IsValid Then
            Return False 'No Update
        End If
        Dim changed As Boolean = False

        '係数に更新あり
        If _bCoefficientChanged Then
            '※対象が現在のバンドの種類であっても、p_row選択中バンドの種類　には反映されない(係数値は使用しないため)
            'tblバンドの種類Row
            Dim crow As clsDataRow = base.GetBandTypeRecord(_sバンドの種類名, False)
            If crow IsNot Nothing Then
                With crow
                    .Value("f_dコマ寸法係数a") = _dコマ寸法係数a
                    .Value("f_dコマ寸法係数b") = _dコマ寸法係数b
                    .Value("f_dコマ要尺係数a") = _dコマ要尺係数a
                    .Value("f_dコマ要尺係数b") = _dコマ要尺係数b
                End With
            End If
            changed = True
        End If

        'テーブルに更新あり
        If GauteDataTable.GetChanges() Is Nothing Then
            Return changed
        End If
        'row取得用
        Dim subtable As New tblゲージDataTable

        For Each workrow As dstWork.tblGaugeRow In GauteDataTable.Rows
            '値がセットされている
            Dim isset As Integer = 0
            If Not workrow.Isf_dコマ寸法実測値Null AndAlso workrow.f_dコマ寸法実測値 Then
                isset += 1
            End If
            If Not workrow.Isf_dコマ要尺実測値Null AndAlso workrow.f_dコマ要尺実測値 Then
                isset += 1
            End If
            If workrow.f_b実測値使用区分 Then
                If isset = 0 Then
                    workrow.f_b実測値使用区分 = False
                End If
            End If
            If Not workrow.Isf_s備考Null AndAlso Not String.IsNullOrWhiteSpace(workrow.f_s備考) Then
                isset += 1
            End If
            If isset = 0 AndAlso Not workrow.f_b実測値使用区分 Then
                Continue For
            End If

            Dim row As tblゲージRow = subtable.NewtblゲージRow
            With row
                .f_sバンドの種類名 = workrow.f_sバンドの種類名
                .f_i本幅 = workrow.f_i本幅
                .f_dコマ寸法実測値 = workrow.f_dコマ寸法実測値
                .f_dコマ要尺実測値 = workrow.f_dコマ要尺実測値
                .f_b実測値使用区分 = workrow.f_b実測値使用区分
                .f_s備考 = workrow.f_s備考
            End With
            subtable.Rows.Add(row)
        Next

        'マスター更新
        If base.UpdateBandTypeGauges(_sバンドの種類名, subtable) Then
            changed = True
        End If
        GauteDataTable.AcceptChanges()

        Return changed
    End Function

    '実測値と係数をクリア
    Public Function Reset() As Boolean
        If Not IsValid Then
            Return False
        End If
        '係数をデフォルト値にする
        Dim tmpMasterTables As New dstMasterTables
        Dim r As tblバンドの種類Row = tmpMasterTables.Tables("tblバンドの種類").NewRow
        _dコマ寸法係数a = r.f_dコマ寸法係数a
        _dコマ寸法係数b = r.f_dコマ寸法係数b
        _dコマ要尺係数a = r.f_dコマ要尺係数a
        _dコマ要尺係数b = r.f_dコマ要尺係数b
        tmpMasterTables.Dispose()
        _bCoefficientChanged = True

        For Each workrow As dstWork.tblGaugeRow In GauteDataTable.Rows
            workrow.Setf_dコマ寸法実測値Null()
            workrow.Setf_dコマ要尺実測値Null()
            workrow.Setf_s備考Null()
            workrow.f_b実測値使用区分 = False
            workrow.f_b係数取得区分 = False
            workrow.f_dコマ寸法計算値 = calcコマ寸法計算値(workrow.f_dひも幅)
            workrow.f_dコマ要尺計算値 = calcコマ要尺計算値(workrow.f_dひも幅)
            If Not _NoCalcIndex.Contains(workrow.f_i本幅) Then
                workrow.f_b係数取得区分 = True
            End If
        Next
        Return True
    End Function

    '係数計算
    Public Function CalcCoefficient() As Boolean
        If Not IsValid Then
            Return False
        End If

        Dim coeffコマ寸法 As New CLeastSquare
        Dim coeffコマ要尺 As New CLeastSquare
        For Each workrow As dstWork.tblGaugeRow In GauteDataTable.Rows
            If Not workrow.f_b係数取得区分 Then
                Continue For
            End If
            If Not workrow.Isf_dコマ寸法実測値Null Then
                coeffコマ寸法.addXY(workrow.f_dひも幅, workrow.f_dコマ寸法実測値)
            End If
            If Not workrow.Isf_dコマ要尺実測値Null Then
                coeffコマ要尺.addXY(workrow.f_dひも幅, workrow.f_dコマ要尺実測値)
            End If
        Next

        If coeffコマ寸法.CalcAB() Then
            p_dコマ寸法係数a = coeffコマ寸法.coeff_a / c_2D寸法比率
            p_dコマ寸法係数b = coeffコマ寸法.coeff_b / _dバンド幅
        End If
        If coeffコマ要尺.CalcAB() Then
            p_dコマ要尺係数a = coeffコマ要尺.coeff_a / c_2D要尺比率
            p_dコマ要尺係数b = coeffコマ要尺.coeff_b / _dバンド幅
        End If

        '再計算
        For Each workrow As dstWork.tblGaugeRow In GauteDataTable.Rows
            workrow.f_dコマ寸法計算値 = calcコマ寸法計算値(workrow.f_dひも幅)
            workrow.f_dコマ要尺計算値 = calcコマ要尺計算値(workrow.f_dひも幅)
        Next

        Return coeffコマ寸法.IsValid AndAlso coeffコマ要尺.IsValid
    End Function


    Private Function calcコマ寸法計算値(ByVal x As Double) As Double
        Dim a As Double = _dコマ寸法係数a * c_2D寸法比率
        Dim b As Double = _dコマ寸法係数b * _dバンド幅
        Return a * x + b
    End Function

    Private Function calcコマ要尺計算値(ByVal x As Double) As Double
        Dim a As Double = _dコマ要尺係数a * c_2D要尺比率
        Dim b As Double = _dコマ要尺係数b * _dバンド幅
        Return a * x + b
    End Function

    '最小二乗法
    Private Class CLeastSquare
        Dim _x As New List(Of Double)
        Dim _y As New List(Of Double)

        Public ReadOnly Property coeff_a As Double
        Public ReadOnly Property coeff_b As Double
        Public ReadOnly Property IsValid As Boolean

        Sub New()
        End Sub

        Sub addXY(ByVal x As Double, ByVal y As Double)
            _x.Add(x)
            _y.Add(y)
        End Sub

        Function CalcAB() As Boolean
            _IsValid = False
            If _x.Count < 2 Then
                Return False
            End If

            Try
                'xの平均
                Dim sum_x As Double = 0
                For Each x As Double In _x
                    sum_x += x
                Next
                Dim average_x As Double = sum_x / _x.Count

                'yの平均
                Dim sum_y As Double = 0
                For Each y As Double In _y
                    sum_y += y
                Next
                Dim average_y As Double = sum_y / _y.Count

                '共分散(x-average_x)*(y-average_y)
                Dim var_xy As Double = 0
                '分散(x-average_x)^2
                Dim var_x2 As Double = 0
                For i As Integer = 0 To _x.Count - 1
                    var_xy += (_x(i) - average_x) * (_y(i) - average_y)
                    var_x2 += (_x(i) - average_x) ^ 2
                Next
                '
                _coeff_a = var_xy / var_x2
                _coeff_b = average_y - _coeff_a * average_x
                _IsValid = True
                Return True

            Catch ex As Exception
                g_clsLog.LogException(ex, "clsLeastSquare.CalcAB")
                Return False
            End Try
        End Function
    End Class



End Class
