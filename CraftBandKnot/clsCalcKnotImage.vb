Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageData
Imports CraftBand.clsImageItem
Imports CraftBand.clsImageItem.CKnot
Imports CraftBand.Tables.dstDataTables

Partial Public Class clsCalcKnot

    'CAD測定値
    Const c_foldingRatio As Double = 2 / 6.43   '要尺の折り位置までの比
    Const c_tiltRatio As Double = 0.216   '折りの傾き(幅に対する差の比)

    ''リスト出力時に生成
    'Dim _ImageList横ひも As clsImageItemList   '横ひもの展開レコードを含む
    'Dim _ImageList縦ひも As clsImageItemList   '縦ひもの展開レコードを含む

    'プレビュー時に生成
    'Dim _imageList側面ひも As clsImageItemList    '側面の展開レコードを含む
    Dim _ImageListコマ As clsImageItemList
    Dim _ImageList描画要素 As clsImageItemList '底や側面の展開図
    Dim _ImageList開始位置 As clsImageItemList

    Const IdxDrawBandStart As Integer = 15




    '底展開状態の全コマ位置
    Dim _KnotFolderSpace As New CKnotFolderSpace



#Region "テクスチャファイル作成用"
    '絵のファイル
    Dim _PlatePngFilePath(clsImageData.cBasketPlateCount - 1) As String
    Property p_sPlatePngFilePath(ByVal pidx As clsImageData.enumBasketPlateIdx, ByVal check As Boolean) As String
        Get
            If check AndAlso Not String.IsNullOrWhiteSpace(_PlatePngFilePath(pidx)) Then
                If IO.File.Exists(_PlatePngFilePath(pidx)) Then
                    Return _PlatePngFilePath(pidx)
                Else
                    Return Nothing
                End If
            Else
                Return _PlatePngFilePath(pidx)
            End If
        End Get
        Set(value As String)
            _PlatePngFilePath(pidx) = value
            If check AndAlso Not String.IsNullOrWhiteSpace(value) AndAlso IO.File.Exists(value) Then
                IO.File.Delete(value)
            End If
        End Set
    End Property

    '絵の回転角度
    Shared _PlateAngle() As Integer = {0, 90, 0, -90, 180}

#End Region

    'コマ空間 (_dコマベース寸法のグリッド)
    '  
    '      1    2    3   → HorizontalCount           　　　　   
    '   +----+----+----+ ...               　　　　y　　　
    ' 1 |    |    |    | ...               　　　　↑　　　　　
    '   +----+----+----+ ...               　　　　│　　　　　
    ' 2 |    |    |    | ...               　　　　│
    '   +----+----+----+ ...               　　　　│原点はグリッドの中心　　　　　
    ' 3 |    |    |    | ...               ────・────→ x
    '   +----+----+----+ ...               　　　　│　　　　　
    ' ↓  ....　　　　　　　　 　　　　　　　　　　│　
    'VerticalCount

    ReadOnly Property p_iコマ空間幅 As Integer
        Get
            Return _i横のコマ数 + 2 * p_i高さコマ数計
        End Get
    End Property

    ReadOnly Property p_iコマ空間高さ As Integer
        Get
            Return _i縦のコマ数 + 2 * p_i高さコマ数計
        End Get
    End Property

    ReadOnly Property p_i高さコマ数計 As Integer
        Get
            Return _i高さのコマ数 + _i折り返しコマ数
        End Get
    End Property

    '四角数,入力値(ひも長加算,ひも幅)がFixした状態で、コマ配置をセットする
    Private Function calc_コマ配置計算() As Boolean
        Dim ret As Boolean = True

        'CalcOutput により、以下には記号がセットされています
        '_tbl縦横展開(emExp._Yoko), _tbl縦横展開(emExp._Tate)

        '_Data.p_tbl側面→_tbl縦横展開(emExp._Side)
        tbl側面to縦横展開DataTable()


        _KnotFolderSpace.Reinitialize(p_iコマ空間幅, p_iコマ空間高さ)
        ret = SetTables()
        'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_KnotFolderSpace={0}", _KnotFolderSpace.ToString)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "_KnotFolderSpace={0}", _KnotFolderSpace.dump)

        Return ret
    End Function

    Private Function image_コマ配置(ByVal isKnotLeft As Boolean) As Boolean
        Dim ret As Boolean = True

        '_KnotFolderSpace 設定済のこと

        'コマ描画
        For Each knotfolder In _KnotFolderSpace
            If knotfolder.Count = 2 Then
                Dim x As Double = (knotfolder.m_index横 - 1) - (_KnotFolderSpace.HorizontalCount / 2)
                Dim y As Double = -(knotfolder.m_index縦 - 1) + (_KnotFolderSpace.VerticalCount / 2)

                Dim p As S実座標 = toPoint(x, y)

                addコマ(p, knotfolder.m_row横方向, knotfolder.m_row縦方向,
                      p_d基本のひも幅, isKnotLeft, knotfolder.m_knotSide)

            End If
        Next

        Return ret
    End Function


    'コマ1点の情報
    Public Class CKnotFolder

        Friend m_row横方向 As tbl縦横展開Row = Nothing
        Friend m_row縦方向 As tbl縦横展開Row = Nothing
        Friend m_index横 As Integer = 0
        Friend m_index縦 As Integer = 0
        Friend m_isStart横 As Boolean = False
        Friend m_isStart縦 As Boolean = False

        Friend m_knotSide As enumKnotSide = enumKnotSide._none

        Sub New(ByVal horz As Integer, ByVal vert As Integer)
            m_index横 = horz
            m_index縦 = vert
        End Sub

        Function Count() As Integer
            Dim cnt As Integer = 0
            If m_row横方向 IsNot Nothing Then
                cnt += 1
            End If
            If m_row縦方向 IsNot Nothing Then
                cnt += 1
            End If
            Return cnt
        End Function

        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("({0},{1})", m_index横, m_index縦)
            If m_row横方向 IsNot Nothing Then
                Dim iひも種 As Integer = m_row横方向.f_iひも種
                sb.AppendFormat(" m_row横方向:({0},{1}){5}{2}{3} {4:F1}", DirectCast(iひも種, enumひも種), m_row横方向.f_iひも番号, m_row横方向.f_s記号, m_row横方向.f_s色, m_row横方向.f_d出力ひも長,
                IIf(Me.m_isStart横, "*", " "))
            Else
                sb.Append("                        ")
            End If
            If m_row縦方向 IsNot Nothing Then
                Dim iひも種 As Integer = m_row縦方向.f_iひも種
                sb.AppendFormat(" m_row縦方向:({0},{1}){5}{2}{3} {4:F1}", DirectCast(iひも種, enumひも種), m_row縦方向.f_iひも番号, m_row縦方向.f_s記号, m_row縦方向.f_s色, m_row縦方向.f_d出力ひも長,
                IIf(Me.m_isStart縦, "*", " "))
            End If
            Return sb.ToString
        End Function
    End Class

    'コマが配置される領域全体
    Public Class CKnotFolderSpace
        Implements IEnumerable(Of CKnotFolder)

        ' 内部データ
        Private _folders(,) As CKnotFolder

        Public ReadOnly Property IsValid As Boolean = False
        '領域サイズ
        Public ReadOnly Property HorizontalCount As Integer  ' 
        Public ReadOnly Property VerticalCount As Integer '

        Public Sub New()
        End Sub

        'サイズ再設定と初期化
        Public Sub Reinitialize(ByVal width As Integer, ByVal height As Integer)
            If width < 1 OrElse width < 1 Then
                _folders = Nothing
                _IsValid = False
            End If

            _HorizontalCount = width
            _VerticalCount = height

            '内部の2次元配列は (行=縦, 列=横) で確保する
            _folders = New CKnotFolder(VerticalCount - 1, HorizontalCount - 1) {}

            For y As Integer = 0 To VerticalCount - 1
                For x As Integer = 0 To HorizontalCount - 1
                    _folders(y, x) = New CKnotFolder(x + 1, y + 1)
                Next
            Next
            _IsValid = True
        End Sub

        'x=1～HorizontalCount, y=1～VerticalCount
        Public ReadOnly Property GetAt(ByVal x As Integer, ByVal y As Integer) As CKnotFolder
            Get
                If Not IsValid OrElse x < 1 OrElse HorizontalCount < x OrElse y < 1 OrElse VerticalCount < y Then
                    Return Nothing
                End If

                ' 内部の配列アクセス時は (行=Y, 列=X) に変換してアクセスする
                Return _folders(y - 1, x - 1)
            End Get
        End Property

        Public Function GetStartKnot() As CKnotFolder
            For Each knot In Me
                If knot.m_isStart横 AndAlso knot.m_isStart縦 Then
                    Return knot
                End If
            Next
            Return Nothing
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("CKnotFolderSpace({0}) 幅={1} 高さ={2}", IsValid, HorizontalCount, VerticalCount).AppendLine()
            For y As Integer = 1 To VerticalCount
                For x As Integer = 1 To HorizontalCount
                    Dim knot As CKnotFolder = GetAt(x, y)
                    If knot.Count = 2 Then
                        If knot.m_isStart横 AndAlso knot.m_isStart縦 Then
                            sb.Append("■")
                        Else
                            sb.Append("田")
                        End If
                    ElseIf knot.Count = 1 Then
                        sb.Append("×")
                    Else
                        sb.Append("　")
                    End If
                Next
                sb.AppendLine()
            Next
            Return sb.ToString
        End Function
        Public Function dump() As String
            Dim sb As New System.Text.StringBuilder
            sb.Append(Me.ToString)
            For x As Integer = 1 To HorizontalCount
                For y As Integer = 1 To VerticalCount
                    Dim knot As CKnotFolder = GetAt(x, y)
                    sb.AppendLine(knot.ToString)
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

    '       emExp._Tate
    '       +----+ 
    '       |    | 
    '  +----+----+----+ 
    '  |    |    |    | emExp._Yoko
    '  +----+----+----+ 
    '       |    |  ↑
    '       +----+  ←emExp._Side

    'テーブルのレコードをセットする
    Function SetTables() As Boolean
        Dim ret As Boolean = True

        Dim _i左から何番目 As Integer = _Data.p_row底_縦横.Value("f_i左から何番目")
        Dim _i上から何番目 As Integer = _Data.p_row底_縦横.Value("f_i上から何番目")

        For i As Integer = 0 To cExpandCount - 1
            Dim bcount As Integer
            Dim setcount As Integer = 0

            Select Case DirectCast(i, emExp)
                Case emExp._Yoko
                    bcount = _i縦のコマ数'縦にコマが並ぶ
                Case emExp._Tate
                    bcount = _i横のコマ数'横にコマが並ぶ
                Case emExp._Side
                    bcount = p_i高さコマ数計
            End Select

            For Each row As tbl縦横展開Row In _tbl縦横展開(i).Select(Nothing, "f_iひも番号 ASC")
                Dim idx As Integer = row.f_iひも番号
                If idx < 1 OrElse bcount < idx Then
                    Return False
                End If

                Dim knot As CKnotFolder
                Select Case DirectCast(i, emExp)
                    Case emExp._Yoko
                        '横ひも
                        For x As Integer = 1 To p_iコマ空間幅
                            knot = _KnotFolderSpace.GetAt(x, p_i高さコマ数計 + idx)
                            If knot IsNot Nothing Then
                                knot.m_row横方向 = row
                                If x = 1 Then
                                    knot.m_knotSide = enumKnotSide._左
                                End If
                                knot.m_isStart横 = (idx = _i上から何番目)
                            End If
                        Next
                        setcount += 1

                    Case emExp._Tate
                        '縦ひも
                        For y As Integer = 1 To p_iコマ空間高さ
                            knot = _KnotFolderSpace.GetAt(p_i高さコマ数計 + idx, y)
                            If knot IsNot Nothing Then
                                knot.m_row縦方向 = row
                                If y = 1 Then
                                    knot.m_knotSide = enumKnotSide._上
                                End If
                                knot.m_isStart縦 = (idx = _i左から何番目)
                            End If
                        Next
                        setcount += 1

                    Case emExp._Side
                        '上側面・下側面
                        For x As Integer = 1 To _i横のコマ数
                            knot = _KnotFolderSpace.GetAt(x + p_i高さコマ数計, p_i高さコマ数計 - idx + 1)
                            If knot IsNot Nothing Then
                                knot.m_row横方向 = row
                            End If
                            knot = _KnotFolderSpace.GetAt(x + p_i高さコマ数計, p_i高さコマ数計 + _i縦のコマ数 + idx)
                            If knot IsNot Nothing Then
                                knot.m_row横方向 = row
                                If x = 1 Then
                                    knot.m_knotSide = enumKnotSide._左
                                End If
                            End If
                        Next
                        '左側面・右側面
                        For y As Integer = 1 To _i縦のコマ数
                            knot = _KnotFolderSpace.GetAt(p_i高さコマ数計 - idx + 1, p_i高さコマ数計 + y)
                            If knot IsNot Nothing Then
                                knot.m_row縦方向 = row
                            End If
                            knot = _KnotFolderSpace.GetAt(p_i高さコマ数計 + _i横のコマ数 + idx, p_i高さコマ数計 + y)
                            If knot IsNot Nothing Then
                                knot.m_row縦方向 = row
                            End If
                        Next

                        setcount += 1
                End Select
            Next
            If setcount <> bcount Then
                ret = False
            End If
        Next
        Return ret
    End Function






    Private Function toPoint(ByVal x As Double, ByVal y As Double) As S実座標
        Return New S実座標(_dコマベース寸法 * x, _dコマベース寸法 * y)
    End Function

    'Private Function getBand(ByVal pos As enumひも種, ByVal idx As Integer) As tbl縦横展開Row
    '    Try
    '        Dim band As clsImageItem = Nothing
    '        Select Case pos
    '            Case enumひも種.i_横
    '                '上からidx番目(1～),マイナスは下から(-1～-横のコマ数)
    '                If idx < 0 Then
    '                    idx = _i横のコマ数 + idx + 1
    '                End If
    '                band = _ImageList横ひも.GetRowItem(enumひも種.i_横, idx)
    '            Case enumひも種.i_縦
    '                '左からidx番目(1～),マイナスは右から(-1～-縦のコマ数)
    '                If idx < 0 Then
    '                    idx = _i縦のコマ数 + idx + 1
    '                End If
    '                band = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, idx)
    '            Case enumひも種.i_側面
    '                '下からidx番目の色(1～),マイナスは上から(-1～-側面のコマ数)
    '                If idx < 0 Then
    '                    idx = _i高さのコマ数 + _i折り返しコマ数 + idx + 1
    '                End If
    '                band = _imageList側面ひも.GetRowItem(enumひも種.i_側面, idx)
    '        End Select
    '        If band Is Nothing Then
    '            g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "getBand Err:{0} {1}", pos, idx)
    '            Return Nothing
    '        End If
    '        Return band.m_row縦横展開

    '    Catch ex As Exception
    '        g_clsLog.LogException(ex, "getBand", pos, idx)
    '        Return Nothing
    '    End Try
    'End Function

    'コマベース左上→コマの中心
    Private Function addコマ(ByVal p左上 As S実座標, ByVal y_band As tbl縦横展開Row, ByVal t_band As tbl縦横展開Row,
                           ByVal dひも幅 As Double, ByVal isleft As Boolean, ByVal mark As enumKnotSide) As Boolean

        Dim knotitem As New clsImageItem(p左上 + (Unit315 * (_dコマベース寸法 / 2)), y_band, t_band,
                                            dひも幅, _dコマの寸法, _dコマ間のすき間, isleft)
        If mark <> enumKnotSide._none Then
            knotitem.m_knot.SetMarkDisp(mark)
        End If

        If _ImageListコマ Is Nothing Then
            _ImageListコマ = New clsImageItemList
        End If
        _ImageListコマ.Add(knotitem)

        Return y_band IsNot Nothing AndAlso t_band IsNot Nothing
    End Function

    '横ひもリストの記号出力情報
    'Private Function imageList横記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean) As Boolean
    '    If _ImageList横ひも Is Nothing Then
    '        Return False
    '    End If

    '    '左側面の左 コマは右側面も
    '    Dim _左上 As S実座標 = toPoint(-(_i高さのコマ数 + _i折り返しコマ数 + (_i横のコマ数 / 2)), (_i縦のコマ数 / 2))
    '    Dim _左上_右側面 As S実座標 = toPoint((_i横のコマ数 / 2), (_i縦のコマ数 / 2))

    '    '上から下へ
    '    For idx As Integer = 1 To p_i横ひもの本数 '_i縦のコマ数
    '        Dim item As clsImageItem = _ImageList横ひも.GetRowItem(enumひも種.i_横, idx)
    '        If item Is Nothing Then
    '            Continue For
    '        End If

    '        Dim mark As enumKnotSide = enumKnotSide._none
    '        ''裏表・バンド幅変更なし
    '        If Not String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号) Then
    '            mark = enumKnotSide._左
    '        End If
    '        '
    '        For j = 1 To _i高さのコマ数 + _i折り返しコマ数
    '            addコマ(_左上 + Unit0 * (j - 1) * _dコマベース寸法, item.m_row縦横展開, getBand(enumひも種.i_側面, -j),
    '                  dひも幅, isKnotLeft, mark)
    '            mark = enumKnotSide._none
    '            addコマ(_左上_右側面 + Unit0 * (j - 1) * _dコマベース寸法, item.m_row縦横展開, getBand(enumひも種.i_側面, j),
    '                  dひも幅, isKnotLeft, mark)
    '        Next
    '        _左上 = _左上 + (Unit270 * _dコマベース寸法)
    '        _左上_右側面 = _左上_右側面 + (Unit270 * _dコマベース寸法)
    '    Next

    '    Return True
    'End Function

    ''縦ひもリストの記号出力情報
    'Private Function imageList縦記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean) As Boolean
    '    If _ImageList縦ひも Is Nothing Then
    '        Return False
    '    End If

    '    '上側面の上
    '    'Dim _左上 As S実座標 = toPoint(-(_i横のコマ数 / 2), _i高さのコマ数 + _i折り返しコマ数 + (_i縦のコマ数 / 2))
    '    Dim _左上_底 As S実座標 = toPoint(-(_i横のコマ数 / 2), (_i縦のコマ数 / 2))

    '    '上側面;左から右へ
    '    For idx As Integer = 1 To p_i縦ひもの本数 '_i横のコマ数
    '        Dim item As clsImageItem = _ImageList縦ひも.GetRowItem(enumひも種.i_縦, idx)
    '        If item Is Nothing Then
    '            Continue For
    '        End If

    '        Dim markL As enumKnotSide = enumKnotSide._none
    '        Dim markU As enumKnotSide = enumKnotSide._none

    '        ''裏表・バンド幅変更なし
    '        If Not String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号) AndAlso
    '            (_i高さのコマ数 + _i折り返しコマ数) = 0 Then
    '            If idx = 1 Then
    '                markL = enumKnotSide._左
    '            End If
    '            markU = enumKnotSide._上
    '        End If

    '        '底のコマ
    '        For j As Integer = 1 To p_i横ひもの本数 '_i縦のコマ数
    '            addコマ(_左上_底 + Unit270 * (j - 1) * _dコマベース寸法, getBand(enumひも種.i_横, j), item.m_row縦横展開,
    '                  dひも幅, isKnotLeft, markL Or markU)
    '            markU = enumKnotSide._none
    '        Next

    '        '_左上 = _左上 + (Unit0 * _dコマベース寸法)
    '        _左上_底 = _左上_底 + (Unit0 * _dコマベース寸法)
    '    Next

    '    Return True
    'End Function


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


    '_imageList側面ひも生成、縦横展開DataTable化したレコードを含む
    'Function imageList側面記号(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean) As Boolean

    '    '側面のレコードを縦横レコード化
    '    Dim tmptable As tbl縦横展開DataTable = tbl側面to縦横展開DataTable()

    '    '以降参照するのでここでセットする
    '    _imageList側面ひも = New clsImageItemList(tmptable, String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号))


    '    '下側面の左
    '    Dim _左上 As S実座標 = toPoint(-(_i横のコマ数 / 2), -(_i縦のコマ数 / 2))
    '    Dim _左下_上側面 As S実座標 = toPoint(-(_i横のコマ数 / 2), 1 + (_i縦のコマ数 / 2))

    '    '下側面:上から下へ
    '    For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
    '        Dim item As clsImageItem = _imageList側面ひも.GetRowItem(enumひも種.i_側面, i)
    '        If item Is Nothing Then
    '            Continue For
    '        End If

    '        Dim markL As enumKnotSide = enumKnotSide._none
    '        Dim markU As enumKnotSide = enumKnotSide._none
    '        ''裏表・バンド幅変更なし
    '        If Not String.IsNullOrWhiteSpace(g_clsSelectBasics.p_sリスト出力記号) Then
    '            markL = enumKnotSide._左
    '            If i = (_i高さのコマ数 + _i折り返しコマ数) Then
    '                markU = enumKnotSide._上
    '            End If
    '        End If

    '        '
    '        For j = 1 To p_i縦ひもの本数 '_i横のコマ数
    '            addコマ(_左上 + Unit0 * (j - 1) * _dコマベース寸法, item.m_row縦横展開, getBand(enumひも種.i_縦, j),
    '                  dひも幅, isKnotLeft, markL)
    '            markL = enumKnotSide._none

    '            addコマ(_左下_上側面 + Unit0 * (j - 1) * _dコマベース寸法, item.m_row縦横展開, getBand(enumひも種.i_縦, j),
    '                  dひも幅, isKnotLeft, markU)
    '        Next
    '        '
    '        _左上 = _左上 + (Unit270 * _dコマベース寸法)
    '        _左下_上側面 = _左下_上側面 + (Unit90 * _dコマベース寸法)
    '    Next
    '    Return True
    'End Function

    Function imageList描画要素(ByVal basecross As Boolean, ByVal sideline As Boolean) As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList


        Dim d側面の高さ As Double = _dコマベース寸法 * (_i高さのコマ数 + _i折り返しコマ数)
        Dim d折り返し高さ As Double = _dコマベース寸法 * _i折り返しコマ数
        Dim deltaコマ右 As S差分 = Unit0 * _dコマベース寸法
        Dim deltaコマ下 As S差分 = Unit270 * _dコマベース寸法
        Dim a四隅 As S四隅

        Dim a底 As S四隅
        a底.p左上 = toPoint(-_i横のコマ数 / 2, _i縦のコマ数 / 2)
        a底.p右上 = toPoint(_i横のコマ数 / 2, _i縦のコマ数 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        Dim delta As S差分
        Dim line As S線分

        '底の画像
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._bottom, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 1)
            item.m_a四隅 = a底
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._bottom, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._bottom)
            itemlist.AddItem(item)
        End If
        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底
        For i As Integer = 1 To _i横のコマ数
            If i < _i横のコマ数 AndAlso basecross Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
            For j As Integer = 1 To _i縦のコマ数
                If i = 1 AndAlso basecross Then
                    line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                    line += deltaコマ下 * j
                    item.m_lineList.Add(line)
                End If
            Next
        Next
        itemlist.AddItem(item)


        '折り返し線
        Dim itemFolding As clsImageItem = Nothing
        If 0 < d折り返し高さ Then
            itemFolding = New clsImageItem(clsImageItem.ImageTypeEnum._折り返し線, 1)
        End If


        '上の側面(前面の画像)
        a四隅.p左下 = a底.p左上
        a四隅.p右下 = a底.p右上
        delta = Unit90 * d側面の高さ
        a四隅.p左上 = a四隅.p左下 + delta
        a四隅.p右上 = a四隅.p右下 + delta
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._front, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 2)
            item.m_a四隅 = a四隅
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._front, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._front)
            itemlist.AddItem(item)
        End If

        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅 = a四隅
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
            line += Unit270 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数 - 1
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                line += deltaコマ下 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
            line += Unit90 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '下の側面(背面の画像)
        a四隅.p左上 = a底.p左下
        a四隅.p右上 = a底.p右下
        delta = Unit270 * d側面の高さ
        a四隅.p左下 = a四隅.p左上 + delta
        a四隅.p右下 = a四隅.p右上 + delta
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._back, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 3)
            item.m_a四隅 = a四隅
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._back, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._back)
            itemlist.AddItem(item)
        End If

        '下の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅 = a四隅
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下)
            line += Unit90 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                line += deltaコマ下 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下)
            line += Unit270 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '左の側面(左側面の画像)
        a四隅.p右上 = a底.p左上
        a四隅.p右下 = a底.p左下
        delta = Unit180 * d側面の高さ
        a四隅.p左上 = a四隅.p右上 + delta
        a四隅.p左下 = a四隅.p右下 + delta
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._leftside, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 4)
            item.m_a四隅 = a四隅
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._leftside, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._leftside)
            itemlist.AddItem(item)
        End If

        '左の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅 = a四隅
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
            line += Unit0 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
            line += Unit180 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '右の側面(右側面の画像)
        a四隅.p左上 = a底.p右上
        a四隅.p左下 = a底.p右下
        delta = Unit0 * d側面の高さ
        a四隅.p右上 = a四隅.p左上 + delta
        a四隅.p右下 = a四隅.p左下 + delta
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._rightside, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 5)
            item.m_a四隅 = a四隅
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._rightside, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._rightside)
            itemlist.AddItem(item)
        End If

        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅 = a四隅
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下)
            line += Unit180 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To _i高さのコマ数 + _i折り返しコマ数
            If i < _i高さのコマ数 + _i折り返しコマ数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下)
            line += Unit0 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)


        If itemFolding IsNot Nothing Then
            itemlist.AddItem(itemFolding)
        End If

        Return itemlist
    End Function

#Region "開始位置"

    Function imageList開始位置(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal outp As clsOutput) As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim line As clsImageItem.S線分

        '要尺の折り位置
        Dim foldingLen As Double = c_foldingRatio * _dコマの要尺

        '要尺
        If True Then
            Dim p要尺位置 As S実座標 = toPoint(-(_i横のコマ数 / 2) - (_i高さのコマ数 + _i折り返しコマ数), (_i縦のコマ数 / 2) + (_i高さのコマ数 + _i折り返しコマ数))
            If _i高さのコマ数 + _i折り返しコマ数 = 0 Then
                p要尺位置 = p要尺位置 + Unit90 * (_dコマベース寸法)
            End If
            If (_i高さのコマ数 + _i折り返しコマ数 - 1) * _dコマベース寸法 < _dコマベース要尺 Then
                p要尺位置.X = -(1 + _i横のコマ数 / 2) * _dコマベース寸法 - (_dコマベース要尺 / 2)
            Else
                p要尺位置.X = p要尺位置.X + (_dコマベース要尺 / 2)
            End If
            item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 9)
            Dim delta As S差分 = Unit270 * dひも幅
            item.m_a四隅.p左上 = p要尺位置 + Unit180 * (_dコマベース要尺 / 2) + Unit90 * (dひも幅 / 2)
            item.m_a四隅.p右上 = p要尺位置 + Unit0 * (_dコマベース要尺 / 2) + Unit90 * (dひも幅 / 2)
            item.m_a四隅.p左下 = item.m_a四隅.p左上 + delta
            item.m_a四隅.p右下 = item.m_a四隅.p右上 + delta
            Dim p As S実座標 = p要尺位置
            If 0 < _dコマ間のすき間 Then
                p = item.m_a四隅.p左上 + Unit0 * (_dコマ間のすき間 / 2)
                line = New S線分(p, p + delta)
                item.m_lineList.Add(line)
                p = item.m_a四隅.p右上 + Unit180 * (_dコマ間のすき間 / 2)
                line = New S線分(p, p + delta)
                item.m_lineList.Add(line)
            End If
            '中心から各折り位置まで
            Dim len_1 As Double = (_dコマの要尺 - foldingLen * 2) / 2 - (c_tiltRatio * dひも幅 / 2)
            Dim len_2 As Double = (_dコマの要尺 - foldingLen * 2) / 2 + (c_tiltRatio * dひも幅 / 2)
            If isKnotLeft Then
                Dim tmp As Double = len_1
                len_1 = len_2
                len_2 = tmp
            End If
            line = New S線分(New S実座標(p要尺位置.X + len_1, p要尺位置.Y + dひも幅 / 2), New S実座標(p要尺位置.X + len_2, p要尺位置.Y - dひも幅 / 2))
            item.m_lineList.Add(line)
            line = New S線分(New S実座標(p要尺位置.X - len_1, p要尺位置.Y - dひも幅 / 2), New S実座標(p要尺位置.X - len_2, p要尺位置.Y + dひも幅 / 2))
            item.m_lineList.Add(line)
            itemlist.AddItem(item)
        End If

        '開始位置の情報をセット
        Dim startInfo As CStartInfo = setStartInfo()
        If startInfo Is Nothing Then
            '設定されていなければ
            Return itemlist 'ここまでの結果
        End If

        '底の開始位置のマーク
        If True Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)
            Dim pコマ左上 As S実座標 = toPoint(-(_i横のコマ数 / 2) + (startInfo.i左から何番目 - 1), (_i縦のコマ数 / 2) - (startInfo.i上から何番目 - 1))
            Dim pコマ右下 As S実座標 = pコマ左上 + Unit315 * _dコマベース寸法

            line = New clsImageItem.S線分(pコマ左上, pコマ左上 + Unit0 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ左上, pコマ左上 + Unit270 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ右下, pコマ右下 + Unit90 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ右下, pコマ右下 + Unit180 * _dコマベース寸法)
            item.m_lineList.Add(line)

            itemlist.AddItem(item)
        End If


        '開始位置のコマの転記
        Dim dバンド長 As Double = _dコマベース要尺
        Dim pコマ位置 As S実座標 = toPoint(0, -_i縦のコマ数 / 2 - (_i高さのコマ数 + _i折り返しコマ数) - 1) + Unit270 * dバンド長
        item = New clsImageItem(pコマ位置, startInfo.row横展開, startInfo.row縦展開,
            dひも幅, _dコマの寸法, _dコマ間のすき間, isKnotLeft, True)
        itemlist.AddItem(item)

        'コマに続くバンド
        Dim d描画幅 As Double = item.m_knot.d描画幅
        If True Then
            '*左右
            Dim item1 As New clsImageItem(startInfo.row横展開, True) '右
            Dim item2 As New clsImageItem(startInfo.row横展開, True) '左
            item1.m_dひも幅 = dひも幅
            item2.m_dひも幅 = dひも幅

            item1.m_rひも位置.p左下 = pコマ位置 + Unit0 * d描画幅
            item2.m_rひも位置.p右上 = pコマ位置 + Unit180 * d描画幅
            If Not isKnotLeft Then
                item1.m_rひも位置.p左下 = item1.m_rひも位置.p左下 + Unit270 * dひも幅
                item2.m_rひも位置.p右上 = item2.m_rひも位置.p右上 + Unit90 * dひも幅
            End If
            item1.m_rひも位置.p右上 = New S実座標(item1.m_rひも位置.p左下.X + dバンド長, item1.m_rひも位置.p左下.Y + dひも幅)
            item2.m_rひも位置.p左下 = New S実座標(item2.m_rひも位置.p右上.X - dバンド長, item2.m_rひも位置.p右上.Y - dひも幅)

            'バンド化
            '始点T(D)　　 　　　終点T(C)
            '　　[□□→(0)□□]　　　↑deltaAx(90)
            '始点F(A) 　　　　　終点F(B)
            Dim band1 As New CBand(startInfo.row横展開)
            Dim band2 As New CBand(startInfo.row横展開)
            band1.aバンド位置 = New S四隅(item1.m_rひも位置.p左下, item1.m_rひも位置.p右下, item1.m_rひも位置.p右上, item1.m_rひも位置.p左上)
            band2.aバンド位置 = New S四隅(item2.m_rひも位置.p左下, item2.m_rひも位置.p右下, item2.m_rひも位置.p右上, item2.m_rひも位置.p左上)
            item1.AddBand(band1, IdxDrawBandStart, 1)
            item2.AddBand(band2, IdxDrawBandStart, 2)

            itemlist.AddItem(item1)
            itemlist.AddItem(item2)

            '*上下
            Dim item3 As New clsImageItem(startInfo.row縦展開, True) '上
            Dim item4 As New clsImageItem(startInfo.row縦展開, True) '下
            item3.m_dひも幅 = dひも幅
            item4.m_dひも幅 = dひも幅
            item3.m_rひも位置.p左下 = pコマ位置 + Unit90 * d描画幅
            item4.m_rひも位置.p右上 = pコマ位置 + Unit270 * d描画幅
            If isKnotLeft Then
                item3.m_rひも位置.p左下 = item3.m_rひも位置.p左下 + Unit180 * dひも幅 '
                item4.m_rひも位置.p右上 = item4.m_rひも位置.p右上 + Unit0 * dひも幅 '
            End If
            item3.m_rひも位置.p右上 = New S実座標(item3.m_rひも位置.p左下.X + dひも幅, item3.m_rひも位置.p左下.Y + dバンド長)
            item4.m_rひも位置.p左下 = New S実座標(item4.m_rひも位置.p右上.X - dひも幅, item4.m_rひも位置.p右上.Y - dバンド長)

            'バンド化
            '始点F(A)□　始点T(D)　→deltaAx(0)
            '　　  　↓(270)
            '終点F(B)□　終点T(C)
            Dim band3 As New CBand(startInfo.row縦展開)
            Dim band4 As New CBand(startInfo.row縦展開)
            band3.aバンド位置 = New S四隅(item3.m_rひも位置.p左上, item3.m_rひも位置.p左下, item3.m_rひも位置.p右下, item3.m_rひも位置.p右上)
            band4.aバンド位置 = New S四隅(item4.m_rひも位置.p左上, item4.m_rひも位置.p左下, item4.m_rひも位置.p右下, item4.m_rひも位置.p右上)
            item3.AddBand(band3, IdxDrawBandStart, 3)
            item4.AddBand(band4, IdxDrawBandStart, 4)

            itemlist.AddItem(item3)
            itemlist.AddItem(item4)
        End If


        '「開始位置」
        Dim p開始位置 As S実座標 = pコマ位置 + Unit135 * (dバンド長) + Unit180 * (dバンド長)
        item = New clsImageItem(p開始位置, {text開始位置()}, dひも幅, 1)
        itemlist.AddItem(item)

        '左から×番目,上から×番目
        Dim pos1 As String = text左から() & startInfo.i左から何番目.ToString & text番目のコマ()
        Dim pos2 As String = text上から() & startInfo.i上から何番目.ToString & text番目のコマ()
        item = New clsImageItem(p開始位置 + Unit315 * (dひも幅 * 2), {pos1, pos2}, dひも幅 * 2 / 3, 2)
        itemlist.AddItem(item)


        '各方向の説明文字列 
        If True Then
            Dim p文字(3) As S実座標
            '上のひも
            p文字(0).X = pコマ位置.X + dひも幅 * 2
            p文字(0).Y = pコマ位置.Y + dバンド長
            '下のひも
            p文字(1).X = pコマ位置.X + dひも幅 * 2
            p文字(1).Y = pコマ位置.Y - 2 * dバンド長 / 3
            '左のひも
            p文字(2).X = pコマ位置.X - dバンド長 - dひも幅 * 15 '想定文字数分
            p文字(2).Y = pコマ位置.Y - dひも幅 * 1.5
            '右のひも
            p文字(3).X = pコマ位置.X + dバンド長 + _dコマの寸法
            p文字(3).Y = pコマ位置.Y + dひも幅

            startInfo.setMyValue(True) 'マイひも長係数を乗算する
            For i As Integer = 0 To 3
                With startInfo
                    '<コマの{0}>
                    Dim str1 As String = String.Format(My.Resources.CalcOutKnotOf, .getSideString(i))
                    str1 += outp.outLengthTextWithUnit(.getBandLength(i))
                    str1 += "  "
                    str1 += .getDiffString(i, outp)

                    '加算計 {0} (縁の始末:{1} ひも長加算:{2} {3}加算:{4})
                    Dim str2 As String = String.Format(My.Resources.CalcOutAddLen,
                        outp.outLengthTextWithUnit(.getAddSum(i)), outp.outLengthText(.getAddHem()), outp.outLengthText(.getAddVert()), .getSideString(i), outp.outLengthText(.getAddEach(i)))

                    '折り位置から
                    Dim str3 As String = String.Format(My.Resources.CalcOutFromFolding)
                    str3 += outp.outLengthTextWithUnit(.getFoldingLength(i))
                    str3 += "  "
                    str3 += .getDiffFoldingString(i, outp)
                    '
                    item = New clsImageItem(p文字(i), {str1, str2, str3}, dひも幅 * 2 / 3, i + 3)
                    itemlist.AddItem(item)
                End With
            Next
        End If

        'コマ位置の中央線
        startInfo.setMyValue(False) 'マイひも長係数を乗算しない
        If True Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 2)
            Dim d1本幅 As Double = dひも幅 / _I基本のひも幅
            If Abs(startInfo.getDiff(0)) < dバンド長 * 2 + _dコマベース要尺 + d1本幅 Then
                '上下中央の横線
                Dim pp As New S実座標(pコマ位置.X - dひも幅 * 1.5, pコマ位置.Y)
                line = New S線分(pp, pp + Unit0 * dひも幅 * 3)
                If Abs(startInfo.getDiff(0)) < d1本幅 Then
                    '1本幅以下
                    item.m_lineList.Add(line)
                ElseIf (_dコマの要尺 / 2 - d1本幅) <= startInfo.getDiff(0) Then
                    '要尺以上、上が長いので中央点は上にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(0) - _dコマベース要尺) / 2
                    line = line + Unit90 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                ElseIf startInfo.getDiff(0) <= -(_dコマの要尺 / 2 - d1本幅) Then
                    '要尺以上、下が長いので中央点は下にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(1) - _dコマベース要尺) / 2
                    line = line + Unit270 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                Else
                    '描けません
                End If
            End If
            If Abs(startInfo.getDiff(2)) < dバンド長 * 2 + _dコマベース要尺 + d1本幅 Then
                '左右中央の縦線
                Dim pp As New S実座標(pコマ位置.X, pコマ位置.Y - dひも幅 * 1.5)
                line = New S線分(pp, pp + Unit90 * dひも幅 * 3)
                If Abs(startInfo.getDiff(2)) < d1本幅 Then
                    '1本幅以下
                    item.m_lineList.Add(line)
                ElseIf (_dコマの要尺 / 2 - d1本幅) <= startInfo.getDiff(2) Then
                    '要尺以上、左が長いので中央点は左にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(2) - _dコマベース要尺) / 2
                    line = line + Unit180 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                ElseIf startInfo.getDiff(2) <= -(_dコマの要尺 / 2 - d1本幅) Then
                    '要尺以上、右が長いので中央点は右にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(3) - _dコマベース要尺) / 2
                    line = line + Unit0 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                Else
                    '描けません
                End If
            End If
            If 0 < item.m_lineList.Count Then
                itemlist.AddItem(item)
            End If
        End If


        Return itemlist
    End Function
#End Region

    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '念のため
        '_imageList側面ひも = Nothing
        _ImageListコマ = Nothing
        _ImageList描画要素 = Nothing
        _ImageList開始位置 = Nothing

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If



        'If _ImageList横ひも Is Nothing OrElse _ImageList縦ひも Is Nothing Then
        '    '処理に必要な情報がありません。
        '    p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
        '    Return False
        'End If

        '記号表示の位置をセット
        Dim isKnotLeft As Boolean = False
        Select Case _Data.p_row底_縦横.Value("f_iコマ上側の縦ひも")
            Case enumコマ上側の縦ひも.i_どちらでも
                isKnotLeft = My.Settings.IsKnotLeft
            Case enumコマ上側の縦ひも.i_左側
                isKnotLeft = True
            Case enumコマ上側の縦ひも.i_右側
                isKnotLeft = False
        End Select
        'Dim dひも幅 As Double = g_clsSelectBasics.p_d指定本幅(_I基本のひも幅)

        '文字サイズと基本色
        imgData.setBasics(_dコマの寸法, _Data.p_row目標寸法.Value("f_s基本色"))

        '通常描画

        'リスト処理で残された情報
        image_コマ配置(isKnotLeft)
        'imageList側面記号(p_d基本のひも幅, isKnotLeft)
        'imageList横記号(p_d基本のひも幅, isKnotLeft)
        'imageList縦記号(p_d基本のひも幅, isKnotLeft)
        _ImageList開始位置 = imageList開始位置(p_d基本のひも幅, isKnotLeft, outp)
        _ImageList描画要素 = imageList描画要素(False, False)

        '中身を移動
        'imgData.MoveList(_ImageList横ひも)
        '_ImageList横ひも = Nothing
        ''imgData.MoveList(_ImageList縦ひも)
        '_ImageList縦ひも = Nothing
        ''imgData.MoveList(_imageList側面ひも)
        '_imageList側面ひも = Nothing

        imgData.MoveList(_ImageListコマ)

        _ImageListコマ = Nothing
        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing
        imgData.MoveList(_ImageList開始位置)
        _ImageList開始位置 = Nothing

        '付属品
        AddPartsImage(imgData, _Data, False) '描画

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function

End Class
