namespace TicTacToe.Data
{
    using System;
    using System.Data.Entity;
    using TicTacToe.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TicTacToe.Data.Migrations;
    using System.Linq;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TicTacToeContext", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Game> Games { get; set; }
    }
}