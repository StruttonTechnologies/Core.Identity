using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authroization
{
    public class RoleDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<ClaimDto> Claims { get; set; } = new();
    }
}
