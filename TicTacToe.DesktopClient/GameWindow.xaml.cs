namespace TicTacToe.DesktopClient
{
    using Microsoft.AspNet.SignalR.Client;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Windows;
    using System.Windows.Controls;
    using TicTacToe.DesktopClient.Common;
    using TicTacToe.DesktopClient.Game;
    using TicTacToe.DesktopClient.User;
    using TicTacToe.DesktopClient.Windows;

    public partial class GameWindow : Window
    {
        private LoginData loginData;
        private IGameData gameData;
        private string _placement;
        private GameMode gameMode;

        public GameWindow(LoginData loginData, string placement, GameMode gameMode, OnlineGame gameData = null)
        {
            InitializeComponent();
            this._placement = placement;
            this.gameMode = gameMode;
            this.loginData = loginData;
            if (gameMode == GameMode.Online)
            {
                this.gameData = gameData;

                this.LabelFirstPlayer.Content = gameData.FirstPlayerName;
                this.LabelSecondPlayer.Content = gameData.SecondPlayerName;
                this.LabelGameState.Content = gameData.GameState;
                this.LabelGameName.Content = gameData.Name;

                HubConnectAsync();
            }
            else if (gameMode == GameMode.Multiplayer)
            {
                this.LabelFirstPlayer.Content = this.loginData.UserName;
                this.LabelGameName.Content = "Multiplayer";
                this.LabelSecondPlayer.Content = "Other guy";
                this.gameData = new MultiPlayerGame()
                {
                    FirstPlayerName = this.loginData.UserName,
                    SecondPlayerName = "Other guy",
                    EnumGameState = GameState.FirstPlayer,
                    Field = new String('0', 9),
                    Name = "Multiplayer"
                };
            }
            else if (gameMode == GameMode.Singleplayer)
            {
                this.LabelFirstPlayer.Content = this.loginData.UserName;
                this.LabelGameName.Content = "Singleplayer";
                this.LabelSecondPlayer.Content = "Bot";
                this.gameData = new SinglePlayerGame()
                {
                    FirstPlayerName = this.loginData.UserName,
                    EnumGameState = GameState.FirstPlayer,
                    Field = new String('0', 9),
                    Name = "Singleplayer"
                };
            }
        }

        public IHubProxy HubProxy { get; set; }
        public HubConnection Connection { get; set; }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.SetPlacement(this._placement);
        }

        private void FieldButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var buttonPosition = button.Name.Substring(button.Name.IndexOf("button") + "button".Length + 1);

            if (this.gameMode == GameMode.Online)
            {
                if (this.gameData.GameState == "FirstPlayerTurn" ||
                    this.gameData.GameState == "SecondPlayerTurn")
                {
                    HttpClient _httpClient = new HttpClient();
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
            else if (this.gameMode == GameMode.Singleplayer)
            {
                if (this.gameData.EnumGameState == GameState.FirstPlayer)
                {
                    SinglePlayerMove(buttonPosition, button);
                    AIMove(buttonPosition, button);
                }
            }
            else if (this.gameMode == GameMode.Multiplayer)
            {
                if (this.gameData.EnumGameState == GameState.FirstPlayer ||
                    this.gameData.EnumGameState == GameState.SecondPlayer)
                {
                    MultiPlayerMove(buttonPosition, button);
                }
            }
        }

        private async Task AIMove(string buttonPosition, Button button)
        {
            if (this.gameData.EnumGameState == GameState.SecondPlayer)
            {
                Thread.Sleep(100);
                Random rand = new Random();
                var aiPosition = rand.Next(0, 9);
                List<Button> buttons = new List<Button>()
                {
                    this.Button0,
                    this.Button1,
                    this.Button2,
                    this.Button3,
                    this.Button4,
                    this.Button5,
                    this.Button6,
                    this.Button7,
                    this.Button8,
                };

                while (this.gameData.Field[aiPosition] != '0')
                {
                    aiPosition = rand.Next(0, 9);
                }

                buttons[aiPosition].Content = "X";
                StringBuilder stringBuilder = new StringBuilder(this.gameData.Field);
                stringBuilder[aiPosition] = 'X';
                this.gameData.Field = stringBuilder.ToString();
                this.gameData.EnumGameState = GameState.FirstPlayer;

                var winner = CheckForWinner(stringBuilder.ToString());

                if (winner == GameState.WinFirstPlayer)
                {
                    this.gameData.EnumGameState = GameState.WinFirstPlayer;
                    MessageBox.Show("You win!");
                    return;
                }
                if (winner == GameState.WinSecondPlayer)
                {
                    this.gameData.EnumGameState = GameState.WinSecondPlayer;
                    MessageBox.Show("Bot win!");
                    return;
                }
                if (winner == GameState.Draw)
                {
                    this.gameData.EnumGameState = GameState.Draw;
                    MessageBox.Show("Draw!");
                    return;
                }
            }
        }

        private void SinglePlayerMove(string buttonPosition, Button button)
        {
            if (this.gameData.EnumGameState == GameState.FirstPlayer)
            {
                var field = new StringBuilder(this.gameData.Field);

                if (field[int.Parse(buttonPosition)] != '0')
                {
                    MessageBox.Show("You cannot place there!");
                    return;
                }

                button.Content = "O";
                field[int.Parse(buttonPosition)] = 'O';
                this.gameData.Field = field.ToString();

                var winner = CheckForWinner(field.ToString());

                if (winner == GameState.WinFirstPlayer)
                {
                    this.gameData.EnumGameState = GameState.WinFirstPlayer;
                    MessageBox.Show("You win!");
                    return;
                }
                if (winner == GameState.WinSecondPlayer)
                {
                    this.gameData.EnumGameState = GameState.WinSecondPlayer;
                    MessageBox.Show("Bot win!");
                    return;
                }
                if (winner == GameState.Draw)
                {
                    this.gameData.EnumGameState = GameState.Draw;
                    MessageBox.Show("Draw!");
                    return;
                }

                this.gameData.EnumGameState = GameState.SecondPlayer;
            }
        }

        private void MultiPlayerMove(string buttonPosition, Button button)
        {
            var field = new StringBuilder(this.gameData.Field);

            if (field[int.Parse(buttonPosition)] != '0')
            {
                MessageBox.Show("You cannot place there!");
                return;
            }

            field[int.Parse(buttonPosition)] = this.gameData.EnumGameState == GameState.FirstPlayer ? 'O' : 'X';
            this.gameData.Field = field.ToString();
            button.Content = field[int.Parse(buttonPosition)];

            var winner = CheckForWinner(this.gameData.Field);

            if (winner == GameState.WinFirstPlayer)
            {
                this.gameData.EnumGameState = GameState.WinFirstPlayer;
                MessageBox.Show(this.gameData.FirstPlayerName + " win!");
                return;
            }
            if (winner == GameState.WinSecondPlayer)
            {
                this.gameData.EnumGameState = GameState.WinSecondPlayer;
                MessageBox.Show("Other guy win!");
                return;
            }
            if (winner == GameState.Draw)
            {
                this.gameData.EnumGameState = GameState.Draw;
                MessageBox.Show("Draw!");
                return;
            }

            this.gameData.EnumGameState = this.gameData.EnumGameState == GameState.FirstPlayer
                ? GameState.SecondPlayer : GameState.FirstPlayer;
        }

        private void QuitGameButtonClick(object sender, RoutedEventArgs e)
        {
            QuitCurrentGame();
        }

        private void ExitGameButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.gameMode == GameMode.Online)
            {
                OtherPlayerLeft();
            }
            Application.Current.Shutdown();
        }

        private async void QuitCurrentGame()
        {
            if (this.gameMode == GameMode.Online)
            {
                OtherPlayerLeft();
            }
            var placement = this.GetPlacement();
            ModesWindow modesWindow = new ModesWindow(this.loginData, placement);
            modesWindow.Show();
            this.Close();
        }

        private async void RefreshGameInfo()
        {
            var bearer = "Bearer " + loginData.Access_Token;
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", bearer);
            var response = await _httpClient.GetAsync(Endpoint.GameById + gameData.Id);
            this.gameData = await response.Content.ReadAsAsync<OnlineGame>();

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
            Connection = new HubConnection(Strings.ServerUri);
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
            if (this.gameMode == GameMode.Online)
            {
                HubProxy.Invoke("PlayerDisconnect");
                if (Connection != null)
                {
                    Connection.Stop();
                    Connection.Dispose();
                }
            }
        }

        private GameState CheckForWinner(string field)
        {
            if (field.All(e => e != '0'))
            {
                return GameState.Draw;
            }

            var firstPlayerIsWinner = Winner('O', field);
            var secondPlayerIsWinner = Winner('X', field);

            if (firstPlayerIsWinner)
            {
                return GameState.WinFirstPlayer;
            }

            if (secondPlayerIsWinner)
            {
                return GameState.WinSecondPlayer;
            }

            return GameState.NoWinner;
        }

        private bool Winner(char playerMark, string field)
        {
            if (field[0] == playerMark && field[1] == playerMark && field[2] == playerMark)
            {
                return true;
            }

            if (field[3] == playerMark && field[4] == playerMark && field[5] == playerMark)
            {
                return true;
            }

            if (field[6] == playerMark && field[7] == playerMark && field[8] == playerMark)
            {
                return true;
            }

            if (field[0] == playerMark && field[3] == playerMark && field[6] == playerMark)
            {
                return true;
            }

            if (field[1] == playerMark && field[4] == playerMark && field[7] == playerMark)
            {
                return true;
            }

            if (field[2] == playerMark && field[5] == playerMark && field[8] == playerMark)
            {
                return true;
            }

            if (field[0] == playerMark && field[4] == playerMark && field[8] == playerMark)
            {
                return true;
            }

            if (field[2] == playerMark && field[4] == playerMark && field[6] == playerMark)
            {
                return true;
            }

            return false;
        }
    }
}