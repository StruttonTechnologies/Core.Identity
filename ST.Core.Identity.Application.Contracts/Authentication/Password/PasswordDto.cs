namespace ST.Core.Identity.Application.Contracts.Authentication.Password
{
    public record PasswordDto(string UserId, string? OldPassword = null, string? NewPassword = null);
}