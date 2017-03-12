// <copyright file="UserStore.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityServer3.StarterKit.Db;
using IdentityServer3.StarterKit.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityServer3.StarterKit.Stores
{
    public class UserStore :
        UserStore<User, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UserStore(Context context)
            : base(context)
        {
        }
    }
}