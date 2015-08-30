namespace TicTacToe.Services.Controllers
{
    using System.Web.Http;
    using TicTacToe.Data;
    using TicTacToe.Models;
    using TicTacToe.Services.Infrastructure;

    [Authorize]
    public class GamesController : ApiController
    {
        private ApplicationDbContext _data = new ApplicationDbContext();
        private IUserIdProvider userIdProvider;

        [HttpPost]
        [ActionName("create")]
        public IHttpActionResult CreateGame()
        {
            var currentUser = this.User.get

            var newGame = new Game()
            {
                FirstPlayer = 
            };
        }
    }
}
