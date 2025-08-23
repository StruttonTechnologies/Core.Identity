using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Represents a request to change a user's password.
    /// </summary>
    public class ChangePasswordRequestDto
    {
        /// <summary>
        /// Gets or sets the current password of the user.
        /// </summary>
        public required string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password to be set for the user.
        /// </summary>
        public required string NewPassword { get; set; }
    }
}
