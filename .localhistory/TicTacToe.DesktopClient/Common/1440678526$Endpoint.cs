namespace TicTacToe.DesktopClient.Common
{
    public class Endpoint
    {
        public const string Register = "http://localhost:61587/api/account/register";
        public const string Log = "http://localhost:61587/Token";
        public const string CreateGame = "http://localhost:61587/api/games/create";
        public const string JoinGame = "http://localhost:61587/api/games/join";
        public const string MakeMove = "http://localhost:61587/api/games/turn";
        public const string AvailableGames = "http://localhost:61587/api/games/available";
        public const string AllGames = "http://localhost:61587/api/games/all";
        public const string AllGames = "http://localhost:61587/api/games/id";
    }
}