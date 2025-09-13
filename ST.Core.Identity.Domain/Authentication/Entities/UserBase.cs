using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Entities
{
    public class IdentityUserBase<TPerson> : IdentityUser<Guid>
    {
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string? ProviderName { get; set; } // e.g., Facebook, Google, Local

        public Guid PersonId { get; set; }
        public virtual TPerson? Person { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true; 
    }
}
