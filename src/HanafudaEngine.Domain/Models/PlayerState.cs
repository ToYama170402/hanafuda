using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Domain.Models;

/// <summary>
/// Represents the state of a player in the game
/// This class is immutable - all state changes return a new instance
/// </summary>
public sealed class PlayerState
{
    /// <summary>
    /// The player's identifier
    /// </summary>
    public PlayerId Id { get; }

    /// <summary>
    /// Cards currently in the player's hand
    /// </summary>
    public IReadOnlyList<Card> Hand { get; }

    /// <summary>
    /// Cards that the player has captured
    /// </summary>
    public IReadOnlyList<Card> CapturedCards { get; }

    /// <summary>
    /// Whether the player has called Koi-Koi this round
    /// </summary>
    public bool HasCalledKoiKoi { get; }

    /// <summary>
    /// Yaku (scoring combinations) that the player has completed
    /// </summary>
    public IReadOnlyList<Yaku> CompletedYaku { get; }

    /// <summary>
    /// The player's current score
    /// </summary>
    public int CurrentScore { get; }

    /// <summary>
    /// Creates a new PlayerState instance
    /// </summary>
    /// <param name="id">The player's identifier</param>
    /// <param name="hand">Cards in the player's hand</param>
    /// <param name="capturedCards">Cards the player has captured</param>
    /// <param name="hasCalledKoiKoi">Whether the player has called Koi-Koi</param>
    /// <param name="completedYaku">Yaku the player has completed</param>
    /// <param name="currentScore">The player's current score</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when currentScore is negative</exception>
    public PlayerState(
        PlayerId id,
        IEnumerable<Card>? hand = null,
        IEnumerable<Card>? capturedCards = null,
        bool hasCalledKoiKoi = false,
        IEnumerable<Yaku>? completedYaku = null,
        int currentScore = 0)
    {
        if (currentScore < 0)
            throw new ArgumentOutOfRangeException(nameof(currentScore), "Current score cannot be negative.");

        Id = id;
        Hand = hand?.ToList().AsReadOnly() ?? Array.Empty<Card>().ToList().AsReadOnly();
        CapturedCards = capturedCards?.ToList().AsReadOnly() ?? Array.Empty<Card>().ToList().AsReadOnly();
        HasCalledKoiKoi = hasCalledKoiKoi;
        CompletedYaku = completedYaku?.ToList().AsReadOnly() ?? Array.Empty<Yaku>().ToList().AsReadOnly();
        CurrentScore = currentScore;
    }

    /// <summary>
    /// Returns a new PlayerState with the specified hand
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when newHand is null</exception>
    public PlayerState WithHand(IEnumerable<Card> newHand)
    {
        if (newHand == null)
            throw new ArgumentNullException(nameof(newHand));

        return new PlayerState(
            Id,
            newHand,
            CapturedCards,
            HasCalledKoiKoi,
            CompletedYaku,
            CurrentScore);
    }

    /// <summary>
    /// Returns a new PlayerState with the specified captured cards
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when newCaptured is null</exception>
    public PlayerState WithCapturedCards(IEnumerable<Card> newCaptured)
    {
        if (newCaptured == null)
            throw new ArgumentNullException(nameof(newCaptured));

        return new PlayerState(
            Id,
            Hand,
            newCaptured,
            HasCalledKoiKoi,
            CompletedYaku,
            CurrentScore);
    }

    /// <summary>
    /// Returns a new PlayerState with the specified Koi-Koi called status
    /// </summary>
    public PlayerState WithKoiKoiCalled(bool called)
    {
        return new PlayerState(
            Id,
            Hand,
            CapturedCards,
            called,
            CompletedYaku,
            CurrentScore);
    }

    /// <summary>
    /// Returns a new PlayerState with the specified completed Yaku
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when yaku is null</exception>
    public PlayerState WithCompletedYaku(IEnumerable<Yaku> yaku)
    {
        if (yaku == null)
            throw new ArgumentNullException(nameof(yaku));

        return new PlayerState(
            Id,
            Hand,
            CapturedCards,
            HasCalledKoiKoi,
            yaku,
            CurrentScore);
    }

    /// <summary>
    /// Returns a new PlayerState with the specified current score
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when score is negative</exception>
    public PlayerState WithCurrentScore(int score)
    {
        if (score < 0)
            throw new ArgumentOutOfRangeException(nameof(score), "Current score cannot be negative.");

        return new PlayerState(
            Id,
            Hand,
            CapturedCards,
            HasCalledKoiKoi,
            CompletedYaku,
            score);
    }
}
