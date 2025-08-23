using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    public class LoginResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
        public Guid UserId { get; set; } 
        public string Username { get; set; } = default!;

    }
}
