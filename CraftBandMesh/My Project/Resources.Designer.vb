﻿'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:4.0.30319.42000
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'このクラスは StronglyTypedResourceBuilder クラスが ResGen
    'または Visual Studio のようなツールを使用して自動生成されました。
    'メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    'ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    '''<summary>
    '''  ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("CraftBandMesh.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        '''  現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  編集中のデータを破棄してよろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskAbandanCurrentWork() As String
            Get
                Return ResourceManager.GetString("AskAbandanCurrentWork", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  このままリスト出力を行いますか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskOutput() As String
            Get
                Return ResourceManager.GetString("AskOutput", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ひも長加算と色をすべてクリアします。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskResetAddLengthColor() As String
            Get
                Return ResourceManager.GetString("AskResetAddLengthColor", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  目標寸法以外をリセットします。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskResetInput() As String
            Get
                Return ResourceManager.GetString("AskResetInput", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  編集中のデータを保存しますか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskSaveCurrentWork() As String
            Get
                Return ResourceManager.GetString("AskSaveCurrentWork", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  無効レコード(番号={0}) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcBadRecord() As String
            Get
                Return ResourceManager.GetString("CalcBadRecord", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  入力されている編みかたの周数を調整します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcConfirmHight() As String
            Get
                Return ResourceManager.GetString("CalcConfirmHight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  最初に見つかった編みかた(指定する場合は1周セット)で周数を調整します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcConfirmPattern() As String
            Get
                Return ResourceManager.GetString("CalcConfirmPattern", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  底の縦横を目標に基づき再計算します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcConfirmRecalc() As String
            Get
                Return ResourceManager.GetString("CalcConfirmRecalc", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  底の縦横を目標および底(楕円)の径に基づき再計算します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcConfirmRecalcDiameter() As String
            Get
                Return ResourceManager.GetString("CalcConfirmRecalcDiameter", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  底(楕円)の径({0})が縦寸法以上になっているため横ひもを置けません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcHeightOver() As String
            Get
                Return ResourceManager.GetString("CalcHeightOver", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}は [{1}] で変更してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcMsgTopBottomBand() As String
            Get
                Return ResourceManager.GetString("CalcMsgTopBottomBand", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}追加用の番号がとれません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoAddNumber() As String
            Get
                Return ResourceManager.GetString("CalcNoAddNumber", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  基本のひも幅を設定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoBaseBandSet() As String
            Get
                Return ResourceManager.GetString("CalcNoBaseBandSet", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  未定義のカテゴリー&apos;{0}&apos;が参照されました。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoDefCategory() As String
            Get
                Return ResourceManager.GetString("CalcNoDefCategory", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  処理に必要な情報がありません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoInformation() As String
            Get
                Return ResourceManager.GetString("CalcNoInformation", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}&apos;{1}&apos;は登録されていません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoMaster() As String
            Get
                Return ResourceManager.GetString("CalcNoMaster", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}の番号{1}で設定にない編みかた名&apos;{2}&apos;(ひも番号{3})が参照されています。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoMasterPattern() As String
            Get
                Return ResourceManager.GetString("CalcNoMasterPattern", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ほぼ目標のサイズになっています。やり直す場合はリセットしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoMoreChange() As String
            Get
                Return ResourceManager.GetString("CalcNoMoreChange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  編みかたが設定されていないため高さを算出できません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoPatternRecord() As String
            Get
                Return ResourceManager.GetString("CalcNoPatternRecord", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoSelect() As String
            Get
                Return ResourceManager.GetString("CalcNoSelect", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  横寸法が小さすぎるため縦ひもを置けません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoShortWidth() As String
            Get
                Return ResourceManager.GetString("CalcNoShortWidth", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  縦ひも間のすき間が最小間隔より小さくなっています。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoSpaceHeight() As String
            Get
                Return ResourceManager.GetString("CalcNoSpaceHeight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  目標とする縦寸法・横寸法・高さ寸法を設定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoTargetSet() As String
            Get
                Return ResourceManager.GetString("CalcNoTargetSet", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  横寸法が指定されていないため、すき間の寸法を計算できません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoTargetWidth() As String
            Get
                Return ResourceManager.GetString("CalcNoTargetWidth", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  目の平均 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutAverageMesh() As String
            Get
                Return ResourceManager.GetString("CalcOutAverageMesh", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  折り返し に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutTurn() As String
            Get
                Return ResourceManager.GetString("CalcOutTurn", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}を指定するのであれば{1}をセットしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcSetHorizontal() As String
            Get
                Return ResourceManager.GetString("CalcSetHorizontal", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  縦寸法が小さすぎるため横ひもを置けません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcTooShortHeight() As String
            Get
                Return ResourceManager.GetString("CalcTooShortHeight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0} の値 {1} を増やしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcTooSmallValue() As String
            Get
                Return ResourceManager.GetString("CalcTooSmallValue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  底(楕円)の径({0})が横寸法以上になっているため縦ひもを置けません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcWidthOver() As String
            Get
                Return ResourceManager.GetString("CalcWidthOver", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  編みかた&apos;{0}&apos;の設定を確認してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcZeroPatternHeight() As String
            Get
                Return ResourceManager.GetString("CalcZeroPatternHeight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  左,右 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CaptionExpand4To6() As String
            Get
                Return ResourceManager.GetString("CaptionExpand4To6", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  左,右 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CaptionExpand4To61() As String
            Get
                Return ResourceManager.GetString("CaptionExpand4To61", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  上,下 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CaptionExpand8To2() As String
            Get
                Return ResourceManager.GetString("CaptionExpand8To2", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  上,下 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CaptionExpand8To21() As String
            Get
                Return ResourceManager.GetString("CaptionExpand8To21", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  角度,放射状に配置する角度 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ExpandingWidthText() As String
            Get
                Return ResourceManager.GetString("ExpandingWidthText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  四角底と楕円底 {0} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FormCaption() As String
            Get
                Return ResourceManager.GetString("FormCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  色の修正 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LOG_ColorModified() As String
            Get
                Return ResourceManager.GetString("LOG_ColorModified", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  本幅数の修正 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LOG_LaneModified() As String
            Get
                Return ResourceManager.GetString("LOG_LaneModified", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  DLLエラー に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property TitleDllError() As String
            Get
                Return ResourceManager.GetString("TitleDllError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  例外発生 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property TitleException() As String
            Get
                Return ResourceManager.GetString("TitleException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  すき間を横寸法に合わせることができません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property WarningWidthPrime() As String
            Get
                Return ResourceManager.GetString("WarningWidthPrime", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
