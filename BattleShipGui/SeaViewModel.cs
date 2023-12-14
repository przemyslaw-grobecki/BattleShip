using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BattleShip;

namespace BattleShipGui;

public class SeaViewModel : INotifyPropertyChanged
{
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public SeaViewModel()
    {
        BlueButtonClickCommand = new RelayCommand(BlueButtonClickHandler);
        RedButtonClickCommand = new RelayCommand(RedButtonClickHandler);
        BoardingFinishedCommand = new RelayCommand(BoardingFinishedButtonHandler);
        SingleMastedShipBoardingButtonCommand = new RelayCommand(singleMastedShipBoardingButtonHandler);
        DoubleMastedShipBoardingButtonCommand = new RelayCommand(doubleMastedShipBoardingButtonHandler);
        TripleMastedShipBoardingButtonCommand = new RelayCommand(tripleMastedShipBoardingButtonHandler);
        QuadrupleMastedShipBoardingButtonCommand = new RelayCommand(quadrupleMastedShipBoardingButtonHandler);
        BlueSea = new ObservableCollection<WaterButtonModel>();
        RedSea = new ObservableCollection<WaterButtonModel>();
        UpdateBlueSea();
        UpdateRedSea();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    public Shipyard Shipyard { get; } = new();
    public ObservableCollection<WaterButtonModel> BlueSea { get; }
    public ObservableCollection<WaterButtonModel> RedSea { get; }
    public ICommand BlueButtonClickCommand { get; }

    private static int countdown = 0;
    private async void BlueButtonClickHandler(object param)
    {
        if (param is not WaterButtonModel waterButtonModel)
        {
            return;
        }

        if (Shipyard.CurrentShipBuilding is null) return;
        if (!Sea.GetInstance().BlueWaters.IsStateEqualTo(waterButtonModel.WaterIdentifier, SeaWaveState.Free)) return;
        try
        {
            Shipyard.CurrentShipBuilding.Extend(waterButtonModel.WaterIdentifier);
            BlueSea.FirstOrDefault(water => water.WaterIdentifier == waterButtonModel.WaterIdentifier)!.Color =
                new SolidColorBrush(Colors.Aqua);
            OnPropertyChanged(nameof(BlueSea));
            countdown--;
            if (countdown == 0)
            {
                Shipyard.FinishBuilding(out var ship);
                if (await Sea.GetInstance().TryBoardShip(ship!))
                {
                    UpdateBlueSea();
                }
            }
        }
        catch(Exception ex)
        {
            //nop
        }
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
                State = water.Value,
                Color = ButtonColorMapper.MapBlue(water.Value)
            });
        }
        OnPropertyChanged(nameof(BlueSea));
        if (blueSea.Values.Count(val => val == SeaWaveState.Wreck) == 20)
        {
            GameEngine.GetInstance().NextState();
            GameResult = "img/lose.png";
            OnPropertyChanged(nameof(GameResult));
            OnPropertyChanged(nameof(IsGameEnded));
        }
    }
    
    
    public ICommand RedButtonClickCommand { get; }

    private async void RedButtonClickHandler(object param)
    {
        if (param is not WaterButtonModel entry)
        {
            return;
        }

        var sea = Sea.GetInstance();
        if (sea.RedWaters.IsStateEqualTo(entry.WaterIdentifier, SeaWaveState.Wreck) ||
            sea.RedWaters.IsStateEqualTo(entry.WaterIdentifier, SeaWaveState.Disturbed))
        {
            return;
        }
        
        //Your shot
        if (!await sea.TrySinkShip(entry.WaterIdentifier, Team.Red))
        {
            //Your opponent shot
            while (await sea.TrySinkShip(sea.BlueWaters.States
                           .Where(kvp => kvp.Value != SeaWaveState.Wreck && kvp.Value != SeaWaveState.Disturbed)
                           .Shuffle(Random.Shared)
                           .First().Key,
                       Team.Blue))
            {
                //nop
            }
        }

        UpdateRedSea();
        UpdateBlueSea();
    }
    
    
    private void UpdateRedSea()
    {
        RedSea.Clear();
        var redSea = Sea.GetInstance().RedWaters.States;
        foreach (var water in redSea)
        {
            RedSea.Add(new WaterButtonModel
            {
                WaterIdentifier = water.Key,
                State = water.Value,
                Color = ButtonColorMapper.MapRed(water.Value)
            });
        }
        OnPropertyChanged(nameof(RedSea));
        if (redSea.Values.Count(val => val == SeaWaveState.Wreck) == 20)
        {
            GameEngine.GetInstance().NextState();
            GameResult = "img/win.png";
            OnPropertyChanged(nameof(GameResult));
            OnPropertyChanged(nameof(IsGameEnded));
        }
    }
    
    public ICommand SingleMastedShipBoardingButtonCommand { get; }
    public bool IsBuildingSingleMastedPossible => Shipyard.IsBuildingSingleMastedPossible;
    private readonly Action<object> singleMastedShipBoardingButtonHandler = param =>
    {
        var shipyard = param as Shipyard;
        if (shipyard.CurrentShipBuilding is not null) return;
        if (shipyard.TryStartBuilding(ShipType.SingleMasted, Team.Blue))
        {
            countdown = 1;
        }
    };
    
    public ICommand DoubleMastedShipBoardingButtonCommand { get; }
    public bool IsBuildingDoubleMastedPossible => Shipyard.IsBuildingDoubleMastedPossible;
    private readonly Action<object> doubleMastedShipBoardingButtonHandler = param =>
    {
        var shipyard = param as Shipyard;
        if (shipyard.CurrentShipBuilding is not null) return;
        if(shipyard.TryStartBuilding(ShipType.DoubleMasted, Team.Blue))
        {
            countdown = 2;
        }
    };
    
    public ICommand TripleMastedShipBoardingButtonCommand { get; }
    public bool IsBuildingTripleMastedPossible => Shipyard.IsBuildingTripleMastedPossible;
    private readonly Action<object> tripleMastedShipBoardingButtonHandler = param =>
    {
        var shipyard = param as Shipyard;
        if (shipyard.CurrentShipBuilding is not null) return;
        if (shipyard.TryStartBuilding(ShipType.TripleMasted, Team.Blue))
        {
            countdown = 3;
        }
    };
    
    public ICommand QuadrupleMastedShipBoardingButtonCommand { get; }
    public bool IsBuildingQuadrupleMastedPossible => Shipyard.IsBuildingQuadrupleMastedPossible;
    private readonly Action<object> quadrupleMastedShipBoardingButtonHandler = param =>
    {
        var shipyard = param as Shipyard;
        if (shipyard.CurrentShipBuilding is not null) return;
        if (shipyard.TryStartBuilding(ShipType.QuadrupleMasted, Team.Blue))
        {
            countdown = 4;
        }
    };

    public ICommand BoardingFinishedCommand { get; }

    private async void BoardingFinishedButtonHandler(object param)
    {
        var sea = Sea.GetInstance();
        var redFleet = RedShipsShipyard.GenerateRedFleet();
        foreach (var ship in redFleet)
        {
            await sea.TryBoardShip(ship);
        }
        
        GameEngine.GetInstance().NextState();
        OnPropertyChanged(nameof(ButtonsVisibility));
        OnPropertyChanged(nameof(IsBlueSeaEnabled));
        OnPropertyChanged(nameof(IsRedSeaEnabled));
        UpdateRedSea();
    }
    
    public Visibility ButtonsVisibility => GameEngine.GetInstance().CurrentState == GameState.ShipBoarding
        ? Visibility.Visible
        : Visibility.Hidden;

    public string GameResult { get; private set; } = "img/win.png";
    public Visibility IsGameEnded => GameEngine.GetInstance().CurrentState == GameState.End ? Visibility.Visible : Visibility.Hidden;
    public bool IsBlueSeaEnabled => GameEngine.GetInstance().CurrentState == GameState.ShipBoarding || GameEngine.GetInstance().CurrentState == GameState.ShipSinking;
    public bool IsRedSeaEnabled => GameEngine.GetInstance().CurrentState == GameState.ShipSinking;
    
    private static class ButtonColorMapper
    {
        public static SolidColorBrush MapBlue(SeaWaveState seaWaveState)
        {
            return (int)seaWaveState switch
            {
                0 => new SolidColorBrush(Color.FromRgb(135, 206, 250)), // Azure
                < 0 and > -10  => new SolidColorBrush(Color.FromRgb(255, 127, 80)), // Coral
                -10 => new SolidColorBrush(Color.FromRgb(255, 0, 0)), // Red
                > 0 and < 10   => new SolidColorBrush(Color.FromRgb(169, 169, 169)), // Gray
                10 => new SolidColorBrush(Color.FromRgb(0, 255, 0)), // Chartreuse
                int.MaxValue => new SolidColorBrush(Color.FromRgb(75, 0, 130)), // Indigo
                _ => throw new ArgumentOutOfRangeException(nameof(seaWaveState), seaWaveState, null)
            };
        }
        
        public static SolidColorBrush MapRed(SeaWaveState seaWaveState)
        {
            return (int)seaWaveState switch
            {
                0 => new SolidColorBrush(Color.FromRgb(135, 206, 250)), // Azure
                < 0 and > -10  => new SolidColorBrush(Color.FromRgb(135, 206, 250)), // Azure
                -10 => new SolidColorBrush(Color.FromRgb(255, 0, 0)), // Red
                > 0 and < 10   => new SolidColorBrush(Color.FromRgb(135, 206, 250)), // Azure
                10 => new SolidColorBrush(Color.FromRgb(135, 206, 250)), // Azure
                int.MaxValue => new SolidColorBrush(Color.FromRgb(75, 0, 130)), // Indigo
                _ => throw new ArgumentOutOfRangeException(nameof(seaWaveState), seaWaveState, null)
            };
        }
    }
}

