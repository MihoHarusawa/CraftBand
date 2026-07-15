Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsSquare45Bottom
Imports CraftBand.Tables.dstDataTables

Partial Public Class clsCalcKnot

    '底展開状態の全コマ位置
    Dim _KnotFolderSpace As New CKnotFolder.CKnotFolderSpace

    'コマ空間 (コマ単位の整数グリッド)
    '・ →
    '↓   1    2    3   → HorizontalCount           　　　　   
    '   +----+----+----+ ...               　　　　　　
    ' 1 |    |    |    | ...       中心からの位置　│　　　　　
    '   +----+----+----+  ...     1/2があるので小数│　　　　　
    ' 2 |    |    |    | ...               　　　　│
    '   +----+----+----+ ...               　　　　│原点はグリッドの中心　　　　　
    ' 3 |    |    |    | ...               ────・────→ coorBaseX
    '   +----+----+----+ ...               　　　　│　　　　　
    ' ↓  ....　　　　　　　　 　　　　　　　　　　↓　      toPoint関数で座標に変換
    'VerticalCount　                             coorBaseY     (_dコマベース寸法を掛ける)


    'Enum emExp を_Yoko,_Tate のみで使用する
    Const cExpYTCount As Integer = 2 '縦横のみ
    Shared Function next_expi(ByVal i As Integer) As Integer
        Dim ne As Integer = i + 1
        If ne = cExpYTCount Then
            Return 0
        End If
        Return ne
    End Function
    Shared Function next_exp(ByVal yt As emExp) As emExp
        If yt = emExp._Yoko Then
            Return emExp._Tate
        Else
            Return emExp._Yoko
        End If
    End Function



#Region "計算用プロパティ"
    ReadOnly Property p_iコマ空間幅 As Integer
        Get
            If _b斜め立ち上げ Then
                Return p_iひもの本数 + 2 * p_i側面の切捨コマ数
            Else
                Return _i横のコマ数 + 2 * p_i編みひもの本数
            End If
        End Get
    End Property

    ReadOnly Property p_iコマ空間高さ As Integer
        Get
            If _b斜め立ち上げ Then
                Return p_iひもの本数 + 2 * p_i側面の切捨コマ数
            Else
                Return _i縦のコマ数 + 2 * p_i編みひもの本数
            End If
        End Get
    End Property

#End Region


#Region "コマ空間のクラス"

    '位置ならびに差分
    Structure SPosition
        Public HorzIndex As Integer
        Public VertIndex As Integer
        Sub New(ByVal h As Integer, ByVal v As Integer)
            HorzIndex = h
            VertIndex = v
        End Sub
        Sub New(ByVal ref As SPosition)
            HorzIndex = ref.HorzIndex
            VertIndex = ref.VertIndex
        End Sub

        '方向を示す差分として使う
        Sub New(ByVal base As SPosition, ByVal way As SPosition)
            HorzIndex = (way.HorzIndex - base.HorzIndex)
            VertIndex = (way.VertIndex - base.VertIndex)
            ToOne()
        End Sub

        Function ToOne() As SPosition
            If 0 < HorzIndex Then
                HorzIndex = 1
            ElseIf HorzIndex < 0 Then
                HorzIndex = -1
            End If

            If 0 < VertIndex Then
                VertIndex = 1
            ElseIf VertIndex < 0 Then
                VertIndex = -1
            End If
            Return Me
        End Function

        '未セット(1～の値で使う前提なので)
        ReadOnly Property IsZero() As Boolean
            Get
                Return HorzIndex = 0 AndAlso VertIndex = 0
            End Get
        End Property

        '有効な値(上限も要チェックだが..)
        ReadOnly Property IsValid() As Boolean
            Get
                Return 0 < HorzIndex AndAlso 0 < VertIndex
            End Get
        End Property

        '比較演算子 =
        Shared Operator =(ByVal c1 As SPosition, ByVal c2 As SPosition) As Boolean
            Return (c1.HorzIndex = c2.HorzIndex) AndAlso (c1.VertIndex = c2.VertIndex)
        End Operator

        '比較演算子 <>
        Shared Operator <>(ByVal c1 As SPosition, ByVal c2 As SPosition) As Boolean
            Return Not (c1 = c2)
        End Operator

        '比較演算子 <= ※片側は同一値を使う前提
        Shared Operator <=(ByVal p1 As SPosition, ByVal p2 As SPosition) As Boolean
            Return (p1.HorzIndex <= p2.HorzIndex) AndAlso (p1.VertIndex <= p2.VertIndex)
        End Operator

        '比較演算子 >= ※片側は同一値を使う前提
        Shared Operator >=(ByVal p1 As SPosition, ByVal p2 As SPosition) As Boolean
            Return (p1.HorzIndex >= p2.HorzIndex) AndAlso (p1.VertIndex >= p2.VertIndex)
        End Operator

        '二項+演算子 
        Shared Operator +(ByVal c1 As SPosition, ByVal delta As SPosition) As SPosition
            Return New SPosition(c1.HorzIndex + delta.HorzIndex, c1.VertIndex + delta.VertIndex)
        End Operator

        '二項-演算子 
        Shared Operator -(ByVal c1 As SPosition, ByVal delta As SPosition) As SPosition
            Return New SPosition(c1.HorzIndex - delta.HorzIndex, c1.VertIndex - delta.VertIndex)
        End Operator


        Overrides Function ToString() As String
            Return String.Format("({0},{1})", HorzIndex, VertIndex)
        End Function

    End Structure



    'コマ1点の情報＆コマ空間
    Public Class CKnotFolder
        Private m_spaceParent As CKnotFolderSpace

        '空間内の位置
        Friend m_position As SPosition

        '横バンド・縦バンド
        Friend m_row縦横展開(cExpYTCount - 1) As tbl縦横展開Row

        Friend m_bottom_category As bottom_category = bottom_category._nodef

        Friend m_knot As CKnot = Nothing
        Friend m_knotSide As DirectionEnum = cDirectionEnumNone
        Dim m_positionSameKnot As SPosition '同一コマの相手方

        Friend Property IsAddBottomBase As Boolean = False '底編み領域

        Friend Property IsBottomBase As Boolean = False '底編み領域

        '中心から横方向の位置・コマの左
        Friend ReadOnly Property coorBaseX As Double
            Get
                Return m_spaceParent.coorBaseX(m_position.HorzIndex)
            End Get
        End Property
        '中心から縦方向反転位置・コマの上
        Friend ReadOnly Property coorBaseY As Double
            Get
                Return m_spaceParent.coorBaseY(m_position.VertIndex)
            End Get
        End Property

        '中心からの位置・コマの左上
        Friend ReadOnly Property coorBaseXY As Double()
            Get
                Return m_spaceParent.coorBaseXY(m_position)
            End Get
        End Property


        Friend Sub New(ByVal parent As CKnotFolderSpace, ByVal horz As Integer, ByVal vert As Integer)
            m_spaceParent = parent
            m_position.HorzIndex = horz
            m_position.VertIndex = vert
        End Sub

        'コマを編むひものセット数
        Function BandSetCount() As Integer
            Dim cnt As Integer = 0
            For i As Integer = 0 To cExpYTCount - 1
                If m_row縦横展開(i) IsNot Nothing Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        '底に位置する
        Function IsInBottom() As Boolean
            If {bottom_category._bottom, bottom_category._bottom_line, bottom_category._center_line}.Contains(m_bottom_category) Then
                Return True
            End If
            Return False
        End Function

        '側面に位置する ※側面内に収まるコマのみを描く
        Function IsInSide() As Boolean
            '半分になる縁のコマは描かない :_side135_edge, _side45R_edge,
            If {bottom_category._side135, bottom_category._side45R,
                bottom_category._side_line_front, bottom_category._side_line_sideR
            }.Contains(m_bottom_category) Then
                Return True
            End If
            Return False
        End Function

        Function IsInPlate() As Boolean
            If IsInBottom() Then
                Return True
            End If
            If IsInSide() Then
                Return True
            End If
            Return False
        End Function

        Function SameKnotFolder() As CKnotFolder
            If m_positionSameKnot.IsZero Then
                Return Nothing
            End If
            Return m_spaceParent.GetAt(m_positionSameKnot)
        End Function



        Private Function bottom_category_mark() As String
            Select Case m_bottom_category
                Case bottom_category._bottom '底の中[※]
                    Return ("■")
                Case bottom_category._bottom_line '境界線[※]
                    Return ("◆")
                Case bottom_category._center_line '中央線
                    Return ("×")

                Case bottom_category._side135 '側面135[※]
                    Return ("□")
                Case bottom_category._side45R '側面45[※]
                    Return ("□")
                Case bottom_category._noexist   '存在しない
                    Return ("　")
                Case bottom_category._side_line_front  '側面の辺・正面と背面側
                    Return ("◇")
                Case bottom_category._side_line_sideR  '側面の辺・側面側
                    Return ("◆")
                Case bottom_category._height_over45R '高さの外45
                    Return ("　")
                Case bottom_category._height_over '高さの外
                    Return ("　")

                Case bottom_category._side135_edge '側面135の縁[※]
                    Return ("◇")
                Case bottom_category._side45R_edge '側面45の縁[※]
                    Return ("◆")
                Case Else 'bottom_category._nodef
                    Return ("　")
            End Select
        End Function

        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0}:{1}", m_position.ToString, m_positionSameKnot)
            For i As Integer = 0 To cExpYTCount - 1
                If m_row縦横展開(i) IsNot Nothing Then
                    With m_row縦横展開(i)
                        Dim iひも種 As Integer = .f_iひも種
                        sb.AppendFormat(" m_row縦横展開:({0} {1} {2}){3}{4} {5:F1} /", DirectCast(i, emExp), DirectCast(iひも種, enumひも種), .f_iひも番号, .f_s記号, .f_s色, .f_d出力ひも長)
                    End With
                Else
                    sb.Append("                        /")
                End If
            Next
            sb.AppendFormat("{0} {1} IsBottomBase={2}", bottom_category_mark(), m_bottom_category.ToString, IsBottomBase)
            If m_knot IsNot Nothing Then
                sb.AppendLine(m_knot.ToString)
            End If
            Return sb.ToString
        End Function


        'コマが配置される領域全体
        Public Class CKnotFolderSpace
            Implements IEnumerable(Of CKnotFolder)

            ' 内部データ
            Dim _folders(,) As CKnotFolder
            Dim _isDiagonal As Boolean

            Public ReadOnly Property IsValid As Boolean = False
            '領域サイズ
            Public ReadOnly Property HorizontalCount As Integer  ' 
            Public ReadOnly Property VerticalCount As Integer '
            '
            Public Property StartPosition As SPosition


            Public Sub New()
            End Sub

            Public Sub InValidate()
                _IsValid = False
            End Sub

            'サイズ再設定と初期化
            Public Sub Reinitialize(ByVal width As Integer, ByVal height As Integer, ByVal isdiag As Boolean)
                If width < 1 OrElse width < 1 Then
                    _folders = Nothing
                    _IsValid = False
                End If

                _HorizontalCount = width
                _VerticalCount = height
                _isDiagonal = isdiag

                '内部の2次元配列は (行=縦, 列=横) で確保する
                _folders = New CKnotFolder(VerticalCount - 1, HorizontalCount - 1) {}

                For y As Integer = 0 To VerticalCount - 1
                    For x As Integer = 0 To HorizontalCount - 1
                        _folders(y, x) = New CKnotFolder(Me, x + 1, y + 1)
                    Next
                Next
                _IsValid = True
            End Sub

            'horz=1～HorizontalCount, vert=1～VerticalCount
            Public ReadOnly Property GetAt(ByVal horz As Integer, ByVal vert As Integer) As CKnotFolder
                Get
                    If Not IsValid OrElse horz < 1 OrElse HorizontalCount < horz OrElse vert < 1 OrElse VerticalCount < vert Then
                        Return Nothing
                    End If

                    ' 内部の配列アクセス時は (行=Y, 列=X) に変換してアクセスする
                    Return _folders(vert - 1, horz - 1)
                End Get
            End Property

            Public ReadOnly Property GetAt(ByVal position As SPosition) As CKnotFolder
                Get
                    Return GetAt(position.HorzIndex, position.VertIndex)
                End Get
            End Property

            '斜め配置の立ち上げ面
            Public Function SetDiagonalMapping(ByVal map As clsSquare45Bottom) As Boolean
                If map Is Nothing Then
                    Return False
                End If

                '各コマの位置属性
                For x As Integer = 1 To HorizontalCount
                    For y As Integer = 1 To VerticalCount
                        Dim knotfolder As CKnotFolder = GetAt(x, y)
                        knotfolder.m_bottom_category = map.CheckIsInBottom(x, y)
                        If {bottom_category._nodef}.Contains(knotfolder.m_bottom_category) Then
                            Return False 'ジャストサイズのはず
                        End If
                    Next
                Next

                '側面の同一箇所
                Dim samecount As Integer = 0
                Dim samePointPairList As CSamePointPairList = map.GetSamePointPairList()
                For Each dd As SSamePointPair In samePointPairList
                    Dim knotfolder As CKnotFolder
                    knotfolder = GetAt(dd.horzIdx, dd.vertIdx)
                    If knotfolder IsNot Nothing Then
                        knotfolder.m_positionSameKnot.HorzIndex = dd.horzIdxRef
                        knotfolder.m_positionSameKnot.VertIndex = dd.vertIdxRef
                        samecount += 1
                    End If
                    knotfolder = GetAt(dd.horzIdxRef, dd.vertIdxRef)
                    If knotfolder IsNot Nothing Then
                        knotfolder.m_positionSameKnot.HorzIndex = dd.horzIdx
                        knotfolder.m_positionSameKnot.VertIndex = dd.vertIdx
                        samecount += 1
                    End If
                Next
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "List Count={0}  SameCount={1}", samePointPairList.Count, samecount)

                Return True
            End Function

            '底のバンドを側面に延長　※側面に既に入っている分は上書きします
            Private Function BottomExtendToSideLine(ByVal pos_from As SPosition, ByVal pos_to As SPosition, ByVal pos_step As SPosition, ByVal expYT As emExp) As Boolean
                Dim knotline1 As CKnotFolder = Nothing
                Dim knotline2 As CKnotFolder = Nothing
                Dim knotfolder As CKnotFolder

                '底の境界点2点を取得
                For pos As SPosition = pos_from To pos_to Step pos_step
                    knotfolder = GetAt(pos)
                    If knotfolder Is Nothing Then Continue For

                    If knotfolder.m_bottom_category = bottom_category._bottom_line Then
                        If knotline1 Is Nothing Then
                            knotline1 = knotfolder
                        ElseIf knotline2 Is Nothing Then
                            knotline2 = knotfolder
                            Exit For
                        End If
                    End If
                Next
                If knotline2 Is Nothing Then
                    Return True '底を通らない
                End If
                '現バンド
                Dim row As tbl縦横展開Row = knotline1.m_row縦横展開(expYT)
                '進行方向
                Dim delta As New SPosition(knotline1.m_position, knotline2.m_position)
                If delta.IsZero Then
                    '差があるはずだけど、なければ処理不可
                    Return False
                End If
                '2つ目の境界点から開始
                Dim position As New SPosition(knotline2.m_position)
                '進行方向にひとつ進める
                position += delta
                knotfolder = GetAt(position)
                Do While knotfolder IsNot Nothing AndAlso knotfolder.IsInSide
                    knotfolder.m_row縦横展開(expYT) = row
                    Dim pair As CKnotFolder = knotfolder.SameKnotFolder()
                    If pair IsNot Nothing Then
                        expYT = next_exp(expYT)
                        pair.m_row縦横展開(expYT) = row
                        knotline1 = knotfolder
                        knotline2 = pair
                        delta = New SPosition(knotline1.m_position, knotline2.m_position)
                        If delta.IsZero Then
                            '差があるはずだけど、なければ処理不可
                            Return False
                        End If
                        position = pair.m_position
                    End If

                    position += delta
                    knotfolder = GetAt(position)
                Loop

                Return True
            End Function

            Public Function BottomExtendToSides() As Boolean
                Dim ret = True
                For vidx As Integer = 1 To VerticalCount
                    ret = ret And BottomExtendToSideLine(New SPosition(1, vidx), New SPosition(HorizontalCount, vidx), New SPosition(1, 0), emExp._Yoko)
                    ret = ret And BottomExtendToSideLine(New SPosition(HorizontalCount, vidx), New SPosition(1, vidx), New SPosition(-1, 0), emExp._Yoko)
                Next
                For hidx As Integer = 1 To HorizontalCount
                    ret = ret And BottomExtendToSideLine(New SPosition(hidx, 1), New SPosition(hidx, VerticalCount), New SPosition(0, 1), emExp._Tate)
                    ret = ret And BottomExtendToSideLine(New SPosition(hidx, VerticalCount), New SPosition(hidx, 1), New SPosition(0, -1), emExp._Tate)
                Next
                Return ret
            End Function

            '開始位置のコマ位置情報
            Public Function GetStartKnot() As CKnotFolder
                If Not IsValid OrElse Not StartPosition.IsValid Then
                    Return Nothing
                End If
                Dim knotStart As CKnotFolder = GetAt(StartPosition)
                If knotStart.IsBottomBase Then
                    Return knotStart
                End If
                Return Nothing
            End Function

            '指定位置から外側のコマ数, 上,下,左,右の順
            Public Function GetStartKnotCountUDLR() As Integer()
                If Not IsValid OrElse Not StartPosition.IsValid Then
                    Return Nothing
                End If
                Dim position As SPosition = GetAt(StartPosition).m_position
                Dim knots(cSideCount - 1) As Integer
                If _isDiagonal Then
                    '折り返しつつのびる
                    knots(SideEnum._上) = GetKomaCount(position, New SPosition(0, -1)) 'U
                    knots(SideEnum._下) = GetKomaCount(position, New SPosition(0, 1)) 'D
                    knots(SideEnum._左) = GetKomaCount(position, New SPosition(-1, 0)) 'L
                    knots(SideEnum._右) = GetKomaCount(position, New SPosition(1, 0)) 'R
                Else
                    '縦横にのびる
                    knots(SideEnum._上) = position.VertIndex - 1
                    knots(SideEnum._下) = VerticalCount - position.VertIndex
                    knots(SideEnum._左) = position.HorzIndex - 1
                    knots(SideEnum._右) = HorizontalCount - position.HorzIndex
                End If

                Return knots
            End Function

            '指定方向、指定位置から外側のコマ数
            Private Function GetKomaCount(ByVal pos_center As SPosition, ByVal delta As SPosition) As Integer
                Dim knotfolder As CKnotFolder = GetAt(pos_center)
                If knotfolder Is Nothing OrElse Not knotfolder.IsBottomBase OrElse delta.IsZero Then
                    Return 0
                End If

                Dim count As Integer = 0
                '開始位置
                Dim position As New SPosition(pos_center)
                '進行方向にひとつ進める
                position += delta
                knotfolder = GetAt(position)
                Do While knotfolder IsNot Nothing AndAlso knotfolder.IsInPlate
                    count += 1
                    Dim pair As CKnotFolder = knotfolder.SameKnotFolder()
                    If pair IsNot Nothing Then
                        Dim k1 As CKnotFolder = knotfolder
                        Dim k2 As CKnotFolder = pair
                        delta = New SPosition(k1.m_position, k2.m_position)
                        If delta.IsZero Then
                            '差があるはずだけど、なければ終わり
                            Return count
                        End If
                        position = pair.m_position
                    End If

                    position += delta
                    knotfolder = GetAt(position)
                Loop

                Return count
            End Function

            '中心から横方向の位置・コマの左
            Friend Function coorBaseX(ByVal hidx As Double) As Double
                Return (hidx - 1) - (HorizontalCount / 2)
            End Function
            '中心から縦方向反転位置・コマの上
            Friend Function coorBaseY(ByVal vidx As Double) As Double
                Return (vidx - 1) - (VerticalCount / 2)
            End Function
            Friend Function coorBaseXY(ByVal hidx As Double, ByVal vidx As Double) As Double()
                Dim xy(1) As Double
                xy(0) = coorBaseX(hidx)
                xy(1) = coorBaseY(vidx)
                Return xy
            End Function
            Friend Function coorBaseXY(ByVal position As SPosition) As Double()
                Return coorBaseXY(position.HorzIndex, position.VertIndex)
            End Function


            Public Overrides Function ToString() As String
                Dim sb As New System.Text.StringBuilder
                sb.AppendFormat("CKnotFolderSpace({0}) HorizontalCount={1} VerticalCount={2} StartPosition={3}", IsValid, HorizontalCount, VerticalCount, StartPosition).AppendLine()
                For x As Integer = 1 To HorizontalCount
                    For y As Integer = 1 To VerticalCount
                        Dim knot As CKnotFolder = GetAt(x, y)
                        sb.AppendLine(knot.ToString)
                    Next
                    sb.AppendLine()
                Next
                Return sb.ToString
            End Function

            Function dump_knot_band() As String
                Dim sb As New System.Text.StringBuilder
                sb.Append("   ")
                For x As Integer = 1 To HorizontalCount
                    sb.AppendFormat("{0:00} ", x)
                Next
                sb.AppendLine()
                For y As Integer = 1 To VerticalCount
                    sb.AppendFormat("{0:00}|", y)
                    For x As Integer = 1 To HorizontalCount
                        Dim knot As CKnotFolder = GetAt(x, y)
                        If knot.BandSetCount() = 2 Then
                            If StartPosition = knot.m_position Then
                                sb.Append("■")
                            Else
                                'sb.Append("田")
                                sb.Append("╋")
                            End If
                        ElseIf knot.BandSetCount() = 1 Then
                            If knot.m_row縦横展開(emExp._Yoko) IsNot Nothing Then
                                sb.Append("━")
                            Else
                                sb.Append("┃")
                            End If
                        Else
                            sb.Append("　")
                        End If
                        sb.Append("|")
                    Next
                    sb.AppendLine()
                Next
                Return sb.ToString
            End Function

            Function dump_bottom_category() As String
                Dim sb As New System.Text.StringBuilder
                sb.Append("   ")
                For x As Integer = 1 To HorizontalCount
                    sb.AppendFormat("{0:00} ", x)
                Next
                sb.AppendLine()
                For y As Integer = 1 To VerticalCount
                    sb.AppendFormat("{0:00}|", y)
                    For x As Integer = 1 To HorizontalCount
                        Dim knot As CKnotFolder = GetAt(x, y)
                        sb.AppendFormat("{0}|", knot.bottom_category_mark)
                    Next
                    sb.AppendLine()
                Next
                Return sb.ToString
            End Function

            Public Iterator Function GetEnumerator() As IEnumerator(Of CKnotFolder) Implements IEnumerable(Of CKnotFolder).GetEnumerator
                ' 空間内のすべての要素を順次返す (スキャンライン順：左上から右下へ)
                For y As Integer = 0 To VerticalCount - 1
                    For x As Integer = 0 To HorizontalCount - 1
                        Yield _folders(y, x)
                    Next
                Next
            End Function

            Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return GetEnumerator()
            End Function
        End Class
    End Class

#End Region


    '四角数,入力値(ひも長加算,ひも幅)がFixした状態で、コマ配置をセットする
    Private Function calc_コマ配置計算() As Boolean
        Dim ret As Boolean = True

        'CalcOutput により、以下には記号がセットされています
        '_tbl縦横展開(emExp._Yoko), _tbl縦横展開(emExp._Tate)
        '_Data.p_tbl側面→_tbl縦横展開(emExp._Side)
        tbl側面to縦横展開DataTable()

        'コマ配置
        If _b斜め立ち上げ Then
            ret = SetTablesDiagonal()
        Else
            ret = SetTables()
        End If

        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}", _KnotFolderSpace.dump_knot_band)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_KnotFolderSpace={0}", _KnotFolderSpace.ToString)

        Return ret
    End Function

    Private Function tbl側面to縦横展開DataTable() As tbl縦横展開DataTable
        '側面のレコードを縦横レコード化
        Dim tmptable As tbl縦横展開DataTable = _tbl縦横展開(emExp._Side)
        tmptable.Clear()

        Dim row As tbl縦横展開Row

        Dim idx As Integer = 1
        For Each r As tbl側面Row In _Data.p_tbl側面.Select(Nothing, "f_i番号 ASC , f_iひも番号 ASC")
            If r.f_i番号 = cHemNumber Then
                Continue For
            End If
            For i As Integer = 1 To r.f_iひも本数
                row = tmptable.Newtbl縦横展開Row
                row.f_iひも種 = enumひも種.i_側面
                row.f_iひも番号 = idx
                row.f_i何本幅 = r.f_i何本幅
                row.f_s記号 = r.f_s記号
                row.f_s色 = r.f_s色
                row.f_dひも長 = r.f_dひも長
                row.f_dひも長加算 = r.f_dひも長加算
                row.f_dひも長加算2 = 0
                row.f_d出力ひも長 = r.f_d連続ひも長
                tmptable.Rows.Add(row)

                idx += 1
            Next
        Next
        Return tmptable
    End Function


    '       emExp._Tate
    '       +------+   emExp._Side
    '       | 前面 | ↓↓
    '  +----+------+----+ 
    '  |    |      |    | emExp._Yoko
    '  +----+------+----+ 
    '       | 背面 |  ←
    '       +------+  ← emExp._Side

    'テーブルのレコードを縦横にセットする
    Function SetTables() As Boolean
        Dim ret As Boolean = True

        '再初期化
        _KnotFolderSpace.Reinitialize(p_iコマ空間幅, p_iコマ空間高さ, False)

        Dim _i左から何番目 As Integer = _Data.p_row底_縦横.Value("f_i左から何番目")
        Dim _i上から何番目 As Integer = _Data.p_row底_縦横.Value("f_i上から何番目")
        _KnotFolderSpace.StartPosition = New SPosition(p_i編みひもの本数 + _i左から何番目, p_i編みひもの本数 + _i上から何番目)

        For i As Integer = 0 To cExpYTSCount - 1
            Dim bcount As Integer
            Dim setcount As Integer = 0

            Select Case DirectCast(i, emExp)
                Case emExp._Yoko
                    bcount = p_i横ひもの本数'縦にコマが並ぶ
                Case emExp._Tate
                    bcount = p_i縦ひもの本数'横にコマが並ぶ
                Case emExp._Side
                    bcount = p_i編みひもの本数
            End Select

            For Each row As tbl縦横展開Row In _tbl縦横展開(i).Select(Nothing, "f_iひも番号 ASC")
                Dim idx As Integer = row.f_iひも番号
                If idx < 1 OrElse bcount < idx Then
                    Return False
                End If

                Dim knotfolder As CKnotFolder
                Select Case DirectCast(i, emExp)
                    Case emExp._Yoko
                        '横ひも
                        For hidx As Integer = 1 To p_iコマ空間幅
                            knotfolder = _KnotFolderSpace.GetAt(hidx, p_i編みひもの本数 + idx)
                            If knotfolder IsNot Nothing Then
                                knotfolder.m_row縦横展開(emExp._Yoko) = row
                                knotfolder.IsBottomBase = True
                                If hidx = 1 Then
                                    knotfolder.m_knotSide = DirectionEnum._左
                                End If
                                If idx = 1 OrElse idx = bcount Then
                                    knotfolder.m_bottom_category = bottom_category._bottom_line
                                ElseIf 1 < idx AndAlso idx < bcount Then
                                    If knotfolder.m_bottom_category = bottom_category._nodef Then
                                        knotfolder.m_bottom_category = bottom_category._bottom
                                    End If
                                End If
                            End If
                        Next
                        setcount += 1

                    Case emExp._Tate
                        '縦ひも
                        For vidx As Integer = 1 To p_iコマ空間高さ
                            knotfolder = _KnotFolderSpace.GetAt(p_i編みひもの本数 + idx, vidx)
                            If knotfolder IsNot Nothing Then
                                knotfolder.m_row縦横展開(emExp._Tate) = row
                                knotfolder.IsBottomBase = True
                                If vidx = 1 Then
                                    knotfolder.m_knotSide = DirectionEnum._上
                                End If
                                If idx = 1 OrElse idx = bcount Then
                                    knotfolder.m_bottom_category = bottom_category._bottom_line
                                ElseIf 1 < idx AndAlso idx < bcount Then
                                    If knotfolder.m_bottom_category = bottom_category._nodef Then
                                        knotfolder.m_bottom_category = bottom_category._bottom
                                    End If
                                End If
                            End If
                        Next
                        setcount += 1

                    Case emExp._Side
                        '上側面・下側面
                        For hidx As Integer = 1 To p_i縦ひもの本数
                            knotfolder = _KnotFolderSpace.GetAt(hidx + p_i編みひもの本数, p_i編みひもの本数 - idx + 1)
                            If knotfolder IsNot Nothing Then
                                knotfolder.m_row縦横展開(emExp._Yoko) = row
                                knotfolder.m_bottom_category = bottom_category._side45R
                                knotfolder.IsBottomBase = False
                            End If
                            knotfolder = _KnotFolderSpace.GetAt(hidx + p_i編みひもの本数, p_i編みひもの本数 + p_i横ひもの本数 + idx)
                            If knotfolder IsNot Nothing Then
                                knotfolder.m_row縦横展開(emExp._Yoko) = row
                                knotfolder.m_bottom_category = bottom_category._side45R
                                knotfolder.IsBottomBase = False
                                If hidx = 1 Then
                                    knotfolder.m_knotSide = DirectionEnum._左
                                End If
                            End If
                        Next
                        '左側面・右側面
                        For vidx As Integer = 1 To p_i横ひもの本数
                            knotfolder = _KnotFolderSpace.GetAt(p_i編みひもの本数 - idx + 1, p_i編みひもの本数 + vidx)
                            If knotfolder IsNot Nothing Then
                                knotfolder.m_row縦横展開(emExp._Tate) = row
                                knotfolder.m_bottom_category = bottom_category._side135
                                knotfolder.IsBottomBase = False
                            End If
                            knotfolder = _KnotFolderSpace.GetAt(p_i編みひもの本数 + p_i縦ひもの本数 + idx, p_i編みひもの本数 + vidx)
                            If knotfolder IsNot Nothing Then
                                knotfolder.m_row縦横展開(emExp._Tate) = row
                                knotfolder.m_bottom_category = bottom_category._side135
                                knotfolder.IsBottomBase = False
                            End If
                        Next

                        setcount += 1
                End Select
            Next
            If setcount <> bcount Then
                ret = False 'ないはずだが
            End If
        Next
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}", _KnotFolderSpace.dump_bottom_category)

        Return ret
    End Function


    '        ／＼     ／＼         
    '      ／前面＼ ／右面＼  
    '    ／       ／＼      ＼
    '    ＼-──／──＼-─→／　同一点のコマにワープ
    '   ↓: ＼／←     →＼／:↓
    '   ↓: ／＼         ／＼:↓
    '     ／     ＼    ／    |＼　方向を変えて進行
    '     ＼       ＼／      |／
    '       ＼    ／ ＼    ／ 面の外で終わる
    '         ＼／     ＼／

    'テーブルのレコードを斜めにセットする
    Function SetTablesDiagonal() As Boolean
        Dim ret As Boolean = True

        _KnotFolderSpace.Reinitialize(p_iコマ空間幅, p_iコマ空間高さ, True)

        '底編み領域
        For vidx As Integer = 1 To p_iひもの本数
            For hidx As Integer = 1 To p_iひもの本数
                Dim knotfolder As CKnotFolder = _KnotFolderSpace.GetAt(hidx + p_i側面の切捨コマ数, vidx + p_i側面の切捨コマ数)
                knotfolder.IsBottomBase = True
            Next
        Next

        '展開状態の面
        Dim map As New clsSquare45Bottom(_i横のコマ数, _i縦のコマ数, p_i側面の切捨コマ数, IIf(p_b側面半コマ, 1, 0))
        If Not map.SetJustSize Then
            Return False
        End If
        If Not _KnotFolderSpace.SetDiagonalMapping(map) Then
            Return False
        End If


        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "({0},{1},{2}){3}", _i横のコマ数, _i縦のコマ数, p_i側面の切捨コマ数, p_b側面半コマ)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}", _KnotFolderSpace.dump_bottom_category)


        Dim _i左から何番目 As Integer = _Data.p_row底_縦横.Value("f_i左から何番目")
        Dim _i上から何番目 As Integer = _Data.p_row底_縦横.Value("f_i上から何番目")
        _KnotFolderSpace.StartPosition = New SPosition(p_i側面の切捨コマ数 + _i左から何番目, p_i側面の切捨コマ数 + _i上から何番目)

        '横ひも
        For Each row As tbl縦横展開Row In _tbl縦横展開(emExp._Yoko).Select(Nothing, "f_iひも番号 ASC")
            Dim idx As Integer = row.f_iひも番号
            If idx < 1 OrElse p_iひもの本数 < idx Then
                Return False
            End If

            Dim vidx As Integer = idx + p_i側面の切捨コマ数
            Dim knotfolder As CKnotFolder
            '横方向のコマ
            For hidx As Integer = 1 To p_iコマ空間幅
                knotfolder = _KnotFolderSpace.GetAt(hidx, vidx)
                If knotfolder IsNot Nothing Then
                    If knotfolder.IsInPlate() Then
                        knotfolder.m_row縦横展開(emExp._Yoko) = row
                    End If
                End If
            Next
        Next

        '縦ひも
        For Each row As tbl縦横展開Row In _tbl縦横展開(emExp._Tate).Select(Nothing, "f_iひも番号 ASC")
            Dim idx As Integer = row.f_iひも番号
            If idx < 1 OrElse p_iひもの本数 < idx Then
                Return False
            End If

            Dim hidx As Integer = idx + p_i側面の切捨コマ数
            Dim knotfolder As CKnotFolder
            '縦方向のコマ
            For vidx As Integer = 1 To p_iコマ空間高さ
                knotfolder = _KnotFolderSpace.GetAt(hidx, vidx)
                If knotfolder IsNot Nothing Then
                    If knotfolder.IsInPlate Then
                        knotfolder.m_row縦横展開(emExp._Tate) = row
                    End If
                End If
            Next
        Next
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}", _KnotFolderSpace.dump_knot_band)

        '側面への編み上げ
        _KnotFolderSpace.BottomExtendToSides()

        Return ret
    End Function

End Class
