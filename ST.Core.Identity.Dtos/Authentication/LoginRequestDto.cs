using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Data transfer object for user login requests.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public required string Password { get; set; }
    }
}
