using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TicTacToe.DesktopClient.Common;
using TicTacToe.DesktopClient.User;

namespace TicTacToe.DesktopClient.Windows
{
    public partial class ModesWindow : Window
    {
        private LoginData _loginData;
        private string _placement;

        public ModesWindow(LoginData loginData, string placement)
        {
            this._placement = placement;
            InitializeComponent();
            this._loginData = loginData;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (!string.IsNullOrEmpty(_placement))
            {
                this.SetPlacement(_placement);
            }
        }

        private void ButtonOnline_Click(object sender, RoutedEventArgs e)
        {
            var placement = this.GetPlacement();
            RoomWindow roomWindow = new RoomWindow(_loginData, placement);
            roomWindow.Show();
            this.Close();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonLogOff_Click(object sender, RoutedEventArgs e)
        {
            var placement = this.GetPlacement();
            MainWindow mainWindow = new MainWindow(placement);
            mainWindow.Show();
            this.Close();
        }

        private void ButtonMultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            var placement = this.GetPlacement();
            GameWindow gameWindow = new GameWindow(this._loginData, placement, GameMode.Multiplayer);
            gameWindow.Show();
            this.Close();
        }

        private void ButtonSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            var placement = this.GetPlacement();
            GameWindow gameWindow = new GameWindow(this._loginData, placement, GameMode.Singleplayer);
            gameWindow.Show();
            this.Close();
        }
    }
}
