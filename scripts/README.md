# Issue作成スクリプト

このディレクトリには、GitHub Issueを一括作成するためのスクリプトが含まれています。

## 📋 スクリプト

### `create_issues.py`

30個の実装Issueを一括作成するPythonスクリプトです。

## 🚀 使用方法

### 1. 前提条件

以下が完了していることを確認してください：

- Python 3.6以上がインストールされていること
- GitHub Personal Access Tokenを取得していること
- マイルストーンが作成されていること（M1, M2, M3, M4）
- ラベルが作成されていること（詳細は `docs/GITHUB-SETUP-CHECKLIST.md` を参照）

### 2. GitHub Personal Access Tokenの作成

1. GitHubの設定ページへ: https://github.com/settings/tokens
2. "Generate new token" → "Generate new token (classic)" をクリック
3. トークン名を入力（例: "Hanafuda Issues"）
4. スコープで **`repo`** を選択（すべてのrepo権限が必要）
5. "Generate token" をクリック
6. 表示されたトークンをコピー（後で見れないので注意！）

### 3. 環境変数の設定

```bash
export GITHUB_TOKEN=your_personal_access_token_here
```

Windowsの場合:
```powershell
set GITHUB_TOKEN=your_personal_access_token_here
```

### 4. スクリプトの実行

```bash
# リポジトリのルートディレクトリで実行
python scripts/create_issues.py
```

または:

```bash
# scriptsディレクトリ内で実行
cd scripts
python create_issues.py
```

### 5. 実行結果の確認

スクリプトは以下を実行します：

1. ✅ 環境変数のチェック
2. 📋 マイルストーンの取得
3. 📝 30個のIssueを順次作成
4. 🔗 依存関係（Depends on #X）を自動設定

成功すると、各Issueの番号が表示されます：

```
[1/30] 作成中: プロジェクト構造とソリューションのセットアップ
  ✅ Issue #5 を作成しました
[2/30] 作成中: Core層の基本データモデル実装
  ✅ Issue #6 を作成しました
...
```

## ⚠️ トラブルシューティング

### エラー: `GITHUB_TOKEN が設定されていません`

環境変数が設定されていません。上記の手順3を実行してください。

### エラー: `マイルストーンが見つかりません`

マイルストーンが作成されていません。`docs/GITHUB-SETUP-CHECKLIST.md` を参照して、以下のマイルストーンを作成してください：

- M1: MVP完成
- M2: 完全ルール実装
- M3: 拡張機能実装
- M4: リリース準備完了

### エラー: `Issue作成失敗`

考えられる原因：

1. **トークンの権限不足**: `repo`スコープが付与されていることを確認
2. **API rate limit**: 少し待ってから再実行
3. **ラベルが存在しない**: 必要なラベルを作成（詳細は `docs/GITHUB-SETUP-CHECKLIST.md`）
4. **ネットワークエラー**: インターネット接続を確認

## 📝 スクリプトの特徴

- ✅ 30個のIssueを自動作成
- ✅ 依存関係を自動設定（Depends on #X）
- ✅ マイルストーンを自動割り当て
- ✅ ラベルを自動割り当て
- ✅ API rate limitを考慮した待機時間
- ✅ エラーハンドリング

## 🔧 カスタマイズ

スクリプトの設定を変更する場合は、`create_issues.py` の先頭部分を編集してください：

```python
# 設定
REPO_OWNER = "ToYama170402"  # リポジトリオーナー
REPO_NAME = "hanafuda"        # リポジトリ名
```

## 📚 参考資料

- [実装計画書](../docs/implementation-plan.md) - 各Issueの詳細な説明
- [GitHubセットアップチェックリスト](../docs/GITHUB-SETUP-CHECKLIST.md) - 事前準備の詳細
- [クイックスタートガイド](../docs/QUICKSTART.md) - 実装開始ガイド

## 💡 ヒント

### 実行前にドライラン

スクリプトを実行する前に、作成されるIssueの内容を確認したい場合は、スクリプト内の `create_github_issue` 関数の呼び出しをコメントアウトして、print文だけを実行することもできます。

### 一部のIssueだけ作成

特定のIssueだけを作成したい場合は、`ISSUES` リストから不要な要素を削除するか、リストのスライシングを使用してください：

```python
# 最初の5つのIssueだけ作成
for i, issue in enumerate(ISSUES[:5], 1):
    ...
```

---

**作成日**: 2025-11-17  
**バージョン**: 1.0
