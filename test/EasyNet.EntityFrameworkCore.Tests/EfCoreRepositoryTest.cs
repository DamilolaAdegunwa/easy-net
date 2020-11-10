using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.DependencyInjection;
using EasyNet.EntityFrameworkCore.Repositories;
using EasyNet.EntityFrameworkCore.Tests.DbContext;
using EasyNet.EntityFrameworkCore.Tests.Entities;
using EasyNet.Extensions;
using EasyNet.Runtime.Session;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyNet.EntityFrameworkCore.Tests
{
    public class EfCoreRepositoryTest
    {
        private readonly IServiceProvider _serviceProvider;

        public EfCoreRepositoryTest()
        {
            var services = new ServiceCollection();

            services
                .AddEasyNet()
                .AddSession<TestSession>()
                .AddEfCore<EfCoreContext>(options =>
                {
                    options.UseSqlite(CreateInMemoryDatabase());
                }, true);

            _serviceProvider = services.BuildServiceProvider();

            InitData();
        }

        #region Get

        [Fact]
        public void TestGet()
        {
            // Arrange
            using var uow = BeginUow();
            var roleRepo = GetRepository<Role>();

            // Act
            var role = roleRepo.Get(2);

            // Assert
            Assert.Equal("User", role.Name);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestGetAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var roleRepo = GetRepository<Role>();

            // Act
            var role = await roleRepo.GetAsync(2);

            // Assert
            Assert.Equal("User", role.Name);

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region GetAllList

        [Fact]
        public void TestGetAllList()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var users = userRepo.GetAllList();

            // Assert
            Assert.Equal(4, users.Count);
            Assert.Equal("User2", users[1].Name);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public void TestGetAllListByPredicate()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var users = userRepo.GetAllList(p => p.Status == Status.Active);

            // Assert
            Assert.Equal(3, users.Count);
            Assert.Equal("User4", users[2].Name);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestGetAllListAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var users = await userRepo.GetAllListAsync();

            // Assert
            Assert.Equal(4, users.Count);
            Assert.Equal("User2", users[1].Name);

            // Complete uow
            await uow.CompleteAsync();
        }

        [Fact]
        public async Task TestGetAllListByPredicateAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var users = await userRepo.GetAllListAsync(p => p.Status == Status.Active);

            // Assert
            Assert.Equal(3, users.Count);
            Assert.Equal("User4", users[2].Name);

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region Signle

        [Fact]
        public void TestSingle()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var user = userRepo.Single(p => p.Name == "User4");

            // Assert
            Assert.NotNull(user);
            Assert.Throws<InvalidOperationException>(
                () => userRepo.Single(p => p.Name == "User0"));

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestSingleAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var user = await userRepo.SingleAsync(p => p.Name == "User4");

            // Assert
            Assert.NotNull(user);
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await userRepo.SingleAsync(p => p.Name == "User0"));

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region SignleOrDefault

        [Fact]
        public void TestSingleOrDefault()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var user4 = userRepo.SingleOrDefault(p => p.Name == "User4");
            var user0 = userRepo.SingleOrDefault(p => p.Name == "User0");

            // Assert
            Assert.NotNull(user4);
            Assert.Null(user0);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestSingleOrDefaultAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var user4 = await userRepo.SingleOrDefaultAsync(p => p.Name == "User4");
            var user0 = await userRepo.SingleOrDefaultAsync(p => p.Name == "User0");

            // Assert
            Assert.NotNull(user4);
            Assert.Null(user0);

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region First

        [Fact]
        public void TestFirst()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var user = userRepo.First();
            var inactiveUser = userRepo.First(p => p.Status == Status.Inactive);

            // Assert
            Assert.NotNull(user);
            Assert.NotNull(inactiveUser);
            Assert.Equal("User1", user.Name);
            Assert.Equal("User3", inactiveUser.Name);
            Assert.Throws<InvalidOperationException>(() => userRepo.First(p => p.Name == "User0"));

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestFirstAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var user = await userRepo.FirstAsync();
            var inactiveUser = await userRepo.FirstAsync(p => p.Status == Status.Inactive);

            // Assert
            Assert.NotNull(user);
            Assert.NotNull(inactiveUser);
            Assert.Equal("User1", user.Name);
            Assert.Equal("User3", inactiveUser.Name);
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await userRepo.FirstAsync(p => p.Name == "User0"));

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region FirstOrDefault

        [Fact]
        public void TestFirstOrDefault()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var user = userRepo.FirstOrDefault();
            var inactiveUser = userRepo.FirstOrDefault(p => p.Status == Status.Inactive);
            var nullUser = userRepo.FirstOrDefault(p => p.Name == "User0");

            // Assert
            Assert.NotNull(user);
            Assert.NotNull(inactiveUser);
            Assert.Null(nullUser);
            Assert.Equal("User1", user.Name);
            Assert.Equal("User3", inactiveUser.Name);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestFirstOrDefaultAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var user = await userRepo.FirstOrDefaultAsync();
            var inactiveUser = await userRepo.FirstOrDefaultAsync(p => p.Status == Status.Inactive);
            var nullUser = await userRepo.FirstOrDefaultAsync(p => p.Name == "User0");

            // Assert
            Assert.NotNull(user);
            Assert.NotNull(inactiveUser);
            Assert.Null(nullUser);
            Assert.Equal("User1", user.Name);
            Assert.Equal("User3", inactiveUser.Name);

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region Count

        [Fact]
        public void TestCount()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var count = userRepo.Count();
            var activeCount = userRepo.Count(p => p.Status == Status.Active);
            var zeroCount = userRepo.Count(p => p.Name == "Zero");

            // Assert
            Assert.Equal(4, count);
            Assert.Equal(3, activeCount);
            Assert.Equal(0, zeroCount);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestCountAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var count = await userRepo.CountAsync();
            var activeCount = await userRepo.CountAsync(p => p.Status == Status.Active);
            var zeroCount = await userRepo.CountAsync(p => p.Name == "Zero");

            // Assert
            Assert.Equal(4, count);
            Assert.Equal(3, activeCount);
            Assert.Equal(0, zeroCount);

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region LongCount

        [Fact]
        public void TestLongCount()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var count = userRepo.LongCount();
            var activeCount = userRepo.LongCount(p => p.Status == Status.Active);
            var zeroCount = userRepo.Count(p => p.Name == "Zero");

            // Assert
            Assert.Equal(4, count);
            Assert.Equal(3, activeCount);
            Assert.Equal(0, zeroCount);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestLongCountAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var count = await userRepo.LongCountAsync();
            var activeCount = await userRepo.LongCountAsync(p => p.Status == Status.Active);
            var zeroCount = await userRepo.CountAsync(p => p.Name == "Zero");

            // Assert
            Assert.Equal(4, count);
            Assert.Equal(3, activeCount);
            Assert.Equal(0, zeroCount);

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region Any

        [Fact]
        public void TestAny()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var activeAny = userRepo.Any(p => p.Status == Status.Active);
            var zeroAny = userRepo.Any(p => p.Name == "Zero");

            // Assert
            Assert.True(activeAny);
            Assert.False(zeroAny);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestAnyAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var activeAny = await userRepo.AnyAsync(p => p.Status == Status.Active);
            var zeroAny = await userRepo.AnyAsync(p => p.Name == "Zero");

            // Assert
            Assert.True(activeAny);
            Assert.False(zeroAny);

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region Insert

        [Fact]
        public void TestInsert()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();
            var roleRepo = GetRepository<Role>();
            var creationAuditedRepo = GetRepository<TestCreationAudited>();

            #region Insert but not SaveChanges

            // Act
            var user5 = new User
            {
                Name = "User5",
                RoleId = 1
            };
            userRepo.Insert(user5);

            var role3 = new Role
            {
                Name = "Role3",
            };
            roleRepo.Insert(role3);

            var creationAudited1 = new TestCreationAudited();
            creationAuditedRepo.Insert(creationAudited1);

            // Assert
            Assert.Equal(0, user5.Id);
            Assert.Equal(4, userRepo.GetAll().AsNoTracking().Count());

            Assert.Equal(0, role3.Id);
            Assert.Equal(2, roleRepo.GetAll().AsNoTracking().Count());

            Assert.Equal(0, creationAudited1.Id);
            Assert.Equal(0, creationAuditedRepo.GetAll().AsNoTracking().Count());

            #endregion

            #region SaveChanges

            // Act
            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal(5, user5.Id);
            Assert.NotNull(userRepo.GetAll().AsNoTracking().SingleOrDefault(p => p.Id == 5));
            Assert.Equal(5, userRepo.GetAll().AsNoTracking().Count());

            Assert.Equal(3, role3.Id);
            Assert.NotNull(roleRepo.GetAll().AsNoTracking().SingleOrDefault(p => p.Id == 3));
            Assert.Equal(3, roleRepo.GetAll().AsNoTracking().Count());

            Assert.Equal(1, creationAudited1.Id);
            Assert.Equal(1, creationAuditedRepo.GetAll().AsNoTracking().SingleOrDefault(p => p.Id == 1)?.CreatorUserId);
            Assert.Equal(1, creationAuditedRepo.GetAll().AsNoTracking().Count());

            #endregion

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestInsertAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();
            var roleRepo = GetRepository<Role>();
            var creationAuditedRepo = GetRepository<TestCreationAudited>();

            #region Insert but not SaveChanges

            // Act
            var user5 = new User
            {
                Name = "User5",
                RoleId = 1
            };
            await userRepo.InsertAsync(user5);

            var role3 = new Role
            {
                Name = "Role3",
            };
            await roleRepo.InsertAsync(role3);

            var creationAudited1 = new TestCreationAudited();
            await creationAuditedRepo.InsertAsync(creationAudited1);

            // Assert
            Assert.Equal(0, user5.Id);
            Assert.Equal(4, await userRepo.GetAll().AsNoTracking().CountAsync());

            Assert.Equal(0, role3.Id);
            Assert.Equal(2, await roleRepo.GetAll().AsNoTracking().CountAsync());

            Assert.Equal(0, creationAudited1.Id);
            Assert.Equal(0, await creationAuditedRepo.GetAll().AsNoTracking().CountAsync());

            #endregion

            #region SaveChanges

            // Act
            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal(5, user5.Id);
            Assert.Equal(5, await userRepo.GetAll().AsNoTracking().CountAsync());
            Assert.NotNull(await userRepo.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 5));

            Assert.Equal(3, role3.Id);
            Assert.NotNull(await roleRepo.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 3));
            Assert.Equal(3, await roleRepo.GetAll().AsNoTracking().CountAsync());

            Assert.Equal(1, creationAudited1.Id);
            Assert.Equal(1, (await creationAuditedRepo.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 1))?.CreatorUserId);
            Assert.Equal(1, await creationAuditedRepo.GetAll().AsNoTracking().CountAsync());

            #endregion

            // Complete uow
            await uow.CompleteAsync();
        }

        [Fact]
        public void TestInsertAndGetId()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();
            var roleRepo = GetRepository<Role>();
            var creationAuditedRepo = GetRepository<TestCreationAudited>();

            // Act
            var role3 = new Role
            {
                Name = "Role3"
            };
            roleRepo.InsertAndGetId(role3);

            var user5 = new User
            {
                Name = "User5",
                RoleId = role3.Id
            };
            userRepo.InsertAndGetId(user5);

            var creationAudited1 = new TestCreationAudited();
            creationAuditedRepo.InsertAndGetId(creationAudited1);

            // Assert
            Assert.Equal(3, role3.Id);
            Assert.NotNull(roleRepo.GetAll().AsNoTracking().SingleOrDefault(p => p.Id == 3));
            Assert.Equal(3, roleRepo.GetAll().AsNoTracking().Count());

            Assert.Equal(5, user5.Id);
            Assert.NotNull(userRepo.GetAll().AsNoTracking().SingleOrDefault(p => p.Id == 5));
            Assert.Equal(5, userRepo.GetAll().AsNoTracking().Count());

            Assert.Equal(1, creationAudited1.Id);
            Assert.Equal(1, creationAuditedRepo.GetAll().AsNoTracking().SingleOrDefault(p => p.Id == 1)?.CreatorUserId);
            Assert.Equal(1, creationAuditedRepo.GetAll().AsNoTracking().Count());

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestInsertAndGetIdAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();
            var roleRepo = GetRepository<Role>();
            var creationAuditedRepo = GetRepository<TestCreationAudited>();

            // Act
            var role3 = new Role
            {
                Name = "Role3"
            };
            await roleRepo.InsertAndGetIdAsync(role3);

            var user5 = new User
            {
                Name = "User5",
                RoleId = 1
            };
            await userRepo.InsertAndGetIdAsync(user5);

            var creationAudited1 = new TestCreationAudited();
            await creationAuditedRepo.InsertAndGetIdAsync(creationAudited1);

            // Assert
            Assert.Equal(3, role3.Id);
            Assert.NotNull(await roleRepo.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 3));
            Assert.Equal(3, await roleRepo.GetAll().AsNoTracking().CountAsync());

            Assert.Equal(5, user5.Id);
            Assert.Equal(5, await userRepo.GetAll().AsNoTracking().CountAsync());
            Assert.NotNull(await userRepo.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 5));

            Assert.Equal(1, creationAudited1.Id);
            Assert.Equal(1, (await creationAuditedRepo.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 1))?.CreatorUserId);
            Assert.Equal(1, await creationAuditedRepo.GetAll().AsNoTracking().CountAsync());

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region Update

        [Fact]
        public void TestUpdate()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();
            var modificationAuditedRepo = GetRepository<TestModificationAudited, long>();

            #region Get then update

            // Act
            var user1 = userRepo.Get(1);
            user1.Name = "TestUser1";
            userRepo.Update(user1);

            var modificationAudited1 = modificationAuditedRepo.Get(1);
            modificationAudited1.Name = "TestUpdate1";
            modificationAuditedRepo.Update(modificationAudited1);

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal("TestUser1", userRepo.GetAll().AsNoTracking().Single(p => p.Id == user1.Id).Name);

            Assert.Equal(1, modificationAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == modificationAudited1.Id).LastModifierUserId);

            #endregion

            #region Attach

            // Act
            var user2 = new User
            {
                Id = 2,
                Name = "TestUser2",
                RoleId = 1
            };
            userRepo.Update(user2);

            var modificationAudited2 = new TestModificationAudited
            {
                Id = 2,
                Name = "TestUpdate2"
            };
            modificationAudited2.Name = "TestUpdate2";
            modificationAuditedRepo.Update(modificationAudited2);

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal("TestUser2", userRepo.GetAll().AsNoTracking().Single(p => p.Id == user2.Id).Name);

            Assert.Equal(1, modificationAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == modificationAudited2.Id).LastModifierUserId);

            #endregion

            #region Update with action

            // Act
            userRepo.Update(3, user =>
            {
                user.Name = "TestUser3";
            });

            modificationAuditedRepo.Update(3, user =>
            {
                user.Name = "TestUpdate3";
            });

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal("TestUser3", userRepo.GetAll().AsNoTracking().Single(p => p.Id == 3).Name);

            Assert.Equal(1, modificationAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == 3).LastModifierUserId);

            #endregion

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestUpdateAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();
            var modificationAuditedRepo = GetRepository<TestModificationAudited, long>();

            #region Get then update

            // Act
            var user1 = await userRepo.GetAsync(1);
            user1.Name = "TestUser1";
            await userRepo.UpdateAsync(user1);

            var modificationAudited1 = await modificationAuditedRepo.GetAsync(1);
            modificationAudited1.Name = "TestUpdate1";
            await modificationAuditedRepo.UpdateAsync(modificationAudited1);

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal("TestUser1", (await userRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == user1.Id)).Name);

            Assert.Equal(1, (await modificationAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == modificationAudited1.Id)).LastModifierUserId);

            #endregion

            #region Attach

            // Act
            var user2 = new User
            {
                Id = 2,
                Name = "TestUser2",
                RoleId = 1
            };
            await userRepo.UpdateAsync(user2);

            var modificationAudited2 = new TestModificationAudited
            {
                Id = 2,
                Name = "TestUpdate2"
            };
            modificationAudited2.Name = "TestUpdate2";
            await modificationAuditedRepo.UpdateAsync(modificationAudited2);

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal("TestUser2", (await userRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == user2.Id)).Name);

            Assert.Equal(1, (await modificationAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == modificationAudited2.Id)).LastModifierUserId);

            #endregion

            #region Update with action

            // Act
            await userRepo.UpdateAsync(3, user =>
            {
                user.Name = "TestUser3";
                return Task.CompletedTask;
            });

            await modificationAuditedRepo.UpdateAsync(3, audited =>
             {
                 audited.Name = "TestUpdate3";

                 return Task.CompletedTask;
             });

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal("TestUser3", (await userRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == 3)).Name);

            Assert.Equal(1, (await modificationAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == 3)).LastModifierUserId);

            #endregion

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region InsertOrUpdate

        [Fact]
        public void TestInsertOrUpdate()
        {
            // Arrange
            using var uow = BeginUow();
            var modificationAuditedRepo = GetRepository<TestModificationAudited, long>();

            // Act
            var modificationAudited1 = new TestModificationAudited
            {
                Id = 1,
                Name = "TestUpdate1"
            };
            modificationAuditedRepo.InsertOrUpdate(modificationAudited1);

            var modificationAudited4 = new TestModificationAudited
            {
                Name = "TestUpdate4"
            };
            modificationAuditedRepo.InsertOrUpdate(modificationAudited4);

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal(4, modificationAuditedRepo.Count());
            Assert.Equal("TestUpdate1", modificationAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == 1).Name);
            Assert.Equal(1, modificationAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == 1).LastModifierUserId);
            Assert.Equal("TestUpdate4", modificationAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == 4).Name);
            Assert.Equal(1, modificationAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == 4).CreatorUserId);

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestInsertOrUpdateAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var modificationAuditedRepo = GetRepository<TestModificationAudited, long>();

            // Act
            var modificationAudited1 = new TestModificationAudited
            {
                Id = 1,
                Name = "TestUpdate1"
            };
            await modificationAuditedRepo.InsertOrUpdateAsync(modificationAudited1);

            var modificationAudited4 = new TestModificationAudited
            {
                Name = "TestUpdate4"
            };
            await modificationAuditedRepo.InsertOrUpdateAsync(modificationAudited4);

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal(4, await modificationAuditedRepo.CountAsync());
            Assert.Equal("TestUpdate1", (await modificationAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == 1)).Name);
            Assert.Equal(1, (await modificationAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == 1)).LastModifierUserId);
            Assert.Equal("TestUpdate4", (await modificationAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == 4)).Name);
            Assert.Equal(1, (await modificationAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == 4)).CreatorUserId);

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region Hard Delete

        [Fact]
        public void TestHardDelete()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            #region Delete by id

            // Act
            userRepo.Delete(1);

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Null(userRepo.GetAll().AsNoTracking().SingleOrDefault(p => p.Id == 1));
            Assert.Equal(3, userRepo.GetAll().AsNoTracking().Count());

            #endregion

            #region Delete by entity

            // Act
            userRepo.Delete(userRepo.Get(2));

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Null(userRepo.GetAll().AsNoTracking().SingleOrDefault(p => p.Id == 2));
            Assert.Equal(2, userRepo.GetAll().AsNoTracking().Count());

            #endregion

            #region Delete by predicate

            // Act
            userRepo.Delete(p => p.RoleId == 2);

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal(0, userRepo.GetAll().AsNoTracking().Count());

            #endregion

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestHardDeleteAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            #region Delete by id

            // Act
            await userRepo.DeleteAsync(1);

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Null(await userRepo.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 1));
            Assert.Equal(3, await userRepo.GetAll().AsNoTracking().CountAsync());

            #endregion

            #region Delete by entity

            // Act
            await userRepo.DeleteAsync(userRepo.Get(2));

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Null(await userRepo.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 2));
            Assert.Equal(2, await userRepo.GetAll().AsNoTracking().CountAsync());

            #endregion

            #region Delete by predicate

            // Act
            await userRepo.DeleteAsync(p => p.RoleId == 2);

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal(0, await userRepo.GetAll().AsNoTracking().CountAsync());

            #endregion

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        #region SoftDelet

        [Fact]
        public void TestSoftDelete()
        {
            // Arrange
            using var uow = BeginUow();
            var deletionAuditedRepo = GetRepository<TestDeletionAudited>();

            #region Delete by id

            // Act
            deletionAuditedRepo.Delete(1);

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal(1, deletionAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == 1).DeleterUserId);
            Assert.Equal(2, deletionAuditedRepo.GetAll().AsNoTracking().Count(p => p.IsDeleted));
            Assert.Equal(6, deletionAuditedRepo.GetAll().AsNoTracking().Count());

            #endregion

            #region Delete by entity

            // Act
            deletionAuditedRepo.Delete(deletionAuditedRepo.Get(2));

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal(1, deletionAuditedRepo.GetAll().AsNoTracking().Single(p => p.Id == 2).DeleterUserId);
            Assert.Equal(3, deletionAuditedRepo.GetAll().AsNoTracking().Count(p => p.IsDeleted));
            Assert.Equal(6, deletionAuditedRepo.GetAll().AsNoTracking().Count());

            #endregion

            #region Delete by predicate

            // Act
            deletionAuditedRepo.Delete(p => p.IsActive == false);

            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal(3, deletionAuditedRepo.GetAll().AsNoTracking().Count(p => p.IsActive == false && p.IsDeleted));
            Assert.Equal(6, deletionAuditedRepo.GetAll().AsNoTracking().Count());

            #endregion

            // Complete uow
            uow.Complete();
        }

        [Fact]
        public async Task TestSoftDeleteAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var deletionAuditedRepo = GetRepository<TestDeletionAudited>();

            #region Delete by id

            // Act
            await deletionAuditedRepo.DeleteAsync(1);

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal(1, (await deletionAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == 1)).DeleterUserId);
            Assert.Equal(2, await deletionAuditedRepo.GetAll().AsNoTracking().CountAsync(p => p.IsDeleted));
            Assert.Equal(6, await deletionAuditedRepo.GetAll().AsNoTracking().CountAsync());

            #endregion

            #region Delete by entity

            // Act
            await deletionAuditedRepo.DeleteAsync(await deletionAuditedRepo.GetAsync(2));

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal(1, (await deletionAuditedRepo.GetAll().AsNoTracking().SingleAsync(p => p.Id == 2)).DeleterUserId);
            Assert.Equal(3, await deletionAuditedRepo.GetAll().AsNoTracking().CountAsync(p => p.IsDeleted));
            Assert.Equal(6, await deletionAuditedRepo.GetAll().AsNoTracking().CountAsync());

            #endregion

            #region Delete by predicate

            // Act
            await deletionAuditedRepo.DeleteAsync(p => p.IsActive == false);

            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal(3, await deletionAuditedRepo.GetAll().AsNoTracking().CountAsync(p => p.IsActive == false && p.IsDeleted));
            Assert.Equal(6, await deletionAuditedRepo.GetAll().AsNoTracking().CountAsync());

            #endregion

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

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
            context.TestDeletionAudited.Add(new TestDeletionAudited { IsActive = false, IsDeleted = true});
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

        public IEfCoreRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity<int>
        {
            return _serviceProvider.GetService<IEfCoreRepository<TEntity>>();
        }

        public IEfCoreRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            return _serviceProvider.GetService<IEfCoreRepository<TEntity, TPrimaryKey>>();
        }
    }

    public class TestSession : EasyNetSessionBase
    {
        public override string UserId => "1";
        public override string UserName => "Test";
        public override string Role => "Admin";
    }
}
