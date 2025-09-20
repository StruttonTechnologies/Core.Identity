using ST.Core.Identity.Models;
using ST.Core.Identity.Validators.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Extensions
{
    public static class AuthenticationContextExtensions
    {
        public static AuthContext ToAuthContext(this AuthenticationContext context) =>
            new(context.ProviderName, context.SessionId, context.Status);
    }
}
