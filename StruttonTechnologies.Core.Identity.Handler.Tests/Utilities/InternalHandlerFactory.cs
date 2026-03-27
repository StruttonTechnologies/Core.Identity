namespace StruttonTechnologies.Core.Identity.Handler.Tests.Utilities
{
    [ExcludeFromCodeCoverage]
    internal static class InternalHandlerFactory
    {
        public static object Create(string fullTypeName, params object[] arguments)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fullTypeName);

            Type openType = Type.GetType($"{fullTypeName}, StruttonTechnologies.Core.Identity.Coordinator", throwOnError: true)!
                ?? throw new InvalidOperationException($"Unable to resolve type '{fullTypeName}'.");

            Type constructedType = openType;
            int arity = openType.IsGenericTypeDefinition ? openType.GetGenericArguments().Length : 0;

            if (arity == 1)
            {
                constructedType = openType.MakeGenericType(typeof(StruttonTechnologies.Core.Identity.Stub.Entities.StubUser));
            }
            else if (arity == 2)
            {
                constructedType = openType.MakeGenericType(typeof(StruttonTechnologies.Core.Identity.Stub.Entities.StubUser), typeof(Guid));
            }

            return Activator.CreateInstance(constructedType, arguments)
                ?? throw new InvalidOperationException($"Unable to create instance of '{constructedType.FullName}'.");
        }

        public static async Task<TResult> InvokeHandleAsync<TResult>(object handler, object request)
        {
            ArgumentNullException.ThrowIfNull(handler);
            ArgumentNullException.ThrowIfNull(request);

            System.Reflection.MethodInfo method = handler.GetType().GetMethod("Handle")
                ?? throw new InvalidOperationException("Handle method not found.");

            Task task = (Task)(method.Invoke(handler, new[] { request, CancellationToken.None })
                ?? throw new InvalidOperationException("Handle invocation returned null."));

            await task.ConfigureAwait(false);

            return (TResult)task.GetType().GetProperty("Result")!.GetValue(task)!;
        }
    }
}
