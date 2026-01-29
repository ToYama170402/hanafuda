using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Domain.Models;

/// <summary>
/// Represents the result of a completed game
/// </summary>
public sealed class GameResult
{
    /// <summary>
    /// The winner of the game (null if the game ended in a draw)
    /// </summary>
    public PlayerId? Winner { get; }

    /// <summary>
    /// The final scores for each player
    /// </summary>
    public IReadOnlyDictionary<PlayerId, int> Scores { get; }

    /// <summary>
    /// The Yaku (scoring combinations) completed by each player
    /// </summary>
    public IReadOnlyDictionary<PlayerId, IReadOnlyList<Yaku>> CompletedYaku { get; }

    /// <summary>
    /// Whether the winner won with Koi-Koi (doubles the score)
    /// </summary>
    public bool IsKoiKoiWin { get; }

    /// <summary>
    /// Whether the game ended in a draw (Sounagare - total flow)
    /// </summary>
    public bool IsDraw { get; }

    /// <summary>
    /// Creates a new GameResult instance
    /// </summary>
    /// <param name="winner">The winner of the game (null if draw)</param>
    /// <param name="scores">The final scores for each player</param>
    /// <param name="completedYaku">The Yaku completed by each player</param>
    /// <param name="isKoiKoiWin">Whether the winner won with Koi-Koi</param>
    public GameResult(
        PlayerId? winner,
        IReadOnlyDictionary<PlayerId, int> scores,
        IReadOnlyDictionary<PlayerId, IReadOnlyList<Yaku>> completedYaku,
        bool isKoiKoiWin = false)
    {
        Winner = winner;
        Scores = scores ?? throw new ArgumentNullException(nameof(scores));
        CompletedYaku = completedYaku ?? throw new ArgumentNullException(nameof(completedYaku));
        IsKoiKoiWin = isKoiKoiWin;
        IsDraw = winner == null;
    }

    /// <summary>
    /// Creates a GameResult for a draw
    /// </summary>
    public static GameResult CreateDraw(
        IReadOnlyDictionary<PlayerId, int> scores,
        IReadOnlyDictionary<PlayerId, IReadOnlyList<Yaku>> completedYaku)
    {
        return new GameResult(null, scores, completedYaku, false);
    }

    /// <summary>
    /// Creates a GameResult for a winner
    /// </summary>
    public static GameResult CreateWin(
        PlayerId winner,
        IReadOnlyDictionary<PlayerId, int> scores,
        IReadOnlyDictionary<PlayerId, IReadOnlyList<Yaku>> completedYaku,
        bool isKoiKoiWin = false)
    {
        return new GameResult(winner, scores, completedYaku, isKoiKoiWin);
    }
}
