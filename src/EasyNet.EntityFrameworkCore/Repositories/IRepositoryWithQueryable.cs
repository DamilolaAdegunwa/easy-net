using System.Linq;
using EasyNet.Domain.Entities;

namespace EasyNet.EntityFrameworkCore.Repositories
{
	public interface IRepositoryWithQueryable<out TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
	{
		/// <summary>
		/// Gets <see cref="IQueryable{TEntity}"/>.
		/// </summary>
		/// <returns></returns>
		IQueryable<TEntity> GetQueryable();
	}
}
