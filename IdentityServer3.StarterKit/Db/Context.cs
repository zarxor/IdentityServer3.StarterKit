// <copyright file="Context.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityServer3.StarterKit.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityServer3.StarterKit.Db
{
    public class Context :
        IdentityDbContext<User, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public Context(string connString)
            : base(connString)
        {
        }
    }
}