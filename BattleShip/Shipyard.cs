using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BattleShip;

public class Shipyard : INotifyPropertyChanged
{
    private int availableSingleMastedShips = 4;
    private int availableDoubleMastedShips = 3;
    private int availableTripleMastedShips = 2;
    private int availableQuadrupleMastedShips = 1;

    private ShipType? currentShipType = null;

    public bool IsBuildingSingleMastedPossible => availableSingleMastedShips > 0 && CurrentShipBuilding is null;
    public bool IsBuildingDoubleMastedPossible => availableDoubleMastedShips > 0 && CurrentShipBuilding is null;
    public bool IsBuildingTripleMastedPossible => availableTripleMastedShips > 0 && CurrentShipBuilding is null;
    public bool IsBuildingQuadrupleMastedPossible => availableQuadrupleMastedShips > 0 && CurrentShipBuilding is null;
    
    public IShipBuilderExtender? CurrentShipBuilding { get; private set; }

    public bool TryStartBuilding(ShipType type, Team team)
    {
        switch (type)
        {
            case ShipType.SingleMasted:
                if (availableSingleMastedShips < 1)
                {
                    return false;
                }
                break;
            case ShipType.DoubleMasted:
                if (availableDoubleMastedShips < 1)
                {
                    return false;
                }
                break;
            case ShipType.TripleMasted:
                if (availableTripleMastedShips < 1)
                {
                    return false;
                }
                break;
            case ShipType.QuadrupleMasted:
                if (availableTripleMastedShips < 1)
                {
                    return false;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        var builder = ShipBuilder
            .StartBuilding(team);
        CurrentShipBuilding = builder;
        currentShipType = type;
        OnPropertyChanged(nameof(IsBuildingSingleMastedPossible));
        OnPropertyChanged(nameof(IsBuildingDoubleMastedPossible));
        OnPropertyChanged(nameof(IsBuildingTripleMastedPossible));
        OnPropertyChanged(nameof(IsBuildingQuadrupleMastedPossible));
        return true;
    }

    public void FinishBuilding(out Ship? ship)
    {
        switch (currentShipType)
        {
            case ShipType.SingleMasted:
                availableSingleMastedShips--;
                break;
            case ShipType.DoubleMasted:
                availableDoubleMastedShips--;
                break;
            case ShipType.TripleMasted:
                availableTripleMastedShips--;
                break;
            case ShipType.QuadrupleMasted:
                availableQuadrupleMastedShips--;
                break;
            case null:
                ship = null;
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }

        ship = CurrentShipBuilding!.Finish().Build();
        CurrentShipBuilding = null;
        currentShipType = null;
        OnPropertyChanged(nameof(IsBuildingSingleMastedPossible));
        OnPropertyChanged(nameof(IsBuildingDoubleMastedPossible));
        OnPropertyChanged(nameof(IsBuildingTripleMastedPossible));
        OnPropertyChanged(nameof(IsBuildingQuadrupleMastedPossible));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}