using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyNet.Domain.Entities;

namespace EasyNet.Domain.Repositories
{
	/// <summary>
	/// This interface is implemented by all repositories to ensure implementation of fixed methods.
	/// </summary>
	/// <typeparam name="TEntity">Main Entity type this repository works on</typeparam>
	/// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
	public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
	{
		#region Select/Get/Query

		/// <summary>
		/// Used to get all entities.
		/// </summary>
		/// <returns></returns>
		List<TEntity> GetAllList();

		/// <summary>
		/// Used to get all entities.
		/// </summary>
		/// <returns></returns>
		Task<List<TEntity>> GetAllListAsync();

		/// <summary>
		/// Used to get all entities based on given <paramref name="predicate"/>.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="List{TEntity}"/>.</returns>
		List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Used to get all entities based on given <paramref name="predicate"/>.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="List{TEntity}"/>.</returns>
		Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets an entity with given primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity to get.</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		/// <exception cref="EasyNetNotFoundEntityException{TEntity,TPrimaryKey}">Throws exception if no entity.</exception>
		TEntity Get(TPrimaryKey id);

		/// <summary>
		/// Gets an entity with given primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity to get.</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		/// <exception cref="EasyNetNotFoundEntityException{TEntity,TPrimaryKey}">Throws exception if no entity.</exception>
		Task<TEntity> GetAsync(TPrimaryKey id);

		/// <summary>
		/// Gets exactly one entity with given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		/// <exception cref="InvalidOperationException">Throws exception if no entity or more than one entity.</exception>
		TEntity Single(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets exactly one entity with given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		/// <exception cref="InvalidOperationException">Throws exception if no entity or more than one entity.</exception>
		Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets a first entity.
		/// </summary>
		/// <returns>The <see cref="TEntity"/>.</returns>
		/// <exception cref="InvalidOperationException">Throws exception if no entity.</exception>
		TEntity First();

		/// <summary>
		/// Gets a first entity.
		/// </summary>
		/// <returns>The <see cref="TEntity"/>.</returns>
		/// <exception cref="InvalidOperationException">Throws exception if no entity.</exception>
		Task<TEntity> FirstAsync();

		/// <summary>
		/// Gets a first entity with given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="TEntity"/> or null.</returns>
		TEntity First(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets a first entity with given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="TEntity"/> or null.</returns>
		Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets only one entity with given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="TEntity"/> or null.</returns>
		/// <exception cref="InvalidOperationException">Throws exception if more than one entity.</exception>
		TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets only one entity with given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="TEntity"/> or null.</returns>
		/// <exception cref="InvalidOperationException">Throws exception if more than one entity.</exception>
		Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets a first entity with given predicate.
		/// </summary>
		/// <returns>The <see cref="TEntity"/> or null.</returns>
		TEntity FirstOrDefault();

		/// <summary>
		/// Gets a first entity with given predicate.
		/// </summary>
		/// <returns>The <see cref="TEntity"/> or null.</returns>
		Task<TEntity> FirstOrDefaultAsync();

		/// <summary>
		/// Gets a first entity with given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="TEntity"/> or null.</returns>
		TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets a first entity with given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>The <see cref="TEntity"/> or null.</returns>
		Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

		#endregion

		#region Insert

		/// <summary>
		/// Inserts a new entity.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The inserted <see cref="TEntity"/>.</returns>
		TEntity Insert(TEntity entity);

		/// <summary>
		/// Inserts a new entity.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The inserted <see cref="TEntity"/>.</returns>
		Task<TEntity> InsertAsync(TEntity entity);

		/// <summary>
		/// Inserts a new entity.
		/// It may require to save current unit of work to be able to retrieve id.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The primary key of the inserted <see cref="TEntity"/>.</returns>
		TPrimaryKey InsertAndGetId(TEntity entity);

		/// <summary>
		/// Inserts a new entity.
		/// It may require to save current unit of work to be able to retrieve id.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The primary key of the inserted <see cref="TEntity"/>.</returns>
		Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		TEntity InsertOrUpdate(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		Task<TEntity> InsertOrUpdateAsync(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// It may require to save current unit of work to be able to retrieve id.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The primary key of the <see cref="TEntity"/>.</returns>
		TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// It may require to save current unit of work to be able to retrieve id.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The primary key of the <see cref="TEntity"/>.</returns>
		Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

		#endregion

		#region Update

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		TEntity Update(TEntity entity);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="entity">The <see cref="TEntity"/>.</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		Task<TEntity> UpdateAsync(TEntity entity);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="id">Id of the entity</param>
		/// <param name="updateAction">The <see cref="Action{TEntity}"/> that can be used to change values of the entity</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="id">Id of the entity</param>
		/// <param name="updateAction">The <see cref="Action{TEntity}"/> that can be used to change values of the entity</param>
		/// <returns>The <see cref="TEntity"/>.</returns>
		Task<TEntity>  UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

		#endregion

		#region Delete

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">Entity to be deleted</param>
		void Delete(TEntity entity);

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">Entity to be deleted</param>
		Task DeleteAsync(TEntity entity);

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="id">Primary key of the entity</param>
		void Delete(TPrimaryKey id);

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="id">Primary key of the entity</param>
		Task DeleteAsync(TPrimaryKey id);

		/// <summary>
		/// Deletes many entities by function.
		/// Notice that: All entities fits to given predicate are retrieved and deleted.
		/// This may cause major performance problems if there are too many entities with
		/// given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		void Delete(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Deletes many entities by function.
		/// Notice that: All entities fits to given predicate are retrieved and deleted.
		/// This may cause major performance problems if there are too many entities with
		/// given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

		#endregion

		#region Aggregates

		/// <summary>
		/// Gets count of all entities in this repository.
		/// </summary>
		/// <returns>Count of entities</returns>
		int Count();

		/// <summary>
		/// Gets count of all entities in this repository.
		/// </summary>
		/// <returns>Count of entities</returns>
		Task<int> CountAsync();

		/// <summary>
		/// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
		/// </summary>
		/// <param name="predicate">A method to filter count</param>
		/// <returns>Count of entities</returns>
		int Count(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
		/// </summary>
		/// <param name="predicate">A method to filter count</param>
		/// <returns>Count of entities</returns>
		Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets count of all entities in this repository (use if expected return value is greater than <see cref="int.MaxValue"/>.
		/// </summary>
		/// <returns>Count of entities</returns>
		long LongCount();

		/// <summary>
		/// Gets count of all entities in this repository (use if expected return value is greater than <see cref="int.MaxValue"/>.
		/// </summary>
		/// <returns>Count of entities</returns>
		Task<long> LongCountAsync();

		/// <summary>
		/// Gets count of all entities in this repository (use if expected return value is greater than <see cref="int.MaxValue"/>.
		/// </summary>
		/// <returns>Count of entities</returns>
		long LongCount(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets count of all entities in this repository based on given <paramref name="predicate"/>
		/// (use this overload if expected return value is greater than <see cref="int.MaxValue"/>).
		/// </summary>
		/// <param name="predicate">A method to filter count</param>
		/// <returns>Count of entities</returns>
		Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Determines whether a sequence contains any elements.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>True or false</returns>
		bool Any(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Determines whether a sequence contains any elements.
		/// </summary>
		/// <param name="predicate">A condition to filter entities.</param>
		/// <returns>True or false</returns>
		Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

		#endregion
	}
}
