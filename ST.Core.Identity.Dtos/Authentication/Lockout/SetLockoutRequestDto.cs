using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Lockout
{
    [ExcludeFromCodeCoverage]
    public record SetLockoutRequestDto(string UserId, DateTimeOffset? LockoutEnd);
}