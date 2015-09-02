﻿using System.Net.Http;
using TicTacToe.DesktopClient.User;

namespace TicTacToe.DesktopClient
{
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

    public partial class RoomWindow : Window
    {
        private LoginData loginData;

        public RoomWindow()
        {
            InitializeComponent();
            var httpClient = new HttpClient();
            var commandResult = String.Empty;
            var bearer = "Bearer " + loginData.Access_Token;

            httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = await httpClient.GetAsync(Endpoint.AvailableGames);

            if (response.IsSuccessStatusCode)
            {
                var games = await response.Content
                    .ReadAsAsync<IEnumerable<GameData>>();

                foreach (var gameData in games)
                {
                    commandResult += String.Format("gameId- {0}, player- {1}, state- {2}", gameData.Id,
                        gameData.PlayerOne, gameData.State);
                }
            }
            else
            {
                commandResult = await response.Content.ReadAsStringAsync();
            }

            lbTodoList.ItemsSource = items;
        }
    }
}
