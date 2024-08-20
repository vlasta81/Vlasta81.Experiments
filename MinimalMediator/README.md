# MinimalMediator

MinimalMediator is a lightweight mediator implementation for publishing messages and sending requests with support for asynchronous operations. The library is built on top of Microsoft.Extensions.DependencyInjection for easy integration into ASP.NET Core projects and other projects using the DI container.

## Interfaces

### `IMediator`

Defines the interface for a mediator object.

#### Methods

- **`Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)`**

  Publishes a message to all subscribers.

  - `TMessage`: The type of the message.
  - `message`: The message to publish.
  - `cancellationToken`: Optional cancellation token for the asynchronous operation.
  
- **`Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)`**

  Sends a request to a specific handler and returns the response.

  - `TRequest`: The type of the request.
  - `TResponse`: The type of the response.
  - `request`: The request to send.
  - `cancellationToken`: Optional cancellation token for the asynchronous operation.
  
- **`void Register<TMessage>(Func<TMessage, CancellationToken, Task> handler)`**

  Registers a handler for a specific message type.

  - `TMessage`: The type of the message.
  - `handler`: The handler function that will be invoked when a message of type `TMessage` is published.

### `IRequestHandler<TRequest, TResponse>`

Defines the interface for request handlers.

#### Methods

- **`Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)`**

  Handles a request and returns the response.

  - `request`: The request to handle.
  - `cancellationToken`: The cancellation token for the asynchronous operation.

## Classes

### `MinimalMediator`

Represents a mediator object that can publish messages and send requests.

#### Methods

- **`Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)`**

  Implements the method for publishing messages.

- **`Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)`**

  Implements the method for sending requests and returning responses.

- **`void Register<TMessage>(Func<TMessage, CancellationToken, Task> handler)`**

  Implements the method for registering a handler for a specific message type.

## Extension Methods

### `ServiceCollectionExtensions`

Provides extension methods for the `IServiceCollection` interface.

#### Methods

- **`IServiceCollection AddMinimalMediatorRequestHandlers(this IServiceCollection services, params Assembly[] assemblies)`**

  Adds request handler types from the specified assemblies to the `IServiceCollection`.

  - `services`: The service collection.
  - `assemblies`: The assemblies to search for request handler types.
  
- **`IServiceCollection AddMinimalMediator(this IServiceCollection services)`**

  Adds `MinimalMediator` as a singleton to the `IServiceCollection`.

  - `services`: The service collection.

#### Private Methods

- **`(Type?, Type?) GetRequestAndResponseTypes(Type handlerType)`**

  Retrieves the request and response types from a given handler type.

  - `handlerType`: The type of the handler.
  - Return Type: A tuple containing the request type and response type, or `null` if the handler does not implement `IRequestHandler<,>`.

## Usage

```csharp
// Register MinimalMediator in your DI container
services.AddMinimalMediator();

// Register request handlers
services.AddMinimalMediatorRequestHandlers(Assembly.GetExecutingAssembly());
```

```csharp
// Publishing a message
await mediator.PublishAsync(new MyMessage());

// Sending a request and getting a response
var response = await mediator.RequestAsync<MyRequest, MyResponse>(new MyRequest());
```

## Error Handling

- `InvalidOperationException`: Thrown if no handler is registered for the given request or if the handler type does not implement the expected interface.
```

This documentation covers all the important aspects of the MinimalMediator library, including interfaces, classes, extension methods, usage examples, and error handling.