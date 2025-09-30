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
        string FirstName,
        string LastName,
        IList<string>? Roles = null
    );
}
