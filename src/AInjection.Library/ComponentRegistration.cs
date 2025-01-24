using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AInjection
{
	public class ComponentRegistration(Type instanceType, Type[] types)
	{
		public Type InstanceType { get; } = instanceType;
		public Type[] AbstractionTypes { get; } = types;
	}
}
