using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;

namespace EasyNet.EntityFrameworkCore.Repositories
{
    public interface IEfCoreRepository<TEntity> : IEfCoreRepository<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
    }
}
