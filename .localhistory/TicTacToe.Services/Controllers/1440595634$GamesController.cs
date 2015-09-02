using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TicTacToe.Services.Controllers
{
    [Authorize]
    public class GamesController : ApiController
    {
        [HttpPost]
        [ActionName("create")]
        public IHttpActionResult 
    }
}
