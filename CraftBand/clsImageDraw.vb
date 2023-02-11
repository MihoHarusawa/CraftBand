Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports CraftBand
Imports CraftBand.clsImageItem

''' <summary>
''' 画像ファイル生成
''' </summary>
Public Class CImageDraw
    Implements IDisposable

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
    Private Const cBandPenWidth As Single = 2   'ひも描画用

    Private Const cBandBrushAlfa As Integer = 50 'ひも塗りつぶしの不透明度(～255)
    Private Const cThinPenAlfa As Integer = 100 '細い線の不透明度(～255)


    '描画領域
    Dim Canvas As Bitmap = Nothing


    '描画用オブジェクト
    Private _Graphic As Graphics

    '標準描画用
    Private _Pen_black_thin As Pen = Nothing
    Private _Pen_black_thick As Pen = Nothing
    Private _Pen_black_dot As Pen = Nothing
    Private _Pen_red As Pen = Nothing

    Private _Font As Font

    Dim _FontSize As Single


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

        Friend Pen_Thin As Pen
        Friend Pen_Thick As Pen
        Friend Brush_Solid As SolidBrush
        Friend Brush_Alfa As SolidBrush

        Sub New(ByVal drcol As Drawing.Color)
            If Not drcol.IsEmpty Then
                Pen_Thin = New Pen(Color.FromArgb(cThinPenAlfa, drcol), cBandPenWidth)
                Pen_Thick = New Pen(drcol, cBandPenWidth)
                Brush_Solid = New SolidBrush(drcol)
                Brush_Alfa = New SolidBrush(Color.FromArgb(cBandBrushAlfa, drcol))
            Else
                Pen_Thin = New Pen(Color.Gray, cThinPenWidth)
                Pen_Thick = New Pen(Color.Black, cThickPenWidth)
                Brush_Solid = New SolidBrush(Color.Black)
                Brush_Alfa = New SolidBrush(Color.FromArgb(cBandBrushAlfa, Color.Gray))
            End If

        End Sub

        Private disposedValue As Boolean
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    Pen_Thin.Dispose()
                    Pen_Thick.Dispose()
                    Brush_Solid.Dispose()
                    Brush_Alfa.Dispose()
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
        If IsDBNull(obj) Then
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
            Dim drcol As Drawing.Color = g_clsMasterTables.GetColorRecordColor(sColor)
            If drcol.IsEmpty Then
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

    Function pixcel_X(x As Double) As Integer
        Return (x - _LeftX) * _pixcel_h * ImageScale
    End Function

    Function pixcel_Y(y As Double) As Integer
        Return -(y - _TopY) * _pixcel_v * ImageScale
    End Function

    Function pixcel_width(width As Double) As Integer
        Return width * _pixcel_h * ImageScale
    End Function

    Function pixcel_height(height As Double) As Integer
        Return height * _pixcel_v * ImageScale
    End Function

    Function pixcel_point(p As S実座標) As Point
        Return New Point(pixcel_X(p.X), pixcel_Y(p.Y))
    End Function

    '四隅の4点の配列
    Function pixcel_lines(sqare As S四隅) As Point()
        Dim point As Point
        Dim lst As New List(Of Point)

        point = New Point(pixcel_X(sqare.p右上.X), pixcel_Y(sqare.p右上.Y))
        lst.Add(point)
        point = New Point(pixcel_X(sqare.p左上.X), pixcel_Y(sqare.p左上.Y))
        lst.Add(point)
        point = New Point(pixcel_X(sqare.p左下.X), pixcel_Y(sqare.p左下.Y))
        lst.Add(point)
        point = New Point(pixcel_X(sqare.p右下.X), pixcel_Y(sqare.p右下.Y))
        lst.Add(point)
        point = New Point(pixcel_X(sqare.p右上.X), pixcel_Y(sqare.p右上.Y))
        lst.Add(point)

        Return lst.ToArray
    End Function

    '領域を長方形へ
    Function pixcel_rectangle(rect As S領域) As Rectangle
        Dim rectan As Rectangle
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

        'ひも描画用
        Dim drcol As Drawing.Color = g_clsMasterTables.GetColorRecordColor(basiccolor)
        _BandDefaultPenBrush = New CPenBrush(drcol)

        'フォントオブジェクトの作成
        _FontSize = pixcel_width(basicbandwidth / 2)
        _Font = New Font("ＭＳ ゴシック", _FontSize)


        'サイズ枠描画
        '_Graphic.DrawRectangle(_Pen_black_thin, 0, 0, width - 1, height - 1)
    End Sub

    '描画
    Function Draw(ByVal imglist As clsImageItemList) As Boolean
        Dim ret As Boolean = True
        For Each item As clsImageItem In imglist
            Select Case item.m_ImageType
                Case ImageTypeEnum._横バンド
                    ret = ret And draw横バンド(item)

                Case ImageTypeEnum._縦バンド
                    ret = ret And draw縦バンド(item)

                Case ImageTypeEnum._底枠
                    ret = ret And draw底枠(item)

                Case ImageTypeEnum._全体枠
                    'ret = ret And draw全体枠(item)

                Case ImageTypeEnum._横の側面
                    ret = ret And draw側面(item)

                Case ImageTypeEnum._縦の側面
                    ret = ret And draw側面(item)

                Case ImageTypeEnum._底の中央線
                    ret = ret And draw底の中央線(item)

                Case ImageTypeEnum._横軸線
                    ret = ret And draw軸線(item)

                Case ImageTypeEnum._縦軸線
                    ret = ret And draw軸線(item)

            End Select
        Next
        Return ret
    End Function


    Function draw横バンド(ByVal item As clsImageItem) As Boolean
        If item.m_row縦横展開 Is Nothing Then
            Return False
        End If
        Dim rect As Rectangle = pixcel_rectangle(item.m_rひも位置)
        _Graphic.FillRectangle(GetBandPenBrush(item.m_row縦横展開.f_s色).Brush_Alfa, rect.X, rect.Y, rect.Width, rect.Height)

        If 1 < item.m_row縦横展開.f_i何本幅 Then
            Dim wid As Double = item.m_rひも位置.y高さ / item.m_row縦横展開.f_i何本幅
            For i As Integer = 1 To item.m_row縦横展開.f_i何本幅 - 1
                Dim p左 As New S実座標(item.m_rひも位置.x最左, item.m_rひも位置.y最上 - wid * i)
                Dim p右 As New S実座標(item.m_rひも位置.x最右, item.m_rひも位置.y最上 - wid * i)
                _Graphic.DrawLine(GetBandPenBrush(item.m_row縦横展開.f_s色).Pen_Thin, pixcel_point(p左), pixcel_point(p右))
            Next
        End If

        _Graphic.DrawRectangle(GetBandPenBrush(item.m_row縦横展開.f_s色).Pen_Thick, rect.X, rect.Y, rect.Width, rect.Height)
        '左
        Dim pMark As New Point(rect.X - _FontSize * 2.7, rect.Y + 3)
        _Graphic.DrawString(item.m_row縦横展開.f_s記号, _Font, GetBandPenBrush(item.m_row縦横展開.f_s色).Brush_Solid, pMark)
        Return True
    End Function

    Function draw縦バンド(ByVal item As clsImageItem) As Boolean
        If item.m_row縦横展開 Is Nothing Then
            Return False
        End If
        Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
        _Graphic.FillRectangle(GetBandPenBrush(item.m_row縦横展開.f_s色).Brush_Alfa, rect.X, rect.Y, rect.Width, rect.Height)

        If 1 < item.m_row縦横展開.f_i何本幅 Then
            Dim wid As Double = item.m_rひも位置.x幅 / item.m_row縦横展開.f_i何本幅
            For i As Integer = 1 To item.m_row縦横展開.f_i何本幅 - 1
                Dim p上 As New S実座標(item.m_rひも位置.x最左 + wid * i, item.m_rひも位置.y最上)
                Dim p下 As New S実座標(item.m_rひも位置.x最左 + wid * i, item.m_rひも位置.y最下)
                _Graphic.DrawLine(GetBandPenBrush(item.m_row縦横展開.f_s色).Pen_Thin, pixcel_point(p上), pixcel_point(p下))
            Next
        End If

        _Graphic.DrawRectangle(GetBandPenBrush(item.m_row縦横展開.f_s色).Pen_Thick, rect.X, rect.Y, rect.Width, rect.Height)
        '上
        Dim pMark As New Point(rect.X + 2, rect.Y - _FontSize * 1.5)
        _Graphic.DrawString(item.m_row縦横展開.f_s記号, _Font, GetBandPenBrush(item.m_row縦横展開.f_s色).Brush_Solid, pMark)
        Return True
    End Function

    Function draw底枠(ByVal item As clsImageItem) As Boolean
        Dim points() As Point = pixcel_lines(item.m_a四隅)
        _Graphic.DrawLines(_Pen_black_thick, points)
        Return True
    End Function

    Function draw全体枠(ByVal item As clsImageItem) As Boolean
        Dim points() As Point = pixcel_lines(item.m_a四隅)
        _Graphic.DrawLines(_Pen_black_thin, points)
        Return True
    End Function

    Function draw側面(ByVal item As clsImageItem) As Boolean
        Dim points() As Point = pixcel_lines(item.m_a四隅)
        _Graphic.DrawLines(_Pen_black_thin, points)
        Return True
    End Function

    Function draw底の中央線(ByVal item As clsImageItem) As Boolean
        For Each line As S線分 In item.m_lineList
            _Graphic.DrawLine(_Pen_red, pixcel_point(line.p開始), pixcel_point(line.p終了))
        Next
        Return True
    End Function

    Function draw軸線(ByVal item As clsImageItem) As Boolean
        For Each line As S線分 In item.m_lineList
            _Graphic.DrawLine(_Pen_black_dot, pixcel_point(line.p開始), pixcel_point(line.p終了))
        Next
        Return True
    End Function

    Function draw文字列(ByVal str As String, ByVal p As S実座標) As Boolean
        _Graphic.DrawString(str, _Font, Brushes.Black, pixcel_point(p))
        Return True
    End Function


    '領域を縦分割した線
    Function pixcel_vert_lines(rect As S領域, lane As Integer) As C線分リスト
        Dim lineList As New C線分リスト
        If 1 < lane Then
            Dim wid As Double = rect.x幅 / (lane - 1)
            For i As Integer = 1 To lane - 1
                Dim p上 As New S実座標(rect.x最左 + wid * i, rect.y最上)
                Dim p下 As New S実座標(rect.x最左 + wid * i, rect.y最下)
                lineList.Add(New S線分(p上, p下))
            Next
        End If
        Return lineList
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

End Class
