using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.Query;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Utilities.AsyncQuerying
{
    /// <summary>
    /// Provides an asynchronous query provider for testing Entity Framework Core queries.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
    [ExcludeFromCodeCoverage]
    internal sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        /// <summary>
        /// The inner query provider used for executing synchronous queries.
        /// </summary>
        private readonly IQueryProvider _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncQueryProvider{TEntity}"/> class.
        /// </summary>
        /// <param name="inner">The inner query provider to wrap.</param>
        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Creates a queryable sequence from the specified expression.
        /// </summary>
        /// <param name="expression">The expression representing the query.</param>
        /// <returns>An <see cref="IQueryable"/> that can enumerate the results of the query.</returns>
        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        /// <summary>
        /// Creates a queryable sequence of the specified element type from the specified expression.
        /// </summary>
        /// <typeparam name="TElement">The type of elements in the sequence.</typeparam>
        /// <param name="expression">The expression representing the query.</param>
        /// <returns>An <see cref="IQueryable{TElement}"/> that can enumerate the results of the query.</returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        /// <summary>
        /// Executes the query represented by the specified expression tree.
        /// </summary>
        /// <param name="expression">The expression representing the query to execute.</param>
        /// <returns>The result of executing the query.</returns>
        public object? Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        /// <summary>
        /// Executes the strongly-typed query represented by the specified expression tree.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression representing the query to execute.</param>
        /// <returns>The result of executing the query.</returns>
        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        /// <summary>
        /// Executes the strongly-typed query asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression representing the query to execute.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The result of executing the query.</returns>
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            return Execute<TResult>(expression);
        }
    }
}
