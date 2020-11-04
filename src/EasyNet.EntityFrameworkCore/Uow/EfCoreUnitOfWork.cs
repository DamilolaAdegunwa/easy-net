using System;
using System.Threading.Tasks;
using EasyNet.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EasyNet.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// </summary>
    public class EfCoreUnitOfWork : DatabaseUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        protected DbContext ActiveDbContext { get; private set; }

        public EfCoreUnitOfWork(IServiceProvider serviceProvider, IDbConnectionBuilder connectionBuilder, IOptions<UnitOfWorkDefaultOptions> defaultOptions)
            : base(connectionBuilder, defaultOptions)
        {
            _serviceProvider = serviceProvider;
        }

        public override void SaveChanges()
        {
            ActiveDbContext?.SaveChanges();
        }

        public override Task SaveChangesAsync()
        {
            return ActiveDbContext?.SaveChangesAsync();
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            if (ActiveDbContext == null)
            {
                ActiveDbContext = _serviceProvider.GetRequiredService<TDbContext>();

                if (ActiveTransaction != null)
                {
                    ActiveDbContext.Database.UseTransaction(ActiveTransaction);
                }
            }

            return (TDbContext)ActiveDbContext;
        }

#if NetCore31

        public virtual async Task<TDbContext> GetOrCreateDbContextAsync<TDbContext>()
            where TDbContext : DbContext
        {
            if (ActiveDbContext == null)
            {
                ActiveDbContext = _serviceProvider.GetRequiredService<TDbContext>();

                if (ActiveTransaction != null)
                {

                    await ActiveDbContext.Database.UseTransactionAsync(ActiveTransaction);
                }
            }

            return (TDbContext)ActiveDbContext;
        }
#else

        public virtual Task<TDbContext> GetOrCreateDbContextAsync<TDbContext>()
           where TDbContext : DbContext
        {
            if (ActiveDbContext == null)
            {
                ActiveDbContext = _serviceProvider.GetRequiredService<TDbContext>();

                if (ActiveTransaction != null)
                {

                    ActiveDbContext.Database.UseTransaction(ActiveTransaction);
                }
            }

            return Task.FromResult((TDbContext)ActiveDbContext);
        }
#endif


        protected override void DisposeUow()
        {
            ActiveTransaction?.Dispose();
            ActiveDbContext?.Dispose();
        }
    }
}
