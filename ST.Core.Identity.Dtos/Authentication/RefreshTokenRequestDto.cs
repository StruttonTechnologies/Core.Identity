using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// DTO for requesting a refresh token operation.
    /// </summary>
    public class RefreshTokenRequestDto
    {
        /// <summary>
        /// The refresh token to be validated and exchanged.
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Optional user identifier for extra validation.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Optional client identifier for multi-host support.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Optional device fingerprint for device-level security.
        /// </summary>
        public string? Fingerprint { get; set; }

        /// <summary>
        /// Optional requested scopes for scope-based access.
        /// </summary>
        public string[]? RequestedScopes { get; set; }
    }
}
