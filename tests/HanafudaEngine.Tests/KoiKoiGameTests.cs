using HanafudaEngine.Core;
using HanafudaEngine.Domain;
using HanafudaEngine.Facade;

namespace HanafudaEngine.Tests;

public class KoiKoiGameTests
{
    [Fact]
    public void KoiKoiGame_CanBeCreated()
    {
        // Arrange & Act
        var game = new KoiKoiGame();

        // Assert
        Assert.NotNull(game);
    }

    [Fact]
    public void GameState_CanBeRetrieved()
    {
        // Arrange
        var game = new KoiKoiGame();

        // Act
        var state = game.GetGameState();

        // Assert
        Assert.NotNull(state);
    }
}
