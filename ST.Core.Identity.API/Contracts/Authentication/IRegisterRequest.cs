using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API.Contracts.Authentication
{
    public interface IRegisterRequest
    {
        string Email { get; }
        string Password { get; }
    }
}
