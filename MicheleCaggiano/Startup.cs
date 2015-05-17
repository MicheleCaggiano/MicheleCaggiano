using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MicheleCaggiano.Startup))]
namespace MicheleCaggiano
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
