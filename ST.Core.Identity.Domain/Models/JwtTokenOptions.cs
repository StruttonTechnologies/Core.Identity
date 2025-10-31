using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Models
{
    public class JwtTokenOptions
    {
        public string SigningKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; }
    }
}
