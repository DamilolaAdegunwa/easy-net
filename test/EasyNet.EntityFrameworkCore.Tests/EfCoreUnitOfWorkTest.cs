using System;
using System.Data.Common;
using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.DependencyInjection;
using EasyNet.EntityFrameworkCore.Tests.DbContext;
using EasyNet.EntityFrameworkCore.Uow;
using EasyNet.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyNet.EntityFrameworkCore.Tests
{
    public class EfCoreUnitOfWorkTest
    {
        private readonly IServiceProvider _serviceProvider;

        public EfCoreUnitOfWorkTest()
        {
            var services = new ServiceCollection();

            services
                .AddEasyNet()
                .AddSession<TestSession>()
                .AddEfCore<EfCoreContext>(options =>
                {
                    options.UseSqlite(CreateInMemoryDatabase());
                });

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void TestBegin()
        {
            #region Without DbContext and Transaction

            // Arrange
            var uow1 = GetEfCoreUnitOfWork();

            // Act
            uow1.Begin(new UnitOfWorkOptions());
            uow1.Complete();

            #endregion

            #region With DbContext but Transaction

            // Arrange
            var uow2 = GetEfCoreUnitOfWork();

            // Act
            uow2.Begin(new UnitOfWorkOptions { IsTransactional = false });
            ((EfCoreUnitOfWork)uow2).GetOrCreateDbContext<EfCoreContext>();
            uow2.Complete();

            // Assert
            Assert.NotNull(uow2.GetPrivateProperty<Microsoft.EntityFrameworkCore.DbContext>("ActiveDbContext"));
            Assert.Null(uow2.GetPrivateProperty<IDbContextTransaction>("ActiveTransaction"));

            #endregion

            #region With DbContext and Transaction

            // Arrange
            var uow3 = GetEfCoreUnitOfWork();

            // Act
            uow3.Begin(new UnitOfWorkOptions { IsTransactional = true });
            ((EfCoreUnitOfWork)uow3).GetOrCreateDbContext<EfCoreContext>();
            uow3.Complete();

            // Assert
            Assert.NotNull(uow3.GetPrivateProperty<Microsoft.EntityFrameworkCore.DbContext>("ActiveDbContext"));
            Assert.NotNull(uow3.GetPrivateProperty<IDbContextTransaction>("ActiveTransaction"));

            #endregion
        }

        [Fact]
        public async Task TestBeginAsync()
        {
            #region Without DbContext and Transaction

            // Arrange
            var uow1 = GetEfCoreUnitOfWork();

            // Act
            uow1.Begin(new UnitOfWorkOptions());
            await uow1.CompleteAsync();

            #endregion

            #region With DbContext but Transaction

            // Arrange
            var uow2 = GetEfCoreUnitOfWork();

            // Act
            uow2.Begin(new UnitOfWorkOptions { IsTransactional = false });
            ((EfCoreUnitOfWork)uow2).GetOrCreateDbContext<EfCoreContext>();
            await uow2.CompleteAsync();

            // Assert
            Assert.NotNull(uow2.GetPrivateProperty<Microsoft.EntityFrameworkCore.DbContext>("ActiveDbContext"));
            Assert.Null(uow2.GetPrivateProperty<IDbContextTransaction>("ActiveTransaction"));

            #endregion

            #region With DbContext and Transaction

            // Arrange
            var uow3 = GetEfCoreUnitOfWork();

            // Act
            uow3.Begin(new UnitOfWorkOptions { IsTransactional = true });
            ((EfCoreUnitOfWork)uow3).GetOrCreateDbContext<EfCoreContext>();
            await uow3.CompleteAsync();

            // Assert
            Assert.NotNull(uow3.GetPrivateProperty<Microsoft.EntityFrameworkCore.DbContext>("ActiveDbContext"));
            Assert.NotNull(uow3.GetPrivateProperty<IDbContextTransaction>("ActiveTransaction"));

            #endregion
        }

        private IUnitOfWork GetEfCoreUnitOfWork()
        {
            return _serviceProvider.GetService<IUnitOfWork>();
        }

        private DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }
    }
}
