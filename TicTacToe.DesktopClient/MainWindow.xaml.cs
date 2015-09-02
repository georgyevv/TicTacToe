using TicTacToe.DesktopClient.Windows;

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
        private string _placement;

        public MainWindow()
        {
            this._httpClient = new HttpClient();
            InitializeComponent();
            this.LogUsername.Focus();
        }

        public MainWindow(string placement = null)
        {
            this._placement = placement;
            this._httpClient = new HttpClient();
            InitializeComponent();
            this.LogUsername.Focus();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (!string.IsNullOrEmpty(_placement))
            {
                this.SetPlacement(_placement);
            }
        }

        private void ClickRegisterButton(object sender, RoutedEventArgs e)
        {
            this.LogUsername.IsEnabled = false;
            this.LogPassword.IsEnabled = false;
            this.RegisterButton.IsEnabled = false;
            this.ButtonLog.IsEnabled = false;
            Register();
        }

        private void ClickLogButton(object sender, RoutedEventArgs e)
        {
            this.LogUsername.IsEnabled = false;
            this.LogPassword.IsEnabled = false;
            this.ButtonLog.IsEnabled = false;
            this.RegisterButton.IsEnabled = false;
            LogIn();
        }

        private async void LogIn()
        {
            var email = this.LogUsername.Text;
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
                ModesWindow roomWindow = new ModesWindow(_loginData, placement);
                roomWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
                this.ButtonLog.IsEnabled = true;
                this.RegisterButton.IsEnabled = true;
                this.LogUsername.IsEnabled = true;
                this.LogPassword.IsEnabled = true;
            }
        }

        private async void Register()
        {
            var email = this.LogUsername.Text;
            var password = this.LogPassword.Password;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", email),
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
            this.ButtonLog.IsEnabled = true;
            this.RegisterButton.IsEnabled = true;
            this.LogUsername.IsEnabled = true;
            this.LogPassword.IsEnabled = true;
        }
    }
}