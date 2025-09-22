using ST.Core.Identity.Domain.Entities.User;

namespace ST.Core.Identity.Fakes.Models
{
    /// <summary>
    /// Represents a test application identity user for fakes and testing scenarios.
    /// Inherits from <see cref="IdentityUserBase{TestAppPerson}"/>.
    /// </summary>
    public class TestAppIdentityUser : IdentityUserBase<Guid,TestAppPerson>
    {
    }
}
