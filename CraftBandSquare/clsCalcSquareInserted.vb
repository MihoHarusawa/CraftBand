Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsInsertExpand
Imports CraftBand.Tables.dstDataTables
Imports CraftBandSquare.clsCalcSquare

Partial Public Class clsCalcSquare


    '配置面                           角度                           18度(e)108度(g)
    '               0度(a)            90度(c)      45度(b)135度(d)   72度(f)162度(h)  
    '--------+-----------------+-----------------+-----------------+-----------------+
    '底面(A) |    <固定長>     |    <固定長>     |  ※斜め各長     |　　　　　　　　 |
    '        |    底枠横のみ   |    底枠縦のみ   | ひも本幅変更不可|　　　対象外　　 |
    '--------+-----------------+-----------------+-----------------+-----------------+
    '側面(B) |    <固定長>     |    <固定長>     |   <固定長>      |   <固定長>      |
    '        |    水平4側面(*) |    縦横4側面    |  斜めに4側面    |  斜めに4側面    |
    '--------+-----------------+-----------------+-----------------+-----------------+
    '全面(C) |    <固定長>     |     長さ2種     |  ※斜め各長     |　　　　　　　　 |
    '        |    水平4側面(*) |    縦横の全長   | ひも本幅変更不可|　　　対象外　　 |
    '--------+-----------------+-----------------+-----------------+-----------------+
    '                                             ※斜め各長は、ひも本幅変更なしとして計算する                                          

#Region "数の認識"

    Dim DELTA72 As New S差分(1, 3) '右に1個・上に3個
    Dim DELTA18 As New S差分(3, 1) '右に3個・上に1個
    Dim DELTA108 As New S差分(-1, 3) '左に1個・上に3個
    Dim DELTA162 As New S差分(-3, 1) '左に3個・上に1個

    Private Const No_Double_Value As Double = Double.MaxValue

    '描画の位置
    Enum draw_position
        before '上・右の側面
        center '底面
        after '下・左の側面
    End Enum

    '目の認識
    Private Function is縦の上下に目あり() As Boolean
        Return g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d目と数える端の目") <= _d上端下端の目
    End Function

    '縦の目数として処理する数
    Private Function get縦の目の実質数() As Integer
        If is縦の上下に目あり() Then
            Return _i縦の目の数 + 2
        Else
            Return _i縦の目の数
        End If
    End Function

    '目の認識
    Private Function is横の左右に目あり() As Boolean
        Return g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d目と数える端の目") <= _d左端右端の目
    End Function

    '横の目数として処理する数
    Private Function get横の目の実質数() As Integer
        If is横の左右に目あり() Then
            Return _i横の目の数 + 2
        Else
            Return _i横の目の数
        End If
    End Function

    Private Function is最下段に目あり() As Boolean
        Return g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d目と数える端の目") <= _d最下段の目
    End Function

    '高さの目数として処理する数
    Private Function get高さの目の実質数() As Integer
        If is最下段に目あり() Then
            Return _i高さの目の数 + 1
        Else
            Return _i高さの目の数
        End If
    End Function

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
            Case enum配置面.i_底面 'A
                Select Case row.f_i角度
                    Case enum角度.i_0度, enum角度.i_90度  '底の横 底の縦 a,c
                        '目を通すなら
                        If (row.f_i中心点 = enum中心点.i_目の中央) AndAlso
                        _d目_ひも間のすき間 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                            row.f_s無効理由 = text何本幅()
                            Return False
                        End If

                    Case enum角度.i_45度, enum角度.i_135度 'b,d
                        If (_b横ひも本幅変更 OrElse _b縦ひも本幅変更 OrElse _b側面ひも本幅変更) Then
                            row.f_s無効理由 = textひも本幅変更()
                            Return False
                        ElseIf (row.f_i中心点 = enum中心点.i_目の中央) Then
                            '目の中央を通すので対角線分の空き
                            If _d目_ひも間のすき間 * ROOT2 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                                row.f_s無効理由 = text何本幅()
                                Return False
                            End If
                        End If

                    Case enum角度.i_18度, enum角度.i_72度, enum角度.i_108度, enum角度.i_162度  'e,f,g,h
                        row.f_s無効理由 = PlateString(enum配置面.i_側面)
                        Return False

                    Case Else
                        row.f_s無効理由 = text角度()
                        Return False
                End Select
            '-------------------------------------------------
            Case enum配置面.i_側面 'B
                If p_i側面ひもの本数 <= 1 Then
                    row.f_s無効理由 = text高さの目の数()
                    Return False
                End If

                Select Case row.f_i角度
                    Case enum角度.i_0度, enum角度.i_90度  '水平の周(全面と同じ)'a,c
                        '目を通すなら
                        If (row.f_i中心点 = enum中心点.i_目の中央) AndAlso
                        _d目_ひも間のすき間 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                            row.f_s無効理由 = text何本幅()
                            Return False
                        End If

                    Case enum角度.i_45度, enum角度.i_135度 'b,d
                        If (row.f_i中心点 = enum中心点.i_目の中央) Then
                            '目の中央を通すので対角線分の空き
                            If _d目_ひも間のすき間 * ROOT2 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                                row.f_s無効理由 = text何本幅()
                                Return False
                            End If
                        End If
                        'ひも位置をもとに配置するので本幅変更可能、ひも外の目に対する仮想ひもは無し

                    Case enum角度.i_72度, enum角度.i_108度  'f,g
                        If (row.f_i中心点 = enum中心点.i_目の中央) Then
                            If _d目_ひも間のすき間 * Math.Sqrt(10) / 3 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                                row.f_s無効理由 = text何本幅()
                                Return False
                            End If
                        End If

                    Case enum角度.i_18度, enum角度.i_162度  'e,,h
                        If (row.f_i中心点 = enum中心点.i_目の中央) Then
                            If _d目_ひも間のすき間 * Math.Sqrt(10) < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                                row.f_s無効理由 = text何本幅()
                                Return False
                            End If
                        End If

                    Case Else
                        row.f_s無効理由 = text角度()
                        Return False
                End Select
            '-------------------------------------------------
            Case enum配置面.i_全面 'C
                Select Case row.f_i角度
                    Case enum角度.i_0度, enum角度.i_90度  '水平の周(側面と同じ) 底の横+底の縦を側面に回す a,c
                        '目を通すなら
                        If (row.f_i中心点 = enum中心点.i_目の中央) AndAlso
                        _d目_ひも間のすき間 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                            row.f_s無効理由 = text何本幅()
                            Return False
                        End If

                    Case enum角度.i_45度, enum角度.i_135度 'b,d
                        If (_b横ひも本幅変更 OrElse _b縦ひも本幅変更 OrElse _b側面ひも本幅変更) Then
                            row.f_s無効理由 = textひも本幅変更()
                            Return False
                        ElseIf (row.f_i中心点 = enum中心点.i_目の中央) Then
                            '目の中央を通すので対角線分の空き
                            If _d目_ひも間のすき間 * ROOT2 < g_clsSelectBasics.p_d指定本幅(row.f_i何本幅) Then
                                row.f_s無効理由 = text何本幅()
                                Return False
                            End If
                        End If

                    Case enum角度.i_18度, enum角度.i_72度, enum角度.i_108度, enum角度.i_162度  'e,f,g,h
                        row.f_s無効理由 = PlateString(enum配置面.i_側面)
                        Return False

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
            Case enum配置面.i_底面 'A
                Select Case row.f_i角度
                    Case enum角度.i_0度  '底の横 a
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = get縦の目の実質数()
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i横ひもの本数 '_i縦の目の数+1
                        End If
                        row.f_dひも長 = p_d四角ベース_横  '底枠の中だけ

                    Case enum角度.i_90度  '底の縦 c
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = get横の目の実質数()
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i縦ひもの本数 '_i横の目の数+1
                        End If
                        row.f_dひも長 = p_d四角ベース_縦  '底枠の中だけ

                    Case enum角度.i_45度, enum角度.i_135度 'b,d
                        count = get底の斜めCount(row)

                    Case enum角度.i_18度, enum角度.i_72度, enum角度.i_108度, enum角度.i_162度  'e,f,g,h
                        '対象外
                        Return -1

                    Case Else
                        Return -1
                End Select
                '-------------------------------------------------
            Case enum配置面.i_側面 'B
                Select Case row.f_i角度
                    Case enum角度.i_0度  '水平の周(全面と同じ) a
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = get横の目の実質数()
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i側面ひもの本数
                        End If
                        row.f_dひも長 = get側面の周長()

                    Case enum角度.i_90度 'c
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = 2 * (get横の目の実質数() + get縦の目の実質数())
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i垂直ひも数
                        End If
                        row.f_dひも長 = p_d四角ベース_高さ 'Zero位置から上、縁は含まない

                    Case enum角度.i_45度, enum角度.i_135度 'b,d
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = 2 * (get横の目の実質数() + get縦の目の実質数())
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i垂直ひも数
                        End If
                        row.f_dひも長 = ROOT2 * p_d四角ベース_高さ 'Zero位置から上、縁は含まない

                    Case enum角度.i_72度, enum角度.i_108度  'f,g
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = 2 * (get横の目の実質数() + get縦の目の実質数())
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i垂直ひも数
                        End If
                        row.f_dひも長 = p_d四角ベース_高さ * (DELTA72.Length / DELTA72.dY)

                    Case enum角度.i_18度, enum角度.i_162度  'e,,h
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = 2 * (get横の目の実質数() + get縦の目の実質数())
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i垂直ひも数
                        End If
                        row.f_dひも長 = p_d四角ベース_高さ * (DELTA18.Length / DELTA18.dY)

                    Case Else
                        Return -1
                End Select
                '-------------------------------------------------
            Case enum配置面.i_全面 'C
                Select Case row.f_i角度
                    Case enum角度.i_0度  '水平の周(側面と同じ) a
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = get横の目の実質数()
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i側面ひもの本数
                        End If
                        row.f_dひも長 = get側面の周長()

                    Case enum角度.i_90度  '底の横+底の縦を側面に回す d
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            count = get横の目の実質数() + get縦の目の実質数()
                        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
                            count = p_i縦ひもの本数 + p_i横ひもの本数
                        End If
                        '縦と横で長さが異なる

                    Case enum角度.i_45度, enum角度.i_135度 'b,d
                        '底と同じ
                        count = get底の斜めCount(row)

                    Case enum角度.i_18度, enum角度.i_72度, enum角度.i_108度, enum角度.i_162度  'e,f,g,h
                        '対象外
                        Return -1

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

    '配置面                           角度                           18度(e)108度(g)
    '               0度(a)            90度(c)      45度(b)135度(d)   72度(f)162度(h)  
    '--------+-----------------+-----------------+-----------------+-----------------+
    '底面(A) |　　<固定長>　　 |　　<固定長>　　 |get底の斜めLength|　　　----  　　 |
    '--------+-----------------+-----------------+-----------------+-----------------+
    '側面(B) |　　<固定長>　　 |　　<固定長>　　 |　　<固定長>　　 |　　<固定長>　　 |
    '--------+-----------------+-----------------+-----------------+-----------------+
    '全面(C) |    <固定長>     |底の横+底の縦2種 |get底の斜めLength|　　　----　　　 |
    '--------+-----------------+-----------------+-----------------+-----------------+

    '差しひもの各長をセットしたテーブルを返す。(固定長ではないケースのみ)
    '※リスト出力時に呼び出し
    'Private Function get差しひもLength(ByVal row As tbl差しひもRow) As tbl縦横展開DataTable
    Private Function get差しひもLength(ByVal row As tbl差しひもRow) As CInsertItemList
        Select Case row.f_i配置面
                '-------------------------------------------------
            Case enum配置面.i_底面 'A
                Select Case row.f_i角度
                    Case enum角度.i_45度, enum角度.i_135度 'b,d
                        Return get底の斜めLength(row)

                    Case Else
                        Return Nothing '固定長
                End Select
                '-------------------------------------------------
            Case enum配置面.i_側面 'B
                Select Case row.f_i角度
                    Case Else
                        Return Nothing '固定長
                End Select
                '-------------------------------------------------
            Case enum配置面.i_全面 'C
                Select Case row.f_i角度
                    Case enum角度.i_90度  '底の横+底の縦を側面に回す c
                        Dim i横ひもの本数 As Integer = p_i横ひもの本数
                        Dim i縦ひもの本数 As Integer = p_i縦ひもの本数
                        If row.f_i中心点 = enum中心点.i_目の中央 Then
                            i横ひもの本数 = get横の目の実質数()
                            i縦ひもの本数 = get縦の目の実質数()
                        End If

                        Dim tmptable As CInsertItemList = New CInsertItemList(row)
                        Dim n開始位置 As Integer = row.f_i開始位置
                        '縦ひも
                        Dim count As Integer = get該当数(n開始位置, i縦ひもの本数, row.f_i何本ごと)
                        If 0 < count Then
                            Dim tmp As New CInsertItem(tmptable)
                            'key
                            tmp.m_iひも種 = enumひも種.i_縦
                            tmp.m_iひも番号 = 1
                            'set
                            tmp.m_iひも数 = count 'ひも数
                            tmp.m_i開始位置 = n開始位置
                            tmp.m_dひも長 = get周の縦() + get側面高(2) '縁は加えない
                            tmptable.Add(tmp)
                        End If
                        n開始位置 = get次の開始位置(n開始位置, i縦ひもの本数, row.f_i何本ごと)

                        '横ひも
                        count = get該当数(n開始位置, i横ひもの本数, row.f_i何本ごと)
                        If 0 < count Then
                            Dim tmp As New CInsertItem(tmptable)
                            'key
                            tmp.m_iひも種 = enumひも種.i_横
                            tmp.m_iひも番号 = 2
                            'set
                            tmp.m_iひも数 = count 'ひも数
                            tmp.m_i開始位置 = n開始位置
                            tmp.m_dひも長 = get周の横() + get側面高(2) '縁は加えない
                            tmptable.Add(tmp)
                        End If
                        If 0 < tmptable.Count Then
                            Return tmptable
                        Else
                            Return Nothing
                        End If

                    Case enum角度.i_45度, enum角度.i_135度 'b,d
                        Return get底の斜めLength(row)

                    Case Else
                        Return Nothing '固定長
                End Select
                '-------------------------------------------------
            Case Else
                Return Nothing '固定長
        End Select

    End Function


    '配置面                           角度                           18度(e)108度(g)
    '               0度(a)            90度(c)      45度(b)135度(d)   72度(f)162度(h)  
    '--------+-----------------+--------------------+--------------------+--------------+
    '底面(A) |image横ひもに差す| image縦ひもに差す  |[L]imageList底の斜め|　　----   　 |
    '--------+-----------------+--------------------+--------------------+--------------+
    '側面(B) |　　側面_水平 　 | image縦ひもに差す  |      image縦ひも面を斜めに　   　 |
    '        |　　          　 | image横ひもに差す  |      image横ひも面を斜めに　　    |
    '--------+-----------------+--------------------+--------------------+--------------+
    '全面(C) |    側面_水平    |[L]image横ひもに差す|[L]imageList底の斜め|　　----   　 |
    '        |　　          　 |[L]image縦ひもに差す|                    |      　　    |
    '--------+-----------------+--------------------+--------------------+--------------+
    '                                                [L]length結果参照
    '_ImageList差しひも生成
    '※プレビュー時に呼び出し(プレビュー処理内でリスト出力後)
    Private Function imageList差しひも() As Boolean
        _ImageList差しひも = New clsImageItemList
        If 0 = _Data.p_tbl差しひも.Rows.Count Then
            Return False
        End If

        For Each row As tbl差しひもRow In _Data.p_tbl差しひも.Select(Nothing, "f_i番号")
            If Not row.f_b有効区分 Then
                Continue For
            End If

            Dim n開始位置 As Integer = row.f_i開始位置
            Dim i何本ごと As Integer = row.f_i何本ごと
            Dim isCenterBand As Boolean = (row.f_i中心点 = enum中心点.i_ひも中央)
            'ひも/目内の位置
            Dim dInnerPosition As Double = 0.5
            If Not row.Isf_i同位置数Null AndAlso Not row.Isf_i同位置順Null AndAlso
                1 < row.f_i同位置数 AndAlso 0 < row.f_i同位置順 Then
                dInnerPosition = (2 * row.f_i同位置順 - 1) / (2 * row.f_i同位置数)
            End If

            Select Case row.f_i配置面
                '-------------------------------------------------
                Case enum配置面.i_底面 'A
                    Select Case row.f_i角度
                        Case enum角度.i_0度  '底の横 a 
                            image横ひもに差す(_InsertExpand.GetOneItem(row), isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.center)

                        Case enum角度.i_90度  '底の縦 c 
                            image縦ひもに差す(_InsertExpand.GetOneItem(row), isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.center)

                        Case enum角度.i_45度, enum角度.i_135度 'b,d
                            imageList底の斜め(row, dInnerPosition)

                        Case enum角度.i_18度, enum角度.i_72度, enum角度.i_108度, enum角度.i_162度  'e,f,g,h
                            '対象外
                    End Select
                '-------------------------------------------------
                Case enum配置面.i_側面 'B
                    Select Case row.f_i角度
                        Case enum角度.i_0度  '水平の周(全面と同じ) a
                            側面_水平(row, dInnerPosition)

                        Case enum角度.i_90度 'c
                            Dim item As CInsertItem = _InsertExpand.GetOneItem(row)
                            n開始位置 = image縦ひもに差す(item, isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.before) '1～縦の本数
                            n開始位置 = image横ひもに差す(item, isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.before) '縦の本数+1 ～横の本数+縦ひも本数
                            n開始位置 = image縦ひもに差す(item, isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.after) '横の本数+縦の本数+1 ～ 2*縦の本数+縦の本数
                            n開始位置 = image横ひもに差す(item, isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.after)  '2*縦の本数+縦の本数+1 ～ 2*横の本数+2*縦の本数

                        Case enum角度.i_45度, enum角度.i_135度, enum角度.i_18度, enum角度.i_72度, enum角度.i_108度, enum角度.i_162度 'b,d,e,f,g,h★
                            n開始位置 = image縦ひも面を斜めに(row, dInnerPosition, n開始位置, row.f_i何本ごと, draw_position.before)
                            n開始位置 = image横ひも面を斜めに(row, dInnerPosition, n開始位置, row.f_i何本ごと, draw_position.before)
                            n開始位置 = image縦ひも面を斜めに(row, dInnerPosition, n開始位置, row.f_i何本ごと, draw_position.after)
                            n開始位置 = image横ひも面を斜めに(row, dInnerPosition, n開始位置, row.f_i何本ごと, draw_position.after)

                    End Select
                '-------------------------------------------------
                Case enum配置面.i_全面 'C
                    Select Case row.f_i角度
                        Case enum角度.i_0度  '水平の周(側面と同じ) a
                            側面_水平(row, dInnerPosition)

                        Case enum角度.i_90度  '底の横+底の縦を側面に回す c
                            If Not _InsertExpand.ContainsKey(row.f_i番号) Then
                                Return False
                            End If
                            Dim tmptable As CInsertItemList = _InsertExpand.GetList(row.f_i番号)
                            For Each tmp As CInsertItem In tmptable
                                'n開始位置←tmp.m_i開始位置
                                If tmp.m_iひも種 = enumひも種.i_横 Then
                                    image横ひもに差す(tmp, isCenterBand, dInnerPosition, tmp.m_i開始位置, i何本ごと, draw_position.center) '1～横ひも本数
                                ElseIf tmp.m_iひも種 = enumひも種.i_縦 Then
                                    image縦ひもに差す(tmp, isCenterBand, dInnerPosition, tmp.m_i開始位置, i何本ごと, draw_position.center) '横の本数+1 ～ 横の本数+縦の本数
                                End If
                            Next

                        Case enum角度.i_45度, enum角度.i_135度 'b,d
                            imageList底の斜め(row, dInnerPosition)

                        Case enum角度.i_18度, enum角度.i_72度, enum角度.i_108度, enum角度.i_162度  'e,f,g,h
                            '対象外
                    End Select

            End Select
        Next

        Return True
    End Function
#End Region

#Region "底の横ひも"
    '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    ' dInnerPosition:同幅内位置(0～1値・中央が0.5)　
    Private Function get横ひもの中心Y(ByVal idx As Integer, ByVal isDesc As Boolean, ByVal dInnerPosition As Double) As Double
        Dim iYoko As Integer = idx
        Dim dip As Double = 1 - dInnerPosition
        If isDesc Then
            iYoko = p_i横ひもの本数 - idx + 1
            dip = dInnerPosition
        End If

        Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
        If itemYoko Is Nothing OrElse itemYoko.m_rひも位置.IsEmpty Then
            Return No_Double_Value
        End If
        Return (itemYoko.m_rひも位置.y最上 - itemYoko.m_rひも位置.y最下) * dip + itemYoko.m_rひも位置.y最下
    End Function

    Private Function get横ひもの目の中心Y(ByVal idx As Integer, ByVal isDesc As Boolean, ByVal dInnerPosition As Double) As Double
        If idx < 1 OrElse get縦の目の実質数() < idx Then
            Return No_Double_Value
        End If

        Dim iYoko As Integer
        Dim dip As Double = dInnerPosition
        Dim isUpper As Boolean = False
        If isDesc Then
            '下から上へ(逆方向)
            If is縦の上下に目あり() Then
                '1=最後の横ひもの下　～ 最初の横ひもの下,最初の横ひもの上
                If idx < get縦の目の実質数() Then
                    iYoko = p_i横ひもの本数 - idx + 1
                    dip = 1 - dInnerPosition
                Else
                    iYoko = 1
                    isUpper = True
                End If
            Else
                '1=最後の横ひもの上　～ 最初の横ひもの下(=最後-1の横ひもの上)
                iYoko = p_i横ひもの本数 - idx + 1
                isUpper = True
            End If
        Else
            '上から下へ(順方向)
            If is縦の上下に目あり() Then
                '1=最初の横ひもの上　～ 最後の横ひもの上,最後の横ひもの下
                If idx < get縦の目の実質数() Then
                    iYoko = idx
                    isUpper = True
                    dip = 1 - dInnerPosition
                Else
                    iYoko = idx - 1
                End If
            Else
                '1=最初の横ひもの下　～ 最後の横ひもの上(=最後-1の横ひもの下)
                iYoko = idx
            End If
        End If

        Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
        If itemYoko Is Nothing OrElse itemYoko.m_rひも位置.IsEmpty Then
            Return No_Double_Value
        End If
        If isUpper Then
            Return itemYoko.m_rひも位置.y最上 + _d目_ひも間のすき間 * dip
        Else
            Return itemYoko.m_rひも位置.y最下 - _d目_ひも間のすき間 * dip
        End If
    End Function


    '底の横ひもへの差しひも　drow には、tbl差しひもRow もしくは tbl縦横展開Row をセット
    Private Function image横ひもに差す(ByVal drow As CInsertItem, ByVal isCenterBand As Boolean, ByVal dInnerPosition As Double, ByVal n開始位置 As Integer, ByVal i何本ごと As Integer, ByVal draw As draw_position) As Integer
        If n開始位置 < 1 OrElse _ImageList差しひも Is Nothing Then
            Return -1
        End If

        Dim i_番号 As Integer = drow.p_i番号

        '本数
        Dim n本数 As Integer = p_i横ひもの本数
        If Not isCenterBand Then
            n本数 = get縦の目の実質数() '横ひもの本数-1 / 横ひもの本数+1
        End If
        If n本数 < n開始位置 Then
            Return n開始位置 - n本数
        End If

        'Dim i本幅 As Integer = drow.Value("p_i何本幅")
        Dim i本幅 As Integer = drow.p_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
        'Dim dひも長 As Double = drow.Value("m_dひも長")
        Dim dひも長 As Double = drow.m_dひも長

        'バンド描画位置
        Dim x_left As Double
        Dim x_right As Double
        If draw = draw_position.before Then
            '右側面
            x_left = getZeroX(1 / 2)
            x_right = getZeroX(1 / 2) + dひも長
        ElseIf draw = draw_position.after Then
            '左側面
            x_left = -getZeroX(1 / 2) - dひも長
            x_right = -getZeroX(1 / 2)
        Else
            'draw = draw_position.center
            x_left = -dひも長 / 2
            x_right = dひも長 / 2
        End If

        Dim isFirst As Boolean = True
        For idx As Integer = n開始位置 To n本数 Step i何本ごと
            Dim y_center As Double
            If isCenterBand Then
                y_center = get横ひもの中心Y(idx, (draw = draw_position.after), dInnerPosition)
            Else
                y_center = get横ひもの目の中心Y(idx, (draw = draw_position.after), dInnerPosition)
            End If
            If y_center = No_Double_Value Then
                Continue For
            End If

            'Dim item As New clsImageItem(ImageTypeEnum._ひも領域, drow, i_番号, idx)
            '色と記号
            Dim tmpitem As tbl縦横展開Row = (New tbl縦横展開DataTable).Newtbl縦横展開Row
            tmpitem.f_s色 = drow.p_s色
            tmpitem.f_s記号 = drow.m_s記号
            Dim item As New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(tmpitem), i_番号, idx)


            item.m_a四隅 = New S四隅(New S領域(New S実座標(x_left, y_center + d幅 / 2),
                                         New S実座標(x_right, y_center - d幅 / 2)))
            If isFirst Then
                '#60
                If IsDrawMarkCurrent Then
                    If (draw = draw_position.before) Then
                        item.p_p文字位置 = item.m_a四隅.p右下
                    ElseIf (draw = draw_position.after) Then
                        item.p_p文字位置 = item.m_a四隅.p左下
                    Else
                        item.p_p文字位置 = item.m_a四隅.p右上
                    End If
                End If
                isFirst = False
            End If

            _ImageList差しひも.AddItem(item)
            If i何本ごと = 0 Then
                Exit For
            End If
        Next

        Return get次の開始位置(n開始位置, n本数, i何本ごと)
    End Function
#End Region

#Region "底の縦ひも"
    ' ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃ ┃
    'dInnerPosition:同幅内位置(0～1値・中央が0.5)
    Private Function get縦ひもの中心X(ByVal idx As Integer, ByVal isDesc As Boolean, ByVal dInnerPosition As Double) As Double
        Dim iTate As Integer = idx
        Dim dip As Double = dInnerPosition
        If isDesc Then
            iTate = p_i縦ひもの本数 - idx + 1
            dip = 1 - dInnerPosition
        End If

        Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
        If itemTate Is Nothing OrElse itemTate.m_rひも位置.IsEmpty Then
            Return No_Double_Value
        End If
        Return (itemTate.m_rひも位置.x最右 - itemTate.m_rひも位置.x最左) * dip + itemTate.m_rひも位置.x最左
    End Function

    Private Function get縦ひもの目の中心X(ByVal idx As Integer, ByVal isDesc As Boolean, ByVal dInnerPosition As Double) As Double
        If idx < 1 OrElse get横の目の実質数() < idx Then
            Return No_Double_Value
        End If

        Dim iTate As Integer
        Dim dip As Double = dInnerPosition
        Dim isLeft As Boolean = False
        If isDesc Then
            '右から左へ(逆方向)
            If is横の左右に目あり() Then
                '1=最後の縦ひもの右　～ 最初の縦ひもの右,最初の縦ひもの左
                If idx < get横の目の実質数() Then
                    iTate = p_i縦ひもの本数 - idx + 1
                    dip = 1 - dInnerPosition
                Else
                    iTate = 1
                    isLeft = True
                End If
            Else
                '1=最後の縦ひもの左　～ 最初の縦ひもの右(=最後-1の縦ひもの左)
                iTate = p_i縦ひもの本数 - idx + 1
                isLeft = True
            End If
        Else
            '左から右へ(順方向)
            If is横の左右に目あり() Then
                '1=最初の縦ひもの左　～ 最後の縦ひもの左,最後の縦ひもの右
                If idx < get縦の目の実質数() Then
                    iTate = idx
                    isLeft = True
                    dip = 1 - dInnerPosition
                Else
                    iTate = idx - 1
                End If
            Else
                '1=最初の縦ひもの右　～ 最後の縦ひもの左(=最後-1の縦ひもの右)
                iTate = idx
            End If
        End If

        Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
        If itemTate Is Nothing OrElse itemTate.m_rひも位置.IsEmpty Then
            Return No_Double_Value
        End If
        If isLeft Then
            Return itemTate.m_rひも位置.x最左 - _d目_ひも間のすき間 * dip
        Else
            Return itemTate.m_rひも位置.x最右 + _d目_ひも間のすき間 * dip
        End If
    End Function


    '底の縦ひもへの差しひも　drow には、tbl差しひもRow もしくは tbl縦横展開Row をセット
    Private Function image縦ひもに差す(ByVal drow As CInsertItem, ByVal isCenterBand As Boolean, ByVal dInnerPosition As Double, ByVal n開始位置 As Integer, ByVal i何本ごと As Integer, ByVal draw As draw_position) As Integer
        If n開始位置 < 1 OrElse _ImageList差しひも Is Nothing Then
            Return -1
        End If

        Dim i_番号 As Integer = drow.p_i番号

        '本数
        Dim n本数 As Integer = p_i縦ひもの本数
        If Not isCenterBand Then
            n本数 = get横の目の実質数() '縦ひもの本数-1 / 縦ひもの本数+1
        End If
        If n本数 < n開始位置 Then
            Return n開始位置 - n本数
        End If

        'Dim i本幅 As Integer = drow.Value("p_i何本幅")
        Dim i本幅 As Integer = drow.p_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
        'Dim dひも長 As Double = drow.Value("m_dひも長")
        Dim dひも長 As Double = drow.m_dひも長

        'バンド描画長
        Dim y_up As Double
        Dim y_down As Double
        If draw = draw_position.before Then
            '上側面
            y_up = getZeroY(1 / 2) + dひも長
            y_down = getZeroY(1 / 2)
        ElseIf draw = draw_position.after Then
            '下側面
            y_up = -getZeroY(1 / 2)
            y_down = -getZeroY(1 / 2) - dひも長
        Else
            'draw = draw_position.center
            y_up = dひも長 / 2
            y_down = -dひも長 / 2
        End If

        Dim isFirst As Boolean = True
        For idx As Integer = n開始位置 To n本数 Step i何本ごと
            Dim x_center As Double
            If isCenterBand Then
                x_center = get縦ひもの中心X(idx, (draw = draw_position.after), dInnerPosition)
            Else
                x_center = get縦ひもの目の中心X(idx, (draw = draw_position.after), dInnerPosition)
            End If
            If x_center = No_Double_Value Then
                Continue For
            End If

            'Dim item As New clsImageItem(ImageTypeEnum._ひも領域, drow, i_番号, idx)
            Dim tmpitem As tbl縦横展開Row = (New tbl縦横展開DataTable).Newtbl縦横展開Row
            tmpitem.f_s色 = drow.p_s色
            tmpitem.f_s記号 = drow.m_s記号
            Dim item As New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(tmpitem), i_番号, idx)

            item.m_a四隅 = New S四隅(New S領域(New S実座標(x_center - d幅 / 2, y_up),
                                         New S実座標(x_center + d幅 / 2, y_down)))
            If isFirst Then
                '#60
                If IsDrawMarkCurrent Then
                    If (draw = draw_position.before) Then
                        item.p_p文字位置 = item.m_a四隅.p右上
                    ElseIf (draw = draw_position.after) Then
                        item.p_p文字位置 = item.m_a四隅.p右下
                    Else
                        item.p_p文字位置 = item.m_a四隅.p左下
                    End If
                End If
                isFirst = False
            End If

            _ImageList差しひも.AddItem(item)
            If i何本ごと = 0 Then
                Exit For
            End If
        Next

        Return get次の開始位置(n開始位置, n本数, i何本ごと)
    End Function
#End Region

#Region "側面高さ4方向"

    '--------------------------------------------------------
    '描画高さゼロ位置からの距離 dInnerPosition:同幅内位置(0～1値・中央が0.5)
    Private Function get側面ひもの中心高さDxDy(ByVal idx As Integer, ByVal dInnerPosition As Double) As Double
        If idx < 1 Then
            Return No_Double_Value
        End If

        '位置は上面から得る
        Dim item側面上 As clsImageItem = _imageList側面上.GetRowItem(enumひも種.i_側面, idx)
        If item側面上 Is Nothing OrElse item側面上.m_rひも位置.IsEmpty Then
            Return No_Double_Value
        End If
        Return ((item側面上.m_rひも位置.y最上 - item側面上.m_rひも位置.y最下) * dInnerPosition) + item側面上.m_rひも位置.y最下 - getZeroY(1 / 2)
    End Function

    Private Function get側面目の中心高さDxDy(ByVal idx As Integer, ByVal dInnerPosition As Double) As Double
        If idx < 1 OrElse get高さの目の実質数() < idx Then '_i高さの目の数 / _i高さの目の数+1
            Return No_Double_Value
        End If

        '位置は上面から得る
        Dim iTakasa As Integer
        If is最下段に目あり() Then
            '1=最初のひもの下,最初のひもの上　～ 最後の縦ひもの上
            If idx = 1 Then
                '最初のひもの最下位置は、_d最下段の目
                Return (_d最下段の目 * _d目_ひも間のすき間) - (_d目_ひも間のすき間 / 2)
            Else
                iTakasa = idx - 1
            End If
        Else
            '1=最初の縦ひもの上　～ 最後の縦ひもの上
            iTakasa = idx
        End If

        Dim item側面上 As clsImageItem = _imageList側面上.GetRowItem(enumひも種.i_側面, iTakasa)
        If item側面上 Is Nothing OrElse item側面上.m_rひも位置.IsEmpty Then
            Return No_Double_Value
        End If
        Return (item側面上.m_rひも位置.y最上 + _d目_ひも間のすき間 * dInnerPosition) - getZeroY(1 / 2)
    End Function


    Private Function 側面_水平(ByVal row As tbl差しひもRow, ByVal dInnerPosition As Double) As Boolean
        If _ImageList差しひも Is Nothing Then
            Return False
        End If

        Dim isCenterBand As Boolean = (row.f_i中心点 = enum中心点.i_ひも中央)
        Dim i本幅 As Integer = row.f_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)

        'バンド長
        Dim band_x As Double = get周の横(1 / 2)
        Dim band_y As Double = get周の縦(1 / 2)

        Dim item As clsImageItem

        '本数
        Dim n本数 As Integer = p_i側面ひもの本数
        If Not isCenterBand Then
            n本数 = get高さの目の実質数() '_i高さの目の数 / _i高さの目の数+1
        End If
        For idx As Integer = row.f_i開始位置 To n本数 Step row.f_i何本ごと
            Dim dxdy As Double
            If isCenterBand Then
                'ひもの上
                dxdy = get側面ひもの中心高さDxDy(idx, dInnerPosition)
            Else
                '目の中
                dxdy = get側面目の中心高さDxDy(idx, dInnerPosition)
            End If
            If dxdy = No_Double_Value Then
                Continue For
            End If
            Dim y_center As Double = dxdy + getZeroY(1 / 2)
            Dim x_center As Double = dxdy + getZeroX(1 / 2)

            '*上
            item = New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(row), row.f_i番号, idx)
            item.m_a四隅 = New S四隅(New S領域(New S実座標(band_x, y_center + d幅 / 2),
                                         New S実座標(-band_x, y_center - d幅 / 2)))
            '#60
            If IsDrawMarkCurrent Then
                item.p_p文字位置 = item.m_a四隅.p右上 + Unit0 * (_d基本のひも幅 / 2)
            End If
            _ImageList差しひも.AddItem(item)


            '*下
            item = New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(row), row.f_i番号, idx)
            item.m_a四隅 = New S四隅(New S領域(New S実座標(band_x, -y_center + d幅 / 2),
                                         New S実座標(-band_x, -y_center - d幅 / 2)))
            _ImageList差しひも.AddItem(item)


            '*左
            item = New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(row), row.f_i番号, idx)
            item.m_a四隅 = New S四隅(New S領域(New S実座標(-x_center + d幅 / 2, band_y),
                                         New S実座標(-x_center - d幅 / 2, -band_y)))
            _ImageList差しひも.AddItem(item)


            '*右
            item = New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(row), row.f_i番号, idx)
            item.m_a四隅 = New S四隅(New S領域(New S実座標(x_center + d幅 / 2, band_y),
                                         New S実座標(x_center - d幅 / 2, -band_y)))
            _ImageList差しひも.AddItem(item)

            If row.f_i何本ごと = 0 Then
                Exit For
            End If
        Next

        Return True
    End Function
#End Region

#Region "側面4面を斜めに"
    'Angle:90度から何度傾けるか Ratio:長さ/高さ
    Private Function angle_ratio(ByVal i角度 As enum角度, ByRef dAngle As Double, ByRef dRatio As Double) As Boolean
        Select Case i角度
            Case enum角度.i_45度
                dAngle = -45
                dRatio = ROOT2
            Case enum角度.i_135度
                dAngle = 45
                dRatio = ROOT2
            Case enum角度.i_18度
                dAngle = DELTA18.Angle - 90
                dRatio = DELTA18.Length / DELTA18.dY
            Case enum角度.i_72度
                dAngle = DELTA72.Angle - 90
                dRatio = DELTA72.Length / DELTA72.dY
            Case enum角度.i_108度
                dAngle = DELTA108.Angle - 90
                dRatio = DELTA72.Length / DELTA72.dY
            Case enum角度.i_162度
                dAngle = DELTA162.Angle - 90
                dRatio = DELTA18.Length / DELTA18.dY
            Case Else
                Return False 'no target
        End Select
        Return True
    End Function


    '上の側面と下の側面
    '※縦横ともdInnerPosition移動→中心位置が対角線上で移動
    Private Function image縦ひも面を斜めに(ByVal row As tbl差しひもRow, ByVal dInnerPosition As Double, ByVal n開始位置 As Integer, ByVal i何本ごと As Integer, ByVal draw As draw_position) As Integer
        If n開始位置 < 1 Then
            Return -1
        End If
        Dim isCenterBand As Boolean = (row.f_i中心点 = enum中心点.i_ひも中央)

        Dim n本数 As Integer = p_i縦ひもの本数
        If Not isCenterBand Then
            n本数 = get横の目の実質数() '縦ひもの本数-1 / 縦ひもの本数+1
        End If
        If n本数 < n開始位置 Then
            Return n開始位置 - n本数
        End If

        Dim dAngle As Double '90度から何度傾けるか
        Dim dRatio As Double '長さ/高さ
        If Not angle_ratio(row.f_i角度, dAngle, dRatio) Then
            Return -1 'no target
        End If

        Dim dxdy As Double
        If isCenterBand Then
            dxdy = get側面ひもの中心高さDxDy(1, IIf(0 < dAngle, dInnerPosition, 1 - dInnerPosition)) '1番目のひもの中心
        Else
            dxdy = get側面目の中心高さDxDy(1, IIf(0 < dAngle, dInnerPosition, 1 - dInnerPosition)) '1番目の目の中心
        End If
        If dxdy = No_Double_Value Then
            Return -1 'no more
        End If
        Dim y_center As Double = dxdy + getZeroY(1 / 2)
        If (draw = draw_position.after) Then
            y_center = -y_center
        End If


        Dim i本幅 As Integer = row.f_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
        Dim isFirst As Boolean = True
        For idx As Integer = n開始位置 To n本数 Step i何本ごと
            Dim x_center As Double
            If isCenterBand Then
                x_center = get縦ひもの中心X(idx, (draw = draw_position.after), dInnerPosition)
            Else
                x_center = get縦ひもの目の中心X(idx, (draw = draw_position.after), dInnerPosition)
            End If
            If x_center = No_Double_Value Then
                Continue For
            End If

            Dim rBand0 As S領域
            If (draw = draw_position.after) Then
                rBand0 = New S領域(New S実座標(x_center - d幅 / 2, y_center + dRatio * dxdy),
                                  New S実座標(x_center + d幅 / 2, y_center - (row.f_dひも長 - dRatio * dxdy)))
            Else
                rBand0 = New S領域(New S実座標(x_center - d幅 / 2, y_center + (row.f_dひも長 - dRatio * dxdy)),
                                  New S実座標(x_center + d幅 / 2, y_center - dRatio * dxdy))
            End If

            Dim aBand As New S四隅(rBand0)


            Dim item As New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(row), row.f_i番号, idx)
            item.m_a四隅 = aBand.Rotate(New S実座標(x_center, y_center), dAngle)
            If isFirst Then
                '#60
                If IsDrawMarkCurrent Then
                    If (draw = draw_position.after) Then
                        item.p_p文字位置 = item.m_a四隅.p右下
                    Else
                        item.p_p文字位置 = item.m_a四隅.p左上
                    End If
                End If
                isFirst = False
            End If

            _ImageList差しひも.AddItem(item)

            If i何本ごと = 0 Then
                Exit For
            End If
        Next

        Dim ret As Integer = get次の開始位置(n開始位置, n本数, i何本ごと)
        Return get次の開始位置(n開始位置, n本数, i何本ごと)
    End Function

    '右の側面と左の側面
    Private Function image横ひも面を斜めに(ByVal row As tbl差しひもRow, ByVal dInnerPosition As Double, ByVal n開始位置 As Integer, ByVal i何本ごと As Integer, ByVal draw As draw_position) As Integer
        If n開始位置 < 1 OrElse _ImageList差しひも Is Nothing Then
            Return -1
        End If
        Dim isCenterBand As Boolean = (row.f_i中心点 = enum中心点.i_ひも中央)

        Dim n本数 As Integer = p_i横ひもの本数
        If Not isCenterBand Then
            n本数 = get縦の目の実質数() '横ひもの本数-1 / 横ひもの本数+1
        End If
        If n本数 < n開始位置 Then
            Return n開始位置 - n本数
        End If

        Dim dAngle As Double '90度から何度傾けるか
        Dim dRatio As Double '長さ/高さ
        If Not angle_ratio(row.f_i角度, dAngle, dRatio) Then
            Return -1 'no target
        End If

        Dim dxdy As Double
        If isCenterBand Then
            dxdy = get側面ひもの中心高さDxDy(1, IIf(0 < dAngle, dInnerPosition, 1 - dInnerPosition)) '1番目のひもの中心
        Else
            dxdy = get側面目の中心高さDxDy(1, IIf(0 < dAngle, dInnerPosition, 1 - dInnerPosition)) '1番目の目の中心
        End If
        If dxdy = No_Double_Value Then
            Return -1 'no more
        End If
        Dim x_center As Double = dxdy + getZeroX(1 / 2)
        If (draw = draw_position.after) Then
            x_center = -x_center
        End If


        Dim i本幅 As Integer = row.f_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)

        Dim isFirst As Boolean = True
        For idx As Integer = n開始位置 To n本数 Step i何本ごと
            Dim y_center As Double
            If isCenterBand Then
                'バンド内の位置
                y_center = get横ひもの中心Y(idx, (draw = draw_position.after), dInnerPosition)
            Else
                y_center = get横ひもの目の中心Y(idx, (draw = draw_position.after), dInnerPosition)
            End If
            If y_center = No_Double_Value Then
                Continue For
            End If

            Dim rBand0 As S領域
            If (draw = draw_position.after) Then
                rBand0 = New S領域(New S実座標(x_center - (row.f_dひも長 - dRatio * dxdy), y_center - d幅 / 2),
                                  New S実座標(x_center + dRatio * dxdy, y_center + d幅 / 2))
            Else
                rBand0 = New S領域(New S実座標(x_center - dRatio * dxdy, y_center - d幅 / 2),
                                  New S実座標(x_center + (row.f_dひも長 - dRatio * dxdy), y_center + d幅 / 2))
            End If

            Dim aBand As New S四隅(rBand0)

            Dim item As New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(row), row.f_i番号, idx)
            item.m_a四隅 = aBand.Rotate(New S実座標(x_center, y_center), dAngle)
            If isFirst Then
                '#60
                If IsDrawMarkCurrent Then
                    If (draw = draw_position.after) Then
                        item.p_p文字位置 = item.m_a四隅.p左上
                    Else
                        item.p_p文字位置 = item.m_a四隅.p右下
                    End If
                End If
                isFirst = False
            End If

            _ImageList差しひも.AddItem(item)

            If i何本ごと = 0 Then
                Exit For
            End If
        Next

        Return get次の開始位置(n開始位置, n本数, i何本ごと)
    End Function
#End Region

#Region "底を斜めに"
    ' ←←←←←←←←←←←←←←←←＼135度
    '↓   45度／→→→→→→→→→→→→→→→→→
    '↓　┌──┬──┬──┬──┬──┬──┐　↓ y_line                                      →横方向: 縦置き計算(中心まで↓dy)
    '↓　│1／ │2／ │3／ │4／ │　　│n／1│　↓← y_center1   (x_center,y_center)              ┌┐  ┌┐  ┌┐  
    '↓　├──┼──┼──┼──┼──┼──┤　↓                　目の中心/交差ひもの中心       ││  ││  ││  ↓縦方向:横置き計算(中心まで←dx)
    '↓　│　　│　  │　　│　　│　　│ ／2│　↓                  イメージ回転の中心            ││  ││  ││     ┌──────┐
    '↓　├──┼──┼──┼──┼──┼──┤　↓                                                ││  ││  ││     └──────┘
    '↓　│　　│　  │　　│　　│　　│ ／3│　↓                                             　 └┘  └┘  └┘     ┌──────┐ 
    '↓　├──┼──┼──┼──┼──┼──┤　↓                                 　　　     m_d長さ:回転前の長さ     └──────┘
    '↓　│　　│　  │　　│　　│　　│    │　↓       
    '↓　├──┼──┼──┼──┼──┼──┤　↓                  1～n:横の目の実質数/縦ひもの本数(全面＆0<高さの目の実質数＆横の左右に目ありの時は+2)
    '↓　│　　│　  │　　│　　│　　│ ／m│　↓                  1～m;縦の目の実質数/横ひもの本数(全面＆0<高さの目の実質数＆縦の上下に目ありの時は+2)
    '　　└──┴──┴──┴──┴──┴──┘     -y_line
    '-x_line ↑                            ↑   x_line
    '      x_center1                  x_centerCorner

    '全数
    Private Function get底の斜めCount(ByVal row As tbl差しひもRow) As Integer
        If row.f_i中心点 = enum中心点.i_目の中央 Then
            Return get横の目の実質数() + get縦の目の実質数() - 1
        ElseIf row.f_i中心点 = enum中心点.i_ひも中央 Then
            Dim i縦ひもの本数 As Integer = p_i縦ひもの本数
            Dim i横ひもの本数 As Integer = p_i横ひもの本数
            If row.f_i配置面 = enum配置面.i_全面 AndAlso 0 < get高さの目の実質数() Then
                If is横の左右に目あり() Then
                    i縦ひもの本数 += 2 '左右の目の外に仮想的なひも
                End If
                If is縦の上下に目あり() Then
                    i横ひもの本数 += 2 '上下の目の外に仮想的なひも
                End If
            End If
            Return i縦ひもの本数 + i横ひもの本数 - 1
        Else
            Return -1
        End If
    End Function

    'ひも長テーブル、ひも幅変更なしのため、45度も135度も同じ長さ
    Private Function get底の斜めLength(ByVal row As tbl差しひもRow) As CInsertItemList
        Dim n点数 As Integer = get底の斜めCount(row)
        Dim count As Integer = get該当数(row.f_i開始位置, n点数, row.f_i何本ごと)
        If 0 = count Then
            Return Nothing
        End If

        '斜め指定
        Dim iひも種 As Integer
        If row.f_i角度 = enum角度.i_45度 Then
            iひも種 = enumひも種.i_45度
        ElseIf row.f_i角度 = enum角度.i_135度 Then
            iひも種 = enumひも種.i_315度
        Else
            Return Nothing
        End If

        '斜めひもが配置される全体エリア
        Dim x_line As Double '絶対値
        Dim y_line As Double '絶対値
        Dim dShortCorner As Double = 0 '角を短くする分、長さ(回転前)ベース
        If row.f_i配置面 = enum配置面.i_全面 Then
            x_line = get周の横(1 / 2) + get側面高(1)
            y_line = get周の縦(1 / 2) + get側面高(1)
            If is縦の上下に目あり() AndAlso is横の左右に目あり() Then
                '側面高の対角線ではなく、側面高にする
                dShortCorner = get側面高(1) * (1 - ROOT2 / 2)
            Else
                '角まで短くする
                dShortCorner = get側面高(1) + (get周の横() - p_d四角ベース_横) / 2
            End If
        ElseIf row.f_i配置面 = enum配置面.i_底面 Then
            x_line = p_d四角ベース_横 / 2
            y_line = p_d四角ベース_縦 / 2
        Else
            Return Nothing
        End If
        Dim maxLine As Double = mdlUnit.Min(x_line, y_line) * 2

        '縦横の点数と通し番号 #23
        Dim nCorner As Integer  '角になるnの通し番号,横に並ぶ数
        Dim nCorner2 As Integer  'もう一方の角の通し番号,縦に並ぶ数
        'n点数 = nCorner + nCorner2 - 1

        If row.f_i中心点 = enum中心点.i_ひも中央 Then
            Dim i縦ひもの本数 As Integer = p_i縦ひもの本数
            Dim i横ひもの本数 As Integer = p_i横ひもの本数
            If row.f_i配置面 = enum配置面.i_全面 AndAlso 0 < get高さの目の実質数() Then
                If is横の左右に目あり() Then
                    i縦ひもの本数 += 2 '左右の目の外に仮想的なひも
                End If
                If is縦の上下に目あり() Then
                    i横ひもの本数 += 2 '上下の目の外に仮想的なひも
                End If
            End If
            nCorner = i縦ひもの本数
            nCorner2 = i横ひもの本数 '反対の角

        ElseIf row.f_i中心点 = enum中心点.i_目の中央 Then
            nCorner = get横の目の実質数()
            nCorner2 = get縦の目の実質数()
        Else
            Return Nothing
        End If


        '座標値
        '開始位置となるX
        Dim x_center1 As Double = -(nCorner - 1) * p_d縦横_四角 / 2
        '角(nCorner)のX
        Dim x_centerCorner As Double = -x_center1
        '角(nCorner=縦の1)のY
        Dim y_center1 As Double = (nCorner2 - 1) * p_d縦横_四角 / 2

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "i番号:{0} n点数={1} nCorner={2} nCorner2={3} x_center1={4:0.0} x_centerCorner({5:0.0}) y_center1({6:0.0}) ",
                                  row.f_i番号, n点数, nCorner, nCorner2, x_center1, x_centerCorner, y_center1)

        '各長さのレコード作成
        Dim tmptable As New CInsertItemList(row)
        For i As Integer = row.f_i開始位置 To n点数 Step row.f_i何本ごと
            Dim tmp As New CInsertItem(tmptable)
            tmp.m_iひも種 = iひも種
            tmp.m_iひも番号 = i

            Dim idx As Integer
            Dim x_center As Double
            Dim y_center As Double
            If i < nCorner Then
                '→横にカウント
                idx = i
                x_center = x_center1 + p_d縦横_四角 * (idx - 1)
                y_center = y_center1
                tmp.m_iFlag = 1 '横へ
                tmp.m_delta = New S差分((x_center + x_line), (y_line - y_center)) 'dx,dy
                tmp.m_d長さ = tmp.m_delta.dX + tmp.m_delta.dY

            ElseIf i = nCorner Then
                '角位置
                idx = i
                x_center = x_centerCorner
                y_center = y_center1
                '左上との距離
                Dim dx1 As Double = (x_center + x_line)
                Dim dy1 As Double = (y_line - y_center)
                '右下との距離
                Dim dx2 As Double = (x_line - x_center)
                Dim dy2 As Double = (y_center + y_line)
                If dy1 <= dx2 Then
                    tmp.m_iFlag = 1 '横扱い
                    tmp.m_delta = New S差分(dx1, dy1)
                    tmp.m_d長さ = tmp.m_delta.dX + tmp.m_delta.dY
                    If 0 < dShortCorner Then
                        tmp.m_delta.dY = dy1 - dShortCorner
                        tmp.m_d長さ -= dShortCorner
                        If (maxLine - dShortCorner) < tmp.m_d長さ Then
                            tmp.m_d長さ = maxLine - dShortCorner
                        End If
                    End If
                Else
                    tmp.m_iFlag = 0 '縦扱い
                    tmp.m_delta = New S差分(dx2, dy2)
                    tmp.m_d長さ = tmp.m_delta.dX + tmp.m_delta.dY
                    If 0 < dShortCorner Then
                        tmp.m_delta.dX = dx2 - dShortCorner
                        tmp.m_d長さ -= dShortCorner
                        If (maxLine - dShortCorner) < tmp.m_d長さ Then
                            tmp.m_d長さ = maxLine - dShortCorner
                        End If
                    End If
                End If

            Else
                '↓下にカウント
                idx = i - nCorner + 1
                x_center = x_centerCorner
                y_center = y_center1 - p_d縦横_四角 * (idx - 1)
                tmp.m_iFlag = 0 '下へ
                tmp.m_delta = New S差分((x_line - x_center), (y_center + y_line)) 'dx,dy
                tmp.m_d長さ = tmp.m_delta.dX + tmp.m_delta.dY
            End If

            If i = nCorner2 AndAlso 0 < dShortCorner Then
                tmp.m_d長さ -= dShortCorner
            End If
            If maxLine < tmp.m_d長さ Then
                tmp.m_d長さ = maxLine
            End If

            'set
            tmp.m_iひも数 = 1 'ひも数
            tmp.m_i開始位置 = i
            tmp.m_idx = idx
            tmp.m_dひも長 = tmp.m_d長さ * ROOT2
            'tmp.p_d出力ひも長 = tmp.m_dひも長 + 2 * row.f_dひも長加算

            '保存値
            tmp.m_pCenter = New S実座標(x_center, y_center)

            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "flg={0} idx={1} delta={2} 長さ={3:0.0} ひも長={4:0.0} center{5}", tmp.m_iFlag, idx, tmp.m_delta, tmp.m_d長さ, tmp.m_dひも長, tmp.m_pCenter)

            tmptable.Add(tmp)
            If row.f_i何本ごと = 0 Then
                Exit For
            End If
        Next
        If 0 < tmptable.Count Then
            Return tmptable
        Else
            Return Nothing
        End If
    End Function

    '斜めの差しひものイメージ dInnerPosition:同幅内位置(0～1値・中央が0.5)
    Private Function imageList底の斜め(ByVal row As tbl差しひもRow, ByVal dInnerPosition As Double) As Boolean
        If Not _InsertExpand.ContainsKey(row.f_i番号) Then
            Return False
        End If

        Dim d対角線幅 As Double
        Dim i本幅 As Integer = row.f_i何本幅
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
        If row.f_i中心点 = enum中心点.i_ひも中央 Then
            d対角線幅 = _d基本のひも幅 * ROOT2 '縦ひも×横ひもを見るべきところを簡易化
        ElseIf row.f_i中心点 = enum中心点.i_目の中央 Then
            d対角線幅 = _d目_ひも間のすき間 * ROOT2
        End If

        Dim bRevert As Boolean
        Dim delta As S差分
        If row.f_i角度 = enum角度.i_45度 Then
            bRevert = False
            delta = New S差分(-45)
        ElseIf row.f_i角度 = enum角度.i_135度 Then
            bRevert = True
            delta = New S差分(-135)
        Else
            Return -1 'no more
        End If
        delta = delta * (dInnerPosition - 0.5) * d対角線幅

        Dim tmptable As CInsertItemList = _InsertExpand.GetList(row.f_i番号)
        For Each tmp As CInsertItem In tmptable

            'Dim item As New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(tmp), row.p_i番号, tmp.m_iひも番号)
            '色と記号
            Dim tmpitem As tbl縦横展開Row = (New tbl縦横展開DataTable).Newtbl縦横展開Row
            tmpitem.f_s色 = row.f_s色
            tmpitem.f_s記号 = row.f_s記号
            Dim item As New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(tmpitem), row.f_i番号, tmp.m_iひも番号)

            Dim x_center As Double = tmp.m_pCenter.X
            Dim y_center As Double = tmp.m_pCenter.Y
            Dim dx As Double = tmp.m_delta.dX
            Dim dy As Double = tmp.m_delta.dY
            Dim sign As Integer = 1

            Dim rBand0 As S領域
            If (0 < tmp.m_iFlag) Then '横へ
                rBand0 = New S領域(New S実座標(x_center - d幅 / 2, y_center + ROOT2 * dy),
                                  New S実座標(x_center + d幅 / 2, y_center - tmp.m_dひも長 + ROOT2 * dy))
            Else
                rBand0 = New S領域(New S実座標(x_center - tmp.m_dひも長 + ROOT2 * dx, y_center + d幅 / 2),
                                  New S実座標(x_center + ROOT2 * dx, y_center - d幅 / 2))
                sign = -1
            End If

            Dim aBand As New S四隅(rBand0)

            'sign = 0 'for debug
            item.m_a四隅 = aBand.Rotate(New S実座標(x_center, y_center), -45 * sign)
            If bRevert Then
                item.m_a四隅 = item.m_a四隅.VertLeft()
            End If
            item.m_a四隅 = item.m_a四隅 + delta
            '#60
            If IsDrawMarkCurrent Then
                item.p_p文字位置 = New S実座標(x_center, y_center)
                If bRevert Then
                    item.p_p文字位置 = item.p_p文字位置.VertLeft()
                End If
                item.p_p文字位置 = item.p_p文字位置 + delta
            End If

            _ImageList差しひも.AddItem(item)
        Next

        Return True
    End Function

#End Region

End Class
