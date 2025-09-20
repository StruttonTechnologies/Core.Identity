using Microsoft.EntityFrameworkCore;
using ST.Core.Identity.Infrastructure.EF;

namespace ST.Core.Identity.Infrastructure.EF.SqlServer.Stores
{
    ///// <summary>
    ///// Provides methods for storing, retrieving, and revoking refresh tokens using a SQL Server database.
    ///// </summary>
    ///// <typeparam name="TUser">The type of the user entity.</typeparam>
    ///// <typeparam name="TPerson">The type of the person entity.</typeparam>
    //public class SqlServerRefreshTokenStore<TUser, TPerson> : IRefreshTokenStore
    //    where TUser : IdentityUserBase<TPerson>
    //    where TPerson : class
    //{
    //    private readonly IdentityDbContextBase<TUser, TPerson> _context;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="SqlServerRefreshTokenStore{TUser, TPerson}"/> class.
    //    /// </summary>
    //    /// <param name="context">The identity database context.</param>
    //    public SqlServerRefreshTokenStore(IdentityDbContextBase<TUser, TPerson> context)
    //    {
    //        _context = context;
    //    }

    //    /// <summary>
    //    /// Saves a refresh token asynchronously.
    //    /// </summary>
    //    /// <param name="token">The refresh token to save.</param>
    //    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    //    /// <returns>A task that represents the asynchronous save operation.</returns>
    //    public async Task SaveAsync(RefreshToken token, CancellationToken cancellationToken = default)
    //    {
    //        _context.RefreshTokens.Add(token);
    //        await _context.SaveChangesAsync(cancellationToken);
    //    }

    //    /// <summary>
    //    /// Retrieves a refresh token by its token string asynchronously.
    //    /// </summary>
    //    /// <param name="token">The token string to search for.</param>
    //    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    //    /// <returns>The <see cref="RefreshToken"/> if found and not revoked; otherwise, null.</returns>
    //    public async Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken = default)
    //    {
    //        return await _context.RefreshTokens
    //            .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked, cancellationToken);
    //    }

    //    /// <summary>
    //    /// Revokes a refresh token by its token string asynchronously.
    //    /// </summary>
    //    /// <param name="token">The token string to revoke.</param>
    //    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    //    /// <returns>A task that represents the asynchronous revoke operation.</returns>
    //    public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
    //    {
    //        var existing = await _context.RefreshTokens
    //            .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);

    //        if (existing is null) return;

    //        existing.IsRevoked = true;
    //        await _context.SaveChangesAsync(cancellationToken);
    //    }
    //}
}