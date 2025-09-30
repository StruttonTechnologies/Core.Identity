using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.EF.SqlServer.Repositories
{
    /// <summary>
    /// Repository for Person entities with custom query methods.
    /// </summary>
    public class PersonRepository<TPerson> : CrudRepository<TPerson>
        where TPerson : PersonBase<TPerson>, new()
    {
        public PersonRepository(DbContext context, ILogger<CrudRepository<TPerson>> logger)
            : base(context, logger)
        {
        }

        /// <summary>
        /// Finds a person by their first name, last name, and contact email.
        /// </summary>
        public async Task<TPerson?> FindByIdentityAsync(
            string firstName,
            string lastName,
            string contactEmail,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));
            ArgumentException.ThrowIfNullOrWhiteSpace(contactEmail, nameof(contactEmail));

            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p =>
                    p.FirstName == firstName &&
                    p.LastName == lastName &&
                    p.ContactEmail == contactEmail,
                    cancellationToken);
        }
    }
}
