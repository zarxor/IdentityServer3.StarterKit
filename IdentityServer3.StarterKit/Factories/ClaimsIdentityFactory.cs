// <copyright file="ClaimsIdentityFactory.cs">
//    2017 - Johan Boström
// </copyright>

using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.StarterKit.Models;
using Microsoft.AspNet.Identity;

namespace IdentityServer3.StarterKit.Factories
{
    public class ClaimsIdentityFactory : ClaimsIdentityFactory<User, string>
    {
        public ClaimsIdentityFactory()
        {
            UserIdClaimType = Core.Constants.ClaimTypes.Subject;
            UserNameClaimType = Core.Constants.ClaimTypes.PreferredUserName;
            RoleClaimType = Core.Constants.ClaimTypes.Role;
        }

        public override async Task<ClaimsIdentity> CreateAsync(UserManager<User, string> manager, User user,
            string authenticationType)
        {
            var ci = await base.CreateAsync(manager, user, authenticationType);

            if (!string.IsNullOrWhiteSpace(user.FirstName))
                ci.AddClaim(new Claim("given_name", user.FirstName));

            if (!string.IsNullOrWhiteSpace(user.LastName))
                ci.AddClaim(new Claim("family_name", user.LastName));

            return ci;
        }
    }
}