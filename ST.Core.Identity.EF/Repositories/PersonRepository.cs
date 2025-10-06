using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Infrastructure.Repositories;
using ST.Core.Infrastructure.Repositories.CrudRepository;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.EF.SqlServer.Repositories
{
    /// <summary>
    /// Repository for Person entities with custom query methods.
    /// </summary>
    public class PersonRepository<TPerson,TKey> : CrudRepository<TPerson,TKey>
        where TPerson : PersonBase<TPerson, TKey>, new()
        where TKey : IEquatable<TKey>
    {
        protected DbSet<TPerson> DbSet => Context.Set<TPerson>();


        public PersonRepository(DbContext context, ILogger<CrudRepository<TPerson, TKey>> logger)
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
        public async Task<ValidationResult> EnsureEmailIsUniqueAsync(string email)
        {
            var exists = await EmailExistsAsync(email);
            return (ValidationResult)(exists
                ? ValidationResultFactory.Failure("Email already exists.")
                : ValidationResultFactory.Success());
        }

        public async Task<TPerson?> GetByEmailAsync(string email) =>
            await DbSet.FirstOrDefaultAsync(p => p.ContactEmail == email);

        public async Task<bool> EmailExistsAsync(string email) =>
            await DbSet.AnyAsync(p => p.ContactEmail == email);


    }
}
