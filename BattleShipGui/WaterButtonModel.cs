using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace BattleShipGui;

public sealed class WaterButtonModel : INotifyPropertyChanged
{
    public (int, int) WaterIdentifier { get; init; }

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
}