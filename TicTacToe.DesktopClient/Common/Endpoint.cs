namespace TicTacToe.DesktopClient.Common
{
    public class Endpoint
    {
        public const string Register =          "http://tictactoe-18.apphb.com//api/account/register";
        public const string Log =               "http://tictactoe-18.apphb.com//Token";
        public const string CreateGame =        "http://tictactoe-18.apphb.com//api/games/create";
        public const string JoinGame =          "http://tictactoe-18.apphb.com//api/games/join";
        public const string MakeMove =          "http://tictactoe-18.apphb.com//api/games/turn";
        public const string AvailableGames =    "http://tictactoe-18.apphb.com//api/games/available";
        public const string AllGames =          "http://tictactoe-18.apphb.com//api/games/all";
        public const string GameById =          "http://tictactoe-18.apphb.com//api/games?id=";
        public const string GameByUserName =    "http://tictactoe-18.apphb.com//api/games/user?username=";
        public const string ChangeGameState =   "http://tictactoe-18.apphb.com//api/games/state?id=";
    }
}