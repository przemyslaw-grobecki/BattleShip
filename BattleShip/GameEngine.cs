namespace BattleShip;

public class GameEngine
{
    private static GameEngine? gameInstance;
        
    public static GameEngine GetInstance()
    {
        return gameInstance ??= new GameEngine();
    }

    public GameState CurrentState { get; private set; } = GameState.ShipBoarding;

    public void NextState()
    {
        CurrentState = CurrentState switch
        {
            GameState.ShipBoarding => GameState.ShipSinking,
            GameState.ShipSinking => GameState.End,
            _ => CurrentState
        };
    }
}