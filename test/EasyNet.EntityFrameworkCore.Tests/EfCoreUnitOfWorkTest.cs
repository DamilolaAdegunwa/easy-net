//using System.Threading.Tasks;
//using EasyNet.DependencyInjection;
//using EasyNet.Domain.Entities;
//using EasyNet.Domain.Repositories;
//using EasyNet.Domain.Uow;
//using EasyNet.EntityFrameworkCore.Repositories;
//using EasyNet.EntityFrameworkCore.Tests.DbContext;
//using EasyNet.EntityFrameworkCore.Tests.Entities;
//using EasyNet.EntityFrameworkCore.Tests.Fixture;
//using EasyNet.EntityFrameworkCore.Uow;
//using Moq;
//using Xunit;

//namespace EasyNet.EntityFrameworkCore.Tests
//{
//    public class EfCoreUnitOfWorkTest : IClassFixture<MsSqlDatabaseFixtureForUow>
//    {
//        private readonly MsSqlDatabaseFixture _msFixture;

//        public EfCoreUnitOfWorkTest(MsSqlDatabaseFixtureForUow msFixture)
//        {
//            _msFixture = msFixture;
//        }

//        //[Fact]
//        //public async Task Test()
//        //{
//        //    // 初始化UnitOfWorkManager
//        //    var unitOfWorkManager = new UnitOfWorkManager(GetIocResolver(), GetCurrentUnitOfWorkProvider(), new UnitOfWorkDefaultOptions());

//        //    using (var uow = unitOfWorkManager.Begin())
//        //    {
//        //        var vehicleRepo = GetRepository<Vehicle>();

//        //        var vehicles = await vehicleRepo.GetAllListAsync();
//        //        Assert.Equal(10, vehicles.Count);

//        //        await uow.CompleteAsync();
//        //    }
//        //}

//        #region Helper

//        private ICurrentUnitOfWorkProvider GetCurrentUnitOfWorkProvider()
//        {
//            return new AsyncLocalCurrentUnitOfWorkProvider();
//        }

//        private IIocResolver GetIocResolver()
//        {
//            var iocResolverMock = new Mock<IIocResolver>();
//            iocResolverMock.Setup(p => p.Resolve<IUnitOfWork>()).Returns(() =>
//            {
//                return new EfCoreUnitOfWork(GetConnectionStringResolver(), GetDbContextResolver(), new UnitOfWorkDefaultOptions());
//            });

//            return iocResolverMock.Object;
//        }

//        private IConnectionStringResolver GetConnectionStringResolver()
//        {
//            var connectionStringResolverMock = new Moq.Mock<IConnectionStringResolver>();
//            connectionStringResolverMock.Setup(p => p.GetNameOrConnectionString()).Returns(_msFixture.ConnectionString);

//            return connectionStringResolverMock.Object;
//        }

//        private IDbContextProvider<UnitTestContext> GetDbContextProvider()
//        {
//            return new UnitOfWorkDbContextProvider<UnitTestContext>(GetCurrentUnitOfWorkProvider());
//        }

//        private IDbContextResolver GetDbContextResolver()
//        {
//            var dbContextResolverMock = new Moq.Mock<IDbContextResolver>();
//            dbContextResolverMock.Setup(p => p.Resolve<UnitTestContext>(It.IsRegex(""), null)).Returns(_msFixture.GetDbContext());

//            return dbContextResolverMock.Object;
//        }

//        private IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
//        {
//            return new EfCoreRepositoryBase<UnitTestContext, TEntity>(GetDbContextProvider());
//        }


//        #endregion
//    }
//}
