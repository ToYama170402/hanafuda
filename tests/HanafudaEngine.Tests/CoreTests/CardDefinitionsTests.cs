using HanafudaEngine.Core.Models;

namespace HanafudaEngine.Tests.CoreTests;

public class CardDefinitionsTests
{
    [Fact]
    public void AllCards_ShouldContainExactly48Cards()
    {
        var allCards = CardDefinitions.AllCards;
        
        Assert.Equal(48, allCards.Count);
    }

    [Fact]
    public void AllCards_ShouldHaveUniqueIds()
    {
        var allCards = CardDefinitions.AllCards;
        var uniqueIds = allCards.Select(c => c.Id).Distinct().Count();
        
        Assert.Equal(48, uniqueIds);
    }

    [Theory]
    [InlineData(Month.January)]
    [InlineData(Month.February)]
    [InlineData(Month.March)]
    [InlineData(Month.April)]
    [InlineData(Month.May)]
    [InlineData(Month.June)]
    [InlineData(Month.July)]
    [InlineData(Month.August)]
    [InlineData(Month.September)]
    [InlineData(Month.October)]
    [InlineData(Month.November)]
    [InlineData(Month.December)]
    public void GetCardsByMonth_ShouldReturn4CardsForEachMonth(Month month)
    {
        var cards = CardDefinitions.GetCardsByMonth(month);
        
        Assert.Equal(4, cards.Count);
        Assert.All(cards, c => Assert.Equal(month, c.Month));
    }

    [Fact]
    public void GetCardsByType_Hikari_ShouldReturn5Cards()
    {
        var hikariCards = CardDefinitions.GetCardsByType(CardType.Hikari);
        
        Assert.Equal(5, hikariCards.Count);
        Assert.All(hikariCards, c => Assert.Equal(CardType.Hikari, c.Type));
    }

    [Fact]
    public void GetCardsByType_Tane_ShouldReturn9Cards()
    {
        var taneCards = CardDefinitions.GetCardsByType(CardType.Tane);
        
        Assert.Equal(9, taneCards.Count);
        Assert.All(taneCards, c => Assert.Equal(CardType.Tane, c.Type));
    }

    [Fact]
    public void GetCardsByType_Tanzaku_ShouldReturn10Cards()
    {
        var tanzakuCards = CardDefinitions.GetCardsByType(CardType.Tanzaku);
        
        Assert.Equal(10, tanzakuCards.Count);
        Assert.All(tanzakuCards, c => Assert.Equal(CardType.Tanzaku, c.Type));
    }

    [Fact]
    public void GetCardsByType_Kasu_ShouldReturn24Cards()
    {
        var kasuCards = CardDefinitions.GetCardsByType(CardType.Kasu);
        
        Assert.Equal(24, kasuCards.Count);
        Assert.All(kasuCards, c => Assert.Equal(CardType.Kasu, c.Type));
    }

    [Fact]
    public void SpecialFlags_RedPoetry_ShouldBeSetOn3Cards()
    {
        var redPoetryCards = CardDefinitions.AllCards
            .Where(c => c.SpecialFlag.HasValue && c.SpecialFlag.Value.HasFlag(SpecialCardFlag.RedPoetry))
            .ToList();
        
        Assert.Equal(3, redPoetryCards.Count);
        Assert.Contains(redPoetryCards, c => c.Month == Month.January && c.Name == "松に赤短");
        Assert.Contains(redPoetryCards, c => c.Month == Month.February && c.Name == "梅に赤短");
        Assert.Contains(redPoetryCards, c => c.Month == Month.March && c.Name == "桜に赤短");
    }

    [Fact]
    public void SpecialFlags_BluePoetry_ShouldBeSetOn3Cards()
    {
        var bluePoetryCards = CardDefinitions.AllCards
            .Where(c => c.SpecialFlag.HasValue && c.SpecialFlag.Value.HasFlag(SpecialCardFlag.BluePoetry))
            .ToList();
        
        Assert.Equal(3, bluePoetryCards.Count);
        Assert.Contains(bluePoetryCards, c => c.Month == Month.June && c.Name == "牡丹に青短");
        Assert.Contains(bluePoetryCards, c => c.Month == Month.September && c.Name == "菊に青短");
        Assert.Contains(bluePoetryCards, c => c.Month == Month.October && c.Name == "紅葉に青短");
    }

    [Fact]
    public void SpecialFlags_SakeCup_ShouldBeSetOn1Card()
    {
        var sakeCupCards = CardDefinitions.AllCards
            .Where(c => c.SpecialFlag.HasValue && c.SpecialFlag.Value.HasFlag(SpecialCardFlag.SakeCup))
            .ToList();
        
        Assert.Single(sakeCupCards);
        Assert.Equal(Month.September, sakeCupCards[0].Month);
        Assert.Equal("菊に盃", sakeCupCards[0].Name);
    }

    [Fact]
    public void SpecialFlags_Deer_ShouldBeSetOn1Card()
    {
        var deerCards = CardDefinitions.AllCards
            .Where(c => c.SpecialFlag.HasValue && c.SpecialFlag.Value.HasFlag(SpecialCardFlag.Deer))
            .ToList();
        
        Assert.Single(deerCards);
        Assert.Equal(Month.October, deerCards[0].Month);
        Assert.Equal("紅葉に鹿", deerCards[0].Name);
    }

    [Fact]
    public void SpecialFlags_Boar_ShouldBeSetOn1Card()
    {
        var boarCards = CardDefinitions.AllCards
            .Where(c => c.SpecialFlag.HasValue && c.SpecialFlag.Value.HasFlag(SpecialCardFlag.Boar))
            .ToList();
        
        Assert.Single(boarCards);
        Assert.Equal(Month.July, boarCards[0].Month);
        Assert.Equal("萩に猪", boarCards[0].Name);
    }

    [Fact]
    public void SpecialFlags_Butterfly_ShouldBeSetOn1Card()
    {
        var butterflyCards = CardDefinitions.AllCards
            .Where(c => c.SpecialFlag.HasValue && c.SpecialFlag.Value.HasFlag(SpecialCardFlag.Butterfly))
            .ToList();
        
        Assert.Single(butterflyCards);
        Assert.Equal(Month.June, butterflyCards[0].Month);
        Assert.Equal("牡丹に蝶", butterflyCards[0].Name);
    }

    [Fact]
    public void SpecialFlags_RainMan_ShouldBeSetOn1Card()
    {
        var rainManCards = CardDefinitions.AllCards
            .Where(c => c.SpecialFlag.HasValue && c.SpecialFlag.Value.HasFlag(SpecialCardFlag.RainMan))
            .ToList();
        
        Assert.Single(rainManCards);
        Assert.Equal(Month.November, rainManCards[0].Month);
        Assert.Equal("柳に小野道風", rainManCards[0].Name);
    }

    [Fact]
    public void HikariCards_ShouldIncludeExpectedCards()
    {
        var hikariCards = CardDefinitions.GetCardsByType(CardType.Hikari);
        
        Assert.Contains(hikariCards, c => c.Month == Month.January && c.Name == "松に鶴");
        Assert.Contains(hikariCards, c => c.Month == Month.March && c.Name == "桜に幕");
        Assert.Contains(hikariCards, c => c.Month == Month.August && c.Name == "芒に月");
        Assert.Contains(hikariCards, c => c.Month == Month.November && c.Name == "柳に小野道風");
        Assert.Contains(hikariCards, c => c.Month == Month.December && c.Name == "桐に鳳凰");
    }

    [Fact]
    public void TaneCards_ShouldIncludeExpectedCards()
    {
        var taneCards = CardDefinitions.GetCardsByType(CardType.Tane);
        
        Assert.Contains(taneCards, c => c.Month == Month.February && c.Name == "梅に鶯");
        Assert.Contains(taneCards, c => c.Month == Month.April && c.Name == "藤に不如帰");
        Assert.Contains(taneCards, c => c.Month == Month.May && c.Name == "菖蒲に八橋");
        Assert.Contains(taneCards, c => c.Month == Month.June && c.Name == "牡丹に蝶");
        Assert.Contains(taneCards, c => c.Month == Month.July && c.Name == "萩に猪");
        Assert.Contains(taneCards, c => c.Month == Month.August && c.Name == "芒に雁");
        Assert.Contains(taneCards, c => c.Month == Month.September && c.Name == "菊に盃");
        Assert.Contains(taneCards, c => c.Month == Month.October && c.Name == "紅葉に鹿");
        Assert.Contains(taneCards, c => c.Month == Month.November && c.Name == "柳に燕");
    }

    [Fact]
    public void AllCards_ShouldReturnSameInstance()
    {
        var allCards1 = CardDefinitions.AllCards;
        var allCards2 = CardDefinitions.AllCards;
        
        // Should return the same instance (singleton pattern via Lazy)
        Assert.Same(allCards1, allCards2);
    }

    [Fact]
    public void GetCardsByMonth_January_ShouldReturnCorrectCards()
    {
        var januaryCards = CardDefinitions.GetCardsByMonth(Month.January);
        
        Assert.Equal(4, januaryCards.Count);
        Assert.Contains(januaryCards, c => c.Type == CardType.Hikari && c.Name == "松に鶴");
        Assert.Contains(januaryCards, c => c.Type == CardType.Tanzaku && c.Name == "松に赤短");
        Assert.Equal(2, januaryCards.Count(c => c.Type == CardType.Kasu && c.Name == "松のカス"));
    }

    [Fact]
    public void GetCardsByMonth_December_ShouldReturnCorrectCards()
    {
        var decemberCards = CardDefinitions.GetCardsByMonth(Month.December);
        
        Assert.Equal(4, decemberCards.Count);
        Assert.Contains(decemberCards, c => c.Type == CardType.Hikari && c.Name == "桐に鳳凰");
        Assert.Equal(3, decemberCards.Count(c => c.Type == CardType.Kasu && c.Name == "桐のカス"));
    }

    [Fact]
    public void GetCard_ShouldReturnCorrectCard()
    {
        // Get Hikari card from January (松に鶴)
        var card = CardDefinitions.GetCard(Month.January, CardType.Hikari);
        
        Assert.NotNull(card);
        Assert.Equal(Month.January, card.Month);
        Assert.Equal(CardType.Hikari, card.Type);
        Assert.Equal("松に鶴", card.Name);
    }

    [Fact]
    public void GetCard_WithIndex_ShouldReturnCorrectCard()
    {
        // January has 2 Kasu cards - get the first one (index 0)
        var card1 = CardDefinitions.GetCard(Month.January, CardType.Kasu, 0);
        var card2 = CardDefinitions.GetCard(Month.January, CardType.Kasu, 1);
        
        Assert.NotNull(card1);
        Assert.NotNull(card2);
        Assert.Equal(Month.January, card1.Month);
        Assert.Equal(Month.January, card2.Month);
        Assert.Equal(CardType.Kasu, card1.Type);
        Assert.Equal(CardType.Kasu, card2.Type);
        Assert.NotEqual(card1.Id, card2.Id);
    }

    [Fact]
    public void GetCard_WithInvalidIndex_ShouldThrowException()
    {
        // January has only 1 Hikari card, so index 1 should throw
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            CardDefinitions.GetCard(Month.January, CardType.Hikari, 1));
        
        Assert.Equal("index", exception.ParamName);
        Assert.Contains("Index 1 is out of range", exception.Message);
        Assert.Contains("Month=January", exception.Message);
        Assert.Contains("CardType=Hikari", exception.Message);
    }

    [Fact]
    public void GetCard_WithNegativeIndex_ShouldThrowException()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            CardDefinitions.GetCard(Month.January, CardType.Hikari, -1));
        
        Assert.Equal("index", exception.ParamName);
        Assert.Contains("Index cannot be negative", exception.Message);
    }

    [Fact]
    public void GetCard_ForNonExistentCombination_ShouldThrowException()
    {
        // December has no Tane cards, so this should throw
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            CardDefinitions.GetCard(Month.December, CardType.Tane, 0));
        
        Assert.Equal("index", exception.ParamName);
        Assert.Contains("Index 0 is out of range", exception.Message);
        Assert.Contains("Found 0 matching card(s)", exception.Message);
    }
}
