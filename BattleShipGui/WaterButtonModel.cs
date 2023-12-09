using System.Windows.Media;

namespace BattleShipGui;

public class WaterButtonModel
{
    public (int, int) WaterIdentifier { get; init; }
    public SolidColorBrush Color { get; set; } = new(Colors.Azure);
}