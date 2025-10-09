using ST.Core.Identity.Domain.Entities.Base;
using System;

namespace ST.Core.Identity.Domain.Entities.Concreate
{
    /// <summary>
    /// Concrete identity role with Guid key and tied to <see cref="IdentityPerson"/>.
    /// </summary>
    public class IdentityRole : IdentityRoleBase<Guid>
    {
        // Out-of-the-box role entity.
    }

    /// <summary>
    /// Concrete identity role with customizable key and tied to <see cref="IdentityPerson{TKey}"/>.
    /// </summary>
    public class IdentityRole<TKey> : IdentityRoleBase<TKey>
        where TKey : IEquatable<TKey>
    {
        // Out-of-the-box role entity.
    }

    
}