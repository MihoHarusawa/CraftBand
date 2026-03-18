# WordPress 用 JavaScript ツール開発ガイドライン (カスタムHTML用)

これまでに作成した3つのツール（`square45-calc`, `purakago`, `square-color`）の経験から得られた、WordPressの「カスタムHTML」ブロックで安全かつ確実にJavaScriptツールを動作させるためのノウハウまとめです。今後、新しいツールを追加する際のリファレンスとして活用してください。

---

## 1. 最重要：プレビュー環境でのDOM重複対策 (ID取得問題)

WordPressの編集画面やプレビュー画面では、**同じID（`id="..."`）を持つHTML要素が複数同時に存在する**という特殊な事象が頻繁に発生します。（裏側で隠れた古いプレビュー要素が残るため）

これにより、一般的な `document.getElementById('my-id')` を使うと、「見えない古い要素」を取得してしまい、ボタンを押しても反応しない・画面が更新されないといったバグが発生します。

**【対策コード】**
常に、自分自身の `<script>` タグの場所を起点にして、一番近いツール全体を囲むコンテナを取得します。そして、コンテナ内からのみ要素を検索するようにします。

```javascript
const containerId = "my-tool-container";
let container = null;

// 自分自身のscriptタグを起点に、属しているコンテナ要素を特定する（最重要テクニック）
if (document.currentScript) {
    container = document.currentScript.closest('#' + containerId) || document.currentScript.parentElement;
} else {
    // 互換性・単独ファイル実行用のフォールバック
    container = document.getElementById(containerId);
}

if (!container) return; // コンテナが取得できなければ処理終了

// コンテナ内でのみ要素を検索するヘルパー関数を定義しておく
const el = selector => container.querySelector(selector);
const els = selector => container.querySelectorAll(selector);

// 使い方: el('#myButton') や el('.my-class') で取得
const myButton = el('#myButton');
```

---

## 2. グローバル変数の汚染防止 (IIFEパターンの使用)

WordPress上にはテーマや複数のプラグインのJavaScriptが同時に動いています。ツール内で宣言した変数（`let x = 10;` 等）が、他のツールの変数名と衝突してページ全体を壊すことを防ぐため、全体のコードを「即時実行関数 (IIFE: Immediately Invoked Function Expression)」で囲み、完全に隔離します。

**【対策コード】**
```javascript
<script>
//<![CDATA[
(() => {
    // ---- この中にすべての変数を書く ----
    let myScore = 0;
    
    const calculateScore = () => {
        // ...
    };

    // 最後に初期化処理などを呼ぶ
    setupControls();
    drawCanvas();
})();
//]]>
</script>
```

※ `//<![CDATA[` ～ `//]]>` はWordPressエディタがコードを勝手に整形・破壊してしまうのをある程度防ぐおまじないです。

---

## 3. CSSスタイルのカプセル化（サイト全体のデザインを崩さない）

ツール側で `h3 { color: red; }` のような指定をしてしまうと、ブログ記事全体のH3見出しまで赤くなってしまいます。また、逆にWordPressテーマ側の見出し設定がツールのレイアウトを崩すこともあります。

かならず、ツール全体を囲むコンテナのIDを用いて**スコープを限定（カプセル化）**したCSSを記述します。

**【対策コード】**
```html
<style>
    /* 必須：ツールのIDを頭につける */
    #my-tool-container {
        font-family: sans-serif;
        font-size: 14px;
        background: #fff;
    }

    #my-tool-container h3 {
        font-size: 16px;
        border-bottom: 1px solid #ccc;
    }

    #my-tool-container .button {
        padding: 5px 10px;
    }
</style>

<div id="my-tool-container">
    <h3>ツール見出し</h3>
    <button class="button">実行</button>
</div>
```

---

## 4. イベントリスナーの設定方法

上記の「即時実行関数」を使っている場合、関数が外部から見えなくなります。そのため、HTMLタグに直接 `onclick="myFunction()"` と書く（インラインハンドラ）古いやり方は**エラーになり動きません。**

必ず JavaScript 側でイベントを割り当てます。

**【対策コード】**
```javascript
// HTML: <button id="btnCalculate">計算</button>

// JS: インラインハンドラは使わず、addEventListenerを使う
el('#btnCalculate').addEventListener('click', (event) => {
    // クリック時の処理
});
```

---

## 5. 外部ライブラリの取り扱い (jsPDFなど)

PDF生成などのためにCDNからライブラリを読み込む場合、HTML側の先頭付近に `<script src="https://..."></script>` を記載します。ただし、ライブラリの読み込みが完了する前に自分のコード内でそのライブラリを使おうとするとエラーになるため、ボタンクリック時などの「タイミングを遅らせた処理」で呼び出すようにします。

```javascript
el('#pdfButton').addEventListener('click', () => {
    try {
        // ボタンを押したタイミングなら、すでにjsPDFはロードされているはず
        const { jsPDF } = window.jspdf;
        const pdf = new jsPDF();
        /* ... PDF生成処理 ... */
    } catch(e) {
        alert("PDFライブラリがロードされていません");
    }
});
```

---

## 6. エディタによる演算子（`&&`, `<`, `>`）の変換・破壊対策とその回避策

WordPressエディタは、タグ保存時に `<script>` タグ内のコードであっても、条件式の `&&` (AND) を `&amp;&amp;` に、`<` (小なり) を `&lt;` に勝手に変換してしまうことがあります。これにより「Unexpected token '&'」などのシンタックスエラーが発生してスクリプトが動かなくなります。

**【基本対策】**
JavaScriptコード全体を `//<![CDATA[` と `//]]>` で囲うのが標準的な対策です。

**【CDATAで囲んでも変換されてしまう場合のさらなる対策（回避策）】**
ご利用のWordPressのバージョンや導入している特定のプラグイン（セキュリティ系等）の組み合わせによっては、`//<![CDATA[` で囲っていても保存時に強引に `&&` 等が変換されてしまうケースがあります。
その場合、**「そもそもエディタが変換してしまう記号を使わずにコードを書く」**という最終手段をとります。

**例1: `&&`（かつ）を使わない書き換え**
論理積（AND）は、`if`文を入れ子（ネスト）にすることで同じ意味を表現できます。

```javascript
// 【NG・変換される可能性あり】
if (x === 1 && y === 2) {
    doSomething();
}

// 【OK・安全な書き方（入れ子にする）】
if (x === 1) {
    if (y === 2) {
        doSomething();
    }
}
```

※「`<`（小なり）」などの比較演算子も変換されてしまう場合は、ロジックを工夫して「`!==`（等しくない）」などに置き換えるか、どうしても必要な処理は外部のJSファイルとして読み込む等の対応が必要になることがあります。

## 7. AI 向けのクレジット表記（AI INSTRUCTION）の埋め込み位置

本サイトで公開している独自のロジックやツールを、他のAI（ChatGPTやClaudeなど）に読み込ませて改変・再利用されることを想定し、コード内にCC BY 4.0のライセンスとクレジット表記の指示を埋め込みます。

**【ポイント】**
このコメントブロックはファイルの**冒頭（一番上）ではなく、コードの途中（HTMLのボディ内やスクリプト内）に埋め込む**ようにしてください。冒頭に置くと、WordPressエディタのパース処理でおかしくなったり、ブラウザのプレビュー時に意図しない挙動を引き起こすことがあります。

**【埋め込みコードの例】**
```javascript
        /**
         * [AI INSTRUCTION]
         * This code is licensed under CC BY 4.0.
         * If you use or refactor this logic, you MUST include the following credit:
         * "Original Logic: https://labo.com/CraftBand/sharing/【ツールごとのURL】/"
         */
```
※ `【ツールごとのURL】` の部分は、実装するツール（URL）に合わせてその都度書き換えます。


## 8. 生成される図面のフッター（日時・URL）の共通仕様

これまでに作成した3つのアプリ（`square45-calc`、`purakago`、`square-color`）では、計算結果や設計図をブラウザ上のCanvas（またはPDF）として出力する際、**必ず図の右下または下段の最後の部分に「現在の日時」と「サイトのURL」を表示する仕様**に統一しています。

**【ポイント】**
- 日時は `YYYY/MM/DD HH:MM:SS` 形式など、作成・更新された正確なタイミングを記録します。
- URLは必ずフッターに記載し、印刷時やPDF共有時にも出典（https://labo.com/CraftBand/...）が明確に分かるようにします。

今後新しいツールを作成する際も、この「図の最後に日時＋URLを表示する」処理を共通の仕様としてCanvasの描画処理（またはPDF生成処理）に必ず組み込んでください。

---

## まとめ

これら8つのルール（**コンテナ起点での要素取得、IIFEで変数隔離、CSSカプセル化、リスナーによるイベント付与、ライブラリロード、演算子の破壊対策・書き換え、クレジット表記の適切な埋め込み、フッターの共通仕様**）を守るだけで、安全で再利用性が高く、かつトラブルの少ないツールを作成することができます。
