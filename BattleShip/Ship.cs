namespace BattleShip
{
    public record Ship
    {
        public ShipType Type { get; init; }
        public Team Team { get; init; }
        public List<(int, int)> Deck { get; init; }
    }
}