using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication.RegisterUser
{
    /// <summary>
    /// Represents the input required to register a new user.
    /// </summary>
    public record RegistrationRequestDto(
        string UserName,
        string Email,
        string Password,
        IList<string>? Roles = null
    );
}
