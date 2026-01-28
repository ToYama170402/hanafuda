namespace HanafudaEngine.Core.Models;

/// <summary>
/// Represents the type of Hanafuda card
/// </summary>
public enum CardType
{
    /// <summary>光札 (Bright cards) - 5 cards</summary>
    Hikari,
    
    /// <summary>種札 (Animal cards) - 9 cards</summary>
    Tane,
    
    /// <summary>短冊札 (Ribbon cards) - 10 cards</summary>
    Tanzaku,
    
    /// <summary>カス札 (Plain cards) - 24 cards</summary>
    Kasu
}
