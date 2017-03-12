// <copyright file="IdentityAdminService.cs">
//    2017 - Johan Boström
// </copyright>

using IdentityServer3.Admin.EntityFramework;
using IdentityServer3.Admin.EntityFramework.Entities;

namespace IdentityServer3.StarterKit.Services
{
    public class IdentityAdminService : IdentityAdminCoreManager<IdentityClient, int, IdentityScope, int>
    {
        public IdentityAdminService(string connectionString)
            : base(connectionString)
        {
        }
    }
}