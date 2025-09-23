using Microsoft.EntityFrameworkCore;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Identity.Domain.Entities.User;
using ST.Core.Identity.EF;

namespace ST.Core.Identity.EF.SqlServer.Data
{
    /// <summary>
    /// SQL Server-specific identity DbContext.
    /// </summary>
    public class SqlServerIdentityDbContext<TKey, TUser, TPerson> :
    IdentityDbContextBase<TKey, TUser, TPerson>
    where TKey : IEquatable<TKey>
    where TUser : IdentityUserBase<TKey, TPerson>
    where TPerson : PersonBase<TPerson>
    {
        public SqlServerIdentityDbContext(
            DbContextOptions<SqlServerIdentityDbContext<TKey, TUser, TPerson>> options)
            : base(options) { }
    }
}