// <copyright file="Startup.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityServer3.StarterKit;
using IdentityServer3.StarterKit.Config;
using IdentityServer3.StarterKit.Extensions;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace IdentityServer3.StarterKit
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var certificate = Certificate.Get();
            app.MapCore(certificate);
        }
    }
}