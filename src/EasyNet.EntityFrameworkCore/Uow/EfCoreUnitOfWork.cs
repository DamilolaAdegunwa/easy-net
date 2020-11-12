using System;
using System.Threading.Tasks;
using EasyNet.Domain.Uow;
using EasyNet.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EasyNet.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// </summary>
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        protected IServiceProvider ServiceProvider { get; }

        protected DbContext ActiveDbContext { get; private set; }

        protected IDbContextTransaction ActiveTransaction { get; private set; }

        public EfCoreUnitOfWork(IServiceProvider serviceProvider, IOptions<UnitOfWorkDefaultOptions> defaultOptions)
            : base(defaultOptions)
        {
            ServiceProvider = serviceProvider;
        }

        public override void SaveChanges()
        {
            ActiveDbContext?.SaveChanges();
        }

        public override Task SaveChangesAsync()
        {
            if (ActiveDbContext != null)
            {
                return ActiveDbContext.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }

        protected override void CompleteUow()
        {
            SaveChanges();
            CommitTransaction();
        }

        protected override async Task CompleteUowAsync()
        {
            await SaveChangesAsync();
            await CommitTransactionAsync();
        }

        protected virtual void CommitTransaction()
        {
            ActiveTransaction?.Commit();
        }

        protected virtual Task CommitTransactionAsync()
        {
#if Net461 || NetCore21 || NetCore22
            ActiveTransaction?.Commit();

            return Task.CompletedTask;
#else
            if (ActiveTransaction != null)
            {
                return ActiveTransaction.CommitAsync();
            }

            return Task.CompletedTask;
#endif
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            if (ActiveDbContext == null)
            {
                ActiveDbContext = ServiceProvider.GetRequiredService<TDbContext>();

                if (Options.IsTransactional == true)
                {
                    ActiveTransaction = ActiveDbContext.Database.BeginTransaction((Options.IsolationLevel ?? System.Transactions.IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
                }
            }

            return (TDbContext)ActiveDbContext;
        }

        protected override void DisposeUow()
        {
            ActiveTransaction?.Dispose();
            ActiveDbContext?.Dispose();
        }
    }
}
