using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MailingAppMVC.Startup))]
namespace MailingAppMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
