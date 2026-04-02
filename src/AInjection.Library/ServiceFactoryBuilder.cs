namespace Dject;

/// <summary>
/// 
/// </summary>
public class ServiceFactoryBuilder
{
    public void Register(Type instanceType, Action<ComponentRegistrationBuilder>? configure = null)
    {
        ComponentRegistrationBuilder builder = new(instanceType);
        if(configure is null)
            builder.Provides(instanceType);
        else
            configure(builder);
        var registration = builder.Build();
        _componentRegistry.Add(instanceType, registration);
        var serviceComponent = new Component(registration.InstanceType);
        foreach(var service in registration.AbstractionTypes)
        {
            // Remove if already exists
            _serviceMapping.Remove(service);
            _serviceMapping.Add(service, serviceComponent);
        }
    }
    public void Register<T>(Action<ComponentRegistrationBuilder>? configure = null)
        => Register(typeof(T), configure);
    
    public ServiceFactory Build()
    {
        return new ServiceFactory(this);
    }
}