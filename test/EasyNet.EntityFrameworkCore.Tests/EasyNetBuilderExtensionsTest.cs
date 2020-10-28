using System.Linq;
using EasyNet.CommonTests;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Repositories;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.DependencyInjection;
using EasyNet.EntityFrameworkCore.Repositories;
using EasyNet.EntityFrameworkCore.Tests.DbContext;
using EasyNet.EntityFrameworkCore.Tests.Entities;
using EasyNet.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyNet.EntityFrameworkCore.Tests
{
    public class EasyNetBuilderExtensionsTest : DependencyInjectionTest
    {
        [Fact]
        public void TestAddEfCoreAsMainOrmTechnology()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(CommonTest.GetHostingEnvironment());

            // Act
            services
                .AddEasyNet()
                .AddEfCore<EfCoreContext>(options =>
                {
                    options.UseSqlite("TestConnectionString");
                }, true);

            var serviceProvider = services.BuildServiceProvider();
            var dbContextOptions = serviceProvider.GetService<DbContextOptions<EfCoreContext>>();
            var sqlServerOptions = dbContextOptions.Extensions.SingleOrDefault(p => p.GetType() == typeof(SqliteOptionsExtension));

            // Assert
            AssertSpecifiedServiceTypeAndImplementationType<EfCoreContext, EfCoreContext>(services, ServiceLifetime.Scoped);
            AssertSpecifiedServiceTypeAndImplementationType<IUnitOfWork, EfCoreUnitOfWork>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IDbContextProvider<EfCoreContext>, UnitOfWorkDbContextProvider<EfCoreContext>>(services, ServiceLifetime.Scoped);
            Assert.NotNull(sqlServerOptions);
            Assert.Equal("TestConnectionString", ((RelationalOptionsExtension)sqlServerOptions).ConnectionString);
            AssertSpecifiedServiceTypeAndImplementationType<IRepository<User, long>, EfCoreRepositoryBase<EfCoreContext, User, long>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IRepository<Role, int>, EfCoreRepositoryBase<EfCoreContext, Role, int>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IRepository<Role>, EfCoreRepositoryBase<EfCoreContext, Role>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<User, long>, EfCoreRepositoryBase<EfCoreContext, User, long>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<Role, int>, EfCoreRepositoryBase<EfCoreContext, Role, int>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<Role>, EfCoreRepositoryBase<EfCoreContext, Role>>(services, ServiceLifetime.Transient);
        }

        [Fact]
        public void TestAddEfCoreNotMainOrmTechnology()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(CommonTest.GetHostingEnvironment());

            // Act
            services
                .AddEasyNet()
                .AddEfCore<EfCoreContext>(options =>
                {
                    options.UseSqlite("TestConnectionString");
                }, false);

            var serviceProvider = services.BuildServiceProvider();
            var dbContextOptions = serviceProvider.GetService<DbContextOptions<EfCoreContext>>();
            var sqlServerOptions = dbContextOptions.Extensions.SingleOrDefault(p => p.GetType() == typeof(SqliteOptionsExtension));

            // Assert
            AssertSpecifiedServiceTypeAndImplementationType<EfCoreContext, EfCoreContext>(services, ServiceLifetime.Scoped);
            AssertSpecifiedServiceTypeAndImplementationType<IUnitOfWork, EfCoreUnitOfWork>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IDbContextProvider<EfCoreContext>, UnitOfWorkDbContextProvider<EfCoreContext>>(services, ServiceLifetime.Scoped);
            Assert.NotNull(sqlServerOptions);
            Assert.Equal("TestConnectionString", ((RelationalOptionsExtension)sqlServerOptions).ConnectionString);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<User, long>, EfCoreRepositoryBase<EfCoreContext, User, long>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<Role, int>, EfCoreRepositoryBase<EfCoreContext, Role, int>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<Role>, EfCoreRepositoryBase<EfCoreContext, Role>>(services, ServiceLifetime.Transient);
            Assert.Equal(0, services.Count(p => p.ServiceType == typeof(IRepository<User, long>)));
            Assert.Equal(0, services.Count(p => p.ServiceType == typeof(IRepository<Role, int>)));
            Assert.Equal(0, services.Count(p => p.ServiceType == typeof(IRepository<Role>)));
        }
    }
}
