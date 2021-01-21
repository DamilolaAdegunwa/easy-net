using EasyNet.CommonTests;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Uow;
using EasyNet.Mvc;
using EasyNet.Runtime.Session;
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
            AssertSpecifiedServiceTypeAndImplementationType<IIocResolver, AspNetCoreIocResolver>(services, ServiceLifetime.Scoped);
			AssertSpecifiedServiceTypeAndImplementationType<IHttpContextAccessor, HttpContextAccessor>(services, ServiceLifetime.Singleton);
			AssertSpecifiedServiceTypeAndImplementationType<EasyNetUowActionFilter, EasyNetUowActionFilter>(services, ServiceLifetime.Transient);
			AssertSpecifiedServiceTypeAndImplementationType<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>(services, ServiceLifetime.Scoped);
			AssertSpecifiedServiceTypeAndImplementationType<IUnitOfWorkManager, UnitOfWorkManager>(services, ServiceLifetime.Scoped);
			AssertSpecifiedServiceTypeAndImplementationType<IUnitOfWork, NullUnitOfWork>(services, ServiceLifetime.Transient);
			AssertSpecifiedServiceTypeAndImplementationType<IPrincipalAccessor, DefaultPrincipalAccessor>(services, ServiceLifetime.Singleton);
			AssertSpecifiedServiceTypeAndImplementationType<IEasyNetSession, ClaimsEasyNetSession>(services, ServiceLifetime.Scoped);
            AssertSpecifiedServiceTypeAndImplementationType<EasyNetUowActionFilter, EasyNetUowActionFilter>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<EasyNetExceptionFilter, EasyNetExceptionFilter>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<EasyNetResultFilter, EasyNetResultFilter>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<EasyNetPageFilter, EasyNetPageFilter>(services, ServiceLifetime.Transient);
			AssertSpecifiedServiceTypeAndImplementationType<IEasyNetExceptionHandler, EasyNetExceptionHandler>(services, ServiceLifetime.Transient);

			var mvcOptions = serviceProvider.GetRequiredService<IOptions<MvcOptions>>().Value;
			Assert.Contains(mvcOptions.Filters, p => ((ServiceFilterAttribute)p).ServiceType == typeof(EasyNetUowActionFilter));
            Assert.Contains(mvcOptions.Filters, p => ((ServiceFilterAttribute)p).ServiceType == typeof(EasyNetExceptionFilter));
            Assert.Contains(mvcOptions.Filters, p => ((ServiceFilterAttribute)p).ServiceType == typeof(EasyNetResultFilter));
            Assert.Contains(mvcOptions.Filters, p => ((ServiceFilterAttribute)p).ServiceType == typeof(EasyNetPageFilter));
		}
	}
}
