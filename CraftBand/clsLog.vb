

#Const DEBUGPRINT = 1 'デバッグウィンドウへの表示、不要なときはゼロにセット


Imports System.Reflection.Emit
Imports System.Xml

Public Class clsLog

    Const MyExtention As String = ".log"
    Const Delimiter As String = ": "

    Public Enum LogLevel
        None = 0 'なし
        Basic = 1
        Trouble = 2 '問題発生(例外を含む)
        Steps = 3 '処理の流れと基本値
        Detail = 4 '詳細値
        Debug = 9
    End Enum

    Overridable Property FileEncode As System.Text.Encoding = System.Text.Encoding.Unicode
    Overridable Property FileAppend As Boolean = False

    Public ReadOnly Property ExePath As String
        Get
            Return _ExePath
        End Get
    End Property
    Public ReadOnly Property DllPath As String
        Get
            Return _DllPath
        End Get
    End Property
    Dim _ExePath As String
    Dim _DllPath As String

    Public ReadOnly Property Level As LogLevel
        Get
            Return _LogLevel
        End Get
    End Property
    Dim _LogLevel As LogLevel
    Dim _FilePath As String
    Dim _IsWorking As Boolean = False
    Dim _StreamWriter As IO.StreamWriter


    Sub New(ByVal level As Integer, ByVal path As String)
        _IsWorking = False
        If level < 1 Then
            Exit Sub
        End If
        _LogLevel = level
        _ExePath = path

#If DEBUG Then
        _LogLevel = LogLevel.Debug
#End If
        _DllPath = System.Reflection.Assembly.GetExecutingAssembly.Location

        Dim filename As String = IO.Path.ChangeExtension(IO.Path.GetFileName(ExePath), MyExtention)
        _FilePath = IO.Path.Combine(IO.Path.GetTempPath(), filename)

        Try
            '追記でなければ消去
            If IO.File.Exists(_FilePath) AndAlso Not FileAppend Then
                IO.File.Delete(_FilePath)
            End If
            _IsWorking = True

            out("----------------------------------------------------", True)
            outFormatMessage("{0} LogLevel={1}", Now(), _LogLevel)
            oubBinInfo(ExePath)
            oubBinInfo(DllPath)
            out("----------------------------------------------------", True)
            subClose()

        Catch ex As Exception
            Debug.Print(ex.Message)
            _IsWorking = False
        End Try
    End Sub

    Private Sub oubBinInfo(ByVal path As String)
        If String.IsNullOrWhiteSpace(path) Then
            Return
        End If
        Dim sb As New System.Text.StringBuilder
        sb.AppendLine(path)

        Dim lt As DateTime = System.IO.File.GetLastWriteTime(path)
        Dim vi As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(path)

        sb.Append(vbTab).AppendFormat("FileName{0}{1}", Delimiter, IO.Path.GetFileName(path)).AppendLine()
        sb.Append(vbTab).AppendFormat("FileVersion{0}{1}", Delimiter, vi.FileVersion).AppendLine()
        sb.Append(vbTab).AppendFormat("ProductVersion{0}{1}", Delimiter, vi.ProductVersion).AppendLine()
        sb.Append(vbTab).AppendFormat("FileSize{0}{1:#,0}", Delimiter, New System.IO.FileInfo(path).Length).AppendLine()
        sb.Append(vbTab).AppendFormat("LastWriteTime{0}{0}", Delimiter, lt)

        out(sb.ToString, True)
    End Sub


    Private Sub out(ByVal str As String, Optional ByVal newline As Boolean = False)

#If DEBUGPRINT Then
        If newline Then
            Debug.WriteLine(str)
        Else
            Debug.Write(str)
        End If
#End If
        If Not _IsWorking Then
            Return
        End If
        Try
            If _StreamWriter Is Nothing Then
                _StreamWriter = New IO.StreamWriter(_FilePath, True, FileEncode)
            End If
            If Not [String].IsNullOrEmpty(str) Then
                _StreamWriter.Write(str)
            End If
            If newline Then
                _StreamWriter.Write(_StreamWriter.NewLine)
            End If
        Catch ex As Exception
            Debug.Print(ex.Message)
            _IsWorking = False
        End Try
    End Sub

    Private Sub outFormatMessage(ByVal format As String, ByVal ParamArray args() As Object)
        If Not String.IsNullOrEmpty(format) Then
            out([String].Format(format, args), True)
        Else
            out(Nothing, True)
        End If
    End Sub

    Private Sub subClose()
        Try
            If Not _StreamWriter Is Nothing Then
                _StreamWriter.Close()
                _StreamWriter = Nothing
            End If
        Catch ex As Exception
            Debug.Print(ex.Message)
            _IsWorking = False
        End Try
    End Sub


    Public Sub LogFormatMessage(ByVal level As LogLevel, ByVal format As String, ByVal ParamArray args() As Object)
        If _LogLevel < level Then
            Exit Sub
        End If
        outFormatMessage(format, args)
        subClose()
    End Sub


    Public Sub LogResourceMessage(ByVal level As LogLevel, ByVal resouce_name As String, ByVal ParamArray args() As Object)
        If _LogLevel < level Then
            Exit Sub
        End If

        Dim sb As New System.Text.StringBuilder

        If Not String.IsNullOrWhiteSpace(resouce_name) Then
            sb.Append(resouce_name).Append(Delimiter)
            sb.Append(GetResourceString(resouce_name))
        End If

        If args IsNot Nothing Then
            For Each arg In args
                If arg IsNot Nothing AndAlso 0 < arg.ToString.Length Then
                    sb.Append(Delimiter).Append(arg)
                End If
            Next
        End If
        out(sb.ToString, True)
        subClose()
    End Sub


    '例外は_LogLevel LogLevel.Trouble以上
    Public Sub LogException(ByVal ex As Exception, ByVal ParamArray args() As Object)
        If _LogLevel < LogLevel.Trouble Then
            Exit Sub
        End If

        Dim sb As New System.Text.StringBuilder
        '例外発生
        sb.Append(My.Resources.LOG_Exception)
        If args IsNot Nothing Then
            For Each arg In args
                If arg IsNot Nothing AndAlso 0 < arg.ToString.Length Then
                    sb.Append(Delimiter).Append(arg)
                End If
            Next
        End If
        sb.AppendLine()
        sb.AppendLine(ex.ToString)

        out(sb.ToString, True)
        subClose()
    End Sub



    Public Sub Close()
        Try
            If Not _StreamWriter Is Nothing Then
                _StreamWriter.Close()
                _StreamWriter = Nothing
            End If
        Catch ex As Exception
            Debug.Print(ex.Message)
            _IsWorking = False
        End Try
    End Sub


    'リソース文字列を得る
    Protected Function GetResourceString(ByVal resname As String) As String
        Dim res As String
        Try
            res = My.Resources.ResourceManager.GetString(resname)
            If Not String.IsNullOrWhiteSpace(res) Then
                Return res
            End If

        Catch ex As Exception
            Debug.Print("GetResourceString NoMine:{0}", ex.ToString)
        End Try

        Return ResourceString(resname)
    End Function

    '派生クラスのリソース文字列
    Public Overridable Function ResourceString(ByVal resname As String) As String
        Return Nothing
    End Function

End Class
