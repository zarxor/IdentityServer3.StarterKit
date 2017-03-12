// <copyright file="UserManager.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityServer3.StarterKit.Factories;
using IdentityServer3.StarterKit.Models;
using IdentityServer3.StarterKit.Stores;
using Microsoft.AspNet.Identity;

namespace IdentityServer3.StarterKit.Managers
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore store)
            : base(store)
        {
            ClaimsIdentityFactory = new ClaimsIdentityFactory();
        }
    }
}