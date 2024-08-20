using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace MinimalMediator
{
    /// <summary>
    /// Defines the interface for a mediator object.
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Publishes a message to all subscribers.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message to publish.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a request to a specific handler and returns the response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response from the handler.</returns>
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Registers a handler for a specific message type.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The handler function to register.</param>
        void Register<TMessage>(Func<TMessage, CancellationToken, Task> handler);
    }

    /// <summary>
    /// Defines the interface for a request handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface IRequestHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Handles a request and returns the response.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents a mediator object that can publish messages and send requests.
    /// </summary>
    public class MinimalMediator : IMediator
    {
        private readonly ConcurrentDictionary<Type, Delegate> _Handlers = new();

        /// <summary>
        /// Publishes a message to all subscribers.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message to publish.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        {
            if (_Handlers.TryGetValue(typeof(TMessage), out Delegate? handler))
            {
                return ((Func<TMessage, CancellationToken, Task>)handler).Invoke(message, cancellationToken);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends a request to a specific handler and returns the response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response from the handler.</returns>
        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        {
            if (_Handlers.TryGetValue(typeof(TRequest), out Delegate? handler))
            {
                try
                {
                    return await ((Func<TRequest, CancellationToken, Task<TResponse>>)handler).Invoke(request, cancellationToken);
                }
                catch (TargetInvocationException ex)
                {
                    throw new InvalidOperationException($"Failed to invoke handler for {typeof(TRequest).Name}", ex.InnerException);
                }
            }
            throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        }

        /// <summary>
        /// Registers a handler for a specific message type.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The handler function that will be invoked when a message of type TMessage is published.</param>
        public void Register<TMessage>(Func<TMessage, CancellationToken, Task> handler)
        {
            _Handlers.AddOrUpdate(typeof(TMessage), handler, (_, _) => handler);
        }
    }

    /// <summary>
    /// Provides extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the request handler types from the specified assemblies to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="assemblies">The assemblies to search for request handler types.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddMinimalMediatorRequestHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            IEnumerable<Type> handlerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IRequestHandler<,>)));

            foreach (Type handlerType in handlerTypes)
            {
                (Type? requestType, Type? responseType) = GetRequestAndResponseTypes(handlerType);

                if (requestType != null && responseType != null)
                {
                    Type serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

                    if (serviceType.IsAssignableFrom(handlerType))
                    {
                        services.AddScoped(serviceType, handlerType);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Handler type {handlerType.Name} does not implement {serviceType.Name}");
                    }
                }
            }
            return services;
        }

        /// <summary>
        /// Adds the MinimalMediator as a singleton to the IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the MinimalMediator to.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddMinimalMediator(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, MinimalMediator>();
            return services;
        }

        /// <summary>
        /// Retrieves the request and response types from a given handler type.
        /// </summary>
        /// <param name="handlerType">The type of the handler.</param>
        /// <returns>A tuple containing the request type and response type, or null if the handler does not implement IRequestHandler<,>.</returns>
        private static (Type?, Type?) GetRequestAndResponseTypes(Type handlerType)
        {
            Type[] interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .ToArray();
            if (interfaces.Length == 1)
            {
                Type[] genericArguments = interfaces[0].GetGenericArguments();
                return (genericArguments[0], genericArguments[1]);
            }
            return (null, null);
        }
    }

}
