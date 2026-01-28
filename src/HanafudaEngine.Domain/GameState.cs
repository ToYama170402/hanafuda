using HanafudaEngine.Core;

namespace HanafudaEngine.Domain;

/// <summary>
/// ゲーム状態を管理するクラス
/// </summary>
public class GameState
{
    public IReadOnlyList<Card> Deck { get; init; } = new List<Card>();
    public IReadOnlyList<Card> Field { get; init; } = new List<Card>();
    public IReadOnlyDictionary<PlayerId, IReadOnlyList<Card>> Hands { get; init; } = new Dictionary<PlayerId, IReadOnlyList<Card>>();
    public IReadOnlyDictionary<PlayerId, IReadOnlyList<Card>> CapturedCards { get; init; } = new Dictionary<PlayerId, IReadOnlyList<Card>>();
    public PlayerId? CurrentPlayer { get; init; }
}
