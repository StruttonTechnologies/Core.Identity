namespace ST.Core.Identity.Dtos.Authentication
{
    public record TokenRefreshRequestDto(string RefreshToken);

    public record TokenResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);
}
