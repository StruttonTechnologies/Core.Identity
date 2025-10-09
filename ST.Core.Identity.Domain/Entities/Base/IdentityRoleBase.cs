using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ST.Core.Identity.Domain.Entities.Base
{
    /// <summary>
    /// Convenience alias for default Guid key type.
    /// </summary>
    public abstract class IdentityRoleBase : IdentityRoleBase<Guid>
    { }

    /// <summary>
    /// Base identity role with provider metadata.
    /// </summary>
    public class IdentityRoleBase<TKey> : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Indicates whether the role is active.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}