namespace BattleShip;

public class Waters 
{
    public readonly Dictionary<(int, int), SeaWaveState> States = new();
    private const int LowerBoundaryInclusive = 0;
    private const int UpperBoundaryExclusive = 10;

    public Waters()
    {
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                States[(i, j)] = SeaWaveState.Free;
            }
        }
    }

    public bool TrySetState((int, int) key, SeaWaveState newState)
    {
        if (!IsInBoundary(key))
        {
            return false;
        }
        if (States.TryGetValue(key, out var water))
        {
            if (newState is SeaWaveState.ShipNearby && water is SeaWaveState.ShipNearby)
            {
                water = water + 1;
                return true;
            }
            
            if (newState is SeaWaveState.WreckNearby && water is SeaWaveState.WreckNearby)
            {
                water = water - 1;
                return true;
            }

            if (newState is SeaWaveState.Free)
            {
                if (water is SeaWaveState.ShipNearby)
                {
                    water = water - 1;
                    return true;
                }

                if (water is SeaWaveState.WreckNearby)
                {
                    water = water + 1;
                    return true;
                }
            }
            water = newState;
        }
        return false;
    }

    public bool IsStateEqualTo((int, int) key, SeaWaveState seaWaveState)
    {
        if (!IsInBoundary(key))
        {
            return false;
        }

        if (States.TryGetValue(key, out var state))
        {
            switch (seaWaveState)
            {
                case SeaWaveState.ShipNearby:
                    return state >= seaWaveState;
                case SeaWaveState.WreckNearby:
                    return state <= seaWaveState;
            }

            return seaWaveState == state;
        }

        return false;
    }

    private static bool IsInBoundary((int, int) key)
    {
        return key.Item1 is >= LowerBoundaryInclusive and < UpperBoundaryExclusive &&
               key.Item2 is >= LowerBoundaryInclusive and < UpperBoundaryExclusive;
    }
}