using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineShop.WebUI.Startup))]
namespace OnlineShop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
