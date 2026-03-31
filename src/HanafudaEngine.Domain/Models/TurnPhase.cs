namespace HanafudaEngine.Domain.Models;

/// <summary>
/// Represents the detailed phases within a player's turn
/// </summary>
public enum TurnPhase
{
    /// <summary>Player is selecting a card from their hand to play</summary>
    PlayFromHand,

    /// <summary>Player is selecting a field card to capture (when multiple cards of the same month exist)</summary>
    SelectFieldCard,

    /// <summary>Player is drawing a card from the deck</summary>
    DrawFromDeck,

    /// <summary>Player is selecting a field card for the drawn card (when multiple cards of the same month exist)</summary>
    SelectFieldCardForDraw,

    /// <summary>Checking for completed Yaku (scoring combinations)</summary>
    YakuCheck,

    /// <summary>Player is deciding whether to call Koi-Koi or end the game</summary>
    KoiKoiDecision,

    /// <summary>Turn has ended</summary>
    TurnEnd
}
