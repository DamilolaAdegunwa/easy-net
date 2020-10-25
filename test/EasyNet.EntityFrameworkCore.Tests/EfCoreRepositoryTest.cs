using System;
using System.Collections.Generic;
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
    public class EfCoreRepositoryTest
    {
        //private readonly DatabaseFixture _fixture;
        private readonly IServiceProvider _serviceProvider;


        public EfCoreRepositoryTest()
        {
            //_fixture = fixture;


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

        #region GetAllList

        [Fact]
        public void TestGetAllList()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var users = userRepo.GetAllList();
            uow.Complete();

            // Assert
            Assert.Equal(4, users.Count);
            Assert.Equal("Name2", users[1].Name);
        }

        [Fact]
        public void TestGetAllListByPredicate()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var users = userRepo.GetAllList(p => p.Status == Status.Active);
            uow.Complete();

            // Assert
            Assert.Equal(3, users.Count);
            Assert.Equal("Name4", users[2].Name);
        }

        [Fact]
        public async Task TestGetAllListAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var users = await userRepo.GetAllListAsync();
            await uow.CompleteAsync();

            // Assert
            Assert.Equal(4, users.Count);
            Assert.Equal("Name2", users[1].Name);
        }

        [Fact]
        public async Task TestGetAllListByPredicateAsync()
        {
            // Arrange
            using var uow = BeginUow();
            var userRepo = GetRepository<User, long>();

            // Act
            var users = await userRepo.GetAllListAsync(p => p.Status == Status.Active);
            await uow.CompleteAsync();

            // Assert
            Assert.Equal(3, users.Count);
            Assert.Equal("Name4", users[2].Name);
        }

        #endregion

        //[Fact]
        //public async Task TestGet()
        //{
        //    var userRepo = GetRepository<User>();

        //    var user = userRepo.Get(2);
        //    Assert.Equal("Name2", user.Name);

        //    user = await userRepo.GetAsync(3);
        //    Assert.Equal("Name3", user.Name);
        //}

        //[Fact]
        //public async Task TestSingle()
        //{
        //    var userRepo = GetRepository<User>();

        //    var user = userRepo.Single(p => p.Name == "Name1");
        //    Assert.Equal("Name1", user.Name);

        //    user = await userRepo.SingleAsync(p => p.Name == "Name3");
        //    Assert.Equal("Name3", user.Name);

        //    Assert.Throws<InvalidOperationException>(() => userRepo.Single(p => p.Name == "Null"));
        //}

        //[Fact]
        //public async Task TestSingleOrDefault()
        //{
        //    var userRepo = GetRepository<User>();

        //    var user = userRepo.SingleOrDefault(p => p.Name == "Name1");
        //    Assert.Equal("Name1", user.Name);

        //    user = await userRepo.SingleOrDefaultAsync(p => p.Name == "Name3");
        //    Assert.Equal("Name3", user.Name);

        //    user = await userRepo.SingleOrDefaultAsync(p => p.Name == "Null");
        //    Assert.Null(user);
        //}

        //[Fact]
        //public async Task TestFirst()
        //{
        //    var userRepo = GetRepository<User>();

        //    var user = userRepo.First();
        //    Assert.Equal("Name1", user.Name);

        //    user = await userRepo.FirstAsync();
        //    Assert.Equal("Name1", user.Name);

        //    user = userRepo.First(p => p.Name.Contains("Name"));
        //    Assert.Equal("Name1", user.Name);

        //    user = await userRepo.FirstAsync(p => p.Status == Status.Inactive && p.Name.Contains("Name"));
        //    Assert.Equal("Name3", user.Name);

        //    Assert.Throws<InvalidOperationException>(() => userRepo.First(p => p.Name == "Null"));
        //}

        //[Fact]
        //public async Task TestFirstOrDefault()
        //{
        //    var userRepo = GetRepository<User>();

        //    var user = userRepo.FirstOrDefault();
        //    Assert.Equal("Name1", user.Name);

        //    user = await userRepo.FirstOrDefaultAsync();
        //    Assert.Equal("Name1", user.Name);

        //    user = userRepo.FirstOrDefault(p => p.Name.Contains("Name"));
        //    Assert.Equal("Name1", user.Name);

        //    user = await userRepo.FirstOrDefaultAsync(p => p.Name.Contains("Name") && p.Status == Status.Inactive);
        //    Assert.Equal("Name3", user.Name);

        //    user = userRepo.FirstOrDefault(p => p.Name == "Null");
        //    Assert.Null(user);
        //}

        //[Fact]
        //public async Task TestCount()
        //{
        //    var userRepo = GetRepository<User>();

        //    var count = userRepo.Count();
        //    Assert.Equal(4, count);

        //    count = await userRepo.CountAsync();
        //    Assert.Equal(4, count);

        //    count = await userRepo.CountAsync(p => p.Status == Status.Active);
        //    Assert.Equal(3, count);

        //    count = await userRepo.CountAsync(p => p.Name == "Null");
        //    Assert.Equal(0, count);
        //}

        //[Fact]
        //public async Task TestLongCount()
        //{
        //    var userRepo = GetRepository<User>();

        //    var count = userRepo.LongCount();
        //    Assert.Equal(4, count);

        //    count = await userRepo.LongCountAsync();
        //    Assert.Equal(4, count);

        //    count = await userRepo.LongCountAsync(p => p.Status == Status.Active);
        //    Assert.Equal(3, count);

        //    count = await userRepo.LongCountAsync(p => p.Name == "Null");
        //    Assert.Equal(0, count);
        //}

        //[Fact]
        //public async Task TestAny()
        //{
        //    var userRepo = GetRepository<User>();

        //    var result = userRepo.Any(p => p.Name.Contains("Name"));
        //    Assert.True(result);

        //    result = await userRepo.AnyAsync(p => p.Status == Status.Active && p.Name == "Name3");
        //    Assert.False(false);
        //}

        //[Fact]
        //public async Task TestInsert()
        //{
        //    var roleRepo = GetRepository<Role>();
        //    var dbContext = roleRepo.GetDbContext();

        //    var role5 = new Role
        //    {
        //        UserId = 1,
        //        Name = "Role5"
        //    };
        //    roleRepo.Insert(role5);
        //    Assert.Equal(0, role5.Id);
        //    dbContext.SaveChanges();
        //    Assert.Equal(5, role5.Id);

        //    var role6 = new Role
        //    {
        //        UserId = 1,
        //        Name = "Role6"
        //    };
        //    await roleRepo.InsertAsync(role6);
        //    Assert.Equal(0, role6.Id);
        //    await dbContext.SaveChangesAsync();
        //    Assert.Equal(6, role6.Id);

        //    var role7 = new Role
        //    {
        //        UserId = 2,
        //        Name = "Role7"
        //    };
        //    roleRepo.InsertAndGetId(role7);
        //    Assert.Equal(7, role7.Id);

        //    var role8 = new Role
        //    {
        //        UserId = 2,
        //        Name = "Role8"
        //    };
        //    await roleRepo.InsertAndGetIdAsync(role8);
        //    Assert.Equal(8, role8.Id);

        //    Assert.NotNull(roleRepo.Get(5));
        //    Assert.NotNull(roleRepo.Get(6));
        //    Assert.NotNull(roleRepo.Get(7));
        //    Assert.NotNull(roleRepo.Get(8));
        //}

        //[Fact]
        //public async Task TestUpdate()
        //{
        //    var roleRepo = GetRepository<Role>();
        //    var dbContext = roleRepo.GetDbContext();
        //    dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;

        //    var role1 = new Role
        //    {
        //        Id = 1,
        //        UserId = 1,
        //        Name = "NewRole1"
        //    };
        //    roleRepo.Update(role1);
        //    dbContext.SaveChanges();
        //    role1 = roleRepo.Get(1);
        //    Assert.Equal("NewRole1", role1.Name);

        //    var role2 = roleRepo.Get(2);
        //    role2.Name = "NewRole2";
        //    await roleRepo.UpdateAsync(role2);
        //    await dbContext.SaveChangesAsync();
        //    role2 = roleRepo.Get(2);
        //    Assert.Equal("NewRole2", role2.Name);

        //    roleRepo.Update(3, role =>
        //    {
        //        role.Name = "NewRole3";
        //    });
        //    dbContext.SaveChanges();
        //    var role3 = roleRepo.Get(3);
        //    Assert.Equal("NewRole3", role3.Name);

        //    roleRepo.Update(4, role =>
        //    {
        //        role.Name = "NewRole4";
        //    });
        //    dbContext.SaveChanges();
        //    var role4 = roleRepo.Get(4);
        //    Assert.Equal("NewRole4", role4.Name);
        //}

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

            context.Users.AddRange(new List<User>
            {
                new User{Name = "Name1", Status = Status.Active},
                new User{Name = "Name2", Status = Status.Active},
                new User{Name = "Name3", Status = Status.Inactive},
                new User{Name = "Name4", Status = Status.Active}
            });

            context.SaveChanges();
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
