﻿namespace TicTacToe.Services.Controllers.ViewModels
{
    public class JoinGameViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstPlayerName { get; set; }

        public string SecondPlayerName { get; set; }

        public string GameState { get; set; }

        public string Field { get; set; }
    }
}