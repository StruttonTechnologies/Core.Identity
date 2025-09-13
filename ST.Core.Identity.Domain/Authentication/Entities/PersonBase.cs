using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Entities
{
    [ExcludeFromCodeCoverage]
    public class PersonBase<TPerson>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the first name of the person.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the person.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the person.
        /// </summary>
        public string ContactEmail { get; set; } = string.Empty;

        

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
