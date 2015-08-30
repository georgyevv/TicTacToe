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
        private LoginData loginData;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_RegisterClick(object sender, RoutedEventArgs e)
        {
            Register();
        }


        private void Button_LogClick(object sender, RoutedEventArgs e)
        {
            LogIn();
        }

        private async void LogIn()
        {
            var httpClient = new HttpClient();

            var email = this.LogEmail.Text;
            var password = this.LogPassword.Password;

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", email),
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("confirmPassword", password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });

            var response = await httpClient.PostAsync(Endpoint.Log, content);
            loginData = await response.Content.ReadAsAsync<LoginData>();

            MessageBox.Show(response.IsSuccessStatusCode
                ? "Sucessfuly logged!"
                : await response.Content.ReadAsStringAsync());

            RoomWindow gm = new RoomWindow(loginData);
            gm.Show();
            this.Close();
        }

        private async void Register()
        {
            var httpClient = new HttpClient();

            var userName = this.RegisterUserName.Text;
            var email = this.RegisterEmail.Text;
            var password = this.RegisterPassword.Password;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("ConfirmPAssword", password),
                new KeyValuePair<string, string>("grant_type", "password")
            });

            var response = await httpClient.PostAsync(Endpoint.Register, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Sucessfuly registrated!");
            }
            else
            {
                await response.Content.ReadAsStringAsync();
                LogIn();
            }
        }
    }
}
