Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports CraftBand
Imports CraftBand.clsDataTables
Imports CraftBand.clsImageItem
Imports CraftBand.clsUpDown
Imports CraftBand.Tables.dstDataTables

Partial Public Class clsCalcHexagon
    '                       
    '                  バンド方向                                                                    
    '    位置順     ↗  120度                                          バンド方向  ＼　位置順
    '   (30度方向)／  　 ＼      ＼      ＼   　 ／　　　／　　　／       60度 　　 ↘(-30度方向)
    '                      ＼  ──+──+─+──+─+──+──　／
    '                       ＼　　　＼／　　＼／　　＼／　　　／斜め60度　
    '                       　＼　　／＼　　／＼　　／＼　　／　　　       
    '                        ─+──+─+──+─+──+─+──横                                  
    '                       　　＼／　　＼／　　＼／　　＼／   ↑六つ目(ひも間)   バンド方向 ↓　位置順
    '                       　　／＼　　／＼　　／＼　　／＼   ↓                    0度     ↓(-90度方向)
    '                        ─+──+─+──+─+──+─+──横                                  
    '                       　／　　＼／　　＼／　　＼／　　＼　　　　
    '                       ／　　　／＼　　／＼　　／＼　　　＼斜め120度　　　
    '                    ／　  ──+──+─+──+─+──+──　　＼
    '                                                                       
    '
    '         
    'バンドの方向角
    Dim BandAngleDegree() As Double = {0, 60, 120}
    Dim DeltaBandAngle() As S差分 = {New S差分(0), New S差分(60), New S差分(120)}

    'バンドの位置順方向
    Dim BandOrderDegree() As Double = {-90, -30, 30}
    Dim DeltaBandOrder() As S差分 = {New S差分(-90), New S差分(-30), New S差分(30)}


    'プレビュー時に生成,描画後はNothing
    Dim _ImageList展開ひも(cAngleCount - 1) As clsImageItemList   '展開レコードを含む
    Dim _ImageList描画要素 As clsImageItemList '底と側面


    'プレビュー画像生成
    Public Function CalcImage(ByVal imgData As clsImageData) As Boolean
        If imgData Is Nothing Then
            '処理に必要な情報がありません。
            p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
            Return False
        End If

        '念のため
        For Each aidx As AngleIndex In [Enum].GetValues(GetType(AngleIndex))
            _ImageList展開ひも(idx(aidx)) = Nothing
        Next

        _ImageList描画要素 = Nothing

        '出力ひもリスト情報
        Dim outp As New clsOutput(imgData.FilePath)
        If Not CalcOutput(outp) Then
            Return False 'p_sメッセージあり
        End If

        '_tbl縦横展開にレコードが残されているはず
        '_ImageList展開ひもを作る
        For Each aidx As AngleIndex In [Enum].GetValues(GetType(AngleIndex))
            If Not imageList展開ひも(aidx) Then
                '処理に必要な情報がありません。
                p_sメッセージ = String.Format(My.Resources.CalcNoInformation)
                Return False
            End If
        Next

        '基本のひも幅(文字サイズ)と基本色
        imgData.setBasics(_d基本のひも幅, _Data.p_row目標寸法.Value("f_s基本色"))

        _ImageList描画要素 = imageList底と側面枠()




        '中身を移動
        For idx As Integer = 0 To cAngleCount - 1
            imgData.MoveList(_ImageList展開ひも(idx))
            _ImageList展開ひも(idx) = Nothing
        Next


        imgData.MoveList(_ImageList描画要素)
        _ImageList描画要素 = Nothing
        '        imgData.MoveList(_ImageList差しひも)
        '        _ImageList差しひも = Nothing

        '描画ファイル作成
        If Not imgData.MakeImage(outp) Then
            p_sメッセージ = imgData.LastError
            Return False
        End If

        Return True
    End Function



    '底の配置領域に立ち上げ増分をプラス, = p_d四角ベース_周
    Private Function get側面の周長() As Double
        Return 2 * (_d六角ベース計(0) + _d六角ベース計(1)) _
            + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分")
    End Function

    '横ひも長計算・描画に使用
    Private Function get周の横(Optional ByVal multi As Double = 1) As Double
        Return multi *
            (_d六角ベース計(0) + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4)
    End Function

    '縦ひも長計算・描画に使用
    Private Function get周の縦(Optional ByVal multi As Double = 1) As Double
        Return multi *
            (_d六角ベース計(1) + g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d立ち上げ時の四角底周の増分") / 4)
    End Function

    '縦ひも長計算・描画に使用   '縁は含まない
    Private Function get側面高(ByVal count As Integer) As Double
        Return count * (_d六角ベース_高さ計 + getZeroSide())
    End Function

    '側面の高さゼロ位置(1=1端)
    Private Function getZeroSide(Optional ByVal multi As Double = 1) As Double
        Return multi * g_clsSelectBasics.p_row選択中バンドの種類.Value("f_d底の厚さ")
    End Function

    '横ひもリストの描画情報 : _tbl縦横展開_横ひも → _ImageList横ひも
    Private Function imageList展開ひも(ByVal aidx As AngleIndex) As Boolean
        If _tbl縦横展開 Is Nothing OrElse _tbl縦横展開(idx(aidx)) Is Nothing Then
            Return False
        End If

        _ImageList展開ひも(idx(aidx)) = New clsImageItemList(_tbl縦横展開(idx(aidx)))
        If _tbl縦横展開(idx(aidx)).Rows.Count = 0 Then
            Return True
        End If


        Dim delta As S差分 = DeltaBandAngle(idx(aidx))
        Dim delta_vert As S差分 = DeltaBandOrder(idx(aidx))

        '最初のバンドの中心位置
        Dim p開始中心 As S実座標 = pOrigin + delta_vert * (-1 * _d六角ベース中(idx(aidx)))
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p開始中心 {0}", p開始中心)

        '位置番号順
        _ImageList展開ひも(idx(aidx)).SortByPosition()
        For Each band As clsImageItem In _ImageList展開ひも(idx(aidx))
            If band.m_row縦横展開 Is Nothing Then
                Continue For
            End If

            If is_idxすき間(aidx, band.m_row縦横展開.f_iひも種) Then
                'すき間は最初に来る
                band.m_ImageType = ImageTypeEnum._描画なし
                '幅を加えるだけ

            ElseIf is_idxひも種(aidx, band.m_row縦横展開.f_iひも種) Then
                '描画タイプ(再)指定
                band.m_ImageType = ImageTypeEnum._バンド

                'm_a四隅にバンド描画位置
                Dim bandwidth As Double = g_clsSelectBasics.p_d指定本幅(band.m_row縦横展開.f_i何本幅)
                Dim d横ひも表示長 As Double = band.m_row縦横展開.f_d出力ひも長

                band.m_a四隅.pA = p開始中心 + delta * (-d横ひも表示長 / 2)
                band.m_a四隅.pB = p開始中心 + delta * (d横ひも表示長 / 2)
                band.m_a四隅.pC = band.m_a四隅.pB + delta_vert * bandwidth
                band.m_a四隅.pD = band.m_a四隅.pA + delta_vert * bandwidth


                'm_rひも位置に記号描画位置
                band.m_rひも位置.p左下 = p開始中心 + delta * ((-d横ひも表示長 / 2) - bandwidth * 2)
                band.m_rひも位置.p右上 = band.m_rひも位置.p左下

                '
                g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "{0}:{1}", band.m_row縦横展開.f_i位置番号, band.m_a四隅)
            Else
                '補強ひもは描画しない
                band.m_ImageType = ImageTypeEnum._描画なし
                Continue For
            End If
            '
            p開始中心 = p開始中心 + delta_vert * band.m_row縦横展開.f_d幅
        Next

        Return True
    End Function

    '底と側面枠
    Function imageList底と側面枠() As clsImageItemList
        Dim item As clsImageItem
        Dim itemlist As New clsImageItemList

        Dim line As S線分
        Dim p1 As S実座標
        Dim p2 As S実座標
        Dim p3 As S実座標
        Dim p4 As S実座標
        Dim p5 As S実座標
        Dim p6 As S実座標



        '   p3 -- p2
        ' ／        ＼
        'p4          p1
        ' ＼        ／
        '   p5 -- p6

        If 1 < _iひもの本数(0) AndAlso 1 < _iひもの本数(1) AndAlso 1 < _iひもの本数(2) Then
            '真ん中の六つ目
            item = New clsImageItem(clsImageItem.ImageTypeEnum._底の中央線, 1)

            p1 = pOrigin + New S差分(0) * (_d六つ目の対角線 / 2)
            p2 = pOrigin + New S差分(60) * (_d六つ目の対角線 / 2)
            p3 = pOrigin + New S差分(120) * (_d六つ目の対角線 / 2)
            p4 = pOrigin + New S差分(180) * (_d六つ目の対角線 / 2)
            p5 = pOrigin + New S差分(240) * (_d六つ目の対角線 / 2)
            p6 = pOrigin + New S差分(300) * (_d六つ目の対角線 / 2)

            line = New S線分(p1, p2)
            item.m_lineList.Add(line)
            line = New S線分(p2, p3)
            item.m_lineList.Add(line)
            line = New S線分(p3, p4)
            item.m_lineList.Add(line)
            line = New S線分(p4, p5)
            item.m_lineList.Add(line)
            line = New S線分(p5, p6)
            item.m_lineList.Add(line)
            line = New S線分(p6, p1)
            item.m_lineList.Add(line)

            itemlist.AddItem(item)
        End If


        '底枠
        item = New clsImageItem(clsImageItem.ImageTypeEnum._底枠2, 1)

        'AngleIndex.idx_0
        Dim fn23 As New S直線式(0, pOrigin + DeltaBandOrder(0) * -_d六角ベース中(0))
        Dim fn56 As New S直線式(0, pOrigin + DeltaBandOrder(0) * (_d六角ベース計(0) - _d六角ベース中(0)))
        'AngleIndex.idx_60
        Dim fn34 As New S直線式(60, pOrigin + DeltaBandOrder(1) * -_d六角ベース中(1))
        Dim fn61 As New S直線式(60, pOrigin + DeltaBandOrder(1) * (_d六角ベース計(1) - _d六角ベース中(1)))
        'AngleIndex.idx_120
        Dim fn45 As New S直線式(120, pOrigin + DeltaBandOrder(2) * -_d六角ベース中(2))
        Dim fn12 As New S直線式(120, pOrigin + DeltaBandOrder(2) * (_d六角ベース計(2) - _d六角ベース中(2)))
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "fn23 {0}", fn23)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "fn56 {0}", fn56)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "fn34 {0}", fn34)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "fn61 {0}", fn61)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "fn45 {0}", fn45)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "fn12 {0}", fn12)

        p1 = fn12.p交点(fn61)
        p2 = fn23.p交点(fn12)
        p3 = fn34.p交点(fn23)
        p4 = fn45.p交点(fn34)
        p5 = fn56.p交点(fn45)
        p6 = fn61.p交点(fn56)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p1 {0}", p1)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p2 {0}", p2)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p3 {0}", p3)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p4 {0}", p4)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p5 {0}", p5)
        g_clsLog.LogFormatMessage(clsLog.LogLevel.Debug, "p6 {0}", p6)

        line = New S線分(p1, p2)
        item.m_lineList.Add(line)
        line = New S線分(p2, p3)
        item.m_lineList.Add(line)
        line = New S線分(p3, p4)
        item.m_lineList.Add(line)
        line = New S線分(p4, p5)
        item.m_lineList.Add(line)
        line = New S線分(p5, p6)
        item.m_lineList.Add(line)
        line = New S線分(p6, p1)
        item.m_lineList.Add(line)

        itemlist.AddItem(item)

        Dim d縁厚さプラス_高さ As Double = get側面高(1) + _d縁の高さ
        Dim d底の厚さ As Double = getZeroSide()

        'AngleIndex.idx_0
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 1)
        item.m_a四隅.p左下 = p3
        item.m_a四隅.p右下 = p2
        item.m_a四隅.p左上 = item.m_a四隅.p左下 + DeltaBandOrder(0) * -d縁厚さプラス_高さ
        item.m_a四隅.p右上 = item.m_a四隅.p右下 + DeltaBandOrder(0) * -d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左下, item.m_a四隅.p右下) + DeltaBandOrder(0) * -d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)
        '        '
        item = New clsImageItem(clsImageItem.ImageTypeEnum._横の側面, 2)
        item.m_a四隅.p左上 = p5
        item.m_a四隅.p右上 = p6
        item.m_a四隅.p左下 = item.m_a四隅.p左上 + DeltaBandOrder(0) * d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p右上 + DeltaBandOrder(0) * d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p右上) + DeltaBandOrder(0) * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)


        'AngleIndex.idx_60
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅.p左上 = p3
        item.m_a四隅.p左下 = p4
        item.m_a四隅.p右上 = item.m_a四隅.p左上 + DeltaBandOrder(1) * -d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p左下 + DeltaBandOrder(1) * -d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下) + DeltaBandOrder(1) * -d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p右上 = p1
        item.m_a四隅.p右下 = p6
        item.m_a四隅.p左上 = item.m_a四隅.p右上 + DeltaBandOrder(1) * d縁厚さプラス_高さ
        item.m_a四隅.p左下 = item.m_a四隅.p右下 + DeltaBandOrder(1) * d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下) + DeltaBandOrder(1) * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        'AngleIndex.idx_120
        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 1)
        item.m_a四隅.p左上 = p4
        item.m_a四隅.p左下 = p5
        item.m_a四隅.p右上 = item.m_a四隅.p左上 + DeltaBandOrder(2) * -d縁厚さプラス_高さ
        item.m_a四隅.p右下 = item.m_a四隅.p左下 + DeltaBandOrder(2) * -d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p左上, item.m_a四隅.p左下) + DeltaBandOrder(2) * -d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)

        item = New clsImageItem(clsImageItem.ImageTypeEnum._縦の側面, 2)
        item.m_a四隅.p右上 = p1
        item.m_a四隅.p右下 = p2
        item.m_a四隅.p左上 = item.m_a四隅.p右上 + DeltaBandOrder(2) * d縁厚さプラス_高さ
        item.m_a四隅.p左下 = item.m_a四隅.p右下 + DeltaBandOrder(2) * d縁厚さプラス_高さ
        line = New S線分(item.m_a四隅.p右上, item.m_a四隅.p右下) + DeltaBandOrder(2) * d底の厚さ
        item.m_lineList.Add(line)
        itemlist.AddItem(item)


        Return itemlist
    End Function



End Class
