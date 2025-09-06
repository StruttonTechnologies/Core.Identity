namespace ST.Core.Identity.Dtos.Authentication.Lockout
{
    public record SetLockoutRequestDto(string UserId, DateTimeOffset? LockoutEnd);
}