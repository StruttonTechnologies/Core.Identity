using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Represents a claim associated with a user.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user.</param>
    /// <param name="Type">The type of the claim.</param>
    /// <param name="Value">The value of the claim.</param>
    [ExcludeFromCodeCoverage]
    public record ClaimResponseDto(string UserId, string Type, string Value);
}