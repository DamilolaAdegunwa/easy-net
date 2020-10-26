using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.EntityFrameworkCore.Uow;
using EasyNet.Runtime.Session;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Repositories
{
	public class EfCoreRepositoryBase<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, int>, IRepository<TEntity>
		 where TEntity : class, IEntity<int>
		 where TDbContext : DbContext
	{
		public EfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
			: base(dbContextProvider)
		{
		}
	}
}
