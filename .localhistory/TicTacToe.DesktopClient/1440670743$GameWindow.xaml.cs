using TicTacToe.DesktopClient.Common;
using TicTacToe.DesktopClient.User;

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

    public partial class GameWindow : Window
    {
        private LoginData loginData; 

        public GameWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.Content = DateTime.Now;

            var httpClient = new HttpClient();
            var bearer = "Bearer " + loginData.Access_Token;

            httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = httpClient.PostAsync(Endpoints.CreateGame, null).Result;

            if (response.IsSuccessStatusCode)
            {
                ("Game sucessuly created!");
            }
        }
    }
}
