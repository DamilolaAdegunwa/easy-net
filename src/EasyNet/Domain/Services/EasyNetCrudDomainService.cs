using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;

namespace EasyNet.Domain.Services
{
    public abstract class EasyNetCrudDomainService<TEntity> : EasyNetCrudDomainService<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected EasyNetCrudDomainService(IIocResolver iocResolver, IRepository<TEntity, int> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetCrudDomainService<TEntity, TPrimaryKey> : EasyNetQueryDomainService<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        protected EasyNetCrudDomainService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver, repository)
        {
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));

            await Repository.InsertAndGetIdAsync(entity);

            return entity;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));

            return Repository.UpdateAsync(entity);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            return Repository.DeleteAsync(id);
        }
    }
}