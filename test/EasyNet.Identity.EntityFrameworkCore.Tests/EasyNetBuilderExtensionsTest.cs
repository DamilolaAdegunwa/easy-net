using System.Linq;
using EasyNet.CommonTests;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Repositories;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.DependencyInjection;
using EasyNet.EntityFrameworkCore.Repositories;
using EasyNet.EntityFrameworkCore.Uow;
using EasyNet.Identity.EntityFrameworkCore.DependencyInjection;
using EasyNet.Identity.EntityFrameworkCore.Domain;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using EasyNet.Identity.EntityFrameworkCore.Tests.DbContext;
using EasyNet.Identity.EntityFrameworkCore.Tests.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyNet.Identity.EntityFrameworkCore.Tests
{
    public class EasyNetBuilderExtensionsTest : DependencyInjectionTest
    {
        [Fact]
        public void TestAddIdentityCore()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(CommonTest.GetHostingEnvironment());

            // Act
            services
                .AddEasyNet()
                .AddEfCore<IdentityContext>(options =>
                {
                    options.UseSqlite("TestConnectionString");
                }, true)
                .AddIdentityCore<User, IdentityContext>();

            // Assert
            AssertSpecifiedServiceTypeAndImplementationType<IdentityContext, IdentityContext>(services, ServiceLifetime.Scoped);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<User>, EfCoreRepositoryBase<IdentityContext, User>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<Role>, EfCoreRepositoryBase<IdentityContext, Role>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<EasyNetUserClaim<int>>, EfCoreRepositoryBase<IdentityContext, EasyNetUserClaim<int>>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<EasyNetRoleClaim<int>>, EfCoreRepositoryBase<IdentityContext, EasyNetRoleClaim<int>>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<EasyNetUserRole<int>>, EfCoreRepositoryBase<IdentityContext, EasyNetUserRole<int>>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<EasyNetUserLogin<int>>, EfCoreRepositoryBase<IdentityContext, EasyNetUserLogin<int>>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<IEfCoreRepository<EasyNetUserToken<int>>, EfCoreRepositoryBase<IdentityContext, EasyNetUserToken<int>>>(services, ServiceLifetime.Transient);
            AssertSpecifiedServiceTypeAndImplementationType<SignInManager<User>, EasyNetSignInManager<User, int>>(services, ServiceLifetime.Scoped);
            AssertSpecifiedServiceTypeAndImplementationType<IEasyNetGeneralSignInManager, EasyNetSignInManager<User, int>>(services, ServiceLifetime.Scoped);
            AssertSpecifiedServiceTypeAndImplementationType<UserManager<User>, EasyNetUserManager<User, int>>(services, ServiceLifetime.Scoped, 2);
        }
    }
}
