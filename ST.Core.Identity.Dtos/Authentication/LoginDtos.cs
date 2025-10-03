namespace ST.Core.Identity.Dtos.Authentication
{
    public record InternalLoginRequestDto(string UserName, string Password);

    public record ExternalLoginRequestDto(string Provider, string ProviderKey);

    public record LoginResponseDto(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        string Username,
        string Provider,
        bool IsNewUser = false,
        bool RequiresTwoFactor = false
    );
}