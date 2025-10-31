using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Configuration
{
    public class TokenProviderOptionsConfig
    {
        public int TokenLifespanHours { get; set; } = 24;
    }
}
