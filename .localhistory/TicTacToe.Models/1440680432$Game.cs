namespace TicTacToe.Models
{
    using System;

    public class Game
    {
        public Game()
        {
            this.Field = new string('0', 9);
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Field { get; set; }

        public DateTime CreatedTime { get; set; }

        public GameState State { get; set; }

        public string FirstPlayerId { get; set; }

        public virtual ApplicationUser FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }

        public virtual ApplicationUser SecondPlayer { get; set; }
    }
}