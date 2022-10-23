Imports CraftBand.Tables
Imports CraftBand.Tables.dstOutput

Public Class clsOutput

    Dim _dstOutput As dstOutput
    Dim _ListOutMark As String

    Public Property FilePath As String


    Dim _LastNumber As Integer = 0
    Dim _LastListOutMark As String

    'データセットのテーブルは1点のみ
    Public ReadOnly Property Table As tblOutputDataTable
        Get
            Return _dstOutput.Tables("tblOutput")
        End Get
    End Property

    Sub New(ByVal mark As String)
        _dstOutput = New dstOutput
        _ListOutMark = mark
    End Sub

    Sub New(ByVal ref As clsOutput)
        _dstOutput = ref._dstOutput.Copy
        _ListOutMark = ref._ListOutMark
        FilePath = ref.FilePath
        _LastNumber = ref._LastNumber
        _LastListOutMark = ref._LastListOutMark
    End Sub

    Sub Clear()
        _dstOutput.Clear()
        _LastNumber = 0

        _LastListOutMark = _ListOutMark
    End Sub

    Sub AddBlankLine()
        Dim row As tblOutputRow = Table.NewtblOutputRow
        _LastNumber += 1
        row.f_iNo = _LastNumber

        row.f_b空行区分 = True
        Table.Rows.Add(row)
    End Sub

    Function NextNewRow(Optional ByVal isSetMark As Boolean = False) As tblOutputRow
        Dim row As tblOutputRow = Table.NewtblOutputRow
        _LastNumber += 1
        row.f_iNo = _LastNumber
        If isSetMark Then
            row.f_s番号 = getMarkIncrement()
        End If

        Table.Rows.Add(row)
        Return row
    End Function

    Private Function getMarkIncrement() As String
        If String.IsNullOrWhiteSpace(_LastListOutMark) Then
            Return ""
        End If
        Dim last As String = _LastListOutMark
        Dim code As Integer = AscW(_LastListOutMark)
        _LastListOutMark = ChrW(code + 1)
        Return last
    End Function


End Class
