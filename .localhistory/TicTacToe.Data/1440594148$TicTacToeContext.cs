namespace TicTacToe.Data
{
    using System;
    using System.Data.Entity;
    using TicTacToe.Models;
    using System.Linq;


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TicTacToeContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Game> Games { get; set; }
    }

    public class TicTacToeContext : DbContext
    {
        public TicTacToeContext()
            : base("TicTacToeContext")
        {
        }

    }
}