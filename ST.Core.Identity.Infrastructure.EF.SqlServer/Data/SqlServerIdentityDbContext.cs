using Microsoft.EntityFrameworkCore;
using ST.Core.Identity.Domain.Entities;

namespace ST.Core.Identity.Infrastructure.EF.SqlServer
{
    /// <summary>
    /// SQL Server-specific identity DbContext.
    /// </summary>
    public class SqlServerIdentityDbContext<TUser, TPerson> :
        IdentityDbContextBase<TUser, TPerson>
        where TUser : IdentityUserBase<TPerson>
        where TPerson : class
    {
        public SqlServerIdentityDbContext(
            DbContextOptions<SqlServerIdentityDbContext<TUser, TPerson>> options)
            : base(options) { }
    }
}