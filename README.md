# hanafuda

[![CI](https://github.com/ToYama170402/hanafuda/actions/workflows/ci.yml/badge.svg)](https://github.com/ToYama170402/hanafuda/actions/workflows/ci.yml)
[![CodeQL](https://github.com/ToYama170402/hanafuda/actions/workflows/codeql.yml/badge.svg)](https://github.com/ToYama170402/hanafuda/actions/workflows/codeql.yml)
[![Code Quality](https://github.com/ToYama170402/hanafuda/actions/workflows/code-quality.yml/badge.svg)](https://github.com/ToYama170402/hanafuda/actions/workflows/code-quality.yml)
[![codecov](https://codecov.io/gh/ToYama170402/hanafuda/branch/main/graph/badge.svg)](https://codecov.io/gh/ToYama170402/hanafuda)

花札のゲームルールを完全AI駆動開発する。

## ドキュメント

### 仕様書
- [こいこい仕様書](docs/hanafuda-specification.md) - こいこいのルール、札の構成、役、ゲーム進行方法の完全な仕様書
- [実装仕様書](docs/implementation-specification.md) - C#ルールエンジンの技術仕様書（データモデル、API設計、実装ガイド）

### 実装計画
- [🚀 クイックスタートガイド](docs/QUICKSTART.md) - すぐに実装を始めたい方向け（最初に読む）
- [✅ GitHubセットアップチェックリスト](docs/GITHUB-SETUP-CHECKLIST.md) - Issue作成前の準備作業
- [実装ロードマップ](docs/implementation-roadmap.md) - ビジュアルで分かりやすい実装の流れ
- [実装計画書](docs/implementation-plan.md) - 仕様書に基づいた段階的な実装計画（30個のIssueに分解）
- [Issue作成ガイド](docs/issues-summary.md) - GitHub Issueを作成するためのガイド
- [仕様書と実装の対応表](docs/specification-mapping.md) - 仕様書の各セクションと実装計画の対応関係

## プロジェクトの進め方

1. **仕様書の確認** - ゲームルールと技術仕様を理解する
2. **実装計画の確認** - 30個のIssueに分解された実装計画を確認する
3. **GitHub準備** - [セットアップチェックリスト](docs/GITHUB-SETUP-CHECKLIST.md)に従ってIssueを作成
4. **段階的な実装** - 4つのフェーズで順次実装を進める
   - フェーズ1: 基本機能（MVP）
   - フェーズ2: 完全なルール実装
   - フェーズ3: 拡張機能
   - フェーズ4: 高度な機能

## プロジェクト構造

```
HanafudaEngine/
├── src/
│   ├── HanafudaEngine.Core/       # 基本的なドメインモデル（値オブジェクト）
│   ├── HanafudaEngine.Domain/     # ゲームロジックとルールエンジン
│   └── HanafudaEngine.Facade/     # ゲーム全体の制御とインターフェース
├── tests/
│   └── HanafudaEngine.Tests/      # xUnitテストプロジェクト
├── docs/                          # ドキュメント
└── HanafudaEngine.sln            # ソリューションファイル
```

## ビルド方法

### 前提条件
- .NET 10.0 SDK以降

### ビルド手順

```bash
# リポジトリのクローン
git clone https://github.com/ToYama170402/hanafuda.git
cd hanafuda

# ソリューションのビルド
dotnet build HanafudaEngine.sln

# テストの実行
dotnet test HanafudaEngine.sln

# クリーンビルド
dotnet clean HanafudaEngine.sln
dotnet build HanafudaEngine.sln

# コードカバレッジ付きでテストを実行
dotnet test HanafudaEngine.sln --collect:"XPlat Code Coverage"

# コードフォーマットのチェック
dotnet format HanafudaEngine.sln --verify-no-changes
```

## CI/CD パイプライン

このプロジェクトは GitHub Actions を使用した包括的な CI/CD パイプラインを備えています。

### ワークフロー

#### 1. CI (継続的インテグレーション)
- **トリガー**: `main` と `develop` ブランチへのプッシュ、プルリクエスト
- **処理内容**:
  - .NET 10.0 環境のセットアップ
  - 依存関係の復元
  - ソリューションのビルド (Release構成)
  - 単体テストの実行
  - コードカバレッジの測定と報告
  - テスト結果とカバレッジレポートのアップロード

#### 2. CodeQL セキュリティ分析
- **トリガー**: `main` と `develop` ブランチへのプッシュ、プルリクエスト、週次スケジュール
- **処理内容**:
  - C# コードのセキュリティ脆弱性スキャン
  - セキュリティと品質の拡張クエリ実行
  - 脆弱性の自動検出と報告

#### 3. コード品質チェック
- **トリガー**: `main` と `develop` ブランチへのプッシュ、プルリクエスト
- **処理内容**:
  - `dotnet format` によるコードフォーマットチェック
  - .NET アナライザーによる静的解析
  - 脆弱なパッケージのスキャン
  - 古くなったパッケージの確認

#### 4. リリース
- **トリガー**:
  - `v*` タグのプッシュ（完全なリリース処理）
  - 手動実行（`workflow_dispatch`、ビルド／テスト／パッケージングとアーティファクトアップロードのみ）
- **処理内容**:
  - バージョン番号の自動取得（タグ付き実行時）または手動入力
  - Release 構成でのビルドとテスト
  - NuGet パッケージとシンボルパッケージの作成
  - NuGet.org および GitHub Packages への公開（タグ付き実行時のみ）
  - GitHub Release の自動作成（タグ付き実行時のみ）
  - アーティファクトのアップロード（全実行時）

#### 5. Dependabot
- **トリガー**: 週次スケジュール (月曜日)
- **処理内容**:
  - NuGet パッケージの自動更新チェック
  - GitHub Actions の自動更新チェック
  - 自動プルリクエストの作成

### 開発ワークフロー

1. **開発時**: コードを変更してプッシュすると、自動的に CI が実行されます
2. **プルリクエスト**: PR を作成すると、すべてのチェックが自動実行されます
3. **リリース**: `v1.0.0` のようなタグを作成すると、自動的にリリースが作成されます

```bash
# リリースタグの作成例
git tag v1.0.0
git push origin v1.0.0
```

## 開発状況

✅ プロジェクト構造とソリューションのセットアップが完了しました  
✅ 基本的な名前空間と構造を確立しました  
✅ CI/CD パイプラインを整備しました  
📋 次のステップ: [実装計画書](docs/implementation-plan.md)に従って実装を進める
