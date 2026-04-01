using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authentication
{
    [ExcludeFromCodeCoverage]

    public record AuthenticateRequestDto(
        string Email,
        string Password);
}
