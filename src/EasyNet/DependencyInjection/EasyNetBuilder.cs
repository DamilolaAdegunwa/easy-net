using System;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.DependencyInjection
{
    /// <summary>
    /// Allows fine grained configuration of EasyNet services.
    /// </summary>
    internal class EasyNetBuilder : IEasyNetBuilder
    {
        public EasyNetBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}
