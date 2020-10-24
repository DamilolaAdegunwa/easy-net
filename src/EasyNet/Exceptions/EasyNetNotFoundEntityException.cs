using EasyNet.Domain.Entities;

namespace EasyNet
{
	public class EasyNetNotFoundEntityException<TEntity, TPrimaryKey> : EasyNetException
	where TEntity : IEntity<TPrimaryKey>
	{
		public EasyNetNotFoundEntityException(object primaryKey) :
			base($"Cannot found {typeof(TEntity).AssemblyQualifiedName} by primary key {primaryKey}.")
		{
		}
	}
}
