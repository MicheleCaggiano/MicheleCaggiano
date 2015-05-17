using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MicheleCaggiano.Web.Startup))]
namespace MicheleCaggiano.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
