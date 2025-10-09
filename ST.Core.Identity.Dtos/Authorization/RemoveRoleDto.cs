using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authorization
{
    public record RemoveRoleDto(string UserId, string RoleName);
}
