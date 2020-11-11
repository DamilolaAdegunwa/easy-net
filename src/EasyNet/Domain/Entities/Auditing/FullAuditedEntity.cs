using System;

namespace EasyNet.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="FullAuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public abstract class FullAuditedEntity : FullAuditedEntity<int, int>
    {

    }

    /// <summary>
    /// Implements <see cref="IFullAudited{TUserPrimaryKey}"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public abstract class FullAuditedEntity<TPrimaryKey> : FullAuditedEntity<TPrimaryKey, int>
    {
    }

    /// <summary>
    /// Implements <see cref="IFullAudited{TUserPrimaryKey}"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserPrimaryKey">Type of the primary key of the user</typeparam>
    public abstract class FullAuditedEntity<TPrimaryKey, TUserPrimaryKey> : AuditedEntity<TPrimaryKey, TUserPrimaryKey>, IFullAudited<TUserPrimaryKey>
        where TUserPrimaryKey : struct
    {
	    /// <inheritdoc/>

        public virtual bool IsDeleted { get; set; }

	    /// <inheritdoc/>

        public virtual TUserPrimaryKey? DeleterUserId { get; set; }

	    /// <inheritdoc/>

        public virtual DateTime? DeletionTime { get; set; }
    }
}
