namespace StruttonTechnologies.Core.Identity.Tests.Exceptions
{
    using StruttonTechnologies.Core.Identity.Exceptions;

    /// <summary>
    /// Contains test scenarios for <see cref="UserValidationException"/>.
    /// </summary>
    public class UserValidationExceptionTests
    {
        private static readonly string[] errors = new[] { "error" };

        [Fact]
        public void ThrowIfInvalid_WithErrors_ThrowsUserValidationException()
        {
            Assert.Throws<UserValidationException>(() => UserValidationException.ThrowIfInvalid(errors));
        }

        [Fact]
        public void ThrowIfUserNameInvalid_WithReservedUserName_ThrowsUserValidationException()
        {
            Assert.Throws<UserValidationException>(() => UserValidationException.ThrowIfUserNameInvalid("admin"));
        }
    }
}
