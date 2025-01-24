using System.Reflection;

namespace AInjection
{
	public class IoCContainer : IServiceProvider
	{
		private readonly Dictionary<Type, Type> _serviceMapping = new();

		public void Register(Type abstractionType, Type implType)
		{
			if(_serviceMapping.ContainsKey(abstractionType))
				_serviceMapping.Remove(abstractionType);

			_serviceMapping.Add(abstractionType, implType);
		}
		public bool TryRegister(Type abstractionType, Type implType)
		{
			return _serviceMapping.TryAdd(abstractionType, implType);
		}
		public bool Contains(Type abstractionType)
		{
			return _serviceMapping.ContainsKey(abstractionType);
		}

		public object? GetService(Type serviceType)
		{
			if(_serviceMapping.TryGetValue(serviceType, out var implType))
				return Activator.CreateInstance(implType);
			return null;
		}
	}
}
