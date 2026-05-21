Imports System.IO
Imports System.Reflection
Imports System.Text.Json
Imports System.Text.RegularExpressions

Public Class clsCommandLine

    Public ReadOnly Property MasterPath As String = Nothing
    Private ReadOnly Property ImageOutputPath As String = Nothing
    Private ReadOnly Property Image2OutputPath As String = Nothing
    Private ReadOnly Property ListOutputPath As String = Nothing
    Public ReadOnly Property IsNewData As Boolean = False
    Public ReadOnly Property IsHeadlessMode As Boolean = False

    'データファイルパス（複数指定可だが先頭のみ使用）
    Dim _dataList As New List(Of String)
    Public ReadOnly Property DataPath As String
        Get
            If _dataList.Count = 0 Then
                Return Nothing
            Else
                Return _dataList(0)
            End If
        End Get
    End Property

    Public ReadOnly Property IsDataOnly As Boolean
        Get
            Return IsValid AndAlso
                Not IsHeadlessMode AndAlso
                Not IsNewData AndAlso
                Not String.IsNullOrWhiteSpace(MasterPath)
        End Get
    End Property

    Public Function GetDataPathArray() As String()
        Return _dataList.ToArray()
    End Function

    Public ReadOnly Property IsValid As Boolean = True

    'メッセージ(エラー・警告・情報)
    Dim _message As New System.Text.StringBuilder
    Public ReadOnly Property Message As String
        Get
            Return _message.ToString()
        End Get
    End Property
    Sub AddMessage(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            Return
        End If
        _message.AppendLine(message)
    End Sub

    '元のコマンドラインのうち、EXE名以降の引数部分
    Public ReadOnly Property OriginalCommandLine() As String
        Get
            Return Environment.CommandLine.Substring(Environment.ProcessPath.Length).TrimStart()
        End Get
    End Property

    Public Function dump() As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat("Properties Of {0} ({1})", Me.GetType.Name, IIf(IsValid, "Valid", "InValid")).AppendLine()

        'プロパティ値
        Const flgs As BindingFlags = BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static Or BindingFlags.GetField Or BindingFlags.GetProperty Or BindingFlags.DeclaredOnly
        Dim members As MemberInfo() = Me.GetType.GetMembers(flgs)
        For Each mem In members
            If mem.MemberType = MemberTypes.Property Then
                Try
                    sb.AppendFormat(" *{0} = '{1}'", mem.Name, (Me.GetType.InvokeMember(mem.Name, flgs, Nothing, Me, Nothing))).AppendLine()
                Catch ex As Exception
                    sb.AppendFormat(" *Exception!! {0} - {1}", mem.Name, ex.Message).AppendLine()
                End Try
            End If
        Next
        '1点以上のデータファイル
        If 1 < _dataList.Count Then
            sb.AppendFormat(" *DataPathList:").AppendLine()
            For Each d In _dataList
                sb.AppendFormat("   - '{0}'", d).AppendLine()
            Next
        End If

        Return sb.ToString
    End Function

    Sub New(ByVal args() As String)
        ' args が Nothing の場合は空配列にする
        If args Is Nothing Then
            args = New String() {}
        End If

        ' --config を探してファイルから引数を読み替える
        Dim configPath As String = Nothing
        For i As Integer = 0 To args.Length - 1
            Dim a As String = args(i)
            If String.IsNullOrWhiteSpace(a) Then Continue For

            Dim lower As String = a.ToLowerInvariant()
            If lower = "--config" Then
                If i + 1 < args.Length Then
                    configPath = args(i + 1)
                End If
                ' configを指定したならファイル必須
                If String.IsNullOrWhiteSpace(configPath) OrElse Not File.Exists(configPath) Then
                    _message.AppendLine($"No config file '{configPath}'")
                    _IsValid = False
                    _IsHeadlessMode = True ' コンソール表示
                    Exit Sub
                End If
                Exit For
            End If
        Next

        Dim useArgs() As String = args
        If Not String.IsNullOrWhiteSpace(configPath) Then
            Try
                Dim fileArgs = loadArgsFromConfigFile(configPath)
                If fileArgs IsNot Nothing AndAlso 0 < fileArgs.Length Then
                    useArgs = fileArgs
                End If
            Catch ex As Exception
                ' ファイルを指定したならNGはエラー
                _message.AppendLine($"Failed to load config from '{configPath}': {ex.Message}")
                _IsValid = False
                _IsHeadlessMode = True ' コンソール表示
                Exit Sub
            End Try
        End If

        Dim iIndex As Integer = 0
        While iIndex < useArgs.Length
            Dim raw As String = useArgs(iIndex)
            If String.IsNullOrWhiteSpace(raw) Then
                iIndex += 1
                Continue While
            End If

            ' 小文字化はここで行う。先頭の '-' と '/' を除去して正規化する
            Dim argLower As String = raw.ToLowerInvariant()
            Dim key As String = argLower.TrimStart("-"c, "/"c) ' "--data" -> "data", "-d" -> "d", "/data" -> "data"

            Select Case key
                Case "data", "d"
                    iIndex += 1
                    If iIndex < useArgs.Length Then
                        _dataList.Add(useArgs(iIndex))
                    End If

                Case "master", "m"
                    iIndex += 1
                    If iIndex < useArgs.Length Then
                        _MasterPath = useArgs(iIndex)
                    End If

                Case "image", "i"
                    iIndex += 1
                    If iIndex < useArgs.Length Then
                        _ImageOutputPath = useArgs(iIndex)
                        _IsHeadlessMode = True
                    End If

                Case "image2", "i2"
                    iIndex += 1
                    If iIndex < useArgs.Length Then
                        _Image2OutputPath = useArgs(iIndex)
                        _IsHeadlessMode = True
                    End If

                Case "list", "l"
                    iIndex += 1
                    If iIndex < useArgs.Length Then
                        _ListOutputPath = useArgs(iIndex)
                        _IsHeadlessMode = True
                    End If

                Case "new", "n"
                    _IsNewData = True

                Case Else
                    '値トークンか未知のスイッチ（ファイル存在チェック）
                    If IO.File.Exists(useArgs(iIndex)) Then
                        If 0 < _dataList.Count Then
                            _message.AppendLine($"Multiple data files specified: '{useArgs(iIndex)}'")
                        End If
                        _dataList.Add(useArgs(iIndex))
                    ElseIf useArgs(iIndex).ToLowerInvariant() <> "--config" AndAlso (iIndex = 0 OrElse useArgs(iIndex - 1).ToLowerInvariant() <> "--config") Then
                        ' "--config" 自体やその引数はスルー
                        _message.AppendLine($"Unknown argument or file not found: '{useArgs(iIndex)}'")
                        _IsValid = False
                    End If
            End Select

            iIndex += 1
        End While

        'すべての引数解析が終わった後、プレースホルダー({DIR}, {NAME})を解決する
        If IsValid AndAlso Not String.IsNullOrWhiteSpace(DataPath) Then
            resolveAllPathPlaceholders()
        End If

        '--config を指定したのに引数が不正だった場合
        If Not String.IsNullOrWhiteSpace(configPath) AndAlso Not IsValid Then
            _IsHeadlessMode = True ' コンソール表示
        End If

    End Sub

    '確定したDataPathを基準に、各パスのプレースホルダーを一括置換・正規化
    Private Sub resolveAllPathPlaceholders()
        Try
            ' 基準となるデータファイルパスを絶対パス化して分解
            Dim fullDataPath As String = Path.GetFullPath(DataPath)
            Dim dirPath As String = Path.GetDirectoryName(fullDataPath)
            Dim nameNoExt As String = Path.GetFileNameWithoutExtension(fullDataPath)

            ' 各プロパティの置換を実行
            _MasterPath = replacePlaceholder(MasterPath, dirPath, nameNoExt)
            _ImageOutputPath = replacePlaceholder(ImageOutputPath, dirPath, nameNoExt)
            _Image2OutputPath = replacePlaceholder(Image2OutputPath, dirPath, nameNoExt)
            _ListOutputPath = replacePlaceholder(ListOutputPath, dirPath, nameNoExt)

        Catch ex As Exception
            _message.AppendLine($"Failed to resolve path placeholders: {ex.Message}")
            _IsValid = False
        End Try
    End Sub

    '文字列内のプレースホルダー({DIR}, {NAME})を置換
    Private Function replacePlaceholder(ByVal rawPath As String, ByVal dir As String, ByVal name As String) As String
        If String.IsNullOrWhiteSpace(rawPath) Then Return rawPath

        ' 大文字・小文字両方に対応（{DIR}, {NAME} を置換）
        Dim resolved As String = rawPath
        resolved = Regex.Replace(resolved, "\{dir\}", dir, RegexOptions.IgnoreCase)
        resolved = Regex.Replace(resolved, "\{name\}", name, RegexOptions.IgnoreCase)

        ' 置換後のパスをWindowsシステムが解釈できる絶対パスにクリーンアップ
        Try
            Return Path.GetFullPath(resolved)
        Catch
            ' 万が一不正な文字が含まれる場合は置換後の文字列をそのまま返す（後続のエラーハンドリングに委ねる）
            Return resolved
        End Try
    End Function

    Private Function loadArgsFromConfigFile(ByVal path As String) As String()
        Dim txt As String = File.ReadAllText(path).Trim()
        If String.IsNullOrWhiteSpace(txt) Then
            Return New String() {}
        End If

        'Try JSON parse first (array or object)
        Try
            Using doc = JsonDocument.Parse(txt)
                Dim root = doc.RootElement
                Dim list As New List(Of String)

                If root.ValueKind = JsonValueKind.Array Then
                    '単純な配列ならそのまま引数のリストとみなす
                    For Each el In root.EnumerateArray()
                        If el.ValueKind = JsonValueKind.String Then
                            list.Add(el.GetString())
                        End If
                    Next
                    If 0 < list.Count Then
                        Return list.ToArray()
                    End If
                ElseIf root.ValueKind = JsonValueKind.Object Then
                    ' top-level keys mapping
                    Dim tmp As JsonElement

                    If root.TryGetProperty("master", tmp) AndAlso tmp.ValueKind = JsonValueKind.String Then
                        list.Add("--master") : list.Add(tmp.GetString())
                    End If
                    If root.TryGetProperty("data", tmp) AndAlso tmp.ValueKind = JsonValueKind.String Then
                        list.Add("--data") : list.Add(tmp.GetString())
                    End If
                    'newはサポートなし

                    ' output オブジェクト内のキー
                    If root.TryGetProperty("output", tmp) AndAlso tmp.ValueKind = JsonValueKind.Object Then
                        Dim outEl As JsonElement = tmp
                        Dim oe As JsonElement
                        If outEl.TryGetProperty("list", oe) AndAlso oe.ValueKind = JsonValueKind.String Then
                            list.Add("--list") : list.Add(oe.GetString())
                        End If
                        If outEl.TryGetProperty("image", oe) AndAlso oe.ValueKind = JsonValueKind.String Then
                            list.Add("--image") : list.Add(oe.GetString())
                        End If
                        If outEl.TryGetProperty("image2", oe) AndAlso oe.ValueKind = JsonValueKind.String Then
                            list.Add("--image2") : list.Add(oe.GetString())
                        End If
                    End If

                    If 0 < list.Count Then
                        Return list.ToArray()
                    End If
                End If
            End Using
        Catch ex As JsonException
            ' JSON 解析失敗ならフォールバックして下へ（既存の SplitArgs を使用）
            _message.AppendFormat("Config file is not valid JSON, falling back to command-line style parsing: {0}", ex.Message).AppendLine()
        End Try

        '既存の挙動：配列でないならコマンドライン形式のテキストを分割して返す
        Return splitArgs(txt)
    End Function

    Private Function splitArgs(commandLine As String) As String()
        Dim pattern As String = "[^\s""]+|""([^""]*)"""
        Dim m As MatchCollection = Regex.Matches(commandLine, pattern)
        Dim list As New List(Of String)
        For Each mm As Match In m
            If mm.Groups.Count > 1 AndAlso mm.Groups(1).Success Then
                list.Add(mm.Groups(1).Value)
            Else
                list.Add(mm.Value)
            End If
        Next
        Return list.ToArray()
    End Function

    'ヘッドレス実行, mainFormにはデータ設定済
    Public Function ExecuteHeadlessMode(ByVal mainForm As Windows.Forms.Form) As Boolean
        _message.Clear()

        '簡易チェック
        If String.IsNullOrWhiteSpace(ImageOutputPath) AndAlso
            String.IsNullOrWhiteSpace(Image2OutputPath) AndAlso
            String.Equals(ImageOutputPath, Image2OutputPath, StringComparison.OrdinalIgnoreCase) Then

            AddMessage("image,image2 cannot be the same path.")
            Return False
        End If

        Try
            Dim commonActions As ICommonActions = TryCast(mainForm, ICommonActions)
            Dim ret As Boolean = True
            Dim msg As String = Nothing

            '画像出力 (/image)
            If Not String.IsNullOrWhiteSpace(ImageOutputPath) Then
                Dim imgfpath As String = ImageOutputPath
                If String.IsNullOrWhiteSpace(Path.GetExtension(imgfpath)) Then
                    imgfpath = imgfpath & ".gif"
                End If
                If IO.File.Exists(imgfpath) Then IO.File.Delete(imgfpath)
                msg = Nothing
                If Not commonActions.MakeImageFile(imgfpath, msg) Then
                    ret = False
                End If
                AddMessage(msg)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "◆ MakeImageFile({0}):<{1}>", imgfpath, msg)
            End If

            '画像2出力 (/image2)
            If Not String.IsNullOrWhiteSpace(Image2OutputPath) Then
                Dim img2fpath As String = Image2OutputPath
                If String.IsNullOrWhiteSpace(Path.GetExtension(img2fpath)) Then
                    img2fpath = img2fpath & ".gif"
                End If
                Dim parent As String = Path.GetDirectoryName(img2fpath)
                If String.IsNullOrEmpty(parent) Then
                    parent = Directory.GetCurrentDirectory()
                End If
                Dim img2Dir As String = Path.Combine(parent, Path.GetFileNameWithoutExtension(img2fpath))
                If Directory.Exists(img2Dir) Then
                    Directory.Delete(img2Dir, True)
                End If
                Directory.CreateDirectory(img2Dir)
                If IO.File.Exists(img2fpath) Then IO.File.Delete(img2fpath)
                msg = Nothing
                If Not commonActions.MakeImageFile2(img2fpath, img2Dir, msg) Then
                    ret = False
                End If
                AddMessage(msg)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "◆ MakeImageFile2({0}):<{1}>", img2fpath, msg)
            End If

            'リスト出力 (/list)
            If Not String.IsNullOrWhiteSpace(ListOutputPath) Then
                Dim outList As String = ListOutputPath
                If String.IsNullOrWhiteSpace(Path.GetExtension(outList)) Then
                    outList = outList & ".csv"
                End If
                If IO.File.Exists(outList) Then IO.File.Delete(outList)
                msg = Nothing
                If Not commonActions.MakeListFile(outList, msg) Then
                    ret = False
                End If
                AddMessage(msg)
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "◆ MakeListFile({0}):<{1}>", outList, msg)
            End If

            Return ret

        Catch ex As Exception
            AddMessage(ex.Message)
            Return False
        End Try
    End Function
End Class