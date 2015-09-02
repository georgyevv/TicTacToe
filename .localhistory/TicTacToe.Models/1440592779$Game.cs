namespace TicTacToe.Models
{
    public class Game
    {
        public Game()
        {
            this.Field = new string('0', 9);
        }

        public int Id { get; set; }

        public string Field { get; set; }

        public string FirstPlayerId { get; set; }

        public ApplicationUser FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }

        public ApplicationUser SecondPlayer { get; set; }


    }
}