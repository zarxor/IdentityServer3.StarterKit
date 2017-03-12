// <copyright file="UserService.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityServer3.AspNetIdentity;
using IdentityServer3.StarterKit.Managers;
using IdentityServer3.StarterKit.Models;

namespace IdentityServer3.StarterKit.Services
{
    public class UserService : AspNetIdentityUserService<User, string>
    {
        public UserService(UserManager userManager)
            : base(userManager)
        {
        }
    }
}