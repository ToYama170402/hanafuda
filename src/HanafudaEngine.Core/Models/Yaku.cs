namespace HanafudaEngine.Core.Models;

/// <summary>
/// Represents a completed yaku (combination) in Koi-Koi
/// </summary>
public sealed class Yaku
{
    /// <summary>
    /// The type of yaku
    /// </summary>
    public YakuType Type { get; }
    
    /// <summary>
    /// The base score for this yaku
    /// </summary>
    public int BaseScore { get; }
    
    /// <summary>
    /// Bonus score for extra cards beyond the minimum requirement
    /// </summary>
    public int BonusScore { get; }
    
    /// <summary>
    /// Total score (BaseScore + BonusScore)
    /// </summary>
    public int TotalScore => BaseScore + BonusScore;
    
    /// <summary>
    /// The cards that make up this yaku
    /// </summary>
    public IReadOnlyList<Card> Cards { get; }
    
    /// <summary>
    /// Creates a new Yaku instance
    /// </summary>
    /// <param name="type">The type of yaku</param>
    /// <param name="baseScore">The base score</param>
    /// <param name="bonusScore">The bonus score for extra cards</param>
    /// <param name="cards">The cards that make up this yaku</param>
    public Yaku(YakuType type, int baseScore, int bonusScore, IEnumerable<Card> cards)
    {
        if (cards == null)
            throw new ArgumentNullException(nameof(cards));
        
        if (baseScore < 0)
            throw new ArgumentOutOfRangeException(nameof(baseScore), "Base score cannot be negative.");
        
        if (bonusScore < 0)
            throw new ArgumentOutOfRangeException(nameof(bonusScore), "Bonus score cannot be negative.");
        
        var cardList = cards.ToList();
        if (cardList.Count == 0)
            throw new ArgumentException("A yaku must consist of at least one card.", nameof(cards));
        
        Type = type;
        BaseScore = baseScore;
        BonusScore = bonusScore;
        Cards = cardList.AsReadOnly();
    }
    
    /// <summary>
    /// Returns a string representation of this Yaku
    /// </summary>
    public override string ToString()
    {
        return $"{Type}: {TotalScore} points ({BaseScore} + {BonusScore}) [{Cards.Count} cards]";
    }
}
