using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API.Contracts.Users
{
    public interface IDeleteUserRequest
    {
        string UserId { get; }
    }
}
