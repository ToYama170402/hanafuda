namespace HanafudaEngine.Core.Models;

/// <summary>
/// Represents special flags for cards used in yaku (combinations)
/// </summary>
[Flags]
public enum SpecialCardFlag
{
    /// <summary>No special flag</summary>
    None = 0,
    
    /// <summary>赤短 (Red poetry ribbon)</summary>
    RedPoetry = 1,
    
    /// <summary>青短 (Blue poetry ribbon)</summary>
    BluePoetry = 2,
    
    /// <summary>盃 (Sake cup)</summary>
    SakeCup = 4,
    
    /// <summary>鹿 (Deer)</summary>
    Deer = 8,
    
    /// <summary>猪 (Boar)</summary>
    Boar = 16,
    
    /// <summary>蝶 (Butterfly)</summary>
    Butterfly = 32,
    
    /// <summary>雨札 (Rain man - 柳に小野道風)</summary>
    RainMan = 64
}
