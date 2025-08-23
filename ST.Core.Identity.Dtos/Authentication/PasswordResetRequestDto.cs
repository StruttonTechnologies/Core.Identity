using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Data transfer object for requesting a password reset.
    /// </summary>
    public class PasswordResetRequestDto
    {
        /// <summary>
        /// Gets or sets the email address associated with the password reset request.
        /// </summary>
        public required string Email { get; set; }
    }
}
