using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Entities
{
    /// <summary>
    /// Represents a refresh token used for renewing authentication sessions.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Gets or sets the refresh token string.
        /// </summary>
        public string Token { get; set; } = default!;

        /// <summary>
        /// Gets or sets the identifier of the user associated with this token.
        /// </summary>
        public string UserId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the username of the user associated with this token.
        /// </summary>
        public string Username { get; set; } = default!;

        /// <summary>
        /// Gets or sets the date and time when the token was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the token expires.
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the token has been revoked.
        /// </summary>
        public bool IsRevoked { get; set; }
    }
}
