using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyNet.Domain.Entities;

namespace EasyNet.Domain.Services
{
    public interface IEasyNetPassivableDomainService<TEntity> : IEasyNetPassivableDomainService<TEntity, int>
        where TEntity : IEntity<int>
    {
    }

    public interface IEasyNetPassivableDomainService<TEntity, in TPrimaryKey> : IEasyNetDomainService
        where TEntity : IEntity<TPrimaryKey>
    {
        Task<TEntity> ArchiveAsync(TPrimaryKey id);

        Task<TEntity> ArchiveAsync(TEntity entity);

        Task<TEntity> ActivateAsync(TPrimaryKey id);

        Task<TEntity> ActivateAsync(TEntity entity);
    }
}
