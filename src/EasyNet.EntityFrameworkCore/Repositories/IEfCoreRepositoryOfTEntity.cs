using EasyNet.Domain.Entities;

namespace EasyNet.EntityFrameworkCore.Repositories
{
    public interface IEfCoreRepository<TEntity> : IEfCoreRepository<TEntity, int> 
        where TEntity : class, IEntity<int>
    {
    }
}
