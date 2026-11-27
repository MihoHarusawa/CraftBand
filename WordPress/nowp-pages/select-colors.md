# select-colors 仕様書 (Ver.1)

本ドキュメントは、WordPress外の静的ページとして配置する `select-colors`（3D色合わせ／二色ペア選択パーツ）の仕様、画面機能、および外部連携APIインターフェースをまとめたものです。  
色データの登録・管理側である [`color-viewer.md`](./color-viewer.md) および共通API規約 [`api_specification.md`](../api_specification.md) もあわせて参照してください。

---

## 1. 概要・配置構成

`color-viewer.html` でブラウザ（`localStorage`）に保持した色ラインナップを RGB 3D空間（0〜255の立方体）上に球体として配置し、斜め格子プレビューを見ながら **A色・B色の2色をペアで選ぶ** ためのツール／共通パーツです。

- **公開URL**: `https://labo.com/CraftBand/nowp-pages/select-colors.html`
- **データ依存**: 同オリジンの `color-viewer.html` が `localStorage` に保存した色一覧・バンド種類名を参照（単独でのXML読込機能は持たない）
- **外部ライブラリ**: Three.js `0.160.0`（ES Modules / `OrbitControls` を CDN 経由で読込）

---

## 2. 動作モード（呼出し元による自動判定）

URLクエリに **`cb_req_id` または `cb_slot` が含まれるか** で、動作モードを自動判別します。

| 項目 | 色選択モード（別画面からの呼出し） | 単体表示モード（スタンドアロン） |
| :--- | :--- | :--- |
| **判定条件** | `cb_req_id` または `cb_slot` が指定されている | 上記パラメータがいずれも未指定 |
| **冒頭アクションバー** | `キャンセル` / `二色を確定して戻る` の2ボタンを表示 | 非表示 |
| **確定時動作** | 現在の A色・B色（未選択分は初期値）を元画面へ返却して閉じる | - |

---

## 3. 画面・機能仕様

### 3.1 タイトル・空データ案内・自動同期
- **タイトル**: `localStorage` のバンド種類名から `<h1>` および `<title>` を **`${バンドの種類名}の色合わせ`** として動的表示（未設定時は `色合わせ`）。
- **空データ時案内**: 保存色数が 0 件の場合、`color-viewer.html` への案内リンクを表示。
- **リアルタイム同期**: 別タブの `color-viewer.html` で色一覧やバンド名が更新された場合、`storage` イベントを検知して3D空間とタイトルを自動再構築。

### 3.2 初期選択色・選択枠・確定時の扱い
- **初期値**: **選択色A = 白（`#ffffff`）**、**選択色B = 黒（`#000000`）**。
- 選択枠 A / B をタップしてターゲットを切り替え、3D空間の球体をタップして色を反映。
- 低輝度色（暗い色）でも視認性を保つため、輝度 `< 75` の場合は枠内テキストを明色（`#f3f4f6`）に自動補正。
- 選ばずに「二色を確定して戻る」を押した場合は白と黒を返却。1色だけ選んだ場合は、選んでいない方を初期値のまま返却。

### 3.3 斜め格子プレビューと粗さスライダー
- 選択色 A・B を 45° 回転の斜め格子パターン（SVG）で即時プレビュー。
- プレビュー直上に5段階のスライダー（「格子の粗さ」、初期値 `3`）を配置し、格子1周期サイズを `24 / 40 / 60 / 90 / 130 px` の5段階で切替可能。

### 3.4 3D空間（RGB立方体）・ライティング・自動回転
- **配置と操作**: 各色を `(r, g, b)` 座標に半径 `14` の球体として配置。ドラッグで回転、ピンチ／ホイールで拡縮、タップ（`300ms` 未満かつ移動距離 `10px` 未満）で Raycaster により最前面の球体を選択。
- **立体ライティング**: 上下の環境光（`HemisphereLight`）、右上前方からの主光源（`DirectionalLight`）、反対側からの弱いリムライト、`MeshPhongMaterial`（`specular` / `shininess`）で球体の立体感を表現。
- **自動回転**:
  - 起動直後から **1分で1回転**（フレームレート非依存の経過時間ベース）でY軸周りに自動回転。
  - 画面または3D空間に対する操作（`pointerdown` / `wheel` / `touchstart` / `keydown` / `input` / `OrbitControls` 操作）で即座に停止。
  - 最後の操作から **20秒間無操作** で自動回転を再開。

---

## 4. ブラウザ永続化（`localStorage`）

`color-viewer.html` と同一オリジン（`https://labo.com`）上で以下のキーを共有します。

| キー名 | 用途 |
| :--- | :--- |
| `craftband_color_viewer_colors_v1` | 表示する色一覧の参照（読取のみ） |
| `craftband_color_viewer_band_name_v1` | タイトル用のバンド種類名の参照（読取のみ） |
| `craftband_color_viewer_bridge_event_v1` | 呼出し元へ返却する確定・キャンセルイベントの一時受渡し |

---

## 5. 外部連携 API 仕様

### 5.1 URLクエリパラメータ

| パラメータ名 | 型 | 必須 | 説明 |
| :--- | :--- | :---: | :--- |
| `cb_req_id` | 文字列 | 任意 | 呼出しリクエストの一意識別子。返却メッセージにそのままエコーバックされ、多重経路での重複受信防止・照合に使用。 |
| `cb_slot` | 文字列 | 任意 | 呼出し元のスロット識別子（例: `AB`）。返却メッセージにそのままエコーバックされる。 |

※ `cb_req_id` または `cb_slot` のいずれかが指定されていれば「色選択モード」になります。

### 5.2 呼出し元への返却プロトコル

「二色を確定して戻る」または「キャンセル」押下時、`color-viewer.html` と共通の同一オリジン向け4経路すべてへ同時に送信し、`window.close()` を試行します：

1. `window.opener.postMessage(message, window.location.origin)`（別タブ／別ウィンドウ呼出し）
2. `window.parent.postMessage(message, window.location.origin)`（`iframe` 埋込呼出し）
3. `BroadcastChannel("craftband_color_viewer_channel_v1").postMessage(message)`
4. `localStorage.setItem("craftband_color_viewer_bridge_event_v1", JSON.stringify(message))`（`storage` イベント経由）

#### 返却ペイロード：二色確定時
```json
{
  "type": "COLOR_VIEWER_SELECT_PAIR",
  "colorA": "#ffffff",
  "nameA": "白",
  "productInfoA": "",
  "colorB": "#000000",
  "nameB": "黒",
  "productInfoB": "",
  "cb_req_id": "AB_1732500000000_ab12",
  "cb_slot": "AB",
  "sentAt": 1732500012345
}
```

#### 返却ペイロード：キャンセル時（`color-viewer.html` と共通）
```json
{
  "type": "COLOR_VIEWER_CANCEL",
  "cb_req_id": "AB_1732500000000_ab12",
  "cb_slot": "AB",
  "sentAt": 1732500012345
}
```

### 5.3 呼出し側ツール実装時の要点
- `color-viewer.html` と同一の `BroadcastChannel` 名・`localStorage` ブリッジキー・`COLOR_VIEWER_CANCEL` 形式を共有しているため、受信リスナーを共通化し、`data.type === "COLOR_VIEWER_SELECT_PAIR"` の分岐を追加するだけで連携可能。
- `message` イベント受信時は `event.origin === window.location.origin` を検証し、`cb_req_id` でリクエスト照合および重複排除を行う。
- `storage` イベントで受信した後は `localStorage.removeItem("craftband_color_viewer_bridge_event_v1")` で後始末を行う。
