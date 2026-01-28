using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Tests.CoreTests;

public class PlayerIdTests
{
    [Fact]
    public void PlayerId_ShouldHaveCorrectValues()
    {
        Assert.Equal(0, (int)PlayerId.Player1);
        Assert.Equal(1, (int)PlayerId.Player2);
    }
    
    [Fact]
    public void PlayerId_ShouldHave2Values()
    {
        var players = Enum.GetValues<PlayerId>();
        Assert.Equal(2, players.Length);
    }
}
