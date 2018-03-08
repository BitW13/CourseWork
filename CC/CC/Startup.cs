using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CC.Startup))]
namespace CC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
