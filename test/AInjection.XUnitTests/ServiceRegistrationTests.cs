using AInjection.XUnitTests.StubTypes;

namespace AInjection.XUnitTests
{
	public class ServiceRegistrationTests
	{
		[Fact]
		public void Container_OverwriteServiceRegistration()
		{
			IoCContainer container = new();
		}
		[Fact]
		public void Container_MultiServiceRegistration()
		{
			IoCContainer container = new();
			container.Register(typeof(IEmptyStub), typeof(EmptyStub));
			container.Register(typeof(EmptyStub), typeof(EmptyStub));
			container.Register(typeof(IDependantStub), typeof(DependantStub));

			Assert.True(container.Contains(typeof(IEmptyStub)));
			Assert.True(container.Contains(typeof(EmptyStub)));
			Assert.True(container.Contains(typeof(IDependantStub)));
			Assert.False(container.Contains(typeof(DependantStub)));
		}
		[Fact]
		public void Container_AbstractSingleServiceRegistration()
		{
			IoCContainer container = new();
			container.Register(typeof(IEmptyStub), typeof(EmptyStub));

			Assert.True(container.Contains(typeof(IEmptyStub)));
			Assert.False(container.Contains(typeof(EmptyStub)));
		}
		[Fact]
		public void Container_SingleServiceRegistration()
		{
			IoCContainer container = new();
			container.Register(typeof(EmptyStub), typeof(EmptyStub));

			Assert.True(container.Contains(typeof(EmptyStub)));
		}
		[Fact]
		public void Container_RegisterUnrelatedServices()
		{
			IoCContainer container = new();
			Assert.Throws<ArgumentException>(() => container.Register(typeof(IEmptyStub), typeof(OrphanStub)));
			Assert.False(container.Contains(typeof(IEmptyStub)));
		}
	}
}