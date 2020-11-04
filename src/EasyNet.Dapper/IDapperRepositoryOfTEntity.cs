using EasyNet.Domain.Entities;

namespace EasyNet.Dapper
{
    public interface IDapperRepository<TEntity> : IDapperRepository<TEntity, int> where TEntity : class, IEntity<int>
    {
    }
}
