# 実装計画 - クイックスタートガイド

このドキュメントは、こいこいルールエンジンの実装を始めるためのクイックスタートガイドです。

## 📖 まず読むべきドキュメント

### 1. [実装ロードマップ](implementation-roadmap.md) ⭐ 最初に読む
ビジュアルで分かりやすい実装の流れ。全体像を把握するのに最適。

### 2. [実装計画書](implementation-plan.md) 📋 詳細を確認
30個のIssueの詳細な説明。各Issueのタスク内容、成果物、推定工数を記載。

### 3. [Issue作成ガイド](issues-summary.md) 🎫 Issue作成時に参照
GitHubでIssueを作成する際のテンプレートとガイド。

## 🚀 すぐに始める手順

### ステップ1: GitHub上でのセットアップ（手動作業）

#### 1.1 マイルストーンの作成
GitHubリポジトリで以下の4つのマイルストーンを作成：

- **M1: MVP完成** - 期限: 開始から3週間後
- **M2: 完全ルール実装** - 期限: 開始から5週間後
- **M3: 拡張機能実装** - 期限: 開始から6週間後
- **M4: リリース準備完了** - 期限: 開始から8週間後

#### 1.2 ラベルの作成
以下のラベルを作成（詳細は[Issue作成ガイド](issues-summary.md#ラベル定義)を参照）：

**技術系:**
- `core` - Core層の実装
- `domain` - Domain層の実装
- `facade` - Facade層の実装
- `testing` - テスト関連

**機能系:**
- `yaku` - 役判定関連
- `koikoi` - こいこい機能
- `special-rule` - 特殊ルール
- `events` - イベントシステム

**優先度:**
- `priority-high` - 高優先度（MVP必須）
- `priority-medium` - 中優先度
- `priority-low` - 低優先度（拡張機能）

#### 1.3 30個のIssueを作成
[Issue作成ガイド](issues-summary.md)のテンプレートを使用して、以下のIssueを作成：

**優先的に作成すべきIssue（最初の3つ）:**
1. Issue #1: プロジェクト構造とソリューションのセットアップ
2. Issue #2: Core層の基本データモデル実装
3. Issue #30: CI/CDの設定

**残りのIssue:**
- フェーズ1: Issue #3-12
- フェーズ2: Issue #13-19, #28
- フェーズ3: Issue #20-23
- フェーズ4: Issue #24-27, #29

### ステップ2: 開発環境のセットアップ

#### 2.1 必要なツール
- .NET SDK（最新のLTS版を推奨）
- IDE（Visual Studio、Visual Studio Code、Rider等）
- Git

#### 2.2 リポジトリのクローン
```bash
git clone https://github.com/ToYama170402/hanafuda.git
cd hanafuda
```

### ステップ3: 実装開始

#### 3.1 Issue #1から始める
最初のIssueを実装：

```bash
git checkout -b feature/issue-1-project-setup
```

**タスク内容:**
- [ ] .NET Coreソリューションファイルの作成
- [ ] 3つのクラスライブラリプロジェクトを作成
- [ ] テストプロジェクトを作成
- [ ] プロジェクト間の参照関係を設定
- [ ] .gitignoreファイルの作成

**作成するプロジェクト:**
```
HanafudaEngine.sln
├── HanafudaEngine.Core/          (クラスライブラリ)
├── HanafudaEngine.Domain/        (クラスライブラリ)
├── HanafudaEngine.Facade/        (クラスライブラリ)
└── HanafudaEngine.Tests/         (xUnitテストプロジェクト)
```

#### 3.2 コマンド例
```bash
# ソリューション作成
dotnet new sln -n HanafudaEngine

# プロジェクト作成
dotnet new classlib -n HanafudaEngine.Core
dotnet new classlib -n HanafudaEngine.Domain
dotnet new classlib -n HanafudaEngine.Facade
dotnet new xunit -n HanafudaEngine.Tests

# ソリューションに追加
dotnet sln add HanafudaEngine.Core/HanafudaEngine.Core.csproj
dotnet sln add HanafudaEngine.Domain/HanafudaEngine.Domain.csproj
dotnet sln add HanafudaEngine.Facade/HanafudaEngine.Facade.csproj
dotnet sln add HanafudaEngine.Tests/HanafudaEngine.Tests.csproj

# プロジェクト参照を設定
cd HanafudaEngine.Domain
dotnet add reference ../HanafudaEngine.Core/HanafudaEngine.Core.csproj

cd ../HanafudaEngine.Facade
dotnet add reference ../HanafudaEngine.Domain/HanafudaEngine.Domain.csproj

cd ../HanafudaEngine.Tests
dotnet add reference ../HanafudaEngine.Core/HanafudaEngine.Core.csproj
dotnet add reference ../HanafudaEngine.Domain/HanafudaEngine.Domain.csproj
dotnet add reference ../HanafudaEngine.Facade/HanafudaEngine.Facade.csproj

cd ..
dotnet build
```

#### 3.3 テストの実行
```bash
dotnet test
```

## 📝 開発の流れ

### 1. Issueを選択
実装するIssueを選び、ブランチを作成：
```bash
git checkout -b feature/issue-X-description
```

### 2. 実装
[実装計画書](implementation-plan.md)の該当Issueを参照し、タスクを実装。

### 3. テスト
```bash
dotnet test
```

### 4. コミットとプッシュ
```bash
git add .
git commit -m "Issue #X: 実装内容の説明"
git push origin feature/issue-X-description
```

### 5. プルリクエスト作成
GitHubでプルリクエストを作成し、レビュー。

### 6. マージ
レビュー完了後、mainブランチにマージ。

## 🎯 開発の優先順位

### 最優先（今すぐ）
1. Issue #1: プロジェクト構造のセットアップ
2. Issue #30: CI/CDの設定（並行）

### 次に重要（Week 1）
1. Issue #2: データモデル実装
2. Issue #4: 状態管理実装（並行可能）
3. Issue #3: 札の定義

### その後（Week 2-3）
- Issue #5: 山札管理
- Issue #6: 役判定
- Issue #7: 得点計算
- Issue #8: ターン進行
- Issue #9: アクション定義
- Issue #10: ファサード
- Issue #11: 配札・勝敗
- Issue #12: 統合テスト（MVP完成！）

## 📊 進捗の確認

### マイルストーンで確認
GitHubのマイルストーンページで進捗を確認。

### チェックリスト
各フェーズ完了時の確認項目：

**フェーズ1完了時:**
- [ ] 手札を出して札を取得できる
- [ ] 基本的な役判定が機能する
- [ ] 得点計算が正確
- [ ] 勝敗判定が正しい
- [ ] テストカバレッジ70%以上

**フェーズ2完了時:**
- [ ] こいこい機能が完全動作
- [ ] 全ての役が判定される
- [ ] 喰い付きが正しく処理される
- [ ] 総流れが正しく処理される
- [ ] テストカバレッジ80%以上

**フェーズ3完了時:**
- [ ] ルール設定がカスタマイズ可能
- [ ] イベントシステムが機能
- [ ] テストカバレッジ85%以上

**フェーズ4完了時:**
- [ ] パフォーマンスが基準を満たす
- [ ] 完全なドキュメントが整備
- [ ] リプレイ機能が動作

## 🤝 チームでの開発

### 役割分担の例（3人チーム）

**メンバーA: コアロジック担当**
- Issue #2, #3, #6, #7 (データモデル、札定義、役判定、得点計算)
- Issue #15, #16, #21 (特殊役、ルール設定)

**メンバーB: ゲームフロー担当**
- Issue #4, #8, #9, #10 (状態管理、ターン進行、アクション、ファサード)
- Issue #13, #14, #22 (こいこい、喰い付き、イベント)

**メンバーC: インフラ・テスト担当**
- Issue #1, #30 (プロジェクト構造、CI/CD)
- Issue #5, #11 (山札管理、配札・勝敗)
- Issue #12, #19, #23, #27 (各フェーズの統合テスト)

### コミュニケーション
- 毎日のスタンドアップミーティング
- コードレビューの実施
- Issueでの進捗共有

## 📚 参考資料

### 仕様書
- [こいこい仕様書](hanafuda-specification.md) - ゲームルール
- [実装仕様書](implementation-specification.md) - 技術仕様

### 実装計画
- [実装ロードマップ](implementation-roadmap.md) - ビジュアルガイド
- [実装計画書](implementation-plan.md) - 詳細な計画
- [仕様書と実装の対応表](specification-mapping.md) - 対応関係

### Issue管理
- [Issue作成ガイド](issues-summary.md) - Issueテンプレート

## 🆘 困ったときは

### よくある質問

**Q: どのIssueから始めればいい？**
A: Issue #1（プロジェクト構造のセットアップ）から始めてください。

**Q: 並行して開発できるIssueは？**
A: [仕様書と実装の対応表](specification-mapping.md#並行実装可能なissueグループ)を参照してください。

**Q: テストはどうすればいい？**
A: 各Issueに対応するテストケースを作成してください。テスト例は実装仕様書を参照。

**Q: 依存関係が分からない**
A: [仕様書と実装の対応表](specification-mapping.md#依存関係グラフ)を参照してください。

## ✅ 完了条件

### MVP完成（フェーズ1）
基本的なゲームが遊べる状態。これを最優先で目指す。

### 完全ルール実装（フェーズ2）
こいこいを含む完全なルールが実装されている。

### 拡張機能実装（フェーズ3）
カスタマイズとイベントシステムが実装されている。

### リリース準備完了（フェーズ4）
最適化、ドキュメント、リプレイ機能が完成している。

---

**重要: まずは[実装ロードマップ](implementation-roadmap.md)を読んで、全体像を把握してください！**

**作成日**: 2025-11-17  
**バージョン**: 1.0
