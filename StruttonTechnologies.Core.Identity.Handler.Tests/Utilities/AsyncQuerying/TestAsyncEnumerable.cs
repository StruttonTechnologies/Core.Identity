using System.Linq.Expressions;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Utilities.AsyncQuerying
{
    /// <summary>
    /// Provides a test implementation of an asynchronous enumerable that supports LINQ queries.
    /// This class is used to enable testing of asynchronous LINQ operations such as ToListAsync.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    [ExcludeFromCodeCoverage]
    internal sealed class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="enumerable">The enumerable collection to wrap.</param>
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="expression">The expression tree representing the query.</param>
        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Returns an asynchronous enumerator that iterates through the collection.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the enumeration.</param>
        /// <returns>An <see cref="IAsyncEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }
}
