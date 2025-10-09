using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Entities.Concreate
{
    using Microsoft.AspNetCore.Identity;
    using System;

    namespace ST.Core.Identity.Domain.Entities.Claim
    {
        /// <summary>
        /// Base identity claim with provider metadata.
        /// </summary>
        public class IdentityClaimBase<TKey> : IdentityUserClaim<TKey>
            where TKey : IEquatable<TKey>
        {
            /// <summary>
            /// Indicates whether the claim is active.
            /// </summary>
            public bool IsActive { get; set; } = true;

            /// <summary>
            /// Timestamp of claim creation.
            /// </summary>
            public DateTime CreateDate { get; set; } = DateTime.UtcNow;

            /// <summary>
            /// Timestamp of last modification.
            /// </summary>
            public DateTime? ModifiedDate { get; set; }
        }
    }
}
