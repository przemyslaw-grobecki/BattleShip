namespace BattleShip;

public static class RedShipsShipyard
{
    public static List<Ship> GenerateRedFleet()
    {
        return new List<Ship>
        {
            new()
            {
                Deck = new List<(int, int)>
                {
                    (0,0),
                    (1,0),
                    (2,0)
                },
                Type = ShipType.TripleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (0,2)
                },
                Type = ShipType.SingleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (0,9),
                },
                Type = ShipType.SingleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (1,4),
                },
                Type = ShipType.SingleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (1,6),
                    (2,6),
                },
                Type = ShipType.DoubleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (2,9),
                    (3,9),
                    (4,9)
                },
                Type = ShipType.TripleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (3,3)
                },
                Type = ShipType.SingleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (6,3),
                    (6,4),
                    (6,5),
                    (6,6)
                },
                Type = ShipType.QuadrupleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (8,4),
                    (9,4),
                },
                Type = ShipType.DoubleMasted,
                IsFinished = true,
                Team = Team.Red
            },
            new()
            {
                Deck = new List<(int, int)>
                {
                    (8,8),
                    (8,9),
                },
                Type = ShipType.DoubleMasted,
                IsFinished = true,
                Team = Team.Red
            },
        };
    }
}