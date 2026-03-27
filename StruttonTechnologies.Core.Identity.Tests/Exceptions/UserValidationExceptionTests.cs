using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Exceptions;

namespace StruttonTechnologies.Core.Identity.Tests.Exceptions
{
    /// <summary>
    /// Contains test scenarios for <see cref="UserValidationException"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserValidationExceptionTests
    {
        private static readonly string[] Errors = new[] { "error" };

        [Fact]
        public void ThrowIfInvalid_WithErrors_ThrowsUserValidationException()
        {
            Assert.Throws<UserValidationException>(() => UserValidationException.ThrowIfInvalid(Errors));
        }

        [Fact]
        public void ThrowIfUserNameInvalid_WithReservedUserName_ThrowsUserValidationException()
        {
            Assert.Throws<UserValidationException>(() => UserValidationException.ThrowIfUserNameInvalid("admin"));
        }
    }
}
