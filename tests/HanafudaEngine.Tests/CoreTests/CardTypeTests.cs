using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Tests.CoreTests;

public class CardTypeTests
{
    [Fact]
    public void CardType_ShouldHave4Values()
    {
        var types = Enum.GetValues<CardType>();
        Assert.Equal(4, types.Length);
    }
    
    [Fact]
    public void CardType_ShouldContainAllTypes()
    {
        var types = Enum.GetValues<CardType>();
        Assert.Contains(CardType.Hikari, types);
        Assert.Contains(CardType.Tane, types);
        Assert.Contains(CardType.Tanzaku, types);
        Assert.Contains(CardType.Kasu, types);
    }
}
