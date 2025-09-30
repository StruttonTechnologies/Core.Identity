using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication.RegisterUser
{
    /// <summary>
    /// Represents the outcome of a successful user registration.
    /// </summary>
    public record RegistrationResponseDto(
        string UserName,
        string Email,
        bool IsNewUser
    );
}
