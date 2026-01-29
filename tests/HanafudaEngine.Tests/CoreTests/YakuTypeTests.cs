using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Tests.CoreTests;

public class YakuTypeTests
{
    [Fact]
    public void YakuType_ShouldHave12Values()
    {
        var types = Enum.GetValues<YakuType>();
        Assert.Equal(12, types.Length);
    }

    [Fact]
    public void YakuType_ShouldContainAllBrightCardYaku()
    {
        var types = Enum.GetValues<YakuType>();
        Assert.Contains(YakuType.Goko, types);
        Assert.Contains(YakuType.Shiko, types);
        Assert.Contains(YakuType.AmeShiko, types);
        Assert.Contains(YakuType.Sanko, types);
    }

    [Fact]
    public void YakuType_ShouldContainAllAnimalCardYaku()
    {
        var types = Enum.GetValues<YakuType>();
        Assert.Contains(YakuType.Inoshikacho, types);
        Assert.Contains(YakuType.Hanami, types);
        Assert.Contains(YakuType.Tsukimi, types);
        Assert.Contains(YakuType.Tane, types);
    }

    [Fact]
    public void YakuType_ShouldContainAllRibbonCardYaku()
    {
        var types = Enum.GetValues<YakuType>();
        Assert.Contains(YakuType.Akatan, types);
        Assert.Contains(YakuType.Aotan, types);
        Assert.Contains(YakuType.Tanzaku, types);
    }

    [Fact]
    public void YakuType_ShouldContainPlainCardYaku()
    {
        var types = Enum.GetValues<YakuType>();
        Assert.Contains(YakuType.Kasu, types);
    }
}
