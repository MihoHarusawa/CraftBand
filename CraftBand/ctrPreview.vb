Imports System.Drawing
Imports System.Windows.Forms


Public Class ctrPreview

    'Panelを置き、各ControlはPanelにAnchorし、Panelをコードでリサイズする
    '※Panelは(0,0)位置に置いています。(3,3)→(-3,-3)ではない。
    'タブコントロールに配置する際は、Dock=Full としてください。

    Public Property PanelSize As Drawing.Size
        Set(value As Drawing.Size)
            If Not _isLoadingData Then
                Panel.Size = New Size(value.Width, value.Height)
                LayoutControls()
            End If
        End Set
        Get
            Return Panel.Size
        End Get
    End Property

    Dim _isLoadingData As Boolean = True 'Designer.vb描画
    Dim _FormCaption As String
    Dim _TabPageName As String
    Dim _clsImageData As clsImageData

    Private Sub ctrPreview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '※グローバル参照値は参照不可

        '    picPreview.Dock = DockStyle.None
        '    picPreview.SizeMode = PictureBoxSizeMode.StretchImage

        '    TrackBar1.Minimum = 0
        '    TrackBar1.Maximum = 1000
        '    TrackBar1.Value = 0
        '    TrackBar1.TickStyle = TickStyle.None
        _isLoadingData = False 'Designer.vb描画完了
    End Sub

    'Load後に一度だけセットしてください
    Sub SetNames(ByVal formcaption As String, ByVal tabname As String, ByVal is3D As Boolean)
        _FormCaption = formcaption
        _TabPageName = tabname
        If is3D Then
            grp3D.Visible = True
            btn3Dモデル.Visible = True
        End If
    End Sub


    Public Sub ShowImage(ByVal clsImageData As clsImageData)
        unloadImage()

        _clsImageData = clsImageData
        loadImage(_clsImageData.GifFilePath)
    End Sub

    Public Sub ClearImage()
        _clsImageData = Nothing
        unloadImage()
    End Sub

    Public Function IsRadioViewerChecked() As Boolean
        Return radビューア.Checked
    End Function


#Region "スライダーでズーム"

    Private m_FitZoom As Double 'Panelに収まる倍率
    Private m_Zoom As Double '現在倍率
    Private m_Image As Image
    '実寸の2倍まで
    Private Const MAX_ZOOM As Double = 2.0

    '
    Private Sub loadImage(filename As String)
        m_Image = Image.FromFile(filename)

        picPreview.Image = m_Image

        'スクロール位置リセット
        panelPreview.AutoScrollPosition = New Point(0, 0)

        'PictureBox位置リセット
        picPreview.Location = Point.Empty

        '全体表示倍率を再計算
        calcFitZoom()

        'スライダーを最小(全体表示)へ
        trkbarPreviewImage.Value = trkbarPreviewImage.Minimum

        m_Zoom = m_FitZoom

        applyZoom()
    End Sub

    Private Sub unloadImage()
        If m_Image IsNot Nothing Then
            picPreview.Image = Nothing
            m_Image.Dispose()
            m_Image = Nothing
        End If
    End Sub

    'Fit倍率計算
    Private Sub calcFitZoom()
        If m_Image Is Nothing Then Return

        Dim zx As Double = CDbl(panelPreview.ClientSize.Width) / m_Image.Width
        Dim zy As Double = CDbl(panelPreview.ClientSize.Height) / m_Image.Height
        m_FitZoom = Math.Min(zx, zy)
    End Sub

    'スライダー値から倍率を計算
    Private Sub updateZoomFromTrackBar()

        Dim rate As Double = CDbl(trkbarPreviewImage.Value - trkbarPreviewImage.Minimum) / (trkbarPreviewImage.Maximum - trkbarPreviewImage.Minimum)
        m_Zoom = m_FitZoom + (MAX_ZOOM - m_FitZoom) * rate
        applyZoom()
    End Sub

    'ズーム適用
    Private Sub applyZoom()
        If m_Image Is Nothing Then Return

        Dim oldCenterX As Integer = -panelPreview.AutoScrollPosition.X + panelPreview.ClientSize.Width \ 2
        Dim oldCenterY As Integer = -panelPreview.AutoScrollPosition.Y + panelPreview.ClientSize.Height \ 2

        Dim scaleX As Double = 1
        Dim scaleY As Double = 1

        If picPreview.Width > 0 Then
            scaleX = m_Zoom / (picPreview.Width / CDbl(m_Image.Width))
        End If

        If picPreview.Height > 0 Then
            scaleY = m_Zoom / (picPreview.Height / CDbl(m_Image.Height))
        End If

        picPreview.Size = New Size(CInt(m_Image.Width * m_Zoom), CInt(m_Image.Height * m_Zoom))

        centerIfSmaller()

        If picPreview.Width > panelPreview.ClientSize.Width Then
            Dim newCenterX As Integer = CInt(oldCenterX * scaleX)

            panelPreview.AutoScrollPosition =
            New Point(
                Math.Max(0, newCenterX - panelPreview.ClientSize.Width \ 2),
                Math.Max(0, -panelPreview.AutoScrollPosition.Y))
        End If

        If picPreview.Height > panelPreview.ClientSize.Height Then
            Dim newCenterY As Integer = CInt(oldCenterY * scaleY)

            panelPreview.AutoScrollPosition =
            New Point(
                Math.Max(0, -panelPreview.AutoScrollPosition.X),
                Math.Max(0, newCenterY - panelPreview.ClientSize.Height \ 2))
        End If
    End Sub

    '小さい時だけ中央寄せ
    Private Sub centerIfSmaller()
        If picPreview.Width < panelPreview.ClientSize.Width Then
            picPreview.Left = (panelPreview.ClientSize.Width - picPreview.Width) \ 2
        Else
            picPreview.Left = 0
        End If

        If picPreview.Height < panelPreview.ClientSize.Height Then
            picPreview.Top = (panelPreview.ClientSize.Height - picPreview.Height) \ 2
        Else
            picPreview.Top = 0
        End If
    End Sub

    'スライダーの位置をイメージの中央にする
    Private Sub LayoutControls()
        trkbarPreviewImage.Left = panelPreview.Left + (panelPreview.Width - trkbarPreviewImage.Width) \ 2
        trkbarPreviewImage.Top = panelPreview.Bottom + 4
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles trkbarPreviewImage.Scroll
        If m_Image Is Nothing Then Return

        updateZoomFromTrackBar()
    End Sub

    Private Sub picPreview_Resize(sender As Object, e As EventArgs) Handles picPreview.Resize
        If m_Image Is Nothing Then Return

        calcFitZoom()
        If trkbarPreviewImage.Value = trkbarPreviewImage.Minimum Then
            '全体表示中
            m_Zoom = m_FitZoom
        End If

        applyZoom()
        LayoutControls()
    End Sub

    Private Sub panelPreview_Resize(sender As Object, e As EventArgs) Handles panelPreview.Resize
        If m_Image Is Nothing Then Return

        calcFitZoom()
        If trkbarPreviewImage.Value = trkbarPreviewImage.Minimum Then
            '全体表示中
            m_Zoom = m_FitZoom
        End If

        applyZoom()
        LayoutControls()
    End Sub
#End Region


    Private Sub btn3Dモデル_Click(sender As Object, e As EventArgs) Handles btn3Dモデル.Click
        If _clsImageData Is Nothing Then
            Return
        End If
        If Not _clsImageData.ModelFileOpen(Nothing) Then
            MessageBox.Show(_clsImageData.LastError, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btn画像ファイル_Click(sender As Object, e As EventArgs) Handles btn画像ファイル.Click
        If _clsImageData Is Nothing Then
            Return
        End If
        If Not _clsImageData.ImgFileOpen Then
            MessageBox.Show(_clsImageData.LastError, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btnブラウザ_Click(sender As Object, e As EventArgs) Handles btnブラウザ.Click
        If _clsImageData Is Nothing Then
            Return
        End If
        If Not _clsImageData.ImgBrowserOpen(clsImageData.cBrowserBasicInfo) Then
            MessageBox.Show(_clsImageData.LastError, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub



End Class
