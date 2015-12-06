using Microsoft.Owin;
using Owin;
using SmartQueue.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace SmartQueue.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
