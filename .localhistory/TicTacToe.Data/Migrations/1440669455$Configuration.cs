namespace TicTacToe.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TicTacToe.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = false;
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicTacToe.Data.ApplicationDbContext";
        }

        protected override void Seed(TicTacToe.Data.ApplicationDbContext context)
        {
        }
    }
}
