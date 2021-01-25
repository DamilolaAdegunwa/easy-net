using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;

namespace EasyNet.Domain.Services
{
    public abstract class EasyNetQueryDomainService<TEntity> : EasyNetQueryDomainService<TEntity, int>, IEasyNetQueryDomainService<TEntity>
        where TEntity : class, IEntity<int>
    {
        protected EasyNetQueryDomainService(IIocResolver iocResolver, IRepository<TEntity, int> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetQueryDomainService<TEntity, TPrimaryKey> : EasyNetDomainService<TEntity, TPrimaryKey>, IEasyNetQueryDomainService<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected EasyNetQueryDomainService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver, repository)
        {
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return Repository.GetAsync(id);
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Repository.GetAllListAsync(predicate);
        }
    }
}