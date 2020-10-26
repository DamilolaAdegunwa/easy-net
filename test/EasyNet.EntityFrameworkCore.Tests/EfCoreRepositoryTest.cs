using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.DependencyInjection;
using EasyNet.EntityFrameworkCore.Repositories;
using EasyNet.EntityFrameworkCore.Tests.DbContext;
using EasyNet.EntityFrameworkCore.Tests.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                .AddEfCore<EfCoreContext>(options =>
                {
                    options.UseSqlite(CreateInMemoryDatabase());
                });

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

            // Assert
            Assert.Equal(0, user5.Id);
            Assert.Equal(4, userRepo.GetQueryable().AsNoTracking().Count());
            Assert.Equal(0, role3.Id);
            Assert.Equal(2, roleRepo.GetQueryable().AsNoTracking().Count());

            #endregion

            #region SaveChanges

            // Act
            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal(5, user5.Id);
            Assert.NotNull(userRepo.GetQueryable().AsNoTracking().SingleOrDefault(p => p.Id == 5));
            Assert.Equal(5, userRepo.GetQueryable().AsNoTracking().Count());
            Assert.Equal(3, role3.Id);
            Assert.NotNull(roleRepo.GetQueryable().AsNoTracking().SingleOrDefault(p => p.Id == 3));
            Assert.Equal(3, roleRepo.GetQueryable().AsNoTracking().Count());

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

            // Assert
            Assert.Equal(0, user5.Id);
            Assert.Equal(4, await userRepo.GetQueryable().AsNoTracking().CountAsync());
            Assert.Equal(0, role3.Id);
            Assert.Equal(2, await roleRepo.GetQueryable().AsNoTracking().CountAsync());

            #endregion

            #region SaveChanges

            // Act
            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal(5, user5.Id);
            Assert.Equal(5, await userRepo.GetQueryable().AsNoTracking().CountAsync());
            Assert.NotNull(await userRepo.GetQueryable().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 5));
            Assert.Equal(3, role3.Id);
            Assert.NotNull(await roleRepo.GetQueryable().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 3));
            Assert.Equal(3, await roleRepo.GetQueryable().AsNoTracking().CountAsync());

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

            // Assert
            Assert.Equal(3, role3.Id);
            Assert.NotNull(roleRepo.GetQueryable().AsNoTracking().SingleOrDefault(p => p.Id == 3));
            Assert.Equal(3, roleRepo.GetQueryable().AsNoTracking().Count());
            Assert.Equal(5, user5.Id);
            Assert.NotNull(userRepo.GetQueryable().AsNoTracking().SingleOrDefault(p => p.Id == 5));
            Assert.Equal(5, userRepo.GetQueryable().AsNoTracking().Count());

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

            // Assert
            Assert.Equal(3, role3.Id);
            Assert.NotNull(await roleRepo.GetQueryable().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 3));
            Assert.Equal(3, await roleRepo.GetQueryable().AsNoTracking().CountAsync());
            Assert.Equal(5, user5.Id);
            Assert.Equal(5, await userRepo.GetQueryable().AsNoTracking().CountAsync());
            Assert.NotNull(await userRepo.GetQueryable().AsNoTracking().SingleOrDefaultAsync(p => p.Id == 5));

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

            #region Get then update

            // Act
            var user1 = userRepo.Get(1);
            user1.Name = "TestUser1";
            userRepo.Update(user1);
            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal("TestUser1", userRepo.GetQueryable().AsNoTracking().Single(p => p.Id == user1.Id).Name);

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
            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal("TestUser2", userRepo.GetQueryable().AsNoTracking().Single(p => p.Id == user2.Id).Name);

            #endregion

            #region Update with action

            // Act
            userRepo.Update(3, user =>
            {
                user.Name = "TestUser3";
            });
            ((IUnitOfWork)uow).SaveChanges();

            // Assert
            Assert.Equal("TestUser3", userRepo.GetQueryable().AsNoTracking().Single(p => p.Id == 3).Name);

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

            #region Get then update

            // Act
            var user1 = await userRepo.GetAsync(1);
            user1.Name = "TestUser1";
            await userRepo.UpdateAsync(user1);
            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal("TestUser1", (await userRepo.GetQueryable().AsNoTracking().SingleAsync(p => p.Id == user1.Id)).Name);

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
            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal("TestUser2", (await userRepo.GetQueryable().AsNoTracking().SingleAsync(p => p.Id == user2.Id)).Name);

            #endregion

            #region Update with action

            // Act
            await userRepo.UpdateAsync(3, user =>
            {
                user.Name = "TestUser3";
                return Task.CompletedTask;
            });
            await ((IUnitOfWork)uow).SaveChangesAsync();

            // Assert
            Assert.Equal("TestUser3", (await userRepo.GetQueryable().AsNoTracking().SingleAsync(p => p.Id == 3)).Name);

            #endregion

            // Complete uow
            await uow.CompleteAsync();
        }

        #endregion

        //[Fact]
        //public async Task TestInsertOrUpdate()
        //{
        //    var authorRepo = GetRepository<Author, long>();
        //    var dbContext = authorRepo.GetDbContext();
        //    dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;

        //    var author11 = new Author
        //    {
        //        Name = "Author11"
        //    };
        //    authorRepo.InsertOrUpdate(author11);
        //    dbContext.SaveChanges();
        //    Assert.Equal(11, author11.Id);

        //    var author12 = new Author
        //    {
        //        Name = "Author12"
        //    };
        //    await authorRepo.InsertOrUpdateAsync(author12);
        //    dbContext.SaveChanges();
        //    Assert.Equal(12, author12.Id);

        //    var author13 = new Author
        //    {
        //        Name = "Author13"
        //    };
        //    authorRepo.InsertOrUpdateAndGetId(author13);
        //    Assert.Equal(13, author13.Id);

        //    var author14 = new Author
        //    {
        //        Name = "Author14"
        //    };
        //    await authorRepo.InsertOrUpdateAndGetIdAsync(author14);
        //    Assert.Equal(14, author14.Id);

        //    Assert.NotNull(authorRepo.Get(11));
        //    Assert.NotNull(authorRepo.Get(12));
        //    Assert.NotNull(authorRepo.Get(13));
        //    Assert.NotNull(authorRepo.Get(14));

        //    var author1 = new Author
        //    {
        //        Id = 1,
        //        Name = "NewAuthor1"
        //    };
        //    authorRepo.InsertOrUpdate(author1);
        //    dbContext.SaveChanges();
        //    author1 = authorRepo.Get(1);
        //    Assert.Equal("NewAuthor1", author1.Name);

        //    var author2 = new Author
        //    {
        //        Id = 2,
        //        Name = "NewAuthor2"
        //    };
        //    authorRepo.InsertOrUpdate(author2);
        //    dbContext.SaveChanges();
        //    author2 = authorRepo.Get(2);
        //    Assert.Equal("NewAuthor2", author2.Name);

        //    var author3 = new Author
        //    {
        //        Id = 3,
        //        Name = "NewAuthor3"
        //    };
        //    authorRepo.InsertOrUpdateAndGetId(author3);
        //    dbContext.SaveChanges();
        //    author3 = authorRepo.Get(3);
        //    Assert.Equal("NewAuthor3", author3.Name);

        //    var author4 = new Author
        //    {
        //        Id = 4,
        //        Name = "NewAuthor4"
        //    };
        //    authorRepo.InsertOrUpdateAndGetId(author4);
        //    dbContext.SaveChanges();
        //    author4 = authorRepo.Get(4);
        //    Assert.Equal("NewAuthor4", author4.Name);
        //}

        //[Fact]
        //public async Task TestDelete()
        //{
        //    var bookRepo = GetRepository<Book, long>();
        //    var dbContext = bookRepo.GetDbContext();

        //    var book1 = new Book
        //    {
        //        Id = 1,
        //        Name = "Book1"
        //    };
        //    bookRepo.Delete(book1);
        //    dbContext.SaveChanges();
        //    Assert.Null(bookRepo.SingleOrDefault(p => p.Id == 1));
        //    Assert.Equal(8, bookRepo.Count(p => p.Id < 10));

        //    var book2 = new Book
        //    {
        //        Id = 2,
        //        Name = "Book2"
        //    };
        //    await bookRepo.DeleteAsync(book2);
        //    await dbContext.SaveChangesAsync();
        //    Assert.Null(bookRepo.SingleOrDefault(p => p.Id == 2));
        //    Assert.Equal(7, bookRepo.Count(p => p.Id < 10));

        //    bookRepo.Delete(3);
        //    dbContext.SaveChanges();
        //    Assert.Equal(6, bookRepo.Count(p => p.Id < 10));

        //    await bookRepo.DeleteAsync(4);
        //    await dbContext.SaveChangesAsync();
        //    Assert.Equal(5, bookRepo.Count(p => p.Id < 10));

        //    bookRepo.Delete(p => p.Name.Contains("Book1"));
        //    dbContext.SaveChanges();
        //    Assert.False(bookRepo.Any(p => p.Name.Contains("Book1")));

        //    await bookRepo.DeleteAsync(p => p.Name.Contains("Book2"));
        //    await dbContext.SaveChangesAsync();
        //    Assert.False(bookRepo.Any(p => p.Name.Contains("Book2")));
        //}

        //#region Helper

        //private IDbContextProvider<EfCoreContext> GetDbContextProvider()
        //{
        //    var dbContextProviderMock = new Moq.Mock<IDbContextProvider<EfCoreContext>>();
        //    dbContextProviderMock.Setup(p => p.GetDbContext()).Returns(_msFixture.GetDbContext());

        //    return dbContextProviderMock.Object;
        //}

        //private IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity<int>
        //{
        //    return new EfCoreRepositoryBase<EfCoreContext, TEntity>(GetDbContextProvider());
        //}

        //private IRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        //{
        //    var dbContextProviderMock = new Moq.Mock<IDbContextProvider<EfCoreContext>>();
        //    dbContextProviderMock.Setup(p => p.GetDbContext()).Returns(_msFixture.GetDbContext());

        //    return new EfCoreRepositoryBase<EfCoreContext, TEntity, TPrimaryKey>(GetDbContextProvider());
        //}

        //#endregion

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

            // Insert default roles
            context.Roles.Add(new Role { Name = "Admin" });
            context.SaveChanges();
            context.Roles.Add(new Role { Name = "User" });
            context.SaveChanges();

            // Insert default users
            context.Users.Add(new User { Name = "User1", Status = Status.Active, RoleId = 1 });
            context.SaveChanges();
            context.Users.Add(new User { Name = "User2", Status = Status.Active, RoleId = 2 });
            context.SaveChanges();
            context.Users.Add(new User { Name = "User3", Status = Status.Inactive, RoleId = 2 });
            context.SaveChanges();
            context.Users.Add(new User { Name = "User4", Status = Status.Active, RoleId = 2 });
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
}
