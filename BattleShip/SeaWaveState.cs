namespace BattleShip
{
    public enum SeaWaveState
    {
        Free = 0,
        WreckNearby = -1,
        Wreck = -10,
        ShipNearby = 1,
        Ship = 10,
        Disturbed = int.MaxValue,  
    }
}