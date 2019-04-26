using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TSLR_DeliveryMessagingSystem.Startup))]
namespace TSLR_DeliveryMessagingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
