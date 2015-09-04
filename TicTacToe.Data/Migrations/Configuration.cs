namespace TicTacToe.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TicTacToe.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
            ContextKey = "TicTacToe.Data.ApplicationDbContext";
        }
    }
}
