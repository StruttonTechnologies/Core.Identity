using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatch.Authorization.Handlers
{
    [AutoRegister(ServiceLifetime.Scoped)]
    internal class AssignRoleCommandHandler
    {
    }
}
