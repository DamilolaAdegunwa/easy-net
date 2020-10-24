using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.DependencyInjection
{
    /// <summary>
    /// An interface for configuring EasyNet services.
    /// </summary>
    public interface IEasyNetBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where MVC services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
