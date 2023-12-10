namespace BattleShip;

public class ShipBuilder : IShipBuilderExtender, IShipBuilderFinisher
{
    private Ship shipToBuild;
    public static IShipBuilderExtender StartBuilding(Team team)
    {
        return new ShipBuilder(team);
    }
    private ShipBuilder(Team team)
    {
        shipToBuild = new Ship
        {
            Team = team
        };
    }
    public IShipBuilderExtender Extend((int, int) deckPart)
    {
        if (shipToBuild.Deck.Any())
        {
            if (shipToBuild.Deck.Contains(deckPart))
            {
                throw new Exception("Bad coordinate");
            }
            if (shipToBuild.Deck.Count > 1)
            {
                if (shipToBuild.Deck[0].Item1 == shipToBuild.Deck[1].Item1)
                {
                    if (shipToBuild.Deck[0].Item1 != deckPart.Item1)
                    {
                        throw new Exception("Bad coordinate");
                    }
                }
                else
                {
                    if (shipToBuild.Deck[0].Item2 != deckPart.Item2)
                    {
                        throw new Exception("Bad coordinate");
                    }
                }
            }

            var all = shipToBuild.Deck.FindAll(d => (d.Item1 + 1 == deckPart.Item1 && d.Item2 == deckPart.Item2) ||
                                                                      (d.Item1 - 1 == deckPart.Item1 && d.Item2 == deckPart.Item2) ||
                                                                      (d.Item1 == deckPart.Item1 && d.Item2 + 1 == deckPart.Item2) ||
                                                                      (d.Item1 == deckPart.Item1 && d.Item2 - 1 == deckPart.Item2));
            if (!all.Any())
            {
                throw new Exception("Bad coordinate");
            }
        }
        shipToBuild.Deck.Add(deckPart);
        return this;
    }

    public IShipBuilderFinisher Finish()
    {
        shipToBuild.IsFinished = true;
        shipToBuild = new Ship
        {
            Deck = shipToBuild.Deck
                .OrderBy(x => shipToBuild.Orientation == ShipOrientation.Horizontal ? x.Item2 : x.Item1).ToList(),
            IsFinished = true,
            Team = shipToBuild.Team
        };
        return this;
    }

    public Ship Build()
    {
        return shipToBuild;
    }
}