using System.Linq;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Repositories
{
    public interface IEfCoreRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> 
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets <see cref="DbContext"/>
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();

        /// <summary>
        /// Gets <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQueryable();
    }
}
