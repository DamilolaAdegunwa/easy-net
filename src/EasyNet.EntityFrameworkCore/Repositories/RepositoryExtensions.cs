using System;
using System.Linq;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;

namespace EasyNet.EntityFrameworkCore.Repositories
{
    public static class RepositoryExtensions
    {
        public static IQueryable<TEntity> GetAll<TEntity>(this IRepository<TEntity> repository) where TEntity : class, IEntity<int>
        {
            if (repository is IEfCoreRepository<TEntity> efCoreRepository)
            {
                return efCoreRepository.GetAll();
            }

            throw new Exception($"The {repository.GetType().AssemblyQualifiedName} is not a IEfCoreRepository.");
        }

        public static IQueryable<TEntity> GetAll<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository) where TEntity : class, IEntity<TPrimaryKey>
        {
            if (repository is IEfCoreRepository<TEntity, TPrimaryKey> efCoreRepository)
            {
                return efCoreRepository.GetAll();
            }

            throw new Exception($"The {repository.GetType().AssemblyQualifiedName} is not a IEfCoreRepository.");
        }
    }
}
