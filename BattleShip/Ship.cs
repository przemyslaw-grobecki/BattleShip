namespace BattleShip
{
    public record Ship
    {
        public bool IsFinished = false;

        private const int LowerBoundaryInclusive = 0;
        private const int UpperBoundaryExclusive = 10;

        public ShipOrientation Orientation
        {
            get
            {
                if (IsFinished)
                {
                    return Deck.Count switch
                    {
                        < 1 => ShipOrientation.Unknown,
                        1 => ShipOrientation.Pointical,
                        _ => Deck[1].Item1 - Deck[0].Item1 != 0 ? ShipOrientation.Horizontal : ShipOrientation.Vertical
                    };
                }

                return ShipOrientation.Unknown;
            }
        }

        
        public ShipType Type { get; init; }
        public Team Team { get; init; }
        public List<(int, int)> Deck { get; init; } = new();
        public List<(int, int)> GetNeighbouringSpace()
        {
            var neighbouringSpace = new List<(int, int)>();
            var deckStart = Deck.First();
            if (Deck.Count == 1)
            {
                foreach (var i in new[]{ -1,1 })
                {
                    foreach (var j in new[]{ -1,1 })
                    {
                        var tempPosition = (deckStart.Item1 + i, deckStart.Item2 + j);
                        if (IsInBoundary(tempPosition))
                        {
                            neighbouringSpace.Add(tempPosition);
                        }
                    }
                }
                return neighbouringSpace;
            }

            var deckEnd = Deck.Last();
            bool isDeckHorizontal = (deckEnd.Item1 - deckStart.Item1) != 0;
            (int, int) watersBehindTheShip;
            (int, int) watersBeforeTheShip;
            if (isDeckHorizontal)
            {
                watersBehindTheShip = (deckStart.Item1 - 1, deckStart.Item2);
                watersBeforeTheShip = (deckEnd.Item1 + 1, deckEnd.Item2);
            }
            else
            {
                watersBehindTheShip = (deckStart.Item1, deckStart.Item2 - 1);
                watersBeforeTheShip = (deckEnd.Item1, deckEnd.Item2 + 1);
            }
            
            neighbouringSpace.Add(watersBeforeTheShip);
            neighbouringSpace.Add(watersBehindTheShip);

            foreach (var position in Deck)
            {
                if (isDeckHorizontal)
                {
                    var leftSideWaters = (position.Item1, position.Item2 + 1);
                    var rightSideWaters = (position.Item1, position.Item2 - 1);
                    if (IsInBoundary(leftSideWaters))
                    {
                        neighbouringSpace.Add(leftSideWaters);
                    }

                    if (IsInBoundary(rightSideWaters))
                    {
                        neighbouringSpace.Add(rightSideWaters);
                    }
                }
                else
                {
                    var leftSideWaters = (position.Item1 + 1, position.Item2);
                    var rightSideWaters = (position.Item1 - 1, position.Item2);
                    if (IsInBoundary(leftSideWaters))
                    {
                        neighbouringSpace.Add(leftSideWaters);
                    }

                    if (IsInBoundary(rightSideWaters))
                    {
                        neighbouringSpace.Add(rightSideWaters);
                    }
                }
            }

            return neighbouringSpace;
        }

        private static bool IsInBoundary((int, int) key)
        {
            return key.Item1 is >= LowerBoundaryInclusive and < UpperBoundaryExclusive &&
                   key.Item2 is >= LowerBoundaryInclusive and < UpperBoundaryExclusive;
        }
    }
}