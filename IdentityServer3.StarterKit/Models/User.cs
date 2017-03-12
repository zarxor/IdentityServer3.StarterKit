// <copyright file="User.cs">
//    2017 - Johan Boström
// </copyright>

using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityServer3.StarterKit.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}