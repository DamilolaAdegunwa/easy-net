namespace EasyNet.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which must be audited.
    /// Related properties automatically set when saving/updating <see cref="Entity"/> objects.
    /// </summary>
    public interface IAudited : IAudited<int>
    {

    }

    /// <summary>
    /// This interface is implemented by entities which must be audited.
    /// Related properties automatically set when saving/updating <see cref="Entity"/> objects.
    /// </summary>
    /// <typeparam name="TUserPrimaryKey">Type of the primary key of the user</typeparam>
    public interface IAudited<TUserPrimaryKey> : ICreationAudited<TUserPrimaryKey>, IModificationAudited<TUserPrimaryKey>
        where TUserPrimaryKey : struct
    {

    }
}
