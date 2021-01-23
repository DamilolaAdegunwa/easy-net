using System.Data;

namespace EasyNet.Domain.Uow
{
    /// <summary>
    /// An interface for getting active connection and transaction.
    /// </summary>
    public interface IActiveTransactionProvider
    {
        /// <summary>
        /// Gets the active transaction or null if current UOW is not transactional.
        /// </summary>
        /// <returns>An <see cref="IDbTransaction"/>.</returns>
        IDbTransaction GetActiveTransaction();

        /// <summary>
        /// Gets the active database connection.
        /// </summary>
        /// <returns>An <see cref="IDbConnection"/>.</returns>
        IDbConnection GetActiveConnection();
    }
}
