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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("CraftBandKnot.Resources", GetType(Resources).Assembly)
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
        '''  規定値をロードします。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskLoadDefault() As String
            Get
                Return ResourceManager.GetString("AskLoadDefault", resourceCulture)
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
        '''  現データの状態を規定値として保存します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskSaveDefault() As String
            Get
                Return ResourceManager.GetString("AskSaveDefault", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  目標に基づき横・縦・高さのコマ数を再計算します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcConfirmRecalc() As String
            Get
                Return ResourceManager.GetString("CalcConfirmRecalc", resourceCulture)
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
        '''  横のコマ数・縦のコマ数・高さのコマ数をセットしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoPieceCountSet() As String
            Get
                Return ResourceManager.GetString("CalcNoPieceCountSet", resourceCulture)
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
        '''  目標とする縦寸法・横寸法・高さ寸法を設定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoTargetSet() As String
            Get
                Return ResourceManager.GetString("CalcNoTargetSet", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  加算計 {0} (縁の始末:{1} ひも長加算:{2} {3}加算:{4}) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutAddLen() As String
            Get
                Return ResourceManager.GetString("CalcOutAddLen", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  コマの{0}より に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutDiffFrom() As String
            Get
                Return ResourceManager.GetString("CalcOutDiffFrom", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  手前({0})と奥は同じ、真ん中で折る に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutFoldingSame() As String
            Get
                Return ResourceManager.GetString("CalcOutFoldingSame", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  折り位置から に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutFromFolding() As String
            Get
                Return ResourceManager.GetString("CalcOutFromFolding", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  コマから に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutFromKnot() As String
            Get
                Return ResourceManager.GetString("CalcOutFromKnot", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  手前({0})が奥より に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutFront() As String
            Get
                Return ResourceManager.GetString("CalcOutFront", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &lt;コマの{0}&gt; に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutKnotOf() As String
            Get
                Return ResourceManager.GetString("CalcOutKnotOf", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  コマの{0}と同じ長さ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutKnotSame() As String
            Get
                Return ResourceManager.GetString("CalcOutKnotSame", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  長い に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutLong() As String
            Get
                Return ResourceManager.GetString("CalcOutLong", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  短い に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutShort() As String
            Get
                Return ResourceManager.GetString("CalcOutShort", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  左 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutSideLeft() As String
            Get
                Return ResourceManager.GetString("CalcOutSideLeft", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  下 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutSideLower() As String
            Get
                Return ResourceManager.GetString("CalcOutSideLower", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  右 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutSideRight() As String
            Get
                Return ResourceManager.GetString("CalcOutSideRight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  上 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutSideUpper() As String
            Get
                Return ResourceManager.GetString("CalcOutSideUpper", resourceCulture)
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
        '''  上,下 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CaptionExpand8To2() As String
            Get
                Return ResourceManager.GetString("CaptionExpand8To2", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  規定値.xml に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property DefaultFileName() As String
            Get
                Return ResourceManager.GetString("DefaultFileName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  四つ畳み編み {0} に類似しているローカライズされた文字列を検索します。
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
        '''  規定値が保存されていません。先に規定値保存を行ってください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageNoDefaultFile() As String
            Get
                Return ResourceManager.GetString("MessageNoDefaultFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  規定値保存ファイル&apos;{0}&apos;がありません。再度規定値保存を行ってください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageNotExistDefaultFile() As String
            Get
                Return ResourceManager.GetString("MessageNotExistDefaultFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  側面の高さが異なるため直方体になりません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ModelDiffHeight() As String
            Get
                Return ResourceManager.GetString("ModelDiffHeight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}が描画できませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ModelNoImage() As String
            Get
                Return ResourceManager.GetString("ModelNoImage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  底 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property OutputTextBottom() As String
            Get
                Return ResourceManager.GetString("OutputTextBottom", resourceCulture)
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
    End Module
End Namespace
