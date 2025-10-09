using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Handlers.Authorization
{
    [AutoRegister(ServiceLifetime.Scoped)]
    internal class AddClaimCommandHandler
    {
    }
}
