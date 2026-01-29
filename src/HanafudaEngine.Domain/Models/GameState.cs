using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Domain.Models;

/// <summary>
/// Represents the complete state of the game
/// This class is immutable - all state changes return a new instance
/// </summary>
public sealed class GameState
{
    /// <summary>
    /// Unique identifier for this game instance
    /// </summary>
    public Guid GameId { get; }

    /// <summary>
    /// The current phase of the game
    /// </summary>
    public GamePhase Phase { get; }

    /// <summary>
    /// The current phase within a turn
    /// </summary>
    public TurnPhase TurnPhase { get; }

    /// <summary>
    /// The player whose turn it currently is
    /// </summary>
    public PlayerId CurrentPlayer { get; }

    /// <summary>
    /// The dealer (parent/Oya) for this round
    /// </summary>
    public PlayerId Dealer { get; }

    /// <summary>
    /// The current turn count
    /// </summary>
    public int TurnCount { get; }

    /// <summary>
    /// Cards remaining in the deck
    /// </summary>
    public IReadOnlyList<Card> Deck { get; }

    /// <summary>
    /// Cards currently on the field
    /// </summary>
    public IReadOnlyList<Card> Field { get; }

    /// <summary>
    /// The state of each player
    /// </summary>
    public IReadOnlyDictionary<PlayerId, PlayerState> Players { get; }

    /// <summary>
    /// The last card played from a player's hand
    /// </summary>
    public Card? LastPlayedCard { get; }

    /// <summary>
    /// The last card drawn from the deck
    /// </summary>
    public Card? LastDrawnCard { get; }

    /// <summary>
    /// Cards waiting to be captured (when player needs to select from multiple matching cards)
    /// </summary>
    public IReadOnlyList<Card> PendingCapture { get; }

    /// <summary>
    /// The winner of the game (null if game is not over)
    /// </summary>
    public PlayerId? Winner { get; }

    /// <summary>
    /// The result of the game (null if game is not over)
    /// </summary>
    public GameResult? Result { get; }

    /// <summary>
    /// Creates a new GameState instance
    /// </summary>
    public GameState(
        Guid gameId,
        GamePhase phase,
        TurnPhase turnPhase,
        PlayerId currentPlayer,
        PlayerId dealer,
        int turnCount,
        IEnumerable<Card>? deck = null,
        IEnumerable<Card>? field = null,
        IReadOnlyDictionary<PlayerId, PlayerState>? players = null,
        Card? lastPlayedCard = null,
        Card? lastDrawnCard = null,
        IEnumerable<Card>? pendingCapture = null,
        PlayerId? winner = null,
        GameResult? result = null)
    {
        if (gameId == Guid.Empty)
            throw new ArgumentException("GameId cannot be Guid.Empty.", nameof(gameId));

        if (turnCount < 0)
            throw new ArgumentOutOfRangeException(nameof(turnCount), "Turn count cannot be negative.");

        GameId = gameId;
        Phase = phase;
        TurnPhase = turnPhase;
        CurrentPlayer = currentPlayer;
        Dealer = dealer;
        TurnCount = turnCount;
        Deck = deck?.ToList().AsReadOnly() ?? Array.Empty<Card>().ToList().AsReadOnly();
        Field = field?.ToList().AsReadOnly() ?? Array.Empty<Card>().ToList().AsReadOnly();
        Players = players ?? new Dictionary<PlayerId, PlayerState>
        {
            { PlayerId.Player1, new PlayerState(PlayerId.Player1) },
            { PlayerId.Player2, new PlayerState(PlayerId.Player2) }
        };
        LastPlayedCard = lastPlayedCard;
        LastDrawnCard = lastDrawnCard;
        PendingCapture = pendingCapture?.ToList().AsReadOnly() ?? Array.Empty<Card>().ToList().AsReadOnly();
        Winner = winner;
        Result = result;
    }

    /// <summary>
    /// Returns a new GameState with the specified game phase
    /// </summary>
    public GameState WithPhase(GamePhase phase)
    {
        return new GameState(
            GameId, phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, Field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified turn phase
    /// </summary>
    public GameState WithTurnPhase(TurnPhase turnPhase)
    {
        return new GameState(
            GameId, Phase, turnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, Field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified current player
    /// </summary>
    public GameState WithCurrentPlayer(PlayerId player)
    {
        return new GameState(
            GameId, Phase, TurnPhase, player, Dealer, TurnCount,
            Deck, Field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified dealer
    /// </summary>
    public GameState WithDealer(PlayerId dealer)
    {
        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, dealer, TurnCount,
            Deck, Field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified turn count
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when turnCount is negative</exception>
    public GameState WithTurnCount(int turnCount)
    {
        if (turnCount < 0)
            throw new ArgumentOutOfRangeException(nameof(turnCount), "Turn count cannot be negative.");

        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, turnCount,
            Deck, Field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified deck
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when deck is null</exception>
    public GameState WithDeck(IEnumerable<Card> deck)
    {
        if (deck == null)
            throw new ArgumentNullException(nameof(deck));

        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            deck, Field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified field
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when field is null</exception>
    public GameState WithField(IEnumerable<Card> field)
    {
        if (field == null)
            throw new ArgumentNullException(nameof(field));

        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified player state
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when playerState is null</exception>
    public GameState WithPlayerState(PlayerId playerId, PlayerState playerState)
    {
        if (playerState == null)
            throw new ArgumentNullException(nameof(playerState));

        var newPlayers = new Dictionary<PlayerId, PlayerState>(Players)
        {
            [playerId] = playerState
        };

        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, Field, newPlayers, LastPlayedCard, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified last played card
    /// </summary>
    public GameState WithLastPlayedCard(Card? card)
    {
        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, Field, Players, card, LastDrawnCard, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified last drawn card
    /// </summary>
    public GameState WithLastDrawnCard(Card? card)
    {
        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, Field, Players, LastPlayedCard, card, PendingCapture, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified pending capture cards
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when cards is null</exception>
    public GameState WithPendingCapture(IEnumerable<Card> cards)
    {
        if (cards == null)
            throw new ArgumentNullException(nameof(cards));

        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, Field, Players, LastPlayedCard, LastDrawnCard, cards, Winner, Result);
    }

    /// <summary>
    /// Returns a new GameState with the specified winner and result
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when result is null</exception>
    public GameState WithWinner(PlayerId winner, GameResult result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));

        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, Field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, winner, result);
    }

    /// <summary>
    /// Returns a new GameState with the specified result (for draw)
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when result is null</exception>
    public GameState WithResult(GameResult result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));

        return new GameState(
            GameId, Phase, TurnPhase, CurrentPlayer, Dealer, TurnCount,
            Deck, Field, Players, LastPlayedCard, LastDrawnCard, PendingCapture, result.Winner, result);
    }
}
