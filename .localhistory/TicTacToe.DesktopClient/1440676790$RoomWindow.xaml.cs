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
    using System.Net.Http;
    using TicTacToe.DesktopClient.Common;
    using TicTacToe.DesktopClient.Game;
    using TicTacToe.DesktopClient.User;

    public partial class RoomWindow : Window
    {
        private LoginData loginData;

        public RoomWindow(LoginData loginData)
        {
            InitializeComponent();
            this.loginData = loginData;
            var httpClient = new HttpClient();
            var bearer = "Bearer " + loginData.Access_Token;

            httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = httpClient.GetAsync(Endpoints.AvailableGames).Result;

            if (response.IsSuccessStatusCode)
            {
                var games = response.Content
                    .ReadAsAsync<IEnumerable<GameData>>().Result;
                this.ListBoxAvailableGames.ItemsSource = games;
            }
            else
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result);
            }
        }

        private void Button_ClickCreateGame(object sender, RoutedEventArgs e)
        {
            CreateGameWindow cgw = new CreateGameWindow(this.loginData);
            cgw.Show();
            this.Close();
        }

        private void Button_ClickJoinGame(object sender, RoutedEventArgs e)
        {
            var selectedGame = this.ListBoxAvailableGames.SelectedItem as GameData;
            var selectedGameTest = this.ListBoxAvailableGames.SelectedItems;
            var selectedGameTest2 = this.ListBoxAvailableGames.SelectedItems.Cast<IEnumerable<GameData>>();

            var httpClient = new HttpClient();
            var bearer = "Bearer " + loginData.Access_Token;
            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id", selectedGame.Id.ToString())
                });

        }
    }
}