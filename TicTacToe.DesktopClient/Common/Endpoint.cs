namespace TicTacToe.DesktopClient.Common
{
    public class Endpoint
    {
#if DEBUG
        public const string Register = "http://localhost:61587//api/account/register";
        public const string Log = "http://localhost:61587//Token";
        public const string CreateGame = "http://localhost:61587//api/games/create";
        public const string JoinGame = "http://localhost:61587//api/games/join";
        public const string MakeMove = "http://localhost:61587//api/games/turn";
        public const string AvailableGames = "http://localhost:61587//api/games/available";
        public const string AllGames = "http://localhost:61587//api/games/all";
        public const string GameById = "http://localhost:61587//api/games?id=";
        public const string GameByUserName = "http://localhost:61587//api/games/user?username=";
        public const string ChangeGameState = "http://localhost:61587//api/games/state?id=";
#else
        public const string Register =          "http://3t.azurewebsites.net//api/account/register";
        public const string Log =               "http://3t.azurewebsites.net//Token";
        public const string CreateGame =        "http://3t.azurewebsites.net//api/games/create";
        public const string JoinGame =          "http://3t.azurewebsites.net//api/games/join";
        public const string MakeMove =          "http://3t.azurewebsites.net//api/games/turn";
        public const string AvailableGames =    "http://3t.azurewebsites.net//api/games/available";
        public const string AllGames =          "http://3t.azurewebsites.net//api/games/all";
        public const string GameById =          "http://3t.azurewebsites.net//api/games?id=";
        public const string GameByUserName =    "http://3t.azurewebsites.net//api/games/user?username=";
        public const string ChangeGameState =   "http://3t.azurewebsites.net//api/games/state?id=";
#endif
    }
}