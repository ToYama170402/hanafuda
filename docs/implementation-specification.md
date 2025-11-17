# こいこいルールエンジン実装仕様書

## 1. 概要

### 1.1 目的
本ドキュメントは、花札ゲーム「こいこい」のルールエンジンをC#で実装するための技術仕様書です。UIを持たない純粋なゲームロジックの実装を目的とします。

### 1.2 スコープ
- ゲームルールの実装（札の管理、ターンの進行、役判定、得点計算）
- ゲーム状態の管理
- プレイヤーのアクション処理
- **スコープ外**: UI、ネットワーク通信、データ永続化

### 1.3 設計原則
- **疎結合**: UIやインフラストラクチャから独立
- **テスタビリティ**: ユニットテスト可能な設計
- **拡張性**: ルール変更やバリエーションへの対応
- **不変性**: ゲーム状態の変更は新しいインスタンスを生成
- **型安全性**: C#の型システムを活用したバグの防止

## 2. システムアーキテクチャ

### 2.1 レイヤー構成

```
┌─────────────────────────────────────┐
│     Application Layer (UI)          │
│     (スコープ外)                      │
└─────────────────────────────────────┘
              ↓ ↑
┌─────────────────────────────────────┐
│     Game Facade                      │
│     ・ゲーム全体の制御                │
│     ・外部インターフェース             │
└─────────────────────────────────────┘
              ↓ ↑
┌─────────────────────────────────────┐
│     Domain Layer                     │
│     ・ゲームロジック                  │
│     ・ルールエンジン                  │
│     ・状態管理                        │
└─────────────────────────────────────┘
              ↓ ↑
┌─────────────────────────────────────┐
│     Core Layer                       │
│     ・基本的なドメインモデル          │
│     ・値オブジェクト                  │
└─────────────────────────────────────┘
```

### 2.2 主要コンポーネント

#### 2.2.1 Core Layer
- `Card`: 札を表現
- `Month`: 月（1月～12月）を表現
- `CardType`: 札の種類（光、種、短冊、カス）を表現
- `Yaku`: 役を表現
- `PlayerId`: プレイヤーIDを表現

#### 2.2.2 Domain Layer
- `GameState`: ゲーム状態の管理
- `Deck`: 山札の管理
- `Field`: 場札の管理
- `Hand`: 手札の管理
- `CapturedCards`: 取得札の管理
- `YakuEvaluator`: 役の判定
- `ScoreCalculator`: 得点計算
- `TurnManager`: ターン進行管理
- `ActionValidator`: アクションの妥当性検証

#### 2.2.3 Facade Layer
- `KoiKoiGame`: ゲーム全体を統括するファサード

## 3. データモデル

### 3.1 基本的な値オブジェクト

#### 3.1.1 Month（月）

```csharp
public enum Month
{
    January = 1,    // 松
    February = 2,   // 梅
    March = 3,      // 桜
    April = 4,      // 藤
    May = 5,        // 菖蒲
    June = 6,       // 牡丹
    July = 7,       // 萩
    August = 8,     // 芒
    September = 9,  // 菊
    October = 10,   // 紅葉
    November = 11,  // 柳
    December = 12   // 桐
}
```

#### 3.1.2 CardType（札の種類）

```csharp
public enum CardType
{
    Hikari,    // 光札
    Tane,      // 種札
    Tanzaku,   // 短冊札
    Kasu       // カス札
}
```

#### 3.1.3 Card（札）

```csharp
public sealed class Card : IEquatable<Card>
{
    public Guid Id { get; }              // 札の一意識別子
    public Month Month { get; }          // 月
    public CardType Type { get; }        // 種類
    public string Name { get; }          // 名前（例: "松に鶴"）
    public bool IsSpecial { get; }       // 特殊札フラグ（赤短、青短など）
    public SpecialCardFlag? SpecialFlag { get; } // 特殊札の種類
    
    // コンストラクタ、等値比較メソッドなど
}
```

#### 3.1.4 SpecialCardFlag（特殊札フラグ）

```csharp
[Flags]
public enum SpecialCardFlag
{
    None = 0,
    RedPoetry = 1,      // 赤短（あかよろし、みよしの）
    BluePoetry = 2,     // 青短（あをたん）
    SakeCup = 4,        // 盃
    Deer = 8,           // 鹿
    Boar = 16,          // 猪
    Butterfly = 32,     // 蝶
    RainMan = 64        // 雨札（柳に小野道風）
}
```

### 3.2 札のコレクション定義

#### 3.2.1 CardDefinitions（札定義）

```csharp
public static class CardDefinitions
{
    // 全48枚の札を定義する静的コレクション
    public static IReadOnlyList<Card> AllCards { get; }
    
    // 月別に札を取得
    public static IReadOnlyList<Card> GetCardsByMonth(Month month);
    
    // 種類別に札を取得
    public static IReadOnlyList<Card> GetCardsByType(CardType type);
    
    // 特定の札を取得（例: 松に鶴）
    public static Card GetCard(Month month, CardType type, int index = 0);
}
```

### 3.3 役（Yaku）

#### 3.3.1 YakuType（役の種類）

```csharp
public enum YakuType
{
    // 光札の役
    Goko,           // 五光
    Shiko,          // 四光
    AmeShiko,       // 雨四光
    Sanko,          // 三光
    
    // 種札の役
    Inoshikacho,    // 猪鹿蝶
    Hanami,         // 花見で一杯
    Tsukimi,        // 月見で一杯
    Tane,           // 種
    
    // 短冊札の役
    Akatan,         // 赤短
    Aotan,          // 青短
    Tanzaku,        // 短冊
    
    // カス札の役
    Kasu            // カス
}
```

#### 3.3.2 Yaku（役）

```csharp
public sealed class Yaku
{
    public YakuType Type { get; }
    public int BaseScore { get; }              // 基本得点
    public int BonusScore { get; }             // ボーナス得点（枚数超過分）
    public int TotalScore => BaseScore + BonusScore;
    public IReadOnlyList<Card> Cards { get; }  // 役を構成する札
    
    public Yaku(YakuType type, int baseScore, int bonusScore, IEnumerable<Card> cards)
    {
        Type = type;
        BaseScore = baseScore;
        BonusScore = bonusScore;
        Cards = cards.ToList().AsReadOnly();
    }
}
```

### 3.4 ゲーム状態

#### 3.4.1 PlayerId（プレイヤーID）

```csharp
public enum PlayerId
{
    Player1 = 0,  // 親
    Player2 = 1   // 子
}
```

#### 3.4.2 GamePhase（ゲームフェーズ）

```csharp
public enum GamePhase
{
    NotStarted,           // 未開始
    Dealing,              // 配札中
    PlayerTurn,           // プレイヤーのターン
    DrawFromDeck,         // 山札から引く
    YakuCheck,            // 役の確認
    KoiKoiDecision,       // こいこい判断
    GameOver,             // ゲーム終了
    Draw                  // 引き分け（総流れ）
}
```

#### 3.4.3 TurnPhase（ターンフェーズ）

```csharp
public enum TurnPhase
{
    PlayFromHand,         // 手札を出す
    SelectFieldCard,      // 場札を選ぶ（同月札が複数ある場合）
    DrawFromDeck,         // 山札を引く
    SelectFieldCardForDraw, // 山札で引いた札の場札を選ぶ
    YakuCheck,            // 役チェック
    KoiKoiDecision,       // こいこい判断
    TurnEnd               // ターン終了
}
```

#### 3.4.4 PlayerState（プレイヤー状態）

```csharp
public sealed class PlayerState
{
    public PlayerId Id { get; }
    public IReadOnlyList<Card> Hand { get; }            // 手札
    public IReadOnlyList<Card> CapturedCards { get; }   // 取得札
    public bool HasCalledKoiKoi { get; }                // こいこい宣言フラグ
    public IReadOnlyList<Yaku> CompletedYaku { get; }   // 完成した役
    public int CurrentScore { get; }                     // 現在の得点
    
    // 不変性を保つため、状態変更は新しいインスタンスを返す
    public PlayerState WithHand(IEnumerable<Card> newHand);
    public PlayerState WithCapturedCards(IEnumerable<Card> newCaptured);
    public PlayerState WithKoiKoiCalled(bool called);
    public PlayerState WithCompletedYaku(IEnumerable<Yaku> yaku);
}
```

#### 3.4.5 GameState（ゲーム状態）

```csharp
public sealed class GameState
{
    public Guid GameId { get; }                                    // ゲームID
    public GamePhase Phase { get; }                                 // ゲームフェーズ
    public TurnPhase TurnPhase { get; }                             // ターンフェーズ
    public PlayerId CurrentPlayer { get; }                          // 現在のプレイヤー
    public PlayerId Dealer { get; }                                 // 親
    public int TurnCount { get; }                                   // ターン数
    public IReadOnlyList<Card> Deck { get; }                        // 山札
    public IReadOnlyList<Card> Field { get; }                       // 場札
    public IReadOnlyDictionary<PlayerId, PlayerState> Players { get; } // プレイヤー状態
    public Card? LastPlayedCard { get; }                            // 最後に出された札
    public Card? LastDrawnCard { get; }                             // 最後に引かれた札
    public IReadOnlyList<Card> PendingCapture { get; }              // 取得待ちの札
    public PlayerId? Winner { get; }                                // 勝者
    public GameResult? Result { get; }                              // ゲーム結果
    
    // 状態変更は新しいインスタンスを返す
    public GameState WithPhase(GamePhase phase);
    public GameState WithTurnPhase(TurnPhase turnPhase);
    public GameState WithCurrentPlayer(PlayerId player);
    public GameState WithDeck(IEnumerable<Card> deck);
    public GameState WithField(IEnumerable<Card> field);
    public GameState WithPlayerState(PlayerId player, PlayerState state);
    public GameState WithWinner(PlayerId winner, GameResult result);
    // その他のwithメソッド...
}
```

#### 3.4.6 GameResult（ゲーム結果）

```csharp
public sealed class GameResult
{
    public PlayerId? Winner { get; }                    // 勝者（nullの場合は引き分け）
    public IReadOnlyDictionary<PlayerId, int> Scores { get; } // 各プレイヤーの得点
    public IReadOnlyDictionary<PlayerId, IReadOnlyList<Yaku>> CompletedYaku { get; } // 完成した役
    public bool IsKoiKoiWin { get; }                    // こいこい勝ちか
    public bool IsDraw { get; }                          // 引き分けか
}
```

## 4. ゲームフロー

### 4.1 ゲーム初期化

```
1. ゲームインスタンス生成
   ↓
2. 親の決定
   ↓
3. デッキのシャッフル
   ↓
4. 配札（各プレイヤー8枚、場8枚）
   ↓
5. 配札の検証
   - 場に同月4枚 → 配り直し
   - 手札に同月4枚 → 手四判定
   ↓
6. ゲーム開始（親のターン）
```

### 4.2 ターンの進行

```
1. PlayFromHand: プレイヤーが手札から1枚選択
   ↓
2. SelectFieldCard: 場に同月札があるか確認
   - なし → 場に札を追加、ステップ3へ
   - 1枚 → 自動取得、ステップ3へ
   - 2枚以上 → プレイヤーが選択、取得後ステップ3へ
   - 3枚（喰い付き） → 全て取得、ステップ3へ
   ↓
3. DrawFromDeck: 山札から1枚引く
   ↓
4. SelectFieldCardForDraw: 場に同月札があるか確認
   - なし → 場に札を追加、ステップ5へ
   - 1枚 → 自動取得、ステップ5へ
   - 2枚以上 → プレイヤーが選択、取得後ステップ5へ
   - 3枚（喰い付き） → 全て取得、ステップ5へ
   ↓
5. YakuCheck: 役の判定
   - 役なし → ターン終了、相手のターンへ
   - 役あり → ステップ6へ
   ↓
6. KoiKoiDecision: こいこいか勝負か選択
   - 勝負 → ゲーム終了
   - こいこい → ターン終了、相手のターンへ
   ↓
7. TurnEnd: ターン終了
   - ターン数カウント
   - 8ターン経過 → 総流れ（引き分け）
   - 継続 → 相手のターンへ
```

### 4.3 状態遷移図

```
[NotStarted]
    ↓
[Dealing]
    ↓
[PlayerTurn] ←→ [DrawFromDeck] ←→ [YakuCheck] ←→ [KoiKoiDecision]
    ↓                                                      ↓
    ← ← ← ← ← ← ← ← ← ← ← ← ← ←                         ↓
    |                                                      ↓
    ↓ (8ターン経過)                                       ↓
[Draw]                                              [GameOver]
```

## 5. 主要クラスとインターフェース

### 5.1 KoiKoiGame（ファサードクラス）

```csharp
public sealed class KoiKoiGame
{
    private GameState _state;
    private readonly YakuEvaluator _yakuEvaluator;
    private readonly ScoreCalculator _scoreCalculator;
    private readonly ActionValidator _actionValidator;
    
    // ゲーム初期化
    public KoiKoiGame(PlayerId dealer, int? randomSeed = null);
    
    // 現在のゲーム状態を取得
    public GameState GetState();
    
    // 可能なアクションを取得
    public IReadOnlyList<IGameAction> GetAvailableActions();
    
    // アクションを実行
    public GameActionResult ExecuteAction(IGameAction action);
    
    // ゲームをリセット
    public void Reset(PlayerId dealer);
}
```

### 5.2 IGameAction（アクションインターフェース）

```csharp
public interface IGameAction
{
    ActionType Type { get; }
}

public enum ActionType
{
    PlayCard,           // 札を出す
    SelectFieldCard,    // 場札を選択
    DrawCard,           // 山札から引く
    CallKoiKoi,         // こいこいを宣言
    CallShobu,          // 勝負を宣言
}

// 具体的なアクション
public sealed class PlayCardAction : IGameAction
{
    public ActionType Type => ActionType.PlayCard;
    public Card Card { get; }
}

public sealed class SelectFieldCardAction : IGameAction
{
    public ActionType Type => ActionType.SelectFieldCard;
    public Card FieldCard { get; }
}

public sealed class CallKoiKoiAction : IGameAction
{
    public ActionType Type => ActionType.CallKoiKoi;
}

public sealed class CallShobuAction : IGameAction
{
    public ActionType Type => ActionType.CallShobu;
}
```

### 5.3 GameActionResult（アクション実行結果）

```csharp
public sealed class GameActionResult
{
    public bool Success { get; }
    public string? ErrorMessage { get; }
    public GameState NewState { get; }
    public IReadOnlyList<GameEvent> Events { get; } // 発生したイベント
    
    public static GameActionResult Successful(GameState newState, IEnumerable<GameEvent> events);
    public static GameActionResult Failed(string errorMessage);
}
```

### 5.4 GameEvent（ゲームイベント）

```csharp
public abstract class GameEvent
{
    public DateTime Timestamp { get; }
}

// 具体的なイベント
public sealed class CardPlayedEvent : GameEvent
{
    public PlayerId Player { get; }
    public Card Card { get; }
}

public sealed class CardsCapturedEvent : GameEvent
{
    public PlayerId Player { get; }
    public IReadOnlyList<Card> Cards { get; }
}

public sealed class YakuCompletedEvent : GameEvent
{
    public PlayerId Player { get; }
    public IReadOnlyList<Yaku> Yaku { get; }
}

public sealed class KoiKoiCalledEvent : GameEvent
{
    public PlayerId Player { get; }
}

public sealed class GameEndedEvent : GameEvent
{
    public GameResult Result { get; }
}
```

## 6. 役判定アルゴリズム

### 6.1 YakuEvaluator（役判定クラス）

```csharp
public sealed class YakuEvaluator
{
    // プレイヤーの取得札から全ての完成した役を判定
    public IReadOnlyList<Yaku> EvaluateYaku(IReadOnlyList<Card> capturedCards);
    
    // 個別の役判定メソッド
    private Yaku? EvaluateGoko(IReadOnlyList<Card> cards);      // 五光
    private Yaku? EvaluateShiko(IReadOnlyList<Card> cards);     // 四光
    private Yaku? EvaluateAmeShiko(IReadOnlyList<Card> cards);  // 雨四光
    private Yaku? EvaluateSanko(IReadOnlyList<Card> cards);     // 三光
    private Yaku? EvaluateInoshikacho(IReadOnlyList<Card> cards); // 猪鹿蝶
    private Yaku? EvaluateHanami(IReadOnlyList<Card> cards);    // 花見で一杯
    private Yaku? EvaluateTsukimi(IReadOnlyList<Card> cards);   // 月見で一杯
    private Yaku? EvaluateTane(IReadOnlyList<Card> cards);      // 種
    private Yaku? EvaluateAkatan(IReadOnlyList<Card> cards);    // 赤短
    private Yaku? EvaluateAotan(IReadOnlyList<Card> cards);     // 青短
    private Yaku? EvaluateTanzaku(IReadOnlyList<Card> cards);   // 短冊
    private Yaku? EvaluateKasu(IReadOnlyList<Card> cards);      // カス
}
```

### 6.2 役判定の優先順位

光札の役は排他的（最も高得点の役のみ成立）：
1. 五光（15点）
2. 四光（10点）
3. 雨四光（8点）
4. 三光（6点）

その他の役は重複可能：
- 猪鹿蝶と種は重複
- 花見で一杯と種は重複
- 月見で一杯と種は重複
- 赤短と短冊は重複
- 青短と短冊は重複

### 6.3 役判定の具体例

#### 五光の判定

```csharp
private Yaku? EvaluateGoko(IReadOnlyList<Card> cards)
{
    var hikariCards = cards.Where(c => c.Type == CardType.Hikari).ToList();
    
    if (hikariCards.Count == 5)
    {
        return new Yaku(YakuType.Goko, baseScore: 15, bonusScore: 0, hikariCards);
    }
    
    return null;
}
```

#### 種の判定（可変得点）

```csharp
private Yaku? EvaluateTane(IReadOnlyList<Card> cards)
{
    var taneCards = cards.Where(c => c.Type == CardType.Tane).ToList();
    
    if (taneCards.Count >= 5)
    {
        int baseScore = 1;
        int bonusScore = taneCards.Count - 5; // 5枚を超える分
        return new Yaku(YakuType.Tane, baseScore, bonusScore, taneCards);
    }
    
    return null;
}
```

#### 赤短の判定

```csharp
private Yaku? EvaluateAkatan(IReadOnlyList<Card> cards)
{
    var redPoetryCards = cards
        .Where(c => c.Type == CardType.Tanzaku && 
                    c.SpecialFlag.HasValue && 
                    c.SpecialFlag.Value.HasFlag(SpecialCardFlag.RedPoetry))
        .ToList();
    
    // 松、梅、桜の赤短3枚
    bool hasMatsu = redPoetryCards.Any(c => c.Month == Month.January);
    bool hasUme = redPoetryCards.Any(c => c.Month == Month.February);
    bool hasSakura = redPoetryCards.Any(c => c.Month == Month.March);
    
    if (hasMatsu && hasUme && hasSakura)
    {
        return new Yaku(YakuType.Akatan, baseScore: 6, bonusScore: 0, redPoetryCards);
    }
    
    return null;
}
```

## 7. 得点計算

### 7.1 ScoreCalculator（得点計算クラス）

```csharp
public sealed class ScoreCalculator
{
    // プレイヤーの最終得点を計算
    public int CalculateFinalScore(
        IReadOnlyList<Yaku> completedYaku,
        bool hasCalledKoiKoi,
        bool wonAfterKoiKoi)
    {
        int totalScore = completedYaku.Sum(y => y.TotalScore);
        
        // こいこいで勝った場合は2倍
        if (hasCalledKoiKoi && wonAfterKoiKoi)
        {
            totalScore *= 2;
        }
        
        return totalScore;
    }
    
    // ゲーム終了時の得点計算
    public GameResult CalculateGameResult(GameState state)
    {
        var scores = new Dictionary<PlayerId, int>();
        var completedYaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>();
        
        foreach (var (playerId, playerState) in state.Players)
        {
            completedYaku[playerId] = playerState.CompletedYaku;
            
            if (state.Winner.HasValue && state.Winner.Value == playerId)
            {
                // 勝者の得点計算
                bool wonAfterKoiKoi = playerState.HasCalledKoiKoi;
                scores[playerId] = CalculateFinalScore(
                    playerState.CompletedYaku,
                    playerState.HasCalledKoiKoi,
                    wonAfterKoiKoi);
            }
            else
            {
                // 敗者の得点は0
                scores[playerId] = 0;
            }
        }
        
        return new GameResult(
            winner: state.Winner,
            scores: scores,
            completedYaku: completedYaku,
            isKoiKoiWin: state.Winner.HasValue && 
                         state.Players[state.Winner.Value].HasCalledKoiKoi,
            isDraw: !state.Winner.HasValue);
    }
}
```

### 7.2 得点計算の例

#### ケース1: 通常の勝負
- プレイヤーが「三光」(6点)を完成
- 「勝負」を宣言
- 最終得点: 6点

#### ケース2: こいこい成功
- プレイヤーが「赤短」(6点)を完成、「こいこい」を宣言
- その後「短冊」(1点)も完成、「勝負」を宣言
- 最終得点: (6 + 1) × 2 = 14点

#### ケース3: こいこい失敗
- プレイヤーが「種」(1点)を完成、「こいこい」を宣言
- 相手が「猪鹿蝶」(5点)を完成、「勝負」を宣言
- プレイヤーの得点: 0点
- 相手の得点: 5 × 2 = 10点

## 8. アクション検証

### 8.1 ActionValidator（アクション検証クラス）

```csharp
public sealed class ActionValidator
{
    // アクションが現在の状態で実行可能かを検証
    public ValidationResult Validate(IGameAction action, GameState state);
    
    // 個別のバリデーション
    private ValidationResult ValidatePlayCard(PlayCardAction action, GameState state);
    private ValidationResult ValidateSelectFieldCard(SelectFieldCardAction action, GameState state);
    private ValidationResult ValidateKoiKoiDecision(IGameAction action, GameState state);
}

public sealed class ValidationResult
{
    public bool IsValid { get; }
    public string? ErrorMessage { get; }
    
    public static ValidationResult Valid();
    public static ValidationResult Invalid(string errorMessage);
}
```

### 8.2 検証ルール

#### 8.2.1 手札を出す際の検証
- 現在のフェーズが`PlayFromHand`であること
- 現在のプレイヤーのターンであること
- 指定された札がプレイヤーの手札に存在すること

#### 8.2.2 場札を選択する際の検証
- 現在のフェーズが`SelectFieldCard`または`SelectFieldCardForDraw`であること
- 選択された場札が実際に場に存在すること
- 選択された場札と出した札/引いた札の月が一致すること

#### 8.2.3 こいこい/勝負を宣言する際の検証
- 現在のフェーズが`KoiKoiDecision`であること
- プレイヤーが役を完成させていること

## 9. 特殊ルールの実装

### 9.1 喰い付き（くいつき）

```csharp
public sealed class KuitsukiHandler
{
    // 喰い付きが発生するか判定
    public bool CheckKuitsuki(Card playedCard, IReadOnlyList<Card> field)
    {
        var sameMonthCards = field.Where(c => c.Month == playedCard.Month).ToList();
        return sameMonthCards.Count == 3;
    }
    
    // 喰い付き時の取得処理
    public IReadOnlyList<Card> ExecuteKuitsuki(
        Card playedCard, 
        Card drawnCard, 
        IReadOnlyList<Card> field)
    {
        if (playedCard.Month != drawnCard.Month)
        {
            throw new InvalidOperationException("喰い付きには同月の札が必要");
        }
        
        var capturedCards = new List<Card> { playedCard, drawnCard };
        capturedCards.AddRange(field.Where(c => c.Month == playedCard.Month));
        
        return capturedCards;
    }
}
```

### 9.2 手四（てし）

```csharp
public sealed class TeshiHandler
{
    // 手四が成立するか判定
    public bool CheckTeshi(IReadOnlyList<Card> hand)
    {
        var groupedByMonth = hand.GroupBy(c => c.Month);
        return groupedByMonth.Any(g => g.Count() == 4);
    }
    
    // 手四による即座の勝利処理（オプション）
    public GameState? HandleTeshi(GameState state, PlayerId player, bool teshiRuleEnabled)
    {
        if (!teshiRuleEnabled)
        {
            return null;
        }
        
        var playerState = state.Players[player];
        if (!CheckTeshi(playerState.Hand))
        {
            return null;
        }
        
        // 役満勝ちとして処理
        var result = new GameResult(
            winner: player,
            scores: new Dictionary<PlayerId, int> { [player] = 15, [GetOpponent(player)] = 0 },
            completedYaku: new Dictionary<PlayerId, IReadOnlyList<Yaku>>(),
            isKoiKoiWin: false,
            isDraw: false);
        
        return state.WithWinner(player, result).WithPhase(GamePhase.GameOver);
    }
    
    private PlayerId GetOpponent(PlayerId player) => 
        player == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;
}
```

### 9.3 総流れ（そうながれ）

```csharp
public sealed class DrawHandler
{
    private const int MaxTurns = 8;
    
    // 総流れが発生するか判定
    public bool CheckDraw(GameState state)
    {
        return state.TurnCount >= MaxTurns && state.Winner == null;
    }
    
    // 総流れ処理
    public GameState HandleDraw(GameState state)
    {
        var result = new GameResult(
            winner: null,
            scores: new Dictionary<PlayerId, int> 
            { 
                [PlayerId.Player1] = 0, 
                [PlayerId.Player2] = 0 
            },
            completedYaku: new Dictionary<PlayerId, IReadOnlyList<Yaku>>(),
            isKoiKoiWin: false,
            isDraw: true);
        
        return state.WithPhase(GamePhase.Draw).WithResult(result);
    }
}
```

### 9.4 配札の検証

```csharp
public sealed class DealingValidator
{
    // 場に同月4枚の札がないか確認
    public bool HasFourOfSameMonth(IReadOnlyList<Card> field)
    {
        var groupedByMonth = field.GroupBy(c => c.Month);
        return groupedByMonth.Any(g => g.Count() == 4);
    }
    
    // 再配札が必要か判定
    public bool RequiresRedeal(GameState state)
    {
        return HasFourOfSameMonth(state.Field);
    }
}
```

## 10. テスト戦略

### 11.1 ユニットテスト

#### 11.1.1 値オブジェクトのテスト
- `Card`の等値比較
- `Month`、`CardType`の検証
- `CardDefinitions`の札定義の整合性

#### 11.1.2 役判定のテスト
- 各役の判定ロジック（正常系）
- 役が成立しない場合（異常系）
- 役の複合（複数役の同時成立）
- 光札の役の排他性

#### 11.1.3 得点計算のテスト
- 基本得点の計算
- ボーナス得点の計算（種、短冊、カス）
- こいこいボーナス（2倍）
- こいこい失敗時の得点（0点）

#### 11.1.4 ゲームフローのテスト
- ゲーム初期化
- ターンの進行
- 札の取得
- 喰い付き
- 手四
- 総流れ

### 11.2 統合テスト

#### 11.2.1 完全なゲームフロー
- 開始から終了までの一連の流れ
- こいこいの宣言と続行
- 複数ラウンドのプレイ

#### 11.2.2 特殊ケース
- 配札で手四が成立
- 配札で場に同月4枚
- 連続するこいこい

### 11.3 テストデータ

#### 11.3.1 固定デッキ
- テスト用に特定の順序で札を配置したデッキ
- 再現可能なゲームシナリオ

```csharp
public static class TestDecks
{
    // 三光が完成するテスト用デッキ
    public static IReadOnlyList<Card> SankoTestDeck { get; }
    
    // 喰い付きが発生するテスト用デッキ
    public static IReadOnlyList<Card> KuitsukiTestDeck { get; }
    
    // 手四が発生するテスト用デッキ
    public static IReadOnlyList<Card> TeshiTestDeck { get; }
}
```

## 12. 拡張性と変則ルール

### 12.1 ルール設定

```csharp
public sealed class GameRules
{
    // 得点設定
    public int GokoScore { get; init; } = 15;      // 五光（デフォルト15点）
    public int ShikoScore { get; init; } = 10;     // 四光（デフォルト10点）
    public int AmeShikoScore { get; init; } = 8;   // 雨四光（デフォルト8点）
    public int SankoScore { get; init; } = 6;      // 三光（デフォルト6点）
    public int InoshikachoScore { get; init; } = 5; // 猪鹿蝶（デフォルト5点）
    public int HanamiScore { get; init; } = 5;     // 花見で一杯（デフォルト5点）
    public int TsukimiScore { get; init; } = 5;    // 月見で一杯（デフォルト5点）
    public int AkatanScore { get; init; } = 6;     // 赤短（デフォルト6点）
    public int AotanScore { get; init; } = 6;      // 青短（デフォルト6点）
    
    // 特殊ルール
    public bool EnableTeshi { get; init; } = false;  // 手四を有効化
    public int KoiKoiMultiplier { get; init; } = 2;  // こいこいの倍率（デフォルト2倍）
    public int MaxTurns { get; init; } = 8;          // 最大ターン数
    
    // デフォルトルール
    public static GameRules Default => new GameRules();
}
```

### 12.2 ルールのカスタマイズ例

```csharp
// 高得点設定
var highScoreRules = new GameRules
{
    GokoScore = 20,
    ShikoScore = 15,
    SankoScore = 10,
    KoiKoiMultiplier = 3
};

// 手四ありルール
var teshiEnabledRules = new GameRules
{
    EnableTeshi = true
};

var game = new KoiKoiGame(PlayerId.Player1, rules: highScoreRules);
```

## 13. エラーハンドリング

### 13.1 例外の種類

```csharp
// ゲームルール違反
public sealed class GameRuleViolationException : Exception
{
    public GameRuleViolationException(string message) : base(message) { }
}

// 無効なアクション
public sealed class InvalidActionException : Exception
{
    public IGameAction Action { get; }
    public InvalidActionException(IGameAction action, string message) 
        : base(message)
    {
        Action = action;
    }
}

// 不正なゲーム状態
public sealed class InvalidGameStateException : Exception
{
    public GameState State { get; }
    public InvalidGameStateException(GameState state, string message) 
        : base(message)
    {
        State = state;
    }
}
```

### 13.2 エラーハンドリング方針

- **プログラミングエラー**: 例外をスロー（nullチェック、範囲外アクセスなど）
- **ゲームルール違反**: 例外をスロー（不正なアクション、無効な状態遷移など）
- **ユーザー入力エラー**: `Result`パターンで処理（`GameActionResult.Failed`）

## 14. パフォーマンス考慮事項

### 14.1 最適化ポイント

#### 14.1.1 不変性とメモリ
- 状態変更時に新しいインスタンスを生成するため、メモリ使用量に注意
- 構造体（struct）の活用を検討（`Card`など）
- `IReadOnlyList`を活用してコピーを最小限に

#### 14.1.2 役判定の最適化
- 役判定は頻繁に実行されるため、効率的なアルゴリズムが重要
- LINQの使用は可読性とパフォーマンスのバランスを考慮
- 必要に応じてキャッシングを導入

## 15. 実装の優先順位

### フェーズ1: 基本機能（MVP）
1. 基本的なデータモデル（Card, Month, CardType, PlayerId）
2. GameStateとPlayerState
3. 札の配布とシャッフル
4. ターンの基本的な進行
5. 基本的な役判定（光札の役、種、短冊、カス）
6. 得点計算
7. 勝敗判定

### フェーズ2: 完全なルール実装
1. こいこいの宣言と処理
2. 喰い付き
3. 特殊な役（猪鹿蝶、花見で一杯、月見で一杯、赤短、青短）
4. 総流れ
5. 配札の検証と再配札

### フェーズ3: 拡張機能
1. 手四（オプション）
2. ルール設定のカスタマイズ
3. イベントシステム

### フェーズ4: 高度な機能
1. パフォーマンス最適化
2. 詳細なログ機能
3. リプレイ機能

## 16. 今後の課題

### 16.1 実装上の課題
- シリアライゼーション/デシリアライゼーション（保存/読込）
- 詳細なログ機能
- デバッグ機能の充実

---

## 付録A: プロジェクト構造例

```
HanafudaEngine/
├── HanafudaEngine.Core/
│   ├── Models/
│   │   ├── Card.cs
│   │   ├── Month.cs
│   │   ├── CardType.cs
│   │   ├── SpecialCardFlag.cs
│   │   ├── CardDefinitions.cs
│   │   ├── Yaku.cs
│   │   └── YakuType.cs
│   └── Enums/
│       └── PlayerId.cs
│
├── HanafudaEngine.Domain/
│   ├── GameState.cs
│   ├── PlayerState.cs
│   ├── GamePhase.cs
│   ├── TurnPhase.cs
│   ├── GameResult.cs
│   ├── Actions/
│   │   ├── IGameAction.cs
│   │   ├── ActionType.cs
│   │   ├── PlayCardAction.cs
│   │   ├── SelectFieldCardAction.cs
│   │   ├── CallKoiKoiAction.cs
│   │   └── CallShobuAction.cs
│   ├── Events/
│   │   ├── GameEvent.cs
│   │   ├── CardPlayedEvent.cs
│   │   ├── CardsCapturedEvent.cs
│   │   ├── YakuCompletedEvent.cs
│   │   ├── KoiKoiCalledEvent.cs
│   │   └── GameEndedEvent.cs
│   ├── Services/
│   │   ├── YakuEvaluator.cs
│   │   ├── ScoreCalculator.cs
│   │   ├── ActionValidator.cs
│   │   ├── KuitsukiHandler.cs
│   │   ├── TeshiHandler.cs
│   │   ├── DrawHandler.cs
│   │   └── DealingValidator.cs
│   └── GameRules.cs
│
├── HanafudaEngine.Facade/
│   ├── KoiKoiGame.cs
│   ├── GameActionResult.cs
│   └── ValidationResult.cs
│
└── HanafudaEngine.Tests/
    ├── Core/
    │   ├── CardTests.cs
    │   └── CardDefinitionsTests.cs
    ├── Domain/
    │   ├── YakuEvaluatorTests.cs
    │   ├── ScoreCalculatorTests.cs
    │   ├── ActionValidatorTests.cs
    │   └── GameStateTests.cs
    ├── Integration/
    │   └── GameFlowTests.cs
    └── TestData/
        └── TestDecks.cs
```

## 付録B: 命名規則

### B.1 C#命名規則
- **クラス名**: PascalCase（例: `GameState`, `YakuEvaluator`）
- **メソッド名**: PascalCase（例: `EvaluateYaku`, `CalculateScore`）
- **プロパティ名**: PascalCase（例: `CurrentPlayer`, `TotalScore`）
- **フィールド名**: _camelCase（例: `_state`, `_yakuEvaluator`）
- **パラメータ名**: camelCase（例: `capturedCards`, `player`）
- **定数名**: PascalCase（例: `MaxTurns`, `DefaultScore`）

### B.2 日本語用語とC#名の対応

| 日本語 | C#名 |
|--------|------|
| 光札 | Hikari |
| 種札 | Tane |
| 短冊札 | Tanzaku |
| カス札 | Kasu |
| 五光 | Goko |
| 四光 | Shiko |
| 雨四光 | AmeShiko |
| 三光 | Sanko |
| 猪鹿蝶 | Inoshikacho |
| 花見で一杯 | Hanami |
| 月見で一杯 | Tsukimi |
| 赤短 | Akatan |
| 青短 | Aotan |
| こいこい | KoiKoi |
| 勝負 | Shobu |
| 喰い付き | Kuitsuki |
| 手四 | Teshi |
| 総流れ | Draw |

## 付録C: 参考資料

### C.1 ゲームルール
- 本プロジェクトの`docs/hanafuda-specification.md`

### C.2 C#設計パターン
- Repository Pattern
- Facade Pattern
- State Pattern（ゲーム状態管理）

### C.3 推奨ライブラリ
- **テスト**: xUnit, NUnit, MSTest
- **モック**: Moq, NSubstitute
- **アサーション**: FluentAssertions
- **DI**: Microsoft.Extensions.DependencyInjection（必要に応じて）

---

**文書バージョン**: 1.0  
**最終更新日**: 2024年  
**作成者**: GitHub Copilot
