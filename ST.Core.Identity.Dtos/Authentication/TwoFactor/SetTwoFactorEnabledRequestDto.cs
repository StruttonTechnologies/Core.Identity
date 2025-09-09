using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.TwoFactor
{
    [ExcludeFromCodeCoverage]
    public record SetTwoFactorEnabledRequestDto(string UserId, bool Enabled);
}