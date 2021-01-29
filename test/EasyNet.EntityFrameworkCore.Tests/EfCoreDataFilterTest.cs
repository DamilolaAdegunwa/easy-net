using System;
using System.Data.Common;
using System.Threading.Tasks;
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
using Xunit;

namespace EasyNet.EntityFrameworkCore.Tests
{
    public class EfCoreDataFilterTest
    {
        private readonly IServiceProvider _serviceProvider;

        public EfCoreDataFilterTest()
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

            InitData();
        }

        [Fact]
        public void TestSoftDeleteFilter()
        {
            // Arrange
            using var uow = BeginUow();
            var deletionAuditedRepo = GetRepository<TestDeletionAudited>();

            #region Enable SoftDelete

            // Act
            var count1 = deletionAuditedRepo.Count();

            // Assert
            Assert.Equal(5, count1);

            #endregion

            #region Disable SoftDelete

            using (((IActiveUnitOfWork)uow).DisableFilter(EasyNetDataFilters.SoftDelete))
            {
                // Act
                var count2 = deletionAuditedRepo.Count();

                // Assert
                Assert.Equal(6, count2);
            }

            #endregion

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestSoftDeleteFilterAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var deletionAuditedRepo = GetRepository<TestDeletionAudited>();

            #region Enable SoftDelete

            // Act
            var count1 = await deletionAuditedRepo.CountAsync();

            // Assert
            Assert.Equal(5, count1);

            #endregion

            #region Disable SoftDelete

            using (((IActiveUnitOfWork)uow).DisableFilter(EasyNetDataFilters.SoftDelete))
            {
                // Act
                var count2 = await deletionAuditedRepo.CountAsync();

                // Assert
                Assert.Equal(6, count2);
            }

            #endregion

            // Complete uow
            await uow.CompleteAsync();
        }

        private DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        private void InitData()
        {
            var context = _serviceProvider.GetService<EfCoreContext>();
            context.Database.EnsureCreated();

            // Insert default roles.
            context.Roles.Add(new Role { Name = "Admin" });
            context.SaveChanges();
            context.Roles.Add(new Role { Name = "User" });
            context.SaveChanges();

            // Insert default users.
            context.Users.Add(new User { Name = "User1", Status = Status.Active, RoleId = 1 });
            context.SaveChanges();
            context.Users.Add(new User { Name = "User2", Status = Status.Active, RoleId = 2 });
            context.SaveChanges();
            context.Users.Add(new User { Name = "User3", Status = Status.Inactive, RoleId = 2 });
            context.SaveChanges();
            context.Users.Add(new User { Name = "User4", Status = Status.Active, RoleId = 2 });
            context.SaveChanges();

            // Insert default test modification audited records.
            context.TestModificationAudited.Add(new TestModificationAudited { Name = "Update1" });
            context.SaveChanges();
            context.TestModificationAudited.Add(new TestModificationAudited { Name = "Update2" });
            context.SaveChanges();
            context.TestModificationAudited.Add(new TestModificationAudited { Name = "Update3" });
            context.SaveChanges();

            // Insert default test deletion audited records.
            context.TestDeletionAudited.Add(new TestDeletionAudited { IsActive = true });
            context.SaveChanges();
            context.TestDeletionAudited.Add(new TestDeletionAudited { IsActive = false });
            context.SaveChanges();
            context.TestDeletionAudited.Add(new TestDeletionAudited { IsActive = true });
            context.SaveChanges();
            context.TestDeletionAudited.Add(new TestDeletionAudited { IsActive = true });
            context.SaveChanges();
            context.TestDeletionAudited.Add(new TestDeletionAudited { IsActive = false });
            context.SaveChanges();
            context.TestDeletionAudited.Add(new TestDeletionAudited { IsActive = false, IsDeleted = true });
            context.SaveChanges();

            // Clear all change trackers
            foreach (var entry in context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

        public IUnitOfWorkCompleteHandle BeginUow()
        {
            return _serviceProvider.GetService<IUnitOfWorkManager>().Begin();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity<int>
        {
            return _serviceProvider.GetService<IRepository<TEntity>>();
        }

        public IRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            return _serviceProvider.GetService<IRepository<TEntity, TPrimaryKey>>();
        }
    }
}
