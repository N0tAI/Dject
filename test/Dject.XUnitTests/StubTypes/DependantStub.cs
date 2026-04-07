using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dject.XUnitTests.StubTypes
{
	internal class DependantStub(IEmptyStub stub) : IDependantStub
	{
		internal IEmptyStub Stub { get; } = stub;
	}
}
