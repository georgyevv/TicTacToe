namespace TicTacToe.Models
{
    public enum GameState
    {
        WaitingForSecondPlayer,
        FirstPlayerTurn,
        SecondPlayerTurn,
        WinFirstPlayer,
        WinSecondPlayer,
        NoWinner,
        Draw
    }
}
