﻿namespace TicTacToe.DesktopClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net.Http;
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
    using TicTacToe.DesktopClient.Game;
    using TicTacToe.DesktopClient.User;

    public partial class GameWindow : Window
    {
        private LoginData loginData;
        private GameData gameData;

        public GameWindow(LoginData loginData, GameData gameData)
        {
            InitializeComponent();
            this.loginData = loginData;
            this.gameData = gameData;

            this.LabelFirstPlayer.Content = gameData.FirstPlayerName;
            this.LabelSecondPlayer.Content = gameData.SecondPlayerName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var buttonName = button.Name.Substring(button.Name.IndexOf("button") + "button".Length); ;


        }
    }
}
