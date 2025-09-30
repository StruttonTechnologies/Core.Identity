using ST.Core.Domain.Contracts;
using ST.Core.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Interfaces
{
    public interface IPersonRepository <TPerson> : ICrudRepository<TPerson>
        where TPerson : PersonBase<TPerson>, new()
    {
        Task<TPerson?> FindByIdentityAsync(string firstName, string lastName, string contactEmail, CancellationToken cancellationToken = default);
    }
}
