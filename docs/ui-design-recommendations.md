# UI設計方針（推奨案）

本ドキュメントは、こいこいルールエンジンにUIを追加する場合の設計方針の推奨案をまとめたものです。  
現在のエンジン実装はスコープ外としてUIを含みませんが、将来的にUIを追加する際の参考として作成しました。

## 1. 基本方針

### 1.1 関心の分離

既存のアーキテクチャを活かし、UIはゲームエンジンに依存するが、ゲームエンジンはUIに依存しない設計を維持する。

```
┌─────────────────────────────────────┐
│     UI Layer（新規追加）              │
│     ・画面描画                        │
│     ・ユーザー入力処理                │
│     ・アニメーション                  │
└─────────────────────────────────────┘
               ↓ ↑
┌─────────────────────────────────────┐
│     ViewModel Layer（新規追加）       │
│     ・UIの状態管理                   │
│     ・コマンドの変換                  │
│     ・バインディングプロパティ         │
└─────────────────────────────────────┘
               ↓ ↑
┌─────────────────────────────────────┐
│     Game Facade（既存）               │
│     KoiKoiGame                       │
└─────────────────────────────────────┘
```

### 1.2 設計原則

- **エンジンの独立性を維持**: `HanafudaEngine.Core/Domain/Facade` は変更しない
- **MVVM パターンを採用**: UIロジックとビジュアルを分離し、テスタビリティを確保
- **リアクティブな状態更新**: `GameEvent` を活用してUIを更新する
- **UIは別プロジェクトとして追加**: ソリューションに新規プロジェクトを追加し、既存プロジェクトを汚染しない

## 2. プラットフォーム選択

### 2.1 推奨: Blazor WebAssembly

C# エコシステムを維持しながらブラウザで動作するため、最も推奨するプラットフォームです。

**メリット**
- C# のみで実装できるため、既存のコードベースと一貫性を保てる
- ブラウザで動作するためインストール不要で広く利用可能
- `HanafudaEngine` パッケージをNuGet経由で参照するだけで統合できる
- テストしやすい（コンポーネントのユニットテストが可能）

**プロジェクト追加例**
```
HanafudaEngine/
├── src/
│   ├── HanafudaEngine.Core/
│   ├── HanafudaEngine.Domain/
│   ├── HanafudaEngine.Facade/
│   └── HanafudaApp.Blazor/       ← 新規追加
├── tests/
│   ├── HanafudaEngine.Tests/
│   └── HanafudaApp.Blazor.Tests/ ← 新規追加（任意）
```

### 2.2 代替案: .NET MAUI

デスクトップ・モバイルのネイティブアプリが必要な場合の選択肢です。

**メリット**
- Windows、macOS、iOS、Android に対応
- ネイティブUIコンポーネントを使用できる

**デメリット**
- Blazor と比較してセットアップが複雑
- プラットフォームごとの差異への対応が必要

### 2.3 参考: コンソールUI

テスト・デバッグ目的であれば、最小コストで実装できます。

```csharp
// コンソールUIの例（最小実装）
var game = new KoiKoiGame(PlayerId.Player1);
while (game.GetState().Phase != GamePhase.GameOver)
{
    var state = game.GetState();
    Console.WriteLine($"場: {string.Join(", ", state.Field.Select(c => c.Name))}");
    
    var actions = game.GetAvailableActions();
    // ユーザーにアクションを選択させる処理...
}
```

## 3. アーキテクチャパターン（MVVM）

### 3.1 ViewModel の設計

```csharp
// UIの状態とゲームエンジンの橋渡し役
public class KoiKoiViewModel : INotifyPropertyChanged
{
    private readonly KoiKoiGame _game;
    
    // UIにバインドするプロパティ
    public IReadOnlyList<CardViewModel> FieldCards { get; private set; }
    public IReadOnlyList<CardViewModel> PlayerHandCards { get; private set; }
    public IReadOnlyList<CardViewModel> OpponentHandCards { get; private set; }
    public IReadOnlyList<CardViewModel> PlayerCapturedCards { get; private set; }
    public IReadOnlyList<CardViewModel> OpponentCapturedCards { get; private set; }
    public IReadOnlyList<YakuViewModel> PlayerYaku { get; private set; }
    public int PlayerScore { get; private set; }
    public int OpponentScore { get; private set; }
    public bool CanCallKoiKoi { get; private set; }
    public bool CanCallShobu { get; private set; }
    public string PhaseMessage { get; private set; }
    
    // UIからのコマンド
    public ICommand PlayCardCommand { get; }
    public ICommand SelectFieldCardCommand { get; }
    public ICommand CallKoiKoiCommand { get; }
    public ICommand CallShobuCommand { get; }
    
    private void OnGameStateChanged(GameState state, IReadOnlyList<GameEvent> events)
    {
        // ゲームイベントに基づいてUIを更新
        FieldCards = state.Field.Select(c => new CardViewModel(c)).ToList();
        // ...
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FieldCards)));
    }
}
```

### 3.2 GameEvent を活用したリアクティブ更新

既存の `GameActionResult.Events` を活用してUIをアニメーション付きで更新します。

```csharp
public async Task ExecuteActionAsync(IGameAction action)
{
    var result = _game.ExecuteAction(action);
    
    if (!result.Success)
    {
        // エラー表示
        return;
    }
    
    // イベントに応じたアニメーションを順次実行
    foreach (var gameEvent in result.Events)
    {
        switch (gameEvent)
        {
            case CardPlayedEvent e:
                await AnimateCardPlay(e.Card);
                break;
            case CardsCapturedEvent e:
                await AnimateCardCapture(e.Cards);
                break;
            case YakuCompletedEvent e:
                await ShowYakuNotification(e.Yaku);
                break;
            case KoiKoiCalledEvent:
                await ShowKoiKoiAnnouncement();
                break;
            case GameEndedEvent e:
                await ShowGameResult(e.Result);
                break;
        }
    }
    
    // ViewModelの状態を更新
    UpdateViewModel(result.NewState);
}
```

## 4. 画面構成

### 4.1 ゲーム画面のレイアウト

```
┌─────────────────────────────────────────────────┐
│  相手の手札（裏向き）   相手の取得札   得点: XX  │
│  [🂠][🂠][🂠][🂠][🂠][🂠][🂠][🂠]                │
├─────────────────────────────────────────────────┤
│                    場 (フィールド)                │
│  [松に鶴][梅に鶯][桜に幕][藤に不如帰]            │
│  [菖蒲に八橋][牡丹に蝶][萩に猪][芒に月]          │
├─────────────────────────────────────────────────┤
│  自分の取得札   役: [三光][種]   得点: XX        │
│  自分の手札                                      │
│  [松に鶴][梅に鶯][桜に幕][藤に不如帰]            │
│                    [こいこい] [勝負]              │
└─────────────────────────────────────────────────┘
```

### 4.2 主要コンポーネント

| コンポーネント | 説明 |
|---|---|
| `CardComponent` | 1枚の札を表示（表/裏・選択状態・ハイライト） |
| `FieldComponent` | 場札エリア（最大8枚） |
| `HandComponent` | 手札エリア（クリックで選択） |
| `CapturedCardsComponent` | 取得札エリア（取得済み一覧） |
| `YakuListComponent` | 完成した役の一覧表示 |
| `ScoreComponent` | 現在の得点表示 |
| `KoiKoiDecisionComponent` | こいこい/勝負の選択ダイアログ |
| `GameResultComponent` | ゲーム終了時の結果表示 |

## 5. UI実装の注意点

### 5.1 ゲームフェーズとUI状態の対応

`GamePhase` および `TurnPhase` に基づいて、インタラクティブな要素を制御します。

| フェーズ | UIの状態 |
|---|---|
| `NotStarted` | ゲーム開始ボタンのみ表示 |
| `Dealing` | 配札アニメーション、入力無効 |
| `PlayerTurn / PlayFromHand` | 手札のカードをクリック可能 |
| `PlayerTurn / SelectFieldCard` | 場札から選択可能なカードをハイライト |
| `DrawFromDeck` | 山札をクリック可能（または自動） |
| `KoiKoiDecision` | こいこい/勝負ボタンを表示 |
| `GameOver` | 結果ダイアログを表示 |

### 5.2 相手プレイヤーの扱い

現在のエンジンはAIを持たないため、UIを追加する際はAIエージェントを別途実装する必要があります。

```csharp
// AIエージェントの例（UIプロジェクト内に実装）
public class SimpleAiAgent
{
    public IGameAction DecideAction(GameState state, IReadOnlyList<IGameAction> availableActions)
    {
        // ランダムに選択する単純なAI
        return availableActions[Random.Shared.Next(availableActions.Count)];
    }
}
```

### 5.3 アクセシビリティ

- 各カードにalt属性またはaria-labelを設定する（例：「1月・松に鶴・光札」）
- キーボード操作での全操作に対応する
- 色だけに依存しない情報伝達（カードの種類はアイコンでも示す）

## 6. 実装優先度（提案）

UI追加を検討する場合、以下の順序で実装することを推奨します。

1. **フェーズ1（最小限）**: コンソールUI
   - ゲームエンジンの動作確認
   - ルールの検証

2. **フェーズ2（基本UI）**: Blazor WebAssembly
   - 2人対戦（人間vs人間、同一端末）
   - カード表示と選択
   - 役と得点の表示

3. **フェーズ3（AI対戦）**: AIエージェントの追加
   - ランダムAI
   - ルールベースAI

4. **フェーズ4（拡張機能）**: UX改善
   - アニメーション
   - サウンド
   - 操作ログ・リプレイ機能

## 7. プロジェクト構成例（Blazor WebAssembly）

```
src/HanafudaApp.Blazor/
├── HanafudaApp.Blazor.csproj
├── Program.cs
├── App.razor
├── Components/
│   ├── CardComponent.razor
│   ├── FieldComponent.razor
│   ├── HandComponent.razor
│   ├── CapturedCardsComponent.razor
│   ├── YakuListComponent.razor
│   └── KoiKoiDecisionComponent.razor
├── Pages/
│   ├── Index.razor        # ゲームメイン画面
│   └── Result.razor       # 結果表示
├── ViewModels/
│   ├── KoiKoiViewModel.cs
│   ├── CardViewModel.cs
│   └── YakuViewModel.cs
└── wwwroot/
    ├── images/
    │   └── cards/         # カード画像（各48枚）
    └── css/
```
