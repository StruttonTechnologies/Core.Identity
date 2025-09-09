using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Password
{
    [ExcludeFromCodeCoverage]
    public record ChangePasswordRequestDto(string UserId, string OldPassword, string NewPassword);
}