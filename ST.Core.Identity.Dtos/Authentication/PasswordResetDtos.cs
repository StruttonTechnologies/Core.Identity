namespace ST.Core.Identity.Dtos.Authentication
{
    public record PasswordResetRequestDto(string Email);

    public record PasswordResetConfirmDto(string Token, string NewPassword);

    public record PasswordResetResponseDto(bool Success, string Message);
}
