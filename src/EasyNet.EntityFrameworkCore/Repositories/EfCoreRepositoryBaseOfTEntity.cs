using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.EntityFrameworkCore.Uow;

namespace EasyNet.EntityFrameworkCore.Repositories
{
	public class EfCoreRepositoryBase<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, int>, IRepository<TEntity>
		 where TEntity : class, IEntity<int>
		 where TDbContext : EasyNetDbContext
	{
		public EfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
			: base(dbContextProvider)
		{
		}
	}
}
