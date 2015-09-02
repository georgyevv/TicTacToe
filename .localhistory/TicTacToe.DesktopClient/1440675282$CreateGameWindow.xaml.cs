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

namespace TicTacToe.DesktopClient
{
    /// <summary>
    /// Interaction logic for CreateGameWindow.xaml
    /// </summary>
    public partial class CreateGameWindow : Window
    {

        private LoginData loginData;

        public CreateGameWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            var gameName = this.TextBoxGameName.Text;

            var bearer = "Bearer " + loginData.Access_Token;

            httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = httpClient.PostAsync(Endpoints.CreateGame, null).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Game sucessuly created!");
            }
            else
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result);
            }

            this.Close();
        }
    }
}
