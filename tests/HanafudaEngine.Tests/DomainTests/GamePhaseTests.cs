using HanafudaEngine.Domain.Models;

namespace HanafudaEngine.Tests.DomainTests;

public class GamePhaseTests
{
    [Fact]
    public void GamePhase_ShouldHaveAllExpectedValues()
    {
        var values = Enum.GetValues<GamePhase>();

        Assert.Contains(GamePhase.NotStarted, values);
        Assert.Contains(GamePhase.Dealing, values);
        Assert.Contains(GamePhase.PlayerTurn, values);
        Assert.Contains(GamePhase.DrawFromDeck, values);
        Assert.Contains(GamePhase.YakuCheck, values);
        Assert.Contains(GamePhase.KoiKoiDecision, values);
        Assert.Contains(GamePhase.GameOver, values);
        Assert.Contains(GamePhase.Draw, values);
    }

    [Theory]
    [InlineData(GamePhase.NotStarted)]
    [InlineData(GamePhase.Dealing)]
    [InlineData(GamePhase.PlayerTurn)]
    [InlineData(GamePhase.DrawFromDeck)]
    [InlineData(GamePhase.YakuCheck)]
    [InlineData(GamePhase.KoiKoiDecision)]
    [InlineData(GamePhase.GameOver)]
    [InlineData(GamePhase.Draw)]
    public void GamePhase_AllValues_ShouldBeValid(GamePhase phase)
    {
        Assert.True(Enum.IsDefined(typeof(GamePhase), phase));
    }
}
