using HanafudaEngine.Core;

namespace HanafudaEngine.Domain;

/// <summary>
/// ゲーム状態を管理するクラス
/// </summary>
public class GameState
{
    public List<Card> Deck { get; init; } = new();
    public List<Card> Field { get; init; } = new();
    public Dictionary<PlayerId, List<Card>> Hands { get; init; } = new();
    public Dictionary<PlayerId, List<Card>> CapturedCards { get; init; } = new();
    public PlayerId CurrentPlayer { get; init; } = new PlayerId("Player1");
}
