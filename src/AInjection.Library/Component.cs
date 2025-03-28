using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AInjection
{
	internal class Component(Type instanceType)
	{
		internal Type InstanceType { get; } = instanceType;
		internal MethodBase? Initializer { get; set; } = null;
	}
}
