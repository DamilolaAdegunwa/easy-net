using System.Linq;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Extensions
{
	public static class EfCoreRepositoryExtensions
	{
		/// <summary>
		/// Gets a <see cref="DbContext"/>.
		/// </summary>
		/// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
		/// <param name="repository">The repository.</param>
		/// <returns>A <see cref="DbContext"/></returns>
		public static DbContext GetDbContext<TEntity>(this IRepository<TEntity> repository)
			where TEntity : class, IEntity<int>
		{
			return repository.GetDbContext<TEntity, int>();
		}

		/// <summary>
		/// Gets a <see cref="DbContext"/>.
		/// </summary>
		/// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
		/// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
		/// <param name="repository">The repository.</param>
		/// <returns>A <see cref="DbContext"/></returns>
		public static DbContext GetDbContext<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
			where TEntity : class, IEntity<TPrimaryKey>
		{
			Check.NotNull(repository, nameof(repository));

			if (repository is IRepositoryWithDbContext repositoryWithDbContext)
			{
				return repositoryWithDbContext.GetDbContext();
			}

			throw new EasyNetException($"The repository {repository.GetType().AssemblyQualifiedName} is not inherit from IRepositoryWithDbContext<TDbContext>.");
		}

		/// <summary>
		/// Gets a <see cref="IQueryable{TEntity}"/>.
		/// </summary>
		/// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
		/// <param name="repository">The repository.</param>
		/// <returns>A <see cref="IQueryable{TEntity}"/>.</returns>
		public static IQueryable<TEntity> GetQueryTable<TEntity>(this IRepository<TEntity> repository)
			where TEntity : class, IEntity<int>
		{
			return repository.GetQueryTable<TEntity, int>();
		}

		/// <summary>
		/// Gets a <see cref="IQueryable{TEntity}"/>.
		/// </summary>
		/// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
		/// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
		/// <param name="repository">The repository.</param>
		/// <returns>A <see cref="IQueryable{TEntity}"/>.</returns>
		public static IQueryable<TEntity> GetQueryTable<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
			where TEntity : class, IEntity<TPrimaryKey>
		{
			Check.NotNull(repository, nameof(repository));

			if (repository is IRepositoryWithQueryable<TEntity, TPrimaryKey> repositoryWithDbContext)
			{
				return repositoryWithDbContext.GetQueryable();
			}

			throw new EasyNetException($"The repository {repository.GetType().AssemblyQualifiedName} is not inherit from IRepositoryWithQueryable<TEntity, TPrimaryKey>.");
		}
	}
}
