using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Data transfer object for logout requests.
    /// </summary>
    public class LogoutRequestDto
    {
        /// <summary>
        /// The refresh token to be invalidated during logout.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
