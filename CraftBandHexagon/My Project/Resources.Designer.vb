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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("CraftBandHexagon.Resources", GetType(Resources).Assembly)
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
        '''  現編集内容を破棄し{0}と同じにします。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskLoadSameAs() As String
            Get
                Return ResourceManager.GetString("AskLoadSameAs", resourceCulture)
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
        '''  目標寸法以外をリセットします。六つ目の高さもリセットしてよろしいですか？
        '''(はいで全てリセット、いいえで六つ目の高さを保持) に類似しているローカライズされた文字列を検索します。
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
        '''  立ち上げ可能な底を作れません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcBadBottom() As String
            Get
                Return ResourceManager.GetString("CalcBadBottom", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  合わせ目の位置は、ひもの本数より小さくしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcBadMarkPosition() As String
            Get
                Return ResourceManager.GetString("CalcBadMarkPosition", resourceCulture)
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
        '''  六つ目を作るために、各ひも2本以上にしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcBandCountAtLeast() As String
            Get
                Return ResourceManager.GetString("CalcBandCountAtLeast", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  上端・下端/斜め左端・右端、最下段の値は、足して目になるようにしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcBottomSpaceValue() As String
            Get
                Return ResourceManager.GetString("CalcBottomSpaceValue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  目標と六つ目サイズに基づき、各ひも数を再計算します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcConfirmRecalc() As String
            Get
                Return ResourceManager.GetString("CalcConfirmRecalc", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  長さを計算できないひもが{0}本あります。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcErrorBandLength() As String
            Get
                Return ResourceManager.GetString("CalcErrorBandLength", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  基本的な情報を取得できません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcErrorBasic() As String
            Get
                Return ResourceManager.GetString("CalcErrorBasic", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ひもの長さを結果に反映することができません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcErrorReflection() As String
            Get
                Return ResourceManager.GetString("CalcErrorReflection", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  合わせ目の位置は1以上にしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcMarkPositionPlus() As String
            Get
                Return ResourceManager.GetString("CalcMarkPositionPlus", resourceCulture)
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
        '''  ひもの本数をセットしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoCountSet() As String
            Get
                Return ResourceManager.GetString("CalcNoCountSet", resourceCulture)
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
        '''  織りタイプは指定されていません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoPattern() As String
            Get
                Return ResourceManager.GetString("CalcNoPattern", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  「ひも中心合わせ」をオフにしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoPatternBandCenter() As String
            Get
                Return ResourceManager.GetString("CalcNoPatternBandCenter", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  右綾/左綾を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoPatternLeftRight() As String
            Get
                Return ResourceManager.GetString("CalcNoPatternLeftRight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  「六つ目の高さ」の値を {0} 以上に設定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoPatternSpace() As String
            Get
                Return ResourceManager.GetString("CalcNoPatternSpace", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  「三角の中」の値がゼロ以上になるよう設定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoPatternTriangle() As String
            Get
                Return ResourceManager.GetString("CalcNoPatternTriangle", resourceCulture)
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
        '''  目標寸法もしくは基本のひも幅が正しくありません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoTargetSet() As String
            Get
                Return ResourceManager.GetString("CalcNoTargetSet", resourceCulture)
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
        '''  ひも長係数が小さすぎます。通常1以上の値です。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcSmallLengthRatio() As String
            Get
                Return ResourceManager.GetString("CalcSmallLengthRatio", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  　Y字分岐 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcYBranch() As String
            Get
                Return ResourceManager.GetString("CalcYBranch", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  左下,右上 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CaptionExpand1to9() As String
            Get
                Return ResourceManager.GetString("CaptionExpand1to9", resourceCulture)
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
        '''  左上,右下 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CaptionExpand7to3() As String
            Get
                Return ResourceManager.GetString("CaptionExpand7to3", resourceCulture)
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
        '''  0度(水平・横),30度,60度(右斜め),90度,120度(左斜め),150度 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property EnumStringAngle() As String
            Get
                Return ResourceManager.GetString("EnumStringAngle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  なし,底面,側面,全面 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property EnumStringPlate() As String
            Get
                Return ResourceManager.GetString("EnumStringPlate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  おもて,うら に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property EnumStringPosition() As String
            Get
                Return ResourceManager.GetString("EnumStringPosition", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  六つ目 {0} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FormCaption() As String
            Get
                Return ResourceManager.GetString("FormCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}のデータを表示できませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageReplaceError() As String
            Get
                Return ResourceManager.GetString("MessageReplaceError", resourceCulture)
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
