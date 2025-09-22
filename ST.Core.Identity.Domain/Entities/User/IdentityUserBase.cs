using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ST.Core.Identity.Domain.Entities.User
{



    /// <summary>
    /// Base identity user with provider metadata and person linkage.
    /// </summary>
    /// 



    public class IdentityUserBase<TKey, TPerson> : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TPerson : class
    {
        /// <summary>
        /// The name of the identity provider (e.g., Google, Facebook, Local).
        /// </summary>
        public string? ProviderName { get; set; }

        /// <summary>
        /// Foreign key to the associated person entity.
        /// </summary>
        public TKey PersonId { get; set; } = default!;

        /// <summary>
        /// Navigation property to the person entity.
        /// </summary>
        public virtual TPerson? Person { get; set; }

        /// <summary>
        /// Indicates whether the user is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Used for optimistic concurrency control.
        /// </summary>
        [ConcurrencyCheck]
        public int RowVersion { get; set; }

        /// <summary>
        /// Timestamp of user creation.
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp of last modification.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
