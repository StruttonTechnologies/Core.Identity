using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Tests.Acceptance;

/// <summary>
/// Documents the API-level acceptance scenarios that must be implemented by consumer API tests.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class AuthenticationAcceptanceRequirementsTests
{
    [Fact]
    public void AuthenticationAcceptanceRequirements_ShouldDefineMinimumProductionScenarios()
    {
        string[] requiredScenarios =
        [
            "Register user",
            "Authenticate user and return access token",
            "Authenticate user and return refresh token",
            "Persist refresh token",
            "Refresh access token using active refresh token",
            "Rotate refresh token on refresh",
            "Reject revoked refresh token reuse",
            "Sign out revokes active refresh token",
            "Sign out all devices revokes all user refresh tokens",
            "Protected endpoint accepts valid access token",
            "Protected endpoint rejects missing token",
            "Protected endpoint rejects revoked or expired token",
        ];

        Assert.All(requiredScenarios, scenario => Assert.False(string.IsNullOrWhiteSpace(scenario)));
    }
}
