using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Represents the response for a change password operation.
    /// </summary>
    public class ChangePasswordResponseDto
    {
        /// <summary>
        /// Indicates whether the password change was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Provides a message describing the result of the password change operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
