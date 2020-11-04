using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using EasyNet.Extensions;
using Microsoft.Extensions.Options;

namespace EasyNet.Domain.Uow
{
    /// <summary>
    /// Allow to use more than one orm technology to access database.
    /// Use external database transaction to share transaction.
    /// See more refer to https://docs.microsoft.com/zh-cn/ef/core/saving/transactions.
    /// </summary>
    public class DatabaseUnitOfWork : UnitOfWorkBase
    {
        private readonly IDbConnectionBuilder _connectionBuilder;

        protected DbConnection ActiveConnection { get; set; }

        protected DbTransaction ActiveTransaction { get; set; }

        public DatabaseUnitOfWork(IDbConnectionBuilder connectionBuilder, IOptions<UnitOfWorkDefaultOptions> defaultOptions) : base(defaultOptions)
        {
            _connectionBuilder = connectionBuilder;
        }

        public override void SaveChanges()
        {
            // no action
        }

        public override Task SaveChangesAsync()
        {
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
#if NetCore31
            return ActiveTransaction?.CommitAsync();
#else
            ActiveTransaction?.Commit();
            return Task.CompletedTask;
#endif
        }

        protected override void DisposeUow()
        {
            // Dispose transaction
            ActiveTransaction?.Dispose();

            // Close && Dispose connection
            if (ActiveConnection?.State != ConnectionState.Closed)
            {
                ActiveConnection?.Close();
            }
            ActiveConnection?.Dispose();
        }

        public virtual DbConnection GetOrCreateConnection()
        {
            if (ActiveConnection == null)
            {
                ActiveConnection = _connectionBuilder.CreateConnection();
                ActiveConnection.Open();

                if (Options.IsTransactional == true)
                {
                    ActiveTransaction = ActiveConnection.BeginTransaction((Options.IsolationLevel ?? System.Transactions.IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
                }
            }

            return ActiveConnection;
        }

        public virtual async Task<DbConnection> GetOrCreateConnectionAsync()
        {
            if (ActiveConnection == null)
            {
                ActiveConnection = _connectionBuilder.CreateConnection();
                await ActiveConnection.OpenAsync();

                if (Options.IsTransactional == true)
                {
#if NetCore31
                    ActiveTransaction = await ActiveConnection.BeginTransactionAsync((Options.IsolationLevel ?? System.Transactions.IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
#else
                    ActiveTransaction = ActiveConnection.BeginTransaction((Options.IsolationLevel ?? System.Transactions.IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
#endif
                }
            }

            return ActiveConnection;
        }
    }
}
