namespace TicTacToe.Services.Controllers
{
    using System.Web.Http;
    using TicTacToe.Data;
    using TicTacToe.Models;
    using TicTacToe.Services.Infrastructure;
    using System;
    using System.Linq;
    using TicTacToe.Services.Controllers.BindingModels;
    using System.Data.Entity;
    using System.Threading.Tasks;

    [Authorize]
    public class GamesController : ApiController
    {
        private readonly ApplicationDbContext _data;
        private readonly IUserIdProvider _userIdProvider;
        private string _currentUserId;

        public GamesController()
        {
            this._data = new ApplicationDbContext();
            this._userIdProvider = new AspNetUserIdProvider();
        }

        [HttpPost]
        [Route("api/games/create")]
        public IHttpActionResult CreateGame()
        {
            this._currentUserId = this._userIdProvider.GetUserId();

            var newGame = new Game()
            {
                FirstPlayerId = this._currentUserId,
                CreatedTime = DateTime.Now,
                State = GameState.WaitingForSecondPlayer
            };

            this._data.Games.Add(newGame);
            this._data.SaveChanges();

            return this.Ok(newGame.Id);
        }

        [HttpPost]
        [Route("api/games/join")]
        public IHttpActionResult JoinGame(JoinGameBindingModel model)
        {
            if (model == null)
            {

            }

            this._currentUserId = this._userIdProvider.GetUserId();
            var game = this._data.Games.Find(model.Id);

            if (this._currentUserId == game.FirstPlayerId)
            {
                return this.BadRequest("You can't join your own game!");
            }

            game.SecondPlayerId = _currentUserId;
            game.State = GameState.FirstPlayerTurn;
            this._data.SaveChanges();

            return this.Ok(game.Id);
        }

        [HttpGet]
        [Route("api/games/available")]
        public async Task<IHttpActionResult> GetAvailableGames()
        {
            var availableGames = await this._data
                .Games
                .Where(g => g.State == GameState.WaitingForSecondPlayer)
                .ToListAsync();

            return this.Ok(availableGames);
        }
    }
}
