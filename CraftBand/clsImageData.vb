Imports System.IO
Imports System.Windows.Forms
Imports CraftBand.CImageDraw
Imports CraftBand.clsImageItem
Imports CraftBand.Tables.dstDataTables
''' <summary>
''' イメージ処理データ(1枚の絵のデータ要素の配置)
''' </summary>
Public Class clsImageData
    'ファイル名
    Public ReadOnly Property FilePath As String

    'ファイルエラー文字列
    Protected _LastError As String
    Public ReadOnly Property LastError As String
        Get
            Return _LastError
        End Get
    End Property

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

    '中心座標
    Public Property CenterCoordinates As S実座標

    'fpath:タイトル、htmlファイル名
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

    Sub AddItem(ByVal item As clsImageItem)
        If item IsNot Nothing Then
            _ImageList.Add(item)
        End If
    End Sub

    Sub Clear()
        _ImageList.Clear()
        If _clsOutput IsNot Nothing Then
            _clsOutput.Clear()
            _clsOutput = Nothing
        End If
    End Sub

    '現clsImageItemが占める領域　_rDrawingRectにセット
    Function CurrentItemDrawingRect() As S領域
        _rDrawingRect.Clear()
        For Each item As clsImageItem In _ImageList
            _rDrawingRect = _rDrawingRect.get拡大領域(item.Get描画領域)
        Next
        Return _rDrawingRect
    End Function


    '描画と画像ファイル生成
    'outp: Html時のタイトルとカットリスト
    Function MakeImage(ByVal outp As clsOutput) As Boolean
        _clsOutput = outp

        ''ひもに記号を振る(縦横展開テーブルを作り直した時用)
        'For Each item As clsImageItem In _ImageList
        '    If item.m_row縦横展開 IsNot Nothing Then
        '        Dim row As tbl縦横展開Row = item.m_row縦横展開
        '        If String.IsNullOrEmpty(row.f_s記号) AndAlso
        '            0 < row.f_i何本幅 AndAlso 0 < row.f_d出力ひも長 Then
        '            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "MakeImage: No f_s記号 {0} {1} {2} ", row.f_sひも名, row.f_iひも種, row.f_iひも番号)
        '            row.f_s記号 = outp.GetBandMark(row.f_i何本幅, row.f_d出力ひも長, row.f_s色)
        '        End If
        '    End If
        'Next

        '最大の描画範囲を_rDrawingRectにセット
        CurrentItemDrawingRect()

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

        Catch ex As DrawException
            g_clsLog.LogException(ex, "clsImageData.DrawException")
            g_clsLog.LogFormatMessage(clsLog.LogLevel.Trouble, "ErrorItem={0}", ex.ErrorItem)
            'イメージ生成時に例外が発生しました。{0}{1}
            _LastError = String.Format(My.Resources.ImageDrawException, ex.ErrorItem, ex.Message)

        Catch ex As Exception
            g_clsLog.LogException(ex, "clsImageData.MakeImage")
            'イメージ生成時に例外が発生しました。{0}{1}
            _LastError = String.Format(My.Resources.ImageDrawException, ex.Message, Nothing)
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
        Dim scalelong As Integer = 10 '目盛を長くする数

        Dim p座標開始点 As S実座標 = _rDrawingRect.p左上
        Dim p座標終了点 As S実座標

        '座標の描画
        Dim unit As Length = New Length(1, g_clsSelectBasics.p_unit出力時の寸法単位)
        If g_clsSelectBasics.p_unit出力時の寸法単位.Is_inch Then '#24
            scalelong = 12
        End If
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
                If i Mod scalelong = 0 Then
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
    Private Function makeHtmlPage(ByVal fpath As String, ByVal imgpath As String, ByVal title As String, ByVal drawinfo As enumBrowserDrawInfo) As Boolean

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
        If drawinfo.HasFlag(enumBrowserDrawInfo._gif) AndAlso
            Not String.IsNullOrWhiteSpace(imgpath) Then
            sb.Append("<img src=""")
            sb.Append(imgpath)
            sb.Append(""">").AppendLine()
        End If
        sb.Append("<P></P>").AppendLine()

        If drawinfo.HasFlag(enumBrowserDrawInfo._cutlist) AndAlso _clsOutput IsNot Nothing Then
            sb.AppendLine(_clsOutput.OutCutListHtml())
        End If
        If drawinfo.HasFlag(enumBrowserDrawInfo._size) AndAlso _clsOutput IsNot Nothing Then
            sb.AppendLine(_clsOutput.OutSizeHtml())
        End If

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


    <Flags()>
    Public Enum enumBrowserDrawInfo
        _none = 0
        _gif = &H1          '画像
        _cutlist = &H2      'カットリスト
        _title_back = &H4   '裏面タイトル
        _size = &H8     'サイズ
    End Enum
    Public Const cBrowserBasicInfo As enumBrowserDrawInfo = enumBrowserDrawInfo._gif Or enumBrowserDrawInfo._cutlist
    Public Const cBrowserBackFace As enumBrowserDrawInfo = enumBrowserDrawInfo._gif Or enumBrowserDrawInfo._title_back
    Public Const cBrowserSize As enumBrowserDrawInfo = enumBrowserDrawInfo._gif Or enumBrowserDrawInfo._size

    'ブラウザで開く
    Public Function ImgBrowserOpen(ByVal drawinfo As enumBrowserDrawInfo) As Boolean
        If String.IsNullOrWhiteSpace(GifFilePath) OrElse Not IO.File.Exists(GifFilePath) Then
            '画像ファイルが作られていません。
            _LastError = String.Format(My.Resources.ErrNoGifFile)
            Return False
        End If

        'html
        Dim htmlFile As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetFileNameWithoutExtension(FilePath) & "_")
        htmlFile = IO.Path.ChangeExtension(htmlFile, ".html")

        Dim title As String = Nothing
        If _clsOutput IsNot Nothing Then
            title = _clsOutput.DataTitle & Parentheses(_clsOutput.DataCreater)
        End If
        If String.IsNullOrWhiteSpace(title) Then
            title = IO.Path.GetFileNameWithoutExtension(FilePath)
        End If
        If drawinfo.HasFlag(enumBrowserDrawInfo._title_back) Then
            '裏面 : {0}
            title = String.Format(My.Resources.TitleBackFace, title)
        End If
        If Not makeHtmlPage(htmlFile, GifFilePath, title, drawinfo) Then
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


    '面の順序
    Enum enumBasketPlateIdx
        _bottom = 0 '底面         横と縦
        _leftside  '左側面
        _front      '前面         横と高さ
        _rightside  '右側面
        _back   '背面
    End Enum
    Public Const cBasketPlateCount As Integer = 5

    '3D
    Public Function CreateOBJWithTextures(ByVal width As Single, ByVal height As Single, ByVal depth As Single,
                                          ByVal textureFiles() As String, ByVal outPath As String) As Boolean
        Try
            Dim outputFolder As String = getOutputFolder(outPath)
            If String.IsNullOrEmpty(outputFolder) Then
                Return True 'Manual Stop
            End If

            ' OBJファイルのパス
            Dim objFilePath As String = IO.Path.Combine(outputFolder, "textured_rectangular_prism.obj")
            ' MTLファイルのパス
            Dim mtlFilePath As String = IO.Path.Combine(outputFolder, "textured_rectangular_prism.mtl")

            ' OBJファイルを作成
            Using writer As New StreamWriter(objFilePath)
                ' MTLファイルを参照
                writer.WriteLine("mtllib textured_rectangular_prism.mtl")

                ' 頂点データを出力 (8頂点)
                writer.WriteLine("v 0 0 0")                 ' 頂点1 (左下手前)
                writer.WriteLine("v " & width & " 0 0")     ' 頂点2 (右下手前)
                writer.WriteLine("v " & width & " " & height & " 0") ' 頂点3 (右上手前)
                writer.WriteLine("v 0 " & height & " 0")    ' 頂点4 (左上手前)
                writer.WriteLine("v 0 0 " & depth)          ' 頂点5 (左下奥)
                writer.WriteLine("v " & width & " 0 " & depth) ' 頂点6 (右下奥)
                writer.WriteLine("v " & width & " " & height & " " & depth) ' 頂点7 (右上奥)
                writer.WriteLine("v 0 " & height & " " & depth) ' 頂点8 (左上奥)

                ' テクスチャ座標 (2D座標)
                writer.WriteLine("vt 0 0")
                writer.WriteLine("vt 1 0")
                writer.WriteLine("vt 1 1")
                writer.WriteLine("vt 0 1")

                ' 面に対応するマテリアルを使用
                ' 底面
                writer.WriteLine("usemtl bottom_texture")
                writer.WriteLine("f 1/1 2/2 6/3 5/4")

                ' 左側面
                writer.WriteLine("usemtl left_texture")
                writer.WriteLine("f 1/1 5/2 8/3 4/4")

                ' 正面
                writer.WriteLine("usemtl front_texture")
                'writer.WriteLine("f 1/1 2/2 3/3 4/4")
                writer.WriteLine("f 1/2 2/1 3/4 4/3")

                ' 右側面
                writer.WriteLine("usemtl right_texture")
                'writer.WriteLine("f 2/1 6/2 7/3 3/4")
                writer.WriteLine("f 2/2 6/1 7/4 3/3")

                ' 背面
                writer.WriteLine("usemtl back_texture")
                writer.WriteLine("f 5/1 6/2 7/3 8/4")

                ' 上面 (透明)
                writer.WriteLine("usemtl transparent_material")
                writer.WriteLine("f 4/1 3/2 7/3 8/4")
            End Using

            ' MTLファイルの作成
            Using writer As New StreamWriter(mtlFilePath)
                ' 各面に対応するマテリアルを定義
                writer.WriteLine("newmtl bottom_texture")
                writer.WriteLine("map_Kd " & IO.Path.GetFileName(textureFiles(0))) '底面のテクスチャ
                writer.WriteLine()

                writer.WriteLine("newmtl left_texture")
                writer.WriteLine("map_Kd " & IO.Path.GetFileName(textureFiles(1))) '左側面のテクスチャ
                writer.WriteLine()

                writer.WriteLine("newmtl front_texture")
                writer.WriteLine("map_Kd " & IO.Path.GetFileName(textureFiles(4))) '背面のテクスチャ
                writer.WriteLine()

                writer.WriteLine("newmtl right_texture")
                writer.WriteLine("map_Kd " & IO.Path.GetFileName(textureFiles(3))) '右側面のテクスチャ
                writer.WriteLine()

                writer.WriteLine("newmtl back_texture")
                writer.WriteLine("map_Kd " & IO.Path.GetFileName(textureFiles(2))) '前面のテクスチャ
                writer.WriteLine()

                ' 透明なマテリアルを定義
                writer.WriteLine("newmtl transparent_material")
                writer.WriteLine("d 0.0") ' 完全に透明に設定
            End Using

            ' 画像ファイルを出力ディレクトリにコピー
            For Each textureFile As String In textureFiles
                IO.File.Copy(textureFile, IO.Path.Combine(outputFolder, IO.Path.GetFileName(textureFile)), True)
            Next

            ' 作成したOBJファイルを開く
            If Not String.IsNullOrWhiteSpace(outPath) Then
                Process.Start("explorer.exe", objFilePath)
            Else
                'フォルダ'{0}' に OBJファイル'{1}' および関連ファイル一式を出力しました。
                Dim msg As String = String.Format(My.Resources.MsgObjOutput, outputFolder, IO.Path.GetFileName(objFilePath))
                MessageBox.Show(msg, IO.Path.GetFileNameWithoutExtension(FilePath), MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return True

        Catch ex As Exception
            'ファイル'{0}'を起動できませんでした。
            _LastError = String.Format(My.Resources.WarningFileStartError, GifFilePath)
            g_clsLog.LogException(ex, "clsImageData.CreateOBJWithTextures", GifFilePath)
            Return False
        End Try
    End Function

    Private Function getOutputFolder(ByVal outDir As String) As String
        Dim outputFolder As String

        If String.IsNullOrWhiteSpace(outDir) Then

            Dim folderDialog As New FolderBrowserDialog()

            '出力先フォルダを選択してください。複数ファイルがセットで出力されます。
            folderDialog.Description = My.Resources.MsgObjOutputSelect
            folderDialog.RootFolder = Environment.SpecialFolder.Desktop '

            If folderDialog.ShowDialog() = DialogResult.OK Then
                outputFolder = folderDialog.SelectedPath
            Else
                Return Nothing 'Cancel
            End If

        Else
            outputFolder = outDir

        End If

        ' 出力ディレクトリを作成
        If Not IO.Directory.Exists(outputFolder) Then
            IO.Directory.CreateDirectory(outputFolder)
        End If

        Return outputFolder
    End Function

End Class
