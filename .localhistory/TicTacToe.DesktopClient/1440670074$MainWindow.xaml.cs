namespace TicTacToe.DesktopClient
{
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_RegisterClick(object sender, RoutedEventArgs e)
        {
            var httpClient = new HttpClient();
            Button button = sender as Button;

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

            var response = httpClient.PostAsync(Endpoints.Register, content).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Sucessfuly registrated!");
            }
            else
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result);
            }

            GameWindow gm = new GameWindow();
            gm.Show();
            this.Close();
        }

        private void Button_LogClick(object sender, RoutedEventArgs e)
        {
            var httpClient = new HttpClient();
            Button button = sender as Button;

            var email = this.RegisterEmail.Text;
            var password = this.RegisterPassword.Password;

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", email),
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("confirmPassword", password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });

            var response = httpClient.PostAsync(Endpoints.Log, content).Result;

            MessageBox.Show(response.IsSuccessStatusCode
                ? "Sucessfuly logged!"
                : response.Content.ReadAsStringAsync().Result);

            GameWindow gm = new GameWindow();
            gm.Show();
            this.Close();
        }
    }
}
