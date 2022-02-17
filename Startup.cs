using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KRoberts_Theatre_Blog.Startup))]

namespace KRoberts_Theatre_Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}