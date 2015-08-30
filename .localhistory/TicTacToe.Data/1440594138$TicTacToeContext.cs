namespace TicTacToe.Data
{
    using System;
    using System.Data.Entity;
    using TicTacToe.Models;
    using System.Linq;

    public class TicTacToeContext : DbContext
    {
        public TicTacToeContext()
            : base("TicTacToeContext")
        {
        }

        public virtual DbSet<Game> Games { get; set; }
    }
}