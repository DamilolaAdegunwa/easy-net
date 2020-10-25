//using System;
//using EasyNet.EntityFrameworkCore.Tests.DbContext;
//using Microsoft.EntityFrameworkCore;

//namespace EasyNet.EntityFrameworkCore.Tests.Fixture
//{
//    public class MsSqlDatabaseFixture : IDisposable
//    {
//        private const string DatabaseName = "NbpEfCoreTests";

//        [Obsolete]
//        public MsSqlDatabaseFixture()
//        {
//            InitDb();
//        }

//        public string ConnectionString => $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={DatabaseName};Integrated Security=True;";

//        [Obsolete]
//        protected virtual void InitDb()
//        {
//            using (var dbContext = GetDbContext())
//            {
//                dbContext.Database.ExecuteSqlCommand("IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE [Users]");
//                dbContext.Database.ExecuteSqlCommand("IF OBJECT_ID('Roles', 'U') IS NOT NULL DROP TABLE [Roles]");
//                dbContext.Database.ExecuteSqlCommand("IF OBJECT_ID('Books', 'U') IS NOT NULL DROP TABLE [Books]");
//                dbContext.Database.ExecuteSqlCommand("IF OBJECT_ID('Authors', 'U') IS NOT NULL DROP TABLE [Authors]");

//                dbContext.Database.ExecuteSqlCommand(@"CREATE TABLE Users (Id int IDENTITY(1,1) not null, Name varchar(50) not null, Status int not null, PRIMARY KEY (Id))");
//                dbContext.Database.ExecuteSqlCommand(@"CREATE TABLE Roles (Id int IDENTITY(1,1) not null, Name varchar(50) not null, UserId int, PRIMARY KEY (Id))");
//                dbContext.Database.ExecuteSqlCommand(@"CREATE TABLE Books (Id bigint IDENTITY(1,1) not null, Name varchar(50) not null, PRIMARY KEY (Id))");
//                dbContext.Database.ExecuteSqlCommand(@"CREATE TABLE Authors (Id bigint IDENTITY(1,1) not null, Name varchar(50) not null, PRIMARY KEY (Id))");

//                // 插入默认的用户列表，在测试过程中不再对User表进行增删改查的操作
//                dbContext.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[Users]([Name],[Status])VALUES('Name1', 0)");
//                dbContext.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[Users]([Name],[Status])VALUES('Name2', 0)");
//                dbContext.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[Users]([Name],[Status])VALUES('Name3', -1)");
//                dbContext.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[Users]([Name],[Status])VALUES('Name4', 0)");

//                // 插入默认的角色列表, 在测试过程中用于测试Update操作
//                dbContext.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[Roles]([UserId],[Name])VALUES(1,'Role1')");
//                dbContext.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[Roles]([UserId],[Name])VALUES(1,'Role2')");
//                dbContext.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[Roles]([UserId],[Name])VALUES(2,'Role3')");
//                dbContext.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[Roles]([UserId],[Name])VALUES(4,'Role4')");

//                // 插入默认的书籍列表, 在测试过程中用户测试Delete操作
//                for(int i= 1; i <= 30; i++)
//                {
//                    dbContext.Database.ExecuteSqlCommand("INSERT INTO [dbo].[Books]([Name])VALUES('Book"+ i + "')");
//                }

//                // 插入默认的作者列表, 在测试过程中用户测试InsertOrUpdate操作
//                for (int i = 1; i <= 10; i++)
//                {
//                    dbContext.Database.ExecuteSqlCommand("INSERT INTO [dbo].[Authors]([Name])VALUES('Authors" + i + "')");
//                }
//            }
//        }

//        public EfCoreContext GetDbContext()
//        {
//	        var builder = new DbContextOptionsBuilder<EfCoreContext>();
//	        builder.UseSqlServer("");

//            return new EfCoreContext(builder.Options);
//        }

//        public void Dispose()
//        {
//        }
//    }
//}
