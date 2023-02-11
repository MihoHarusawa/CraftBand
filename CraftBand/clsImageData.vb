
Imports System.Drawing
Imports CraftBand
Imports CraftBand.clsImageItem
Imports CraftBand.Tables
''' <summary>
''' イメージ処理データ(1枚の絵のデータ要素の配置)
''' </summary>
Public Class clsImageData


    'ファイル名
    Public ReadOnly Property FilePath As String
    'ファイルエラー文字列
    Public ReadOnly Property LastError As String

    'イメージファイル
    Public ReadOnly Property GifFilePath As String

    '基本のひも幅
    Public ReadOnly Property BasicBandWidth As Double
    '基本色
    Public ReadOnly Property BasicColor As String

    '出力ひもリスト情報
    Dim _clsOutput As clsOutput
    '描画対象のアイテム
    Dim _ImageList As New clsImageItemList

    '描画の範囲
    Dim _rDrawingRect As S領域
    Public ReadOnly Property DrawingRect As S領域
        Get
            Return _rDrawingRect
        End Get
    End Property


    Sub New(ByVal fpath As String)
        _FilePath = fpath
    End Sub

    Sub setBasics(ByVal bandWidth As Double, ByVal basicColor As String)
        _BasicBandWidth = bandWidth
        _BasicColor = basicColor
    End Sub

    Sub MoveList(ByVal lst As clsImageItemList)
        _ImageList.MoveList(lst)
    End Sub

    Sub Clear()
        _ImageList.Clear()
        If _clsOutput IsNot Nothing Then
            _clsOutput.Clear()
            _clsOutput = Nothing
        End If
    End Sub


    '描画と画像ファイル生成
    Function MakeImage(ByVal outp As clsOutput) As Boolean
        _clsOutput = outp

        'ひもに記号を振る
        For Each item As clsImageItem In _ImageList
            If item.m_row縦横展開 IsNot Nothing Then
                item.m_row縦横展開.f_s記号 = outp.GetBandMark(item.m_row縦横展開.f_i何本幅, item.m_row縦横展開.f_d出力ひも長, item.m_row縦横展開.f_s色)
            End If
        Next

        '最大の描画範囲を得る
        _rDrawingRect.Clear()
        For Each item As clsImageItem In _ImageList
            _rDrawingRect = _rDrawingRect.get拡大領域(item.Get描画領域)
        Next
        '少し広げる(少なくともバンド幅の2倍分)
        _rDrawingRect.enLarge(mdlUnit.Max(New Length(1, "cm").Value, BasicBandWidth * 2))

        '縦軸追加
        _ImageList.AddItem(setAxis(False))
        '横軸追加
        _ImageList.AddItem(setAxis(True))

        '描画
        Dim ret As Boolean = False
        Dim imgdraw As New CImageDraw(Me)
        Try
            '描画順
            _ImageList.Sort()
            '描画
            imgdraw.Draw(_ImageList)

            '保存ファイルパス　※同じファイルには上書きできないので
            _GifFilePath = IO.Path.ChangeExtension(IO.Path.GetTempFileName, ".gif")

            If Not imgdraw.FileSave(GifFilePath) OrElse Not IO.File.Exists(GifFilePath) Then
                '"gifファイル'{0}'生成エラー"
                _LastError = String.Format(My.Resources.ErrImageGifCreate, GifFilePath)
            Else
                ret = True
            End If

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsImageData.MakeImage")
            _LastError = ex.Message
        Finally
            imgdraw.Dispose()

        End Try

        Return ret
    End Function

    '軸線と目盛線生成
    Private Function setAxis(ByVal isVirtical As Boolean) As clsImageItem
        Dim item As clsImageItem
        Dim diff As S差分
        Dim diffStep As S差分
        Dim steps As Integer

        Dim space As New Length(2, "mm")
        Dim p座標開始点 As New S実座標(_rDrawingRect.x最左 + space.Value, _rDrawingRect.y最上 - space.Value)

        '座標の描画
        Dim unit As Length
        If g_clsSelectBasics.p_unit設定時の寸法単位.Is_inch Then
            unit = New Length(1, "in")
        Else
            unit = New Length(1, "cm")
        End If

        If isVirtical Then
            item = New clsImageItem(ImageTypeEnum._縦軸線, 1)
            diff = New S差分(0, -unit.Value)
            diffStep = New S差分(unit.Value / 4, 0)
            steps = Int((_rDrawingRect.x幅 - space.Value) / unit.Value)
        Else
            item = New clsImageItem(ImageTypeEnum._横軸線, 1)
            diff = New S差分(unit.Value, 0)
            diffStep = New S差分(0, -unit.Value / 4)
            steps = Int((_rDrawingRect.y高さ - space.Value) / unit.Value)
        End If

        Dim pFrom As S実座標 = p座標開始点
        For i As Integer = 0 To steps
            Dim pTo As S実座標 = pFrom + diff
            Dim pToStep As S実座標 = pTo + diffStep
            item.m_lineList.Add(New S線分(pTo, pToStep))
            pFrom = pTo
        Next
        item.m_lineList.Add(New S線分(p座標開始点, pFrom))

        Return item
    End Function


    'HTMLファイルを作る
    Private Function makeHtmlPage(ByVal fpath As String, ByVal imgpath As String, ByVal title As String) As Boolean

        Dim sb As New System.Text.StringBuilder
        sb.Append("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"">").AppendLine()
        sb.Append("<HTML>").AppendLine()
        sb.Append("<HEAD> ").AppendLine()
        sb.Append("<META http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">").AppendLine()
        sb.Append("<TITLE>")
        sb.Append(title)
        sb.Append("</TITLE>").AppendLine()
        sb.Append("</HEAD>").AppendLine()
        sb.AppendFormat("<BODY bgcolor=""{0}"">", "white").AppendLine()
        sb.Append("<H1><P>").AppendLine()
        sb.Append(title).AppendLine()
        sb.Append("</P></H1>").AppendLine()

        '
        If Not String.IsNullOrWhiteSpace(imgpath) Then
            sb.Append("<img src=""")
            sb.Append(imgpath)
            sb.Append(""">").AppendLine()
        End If
        sb.Append("<P></P>").AppendLine()

        sb.AppendLine(_clsOutput.OutCutListHtml())

        sb.Append("</BODY>").AppendLine()
        sb.Append("</HTML>").AppendLine()
        sb.Append("").AppendLine()

        Try
            'UTF8コード指定
            Using sw As New System.IO.StreamWriter(fpath, False, System.Text.Encoding.UTF8) '上書き
                sw.Write(sb.ToString)
                sw.Close()
            End Using
            Return True

        Catch ex As Exception
            ''{0}'ページを作成できませんでした。"
            _LastError = String.Format(My.Resources.ErrHtmlFile, fpath)
            g_clsLog.LogException(ex, "clsImageData.makeHtmlPage", LastError)
            Return False
        End Try

    End Function


    'ブラウザで開く
    Public Function ImgBrowserOpen() As Boolean
        If String.IsNullOrWhiteSpace(GifFilePath) OrElse Not IO.File.Exists(GifFilePath) Then
            '画像ファイルが作られていません。
            _LastError = String.Format(My.Resources.ErrNoGifFile)
            Return False
        End If

        'html
        Dim htmlFile As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetFileNameWithoutExtension(FilePath) & "_")
        htmlFile = IO.Path.ChangeExtension(htmlFile, ".html")

        Dim title As String = IO.Path.GetFileNameWithoutExtension(FilePath)
        If Not makeHtmlPage(htmlFile, GifFilePath, title) Then
            'LastErrorあり
            Return False
        End If

        Try
            Dim url As New Uri(htmlFile)
            'System.Diagnostics.Process.Start(url.ToString)
            System.Diagnostics.Process.Start("cmd", "/C start " & url.ToString)
            Return True

        Catch ex As Exception
            ''{0}'ページを開くことができません。"
            _LastError = String.Format(My.Resources.ErrHtmlProcessStart, htmlFile)
            g_clsLog.LogException(ex, "clsImageData.ImgBrowserOpen", htmlFile)
            Return False

        End Try
    End Function



    '画像ファイルを開く
    Public Function ImgFileOpen() As Boolean
        If String.IsNullOrWhiteSpace(GifFilePath) OrElse Not IO.File.Exists(GifFilePath) Then
            '画像ファイルが作られていません。
            _LastError = String.Format(My.Resources.ErrNoGifFile)
            Return False
        End If

        Try
            Dim p As New Process
            p.StartInfo.FileName = GifFilePath
            p.StartInfo.UseShellExecute = True
            p.Start()
            Return True

        Catch ex As Exception
            'ファイル'{0}'を起動できませんでした。
            _LastError = String.Format(My.Resources.WarningFileStartError, GifFilePath)
            g_clsLog.LogException(ex, "clsImageData.ImgFileOpen", GifFilePath)
            Return False

        End Try

    End Function


    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("FilePath={0}", FilePath).AppendLine()
        sb.AppendFormat("LastError={0}", LastError).AppendLine()
        sb.AppendFormat("GifFilePath={0}", GifFilePath).AppendLine()
        sb.AppendFormat("BasicBandWidth={0} BasicColor={1}", BasicBandWidth, BasicColor).AppendLine()
        sb.AppendFormat("DrawingRect={0}", DrawingRect.ToString).AppendLine()
        sb.AppendFormat("ImageList {0}", _ImageList.ToString)
        Return sb.ToString
    End Function

End Class
