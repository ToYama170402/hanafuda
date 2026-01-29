namespace HanafudaEngine.Core.Models;

/// <summary>
/// Provides static definitions for all 48 Hanafuda cards
/// </summary>
public static class CardDefinitions
{
    private static readonly Lazy<IReadOnlyList<Card>> _allCards = new Lazy<IReadOnlyList<Card>>(InitializeCards);
    private static readonly Lazy<IReadOnlyDictionary<Month, IReadOnlyList<Card>>> _cardsByMonth = new Lazy<IReadOnlyDictionary<Month, IReadOnlyList<Card>>>(InitializeCardsByMonth);
    private static readonly Lazy<IReadOnlyDictionary<CardType, IReadOnlyList<Card>>> _cardsByType = new Lazy<IReadOnlyDictionary<CardType, IReadOnlyList<Card>>>(InitializeCardsByType);

    /// <summary>
    /// Gets all 48 Hanafuda cards
    /// </summary>
    public static IReadOnlyList<Card> AllCards => _allCards.Value;

    /// <summary>
    /// Gets cards for a specific month
    /// </summary>
    public static IReadOnlyList<Card> GetCardsByMonth(Month month)
    {
        return _cardsByMonth.Value[month];
    }

    /// <summary>
    /// Gets cards of a specific type
    /// </summary>
    public static IReadOnlyList<Card> GetCardsByType(CardType type)
    {
        return _cardsByType.Value[type];
    }

    /// <summary>
    /// Gets a specific card by month, type, and index
    /// </summary>
    /// <param name="month">The month of the card</param>
    /// <param name="type">The type of the card</param>
    /// <param name="index">The index within cards of the same month and type (default: 0)</param>
    /// <returns>The card matching the criteria</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is negative or beyond available cards</exception>
    public static Card GetCard(Month month, CardType type, int index = 0)
    {
        // Use cached dictionary for better performance
        var monthCards = _cardsByMonth.Value[month];
        var matchingCards = monthCards.Where(c => c.Type == type).ToList();
        
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, 
                $"Index cannot be negative.");
        }
        
        if (index >= matchingCards.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index,
                $"Index {index} is out of range for Month={month}, CardType={type}. Found {matchingCards.Count} matching card(s).");
        }
        
        return matchingCards[index];
    }

    private static IReadOnlyDictionary<Month, IReadOnlyList<Card>> InitializeCardsByMonth()
    {
        return AllCards
            .GroupBy(c => c.Month)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<Card>)g.ToList().AsReadOnly())
            .AsReadOnly();
    }

    private static IReadOnlyDictionary<CardType, IReadOnlyList<Card>> InitializeCardsByType()
    {
        return AllCards
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<Card>)g.ToList().AsReadOnly())
            .AsReadOnly();
    }

    private static IReadOnlyList<Card> InitializeCards()
    {
        var cards = new List<Card>();

        // 1月 - 松 (January - Pine)
        cards.Add(new Card(Guid.Parse("00000001-0001-0001-0000-000000000000"), Month.January, CardType.Hikari, "松に鶴"));
        cards.Add(new Card(Guid.Parse("00000001-0001-0002-0000-000000000000"), Month.January, CardType.Tanzaku, "松に赤短", SpecialCardFlag.RedPoetry));
        cards.Add(new Card(Guid.Parse("00000001-0001-0003-0000-000000000000"), Month.January, CardType.Kasu, "松のカス"));
        cards.Add(new Card(Guid.Parse("00000001-0001-0004-0000-000000000000"), Month.January, CardType.Kasu, "松のカス"));

        // 2月 - 梅 (February - Plum)
        cards.Add(new Card(Guid.Parse("00000002-0002-0001-0000-000000000000"), Month.February, CardType.Tane, "梅に鶯"));
        cards.Add(new Card(Guid.Parse("00000002-0002-0002-0000-000000000000"), Month.February, CardType.Tanzaku, "梅に赤短", SpecialCardFlag.RedPoetry));
        cards.Add(new Card(Guid.Parse("00000002-0002-0003-0000-000000000000"), Month.February, CardType.Kasu, "梅のカス"));
        cards.Add(new Card(Guid.Parse("00000002-0002-0004-0000-000000000000"), Month.February, CardType.Kasu, "梅のカス"));

        // 3月 - 桜 (March - Cherry Blossom)
        cards.Add(new Card(Guid.Parse("00000003-0003-0001-0000-000000000000"), Month.March, CardType.Hikari, "桜に幕"));
        cards.Add(new Card(Guid.Parse("00000003-0003-0002-0000-000000000000"), Month.March, CardType.Tanzaku, "桜に赤短", SpecialCardFlag.RedPoetry));
        cards.Add(new Card(Guid.Parse("00000003-0003-0003-0000-000000000000"), Month.March, CardType.Kasu, "桜のカス"));
        cards.Add(new Card(Guid.Parse("00000003-0003-0004-0000-000000000000"), Month.March, CardType.Kasu, "桜のカス"));

        // 4月 - 藤 (April - Wisteria)
        cards.Add(new Card(Guid.Parse("00000004-0004-0001-0000-000000000000"), Month.April, CardType.Tane, "藤に不如帰"));
        cards.Add(new Card(Guid.Parse("00000004-0004-0002-0000-000000000000"), Month.April, CardType.Tanzaku, "藤に短冊"));
        cards.Add(new Card(Guid.Parse("00000004-0004-0003-0000-000000000000"), Month.April, CardType.Kasu, "藤のカス"));
        cards.Add(new Card(Guid.Parse("00000004-0004-0004-0000-000000000000"), Month.April, CardType.Kasu, "藤のカス"));

        // 5月 - 菖蒲 (May - Iris)
        cards.Add(new Card(Guid.Parse("00000005-0005-0001-0000-000000000000"), Month.May, CardType.Tane, "菖蒲に八橋"));
        cards.Add(new Card(Guid.Parse("00000005-0005-0002-0000-000000000000"), Month.May, CardType.Tanzaku, "菖蒲に短冊"));
        cards.Add(new Card(Guid.Parse("00000005-0005-0003-0000-000000000000"), Month.May, CardType.Kasu, "菖蒲のカス"));
        cards.Add(new Card(Guid.Parse("00000005-0005-0004-0000-000000000000"), Month.May, CardType.Kasu, "菖蒲のカス"));

        // 6月 - 牡丹 (June - Peony)
        cards.Add(new Card(Guid.Parse("00000006-0006-0001-0000-000000000000"), Month.June, CardType.Tane, "牡丹に蝶", SpecialCardFlag.Butterfly));
        cards.Add(new Card(Guid.Parse("00000006-0006-0002-0000-000000000000"), Month.June, CardType.Tanzaku, "牡丹に青短", SpecialCardFlag.BluePoetry));
        cards.Add(new Card(Guid.Parse("00000006-0006-0003-0000-000000000000"), Month.June, CardType.Kasu, "牡丹のカス"));
        cards.Add(new Card(Guid.Parse("00000006-0006-0004-0000-000000000000"), Month.June, CardType.Kasu, "牡丹のカス"));

        // 7月 - 萩 (July - Bush Clover)
        cards.Add(new Card(Guid.Parse("00000007-0007-0001-0000-000000000000"), Month.July, CardType.Tane, "萩に猪", SpecialCardFlag.Boar));
        cards.Add(new Card(Guid.Parse("00000007-0007-0002-0000-000000000000"), Month.July, CardType.Tanzaku, "萩に短冊"));
        cards.Add(new Card(Guid.Parse("00000007-0007-0003-0000-000000000000"), Month.July, CardType.Kasu, "萩のカス"));
        cards.Add(new Card(Guid.Parse("00000007-0007-0004-0000-000000000000"), Month.July, CardType.Kasu, "萩のカス"));

        // 8月 - 芒 (August - Susuki grass)
        cards.Add(new Card(Guid.Parse("00000008-0008-0001-0000-000000000000"), Month.August, CardType.Hikari, "芒に月"));
        cards.Add(new Card(Guid.Parse("00000008-0008-0002-0000-000000000000"), Month.August, CardType.Tane, "芒に雁"));
        cards.Add(new Card(Guid.Parse("00000008-0008-0003-0000-000000000000"), Month.August, CardType.Kasu, "芒のカス"));
        cards.Add(new Card(Guid.Parse("00000008-0008-0004-0000-000000000000"), Month.August, CardType.Kasu, "芒のカス"));

        // 9月 - 菊 (September - Chrysanthemum)
        cards.Add(new Card(Guid.Parse("00000009-0009-0001-0000-000000000000"), Month.September, CardType.Tane, "菊に盃", SpecialCardFlag.SakeCup));
        cards.Add(new Card(Guid.Parse("00000009-0009-0002-0000-000000000000"), Month.September, CardType.Tanzaku, "菊に青短", SpecialCardFlag.BluePoetry));
        cards.Add(new Card(Guid.Parse("00000009-0009-0003-0000-000000000000"), Month.September, CardType.Kasu, "菊のカス"));
        cards.Add(new Card(Guid.Parse("00000009-0009-0004-0000-000000000000"), Month.September, CardType.Kasu, "菊のカス"));

        // 10月 - 紅葉 (October - Maple)
        cards.Add(new Card(Guid.Parse("0000000A-000A-0001-0000-000000000000"), Month.October, CardType.Tane, "紅葉に鹿", SpecialCardFlag.Deer));
        cards.Add(new Card(Guid.Parse("0000000A-000A-0002-0000-000000000000"), Month.October, CardType.Tanzaku, "紅葉に青短", SpecialCardFlag.BluePoetry));
        cards.Add(new Card(Guid.Parse("0000000A-000A-0003-0000-000000000000"), Month.October, CardType.Kasu, "紅葉のカス"));
        cards.Add(new Card(Guid.Parse("0000000A-000A-0004-0000-000000000000"), Month.October, CardType.Kasu, "紅葉のカス"));

        // 11月 - 柳 (November - Willow)
        cards.Add(new Card(Guid.Parse("0000000B-000B-0001-0000-000000000000"), Month.November, CardType.Hikari, "柳に小野道風", SpecialCardFlag.RainMan));
        cards.Add(new Card(Guid.Parse("0000000B-000B-0002-0000-000000000000"), Month.November, CardType.Tane, "柳に燕"));
        cards.Add(new Card(Guid.Parse("0000000B-000B-0003-0000-000000000000"), Month.November, CardType.Tanzaku, "柳に短冊"));
        cards.Add(new Card(Guid.Parse("0000000B-000B-0004-0000-000000000000"), Month.November, CardType.Kasu, "柳のカス"));

        // 12月 - 桐 (December - Paulownia)
        cards.Add(new Card(Guid.Parse("0000000C-000C-0001-0000-000000000000"), Month.December, CardType.Hikari, "桐に鳳凰"));
        cards.Add(new Card(Guid.Parse("0000000C-000C-0002-0000-000000000000"), Month.December, CardType.Kasu, "桐のカス"));
        cards.Add(new Card(Guid.Parse("0000000C-000C-0003-0000-000000000000"), Month.December, CardType.Kasu, "桐のカス"));
        cards.Add(new Card(Guid.Parse("0000000C-000C-0004-0000-000000000000"), Month.December, CardType.Kasu, "桐のカス"));

        return cards.AsReadOnly();
    }
}
