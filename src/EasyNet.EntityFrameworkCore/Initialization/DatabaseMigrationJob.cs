using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.Uow;
using EasyNet.Runtime.Initialization;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Initialization
{
    [UnitOfWork(false)]
    public class DatabaseMigrationJob<TDbContext> : IEasyNetInitializationJob where TDbContext : EasyNetDbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        public DatabaseMigrationJob(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public void Start()
        {
            _dbContextProvider.GetDbContext().Database.Migrate();
        }
    }
}
