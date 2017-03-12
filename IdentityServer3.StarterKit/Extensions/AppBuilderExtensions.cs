// <copyright file="AppBuilderExtensions.cs">
//    2017 - Johan Boström
// </copyright>

using System.Security.Cryptography.X509Certificates;
using IdentityAdmin.Configuration;
using IdentityAdmin.Core;
using IdentityManager;
using IdentityManager.Configuration;
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

                factory.Register(new Core.Configuration.Registration<UserManager>());
                factory.Register(new Core.Configuration.Registration<UserStore>());
                factory.Register(
                    new Core.Configuration.Registration<Context>(resolver => new Context(Constants.ConnectionStringName)));

                factory.UserService = new Core.Configuration.Registration<IUserService, UserService>();

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

        public static IAppBuilder MapManager(this IAppBuilder app)
        {
            app.Map(Constants.Routes.IdMgr, mgrApp =>
            {
                var factory = new IdentityManagerServiceFactory();

                factory.Register(
                    new IdentityManager.Configuration.Registration<Context>(
                        resolver => new Context(Constants.ConnectionStringName)));
                factory.Register(new IdentityManager.Configuration.Registration<UserStore>());
                factory.Register(new IdentityManager.Configuration.Registration<RoleStore>());
                factory.Register(new IdentityManager.Configuration.Registration<UserManager>());
                factory.Register(new IdentityManager.Configuration.Registration<RoleManager>());
                factory.IdentityManagerService =
                    new IdentityManager.Configuration.Registration<IIdentityManagerService, IdentityManagerService>();

                mgrApp.UseIdentityManager(new IdentityManagerOptions
                {
                    Factory = factory,
                    SecurityConfiguration = new HostSecurityConfiguration
                    {
                        HostAuthenticationType = "Cookies",
                        AdditionalSignOutType = "oidc"
                    }
                });
            });

            return app;
        }

        public static IAppBuilder MapAdmin(this IAppBuilder app)
        {
            app.Map(Constants.Routes.IdAdm, admApp =>
            {
                var factory = new IdentityAdminServiceFactory
                {
                    IdentityAdminService =
                        new IdentityAdmin.Configuration.Registration<IIdentityAdminService>(
                            r => new IdentityAdminService(Constants.ConnectionStringName))
                };

                admApp.UseIdentityAdmin(new IdentityAdminOptions
                {
                    Factory = factory,
                    AdminSecurityConfiguration = new AdminHostSecurityConfiguration
                    {
                        HostAuthenticationType = "Cookies",
                        AdditionalSignOutType = "oidc"
                    }
                });
            });

            return app;
        }
    }
}