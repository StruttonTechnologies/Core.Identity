using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.User
{
    [ExcludeFromCodeCoverage]
    public record UpdateUsernameRequestDto(string UserId, string NewUserName);
}