
Imports System.Drawing
Imports CraftBand
Imports CraftBand.clsImageItem
Imports CraftBand.Tables
Imports CraftBand.Tables.dstDataTables
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

        '文字列領域計算用
        clsImageItem.s_BasicFontSize = BasicBandWidth / 2 'CImageDraw._Fontのサイズ
    End Sub

    Sub MoveList(ByVal lst As clsImageItemList)
        If lst IsNot Nothing Then
            _ImageList.MoveList(lst)
        End If
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

        'ひもに記号を振る(縦横展開テーブルを作り直した時用)
        For Each item As clsImageItem In _ImageList
            If item.m_row縦横展開 IsNot Nothing Then
                Dim row As tbl縦横展開Row = item.m_row縦横展開
                If String.IsNullOrEmpty(row.f_s記号) AndAlso
                    0 < row.f_i何本幅 AndAlso 0 < row.f_d出力ひも長 Then
                    row.f_s記号 = outp.GetBandMark(row.f_i何本幅, row.f_d出力ひも長, row.f_s色)
                End If
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
        Dim axvert As clsImageItem = setAxis(False)
        If axvert Is Nothing Then
            Return False
        End If
        _ImageList.AddItem(axvert)

        '横軸追加
        Dim axhorz As clsImageItem = setAxis(True)
        If axhorz Is Nothing Then
            Return False
        End If
        _ImageList.AddItem(axhorz)

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

        Dim delta As S差分 '目盛の間隔
        Dim scale As S差分 '目盛方向
        Dim count As Integer '目盛の数
        Dim len As New Length(2, "mm") '目盛の幅

        Dim p座標開始点 As S実座標 = _rDrawingRect.p左上
        Dim p座標終了点 As S実座標

        '座標の描画
        Dim unit As Length = New Length(1, g_clsSelectBasics.p_unit出力時の寸法単位)
        Try
            Dim item As clsImageItem
            If isVirtical Then
                item = New clsImageItem(ImageTypeEnum._縦軸線, 1)
                p座標終了点 = _rDrawingRect.p左下
                delta = New S差分(0, -unit.Value)
                scale = New S差分(len.Value, 0) '─
                count = Int((p座標開始点.Y - p座標終了点.Y) / unit.Value)
            Else
                item = New clsImageItem(ImageTypeEnum._横軸線, 1)
                p座標終了点 = _rDrawingRect.p右上
                delta = New S差分(unit.Value, 0)
                scale = New S差分(0, -len.Value) '│
                count = Int((p座標終了点.X - p座標開始点.X) / unit.Value)
            End If

            Dim p As S実座標 = p座標開始点 '目盛点
            Dim line As S線分
            For i As Integer = 0 To count - 1
                If i Mod 10 = 0 Then
                    line = New S線分(p, p + scale * 2)
                Else
                    line = New S線分(p, p + scale)
                End If
                item.m_lineList.Add(line)
                p += delta
            Next
            item.m_lineList.Add(New S線分(p座標開始点, p座標終了点))
            Return item

        Catch ex As Exception
            g_clsLog.LogException(ex, "setAxis", isVirtical)
            _LastError = ex.Message
        End Try
        Return Nothing

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
