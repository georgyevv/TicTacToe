namespace TicTacToe.Services.Controllers
{
    using System.Web.Http;
    using TicTacToe.Data;
    using TicTacToe.Models;
    using TicTacToe.Services.Infrastructure;

    [Authorize]
    public class GamesController : ApiController
    {
        private ApplicationDbContext _data;
        private IUserIdProvider _userIdProvider;

        public GamesController()
        {
            this._data = new ApplicationDbContext();
            this._userIdProvider = new AspNetUserIdProvider();
        }

        [HttpPost]
        [ActionName("create")]
        public IHttpActionResult CreateGame()
        {
            var currentUserId = this._userIdProvider.GetUserId();

            var newGame = new Game()
            {
                FirstPlayer = currentUserId
            };
        }
    }
}
