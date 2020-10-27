namespace EasyNet.Domain.Entities.Auditing
{
    public interface IModificationAudited : IHasModificationTime
    {
    }

    /// <summary>
    /// This interface is implemented by entities that is wanted to store modification information (who and when modified lastly).
    /// Properties are automatically set when updating the <see cref="IEntity"/>.
    /// </summary>
    /// <typeparam name="TUserPrimaryKey">Type of the primary key of the user</typeparam>
    public interface IModificationAudited<TUserPrimaryKey> : IModificationAudited
        where TUserPrimaryKey : struct
    {
        /// <summary>
        /// Id of the creator user of this entity.
        /// </summary>
        TUserPrimaryKey? LastModifierUserId { get; set; }
    }
}
