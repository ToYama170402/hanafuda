using HanafudaEngine.Core;

namespace HanafudaEngine.Tests;

public class CoreTests
{
    [Fact]
    public void Card_CanBeCreated()
    {
        // Arrange & Act
        var card = new Card(Month.January, CardType.Hikari, "松に鶴");

        // Assert
        Assert.Equal(Month.January, card.Month);
        Assert.Equal(CardType.Hikari, card.Type);
        Assert.Equal("松に鶴", card.Name);
    }

    [Fact]
    public void PlayerId_CanBeCreated()
    {
        // Arrange & Act
        var playerId = new PlayerId("Player1");

        // Assert
        Assert.Equal("Player1", playerId.Value);
    }
}
