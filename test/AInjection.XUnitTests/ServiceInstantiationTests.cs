using AInjection.XUnitTests.StubTypes;

namespace AInjection.XUnitTests
{
	public class ServiceInstantiationTests
	{
		[Fact]
		public void Container_InstantiateEmptyService()
		{
			ServiceFactory container = new();
			container.Register(typeof(EmptyStub), typeof(EmptyStub));

			Assert.True(container.Contains(typeof(EmptyStub)));
			Assert.NotNull(container.GetService(typeof(EmptyStub)));
		}

		[Fact]
		public void Container_InstantiateConcreteServiceAbstract()
		{
			ServiceFactory container = new();
			container.Register(typeof(IEmptyStub), typeof(EmptyStub));

			var instance = container.GetService(typeof(IEmptyStub));
			Assert.NotNull(instance);
			Assert.True(instance.GetType() == typeof(EmptyStub));
		}

		[Fact]
		public void Container_InstantiateConcreteService()
		{
			ServiceFactory container = new();
			container.Register(typeof(IEmptyStub), typeof(EmptyStub));
			container.Register(typeof(EmptyStub), typeof(EmptyStub));

			var maskedInstance = container.GetService(typeof(IEmptyStub));
			Assert.NotNull(maskedInstance);

			var concreteInstance = container.GetService(typeof(EmptyStub));
			Assert.NotNull(concreteInstance);
		}

		[Fact]
		public void Container_InstantiateWrongDependantService()
		{
			ServiceFactory container = new();
			container.Register(typeof(IDependantStub), typeof(DependantStub));
			Assert.Throws<MissingMethodException>(() => container.GetService(typeof(IDependantStub)));
		}

		[Fact]
		public void Container_InstantiateDependantService()
		{
			ServiceFactory container = new();
			container.Register(typeof(IDependantStub), typeof(DependantStub));
			container.Register(typeof(IEmptyStub), typeof(EmptyStub));

			Assert.NotNull(container.GetService(typeof(IDependantStub)));
		}

		[Fact]
		public void Container_ThrowOnCyclicalDependency()
		{
			ServiceFactory container = new();
			container.Register<CyclicalDependencyStub>
		}
	}
}
