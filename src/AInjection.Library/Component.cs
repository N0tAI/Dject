using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AInjection
{
	internal class Component(Type instanceType)
	{
		internal Type InstanceType { get; } = instanceType;
	}
}
