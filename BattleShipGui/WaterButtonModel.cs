using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using BattleShip;

namespace BattleShipGui;

public sealed class WaterButtonModel : INotifyPropertyChanged
{
    public (int, int) WaterIdentifier { get; init; }

    public string Letter => ToLetter(WaterIdentifier.Item1);
    public string Number => WaterIdentifier.Item2.ToString();

    public string Signature
    {
        get
        {
            switch (State)
            {
                case SeaWaveState.Disturbed:
                    return "O";
                case SeaWaveState.Wreck:
                    return "X";
                default: return Letter + Number;;
            }
        }
    }

    public SeaWaveState State { get; set; }

    private SolidColorBrush color = new(Colors.Azure);

    public SolidColorBrush Color
    {
        get => color;
        set => SetField(ref color, value);
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private string ToLetter(int i)
    {
        return i switch
        {
            0 => "A",
            1 => "B",
            2 => "C",
            3 => "D",
            4 => "E",
            5 => "F",
            6 => "G",
            7 => "H",
            8 => "I",
            9 => "J",
            _ => ""
        };
    }
}