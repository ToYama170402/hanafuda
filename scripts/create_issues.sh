#!/bin/bash

# こいこいルールエンジン実装のための30個のIssueを作成するスクリプト
# 使用方法: ./scripts/create_issues.sh
# 前提条件: GitHub CLI (gh) がインストールされ、認証済みであること

set -e

REPO="ToYama170402/hanafuda"

echo "🚀 こいこいルールエンジン実装のIssue作成を開始します"
echo "📋 30個のIssueを作成します"
echo ""

# GitHub CLIがインストールされているか確認
if ! command -v gh &> /dev/null; then
    echo "❌ GitHub CLI (gh) がインストールされていません。"
    echo "   インストール方法: https://cli.github.com/"
    exit 1
fi

# 認証確認
if ! gh auth status &> /dev/null; then
    echo "❌ GitHub CLIが認証されていません。"
    echo "   実行してください: gh auth login"
    exit 1
fi

echo "✅ GitHub CLI が正しくセットアップされています"
echo ""

# 確認
read -p "Issueを作成しますか？ (y/n): " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]
then
    echo "キャンセルしました"
    exit 0
fi

echo ""
echo "📝 Issueの作成を開始します..."
echo ""

# Issue作成関数
create_issue() {
    local num=$1
    local title="$2"
    local body="$3"
    local labels="$4"
    local milestone="$5"
    
    echo "[$num/30] 作成中: $title"
    
    # Issue作成
    gh issue create \
        --repo "$REPO" \
        --title "$title" \
        --body "$body" \
        --label "$labels" \
        --milestone "$milestone" 2>&1 | grep -oP 'https://github.com/[^/]+/[^/]+/issues/\K\d+' || echo "?"
    
    # API rate limitを考慮して少し待機
    sleep 1
}

# Issue番号を保存する配列
declare -a ISSUE_NUMBERS

# フェーズ1: 基本機能（MVP）

ISSUE_NUMBERS[1]=$(create_issue 1 \
    "プロジェクト構造とソリューションのセットアップ" \
    "## 概要
プロジェクトの基本構造とソリューションファイルをセットアップします。

## タスク内容
- [ ] .NET Coreソリューションファイルの作成
- [ ] 以下のプロジェクトを作成
  - \`HanafudaEngine.Core\` (クラスライブラリ)
  - \`HanafudaEngine.Domain\` (クラスライブラリ)
  - \`HanafudaEngine.Facade\` (クラスライブラリ)
  - \`HanafudaEngine.Tests\` (xUnitテストプロジェクト)
- [ ] プロジェクト間の参照関係を設定
- [ ] 基本的な名前空間構造を確立
- [ ] .gitignoreファイルの作成（bin/, obj/, .vs/など）
- [ ] READMEの更新（ビルド手順の追加）

## 成果物
- \`HanafudaEngine.sln\`
- 各プロジェクトの\`.csproj\`ファイル
- 更新された\`.gitignore\`

## 推定工数
2-3時間

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [クイックスタートガイド](../docs/QUICKSTART.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "setup,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[1]} を作成しました"

ISSUE_NUMBERS[2]=$(create_issue 2 \
    "Core層の基本データモデル実装" \
    "## 概要
Core層の基本的なデータモデル（値オブジェクト）を実装します。

## タスク内容
- [ ] \`Month\`列挙型の実装
- [ ] \`CardType\`列挙型の実装
- [ ] \`SpecialCardFlag\`列挙型の実装
- [ ] \`Card\`クラスの実装（等値比較含む）
- [ ] \`PlayerId\`列挙型の実装
- [ ] \`YakuType\`列挙型の実装
- [ ] \`Yaku\`クラスの実装
- [ ] 基本的なユニットテストの作成

## 成果物
- \`HanafudaEngine.Core/Models/\`以下の全クラス
- 対応するユニットテスト

## 推定工数
4-6時間

## 依存関係
Depends on #${ISSUE_NUMBERS[1]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "core,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[2]} を作成しました"

ISSUE_NUMBERS[3]=$(create_issue 3 \
    "札の定義と管理" \
    "## 概要
48枚の花札を定義し、札の管理機能を実装します。

## タスク内容
- [ ] \`CardDefinitions\`クラスの実装
- [ ] 48枚の花札を定義（実装仕様書2.2に基づく）
- [ ] 月別、種類別の札取得メソッドの実装
- [ ] 特殊札フラグの設定（赤短、青短、盃、鹿、猪、蝶、雨札）
- [ ] 札定義の整合性テスト
  - 合計48枚の確認
  - 各月4枚の確認
  - 光札5枚、種札9枚、短冊札10枚、カス札24枚の確認

## 成果物
- \`HanafudaEngine.Core/Models/CardDefinitions.cs\`
- 札定義の検証テスト

## 推定工数
6-8時間

## 依存関係
Depends on #${ISSUE_NUMBERS[2]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)" \
    "core,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[3]} を作成しました"

ISSUE_NUMBERS[4]=$(create_issue 4 \
    "ゲーム状態管理クラスの実装" \
    "## 概要
ゲームの状態を管理するクラスを実装します。

## タスク内容
- [ ] \`GamePhase\`列挙型の実装
- [ ] \`TurnPhase\`列挙型の実装
- [ ] \`PlayerState\`クラスの実装（不変性を保つWithメソッド含む）
- [ ] \`GameState\`クラスの実装（不変性を保つWithメソッド含む）
- [ ] \`GameResult\`クラスの実装
- [ ] 状態遷移のユニットテスト

## 成果物
- \`HanafudaEngine.Domain/\`以下の状態管理クラス
- 状態遷移テスト

## 推定工数
6-8時間

## 依存関係
Depends on #${ISSUE_NUMBERS[2]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[4]} を作成しました"

ISSUE_NUMBERS[5]=$(create_issue 5 \
    "山札管理とシャッフル機能" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[3]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)" \
    "domain,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[5]} を作成しました"

ISSUE_NUMBERS[6]=$(create_issue 6 \
    "基本的な役判定の実装（光札、種、短冊、カス）" \
    "## 概要
基本的な役の判定ロジックを実装します。

## タスク内容
- [ ] \`YakuEvaluator\`クラスの基本構造
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
- \`HanafudaEngine.Domain/Services/YakuEvaluator.cs\`
- 役判定の正常系・異常系テスト

## 推定工数
8-10時間

## 依存関係
Depends on #${ISSUE_NUMBERS[3]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,yaku,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[6]} を作成しました"

ISSUE_NUMBERS[7]=$(create_issue 7 \
    "得点計算機能の実装" \
    "## 概要
役の得点を計算する機能を実装します。

## タスク内容
- [ ] \`ScoreCalculator\`クラスの実装
- [ ] 基本得点の計算
- [ ] ボーナス得点の計算（種、短冊、カスの枚数超過分）
- [ ] こいこいボーナス（2倍）の処理
- [ ] こいこい失敗時の得点処理（0点）
- [ ] \`CalculateFinalScore\`メソッドの実装
- [ ] \`CalculateGameResult\`メソッドの実装
- [ ] 得点計算の各種パターンテスト

## 成果物
- \`HanafudaEngine.Domain/Services/ScoreCalculator.cs\`
- 得点計算テスト

## 推定工数
4-5時間

## 依存関係
Depends on #${ISSUE_NUMBERS[6]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[7]} を作成しました"

ISSUE_NUMBERS[8]=$(create_issue 8 \
    "ターン進行の基本ロジック" \
    "## 概要
ターンの進行を管理するロジックを実装します。

## タスク内容
- [ ] \`TurnManager\`クラスの実装
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
- \`HanafudaEngine.Domain/Services/TurnManager.cs\`
- ターン進行テスト

## 推定工数
8-10時間

## 依存関係
Depends on #${ISSUE_NUMBERS[4]}, #${ISSUE_NUMBERS[5]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[8]} を作成しました"

ISSUE_NUMBERS[9]=$(create_issue 9 \
    "アクション定義とバリデーション" \
    "## 概要
プレイヤーのアクションを定義し、バリデーション機能を実装します。

## タスク内容
- [ ] \`IGameAction\`インターフェースの定義
- [ ] \`ActionType\`列挙型の定義
- [ ] 各アクションクラスの実装
  - \`PlayCardAction\`
  - \`SelectFieldCardAction\`
  - \`CallKoiKoiAction\`
  - \`CallShobuAction\`
- [ ] \`ActionValidator\`クラスの実装
- [ ] \`ValidationResult\`クラスの実装
- [ ] 各アクションの妥当性検証ロジック
- [ ] バリデーションのテスト

## 成果物
- \`HanafudaEngine.Domain/Actions/\`以下の全クラス
- \`HanafudaEngine.Domain/Services/ActionValidator.cs\`
- バリデーションテスト

## 推定工数
6-8時間

## 依存関係
Depends on #${ISSUE_NUMBERS[4]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[9]} を作成しました"

ISSUE_NUMBERS[10]=$(create_issue 10 \
    "ゲームファサードの実装" \
    "## 概要
ゲーム全体を統括するファサードクラスを実装します。

## タスク内容
- [ ] \`KoiKoiGame\`クラスの基本構造
- [ ] ゲーム初期化処理
- [ ] \`GetState()\`メソッドの実装
- [ ] \`GetAvailableActions()\`メソッドの実装
- [ ] \`ExecuteAction()\`メソッドの実装
- [ ] \`GameActionResult\`クラスの実装
- [ ] ゲームリセット機能
- [ ] 基本的なゲームフローの統合テスト

## 成果物
- \`HanafudaEngine.Facade/KoiKoiGame.cs\`
- \`HanafudaEngine.Facade/GameActionResult.cs\`
- 統合テスト

## 推定工数
8-10時間

## 依存関係
Depends on #${ISSUE_NUMBERS[4]}, #${ISSUE_NUMBERS[8]}, #${ISSUE_NUMBERS[9]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "facade,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[10]} を作成しました"

ISSUE_NUMBERS[11]=$(create_issue 11 \
    "配札検証と勝敗判定" \
    "## 概要
配札の妥当性検証と勝敗判定のロジックを実装します。

## タスク内容
- [ ] \`DealingValidator\`クラスの実装
- [ ] 場に同月4枚の検証
- [ ] 再配札処理
- [ ] 勝敗判定ロジック
  - 役完成時に「勝負」を宣言した場合
  - 8ターン経過で総流れ（引き分け）
- [ ] 親の交代処理
- [ ] 勝敗判定のテスト

## 成果物
- \`HanafudaEngine.Domain/Services/DealingValidator.cs\`
- 勝敗判定ロジック
- 配札・勝敗のテスト

## 推定工数
4-5時間

## 依存関係
Depends on #${ISSUE_NUMBERS[5]}, #${ISSUE_NUMBERS[10]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[11]} を作成しました"

ISSUE_NUMBERS[12]=$(create_issue 12 \
    "MVP統合テストとデバッグ" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[11]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "testing,priority-high" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[12]} を作成しました"

# フェーズ2: 完全なルール実装

ISSUE_NUMBERS[13]=$(create_issue 13 \
    "こいこいの宣言と処理" \
    "## 概要
こいこいの宣言と処理ロジックを実装します。

## タスク内容
- [ ] こいこい/勝負の選択フェーズの実装
- [ ] \`KoiKoiDecision\`フェーズの状態管理
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
Depends on #${ISSUE_NUMBERS[12]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)" \
    "domain,koikoi,priority-high" \
    "M2: 完全ルール実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[13]} を作成しました"

ISSUE_NUMBERS[14]=$(create_issue 14 \
    "喰い付き（くいつき）の実装" \
    "## 概要
喰い付きの判定と処理ロジックを実装します。

## タスク内容
- [ ] \`KuitsukiHandler\`クラスの実装
- [ ] 喰い付き判定ロジック
  - 手札、場札、山札で同月3枚
- [ ] 喰い付き時の4枚取得処理
- [ ] 喰い付きのテスト

## 成果物
- \`HanafudaEngine.Domain/Services/KuitsukiHandler.cs\`
- 喰い付きテスト

## 推定工数
4-5時間

## 依存関係
Depends on #${ISSUE_NUMBERS[12]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,special-rule,priority-high" \
    "M2: 完全ルール実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[14]} を作成しました"

ISSUE_NUMBERS[15]=$(create_issue 15 \
    "特殊な役の実装（猪鹿蝶、花見で一杯、月見で一杯）" \
    "## 概要
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
- \`YakuEvaluator\`への追加実装
- 特殊役テスト

## 推定工数
4-5時間

## 依存関係
Depends on #${ISSUE_NUMBERS[6]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)" \
    "domain,yaku,priority-medium" \
    "M2: 完全ルール実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[15]} を作成しました"

ISSUE_NUMBERS[16]=$(create_issue 16 \
    "短冊の特殊役（赤短、青短）の実装" \
    "## 概要
赤短と青短の役を実装します。

## タスク内容
- [ ] 赤短の判定ロジック
  - 松・梅・桜の赤短3枚
- [ ] 青短の判定ロジック
  - 牡丹・菊・紅葉の青短3枚
- [ ] 赤短/青短と「短冊」の重複処理
- [ ] 特殊短冊役のテスト

## 成果物
- \`YakuEvaluator\`への追加実装
- 特殊短冊役テスト

## 推定工数
4-5時間

## 依存関係
Depends on #${ISSUE_NUMBERS[6]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)" \
    "domain,yaku,priority-medium" \
    "M2: 完全ルール実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[16]} を作成しました"

ISSUE_NUMBERS[17]=$(create_issue 17 \
    "総流れ（そうながれ）の実装" \
    "## 概要
8ターン経過時の総流れ（引き分け）処理を実装します。

## タスク内容
- [ ] \`DrawHandler\`クラスの実装
- [ ] 8ターン経過の判定
- [ ] 総流れ時の処理
  - 引き分け判定
  - 親の続投
- [ ] 総流れのテスト

## 成果物
- \`HanafudaEngine.Domain/Services/DrawHandler.cs\`
- 総流れテスト

## 推定工数
3-4時間

## 依存関係
Depends on #${ISSUE_NUMBERS[11]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,special-rule,priority-medium" \
    "M2: 完全ルール実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[17]} を作成しました"

ISSUE_NUMBERS[18]=$(create_issue 18 \
    "配札の再検証機能" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[11]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)" \
    "domain,priority-medium" \
    "M2: 完全ルール実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[18]} を作成しました"

ISSUE_NUMBERS[19]=$(create_issue 19 \
    "フェーズ2統合テストとデバッグ" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[13]}, #${ISSUE_NUMBERS[14]}, #${ISSUE_NUMBERS[15]}, #${ISSUE_NUMBERS[16]}, #${ISSUE_NUMBERS[17]}, #${ISSUE_NUMBERS[18]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)" \
    "testing,priority-high" \
    "M2: 完全ルール実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[19]} を作成しました"

# フェーズ3: 拡張機能

ISSUE_NUMBERS[20]=$(create_issue 20 \
    "手四（てし）の実装（オプション）" \
    "## 概要
手四による即座の勝利処理を実装します（オプション機能）。

## タスク内容
- [ ] \`TeshiHandler\`クラスの実装
- [ ] 手四判定ロジック
  - 配られた手札に同月4枚
- [ ] 手四による即座の勝利処理
- [ ] 手四ルールの有効/無効切り替え
- [ ] 手四のテスト

## 成果物
- \`HanafudaEngine.Domain/Services/TeshiHandler.cs\`
- 手四テスト

## 推定工数
4-5時間

## 依存関係
Depends on #${ISSUE_NUMBERS[19]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [ゲームルール仕様書](../docs/hanafuda-specification.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,special-rule,optional,priority-low" \
    "M3: 拡張機能実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[20]} を作成しました"

ISSUE_NUMBERS[21]=$(create_issue 21 \
    "ルール設定のカスタマイズ機能" \
    "## 概要
ゲームルールをカスタマイズできる機能を実装します。

## タスク内容
- [ ] \`GameRules\`クラスの実装
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
- \`HanafudaEngine.Domain/GameRules.cs\`
- ルールカスタマイズテスト

## 推定工数
5-6時間

## 依存関係
Depends on #${ISSUE_NUMBERS[19]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,configuration,priority-medium" \
    "M3: 拡張機能実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[21]} を作成しました"

ISSUE_NUMBERS[22]=$(create_issue 22 \
    "イベントシステムの実装" \
    "## 概要
ゲームイベントを記録するシステムを実装します。

## タスク内容
- [ ] \`GameEvent\`基底クラスの実装
- [ ] 各種イベントクラスの実装
  - \`CardPlayedEvent\`
  - \`CardsCapturedEvent\`
  - \`YakuCompletedEvent\`
  - \`KoiKoiCalledEvent\`
  - \`GameEndedEvent\`
- [ ] イベント発行機構
- [ ] イベントの収集と履歴管理
- [ ] イベントのテスト

## 成果物
- \`HanafudaEngine.Domain/Events/\`以下の全クラス
- イベントシステムのテスト

## 推定工数
6-8時間

## 依存関係
Depends on #${ISSUE_NUMBERS[10]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "domain,events,priority-medium" \
    "M3: 拡張機能実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[22]} を作成しました"

ISSUE_NUMBERS[23]=$(create_issue 23 \
    "フェーズ3統合テストとデバッグ" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[20]}, #${ISSUE_NUMBERS[21]}, #${ISSUE_NUMBERS[22]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)" \
    "testing,priority-medium" \
    "M3: 拡張機能実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[23]} を作成しました"

# フェーズ4: 高度な機能

ISSUE_NUMBERS[24]=$(create_issue 24 \
    "パフォーマンス最適化" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[23]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "performance,priority-low" \
    "M4: リリース準備完了")

echo "  ✅ Issue #${ISSUE_NUMBERS[24]} を作成しました"

ISSUE_NUMBERS[25]=$(create_issue 25 \
    "詳細なログ機能" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[22]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)" \
    "logging,priority-low" \
    "M4: リリース準備完了")

echo "  ✅ Issue #${ISSUE_NUMBERS[25]} を作成しました"

ISSUE_NUMBERS[26]=$(create_issue 26 \
    "リプレイ機能" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[22]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)" \
    "replay,priority-low" \
    "M4: リリース準備完了")

echo "  ✅ Issue #${ISSUE_NUMBERS[26]} を作成しました"

ISSUE_NUMBERS[27]=$(create_issue 27 \
    "フェーズ4統合テストと最終調整" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[24]}, #${ISSUE_NUMBERS[25]}, #${ISSUE_NUMBERS[26]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)" \
    "testing,priority-low" \
    "M4: リリース準備完了")

echo "  ✅ Issue #${ISSUE_NUMBERS[27]} を作成しました"

# その他

ISSUE_NUMBERS[28]=$(create_issue 28 \
    "エラーハンドリングの実装" \
    "## 概要
カスタム例外とエラーハンドリングを実装します。

## タスク内容
- [ ] カスタム例外クラスの実装
  - \`GameRuleViolationException\`
  - \`InvalidActionException\`
  - \`InvalidGameStateException\`
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
Depends on #${ISSUE_NUMBERS[9]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)
- [実装仕様書](../docs/implementation-specification.md)" \
    "error-handling,priority-medium" \
    "M2: 完全ルール実装")

echo "  ✅ Issue #${ISSUE_NUMBERS[28]} を作成しました"

ISSUE_NUMBERS[29]=$(create_issue 29 \
    "APIリファレンスの作成" \
    "## 概要
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
Depends on #${ISSUE_NUMBERS[19]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)" \
    "documentation,priority-medium" \
    "M4: リリース準備完了")

echo "  ✅ Issue #${ISSUE_NUMBERS[29]} を作成しました"

ISSUE_NUMBERS[30]=$(create_issue 30 \
    "CI/CDの設定" \
    "## 概要
継続的インテグレーション/デプロイメントを設定します。

## タスク内容
- [ ] GitHub Actionsワークフローの作成
- [ ] ビルドパイプラインの設定
- [ ] テスト自動実行の設定
- [ ] コードカバレッジの測定
- [ ] 静的解析ツールの統合
- [ ] 自動リリースフローの設定

## 成果物
- \`.github/workflows/\`以下の設定ファイル
- 自動化されたCI/CDパイプライン

## 推定工数
4-5時間

## 依存関係
Depends on #${ISSUE_NUMBERS[1]}

## 参考資料
- [実装計画書](../docs/implementation-plan.md)" \
    "ci-cd,priority-medium" \
    "M1: MVP完成")

echo "  ✅ Issue #${ISSUE_NUMBERS[30]} を作成しました"

echo ""
echo "✅ 30個のIssueの作成が完了しました！"
echo ""
echo "📋 作成されたIssue番号:"
for i in {1..30}; do
    echo "  Issue ${i}: #${ISSUE_NUMBERS[$i]}"
done
echo ""
echo "📋 次のステップ:"
echo "  1. GitHubでIssueを確認: https://github.com/${REPO}/issues"
echo "  2. 必要に応じてIssueを編集"
echo "  3. Issue #4に各IssueをSub Issueとして関連付け"
echo "  4. docs/QUICKSTART.mdを読んで実装を開始"
echo ""
