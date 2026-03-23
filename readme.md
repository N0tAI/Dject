# AInjection

**AInjection** is a lightweight, fluent Dependency Injection (DI) container for C# .NET. 
It provides an intuitive and easy-to-use API for registering services, managing dependencies, and resolving object graphs automatically. 

This project was built to demonstrate a deep understanding of core .NET concepts, including Reflection, Inversion of Control (IoC), generic type constraints, and test-driven development.

## Features

- **Fluent Registration API**: Register components and their implemented interfaces easily using a fluent builder pattern.
- **Automatic Dependency Resolution**: Automatically resolves and injects dependencies through constructor injection.
- **Dependency Validation**: Detects and prevents circular dependencies, and validates that all required dependencies are registered.
- **Type Safety**: Ensures that registered components are actually assignable to the services they claim to provide.
- **Robust Testing**: Comprehensive unit tests written using xUnit to ensure reliability and correct behavior.
- **Benchmarking**: Includes a benchmark project (`AInjection.Benchmarks`) for performance analysis.

*(Note: Advanced features like specific lifetimes (Singleton, Transient) are currently in active development.)*

## Getting Started

### Installation

Clone the repository and include the `AInjection.Library` project in your solution.

### Usage

**1. Define your interfaces and implementations:**

```csharp
public interface IMessageService 
{
    void SendMessage(string message);
}

public class EmailService : IMessageService 
{
    public void SendMessage(string message) 
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class NotificationController 
{
    private readonly IMessageService _messageService;

    // Dependencies are automatically injected via constructor
    public NotificationController(IMessageService messageService) 
    {
        _messageService = messageService;
    }

    public void Notify(string text) 
    {
        _messageService.SendMessage(text);
    }
}
```

**2. Register your services in the container:**

```csharp
using AInjection;

// Initialize the DI container
ServiceFactory factory = new ServiceFactory();

// Register the service and define what interfaces it provides
factory.Register<EmailService>(builder => 
{
    builder.Provides<IMessageService>();
});

// Register a component without a specific interface (registers as self)
factory.Register<NotificationController>();
```

**3. Resolve and use your services:**

```csharp
// The factory will automatically instantiate NotificationController 
// and inject the EmailService into its constructor.
var controller = (NotificationController)factory.GetService(typeof(NotificationController));

controller.Notify("Hello from AInjection!");
```

## Advanced Configuration

### Multiple Service Implementations
A single component can be registered to provide multiple service types:

```csharp
factory.Register<MyMultiService>(builder => 
{
    builder.Provides<IServiceA>()
           .Provides<IServiceB>()
           .Provides<MyMultiService>(); // Can also provide itself
});
```

### Checking Registrations
You can check if a specific service is available in the container before attempting to resolve it:

```csharp
if (factory.IsProvided<IMessageService>()) 
{
    // Safe to resolve
}
```

## Architecture & Design

- `ServiceFactory`: The core IoC container. It stores service mappings and orchestrates instantiation using reflection.
- `ComponentRegistrationBuilder`: Provides a fluent interface to configure how a type is registered and what abstractions it fulfills.
- `InstanceLifetime` (WIP): Abstract base for managing how instances are cached or created (e.g., Singleton vs. Transient).

## Testing

The project is fully tested using **xUnit**. The test suite covers:
- Valid and invalid component registrations.
- Complex dependency resolution.
- Circular dependency detection.
- Missing dependency detection.

To run the tests, use the .NET CLI:
```bash
dotnet test test/AInjection.XUnitTests
```

## License

This project is licensed under the terms found in the `license.md` file.