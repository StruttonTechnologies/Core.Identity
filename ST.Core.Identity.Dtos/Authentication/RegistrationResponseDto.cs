using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Represents the response returned after a user registration attempt.
    /// </summary>
    public class RegistrationResponseDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether the registration was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the registered user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the authentication token if auto-login is enabled; otherwise, null.
        /// </summary>
        public string Token { get; set; } = string.Empty; // Optional, if auto-login

        public List<string> Errors { get; set; } = new List<string>();

    }
}
