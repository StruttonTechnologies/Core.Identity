using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Request DTO for retrieving claims associated with a specific user.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user whose claims are to be retrieved.</param>
    [ExcludeFromCodeCoverage]
    public record GetUserClaimsRequestDto(string UserId);
}