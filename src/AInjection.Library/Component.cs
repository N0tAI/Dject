using System.Reflection;

namespace AInjection
{
	internal class Component(Type instanceType)
	{
		internal Type InstanceType { get; } = instanceType;
		internal MethodBase? Initializer { get; set; } = null;
	}
}
