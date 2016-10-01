using Microsoft.Owin;
using Owin;
using System.Globalization;
using System.Threading;

[assembly: OwinStartupAttribute(typeof(BooksCatalogeMVC.Startup))]
namespace BooksCatalogeMVC
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}
