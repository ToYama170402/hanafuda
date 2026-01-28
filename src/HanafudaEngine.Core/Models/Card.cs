namespace HanafudaEngine.Core.Models;

/// <summary>
/// Represents a single Hanafuda card
/// </summary>
public sealed class Card : IEquatable<Card>
{
    /// <summary>
    /// Unique identifier for the card instance
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// The month this card belongs to
    /// </summary>
    public Month Month { get; }
    
    /// <summary>
    /// The type of card
    /// </summary>
    public CardType Type { get; }
    
    /// <summary>
    /// The name of the card (e.g., "松に鶴")
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Indicates if this card has special flags
    /// </summary>
    public bool IsSpecial => SpecialFlag.HasValue;
    
    /// <summary>
    /// Special flags for this card (e.g., RedPoetry, BluePoetry)
    /// </summary>
    public SpecialCardFlag? SpecialFlag { get; }
    
    /// <summary>
    /// Creates a new Card instance
    /// </summary>
    public Card(Guid id, Month month, CardType type, string name, SpecialCardFlag? specialFlag = null)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Card Id cannot be Guid.Empty.", nameof(id));
        
        Id = id;
        Month = month;
        Type = type;
        
        if (name == null)
            throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Card name cannot be empty or whitespace.", nameof(name));
        
        Name = name;
        SpecialFlag = specialFlag;
    }
    
    /// <summary>
    /// Determines whether the specified Card is equal to the current Card based on Id
    /// </summary>
    public bool Equals(Card? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }
    
    /// <summary>
    /// Determines whether the specified object is equal to the current Card
    /// </summary>
    public override bool Equals(object? obj)
    {
        return Equals(obj as Card);
    }
    
    /// <summary>
    /// Returns a hash code for this Card based on Id
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    /// <summary>
    /// Returns a string representation of this Card
    /// </summary>
    public override string ToString()
    {
        return $"{Name} ({Month}, {Type})";
    }
    
    public static bool operator ==(Card? left, Card? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }
    
    public static bool operator !=(Card? left, Card? right)
    {
        return !(left == right);
    }
}
