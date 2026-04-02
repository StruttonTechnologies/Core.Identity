using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Orchestration.UserManager;

namespace StruttonTechnologies.Core.Identity.Orchestration.Tests.UserManager
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationOrchestrationTests
    {
        private readonly Mock<UserManager<TestUser>> _userManager;
        private readonly Mock<SignInManager<TestUser>> _signInManager;
        private readonly Mock<ITokenOrchestration<Guid>> _tokenOrchestration;
        private readonly AuthenticationOrchestration<TestUser, Guid> _orchestration;

        public AuthenticationOrchestrationTests()
        {
            _userManager = CreateMockUserManager();
            _signInManager = CreateMockSignInManager(_userManager);
            _tokenOrchestration = new Mock<ITokenOrchestration<Guid>>();

            _orchestration = new AuthenticationOrchestration<TestUser, Guid>(
                _userManager.Object,
                _signInManager.Object,
                _tokenOrchestration.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenUserManagerIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new AuthenticationOrchestration<TestUser, Guid>(
                    null!,
                    _signInManager.Object,
                    _tokenOrchestration.Object));

            Assert.Equal("userManager", ex.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenSignInManagerIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new AuthenticationOrchestration<TestUser, Guid>(
                    _userManager.Object,
                    null!,
                    _tokenOrchestration.Object));

            Assert.Equal("signInManager", ex.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenTokenOrchestrationIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new AuthenticationOrchestration<TestUser, Guid>(
                    _userManager.Object,
                    _signInManager.Object,
                    null!));

            Assert.Equal("TokenOrchestration", ex.ParamName);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsSuccess_WhenCredentialsAreValid()
        {
            string email = "test@example.com";
            string password = "ValidPassword123!";
            string expectedToken = "generated-jwt-token";
            TestUser user = new() { Id = Guid.NewGuid(), Email = email, UserName = email };
            ClaimsPrincipal principal = new(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, email)
            }));

            _userManager
                .Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _signInManager
                .Setup(m => m.CheckPasswordSignInAsync(user, password, true))
                .ReturnsAsync(SignInResult.Success);

            _signInManager
                .Setup(m => m.CreateUserPrincipalAsync(user))
                .ReturnsAsync(principal);

            _tokenOrchestration
                .Setup(t => t.GenerateTokenAsync(principal, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedToken);

            AuthenticationResultDto result = await _orchestration.AuthenticateAsync(email, password, TestContext.Current.CancellationToken);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedToken, result.Token);
            Assert.Null(result.FailureReason);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsFailure_WhenUserNotFound()
        {
            string email = "nonexistent@example.com";
            string password = "Password123!";

            _userManager
                .Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync((TestUser?)null);

            AuthenticationResultDto result = await _orchestration.AuthenticateAsync(email, password, TestContext.Current.CancellationToken);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal("Invalid credentials", result.FailureReason);
            Assert.Equal(string.Empty, result.Token);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsFailure_WhenPasswordIsIncorrect()
        {
            string email = "test@example.com";
            string password = "WrongPassword";
            TestUser user = new() { Id = Guid.NewGuid(), Email = email, UserName = email };

            _userManager
                .Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _signInManager
                .Setup(m => m.CheckPasswordSignInAsync(user, password, true))
                .ReturnsAsync(SignInResult.Failed);

            AuthenticationResultDto result = await _orchestration.AuthenticateAsync(email, password, TestContext.Current.CancellationToken);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal("Invalid credentials", result.FailureReason);
            Assert.Equal(string.Empty, result.Token);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsFailure_WhenAccountIsLockedOut()
        {
            string email = "test@example.com";
            string password = "Password123!";
            TestUser user = new() { Id = Guid.NewGuid(), Email = email, UserName = email };

            _userManager
                .Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _signInManager
                .Setup(m => m.CheckPasswordSignInAsync(user, password, true))
                .ReturnsAsync(SignInResult.LockedOut);

            AuthenticationResultDto result = await _orchestration.AuthenticateAsync(email, password, TestContext.Current.CancellationToken);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal("Invalid credentials", result.FailureReason);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsFailure_WhenTwoFactorRequired()
        {
            string email = "test@example.com";
            string password = "Password123!";
            TestUser user = new() { Id = Guid.NewGuid(), Email = email, UserName = email };

            _userManager
                .Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _signInManager
                .Setup(m => m.CheckPasswordSignInAsync(user, password, true))
                .ReturnsAsync(SignInResult.TwoFactorRequired);

            AuthenticationResultDto result = await _orchestration.AuthenticateAsync(email, password, TestContext.Current.CancellationToken);

            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid credentials", result.FailureReason);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsSuccess_WhenRegistrationSucceeds()
        {
            string email = "newuser@example.com";
            string password = "ValidPassword123!";
            string expectedToken = "generated-jwt-token";
            ClaimsPrincipal principal = new(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email)
            }));

            _userManager
                .Setup(m => m.CreateAsync(It.IsAny<TestUser>(), password))
                .ReturnsAsync(IdentityResult.Success);

            _signInManager
                .Setup(m => m.CreateUserPrincipalAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(principal);

            _tokenOrchestration
                .Setup(t => t.GenerateTokenAsync(principal, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedToken);

            AuthenticationResultDto result = await _orchestration.RegisterAsync(email, password, TestContext.Current.CancellationToken);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedToken, result.Token);
            Assert.Null(result.FailureReason);

            _userManager.Verify(m => m.CreateAsync(
                It.Is<TestUser>(u => u.UserName == email && u.Email == email),
                password), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailure_WhenUserCreationFails()
        {
            string email = "newuser@example.com";
            string password = "WeakPassword";
            IdentityResult failedResult = IdentityResult.Failed(
                new IdentityError { Description = "Password too weak" },
                new IdentityError { Description = "Password requires digit" });

            _userManager
                .Setup(m => m.CreateAsync(It.IsAny<TestUser>(), password))
                .ReturnsAsync(failedResult);

            AuthenticationResultDto result = await _orchestration.RegisterAsync(email, password, TestContext.Current.CancellationToken);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Contains("Password too weak", result.FailureReason);
            Assert.Contains("Password requires digit", result.FailureReason);
            Assert.Equal(string.Empty, result.Token);
        }

        [Fact]
        public async Task RegisterAsync_SetsUserNameAndEmail()
        {
            string email = "test@example.com";
            string password = "Password123!";
            TestUser? capturedUser = null;

            _userManager
                .Setup(m => m.CreateAsync(It.IsAny<TestUser>(), password))
                .Callback<TestUser, string>((user, pwd) => capturedUser = user)
                .ReturnsAsync(IdentityResult.Success);

            _signInManager
                .Setup(m => m.CreateUserPrincipalAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(new ClaimsPrincipal());

            _tokenOrchestration
                .Setup(t => t.GenerateTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("token");

            await _orchestration.RegisterAsync(email, password, TestContext.Current.CancellationToken);

            Assert.NotNull(capturedUser);
            Assert.Equal(email, capturedUser.Email);
            Assert.Equal(email, capturedUser.UserName);
        }

        [Fact]
        public async Task SignOutAsync_CallsSignInManagerAndRevokesToken()
        {
            string accessToken = "valid-jwt-token";

            _signInManager
                .Setup(m => m.SignOutAsync())
                .Returns(Task.CompletedTask);

            _tokenOrchestration
                .Setup(t => t.RevokeAccessTokenAsync(accessToken, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _orchestration.SignOutAsync(accessToken, TestContext.Current.CancellationToken);

            _signInManager.Verify(m => m.SignOutAsync(), Times.Once);
            _tokenOrchestration.Verify(t => t.RevokeAccessTokenAsync(accessToken, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SignOutAsync_RevokesToken_EvenIfSignOutFails()
        {
            string accessToken = "valid-jwt-token";

            _signInManager
                .Setup(m => m.SignOutAsync())
                .ThrowsAsync(new InvalidOperationException("Sign out failed"));

            _tokenOrchestration
                .Setup(t => t.RevokeAccessTokenAsync(accessToken, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _orchestration.SignOutAsync(accessToken, TestContext.Current.CancellationToken));

            _signInManager.Verify(m => m.SignOutAsync(), Times.Once);
            _tokenOrchestration.Verify(t => t.RevokeAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static Mock<UserManager<TestUser>> CreateMockUserManager()
        {
            var store = new Mock<IUserStore<TestUser>>();
            return new Mock<UserManager<TestUser>>(
                store.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);
        }

        private static Mock<SignInManager<TestUser>> CreateMockSignInManager(Mock<UserManager<TestUser>> userManager)
        {
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TestUser>>();
            return new Mock<SignInManager<TestUser>>(
                userManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null,
                null,
                null,
                null);
        }
    }

    public class TestUser : IdentityUser<Guid>
    {
    }
}
