using HanafudaEngine.Core.Models;
using HanafudaEngine.Domain.Models;

namespace HanafudaEngine.Tests.DomainTests;

/// <summary>
/// Tests for state transition scenarios in the game
/// </summary>
public class StateTransitionTests
{
    [Fact]
    public void StateTransition_GameStart_ShouldTransitionFromNotStartedToDealing()
    {
        var gameId = Guid.NewGuid();
        var initialState = new GameState(
            gameId,
            GamePhase.NotStarted,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        var dealingState = initialState.WithPhase(GamePhase.Dealing);

        Assert.Equal(GamePhase.NotStarted, initialState.Phase);
        Assert.Equal(GamePhase.Dealing, dealingState.Phase);
    }

    [Fact]
    public void StateTransition_AfterDealing_ShouldTransitionToPlayerTurn()
    {
        var gameId = Guid.NewGuid();
        var dealingState = new GameState(
            gameId,
            GamePhase.Dealing,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        var playerTurnState = dealingState.WithPhase(GamePhase.PlayerTurn);

        Assert.Equal(GamePhase.Dealing, dealingState.Phase);
        Assert.Equal(GamePhase.PlayerTurn, playerTurnState.Phase);
    }

    [Fact]
    public void StateTransition_TurnPhases_ShouldProgressThroughAllPhases()
    {
        var gameId = Guid.NewGuid();
        var state = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            1);

        // Progress through turn phases
        var afterPlayFromHand = state.WithTurnPhase(TurnPhase.DrawFromDeck);
        var afterDrawFromDeck = afterPlayFromHand.WithTurnPhase(TurnPhase.YakuCheck);
        var afterYakuCheck = afterDrawFromDeck.WithTurnPhase(TurnPhase.TurnEnd);

        Assert.Equal(TurnPhase.PlayFromHand, state.TurnPhase);
        Assert.Equal(TurnPhase.DrawFromDeck, afterPlayFromHand.TurnPhase);
        Assert.Equal(TurnPhase.YakuCheck, afterDrawFromDeck.TurnPhase);
        Assert.Equal(TurnPhase.TurnEnd, afterYakuCheck.TurnPhase);
    }

    [Fact]
    public void StateTransition_PlayerSwitch_ShouldAlternateCurrentPlayer()
    {
        var gameId = Guid.NewGuid();
        var player1Turn = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            1);

        var player2Turn = player1Turn
            .WithCurrentPlayer(PlayerId.Player2)
            .WithTurnCount(2);

        var player1TurnAgain = player2Turn
            .WithCurrentPlayer(PlayerId.Player1)
            .WithTurnCount(3);

        Assert.Equal(PlayerId.Player1, player1Turn.CurrentPlayer);
        Assert.Equal(1, player1Turn.TurnCount);
        Assert.Equal(PlayerId.Player2, player2Turn.CurrentPlayer);
        Assert.Equal(2, player2Turn.TurnCount);
        Assert.Equal(PlayerId.Player1, player1TurnAgain.CurrentPlayer);
        Assert.Equal(3, player1TurnAgain.TurnCount);
    }

    [Fact]
    public void StateTransition_YakuDetected_ShouldTransitionToKoiKoiDecision()
    {
        var gameId = Guid.NewGuid();
        var yakuCheckState = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.YakuCheck,
            PlayerId.Player1,
            PlayerId.Player1,
            3);

        // Player has completed a Yaku, should transition to KoiKoiDecision
        var koiKoiDecisionState = yakuCheckState
            .WithPhase(GamePhase.KoiKoiDecision)
            .WithTurnPhase(TurnPhase.KoiKoiDecision);

        Assert.Equal(GamePhase.PlayerTurn, yakuCheckState.Phase);
        Assert.Equal(TurnPhase.YakuCheck, yakuCheckState.TurnPhase);
        Assert.Equal(GamePhase.KoiKoiDecision, koiKoiDecisionState.Phase);
        Assert.Equal(TurnPhase.KoiKoiDecision, koiKoiDecisionState.TurnPhase);
    }

    [Fact]
    public void StateTransition_KoiKoiCalled_ShouldContinueGame()
    {
        var gameId = Guid.NewGuid();
        var koiKoiDecisionState = new GameState(
            gameId,
            GamePhase.KoiKoiDecision,
            TurnPhase.KoiKoiDecision,
            PlayerId.Player1,
            PlayerId.Player1,
            3);

        // Player calls Koi-Koi, game continues
        var continueState = koiKoiDecisionState
            .WithPhase(GamePhase.PlayerTurn)
            .WithTurnPhase(TurnPhase.PlayFromHand)
            .WithPlayerState(
                PlayerId.Player1,
                koiKoiDecisionState.Players[PlayerId.Player1].WithKoiKoiCalled(true))
            .WithCurrentPlayer(PlayerId.Player2)
            .WithTurnCount(4);

        Assert.Equal(GamePhase.KoiKoiDecision, koiKoiDecisionState.Phase);
        Assert.False(koiKoiDecisionState.Players[PlayerId.Player1].HasCalledKoiKoi);
        Assert.Equal(GamePhase.PlayerTurn, continueState.Phase);
        Assert.True(continueState.Players[PlayerId.Player1].HasCalledKoiKoi);
        Assert.Equal(PlayerId.Player2, continueState.CurrentPlayer);
    }

    [Fact]
    public void StateTransition_ShobuCalled_ShouldEndGame()
    {
        var gameId = Guid.NewGuid();
        var koiKoiDecisionState = new GameState(
            gameId,
            GamePhase.KoiKoiDecision,
            TurnPhase.KoiKoiDecision,
            PlayerId.Player1,
            PlayerId.Player1,
            5);

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
        var result = GameResult.CreateWin(PlayerId.Player1, scores, yaku);

        // Player calls Shobu (end game), game ends
        var gameOverState = koiKoiDecisionState
            .WithPhase(GamePhase.GameOver)
            .WithWinner(PlayerId.Player1, result);

        Assert.Equal(GamePhase.KoiKoiDecision, koiKoiDecisionState.Phase);
        Assert.Null(koiKoiDecisionState.Winner);
        Assert.Equal(GamePhase.GameOver, gameOverState.Phase);
        Assert.Equal(PlayerId.Player1, gameOverState.Winner);
        Assert.NotNull(gameOverState.Result);
    }

    [Fact]
    public void StateTransition_EightTurnsCompleted_ShouldEndInDraw()
    {
        var gameId = Guid.NewGuid();
        var state = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.TurnEnd,
            PlayerId.Player2,
            PlayerId.Player1,
            8); // 8 turns completed (each player has 8 cards)

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

        var drawState = state
            .WithPhase(GamePhase.Draw)
            .WithResult(result);

        Assert.Equal(8, state.TurnCount);
        Assert.Equal(GamePhase.Draw, drawState.Phase);
        Assert.True(drawState.Result?.IsDraw);
        Assert.Null(drawState.Winner);
    }

    [Fact]
    public void StateTransition_CompleteGameFlow_ShouldMaintainStateIntegrity()
    {
        var gameId = Guid.NewGuid();

        // Start game
        var state = new GameState(
            gameId,
            GamePhase.NotStarted,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            0);

        // Deal cards
        state = state.WithPhase(GamePhase.Dealing);
        Assert.Equal(GamePhase.Dealing, state.Phase);

        // Start first turn
        state = state.WithPhase(GamePhase.PlayerTurn);
        Assert.Equal(GamePhase.PlayerTurn, state.Phase);
        Assert.Equal(PlayerId.Player1, state.CurrentPlayer);

        // Play card
        state = state.WithTurnPhase(TurnPhase.DrawFromDeck);
        Assert.Equal(TurnPhase.DrawFromDeck, state.TurnPhase);

        // Complete turn
        state = state
            .WithTurnPhase(TurnPhase.TurnEnd)
            .WithCurrentPlayer(PlayerId.Player2)
            .WithTurnCount(1);
        Assert.Equal(TurnPhase.TurnEnd, state.TurnPhase);
        Assert.Equal(PlayerId.Player2, state.CurrentPlayer);
        Assert.Equal(1, state.TurnCount);

        // Verify state consistency
        Assert.Equal(gameId, state.GameId);
        Assert.Equal(PlayerId.Player1, state.Dealer);
        Assert.NotNull(state.Players);
        Assert.Equal(2, state.Players.Count);
    }

    [Fact]
    public void StateTransition_WithFieldCardSelection_ShouldTrackPendingCaptures()
    {
        var gameId = Guid.NewGuid();
        var card1 = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        var card2 = new Card(Guid.NewGuid(), Month.January, CardType.Tane, "松に赤短");
        var card3 = new Card(Guid.NewGuid(), Month.January, CardType.Tanzaku, "松に短冊");

        var state = new GameState(
            gameId,
            GamePhase.PlayerTurn,
            TurnPhase.PlayFromHand,
            PlayerId.Player1,
            PlayerId.Player1,
            1);

        // Player plays a January card, multiple January cards on field
        state = state
            .WithTurnPhase(TurnPhase.SelectFieldCard)
            .WithLastPlayedCard(card1)
            .WithPendingCapture(new[] { card2, card3 });

        Assert.Equal(TurnPhase.SelectFieldCard, state.TurnPhase);
        Assert.Equal(card1, state.LastPlayedCard);
        Assert.Equal(2, state.PendingCapture.Count);
        Assert.Contains(card2, state.PendingCapture);
        Assert.Contains(card3, state.PendingCapture);
    }
}
