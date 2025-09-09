using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    [ExcludeFromCodeCoverage]
    public record TokenResponseDto(string UserId, string Token, DateTime ExpiresAt);
}