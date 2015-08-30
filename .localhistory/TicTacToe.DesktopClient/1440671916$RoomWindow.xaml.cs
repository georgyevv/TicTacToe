using System.Net.Http;
using TicTacToe.DesktopClient.Common;
using TicTacToe.DesktopClient.Game;
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

        public RoomWindow(LoginData loginData)
        {
            this.loginData = loginData;
            InitializeComponent();
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
    }
}
