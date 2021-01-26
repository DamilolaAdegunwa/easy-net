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
using EasyNet.Extensions;
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

        protected virtual bool IsMayHaveTenantFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(EasyNetDataFilters.MayHaveTenant) == true;

        protected virtual bool IsMustHaveTenantFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(EasyNetDataFilters.MustHaveTenant) == true;


        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(EasyNetDbContext).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

        public EasyNetDbContext(DbContextOptions options, ICurrentUnitOfWorkProvider currentUnitOfWorkProvider, IEasyNetSession session) : base(options)
        {
            CurrentUnitOfWorkProvider = currentUnitOfWorkProvider;
            EasyNetSession = session ?? NullEasyNetSession.Instance;
        }

        protected ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; }

        protected IEasyNetSession EasyNetSession { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType == null)
            {
                Expression<Func<TEntity, bool>> expression = null;

                if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                {
                    Expression<Func<TEntity, bool>> softDeleteFilter = e => !IsSoftDeleteFilterEnabled || !((ISoftDelete)e).IsDeleted;
                    expression = softDeleteFilter;
                }

                if (IsMustHaveTenantFilterEnabled)
                {
                    if (typeof(TEntity).HasImplementedRawGeneric(typeof(IMustHaveTenant<>)))
                    {
                        //Expression<Func<TEntity, bool>> mustHaveTenantFilter = e => ((IMustHaveTenant)e).TenantId == CurrentTenantId;
                        //expression = expression == null ? mustHaveTenantFilter : CombineExpressions(expression, mustHaveTenantFilter);
                    }
                }
                else if (IsMayHaveTenantFilterEnabled)
                {
                    if (typeof(TEntity).HasImplementedRawGeneric(typeof(IMayHaveTenant<>)))
                    {
                        //Expression<Func<TEntity, bool>> mayHaveTenantFilter = e => ((IMayHaveTenant)e).TenantId == CurrentTenantId;
                        //expression = expression == null ? mayHaveTenantFilter : CombineExpressions(expression, mayHaveTenantFilter);
                    }
                }

                if (expression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(expression);
                }
            }
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
            EntityAuditingHelper.SetCreationAuditProperties(entry.Entity, EasyNetSession.UserId);
        }

        protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry)
        {
            EntityAuditingHelper.SetModificationAuditProperties(entry.Entity, EasyNetSession.UserId);
        }

        protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry)
        {
            if (entry.Entity is ISoftDelete)
            {
                entry.State = EntityState.Modified;
                EntityAuditingHelper.SetDeletionAuditProperties(entry.Entity, EasyNetSession.UserId);
            }
        }

        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            return ExpressionCombiner.Combine(expression1, expression2);
        }
    }
}
