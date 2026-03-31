using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Exceptions;

namespace StruttonTechnologies.Core.Identity.Tests.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UserNotFoundExceptionTests
    {
        [Fact]
        public void Constructor_Default_CreatesException()
        {
            UserNotFoundException exception = new UserNotFoundException();

            Assert.NotNull(exception);
            Assert.IsAssignableFrom<UserNotFoundException>(exception);
        }

        [Fact]
        public void Constructor_WithMessage_SetsMessage()
        {
            string message = "User not found";

            UserNotFoundException exception = new UserNotFoundException(message);

            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsProperties()
        {
            string message = "Test message";
            Exception innerException = new InvalidOperationException("Inner");

            UserNotFoundException exception = new UserNotFoundException(message, innerException);

            Assert.Equal(message, exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void Constructor_WithUserId_SetsFormattedMessage()
        {
            Guid userId = Guid.NewGuid();

            UserNotFoundException exception = new UserNotFoundException(userId);

            Assert.Contains(userId.ToString(), exception.Message);
            Assert.Contains("not found", exception.Message);
            Assert.Contains("store", exception.Message);
        }

        [Fact]
        public void Constructor_WithIntUserId_SetsFormattedMessage()
        {
            int userId = 12345;

            UserNotFoundException exception = new UserNotFoundException(userId);

            Assert.Contains(userId.ToString(), exception.Message);
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public void Constructor_WithStringUserId_SetsFormattedMessage()
        {
            string userId = "user-abc-123";

            UserNotFoundException exception = new UserNotFoundException(userId);

            Assert.Contains(userId, exception.Message);
        }

        [Fact]
        public void ThrowIfNull_ThrowsException_WhenUserIsNull()
        {
            TestUser? user = null;

            Assert.Throws<UserNotFoundException>(() =>
                UserNotFoundException.ThrowIfNull(user));
        }

        [Fact]
        public void ThrowIfNull_DoesNotThrow_WhenUserExists()
        {
            TestUser user = new TestUser { Id = Guid.NewGuid() };

            UserNotFoundException.ThrowIfNull(user);
        }

        [Fact]
        public void ThrowIfNull_WorksWithDifferentUserTypes()
        {
            CustomUser? user = null;

            Assert.Throws<UserNotFoundException>(() =>
                UserNotFoundException.ThrowIfNull(user));
        }

        private class TestUser
        {
            public Guid Id { get; set; }
        }

        private class CustomUser
        {
            public int Id { get; set; }
        }
    }
}
