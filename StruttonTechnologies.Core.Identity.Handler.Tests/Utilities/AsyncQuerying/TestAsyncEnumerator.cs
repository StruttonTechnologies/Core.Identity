namespace StruttonTechnologies.Core.Identity.Handler.Tests.Utilities.AsyncQuerying
{
    /// <summary>
    /// Provides a test implementation of <see cref="IAsyncEnumerator{T}"/> that wraps a synchronous enumerator.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    [ExcludeFromCodeCoverage]
    internal sealed class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAsyncEnumerator{T}"/> class.
        /// </summary>
        /// <param name="inner">The synchronous enumerator to wrap.</param>
        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public T Current => _inner.Current;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> that represents the asynchronous dispose operation.</returns>
        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Advances the enumerator asynchronously to the next element of the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> that will complete with a result of <c>true</c> if the enumerator was successfully advanced
        /// to the next element, or <c>false</c> if the enumerator has passed the end of the collection.
        /// </returns>
        public ValueTask<bool> MoveNextAsync()
        {
            return new(_inner.MoveNext());
        }
    }
}
