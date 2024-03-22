Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables
Imports CraftBand.clsUpDown
Imports CraftBand.Tables.dstDataTables

Partial Public Class clsCalcSquare

    Dim DELTA72 As New S差分(1, 3) '右に1個・上に3個
    Dim DELTA18 As New S差分(3, 1) '右に3個・上に1個
    Dim DELTA108 As New S差分(-1, 3) '左に1個・上に3個
    Dim DELTA162 As New S差分(-3, 1) '左に3個・上に1個



    '
    'ひも長で描画(出力ひも長は、これに係数をかけて、ひも長加算する)
    '　　　　　　　　┌──────┐
    '　　　　　　　　├──────┤           側面枠の高さ = get側面高() +  _d縁の高さ
    '　　　　　　　　│　　　　　　│           
    '　　　　　　　　├‥‥‥‥‥‥┤           ─ get周の横() × get周の縦()  
    '　　　　┌┬─┬┼──────┼┬─┬┐           ↑  (この差は)
    '　　　　││　：│┏━━━━┓│：　││           ↓f_d立ち上げ時の四角底周の増分
    '　　　　││　：│┃　　　　┃│：　││   ━　p_d四角ベース_横 × p_d四角ベース_縦
    '　　　　││　：│┃　　　　┃│：　││   　　　　　(縦ひも・横ひもはこの中)
    '　　　　││　：│┗━━━━┛│：　││   　
    '　　　　└┴─┴┼──────┼┴─┴┘     
    '　点線がZero位置├‥‥‥‥‥‥┤f_d底の厚さ 　　　　　┐
    '　　　　　　　　│　　　　　　│p_d四角ベース_高さ　　┘get側面高()
    '　　　　　　　　├──────┤　　　　　　　　　　　　　　　　　　
    '　　　　　　　　└──────┘_d縁の高さ/_d縁の垂直ひも長
    '
    '描画するひもの長さ = get周の横()/get周の縦()   
    '　　　　　　　　　       + get側面高()
    '　　　　　　　　　　　　 + [_d縁の高さ と _d縁の垂直ひも長 の小さい方]
    '
    '


    'プレビュー時に生成,描画後はNothing
    Dim _ImageList横ひも As clsImageItemList   '横ひもの展開レコードを含む
    Dim _ImageList縦ひも As clsImageItemList   '縦ひもの展開レコードを含む
    Dim _imageList側面上 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面左 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面下 As clsImageItemList    '側面の展開レコードを含む
    Dim _imageList側面右 As clsImageItemList    '側面の展開レコードを含む
    Dim _ImageList差しひも As clsImageItemList
    Dim _ImageList描画要素 As clsImageItemList '底と側面


    'プレビュー画像生成
    'isBackFace=trueの時、UpDownを裏面として適用
    Public Function CalcImage(ByVal imgData As clsImageData, ByVal isBackFace As Boolean) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '念のため
        _ImageList横ひも = Nothing
        _ImageList縦ひも = Nothing
        _imageList側面上 = Nothing
        _imageList側面左 = Nothing
        _imageList側面下 = Nothing
        _imageList側面右 = Nothing
        _ImageList描画要素 = Nothing
        _ImageList差しひも = Nothing

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        '_tbl縦横展開_横ひも,_tbl縦横展開_縦ひもにレコードが残されているはず
        '_ImageList横ひも, _ImageList縦ひもを作る
        If Not imageList横ひも() OrElse Not imageList縦ひも() Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '基本のひも幅(文字サイズ)と基本色
        imgData.setBasics(_d基本のひも幅, _Data.p_row目標寸法.Value("f_s基本色"))


        If Not imageList四側面() Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If
        _ImageList描画要素 = imageList底と側面枠()


        '描画用のデータ追加
        regionUpDown底(isBackFace)
        regionUpDown側面1(isBackFace)
        regionUpDown側面2(isBackFace)
        regionUpDown側面3(isBackFace)
        regionUpDown側面4(isBackFace)

        If Not isBackFace Then
            '差しひも
            imageList差しひも() '_ImageList差しひも生成
        End If

        '中身を移動
        imgData.MoveList(_ImageList横ひも)
        _ImageList横ひも = Nothing
        imgData.MoveList(_ImageList縦ひも)
        _ImageList縦ひも = Nothing

        imgData.MoveList(_imageList側面上)
        imgData.MoveList(_imageList側面左)
        imgData.MoveList(_imageList側面下)
        imgData.MoveList(_imageList側面右)
        _imageList側面上 = Nothing
        _imageList側面左 = Nothing
        _imageList側面下 = Nothing
        _imageList側面右 = Nothing

        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing
        imgData.MoveList(_ImageList差しひも)
        _ImageList差しひも = Nothing

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function


#Region "描画のベースとなる値"

    Private Const No_Double_Value As Double = Double.MaxValue

    '描画の位置
    Enum draw_position
        before '上・右の側面
        center '底面
        after '下・左の側面
    End Enum

    '底の配置領域に立ち上げ増分をプラス, = p_d四角ベース_周
    Private Function get側面の周長() As Double
        Return 2 * (_d四角ベース_横計 + _d四角ベース_縦計) _
            + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分")
    End Function

    '横ひも長計算・描画に使用
    Private Function get周の横(Optional ByVal multi As Double = 1) As Double
        Return multi *
            (_d四角ベース_横計 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4)
    End Function

    '縦ひも長計算・描画に使用
    Private Function get周の縦(Optional ByVal multi As Double = 1) As Double
        Return multi *
            (_d四角ベース_縦計 + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4)
    End Function

    '縦ひも長計算・描画に使用   '縁は含まない
    Private Function get側面高(ByVal count As Integer) As Double
        Return count * (_d四角ベース_高さ計 + getZeroSide())
    End Function

    '側面の高さゼロ位置(1=1端)
    Private Function getZeroSide(Optional ByVal multi As Double = 1) As Double
        Return multi * g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
    End Function

    '描画の横高ゼロ描画位置
    Private Function getZeroX(Optional ByVal multi As Double = 1) As Double
        Return multi * (get周の横() + getZeroSide(2))
    End Function

    '描画の縦高ゼロ描画位置
    Private Function getZeroY(Optional ByVal multi As Double = 1) As Double
        Return multi * (get周の縦() + getZeroSide(2))
    End Function

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

#Region "差しひも"

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

#Region "呼び出しケース分類"

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


    '差しひもの各長をセットしたテーブルを返す。(固定長ではないケースのみ)
    '※リスト出力時に呼び出し
    Private Function get差しひもLength(ByVal row As tbl差しひもRow) As tbl縦横展開DataTable
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

                        Dim tmptable As tbl縦横展開DataTable = New tbl縦横展開DataTable
                        Dim n開始位置 As Integer = row.f_i開始位置
                        '縦ひも
                        Dim count As Integer = get該当数(n開始位置, i縦ひもの本数, row.f_i何本ごと)
                        If 0 < count Then
                            Dim tmp As tbl縦横展開Row = tmptable.Newtbl縦横展開Row
                            'key
                            tmp.f_iひも種 = enumひも種.i_縦
                            tmp.f_iひも番号 = 1
                            'copy
                            tmp.f_i何本幅 = row.f_i何本幅
                            tmp.f_s色 = row.f_s色
                            tmp.f_i位置番号 = row.f_i番号
                            'set
                            tmp.f_iVal1 = count 'ひも数
                            tmp.f_iVal2 = n開始位置
                            tmp.f_dひも長 = get周の縦() + get側面高(2) '縁は加えない
                            tmp.f_d出力ひも長 = tmp.f_dひも長 + 2 * row.f_dひも長加算
                            'skip:f_sひも名,f_d幅,f_d長さ
                            tmptable.Rows.Add(tmp)
                        End If
                        n開始位置 = get次の開始位置(n開始位置, i縦ひもの本数, row.f_i何本ごと)

                        '横ひも
                        count = get該当数(n開始位置, i横ひもの本数, row.f_i何本ごと)
                        If 0 < count Then
                            Dim tmp As tbl縦横展開Row = tmptable.Newtbl縦横展開Row
                            'key
                            tmp.f_iひも種 = enumひも種.i_横
                            tmp.f_iひも番号 = 2
                            'copy
                            tmp.f_i何本幅 = row.f_i何本幅
                            tmp.f_s色 = row.f_s色
                            tmp.f_i位置番号 = row.f_i番号
                            'set
                            tmp.f_iVal1 = count 'ひも数
                            tmp.f_iVal2 = n開始位置
                            tmp.f_dひも長 = get周の横() + get側面高(2) '縁は加えない
                            tmp.f_d出力ひも長 = tmp.f_dひも長 + 2 * row.f_dひも長加算
                            'skip:f_i位置番号,f_sひも名,f_d幅,f_d長さ
                            tmptable.Rows.Add(tmp)
                        End If
                        If 0 < tmptable.Rows.Count Then
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


    '_ImageList差しひも生成
    'プレビュー時に呼び出し(プレビュー処理内でリスト出力後)
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
                            image横ひもに差す(New clsDataRow(row), isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.center)

                        Case enum角度.i_90度  '底の縦 c 
                            image縦ひもに差す(New clsDataRow(row), isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.center)

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
                            n開始位置 = image縦ひもに差す(New clsDataRow(row), isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.before) '1～縦の本数
                            n開始位置 = image横ひもに差す(New clsDataRow(row), isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.before) '縦の本数+1 ～横の本数+縦ひも本数
                            n開始位置 = image縦ひもに差す(New clsDataRow(row), isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.after) '横の本数+縦の本数+1 ～ 2*縦の本数+縦の本数
                            n開始位置 = image横ひもに差す(New clsDataRow(row), isCenterBand, dInnerPosition, n開始位置, i何本ごと, draw_position.after)  '2*縦の本数+縦の本数+1 ～ 2*横の本数+2*縦の本数

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
                            If Not _sasihimo.ContainsKey(row.f_i番号) Then
                                Return False
                            End If
                            Dim tmptable As tbl縦横展開DataTable = _sasihimo(row.f_i番号)
                            For Each tmp As tbl縦横展開Row In tmptable
                                'n開始位置←tmp.f_iVal2
                                If tmp.f_iひも種 = enumひも種.i_横 Then
                                    image横ひもに差す(New clsDataRow(tmp), isCenterBand, dInnerPosition, tmp.f_iVal2, i何本ごと, draw_position.center) '1～横ひも本数
                                ElseIf tmp.f_iひも種 = enumひも種.i_縦 Then
                                    image縦ひもに差す(New clsDataRow(tmp), isCenterBand, dInnerPosition, tmp.f_iVal2, i何本ごと, draw_position.center) '横の本数+1 ～ 横の本数+縦の本数
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
    Private Function image横ひもに差す(ByVal drow As clsDataRow, ByVal isCenterBand As Boolean, ByVal dInnerPosition As Double, ByVal n開始位置 As Integer, ByVal i何本ごと As Integer, ByVal draw As draw_position) As Integer
        If n開始位置 < 1 OrElse _ImageList差しひも Is Nothing Then
            Return -1
        End If

        Dim i_番号 As Integer
        If drow.ContainsName("f_i番号") Then
            i_番号 = drow.Value("f_i番号") 'tbl差しひもRow
        Else
            i_番号 = drow.Value("f_i位置番号") 'tbl縦横展開Row
        End If

        '本数
        Dim n本数 As Integer = p_i横ひもの本数
        If Not isCenterBand Then
            n本数 = get縦の目の実質数() '横ひもの本数-1 / 横ひもの本数+1
        End If
        If n本数 < n開始位置 Then
            Return n開始位置 - n本数
        End If

        Dim i本幅 As Integer = drow.Value("f_i何本幅")
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
        Dim dひも長 As Double = drow.Value("f_dひも長")

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

            Dim item As New clsImageItem(ImageTypeEnum._ひも領域, drow, i_番号, idx)
            item.m_a四隅 = New S四隅(New S領域(New S実座標(x_left, y_center + d幅 / 2),
                                         New S実座標(x_right, y_center - d幅 / 2)))
            If isFirst Then
                If (draw = draw_position.before) Then
                    item.p_p文字位置 = item.m_a四隅.p右下
                ElseIf (draw = draw_position.after) Then
                    item.p_p文字位置 = item.m_a四隅.p左下
                Else
                    item.p_p文字位置 = item.m_a四隅.p右上
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
    Private Function image縦ひもに差す(ByVal drow As clsDataRow, ByVal isCenterBand As Boolean, ByVal dInnerPosition As Double, ByVal n開始位置 As Integer, ByVal i何本ごと As Integer, ByVal draw As draw_position) As Integer
        If n開始位置 < 1 OrElse _ImageList差しひも Is Nothing Then
            Return -1
        End If

        Dim i_番号 As Integer
        If drow.ContainsName("f_i番号") Then
            i_番号 = drow.Value("f_i番号") 'tbl差しひもRow
        Else
            i_番号 = drow.Value("f_i位置番号") 'tbl縦横展開Row
        End If

        '本数
        Dim n本数 As Integer = p_i縦ひもの本数
        If Not isCenterBand Then
            n本数 = get横の目の実質数() '縦ひもの本数-1 / 縦ひもの本数+1
        End If
        If n本数 < n開始位置 Then
            Return n開始位置 - n本数
        End If

        Dim i本幅 As Integer = drow.Value("f_i何本幅")
        Dim d幅 As Double = g_clsSelectBasics.p_d指定本幅(i本幅)
        Dim dひも長 As Double = drow.Value("f_dひも長")

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

            Dim item As New clsImageItem(ImageTypeEnum._ひも領域, drow, i_番号, idx)
            item.m_a四隅 = New S四隅(New S領域(New S実座標(x_center - d幅 / 2, y_up),
                                         New S実座標(x_center + d幅 / 2, y_down)))
            If isFirst Then
                If (draw = draw_position.before) Then
                    item.p_p文字位置 = item.m_a四隅.p右上
                ElseIf (draw = draw_position.after) Then
                    item.p_p文字位置 = item.m_a四隅.p右下
                Else
                    item.p_p文字位置 = item.m_a四隅.p左下
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
            item.p_p文字位置 = item.m_a四隅.p右上 + Unit0 * (_d基本のひも幅 / 2)
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
                If (draw = draw_position.after) Then
                    item.p_p文字位置 = item.m_a四隅.p右下
                Else
                    item.p_p文字位置 = item.m_a四隅.p左上
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
                If (draw = draw_position.after) Then
                    item.p_p文字位置 = item.m_a四隅.p左上
                Else
                    item.p_p文字位置 = item.m_a四隅.p右下
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
    '↓　├──┼──┼──┼──┼──┼──┤　↓                                 　　　     f_d長さ:回転前の長さ     └──────┘
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
    Private Function get底の斜めLength(ByVal row As tbl差しひもRow) As tbl縦横展開DataTable
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
        Dim tmptable As tbl縦横展開DataTable = New tbl縦横展開DataTable
        For i As Integer = row.f_i開始位置 To n点数 Step row.f_i何本ごと
            Dim tmp As tbl縦横展開Row = tmptable.Newtbl縦横展開Row
            tmp.f_iひも種 = iひも種
            tmp.f_iひも番号 = i
            'copy
            tmp.f_i何本幅 = row.f_i何本幅
            tmp.f_s色 = row.f_s色
            tmp.f_i位置番号 = row.f_i番号

            Dim idx As Integer
            Dim x_center As Double
            Dim y_center As Double
            If i < nCorner Then
                '→横にカウント
                idx = i
                x_center = x_center1 + p_d縦横_四角 * (idx - 1)
                y_center = y_center1
                tmp.f_iVal4 = 1 '横へ
                tmp.f_dVal1 = (x_center + x_line) 'dx
                tmp.f_dVal2 = (y_line - y_center) 'dy
                tmp.f_d長さ = tmp.f_dVal1 + tmp.f_dVal2

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
                    tmp.f_iVal4 = 1 '横扱い
                    tmp.f_dVal1 = dx1
                    tmp.f_dVal2 = dy1
                    tmp.f_d長さ = tmp.f_dVal1 + tmp.f_dVal2
                    If 0 < dShortCorner Then
                        tmp.f_dVal2 = dy1 - dShortCorner
                        tmp.f_d長さ -= dShortCorner
                        If (maxLine - dShortCorner) < tmp.f_d長さ Then
                            tmp.f_d長さ = maxLine - dShortCorner
                        End If
                    End If
                Else
                    tmp.f_iVal4 = 0 '縦扱い
                    tmp.f_dVal1 = dx2
                    tmp.f_dVal2 = dy2
                    tmp.f_d長さ = tmp.f_dVal1 + tmp.f_dVal2
                    If 0 < dShortCorner Then
                        tmp.f_dVal1 = dx2 - dShortCorner
                        tmp.f_d長さ -= dShortCorner
                        If (maxLine - dShortCorner) < tmp.f_d長さ Then
                            tmp.f_d長さ = maxLine - dShortCorner
                        End If
                    End If
                End If

            Else
                '↓下にカウント
                idx = i - nCorner + 1
                x_center = x_centerCorner
                y_center = y_center1 - p_d縦横_四角 * (idx - 1)
                tmp.f_iVal4 = 0 '下へ
                tmp.f_dVal1 = (x_line - x_center) 'dx
                tmp.f_dVal2 = (y_center + y_line) 'dy
                tmp.f_d長さ = tmp.f_dVal1 + tmp.f_dVal2
            End If

            If i = nCorner2 AndAlso 0 < dShortCorner Then
                tmp.f_d長さ -= dShortCorner
            End If
            If maxLine < tmp.f_d長さ Then
                tmp.f_d長さ = maxLine
            End If

            'set
            tmp.f_iVal1 = 1 'ひも数
            tmp.f_iVal2 = i
            tmp.f_iVal3 = idx
            tmp.f_dひも長 = tmp.f_d長さ * ROOT2
            tmp.f_d出力ひも長 = tmp.f_dひも長 + 2 * row.f_dひも長加算

            '保存値
            tmp.f_dVal3 = x_center
            tmp.f_dVal4 = y_center

            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "flg={0} idx={1} dx={2:0.0} dy={3:0.0} 長さ={4:0.0} ひも長={5:0.0} center({6:0.0},{7:0.0})", tmp.f_iVal4, idx, tmp.f_dVal1, tmp.f_dVal2, tmp.f_d長さ, tmp.f_dひも長, tmp.f_dVal3, tmp.f_dVal4)
            tmptable.Rows.Add(tmp)
            If row.f_i何本ごと = 0 Then
                Exit For
            End If
        Next
        If 0 < tmptable.Rows.Count Then
            Return tmptable
        Else
            Return Nothing
        End If
    End Function

    '斜めの差しひものイメージ dInnerPosition:同幅内位置(0～1値・中央が0.5)
    Private Function imageList底の斜め(ByVal row As tbl差しひもRow, ByVal dInnerPosition As Double) As Boolean
        If Not _sasihimo.ContainsKey(row.f_i番号) Then
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

        Dim tmptable As tbl縦横展開DataTable = _sasihimo(row.f_i番号)
        For Each tmp As tbl縦横展開Row In tmptable
            Dim item As New clsImageItem(ImageTypeEnum._ひも領域, New clsDataRow(tmp), row.f_i番号, tmp.f_iひも番号)

            Dim x_center As Double = tmp.f_dVal3
            Dim y_center As Double = tmp.f_dVal4
            Dim dx As Double = tmp.f_dVal1
            Dim dy As Double = tmp.f_dVal2
            Dim sign As Integer = 1

            Dim rBand0 As S領域
            If (0 < tmp.f_iVal4) Then '横へ
                rBand0 = New S領域(New S実座標(x_center - d幅 / 2, y_center + ROOT2 * dy),
                                  New S実座標(x_center + d幅 / 2, y_center - tmp.f_dひも長 + ROOT2 * dy))
            Else
                rBand0 = New S領域(New S実座標(x_center - tmp.f_dひも長 + ROOT2 * dx, y_center + d幅 / 2),
                                  New S実座標(x_center + ROOT2 * dx, y_center - d幅 / 2))
                sign = -1
            End If

            Dim aBand As New S四隅(rBand0)

            'sign = 0 'for debug
            item.m_a四隅 = aBand.Rotate(New S実座標(x_center, y_center), -45 * sign)
            item.p_p文字位置 = New S実座標(x_center, y_center)
            If bRevert Then
                item.m_a四隅 = item.m_a四隅.VertLeft()
                item.p_p文字位置 = item.p_p文字位置.VertLeft()
            End If
            item.m_a四隅 = item.m_a四隅 + delta
            item.p_p文字位置 = item.p_p文字位置 + delta

            _ImageList差しひも.AddItem(item)
        Next

        Return True
    End Function

#End Region
#End Region

#Region "横ひも・縦ひも・4側面"

    '横ひもリストの描画情報 : _tbl縦横展開_横ひも → _ImageList横ひも
    Private Function imageList横ひも() As Boolean
        If _tbl縦横展開_横ひも Is Nothing Then
            Return False
        End If

        _ImageList横ひも = New clsImageItemList(_tbl縦横展開_横ひも)
        If _tbl縦横展開_横ひも.Rows.Count = 0 Then
            Return True
        End If

        'Dim d縁までの固定長 As Double = get周の横() + get側面高(2) + 2 * mdlUnit.Min(_d縁の高さ, _d縁の垂直ひも長)
        Dim Y横ひも上 As Double = p_d四角ベース_縦 / 2

        '上から下へ(位置順)
        _ImageList横ひも.SortByPosition()
        For Each band As clsImageItem In _ImageList横ひも
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            If band.m_row縦横展開.f_iひも種 = enumひも種.i_横 Then
                Dim bandwidth As S差分 = Unit270 * g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)
                'Dim d横ひも表示長 As Double = d縁までの固定長
                Dim d横ひも表示長 As Double = band.m_row縦横展開.f_d出力ひも長

                band.m_rひも位置.p左上 = New S実座標(-d横ひも表示長 / 2, Y横ひも上)
                band.m_rひも位置.p右下 = band.m_rひも位置.p左上 + bandwidth + Unit0 * (d横ひも表示長)
                'band.m_borderひも = DirectionEnum._上 Or DirectionEnum._下
            Else
                '補強ひもは描画しない
                band.m_bNoMark = True
            End If
            '
            Y横ひも上 -= band.m_row縦横展開.f_d幅
        Next

        Return True
    End Function

    '縦ひもリストの描画情報 : _tbl縦横展開_縦ひも → _ImageList縦ひも
    Private Function imageList縦ひも() As Boolean
        If _tbl縦横展開_縦ひも Is Nothing Then
            Return False
        End If

        _ImageList縦ひも = New clsImageItemList(_tbl縦横展開_縦ひも)
        If _tbl縦横展開_縦ひも.Rows.Count = 0 Then
            Return True
        End If

        'Dim d縁までの固定長 As Double = get周の縦() + get側面高(2) + 2 * mdlUnit.Min(_d縁の高さ, _d縁の垂直ひも長)
        Dim X縦ひも左 As Double = -p_d四角ベース_横 / 2

        '左から右へ(位置順)
        _ImageList縦ひも.SortByPosition()
        For Each band As clsImageItem In _ImageList縦ひも
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            If band.m_row縦横展開.f_iひも種 = enumひも種.i_縦 Then
                Dim bandwidth As S差分 = Unit0 * g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)
                'Dim d縦ひも表示長 As Double = d縁までの固定長
                Dim d縦ひも表示長 As Double = band.m_row縦横展開.f_d出力ひも長

                band.m_rひも位置.p左上 = New S実座標(X縦ひも左, d縦ひも表示長 / 2)
                band.m_rひも位置.p右下 = band.m_rひも位置.p左上 + bandwidth + Unit270 * (d縦ひも表示長)
                'band.m_borderひも = DirectionEnum._左 Or DirectionEnum._右
            Else
                band.m_bNoMark = True
            End If

            X縦ひも左 += band.m_row縦横展開.f_d幅
        Next

        Return True
    End Function

    '_imageList側面上・_imageList側面左・_imageList側面下・_imageList側面右生成
    '※側面のレコードはリスト出力時にadjust_側面() 済み
    Function imageList四側面() As Boolean

        '側面のレコードを縦横レコード化
        Dim tmptable As New tbl縦横展開DataTable
        Dim row As tbl縦横展開Row

        Dim d最下の高さ As Double = 0

        Dim idx As Integer = 1
        For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, "f_i番号 ASC , f_iひも番号 ASC")
            If r.f_i番号 = cHemNumber Then
                '縁は編みかたとして処理
                Continue For
            End If
            If r.f_i番号 = cIdxSpace Then
                '最下段のスペースはレコードにしない
                d最下の高さ = r.f_d高さ
                Continue For
            End If
            For i As Integer = 1 To r.f_iひも本数
                row = tmptable.Newtbl縦横展開Row
                row.f_iひも種 = enumひも種.i_側面
                row.f_iひも番号 = idx
                row.f_i位置番号 = i '参考値
                row.f_i何本幅 = r.f_i何本幅
                row.f_s記号 = r.f_s記号
                row.f_s色 = r.f_s色
                row.f_dひも長 = r.f_dひも長
                row.f_dひも長加算 = r.f_dひも長加算
                row.f_dひも長加算2 = 0
                row.f_d出力ひも長 = r.f_d連続ひも長
                If _b縦横側面を展開する Then
                    row.f_d幅 = r.f_d高さ '個別
                Else
                    row.f_d幅 = _d基本のひも幅 + _d目_ひも間のすき間 '合計なので再計算
                End If
                tmptable.Rows.Add(row)

                idx += 1
            Next
        Next
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "tmptable:{0}", New clsGroupDataRow(tmptable).ToString)


        '以降参照するのでここでセットする
        _imageList側面上 = New clsImageItemList(tmptable)
        _imageList側面左 = New clsImageItemList(tmptable)
        _imageList側面下 = New clsImageItemList(tmptable)
        _imageList側面右 = New clsImageItemList(tmptable)

        Dim item As clsImageItem

        Dim p上ひも左下 As New S実座標(-get周の横(1 / 2), d最下の高さ + getZeroY(1 / 2))
        Dim p下ひも左上 As New S実座標(-get周の横(1 / 2), -d最下の高さ - getZeroY(1 / 2))
        Dim p左ひも右上 As New S実座標(-d最下の高さ - getZeroX(1 / 2), get周の縦(1 / 2))
        Dim p右ひも左上 As New S実座標(d最下の高さ + getZeroX(1 / 2), get周の縦(1 / 2))

        '1～_i高さの目の数
        For i As Integer = 1 To p_i側面ひもの本数
            '*上
            item = _imageList側面上.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面上:{0}", i)
            Else
                item.m_ImageType = ImageTypeEnum._横バンド

                item.m_rひも位置.p左下 = p上ひも左下
                item.m_rひも位置.p右上 = p上ひも左下 _
                + Unit90 * g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅) _
                + Unit0 * get周の横()
                item.m_borderひも = DirectionEnum._上 Or DirectionEnum._下

                p上ひも左下 = p上ひも左下 + Unit90 * item.m_row縦横展開.f_d幅
            End If

            '*下
            item = _imageList側面下.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面下:{0}", i)
            Else
                item.m_ImageType = ImageTypeEnum._横バンド

                item.m_rひも位置.p左上 = p下ひも左上
                item.m_rひも位置.p右下 = p下ひも左上 _
                + Unit270 * g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅) _
                + Unit0 * get周の横()
                item.m_borderひも = DirectionEnum._上 Or DirectionEnum._下
                item.m_bNoMark = True '記号なし

                p下ひも左上 = p下ひも左上 + Unit270 * item.m_row縦横展開.f_d幅
            End If

            '*左
            item = _imageList側面左.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面左:{0}", i)
            Else
                item.m_ImageType = ImageTypeEnum._縦バンド

                item.m_rひも位置.p右上 = p左ひも右上
                item.m_rひも位置.p左下 = p左ひも右上 _
                + Unit180 * g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅) _
                + Unit270 * get周の縦()
                item.m_borderひも = DirectionEnum._左 Or DirectionEnum._右
                item.m_bNoMark = True '記号なし

                p左ひも右上 = p左ひも右上 + Unit180 * item.m_row縦横展開.f_d幅
            End If

            '*右
            item = _imageList側面右.GetRowItem(enumひも種.i_側面, i)
            If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "No Record _imageList側面右:{0}", i)
            Else
                item.m_ImageType = ImageTypeEnum._縦バンド

                item.m_rひも位置.p左上 = p右ひも左上
                item.m_rひも位置.p右下 = p右ひも左上 _
                    + Unit0 * g_clsSelectBasics.p_d指定本幅(item.m_row縦横展開.f_i何本幅) _
                    + Unit270 * get周の縦()
                item.m_borderひも = DirectionEnum._左 Or DirectionEnum._右
                item.m_bNoMark = True '記号なし

                p右ひも左上 = p右ひも左上 + Unit0 * item.m_row縦横展開.f_d幅
            End If
        Next

        '縁のレコードをイメージ情報化
        Dim cond As String = String.Format("f_i番号 = {0}", cHemNumber)
        Dim groupRow = New clsGroupDataRow(_Data.p_tbl側面.Select(cond, "f_iひも番号 ASC"), "f_iひも番号")

        Dim d高さ As Double = groupRow.GetNameValueSum("f_d高さ")
        Dim nひも本数 As Integer = groupRow.GetNameValueSum("f_iひも本数")
        If 0 < nひも本数 Then

            '*上
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 1)
            item.m_a四隅.p左下 = p上ひも左下
            item.m_a四隅.p右下 = p上ひも左下 + Unit0 * get周の横()
            item.m_a四隅.p左上 = p上ひも左下 + Unit90 * d高さ
            item.m_a四隅.p右上 = p上ひも左下 + Unit90 * d高さ + Unit0 * get周の横()

            '文字位置
            item.p_p文字位置 = p上ひも左下 + Unit0 * get周の横() + Unit90 * d高さ
            _imageList側面上.AddItem(item)

            '*下
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 2)
            item.m_a四隅.p左上 = p下ひも左上
            item.m_a四隅.p右上 = p下ひも左上 + Unit0 * get周の横()
            item.m_a四隅.p左下 = p下ひも左上 + Unit270 * d高さ
            item.m_a四隅.p右下 = p下ひも左上 + Unit270 * d高さ + Unit0 * get周の横()
            '文字なし
            _imageList側面下.AddItem(item)

            '*左
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 3)
            item.m_a四隅.p右上 = p左ひも右上
            item.m_a四隅.p左上 = p左ひも右上 + Unit180 * d高さ
            item.m_a四隅.p右下 = p左ひも右上 + Unit270 * get周の縦()
            item.m_a四隅.p左下 = p左ひも右上 + Unit180 * d高さ + Unit270 * get周の縦()
            '文字なし
            _imageList側面左.AddItem(item)

            '*右
            item = New clsImageItem(ImageTypeEnum._編みかた, groupRow, 4)
            item.m_a四隅.p左上 = p右ひも左上
            item.m_a四隅.p右上 = p右ひも左上 + Unit0 * d高さ
            item.m_a四隅.p左下 = p右ひも左上 + Unit270 * get周の縦()
            item.m_a四隅.p右下 = p右ひも左上 + Unit0 * d高さ + Unit270 * get周の縦()
            '文字なし
            _imageList側面右.AddItem(item)

        End If

        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面上:{0}", _imageList側面上.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面左:{0}", _imageList側面左.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面下:{0}", _imageList側面下.ToString)
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_imageList側面右:{0}", _imageList側面右.ToString)
        Return True
    End Function

    '底と側面枠
    Function imageList底と側面枠() As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        Dim a底 As S四隅
        a底.p左上 = New S実座標(-p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2)
        a底.p右上 = New S実座標(p_d四角ベース_横 / 2, p_d四角ベース_縦 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        Dim a底の周 As S四隅
        a底の周.p左上 = New S実座標(-get周の横(1 / 2), get周の縦(1 / 2))
        a底の周.p右上 = New S実座標(get周の横(1 / 2), get周の縦(1 / 2))
        a底の周.p左下 = -a底の周.p右上
        a底の周.p右下 = -a底の周.p左上


        '底
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底

        Dim line As S線分
        '右上→左上
        line = New S線分(a底の周.p右上, a底の周.p左上)
        item.m_lineList.Add(line)
        '左上→左下
        line = New S線分(a底の周.p左上, a底の周.p左下)
        item.m_lineList.Add(line)
        '左下→右下
        line = New S線分(a底の周.p左下, a底の周.p右下)
        item.m_lineList.Add(line)
        '右下→右上
        line = New S線分(a底の周.p右下, a底の周.p右上)
        item.m_lineList.Add(line)

        itemlist.AddItem(item)

        Dim d縁厚さプラス_高さ As Double = get側面高(1) + _d縁の高さ
        Dim d底の厚さ As Double = getZeroSide()

        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.p左下 = a底の周.p左上
        item.m_a四隅.p右下 = a底の周.p右上
        item.m_a四隅.p左上 = item.m_a四隅.p左下 + Unit90 * d縁厚さプラス_高さ
        item.m_a四隅.p右上 = item.m_a四隅.p右下 + Unit90 * d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下) + Unit90 * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅.p左上 = a底の周.p右上
        item.m_a四隅.p左下 = a底の周.p右下
        item.m_a四隅.p右上 = item.m_a四隅.p左上 + Unit0 * d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p左下 + Unit0 * d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下) + Unit0 * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        '下の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅.p左上 = a底の周.p左下
        item.m_a四隅.p右上 = a底の周.p右下
        item.m_a四隅.p左下 = item.m_a四隅.p左上 + Unit270 * d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p右上 + Unit270 * d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上) + Unit270 * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        '左の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p右上 = a底の周.p左上
        item.m_a四隅.p右下 = a底の周.p左下
        item.m_a四隅.p左上 = item.m_a四隅.p右上 + Unit180 * d縁厚さプラス_高さ
        item.m_a四隅.p左下 = item.m_a四隅.p右下 + Unit180 * d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下) + Unit180 * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        Return itemlist
    End Function

#End Region

#Region "ひも上下"
    Dim _CUpDown As New clsUpDown   'CheckBoxTableは使わない

    '#52 描画しない色
    Private Function isDrawingItem(ByVal item As clsImageItem) As Boolean
        If item Is Nothing OrElse item.m_row縦横展開 Is Nothing Then
            Return False
        End If
        If g_clsSelectBasics.IsNoDrawingColor(item.m_row縦横展開.f_s色) Then
            Return False
        End If
        Return True
    End Function

    '底の上下をm_regionListにセット
    Private Function regionUpDown底(ByVal isBackFace As Boolean) As Boolean
        If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
            Return False
        End If

        _CUpDown.TargetFace = enumTargetFace.Bottom '底
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If
        Dim revRange As Integer = -1
        If isBackFace Then
            revRange = p_i縦ひもの本数
        End If

        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                If _CUpDown.GetIsDown(iTate, iYoko, revRange) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If isDrawingItem(itemYoko) Then
                        itemTate.m_regionList.Add領域(itemYoko.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                If _CUpDown.GetIsUp(iTate, iYoko, revRange) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If isDrawingItem(itemTate) Then
                        itemYoko.m_regionList.Add領域(itemTate.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        Return True
    End Function


    '側面(上)の上下をm_regionListにセット
    Private Function regionUpDown側面1(ByVal isBackFace As Boolean) As Boolean
        If _ImageList縦ひも Is Nothing OrElse _imageList側面上 Is Nothing Then
            Return False
        End If

        _CUpDown.TargetFace = enumTargetFace.Side12 '側面(上右)
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If
        Dim revRange As Integer = -1
        If isBackFace Then
            revRange = p_i縦ひもの本数
        End If


        Dim horzDif As Integer = 0 '横に上→右
        '*上の側面
        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト

            For iTakasa As Integer = 1 To p_i側面ひもの本数
                If _CUpDown.GetIsDown(iTate + horzDif, iTakasa, revRange) Then
                    Dim itemUSide As clsImageItem = _imageList側面上.GetRowItem(enumひも種.i_側面, iTakasa)
                    If isDrawingItem(itemUSide) Then
                        itemTate.m_regionList.Add領域(itemUSide.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To p_i側面ひもの本数
            Dim itemUSide As clsImageItem = _imageList側面上.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemUSide Is Nothing Then
                Continue For
            End If
            If itemUSide.m_regionList Is Nothing Then itemUSide.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                If _CUpDown.GetIsUp(iTate + horzDif, iTakasa, revRange) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If isDrawingItem(itemTate) Then
                        itemUSide.m_regionList.Add領域(itemTate.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemUSide({0}):{1}", iTakasa, itemUSide.m_regionList.ToString)
        Next


        Return True
    End Function

    '側面(右)の上下をm_regionListにセット
    Private Function regionUpDown側面2(ByVal isBackFace As Boolean) As Boolean
        If _ImageList横ひも Is Nothing OrElse _imageList側面右 Is Nothing Then
            Return False
        End If

        Dim revRange As Integer
        Dim revRange2 As Integer
        If isBackFace Then
            'うら
            _CUpDown.TargetFace = enumTargetFace.Side34 '側面(下左) の左を適用
            revRange = p_i縦ひもの本数
            revRange2 = p_i横ひもの本数
        Else
            'おもて
            _CUpDown.TargetFace = enumTargetFace.Side12 '側面(上右) の右を適用
            revRange = -1
            revRange2 = -1
        End If
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If


        Dim horzDif As Integer = p_i縦ひもの本数 '横に上→右

        '*右の側面
        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            For iTakasa As Integer = 1 To p_i側面ひもの本数
                If _CUpDown.GetIsDown(iYoko + horzDif, iTakasa, revRange, revRange2) Then
                    Dim itemRSide As clsImageItem = _imageList側面右.GetRowItem(enumひも種.i_側面, iTakasa)
                    If isDrawingItem(itemRSide) Then
                        itemYoko.m_regionList.Add領域(itemRSide.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To p_i側面ひもの本数
            Dim itemRSide As clsImageItem = _imageList側面右.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemRSide Is Nothing Then
                Continue For
            End If
            If itemRSide.m_regionList Is Nothing Then itemRSide.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                If _CUpDown.GetIsUp(iYoko + horzDif, iTakasa, revRange, revRange2) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If isDrawingItem(itemYoko) Then
                        itemRSide.m_regionList.Add領域(itemYoko.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemRSide({0}):{1}", iTakasa, itemRSide.m_regionList.ToString)
        Next

        Return True
    End Function

    '側面(下)の上下をm_regionListにセット
    Private Function regionUpDown側面3(ByVal isBackFace As Boolean) As Boolean
        If _ImageList縦ひも Is Nothing OrElse _imageList側面下 Is Nothing Then
            Return False
        End If

        _CUpDown.TargetFace = enumTargetFace.Side34 '側面
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If
        Dim revRange As Integer = -1
        If isBackFace Then
            revRange = p_i縦ひもの本数
        End If

        Dim horzDif As Integer = 0 '横に下→左

        '*下の側面(UpDownは左→右)
        For iTate As Integer = 1 To p_i縦ひもの本数
            Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
            If itemTate Is Nothing Then
                Continue For
            End If
            If itemTate.m_regionList Is Nothing Then itemTate.m_regionList = New C領域リスト
            Dim horzIdx As Integer = p_i縦ひもの本数 - iTate + 1 + horzDif

            For iTakasa As Integer = 1 To p_i側面ひもの本数
                If _CUpDown.GetIsDown(horzIdx, iTakasa, revRange) Then
                    Dim itemDSide As clsImageItem = _imageList側面下.GetRowItem(enumひも種.i_側面, iTakasa)
                    If isDrawingItem(itemDSide) Then
                        itemTate.m_regionList.Add領域(itemDSide.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemTate({0}):{1}", iTate, itemTate.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To p_i側面ひもの本数
            Dim itemDSide As clsImageItem = _imageList側面下.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemDSide Is Nothing Then
                Continue For
            End If
            If itemDSide.m_regionList Is Nothing Then itemDSide.m_regionList = New C領域リスト

            For iTate As Integer = 1 To p_i縦ひもの本数
                Dim horzIdx As Integer = p_i縦ひもの本数 - iTate + 1 + horzDif
                If _CUpDown.GetIsUp(horzIdx, iTakasa, revRange) Then
                    Dim itemTate As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, iTate)
                    If isDrawingItem(itemTate) Then
                        itemDSide.m_regionList.Add領域(itemTate.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemUSide({0}):{1}", iTakasa, itemDSide.m_regionList.ToString)
        Next

        Return True
    End Function

    '側面(左)の上下をm_regionListにセット
    Private Function regionUpDown側面4(ByVal isBackFace As Boolean) As Boolean
        If _ImageList横ひも Is Nothing OrElse _imageList側面左 Is Nothing Then
            Return False
        End If

        Dim revRange As Integer
        Dim revRange2 As Integer
        If isBackFace Then
            'うら
            _CUpDown.TargetFace = enumTargetFace.Side12 '側面(上右) の右を適用
            revRange = p_i縦ひもの本数
            revRange2 = p_i横ひもの本数
        Else
            'おもて
            _CUpDown.TargetFace = enumTargetFace.Side34 '側面(下左) の左を適用
            revRange = -1
            revRange2 = -1
        End If
        If Not _Data.ToClsUpDown(_CUpDown) Then
            _CUpDown.Reset(p_i垂直ひも半数)
        End If
        If Not _CUpDown.IsValid(False) Then 'チェックはMatrix
            Return False
        End If

        Dim horzDif As Integer = p_i縦ひもの本数 '横に下→左

        '*左の側面(UpDownは下→上)
        For iYoko As Integer = 1 To p_i横ひもの本数
            Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
            If itemYoko Is Nothing Then
                Continue For
            End If
            If itemYoko.m_regionList Is Nothing Then itemYoko.m_regionList = New C領域リスト

            Dim horzIdx As Integer = p_i横ひもの本数 - iYoko + 1 + horzDif
            For iTakasa As Integer = 1 To p_i側面ひもの本数
                If _CUpDown.GetIsDown(horzIdx, iTakasa, revRange, revRange2) Then
                    Dim itemLSide As clsImageItem = _imageList側面左.GetRowItem(enumひも種.i_側面, iTakasa)
                    If isDrawingItem(itemLSide) Then
                        itemYoko.m_regionList.Add領域(itemLSide.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemYoko({0}):{1}", iYoko, itemYoko.m_regionList.ToString)
        Next

        For iTakasa As Integer = 1 To p_i側面ひもの本数
            Dim itemLSide As clsImageItem = _imageList側面左.GetRowItem(enumひも種.i_側面, iTakasa)
            If itemLSide Is Nothing Then
                Continue For
            End If
            If itemLSide.m_regionList Is Nothing Then itemLSide.m_regionList = New C領域リスト

            For iYoko As Integer = 1 To p_i横ひもの本数
                Dim horzIdx As Integer = p_i横ひもの本数 - iYoko + 1 + horzDif
                If _CUpDown.GetIsUp(horzIdx, iTakasa, revRange, revRange2) Then
                    Dim itemYoko As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, iYoko)
                    If isDrawingItem(itemYoko) Then
                        itemLSide.m_regionList.Add領域(itemYoko.m_rひも位置)
                    End If
                End If
            Next
            'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "itemRSide({0}):{1}", iTakasa, itemLSide.m_regionList.ToString)
        Next

        Return True
    End Function

#End Region

End Class
