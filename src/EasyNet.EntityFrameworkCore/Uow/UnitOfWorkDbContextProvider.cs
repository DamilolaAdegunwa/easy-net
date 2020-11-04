using System.Threading.Tasks;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Implements <see cref="IDbContextProvider{TDbContext}"/> that gets DbContext from active unit of work.
    /// </summary>
    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public UnitOfWorkDbContextProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public TDbContext GetDbContext()
        {
            return _currentUnitOfWorkProvider.Current.GetDbContext<TDbContext>();
        }

        public Task<TDbContext> GetDbContextAsync()
        {
            return _currentUnitOfWorkProvider.Current.GetDbContextAsync<TDbContext>();
        }
    }
}
