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

        public Waters BlueWaters { get; } = new();
        public Waters RedWaters { get; } = new();

        private readonly SemaphoreSlim mutex = new(1, 1);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="water"></param>
        /// <param name="teamUnderFire"></param>
        /// <returns></returns>

        public async Task<bool> TrySinkShip((int, int) water, Team teamUnderFire)
        {
            await mutex.WaitAsync();
            try
            {
                var watersUnderFire = teamUnderFire == Team.Blue ? BlueWaters : RedWaters;
                if (watersUnderFire.IsStateEqualTo(water, SeaWaveState.Ship))
                {
                    return watersUnderFire.TrySetState(water, SeaWaveState.Wreck);
                }
                else
                {
                    watersUnderFire.TrySetState(water, SeaWaveState.Disturbed);
                    return false;
                }
            }
            finally
            {
                mutex.Release();
            }
        }
        
        /// <summary>
        /// Sets the sea states for a given ship. Method to be used only on boarding phase.
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        public async Task<bool> TryBoardShip(Ship ship)
        {
            await mutex.WaitAsync();
            try
            {
                var boardingWaters = ship.Team == Team.Blue ? BlueWaters : RedWaters;
                var neighbourhood = ship.GetNeighbouringSpace();
                if (neighbourhood.Any(space => !(boardingWaters.IsStateEqualTo(space, SeaWaveState.Free) ||
                                                     boardingWaters.IsStateEqualTo(space, SeaWaveState.ShipNearby))))
                {
                    return false;
                }

                if (ship.Deck.Any(board => !boardingWaters.IsStateEqualTo(board, SeaWaveState.Free)))
                {
                    return false;
                }
                
                foreach (var board in ship.Deck)
                {
                    boardingWaters.TrySetState(board, SeaWaveState.Ship);
                }

                foreach (var neighbouringSpace in neighbourhood)
                {
                    boardingWaters.TrySetState(neighbouringSpace, SeaWaveState.ShipNearby);
                } 
            }
            finally
            {
                mutex.Release();
            }
            return true;
        }
        
        /// <summary>
        /// Removes ship
        /// </summary>
        /// <param name="shipToBeRemoved"></param>
        /// <param name="teamOfShipRemoval"></param>
        /// <returns></returns>
        public async Task<bool> TryRemoveShip(Ship shipToBeRemoved)
        {
            await mutex.WaitAsync();
            try
            {
                var watersOfRemoval = shipToBeRemoved.Team == Team.Blue ? BlueWaters : RedWaters;
                var neighbourhood = shipToBeRemoved.GetNeighbouringSpace();
                foreach (var spaceToRemove in neighbourhood.Concat(shipToBeRemoved.Deck).ToList())
                {
                    watersOfRemoval.TrySetState(spaceToRemove, SeaWaveState.Free);
                }
                return true;
            }
            finally
            {
                mutex.Release();
            }
        }
    }
}