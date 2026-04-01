using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class JwtTokenOptions
    {
        public string SigningKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        public int ExpirationMinutes { get; set; }
    }
}
