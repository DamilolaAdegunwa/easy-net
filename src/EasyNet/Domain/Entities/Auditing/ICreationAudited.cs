namespace EasyNet.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities that is wanted to store creation information (who and when created).
    /// Creation time and creator user are automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface ICreationAudited : ICreationAudited<int>
    {
    }

	/// <summary>
	/// This interface is implemented by entities that is wanted to store creation information (who and when created).
	/// Creation time and creator user are automatically set when saving <see cref="Entity"/> to database.
	/// </summary>
	public interface ICreationAudited<TUserPrimaryKey> : IHasCreationTime
		where TUserPrimaryKey : struct
	{
		/// <summary>
		/// Id of the creator user of this entity.
		/// </summary>
		TUserPrimaryKey? CreatorUserId { get; set; }
	}
}
