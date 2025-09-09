using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.User
{
    [ExcludeFromCodeCoverage]
    public record UpdateEmailRequestDto(string UserId, string NewEmail);
}