namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    public record VerifyTokenRequestDto(string UserId, string TokenProvider, string Token);
}