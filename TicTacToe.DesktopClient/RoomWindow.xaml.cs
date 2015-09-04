namespace TicTacToe.DesktopClient
{
    using Microsoft.AspNet.SignalR.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Windows;
    using TicTacToe.DesktopClient.Common;
    using TicTacToe.DesktopClient.Game;
    using TicTacToe.DesktopClient.User;
    using TicTacToe.DesktopClient.Windows;

    public partial class RoomWindow : Window
    {
        private bool shouldCloseWindow;
        private LoginData loginData;
        private string _placement;

        public RoomWindow(LoginData loginData, string placement)
        {
            InitializeComponent();
            this.loginData = loginData;
            this._placement = placement;

            UpdateRooms();
            HubConnectAsync();
        }

        private IHubProxy HubProxy { get; set; }
        private HubConnection Connection { get; set; }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.SetPlacement(this._placement);
        }

        public async void UpdateRooms()
        {
            HttpClient _httpClient = new HttpClient();
            var bearer = "Bearer " + loginData.Access_Token;
            _httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = await _httpClient.GetAsync(Endpoint.AvailableGames);

            if (response.IsSuccessStatusCode)
            {
                var games = await response.Content
                    .ReadAsAsync<IEnumerable<OnlineGame>>();

                this.ListBoxAvailableGames.ItemsSource = games;
                if (games != null && !games.Any())
                {
                    this.LabelNoGamesAvailable.Visibility = Visibility.Visible;
                }
                else
                {
                    this.LabelNoGamesAvailable.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
        }

        protected override void OnContentRendered(EventArgs e)
        {
            if (shouldCloseWindow)
            {
                this.Close();
            }

            base.OnContentRendered(e);
        }

        private void ClickCreateGameButton(object sender, RoutedEventArgs e)
        {
            this.ButtonCancel.Visibility = Visibility.Visible;
            this.LabelNewGameHeader.Visibility = Visibility.Visible;
            this.TextBoxGameName.Visibility = Visibility.Visible;
            this.ButtonOk.Visibility = Visibility.Visible;
            this.LabelNewGameLabel.Visibility = Visibility.Visible;

            this.ListBoxAvailableGames.Visibility = Visibility.Collapsed;
            this.LabelAvailableGames.Visibility = Visibility.Collapsed;
            this.LabelNoGamesAvailable.Visibility = Visibility.Collapsed;

            this.ButtonCreateGame.IsEnabled = false;
            this.ButtonJoinGame.IsEnabled = false;
        }

        private void ClickJoinGameButton(object sender, RoutedEventArgs e)
        {
            if (this.ListBoxAvailableGames.SelectedItem != null)
            {
                this.ButtonJoinGame.IsEnabled = false;
                HttpClient _httpClient = new HttpClient();
                var selectedGame = this.ListBoxAvailableGames.SelectedItem as OnlineGame;
                var bearer = "Bearer " + loginData.Access_Token;

                var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("id", selectedGame.Id.ToString())
                });

                _httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
                var response = _httpClient.PostAsync(Endpoint.JoinGame, content).Result;
                var gameData = response.Content.ReadAsAsync<OnlineGame>().Result;

                if (response.IsSuccessStatusCode)
                {
                    var placement = this.GetPlacement();
                    GameWindow gameWindow = new GameWindow(loginData, placement, GameMode.Online, gameData);
                    gameWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private void ClickSubmitCreateGameButton(object sender, RoutedEventArgs e)
        {
            this.ButtonOk.IsEnabled = false;
            CreateGame();
        }

        private void ClickCancelButton(object sender, RoutedEventArgs e)
        {
            this.ButtonCancel.Visibility = Visibility.Collapsed;
            this.LabelNewGameHeader.Visibility = Visibility.Collapsed;
            this.TextBoxGameName.Visibility = Visibility.Collapsed;
            this.ButtonOk.Visibility = Visibility.Collapsed;
            this.LabelNewGameLabel.Visibility = Visibility.Collapsed;

            this.ListBoxAvailableGames.Visibility = Visibility.Visible;
            this.LabelAvailableGames.Visibility = Visibility.Visible;

            this.ButtonCreateGame.IsEnabled = true;
            this.ButtonJoinGame.IsEnabled = true;
            UpdateRooms();
        }

        private void ClickModesButton(object sender, RoutedEventArgs e)
        {
            var placment = this.GetPlacement();
            ModesWindow modesWindow = new ModesWindow(this.loginData, placment);
            modesWindow.Show();
            this.Close();
        }

        private void ClickExitButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void HubConnectAsync()
        {
            Connection = new HubConnection(Strings.ServerUri);
            HubProxy = Connection.CreateHubProxy("GameHub");
            HubProxy.On("UpdateRooms", () =>
                this.Dispatcher.Invoke(UpdateRooms));

            await Connection.Start();
        }

        private async void CreateGame()
        {
            HttpClient _httpClient = new HttpClient();
            var gameName = this.TextBoxGameName.Text;
            var bearer = "Bearer " + loginData.Access_Token;

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("name", gameName)
                });

            _httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = await _httpClient.PostAsync(Endpoint.CreateGame, content);
            var gameData = await response.Content.ReadAsAsync<OnlineGame>();

            if (response.IsSuccessStatusCode)
            {
                var placement = this.GetPlacement();
                GameWindow gameWindow = new GameWindow(loginData, placement, GameMode.Online, gameData);
                HubProxy.Invoke("UpdateRooms");
                gameWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
        }
    }
}