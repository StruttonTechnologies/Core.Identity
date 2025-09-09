
using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Lockout
{
    /// <summary>
    /// Represents the lockout status of a user, including whether they are currently locked out and when the lockout ends.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record LockoutStatusResponseDto(Guid UserId, bool IsLockedOut, DateTime? LockoutEnd, int AccessFailedCount);
}