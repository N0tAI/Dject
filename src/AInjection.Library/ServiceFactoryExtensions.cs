namespace AInjection
{
	public static class ServiceFactoryExtensions
	{
		public static void Register<TService>(this ServiceFactory container)
			=> container.Register<TService, TService>();
		public static void  Register(this ServiceFactory container, Type serviceType)
			=> container.Register(serviceType, serviceType);
	}
}
