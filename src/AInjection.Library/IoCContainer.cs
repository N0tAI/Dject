using System.Reflection;

namespace AInjection
{
	public class IoCContainer : IServiceProvider
	{
		private readonly Dictionary<Type, Type> _serviceMapping = new();

		public void Register<TAbstraction, TInstance>()
			where TInstance : TAbstraction
			=> RegisterService(typeof(TAbstraction), typeof(TInstance));
		public void Register(Type abstraction, Type instance)
		{
			if (instance != abstraction && !instance.IsAssignableTo(abstraction))
				throw new ArgumentException($"Type {instance.FullName} is not assignable to {abstraction.FullName}");
			RegisterService(abstraction, instance);
		}
		private void RegisterService(Type abstractionType, Type implType)
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
			{
				List<ConstructorInfo> validConstructors = new();
				var ctors = implType.GetConstructors();

				// Find best matching ctor (ctor with the most parameters that matches those in the mapping)
				foreach (var ctor in ctors)
				{
					var parameters = ctor.GetParameters();
					bool hasAllParameters = true;
					foreach(var param in parameters)
					{
						if (!_serviceMapping.ContainsKey(param.ParameterType))
						{
							hasAllParameters = false;
							break;
						}
					}
					if (hasAllParameters)
						validConstructors.Add(ctor);
				}

				if (validConstructors.Count == 0)
					throw new MissingMethodException($"Service instance ${implType.FullName} does not have parameterless constructor or constructor that can be fulfilled");

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
