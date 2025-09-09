using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Password
{
    [ExcludeFromCodeCoverage]
    public record ResetPasswordRequestDto(string UserId, string Token, string NewPassword);
}