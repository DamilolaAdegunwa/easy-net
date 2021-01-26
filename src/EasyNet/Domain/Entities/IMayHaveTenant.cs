namespace EasyNet.Domain.Entities
{
    /// <summary>
    /// Implement this interface for an entity which may have TenantId.
    /// </summary>
    public interface IMayHaveTenant : IMayHaveTenant<int>
    {
    }

    /// <summary>
    /// Implement this interface for an entity which may have TenantId.
    /// </summary>
    public interface IMayHaveTenant<TTenantKey> where TTenantKey : struct
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        TTenantKey? TenantId { get; set; }
    }
}
