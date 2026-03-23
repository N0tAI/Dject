using System.Reflection;

namespace AInjection
{
	internal class Component(Type instanceType)
	{
		internal Type InstanceType { get; } = instanceType;
		internal InstanceProvider Provider { get; } = new(instanceType);
	}
}
