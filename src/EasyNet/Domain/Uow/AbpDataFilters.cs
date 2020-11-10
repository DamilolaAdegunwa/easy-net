namespace EasyNet.Domain.Uow
{
    /// <summary>
    /// Standard filters of ABP.
    /// </summary>
    public static class EasyNetDataFilters
    {
        /// <summary>
        /// "SoftDelete".
        /// Soft delete filter.
        /// Prevents getting deleted data from database.
        /// See <see cref="Entities.ISoftDelete"/> interface.
        /// </summary>
        public const string SoftDelete = "SoftDelete";

        /// <summary>
        /// "MustHaveTenant".
        /// Tenant filter to prevent getting data that is
        /// not belong to current tenant.
        /// </summary>
        public const string MustHaveTenant = "MustHaveTenant";

        /// <summary>
        /// "MayHaveTenant".
        /// Tenant filter to prevent getting data that is
        /// not belong to current tenant.
        /// </summary>
        public const string MayHaveTenant = "MayHaveTenant";

        /// <summary>
        /// Standard parameters of EasyNet.
        /// </summary>
        public static class Parameters
        {
            /// <summary>
            /// "tenantId".
            /// </summary>
            public const string TenantId = "tenantId";

            /// <summary>
            /// "isDeleted".
            /// </summary>
            public const string IsDeleted = "isDeleted";
        }
    }
}
