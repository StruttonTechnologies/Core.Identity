using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User
{
    /// <summary>
    /// Provides factory methods for mocking <see cref="UserManager{TestUser}"/> CreateAsync and CreateNoPasswordAsync behavior.
    /// </summary>
    public static class CreateAsyncMock
    {
        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when creating a user.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithSuccess()
        {
            var mock = MockUserManagerFactory.Create();

            mock.Setup(m => m.CreateAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(m => m.AddPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            return mock;
        }

        public static Mock<UserManager<TestUser>> WithFailureOnPassword(string? errorDescription = "Add password failed.")
        {
            var mock = MockUserManagerFactory.Create();

            mock.Setup(m => m.CreateAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(m => m.AddPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription! }));

            return mock;
        }



        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when creating a user.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithFailure(string? errorDescription = "Create user failed.")
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.CreateAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription! }));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for CreateAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithCustomResult(Func<TestUser, IdentityResult> resultFunc)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.CreateAsync(It.IsAny<TestUser>()))
                .ReturnsAsync((TestUser user) => resultFunc(user));
            return mock;
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always succeeds when creating a user without a password.
        /// </summary>
        public static Mock<UserManager<TestUser>> CreateNoPasswordWithSuccess()
        {
            return WithSuccess();
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that always fails when creating a user without a password.
        /// </summary>
        public static Mock<UserManager<TestUser>> CreateNoPasswordWithFailure(string? errorDescription = "Create user without password failed.")
        {
            return WithFailure(errorDescription);
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> with custom result for CreateNoPasswordAsync.
        /// </summary>
        public static Mock<UserManager<TestUser>> CreateNoPasswordWithCustomResult(Func<TestUser, IdentityResult> resultFunc)
        {
            return WithCustomResult(resultFunc);
        }

        /// <summary>
        /// Returns a mock <see cref="UserManager{TestUser}"/> that throws the specified exception when creating a user.
        /// </summary>
        public static Mock<UserManager<TestUser>> WithException(Exception exception)
        {
            var mock = MockUserManagerFactory.Create();
            mock.Setup(m => m.CreateAsync(It.IsAny<TestUser>()))
                .ThrowsAsync(exception);
            return mock;
        }
    }
}