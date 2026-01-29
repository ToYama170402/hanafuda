using HanafudaEngine.Core.Models;
using HanafudaEngine.Domain.Models;

namespace HanafudaEngine.Tests.DomainTests;

public class PlayerStateTests
{
    [Fact]
    public void PlayerState_Constructor_ShouldSetPropertiesCorrectly()
    {
        var playerState = new PlayerState(PlayerId.Player1);

        Assert.Equal(PlayerId.Player1, playerState.Id);
        Assert.Empty(playerState.Hand);
        Assert.Empty(playerState.CapturedCards);
        Assert.False(playerState.HasCalledKoiKoi);
        Assert.Empty(playerState.CompletedYaku);
        Assert.Equal(0, playerState.CurrentScore);
    }

    [Fact]
    public void PlayerState_Constructor_WithAllParameters_ShouldSetPropertiesCorrectly()
    {
        var hand = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴")
        };
        var captured = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.February, CardType.Tane, "梅に鶯")
        };
        var yakuCards = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.March, CardType.Hikari, "桜に幕")
        };
        var yaku = new List<Yaku>
        {
            new Yaku(YakuType.Sanko, 5, 0, yakuCards)
        };

        var playerState = new PlayerState(
            PlayerId.Player2,
            hand,
            captured,
            true,
            yaku,
            10);

        Assert.Equal(PlayerId.Player2, playerState.Id);
        Assert.Single(playerState.Hand);
        Assert.Single(playerState.CapturedCards);
        Assert.True(playerState.HasCalledKoiKoi);
        Assert.Single(playerState.CompletedYaku);
        Assert.Equal(10, playerState.CurrentScore);
    }

    [Fact]
    public void PlayerState_WithHand_ShouldReturnNewInstanceWithUpdatedHand()
    {
        var originalState = new PlayerState(PlayerId.Player1);
        var newHand = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.March, CardType.Hikari, "桜に幕")
        };

        var newState = originalState.WithHand(newHand);

        Assert.NotSame(originalState, newState);
        Assert.Empty(originalState.Hand);
        Assert.Single(newState.Hand);
        Assert.Equal(PlayerId.Player1, newState.Id);
    }

    [Fact]
    public void PlayerState_WithCapturedCards_ShouldReturnNewInstanceWithUpdatedCaptured()
    {
        var originalState = new PlayerState(PlayerId.Player1);
        var newCaptured = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.April, CardType.Tane, "藤に不如帰")
        };

        var newState = originalState.WithCapturedCards(newCaptured);

        Assert.NotSame(originalState, newState);
        Assert.Empty(originalState.CapturedCards);
        Assert.Single(newState.CapturedCards);
        Assert.Equal(PlayerId.Player1, newState.Id);
    }

    [Fact]
    public void PlayerState_WithKoiKoiCalled_ShouldReturnNewInstanceWithUpdatedFlag()
    {
        var originalState = new PlayerState(PlayerId.Player1);

        var newState = originalState.WithKoiKoiCalled(true);

        Assert.NotSame(originalState, newState);
        Assert.False(originalState.HasCalledKoiKoi);
        Assert.True(newState.HasCalledKoiKoi);
    }

    [Fact]
    public void PlayerState_WithCompletedYaku_ShouldReturnNewInstanceWithUpdatedYaku()
    {
        var originalState = new PlayerState(PlayerId.Player1);
        var yakuCards = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴"),
            new Card(Guid.NewGuid(), Month.March, CardType.Hikari, "桜に幕"),
            new Card(Guid.NewGuid(), Month.August, CardType.Hikari, "芒に月")
        };
        var yaku = new List<Yaku>
        {
            new Yaku(YakuType.Goko, 15, 0, yakuCards)
        };

        var newState = originalState.WithCompletedYaku(yaku);

        Assert.NotSame(originalState, newState);
        Assert.Empty(originalState.CompletedYaku);
        Assert.Single(newState.CompletedYaku);
        Assert.Equal(YakuType.Goko, newState.CompletedYaku[0].Type);
    }

    [Fact]
    public void PlayerState_WithCurrentScore_ShouldReturnNewInstanceWithUpdatedScore()
    {
        var originalState = new PlayerState(PlayerId.Player1);

        var newState = originalState.WithCurrentScore(25);

        Assert.NotSame(originalState, newState);
        Assert.Equal(0, originalState.CurrentScore);
        Assert.Equal(25, newState.CurrentScore);
    }

    [Fact]
    public void PlayerState_ImmutableCollections_ShouldNotAllowModification()
    {
        var hand = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.May, CardType.Tanzaku, "菖蒲に短冊")
        };
        var playerState = new PlayerState(PlayerId.Player1, hand);

        // Modifying original collection should not affect PlayerState
        hand.Add(new Card(Guid.NewGuid(), Month.June, CardType.Tane, "牡丹に蝶"));

        Assert.Single(playerState.Hand);
    }

    [Fact]
    public void PlayerState_ChainedWithMethods_ShouldWorkCorrectly()
    {
        var card1 = new Card(Guid.NewGuid(), Month.July, CardType.Tane, "萩に猪");
        var card2 = new Card(Guid.NewGuid(), Month.August, CardType.Hikari, "芒に月");

        var state = new PlayerState(PlayerId.Player2)
            .WithHand(new[] { card1 })
            .WithCapturedCards(new[] { card2 })
            .WithKoiKoiCalled(true)
            .WithCurrentScore(15);

        Assert.Equal(PlayerId.Player2, state.Id);
        Assert.Single(state.Hand);
        Assert.Single(state.CapturedCards);
        Assert.True(state.HasCalledKoiKoi);
        Assert.Equal(15, state.CurrentScore);
    }
}
