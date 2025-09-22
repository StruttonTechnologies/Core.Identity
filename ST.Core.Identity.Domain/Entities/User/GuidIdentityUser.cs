using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Entities.User
{
    public class GuidIdentityUser<TPerson> : IdentityUserBase<Guid, TPerson>
    where TPerson : class
    { }
}
