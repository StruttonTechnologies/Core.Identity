using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    [ExcludeFromCodeCoverage]
    public record VerifyTokenRequestDto(string UserId, string TokenProvider, string Token);
}