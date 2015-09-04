namespace TicTacToe.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using TicTacToe.Data.Migrations;
    using TicTacToe.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
#if DEBUG
        public ApplicationDbContext()
            : base("PCContext", throwIfV1Schema: false)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }
#else
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }
#endif

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Game> Games { get; set; }
    }
}