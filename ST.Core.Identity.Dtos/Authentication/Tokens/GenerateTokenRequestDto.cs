namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    public record GenerateTokenRequestDto(string UserId, string TokenProvider);
}