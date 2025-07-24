Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables

''' <summary>
''' 画像ファイル生成
''' </summary>
Public Class CImageDraw
    Implements IDisposable

    'クリップ画像保存の形式
    Public Const cImageClipFileExtention As String = ".png"


    Dim ImageScale As Double = 1

    ' (原点)
    '　●→→→→→→→→→→→→X
    '　↓　(TopLeft)
    '　↓　　┌───┐
    '　↓　　│　　　││
    '　↓　　│　　　│↓Height
    '　↓　　└───┘
    '　↓     ───→ (BottomRight)
    '  Y　　　Width　
    '


    '定数
    Private Const cThinPenWidth As Single = 0.1 '細い
    Private Const cThickPenWidth As Single = 3 '太い

    '描画領域
    Dim Canvas As Bitmap = Nothing


    '描画用オブジェクト
    Private _Graphic As Graphics

    '標準描画用
    Private _Pen_black_thin As Pen = Nothing
    Private _Pen_black_thick As Pen = Nothing
    Private _Pen_black_dot As Pen = Nothing
    Private _Pen_red As Pen = Nothing
    Private _Brush_black As SolidBrush = Nothing

    Private _Font As Font

    Dim _FontSize As Single

    'ひもの幅
    Dim _KnotBandWidth As Double


#Region "画面の解像度"
    '画面の解像度(画面から取得)
    Private Shared dpi_h As Double = -1.0F
    Private Shared dpi_v As Double = -1.0F

    Private Sub getPixcel()
        '画面の解像度取得(1回だけ)
        If dpi_h < 0 OrElse dpi_v < 0 Then
            Dim measure As New Bitmap(100, 100) 'Width,Heightはダミー
            '1インチあたりのピクセル数
            dpi_h = measure.HorizontalResolution
            dpi_v = measure.VerticalResolution
            measure.Dispose()
        End If
    End Sub

    Public ReadOnly Property VerticalDpi As Double
        Get
            getPixcel()
            Return dpi_v
        End Get
    End Property

    Public ReadOnly Property HorizontalDpi As Double
        Get
            getPixcel()
            Return dpi_h
        End Get
    End Property
#End Region

#Region "ひも描画色のセット"

    Private _BandDefaultPenBrush As CPenBrush
    Private _BandPenBrushMap As New Dictionary(Of String, CPenBrush)

    Private Class CPenBrush
        Implements IDisposable

        Friend PenBand As Pen = Nothing '外枠線
        Friend PenLane As Pen = Nothing '中線
        Friend BrushSolid As SolidBrush = Nothing '記号色
        Friend BrushAlfa As SolidBrush = Nothing '内部塗りつぶし色
        Friend PenWidth As Single = 0 'ペンの幅

        Sub New(ByVal drcol As clsColorRecordSet)
            If drcol IsNot Nothing Then
                If Not drcol.BaseColor.IsEmpty Then
                    BrushSolid = New SolidBrush(drcol.BaseColor)
                End If
                If 0 < drcol.FramePenWidth AndAlso Not drcol.FramePenColor.IsEmpty Then
                    PenBand = New Pen(drcol.FramePenColor, drcol.FramePenWidth)
                    PenWidth = drcol.FramePenWidth
                End If
                If 0 < drcol.LanePenWidth AndAlso Not drcol.LanePenColor.IsEmpty Then
                    PenLane = New Pen(drcol.LanePenColor, drcol.LanePenWidth)
                End If
                If Not drcol.BrushAlfaColor.IsEmpty Then
                    BrushAlfa = New SolidBrush(drcol.BrushAlfaColor)
                End If
            Else
                'デフォルトの一式
                PenBand = New Pen(Color.Black, cThickPenWidth)
                PenWidth = cThickPenWidth
                PenLane = New Pen(Color.Gray, cThinPenWidth)
                BrushSolid = New SolidBrush(Color.Black)
            End If
        End Sub

        '描画されない値(#52)
        Public ReadOnly Property IsNoDrawing As Boolean
            Get
                'Debug.Print("{0},{1},{2}", BrushSolid.Color.R, BrushSolid.Color.G, BrushSolid.Color.B)
                Return PenBand Is Nothing AndAlso PenLane Is Nothing AndAlso BrushAlfa Is Nothing AndAlso
                    ((BrushSolid Is Nothing) OrElse (BrushSolid.Color.R = MaxRgbValue AndAlso BrushSolid.Color.G = MaxRgbValue AndAlso BrushSolid.Color.B = MaxRgbValue))
            End Get
        End Property

        Private disposedValue As Boolean
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    If PenLane IsNot Nothing Then PenLane.Dispose()
                    If PenBand IsNot Nothing Then PenBand.Dispose()
                    If BrushSolid IsNot Nothing Then BrushSolid.Dispose()
                    If BrushAlfa IsNot Nothing Then BrushAlfa.Dispose()
                End If

                ' TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                ' TODO: 大きなフィールドを null に設定します
                disposedValue = True
            End If
        End Sub

        ' ' TODO: 'Dispose(disposing As Boolean)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        ' Protected Overrides Sub Finalize()
        '     ' このコードを変更しないでください。クリーンアップ コードを 'Dispose(disposing As Boolean)' メソッドに記述します
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' このコードを変更しないでください。クリーンアップ コードを 'Dispose(disposing As Boolean)' メソッドに記述します
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class

    Private Function GetBandPenBrush(ByVal obj As Object) As CPenBrush
        If obj Is Nothing OrElse IsDBNull(obj) Then
            Return _BandDefaultPenBrush
        End If
        Dim sColor As String = obj.ToString
        If String.IsNullOrWhiteSpace(sColor) Then
            '色が空
            Return _BandDefaultPenBrush
        End If
        Dim oneset As CPenBrush = Nothing
        If _BandPenBrushMap.ContainsKey(sColor) Then
            If _BandPenBrushMap(sColor) IsNot Nothing Then
                Return _BandPenBrushMap(sColor)
            End If
        Else
            Dim drcol As clsColorRecordSet = g_clsMasterTables.GetColorRecordSet(sColor, g_clsSelectBasics.p_s対象バンドの種類名, False)
            If drcol Is Nothing Then
                '色はあるがマスターにない
                _BandPenBrushMap.Add(sColor, Nothing)
            Else
                oneset = New CPenBrush(drcol)
                _BandPenBrushMap.Add(sColor, oneset)
                Return oneset
            End If
        End If
        Return _BandDefaultPenBrush
    End Function
#End Region

#Region "実座標→ピクセル"

    '原点となる位置
    Dim _TopY As Double
    Dim _LeftX As Double
    '現単位あたりのピクセル数
    Dim _pixcel_h As Double
    Dim _pixcel_v As Double

    Function pixcel_X(x As Double) As Single
        Return (x - _LeftX) * _pixcel_h * ImageScale
    End Function

    Function pixcel_Y(y As Double) As Single
        Return -(y - _TopY) * _pixcel_v * ImageScale
    End Function

    Function pixcel_width(width As Double) As Single
        Return width * _pixcel_h * ImageScale
    End Function

    Function pixcel_height(height As Double) As Single
        Return height * _pixcel_v * ImageScale
    End Function

    Function pixcel_point(p As S実座標) As PointF
        Return New Point(pixcel_X(p.X), pixcel_Y(p.Y))
    End Function

    '線分の2点の配列
    Function pixcel_lines(line As S線分) As PointF()
        Dim lst As New List(Of PointF)
        lst.Add(pixcel_point(line.p開始))
        lst.Add(pixcel_point(line.p終了))
        Return lst.ToArray
    End Function

    '四隅の4点の配列
    Function pixcel_lines(sqare As S四隅) As PointF()
        Dim lst As New List(Of PointF)
        For i As Integer = 0 To 4
            Dim ii As Integer = i Mod 4 '閉ループ
            Dim point As PointF = New PointF(pixcel_X(sqare.Point(ii).X), pixcel_Y(sqare.Point(ii).Y))
            lst.Add(point)
        Next
        Return lst.ToArray
    End Function

    '領域を長方形へ
    Function pixcel_rectangle(rect As S領域) As RectangleF
        Dim rectan As RectangleF
        '左上X,Y
        rectan.X = pixcel_X(rect.p左下.X)
        rectan.Y = pixcel_Y(rect.p右上.Y)
        rectan.Width = pixcel_width(rect.p右上.X - rect.p左下.X)
        rectan.Height = pixcel_height(rect.p右上.Y - rect.p左下.Y)
        Return rectan
    End Function


#End Region

    '描画の基本情報取得
    Sub New(ByVal imgdata As clsImageData)
        Try
            '現単位あたりのピクセル数
            Dim one_inch As New Length(1, "in")
            _pixcel_h = HorizontalDpi / (one_inch.Value)
            _pixcel_v = VerticalDpi / (one_inch.Value)

            '計算済の描画領域情報に対応したピクセル数を得る
            Dim width As Integer = Math.Ceiling(imgdata.DrawingRect.x幅 * ImageScale * _pixcel_h) + 1
            Dim height As Integer = Math.Ceiling(imgdata.DrawingRect.y高さ * ImageScale * _pixcel_v) + 1

            '座標変換の起点
            _TopY = imgdata.DrawingRect.y最上
            _LeftX = imgdata.DrawingRect.x最左

            'ピクセルを指定して描画領域の準備
            newCanvas(width, height, imgdata.BasicColor, imgdata.BasicBandWidth)
        Catch ex As Exception
            g_clsLog.LogException(ex, "CImageDraw.New")
            Canvas = Nothing
        End Try
    End Sub

    '描画用オブジェクト生成
    Private Sub newCanvas(ByVal width As Integer, ByVal height As Integer, ByVal basiccolor As String, ByVal basicbandwidth As Double)
        '描画領域を作成
        Canvas = New Bitmap(width, height)

        'ImageオブジェクトのGraphicsオブジェクトを作成する
        _Graphic = Graphics.FromImage(Canvas)
        'ピクセル単位
        _Graphic.PageUnit = GraphicsUnit.Pixel
        '全体を塗りつぶす
        _Graphic.FillRectangle(Brushes.White, _Graphic.VisibleClipBounds)

        'Penオブジェクトの作成
        _Pen_black_thin = New Pen(Color.Black, cThinPenWidth)
        _Pen_black_thick = New Pen(Drawing.Color.Black, cThickPenWidth)
        _Pen_black_dot = New Pen(Drawing.Color.Black, cThinPenWidth)
        _Pen_black_dot.DashStyle = Drawing2D.DashStyle.Dot
        _Pen_red = New Pen(Drawing.Color.Red, cThickPenWidth)
        _Brush_black = New SolidBrush(Drawing.Color.Black)

        'ひも描画用
        Dim drcol As clsColorRecordSet = g_clsMasterTables.GetColorRecordSet(basiccolor, g_clsSelectBasics.p_s対象バンドの種類名, False)
        _BandDefaultPenBrush = New CPenBrush(drcol)

        'フォントオブジェクトの作成
        _FontSize = pixcel_width(basicbandwidth / 2)
        _Font = New Font(My.Resources.FontNameMark, _FontSize)

        '滑らかな描画
        _Graphic.SmoothingMode = SmoothingMode.AntiAlias
        'サイズ枠描画
        '_Graphic.DrawRectangle(_Pen_black_thin, 0, 0, width - 1, height - 1)
    End Sub

    Public Class DrawException
        Inherits Exception

        Public Property ErrorItem As clsImageItem

        Public Sub New(ByVal ex As Exception, ByVal item As clsImageItem, ByVal msg As String)
            MyBase.New(msg, ex)
            Me.ErrorItem = item
        End Sub
    End Class

    '描画
    Function Draw(ByVal imglist As clsImageItemList) As Boolean
        If Canvas Is Nothing Then
            Return False
        End If
        Dim ret As Boolean = True
        For Each item As clsImageItem In imglist
            Try
                If Not DrawItem(item) Then
                    ret = False
                End If
            Catch ex As Exception
                Throw New DrawException(ex, item, "Please provide this information along with your data to the developer.")
            End Try
        Next
        Return ret
    End Function

    Function DrawItem(ByVal item As clsImageItem) As Boolean
        Select Case item.m_ImageType
            Case ImageTypeEnum._横バンド
                Return draw横バンド(item)

            Case ImageTypeEnum._縦バンド
                Return draw縦バンド(item)

            Case ImageTypeEnum._バンドセット
                Return drawバンドセット(item)

            Case ImageTypeEnum._コマ
                Return drawコマ(item)

            Case ImageTypeEnum._底枠
                Return draw底枠(item)

            Case ImageTypeEnum._底枠2
                Return draw底枠2(item)

            Case ImageTypeEnum._横の側面, ImageTypeEnum._縦の側面
                Return draw側面(item)

            Case ImageTypeEnum._四隅領域
                Return draw四隅領域(item)

            Case ImageTypeEnum._四隅領域線
                Return draw四隅領域線(item)

            Case ImageTypeEnum._底の中央線
                Return draw底の中央線(item)

            Case ImageTypeEnum._折り返し線
                Return draw折り返し線(item)

            Case ImageTypeEnum._文字列
                Return draw文字列(item)

            Case ImageTypeEnum._編みかた
                Return draw編みかた(item)

            Case ImageTypeEnum._画像保存
                Return save画像(item)

            Case ImageTypeEnum._画像貼付
                Return load画像(item)

            Case ImageTypeEnum._底楕円
                Return draw底楕円(item)

            Case ImageTypeEnum._付属品
                Return draw付属品(item)

            Case ImageTypeEnum._横軸線
                Return draw軸線(item)

            Case ImageTypeEnum._縦軸線
                Return draw軸線(item)

            Case Else
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "CImageDraw.DrawItem:No Def({0}:{1})", item.m_ImageType, item)
                Return False
        End Select
    End Function

    Function draw横バンド(ByVal item As clsImageItem) As Boolean
        Return subバンド(item, False)
    End Function

    Function draw縦バンド(ByVal item As clsImageItem) As Boolean
        Return subバンド(item, True)
    End Function

    Function subバンド(ByVal item As clsImageItem, ByVal isVirtical As Boolean) As Boolean
        If item.m_row縦横展開 Is Nothing Then
            Return False
        End If
        Dim colset As CPenBrush = GetBandPenBrush(item.m_row縦横展開.f_s色)
        If colset Is Nothing OrElse colset.IsNoDrawing Then
            Return False
        End If

        'CBand描画に移行中
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "★ subバンド isVirtical:{0},f_s記号:{1}", isVirtical, item.m_row縦横展開.f_s記号)
        'If item.m_regionList IsNot Nothing Then
        '    g_clsLog.LogFormatMessage(clsLog.LogLevel.Basic, "× m_regionList({0})", item.m_regionList.Count)
        'End If

        Dim ret As Boolean = True
        Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
        If (isVirtical And (0 < rect.Height)) OrElse (Not isVirtical And (0 < rect.Width)) Then
            Dim widraw As Single
            If isVirtical Then
                widraw = pixcel_width(item.m_rひも位置.x幅)
            Else
                widraw = pixcel_height(item.m_rひも位置.y高さ)
            End If

            'If item.m_regionList IsNot Nothing AndAlso 0 < item.m_regionList.Count Then
            '    '白抜き箇所がある
            '    If isVirtical Then
            '        '縦ひも
            '        Dim sublist As C領域リスト = item.m_regionList.CrossingX(item.m_rひも位置.x最左, item.m_rひも位置.x最右)
            '        If 0 < sublist.Count Then
            '            sublist.SortByY()
            '            'Debug.Print(sublist.ToString)
            '            Dim YStart As Double = item.m_rひも位置.y最下
            '            Dim YEnd As Double = item.m_rひも位置.y最上
            '            Dim borderひも As DirectionEnum = item.m_borderひも And Not (DirectionEnum._上)

            '            Dim part_r As S領域 = item.m_rひも位置
            '            For Each r As S領域 In sublist
            '                If r.y最下 < YStart Then
            '                    '範囲より下なのでスルー
            '                ElseIf YEnd < r.y最上 Then
            '                    '範囲より上なので終わり
            '                    Exit For
            '                Else
            '                    '範囲内
            '                    part_r.y最上 = r.y最下
            '                    ret = ret And partバンド(isVirtical, colset, pixcel_rectangle(part_r),
            '                                          widraw, item.m_row縦横展開.f_i何本幅, borderひも)
            '                    part_r.y最下 = r.y最上
            '                    borderひも = borderひも And Not (DirectionEnum._下)
            '                End If
            '            Next
            '            '
            '            borderひも = borderひも Or (item.m_borderひも And (DirectionEnum._上))
            '            part_r.p右上.Y = YEnd
            '            ret = ret And partバンド(isVirtical, colset, pixcel_rectangle(part_r),
            '                                          widraw, item.m_row縦横展開.f_i何本幅, borderひも)

            '        Else
            '            ret = ret And partバンド(isVirtical, colset, rect, widraw, item.m_row縦横展開.f_i何本幅, item.m_borderひも)
            '        End If
            '    Else
            '        '横ひも
            '        Dim sublist As C領域リスト = item.m_regionList.CrossingY(item.m_rひも位置.y最下, item.m_rひも位置.y最上)
            '        If 0 < sublist.Count Then
            '            sublist.SortByX()
            '            'Debug.Print(sublist.ToString)
            '            Dim XStart As Double = item.m_rひも位置.x最左
            '            Dim XEnd As Double = item.m_rひも位置.x最右
            '            Dim borderひも As DirectionEnum = item.m_borderひも And Not (DirectionEnum._右)

            '            Dim part_r As S領域 = item.m_rひも位置
            '            For Each r As S領域 In sublist
            '                If r.x最左 < XStart Then
            '                    '範囲より左なのでスルー
            '                ElseIf XEnd < r.x最右 Then
            '                    '範囲より右なので終わり
            '                    Exit For
            '                Else
            '                    '範囲内
            '                    part_r.x最右 = r.x最左
            '                    ret = ret And partバンド(isVirtical, colset, pixcel_rectangle(part_r),
            '                                          widraw, item.m_row縦横展開.f_i何本幅, borderひも)
            '                    part_r.x最左 = r.x最右
            '                    borderひも = borderひも And Not (DirectionEnum._左)
            '                End If
            '            Next
            '            '
            '            borderひも = borderひも Or (item.m_borderひも And (DirectionEnum._右))
            '            part_r.p右上.X = XEnd
            '            ret = ret And partバンド(isVirtical, colset, pixcel_rectangle(part_r),
            '                                          widraw, item.m_row縦横展開.f_i何本幅, borderひも)
            '        Else
            '            ret = ret And partバンド(isVirtical, colset, rect, widraw, item.m_row縦横展開.f_i何本幅, item.m_borderひも)
            '        End If
            '    End If

            'Else
            '1本だけのひも
            ret = ret And partバンド(isVirtical, colset, rect, widraw, item.m_row縦横展開.f_i何本幅, item.m_borderひも)
            'End If
        End If

        '記号
        If Not item.m_bNoMark Then
            If isVirtical Then
                '記号を上に
                If Not String.IsNullOrWhiteSpace(item.m_row縦横展開.f_s記号) AndAlso colset.BrushSolid IsNot Nothing Then
                    Dim pMark As New PointF(rect.X + 2, rect.Y - _FontSize * 1.5)
                    _Graphic.DrawString(item.m_row縦横展開.f_s記号, _Font, colset.BrushSolid, pMark)
                End If
            Else
                '記号を左に
                If Not String.IsNullOrWhiteSpace(item.m_row縦横展開.f_s記号) AndAlso colset.BrushSolid IsNot Nothing Then
                    Dim pMark As New PointF(rect.X - _FontSize * 2.7, rect.Y + 3)
                    _Graphic.DrawString(item.m_row縦横展開.f_s記号, _Font, colset.BrushSolid, pMark)
                End If
            End If
        End If
        Return True
    End Function

    Private Function partバンド(ByVal isVirtical As Boolean, ByVal colset As CPenBrush, ByVal rect As RectangleF, ByVal widraw As Single, ByVal i何本幅 As Integer, ByVal borderひも As DirectionEnum) As Boolean
        Dim widHalf As Single = colset.PenWidth / 2

        '塗りつぶし
        If colset.BrushAlfa IsNot Nothing Then
            _Graphic.FillRectangle(colset.BrushAlfa, rect.X + widHalf, rect.Y + widHalf, rect.Width - colset.PenWidth, rect.Height - colset.PenWidth)
        End If

        '本幅線
        Dim wid As Single = (widraw - colset.PenWidth) / i何本幅
        If colset.PenLane IsNot Nothing AndAlso 1 < i何本幅 Then
            If isVirtical Then
                For i As Integer = 1 To i何本幅 - 1
                    Dim p上 As New PointF(rect.X + wid * i + widHalf, rect.Y + widHalf)
                    Dim p下 As New PointF(rect.X + wid * i + widHalf, rect.Y + rect.Height - colset.PenWidth)
                    _Graphic.DrawLine(colset.PenLane, p上, p下)
                Next
            Else
                For i As Integer = 1 To i何本幅 - 1
                    Dim p左 As New PointF(rect.X + widHalf, rect.Y + wid * i + widHalf)
                    Dim p右 As New PointF(rect.X + rect.Width - colset.PenWidth, rect.Y + wid * i + widHalf)
                    _Graphic.DrawLine(colset.PenLane, p左, p右)
                Next
            End If
        End If

        'バンド枠
        If colset.PenBand IsNot Nothing Then
            If borderひも = cDirectionEnumAll Then
                _Graphic.DrawRectangle(colset.PenBand, rect.X + widHalf, rect.Y + widHalf, rect.Width - colset.PenWidth, rect.Height - colset.PenWidth)
            Else
                If borderひも.HasFlag(DirectionEnum._上) Then
                    _Graphic.DrawLine(colset.PenBand,
                                          rect.X + widHalf, rect.Y + widHalf,
                                          rect.X + rect.Width - widHalf, rect.Y + widHalf)
                End If
                If borderひも.HasFlag(DirectionEnum._左) Then
                    _Graphic.DrawLine(colset.PenBand,
                                          rect.X + widHalf, rect.Y + widHalf,
                                          rect.X + widHalf, rect.Y + rect.Height - widHalf)
                End If
                If borderひも.HasFlag(DirectionEnum._下) Then
                    _Graphic.DrawLine(colset.PenBand,
                                          rect.X + widHalf, rect.Y + rect.Height - widHalf,
                                          rect.X + rect.Width - widHalf, rect.Y + rect.Height - widHalf)
                End If
                If borderひも.HasFlag(DirectionEnum._右) Then
                    _Graphic.DrawLine(colset.PenBand,
                                          rect.X + rect.Width - widHalf, rect.Y + widHalf,
                                          rect.X + rect.Width - widHalf, rect.Y + rect.Height - widHalf)
                End If
            End If
        End If
        Return True
    End Function

    Function drawバンドセット(ByVal item As clsImageItem) As Boolean

        '描かない領域を反転するための新たな領域を作成
        Dim invertedClip As Region
        If item.m_rDraw.IsEmpty Then
            invertedClip = New Region(New Rectangle(0, 0, Canvas.Size.Width, Canvas.Size.Height))
        Else
            invertedClip = New Region(pixcel_rectangle(item.m_rDraw))
        End If

        If item.m_clipList IsNot Nothing AndAlso 0 < item.m_clipList.Count Then
            For Each clip As CBand In item.m_clipList
                If clip Is Nothing Then
                    Continue For
                End If
                Dim colset As CPenBrush = GetBandPenBrush(clip._s色)
                If colset Is Nothing OrElse colset.IsNoDrawing Then
                    Continue For
                End If
                'ひもの領域
                Dim points() As PointF = pixcel_lines(clip.aバンド位置)
                Dim path As New GraphicsPath()
                path.AddPolygon(points)

                '反転領域から除外する
                invertedClip.Exclude(path)
            Next
        End If
        '円は1点のみ
        If item.m_is円 Then
            Dim rect As RectangleF = pixcel_rectangle(item.m_a四隅.r外接領域)
            Dim path As New GraphicsPath()
            path.AddArc(rect, 0, 360)
            invertedClip.Exclude(path)
        End If

        ' クリップ領域を新たな領域に設定
        _Graphic.Clip = invertedClip

        'ひも描画
        Dim ret As Boolean = True
        If item.m_bandList IsNot Nothing AndAlso 0 < item.m_bandList.Count Then
            For Each band As CBand In item.m_bandList
                ret = ret And drawバンド(band)
            Next
        End If

        ' クリップ領域を解除する
        _Graphic.ResetClip()

        Return ret
    End Function

    Function drawバンド(ByVal band As CBand) As Boolean
        If band Is Nothing Then
            Return False
        End If
        Dim colset As CPenBrush = GetBandPenBrush(band._s色)
        If colset Is Nothing OrElse colset.IsNoDrawing Then
            Return False
        End If

        Dim ret As Boolean = True
        'ひもの領域
        Dim points() As PointF = pixcel_lines(band.aバンド位置)
        If colset.BrushAlfa IsNot Nothing Then
            _Graphic.FillPolygon(colset.BrushAlfa, points)
        End If

        Dim p始点F As PointF = points(CBand.i_始点F)
        Dim p始点T As PointF = points(CBand.i_始点T)
        Dim p終点F As PointF
        Dim p終点T As PointF

        'バンドの枠線幅 ※pixcel変換により角度が変わるので再計算
        Dim ftlen As Single = Math.Sqrt((p始点T.X - p始点F.X) ^ 2 + (p始点T.Y - p始点F.Y) ^ 2)
        Dim wid_dx As Single = colset.PenWidth * (p始点T.X - p始点F.X) / ftlen
        Dim wid_dy As Single = colset.PenWidth * (p始点T.Y - p始点F.Y) / ftlen

        'バンド方向に、枠線幅分内側に入ったライン
        p始点F = New PointF(points(CBand.i_始点F).X + wid_dx, points(CBand.i_始点F).Y + wid_dy)
        p始点T = New PointF(points(CBand.i_始点T).X - wid_dx, points(CBand.i_始点T).Y - wid_dy)
        p終点F = New PointF(points(CBand.i_終点F).X + wid_dx, points(CBand.i_終点F).Y + wid_dy)
        'p終点T = New PointF(points(CBand.i_終点T).X - wid_dx, points(CBand.i_終点T).Y - wid_dy)

        '本幅線
        Dim i何本幅 As Integer = band._i何本幅
        If colset.PenLane IsNot Nothing AndAlso 1 < i何本幅 Then
            Dim dx As Single = (p始点T.X - p始点F.X) / i何本幅
            Dim dy As Single = (p始点T.Y - p始点F.Y) / i何本幅
            Dim pStart As PointF = p始点F
            Dim pEnd As PointF = p終点F
            For i As Integer = 1 To i何本幅 - 1
                pStart.X += dx
                pStart.Y += dy
                pEnd.X += dx
                pEnd.Y += dy
                _Graphic.DrawLine(colset.PenLane, pStart, pEnd)
            Next
        End If

        'バンド枠
        If colset.PenBand IsNot Nothing Then
            Dim wid_half_dx As Single = wid_dx / 2
            Dim wid_half_dy As Single = wid_dy / 2
            'バンド方向に、1/2枠線幅分内側に入ったライン
            p始点F = New PointF(points(CBand.i_始点F).X + wid_half_dx, points(CBand.i_始点F).Y + wid_half_dy)
            p始点T = New PointF(points(CBand.i_始点T).X - wid_half_dx, points(CBand.i_始点T).Y - wid_half_dy)
            p終点F = New PointF(points(CBand.i_終点F).X + wid_half_dx, points(CBand.i_終点F).Y + wid_half_dy)
            p終点T = New PointF(points(CBand.i_終点T).X - wid_half_dx, points(CBand.i_終点T).Y - wid_half_dy)
            _Graphic.DrawLine(colset.PenBand, p始点F, p終点F)
            _Graphic.DrawLine(colset.PenBand, p始点T, p終点T)

            If band.is始点FT線 Then
                _Graphic.DrawLine(colset.PenBand, points(CBand.i_始点F), points(CBand.i_始点T))
            End If
            If band.is終点FT線 Then
                _Graphic.DrawLine(colset.PenBand, points(CBand.i_終点F), points(CBand.i_終点T))
            End If
        End If

        '記号
        If Not band.p文字位置.IsZero AndAlso
            Not String.IsNullOrWhiteSpace(band._s記号) AndAlso
            colset.BrushSolid IsNot Nothing Then
            Dim p As PointF = pixcel_point(band.p文字位置)
            Dim pMark As New PointF(p.X + 2, p.Y - _FontSize * 1.5)
            _Graphic.DrawString(band._s記号, _Font, colset.BrushSolid, pMark)
        End If
        Return ret
    End Function


    Private Sub subコマ(colset As CPenBrush, a As S四隅, l As S線分)
        Dim polygon As PointF() = pixcel_lines(a)
        '塗りつぶし
        If colset.BrushAlfa IsNot Nothing Then
            _Graphic.FillPolygon(colset.BrushAlfa, polygon)
        End If

        'バンド
        If colset.PenBand IsNot Nothing Then
            _Graphic.DrawPolygon(colset.PenBand, polygon)
        End If

        '線
        If colset.PenLane IsNot Nothing Then
            _Graphic.DrawLine(colset.PenLane, pixcel_point(l.p開始), pixcel_point(l.p終了))
        End If
    End Sub

    Function drawコマ(ByVal item As clsImageItem) As Boolean
        If item.m_row縦横展開 Is Nothing OrElse item.m_row縦横展開2 Is Nothing Then
            Return False
        End If
        Dim colsetYoko As CPenBrush = GetBandPenBrush(item.m_row縦横展開.f_s色)
        Dim colsetTate As CPenBrush = GetBandPenBrush(item.m_row縦横展開2.f_s色)
        If colsetTate Is Nothing OrElse colsetYoko Is Nothing Then
            Return False
        End If

        If item.m_knot.IsDrawArea Then
            Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
            _Graphic.DrawRectangle(_Pen_black_dot, rect.X, rect.Y, rect.Width, rect.Height)
        End If
        If Not colsetYoko.IsNoDrawing Then
            subコマ(colsetYoko, item.m_knot.a右上四角, item.m_knot.l右上線)
            subコマ(colsetYoko, item.m_knot.a左下四角, item.m_knot.l左下線)
        End If
        If Not colsetTate.IsNoDrawing Then
            subコマ(colsetTate, item.m_knot.a左上四角, item.m_knot.l左上線)
            subコマ(colsetTate, item.m_knot.a右下四角, item.m_knot.l右下線)
        End If
        Return True
    End Function

    Function draw底枠(ByVal item As clsImageItem) As Boolean
        Dim ret As Boolean = True
        If item.m_is円 Then
            ret = ret And draw_ellipse(item.m_a四隅.r外接領域, LineTypeEnum._black_thick)
        Else
            ret = ret And draw_area(item.m_a四隅, LineTypeEnum._black_thick)
        End If
        ret = ret And draw_linelist(item.m_lineList, LineTypeEnum._black_dot)
        Return True
    End Function

    Function draw底枠2(ByVal item As clsImageItem) As Boolean
        Return draw_linelist(item.m_lineList, LineTypeEnum._black_thick)
    End Function

    Function draw側面(ByVal item As clsImageItem) As Boolean
        Dim ret As Boolean = True
        ret = ret And draw_area(item.m_a四隅, LineTypeEnum._black_thin)
        ret = ret And draw_linelist(item.m_lineList, LineTypeEnum._black_dot)
        Return ret
    End Function

    Function draw四隅領域(ByVal item As clsImageItem) As Boolean
        Dim ret As Boolean = True
        If item.m_is円 Then
            ret = ret And draw_ellipse(item.m_a四隅.r外接領域, LineTypeEnum._black_thin)
        Else
            ret = ret And draw_area(item.m_a四隅, LineTypeEnum._black_thin)
        End If
        ret = ret And draw_linelist(item.m_lineList, LineTypeEnum._black_dot)
        Return ret
    End Function

    Function draw四隅領域線(ByVal item As clsImageItem) As Boolean
        Dim ret As Boolean = True
        If item.m_is円 Then
            ret = ret And draw_ellipse(item.m_a四隅.r外接領域, item.m_ltype)
        Else
            ret = ret And draw_area(item.m_a四隅, item.m_ltype)
        End If
        ret = ret And draw_linelist(item.m_lineList, item.m_ltype)
        Return ret
    End Function

    Function draw底の中央線(ByVal item As clsImageItem) As Boolean
        Return draw_linelist(item.m_lineList, LineTypeEnum._red)
    End Function

    Function draw折り返し線(ByVal item As clsImageItem) As Boolean
        Return draw_linelist(item.m_lineList, LineTypeEnum._black_thick)
    End Function

    Private Function draw_linelist(ByVal lineList As C線分リスト, ByVal ltype As LineTypeEnum)
        For Each line As S線分 In lineList
            Select Case ltype
                Case LineTypeEnum._black_thin
                    _Graphic.DrawLine(_Pen_black_thin, pixcel_point(line.p開始), pixcel_point(line.p終了))
                Case LineTypeEnum._black_thick
                    _Graphic.DrawLine(_Pen_black_thick, pixcel_point(line.p開始), pixcel_point(line.p終了))
                Case LineTypeEnum._black_dot
                    _Graphic.DrawLine(_Pen_black_dot, pixcel_point(line.p開始), pixcel_point(line.p終了))
                Case LineTypeEnum._red
                    _Graphic.DrawLine(_Pen_red, pixcel_point(line.p開始), pixcel_point(line.p終了))
                Case Else 'nodef
                    Return False
            End Select
        Next
        Return True
    End Function

    Private Function draw_area(ByVal area As S四隅, ByVal ltype As LineTypeEnum)
        If area.IsEmpty Then
            Return True
        End If
        Dim points() As PointF = pixcel_lines(area)
        Select Case ltype
            Case LineTypeEnum._black_thin
                _Graphic.DrawLines(_Pen_black_thin, points)
            Case LineTypeEnum._black_thick
                _Graphic.DrawLines(_Pen_black_thick, points)
            Case LineTypeEnum._black_dot
                _Graphic.DrawLines(_Pen_black_dot, points)
            Case LineTypeEnum._red
                _Graphic.DrawLines(_Pen_red, points)
            Case Else 'nodef
                Return False
        End Select
        Return True
    End Function

    Private Function draw_ellipse(ByVal region As S領域, ByVal ltype As LineTypeEnum)
        If region.IsEmpty Then
            Return True
        End If
        Dim rect As RectangleF = pixcel_rectangle(region)
        Select Case ltype
            Case LineTypeEnum._black_thin
                _Graphic.DrawEllipse(_Pen_black_thin, rect)
            Case LineTypeEnum._black_thick
                _Graphic.DrawEllipse(_Pen_black_thick, rect)
            Case LineTypeEnum._black_dot
                _Graphic.DrawEllipse(_Pen_black_dot, rect)
            Case LineTypeEnum._red
                _Graphic.DrawEllipse(_Pen_red, rect)
            Case Else 'nodef
                Return False
        End Select
        Return True
    End Function


    Function draw文字列(ByVal item As clsImageItem) As Boolean
        If item.m_aryString Is Nothing OrElse item.m_sizeFont <= 0 Then
            Return False
        End If

        'フォントオブジェクトの作成
        Dim fontsize As Single = pixcel_width(item.m_sizeFont)
        Dim font = New Font(My.Resources.FontNameString, fontsize)
        Dim p As PointF = pixcel_point(item.p_p文字位置)
        For Each str As String In item.m_aryString
            _Graphic.DrawString(str, font, _Brush_black, p)
            p.Y += fontsize * 2
        Next
        font.Dispose()
        Return True
    End Function

    Function draw編みかた(ByVal item As clsImageItem) As Boolean
        If item.m_groupRow Is Nothing Then
            Return False
        End If
        'ひも番号1の色
        Dim color As String = item.m_groupRow.GetIndexNameValue(1, "f_s色")
        Dim colset As CPenBrush = GetBandPenBrush(color)
        If colset Is Nothing OrElse colset.IsNoDrawing Then
            Return False
        End If
        '同じ編みかたの領域
        Dim points() As PointF = pixcel_lines(item.m_a四隅)
        If colset.BrushAlfa IsNot Nothing Then
            _Graphic.FillPolygon(colset.BrushAlfa, points)
        End If
        _Graphic.DrawLines(colset.PenBand, points)
        '周の線
        If colset.PenLane IsNot Nothing Then
            For Each line As S線分 In item.m_lineList
                _Graphic.DrawLine(colset.PenLane, pixcel_point(line.p開始), pixcel_point(line.p終了))
            Next
        End If
        '編みかた名
        If Not item.p_p文字位置.IsZero AndAlso colset.BrushSolid IsNot Nothing Then
            Dim p As PointF = pixcel_point(item.p_p文字位置)
            Dim str As String = item.m_groupRow.GetNameValueSum("f_s記号")
            str += item.m_groupRow.GetNameValue("f_s編みかた名")
            _Graphic.DrawString(str, _Font, colset.BrushSolid, p)
        End If

        Return True
    End Function

    Function draw底楕円(ByVal item As clsImageItem) As Boolean
        Dim colset As CPenBrush = Nothing
        Dim laps As Integer = 1 '周数
        If item.m_groupRow IsNot Nothing Then
            'ひも番号1の色
            Dim color As String = item.m_groupRow.GetIndexNameValue(1, "f_s色")
            colset = GetBandPenBrush(color)
            If colset Is Nothing OrElse colset.IsNoDrawing Then
                Return False
            End If
            laps = item.m_groupRow.GetIndexNameValue(1, "f_i周数")
        Else
            '対応レコードがない場合は底枠として描く
            laps = 1
        End If


        If item.m_is円 Then
            '円/楕円           ・╭ ╮・ m_a四隅に内接
            '                  ・╰ ╯・
            Dim r外接 As S領域 = item.m_a四隅.r外接領域
            Dim rect As RectangleF = pixcel_rectangle(r外接)
            If colset Is Nothing Then
                '底枠
                _Graphic.DrawEllipse(_Pen_black_thick, rect)
                Return True

            Else
                If colset.PenBand IsNot Nothing Then
                    _Graphic.DrawEllipse(colset.PenBand, rect)
                End If
                '塗りつぶし幅の指定があれば
                If 0 < item.m_dひも幅 Then
                    '内側を塗りつぶす
                    Dim r内側 As New S領域(r外接)
                    If colset.BrushAlfa IsNot Nothing Then
                        r内側.enLarge(-item.m_dひも幅)
                        If r内側.x幅 = 0 OrElse r内側.y高さ = 0 Then
                            '円を塗りつぶす
                            _Graphic.FillEllipse(colset.BrushAlfa, rect)
                        Else
                            'ドーナツ
                            Dim path As New GraphicsPath()
                            path.AddEllipse(rect)
                            Dim inner As RectangleF = pixcel_rectangle(r内側)
                            path.AddEllipse(inner)
                            _Graphic.FillPath(colset.BrushAlfa, path)
                        End If

                    End If
                    '複数周
                    If 1 < laps AndAlso colset.PenLane IsNot Nothing Then
                        r内側 = New S領域(r外接)
                        Dim dlap As Double = item.m_dひも幅 / laps
                        For i As Integer = 1 To laps - 1
                            r内側.enLarge(-dlap)
                            If r内側.x幅 = 0 OrElse r内側.y高さ = 0 Then
                                Exit For
                            End If
                            _Graphic.DrawEllipse(colset.PenLane, pixcel_rectangle(r内側))
                        Next
                    End If
                End If
            End If

        Else
            '底楕円              m_lineList(0)          ※上下左右対称を前提とする
            '                 ╭    ←─    ╮
            '                  1・      ・0 
            '   m_lineList(1)↓  m_a四隅   ↑m_lineList(3) 
            '                  2・      ・3 
            '                 ╰    ─→    ╯
            '                    m_lineList(2)

            Dim rサイズ As S領域
            '上下左右対称のため右上領域から取得
            rサイズ.x幅 = (item.m_lineList(3).p終了.X - item.m_a四隅.p右上.X) * 2
            rサイズ.y高さ = (item.m_lineList(0).p開始.Y - item.m_a四隅.p右上.Y) * 2
            Dim path As GraphicsPath = getpath(item.m_a四隅, rサイズ)

            If colset Is Nothing Then
                '底枠
                _Graphic.DrawPath(_Pen_black_thick, path)
                Return True

            Else
                '楕円底
                If colset.PenBand IsNot Nothing Then
                    _Graphic.DrawPath(colset.PenBand, path)
                End If

                '塗りつぶし幅の指定があれば
                If 0 < item.m_dひも幅 Then
                    Dim r内サイズ As New S領域(rサイズ)
                    If colset.BrushAlfa IsNot Nothing Then
                        r内サイズ.enLarge(-item.m_dひも幅)
                        '幅分小さい楕円
                        Dim inner As GraphicsPath = getpath(item.m_a四隅, r内サイズ)
                        '塗りつぶし
                        Dim clipregion As New Region(path)
                        clipregion.Exclude(inner)
                        _Graphic.SetClip(clipregion, CombineMode.Replace)
                        _Graphic.FillPath(colset.BrushAlfa, path)
                        _Graphic.ResetClip()
                    End If

                    '複数周
                    If 1 < laps AndAlso colset.PenLane IsNot Nothing Then
                        r内サイズ = New S領域(rサイズ)
                        Dim dlap As Double = item.m_dひも幅 / laps
                        For i As Integer = 1 To laps - 1
                            r内サイズ.enLarge(-dlap)
                            Dim lappath As GraphicsPath = getpath(item.m_a四隅, r内サイズ)
                            _Graphic.DrawPath(colset.PenLane, lappath)
                        Next
                    End If
                End If
            End If
        End If

        '編みかた名
        If Not item.p_p文字位置.IsZero AndAlso
            colset IsNot Nothing AndAlso colset.BrushSolid IsNot Nothing Then
            Dim p As PointF = pixcel_point(item.p_p文字位置)
            Dim str As String = item.m_groupRow.GetNameValueSum("f_s記号")
            str += item.m_groupRow.GetNameValue("f_s編みかた名")
            _Graphic.DrawString(str, _Font, colset.BrushSolid, p)
        End If

        Return True
    End Function

    Private Function getpath(ByVal a弧の中心 As S四隅, ByVal r外接サイズ As S領域) As GraphicsPath
        Dim path As New GraphicsPath()
        '楕円弧 ※座標が上下逆になるため、角度も逆回りになります
        Dim centers() As PointF = pixcel_lines(a弧の中心)
        Dim rx As Single = pixcel_width(r外接サイズ.x幅 / 2)
        Dim ry As Single = pixcel_height(r外接サイズ.y高さ / 2)
        '右上
        If rx = 0 Then
            path.AddLine(centers(0).X, centers(0).Y - ry, centers(0).X, centers(0).Y)
        ElseIf ry = 0 AndAlso 0 < rx Then
            path.AddLine(centers(0).X, centers(0).Y, centers(0).X + rx, centers(0).Y)
        Else
            path.AddArc(centers(0).X - rx, centers(0).Y - ry, rx * 2, ry * 2, 270, 90)
        End If
        path.AddLine(centers(0).X + rx, centers(0).Y, centers(3).X + rx, centers(3).Y)

        '右下
        If rx = 0 Then
            path.AddLine(centers(3).X, centers(3).Y, centers(3).X, centers(3).Y + ry)
        ElseIf ry = 0 AndAlso 0 < rx Then
            path.AddLine(centers(3).X + rx, centers(3).Y, centers(3).X, centers(3).Y)
        Else
            path.AddArc(centers(3).X - rx, centers(3).Y - ry, rx * 2, ry * 2, 0, 90)
        End If
        path.AddLine(centers(3).X, centers(3).Y + ry, centers(2).X, centers(2).Y + ry)

        '左下
        If rx = 0 Then
            path.AddLine(centers(2).X, centers(2).Y + ry, centers(2).X, centers(2).Y)
        ElseIf ry = 0 AndAlso 0 < rx Then
            path.AddLine(centers(2).X, centers(2).Y, centers(2).X - rx, centers(2).Y)
        Else
            path.AddArc(centers(2).X - rx, centers(2).Y - ry, rx * 2, ry * 2, 90, 90)
        End If
        path.AddLine(centers(2).X - rx, centers(2).Y, centers(1).X - rx, centers(1).Y)

        '左上
        If rx = 0 Then
            path.AddLine(centers(1).X, centers(1).Y, centers(1).X, centers(1).Y - ry)
        ElseIf ry = 0 AndAlso 0 < rx Then
            path.AddLine(centers(1).X - rx, centers(1).Y, centers(1).X, centers(1).Y)
        Else
            path.AddArc(centers(1).X - rx, centers(1).Y - ry, rx * 2, ry * 2, 180, 90)
        End If
        path.AddLine(centers(1).X, centers(1).Y - ry, centers(0).X, centers(0).Y - ry)

        path.CloseFigure() ' 閉じる

        Return path
    End Function

    Function draw付属品(ByVal item As clsImageItem) As Boolean
        If item.m_row追加品 Is Nothing Then
            Return False
        End If
        '色
        Dim color As String = item.m_row追加品.f_s色
        Dim colset As CPenBrush = GetBandPenBrush(color)
        If colset Is Nothing OrElse colset.IsNoDrawing Then
            Return False
        End If

        Dim ret As Boolean = True
        Select Case item.m_row追加品.f_i描画形状
            Case enum描画形状.i_横バンド
                Dim band As New CBand(item.m_row追加品)
                band.SetBandF(New S線分(item.m_rひも位置.p左下, item.m_rひも位置.p右下), item.m_dひも幅, Unit90)
                ret = drawバンド(band)

            Case enum描画形状.i_横四角

                Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
                If 0 < item.m_dひも幅 Then
                    If colset.BrushAlfa IsNot Nothing Then
                        _Graphic.FillRectangle(colset.BrushAlfa, rect)
                    End If
                    If colset.PenBand IsNot Nothing Then
                        _Graphic.DrawRectangle(colset.PenBand, rect.X, rect.Y, rect.Width, rect.Height)
                    End If
                Else
                    If colset.PenBand IsNot Nothing Then
                        _Graphic.DrawLine(colset.PenBand, New Point(rect.Left, rect.Top), New Point(rect.Right, rect.Top))
                    End If
                End If

            Case enum描画形状.i_正方形_辺, enum描画形状.i_長方形_横, enum描画形状.i_正方形_周, enum描画形状.i_長方形_周

                If 0 < item.m_dひも幅 AndAlso item.m_dひも幅 < item.m_rひも位置.x幅 AndAlso item.m_dひも幅 < item.m_rひも位置.y高さ Then
                    Dim r外 As S領域 = item.m_rひも位置.get拡大領域(item.m_dひも幅 / 2)
                    Dim r内 As S領域 = item.m_rひも位置.get拡大領域(-item.m_dひも幅 / 2)
                    Dim rect外 As RectangleF = pixcel_rectangle(r外)
                    Dim rect内 As RectangleF = pixcel_rectangle(r内)

                    Dim path As New GraphicsPath()
                    path.AddRectangle(rect外)
                    path.AddRectangle(rect内)
                    path.FillMode = FillMode.Alternate
                    If colset.BrushAlfa IsNot Nothing Then
                        _Graphic.FillPath(colset.BrushAlfa, path)
                    End If
                    If colset.PenBand IsNot Nothing Then
                        _Graphic.DrawPath(colset.PenBand, path)
                    End If
                Else
                    Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
                    If colset.PenBand IsNot Nothing Then
                        _Graphic.DrawRectangle(colset.PenBand, rect.X, rect.Y, rect.Width, rect.Height)
                    End If
                End If

            Case enum描画形状.i_円_径, enum描画形状.i_楕円_横径, enum描画形状.i_円_周, enum描画形状.i_楕円_周

                If 0 < item.m_dひも幅 AndAlso item.m_dひも幅 < item.m_rひも位置.x幅 AndAlso item.m_dひも幅 < item.m_rひも位置.y高さ Then
                    Dim r外 As S領域 = item.m_rひも位置.get拡大領域(item.m_dひも幅 / 2)
                    Dim r内 As S領域 = item.m_rひも位置.get拡大領域(-item.m_dひも幅 / 2)
                    Dim rect外 As RectangleF = pixcel_rectangle(r外)
                    Dim rect内 As RectangleF = pixcel_rectangle(r内)

                    Dim path As New GraphicsPath()
                    path.AddEllipse(rect外)
                    path.AddEllipse(rect内)
                    path.FillMode = FillMode.Alternate
                    If colset.BrushAlfa IsNot Nothing Then
                        _Graphic.FillPath(colset.BrushAlfa, path)
                    End If
                    If colset.PenBand IsNot Nothing Then
                        _Graphic.DrawPath(colset.PenBand, path)
                    End If
                Else
                    Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
                    If colset.PenBand IsNot Nothing Then
                        _Graphic.DrawEllipse(colset.PenBand, rect.X, rect.Y, rect.Width, rect.Height)
                    End If
                End If

            Case enum描画形状.i_上半円_径, enum描画形状.i_上半円_周

                Dim r円 As S領域 = item.m_rひも位置 '上半分
                r円.y最下 -= r円.y高さ

                If 0 < item.m_dひも幅 AndAlso item.m_dひも幅 < r円.x幅 AndAlso item.m_dひも幅 < r円.y高さ Then
                    Dim r外 As S領域 = r円.get拡大領域(item.m_dひも幅 / 2)
                    Dim r内 As S領域 = r円.get拡大領域(-item.m_dひも幅 / 2)
                    Dim rect外 As RectangleF = pixcel_rectangle(r外)
                    Dim rect内 As RectangleF = pixcel_rectangle(r内)

                    If colset.BrushAlfa IsNot Nothing Then
                        Dim path As New GraphicsPath()
                        path.AddArc(rect外, 180, 180)
                        path.AddArc(rect内, 180, 180)
                        path.FillMode = FillMode.Alternate
                        _Graphic.FillPath(colset.BrushAlfa, path)
                    End If
                    If colset.PenBand IsNot Nothing Then
                        Dim path外 As New GraphicsPath()
                        path外.AddArc(rect外, 180, 180)
                        path外.FillMode = FillMode.Alternate
                        _Graphic.DrawPath(colset.PenBand, path外)
                        Dim path内 As New GraphicsPath()
                        path内.AddArc(rect内, 180, 180)
                        path内.FillMode = FillMode.Alternate
                        _Graphic.DrawPath(colset.PenBand, path内)
                    End If
                Else
                    Dim rect As RectangleF = pixcel_rectangle(r円)
                    If colset.PenBand IsNot Nothing Then
                        _Graphic.DrawArc(colset.PenBand, rect, 180, 180)
                    End If
                End If

            Case enum描画形状.i_横線
                Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
                If colset.PenBand IsNot Nothing Then
                    _Graphic.DrawLine(colset.PenBand, New Point(rect.Left, rect.Top), New Point(rect.Right, rect.Top))
                End If

            Case enum描画形状.i_縦線
                Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
                If colset.PenBand IsNot Nothing Then
                    _Graphic.DrawLine(colset.PenBand, New Point(rect.Left, rect.Top), New Point(rect.Left, rect.Bottom))
                End If

        End Select

        '付属品名
        If Not item.p_p文字位置.IsZero AndAlso colset.BrushSolid IsNot Nothing Then
            Dim p As PointF = pixcel_point(item.p_p文字位置)
            Dim str As String = item.m_row追加品.f_s記号
            str += item.m_row追加品.f_s付属品名
            If Not String.IsNullOrWhiteSpace(item.m_row追加品.f_s付属品ひも名) Then
                str += "/"
                str += item.m_row追加品.f_s付属品ひも名
            End If
            _Graphic.DrawString(str, _Font, colset.BrushSolid, p)
        End If

        Return ret
    End Function

    Function draw軸線(ByVal item As clsImageItem) As Boolean
        For Each line As S線分 In item.m_lineList
            _Graphic.DrawLine(_Pen_black_dot, pixcel_point(line.p開始), pixcel_point(line.p終了))
        Next
        Return True
    End Function



    'ファイル保存
    Function FileSave(ByVal fpath As String) As Boolean
        If Canvas Is Nothing Then
            Return False
        End If
        Dim ext As String = IO.Path.GetExtension(fpath)
        If String.IsNullOrWhiteSpace(ext) Then
            Return False
        End If

        Dim fmt As ImageFormat
        Select Case ext.ToLower
            Case ".bmp"
                fmt = ImageFormat.Bmp
            Case ".gif"
                fmt = ImageFormat.Gif
            Case ".png"
                fmt = ImageFormat.Png
            Case ".tiff", ".tif"
                fmt = ImageFormat.Tiff
            Case ".jpg"
                fmt = ImageFormat.Jpeg
            Case Else
                Return False
        End Select

        Try
            Canvas.Save(fpath, fmt)
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "CImageDraw.FileSave")
            Return False

        End Try
    End Function


    '領域を画像保存
    Function save画像(ByVal item As clsImageItem) As Boolean
        If Canvas Is Nothing OrElse item Is Nothing OrElse
            item.m_a四隅.IsEmpty OrElse String.IsNullOrWhiteSpace(item.m_fpath) Then
            Return False
        End If

        Try
            If IO.File.Exists(item.m_fpath) Then
                IO.File.Delete(item.m_fpath)
            End If

            '※角度は、BitMap に対してそのまま適用
            Dim savepng As New CSavePng
            Dim ret As Boolean = savepng.CopyRotateAndSaveToPNG(Canvas, pixcel_lines(item.m_a四隅), item.m_angle, item.m_fpath)

            'エラーにしない(結果はファイルの有無で判断)
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "CImageDraw.save画像")
            Return False

        End Try
    End Function


    '指定点に画像ファイル貼付
    Function load画像(ByVal item As clsImageItem) As Boolean
        If Canvas Is Nothing OrElse item Is Nothing OrElse
             Not IO.File.Exists(item.m_fpath) Then
            Return False
        End If

        Try
            '画像を読み込む
            Dim img As Image = Image.FromFile(item.m_fpath)
            Dim p As PointF = pixcel_point(item.m_a四隅.p左上)
            ' 画像を描画する
            _Graphic.DrawImage(img, p.X, p.Y)

            ' リソースを解放
            img.Dispose()
            Return True

        Catch ex As Exception
            g_clsLog.LogException(ex, "CImageDraw.load画像")
            Return False
        End Try
    End Function


    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: マネージド状態を破棄します (マネージド オブジェクト)
                If Canvas IsNot Nothing Then
                    _Pen_black_thin.Dispose()
                    _Pen_black_thick.Dispose()
                    _Pen_black_dot.Dispose()
                    _Pen_red.Dispose()
                    _Brush_black.Dispose()
                    _Font.Dispose()

                    _BandDefaultPenBrush.Dispose()
                    For Each oneset In _BandPenBrushMap.Values
                        If oneset IsNot Nothing Then
                            oneset.Dispose()
                        End If
                    Next
                    _BandPenBrushMap.Clear()

                    _Graphic.Dispose()
                    Canvas.Dispose()
                    Canvas = Nothing
                End If
            End If
            ' TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
            ' TODO: 大きなフィールドを null に設定します
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 'Dispose(disposing As Boolean)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
    ' Protected Overrides Sub Finalize()
    '     ' このコードを変更しないでください。クリーンアップ コードを 'Dispose(disposing As Boolean)' メソッドに記述します
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを 'Dispose(disposing As Boolean)' メソッドに記述します
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub



    '指定の領域を切り出し、回転して画像ファイルを作る
    Private Class CSavePng
        Function CopyRotateAndSaveToPNG(originalBitmap As Bitmap, points() As PointF, angle As Single, outputFilePath As String) As Boolean
            '外接矩形を取得
            Dim path As New Drawing2D.GraphicsPath()
            path.AddPolygon(points)
            Dim bounds As RectangleF = path.GetBounds()

            '1. 外接矩形サイズで新しいビットマップを作成
            Dim width As Integer = CInt(Math.Ceiling(bounds.Width))
            Dim height As Integer = CInt(Math.Ceiling(bounds.Height))
            Dim clippedBitmap As New Bitmap(width, height)

            '外接矩形をバウンディングボックスの原点に合わせてシフトする
            Dim mtxShift As New Drawing2D.Matrix()
            mtxShift.Translate(-bounds.X, -bounds.Y)  ' path を bounds の位置から (0, 0) へシフト
            path.Transform(mtxShift)  ' path 全体をシフトする

            'path部分のみを外接矩形サイズに描画
            Using g As Graphics = Graphics.FromImage(clippedBitmap)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.Clear(Color.Transparent)
                g.SetClip(path) 'シフト後の path
                '切り取り描画
                g.DrawImage(originalBitmap, 0, 0, bounds, GraphicsUnit.Pixel)
            End Using
            'clippedBitmap.Save(outputFilePath, Imaging.ImageFormat.Png)



            '2. 中心に対して指定角度回転
            'ビットマップ
            Dim rotatedBitmap As Bitmap = RotateBitmap(clippedBitmap, angle)
            'rotatedBitmap.Save(outputFilePath, Imaging.ImageFormat.Png)

            '回転したpathの外接矩形
            Dim rotatedPath As New Drawing2D.GraphicsPath(path.PathPoints, path.PathTypes)
            Dim rotateMatrix As New Drawing2D.Matrix()
            rotateMatrix.RotateAt(angle, New PointF(rotatedBitmap.Width / 2, rotatedBitmap.Height / 2))
            rotatedPath.Transform(rotateMatrix)
            Dim rotatedBounds As RectangleF = rotatedPath.GetBounds()

            '回転したビットマップ上の中心位置
            Dim centerRectangle As New RectangleF((rotatedBitmap.Width - rotatedBounds.Width) / 2,
                                                  (rotatedBitmap.Height - rotatedBounds.Height) / 2,
                                                  rotatedBounds.Width, rotatedBounds.Height)



            '3. 回転後のpathの外接矩形サイズでビットマップを作成
            Dim finalWidth As Integer = CInt(Math.Ceiling(rotatedBounds.Width))
            Dim finalHeight As Integer = CInt(Math.Ceiling(rotatedBounds.Height))
            Dim finalBitmap As New Bitmap(finalWidth, finalHeight)

            '回転後の外接矩形部分のみを描画
            Using g As Graphics = Graphics.FromImage(finalBitmap)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.Clear(Color.Transparent)

                g.DrawImage(rotatedBitmap, 0, 0, centerRectangle, GraphicsUnit.Pixel)
            End Using

            '4. 結果をファイルに保存
            finalBitmap.Save(outputFilePath, Imaging.ImageFormat.Png)

            ' リソース解放
            clippedBitmap.Dispose()
            rotatedBitmap.Dispose()
            finalBitmap.Dispose()

            Return True
        End Function

        Function RotateBitmap(bmp As Bitmap, angle As Single) As Bitmap
            'ビットマップの中心を回転の中心に設定
            Dim rotateAtX As Single = bmp.Width / 2
            Dim rotateAtY As Single = bmp.Height / 2

            '回転後に収まる新しいビットマップのサイズを計算
            Dim rotateMatrix As New Drawing2D.Matrix()
            rotateMatrix.RotateAt(angle, New PointF(rotateAtX, rotateAtY))

            'ビットマップのバウンディングボックスを回転して新しい外接矩形を取得
            Dim path As New Drawing2D.GraphicsPath()
            path.AddRectangle(New RectangleF(0, 0, bmp.Width, bmp.Height))
            path.Transform(rotateMatrix)
            Dim rotatedBounds As RectangleF = path.GetBounds()

            '新しいビットマップを外接矩形のサイズで作成
            Dim rotatedBitmap As New Bitmap(CInt(Math.Ceiling(rotatedBounds.Width)), CInt(Math.Ceiling(rotatedBounds.Height)))

            Using g As Graphics = Graphics.FromImage(rotatedBitmap)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.Clear(Color.Transparent)

                ' 回転の中心をビットマップの中心に設定し、回転後のビットマップを新しいキャンバスの中央に配置
                g.TranslateTransform(rotatedBitmap.Width / 2, rotatedBitmap.Height / 2)
                g.RotateTransform(angle)

                ' ビットマップの中心を回転の基準点にして、元のビットマップを描画
                g.TranslateTransform(-rotateAtX, -rotateAtY)
                g.DrawImage(bmp, New PointF(0, 0))
            End Using

            Return rotatedBitmap
        End Function

    End Class
End Class
