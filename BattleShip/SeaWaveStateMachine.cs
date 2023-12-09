namespace BattleShip;

public static class SeaWaveStateMachine
{

    public static SeaWaveState NextState(SeaWaveState currentState)
    {
        if (GameEngine.GetInstance().CurrentState == GameState.ShipBoarding)
        {
            switch (currentState)
            {
                case SeaWaveState.Free:
                    return SeaWaveState.Ship;
                    break;
                case SeaWaveState.WreckNearby:
                    break;
                case SeaWaveState.Wreck:
                    break;
                case SeaWaveState.ShipNearby:
                    break;
                case SeaWaveState.Ship:
                    break;
                case SeaWaveState.Disturbed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
            }
        }

        return SeaWaveState.Disturbed;
    }
}