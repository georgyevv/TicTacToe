namespace TicTacToe.Services.Hubs
{
    using Microsoft.AspNet.SignalR;
    
    public class GameHub : Hub
    {
        public void Update()
        {
            Clients.All.RefreshGameInfo();
        }

        public void PlayerDisconnect()
        {
            Clients.All.OtherPlayerLeft();
        }

        public void UpdateRooms()
        {
            Clients.All.UpdateRooms();
        }
    }
}