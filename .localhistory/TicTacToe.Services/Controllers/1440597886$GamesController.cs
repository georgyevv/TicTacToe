namespace TicTacToe.Services.Controllers
{
    using System.Web.Http;
    using TicTacToe.Data;
    using TicTacToe.Models;
    using TicTacToe.Services.Infrastructure;
    using System;
    using System.Linq;
    using TicTacToe.Services.Controllers.BindingModels;

    [Authorize]
    public class GamesController : ApiController
    {
        private ApplicationDbContext _data;
        private IUserIdProvider _userIdProvider;
        private string _currentUserId;

        public GamesController()
        {
            this._currentUserId = this._userIdProvider.GetUserId();
            this._data = new ApplicationDbContext();
            this._userIdProvider = new AspNetUserIdProvider();
        }

        [HttpPost]
        [ActionName("create")]
        public IHttpActionResult CreateGame()
        {
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
        [ActionName("join")]
        public IHttpActionResult JoinGame(JoinGameBindingModel model)
        {
            var game = this._data.Games.Where(g => g.Id == model.Id);
        }
    }
}
