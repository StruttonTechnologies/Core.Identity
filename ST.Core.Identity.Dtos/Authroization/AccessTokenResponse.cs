using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authroization
{
    public class AccessTokenResponse
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
        public string? TokenType { get; set; } = "Bearer";
    }


}
