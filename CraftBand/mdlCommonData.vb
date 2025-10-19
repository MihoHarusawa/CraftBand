Imports System.IO
Imports System.Text

Module mdlCommonData
    Private Const CONFIG_FILE_NAME As String = "CraftBand.config"
    Private Const VENDOR_FOLDER As String = "Labo"
    Private Const APP_FOLDER As String = "CraftBand"

    ''' <summary>
    ''' 共有設定ファイルの完全なパスを取得します。
    ''' C:\Users\Username\AppData\Roaming\Labo\CraftBand\CraftBand.config に対応。
    ''' </summary>
    Private ReadOnly Property SharedConfigFilePath() As String
        Get
            Dim appDataDir As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim appFolderFull As String = Path.Combine(appDataDir, VENDOR_FOLDER, APP_FOLDER)
            Return Path.Combine(appFolderFull, CONFIG_FILE_NAME)
        End Get
    End Property

    ' ディレクトリが存在しない場合に作成
    Private Function EnsureDirectoryExists() As Boolean
        Dim dir As String = Path.GetDirectoryName(SharedConfigFilePath)
        If Not Directory.Exists(dir) Then
            Try
                Directory.CreateDirectory(dir)
                Return True
            Catch ex As Exception
                g_clsLog.LogException(ex, "EnsureDirectoryExists", SharedConfigFilePath)
                Return False
            End Try
        End If
        Return True
    End Function

    ''' <summary>
    ''' 設定ファイルから指定されたセクションとキーの値を取得します。
    ''' </summary>
    ''' <param name="section">設定のカテゴリー名 (例: "General")</param>
    ''' <param name="key">取得したいデータのキー (例: "LastRunApp")</param>
    ''' <returns>見つかった値。見つからない場合は空の文字列 ("")。</returns>
    Public Function GetCommonData(ByVal section As String, ByVal key As String) As String
        If Not File.Exists(SharedConfigFilePath) Then
            Return String.Empty
        End If

        ' 目的のセクション名 (大文字小文字を区別しない)
        Dim targetSection As String = $"[{section}]"
        Dim foundSection As Boolean = False

        Try
            ' ファイルを一行ずつ読み込み
            For Each line In File.ReadLines(SharedConfigFilePath)
                Dim trimmedLine As String = line.Trim()

                If String.IsNullOrWhiteSpace(trimmedLine) OrElse trimmedLine.StartsWith(";") OrElse trimmedLine.StartsWith("#") Then
                    Continue For ' 空行やコメントをスキップ
                End If

                ' セクションヘッダーをチェック
                If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                    ' 目的のセクションが見つかったか判定
                    If trimmedLine.Equals(targetSection, StringComparison.OrdinalIgnoreCase) Then
                        foundSection = True
                    Else
                        ' 目的のセクションが見つかった後、次のセクションに進んだ場合
                        If foundSection Then Exit For
                    End If
                ElseIf foundSection Then
                    ' セクションが見つかった状態で、キー=値の行をチェック
                    Dim parts As String() = trimmedLine.Split(New Char() {"="c}, 2)

                    If parts.Length = 2 Then
                        Dim currentKey As String = parts(0).Trim()
                        Dim currentValue As String = parts(1).Trim()

                        ' キーが一致したら値を返す (大文字小文字を区別しない)
                        If currentKey.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                            Return currentValue
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            g_clsLog.LogException(ex, "GetCommonData", section, key)
        End Try

        Return String.Empty
    End Function

    ''' <summary>
    ''' 設定ファイルに指定されたセクションとキーの値をセットし、保存します。
    ''' </summary>
    ''' <param name="section">設定のカテゴリー名</param>
    ''' <param name="key">設定したいデータのキー</param>
    ''' <param name="value">設定したい値</param>
    ''' <returns>保存に成功した場合 True。</returns>
    Public Function SetCommonData(ByVal section As String, ByVal key As String, ByVal value As String) As Boolean
        If Not EnsureDirectoryExists() Then
            Return False
        End If

        Dim targetSection As String = $"[{section}]"
        Dim newEntry As String = $"{key}={value}"
        Dim lines As New List(Of String)
        Dim sectionFound As Boolean = False
        Dim keyUpdated As Boolean = False

        ' 1. ファイルを読み込み、データをリストに格納
        If File.Exists(SharedConfigFilePath) Then
            lines.AddRange(File.ReadAllLines(SharedConfigFilePath))
        End If

        ' 2. 既存データの更新
        For i As Integer = 0 To lines.Count - 1
            Dim trimmedLine As String = lines(i).Trim()

            ' セクションチェック
            If trimmedLine.Equals(targetSection, StringComparison.OrdinalIgnoreCase) Then
                sectionFound = True
            ElseIf trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                ' 新しいセクションに達したら、現在のセクション処理を終了
                If sectionFound AndAlso Not keyUpdated Then Exit For
            ElseIf sectionFound Then
                ' セクション内でキーを検索し、値を更新
                Dim parts As String() = trimmedLine.Split(New Char() {"="c}, 2)
                If parts.Length = 2 AndAlso parts(0).Trim().Equals(key, StringComparison.OrdinalIgnoreCase) Then
                    lines(i) = newEntry ' キーの行を新しい値で置き換え
                    keyUpdated = True
                    Exit For ' 更新完了
                End If
            End If
        Next

        ' 3. キーが見つからなかった場合、セクションの末尾に追加
        If Not keyUpdated Then
            Dim insertIndex As Integer = -1

            ' セクションの末尾のインデックスを検索
            For i As Integer = 0 To lines.Count - 1
                Dim trimmedLine As String = lines(i).Trim()
                If trimmedLine.Equals(targetSection, StringComparison.OrdinalIgnoreCase) Then
                    sectionFound = True
                    insertIndex = i + 1 ' セクションヘッダーの次から検索開始
                ElseIf sectionFound AndAlso trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                    ' 次のセクションに達した場合
                    insertIndex = i ' 次のセクションの直前が挿入位置
                    Exit For
                End If
            Next

            If sectionFound Then
                ' セクションの末尾（または次のセクションの直前）に挿入
                If insertIndex = -1 Then insertIndex = lines.Count ' ファイル末尾が挿入位置
                lines.Insert(insertIndex, newEntry)
            Else
                ' セクション自体が見つからなかった場合、ファイルの末尾に追加
                lines.Add("") ' 見やすさのために空行を追加
                lines.Add(targetSection)
                lines.Add(newEntry)
            End If
        End If

        ' 4. 全ての行をファイルに書き込む (上書き保存)
        Try
            File.WriteAllLines(SharedConfigFilePath, lines.ToArray(), Encoding.UTF8)
            Return True
        Catch ex As Exception
            g_clsLog.LogException(ex, "SetCommonData", section, key, value)
            Return False
        End Try
    End Function


End Module
