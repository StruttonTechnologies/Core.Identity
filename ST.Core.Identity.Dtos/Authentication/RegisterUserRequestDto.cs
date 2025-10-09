using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    public record RegisterUserRequestDto(
        string Email,
        string Password,
        string DisplayName
    );
}
