using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;

namespace EasyNet.Domain.Services
{
    public abstract class EasyNetPassivableDomainService<TEntity> : EasyNetPassivableDomainService<TEntity, int>
        where TEntity : class, IEntity<int>, IPassivable
    {
        protected EasyNetPassivableDomainService(IIocResolver iocResolver, IRepository<TEntity, int> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetPassivableDomainService<TEntity, TPrimaryKey> : EasyNetCrudDomainService<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, IPassivable
    {
        protected EasyNetPassivableDomainService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver, repository)
        {
        }

        /// <summary>
        /// Archive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> ArchiveAsync(TPrimaryKey id)
        {
            var entity = await Repository.GetAsync(id);

            return await ArchiveAsync(entity);
        }

        /// <summary>
        /// Archive
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> ArchiveAsync(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));

            entity.IsActive = false;

            return await Repository.UpdateAsync(entity);
        }

        /// <summary>
        /// Activate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> ActivateAsync(TPrimaryKey id)
        {
            var entity = await Repository.GetAsync(id);

            return await ActivateAsync(entity);
        }

        /// <summary>
        /// Activate
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> ActivateAsync(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));

            entity.IsActive = true;

            return await Repository.UpdateAsync(entity);
        }
    }
}