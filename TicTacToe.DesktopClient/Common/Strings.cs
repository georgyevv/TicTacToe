namespace TicTacToe.DesktopClient.Common
{
    public class Strings
    {
#if DEBUG
        public const string ServerUri = "http://localhost:61587/signalr";
#else
        public const string ServerUri = "http://3t.azurewebsites.net/signalr";
#endif
    }
}
