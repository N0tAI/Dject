using AInjection.XUnitTests.StubTypes;

namespace AInjection.XUnitTests
{
	public class ServiceInstantiationTests
	{
		/// <summary>
		/// Can instantiate a service with no dependencies
		/// </summary>
		/// <remarks>Checks against both abstract and concrete classes</remarks>
		[Fact(DisplayName = "Create service with no dependencies")]
		public void InstantiateNoDependencies()
		{
			ServiceFactory factory = new();
			factory.Register<EmptyStub>(b =>
			{
				b.Provides<EmptyStub>()
				 .Provides<IEmptyStub>();
			});

			var maskedInstance = factory.GetService(typeof(IEmptyStub));
			Assert.NotNull(maskedInstance);

			var concreteInstance = factory.GetService(typeof(EmptyStub));
			Assert.NotNull(concreteInstance);
		}

		/// <summary>
		/// Ensure that a concrete type cannot be directly resolved if not specified as provided
		/// </summary>
		[Fact(DisplayName = "Instantiation respects registered services")]
		public void RefuseToInstantiateUnprovidedServices()
		{
			ServiceFactory factory = new();
			factory.Register<EmptyStub>(b => b.Provides<IEmptyStub>());

			Assert.True(factory.IsProvided<IEmptyStub>());
			var maskedInstance = factory.GetService(typeof(IEmptyStub));
			Assert.NotNull(maskedInstance);

			Assert.False(factory.IsProvided<EmptyStub>());
			var concreteInstance = factory.GetService(typeof(EmptyStub));
			Assert.Null(concreteInstance);
		}

		/// <summary>
		/// Ensure that a <see cref="MissingMethodException"/> is thrown if missing dependencies
		/// </summary>
		[Fact(DisplayName = "Throw on creating service with missing dependencies")]
		public void ThrowOnMissingDependencies()
		{
			ServiceFactory factory = new();
			factory.Register<DependantStub>(b => b.Provides<IDependantStub>());
			Assert.Throws<MissingMethodException>(() => factory.GetService(typeof(IDependantStub)));
		}

		/// <summary>
		/// Ensure that a service with the required dependencies can be created
		/// </summary>
		[Fact(DisplayName = "Create service with correct dependencies")]
		public void InstantiateWithAllDependencies()
		{
			ServiceFactory factory = new();
			factory.Register<DependantStub>(b => b.Provides<IDependantStub>());
			factory.Register<EmptyStub>(b => b.Provides<IEmptyStub>());

			Assert.NotNull(factory.GetService(typeof(IDependantStub)));
		}

		[Fact(DisplayName = "Throw on circular dependency")]
		public void ThrowOnServiceCyclicalDependency()
		{
			ServiceFactory factory = new();
			factory.Register<CyclicalDependencyStub>();
		}
	}
}
