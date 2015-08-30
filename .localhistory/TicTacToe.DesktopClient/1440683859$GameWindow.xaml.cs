namespace TicTacToe.DesktopClient
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

        public GameWindow(LoginData loginData, GameData gameData, bool isClickedRefreshButton = false)
        {
            InitializeComponent();
            this.loginData = loginData;
            this.gameData = gameData;

            if (isClickedRefreshButton)
            {
                RefreshGameInfo();
            }

            this.LabelFirstPlayer.Content = gameData.FirstPlayerName;
            this.LabelSecondPlayer.Content = gameData.SecondPlayerName;
            this.LabelGameState.Content = gameData.GameState;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            Button button = sender as Button;
            var buttonName = button.Name.Substring(button.Name.IndexOf("button") + "button".Length);

            var bearer = "Bearer " + loginData.Access_Token;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", gameData.Id.ToString()), 
                new KeyValuePair<string, string>("x", gameData.Id.ToString()), 
                new KeyValuePair<string, string>("y", gameData.Id.ToString())
            });

            httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = httpClient.PostAsync(Endpoint.MakeMove, content).Result;

            if (response.IsSuccessStatusCode)
            {
                button.Content = "Good";
            }
            else
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result);
            }
        }

        private void Button_ClickRefresh(object sender, RoutedEventArgs e)
        {
            GameWindow gm = new GameWindow(this.loginData, this.gameData);
            gm.Show();
            this.Close();
        }

        private void Button_ClickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void RefreshGameInfo()
        {
            HttpClient httpClient = new HttpClient();
            var bearer = "Bearer " + loginData.Access_Token;

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id", this.gameData.Id.ToString())
                });

            httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = await httpClient.GetAsync(Endpoint.GameById);
            var moreGameInfo = await response.Content.ReadAsAsync<GameData>();

            if (response.IsSuccessStatusCode)
            {
                GameWindow gameWindow = new GameWindow(loginData, moreGameInfo);
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