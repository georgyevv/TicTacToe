using System;
using TicTacToe.DesktopClient.Common;

namespace TicTacToe.DesktopClient.Game
{
    public class MultiPlayerGame : IGameData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstPlayerName { get; set; }
        public string SecondPlayerName { get; set; }
        public string GameState { get; set; }
        public string Field { get; set; }

        public GameState EnumGameState { get; set; }
    }
}