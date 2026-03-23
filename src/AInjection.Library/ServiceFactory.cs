using System.Reflection;

namespace AInjection
{
	// TODO: Document!
	public class ServiceFactory : IServiceProvider
	{
		private readonly Dictionary<Type, ComponentRegistration> _componentRegistry = new();
		private readonly Dictionary<Type, Component> _serviceMapping = new();

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

		public bool IsProvided(Type abstractionType)
		{
			return _serviceMapping.ContainsKey(abstractionType);
		}
		public bool IsProvided<T>()
			=> IsProvided(typeof(T));

		public object? GetService(Type serviceType)
		{
			if (!_serviceMapping.TryGetValue(serviceType, out var component))
				return null;
			
			
        var ctorArguments = component.Initializer.GetParameters().Select(param => GetService(param.ParameterType)!);
        component.Initializer.Invoke(null, ctorArguments.ToArray())

			return ;
		}
	}
}
