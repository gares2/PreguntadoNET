using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Preguntado.Startup))]
namespace Preguntado
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
