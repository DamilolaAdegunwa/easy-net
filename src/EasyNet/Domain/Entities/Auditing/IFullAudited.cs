namespace EasyNet.Domain.Entities.Auditing
{
	/// <summary>
	/// This interface ads <see cref="IDeletionAudited"/> to <see cref="IAudited"/> for a fully audited entity.
	/// </summary>
	public interface IFullAudited : IFullAudited<int>
	{

	}

	/// <summary>
	/// This interface ads <see cref="IDeletionAudited"/> to <see cref="IAudited"/> for a fully audited entity.
	/// </summary>
	/// <typeparam name="TUserPrimaryKey">Type of the primary key of the user</typeparam>
	public interface IFullAudited<TUserPrimaryKey> : IAudited<TUserPrimaryKey>, IDeletionAudited<TUserPrimaryKey>
		where TUserPrimaryKey : struct
	{

	}
}
