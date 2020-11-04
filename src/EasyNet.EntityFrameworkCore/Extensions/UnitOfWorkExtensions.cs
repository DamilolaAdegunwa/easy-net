using System;
using System.Threading.Tasks;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Extensions
{
    /// <summary>
    /// Extension methods for UnitOfWork.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Gets a DbContext as a part of active unit of work.
        /// This method can be called when current unit of work is an <see cref="EfCoreUnitOfWork"/>.
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
        /// <param name="unitOfWork">Current (active) unit of work</param>
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork)
            where TDbContext : DbContext
        {
            Check.NotNull(unitOfWork, nameof(unitOfWork));

            if (unitOfWork is EfCoreUnitOfWork efCoreUnitOfWork)
            {
                return efCoreUnitOfWork.GetOrCreateDbContext<TDbContext>();
            }

            throw new ArgumentException("unitOfWork is not type of " + typeof(EfCoreUnitOfWork).FullName, nameof(unitOfWork));
        }

        /// <summary>
        /// Gets a DbContext as a part of active unit of work.
        /// This method can be called when current unit of work is an <see cref="EfCoreUnitOfWork"/>.
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
        /// <param name="unitOfWork">Current (active) unit of work</param>
        public static Task<TDbContext> GetDbContextAsync<TDbContext>(this IActiveUnitOfWork unitOfWork)
            where TDbContext : DbContext
        {
            Check.NotNull(unitOfWork, nameof(unitOfWork));

            if (unitOfWork is EfCoreUnitOfWork efCoreUnitOfWork)
            {
                return efCoreUnitOfWork.GetOrCreateDbContextAsync<TDbContext>();
            }

            throw new ArgumentException("unitOfWork is not type of " + typeof(EfCoreUnitOfWork).FullName, nameof(unitOfWork));
        }
    }
}
