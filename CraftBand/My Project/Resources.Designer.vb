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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("CraftBand.Resources", GetType(Resources).Assembly)
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
        '''  色&lt;{0}&gt;はクリアされます。よろしいですか？
        '''(残す場合は&apos;対象&apos;のチェックを外してください) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskColorClear() As String
            Get
                Return ResourceManager.GetString("AskColorClear", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  新たに&apos;{0}&apos;を追加します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskConfirmAdd() As String
            Get
                Return ResourceManager.GetString("AskConfirmAdd", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  既存の&apos;{0}&apos;に{1}を追加します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskConfirmAppend() As String
            Get
                Return ResourceManager.GetString("AskConfirmAppend", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  『設定時の寸法単位』を変更しても、既に設定済みの値は変わりません。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskConfirmChangeUnit() As String
            Get
                Return ResourceManager.GetString("AskConfirmChangeUnit", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &apos;{0}&apos;をすべて削除します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskConfirmDeleteGroup() As String
            Get
                Return ResourceManager.GetString("AskConfirmDeleteGroup", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &apos;{0}&apos;(ひも番号{1})を削除します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskConfirmDeleteSub() As String
            Get
                Return ResourceManager.GetString("AskConfirmDeleteSub", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  バンドの種類にない色({0})は使えません。変更しますか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskNoBandTypeColor() As String
            Get
                Return ResourceManager.GetString("AskNoBandTypeColor", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  同名データに対しては、現設定値で上書きしてよろしいですか？
        '''(はい=上書き,いいえ=既存値保持) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskOverwriteForExist() As String
            Get
                Return ResourceManager.GetString("AskOverwriteForExist", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  同名データに対しては、読み取り値で上書きしてよろしいですか？
        '''(はい=上書き,いいえ=現在値保持) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskOverwriteForSame() As String
            Get
                Return ResourceManager.GetString("AskOverwriteForSame", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}が変更されました。選択されたファイル&apos;{1}&apos;を読み直しますか？
        '''(はい=読み直す,いいえ=現設定を上書きする) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskReloadMasterDataFile() As String
            Get
                Return ResourceManager.GetString("AskReloadMasterDataFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  実測値を全てクリアし規定値に戻します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskResetAll() As String
            Get
                Return ResourceManager.GetString("AskResetAll", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}をすべて初期状態に戻します。よろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskResetExpanding() As String
            Get
                Return ResourceManager.GetString("AskResetExpanding", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  新しい{0}として&apos;{1}&apos;が指定されました。現在の設定をクリアしますか？
        '''(はい=ゼロから作り直す,いいえ=現設定維持) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskResetMasterDataFile() As String
            Get
                Return ResourceManager.GetString("AskResetMasterDataFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ひも上下の編集内容をクリアします。サイズも初期化してよろしいですか？
        '''(はいで全て初期化、いいえはサイズ保持) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskResetUpDown() As String
            Get
                Return ResourceManager.GetString("AskResetUpDown", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  先の&apos;{0}&apos;の変更を保存します。よろしいですか？(保存しない場合は[いいえ]) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskSaveChange() As String
            Get
                Return ResourceManager.GetString("AskSaveChange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  現在の選択では{0}できません。全範囲を{0}してよろしいですか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskTargetAllRange() As String
            Get
                Return ResourceManager.GetString("AskTargetAllRange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  選択された領域を循環{0}しますか？　(はい=循環,いいえ=一方向) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AskTargetRectangle() As String
            Get
                Return ResourceManager.GetString("AskTargetRectangle", resourceCulture)
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
        '''  {0}を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcNoSelect() As String
            Get
                Return ResourceManager.GetString("CalcNoSelect", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0} 本 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutCount() As String
            Get
                Return ResourceManager.GetString("CalcOutCount", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  カットリスト に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutCutList() As String
            Get
                Return ResourceManager.GetString("CalcOutCutList", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}幅 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutLane() As String
            Get
                Return ResourceManager.GetString("CalcOutLane", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  単純計 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutLongest() As String
            Get
                Return ResourceManager.GetString("CalcOutLongest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  最長 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutLonguest() As String
            Get
                Return ResourceManager.GetString("CalcOutLonguest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  面積長 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutShortest() As String
            Get
                Return ResourceManager.GetString("CalcOutShortest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  集計値 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcOutSum() As String
            Get
                Return ResourceManager.GetString("CalcOutSum", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ひも上下レコードの読み取りエラーです。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CalcUpDownLoadErr() As String
            Get
                Return ResourceManager.GetString("CalcUpDownLoadErr", resourceCulture)
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
        '''  長さと重さの換算 &lt;{0}&gt; に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CaptionBandTypeWeight() As String
            Get
                Return ResourceManager.GetString("CaptionBandTypeWeight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  レコードを追加できませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrAddRecord() As String
            Get
                Return ResourceManager.GetString("ErrAddRecord", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  指定された&apos;{0}&apos;は{1}用のファイルではありません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrBadFormat() As String
            Get
                Return ResourceManager.GetString("ErrBadFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}の値をセットしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrBadValue() As String
            Get
                Return ResourceManager.GetString("ErrBadValue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}{1}を確認してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrCheckNoCross() As String
            Get
                Return ResourceManager.GetString("ErrCheckNoCross", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  サイズ({0},{1})のエラーです。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrCheckValid() As String
            Get
                Return ResourceManager.GetString("ErrCheckValid", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  実測値の不足もしくは値のエラーのため計算できなかった係数があります。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrGridCoeff() As String
            Get
                Return ResourceManager.GetString("ErrGridCoeff", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}行目&lt;{1}&gt; データエラー{2}{3} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrGridData() As String
            Get
                Return ResourceManager.GetString("ErrGridData", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}行目&lt;{1}&gt; {2} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrGridLine() As String
            Get
                Return ResourceManager.GetString("ErrGridLine", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  グリッドの警告 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrGridTitle() As String
            Get
                Return ResourceManager.GetString("ErrGridTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &apos;{0}&apos;ページを作成できませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrHtmlFile() As String
            Get
                Return ResourceManager.GetString("ErrHtmlFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &apos;{0}&apos;ページを開くことができません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrHtmlProcessStart() As String
            Get
                Return ResourceManager.GetString("ErrHtmlProcessStart", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  gifファイル&apos;{0}&apos;生成エラー に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrImageGifCreate() As String
            Get
                Return ResourceManager.GetString("ErrImageGifCreate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  バンドの種類が正しく設定されていません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrKnotBandType() As String
            Get
                Return ResourceManager.GetString("ErrKnotBandType", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}本幅の要尺寸法値が不正です。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrKnotPieceLength() As String
            Get
                Return ResourceManager.GetString("ErrKnotPieceLength", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}本幅のコマ寸法値が不正です。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrKnotPieceSize() As String
            Get
                Return ResourceManager.GetString("ErrKnotPieceSize", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  『長さと重さ』値を正しくセットしてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrLengthWeight() As String
            Get
                Return ResourceManager.GetString("ErrLengthWeight", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &lt;数値&gt; &lt;長さの単位&gt; = &lt;数値&gt; &lt;重さの単位&gt; のように入力 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrLengthWeightValue() As String
            Get
                Return ResourceManager.GetString("ErrLengthWeightValue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  設定データを保存するファイルが指定されませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrMasterTableFileCancel() As String
            Get
                Return ResourceManager.GetString("ErrMasterTableFileCancel", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  &apos;{0}&apos;とは別の設定データファイルを指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrMsgSameMasterTableFile() As String
            Get
                Return ResourceManager.GetString("ErrMsgSameMasterTableFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}以下の数値を設定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrMsgTooLarge() As String
            Get
                Return ResourceManager.GetString("ErrMsgTooLarge", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}以上の数値を設定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrMsgTooSmall() As String
            Get
                Return ResourceManager.GetString("ErrMsgTooSmall", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrNewName() As String
            Get
                Return ResourceManager.GetString("ErrNewName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  変更したい色の&apos;対象&apos;にチェックを入れてください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrNoChangeCheck() As String
            Get
                Return ResourceManager.GetString("ErrNoChangeCheck", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  画像ファイルが作られていません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrNoGifFile() As String
            Get
                Return ResourceManager.GetString("ErrNoGifFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  起動に必要なパラメータがありません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrNoInitializeParameters() As String
            Get
                Return ResourceManager.GetString("ErrNoInitializeParameters", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  新しい/既存の{0}を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrNoName() As String
            Get
                Return ResourceManager.GetString("ErrNoName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  指定されたファイル&apos;{0}&apos;は{1}用です。{2}では使えません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrOnotherFormat() As String
            Get
                Return ResourceManager.GetString("ErrOnotherFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  指定ファイル&apos;{0}&apos;から設定データを読み取ることができませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrReadMasterTableFile() As String
            Get
                Return ResourceManager.GetString("ErrReadMasterTableFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  既存の{0}&lt;{1}&gt;と同じにはできません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrSameName() As String
            Get
                Return ResourceManager.GetString("ErrSameName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  設定データのファイル&apos;{0}への保存に失敗しました。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrSaveMasterTableFile() As String
            Get
                Return ResourceManager.GetString("ErrSaveMasterTableFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ＭＳ ゴシック に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FontNameMark() As String
            Get
                Return ResourceManager.GetString("FontNameMark", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ＭＳ Ｐ明朝 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FontNameString() As String
            Get
                Return ResourceManager.GetString("FontNameString", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  長さを入力して[→] / 重さを入力して[←] に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property GuideLengthWeigt() As String
            Get
                Return ResourceManager.GetString("GuideLengthWeigt", resourceCulture)
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
        '''  未定義のDataType に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LOG_DataType() As String
            Get
                Return ResourceManager.GetString("LOG_DataType", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  例外発生 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LOG_Exception() As String
            Get
                Return ResourceManager.GetString("LOG_Exception", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  未定義のフィールド名 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LOG_FieldName() As String
            Get
                Return ResourceManager.GetString("LOG_FieldName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  後処理完了 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LOG_Finalized() As String
            Get
                Return ResourceManager.GetString("LOG_Finalized", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ダイアログ表示終了 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LOG_FormEnd() As String
            Get
                Return ResourceManager.GetString("LOG_FormEnd", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  設定ファイルのエクスポート (上書き) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogExportMasterFile() As String
            Get
                Return ResourceManager.GetString("LogExportMasterFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  + 追加 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportAdd() As String
            Get
                Return ResourceManager.GetString("LogImportAdd", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  + 色の追加 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportAddColor() As String
            Get
                Return ResourceManager.GetString("LogImportAddColor", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  - 変更後 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportAfter() As String
            Get
                Return ResourceManager.GetString("LogImportAfter", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  + 変更前 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportBefore() As String
            Get
                Return ResourceManager.GetString("LogImportBefore", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  - 同名あり 既存を保持 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportExistSkip() As String
            Get
                Return ResourceManager.GetString("LogImportExistSkip", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  設定ファイルのインポート (上書き) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportMasterFile() As String
            Get
                Return ResourceManager.GetString("LogImportMasterFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  - 対象外 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportNoTarget() As String
            Get
                Return ResourceManager.GetString("LogImportNoTarget", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  - 同名あり 既存と一致 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportSameSkip() As String
            Get
                Return ResourceManager.GetString("LogImportSameSkip", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  テーブルの現点数とファイル点数 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LogImportTable() As String
            Get
                Return ResourceManager.GetString("LogImportTable", resourceCulture)
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
        '''  {0}点を変更しました。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageColorChanged() As String
            Get
                Return ResourceManager.GetString("MessageColorChanged", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  変更はありませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageColorNoChange() As String
            Get
                Return ResourceManager.GetString("MessageColorNoChange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  少なくとも1点の変更を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageColorSet() As String
            Get
                Return ResourceManager.GetString("MessageColorSet", resourceCulture)
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
        '''  水平・垂直の本数を指定してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageNoUpDownSize() As String
            Get
                Return ResourceManager.GetString("MessageNoUpDownSize", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}できませんでした。クリア(初期化)してやり直してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageUpDownError() As String
            Get
                Return ResourceManager.GetString("MessageUpDownError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}できませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MessageUpDownNop() As String
            Get
                Return ResourceManager.GetString("MessageUpDownNop", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ファイル&apos;{0}&apos;に {1}点の書き出しを行いました。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgExported() As String
            Get
                Return ResourceManager.GetString("MsgExported", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  [{0}] の {1}点を更新しました。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpdate() As String
            Get
                Return ResourceManager.GetString("MsgUpdate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  全更新数は {0}点でした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpdateAll() As String
            Get
                Return ResourceManager.GetString("MsgUpdateAll", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  [{0}] の更新エラーです。設定メニューで確認してください。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpdateError() As String
            Get
                Return ResourceManager.GetString("MsgUpdateError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  [{0}] の更新はありません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpdateNone() As String
            Get
                Return ResourceManager.GetString("MsgUpdateNone", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ON:{0}点 OFF:{1}点(全{2}点)　ON比率 {3:f2} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpDounCount() As String
            Get
                Return ResourceManager.GetString("MsgUpDounCount", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  最小連続数:{0}  最大連続数:{1} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpDounMinMax() As String
            Get
                Return ResourceManager.GetString("MsgUpDounMinMax", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  行 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpDownHorizontal() As String
            Get
                Return ResourceManager.GetString("MsgUpDownHorizontal", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}の最大連続数:{1} ({2}) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpDownMax() As String
            Get
                Return ResourceManager.GetString("MsgUpDownMax", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  {0}の最小連続数:{1} ({2}) に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpDownMin() As String
            Get
                Return ResourceManager.GetString("MsgUpDownMin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  列 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MsgUpDownVertical() As String
            Get
                Return ResourceManager.GetString("MsgUpDownVertical", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  裏面 : {0} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property TitleBackFace() As String
            Get
                Return ResourceManager.GetString("TitleBackFace", resourceCulture)
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
        
        '''<summary>
        '''  指定されたファイル&apos;{0}&apos;は読み取れませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property WarningBadWorkData() As String
            Get
                Return ResourceManager.GetString("WarningBadWorkData", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  指定されたファイル&apos;{0}&apos;への保存ができませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property WarningFileSaveError() As String
            Get
                Return ResourceManager.GetString("WarningFileSaveError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ファイル&apos;{0}&apos;を起動できませんでした。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property WarningFileStartError() As String
            Get
                Return ResourceManager.GetString("WarningFileStartError", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
