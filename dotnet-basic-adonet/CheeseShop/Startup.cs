using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CheeseShop.Startup))]
namespace CheeseShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
