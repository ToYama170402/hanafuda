using HanafudaEngine.Core.Models;
using HanafudaEngine.Domain.Models;

namespace HanafudaEngine.Tests.DomainTests;

public class GameResultTests
{
    [Fact]
    public void GameResult_Constructor_WithWinner_ShouldSetPropertiesCorrectly()
    {
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

        var result = new GameResult(PlayerId.Player1, scores, yaku, false);

        Assert.Equal(PlayerId.Player1, result.Winner);
        Assert.Equal(10, result.Scores[PlayerId.Player1]);
        Assert.Equal(5, result.Scores[PlayerId.Player2]);
        Assert.False(result.IsKoiKoiWin);
        Assert.False(result.IsDraw);
    }

    [Fact]
    public void GameResult_Constructor_WithKoiKoiWin_ShouldSetIsKoiKoiWinTrue()
    {
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 20 },
            { PlayerId.Player2, 0 }
        };
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku>() },
            { PlayerId.Player2, new List<Yaku>() }
        };

        var result = new GameResult(PlayerId.Player1, scores, yaku, true);

        Assert.True(result.IsKoiKoiWin);
        Assert.False(result.IsDraw);
    }

    [Fact]
    public void GameResult_Constructor_WithNullWinner_ShouldSetIsDrawTrue()
    {
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

        var result = new GameResult(null, scores, yaku, false);

        Assert.Null(result.Winner);
        Assert.True(result.IsDraw);
    }

    [Fact]
    public void GameResult_Constructor_WithNullScores_ShouldThrowArgumentNullException()
    {
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku>() },
            { PlayerId.Player2, new List<Yaku>() }
        };

        Assert.Throws<ArgumentNullException>(() =>
            new GameResult(PlayerId.Player1, null!, yaku, false));
    }

    [Fact]
    public void GameResult_Constructor_WithNullCompletedYaku_ShouldThrowArgumentNullException()
    {
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 10 },
            { PlayerId.Player2, 5 }
        };

        Assert.Throws<ArgumentNullException>(() =>
            new GameResult(PlayerId.Player1, scores, null!, false));
    }

    [Fact]
    public void GameResult_Constructor_WithKoiKoiWinButNoWinner_ShouldThrowArgumentException()
    {
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

        Assert.Throws<ArgumentException>(() =>
            new GameResult(null, scores, yaku, true));
    }

    [Fact]
    public void GameResult_Constructor_WithNegativeScore_ShouldThrowArgumentException()
    {
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, -10 },
            { PlayerId.Player2, 5 }
        };
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku>() },
            { PlayerId.Player2, new List<Yaku>() }
        };

        Assert.Throws<ArgumentException>(() =>
            new GameResult(PlayerId.Player1, scores, yaku, false));
    }

    [Fact]
    public void GameResult_CreateDraw_ShouldCreateDrawResult()
    {
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

        Assert.Null(result.Winner);
        Assert.True(result.IsDraw);
        Assert.False(result.IsKoiKoiWin);
    }

    [Fact]
    public void GameResult_CreateWin_ShouldCreateWinResult()
    {
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 15 },
            { PlayerId.Player2, 0 }
        };
        var yakuCard = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku> { new Yaku(YakuType.Sanko, 5, 0, new[] { yakuCard }) } },
            { PlayerId.Player2, new List<Yaku>() }
        };

        var result = GameResult.CreateWin(PlayerId.Player1, scores, yaku, false);

        Assert.Equal(PlayerId.Player1, result.Winner);
        Assert.False(result.IsDraw);
        Assert.False(result.IsKoiKoiWin);
    }

    [Fact]
    public void GameResult_CreateWin_WithKoiKoi_ShouldCreateKoiKoiWinResult()
    {
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 30 },
            { PlayerId.Player2, 0 }
        };
        var yakuCard = new Card(Guid.NewGuid(), Month.December, CardType.Hikari, "桐に鳳凰");
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku> { new Yaku(YakuType.Goko, 15, 0, new[] { yakuCard }) } },
            { PlayerId.Player2, new List<Yaku>() }
        };

        var result = GameResult.CreateWin(PlayerId.Player1, scores, yaku, true);

        Assert.Equal(PlayerId.Player1, result.Winner);
        Assert.True(result.IsKoiKoiWin);
        Assert.False(result.IsDraw);
    }

    [Fact]
    public void GameResult_DefensiveCopy_ShouldNotAllowExternalMutation()
    {
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 15 },
            { PlayerId.Player2, 5 }
        };
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku>() },
            { PlayerId.Player2, new List<Yaku>() }
        };

        var result = GameResult.CreateWin(PlayerId.Player1, scores, yaku);

        // Modify original dictionaries
        scores[PlayerId.Player1] = 999;
        yaku[PlayerId.Player1] = new List<Yaku> { new Yaku(YakuType.Goko, 15, 0, new[] { new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴") }) };

        // GameResult should not be affected
        Assert.Equal(15, result.Scores[PlayerId.Player1]);
        Assert.Empty(result.CompletedYaku[PlayerId.Player1]);
    }

    [Fact]
    public void GameResult_WithCompletedYaku_ShouldIncludeYakuInformation()
    {
        var card1 = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        var card2 = new Card(Guid.NewGuid(), Month.February, CardType.Tane, "梅に鶯");
        var card3 = new Card(Guid.NewGuid(), Month.March, CardType.Tane, "桜に幕");
        var yaku1 = new Yaku(YakuType.Sanko, 5, 0, new[] { card1 });
        var yaku2 = new Yaku(YakuType.Tane, 1, 3, new[] { card2, card3 });
        var yaku = new Dictionary<PlayerId, IReadOnlyList<Yaku>>
        {
            { PlayerId.Player1, new List<Yaku> { yaku1, yaku2 } },
            { PlayerId.Player2, new List<Yaku>() }
        };
        var scores = new Dictionary<PlayerId, int>
        {
            { PlayerId.Player1, 9 },
            { PlayerId.Player2, 0 }
        };

        var result = GameResult.CreateWin(PlayerId.Player1, scores, yaku);

        Assert.Equal(2, result.CompletedYaku[PlayerId.Player1].Count);
        Assert.Equal(YakuType.Sanko, result.CompletedYaku[PlayerId.Player1][0].Type);
        Assert.Equal(YakuType.Tane, result.CompletedYaku[PlayerId.Player1][1].Type);
    }
}
