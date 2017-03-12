// <copyright file="DefaultSetup.cs">
//    2017 - Johan Boström
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework;
using IdentityServer3.StarterKit.Db;
using IdentityServer3.StarterKit.Managers;
using IdentityServer3.StarterKit.Models;
using IdentityServer3.StarterKit.Stores;
using Microsoft.AspNet.Identity;

namespace IdentityServer3.StarterKit.Config
{
    public class DefaultSetup
    {
        public static void Configure(EntityFrameworkServiceOptions options)
        {
            using (var db = new ScopeConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Scopes.Any())
                {
                    foreach (var s in StandardScopes.All)
                    {
                        var e = s.ToEntity();
                        db.Scopes.Add(e);
                    }

                    db.Scopes.Add(StandardScopes.Roles.ToEntity());
                    db.Scopes.Add(StandardScopes.OfflineAccess.ToEntity());

                    db.SaveChanges();
                }
            }

            using (var db = new Context(options.ConnectionString))
            {
                if (!db.Users.Any())
                {
                    using (var userManager = new UserManager(new UserStore(db)))
                    {
                        var defaultUserPassword = "skywalker"; // Must be atleast 6 characters
                        var user = new User
                        {
                            UserName = "administrator",
                            FirstName = "Luke",
                            LastName = "Skywalker",
                            Email = "luke.skywalker@email.com",
                            EmailConfirmed = true
                        };
                        userManager.Create(user, defaultUserPassword);
                        userManager.AddClaim(user.Id,
                            new Claim(Core.Constants.ClaimTypes.WebSite, "https://www.johanbostrom.se/"));
                        userManager.AddClaim(user.Id,
                            new Claim(Core.Constants.ClaimTypes.Role, IdentityManager.Constants.AdminRoleName));
                        userManager.AddClaim(user.Id,
                            new Claim(Core.Constants.ClaimTypes.Role, IdentityAdmin.Constants.AdminRoleName));
                    }

                    db.SaveChanges();
                }
            }

            using (var db = new ClientConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Clients.Any())
                {
                    var defaultImplicitClient = new Client
                    {
                        ClientName = "Default Implicit Flow Client",
                        ClientId = "default.implicit",
                        Flow = Flows.Implicit,
                        Enabled = true,
                        AllowedScopes = new List<string>
                        {
                            Core.Constants.StandardScopes.OpenId,
                            Core.Constants.StandardScopes.Profile,
                            Core.Constants.StandardScopes.Email,
                            Core.Constants.StandardScopes.Roles,
                            Core.Constants.StandardScopes.Address,
                            Core.Constants.StandardScopes.Phone,
                            Core.Constants.StandardScopes.OfflineAccess,
                            IdentityManager.Constants.IdMgrScope
                        },
                        ClientUri = "https://localhost:44300/",
                        RequireConsent = false,
                        RedirectUris = new List<string>
                        {
                            "https://localhost:44300"
                        },
                        PostLogoutRedirectUris = new List<string>
                        {
                            "https://localhost:44300" + Constants.Routes.IdMgr,
                            "https://localhost:44300" + Constants.Routes.IdAdm
                        },
                        LogoutSessionRequired = true
                    };
                    
                    db.Clients.Add(defaultImplicitClient.ToEntity());

                    db.SaveChanges();
                }
            }
        }
    }
}