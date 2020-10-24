using EasyNet.CommonTests;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Uow;
using EasyNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace EasyNet.Tests
{
	public class EasyNetServiceCollectionExtensionsTest : DependencyInjectionTest
	{
		[Fact]
		public void TestAddEasyNet()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddSingleton(CommonTest.GetHostingEnvironment());

			// Act
			services.AddEasyNet();
			var serviceProvider = services.BuildServiceProvider();

			// Assert
			AssertSpecifiedServiceTypeAndImplementationType<IHttpContextAccessor, HttpContextAccessor>(services, ServiceLifetime.Singleton);
			AssertSpecifiedServiceTypeAndImplementationType<EasyNetUowActionFilter, EasyNetUowActionFilter>(services, ServiceLifetime.Transient);
			AssertSpecifiedServiceTypeAndImplementationType<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>(services, ServiceLifetime.Scoped);
			AssertSpecifiedServiceTypeAndImplementationType<IUnitOfWorkManager, UnitOfWorkManager>(services, ServiceLifetime.Scoped);
			AssertSpecifiedServiceTypeAndImplementationType<IUnitOfWork, NullUnitOfWork>(services, ServiceLifetime.Transient);

			var mvcOptions = serviceProvider.GetService<IOptions<MvcOptions>>().Value;
			Assert.Contains(mvcOptions.Filters, p => ((ServiceFilterAttribute)p).ServiceType == typeof(EasyNetUowActionFilter));
		}
	}
}
