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
                foreach (var i in new[]{ -1, 0, 1 })
                {
                    foreach (var j in new[]{ -1, 0, 1 })
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
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
            (int, int) watersBehindTheShip1;
            (int, int) watersBeforeTheShip1;
            (int, int) watersBehindTheShip2;
            (int, int) watersBeforeTheShip2;
            (int, int) watersBehindTheShip3;
            (int, int) watersBeforeTheShip3;
            if (isDeckHorizontal)
            {
                watersBehindTheShip1 = (deckStart.Item1 - 1, deckStart.Item2);
                watersBeforeTheShip1 = (deckEnd.Item1 + 1, deckEnd.Item2);
                watersBehindTheShip2 = (deckStart.Item1 - 1, deckStart.Item2 + 1);
                watersBeforeTheShip2 = (deckEnd.Item1 + 1, deckEnd.Item2 + 1);
                watersBehindTheShip3 = (deckStart.Item1 - 1, deckStart.Item2 - 1);
                watersBeforeTheShip3 = (deckEnd.Item1 + 1, deckEnd.Item2 - 1);
            }
            else
            {
                watersBehindTheShip1 = (deckStart.Item1, deckStart.Item2 - 1);
                watersBeforeTheShip1 = (deckEnd.Item1, deckEnd.Item2 + 1);
                watersBehindTheShip2 = (deckStart.Item1 + 1, deckStart.Item2 - 1);
                watersBeforeTheShip2 = (deckEnd.Item1 + 1, deckEnd.Item2 + 1);
                watersBehindTheShip3 = (deckStart.Item1 - 1, deckStart.Item2 - 1);
                watersBeforeTheShip3 = (deckEnd.Item1 - 1, deckEnd.Item2 + 1);
            }

            if (IsInBoundary(watersBeforeTheShip1))
            {
                neighbouringSpace.Add(watersBeforeTheShip1);
            }

            if (IsInBoundary(watersBehindTheShip1))
            {
                neighbouringSpace.Add(watersBehindTheShip1);
            }
            
            if (IsInBoundary(watersBeforeTheShip2))
            {
                neighbouringSpace.Add(watersBeforeTheShip2);
            }

            if (IsInBoundary(watersBehindTheShip2))
            {
                neighbouringSpace.Add(watersBehindTheShip2);
            }           
            if (IsInBoundary(watersBeforeTheShip3))
            {
                neighbouringSpace.Add(watersBeforeTheShip3);
            }

            if (IsInBoundary(watersBehindTheShip3))
            {
                neighbouringSpace.Add(watersBehindTheShip3);
            }

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