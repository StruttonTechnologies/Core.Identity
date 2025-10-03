namespace ST.Core.Identity.Dtos.Authentication
{
    public record TwoFactorSetupDto(string SharedKey, string QrCodeUrl);

    public record TwoFactorVerifyDto(string Code);

    public record TwoFactorResponseDto(bool Success, string Message);
}