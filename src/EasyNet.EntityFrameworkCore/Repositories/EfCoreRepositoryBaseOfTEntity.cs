using EasyNet.Domain.Entities;
using EasyNet.Domain.Uow;

namespace EasyNet.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryBase<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, int>, IEfCoreRepository<TEntity>
         where TEntity : class, IEntity<int>
         where TDbContext : EasyNetDbContext
    {
        public EfCoreRepositoryBase(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
            : base(currentUnitOfWorkProvider)
        {
        }
    }
}
