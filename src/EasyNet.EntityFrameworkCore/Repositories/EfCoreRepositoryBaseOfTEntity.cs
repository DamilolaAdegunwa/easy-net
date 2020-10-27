using EasyNet.Domain.Entities;
using EasyNet.EntityFrameworkCore.Uow;
using EasyNet.Runtime.Session;

namespace EasyNet.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryBase<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, int>, IEfCoreRepository<TEntity>
         where TEntity : class, IEntity<int>
         where TDbContext : EasyNetDbContext
    {
        public EfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider, IEasyNetSession session)
            : base(dbContextProvider, session)
        {
        }
    }
}
