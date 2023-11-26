using System;
using System.Collections.Generic;

namespace BattleShipEngine
{
    public class Ship
    {
        public ShipType Type { get; private set; }
        public Team Team { get; private set; }
        public List<Tuple<int, int>> Deck { get; private set; }
    }
}