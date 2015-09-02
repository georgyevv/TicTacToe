using System.Web.Http;
using TicTacToe.Data;

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
            var 
        }
    }
}
