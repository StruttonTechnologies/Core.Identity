namespace ST.Core.Identity.Dtos.Authentication.TwoFactor
{
    public record SetTwoFactorEnabledRequestDto(string UserId, bool Enabled);
}