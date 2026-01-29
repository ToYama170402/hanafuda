using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Tests.CoreTests;

public class SpecialCardFlagTests
{
    [Fact]
    public void SpecialCardFlag_None_ShouldBeZero()
    {
        Assert.Equal(0, (int)SpecialCardFlag.None);
    }

    [Fact]
    public void SpecialCardFlag_ShouldBePowerOfTwo()
    {
        Assert.Equal(1, (int)SpecialCardFlag.RedPoetry);
        Assert.Equal(2, (int)SpecialCardFlag.BluePoetry);
        Assert.Equal(4, (int)SpecialCardFlag.SakeCup);
        Assert.Equal(8, (int)SpecialCardFlag.Deer);
        Assert.Equal(16, (int)SpecialCardFlag.Boar);
        Assert.Equal(32, (int)SpecialCardFlag.Butterfly);
        Assert.Equal(64, (int)SpecialCardFlag.RainMan);
    }

    [Fact]
    public void SpecialCardFlag_CanBeCombined()
    {
        var combined = SpecialCardFlag.RedPoetry | SpecialCardFlag.BluePoetry;
        Assert.True(combined.HasFlag(SpecialCardFlag.RedPoetry));
        Assert.True(combined.HasFlag(SpecialCardFlag.BluePoetry));
        Assert.False(combined.HasFlag(SpecialCardFlag.SakeCup));
    }
}
