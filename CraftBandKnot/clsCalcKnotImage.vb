Imports System.DirectoryServices
Imports CraftBand
Imports CraftBand.clsImageData
Imports CraftBand.clsImageItem

Partial Public Class clsCalcKnot

    'CAD測定値
    Const c_foldingRatio As Double = 2 / 6.43   '要尺の折り位置までの比
    Const c_tiltRatio As Double = 0.216   '折りの傾き(幅に対する差の比)

    'この長さより短いバンドは描かない
    Shared c_minBandLength As Double = New Length(0.5, "mm").Value


    'プレビュー時に生成
    Dim _ImageListコマ As clsImageItemList
    Dim _ImageList描画要素 As clsImageItemList '底や側面の展開図
    Dim _ImageList開始位置 As clsImageItemList

    Const IdxDrawBandBridge As Integer = 10
    Const IdxDrawBandStart As Integer = 15



#Region "テクスチャファイル作成用"
    '絵のファイル
    Dim _PlatePngFilePath(clsImageData.cBasketPlateCount - 1) As String
    Property p_sPlatePngFilePath(ByVal pidx As clsImageData.enumBasketPlateIdx, ByVal check As Boolean) As String
        Get
            If check AndAlso Not String.IsNullOrWhiteSpace(_PlatePngFilePath(pidx)) Then
                If IO.File.Exists(_PlatePngFilePath(pidx)) Then
                    Return _PlatePngFilePath(pidx)
                Else
                    Return Nothing
                End If
            Else
                Return _PlatePngFilePath(pidx)
            End If
        End Get
        Set(value As String)
            _PlatePngFilePath(pidx) = value
            If check AndAlso Not String.IsNullOrWhiteSpace(value) AndAlso IO.File.Exists(value) Then
                IO.File.Delete(value)
            End If
        End Set
    End Property

    '絵の回転角度
    Shared _PlateAngle() As Integer = {0, 90, 0, -90, 180}
    Shared _PlateAngle45() As Integer = {45, 135, 45, -45, -135}

#End Region



    '座標ベースの描画
    '                   コマ空間(コマの整数単位・下がプラス)
    '　　　　　↑y　　↙　　
    '　　┌──╂──┐　　　　　　　　
    '　　│　　┃　　│　　
    '━━┿━━╋━━┿→ x
    '　　│　　┃　　│　　
    '　　└──╂──┘　　　
    '　　　　　┃　　　　　

    Private Function toPoint(ByVal xbase As Double, ByVal ybase As Double) As S実座標
        Return New S実座標(_dコマベース寸法 * xbase, -_dコマベース寸法 * ybase)
    End Function

    Private Function toPoint(ByVal base() As Double) As S実座標
        Return toPoint(base(0), base(1))
    End Function


    '条件に合うコマとバンドの描画
    Private Function addコマ(ByVal knotfolder As CKnotFolder, ByVal isleft As Boolean, ByVal isDispMark As Boolean,
                           ByVal isBottomOnly As Boolean, ByVal isAllBand As Boolean, ByVal isKnotFrame As Boolean) As Boolean

        '有効なknotfolderであることまではチェック済
        If isBottomOnly AndAlso Not knotfolder.IsBottomBase Then
            Return True
        End If

        'コマ位置(コマベース左上→コマの中心)にコマ描画
        Dim p左上 As S実座標 = toPoint(knotfolder.coorBaseXY)
        Dim p中心 As S実座標 = p左上 + (Unit315 * (_dコマベース寸法 / 2))

        Dim knot As New CKnot(p中心, _dコマの寸法, _dコマ間のすき間, isleft)

        Dim bandY As New CBand(knotfolder.m_row縦横展開(emExp._Yoko))
        Dim bandT As New CBand(knotfolder.m_row縦横展開(emExp._Tate))
        knot.SetBandYH(bandY, bandT, _d基本のひも幅, False) '展開図のコマ

        'コマ枠
        If isKnotFrame Then
            knot.SetRegionDisp(True, False) 'コマ寸法
        End If

        '記号
        If isDispMark Then
            If isBottomOnly Then
                If knotfolder.BottomBaseMarkSide <> cDirectionEnumNone Then
                    knot.SetMarkDisp(knotfolder.BottomBaseMarkSide)
                End If
            Else
                If knotfolder.KnotMarkSide <> cDirectionEnumNone Then
                    knot.SetMarkDisp(knotfolder.KnotMarkSide)
                End If
            End If
        End If

        '「ひも全体」指定の時はバンドを表示
        Dim isDraw As Boolean = isAllBand
        If isBottomOnly AndAlso knotfolder.BottomBaseRimSide = cDirectionEnumNone Then
            isDraw = False
        End If
        If Not isBottomOnly AndAlso knotfolder.KnotRimSide = cDirectionEnumNone Then
            isDraw = False
        End If
        'バンド表示
        If isDraw Then

            Dim sideDraw As DirectionEnum = cDirectionEnumNone
            If isBottomOnly Then
                sideDraw = knotfolder.BottomBaseRimSide
            ElseIf _b斜め立ち上げ Then
                sideDraw = knotfolder.KnotRimSide
            Else
                '除外:縦横時の側面の辺
                sideDraw = (knotfolder.KnotRimSide(emExp._Tate) Or knotfolder.KnotRimSide(emExp._Yoko))
            End If

            If sideDraw <> cDirectionEnumNone Then
                Dim bandlist As New CBandList
                '指定のある側
                Dim sides() As SideIndexEnum = DirectionToSideIndex(sideDraw)
                For Each side As SideIndexEnum In sides
                    Dim addlen As Double = knotfolder.AdditionalLength(side)
                    '底のみの場合は側面ぶんの長さを加える
                    If isBottomOnly Then
                        addlen += knotfolder.SideKomaCount(side) * _dコマベース要尺 * p_dマイひも長係数
                    End If
                    If c_minBandLength < addlen Then
                        Dim band As CBand = knot.GetExBand(side, addlen)
                        bandlist.Add(band)
                    End If
                Next

                If 0 < bandlist.Count Then
                    Dim item As New clsImageItem(bandlist, knotfolder.m_position.HorzIndex, knotfolder.m_position.VertIndex)
                    _ImageListコマ.Add(item)
                End If
            End If
        End If

        Dim knotitem As New clsImageItem(knot, knotfolder.m_position.HorzIndex, knotfolder.m_position.VertIndex)
        _ImageListコマ.Add(knotitem)

        knotfolder.Knot = knot
        Return True
    End Function

    '隣接するコマがある場合は、コマをバンドで繋ぐ
    Private Function bridgeコマ(ByVal knotfolder As CKnotFolder, ByVal bandlist As CBandList) As Boolean
        'すき間が一定以上で、コマのあるknotfolderであることまではチェック済


        '右方向
        Dim knotRight As CKnotFolder = knotfolder.NextSideKnotFolder(SideIndexEnum._右側)
        If knotRight IsNot Nothing AndAlso knotRight.Knot IsNot Nothing Then
            '斜めのワープでなければ
            If knotfolder.SameKnotFolder Is Nothing OrElse
               knotRight.SameKnotFolder Is Nothing OrElse
               New SPosition(knotfolder.m_position, knotRight.m_position).GetSide <> SideIndexEnum._右側 Then

                Dim band As CBand = knotfolder.Knot.GetExBand始点(SideIndexEnum._右側)
                knotRight.Knot.SetExBand終点(band, SideIndexEnum._左側)
                bandlist.Add(band)
            End If
        End If

        '下方向
        Dim knotDown As CKnotFolder = knotfolder.NextSideKnotFolder(SideIndexEnum._下側)
        If knotDown IsNot Nothing AndAlso knotDown.Knot IsNot Nothing Then
            '斜めのワープでなければ
            If knotfolder.SameKnotFolder Is Nothing OrElse
               knotDown.SameKnotFolder Is Nothing OrElse
               New SPosition(knotfolder.m_position, knotDown.m_position).GetSide <> SideIndexEnum._下側 Then

                Dim band As CBand = knotfolder.Knot.GetExBand始点(SideIndexEnum._下側)
                knotDown.Knot.SetExBand終点(band, SideIndexEnum._上側)
                bandlist.Add(band)
            End If
        End If

        '縦横の場合、となりの側面へのつなぎ
        If _b斜め立ち上げ Then
            Return True
        End If

        Dim sides() As SideIndexEnum = DirectionToSideIndex(knotfolder.KnotRimSide(emExp._Side))
        For Each side As SideIndexEnum In sides
#If 0 Then
            Dim addlen As Double = knotfolder.AdditionalLength(side)
            addlen = addlen / 2
            If addlen <= c_minBandLength Then
                Continue For
            End If
            Dim band As CBand = knotfolder.Knot.GetExBand(side, addlen)
            band.is終点FT線 = False
#Else
            Dim dir As New SPosition(side)
            Dim delta As New S差分(dir.HorzIndex, -dir.VertIndex) 'Y軸逆
            delta *= _dコマベース寸法

            Dim tmpKnot As New CKnot(knotfolder.Knot)
            tmpKnot.Move(delta)

            Dim opside As SideIndexEnum = (-dir).GetSide()
            Dim band As CBand = knotfolder.Knot.GetExBand始点(side)
            tmpKnot.SetExBand終点(band, opside)
#End If
            bandlist.Add(band)
        Next

        Return True
    End Function

    Private Function imagelistコマ配置(ByVal isKnotLeft As Boolean, ByVal isDispMark As Boolean,
                                   ByVal isBottomOnly As Boolean, ByVal isAllBand As Boolean, ByVal isKnotFrame As Boolean) As Boolean
        '_KnotFolderSpace 設定済のこと
        If Not _KnotFolderSpace.IsValid Then
            Return False
        End If

        If _ImageListコマ Is Nothing Then
            _ImageListコマ = New clsImageItemList
        End If

        '高さゼロは底のみと同じ
        Dim iBottomBaseOnly As Boolean = isBottomOnly
        If _b斜め立ち上げ Then
            If p_i側面の切捨コマ数 = 0 Then
                iBottomBaseOnly = True
            End If
        Else
            If p_i編みひもの本数 = 0 Then
                iBottomBaseOnly = True
            End If
        End If

        '各コマ描画
        For Each knotfolder In _KnotFolderSpace
            If knotfolder IsNot Nothing AndAlso 2 <= knotfolder.BandSetCount Then
                addコマ(knotfolder, isKnotLeft, isDispMark, iBottomBaseOnly, isAllBand, isKnotFrame)
            End If
        Next
        'コマ間の接続
        If c_minBandLength < _dコマ間のすき間 Then
            Dim bandlist As New CBandList
            For Each knotfolder In _KnotFolderSpace
                If knotfolder IsNot Nothing AndAlso knotfolder.Knot IsNot Nothing Then
                    bridgeコマ(knotfolder, bandlist)
                End If
            Next
            If 0 < bandlist.Count Then
                Dim item As New clsImageItem(bandlist, IdxDrawBandBridge, 1)
                _ImageListコマ.Add(item)

                'g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "bridgeコマ={0}", bandlist.ToString)
            End If
        End If


        Return True
    End Function


    '底と側面枠(縦横)
    '底のみ=isBottomOnly
    Function imageList描画要素(ByVal isBottomOnly As Boolean) As clsImageItemList
        Dim basecross As Boolean = False '底のグリッド
        Dim sideline As Boolean = False '側面のコマライン

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList


        Dim d側面の高さ As Double = _dコマベース寸法 * p_i編みひもの本数
        Dim d折り返し高さ As Double = _dコマベース寸法 * _i折り返しコマ数
        Dim deltaコマ右 As S差分 = Unit0 * _dコマベース寸法
        Dim deltaコマ下 As S差分 = Unit270 * _dコマベース寸法
        Dim a四隅 As S四隅

        Dim a底 As S四隅
        a底.p左上 = toPoint(-_i横のコマ数 / 2, -_i縦のコマ数 / 2)
        a底.p右上 = toPoint(_i横のコマ数 / 2, -_i縦のコマ数 / 2)
        a底.p左下 = -a底.p右上
        a底.p右下 = -a底.p左上

        Dim delta As S差分
        Dim line As S線分

        '底の画像
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._bottom, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 1)
            item.m_a四隅 = a底
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._bottom, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._bottom)
            itemlist.AddItem(item)
        End If
        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底
        For i As Integer = 1 To _i横のコマ数
            If i < _i横のコマ数 AndAlso basecross Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
            For j As Integer = 1 To _i縦のコマ数
                If i = 1 AndAlso basecross Then
                    line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                    line += deltaコマ下 * j
                    item.m_lineList.Add(line)
                End If
            Next
        Next
        itemlist.AddItem(item)


        '折り返し線
        Dim itemFolding As clsImageItem = Nothing
        If 0 < d折り返し高さ AndAlso Not isBottomOnly Then
            itemFolding = New clsImageItem(clsImageItem.ImageTypeEnum._折り返し線, 1)
        End If


        '上の側面(前面の画像)
        a四隅.p左下 = a底.p左上
        a四隅.p右下 = a底.p右上
        delta = Unit90 * d側面の高さ
        a四隅.p左上 = a四隅.p左下 + delta
        a四隅.p右上 = a四隅.p右下 + delta
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._front, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 2)
            item.m_a四隅 = a四隅
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._front, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._front)
            itemlist.AddItem(item)
        End If

        '上の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅 = a四隅
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
            line += Unit270 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To p_i編みひもの本数 - 1
            If i < p_i編みひもの本数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                line += deltaコマ下 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
            line += Unit90 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '下の側面(背面の画像)
        a四隅.p左上 = a底.p左下
        a四隅.p右上 = a底.p右下
        delta = Unit270 * d側面の高さ
        a四隅.p左下 = a四隅.p左上 + delta
        a四隅.p右下 = a四隅.p右上 + delta
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._back, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 3)
            item.m_a四隅 = a四隅
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._back, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._back)
            itemlist.AddItem(item)
        End If

        '下の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅 = a四隅
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下)
            line += Unit90 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To p_i編みひもの本数
            If i < p_i編みひもの本数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上)
                line += deltaコマ下 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下)
            line += Unit270 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '左の側面(左側面の画像)
        a四隅.p右上 = a底.p左上
        a四隅.p右下 = a底.p左下
        delta = Unit180 * d側面の高さ
        a四隅.p左上 = a四隅.p右上 + delta
        a四隅.p左下 = a四隅.p右下 + delta
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._leftside, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 4)
            item.m_a四隅 = a四隅
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._leftside, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._leftside)
            itemlist.AddItem(item)
        End If

        '左の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅 = a四隅
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
            line += Unit0 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To p_i編みひもの本数
            If i < p_i編みひもの本数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
            line += Unit180 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)

        '右の側面(右側面の画像)
        a四隅.p左上 = a底.p右上
        a四隅.p左下 = a底.p右下
        delta = Unit0 * d側面の高さ
        a四隅.p右上 = a四隅.p左上 + delta
        a四隅.p右下 = a四隅.p左下 + delta
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._rightside, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 5)
            item.m_a四隅 = a四隅
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._rightside, False)
            item.m_angle = _PlateAngle(enumBasketPlateIdx._rightside)
            itemlist.AddItem(item)
        End If

        '右の側面
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅 = a四隅
        If itemFolding IsNot Nothing Then
            line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下)
            line += Unit180 * d折り返し高さ
            itemFolding.m_lineList.Add(line)
        End If
        For i As Integer = 1 To p_i編みひもの本数
            If i < p_i編みひもの本数 AndAlso sideline Then
                line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下)
                line += deltaコマ右 * i
                item.m_lineList.Add(line)
            End If
        Next
        If 0 < _d縁の高さ Then
            line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下)
            line += Unit0 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)


        If itemFolding IsNot Nothing Then
            itemlist.AddItem(itemFolding)
        End If

        Return itemlist
    End Function


    '側面の外向きベクトル(長さ1)
    Shared _unit225 As New S差分(225) '左側面
    Shared _unit135 As New S差分(135) '前面
    Shared _unit45 As New S差分(45) '右側面
    Shared _unit315 As New S差分(315) '背面

    '底と側面枠(斜め)
    '底のみ=isBottomOnly =isAllBand
    Function imageList斜め描画要素(ByVal isBottomOnly As Boolean, ByVal isAllBand As Boolean) As clsImageItemList
        '
        '    (1,1)    A3        A2      → p_iコマ空間幅
        '       +----------------------+                                  
        '       |    ／＼     ／＼     |           ※coorBaseXY:コマ位置の左上を返す  
        '       |  ／前面＼ ／右面＼   |                       
        '     B3|／    A1 ／＼      ＼ |D2              
        '       |＼     ／    ＼ D1  ／|              
        '       |   ＼／横       ＼／  | 
        '       |   ／＼         ／＼  |  
        '       | ／ B1  ＼縦  ／    ＼|  
        '     B2| ＼       ＼／C1    ／|D3
        '       |   ＼左面／ ＼背面／  |
        '       |     ＼／     ＼／    |
        '       +----------------------+                                  
        '    ↓        C2       C3
        'p_iコマ空間高さ

        Dim pA1 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + _i横のコマ数 + p_i側面の切捨コマ数, 1 + p_i側面の切捨コマ数))
        Dim pB1 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + p_i側面の切捨コマ数, 1 + _i横のコマ数 + p_i側面の切捨コマ数))
        Dim pC1 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + _i縦のコマ数 + p_i側面の切捨コマ数, 1 + _i横のコマ数 + p_i側面の切捨コマ数 + _i縦のコマ数))
        Dim pD1 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + _i横のコマ数 + p_i側面の切捨コマ数 + _i縦のコマ数, 1 + _i縦のコマ数 + p_i側面の切捨コマ数))

        Dim d As Double = IIf(p_b側面半コマ, 0.5, 0)
        Dim pA2 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + d + _i横のコマ数 + 2 * p_i側面の切捨コマ数, 1 - d))
        Dim pA3 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 - d + _i横のコマ数, 1 - d))
        Dim pB2 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 - d, 1 + d + _i横のコマ数 + 2 * p_i側面の切捨コマ数))
        Dim pB3 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 - d, 1 - d + _i横のコマ数))
        Dim pC2 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 - d + _i縦のコマ数, 1 + d + p_iコマ空間高さ))
        Dim pC3 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + d + _i縦のコマ数 + 2 * p_i側面の切捨コマ数, 1 + d + p_iコマ空間高さ))
        Dim pD2 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + d + p_iコマ空間幅, 1 - d + _i縦のコマ数))
        Dim pD3 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + d + p_iコマ空間幅, 1 + d + _i縦のコマ数 + 2 * p_i側面の切捨コマ数))


        '折り返し線
        Dim d折り返し高さ As Double = 0
        Dim itemFolding As clsImageItem = Nothing
        If 0 < _i折り返しコマ数 Then
            itemFolding = New clsImageItem(clsImageItem.ImageTypeEnum._折り返し線, 1)
            d折り返し高さ = -1 * _dコマベース寸法 * _i折り返しコマ数 * ROOT2
        End If

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim line As S線分


        Dim a底 As New S四隅(pA1, pB1, pC1, pD1)
        '** 底の画像
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._bottom, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 1)
            item.m_a四隅 = a底
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._bottom, False)
            item.m_angle = _PlateAngle45(enumBasketPlateIdx._bottom)
            itemlist.AddItem(item)
        End If
        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠, 1)
        item.m_a四隅 = a底
        itemlist.AddItem(item)
        '底編みの枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 0)
        item.m_a四隅 = New S四隅(a底.r外接領域)
        itemlist.AddItem(item)

        '** 前面(左上の側面)_unit135
        Dim a前面 As New S四隅(pA3, pB3, pB1, pA1)
        '枠線
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅 = a前面
        If 0 < _d縁の高さ Then
            line = New S線分(a前面.pA, a前面.pB)
            line += _unit135 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)
        '折り返し線
        line = New S線分(a前面.pA, a前面.pB)
        line += _unit135 * d折り返し高さ
        If itemFolding IsNot Nothing Then
            itemFolding.m_lineList.Add(line)
        End If
        '画像
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._front, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 2)
            item.m_a四隅 = New S四隅(line.p開始, line.p終了, a前面.pC, a前面.pD)
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._front, False)
            item.m_angle = _PlateAngle45(enumBasketPlateIdx._front)
            itemlist.AddItem(item)
        End If

        '** 背面(右下の側面)_unit315
        Dim a背面 As New S四隅(pD1, pC1, pC3, pD3)
        '枠線
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅 = a背面
        If 0 < _d縁の高さ Then
            line = New S線分(a背面.pC, a背面.pD)
            line += _unit315 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)
        '折り返し線
        line = New S線分(a背面.pC, a背面.pD)
        line += _unit315 * d折り返し高さ
        If itemFolding IsNot Nothing Then
            itemFolding.m_lineList.Add(line)
        End If
        '画像
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._back, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 3)
            item.m_a四隅 = New S四隅(a背面.pA, a背面.pB, line.p開始, line.p終了)
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._back, False)
            item.m_angle = _PlateAngle45(enumBasketPlateIdx._back)
            itemlist.AddItem(item)
        End If


        '** 左側面(左下の側面)_unit225
        Dim a左側面 As New S四隅(pB1, pB2, pC2, pC1)
        '枠線
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅 = a左側面
        If 0 < _d縁の高さ Then
            line = New S線分(a左側面.pB, a左側面.pC)
            line += _unit225 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)
        '折り返し線
        line = New S線分(a左側面.pB, a左側面.pC)
        line += _unit225 * d折り返し高さ
        If itemFolding IsNot Nothing Then
            itemFolding.m_lineList.Add(line)
        End If
        '画像
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._leftside, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 4)
            item.m_a四隅 = New S四隅(a左側面.pA, line.p開始, line.p終了, a左側面.pD)
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._leftside, False)
            item.m_angle = _PlateAngle45(enumBasketPlateIdx._leftside)
            itemlist.AddItem(item)
        End If

        '** 右側面(右上の側面)_unit45
        Dim a右側面 As New S四隅(pA2, pA1, pD1, pD2)
        '枠線
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅 = a右側面
        If 0 < _d縁の高さ Then
            line = New S線分(a右側面.pD, a右側面.pA)
            line += _unit45 * _d縁の高さ
            item.m_lineList.Add(line)
        End If
        itemlist.AddItem(item)
        '折り返し線
        line = New S線分(a右側面.pD, a右側面.pA)
        line += _unit45 * d折り返し高さ
        If itemFolding IsNot Nothing Then
            itemFolding.m_lineList.Add(line)
        End If
        '画像
        If Not String.IsNullOrWhiteSpace(p_sPlatePngFilePath(enumBasketPlateIdx._rightside, False)) Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._画像保存, 5)
            item.m_a四隅 = New S四隅(line.p終了, a右側面.pB, a右側面.pC, line.p開始)
            item.m_fpath = p_sPlatePngFilePath(enumBasketPlateIdx._rightside, False)
            item.m_angle = _PlateAngle45(enumBasketPlateIdx._rightside)
            itemlist.AddItem(item)
        End If

        '折り返し線
        If itemFolding IsNot Nothing Then
            itemlist.AddItem(itemFolding)
        End If

        '以下、ペアのコマがあるとき
        If 0 = p_i側面の切捨コマ数 OrElse isBottomOnly Then
            Return itemlist
        End If
        If isAllBand Then
            'ペアのコマを接続
            For i As Integer = 1 To p_i側面の切捨コマ数
                Dim len As Double = _dコマベース寸法_対角 * (i - 0.5)
                Dim aR As New S四隅(New S領域(New S実座標(-len, -len), New S実座標(len, len)))
                Dim delta As New S差分(pOrigin, pD1)
                '右
                item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域線, 100 + i)
                item.m_a四隅 = aR + New S差分(pOrigin, pD1)
                item.m_is円 = True
                item.m_ltype = LineTypeEnum._black_dot
                item.m_angleStart = -45
                item.m_angleSweep = 90
                itemlist.AddItem(item)

                '上
                item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域線, 100 + i)
                item.m_a四隅 = aR + New S差分(pOrigin, pA1)
                item.m_is円 = True
                item.m_ltype = LineTypeEnum._black_dot
                item.m_angleStart = 225
                item.m_angleSweep = 90
                itemlist.AddItem(item)

                '左
                item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域線, 100 + i)
                item.m_a四隅 = aR + New S差分(pOrigin, pB1)
                item.m_is円 = True
                item.m_ltype = LineTypeEnum._black_dot
                item.m_angleStart = 135
                item.m_angleSweep = 90
                itemlist.AddItem(item)

                '下
                item = New clsImageItem(clsImageItem.ImageTypeEnum._四隅領域線, 100 + i)
                item.m_a四隅 = aR + New S差分(pOrigin, pC1)
                item.m_is円 = True
                item.m_ltype = LineTypeEnum._black_dot
                item.m_angleStart = 45
                item.m_angleSweep = 90
                itemlist.AddItem(item)
            Next

        Else
            '面から出る部分は半透明化
            Dim over As Double = _dコマの寸法 / 2
            pA3 += _unit135 * over
            pB3 += _unit135 * over
            pB2 += _unit225 * over
            pC2 += _unit225 * over
            pC3 += _unit315 * over
            pD3 += _unit315 * over
            pD2 += _unit45 * over
            pA2 += _unit45 * over

            Dim alfa As Integer = 160
            item = New clsImageItem(clsImageItem.ImageTypeEnum._半透明白, 1)
            item.m_a四隅 = New S四隅(New S線分(pA3, pA2).p中点, pA3, pA1, pA2)
            item.m_alfa = alfa
            itemlist.AddItem(item)
            '
            item = New clsImageItem(clsImageItem.ImageTypeEnum._半透明白, 2)
            item.m_a四隅 = New S四隅(pB3, New S線分(pB3, pB2).p中点, pB2, pB1)
            item.m_alfa = alfa
            itemlist.AddItem(item)
            '
            item = New clsImageItem(clsImageItem.ImageTypeEnum._半透明白, 3)
            item.m_a四隅 = New S四隅(pC1, pC2, New S線分(pC2, pC3).p中点, pC3)
            item.m_alfa = alfa
            itemlist.AddItem(item)
            '
            item = New clsImageItem(clsImageItem.ImageTypeEnum._半透明白, 4)
            item.m_a四隅 = New S四隅(pD2, pD1, pD3, New S線分(pD3, pD2).p中点)
            item.m_alfa = alfa
            itemlist.AddItem(item)
        End If

        Return itemlist
    End Function

#Region "開始位置"

    Function imageList開始位置(ByVal dひも幅 As Double, ByVal isKnotLeft As Boolean, ByVal outp As clsOutput, ByVal isKnotFrame As Boolean, ByVal isStartPosition As Boolean) As clsImageItemList

        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList
        Dim line As clsImageItem.S線分

        '要尺の折り位置
        Dim foldingLen As Double = c_foldingRatio * _dコマの要尺

        '要尺
        If True Then
            'コマ空間の左上位置
            'Dim p要尺位置 As S実座標 = toPoint(-(_i横のコマ数 / 2) - p_i編みひもの本数, -(_i縦のコマ数 / 2) - p_i編みひもの本数)
            Dim p要尺位置 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1, 1))

            If _b斜め立ち上げ Then
                If _i横のコマ数 * _dコマベース寸法 / ROOT2 < _dコマベース要尺 Then
                    p要尺位置 = p要尺位置 + Unit90 * (_dコマベース寸法)
                End If

            Else
                If p_i編みひもの本数 = 0 Then
                    p要尺位置 = p要尺位置 + Unit90 * (_dコマベース寸法)
                End If
                If (p_i編みひもの本数 - 1) * _dコマベース寸法 < _dコマベース要尺 Then
                    p要尺位置.X = -(1 + _i横のコマ数 / 2) * _dコマベース寸法 - (_dコマベース要尺 / 2)
                Else
                    p要尺位置.X = p要尺位置.X + (_dコマベース要尺 / 2)
                End If
            End If

            item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 9)
            Dim delta As S差分 = Unit270 * dひも幅
            item.m_a四隅.p左上 = p要尺位置 + Unit180 * (_dコマベース要尺 / 2) + Unit90 * (dひも幅 / 2)
            item.m_a四隅.p右上 = p要尺位置 + Unit0 * (_dコマベース要尺 / 2) + Unit90 * (dひも幅 / 2)
            item.m_a四隅.p左下 = item.m_a四隅.p左上 + delta
            item.m_a四隅.p右下 = item.m_a四隅.p右上 + delta
            Dim p As S実座標 = p要尺位置
            If 0 < _dコマ間のすき間 Then
                p = item.m_a四隅.p左上 + Unit0 * (_dコマ間のすき間 / 2)
                line = New S線分(p, p + delta)
                item.m_lineList.Add(line)
                p = item.m_a四隅.p右上 + Unit180 * (_dコマ間のすき間 / 2)
                line = New S線分(p, p + delta)
                item.m_lineList.Add(line)
            End If
            '中心から各折り位置まで
            Dim len_1 As Double = (_dコマの要尺 - foldingLen * 2) / 2 - (c_tiltRatio * dひも幅 / 2)
            Dim len_2 As Double = (_dコマの要尺 - foldingLen * 2) / 2 + (c_tiltRatio * dひも幅 / 2)
            If isKnotLeft Then
                Dim tmp As Double = len_1
                len_1 = len_2
                len_2 = tmp
            End If
            line = New S線分(New S実座標(p要尺位置.X + len_1, p要尺位置.Y + dひも幅 / 2), New S実座標(p要尺位置.X + len_2, p要尺位置.Y - dひも幅 / 2))
            item.m_lineList.Add(line)
            line = New S線分(New S実座標(p要尺位置.X - len_1, p要尺位置.Y - dひも幅 / 2), New S実座標(p要尺位置.X - len_2, p要尺位置.Y + dひも幅 / 2))
            item.m_lineList.Add(line)
            itemlist.AddItem(item)
        End If


        '開始位置情報
        If Not isStartPosition Then
            Return itemlist 'ここまでの結果
        End If

        '開始位置の情報をセット
        Dim startInfo As CStartInfo = setStartInfo()
        If startInfo Is Nothing Then
            '設定されていなければ
            Return itemlist 'ここまでの結果
        End If

        '底の開始位置のマーク
        If True Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)
            'Dim pコマ左上 As S実座標 = toPoint(-(_i横のコマ数 / 2) + (startInfo.i左から何番目 - 1), -(_i縦のコマ数 / 2) + (startInfo.i上から何番目 - 1))
            Dim pコマ左上 As S実座標 = toPoint(startInfo.StartKoma.coorBaseXY())
            Dim pコマ右下 As S実座標 = pコマ左上 + Unit315 * _dコマベース寸法

            line = New clsImageItem.S線分(pコマ左上, pコマ左上 + Unit0 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ左上, pコマ左上 + Unit270 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ右下, pコマ右下 + Unit90 * _dコマベース寸法)
            item.m_lineList.Add(line)
            line = New clsImageItem.S線分(pコマ右下, pコマ右下 + Unit180 * _dコマベース寸法)
            item.m_lineList.Add(line)

            itemlist.AddItem(item)
        End If

        'コマが描かれた領域
        Dim rDraw As S領域 = clsImageData.ItemDrawingRect(_ImageListコマ)
        'コマ空間の下中央位置
        Dim pコマ空間下 As S実座標 = toPoint(_KnotFolderSpace.coorBaseXY(1 + p_iコマ空間幅 / 2, 1 + p_iコマ空間高さ))
        If rDraw.y最下 < pコマ空間下.Y Then
            pコマ空間下.Y = rDraw.y最下
        End If

        '開始位置のコマの転記
        Dim dバンド長 As Double = _dコマベース要尺
        Dim pコマ位置 As S実座標 = pコマ空間下 + Unit270 * (dバンド長 + _dコマベース寸法)

        Dim knot = New CKnot(pコマ位置, _dコマの寸法, _dコマ間のすき間, isKnotLeft)

        Dim bandY As New CBand(startInfo.row横展開)
        Dim bandT As New CBand(startInfo.row縦展開)
        knot.SetBandYH(bandY, bandT, dひも幅, True) '開始位置表示用に傾ける

        If isKnotFrame Then
            knot.SetRegionDisp(False, True) 'コマ寸法+すき間
        End If

        'next index
        item = New clsImageItem(knot, p_i横ひもの本数 + 1, p_i縦ひもの本数 + 1)

        itemlist.AddItem(item)

        'コマに続くバンド
        Dim d描画幅 As Double = item.m_knot.Get描画幅()
        If True Then
            '*左右
            Dim item1r As New S領域 '右
            Dim item2r As New S領域 '左

            item1r.p左下 = pコマ位置 + Unit0 * d描画幅
            item2r.p右上 = pコマ位置 + Unit180 * d描画幅
            If Not isKnotLeft Then
                item1r.p左下 = item1r.p左下 + Unit270 * dひも幅
                item2r.p右上 = item2r.p右上 + Unit90 * dひも幅
            End If
            item1r.p右上 = New S実座標(item1r.p左下.X + dバンド長, item1r.p左下.Y + dひも幅)
            item2r.p左下 = New S実座標(item2r.p右上.X - dバンド長, item2r.p右上.Y - dひも幅)

            'バンド化
            '始点T(D)　　 　　　終点T(C)
            '　　[□□→(0)□□]　　　↑deltaAx(90)
            '始点F(A) 　　　　　終点F(B)
            Dim band1 As New CBand(startInfo.row横展開)
            Dim band2 As New CBand(startInfo.row横展開)
            band1.aバンド位置 = New S四隅(item1r.p左下, item1r.p右下, item1r.p右上, item1r.p左上)
            band2.aバンド位置 = New S四隅(item2r.p左下, item2r.p右下, item2r.p右上, item2r.p左上)
            Dim item1 As New clsImageItem(band1, IdxDrawBandStart, 1) '右
            Dim item2 As New clsImageItem(band2, IdxDrawBandStart, 2) '左

            itemlist.AddItem(item1)
            itemlist.AddItem(item2)

            '*上下
            Dim item3r As New S領域 '上
            Dim item4r As New S領域 '下
            item3r.p左下 = pコマ位置 + Unit90 * d描画幅
            item4r.p右上 = pコマ位置 + Unit270 * d描画幅
            If isKnotLeft Then
                item3r.p左下 = item3r.p左下 + Unit180 * dひも幅 '
                item4r.p右上 = item4r.p右上 + Unit0 * dひも幅 '
            End If
            item3r.p右上 = New S実座標(item3r.p左下.X + dひも幅, item3r.p左下.Y + dバンド長)
            item4r.p左下 = New S実座標(item4r.p右上.X - dひも幅, item4r.p右上.Y - dバンド長)

            'バンド化
            '始点F(A)□　始点T(D)　→deltaAx(0)
            '　　  　↓(270)
            '終点F(B)□　終点T(C)
            Dim band3 As New CBand(startInfo.row縦展開)
            Dim band4 As New CBand(startInfo.row縦展開)
            band3.aバンド位置 = New S四隅(item3r.p左上, item3r.p左下, item3r.p右下, item3r.p右上)
            band4.aバンド位置 = New S四隅(item4r.p左上, item4r.p左下, item4r.p右下, item4r.p右上)
            Dim item3 As New clsImageItem(band3, IdxDrawBandStart, 3)
            Dim item4 As New clsImageItem(band4, IdxDrawBandStart, 4)

            itemlist.AddItem(item3)
            itemlist.AddItem(item4)
        End If


        '「開始位置」
        Dim p開始位置 As S実座標 = pコマ位置 + Unit135 * (dバンド長) + Unit180 * (dバンド長)
        item = New clsImageItem(p開始位置, {text開始位置()}, dひも幅, 1)
        itemlist.AddItem(item)

        '左から×番目,上から×番目
        Dim pos1 As String = text左から() & startInfo.i左から何番目.ToString & text番目のコマ()
        Dim pos2 As String = text上から() & startInfo.i上から何番目.ToString & text番目のコマ()
        item = New clsImageItem(p開始位置 + Unit315 * (dひも幅 * 2), {pos1, pos2}, dひも幅 * 2 / 3, 2)
        itemlist.AddItem(item)


        '各方向の説明文字列 
        If True Then
            Dim p文字(cSideIndexEnumCount - 1) As S実座標
            '上のひも
            p文字(SideIndexEnum._上側).X = pコマ位置.X + dひも幅 * 2
            p文字(SideIndexEnum._上側).Y = pコマ位置.Y + dバンド長
            '下のひも
            p文字(SideIndexEnum._下側).X = pコマ位置.X + dひも幅 * 2
            p文字(SideIndexEnum._下側).Y = pコマ位置.Y - 2 * dバンド長 / 3
            '左のひも
            p文字(SideIndexEnum._左側).X = pコマ位置.X - dバンド長 - dひも幅 * 15 '想定文字数分
            p文字(SideIndexEnum._左側).Y = pコマ位置.Y - dひも幅 * 1.5
            '右のひも
            p文字(SideIndexEnum._右側).X = pコマ位置.X + dバンド長 + _dコマの寸法
            p文字(SideIndexEnum._右側).Y = pコマ位置.Y + dひも幅

            startInfo.setMyValue(True) 'マイひも長係数を乗算する
            For i As Integer = 0 To cSideIndexEnumCount - 1
                With startInfo
                    '<コマの{0}>
                    Dim str1 As String = String.Format(My.Resources.CalcOutKnotOf, .getSideString(i))
                    str1 += outp.outLengthTextWithUnit(.getBandLength(i))
                    str1 += "  "
                    str1 += .getDiffString(i, outp)

                    '加算計 {0} (縁の始末:{1} ひも長加算:{2} {3}加算:{4})
                    Dim str2 As String = String.Format(My.Resources.CalcOutAddLen,
                        outp.outLengthTextWithUnit(.getAddSum(i)), outp.outLengthText(.getAddHem()), outp.outLengthText(.getAddVert()), .getSideString(i), outp.outLengthText(.getAddEach(i)))

                    '折り位置から
                    Dim str3 As String = String.Format(My.Resources.CalcOutFromFolding)
                    str3 += outp.outLengthTextWithUnit(.getFoldingLength(i))
                    str3 += "  "
                    str3 += .getDiffFoldingString(i, outp)
                    '
                    item = New clsImageItem(p文字(i), {str1, str2, str3}, dひも幅 * 2 / 3, i + 3)
                    itemlist.AddItem(item)
                End With
            Next
        End If

        'コマ位置の中央線
        startInfo.setMyValue(False) 'マイひも長係数を乗算しない
        If True Then
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 2)
            Dim d1本幅 As Double = dひも幅 / _I基本のひも幅
            If Abs(startInfo.getDiff(SideIndexEnum._上側)) < dバンド長 * 2 + _dコマベース要尺 + d1本幅 Then
                '上下中央の横線
                Dim pp As New S実座標(pコマ位置.X - dひも幅 * 1.5, pコマ位置.Y)
                line = New S線分(pp, pp + Unit0 * dひも幅 * 3)
                If Abs(startInfo.getDiff(SideIndexEnum._上側)) < d1本幅 Then
                    '1本幅以下
                    item.m_lineList.Add(line)
                ElseIf (_dコマの要尺 / 2 - d1本幅) <= startInfo.getDiff(SideIndexEnum._上側) Then
                    '要尺以上、上が長いので中央点は上にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(SideIndexEnum._上側) - _dコマベース要尺) / 2
                    line = line + Unit90 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                ElseIf startInfo.getDiff(SideIndexEnum._上側) <= -(_dコマの要尺 / 2 - d1本幅) Then
                    '要尺以上、下が長いので中央点は下にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(SideIndexEnum._下側) - _dコマベース要尺) / 2
                    line = line + Unit270 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                Else
                    '描けません
                End If
            End If
            If Abs(startInfo.getDiff(SideIndexEnum._左側)) < dバンド長 * 2 + _dコマベース要尺 + d1本幅 Then
                '左右中央の縦線
                Dim pp As New S実座標(pコマ位置.X, pコマ位置.Y - dひも幅 * 1.5)
                line = New S線分(pp, pp + Unit90 * dひも幅 * 3)
                If Abs(startInfo.getDiff(SideIndexEnum._左側)) < d1本幅 Then
                    '1本幅以下
                    item.m_lineList.Add(line)
                ElseIf (_dコマの要尺 / 2 - d1本幅) <= startInfo.getDiff(SideIndexEnum._左側) Then
                    '要尺以上、左が長いので中央点は左にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(SideIndexEnum._左側) - _dコマベース要尺) / 2
                    line = line + Unit180 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                ElseIf startInfo.getDiff(SideIndexEnum._左側) <= -(_dコマの要尺 / 2 - d1本幅) Then
                    '要尺以上、右が長いので中央点は右にある
                    Dim distance_from_knot_area As Double = (startInfo.getDiff(SideIndexEnum._右側) - _dコマベース要尺) / 2
                    line = line + Unit0 * (_dコマベース寸法 / 2 + distance_from_knot_area)
                    item.m_lineList.Add(line)
                Else
                    '描けません
                End If
            End If
            If 0 < item.m_lineList.Count Then
                itemlist.AddItem(item)
            End If
        End If


        Return itemlist
    End Function
#End Region

    'プレビュー画像生成
    '底のみ=isBottomOnly, ひも全体=isAllBand, コマ枠=isKnotFrame
    Public Function CalcImage(ByVal imgData As clsImageData, ByVal isDrawMark As Boolean,
                              ByVal isBottomOnly As Boolean, ByVal isAllBand As Boolean, ByVal isKnotFrame As Boolean, ByVal isStartPosition As Boolean) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '念のため
        _ImageListコマ = Nothing
        _ImageList描画要素 = Nothing
        _ImageList開始位置 = Nothing

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        ''記号表示の位置をセット
        'Dim isKnotLeft As Boolean = False
        'Select Case _Data.p_row底_縦横.Value("f_iコマ上側の縦ひも")
        '    Case enumコマ上側の縦ひも.i_どちらでも
        '        isKnotLeft = My.Settings.IsKnotLeft
        '    Case enumコマ上側の縦ひも.i_左側
        '        isKnotLeft = True
        '    Case enumコマ上側の縦ひも.i_右側
        '        isKnotLeft = False
        'End Select

        '文字サイズと基本色
        imgData.setBasics(_dコマの寸法, _Data.p_row目標寸法.Value("f_s基本色"))

        '_ImageListコマにセット(CalcOutput結果に基づく)
        If Not imagelistコマ配置(_bコマ上縦ひも左側, isDrawMark,
                             isBottomOnly, isAllBand, isKnotFrame) Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If
        _ImageList開始位置 = imageList開始位置(_d基本のひも幅, _bコマ上縦ひも左側, outp, isKnotFrame, isStartPosition)

        '底と側面枠
        If _b斜め立ち上げ Then
            _ImageList描画要素 = imageList斜め描画要素(isBottomOnly, isAllBand)
        Else
            _ImageList描画要素 = imageList描画要素(isBottomOnly)
        End If

        '中身を移動
        imgData.MoveList(_ImageListコマ)
        _ImageListコマ = Nothing
        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing
        imgData.MoveList(_ImageList開始位置)
        _ImageList開始位置 = Nothing

        '付属品
        AddPartsImage(imgData, _Data, False) '描画

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function

End Class
