using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyNet.Domain.Entities;

namespace EasyNet.Domain.Services
{
    public interface IEasyNetQueryDomainService<TEntity> : IEasyNetQueryDomainService<TEntity, int>
        where TEntity : IEntity<int>
    {
    }

    public interface IEasyNetQueryDomainService<TEntity, in TPrimaryKey> : IEasyNetDomainService
        where TEntity : IEntity<TPrimaryKey>
    {
        Task<TEntity> GetByIdAsync(TPrimaryKey id);

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);
    }
}
