namespace ST.Core.Identity.Dtos.Authentication.TwoFactor
{
    public record VerifyTwoFactorTokenRequestDto(string UserId, string Provider, string Token);
}