using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;

namespace EasyNet.Dapper
{
    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this repository works on</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public interface IDapperRepository<TEntity, in TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select/Get/Query

        /// <summary>
        /// 获取所有的实体对象
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="buffered">是否在内存中缓存执行结果，默认为True</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAllList(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取所有的实体对象
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllListAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取单个实体对象
        /// 如果没有找到对象或者找到多个对象将会报错
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        TEntity Single(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取单个实体对象
        /// 如果没有找到对象或者找到多个对象将会报错
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        Task<TEntity> SingleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取第一个实体对象
        /// 如果没有找到对象将会报错
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        TEntity First(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取第一个实体对象
        /// 如果没有找到对象将会报错
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        Task<TEntity> FirstAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取单个实体对象，如果没有找到对象则返回Null
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        TEntity SingleOrDefault(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取单个实体对象，如果没有找到对象则返回Null
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        Task<TEntity> SingleOrDefaultAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取第一个实体对象，如果没有找到对象则返回Null
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        TEntity FirstOrDefault(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 获取第一个实体对象，如果没有找到对象则返回Null
        /// </summary>
        /// <param name="sql">执行查询的Sql语句</param>
        /// <param name="param">查询时使用的条件参数</param>
        /// <param name="commandTimeout">命令执行超时时间（秒）</param>
        /// <param name="commandType">执行的命令类型</param>
        Task<TEntity> FirstOrDefaultAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        #endregion
    }
}
