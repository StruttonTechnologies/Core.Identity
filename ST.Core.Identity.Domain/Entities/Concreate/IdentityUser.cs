using ST.Core.Identity.Domain.Entities.Base;
using ST.Core.Identity.Domain.Entities.User;

namespace ST.Core.Identity.Domain.Entities.Concreate
{
    /// <summary>
    /// Concrete identity user tied to <see cref="IdentityPerson"/> with <see cref="Guid"/> key.
    /// Ready-to-use without inheritance.
    /// </summary>
    public class IdentityUser : IdentityUserBase<Guid>
    {
        // Out-of-the-box user entity.
    }

    /// <summary>
    /// Concrete identity user with customizable key and person types.
    /// </summary>
    public class IdentityUser<TKey, TPerson> : IdentityUserBase<TKey>
        where TKey : IEquatable<TKey>
    {
        // Out-of-the-box user entity.
    }
}