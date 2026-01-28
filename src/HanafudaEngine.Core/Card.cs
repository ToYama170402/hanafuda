namespace HanafudaEngine.Core;

/// <summary>
/// 花札の1枚の札を表現するクラス
/// </summary>
public class Card
{
    public Month Month { get; init; }
    public CardType Type { get; init; }
    public string Name { get; init; } = string.Empty;

    public Card(Month month, CardType type, string name)
    {
        Month = month;
        Type = type;
        Name = name;
    }
}
