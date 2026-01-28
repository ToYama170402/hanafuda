# マイルストーン作成ガイド

このガイドは、`GITHUB-SETUP-CHECKLIST.md` に記載されている4つのマイルストーンを GitHub に作成する方法を説明します。

## 📋 作成するマイルストーン

| # | タイトル | 期限（プロジェクト開始から） | 説明 |
|---|---------|--------------------------|------|
| 1 | M1: MVP完成 | 3週間後 | 基本的なゲームフローが動作する最小限の機能 |
| 2 | M2: 完全ルール実装 | 5週間後 | こいこいを含む完全なゲームルールの実装 |
| 3 | M3: 拡張機能実装 | 6週間後 | カスタマイズ機能やイベントシステムの実装 |
| 4 | M4: リリース準備完了 | 8週間後 | 最適化、ドキュメント、リプレイ機能の実装 |

## 🚀 方法1: スクリプトを使用した自動作成（推奨）

### 前提条件
- GitHub CLI (`gh`) がインストールされていること
- GitHub にログインしていること (`gh auth login`)
- リポジトリへの書き込み権限があること

### 実行方法

```bash
# スクリプトに実行権限を付与
chmod +x scripts/create-milestones.sh

# プロジェクト開始日を今日として実行
./scripts/create-milestones.sh

# または、プロジェクト開始日を指定して実行
./scripts/create-milestones.sh 2026-01-28
```

### スクリプトの動作

スクリプトは以下を自動的に実行します：
1. プロジェクト開始日から各マイルストーンの期限を計算
2. GitHub API を使用してマイルストーンを作成
3. 作成結果を表示

## 🖱️ 方法2: GitHub Web UI から手動作成

### 手順

1. GitHub リポジトリにアクセス: https://github.com/ToYama170402/hanafuda
2. 「Issues」タブをクリック
3. 「Milestones」をクリック
4. 「New milestone」ボタンをクリック
5. 以下の情報を入力して各マイルストーンを作成:

#### M1: MVP完成
- **Title**: `M1: MVP完成`
- **Due date**: プロジェクト開始から3週間後の日付
- **Description**: 
  ```
  基本的なゲームフローが動作する最小限の機能
  ```

#### M2: 完全ルール実装
- **Title**: `M2: 完全ルール実装`
- **Due date**: プロジェクト開始から5週間後の日付
- **Description**: 
  ```
  こいこいを含む完全なゲームルールの実装
  ```

#### M3: 拡張機能実装
- **Title**: `M3: 拡張機能実装`
- **Due date**: プロジェクト開始から6週間後の日付
- **Description**: 
  ```
  カスタマイズ機能やイベントシステムの実装
  ```

#### M4: リリース準備完了
- **Title**: `M4: リリース準備完了`
- **Due date**: プロジェクト開始から8週間後の日付
- **Description**: 
  ```
  最適化、ドキュメント、リプレイ機能の実装
  ```

6. 各マイルストーンの「Create milestone」ボタンをクリック

## 🔍 方法3: GitHub CLI で手動作成

GitHub CLI を使用して個別にマイルストーンを作成することもできます：

```bash
# プロジェクト開始日を設定（例: 2026-01-28）
START_DATE="2026-01-28"

# M1: MVP完成（3週間後）
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  /repos/ToYama170402/hanafuda/milestones \
  -f title='M1: MVP完成' \
  -f state='open' \
  -f description='基本的なゲームフローが動作する最小限の機能' \
  -f due_on='2026-02-18T23:59:59Z'

# M2: 完全ルール実装（5週間後）
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  /repos/ToYama170402/hanafuda/milestones \
  -f title='M2: 完全ルール実装' \
  -f state='open' \
  -f description='こいこいを含む完全なゲームルールの実装' \
  -f due_on='2026-03-04T23:59:59Z'

# M3: 拡張機能実装（6週間後）
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  /repos/ToYama170402/hanafuda/milestones \
  -f title='M3: 拡張機能実装' \
  -f state='open' \
  -f description='カスタマイズ機能やイベントシステムの実装' \
  -f due_on='2026-03-11T23:59:59Z'

# M4: リリース準備完了（8週間後）
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  /repos/ToYama170402/hanafuda/milestones \
  -f title='M4: リリース準備完了' \
  -f state='open' \
  -f description='最適化、ドキュメント、リプレイ機能の実装' \
  -f due_on='2026-03-25T23:59:59Z'
```

**注意**: `due_on` の日付は、プロジェクト開始日に応じて適切に計算してください。

## ✅ 確認方法

マイルストーンが正しく作成されたか確認するには：

### GitHub CLI を使用
```bash
gh api /repos/ToYama170402/hanafuda/milestones
```

### GitHub Web UI を使用
https://github.com/ToYama170402/hanafuda/milestones にアクセス

## 📚 関連ドキュメント

- [GITHUB-SETUP-CHECKLIST.md](GITHUB-SETUP-CHECKLIST.md) - 完全なセットアップチェックリスト
- [milestones.json](milestones.json) - マイルストーンデータ（JSON形式）
- [issues-summary.md](issues-summary.md) - Issue作成のための詳細情報

## 💡 ヒント

- プロジェクト開始日は、実際にプロジェクトを開始する日付に合わせてください
- マイルストーンの期限は後から変更可能です
- 各マイルストーンには、関連する Issues を紐付けることができます

## ❓ トラブルシューティング

### 「Permission denied」エラーが発生する場合
- GitHub にログインしているか確認: `gh auth status`
- リポジトリへの書き込み権限があるか確認

### 日付の計算がうまくいかない場合
- 手動で日付を計算して、GitHub CLI のコマンドを直接実行してください
- または、GitHub Web UI を使用してください

### スクリプトが実行できない場合
- 実行権限を確認: `ls -l scripts/create-milestones.sh`
- 実行権限を付与: `chmod +x scripts/create-milestones.sh`
