using EasyNet.Runtime.Initialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.DependencyInjection
{
    public static class EasyNetApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEasyNet(this IApplicationBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Init EasyNet
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IEasyNetInitializer>().Init();
            }

            return builder;
        }
    }
}