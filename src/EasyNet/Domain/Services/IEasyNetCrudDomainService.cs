using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyNet.Domain.Entities;

namespace EasyNet.Domain.Services
{
    public interface IEasyNetCrudDomainService<TEntity> : IEasyNetCrudDomainService<TEntity, int>
        where TEntity : IEntity<int>
    {
    }

    public interface IEasyNetCrudDomainService<TEntity, in TPrimaryKey> : IEasyNetDomainService
        where TEntity : IEntity<TPrimaryKey>
    {
        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(TPrimaryKey id);
    }
}
