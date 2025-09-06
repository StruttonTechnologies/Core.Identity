namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    public record TokenResponseDto(string UserId, string Token, DateTime ExpiresAt);
}