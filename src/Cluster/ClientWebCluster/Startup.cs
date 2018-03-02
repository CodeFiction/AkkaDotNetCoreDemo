using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientWebCluster;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace ClientWebCluster
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            // ConfigureAuth(app);
        }
    }
}