#!/usr/bin/env python3
"""
こいこいルールエンジン実装のための30個のIssueを作成するスクリプト

使用方法:
    python scripts/create_issues.py

前提条件:
    1. Python 3.6以上
    2. GitHub Personal Access Token (環境変数 GITHUB_TOKEN に設定)
    3. マイルストーンとラベルが作成済み

環境変数:
    GITHUB_TOKEN: GitHubのPersonal Access Token (repo権限が必要)
    GITHUB_REPOSITORY: リポジトリ名 (デフォルト: ToYama170402/hanafuda)
"""

import os
import sys
import json
import time
from urllib import request, error
from urllib.parse import urlencode

# 設定
REPO_OWNER = "ToYama170402"
REPO_NAME = "hanafuda"
GITHUB_API_URL = "https://api.github.com"
GITHUB_TOKEN = os.environ.get("GITHUB_TOKEN")

# Issue定義
ISSUES = [
    # フェーズ1: 基本機能（MVP）
    {
        "title": "プロジェクト構造とソリューションのセットアップ",
        "body": """## 概要
プロジェクトの基本構造とソリューションファイルをセットアップします。

## タスク内容
- [ ] .NET Coreソリューションファイルの作成
- [ ] 以下のプロジェクトを作成
  - `HanafudaEngine.Core` (クラスライブラリ)
  - `HanafudaEngine.Domain` (クラスライブラリ)
  - `HanafudaEngine.Facade` (クラスライブラリ)
  - `HanafudaEngine.Tests` (xUnitテストプロジェクト)
- [ ] プロジェクト間の参照関係を設定
- [ ] 基本的な名前空間構造を確立
- [ ] .gitignoreファイルの作成（bin/, obj/, .vs/など）
- [ ] READMEの更新（ビルド手順の追加）

## 成果物
- `HanafudaEngine.sln`
- 各プロジェクトの`.csproj`ファイル
- 更新された`.gitignore`

## 推定工数
2-3時間

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-1-プロジェクト構造とソリューションのセットアップ)
- [クイックスタートガイド](../docs/QUICKSTART.md)
- [実装仕様書](../docs/implementation-specification.md)""",
        "labels": ["setup", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": []
    },
    {
        "title": "Core層の基本データモデル実装",
        "body": """## 概要
Core層の基本的なデータモデル（値オブジェクト）を実装します。

## タスク内容
- [ ] `Month`列挙型の実装
- [ ] `CardType`列挙型の実装
- [ ] `SpecialCardFlag`列挙型の実装
- [ ] `Card`クラスの実装（等値比較含む）
- [ ] `PlayerId`列挙型の実装
- [ ] `YakuType`列挙型の実装
- [ ] `Yaku`クラスの実装
- [ ] 基本的なユニットテストの作成

## 成果物
- `HanafudaEngine.Core/Models/`以下の全クラス
- 対応するユニットテスト

## 推定工数
4-6時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-2-core層の基本データモデル実装)
- [実装仕様書](../docs/implementation-specification.md#31-基本的な値オブジェクト)""",
        "labels": ["core", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [1]
    },
    {
        "title": "札の定義と管理",
        "body": """## 概要
48枚の花札を定義し、札の管理機能を実装します。

## タスク内容
- [ ] `CardDefinitions`クラスの実装
- [ ] 48枚の花札を定義（実装仕様書2.2に基づく）
- [ ] 月別、種類別の札取得メソッドの実装
- [ ] 特殊札フラグの設定（赤短、青短、盃、鹿、猪、蝶、雨札）
- [ ] 札定義の整合性テスト
  - 合計48枚の確認
  - 各月4枚の確認
  - 光札5枚、種札9枚、短冊札10枚、カス札24枚の確認

## 成果物
- `HanafudaEngine.Core/Models/CardDefinitions.cs`
- 札定義の検証テスト

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-3-札の定義と管理)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#2-花札の構成)""",
        "labels": ["core", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [2]
    },
    {
        "title": "ゲーム状態管理クラスの実装",
        "body": """## 概要
ゲームの状態を管理するクラスを実装します。

## タスク内容
- [ ] `GamePhase`列挙型の実装
- [ ] `TurnPhase`列挙型の実装
- [ ] `PlayerState`クラスの実装（不変性を保つWithメソッド含む）
- [ ] `GameState`クラスの実装（不変性を保つWithメソッド含む）
- [ ] `GameResult`クラスの実装
- [ ] 状態遷移のユニットテスト

## 成果物
- `HanafudaEngine.Domain/`以下の状態管理クラス
- 状態遷移テスト

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-4-ゲーム状態管理クラスの実装)
- [実装仕様書](../docs/implementation-specification.md#34-ゲーム状態)""",
        "labels": ["domain", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [2]
    },
    {
        "title": "山札管理とシャッフル機能",
        "body": """## 概要
山札のシャッフルと配札機能を実装します。

## タスク内容
- [ ] デッキのシャッフルロジック実装
- [ ] シード値指定によるテスト可能なシャッフル
- [ ] 配札ロジックの実装（親4枚→子4枚→場4枚→親4枚→子4枚→場4枚）
- [ ] 山札からのドロー機能
- [ ] シャッフルのテスト

## 成果物
- 山札管理機能
- テスト用の固定デッキ機能

## 推定工数
3-4時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-5-山札管理とシャッフル機能)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#33-札の配布)""",
        "labels": ["domain", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [3]
    },
    {
        "title": "基本的な役判定の実装（光札、種、短冊、カス）",
        "body": """## 概要
基本的な役の判定ロジックを実装します。

## タスク内容
- [ ] `YakuEvaluator`クラスの基本構造
- [ ] 五光の判定ロジック
- [ ] 四光の判定ロジック
- [ ] 雨四光の判定ロジック
- [ ] 三光の判定ロジック
- [ ] 種（5枚以上）の判定ロジック
- [ ] 短冊（5枚以上）の判定ロジック
- [ ] カス（10枚以上）の判定ロジック
- [ ] 光札の役の排他性処理
- [ ] 各役の包括的なテスト

## 成果物
- `HanafudaEngine.Domain/Services/YakuEvaluator.cs`
- 役判定の正常系・異常系テスト

## 推定工数
8-10時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-6-基本的な役判定の実装光札種短冊カス)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#6-役やく)
- [実装仕様書](../docs/implementation-specification.md#6-役判定アルゴリズム)""",
        "labels": ["domain", "yaku", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [3]
    },
    {
        "title": "得点計算機能の実装",
        "body": """## 概要
役の得点を計算する機能を実装します。

## タスク内容
- [ ] `ScoreCalculator`クラスの実装
- [ ] 基本得点の計算
- [ ] ボーナス得点の計算（種、短冊、カスの枚数超過分）
- [ ] こいこいボーナス（2倍）の処理
- [ ] こいこい失敗時の得点処理（0点）
- [ ] `CalculateFinalScore`メソッドの実装
- [ ] `CalculateGameResult`メソッドの実装
- [ ] 得点計算の各種パターンテスト

## 成果物
- `HanafudaEngine.Domain/Services/ScoreCalculator.cs`
- 得点計算テスト

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-7-得点計算機能の実装)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#7-得点計算)
- [実装仕様書](../docs/implementation-specification.md#7-得点計算)""",
        "labels": ["domain", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [6]
    },
    {
        "title": "ターン進行の基本ロジック",
        "body": """## 概要
ターンの進行を管理するロジックを実装します。

## タスク内容
- [ ] `TurnManager`クラスの実装
- [ ] ターンフェーズの遷移ロジック
  - PlayFromHand: 手札を出す
  - SelectFieldCard: 場札を選ぶ
  - DrawFromDeck: 山札を引く
  - SelectFieldCardForDraw: 引いた札で場札を選ぶ
  - YakuCheck: 役チェック
  - TurnEnd: ターン終了
- [ ] 札の取得ロジック
  - 同月札なし → 場に追加
  - 同月札1枚 → 自動取得
  - 同月札2枚以上 → 選択待ち
- [ ] プレイヤー交代処理
- [ ] ターン数のカウント
- [ ] ターン進行のテスト

## 成果物
- `HanafudaEngine.Domain/Services/TurnManager.cs`
- ターン進行テスト

## 推定工数
8-10時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-8-ターン進行の基本ロジック)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#41-ターンの流れ)
- [実装仕様書](../docs/implementation-specification.md#42-ターンの進行)""",
        "labels": ["domain", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [4, 5]
    },
    {
        "title": "アクション定義とバリデーション",
        "body": """## 概要
プレイヤーのアクションを定義し、バリデーション機能を実装します。

## タスク内容
- [ ] `IGameAction`インターフェースの定義
- [ ] `ActionType`列挙型の定義
- [ ] 各アクションクラスの実装
  - `PlayCardAction`
  - `SelectFieldCardAction`
  - `CallKoiKoiAction`
  - `CallShobuAction`
- [ ] `ActionValidator`クラスの実装
- [ ] `ValidationResult`クラスの実装
- [ ] 各アクションの妥当性検証ロジック
- [ ] バリデーションのテスト

## 成果物
- `HanafudaEngine.Domain/Actions/`以下の全クラス
- `HanafudaEngine.Domain/Services/ActionValidator.cs`
- バリデーションテスト

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-9-アクション定義とバリデーション)
- [実装仕様書](../docs/implementation-specification.md#52-igameaction)""",
        "labels": ["domain", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [4]
    },
    {
        "title": "ゲームファサードの実装",
        "body": """## 概要
ゲーム全体を統括するファサードクラスを実装します。

## タスク内容
- [ ] `KoiKoiGame`クラスの基本構造
- [ ] ゲーム初期化処理
- [ ] `GetState()`メソッドの実装
- [ ] `GetAvailableActions()`メソッドの実装
- [ ] `ExecuteAction()`メソッドの実装
- [ ] `GameActionResult`クラスの実装
- [ ] ゲームリセット機能
- [ ] 基本的なゲームフローの統合テスト

## 成果物
- `HanafudaEngine.Facade/KoiKoiGame.cs`
- `HanafudaEngine.Facade/GameActionResult.cs`
- 統合テスト

## 推定工数
8-10時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-10-ゲームファサードの実装)
- [実装仕様書](../docs/implementation-specification.md#51-koikoigame)""",
        "labels": ["facade", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [4, 8, 9]
    },
    {
        "title": "配札検証と勝敗判定",
        "body": """## 概要
配札の妥当性検証と勝敗判定のロジックを実装します。

## タスク内容
- [ ] `DealingValidator`クラスの実装
- [ ] 場に同月4枚の検証
- [ ] 再配札処理
- [ ] 勝敗判定ロジック
  - 役完成時に「勝負」を宣言した場合
  - 8ターン経過で総流れ（引き分け）
- [ ] 親の交代処理
- [ ] 勝敗判定のテスト

## 成果物
- `HanafudaEngine.Domain/Services/DealingValidator.cs`
- 勝敗判定ロジック
- 配札・勝敗のテスト

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-11-配札検証と勝敗判定)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#34-配札の確認)
- [実装仕様書](../docs/implementation-specification.md#94-配札の検証)""",
        "labels": ["domain", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [5, 10]
    },
    {
        "title": "MVP統合テストとデバッグ",
        "body": """## 概要
MVP版の統合テストを実施し、バグを修正します。

## タスク内容
- [ ] 完全なゲームフローの統合テスト
  - ゲーム開始から終了まで
  - 正常系の動作確認
- [ ] 各種エッジケースのテスト
- [ ] パフォーマンステスト（基本的なもの）
- [ ] バグ修正
- [ ] ドキュメントの更新

## 成果物
- 包括的な統合テスト
- 動作するMVP版ルールエンジン

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-12-mvp統合テストとデバッグ)
- [実装仕様書](../docs/implementation-specification.md#10-テスト戦略)""",
        "labels": ["testing", "priority-high"],
        "milestone": "M1: MVP完成",
        "depends_on": [11]
    },
    # フェーズ2: 完全なルール実装
    {
        "title": "こいこいの宣言と処理",
        "body": """## 概要
こいこいの宣言と処理ロジックを実装します。

## タスク内容
- [ ] こいこい/勝負の選択フェーズの実装
- [ ] `KoiKoiDecision`フェーズの状態管理
- [ ] こいこいフラグの管理
- [ ] こいこい宣言後のゲーム続行処理
- [ ] こいこい成功時の得点2倍処理
- [ ] こいこい失敗時の処理（相手の得点2倍）
- [ ] 双方がこいこいを宣言した場合の処理
- [ ] こいこいのテスト

## 成果物
- こいこい機能の完全実装
- こいこいシナリオのテスト

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-13-こいこいの宣言と処理)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#5-こいこいの選択)""",
        "labels": ["domain", "koikoi", "priority-high"],
        "milestone": "M2: 完全ルール実装",
        "depends_on": [12]
    },
    {
        "title": "喰い付き（くいつき）の実装",
        "body": """## 概要
喰い付きの判定と処理ロジックを実装します。

## タスク内容
- [ ] `KuitsukiHandler`クラスの実装
- [ ] 喰い付き判定ロジック
  - 手札、場札、山札で同月3枚
- [ ] 喰い付き時の4枚取得処理
- [ ] 喰い付きのテスト

## 成果物
- `HanafudaEngine.Domain/Services/KuitsukiHandler.cs`
- 喰い付きテスト

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-14-喰い付きくいつきの実装)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#42-特殊な状況)
- [実装仕様書](../docs/implementation-specification.md#91-喰い付きくいつき)""",
        "labels": ["domain", "special-rule", "priority-high"],
        "milestone": "M2: 完全ルール実装",
        "depends_on": [12]
    },
    {
        "title": "特殊な役の実装（猪鹿蝶、花見で一杯、月見で一杯）",
        "body": """## 概要
特殊な種札の役を実装します。

## タスク内容
- [ ] 猪鹿蝶の判定ロジック
  - 萩に猪、紅葉に鹿、牡丹に蝶の3枚
- [ ] 花見で一杯の判定ロジック
  - 桜に幕と菊に盃の2枚
- [ ] 月見で一杯の判定ロジック
  - 芒に月と菊に盃の2枚
- [ ] 特殊な種札の役と「種」の重複処理
- [ ] 特殊役のテスト

## 成果物
- `YakuEvaluator`への追加実装
- 特殊役テスト

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-15-特殊な役の実装猪鹿蝶花見で一杯月見で一杯)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#62-種札の役)""",
        "labels": ["domain", "yaku", "priority-medium"],
        "milestone": "M2: 完全ルール実装",
        "depends_on": [6]
    },
    {
        "title": "短冊の特殊役（赤短、青短）の実装",
        "body": """## 概要
赤短と青短の役を実装します。

## タスク内容
- [ ] 赤短の判定ロジック
  - 松・梅・桜の赤短3枚
- [ ] 青短の判定ロジック
  - 牡丹・菊・紅葉の青短3枚
- [ ] 赤短/青短と「短冊」の重複処理
- [ ] 特殊短冊役のテスト

## 成果物
- `YakuEvaluator`への追加実装
- 特殊短冊役テスト

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-16-短冊の特殊役赤短青短の実装)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#63-短冊札の役)""",
        "labels": ["domain", "yaku", "priority-medium"],
        "milestone": "M2: 完全ルール実装",
        "depends_on": [6]
    },
    {
        "title": "総流れ（そうながれ）の実装",
        "body": """## 概要
8ターン経過時の総流れ（引き分け）処理を実装します。

## タスク内容
- [ ] `DrawHandler`クラスの実装
- [ ] 8ターン経過の判定
- [ ] 総流れ時の処理
  - 引き分け判定
  - 親の続投
- [ ] 総流れのテスト

## 成果物
- `HanafudaEngine.Domain/Services/DrawHandler.cs`
- 総流れテスト

## 推定工数
3-4時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-17-総流れそうながれの実装)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#42-特殊な状況)
- [実装仕様書](../docs/implementation-specification.md#93-総流れそうながれ)""",
        "labels": ["domain", "special-rule", "priority-medium"],
        "milestone": "M2: 完全ルール実装",
        "depends_on": [11]
    },
    {
        "title": "配札の再検証機能",
        "body": """## 概要
配札の再検証機能を強化します。

## タスク内容
- [ ] 場に同月4枚の検証ロジックの強化
- [ ] 自動再配札処理の実装
- [ ] 再配札のテスト
- [ ] 再配札の回数制限（無限ループ防止）

## 成果物
- 強化された配札検証
- 再配札テスト

## 推定工数
3-4時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-18-配札の再検証機能)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#34-配札の確認)""",
        "labels": ["domain", "priority-medium"],
        "milestone": "M2: 完全ルール実装",
        "depends_on": [11]
    },
    {
        "title": "フェーズ2統合テストとデバッグ",
        "body": """## 概要
完全ルール実装版の統合テストを実施します。

## タスク内容
- [ ] 完全ルールでの統合テスト
- [ ] 複雑なゲームシナリオのテスト
  - こいこいの連続
  - 喰い付きを含むゲーム
  - 総流れのケース
- [ ] バグ修正
- [ ] パフォーマンス検証
- [ ] ドキュメントの更新

## 成果物
- 完全ルール実装版のルールエンジン
- 包括的な統合テスト

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-19-フェーズ2統合テストとデバッグ)""",
        "labels": ["testing", "priority-high"],
        "milestone": "M2: 完全ルール実装",
        "depends_on": [13, 14, 15, 16, 17, 18]
    },
    # フェーズ3: 拡張機能
    {
        "title": "手四（てし）の実装（オプション）",
        "body": """## 概要
手四による即座の勝利処理を実装します（オプション機能）。

## タスク内容
- [ ] `TeshiHandler`クラスの実装
- [ ] 手四判定ロジック
  - 配られた手札に同月4枚
- [ ] 手四による即座の勝利処理
- [ ] 手四ルールの有効/無効切り替え
- [ ] 手四のテスト

## 成果物
- `HanafudaEngine.Domain/Services/TeshiHandler.cs`
- 手四テスト

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-20-手四てしの実装オプション)
- [ゲームルール仕様書](../docs/hanafuda-specification.md#42-特殊な状況)
- [実装仕様書](../docs/implementation-specification.md#92-手四てし)""",
        "labels": ["domain", "special-rule", "optional", "priority-low"],
        "milestone": "M3: 拡張機能実装",
        "depends_on": [19]
    },
    {
        "title": "ルール設定のカスタマイズ機能",
        "body": """## 概要
ゲームルールをカスタマイズできる機能を実装します。

## タスク内容
- [ ] `GameRules`クラスの実装
- [ ] 各役の得点をカスタマイズ可能に
  - 五光、四光、雨四光、三光
  - 猪鹿蝶、花見で一杯、月見で一杯
  - 赤短、青短
- [ ] 特殊ルールの有効/無効切り替え
  - 手四の有効化
  - こいこいの倍率設定（2倍、3倍など）
  - 最大ターン数の設定
- [ ] デフォルトルールの定義
- [ ] カスタムルールのテスト

## 成果物
- `HanafudaEngine.Domain/GameRules.cs`
- ルールカスタマイズテスト

## 推定工数
5-6時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-21-ルール設定のカスタマイズ機能)
- [実装仕様書](../docs/implementation-specification.md#12-拡張性と変則ルール)""",
        "labels": ["domain", "configuration", "priority-medium"],
        "milestone": "M3: 拡張機能実装",
        "depends_on": [19]
    },
    {
        "title": "イベントシステムの実装",
        "body": """## 概要
ゲームイベントを記録するシステムを実装します。

## タスク内容
- [ ] `GameEvent`基底クラスの実装
- [ ] 各種イベントクラスの実装
  - `CardPlayedEvent`
  - `CardsCapturedEvent`
  - `YakuCompletedEvent`
  - `KoiKoiCalledEvent`
  - `GameEndedEvent`
- [ ] イベント発行機構
- [ ] イベントの収集と履歴管理
- [ ] イベントのテスト

## 成果物
- `HanafudaEngine.Domain/Events/`以下の全クラス
- イベントシステムのテスト

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-22-イベントシステムの実装)
- [実装仕様書](../docs/implementation-specification.md#54-gameevent)""",
        "labels": ["domain", "events", "priority-medium"],
        "milestone": "M3: 拡張機能実装",
        "depends_on": [10]
    },
    {
        "title": "フェーズ3統合テストとデバッグ",
        "body": """## 概要
拡張機能を含む統合テストを実施します。

## タスク内容
- [ ] 拡張機能を含む統合テスト
- [ ] カスタムルールでのゲームテスト
- [ ] イベントシステムの動作確認
- [ ] バグ修正
- [ ] ドキュメントの更新

## 成果物
- 拡張機能を含む完全版ルールエンジン
- 更新されたドキュメント

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-23-フェーズ3統合テストとデバッグ)""",
        "labels": ["testing", "priority-medium"],
        "milestone": "M3: 拡張機能実装",
        "depends_on": [20, 21, 22]
    },
    # フェーズ4: 高度な機能
    {
        "title": "パフォーマンス最適化",
        "body": """## 概要
ルールエンジンのパフォーマンスを最適化します。

## タスク内容
- [ ] パフォーマンスプロファイリング
- [ ] ボトルネックの特定
- [ ] 役判定の最適化
  - キャッシング機構の導入
  - LINQ vs ループの最適化
- [ ] 状態オブジェクトのメモリ最適化
- [ ] ベンチマークテストの作成
- [ ] 最適化効果の測定

## 成果物
- 最適化されたルールエンジン
- パフォーマンステスト

## 推定工数
8-10時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-24-パフォーマンス最適化)
- [実装仕様書](../docs/implementation-specification.md#14-パフォーマンス考慮事項)""",
        "labels": ["performance", "priority-low"],
        "milestone": "M4: リリース準備完了",
        "depends_on": [23]
    },
    {
        "title": "詳細なログ機能",
        "body": """## 概要
デバッグ用の詳細なログ機能を実装します。

## タスク内容
- [ ] ログフレームワークの統合（Serilogなど）
- [ ] 構造化ログの実装
- [ ] ログレベルの設定
  - デバッグ、情報、警告、エラー
- [ ] ゲームフローの詳細ログ
- [ ] デバッグ用のダンプ機能
- [ ] ログのテスト

## 成果物
- 詳細なログ機能
- ログテスト

## 推定工数
5-6時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-25-詳細なログ機能)""",
        "labels": ["logging", "priority-low"],
        "milestone": "M4: リリース準備完了",
        "depends_on": [22]
    },
    {
        "title": "リプレイ機能",
        "body": """## 概要
ゲームのリプレイ機能を実装します。

## タスク内容
- [ ] ゲーム履歴の記録
- [ ] ゲーム状態のスナップショット
- [ ] リプレイデータのシリアライゼーション
- [ ] リプレイの再生機能
- [ ] 早送り/巻き戻し機能
- [ ] リプレイのテスト

## 成果物
- リプレイ機能
- リプレイテスト

## 推定工数
8-10時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-26-リプレイ機能)""",
        "labels": ["replay", "priority-low"],
        "milestone": "M4: リリース準備完了",
        "depends_on": [22]
    },
    {
        "title": "フェーズ4統合テストと最終調整",
        "body": """## 概要
最終的な統合テストと調整を行います。

## タスク内容
- [ ] 全機能を含む最終統合テスト
- [ ] パフォーマンステストの実施
- [ ] コードレビューと改善
- [ ] ドキュメントの最終更新
  - APIリファレンス
  - 使用例
  - パフォーマンス特性
- [ ] リリースノートの作成

## 成果物
- 最終版ルールエンジン
- 完全なドキュメント

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-27-フェーズ4統合テストと最終調整)""",
        "labels": ["testing", "priority-low"],
        "milestone": "M4: リリース準備完了",
        "depends_on": [24, 25, 26]
    },
    # その他
    {
        "title": "エラーハンドリングの実装",
        "body": """## 概要
カスタム例外とエラーハンドリングを実装します。

## タスク内容
- [ ] カスタム例外クラスの実装
  - `GameRuleViolationException`
  - `InvalidActionException`
  - `InvalidGameStateException`
- [ ] エラーハンドリング方針の実装
  - プログラミングエラー → 例外
  - ゲームルール違反 → 例外
  - ユーザー入力エラー → Resultパターン
- [ ] エラーメッセージの国際化対応（オプション）
- [ ] エラーハンドリングのテスト

## 成果物
- カスタム例外クラス
- エラーハンドリングテスト

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-28-エラーハンドリングの実装)
- [実装仕様書](../docs/implementation-specification.md#13-エラーハンドリング)""",
        "labels": ["error-handling", "priority-medium"],
        "milestone": "M2: 完全ルール実装",
        "depends_on": [9]
    },
    {
        "title": "APIリファレンスの作成",
        "body": """## 概要
完全なAPIドキュメントを作成します。

## タスク内容
- [ ] XMLドキュメントコメントの追加
- [ ] DocFXまたはSandcastleを使用したAPIドキュメント生成
- [ ] サンプルコードの作成
- [ ] 使用例のドキュメント
- [ ] トラブルシューティングガイド

## 成果物
- APIリファレンスドキュメント
- サンプルコード集

## 推定工数
6-8時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-29-apiリファレンスの作成)""",
        "labels": ["documentation", "priority-medium"],
        "milestone": "M4: リリース準備完了",
        "depends_on": [19]
    },
    {
        "title": "CI/CDの設定",
        "body": """## 概要
継続的インテグレーション/デプロイメントを設定します。

## タスク内容
- [ ] GitHub Actionsワークフローの作成
- [ ] ビルドパイプラインの設定
- [ ] テスト自動実行の設定
- [ ] コードカバレッジの測定
- [ ] 静的解析ツールの統合
- [ ] 自動リリースフローの設定

## 成果物
- `.github/workflows/`以下の設定ファイル
- 自動化されたCI/CDパイプライン

## 推定工数
4-5時間

## 依存関係
{depends_on}

## 参考資料
- [実装計画書](../docs/implementation-plan.md#issue-30-cicdの設定)""",
        "labels": ["ci-cd", "priority-medium"],
        "milestone": "M1: MVP完成",
        "depends_on": [1]
    },
]


def get_milestone_number(milestone_name):
    """マイルストーン名からマイルストーン番号を取得"""
    url = f"{GITHUB_API_URL}/repos/{REPO_OWNER}/{REPO_NAME}/milestones"
    req = request.Request(url, headers={
        "Authorization": f"token {GITHUB_TOKEN}",
        "Accept": "application/vnd.github.v3+json"
    })
    
    try:
        with request.urlopen(req) as response:
            milestones = json.loads(response.read().decode())
            for ms in milestones:
                if ms["title"] == milestone_name:
                    return ms["number"]
    except error.HTTPError as e:
        print(f"エラー: マイルストーン取得失敗 - {e}")
    
    return None


def create_github_issue(title, body, labels, milestone_name):
    """GitHubにIssueを作成"""
    milestone_number = get_milestone_number(milestone_name)
    if not milestone_number:
        print(f"警告: マイルストーン '{milestone_name}' が見つかりません")
    
    data = {
        "title": title,
        "body": body,
        "labels": labels
    }
    
    if milestone_number:
        data["milestone"] = milestone_number
    
    url = f"{GITHUB_API_URL}/repos/{REPO_OWNER}/{REPO_NAME}/issues"
    req = request.Request(
        url,
        data=json.dumps(data).encode(),
        headers={
            "Authorization": f"token {GITHUB_TOKEN}",
            "Accept": "application/vnd.github.v3+json",
            "Content-Type": "application/json"
        },
        method="POST"
    )
    
    try:
        with request.urlopen(req) as response:
            result = json.loads(response.read().decode())
            return result["number"]
    except error.HTTPError as e:
        error_body = e.read().decode()
        print(f"エラー: Issue作成失敗 - {e}")
        print(f"詳細: {error_body}")
        return None


def main():
    """メイン処理"""
    if not GITHUB_TOKEN:
        print("❌ 環境変数 GITHUB_TOKEN が設定されていません")
        print("   Personal Access Token を作成し、環境変数に設定してください:")
        print("   export GITHUB_TOKEN=your_token_here")
        sys.exit(1)
    
    print("🚀 こいこいルールエンジン実装のIssue作成を開始します")
    print(f"📋 リポジトリ: {REPO_OWNER}/{REPO_NAME}")
    print(f"📝 作成するIssue数: {len(ISSUES)}")
    print()
    
    # 確認
    response = input("Issueを作成しますか？ (y/n): ")
    if response.lower() != 'y':
        print("キャンセルしました")
        sys.exit(0)
    
    print()
    print("📝 Issueの作成を開始します...")
    print()
    
    created_issues = {}
    
    for i, issue in enumerate(ISSUES, 1):
        print(f"[{i}/{len(ISSUES)}] 作成中: {issue['title']}")
        
        # 依存関係の解決
        depends_on_text = ""
        if issue["depends_on"]:
            depends_on_numbers = [created_issues.get(dep, dep) for dep in issue["depends_on"]]
            depends_on_text = "Depends on " + ", ".join([f"#{num}" for num in depends_on_numbers])
        
        # Issueボディの依存関係を置換
        body = issue["body"].replace("{depends_on}", depends_on_text)
        
        # Issue作成
        issue_number = create_github_issue(
            issue["title"],
            body,
            issue["labels"],
            issue["milestone"]
        )
        
        if issue_number:
            created_issues[i] = issue_number
            print(f"  ✅ Issue #{issue_number} を作成しました")
        else:
            print(f"  ❌ Issue作成に失敗しました")
        
        # API rate limitを考慮して待機
        time.sleep(2)
    
    print()
    print(f"✅ {len(created_issues)}/{len(ISSUES)} 個のIssueを作成しました！")
    print()
    print("📋 次のステップ:")
    print(f"  1. GitHubでIssueを確認: https://github.com/{REPO_OWNER}/{REPO_NAME}/issues")
    print("  2. 必要に応じてIssueを編集")
    print("  3. Issue #4に各IssueをSub Issueとして関連付け")
    print("  4. docs/QUICKSTART.mdを読んで実装を開始")
    print()


if __name__ == "__main__":
    main()
