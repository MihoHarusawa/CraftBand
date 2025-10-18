# プロジェクト / 実行ファイル一覧

## CraftBandMesh / CraftBandMesh.exe
縦横(+楕円)・丸・輪弧の底、プラス側面を編む方式のサイズ計算を行うアプリです。
データベース化したバンドの種類や編みかた・付属品から選択して組み合わせられますので、
クラフトバンド/紙バンドのオリジナルレシピが簡単に作成できます。
プレビュー機能で、底ひもの配置間隔や側面の編みかたのバランスを確認できます。

## CraftBandSqare45 / CraftBandSqare45.exe
北欧編みや斜め網代など、縦横に組んだ底を、交差方向に対して45度で立ち上げる方式のサイズ計算を行うアプリです。
プレビュー機能もついていますので、配置が簡単に確認できます。
画像は等倍で生成しますので、印刷すれば型紙として使うこともできます。
折りカラー編み(OriColorWeave)にも対応しました。

## CraftBandKnot / CraftBandKnot.exe
四つ畳み編み(石畳編み/ノット編み/2本結び)のサイズ計算を行うアプリです。
バンドの種類ごとにゲージを持っており、要尺・コマ寸法については、
入力した実測値/実測値から計算した係数/既定の係数、どれを使うか設定できます。
コマの形状(右側・左側)やひもの色に応じたプレビュー画像で出来上がりを確認することができます。

## CraftBandSquare / CraftBandSquare.exe
縦横に組んだ底をそのまま立ち上げる方式のサイズ計算を行うアプリです。
ひもの間隔を指定すれば四つ目になります。
ひもの上や目の中に、縦/横/斜め方向の差しひもを指定することができます。
ひも幅や色を個別に変えたり、底や側面の交差の上下パターンを指定してプレビューすれば、模様をデザインできます。

## CraftBandHexagon / CraftBandHexagon.exe
60度ごと3方向にひもを組み、ひもに沿って立ち上げるタイプのかごをデザインするアプリです。
ひも幅・六つ目のサイズの他、六つ目の脇に作られる三角形のサイズから計算させることができます。
側面の六つ目のサイズを立ち上げ位置に合わせて調整できます。
六つ目の型紙を作るのに便利です。右綾/左綾や、3軸織り模様を描画できます。

## CbMesh / CbMesh.exe
シリーズアプリのランチャーです。
拡張子に関連付けると、5点のexeから対応するアプリを起動できます。

## CraftBand / CraftBand.dll
設定ファイルのデータベース、編集用フォームなどの共通ライブラリです


# Features
* バンドによって作る'かご'をデザインすることができます。
* バンドの種類・編みかた・付属品などは、設定ファイルに登録し、選ぶだけで使用できます。
* シリーズのアプリは、同じ設定データを共有することができます。
* でき上がり寸法を見ながら、ひも幅やひも数を変えることで、自分の好きなサイズに作れます。
* 手持ちのひもの長さに合わせて、サイズを決めることができます。
* ひもに色を設定することで、色ごとのカットリストを出力することができます。
* バンド色と幅の並び・上下交差パターンの組み合わせをシミュレーションできます。


The CraftBandMesh series is a collection of applications designed for creating baskets 
using bands or tapes made from paper, plastic, or natural materials such as bamboo. 
These applications help you calculate the size, number of bands, length, and patterns created by weaving.

The series includes multiple applications, each tailored to different fundamental weaving methods. 
All applications share a common database that acts as a master configuration file. 
With this series, you can easily create your own original designs and save them as files, 
allowing you to organize and manage your recipes as a library.


# CraftBandMesh XML Format (.cbmesh)

CraftBandMesh XML Format は、CraftBandMesh シリーズで共通に使用される XML ベースのデータ形式です。

このフォーマットは、従来 `.XML` / `.xml` で保存されていたデータを拡張・統合したもので、
**Ver.1.9 以降** のアプリケーションでは `.cbmesh` 拡張子が正式採用されています。
旧バージョンの `.XML` / `.xml` ファイルも引き続き互換性を保って読み込むことができます。

## 　 拡張子と用途
- `.cbmesh` … 作品データおよび設定データの両方で使用されます。新規保存では `.cbmesh` が推奨です。  
- `.CBMESH` … 表記上、設定データを識別するために使われますが、扱いは `.cbmesh` と同一です。  

## 　 データ構造
CraftBandMesh XML Format は XML による DataSet 構造で構成され、XSD スキーマで定義されています。  
データの種類はルート要素によって区別されます：  
- 設定データ: `<dstMasterTables>`  
- 作品データ: `<dstDataTables>`  

アプリ内部では、それぞれ `dstMasterTables.ReadXml()` および `dstDataTables.ReadXml()` によって処理されます。
これにより、`.cbmesh` / `.xml` / `.CBMESH` / `.XML` のすべてを同等に扱うことが可能です。


# CraftBandMesh XML Format (.cbmesh)

The **CraftBandMesh XML Format** is a unified XML-based data format used across the CraftBandMesh series.

This format is an extension and integration of the data previously saved with the `.XML` / `.xml` extensions.
Starting from **Version 1.9**, the `.cbmesh` extension has been officially adopted.
Older `.XML` / `.xml` files remain fully compatible and can still be loaded.

## File Extensions and Usage
- `.cbmesh` - Used for both **Project Data** and **Master Configuration Data**. New files are recommended to use `.cbmesh`.
- `.CBMESH` - Conventionally used in documentation to distinguish **Master Configuration Data**, but is **functionally identical** to `.cbmesh`.

## Data Structure
Both data types share the same XML DataSet structure, which is defined by an XSD schema.
The type of data is differentiated by its root element:
- **Master Configuration Data**: `<dstMasterTables>`
- **Project Data**: `<dstDataTables>`

Internally, they are processed by `dstMasterTables.ReadXml()` and `dstDataTables.ReadXml()`, respectively.
This allows all `.cbmesh` / `.xml` / `.CBMESH` / `.XML` files to be handled identically.


# Requirement

* Microsoft .NET Runtime 6.0
* Microsoft Windows Desktop Runtime 6.0

.NETについては、初回起動時にMicrosoftのWebページからダウンロードを促されますが、
64bit版についてはインストーラを入れています。


# Installation

バイナリ一式があれば、どこに置いてもよいです。
各、実行ファイル(.exe)を開いて実行してください。


# Current Binary Version

- Installer         1.8.15
- CraftBand.dll     1.8.15.0
- CraftBandMesh     1.8.15.0
- CraftBandSqare45  1.5.15.0  
- CraftBandKnot     1.4.15.0
- CraftBandSquare   1.3.15.0
- CraftBandHexagon  1.0.15.0
- CbMesh            1.	

# Usage

* CraftBandMesh:    https://labo.com/CraftBand/CraftbandMesh/
* CraftBandSqare45: https://labo.com/CraftBand/CraftBandSquare45/
* CraftBandKnot :   https://labo.com/CraftBand/CraftBandKnot/
* CraftBandSquare:  https://labo.com/CraftBand/CraftBandSquare/
* CraftBandHexagon: https://labo.com/CraftBand/CraftBandHexagon/
* CbMesh:           https://labo.com/CraftBand/CbMesh/
* サンプルなど      https://labo.com/CraftBand/


# Author

* Miho Harusawa (Labo Harusawa)
* E-mail: haru@labo.com

# License

CraftBand Series is under [MIT license]


## Seeking Translation Contributors

If you wish to use these applications in another language and if you have the ability to translate it from Japanese to that language, Please consider participating as a contributor to the project.
