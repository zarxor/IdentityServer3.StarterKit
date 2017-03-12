// <copyright file="IdentityManagerService.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityManager.AspNetIdentity;
using IdentityServer3.StarterKit.Managers;
using IdentityServer3.StarterKit.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityServer3.StarterKit.Services
{
    public class IdentityManagerService : AspNetIdentityManagerService<User, string, IdentityRole, string>
    {
        public IdentityManagerService(UserManager userMgr, RoleManager roleMgr)
            : base(userMgr, roleMgr)
        {
        }
    }
}