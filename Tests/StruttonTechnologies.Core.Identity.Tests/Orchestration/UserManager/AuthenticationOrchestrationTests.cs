using Microsoft.AspNetCore.Http;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Orchestration.UserManager;

namespace StruttonTechnologies.Core.Identity.Tests.Orchestration.UserManager;

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
        ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            new AuthenticationOrchestration<TestUser, Guid>(
                null!,
                _signInManager.Object,
                _tokenOrchestration.Object));

        Assert.Equal("userManager", ex.ParamName);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenSignInManagerIsNull()
    {
        ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            new AuthenticationOrchestration<TestUser, Guid>(
                _userManager.Object,
                null!,
                _tokenOrchestration.Object));

        Assert.Equal("signInManager", ex.ParamName);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenTokenOrchestrationIsNull()
    {
        ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            new AuthenticationOrchestration<TestUser, Guid>(
                _userManager.Object,
                _signInManager.Object,
                null!));

        Assert.Equal("tokenOrchestration", ex.ParamName);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsSuccess_WhenCredentialsAreValid()
    {
        string email = "test@example.com";
        string password = "ValidPassword123!";
        string expectedAccessToken = "generated-access-token";
        string expectedRefreshToken = "generated-refresh-token";

        TestUser user = CreateUser(email);

        ClaimsPrincipal principal = CreatePrincipal(user.Id, email);

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
            .ReturnsAsync(expectedAccessToken);

        _tokenOrchestration
            .Setup(t => t.GenerateRefreshTokenAsync(user.Id, email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRefreshToken);

        AuthenticationResultDto result = await _orchestration.AuthenticateAsync(
            email,
            password,
            TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedAccessToken, result.AccessToken);
        Assert.Equal(expectedRefreshToken, result.RefreshToken);
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

        AuthenticationResultDto result = await _orchestration.AuthenticateAsync(
            email,
            password,
            TestContext.Current.CancellationToken);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid credentials", result.FailureReason);
        Assert.Equal(string.Empty, result.AccessToken);
        Assert.Equal(string.Empty, result.RefreshToken);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsFailure_WhenPasswordIsIncorrect()
    {
        string email = "test@example.com";
        string password = "WrongPassword";
        TestUser user = CreateUser(email);

        _userManager
            .Setup(m => m.FindByEmailAsync(email))
            .ReturnsAsync(user);

        _signInManager
            .Setup(m => m.CheckPasswordSignInAsync(user, password, true))
            .ReturnsAsync(SignInResult.Failed);

        AuthenticationResultDto result = await _orchestration.AuthenticateAsync(
            email,
            password,
            TestContext.Current.CancellationToken);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid credentials", result.FailureReason);
        Assert.Equal(string.Empty, result.AccessToken);
        Assert.Equal(string.Empty, result.RefreshToken);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsFailure_WhenAccountIsLockedOut()
    {
        string email = "test@example.com";
        string password = "Password123!";
        TestUser user = CreateUser(email);

        _userManager
            .Setup(m => m.FindByEmailAsync(email))
            .ReturnsAsync(user);

        _signInManager
            .Setup(m => m.CheckPasswordSignInAsync(user, password, true))
            .ReturnsAsync(SignInResult.LockedOut);

        AuthenticationResultDto result = await _orchestration.AuthenticateAsync(
            email,
            password,
            TestContext.Current.CancellationToken);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid credentials", result.FailureReason);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsFailure_WhenTwoFactorRequired()
    {
        string email = "test@example.com";
        string password = "Password123!";
        TestUser user = CreateUser(email);

        _userManager
            .Setup(m => m.FindByEmailAsync(email))
            .ReturnsAsync(user);

        _signInManager
            .Setup(m => m.CheckPasswordSignInAsync(user, password, true))
            .ReturnsAsync(SignInResult.TwoFactorRequired);

        AuthenticationResultDto result = await _orchestration.AuthenticateAsync(
            email,
            password,
            TestContext.Current.CancellationToken);

        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid credentials", result.FailureReason);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsSuccess_WhenRegistrationSucceeds()
    {
        string email = "newuser@example.com";
        string password = "ValidPassword123!";
        string expectedAccessToken = "generated-access-token";
        string expectedRefreshToken = "generated-refresh-token";

        ClaimsPrincipal principal = new(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, email)
        ]));

        _userManager
            .Setup(m => m.CreateAsync(It.IsAny<TestUser>(), password))
            .ReturnsAsync(IdentityResult.Success);

        _signInManager
            .Setup(m => m.CreateUserPrincipalAsync(It.IsAny<TestUser>()))
            .ReturnsAsync(principal);

        _tokenOrchestration
            .Setup(t => t.GenerateTokenAsync(principal, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAccessToken);

        _tokenOrchestration
            .Setup(t => t.GenerateRefreshTokenAsync(It.IsAny<Guid>(), email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRefreshToken);

        AuthenticationResultDto result = await _orchestration.RegisterAsync(
            email,
            password,
            TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedAccessToken, result.AccessToken);
        Assert.Equal(expectedRefreshToken, result.RefreshToken);
        Assert.Null(result.FailureReason);

        _userManager.Verify(
            m => m.CreateAsync(
                It.Is<TestUser>(u => u.UserName == email && u.Email == email),
                password),
            Times.Once);
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

        AuthenticationResultDto result = await _orchestration.RegisterAsync(
            email,
            password,
            TestContext.Current.CancellationToken);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Contains("Password too weak", result.FailureReason, StringComparison.Ordinal);
        Assert.Contains("Password requires digit", result.FailureReason, StringComparison.Ordinal);
        Assert.Equal(string.Empty, result.AccessToken);
        Assert.Equal(string.Empty, result.RefreshToken);
    }

    [Fact]
    public async Task RegisterAsync_SetsUserNameAndEmail()
    {
        string email = "test@example.com";
        string password = "Password123!";
        TestUser? capturedUser = null;

        _userManager
            .Setup(m => m.CreateAsync(It.IsAny<TestUser>(), password))
            .Callback<TestUser, string>((user, _) => capturedUser = user)
            .ReturnsAsync(IdentityResult.Success);

        _signInManager
            .Setup(m => m.CreateUserPrincipalAsync(It.IsAny<TestUser>()))
            .ReturnsAsync(new ClaimsPrincipal());

        _tokenOrchestration
            .Setup(t => t.GenerateTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("access-token");

        _tokenOrchestration
            .Setup(t => t.GenerateRefreshTokenAsync(It.IsAny<Guid>(), email, It.IsAny<CancellationToken>()))
            .ReturnsAsync("refresh-token");

        await _orchestration.RegisterAsync(
            email,
            password,
            TestContext.Current.CancellationToken);

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

        await _orchestration.SignOutAsync(
            accessToken,
            TestContext.Current.CancellationToken);

        _signInManager.Verify(m => m.SignOutAsync(), Times.Once);

        _tokenOrchestration.Verify(
            t => t.RevokeAccessTokenAsync(accessToken, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SignOutAsync_DoesNotRevokeToken_WhenSignOutFails()
    {
        string accessToken = "valid-jwt-token";

        _signInManager
            .Setup(m => m.SignOutAsync())
            .ThrowsAsync(new InvalidOperationException("Sign out failed"));

        _tokenOrchestration
            .Setup(t => t.RevokeAccessTokenAsync(accessToken, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _orchestration.SignOutAsync(
                accessToken,
                TestContext.Current.CancellationToken));

        _signInManager.Verify(m => m.SignOutAsync(), Times.Once);

        _tokenOrchestration.Verify(
            t => t.RevokeAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static TestUser CreateUser(string email)
    {
        return new TestUser
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = email
        };
    }

    private static ClaimsPrincipal CreatePrincipal(Guid userId, string email)
    {
        ClaimsIdentity identity = new(
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, email)
        ]);

        return new ClaimsPrincipal(identity);
    }

    private static Mock<UserManager<TestUser>> CreateMockUserManager()
    {
        Mock<IUserStore<TestUser>> store = new();

        return new Mock<UserManager<TestUser>>(
            store.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);
    }

    private static Mock<SignInManager<TestUser>> CreateMockSignInManager(
        Mock<UserManager<TestUser>> userManager)
    {
        Mock<IHttpContextAccessor> contextAccessor = new();
        Mock<IUserClaimsPrincipalFactory<TestUser>> claimsFactory = new();

        return new Mock<SignInManager<TestUser>>(
            userManager.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            null!,
            null!,
            null!,
            null!);
    }
}

public sealed class TestUser : IdentityUser<Guid>
{
}
