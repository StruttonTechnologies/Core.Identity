namespace StruttonTechnologies.Core.Identity.Tests.Exceptions
{
    using StruttonTechnologies.Core.Identity.Exceptions;

    /// <summary>
    /// Contains test scenarios for <see cref="PasswordValidationException"/>.
    /// </summary>
    public class PasswordValidationExceptionTests
    {
        private static readonly string[] errors = new[] { "one", "two" };

        [Fact]
        public void Constructor_WithErrors_PopulatesErrors()
        {
            PasswordValidationException exception = new PasswordValidationException(errors);

            Assert.Equal(2, exception.Errors.Count);
        }

        [Fact]
        public void FromPassword_WithWeakPassword_ReturnsValidationErrors()
        {
            PasswordValidationException exception = PasswordValidationException.FromPassword("short");

            Assert.NotEmpty(exception.Errors);
        }
    }
}
