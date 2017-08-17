using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cMcglynnShoppingApp.Startup))]
namespace cMcglynnShoppingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
