using AInjection.XUnitTests.StubTypes;

namespace AInjection.XUnitTests
{
	public class ComponentRegistrationTests
	{
		/// <summary>
		/// Ensure that a component can be registered at all
		/// </summary>
		[Fact(DisplayName = "Component can be registered")]
		public void RegisterComponent()
		{
			ServiceFactory factory = new();
			factory.Register<EmptyStub>();

			Assert.True(factory.IsProvided(typeof(EmptyStub)));
		}

		/// <summary>
		/// Ensure that a component can be registered only as an abstract type and the concrete type is not recognized in the public api of the factory 
		/// </summary>
		[Fact(DisplayName = "Component implements abstract service only")]
		public void ComponentAsAbstract()
		{
			ServiceFactory factory = new();
			factory.Register<EmptyStub>(b => b.Provides<IEmptyStub>());

			Assert.True(factory.IsProvided(typeof(IEmptyStub)));
			Assert.False(factory.IsProvided(typeof(EmptyStub)));
		}

		/// <summary>
		/// Ensure that a new component registration can overwrite an existing service definition
		/// </summary>
		[Fact(DisplayName = "Components overwrite existing services")]
		public void ComponentOverwritesService()
		{
			ServiceFactory factory = new();

			factory.Register<EmptyStub>(b => b.Provides<IStub>().Provides<IEmptyStub>());
			Assert.NotNull(factory.GetService(typeof(IStub)));
			Assert.True(factory.GetService(typeof(IStub))!.GetType() == typeof(EmptyStub));
			
			factory.Register<DependantStub>(b => b.Provides<IStub>());
			Assert.NotNull(factory.GetService(typeof(IStub)));
			Assert.True(factory.GetService(typeof(IStub))!.GetType() == typeof(DependantStub));
		}

		/// <summary>
		/// Ensure a single component can implement many services
		/// </summary>
		[Fact(DisplayName = "Component can be many services")]
		public void ComponentAsManyServices()
		{
			ServiceFactory factory = new();
			factory.Register<EmptyStub>(b =>
			{
				b.Provides<EmptyStub>()
				 .Provides<IEmptyStub>()
				 .Provides<IStub>();
			});

			Assert.True(factory.IsProvided<IStub>());
			Assert.True(factory.IsProvided<IEmptyStub>());
			Assert.True(factory.IsProvided<EmptyStub>());
		}

		/// <summary>
		/// Ensure the <see cref="ServiceFactory"/> does not provide instances to types that cannot be cast to the service
		/// </summary>
		[Fact(DisplayName = "Component must be assignable to service")]
		public void RequireComponentImplementsService()
		{
			ServiceFactory factory = new();
			Assert.Throws<ArgumentException>(() => factory.Register<OrphanStub>(b => b.Provides<IStub>()));
			Assert.False(factory.IsProvided(typeof(IEmptyStub)));
		}

		/// <summary>
		/// Ensures that related (in the same tree but not the same branch) types are not accepted (in case there is an error in the internal type validation) 
		/// </summary>
		[Fact(DisplayName = "Component is directly related to the service type (not diverging from a common ancestor)")]
		public void RequireComponentDirectRelation()
		{
			ServiceFactory factory = new();
			Assert.Throws<ArgumentException>(() => factory.Register<DependantStub>(b => b.Provides<IEmptyStub>()));
			Assert.False(factory.IsProvided(typeof(IEmptyStub)));
		}
	}
}