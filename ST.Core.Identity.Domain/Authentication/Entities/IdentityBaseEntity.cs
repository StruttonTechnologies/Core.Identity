using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Entities
{
    public abstract class IdentityEntityBase<TPerson>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ConcurrencyCheck]
        public int RowVersion { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }

        public virtual TPerson? CreatedBy { get; set; }
        public virtual TPerson? ModifiedBy { get; set; }
    }


}
