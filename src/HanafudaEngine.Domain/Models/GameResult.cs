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
    /// <exception cref="ArgumentNullException">Thrown when scores or completedYaku is null</exception>
    /// <exception cref="ArgumentException">Thrown when isKoiKoiWin is true but winner is null, or when scores contain negative values</exception>
    public GameResult(
        PlayerId? winner,
        IReadOnlyDictionary<PlayerId, int> scores,
        IReadOnlyDictionary<PlayerId, IReadOnlyList<Yaku>> completedYaku,
        bool isKoiKoiWin = false)
    {
        if (scores == null)
            throw new ArgumentNullException(nameof(scores));
        if (completedYaku == null)
            throw new ArgumentNullException(nameof(completedYaku));

        // Validate that isKoiKoiWin can only be true if there's a winner
        if (isKoiKoiWin && winner == null)
            throw new ArgumentException("isKoiKoiWin cannot be true when winner is null.", nameof(isKoiKoiWin));

        // Validate that scores are non-negative
        foreach (var score in scores.Values)
        {
            if (score < 0)
                throw new ArgumentException("Scores cannot contain negative values.", nameof(scores));
        }

        Winner = winner;

        // Defensive copy of scores dictionary
        Scores = new Dictionary<PlayerId, int>(scores);

        // Defensive copy of completedYaku dictionary with deep copy of yaku lists
        var yakuCopy = new Dictionary<PlayerId, IReadOnlyList<Yaku>>();
        foreach (var kvp in completedYaku)
        {
            yakuCopy[kvp.Key] = kvp.Value?.ToList().AsReadOnly() ?? (IReadOnlyList<Yaku>)Array.Empty<Yaku>();
        }
        CompletedYaku = yakuCopy;

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
