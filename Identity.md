# StruttonTechnologies.Core.Identity

A comprehensive identity and authentication framework built on ASP.NET Core Identity with CQRS architecture, JWT token management, and Entity Framework Core integration.

---

## Overview

This solution provides a structured identity system that enforces clean separation between presentation, coordination, orchestration, and data access layers. Instead of mixing authentication logic directly into controllers or services, identity operations are handled through defined contracts, MediatR commands/queries, and coordinated execution.

### Architecture Layers

- **API**: Controllers and HTTP request handling
- **Coordinator**: MediatR command/query handlers (CQRS)
- **Orchestration**: Business logic and UserManager/SignInManager operations
- **Domain**: Entities and domain models
- **Data**: Entity Framework Core repositories and DbContext
- **Contracts**: Interface definitions and DTOs

---

## NuGet Packages

### Core Packages

#### StruttonTechnologies.Core.Identity.Coordinator
**Package**: `StruttonTechnologies.Core.Identity.Coordinator`  
**Purpose**: MediatR command and query orchestration layer  
**Use When**: Implementing CQRS pattern for identity operations

**Key Features**:
- MediatR request handlers for all identity operations
- Commands: Authentication, user management, authorization
- Queries: User retrieval, role/claim queries
- Clean separation of coordination logic

#### StruttonTechnologies.Core.Identity.Coordinator.Contracts
**Package**: `StruttonTechnologies.Core.Identity.Coordinator.Contracts`  
**Purpose**: Command and query contract definitions  
**Use When**: Consuming identity operations via MediatR

**Key Features**:
- All MediatR IRequest definitions
- Authentication commands
- User management commands and queries
- Authorization commands and queries
- JWT token commands

#### StruttonTechnologies.Core.Identity.Orchestration
**Package**: `StruttonTechnologies.Core.Identity.Orchestration`  
**Purpose**: Business logic and identity infrastructure orchestration  
**Use When**: Implementing custom identity workflows

**Key Features**:
- UserManager and SignInManager orchestration
- JWT token generation and validation
- Authentication workflow management
- External login integration

#### StruttonTechnologies.Core.Identity.Orchestration.Contracts
**Package**: `StruttonTechnologies.Core.Identity.Orchestration.Contracts`  
**Purpose**: Orchestration layer interface definitions  
**Use When**: Implementing or mocking orchestration services

**Key Features**:
- `IAuthenticationOrchestration`: Authentication workflows
- `ITokenOrchestration`: JWT token operations
- `IExternalLoginIdentity`: External provider integration

#### StruttonTechnologies.Core.Identity.Domain
**Package**: `StruttonTechnologies.Core.Identity.Domain`  
**Purpose**: Domain entities and aggregates  
**Use When**: Defining database schema or domain models

**Key Features**:
- `IdentityUser`: Extended ASP.NET Core Identity user
- `IdentityRole`: Extended ASP.NET Core Identity role
- `RefreshToken`: JWT refresh token entity
- `AccessTokenRevocation`: Token revocation tracking
- `JwtTokenOptions`: Token configuration model

#### StruttonTechnologies.Core.Identity.Domain.Contracts
**Package**: `StruttonTechnologies.Core.Identity.Domain.Contracts`  
**Purpose**: Domain service contracts  
**Use When**: Implementing token storage or custom token management

**Key Features**:
- `IRefreshTokenStore`: Refresh token persistence
- `IAccessTokenRevocationStore`: Token revocation tracking
- `IJwtUserTokenManager`: Custom token provider for ASP.NET Core Identity

#### StruttonTechnologies.Core.Identity.EF
**Package**: `StruttonTechnologies.Core.Identity.EF`  
**Purpose**: Entity Framework Core integration  
**Use When**: Setting up database persistence

**Key Features**:
- `CoreIdentityDbContext<TKey, TUser, TRole>`: Base DbContext
- `EfRefreshTokenStore`: EF Core refresh token repository
- `SqlServerRefreshTokenStore`: SQL Server optimized implementation
- `EfAccessTokenRevocationStore`: Token revocation repository
- Pre-configured entity configurations

#### StruttonTechnologies.Core.Identity.Dtos
**Package**: `StruttonTechnologies.Core.Identity.Dtos`  
**Purpose**: Data transfer objects for transport and orchestration  
**Use When**: Building APIs or consuming identity services

**Key Features**:
- Authentication DTOs (login, register, token responses)
- User management DTOs (profiles, summaries, details)
- Authorization DTOs (roles, claims, principals)
- External login DTOs

#### StruttonTechnologies.Core.Identity.JwtTokenManager
**Package**: `StruttonTechnologies.Core.Identity.JwtTokenManager`  
**Purpose**: JWT token provider for ASP.NET Core Identity  
**Use When**: Configuring JWT authentication

**Key Features**:
- `JwtUserTokenManager`: Custom IUserTwoFactorTokenProvider
- Token generation and validation
- Refresh token support
- Configurable token options

#### StruttonTechnologies.Core.Identity.Data
**Package**: `StruttonTechnologies.Core.Identity.Data`  
**Purpose**: Known constants and seed data  
**Use When**: Setting up initial data or referencing standard values

**Key Features**:
- `KnownRoles`: Standard role definitions
- `KnownClaims`: Standard claim types
- `KnownIdentityProviders`: OAuth/OIDC provider names
- `KnownScopes`: Standard OAuth scopes
- `KnownUsers`: Seed user data
- `KnownReservedUsernames`: System reserved usernames
- `KnownPasswordBlacklist`: Common password blacklist

---

## Quick Start

### 1. Basic Setup with MediatR

```csharp
// Install packages
// StruttonTechnologies.Core.Identity.Coordinator
// StruttonTechnologies.Core.Identity.Coordinator.Contracts
// StruttonTechnologies.Core.Identity.EF

// Configure services
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AuthenticateUserHandler).Assembly));
services.AddDbContext<CoreIdentityDbContext<string, IdentityUser, IdentityRole>>(options => 
    options.UseSqlServer(connectionString));
```

### 2. User Authentication

```csharp
// Instead of direct UserManager calls
var user = await _userManager.FindByEmailAsync(email);
var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

// Use coordinated commands
var result = await _mediator.Send(new AuthenticateUserCommand(email, password));
if (result.IsSuccess)
{
    // result.Token contains JWT
}
```

### 3. User Registration

```csharp
// Register new user
var result = await _mediator.Send(new RegisterUserCommand(
    email: "user@example.com",
    password: "SecurePassword123!",
    displayName: "John Doe"
));
```

### 4. JWT Token Management

```csharp
// Generate JWT tokens for a user
var tokenResponse = await _mediator.Send(new GenerateTokenCommand(userId));
// tokenResponse contains AccessToken and RefreshToken

// Refresh expired access token
var newTokens = await _mediator.Send(new RefreshTokenCommand(refreshToken));

// Revoke token (logout)
await _mediator.Send(new RevokeTokenCommand(refreshToken));
```

### 5. User Queries

```csharp
// Get user by ID
var user = await _mediator.Send(new GetUserByIdQuery(userId));
// Returns UserDetailResult with roles, claims, and external logins

// Get user by email
var user = await _mediator.Send(new GetUserByEmailQuery(email));

// Get all users
var users = await _mediator.Send(new GetAllUsersQuery());
// Returns IEnumerable<UserSummaryResult>

// Get user profile
var profile = await _mediator.Send(new GetUserProfileQuery(userId));
```

### 6. Authorization Operations

```csharp
// Assign role to user
await _mediator.Send(new AssignRoleCommand(userId, "Administrator"));

// Remove role from user
await _mediator.Send(new RemoveRoleCommand(userId, "Administrator"));

// Add claim to user
await _mediator.Send(new AddClaimCommand(userId, claimType, claimValue));

// Get user roles
var roles = await _mediator.Send(new GetUserRolesQuery(userId));

// Get user claims
var claims = await _mediator.Send(new GetUserClaimsQuery(userId));

// Get ClaimsPrincipal for authorization
var principal = await _mediator.Send(new GetClaimsPrincipalQuery(userId));
```

### 7. User Management Commands

```csharp
// Update user
await _mediator.Send(new UpdateUserCommand(userId, email, displayName));

// Enable/Disable user
await _mediator.Send(new EnableUserCommand(userId));
await _mediator.Send(new DisableUserCommand(userId));

// Lock/Unlock user (security)
await _mediator.Send(new LockUserCommand(userId, lockoutEnd));
await _mediator.Send(new UnlockUserCommand(userId));

// Change password
await _mediator.Send(new ChangePasswordCommand(userId, currentPassword, newPassword));

// Reset password (forgot password flow)
await _mediator.Send(new ForgotPasswordCommand(email));
await _mediator.Send(new ResetPasswordCommand(email, resetToken, newPassword));

// Confirm email
await _mediator.Send(new ConfirmEmailCommand(userId, confirmationToken));
```

### 8. External Login Integration

```csharp
// Login with external provider
var result = await _mediator.Send(new ExternalLoginCommand(provider, accessToken));

// Link external login to existing user
await _mediator.Send(new LinkExternalLoginCommand(userId, provider, providerKey));

// Unlink external login
await _mediator.Send(new UnlinkExternalLoginCommand(userId, provider, providerKey));

// Get user's external logins
var logins = await _mediator.Send(new GetUserLoginsQuery(userId));
```

### 9. Database Setup

```csharp
// Define your custom user and role if needed
public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; }
}

// Create DbContext
public class ApplicationDbContext : CoreIdentityDbContext<string, ApplicationUser, IdentityRole>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
}

// Configure in Startup
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

---

## Available Commands

### Authentication Commands
- `AuthenticateUserCommand`: Login with email/password
- `RegisterUserCommand`: Register new user
- `ChangePasswordCommand`: Change user password
- `ForgotPasswordCommand`: Initiate password reset
- `ResetPasswordCommand`: Complete password reset
- `ConfirmEmailCommand`: Confirm email address
- `SignOutCommand`: Sign out user

### User Management Commands
- `CreateUserCommand`: Create user programmatically
- `UpdateUserCommand`: Update user details
- `EnableUserCommand`: Enable user account
- `DisableUserCommand`: Disable user account
- `LockUserCommand`: Lock user account (security)
- `UnlockUserCommand`: Unlock user account
- `DeleteUserCommand`: Delete user account

### Authorization Commands
- `AssignRoleCommand`: Assign role to user
- `RemoveRoleCommand`: Remove role from user
- `AddClaimCommand`: Add claim to user
- `RemoveClaimCommand`: Remove claim from user

### JWT Token Commands
- `GenerateTokenCommand`: Generate JWT access and refresh tokens
- `RefreshTokenCommand`: Refresh access token using refresh token
- `RevokeTokenCommand`: Revoke refresh token (logout)

### External Login Commands
- `ExternalLoginCommand`: Login with external provider
- `LinkExternalLoginCommand`: Link external login to user
- `UnlinkExternalLoginCommand`: Unlink external login from user

---

## Available Queries

### User Queries
- `GetUserByIdQuery`: Get user by unique identifier
- `GetUserByEmailQuery`: Get user by email address
- `GetAllUsersQuery`: Get all users (summary view)
- `GetUserProfileQuery`: Get user profile details
- `GetUserLoginsQuery`: Get user's external logins
- `GetNormalizedEmailQuery`: Get normalized email for lookup

### Authorization Queries
- `GetUserRolesQuery`: Get user's assigned roles
- `GetUserClaimsQuery`: Get user's claims
- `GetClaimsPrincipalQuery`: Get ClaimsPrincipal for user

---

## Integration Examples

### ASP.NET Core API Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new AuthenticateUserCommand(
            request.Email, 
            request.Password
        ));

        if (result.IsSuccess)
        {
            return Ok(new { token = result.Token });
        }

        return Unauthorized(new { error = result.FailureReason });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _mediator.Send(new RegisterUserCommand(
            request.Email,
            request.Password,
            request.DisplayName
        ));

        return Ok(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _mediator.Send(new ChangePasswordCommand(
            userId,
            request.CurrentPassword,
            request.NewPassword
        ));

        return Ok(result);
    }
}
```

### Minimal API Example

```csharp
app.MapPost("/api/auth/login", async (LoginRequest request, IMediator mediator) =>
{
    var result = await mediator.Send(new AuthenticateUserCommand(
        request.Email,
        request.Password
    ));

    return result.IsSuccess 
        ? Results.Ok(new { token = result.Token })
        : Results.Unauthorized();
});

app.MapPost("/api/users", async (CreateUserRequest request, IMediator mediator) =>
{
    var result = await mediator.Send(new CreateUserCommand(
        request.Email,
        request.Password,
        request.DisplayName
    ));

    return Results.Ok(result);
}).RequireAuthorization();
```

### Custom Service Integration

```csharp
public class UserService
{
    private readonly IMediator _mediator;

    public UserService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<UserDetailResult> GetUserDetailsAsync(string userId)
    {
        return await _mediator.Send(new GetUserByIdQuery(userId));
    }

    public async Task AssignAdministratorRoleAsync(string userId)
    {
        await _mediator.Send(new AssignRoleCommand(userId, KnownRoles.Administrator));
    }

    public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
    {
        var result = await _mediator.Send(new AuthenticateUserCommand(email, password));
        return result.IsSuccess;
    }
}
```

---

## Configuration

### JWT Configuration

```csharp
services.Configure<JwtTokenOptions>(options =>
{
    options.Issuer = "your-issuer";
    options.Audience = "your-audience";
    options.SecretKey = "your-secret-key";
    options.AccessTokenExpirationMinutes = 15;
    options.RefreshTokenExpirationDays = 7;
});
```

### Identity Configuration

```csharp
services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddTokenProvider<JwtUserTokenManager>("JWT");
```

---

## Why This Architecture Matters

### Benefits

**Clear Separation of Concerns**
- API layer handles HTTP concerns
- Coordinator layer handles CQRS orchestration
- Orchestration layer handles business logic
- Domain layer defines entities
- Data layer handles persistence

**Testability**
- Each layer can be tested independently
- Commands and queries are testable without HTTP infrastructure
- Orchestration logic isolated from data access
- Easy to mock dependencies

**Maintainability**
- Consistent patterns across all identity operations
- Changes localized to specific layers
- Easy to locate and modify functionality
- Reduced code duplication

**Predictability**
- All operations follow CQRS pattern
- Well-defined contracts and DTOs
- Type-safe command and query handling
- Explicit dependencies

**Scalability**
- Commands and queries can be optimized independently
- Easy to add caching at coordinator or orchestration layer
- Can distribute command processing if needed
- Read models (queries) separate from write models (commands)

---

## Testing Support

The solution includes test helper packages:

- **StruttonTechnologies.Core.Identity.Fakes**: Fake implementations for testing
- **StruttonTechnologies.Core.Identity.Mocks**: Mock objects and builders
- **StruttonTechnologies.Core.Identity.Test.Data**: Test data generators
- **StruttonTechnologies.Core.Identity.Stub**: Stub implementations

---

## 🚧 Status

This project is currently in **alpha** development (version 1.0.0-alpha). The architecture and APIs are stabilizing, but breaking changes may occur as the implementation continues to evolve.

---

## Additional Resources

- **ASP.NET Core Identity**: [Official Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- **MediatR**: [GitHub Repository](https://github.com/jbogard/MediatR)
- **JWT Authentication**: [jwt.io](https://jwt.io/)
- **CQRS Pattern**: [Martin Fowler's CQRS](https://martinfowler.com/bliki/CQRS.html)
