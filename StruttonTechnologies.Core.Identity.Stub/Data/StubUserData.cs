using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Stub.Data
{
    /// <summary>
    /// Provides canned <see cref="StubUser"/> instances for testing scenarios.
    /// Mirrors the domain's IdentityUser shape.
    /// </summary>
    public static class StubUserData
    {
        /// <summary>
        /// Default stub user with predictable values.
        /// </summary>
        public static readonly StubUser Default = new StubUser
        {
            Id = Guid.NewGuid(),
            UserName = "stub.user",
            Email = "stub.user@example.com",
        };

        /// <summary>
        /// Admin stub user with elevated role semantics.
        /// </summary>
        public static readonly StubUser Admin = new StubUser
        {
            Id = Guid.NewGuid(),
            UserName = "stub.admin",
            Email = "stub.admin@example.com",
        };

        /// <summary>
        /// Factory method for creating ad-hoc stub users.
        /// </summary>
        public static StubUser Create(Guid id, string userName, string email)
        {
            return new StubUser
            {
                Id = id,
                UserName = userName,
                Email = email,
            };
        }
    }
}
