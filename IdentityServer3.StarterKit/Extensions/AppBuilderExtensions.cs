// <copyright file="AppBuilderExtensions.cs">
//    2017 - Johan Boström
// </copyright>

using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.EntityFramework;
using IdentityServer3.StarterKit.Config;
using IdentityServer3.StarterKit.Db;
using IdentityServer3.StarterKit.Managers;
using IdentityServer3.StarterKit.Services;
using IdentityServer3.StarterKit.Stores;
using Owin;

namespace IdentityServer3.StarterKit.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder MapCore(this IAppBuilder app, X509Certificate2 signingCertificate)
        {
            app.Map(Constants.Routes.Core, coreApp =>
            {
                var efConfig = new EntityFrameworkServiceOptions
                {
                    ConnectionString = Constants.ConnectionStringName
                };

                var factory = new IdentityServerServiceFactory();

                factory.RegisterConfigurationServices(efConfig);
                factory.RegisterOperationalServices(efConfig);
                factory.RegisterClientStore(efConfig);
                factory.RegisterScopeStore(efConfig);

                factory.Register(new Registration<UserManager>());
                factory.Register(new Registration<UserStore>());
                factory.Register(new Registration<Context>(resolver => new Context(Constants.ConnectionStringName)));

                factory.UserService = new Registration<IUserService, UserService>();

                DefaultSetup.Configure(efConfig);

                coreApp.UseIdentityServer(new IdentityServerOptions
                {
                    Factory = factory,
                    SigningCertificate = signingCertificate,
                    SiteName = "IdentityServer3 Starter Kit",
                    LoggingOptions = new LoggingOptions
                    {
                        EnableKatanaLogging = true
                    },
                    EventsOptions = new EventsOptions
                    {
                        RaiseFailureEvents = true,
                        RaiseInformationEvents = true,
                        RaiseSuccessEvents = true,
                        RaiseErrorEvents = true
                    }
                });
            });

            return app;
        }
    }
}