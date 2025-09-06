namespace ST.Core.Identity.Dtos.Authentication.TwoFactor
{
    public record GenerateTwoFactorTokenRequestDto(string UserId, string Provider);
}