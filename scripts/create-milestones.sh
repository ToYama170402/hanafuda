#!/bin/bash

# マイルストーン作成スクリプト
# このスクリプトは GitHub CLI (gh) を使用してマイルストーンを作成します
# 使用方法: ./scripts/create-milestones.sh [プロジェクト開始日 YYYY-MM-DD]

set -e

# リポジトリのオーナーとリポジトリ名を取得
REPO_OWNER="ToYama170402"
REPO_NAME="hanafuda"

# プロジェクト開始日（引数で指定、デフォルトは今日）
PROJECT_START_DATE="${1:-$(date +%Y-%m-%d)}"

echo "プロジェクト開始日: $PROJECT_START_DATE"
echo "マイルストーンを作成します..."
echo ""

# 日付計算関数（週を追加）
calculate_due_date() {
  local start_date=$1
  local weeks=$2
  
  if [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS
    date -j -v+${weeks}w -f "%Y-%m-%d" "$start_date" +%Y-%m-%d
  else
    # Linux
    date -d "$start_date + $weeks weeks" +%Y-%m-%d
  fi
}

# マイルストーン1: MVP完成
echo "作成中: M1: MVP完成"
DUE_DATE_1=$(calculate_due_date "$PROJECT_START_DATE" 3)
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  -H "X-GitHub-Api-Version: 2022-11-28" \
  /repos/${REPO_OWNER}/${REPO_NAME}/milestones \
  -f title='M1: MVP完成' \
  -f state='open' \
  -f description='基本的なゲームフローが動作する最小限の機能' \
  -f due_on="${DUE_DATE_1}T23:59:59Z" || echo "  エラー: M1の作成に失敗しました"

echo "  完了: M1 (期限: $DUE_DATE_1)"
echo ""

# マイルストーン2: 完全ルール実装
echo "作成中: M2: 完全ルール実装"
DUE_DATE_2=$(calculate_due_date "$PROJECT_START_DATE" 5)
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  -H "X-GitHub-Api-Version: 2022-11-28" \
  /repos/${REPO_OWNER}/${REPO_NAME}/milestones \
  -f title='M2: 完全ルール実装' \
  -f state='open' \
  -f description='こいこいを含む完全なゲームルールの実装' \
  -f due_on="${DUE_DATE_2}T23:59:59Z" || echo "  エラー: M2の作成に失敗しました"

echo "  完了: M2 (期限: $DUE_DATE_2)"
echo ""

# マイルストーン3: 拡張機能実装
echo "作成中: M3: 拡張機能実装"
DUE_DATE_3=$(calculate_due_date "$PROJECT_START_DATE" 6)
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  -H "X-GitHub-Api-Version: 2022-11-28" \
  /repos/${REPO_OWNER}/${REPO_NAME}/milestones \
  -f title='M3: 拡張機能実装' \
  -f state='open' \
  -f description='カスタマイズ機能やイベントシステムの実装' \
  -f due_on="${DUE_DATE_3}T23:59:59Z" || echo "  エラー: M3の作成に失敗しました"

echo "  完了: M3 (期限: $DUE_DATE_3)"
echo ""

# マイルストーン4: リリース準備完了
echo "作成中: M4: リリース準備完了"
DUE_DATE_4=$(calculate_due_date "$PROJECT_START_DATE" 8)
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  -H "X-GitHub-Api-Version: 2022-11-28" \
  /repos/${REPO_OWNER}/${REPO_NAME}/milestones \
  -f title='M4: リリース準備完了' \
  -f state='open' \
  -f description='最適化、ドキュメント、リプレイ機能の実装' \
  -f due_on="${DUE_DATE_4}T23:59:59Z" || echo "  エラー: M4の作成に失敗しました"

echo "  完了: M4 (期限: $DUE_DATE_4)"
echo ""

echo "すべてのマイルストーンの作成が完了しました！"
echo ""
echo "確認するには以下のコマンドを実行してください:"
echo "  gh api /repos/${REPO_OWNER}/${REPO_NAME}/milestones"
echo ""
echo "または GitHub Web UI で確認:"
echo "  https://github.com/${REPO_OWNER}/${REPO_NAME}/milestones"
