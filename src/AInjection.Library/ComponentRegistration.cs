namespace AInjection
{
	public class ComponentRegistration(Type instanceType, Type[] types)
	{
		public Type InstanceType { get; } = instanceType;
		public Type[] AbstractionTypes { get; } = types;
	}
}
