using System.Reflection;

namespace Dject
{
	internal class InstanceEntry(Type instanceType)
	{
		internal Type InstanceType { get; } = instanceType;
	}
}
