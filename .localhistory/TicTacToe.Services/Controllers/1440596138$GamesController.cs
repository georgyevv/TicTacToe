using System.Web.Http;
using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.Services.Controllers
{
    [Authorize]
    public class GamesController : ApiController
    {
        private ApplicationDbContext _data = new ApplicationDbContext();

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
