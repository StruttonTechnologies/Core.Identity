using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Data transfer object for user registration requests.
    /// </summary>
    public class RegistrationRequestDto
    {
        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the provider name for the new user.
        /// Defaults to "RidgeRiders".
        /// Optional.
        /// </summary>
        public string? ProviderName { get; set; } = "Local";
    }
}
