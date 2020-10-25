//using System;
//using Microsoft.EntityFrameworkCore;

//namespace EasyNet.EntityFrameworkCore.Tests.Fixture
//{
//    [Obsolete]
//    public class MsSqlDatabaseFixtureForUow : MsSqlDatabaseFixture
//    {
//        [Obsolete]
//        protected override void InitDb()
//        {
//            using (var dbContext = GetDbContext())
//            {
//                dbContext.Database.ExecuteSqlCommand("IF OBJECT_ID('Vehicles', 'U') IS NOT NULL DROP TABLE [Vehicles]");

//                dbContext.Database.ExecuteSqlCommand(@"CREATE TABLE Vehicles (Id int IDENTITY(1,1) not null, Name varchar(50) not null, PRIMARY KEY (Id))");
           
//                // 插入默认的车辆列表
//                for (int i = 1; i <= 10; i++)
//                {
//                    dbContext.Database.ExecuteSqlCommand("INSERT INTO [dbo].[Vehicles]([Name])VALUES('Vehicles" + i + "')");
//                }
//            }
//        }
//    }
//}
