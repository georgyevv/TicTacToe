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
    using Microsoft.AspNet.SignalR.Client;

    public partial class GameWindow : Window
    {
        ////////////////////////////////////
        public IHubProxy HubProxy { get; set; }
        //private const string ServerURI = "http://tictactoe-18.apphb.com/signalr";
        private const string ServerURI = "http://localhost:61587/signalr"; // TODO: Comment this in production!
        public HubConnection Connection { get; set; }
        ///////////////////////////////////
        private LoginData loginData;
        private GameData gameData;
        private string _placement;

        public GameWindow(LoginData loginData, GameData gameData, string placement, bool calledFromMainWindow = false)
        {
            InitializeComponent();
            this.loginData = loginData;
            this.gameData = gameData;
            this._placement = placement;

            this.LabelFirstPlayer.Content = gameData.FirstPlayerName;
            this.LabelSecondPlayer.Content = gameData.SecondPlayerName;
            this.LabelGameState.Content = gameData.GameState;
            this.LabelGameName.Content = gameData.Name;

            HubConnectAsync();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.SetPlacement(this._placement);
        }

        private void FieldButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.gameData.GameState == "FirstPlayerTurn" ||
                this.gameData.GameState == "SecondPlayerTurn")
            {
                HttpClient _httpClient = new HttpClient();
                Button button = sender as Button;
                var buttonPosition = button.Name.Substring(button.Name.IndexOf("button") + "button".Length + 1);
                var bearer = "Bearer " + loginData.Access_Token;

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id", gameData.Id.ToString()),
                    new KeyValuePair<string, string>("Position", buttonPosition),
                });

                _httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
                var response = _httpClient.PostAsync(Endpoint.MakeMove, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    button.Content = this.gameData.GameState == "FirstPlayerTurn" ? 'O' : 'X';
                    HubProxy.Invoke("Update");
                }
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private void QuitGameButtonClick(object sender, RoutedEventArgs e)
        {
            QuitCurrentGame();
        }

        private void ExitGameButtonClick(object sender, RoutedEventArgs e)
        {
            OtherPlayerLeft();
            Application.Current.Shutdown();
        }

        private async void QuitCurrentGame()
        {
            OtherPlayerLeft();
            var placement = this.GetPlacement();
            RoomWindow roomWindow = new RoomWindow(this.loginData, placement);
            roomWindow.Show();
            this.Close();
    }

        private async void RefreshGameInfo()
        {
            var bearer = "Bearer " + loginData.Access_Token;
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = await _httpClient.GetAsync(Endpoint.GameById + gameData.Id);
            this.gameData = await response.Content.ReadAsAsync<GameData>();

            if (response.IsSuccessStatusCode)
            {
                this.LabelGameName.Content = gameData.Name;
                this.LabelGameState.Content = gameData.GameState;
                this.LabelFirstPlayer.Content = gameData.FirstPlayerName;
                this.LabelSecondPlayer.Content = gameData.SecondPlayerName;
                this.Button0.Content = gameData.Field[0] == '0' ? ' ' : gameData.Field[0];
                this.Button1.Content = gameData.Field[1] == '0' ? ' ' : gameData.Field[1];
                this.Button2.Content = gameData.Field[2] == '0' ? ' ' : gameData.Field[2];
                this.Button3.Content = gameData.Field[3] == '0' ? ' ' : gameData.Field[3];
                this.Button4.Content = gameData.Field[4] == '0' ? ' ' : gameData.Field[4];
                this.Button5.Content = gameData.Field[5] == '0' ? ' ' : gameData.Field[5];
                this.Button6.Content = gameData.Field[6] == '0' ? ' ' : gameData.Field[6];
                this.Button7.Content = gameData.Field[7] == '0' ? ' ' : gameData.Field[7];
                this.Button8.Content = gameData.Field[8] == '0' ? ' ' : gameData.Field[8];
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
        }

        private async void HubConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("GameHub");
            HubProxy.On("RefreshGameInfo", () =>
                this.Dispatcher.Invoke(RefreshGameInfo));
            HubProxy.On("OtherPlayerLeft", () => this.Dispatcher.Invoke(OtherPlayerLeft));

            await Connection.Start();
            HubProxy.Invoke("Update"); //TODO: We need this shiet 
        }

        private void Connection_Closed()
        {
            //Hide chat UI; show login UI 
            var dispatcher = Application.Current.Dispatcher;
            //dispatcher.Invoke(() => ChatPanel.Visibility = Visibility.Collapsed);
            //dispatcher.Invoke(() => ButtonSend.IsEnabled = false);
            dispatcher.Invoke(() => LabelGameState.Content = "You have been disconnected.");
            //dispatcher.Invoke(() => SignInPanel.Visibility = Visibility.Visible);
        }

        public async void OtherPlayerLeft()
        {
            HttpClient _httpClient = new HttpClient();
            this.gameData.GameState = "Other player left!";
            this.LabelGameState.Content = this.gameData.GameState;
            await _httpClient.PostAsync(Endpoint.ChangeGameState + gameData.Id, null);
        }

        private void WPFClient_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HubProxy.Invoke("PlayerDisconnect");
            if (Connection != null)
            {
                Connection.Stop();
                Connection.Dispose();
            }
        }
    }
}