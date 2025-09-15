using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Entities
{
    public class IdentityUserBase<TPerson> : IdentityUser

        where TPerson : class
    {

        public string? ProviderName { get; set; } // e.g., Facebook, Google, Local

        public Guid PersonId { get; set; } 
        public virtual TPerson? Person { get; set; }

        public bool IsActive { get; set; } = true;
        
        [ConcurrencyCheck]
        public int RowVersion { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

    }
}
