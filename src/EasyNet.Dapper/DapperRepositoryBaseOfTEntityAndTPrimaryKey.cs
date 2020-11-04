using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EasyNet.Data;
using EasyNet.Domain.Entities;

namespace EasyNet.Dapper
{
    /// <summary>
    /// 实现接口 <see cref="IDapperRepository{TEntity,TPrimaryKey}" />.
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class DapperRepositoryBase<TEntity, TPrimaryKey> : IDapperRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public DapperRepositoryBase(IActiveTransactionProvider activeTransactionProvider)
        {
            Connection = activeTransactionProvider.GetActiveConnection();
            Transaction = activeTransactionProvider.GetActiveTransaction();
        }

        protected IDbConnection Connection { get; }

        protected IDbTransaction Transaction { get; }

        protected int? DefaultCommandTimeout { get; }

        #region Select/Get/Query

        public IEnumerable<TEntity> GetAllList(
           string sql,
           object param = null,
           bool buffered = true,
           int? commandTimeout = null,
           CommandType? commandType = null)
        {
            return Connection.Query<TEntity>(sql, param, Transaction, buffered, PatchCommandTimeout(commandTimeout), commandType);
        }

        public Task<IEnumerable<TEntity>> GetAllListAsync(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QueryAsync<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        public TEntity Single(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QuerySingle<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        public Task<TEntity> SingleAsync(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QuerySingleAsync<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        public TEntity First(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QueryFirst<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        public Task<TEntity> FirstAsync(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QueryFirstAsync<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        public TEntity SingleOrDefault(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QuerySingleOrDefault<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        public Task<TEntity> SingleOrDefaultAsync(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QuerySingleOrDefaultAsync<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        public TEntity FirstOrDefault(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QueryFirstOrDefault<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        public Task<TEntity> FirstOrDefaultAsync(
            string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Connection.QueryFirstOrDefaultAsync<TEntity>(sql, param, Transaction, PatchCommandTimeout(commandTimeout), commandType);
        }

        #endregion

        #region Helper

        private int? PatchCommandTimeout(int? commandTimeout)
        {
            return commandTimeout ?? DefaultCommandTimeout;
        }

        #endregion
    }
}
