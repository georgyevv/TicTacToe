using TicTacToe.DesktopClient.Common;

namespace TicTacToe.DesktopClient.Game
{
    public interface IGameData
    {
        int Id { get; set; }

        string Name { get; set; }

        string FirstPlayerName { get; set; }

        string SecondPlayerName { get; set; }

        string GameState { get; set; }

        string Field { get; set; }

        GameState EnumGameState { get; set; }
    }
}