using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyNet
{
    /// <summary>
    /// Provides programmatic configuration for the EasyNet framework.
    /// </summary>
    public class EasyNetOptions
    {
        /// <summary>
        /// Used to control whether to automatically start a new unit of work before <see cref="IAsyncActionFilter.OnActionExecutionAsync" />.
        /// Default is true.
        /// </summary>
        public bool UnitOfWorkAutoStart { get; set; } = true;
    }
}
