using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authroization
{
    public class PermissionDto
    {
        public required string Name { get; set; }
        public string Description { get; set; }= string.Empty;
    }
}
