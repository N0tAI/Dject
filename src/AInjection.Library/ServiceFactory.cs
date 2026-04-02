using System.Reflection;

namespace Dject
{
	// TODO: Document!
	public class ServiceFactory : IServiceProvider
	{
		private readonly Dictionary<Type, InstanceEntry> _componentRegistry = new();
		private readonly Dictionary<Type, Type> _serviceMapping = new();

		internal ServiceFactory(ServiceFactoryBuilder builder)
		{
			
		}
		
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
			component.Initializer.Invoke(null, ctorArguments.ToArray());

			return ;
		}
	}
}
