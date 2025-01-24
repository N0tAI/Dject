using System.ComponentModel;
using System.Reflection;

namespace AInjection
{
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
			ComponentRegistration registration = builder.Build();
			_componentRegistry.Add(instanceType, registration);
			var serviceComponent = new Component(registration.InstanceType);
			foreach(var service in registration.AbstractionTypes)
			{
				if(_serviceMapping.ContainsKey(service))
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
			if(_serviceMapping.TryGetValue(serviceType, out var component))
			{
				List<ConstructorInfo> validConstructors = new();
				var implementingType = component.InstanceType;
				var ctors = implementingType.GetConstructors();

				// Find best matching ctor (ctor with the most parameters that matches those in the mapping)
				foreach (var ctor in ctors)
				{
					var parameters = ctor.GetParameters();
					bool hasAllParameters = true;
					foreach(var param in parameters)
					{
						// Parameter is not registered in the DI container thus cannot be handled
						if (!_serviceMapping.ContainsKey(param.ParameterType))
						{
							hasAllParameters = false;
							break;
						}
						// Parameter cannot be the type being instantiated as would be a cyclical dependency (bad practice + stack overflow)
						if(param.ParameterType == serviceType || param.ParameterType == implementingType)
						{
							hasAllParameters = false;
							break;
						}
					}
					if (hasAllParameters)
						validConstructors.Add(ctor);
				}

				if (validConstructors.Count == 0)
					throw new MissingMethodException($"Service instance ${implementingType.FullName} does not have parameterless constructor or constructor that can be fulfilled");

				validConstructors.OrderByDescending(ctor => ctor.GetParameters().Count());
				var ctorToUse = validConstructors[0];
				List<object> ctorArguments = new();
				foreach (var param in ctorToUse.GetParameters())
					ctorArguments.Add(GetService(param.ParameterType)!);

				return ctorToUse.Invoke(ctorArguments.ToArray());
			}
			return null;
		}
	}
}
