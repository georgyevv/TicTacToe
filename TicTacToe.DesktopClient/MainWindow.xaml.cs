namespace TicTacToe.DesktopClient
{
    using TicTacToe.DesktopClient.User;
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
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using TicTacToe.DesktopClient.Common;

    public partial class MainWindow : Window
    {
        private LoginData _loginData;
        private HttpClient _httpClient;

        public MainWindow()
        {
            this._httpClient = new HttpClient();
            InitializeComponent();
            this.LogEmail.Focus();
        }

        private void ClickRegisterButton(object sender, RoutedEventArgs e)
        {
            Register();
        }

        private void ClickLogButton(object sender, RoutedEventArgs e)
        {
            LogIn();
        }

        private async void LogIn()
        {
            var email = this.LogEmail.Text;
            var password = this.LogPassword.Password;

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", email),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("confirmPassword", password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });

            var response = await this._httpClient.PostAsync(Endpoint.Log, content);
            var loginDataPlus = await response.Content.ReadAsAsync<LoginData>();
            this._loginData = loginDataPlus;

            if (response.IsSuccessStatusCode)
            {
                var placement = this.GetPlacement();
                RoomWindow roomWindow = new RoomWindow(_loginData, placement);
                roomWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
        }

        private async void Register()
        {
            var email = this.RegisterEmail.Text;
            var password = this.RegisterPassword.Password;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("ConfirmPAssword", password),
                new KeyValuePair<string, string>("grant_type", "password")
            });

            var response = await this._httpClient.PostAsync(Endpoint.Register, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Successfully registered!");
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
        }
    }
}