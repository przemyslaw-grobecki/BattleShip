namespace BattleShip;

public class Waters : Dictionary<(int, int), SeaWaveState>
{
    private const int LowerBoundaryInclusive = 0;
    private const int UpperBoundaryExclusive = 10;
    
    public new bool TryAdd((int, int) key, SeaWaveState value)
    {
        if (LowerBoundaryInclusive <= key.Item1 && key.Item1 < UpperBoundaryExclusive &&
            LowerBoundaryInclusive <= key.Item2 && key.Item2 < UpperBoundaryExclusive)
        {
            return base.TryAdd(key, value);
        }
        return false;
    }
}