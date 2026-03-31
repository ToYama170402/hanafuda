namespace HanafudaEngine.Domain.Models;

/// <summary>
/// Represents the main phases of the game
/// </summary>
public enum GamePhase
{
    /// <summary>Game has not started yet</summary>
    NotStarted,

    /// <summary>Cards are being dealt</summary>
    Dealing,

    /// <summary>Player is taking their turn</summary>
    PlayerTurn,

    /// <summary>Drawing from the deck</summary>
    DrawFromDeck,

    /// <summary>Checking for completed Yaku (scoring combinations)</summary>
    YakuCheck,

    /// <summary>Player is deciding whether to call Koi-Koi or end the game</summary>
    KoiKoiDecision,

    /// <summary>Game has ended with a winner</summary>
    GameOver,

    /// <summary>Game ended in a draw (Sounagare)</summary>
    Draw
}
