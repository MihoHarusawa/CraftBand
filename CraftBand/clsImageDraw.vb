Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Reflection
Imports System.Windows.Forms.AxHost
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip
Imports CraftBand.clsImageItem
Imports CraftBand.clsMasterTables

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

        Friend PenBand As Pen = Nothing 'ひもの外形
        Friend PenLane As Pen = Nothing '本幅線
        Friend BrushSolid As SolidBrush = Nothing '記号
        Friend BrushAlfa As SolidBrush = Nothing '内部塗りつぶし
        Friend PenWidth As Single = 0 'ペンの幅

        Sub New(ByVal drcol As clsColorRecordSet)
            If drcol IsNot Nothing Then
                If Not drcol.PenColor.IsEmpty Then
                    If 0 < drcol.PenWidth Then
                        PenBand = New Pen(drcol.PenColor, drcol.PenWidth)
                        PenWidth = drcol.PenWidth
                    End If
                    BrushSolid = New SolidBrush(drcol.PenColor)
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
                'BrushAlfa = New SolidBrush(Color.FromArgb(50, Color.LightGray))
            End If
        End Sub

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
            Dim drcol As clsColorRecordSet = g_clsMasterTables.GetColorRecordSet(sColor)
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

    '四隅の4点の配列
    Function pixcel_lines(sqare As S四隅) As PointF()
        Dim point As PointF
        Dim lst As New List(Of PointF)

        point = New PointF(pixcel_X(sqare.p右上.X), pixcel_Y(sqare.p右上.Y))
        lst.Add(point)
        point = New PointF(pixcel_X(sqare.p左上.X), pixcel_Y(sqare.p左上.Y))
        lst.Add(point)
        point = New PointF(pixcel_X(sqare.p左下.X), pixcel_Y(sqare.p左下.Y))
        lst.Add(point)
        point = New PointF(pixcel_X(sqare.p右下.X), pixcel_Y(sqare.p右下.Y))
        lst.Add(point)
        point = New PointF(pixcel_X(sqare.p右上.X), pixcel_Y(sqare.p右上.Y))
        lst.Add(point)

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
        Dim drcol As clsColorRecordSet = g_clsMasterTables.GetColorRecordSet(basiccolor)
        _BandDefaultPenBrush = New CPenBrush(drcol)

        'フォントオブジェクトの作成
        _FontSize = pixcel_width(basicbandwidth / 2)
        _Font = New Font(My.Resources.FontNameMark, _FontSize)

        'サイズ枠描画
        '_Graphic.DrawRectangle(_Pen_black_thin, 0, 0, width - 1, height - 1)
    End Sub

    '描画
    Function Draw(ByVal imglist As clsImageItemList) As Boolean
        If Canvas Is Nothing Then
            Return False
        End If
        Dim ret As Boolean = True
        For Each item As clsImageItem In imglist
            Select Case item.m_ImageType
                Case ImageTypeEnum._横バンド
                    ret = ret And draw横バンド(item)

                Case ImageTypeEnum._縦バンド
                    ret = ret And draw縦バンド(item)

                Case ImageTypeEnum._コマ
                    ret = ret And drawコマ(item)

                Case ImageTypeEnum._底枠
                    ret = ret And draw底枠(item)

                Case ImageTypeEnum._全体枠
                    'ret = ret And draw全体枠(item)

                Case ImageTypeEnum._横の側面, ImageTypeEnum._縦の側面, ImageTypeEnum._四隅領域
                    ret = ret And draw側面(item)

                Case ImageTypeEnum._底の中央線
                    ret = ret And draw底の中央線(item)

                Case ImageTypeEnum._折り返し線
                    ret = ret And draw折り返し線(item)

                Case ImageTypeEnum._文字列
                    ret = ret And draw文字列(item)

                Case ImageTypeEnum._編みかた
                    ret = ret And draw編みかた(item)

                Case ImageTypeEnum._底楕円
                    ret = ret And draw底楕円(item)

                Case ImageTypeEnum._差しひも
                    ret = ret And draw差しひも(item)

                Case ImageTypeEnum._付属品
                    ret = ret And draw付属品(item)

                Case ImageTypeEnum._横軸線
                    ret = ret And draw軸線(item)

                Case ImageTypeEnum._縦軸線
                    ret = ret And draw軸線(item)

            End Select
        Next
        Return ret
    End Function


    Function draw横バンド(ByVal item As clsImageItem) As Boolean
        Return subバンド(item, False)

        'If item.m_row縦横展開 Is Nothing Then
        '    Return False
        'End If
        'Dim colset As CPenBrush = GetBandPenBrush(item.m_row縦横展開.f_s色)
        'If colset Is Nothing Then
        '    Return False
        'End If
        'Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
        'If 0 < rect.Width Then
        '    Dim widHalf As Single = colset.PenWidth / 2
        '    ''白でぬりつぶし
        '    '_Graphic.FillRectangle(Brushes.White, rect.X + colset.PenWidth / 2, rect.Y + colset.PenWidth / 2, rect.Width - colset.PenWidth, rect.Height - colset.PenWidth)

        '    '塗りつぶし
        '    If colset.BrushAlfa IsNot Nothing Then
        '        _Graphic.FillRectangle(colset.BrushAlfa, rect.X + widHalf, rect.Y + widHalf, rect.Width - colset.PenWidth, rect.Height - colset.PenWidth)
        '    End If

        '    '本幅線
        '    If colset.PenLane IsNot Nothing AndAlso 1 < item.m_row縦横展開.f_i何本幅 Then
        '        Dim wid As Single = (pixcel_height(item.m_rひも位置.y高さ) - colset.PenWidth) / item.m_row縦横展開.f_i何本幅
        '        For i As Integer = 1 To item.m_row縦横展開.f_i何本幅 - 1
        '            Dim p左 As New PointF(rect.X + widHalf, rect.Y + wid * i + widHalf)
        '            Dim p右 As New PointF(rect.X + rect.Width - colset.PenWidth, rect.Y + wid * i + widHalf)
        '            _Graphic.DrawLine(colset.PenLane, p左, p右)
        '        Next
        '    End If

        '    'バンド
        '    If colset.PenBand IsNot Nothing Then
        '        If item.m_borderひも = cDirectionEnumAll Then
        '            _Graphic.DrawRectangle(colset.PenBand, rect.X + widHalf, rect.Y + widHalf, rect.Width - colset.PenWidth, rect.Height - colset.PenWidth)
        '        Else
        '            If item.m_borderひも.HasFlag(DirectionEnum._上) Then
        '                _Graphic.DrawLine(colset.PenBand,
        '                          rect.X + widHalf, rect.Y + widHalf,
        '                          rect.X + rect.Width - widHalf, rect.Y + widHalf)
        '            End If
        '            If item.m_borderひも.HasFlag(DirectionEnum._左) Then
        '                _Graphic.DrawLine(colset.PenBand,
        '                          rect.X + widHalf, rect.Y + widHalf,
        '                          rect.X + widHalf, rect.Y + rect.Height - widHalf)
        '            End If
        '            If item.m_borderひも.HasFlag(DirectionEnum._下) Then
        '                _Graphic.DrawLine(colset.PenBand,
        '                          rect.X + widHalf, rect.Y + rect.Height - widHalf,
        '                          rect.X + rect.Width - widHalf, rect.Y + rect.Height - widHalf)
        '            End If
        '            If item.m_borderひも.HasFlag(DirectionEnum._右) Then
        '                _Graphic.DrawLine(colset.PenBand,
        '                          rect.X + rect.Width - widHalf, rect.Y + widHalf,
        '                          rect.X + rect.Width - widHalf, rect.Y + rect.Height - widHalf)
        '            End If
        '        End If
        '    End If
        'End If


        'If Not item.m_bNoMark Then
        '    '記号を左に
        '    If Not String.IsNullOrWhiteSpace(item.m_row縦横展開.f_s記号) AndAlso colset.BrushSolid IsNot Nothing Then
        '        Dim pMark As New PointF(rect.X - _FontSize * 2.7, rect.Y + 3)
        '        _Graphic.DrawString(item.m_row縦横展開.f_s記号, _Font, colset.BrushSolid, pMark)
        '    End If
        'End If

        'Return True
    End Function

    Function draw縦バンド(ByVal item As clsImageItem) As Boolean
        Return subバンド(item, True)

        'If item.m_row縦横展開 Is Nothing Then
        '    Return False
        'End If
        'Dim colset As CPenBrush = GetBandPenBrush(item.m_row縦横展開.f_s色)
        'If colset Is Nothing Then
        '    Return False
        'End If
        'Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
        'If 0 < rect.Height Then
        '    Dim widHalf As Single = colset.PenWidth / 2

        '    '塗りつぶし
        '    If colset.BrushAlfa IsNot Nothing Then
        '        _Graphic.FillRectangle(colset.BrushAlfa, rect.X + widHalf, rect.Y + widHalf, rect.Width - colset.PenWidth, rect.Height - colset.PenWidth)
        '    End If

        '    '本幅線
        '    If colset.PenLane IsNot Nothing AndAlso 1 < item.m_row縦横展開.f_i何本幅 Then
        '        Dim wid As Single = (pixcel_width(item.m_rひも位置.x幅) - colset.PenWidth) / item.m_row縦横展開.f_i何本幅
        '        For i As Integer = 1 To item.m_row縦横展開.f_i何本幅 - 1
        '            Dim p上 As New PointF(rect.X + wid * i + widHalf, rect.Y + widHalf)
        '            Dim p下 As New PointF(rect.X + wid * i + widHalf, rect.Y + rect.Height - colset.PenWidth)
        '            _Graphic.DrawLine(colset.PenLane, p上, p下)
        '        Next
        '    End If

        '    'バンド
        '    If colset.PenBand IsNot Nothing Then
        '        If item.m_borderひも = cDirectionEnumAll Then
        '            _Graphic.DrawRectangle(colset.PenBand, rect.X + widHalf, rect.Y + widHalf, rect.Width - colset.PenWidth, rect.Height - colset.PenWidth)
        '        Else
        '            If item.m_borderひも.HasFlag(DirectionEnum._上) Then
        '                _Graphic.DrawLine(colset.PenBand,
        '                                  rect.X + widHalf, rect.Y + widHalf,
        '                                  rect.X + rect.Width - widHalf, rect.Y + widHalf)
        '            End If
        '            If item.m_borderひも.HasFlag(DirectionEnum._左) Then
        '                _Graphic.DrawLine(colset.PenBand,
        '                                  rect.X + widHalf, rect.Y + widHalf,
        '                                  rect.X + widHalf, rect.Y + rect.Height - widHalf)
        '            End If
        '            If item.m_borderひも.HasFlag(DirectionEnum._下) Then
        '                _Graphic.DrawLine(colset.PenBand,
        '                                  rect.X + widHalf, rect.Y + rect.Height - widHalf,
        '                                  rect.X + rect.Width - widHalf, rect.Y + rect.Height - widHalf)
        '            End If
        '            If item.m_borderひも.HasFlag(DirectionEnum._右) Then
        '                _Graphic.DrawLine(colset.PenBand,
        '                                  rect.X + rect.Width - widHalf, rect.Y + widHalf,
        '                                  rect.X + rect.Width - widHalf, rect.Y + rect.Height - widHalf)
        '            End If
        '        End If
        '    End If

        'End If

        'If Not item.m_bNoMark Then
        '    '記号を上に
        '    If Not String.IsNullOrWhiteSpace(item.m_row縦横展開.f_s記号) AndAlso colset.BrushSolid IsNot Nothing Then
        '        Dim pMark As New PointF(rect.X + 2, rect.Y - _FontSize * 1.5)
        '        _Graphic.DrawString(item.m_row縦横展開.f_s記号, _Font, colset.BrushSolid, pMark)
        '    End If
        'End If
        'Return True
    End Function

    Function subバンド(ByVal item As clsImageItem, ByVal isVirtical As Boolean) As Boolean
        If item.m_row縦横展開 Is Nothing Then
            Return False
        End If
        Dim colset As CPenBrush = GetBandPenBrush(item.m_row縦横展開.f_s色)
        If colset Is Nothing Then
            Return False
        End If
        Dim ret As Boolean = True
        Dim rect As RectangleF = pixcel_rectangle(item.m_rひも位置)
        If (isVirtical And (0 < rect.Height)) OrElse (Not isVirtical And (0 < rect.Width)) Then
            Dim widraw As Single
            If isVirtical Then
                widraw = pixcel_width(item.m_rひも位置.x幅)
            Else
                widraw = pixcel_height(item.m_rひも位置.y高さ)
            End If

            If item.m_regionList IsNot Nothing AndAlso 0 < item.m_regionList.Count Then
                '白抜き箇所がある
                If isVirtical Then
                    '縦ひも
                    item.m_regionList.SortByY()
                    'Debug.Print(item.m_regionList.ToString)
                    Dim YStart As Double = item.m_rひも位置.y最下
                    Dim YEnd As Double = item.m_rひも位置.y最上
                    Dim borderひも As DirectionEnum = item.m_borderひも And Not (DirectionEnum._上)

                    Dim part_r As S領域 = item.m_rひも位置
                    For Each r As S領域 In item.m_regionList
                        If r.y最下 < YStart Then
                            '範囲より下なのでスルー
                        ElseIf YEnd < r.y最下 Then
                            '範囲より上なので終わり
                            Exit For
                        Else
                            '範囲内
                            part_r.y最上 = r.y最下
                            ret = ret And partバンド(isVirtical, colset, pixcel_rectangle(part_r),
                                                  widraw, item.m_row縦横展開.f_i何本幅, borderひも)
                            part_r.y最下 = r.y最上
                        End If
                        borderひも = borderひも And Not (DirectionEnum._下)
                    Next
                    '
                    borderひも = borderひも Or (item.m_borderひも And (DirectionEnum._上))
                    part_r.p右上.Y = YEnd
                    ret = ret And partバンド(isVirtical, colset, pixcel_rectangle(part_r),
                                                  widraw, item.m_row縦横展開.f_i何本幅, borderひも)

                Else
                    '横ひも
                    item.m_regionList.SortByX()
                    'Debug.Print(item.m_regionList.ToString)
                    Dim XStart As Double = item.m_rひも位置.x最左
                    Dim XEnd As Double = item.m_rひも位置.x最右
                    Dim borderひも As DirectionEnum = item.m_borderひも And Not (DirectionEnum._右)

                    Dim part_r As S領域 = item.m_rひも位置
                    For Each r As S領域 In item.m_regionList
                        If r.x最左 < XStart Then
                            '範囲より左なのでスルー
                        ElseIf XEnd < r.x最左 Then
                            '範囲より右なので終わり
                            Exit For
                        Else
                            '範囲内
                            part_r.x最右 = r.x最左
                            ret = ret And partバンド(isVirtical, colset, pixcel_rectangle(part_r),
                                                  widraw, item.m_row縦横展開.f_i何本幅, borderひも)
                            part_r.x最左 = r.x最右
                        End If
                        borderひも = borderひも And Not (DirectionEnum._左)
                    Next
                    '
                    borderひも = borderひも Or (item.m_borderひも And (DirectionEnum._右))
                    part_r.p右上.X = XEnd
                    ret = ret And partバンド(isVirtical, colset, pixcel_rectangle(part_r),
                                                  widraw, item.m_row縦横展開.f_i何本幅, borderひも)
                End If

            Else
                '1本だけのひも
                ret = ret And partバンド(isVirtical, colset, rect, widraw, item.m_row縦横展開.f_i何本幅, item.m_borderひも)
            End If
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
        subコマ(colsetYoko, item.m_knot.a右上四角, item.m_knot.l右上線)
        subコマ(colsetTate, item.m_knot.a左上四角, item.m_knot.l左上線)
        subコマ(colsetYoko, item.m_knot.a左下四角, item.m_knot.l左下線)
        subコマ(colsetTate, item.m_knot.a右下四角, item.m_knot.l右下線)
        Return True
    End Function
    Function draw底枠(ByVal item As clsImageItem) As Boolean
        Dim points() As PointF = pixcel_lines(item.m_a四隅)
        _Graphic.DrawLines(_Pen_black_thick, points)
        For Each line As S線分 In item.m_lineList
            _Graphic.DrawLine(_Pen_black_dot, pixcel_point(line.p開始), pixcel_point(line.p終了))
        Next
        Return True
    End Function

    Function draw全体枠(ByVal item As clsImageItem) As Boolean
        Dim points() As PointF = pixcel_lines(item.m_a四隅)
        _Graphic.DrawLines(_Pen_black_thin, points)
        Return True
    End Function

    Function draw側面(ByVal item As clsImageItem) As Boolean
        Dim points() As PointF = pixcel_lines(item.m_a四隅)
        _Graphic.DrawLines(_Pen_black_thin, points)
        For Each line As S線分 In item.m_lineList
            _Graphic.DrawLine(_Pen_black_dot, pixcel_point(line.p開始), pixcel_point(line.p終了))
        Next
        Return True
    End Function

    Function draw底の中央線(ByVal item As clsImageItem) As Boolean
        For Each line As S線分 In item.m_lineList
            _Graphic.DrawLine(_Pen_red, pixcel_point(line.p開始), pixcel_point(line.p終了))
        Next
        Return True
    End Function

    Function draw折り返し線(ByVal item As clsImageItem) As Boolean
        For Each line As S線分 In item.m_lineList
            _Graphic.DrawLine(_Pen_black_thick, pixcel_point(line.p開始), pixcel_point(line.p終了))
        Next
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
        If colset Is Nothing Then
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
        If item.m_groupRow Is Nothing Then
            Return False
        End If
        'ひも番号1の色
        Dim color As String = item.m_groupRow.GetIndexNameValue(1, "f_s色")
        Dim colset As CPenBrush = GetBandPenBrush(color)
        If colset Is Nothing Then
            Return False
        End If
        '周の線
        If colset.PenLane IsNot Nothing Then
            For Each line As S線分 In item.m_lineList
                _Graphic.DrawLine(colset.PenBand, pixcel_point(line.p開始), pixcel_point(line.p終了))
            Next
        End If
        '楕円弧
        Dim centers() As PointF = pixcel_lines(item.m_a四隅)
        Dim rx As Single = pixcel_width(item.m_lineList(3).p終了.X - item.m_a四隅.p右上.X)
        Dim ry As Single = pixcel_height(item.m_lineList(0).p開始.Y - item.m_a四隅.p右上.Y)
        '右上
        _Graphic.DrawArc(colset.PenBand, centers(0).X - rx, centers(0).Y - ry, rx * 2, ry * 2, 270, 90)
        '左上
        _Graphic.DrawArc(colset.PenBand, centers(1).X - rx, centers(1).Y - ry, rx * 2, ry * 2, 180, 90)
        '左下
        _Graphic.DrawArc(colset.PenBand, centers(2).X - rx, centers(2).Y - ry, rx * 2, ry * 2, 90, 90)
        '右下
        _Graphic.DrawArc(colset.PenBand, centers(3).X - rx, centers(3).Y - ry, rx * 2, ry * 2, 0, 90)

        '編みかた名
        If Not item.p_p文字位置.IsZero AndAlso colset.BrushSolid IsNot Nothing Then
            Dim p As PointF = pixcel_point(item.p_p文字位置)
            Dim str As String = item.m_groupRow.GetNameValueSum("f_s記号")
            str += item.m_groupRow.GetNameValue("f_s編みかた名")
            _Graphic.DrawString(str, _Font, colset.BrushSolid, p)
        End If

        Return True
    End Function

    Function draw差しひも(ByVal item As clsImageItem) As Boolean
        If item.m_groupRow Is Nothing Then
            Return False
        End If
        'ひも番号1の色
        Dim color As String = item.m_groupRow.GetIndexNameValue(1, "f_s色")
        Dim colset As CPenBrush = GetBandPenBrush(color)
        If colset Is Nothing Then
            Return False
        End If
        'ひもの領域
        Dim points() As PointF = pixcel_lines(item.m_a四隅)
        If colset.BrushAlfa IsNot Nothing Then
            _Graphic.FillPolygon(colset.BrushAlfa, points)
        End If
        _Graphic.DrawLines(colset.PenBand, points)

        '編みかた名
        If Not item.p_p文字位置.IsZero AndAlso colset.BrushSolid IsNot Nothing Then
            Dim p As PointF = pixcel_point(item.p_p文字位置)
            Dim str As String = item.m_groupRow.GetNameValueSum("f_s記号")
            str += item.m_groupRow.GetNameValue("f_s編みかた名")
            _Graphic.DrawString(str, _Font, colset.BrushSolid, p)
        End If

        Return True
    End Function

    Function draw付属品(ByVal item As clsImageItem) As Boolean
        If item.m_groupRow Is Nothing Then
            Return False
        End If
        'ひも番号1の色
        Dim color As String = item.m_groupRow.GetIndexNameValue(1, "f_s色")
        Dim colset As CPenBrush = GetBandPenBrush(color)
        If colset Is Nothing Then
            Return False
        End If
        '付属品の領域
        Dim points() As PointF = pixcel_lines(item.m_a四隅)
        If colset.BrushAlfa IsNot Nothing Then
            _Graphic.FillPolygon(colset.BrushAlfa, points)
        End If
        _Graphic.DrawLines(colset.PenBand, points)
        '付属品名
        If Not item.p_p文字位置.IsZero AndAlso colset.BrushSolid IsNot Nothing Then
            Dim p As PointF = pixcel_point(item.p_p文字位置)
            Dim str As String = item.m_groupRow.GetNameValueSum("f_s記号")
            str += item.m_groupRow.GetNameValue("f_s付属品名")
            _Graphic.DrawString(str, _Font, colset.BrushSolid, p)
        End If

        Return True
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

End Class
