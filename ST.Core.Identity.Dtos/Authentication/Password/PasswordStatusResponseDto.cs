using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Password
{
    [ExcludeFromCodeCoverage]
    public record PasswordStatusResponseDto(string UserId, bool HasPassword);
}