using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Tests.CoreTests;

public class CardTests
{
    [Fact]
    public void Card_Constructor_ShouldSetPropertiesCorrectly()
    {
        var id = Guid.NewGuid();
        var card = new Card(id, Month.January, CardType.Hikari, "松に鶴", SpecialCardFlag.None);
        
        Assert.Equal(id, card.Id);
        Assert.Equal(Month.January, card.Month);
        Assert.Equal(CardType.Hikari, card.Type);
        Assert.Equal("松に鶴", card.Name);
        Assert.Equal(SpecialCardFlag.None, card.SpecialFlag);
        Assert.False(card.IsSpecial);
    }
    
    [Fact]
    public void Card_Constructor_WithSpecialFlag_ShouldSetIsSpecialTrue()
    {
        var id = Guid.NewGuid();
        var card = new Card(id, Month.January, CardType.Tanzaku, "松に赤短", SpecialCardFlag.RedPoetry);
        
        Assert.Equal(SpecialCardFlag.RedPoetry, card.SpecialFlag);
        Assert.True(card.IsSpecial);
    }
    
    [Fact]
    public void Card_Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        var id = Guid.NewGuid();
        Assert.Throws<ArgumentNullException>(() => 
            new Card(id, Month.January, CardType.Hikari, null!));
    }
    
    [Fact]
    public void Card_Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => 
            new Card(Guid.Empty, Month.January, CardType.Hikari, "松に鶴"));
    }
    
    [Fact]
    public void Card_Constructor_WithEmptyName_ShouldThrowArgumentException()
    {
        var id = Guid.NewGuid();
        Assert.Throws<ArgumentException>(() => 
            new Card(id, Month.January, CardType.Hikari, ""));
    }
    
    [Fact]
    public void Card_Constructor_WithWhitespaceName_ShouldThrowArgumentException()
    {
        var id = Guid.NewGuid();
        Assert.Throws<ArgumentException>(() => 
            new Card(id, Month.January, CardType.Hikari, "   "));
    }
    
    [Fact]
    public void Card_Equals_WithSameId_ShouldReturnTrue()
    {
        var id = Guid.NewGuid();
        var card1 = new Card(id, Month.January, CardType.Hikari, "松に鶴");
        var card2 = new Card(id, Month.February, CardType.Tane, "Different Name");
        
        Assert.True(card1.Equals(card2));
        Assert.Equal(card1, card2);
    }
    
    [Fact]
    public void Card_Equals_WithDifferentId_ShouldReturnFalse()
    {
        var card1 = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        var card2 = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        
        Assert.False(card1.Equals(card2));
        Assert.NotEqual(card1, card2);
    }
    
    [Fact]
    public void Card_Equals_WithNull_ShouldReturnFalse()
    {
        var card = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        
        Assert.False(card.Equals(null));
    }
    
    [Fact]
    public void Card_GetHashCode_WithSameId_ShouldReturnSameHashCode()
    {
        var id = Guid.NewGuid();
        var card1 = new Card(id, Month.January, CardType.Hikari, "松に鶴");
        var card2 = new Card(id, Month.February, CardType.Tane, "Different Name");
        
        Assert.Equal(card1.GetHashCode(), card2.GetHashCode());
    }
    
    [Fact]
    public void Card_OperatorEquals_ShouldWorkCorrectly()
    {
        var id = Guid.NewGuid();
        var card1 = new Card(id, Month.January, CardType.Hikari, "松に鶴");
        var card2 = new Card(id, Month.January, CardType.Hikari, "松に鶴");
        var card3 = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        
        Assert.True(card1 == card2);
        Assert.False(card1 == card3);
    }
    
    [Fact]
    public void Card_OperatorNotEquals_ShouldWorkCorrectly()
    {
        var id = Guid.NewGuid();
        var card1 = new Card(id, Month.January, CardType.Hikari, "松に鶴");
        var card2 = new Card(id, Month.January, CardType.Hikari, "松に鶴");
        var card3 = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        
        Assert.False(card1 != card2);
        Assert.True(card1 != card3);
    }
    
    [Fact]
    public void Card_ToString_ShouldReturnFormattedString()
    {
        var card = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        var result = card.ToString();
        
        Assert.Contains("松に鶴", result);
        Assert.Contains("January", result);
        Assert.Contains("Hikari", result);
    }
}
