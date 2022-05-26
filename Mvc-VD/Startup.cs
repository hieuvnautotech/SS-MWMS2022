using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Mvc_VD.Startup))]
namespace Mvc_VD
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Find and register SignalR hubs
            app.MapSignalR();

        }
        
    }
}