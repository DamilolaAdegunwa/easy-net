using System;
using System.Threading.Tasks;
using System.Transactions;
using EasyNet.CommonTests;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Uow;
using EasyNet.Mvc;
using EasyNet.Runtime.Session;
using EasyNet.Tests.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace EasyNet.Tests
{
    public class EasyNetBuilderExtensionsTest : DependencyInjectionTest
    {
        [Fact]
        public void TestConfigureUnitOfWorkDefaultOptions()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(CommonTest.GetHostingEnvironment());

            // Act
            services
                .AddEasyNet()
                .ConfigureUnitOfWorkDefaultOptions(options =>
                {
                    options.IsTransactional = false;
                    options.Scope = TransactionScopeOption.Suppress;
                    options.Timeout = TimeSpan.Zero;
                    options.IsolationLevel = IsolationLevel.RepeatableRead;
                });

            var serviceProvider = services.BuildServiceProvider();
            var defaultOptions = serviceProvider.GetRequiredService<IOptions<UnitOfWorkDefaultOptions>>().Value;

            // Assert
            Assert.Equal(false, defaultOptions.IsTransactional);
            Assert.Equal(TransactionScopeOption.Suppress, defaultOptions.Scope);
            Assert.Equal(TimeSpan.Zero, defaultOptions.Timeout);
            Assert.Equal(IsolationLevel.RepeatableRead, defaultOptions.IsolationLevel);
        }

        [Fact]
        public void TestAddSession()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(CommonTest.GetHostingEnvironment());

            // Act
            services
                .AddEasyNet()
                .AddSession<TestSession>();

            var serviceProvider = services.BuildServiceProvider();
            var session = serviceProvider.GetRequiredService<IEasyNetSession>();

            // Assert
            AssertSpecifiedServiceTypeAndImplementationType<IEasyNetSession, TestSession>(services, ServiceLifetime.Scoped);
            Assert.Equal("1", session.UserId);
            Assert.Equal("Test", session.UserName);
            Assert.Equal("Admin", session.Role);
        }

        [Fact]
        public void TestAddIocResolver()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(CommonTest.GetHostingEnvironment());

            // Act
            services
                .AddEasyNet()
                .AddIocResolver<TestIocResolver>();

            var serviceProvider = services.BuildServiceProvider();
            var iocResolver = serviceProvider.GetRequiredService<IIocResolver>();

            // Assert
            AssertSpecifiedServiceTypeAndImplementationType<IIocResolver, TestIocResolver>(services, ServiceLifetime.Scoped);
            Assert.Equal(typeof(TestIocResolver), iocResolver.GetType());
        }

        [Fact]
        public void TestAddExceptionHandler()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(CommonTest.GetHostingEnvironment());

            // Act
            services
                .AddEasyNet()
                .AddExceptionHandler<TestExceptionHandler>();

            var serviceProvider = services.BuildServiceProvider();
            var exceptionHandler = serviceProvider.GetRequiredService<IEasyNetExceptionHandler>();

            // Assert
            AssertSpecifiedServiceTypeAndImplementationType<IEasyNetExceptionHandler, TestExceptionHandler>(services, ServiceLifetime.Transient);
            Assert.Equal(typeof(TestExceptionHandler), exceptionHandler.GetType());
        }
    }

    public class TestIocResolver : IIocResolver
    {
        public T GetService<T>(bool required = true)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType, bool required = true)
        {
            throw new NotImplementedException();
        }

        public IScopeIocResolver CreateScope()
        {
            throw new NotImplementedException();
        }
    }

    public class TestExceptionHandler : IEasyNetExceptionHandler
    {
        public object WrapException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task Handle(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
