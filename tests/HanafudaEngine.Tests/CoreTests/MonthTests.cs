using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Tests.CoreTests;

public class MonthTests
{
    [Fact]
    public void Month_ShouldHaveCorrectValues()
    {
        Assert.Equal(1, (int)Month.January);
        Assert.Equal(2, (int)Month.February);
        Assert.Equal(3, (int)Month.March);
        Assert.Equal(4, (int)Month.April);
        Assert.Equal(5, (int)Month.May);
        Assert.Equal(6, (int)Month.June);
        Assert.Equal(7, (int)Month.July);
        Assert.Equal(8, (int)Month.August);
        Assert.Equal(9, (int)Month.September);
        Assert.Equal(10, (int)Month.October);
        Assert.Equal(11, (int)Month.November);
        Assert.Equal(12, (int)Month.December);
    }
    
    [Fact]
    public void Month_ShouldHave12Values()
    {
        var months = Enum.GetValues<Month>();
        Assert.Equal(12, months.Length);
    }
}
