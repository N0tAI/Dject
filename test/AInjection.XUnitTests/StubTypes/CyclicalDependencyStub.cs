namespace AInjection.XUnitTests.StubTypes
{
	internal class CyclicalDependencyStub(CyclicalDependencyStub stub) : IDependantStub
	{
		private readonly CyclicalDependencyStub _stub = stub;
	}
}
