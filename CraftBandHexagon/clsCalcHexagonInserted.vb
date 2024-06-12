Imports System.Reflection
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsInsertExpand
Imports CraftBand.Tables.dstDataTables
Imports CraftBandHexagon.clsCalcHexagon.CHex


Partial Public Class clsCalcHexagon

    'このファイル内だけで参照します
    'Dim _ImageList差しひも As New clsImageItemList
    'Dim _BandListForClip差しひも As New CBandList

    'enum角度 0,i_30度h,i_60度h,i_90度h,i_120度h,i_150度h
    Dim ANGLE角度() As Integer = {0, 30, 60, 90, 120, 150}


    '配置面             バンドに平行方向                    バンドに直角方向                                   
    '                0度       　   60度,120度　　　　　 90度       30度,150度
    '--------+------------------+-----------------+-----------------+-----------------+
    '底面(A) |          横ひも数/斜めひも数+1     |    (*)その方向の目の数+1       　 |
    '        | _hex底の辺・ひもと同位置・目幅     |  _hex底の辺・位置計算・対角幅     |
    '--------+------------------+-----------------+-----------------+-----------------+
    '全面(C) |          横ひも数/斜めひも数+1     |    (*)その方向の目の数+1       　 |
    '        |_ hex側面上辺・ひもと同位置・目幅   |  _hex底の辺・位置計算・対角幅     |
    '--------+------------------+-----------------+-----------------+-----------------+
    '側面(B) |側面の編みひも数  |          垂直ひも数(ひも中心合わせは-6)             |
    '        |    1はひも上     |                     固定長                          |
    '        |展開なければ固定長|                    斜めに6側面                      |
    '--------+------------------+-----------------+-----------------+-----------------+
    '                                                    (*)ひも幅変更はNG                      

#Region "サブ関数"
    '仮の関数
    Private Const No_Double_Value As Double = Double.MaxValue



    '1から開始の点数、開始位置と何本置き指定の該当数
    Private Function get該当数(ByVal i開始位置 As Integer, ByVal i点数 As Integer, ByVal i何本ごと As Integer) As Integer
        If i開始位置 < 1 Then
            Return 0
        End If
        If i点数 < i開始位置 Then
            Return 0
        End If
        If i何本ごと = 0 Then
            Return 1 '1点のみ
        End If
        Return 1 + (i点数 - i開始位置) \ i何本ごと
    End Function

    '次の面に続く場合の開始位置
    Private Function get次の開始位置(ByVal i開始位置 As Integer, ByVal i点数 As Integer, ByVal i何本ごと As Integer) As Integer
        If i開始位置 < 1 Then
            Return -1 '次は無い
        End If
        If i点数 < i開始位置 Then
            Return i開始位置 - i点数
        End If
        If i何本ごと = 0 Then
            Return -1 '次は無い
        End If
        Return i何本ごと - ((i点数 - i開始位置) Mod i何本ごと)
    End Function

#End Region


#Region "有効チェックとひも数(常時)"

    '差しひもが有効か？　無効の時は、f_s無効理由をセットしFalseを返す
    '※各レコードや設定が変わるたび呼び出し
    Private Function is差しひもavairable(ByVal row As tbl差しひもRow) As Boolean
        If row Is Nothing Then
            Return False
        End If

        Select Case row.f_i配置面
            '-------------------------------------------------
            Case enum配置面.i_底面, enum配置面.i_全面 'A,C
                Select Case row.f_i角度
                    Case enum角度Hex.i_0度H, enum角度Hex.i_60度H, enum角度Hex.i_120度H
                        If _d六つ目の高さ < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                            row.f_s無効理由 = text何本幅()
                            Return False
                        End If

                    Case enum角度Hex.i_30度H, enum角度Hex.i_90度H, enum角度Hex.i_150度H
                        If p_bひも本幅変更() Then '側面以外
                            row.f_s無効理由 = textひも本幅変更()
                            Return False
                        End If
                        If p_d六つ目の対角線 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                            row.f_s無効理由 = text対角線()
                            Return False
                        End If

                    Case Else
                        row.f_s無効理由 = text角度()
                        Return False
                End Select
            '-------------------------------------------------
            Case enum配置面.i_側面 'B
                If _i側面の編みひも数 < 1 Then
                    row.f_s無効理由 = text側面の編みひも()
                    Return False
                End If

                Select Case row.f_i角度
                    Case enum角度Hex.i_0度H, enum角度Hex.i_60度H, enum角度Hex.i_120度H
                        If _d六つ目の高さ < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                            row.f_s無効理由 = text何本幅()
                            Return False
                        End If

                    Case enum角度Hex.i_30度H, enum角度Hex.i_90度H, enum角度Hex.i_150度H
                        If p_d六つ目の対角線 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                            row.f_s無効理由 = text対角線()
                            Return False
                        End If

                    Case Else
                        row.f_s無効理由 = text角度()
                        Return False
                End Select

                '-------------------------------------------------
            Case Else 'enum配置面.i_なし/以外
                row.f_s無効理由 = text配置面()
                Return False

        End Select

        Return True
    End Function

    '差しひもの全数を返す。無効はチェック済とする。固定長ならf_dひも長にセットする。
    '※各レコードや設定が変わるたび呼び出し(is差しひもavairableであれば)
    Private Function get差しひもCount(ByVal row As tbl差しひもRow) As Integer
        Dim count As Integer = -1

        Select Case row.f_i配置面
                '-------------------------------------------------
            Case enum配置面.i_底面, enum配置面.i_全面 'A,C
                Dim is全面 As Boolean = (row.f_i配置面 = CType(enum配置面.i_全面, Integer))
                Select Case row.f_i角度
                    Case enum角度Hex.i_0度H
                        count = count底を平行に差す(row, cIdxAngle0, is全面)
                    Case enum角度Hex.i_60度H
                        count = count底を平行に差す(row, cIdxAngle60, is全面)
                    Case enum角度Hex.i_120度H
                        count = count底を平行に差す(row, cIdxAngle120, is全面)

                    Case enum角度Hex.i_90度H  '0度に直角
                        count = count底を直角に差す(row, cIdxAngle0, is全面)
                    Case enum角度Hex.i_150度H  '60度に直角
                        count = count底を直角に差す(row, cIdxAngle60, is全面)
                    Case enum角度Hex.i_30度H '120度に直角
                        count = count底を直角に差す(row, cIdxAngle120, is全面)

                    Case Else
                        Return -1
                End Select
                '-------------------------------------------------
            Case enum配置面.i_側面 'B
                Select Case row.f_i角度
                    Case enum角度Hex.i_0度H
                        count = count側面を水平に(row)

                    Case enum角度Hex.i_60度H, enum角度Hex.i_120度H, enum角度Hex.i_90度H, enum角度Hex.i_30度H, enum角度Hex.i_150度H
                        Dim angle As Integer = ANGLE角度(row.f_i角度)
                        count = count側面を斜めに(row, angle)

                    Case Else
                        Return -1
                End Select
                '-------------------------------------------------

            Case Else
                Return -1
        End Select

        Return count
    End Function

#End Region

#Region "ひも長とイメージ(リスト処理以降)"

    '呼び出し関数    バンドに平行方向               バンドに直角方向                                   
    '              0度     　   60度,120度　　 　 90度       30度,150度
    '---------+-----------------+-----------+---------------+-------------+
    '底面(A)  |      ～底を平行に差す       |     ～底を直角に差す        |
    '全面(C)  |        (aidx,is全面)        |      (aidx,is全面)          |
    '---------+-----------------+-----------+---------------+-------------+
    '側面(B)  | ～側面を水平に  |         ～側面を斜めに                  |
    '         |                 |           (angle_side)                       |
    '---------+-----------------+-----------+---------------+-------------+

    'count～ 以降は、必要に応じて calc_追加計算() を呼び出す
    'length～ で、固定長/個々長を問わず、_clsInsertExpandに登録してCInsertItemListを返す
    'image～ は、_clsInsertExpandに登録がある前提で処理する


    '差しひもの各長をセットしたテーブルを返す。(固定長の場合も1レコードで返す)
    '※リスト出力時に呼び出し,呼び出し先で記号をセット
    Private Function get差しひもLength(ByVal row As tbl差しひもRow) As CInsertItemList
        Select Case row.f_i配置面
                '-------------------------------------------------
            Case enum配置面.i_底面, enum配置面.i_全面 'A,C
                Dim is全面 As Boolean = (row.f_i配置面 = CType(enum配置面.i_全面, Integer))
                Select Case row.f_i角度
                    Case enum角度Hex.i_0度H
                        Return length底を平行に差す(row, cIdxAngle0, is全面)
                    Case enum角度Hex.i_60度H
                        Return length底を平行に差す(row, cIdxAngle60, is全面)
                    Case enum角度Hex.i_120度H
                        Return length底を平行に差す(row, cIdxAngle120, is全面)

                    Case enum角度Hex.i_90度H  '0度に直角
                        Return length底を直角に差す(row, cIdxAngle0, is全面)
                    Case enum角度Hex.i_150度H  '60度に直角
                        Return length底を直角に差す(row, cIdxAngle60, is全面)
                    Case enum角度Hex.i_30度H '120度に直角
                        Return length底を直角に差す(row, cIdxAngle120, is全面)

                    Case Else

                End Select
                '-------------------------------------------------
            Case enum配置面.i_側面 'B
                Select Case row.f_i角度
                    Case enum角度Hex.i_0度H
                        Return length側面を水平に(row)

                    Case enum角度Hex.i_60度H, enum角度Hex.i_120度H, enum角度Hex.i_90度H, enum角度Hex.i_30度H, enum角度Hex.i_150度H
                        Dim angle As Integer = ANGLE角度(row.f_i角度)
                        Return length側面を斜めに(row, angle)

                    Case Else

                End Select


            Case Else

        End Select

        Return Nothing
    End Function

    '_ImageList差しひもにアイテム生成
    'プレビュー時に呼び出し(プレビュー処理内でリスト出力後)
    Private Function imageList差しひも(ByVal bandListForClip差しひも As CBandList) As clsImageItemList
        If 0 = _Data.p_tbl差しひも.Rows.Count Then
            Return Nothing
        End If
        Dim imgList差しひも As New clsImageItemList

        For Each row As tbl差しひもRow In _Data.p_tbl差しひも.Select(Nothing, "f_i番号")
            If Not row.f_b有効区分 Then
                Continue For
            End If

            'ひも/目内の位置
            Dim dInnerPosition As Double = 0.5
            If Not row.Isf_i同位置数Null AndAlso Not row.Isf_i同位置順Null AndAlso
                1 < row.f_i同位置数 AndAlso 0 < row.f_i同位置順 Then
                dInnerPosition = (2 * row.f_i同位置順 - 1) / (2 * row.f_i同位置数)
            End If

            Dim bandlist As CBandList = Nothing

            Select Case row.f_i配置面
                '-------------------------------------------------
                Case enum配置面.i_底面, enum配置面.i_全面 'A,C
                    Dim is全面 As Boolean = (row.f_i配置面 = CType(enum配置面.i_全面, Integer))
                    Select Case row.f_i角度
                        Case enum角度Hex.i_0度H
                            bandlist = image底を平行に差す(row, cIdxAngle0, is全面, dInnerPosition)
                        Case enum角度Hex.i_60度H
                            bandlist = image底を平行に差す(row, cIdxAngle60, is全面, dInnerPosition)
                        Case enum角度Hex.i_120度H
                            bandlist = image底を平行に差す(row, cIdxAngle120, is全面, dInnerPosition)

                        Case enum角度Hex.i_90度H  '0度に直角
                            bandlist = image底を直角に差す(row, cIdxAngle0, is全面, dInnerPosition)
                        Case enum角度Hex.i_150度H  '60度に直角
                            bandlist = image底を直角に差す(row, cIdxAngle60, is全面, dInnerPosition)
                        Case enum角度Hex.i_30度H '120度に直角
                            bandlist = image底を直角に差す(row, cIdxAngle120, is全面, dInnerPosition)

                        Case Else
                            '対象外
                    End Select
                '-------------------------------------------------
                Case enum配置面.i_側面 'B
                    Select Case row.f_i角度
                        Case enum角度Hex.i_0度H
                            bandlist = image側面を水平に(row, dInnerPosition)

                        Case enum角度Hex.i_60度H, enum角度Hex.i_120度H, enum角度Hex.i_90度H, enum角度Hex.i_30度H, enum角度Hex.i_150度H
                            Dim angle As Integer = ANGLE角度(row.f_i角度)
                            bandlist = image側面を斜めに(row, angle, dInnerPosition)

                        Case Else
                            '対象外
                    End Select

            End Select

            If bandlist IsNot Nothing AndAlso 0 < bandlist.Count Then
                Dim item As New clsImageItem(bandlist, IdxBandDrawInsertFront, row.f_i番号)
                If CType(row.f_i差し位置, enum差し位置) = enum差し位置.i_うら Then
                    item.AddClip(bandListForClip差しひも)
                    item.m_Index = IdxBandDrawInsertBack
                End If
                imgList差しひも.AddItem(item)
            End If
        Next

        Return imgList差しひも
    End Function

#End Region

#Region "底ひもの平行方向"
    '底の各方向への差しひも　

    Private Function count底を平行に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean) As Integer
        Return _BandPositions(aidx).InsertCount
    End Function


    Private Function length底を平行に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean) As CInsertItemList
        Dim insertItemList As CInsertItemList = _clsInsertExpand.GetList(row.f_i番号)
        If insertItemList IsNot Nothing Then
            Return insertItemList
        End If

        insertItemList = New CInsertItemList(row)
        _clsInsertExpand.Add(row.f_i番号, insertItemList)


        Return insertItemList
    End Function



    ' dInnerPosition:同幅内位置(0～1値・中央が0.5)　
    Private Function image底を平行に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean, ByVal dInnerPosition As Double) As CBandList
        Dim insertItemList As CInsertItemList = _clsInsertExpand.GetList(row.f_i番号)
        If insertItemList Is Nothing Then
            Return Nothing
        End If
        Dim bandlist As New CBandList

        Dim n開始位置 As Integer = row.f_i開始位置
        Dim i何本ごと As Integer = row.f_i何本ごと
        Dim i_番号 As Integer = row.f_i番号

        '本数
        Dim n本数 As Integer = _iひもの本数(aidx) + 1 '＋1位置まで
        If n本数 < n開始位置 Then
            Return Nothing
        End If

        Dim i本幅 As Integer = row.f_i何本幅
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)



        Dim dShiftBefore As Double = _d六つ目の高さ * (-1 + dInnerPosition)
        Dim dShiftAfter As Double = _d六つ目の高さ * dInnerPosition

        'バンド描画
        For idx As Integer = n開始位置 To n本数 Step i何本ごと
            Dim isLast As Boolean = (idx = (_iひもの本数(aidx) + 1))

            Dim pCenter As S実座標
            If isLast Then
                If Not _BandPositions(aidx).getひも端からシフト点(_iひもの本数(aidx), dShiftAfter, pCenter) Then
                    Continue For
                End If
            Else
                If Not _BandPositions(aidx).getひも端からシフト点(idx, dShiftBefore, pCenter) Then
                    Continue For
                End If
            End If

            '描画点
            Dim pA As S実座標
            Dim pB As S実座標

            '六角形との交点
            Dim cp As CHex.CCrossPoint
            Dim hex As CHex
            If is全面 Then
                hex = _hex側面上辺
            Else
                hex = _hex底の辺
            End If
            cp = hex.get辺との交点(pCenter, cBandAngleDegree(aidx))
            If cp Is Nothing Then
                If idx = 1 Then
                    '最初の前(軸方向最後の後・バンドと逆方向)
                    Dim line As S線分 = hex.line辺(CHex.hexidx(aidx, CHexLine.lineIdx.i_2nd))
                    Dim d距離 As Double = New S直線式(line).d距離(pCenter)
                    pA = line.p終了 + cDeltaAxisDirection(aidx) * d距離
                    pB = line.p開始 + cDeltaAxisDirection(aidx) * d距離
                ElseIf isLast Then
                    '最後の後(軸方向最初の前・バンドと同方向)
                    Dim line As S線分 = hex.line辺(CHex.hexidx(aidx, CHexLine.lineIdx.i_1st))
                    Dim d距離 As Double = New S直線式(line).d距離(pCenter)
                    pA = line.p開始 + cDeltaAxisDirection(aidx) * -d距離
                    pB = line.p終了 + cDeltaAxisDirection(aidx) * -d距離
                Else
                    'あるはず？
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No CrossPoint")
                    Continue For
                End If
            Else
                pA = cp.CrossLine.p開始
                pB = cp.CrossLine.p終了
            End If



            Dim band = New CBand(row)
            band._s記号 = "仮"

            'バンド描画位置
            band.p始点F = pA + cDeltaAxisDirection(aidx) * (-dひも幅 / 2)
            band.p終点F = pB + cDeltaAxisDirection(aidx) * (-dひも幅 / 2)
            band.p始点T = pA + cDeltaAxisDirection(aidx) * (dひも幅 / 2)
            band.p終点T = pB + cDeltaAxisDirection(aidx) * (dひも幅 / 2)

            '記号描画位置(現物合わせ)
            If IsDrawMarkCurrent Then
                If cBandAngleDegree(aidx) = 120 Then
                    band.p文字位置 = pB +
                  cDeltaBandDirection(aidx) * (dひも幅 / 2) +
                  cDeltaAxisDirection(aidx) * (dひも幅 / 2)
                ElseIf cBandAngleDegree(aidx) = 60 Then
                    band.p文字位置 = pA +
                    cDeltaBandDirection(aidx) * -dひも幅 +
                    cDeltaAxisDirection(aidx) * (dひも幅 / 3)
                ElseIf cBandAngleDegree(aidx) = 0 Then
                    band.p文字位置 = pA +
                    cDeltaBandDirection(aidx) * -dひも幅 +
                    cDeltaAxisDirection(aidx) * -(dひも幅 / 2)
                End If
            End If

            bandlist.Add(band)


            If i何本ごと = 0 Then
                Exit For
            End If
        Next


        Return bandlist
    End Function

#End Region

#Region "側面の水平方向"

    Private Function count側面を水平に(ByVal row As tbl差しひもRow) As Integer
        '編みひも情報
        If Not calc_追加計算(CalcStatus._position, CalcStatus._none, CalcStatus._none) Then
            Return -1
        End If
        Dim ratio As Double = _CSideBandList.AllSameRatio()
        If 0 < ratio Then
            row.f_dひも長 = get底の六角計の周() * ratio
        End If
        Return _CSideBandList.InsertCount
    End Function


    Private Function length側面を水平に(ByVal row As tbl差しひもRow) As CInsertItemList
        '編みひも情報
        If Not calc_追加計算(CalcStatus._position, CalcStatus._none, CalcStatus._none) Then
            Return Nothing
        End If
        Dim insertItemList As CInsertItemList = _clsInsertExpand.GetList(row.f_i番号)
        If insertItemList IsNot Nothing Then
            Return insertItemList
        End If

        insertItemList = New CInsertItemList(row)
        _clsInsertExpand.Add(row.f_i番号, insertItemList)



        Return insertItemList
    End Function

    Private Function image側面を水平に(ByVal row As tbl差しひもRow, ByVal dInnerPosition As Double) As CBandList
        '編みひも情報
        If Not calc_追加計算(CalcStatus._position, CalcStatus._none, CalcStatus._none) Then
            Return Nothing
        End If
        Dim insertItemList As CInsertItemList = _clsInsertExpand.GetList(row.f_i番号)
        If insertItemList Is Nothing Then
            Return Nothing
        End If
        Dim bandlist As New CBandList

        Dim i本幅 As Integer = row.f_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
        Dim i_番号 As Integer = row.f_i番号


        '本数
        Dim n本数 As Integer = _CSideBandList.InsertCount()
        For idx As Integer = row.f_i開始位置 To n本数 Step row.f_i何本ごと

            Dim dHeight As Double = _CSideBandList.GetHeight(idx, dInnerPosition)
            If dHeight = No_Double_Value Then
                Continue For
            End If
            Dim ratio As Double = _CSideBandList.GetRatio(idx)

            '6側面
            For hexidx As Integer = 0 To CHex.cHexCount - 1

                Dim band As CBand = New CBand(row)
                '厚さを加えた長さ、底位置からスタート
                Dim line As S線分 = _hex底の辺に厚さ.line辺(hexidx) +
                         CHex.delta辺の外向き法線(hexidx) * (-p_d底の厚さ + dHeight)

                band._s記号 = "仮"
                band.p始点F = line.p開始
                band.p終点F = line.p終了
                band.p始点T = line.p開始 + CHex.delta辺の外向き法線(hexidx) * d幅
                band.p終点T = line.p終了 + CHex.delta辺の外向き法線(hexidx) * d幅
                band.SetLengthRatio(ratio)
                If IsDrawMarkCurrent Then
                    '文字は上側面(コード固定)
                    If hexidx = 3 Then
                        band.p文字位置 = band.p始点F
                    End If
                End If
                band.is始点FT線 = False
                band.is終点FT線 = False

                bandlist.Add(band)
            Next

            If row.f_i何本ごと = 0 Then
                Exit For
            End If
        Next

        Return bandlist
    End Function

#End Region

#Region "側面を斜め方向"

    Private Function count側面を斜めに(ByVal row As tbl差しひもRow, ByVal angle As Integer) As Integer
        '側面の情報
        If Not calc_追加計算(CalcStatus._none, CalcStatus._none, CalcStatus._position) Then
            Return -1
        End If



        row.f_dひも長 = p_d六つ目ベース_高さ / New S差分(angle).dY 'Zero位置から上、縁は含まない
        Dim count = p_i垂直ひも数
        If _bひも中心合わせ Then
            count -= 6
        End If
        Return count
    End Function


    Private Function length側面を斜めに(ByVal row As tbl差しひもRow, ByVal angle As Integer) As CInsertItemList
        '側面の情報
        If Not calc_追加計算(CalcStatus._none, CalcStatus._none, CalcStatus._position) Then
            Return Nothing
        End If
        Dim insertItemList As CInsertItemList = _clsInsertExpand.GetList(row.f_i番号)
        If insertItemList IsNot Nothing Then
            Return insertItemList
        End If

        insertItemList = New CInsertItemList(row)
        _clsInsertExpand.Add(row.f_i番号, insertItemList)


        Return insertItemList
    End Function

    Private Function image側面を斜めに(ByVal row As tbl差しひもRow, ByVal angle As Integer, ByVal dInnerPosition As Double) As CBandList
        '側面の情報
        If Not calc_追加計算(CalcStatus._none, CalcStatus._none, CalcStatus._position) Then
            Return Nothing
        End If

        Dim bandlist As New CBandList

        '上側面から右周り
        Dim n開始位置 As Integer = row.f_i開始位置
        Dim i何本ごと As Integer = row.f_i何本ごと
        For Each hxidx As Integer In {3, 2, 1, 0, 5, 4}
            n開始位置 = image側面を斜めに(bandlist, hxidx, row, angle, dInnerPosition, n開始位置, row.f_i何本ごと)
        Next

        Return bandlist
    End Function

    '側面を斜めに
    Private Function image側面を斜めに(ByVal bandlist As CBandList, ByVal hexidx As Integer, ByVal row As tbl差しひもRow, ByVal angle As Integer, ByVal dInnerPosition As Double, ByVal n開始位置 As Integer, ByVal i何本ごと As Integer) As Integer
        '側面の情報

        If n開始位置 < 1 Then
            Return -1
        End If

        Dim splate As CSidePlate = _SidePlate(hexidx)

        Dim n本数 As Integer = splate.getCount()
        If n本数 < n開始位置 Then
            Return n開始位置 - n本数
        End If

        Dim angle_side As Integer = CHex.Angle辺(hexidx) + angle + 180
        Dim dRatio As Double = New S差分(angle).dY '長さ/高さ
        Dim dHeight As Double = get側面高さ() / dRatio


        Dim i本幅 As Integer = row.f_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)

        Dim isFirst As Boolean = True
        For idx As Integer = n開始位置 To n本数 Step i何本ごと

            Dim pCenter As S実座標 = splate.getCenter(idx, dInnerPosition)
            If pCenter.IsZero Then
                If i何本ごと = 0 Then
                    Exit For
                End If
                Continue For
            End If

            Dim band As CBand = New CBand(row)

            Dim line As New S線分(pCenter, pCenter + New S差分(angle_side) * dHeight)

            band._s記号 = "仮"
            band.p始点F = line.p開始
            band.p終点F = line.p終了
            band.p始点T = line.p開始 + New S差分(angle_side + 90) * d幅
            band.p終点T = line.p終了 + New S差分(angle_side + 90) * d幅
            If IsDrawMarkCurrent Then
                band.p文字位置 = band.p始点F
            End If

            bandlist.Add(band)

            If i何本ごと = 0 Then
                Exit For
            End If
        Next

        Return get次の開始位置(n開始位置, n本数, i何本ごと)
    End Function

#End Region

#Region "底ひもの直角方向"

    'aidxの軸方向
    Private Function count底を直角に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean) As Integer
        '底ひも直角方向の情報
        If Not calc_追加計算(CalcStatus._none, CalcStatus._position, CalcStatus._none) Then
            Return -1
        End If
        Return _iひもの本数(aidx) + 1 'TODO:
    End Function


    Private Function length底を直角に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean) As CInsertItemList
        '底ひも直角方向の情報
        If Not calc_追加計算(CalcStatus._none, CalcStatus._position, CalcStatus._none) Then
            Return Nothing
        End If
        Dim insertItemList As CInsertItemList = _clsInsertExpand.GetList(row.f_i番号)
        If insertItemList IsNot Nothing Then
            Return insertItemList
        End If

        insertItemList = New CInsertItemList(row)
        _clsInsertExpand.Add(row.f_i番号, insertItemList)


        Return insertItemList
    End Function

    'enum角度.i_90度h=cIdxAngle0 enum角度.i_150度h=cIdxAngle60 enum角度.i_30度h=cIdxAngle120
    '斜めの差しひものイメージ dInnerPosition:同幅内位置(0～1値・中央が0.5)
    Private Function image底を直角に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean, ByVal dInnerPosition As Double) As CBandList
        If Not calc_追加計算(CalcStatus._none, CalcStatus._position, CalcStatus._none) Then
            Return Nothing
        End If
        Dim insertItemList As CInsertItemList = _clsInsertExpand.GetList(row.f_i番号)
        If insertItemList Is Nothing Then
            Return Nothing
        End If
        Dim bandlist As New CBandList

        Dim d対角線幅 As Double = _d六つ目の高さ / SIN60
        Dim dひも幅 As Double = _d基本のひも幅 / SIN60
        Dim unit As Double = (d対角線幅 + dひも幅) / 2


        Dim i本幅 As Integer = row.f_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)

        Dim angle As Integer = ANGLE角度(row.f_i角度)
        Dim fn As New S直線式(angle + 90, pOrigin)

        Dim p1 As S実座標 = _hex最外六角形.get最外の点(angle + 90)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}:{1}", angle, p1.ToString)
        p1 = fn.p直交点(p1)


        Dim p2 As S実座標 = _hex最外六角形.get最外の点(angle - 90)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}:{1}", angle, p2.ToString)
        p2 = fn.p直交点(p2)


        Dim div As Double = New S線分(p1, p2).Length / unit

        Dim count As Integer = div + 6

        Dim shift As New S差分(angle + 90)
        shift *= -unit

        Dim delta As S差分
        delta = delta * (dInnerPosition - 0.5) * d対角線幅


        For i As Integer = -3 To count + 3
            'pは六角形の内側とは限らない
            Dim p As S実座標 = p1 + shift * i

            Dim cp As CCrossPoint = _hex底の辺.get辺との交点(p, angle)
            If cp Is Nothing OrElse Not cp.IsExist Then
                Continue For
            End If

            If is全面 Then
                cp = _hex側面上辺.get辺との交点(p, angle)
                If cp Is Nothing OrElse Not cp.IsExist Then
                    Continue For
                End If
            End If

            Dim band As CBand = New CBand(row)

            Dim line As S線分 = cp.CrossLine
            band._s記号 = "仮"
            band.p始点F = line.p開始 + New S差分(angle + 90) * -(d幅 / 2)
            band.p終点F = line.p終了 + New S差分(angle + 90) * -(d幅 / 2)
            band.p始点T = line.p開始 + New S差分(angle + 90) * (d幅 / 2)
            band.p終点T = line.p終了 + New S差分(angle + 90) * (d幅 / 2)
            If IsDrawMarkCurrent Then
                band.p文字位置 = band.p始点F
            End If

            bandlist.Add(band)

        Next

        Return bandlist
    End Function
#End Region

End Class
