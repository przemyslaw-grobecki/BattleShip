using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BattleShipEngine
{
    public static class Sea
    {
        private static ConcurrentDictionary<KeyValuePair<int, int>, SeaWaveState> waters =
            new ConcurrentDictionary<KeyValuePair<int, int>, SeaWaveState>();

        public void BoardShip(Ship ship)
        {
            
        }
        
        
    }
}