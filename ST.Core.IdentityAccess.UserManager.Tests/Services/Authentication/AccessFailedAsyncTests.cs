using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.Stores;
using ST.Core.IdentityAccess.Fakes.UserManager;


namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Contains unit tests for the <see cref="AuthenticationUserService{TUser}.AccessFailedAsync"/> method.
    /// </summary>
    public class AccessFailedAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that <see cref="AuthenticationUserService{TUser}.AccessFailedAsync"/> increments the failure count for a valid user.
        /// </summary>
        [Fact]
        public async Task AccessFailedAsync_LockoutDisabled_StillIncrementsFailureCount()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            user.LockoutEnabled = false;

            var createResult = await UserManager.CreateAsync(user);
            Assert.True(createResult.Succeeded);

            var storedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.NotNull(storedUser);

            var result = await Service.AccessFailedAsync(user);
            Assert.True(result.Succeeded);

            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.Equal(1, updatedUser!.AccessFailedCount); 
        }



        /// <summary>
        /// Verifies that <see cref="AuthenticationUserService{TUser}.AccessFailedAsync"/> throws <see cref="ArgumentNullException"/> when the user is null.
        /// </summary>
        [Fact]
        public async Task AccessFailedAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.AccessFailedAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that <see cref="AuthenticationUserService{TUser}.AccessFailedAsync"/> returns a failed <see cref="IdentityResult"/> when an exception occurs.
        /// </summary>
        [Fact]
        public async Task AccessFailedAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault(); 
            var result = await Service.AccessFailedAsync(user);


            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }

        [Fact]
        public async Task Store_CanCreateAndFindUserById()
        {
            var store = new InMemoryUserStore();
            var user = TestAppUserIdentityFactory.Create("shawn");

            await store.CreateAsync(user, CancellationToken.None);
            var retrieved = await store.FindByIdAsync(user.Id, CancellationToken.None);

            Assert.NotNull(retrieved);
            Assert.Equal(user.UserName, retrieved.UserName);
        }


    }
}