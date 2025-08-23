using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authroization
{
    public class RoleAssignmentDto
    {
        public required string UserId { get; set; }
        public required List<string> Roles { get; set; }
    }
}
