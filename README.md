[English README](README_EN.md)

# プロジェクト / 実行ファイル一覧

CraftBandMesh シリーズは、クラフトバンド／紙バンドなどの帯状素材を用いた
かご製作のための **設計・計算・検証・記録** を支援するアプリケーション群です。

公式サイトには、本シリーズで作成された多数の **フリーデータ** が、
製作途中および完成後の写真とともに登録・公開されています。  
これらのデータは単なるサンプルではなく、  
**実制作に基づく設計例・寸法検討・構造確認のための参照用データベース**
として位置づけられています。


## CraftBandMesh / CraftBandMesh.exe
縦横（＋楕円）・丸・輪弧の底、および側面を編む方式のサイズ計算を行うアプリです。  
データベース化したバンドの種類、編みかた、付属品から選択して組み合わせることで、  
クラフトバンド／紙バンドのオリジナルレシピを簡単に作成できます。  
プレビュー機能により、底ひもの配置間隔や側面の編みかたのバランスを確認できます。

## CraftBandSqare45 / CraftBandSqare45.exe
北欧編みや斜め網代など、縦横に組んだ底を交差方向に対して 45 度で立ち上げる方式の
サイズ計算を行うアプリです。  
プレビュー機能により配置を簡単に確認できます。  
画像は等倍で生成されるため、印刷して型紙として使用できます。  
折りカラー編み（OriColor Weave）にも対応しています。

## CraftBandKnot / CraftBandKnot.exe
四つ畳み編み（石畳編み／ノット編み／2 本結び）のサイズ計算を行うアプリです。  
バンドの種類ごとにゲージを持ち、要尺やコマ寸法について  
実測値／実測値から算出した係数／既定の係数のいずれを使用するか設定できます。  
コマの形状（右側・左側）やひもの色に応じたプレビュー画像で完成形を確認できます。

## CraftBandSquare / CraftBandSquare.exe
縦横に組んだ底をそのまま立ち上げる方式のサイズ計算を行うアプリです。  
ひもの間隔を指定すれば四つ目になります。  
ひもの上や目の中に、縦／横／斜め方向の差しひもを指定できます。  
ひも幅や色を個別に変更し、底や側面の交差の上下パターンを指定して
プレビューすることで、模様をデザインできます。

## CraftBandHexagon / CraftBandHexagon.exe
60 度ごと 3 方向にひもを組み、ひもに沿って立ち上げるタイプのかごを
デザインするアプリです。  
ひも幅や六つ目のサイズのほか、六つ目の脇にできる三角形のサイズから
計算することもできます。  
側面の六つ目のサイズを立ち上げ位置に合わせて調整できます。  
六つ目の型紙作成に便利で、右綾／左綾や 3 軸織り模様を描画できます。

## CbMesh / CbMesh.exe
シリーズアプリのランチャーです。  
拡張子に関連付けることで、5 点の exe の中から対応するアプリを起動できます。

## CraftBand / CraftBand.dll
設定ファイルのデータベースや編集用フォームなどを提供する共通ライブラリです。


# Features
* バンド素材による「かご」を設計・デザインできます
* バンドの種類・編みかた・付属品を設定ファイルとして登録し、選択して使用できます
* シリーズの各アプリで同じ設定データを共有できます
* でき上がり寸法を確認しながら、ひも幅や本数を調整できます
* 手持ちのひもの長さに合わせてサイズを決定できます
* ひもに色を設定し、色ごとのカットリストを出力できます
* バンド色・幅・上下交差パターンの組み合わせをシミュレーションできます
* 一部のアプリでは、完成形を 3D プレビューで確認できます
* 最近のバージョンアップにより、完成形だけでなく  
  **製作途中のプロセス情報もデータとして保存可能**になりました
* 本ソフトで作成された多数のフリーデータが、製作写真とともに公式サイトに公開されており、  
  実制作に基づく設計や寸法検討の参考資料として活用できます


# CraftBandMesh XML Format (.cbmesh)

CraftBandMesh XML Format は、CraftBandMesh シリーズで共通に使用される
XML ベースのデータ形式です。

従来 `.xml` で保存されていたデータを拡張・統合したもので、  
**Ver.1.9 以降** は `.cbmesh` 拡張子が正式採用されています。  
旧形式の `.xml` ファイルも引き続き互換性があります。

## 拡張子と用途
- `.cbmesh` … 作品データおよび設定データ（推奨）
- `.CBMESH` … 表記上、設定データ識別用（実体は同一）

## データ構造
XML DataSet 構造で、XSD スキーマにより定義されています。
- 設定データ: `<dstMasterTables>`
- 作品データ: `<dstDataTables>`

内部ではそれぞれ `ReadXml()` により処理され、  
拡張子や大文字小文字を区別せずに扱えます。


# Requirement
* Microsoft .NET Runtime 6.0
* Microsoft Windows Desktop Runtime 6.0


# Installation
バイナリ一式を任意の場所に配置し、各 `.exe` を直接実行してください。


# Current Binary Version
- Installer         1.9.1
- CraftBand.dll     1.9.1.0
- CraftBandMesh     1.9.1.0
- CraftBandSqare45  1.6.1.0
- CraftBandKnot     1.5.1.0
- CraftBandSquare   1.4.1.0
- CraftBandHexagon  1.1.1.0
- CbMesh            1.0.1.0


# Usage
* シリーズ概要・ソフト説明  
  https://labo.com/CraftBand/craftbandmesh-series/
* CraftBandMesh  
  https://labo.com/CraftBand/CraftbandMesh/
* CraftBandSqare45  
  https://labo.com/CraftBand/CraftBandSquare45/
* CraftBandKnot  
  https://labo.com/CraftBand/CraftBandKnot/
* CraftBandSquare  
  https://labo.com/CraftBand/CraftBandSquare/
* CraftBandHexagon  
  https://labo.com/CraftBand/CraftBandHexagon/
* CbMesh  
  https://labo.com/CraftBand/CbMesh/
* サンプル・フリーデータ  
  https://labo.com/CraftBand/sitemap/


# Author
* Miho Harusawa (CraftBandLabo)
* E-mail: haru@labo.com


# License
CraftBand Series is released under the MIT License.


## Project Contributors Wanted

本プロジェクトでは、機能追加、派生ツールの作成、ドキュメント整備など、
さまざまな形でのプロジェクト協力者を歓迎します。

標準フォーマット（`.cbmesh`）や共通ライブラリについても、  
**独自の利用・拡張・解釈を行うことを推奨**しています。  
本シリーズを基盤とした専門的・実験的な活用も自由に行ってください。
