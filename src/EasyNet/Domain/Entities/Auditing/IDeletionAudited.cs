namespace EasyNet.Domain.Entities.Auditing
{
    public interface IDeletionAudited : IHasDeletionTime
    {
    }

	/// <summary>
	/// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
	/// </summary>
	/// <typeparam name="TUserPrimaryKey">Type of the primary key of the user</typeparam>
	public interface IDeletionAudited<TUserPrimaryKey> : IDeletionAudited
		where TUserPrimaryKey : struct
	{

		/// <summary>
		/// Reference to the deleter user of this entity.
		/// </summary>
		TUserPrimaryKey? DeleterUserId { get; set; }
	}
}
