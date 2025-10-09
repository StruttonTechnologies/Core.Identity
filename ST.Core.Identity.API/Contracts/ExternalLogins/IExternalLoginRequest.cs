using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API.Contracts.ExternalLogins
{
    public interface IExternalLoginRequest
    {
        string Provider { get; }
        string IdToken { get; }
    }
}
