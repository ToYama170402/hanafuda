using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Tests.CoreTests;

public class YakuTests
{
    [Fact]
    public void Yaku_Constructor_ShouldSetPropertiesCorrectly()
    {
        var card1 = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        var card2 = new Card(Guid.NewGuid(), Month.March, CardType.Hikari, "桜に幕");
        var cards = new[] { card1, card2 };

        var yaku = new Yaku(YakuType.Sanko, 6, 0, cards);

        Assert.Equal(YakuType.Sanko, yaku.Type);
        Assert.Equal(6, yaku.BaseScore);
        Assert.Equal(0, yaku.BonusScore);
        Assert.Equal(6, yaku.TotalScore);
        Assert.Equal(2, yaku.Cards.Count);
        Assert.Contains(card1, yaku.Cards);
        Assert.Contains(card2, yaku.Cards);
    }

    [Fact]
    public void Yaku_Constructor_WithBonusScore_ShouldCalculateTotalScoreCorrectly()
    {
        var cards = new[]
        {
            new Card(Guid.NewGuid(), Month.February, CardType.Tane, "梅に鶯"),
            new Card(Guid.NewGuid(), Month.April, CardType.Tane, "藤に不如帰"),
            new Card(Guid.NewGuid(), Month.May, CardType.Tane, "菖蒲に八橋"),
            new Card(Guid.NewGuid(), Month.June, CardType.Tane, "牡丹に蝶"),
            new Card(Guid.NewGuid(), Month.July, CardType.Tane, "萩に猪"),
            new Card(Guid.NewGuid(), Month.August, CardType.Tane, "芒に雁"),
            new Card(Guid.NewGuid(), Month.September, CardType.Tane, "菊に盃")
        };

        var yaku = new Yaku(YakuType.Tane, 1, 2, cards);

        Assert.Equal(1, yaku.BaseScore);
        Assert.Equal(2, yaku.BonusScore);
        Assert.Equal(3, yaku.TotalScore);
        Assert.Equal(7, yaku.Cards.Count);
    }

    [Fact]
    public void Yaku_Constructor_WithNullCards_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Yaku(YakuType.Goko, 15, 0, null!));
    }

    [Fact]
    public void Yaku_Constructor_WithEmptyCards_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Yaku(YakuType.Goko, 15, 0, Array.Empty<Card>()));
    }

    [Fact]
    public void Yaku_Constructor_WithNullElementInCards_ShouldThrowArgumentException()
    {
        var cards = new Card?[] { new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴"), null };
        Assert.Throws<ArgumentException>(() =>
            new Yaku(YakuType.Goko, 15, 0, cards!));
    }

    [Fact]
    public void Yaku_Constructor_WithNegativeBaseScore_ShouldThrowArgumentOutOfRangeException()
    {
        var card = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Yaku(YakuType.Goko, -1, 0, new[] { card }));
    }

    [Fact]
    public void Yaku_Constructor_WithNegativeBonusScore_ShouldThrowArgumentOutOfRangeException()
    {
        var card = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Yaku(YakuType.Goko, 15, -1, new[] { card }));
    }

    [Fact]
    public void Yaku_Cards_ShouldBeReadOnly()
    {
        var card = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        var cards = new[] { card };

        var yaku = new Yaku(YakuType.Sanko, 6, 0, cards);

        // Verify it's IReadOnlyList
        Assert.IsAssignableFrom<IReadOnlyList<Card>>(yaku.Cards);

        // Verify it's actually read-only by casting to ICollection and checking IsReadOnly
        var collection = Assert.IsAssignableFrom<ICollection<Card>>(yaku.Cards);
        Assert.True(collection.IsReadOnly);
    }

    [Fact]
    public void Yaku_ToString_ShouldReturnFormattedString()
    {
        var card = new Card(Guid.NewGuid(), Month.January, CardType.Hikari, "松に鶴");
        var yaku = new Yaku(YakuType.Goko, 15, 0, new[] { card });

        var result = yaku.ToString();

        Assert.Contains("Goko", result);
        Assert.Contains("15", result);
        Assert.Contains("points", result);
    }
}
