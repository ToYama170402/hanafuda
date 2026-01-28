# hanafuda
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
```

## 開発状況

✅ プロジェクト構造とソリューションのセットアップが完了しました  
✅ 基本的な名前空間と構造を確立しました  
📋 次のステップ: [実装計画書](docs/implementation-plan.md)に従って実装を進める
