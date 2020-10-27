using System;

namespace EasyNet.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited{TUserPrimaryKey}"/>.
    /// </summary>
    public abstract class AuditedEntity : AuditedEntity<int, int>
    {

    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited{TUserPrimaryKey}"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public abstract class AuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey, int>
    {
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited{TUserPrimaryKey}"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserPrimaryKey">Type of the user</typeparam>
    public abstract class AuditedEntity<TPrimaryKey, TUserPrimaryKey> : CreationAuditedEntity<TPrimaryKey, TUserPrimaryKey>, IAudited<TUserPrimaryKey>
        where TUserPrimaryKey : struct
    {
	    /// <inheritdoc/>

        public virtual DateTime? LastModificationTime { get; set; }

	    /// <inheritdoc/>

        public virtual TUserPrimaryKey? LastModifierUserId { get; set; }
    }
}
