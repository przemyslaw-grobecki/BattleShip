namespace BattleShip;

public class GameEngine
{
    private static GameEngine? gameInstance;
        
    public static GameEngine GetInstance()
    {
        return gameInstance ??= new GameEngine();
    }

    public GameState CurrentState => GameState.ShipBoarding;
}