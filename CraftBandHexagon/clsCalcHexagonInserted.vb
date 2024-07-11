Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsImageItem.CBand
Imports CraftBand.clsInsertExpand
Imports CraftBand.Tables.dstDataTables


Partial Public Class clsCalcHexagon
    '差しひも保存値
    Dim _InsertExpand As clsInsertExpand



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
    '                                                         (*)ひも幅変更はNG                      

#Region "サブ関数"

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
                        '固定長の場合は中でrowにセットする
                        count = count側面を水平に(row)

                    Case enum角度Hex.i_60度H, enum角度Hex.i_120度H,
                         enum角度Hex.i_30度H, enum角度Hex.i_90度H, enum角度Hex.i_150度H
                        '固定長、中でrowにセット
                        Dim angleSideBand As Integer = CSidePlate.SideAngleSelected(row.f_i角度)
                        count = count側面を斜めに(row, angleSideBand)

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
    'length～ で、固定長/個々長を問わず、_InsertExpandに登録してCInsertItemListを返す
    'image～ は、_clsInsertExpandに登録がある前提で処理する

    '<固定長>の場合
    '　長さはcount時にセット、画面に表示される(→位置に応じたひも数がセットされる)
    '  length時は、呼び出されない
    '  image時に個々の位置を処理(I/FにGetOneItemで作ったラッパーを使用)
    '長さが異なる複数の差しひもの場合
    '　count時の長さはNothingにセット
    '  length時に展開してCInsertItemListを返し、記号取得する
    '  image時に先のCInsertItemListを元に処理


    '差しひもの各長をセットしたテーブルを返す。(固定長の場合は呼び出されない)
    '※リスト出力時に呼び出し,呼び出し先で記号をセット
    Private Function get差しひもLength(ByVal row As tbl差しひもRow) As CInsertItemList

        'ひも/目内の位置
        Dim dInnerPosition As Double = 0.5
        If Not row.Isf_i同位置数Null AndAlso Not row.Isf_i同位置順Null AndAlso
                1 < row.f_i同位置数 AndAlso 0 < row.f_i同位置順 Then
            dInnerPosition = (2 * row.f_i同位置順 - 1) / (2 * row.f_i同位置数)
        End If

        Select Case row.f_i配置面
                '-------------------------------------------------
            Case enum配置面.i_底面, enum配置面.i_全面 'A,C
                Dim is全面 As Boolean = (row.f_i配置面 = CType(enum配置面.i_全面, Integer))
                Select Case row.f_i角度
                    Case enum角度Hex.i_0度H
                        Return length底を平行に差す(row, cIdxAngle0, is全面, dInnerPosition)
                    Case enum角度Hex.i_60度H
                        Return length底を平行に差す(row, cIdxAngle60, is全面, dInnerPosition)
                    Case enum角度Hex.i_120度H
                        Return length底を平行に差す(row, cIdxAngle120, is全面, dInnerPosition)

                    Case enum角度Hex.i_90度H  '0度に直角
                        Return length底を直角に差す(row, cIdxAngle0, is全面, dInnerPosition)
                    Case enum角度Hex.i_150度H  '60度に直角
                        Return length底を直角に差す(row, cIdxAngle60, is全面, dInnerPosition)
                    Case enum角度Hex.i_30度H '120度に直角
                        Return length底を直角に差す(row, cIdxAngle120, is全面, dInnerPosition)

                    Case Else

                End Select
                '-------------------------------------------------
            Case enum配置面.i_側面 'B
                Select Case row.f_i角度
                    Case enum角度Hex.i_0度H
                        '固定長の場合は呼び出されない
                        Return length側面を水平に(row, dInnerPosition)

                    Case enum角度Hex.i_60度H, enum角度Hex.i_120度H, enum角度Hex.i_90度H, enum角度Hex.i_30度H, enum角度Hex.i_150度H
                        '固定長なので呼び出されないはずだが、念のため。。
                        Dim angleSideBand As Integer = CSidePlate.SideAngleSelected(row.f_i角度)
                        Return length側面を斜めに(row, angleSideBand)

                    Case Else

                End Select
                '-------------------------------------------------
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
                            '固定長/可変長とも
                            bandlist = image側面を水平に(row, dInnerPosition)

                        Case enum角度Hex.i_60度H, enum角度Hex.i_120度H, enum角度Hex.i_90度H, enum角度Hex.i_30度H, enum角度Hex.i_150度H
                            Dim angleSideBand As Integer = CSidePlate.SideAngleSelected(row.f_i角度)
                            bandlist = image側面を斜めに(row, angleSideBand, dInnerPosition)

                        Case Else
                            '対象外
                    End Select

            End Select

            If bandlist IsNot Nothing AndAlso 0 < bandlist.Count Then
                Dim item As New clsImageItem(bandlist, IdxBandDrawInsert, row.f_i番号)
                If CType(row.f_i差し位置, enum差し位置) = enum差し位置.i_うら Then
                    item.AddClip(bandListForClip差しひも)
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
        Return _BandPositions(aidx).InsertCount(is全面)
    End Function


    Private Function length底を平行に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean, ByVal dInnerPosition As Double) As CInsertItemList
        Dim insertItemList As CInsertItemList = _InsertExpand.GetList(row.f_i番号)
        If insertItemList IsNot Nothing Then
            Return insertItemList
        End If

        insertItemList = New CInsertItemList(row)
        _InsertExpand.Add(row.f_i番号, insertItemList)

        '本数
        Dim n本数 As Integer = _BandPositions(aidx).InsertCount(is全面)
        If n本数 < row.f_i開始位置 Then
            Return insertItemList '空リスト
        End If

        '六角形との交点から長さ取得
        For idx As Integer = row.f_i開始位置 To n本数 Step row.f_i何本ごと

            Dim pPoint As S実座標
            If Not _BandPositions(aidx).getInsertBandPoint(idx, is全面, dInnerPosition, pPoint) Then
                Continue For
            End If

            Dim insertItem As New CInsertItem(insertItemList)
            insertItem.m_idx = idx
            insertItem.m_iひも番号 = idx
            insertItem.m_pCenter = pPoint

            Dim cp As CHex.CCrossPoint
            Dim hex As CHex
            If is全面 Then
                hex = _hex側面上辺
            Else
                hex = _hex底の辺
            End If
            cp = hex.get辺との交点(pPoint, cBandAngleDegree(aidx))
            If cp Is Nothing Then
                If idx = 1 Then '_b端の目あり
                    '最初の前(軸方向最後の後・バンドと逆方向)
                    Dim line As S線分 = hex.line辺(CHex.hexidx(aidx, CHexLine.lineIdx.i_2nd))
                    insertItem.m_dひも長 = line.Length
                    Dim d距離 As Double = New S直線式(line).d距離(pPoint)
                    insertItem.m_line = line + cDeltaAxisDirection(aidx) * d距離
                ElseIf idx = n本数 Then '_b端の目あり
                    '最後の後(軸方向最初の前・バンドと同方向)
                    Dim line As S線分 = hex.line辺(CHex.hexidx(aidx, CHexLine.lineIdx.i_1st))
                    insertItem.m_dひも長 = line.Length
                    Dim d距離 As Double = New S直線式(line).d距離(pPoint)
                    insertItem.m_line = line + cDeltaAxisDirection(aidx) * -d距離
                Else
                    'あるはず？
                    g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No CrossPoint")
                    Continue For
                End If
            Else
                insertItem.m_dひも長 = cp.CrossLine.Length
                insertItem.m_line = cp.CrossLine
            End If
            insertItemList.Add(insertItem)

            If row.f_i何本ごと = 0 Then
                Exit For
            End If
        Next

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "length側面を水平に({0},{1},{2})開始位置({3}) 記号({4})", row.f_i番号, CType(row.f_i配置面, enum配置面), CType(row.f_i角度, enum角度Hex), row.f_i開始位置, row.f_s記号)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}", insertItemList)
        Return insertItemList
    End Function



    ' dInnerPosition:同幅内位置(0～1値・中央が0.5)　
    Private Function image底を平行に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean, ByVal dInnerPosition As Double) As CBandList
        Dim insertItemList As CInsertItemList = _InsertExpand.GetList(row.f_i番号)
        If insertItemList Is Nothing Then
            Return Nothing
        End If
        Dim bandlist As New CBandList

        For Each insertItem As CInsertItem In insertItemList
            Dim band = New CBand(insertItem)
            Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
            band.SetBand(insertItem.m_line, dひも幅, cDeltaAxisDirection(aidx))

            '記号描画位置(現物合わせ)
            If IsDrawMarkCurrent Then
                Dim mark As enumMarkPosition = {enumMarkPosition._終点Fの後, enumMarkPosition._終点の後, enumMarkPosition._始点Fの前}(aidx)
                band.SetMarkPosition(mark, _d基本のひも幅)
            End If

            bandlist.Add(band)
        Next

        Return bandlist
    End Function

#End Region

#Region "側面の水平方向"

    '固定長の場合は中でrowにセットする
    Private Function count側面を水平に(ByVal row As tbl差しひもRow) As Integer
        '編みひも情報
        If Not calc_追加計算(CalcStatus._position, CalcStatus._none, CalcStatus._none) Then
            Return -1
        End If

        '全て同じ周長比率対底の周か
        Dim ratio As Double = _CSideBandList.AllSameRatio()
        If 0 < ratio Then
            '存在してかつ同じなら固定値
            row.f_dひも長 = get底の六角計の周() * ratio '画面表示用
        End If

        Return _CSideBandList.InsertCount
    End Function

    '呼び出されるのは可変長の場合のはず
    Private Function length側面を水平に(ByVal row As tbl差しひもRow, ByVal dInnerPosition As Double) As CInsertItemList
        '編みひも情報
        If Not calc_追加計算(CalcStatus._position, CalcStatus._none, CalcStatus._none) Then
            Return Nothing
        End If
        Dim insertItemList As CInsertItemList = _InsertExpand.GetList(row.f_i番号)
        If insertItemList IsNot Nothing Then
            Return insertItemList
        End If

        insertItemList = New CInsertItemList(row)
        _InsertExpand.Add(row.f_i番号, insertItemList)

        Dim n本数 As Integer = _CSideBandList.InsertCount()
        For idx As Integer = row.f_i開始位置 To n本数 Step row.f_i何本ごと
            Dim ratio As Double = _CSideBandList.GetRatio(idx, dInnerPosition)

            'ここでは、6側面合計の周長のみ計算
            Dim item As New CInsertItem(insertItemList)
            item.m_dひも長 = get底の六角計の周() * ratio
            item.m_iひも番号 = idx
            '
            item.m_idx = idx
            insertItemList.Add(item)

            If row.f_i何本ごと = 0 Then
                Exit For
            End If
        Next

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "length側面を水平に({0},{1},{2})開始位置({3}) 記号({4})", row.f_i番号, CType(row.f_i配置面, enum配置面), CType(row.f_i角度, enum角度Hex), row.f_i開始位置, row.f_s記号)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}", insertItemList)
        Return insertItemList
    End Function

    Private Function image側面を水平に(ByVal row As tbl差しひもRow, ByVal dInnerPosition As Double) As CBandList
        Dim insertItemList As CInsertItemList = _InsertExpand.GetList(row.f_i番号)
        If insertItemList Is Nothing Then
            '固定長の場合は作られず、記号は row(tbl差しひもRow)にある
        End If
        Dim bandlist As New CBandList

        Dim i本幅 As Integer = row.f_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)

        '本数
        Dim n本数 As Integer = _CSideBandList.InsertCount()
        For idx As Integer = row.f_i開始位置 To n本数 Step row.f_i何本ごと

            Dim dHeight As Double
            If Not _CSideBandList.GetHeight(idx, dInnerPosition, dHeight) Then
                Continue For
            End If
            Dim ratio As Double = _CSideBandList.AllSameRatio()

            '簡易処理。固定/可変・idxの一致・レコード数の一致チェックは省略
            Dim s記号 As String = Nothing 'insertItemListの記号
            If insertItemList IsNot Nothing Then
                Dim item As CInsertItem = insertItemList.FindItem(idx)
                If item IsNot Nothing Then
                    s記号 = item.m_s記号
                    ratio = _CSideBandList.GetRatio(idx, dInnerPosition)
                End If
            End If

            '6側面
            For hexidx As Integer = 0 To CHex.cHexCount - 1

                Dim band As CBand = New CBand(row) 'row内の記号
                If Not String.IsNullOrEmpty(s記号) Then
                    band._s記号 = s記号
                End If

                '厚さを加えた長さ、底位置からスタート
                Dim line As S線分 = _hex底の辺に厚さ.line辺(hexidx)
                line.Revert() '反転。辺の方向を、外向き法線-90にするため
                Dim delta As S差分 = CHex.delta辺の外向き法線(hexidx)
                line += delta * (-p_d底の厚さ) '底の辺に厚さ→底の辺

                'ゼロ位置からの高さを加える
                line += delta * (dHeight)
                band.SetBand(line, d幅, delta) '#71
                band.SetLengthRatio(ratio)
                If IsDrawMarkCurrent AndAlso hexidx = 0 Then 'コード固定・下側面
                    Dim mark As enumMarkPosition = enumMarkPosition._終点の後
                    band.SetMarkPosition(mark, _d基本のひも幅)
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

    '固定長を返す
    Private Function count側面を斜めに(ByVal row As tbl差しひもRow, ByVal angleSideBand As Integer) As Integer
        If angleSideBand <= 0 OrElse 180 <= angleSideBand Then '0<Sin(angleSideBand)
            Return -1
        End If

        '固定長
        row.f_dひも長 = get側面高さ() / New S差分(angleSideBand).dY 'Zero位置から上、縁は含まない
        Dim count = p_i垂直ひも半数 '角1点
        If _bひも中心合わせ Then
            count += 6 '角2点
        End If
        Return count
    End Function

    '固定長なので呼び出されないはずだが。。
    Private Function length側面を斜めに(ByVal row As tbl差しひもRow, ByVal angleSideBand As Integer) As CInsertItemList
        'insertItemListも作りません
        Return Nothing
    End Function

    Private Function image側面を斜めに(ByVal row As tbl差しひもRow, ByVal angleSideBand As Integer, ByVal dInnerPosition As Double) As CBandList
        If angleSideBand <= 0 OrElse 180 <= angleSideBand Then '0<Sin(angleSideBand)
            Return Nothing
        End If
        '編みひもと側面の情報
        If Not calc_追加計算(CalcStatus._position, CalcStatus._none, CalcStatus._position) Then
            Return Nothing
        End If

        '側面最初の目の高さ
        Dim dHeightFirstHorizontal As Double
        If Not _CSideBandList.GetHeight(1, 0.5, dHeightFirstHorizontal) Then
            Return Nothing
        End If

        Dim bandlist As New CBandList

        Dim arg As New arg側面を斜めに
        arg.bandlist = bandlist
        arg.row = row
        arg.angleSideBand = angleSideBand
        arg.dInnerPosition = dInnerPosition
        arg.dひも幅 = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)
        arg.dひも長 = row.f_dひも長
        arg.dHeightFirstHorizontal = dHeightFirstHorizontal

        '上側面から右周り
        Dim n開始位置 As Integer = row.f_i開始位置
        Dim i何本ごと As Integer = row.f_i何本ごと
        For Each hxidx As Integer In CSidePlate.PlateOrderHexIndex
            n開始位置 = image側面を斜めに(arg, hxidx, n開始位置, row.f_i何本ごと)
        Next

        Return bandlist
    End Function

    Private Structure arg側面を斜めに
        Dim bandlist As CBandList
        Dim row As tbl差しひもRow
        Dim angleSideBand As Integer
        Dim dInnerPosition As Double

        Dim dHeightFirstHorizontal As Double
        Dim dひも長 As Double
        Dim dひも幅 As Double
    End Structure

    '側面を斜めに
    Private Function image側面を斜めに(ByVal arg As arg側面を斜めに, ByVal hexidx As Integer, ByVal n開始位置 As Integer, ByVal i何本ごと As Integer) As Integer
        If n開始位置 < 1 Then
            Return -1
        End If

        '側面の情報
        Dim splate As CSidePlate = _SidePlate(hexidx)

        Dim n本数 As Integer = splate.InsertCount()
        If n本数 < n開始位置 Then
            Return n開始位置 - n本数
        End If

        '水平の目のライン
        Dim line水平の目 As S線分 = _hex底の辺.line辺(hexidx) + CHex.delta辺の外向き法線(hexidx) * arg.dHeightFirstHorizontal
        Dim fn水平の目ライン As New S直線式(line水平の目) '逆向きだが式にすると同じ
        '底の辺
        Dim fn底の辺ライン As New S直線式(_hex底の辺.line辺(hexidx))
        '差しひもの角度
        Dim angle差しひも As Integer = (CHex.Angle辺(hexidx) - 180) + arg.angleSideBand


        For idx As Integer = n開始位置 To n本数 Step i何本ごと
            '六つ目内の中心点を通る目のライン
            Dim fn目のライン As S直線式
            If splate.getFnCenter(idx, arg.dInnerPosition, fn目のライン) Then

                Dim p回転中心 As S実座標 = fn水平の目ライン.p交点(fn目のライン)
                Dim fn差しひもライン As New S直線式(angle差しひも, p回転中心)
                Dim p底との交点 As S実座標 = fn差しひもライン.p交点(fn底の辺ライン)

                Dim band As CBand = New CBand(arg.row)
                Dim line As New S線分(p底との交点, p底との交点 + New S差分(angle差しひも) * arg.dひも長)
                band.SetBand(line, arg.dひも幅, New S差分(angle差しひも + 90))
                If IsDrawMarkCurrent AndAlso idx = 1 Then
                    band.SetMarkPosition(enumMarkPosition._終点の後, _d基本のひも幅)
                End If

                arg.bandlist.Add(band)
            End If

            If i何本ごと = 0 Then
                Exit For
            End If
        Next

        Return get次の開始位置(n開始位置, n本数, i何本ごと)
    End Function

#End Region

#Region "底ひもの直角方向"
    'aidxの軸方向に対応
    '90度Hex→cIdxAngle0  150度Hex→cIdxAngle60  30度Hex→cIdxAngle120

    Private Function count底を直角に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean) As Integer
        '底ひも直角方向の情報
        If Not calc_追加計算(CalcStatus._none, CalcStatus._position, CalcStatus._none) Then
            Return -1
        End If
        Return _BottomRightAngle(aidx).InsertCount(is全面)
    End Function


    Private Function length底を直角に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean, ByVal dInnerPosition As Double) As CInsertItemList
        '底ひも直角方向の情報
        If Not calc_追加計算(CalcStatus._none, CalcStatus._position, CalcStatus._none) Then
            Return Nothing
        End If
        Dim insertItemList As CInsertItemList = _InsertExpand.GetList(row.f_i番号)
        If insertItemList IsNot Nothing Then
            Return insertItemList
        End If

        insertItemList = New CInsertItemList(row)
        _InsertExpand.Add(row.f_i番号, insertItemList)

        '本数
        Dim n本数 As Integer = _BottomRightAngle(aidx).InsertCount(is全面)
        If n本数 < row.f_i開始位置 Then
            Return insertItemList '空リスト
        End If

        '基準点ライン方向
        Dim angle基準点ライン As Integer = _BottomRightAngle(aidx)._angle基準点ライン
        Dim angle差しひも方向 As Integer = _BottomRightAngle(aidx)._angle差しひも方向

        For idx As Integer = row.f_i開始位置 To n本数 Step row.f_i何本ごと
            '基準点取得。六角形の内側とは限りません
            Dim p As S実座標
            If Not _BottomRightAngle(aidx).GetPoint(idx, is全面, dInnerPosition, p) Then
                Continue For
            End If

            Dim hex As CHex
            If is全面 Then
                hex = _hex側面上辺
            Else
                hex = _hex底の辺
            End If
            Dim cp As CHex.CCrossPoint = hex.get辺との交点(p, angle差しひも方向)
            If cp Is Nothing OrElse Not cp.IsExist Then
                Continue For
            End If

            Dim insertItem As New CInsertItem(insertItemList)
            insertItem.m_idx = idx
            insertItem.m_iひも番号 = idx
            insertItem.m_dひも長 = cp.CrossLine.Length

            insertItem.m_pCenter = p
            insertItem.m_line = cp.CrossLine
            insertItemList.Add(insertItem)

            If row.f_i何本ごと = 0 Then
                Exit For
            End If
        Next

        Return insertItemList
    End Function

    '斜めの差しひものイメージ dInnerPosition:同幅内位置(0～1値・中央が0.5)
    Private Function image底を直角に差す(ByVal row As tbl差しひもRow, ByVal aidx As Integer, ByVal is全面 As Boolean, ByVal dInnerPosition As Double) As CBandList
        If Not calc_追加計算(CalcStatus._none, CalcStatus._position, CalcStatus._none) Then
            Return Nothing
        End If
        Dim insertItemList As CInsertItemList = _InsertExpand.GetList(row.f_i番号)
        If insertItemList Is Nothing Then
            Return Nothing
        End If
        Dim bandlist As New CBandList

        Dim delta幅方向 As New S差分(_BottomRightAngle(aidx)._angle基準点ライン)
        Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(row.f_i何本幅)


        For Each insertItem As CInsertItem In insertItemList
            Dim band = New CBand(insertItem)
            band.SetBand(insertItem.m_line, dひも幅, delta幅方向)

            '記号描画位置(現物合わせ)
            If IsDrawMarkCurrent Then
                '記号位置                        90度Hex                      150度Hex                         30度Hex
                Dim mark As enumMarkPosition = {enumMarkPosition._始点Fの前, enumMarkPosition._終点Fの後, enumMarkPosition._始点Fの前}(aidx)
                band.SetMarkPosition(mark, _d基本のひも幅 * 2)
            End If

            bandlist.Add(band)
        Next

        Return bandlist
    End Function
#End Region

End Class
