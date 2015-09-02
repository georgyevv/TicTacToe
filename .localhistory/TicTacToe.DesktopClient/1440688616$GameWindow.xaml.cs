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

        public GameWindow(LoginData loginData, GameData gameData)
        {
            InitializeComponent();
            this.loginData = loginData;
            this.gameData = gameData;

            this.LabelFirstPlayer.Content = gameData.FirstPlayerName;
            this.LabelSecondPlayer.Content = gameData.SecondPlayerName;
            this.LabelGameState.Content = gameData.GameState;
            this.LabelGameName.Content = gameData.Name;

            RefreshGameInfo();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.gameData.GameState == "FirstPlayerTurn" ||
                this.gameData.GameState == "SecondPlayerTurn")
            {
                HttpClient httpClient = new HttpClient();
                Button button = sender as Button;
                var buttonPosition = button.Name.Substring(button.Name.IndexOf("button") + "button".Length + 1);

                var bearer = "Bearer " + loginData.Access_Token;
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id", gameData.Id.ToString()), 
                    new KeyValuePair<string, string>("Position", buttonPosition), 
                });

                httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
                var response = httpClient.PostAsync(Endpoint.MakeMove, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    button.Content = this.gameData.GameState == "FirstPlayerTurn" ? '1' : '2';
                }
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private void Button_ClickRefresh(object sender, RoutedEventArgs e)
        {
            RefreshGameInfo();
        }

        private void Button_ClickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void RefreshGameInfo()
        {
            HttpClient httpClient = new HttpClient();
            var bearer = "Bearer " + loginData.Access_Token;

            httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = await httpClient.GetAsync(Endpoint.GameById + gameData.Id);
            var moreGameInfo = await response.Content.ReadAsAsync<GameData>();

            if (response.IsSuccessStatusCode)
            {
                this.LabelGameName.Content = moreGameInfo.Name;
                this.LabelGameState.Content = moreGameInfo.GameState;
                this.LabelFirstPlayer.Content = moreGameInfo.FirstPlayerName;
                this.LabelSecondPlayer.Content = moreGameInfo.SecondPlayerName;
                this.Button0.Content = moreGameInfo.Field[0];
                this.Button1.Content = moreGameInfo.Field[1];
                this.Button2.Content = moreGameInfo.Field[2];
                this.Button3.Content = moreGameInfo.Field[3];
                this.Button4.Content = moreGameInfo.Field[4];
                this.Button5.Content = moreGameInfo.Field[5];
                this.Button6.Content = moreGameInfo.Field[6];
                this.Button7.Content = moreGameInfo.Field[7];
                this.Button8.Content = moreGameInfo.Field[8];

                this.gameData = moreGameInfo;
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
        }
    }
}