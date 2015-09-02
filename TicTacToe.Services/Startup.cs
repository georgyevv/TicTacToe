using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TicTacToe.Services.Startup))]

namespace TicTacToe.Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
