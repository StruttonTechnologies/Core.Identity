using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Entities
{
    public class UserBase : IdentityUser<Guid>
    {
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string? ProviderName { get; set; } // e.g., Facebook, Google, Local
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true; 
    }
}
