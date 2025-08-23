using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Represents the response returned after a logout operation.
    /// </summary>
    public class LogoutResponseDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether the logout was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets a message describing the result of the logout operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
