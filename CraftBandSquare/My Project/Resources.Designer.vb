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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("CraftBandSquare.Resources", GetType(Resources).Assembly)
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
        '''  縁を削除してよろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskDeleteEdge() As String
            Get
                Return ResourceManager.GetString("AskDeleteEdge", resourceCulture)
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
        '''  ひも上下の編集内容をすべてクリアします。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskResetUpDown() As String
            Get
                Return ResourceManager.GetString("AskResetUpDown", resourceCulture)
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
        '''  無効レコード(番号={0}) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcBadRecord() As String
            Get
                Return ResourceManager.GetString("CalcBadRecord", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  目標に基づき横・縦・高さのひも(目)数を再計算します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
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
        '''  {0}の番号{1}で設定にない付属品名&apos;{2}&apos;(ひも番号{3})が参照されています。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoMasterOption() As String
            Get
                Return ResourceManager.GetString("CalcNoMasterOption", resourceCulture)
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
        '''  {0}を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoSelect() As String
            Get
                Return ResourceManager.GetString("CalcNoSelect", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  横ひも・縦ひも・編みひもの本数(目の数)をセットしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoSquareCountSet() As String
            Get
                Return ResourceManager.GetString("CalcNoSquareCountSet", resourceCulture)
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
        '''  ひも上下レコードの保存エラーです。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcUpDownSaveErr() As String
            Get
                Return ResourceManager.GetString("CalcUpDownSaveErr", resourceCulture)
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
        '''  0度(水平・横),45度(右斜め),90度(垂直・縦),135度(左斜め) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property EnumStringAngle() As String
            Get
                Return ResourceManager.GetString("EnumStringAngle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  目の中央,ひも中央 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property EnumStringCenter() As String
            Get
                Return ResourceManager.GetString("EnumStringCenter", resourceCulture)
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
        '''  四つ目と四角 {0} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FormCaption() As String
            Get
                Return ResourceManager.GetString("FormCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  左下 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LeftLower() As String
            Get
                Return ResourceManager.GetString("LeftLower", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  左上 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LeftUpper() As String
            Get
                Return ResourceManager.GetString("LeftUpper", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  チェックOKです。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageCheckOK() As String
            Get
                Return ResourceManager.GetString("MessageCheckOK", resourceCulture)
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
        '''  これ以上増やせません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageNoMore() As String
            Get
                Return ResourceManager.GetString("MessageNoMore", resourceCulture)
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
        '''  水平・垂直の本数を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageNoUpDownSize() As String
            Get
                Return ResourceManager.GetString("MessageNoUpDownSize", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}できませんでした。リセットしてやり直してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageUpDownError() As String
            Get
                Return ResourceManager.GetString("MessageUpDownError", resourceCulture)
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
        '''  上下図の登録 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property UpDownAppend() As String
            Get
                Return ResourceManager.GetString("UpDownAppend", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  新たな上下図名を入力してください。[OK]ボタンで現パターンを登録します。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property UpDownAppendInstruction() As String
            Get
                Return ResourceManager.GetString("UpDownAppendInstruction", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &apos;{0}&apos;は既に登録されています。置き換えますか？(いいえで別の名前を指定) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property UpDownExistName() As String
            Get
                Return ResourceManager.GetString("UpDownExistName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &apos;{0}&apos;は登録されていません。登録名を選択してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property UpDownNoExistName() As String
            Get
                Return ResourceManager.GetString("UpDownNoExistName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  上下図名がセットされていません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property UpDownNoName() As String
            Get
                Return ResourceManager.GetString("UpDownNoName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  上下図の呼出 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property UpDownRead() As String
            Get
                Return ResourceManager.GetString("UpDownRead", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  上下図名を選択してください。[OK]ボタンで現パターンに反映します。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property UpDownReadInstruction() As String
            Get
                Return ResourceManager.GetString("UpDownReadInstruction", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
