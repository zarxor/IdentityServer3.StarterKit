// <copyright file="RoleStore.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityServer3.StarterKit.Db;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityServer3.StarterKit.Stores
{
    public class RoleStore : RoleStore<IdentityRole>
    {
        public RoleStore(Context context)
            : base(context)
        {
        }
    }
}