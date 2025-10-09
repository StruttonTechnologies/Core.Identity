using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Stub.Data
{
    public static class StubRoleData
    {

        /// <summary>
        /// Defines the set of allowed stub roles for testing scenarios.
        /// </summary>
        public enum AllowedRole
        {
            Default,
            Admin,
            Contributor,
            Manager,
            Viewer
        }


        public static IdentityRole<Guid> Create(AllowedRole role = AllowedRole.Default)
        {
            return role switch
            {
                AllowedRole.Admin => new IdentityRole<Guid>("Admin") { Id = Guid.NewGuid(), NormalizedName = "ADMIN" },
                AllowedRole.Manager => new IdentityRole<Guid>("Manager") { Id = Guid.NewGuid(), NormalizedName = "MANAGER" },
                AllowedRole.Contributor => new IdentityRole<Guid>("Contributor") { Id = Guid.NewGuid(), NormalizedName = "CONTRIBUTOR" },
                AllowedRole.Viewer => new IdentityRole<Guid>("Viewer") { Id = Guid.NewGuid(), NormalizedName = "VIEWER" },
                _ => new IdentityRole<Guid>("Default") { Id = Guid.NewGuid(), NormalizedName = "DEFAULT" }
            };
        }
    }
}
