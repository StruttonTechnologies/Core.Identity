using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API.Contracts.Authentication
{
    public interface IChangePasswordRequest
    {
        string UserId { get; }
        string CurrentPassword { get; }
        string NewPassword { get; }
    }
}
