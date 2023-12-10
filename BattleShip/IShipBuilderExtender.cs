namespace BattleShip;

public interface IShipBuilderExtender
{
    public IShipBuilderExtender Extend((int, int) deckPart);
    public IShipBuilderFinisher Finish();
}