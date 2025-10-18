Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Module mdlProcess
    Public Const WM_COPYDATA As Integer = &H4A
    Private Const SW_RESTORE As Integer = 9

    <StructLayout(LayoutKind.Sequential)>
    Private Structure COPYDATASTRUCT
        Public dwData As IntPtr
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As IntPtr, ByRef lParam As COPYDATASTRUCT) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function GetWindowThreadProcessId(hWnd As IntPtr, ByRef lpdwProcessId As UInteger) As UInteger
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function AttachThreadInput(idAttach As UInteger, idAttachTo As UInteger, fAttach As Boolean) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function SetForegroundWindow(hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function BringWindowToTop(hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function IsIconic(hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
    End Function

    '指定ウィンドウを前面に出す。最小化されていれば復元し、スレッド入力を結合して SetForegroundWindow の制限を回避する。
    Public Sub ActivateWindow(hWnd As IntPtr)
        If hWnd = IntPtr.Zero Then
            Return
        End If

        Try
            ' 最小化なら復元
            If IsIconic(hWnd) Then
                ShowWindow(hWnd, SW_RESTORE)
            End If

            Dim fg As IntPtr = GetForegroundWindow()
            If fg = hWnd Then
                Return
            End If

            Dim fgProcId As UInteger = 0
            Dim fgThreadId As UInteger = GetWindowThreadProcessId(fg, fgProcId)

            Dim targetProcId As UInteger = 0
            Dim targetThreadId As UInteger = GetWindowThreadProcessId(hWnd, targetProcId)

            If fgThreadId <> targetThreadId Then
                ' フォアグラウンドスレッドと対象ウィンドウスレッドを結合してフォーカスを許可
                AttachThreadInput(fgThreadId, targetThreadId, True)
                BringWindowToTop(hWnd)
                SetForegroundWindow(hWnd)
                AttachThreadInput(fgThreadId, targetThreadId, False)
            Else
                BringWindowToTop(hWnd)
                SetForegroundWindow(hWnd)
            End If
        Catch ex As Exception
            mdlDllMain.g_clsLog.LogException(ex, "mdlProcess.ActivateWindow")
        End Try
    End Sub

    '指定ウィンドウにファイルパスを送る
    Public Sub SendFilePath(hWnd As IntPtr, fpath As String)
        Dim bytes As Byte() = System.Text.Encoding.Unicode.GetBytes(fpath & vbNullChar)
        Dim cds As New COPYDATASTRUCT
        cds.dwData = IntPtr.Zero
        cds.cbData = bytes.Length
        cds.lpData = Marshal.AllocHGlobal(bytes.Length)
        Marshal.Copy(bytes, 0, cds.lpData, bytes.Length)
        SendMessage(hWnd, WM_COPYDATA, IntPtr.Zero, cds)
        Marshal.FreeHGlobal(cds.lpData)
    End Sub

    'WM_COPYDATA メッセージから受け取ったファイルパスを取得する
    Public Function ReceiveFilePath(ByRef m As Message) As String()
        Dim flist As New List(Of String)
        If m.Msg = WM_COPYDATA Then
            Try
                Dim cds As COPYDATASTRUCT = CType(Marshal.PtrToStructure(m.LParam, GetType(COPYDATASTRUCT)), COPYDATASTRUCT)
                If cds.lpData <> IntPtr.Zero AndAlso cds.cbData > 0 Then
                    ' Unicode 文字列として受け取る
                    Dim s As String = Marshal.PtrToStringUni(cds.lpData, cds.cbData \ 2)
                    If Not String.IsNullOrWhiteSpace(s) Then
                        ' null か区切りがあれば分割して処理
                        Dim parts() As String = s.Split(New Char() {ChrW(0), vbLf(0)}, StringSplitOptions.RemoveEmptyEntries)
                        For Each p As String In parts
                            If Not String.IsNullOrWhiteSpace(p) Then
                                flist.Add(p)
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                g_clsLog.LogException(ex, "frmMain.WndProc WM_COPYDATA")
                Return Nothing
            End Try

            m.Result = IntPtr.Zero
            Return flist.ToArray()
        Else
            Return Nothing
        End If
    End Function



    '開きたいデータファイル名を選択する
    Public Function OpenDataFileDialog(Optional ByVal def As String = Nothing) As String
        Dim OpenFileDialog1 As New OpenFileDialog
        'データファイル (*.cbmesh; *.xml)|*.cbmesh;*.xml|全て (*.*)|*.*
        OpenFileDialog1.Filter = My.Resources.DataFileFilterOpen
        'データを読み取るファイルを指定してください。
        OpenFileDialog1.Title = My.Resources.DataFileOpenTitle
        OpenFileDialog1.DefaultExt = clsDataTables.DataExtention.Trim(".")
        OpenFileDialog1.FileName = def
        If OpenFileDialog1.ShowDialog() <> DialogResult.OK Then
            Return Nothing
        End If
        Return OpenFileDialog1.FileName
    End Function

    '保存したいデータファイル名を選択する
    Public Function SaveDataFileDialog(Optional ByVal def As String = Nothing) As String
        Dim SaveFileDialog1 As New SaveFileDialog
        'データファイル (*.cbmesh)|*.cbmesh|旧ファイル (*.xml)|*.xml|全て (*.*)|*.*
        SaveFileDialog1.Filter = My.Resources.DataFileFilterSave
        'データを保存するファイルを指定してください。
        SaveFileDialog1.Title = My.Resources.DataFileSaveTitle
        SaveFileDialog1.DefaultExt = clsDataTables.DataExtention.Trim(".")
        If Not String.IsNullOrWhiteSpace(def) Then
            SaveFileDialog1.FileName = IO.Path.ChangeExtension(def, clsDataTables.DataExtention)
        End If
        If SaveFileDialog1.ShowDialog <> DialogResult.OK Then
            Return Nothing
        End If
        Return SaveFileDialog1.FileName
    End Function

End Module
