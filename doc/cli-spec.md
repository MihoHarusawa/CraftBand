# コマンドライン・ヘッドレス実行 仕様書

CraftBandMesh シリーズは、通常の GUI 起動に加え、コマンドラインおよび JSON 設定ファイルによるヘッドレス実行に対応しています。外部プログラム（スイートアプリ、バッチファイル、PowerShell 等）や AI エージェントから、設計データ（`.cbmesh`）の処理・プレビュー生成・一覧出力などの自動化が可能です。

---

## 1. アプリケーション群と起動の概要

個別アプリ（いずれも同一 XML 形式 `.cbmesh` を読み書き。内容はアプリごとに異なり、データは作成したアプリ専用）:

* CraftBandMesh
* CraftBandKnot
* CraftBandSquare45
* CraftBandSquare
* CraftBandHexagon

対応 EXE は、データ内 `tbl目標寸法` の `f_sEXE名` に記載されています。

**CbMesh（ランチャー）**  
XML データの種別を判別し、対応アプリを起動します。`.dbmesh` 拡張子は CbMesh に対応付けています。データの所属 EXE が分かっている場合は **対応 EXE を直接起動** してください（誤った EXE ではデータエラーになります）。CbMesh 経由は二段起動になるため、直接起動を推奨します。

---

## 2. 入出力の概要

**入力**

* データ: 個別の XML プロジェクトファイル（`.cbmesh`）
* マスター（任意）: 処理専用のマスター設定（`.CBMESH`）。省略時は実行環境に保持されたマスターが参照されます

**出力**（コマンドライン記述は全アプリ共通）

| 種別 | 内容 |
| --- | --- |
| `image` | プレビュー画面の底配置図 → GIF |
| `image2` | プレビュー2の側面連結図 → GIF。同名フォルダに 3D モデル（`.obj`）も出力（対応: CraftBandSquare / CraftBandSquare45 / CraftBandKnot のみ） |
| `list` | ひもリスト（配置・サイズ・集計・カットリスト等）→ CSV |

いずれかの出力が指定された場合はヘッドレス動作します。入力のみ、または引数なしの場合は GUI を開きます。

---

## 3. 動作モードの判定ルール

起動時の引数により、次の **3 モード** に自動切り替えします。

| モード | 判定条件 | 振る舞い |
| --- | --- | --- |
| **A. ヘッドレスモード**<br>（画面非表示・自動出力） | 出力系（`-i`, `-i2`, `-l`）が **1 つ以上**、またはその内容を含む `--config` が指定された場合 | 画面（Form）を表示せず、バックグラウンドで計算・ファイル出力後に終了 |
| **B. GUIモード**<br>（入力ファイル指定起動） | 出力系なしで、入力系（`-d`, `-m`）またはスイッチなしの単独パス（D&D）がある場合 | 指定ファイルを読み込んだ状態でメイン画面を起動 |
| **C. GUIモード**<br>（通常起動 / 新規起動） | 引数なし、または `-n` のみ | `-n` あり: 空（新規）。なし: 前回値（履歴）を復元して起動 |

**実行時の内部動作**

* GUI: 出力指定がなければ `ShowDialog` でユーザー操作
* ヘッドレス: 出力指定または `--config` により `IsHeadlessMode` が true。`mdlDllMain.MainProcess` 経由で `ICommonActions.ExecuteHeadlessMode` 等を実行（フォーム非表示）
* `--config`: ファイル内の引数群で **置換**（コマンドライン引数とのマージは行わない）

---

## 4. 起動方法（例）

* **コンフィグ指定（推奨）**: `MyApp.exe --config "C:\path\config.json"`
* **直接スイッチ指定**: `MyApp.exe --data "C:\path\data.cbmesh" --image "C:\out\preview.gif" --list "C:\out\result.csv"`
* **終了コード取得**: 呼び出し側でプロセス終了を待つ

  * Batch: `start "" /wait "C:\path\MyApp.exe" --config "C:\path\config.json"`
  * PowerShell: `& "C:\path\MyApp.exe" --config "C:\path\config.json"; echo $LASTEXITCODE`

---

## 5. コマンドラインスイッチ一覧

* **大文字・小文字**: スイッチ名は区別しない（内部で小文字に正規化）
* **プレフィックス**: `-data` / `--data` / `/data` は同一視
* **`--key=value` 形式**: 非対応
* **`-n` / `--new`**: CbMesh から起動する場合のみ有効。`--config` に記載しても認識されない

| 短縮形 | 完全形 | パラメータ | 役割 | 備考 |
| --- | --- | --- | --- | --- |
| **(なし)** | (なし) | `[ファイルパス]` | 入力データ | **D&D 互換**。先頭引数がスイッチでなければデータファイルとみなす |
| `-d` | `--data` | `<FilePath>` | 入力データ | 複数指定時は先頭のみ有効 |
| `-m` | `--master` | `<FilePath>` | 入力マスター | 最優先。省略時は環境の優先順位（前回値等） |
| `-n` | `--new` | （なし） | 新規作成 | GUI モード時のみ（上記参照） |
| `-i` | `--image` | `<OutputPath>` | 画像1 (GIF) | **ヘッドレストリガー**（拡張子省略時 `.gif`） |
| `-i2` | `--image2` | `<OutputPath>` | 画像2 (GIF) | **ヘッドレストリガー**。3D 用。指定パス名のフォルダを同階層に自動作成（拡張子省略時 `.gif`） |
| `-l` | `--list` | `<OutputPath>` | リスト (CSV) | **ヘッドレストリガー**（拡張子省略時 `.csv`） |
| (なし) | `--config` | `<ConfigPath>` | 設定ファイル | JSON（推奨）またはテキスト |

**出力パスの拡張子（共通ルール）**

出力先に拡張子がない場合、次を自動付加（`--config` の `output` キーも同様）:

| 種別 | 対応スイッチ / JSONキー | 付加される拡張子 |
| --- | --- | --- |
| 画像1 | `-i` / `--image`、`output.image` | `.gif` |
| 画像2 | `-i2` / `--image2`、`output.image2` | `.gif` |
| リスト | `-l` / `--list`、`output.list` | `.csv` |

---

## 6. 設定ファイル（`--config`）の仕様

### 6.1. JSON形式（推奨）

```json
{
  "master": "C:/App/Master/standard.CBMESH",
  "data": "C:/App/Data/d_20260515.cbmesh",
  "output": {
    "list": "C:/App/Export/result.csv",
    "image": "C:/App/Export/preview.gif",
    "image2": "C:/App/Export/preview2.gif"
  }
}
```

* `output.image` / `output.image2`: 拡張子省略時 `.gif`
* `output.list`: 拡張子省略時 `.csv`
* JSON 内のパスは **`/` 推奨**（`\` はエスケープのため `\\` が必要。単一 `\` は構文エラー）

  * **○** `"data": "C:/App/Data/d_20260515.cbmesh"`
  * **△** `"data": "C:\\App\\Data\\d_20260515.cbmesh"`
  * **×** `"data": "C:\App\Data\d_20260515.cbmesh"`

### 6.2. テキスト形式（フォールバック）

JSON として解析できない場合、内容をコマンドライン引数列としてスペース分割して解析します。複雑なエスケープ（`\"` 等）は非推奨のため、可能な限り JSON を使用してください。

```text
--data "C:\Data\target.cbmesh" --master "C:\Master\std.CBMESH" --list "C:\Export\out.csv"
```

---

## 7. 動的パス置換（プレースホルダー）

出力先（`--list`, `--image`, `--image2`）および `--master` に、入力データ（`DataPath`）基準のマクロを記述できます。実行時に置換し絶対パスへ正規化します。

* **`{DIR}`** / `{dir}`: 入力データのフォルダ絶対パス
* **`{NAME}`** / `{name}`: 入力データの拡張子なしファイル名

置換後に拡張子がない場合は §5 の共通ルールを適用します。

**記述例**（入力: `C:\App\Data\Square-Snow.cbmesh`）

1. 固定名: `--list "{DIR}\output.csv"` → `C:\App\Data\output.csv`
2. 接尾辞: `--list "{DIR}\{NAME}_result.csv"` → `C:\App\Data\Square-Snow_result.csv`
3. JSON: `"image": "{DIR}/{NAME}_preview.gif"` → `C:\App\Data\Square-Snow_preview.gif`
4. 拡張子省略: `"list": "{DIR}/{NAME}"`, `"image": "{DIR}/{NAME}"` → `Square-Snow.csv`, `Square-Snow.gif`

---

## 8. 出力・ログ・エラー処理

**生成ファイル（CSV / GIF 等）**

* ヘッドレスでは、コマンドラインで指定したパスへ書き出す
* **既存ファイルは上書き・削除して置換**する（問題がある場合はログ・終了コードで通知）

**コマンドラインへの表示**

* ヘッドレス実行の**結果・メッセージ・警告などは、起動したコマンドライン（コンソール）に表示**する
* GUI のダイアログは表示しない

**ログファイル**

* 実行の詳細ログは **Windows の一時フォルダ（`%TMP%`）** に出力する
* ファイル名は **`<EXEファイル名>.log`**（例: `CraftBandHexagon.exe` → `%TMP%\CraftBandHexagon.log`）
* バッチ等から参照する場合の例: `%TEMP%\CraftBandHexagon.log`

---

## 9. 終了コード

`DllParameters.ProcessCode` / `clsCommandLine.EndCode`（`start /wait` 等で取得）:

| コード | 名称 | 意味 |
| --- | --- | --- |
| 0 | NormalEnd | 正常終了 |
| 1 | DialogResultNG | ダイアログが OK 以外 |
| 5 | HeadlessExecuteError | ヘッドレス実行失敗 |
| 8 | DllFinalizeError | DLL 終了処理失敗（マスター保存失敗等） |
| 9 | Exception | 例外発生 |
| 97 | InvalidData | データ識別失敗等、実行不可 |
| 98 | InvalidArgument | 引数エラー等、実行不可 |
| 99 | DllInitializeError | DLL 開始処理失敗（マスターなし等）、実行不可 |

**成否の目安**

* **成功**: 出力ファイルが正常に生成された場合
* **失敗**: 未知のスイッチ、`--config` 不在、ファイル生成（`ICommonActions`）が `False` を返した場合 等

---
