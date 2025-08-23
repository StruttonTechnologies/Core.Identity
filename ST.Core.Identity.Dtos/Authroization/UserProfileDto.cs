using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authroization
{
    /// <summary>
    /// Represents a user profile data transfer object.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string DisplayName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the roles assigned to the user.
        /// </summary>
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
