namespace ST.Core.Identity.Dtos.Authentication.Password
{
    public record ResetPasswordRequestDto(string UserId, string Token, string NewPassword);
}