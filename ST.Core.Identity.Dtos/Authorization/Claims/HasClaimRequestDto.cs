using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Represents a request to check if a user has a specific claim.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user.</param>
    /// <param name="Type">The type of the claim.</param>
    /// <param name="Value">The value of the claim.</param>
    [ExcludeFromCodeCoverage]
    public record HasClaimRequestDto(string UserId, string Type, string Value);
}