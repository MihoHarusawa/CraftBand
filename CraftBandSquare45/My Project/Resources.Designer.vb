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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("CraftBandSquare45.Resources", GetType(Resources).Assembly)
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
        '''  {0}に基づき再度初期化してよろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskInitializeAgain() As String
            Get
                Return ResourceManager.GetString("AskInitializeAgain", resourceCulture)
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
        '''  {0}の指定が正しくありません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcBadSetValue() As String
            Get
                Return ResourceManager.GetString("CalcBadSetValue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  目標に基づき横・縦・高さの四角数を再計算します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
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
        '''  &apos;{0}&apos;{1}&apos;は登録されていません。 に類似しているローカライズされた文字列を検索します。
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
        '''  底が長方形になっていません。横{0:f1} ({1:f1})  縦{2:f1} ({3:f1}) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoRectangle() As String
            Get
                Return ResourceManager.GetString("CalcNoRectangle", resourceCulture)
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
        '''  横の四角数・縦の四角数・高さの四角数をセットしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoSquareCountSet() As String
            Get
                Return ResourceManager.GetString("CalcNoSquareCountSet", resourceCulture)
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
        '''  斜め四角45度 {0} に類似しているローカライズされた文字列を検索します。
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
        '''  現在の値では合わせることはできません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageCannotSuit() As String
            Get
                Return ResourceManager.GetString("MessageCannotSuit", resourceCulture)
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
        '''  {0}が長方形でないため描画できません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ModelNoRectangle() As String
            Get
                Return ResourceManager.GetString("ModelNoRectangle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  現在の値で編集することはできません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgCannotEdit() As String
            Get
                Return ResourceManager.GetString("MsgCannotEdit", resourceCulture)
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
