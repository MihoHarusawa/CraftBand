Imports CraftBand

Public Class clsMyLog
    Inherits clsLog

    Sub New(ByVal level As Integer)
        MyBase.New(level, System.Reflection.Assembly.GetExecutingAssembly.Location)
    End Sub


    'このモジュールのリソース文字列
    Public Overrides Function ResourceString(ByVal resname As String) As String
        Try
            Return My.Resources.ResourceManager.GetString(resname)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


End Class
