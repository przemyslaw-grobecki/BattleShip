using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using BattleShip;

namespace BattleShipGui;

public class SeaViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ObservableCollection<(int, int)> _buttonPositions = new(Sea.GetInstance().BlueWaters.States.Keys.ToList());

    public ObservableCollection<(int, int)> ButtonPositions
    {
        get { return _buttonPositions; }
        set
        {
            if (_buttonPositions != value)
            {
                _buttonPositions = value;
                OnPropertyChanged(nameof(ButtonPositions));
            }
        }
    }

    public object ButtonClickCommand { get; }
}