namespace HanafudaEngine.Core.Models;

/// <summary>
/// Represents the types of yaku (combinations) in Koi-Koi
/// </summary>
public enum YakuType
{
    // 光札の役 (Bright card combinations)

    /// <summary>五光 - All 5 bright cards (15 points)</summary>
    Goko,

    /// <summary>四光 - 4 bright cards excluding Rain Man (10 points)</summary>
    Shiko,

    /// <summary>雨四光 - 4 bright cards including Rain Man (8 points)</summary>
    AmeShiko,

    /// <summary>三光 - Pine, Cherry, and Susuki bright cards (6 points)</summary>
    Sanko,

    // 種札の役 (Animal card combinations)

    /// <summary>猪鹿蝶 - Boar, Deer, and Butterfly (5 points)</summary>
    Inoshikacho,

    /// <summary>花見で一杯 - Cherry blossom curtain and sake cup (5 points)</summary>
    Hanami,

    /// <summary>月見で一杯 - Moon and sake cup (5 points)</summary>
    Tsukimi,

    /// <summary>種 - 5 or more animal cards (1 point + bonus)</summary>
    Tane,

    // 短冊札の役 (Ribbon card combinations)

    /// <summary>赤短 - 3 red poetry ribbons (6 points)</summary>
    Akatan,

    /// <summary>青短 - 3 blue poetry ribbons (6 points)</summary>
    Aotan,

    /// <summary>短冊 - 5 or more ribbon cards (1 point + bonus)</summary>
    Tanzaku,

    // カス札の役 (Plain card combinations)

    /// <summary>カス - 10 or more plain cards (1 point + bonus)</summary>
    Kasu
}
