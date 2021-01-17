using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Implements IRepository for Entity Framework.
    /// </summary>
    /// <typeparam name="TDbContext">DbContext which contains <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public class EfCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey> : IEfCoreRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : EasyNetDbContext
    {
        protected TDbContext DbContext { get; }

        protected virtual DbSet<TEntity> Table => DbContext.Set<TEntity>();

        public EfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        {
            DbContext = dbContextProvider.GetDbContext();
        }

        /// <inheritdoc/>
        public DbContext GetDbContext()
        {
            return DbContext;
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable();
        }

        #region Select/Get/Query

        /// <inheritdoc/>
        public virtual List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        /// <inheritdoc/>
        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return GetAll().ToListAsync();
        }

        /// <inheritdoc/>
        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        /// <inheritdoc/>
        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToListAsync();
        }

        /// <inheritdoc/>
        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = SingleOrDefault(CreateEqualityExpressionForId(id));

            if (entity == null) throw new EasyNetNotFoundEntityException<TEntity, TPrimaryKey>(id);

            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await SingleOrDefaultAsync(CreateEqualityExpressionForId(id));

            if (entity == null) throw new EasyNetNotFoundEntityException<TEntity, TPrimaryKey>(id);

            return entity;
        }

        /// <inheritdoc/>
        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleAsync(predicate);
        }

        /// <inheritdoc/>
        public TEntity First()
        {
            return GetAll().First();
        }

        /// <inheritdoc/>
        public Task<TEntity> FirstAsync()
        {
            return GetAll().FirstAsync();
        }

        /// <inheritdoc/>
        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().First(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstAsync(predicate);
        }

        /// <inheritdoc/>
        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleOrDefault(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleOrDefaultAsync(predicate);
        }

        /// <inheritdoc/>
        public virtual TEntity FirstOrDefault()
        {
            return GetAll().FirstOrDefault();
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> FirstOrDefaultAsync()
        {
            return GetAll().FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region Insert

        /// <inheritdoc/>
        public virtual TEntity Insert(TEntity entity)
        {
            Table.Add(entity);
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
            return entity;
        }

        /// <inheritdoc/>
        public virtual TPrimaryKey InsertAndGetId(TEntity entity)
        {
            Insert(entity);

            if (MayHaveTemporaryKey(entity))
            {
                DbContext.SaveChanges();
            }

            return entity.Id;
        }

        /// <inheritdoc/>
        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            await InsertAsync(entity);

            if (MayHaveTemporaryKey(entity))
            {
                await DbContext.SaveChangesAsync();
            }

            return entity.Id;
        }

        /// <inheritdoc/>
        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            if (MayHaveTemporaryKey(entity))
            {
                Insert(entity);
            }
            else
            {
                Update(entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            if (MayHaveTemporaryKey(entity))
            {
                await InsertAsync(entity);
            }
            else
            {
                await UpdateAsync(entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (MayHaveTemporaryKey(entity))
            {
                DbContext.SaveChanges();
            }

            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (MayHaveTemporaryKey(entity))
            {
                await DbContext.SaveChangesAsync();
            }

            return entity.Id;
        }

        #endregion

        #region Update

        /// <inheritdoc/>
        public virtual TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity = Update(entity);
            return Task.FromResult(entity);
        }

        /// <inheritdoc/>
        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);

            if (DbContext.ChangeTracker.QueryTrackingBehavior == QueryTrackingBehavior.NoTracking)
            {
                AttachIfNot(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);

            if (DbContext.ChangeTracker.QueryTrackingBehavior == QueryTrackingBehavior.NoTracking)
            {
                AttachIfNot(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            return entity;
        }

        #endregion

        #region Delete

        /// <inheritdoc/>
        public virtual void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        /// <inheritdoc/>
        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = SingleOrDefault(CreateEqualityExpressionForId(id));
            if (entity != null)
            {
                Delete(entity);
            }

            // Don't do anything if no entity can be found.
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = await SingleOrDefaultAsync(CreateEqualityExpressionForId(id));
            if (entity != null)
            {
                Delete(entity);
            }

            // Don't do anything if no entity can be found.
        }

        /// <inheritdoc/>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = GetAllList(predicate);

            foreach (var entity in entities)
            {
                Table.Remove(entity);
            }
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetAllListAsync(predicate);

            foreach (var entity in entities)
            {
                Table.Remove(entity);
            }
        }

        #endregion

        #region Aggregates

        /// <inheritdoc/>
        public virtual int Count()
        {
            return GetAll().Count();
        }

        /// <inheritdoc/>
        public virtual Task<int> CountAsync()
        {
            return GetAll().CountAsync();
        }

        /// <inheritdoc/>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Count(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().CountAsync(predicate);
        }

        /// <inheritdoc/>
        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        /// <inheritdoc/>
        public virtual Task<long> LongCountAsync()
        {
            return GetAll().LongCountAsync();
        }

        /// <inheritdoc/>
        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCountAsync(predicate);
        }

        /// <inheritdoc/>
        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Any(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().AnyAsync(predicate);
        }

        #endregion

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = DbContext.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        protected virtual TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = DbContext.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

            var idValue = Convert.ChangeType(id, typeof(TPrimaryKey));

            Expression<Func<object>> closure = () => idValue;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        protected virtual bool MayHaveTemporaryKey(TEntity entity)
        {
            if (typeof(TPrimaryKey) == typeof(byte))
            {
                return true;
            }

            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(entity.Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(entity.Id) <= 0;
            }

            return false;
        }
    }
}
