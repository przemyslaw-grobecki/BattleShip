using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BattleShip
{
    public class Sea
    {
        private static Sea? seaInstance;
        
        public static Sea GetInstance()
        {
            return seaInstance ??= new Sea();
        }
        
        private Sea()
        {
            
        }
        
        private static Waters waters = new();
        private readonly SemaphoreSlim mutex = new(1, 1);
        
        public bool TryGetShip((int, int) water, out Ship? ship)
        {
            return waters.TryGetValue(water, ship);
        }

        public async Task RemoveShip(Ship ship)
        {
            
        }
        
        public async Task AddShip(Ship ship)
        {
            await mutex.WaitAsync();
            try
            {
                var copy = waters.ToDictionary(entry => entry.Key,
                    entry => entry.Value);
                foreach (var boardPiece in ship.Deck)
                {
                    if (!copy.TryAdd(boardPiece,
                            ship.Team == Team.Blue ? SeaWaveState.BlueShipOccupied : SeaWaveState.RedShipOccupied))
                    {
                        throw new Exception("Already ship");
                    }
                }
            }
            finally
            {
                mutex.Release();
            }
        }
    }
}