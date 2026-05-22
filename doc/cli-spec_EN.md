# Command-Line and Headless Execution Specification

The CraftBandMesh series supports headless execution via the command line and JSON configuration files, in addition to normal GUI startup. External programs (suite apps, batch files, PowerShell, etc.) and AI agents can automate processing of design data (`.cbmesh`), preview generation, list export, and more.

---

## 1. Application Family and Startup Overview

Individual applications (all read and write the same XML format `.cbmesh`; content differs per app, and data is specific to the app that created it):

* CraftBandMesh
* CraftBandKnot
* CraftBandSquare45
* CraftBandSquare
* CraftBandHexagon

The corresponding EXE is recorded in the data field `f_sEXE名` in table `tbl目標寸法`.

**CbMesh (launcher)**  
Identifies the XML data type and starts the matching application. The `.dbmesh` extension is associated with CbMesh. When you know which EXE owns the data, **start that EXE directly** (the wrong EXE causes a data error). Launching via CbMesh is a two-step process; direct launch is recommended.

When started via CbMesh.exe, the exit code available to the caller is **CbMesh.exe’s own**, and CbMesh.exe exits successfully (0) once the individual EXE has been launched. **The individual EXE’s processing result and exit code are not available through CbMesh.exe** (see §9).

---

## 2. Input and Output Overview

**Input**

* Data: individual XML project file (`.cbmesh`)
* Master (optional): master settings (`.CBMESH`) for that run only. If omitted, the master stored in the execution environment is used

**Output** (command-line syntax is common to all apps)

| Type | Description |
| --- | --- |
| `image` | Bottom layout from the preview screen → GIF |
| `image2` | Side-joined view from preview 2 → GIF; also writes a 3D model (`.obj`) in a same-named folder (supported: CraftBandSquare / CraftBandSquare45 / CraftBandKnot only) |
| `list` | Band list (layout, sizes, totals, cut list, etc.) → CSV |

If any output is specified, the app runs headless. Input only or no arguments opens the GUI.

---

## 3. Operating Mode Selection

Arguments at startup automatically select one of **three modes**:

| Mode | Condition | Behavior |
| --- | --- | --- |
| **A. Headless mode**<br>(no UI, automatic output) | One or more output switches (`-i`, `-i2`, `-l`), or `--config` that includes them | No form is shown; compute and write files in the background, then exit |
| **B. GUI mode**<br>(open with input file) | No output switches; input switches (`-d`, `-m`) or a lone path without a switch (drag-and-drop) | Opens the main UI with the specified file loaded |
| **C. GUI mode**<br>(normal / new) | No arguments, or `-n` only | With `-n`: empty (new). Without `-n`: restore previous session (history) |

**Runtime behavior**

* GUI: if no output is specified, `ShowDialog` for user interaction
* Headless: output or `--config` sets `IsHeadlessMode` to true; runs via `mdlDllMain.MainProcess` and `ICommonActions.ExecuteHeadlessMode` (no form)
* `--config`: arguments in the file **replace** the command line (no merge with CLI arguments)

---

## 4. How to Launch (Examples)

* **Config file (recommended)**: `MyApp.exe --config "C:\path\config.json"`
* **Direct switches**: `MyApp.exe --data "C:\path\data.cbmesh" --image "C:\out\preview.gif" --list "C:\out\result.csv"`
* **Exit code**: wait for process exit on the caller side

  * Batch: `start "" /wait "C:\path\MyApp.exe" --config "C:\path\config.json"`
  * PowerShell: `& "C:\path\MyApp.exe" --config "C:\path\config.json"; echo $LASTEXITCODE`

---

## 5. Command-Line Switches

* **Case**: switch names are case-insensitive (normalized to lowercase internally)
* **Prefix**: `-data`, `--data`, and `/data` are equivalent
* **`--key=value`**: not supported
* **`-n` / `--new`**: only when launching from CbMesh; ignored if written in `--config`

| Short | Long | Parameter | Role | Notes |
| --- | --- | --- | --- | --- |
| **(none)** | (none) | `[file path]` | Input data | **Drag-and-drop compatible**. If the first argument is not a switch, it is treated as the data file |
| `-d` | `--data` | `<FilePath>` | Input data | If specified multiple times, only the first is used |
| `-m` | `--master` | `<FilePath>` | Input master | Highest priority; if omitted, environment priority (e.g. last used) applies |
| `-n` | `--new` | (none) | New document | GUI mode only (see above) |
| `-i` | `--image` | `<OutputPath>` | Image 1 (GIF) | **Headless trigger** (`.gif` appended if extension omitted) |
| `-i2` | `--image2` | `<OutputPath>` | Image 2 (GIF) | **Headless trigger**. For 3D; creates a folder alongside the given path name (`.gif` if extension omitted) |
| `-l` | `--list` | `<OutputPath>` | List (CSV) | **Headless trigger** (`.csv` if extension omitted) |
| (none) | `--config` | `<ConfigPath>` | Config file | JSON (recommended) or text |

**Output path extensions (common rule)**

If the destination path has no extension, the following are appended (`output` keys in `--config` follow the same rule):

| Type | Switch / JSON key | Appended extension |
| --- | --- | --- |
| Image 1 | `-i` / `--image`, `output.image` | `.gif` |
| Image 2 | `-i2` / `--image2`, `output.image2` | `.gif` |
| List | `-l` / `--list`, `output.list` | `.csv` |

---

## 6. Configuration File (`--config`)

### 6.1. JSON (Recommended)

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

* `output.image` / `output.image2`: append `.gif` if extension omitted
* `output.list`: append `.csv` if extension omitted
* Use **`/` in JSON paths** (`\` must be doubled as `\\`; a single `\` is a JSON syntax error)

  * **Recommended**: `"data": "C:/App/Data/d_20260515.cbmesh"`
  * **Allowed**: `"data": "C:\\App\\Data\\d_20260515.cbmesh"`
  * **Invalid**: `"data": "C:\App\Data\d_20260515.cbmesh"`

### 6.2. Text Format (Fallback)

If the file is not valid JSON, its contents are split on spaces and parsed as command-line arguments. Complex escapes (`\"`, etc.) are not fully supported; prefer JSON when possible.

```text
--data "C:\Data\target.cbmesh" --master "C:\Master\std.CBMESH" --list "C:\Export\out.csv"
```

---

## 7. Dynamic Path Placeholders

For output paths (`--list`, `--image`, `--image2`) and `--master`, you can use macros based on the resolved input data (`DataPath`). They are substituted at runtime and normalized to absolute paths.

* **`{DIR}`** / `{dir}`: absolute path of the folder containing the input data file
* **`{NAME}`** / `{name}`: file name of the input data without extension

If the path still has no extension after substitution, apply the common rule in §5.

**Examples** (input: `C:\App\Data\Square-Snow.cbmesh`)

1. Fixed name: `--list "{DIR}\output.csv"` → `C:\App\Data\output.csv`
2. Suffix: `--list "{DIR}\{NAME}_result.csv"` → `C:\App\Data\Square-Snow_result.csv`
3. JSON: `"image": "{DIR}/{NAME}_preview.gif"` → `C:\App\Data\Square-Snow_preview.gif`
4. Omit extension: `"list": "{DIR}/{NAME}"`, `"image": "{DIR}/{NAME}"` → `Square-Snow.csv`, `Square-Snow.gif`

---

## 8. Output, Logging, and Errors

**Generated files (CSV, GIF, etc.)**

* In headless mode, files are written to paths specified on the command line
* **Existing files are overwritten or deleted and replaced** (issues are reported via log and exit code)

**Command-line display**

* **Results, messages, and warnings** from headless runs are **shown on the console** used to start the process
* GUI dialogs are not shown

**Log file**

* Detailed execution logs are written under **Windows temporary folder (`%TMP%`)**
* File name: **`<EXE file name>.log`** (e.g. `CraftBandHexagon.exe` → `%TMP%\CraftBandHexagon.log`)
* Example for batch scripts: `%TEMP%\CraftBandHexagon.log`

---

## 9. Exit Codes

### 9.1. Direct launch of an individual EXE

`DllParameters.ProcessCode` / `clsCommandLine.EndCode` (obtain with `start /wait`, etc.):

| Code | Name | Meaning |
| --- | --- | --- |
| 0 | NormalEnd | Success |
| 1 | DialogResultNG | Dialog result was not OK |
| 5 | HeadlessExecuteError | Headless execution failed |
| 8 | DllFinalizeError | DLL shutdown failed (e.g. master save failed) |
| 9 | Exception | Exception occurred |
| 97 | InvalidData | Cannot run (e.g. data identification failed) |
| 98 | InvalidArgument | Cannot run (e.g. argument error) |
| 99 | DllInitializeError | Cannot run (e.g. DLL init failed, no master) |

**Success / failure**

* **Success**: output files were generated successfully
* **Failure**: unknown switch, missing `--config`, file generation (`ICommonActions`) returned `False`, etc.

### 9.2. Launch via CbMesh.exe

CbMesh.exe is a launcher that starts the matching individual EXE. The exit code obtained with `start /wait`, etc. is **CbMesh.exe’s**; the table and success/failure notes in §9.1 **do not apply to the individual EXE**.

* **Success (0)**: CbMesh.exe exits after it has identified the data and successfully started the matching individual EXE. Whether the individual EXE later completed headless processing or produced output files cannot be determined from this exit code.
* **Failure (non-zero)**: Data identification or launch failed on the CbMesh.exe side (events comparable to 97, 98, etc. in §9.1).
* **Individual EXE processing result**: Headless output success/failure, §9.1 exit codes, and log contents are **not returned to the caller via CbMesh.exe**. If automation must judge the individual EXE’s outcome, **start the matching EXE directly** when its owner is known (see §1).

---
