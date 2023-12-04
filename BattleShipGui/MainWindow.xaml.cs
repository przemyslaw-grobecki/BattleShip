using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BattleShip;

namespace BattleShipGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Sea sea = Sea.GetInstance();
        public MainWindow()
        {
            var BlueWaters = new ObservableCollection<int>();
            BlueWaters.Add(1);
            BlueWaters.Add(2);
            BlueWaters.Add(3);
            BlueWaters.Add(4);
            BlueWaters.Add(5);
            DataContext = BlueWaters;
            InitializeComponent();
        }
    }
}