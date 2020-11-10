using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Entities.Auditing;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.Extensions;
using EasyNet.Linq;
using EasyNet.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EasyNet.EntityFrameworkCore
{
    public class EasyNetDbContext : DbContext
    {
        protected virtual bool IsSoftDeleteFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(EasyNetDataFilters.SoftDelete) == true;

        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(EasyNetDbContext).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

        public EasyNetDbContext(DbContextOptions options, ICurrentUnitOfWorkProvider currentUnitOfWorkProvider = null, IEasyNetSession session = null) : base(options)
        {
            CurrentUnitOfWorkProvider = currentUnitOfWorkProvider;
            Session = session ?? NullEasyNetSession.Instance;
        }

        protected ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; }

        protected IEasyNetSession Session { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });

                //ConfigureGlobalValueConverterMethodInfo
                //    .MakeGenericMethod(entityType.ClrType)
                //    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType == null && ShouldFilterEntity<TEntity>(entityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                if (IsSoftDeleteFilterEnabled)
                {
                    Expression<Func<TEntity, bool>> softDeleteFilter = e => ((ISoftDelete)e).IsDeleted == false;
                    expression = softDeleteFilter;
                }
            }

            //if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
            //{
            //    Expression<Func<TEntity, bool>> mayHaveTenantFilter = e => !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId;
            //    expression = expression == null ? mayHaveTenantFilter : CombineExpressions(expression, mayHaveTenantFilter);
            //}

            //if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
            //{
            //    Expression<Func<TEntity, bool>> mustHaveTenantFilter = e => !IsMustHaveTenantFilterEnabled || ((IMustHaveTenant)e).TenantId == CurrentTenantId;
            //    expression = expression == null ? mustHaveTenantFilter : CombineExpressions(expression, mustHaveTenantFilter);
            //}

            return expression;
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(IMayHaveTenant<>).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(IMustHaveTenant<>).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return false;
        }

        public override int SaveChanges()
        {
            ApplyEasyNetConcepts();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyEasyNetConcepts();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ApplyEasyNetConcepts()
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if (entry.State != EntityState.Modified && entry.CheckOwnedEntityChange())
                {
                    Entry(entry.Entity).State = EntityState.Modified;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        ApplyAbpConceptsForAddedEntity(entry);
                        break;
                    case EntityState.Modified:
                        ApplyAbpConceptsForModifiedEntity(entry);
                        break;
                    case EntityState.Deleted:
                        ApplyAbpConceptsForDeletedEntity(entry);
                        break;
                }
            }
        }

        protected virtual void ApplyAbpConceptsForAddedEntity(EntityEntry entry)
        {
            EntityAuditingHelper.SetCreationAuditProperties(entry.Entity, Session.UserId);
        }

        protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry)
        {
            EntityAuditingHelper.SetModificationAuditProperties(entry.Entity, Session.UserId);
        }

        protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry)
        {
            if (entry.Entity is ISoftDelete)
            {
                entry.State = EntityState.Modified;
                EntityAuditingHelper.SetDeletionAuditProperties(entry.Entity, Session.UserId);
            }
        }

        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            return ExpressionCombiner.Combine(expression1, expression2);
        }
    }
}
