using System;
using EasyNet.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.Identity.EntityFrameworkCore.UI.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up EasyNet.Identity services in an <see cref="IEasyNetBuilder" />.
    /// </summary>
    public static class EasyNetBuilderExtensions
    {
        public static IEasyNetBuilder ConfigureIdentityUiOptions( this IEasyNetBuilder builder, Action<EasyNetIdentityDefaultUiOptions> setupAction)
        {
            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}
