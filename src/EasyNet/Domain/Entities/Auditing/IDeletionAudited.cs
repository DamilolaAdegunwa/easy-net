namespace EasyNet.Domain.Entities.Auditing
{
	/// <summary>
	/// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
	/// </summary>
	public interface IDeletionAudited : IDeletionAudited<int>
	{
	}

	/// <summary>
	/// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
	/// </summary>
	/// <typeparam name="TUserPrimaryKey">Type of the primary key of the user</typeparam>
	public interface IDeletionAudited<TUserPrimaryKey> : IHasDeletionTime
		where TUserPrimaryKey : struct
	{

		/// <summary>
		/// Reference to the deleter user of this entity.
		/// </summary>
		TUserPrimaryKey? DeleterUserId { get; set; }
	}
}
