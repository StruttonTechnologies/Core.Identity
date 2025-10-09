using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API.Contracts.Authentication
{
    public interface IRefreshTokenRequest
    {
        string RefreshToken { get; }
    }
}
