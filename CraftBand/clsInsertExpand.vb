

Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsInsertExpand
Imports CraftBand.clsInsertExpand.CInsertItem
Imports CraftBand.Tables.dstDataTables

Public Class clsInsertExpand
    Class CInsertItem
        Dim _parent As CInsertItemList

        Dim m_iひも番号 As Integer

        Dim m_dひも幅 As Double
        Dim m_d長さ As Double
        Dim m_iひも数 As Integer
        Dim m_s記号 As String

        Dim m_angle As Integer
        Dim m_line As S線分 'm_angle方向

        '識別情報
        Friend ReadOnly Property Ident As String
            Get
                Return String.Format("({0})", _parent._row.f_i番号)
            End Get
        End Property

        Sub New(ByVal parent As CInsertItemList)
            _parent = parent
        End Sub

        Enum mark_position
            _なし
            _始点の前
            _終点の後
        End Enum


        Function ToBand(ByVal draw_mark As mark_position) As CBand
            Dim band = New CBand(_parent._row)

            Dim delta As New S差分(m_angle)
            Dim deltaAx As New S差分(m_angle + 90)

            'バンド描画位置
            band.p始点F = m_line.p開始 + deltaAx * (-m_dひも幅 / 2)
            band.p終点F = m_line.p終了 + deltaAx * (-m_dひも幅 / 2)
            band.p始点T = m_line.p開始 + deltaAx * (m_dひも幅 / 2)
            band.p終点T = m_line.p終了 + deltaAx * (m_dひも幅 / 2)

            '記号描画位置
            If draw_mark = mark_position._始点の前 Then
                band.p文字位置 = m_line.p開始 + delta * -m_dひも幅
            ElseIf draw_mark = mark_position._終点の後 Then
                band.p文字位置 = m_line.p終了 + delta * m_dひも幅
            End If

            Return band
        End Function

        Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            sb.AppendFormat("{0} ひも幅({1:f1}) ", Ident, m_dひも幅)
            sb.AppendFormat("長さ({0:f1}) ひも数{1} 記号{2} ", m_d長さ, m_iひも数, m_s記号)
            sb.AppendFormat("角度({0}) 線分{1}", m_angle, m_line)
            Return sb.ToString
        End Function

    End Class

    '
    Class CInsertItemList
        Inherits List(Of CInsertItem)

        Friend _row As tbl差しひもRow

        Sub New(ByVal row As tbl差しひもRow)
            _row = row
        End Sub

        Function ConvertToBandList(ByVal draw_mark As mark_position) As CBandList
            Dim bandlist As New CBandList
            For Each item As CInsertItem In Me
                Dim band As CBand = item.ToBand(draw_mark)
                bandlist.Add(band)
            Next
            Return bandlist
        End Function

        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            For Each item As CInsertItem In Me
                sb.AppendLine(item.ToString)
            Next
            Return sb.ToString
        End Function

    End Class

    Class CInsertItemListMap
        Inherits Dictionary(Of Integer, CInsertItemList)


        Public Overrides Function ToString() As String
            Dim sb As New System.Text.StringBuilder
            For Each key As Integer In Me.Keys
                sb.AppendFormat("{0}:{1}", key, Me(key).ToString)
            Next
            Return sb.ToString
        End Function
    End Class


End Class
