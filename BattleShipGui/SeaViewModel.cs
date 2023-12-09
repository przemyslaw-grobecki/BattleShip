using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using BattleShip;

namespace BattleShipGui;

public class SeaViewModel : INotifyPropertyChanged
{
    public SeaViewModel()
    {
        BlueButtonClickCommand = new RelayCommand(BlueButtonClickhandler);
        RedButtonClickCommand = new RelayCommand(RedButtonClickhandler);
        BlueSea = new ObservableCollection<WaterButtonModel>();
        RedSea = new ObservableCollection<WaterButtonModel>();
        UpdateBlueSea();
        UpdateRedSea();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<WaterButtonModel> BlueSea { get; }
    public ObservableCollection<WaterButtonModel> RedSea { get; }
    
    public ICommand BlueButtonClickCommand { get; }

    private void BlueButtonClickhandler(object param)
    {
        if (param is not WaterButtonModel entry)
        {
            return;
        }
        
        Sea.GetInstance().BlueWaters.TrySetState(entry.WaterIdentifier, SeaWaveState.Ship);
        UpdateBlueSea();
    }
    
    
    private void UpdateBlueSea()
    {
        BlueSea.Clear();
        var blueSea = Sea.GetInstance().BlueWaters.States;
        foreach (var water in blueSea)
        {
            BlueSea.Add(new WaterButtonModel()
            {
                WaterIdentifier = water.Key,
                Color = ButtonColorMapper.Map(water.Value)
            });
        }
        OnPropertyChanged(nameof(BlueSea));
    }
    
    
    public ICommand RedButtonClickCommand { get; }

    private void RedButtonClickhandler(object param)
    {
        if (param is not WaterButtonModel entry)
        {
            return;
        }
        
        Sea.GetInstance().RedWaters.TrySetState(entry.WaterIdentifier, SeaWaveState.Ship);
        UpdateRedSea();
    }
    
    
    private void UpdateRedSea()
    {
        RedSea.Clear();
        var redSea = Sea.GetInstance().RedWaters.States;
        foreach (var water in redSea)
        {
            RedSea.Add(new WaterButtonModel()
            {
                WaterIdentifier = water.Key,
                Color = ButtonColorMapper.Map(water.Value)
            });
        }
        OnPropertyChanged(nameof(RedSea));
    }
    
    private static class ButtonColorMapper
    {
        public static SolidColorBrush Map(SeaWaveState seaWaveState)
        {
            return seaWaveState switch
            {
                SeaWaveState.Free => new SolidColorBrush(Color.FromRgb(135, 206, 250)), // Azure
                SeaWaveState.WreckNearby => new SolidColorBrush(Color.FromRgb(255, 127, 80)), // Coral
                SeaWaveState.Wreck => new SolidColorBrush(Color.FromRgb(255, 0, 0)), // Red
                SeaWaveState.ShipNearby => new SolidColorBrush(Color.FromRgb(169, 169, 169)), // Gray
                SeaWaveState.Ship => new SolidColorBrush(Color.FromRgb(0, 255, 0)), // Chartreuse
                SeaWaveState.Disturbed => new SolidColorBrush(Color.FromRgb(75, 0, 130)), // Indigo
                _ => throw new ArgumentOutOfRangeException(nameof(seaWaveState), seaWaveState, null)
            };
        }

        public static SeaWaveState Map(SolidColorBrush brush)
        {
            Color color = ((SolidColorBrush)brush).Color;

            if (color == Color.FromRgb(135, 206, 250))
            {
                return SeaWaveState.Free;
            }

            if (color == Color.FromRgb(255, 127, 80))
            {
                return SeaWaveState.WreckNearby;
            }

            if (color == Color.FromRgb(255, 0, 0))
            {
                return SeaWaveState.Wreck;
            }

            if (color == Color.FromRgb(169, 169, 169))
            {
                return SeaWaveState.ShipNearby;
            }

            if (color == Color.FromRgb(0, 255, 0))
            {
                return SeaWaveState.Ship;
            }

            if (color == Color.FromRgb(75, 0, 130))
            {
                return SeaWaveState.Disturbed;
            }

            throw new ArgumentOutOfRangeException(nameof(color), color, null);
        }
    }
}

