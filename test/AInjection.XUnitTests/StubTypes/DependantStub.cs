using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AInjection.XUnitTests.StubTypes
{
	internal class DependantStub(IEmptyStub stub)
	{
	}
}
