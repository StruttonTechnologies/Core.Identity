using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Represents the response for a password reset request.
    /// </summary>
    public class PasswordResetResponseDto
    {
        /// <summary>
        /// Indicates whether the password reset was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// The token generated for resetting the password.
        /// </summary>
        public string ResetToken { get; set; } = string.Empty;

        /// <summary>
        /// A message describing the result of the password reset operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
