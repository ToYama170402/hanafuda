using HanafudaEngine.Core.Models;
using HanafudaEngine.Domain.Models;

namespace HanafudaEngine.Tests.DomainTests;

public class GameStateTests
{
    [Fact]
    public void GameState_Constructor_WithMinimalParameters_ShouldSetDefaultValues()
    {
        var gameId = Guid.NewGuid();
        var gameState = new GameState(
            gameId,
            GamePhase.NotStarted,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        Assert.Equal(gameId, gameState.GameId);
        Assert.Equal(GamePhase.NotStarted, gameState.Phase);
        Assert.Equal(TurnPhase.PlayFromHand, gameState.TurnPhase);
        Assert.Equal(PlayerId.Player1, gameState.CurrentPlayer);
        Assert.Equal(PlayerId.Player1, gameState.Dealer);
        Assert.Equal(0, gameState.TurnCount);
        Assert.Empty(gameState.Deck);
        Assert.Empty(gameState.Field);
        Assert.NotNull(gameState.Players);
        Assert.Equal(2, gameState.Players.Count);
        Assert.Null(gameState.LastPlayedCard);
        Assert.Null(gameState.LastDrawnCard);
        Assert.Empty(gameState.PendingCapture);
        Assert.Null(gameState.Winner);
        Assert.Null(gameState.Result);
    }

    [Fact]
    public void GameState_Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new GameState(
                Guid.Empty,
                GamePhase.NotStarted,
                TurnPhase.PlayFromHand,
                PlayerId.Player1,
                PlayerId.Player1,
                0));
    }

    [Fact]
    public void GameState_Constructor_WithAllParameters_ShouldSetPropertiesCorrectly()
    {
        var gameId = Guid.NewGuid();
        var deck = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴")
        };
        var field = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.February, CardType.Tane, "梅に鶯")
        };
        var players = new Dictionary<PlayerId, PlayerState>
        {
            { PlayerId.Player1, new PlayerState(PlayerId.Player1) },
            { PlayerId.Player2, new PlayerState(PlayerId.Player2) }
        };
        var lastPlayed = new Card(Guid.NewGuid(), Month.March, CardType.Tanzaku, "桜に短冊");
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 10 },
            { PlayerId.Player2, 5 }
        };
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku>() },
            { PlayerId.Player2, new List<Yaku>() }
        };
        var result = new GameResult(PlayerId.Player1, scores, yaku);

        var gameState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.DrawFromDeck,
            PlayerId.Player2,
            PlayerId.Player1,
            5,
            deck,
            field,
            players,
            lastPlayed,
            null,
            null,
            PlayerId.Player1,
            result);

        Assert.Equal(gameId, gameState.GameId);
        Assert.Equal(GamePhase.PlayerTurn, gameState.Phase);
        Assert.Equal(TurnPhase.DrawFromDeck, gameState.TurnPhase);
        Assert.Equal(PlayerId.Player2, gameState.CurrentPlayer);
        Assert.Equal(PlayerId.Player1, gameState.Dealer);
        Assert.Equal(5, gameState.TurnCount);
        Assert.Single(gameState.Deck);
        Assert.Single(gameState.Field);
        Assert.Equal(lastPlayed, gameState.LastPlayedCard);
        Assert.Equal(PlayerId.Player1, gameState.Winner);
        Assert.Equal(result, gameState.Result);
    }

    [Fact]
    public void GameState_WithPhase_ShouldReturnNewInstanceWithUpdatedPhase()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.NotStarted,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        var newState = originalState.WithPhase(GamePhase.Dealing);

        Assert.NotSame(originalState, newState);
        Assert.Equal(GamePhase.NotStarted, originalState.Phase);
        Assert.Equal(GamePhase.Dealing, newState.Phase);
        Assert.Equal(gameId, newState.GameId);
    }

    [Fact]
    public void GameState_WithTurnPhase_ShouldReturnNewInstanceWithUpdatedTurnPhase()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        var newState = originalState.WithTurnPhase(TurnPhase.DrawFromDeck);

        Assert.NotSame(originalState, newState);
        Assert.Equal(TurnPhase.PlayFromHand, originalState.TurnPhase);
        Assert.Equal(TurnPhase.DrawFromDeck, newState.TurnPhase);
    }

    [Fact]
    public void GameState_WithCurrentPlayer_ShouldReturnNewInstanceWithUpdatedPlayer()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        var newState = originalState.WithCurrentPlayer(PlayerId.Player2);

        Assert.NotSame(originalState, newState);
        Assert.Equal(PlayerId.Player1, originalState.CurrentPlayer);
        Assert.Equal(PlayerId.Player2, newState.CurrentPlayer);
    }

    [Fact]
    public void GameState_WithDealer_ShouldReturnNewInstanceWithUpdatedDealer()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.NotStarted,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        var newState = originalState.WithDealer(PlayerId.Player2);

        Assert.NotSame(originalState, newState);
        Assert.Equal(PlayerId.Player1, originalState.Dealer);
        Assert.Equal(PlayerId.Player2, newState.Dealer);
    }

    [Fact]
    public void GameState_WithTurnCount_ShouldReturnNewInstanceWithUpdatedCount()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        var newState = originalState.WithTurnCount(3);

        Assert.NotSame(originalState, newState);
        Assert.Equal(0, originalState.TurnCount);
        Assert.Equal(3, newState.TurnCount);
    }

    [Fact]
    public void GameState_WithDeck_ShouldReturnNewInstanceWithUpdatedDeck()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);
        var newDeck = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.April, CardType.Tane, "藤に不如帰")
        };

        var newState = originalState.WithDeck(newDeck);

        Assert.NotSame(originalState, newState);
        Assert.Empty(originalState.Deck);
        Assert.Single(newState.Deck);
    }

    [Fact]
    public void GameState_WithField_ShouldReturnNewInstanceWithUpdatedField()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);
        var newField = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.May, CardType.Tanzaku, "菖蒲に短冊")
        };

        var newState = originalState.WithField(newField);

        Assert.NotSame(originalState, newState);
        Assert.Empty(originalState.Field);
        Assert.Single(newState.Field);
    }

    [Fact]
    public void GameState_WithPlayerState_ShouldReturnNewInstanceWithUpdatedPlayerState()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);
        var newPlayerState = new PlayerState(PlayerId.Player1)
            .WithCurrentScore(15);

        var newState = originalState.WithPlayerState(PlayerId.Player1, newPlayerState);

        Assert.NotSame(originalState, newState);
        Assert.Equal(0, originalState.Players[PlayerId.Player1].CurrentScore);
        Assert.Equal(15, newState.Players[PlayerId.Player1].CurrentScore);
    }

    [Fact]
    public void GameState_WithLastPlayedCard_ShouldReturnNewInstanceWithUpdatedCard()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.DrawFromDeck,
            PlayerId.Player1,
            PlayerId.Player1,
            0);
        var card = new Card(Guid.NewGuid(), Month.June, CardType.Tane, "牡丹に蝶");

        var newState = originalState.WithLastPlayedCard(card);

        Assert.NotSame(originalState, newState);
        Assert.Null(originalState.LastPlayedCard);
        Assert.Equal(card, newState.LastPlayedCard);
    }

    [Fact]
    public void GameState_WithLastDrawnCard_ShouldReturnNewInstanceWithUpdatedCard()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.DrawFromDeck,
            PlayerId.Player1,
            PlayerId.Player1,
            0);
        var card = new Card(Guid.NewGuid(), Month.July, CardType.Tane, "萩に猪");

        var newState = originalState.WithLastDrawnCard(card);

        Assert.NotSame(originalState, newState);
        Assert.Null(originalState.LastDrawnCard);
        Assert.Equal(card, newState.LastDrawnCard);
    }

    [Fact]
    public void GameState_WithPendingCapture_ShouldReturnNewInstanceWithUpdatedPending()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.SelectFieldCard,
            PlayerId.Player1,
            PlayerId.Player1,
            0);
        var pendingCards = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.August, CardType.Hikari, "芒に月")
        };

        var newState = originalState.WithPendingCapture(pendingCards);

        Assert.NotSame(originalState, newState);
        Assert.Empty(originalState.PendingCapture);
        Assert.Single(newState.PendingCapture);
    }

    [Fact]
    public void GameState_WithWinner_ShouldReturnNewInstanceWithWinnerAndResult()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.YakuCheck,
            PlayerId.Player1,
            PlayerId.Player1,
            0);
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 15 },
            { PlayerId.Player2, 0 }
        };
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku>() },
            { PlayerId.Player2, new List<Yaku>() }
        };
        var result = new GameResult(PlayerId.Player1, scores, yaku);

        var newState = originalState.WithWinner(PlayerId.Player1, result);

        Assert.NotSame(originalState, newState);
        Assert.Null(originalState.Winner);
        Assert.Equal(PlayerId.Player1, newState.Winner);
        Assert.Equal(result, newState.Result);
    }

    [Fact]
    public void GameState_WithResult_ShouldReturnNewInstanceWithResult()
    {
        var gameId = Guid.NewGuid();
        var originalState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.YakuCheck,
            PlayerId.Player1,
            PlayerId.Player1,
            0);
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 0 },
            { PlayerId.Player2, 0 }
        };
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku>() },
            { PlayerId.Player2, new List<Yaku>() }
        };
        var result = GameResult.CreateDraw(scores, yaku);

        var newState = originalState.WithResult(result);

        Assert.NotSame(originalState, newState);
        Assert.Null(originalState.Result);
        Assert.Equal(result, newState.Result);
        Assert.NotNull(newState.Result);
        Assert.True(newState.Result.IsDraw);
    }

    [Fact]
    public void GameState_ImmutableCollections_ShouldNotAllowModification()
    {
        var gameId = Guid.NewGuid();
        var deck = new List<Card>
        {
            new Card(Guid.NewGuid(), Month.September, CardType.Tane, "菊に盃")
        };
        var gameState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0,
            deck);

        // Modifying original collection should not affect GameState
        deck.Add(new Card(Guid.NewGuid(), Month.October, CardType.Tane, "紅葉に鹿"));

        Assert.Single(gameState.Deck);
    }

    [Fact]
    public void GameState_ChainedWithMethods_ShouldWorkCorrectly()
    {
        var gameId = Guid.NewGuid();
        var card1 = new Card(Guid.NewGuid(), Month.November, CardType.Hikari, "柳に小野道風");
        var card2 = new Card(Guid.NewGuid(), Month.December, CardType.Hikari, "桐に鳳凰");

        var state = new GameState(
                gameId,
                GamePhase.NotStarted,
                TurnPhase.PlayFromHand,
                PlayerId.Player1,
                PlayerId.Player1,
                0)
            .WithPhase(GamePhase.PlayerTurn)
            .WithTurnPhase(TurnPhase.DrawFromDeck)
            .WithTurnCount(3)
            .WithLastPlayedCard(card1)
            .WithLastDrawnCard(card2);

        Assert.Equal(GamePhase.PlayerTurn, state.Phase);
        Assert.Equal(TurnPhase.DrawFromDeck, state.TurnPhase);
        Assert.Equal(3, state.TurnCount);
        Assert.Equal(card1, state.LastPlayedCard);
        Assert.Equal(card2, state.LastDrawnCard);
    }
}
