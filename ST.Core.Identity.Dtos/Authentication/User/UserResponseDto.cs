using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.User
{
    [ExcludeFromCodeCoverage]
    public record UserResponseDto(string UserId, string UserName, string Email, bool EmailConfirmed);
}