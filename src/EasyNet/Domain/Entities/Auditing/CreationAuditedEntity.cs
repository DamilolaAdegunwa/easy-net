using System;
using EasyNet.Timing;

namespace EasyNet.Domain.Entities.Auditing
{
	/// <summary>
	/// This class can be used to simplify implementing <see cref="ICreationAudited"/>.
	/// </summary>
	public abstract class CreationAuditedEntity : CreationAuditedEntity<int, int>
	{
	}

	/// <summary>
	/// This class can be used to simplify implementing <see cref="ICreationAudited"/>.
	/// </summary>
	/// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
	public abstract class CreationAuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey, int>
	{
	}

	/// <summary>
	/// This class can be used to simplify implementing <see cref="ICreationAudited"/>.
	/// </summary>
	/// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
	/// <typeparam name="TUserPrimaryKey">Type of the primary key of the user</typeparam>
	public abstract class CreationAuditedEntity<TPrimaryKey, TUserPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited<TUserPrimaryKey>
		where TUserPrimaryKey : struct
	{
		/// <inheritdoc/>

		public virtual DateTime CreationTime { get; set; }

		/// <inheritdoc/>

		public virtual TUserPrimaryKey? CreatorUserId { get; set; }
	}
}
