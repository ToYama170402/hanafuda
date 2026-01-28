using HanafudaEngine.Core;
using HanafudaEngine.Domain;

namespace HanafudaEngine.Facade;

/// <summary>
/// こいこいゲーム全体を統括するファサードクラス
/// </summary>
public class KoiKoiGame
{
    private GameState _gameState;

    public KoiKoiGame()
    {
        _gameState = new GameState();
    }

    /// <summary>
    /// ゲームを初期化する
    /// </summary>
    public void Initialize()
    {
        // TODO: ゲームの初期化ロジックを実装
    }

    /// <summary>
    /// 現在のゲーム状態を取得する
    /// </summary>
    public GameState GetGameState()
    {
        return _gameState;
    }
}
