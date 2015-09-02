using TicTacToe.Services.Controllers.ViewModels;

namespace TicTacToe.Services.Controllers
{
    using System.Web.Http;
    using TicTacToe.Data;
    using System.Text;
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
        private ApplicationDbContext _data;
        private IUserIdProvider _userIdProvider;
        private string _currentUserId;

        public GamesController()
        {
            this._data = new ApplicationDbContext();
            this._userIdProvider = new AspNetUserIdProvider();
        }

        [HttpPost]
        [Route("api/games/create")]
        public IHttpActionResult CreateGame(CreateGameBindingModel model)
        {
            this._currentUserId = this._userIdProvider.GetUserId();

            var newGame = new Game()
            {
                Name = model.Name,
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
                return this.BadRequest("You have to specify game id!");
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

            JoinGameViewModel gameViewModel = new JoinGameViewModel()
            {
                Id = game.Id,
                Name = game.Name,
                FirstPlayerName = game.FirstPlayer.UserName,
                SecondPlayerName = game.SecondPlayer.UserName
            };

            return this.Ok(gameViewModel);
        }

        [HttpPost]
        [Route("api/games/turn")]
        public IHttpActionResult Playturn(PlayTurnBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("You have to specify game id!");
            }

            this._currentUserId = this._userIdProvider.GetUserId();
            var game = this._data.Games.Find(model.Id);
            var gameState = game.State;

            if (game.State == GameState.FirstPlayerTurn && game.SecondPlayerId == _currentUserId ||
                game.State == GameState.SecondPlayerTurn && game.FirstPlayerId == _currentUserId)
            {
                return this.BadRequest("It's not your turn!");
            }

            var fieldLenght = (int)Math.Sqrt(game.Field.Length);
            var positionHit = model.X + (model.Y * fieldLenght);

            var field = new StringBuilder(game.Field);

            if (field[positionHit] != '0')
            {
                return this.BadRequest("You cannot place there!");
            }

            field[positionHit] = game.State == GameState.FirstPlayerTurn ? '1' : '2';
            game.Field = field.ToString();

            var winner = CheckForWinner(game.Field);

            if (winner == GameState.WinFirstPlayer)
            {
                game.State = GameState.WinFirstPlayer;
                this._data.SaveChanges();
                return this.Ok("First player win!");
            }
            if (winner == GameState.WinSecondPlayer)
            {
                game.State = GameState.WinSecondPlayer;
                this._data.SaveChanges();
                return this.Ok("Second player win!");
            }
            if (winner == GameState.Draw)
            {
                game.State = GameState.Draw;
                this._data.SaveChanges();
                return this.Ok("Draw!");
            }

            game.State = game.State == GameState.FirstPlayerTurn ? GameState.SecondPlayerTurn : GameState.FirstPlayerTurn;
            this._data.SaveChanges();

            return this.Ok();
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

        [HttpGet]
        [Route("api/games/all")]
        public async Task<IHttpActionResult> GetAllgames()
        {
            var allGames = await this._data
                .Games
                .ToListAsync();

            return this.Ok(allGames);
        }

        [HttpGet]
        [Route("api/games/{id}")]
        public async Task<IHttpActionResult> GetGameById(int id)
        {
            var searchedGame = await this._data
                .Games
                .Where(g => g.Id == id)
                .Select(g => new JoinGameViewModel()
            {
                Id = g.Id,
                Name = g.Name,
                FirstPlayerName = g.FirstPlayer.UserName,
                SecondPlayerName = g.SecondPlayer.UserName
            })

            return this.Ok()
        }

        private GameState CheckForWinner(string field)
        {
            if (field.All(e => e != '0'))
            {
                return GameState.Draw;
            }

            var firstPlayerIsWinner = Winner('1', field);
            var secondPlayerIsWinner = Winner('2', field);

            if (firstPlayerIsWinner)
            {
                return GameState.WinFirstPlayer;
            }

            if (secondPlayerIsWinner)
            {
                return GameState.WinSecondPlayer;
            }

            return GameState.NoWinner;
        }

        private bool Winner(char playerMark, string field)
        {
            if (field[0] == playerMark && field[1] == playerMark && field[2] == playerMark)
            {
                return true;
            }

            if (field[3] == playerMark && field[4] == playerMark && field[5] == playerMark)
            {
                return true;
            }

            if (field[6] == playerMark && field[7] == playerMark && field[8] == playerMark)
            {
                return true;
            }

            if (field[0] == playerMark && field[3] == playerMark && field[6] == playerMark)
            {
                return true;
            }

            if (field[1] == playerMark && field[4] == playerMark && field[7] == playerMark)
            {
                return true;
            }

            if (field[2] == playerMark && field[5] == playerMark && field[8] == playerMark)
            {
                return true;
            }

            if (field[0] == playerMark && field[4] == playerMark && field[8] == playerMark)
            {
                return true;
            }

            if (field[2] == playerMark && field[4] == playerMark && field[6] == playerMark)
            {
                return true;
            }

            return false;
        }
    }
}
