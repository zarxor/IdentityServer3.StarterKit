// <copyright file="RoleManager.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityServer3.StarterKit.Stores;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityServer3.StarterKit.Managers
{
    public class RoleManager : RoleManager<IdentityRole>
    {
        public RoleManager(RoleStore store)
            : base(store)
        {
        }
    }
}