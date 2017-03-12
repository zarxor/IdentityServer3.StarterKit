// <copyright file="Startup.cs">
//    2017 - Johan Boström
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityManager.Extensions;
using IdentityServer3.Core.Logging;
using IdentityServer3.StarterKit;
using IdentityServer3.StarterKit.Config;
using IdentityServer3.StarterKit.Extensions;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace IdentityServer3.StarterKit
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LogProvider.SetCurrentLogProvider(new TraceLogProvider());

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                Authority = $"https://localhost:44300{Constants.Routes.Core}",
                ClientId = "default.implicit",
                RedirectUri = "https://localhost:44300",
                ResponseType = "id_token",
                UseTokenLifetime = false,
                Scope = $"openid roles profile",
                SignInAsAuthenticationType = "Cookies",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = async n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var result = await n.OwinContext.Authentication.AuthenticateAsync("Cookies");
                            var idToken = result?.Identity.Claims.GetValue("id_token");
                            if (idToken != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idToken;
                                n.ProtocolMessage.PostLogoutRedirectUri =
                                    $"https://localhost:44300{Constants.Routes.IdMgr}";
                            }
                        }
                    }
                }
            });


            var certificate = Certificate.Get();
            app.MapCore(certificate)
                .MapManager();
        }

        public class TraceLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (logLevel, messageFunc, exception, formatParameters) =>
                {
                    if (messageFunc != null)
                    {
                        var message = messageFunc();
                        try
                        {
                            var formatedMessage = string.Format(message, formatParameters);
                            Trace.WriteLine(formatedMessage);
                        }
                        catch
                        {
                            Trace.WriteLine(message);
                        }
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                return new Disposable();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                return new Disposable();
            }

            public class Disposable : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }
    }
}