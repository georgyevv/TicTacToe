using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using TicTacToe.DesktopClient.Game;
using TicTacToe.DesktopClient.User;

namespace TicTacToe.DesktopClient
{
    /// <summary>
    /// Interaction logic for CreateGameWindow.xaml
    /// </summary>
    public partial class CreateGameWindow : Window
    {
        private LoginData loginData;

        public CreateGameWindow(LoginData loginData)
        {
            InitializeComponent();
            this.loginData = loginData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateGame();
        }

        private async void CreateGame()
        {
            HttpClient httpClient = new HttpClient();
            var gameName = this.TextBoxGameName.Text;
            var bearer = "Bearer " + loginData.Access_Token;

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("name", gameName)
                });

            httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = await httpClient.PostAsync(Endpoint.CreateGame, content);
            var moreGameInfo = response.Content.ReadAsAsync<GameData>().Result;

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
