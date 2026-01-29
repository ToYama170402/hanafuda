using HanafudaEngine.Domain.Models;

namespace HanafudaEngine.Tests.DomainTests;

public class TurnPhaseTests
{
    [Fact]
    public void TurnPhase_ShouldHaveAllExpectedValues()
    {
        var values = Enum.GetValues<TurnPhase>();

        Assert.Contains(TurnPhase.PlayFromHand, values);
        Assert.Contains(TurnPhase.SelectFieldCard, values);
        Assert.Contains(TurnPhase.DrawFromDeck, values);
        Assert.Contains(TurnPhase.SelectFieldCardForDraw, values);
        Assert.Contains(TurnPhase.YakuCheck, values);
        Assert.Contains(TurnPhase.KoiKoiDecision, values);
        Assert.Contains(TurnPhase.TurnEnd, values);
    }

    [Theory]
    [InlineData(TurnPhase.PlayFromHand)]
    [InlineData(TurnPhase.SelectFieldCard)]
    [InlineData(TurnPhase.DrawFromDeck)]
    [InlineData(TurnPhase.SelectFieldCardForDraw)]
    [InlineData(TurnPhase.YakuCheck)]
    [InlineData(TurnPhase.KoiKoiDecision)]
    [InlineData(TurnPhase.TurnEnd)]
    public void TurnPhase_AllValues_ShouldBeValid(TurnPhase phase)
    {
        Assert.True(Enum.IsDefined(typeof(TurnPhase), phase));
    }
}
