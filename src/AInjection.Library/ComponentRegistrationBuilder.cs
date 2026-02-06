namespace AInjection
{
	/// <summary>
	/// Builder class for constructing necessary data to build a new component
	/// </summary>
	/// <param name="instanceType">The type that is implementating the service(s) in <see cref="ServiceTypes"/></param>
	public class ComponentRegistrationBuilder(Type instanceType)
	{
		/// <summary>
		/// The type that is implementating the service(s) in <see cref="ServiceTypes"/>
		/// </summary>
		public Type InstanceType { get; } = instanceType;
		private List<Type>? _serviceTypes = null;
		/// <summary>
		/// All services this component implements
		/// </summary>
		public Type[] ServiceTypes => _serviceTypes?.ToArray() ?? Array.Empty<Type>();

		/// <summary>
		/// Register the component as implementing <paramref name="serviceType"/>
		/// </summary>
		/// <param name="serviceType">The service that the component should implement</param>
		/// <returns>A component registration builder to chain configuration calls</returns>
		/// <exception cref="ArgumentException">Thrown if <see cref="InstanceType"/> is not implicitly assignable to <paramref name="serviceType"/></exception>
		public ComponentRegistrationBuilder Provides(Type serviceType)
		{
			if (!InstanceType.IsAssignableTo(serviceType))
				throw new ArgumentException($"Type {InstanceType.FullName} is not assignable to {serviceType.FullName}"); ;
			_serviceTypes ??= new();
			_serviceTypes.Add(serviceType);
			return this;
		}

		/// <summary>
		/// Register the component as implementing <typeparamref name="TService"/>
		/// </summary>
		/// <typeparam name="TService">The service that the component should implement</typeparam>
		/// <returns>A component registration builder to chain configuration calls</returns>
		/// <exception cref="ArgumentException">Thrown if <see cref="InstanceType"/> is not implicitly assignable to <typeparamref name="TService"/></exception>
		public ComponentRegistrationBuilder Provides<TService>()
			=> Provides(typeof(TService));

		internal ComponentRegistration Build()
		{
			return new ComponentRegistration(InstanceType, ServiceTypes);
		}
	}
}
