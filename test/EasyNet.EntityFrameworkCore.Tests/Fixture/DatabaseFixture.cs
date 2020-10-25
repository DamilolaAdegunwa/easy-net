using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.DependencyInjection;
using EasyNet.EntityFrameworkCore.Tests.DbContext;
using EasyNet.EntityFrameworkCore.Tests.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.EntityFrameworkCore.Tests.Fixture
{
    public class DatabaseFixture : IDisposable
    {
        private IUnitOfWorkCompleteHandle _uow;
        private IUnitOfWorkManager _uowManager;

        public DatabaseFixture()
        {
            var services = new ServiceCollection();

            services
                .AddEasyNet()
                .AddEfCore<EfCoreContext>(options =>
                {
                    options.UseSqlite(CreateInMemoryDatabase());
                });

            ServiceProvider = services.BuildServiceProvider();

            InitData();
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public IServiceProvider ServiceProvider { get; }

        private void InitData()
        {
            _uowManager = ServiceProvider.GetService<IUnitOfWorkManager>();
            _uow = _uowManager.Begin(new UnitOfWorkOptions { IsTransactional = false });

            //var current = ServiceProvider.GetService<ICurrentUnitOfWorkProvider>();
            //var abc = ServiceProvider.GetServices<IRepository<User, long>>();
            //var context = ServiceProvider.GetService<EfCoreContext>();
            //context.Database.EnsureCreated();
            //context.Users.AddRange(new List<User>
            //    {
            //        new User{Name = "Name1", Status = Status.Active},
            //        new User{Name = "Name2", Status = Status.Active},
            //        new User{Name = "Name3", Status = Status.Inactive},
            //        new User{Name = "Name4", Status = Status.Active}
            //    });
            //context.SaveChanges();

        }

        public IUnitOfWorkCompleteHandle BeginUow()
        {
            return ServiceProvider.GetService<IUnitOfWorkManager>()
                .Begin(new UnitOfWorkOptions { IsTransactional = false });
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity<int>
        {
            return ServiceProvider.GetService<IRepository<TEntity>>();
        }

        public IRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            return ServiceProvider.GetService<IRepository<TEntity, TPrimaryKey>>();
        }

        public void Dispose()
        {
            _uow.Complete();
            _uow.Dispose();
        }
    }
}
