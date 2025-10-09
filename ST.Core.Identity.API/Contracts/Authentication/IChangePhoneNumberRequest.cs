using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API.Contracts.Authentication
{
    public interface IChangePhoneNumberRequest
    {
        string UserId { get; }
        string PhoneNumber { get; }
        string Token { get; }
    }
}
